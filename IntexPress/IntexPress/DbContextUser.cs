using DBUserCodeFirst;
using IntexPress.Models;
using Nemiro.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TweetSharp;

namespace IntexPress
{
    public static class DbContextUser
    {
        public static User user = null;

        public static List<string> userLoginList = new List<string>();
        private static bool CheckUser(User _user)
        {
            using (SampleContext context = new SampleContext())
            {
                foreach(var user in context.users)
                {
                    if (_user.email == user.email)
                        return true;
                }
                foreach (var user in context.users)
                {
                    if (_user.login == user.login)
                        return true;
                }
            }
            return false;
        }
        public static bool InserNewUser(User user)
        {
            if (!CheckUser(user))
            {
                DBUser dbUser = new DBUser();
                dbUser.email = user.email;
                dbUser.login = user.login;
                dbUser.password = user.password;
                dbUser.validationEmail = user.validationEmail;
                SampleContext context = new SampleContext();
                context.users.Add(dbUser);
                context.SaveChanges();
                DbContextUser.user = user;
                return true;
            }
            else return false;
        }
        public static bool CheckLoginPassword(User _user)
        {
            using (SampleContext context = new SampleContext())
            {
                foreach (var user in context.users)
                {
                    if (_user.login == user.login && _user.password == user.password)
                    {
                        DbContextUser.user = _user;
                        return true;
                    }
                }              
            }
            return false;
        }
        public static void ValidationTrue()
        {
            bool userFound = false;
            using (SampleContext context = new SampleContext())
            {               
                foreach (var user in context.users)
                {
                    if (DbContextUser.user.email == user.email)
                    {
                        user.validationEmail = true;
                        DbContextUser.user.validationEmail = true;
                        userFound = true;                      
                        break;
                    }
                }
                if (userFound)
                    context.SaveChanges();
            }
           
        }
    }
}