using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditAirlineModal : ContentPage
{
    public List<Country> Countries = new List<Country>();
    bool IsEditing;
    Airline airline;
    Picker CountryPicker = null;
    bool IsLoaded = false;

    public EditAirlineModal()
	{
		InitializeComponent();
        IsEditing = false;
        AirlineICAO.Text = "";
        AirlineIATA.Text = "";
        AirlineName.Text = "";
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

    private async void Submit_Clicked(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            await DisplayAlert("Something has gone wrong with saving", ex.Message, "OK");
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

        regionNames.Add("Create New");
        foreach (Country country in Countries)
        {
            regionNames.Add(country.name + " - " + country.id);
        }


        CountryPicker.ItemsSource = regionNames;

        CountryPicker.Title = "Select a Country";

        if (IsEditing)
        {
            int ID = Countries.FindIndex(c => c.id == airline.region);
            CountryPicker.SelectedIndex = ID;
        }

        CountryPicker.SelectedIndexChanged += CountryPickerSelectionChanged;

        GridMain.Add(CountryPicker, 1, 3);

        IsLoaded = true;
    }

    private void CountryPickerSelectionChanged(object sender, EventArgs e)
    {
        switch (CountryPicker.SelectedItem)
        {
            case "Create New":
                CreateNewCountry();
                break;
            default:
                CheckIfValid();
                break;
        }
    }

    private void CreateNewCountry()
    {
        EditCountryModal editCountryModal = new EditCountryModal();
        Navigation.PushAsync(editCountryModal);
    }

    private void CheckIfValid()
    {
        if (IsLoaded)
        {
            Submit.IsEnabled = CountryPicker.SelectedIndex >= 1 && AirlineICAO.Text.Length > 0 && AirlineIATA.Text.Length > 0 && AirlineName.Text.Length > 0;
        }
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        CheckIfValid();
    }
}