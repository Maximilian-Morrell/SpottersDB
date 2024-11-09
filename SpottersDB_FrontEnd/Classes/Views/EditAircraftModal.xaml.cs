using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditAircraftModal : ContentPage
{
    public List<Country> Countries = new List<Country>();
    public List<AircraftType> Types = new List<AircraftType>();
    public List<Airline> Airlines = new List<Airline>();
    bool IsEditing;
   // Airline airline;
    Picker CountryPicker = null;
    Picker TypePicker = null;
    Picker AirlinePicker = null;


    public EditAircraftModal()
	{
		InitializeComponent();
        this.IsEditing = false;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        GetAllTypes();
        GetAllAirlines();
        GetAllCountries();
        base.OnNavigatedTo(args);
    }

    public async void GetAllTypes()
    {
        if (TypePicker != null)
        {
            GridMain.Children.Remove(TypePicker);
        }

        Types = await HTTP_Controller.GetAircraftTypes();
        TypePicker = new Picker();
        List<string> TypeNames = new List<string>();

        foreach (AircraftType type in Types)
        {
            TypeNames.Add(type.icaoCode + " - " + type.id);
        }

        TypeNames.Add("Create New");

        TypePicker.ItemsSource = TypeNames;

        TypePicker.Title = "Select an Aircraft Type";

        if (IsEditing)
        {
            //  int ID = Countries.FindIndex(c => c.id == airline.region);
            //  CountryPicker.SelectedIndex = ID;
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

        foreach (Airline airline in Airlines)
        {
            airlineNames.Add(airline.name + " - " + airline.id);
        }

        airlineNames.Add("Create New");

        AirlinePicker.ItemsSource = airlineNames;

        AirlinePicker.Title = "Select an Airline";

        if (IsEditing)
        {
            //  int ID = Countries.FindIndex(c => c.id == airline.region);
            //  CountryPicker.SelectedIndex = ID;
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

        foreach (Country country in Countries)
        {
            regionNames.Add(country.name + " - " + country.id);
        }

        regionNames.Add("Create New");

        CountryPicker.ItemsSource = regionNames;

        CountryPicker.Title = "Select a Country";

        if (IsEditing)
        {
          //  int ID = Countries.FindIndex(c => c.id == airline.region);
          //  CountryPicker.SelectedIndex = ID;
        }

        CountryPicker.SelectedIndexChanged += CountryPickerSelectionChanged;

        GridMain.Add(CountryPicker, 1, 4);
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