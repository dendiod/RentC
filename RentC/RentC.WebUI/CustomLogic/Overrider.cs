using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace RentC.WebUI.CustomLogic
{
    public class Overrider
    {
        internal void OverrideError(Controller controller, string key, string message)
        {
            var modelState = controller.ModelState[key];
            for (int i = 0; i < modelState.Errors.Count; ++i)
            {
                if (modelState.Errors[i].ErrorMessage.Contains("is not valid for"))
                {
                    modelState.Errors.RemoveAt(i);
                    modelState.Errors.Add(message);
                    break;
                }
            }
        }
    }
}