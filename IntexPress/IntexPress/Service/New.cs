using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntexPress.Service
{
    public class New 
    {
        public static List<string> tagsList = new List<string>();

        public int newId { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public string text { get; set; }
        public string picture { get; set; }
        public string tag { get; set; }
        public string description { get; set; }
    }
}