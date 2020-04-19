using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models;
using soccer1.Models.main_blocks;
using System.Web.Script.Serialization;

namespace soccer1.Models
{
    
    public class SymShootMatchesList
    {


        private static symShootMatch[] matchList = new symShootMatch[100];
        private static string waitingPlayerId;

        #region Massages Form Client
        public void NewWaitingPlayer(string PlayerId)
        {
            waitingPlayerId = PlayerId;
        }
       


        public void TimerIsUp(string PlayerId, int MatchId, int turn)
        {
            if(matchList[MatchId] == null)
            {
                Errors.AddBigError("matchList["+ MatchId.ToString() + "] == null");
                return;
            }
            matchList[MatchId].TimerIsUp(PlayerId, turn);
        }

        public bool SubistitutionsHappened(int MatchId, string PlayerId, int TurnNumber, string jsonpart)
        {
            if (matchList[MatchId] == null)
            {
                Errors.AddBigError("matchList[" + MatchId.ToString() + "] == null");
                return false;
            }
            return matchList[MatchId].SubistitutionsHappened(PlayerId, TurnNumber, jsonpart);
        }

        public bool ElixirUseHappened(int MatchId, string PlayerId, int TurnNumber, string jsonpart)

        {
            if (matchList[MatchId] == null)
            {
                Errors.AddBigError("matchList[" + MatchId.ToString() + "] == null");
                return false;
            }
            return matchList[MatchId].ElixirUseHappened(PlayerId, TurnNumber, jsonpart);
        }


        public bool shootHapened(int matchNumber, string PlayeriD, int TurnNumber, string jsonpart)
        {
            if (matchList[matchNumber] == null)
            {
                Errors.AddBigError("matchList[" + matchNumber.ToString() + "] == null");
                return false;
            }
            return matchList[matchNumber].ShootHappeded(PlayeriD, TurnNumber, jsonpart);
        }

        public void stopedPosition(int matchId, string nameId, int TurnNumber, string jsonpart)
        {
            if (matchList[matchId] == null)
            {
                Errors.AddBigError("matchList[" + matchId.ToString() + "] == null");
                return;
            }
            matchList[matchId].GetStationeryPostion(nameId, TurnNumber, jsonpart);
        }


        public string ReturnEvent(string playerId, int matchId, int EventNumber, string request)
        {
            string result = "Error";
            MatchEventsArray defultEvent = new MatchEventsArray();
            defultEvent.Events = new MatchEvents[1];
            defultEvent.Events[0].EventTypes = MatchMassageType.Error;
            defultEvent.Events[0].desitionBodys = "Match not finded";
            defultEvent.Events[0].EventNumber = -1;
                     

            if (0 <= matchId && matchId < matchList.Length)
            {
                if (matchList[matchId] != null)
                {
                    result = matchList[matchId].ReturnEvent(playerId, EventNumber, request);
                    return result;
                }
                else
                {
                    defultEvent.Events[0].desitionBodys = "matchId not finded ";
                    Errors.AddBigError("matchList[" + matchId.ToString() + "] == null");                    
                }
            } else
            {
                defultEvent.Events[0].desitionBodys = "matchId out of renge ";
            }
            result = new JavaScriptSerializer().Serialize(defultEvent);
            return result;
        }


        public string NotAcceptedToPlay(string playerId, int matchId)
        {
            if (matchList[matchId] == null)
            {
                Errors.AddBigError("matchList[" + matchId.ToString() + "] == null");
                return "Error";
            }
            return matchList[matchId].PlayerOneNotAcceptedToPlay(playerId);
            
        }

        public void CanceledThePlayRequest(string playerId, int matchId)
        {
            if (matchList[matchId] == null)
            {
                Errors.AddBigError("matchList[" + matchId.ToString() + "] == null");
                return ;
            }
            matchList[matchId].playerLostOrCenceled(playerId);
        }

        public string PlayAccepted(string playerId, int matchId, int GatheredMoney)
        {
            if (matchList[matchId] == null)
            {
                Errors.AddBigError("matchList[" + matchId.ToString() + "] == null");
                return "Error";
            }
            return matchList[matchId].PlayerOnePlayAccepted(playerId, GatheredMoney);

           
        }


        public int AddMoneyToPlayerForWaiting(string playerId, int matchId)
        {
            if (matchList[matchId] == null)
            {
                Errors.AddBigError("matchList[" + matchId.ToString() + "] == null");
                return 0;
            }
            return matchList[matchId].AddMoneyToPlayerForWaiting(playerId);


        }

        public string GoingHomeAndWaiting(string playerId, int matchId)
        {
            if (matchList[matchId] == null)
            {
                Errors.AddBigError("matchList[" + matchId.ToString() + "] == null");
                return "Error";
            }
            return matchList[matchId].GoingHomeAndWaiting(playerId);
        }

        

        //cliam will be 1 or -1
        public void GoalClaim(int matchId, string nameId, int TurnNumber, int Claim)
        {
            if (matchList[matchId] == null)
            {
                Errors.AddBigError("matchList[" + matchId.ToString() + "] == null");
                return ;
            }
            matchList[matchId].GoalReport(nameId, TurnNumber, Claim);
        }

        #endregion


        // reaturn best sutable match... id there is no match return -1
        public int FindSutableMatch(float PlayerPowerLevel, string SelectedLeage, string groundCharSt)
        {
            float bestpowerDiference = float.MaxValue;
            int bestMatch = -1;
            int[] test1 =ReturnMatchesWithSituation(PreMatchSituation.WFSecondPlayer);
            int[] test2 = ReturnMatchesWithSituation(PreMatchSituation.WFSecondPlayerAtHome);
            for (int i = (matchList.Length - 1); 0 <= i; i--)if(matchList[i] != null)
            {                
                if (( matchList[i].GivePreSituation() == PreMatchSituation.WFSecondPlayer || matchList[i].GivePreSituation() == PreMatchSituation.WFSecondPlayerAtHome) && matchList[i].GivLeague() == SelectedLeage)
                {                    
                    matchList[i].IsPreSituationStillValid();
                    if ( matchList[i].GivePreSituation() == PreMatchSituation.WFSecondPlayer || matchList[i].GivePreSituation() == PreMatchSituation.WFSecondPlayerAtHome)
                    {
                        if (Math.Abs(matchList[i].GivePlayerOnePower() - PlayerPowerLevel) < bestpowerDiference)
                        {
                            if(matchList[i].groundCharString == groundCharSt)
                                {
                                    bestMatch = i;
                                    bestpowerDiference = Math.Abs(matchList[i].GivePlayerOnePower() - PlayerPowerLevel);
                                }
                                else
                                {
                                    matchList[i].playerOneGroundExpired();
                                }
                           
                        }
                    }
                }
            }
            if (bestpowerDiference < Statistics.maxAcceptedPowerDiference)
            {
                return bestMatch;
            }
            else
            {
                return -1;
            }
        }

        public string ReturnOpponentOf(string IdName, int matchId)
        {
            if (matchList[matchId] == null)
            {
                Errors.AddBigError("matchList[" + matchId.ToString() + "] == null");
                return "Error";
            }
            string firstId = matchList[matchId].ReturnFirstPlayerByIdName();
            string SecId = matchList[matchId].ReturnSeccondPlayerByIdName();
            if (firstId == IdName) { return SecId; }
            if (SecId == IdName) { return firstId; }
            //int firstId = matchList[matchId].ReturnFirstPlayer();
            //int SecId = matchList[matchId].ReturnSecondPlayer();
            //if (firstId == connectionId) { return SecId; }
            //if (SecId == connectionId) { return firstId; }
            Errors.AddBigError("this player is not in its clamed match");
            return "Erroer";
        }
        public void AddSecondPlayer(int matchNumber, string playerNameId, float playerPower)
        {
            if (matchList[matchNumber] == null)
            {
                Errors.AddBigError("matchList[" + matchNumber.ToString() + "] == null");
                return ;
            }
            if ( matchList[matchNumber].GivePreSituation() == PreMatchSituation.WFSecondPlayer)
            {
                matchList[matchNumber].AddSecondPlayerStartImideatly(playerNameId, playerPower);
            }
            else
            {
                if (matchList[matchNumber].GivePreSituation() == PreMatchSituation.WFSecondPlayerAtHome)
                {
                    matchList[matchNumber].AddSecondPlayerAndWaitForFisrtRespond(playerNameId, playerPower);
                }
            }
        }

      

        public void ClearMatchesOfPlayer(string nameId)
        {
            for (int i = 0; i < matchList.Length; i++)
            {
                if (matchList[i] != null) { matchList[i].playerLostOrCenceled(nameId); }
            }
        }

        public void playerOfMatchLost(int matchId, string PlayerNameId)
        {
            if (matchList[matchId] == null)
            {
                Errors.AddBigError("matchList[" + matchId.ToString() + "] == null");
                return;
            }
            if (matchId < 0)
            {
                Errors.AddBigError("unacceptable match id at playerOfMatchLost. match id: " + matchId.ToString());
                return;
            }
            //Log.AddMatchLog(matchId, " player " + PlayerNameId + " Of Match Lost");
            matchList[matchId].playerLostOrCenceled(PlayerNameId);
        }


        private static Mutex addMatch = new Mutex();
        public int AddNewMatchWithPlayerOne(string playerIdName, float playerPower, string SelectedLeage, string groundCharSt,int numberOfTurns)
        {
           
            addMatch.WaitOne();
            int bestMatch = -1;
            for (int i = (matchList.Length - 1); 0 <= i; i--)if(matchList[i] != null)
            {
                if (matchList[i].GivePreSituation() == PreMatchSituation.NonExistance)
                {
                    bestMatch = i;
                }
            }
            if (bestMatch == -1)
            {
                Log.AddLog("matchList is full. try to free some");
                for (int k =0; k < matchList.Length; k++)if(matchList[k]!=null)
                {
                    matchList[k].IsPreSituationStillValid();
                }
                for (int i = (matchList.Length - 1); 0 <= i; i--) if (matchList[i] != null)
                    {
                    if (matchList[i].GivePreSituation() == PreMatchSituation.NonExistance)
                    {
                        bestMatch = i;
                    }
                }

                if(bestMatch == -1)
                {
                    Errors.AddBigError(" find no empty match to add player one");
                    addMatch.ReleaseMutex();
                    return -1;
                } 
            }
            
                matchList[bestMatch] = new symShootMatch();
                matchList[bestMatch].InitiatMatchWithOnePlayer(playerIdName, playerPower, bestMatch, SelectedLeage, groundCharSt, numberOfTurns);


                //Log.AddMatchLog(bestMatch, "added with player" + playerIdName + " as first player");
                addMatch.ReleaseMutex();
                return bestMatch;

            //ConnectedPlayersList.SetPlayerMatch(playerIdName, bestMatch);            
        }

        int[] ReturnMatchesWithSituation(PreMatchSituation situation)
        {
            int[] ar1 = new int[matchList.Length];
            int counter = 0;
            for (int i = (matchList.Length - 1); 0 <= i; i--) if (matchList[i] != null)
                {
                    if (matchList[i].GivePreSituation() == situation)
                    {
                        ar1[counter] = i;
                        counter++;
                    }
                }
            int[] ar2 = new int[counter];

            for (int i = 0; i < counter; i++)
            {
                ar2[i] = ar1[i];
            }
            return ar2;
        }

        public string ReturnActiveMatches()
        {
            string result = "";
            for (int i = 0; i < matchList.Length; i++)if(matchList[i]!=null)
            {
                
                if (matchList[i].GivePreSituation() != PreMatchSituation.NonExistance)
                {
                    // if(matchList[i].GivePreSituation() == PreMatchSituation.WithOnePlayer || matchList[i].GivePreSituation() == PreMatchSituation.WFSecondPlayer)
                    result = result + " ---------  .matchNumber: " + i.ToString() + " .with first player: " + matchList[i].ReturnFirstPlayerByIdName()+ " .with Second player: " + matchList[i].ReturnSeccondPlayerByIdName() + "  .situation: " + matchList[i].GivePreSituation().ToString();
                }
            }
            return result;
        }

        // this function will be called ones at start of server
        public static void FillArrays()
        {
            for (int i = 0; i < matchList.Length; i++)
            {
                matchList[i] = new symShootMatch();
            }
            lastCheckMatchListForTimeOut = DateTime.Now;
        }
        public static void DelletMatch(int matchNumber)
        {
            matchList[matchNumber] = null;
        }
        public void PlayerLeaveMatch(int matchId, string NameId)
        {
            if (matchList[matchId] == null)
            {
                Errors.AddBigError("matchList[" + matchId.ToString() + "] == null");
                return;
            }
            if (-1<matchId && matchId< matchList.Length)
            {
                matchList[matchId].playerLostOrCenceled(NameId);
            }
        }


        public static DateTime ReturnMatchConnectionTime(int matchId)
        {
            if (matchList[matchId] == null)
            {
                Errors.AddBigError("matchList[" + matchId.ToString() + "] == null");
                DateTime time = new DateTime();
                return time;
            }
            return matchList[matchId].ReturnConnectionTime();
        }

        private static Mutex CheckMatchListMutex = new Mutex();
        public static void CheckMatchListForTimeOut()
        {
            CheckMatchListMutex.WaitOne();
            float timeFromLast =  (float)DateTime.Now.Subtract(lastCheckMatchListForTimeOut).TotalSeconds;
            if (MatchListTimeIntervals < timeFromLast)
            {
                for (int i = 0; i < matchList.Length; i++)
                {
                    matchList[i].IsPreSituationStillValid();
                }
            }
            CheckMatchListMutex.ReleaseMutex();
        }
        static DateTime lastCheckMatchListForTimeOut;
        const float MatchListTimeIntervals = 60.0f;
    }
}