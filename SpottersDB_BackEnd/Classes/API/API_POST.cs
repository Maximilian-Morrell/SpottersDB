using Microsoft.AspNetCore.Mvc;
using SpottersDB_BackEnd.Classes.Structure;
using SpottersDB_BackEnd.Classes.Utilities;
using System.Net;

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
            app.MapPost("/Post/Airline", (HttpRequest req) => Post_Airline(req));

            // Post AircraftType Route

            // Post Manufactorer Route

            // Post Aircraft Route

            // Post SpottingTrip Route

            // Post SpottingPicture Route
        }

        private async void Post_Country(HttpRequest req)
        {
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Country country = new Country(form["ICAO"], form["Name"]);
                sqlcontroller.AddCountry(country);
            }
            catch (Exception e)
            {

            }
        }

        private async void Post_Airport(HttpRequest req)
        {
            IFormCollection form = await req.ReadFormAsync();
            Airport airport = new Airport(form["ICAO"], form["IATA"], form["Name"], form["Description"], form["City"], Convert.ToInt32(form["Country"]));
            sqlcontroller.AddAirport(airport);
        }

        private async void Post_Airline(HttpRequest req)
        {
            IFormCollection form = await req.ReadFormAsync();
            Airline airline = new Airline(form["ICAO"], form["IATA"], form["Name"], form["Location"]);
            sqlcontroller.AddAirline(airline);
        }
    }
}
