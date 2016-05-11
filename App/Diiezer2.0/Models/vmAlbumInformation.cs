using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diiezer2._0.Models
{
    public class vmAlbumInformation
    {
        public String nom { get; set; }
        public String artiste { get; set; }
        public int idArtiste { get; set; }
        public int nombre { get; set; }
        public List<Chanson> chansons { get; set; }
        public int duree { get; set; }
        public String style { get; set; }
        public String cover { get; set; }
    }


}
