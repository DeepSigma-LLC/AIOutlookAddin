using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_PlugIn
{
    internal static class UIControls
    {
        internal static void LoadListControl(ToolbarRibbon toolbar,  RibbonDropDown control, string[] items)
        {
            if (items.Length == 0)
            {
                ClearDropDown(toolbar, control);
                return;
            }

            control.Items.Clear();
            control.SelectedItem = null;

            foreach (string item in items)
            {
                int lastItemIndex = control.Items.Count;

                control.Items.Add(toolbar.Factory.CreateRibbonDropDownItem());
                control.Items[lastItemIndex].Label = item;
            }

            
        }

        internal static void LoadListControl(ToolbarRibbon toolbar, RibbonComboBox control, string[] items)
        {
            control.Items.Clear();

            foreach (string item in items)
            {
                int lastItemIndex = control.Items.Count;

                control.Items.Add(toolbar.Factory.CreateRibbonDropDownItem());
                control.Items[lastItemIndex].Label = item;
            }
        }

        internal static void ClearDropDown(ToolbarRibbon toolbar, RibbonDropDown control)
        {
            control.Items.Clear();
            control.SelectedItem = null;

            //Add blank
            var blank = toolbar.Factory.CreateRibbonDropDownItem();
            blank.Label = "";
            control.Items.Add(blank);
            control.SelectedItem = blank;
        }
    }
}
