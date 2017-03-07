using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using Bogus;
using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Converters;

namespace NextPage.SupportSync
{
    public class Zapier
    {
        Faker faker = new Faker();

        public class CustomerField
        {
            public int CustomerId { get; set; }
            public string FieldName { get; set; }
            public string FieldValue { get; set; }
        }

        public class Customer
        {
            public string CustomerFullName { get; set; }
            public string CustomerOrganization { get; set; }
            public string CustomerRecipient { get; set; }
            public string CustomerAddress { get; set; }
            public string CustomerCity { get; set; }
            public string CustomerState { get; set; }
            public string CustomerZip { get; set; }
            public string CustomerCountry { get; set; }
            public string CustomerPhone { get; set; }
            public string CustomerPhoneExt { get; set; }
            public string CustomerEmail { get; set; }
            public int CustomerId { get; set; }
            public bool CustomerAddressCheckOff { get; set; }
            //public int CustomerFieldTokenNo { get; set; }
            //public string CustomerFieldParentName { get; set; }
            //public DateTime CustomerFieldLastActiveDate { get; set; }
            public List<CustomerField> CustomerFieldList { get; set; }
        }


        public Customer GetCustomer()
        {
            var cust = Customers[0];
            return cust;
        }

        public ExpandoObject GetCustomersFlatJson()
        {
            var cust = Customers[0];
            var expConverter = new ExpandoObjectConverter();
            ExpandoObject mycust = JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(cust), expConverter);

            foreach (var field in cust.CustomerFieldList)
            {
                AddProperty(mycust, field.FieldName, field.FieldValue);
            }

            RemoveProperty(mycust, "CustomerFieldList");

            return mycust;
        }

        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        public static void RemoveProperty(ExpandoObject expando, string propertyName)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            expandoDict.Remove(propertyName);
        }

        List<Customer> Customers = new List<Customer>();

        public void LoadCustomers()
        {
            var testCustomers = new Faker<Customer>()
                        .RuleFor(u => u.CustomerFullName, f => f.Name.FirstName() + " " + f.Name.LastName())
                        .RuleFor(u => u.CustomerOrganization, f => f.Company.CompanyName())
                        .RuleFor(u => u.CustomerRecipient, f => f.Name.FirstName())
                        .RuleFor(u => u.CustomerAddress, f => f.Address.StreetAddress(true))//todo confirm
                        .RuleFor(u => u.CustomerCity, f => f.Address.City())
                        .RuleFor(u => u.CustomerState, f => f.Address.State())
                        .RuleFor(u => u.CustomerZip, f => f.Address.ZipCode())
                        .RuleFor(u => u.CustomerCountry, f => f.Address.Country())
                        .RuleFor(u => u.CustomerPhone, f => f.Phone.PhoneNumber())
                        .RuleFor(u => u.CustomerPhoneExt, f => f.Random.Number(1000, 2000).ToString())
                        .RuleFor(u => u.CustomerEmail, f => f.Internet.Email())
                        .RuleFor(u => u.CustomerId, f => f.Random.Number(100, 1000))
                        .RuleFor(u => u.CustomerAddressCheckOff, f => f.Random.Bool());
            Customers = testCustomers.Generate(10).ToList();

            var fieldNames = new[] { "ParentName", "OrderAmount" };

            foreach (var cust in Customers)
            {
                cust.CustomerFieldList = new List<CustomerField>();
                foreach (var name in fieldNames)
                {
                    var field = new CustomerField();
                    field.FieldName = name;
                    field.FieldValue = faker.Name.FirstName();
                    field.CustomerId = cust.CustomerId;
                    cust.CustomerFieldList.Add(field);
                }
            }
        }
    }
}