using SpottersDB_FrontEnd.Classes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.Structure
{
    public class SpottingTrip
    {
        private int _ID;
        private DateTime _Start;
        private DateTime _End;
        private string _Name;
        private string _Description;
        private string _AirportIDs;

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

        public DateTime start
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

        public DateTime end
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

        public string AirportIDs
        {
            get
            {
                return _AirportIDs;
            }
            set
            {
                _AirportIDs = value;
            }
        }

        public SpottingTrip()
        {

        }

        public SpottingTrip(string Name, string Description, DateTime Start, DateTime End, List<Airport> airports)
        {
            this.name = Name;
            this.description = Description;
            this.start = Start;
            this.end = End;
            this.AirportIDs = "";
            foreach(Airport airport in airports)
            {
                this.AirportIDs = this.AirportIDs + "," + airport.id;
            }

            if (this.AirportIDs.Length > 0)
            {
                this.AirportIDs = this.AirportIDs.Substring(1);
            }

        }

        public SpottingTrip(int ID , string Name, string Description, DateTime Start, DateTime End, List<Airport> airports)
        {
            this.id = ID;
            this.name = Name;
            this.description = Description;
            this.start = Start;
            this.end = End;
            this.AirportIDs = "";
            foreach (Airport airport in airports)
            {
                this.AirportIDs = this.AirportIDs + "," + airport.id;
            }
            if (this.AirportIDs.Length > 0)
            {
                this.AirportIDs = this.AirportIDs.Substring(1);
            }
        }

        public async Task<List<Airport>> GetAirports()
        {
            return await HTTP_Controller.GetAirportsFromSpottingTrip(id);
        }
    }
}
