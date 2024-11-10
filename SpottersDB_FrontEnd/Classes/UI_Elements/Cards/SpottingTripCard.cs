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

        public async Task<Frame> Card(SpottingTrip spottingTrip)
        {
            Frame f = new Frame();
            f.CornerRadius = 10;
            f.Padding = 10;
            f.BackgroundColor = Color.FromRgb(128, 128, 128);
            f.HasShadow = true;

            Grid parent = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                   // new RowDefinition(), - for the delete Button
                    new RowDefinition()
                }
            };

            f.Content = parent;
            parent.MaximumWidthRequest = 500;
            parent.WidthRequest = 500;
            parent.MaximumHeightRequest = 400;
            parent.HeightRequest = 400;
            parent.Margin = 10;

            Label lblName = new Label();
            lblName.Text = spottingTrip.name;
            lblName.FontSize = 58;
            lblName.FontAttributes = FontAttributes.Bold;
            lblName.HorizontalTextAlignment = TextAlignment.Center;
            lblName.VerticalTextAlignment = TextAlignment.Center;
            lblName.VerticalOptions = LayoutOptions.Center;
            parent.Add(lblName, 0, 0);

            Label lblDescription = new Label();
            lblDescription.Text = spottingTrip.description;
            lblDescription.LineBreakMode = LineBreakMode.WordWrap;
            lblDescription.HorizontalTextAlignment = TextAlignment.Center;
            lblDescription.FontSize = 20;
            lblDescription.VerticalOptions = LayoutOptions.Center;
            lblDescription.VerticalTextAlignment = TextAlignment.Center;
            parent.Add(lblDescription, 0, 1);

            Label lblAirportList = new Label();
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
                lblAirportList.Text = AirportNameList.Substring(1);
            }
            else
            {
                lblAirportList.Text = "No Airport assigned to this trip!";
            }
            lblAirportList.HorizontalTextAlignment = TextAlignment.Center;
            lblAirportList.LineBreakMode = LineBreakMode.WordWrap;
            lblAirportList.FontSize = 15;
            lblAirportList.VerticalOptions = LayoutOptions.Center;
            lblAirportList.VerticalTextAlignment = TextAlignment.Center;
            parent.Add(lblAirportList, 0, 2);

            Label spottingTripDates = new Label();
            spottingTripDates.Text = spottingTrip.start.ToString("dd.MM.yyyy - HH:mm") + " / " + spottingTrip.end.ToString("dd.MM.yyyy - HH:mm");
            spottingTripDates.HorizontalTextAlignment = TextAlignment.Center;
            spottingTripDates.FontSize = 20;
            spottingTripDates.VerticalOptions = LayoutOptions.Center;
            spottingTripDates.VerticalTextAlignment = TextAlignment.Center;
            parent.Add(spottingTripDates, 0, 3);

            Button editBtn = new Button();
            editBtn.Text = "Edit";
            editBtn.CommandParameter = spottingTrip;
            editBtn.Clicked += EditBtn_Clicked;
            editBtn.HorizontalOptions = LayoutOptions.Fill;
            editBtn.VerticalOptions = LayoutOptions.End;
            parent.Add(editBtn, 0, 4);

            return f;
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
