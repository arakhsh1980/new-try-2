using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using soccer1.Models;
using soccer1.Models.utilites;
using soccer1.Models.main_blocks;
using soccer1.Models.DataBase;

namespace soccer1.Controllers
{



    public class ChoosePlayerController : Controller
    {
        [HttpPost]
        public string OtherPlayerTeam(FormCollection collection)
        {
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            DataDBContext dataBase = new DataDBContext();
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);

            if (player != null)
            {
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                string IdName = pl.id;
                string opponentId = new MatchList().ReturnOpponentOf(IdName, matchId);
                if (opponentId == "Erroer") { return "Error"; }
                // TeamForConnectedPlayers opponentTeam =ConnectedPlayersList.ReturnPlayerTeam(opponentId);

                TeamForConnectedPlayers opponentTeam = pl.ReturnYourTeam();
                Convertors convertor = new Convertors();
                return convertor.TeamForSerializeToJson(convertor.TeamToTeamForSerialize(opponentTeam));
            }
            return "This Player Not Exist";
        }
    }
    
}