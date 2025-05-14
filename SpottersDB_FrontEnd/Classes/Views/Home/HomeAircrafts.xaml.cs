using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.UI_Elements.Cards;
using SpottersDB_FrontEnd.Classes.Utilities;
using SpottersDB_FrontEnd.Classes.Views.Home.Details;

namespace SpottersDB_FrontEnd.Classes.Views.Home;

public partial class HomeAircrafts : ContentPage
{
    List<Aircraft> AllAircrafts = null;
    List<Country> Countries = null;
    List<Airline> Airlines = null;
    List<AircraftType> Types = null;
    List<Border> Cards = new List<Border>();
    bool IsLoaded = false;

    public HomeAircrafts()
	{
		InitializeComponent();
	}

    private EventHandler OpenClicked(Aircraft aircraft)
    {
        AircraftDetails aircraftDetails = new AircraftDetails(aircraft);
        Navigation.PushAsync(aircraftDetails);
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
        List<Country> CountriesTmp = await HTTP_Controller.GetCountries();
        Countries = new List<Country>();
        CountryPicker.Items.Clear();
        CountryPicker.Items.Add("");
        if (Countries != null)
        {
            foreach (Country country in CountriesTmp)
            {
                if (country.icaO_Code != "")
                {
                    Countries.Add(country);
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
        IsLoaded = true;
        ContentParent.Clear();
        LoadAircrafts();
    }

    private async void LoadAircrafts()
    {
        try
        {
            AllAircrafts = await HTTP_Controller.GetAircrafts();
            ContentParent.Opacity = 1;
            await Microsoft.Maui.Controls.ViewExtensions.FadeTo(ContentParent, 0, 100, Easing.CubicInOut);
            ContentParent.Clear();
            foreach (Aircraft aircraft in FilterAircraft())
            {
                AircraftCard card = new AircraftCard();
                Border b = await card.CardHome(aircraft);
                card.EditClicked += OpenClicked;
                Cards.Add(b);
                ContentParent.Children.Add(b);
            }
            await Microsoft.Maui.Controls.ViewExtensions.FadeTo(ContentParent, 1, 100, Easing.CubicInOut);

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error: ", ex.Message, "Ok");
        }
    }

    private List<Aircraft> FilterAircraft()
    {
        string SearchParams = Search.Text.ToLower();
        List<Aircraft> AircraftFiltered = AllAircrafts.FindAll(air => air.registration.ToLower().Contains(SearchParams));

        if (CountryPicker.SelectedIndex > 0)
        {
            int CountryID = Countries[CountryPicker.SelectedIndex - 1].id;
            AircraftFiltered = AircraftFiltered.FindAll(air => air.countryID == CountryID);
        }

        if(AirlinePicker.SelectedIndex > 0)
        {
            int AirlineID = Airlines[AirlinePicker.SelectedIndex - 1].id;
            AircraftFiltered = AircraftFiltered.FindAll(air => air.airlineID == AirlineID);
        }

        if (TypePicker.SelectedIndex > 0)
        {
            int TypeID = Types[TypePicker.SelectedIndex - 1].id;
            AircraftFiltered = AircraftFiltered.FindAll(air => air.typeID == TypeID);
        }

        return AircraftFiltered;
    }

    private void Search_TextChanged(object sender, TextChangedEventArgs e)
    {
        if(IsLoaded)
        {
            LoadAircrafts();
        }

    }

    private void SelectionChanged(object sender, EventArgs e)
    {
        if(IsLoaded)
        {
            LoadAircrafts();
        }
    }

}