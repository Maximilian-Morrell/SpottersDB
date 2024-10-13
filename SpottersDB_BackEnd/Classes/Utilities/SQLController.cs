using Microsoft.Data.SqlClient;
using SpottersDB_BackEnd.Classes.Structure;

namespace SpottersDB_BackEnd.Classes.Utilities
{
    public class SQLController
    {
        // Instances for reuse
        private SqlConnection con = new SqlConnection("server = (localdb)\\MSSQLLocalDB; integrated security = false;");
        private SqlCommand cmd = null;
        private WebApplication app;
        // JUST FOR DEBUGGING
        private bool isDebugMode = true;

        // Checks if DB Exists
        public void ConnectToDB(string DatabaseName, WebApplication app)
        {
            cmd = new SqlCommand("", con);
            // JUST FOR DEBUGGING
            if (isDebugMode)
            {
                con.Open();
                cmd.CommandText = "DROP DATABASE " + DatabaseName;
                cmd.ExecuteNonQuery();
                isDebugMode = false;
            }
            try
            {
                // Access the WebApp for logger & stuff
                this.app = app;
                // Trys to connect to DB
                con.Open();
                con.ChangeDatabase(DatabaseName);
                con.Close();
                con.ConnectionString = "server = (localdb)\\MSSQLLocalDB; integrated security = false; database = " + DatabaseName;
            }
            catch (Exception e)
            {
                // Fails
                // DB does not Exists
                CreateDatabase(DatabaseName);
            }
            con.Close();
        }

        // Creates the DB
        private void CreateDatabase(string DatabaseName)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                con.ConnectionString = "server = (localdb)\\MSSQLLocalDB; integrated security = false;";
                con.Open();
                // Creates the DB
                cmd.CommandText = "CREATE DATABASE " + DatabaseName + ";";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Database: " + DatabaseName);
                con.ChangeDatabase(DatabaseName);
                // Creates the Countries Table
                cmd.CommandText = "CREATE TABLE Countries (CountryID int NOT NULL PRIMARY KEY IDENTITY, CountryICAOCode char(10), CountryName text);";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table Countries");
                // Creates the Airports Table
                cmd.CommandText = "CREATE TABLE Airports (AirportID int NOT NULL PRIMARY KEY IDENTITY, AirportICAOCode char(4), AirportIATACode char(3), AirportName text, AirportDescription text, AirportCity text, CountryID int, CONSTRAINT [FK_Airport_Country] FOREIGN KEY ([CountryID]) REFERENCES [Countries](CountryID));";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table Airports");
                // Creates the Airline Table
                cmd.CommandText = "CREATE TABLE Airlines (AirlineID int NOT NULL PRIMARY KEY IDENTITY, AirlineICAOCode char(10), AirlineIATACode char(10), AirlineName text, AirlineRegion int, CONSTRAINT [FK_Airline_Country] FOREIGN KEY ([AirlineRegion]) REFERENCES [Countries](CountryID));";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table Airlines");
                // Create the Manufactorer Table
                cmd.CommandText = "CREATE TABLE Manufactorers (ManufactorerID int NOT NULL PRIMARY KEY IDENTITY, ManufactorerName text, ManufactorerRegion int, CONSTRAINT [FK_Manufactorer_Country] FOREIGN KEY ([ManufactorerRegion]) REFERENCES [Countries](CountryID));";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table Manufactorers");
                // Create the AircraftType Table
                cmd.CommandText = "CREATE TABLE AircraftTypes (AircraftTypeID int NOT NULL PRIMARY KEY IDENTITY, AircraftTypeICAO char(10), AircraftTypeName text, AircraftTypeNickName text, AircraftTypeManufactorerID int, CONSTRAINT [FK_AircraftType_Manufactorer] FOREIGN KEY ([AircraftTypeManufactorerID]) REFERENCES [Manufactorers](ManufactorerID));";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table AircraftTypes");
                // Create the Aircraft Table
                cmd.CommandText = "CREATE TABLE Aircrafts (AircraftID int NOT NULL PRIMARY KEY IDENTITY, AircraftRegistration char(6), AircraftDescription text, AircraftTypeID int, AircraftCountryID int, AircraftAirlineID int, CONSTRAINT [FK_Aircraft_AircraftType] FOREIGN KEY ([AircraftTypeID]) REFERENCES [AircraftTypes](AircraftTypeID), CONSTRAINT [FK_Aircraft_Country] FOREIGN KEY ([AircraftCountryID]) REFERENCES [Countries](CountryID), CONSTRAINT [FK_Aircraft_Airline] FOREIGN KEY ([AircraftAirlineID]) REFERENCES [Airlines](AirlineID));";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table Aircrafts");
                // Create Spotting Trip Table
                cmd.CommandText = "CREATE TABLE SpottingTrips (SpottingTripID int NOT NULL PRIMARY KEY IDENTITY, SpottingTripStart datetime2(0), SpottingTripEnd datetime2(0), SpottingTripName text, SpottingTripDescription text);";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table SpottingTrips");
                // Create SpottingPicture Table
                cmd.CommandText = "CREATE TABLE SpottingPictures (SpottingPictureID int NOT NULL PRIMARY KEY IDENTITY, SpottingPictureName text, SpottingPictureDescription text, SpottingPictureURL text, SpottingPictureOriginalFileName text, SpottingPictureSpottingTripID int, SpottingPictureAircraftID int, SpottingPictureAirportID int, CONSTRAINT [FK_SpottingPicture_SpottingTrip] FOREIGN KEY ([SpottingPictureSpottingTripID]) REFERENCES [SpottingTrips](SpottingTripID), CONSTRAINT [FK_SpottingPicture_Aircraft] FOREIGN KEY ([SpottingPictureAircraftID]) REFERENCES [Aircrafts](AircraftID), CONSTRAINT [FK_SpottingPicture_Airport] FOREIGN KEY ([SpottingPictureAirportID]) REFERENCES [Airports](AirportID));";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table SpottingPictures");
                con.Close();
                ConnectToDB(DatabaseName, app);
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }
        }

        public void AddCountry(Country country)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"INSERT INTO Countries (CountryICAOCode, CountryName) VALUES ('{country.ICAO_Code}', '{country.Name}')";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Saved a Country Object", country);
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            con.Close();
        }

        public void AddAirport(Airport airport)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"INSERT INTO Airports (AirportICAOCode, AirportIATACode, AirportName, AirportDescription, AirportCity, CountryID) VALUES ('{airport.ICAO_Code}', '{airport.IATA_Code}', '{airport.Name}', '{airport.Description}', '{airport.City}', '{airport.CountryID}')";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Saved an Airport Object", airport);
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }
            con.Close();
        }

        public void AddAirline(Airline airline)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"INSERT INTO Airlines (AirlineICAOCode, AirlineIATACode, AirlineName, AirlineRegion) VALUES ('{airline.ICAO}', '{airline.IATA}', '{airline.Name}', '{airline.Region}');";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Saved an Airline Object", airline);
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }
            con.Close();
        }

        public void AddManufactorer(Manufactorer manufactorer)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"INSERT INTO Manufactorers (ManufactorerName, ManufactorerRegion) VALUES ('{manufactorer.Name}','{manufactorer.Region}');";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Saved a Manufactorer Object", manufactorer);
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }
            con.Close();
        }

        public void AddAircraftType(AircraftType aircraftType)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"INSERT INTO AircraftTypes (AircraftTypeICAO, AircraftTypeName, AircraftTypeNickName, AircraftTypeManufactorerID) VALUES ('{aircraftType.ICAOCode}', '{aircraftType.FullName}', '{aircraftType.NickName}', '{aircraftType.ManufactorerID}');";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Saved an AircraftType Object", aircraftType);
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }
        }

        public void AddAircraft(Aircraft aircraft)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"INSERT INTO Aircrafts (AircraftRegistration, AircraftDescription, AircraftTypeID, AircraftCountryID, AircraftAirlineID) VALUES('{aircraft.Registration}', '{aircraft.Description}', '{aircraft.TypeID}', '{aircraft.CountryID}', '{aircraft.AirlineID}');";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Saved an Aircraft Object", aircraft);
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }
            con.Close();
        }

        public void AddSpottingTrip(SpottingTrip spottingTrip)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"INSERT INTO SpottingTrips (SpottingTripStart, SpottingTripEnd, SpottingTripName, SpottingTripDescription) VALUES ('{spottingTrip.Start.ToString("yyyy-MM-dd HH:mm:ss")}', '{spottingTrip.End.ToString("yyyy-MM-dd HH:mm:ss")}', '{spottingTrip.Name}', '{spottingTrip.Description}');";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Saved a SpottingTrip Object", spottingTrip);
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }
            con.Close();
        }

        public void AddSpottingPicture(SpottingPicture spottingPicture)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"INSERT INTO SpottingPictures (SpottingPictureName, SpottingPictureDescription, SpottingPictureURL, SpottingPictureOriginalFileName, SpottingPictureSpottingTripID, SpottingPictureAircraftID, SpottingPictureAirportID) VALUES ('{spottingPicture.Name}','{spottingPicture.Description}','{spottingPicture.PictureUrl}','{spottingPicture.OriginalFileName}','{spottingPicture.SpottingTripID}','{spottingPicture.AircraftID}','{spottingPicture.AirportID}');";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Saved a SpottingPicture Object", spottingPicture);
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }
            con.Close();
        }

        public void UpdateCountry(Country country)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"UPDATE Countries SET CountryICAOCode = '{country.ICAO_Code}', CountryName = '{country.Name}' WHERE CountryID = {country.ID}";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Updated Countries Object");
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }
            con.Close();
        }

        public void UpdateAirport(Airport airport)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"UPDATE Airports SET AirportICAOCode = '{airport.ICAO_Code}', AirportIATACode = '{airport.IATA_Code}', AirportName = '{airport.Name}', AirportDescription = '{airport.Description}', AirportCity = '{airport.City}', CountryID = {airport.CountryID} WHERE AirportID = {airport.ID}";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Updated Airport Object");
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }
            con.Close();
        }

        public void UpdateAirline(Airline airline)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"UPDATE Airlines SET AirlineICAOCode = '{airline.ICAO}', AirlineIATACode = '{airline.IATA}', AirlineName = '{airline.Name}', AirlineRegion = {airline.Region} WHERE AirlineID = {airline.ID}";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Updated Airline Object");
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }
            con.Close();
        }

        public void UpdateAircraftType(AircraftType aircraftType)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"UPDATE AircraftTypes SET AircraftTypeICAO = '{aircraftType.ICAOCode}', AircraftTypeName = '{aircraftType.NickName}', AircraftTypeNickName = '{aircraftType.NickName}', AircraftTypeManufactorerID = {aircraftType.ManufactorerID} WHERE AircraftTypeID = {aircraftType.ID}";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Updated AircraftType Object");
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }
            con.Close();
        }
    }
}


