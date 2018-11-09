using System;
using System.Linq;
using System.Text;
using MvcDynamicForms.NetCore.Enums;
using MvcDynamicForms.NetCore.Fields.Abstract;

namespace MvcDynamicForms.NetCore.Fields
{
    /// <summary>
    /// Represents a list of html checkbox inputs.
    /// </summary>
    [Serializable]
    public class CheckBoxList : OrientableField
    {
        public override string RenderHtml()
        {
            var html = new StringBuilder(this.Template);
            var inputName = this.GetHtmlId();

            // prompt label
            var prompt = new TagBuilder("label");
            prompt.AddCssClass(this._promptClass);
            prompt.SetInnerText(this.GetPrompt());
            html.Replace(PlaceHolders.Prompt, prompt.ToString());

            // error label
            if (!this.ErrorIsClear)
            {
                var error = new TagBuilder("label");
                error.AddCssClass(this._errorClass);
                ;
                error.SetInnerText(this.Error);
                html.Replace(PlaceHolders.Error, error.ToString());
            }

            // list of checkboxes
            var input = new StringBuilder();
            var ul = new TagBuilder("ul");
            ul.AddCssClass(this._orientation == Orientation.Vertical ? this._verticalClass : this._horizontalClass);
            ul.AddCssClass(this._listClass);
            input.Append(ul.ToString(TagRenderMode.StartTag));

            var choicesList = this._choices.ToList();
            for (int i = 0; i < choicesList.Count; i++)
            {
                ListItem choice = choicesList[i];
                string chkId = inputName + i;

                // open list item
                var li = new TagBuilder("li");
                input.Append(li.ToString(TagRenderMode.StartTag));

                // checkbox input
                var chk = new TagBuilder("input");
                chk.Attributes.Add("type", "checkbox");
                chk.Attributes.Add("name", inputName);
                chk.Attributes.Add("id", chkId);
                chk.Attributes.Add("value", choice.Value);
                if (choice.Selected)
                    chk.Attributes.Add("checked", "checked");
                chk.MergeAttributes(this._inputHtmlAttributes);
                chk.MergeAttributes(choice.HtmlAttributes);
                input.Append(chk.ToString(TagRenderMode.SelfClosing));

                // checkbox label
                var lbl = new TagBuilder("label");
                lbl.Attributes.Add("for", chkId);
                lbl.AddCssClass(this._inputLabelClass);
                lbl.SetInnerText(choice.Text);
                input.Append(lbl.ToString());

                // close list item
                input.Append(li.ToString(TagRenderMode.EndTag));
            }
            input.Append(ul.ToString(TagRenderMode.EndTag));

            // add hidden tag, so that a value always gets sent
            var hidden = new TagBuilder("input");
            hidden.Attributes.Add("type", "hidden");
            hidden.Attributes.Add("id", inputName + "_hidden");
            hidden.Attributes.Add("name", inputName);
            hidden.Attributes.Add("value", string.Empty);
            html.Replace(PlaceHolders.Input, input.ToString() + hidden.ToString(TagRenderMode.SelfClosing));

            // wrapper id
            html.Replace(PlaceHolders.FieldWrapperId, this.GetWrapperId());

            return html.ToString();
        }
    }
}