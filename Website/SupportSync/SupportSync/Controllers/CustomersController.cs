using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NextPage.SupportSync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
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

        [HttpPost]
        public NextPage.SupportSync.Zapier.Customer AddEditCustomer()
        {
            Request.InputStream.Seek(0, SeekOrigin.Begin);
            string jsonObj = new StreamReader(Request.InputStream).ReadToEnd();

            var customer = JsonConvert.DeserializeObject<NextPage.SupportSync.Zapier.Customer>(jsonObj.ToString());
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonObj.ToString());

            var fields = new List<NextPage.SupportSync.Zapier.CustomerField>();
            foreach (var val in values)
            {
                if (!PropertyUtils.Exists(val.Key, customer))
                {
                    fields.Add(new NextPage.SupportSync.Zapier.CustomerField { CustomerId = customer.CustomerId, FieldName = val.Key, FieldValue = val.Value });
                }
            }
            customer.CustomerFieldList = fields;
            MvcApplication.zap.AddEditCustomer(customer);
            return customer;
        }

        public string GetCustomer(int customerId)
        {
            var obj = MvcApplication.zap.GetCustomersFlatJson(customerId);

            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });
            var json = serializer.Serialize(obj);

            Response.ContentType = "application/json";
            return json;
        }

        public JsonResult GetCustomers()
        {
            return Json(MvcApplication.zap.Customers, JsonRequestBehavior.AllowGet);
        }

        //public string CustomersFlatJson()
        //{
        //    var obj = MvcApplication.zap.GetCustomersFlatJson();

        //    var serializer = new JavaScriptSerializer();
        //    serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });
        //    var json = serializer.Serialize(obj);


        //    return json;
        //}

        //public JsonResult Customer()
        //{
        //    var result = MvcApplication.zap.GetCustomer();
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
    }
}