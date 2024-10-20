using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditCountryModal : ContentPage
{
    bool IsEdit;
    int ID;
	public EditCountryModal()
	{
		InitializeComponent();
        Title = "Create new Country";
        Submit.Text = "Create";
        IsEdit = false;
        Submit.Clicked += Submit_Clicked;
    }

    public EditCountryModal(Country c)
    {
        ID = c.id;
        IsEdit = true;
        InitializeComponent();
        Title = "Edit: " + c.name;
        Submit.Text = "Edit";

        Submit.Clicked += Submit_Clicked;
    }

    private async void Submit_Clicked(object sender, EventArgs e)
    {
        HTTP_Controller httpController = new HTTP_Controller();
        Country c;
        if (IsEdit)
        {
            c = new Country(ID, CountryICAO.Text, CountryName.Text);
            await httpController.EditCountry(c);
            Navigation.RemovePage(this);
        }
        else
        {
            c = new Country(CountryICAO.Text, CountryName.Text);
            await httpController.AddCountry(c);
            Navigation.RemovePage(this);
        }

    }
}