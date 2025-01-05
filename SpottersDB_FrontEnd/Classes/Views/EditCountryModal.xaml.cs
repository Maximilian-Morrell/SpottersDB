using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditCountryModal : ContentPage
{
    bool IsEdit;
    bool IsRegion;
    bool IsLoaded = false;
    int ID;

    public void InitTxtBoxes()
    {
        CountryICAO.Text = "";
        CountryName.Text = "";
    }

	public EditCountryModal()
	{
		InitializeComponent();
        InitTxtBoxes();
        Submit.IsEnabled = false;
        Title = "Create new Country";
        Submit.Text = "Create";
        IsEdit = false;
        IsRegion = false;
        Submit.Clicked += Submit_Clicked;
        IsLoaded = true;
    }

    public EditCountryModal(bool IsRegionOnly)
    {
        InitializeComponent();
        InitTxtBoxes();
        Title = "Create new Region";
        Submit.Text = "Create";
        IsEdit = false;
        Submit.IsEnabled = false;
        IsRegion = IsRegionOnly;
        Submit.Clicked += Submit_Clicked;
        CountryICAO.IsEnabled = !IsRegionOnly;
        IsLoaded = true;
    }

    public EditCountryModal(Country c)
    {
        InitializeComponent();
        InitTxtBoxes();
        ID = c.id;
        IsEdit = true;
        Title = "Edit: " + c.name;
        Submit.Text = "Edit";
        IsRegion = c.icaO_Code.Length == 0;
        CountryICAO.IsEnabled = !IsRegion;
        CountryICAO.Text = c.icaO_Code;
        CountryName.Text = c.name;
        Submit.Clicked += Submit_Clicked;
        IsLoaded = true;
    }

    private async void Submit_Clicked(object sender, EventArgs e)
    {
        Country c;
        if (IsEdit)
        {
            c = new Country(ID, CountryICAO.Text, CountryName.Text);
            await HTTP_Controller.EditCountry(c);
            Navigation.RemovePage(this);
        }
        else
        {
            c = new Country(CountryICAO.Text, CountryName.Text);
            await HTTP_Controller.AddCountry(c);
            Navigation.RemovePage(this);
        }

    }


    private void TextChanged(object sender, TextChangedEventArgs e)
    {
        if(IsLoaded)
        {
            if (IsRegion)
            {
                Submit.IsEnabled = CountryName.Text.Length >= 1;
            }
            else if(IsEdit)
            {
                Submit.IsEnabled = CountryName.Text.Length >= 1 && CountryICAO.Text.Length >= 1;
            }
            else
            {
                Submit.IsEnabled = CountryName.Text.Length >= 1;
            }
        }
    }
}