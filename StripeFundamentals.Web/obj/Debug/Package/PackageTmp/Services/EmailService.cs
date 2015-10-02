using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;


namespace StripeFundamentals.Web.Services
{
    public class EmailService
    {
        public void SendRefundEmail(StripeCharge stripeCharge)
        {
            var message = new MailMessage() { IsBodyHtml = true };
            message.To.Add(new MailAddress("haraissia@sosland.com"));
            message.Subject = string.Format("refund requested on charge: {0}", stripeCharge.Id);
            message.Body = string.Format("<p>{0}</p>", string.Format("A customer at this email address {0} was issued a refund on their purchase: '{1}'.  Please follow up to determine a reason.", stripeCharge.ReceiptEmail, stripeCharge.Description));

            using (var smtp = new SmtpClient())
            {
                smtp.Send(message);
            }

        }


        public void SendPaymentReceivedFromChargeEmail(StripeCharge stripeCharge)
        {

            var customer = stripeCharge.Customer;
            var message = new MailMessage() { IsBodyHtml = true };
            message.To.Add(new MailAddress("haraissia@sosland.com"));
            message.Subject = string.Format("Receipt for purchasing  {0} - {1}", stripeCharge.Description, stripeCharge.Id);
            message.Body = string.Format("<p>{0}</p>", string.Format("A customer at this email address {0} confirmation of this purchase: '{1}'.  Please follow up to determine a reason.", stripeCharge.ReceiptEmail, stripeCharge.Description));

            using (var smtp = new SmtpClient())
            {
                smtp.Send(message);
            }

        }

    }
}