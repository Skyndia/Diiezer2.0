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

            List<vmChansonInformation> vmChansonInformations = new List<vmChansonInformation>();
            

            var chansons = db.Chanson.Include(c => c.Album1).ToList();
            foreach (var item in chansons)
            {
                var notes = db.Note.Where(c => c.Chanson1.Id == id).ToList();
                int i = 0;
                int tmp = 0;
                int note;
                foreach (var item2 in notes)
                {
                    i++;
                    tmp = tmp + item2.Note1;
                }
                if (i == 0)
                {
                    note = 2;
                }
                else note = tmp / i;

                vmChansonInformations.Add(new vmChansonInformation {
                    album = item.Album1.Titre,
                    artiste = item.Album1.Artiste1.Nom,
                    durée = (int)item.Durée,
                    note = note,
                    titre = item.Titre,
                    idAlbum = item.Album1.Id,
                    idArtiste = item.Album1.Artiste1.Id,
                    idChanson = item.Id,
                    musique = item.Musique
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
            var notes = db.Note.Where(c => c.Chanson1.Id == id).ToList();
            int i = 0;
            int tmp = 0;
            int note;
            foreach (var item in notes)
            {
                i++;
                tmp = tmp + item.Note1;
            }
            if (i == 0)
            {
                note = 2;
            }
            else note = tmp / i;
            vmChansonInformation info = new vmChansonInformation
            {
                album = chanson.Album1.Titre,
                artiste = chanson.Album1.Artiste1.Nom,
                durée = (int)chanson.Durée,
                note = note,
                titre = chanson.Titre,
                idAlbum = chanson.Album1.Id,
                idArtiste = chanson.Album1.Artiste1.Id,
                idChanson = chanson.Id,
                musique = chanson.Musique
            };

            if (chanson == null)
            {
                return HttpNotFound();
            }
            return View(info);
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
