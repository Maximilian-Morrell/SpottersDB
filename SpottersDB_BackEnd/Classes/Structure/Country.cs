namespace SpottersDB_BackEnd.Classes.Structure
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

        public Country(int ID, string ICAO_Code, string Name)
        {
            this.ID = ID;
            this.Name= Name;
            this.ICAO_Code= ICAO_Code;
        }
    }
}
