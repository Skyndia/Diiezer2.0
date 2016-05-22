using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diiezer2._0.Models
{
    public class vmPanier
    {
        public List<vmAlbumInformation> albums { get; set; }
        public List<vmChansonInformation> chansons { get; set; }
        public double total { get; set; }
    }
}