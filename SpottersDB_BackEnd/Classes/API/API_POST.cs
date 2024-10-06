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

            // Post Manufactorer Route
            app.MapPost("/Post/Manufactorer", (HttpRequest req) => Post_Manufactorer(req));

            // Post AircraftType Route
            app.MapPost("/Post/AircraftType", (HttpRequest req) => Post_AircraftType(req));

            // Post Aircraft Route
            app.MapPost("/Post/Aircraft", (HttpRequest req) => Post_Aircraft(req));

            // Post SpottingTrip Route
            app.MapPost("/Post/SpottingTrip", (HttpRequest req) => Post_SpottingTrip(req));

            // Post SpottingPicture Route
            app.MapPost("/Post/SpottingPicture", (HttpRequest req) => Post_SpottingPicture(req));
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
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Airport airport = new Airport(form["ICAO"], form["IATA"], form["Name"], form["Description"], form["City"], Convert.ToInt32(form["Country"]));
                sqlcontroller.AddAirport(airport);
            }
            catch (Exception e)
            {

            }
        }

        private async void Post_Airline(HttpRequest req)
        {
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Airline airline = new Airline(form["ICAO"], form["IATA"], form["Name"], Convert.ToInt32(form["Region"]));
                sqlcontroller.AddAirline(airline);
            }
            catch (Exception e)
            {

            }
        }

        private async void Post_Manufactorer(HttpRequest req)
        {
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Manufactorer manufactorer = new Manufactorer(form["Name"], Convert.ToInt32(form["Region"]));
                sqlcontroller.AddManufactorer(manufactorer);
            }
            catch (Exception e)
            {

            }
        }

        private async void Post_AircraftType(HttpRequest req)
        {
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                AircraftType aircraftType = new AircraftType(form["ICAO"], form["FullName"], form["NickName"], Convert.ToInt32(form["ManufactorerID"]));
                sqlcontroller.AddAircraftType(aircraftType);
            }
            catch (Exception e)
            {

            }
        }

        private async void Post_Aircraft(HttpRequest req)
        {
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Aircraft aircraft = new Aircraft(form["Registration"], form["Description"], Convert.ToInt32(form["TypeID"]), Convert.ToInt32(form["CountryID"]), Convert.ToInt32(form["AirlineID"]));
                sqlcontroller.AddAircraft(aircraft);
            }
            catch (Exception e)
            {

            }
        }

        private async void Post_SpottingTrip(HttpRequest req)
        {

            /// ToDo: Linking Table
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                SpottingTrip spottingTrip = new SpottingTrip(Convert.ToDateTime(form["Start"]), Convert.ToDateTime(form["End"]), form["Name"], form["Description"]);
                sqlcontroller.AddSpottingTrip(spottingTrip);
            }
            catch (Exception e)
            {

            }
        }

        private async void Post_SpottingPicture(HttpRequest req)
        {
            string BasePath = Program.Domain + "/Pic";

            try
            {
                IFormFile file = req.Form.Files[0];

                IFormCollection form = await req.ReadFormAsync();
                string FolderPath = Path.GetFullPath(Environment.CurrentDirectory) + "/Images";
                string OldFileName = file.FileName;
                string FileExtension = Path.GetExtension(file.FileName);
                string FileName = Guid.NewGuid().ToString() + FileExtension;

                FileStream fs = File.Create(FolderPath + "/" + FileName);
                file.CopyTo(fs);
                fs.Close();
                fs.Close();

                string URL = BasePath + "/" + FileName;
                SpottingPicture spottingPicture = new SpottingPicture(form["Name"], form["Description"], URL, OldFileName, Convert.ToInt32(form["SpottingTripID"]), Convert.ToInt32(form["AircraftID"]), Convert.ToInt32(form["AirportID"]));
                sqlcontroller.AddSpottingPicture(spottingPicture);
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }
        }
    }
}
