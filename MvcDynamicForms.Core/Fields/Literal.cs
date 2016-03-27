namespace MvcDynamicForms.Core.Fields
{
    using System;
    using System.Text;

    using MvcDynamicForms.Core.Fields.Abstract;

    /// <summary>
    /// Represents html to be rendered on the form.
    /// </summary>
    [Serializable]
    public class Literal : Field
    {
        /// <summary>
        /// The html to be rendered on the form.
        /// </summary>
        public string Html { get; set; }

        public override string RenderHtml()
        {
            var html = new StringBuilder(this.Template);
            html.Replace(PlaceHolders.Literal, this.Html);

            // wrapper id
            html.Replace(PlaceHolders.FieldWrapperId, this.GetWrapperId());

            return html.ToString();
        }

        protected override string BuildDefaultTemplate()
        {
            return PlaceHolders.Literal;
            
        }
    }
}
