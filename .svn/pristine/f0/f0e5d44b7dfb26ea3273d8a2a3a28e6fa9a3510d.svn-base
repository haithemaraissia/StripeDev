using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stripe;

namespace StripeFundamentals.Web.Controllers
{
    public class InitPlansController : Controller
    {
        // GET: InitPlans
        public ActionResult Index()
        {

            var myFreePlan = new StripePlanCreateOptions();
            myFreePlan.Id = "free_plan";
            myFreePlan.Amount = 0;           // all amounts on Stripe are in cents, pence, etc
            myFreePlan.Currency = "usd";        // "usd" only supported right now
            myFreePlan.Interval = "month";      // "month" or "year"
            myFreePlan.IntervalCount = 1;       // optional
            myFreePlan.Name = "Basic Plan";
            myFreePlan.TrialPeriodDays = 1;    // amount of time that will lapse before the customer is billed

            var myBasicPlan = new StripePlanCreateOptions();
            myBasicPlan.Id = "basic_plan";
            myBasicPlan.Amount = 0;           // all amounts on Stripe are in cents, pence, etc
            myBasicPlan.Currency = "usd";        // "usd" only supported right now
            myBasicPlan.Interval = "month";      // "month" or "year"
            myBasicPlan.IntervalCount = 1;       // optional
            myBasicPlan.Name = "Basic Plan";
            myBasicPlan.TrialPeriodDays = 1;    // amount of time that will lapse before the customer is billed

            var myProfessionalPlan = new StripePlanCreateOptions();
            myProfessionalPlan.Id = "pro_plan";
            myProfessionalPlan.Amount = 999;           // all amounts on Stripe are in cents, pence, etc
            myProfessionalPlan.Currency = "usd";        // "usd" only supported right now
            myProfessionalPlan.Interval = "month";      // "month" or "year"
            myProfessionalPlan.IntervalCount = 1;       // optional
            myProfessionalPlan.Name = "Professional Plan";
            myProfessionalPlan.TrialPeriodDays = 1;    // amount of time that will lapse before the customer is billed

            var myBuinessPlan = new StripePlanCreateOptions();
            myBuinessPlan.Id = "business_plan";
            myBuinessPlan.Amount = 1999;           // all amounts on Stripe are in cents, pence, etc
            myBuinessPlan.Currency = "usd";        // "usd" only supported right now
            myBuinessPlan.Interval = "month";      // "month" or "year"
            myBuinessPlan.IntervalCount = 1;       // optional
            myBuinessPlan.Name = "Business Plan";
            myBuinessPlan.TrialPeriodDays = 1;    // amount of time that will lapse before the customer is billed

            var planService = new StripePlanService();
            StripePlan response = planService.Create(myFreePlan);
            StripePlan response2 = planService.Create(myBasicPlan);
            StripePlan response3 = planService.Create(myProfessionalPlan);
            StripePlan response4 = planService.Create(myBuinessPlan);


        CreateCoupon();


            return View();
        }

        private void CreateCoupon()
        {
            var myHolidayCoupon = new StripeCouponCreateOptions();
            myHolidayCoupon.Id = "HOLIDAY10OFF";
            myHolidayCoupon.PercentOff = 10;
            myHolidayCoupon.Duration = "repeating"; // "forever", "once", or "repeating"
            myHolidayCoupon.DurationInMonths = 3; // valid when "repeating" only
// set these if you want to
            myHolidayCoupon.MaxRedemptions = 100;
            myHolidayCoupon.RedeemBy = new DateTime(2015, 12, 10);
            var couponService = new StripeCouponService();
            StripeCoupon response5 = couponService.Create(myHolidayCoupon);


            var my25DollarCoupon = new StripeCouponCreateOptions();
            my25DollarCoupon.Id = "25DollarsOFF";
            my25DollarCoupon.AmountOff = 2500;
            my25DollarCoupon.Currency = "usd";
            my25DollarCoupon.Duration = "repeating"; // "forever", "once", or "repeating"
            my25DollarCoupon.DurationInMonths = 3; // valid when "repeating" only
// set these if you want to
            my25DollarCoupon.MaxRedemptions = 100;
            my25DollarCoupon.RedeemBy = new DateTime(2015, 12, 10);
            var couponService2 = new StripeCouponService();
            StripeCoupon response6 = couponService.Create(my25DollarCoupon);
        }
    }
}