using Microsoft.Maui.Controls.Shapes;
using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;
using SpottersDB_FrontEnd.Classes.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.UI_Elements.Cards
{
    internal class AircraftCard
    {
        public delegate EventHandler EditClickHandler(Aircraft aircraft);
        public event EditClickHandler EditClicked;
        public delegate EventHandler DeleteClickedHandler(Aircraft aircraft);
        public event DeleteClickedHandler DeleteClicked;

        public async Task<Border> Card(Aircraft aircraft)
        {
            // Create the Border which adds the roundness to the edges and acts as the final content container
            Border b = UI_Utilities.CreateBorder();

            // Creates the Parent Grid where all of the different UI Elements are aligned in
            Grid parent = UI_Utilities.CreateGrid(b, 6);

            // Name Label
            Label lblName = UI_Utilities.CreateLabel(parent, aircraft.registration, 0, 0, 50, FontAttributes.Bold);

            // Aircraft Type Label
            AircraftType type = await aircraft.GetAircraftType();
            Label lblType = UI_Utilities.CreateLabel(parent, type.icaoCode, 0, 1, 30);

            // Airline Label
            Airline airline = await aircraft.GetAirline();
            Label lblAirline = UI_Utilities.CreateLabel(parent, airline.iata, 0, 2, 30);

            // Country Label
            Country c = await aircraft.GetCountry();
            Label lblRegion = UI_Utilities.CreateLabel(parent, c.name, 0, 3, 30);

            // Edit Button
            Button editBtn = UI_Utilities.CreateButton(false, parent, "Edit", aircraft, EditBtn_Clicked, 0, 4);

            // Delete Button
            Button deleteBtn = UI_Utilities.CreateButton(true, parent, "Delete", aircraft, DeleteBtn_Clicked, 0, 5);

            return b;
        }

        public async Task<Border> CardHome(Aircraft aircraft)
        {
            // Create the Border which adds the roundness to the edges and acts as the final content container
            Border b = UI_Utilities.CreateBorder(Margin: 10);

            // Creates the Parent Grid where all of the different UI Elements are aligned in
            Grid parent = UI_Utilities.CreateGrid(b, 6);

            // Name Label
            Label lblName = UI_Utilities.CreateLabel(parent, aircraft.registration, 0, 0, 50, FontAttributes.Bold);

            // Aircraft Type Label
            AircraftType type = await aircraft.GetAircraftType();
            Label lblType = UI_Utilities.CreateLabel(parent, type.icaoCode, 0, 1, 30);

            // Airline Label
            Airline airline = await aircraft.GetAirline();
            Label lblAirline = UI_Utilities.CreateLabel(parent, airline.iata, 0, 2, 30);

            // Country Label
            Country c = await aircraft.GetCountry();
            Label lblRegion = UI_Utilities.CreateLabel(parent, c.name, 0, 3, 30);

            // Edit Button
            Button Open = UI_Utilities.CreateButton(false, parent, "Open", aircraft, EditBtn_Clicked, 0, 4);

            return b;
        }

        private void DeleteBtn_Clicked(object? sender, EventArgs e)
        {
            Button b = sender as Button;
            DeleteClickedHandler handler = DeleteClicked;
            handler(b.CommandParameter as Aircraft);
        }

        private void EditBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button b = sender as Button;
                EditClickHandler handler = EditClicked;
                handler(b.CommandParameter as Aircraft);
            }
            catch (Exception ex)
            {
                Window w = new Window(new ErrorBox(ex.StackTrace, ex.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
        }
    }
}
