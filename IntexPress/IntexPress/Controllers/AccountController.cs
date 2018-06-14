using DBUserCodeFirst;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using IntexPress.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

//https://www.youtube.com/watch?v=TYmR59zwwe0 drag'n'drop
namespace IntexPress.Controllers
{
    public class AccountController : Controller
    {
        public static bool wrongEmail = false;
        public static bool isSave = false;
        public static bool? newValid = null;
        public static bool? newExist = null;
        public static Stack<DBNews> myNews = null;
        public static DBUser author;
        public static int changeNewId;
        public static string title;
        private static bool isChangePicture = false;
        public static bool largeLenght;

        public ActionResult Index()
        {
            myNews = MyNews();
            if (UserService.user.admit == 4)
            {
                ViewBag.users = UserService.GetAllUsers();
            }      
            return View();
        }
        private Stack<DBNews> MyNews()
        {
            return NewService.GetNews(UserService.user.userId);
        }

        public ActionResult FullAccounts()
        {
            ViewBag.users = UserService.GetAllUsers();
            return View() ;
        }
        public ActionResult CreateNews()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult NewNews(New news)
        {

            if (news.title == null || news.category == null || news.text == null || news.description==null)
            {
                newValid = false;
                return RedirectToAction("Index", "Account");
            }
            else
            {
                int idUser = 0;
                if (JsonController.changeUser > 0)
                {
                    idUser = UserService.user.userId;
                    UserService.ChangeUser(JsonController.changeUser);
                }
                newValid = true;
                if (NewService.InsertNew(UserService.user, news))
                {
                   
                    TagService.InsertTags(JsonController.tagList);
                    NewsTagsService.InserNewPairs(JsonController.tagList,news.title);
                    myNews.Push(NewService.newStack.FirstOrDefault());
                    newExist = false;
                    if (idUser > 0)
                    {
                        UserService.ChangeUser(idUser);
                       idUser = 0;
                    }

                    return RedirectToAction("StartPage", "Home");
                }
                else newExist = true;
            }
            return RedirectToAction("Index", "Account");

        }
        public ActionResult UserRoom(string login)
        {
            DBUser author = UserService.ReturnClass(login);
            
            ViewBag.author = author;
            ViewBag.newsUser = NewService.GetNewsAuthor(author);
            return View();
        }

        public ActionResult ChangeUserData(User user)
        {
            UserService.ChangeUserParameters(user);       
            return View("Index");
        }
       
        [HttpPost]
        public ActionResult UploadFile(IEnumerable<HttpPostedFileBase> files)
        {
            foreach (var file in files)
                ImageService.UploadFile(file);
            return Json("file uploaded successfully");
        }

        public void ChangeNewsPicture(IEnumerable<HttpPostedFileBase> files)
        {

            foreach (var file in files)
                ImageService.UploadFile(file);
            isChangePicture = true;
        }
        [ValidateInput(false)]
        public ActionResult ChangeNews(int id)
        {
            DBNews a = new DBNews();
            using (SampleContext context = new SampleContext())
            {
               var row = context.news.SingleOrDefault(x => x.newsId == id);
                a.newsId = row.newsId;
                a.text =row.text;
                a.description = row.description;
                a.image = row.image;
                a.title = row.title;
            }
            title = a.title;
            ViewBag.id = a.newsId;
            ViewBag.text = a.text;
            ViewBag.description = a.description;
            ViewBag.title = a.title;
            ViewBag.image = a.image;
            changeNewId = id;
            return View();
        }

        [ValidateInput(false)]
        public ActionResult ConvertNews(string title,New news,string _text,string _description)
        {
            NewService.FillParameters();

            if (UserService.user.admit == 4)
            {
                ViewBag.users = UserService.GetAllUsers();
            }
            HttpContext.Response.AddHeader("X-XSS-Protection", "0");
            news.text = _text;
            news.description = _description;
            news.title = title;
            if(String.IsNullOrWhiteSpace(news.text) || String.IsNullOrWhiteSpace(news.description) || String.IsNullOrWhiteSpace(news.title))
            {
                return View("Index");
            }
            if (isChangePicture)
            {
                if (!string.IsNullOrEmpty(ImageService.image))
                {
                    news.picture = ImageService.image;
                    ImageService.image = string.Empty;
                }
                isChangePicture = !isChangePicture;
            }
            else
            {
                string picture;
                if ((picture=ImageService.GetPicture(changeNewId)) != "")
                {
                    news.picture = picture;
                }
            }
            
            NewService.ConvertNews(changeNewId,news);
            return View("Index");
        }

        public static void IsSave()
        {
            wrongEmail = false;
            isSave = true;
        }
        public static void NotSave()
        {
            wrongEmail = true;
            isSave = false;
        }
        public static void AddTags()
        {
            TagService.InsertTag("Библиотека");
            TagService.InsertTag("ДТП");
            TagService.InsertTag("Спа");
            TagService.InsertTag("Пенсионеры");
        }
        public static void AddCategor()
        {
            CategoryService.AddCategor("Образование");
            CategoryService.AddCategor("Люди");
            CategoryService.AddCategor("Авто");
            CategoryService.AddCategor("Технологии");
            CategoryService.AddCategor("Недвижимость");
        }
    }
}