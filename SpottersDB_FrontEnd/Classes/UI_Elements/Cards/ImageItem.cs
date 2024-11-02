using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.UI_Elements.Cards
{
    internal class ImageItem
    {

        public static Frame GetImageItem(string URL, string Info = "TBD")
        {
            Frame F = new Frame();
            F.WidthRequest = 100;
            F.HeightRequest = 100;
            F.CornerRadius = 10;

            Image image = new Image();
            image.Source = URL;
            image.HeightRequest = 100;
            image.WidthRequest = 100;
            image.Aspect = Aspect.Fill;

            Label lbl = new Label();
            lbl.Text = Info;
            lbl.VerticalOptions = LayoutOptions.End;
            lbl.HorizontalOptions = LayoutOptions.Center;
            lbl.BackgroundColor = Colors.Black.WithAlpha(0.5f);

            Grid grid = new Grid();
            grid.Children.Add(image);
            grid.Children.Add(lbl);

            F.Content = grid;
            return F;
        }
    }
}
