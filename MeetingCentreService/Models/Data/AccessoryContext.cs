using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace MeetingCentreService.Models.Data
{
    public class AccessoryContext : DbContext
    {
        public DbSet<Entities.AccessoriesCategory> CategorySet { get; set; }
        public DbSet<Entities.Accessory> AccessorySet { get; set; }
        public DbSet<Entities.Accessory.AccessoryHandOutOccurance> HandOuts { get; set; }

        public AccessoryContext() : base() { }

        // Initializer set in MainWindow.xaml.cs
        public class AccessoriesInitializer : DropCreateDatabaseIfModelChanges<AccessoryContext>
        {
            protected override void Seed(AccessoryContext context)
            {
                try
                {
                    context.CategorySet.Add(new Entities.AccessoriesCategory() { Name = "Drinks", Icon = "\xEE64" });
                    context.CategorySet.Add(new Entities.AccessoriesCategory() { Name = "Snacks", Icon = "\xED56" });
                    context.CategorySet.Add(new Entities.AccessoriesCategory() { Name = "Utilities", Icon = "\xE805" });
                    context.SaveChanges();
                } catch (System.Data.Common.DbException e) {
                    Console.WriteLine(e.Message);
                }
                base.Seed(context);
            }
        }
    }
}
