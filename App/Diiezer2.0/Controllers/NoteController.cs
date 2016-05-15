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
    public class NoteController : Controller
    {
        private DiiezerDBEntities db = new DiiezerDBEntities();

        

        [HttpPost]
        public ActionResult Soumettre(string idMusique, int note, string idAlbum)
        {
            int idA = int.Parse(idAlbum);
            int idM = int.Parse(idMusique);
            

            var notes = db.Note.Where(c => c.Chanson1.Id == idM && c.Utilisateur == User.Identity.Name).ToList();


            if (notes.Count()>0)
            {
                var currentNote = notes.First();
                currentNote.Note1 = note;
                db.SaveChanges();
                
            } else
            {
                Note nouvelleNote = new Note();
                nouvelleNote.Note1 = note;
                nouvelleNote.Chanson = idM;
                nouvelleNote.Utilisateur = User.Identity.Name;
                db.Note.Add(nouvelleNote);
                db.SaveChanges();
            }
            
            string url = "../Album/Details/" + idAlbum;
            return Redirect(url);
        }

        // GET: Note
        public ActionResult Index()
        {
            var note = db.Note.Include(n => n.Chanson1);
            return View(note.ToList());
        }

        // GET: Note/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Note/Create
        public ActionResult Create()
        {
            ViewBag.Chanson = new SelectList(db.Chanson, "Id", "Titre");
            return View();
        }

        // POST: Note/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Chanson,Utilisateur,Note1")] Note note)
        {
            if (ModelState.IsValid)
            {
                db.Note.Add(note);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Chanson = new SelectList(db.Chanson, "Id", "Titre", note.Chanson);
            return View(note);
        }

        // GET: Note/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.Chanson = new SelectList(db.Chanson, "Id", "Titre", note.Chanson);
            return View(note);
        }

        // POST: Note/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Chanson,Utilisateur,Note1")] Note note)
        {
            if (ModelState.IsValid)
            {
                db.Entry(note).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Chanson = new SelectList(db.Chanson, "Id", "Titre", note.Chanson);
            return View(note);
        }

        // GET: Note/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Note/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = db.Note.Find(id);
            db.Note.Remove(note);
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
