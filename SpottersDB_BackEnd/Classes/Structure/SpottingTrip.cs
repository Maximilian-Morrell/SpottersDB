namespace SpottersDB_BackEnd.Classes.Structure
{
    public class SpottingTrip
    {
        private int _ID;
        private DateTime _Start;
        private DateTime _End;
        private string _Name;
        private string _Description;

        public int ID
        {
            get
            {
                return _ID;
            }
        }

        public DateTime Start
        {
            get
            {
                return _Start;
            }
            set
            {
                _Start = value;
            }
        }

        public DateTime End
        {
            get
            {
                return _End;
            }
            set
            {
                _End = value;
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

        // Empty Constructor to avoid problems with compiler
        public SpottingTrip()
        {

        }

        public SpottingTrip(DateTime Start, DateTime End, string Name, string Description = "")
        {
            this.Start = Start;
            this.End = End;
            this.Name = Name;
            this.Description = Description;
        }
    }
}
