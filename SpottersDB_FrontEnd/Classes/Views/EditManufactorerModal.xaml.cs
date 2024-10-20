using Microsoft.Maui.Platform;
using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;
using System.Collections;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditManufactorerModal : ContentPage
{
    public List<Country> Countries = new List<Country>();
    bool IsEditing;
    int ID = -1;
    Manufactorer manufactorer;

    public EditManufactorerModal()
	{
		InitializeComponent();
        IsEditing = false;
	}

    public EditManufactorerModal(Manufactorer man)
    {
        InitializeComponent();

        manufactorer = man;
        IsEditing = true;
        ManufactorerName.Text = manufactorer.name;
        GetAllRegions();

    }

    public async void GetAllRegions()
    {
        List<Country> CountriesAll = await HTTP_Controller.GetCountries();
        Picker test = new Picker();
        List<string> regionNames = new List<string>();

        foreach (Country country in CountriesAll)
        {
            if(country.icaO_Code == "")
            {
                regionNames.Add(country.name);
                Countries.Add(country);
            }
        }

        test.ItemsSource = regionNames;


        if (IsEditing)
        {
            ID = Countries.FindIndex(c => c.id == manufactorer.region);
            test.SelectedIndex = ID;
        }

        test.SelectedIndexChanged += CountrySelectionChanged;

        GridMain.Add(test, 1, 1);
    }

    private void CountrySelectionChanged(object sender, EventArgs e)
    {
        Picker picker = sender as Picker;
        manufactorer.region = Countries[ID].id;
    }
}