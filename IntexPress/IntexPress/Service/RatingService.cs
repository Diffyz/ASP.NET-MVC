using DBUserCodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntexPress.Service
{
    public class RatingService
    {
        internal static void InsertRating(DBNews news, int evaluat)
        {
            using (SampleContext context = new SampleContext())
            {
                try
                {
                    foreach (var index in context.ratings)
                    {
                        if (UserService.user.userId == index.userId && news.newsId == index.newsId)
                            return;
                    }
                }
                catch { }
                Rating rating = new Rating();
                rating.userId = UserService.user.userId;
                rating.newsId = news.newsId;
                rating.rating = evaluat;
                context.ratings.Add(rating);
                context.SaveChanges();

                float sum = 0;
                float count = 0;

                foreach (var index in context.ratings)
                {
                    if (index.newsId == news.newsId)
                    {
                        sum += index.rating;
                        ++count;
                    }
                }
                float middleRating = sum / count;
                context.SaveChanges();

                foreach (var index in context.news)
                {
                    if (index.newsId == news.newsId)
                        index.rating = (float)Math.Round(middleRating,2);
                }

                news.rating = middleRating;             
                context.SaveChanges();
            }
        }
        internal static void SetRating()
        {
            if (UserService.user != null)
            {
                try
                {
                    using (SampleContext context = new SampleContext())
                    {
                        List<int> newsHaveRating = context.ratings.Where(x => x.userId == UserService.user.userId).Select(x => x.newsId).ToList();
                        foreach (var item in newsHaveRating)
                        {
                            UserService.user.newsListRating.Add(item);
                        }
                    }
                }
                catch { }
            }
        }
    }
}