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





namespace soccer1.Controllers
{
    public class LoadingController : Controller
    {

        private static Mutex AddNew = new Mutex();
        // POST: Loading/LoadPlayerData
        [HttpPost]
        public string LoadPlayerData(FormCollection collection)
        {            
            string id = Request.Form["PlayerId"];
            DataDBContext dataBase = new DataDBContext();
            PlayerForDatabase player = dataBase.playerInfoes.Find(id);
            if(player== null) {
                AddNew.WaitOne();
                player = new Utilities().ReturnDefultPlayer();
                string ss = new Random().NextDouble().ToString();               
                int index = dataBase.playerInfoes.Count<PlayerForDatabase>() + 1;
                player.id = index.ToString() + ss;
                dataBase.playerInfoes.Add(player);
                AddNew.ReleaseMutex();
            }
            PlayerForSerial plsr = new Convertors().PForDatabaseToPForSerial(player); 
            Log.AddPlayerLog(plsr.id, "player"+ plsr.id.ToString() + " added by " + plsr.id + " ID");
            string uu = new JavaScriptSerializer().Serialize(plsr);
            return uu;  
        }

    }
}
