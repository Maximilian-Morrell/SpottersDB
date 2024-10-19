using SpottersDB_FrontEnd.Classes.Structure;
using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.UI_Elements.Cards
{
    internal class CountryCard
    {
        public CountryCard()
        {

        }

        public delegate System.EventHandler EditClickedHandler(Country country);
        public event EditClickedHandler EditClicked;

        public Frame Card(Country country)
        {
            

            Frame f = new Frame();
            f.CornerRadius = 10;
            f.Padding = 10;
            f.BackgroundColor = Color.FromRgb(128, 128, 128);
            f.HasShadow = true;

            VerticalStackLayout parent = new VerticalStackLayout();
            f.Content = parent;
            parent.MinimumWidthRequest = 400;
            parent.Spacing = 10;

            Label lblName = new Label();
            lblName.Text = country.name;
            lblName.FontSize = 48;
            lblName.FontAttributes = FontAttributes.Bold;
            lblName.HorizontalTextAlignment = TextAlignment.Center;
            parent.Children.Add(lblName);

            Label lblICAO = new Label();
            lblICAO.Text = country.icaO_Code;
            lblICAO.HorizontalTextAlignment = TextAlignment.Center;
            parent.Children.Add(lblICAO);

            Button editBtn = new Button();
            editBtn.Text = "Edit";
            editBtn.CommandParameter = country;
            editBtn.Clicked += EditBtn_Clicked;
            editBtn.HorizontalOptions = LayoutOptions.Center;

            parent.Children.Add(editBtn);

            return f;
        }

        private void EditBtn_Clicked(object sender, EventArgs e)
        {
            Button b = sender as Button;
            EditClickedHandler handler = EditClicked;
            handler(b.CommandParameter as Country);
        }
    }
}
