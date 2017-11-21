using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WMTest.Models
{
    public class Context : DbContext
    {
        public Context() : base("name=WMTestDBConnectionString")
        {
          
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<FirstTaskModel> FirstTaskModels { set; get; }
        public DbSet<SecondTaskModel> SecondTaskModels { set; get; }
        public DbSet<ThirdTaskModel> ThirdTaskModels { set; get; }
    }

    //
    public class DBInitializer : DropCreateDatabaseAlways<Context>
    {
        protected override void Seed(Context context)
        {
            for (int i = 1; i < 10; i++)
            {
                context.FirstTaskModels.Add(new FirstTaskModel() { Name = string.Format("Model{0}", i) });
                context.SecondTaskModels.Add(new SecondTaskModel() { Value = i });
                context.ThirdTaskModels.Add(new ThirdTaskModel() { Balance = i * 1000 });
            }
            base.Seed(context);
        }
    }
}