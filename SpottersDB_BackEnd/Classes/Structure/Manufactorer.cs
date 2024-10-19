namespace SpottersDB_BackEnd.Classes.Structure
{
    public class Manufactorer
    {
        private int _ID;
        private string _Name;
        private int _Region;

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

        public Manufactorer()
        {

        }

        public Manufactorer(string Name, int Region)
        {
            this.Name = Name;
            this.Region = Region;
        }

        public Manufactorer(int ID, string Name, int Region)
        {
            this.ID = ID;
            this.Name = Name;
            this.Region = Region;
        }
    }
}
