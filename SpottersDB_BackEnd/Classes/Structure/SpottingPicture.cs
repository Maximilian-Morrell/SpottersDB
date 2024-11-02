namespace SpottersDB_BackEnd.Classes.Structure
{
    public class SpottingPicture
    {
        private int _ID;
        private string _Name;
        private string _Description;
        private string _PictureUrl;
        private string _OriginalFileName;
        private int _SpottingTripID;
        private int _AircraftID;

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

        public string PictureUrl
        {
            get
            {
                return _PictureUrl;
            }
            set
            {
                _PictureUrl = value;
            }
        }

        public string OriginalFileName
        {
            get
            {
                return _OriginalFileName;
            }
            set
            {
                _OriginalFileName = value;
            }
        }

        public int SpottingTripID
        {
            get
            {
                return _SpottingTripID;
            }
            set
            {
                _SpottingTripID = value;
            }
        }

        public int AircraftID
        {
            get
            {
                return _AircraftID;
            }
            set
            {
                _AircraftID = value;
            }
        }

        public SpottingPicture()
        {

        }

        public SpottingPicture(string Name, string Description, string PictureURL, string OriginalFileName, int SpottingTripID, int AircraftID)
        {
            this.Name = Name;
            this.Description = Description;
            this.PictureUrl = PictureURL;
            this.OriginalFileName = OriginalFileName;
            this.SpottingTripID = SpottingTripID;
            this.AircraftID = AircraftID;
        }

        public SpottingPicture(int ID, string Name, string Description, string PictureURL, string OriginalFileName, int SpottingTripID, int AircraftID)
        {
            this.ID = ID;
            this.Name = Name;
            this.Description = Description;
            this.PictureUrl = PictureURL;
            this.OriginalFileName = OriginalFileName;
            this.SpottingTripID = SpottingTripID;
            this.AircraftID = AircraftID;
        }
    }
}
