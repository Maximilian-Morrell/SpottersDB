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
        public delegate EventHandler DeleteClickedHandler(Country country);
        public event DeleteClickedHandler DeleteClicked;

        public Border Card(Country country, string URL)
        {
            Border CardBorder = UI_Utilities.CreateBorder(Padding: 0);

            Border b = UI_Utilities.CreateAbsoluteBorder(640, 426.5);

            Image imgB = UI_Utilities.CreateImage(URL, 0.5, 640, 426.5);

            AbsoluteLayout GrandParent = UI_Utilities.CreateAbsoluteLayout(640, 426.5);
            CardBorder.Content = GrandParent;
            GrandParent.Children.Add(imgB);
            GrandParent.Add(b);

            Grid parent = null;

            if(country.icaO_Code != "")
            {
                parent = UI_Utilities.CreateGrid(b, 4, 620, 407);

                Label lblName = UI_Utilities.CreateLabel(parent, country.icaO_Code, 0, 0, 50, FontAttributes.Bold);

                Label lblCountry = UI_Utilities.CreateLabel(parent, country.name, 0, 1, 20);
                lblCountry.LineBreakMode = LineBreakMode.WordWrap;

                Button editBtn = UI_Utilities.CreateButton(false, parent, "Edit", country, EditBtn_Clicked, 0, 2);

                Button deleteBtn = UI_Utilities.CreateButton(true, parent, "Delete", country, DeleteBtn_Clicked, 0, 3);
            }
            else
            {
                parent = UI_Utilities.CreateGrid(b, 3, 620, 407);
                Label lblName = UI_Utilities.CreateLabel(parent, country.name, 0, 0, 30, FontAttributes.Bold);
                lblName.LineBreakMode = LineBreakMode.WordWrap;

                Button editBtn = UI_Utilities.CreateButton(false, parent, "Edit", country, EditBtn_Clicked, 0, 1);

                Button deleteBtn = UI_Utilities.CreateButton(true, parent, "Delete", country, DeleteBtn_Clicked, 0,2);

            }

            return CardBorder;
        }

        public Border Card(Country country)
        {
            Border b = UI_Utilities.CreateBorder();

            Grid parent = null;

            if (country.icaO_Code != "")
            {
                parent = UI_Utilities.CreateGrid(b, 4, MaximumWidth: 400);

                Label lblName = UI_Utilities.CreateLabel(parent, country.icaO_Code, 0, 0, 50, FontAttributes.Bold);

                Label lblCountry = UI_Utilities.CreateLabel(parent, country.name, 0, 1, 20);
                lblCountry.LineBreakMode = LineBreakMode.WordWrap;

                Button editBtn = UI_Utilities.CreateButton(false, parent, "Edit", country, EditBtn_Clicked, 0, 2);

                Button deleteBtn = UI_Utilities.CreateButton(true, parent, "Delete", country, DeleteBtn_Clicked, 0, 3);
            }
            else
            {
                parent = UI_Utilities.CreateGrid(b, 3, MaximumWidth: 400);
                Label lblName = UI_Utilities.CreateLabel(parent, country.name, 0, 0, 30, FontAttributes.Bold);
                lblName.LineBreakMode = LineBreakMode.WordWrap;

                Button editBtn = UI_Utilities.CreateButton(false, parent, "Edit", country, EditBtn_Clicked, 0, 1);

                Button deleteBtn = UI_Utilities.CreateButton(true, parent, "Delete", country, DeleteBtn_Clicked, 0, 2);

            }

            return b;
        }

        private void DeleteBtn_Clicked(object? sender, EventArgs e)
        {
            Button b = sender as Button;
            DeleteClickedHandler handler = DeleteClicked;
            handler(b.CommandParameter as Country);
        }

        private void EditBtn_Clicked(object sender, EventArgs e)
        {
            Button b = sender as Button;
            EditClickedHandler handler = EditClicked;
            handler(b.CommandParameter as Country);
        }
    }
}
