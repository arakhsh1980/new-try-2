using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models;

namespace soccer1.Models
{
   

    
    public class TwoPlayerMatch
    {


        public TwoPlayerMatch()
        {
            preSituation = PreMatchSituation.NonExistance;
            matchNumber = -1;
        }

        private bool isBothPositionsSimilar(string json1, string json2)
        {
            if (json1 == json2)
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

        public void playerLost(string playerid)
        {
            if(preSituation == PreMatchSituation.NonExistance) { Errors.AddBigError(" player on Non Existance match lost"); return; }
            if(preSituation == PreMatchSituation.WithOnePlayer && playerid==playerOneIdName)
            {
                GoNonExistance();
                //ConnectedPlayersList.connectionInfos[playerOneId].ActiveMatchId = -1;
                    return;
            }
            if(preSituation == PreMatchSituation.WithTwoPlayer && playerid == playerOneIdName)
            {
                PlayerWin(2); return;
            }
            if (preSituation == PreMatchSituation.WithTwoPlayer && playerid == playerTwoIdName)
            {
                PlayerWin(1); return;
            }


        }

        public void StartMatch(string StarterId, float StarterPower, int Matchnum, string SelectedLeage)
        {
            playerOneIdName = StarterId;
            PlayerOnePower = StarterPower;
           // PlayerOneShootTime = ConnectedPlayersList.connectedPlayers[StarterId].ShootTime;
            playerOnePawnsPositions = null;
            playerTwoPawnsPositions = null;
            matchNumber = Matchnum;
            league = SelectedLeage;
            CreationTime = DateTime.Now;            
            preSituation = PreMatchSituation.WithOnePlayer;
            AddPlayerEvent(StarterId, ServrMasage.WatForOthr, "");            
        }

        public void AddSecondPlayerToMatch(string connectionId,float power)
        {
            playerTwoIdName = connectionId;
            PlayerTwoPower = power;
           //PlayerTwoShootTime = ConnectedPlayersList.connectedPlayers[connectionId].ShootTime;
           
            //ConnectedPlayersList.SetPlayerMatch(playerOneId, matchNumber);
            //ConnectedPlayersList.SetPlayerMatch(playerTwoId, matchNumber);
            situation = MatchSituation.WFShoot;
            preSituation = PreMatchSituation.WithTwoPlayer;
            lastShootTime = DateTime.Now;
            SendMassageToPlayers(ServrMasage.GoToMatchi, matchNumber.ToString());
            Log.AddMatchLog(matchNumber, " match started. pl1: " + playerOneIdName.ToString() + " . pl2: " + playerTwoIdName.ToString());
        }

        public void ShootHappeded(ShootActionCode shot, string shoot)
        {
            if(shot.playerID!= playerOneIdName && isPlayerOnTurn) { Log.AddPlayerLog(shot.playerID,"out of turn shoot " ); return; }
            if (shot.playerID != playerTwoIdName && !isPlayerOnTurn) { Log.AddPlayerLog(shot.playerID, "out of turn shoot "); return; }
            if (situation != MatchSituation.WFShoot) { Errors.AddSmallError("untiming shoot resived"); return; }

            playerOnePawnsPositions = null;
            playerTwoPawnsPositions = null;
            isPlayerOnTurn = !isPlayerOnTurn;
            lastShootTime = DateTime.Now;
            situation = MatchSituation.WFStationeryPositions;
            SendMassageToPlayers(ServrMasage.ActTisShot, shoot);
        }

        public void GetStationeryPostion(string SenderId , string jsonpart)
        {   
            if(situation != MatchSituation.WFStationeryPositions)
            {
                Errors.AddSmallError("untiming Stationery Positions resived");
                return;
            }
            if(SenderId== playerOneIdName) {
                playerOnePawnsPositions = jsonpart;
                Log.AddPlayerLog(playerOneIdName," Stationery position resived");
            }
            if(SenderId == playerTwoIdName) {
                playerTwoPawnsPositions = jsonpart;
                Log.AddPlayerLog(playerTwoIdName, " Stationery position resived");
            }
            if(playerOnePawnsPositions != null && playerTwoPawnsPositions != null)
            {
                if(isBothPositionsSimilar(playerOnePawnsPositions, playerTwoPawnsPositions))
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
        
        public void GoalReport(string SenderId, int GoalClaim)
        {
            Log.AddPlayerLog(SenderId, " Goal Cliam : " + GoalClaim.ToString());
            if(situation != MatchSituation.WFStationeryPositions) { Errors.AddClientError("Goal claim out of time by "+ SenderId.ToString()); }
            string Scorer =null;
            //situation = MatchSituation.WFGoalClaim;
            if (SenderId == playerOneIdName) { playerOneGoalClaim = GoalClaim; }
            if (SenderId == playerTwoIdName) { playerTwoGoalClaim = GoalClaim; }
            if( playerOneGoalClaim == (-1* playerTwoGoalClaim) )
            {
                if (playerOneGoalClaim == 1)
                {
                    playerOneScore++;
                    Scorer = playerOneIdName;
                }
                if (playerOneGoalClaim == -1)
                {
                    playerTwoScore++;
                    Scorer = playerTwoIdName;
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
                if (Scorer == null) { Errors.AddSmallError("different claim About Goal in Match nmumber: "+ matchNumber.ToString()); return; }
                SendMassageToPlayers(ServrMasage.PlayerGoal, Scorer.ToString());
            }  
        }        
       
        public void ItsMyTimeClaim(string SenderId) {
            if(isPlayerOnTurn && SenderId== playerOneIdName) { Errors.AddClientError("ItsMyTimeClaim on its turn");  return; }
            if(!isPlayerOnTurn && SenderId == playerTwoIdName) { Errors.AddClientError("ItsMyTimeClaim on its turn"); return; }
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
            PreMatchSituation x;
            x = preSituation
            return x;
        }

        public string GivLeague()
        {
            string lg;
            lg = league;
            return lg;
        }

        public float GivePlayerOnePower()
        {
            float x;
            x= PlayerOnePower;
            return x;
        }

        public void CheckYourSelf()
        {

        }

        public DateTime ReturnConnectionTime()
        {
            DateTime time;
            time=
            return CreationTime;
        }

        public string ReturnFirstPlayer()
        {
            return playerOneIdName;
        }
        
        public string ReturnFirstPlayerByIdName()
        {
            return playerOneIdName;
        }
        public string ReturnSeccondPlayerByIdName()
        {
            return playerTwoIdName;
        }

        public string ReturnSecondPlayer()
        {
            return playerTwoIdName;
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
                //ConnectedPlayersList.PlayerWined(playerOneId, betedMoney);
                SendMassageToPlayers(ServrMasage.Winnerisii, playerOneIdName.ToString());
                GoNonExistance();
            }
            if (winer == 2)
            {
                //ConnectedPlayersList.PlayerWined(playerTwoId, betedMoney);
                SendMassageToPlayers(ServrMasage.Winnerisii, playerTwoIdName.ToString());
                GoNonExistance();
            }
            Errors.AddBigError("Player three wined !");
            GoNonExistance();
        }

        private void SendMassageToPlayers(ServrMasage type, string massage)
        {
            AddPlayerEvent(playerOneIdName, type, massage);
            AddPlayerEvent(playerTwoIdName, type, massage);
        }
        
        private void AddPlayerEvent(string Id, ServrMasage massageType, string eventMassage)
        {
            if(Id== playerOneIdName) {
                playerOneEventType = massageType;
                playerOneEvent = eventMassage;
            }
            else
            {
                playerTwoEventType = massageType;
                playerTwoEvent = eventMassage;
            }

        }

        private void GoNonExistance()
        {
            preSituation = PreMatchSituation.NonExistance;
            matchNumber = -1;
            playerOneIdName = null;
            playerTwoIdName = null;
            isPlayerOnTurn = true;
            playerOneGoalClaim = 0;
            playerTwoGoalClaim = 0;
            goalForWin = 2;
    }


        private ServrMasage playerOneEventType;
        private ServrMasage playerTwoEventType;
        private string playerOneEvent;
        private string playerTwoEvent;

        private string playerOneIdName;
        //private string IdNameplayerOneId;
        //private string IdNameplayerTwoId;
        private float PlayerOnePower;
        private int playerOneScore;
        private int PlayerOneShootTime;
        private string playerTwoIdName;
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