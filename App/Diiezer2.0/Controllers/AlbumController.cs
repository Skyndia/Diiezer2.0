
using Diiezer2._0.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Diiezer2._0.Models
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

        private vmAlbumInformation getVmAlbum(Album album, int id)
        {
            var chansons = db.Chanson.Where(c => c.Album1.Id == id).ToList();
            string user = User.Identity.Name;

            List<vmChansonInformation> vmChansonInformations = new List<vmChansonInformation>();

            foreach (var item in chansons)
            {

                string musique;
                bool isExtract = true;

                if (db.Achat.Where(c => c.Chanson1.Id == item.Id && c.Utilisateur == user).ToList().Count() >= 1)
                {
                    musique = item.Musique;
                    isExtract = false;
                }
                else musique = item.Extrait;
                string titre = item.Titre;

                vmChansonInformations.Add(new vmChansonInformation
                {
                    album = item.Album1.Titre,
                    isExtract = isExtract,
                    artiste = item.Album1.Artiste1.Nom,
                    durée = (int)item.Durée,
                    note = (int)item.Note,
                    titre = titre,
                    idAlbum = item.Album1.Id,
                    idArtiste = item.Album1.Artiste1.Id,
                    idChanson = item.Id,
                    musique = musique,
                    prix = (double)item.Prix / 100.0
                });
            }
            //Je veux arrondir album.note---------
            double partieDecimale = album.Note - Math.Floor(album.Note);
            int noteArrondie = (int)(Math.Floor(album.Note));
            if (partieDecimale > 0.5) noteArrondie += 1;

            //----------------------------------
            //Construction d'une liste pour le lecteur
            int compteur = 0;
            List<titrePourLecteur> listLecteur = new List<titrePourLecteur>();
            foreach (var item in chansons)
            {
                compteur++;

                string musique;

                if (db.Achat.Where(c => c.Chanson1.Id == item.Id && c.Utilisateur == user).ToList().Count() >= 1)
                {
                    musique = item.Musique;
                }
                else musique = item.Extrait;

                titrePourLecteur obj = new titrePourLecteur
                {
                    track = "\"" + compteur.ToString() + "\"",
                    name = "\"" + item.Titre + "\"",
                    length = "\"" + (item.Durée / 60).ToString() + ":" + (item.Durée % 60).ToString() + "\"",
                    file = "\"" + musique.Remove(musique.Count() - 4) + "\""
                };
                listLecteur.Add(obj);
            }
            string listLecteurString = "[";
            foreach (var item in listLecteur)
            {
                listLecteurString += item.toString() + ",";
            }
            listLecteurString = listLecteurString.Remove(listLecteurString.Count() - 1);
            listLecteurString += "]";


            //------------------------------------
            vmAlbumInformation albuminfo = new vmAlbumInformation
            {
                id = album.Id,
                nom = album.Titre,
                artiste = album.Artiste1.Nom,
                idArtiste = album.Artiste1.Id,
                chansons = vmChansonInformations,
                cover = album.Cover,
                duree = (int)album.Durée,
                nombre = (int)album.NbChanson,
                genre = album.Genre1.Nom,
                note = noteArrondie, //la note est de type double dans la DB
                prix = album.Prix / 100.0,
                date = album.Date.ToString().Substring(0, 10),
                toto = listLecteurString
            };
            Session["vmAlbum"] = new vmAlbumInformation();
            Session["vmAlbum"] = albuminfo;
            return albuminfo;
        }

        // GET: Album/Details/5
        public ActionResult Details(int? id)
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

            vmAlbumInformation albuminfo = getVmAlbum(album, id.Value);


            return View(albuminfo);
        }

        public ActionResult partialEcouterAlbum(string id)
        {
            int idA = int.Parse(id);
            Album album = db.Album.Find(idA);
            if (album == null)
            {
                return HttpNotFound();
            }

            //vmAlbumInformation albuminfo = getVmAlbum(album, idA);
            var albuminfo = Session["vmAlbum"] as vmAlbumInformation;
            return PartialView("../Album/partialLecteur", albuminfo);
        }

        public ActionResult partialListeChanson(string id)
        {
            int idA = int.Parse(id);
            Album album = db.Album.Find(idA);
            if (album == null)
            {
                return HttpNotFound();
            }

            //vmAlbumInformation albuminfo = getVmAlbum(album, idA);
            var albuminfo = Session["vmAlbum"] as vmAlbumInformation;

            return PartialView("../Chanson/IndexPartialDark", albuminfo.chansons);
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

        
    }
}
