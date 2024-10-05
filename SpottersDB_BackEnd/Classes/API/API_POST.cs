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
            app.MapPost("/Post/Airport", (HttpRequest req) => Post_Airport(req));

            // Post Airline Route

            // Post AircraftType Route

            // Post Manufactorer Route

            // Post Aircraft Route

            // Post SpottingTrip Route

            // Post SpottingPicture Route
        }

        private async void Post_Country(HttpRequest req)
        {
            IFormCollection form = await req.ReadFormAsync();
            Country country = new Country(form["ICAO"], form["Name"]);
            sqlcontroller.AddCountry(country);
        }

        private async void Post_Airport(HttpRequest req)
        {
            IFormCollection form = await req.ReadFormAsync();
            Airport airport = new Airport(form["ICAO"], form["IATA"], form["Name"], form["Description"], form["City"], Convert.ToInt32(form["Country"]));
            sqlcontroller.AddAirport(airport);
        }
    }
}
