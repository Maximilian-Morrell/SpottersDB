using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.UI_Elements.Cards;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views
{
    public partial class MainPage : ContentPage
    {
        HTTP_Controller httpController;
        List<Country> countries;
        public MainPage()
        {
            InitializeComponent();
            httpController = new HTTP_Controller();
            LoadEverything();
            AddCountry.Clicked += AddCountry_Clicked;
        }

        public void LoadEverything()
        {
            LoadCountries();
        }

        protected override void OnAppearing()
        {
            LoadEverything();
            base.OnAppearing();
        }

        private async void LoadCountries()
        {

            CountryParent.Children.Clear();

            countries = await httpController.GetCountries();
            foreach(Country country in countries)
            {
                CountryCard countryCard = new CountryCard();
                countryCard.EditClicked += CountryCard_EditClicked;
                CountryParent.Children.Add(countryCard.Card(country));
            }
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