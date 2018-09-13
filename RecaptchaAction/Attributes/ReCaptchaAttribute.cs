using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecaptchaAction.Models;

namespace RecaptchaAction.Attributes
{
    public class ReCaptchaAttribute : ActionFilterAttribute
    {
        private readonly string _secret = "6LeleW8UAAAAAD7gvOgRCLHOUmbebvpHGn4TpAey";
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var recaptchaResponse = httpContext.Request.Form["recaptcha_response"];
            var recaptchaAction = httpContext.Request.Form["recaptcha_action"];

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"https://www.google.com/recaptcha/api/siteverify");
                var response = client.GetStringAsync($"?response={recaptchaResponse}&secret{_secret}=&action={recaptchaAction}").Result;
                var result = JsonConvert.DeserializeObject<RecaptchaResponse>(response);
                if (result.Success == false || result.Score < float.Parse("0,1"))
                {
                    ((Controller)context.Controller).ModelState.AddModelError("ReCaptcha", "Error");
                    context.Result = ((Controller)context.Controller).BadRequest("ReCaptcha");
                }
            }
        }


    }
}
