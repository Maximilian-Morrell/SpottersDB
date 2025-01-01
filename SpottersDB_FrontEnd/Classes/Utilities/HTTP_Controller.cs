using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Views;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.Utilities
{
    internal class HTTP_Controller
    {
        private static readonly Uri _URL = new Uri("http://localhost:5032/");

        private static HttpClient GetHttpClient()
        {
            HttpClient client = null;
            try
            {
                HttpClientHandler httpHandler = new HttpClientHandler();
                httpHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                client = new HttpClient(httpHandler);
                client.BaseAddress = _URL;
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return client;
        }

        public static async Task<List<Country>> GetCountries()
        {
            List<Country> countries = new List<Country>();
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage respone = await client.GetAsync("/Get/Countries");
                string content = await respone.Content.ReadAsStringAsync();
                countries = JsonSerializer.Deserialize<List<Country>>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return countries;
        }

        public static async Task<List<Country>> GetCountries(bool OnlyCountries)
        {
            List<Country> countries = new List<Country>();
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage respone = await client.GetAsync("/Get/OnlyCountries");
                string content = await respone.Content.ReadAsStringAsync();
                countries = JsonSerializer.Deserialize<List<Country>>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return countries;
        }

        public static async Task<List<Country>> GetRegions()
        {
            List<Country> countries = new List<Country>();
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage respone = await client.GetAsync("/Get/Regions");
                string content = await respone.Content.ReadAsStringAsync();
                countries = JsonSerializer.Deserialize<List<Country>>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return countries;
        }

        public static async Task<HttpResponseMessage> EditCountry(Country country)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();

                content.Add(new StringContent(country.id.ToString()), "ID");
                content.Add(new StringContent(country.icaO_Code), "ICAO");
                content.Add(new StringContent(country.name), "Name");

                response = await client.PostAsync("/Patch/Country", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return response;
        }

        public static async Task<HttpResponseMessage> AddCountry(Country country)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();

                content.Add(new StringContent(country.icaO_Code), "ICAO");
                content.Add(new StringContent(country.name), "Name");

                response = await client.PostAsync("/Post/Country", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return response;
        }

        public static async Task<Country> GetCountryByID(int ID)
        {
            Country country = null;
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage response = await client.GetAsync("/Get/Country?ID=" +  ID);
                string content = await response.Content.ReadAsStringAsync();
                country = JsonSerializer.Deserialize<Country>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return country;
        }

        public static async Task<List<Manufactorer>> GetManufactorers()
        {
            List<Manufactorer> manufactorers = new List<Manufactorer>();
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage response = await client.GetAsync("/Get/Manufactorers");
                string content = await response.Content.ReadAsStringAsync();
                manufactorers = JsonSerializer.Deserialize<List<Manufactorer>>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return manufactorers;
        }

        public static async Task<HttpResponseMessage> UpdateManufactorer(Manufactorer manufactorer)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();
                content.Add(new StringContent(manufactorer.id.ToString()), "ID");
                content.Add(new StringContent(manufactorer.name), "Name");
                content.Add(new StringContent(manufactorer.region.ToString()), "Region");

                response = await client.PostAsync("/Patch/Manufactorer", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return response;
        }

        public static async Task<HttpResponseMessage> AddNewManufactorer(Manufactorer manufactorer)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();

                content.Add(new StringContent(manufactorer.name), "Name");
                content.Add(new StringContent(manufactorer.region.ToString()), "Region");

                response = await client.PostAsync("/Post/Manufactorer", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return response;
        }

        public static async Task<string> GetNewestPhotoFromCountry(int CountryID)
        {
            string URL = "";
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage response = await client.GetAsync("/Get/Newest/Country?ID=" + CountryID);
                URL = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return URL;
        }

        public static async Task<HttpResponseMessage> AddNewAircraftType(AircraftType aircraftType)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();

                content.Add(new StringContent(aircraftType.icaoCode), "ICAO");
                content.Add(new StringContent(aircraftType.fullName), "FullName");
                content.Add(new StringContent(aircraftType.nickName), "NickName");
                content.Add(new StringContent(aircraftType.manufactorerID.ToString()), "ManufactorerID");

                response = await client.PostAsync("/Post/AircraftType", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return response;
        }

        public static async Task<Manufactorer> GetManufactorerByID(int ID)
        {
            Manufactorer manufactorer = null;
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage response = await client.GetAsync("/Get/Manufactorer?ID=" + ID);
                string content = await response.Content.ReadAsStringAsync();
                manufactorer = JsonSerializer.Deserialize<Manufactorer>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return manufactorer;
        }

        public static async Task<Airline> GetAirlineByID(int ID)
        {
            Airline airline = null;
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage response = await client.GetAsync("/Get/Airline?ID=" + ID);
                string content = await response.Content.ReadAsStringAsync();
                airline = JsonSerializer.Deserialize<Airline>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return airline;
        }

        public static async Task<AircraftType> GetAircraftTypeByID(int ID)
        {
            AircraftType type = null;
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage response = await client.GetAsync("/Get/AircraftType?ID=" + ID);
                string content = await response.Content.ReadAsStringAsync();
                type = JsonSerializer.Deserialize<AircraftType>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return type;
        }

        public static async Task<List<AircraftType>> GetAircraftTypes()
        {
            List<AircraftType> aircraftTypes = new List<AircraftType>();
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage response = await client.GetAsync("/Get/AircraftTypes");
                string content = await response.Content.ReadAsStringAsync();
                aircraftTypes = JsonSerializer.Deserialize<List<AircraftType>>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return aircraftTypes;
        }

        public static async Task<HttpResponseMessage> UpdateAircraftType(AircraftType aircraftType)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();
                content.Add(new StringContent(aircraftType.id.ToString()), "ID");
                content.Add(new StringContent(aircraftType.icaoCode), "ICAO");
                content.Add(new StringContent(aircraftType.fullName), "FullName");
                content.Add(new StringContent(aircraftType.nickName), "NickName");
                content.Add(new StringContent(aircraftType.manufactorerID.ToString()), "ManufactorerID");

                response = await client.PostAsync("/Patch/AircraftType", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return response;
        }

        public static async Task<HttpResponseMessage> AddNewAirline(Airline airline)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();

                content.Add(new StringContent(airline.icao), "ICAO");
                content.Add(new StringContent(airline.iata), "IATA");
                content.Add(new StringContent(airline.name), "Name");
                content.Add(new StringContent(airline.region.ToString()), "Region");

                response = await client.PostAsync("/Post/Airline", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return response;
        }

        public static async Task<HttpResponseMessage> UpdateAirline(Airline airline)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();
                content.Add(new StringContent(airline.id.ToString()), "ID");
                content.Add(new StringContent(airline.icao), "ICAO");
                content.Add(new StringContent(airline.iata), "IATA");
                content.Add(new StringContent(airline.name), "Name");
                content.Add(new StringContent(airline.region.ToString()), "Region");

                response = await client.PostAsync("/Patch/Airline", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return response;
        }

        public static async Task<List<Airline>> GetAirlines()
        {
            List<Airline> airlines = new List<Airline>();
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage respone = await client.GetAsync("/Get/Airlines");
                string content = await respone.Content.ReadAsStringAsync();
                airlines = JsonSerializer.Deserialize<List<Airline>>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return airlines;
        }

        public static async Task<HttpResponseMessage> AddAirport(Airport airport)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();
                content.Add(new StringContent(airport.icaO_Code), "ICAO");
                content.Add(new StringContent(airport.iatA_Code), "IATA");
                content.Add(new StringContent(airport.name), "Name");
                content.Add(new StringContent(airport.description), "Description");
                content.Add(new StringContent(airport.city), "City");
                content.Add(new StringContent(airport.countryID.ToString()), "Country");

                response = await client.PostAsync("/Post/Airport", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }

            return response;
        }

        public static async Task<HttpResponseMessage> UpdateAirport(Airport airport)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();
                content.Add(new StringContent(airport.id.ToString()), "ID");
                content.Add(new StringContent(airport.icaO_Code), "ICAO");
                content.Add(new StringContent(airport.iatA_Code), "IATA");
                content.Add(new StringContent(airport.name), "Name");
                content.Add(new StringContent(airport.description), "Description");
                content.Add(new StringContent(airport.city), "City");
                content.Add(new StringContent(airport.countryID.ToString()), "Country");

                response = await client.PostAsync("/Patch/Airport", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }

            return response;
        }

        public static async Task<List<Airport>> GetAirports()
        {
            List<Airport> airports = new List<Airport>();
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage respone = await client.GetAsync("/Get/Airports");
                string content = await respone.Content.ReadAsStringAsync();
                airports = JsonSerializer.Deserialize<List<Airport>>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return airports;
        }

        public static async Task<Airport> GetAirport(int ID)
        {
            Airport airport = null;
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage respone = await client.GetAsync("/Get/Airport?ID=" + ID);
                string content = await respone.Content.ReadAsStringAsync();
                airport = JsonSerializer.Deserialize<Airport>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return airport;
        }

        public static async Task<HttpResponseMessage> AddNewAircraft(Aircraft aircraft)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();

                content.Add(new StringContent(aircraft.registration), "Registration");
                content.Add(new StringContent(aircraft.description), "Description");
                content.Add(new StringContent(aircraft.typeID.ToString()), "TypeID");
                content.Add(new StringContent(aircraft.countryID.ToString()), "CountryID");
                content.Add(new StringContent(aircraft.airlineID.ToString()), "AirlineID");

                response = await client.PostAsync("/Post/Aircraft", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return response;
        }

        public static async Task<HttpResponseMessage> UpdateAircraft(Aircraft aircraft)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();

                content.Add(new StringContent(aircraft.id.ToString()), "ID");
                content.Add(new StringContent(aircraft.registration), "Registration");
                content.Add(new StringContent(aircraft.description), "Description");
                content.Add(new StringContent(aircraft.typeID.ToString()), "TypeID");
                content.Add(new StringContent(aircraft.countryID.ToString()), "CountryID");
                content.Add(new StringContent(aircraft.airlineID.ToString()), "AirlineID");

                response = await client.PostAsync("/Patch/Aircraft", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return response;
        }

        public static async Task<List<Aircraft>> GetAircrafts()
        {
            List<Aircraft> aircrafts = new List<Aircraft>();
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage respone = await client.GetAsync("/Get/Aircrafts");
                string content = await respone.Content.ReadAsStringAsync();
                aircrafts = JsonSerializer.Deserialize<List<Aircraft>>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return aircrafts;
        }

        public static async Task<Aircraft> GetAircraft(int ID)
        {
            Aircraft aircraft = null;
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage respone = await client.GetAsync("/Get/Aircraft?ID="+ ID);
                string content = await respone.Content.ReadAsStringAsync();
                aircraft = JsonSerializer.Deserialize<Aircraft>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return aircraft;
        }

        public static async Task<List<Airport>> GetAirportsFromSpottingTrip(int ID)
        {
            List<Airport> airports = new List<Airport>();
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage respone = await client.GetAsync("/Get/Airports/SpottingTrip?ID="+ ID);
                string content = await respone.Content.ReadAsStringAsync();
                airports = JsonSerializer.Deserialize<List<Airport>>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return airports;
        }

        public static async Task<HttpResponseMessage> AddNewSpottingTrip(SpottingTrip spottingTrip)
        {
            Thread.Sleep(500);
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();

                content.Add(new StringContent(spottingTrip.name), "Name");
                content.Add(new StringContent(spottingTrip.description), "Description");
                content.Add(new StringContent(spottingTrip.start.ToString()), "Start");
                content.Add(new StringContent(spottingTrip.end.ToString()), "End");
                content.Add(new StringContent(spottingTrip.AirportIDs), "AirportID");

                response = await client.PostAsync("/Post/SpottingTrip", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return response;
        }

        public static async Task<HttpResponseMessage> UpdateSpottingTrip(SpottingTrip spottingTrip)
        {
            Thread.Sleep(500);
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();

                content.Add(new StringContent(spottingTrip.id.ToString()), "ID");
                content.Add(new StringContent(spottingTrip.name), "Name");
                content.Add(new StringContent(spottingTrip.description), "Description");
                content.Add(new StringContent(spottingTrip.start.ToString()), "Start");
                content.Add(new StringContent(spottingTrip.end.ToString()), "End");
                content.Add(new StringContent(spottingTrip.AirportIDs), "AirportID");

                response = await client.PostAsync("/Patch/SpottingTrip", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return response;
        }

        public static async Task<List<SpottingTrip>> GetSpottingTrips()
        {
            List<SpottingTrip> spottingTrips = new List<SpottingTrip>();
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage respone = await client.GetAsync("/Get/SpottingTrips");
                string content = await respone.Content.ReadAsStringAsync();
                spottingTrips = JsonSerializer.Deserialize<List<SpottingTrip>>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return spottingTrips;
        }

        public static async Task<SpottingTrip> GetSpottingTrip(int ID)
        {
            SpottingTrip spottingTrip = null;
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage respone = await client.GetAsync("/Get/SpottingTrip?ID=" + ID);
                string content = await respone.Content.ReadAsStringAsync();
                spottingTrip = JsonSerializer.Deserialize<SpottingTrip>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return spottingTrip;
        }

        public static async Task<int> GetLinkID(int SpottingTripID, int AirportID)
        {
            int LinkID = -1;
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage respone = await client.GetAsync($"/Get/SpottingTripAirport/LinkID?SpottingTripID={SpottingTripID}&AirportID={AirportID}");
                string content = await respone.Content.ReadAsStringAsync();
                LinkID = JsonSerializer.Deserialize<int>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return LinkID;
        }

        public static async Task<List<int>> GetSpottingTripAirport(int LinkID)
        {
            List<int> SpottingTripAirport = new List<int>();
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage respone = await client.GetAsync($"/Get/SpottingTripAirport/SpottingTripAirport?LinkID={LinkID}");
                string content = await respone.Content.ReadAsStringAsync();
                SpottingTripAirport = JsonSerializer.Deserialize<List<int>>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return SpottingTripAirport;
        }

        public static async Task<HttpResponseMessage> UpdateSpottingPicture(SpottingPicture spottingPicture)
        {
            Thread.Sleep(500);
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();

                content.Add(new StringContent(spottingPicture.id.ToString()), "ID");
                content.Add(new StringContent(spottingPicture.name), "Name");
                content.Add(new StringContent(spottingPicture.description), "Description");
                content.Add(new StringContent(spottingPicture.pictureUrl), "PictureURL");
                content.Add(new StringContent(spottingPicture.originalFileName), "OriginalFileName");
                content.Add(new StringContent(spottingPicture.spottingTripAirportID.ToString()), "SpottingTripAirport");
                content.Add(new StringContent(spottingPicture.spottingTripAirportID.ToString()), "AircraftID");

                response = await client.PostAsync("/Patch/SpottingPicture", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return response;
        }

        public static async Task<HttpResponseMessage> UpdateSpottingPicture(SpottingPicture spottingPicture, FileResult file)
        {
            Thread.Sleep(500);
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();

                content.Add(new StringContent(spottingPicture.id.ToString()), "ID");
                content.Add(new StringContent(spottingPicture.name), "Name");
                content.Add(new StringContent(spottingPicture.description), "Description");
                content.Add(new StringContent(spottingPicture.pictureUrl), "PictureURL");
                content.Add(new StringContent(spottingPicture.spottingTripAirportID.ToString()), "SpottingTripAirport");
                content.Add(new StringContent(spottingPicture.spottingTripAirportID.ToString()), "AircraftID");
                StreamContent Picture = new StreamContent(File.OpenRead(file.FullPath));
                content.Add(Picture, "File", file.FileName);

                response = await client.PostAsync("/Patch/SpottingPicture", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return response;
        }

        public static async Task<HttpResponseMessage> AddNewSpottingPicture(SpottingPicture spottingPicture, FileResult file)
        {
            Thread.Sleep(500);
            MultipartFormDataContent content = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = GetHttpClient();

                content.Add(new StringContent(spottingPicture.name), "Name");
                content.Add(new StringContent(spottingPicture.description), "Description");
                content.Add(new StringContent(spottingPicture.spottingTripAirportID.ToString()), "SpottingTripAirport");
                content.Add(new StringContent(spottingPicture.aircraftID.ToString()), "AircraftID");
                StreamContent Picture = new StreamContent(File.OpenRead(file.FullPath));
                content.Add(Picture, "File", file.FileName);

                response = await client.PostAsync("/Post/SpottingPicture", content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return response;
        }

        public static async Task<List<SpottingPicture>> GetSpottingPictures()
        {
            List<SpottingPicture> spottingPictures = new List<SpottingPicture>();
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage respone = await client.GetAsync("/Get/SpottingPictures");
                string content = await respone.Content.ReadAsStringAsync();
                spottingPictures = JsonSerializer.Deserialize<List<SpottingPicture>>(content);
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            return spottingPictures;
        }

        public static async Task<bool> DeleteCountry(Country c)
        {
            HttpResponseMessage message = null;
            string Content = null;
            try
            {
                HttpClient client = GetHttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StringContent(c.id.ToString()), "ID");
                message = await client.PostAsync("/Delete/Country", content);
                Content = await message.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Window w = new Window(new ErrorBox(e.StackTrace, e.InnerException.Message));
                Application.Current.OpenWindow(w);
            }

            return Convert.ToBoolean(Content);
        }
    }
}
