using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUserCodeFirst
{
    public class Like
    {
        [Key]       
        public int likeId { get; set; }
        [Required]
        public int userId { get; set; }
        [Required]
        public int commendId { get; set; }
        [Required]
        public bool isLike { get; set; } = false;
    }
}
