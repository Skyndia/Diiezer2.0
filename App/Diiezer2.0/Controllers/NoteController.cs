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

        //Permet de noter en cliquant sur les 
        public ActionResult Noter(string idMusique, string note)
        {
            int idM = int.Parse(idMusique);
            int not = int.Parse(note);
            var notes = db.Note.Where(c => c.Chanson1.Id == idM && c.Utilisateur == User.Identity.Name).ToList();
            int ancienneNoteChanson = 0;
            //Update de la table note
            //Si il y avait déjà une note
            if (notes.Count() > 0)
            {
                var currentNote = notes.First();
                ancienneNoteChanson = currentNote.Valeur; //Save qui servira pour noter l'album
                currentNote.Valeur = not;              
            }
            else//Si on ajoute une nouvelle note
            {
                Note nouvelleNote = new Note();
                nouvelleNote.Valeur = not;
                nouvelleNote.Chanson = idM;
                nouvelleNote.Utilisateur = User.Identity.Name;
                db.Note.Add(nouvelleNote);
            }
            //Update de la note de la chanson (dans la table chanson)
            var chanson = db.Chanson.Where(c => c.Id == idM).First();

            double ancienneNote = chanson.Note;
            int nbNoteChanson;
            double newNoteChanson;

            if (notes.Count() > 0) //Si il y avait déjà une note
            {
                nbNoteChanson = chanson.NbNote;
                newNoteChanson = (ancienneNote * nbNoteChanson - ancienneNoteChanson + not) / nbNoteChanson;
            }
            else
            {
                nbNoteChanson = chanson.NbNote + 1;
                newNoteChanson = (ancienneNote * chanson.NbNote + not) / nbNoteChanson;
            }

            if (chanson.NbNote == 0) // Si la chanson n'avait pas encore été notée
            {
                chanson.Note = not;
                chanson.NbNote = 1;
            }
            else
            {
                chanson.Note = newNoteChanson;
                chanson.NbNote = nbNoteChanson;
            }
            

            //Update de la note de l'album correspondant :


            var album = chanson.Album1;
            var chansons = db.Chanson.Where(c => c.Album1.Id == album.Id).ToList();

            double somme = 0;
            foreach (var item in chansons)
            {
                somme += item.Note;
            }
            album.Note = somme / (double)album.NbChanson;
            

            db.SaveChanges();
            vmChansonInformation result = new vmChansonInformation();
            result.idChanson = idM;

            //Je veux arrondir chanson.note---------
            double partieDecimale = chanson.Note - Math.Floor(chanson.Note);
            int noteArrondie = (int)(Math.Floor(chanson.Note));
            if (partieDecimale > 0.5) noteArrondie += 1;
            result.note = noteArrondie;

            return PartialView("partialNote", result);

        }

        //GET : partialNote
        public ActionResult partialNote(int idChanson, int note)
        {
            vmChansonInformation result = new vmChansonInformation();
            result.note = note;
            result.idChanson = idChanson;
            return PartialView(result);
        }

        // GET: Note
        public ActionResult Index()
        {
            var note = db.Note.Include(n => n.Chanson1);
            return View(note.ToList());
        }

        
    }
}
