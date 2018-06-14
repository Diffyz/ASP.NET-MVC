using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUserCodeFirst
{
    public class DBNews
    {
        [Key]
        public int newsId { get; set; }
        [Required]
        public int userId { get; set; }

        [Required]
        public string nameAuthor { get; set; }
        [Required]
        public int countLikeAuthor { get; set; }

        public string description{get;set;}

        [Required]
        public string title { get; set; }
        [Required]
        public string categories { get; set; }
        [Required]
        public string text { get; set; }
        public string image { get; set; }
        public int like { get; set; }
        public float rating { get; set; } = 0;
    }
}
