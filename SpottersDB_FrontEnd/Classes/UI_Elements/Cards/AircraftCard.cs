using Microsoft.Maui.Controls.Shapes;
using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.UI_Elements.Cards
{
    internal class AircraftCard
    {
        public delegate EventHandler EditClickHandler(Aircraft aircraft);
        public event EditClickHandler EditClicked;

        public async Task<Border> Card(Aircraft aircraft)
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
            parent.MaximumWidthRequest = 500;
            parent.WidthRequest = 400;
            parent.MaximumHeightRequest = 250;
            parent.HeightRequest = 250;
            parent.Margin = 10;

            Label lblName = new Label();
            lblName.Text = aircraft.registration;
            lblName.FontSize = 58;
            lblName.FontAttributes = FontAttributes.Bold;
            lblName.HorizontalTextAlignment = TextAlignment.Center;
            lblName.VerticalTextAlignment = TextAlignment.Center;
            lblName.VerticalOptions = LayoutOptions.Center;
            parent.Add(lblName, 0, 0);

            Label lblType = new Label();
            AircraftType type = await aircraft.GetAircraftType();
            lblType.Text = type.icaoCode;
            lblType.HorizontalTextAlignment = TextAlignment.Center;
            lblType.FontSize = 30;
            lblType.VerticalOptions = LayoutOptions.Center;
            lblType.VerticalTextAlignment = TextAlignment.Center;
            parent.Add(lblType, 0, 1); 

            Label lblAirline = new Label();
            Airline airline = await aircraft.GetAirline();
            lblAirline.Text = airline.iata;
            lblAirline.HorizontalTextAlignment = TextAlignment.Center;
            lblAirline.FontSize = 30;
            lblAirline.VerticalOptions = LayoutOptions.Center;
            lblAirline.VerticalTextAlignment = TextAlignment.Center;
            parent.Add(lblAirline, 0, 2);

            Label lblRegion = new Label();
            Country c = await aircraft.GetCountry();
            lblRegion.Text = c.name;
            lblRegion.HorizontalTextAlignment = TextAlignment.Center;
            lblRegion.FontSize = 30;
            lblRegion.VerticalOptions = LayoutOptions.Center;
            lblRegion.VerticalTextAlignment = TextAlignment.Center;
            parent.Add(lblRegion, 0, 3);

            Button editBtn = new Button();
            editBtn.Text = "Edit";
            editBtn.CommandParameter = aircraft;
            editBtn.Clicked += EditBtn_Clicked;
            editBtn.HorizontalOptions = LayoutOptions.Fill;
            editBtn.VerticalOptions = LayoutOptions.End;
            parent.Add(editBtn, 0, 4);

            return b;
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
