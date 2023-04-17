using System;
using System.Web.Mvc;
using System.Globalization;

namespace ProjetoTransportadora.ModelBinder
{
    public class DoubleModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;

            try
            {
                actualValue = Convert.ToDouble(valueResult.AttemptedValue, CultureInfo.CurrentCulture);
            }
            catch (FormatException ex)
            {
                modelState.Errors.Add(ex);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);

            return actualValue;
        }
    }
}