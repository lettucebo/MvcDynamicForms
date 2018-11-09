using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MvcDynamicForms.NetCore
{
    public class DynamicFormModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (context.Metadata.ModelType == typeof(Form))
            {
                return new FormModelBinder();
            }
            return null;
        }
    }
}
