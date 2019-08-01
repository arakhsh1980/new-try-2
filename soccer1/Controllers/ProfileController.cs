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
    public class ProfileController : Controller
    {
        private DataDBContext dataBase = new DataDBContext();
        // GET: ProfileConnection
        [HttpPost]
        public string UpdateProfile(FormCollection collection)
        {            
            string PlayerId = Request.Form["PlayerId"];
            bool interactionResult=false;           
            string Team1 = collection["Team"];
            TeamForSerialize teamfs = new JavaScriptSerializer().Deserialize<TeamForSerialize>(Team1);                
            TeamForConnectedPlayers playerteam = new Convertors().TeamForSerializeToTeam(teamfs);
            
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            if (player != null)
            {
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                interactionResult= pl.ChangeTeam(playerteam);
                /*
                if (interactionResult)
                {
                    player.changePlayer(pl.returnDataBaseVersion());
                    dataBase.Entry(player).State = EntityState.Modified;
                    dataBase.SaveChanges();
                } 
                */
            }
            return interactionResult.ToString();
        }

        [HttpPost]
        public string UpgradePawnto(FormCollection collection)
        {            
            int pawnCode = Int32.Parse(Request.Form["pawnCode"]);
            int newPawnType = Int32.Parse(Request.Form["newPawnType"]);
            string PlayerId = Request.Form["PlayerId"];
            bool interactionResult = false;
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            if (player != null)
            {
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                interactionResult = pl.UpgradePawnto(pawnCode, newPawnType);
                /*
                if (interactionResult)
                {
                    player.changePlayer(pl.returnDataBaseVersion());
                    dataBase.Entry(player).State = EntityState.Modified;
                    dataBase.SaveChanges();
                }
                */
            }
            return interactionResult.ToString();
        }

    }
}