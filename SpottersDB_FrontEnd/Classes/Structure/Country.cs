using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.Structure
{
    public class Country
    {
        private int _CountryID;
        private string _CountryICAOCode;
        private string _CountryName;

        public int id
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

        public string icaO_Code
        {
            get
            {
                return _CountryICAOCode;
            }
            set
            {
                _CountryICAOCode = value;
            }
        }

        public string name
        {
            get
            {
                return _CountryName;
            }
            set
            {
                _CountryName = value;
            }
        }

        public Country()
        {

        }

        public Country(int ID, string ICAO, string Name)
        {
            id = ID;
            icaO_Code = ICAO;
            name = Name;
        }
    }
}
