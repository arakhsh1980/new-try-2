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
        private DataDBContext dataBase = new DataDBContext();
        // GET: Menu/Details/5
        [HttpPost]
        public string CheckForNewEvent(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string MatchType = Request.Form["MatchType"];
            int EventNumber = Int32.Parse(Request.Form["EventNumber"]);
            string request = Request.Form["request"];
            if (MatchType == "SymShoots")
            {
                return new SymShootMatchesList().ReturnEvent(PlayerId, matchId, EventNumber, request);
            }
            else
            {
                return new SymShootMatchesList().ReturnEvent(PlayerId, matchId, EventNumber, request);
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

        [HttpPost]
        public string AddMoneyToPlayerForWaiting(FormCollection collection)
        {
            //int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string MatchType = Request.Form["MatchType"];
            //int GatheredMoney = Int32.Parse(Request.Form["GatheredMoney"]);

            int reuslt = 0;
            if (MatchType == "SymShoots")
            {
                reuslt =  new SymShootMatchesList().AddMoneyToPlayerForWaiting(PlayerId, matchId);
                if (0<reuslt)
                {
                    PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
                    if (player != null)
                    {

                        PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                        pl.reWriteAccordingTo(player);
                        Property AddedForWaiting = new Property();
                        AddedForWaiting.Alminum = reuslt;
                        pl.AddProperty(AddedForWaiting);
                            pl.SaveChanges();
                    }
                    else
                    {
                        reuslt = 0;
                    }
                }
                
            }
            else
            {
                
            }
            return reuslt.ToString();
        }




    }
}