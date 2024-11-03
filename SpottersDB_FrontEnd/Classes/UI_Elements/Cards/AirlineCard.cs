using SpottersDB_FrontEnd.Classes.Structure;
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

        public async Task<Frame> Card(Airline airline)
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
                   // new RowDefinition(), - for the delete Button
                    new RowDefinition()
                }
            };

            f.Content = parent;
            parent.MaximumWidthRequest = 500;
            parent.WidthRequest = 400;
            parent.MaximumHeightRequest = 200;
            parent.HeightRequest = 200;
            parent.Margin = 10;

            Label lblName = new Label();
            lblName.Text = airline.name;
            lblName.FontSize = 58;
            lblName.FontAttributes = FontAttributes.Bold;
            lblName.HorizontalTextAlignment = TextAlignment.Center;
            lblName.VerticalTextAlignment = TextAlignment.Center;
            lblName.VerticalOptions = LayoutOptions.Center;
            parent.Add(lblName, 0, 0);

            Label lblIATA_ICAO = new Label();
            lblIATA_ICAO.Text = airline.icao + " - " + airline.iata;
            lblIATA_ICAO.HorizontalTextAlignment = TextAlignment.Center;
            lblIATA_ICAO.FontSize = 30;
            lblIATA_ICAO.VerticalOptions = LayoutOptions.Center;
            lblIATA_ICAO.VerticalTextAlignment = TextAlignment.Center;
            parent.Add(lblIATA_ICAO, 0, 1);

            Label lblRegion = new Label();
            Country c = await airline.GetRegion();
            lblRegion.Text = c.name;
            lblRegion.HorizontalTextAlignment = TextAlignment.Center;
            lblRegion.FontSize = 30;
            lblRegion.VerticalOptions = LayoutOptions.Center;
            lblRegion.VerticalTextAlignment = TextAlignment.Center;
            parent.Add(lblRegion, 0, 2);

            Button editBtn = new Button();
            editBtn.Text = "Edit";
            editBtn.CommandParameter = airline;
            editBtn.Clicked += EditBtn_Clicked;
            editBtn.HorizontalOptions = LayoutOptions.Fill;
            editBtn.VerticalOptions = LayoutOptions.End;
            parent.Add(editBtn, 0, 3);

            return f;
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
