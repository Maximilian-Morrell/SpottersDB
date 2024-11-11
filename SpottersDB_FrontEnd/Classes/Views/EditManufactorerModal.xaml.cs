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

    public EditManufactorerModal()
	{
		InitializeComponent();
        IsEditing = false;
        Submit.Clicked += Submit_Clicked;
    }

    private void Submit_Clicked(object sender, EventArgs e)
    {
        string Name = ManufactorerName.Text;
        int Region = Countries[RegionPicker.SelectedIndex].id;
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

    public EditManufactorerModal(Manufactorer man)
    {
        InitializeComponent();

        manufactorer = man;
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

        foreach (Country country in Countries)
        {
            regionNames.Add(country.name + " - " + country.id);
        }

        regionNames.Add("Create New");

        RegionPicker.ItemsSource = regionNames;

        RegionPicker.Title = "Select a Country";

        if (IsEditing)
        {
            int ID = Countries.FindIndex(c => c.id == manufactorer.region);
            RegionPicker.SelectedIndex = ID;
        }

        RegionPicker.SelectedIndexChanged += RegionPickerSelectionChange;

        GridMain.Add(RegionPicker, 1, 1);
    }

    private void RegionPickerSelectionChange(object sender, EventArgs e)
    {
        switch(RegionPicker.SelectedItem)
        {
            case "Create New":
                CreateNewRegion();
                break;
        }
    }

    private void CreateNewRegion()
    {
        EditCountryModal editCountryModal = new EditCountryModal(true);
        Navigation.PushAsync(editCountryModal);
    }
}