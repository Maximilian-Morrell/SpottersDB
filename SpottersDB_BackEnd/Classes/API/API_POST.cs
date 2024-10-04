using Microsoft.AspNetCore.Mvc;
using SpottersDB_BackEnd.Classes.Structure;
using SpottersDB_BackEnd.Classes.Utilities;

namespace SpottersDB_BackEnd.Classes.API
{
    public class API_POST : API_Base
    {
        public API_POST(WebApplication application, SQLController sqlcontroller) : base(application, sqlcontroller)
        {

        }

        protected override void MainAPI()
        {
            // Post Country Route
            app.MapPost("/Post/Country", (HttpRequest req) => Post_Country(req));
            // Post Airport Route

            // Post Airline Route

            // Post AircraftType Route

            // Post Manufactorer Route

            // Post Aircraft Route

            // Post SpottingTrip Route

            // Post SpottingPictur Route
        }

        private async void Post_Country(HttpRequest req)
        {
            IFormCollection form = await req.ReadFormAsync();
            Country country = new Country(form["ICAO"], form["Name"]);
            sqlcontroller.AddCountry(country);
        }
    }
}
