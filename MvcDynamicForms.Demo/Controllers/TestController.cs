namespace MvcDynamicForms.Demo.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using MvcDynamicForms.Core;
    using MvcDynamicForms.Core.Fields;
    using MvcDynamicForms.Demo.Models;

    public class TestController : Controller
    {
        public ActionResult Index()
        {
            var form = FormProvider.GetForm();

            // layout templates
            form.Template = string.Format(@"<fieldset><legend>Basic Info</legend><table>{0}</table>{1}{2}</fieldset>",
                PlaceHolders.Fields,
                PlaceHolders.SerializedForm,
                PlaceHolders.DataScript);

            form.SetFieldTemplates(
                string.Format(@"<tr><th valign=""top"">{0}</th><td valign=""top"">{1}{2}{3}</td></tr>",
                    PlaceHolders.Prompt, PlaceHolders.Error, PlaceHolders.Input, PlaceHolders.Literal),
                form.Fields.ToArray());

            form.SetFieldTemplates(
                string.Format(@"<tr><th /><td valign=""top"" class=""chkCell"">{0}{1}{2}</td></tr>",
                    PlaceHolders.Error, PlaceHolders.Input, PlaceHolders.Prompt),
                    form.Fields.Where(x => x is CheckBox).ToArray());

            form.Fields.Single(x => x.Key == "description").Template = "</table><p>" + PlaceHolders.Literal + "</p><table>";

            form.Serialize = true;

            return this.View(form);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(Form form)
        {
            if (form.Validate())
            {
                return this.View("Responses", form);
            }

            return this.View(form);
        }
    }
}