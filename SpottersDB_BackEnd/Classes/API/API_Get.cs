﻿using Microsoft.AspNetCore.Mvc;
using SpottersDB_BackEnd.Classes.Structure;
using SpottersDB_BackEnd.Classes.Utilities;
using System.Reflection;
using System.Text.Json;

namespace SpottersDB_BackEnd.Classes.API
{
    public class API_GET : API_Base
    {
        public API_GET(WebApplication app, SQLController sqlcontroller) : base(app, sqlcontroller)
        {

        }

        protected override void MainAPI()
        {
            // Get Countries Route
            app.MapGet("/Get/Countries", GET_Countries);
            // Get Only Countries Route
            app.MapGet("/Get/OnlyCountries", GET_OnlyCountries);
            // Get Country by ID Route
            app.MapGet("/Get/Country", (int ID) => GET_Country(ID));
            // Get Regions Route
            app.MapGet("/Get/Regions", GET_Regions);

            // Get Airports Route
            app.MapGet("/Get/Airports", GET_Airports);
            // Get Airport by ID Route
            app.MapGet("/Get/Airport", (int ID) => GET_Airport(ID));

            // Get Airlines Route
            app.MapGet("/Get/Airlines", GET_Airlines);
            // Get Airline By ID Route
            app.MapGet("/Get/Airline", (int ID) => GET_Airline(ID));

            // Get AircraftTypes Route
            app.MapGet("/Get/AircraftTypes", GET_AircraftTypes);
            // Get AircraftType By ID Route
            app.MapGet("/Get/AircraftType", (int ID) => GET_AircraftType(ID));

            // Get Manufactorers Route
            app.MapGet("/Get/Manufactorers", GET_Manufactorers);
            // Get Manufactorer By ID Route
            app.MapGet("/Get/Manufactorer", (int ID) => GET_Manufactorer(ID));

            // Get Aircrafts Route
            app.MapGet("/Get/Aircrafts", GET_Aircrafts);
            // Get Aircraft By Type Route
            app.MapGet("/Get/Aircrafts/Type", (int TypeID) => GET_AircraftsByTypeID(TypeID));
            // Get Aircraft By ID Route
            app.MapGet("/Get/Aircraft", (int ID) => GET_Aircraft(ID));

            // Get SpottingTrips Route
            app.MapGet("/Get/SpottingTrips", GET_SpottingTrips);
            // Get SpottingTrip By ID Route
            app.MapGet("/Get/SpottingTrip", (int ID) => GET_SpottingTrip(ID));

            // Get SpottingPictures Route
            app.MapGet("/Get/SpottingPictures", GET_SpottingPictures);
            // Get SpottingPicture By ID Route
            app.MapGet("/Get/SpottingPicture", (int ID) => GET_SpottingPicture(ID));
            // Get SpottingPicture By Aircraft Route
            app.MapGet("/Get/SpottingPictures/Aircraft", (int AircraftID) => GET_SpottingPicturesByAircraftID(AircraftID));

            // Get NewestImageFromCountry Route
            app.MapGet("/Get/Newest/Country", (int ID) => GET_NewestImageFromCountry(ID));

            //Get Airports from SpottingTrip Route
            app.MapGet("/Get/Airports/SpottingTrip", (int ID) => GET_AirportsFromSpottingTrip(ID));

            //Get SpottingTripLinkTable Route
            app.MapGet("/Get/SpottingTripAirport/LinkID", (int SpottingTripID, int AirportID) => GET_LinkID(SpottingTripID, AirportID));
            app.MapGet("/Get/SpottingTripAirport/SpottingTripAirport", (int LinkID) => GET_SpottingTripAirport(LinkID));
        }

        private List<Country> GET_Countries()
        {
            return sqlcontroller.GetCountries();
        }

        private List<Country> GET_OnlyCountries()
        {
            return sqlcontroller.GetCountries(true);
        }

        private Country GET_Country(int ID)
        {
            return sqlcontroller.GetCountryByID(ID);
        }

        private List<Airport> GET_Airports()
        {
            return sqlcontroller.GetAirports();
        }

        private Airport GET_Airport(int ID)
        {
            return sqlcontroller.GetAirportByID(ID);
        }

        private List<Airline> GET_Airlines()
        {
            return sqlcontroller.GetAirlines();
        }

        private Airline GET_Airline(int ID)
        {
            return sqlcontroller.GetAirlineByID(ID);
        }

        private List<AircraftType> GET_AircraftTypes()
        {
            return sqlcontroller.GetAircraftTypes();
        }

        private AircraftType GET_AircraftType(int ID)
        {
            return sqlcontroller.GetAircraftTypeByID(ID);
        }

        private List<Manufactorer> GET_Manufactorers()
        {
            return sqlcontroller.GetManufactorers();
        }

        private Manufactorer GET_Manufactorer(int ID)
        {
            return sqlcontroller.GetManufactorerByID(ID);
        }

        private List<Aircraft> GET_Aircrafts()
        {
            return sqlcontroller.GetAircrafts();
        }

        private List<Aircraft> GET_AircraftsByTypeID(int TypeID)
        {
            return sqlcontroller.GetAircraftsByTypeID(TypeID);
        }

        private Aircraft GET_Aircraft(int ID)
        {
            return sqlcontroller.GetAircraftByID(ID);
        }

        private List<SpottingTrip> GET_SpottingTrips()
        {
            return sqlcontroller.GetSpottingTrips();
        }

        private SpottingTrip GET_SpottingTrip(int ID)
        {
            return sqlcontroller.GetSpottingTripByID(ID);
        }

        private List<SpottingPicture> GET_SpottingPictures()
        {
            return sqlcontroller.GetSpottingPictures();
        }

        private SpottingPicture GET_SpottingPicture(int ID)
        {
            return sqlcontroller.GetSpottingPictureByID(ID);
        }

        private List<SpottingPicture> GET_SpottingPicturesByAircraftID(int ID)
        {
            return sqlcontroller.GetSpottingPictureByAircraftID(ID);
        }

        private List<Country> GET_Regions()
        {
            return sqlcontroller.GetRegions();
        } 

        private string GET_NewestImageFromCountry(int ID)
        {
            return sqlcontroller.GetNewestImageFromCountry(ID);
        }

        private List<Airport> GET_AirportsFromSpottingTrip(int ID)
        {
            return sqlcontroller.GetAirportsFromSpottingTrip(ID);
        }

        private int GET_LinkID(int SpottingTripID, int AirportID)
        {
            return sqlcontroller.GetLinkID(SpottingTripID, AirportID);
        }

        private Dictionary<string, int> GET_SpottingTripAirport(int LinkID)
        {
            return sqlcontroller.GetSpottingTripAirportFromLinkID(LinkID);
        }
    }
}
