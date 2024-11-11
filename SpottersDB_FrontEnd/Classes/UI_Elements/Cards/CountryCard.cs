using SpottersDB_FrontEnd.Classes.Structure;
using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.UI_Elements.Cards
{
    internal class CountryCard
    {
        public CountryCard()
        {

        }

        public delegate EventHandler EditClickedHandler(Country country);
        public event EditClickedHandler EditClicked;

        public Frame Card(Country country, string URL)
        {
            

            Frame f = new Frame();
            f.CornerRadius = 10;
            f.Padding = 10;
            f.BackgroundColor = Color.FromRgb(128, 128, 128);
            f.HasShadow = true;
            f.Padding = 0;
            f.Margin = 0;

            Grid parent = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition{Height = new GridLength(2, GridUnitType.Star)},
                    new RowDefinition(),
                   // new RowDefinition(), - for the delete Button
                    new RowDefinition()
                }
            };

            f.Content = parent;
            parent.MaximumWidthRequest = 500;
            parent.WidthRequest = 400;
            parent.MaximumHeightRequest = 250;
            parent.HeightRequest = 250;
            parent.Margin = 10;

            Label lblName = new Label();
            lblName.Text = country.name;
            lblName.FontSize = 40;
            lblName.FontAttributes = FontAttributes.Bold;
            lblName.VerticalOptions = LayoutOptions.Fill;
            lblName.VerticalTextAlignment = TextAlignment.Center;
            lblName.HorizontalTextAlignment = TextAlignment.Center;
            parent.Add(lblName, 0, 0);



            if (country.icaO_Code == "")
            {
                Grid.SetRowSpan(lblName, 3);
                lblName.FontSize = 55;
                parent.MaximumHeightRequest = 200;
                parent.HeightRequest = 200;
            }
            else
            {
                Frame imgF = ImageItem.GetImageCardItem(URL);
                imgF.Scale = 1.2;
                parent.SetRowSpan(imgF, 3);
                parent.Children.Add(imgF);
                Label lblICAO = new Label();
                lblICAO.Text = country.icaO_Code;
                lblICAO.FontSize = 30;
                lblICAO.HorizontalTextAlignment = TextAlignment.Center;
                parent.Add(lblICAO, 0, 1);
            }

            Button editBtn = new Button();
            editBtn.Text = "Edit";
            editBtn.CommandParameter = country;
            editBtn.Clicked += EditBtn_Clicked;
            editBtn.VerticalOptions = LayoutOptions.End;
           // editBtn.HorizontalOptions = LayoutOptions.Center;

            parent.Add(editBtn,0,2);

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
