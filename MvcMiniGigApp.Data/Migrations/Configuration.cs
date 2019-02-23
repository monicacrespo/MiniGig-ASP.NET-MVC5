namespace MvcMiniGigApp.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using MvcMiniGigApp.Data;

    internal sealed class Configuration : DbMigrationsConfiguration<MiniGigContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        //The method Seed is used to populate the database each time is invoked update database
        protected override void Seed(MiniGigContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.           

            DataHelpers.NewDbWithSeed();
        }
    }
}
