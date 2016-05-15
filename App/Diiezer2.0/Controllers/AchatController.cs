using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Diiezer2._0.Models;

namespace Diiezer2._0.Controllers
{
    public class AchatController : Controller
    {
        private DiiezerDBEntities db = new DiiezerDBEntities();

        public ActionResult AcheterChanson(int idChanson)
        {

            var achats = db.Achat.Where(c => c.Utilisateur == User.Identity.Name && c.Chanson1.Id == idChanson).ToList();
            if (achats.Count() == 0)
            {
                Achat nouvelAchat = new Achat();
                nouvelAchat.Utilisateur = User.Identity.Name;
                nouvelAchat.Chanson = idChanson;
                nouvelAchat.Date = DateTime.Now;
                db.Achat.Add(nouvelAchat);
                db.SaveChanges();
            }


            return Redirect("../Chanson/Details/"+idChanson.ToString());
        }

        public ActionResult AcheterAlbum(int idAlbum)
        {
            var chansons = db.Chanson.Where(c => c.Album1.Id == idAlbum).ToList();

            foreach (var item in chansons)
            {
                var achats = db.Achat.Where(c => c.Utilisateur == User.Identity.Name && c.Chanson1.Id == item.Id).ToList();
                if (achats.Count() == 0)
                {
                    Achat nouvelAchat = new Achat();
                    nouvelAchat.Utilisateur = User.Identity.Name;
                    nouvelAchat.Chanson = item.Id;
                    nouvelAchat.Date = DateTime.Now;
                    db.Achat.Add(nouvelAchat);
                    db.SaveChanges();
                }
            }

            


            return Redirect("../Album/Details/" + idAlbum.ToString());
        }

        // GET: Achat
        public ActionResult Index()
        {
            var achat = db.Achat.Include(a => a.Chanson1);
            return View(achat.ToList());
        }

        public ActionResult MesAchats()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var achats = db.Achat.Where(c=>c.Utilisateur == User.Identity.Name).Reverse();

            return View(achats);
        }

        // GET: Achat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Achat achat = db.Achat.Find(id);
            if (achat == null)
            {
                return HttpNotFound();
            }
            return View(achat);
        }

        // GET: Achat/Create
        public ActionResult Create()
        {
            ViewBag.Chanson = new SelectList(db.Chanson, "Id", "Titre");
            return View();
        }

        // POST: Achat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Chanson,Utilisateur")] Achat achat)
        {
            if (ModelState.IsValid)
            {
                db.Achat.Add(achat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Chanson = new SelectList(db.Chanson, "Id", "Titre", achat.Chanson);
            return View(achat);
        }

        // GET: Achat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Achat achat = db.Achat.Find(id);
            if (achat == null)
            {
                return HttpNotFound();
            }
            ViewBag.Chanson = new SelectList(db.Chanson, "Id", "Titre", achat.Chanson);
            return View(achat);
        }

        // POST: Achat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Chanson,Utilisateur")] Achat achat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(achat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Chanson = new SelectList(db.Chanson, "Id", "Titre", achat.Chanson);
            return View(achat);
        }

        // GET: Achat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Achat achat = db.Achat.Find(id);
            if (achat == null)
            {
                return HttpNotFound();
            }
            return View(achat);
        }

        // POST: Achat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Achat achat = db.Achat.Find(id);
            db.Achat.Remove(achat);
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
