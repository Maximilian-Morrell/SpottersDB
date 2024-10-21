using SpottersDB_FrontEnd.Classes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.Structure
{
    public class Manufactorer
    {
        private int _ID;
        private string _Name;
        private int _Region;

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

        public int region
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
            this.name = Name;
            this.region = Region;
        }

        public Manufactorer(int ID, string Name, int Region)
        {
            this.id = ID;
            this.name = Name;
            this.region = Region;
        }

        public async Task<Country> GetRegion()
        {
            return await HTTP_Controller.GetCountryByID(region);
        }
    }
}
