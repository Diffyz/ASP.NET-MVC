
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace DBUserCodeFirst
{
    public class DBUser
    {
        [Key]
        public int userId { get; set; }

        [Required]
        [MaxLength(30)]
        public string login { get; set; }

        [Required]
        [MaxLength(30)]
        public string email { get; set; }

        [Required]
        [MaxLength(30)]
        public string password { get; set; }
        [Required]
        public int admit { get; set; } = 4;
        public bool validationEmail { get; set; }
        [Required]
        public int countLike { get; set; } = 0;
        public string location { get; set; } = "undefined";
        public bool isBlock { get; set; } = false;
        public string userPicture { get; set; } = "http://localhost:52310/Content/img/user.png";
        public string sex { get; set; } = "";
        public string phone { get; set; } = "";
        public float ratingAt { get; set; } = 0;
    }
}