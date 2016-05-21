using Diiezer2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diiezer2._0.Controllers
{
    public class CommentaireController : Controller
    {
        private DiiezerDBEntities db = new DiiezerDBEntities();
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

        public ActionResult partialComment(Commentaire comment,int? idbox)
        {
            ViewBag.idbox = idbox;
            return PartialView(comment);
        }

        public ActionResult partialEditComm(int? idCommentaire, int idbox)
        {
            ViewBag.idbox = idbox;
            var com = db.Commentaire.Where(c => c.Id == idCommentaire).First();
            return PartialView(com);
        }

        public ActionResult saveCommentChanges(int idCommentaire, int idbox, string text)
        {
            var com = db.Commentaire.Where(c => c.Id == idCommentaire).First();

            com.Texte = text;
            db.SaveChanges();
            ViewBag.idbox = idbox;
            return PartialView("partialComment", com);
        }
    }
}