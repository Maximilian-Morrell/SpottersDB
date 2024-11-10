using SpottersDB_FrontEnd.Classes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.Structure
{
    public class SpottingPicture
    {
        private int _ID;
        private string _Name;
        private string _Description;
        private string _PictureUrl;
        private string _OriginalFileName;
        private int _SpottingTripAirportID;
        private int _AircraftID;

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

        public string name
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

        public string description
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

        public string pictureUrl
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

        public string orginalFileName
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

        public int spottingTripAirportID
        {
            get
            {
                return _SpottingTripAirportID;
            }
            set
            {
                _SpottingTripAirportID = value;
            }
        }

        public int aircraftID
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

        public SpottingPicture(string Name, string Description, int SpottingTripAirportID, int AircraftID)
        {
            this.name = Name;
            this.description = Description;
            this.spottingTripAirportID = SpottingTripAirportID;
            this.aircraftID = AircraftID;
        }

        public SpottingPicture(int ID, string Name, string Description, int SpottingTripAirportID, int AircraftID)
        {
            this.id = ID;
            this.name = Name;
            this.description = Description;
            this.spottingTripAirportID = SpottingTripAirportID;
            this.aircraftID = AircraftID;
        }

        public async Task<List<int>> GetSpottingTripAirport()
        {
            return await HTTP_Controller.GetSpottingTripAirport(spottingTripAirportID);
        }
    }
}
