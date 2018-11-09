using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcDynamicForms.NetCore.Fields;
using MvcDynamicForms.NetCore.Fields.Abstract;
using Newtonsoft.Json;

namespace MvcDynamicForms.NetCore
{
    public class FormModelBinder : IModelBinder
    {
        // created with https://www.stevejgordon.co.uk/html-encode-string-aspnet-core-model-binding
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var postedForm = AppHttpContext.Current.Request.Form;
            var postedFiles = AppHttpContext.Current.Request.Form.Files;

            var allKeys = postedForm.Keys;

            var form = (Form)bindingContext.Model;
            if (form == null && !string.IsNullOrEmpty(postedForm["MvcDynamicSerializedForm"]))
            {
                form = SerializationUtility.Deserialize<Form>(postedForm["MvcDynamicSerializedForm"]);
            }

            if (form == null)
                throw new NullReferenceException("The dynamic form object was not found. Be sure to include PlaceHolders.SerializedForm in your form template.");

            foreach (var key in allKeys.Where(x => x.StartsWith(form.FieldPrefix)))
            {
                string fieldKey = key.Remove(0, form.FieldPrefix.Length);
                InputField dynField = form.InputFields.SingleOrDefault(f => f.Key == fieldKey);

                if (dynField == null)
                    continue;

                if (dynField is TextField)
                {
                    var txtField = (TextField)dynField;
                    txtField.Value = postedForm[key];
                }
                else if (dynField is ListField)
                {
                    var lstField = (ListField)dynField;

                    // clear all choice selections            
                    foreach (var choice in lstField.Choices)
                        choice.Selected = false;

                    // set current selections
                    foreach (string value in postedForm[key])
                    {
                        var choice = lstField.Choices.FirstOrDefault(x => x.Value == value);
                        if (choice != null)
                            choice.Selected = true;
                    }

                    //lstField.Choices.Remove(.Remove(""); what was this for?
                }
                else if (dynField is CheckBox)
                {
                    var chkField = (CheckBox)dynField;
                    chkField.Checked = bool.Parse(postedForm[key][0]);
                }
                else if (dynField is FileUpload)
                {
                    var fileField = (FileUpload)dynField;
                    // fileField.PostedFile = postedFiles[key];
                }
                else if (dynField is Hidden)
                {
                    var hiddenField = (Hidden)dynField;
                    hiddenField.Value = postedForm[key];
                }
            }

            var result = form;
            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }


    }
}
