using SpottersDB_BackEnd.Classes.Exceptions;

namespace SpottersDB_BackEnd.Classes.Structure
{
    public class Airport
    {
        private int _ID;
        private string _ICAO_Code;
        private string _IATA_Code;
        private string _Name;
        private string _Description;
        private string _City;
        private int _CountryID;

        public int ID
        {
            get
            {
                return _ID;
            }
            private set
            {
                _ID = value;
            }
        }

        public string ICAO_Code
        {
            get
            {
                return _ICAO_Code;
            }
            set
            {
                // Check if valid ICAO Airport Code (Every ICAO Airport Code is 4 Letters)
                if(value.Length == 4)
                {
                    _ICAO_Code = value;
                }
                else
                {
                    throw new Exception_Invalid_Airport_Code("Invalid ICAO Airport Code");
                }
            }
        }

        public string IATA_Code
        {
            get
            {
                return _IATA_Code;
            }
            set
            {
                // Check if valid IATA Airport Code (Every IATA Airport is 3 Letters)
                if(value.Length == 3)
                {
                    _IATA_Code = value;
                }
                else
                {
                    throw new Exception_Invalid_Airport_Code("Invalid IATA Airport Code");
                }
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        public string Description
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

        public string City
        {
            get
            {
                return _City;
            }
            set
            {
                _City = value;
            }
        }

        public int CountryID
        {
            get
            {
                return _CountryID;
            }
            set
            {
                // Maybe add a check later
                _CountryID = value;
            }
        }

        // Empty Constructor to avoid bullying from evil compiler
        public Airport()
        {

        }

        // Full Constructor
        public Airport(string ICAO, string IATA, string Name, string Description, string City, int CountryID)
        {
            this.ICAO_Code = ICAO;
            this.IATA_Code = IATA;
            this.Name = Name;
            this.Description = Description;
            this.City = City;
            this.CountryID = CountryID;
        }

        public Airport(int ID, string ICAO, string IATA, string Name, string Description, string City, int CountryID)
        {
            this.ID = ID;
            this.ICAO_Code = ICAO;
            this.IATA_Code = IATA;
            this.Name = Name;
            this.Description = Description;
            this.City = City;
            this.CountryID = CountryID;
        }
    }
}
