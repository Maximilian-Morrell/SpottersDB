using Microsoft.Data.SqlClient;
using SpottersDB_BackEnd.Classes.Structure;
using System.Data;
using System.Data.Common;

namespace SpottersDB_BackEnd.Classes.Utilities
{
    public class SQLController
    {
        // Instances for reuse
        private SqlConnection con = new SqlConnection("server = (localdb)\\MSSQLLocalDB; integrated security = false;");
        private SqlCommand cmd = null;
        private SqlDataReader reader = null;
        private WebApplication app;
        // JUST FOR DEBUGGING
        private bool isDebugMode = false;

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
                con.Close();
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
                cmd.CommandText = "CREATE TABLE Countries (CountryID int NOT NULL PRIMARY KEY IDENTITY, CountryICAOCode varchar(10), CountryName varchar(255));";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table Countries");
                // Creates the Airports Table
                cmd.CommandText = "CREATE TABLE Airports (AirportID int NOT NULL PRIMARY KEY IDENTITY, AirportICAOCode char(4), AirportIATACode char(3), AirportName text, AirportDescription text, AirportCity varchar(255), CountryID int, CONSTRAINT [FK_Airport_Country] FOREIGN KEY ([CountryID]) REFERENCES [Countries](CountryID));";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table Airports");
                // Creates the Airline Table
                cmd.CommandText = "CREATE TABLE Airlines (AirlineID int NOT NULL PRIMARY KEY IDENTITY, AirlineICAOCode char(3), AirlineIATACode char(2), AirlineName text, AirlineRegion int, CONSTRAINT [FK_Airline_Country] FOREIGN KEY ([AirlineRegion]) REFERENCES [Countries](CountryID));";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table Airlines");
                // Create the Manufactorer Table
                cmd.CommandText = "CREATE TABLE Manufactorers (ManufactorerID int NOT NULL PRIMARY KEY IDENTITY, ManufactorerName text, ManufactorerRegion int, CONSTRAINT [FK_Manufactorer_Country] FOREIGN KEY ([ManufactorerRegion]) REFERENCES [Countries](CountryID));";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table Manufactorers");
                // Create the AircraftType Table
                cmd.CommandText = "CREATE TABLE AircraftTypes (AircraftTypeID int NOT NULL PRIMARY KEY IDENTITY, AircraftTypeICAO varchar(10), AircraftTypeName text, AircraftTypeNickName text, AircraftTypeManufactorerID int, CONSTRAINT [FK_AircraftType_Manufactorer] FOREIGN KEY ([AircraftTypeManufactorerID]) REFERENCES [Manufactorers](ManufactorerID));";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table AircraftTypes");
                // Create the Aircraft Table
                cmd.CommandText = "CREATE TABLE Aircrafts (AircraftID int NOT NULL PRIMARY KEY IDENTITY, AircraftRegistration varchar(6), AircraftDescription text, AircraftTypeID int, AircraftCountryID int, AircraftAirlineID int, CONSTRAINT [FK_Aircraft_AircraftType] FOREIGN KEY ([AircraftTypeID]) REFERENCES [AircraftTypes](AircraftTypeID), CONSTRAINT [FK_Aircraft_Country] FOREIGN KEY ([AircraftCountryID]) REFERENCES [Countries](CountryID), CONSTRAINT [FK_Aircraft_Airline] FOREIGN KEY ([AircraftAirlineID]) REFERENCES [Airlines](AirlineID));";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table Aircrafts");
                // Create Spotting Trip Table
                cmd.CommandText = "CREATE TABLE SpottingTrips (SpottingTripID int NOT NULL PRIMARY KEY IDENTITY, SpottingTripStart datetime2(0), SpottingTripEnd datetime2(0), SpottingTripName text, SpottingTripDescription text);";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table SpottingTrips");
                // Create SpottingPicture Table
                cmd.CommandText = "CREATE TABLE SpottingPictures (SpottingPictureID int NOT NULL PRIMARY KEY IDENTITY, SpottingPictureName text, SpottingPictureDescription text, SpottingPictureURL text, SpottingPictureOriginalFileName text, SpottingPictureSpottingTripID int, SpottingPictureAircraftID int, CONSTRAINT [FK_SpottingPicture_SpottingTrip] FOREIGN KEY ([SpottingPictureSpottingTripID]) REFERENCES [SpottingTrips](SpottingTripID), CONSTRAINT [FK_SpottingPicture_Aircraft] FOREIGN KEY ([SpottingPictureAircraftID]) REFERENCES [Aircrafts](AircraftID));";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Table SpottingPictures");
                // Create SpottingTrip & Airport Link Table
                cmd.CommandText = "CREATE TABLE SpottingTripAirports (LinkID int NOT NULL PRIMARY KEY IDENTITY, SpottingTripID int, AirportID int, CONSTRAINT [FK_SpottingTripAirport_SpottingTrip] FOREIGN KEY ([SpottingTripID]) REFERENCES [SpottingTrips](SpottingTripID), CONSTRAINT [FK_SpottingTripAirport_Airport] FOREIGN KEY ([AirportID]) REFERENCES [Airports](AirportID));";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Created the Link Table for SpottingTrips and Airports");
                con.Close();
                ConnectToDB(DatabaseName, app);
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }

        public void AddCountry(Country country)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }

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

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
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

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
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

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
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

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
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

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
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

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }

        public void AddSpottingTrip(SpottingTrip spottingTrip, int AirportID)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"INSERT INTO SpottingTrips (SpottingTripStart, SpottingTripEnd, SpottingTripName, SpottingTripDescription) VALUES ('{spottingTrip.Start.ToString("yyyy-MM-dd HH:mm:ss")}', '{spottingTrip.End.ToString("yyyy-MM-dd HH:mm:ss")}', '{spottingTrip.Name}', '{spottingTrip.Description}'); SELECT SCOPE_IDENTITY()";
                int ID = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.CommandText = $"INSERT INTO SpottingTripAirports (SpottingTripID, AirportID) VALUES ('{ID}', '{AirportID}');";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Saved a SpottingTrip Object", spottingTrip);
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }

        public void AddSpottingPicture(SpottingPicture spottingPicture)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"INSERT INTO SpottingPictures (SpottingPictureName, SpottingPictureDescription, SpottingPictureURL, SpottingPictureOriginalFileName, SpottingPictureSpottingTripID, SpottingPictureAircraftID) VALUES ('{spottingPicture.Name}','{spottingPicture.Description}','{spottingPicture.PictureUrl}','{spottingPicture.OriginalFileName}','{spottingPicture.SpottingTripID}','{spottingPicture.AircraftID}');";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Saved a SpottingPicture Object", spottingPicture);
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
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

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
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

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
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

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }

        public void UpdateAircraftType(AircraftType aircraftType)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"UPDATE AircraftTypes SET AircraftTypeICAO = '{aircraftType.ICAOCode}', AircraftTypeName = '{aircraftType.FullName}', AircraftTypeNickName = '{aircraftType.NickName}', AircraftTypeManufactorerID = {aircraftType.ManufactorerID} WHERE AircraftTypeID = {aircraftType.ID}";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Updated AircraftType Object");
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }

        public void UpdateManufactorer(Manufactorer manufactorer)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"UPDATE Manufactorers SET ManufactorerName = '{manufactorer.Name}', ManufactorerRegion = {manufactorer.Region} WHERE ManufactorerID = {manufactorer.ID}";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Updated a Manufactorer Object");
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }

        public void UpdateAircraft(Aircraft aircraft)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"UPDATE Aircrafts SET AircraftRegistration = '{aircraft.Registration}', AircraftDescription = '{aircraft.Description}', AircraftTypeID = {aircraft.TypeID}, AircraftCountryID = {aircraft.CountryID}, AircraftAirlineID = {aircraft.AirlineID} WHERE AircraftID = {aircraft.ID}";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Updated an Aircraft Object");
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }

        public void UpdateSpottingTrip(SpottingTrip spottingTrip)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"UPDATE SpottingTrips SET SpottingTripStart = '{spottingTrip.Start.ToString("yyyy-MM-dd HH:mm:ss")}', SpottingTripEnd = '{spottingTrip.End.ToString("yyyy-MM-dd HH:mm:ss")}', SpottingTripName = '{spottingTrip.Name}', SpottingTripDescription = '{spottingTrip.Description}' WHERE SpottingTripID = {spottingTrip.ID}";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Updated a SpottingTrip Object");
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }

        public void UpdateSpottingPicture(SpottingPicture spottingPicture)
        {
            try
            {
                con.Open();
                cmd.CommandText = $"UPDATE SpottingPictures SET SpottingPictureName = '{spottingPicture.Name}', SpottingPictureDescription = '{spottingPicture.Description}', SpottingPictureURL = '{spottingPicture.PictureUrl}', SpottingPictureOriginalFileName = '{spottingPicture.OriginalFileName}', SpottingPictureSpottingTripID = {spottingPicture.SpottingTripID}, SpottingPictureAircraftID = {spottingPicture.AircraftID} WHERE SpottingPictureID = {spottingPicture.ID}";
                cmd.ExecuteNonQuery();
                app.Logger.LogInformation("Updated a SpottingPicture Object");
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }

        public List<Country> GetCountries()
        {
            List<Country> Countries = new List<Country>();
            try
            {
                con.Open();
                cmd.CommandText = $"SELECT * FROM Countries";
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Country country = new Country(Convert.ToInt32(reader["CountryID"]), Convert.ToString(reader["CountryICAOCode"]), Convert.ToString(reader["CountryName"]));
                    Countries.Add(country);
                }
                app.Logger.LogInformation("Read " + Countries.Count + " Country Objects");
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogInformation(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            return Countries;
        }

        public List<Country> GetRegions()
        {
            List<Country> Countries = new List<Country>();
            try
            {
                con.Open();
                cmd.CommandText = "SELECT * FROM Countries WHERE CountryICAOCode = ''";
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Country country = new Country(Convert.ToInt32(reader["CountryID"]), Convert.ToString(reader["CountryICAOCode"]), Convert.ToString(reader["CountryName"]));
                    Countries.Add(country);
                }
                app.Logger.LogInformation("Read " + Countries.Count + " Country Objects");
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogInformation(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            return Countries;
        }

        public Country GetCountryByID(int ID)
        {
            Country country = null;
            try
            {
                con.Open();
                cmd.CommandText = $"SELECT * FROM Countries WHERE CountryID = {ID}";
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    country = new Country(Convert.ToInt32(reader["CountryID"]), Convert.ToString(reader["CountryICAOCode"]), Convert.ToString(reader["CountryName"]));
                }
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogInformation(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            return country;
        }

        public List<Airport> GetAirports()
        {
            List<Airport> Airports = new List<Airport>();
            try
            {
                con.Open();
                cmd.CommandText = $"SELECT * FROM Airports";
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Airport airport = new Airport(Convert.ToInt32(reader["AirportID"]), Convert.ToString(reader["AirportICAOCode"]), Convert.ToString(reader["AirportIATACode"]), Convert.ToString(reader["AirportName"]), Convert.ToString(reader["AirportDescription"]), Convert.ToString(reader["AirportCity"]), Convert.ToInt32(reader["CountryID"]));
                    Airports.Add(airport);
                }
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            return Airports;
        }

        public Airport GetAirportByID(int ID)
        {
            Airport airport = null;
            try
            {
                con.Open();
                cmd.CommandText = $"SELECT * FROM Airports WHERE AirportID = {ID}";
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    airport = new Airport(Convert.ToInt32(reader["AirportID"]), Convert.ToString(reader["AirportICAOCode"]), Convert.ToString(reader["AirportIATACode"]), Convert.ToString(reader["AirportName"]), Convert.ToString(reader["AirportDescription"]), Convert.ToString(reader["AirportCity"]), Convert.ToInt32(reader["CountryID"]));
                }
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            return airport;
        }

        public List<Airline> GetAirlines()
        {
            List<Airline> Airlines = new List<Airline>();
            try
            {
                con.Open();
                cmd.CommandText = $"SELECT * FROM Airlines";
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Airline airline = new Airline(Convert.ToInt32(reader["AirlineID"]), Convert.ToString(reader["AirlineICAOCode"]), Convert.ToString(reader["AirlineIATACode"]), Convert.ToString(reader["AirlineName"]), Convert.ToInt32(reader["AirlineRegion"]));
                    Airlines.Add(airline);
                }
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            return Airlines;
        }

        public Airline GetAirlineByID(int ID)
        {
            Airline airline = null;
            try
            {
                con.Open();
                cmd.CommandText = $"SELECT * FROM Airlines WHERE AirlineID = {ID}";
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    airline = new Airline(Convert.ToInt32(reader["AirlineID"]), Convert.ToString(reader["AirlineICAOCode"]), Convert.ToString(reader["AirlineIATACode"]), Convert.ToString(reader["AirlineName"]), Convert.ToInt32(reader["AirlineRegion"]));
                }
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            return airline;
        }

        public List<AircraftType> GetAircraftTypes()
        {
            List<AircraftType> AircraftTypes = new List<AircraftType>();
            try
            {
                con.Open();
                cmd.CommandText = $"SELECT * FROM AircraftTypes";
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                   AircraftType aircraftType = new AircraftType(Convert.ToInt32(reader["AircraftTypeID"]), Convert.ToString(reader["AircraftTypeICAO"]), Convert.ToString(reader["AircraftTypeName"]), Convert.ToString(reader["AircraftTypeNickName"]), Convert.ToInt32(reader["AircraftTypeManufactorerID"]));
                   AircraftTypes.Add(aircraftType);
                }
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            return AircraftTypes;
        }

        public AircraftType GetAircraftTypeByID(int ID)
        {
            AircraftType aircraftType = null;
            try
            {
                con.Open();
                cmd.CommandText = $"SELECT * FROM AircraftTypes WHERE AircraftTypeID = {ID}";
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    aircraftType = new AircraftType(Convert.ToInt32(reader["AircraftTypeID"]), Convert.ToString(reader["AircraftTypeICAO"]), Convert.ToString(reader["AircraftTypeName"]), Convert.ToString(reader["AircraftTypeNickName"]), Convert.ToInt32(reader["AircraftTypeManufactorerID"]));
                }
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            return aircraftType;
        }

        public List<Manufactorer> GetManufactorers()
        {
            List<Manufactorer> Manufactorers = new List<Manufactorer>();
            try
            {
                con.Open();
                cmd.CommandText = $"SELECT * FROM Manufactorers";
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    Manufactorer manufactorer = new Manufactorer(Convert.ToInt32(reader["ManufactorerID"]), Convert.ToString(reader["ManufactorerName"]), Convert.ToInt32(reader["ManufactorerRegion"]));
                    Manufactorers.Add(manufactorer);
                }
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            return Manufactorers;
        }

        public Manufactorer GetManufactorerByID(int id)
        {
            Manufactorer manufactorer = null;
            try
            {
                con.Open();
                cmd.CommandText = $"SELECT * FROM Manufactorers WHERE ManufactorerID = {id}";
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    manufactorer = new Manufactorer(Convert.ToInt32(reader["ManufactorerID"]), Convert.ToString(reader["ManufactorerName"]), Convert.ToInt32(reader["ManufactorerRegion"]));
                }
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            return manufactorer;
        }

        public List<Aircraft> GetAircrafts()
        {
            List<Aircraft> aircrafts = new List<Aircraft>();
            try
            {
                con.Open();
                cmd.CommandText = "SELECT * FROM Aircrafts";
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    Aircraft aircraft = new Aircraft(Convert.ToInt32(reader["AircraftID"]), Convert.ToString(reader["AircraftRegistration"]), Convert.ToString(reader["AircraftDescription"]), Convert.ToInt32(reader["AircraftTypeID"]), Convert.ToInt32(reader["AircraftCountryID"]), Convert.ToInt32(reader["AircraftAirlineID"]));
                    aircrafts.Add(aircraft);
                }
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            return aircrafts;
        }

        public Aircraft GetAircraftByID(int id)
        {
            Aircraft aircraft = null;
            try
            {
                con.Open();
                cmd.CommandText = $"SELECT * FROM Aircrafts WHERE AircraftID = {id}";
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    aircraft = new Aircraft(Convert.ToInt32(reader["AircraftID"]), Convert.ToString(reader["AircraftRegistration"]), Convert.ToString(reader["AircraftDescription"]), Convert.ToInt32(reader["AircraftTypeID"]), Convert.ToInt32(reader["AircraftCountryID"]), Convert.ToInt32(reader["AircraftAirlineID"]));
                }
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            return aircraft;
        }

        public List<SpottingTrip> GetSpottingTrips()
        {
            List<SpottingTrip> spottingTrips = new List<SpottingTrip>();
            try
            {
                con.Open();
                cmd.CommandText = "SELECT * FROM SpottingTrips";
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    SpottingTrip spottingTrip = new SpottingTrip(Convert.ToInt32(reader["SpottingTripID"]), Convert.ToDateTime(reader["SpottingTripStart"]), Convert.ToDateTime(reader["SpottingTripEnd"]), Convert.ToString(reader["SpottingTripName"]), Convert.ToString(reader["SpottingTripDescription"]));
                    spottingTrips.Add(spottingTrip);
                }
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            return spottingTrips;
        }

        public SpottingTrip GetSpottingTripByID(int id)
        {
            SpottingTrip spottingTrip = null;
            try
            {
                con.Open();
                cmd.CommandText = $"SELECT * FROM SpottingTrips WHERE SpottingTripID = {id}";
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    spottingTrip = new SpottingTrip(Convert.ToInt32(reader["SpottingTripID"]), Convert.ToDateTime(reader["SpottingTripStart"]), Convert.ToDateTime(reader["SpottingTripEnd"]), Convert.ToString(reader["SpottingTripName"]), Convert.ToString(reader["SpottingTripDescription"]));
                }
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            return spottingTrip;
        }

        public List<SpottingPicture> GetSpottingPictures()
        {
            List<SpottingPicture> spottingPictures = new List<SpottingPicture>();
            try
            {
                con.Open();
                cmd.CommandText = "SELECT * FROM SpottingPictures";
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    SpottingPicture spottingPicture = new SpottingPicture(Convert.ToInt32(reader["SpottingPictureID"]), Convert.ToString(reader["SpottingPictureName"]), Convert.ToString(reader["SpottingPictureDescription"]), Convert.ToString(reader["SpottingPictureURL"]), Convert.ToString(reader["SpottingPictureOriginalFileName"]), Convert.ToInt32(reader["SpottingPictureSpottingTripID"]), Convert.ToInt32(reader["SpottingPictureAircraftID"]));
                    spottingPictures.Add(spottingPicture);
                }
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            return spottingPictures;
        }

        public SpottingPicture GetSpottingPictureByID(int id)
        {
            SpottingPicture spottingPicture = null;
            try
            {
                con.Open();
                cmd.CommandText = $"SELECT * FROM SpottingPictures WHERE SpottingPictureID = {id}";
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    spottingPicture = new SpottingPicture(Convert.ToInt32(reader["SpottingPictureID"]), Convert.ToString(reader["SpottingPictureName"]), Convert.ToString(reader["SpottingPictureDescription"]), Convert.ToString(reader["SpottingPictureURL"]), Convert.ToString(reader["SpottingPictureOriginalFileName"]), Convert.ToInt32(reader["SpottingPictureSpottingTripID"]), Convert.ToInt32(reader["SpottingPictureAircraftID"]));
                }
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            return spottingPicture;
        }

        public string GetNewestImageFromCountry(int Country)
        {
            string newestImage = "";
            try
            {
                con.Open();
                cmd.CommandText = $"SELECT sp.SpottingPictureURL FROM Countries c JOIN Airports a ON c.CountryID = a.CountryID JOIN SpottingTripAirports sta ON a.AirportID = sta.AirportID JOIN SpottingPictures sp ON sp.SpottingPictureSpottingTripID = sta.SpottingTripID WHERE c.CountryID = {Country};";
                reader = cmd.ExecuteReader();
                {
                    while(reader.Read())
                    {
                        newestImage = Convert.ToString(reader[0]);
                    }
                }
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
            }

            return newestImage;
        }
    }
}


