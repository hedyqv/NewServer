using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Collections.Specialized;
using WebApplication1.Ctrl;
using System.Collections.Specialized;

namespace WebApplication1
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");

            string path = HttpUtility.UrlDecode(context.Request.Path.ToString());

            var paramter = path.Split('/');

            Assembly assem = Assembly.GetExecutingAssembly();
            var type = assem.GetType("WebApplication1.Ctrl." + paramter[2], true, true);


            var obj = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod(paramter[3], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            //var getData = methodInfo.Invoke(obj, null);
            object getData;

            if (methodInfo.GetParameters().Length > 0) {
                var modelData = context.Request.QueryString.ToEntity(methodInfo.GetParameters()[0].ParameterType);
                getData = methodInfo.Invoke(obj,new object[]{modelData});
            }else{
                getData = methodInfo.Invoke(obj,null);
            }

            context.Response.Write(getData);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}