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

        public static Frame GetImageCardItem(string URL, string Info = "TBD")
        {
            Frame F = new Frame();
            F.CornerRadius = 10;
            F.Padding = 0;
            F.Margin = 0;
            F.IsClippedToBounds = true;
            F.HasShadow = false;

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

            F.Content = grid;
            F.ZIndex = -1;
            return F;
        }
    }
}
