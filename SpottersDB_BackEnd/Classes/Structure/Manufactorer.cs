namespace SpottersDB_BackEnd.Classes.Structure
{
    public class Manufactorer
    {
        private int _ID;
        private string _Name;
        private string _Location;

        public int ID
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

        public string Location
        {
            get
            {
                return _Location;
            }
            set
            {
                _Location = value;
            }
        }

        public Manufactorer()
        {

        }

        public Manufactorer(string Name, string Location)
        {
            this.Name = Name;
            this.Location = Location;
        }
    }
}
