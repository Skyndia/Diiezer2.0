﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diiezer2._0.Models
{
    public class vmResearchResult
    {
        public List<vmArtisteCover> artistes { get; set; }
        public List<Album> albums { get; set; }
        public List<vmChansonInformation> chansons { get; set; }
    }
}

