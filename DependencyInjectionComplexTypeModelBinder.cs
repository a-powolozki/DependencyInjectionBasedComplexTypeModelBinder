using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace De.Powolozki.AspNetCore.ModelBinding
{
    public class DependencyInjectionComplexTypeModelBinder : ComplexTypeModelBinder
    {
        public DependencyInjectionComplexTypeModelBinder(
            IDictionary<ModelMetadata, IModelBinder> propertyBinders,
            ILoggerFactory loggerFactory) : base(propertyBinders, loggerFactory) { }

        protected override object CreateModel(ModelBindingContext bindingContext)
        {
            var modelFromService = bindingContext?.HttpContext?.RequestServices?.GetService(bindingContext.ModelType);

            return modelFromService ?? base.CreateModel(bindingContext);
        }
    }
}