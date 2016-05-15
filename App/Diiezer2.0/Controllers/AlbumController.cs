﻿using Diiezer2._0.Models;
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
        public ActionResult Index()
        {
            ViewBag.userAuthenticated = User.Identity.IsAuthenticated;
            ViewBag.userName = User.Identity.Name;

            var album = db.Album.Include(a => a.Artiste1).ToList();
            album.Sort((x, y) => x.Titre.CompareTo(y.Titre));
            return View(album);
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
            string user = User.Identity.Name;

            List<vmChansonInformation> vmChansonInformations = new List<vmChansonInformation>();

            foreach (var item in chansons)
            {
                var notes = db.Note.Where(c => c.Chanson1.Id == id).ToList();
                int i = 0;
                int tmp = 0;
                int note;
                foreach (var item2 in notes)
                {
                    i++;
                    tmp = tmp + item2.Note1.Value;
                }
                if (i == 0)
                {
                    note = 2;
                }
                else note = tmp / i;

                string musique;
                bool isExtract = true;

                if (db.Achat.Where(c => c.Chanson1.Id == item.Id && c.Utilisateur == user).ToList().Count() >= 1)
                {
                    musique = item.Musique;
                    isExtract = false;
                }
                else musique = item.Extrait;

                vmChansonInformations.Add(new vmChansonInformation
                {
                    album = item.Album1.Titre,
                    isExtract = isExtract,
                    artiste = item.Album1.Artiste1.Nom,
                    durée = (int)item.Durée,
                    note = note,
                    titre = item.Titre,
                    idAlbum = item.Album1.Id,
                    idArtiste = item.Album1.Artiste1.Id,
                    idChanson = item.Id,
                    musique = musique
                });
            }

            vmAlbumInformation albuminfo = new vmAlbumInformation
            {
                nom = album.Titre,
                artiste = album.Artiste1.Nom,
                idArtiste = album.Artiste1.Id,
                chansons = vmChansonInformations,
                cover = album.Cover,
                duree = (int)album.Durée,
                nombre = (int)album.NbChanson,
                genre = album.Genre1.Nom
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
