using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinkodApp.Web;
using BinkodApp.Web.Models;
using BinkodApp.Web.Controllers;
using BinkodApp.Web.Helper;

namespace BinkodApp.Web.Controllers
{
    public partial class ServiceController : Controller
    {
        // GET: ServiceController_Partial
        public ActionResult Banks()
        {
            return View();
        }

        public ActionResult CaptureScreen()
        {
            try
            {
                CaptureScreenshot.FullScreenshot();
                return Json(new { success = true, message = "", JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                string _log = "http://boredsilly.in/Content/Logs/" + "Log_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
                return Json(new { success = false, message = ex.Message, url = _log, JsonRequestBehavior.AllowGet });
            }
        }


    }
}