﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Stripe;

namespace StripeFundamentals.Web.Controllers
{
    public class StripeWebhookController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            Stream request = Request.InputStream;
            request.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(request).ReadToEnd();
            StripeEvent stripeEvent = null;
            try
            {
                stripeEvent = StripeEventUtility.ParseEvent(json);


                ///// FOR PRODUCTION TO Confirm that strip send the Events///// FOR PRODUCTION
                //stripeEvent = VerifyEventSentFromStripe(stripeEvent);
              
                // if (HasEventBeenProcessedPreviously(stripeEvent)) { return new HttpStatusCodeResult(HttpStatusCode.OK); };
                ///// FOR PRODUCTION TO Confirm that strip send the Events///// FOR PRODUCTION
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, string.Format("Unable to parse incoming event.  The following error occurred: {0}", ex.Message));
            }

            if (stripeEvent == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Incoming event empty");

            var emailService = new Services.EmailService();
            switch (stripeEvent.Type)
            {
                case StripeEvents.ChargeRefunded:
                    var charge = Mapper<StripeCharge>.MapFromJson(stripeEvent.Data.Object.ToString());
                    emailService.SendRefundEmail(charge); 
                    break;
                case StripeEvents.InvoicePaymentSucceeded :
                  //  To generate email to send for confimration
                    var charge2 = Mapper<StripeCharge>.MapFromJson(stripeEvent.Data.Object.ToString());
                    emailService.SendPaymentReceivedFromInvoicePaymentSucceed(charge2);
                    break;


                //case StripeEvents.CustomerSubscriptionUpdated:
                //    break;

                //                    case StripeEvents.CustomerSubscriptionDeleted:
                //    break;

                //                    case StripeEvents.CustomerSubscriptionCreated:
                //    break;
                    
                //default:
                //    break;
            }

            //TODO: log Stripe eventid to StripeEvent table in application database
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private bool HasEventBeenProcessedPreviously(StripeEvent stripeEvent)
        {
            //lookup in your database's StripeEventLog by  eventid
            //if eventid exists return true
            return false;

        }

        private static StripeEvent VerifyEventSentFromStripe(StripeEvent stripeEvent)
        {
            var eventService = new StripeEventService(ConfigurationManager.AppSettings["stripeSecretKey"]);
            stripeEvent = eventService.Get(stripeEvent.Id);
            return stripeEvent;
        }


       
    }
}