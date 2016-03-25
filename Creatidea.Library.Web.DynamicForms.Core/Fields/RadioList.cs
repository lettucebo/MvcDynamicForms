using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Creatidea.Library.Web.DynamicForms.Core.Enums;
using Creatidea.Library.Web.DynamicForms.Core.Fields.Abstract;

namespace Creatidea.Library.Web.DynamicForms.Core.Fields
{
    /// <summary>
    /// Represents a list of html radio button inputs.
    /// </summary>
    [Serializable]
    public class RadioList : OrientableField
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
                error.AddCssClass(_errorClass);
                error.SetInnerText(Error);
                html.Replace(PlaceHolders.Error, error.ToString());
            }

            // list of radio buttons        
            var input = new StringBuilder();
            var ul = new TagBuilder("ul");
            ul.Attributes.Add("class", _orientation == Orientation.Vertical ? _verticalClass : _horizontalClass);
            ul.AddCssClass(_listClass);
            input.Append(ul.ToString(TagRenderMode.StartTag));

            var choicesList = _choices.ToList();
            for (int i = 0; i < choicesList.Count; i++)
            {
                ListItem choice = choicesList[i];
                string radId = inputName + i;

                // open list item
                var li = new TagBuilder("li");
                input.Append(li.ToString(TagRenderMode.StartTag));

                // radio button input
                var rad = new TagBuilder("input");
                rad.Attributes.Add("type", "radio");
                rad.Attributes.Add("name", inputName);
                rad.Attributes.Add("id", radId);
                rad.Attributes.Add("value", choice.Value);
                if (choice.Selected) 
                    rad.Attributes.Add("checked", "checked");
                rad.MergeAttributes(_inputHtmlAttributes);
                rad.MergeAttributes(choice.HtmlAttributes);
                input.Append(rad.ToString(TagRenderMode.SelfClosing));

                // checkbox label
                var lbl = new TagBuilder("label");
                lbl.Attributes.Add("for", radId);
                lbl.Attributes.Add("class", _inputLabelClass);
                lbl.SetInnerText(choice.Text);
                input.Append(lbl.ToString());

                // close list item
                input.Append(li.ToString(TagRenderMode.EndTag));
            }
            input.Append(ul.ToString(TagRenderMode.EndTag));
            html.Replace(PlaceHolders.Input, input.ToString());

            // wrapper id
            html.Replace(PlaceHolders.FieldWrapperId, GetWrapperId());

            return html.ToString();
            
        }
    }
}