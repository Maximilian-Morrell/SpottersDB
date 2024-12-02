using SpottersDB_FrontEnd.Classes.Structure;
using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpottersDB_FrontEnd.Classes.Utilities;
using Microsoft.Maui.Controls.Shapes;

namespace SpottersDB_FrontEnd.Classes.UI_Elements.Cards
{
    internal class CountryCard
    {
        public CountryCard()
        {

        }

        public delegate EventHandler EditClickedHandler(Country country);
        public event EditClickedHandler EditClicked;

        public Border Card(Country country, string URL)
        {
            

            Border b = new Border();
            RoundRectangle rr = new RoundRectangle();
            rr.CornerRadius = 10;
            b.StrokeShape = rr;
            b.Padding = 10;
            b.BackgroundColor = Color.FromRgb(128, 128, 128);
            b.Padding = 0;
            b.Margin = 0;

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

            b.Content = parent;
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
                Border imgB = ImageItem.GetImageCardItem(URL);
                imgB.Scale = 1.2;
                parent.SetRowSpan(imgB, 3);
                parent.Children.Add(imgB);
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

            return b;
        }

        private void EditBtn_Clicked(object sender, EventArgs e)
        {
            Button b = sender as Button;
            EditClickedHandler handler = EditClicked;
            handler(b.CommandParameter as Country);
        }
    }
}
