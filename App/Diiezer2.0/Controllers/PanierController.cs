using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diiezer2._0.Controllers
{
    public class PanierController : Controller
    {
        // GET: Panier
        public ActionResult MonPanier()
        {
            return View();
        }
    }
}