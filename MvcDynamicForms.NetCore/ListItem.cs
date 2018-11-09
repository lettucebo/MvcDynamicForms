using System;
using System.Collections.Generic;

namespace MvcDynamicForms.NetCore
{
    [Serializable]
    public class ListItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }

        public ListItem()
        {
        }

        public ListItem(string value) : this(value, value)
        {
        }

        public ListItem(string text, string value) : this(text, value, false)
        {
        }

        public ListItem(string text, string value, bool selected)
        {
            this.Text = text;
            this.Value = value;
            this.Selected = selected;
            this.HtmlAttributes = new Dictionary<string, string>();
        }

        public override string ToString()
        {
            return string.Format("[Text: {0}; Value: {1}]", this.Text, this.Value);
        }

        public override bool Equals(object obj)
        {
            return obj != null
                   && obj is ListItem
                   && obj.ToString() == this.ToString();
        }

        public override int GetHashCode()
        {
            return ("b1721411-cec2-4d59-b18d-09a02298d365" + this.ToString()).GetHashCode();
        }

        /// <summary>
        /// Stores html attributes to be rendered with the list item.
        /// </summary>
        public Dictionary<string, string> HtmlAttributes { get; set; }
    }
}