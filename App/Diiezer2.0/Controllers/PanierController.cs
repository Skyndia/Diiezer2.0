using Diiezer2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diiezer2._0.Controllers
{
    public class PanierController : Controller
    {
        private DiiezerDBEntities db = new DiiezerDBEntities();

        // GET: Panier
        public ActionResult MonPanier()
        {
            //on doit envoyer les albums et les chansons faisant partie du panier de l'utilisateur. 
            string user = User.Identity.Name;
            var panier = db.Panier.Where(p => p.Utilisateur == user).ToList();
            var panierAlbums = panier.Where(p => p.IsAlbum == 1).ToList();
            var panierChansons = panier.Where(p => p.IsAlbum == 0).ToList();

            double total = 0; //total du panier en euros

            List<Album> albums = new List<Album>();
            List<Chanson> chansons = new List<Chanson>();

            foreach (var item in panierAlbums)
            {
                var album = db.Album.Where(a => a.Id == item.Object).First();
                albums.Add(album);
            }
            foreach (var item in panierChansons)
            {
                var chanson = db.Chanson.Where(a => a.Id == item.Object).First();
                chansons.Add(chanson);
            }
            //voila maintenant on a les listes des albums et des chansons que l'utilisateur veut acheter

            //on doit créer la liste des vmChansonInformation à partir de chansons
            List<vmChansonInformation> vmList = new List<vmChansonInformation>();

            foreach (var item in chansons)
            {
                vmList.Add(new vmChansonInformation
                {
                    album = item.Album1.Titre,
                    artiste = item.Album1.Artiste1.Nom,
                    titre = item.Titre,
                    idAlbum = item.Album1.Id,
                    idArtiste = item.Album1.Artiste1.Id,
                    idChanson = item.Id,
                    prix = (double)item.Prix / 100.0
                });
                total += (double)item.Prix / 100.0;
            }

            //voila pour les chansons non rangées dans un album.
            //On doit maintenant s'occuper des albums. On veut envoyer l'album mais aussi le nom de chacune de ses chansons.
            List<vmAlbumInformation> vmAlbumsList = new List<vmAlbumInformation>();

            foreach (var item in albums)
            {
                chansons = db.Chanson.Where(c => c.Album == item.Id).ToList();
                List<vmChansonInformation> vmChansonsList = new List<vmChansonInformation>();
                foreach (var song in chansons)
                {
                    vmChansonsList.Add(new vmChansonInformation
                    {
                        titre = song.Titre,
                        prix = (double)song.Prix / 100.0,
                        idChanson = song.Id
                    });
                }

                vmAlbumsList.Add(new vmAlbumInformation
                {
                    id = item.Id,
                    nom = item.Titre,
                    artiste = item.Artiste1.Nom,
                    idArtiste = item.Artiste1.Id,
                    chansons = vmChansonsList,
                    prix = item.Prix / 100.0
                });
                total += item.Prix / 100.0;
            }

            //Voila, plus qu'à renvoyer les info vers la vue
            vmPanier result = new vmPanier();
            result.albums = vmAlbumsList;
            result.chansons = vmList;
            result.total = total;
            return View("MonPanier",result);
        }

        

        public ActionResult AjoutChansonPanier(int idChanson, string url)
        {
            Chanson chanson = db.Chanson.Where(c => c.Id == idChanson).First();
            var panierChanson = db.Panier.Where(p => p.Object == idChanson).ToList();
            var panierAlbum = db.Panier.Where(p => p.Object == chanson.Album).ToList();

            if ((panierAlbum.Count == 0)&&(panierChanson.Count == 0))
            {
                Panier nouveauDesir = new Panier();
                nouveauDesir.Utilisateur = User.Identity.Name;
                nouveauDesir.Object = idChanson;
                nouveauDesir.IsAlbum = 0;
                db.Panier.Add(nouveauDesir);
                db.SaveChanges();
            }
            

            return Redirect(url);
        }

        public ActionResult partialAjouterChansonPanier(int idChanson)
        {
            Chanson chanson = db.Chanson.Where(c => c.Id == idChanson).First();
            var panierChanson = db.Panier.Where(p => p.Object == idChanson).ToList();
            var panierAlbum = db.Panier.Where(p => p.Object == chanson.Album).ToList();

            if ((panierAlbum.Count == 0) && (panierChanson.Count == 0))
            {
                Panier nouveauDesir = new Panier();
                nouveauDesir.Utilisateur = User.Identity.Name;
                nouveauDesir.Object = idChanson;
                nouveauDesir.IsAlbum = 0;
                db.Panier.Add(nouveauDesir);
                db.SaveChanges();
            }

            return PartialView("../Chanson/partialDejaPanier");
        }

        public ActionResult AjoutAlbumPanier(int idAlbum)
        {
            var chansons = db.Chanson.Where(c => c.Album1.Id == idAlbum).ToList();

            //Si certaines chansons de l'album sont déjà dans le panier, on les retire
            foreach (var item in chansons)
            {
                var panier = db.Panier.Where(c => c.Utilisateur == User.Identity.Name && c.Object == item.Id).ToList();
                if (panier.Count != 0)
                {
                    db.Panier.Remove(panier.ElementAt(0));
                }
            }

            //On ajoute tout l'album au panier
            Panier nouveauDesir = new Panier();
            nouveauDesir.Utilisateur = User.Identity.Name;
            nouveauDesir.Object = idAlbum;
            nouveauDesir.IsAlbum = 1;
            db.Panier.Add(nouveauDesir);

            db.SaveChanges();
            return Redirect("../Album/Details/" + idAlbum.ToString());
        }
        
        public ActionResult supprimerPanier(int id)
        {
            var user = User.Identity.Name;
            var panier = db.Panier.Where(p => p.Object == id && p.Utilisateur == user).ToList();
            if (panier.Count < 1)
            {
                return HttpNotFound("On essaye de supprimer du panier un truc qui n'existe pas !");
            }
            else
            {
                Panier aSupprimer = panier.First();
                db.Panier.Remove(aSupprimer);
                db.SaveChanges();
            }
            return MonPanier();
        }
    }
}
