using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.UI_Elements.Cards;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views.Home.Details;

public partial class AircraftDetails : ContentPage
{
    Country country;
    Aircraft aircraft;
    Airline airline;
    AircraftType aircraftType;
    Manufactorer manufactorer;
    List<SpottingPicture> pictures;
    HashSet<int> trips = new HashSet<int>();
    public AircraftDetails(Aircraft aircraft)
    {
        this.aircraft = aircraft;
        InitializeComponent();
        GetData(aircraft);
    }

    private EventHandler OpenClicked(SpottingPicture spottingPicture)
    {
        SpottingPictureDetails picDetails = new SpottingPictureDetails(spottingPicture);
        Navigation.PushAsync(picDetails);
        return null;
    }

    private async void GetData(Aircraft aircraft)
    {
        airline = await HTTP_Controller.GetAirlineByID(aircraft.airlineID);
        country = await HTTP_Controller.GetCountryByID(aircraft.countryID);
        aircraftType = await HTTP_Controller.GetAircraftTypeByID(aircraft.typeID);
        manufactorer = await HTTP_Controller.GetManufactorerByID(aircraftType.manufactorerID);

        pictures = await HTTP_Controller.GetSpottingPicturesByAircraft(aircraft.id);

        foreach (SpottingPicture pic in pictures)
        {
            Dictionary<string, int> Trip_Airport = await HTTP_Controller.GetSpottingTripAirport(pic.spottingTripAirportID);
            trips.Add(Trip_Airport["SpottingTrip"]);
        }
        FillInformation();
    }

    public async void FillInformation()
    {
        Title = "Aircraft Details: " + aircraft.registration;
        LBL_Registration.Text = aircraft.registration;
        LBL_Coutnry.Text = country.name;
        LBL_Type.Text = aircraftType.fullName;
        LBL_Manufactorer.Text = manufactorer.name;
        LBL_Airline.Text = airline.name;
        foreach (int tripID in trips)
        {
            SpottingTrip trip = await HTTP_Controller.GetSpottingTrip(tripID);
            VerticalStackLayout parent = new VerticalStackLayout();
            Label lbl = UI_Utilities.CreateLabel(parent, trip.name, 50, FontAttributes.Bold);
            FlexLayout Test = new FlexLayout();
            Test.Wrap = Microsoft.Maui.Layouts.FlexWrap.Wrap;
            Test.JustifyContent = Microsoft.Maui.Layouts.FlexJustify.SpaceEvenly;
            Test.Direction = Microsoft.Maui.Layouts.FlexDirection.Row;
            Dictionary<string, int> LinkID = await HTTP_Controller.GetSpottingTripAirport(trip.id);
            int searchID = LinkID["SpottingTrip"];
            foreach (SpottingPicture pic in pictures.Where(p => p.spottingTripAirportID == searchID))
            {
                SpottingPictureCard picCard = new SpottingPictureCard();
                Border b = await picCard.CardHome(pic);
                picCard.EditClicked += OpenClicked;
                Test.Add(b);
            }
            parent.Add(Test);
            SpottingTripParent.Add(parent);
        }
    }
}