using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Stripe;
using Stripe.Infrastructure;

namespace StripeFundamentals.Web.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {

            var customerList = new StripeCustomerService().List();
            StripeCustomer firstcustomer = customerList.First();
            //  var invoice = new StripeInvoice();

            StripeInvoice firstCusomterInvoice =
                new StripeInvoiceService().List().FirstOrDefault(x => x.CustomerId == firstcustomer.Id);


            return View(firstCusomterInvoice);

        }



        public StripeCustomer GetAll()
        {
            var url = "https://api.stripe.com/v1/customers";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization",
                GetAuthorizationHeaderValue(ConfigurationManager.AppSettings["stripeSecretKey"]));
            request.Headers.Add("Stripe-Version", StripeConfiguration.ApiVersion);
            request.Method = "GET";
            var response = ExecuteWebRequest(request);


            //Check return type
            return Mapper<StripeCustomer>.MapFromJson(response);

        }
        /// <summary>
        /// For Calling The API Directly
        /// </summary>
        /// <param name="webRequest"></param>
        /// <returns></returns>
        private static string ExecuteWebRequest(WebRequest webRequest)
        {
            try
            {
                using (var response = webRequest.GetResponse())
                {
                    return ReadStream(response.GetResponseStream().ToString());
                }
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                {
                    var statusCode = ((HttpWebResponse)webException.Response).StatusCode;

                    var stripeError = new StripeError();

                    if (webRequest.RequestUri.ToString().Contains("oauth"))
                        stripeError = Mapper<StripeError>.MapFromJson(ReadStream(webException.Response.GetResponseStream().ToString()));
                    else
                        stripeError = Mapper<StripeError>.MapFromJson(ReadStream(webException.Response.GetResponseStream().ToString()), "error");

                    throw new StripeException(statusCode, stripeError, stripeError.Message);
                }

                throw;
            }
        }

        private static string ReadStream(string stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private static string GetAuthorizationHeaderValue(string apiKey)
        {
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:", apiKey)));
            return string.Format("Basic {0}", token);
        }

        private static string GetAuthorizationHeaderValueBearer(string apiKey)
        {
            return string.Format("Bearer {0}", apiKey);
        }

    }
}