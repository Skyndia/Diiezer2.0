using Diiezer2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diiezer2._0.Controllers
{
    public class ResearchController : Controller
    {
        private DiiezerDBEntities db = new DiiezerDBEntities();

        // GET: Research
        public ActionResult Begin()
        {


            return View();
        }

        //POST : Search
        [HttpPost]
        public ActionResult Search(FormCollection criteres)
        {
            String vue = criteres["but"]; //Vue à renvoyer
            if (vue.CompareTo("SearchArtiste") == 0) //on doit renvoyer une liste de vmArtisteCover
            {
                List<vmArtisteCover> vmList = new List<vmArtisteCover>();
                List<Artiste> artistes;

                String champArtiste = criteres["artiste"];
                artistes = db.Artiste.Where(a => a.Nom.Contains(champArtiste)).ToList();

                foreach (var art in artistes)
                {
                    vmArtisteCover artisteInfo = new vmArtisteCover
                    {
                        id = art.Id,
                        nom = art.Nom,
                        cover = db.Album.Where(a => a.Artiste1.Id == art.Id).FirstOrDefault().Cover
                    };

                    vmList.Add(artisteInfo);
                }
                vmList.Sort((x, y) => x.nom.CompareTo(y.nom));
                return View(vue, vmList);
            }

            return View(vue);
        }
    }
}
