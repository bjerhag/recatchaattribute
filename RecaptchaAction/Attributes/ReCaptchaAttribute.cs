using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecaptchaAction.Models;
using Microsoft.Extensions.Configuration;

namespace RecaptchaAction.Attributes
{
    public class ReCaptchaFilterAttribute : ActionFilterAttribute
    {
        internal IConfiguration Config { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var secret = Config.GetSection("Recaptcha:Secret").Value;
                var scoreLimit = float.Parse(Config.GetSection("Recaptcha:ScoreLimit")?.Value ?? "0");
                if (string.IsNullOrEmpty(secret))
                {
                    Console.WriteLine("No recaptcha secret");
                    return;
                }

                var httpContext = context.HttpContext;
                var recaptchaResponse = httpContext.Request.Form["recaptcha_response"];
                var recaptchaAction = httpContext.Request.Form["recaptcha_action"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(@"https://www.google.com/recaptcha/api/siteverify");

                    var response = client.GetStringAsync($"?response={recaptchaResponse}&secret{secret}=&action={recaptchaAction}").Result;
                    var result = JsonConvert.DeserializeObject<RecaptchaResponse>(response);

                    if (result.Success == false || result.Score < float.Parse("0,1"))
                    {
                        ((Controller)context.Controller).ModelState.AddModelError("ReCaptcha", "Error");
                        context.Result = ((Controller)context.Controller).BadRequest("ReCaptcha Error");
                        Console.WriteLine(string.Join(',', result.ErrorCodes));
                    }
                }
            }
            catch (Exception e)
            {
                ((Controller)context.Controller).ModelState.AddModelError("ReCaptcha", "Error");
                context.Result = ((Controller)context.Controller).BadRequest("ReCaptcha Error");
                Console.WriteLine(e.Message);
            }
        }
    }
}
