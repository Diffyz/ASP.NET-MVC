using DBUserCodeFirst;
using IntexPress.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntexPress.Service
{
    public class CommentsService
    {
        internal static Stack<Comment> GetListComments(DBNews news)
        {
            Stack<Comment> commentsList = new Stack<Comment>();
            Dictionary<int, bool> CommentIdIsLike = new Dictionary<int, bool>();
            using (SampleContext context = new SampleContext())
            {
                try
                {
                    List<int> key = (from x in context.likes where x.userId == UserService.user.userId select x.commendId).ToList();
                    List<bool> value = (from x in context.likes where x.userId == UserService.user.userId select x.isLike).ToList();
                    for (int i = 0; i <= key.Count - 1; ++i)
                    {
                        CommentIdIsLike.Add(key[i], value[i]);
                    }
                }
                catch { }
                try
                {
                    foreach (var item in context.comments)
                    {
                       

                        if (news.newsId == item.newId)
                        {


                            if (CommentIdIsLike.ContainsKey(item.commentId))
                            {
                                item.isLikeComment = CommentIdIsLike[item.commentId];
                            }
                            else
                            {
                                item.isLikeComment = false;
                            }
                            commentsList.Push(item);

                        }
                    }
                    context.SaveChanges();

                }
                catch { }
                try
                {
                }
                catch { }
            }
            return commentsList;
        }
        internal static int GetCountComments(int? newsId=null)
        {
            int count = 0;
            using(SampleContext context = new SampleContext())
            {
                try
                {
                    if (newsId != null)
                    {
                        foreach (var item in context.comments)
                            if (item.newId == newsId)
                                ++count;
                    }
                    else
                    {
                        foreach (var item in context.comments)
                            if (item.newId == NewService.currentStateNews.newsId)
                                ++count;
                    }
                }
                catch { }
            }
            return count;
        }
        internal static int InserNew(int _userId, int _newsId, string text)
        {
            int commendId;
            using (SampleContext context = new SampleContext())
            {
                string userLogin = context.users.Where(x => x.userId == _userId).Select(x => x.login).First();
                Comment Comment = new Comment();
                Comment.newId = _newsId;
                Comment.text = text;
                Comment.userId = _userId;                           
                Comment.userLogin = userLogin;
                Comment.countLike = 0;

                context.comments.Add(Comment);
                context.SaveChanges();             
                commendId = (from x in context.comments where x.userId == _userId && x.newId == _newsId && x.text == text select x.commentId).ToList().LastOrDefault();                
            }
            return commendId;
        }
        internal static string GetLoginAuthor(int _userId)
        {
            string userLogin = string.Empty;
            using (SampleContext context = new SampleContext())
            {
                userLogin = context.users.Where(x => x.userId == _userId).Select(x => x.login).First();
            }
            return userLogin;
        }
    }
}