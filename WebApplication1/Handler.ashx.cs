using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Collections.Specialized;
using WebApplication1.Ctrl;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace WebApplication1
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");

            string path = HttpUtility.UrlDecode(context.Request.Path.ToString());

            var paramter = path.Split('/');

            Assembly assem = Assembly.GetExecutingAssembly();
            var type = assem.GetType("WebApplication1.Ctrl." + paramter[2], true, true);


            var obj = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod(paramter[3], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            //var getData = methodInfo.Invoke(obj, null);
            object getData = null;

            //get请求
            if (request.HttpMethod == "GET") {
                if (methodInfo.GetParameters().Length > 0)
                {

                    if (methodInfo.GetParameters()[0].ParameterType == typeof(String))
                    {
                        var reqString = request.QueryString.AllKeys[0];

                        getData = methodInfo.Invoke(obj, new object[] { reqString });
                    }
                    else
                    {
                        var modelData = request.QueryString.ToEntity(methodInfo.GetParameters()[0].ParameterType);
                        getData = methodInfo.Invoke(obj, new object[] { modelData });
                    }
                }
                else
                {
                    getData = methodInfo.Invoke(obj, null);
                }
            }
            

            //post请求
            if (request.HttpMethod == "POST") {
                string documentContents;
                using (Stream receiveStream = request.InputStream)
                {
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        documentContents = readStream.ReadToEnd();
                    }
                }

                if (methodInfo.GetParameters()[0].ParameterType == typeof(String))
                {
                    getData = methodInfo.Invoke(obj, new object[] { documentContents });
                }
                else { 
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    var  routes_list = json_serializer.DeserializeObject(documentContents);
                    getData = methodInfo.Invoke(obj, new object[] { routes_list });
                }
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