using Microsoft.Maui.Controls.Shapes;
using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.UI_Elements.Cards
{
    internal class AircraftTypeCard
    {
        public delegate EventHandler EditClickHandler(AircraftType aircraftType);
        public event EditClickHandler EditClicked;
        public delegate EventHandler DeleteClickedHandler(AircraftType aircraftType);
        public event DeleteClickedHandler DeleteClicked;

        public async Task<Border> Card(AircraftType aircraftType)
        {
            Border b = UI_Utilities.CreateBorder();

            Grid parent = UI_Utilities.CreateGrid(b, 6);

            Label lblTypeCode = UI_Utilities.CreateLabel(parent, aircraftType.icaoCode, 0, 0, 50, FontAttributes.Bold);

            Label lblName = UI_Utilities.CreateLabel(parent, aircraftType.fullName, 0, 1, 30);

            if(aircraftType.nickName != "")
            {
                Label lblNickName = UI_Utilities.CreateLabel(parent, "\"" + aircraftType.nickName + "\"", 0, 2, 20);
            }
            else
            {
                Grid.SetRowSpan(lblName, 2);
            }

            Manufactorer m = await aircraftType.GetManufactorer();

            Button editBtn = UI_Utilities.CreateButton(false, parent, "Edit", aircraftType, EditBtn_Clicked, 0, 4);

            Button deleteBtn = UI_Utilities.CreateButton(true, parent, "Delete", aircraftType, DeleteBtn_Clicked, 0, 5);

            return b;
        }

        public async Task<Border> CardHome(AircraftType aircraftType)
        {
            Border b = UI_Utilities.CreateBorder(Margin:10);

            Grid parent = UI_Utilities.CreateGrid(b, 5);

            Label lblTypeCode = UI_Utilities.CreateLabel(parent, aircraftType.icaoCode, 0, 0, 50, FontAttributes.Bold);

            Label lblName = UI_Utilities.CreateLabel(parent, aircraftType.fullName, 0, 1, 30);

            if (aircraftType.nickName != "")
            {
                Label lblNickName = UI_Utilities.CreateLabel(parent, "\"" + aircraftType.nickName + "\"", 0, 2, 20);
            }
            else
            {
                Grid.SetRowSpan(lblName, 2);
            }

            Manufactorer m = await aircraftType.GetManufactorer();

            Button editBtn = UI_Utilities.CreateButton(false, parent, "Open", aircraftType, EditBtn_Clicked, 0, 4);

            return b;
        }

        private void DeleteBtn_Clicked(object? sender, EventArgs e)
        {
            Button b = sender as Button;
            DeleteClickedHandler handler = DeleteClicked;
            handler(b.CommandParameter as AircraftType);
        }

        private void EditBtn_Clicked(object sender, EventArgs e)
        {
            Button b = sender as Button;
            EditClickHandler handler = EditClicked;
            handler(b.CommandParameter as AircraftType);
        }
    }
}
