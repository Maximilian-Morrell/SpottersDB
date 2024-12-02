using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.UI_Elements.Cards;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditSpottingTripModal : ContentPage
{
    public List<Airport> airports = new List<Airport>();
    public List<Airport> SelectedAirports = new List<Airport>();
    bool IsEditing;
    SpottingTrip spottingTrip;
    Picker AirportPicker = null;

    public EditSpottingTripModal()
	{
		InitializeComponent();
        Submit.Clicked += Submit_Clicked;
        this.IsEditing = false;
	}

    public EditSpottingTripModal(SpottingTrip spottingTrip, List<Airport> SelectedAirports)
    {
        InitializeComponent();
        Submit.Clicked += Submit_Clicked;
        this.spottingTrip = spottingTrip;
        SpottingTripName.Text = spottingTrip.name;
        SpottingTripDescription.Text = spottingTrip.description;
        SpottingTripStartDate.Date = spottingTrip.start.Date;
        SpottingTripStartTime.Time = new TimeSpan(spottingTrip.start.TimeOfDay.Hours, spottingTrip.start.Minute, spottingTrip.start.Second);
        SpottingTripStartDate.Date = spottingTrip.end.Date;
        SpottingTripStartTime.Time = new TimeSpan(spottingTrip.end.TimeOfDay.Hours, spottingTrip.end.Minute, spottingTrip.end.Second);
        this.SelectedAirports = SelectedAirports;
        foreach(Airport airport in SelectedAirports)
        {
            AddAirport(airport);
        }
        this.IsEditing = true;
    }

    private void Submit_Clicked(object sender, EventArgs e)
    {
        DateTime Start = new DateTime(SpottingTripStartDate.Date.Year, SpottingTripStartDate.Date.Month, SpottingTripStartDate.Date.Day, SpottingTripStartTime.Time.Hours, SpottingTripStartTime.Time.Minutes, 0);
        DateTime End = new DateTime(SpottingTripEndDate.Date.Year, SpottingTripEndDate.Date.Month, SpottingTripEndDate.Date.Day, SpottingTripEndTime.Time.Hours, SpottingTripEndTime.Time.Minutes, 0);
        if (IsEditing)
        {
            int ID = spottingTrip.id;
            spottingTrip = new SpottingTrip(ID, SpottingTripName.Text, SpottingTripDescription.Text, Start, End, SelectedAirports);
            HTTP_Controller.UpdateSpottingTrip(spottingTrip);
        }
        else
        {
            spottingTrip = new SpottingTrip(SpottingTripName.Text, SpottingTripDescription.Text, Start, End, SelectedAirports);
            HTTP_Controller.AddNewSpottingTrip(spottingTrip);
        }
        Navigation.RemovePage(this);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        GetAllAirports();
        base.OnNavigatedTo(args);
    }

    public async void GetAllAirports()
    {
        if (AirportPicker != null)
        {
            AirportPickerParent.Children.Remove(AirportPicker);
        }

        airports = await HTTP_Controller.GetAirports();
        foreach(Airport airport in SelectedAirports)
        {
            int id = airports.FindIndex(a => a.id == airport.id);
            airports.RemoveAt(id);
        }
        
        if(AirportParent.Children.Count > 0)
        {
            AirportParent.Clear();
            foreach (Airport airport in SelectedAirports)
            {
                AddAirport(airport);
            }
        }

        AirportPicker = new Picker();
        List<string> airportNames = new List<string>();

        foreach(Airport airport in airports)
        {
            airportNames.Add(airport.name + " - " + airport.id);
        }

        airportNames.Add("Create New");

        AirportPicker.ItemsSource = airportNames;

        AirportPicker.Title = "Select an Airport";

        if (IsEditing)
        {
          //  int ID = airports.FindIndex(c => c.id == manufactorer.region);
          //  RegionPicker.SelectedIndex = ID;
        }

        AirportPicker.SelectedIndexChanged += AirportPickerSelectionChange;
        AirportPicker.HorizontalOptions = LayoutOptions.End;

        AirportPickerParent.Children.Add(AirportPicker);
    }

    private void AirportPickerSelectionChange(object sender, EventArgs e)
    {
        switch (AirportPicker.SelectedItem)
        {
            case "Create New":
                CreateNewAirport();
                break;
            default:
                AddAirport();
                break;
        }
    }

    private void CreateNewAirport()
    {
        EditAirportModal editAirportModal = new EditAirportModal();
        Navigation.PushAsync(editAirportModal);
    }

    private async void AddAirport()
    {
        SelectedAirports.Add(airports[AirportPicker.SelectedIndex]);
        AirportCard airportCard = new AirportCard();
        VerticalStackLayout Container = new VerticalStackLayout();
        Border b = await airportCard.Card(airports[AirportPicker.SelectedIndex]);
        Container.Children.Add(b);
        Button btn = new Button();
        btn.Text = "Delete Airport from Trip";
        btn.CommandParameter = airports[AirportPicker.SelectedIndex];
        btn.Clicked += DeleteFromTripClicked;
        Container.Children.Add(btn);
        airportCard.EditClicked += AirportCard_EditClicked;
        AirportParent.Children.Add(Container);
        GetAllAirports();
    }

    private void DeleteFromTripClicked(object sender, EventArgs e)
    {
        try
        {
            Button b = sender as Button;
            Airport airport = b.CommandParameter as Airport;

            int ID = SelectedAirports.FindIndex(a => a.id == airport.id);
            SelectedAirports.RemoveAt(ID);
            GetAllAirports();
        }
        catch (Exception ex)
        {
            Window w = new Window(new ErrorBox(ex.StackTrace, ex.InnerException.Message));
            Application.Current.OpenWindow(w);
        }
    }

    private async void AddAirport(Airport airport)
    {
        AirportCard airportCard = new AirportCard();
        VerticalStackLayout Container = new VerticalStackLayout();
        Border b = await airportCard.Card(airport);
        Container.Children.Add(b);
        Button btn = new Button();
        btn.Text = "Delete Airport from Trip";
        btn.CommandParameter = airport;
        btn.Clicked += DeleteFromTripClicked;
        Container.Children.Add(btn);
        airportCard.EditClicked += AirportCard_EditClicked;
        AirportParent.Children.Add(Container);
    }

    private EventHandler AirportCard_EditClicked(Airport airport)
    {
        EditAirportModal airportModal = new EditAirportModal(airport);
        Navigation.PushAsync(airportModal);
        return null;
    }
}