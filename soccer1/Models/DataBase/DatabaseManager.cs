using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models.main_blocks;
using soccer1.Models.utilites;
using System.Data.Entity;



namespace soccer1.Models
{
    public class DatabaseManager
    {
        
        private static playerInfoDBContext db = new playerInfoDBContext();
        private static PawnInfoDBContext Pawndb = new PawnInfoDBContext();
        private static ElixirInfoDBContext Elixirdb = new ElixirInfoDBContext();
        private static FormationInfoDBContext Formationdb = new FormationInfoDBContext();
        


        //load a player and add it to array
        public static PlayerForConnectedPlayer LoadPlayerData(string PlayerId)
        {
            PlayerForConnectedPlayer playerInfo;
            PlayerForDatabase playerForPlayer;
            if (PlayerId == null)
            {
                playerInfo = AddNewDefultPlayerAndReturnIt();
            }
            else
            {

                playerForPlayer = db.playerInfoes2.Find(PlayerId);
                //playerInfo = db.playerInfoes2.Find(PlayerId);                
                if (playerForPlayer == null)
                {
                    playerInfo = AddNewDefultPlayerAndReturnIt();
                }
                else
                {
                    playerInfo = Convertors.PlayerForDatabaseToPlayer(playerForPlayer);
                }
            }
            
            return playerInfo;
        }


        public static Pawn LoadPawnData(int numberofpawn)
        {
            Pawn PawnInfo;

            // PawnInfo = db.playerInfoes2.Find(numberofpawn);
            PawnInfo = Pawndb.Pawnss.Find(numberofpawn);

            return PawnInfo;
        }
        

        public static Elixir LoadElixirData(int numberofElixir)
        {
            Elixir ElixirInfo;

            // PawnInfo = db.playerInfoes2.Find(numberofpawn);
            ElixirInfo = Elixirdb.Elixirs.Find(numberofElixir);

            return ElixirInfo;
        }

        public static void SaveChangesOnPlayer(PlayerForConnectedPlayer pl)
        {   
            SaveChangesOnPlayerInDataBase(Convertors.PlayerToPlayerForDatabase(pl));
        }

        public static void SaveChangesOnPlayerInDataBase(PlayerForDatabase pl)
        {
            PlayerForDatabase playerInfo = db.playerInfoes2.Find(pl.id);
            if (playerInfo == null)
            {
                Errors.AddBigError("player dident find for saveing");
            }
            else
            {
                playerInfo.CurrentFormation = pl.CurrentFormation;
                playerInfo.ElixirInBench = pl.ElixirInBench;
                playerInfo.Fan = pl.Fan;
                playerInfo.level = pl.level;
                playerInfo.Money = pl.Money;
                playerInfo.Name = pl.Name;
                playerInfo.otherElixirs = pl.otherElixirs;
                playerInfo.pawnsInBench = pl.pawnsInBench;
                playerInfo.PlayeingPawns = pl.PlayeingPawns;
                playerInfo.PowerLevel = pl.PowerLevel;
                playerInfo.SoccerSpetial = pl.SoccerSpetial;
                playerInfo.UsableFormations = pl.UsableFormations;
                db.Entry(playerInfo).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static void AddPawnToDataBase(Pawn p)
        {
            if (Pawndb.Pawnss.Find(p.IdName) == null)
            {
                Pawndb.Pawnss.Add(p);
                Pawndb.SaveChanges();
            }
            else
            {
                Pawn pp = Pawndb.Pawnss.Find(p.IdName);
                pp = p;
                Pawndb.Entry(pp).State = EntityState.Modified;
                Pawndb.SaveChanges();
            }
        }



        public static void AddFormationToDataBase(Formation ff)
        {
            if (Formationdb.Formationss.Find(ff.IdName) == null)
            {
                Formationdb.Formationss.Add(ff);
                Formationdb.SaveChanges();
            }
            else
            {
                Formation pp = Formationdb.Formationss.Find(ff.IdName);
                pp = ff;
                Formationdb.Entry(pp).State = EntityState.Modified;
                Formationdb.SaveChanges();
            }
        }


        public static void AddElixirToDataBase(Elixir el)
        {
            if (Elixirdb.Elixirs.Find(el.IdName) == null)
            {
                Elixirdb.Elixirs.Add(el);
                Elixirdb.SaveChanges();
            }
            else
            {
                Elixir pp = Elixirdb.Elixirs.Find(el.IdName);
                pp = el;
                Elixirdb.Entry(pp).State = EntityState.Modified;
                Elixirdb.SaveChanges();
            }
        }

        private static PlayerForConnectedPlayer AddNewDefultPlayerAndReturnIt()
        {   
            PlayerForConnectedPlayer starterPlyer = utilities.ReturnDefultPlayer();
            string ss = new Random().NextDouble().ToString();            
            starterPlyer.id = db.playerInfoes2.Count<PlayerForDatabase>().ToString()+ ss;
            PlayerForDatabase player = Convertors.PlayerToPlayerForDatabase(starterPlyer);
            db.playerInfoes2.Add(player);
            db.SaveChanges();
            return starterPlyer;
        }      

        //public static Pawn LoadPawnDataFromServer(string PlayerId , string IdName)
        //{
        //    Player playerInfo;
        //    PlayerForDatabase playerForPlayer;
            

        //        playerForPlayer = db.playerInfoes2.Find(PlayerId);
               
        //        playerInfo = Convertors.PlayerForDatabaseToPlayer(playerForPlayer);
        //   if( playerInfo.Coin > 7 )
        //    {

        //    }
        //    //ConnectedPlayersList
        //    return playerInfo;

        //}
    }
}