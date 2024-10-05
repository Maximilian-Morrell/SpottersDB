using Microsoft.Data.SqlClient;
using SpottersDB_BackEnd.Classes.Structure;

namespace SpottersDB_BackEnd.Classes.Utilities
{
    public class SQLController
    {
        // Instances for reuse
        private SqlConnection con = new SqlConnection("server = (localdb)\\MSSQLLocalDB; integrated security = false;");
        private SqlCommand cmd = null;

        // Checks if DB Exists
        public void ConnectToDB(string DatabaseName)
        {
            cmd = new SqlCommand("", con);
            try
            {
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
                con.ChangeDatabase(DatabaseName);
                // Creates the Countries Table
                cmd.CommandText = "CREATE TABLE Countries (CountryID INT NOT NULL PRIMARY KEY IDENTITY, ICAOCode char(10), CountryName char(255));";
                cmd.ExecuteNonQuery();
                // Creates the Airports Table
                cmd.CommandText = "Create TABLE Airports (AirpotID INT NOT NULL PRIMARY KEY IDENTITY, ICAOCode char(4), IATACode char(3), AirportName char(255), AirportDescription text, AirportCity char(255), CountryID int, CONSTRAINT [FK_Airport_Country] FOREIGN KEY ([CountryID]) REFERENCES [Countries](CountryID));";
                cmd.ExecuteNonQuery();
                // Creates the Airline Object

                con.Close();
                ConnectToDB(DatabaseName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void AddCountry(Country country)
        {
            try
            {
                con.Open();
                cmd.CommandText = $@"INSERT INTO Countries (ICAOCode, CountryName) VALUES ('{country.ICAO_Code}', '{country.Name}')";
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            con.Close();
        }
    }
}


