using SpottersDB_FrontEnd.Classes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.Structure
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

        public string icaO_Code
        {
            get
            {
                return _ICAO_Code;
            }
            set
            {
                _ICAO_Code = value;
            }
        }

        public string iatA_Code
        {
            get
            {
                return _IATA_Code;
            }
            set
            {
                _IATA_Code = value;
            }
        }

        public string name
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

        public string city
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

        public Airport()
        {

        }

        public Airport(string ICAO, string IATA, string Name, string Description, string City, int countryID)
        {
            this.icaO_Code = ICAO;
            this.iatA_Code = IATA;
            this.name = Name;
            this.description = Description;
            this.city = City;
            this.countryID = countryID;
        }

        public Airport(int ID, string ICAO, string IATA, string Name, string Description, string City, int countryID)
        {
            this.id = ID;
            this.icaO_Code = ICAO;
            this.iatA_Code = IATA;
            this.name = Name;
            this.description = Description;
            this.city = City;
            this.countryID = countryID;
        }

        public async Task<Country> GetRegion()
        {
            Thread.Sleep(50);
            return await HTTP_Controller.GetCountryByID(countryID);
        }
    }
}
