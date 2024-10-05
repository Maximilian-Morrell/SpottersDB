namespace SpottersDB_BackEnd.Classes.Structure
{
    public class Airline
    {
        private int _ID;
        private string _ICAO;
        private string _IATA;
        private string _Name;
        private int _Region;

        public int ID
        {
            get
            {
                return _ID;
            }
        }
        
        public string ICAO
        {
            get
            {
                return _ICAO;
            }
            set
            {
                _ICAO = value;
            }
        }

        public string IATA
        {
            get
            {
                return _IATA;
            }
            set
            {
                _IATA = value;
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

        public int Region
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
            this.ICAO = ICAO;
            this.IATA = IATA;
            this.Name = Name;
            this.Region = Region;
        }
    }
}
