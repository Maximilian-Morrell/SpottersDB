using SpottersDB_FrontEnd.Classes.Structure;
using System;
using System.Collections.Generic;
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
        private readonly string _URL = "https://localhost:7090/";
        private HttpClient _httpClient;

        public HTTP_Controller()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_URL);
        }

        public async Task<List<Country>> GetCountries()
        {
            List<Country> countries = new List<Country>();
            try
            {
                HttpResponseMessage respone = await _httpClient.GetAsync("/Get/Countries");
                string content = await respone.Content.ReadAsStringAsync();
                countries = JsonSerializer.Deserialize<List<Country>>(content);
            }
            catch (Exception e)
            {
                
            }
            return countries;
        }
    }
}
