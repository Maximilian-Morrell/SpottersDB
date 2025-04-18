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
    internal class SpottingPictureCard
    {
        public delegate EventHandler EditClickHandler(SpottingPicture spottingPicture);
        public event EditClickHandler EditClicked;
        public delegate EventHandler DeleteClickedHandler(SpottingPicture spottingPicture);
        public event DeleteClickedHandler DeleteClicked;

        public async Task<Border> Card(SpottingPicture SpottingPicture)
        {
            Border CardBorder = UI_Utilities.CreateBorder(Padding: 0);
            
            Border b = UI_Utilities.CreateAbsoluteBorder(640, 426.5);

            Image imgB = UI_Utilities.CreateImage(SpottingPicture.pictureUrl, 0.5, 640, 426.5);

            AbsoluteLayout GrandParent = UI_Utilities.CreateAbsoluteLayout(640, 426.5);
            GrandParent.Children.Add(imgB);
            GrandParent.Children.Add(b);
            CardBorder.Content = GrandParent;

            Grid parent = UI_Utilities.CreateGrid(b, 6, 620, 407);

            Label lblName = UI_Utilities.CreateLabel(parent, SpottingPicture.name, 0, 0, 50, FontAttributes.Bold);

            Label lblDescription = UI_Utilities.CreateLabel(parent, SpottingPicture.description, 0, 1, 20);
            lblDescription.LineBreakMode = LineBreakMode.WordWrap;

            Dictionary<string, int> SpottingTripAirport = await HTTP_Controller.GetSpottingTripAirport(SpottingPicture.spottingTripAirportID);
            SpottingTrip spottingTrip = await HTTP_Controller.GetSpottingTrip(SpottingTripAirport["SpottingTrip"]);
            Airport airport = await HTTP_Controller.GetAirport(SpottingTripAirport["Airport"]);

            Label lblAirportList = UI_Utilities.CreateLabel(parent, spottingTrip.name + " - " + airport.icaO_Code, 0, 2, 20);
            lblAirportList.LineBreakMode = LineBreakMode.WordWrap;

            Aircraft aircraft = await HTTP_Controller.GetAircraft(SpottingPicture.aircraftID);
            Label spottingTripDates = UI_Utilities.CreateLabel(parent, aircraft.registration, 0, 3, 20);

            Button editBtn = UI_Utilities.CreateButton(false, parent, "Edit", SpottingPicture, EditBtn_Clicked, 0, 4);

            Button deleteBtn = UI_Utilities.CreateButton(true, parent, "Delete", SpottingPicture, DeleteBtn_Clicked, 0, 5);

            return CardBorder;
        }

        public async Task<Border> CardHome(SpottingPicture SpottingPicture)
        {
            Border CardBorder = UI_Utilities.CreateBorder(Padding: 0);

            Border b = UI_Utilities.CreateAbsoluteBorder(640, 426.5);

            Image imgB = UI_Utilities.CreateImage(SpottingPicture.pictureUrl, 0.5, 640, 426.5);

            AbsoluteLayout GrandParent = UI_Utilities.CreateAbsoluteLayout(640, 426.5);
            GrandParent.Children.Add(imgB);
            GrandParent.Children.Add(b);
            CardBorder.Content = GrandParent;

            Grid parent = UI_Utilities.CreateGrid(b, 5, 620, 407);

            Label lblName = UI_Utilities.CreateLabel(parent, SpottingPicture.name, 0, 0, 50, FontAttributes.Bold);

            Label lblDescription = UI_Utilities.CreateLabel(parent, SpottingPicture.description, 0, 1, 20);
            lblDescription.LineBreakMode = LineBreakMode.WordWrap;

            Dictionary<string, int> SpottingTripAirport = await HTTP_Controller.GetSpottingTripAirport(SpottingPicture.spottingTripAirportID);
            SpottingTrip spottingTrip = await HTTP_Controller.GetSpottingTrip(SpottingTripAirport["SpottingTrip"]);
            Airport airport = await HTTP_Controller.GetAirport(SpottingTripAirport["Airport"]);

            Label lblAirportList = UI_Utilities.CreateLabel(parent, spottingTrip.name + " - " + airport.icaO_Code, 0, 2, 20);
            lblAirportList.LineBreakMode = LineBreakMode.WordWrap;

            Aircraft aircraft = await HTTP_Controller.GetAircraft(SpottingPicture.aircraftID);
            Label spottingTripDates = UI_Utilities.CreateLabel(parent, aircraft.registration, 0, 3, 20);

            Button editBtn = UI_Utilities.CreateButton(false, parent, "Open", SpottingPicture, EditBtn_Clicked, 0, 4);

            return CardBorder;
        }

        private void DeleteBtn_Clicked(object? sender, EventArgs e)
        {
            Button b = sender as Button;
            DeleteClickedHandler handler = DeleteClicked;
            handler(b.CommandParameter as SpottingPicture);
        }

        private async void EditBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button b = sender as Button;
                EditClickHandler handler = EditClicked;
                SpottingPicture spottingPicture = b.CommandParameter as SpottingPicture;
                handler(spottingPicture);
            }
            catch (Exception ex)
            {
                Window w = new Window(new ErrorBox(ex.StackTrace, ex.InnerException.Message));
                Application.Current.OpenWindow(w);
            }
        }
    }
}
