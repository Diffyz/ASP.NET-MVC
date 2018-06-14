using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUserCodeFirst
{
    public class Category
    {
        [Key]
        public int categoryId { get; set; }

        [Required]
        public string category { get; set; }
    }
}
