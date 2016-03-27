namespace MvcDynamicForms.Core.Fields
{
    using System;
    using System.Text;
    using System.Web.Mvc;

    using MvcDynamicForms.Core.Fields.Abstract;

    /// <summary>
    /// Represents an html textarea element.
    /// </summary>
    [Serializable]
    public class TextArea : TextField
    {
        public override string RenderHtml()
        {
            var html = new StringBuilder(this.Template);
            var inputName = this.GetHtmlId();

            // prompt label
            var prompt = new TagBuilder("label");
            prompt.SetInnerText(this.GetPrompt());
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("class", this._promptClass);
            html.Replace(PlaceHolders.Prompt, prompt.ToString());

            // error label
            if (!this.ErrorIsClear)
            {
                var error = new TagBuilder("label");
                error.Attributes.Add("for", inputName);
                error.Attributes.Add("class", this._errorClass);
                error.SetInnerText(this.Error);
                html.Replace(PlaceHolders.Error, error.ToString());
            }

            // input element
            var txt = new TagBuilder("textarea");
            txt.Attributes.Add("name", inputName);
            txt.Attributes.Add("id", inputName);
            txt.SetInnerText(this.Value);
            txt.MergeAttributes(this._inputHtmlAttributes);
            html.Replace(PlaceHolders.Input, txt.ToString());

            // wrapper id
            html.Replace(PlaceHolders.FieldWrapperId, this.GetWrapperId());

            return html.ToString();
        }
    }
}