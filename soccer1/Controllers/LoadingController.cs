using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using soccer1.Models;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using soccer1.Models.utilites;
using soccer1.Models.main_blocks;
using System.Threading;
using soccer1.Models.DataBase;
using System.Data.Entity;





namespace soccer1.Controllers
{
    public class LoadingController : Controller
    {
        private DataDBContext dataBase = new DataDBContext();
        private static Mutex AddNew = new Mutex();
        // POST: Loading/LoadPlayerData
        [HttpPost]
        public string LoadPlayerData(FormCollection collection)
        {            
            string id = Request.Form["PlayerId"];
            new AssetManager().LoadDataFromServerifitsFirstTime();
            PlayerForDatabase player = dataBase.playerInfoes.Find(id);
            PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
            if (player== null) {
                AddNew.WaitOne();
                player = new Utilities().ReturnDefultPlayer();
                string ss = new Random().NextDouble().ToString();               
                int index = dataBase.playerInfoes.Count<PlayerForDatabase>() + 1;
                player.id = index.ToString() + ss;
                dataBase.playerInfoes.Add(player);
                dataBase.SaveChanges();
                AddNew.ReleaseMutex();
                pl.reWriteAccordingTo(player);
            }
            else
            {
                pl.reWriteAccordingTo(player);
                pl.UpdateAll();
                player.ChangesAcoordingTo(pl);
                dataBase.Entry(player).State = EntityState.Modified;
                dataBase.SaveChanges();
            }
            //new MatchList().ClearMatchesOfPlayer(player.id);
            new SymShootMatchesList().ClearMatchesOfPlayer(player.id);
            PlayerForSerial plsr = pl.ReturnForSerialize();
            string[] iddd= plsr.id.Split('.');
            if(AssetManager.hostPlayers.Contains(int.Parse(iddd[0])))
            {
                plsr.IsTHisAHostPlayer = true;
            }
            else
            {
                plsr.IsTHisAHostPlayer = false;
            }
           // PlayerForSerial plsr = new Convertors().PForDatabaseToPForSerial(player);
            dataBase.SaveChanges();
            Log.AddPlayerLog(plsr.id, "player"+ plsr.id.ToString() + " added by " + plsr.id + " ID");
            string uu = new JavaScriptSerializer().Serialize(plsr);
            return uu;  
        }


        [HttpPost]
        public string LoadGaolMemory(FormCollection collection)
        {
            string id = Request.Form["PlayerId"];
            string GoalmemoryNumber = Request.Form["GoalmemoryNumber"];
            new AssetManager().LoadDataFromServerifitsFirstTime();
            PlayerForDatabase player = dataBase.playerInfoes.Find(id);
            string result="";
            if (player == null)
            {
                return "";
            }
            else
            {
                switch (GoalmemoryNumber)
                {
                    case "0":
                        result = player.GoalMemory0;
                        break;
                    case "1":
                        result = player.GoalMemory1;
                        break;
                    case "2":
                        result = player.GoalMemory2;
                        break;
                    case "3":
                        result = player.GoalMemory3;
                        break;
                    case "4":
                        result = player.GoalMemory4;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }


        


        /*
        public string SponserUpdate(FormCollection collection)
        {
            string id = Request.Form["PlayerId"];
            new AssetManager().LoadDataFromServerifitsFirstTime();
            PlayerForDatabase player = dataBase.playerInfoes.Find(id);
            PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
            if (player == null)
            {
                return null;
            }
            else
            {
                pl.reWriteAccordingTo(player);
                pl.UpdateAll();
                dataBase.SaveChanges();
                return  pl.sponsorAlmimun + "*" + pl.sponsorGold;
            }
        }
        */

        [HttpPost]
        public string ReturnXpPakage(FormCollection collection)
        {
            string PlayerId = Request.Form["PlayerId"];
            new AssetManager().LoadDataFromServerifitsFirstTime();
            bool interactionResult = false;
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            if (player != null)
            {
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                BuyableXpPakage returningPack = pl.RequestPlayerPakage();
                player.ChangesAcoordingTo(pl);
                dataBase.Entry(player).State = EntityState.Modified;
                dataBase.SaveChanges();
                string uu = new JavaScriptSerializer().Serialize(returningPack);
                return uu;
            }
            else
            {
                return false.ToString();
            }

        }


    }
}
