using StripeFundamentals.Models.Book;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stripe;
using StripeFundamentals.Web.Services;


namespace StripeFundamentals.Controllers
{
    public class BookController : Controller
    {
        // embedded form
        public ActionResult Index()
        {
            string stripePublishableKey = ConfigurationManager.AppSettings["stripePublishableKey"];
            var model = new IndexViewModel() { StripePublishableKey = stripePublishableKey };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Charge(ChargeViewModel chargeViewModel)
        {
            Debug.WriteLine(chargeViewModel.StripeEmail);
            Debug.WriteLine(chargeViewModel.StripeToken);
            return RedirectToAction("Confirmation");
        }

        public ActionResult Custom()
        {
            string stripePublishableKey = ConfigurationManager.AppSettings["stripePublishableKey"];
            var model = new CustomViewModel() { StripePublishableKey = stripePublishableKey, PaymentFormHidden = true };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Custom(CustomViewModel customViewModel)
        {
            customViewModel.PaymentFormHidden = false;
            var chargeOptions = new StripeChargeCreateOptions()
            {
                //required
                Amount = 3900,
                Currency = "usd",
                Source = new StripeSourceOptions() { TokenId = customViewModel.StripeToken },
                //optional
                Description = string.Format("JavaScript Framework Guide Ebook for {0}", customViewModel.StripeEmail),
                ReceiptEmail = customViewModel.StripeEmail
            };


            var chargeService = new StripeChargeService();

            try
            {
                StripeCharge stripeCharge = chargeService.Create(chargeOptions);
                
                //Validate charge
                chargeService.Capture(stripeCharge.Id);

                if (stripeCharge.Status == "succeeded")
                {
                    //creating the customer and add it to stripe
                    var newCustomer = new StripeCustomerService().Create(
                        new StripeCustomerCreateOptions
                        {
                            Email = customViewModel.StripeEmail
                        }
                        );
                    // CREATE NEW INVOICE
                    // var invoiceService = new StripeInvoiceService();
                    // StripeInvoice response = invoiceService.Create(newCustomer.Id); // optional StripeInvoiceCreateOptions
                    //var response = invoiceService.Upcoming(newCustomer.Id);
                    // stripeCharge.InvoiceId = response.Id;


                    //SEND THE CONFIRMATION
                    var emailService = new EmailService();
                    emailService.SendPaymentReceivedFromChargeEmail(stripeCharge);

                }
            }
            catch (StripeException stripeException)
            {
                Debug.WriteLine(stripeException.Message);
                ModelState.AddModelError(string.Empty, stripeException.Message);
                return View(customViewModel);
            }

            return RedirectToAction("Confirmation");

        }



        public ActionResult Confirmation()
        {
            return View();
        }


    }
}