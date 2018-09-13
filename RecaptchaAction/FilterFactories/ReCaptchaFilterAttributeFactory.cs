using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using RecaptchaAction.Attributes;
using System;

namespace RecaptchaAction.FilterFactories
{
    public class ReCaptchaFilterAttributeFactory : IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var config = serviceProvider.GetService(typeof(IConfiguration)) as IConfiguration;
            var attribute = new ReCaptchaFilterAttribute();
            attribute.Config = config;
            return attribute;

        }
    }
}
