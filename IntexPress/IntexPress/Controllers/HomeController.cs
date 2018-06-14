using DBUserCodeFirst;
using IntexPress.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace IntexPress.Controllers
{
    public class HomeController : Controller
    {
        public static bool fillDataLists = false;
        public static bool isFind = false;
        public static Dictionary<int, List<string>> newsIdTagsPairs = new Dictionary<int, List<string>>();
        public static Dictionary<int, string> newsUserPicture = new Dictionary<int, string>();

        public ActionResult StartPage()
        {
            ViewBag.tags= TagService.GetTagsList();
            if (!fillDataLists)
            {
                fillDataLists = !fillDataLists;
                FillData();       
            }
            AccountController.wrongEmail = false;
            AccountController.isSave = false;
            AccountController.largeLenght = false;
            NewService.SetLike();
            ViewBag.search = NewService.newStack;
            NewService.FillParameters();
            return View();       
        }

        private void FillData()
        {
            CategoryService.FillCategory();
            TagService.FillTags();
            AccountController.AddCategor();
            AccountController.AddTags();
            NewService.GetNews();
            RatingService.SetRating();
            CommentsService.GetCountComments();
        }

        public ActionResult Top()
        {
            ViewBag.tags = TagService.GetTagsList();
            var a = NewService.newStack.OrderBy(x => x.rating).ToList();
            a.Reverse();
            ViewBag.search = a;
            return View();
        }

        public ActionResult SearchCategory(string search)
        {
            ViewBag.search = NewService.SearchCategoryNews(search);
            ViewBag.tags = TagService.GetTagsList();
            ViewBag.isSearch = true;
            return View("StartPage");
        }

        public ActionResult Search(string search="")
        {
            ViewBag.search =NewService.FindSuitableNews(search);
            ViewBag.tags= TagService.GetTagsList();
            ViewBag.isSearch = true;
            return View("StartPage");
        }      

        public ActionResult SearchTag(string search)
        {
            ViewBag.search =NewsTagsService.FindSuitableNewsTag(search);
            ViewBag.tags = TagService.GetTagsList();
            ViewBag.isSearch = true;
            return View("StartPage");
        }

        public ActionResult Auth()
        {
            return View();
        }

        public ActionResult Restart()
        {
            ViewBag.tags = TagService.GetTagsList();
            UserService.user = null;
            ViewBag.search = NewService.newStack;
            return View("StartPage");
        }

        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files)
        {
            foreach (var file in files)
            {
                string filePath = Guid.NewGuid() + Path.GetExtension(file.FileName);
                file.SaveAs(Path.Combine(Server.MapPath("~/UploadedFiles"), filePath));
                //Here can write code for save this information in database
            }
            return Json("file uploaded successfully");
        }

        [ValidateInput(false)]
        public ActionResult ReadNews(int newsId,int numberNews)
        {
            Stack<Comment> comments = new Stack<Comment>();
            NewService.currentStateNews = NewService.GetNew(newsId);
            comments =  CommentsService.GetListComments(NewService.currentStateNews);
            JsonController.countComments = CommentsService.GetCountComments();


            List<int> countLikeList = new List<int>();
            int rating=0;
            using (SampleContext context = new SampleContext())
            {
                foreach (var item in comments)
                {
                  
                    var rowUser = context.users.SingleOrDefault(x => x.userId == item.userId);
                    if (rowUser != null)
                    {
                        if (rowUser.countLike >= 0)
                            countLikeList.Add(rowUser.countLike);
                    }
                    else
                    {
                        countLikeList.Add(0);
                    }
                                          
                }
                try
                {
                    int countLikeUser = context.users.SingleOrDefault(x => x.userId == NewService.currentStateNews.userId).countLike;
                    rating = countLikeUser;
                }
                catch { }
            }
           
            ViewBag.countLike = rating;
            ViewBag.usersCountLike = countLikeList;
            ViewBag.comment = comments;
            ViewBag.news = NewService.currentStateNews;
            ViewBag.numberNews = numberNews;
            return View();
        }
    }
}