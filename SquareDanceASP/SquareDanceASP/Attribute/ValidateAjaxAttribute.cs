using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SquareDanceASP.Attribute
{
    public class ValidateAjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                return;

            var modelState = filterContext.Controller.ViewData.ModelState;
            if (!modelState.IsValid)
            {
                var errorModel = new List<string>();
                //var errorModel =
                //        from x in modelState.Keys
                //        where modelState[x].Errors.Count > 0
                //        select new
                //        {
                //            key = x,
                //            Errors = modelState[x].Errors.
                //                                          Select(y => y.ErrorMessage).
                //                                          ToArray()
                //        };
                foreach (var key in modelState.Keys)
                {
                    var errors = modelState[key].Errors.Select(x => x.ErrorMessage).ToArray();
                    foreach (var error in errors)
                    {
                        errorModel.Add(error);
                    }
                }

                filterContext.Result = new JsonResult()
                {
                    Data = errorModel
                };
                filterContext.HttpContext.Response.StatusCode =
                                                      (int)HttpStatusCode.BadRequest;
            }
        }
    }
}