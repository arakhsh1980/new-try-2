﻿using System;
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
    public class FactoryController : Controller
    {
        private DataDBContext dataBase = new DataDBContext();
       // public static Mutex AddNewPartmutex = new Mutex();
        [HttpPost]
        public string BuildPartOrder(FormCollection collection)
        {
           

            string PlayerId = Request.Form["PlayerId"];
            //short PartID = short.Parse(Request.Form["PartID"]);
            short builderNumber = short.Parse(Request.Form["builderNumber"]);
           // short partType = short.Parse(Request.Form["partType"]);
            short goldLevel = short.Parse(Request.Form["goldLevel"]);
            RoboPartType partType = (RoboPartType)(short.Parse(Request.Form["PartID"]));
            bool result = false;
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            if (player != null)
            {
                
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                result= pl.AddBuildOrder( partType, goldLevel, builderNumber);
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


        public string BuildPartFinished(FormCollection collection)
        {
            string PlayerId = Request.Form["PlayerId"];
            short builderNumber = short.Parse(Request.Form["builderNumber"]);
            bool result = false;
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            if (player != null)
            {
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                string resultST = pl.FinishBuildOrderIfFinishTimeReached(builderNumber);
                if (resultST == "Done")
                {
                    player.ChangesAcoordingTo(pl);
                    dataBase.Entry(player).State = EntityState.Modified;
                    dataBase.SaveChanges();
                }
                return resultST;
            }
            return result.ToString();
        }

        public string CancelBuildPartOrder(FormCollection collection)
        {
            string PlayerId = Request.Form["PlayerId"];
            short builderNumber = short.Parse(Request.Form["builderNumber"]);
            bool result = false;
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            if (player != null)
            {
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                string resultST = pl.CancelBuildPartOrder(builderNumber);
                if (resultST == "Done")
                {
                    player.ChangesAcoordingTo(pl);
                    dataBase.Entry(player).State = EntityState.Modified;
                    dataBase.SaveChanges();
                }
                return resultST;
            }
            return result.ToString();
        }


    }
}