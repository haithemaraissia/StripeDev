using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stripe;

namespace StripeFundamentals.Web.Controllers
{
    public class InvoicesController : Controller
    {
        // GET: Invoices
        public ActionResult Index()
        {


            //CUS ID For Testing
            //cus_75PYlPJEmsQb9X

            var customerList = new StripeCustomerService().List();
            StripeCustomer firstcustomer = customerList.First();
            //  var invoice = new StripeInvoice();

            StripeInvoice firstCusomterInvoice =
                new StripeInvoiceService().List().FirstOrDefault(x => x.CustomerId == firstcustomer.Id);


            return View(firstCusomterInvoice);
        }
    }
}