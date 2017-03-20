namespace MvcDynamicForms.Core.Fields
{
    using System;
    using System.Text;
    using System.Web.Mvc;
    using MvcDynamicForms.Core.Fields.Abstract;

    /// <summary>
    /// Represents a single html checkbox input field.
    /// </summary>
    [Serializable]
    public class CheckBox : InputField
    {
        private string _checkedValue = "Yes";
        private string _uncheckedValue = "No";

        /// <summary>
        /// The text to be used as the user's response when they check the checkbox.
        /// </summary>
        public string CheckedValue
        {
            get { return this._checkedValue; }
            set { this._checkedValue = value; }
        }

        /// <summary>
        /// The text to be used as the user's response when they do not check the checkbox.
        /// </summary>
        public string UncheckedValue
        {
            get { return this._uncheckedValue; }
            set { this._uncheckedValue = value; }
        }

        /// <summary>
        /// The state of the checkbox.
        /// </summary>
        public bool Checked { get; set; }

        public override string Response
        {
            get { return this.Checked ? this._checkedValue : this._uncheckedValue; }
        }

        public CheckBox()
        {
            // give the checkbox a different default prompt class
            this._promptClass = "MvcDynamicCheckboxPrompt";
        }

        public override bool Validate()
        {
            this.ClearError();

            if (this.Required && !this.Checked)
            {
                // Isn't valid
                this.Error = this._requiredMessage;
            }

            this.FireValidated();
            return this.ErrorIsClear;
        }

        public override string RenderHtml()
        {
            var inputName = this.GetHtmlId();
            var html = new StringBuilder(this.Template);

            // error label
            if (!this.ErrorIsClear)
            {
                var error = new TagBuilder("label");
                error.SetInnerText(this.Error);
                error.Attributes.Add("for", inputName);
                error.AddCssClass(this._errorClass);
                html.Replace(PlaceHolders.Error, error.ToString());
            }

            // checkbox input
            var chk = new TagBuilder("input");
            chk.Attributes.Add("id", inputName);
            chk.Attributes.Add("name", inputName);
            chk.Attributes.Add("type", "checkbox");
            if (this.Checked) chk.Attributes.Add("checked", "checked");
            chk.Attributes.Add("value", bool.TrueString);
            chk.MergeAttributes(this._inputHtmlAttributes);

            // hidden input (so that value is posted when checkbox is unchecked)
            var hdn = new TagBuilder("input");
            hdn.Attributes.Add("type", "hidden");
            hdn.Attributes.Add("id", inputName + "_hidden");
            hdn.Attributes.Add("name", inputName);
            hdn.Attributes.Add("value", bool.FalseString);
            html.Replace(PlaceHolders.Input,
                chk.ToString(TagRenderMode.SelfClosing) + hdn.ToString(TagRenderMode.SelfClosing));

            // prompt label
            var prompt = new TagBuilder("label");
            prompt.SetInnerText(this.GetPrompt());
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("class", this._promptClass);
            html.Replace(PlaceHolders.Prompt, prompt.ToString());

            // wrapper id
            html.Replace(PlaceHolders.FieldWrapperId, this.GetWrapperId());

            return html.ToString();
        }

        protected override string BuildDefaultTemplate()
        {
            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("MvcFieldWrapper");
            wrapper.Attributes["id"] = PlaceHolders.FieldWrapperId;
            wrapper.InnerHtml = PlaceHolders.Error + PlaceHolders.Input + PlaceHolders.Prompt;
            return wrapper.ToString();
        }
    }
}