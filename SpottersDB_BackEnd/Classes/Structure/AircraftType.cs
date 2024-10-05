namespace SpottersDB_BackEnd.Classes.Structure
{
    public class AircraftType
    {
        private int _ID;
        private string _ICAOCode;
        private string _FullName;
        private string _NickName;
        private int _ManufactorerID;

        public int ID
        {
            get
            {
                return _ID;
            }
        }

        public string ICAOCode
        {
            get
            {
                return _ICAOCode;
            }
            set
            {
                _ICAOCode = value;
            }
        }

        public string FullName
        {
            get
            {
                return _FullName;
            }
            set
            {
                _FullName = value;
            }
        }

        public string NickName
        {
            get
            {
                return _NickName;
            }
            set
            {
                _NickName = value;
            }
        }

        public int ManufactorerID
        {
            get
            {
                return _ManufactorerID;
            }
            set
            {
                _ManufactorerID = value;
            }
        }

        public AircraftType()
        {

        }

        public AircraftType(string ICAOCode, string FullName, string NickName, int ManufactorerID)
        {
            this.ICAOCode = ICAOCode;
            this.FullName = FullName;
            this.ManufactorerID = ManufactorerID;
            this.NickName = NickName;
        }
    }
}
