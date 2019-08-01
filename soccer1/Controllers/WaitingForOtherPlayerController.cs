using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using soccer1.Models;
using soccer1.Models.DataBase;
using soccer1.Models.main_blocks;

namespace soccer1.Controllers
{
    public class WaitingForOtherPlayerController : Controller
    {
        // GET: Menu/Details/5
        [HttpPost]
        public string CheckForNewEvent(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string MatchType = Request.Form["MatchType"];
            if (MatchType == "SymShoots")
            {
                return new SymShootMatchesList().ReturnEvent(PlayerId, matchId);
            }
            else
            {
                return new SymShootMatchesList().ReturnEvent(PlayerId, matchId);
                // return new MatchList().ReturnEvent(PlayerId, matchId);
            }


        }

        [HttpPost]
        public string NotAcceptedToPlay(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string MatchType = Request.Form["MatchType"];
            if (MatchType == "SymShoots")
            {
                return new SymShootMatchesList().NotAcceptedToPlay(PlayerId, matchId);
            }
            else
            {
                return null;
                //return new MatchList().NotAcceptedToPlay(PlayerId, matchId);
            }


        }

        [HttpPost]
        public string PlayAccepted(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string MatchType = Request.Form["MatchType"];
            int GatheredMoney = Int32.Parse(Request.Form["GatheredMoney"]);
            

            if (MatchType == "SymShoots")
            {
                return new SymShootMatchesList().PlayAccepted(PlayerId, matchId, GatheredMoney);
            }
            else
            {
                return null;
                //return new MatchList().NotAcceptedToPlay(PlayerId, matchId);
            }


        }


        


    }
}