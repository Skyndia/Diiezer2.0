using Diiezer2._0.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diiezer2._0.Controllers
{
    public class ResearchController : Controller
    {
        private DiiezerDBEntities db = new DiiezerDBEntities();

        // GET: Research
        public ActionResult Begin()
        {
            return View();
        }

        //Prends une liste de chansons et en rend la vmChansonInformation
        private List<vmChansonInformation> getVm(List<Chanson> chansons)
        {
            string user = User.Identity.Name;

            List<vmChansonInformation> vmChansonInformations = new List<vmChansonInformation>();

            foreach (var item in chansons)
            {
                var notes = db.Note.Where(c => c.Chanson1.Id == item.Id).ToList();
                int i = 0;
                int tmp = 0;
                int note;
                foreach (var item2 in notes)
                {
                    i++;

                    tmp = tmp + item2.Valeur;

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
            vmChansonInformations.Sort((x, y) => x.titre.CompareTo(y.titre));
            return vmChansonInformations;
        }


        //Fait la requete sur les albums
        private List<Album> requeteAlbum(FormCollection criteres)
        {
            List<Album> result = new List<Album>();
            string champAlbum = criteres["album"];
            string champArtiste = criteres["artiste"];
            string champGenre = criteres["Genre"];
            int champNote = int.Parse(criteres["note"]);

            //Requête pour les artistes ici :
            result = db.Album.Where(a => a.Titre.Contains(champAlbum) && 
            (a.Artiste1.Nom.Contains(champArtiste)) && 
            a.Genre1.Nom.Contains(champGenre) &&
            a.Note >= champNote).ToList();

            return result;
        }

        //Fait la recherche dans la table des artistes
        
        private List<vmArtisteCover> requeteArtiste(FormCollection criteres)
        {
            List<vmArtisteCover> result = new List<vmArtisteCover>();
            List<Artiste> artistes = new List<Artiste>();
            List<Album> albums = new List<Album>();

            //On prend les albums qui correspondent à la requete
            albums = requeteAlbum(criteres);

            //Puis on en extrait les artistes
            foreach (var alb in albums)
            {
                if (!artistes.Contains(alb.Artiste1)) artistes.Add(alb.Artiste1);
            }

            foreach (var art in artistes)
            {
                vmArtisteCover artisteInfo = new vmArtisteCover
                {
                    id = art.Id,
                    nom = art.Nom,
                    cover = db.Album.Where(a => a.Artiste1.Id == art.Id).FirstOrDefault().Cover
                };

                result.Add(artisteInfo);
            }
            return result;
        }

        //fait la requete sur les chansons
        private List<vmChansonInformation> requeteChanson(FormCollection criteres)
        {
            List<vmChansonInformation> result = new List<vmChansonInformation>();
            string champTitre = criteres["titre"];
            string champGenre = criteres["genre"];
            int champNote = int.Parse(criteres["note"]);

            List<Chanson> chansons = new List<Chanson>();

            //Requête pour les artistes ici :
            //chansons = db.Chanson.Include(c => c.Album1).Where(a => a.Titre.Contains(champTitre) &&
            //a.Album1.Genre1.Nom.Contains(champGenre)).ToList();

            //Bon, Faut commencer par prendre toutes les notes supérieure à champNote. 
            //Après on en extrait les chansons. Puis on applique les critères sur les chansons.

            var notes = db.Note.Include(n => n.Chanson1).Where(n => n.Valeur >= champNote).ToList();
            foreach (var item in notes)
            {
                if (!chansons.Contains(item.Chanson1))
                {
                    chansons.Add(item.Chanson1);
                }
            }
            var chansonsFiltree = chansons.Where(a => a.Titre.Contains(champTitre) &&
            a.Album1.Genre1.Nom.Contains(champGenre)).ToList();


            result = getVm(chansonsFiltree);
            return result;
        }

        //Fait la recherche dans la table des artistes
        private List<vmArtisteCover> requeteArtisteRapide(String critere)
        {
            List<vmArtisteCover> result = new List<vmArtisteCover>();
            List<Artiste> artistes = new List<Artiste>();

            String champArtiste = critere;

            //Requête pour les artistes ici :
            artistes = db.Artiste.Where(a => a.Nom.Contains(champArtiste)).ToList();

            foreach (var art in artistes)
            {
                vmArtisteCover artisteInfo = new vmArtisteCover
                {
                    id = art.Id,
                    nom = art.Nom,
                    cover = db.Album.Where(a => a.Artiste1.Id == art.Id).FirstOrDefault().Cover
                };

                result.Add(artisteInfo);
            }
            return result;
        }

        //Fait la requete sur les albums
        private List<Album> requeteAlbumRapide(String critere)
        {
            List<Album> result = new List<Album>();
            String champAlbum = critere;

            //Requête pour les artistes ici :
            result = db.Album.Where(a => a.Titre.Contains(champAlbum)).ToList();

            return result;
        }

        //fait la requete sur les chansons
        private List<vmChansonInformation> requeteChansonRapide(String critere)
        {
            List<vmChansonInformation> result = new List<vmChansonInformation>();
            String champTitre = critere;
            List<Chanson> chansons = new List<Chanson>();

            //Requête pour les artistes ici :
            chansons = db.Chanson.Include(c => c.Album1).Where(a => a.Titre.Contains(champTitre)).ToList();


            result = getVm(chansons);
            return result;
        }

        //POST : Search
        [HttpPost]
        public ActionResult Search(FormCollection criteres)
        {
            vmResearchResult result = new vmResearchResult();
            String but = criteres["but"]; //but : Artistes, Albums, chansons ou tous

            //PARTIE ARTISTES
            if ((but.CompareTo("SearchArtists") == 0)||(but.CompareTo("SearchAll") == 0)) //On cherche des Artistes
            {
                result.artistes = requeteArtiste(criteres);
                result.artistes.Sort((x, y) => x.nom.CompareTo(y.nom));
            }
            //PARTIE ALBUMS
            if ((but.CompareTo("SearchAlbums") == 0) || (but.CompareTo("SearchAll") == 0)) //On cherche des Albums
            {
                result.albums = requeteAlbum(criteres);
                result.albums.Sort((x, y) => x.Titre.CompareTo(y.Titre));              
            }

            //PARTIE CHANSONS
            if ((but.CompareTo("SearchTitres") == 0) || (but.CompareTo("SearchAll") == 0)) //On cherche des Chansons
            {
                result.chansons = requeteChanson(criteres);
                result.chansons.Sort((x, y) => x.titre.CompareTo(y.titre));
            }
            ViewBag.but = but;
            return View("SearchResults", result);
        }

        //POST : Quick Search
        public ActionResult QuickSearch(String critere)
        {
            vmResearchResult result = new vmResearchResult();

            result.artistes = requeteArtisteRapide(critere);
            result.artistes.Sort((x, y) => x.nom.CompareTo(y.nom));

            result.albums = requeteAlbumRapide(critere);
            result.albums.Sort((x, y) => x.Titre.CompareTo(y.Titre));

            result.chansons = requeteChansonRapide(critere);
            result.chansons.Sort((x, y) => x.titre.CompareTo(y.titre));

            ViewBag.but = "SearchAll";

            return View("SearchResults", result);
        }
    }
}
