using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUserCodeFirst
{
    public class NewsTags
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int newsId { get; set; }

        [Required]
        public int tagId { get; set; }
    }
}
