﻿using Microsoft.AspNetCore.Server.HttpSys;
using SpottersDB_BackEnd.Classes.Structure;
using SpottersDB_BackEnd.Classes.Utilities;

namespace SpottersDB_BackEnd.Classes.API
{
    public class API_PATCH : API_Base
    {
        public API_PATCH(WebApplication app, SQLController sQLController) : base(app, sQLController)
        {

        }

        protected override void MainAPI()
        {
            // Patch Country Route
            app.MapPost("/Patch/Country", (HttpRequest req) => PATCH_Country(req));

            // Patch Airport Route
            app.MapPost("/Patch/Airport", (HttpRequest req) => PATCH_Airport(req));

            // Patch Airline Route
            app.MapPost("/Patch/Airline", (HttpRequest req) => PATCH_Airline(req));

            // Patch AircraftType Route
            app.MapPost("/Patch/AircraftType", (HttpRequest req) => PATCH_AircraftType(req));

            // Patch Manufactorer Route
            app.MapPost("/Patch/Manufactorer", (HttpRequest req) => PATCH_Manufactorer(req));

            // Patch Aircraft Route
            app.MapPost("/Patch/Aircraft", (HttpRequest req) => PATCH_Aircraft(req));

            // Patch SpottingTrip Route
            app.MapPost("/Patch/SpottingTrip", (HttpRequest req) => PATCH_SpottingTrip(req));

            // Patch SpottingPicture Route
            app.MapPost("/Patch/SpottingPicture", (HttpRequest req) => PATCH_SpottingPicture(req));
        }

        private async void PATCH_Country(HttpRequest req)
        {
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Country country = new Country(Convert.ToInt32(form["ID"]), form["ICAO"], form["Name"]);
                sqlcontroller.UpdateCountry(country);
            }
            catch (Exception e)
            {

            }
        }

        private async void PATCH_Airport(HttpRequest req)
        {
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Airport airport = new Airport(Convert.ToInt32(form["ID"]), form["ICAO"], form["IATA"], form["Name"], form["Description"], form["City"], Convert.ToInt32(form["Country"]));
                sqlcontroller.UpdateAirport(airport);
            }
            catch (Exception e)
            {

            }
        }

        private async void PATCH_Airline(HttpRequest req)
        {
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Airline airline = new Airline(Convert.ToInt32(form["ID"]), form["ICAO"], form["IATA"], form["Name"], Convert.ToInt32(form["Region"]));
                sqlcontroller.UpdateAirline(airline);
            }
            catch (Exception e)
            {
                
            }
        }

        private async void PATCH_AircraftType(HttpRequest req)
        {
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                AircraftType aircraftType = new AircraftType(Convert.ToInt32(form["ID"]), form["ICAO"], form["FullName"], form["NickName"], Convert.ToInt32(form["ManufactorerID"]));
                sqlcontroller.UpdateAircraftType(aircraftType);
            }
            catch (Exception e)
            {

            }
        }

        private async void PATCH_Manufactorer(HttpRequest req)
        {
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Manufactorer manufactorer = new Manufactorer(Convert.ToInt32(form["ID"]), form["Name"], Convert.ToInt32(form["Region"]));
                sqlcontroller.UpdateManufactorer(manufactorer);
            }
            catch (Exception e)
            {

            }
        }

        private async void PATCH_Aircraft(HttpRequest req)
        {
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Aircraft aircraft = new Aircraft(Convert.ToInt32(form["ID"]), form["Registration"], form["Description"], Convert.ToInt32(form["TypeID"]), Convert.ToInt32(form["CountryID"]), Convert.ToInt32(form["AirlineID"]));
                sqlcontroller .UpdateAircraft(aircraft);
            }
            catch (Exception e)
            {

            }
        }

        private async void PATCH_SpottingTrip(HttpRequest req)
        {
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                SpottingTrip spottingTrip = new SpottingTrip(Convert.ToInt32(form["ID"]), Convert.ToDateTime(form["Start"]), Convert.ToDateTime(form["End"]), form["Name"], form["Description"]);
                List<int> AirportIDs = new List<int>();
                if (form["AirportID"] != "")
                {
                    foreach (string AirportID in Convert.ToString(form["AirportID"]).Split(','))
                    {
                        AirportIDs.Add(Convert.ToInt32(AirportID));
                    }
                }
                sqlcontroller.UpdateSpottingTrip(spottingTrip, AirportIDs);
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }
        }

        private async void PATCH_SpottingPicture(HttpRequest req)
        {
            try
            {
                if(req.Form.Files.Count > 0)
                {
                    string BasePath = app.Urls.ToList()[0] + "/Pic";
                    string URL = "";
                    string OldFileName = "";
                    
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

                    IFormCollection form = await req.ReadFormAsync();
                    
                    string oldFileName = form["PictureURL"];
                    File.Delete(FolderPath + "/" + oldFileName);

                    SpottingPicture spottingPicture = new SpottingPicture(Convert.ToInt32(form["ID"]), form["Name"], form["Description"], URL, OldFileName, Convert.ToInt32(form["SpottingTripAirport"]), Convert.ToInt32(form["AircraftID"]));
                    sqlcontroller.UpdateSpottingPicture(spottingPicture);
                }
                else
                {
                    IFormCollection form = await req.ReadFormAsync();
                    SpottingPicture spottingPicture = new SpottingPicture(Convert.ToInt32(form["ID"]), form["Name"], form["Description"], form["PictureURL"], form["OriginalFileName"], Convert.ToInt32(form["SpottingTripAirport"]), Convert.ToInt32(form["AircraftID"]));
                    sqlcontroller.UpdateSpottingPicture(spottingPicture);
                }
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }
        }
    }
}
