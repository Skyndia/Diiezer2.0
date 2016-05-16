using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diiezer2._0.Models
{
    public class vmAlbumInformation
    {
        public int id { get; set; }
        public String nom { get; set; }
        public String artiste { get; set; }
        public int idArtiste { get; set; }
        public int nombre { get; set; }
        public List<vmChansonInformation> chansons { get; set; }
        public int duree { get; set; }
        public string genre { get; set; }
        public String cover { get; set; }
        public int note { get; set; }

        //note l'album en moyenne
        public void noterAlbum(List<vmChansonInformation> vmChansons)
        {
            int somme = 0, cpt = 0;
            foreach (var item in vmChansons)
            {
                somme += item.note;
                cpt++;
            }
            if (cpt == 0) note = 0;
            else note = somme / cpt;
        }
    }
}
