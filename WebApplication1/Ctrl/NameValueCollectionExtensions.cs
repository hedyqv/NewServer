using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Specialized;

namespace WebApplication1.Ctrl
{
    public static class NameValueCollectionExtensions
    {
        public static object ToEntity(this NameValueCollection url, Type type)
        {
            var allKey = url.AllKeys;
            var ps = type.GetProperties();//获得公共成员
            object obj = Activator.CreateInstance(type); //创建类型对象
            //将数据放到对象中
            foreach (var item in ps)
            {
                if (allKey.Contains(item.Name))
                {
                    //把数据强转为对应的类型
                    item.SetValue(obj, Convert.ChangeType(url[item.Name], item.PropertyType));
                }
            }
            return obj;
        }
    }
}
