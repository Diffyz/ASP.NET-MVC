using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUserCodeFirst
{
    public class Rating
    {
        [Required]
        [Key]
        public int ratingId { get; set; }
        [Required]
        public int userId { get; set; }
        [Required]
        public int newsId { get; set; }
        [Required]
        public int rating { get; set; }

    }
}
