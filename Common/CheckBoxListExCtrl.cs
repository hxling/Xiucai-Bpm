using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls; 

namespace Xiucai.Common
{
    public class CheckBoxListExCtrl : CheckBoxList, IRepeatInfoUser
    {

        void IRepeatInfoUser.RenderItem(ListItemType itemType, int repeatIndex, RepeatInfo repeatInfo, HtmlTextWriter writer)
        {
            writer.WriteBeginTag("input");
            writer.WriteAttribute("type", "checkbox");
            writer.WriteAttribute("name", UniqueID);
            writer.WriteAttribute("id", ClientID + "_" +
            repeatIndex.ToString(NumberFormatInfo.InvariantInfo));
            writer.WriteAttribute("value", Items[repeatIndex].Value);
            System.Web.UI.AttributeCollection attrs = Items[repeatIndex].Attributes;
            foreach (string key in attrs.Keys)
            {
                writer.WriteAttribute(key, attrs[key]);
            }
            writer.Write(">");
            writer.WriteBeginTag("label");
            writer.Write(">");
            writer.Write(Items[repeatIndex].Text);
            writer.WriteEndTag("label");
        }

    }
}
