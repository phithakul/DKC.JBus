using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKC.JBus
{
    public class CustomModelBinder : DefaultModelBinder
    {
        // EmptyStringModelBinder
        public override object BindModel(ControllerContext controllerContext,
                                 ModelBindingContext bindingContext)
        {
            bindingContext.ModelMetadata.ConvertEmptyStringToNull = false;
            //Binders = new ModelBinderDictionary() { DefaultBinder = this }; // set when use the ModelBinderAttribute
            return base.BindModel(controllerContext, bindingContext);
        }

        // TrimModelBinder
        protected override void SetProperty(ControllerContext controllerContext,
          ModelBindingContext bindingContext,
          System.ComponentModel.PropertyDescriptor propertyDescriptor, object value)
        {
            if (propertyDescriptor.PropertyType == typeof(string))
            {
                var stringValue = (string)value;
                if (!string.IsNullOrEmpty(stringValue))
                    stringValue = stringValue.Trim();

                value = stringValue;
            }

            base.SetProperty(controllerContext, bindingContext,
                                propertyDescriptor, value);
        }
    }
}