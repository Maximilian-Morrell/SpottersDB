using Microsoft.Maui.Controls.Shapes;
using SpottersDB_FrontEnd.Classes.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottersDB_FrontEnd.Classes.UI_Elements.Cards
{
    internal class ImageItem
    {

        public static Border GetImageCardItem(string URL, string Info = "TBD")
        {
            Border b = new Border();
            RoundRectangle rr = new RoundRectangle();
            rr.CornerRadius = 10;
            b.StrokeShape = rr;
            b.Padding = 0;
            b.Margin = 0;

            Image image = new Image();
            try
            {
                image.Source = new UriImageSource
                {
                    Uri = new Uri(URL)
                };
            }
            catch (Exception e)
            {

            }

            image.Aspect = Aspect.Fill;

            BoxView boxView = new BoxView();
            boxView.Color = Colors.Gray.WithAlpha(0.5f);

            Grid grid = new Grid();
            grid.Padding = 0;
            grid.RowSpacing = 0;
            grid.Margin = 0;
            grid.Children.Add(image);
            grid.Children.Add(boxView);

            b.Content = grid;
            b.ZIndex = -1;
            return b;
        }
    }
}
