using Microsoft.Data.SqlClient;
using SpottersDB_BackEnd.Classes.Structure;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

namespace SpottersDB_BackEnd.Classes.Utilities
{
    public class SQLController
    {
        #region Instances for reuse
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1,1);
        private SqlConnection con = new SqlConnection("server = (localdb)\\MSSQLLocalDB; integrated security = false;");
        private SqlCommand cmd = null;
        List<Dictionary<string, object>> ReaderData;
        private WebApplication app;
        // JUST FOR DEBUGGING
        private bool isDebugMode = false;
        #endregion

        #region ExecuteCommand Methods
        private bool ExecuteCMD(string CMDText, string LogText)
        {
            semaphore.Wait();
            int SuccessInt = 0;
            try
            {
                con.Open();
                cmd.CommandText = CMDText;
                SuccessInt = cmd.ExecuteNonQuery();
                app.Logger.LogInformation($"[{DateTime.Now.ToString("HH:mm:ss")}]: {LogText}");
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
                con.Close();
            }
            semaphore.Release();
            return SuccessInt > 0;
        }

        private object ExecuteCMDScalar(string CMDText, string LogText)
        {
            semaphore.Wait();
            Object obj = null;
            try
            {
                con.Open();
                cmd.CommandText = CMDText;
                obj = cmd.ExecuteScalar();
                app.Logger.LogInformation($"[{DateTime.Now.ToString("HH:mm:ss")}]: {LogText}");
                con.Close();
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
                con.Close();
            }
            semaphore.Release();
            return obj;
        }

        private List<Dictionary<string, object>> ExecuteReadCMD(string CMDText, string LogText)
        {
            semaphore.Wait();
            List<Dictionary<string, object>> ReaderData = new List<Dictionary<string, object>>();
            try
            {
                con.Open();
                cmd.CommandText = CMDText;
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    Dictionary<string, object> pair = new Dictionary<string, object>();
                    for(int i = 0; i < reader.FieldCount; i++)
                    {
                        pair[reader.GetName(i)] = reader.GetValue(i);
                    }
                    ReaderData.Add(pair);
                }
                con.Close();
                app.Logger.LogInformation($"[{DateTime.Now.ToString("HH:mm:ss")}]: {LogText}");
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
                con.Close();
            }
            semaphore.Release();
            return ReaderData;
        }
        #endregion

        #region Setting UP DB
        // Checks if DB Exists
        public void ConnectToDB(string DatabaseName, WebApplication app)
        {
            // Access the WebApp for logger & stuff
            this.app = app;
            cmd = new SqlCommand("", con);
            // JUST FOR DEBUGGING
            if (isDebugMode)
            {
                ExecuteCMD($"DROP DATABASE {DatabaseName}", "DEBUG: Dropping the DB");
                isDebugMode = false;
            }


            // Trys to connect to DB
            ReaderData = ExecuteReadCMD("SELECT name FROM sys.databases", "Checking if Database exists");
            bool DatabaseExists = false;
            foreach (Dictionary<string, object> Data in ReaderData)
            {
                if (!DatabaseExists)
                {
                    DatabaseExists = Data["name"] == DatabaseName;
                }
            }

            if (DatabaseExists)
            {
                con.ConnectionString = "server = (localdb)\\MSSQLLocalDB; integrated security = false; database = " + DatabaseName;
            }
            else
            {
                CreateDatabase(DatabaseName);
            }
        }

        // Creates the DB
        private void CreateDatabase(string DatabaseName)
        {
            // Delete any images that are for some reason in the Image Directory
            foreach (string f in Directory.GetFiles(Path.GetFullPath(Environment.CurrentDirectory) + "/Images"))
            {
                File.Delete(f);
            }

            // Creates the DB
            ExecuteCMD("CREATE DATABASE " + DatabaseName, "Created the Database: " + DatabaseName);
            con.ConnectionString = "server = (localdb)\\MSSQLLocalDB; integrated security = false; database = " + DatabaseName;

            // Creates the Countries Table
            ExecuteCMD("CREATE TABLE Countries (CountryID int NOT NULL PRIMARY KEY IDENTITY, CountryICAOCode varchar(10), CountryName varchar(255))", "Created the Table Countries");

            // Creates the Airports Table
            ExecuteCMD("CREATE TABLE Airports (AirportID int NOT NULL PRIMARY KEY IDENTITY, AirportICAOCode char(4), AirportIATACode char(3), AirportName text, AirportDescription text, AirportCity varchar(255), CountryID int, CONSTRAINT [FK_Airport_Country] FOREIGN KEY ([CountryID]) REFERENCES [Countries](CountryID))", "Created the Table Airports");

            // Creates the Airline Table
            ExecuteCMD("CREATE TABLE Airlines (AirlineID int NOT NULL PRIMARY KEY IDENTITY, AirlineICAOCode char(3), AirlineIATACode char(2), AirlineName text, AirlineRegion int, CONSTRAINT [FK_Airline_Country] FOREIGN KEY ([AirlineRegion]) REFERENCES [Countries](CountryID))", "Created the Table Airlines");

            // Create the Manufactorer Table
            ExecuteCMD("CREATE TABLE Manufactorers (ManufactorerID int NOT NULL PRIMARY KEY IDENTITY, ManufactorerName text, ManufactorerRegion int, CONSTRAINT [FK_Manufactorer_Country] FOREIGN KEY ([ManufactorerRegion]) REFERENCES [Countries](CountryID))", "Created the Table Manufactorers");

            // Create the AircraftType Table
            ExecuteCMD("CREATE TABLE AircraftTypes (AircraftTypeID int NOT NULL PRIMARY KEY IDENTITY, AircraftTypeICAO varchar(10), AircraftTypeName text, AircraftTypeNickName text, AircraftTypeManufactorerID int, CONSTRAINT [FK_AircraftType_Manufactorer] FOREIGN KEY ([AircraftTypeManufactorerID]) REFERENCES [Manufactorers](ManufactorerID))", "Created the Table AircraftTypes");

            // Create the Aircraft Table
            ExecuteCMD("CREATE TABLE Aircrafts (AircraftID int NOT NULL PRIMARY KEY IDENTITY, AircraftRegistration varchar(6), AircraftDescription text, AircraftTypeID int, AircraftCountryID int, AircraftAirlineID int, CONSTRAINT [FK_Aircraft_AircraftType] FOREIGN KEY ([AircraftTypeID]) REFERENCES [AircraftTypes](AircraftTypeID), CONSTRAINT [FK_Aircraft_Country] FOREIGN KEY ([AircraftCountryID]) REFERENCES [Countries](CountryID), CONSTRAINT [FK_Aircraft_Airline] FOREIGN KEY ([AircraftAirlineID]) REFERENCES [Airlines](AirlineID))", "Created the Table Aircrafts");

            // Create Spotting Trip Table
            ExecuteCMD("CREATE TABLE SpottingTrips (SpottingTripID int NOT NULL PRIMARY KEY IDENTITY, SpottingTripStart datetime2(0), SpottingTripEnd datetime2(0), SpottingTripName text, SpottingTripDescription text)", "Created the Table SpottingTrips");

            // Create SpottingTrip & Airport Link Table
            ExecuteCMD("CREATE TABLE SpottingTripAirports (LinkID int NOT NULL PRIMARY KEY IDENTITY, SpottingTripID int, AirportID int, CONSTRAINT [FK_SpottingTripAirport_SpottingTrip] FOREIGN KEY ([SpottingTripID]) REFERENCES [SpottingTrips](SpottingTripID), CONSTRAINT [FK_SpottingTripAirport_Airport] FOREIGN KEY ([AirportID]) REFERENCES [Airports](AirportID))", "Created the Link Table for SpottingTrips and Airports");

            // Create SpottingPicture Table
            ExecuteCMD("CREATE TABLE SpottingPictures (SpottingPictureID int NOT NULL PRIMARY KEY IDENTITY, SpottingPictureName text, SpottingPictureDescription text, SpottingPictureURL text, SpottingPictureOriginalFileName text, SpottingTripAirportID int, SpottingPictureAircraftID int, CONSTRAINT [FK_SpottingPicture_SpottingTripAirport] FOREIGN KEY ([SpottingTripAirportID]) REFERENCES [SpottingTripAirports](LinkID) ON DELETE SET NULL, CONSTRAINT [FK_SpottingPicture_Aircraft] FOREIGN KEY ([SpottingPictureAircraftID]) REFERENCES [Aircrafts](AircraftID))", "Created the Table SpottingPictures");
        }
        #endregion

        #region Add Objects
        public bool AddCountry(Country country)
        {
            return ExecuteCMD($"INSERT INTO Countries (CountryICAOCode, CountryName) VALUES ('{country.ICAO_Code}', '{country.Name}')", "Saved a Country Object");
        }

        public bool AddAirport(Airport airport)
        {
            return ExecuteCMD($"INSERT INTO Airports (AirportICAOCode, AirportIATACode, AirportName, AirportDescription, AirportCity, CountryID) VALUES ('{airport.ICAO_Code}', '{airport.IATA_Code}', '{airport.Name}', '{airport.Description}', '{airport.City}', '{airport.CountryID}')", "Saved an Airport Object");
        }

        public bool AddAirline(Airline airline)
        {
           return ExecuteCMD($"INSERT INTO Airlines (AirlineICAOCode, AirlineIATACode, AirlineName, AirlineRegion) VALUES ('{airline.ICAO}', '{airline.IATA}', '{airline.Name}', '{airline.Region}')", "Saved an Airline Object");
        }

        public bool AddManufactorer(Manufactorer manufactorer)
        {
           return ExecuteCMD($"INSERT INTO Manufactorers (ManufactorerName, ManufactorerRegion) VALUES ('{manufactorer.Name}','{manufactorer.Region}')", "Saved a Manufactorer Object");
        }

        public bool AddAircraftType(AircraftType aircraftType)
        {
            return ExecuteCMD($"INSERT INTO AircraftTypes (AircraftTypeICAO, AircraftTypeName, AircraftTypeNickName, AircraftTypeManufactorerID) VALUES ('{aircraftType.ICAOCode}', '{aircraftType.FullName}', '{aircraftType.NickName}', '{aircraftType.ManufactorerID}')", "Saved an AircraftType Object");
        }

        public bool AddAircraft(Aircraft aircraft)
        {
            return ExecuteCMD($"INSERT INTO Aircrafts (AircraftRegistration, AircraftDescription, AircraftTypeID, AircraftCountryID, AircraftAirlineID) VALUES('{aircraft.Registration}', '{aircraft.Description}', '{aircraft.TypeID}', '{aircraft.CountryID}', '{aircraft.AirlineID}')", "Saved an Aircraft Object");
        }

        public bool AddSpottingTrip(SpottingTrip spottingTrip, List<int> AirportIDs)
        {
            Object obj = ExecuteCMDScalar($"INSERT INTO SpottingTrips (SpottingTripStart, SpottingTripEnd, SpottingTripName, SpottingTripDescription) VALUES ('{spottingTrip.Start.ToString("yyyy-MM-dd HH:mm:ss")}', '{spottingTrip.End.ToString("yyyy-MM-dd HH:mm:ss")}', '{spottingTrip.Name}', '{spottingTrip.Description}'); SELECT SCOPE_IDENTITY()", "Saved Spotting Trip");
            int ID = Convert.ToInt32(obj);

            bool Successful = true;
            foreach(int AirportID in AirportIDs)
            {
                if(Successful)
                {
                    Successful = ExecuteCMD($"INSERT INTO SpottingTripAirports (SpottingTripID, AirportID) VALUES ('{ID}', '{AirportID}')", "Saved an Link Item between Spottingtrip & Airport");
                }
            }
            return Successful;
        }

        public bool AddSpottingPicture(SpottingPicture spottingPicture)
        {
            return ExecuteCMD($"INSERT INTO SpottingPictures (SpottingPictureName, SpottingPictureDescription, SpottingPictureURL, SpottingPictureOriginalFileName, SpottingTripAirportID, SpottingPictureAircraftID) VALUES ('{spottingPicture.Name}','{spottingPicture.Description}','{spottingPicture.PictureUrl}','{spottingPicture.OriginalFileName}','{spottingPicture.SpottingTripAirportID}','{spottingPicture.AircraftID}')", "Saved a SpottingPicture Object");
        }
        #endregion

        #region Update Objects
        public bool UpdateCountry(Country country)
        {
            return ExecuteCMD($"UPDATE Countries SET CountryICAOCode = '{country.ICAO_Code}', CountryName = '{country.Name}' WHERE CountryID = {country.ID}", "Updated Countries Object");
        }

        public bool UpdateAirport(Airport airport)
        {
            return ExecuteCMD($"UPDATE Airports SET AirportICAOCode = '{airport.ICAO_Code}', AirportIATACode = '{airport.IATA_Code}', AirportName = '{airport.Name}', AirportDescription = '{airport.Description}', AirportCity = '{airport.City}', CountryID = {airport.CountryID} WHERE AirportID = {airport.ID}", "Updated Airport Object");
        }

        public bool UpdateAirline(Airline airline)
        {
            return ExecuteCMD($"UPDATE Airlines SET AirlineICAOCode = '{airline.ICAO}', AirlineIATACode = '{airline.IATA}', AirlineName = '{airline.Name}', AirlineRegion = {airline.Region} WHERE AirlineID = {airline.ID}", "Updated Airline Object");
        }

        public bool UpdateAircraftType(AircraftType aircraftType)
        {
           return ExecuteCMD($"UPDATE AircraftTypes SET AircraftTypeICAO = '{aircraftType.ICAOCode}', AircraftTypeName = '{aircraftType.FullName}', AircraftTypeNickName = '{aircraftType.NickName}', AircraftTypeManufactorerID = {aircraftType.ManufactorerID} WHERE AircraftTypeID = {aircraftType.ID}", "Updated AircraftType Object");
        }

        public bool UpdateManufactorer(Manufactorer manufactorer)
        {
            return ExecuteCMD($"UPDATE Manufactorers SET ManufactorerName = '{manufactorer.Name}', ManufactorerRegion = {manufactorer.Region} WHERE ManufactorerID = {manufactorer.ID}", "Updated a Manufactorer Object");
        }

        public bool UpdateAircraft(Aircraft aircraft)
        {
            return ExecuteCMD($"UPDATE Aircrafts SET AircraftRegistration = '{aircraft.Registration}', AircraftDescription = '{aircraft.Description}', AircraftTypeID = {aircraft.TypeID}, AircraftCountryID = {aircraft.CountryID}, AircraftAirlineID = {aircraft.AirlineID} WHERE AircraftID = {aircraft.ID}", "Updated an Aircraft Object");
        }

        public bool UpdateSpottingTrip(SpottingTrip spottingTrip, List<int> AirportIDs)
        {
            bool IsSuccessfull = true;
            IsSuccessfull = ExecuteCMD($"UPDATE SpottingTrips SET SpottingTripStart = '{spottingTrip.Start.ToString("yyyy-MM-dd HH:mm:ss")}', SpottingTripEnd = '{spottingTrip.End.ToString("yyyy-MM-dd HH:mm:ss")}', SpottingTripName = '{spottingTrip.Name}', SpottingTripDescription = '{spottingTrip.Description}' WHERE SpottingTripID = {spottingTrip.ID}", "Updating the SpottingTrip Object");

            ReaderData = ExecuteReadCMD($"SELECT AirportID FROM SpottingTripAirports WHERE SpottingTripID = {spottingTrip.ID}", "Getting all AirportIDs from a SpottingTrip");
            List<int> Airports2Delete = new List<int>();
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                int ID = Convert.ToInt32(Data["AirportIDs"]);
                if (AirportIDs.Contains(ID))
                {
                    AirportIDs.Remove(ID);
                }
                else
                {
                    Airports2Delete.Add(ID);
                }
            }

            foreach(int airport2delete in Airports2Delete)
            {
                if(IsSuccessfull)
                {
                    IsSuccessfull = ExecuteCMD($"DELETE FROM SpottingTripAirports WHERE AirportID = {airport2delete} AND SpottingTripID = {spottingTrip.ID}", "Delete Airports from SpottingTrip");
                }
                
            }

            foreach(int AirportID in AirportIDs)
            {
                if(IsSuccessfull)
                {
                  IsSuccessfull = ExecuteCMD($"INSERT INTO SpottingTripAirports (SpottingTripID, AirportID) VALUES ('{spottingTrip.ID}', '{AirportID}')", "Add Airports to SpottingTrip");
                }
            }
            return IsSuccessfull;
        }

        public bool UpdateSpottingPicture(SpottingPicture spottingPicture)
        {
            return ExecuteCMD($"UPDATE SpottingPictures SET SpottingPictureName = '{spottingPicture.Name}', SpottingPictureDescription = '{spottingPicture.Description}', SpottingPictureURL = '{spottingPicture.PictureUrl}', SpottingPictureOriginalFileName = '{spottingPicture.OriginalFileName}', SpottingTripAirportID = {spottingPicture.SpottingTripAirportID}, SpottingPictureAircraftID = {spottingPicture.AircraftID} WHERE SpottingPictureID = {spottingPicture.ID}", "Updated a SpottingPicture Object");
        }
        #endregion

        #region Get Objects
        public List<Country> GetCountries()
        {
            List<Country> Countries = new List<Country>();
            ReaderData = ExecuteReadCMD($"SELECT * FROM Countries", "Reading Country/Region Objects");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                Country country = new Country(Convert.ToInt32(Data["CountryID"]), Convert.ToString(Data["CountryICAOCode"]), Convert.ToString(Data["CountryName"]));
                Countries.Add(country);
            }
            return Countries;
        }

        public List<Country> GetCountries(bool OnlyCountries)
        {
            List<Country> Countries = new List<Country>();
            ReaderData = ExecuteReadCMD($"SELECT * FROM Countries WHERE NOT CountryICAOCode = ''", "Reading Only Country Objects");
            foreach(Dictionary<string , object> Data in ReaderData)
            {
                Country country = new Country(Convert.ToInt32(Data["CountryID"]), Convert.ToString(Data["CountryICAOCode"]), Convert.ToString(Data["CountryName"]));
                Countries.Add(country);
            }
            return Countries;
        }

        public List<Country> GetRegions()
        {
            List<Country> Countries = new List<Country>();
            ReaderData = ExecuteReadCMD($"SELECT * FROM Countries WHERE CountryICAOCode = ''", "Reading Only Region Objects");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                Country country = new Country(Convert.ToInt32(Data["CountryID"]), Convert.ToString(Data["CountryICAOCode"]), Convert.ToString(Data["CountryName"]));
                Countries.Add(country);
            }
            return Countries;
        }

        public Country GetCountryByID(int ID)
        {
            Country country = null;
            ReaderData = ExecuteReadCMD($"SELECT * FROM Countries WHERE CountryID = {ID}", $"Reading the Country Object with the ID = {ID}");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                country = new Country(Convert.ToInt32(Data["CountryID"]), Convert.ToString(Data["CountryICAOCode"]), Convert.ToString(Data["CountryName"]));
            }
            return country;
        }

        public List<Airport> GetAirports()
        {
            List<Airport> Airports = new List<Airport>();
            ReaderData = ExecuteReadCMD($"SELECT * FROM Airports", "Reading Airport Objects");
            foreach(Dictionary <string, object> Data in ReaderData)
            {
                Airport airport = new Airport(Convert.ToInt32(Data["AirportID"]), Convert.ToString(Data["AirportICAOCode"]), Convert.ToString(Data["AirportIATACode"]), Convert.ToString(Data["AirportName"]), Convert.ToString(Data["AirportDescription"]), Convert.ToString(Data["AirportCity"]), Convert.ToInt32(Data["CountryID"]));
                Airports.Add(airport);
            }
            return Airports;
        }

        public Airport GetAirportByID(int ID)
        {
            Airport airport = null;
            ReaderData = ExecuteReadCMD($"SELECT * FROM Airports WHERE AirportID = {ID}", $"Reading Airport Object with ID {ID}");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                airport = new Airport(Convert.ToInt32(Data["AirportID"]), Convert.ToString(Data["AirportICAOCode"]), Convert.ToString(Data["AirportIATACode"]), Convert.ToString(Data["AirportName"]), Convert.ToString(Data["AirportDescription"]), Convert.ToString(Data["AirportCity"]), Convert.ToInt32(Data["CountryID"]));
            }
            return airport;
        }

        public List<Airline> GetAirlines()
        {
            List<Airline> Airlines = new List<Airline>();
            ReaderData = ExecuteReadCMD($"SELECT * FROM Airlines", "Reading Airline Objects");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                Airline airline = new Airline(Convert.ToInt32(Data["AirlineID"]), Convert.ToString(Data["AirlineICAOCode"]), Convert.ToString(Data["AirlineIATACode"]), Convert.ToString(Data["AirlineName"]), Convert.ToInt32(Data["AirlineRegion"]));
                Airlines.Add(airline);
            }
            return Airlines;
        }

        public Airline GetAirlineByID(int ID)
        {
            Airline airline = null;
            ReaderData = ExecuteReadCMD($"SELECT * FROM Airlines WHERE AirlineID = {ID}", $"Reading Airline Object with ID {ID}");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                airline = new Airline(Convert.ToInt32(Data["AirlineID"]), Convert.ToString(Data["AirlineICAOCode"]), Convert.ToString(Data["AirlineIATACode"]), Convert.ToString(Data["AirlineName"]), Convert.ToInt32(Data["AirlineRegion"]));
            }
            return airline;
        }

        public List<AircraftType> GetAircraftTypes()
        {
            List<AircraftType> AircraftTypes = new List<AircraftType>();
            ReaderData = ExecuteReadCMD($"SELECT * FROM AircraftTypes", "Reading the AircraftType Objects");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                AircraftType aircraftType = new AircraftType(Convert.ToInt32(Data["AircraftTypeID"]), Convert.ToString(Data["AircraftTypeICAO"]), Convert.ToString(Data["AircraftTypeName"]), Convert.ToString(Data["AircraftTypeNickName"]), Convert.ToInt32(Data["AircraftTypeManufactorerID"]));
                AircraftTypes.Add(aircraftType);
            }
            return AircraftTypes;
        }

        public AircraftType GetAircraftTypeByID(int ID)
        {
            AircraftType aircraftType = null;
            ReaderData = ExecuteReadCMD($"SELECT * FROM AircraftTypes WHERE AircraftTypeID = {ID}", $"Reading the AircraftType Object with the ID {ID}");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                aircraftType = new AircraftType(Convert.ToInt32(Data["AircraftTypeID"]), Convert.ToString(Data["AircraftTypeICAO"]), Convert.ToString(Data["AircraftTypeName"]), Convert.ToString(Data["AircraftTypeNickName"]), Convert.ToInt32(Data["AircraftTypeManufactorerID"]));
            }
            return aircraftType;
        }

        public List<Manufactorer> GetManufactorers()
        {
            List<Manufactorer> Manufactorers = new List<Manufactorer>();
            ReaderData = ExecuteReadCMD($"SELECT * FROM Manufactorers", "Reading the Manufactorer Objects");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                Manufactorer manufactorer = new Manufactorer(Convert.ToInt32(Data["ManufactorerID"]), Convert.ToString(Data["ManufactorerName"]), Convert.ToInt32(Data["ManufactorerRegion"]));
                Manufactorers.Add(manufactorer);
            }
            return Manufactorers;
        }

        public Manufactorer GetManufactorerByID(int id)
        {
            Manufactorer manufactorer = null;
            ReaderData = ExecuteReadCMD($"SELECT * FROM Manufactorers WHERE ManufactorerID = {id}", $"Reading the Manufactorer Object with the ID {id}");
            foreach(Dictionary <string, object> Data in ReaderData)
            {
                manufactorer = new Manufactorer(Convert.ToInt32(Data["ManufactorerID"]), Convert.ToString(Data["ManufactorerName"]), Convert.ToInt32(Data["ManufactorerRegion"]));
            }
            return manufactorer;
        }

        public List<Aircraft> GetAircrafts()
        {
            List<Aircraft> aircrafts = new List<Aircraft>();
            ReaderData = ExecuteReadCMD("SELECT * FROM Aircrafts", "Reading Aircraft Objects");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                Aircraft aircraft = new Aircraft(Convert.ToInt32(Data["AircraftID"]), Convert.ToString(Data["AircraftRegistration"]), Convert.ToString(Data["AircraftDescription"]), Convert.ToInt32(Data["AircraftTypeID"]), Convert.ToInt32(Data["AircraftCountryID"]), Convert.ToInt32(Data["AircraftAirlineID"]));
                aircrafts.Add(aircraft);
            }
            return aircrafts;
        }

        public Aircraft GetAircraftByID(int id)
        {
            Aircraft aircraft = null;
            ReaderData = ExecuteReadCMD($"SELECT * FROM Aircrafts WHERE AircraftID = {id}", $"Reading Aircraft Object with the ID {id}");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                aircraft = new Aircraft(Convert.ToInt32(Data["AircraftID"]), Convert.ToString(Data["AircraftRegistration"]), Convert.ToString(Data["AircraftDescription"]), Convert.ToInt32(Data["AircraftTypeID"]), Convert.ToInt32(Data["AircraftCountryID"]), Convert.ToInt32(Data["AircraftAirlineID"]));
            }
            return aircraft;
        }

        public List<SpottingTrip> GetSpottingTrips()
        {
            List<SpottingTrip> spottingTrips = new List<SpottingTrip>();
            ReaderData = ExecuteReadCMD("SELECT * FROM SpottingTrips", "Reading SpottingTrip Objects");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                SpottingTrip spottingTrip = new SpottingTrip(Convert.ToInt32(Data["SpottingTripID"]), Convert.ToDateTime(Data["SpottingTripStart"]), Convert.ToDateTime(Data["SpottingTripEnd"]), Convert.ToString(Data["SpottingTripName"]), Convert.ToString(Data["SpottingTripDescription"]));
                spottingTrips.Add(spottingTrip);
            }
            return spottingTrips;
        }

        public SpottingTrip GetSpottingTripByID(int id)
        {
            SpottingTrip spottingTrip = null;
            ReaderData = ExecuteReadCMD($"SELECT * FROM SpottingTrips WHERE SpottingTripID = {id}", $"Reading SpottingTrip with ID {id}");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                spottingTrip = new SpottingTrip(Convert.ToInt32(Data["SpottingTripID"]), Convert.ToDateTime(Data["SpottingTripStart"]), Convert.ToDateTime(Data["SpottingTripEnd"]), Convert.ToString(Data["SpottingTripName"]), Convert.ToString(Data["SpottingTripDescription"]));
            }
            return spottingTrip;
        }

        public List<SpottingPicture> GetSpottingPictures()
        {
            List<SpottingPicture> spottingPictures = new List<SpottingPicture>();
            ReaderData = ExecuteReadCMD("SELECT * FROM SpottingPictures", "Reading SpottingPicture Objects");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                SpottingPicture spottingPicture = new SpottingPicture(Convert.ToInt32(Data["SpottingPictureID"]), Convert.ToString(Data["SpottingPictureName"]), Convert.ToString(Data["SpottingPictureDescription"]), Convert.ToString(Data["SpottingPictureURL"]), Convert.ToString(Data["SpottingPictureOriginalFileName"]), Convert.ToInt32(Data["SpottingTripAirportID"]), Convert.ToInt32(Data["SpottingPictureAircraftID"]));
                spottingPictures.Add(spottingPicture);
            }
            return spottingPictures;
        }

        public SpottingPicture GetSpottingPictureByID(int id)
        {
            SpottingPicture spottingPicture = null;
            ReaderData = ExecuteReadCMD($"SELECT * FROM SpottingPictures WHERE SpottingPictureID = {id}", $"Reading SpottingPicture with ID {id}");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                spottingPicture = new SpottingPicture(Convert.ToInt32(Data["SpottingPictureID"]), Convert.ToString(Data["SpottingPictureName"]), Convert.ToString(Data["SpottingPictureDescription"]), Convert.ToString(Data["SpottingPictureURL"]), Convert.ToString(Data["SpottingPictureOriginalFileName"]), Convert.ToInt32(Data["SpottingPictureSpottingTripID"]), Convert.ToInt32(Data["SpottingPictureAircraftID"]));
            }
            return spottingPicture;
        }

        public string GetNewestImageFromCountry(int Country)
        {
            string newestImage = "";
            ReaderData = ExecuteReadCMD($"SELECT sp.SpottingPictureURL FROM Countries c JOIN Airports a ON c.CountryID = a.CountryID JOIN SpottingTripAirports sta ON a.AirportID = sta.AirportID JOIN SpottingPictures sp ON sp.SpottingTripAirportID = sta.LinkID WHERE c.CountryID = {Country}", "Getting the newest Picture from a Country");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                newestImage = Convert.ToString(Data["SpottingPictureURL"]);
            }
            return newestImage;
        }

        public List<Airport> GetAirportsFromSpottingTrip(int ID)
        {
            List<Airport> airports = new List<Airport>();
            ReaderData = ExecuteReadCMD($"SELECT a.* FROM SpottingTripAirports sta JOIN Airports a ON sta.AirportID = a.AirportID WHERE sta.SpottingTripID = {ID}", $"Getting Airports from SpottingTrip with ID {ID}");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                airports.Add(new Airport(Convert.ToInt32(Data["AirportID"]), Convert.ToString(Data["AirportICAOCode"]), Convert.ToString(Data["AirportIATACode"]), Convert.ToString(Data["AirportName"]), Convert.ToString(Data["AirportDescription"]), Convert.ToString(Data["AirportCity"]), Convert.ToInt32(Data["CountryID"])));
            }
            return airports;
        }

        public int GetLinkID(int SpottingTripID, int AirportID)
        {
            int LinkID = -1;
            ReaderData = ExecuteReadCMD($"SELECT LinkID FROM SpottingTripAirports WHERE SpottingTripID = {SpottingTripID} AND AirportID = {AirportID}", $"Getting LinkID from SpottingTrip {SpottingTripID} and Airport {AirportID}");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                LinkID = Convert.ToInt32(Data["LinkID"]);
            }
            return LinkID;
        }

        public Dictionary<string, int> GetSpottingTripAirportFromLinkID(int LinkID)
        {
            Dictionary<string, int> IDs = new Dictionary<string, int>();
            ReaderData = ExecuteReadCMD($"SELECT SpottingTripID, AirportID FROM SpottingTripAirports WHERE LinkID = {LinkID}", $"Getting Airport & SpottingTrip from LinkID {LinkID}");
            foreach(Dictionary<string, object> Data in ReaderData)
            {
                IDs.Add("SpottingTrip", Convert.ToInt32(Data["SpottingTripID"]));
                IDs.Add("Airport", Convert.ToInt32(Data["AirportID"]));
            }
            return IDs;
        }
        #endregion

        #region Delete Objects
        public bool DeleteCountryByID(int CountryID)
        {
            return ExecuteCMD($"DELETE FROM Countries WHERE CountryID = {CountryID}", $"Deleting Country with ID {CountryID}");
        }



        #endregion
    }
}


