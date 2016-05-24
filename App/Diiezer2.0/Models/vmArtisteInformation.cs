using Diiezer2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Diiezer2._0.Models

{
   public class vmArtisteInformation
    {
        public int id { get; set; }
        public String nom { get; set; }
        public List<Album> albums { get; set; }
        public string cover { get; set; }
    }
}
