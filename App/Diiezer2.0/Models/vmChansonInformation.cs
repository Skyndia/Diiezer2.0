using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diiezer2._0.Models
{
    public class vmChansonInformation
    {
        public String titre { get; set; }
        public int idChanson { get; set; }
        public List<Commentaire> commentaires { get; set; }
        public bool isExtract { get; set; }
        public int durée { get; set; }
        public String album { get; set; }
        public int idAlbum { get; set; }
        public int note { get; set; }
        public String artiste { get; set; }
        public int idArtiste { get; set; }
        public String musique { get; set; }
        public double prix { get; set; }
        public bool isInPanier { get; set; }
    }
}
