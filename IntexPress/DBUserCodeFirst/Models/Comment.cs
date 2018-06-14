using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUserCodeFirst
{
    public class Comment
    {
        [Key]
        public int commentId { get; set; }
        [Required]
        public int newId { get; set; }
        [Required]
        public int userId { get; set; }
        [Required]
        public string userLogin { get; set; }
        [Required]
        public string text { get; set; }

        [Required]
        public int countLike { get; set; } = 0;

        [Required]
        public bool isLikeComment { get; set; } = false;
    }
}
