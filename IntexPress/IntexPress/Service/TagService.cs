using DBUserCodeFirst;
using IntexPress.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntexPress.Service
{
    public static class TagService
    {
        public static void InsertTag(string _tag)
        {
            using (SampleContext context = new SampleContext())
            {
                try
                {
                    foreach (var tag in context.tags)
                    {
                        if (tag.tag == _tag)
                            return;
                    }
                }
                catch { }
                Tag newtag = new Tag();
                newtag.tag = _tag;
                context.tags.Add(newtag);
                context.SaveChanges();
            }
        }

        public static void FillTags()
        {
            using (SampleContext context = new SampleContext())
            {
                try
                {
                    New.tagsList.Clear();
                    foreach (var item in context.tags)
                    {
                        New.tagsList.Add(item.tag);
                    }
                }
                catch { }
            }
        }

        public static List<string> GetTagsList()
        {
            List<string> tagList = new List<string>();
            tagList.Clear();
            using (SampleContext context =new SampleContext())
            {
                foreach (var tag in context.tags)
                    tagList.Add(tag.tag);
            }
            return tagList;
        }

        internal static void InsertTags(List<string> tagList)
        {
            if (tagList.Count != 0)
            {
                using (SampleContext context = new SampleContext())
                {
                    foreach (var item in tagList)
                    {
                        var rowTag = context.tags.SingleOrDefault(x => x.tag == item);
                        if (rowTag == null)
                        {
                            Tag newTag = new Tag();
                            newTag.tag = item;
                            context.tags.Add(newTag);
                            context.SaveChanges();
                        }
                    }
                }
            }
        }

        internal static int GetIdTag(string param)
        {
            using(SampleContext context = new SampleContext())
            {
                var id =  context.tags.SingleOrDefault(x => x.tag == param).tagId;
                if (id > 0)
                    return id;
                
            }
            return -1;
        }
    }
}