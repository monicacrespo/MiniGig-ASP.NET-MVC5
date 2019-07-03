using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MvcMiniGigApp.Domain;

namespace MvcMiniGigApp.Data
{
    public class DataHelpers
    {
        public static void NewDbWithSeed()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<MiniGigContext>());
            using (var context = new MiniGigContext())
            {
                if (context.Gigs.Any())
                {
                    return;
                }

                var popMusic = context.MusicGenres.Add(new MusicGenre { Category = "Pop" });
                var rockMusic = context.MusicGenres.Add(new MusicGenre { Category = "Rock" });
                var punkMusic = context.MusicGenres.Add(new MusicGenre { Category = "Punk" });
                var jazzMusic = context.MusicGenres.Add(new MusicGenre { Category = "Jazz" });
                var flamencoMusic = context.MusicGenres.Add(new MusicGenre { Category = "Flamenco" });
                var reggaetonMusic = context.MusicGenres.Add(new MusicGenre { Category = "Reggaeton" });               

                context.SaveChanges();

                var j = new Gig
                {
                    Name = "The Chacer's Live Gig",
                    GigDate = new DateTime(2019, 5, 2),
                    MusicGenreId = 3,
                    MusicGenre = punkMusic
                };
                var s = new Gig
                {
                    Name = "Ed Sheeran Pop",
                    GigDate = new DateTime(2019, 5, 2),
                    MusicGenreId = 1,
                    MusicGenre = popMusic

                };

                var g = new Gig
                {
                    Name = "Rozalem",
                    GigDate = new DateTime(2019, 5, 2),
                    MusicGenreId = 5,
                    MusicGenre = flamencoMusic
                };

                var l = new Gig
                {
                    Name = "Luis Fonsi & Daddy Yankee",
                    GigDate = new DateTime(2019, 5, 2),
                    MusicGenreId = 6,
                    MusicGenre = reggaetonMusic
                };

                var K = new Gig
                {
                    Name = "David Bowie",
                    GigDate = new DateTime(2019, 5, 2),
                    MusicGenreId = 2,
                    MusicGenre = rockMusic
                };

                var a = new Gig
                {
                    Name = "Muse",
                    GigDate = new DateTime(2019, 5, 2),
                    MusicGenreId = 2,
                    MusicGenre = rockMusic
                };

                var gigList = new List<Gig>();

                for (int i = 0; i < 10; i++)
                {
                    gigList.Add(new Gig
                    {
                        Name = string.Format("z{0}", i.ToString()),
                        GigDate = new DateTime(2019, 5, 2),
                        MusicGenreId = 1,
                        MusicGenre = popMusic
                    });
                }

                context.Gigs.AddRange(new List<Gig> { j, s, g, l, K, a });
                context.Gigs.AddRange(gigList);

                context.SaveChanges();

                //context.Database.ExecuteSqlCommand(
                //  @"CREATE PROCEDURE GetOldGigs
                //            AS  SELECT * FROM Gigs WHERE GigDate<='1/1/1980'");

                //context.Database.ExecuteSqlCommand(
                //   @"CREATE PROCEDURE DeleteGigViaId
                //             @Id int
                //             AS
                //             DELETE from Gigs Where Id = @id
                //             RETURN @@rowcount");
            }
        }
    }
}

