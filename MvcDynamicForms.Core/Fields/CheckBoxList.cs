using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Creatidea.Library.Web.DynamicForms.Core.Enums;
using Creatidea.Library.Web.DynamicForms.Core.Fields.Abstract;

namespace Creatidea.Library.Web.DynamicForms.Core.Fields
{
    /// <summary>
    /// Represents a list of html checkbox inputs.
    /// </summary>
    [Serializable]
    public class CheckBoxList : OrientableField
    {
        public override string RenderHtml()
        {
            var html = new StringBuilder(Template);
            var inputName = GetHtmlId();

            // prompt label
            var prompt = new TagBuilder("label");
            prompt.AddCssClass(_promptClass);
            prompt.SetInnerText(GetPrompt());
            html.Replace(PlaceHolders.Prompt, prompt.ToString());

            // error label
            if (!ErrorIsClear)
            {
                var error = new TagBuilder("label");
                error.AddCssClass(_errorClass); ;
                error.SetInnerText(Error);
                html.Replace(PlaceHolders.Error, error.ToString());
            }

            // list of checkboxes
            var input = new StringBuilder();
            var ul = new TagBuilder("ul");
            ul.AddCssClass(_orientation == Orientation.Vertical ? _verticalClass : _horizontalClass);
            ul.AddCssClass(_listClass);
            input.Append(ul.ToString(TagRenderMode.StartTag));

            var choicesList = _choices.ToList();
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
                chk.MergeAttributes(_inputHtmlAttributes);
                chk.MergeAttributes(choice.HtmlAttributes);
                input.Append(chk.ToString(TagRenderMode.SelfClosing));

                // checkbox label
                var lbl = new TagBuilder("label");
                lbl.Attributes.Add("for", chkId);
                lbl.AddCssClass(_inputLabelClass);
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
            html.Replace(PlaceHolders.FieldWrapperId, GetWrapperId());

            return html.ToString();
        }
    }
}
