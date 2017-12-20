using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinkodApp.Core;

namespace BinkodApp.Web.Controllers
{
    public partial class ServiceController : Controller
    {
        // GET: Service
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConvertNumbers(string number = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(number))
                {
                    if (number.Contains("."))
                    {
                        Decimal _number = Convert.ToDecimal(number);
                        number = Utils.DecimalToWords(_number);
                    }
                    else
                    {
                        Int64 _number = Convert.ToInt64(number);
                        number = Utils.NumberToWords(_number);
                    }

                    return Json(new { success = true, data = number, JsonRequestBehavior.AllowGet });
                }
                else
                    return Json(new { success = false, data = "Please enter a valid number.", JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = "Please enter a valid number.", JsonRequestBehavior.AllowGet });
            }
        }
    }
}