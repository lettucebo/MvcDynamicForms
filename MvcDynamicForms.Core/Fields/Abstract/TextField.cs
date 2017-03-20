namespace MvcDynamicForms.Core.Fields.Abstract
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents an html input field that will accept a text response from the user.
    /// </summary>
    [Serializable]
    public abstract class TextField : InputField
    {
        private string _regexMessage = "Invalid";

        /// <summary>
        /// A regular expression that will be applied to the user's text respone for validation.
        /// </summary>
        public string RegularExpression { get; set; }

        /// <summary>
        /// The error message that is displayed to the user when their response does no match the regular expression.
        /// </summary>
        public string RegexMessage
        {
            get { return this._regexMessage; }
            set { this._regexMessage = value; }
        }

        private string _value;

        public string Value
        {
            get { return this._value ?? string.Empty; }
            set { this._value = value; }
        }

        public override string Response
        {
            get { return this.Value.Trim(); }
        }

        public override bool Validate()
        {
            this.ClearError();

            if (string.IsNullOrEmpty(this.Response))
            {
                if (this.Required)
                {
                    // invalid: is required and no response has been given
                    this.Error = this.RequiredMessage;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this.RegularExpression))
                {
                    var regex = new Regex(this.RegularExpression);
                    if (!regex.IsMatch(this.Value))
                    {
                        // invalid: has regex and response doesn't match
                        this.Error = this.RegexMessage;
                    }
                }
            }

            this.FireValidated();
            return this.ErrorIsClear;
        }
    }
}