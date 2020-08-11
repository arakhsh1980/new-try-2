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
using System.Data.Entity;

namespace soccer1.Controllers
{
    public class GarageController : Controller
    {
        private DataDBContext dataBase = new DataDBContext();

        [HttpPost]
        public string ScrapPartOrder(FormCollection collection)
        {


            string PlayerId = Request.Form["PlayerId"];
            short PartID = short.Parse(Request.Form["PartID"]);
            short partType = short.Parse(Request.Form["partType"]);
            short goldLevel = short.Parse(Request.Form["goldLevel"]);
            //int partCode = Int32.Parse(Request.Form["partCode"]);
            bool result = false;
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            if (player != null)
            {

                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                result = pl.ScrapPartOrder(PartID, partType, goldLevel);
                if (result)
                {
                    player.ChangesAcoordingTo(pl);
                    dataBase.Entry(player).State = EntityState.Modified;
                    dataBase.SaveChanges();
                }
                return result.ToString();
            }
            return result.ToString();
        }


        [HttpPost]
        public string UpdateRoboParts(FormCollection collection)
        {
            string PlayerId = Request.Form["PlayerId"];
            long[] newRobos = new long[9];
            for(int i =0; i<newRobos.Length; i++)
            {
                newRobos[i] = long.Parse(Request.Form["TeamRobo" + i.ToString()]);
            }
            List<long> newOutOfTeamPawns = new List<long>();
            for (int i = 0; i < 2; i++)
            {
                long newp = long.Parse(Request.Form["outOfTeam" + i.ToString()]);
                if(0 <= newp)
                {
                    newOutOfTeamPawns.Add(newp);
                }
            }

            bool result = false;
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            if (player != null)
            {
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                result = pl.ChangeRoboParts(newRobos, newOutOfTeamPawns);
                if (result)
                {
                    player.ChangesAcoordingTo(pl);
                    dataBase.Entry(player).State = EntityState.Modified;
                    dataBase.SaveChanges();
                }
                return result.ToString();
            }
            return result.ToString();
        }
    }
}