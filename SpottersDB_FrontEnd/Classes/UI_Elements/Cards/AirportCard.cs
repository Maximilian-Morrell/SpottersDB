using Microsoft.Maui.Controls.Shapes;
using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;
using SpottersDB_FrontEnd.Classes.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.UI_Elements.Cards
{
    internal class AirportCard
    {
        public delegate EventHandler EditClickHandler(Airport airport);
        public event EditClickHandler EditClicked;
        public delegate EventHandler DeleteClickedHandler(Airport airport);
        public event DeleteClickedHandler DeleteClicked;

        public async Task<Border> Card(Airport airport)
        {
            Border b = UI_Utilities.CreateBorder();

            Grid parent = UI_Utilities.CreateGrid(b, 6, MaximumHeight: 400);

            Label lblIATA_ICAO = UI_Utilities.CreateLabel(parent, airport.icaO_Code + " - " + airport.iatA_Code, 0, 0, 50, FontAttributes.Bold);

            Label lblAName = UI_Utilities.CreateLabel(parent, airport.name, 0, 1, 18);
            lblIATA_ICAO.LineBreakMode = LineBreakMode.WordWrap;

            Label lblCity = UI_Utilities.CreateLabel(parent, airport.city, 0, 2, 30);

            Country c = await airport.GetRegion();
            Label lblRegion = UI_Utilities.CreateLabel(parent, c.name, 0, 3, 30);

            Button editBtn = UI_Utilities.CreateButton(false, parent, "Edit", airport, EditBtn_Clicked, 0, 4);

            Button deleteBtn = UI_Utilities.CreateButton(true, parent, "Delete", airport, DeleteBtn_Clicked, 0, 5);

            return b;
        }

        private void DeleteBtn_Clicked(object? sender, EventArgs e)
        {
            Button b = sender as Button;
            DeleteClickedHandler handler = DeleteClicked;
            handler(b.CommandParameter as Airport);
        }

        private void EditBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button b = sender as Button;
                EditClickHandler handler = EditClicked;
                handler(b.CommandParameter as Airport);
            }
            catch (Exception ex)
            {
                Window w = new Window(new ErrorBox(ex.StackTrace, ex.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
        }
    }
}
