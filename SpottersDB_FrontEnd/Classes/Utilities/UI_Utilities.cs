using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace SpottersDB_FrontEnd.Classes.Utilities
{
    internal class UI_Utilities
    {
        public static Border CreateBorder(int CornerRadius = 10, int Padding = 10, int Margin = 0)
        {
            Border b = new Border();
            RoundRectangle rr = new RoundRectangle();
            rr.CornerRadius = CornerRadius;
            b.StrokeShape = rr;
            b.Padding = Padding;
            b.BackgroundColor = Color.FromRgb(128, 128, 128);
            b.Margin = Margin;
            return b;
        }

        public static Grid CreateGrid(Border b, int Rows, int MaximumWidth = 500, int MaximumHeight = 300, int Margin = 10)
        {
            Grid parent = new Grid();

            for (int i = 0; i < Rows; i++)
            {
                parent.RowDefinitions.Add(new RowDefinition());
            }

            b.Content = parent;
            parent.MaximumWidthRequest = MaximumWidth;
            parent.WidthRequest = MaximumWidth;
            parent.MaximumHeightRequest = MaximumHeight;
            parent.HeightRequest = MaximumHeight;
            parent.Margin = Margin;
            return parent;
        }

        public static Label CreateLabel(Grid parent, string Content, int Column, int Row, int fontSize, FontAttributes fontAttributes)
        {
            Label lbl = new Label();
            lbl.Text = Content;
            lbl.FontSize = fontSize;
            lbl.FontAttributes = fontAttributes;
            lbl.HorizontalTextAlignment = TextAlignment.Center;
            lbl.VerticalTextAlignment = TextAlignment.Center;
            lbl.VerticalOptions = LayoutOptions.Center;
            parent.Add(lbl, Column, Row);
            return lbl;
        }
    }
}
