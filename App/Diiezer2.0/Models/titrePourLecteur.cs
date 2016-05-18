using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diiezer2._0.Models
{
    public class titrePourLecteur
    {
        public string track { get; set; }
        public string name { get; set; }
        public string length { get; set; }
        public string file { get; set;}

        public string toString()
        {
            string result;
            result = @"{ ""track"" : " + track + @", ""name"" : " + name + @", ""length"" : " + length + @", ""file"" : " + file + "}";
            return result;
        }
    }

     
}