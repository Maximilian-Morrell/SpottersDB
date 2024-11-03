using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditAirlineModal : ContentPage
{
    public List<Country> Countries = new List<Country>();
    bool IsEditing;
    Airline airline;
    Picker CountryPicker = null;

    public EditAirlineModal()
	{
		InitializeComponent();
        IsEditing = false;
        Submit.Clicked += Submit_Clicked;
    }

    public EditAirlineModal(Airline airline)
    {
        InitializeComponent();
        IsEditing = true;
        Submit.Clicked += Submit_Clicked;
        this.airline = airline;
        AirlineICAO.Text = airline.icao;
        AirlineIATA.Text = airline.iata;
        AirlineName.Text = airline.name;
    }

    private void Submit_Clicked(object sender, EventArgs e)
    {
        int Region = Countries[CountryPicker.SelectedIndex].id;
        if (IsEditing)
        {
            int ID = airline.id;
            airline = new Airline(ID, AirlineICAO.Text, AirlineIATA.Text, AirlineName.Text, Region);
            HTTP_Controller.UpdateAirline(airline);
            Navigation.RemovePage(this);
        }
        else
        {
            airline = new Airline(AirlineICAO.Text, AirlineIATA.Text, AirlineName.Text, Region);
            HTTP_Controller.AddNewAirline(airline);
            Navigation.RemovePage(this);
        }
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        GetAllCountries();
        base.OnNavigatedTo(args);
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
            int ID = Countries.FindIndex(c => c.id == airline.region);
            CountryPicker.SelectedIndex = ID;
        }

        CountryPicker.SelectedIndexChanged += CountryPickerSelectionChanged;

        GridMain.Add(CountryPicker, 1, 3);
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