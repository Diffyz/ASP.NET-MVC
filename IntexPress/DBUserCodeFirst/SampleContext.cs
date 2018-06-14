using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DBUserCodeFirst
{
    public class SampleContext : DbContext
    {
        public SampleContext() : base("IntexPress") {}
        public DbSet<DBUser> users { get; set; }
        public DbSet<Tag> tags { get; set; }
        public DbSet<DBNews> news { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Rating> ratings { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Like> likes { get; set; }
        public DbSet<NewsTags> newsTags { get; set; }
    }
}
