using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using WR.Modelo.Domain.Entities.Base;

namespace WR.Modelo.Api.Filters
{
    public class ProviderModelBinder : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(QueryFilter))
                return new BinderTypeModelBinder(typeof(QueryFilterModelBinder));

            return null;
        }
    }
}
