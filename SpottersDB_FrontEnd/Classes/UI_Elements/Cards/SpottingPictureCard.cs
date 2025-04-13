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
            Border final_Border = UI_Utilities.CreateBorder(Padding: 0);
            Border b = new Border();
            AbsoluteLayout.SetLayoutBounds(b, new Rect(0, 0, 640, 426.5));
            AbsoluteLayout.SetLayoutFlags(b, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.None);

            Image imgB = UI_Utilities.CreateImage(SpottingPicture.pictureUrl);
            imgB.Opacity = 0.5;
            AbsoluteLayout.SetLayoutBounds(imgB, new Rect(0, 0, 640, 426.5));
            AbsoluteLayout.SetLayoutFlags(imgB, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.None);

            AbsoluteLayout GrandParent = new AbsoluteLayout();
            GrandParent.HeightRequest = 426.5;
            GrandParent.WidthRequest = 640;

            GrandParent.Children.Add(imgB);
            GrandParent.Children.Add(b);
            final_Border.Content = GrandParent;

            Grid parent = UI_Utilities.CreateGrid(b, 6, 620, 407);

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


            Dictionary<string, int> SpottingTripAirport = await HTTP_Controller.GetSpottingTripAirport(SpottingPicture.spottingTripAirportID);
            SpottingTrip spottingTrip = await HTTP_Controller.GetSpottingTrip(SpottingTripAirport["SpottingTrip"]);
            Airport airport = await HTTP_Controller.GetAirport(SpottingTripAirport["Airport"]);
            
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

            Button deleteBtn = new Button();
            deleteBtn.Text = "Delete";
            deleteBtn.CommandParameter = SpottingPicture;
            deleteBtn.Clicked += DeleteBtn_Clicked; ;
            deleteBtn.VerticalOptions = LayoutOptions.End;
            deleteBtn.TextColor = Microsoft.Maui.Graphics.Colors.White;
            deleteBtn.BackgroundColor = Microsoft.Maui.Graphics.Color.FromRgb(209, 36, 42);
            parent.Add(deleteBtn, 0, 5);

            return final_Border;
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
