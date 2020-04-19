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
using soccer1.Models.DataBase;
using soccer1.Models.main_blocks;
using System.Data.Entity;

namespace soccer1.Controllers
{
    public class UpgradePawnController : Controller
    {
        private DataDBContext dataBase = new DataDBContext();
        [HttpPost]
        public string UpgradePawnto(FormCollection collection)
        {
            long pawnCode = long.Parse(Request.Form["pawnCode"]);
            int newBaseID = Int32.Parse(Request.Form["newPawnType"]);
            string PlayerId = Request.Form["PlayerId"];
            bool interactionResult = false;
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            if (player != null)
            {
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                interactionResult = pl.UpgradePawnto(pawnCode, newBaseID);                
                if (interactionResult)
                {
                    pl.SaveChanges();
                }
            }
            return interactionResult.ToString();
        }

        [HttpPost]
        public string BuyXpPackage(FormCollection collection)
        {
            long pawnCode = long.Parse(Request.Form["pawnCode"]);
            string PlayerId = Request.Form["PlayerId"];
            bool interactionResult = false;
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            if (player != null)
            {
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                interactionResult = pl.BuyXpPakage(pawnCode);
                
                if (interactionResult)
                {
                    pl.SaveChanges();
                }
            }
            return interactionResult.ToString();
        }


        

    }
}