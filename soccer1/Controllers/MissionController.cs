using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using soccer1.Models;
using soccer1.Models.main_blocks;
using soccer1.Models.utilites;
using soccer1.Models.DataBase;
using System.Threading;

namespace soccer1.Controllers
{
    public class MissionController : Controller
    {
        private DataDBContext dataBase = new DataDBContext();
        // public static Mutex AddNewPartmutex = new Mutex();
        [HttpPost]
        public string MissionDone(FormCollection collection)
        {
            string PlayerId = Request.Form["PlayerId"];
            short PartID = short.Parse(Request.Form["PartID"]);
            short builderNumber = short.Parse(Request.Form["builderNumber"]);
            short partType = short.Parse(Request.Form["partType"]);
            short goldLevel = short.Parse(Request.Form["goldLevel"]);
            bool result = false;
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            if (player != null)
            {

                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                result = pl.AddBuildOrder(PartID, partType, goldLevel, builderNumber);
                if (result)
                {
                    pl.SaveChanges();
                }
                return result.ToString();
            }
            return result.ToString();
        }


    }
}