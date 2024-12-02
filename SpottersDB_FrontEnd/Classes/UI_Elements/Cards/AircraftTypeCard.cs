using Microsoft.Maui.Controls.Shapes;
using SpottersDB_FrontEnd.Classes.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.UI_Elements.Cards
{
    internal class AircraftTypeCard
    {
        public delegate EventHandler EditClickHandler(AircraftType aircraftType);
        public event EditClickHandler EditClicked;

        public async Task<Border> Card(AircraftType aircraftType)
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
            parent.MaximumHeightRequest = 300;
            parent.HeightRequest = 300;
            parent.Margin = 10;

            Label lblTypeCode = new Label();
            lblTypeCode.Text = aircraftType.icaoCode;
            lblTypeCode.FontSize = 58;
            lblTypeCode.FontAttributes = FontAttributes.Bold;
            lblTypeCode.HorizontalTextAlignment = TextAlignment.Center;
            lblTypeCode.VerticalTextAlignment = TextAlignment.Center;
            lblTypeCode.VerticalOptions = LayoutOptions.Center;
            parent.Add(lblTypeCode, 0, 0);

            Label lblName = new Label();
            lblName.Text = aircraftType.fullName;
            lblName.FontSize = 30;
            lblName.HorizontalTextAlignment = TextAlignment.Center;
            lblName.VerticalTextAlignment = TextAlignment.Center;
            lblName.VerticalOptions = LayoutOptions.Center;
            parent.Add(lblName, 0, 1); 

            if(aircraftType.nickName != "")
            {
                Label lblNickName = new Label();
                lblNickName.Text = "\"" + aircraftType.nickName + "\"";
                lblNickName.FontSize = 20;
                lblNickName.HorizontalTextAlignment = TextAlignment.Center;
                lblNickName.VerticalTextAlignment = TextAlignment.Center;
                lblNickName.VerticalOptions = LayoutOptions.Center;
                parent.Add(lblNickName, 0, 2);
            }
            else
            {
                Grid.SetRowSpan(lblName, 2);
            }

            Label lblManufactorer = new Label();
            Manufactorer m = await aircraftType.GetManufactorer();
            lblManufactorer.Text = m.name;
            lblManufactorer.HorizontalTextAlignment = TextAlignment.Center;
            lblManufactorer.FontSize = 30;
            lblManufactorer.VerticalOptions = LayoutOptions.Center;
            lblManufactorer.VerticalTextAlignment = TextAlignment.Center;
            parent.Add(lblManufactorer, 0, 3);

            Button editBtn = new Button();
            editBtn.Text = "Edit";
            editBtn.CommandParameter = aircraftType;
            editBtn.Clicked += EditBtn_Clicked;
            editBtn.HorizontalOptions = LayoutOptions.Fill;
            editBtn.VerticalOptions = LayoutOptions.End;
            parent.Add(editBtn, 0, 4);

            return b;
        }

        private void EditBtn_Clicked(object sender, EventArgs e)
        {
            Button b = sender as Button;
            EditClickHandler handler = EditClicked;
            handler(b.CommandParameter as AircraftType);
        }
    }
}
