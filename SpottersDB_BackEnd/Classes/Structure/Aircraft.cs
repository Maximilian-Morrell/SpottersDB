namespace SpottersDB_BackEnd.Classes.Structure
{
    public class Aircraft
    {
        private int _ID;
        private string _Registration;
        private string _Description;
        private int _TypeID;
        private int _CountryID;
        private int _AirlineID;

        public int ID
        {
            get
            {
                return _ID;
            }
        }

        public string Registration
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

        public int TypeID
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

        public int CountryID
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

        public int AirlineID
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

        public Aircraft(string Registration, string Description, int TypeID, int CountryID, int AirlineID)
        {
            this.Registration = Registration;
            this.Description = Description;
            this.TypeID = TypeID;
            this.CountryID = CountryID;
            this.AirlineID = AirlineID;
        }
    }
}
