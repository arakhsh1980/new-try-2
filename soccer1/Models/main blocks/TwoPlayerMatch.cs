using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models;

namespace soccer1.Models
{
    public static class playerMatchActions {
        public static bool isBothPositionsSimilar(string json1 , string json2)
        {
            if(json1== json2)
            {
                return true;
            }
            else
            {
                Errors.AddSmallError("two different result");
                return false;
                //tag two player and make a decition for situation
            }


            
        }
    }


    
    public class TwoPlayerMatch
    {


        public TwoPlayerMatch()
        {
            preSituation = PreMatchSituation.NonExistance;
            matchNumber = -1;
        }

        public void playerLost(int playerid)
        {
            if(preSituation == PreMatchSituation.NonExistance) { Errors.AddBigError(" player on Non Existance match lost"); return; }
            if(preSituation == PreMatchSituation.WithOnePlayer && playerid==playerOneId)
            {
                GoNonExistance();
                //ConnectedPlayersList.connectionInfos[playerOneId].ActiveMatchId = -1;
                    return;
            }
            if(preSituation == PreMatchSituation.WithTwoPlayer && playerid == playerOneId)
            {
                PlayerWin(2); return;
            }
            if (preSituation == PreMatchSituation.WithTwoPlayer && playerid == playerTwoId)
            {
                PlayerWin(1); return;
            }


        }

        public void StartMatch(int StarterId, int Matchnum, string SelectedLeage)
        {
            playerOneId = StarterId;
            PlayerOnePower = ConnectedPlayersList.ReturnPlayerPowerLevel(StarterId);
           // PlayerOneShootTime = ConnectedPlayersList.connectedPlayers[StarterId].ShootTime;
            playerOnePawnsPositions = null;
            playerTwoPawnsPositions = null;
            matchNumber = Matchnum;
            league = SelectedLeage;
            CreationTime = DateTime.Now;            
            preSituation = PreMatchSituation.WithOnePlayer;
            ConnectedPlayersList.AddPlayerEvent(StarterId, ServrMasage.WatForOthr, "");            
        }

        public void AddSecondPlayerToMatch(int connectionId)
        {
            playerTwoId = connectionId;
            PlayerTwoPower = ConnectedPlayersList.ReturnPlayerPowerLevel(connectionId);
           //PlayerTwoShootTime = ConnectedPlayersList.connectedPlayers[connectionId].ShootTime;
            ConnectedPlayersList.SetPlayerMatch(playerOneId, matchNumber);
            ConnectedPlayersList.SetPlayerMatch(playerTwoId, matchNumber);
            situation = MatchSituation.WFShoot;
            preSituation = PreMatchSituation.WithTwoPlayer;
            lastShootTime = DateTime.Now;
            SendMassageToPlayers(ServrMasage.GoToMatchi, matchNumber.ToString());
            Log.AddMatchLog(matchNumber, " match started. pl1: " + playerOneId.ToString() + " . pl2: " + playerTwoId.ToString());
        }

        public void ShootHappeded(ShootActionCode shot, string shoot)
        {
            if(shot.playerID!= playerOneId && isPlayerOnTurn) { Log.AddPlayerLog(shot.playerID,"out of turn shoot " ); return; }
            if (shot.playerID != playerTwoId && !isPlayerOnTurn) { Log.AddPlayerLog(shot.playerID, "out of turn shoot "); return; }
            if (situation != MatchSituation.WFShoot) { Errors.AddSmallError("untiming shoot resived"); return; }

            playerOnePawnsPositions = null;
            playerTwoPawnsPositions = null;
            isPlayerOnTurn = !isPlayerOnTurn;
            lastShootTime = DateTime.Now;
            situation = MatchSituation.WFStationeryPositions;
            SendMassageToPlayers(ServrMasage.ActTisShot, shoot);
        }

        public void GetStationeryPostion(int SenderId , string jsonpart)
        {   
            if(situation != MatchSituation.WFStationeryPositions)
            {
                Errors.AddSmallError("untiming Stationery Positions resived");
                return;
            }
            if(SenderId== playerOneId) {
                playerOnePawnsPositions = jsonpart;
                Log.AddPlayerLog(playerOneId," Stationery position resived");
            }
            if(SenderId == playerTwoId) {
                playerTwoPawnsPositions = jsonpart;
                Log.AddPlayerLog(playerTwoId, " Stationery position resived");
            }
            if(playerOnePawnsPositions != null && playerTwoPawnsPositions != null)
            {
                if(playerMatchActions.isBothPositionsSimilar(playerOnePawnsPositions, playerTwoPawnsPositions))
                {
                    situation = MatchSituation.WFShoot;
                    Log.AddMatchLog(matchNumber, " Stationery position Accepted");
                    SendMassageToPlayers(ServrMasage.shotIsDown, "");
                }
                else
                {
                    Log.AddMatchLog(matchNumber, " Error Eroor different result of shoots");
                    Errors.AddBigError("different result of shoots");
                    situation = MatchSituation.WFShoot;
                    SendMassageToPlayers(ServrMasage.shotIsDown, "");
                }
            }
            else
            {
                //Log.AddMatchLog(matchNumber, " Error Eroor One null position");
                //Errors.AddBigError("One null position");
            }

        }
        
        public void GoalReport(int SenderId, int GoalClaim)
        {
            Log.AddPlayerLog(SenderId, " Goal Cliam : " + GoalClaim.ToString());
            if(situation != MatchSituation.WFStationeryPositions) { Errors.AddClientError("Goal claim out of time by "+ SenderId.ToString()); }
            int Scorer=0;
            //situation = MatchSituation.WFGoalClaim;
            if (SenderId == playerOneId) { playerOneGoalClaim = GoalClaim; }
            if (SenderId == playerTwoId) { playerTwoGoalClaim = GoalClaim; }
            if( playerOneGoalClaim == (-1* playerTwoGoalClaim) )
            {
                if (playerOneGoalClaim == 1)
                {
                    playerOneScore++;
                    Scorer = playerOneId;
                }
                if (playerOneGoalClaim == -1)
                {
                    playerTwoScore++;
                    Scorer = playerTwoId;
                }
                if (goalForWin <= playerOneScore) {
                    PlayerWin(1);
                    return;
                }
                if (goalForWin <= playerTwoScore)
                {
                    PlayerWin(2);
                    return;
                }
                if (Scorer == 0) { Errors.AddSmallError("different claim About Goal in Match nmumber: "+ matchNumber.ToString()); return; }
                SendMassageToPlayers(ServrMasage.PlayerGoal, Scorer.ToString());
            }  
        }        
       
        public void ItsMyTimeClaim(int SenderId) {
            if(isPlayerOnTurn && SenderId== playerOneId) { Errors.AddClientError("ItsMyTimeClaim on its turn");  return; }
            if(!isPlayerOnTurn && SenderId == playerTwoId) { Errors.AddClientError("ItsMyTimeClaim on its turn"); return; }
            TimeSpan deferentTime = DateTime.Now.Subtract(lastShootTime);
            if (isPlayerOnTurn && PlayerOneShootTime < deferentTime.TotalSeconds)
            {
                playerTimeOut();
                return;
            }
            if (!isPlayerOnTurn && PlayerOneShootTime < deferentTime.TotalSeconds)
            {
                playerTimeOut();
                return;
            }

        }

        public PreMatchSituation GivePreSituation()
        {
            return preSituation;
        }

        public string GivLeague()
        {
            return league;
        }

        public float GivePlayerOnePower()
        {
            return PlayerOnePower;
        }

        public void CheckYourSelf()
        {

        }

        public DateTime ReturnConnectionTime()
        {
            return CreationTime;
        }

        public int ReturnFirstPlayer()
        {
            return playerOneId;
        }

        public int ReturnSecondPlayer()
        {
            return playerTwoId;
        }

        //public bool isPlayersConnected()
        //{
        //    if(situation == MatchSituation.NonExistance) { return false; }
        //    if(situation == MatchSituation.WithOnePlayer)
        //        if (ConnectedPlayersList.IsConnected(playerOneId))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            GoNonExistance();
        //            return false;
        //        }
        //    if (situation == MatchSituation.WithOnePlayer)
        //        if (ConnectedPlayersList.IsConnected(playerOneId))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            GoNonExistance();
        //            return false;
        //        }

        //}
        //}


        /// <summary>
        /// /////////private part
        /// </summary>

        private void playerTimeOut()
        {
            playerOnePawnsPositions = null;
            playerTwoPawnsPositions = null;
            isPlayerOnTurn = !isPlayerOnTurn;
            lastShootTime = DateTime.Now;
            SendMassageToPlayers(ServrMasage.PlrTimeOut, "");
            situation = MatchSituation.WFShoot;
        }

        private int ChackForWinner()
        {
            if (goalForWin <= playerOneScore) { return 1; }
            if (goalForWin <= playerTwoScore) { return 2; }
            return 0;
        }


        private void PlayerWin(int winer)
        {
            Log.AddMatchLog(matchNumber, " player "+ winer.ToString() + " wined");
            if (winer == 1)
            {
                ConnectedPlayersList.PlayerWined(playerOneId, betedMoney);
                SendMassageToPlayers(ServrMasage.Winnerisii, playerOneId.ToString());
                GoNonExistance();
            }
            if (winer == 2)
            {
                ConnectedPlayersList.PlayerWined(playerTwoId, betedMoney);
                SendMassageToPlayers(ServrMasage.Winnerisii, playerTwoId.ToString());
                GoNonExistance();
            }
            Errors.AddBigError("Player three wined !");
            GoNonExistance();
        }

        private void SendMassageToPlayers(ServrMasage type, string massage)
        {
            ConnectedPlayersList.AddPlayerEvent(playerOneId, type, massage);
            ConnectedPlayersList.AddPlayerEvent(playerTwoId, type, massage);
        }
        
        private void GoNonExistance()
        {
            preSituation = PreMatchSituation.NonExistance;
            matchNumber = -1;
            playerOneId = -1;
            playerTwoId = -1;
            isPlayerOnTurn = true;
            playerOneGoalClaim = 0;
            playerTwoGoalClaim = 0;
            goalForWin = 2;
    }

        


        private int playerOneId;
        private float PlayerOnePower;
        private int playerOneScore;
        private int PlayerOneShootTime;
        private int playerTwoId;
        private float PlayerTwoPower;
        private int playerTwoScore;
        private int PlayerTwoShootTime;
        private string league ;
        private int betedMoney;
        private int matchNumber =-1;
        private bool isPlayerOnTurn =true;
        private int shootCounter = 0;
        private DateTime CreationTime ;
        private DateTime lastShootTime;
        private MatchSituation situation ;
        private string playerOnePawnsPositions;
        private string playerTwoPawnsPositions;
        private int playerOneGoalClaim = 0;
        private int playerTwoGoalClaim = 0;
        private int goalForWin=2;
        private PreMatchSituation preSituation = PreMatchSituation.NonExistance;
}
    
}