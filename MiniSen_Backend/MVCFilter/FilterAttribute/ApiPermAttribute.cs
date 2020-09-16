using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSen_Backend.MVCFilter.FilterAttribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ApiPermAttribute : Attribute
    {
        public static string autoDefaultPrefix = "Api/";
        public bool AutoCreate { get; set; }
        public string ApiUrl { get; set; }

        public ApiPermAttribute() 
        {
            this.AutoCreate = true;
        }

        public ApiPermAttribute(string apiUrl)
        {
            this.AutoCreate = false;
            this.ApiUrl = apiUrl;
        }
    }
}
