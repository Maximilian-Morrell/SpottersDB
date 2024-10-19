using SpottersDB_FrontEnd.Classes.Structure;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditCountryModal : ContentPage
{
	public EditCountryModal()
	{
		InitializeComponent();
        Title = "Create new Country";
	}

    public EditCountryModal(Country c)
    {
        InitializeComponent();
        Title = "Edit: " + c.name;
    }
}