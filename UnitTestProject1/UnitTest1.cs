using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextPage.SupportSync;
namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                var zap = new Zapier();
                zap.LoadCustomers();
                var json = zap.GetCustomerFlat();
                Console.Write(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
