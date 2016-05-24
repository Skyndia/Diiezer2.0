using Diiezer2._0.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Diiezer2._0.Models
{
    public class ChansonController : Controller
    {
        private DiiezerDBEntities db = new DiiezerDBEntities();

        // GET: Chanson/Index/5
        public ActionResult Index(string tri)
        {
            List<vmChansonInformation> vmChansonInformations =  this.getvmChansonInformations();

            //Si requete Ajax
            if (Request.IsAjaxRequest())
            {
                if (tri == "note")
                {
                    vmChansonInformations.Sort((x, y) => x.note.CompareTo(y.note));
                    vmChansonInformations.Reverse();
                }
                else if (tri == "album")
                {
                    vmChansonInformations.Sort((x, y) => x.album.CompareTo(y.album));
                }
                else if (tri == "artiste")
                {
                    vmChansonInformations.Sort((x, y) => x.artiste.CompareTo(y.artiste));
                }
                else if (tri == "duree")
                {
                    vmChansonInformations.Sort((x, y) => x.durée.CompareTo(y.durée));
                    vmChansonInformations.Reverse();
                }
                else if (tri == "prix")
                {
                    vmChansonInformations.Sort((x, y) => x.prix.CompareTo(y.prix));
                }
                else
                {
                    vmChansonInformations.Sort((x, y) => x.titre.CompareTo(y.titre));
                }
                return PartialView("IndexPartial", vmChansonInformations);
            }
            else
            {
                vmChansonInformations.Sort((x, y) => x.titre.CompareTo(y.titre));
            }
            return View(vmChansonInformations);
        }

        public List<vmChansonInformation> getvmChansonInformations()
        {
            
            List<vmChansonInformation> vmChansonInformations = new List<vmChansonInformation>();

            var chansons = db.Chanson.Include(c => c.Album1).ToList();
            foreach (var item in chansons)
            {
                string musique = item.Extrait;
                bool isExtract = true;
                if (User.Identity.IsAuthenticated)
                {
                    string user = User.Identity.Name;
                    if (db.Achat.Where(c => c.Chanson1.Id == item.Id && c.Utilisateur == user).ToList().Count() >= 1)
                    {
                        musique = item.Musique;
                        isExtract = false;
                    }
                }

                //Je veux arrondir chanson.note---------
                double partieDecimale = item.Note - Math.Floor(item.Note);
                int noteArrondie = (int)(Math.Floor(item.Note));
                if (partieDecimale > 0.5) noteArrondie += 1;

                //La chanson est elle dans le panier de l'utilisateur ?
                var panier = db.Panier.Where(p => p.Utilisateur == User.Identity.Name && p.Object == item.Id).ToList();
                bool inPanier = false;
                if (panier.Count > 0) { inPanier = true; }

                vmChansonInformations.Add(new vmChansonInformation
                {
                    album = item.Album1.Titre,
                    isExtract = isExtract,
                    artiste = item.Album1.Artiste1.Nom,
                    durée = (int)item.Durée,
                    note = noteArrondie,
                    titre = item.Titre,
                    idAlbum = item.Album1.Id,
                    idArtiste = item.Album1.Artiste1.Id,
                    idChanson = item.Id,
                    musique = musique,
                    prix = (double)item.Prix / 100.0,
                    isInPanier = inPanier
                });
            }    
            return vmChansonInformations;
        }

        // GET: Chanson/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Chanson chanson = db.Chanson.Find(id);
            string user = User.Identity.Name;
            
            bool isExtract = true;
            string musique;

            if (db.Achat.Where(c => c.Chanson1.Id == chanson.Id && c.Utilisateur == user).ToList().Count() >= 1)
            {
                musique = chanson.Musique;
                isExtract = false;
            }
            else musique = chanson.Extrait;
            ViewBag.isExtract = isExtract;

            var comms = db.Commentaire.Where(c => c.IdChanson == id).ToList();
            comms.Reverse();

            //Je veux arrondir chanson.note---------
            double partieDecimale = chanson.Note - Math.Floor(chanson.Note);
            int noteArrondie = (int)(Math.Floor(chanson.Note));
            if (partieDecimale > 0.5) noteArrondie += 1;

            var panier = db.Panier.Where(p => p.Utilisateur == User.Identity.Name && p.Object == chanson.Id).ToList();
            bool inPanier = false;
            if (panier.Count > 0) { inPanier = true; }

            vmChansonInformation info = new vmChansonInformation
            {
                album = chanson.Album1.Titre,
                artiste = chanson.Album1.Artiste1.Nom,
                durée = (int)chanson.Durée,
                isExtract = isExtract,
                commentaires = comms,
                note = noteArrondie,
                titre = chanson.Titre,
                idAlbum = chanson.Album1.Id,
                idArtiste = chanson.Album1.Artiste1.Id,
                idChanson = chanson.Id,
                musique = musique,
                prix = (double)chanson.Prix / 100.0,
                isInPanier = inPanier
        };

            if (chanson == null)
            {
                return HttpNotFound();
            }
            return View(info);
        }

        public ActionResult TriNote( List<vmChansonInformation> chansons, string url)
        {
            chansons.Sort((x, y) => x.note.CompareTo(y.note));
            chansons.Reverse();
            

            return View(url,chansons);
        }

        

        //Get : Chanson/IndexPartial
        public ActionResult IndexPartial(List<vmChansonInformation> model,string tri)
        {
            if (model != null)
            {
                if (tri == "note")
                {
                    model.Sort((x, y) => x.note.CompareTo(y.note));
                    model.Reverse();
                }
                else if (tri == "album")
                {
                    model.Sort((x, y) => x.album.CompareTo(y.album));
                }
                else if (tri == "artiste")
                {
                    model.Sort((x, y) => x.artiste.CompareTo(y.artiste));
                }
                else if (tri == "duree")
                {
                    model.Sort((x, y) => x.durée.CompareTo(y.durée));
                    model.Reverse();
                }
                else if (tri == "prix")
                {
                    model.Sort((x, y) => x.prix.CompareTo(y.prix));
                }
                else
                {
                    model.Sort((x, y) => x.titre.CompareTo(y.titre));
                }
            }
            else { model = new List<vmChansonInformation>(); }
                
                      
            return PartialView(model);
        }

        public ActionResult partialBtnAcheter(vmChansonInformation vm)
        {
            return PartialView(vm);
        }

        public ActionResult partialDejaPanier()
        {
            return PartialView();
        }

        // GET: Chanson/Create
        public ActionResult Create()
        {
            ViewBag.album = new SelectList(db.Album, "Id", "Titre");
            return View();
        }

        // POST: Chanson/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Durée,Titre,Album,Note")] Chanson chanson)
        {
            if (ModelState.IsValid)
            {
                db.Chanson.Add(chanson);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.album = new SelectList(db.Album, "Id", "Titre", chanson.Album);
            return View(chanson);
        }

        // GET: Chanson/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chanson chanson = db.Chanson.Find(id);
            if (chanson == null)
            {
                return HttpNotFound();
            }
            ViewBag.album = new SelectList(db.Album, "Id", "Titre", chanson.Album);
            return View(chanson);
        }

        // POST: Chanson/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Durée,Titre,Album,Note")] Chanson chanson)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chanson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.album = new SelectList(db.Album, "Id", "Titre", chanson.Album);
            return View(chanson);
        }

        // GET: Chanson/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chanson chanson = db.Chanson.Find(id);
            if (chanson == null)
            {
                return HttpNotFound();
            }
            return View(chanson);
        }

        // POST: Chanson/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chanson chanson = db.Chanson.Find(id);
            db.Chanson.Remove(chanson);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
