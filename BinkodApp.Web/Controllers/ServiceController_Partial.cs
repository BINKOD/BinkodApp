using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinkodApp.Web;
using BinkodApp.Web.Models;
using BinkodApp.Web.Controllers;

namespace BinkodApp.Web.Controllers
{
    public partial class ServiceController : Controller
    {
        // GET: ServiceController_Partial
        public ActionResult Banks()
        {
            return View();
        }


    }
}