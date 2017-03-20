namespace MvcDynamicForms.Core.Fields
{
    using System;
    using System.Text;
    using System.Web.Mvc;
    using MvcDynamicForms.Core.Fields.Abstract;

    /// <summary>
    /// Represents an html select element.
    /// </summary>
    [Serializable]
    public class Select : ListField
    {
        /// <summary>
        /// The number of options to display at a time.
        /// </summary>
        public int Size
        {
            get
            {
                string size;
                return this._inputHtmlAttributes.TryGetValue("size", out size) ? int.Parse(size) : 1;
            }
            set { this._inputHtmlAttributes["size"] = value.ToString(); }
        }

        /// <summary>
        /// Determines whether the select element will accept multiple selections.
        /// </summary>
        public bool MultipleSelection
        {
            get
            {
                string multiple;
                if (this._inputHtmlAttributes.TryGetValue("multiple", out multiple))
                {
                    return multiple.ToLower() == "multiple";
                }
                return false;
            }
            set { this._inputHtmlAttributes["multiple"] = value.ToString(); }
        }

        /// <summary>
        /// The text to be rendered as the first option in the select list when ShowEmptyOption is set to true.
        /// </summary>
        public string EmptyOption { get; set; }

        /// <summary>
        /// Determines whether a valueless option is rendered as the first option in the list.
        /// </summary>
        public bool ShowEmptyOption { get; set; }

        public override string RenderHtml()
        {
            var html = new StringBuilder(this.Template);
            var inputName = this.GetHtmlId();

            // prompt
            var prompt = new TagBuilder("label");
            prompt.AddCssClass(this._promptClass);
            prompt.Attributes.Add("for", inputName);
            prompt.SetInnerText(this.GetPrompt());
            html.Replace(PlaceHolders.Prompt, prompt.ToString());

            // error label
            if (!this.ErrorIsClear)
            {
                var error = new TagBuilder("label");
                error.AddCssClass(this._errorClass);
                error.Attributes.Add("for", inputName);
                error.SetInnerText(this.Error);
                html.Replace(PlaceHolders.Error, error.ToString());
            }

            // open select element
            var input = new StringBuilder();
            var select = new TagBuilder("select");
            select.Attributes.Add("id", inputName);
            select.Attributes.Add("name", inputName);
            select.MergeAttributes(this._inputHtmlAttributes);
            input.Append(select.ToString(TagRenderMode.StartTag));

            // initial empty option
            if (this.ShowEmptyOption)
            {
                var opt = new TagBuilder("option");
                opt.Attributes.Add("value", null);
                opt.SetInnerText(this.EmptyOption);
                input.Append(opt.ToString());
            }

            // options
            foreach (var choice in this._choices)
            {
                var opt = new TagBuilder("option");
                opt.Attributes.Add("value", choice.Value);
                if (choice.Selected)
                    opt.Attributes.Add("selected", "selected");
                opt.MergeAttributes(choice.HtmlAttributes);
                opt.SetInnerText(choice.Text);
                input.Append(opt.ToString());
            }

            // close select element
            input.Append(select.ToString(TagRenderMode.EndTag));

            // add hidden tag, so that a value always gets sent for select tags
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