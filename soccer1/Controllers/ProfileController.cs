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

namespace soccer1.Controllers
{
    public class ProfileController : Controller
    {
        // GET: ProfileConnection
        [HttpPost]
        public string UpdateProfile(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            string PlayerId = Request.Form["PlayerId"];

            if (ConnectedPlayersList.IsConnectedById(ConnectionId, PlayerId))
            {
                string Team1 = collection["Team"];
                TeamForSerialize teamfs = new JavaScriptSerializer().Deserialize<TeamForSerialize>(Team1);
                TeamForConnectedPlayers playerteam = Convertors.TeamForSerializeToTeam(teamfs);
                ConnectedPlayersList.changeProfileData(playerteam, ConnectionId);
                //PlayerForConnectedPlayer playera = DatabaseManager.LoadPlayerData(PlayerId);
                //DatabaseManager.SaveChangesOnPlayer(playera);
                
                return true.ToString();
            }
            else
            {
                Log.AddLog("Error : Can Not Add Pawn");
                return false.ToString();
            }
        }

        public string LoadTeamPlayerData(FormCollection collection)
        {
            string id = Request.Form["PlayerId"];

            PlayerForSerial plsr = ConnectedPlayersList.LoadPlayerDataFromServer(id);
            Log.AddPlayerLog(plsr.CoonId, "player" + plsr.CoonId.ToString() + " added by " + plsr.id + " ID");
            string uu = new JavaScriptSerializer().Serialize(plsr);
            return uu;

        }
    }
}