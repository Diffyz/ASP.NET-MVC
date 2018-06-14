using DBUserCodeFirst;
using IntexPress.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace IntexPress.Service
{
    public class NewsTagsService
    {
        internal static void InserNewPairs(List<string> tagList, string title)
        {
            if (tagList.Count != 0)
            {
                using(SampleContext context = new SampleContext())
                {
                    int newId = context.news.SingleOrDefault(x=>x.title==title).newsId;
                    NewsTags newTagNews= new NewsTags();
                    foreach (var item in tagList)
                    {
                        var tagId = context.tags.SingleOrDefault(x=>x.tag==item).tagId;
                        newTagNews.newsId = newId;
                        newTagNews.tagId = tagId;
                        context.newsTags.Add(newTagNews);
                        context.SaveChanges();
                    }
                }
                JsonController.tagList.Clear();
            }
        }
        public static Stack<DBNews> FindSuitableNewsTag(string word)
        {
            Stack<DBNews> newsAll = new Stack<DBNews>();
            using (SampleContext context = new SampleContext())
            {
                int idTag = context.tags.SingleOrDefault(u => u.tag == word).tagId;
                List<int> idNewsList = context.newsTags.Where(u => u.tagId == idTag).Select(x => x.newsId).ToList();
                foreach (var i in idNewsList)
                    newsAll.Push(context.news.SingleOrDefault(u => u.newsId == i));

            }
            return newsAll;
        }
        internal static void Remove(int changeNewId, string param)
        {
            using(SampleContext context = new SampleContext())
            {
                var idTag = context.tags.SingleOrDefault(u => u.tag == param).tagId;
                var newsTag = context.newsTags.SingleOrDefault(u => u.newsId == changeNewId && u.tagId == idTag);
                if (newsTag != null)
                {
                    context.newsTags.Remove(newsTag);
                    context.SaveChanges();
                }
            }
        }

        internal static void InserNewPair(int newId, int tagId)
        {
            using (SampleContext context = new SampleContext())
            {
                var idTag = context.newsTags.SingleOrDefault(u => u.newsId == newId && tagId == u.tagId);
                if (idTag == null)
                {
                    NewsTags newsTags = new NewsTags();
                    newsTags.newsId = newId;
                    newsTags.tagId = tagId;
                    context.newsTags.Add(newsTags);
                    context.SaveChanges();
                }
               
            }
        }
    }
}