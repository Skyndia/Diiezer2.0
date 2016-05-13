using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diiezer2._0.Controllers
{
    public class ResearchController : Controller
    {
        // GET: Research
        public ActionResult Begin()
        {


            return View();
        }

        //POST : Search
        [HttpPost]
        public ActionResult Search(FormCollection criteres)
        {
            String vue = criteres["but"];
            return View(vue);
        }
    }
}