using DBUserCodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntexPress.Service
{
    public static class CategoryService
    {
        public static List<string> categoryList = new List<string>();

        public static void FillCategory()
        {
            using (SampleContext context = new SampleContext())
            {
                try
                {
                    categoryList.Clear();
                    foreach (var item in context.categories)
                    {
                        categoryList.Add(item.category);
                    }
                }
                catch { }
            }
        }
        public static void AddCategor(string _category)
        {
            using (SampleContext context = new SampleContext())
            {
                try
                {
                    foreach (var item in context.categories)
                    {
                        if (item.category.Equals(_category))
                        {
                            return;
                        }
                    }
                    categoryList.Add(_category);
                    Category Category = new Category();
                    Category.category = _category;
                    context.categories.Add(Category);
                    context.SaveChanges();
                }
                catch { }
            }
        }
    }
}