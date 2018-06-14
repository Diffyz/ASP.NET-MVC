using DBUserCodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntexPress.Service
{
    public class LikeService
    {
        internal static void InsertNew(int commendId, int userId,bool state=false)
        {
            using (SampleContext context = new SampleContext())
            {
                Like like = new Like();
                like.commendId = commendId;
                like.userId = userId;
                like.isLike = state;            
                context.likes.Add(like);
                context.SaveChanges();
            }
        }
    }
}