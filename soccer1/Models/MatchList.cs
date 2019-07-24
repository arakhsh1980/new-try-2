﻿using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models;
using System.Web.Script.Serialization;

namespace soccer1.Models
{
    public class MatchList
    {

        
        private static TwoPlayerMatch[] matchList = new TwoPlayerMatch[10];


#region Massages Form Client

        public void TimerIsUp(string PlayerId, int MatchId, int turn)
        {
            matchList[MatchId].TimerIsUp(PlayerId,turn);
        }

        public bool SubistitutionsHappened(int MatchId, string PlayerId,int TurnNumber, string jsonpart)
        {
           return matchList[MatchId].SubistitutionsHappened(PlayerId,  TurnNumber, jsonpart);
        }

        public bool ElixirUseHappened(int MatchId, string PlayerId,int TurnNumber, string jsonpart)

        {
            return matchList[MatchId].ElixirUseHappened(PlayerId, TurnNumber, jsonpart);
        }


        public bool shootHapened(int matchNumber, string PlayeriD,int TurnNumber, string jsonpart)
        {
            return matchList[matchNumber].ShootHappeded(PlayeriD, TurnNumber, jsonpart);
        }

        public void stopedPosition(int matchId, string nameId, int TurnNumber, string jsonpart)
        {
            matchList[matchId].GetStationeryPostion(nameId,  TurnNumber, jsonpart);
        }


        public string ReturnEvent(string playerId, int matchId)
        {
            string result= "Error";
            if (0 <= matchId && matchId < matchList.Length)
            {
                result = matchList[matchId].ReturnEvent(playerId);
            }
            if(result == "Error")
            {
                MatchEvents defultEvent = new MatchEvents();
                defultEvent.EventTypes = new List<MatchMassageType>();
                defultEvent.desitionBodys = new List<string>();
                defultEvent.EventTypes.Add(MatchMassageType.Error);
                defultEvent.desitionBodys.Add("");
                result = new JavaScriptSerializer().Serialize(defultEvent);
            }
            return result;
        }

        //cliam will be 1 or -1
        public void GoalClaim(int matchId, string nameId, int TurnNumber, int Claim)
        {
            matchList[matchId].GoalReport(nameId,  TurnNumber, Claim);
        }

        #endregion


        // reaturn best sutable match... id there is no match return -1
        public int  FindSutableMatch(float PlayerPowerLevel, string SelectedLeage)
        {
            float bestpowerDiference = float.MaxValue;
            int bestMatch = -1;
            
            for (int i = (matchList.Length - 1); 0 <= i; i--)
            {
                matchList[i].CheckYourSelf();
                if ((matchList[i].GivePreSituation() == PreMatchSituation.WithOnePlayer || matchList[i].GivePreSituation() == PreMatchSituation.WFSecondPlayer) && matchList[i].GivLeague() == SelectedLeage)
                {
                    if (Math.Abs(matchList[i].GivePlayerOnePower() - PlayerPowerLevel) < bestpowerDiference)
                    {
                        bestMatch = i;
                        bestpowerDiference = Math.Abs(matchList[i].GivePlayerOnePower() - PlayerPowerLevel);
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
        public void AddSecondPlayerToMatch(int matchNumber , string playerNameId,float playerPower )
        {
            matchList[matchNumber].AddSecondPlayerToMatch(playerNameId, playerPower);
            
            //ConnectedPlayersList.SetPlayerMatch(playerConnId, matchNumber);
        }

        public string ReturnActiveMatches()
        {
            string result = "";
            for(int i =0; i< matchList.Length; i++)
            {
                result = result + ".    matchNumber " + i.ToString() + " with player " + matchList[i].ReturnFirstPlayerByIdName() + matchList[i].GivePreSituation().ToString();
                if (matchList[i].GivePreSituation() != PreMatchSituation.NonExistance)
                {
                   // if(matchList[i].GivePreSituation() == PreMatchSituation.WithOnePlayer || matchList[i].GivePreSituation() == PreMatchSituation.WFSecondPlayer)
                    
                }
            }
            return result;
        }

        public void ClearMatchesOfPlayer(string nameId)
        {
            for(int i=0; i< matchList.Length; i++)
            {
                matchList[i].playerLost(nameId);
            }
        }

        public void playerOfMatchLost(int matchId, string PlayerNameId)
        {
            if (matchId < 0) {
                Errors.AddBigError("unacceptable match id at playerOfMatchLost. match id: "+ matchId.ToString());
                return;                
            }
            //Log.AddMatchLog(matchId, " player " + PlayerNameId + " Of Match Lost");
            matchList[matchId].playerLost(PlayerNameId);
        }


        private static Mutex addMatch = new Mutex();
        public int AddNewMatchWithPlayerOne(string playerIdName,float playerPower, string SelectedLeage)
        {
            addMatch.WaitOne();
            int bestMatch = -1;
            for (int i = (matchList.Length - 1); 0 <= i; i--)
            {
                if (matchList[i].GivePreSituation() == PreMatchSituation.NonExistance)
                {
                    bestMatch = i;
                }
            }
            if(bestMatch == -1) {
                Errors.AddBigError(" find no empty match to add player one");
                addMatch.ReleaseMutex();
                return -1;
            }
            else
            {
                matchList[bestMatch] = new TwoPlayerMatch();
                matchList[bestMatch].StartMatchWithOnePlayer(playerIdName, playerPower, bestMatch, SelectedLeage);


                //Log.AddMatchLog(bestMatch, "added with player" + playerIdName + " as first player");
                addMatch.ReleaseMutex();
                return bestMatch;

            }
           
            //ConnectedPlayersList.SetPlayerMatch(playerIdName, bestMatch);            
        }
        

        // this function will be called ones at start of server
        public void FillArrays()
        {
            for (int i = 0; i < matchList.Length; i++)
            {
                matchList[i] = new TwoPlayerMatch();                
            }
        }

        public void PlayerLeaveMatch(int matchId, string NameId)
        {
            matchList[matchId].playerLost(NameId);            
        }


        public static DateTime ReturnMatchConnectionTime(int matchId)
        {
            return matchList[matchId].ReturnConnectionTime();
        }

    }
}