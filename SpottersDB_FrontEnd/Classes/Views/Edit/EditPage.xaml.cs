﻿using SpottersDB_FrontEnd.Classes.Structure;
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
            AddAircraft.Clicked += AddAircraft_Clicked;
            AddSpottingTrip.Clicked += AddSpottingTrip_Clicked;
            AddSpottingPicture.Clicked += AddSpottingPicture_Clicked;
        }

        private void AddSpottingPicture_Clicked(object sender, EventArgs e)
        {
            EditSpottingPictureModal editSpottingPictureModal = new EditSpottingPictureModal();
            Navigation.PushAsync(editSpottingPictureModal);
        }

        private void AddSpottingTrip_Clicked(object sender, EventArgs e)
        {
            EditSpottingTripModal editSpottingTripModal = new EditSpottingTripModal();
            Navigation.PushAsync(editSpottingTripModal);
        }

        private void AddAircraft_Clicked(object sender, EventArgs e)
        {
            EditAircraftModal editAircraftModal = new EditAircraftModal();
            Navigation.PushAsync(editAircraftModal);
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
            if(countries != null)
            {
                foreach (Country country in countries)
                {
                    CountryCard countryCard = new CountryCard();
                    countryCard.EditClicked += CountryCard_EditClicked;
                    countryCard.DeleteClicked += CountryCard_DeleteClicked;
                    if (country.icaO_Code == "")
                    {
                        RegionParent.Children.Add(countryCard.Card(country));
                    }
                    else
                    {
                        string URL = await HTTP_Controller.GetNewestPhotoFromCountry(country.id);
                        if(URL == "")
                        {
                            CountryParent.Children.Add(countryCard.Card(country));
                        }
                        else
                        {
                            CountryParent.Children.Add(countryCard.Card(country, URL));
                        }
                    }
                }
            }

            LoadManufactorers();
        }

        private EventHandler CountryCard_DeleteClicked(Country country)
        {
            DeleteCountry(country);
            return null;
        }

        private async void DeleteCountry(Country country)
        {
            string action = await DisplayActionSheet("Delete " + country.name + "?", "Cancle", "Delete");
            if(action == "Delete")
            {
                if (await HTTP_Controller.DeleteCountry(country))
                {
                    LoadEverything();
                }
                else
                {
                    await DisplayAlert("Error", "Deletion failed. The deleted country is probably still referenced somewhere", "OK");
                }
            }
        }

        private async void DeleteAirline(Airline airline)
        {
            string action = await DisplayActionSheet("Delete " + airline.name + "?", "Cancle", "Delete");
            if (action == "Delete")
            {
                if (await HTTP_Controller.DeleteAirline(airline))
                {
                    LoadEverything();
                }
                else
                {
                    await DisplayAlert("Error", "Deletion failed. The deleted country is probably still referenced somewhere", "OK");
                }
            }
        }

        private async void DeleteAirport(Airport airport)
        {
            string action = await DisplayActionSheet("Delete " + airport.name + "?", "Cancle", "Delete");
            if (action == "Delete")
            {
                if (await HTTP_Controller.DeleteAirport(airport))
                {
                    LoadEverything();
                }
                else
                {
                    await DisplayAlert("Error", "Deletion failed. The deleted country is probably still referenced somewhere", "OK");
                }
            }
        }

        private async void DeleteAircraftType(AircraftType aircraftType)
        {
            string action = await DisplayActionSheet("Delete " + aircraftType.fullName + "?", "Cancle", "Delete");
            if (action == "Delete")
            {
                if (await HTTP_Controller.DeleteAircraftType(aircraftType))
                {
                    LoadEverything();
                }
                else
                {
                    await DisplayAlert("Error", "Deletion failed. The deleted country is probably still referenced somewhere", "OK");
                }
            }
        }

        private async void DeleteManufactorer(Manufactorer manufactorer)
        {
            string action = await DisplayActionSheet("Delete " + manufactorer.name + "?", "Cancle", "Delete");
            if (action == "Delete")
            {
                if (await HTTP_Controller.DeleteManufactorer(manufactorer))
                {
                    LoadEverything();
                }
                else
                {
                    await DisplayAlert("Error", "Deletion failed. The deleted country is probably still referenced somewhere", "OK");
                }
            }
        }

        private async void DeleteAircraft(Aircraft aircraft)
        {
            string action = await DisplayActionSheet("Delete " + aircraft.registration + "?", "Cancle", "Delete");
            if (action == "Delete")
            {
                if (await HTTP_Controller.DeleteAircraft(aircraft))
                {
                    LoadEverything();
                }
                else
                {
                    await DisplayAlert("Error", "Deletion failed. The deleted country is probably still referenced somewhere", "OK");
                }
            }
        }

        private async void DeleteSpottingPicture(SpottingPicture spottingPicture)
        {
            string action = await DisplayActionSheet("Delete " + spottingPicture.name + "?", "Cancle", "Delete");
            if (action == "Delete")
            {
                if (await HTTP_Controller.DeleteSpottingPicture(spottingPicture))
                {
                    LoadEverything();
                }
                else
                {
                    await DisplayAlert("Error", "Deletion failed. The deleted country is probably still referenced somewhere", "OK");
                }
            }
        }

        private async void DeleteSpottingTrip(SpottingTrip spottingTrip)
        {
            string action = await DisplayActionSheet("Delete " + spottingTrip.name + "?", "Cancle", "Delete");
            if (action == "Delete")
            {
                if (await HTTP_Controller.DeleteSpottingTrip(spottingTrip))
                {
                    LoadEverything();
                }
                else
                {
                    await DisplayAlert("Error", "Deletion failed. The deleted country is probably still referenced somewhere", "OK");
                }
            }
        }

        private async void LoadManufactorers()
        {
            ManufactorerParent.Children.Clear();

            List<Manufactorer> manufactorers = await HTTP_Controller.GetManufactorers();
            if(manufactorers != null)
            {
                foreach (Manufactorer manufactorer in manufactorers)
                {
                    ManufactorerCard man = new ManufactorerCard();
                    man.EditClicked += Manufactorer_EditClicked;
                    man.DeleteClicked += Man_DeleteClicked;
                    Border b = await man.Card(manufactorer);
                    ManufactorerParent.Children.Add(b);
                }
            }
            LoadAircraftTypes();
        }

        private EventHandler Man_DeleteClicked(Manufactorer manufactorer)
        {
            DeleteManufactorer(manufactorer);
            return null;
        }

        private async void LoadAircraftTypes()
        {
            AircraftTypeParent.Children.Clear();
            List<AircraftType> aircraftTypes = await HTTP_Controller.GetAircraftTypes();
            if(aircraftTypes != null)
            {
                foreach (AircraftType aircraftType in aircraftTypes)
                {
                    AircraftTypeCard air = new AircraftTypeCard();
                    air.EditClicked += AircraftType_EditClicked;
                    air.DeleteClicked += AircraftType_Deleteclicked;
                    Border b = await air.Card(aircraftType);
                    AircraftTypeParent.Children.Add(b);
                }
            }
            LoadAirlines();
        }

        private EventHandler AircraftType_Deleteclicked(AircraftType aircraftType)
        {
            DeleteAircraftType(aircraftType);
            return null;
        }

        private async void LoadAirlines()
        {
            AirlineParent.Children.Clear();
            List<Airline> airlines = await HTTP_Controller.GetAirlines();
            if(airlines != null)
            {
                foreach (Airline airline in airlines)
                {
                    AirlineCard air = new AirlineCard();
                    air.EditClicked += Airline_EditClicked;
                    air.DeleteClicked += Airline_DeleteClicked;
                    Border b = await air.Card(airline);
                    AirlineParent.Children.Add(b);
                }
            }
            LoadAirports();
        }

        private EventHandler Airline_DeleteClicked(Airline airline)
        {
            DeleteAirline(airline);
            return null;
        }

        private async void LoadAirports()
        {
            AirportParent.Children.Clear();
            List<Airport> airports = await HTTP_Controller.GetAirports();
            foreach(Airport airport in airports)
            {
                AirportCard airportCard = new AirportCard();
                airportCard.EditClicked += AirportCard_EditClicked;
                airportCard.DeleteClicked += AirportCard_DeleteClicked;
                Border b = await airportCard.Card(airport);
                AirportParent.Children.Add(b);
            }
            LoadAircrafts();
        }

        private EventHandler AirportCard_DeleteClicked(Airport airport)
        {
            DeleteAirport(airport);
            return null;
        }

        private async void LoadAircrafts()
        {
            AircraftParent.Children.Clear();
            List<Aircraft> aircrafts = await HTTP_Controller.GetAircrafts();
            if( aircrafts != null )
            {
                foreach (Aircraft aircraft in aircrafts)
                {
                    AircraftCard aircraftCard = new AircraftCard();
                    aircraftCard.EditClicked += AircraftCard_EditClicked;
                    aircraftCard.DeleteClicked += AircraftCard_DeleteClicked;
                    Border b = await aircraftCard.Card(aircraft);
                    AircraftParent.Children.Add(b);
                }
            }
            LoadSpottingTrips();
        }

        private EventHandler AircraftCard_DeleteClicked(Aircraft aircraft)
        {
            DeleteAircraft(aircraft);
            return null;
        }

        private async void LoadSpottingTrips()
        {
            SpottingTripParent.Children.Clear();
            List<SpottingTrip> spottingTrips = await HTTP_Controller.GetSpottingTrips();
            if( spottingTrips != null )
            {
                foreach (SpottingTrip spottingTrip in spottingTrips)
                {
                    SpottingTripCard spottingTripCard = new SpottingTripCard();
                    spottingTripCard.EditClicked += SpottingTripCard_EditClicked;
                    spottingTripCard.DeleteClicked += SpottingTripCard_DeleteClicked;
                    Border b = await spottingTripCard.Card(spottingTrip);
                    SpottingTripParent.Children.Add(b);
                }
            }

            LoadSpottingPictures();
        }

        private EventHandler SpottingTripCard_DeleteClicked(SpottingTrip spottingtrip)
        {
            DeleteSpottingTrip(spottingtrip);
            return null;
        }

        private async void LoadSpottingPictures()
        {
            SpottingPictureParent.Children.Clear();
            List<SpottingPicture> SpottingPictures = await HTTP_Controller.GetSpottingPictures();
            if(SpottingPictures != null)
            {
                foreach (SpottingPicture spottingPicture in SpottingPictures)
                {
                    SpottingPictureCard spottingPictureCard = new SpottingPictureCard();
                    Border b = await spottingPictureCard.Card(spottingPicture);
                    spottingPictureCard.EditClicked += SpottingPictureCard_EditClicked;
                    spottingPictureCard.DeleteClicked += SpottingPictureCard_DeleteClicked;
                    SpottingPictureParent.Children.Add(b);
                }
            }
        }

        private EventHandler SpottingPictureCard_DeleteClicked(SpottingPicture spottingPicture)
        {
            DeleteSpottingPicture(spottingPicture);
            return null;
        }

        private EventHandler SpottingPictureCard_EditClicked(SpottingPicture spottingPicture)
        {
            EditSpottingPictureModal editSpottingPictureModal = new EditSpottingPictureModal(spottingPicture);
            Navigation.PushAsync(editSpottingPictureModal);
            return null;
        }

        private EventHandler SpottingTripCard_EditClicked(SpottingTrip spottingTrip, List<Airport> SelectedAirport)
        {
            EditSpottingTripModal editSpottingTripModal = new EditSpottingTripModal(spottingTrip, SelectedAirport);
            Navigation.PushAsync(editSpottingTripModal);
            return null;
        }

        private EventHandler AircraftCard_EditClicked(Aircraft aircraft)
        {
            EditAircraftModal editAircraftModal = new EditAircraftModal(aircraft);
            Navigation.PushAsync(editAircraftModal);
            return null;
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