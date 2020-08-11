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
using System.IO;
using System.Net;
using System.Text;
using System.Data.Entity;

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
                string teamstring = new Convertors().TeamForSerializeToJson(opteam);
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
            if (player != null)
            {
                return player.Name + "*" + player.sponsorName;
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
            string playerName = Request.Form["playerName"];
            string MatchType = Request.Form["MatchType"];
            bool IsPlayAsHost = bool.Parse(Request.Form["IsPlayAsHost"]);
            bool ShouldSendNotification = bool.Parse(Request.Form["ShouldSendNotification"]);
            string groundCharSt = Request.Form["groundCharSt"];
            int numberOfTurns = int.Parse(Request.Form["numberOfTurns"]);


            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            if (player != null)
            {
                if(player.Name != playerName)
                {
                    player.Name = playerName;
                    dataBase.Entry(player).State = EntityState.Modified;
                    dataBase.SaveChanges();
                }
                //

                new SymShootMatchesList().ClearMatchesOfPlayer(player.id);
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                pl.UpdateAll();
                if (pl.NumberOfTickets < 1)
                {
                    return "NotEnothTicket";
                }
                if (new Utilities().TimePointofNow() < pl.playPrehibititionFinishTime)
                {
                    return "wait " + (pl.playPrehibititionFinishTime - new Utilities().TimePointofNow()).ToString() + " minutes and try agane";
                }
                if (MatchType == "SymShoots")
                {
                    int bestmatch = -1;
                    new SymShootMatchesList().ClearMatchesOfPlayer(PlayerId);
                    if (!IsPlayAsHost)
                    {
                        bestmatch = new SymShootMatchesList().FindSutableMatch(pl.totalXp, SelectedLeage, groundCharSt);
                    }
                    else
                    {
                        bestmatch = -1;
                    }
                    string PlIdName = pl.id;
                    float PlPower = pl.totalXp;
                    int matchId;
                    // FindSutableMatch return -1 if dident find a sutable match
                    if (!IsPlayAsHost && ShouldSendNotification)
                    {
                        sendNotificationToHosts();
                    }
                    string PlayerShowName = pl.Name;
                    string PlayerSponserName = pl.sponsorName;
                    string PlayerTeamString = new JavaScriptSerializer().Serialize(pl.team);
                    if (bestmatch == -1)
                    {
                        matchId = new SymShootMatchesList().AddNewMatchWithPlayerOne(PlIdName, PlPower, SelectedLeage, groundCharSt, numberOfTurns, PlayerShowName, PlayerSponserName, PlayerTeamString);

                        return "YouAreFisrtt" + matchId;
                    }
                    else
                    {
                        new SymShootMatchesList().AddSecondPlayer(bestmatch, PlIdName, PlPower, PlayerShowName, PlayerSponserName, PlayerTeamString);
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
            return "you are not exist";
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
                string result = new SymShootMatchesList().PlayAccepted(PlayerId, matchId, GatheredMoney);

                return result;
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
            int EventNumber = Int32.Parse(Request.Form["EventNumber"]);
            string request = Request.Form["request"];
            if (MatchType == "SymShoots")
            {
                MatchEventsArray result = new SymShootMatchesList().ReturnEvent(PlayerId, matchId, EventNumber, request);
                for (int i = 0; i < result.Events.Length; i++)if(result.Events[i].EventTypes == MatchMassageType.GoToMatchi)
                {
                        PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
                        if (player != null)
                        {
                            PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                            pl.reWriteAccordingTo(player);
                            pl.NumberOfTickets = pl.NumberOfTickets - 1.0f;
                            pl.playPrehibititionFinishTime = 0; 
                            player.ChangesAcoordingTo(pl);
                            dataBase.Entry(player).State = EntityState.Modified;
                            dataBase.SaveChanges();
                        }

                    }
                return new JavaScriptSerializer().Serialize(result);
            }
            else
            {
                //return new SymShootMatchesList().ReturnEvent(PlayerId, matchId, EventNumber, request);
                // return new MatchList().ReturnEvent(PlayerId, matchId);
                return "";
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




        void sendNotificationToHosts()
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic Mzk4NTA5M2YtZGZlMS00NDcwLTk4ZDctNjYzZWIzYTgzZjY4");

            var serializer = new JavaScriptSerializer();
            var obj = new
            {
                app_id = "43f116ed-ae97-4c61-a03b-0e2d434b56f6",
                contents = new { en = "play Robo Soccer " },
                included_segments = new string[] { "HostPlayer" },
                collapse_id = "roboSoccer",
                ttl = 60,
                priority = 20
            };
            var param = serializer.Serialize(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }

            System.Diagnostics.Debug.WriteLine(responseContent);
        }




    }
}