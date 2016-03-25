using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;
using Creatidea.Library.Web.DynamicForms.Core.Fields;
using Creatidea.Library.Web.DynamicForms.Core.Fields.Abstract;

namespace Creatidea.Library.Web.DynamicForms.Core
{
    /// <summary>
    /// Represents an html input form that can be dynamically rendered at runtime.
    /// </summary>
    [Serializable]
    [ModelBinder(typeof(DynamicFormModelBinder))]
    public class Form
    {
        private string _fieldPrefix = "MvcDynamicField_";
        private FieldList _fields;
        public string Template { get; set; }

        /// <summary>
        /// A collection of Field objects.
        /// </summary>
        public FieldList Fields
        {
            get
            {
                return _fields;
            }
        }
        /// <summary>
        /// Gets or sets the string that is used to prefix html input elements' id and name attributes.
        /// This value must comply with the naming rules for HTML id attributes and Input elements' name attributes.
        /// </summary>
        public string FieldPrefix
        {
            get
            {
                return _fieldPrefix;
            }
            set
            {
                _fieldPrefix = value ?? "";
            }
        }
        /// <summary>
        /// Gets or sets the boolean value determining if the form should serialize itself into the string returned by the RenderHtml() method.
        /// </summary>
        public bool Serialize { get; set; }
        /// <summary>
        /// Returns an enumeration of Field objects that are of type InputField.
        /// </summary>
        public IEnumerable<InputField> InputFields
        {
            get
            {
                return _fields.OfType<InputField>();
            }
        }

        public Form()
        {
            _fields = new FieldList(this);
            Template = BuildDefaultTemplate();
        }

        private string BuildDefaultTemplate()
        {
            var formWrapper = new TagBuilder("div");
            formWrapper.AddCssClass("MvcDynamicForm");
            formWrapper.InnerHtml = PlaceHolders.Fields + PlaceHolders.SerializedForm + PlaceHolders.DataScript;
            return formWrapper.ToString();
        }

        /// <summary>
        /// Renders a script block containing a JSON representation of each fields' client side data.
        /// </summary>
        public string RenderDataScript(string jsVarName)
        {
            if (string.IsNullOrEmpty(jsVarName))
                jsVarName = "MvcDynamicFieldData";

            if (_fields.Any(x => x.HasClientData))
            {
                var data = new Dictionary<string, Dictionary<string, DataItem>>();
                foreach (var field in _fields.Where(x => x.HasClientData))
                    data.Add(field.Key, field.DataDictionary);

                var script = new TagBuilder("script");
                script.Attributes["type"] = "text/javascript";
                script.InnerHtml = string.Format("{0}var {1} = {2};",
                    Environment.NewLine,
                    jsVarName,
                    data.ToJson());

                return script.ToString();
            }

            return null;
        }
        /// <summary>
        /// Validates each displayed InputField object contained in the Fields collection. 
        /// Validation causes the Error property to be set for each InputField object.
        /// </summary>
        /// <returns>Returns true if every InputField object is valid. False is returned otherwise.</returns>
        public bool Validate()
        {
            return Validate(true);
        }
        /// <summary>
        /// Validates each displayed InputField object contained in the Fields collection. 
        /// Validation causes the Error property to be set for each InputField object.
        /// </summary>
        /// <param name="onlyDisplayed">Whether to validate only displayed fields.</param>
        /// <returns>Returns true if every InputField object is valid. False is returned otherwise.</returns>
        public bool Validate(bool onlyDisplayed)
        {
            bool isValid = true;

            foreach (var field in InputFields.Where(x => !onlyDisplayed || x.Display))
                isValid = isValid & field.Validate();

            return isValid;
        }
        /// <summary>
        /// Returns a string containing the rendered HTML of every contained Field object.
        /// Optionally, the form's serialized state and/or JavaScript data can be included in the returned HTML string.
        /// </summary>        
        /// <param name="formatHtml">Determines whether to format the generated html with indentation and whitespace for readability.</param>
        /// <returns>Returns a string containing the rendered html of every contained Field object.</returns>
        public string RenderHtml(bool formatHtml)
        {
            var fieldsHtml = new StringBuilder();
            foreach (var field in _fields.Where(x => x.Display).OrderBy(x => x.DisplayOrder))
                fieldsHtml.Append(field.RenderHtml());

            var html = new StringBuilder(Template);
            html.Replace(PlaceHolders.Fields, fieldsHtml.ToString());

            if (Serialize)
            {
                var hdn = new TagBuilder("input");
                hdn.Attributes["type"] = "hidden";
                hdn.Attributes["id"] = MagicStrings.MvcDynamicSerializedForm;
                hdn.Attributes["name"] = MagicStrings.MvcDynamicSerializedForm;
                hdn.Attributes["value"] = SerializationUtility.Serialize(this);
                html.Replace(PlaceHolders.SerializedForm, hdn.ToString(TagRenderMode.SelfClosing));
            }

            html.Replace(PlaceHolders.DataScript, RenderDataScript(null));

            PlaceHolders.RemoveAll(html);

            if (formatHtml)
                return XDocument.Parse(html.ToString()).ToString();

            return html.ToString();
        }
        /// <summary>
        /// Returns a string containing the rendered html of every contained Field object. The html can optionally include the Form object's state serialized into a hidden field.
        /// </summary>
        /// <returns>Returns a string containing the rendered html of every contained Field object.</returns>
        public string RenderHtml()
        {
            return RenderHtml(false);
        }
        /// <summary>
        /// This method clears the Error property of each contained InputField.
        /// </summary>
        public void ClearAllErrors()
        {
            foreach (var inputField in InputFields)
                inputField.ClearError();
        }
        /// <summary>
        /// This method provides a convenient way of adding multiple Field objects at once.
        /// </summary>
        /// <param name="fields">Field object(s)</param>
        public void AddFields(params Field[] fields)
        {
            foreach (var field in fields)
            {
                _fields.Add(field);
            }
        }
        /// <summary>
        /// Provides a convenient way the end users' responses to each InputField
        /// </summary>
        /// <param name="completedOnly">Determines whether to return a Response object for only InputFields that the end user completed.</param>
        /// <returns>List of Response objects.</returns>
        public List<Response> GetResponses(bool completedOnly)
        {
            var responses = new List<Response>();
            foreach (var field in InputFields.OrderBy(x => x.DisplayOrder))
            {
                var response = new Response
                {
                    Title = field.GetResponseTitle(),
                    Value = field.Response
                };

                if (completedOnly && string.IsNullOrEmpty(response.Value))
                    continue;

                responses.Add(response);
            }

            return responses;
        }
        /// <summary>
        /// Provides a convenient way to set the template for multiple fields.
        /// </summary>
        /// <param name="template">The fields' HTML template.</param>
        public void SetFieldTemplates(string template, params Field[] fields)
        {
            foreach (var field in fields)
                field.Template = template;
        }

        internal void FireModelBoundEvents()
        {
            foreach (var fileUpload in Fields.Where(x => x is FileUpload).Cast<FileUpload>())
            {               
                fileUpload.FireFilePosted();
            }
        }
    }
}