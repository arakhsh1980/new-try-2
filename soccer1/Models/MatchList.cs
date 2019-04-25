using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models;
using System.Web.Script.Serialization;

namespace soccer1.Models
{
    public static class MatchList
    {

        
        private static TwoPlayerMatch[] matchList = new TwoPlayerMatch[1000];

       
        // reaturn best sutable match... id there is no match return -1
        public static int FindSutableMatch(float PlayerPowerLevel, string SelectedLeage)
        {
            float bestpowerDiference = float.MaxValue;
            int bestMatch = -1;
            
            for (int i = (matchList.Length - 1); 0 <= i; i--)
            {
                matchList[i].CheckYourSelf();
                if (matchList[i].GivePreSituation() == PreMatchSituation.WithOnePlayer && matchList[i].GivLeague() == SelectedLeage)
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

        public static int ReturnOpponentOf(int connectionId, int matchId)
        {
            int firstId=matchList[matchId].ReturnFirstPlayer();
            int SecId = matchList[matchId].ReturnSecondPlayer();
            if(firstId== connectionId) { return SecId; }
            if (SecId == connectionId) { return firstId; }
            Errors.AddBigError("this player is not in its clamed match");
            return -1;
        }
        public static void AddSecondPlayerToMatch(int matchNumber , int playerConnId)
        {
            matchList[matchNumber].AddSecondPlayerToMatch(playerConnId);
            Log.AddMatchLog(matchNumber, "Second player added to match by id:"+ playerConnId.ToString());
            //ConnectedPlayersList.SetPlayerMatch(playerConnId, matchNumber);
        }

        public static void shootHapened(ShootActionCode shoot, string jsonpart)
        {
            matchList[shoot.MatchID].ShootHappeded(shoot, jsonpart);
        }

        public static void stopedPosition(int matchId, int ConnectionId, string jsonpart)
        {
            matchList[matchId].GetStationeryPostion(ConnectionId, jsonpart);           
        }
        
        public static void playerOfMatchLost(int matchId, int playerId)
        {
            if (matchId < 0) {
                Errors.AddBigError("unacceptable match id at playerOfMatchLost. match id: "+ matchId.ToString());
                return;                
            }
            Log.AddMatchLog(matchId, " player " + ConnectedPlayersList.ReturnIdbyConnId(playerId) + " Of Match Lost");
            matchList[matchId].playerLost(playerId);
        }

        public static void AddNewMatchWithPlayerOne(int playerConnId, string SelectedLeage)
        {
            int bestMatch = -1;
            for (int i = (matchList.Length - 1); 0 <= i; i--)
            {
                if (matchList[i].GivePreSituation() == PreMatchSituation.NonExistance)
                {
                    bestMatch = i;
                }
            }
            if(bestMatch == -1) { Errors.AddBigError(" find no empty match to add player one"); return; }
            matchList[bestMatch].StartMatch(playerConnId, bestMatch, SelectedLeage);
            

            Log.AddMatchLog(bestMatch ,  "added with player" + playerConnId + " as first player");
            ConnectedPlayersList.SetPlayerMatch(playerConnId, bestMatch);            
        }
            
        
        //cliam will be 1 or -1
        public static void GoalClaim(int matchId, int ConnectionId, int Claim) {
            matchList[matchId].GoalReport(ConnectionId, Claim);
        }

        // this function will be called ones at start of server
        public static void FillArrays()
        {
            for (int i = 0; i < matchList.Length; i++)
            {
                matchList[i] = new TwoPlayerMatch();                
            }
        }

        public static void PlayerLeaveMatch(int matchId, int ConnectionId)
        {
            matchList[matchId].playerLost(ConnectionId);            
        }

        public static void ItsMyTurn(int matchId, int ConnectionId)
        {
            matchList[matchId].ItsMyTimeClaim(ConnectionId);
        }

        public static DateTime ReturnMatchConnectionTime(int matchId)
        {
            return matchList[matchId].ReturnConnectionTime();
        }

    }
}