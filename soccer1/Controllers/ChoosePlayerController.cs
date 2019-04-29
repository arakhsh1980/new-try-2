using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using soccer1.Models;
using soccer1.Models.utilites;
using soccer1.Models.main_blocks;
using System.Web.Script.Serialization;

namespace soccer1.Controllers
{

   

    public class ChoosePlayerController : Controller
    {
        [HttpPost]
        public string OtherPlayerTeam(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            if (!ConnectedPlayersList.IsConnectedByIdAndMatch(ConnectionId, PlayerId, matchId)) { return"Error In Load"; }
            int opponentId = MatchList.ReturnOpponentOf(ConnectionId, matchId);
            if (opponentId == -1) { return "Error"; }
            TeamForConnectedPlayers opponentTeam =ConnectedPlayersList.ReturnPlayerTeam(opponentId);
            Convertors convertor = new Convertors();
            return convertor.TeamForSerializeToJson(convertor.TeamToTeamForSerialize(opponentTeam));
        }
    }
    
}