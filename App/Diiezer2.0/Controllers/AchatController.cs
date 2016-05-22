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

        private void AcheterChanson(int idChanson, string user)
        {

            var achats = db.Achat.Where(c => c.Utilisateur == user && c.Chanson1.Id == idChanson).ToList();
            if (achats.Count() == 0)
            {
                Achat nouvelAchat = new Achat();
                nouvelAchat.Utilisateur = user;
                nouvelAchat.Chanson = idChanson;
                nouvelAchat.Date = DateTime.Now;
                db.Achat.Add(nouvelAchat);
                db.SaveChanges();
            }
        }

        private void AcheterAlbum(int idAlbum, string user)
        {
            var chansons = db.Chanson.Where(c => c.Album1.Id == idAlbum).ToList();

            foreach (var item in chansons)
            {
                var achats = db.Achat.Where(c => c.Utilisateur == user && c.Chanson1.Id == item.Id).ToList();
                if (achats.Count() == 0)
                {
                    Achat nouvelAchat = new Achat();
                    nouvelAchat.Utilisateur = user;
                    nouvelAchat.Chanson = item.Id;
                    nouvelAchat.Date = DateTime.Now;
                    db.Achat.Add(nouvelAchat);
                    db.SaveChanges();
                }
            }
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
            List<Achat> achats = new List<Achat>();
            achats = db.Achat.Where(c=>c.Utilisateur == User.Identity.Name).ToList();
            achats.Reverse();

            return View(achats);
        }

        public ActionResult Acheter(string user)
        {
            List<Panier> panier = db.Panier.Where(p => p.Utilisateur == user).ToList();
            foreach (Panier achat in panier)
            {
                if (achat.IsAlbum == 0)
                {
                    AcheterChanson(achat.Object, user);
                }
                else
                {
                    AcheterAlbum(achat.Object, user);
                }
                db.Panier.Remove(achat);
                
            }
            db.SaveChanges();
            return Redirect("MesAchats");
        }

        public ActionResult partialAudio(int idChanson)
        {
            string path = db.Chanson.Where(c => c.Id == idChanson).First().Musique;
            return PartialView("partialAudio", path);
        }
    }
}
