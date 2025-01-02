using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;
using System;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditSpottingPictureModal : ContentPage
{
    public List<SpottingTrip> SpottingTrips = new List<SpottingTrip>();
    public List<Airport> Airports = new List<Airport>();
    public List<Aircraft> Aircrafts = new List<Aircraft>();
    FileResult fileResult;
    bool IsEditing;
    SpottingPicture spottingPicture;
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

    public EditSpottingPictureModal(SpottingPicture spottingPicture)
    {
        InitializeComponent();
        this.IsEditing = true;
        Submit.Clicked += Submit_Clicked;
        BtnFilePicker.Clicked += BtnFilePicker_Clicked;
        this.spottingPicture = spottingPicture;
        SetUp();
        GetAllAirports();
        
    }

    private async void SetUp()
    {
        SpottingPictureName.Text = spottingPicture.name;
        SpottingPictureDescription.Text = spottingPicture.description;
        PreviewImage.Source = new UriImageSource
        {
            Uri = new Uri(spottingPicture.pictureUrl)
        };
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
        SaveSpottingPicture();
    }

    private async void SaveSpottingPicture()
    {
        int Aircraft = Aircrafts[AircraftPicker.SelectedIndex].id;
        int SpottingTrip = SpottingTrips[SpottingTripPicker.SelectedIndex].id;
        int Airport = Airports[AirportPicker.SelectedIndex].id;

        int LinkID = await HTTP_Controller.GetLinkID(SpottingTrip, Airport);

        if (IsEditing)
        {
            if (fileResult == null)
            {
                spottingPicture.name = SpottingPictureName.Text;
                spottingPicture.description = SpottingPictureDescription.Text;
                spottingPicture.spottingTripAirportID = LinkID;
                spottingPicture.aircraftID = Aircraft;

                HTTP_Controller.UpdateSpottingPicture(spottingPicture);
            }
            else
            {
                string FileName = spottingPicture.pictureUrl.Substring(spottingPicture.pictureUrl.LastIndexOf('/') +1);
                int ID = spottingPicture.id;
                spottingPicture = new SpottingPicture(ID, SpottingPictureName.Text, SpottingPictureDescription.Text, LinkID, Aircraft);
                spottingPicture.pictureUrl = FileName;
                HTTP_Controller.UpdateSpottingPicture(spottingPicture, fileResult);
            }
        }
        else
        {
            spottingPicture = new SpottingPicture(SpottingPictureName.Text, SpottingPictureDescription.Text, LinkID, Aircraft);
            HTTP_Controller.AddNewSpottingPicture(spottingPicture, fileResult);
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
            Dictionary<string, int> ID = await HTTP_Controller.GetSpottingTripAirport(spottingPicture.spottingTripAirportID);
            int PickerID = SpottingTrips.FindIndex(s => s.id == ID["SpottingTrip"]);
            SpottingTripPicker.SelectedIndex = PickerID;
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
            int ID = Aircrafts.FindIndex(a => a.id == spottingPicture.aircraftID);
            AircraftPicker.SelectedIndex = ID;
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

        if(IsEditing && AirportPicker == null)
        {
            Dictionary<string, int> IDs = await HTTP_Controller.GetSpottingTripAirport(spottingPicture.spottingTripAirportID);
            Airports = await HTTP_Controller.GetAirportsFromSpottingTrip(IDs["Airport"]);
        }
        else
        {
            int SpottingTripID = SpottingTrips[SpottingTripPicker.SelectedIndex].id;
            Airports = await HTTP_Controller.GetAirportsFromSpottingTrip(SpottingTripID);
        }

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
            Dictionary<string, int> ID = await HTTP_Controller.GetSpottingTripAirport(spottingPicture.spottingTripAirportID);
            int PickerID = Airports.FindIndex(s => s.id == ID["Airport"]);
            AirportPicker.SelectedIndex = PickerID;
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