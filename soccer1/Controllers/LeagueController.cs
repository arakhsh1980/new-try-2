using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using soccer1.Models;


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
            if (ConnectedPlayersList.IsConnectedById(ConnectionId, PlayerId))
            {
                string mm = ConnectedPlayersList.ReadPlayerEvent(ConnectionId);                
                return mm;               
            }
            else
            {                
                return ServrMasage.Disconcted.ToString();
            }
                
        }

        
        // POST: if first player return true.
        [HttpPost]
        public string ReadyToPlay(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            string  PlayerId = Request.Form["PlayerId"];
            string  SelectedLeage = Request.Form["SelectedLeage"];
            if(ConnectedPlayersList.IsConnectedById(ConnectionId, PlayerId) && (ConnectedPlayersList.ReturnPlayerActiveMatch(ConnectionId) == -1))
            {
                
                // FindSutableMatch return -1 if dident find a sutable match
                int bestmatch = MatchList.FindSutableMatch(ConnectedPlayersList.ReturnPlayerPowerLevel(ConnectionId), SelectedLeage);
                if (bestmatch == -1)
                {
                    MatchList.AddNewMatchWithPlayerOne(ConnectionId, SelectedLeage);
                    
                    return "You Are Fisrt";
                }
                else
                {
                    MatchList.AddSecondPlayerToMatch(bestmatch, ConnectionId);
                    return "You Are Second";
                }
            }
            else
            {
                return "You Are Disconnectid";
            }
        }
    }
}
