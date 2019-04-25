using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using soccer1.Models;
using System.Web.Script.Serialization;
using soccer1.Models.main_blocks;

namespace soccer1.Controllers
{
    

    public class MatchController : Controller
    {
      
        // POST: Match/Create
        [HttpPost]
        public void PlayerShootDesition(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            if (!ConnectedPlayersList.IsConnectedByIdAndMatch(ConnectionId, PlayerId, matchId)){ return; }
            string jsonpart = collection["jsonCode"];
            ShootActionCode shoot = new JavaScriptSerializer().Deserialize<ShootActionCode>(jsonpart);
            Log.AddLog("shoot resived. shotter : " + shoot.playerID);
            if (/*ConnectedPlayersList.IsShootValid(shoot)*/ true)
            {
                MatchList.shootHapened(shoot, jsonpart);
            }
            else
            {
               // Errors.AddSmallError("an invalid shoot resived");
            }
            

        }

        [HttpPost]
        public void StopedPositions(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string PlayerId = Request.Form["PlayerId"];
            if (!ConnectedPlayersList.IsConnectedByIdAndMatch(ConnectionId, PlayerId, matchId)) { return; }
            string jsonpart = collection["jsonCode"];            
            MatchList.stopedPosition(matchId, ConnectionId, jsonpart);            
        }

        [HttpPost]
        public void GoalHapened(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string PlayerId = Request.Form["PlayerId"];
            int claim = Int32.Parse(Request.Form["GoalClaim"]); 
            if (!ConnectedPlayersList.IsConnectedByIdAndMatch(ConnectionId, PlayerId, matchId)) { return; }
            // if player take a goal it willsend -1
            int Claim = Int32.Parse(Request.Form["GoalClaim"]);
            //ShootActionCode shoot = new JavaScriptSerializer().Deserialize<ShootActionCode>(jsonpart);
            //Log.AddLog("shoot resived. shotter : " + shoot.playerID);
            MatchList.GoalClaim(matchId, ConnectionId, Claim);
        }

        [HttpPost]
        public void PlayerLeaveMatch(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string PlayerId = Request.Form["PlayerId"];
            if (!ConnectedPlayersList.IsConnectedByIdAndMatch(ConnectionId, PlayerId, matchId)) { return; }
            //ShootActionCode shoot = new JavaScriptSerializer().Deserialize<ShootActionCode>(jsonpart);
            //Log.AddLog("shoot resived. shotter : " + shoot.playerID);
            MatchList.PlayerLeaveMatch(matchId, ConnectionId);
        }

        [HttpPost]
        public void ItsMyTurn(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string PlayerId = Request.Form["PlayerId"];
            if (!ConnectedPlayersList.IsConnectedByIdAndMatch(ConnectionId, PlayerId, matchId)) { return; }
            
            MatchList.ItsMyTurn(matchId, ConnectionId);
        }

        // GET: Menu/Details/5
        [HttpPost]
        public string CheckForNewEvent(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string PlayerId = Request.Form["PlayerId"];
            if (ConnectedPlayersList.IsConnectedByIdAndMatch(ConnectionId, PlayerId, matchId) )
            {
                string mm = ConnectedPlayersList.ReadPlayerEvent(ConnectionId);
                //Log.AddLog("CheckForNewEvent from player "+ id.ToString());
                return mm;
            }
            else
            {
                return ServrMasage.Disconcted.ToString();
            }

        }


    }
}
