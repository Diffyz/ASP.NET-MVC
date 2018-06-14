using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace IntexPress.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        //https://www.youtube.com/watch?v=oGeAYd3idBc
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Change(String LanguageAbbrevation)
        {
            if (LanguageAbbrevation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbrevation);
            }
            HttpCookie cookie = new HttpCookie("Language");           
            cookie.Value = LanguageAbbrevation;
            Response.Cookies.Add(cookie);
            return View("Index");
        }
    }
}