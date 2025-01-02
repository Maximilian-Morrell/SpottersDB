using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Views;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.Utilities
{
    internal class HTTP_Controller
    {
        #region Setup Stuff
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
        #endregion

        #region APICall Methods
        private static async Task<Object> APIGet(string URL, Type ObjType)
        {
            Object test = null;
            try
            {
                HttpClient client = GetHttpClient();
                HttpResponseMessage respone = await client.GetAsync(URL);
                string contentString = await respone.Content.ReadAsStringAsync();
                if(ObjType != typeof(string))
                {
                    test = JsonSerializer.Deserialize(contentString, ObjType);
                }
                else
                {
                    test = contentString;
                }
            }
            catch (Exception)
            {

            }
            return test;
        }

        private static async Task<bool> APIPost(string URL, Dictionary<string, string> ContentData, FileResult file = null)
        {
            HttpResponseMessage message = null;
            string Content = null;
            try
            {
                HttpClient client = GetHttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent();
                foreach(string Key in ContentData.Keys.ToList())
                {
                    content.Add(new StringContent(ContentData[Key]), Key);
                }
                if(file != null)
                {
                    StreamContent Picture = new StreamContent(File.OpenRead(file.FullPath));
                    content.Add(Picture, "File", file.FileName);
                }
                message = await client.PostAsync(URL, content);
                Content = await message.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {

            }

            return Convert.ToBoolean(Content);
        }
        #endregion

        #region Get/PATCH/POST Objects
        public static async Task<List<Country>> GetCountries()
        {
            return (List<Country>) await APIGet("/Get/Countries", new List<Country>().GetType());
        }

        public static async Task<List<Country>> GetCountries(bool OnlyCountries)
        {
            return (List<Country>) await APIGet("/Get/OnlyCountries", new List<Country>().GetType());
        }

        public static async Task<List<Country>> GetRegions()
        {
            return (List<Country>) await APIGet("/Get/Regions", new List<Country>().GetType());
        }

        public static async Task<bool> EditCountry(Country country)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("ID", country.id.ToString());
            pairs.Add("ICAO", country.icaO_Code);
            pairs.Add("Name", country.name);
            return await APIPost("/Patch/Country", pairs);
        }


        public static async Task<bool> AddCountry(Country country)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("ICAO", country.icaO_Code);
            pairs.Add("Name", country.name);
            return await APIPost("/Post/Country", pairs);
        }

        public static async Task<Country> GetCountryByID(int ID)
        {
            return (Country) await APIGet("/Get/Country?ID=" + ID, new Country().GetType());
        }

        public static async Task<List<Manufactorer>> GetManufactorers()
        {
            return (List<Manufactorer>) await APIGet("/Get/Manufactorers", new List<Manufactorer>().GetType());
        }

        public static async Task<bool> UpdateManufactorer(Manufactorer manufactorer)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("ID", manufactorer.id.ToString());
            pairs.Add("Name", manufactorer.name);
            pairs.Add("Region", manufactorer.region.ToString());
            return await APIPost("/Patch/Manufactorer", pairs);
        }

        public static async Task<bool> AddNewManufactorer(Manufactorer manufactorer)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("Name", manufactorer.name);
            pairs.Add("Region", manufactorer.region.ToString());
            return await APIPost("/Post/Manufactorer", pairs);
        }

        public static async Task<string> GetNewestPhotoFromCountry(int CountryID)
        {
            return (string) await APIGet("/Get/Newest/Country?ID=" + CountryID, typeof(string));
        }

        public static async Task<bool> AddNewAircraftType(AircraftType aircraftType)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("ICAO", aircraftType.icaoCode);
            pairs.Add("FullName", aircraftType.fullName);
            pairs.Add("NickName", aircraftType.nickName);
            pairs.Add("ManufactorerID", aircraftType.manufactorerID.ToString());
            return await APIPost("/Post/AircraftType", pairs);
        }

        public static async Task<Manufactorer> GetManufactorerByID(int ID)
        {
            return (Manufactorer) await APIGet("/Get/Manufactorer?ID=" + ID, new Manufactorer().GetType());
        }

        public static async Task<Airline> GetAirlineByID(int ID)
        {
            return (Airline) await APIGet("/Get/Airline?ID=" + ID, new Airline().GetType());
        }

        public static async Task<AircraftType> GetAircraftTypeByID(int ID)
        {
            return (AircraftType) await APIGet("/Get/AircraftType?ID=" + ID, new AircraftType().GetType());
        }

        public static async Task<List<AircraftType>> GetAircraftTypes()
        {
            return (List<AircraftType>) await APIGet("/Get/AircraftTypes", new List<AircraftType>().GetType());
        }

        public static async Task<bool> UpdateAircraftType(AircraftType aircraftType)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("ID", aircraftType.id.ToString());
            pairs.Add("ICAO", aircraftType.icaoCode);
            pairs.Add("Fullname", aircraftType.fullName);
            pairs.Add("NickName", aircraftType.nickName);
            pairs.Add("ManufactorerID", aircraftType.id.ToString());
            return await APIPost("/Patch/AircraftType", pairs);
        }

        public static async Task<bool> AddNewAirline(Airline airline)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("ICAO", airline.icao);
            pairs.Add("IATA", airline.iata);
            pairs.Add("Name", airline.name);
            pairs.Add("Region", airline.region.ToString());
            return await APIPost("/Post/Airline", pairs);
        }

        public static async Task<bool> UpdateAirline(Airline airline)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("ID", airline.id.ToString());
            pairs.Add("ICAO", airline.icao);
            pairs.Add("IATA", airline.iata);
            pairs.Add("Name", airline.name);
            pairs.Add("Region", airline.region.ToString());
            return await APIPost("/Patch/Airline", pairs);
        }

        public static async Task<List<Airline>> GetAirlines()
        {
            return (List<Airline>) await APIGet("/Get/Airlines", new List<Airline>().GetType());
        }

        public static async Task<bool> AddAirport(Airport airport)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("ICAO", airport.icaO_Code);
            pairs.Add("IATA", airport.iatA_Code);
            pairs.Add("Name", airport.name);
            pairs.Add("Description", airport.description);
            pairs.Add("City", airport.city);
            pairs.Add("Country", airport.countryID.ToString());
            return await APIPost("/Post/Airport", pairs);
        }

        public static async Task<bool> UpdateAirport(Airport airport)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("ID", airport.id.ToString());
            pairs.Add("ICAO", airport.icaO_Code);
            pairs.Add("IATA", airport.iatA_Code);
            pairs.Add("Name", airport.name);
            pairs.Add("Description", airport.description);
            pairs.Add("City", airport.city);
            pairs.Add("Country", airport.countryID.ToString());
            return await APIPost("/Patch/Airport", pairs);
        }

        public static async Task<List<Airport>> GetAirports()
        {
            return (List<Airport>) await APIGet("/Get/Airports", new List<Airport>().GetType());
        }

        public static async Task<Airport> GetAirport(int ID)
        {
            return (Airport) await APIGet("/Get/Airport?ID=" + ID, new Airport().GetType());
        }

        public static async Task<bool> AddNewAircraft(Aircraft aircraft)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("Registration", aircraft.registration);
            pairs.Add("Description", aircraft.description);
            pairs.Add("TypeID", aircraft.typeID.ToString());
            pairs.Add("CountryID", aircraft.countryID.ToString());
            pairs.Add("AirlineID", aircraft.airlineID.ToString());
            return await APIPost("/Post/Aircraft", pairs);
        }

        public static async Task<bool> UpdateAircraft(Aircraft aircraft)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("ID", aircraft.id.ToString());
            pairs.Add("Registration", aircraft.registration);
            pairs.Add("Description", aircraft.description);
            pairs.Add("TypeID", aircraft.typeID.ToString());
            pairs.Add("CountryID", aircraft.countryID.ToString());
            pairs.Add("AirlineID", aircraft.airlineID.ToString());
            return await APIPost("/Patch/Aircraft", pairs);
        }

        public static async Task<List<Aircraft>> GetAircrafts()
        {
            return (List<Aircraft>) await APIGet("/Get/Aircrafts", new List<Aircraft>().GetType());
        }

        public static async Task<Aircraft> GetAircraft(int ID)
        {
            return (Aircraft) await APIGet("/Get/Aircraft?ID=" + ID, new Aircraft().GetType());
        }

        public static async Task<List<Airport>> GetAirportsFromSpottingTrip(int ID)
        {
            return (List<Airport>) await APIGet("/Get/Airports/SpottingTrip?ID=" + ID, new List<Airport>().GetType());
        }

        public static async Task<bool> AddNewSpottingTrip(SpottingTrip spottingTrip)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("Name", spottingTrip.name);
            pairs.Add("Description", spottingTrip.description);
            pairs.Add("Start", spottingTrip.start.ToString());
            pairs.Add("End", spottingTrip.end.ToString());
            pairs.Add("AirportIDs", spottingTrip.AirportIDs.ToString());
            return await APIPost("/Post/SpottingTrip", pairs);
        }

        public static async Task<bool> UpdateSpottingTrip(SpottingTrip spottingTrip)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("ID", spottingTrip.id.ToString());
            pairs.Add("Name", spottingTrip.name);
            pairs.Add("Description", spottingTrip.description);
            pairs.Add("Start", spottingTrip.start.ToString());
            pairs.Add("End", spottingTrip.end.ToString());
            pairs.Add("AirportIDs", spottingTrip.AirportIDs.ToString());
            return await APIPost("/Patch/SpottingTrip", pairs);
        }

        public static async Task<List<SpottingTrip>> GetSpottingTrips()
        {
            return (List<SpottingTrip>) await APIGet("/Get/SpottingTrips", new List<SpottingTrip>().GetType());
        }

        public static async Task<SpottingTrip> GetSpottingTrip(int ID)
        {
            return (SpottingTrip) await APIGet("/Get/SpottingTrip?ID=" + ID, new SpottingTrip().GetType());
        }

        public static async Task<int> GetLinkID(int SpottingTripID, int AirportID)
        {
            return (int) await APIGet($"/Get/SpottingTripAirport/LinkID?SpottingTripID={SpottingTripID}&AirportID={AirportID}", typeof(int));
        }

        public static async Task<Dictionary<string, int>> GetSpottingTripAirport(int LinkID)
        {
            return (Dictionary<string, int>) await APIGet($"/Get/SpottingTripAirport/SpottingTripAirport?LinkID={LinkID}", new Dictionary<string, int>().GetType());
        }

        public static async Task<bool> UpdateSpottingPicture(SpottingPicture spottingPicture)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("ID", spottingPicture.id.ToString());
            pairs.Add("Name", spottingPicture.name);
            pairs.Add("Description", spottingPicture.description);
            pairs.Add("PictureURL", spottingPicture.pictureUrl);
            pairs.Add("OriginalFileName", spottingPicture.originalFileName);
            pairs.Add("SpottingTripAirport", spottingPicture.spottingTripAirportID.ToString());
            pairs.Add("AircraftID", spottingPicture.aircraftID.ToString());
            return await APIPost("/Patch/SpottingPicture", pairs);
        }

        public static async Task<bool> UpdateSpottingPicture(SpottingPicture spottingPicture, FileResult file)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("ID", spottingPicture.id.ToString());
            pairs.Add("Name", spottingPicture.name);
            pairs.Add("Description", spottingPicture.description);
            pairs.Add("PictureURL", spottingPicture.pictureUrl);
            pairs.Add("OriginalFileName", spottingPicture.originalFileName);
            pairs.Add("SpottingTripAirport", spottingPicture.spottingTripAirportID.ToString());
            pairs.Add("AircraftID", spottingPicture.aircraftID.ToString());
            return await APIPost("/Patch/SpottingPicture", pairs, file);
        }

        public static async Task<bool> AddNewSpottingPicture(SpottingPicture spottingPicture, FileResult file)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("Name", spottingPicture.name);
            pairs.Add("Description", spottingPicture.description);
            pairs.Add("SpottingTripAirport", spottingPicture.spottingTripAirportID.ToString());
            pairs.Add("AircraftID", spottingPicture.aircraftID.ToString());
            return await APIPost("/Post/SpottingPicture", pairs, file);
        }

        public static async Task<List<SpottingPicture>> GetSpottingPictures()
        {
            return (List<SpottingPicture>) await APIGet("/Get/SpottingPictures", new List<SpottingPicture>().GetType());
        }
        #endregion

        #region Delete
        public static async Task<bool> DeleteCountry(Country c)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("ID", c.name);
            return await APIPost("/Delete/Country", pairs);
        }


        #endregion
    }
}
