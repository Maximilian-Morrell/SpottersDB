using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditAirportModal : ContentPage
{
    public List<Country> Countries = new List<Country>();
    bool IsEditing;
    Airport airport;
    Picker CountryPicker = null;
    bool IsLoaded = false;

    public EditAirportModal()
	{
		InitializeComponent();
        Submit.Clicked += Submit_Clicked;
        AirportICAO.Text = "";
        AirportIATA.Text = "";
        AirportName.Text = "";
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

    private async void Submit_Clicked(object sender, EventArgs e)
    {
        try
        {
            int Region = Countries[CountryPicker.SelectedIndex - 1].id;
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
        catch (Exception ex)
        {
            await DisplayAlert("Something has gone wrong with saving", ex.Message, "OK");
        }
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
           int ID = Countries.FindIndex(c => c.id == airport.countryID);
           CountryPicker.SelectedIndex = ID;
        }

        CountryPicker.SelectedIndexChanged += CountryPickerSelectionChanged;

        GridMain.Add(CountryPicker, 1, 5);
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
            Submit.IsEnabled = CountryPicker.SelectedIndex >= 1 && AirportICAO.Text.Length > 0 && AirportIATA.Text.Length > 0 && AirportName.Text.Length > 0;
        }
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        CheckIfValid();
    }
}