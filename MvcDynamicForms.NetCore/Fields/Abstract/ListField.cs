using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcDynamicForms.NetCore.Fields.Abstract
{
    /// <summary>
    /// Represents an html input field that contains choices for the end user to choose from.
    /// </summary>
    [Serializable]
    public abstract class ListField : InputField
    {
        protected List<ListItem> _choices = new List<ListItem>();
        protected string _responseDelimiter = ", ";

        /// <summary>
        /// Gets or sets choices represented as a comma delimited string. When setting, the current choices are first cleared.
        /// </summary>
        public string CommaDelimitedChoices
        {
            get
            {
                var delim = ",";
                var sb = new StringBuilder();
                foreach (var choice in this.Choices)
                {
                    sb.Append(choice.Text + delim);
                }
                return sb.ToString().TrimEnd(delim.ToCharArray());
            }
            set
            {
                this._choices.Clear();
                this.AddChoices(value);
            }
        }

        /// <summary>
        /// The choices that the end user can choose from.
        /// </summary>
        public List<ListItem> Choices
        {
            get { return this._choices; }
            set { this._choices = value; }
        }

        /// <summary>
        /// The text used to delimit multiple choices from the end user.
        /// </summary>
        public string ResponseDelimiter
        {
            get { return this._responseDelimiter; }
            set { this._responseDelimiter = value; }
        }

        public override string Response
        {
            get
            {
                // builds a delimited list of the responses
                var value = new StringBuilder();

                foreach (var choice in this._choices)
                {
                    value.Append(choice.Selected ? choice.Value + this._responseDelimiter : string.Empty);
                }

                return value.ToString().TrimEnd(this._responseDelimiter.ToCharArray()).Trim();
            }
        }

        public override bool Validate()
        {
            this.ClearError();
            if (this.Required && !this._choices.Select(x => x.Selected).Contains(true))
            {
                // invalid: required and no checkbox was selected
                this.Error = this._requiredMessage;
                return false;
            }

            // valid
            this.FireValidated();
            return this.ErrorIsClear;
        }

        /// <summary>
        /// Provides a convenient way to add choices.
        /// </summary>
        /// <param name="choices">A delimited string of choices.</param>
        /// <param name="delimiter">The delimiter used to seperate the choices.</param>
        public void AddChoices(string choices, string delimiter)
        {
            if (string.IsNullOrEmpty(choices)) return;

            choices.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList()
                .ForEach(c => this._choices.Add(new ListItem(c)));
        }

        /// <summary>
        /// Provides a convenient way to add choices.
        /// </summary>
        /// <param name="choices">A comma delimited string of choices.</param>
        public void AddChoices(string choices)
        {
            this.AddChoices(choices, ",");
        }
    }
}