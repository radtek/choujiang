using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;


namespace XGTech.Web.Infrastructure
{
    public static class UrlHelperExtensions
    {
        public static string ContentVersioned(this IUrlHelper self, string contentPath)
        {
            string versionContentPath = contentPath + "?v=" + Assembly.GetAssembly(typeof(UrlHelperExtensions)).GetName().Version.ToString() + "." + new Random().Next(1000, 9999);
            return self.Content(versionContentPath);
        }
    }
}
