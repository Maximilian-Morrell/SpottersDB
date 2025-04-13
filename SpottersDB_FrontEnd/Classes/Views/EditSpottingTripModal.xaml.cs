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
    bool IsLoaded = false;

    public EditSpottingTripModal()
	{
		InitializeComponent();
        Submit.Clicked += Submit_Clicked;
        SpottingTripName.Text = "";
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
        Submit.IsEnabled = true;
    }

    private async void Submit_Clicked(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            await DisplayAlert("Something has gone wrong with saving", ex.Message, "OK");
        }

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
            GridMain.Remove(AirportPicker);
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

        airportNames.Add("Create New");
        foreach (Airport airport in airports)
        {
            airportNames.Add(airport.name + " - " + airport.id);
        }

        AirportPicker.ItemsSource = airportNames;

        AirportPicker.Title = "Select an Airport";

        AirportPicker.SelectedIndexChanged += AirportPickerSelectionChange;
        AirportPicker.HorizontalOptions = LayoutOptions.End;

        GridMain.Add(AirportPicker, 1,4);
        Grid.SetColumnSpan(AirportPicker, 2);
        IsLoaded = true;
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
                CheckIfValid();
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
        try
        {
            SelectedAirports.Add(airports[AirportPicker.SelectedIndex - 1]);
            AirportCard airportCard = new AirportCard();
            VerticalStackLayout Container = new VerticalStackLayout();
            Border b = await airportCard.Card(airports[AirportPicker.SelectedIndex - 1]);
            Container.Children.Add(b);
            Button btn = new Button();
            btn.Text = "Delete Airport from Trip";
            btn.CommandParameter = airports[AirportPicker.SelectedIndex -1];
            btn.Clicked += DeleteFromTripClicked;
            Container.Children.Add(btn);
            airportCard.EditClicked += AirportCard_EditClicked;
            AirportParent.Children.Add(Container);
            GetAllAirports();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Something has gone wrong with adding the Airport to the List", ex.Message, "OK");
        }
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
            CheckIfValid();
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

    private void CheckIfValid()
    {
        if(IsLoaded)
        {
            Submit.IsEnabled = SpottingTripName.Text.Length > 0 && SpottingTripStartDate.Date != null && SpottingTripStartTime.Time != null && SpottingTripEndDate.Date >= SpottingTripStartDate.Date && SpottingTripEndTime.Time != null && SelectedAirports.Count > 0;
        }
    }

    private void Date_Selected(object sender, DateChangedEventArgs e)
    {
        CheckIfValid();
    }

    private void Time_Selected(object sender, TimeChangedEventArgs e)
    {
        CheckIfValid();
    }

    private void SpottingTripName_TextChanged(object sender, TextChangedEventArgs e)
    {
        CheckIfValid();
    }
}