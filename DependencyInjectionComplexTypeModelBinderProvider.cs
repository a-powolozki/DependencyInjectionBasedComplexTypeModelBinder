using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace De.Powolozki.AspNetCore.ModelBinding
{
    public class DependencyInjectionComplexTypeModelBinderProvider : IModelBinderProvider
    {
        IModelBinder IModelBinderProvider.GetBinder(ModelBinderProviderContext context)
        {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }


            if (context.Metadata.IsComplexType && !context.Metadata.IsCollectionType &&
                context.Services.GetService(context.Metadata.ModelType) != null &&
                ModelComesFromQuery(context))
            {

                var propertyBinders = context.Metadata.Properties
                    .Select(prop => new { Key = prop, Value = context.CreateBinder(prop) })
                    .ToDictionary(q => q.Key, q => q.Value);

                var loggerFactory = context.Services.GetService<ILoggerFactory>();
                return new DependencyInjectionComplexTypeModelBinder(propertyBinders, loggerFactory);
            }

            return null;
        }

        private static bool ModelComesFromQuery(ModelBinderProviderContext context)
        {
            var bindingSource = context.BindingInfo?.BindingSource;
            return bindingSource == null || bindingSource.CanAcceptDataFrom(BindingSource.Query);
        }
    }
}