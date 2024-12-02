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

        public async Task<Border> Card(SpottingPicture SpottingPicture)
        {
            Border b = new Border();
            RoundRectangle rr = new RoundRectangle();
            rr.CornerRadius = 10;
            b.StrokeShape = rr;
            b.Padding = 10;
            b.BackgroundColor = Color.FromRgb(128, 128, 128);

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

            b.Content = parent;
            parent.MaximumWidthRequest = 480;
            parent.WidthRequest = 480;
            parent.MaximumHeightRequest = 300;
            parent.HeightRequest = 300;
            parent.Margin = 10;

            Label lblName = new Label();
            lblName.Text = SpottingPicture.name;
            lblName.FontSize = 58;
            lblName.FontAttributes = FontAttributes.Bold;
            lblName.HorizontalTextAlignment = TextAlignment.Center;
            lblName.VerticalTextAlignment = TextAlignment.Center;
            lblName.VerticalOptions = LayoutOptions.Center;
            parent.Add(lblName, 0, 0);

            Label lblDescription = new Label();
            lblDescription.Text = SpottingPicture.description;
            lblDescription.LineBreakMode = LineBreakMode.WordWrap;
            lblDescription.HorizontalTextAlignment = TextAlignment.Center;
            lblDescription.FontSize = 20;
            lblDescription.VerticalOptions = LayoutOptions.Center;
            lblDescription.VerticalTextAlignment = TextAlignment.Center;
            parent.Add(lblDescription, 0, 1);

            Border imgB = ImageItem.GetImageCardItem(SpottingPicture.pictureUrl);
            imgB.Scale = 1.2;
            parent.SetRowSpan(imgB, 5);
            parent.Children.Add(imgB);

            List<int> SpottingTripAirport = await HTTP_Controller.GetSpottingTripAirport(SpottingPicture.spottingTripAirportID);
            SpottingTrip spottingTrip = await HTTP_Controller.GetSpottingTrip(SpottingTripAirport[0]);
            Airport airport = await HTTP_Controller.GetAirport(SpottingTripAirport[1]);
            
            Label lblAirportList = new Label();
            lblAirportList.Text = spottingTrip.name + " - " + airport.icaO_Code;
            lblAirportList.HorizontalTextAlignment = TextAlignment.Center;
            lblAirportList.LineBreakMode = LineBreakMode.WordWrap;
            lblAirportList.FontSize = 15;
            lblAirportList.VerticalOptions = LayoutOptions.Center;
            lblAirportList.VerticalTextAlignment = TextAlignment.Center;
            parent.Add(lblAirportList, 0, 2);

            Aircraft aircraft = await HTTP_Controller.GetAircraft(SpottingPicture.aircraftID);
            Label spottingTripDates = new Label();
            spottingTripDates.Text = aircraft.registration;
            spottingTripDates.HorizontalTextAlignment = TextAlignment.Center;
            spottingTripDates.FontSize = 20;
            spottingTripDates.VerticalOptions = LayoutOptions.Center;
            spottingTripDates.VerticalTextAlignment = TextAlignment.Center;
            parent.Add(spottingTripDates, 0, 3);

            Button editBtn = new Button();
            editBtn.Text = "Edit";
            editBtn.CommandParameter = SpottingPicture;
            editBtn.Clicked += EditBtn_Clicked;
            editBtn.HorizontalOptions = LayoutOptions.Fill;
            editBtn.VerticalOptions = LayoutOptions.End;
            parent.Add(editBtn, 0, 4);

            return b;
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
