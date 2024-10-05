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
                cmd.CommandText = "CREATE TABLE Countries (CountryID INT NOT NULL PRIMARY KEY IDENTITY, ICAOCode char(10), CountryName text);";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table Countries");
                // Creates the Airports Table
                cmd.CommandText = "Create TABLE Airports (AirpotID INT NOT NULL PRIMARY KEY IDENTITY, ICAOCode char(4), IATACode char(3), AirportName text, AirportDescription text, AirportCity text, CountryID int, CONSTRAINT [FK_Airport_Country] FOREIGN KEY ([CountryID]) REFERENCES [Countries](CountryID));";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table Airports");
                // Creates the Airline Table
                cmd.CommandText = "Create TABLE Airlines (AirlineID INT NOT NULL PRIMARY KEY IDENTITY, ICAOCode char(10), IATACode char(10), AirlineName text, AirlineLocation text);";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table Airlines");
                // Create the AircraftType Table
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
                cmd.CommandText = $"INSERT INTO Countries (ICAOCode, CountryName) VALUES ('{country.ICAO_Code}', '{country.Name}')";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Saved a Country Object");
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
                cmd.CommandText = $"INSERT INTO Airports (ICAOCode, IATACode, AirportName, AirportDescription, AirportCity, CountryID) VALUES ('{airport.ICAO_Code}', '{airport.IATA_Code}', '{airport.Name}', '{airport.Description}', '{airport.City}', '{airport.CountryID}')";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Saved an Airport Object");
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
                cmd.CommandText = $"INSERT INTO Airlines (ICAOCode, IATACode, AirlineName, AirlineLocation) VALUES ('{airline.ICAO}', '{airline.IATA}', '{airline.Name}', '{airline.Location}');";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Saved an Airline Object");
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


