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

        // Checks if DB Exists
        public void ConnectToDB(string DatabaseName, WebApplication app)
        {
            cmd = new SqlCommand("", con);
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
                cmd.CommandText = "CREATE TABLE Airports (AirpotID int NOT NULL PRIMARY KEY IDENTITY, AirportICAOCode char(4), AirportIATACode char(3), AirportName text, AirportDescription text, AirportCity text, CountryID int, CONSTRAINT [FK_Airport_Country] FOREIGN KEY ([CountryID]) REFERENCES [Countries](CountryID));";
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
    }
}


