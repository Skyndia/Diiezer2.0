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
    }
}