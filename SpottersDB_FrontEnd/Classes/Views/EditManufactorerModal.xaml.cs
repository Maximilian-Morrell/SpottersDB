using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;
using System.Collections;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditManufactorerModal : ContentPage
{
    IList<Country> Countries = new List<Country>();
    bool IsEditing;

    public EditManufactorerModal()
	{
		InitializeComponent();
	}

    public EditManufactorerModal(Manufactorer manufactorer)
    {
        InitializeComponent();

        IsEditing = true;
        ManufactorerName.Text = manufactorer.name;
        GetAllRegions();
        
    }

    public async void GetAllRegions()
    {
        List<Country> CountriesAll = await HTTP_Controller.GetCountries();
        List<string> regionNames = new List<string>();
        foreach (Country country in CountriesAll)
        {
            if(country.icaO_Code == "")
            {
                regionNames.Add(country.name);
                Countries.Add(country);
            }
        }

        

       ManufactorerRegionDropDown.ItemsSource = regionNames;
    }
}