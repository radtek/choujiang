using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XGTech.BLL;
using XGTech.Config;

namespace XGTech.Web.Models
{

    public class MoviesRedirectRule : IRule
    {
        private readonly string[] _matchPaths;
        private readonly string _newPath;

        public MoviesRedirectRule(string[] matchPaths, string newPath)
        {
            _matchPaths = matchPaths;
            _newPath = newPath;
        }


        public void ApplyRule(RewriteContext context)
        {
            return;
            var request = context.HttpContext.Request;

              if (request.Path.Value.IndexOf("Css") >-1|| request.Path.Value.IndexOf("DownExcel") >-1||
                request.Path.Value.IndexOf("ExcelTemplate") > -1 || request.Path.Value.IndexOf("fileUpload") > -1||
                request.Path.Value.IndexOf("font") > -1 || request.Path.Value.IndexOf("images") > -1||
                request.Path.Value.IndexOf("js") > -1 || request.Path.Value.IndexOf("lay") > -1||
                request.Path.Value.IndexOf("lib") > -1 || request.Path.Value.IndexOf("Scripts") > -1||
                request.Path.Value.IndexOf("Export") > -1)
            {
                return;
            }


           
        }
    }
}
