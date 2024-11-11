using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditAirportModal : ContentPage
{
    public List<Country> Countries = new List<Country>();
    bool IsEditing;
    Airport airport;
    Picker CountryPicker = null;

    public EditAirportModal()
	{
		InitializeComponent();
        Submit.Clicked += Submit_Clicked;
        IsEditing = false;
	}

    public EditAirportModal(Airport airport)
    {
        InitializeComponent();
        Submit.Clicked += Submit_Clicked;
        this.IsEditing = true;
        this.airport = airport;

        AirportICAO.Text = airport.icaO_Code;
        AirportIATA.Text = airport.iatA_Code;
        AirportName.Text = airport.name;
        AirportDescription.Text = airport.description;
        AirportCity.Text = airport.city;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        GetAllCountries();
        base.OnNavigatedTo(args);
    }

    private void Submit_Clicked(object sender, EventArgs e)
    {
        int Region = Countries[CountryPicker.SelectedIndex].id;
        if (IsEditing)
        {
            int ID = airport.id;
            airport = new Airport(ID, AirportICAO.Text, AirportIATA.Text, AirportName.Text, AirportDescription.Text, AirportCity.Text, Region);
            HTTP_Controller.UpdateAirport(airport);
        }
        else
        {
            airport = new Airport(AirportICAO.Text, AirportIATA.Text, AirportName.Text, AirportDescription.Text, AirportCity.Text, Region);
            HTTP_Controller.AddAirport(airport);
        }

        Navigation.RemovePage(this);
    }

    public async void GetAllCountries()
    {
        if (CountryPicker != null)
        {
            GridMain.Children.Remove(CountryPicker);
        }

        Countries = await HTTP_Controller.GetCountries(true);
        CountryPicker = new Picker();
        List<string> regionNames = new List<string>();

        foreach (Country country in Countries)
        {
            regionNames.Add(country.name + " - " + country.id);
        }

        regionNames.Add("Create New");

        CountryPicker.ItemsSource = regionNames;

        CountryPicker.Title = "Select a Country";

        if (IsEditing)
        {
           int ID = Countries.FindIndex(c => c.id == airport.countryID);
           CountryPicker.SelectedIndex = ID;
        }

        CountryPicker.SelectedIndexChanged += CountryPickerSelectionChanged;

        GridMain.Add(CountryPicker, 1, 5);
    }

    private void CountryPickerSelectionChanged(object sender, EventArgs e)
    {
        switch (CountryPicker.SelectedItem)
        {
            case "Create New":
                CreateNewCountry();
                break;
        }
    }

    private void CreateNewCountry()
    {
        EditCountryModal editCountryModal = new EditCountryModal();
        Navigation.PushAsync(editCountryModal);
    }
}