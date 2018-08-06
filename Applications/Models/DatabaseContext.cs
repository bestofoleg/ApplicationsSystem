using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Applications.Models
{
    public class DatabaseContext : DbContext
    {

        public DbSet<Application> Applications { get; set; }
        public DbSet<Item> Items { get; set; }

    }
}