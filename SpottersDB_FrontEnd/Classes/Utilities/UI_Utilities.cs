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
        public static Border CreateAbsoluteBorder(double Width, double Height)
        {
            Border b = new Border();
            AbsoluteLayout.SetLayoutBounds(b, new Rect(0, 0, Width, Height));
            AbsoluteLayout.SetLayoutFlags(b, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.None);
            return b;
        }

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

        public static Grid CreateGrid(Border b, int Rows, int MaximumWidth = 500, int MaximumHeight = 300, int Margin = 5)
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

        public static Label CreateLabel(Grid parent, string Content, int Column, int Row, int fontSize)
        {
            Label lbl = new Label();
            lbl.Text = Content;
            lbl.FontSize = fontSize;
            lbl.HorizontalTextAlignment = TextAlignment.Center;
            lbl.VerticalTextAlignment = TextAlignment.Center;
            lbl.VerticalOptions = LayoutOptions.Fill;
            parent.Add(lbl, Column, Row);
            return lbl;
        }
        public static Label CreateLabel(Grid parent, string Content, int Column, int Row, int fontSize, FontAttributes fontAttributes)
        {
            Label lbl = new Label();
            lbl.Text = Content;
            lbl.FontSize = fontSize;
            lbl.FontAttributes = fontAttributes;
            lbl.HorizontalTextAlignment = TextAlignment.Center;
            lbl.VerticalTextAlignment = TextAlignment.Center;
            lbl.VerticalOptions = LayoutOptions.Fill;
            parent.Add(lbl, Column, Row);
            return lbl;
        }

        public static Button CreateButton(bool IsDelete, Grid parent, string Text, object CommandParameter, EventHandler Clicked, int Column, int Row)
        {
            Button btn = new Button();
            btn.Text = Text;
            btn.CommandParameter = CommandParameter;
            btn.Clicked += Clicked;
            btn.HorizontalOptions = LayoutOptions.Fill;
            btn.VerticalOptions = LayoutOptions.End;
            parent.Add(btn, Column, Row);
            if(IsDelete)
            {
                btn.TextColor = Microsoft.Maui.Graphics.Colors.White;
                btn.BackgroundColor = Microsoft.Maui.Graphics.Color.FromRgb(209, 36, 42);
            }
            return btn;
        }

        public static Image CreateImage(string URL)
        {
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

            return image;
        }

        public static Image CreateImage(string URL, double Opacity, double Width, double Height)
        {
            Image image = new Image();
            image.Opacity = Opacity;
            AbsoluteLayout.SetLayoutBounds(image, new Rect(0, 0, Width, Height));
            AbsoluteLayout.SetLayoutFlags(image, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.None);
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

            return image;
        }

        public static AbsoluteLayout CreateAbsoluteLayout(double Width, double Height)
        {
            AbsoluteLayout AL = new AbsoluteLayout();
            AL.HeightRequest = Height;
            AL.WidthRequest = Width;
            return AL;
        }
    }
}
