using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Creatidea.Library.Web.DynamicForms.Core.Fields.Abstract
{
    /// <summary>
    /// Represents a dynamically generated form field.
    /// </summary>
    [Serializable]
    public abstract class Field
    {
        protected string _key = Guid.NewGuid().ToString().Replace("-", "");
        protected Form _form;
        private bool _display = true;
        protected Dictionary<string, DataItem> _dataDictionary = new Dictionary<string, DataItem>();


        public Field()
        {
            Template = BuildDefaultTemplate();
        }
        protected abstract string BuildDefaultTemplate();

        /// <summary>
        /// The fields's HTML template.
        /// </summary>
        public string Template { get; set; }
        /// <summary>
        /// Whether the field should be rendered when Form.RenderHtml() is called.
        /// </summary>
        public bool Display
        {
            get
            {
                return _display;
            }
            set
            {
                _display = value;
            }
        }
        /// <summary>
        /// A friendly field group name. Use this to group fields together for your own purposes.
        /// </summary>
        public string Group { get; set; }
        internal Form Form
        {
            get
            {
                return _form;
            }
            set
            {
                _form = value;
            }
        }
        /// <summary>
        /// Used to identify field.
        /// </summary>
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                if (_form != null)
                    _form.Fields.ValidateKey(value);

                _key = value;
            }
        }
        /// <summary>
        /// An arbitrary dictionary of objects. Use this to attach objects to your fields.
        /// </summary>
        public Dictionary<string, DataItem> DataDictionary
        {
            get
            {
                return _dataDictionary;
            }
        }
        /// <summary>
        /// Whether the DataDictionary contains any data objects that will be rendered.
        /// </summary>
        public bool HasClientData
        {
            get
            {
                return _dataDictionary.Where(x => x.Value.ClientSide).Count() > 0;
            }
        }
        /// <summary>
        /// The relative position that the field is rendered to html.
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Renders the field as html.
        /// </summary>
        /// <returns>Returns a string containing the rendered html of the Field object.</returns>
        public abstract string RenderHtml();
        /// <summary>
        /// Retrieves a strongly typed object from the DataDictionary;
        /// </summary>
        public T GetDataValue<T>(string key)
        {
            DataItem data;
            if (!_dataDictionary.TryGetValue(key, out data))
            {
                return default(T);
            }
            return (T)data.Value;
        }
        /// <summary>
        /// Adds an object to the data dictionary. By default, the object will not be rendered.
        /// </summary>
        /// <param name="key">The dictionary key.</param>
        /// <param name="value">The object to store</param>
        public void AddDataValue(string key, object value)
        {
            AddDataValue(key, value, false);
        }
        /// <summary>
        /// Adds an object to the data dictionary.
        /// </summary>
        /// <param name="key">The dictionary key.</param>
        /// <param name="value">The object to store.</param>
        /// <param name="clientSide">Whether the data will be rendered on the client.</param>
        public void AddDataValue(string key, object value, bool clientSide)
        {
            _dataDictionary.Add(key, new DataItem(value, clientSide));
        }
        /// <summary>
        /// Gets a value that can be associated with an HTML input element's id or name attribute.
        /// </summary>
        public string GetHtmlId()
        {
            string id = _form.FieldPrefix + _key;

            if (!Regex.IsMatch(id, RegexPatterns.HtmlId))
                throw new Exception("The combination of Form.FieldPrefix + Field.Key does not produce a valid id attribute value for an HTML element. It must begin with a letter and can only contain letters, digits, hyphens, and underscores.");

            return id;
        }

        protected string GetWrapperId()
        {
            return GetHtmlId() + "_wrapper";
        }
    }
}