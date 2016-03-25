using System.Web.Mvc;
using Creatidea.Library.Web.DynamicForms.Core;
using Creatidea.Library.Web.DynamicForms.Demo.Models;

namespace Creatidea.Library.Web.DynamicForms.Demo.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /*
         * First off, these Demos do the exact same thing from the end user's perspective.
         * The difference is in how the Form object is persisted across requests.
         * 
         * Most often, you'll need to keep the original Form and Field objects around for as long 
         * as your user is working on completing the form. This is because, when InputField 
         * objects are constructed, they are keyed with a new Guid. See Demos 1 & 2 for examples.
         * 
         * You can key your InputFields manually by setting the InputField.Key property.
         * If you do this and can guarantee that the Fields and their Keys will not change after
         * a complete reconstruction of all objects, then you don't have to persist the objects across
         * requests. See Demo 3.
         * 
         * In Demo 1, the Form object graph is serialized to a string and stored in a hidden field
         * in the page's HTML.
         * 
         * In Demo 2, the Form object graph is simply stored in TempData (short lived session state).
         * 
         * In Demo 3, the Form object graph is not persisted across requests. It is reconstructed
         * on each request and the InputField's keys are set manually.
         * 
         * The serialization approach (demo 1) results in more concise code in the controller. 
         * Serializing the Form is also more reliable, in my opinion.
         * 
         * However, response time increases because of serialized data 
         * and the (de)serialization process takes time, as well.
         * 
         * The approach you take depends on your needs.
         */

        public ActionResult Demo1()
        {
            var form = FormProvider.GetForm();

            // we are going to store the form and 
            // the field objects on the page across requests
            form.Serialize = true;

            return View("Demo", form);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Demo1(Form form)
        {   //                           ^
            // no need to retrieve the form object from anywhere
            // just use a parameter on the Action method that we are posting to

            if (form.Validate()) //input is valid
                return View("Responses", form);

            // input is not valid
            return View("Demo", form);
        }

        public ActionResult Demo2()
        {
            var form = FormProvider.GetForm();

            // we are going to store the form 
            // in server memory across requests
            TempData["form"] = form;

            return View("Demo", form);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionName("Demo2")]
        public ActionResult Demo2Post()
        {
            // we have to get the form object from
            // server memory and manually perform model binding
            var form = (Form)TempData["form"];
            UpdateModel(form);

            if (form.Validate()) // input is valid
                return View("Responses", form);

            // input is not valid
            TempData["form"] = form;
            return View("Demo", form);
        }

        public ActionResult Demo3()
        {
            // recreate the form and set the keys
            var form = FormProvider.GetForm();
            Demo3SetKeys(form);

            // set user input on recreated form
            UpdateModel(form);

            if (Request.HttpMethod == "POST" && form.Validate()) // input is valid
                return View("Responses", form);

            // input is not valid
            return View("Demo", form);
        }

        void Demo3SetKeys(Form form)
        {
            int key = 1;
            foreach (var field in form.InputFields)
            {
                field.Key = key++.ToString();
            }
        }
    }
}
