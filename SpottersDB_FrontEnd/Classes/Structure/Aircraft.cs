using SpottersDB_FrontEnd.Classes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.Structure
{
    public class Aircraft
    {
        private int _ID;
        private string _Registration;
        private string _Description;
        private int _TypeID;
        private int _CountryID;
        private int _AirlineID;

        public int id
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        public string registration
        {
            get
            {
                return _Registration;
            }
            set
            {
                _Registration = value;
            }
        }

        public string description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }

        public int typeID
        {
            get
            {
                return _TypeID;
            }
            set
            {
                _TypeID = value;
            }
        }

        public int countryID
        {
            get
            {
                return _CountryID;
            }
            set
            {
                _CountryID = value;
            }
        }

        public int airlineID
        {
            get
            {
                return _AirlineID;
            }
            set
            {
                _AirlineID = value;
            }
        }

        public Aircraft()
        {

        }

        public Aircraft(string Registration, string Description, int TypeID, int AirlineID, int CountryID)
        {
            this.registration = Registration;
            this.description = Description;
            this.typeID = TypeID;
            this.airlineID = AirlineID;
            this.countryID = CountryID;
        }

        public Aircraft(int ID, string Registration, string Description, int TypeID, int AirlineID, int CountryID)
        {
            this.id = ID;
            this.registration = Registration;
            this.description = Description;
            this.typeID = TypeID;
            this.airlineID = AirlineID;
            this.countryID = CountryID;
        }

        public async Task<AircraftType> GetAircraftType()
        {
            return await HTTP_Controller.GetAircraftTypeByID(typeID);
        }

        public async Task<Airline> GetAirline()
        {
            return await HTTP_Controller.GetAirlineByID(airlineID);
        }

        public async Task<Country> GetCountry()
        {
            return await HTTP_Controller.GetCountryByID(countryID);
        }
    }
}
