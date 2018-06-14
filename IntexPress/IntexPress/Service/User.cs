using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntexPress.Service
{
    public class User
    {
        public int userId { get; set; }
        [Required(ErrorMessage = "Введите логин")]
        public string login{get;set;}

        [Required(ErrorMessage = "Введите E-mail")]
        [RegularExpression(@".+\@.+\..+", ErrorMessage = "Неправильный E-mail")]
        public string email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        public string password { get; set; }

        public bool validationEmail { get; set; }
        public List<int> newsListRating = new List<int>();
        public int countLikeAuthor { get; set; }

        public int admit { get; set; } = 4;
        public bool isBlock { get; set; } = false;
        public string picture { get; set; } = "http://localhost:52310/Content/img/user.png";

        public string sex { get; set; } = "";
        public string phone { get; set; } = "";
        public float ratingAt { get; set; } = 0;
    }
}