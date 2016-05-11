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
    public class AlbumController : Controller
    {
        private DiiezerDBEntities db = new DiiezerDBEntities();

        // GET: Album
        public ActionResult Index2()
        {
            var album = db.Album.Include(a => a.Artiste1);
            return View(album.ToList());
        }

        // GET: Album/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var context = new DiiezerDBEntities();
            
            Album album = context.Album.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }

            var chansons = context.Chanson.Where(c => c.Album1.Id == id).ToList();

            vmAlbumInformation albuminfo = new vmAlbumInformation
            {
                nom = album.Titre,
                artiste = album.Artiste1.Nom,
                idArtiste = album.Artiste1.Id,
                chansons = chansons,
                cover = "Diiezer/Content/cover/1.jpg",
                duree = (int)album.Durée,
                nombre = (int)album.NbChanson,
                style = album.Style
            };
               
      


            return View(albuminfo);
        }


        // GET: Album/Create
        public ActionResult Create()
        {
            ViewBag.Artiste = new SelectList(db.Artiste, "Id", "Nom");
            return View();
        }

        // POST: Album/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NbChanson,Titre,Durée,Artiste,Date,Style")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Album.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Artiste = new SelectList(db.Artiste, "Id", "Nom", album.Artiste);
            return View(album);
        }

        // GET: Album/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Album.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            ViewBag.Artiste = new SelectList(db.Artiste, "Id", "Nom", album.Artiste);
            return View(album);
        }

        // POST: Album/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NbChanson,Titre,Durée,Artiste,Date,Style")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Artiste = new SelectList(db.Artiste, "Id", "Nom", album.Artiste);
            return View(album);
        }

        // GET: Album/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Album.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Album/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Album.Find(id);
            db.Album.Remove(album);
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
