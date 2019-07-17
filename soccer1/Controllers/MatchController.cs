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
        public string PlayerShootDesition(FormCollection collection)
        {           
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            int TurnNumber = Int32.Parse(Request.Form["TurnNumber"]);
            int pawnAssingedIndex = Int32.Parse(Request.Form["pawnAssingedIndex"]);
            string jsonpart = collection["jsonCode"];
            bool result = false;
            ShootActionCode shoot = new JavaScriptSerializer().Deserialize<ShootActionCode>(jsonpart);
            //Log.AddLog("shoot resived. shotter : " + shoot.playerIDName);
            if (/*ConnectedPlayersList.IsShootValid(shoot)*/ true)
            {
                result = new MatchList().shootHapened(matchId, PlayerId, TurnNumber, jsonpart);
            }
            else
            {
               // Errors.AddSmallError("an invalid shoot resived");
            }
            return result.ToString();

        }

        [HttpPost]
        public string PlayerSubistitutionsDesition(FormCollection collection)
        {
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string jsonpart = collection["jsonCode"];
            int TurnNumber = Int32.Parse(Request.Form["TurnNumber"]);
            bool result = false;
            //Log.AddLog("shoot resived. shotter : " + shoot.playerIDName);
            if (/*ConnectedPlayersList.IsShootValid(shoot)*/ true)
            {
                result= new MatchList().SubistitutionsHappened(matchId, PlayerId, TurnNumber, jsonpart);
            }
            else
            {
                // Errors.AddSmallError("an invalid shoot resived");
            }
            return result.ToString();

        }



        [HttpPost]
        public string PlayerElixirUseDesition(FormCollection collection)
        {
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string jsonpart = collection["jsonCode"];
            int TurnNumber = Int32.Parse(Request.Form["TurnNumber"]);
            bool result = false;
            //Log.AddLog("shoot resived. shotter : " + shoot.playerIDName);
            if (/*ConnectedPlayersList.IsShootValid(shoot)*/ true)
            {
                result = new MatchList().ElixirUseHappened(matchId, PlayerId, TurnNumber, jsonpart);
            }
            else
            {
                // Errors.AddSmallError("an invalid shoot resived");
            }
            return result.ToString();

        }


        [HttpPost]
        public void StopedPositions(FormCollection collection)
        {
            //int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string PlayerId = Request.Form["PlayerId"];
            //if (!ConnectedPlayersList.IsConnectedByIdAndMatch(ConnectionId, PlayerId, matchId)) { return; }
            string jsonpart = collection["jsonCode"];
            int TurnNumber = Int32.Parse(Request.Form["TurnNumber"]);
            new MatchList().stopedPosition(matchId, PlayerId, TurnNumber, jsonpart);            
        }

        [HttpPost]
        public void GoalHapened(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string PlayerId = Request.Form["PlayerId"];
            int claim = Int32.Parse(Request.Form["GoalClaim"]); 
            //if (!ConnectedPlayersList.IsConnectedByIdAndMatch(ConnectionId, PlayerId, matchId)) { return; }
            // if player take a goal it willsend -1
            int Claim = Int32.Parse(Request.Form["GoalClaim"]);
            int TurnNumber = Int32.Parse(Request.Form["TurnNumber"]);
            //ShootActionCode shoot = new JavaScriptSerializer().Deserialize<ShootActionCode>(jsonpart);
            //Log.AddLog("shoot resived. shotter : " + shoot.playerID);
            new MatchList().GoalClaim(matchId, PlayerId, TurnNumber, Claim);
        }

        [HttpPost]
        public void PlayerLeaveMatch(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string PlayerId = Request.Form["PlayerId"];
            //if (!ConnectedPlayersList.IsConnectedByIdAndMatch(ConnectionId, PlayerId, matchId)) { return; }
            //ShootActionCode shoot = new JavaScriptSerializer().Deserialize<ShootActionCode>(jsonpart);
            //Log.AddLog("shoot resived. shotter : " + shoot.playerID);
            new MatchList().PlayerLeaveMatch(matchId, PlayerId);
        }

       

        // GET: Menu/Details/5
        [HttpPost]
        public string CheckForNewMatchEvent(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string PlayerId = Request.Form["PlayerId"];            
            return new MatchList().ReturnEvent(PlayerId, matchId);
        }

        [HttpPost]
        public void TimerIsUp(FormCollection collection)
        {            
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            int TurnNumber = Int32.Parse(Request.Form["TurnNumber"]);            
            string PlayerId = Request.Form["PlayerId"];
            new MatchList().TimerIsUp(PlayerId, matchId, TurnNumber);
        }




    }
}
