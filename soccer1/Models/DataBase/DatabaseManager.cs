using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models.main_blocks;
using soccer1.Models.utilites;
using soccer1.Models.DataBase;
using System.Data.Entity;
using System.Threading;



namespace soccer1.Models
{
    public class DatabaseManager
    {

        
       
        
        

        //load a player and add it to array
        /*
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

                DataDBContext soccerDataBase = new DataDBContext();
                playerForPlayer = soccerDataBase.playerInfoes.Find(PlayerId);
                    
                //playerInfo = db.playerInfoes2.Find(PlayerId);                
                if (playerForPlayer == null)
                {
                    playerInfo = AddNewDefultPlayerAndReturnIt();
                }
                else
                {
                    playerInfo = new PlayerForConnectedPlayer();
                    playerInfo.reWriteAccordingTo(playerForPlayer);
                }
            }
            
            return playerInfo;
        }
        */
        /*
        public static Pawn LoadPawnData(int numberofpawn)
        {
            DataDBContext dataBase = new DataDBContext();
            Pawn PawnInfo;

            // PawnInfo = db.playerInfoes2.Find(numberofpawn);
            PawnInfo = dataBase.allPawns.Find(numberofpawn);

            return PawnInfo;
        }
        

        public static Elixir LoadElixirData(int numberofElixir)
        {
            DataDBContext dataBase = new DataDBContext();
            Elixir ElixirInfo;

            // PawnInfo = db.playerInfoes2.Find(numberofpawn);
            ElixirInfo = dataBase.allElixires.Find(numberofElixir);

            return ElixirInfo;
        }
        */

        /*
        public static void SaveChangesOnPlayer(PlayerForConnectedPlayer pl)
        {   
            SaveChangesOnPlayerInDataBase(Convertors.PlayerToPlayerForDatabase(pl));
        }
        */
        /*
        static void SaveChangesOnPlayerInDataBase(PlayerForDatabase pl)
        {
            DataDBContext dataBase = new DataDBContext();
            PlayerForDatabase playerInfo = dataBase.playerInfoes.Find(pl.id);
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
                
                dataBase.Entry(playerInfo).State = EntityState.Modified;
                dataBase.SaveChanges();
            }
        }
        */

        public static void AddPawnToDataBase(Pawn p)
        {
            DataDBContext dataBase = new DataDBContext();

            Pawn pp = dataBase.allPawns.Find(p.IdName);
            if (pp == null)
            {
                dataBase.allPawns.Add(p);
                dataBase.SaveChanges();
            }
            else
            {
                dataBase.allPawns.Remove(pp);
                dataBase.SaveChanges();
                dataBase.allPawns.Add(p);
                dataBase.SaveChanges();
            }
        }



        public static void AddFormationToDataBase(Formation ff)
        {
            DataDBContext dataBase = new DataDBContext();
            Formation pp = dataBase.allFormations.Find(ff.IdName);
            if (pp == null)
            {
                dataBase.allFormations.Add(ff);
                dataBase.SaveChanges();
            }
            else
            {
                dataBase.allFormations.Remove(pp);
                dataBase.SaveChanges();
                dataBase.allFormations.Add(ff);
                dataBase.SaveChanges();                
            }
        }


        public static void AddElixirToDataBase(Elixir el)
        {
            DataDBContext dataBase = new DataDBContext();
            Elixir elold = dataBase.allElixires.Find(el.IdName);
            if (elold == null)
            {
                dataBase.allElixires.Add(el);
                dataBase.SaveChanges();
            }
            else
            {
                dataBase.allElixires.Remove(elold);
                dataBase.SaveChanges();
                dataBase.allElixires.Add(el);
                dataBase.SaveChanges();
            }
        }


        /*
        private static Mutex addDefultPlayer = new Mutex();
        private static PlayerForConnectedPlayer AddNewDefultPlayerAndReturnIt()
        {
            
            DataDBContext dataBase = new DataDBContext();            
            PlayerForConnectedPlayer starterPlyer = new Utilities().ReturnDefultPlayer();
            string ss = new Random().NextDouble().ToString();
            addDefultPlayer.WaitOne();
            int index = dataBase.playerInfoes.Count<PlayerForDatabase>() + 1;
            starterPlyer.id = index.ToString()+ ss;
            starterPlyer.AddTodDataBase();
            addDefultPlayer.ReleaseMutex();
            return starterPlyer;
        }      
        */
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