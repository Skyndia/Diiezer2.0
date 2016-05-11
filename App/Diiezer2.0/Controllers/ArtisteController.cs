using Diiezer2._0.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Diiezer.Models
{
    public class ArtisteController : Controller
    {
        private DiiezerDBEntities db = new DiiezerDBEntities();

        // GET: Artiste
        public ActionResult Index()
        {
            return View(db.Artiste.ToList());
        }

        // GET: Artiste/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artiste artiste = db.Artiste.Find(id);
            if (artiste == null)
            {
                return HttpNotFound();
            }

            DiiezerDBEntities context = new DiiezerDBEntities();

            vmArtisteInformation artisteInfo = new vmArtisteInformation {
                id = artiste.Id,
                nom = artiste.Nom,
                albums = context.Album.Where(a => a.Artiste1.Id == id).ToList()
            };
            return View(artisteInfo);
        }

        // GET: Artiste/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artiste/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom")] Artiste artiste)
        {
            if (ModelState.IsValid)
            {
                db.Artiste.Add(artiste);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artiste);
        }

        // GET: Artiste/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artiste artiste = db.Artiste.Find(id);
            if (artiste == null)
            {
                return HttpNotFound();
            }
            return View(artiste);
        }

        // POST: Artiste/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom")] Artiste artiste)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artiste).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artiste);
        }

        // GET: Artiste/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artiste artiste = db.Artiste.Find(id);
            if (artiste == null)
            {
                return HttpNotFound();
            }
            return View(artiste);
        }

        // POST: Artiste/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artiste artiste = db.Artiste.Find(id);
            db.Artiste.Remove(artiste);
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
