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
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                id = 1;
            }
            string user = User.Identity.Name;

            List<vmChansonInformation> vmChansonInformations = new List<vmChansonInformation>();
            

            var chansons = db.Chanson.Include(c => c.Album1).ToList();
            foreach (var item in chansons)
            {
                string musique;
                bool isExtract = true;

                if (db.Achat.Where(c=>c.Chanson1.Id == item.Id && c.Utilisateur == user ).ToList().Count() >= 1)
                {
                    musique = item.Musique;
                    isExtract = false;
                } else musique = item.Extrait;

                vmChansonInformations.Add(new vmChansonInformation {
                    album = item.Album1.Titre,
                    isExtract = isExtract,
                    artiste = item.Album1.Artiste1.Nom,
                    durée = (int)item.Durée,
                    note = (int)item.Note,
                    titre = item.Titre,
                    idAlbum = item.Album1.Id,
                    idArtiste = item.Album1.Artiste1.Id,
                    idChanson = item.Id,
                    musique = musique
                });
            }
            int NbChansonsParPage = 20;
            int nbPage = vmChansonInformations.Count / NbChansonsParPage + 1;
            int precedente = 1;
            int suivante = nbPage;

            if (id > 2) precedente = id.Value - 1;
            if (id < nbPage - 1) suivante = id.Value + 1;

            ViewBag.page = id;
            ViewBag.nbPage = nbPage;
            ViewBag.precedente = precedente;
            ViewBag.suivante = suivante;

            int n = vmChansonInformations.Count;
            int max = NbChansonsParPage;
            if (NbChansonsParPage > n - NbChansonsParPage * (id.Value - 1)) max = n - NbChansonsParPage * (id.Value - 1);


            vmChansonInformations.Sort((x, y) => x.titre.CompareTo(y.titre));
            return View(vmChansonInformations.Skip(20 * (id.Value - 1)).Take(max));
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

            vmChansonInformation info = new vmChansonInformation
            {
                album = chanson.Album1.Titre,
                artiste = chanson.Album1.Artiste1.Nom,
                durée = (int)chanson.Durée,
                isExtract = isExtract,
                commentaires = comms,
                note = (int)chanson.Note,
                titre = chanson.Titre,
                idAlbum = chanson.Album1.Id,
                idArtiste = chanson.Album1.Artiste1.Id,
                idChanson = chanson.Id,
                musique = musique
            };

            if (chanson == null)
            {
                return HttpNotFound();
            }
            return View(info);
        }

        [HttpPost]
        public ActionResult Commenter(string idMusique, string comment, string url)
        {

            Commentaire comm = new Commentaire();
            comm.IdChanson = int.Parse(idMusique);
            comm.Texte = comment;
            comm.Utilisateur = User.Identity.Name;
            db.Commentaire.Add(comm);
            db.SaveChanges();
            

            return Redirect(url);
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
