using MetalPerformanceShaders;
using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditSpottingPictureModal : ContentPage
{
    public List<SpottingTrip> SpottingTrips = new List<SpottingTrip>();
    public List<Airport> Airports = new List<Airport>();
    public List<Aircraft> Aircrafts = new List<Aircraft>();
    FileResult fileResult;
    bool IsEditing;
    //Manufactorer manufactorer;
    Picker SpottingTripPicker = null;
    Picker AirportPicker = null;
    Picker AircraftPicker = null;

    public EditSpottingPictureModal()
	{
		InitializeComponent();
        this.IsEditing = false;
        Submit.Clicked += Submit_Clicked;
        BtnFilePicker.Clicked += BtnFilePicker_Clicked;
    }

    private void BtnFilePicker_Clicked(object sender, EventArgs e)
    {
        OpenFile();
    }

    private async void OpenFile()
    {
        PickOptions pickOptions = new PickOptions();
        pickOptions.FileTypes = FilePickerFileType.Images;
        pickOptions.PickerTitle = "Select Image";
        fileResult = await FilePicker.PickAsync(pickOptions);
        PreviewImage.Source = ImageSource.FromFile(fileResult.FullPath);
    }

    private void Submit_Clicked(object sender, EventArgs e)
    {
        int Aircraft = Aircrafts[AircraftPicker.SelectedIndex].id;
        int SpottingTrip = SpottingTrips[SpottingTripPicker.SelectedIndex].id;
        int Airport = Airports[AirportPicker.SelectedIndex].id;
        if (IsEditing)
        {

        }
        else
        {

        }

        Navigation.RemovePage(this);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        GetAllSpottingTrips();
        GetAllAircraft();
        base.OnNavigatedTo(args);
    }

    public async void GetAllSpottingTrips()
    {
        if (SpottingTripPicker != null)
        {
            GridMain.Children.Remove(SpottingTripPicker);
        }

        if(AirportPicker != null)
        {
            GridMain.Children.Remove(AirportPicker);    
        }

        SpottingTrips = await HTTP_Controller.GetSpottingTrips();
        SpottingTripPicker = new Picker();
        List<string> spottingTripNames = new List<string>();

        foreach (SpottingTrip spottingTrip in SpottingTrips)
        {
            spottingTripNames.Add(spottingTrip.name + " - " + spottingTrip.id);
        }

        spottingTripNames.Add("Create New");

        SpottingTripPicker.ItemsSource = spottingTripNames;

        SpottingTripPicker.Title = "Select a SpottingTrip";

        if (IsEditing)
        {
           // int ID = Countries.FindIndex(c => c.id == manufactorer.region);
           // RegionPicker.SelectedIndex = ID;
        }

        SpottingTripPicker.SelectedIndexChanged += SpottingTripPickerSelectionChanged;

        GridMain.Add(SpottingTripPicker, 1, 3);
    }

    public async void GetAllAircraft()
    {
        if (AircraftPicker != null)
        {
            GridMain.Children.Remove(AircraftPicker);
        }

        Aircrafts = await HTTP_Controller.GetAircrafts();
        AircraftPicker = new Picker();
        List<string> aircraftNames = new List<string>();

        foreach (Aircraft aircraft in Aircrafts)
        {
            aircraftNames.Add(aircraft.registration + " - " + aircraft.id);
        }

        aircraftNames.Add("Create New");

        AircraftPicker.ItemsSource = aircraftNames;

        AircraftPicker.Title = "Select an Aircraft";

        if (IsEditing)
        {
            // int ID = Countries.FindIndex(c => c.id == manufactorer.region);
            // RegionPicker.SelectedIndex = ID;
        }

        AircraftPicker.SelectedIndexChanged += AircraftPickerSelectionChanged;
        Grid.SetColumnSpan(AircraftPicker, 2);

        GridMain.Add(AircraftPicker, 1, 4);
    }

    private void AircraftPickerSelectionChanged(object sender, EventArgs e)
    {
        switch (AircraftPicker.SelectedItem)
        {
            case "Create New":
                CreateNewAircraft();
                break;
        }
    }

    private void AirportPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (AirportPicker.SelectedItem)
        {
            case "Create New":
                CreateNewAirport();
                break;
        }
    }

    public async void GetAllAirports()
    {
        if (AirportPicker != null)
        {
            GridMain.Children.Remove(AirportPicker);
        }

        int SpottingTripID = SpottingTrips[SpottingTripPicker.SelectedIndex].id;
        Airports = await HTTP_Controller.GetAirportsFromSpottingTrip(SpottingTripID);

        AirportPicker = new Picker();
        AirportPicker.Title = "Select an Airport";

        AirportPicker.SelectedIndexChanged += AirportPicker_SelectedIndexChanged;

        List<string> airportNames = new List<string>();

        foreach (Airport airport in Airports)
        {
            airportNames.Add(airport.name + " - " + airport.id);
        }

        airportNames.Add("Create New");

        AirportPicker.ItemsSource = airportNames;


        if (IsEditing)
        {
            // int ID = Countries.FindIndex(c => c.id == manufactorer.region);
            // RegionPicker.SelectedIndex = ID;
        }

        AirportPicker.IsEnabled = true;
        GridMain.Add(AirportPicker, 2, 3);
    }

    private void SpottingTripPickerSelectionChanged(object sender, EventArgs e)
    {
        switch (SpottingTripPicker.SelectedItem)
        {
            case "Create New":
                CreateNewSpottingTrip();
                break;
            default:
                GetAllAirports();
                break;
        }
    }

    private void CreateNewSpottingTrip()
    {
        EditSpottingTripModal editSpottingTripModal = new EditSpottingTripModal();
        Navigation.PushAsync(editSpottingTripModal);
    }

    private void CreateNewAirport()
    {
        EditAirportModal editAirportModal = new EditAirportModal();
        Navigation.PushAsync(editAirportModal);
    }

    private void CreateNewAircraft()
    {
        EditAircraftModal editAircraftModal = new EditAircraftModal();
        Navigation.PushAsync(editAircraftModal);
    }
}