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
    internal class AirlineCard
    {
        public delegate EventHandler EditClickHandler(Airline airline);
        public event EditClickHandler EditClicked;
        public delegate EventHandler DeleteClickedHandler(Airline airline);
        public event DeleteClickedHandler DeleteClicked;

        public async Task<Border> Card(Airline airline)
        {
            Border b = UI_Utilities.CreateBorder();

            Grid parent = UI_Utilities.CreateGrid(b, 5);

            Label lblName = UI_Utilities.CreateLabel(parent, airline.iata, 0, 0, 50, FontAttributes.Bold);

            Label lblIATA_ICAO = UI_Utilities.CreateLabel(parent, airline.name, 0, 1, 20);

            Country c = await airline.GetRegion();
            Label lblRegion = UI_Utilities.CreateLabel(parent, c.name, 0, 2, 30);

            Button editBtn = UI_Utilities.CreateButton(false, parent, "Edit", airline, EditBtn_Clicked, 0, 3);

            Button deleteBtn = UI_Utilities.CreateButton(true, parent, "Delete", airline, DeleteBtn_Clicked, 0, 4);

            return b;
        }

        private void DeleteBtn_Clicked(object? sender, EventArgs e)
        {
            Button b = sender as Button;
            DeleteClickedHandler handler = DeleteClicked;
            handler(b.CommandParameter as Airline);
        }

        private void EditBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button b = sender as Button;
                EditClickHandler handler = EditClicked;
                handler(b.CommandParameter as Airline);
            }
            catch (Exception ex)
            {
                Window w = new Window(new ErrorBox(ex.StackTrace, ex.InnerException.Message));
                Application.Current.OpenWindow(w);
                
            }
        }
    }
}
