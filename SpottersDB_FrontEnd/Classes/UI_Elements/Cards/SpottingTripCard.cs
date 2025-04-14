using Microsoft.Maui.Controls.Shapes;
using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;
using SpottersDB_FrontEnd.Classes.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SpottersDB_FrontEnd.Classes.UI_Elements.Cards
{
    internal class SpottingTripCard
    {
        public delegate EventHandler EditClickHandler(SpottingTrip spottingTrip, List<Airport> SelectedAirport);
        public event EditClickHandler EditClicked;
        public delegate EventHandler DeleteClickedHandler(SpottingTrip spottingtrip);
        public event DeleteClickedHandler DeleteClicked;


        public async Task<Border> Card(SpottingTrip spottingTrip)
        {
            Border b = UI_Utilities.CreateBorder();

            Grid parent = UI_Utilities.CreateGrid(b, 6);

            Label lblName = UI_Utilities.CreateLabel(parent, spottingTrip.name, 0, 0, 40, FontAttributes.Bold);

            Label lblDescription = UI_Utilities.CreateLabel(parent, spottingTrip.description, 0, 1, 15);
            lblDescription.LineBreakMode = LineBreakMode.WordWrap;

            string AirportListTxt = "";
            List<Airport> airports = new List<Airport>();
            try
            {
               airports = await HTTP_Controller.GetAirportsFromSpottingTrip(spottingTrip.id);
            }
            catch (Exception ex)
            {
                Window w = new Window(new ErrorBox(ex.StackTrace, ex.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
            if(airports.Count > 0)
            {
                string AirportNameList = "";
                foreach (Airport airport in airports)
                {
                    AirportNameList = AirportNameList + ", " + airport.icaO_Code;
                }
                AirportListTxt = AirportNameList.Substring(1);
            }
            else
            {
                AirportListTxt = "No Airport assigned to this trip!";
            }
            Label lblAirportList = UI_Utilities.CreateLabel(parent, AirportListTxt, 0, 2, 20);
            lblAirportList.LineBreakMode = LineBreakMode.WordWrap;

            string DateTXT = spottingTrip.start.ToString("dd.MM.yyyy - HH:mm") + " / " + spottingTrip.end.ToString("dd.MM.yyyy - HH:mm");
            Label spottingTripDates = UI_Utilities.CreateLabel(parent, DateTXT, 0, 3, 20);

            Button editBtn = UI_Utilities.CreateButton(false, parent, "Edit", spottingTrip, EditBtn_Clicked, 0, 4);

            Button deleteBtn = UI_Utilities.CreateButton(true, parent, "Delete", spottingTrip, DeleteBtn_Clicked, 0, 5);

            return b;
        }

        private void DeleteBtn_Clicked(object? sender, EventArgs e)
        {
            Button b = sender as Button;
            DeleteClickedHandler handler = DeleteClicked;
            handler(b.CommandParameter as SpottingTrip);
        }

        private async void EditBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button b = sender as Button;
                EditClickHandler handler = EditClicked;
                SpottingTrip spottingTrip = b.CommandParameter as SpottingTrip;
                handler(spottingTrip, await HTTP_Controller.GetAirportsFromSpottingTrip(spottingTrip.id));
            }
            catch (Exception ex)
            {
                Window w = new Window(new ErrorBox(ex.StackTrace, ex.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
        }
    }
}
