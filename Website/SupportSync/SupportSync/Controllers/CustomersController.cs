using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SupportSync.Controllers
{
    public class CustomersController : Controller
    {
        public class ExpandoJSONConverter : JavaScriptConverter
        {
            public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
            {
                throw new NotImplementedException();
            }
            public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
            {
                var result = new Dictionary<string, object>();
                var dictionary = obj as IDictionary<string, object>;
                foreach (var item in dictionary)
                    result.Add(item.Key, item.Value);
                return result;
            }
            public override IEnumerable<Type> SupportedTypes
            {
                get
                {
                    return new ReadOnlyCollection<Type>(new Type[] { typeof(System.Dynamic.ExpandoObject) });
                }
            }
        }

        public string CustomersFlatJson()
        {
            var obj = MvcApplication.zap.GetCustomersFlatJson();

            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });
            var json = serializer.Serialize(obj);

            return json;
        }

        public JsonResult Customer()
        {
            var result = MvcApplication.zap.GetCustomer();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}