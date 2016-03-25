using System;
using System.Text;
using System.Web.Mvc;
using Creatidea.Library.Web.DynamicForms.Core.Fields.Abstract;

namespace Creatidea.Library.Web.DynamicForms.Core.Fields
{
    /// <summary>
    /// Represents an html textarea element.
    /// </summary>
    [Serializable]
    public class TextArea : TextField
    {
        public override string RenderHtml()
        {
            var html = new StringBuilder(Template);
            var inputName = GetHtmlId();

            // prompt label
            var prompt = new TagBuilder("label");
            prompt.SetInnerText(GetPrompt());
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("class", _promptClass);
            html.Replace(PlaceHolders.Prompt, prompt.ToString());

            // error label
            if (!ErrorIsClear)
            {
                var error = new TagBuilder("label");
                error.Attributes.Add("for", inputName);
                error.Attributes.Add("class", _errorClass);
                error.SetInnerText(Error);
                html.Replace(PlaceHolders.Error, error.ToString());
            }

            // input element
            var txt = new TagBuilder("textarea");
            txt.Attributes.Add("name", inputName);
            txt.Attributes.Add("id", inputName);
            txt.SetInnerText(Value);
            txt.MergeAttributes(_inputHtmlAttributes);
            html.Replace(PlaceHolders.Input, txt.ToString());

            // wrapper id
            html.Replace(PlaceHolders.FieldWrapperId, GetWrapperId());

            return html.ToString();
        }
    }
}