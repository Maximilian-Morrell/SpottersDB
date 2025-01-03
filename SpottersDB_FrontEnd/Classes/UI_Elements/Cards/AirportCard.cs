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
    internal class AirportCard
    {
        public delegate EventHandler EditClickHandler(Airport airport);
        public event EditClickHandler EditClicked;
        public delegate EventHandler DeleteClickedHandler(Airport airport);
        public event DeleteClickedHandler DeleteClicked;

        public async Task<Border> Card(Airport airport)
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
                    new RowDefinition(),
                    new RowDefinition()
                }
            };

            b.Content = parent;
            parent.MaximumWidthRequest = 500;
            parent.WidthRequest = 500;
            parent.MaximumHeightRequest = 250;
            parent.HeightRequest = 250;
            parent.Margin = 10;

            Label lblName = new Label();
            lblName.Text = airport.name;
            lblName.FontSize = 58;
            lblName.LineBreakMode = LineBreakMode.WordWrap;
            lblName.FontAttributes = FontAttributes.Bold;
            lblName.HorizontalTextAlignment = TextAlignment.Center;
            lblName.VerticalTextAlignment = TextAlignment.Center;
            lblName.VerticalOptions = LayoutOptions.Center;
            parent.Add(lblName, 0, 0);

            Label lblIATA_ICAO = new Label();
            lblIATA_ICAO.Text = airport.icaO_Code + " - " + airport.iatA_Code;
            lblIATA_ICAO.HorizontalTextAlignment = TextAlignment.Center;
            lblIATA_ICAO.FontSize = 30;
            lblIATA_ICAO.VerticalOptions = LayoutOptions.Center;
            lblIATA_ICAO.VerticalTextAlignment = TextAlignment.Center;
            parent.Add(lblIATA_ICAO, 0, 1); 

            Label lblCity = new Label();
            lblCity.Text = airport.city;
            lblCity.HorizontalTextAlignment = TextAlignment.Center;
            lblCity.FontSize = 30;
            lblCity.VerticalOptions = LayoutOptions.Center;
            lblCity.VerticalTextAlignment = TextAlignment.Center;
            parent.Add(lblCity, 0, 2);

            Label lblRegion = new Label();
            Country c = await airport.GetRegion();
            lblRegion.Text = c.name;
            lblRegion.HorizontalTextAlignment = TextAlignment.Center;
            lblRegion.FontSize = 30;
            lblRegion.VerticalOptions = LayoutOptions.Center;
            lblRegion.VerticalTextAlignment = TextAlignment.Center;
            parent.Add(lblRegion, 0, 3);

            Button editBtn = new Button();
            editBtn.Text = "Edit";
            editBtn.CommandParameter = airport;
            editBtn.Clicked += EditBtn_Clicked;
            editBtn.HorizontalOptions = LayoutOptions.Fill;
            editBtn.VerticalOptions = LayoutOptions.End;
            parent.Add(editBtn, 0, 4);

            Button deleteBtn = new Button();
            deleteBtn.Text = "Delete";
            deleteBtn.CommandParameter = airport;
            deleteBtn.Clicked += DeleteBtn_Clicked; ;
            deleteBtn.VerticalOptions = LayoutOptions.End;
            parent.Add(deleteBtn, 0, 5);

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
