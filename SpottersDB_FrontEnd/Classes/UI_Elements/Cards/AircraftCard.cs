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
            Border b = UI_Utilities.CreateBorder();


            Grid parent = UI_Utilities.CreateGrid(b, 6);

            Label lblName = UI_Utilities.CreateLabel(parent, aircraft.registration, 0, 0, 58, FontAttributes.Bold);


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

            Button deleteBtn = new Button();
            deleteBtn.Text = "Delete";
            deleteBtn.CommandParameter = aircraft;
            deleteBtn.Clicked += DeleteBtn_Clicked; ;
            deleteBtn.VerticalOptions = LayoutOptions.End;
            deleteBtn.TextColor = Microsoft.Maui.Graphics.Colors.White;
            deleteBtn.BackgroundColor = Microsoft.Maui.Graphics.Color.FromRgb(209, 36, 42);
            parent.Add(deleteBtn, 0, 5);

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
