using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.UI_Elements.Cards;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class HomeAircrafts : ContentPage
{
    List<Country> Countries = null;
    List<Airline> Airlines = null;
    List<AircraftType> Types = null;
    public HomeAircrafts()
	{
		InitializeComponent();
	}

    private EventHandler OpenClicked(Aircraft aircraft)
    {
        //EditAirlineModal editAirlineModal = new EditAirlineModal(airline);
        //Navigation.PushAsync(editAirlineModal);
        return null;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        LoadEverything();
        base.OnNavigatedTo(args);
    }

    public async void LoadEverything()
    {
        LoadFilters();
    }

    private async void LoadFilters()
    {
        Countries = await HTTP_Controller.GetCountries();
        CountryPicker.Items.Clear();
        CountryPicker.Items.Add("");
        if (Countries != null)
        {
            foreach (Country country in Countries)
            {
                if (country.icaO_Code != "")
                {
                    CountryPicker.Items.Add(country.icaO_Code + " - " + country.name);
                }
            }
        }

        Airlines = await HTTP_Controller.GetAirlines();
        AirlinePicker.Items.Clear();
        AirlinePicker.Items.Add("");
        if(Airlines != null)
        {
            foreach(Airline air in Airlines)
            {
                AirlinePicker.Items.Add(air.iata + " - " + air.name);
            }
        }

        Types = await HTTP_Controller.GetAircraftTypes();
        TypePicker.Items.Clear();
        TypePicker.Items.Add("");
        if(Types != null)
        {
            foreach(AircraftType type in Types)
            {
                TypePicker.Items.Add(type.icaoCode + " - " + type.fullName);
            }
        }

       LoadAircrafts();
    }

    private async void LoadAircrafts()
    {
        List<Aircraft> Aircrafts = await HTTP_Controller.GetAircrafts();
        foreach(Aircraft aircraft in Aircrafts)
        {
            AircraftCard card = new AircraftCard();
            Border b = await card.CardHome(aircraft);
            card.EditClicked += OpenClicked;
            ContentParent.Children.Add(b);
        }
    }
}