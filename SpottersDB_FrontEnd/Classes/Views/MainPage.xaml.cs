using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.UI_Elements.Cards;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views
{
    public partial class MainPage : ContentPage
    {   
        public MainPage()
        {
            InitializeComponent();
            AddCountry.Clicked += AddCountry_Clicked;
            AddManufactorer.Clicked += AddManufactorer_Clicked;
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