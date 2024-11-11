using SpottersDB_FrontEnd.Classes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.Structure
{
    public class Airline
    {
        private int _ID;
        private string _ICAO;
        private string _IATA;
        private string _Name;
        private int _Region;

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

        public string icao
        {
            get
            {
                return _ICAO ?? "";
            }
            set
            {
                _ICAO = value;
            }
        }

        public string iata
        {
            get
            {
                return _IATA ?? "";
            }
            set
            {
                _IATA = value;
            }
        }

        public string name
        {
            get
            {
                return _Name ?? "";
            }
            set
            {
                _Name = value;
            }
        }

        public int region
        {
            get
            {
                return _Region;
            }
            set
            {
                _Region = value;
            }
        }

        public Airline()
        {

        }

        public Airline(string ICAO, string IATA, string Name, int Region)
        {
            this.icao = ICAO;
            this.iata = IATA;
            this.name = Name;
            this.region = Region;
        }

        public Airline(int ID, string ICAO, string IATA, string Name, int Region)
        {
            this.id = ID;
            this.icao = ICAO;
            this.iata = IATA;
            this.name = Name;
            this.region = Region;
        }

        public async Task<Country> GetRegion()
        {
            return await HTTP_Controller.GetCountryByID(region);
        }
    }
}
