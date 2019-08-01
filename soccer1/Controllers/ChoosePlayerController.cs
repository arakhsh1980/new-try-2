using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using soccer1.Models;
using soccer1.Models.utilites;
using soccer1.Models.main_blocks;
using soccer1.Models.DataBase;
using System.Web.Script.Serialization;

namespace soccer1.Controllers
{



    public class ChoosePlayerController : Controller
    {
        private DataDBContext dataBase = new DataDBContext();
        [HttpPost]
        public string OtherPlayerTeam(FormCollection collection)
        {
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string MatchType = Request.Form["MatchType"];
            string opponentId;
            if (MatchType == "SymShoots")
            {
                opponentId = new SymShootMatchesList().ReturnOpponentOf(PlayerId, matchId);
            }
            else
            {
                opponentId = new SymShootMatchesList().ReturnOpponentOf(PlayerId, matchId);
                //opponentId = new MatchList().ReturnOpponentOf(PlayerId, matchId);
            }

            

            PlayerForDatabase player = dataBase.playerInfoes.Find(opponentId);

            if (player != null)
            {
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                TeamForConnectedPlayers opponentTeam = pl.ReturnYourTeam();
                TeamForSerialize opteam = new Convertors().TeamToTeamForSerialize(opponentTeam);
                string teamstring= new Convertors().TeamForSerializeToJson(opteam);
                //testtt test = new testtt();
                //test.CurrentFormation = opteam.CurrentFormation;

                return teamstring;

            }
            return "This Player Not Exist";
        }


        [HttpPost]
        public string OtherPlayerName(FormCollection collection)
        {
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string MatchType = Request.Form["MatchType"];
            string opponentId;
            if (MatchType == "SymShoots")
            {
                opponentId = new SymShootMatchesList().ReturnOpponentOf(PlayerId, matchId);
            }
            else
            {
                opponentId = new SymShootMatchesList().ReturnOpponentOf(PlayerId, matchId);
                //opponentId = new MatchList().ReturnOpponentOf(PlayerId, matchId);
            }

            
            PlayerForDatabase player = dataBase.playerInfoes.Find(opponentId);
            if(player != null)
            {
                return player.Name;
            }
            else
            {
                return "FalseId";
            }
            
        }

        // POST: if first player return true.
        [HttpPost]
        public string ReadyToPlay(FormCollection collection)
        {

            string PlayerId = Request.Form["PlayerId"];
            string SelectedLeage = Request.Form["SelectedLeage"];
            string MatchType = Request.Form["MatchType"];
            
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            if (player != null)
            {
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                if (MatchType == "SymShoots")
                {
                    new SymShootMatchesList().ClearMatchesOfPlayer(PlayerId);
                    int bestmatch = new SymShootMatchesList().FindSutableMatch(pl.PowerLevel, SelectedLeage);
                    string PlIdName = pl.id;
                    float PlPower = pl.PowerLevel;
                    int matchId;
                    // FindSutableMatch return -1 if dident find a sutable match
                    if (bestmatch == -1)
                    {
                        matchId = new SymShootMatchesList().AddNewMatchWithPlayerOne(PlIdName, PlPower, SelectedLeage);

                        return "YouAreFisrtt" + matchId;
                    }
                    else
                    {                        
                        new SymShootMatchesList().AddSecondPlayer(bestmatch, PlIdName, PlPower);
                        return "YouAreSecond" + bestmatch;
                    }
                }
                else
                {
                    /*
                    new MatchList().ClearMatchesOfPlayer(player.id);
                    int bestmatch = new MatchList().FindSutableMatch(pl.PowerLevel, SelectedLeage);
                    string PlIdName = pl.id;
                    float PlPower = pl.PowerLevel;
                    int matchId;
                    // FindSutableMatch return -1 if dident find a sutable match
                    if (bestmatch == -1)
                    {
                        matchId = new MatchList().AddNewMatchWithPlayerOne(PlIdName, PlPower, SelectedLeage);

                        return "YouAreFisrtt" + matchId;
                    }
                    else
                    {
                        new MatchList().AddSecondPlayerToMatch(bestmatch, PlIdName, PlPower);
                        return "YouAreSecond" + bestmatch;
                    }
                    */
                }
            }
            return "you are connected";
        }

        [HttpPost]
        public string CancelPlay(FormCollection collection)
        {

            string PlayerId = Request.Form["PlayerId"];            
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string MatchType = Request.Form["MatchType"];
            if (MatchType == "SymShoots")
            {
                new SymShootMatchesList().CanceledThePlayRequest(PlayerId, matchId);
                return true.ToString();
            }
            else
            {
                return true.ToString();
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
        public string GoingHomeAndWaiting(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);
            string MatchType = Request.Form["MatchType"];
            if (MatchType == "SymShoots")
            {
                return new SymShootMatchesList().GoingHomeAndWaiting(PlayerId, matchId);
            }
            else
            {
                return new SymShootMatchesList().GoingHomeAndWaiting(PlayerId, matchId);
                //return new MatchList().ReturnEvent(PlayerId, matchId);
            }
        }


        [HttpPost]
        public string ReturnGamePreference(FormCollection collection)
        {            
            return Statistics.BasePrefrance;
        }


    }

}