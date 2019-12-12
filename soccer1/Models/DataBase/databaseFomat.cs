
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.Entity;
using soccer1.Models.main_blocks;
using System.ComponentModel.DataAnnotations;

namespace soccer1.Models.DataBase
{
    public class DataDBContext : DbContext
    {
        static DataDBContext(){
            Database.SetInitializer<DataDBContext>(new CreateDatabaseIfNotExists<DataDBContext>());
        }
        public DbSet<PlayerForDatabase> playerInfoes { get; set; }
        //public DbSet<Pawn> allPawns { get; set; }

        public DbSet<Elixir> allElixires { get; set; }

        public DbSet<Formation> allFormations { get; set; }

        public DbSet<Offer> allOffers { get; set; }

        public DbSet<RoboPart> allParts { get; set; }

        public DbSet<RoboBase> allBases { get; set; }





    }
}