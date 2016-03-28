# MvcDynamicForms

Create dynamic forms in ASP.NET MVC.

# Fork Source

This is fork of ronnieoverby's project [ASP.NET MVC Dynamic Forms](https://mvcdynamicforms.codeplex.com/). 

Because [ASP.NET MVC Dynamic Forms](https://mvcdynamicforms.codeplex.com/) is no longer maintained. And original project is write for ASP.NET MVC 2 and below.

So I update it to ASP.NET MVC5.

# Requirements

this library requires .NET 4.5.2 and above.

# Installation

You can either <a href="https://github.com/lettucebo/MvcDynamicForms.git">download</a> the source and build your own dll or, if you have the NuGet package manager installed, you can grab them automatically.

```
PM> Install-Package MvcDynamicForms
```

Once you have the libraries properly referenced in your project, you can include calls to them in your code. 
For a sample implementation, check the [Demo](https://github.com/lettucebo/MvcDynamicForms/tree/master/MvcDynamicForms.Demo) folder.

Add the following namespaces to use the library:
``` csharp
using MvcDynamicForms.Core;
using MvcDynamicForms.Core.Fields;
```

# Getting Started

First off, below Demos do the exact same thing from the end user's perspective. The difference is in how the Form object is persisted across requests.

Most often, you'll need to keep the original Form and Field objects around for as long as your user is working on completing the form. This is because, when InputField objects are constructed, they are keyed with a new Guid. See [How to #1](#how-to-1) & [How to #2](#how-to-2) for examples.

You can key your InputFields manually by setting the InputField.Key property. If you do this and can guarantee that the Fields and their Keys will not change after a complete reconstruction of all objects, then you don't have to persist the objects across requests. See [How to #3](#how-to-3).

<hr>

FormProvider.cs provide demo data for showing how MvcDynamicForms works.

Detail information is in the FormProvider.cs comment.

<hr>

### How to #1 Passing Form through ModelBinding

In this Demo, the Form object graph is serialized to a string and stored in a hidden field in the page's HTML.

``` csharp
public ActionResult Demo1()
{
    var form = FormProvider.GetForm();

    // we are going to store the form and 
    // the field objects on the page across requests
    form.Serialize = true;

    return this.View("Demo", form);
}
```

Showing the form html
``` html
@model MvcDynamicForms.Core.Form

@using (Html.BeginForm(null, null, FormMethod.Post, new { enctype = "multipart/form-data" }))
{
    @Html.Raw(Model.RenderHtml(true))

    <input type="submit" value="Submit" />
}
```

### How to #2 Show the response

In this Demo, simply show how to echo the responses.

``` csharp
[HttpPost]
public ActionResult Demo1(Form form)
{
    // no need to retrieve the form object from anywhere
    // just use a parameter on the Action method that we are posting to

    if (form.Validate()) //input is valid
        return this.View("Responses", form);

    // input is not valid
    return this.View("Demo", form);
}
```

``` html
@model MvcDynamicForms.Core.Form

foreach (var response in Model.GetResponses(true))
{
    <tr>
        <td>
            @response.Title
        </td>
        <td>
            @response.Value
        </td>
    </tr>
}
```

### How to #3 Passing Form through TempData

In this Demo, the Form object graph is simply stored in TempData (short lived session state).

``` csharp
public ActionResult Demo2()
{
    var form = FormProvider.GetForm();

    // we are going to store the form 
    // in server memory across requests
    this.TempData["form"] = form;

    return this.View("Demo", form);
}

[HttpPost]
[ActionName("Demo2")]
public ActionResult Demo2Post()
{
    // we have to get the form object from
    // server memory and manually perform model binding
    var form = (Form)this.TempData["form"];
    this.UpdateModel(form);

    if (form.Validate()) // input is valid
        return this.View("Responses", form);

    // input is not valid
    this.TempData["form"] = form;
    return this.View("Demo", form);
}
```

### How to #4

In this Demo, the Form object graph is not persisted across requests. It is reconstructed on each request and the InputField's keys are set manually.

``` csharp
public ActionResult Demo3()
{
    // recreate the form and set the keys
    var form = FormProvider.GetForm();
    this.Demo3SetKeys(form);

    // set user input on recreated form
    this.UpdateModel(form);

    if (this.Request.HttpMethod == "POST" && form.Validate()) // input is valid
        return this.View("Responses", form);

    // input is not valid
    return this.View("Demo", form);
}

void Demo3SetKeys(Form form)
{
    int key = 1;
    foreach (var field in form.InputFields)
    {
        field.Key = key++.ToString();
    }
}
```

### How to #5 Custom Html Attribute

``` csharp
var attr = new Dictionary<string, string>();
attr.Add("class", "form-control");
attr.Add("placeholder", "Please Enter Name");

var name = new TextBox
{
    ResponseTitle = "Name",
    Prompt = "Enter your full name:",
    DisplayOrder = 20,
    Required = true,
    RequiredMessage = "Your full name is required",
    InputHtmlAttributes = attr
};
```

### Note
The serialization approach (demo 1) results in more concise code in the controller. Serializing the Form is also more reliable, in my opinion.

However, response time increases because of serialized data and the (de)serialization process takes time, as well. 

The approach you take depends on your needs.