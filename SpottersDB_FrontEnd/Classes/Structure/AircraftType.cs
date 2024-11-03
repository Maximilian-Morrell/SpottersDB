using Microsoft.Maui.Controls;
using SpottersDB_FrontEnd.Classes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.Structure
{
    public class AircraftType
    {
        private int _ID;
        private string _ICAOCode;
        private string _FullName;
        private string _Nickname;
        private int _ManufactorerID;

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

        public string icaoCode
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

        public string fullName
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

        public string nickName
        {
            get
            {
                return _Nickname;
            }
            set
            {
                _Nickname = value;
            }
        }

        public int manufactorerID
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
            this.icaoCode = ICAOCode;
            this.fullName = FullName;
            this.nickName = NickName;
            this.manufactorerID = ManufactorerID;
        }

        public AircraftType(int ID, string ICAOCode, string FullName, string NickName, int ManufactorerID)
        {
            this.id = ID;
            this.icaoCode = ICAOCode;
            this.fullName = FullName;
            this.nickName = NickName;
            this.manufactorerID = ManufactorerID;
        }

        public async Task<Manufactorer> GetManufactorer()
        {
            return await HTTP_Controller.GetManufactorerByID(manufactorerID);
        }
    }
}
