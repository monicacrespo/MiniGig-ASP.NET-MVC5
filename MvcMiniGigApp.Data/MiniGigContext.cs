﻿using System.Data.Entity;
using MvcMiniGigApp.Domain;
namespace MvcMiniGigApp.Data
{
    public class MiniGigContext: DbContext
    {
        public MiniGigContext(): base("name=MiniGigConnection"){
        }     
        public virtual DbSet<Gig> Gigs { get; set; }
        public virtual DbSet<MusicGenre> MusicGenres { get; set; }
    }
}