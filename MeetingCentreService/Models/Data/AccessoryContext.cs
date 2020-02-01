using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace MeetingCentreService.Models.Data
{
    /// <summary>
    /// Context for accessing EF database of accessories.
    /// </summary>
    public class AccessoryContext : DbContext
    {
        /// <summary cref="Entities.AccessoriesCategory">
        /// Database Set of Categories
        /// </summary>
        public DbSet<Entities.AccessoriesCategory> CategorySet { get; set; }
        /// <summary cref="Entities.Accessory">
        /// Database Set of Accessories
        /// </summary>
        public DbSet<Entities.Accessory> AccessorySet { get; set; }
        /// <summary cref="Entities.Accessory.AccessoryHandOutOccurance">
        /// Reference Database Set of Accessory Hand Outs
        /// </summary>
        public DbSet<Entities.Accessory.AccessoryHandOutOccurance> HandOuts { get; set; }

        /// <summary>
        /// Create a new AccessoryContext for development
        /// </summary>
        public AccessoryContext() : base() { } // Used without connection string for development
        /// <summary>
        /// Create a new AccessoryContext with an existing database
        /// </summary>
        /// <param name="connectionString"></param>
        public AccessoryContext(string connectionString) : base(connectionString) { }

        // Initializer set in MainWindow.xaml.cs
        public class AccessoriesInitializer : DropCreateDatabaseIfModelChanges<AccessoryContext>
        {
            protected override void Seed(AccessoryContext context)
            {
                try
                {
                    // Add some default categories
                    context.CategorySet.Add(new Entities.AccessoriesCategory() { Name = "Drinks", Icon = "\xEE64" });
                    context.CategorySet.Add(new Entities.AccessoriesCategory() { Name = "Snacks", Icon = "\xED56" });
                    context.CategorySet.Add(new Entities.AccessoriesCategory() { Name = "Utilities", Icon = "\xE805" });
                    context.SaveChanges();
                } catch (System.Data.Common.DbException e) {
                    Console.WriteLine(e.Message);
                }
                base.Seed(context); // Just for reference, base does nothing
            }
        }
    }
}
