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

        public async Task<Frame> Card(Manufactorer manufactorer)
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
            lblName.Text = manufactorer.name;
            lblName.FontSize = 48;
            lblName.FontAttributes = FontAttributes.Bold;
            lblName.HorizontalTextAlignment = TextAlignment.Center;
            parent.Children.Add(lblName);

            Label lblRegion = new Label();
            Country c = await manufactorer.GetRegion();
            lblRegion.Text = c.name;
            lblRegion.HorizontalTextAlignment = TextAlignment.Center;
            parent.Children.Add(lblRegion);

            Button editBtn = new Button();
            editBtn.Text = "Edit";
            editBtn.CommandParameter = manufactorer;
            editBtn.Clicked += EditBtn_Clicked;
            editBtn.HorizontalOptions = LayoutOptions.Center;
            parent.Children.Add(editBtn);

            return f;
        }

        private void EditBtn_Clicked(object sender, EventArgs e)
        {
            Button b = sender as Button;
            EditClickHandler handler = EditClicked;
            handler(b.CommandParameter as Manufactorer);
        }
    }
}
