using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PriorityManager.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult ErrorMessage(string message)
        {
            ViewBag.ErrorMessage = message;
            return View("Error");
        }

    }
}
