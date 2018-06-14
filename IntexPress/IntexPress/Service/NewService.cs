using DBUserCodeFirst;
using IntexPress.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace IntexPress.Service
{
    public class NewService
    {
        public static Stack<DBNews> newStack = new Stack<DBNews>();
        public static DBNews currentStateNews;

        internal static void GetNews()
        {
            newStack.Clear();
            using (SampleContext context = new SampleContext())
            {
                try
                {
                    foreach (var i in context.news)                      
                        newStack.Push(i);
                }
                catch { }
            }
        }

        internal static void SetLike()
        {
            if (newStack.Count > 0)
            {
                using (SampleContext context = new SampleContext())
                {
                    try
                    {
                        foreach (var item in newStack)
                            item.countLikeAuthor = context.users.SingleOrDefault(x => x.userId == item.userId).countLike;
                        
                    }
                    catch { }
                }
            }
        }

        internal static void FillParameters()
        {
           HomeController.newsIdTagsPairs.Clear();
            HomeController.newsUserPicture.Clear();
            using (SampleContext context = new SampleContext())
            {
                foreach (var news in NewService.newStack)
                {
                    List<string> tagList = new List<string>();

                    var newsTagsList = context.newsTags.Where(x => x.newsId == news.newsId).Select(u => u.tagId).ToList();

                    if (newsTagsList.Count >= 0)
                    {
                        for (var i = 0; i < newsTagsList.Count; ++i)
                        {
                            int id = newsTagsList[i];
                            tagList.Add(context.tags.SingleOrDefault(u => u.tagId == id).tag);
                        }
                        HomeController.newsIdTagsPairs.Add(news.newsId, tagList);
                    }
                    string userPicture;
                    try
                    {
                        userPicture = context.users.SingleOrDefault(x => x.userId == news.userId).userPicture;
                    }
                    catch
                    {
                        userPicture = "http://localhost:52310/Content/img/user.png";
                    }
                    HomeController.newsUserPicture.Add(news.newsId, userPicture);
                }
            }
        }

      

        internal static Stack<DBNews> GetNews(int id)
        {
            Stack<DBNews> myNews = new Stack<DBNews>();
            using (SampleContext context = new SampleContext())
            {
                try
                {
                    var news = context.news.Where(u => u.newsId == id).Select(u => u).ToList();
                    foreach (var item in news)
                        myNews.Push(item);
                }
                catch { }
            }
            return myNews;
        }

        internal static DBNews GetNew(int id)
        {
            DBNews news = new DBNews();
            using (SampleContext context = new SampleContext())
            {
                try
                {
                    foreach (var item in context.news)
                        if (item.newsId == id)
                        {
                            news = item;
                        }
                }
                catch { }
            }
                return news;
        }

        internal static dynamic SearchCategoryNews(string search)
        {
            Stack<DBNews> news = new Stack<DBNews>();
            List<int> idTagsSearch = new List<int>();

            if (JsonController.searchTagList.Count > 0)
            {
                using (SampleContext context = new SampleContext())
                {
                    foreach (var tag in JsonController.searchTagList)
                        idTagsSearch.Add(context.tags.SingleOrDefault(u => u.tag == tag).tagId);
                }
            }

            foreach (var item in newStack)
            {
                if (item.categories.Contains(search))
                    if (item.rating >= JsonController.SearchNewsRating)
                    {
                        if (idTagsSearch.Count > 0)
                        {
                            List<int> idTagsNewsList = new List<int>();
                            using (SampleContext context = new SampleContext())
                            {
                                //news have tags
                                idTagsNewsList = context.newsTags.Where(x => x.newsId == item.newsId).Select(u => u.tagId).ToList();
                            }
                            bool suitubleNews = true;
                            foreach (var el in idTagsSearch)
                            {
                                if (!idTagsNewsList.Contains(el))
                                {
                                    suitubleNews = false;
                                    break;
                                }
                            }
                            if (suitubleNews == true)
                                news.Push(item);

                        }
                        else
                        {
                            news.Push(item);
                        }
                    }
            }
            JsonController.searchTagList.Clear();
            return news;
        }



        public static Stack<DBNews> FindSuitableNews(string search)
        {
            Stack<DBNews> news = new Stack<DBNews>();
            List<int> idTagsSearch = new List<int>();

            if (JsonController.searchTagList.Count > 0)
            {
                using (SampleContext context = new SampleContext())
                {
                    foreach (var tag in JsonController.searchTagList)
                        idTagsSearch.Add(context.tags.SingleOrDefault(u => u.tag == tag).tagId);
                }

            }

            foreach (var item in NewService.newStack)
            {
                if (item.title.Contains(search) || item.text.Contains(search) || item.title.Contains(search) || item.description.Contains(search) || item.categories.Contains(search))
                    if (item.rating >= JsonController.SearchNewsRating)
                    {
                        if (idTagsSearch.Count > 0)
                        {
                            List<int> idTagsNewsList = new List<int>();
                            using (SampleContext context = new SampleContext())
                            {
                                //news have tags
                                idTagsNewsList = context.newsTags.Where(x => x.newsId == item.newsId).Select(u => u.tagId).ToList();
                            }
                            bool suitubleNews = true;
                            foreach (var el in idTagsSearch)
                            {
                                if (!idTagsNewsList.Contains(el))
                                {
                                    suitubleNews = false;
                                    break;
                                }
                            }
                            if (suitubleNews == true)
                                news.Push(item);

                        }
                        else
                        {
                            news.Push(item);
                        }
                    }
            }
            JsonController.searchTagList.Clear();
            return news;
        }

        internal static void DeleteImage(int id)
        {
            using(SampleContext context = new SampleContext())
            {
                var newsRow = context.news.SingleOrDefault(x=>x.newsId== id);
                if (newsRow != null)
                {
                    newStack.SingleOrDefault(u => u.newsId == id).image=null;
                    newsRow.image = null;
                    context.SaveChanges();
                }
            }
        }

        internal static bool InsertNew(User user, New news)
        {
            using (SampleContext context = new SampleContext())
            {
                try
                {
                    foreach (var i in context.news)
                        if (i.title == news.title)
                            return false;
                }
                catch { }

                DBUser rowdBUser = context.users.SingleOrDefault(x => x.userId == user.userId);

                DBNews newNews = new DBNews();
                newNews.userId = user.userId;
                newNews.title = news.title;
                newNews.categories = news.category;
                newNews.text = news.text;
                newNews.nameAuthor = UserService.user.login;
                newNews.countLikeAuthor = rowdBUser.countLike;
                newNews.description = news.description;
                if (!string.IsNullOrEmpty(ImageService.image))
                    newNews.image = ImageService.image;
                newStack.Push(newNews);

                context.news.Add(newNews);
                context.SaveChanges();
                ImageService.image = string.Empty;
            }
            return true;
        }

        internal static List<DBNews> GetNewsAuthor(DBUser author)
        {
            using(SampleContext context = new SampleContext())
            {
                var tmp = context.news.Where(u => u.userId == author.userId).Select(u => u).ToList();
                if (tmp != null)
                    return tmp;
            }
            return new List<DBNews>();
        }

        internal static void ConvertNews(int id,New news)
        {
            using (SampleContext context = new SampleContext())
            {
                var rowNews = context.news.SingleOrDefault(x => x.newsId == id);
                if (rowNews != null)
                {
                    rowNews.title = news.title;
                    rowNews.description = news.description;
                    rowNews.text = news.text;
                    rowNews.image = news.picture;
                    context.SaveChanges();
                    var stackNews = newStack.SingleOrDefault(x => x.newsId == id);
                    stackNews.title = news.title;
                    stackNews.description = news.description;
                    stackNews.text = news.text;
                    stackNews.image = news.picture;
                }

            }

        }

        internal static void DeleteNew(int id)
        {
            int count1 = newStack.Count;
            using (SampleContext context = new SampleContext())
            {
                DBNews rowNews = context.news.SingleOrDefault(x => x.newsId == id);
                if (rowNews != null)
                {
                    context.news.Remove(rowNews);
                    context.SaveChanges();

                    List<DBNews> newsList = new List<DBNews>();

                    foreach (var item in newStack)
                    {
                        newsList.Add(item);
                    }
                    int countstart = newsList.Count;
                    newsList.RemoveAll(x=>x.newsId==rowNews.newsId);

                    newStack.Clear();
                    foreach (var item in newsList)
                        newStack.Push(item);
                    int count2 = newStack.Count;
                }
            }
        }

        internal static void GetNews(float rating)
        {
            newStack.Clear();
            using (SampleContext context = new SampleContext())
            {
                try
                {
                    foreach (var item in context.news)
                        if (item.rating >= rating)
                            newStack.Push(item);
                }
                catch { }
            }
        }
    }
}