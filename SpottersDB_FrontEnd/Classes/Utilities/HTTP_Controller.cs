using SpottersDB_FrontEnd.Classes.Structure;
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
        private static readonly Uri _URL = new Uri("https://localhost:7090/");

        private static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = _URL;
            return client;
        }

        public static async Task<List<Country>> GetCountries()
        {
            List<Country> countries = new List<Country>();
            HttpClient client = GetHttpClient();
            try
            {
                HttpResponseMessage respone = await client.GetAsync("/Get/Countries");
                string content = await respone.Content.ReadAsStringAsync();
                countries = JsonSerializer.Deserialize<List<Country>>(content);
            }
            catch (Exception e)
            {
                
            }
            return countries;
        }

        public static async Task<HttpResponseMessage> EditCountry(Country country)
        {
            HttpClient client = GetHttpClient();

            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(country.id.ToString()), "ID");
            content.Add(new StringContent(country.icaO_Code), "ICAO");
            content.Add(new StringContent(country.name), "Name");

            HttpResponseMessage response = await client.PostAsync("/Patch/Country", content);
            return response;
        }

        public static async Task<HttpResponseMessage> AddCountry(Country country)
        {
            HttpClient client = GetHttpClient();

            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(country.icaO_Code), "ICAO");
            content.Add(new StringContent(country.name), "Name");

            HttpResponseMessage response = await client.PostAsync("/Post/Country", content);
            return response;
        }

        public static async Task<Country> GetCountryByID(int ID)
        {
            HttpClient client = GetHttpClient();
            Country country = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync("/Get/Country?ID=" +  ID);
                string content = await response.Content.ReadAsStringAsync();
                country = JsonSerializer.Deserialize<Country>(content);
            }
            catch (Exception e)
            {

            }
            return country;
        }

        public static async Task<List<Manufactorer>> GetManufactorers()
        {
            HttpClient client = GetHttpClient();
            List<Manufactorer> manufactorers = new List<Manufactorer>();
            try
            {
                HttpResponseMessage response = await client.GetAsync("/Get/Manufactorers");
                string content = await response.Content.ReadAsStringAsync();
                manufactorers = JsonSerializer.Deserialize<List<Manufactorer>>(content);
            }
            catch (Exception e)
            {

            }
            return manufactorers;
        }
    }
}
