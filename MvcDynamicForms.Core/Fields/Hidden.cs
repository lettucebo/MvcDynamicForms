using System;
using System.Text;
using System.Web.Mvc;
using Creatidea.Library.Web.DynamicForms.Core.Fields.Abstract;

namespace Creatidea.Library.Web.DynamicForms.Core.Fields
{
    /// <summary>
    /// Represents an html hidden input element.
    /// </summary>
    [Serializable]
    public class Hidden : InputField
    {
        /// <summary>
        /// The value of the hidden field.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The value of the hidden field. Returns the Value property.
        /// </summary>
        public override string Response
        {
            get
            {
                return Value;
            }
        }

        /// <summary>
        /// Validates the hidden field. This always returns true.
        /// </summary>
        public override bool Validate()
        {
            return true;
        }

        /// <summary>
        /// Render the hidden field as an html hidden input element.
        /// </summary>
        /// <returns></returns>
        public override string RenderHtml()
        {
            var html = new StringBuilder(Template);
            var inputName = GetHtmlId();

            // input element
            var hdn = new TagBuilder("input");
            hdn.Attributes.Add("name", inputName);
            hdn.Attributes.Add("id", inputName);
            hdn.Attributes.Add("type", "hidden");
            hdn.Attributes.Add("value", Value);
            hdn.MergeAttributes(_inputHtmlAttributes);
            html.Replace(PlaceHolders.Input, hdn.ToString(TagRenderMode.SelfClosing));

            // wrapper id
            html.Replace(PlaceHolders.FieldWrapperId, GetWrapperId());

            return html.ToString();
        }

        protected override string BuildDefaultTemplate()
        {
            return PlaceHolders.Input;
        }

    }
}