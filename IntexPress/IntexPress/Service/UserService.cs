using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DBUserCodeFirst;
using IntexPress.Controllers;

namespace IntexPress.Service
{
    public static class UserService 
    {
        public static User user = null;
        public static string validationEmail = null;
        private const int maxLenghtParameter = 30;

        internal static void DeleteUser(int id)
        {
            using (SampleContext context = new SampleContext())
            {
                var userRow = context.users.SingleOrDefault(x => x.userId == id);
                if (userRow != null)
                {
                    context.users.Remove(userRow);
                    context.SaveChanges();
                }
            }
        }
        public static string GetShortLogin()
        {
            string shortName = string.Empty;
            for (int i = 0; i < 6; ++i)
                shortName += user.login[i];
            return shortName;
        }
        private static bool CheckUser(User _user)
        {
            using (SampleContext context = new SampleContext())
            {
                try
                {
                    var userRow = context.users.SingleOrDefault(u => u.email == _user.email || u.login == _user.login);
                    if (userRow != null)
                        return true;
                }
                catch { }
            }
            return false;
        }
        internal static bool InserNew(User user)
        {
            if (!CheckUser(user))
            {
                DBUser dbUser = new DBUser();
                dbUser.email = user.email;
                dbUser.login = user.login;
                dbUser.password = user.password;
                dbUser.validationEmail = user.validationEmail;
                dbUser.admit = user.admit;
                dbUser.userPicture = user.picture;
                dbUser.phone = user.phone;
                dbUser.sex = user.sex;
                using (SampleContext context = new SampleContext())
                {
                    context.users.Add(dbUser);
                    context.SaveChanges();
                    UserService.user = user;
                    UserService.user.userId = dbUser.userId;
                    return true;
                }
            }
            else
            {
                using (SampleContext context =new SampleContext())
                {
                    var index = context.users.SingleOrDefault(u=>u.email==user.email);

                    if (index != null) { 
                            UserService.user = new User();
                            UserService.user.userId = index.userId;
                            UserService.user.admit = index.admit;
                            UserService.user.countLikeAuthor = index.countLike;
                            UserService.user.email = index.email;
                            UserService.user.login = index.login;
                            UserService.user.password = index.password;
                            UserService.user.validationEmail = index.validationEmail;
                            UserService.user.picture = index.userPicture;
                            UserService.user.phone = index.phone;
                            UserService.user.sex = index.sex;
                            UserService.user.ratingAt = index.ratingAt;
                            return true;
                        }
                }
            }
            return false;
        }

      

        internal static void ChangeUserParameters(User user)
        {
            
            if (!String.IsNullOrWhiteSpace(user.phone))
            {
                ChangePhone(user.phone);
            }
            if (!String.IsNullOrWhiteSpace(user.sex))
            {
                ChangeSex(user.sex);
            }
            if (!String.IsNullOrWhiteSpace(user.login))
            {
                if (user.login.Length > maxLenghtParameter)
                    AccountController.largeLenght = true;
                else if (ChangeLogin(user.login))
                {
                    AccountController.largeLenght = false;
                    AccountController.IsSave();
                }
                else
                    AccountController.NotSave();
            }
            if (!String.IsNullOrWhiteSpace(user.password))
            {
                if (user.login.Length > maxLenghtParameter)
                    AccountController.largeLenght = true;
                else
                {
                    AccountController.largeLenght = false;
                    ChangePassword(user.password);
                }
            }
        }

        internal static void ChangeUser(int id)
        {
            using(SampleContext context = new SampleContext())
            {
                var userRow = context.users.SingleOrDefault(u => u.userId == id);
                if (userRow != null) {
                    UserService.user.userId= userRow.userId;
                    UserService.user.admit = userRow.admit;
                    UserService.user.countLikeAuthor = userRow.countLike;
                    UserService.user.email = userRow.email;
                    UserService.user.login = userRow.login;
                    UserService.user.password = userRow.password;
                    UserService.user.validationEmail = userRow.validationEmail;
                    UserService.user.picture = userRow.userPicture;
                    UserService.user.phone = userRow.phone;
                    UserService.user.sex = userRow.sex;
                    UserService.user.ratingAt = userRow.ratingAt;

                }
            }
        }

        internal static void ChangePhone(string phone)
        {
            using(SampleContext context = new SampleContext())
            {
                var userRow = context.users.SingleOrDefault(u => u.userId == user.userId);
                if (userRow != null)
                {
                    user.phone = phone;
                    userRow.phone = phone;
                    context.SaveChanges();
                }
            }
        }

        internal static void ChangeSex(string sex)
        {
            if (sex == "Женский")
                sex = "Woman";
            if (sex == "Мужской")
                sex = "Male";
            using (SampleContext context = new SampleContext())
            {
                var userRow = context.users.SingleOrDefault(u => u.userId == user.userId);
                if (userRow != null)
                {
                    user.sex = sex;
                    userRow.sex = sex;
                    context.SaveChanges();
                }
            }
        }

        public static Stack<DBUser> GetAllUsers()
        {
            Stack<DBUser> users = new Stack<DBUser>();
            
            using (SampleContext context = new SampleContext())
            {
                try{
                    foreach (var item in context.users)
                    {
                        users.Push(item);
                    }
                }
                catch { }
            }
            return users;
        }

        internal static void ChangeRating(float rating)
        {
            using (SampleContext context = new SampleContext())
            {
                var userRow = context.users.SingleOrDefault(u => u.userId == user.userId);
                if (userRow != null)
                {
                    userRow.ratingAt = rating;
                    user.ratingAt = rating;
                    context.SaveChanges();
                }
}
        }

        internal static int GetCountLikeAuthor(int userId)
        {
           using(SampleContext context = new SampleContext())
            {
                var userRow = context.users.SingleOrDefault(x => x.userId == userId);
                if (userRow != null)
                    return userRow.countLike;              
            }
            return -1;
        }
        internal static void ChangePassword(string password)
        {
            using (SampleContext context = new SampleContext())
            {
                var rowUser = context.users.SingleOrDefault(u => u.email == user.email);
                if (rowUser != null)
                {
                    rowUser.password = password;
                    user.password = password;
                    context.SaveChanges();
                }             
            }        
        }
        internal static bool CheckLoginPassword(User _user)
        {
            using (SampleContext context = new SampleContext())
            {
                try
                {
                    var user = context.users.SingleOrDefault(u=>u.login== _user.login && u.password== _user.password);

                    if (user != null)
                    {
                        UserService.user = new User();
                        UserService.user.login = user.login;
                        UserService.user.password = user.password;
                        UserService.user.email = user.email;
                        UserService.user.userId = user.userId;
                        UserService.user.validationEmail = user.validationEmail;
                        UserService.user.admit = user.admit;
                        UserService.user.picture = user.userPicture;
                        UserService.user.phone = user.phone;
                        UserService.user.sex = user.sex;
                        UserService.user.ratingAt = user.ratingAt;

                        return true;
                    }               
                }
                catch { }
                context.SaveChanges();
            }
          
            return false;
        }
        internal static void ChangeBlock(int id, bool isBlock)
        {
            using (SampleContext context = new SampleContext())
            {
                var rowUser = context.users.SingleOrDefault(x => x.userId == id);
                if (rowUser != null)
                {
                    rowUser.isBlock = isBlock;
                    context.SaveChanges();
                }

                
            }
        }
        internal static void ChangeAdmit(int id , int admit)
        {
            using (SampleContext context = new SampleContext())
            {
                if (user.userId == id)
                {
                    user.admit = admit;
                }
                var rowUser = context.users.SingleOrDefault(x => x.userId == id);
                if (rowUser != null)
                {
                    rowUser.admit = admit;
                    context.SaveChanges();
                }
            }
        }   

        private static void ChangeLoginNews()
        {
            using (SampleContext context = new SampleContext())
            {
                var newsList = context.news.Where(u => u.userId == user.userId).Select(u => u).ToList();
                if (newsList.Count > 0)
                {
                    foreach (var index in newsList)
                        index.nameAuthor = user.login;
                    context.SaveChanges();
                }
            }
        }
        private static void ChangeLoginComment()
        {
            using(SampleContext context = new SampleContext())
            {
                var commentList = context.comments.Where(u => u.userId == user.userId).Select(u => u).ToList();
                if (commentList.Count > 0)
                {
                    foreach (var index in commentList)
                        index.userLogin = user.login;
                    context.SaveChanges();
                }
            }
        }
        internal static bool ChangeLogin(string login)
        {
            using (SampleContext context = new SampleContext())
            {
                var userLoginRow = context.users.SingleOrDefault(u => u.login == login);
                if (userLoginRow == null)
                {
                    var userEmailRow = context.users.SingleOrDefault(u => u.email == UserService.user.email);
                    if (userEmailRow != null)
                    {
                        userEmailRow.login = login;
                        context.SaveChanges();
                        user.login = login;
                        ChangeLoginNews();
                        ChangeLoginComment();
                        NewService.GetNews();
                        return true;
                    }
                }
            }
            return false ;
        }
        internal static void ValidationEmailTrue()
        {
            using (SampleContext context = new SampleContext())
            {
                var user = context.users.SingleOrDefault(u => u.email == validationEmail);
                if (user != null)
                {
                    user.validationEmail = true;
                    UserService.user.login = user.login;
                    UserService.user.password = user.password;
                    UserService.user.email = user.email;
                    UserService.user.validationEmail = user.validationEmail;
                    UserService.user.userId = user.userId;
                    UserService.user.admit = user.admit;
                    UserService.user.picture = user.userPicture;
                    UserService.user.sex = user.sex;
                    UserService.user.phone = user.phone;
                    UserService.user.ratingAt = user.ratingAt;

                    context.SaveChanges();
                }
            }
           
        }
       
        internal static DBUser ReturnClass(string login)
        {
            using (SampleContext context = new SampleContext())
            {
                var item = context.users.SingleOrDefault(u=>u.login==login);
                if (item != null)
                    return item;
            }
            return new DBUser();
        }
    }
}