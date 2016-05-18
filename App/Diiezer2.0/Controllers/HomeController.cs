using Diiezer2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diiezer2._0.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult LesMieuxNotees()
        {
            DiiezerDBEntities db = new DiiezerDBEntities();
            var chansons = db.Chanson.ToList();
            chansons.Sort((x, y) => x.Note.CompareTo(y.Note));
            string user = User.Identity.Name;

            List<vmChansonInformation> vmChansonInformations = new List<vmChansonInformation>();
            foreach (var item in chansons)
            {
                string musique = item.Extrait; ;
                bool isExtract = true;

                if (db.Achat.Where(c => c.Chanson1.Id == item.Id && c.Utilisateur == user).ToList().Count() >= 1)
                {
                    musique = item.Musique;
                    isExtract = false;
                }

                vmChansonInformations.Add(
                    new vmChansonInformation
                    {
                        album = item.Album1.Titre,
                        isExtract = isExtract,
                        artiste = item.Album1.Artiste1.Nom,
                        durée = (int)item.Durée,
                        note = (int)item.Note,
                        titre = item.Titre,
                        idAlbum = item.Album1.Id,
                        idArtiste = item.Album1.Artiste1.Id,
                        idChanson = item.Id,
                        musique = musique,
                        prix = (double)item.Prix / 100.0
                    }
                    );
            }

            return View(vmChansonInformations);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}