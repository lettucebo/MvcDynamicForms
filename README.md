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

### How to #1

In Demo 1, the Form object graph is serialized to a string and stored in a hidden field in the page's HTML.

### How to #2

In Demo 2, the Form object graph is simply stored in TempData (short lived session state).

### How to #3

In Demo 3, the Form object graph is not persisted across requests. It is reconstructed on each request and the InputField's keys are set manually.

### Note
The serialization approach (demo 1) results in more concise code in the controller. Serializing the Form is also more reliable, in my opinion.

However, response time increases because of serialized data and the (de)serialization process takes time, as well. 

The approach you take depends on your needs.