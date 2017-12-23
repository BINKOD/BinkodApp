using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinkodApp.Core;

namespace BinkodApp.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {              
                ViewBag.ip = Utils.GetIPAddress();
            }
            catch (Exception ex) { Common.ExceptionLog(ex.Message); }

            return View();
        }
        public ActionResult Log()
        {
            string logURL = "http://boredsilly.in/Content/Logs/";
            bool _success = true;
            try
            {
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                logURL = baseUrl.Contains("localhost") ? logURL : baseUrl + "Content/Logs/";
                if (baseUrl.Contains("localhost")) _success = false;
                Common.ExceptionLog("");
            }
            catch (Exception ex) { Common.ExceptionLog(ex.Message); }

            logURL = logURL + "Log_" + Utils.DateFormatForFilename() + ".txt";
            return Json(new { success = _success, message = logURL, JsonRequestBehavior.AllowGet });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}