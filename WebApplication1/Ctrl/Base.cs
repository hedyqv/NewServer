using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.DataContract;

namespace WebApplication1.Ctrl
{
    public class Base
    {
        public string Show()
        {
            return "This is Base.Show";
        }

        public string DoByString(string req)
        {
            return string.Format("do by string: {0}", req);
        }

        public string DoByModel(DoByModelRequest req)
        {
            return string.Format("do by model: {0} - {1}", req.Name, req.Age);
        }
    }
}