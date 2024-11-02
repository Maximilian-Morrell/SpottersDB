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
    }
}
