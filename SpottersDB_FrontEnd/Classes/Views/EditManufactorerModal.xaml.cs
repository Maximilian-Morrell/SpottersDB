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

    private void Submit_Clicked(object sender, EventArgs e)
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
        RegionPicker  = new Picker();
        List<string> regionNames = new List<string>();

        regionNames.Add("Create New");

        foreach (Country country in Countries)
        {
            regionNames.Add(country.name + " - " + country.id);
        }


        RegionPicker.ItemsSource = regionNames;

        RegionPicker.Title = "Select a Country";

        if (IsEditing)
        {
            int ID = Countries.FindIndex(c => c.id == manufactorer.region) + 1;
            RegionPicker.SelectedIndex = ID;
        }

        RegionPicker.SelectedIndexChanged += RegionPickerSelectionChange;
        IsLoaded = true;
        CheckIfAllValid();
        GridMain.Add(RegionPicker, 1, 1);
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