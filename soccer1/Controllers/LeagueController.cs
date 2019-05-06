using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using soccer1.Models;
using soccer1.Models.DataBase;
using soccer1.Models.main_blocks;
using System.Data.Entity;


namespace soccer1.Controllers
{
    public class LeagueController : Controller
    {


        // GET: Menu/Details/5
        [HttpPost]
        public string CheckForNewEvent(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            MatchMassage massage = new MatchList().ReturnEvent(PlayerId, matchId);
            return massage.type.ToString() + massage.body;
        }


        
        // POST: if first player return true.
        [HttpPost]
        public string ReadyToPlay(FormCollection collection)
        {
            
            string  PlayerId = Request.Form["PlayerId"];
            string  SelectedLeage = Request.Form["SelectedLeage"];
            DataDBContext dataBase = new DataDBContext();
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            if (player != null)
            {
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                new MatchList().ClearMatchesOfPlayer(player.id);
                int bestmatch = new MatchList().FindSutableMatch(pl.PowerLevel, SelectedLeage);
                string PlIdName = pl.id;
                float PlPower = pl.PowerLevel;
                int matchId;
                // FindSutableMatch return -1 if dident find a sutable match
                if (bestmatch == -1)
                {
                    matchId= new MatchList().AddNewMatchWithPlayerOne(PlIdName, PlPower, SelectedLeage);

                    return "YouAreFisrtt"+ matchId;
                }
                else
                {
                    new MatchList().AddSecondPlayerToMatch(bestmatch, PlIdName, PlPower);
                    return "YouAreSecond"+ bestmatch;
                }
            }
            return "you are connected";
        }


    }
}
