using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Creatidea.Library.Web.DynamicForms.Core.Fields.Abstract
{

    public delegate void ValidatedEventHandler(InputField inputField, InputFieldValidationEventArgs e);

    /// <summary>
    /// Represents a dynamically generated html input field.
    /// </summary>
    [Serializable]
    public abstract class InputField : Field
    {
        public event ValidatedEventHandler Validated;

        protected string _requiredMessage = "Required";
        protected string _promptClass = "MvcDynamicFieldPrompt";
        protected string _errorClass = "MvcDynamicFieldError";
        protected Dictionary<string, string> _inputHtmlAttributes = new Dictionary<string, string>();

        /// <summary>
        /// Used to identify InputFields when working with end users' responses.
        /// </summary>
        public string ResponseTitle { get; set; }
        /// <summary>
        /// The question asked of the end user.
        /// </summary>
        public string Prompt { get; set; }
        /// <summary>
        /// The html class applied to the label element that is used to prompt the user.
        /// </summary>
        public string PromptClass
        {
            get
            {
                return _promptClass;
            }
            set
            {
                _promptClass = value;
            }
        }
        /// <summary>
        /// String representing the user's response to the field.
        /// </summary>
        public abstract string Response { get; }
        /// <summary>
        /// Whether the field must be completed to be valid.
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// The error message that the end user sees if they do not complete the field.
        /// </summary>
        public string RequiredMessage
        {
            get
            {
                return _requiredMessage;
            }
            set
            {
                _requiredMessage = value;
            }
        }
        /// <summary>
        /// The error message that the end user sees.
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// The class attribute of the label element that is used to display an error message to the user.
        /// </summary>
        public string ErrorClass
        {
            get
            {
                return _errorClass;
            }
            set
            {
                _errorClass = value;
            }
        }
        /// <summary>
        /// True if the field is valid; false otherwise.
        /// </summary>
        public bool ErrorIsClear
        {
            get
            {
                return string.IsNullOrEmpty(Error);
            }
        }
        /// <summary>
        /// Collection of html attribute names and values that will be applied to the rendered input elements.
        /// For list fields, these will be applied to every ListItem.
        /// Use the ListItem.HtmlAttributes dictionary for rendering attributes for individual list items.
        /// </summary>
        public Dictionary<string, string> InputHtmlAttributes
        {
            get
            {
                return _inputHtmlAttributes;
            }
            set
            {
                _inputHtmlAttributes = value;
            }
        }
        /// <summary>
        /// Validates the user's response.
        /// </summary>
        /// <returns></returns>
        public abstract bool Validate();
        /// <summary>
        /// Removes the message stored in the Error property.
        /// </summary>
        public void ClearError()
        {
            Error = null;
        }

        protected override string BuildDefaultTemplate()
        {
            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("MvcFieldWrapper");
            wrapper.Attributes["id"] = PlaceHolders.FieldWrapperId;
            wrapper.InnerHtml = PlaceHolders.Prompt + PlaceHolders.Error + PlaceHolders.Input;
            return wrapper.ToString();
        }

        protected string GetPrompt()
        {
            return Prompt ?? ResponseTitle ?? _key;
        }

        internal string GetResponseTitle()
        {
            return ResponseTitle ?? Prompt ?? _key;
        }

        protected virtual void FireValidated()
        {
            if (Validated != null)
                Validated(this, new InputFieldValidationEventArgs { IsValid = ErrorIsClear });
        }

    }

    public class InputFieldValidationEventArgs : EventArgs
    {
        public bool IsValid { get; set; }
    }

}