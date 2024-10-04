﻿namespace SpottersDB_BackEnd.Classes.Structure
{
    public class Country
    {
        private int _ID;
        private string _ICAO_Code;
        private string _Name;

        public int ID
        {
            get
            {
                return _ID;
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
                _ICAO_Code = value;
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

        public Country() 
        {

        }

        public Country(string ICAO_Code, string Name)
        {
            this.ICAO_Code = ICAO_Code;
            this.Name = Name;
        }
    }
}