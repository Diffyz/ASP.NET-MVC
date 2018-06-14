using DBUserCodeFirst;
using IntexPress.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntexPress.Controllers
{
    public class JsonController : Controller
    {
        public static float SearchNewsRating;
        public static float numberNews;
        public static int countComments=0;
        public static List<string> tagList = new List<string>();
        public static List<string> searchTagList = new List<string>();
        public static int changeUser = 0;

        #region Rating
        [HttpPost]
        public JsonResult GetRating()
        {
            UserService.user.newsListRating.Add(NewService.newStack.ElementAt(Convert.ToInt32(numberNews)).newsId);         
            return Json(NewService.newStack.ElementAt(Convert.ToInt32(numberNews)).rating);
        }

        [HttpPost]
        public void SetRating(string parameters)
        {
            String[] words = parameters.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            //words[0] this is number element stackNews
            //words[1] this is digit rating
            
            RatingService.InsertRating(
                NewService.newStack.ElementAtOrDefault(Convert.ToInt32(words[0])),
                Convert.ToInt32(words[1]));
            numberNews =Convert.ToInt32(words[0]);
        }
        #endregion

        #region Comment
        [HttpPost]
        public JsonResult CreateComment(string parameters)
        {       
            //JsonRequestBehavior.AllowGet allow http get Json requests
            String[] words = parameters.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //words[0] userId
            //words[1] newsId

            const int countSpace = 2;
            int userId = Convert.ToInt32(words[0]);
            int newsId = Convert.ToInt32(words[1]);
            parameters = parameters.Substring(words[0].Length+words[1].Length + countSpace);
            var idLike = CommentsService.InserNew(userId, newsId, parameters);
            string commentLoginAuthor = CommentsService.GetLoginAuthor(userId);
            int countLikeAuthor = UserService.GetCountLikeAuthor(userId);
            parameters = idLike + " " + commentLoginAuthor + " " + countLikeAuthor + " " + parameters;
            return Json(parameters, JsonRequestBehavior.AllowGet);         
        }

        public JsonResult GetNewNews(string parameters)
        {
            List<Comment> allComments = new List<Comment>();
            List<Comment> actualComments = new List<Comment>();
            if (UserService.user != null)
            {
               
                int newCountComments = CommentsService.GetCountComments(NewService.currentStateNews.newsId);
                if (countComments < newCountComments)
                {

                    using (SampleContext context = new SampleContext())
                    {
                        allComments = context.comments.Where(x => x.newId == NewService.currentStateNews.newsId).ToList();
                    }
                    int i = newCountComments - countComments;
                    for (; i != 0; --i)
                    {
                        actualComments.Add(allComments[allComments.Count - i]);
                    }
                    countComments = newCountComments;
                }
            }

            return Json(actualComments, JsonRequestBehavior.AllowGet);         
        }
        #endregion

        #region Like
        public void ChangeLikeLess(string parameters)
        {
            int commentId = Convert.ToInt32(parameters);

            using (SampleContext context = new SampleContext())
            {
                var rowComment = context.comments.SingleOrDefault(x => x.commentId == commentId);
                if(rowComment != null)
                {
                    var rowUser = context.users.SingleOrDefault(x => x.userId == rowComment.userId);
                    --rowUser.countLike;
                    --rowComment.countLike;
                    context.SaveChanges();
                }
                var rowLike = context.likes.SingleOrDefault(x=>x.userId==UserService.user.userId && x.commendId== commentId);

                if(rowLike != null)
                {                
                    rowLike.isLike = false;
                    context.SaveChanges();
                }
                else
                {
                    LikeService.InsertNew(commentId,UserService.user.userId,false);
                }
            }
        }
        public void ChangeLikeMore(string parameters)
        {
            int commentId = Convert.ToInt32(parameters);
            using (SampleContext context = new SampleContext())
            {
                var rowComment = context.comments.SingleOrDefault(x => x.commentId == commentId);

                if (rowComment != null)
                {
                    var rowUser = context.users.SingleOrDefault(x => x.userId == rowComment.userId);
 
                    ++rowUser.countLike;
                    ++rowComment.countLike;
                    context.SaveChanges();
                }

                var rowLike = context.likes.SingleOrDefault(x => x.userId == UserService.user.userId && x.commendId == commentId);

                if (rowLike != null)
                {
                    rowLike.isLike = true;
                    context.SaveChanges();
                }
                else
                {
                    LikeService.InsertNew(commentId, UserService.user.userId,true);
                }
            }
        }
        #endregion

        #region News

        [HttpPost]
        public void DeletePicture(string newsId)
        {
            NewService.DeleteImage(Convert.ToInt32(newsId));
        }

        [HttpPost]
        public void DeleteNew(string parameters)
        {
            String[] words = parameters.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //words[0] newsId
            NewService.DeleteNew(Convert.ToInt32(words[0]));            
        }
        #endregion

        #region SearchNews
        public void SetSearchNews(string param)
        {
            try
            {
                SearchNewsRating = Convert.ToSingle(param);
            }
            catch { }
                             
        }
        #endregion

        #region Users
        public void ChangeAdmit(string parameters)
        {
            String[] words = parameters.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            UserService.ChangeAdmit(Convert.ToInt32(words[0]), Convert.ToInt32(words[1]));
        }
        public void UnBlockUser(string parameters)
        {
            UserService.ChangeBlock(Convert.ToInt32(parameters), false);
        }
        public void BlockUser(string parameters)
        {
            UserService.ChangeBlock(Convert.ToInt32(parameters), true);
        }
        public void DeleteUser(string parameters)
        {
            UserService.DeleteUser(Convert.ToInt32(parameters));
        }
        public void ChangeSession(string param)
        {
            changeUser = Convert.ToInt32(param);
        }
        public void ChangeSex(string param)
        {
            UserService.ChangeSex(param);
        }
        public void ChangeRating(string param)
        {
            UserService.ChangeRating(Convert.ToSingle(param));
            NewService.GetNews(Convert.ToSingle(param));
        }
        #endregion

        #region Tags
        public JsonResult getTagForSearch()
        {
            return Json(TagService.GetTagsList(), JsonRequestBehavior.AllowGet);
        }
        public void PushTag(string param)
        {
            var find = tagList.Find(x => x == param);
            if (find == null)
            {
                tagList.Add(param);
            }
        }
        public void RemoveTag(string param)
        {
            tagList.Remove(param);
        }
        public void PushTagSearch(string param)
        {
            var find = searchTagList.Find(x => x == param);
            if (find == null)
            {
                searchTagList.Add(param);
            }
        }
        public void RemoveTagSearch(string param)
        {
            searchTagList.Remove(param);
        }
        public void RemoveTagNews(string param)
        {
           List<string> tagList = HomeController.newsIdTagsPairs[AccountController.changeNewId];
            tagList.Remove(param);
            HomeController.newsIdTagsPairs[AccountController.changeNewId] = tagList;
            NewsTagsService.Remove(AccountController.changeNewId,param);
        }
        public void InsertNewPair(string param)
        {
            List<string> tagList = HomeController.newsIdTagsPairs[AccountController.changeNewId];
            tagList.Add(param);
            HomeController.newsIdTagsPairs[AccountController.changeNewId] = tagList;
            TagService.InsertTag(param);
            NewsTagsService.InserNewPair(AccountController.changeNewId,TagService.GetIdTag(param));
        }
        #endregion

    }
}