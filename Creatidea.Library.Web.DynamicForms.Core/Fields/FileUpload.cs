using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Creatidea.Library.Web.DynamicForms.Core.Fields.Abstract;

namespace Creatidea.Library.Web.DynamicForms.Core.Fields
{
    public delegate void FilePostedEventHandler( FileUpload fileUploadField, EventArgs e);

    [Serializable]
    public class FileUpload : InputField
    {
        public event FilePostedEventHandler Posted;

        [NonSerialized]
        private HttpPostedFileBase _postedFile;
        private string _invalidExtensionError = "Invalid File Type";

        public string InvalidExtensionError
        {
            get { return _invalidExtensionError; }
            set { _invalidExtensionError = value; }
        }

        public HttpPostedFileBase PostedFile
        {
            get { return _postedFile; }
            set { _postedFile = value; }
        }

        /// <summary>
        /// A comma delimited list of acceptable file extensions.
        /// </summary>
        public string ValidExtensions { get; set; }

        public bool FileWasPosted
        {
            get
            {
                return PostedFile != null && !string.IsNullOrEmpty(PostedFile.FileName);
            }
        }

        public override string Response
        {
            get { return PostedFile.FileName; }
        }

        public override bool Validate()
        {
            ClearError();

            if (Required && !FileWasPosted)
            {
                Error = RequiredMessage;
            }
            else if (!string.IsNullOrEmpty(ValidExtensions))
            {
                var exts = ValidExtensions.ToUpper().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (!exts.Contains(Path.GetExtension(PostedFile.FileName).ToUpper()))
                {
                    Error = InvalidExtensionError;
                }
            }

            FireValidated();
            return ErrorIsClear;
        }

        public override string RenderHtml()
        {
            var html = new StringBuilder(Template);
            var inputName = GetHtmlId();

            // prompt label
            var prompt = new TagBuilder("label");
            prompt.SetInnerText(GetPrompt());
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("class", _promptClass);
            html.Replace(PlaceHolders.Prompt, prompt.ToString());

            // error label
            if (!ErrorIsClear)
            {
                var error = new TagBuilder("label");
                error.Attributes.Add("for", inputName);
                error.Attributes.Add("class", _errorClass);
                error.SetInnerText(Error);
                html.Replace(PlaceHolders.Error, error.ToString());
            }

            // input element
            var txt = new TagBuilder("input");
            txt.Attributes.Add("name", inputName);
            txt.Attributes.Add("id", inputName);
            txt.Attributes.Add("type", "file");
            txt.MergeAttributes(_inputHtmlAttributes);
            html.Replace(PlaceHolders.Input, txt.ToString(TagRenderMode.SelfClosing));

            // wrapper id
            html.Replace(PlaceHolders.FieldWrapperId, GetWrapperId());

            return html.ToString();
        }

        internal void FireFilePosted()
        {
            if (FileWasPosted && Posted != null)
                Posted(this, new EventArgs());
        }
    }    
}