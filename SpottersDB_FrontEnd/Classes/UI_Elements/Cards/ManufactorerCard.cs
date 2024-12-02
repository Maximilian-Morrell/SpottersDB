using Microsoft.Maui.Controls.Shapes;
using SpottersDB_FrontEnd.Classes.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.UI_Elements.Cards
{
    class ManufactorerCard
    {
        public delegate EventHandler EditClickHandler(Manufactorer manufactorer);
        public event EditClickHandler EditClicked;

        public async Task<Border> Card(Manufactorer manufactorer)
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
                   // new RowDefinition(), - for the delete Button
                    new RowDefinition()
                }
            };

            b.Content = parent;
            parent.MaximumWidthRequest = 500;
            parent.WidthRequest = 400;
            parent.MaximumHeightRequest = 200;
            parent.HeightRequest = 200;
            parent.Margin = 10;

            Label lblName = new Label();
            lblName.Text = manufactorer.name;
            lblName.FontSize = 58;
            lblName.FontAttributes = FontAttributes.Bold;
            lblName.HorizontalTextAlignment = TextAlignment.Center;
            lblName.VerticalTextAlignment = TextAlignment.Center;
            lblName.VerticalOptions = LayoutOptions.Center;
            parent.Add(lblName, 0,0);

            Label lblRegion = new Label();
            Country c = await manufactorer.GetRegion();
            lblRegion.Text = c.name;
            lblRegion.HorizontalTextAlignment = TextAlignment.Center;
            lblRegion.FontSize = 30;
            lblRegion.VerticalOptions = LayoutOptions.Center;
            lblRegion.VerticalTextAlignment = TextAlignment.Center;
            parent.Add(lblRegion, 0,1);

            Button editBtn = new Button();
            editBtn.Text = "Edit";
            editBtn.CommandParameter = manufactorer;
            editBtn.Clicked += EditBtn_Clicked;
            editBtn.HorizontalOptions = LayoutOptions.Fill;
            editBtn.VerticalOptions = LayoutOptions.End;
            parent.Add(editBtn, 0, 2);

            return b;
        }

        private void EditBtn_Clicked(object sender, EventArgs e)
        {
            Button b = sender as Button;
            EditClickHandler handler = EditClicked;
            handler(b.CommandParameter as Manufactorer);
        }
    }
}
