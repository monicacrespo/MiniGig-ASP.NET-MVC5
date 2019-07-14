namespace MvcMiniGigApp.Data
{
    using System.Data.Entity;
    using MvcMiniGigApp.Domain;

    public class MiniGigContext: DbContext
    {
        public MiniGigContext(): base("name=MiniGigConnection"){
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MiniGigContext, Migrations.Configuration>(useSuppliedContext:true));
        }
        public virtual DbSet<Gig> Gigs { get; set; }
        public virtual DbSet<MusicGenre> MusicGenres { get; set; }
    }
}