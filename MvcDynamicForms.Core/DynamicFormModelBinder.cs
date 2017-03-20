using System.Web;

namespace MvcDynamicForms.Core
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using MvcDynamicForms.Core.Fields;
    using MvcDynamicForms.Core.Fields.Abstract;

    class DynamicFormModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var postedForm = controllerContext.RequestContext.HttpContext.Request.Form;
            var postedFiles = controllerContext.RequestContext.HttpContext.Request.Files;

            var allKeys = postedForm.AllKeys.Union(postedFiles.AllKeys);

            var form = (Form)bindingContext.Model;
            if (form == null && !string.IsNullOrEmpty(postedForm[MagicStrings.MvcDynamicSerializedForm]))
            {
                form = SerializationUtility.Deserialize<Form>(postedForm[MagicStrings.MvcDynamicSerializedForm]);
            }

            if (form == null)
                throw new NullReferenceException(
                    "The dynamic form object was not found. Be sure to include PlaceHolders.SerializedForm in your form template.");

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
                    foreach (string value in postedForm.GetValues(key))
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
                    chkField.Checked = bool.Parse(postedForm.GetValues(key)[0]);
                }
                else if (dynField is FileUpload)
                {
                    var fileField = (FileUpload)dynField;
                    fileField.PostedFile = postedFiles[key];
                }
                else if (dynField is Hidden)
                {
                    var hiddenField = (Hidden)dynField;
                    hiddenField.Value = postedForm[key];
                }
            }

            form.FireModelBoundEvents();
            return form;
        }
    }
}