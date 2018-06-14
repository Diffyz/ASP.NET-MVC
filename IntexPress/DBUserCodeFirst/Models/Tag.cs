using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUserCodeFirst
{
    public class Tag
    {
        [Key]
        [Required]
        public int tagId { get; set; }

        [Required]
        public string tag { get; set; }
    }
}
