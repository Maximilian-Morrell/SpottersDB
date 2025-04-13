using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditAircraftModal : ContentPage
{
    public List<Country> Countries = new List<Country>();
    public List<AircraftType> Types = new List<AircraftType>();
    public List<Airline> Airlines = new List<Airline>();
    bool IsEditing;
    Aircraft aircraft;
    Picker CountryPicker = null;
    Picker TypePicker = null;
    Picker AirlinePicker = null;
    bool IsLoaded = false;


    public EditAircraftModal()
	{
		InitializeComponent();
        Submit.Clicked += Submit_Clicked;
        AircraftRegistration.Text = "";
        AircraftDescription.Text = "";
        this.IsEditing = false;
	}

    public EditAircraftModal(Aircraft aircraft)
    {
        InitializeComponent();
        Submit.Clicked += Submit_Clicked;
        this.aircraft = aircraft;
        AircraftRegistration.Text = aircraft.registration;
        AircraftDescription.Text = aircraft.description;
        Submit.IsEnabled = true;
        this.IsEditing = true;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        GetAllTypes();
        GetAllAirlines();
        GetAllCountries();
        base.OnNavigatedTo(args);
    }

    private async void Submit_Clicked(object sender, EventArgs e)
    {
        try
        {
            int Type = Types[TypePicker.SelectedIndex - 1].id;
            int Airline = Airlines[AirlinePicker.SelectedIndex -1 ].id;
            int Country = Countries[CountryPicker.SelectedIndex - 1].id;
            if (IsEditing)
            {
                int ID = aircraft.id;
                aircraft = new Aircraft(ID, AircraftRegistration.Text, AircraftDescription.Text, Type, Airline, Country);
                HTTP_Controller.UpdateAircraft(aircraft);
            }
            else
            {
                aircraft = new Aircraft(AircraftRegistration.Text, AircraftDescription.Text, Type, Airline, Country);
                HTTP_Controller.AddNewAircraft(aircraft);
            }
            Navigation.RemovePage(this);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Something has gone wrong with saving", ex.Message, "OK");
        }
    }

    public async void GetAllTypes()
    {
        if (TypePicker != null)
        {
            GridMain.Children.Remove(TypePicker);
        }
        Thread.Sleep(250);
        Types = await HTTP_Controller.GetAircraftTypes();
        TypePicker = new Picker();
        List<string> TypeNames = new List<string>();

        TypeNames.Add("Create New");
        foreach (AircraftType type in Types)
        {
            TypeNames.Add(type.icaoCode + " - " + type.id);
        }


        TypePicker.ItemsSource = TypeNames;

        TypePicker.Title = "Select an Aircraft Type";

        if (IsEditing)
        {
            int ID = Types.FindIndex(c => c.id == aircraft.typeID);
            TypePicker.SelectedIndex = ID;
        }

        TypePicker.SelectedIndexChanged += TypePicker_SelectedIndexChanged;

        GridMain.Add(TypePicker, 1, 2);
    }

    private void TypePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (TypePicker.SelectedItem)
        {
            case "Create New":
                CreateNewType();
                break;
            default:
                CheckIfValid();
                break;
        }
    }

    private void CreateNewType()
    {
        EditAircraftTypeModal editTypeModal = new EditAircraftTypeModal();
        Navigation.PushAsync(editTypeModal);
    }

    public async void GetAllAirlines()
    {
        if (AirlinePicker != null)
        {
            GridMain.Children.Remove(AirlinePicker);
        }

        Airlines = await HTTP_Controller.GetAirlines();
        AirlinePicker = new Picker();
        List<string> airlineNames = new List<string>();

        airlineNames.Add("Create New");
        foreach (Airline airline in Airlines)
        {
            airlineNames.Add(airline.name + " - " + airline.id);
        }


        AirlinePicker.ItemsSource = airlineNames;

        AirlinePicker.Title = "Select an Airline";

        if (IsEditing)
        {
            int ID = Airlines.FindIndex(c => c.id == aircraft.airlineID);
            AirlinePicker.SelectedIndex = ID;
        }

        AirlinePicker.SelectedIndexChanged += AirlinePicker_SelectedIndexChanged;

        GridMain.Add(AirlinePicker, 1, 3);
    }

    private void AirlinePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (AirlinePicker.SelectedItem)
        {
            case "Create New":
                CreateNewAirline();
                break;
            default:
                CheckIfValid();
                break;
        }
    }

    private void CreateNewAirline()
    {
        EditAirlineModal editAirlineModal = new EditAirlineModal();
        Navigation.PushAsync(editAirlineModal);
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
            int ID = Countries.FindIndex(c => c.id == aircraft.countryID);
            CountryPicker.SelectedIndex = ID + 1;
        }

        CountryPicker.SelectedIndexChanged += CountryPickerSelectionChanged;

        GridMain.Add(CountryPicker, 1, 4);
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
            Submit.IsEnabled = TypePicker.SelectedIndex >= 1 && AirlinePicker.SelectedIndex >= 1 && CountryPicker.SelectedIndex >= 1 && AircraftRegistration.Text.Length > 0;
        }
    }

    private void AircraftRegistration_TextChanged(object sender, TextChangedEventArgs e)
    {
        CheckIfValid();
    }
}