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

            // Post Delete Country Route
            app.MapPost("/Delete/Country", (HttpRequest req) => Delete_Country(req));
        }

        private async Task<bool> Post_Country(HttpRequest req)
        {
            bool IsSuccessfull = false;
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Country country = new Country(form["ICAO"], form["Name"]);
                IsSuccessfull = sqlcontroller.AddCountry(country);
            }
            catch (Exception)
            {

            }
            return IsSuccessfull;
        }

        private async Task<bool> Post_Airport(HttpRequest req)
        {
            bool IsSuccessfull = false;
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Airport airport = new Airport(form["ICAO"], form["IATA"], form["Name"], form["Description"], form["City"], Convert.ToInt32(form["Country"]));
                IsSuccessfull = sqlcontroller.AddAirport(airport);
            }
            catch (Exception)
            {

            }
            return IsSuccessfull;
        }

        private async Task<bool> Post_Airline(HttpRequest req)
        {
            bool IsSuccessfull = false;
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Airline airline = new Airline(form["ICAO"], form["IATA"], form["Name"], Convert.ToInt32(form["Region"]));
                IsSuccessfull = sqlcontroller.AddAirline(airline);
            }
            catch (Exception)
            {

            }
            return IsSuccessfull;
        }

        private async Task<bool> Post_Manufactorer(HttpRequest req)
        {
            bool IsSuccessfull = false;
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Manufactorer manufactorer = new Manufactorer(form["Name"], Convert.ToInt32(form["Region"]));
                IsSuccessfull = sqlcontroller.AddManufactorer(manufactorer);
            }
            catch (Exception)
            {

            }
            return IsSuccessfull;
        }

        private async Task<bool> Post_AircraftType(HttpRequest req)
        {
            bool IsSuccessfull = false;
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                AircraftType aircraftType = new AircraftType(form["ICAO"], form["FullName"], form["NickName"], Convert.ToInt32(form["ManufactorerID"]));
                IsSuccessfull = sqlcontroller.AddAircraftType(aircraftType);
            }
            catch (Exception)
            {

            }
            return IsSuccessfull;
        }

        private async Task<bool> Post_Aircraft(HttpRequest req)
        {
            bool IsSuccessfull = false;
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Aircraft aircraft = new Aircraft(form["Registration"], form["Description"], Convert.ToInt32(form["TypeID"]), Convert.ToInt32(form["CountryID"]), Convert.ToInt32(form["AirlineID"]));
                IsSuccessfull = sqlcontroller.AddAircraft(aircraft);
            }
            catch (Exception)
            {

            }
            return IsSuccessfull;
        }

        private async Task<bool> Post_SpottingTrip(HttpRequest req)
        {
            bool IsSuccessfull = false;
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                SpottingTrip spottingTrip = new SpottingTrip(Convert.ToDateTime(form["Start"]), Convert.ToDateTime(form["End"]), form["Name"], form["Description"]);
                List<int> AirportIDs = new List<int>(); if (form["AirportID"] != "")
                {
                    foreach (string AirportID in Convert.ToString(form["AirportIDs"]).Split(','))
                    {
                        AirportIDs.Add(Convert.ToInt32(AirportID));
                    }
                }
                IsSuccessfull = sqlcontroller.AddSpottingTrip(spottingTrip, AirportIDs);
            }
            catch (Exception)
            {

            }
            return IsSuccessfull;
        }

        private async Task<bool> Post_SpottingPicture(HttpRequest req)
        {
            string BasePath = app.Urls.ToList()[0] + "/Pic";
            bool IsSuccessfull = false;
            try
            {
                string URL = "";
                string OldFileName = "";
                if (req.Form.Files.Count > 0)
                {
                    IFormFile file = req.Form.Files[0];

                    string FolderPath = Path.GetFullPath(Environment.CurrentDirectory) + "/Images";
                    OldFileName = file.FileName;
                    string FileExtension = Path.GetExtension(file.FileName);
                    string FileName = Guid.NewGuid().ToString() + FileExtension;

                    FileStream fs = File.Create(FolderPath + "/" + FileName);
                    file.CopyTo(fs);
                    fs.Close();
                    fs.Close();

                    URL = BasePath + "/" + FileName;
                }
                IFormCollection form = await req.ReadFormAsync();
                SpottingPicture spottingPicture = new SpottingPicture(form["Name"], form["Description"], URL, OldFileName, Convert.ToInt32(form["SpottingTripAirport"]), Convert.ToInt32(form["AircraftID"]));
                IsSuccessfull = sqlcontroller.AddSpottingPicture(spottingPicture);
            }
            catch (Exception)
            {

            }
            return IsSuccessfull;
        }

        private async Task<bool> Delete_Country(HttpRequest req)
        {
            bool Success = false;
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Success = sqlcontroller.DeleteCountryByID(Convert.ToInt32(form["ID"]));
            }
            catch (Exception)
            {

            }

            return Success;
        }

        private async Task<bool> Delete_Airport(HttpRequest req)
        {
            bool Success = false;
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Success = sqlcontroller.DeleteAirportByID(Convert.ToInt32(form["ID"]));
            }
            catch (Exception)
            {

            }

            return Success;
        }

        private async Task<bool> Delete_Airline(HttpRequest req)
        {
            bool Success = false;
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Success = sqlcontroller.DeleteAirlineByID(Convert.ToInt32(form["ID"]));
            }
            catch (Exception)
            {

            }

            return Success;
        }

        private async Task<bool> Delete_AircraftType(HttpRequest req)
        {
            bool Success = false;
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Success = sqlcontroller.DeleteAircraftTypeByID(Convert.ToInt32(form["ID"]));
            }
            catch (Exception)
            {

            }

            return Success;
        }

        private async Task<bool> Delete_Manufactorer(HttpRequest req)
        {
            bool Success = false;
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Success = sqlcontroller.DeleteManufactorerByID(Convert.ToInt32(form["ID"]));
            }
            catch (Exception)
            {

            }

            return Success;
        }

        private async Task<bool> Delete_Aircraft(HttpRequest req)
        {
            bool Success = false;
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Success = sqlcontroller.DeleteAircraftByID(Convert.ToInt32(form["ID"]));
            }
            catch (Exception)
            {

            }

            return Success;
        }

        private async Task<bool> Delete_SpottingTrip(HttpRequest req)
        {
            bool Success = false;
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Success = sqlcontroller.DeleteSpottingTripByID(Convert.ToInt32(form["ID"]));
            }
            catch (Exception)
            {

            }

            return Success;
        }

        private async Task<bool> Delete_SpottingPicture(HttpRequest req)
        {
            bool Success = false;
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                SpottingPicture pic = sqlcontroller.GetSpottingPictureByID(Convert.ToInt32(form["ID"]));
                Success = sqlcontroller.DeleteSpottingPictureByID(Convert.ToInt32(form["ID"]));
                if(Success)
                {
                    string FolderPath = Path.GetFullPath(Environment.CurrentDirectory) + "/Images";
                    File.Delete(FolderPath + "/" + pic.PictureUrl);
                }
            }
            catch (Exception)
            {

            }

            return Success;
        }
    }
}
