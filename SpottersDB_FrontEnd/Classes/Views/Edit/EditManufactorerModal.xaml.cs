using Microsoft.Maui.Platform;
using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;
using System.Collections;
using System.Reflection;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditManufactorerModal : ContentPage
{
    public List<Country> Countries = new List<Country>();
    bool IsEditing;
    Manufactorer manufactorer;
    Picker RegionPicker = null;
    bool IsLoaded = false;

    public EditManufactorerModal()
	{
		InitializeComponent();
        IsEditing = false;
        Submit.IsEnabled = false;
        ManufactorerName.Text = "";
        Submit.Clicked += Submit_Clicked;
    }

    private async void Submit_Clicked(object sender, EventArgs e)
    {
        try
        {
            string Name = ManufactorerName.Text;
            int Region = Countries[RegionPicker.SelectedIndex -1 ].id;
            if (IsEditing)
            {
                Manufactorer newManufactorer = new Manufactorer(manufactorer.id, Name, Region);
                HTTP_Controller.UpdateManufactorer(newManufactorer);
            }
            else
            {
                Manufactorer newManufactorer = new Manufactorer(Name, Region);
                HTTP_Controller.AddNewManufactorer(newManufactorer);
            }

            Navigation.RemovePage(this);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Something has gone wrong with saving", ex.Message, "OK");
        }
    }

    public EditManufactorerModal(Manufactorer man)
    {
        InitializeComponent();

        manufactorer = man;
        ManufactorerName.Text = man.name;
        IsEditing = true;
        ManufactorerName.Text = manufactorer.name;
        Submit.Clicked += Submit_Clicked;
        Submit.IsEnabled = true;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        GetAllRegions();
        base.OnNavigatedTo(args);
    }

    public async void GetAllRegions()
    {
        if(RegionPicker != null)
        {
            GridMain.Children.Remove(RegionPicker);
        }

        Countries = await HTTP_Controller.GetRegions();
        List<string> regionNames = new List<string>();
        foreach (Country country in Countries)
        {
            regionNames.Add(country.name);
        }

        if(IsEditing)
        {
            int ID = Countries.FindIndex(c => c.id == manufactorer.region);
            RegionPicker = UI_Utilities.CreatePicker(GridMain, RegionPickerSelectionChange, 1, 1, regionNames, "Select a Region", ID);
        }
        else
        {
            RegionPicker = UI_Utilities.CreatePicker(GridMain, RegionPickerSelectionChange, 1, 1, regionNames, "Select a Region");
        }

        IsLoaded = true;
        CheckIfAllValid();
    }

    private void RegionPickerSelectionChange(object sender, EventArgs e)
    {
        switch(RegionPicker.SelectedItem)
        {
            case "Create New":
                CreateNewRegion();
                break;
            default:
                CheckIfAllValid();
                break;

        }
    }

    private void CheckIfAllValid()
    {
        if(IsLoaded)
        {
            Submit.IsEnabled = RegionPicker.SelectedIndex >= 1 && ManufactorerName.Text.Length >= 1;
        }
    }

    private void CreateNewRegion()
    {
        EditCountryModal editCountryModal = new EditCountryModal(true);
        Navigation.PushAsync(editCountryModal);
    }

    private void ManufactorerName_TextChanged(object sender, TextChangedEventArgs e)
    {
        CheckIfAllValid();
    }
}