using System;
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

        
        private static TwoPlayerMatch[] matchList = new TwoPlayerMatch[1000];

       
        // reaturn best sutable match... id there is no match return -1
        public int  FindSutableMatch(float PlayerPowerLevel, string SelectedLeage)
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

        public void shootHapened(ShootActionCode shoot, string jsonpart)
        {
            matchList[shoot.MatchID].ShootHappeded(shoot, jsonpart);
        }

        public void stopedPosition(int matchId, string nameId, string jsonpart)
        {
            matchList[matchId].GetStationeryPostion(nameId, jsonpart);           
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
                matchList[bestMatch].StartMatch(playerIdName, playerPower, bestMatch, SelectedLeage);


                //Log.AddMatchLog(bestMatch, "added with player" + playerIdName + " as first player");
                addMatch.ReleaseMutex();
                return bestMatch;

            }
           
            //ConnectedPlayersList.SetPlayerMatch(playerIdName, bestMatch);            
        }
        
        public MatchMassage ReturnEvent(string playerId, int matchId)
        {
            if(0<=matchId && matchId< matchList.Length)
            {
                return matchList[matchId].ReturnEvent(playerId);
            }
            else
            {
                MatchMassage errormas = new MatchMassage();
                errormas.type = MatchMassageType.Error;
                errormas.body = "Matchlist. ReturnEvent. Error";
                return errormas;
            }
           
        }
        
        //cliam will be 1 or -1
        public void GoalClaim(int matchId, string  nameId, int Claim) {
            matchList[matchId].GoalReport(nameId, Claim);
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

        public void ItsMyTurn(int matchId, string nameId)
        {
            matchList[matchId].ItsMyTimeClaim(nameId);
        }

        public static DateTime ReturnMatchConnectionTime(int matchId)
        {
            return matchList[matchId].ReturnConnectionTime();
        }

    }
}