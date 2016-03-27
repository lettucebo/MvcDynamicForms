using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace Creatidea.Library.Web.DynamicForms.Core
{
    public static class SerializationUtility
    {
        public static string Serialize(object obj)
        {
            StringWriter writer = new StringWriter();
            new LosFormatter().Serialize(writer, obj);
            return writer.ToString();
        }
        public static T Deserialize<T>(string data)
        {
            if (data == null) return default(T);
            return (T)(new LosFormatter()).Deserialize(data);
        }
        /// <summary>
        /// Creates a JSON graph of all of the field's client-side data.
        /// </summary>
        public static string ToJson(this Dictionary<string, Dictionary<string, DataItem>> dict)
        {
            var main = new Dictionary<string, Dictionary<string, object>>();
            foreach (var item in dict)
            {                
                var temp = new Dictionary<string, object>();
                foreach (var item2 in item.Value.Where(x => x.Value.ClientSide))
                    temp.Add(item2.Key, item2.Value.Value);

                main.Add(item.Key, temp);
            }
            var json = new JavaScriptSerializer();
            return json.Serialize(main);
        }
    }
}