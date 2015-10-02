using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StripeFundamentals.Web.Controllers
{
    public class WebHookTestController : Controller
    {
        [HttpPost]
        public ActionResult Webhook()
        {
            var json = "";
            using (var inputStream = new System.IO.StreamReader(Request.InputStream))
            {
                json = inputStream.ReadToEnd();
            }

            return Json(new { message = "success!" });
        }
    }
}