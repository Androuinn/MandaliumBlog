using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Mandalium.API.Filters
{
    public class CultureFilterAttribute : ActionFilterAttribute
    {


        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            //if (context.HttpContext.Request != null)
            //{
            //    string language= context.HttpContext.Request.Headers["lang"];

            //    try
            //    {
            //        CultureInfo.CurrentUICulture = new CultureInfo(language);
            //    }
            //    catch
            //    {
            //        CultureInfo.CurrentUICulture = new CultureInfo("tr-TR");
            //    }
            //}

            return base.OnActionExecutionAsync(context, next);
        }

    }
}
