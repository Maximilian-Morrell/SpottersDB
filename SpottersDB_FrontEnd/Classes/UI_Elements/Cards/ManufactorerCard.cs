using Microsoft.Maui.Controls.Shapes;
using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;
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
        public delegate EventHandler DeleteClickedHandler(Manufactorer manufactorer);
        public event DeleteClickedHandler DeleteClicked;

        public async Task<Border> Card(Manufactorer manufactorer)
        {
            Border b = UI_Utilities.CreateBorder();

            Grid parent = UI_Utilities.CreateGrid(b, 4, MaximumHeight: 250);

            Label lblName = UI_Utilities.CreateLabel(parent, manufactorer.name, 0, 0, 50, FontAttributes.Bold);

            Country c = await manufactorer.GetRegion();
            Label lblRegion = UI_Utilities.CreateLabel(parent, c.name, 0, 1, 20);
            lblRegion.LineBreakMode = LineBreakMode.WordWrap;

            Button editBtn = UI_Utilities.CreateButton(false, parent, "Edit", manufactorer, EditBtn_Clicked, 0, 2);

            Button deleteBtn = UI_Utilities.CreateButton(true, parent, "Delete", manufactorer, DeleteBtn_Clicked, 0, 3);

            return b;
        }

        private void DeleteBtn_Clicked(object? sender, EventArgs e)
        {
            Button b = sender as Button;
            DeleteClickedHandler handler = DeleteClicked;
            handler(b.CommandParameter as Manufactorer);
        }

        private void EditBtn_Clicked(object sender, EventArgs e)
        {
            Button b = sender as Button;
            EditClickHandler handler = EditClicked;
            handler(b.CommandParameter as Manufactorer);
        }
    }
}
