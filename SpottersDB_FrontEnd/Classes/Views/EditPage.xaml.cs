using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.UI_Elements.Cards;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views
{
    public partial class EditPage : ContentPage
    {   
        public EditPage()
        {
            InitializeComponent();
            AddCountry.Clicked += AddCountry_Clicked;
            AddManufactorer.Clicked += AddManufactorer_Clicked;
            AddAircraftType.Clicked += AddAircraftType_Clicked;
            AddAirline.Clicked += AddAirline_Clicked;
            AddAirport.Clicked += AddAirport_Clicked;
        }

        private void AddAirport_Clicked(object sender, EventArgs e)
        {
            EditAirportModal editAirportModal = new EditAirportModal();
            Navigation.PushAsync(editAirportModal);
        }

        private void AddAirline_Clicked(object sender, EventArgs e)
        {
            EditAirlineModal editAirlineModal = new EditAirlineModal();
            Navigation.PushAsync(editAirlineModal);
        }

        private void AddAircraftType_Clicked(object sender, EventArgs e)
        {
            EditAircraftTypeModal editAircraftTypeModal = new EditAircraftTypeModal();
            Navigation.PushAsync(editAircraftTypeModal);
        }

        private void AddManufactorer_Clicked(object sender, EventArgs e)
        {
            EditManufactorerModal editManufactorerModal = new EditManufactorerModal();
            Navigation.PushAsync(editManufactorerModal);
        }

        public async void LoadEverything()
        {
            LoadCountries();
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            LoadEverything();
            base.OnNavigatedTo(args);
        }

        private async void LoadCountries()
        {

            CountryParent.Children.Clear();
            RegionParent.Children.Clear();

            List<Country> countries = await HTTP_Controller.GetCountries();
            foreach(Country country in countries)
            {
                CountryCard countryCard = new CountryCard();
                countryCard.EditClicked += CountryCard_EditClicked;
                if(country.icaO_Code == "")
                {
                    RegionParent.Children.Add(countryCard.Card(country, ""));
                }
                else
                {
                    string URL = await HTTP_Controller.GetNewestPhotoFromCountry(country.id);
                    CountryParent.Children.Add(countryCard.Card(country, URL));
                }
            }

            LoadManufactorers();
        }

        private async void LoadManufactorers()
        {
            ManufactorerParent.Children.Clear();

            List<Manufactorer> manufactorers = await HTTP_Controller.GetManufactorers();
            foreach(Manufactorer manufactorer in manufactorers)
            {
                ManufactorerCard man = new ManufactorerCard();
                man.EditClicked += Manufactorer_EditClicked;
                Frame f = await man.Card(manufactorer);
                ManufactorerParent.Children.Add(f);
            }
            LoadAircraftTypes();
        }

        private async void LoadAircraftTypes()
        {
            AircraftTypeParent.Children.Clear();
            List<AircraftType> aircraftTypes = await HTTP_Controller.GetAircraftTypes();
            foreach(AircraftType aircraftType in aircraftTypes)
            {
                AircraftTypeCard air = new AircraftTypeCard();
                air.EditClicked += AircraftType_EditClicked;
                Frame f = await air.Card(aircraftType);
                AircraftTypeParent.Children.Add(f);
            }
            LoadAirlines();
        }

        private async void LoadAirlines()
        {
            AirlineParent.Children.Clear();
            List<Airline> airlines = await HTTP_Controller.GetAirlines();
            foreach(Airline airline in airlines)
            {
                AirlineCard air = new AirlineCard();
                air.EditClicked += Airline_EditClicked;
                Frame f = await air.Card(airline);
                AirlineParent.Children.Add(f);
            }
            LoadAirports();
        }

        private async void LoadAirports()
        {
            AirportParent.Children.Clear();
            List<Airport> airports = await HTTP_Controller.GetAirports();
            foreach(Airport airport in airports)
            {
                AirportCard airportCard = new AirportCard();
                airportCard.EditClicked += AirportCard_EditClicked;
                Frame f = await airportCard.Card(airport);
                AirportParent.Children.Add(f);
            }
        }

        private EventHandler AirportCard_EditClicked(Airport airport)
        {
            EditAirportModal editAirportModal = new EditAirportModal(airport);
            Navigation.PushAsync(editAirportModal);
            return null;
        }

        private EventHandler Airline_EditClicked(Airline airline)
        {
            EditAirlineModal editAirlineModal = new EditAirlineModal(airline);
            Navigation.PushAsync(editAirlineModal);
            return null;
        }

        private EventHandler AircraftType_EditClicked(AircraftType aircraftType)
        {
            EditAircraftTypeModal editAircraftTypeModal = new EditAircraftTypeModal(aircraftType);
            Navigation.PushAsync(editAircraftTypeModal);
            return null;
        }

        private EventHandler Manufactorer_EditClicked(Manufactorer manufactorer)
        {
            EditManufactorerModal editManufactorerModal = new EditManufactorerModal(manufactorer);
            Navigation.PushAsync(editManufactorerModal);

            return null;
        }

        private void AddCountry_Clicked(object sender, EventArgs e)
        {
            EditCountryModal editCountryModal = new EditCountryModal();
            Navigation.PushAsync(editCountryModal);
        }

        private EventHandler CountryCard_EditClicked(Country country)
        {
            EditCountryModal editCountryModal = new EditCountryModal(country);
            Navigation.PushAsync(editCountryModal);

            return null;
        }
    }
}