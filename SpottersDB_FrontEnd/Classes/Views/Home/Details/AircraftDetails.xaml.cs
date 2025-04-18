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
    HashSet<SpottingTrip> trips = new HashSet<SpottingTrip>();
    Dictionary<int, int> Trip_Link = new Dictionary<int, int>();
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
            trips.Add(await HTTP_Controller.GetSpottingTrip(Trip_Airport["SpottingTrip"]));
            Trip_Link.Add(Trip_Airport["SpottingTrip"], pic.spottingTripAirportID);
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
        foreach (SpottingTrip trip in trips)
        {
            VerticalStackLayout parent = new VerticalStackLayout();
            Label lbl = UI_Utilities.CreateLabel(parent, trip.name, 50, FontAttributes.Bold);
            FlexLayout Test = new FlexLayout();
            foreach (SpottingPicture pic in pictures.Where(p => p.spottingTripAirportID == Trip_Link[trip.id]))
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