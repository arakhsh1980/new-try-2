using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models;
using soccer1.Models.main_blocks;
using System.Threading;
using System.Web.Script.Serialization;
using soccer1.Models.DataBase;
using System.Data.Entity;


namespace soccer1.Models.main_blocks
{
    public struct GainedXp
    {
        public int[] AssingedIndex  ;
        public int[] xpVAl ;
    }

    public class symShootMatch
    {


        public symShootMatch()
        {
            isFirstReboot = true;
            RebootMatch();
        }

        private Mutex mainMutex = new Mutex();
        private Mutex eventMutex = new Mutex();
        private Mutex TimeControlerMutex = new Mutex();

        private DataDBContext dataBase = new DataDBContext();

        #region every turn action

        public bool SubistitutionsHappened(string PlayerId, int TurnNumber, string jsonpart)
        {
            //bool result = false;
            if (TurnNumber != currentTurn) { return false; }
            if (PlayerId == playerOneIdName)
            {
                AddPlayerEvent(playerTwoIdName, MatchMassageType.Substituti, jsonpart);
            }
            if (PlayerId == playerTwoIdName)
            {
                AddPlayerEvent(playerOneIdName, MatchMassageType.Substituti, jsonpart);
            }
            return true;
        }



        public bool ElixirUseHappened(string PlayerId, int TurnNumber, string jsonpart)
        {
            bool result = false;
            if (TurnNumber != currentTurn) { return false; }
            if (PlayerId == playerOneIdName)
            {               
                AddPlayerEvent(playerTwoIdName, MatchMassageType.ElixirUsea, jsonpart);
            }
            if (PlayerId == playerTwoIdName)
            {
                AddPlayerEvent(playerOneIdName, MatchMassageType.ElixirUsea, jsonpart);
            }
            return result;
        }

        
        public bool ShootHappeded(string playerIDName, int TurnNumber, string shoot)
        {
            WFShootStartTime = TimeFromStart();
            bool result = false;
            Log.AddMatchLog(matchNumber, "twoPlayerMatch.ShootHappeded.shooter:" + playerIDName);
            if (TurnNumber != currentTurn) { Errors.AddSmallError("untiming shoot resived. out of turn"); return false; }
            if (situation != MatchSituation.WFShoot)
            {
                Errors.AddSmallError("ShootHappeded.untiming Stationery Positions resived");
                return false;
            }
            mainMutex.WaitOne();
            if (playerIDName == playerOneIdName && playerOneShoot==null)
            {
                playerOneShoot = shoot;
                //pl1PawnShooterAssingedIndex = PawnAssignIndex;
                AddXpToShooter(true, NominatedXperiance.simpleShootXp);
                
                
                Log.AddPlayerLog(playerOneIdName, " ShootHappeded resived");
                result = true;
            }
            if (playerIDName == playerTwoIdName && playerTwoShoot == null)
            {
                playerTwoShoot = shoot;
                //pl2PawnShooterAssingedIndex = PawnAssignIndex;
                AddXpToShooter(false, NominatedXperiance.simpleShootXp);                
                Log.AddPlayerLog(playerTwoIdName, " ShootHappeded resived");
                result = true;
            }
            if (playerOneShoot != null && playerTwoShoot != null)
            {
                eventMutex.WaitOne();
                lastShootTime = DateTime.Now;
                situation = MatchSituation.WFStationeryPositions;
                SendMassageToPlayers(MatchMassageType.ActTisShot, playerOneShoot);
                SendMassageToPlayers(MatchMassageType.ActTisShot, playerTwoShoot);
                playerOneShoot = null;
                playerTwoShoot = null;
                eventMutex.ReleaseMutex();
            }
           
            mainMutex.ReleaseMutex();
            return result;
        }

        public void GetStationeryPostion(string SenderId, int TurnNumber, string jsonpart)
        {
            Log.AddMatchLog(matchNumber, "twoPlayerMatch.GetStationeryPostion.SenderId:" + SenderId);
            if (TurnNumber != currentTurn) { Errors.AddSmallError("untiming shoot resived. out of turn"); return; }
            if (IsgoalHappenInTurn)
            {
                isUnSolvedStationeryPostion = false;
                return;
            }
            if (situation != MatchSituation.WFStationeryPositions)
            {
                Errors.AddSmallError("GetStationeryPostion.untiming Stationery Positions resived");
                return;
            }
            TimeofLastStationeryPosition = TimeFromStart();
            isUnSolvedStationeryPostion = true;
            mainMutex.WaitOne();
            if (SenderId == playerOneIdName)
            {
                playerOnePawnsPositions = jsonpart;
                Log.AddPlayerLog(playerOneIdName, " GetStationeryPostion.Stationery position resived");
            }
            if (SenderId == playerTwoIdName)
            {
                playerTwoPawnsPositions = jsonpart;
                Log.AddPlayerLog(playerTwoIdName, " GetStationeryPostion.Stationery position resived");
            }
            if (playerOnePawnsPositions != null && playerTwoPawnsPositions != null)
            {
                if (isBothPositionsSimilar(playerOnePawnsPositions, playerTwoPawnsPositions))
                {
                    situation = MatchSituation.WFShoot;

                    Log.AddMatchLog(matchNumber, " GetStationeryPostion.Stationery position Accepted");
                    playerOnePawnsPositions = null;
                    playerTwoPawnsPositions = null;
                    isUnSolvedStationeryPostion = false;
                    StartNextTurn();
                    //SendMassageToPlayers(MatchMassageType.shotIsDown, "");
                }
                else
                {
                    Log.AddMatchLog(matchNumber, "GetStationeryPostion. Error Eroor different result of shoots");
                    Errors.AddBigError("different result of shoots");
                    situation = MatchSituation.WFShoot;
                    //SendMassageToPlayers(MatchMassageType.shotIsDown, "");
                }
            }
            else
            {
                //Log.AddMatchLog(matchNumber, " Error Eroor One null position");
                //Errors.AddBigError("One null position");
            }
            mainMutex.ReleaseMutex();
        }

        public void GoalReport(string SenderId, int TurnNumber, int GoalClaim)
        {
            if (TurnNumber != currentTurn) { Errors.AddSmallError("untiming goal resived. out of turn"); return; }
            Log.AddMatchLog(matchNumber, "twoPlayerMatch.GoalReport.SenderId:" + SenderId);
            mainMutex.WaitOne();
            if (situation != MatchSituation.WFStationeryPositions) { Errors.AddClientError("Goal claim out of time by " + SenderId.ToString()); }
            isUnSolvedGoalClaim = true;
            ThisTurnScorerID = null;
            //situation = MatchSituation.WFGoalClaim;
            if (SenderId == playerOneIdName) { playerOneGoalClaim = GoalClaim; }
            if (SenderId == playerTwoIdName) { playerTwoGoalClaim = GoalClaim; }
            if (playerOneGoalClaim == (-1 * playerTwoGoalClaim))
            {
                if (playerOneGoalClaim == 1)
                {
                    IsgoalHappenInTurn = true;
                    AddXpToShooter(true, NominatedXperiance.GoalXp);
                    AddXpToTeam(true, NominatedXperiance.GoalScorerTeamXp);
                    playerOneScore++;
                    Log.AddMatchLog(matchNumber, "twoPlayerMatch.GoalReport.playerOneScore:" + playerOneScore);
                    ThisTurnScorerID = playerOneIdName;
                    isPlayerOneTurn = false;
                    playerOnePawnsPositions = null;
                    playerTwoPawnsPositions = null;
                    isUnSolvedGoalClaim = false;

                }
                if (playerOneGoalClaim == -1)
                {
                    IsgoalHappenInTurn = true;
                    AddXpToShooter(false, NominatedXperiance.GoalXp);
                    AddXpToTeam(false, NominatedXperiance.GoalScorerTeamXp);
                    playerTwoScore++;
                    Log.AddMatchLog(matchNumber, "twoPlayerMatch.GoalReport.playerTwoScore:" + playerTwoScore);
                    ThisTurnScorerID = playerTwoIdName;
                    isPlayerOneTurn = true;
                    playerOnePawnsPositions = null;
                    playerTwoPawnsPositions = null;
                    isUnSolvedGoalClaim = false;

                }
                if (goalForWin <= playerOneScore)
                {
                    PlayerWin(true);
                    mainMutex.ReleaseMutex();
                    return;
                }
                if (goalForWin <= playerTwoScore)
                {
                    PlayerWin(false);
                    mainMutex.ReleaseMutex();
                    return;
                }
                if (ThisTurnScorerID == null)
                {
                    Errors.AddSmallError("different claim About Goal in Match nmumber: " + matchNumber.ToString());
                    mainMutex.ReleaseMutex();
                    return;
                }
                SendMassageToPlayers(MatchMassageType.PlayerGoal, ThisTurnScorerID);
                StartNextTurn();
            }
            mainMutex.ReleaseMutex();
        }

        public string ReturnEvent(string playerIdName)
        {
            if (preSituation == PreMatchSituation.TwoPlayerInPlay ) CheckIfPlayersInTheMatch();
            IsPreSituationStillValid();
            eventMutex.WaitOne();

            string uu = "Error";
           

            if (playerIdName == playerOneIdName)
            {
                //uu = NothingNewEventString;
                uu = new JavaScriptSerializer().Serialize(playerOneEvents);
                playerOneEvents.EventTypes.Clear();
                playerOneEvents.desitionBodys.Clear();
                isUnreadEventForPlayerOne = false;
                playerOneLastEventReadTime = TimeFromStart();
            }
            else
            {
                if (playerIdName == playerTwoIdName)
                {
                    //uu = NothingNewEventString;
                    uu = new JavaScriptSerializer().Serialize(playerTwoEvents);
                    playerTwoEvents.EventTypes.Clear();
                    playerTwoEvents.desitionBodys.Clear();
                    isUnreadEventForPlayerTwo = false;
                    playerTwoLastEventReadTime = TimeFromStart();
                }
                else
                {
                    Errors.AddBigError("symShootMatch.ReturnEvent. player id is not playerOneIdName or playerTwoEvents ");
                }
            }
            eventMutex.ReleaseMutex();
            if (!isPlayerOneInMatch) { isUnreadEventForPlayerOne = false; }
            if (!isPlayerTwoInMatch) { isUnreadEventForPlayerTwo = false; }
            if (!isUnreadEventForPlayerOne && !isUnreadEventForPlayerTwo && situation == MatchSituation.EndedPlay)
            {
                RebootMatch();
            }
            return uu;
        }

       

        #endregion


        #region Resiving funcitons

        public void TimerIsUp(string PlayerId, int turn)
        {
            if(situation == MatchSituation.EndedPlay) { return ; }
            Log.AddMatchLog(matchNumber, "twoPlayerMatch.ItsMyTimeClaim.SenderId:" + PlayerId);
            mainMutex.WaitOne();
            if (lastTurnfinishWithTimerUp < turn)
            {
                lastTurnfinishWithTimerUp = turn;
                TimeSpan deferentTime = DateTime.Now.Subtract(lastShootTime);
                if (PlayerId == playerOneIdName)
                {
                    float timeFromLastEventRead = TimeFromStart() - playerTwoLastEventReadTime;
                    if (Statistics.ConnectionTimeOut < timeFromLastEventRead)
                    {
                        PlayerWin(true);
                        mainMutex.ReleaseMutex();
                        return;
                    }
                }
                else
                {
                    float timeFromLastEventRead = TimeFromStart() - playerOneLastEventReadTime;
                    if (Statistics.ConnectionTimeOut < timeFromLastEventRead)
                    {
                        PlayerWin(false);
                        mainMutex.ReleaseMutex();
                        return;
                    }
                }
                StartNextTurn();
            }
            mainMutex.ReleaseMutex();
        }
       

        public string PlayerOneNotAcceptedToPlay(string playerId)
        {
            if(playerOneIdName != playerId) { return false.ToString(); }
            Log.AddMatchLog(matchNumber, "twoPlayerMatch.StartMatch.StarterId:" + playerTwoIdName);
            string oldplayerTwoIdName = playerTwoIdName;
            float oldPlayerTwoPower = PlayerTwoPower;
            int  oldmatchNumber = matchNumber;
            string  oldleague = league;
            RebootMatch();
            InitiatMatchWithOnePlayer(oldplayerTwoIdName, oldPlayerTwoPower, oldmatchNumber, oldleague);
            preSituation = PreMatchSituation.WFSecondPlayerAtHome;
            AddPlayerEvent(playerTwoIdName, MatchMassageType.WatForOthr, "");
            return true.ToString();
        }


        public string PlayerOnePlayAccepted(string playerId, int GatheredMoney)
        {
            if (playerOneIdName != playerId) { return false.ToString(); }
            Log.AddMatchLog(matchNumber, "twoPlayerMatch.StartMatch.StarterId:" + playerTwoIdName);
            GatheredMoneyForWaiting = GatheredMoney;
            StartMatch();
            isPlayerOneInMatch = true;
            return true.ToString();
        }

        
        public string PlayerOneGoingHomeAndWaiting(string playerId)
        {
            if (playerOneIdName != playerId) { return false.ToString(); }
            preSituation = PreMatchSituation.WFSecondPlayerAtHome;
            return true.ToString();
        }

        public void InitiatMatchWithOnePlayer(string StarterId, float StarterPower, int Matchnum, string SelectedLeage)
        {
            Log.AddMatchLog(matchNumber, "symShootMatch.InitiatMatchWithOnePlayer.seconrPlayerID:" + StarterId);
            startMatchTime = DateTime.Now;
            mainMutex.WaitOne();
            playerOneLastEventReadTime = TimeFromStart();
            playerOneIdName = StarterId;
            PlayerOnePower = StarterPower;
            // PlayerOneShootTime = ConnectedPlayersList.connectedPlayers[StarterId].ShootTime;
            playerOnePawnsPositions = null;
            playerTwoPawnsPositions = null;
            matchNumber = Matchnum;
            league = SelectedLeage;
            CreationTime = DateTime.Now;
            preSituation = PreMatchSituation.WFSecondPlayer;
            betedMoney = new LeaugeManager().LeaugEnterencePice(SelectedLeage);
            //AddPlayerEvent(StarterId, MatchMassageType.WatForOthr, "");
            mainMutex.ReleaseMutex();
            isPlayerOneInMatch = true;
        }

        public void StartMatch()
        {
            Log.AddMatchLog(matchNumber, "startMatch" );
            mainMutex.WaitOne();
            
            PlayerForDatabase player1 = dataBase.playerInfoes.Find(playerOneIdName);
            PlayerForDatabase player2 = dataBase.playerInfoes.Find(playerTwoIdName);
            if (player1 == null || player2 == null) { RebootMatch(); return; }
            PlayerForConnectedPlayer pl1 = new PlayerForConnectedPlayer();
            PlayerForConnectedPlayer pl2 = new PlayerForConnectedPlayer();
            pl1.reWriteAccordingTo(player1);
            pl2.reWriteAccordingTo(player2);
            Property p1Eprice = new Property();
            p1Eprice.DeepCopey(betedMoney);
            p1Eprice.Alminum -= GatheredMoneyForWaiting;
            if (p1Eprice.Alminum < 0) { p1Eprice.Alminum = 0; }
            pl1.SubtractProperty(p1Eprice);
            pl2.SubtractProperty(betedMoney);
            /*
            player1.changePlayer(pl1.returnDataBaseVersion());
            dataBase.Entry(player1).State = EntityState.Modified;
            player2.changePlayer(pl2.returnDataBaseVersion());
            dataBase.Entry(player2).State = EntityState.Modified;
            dataBase.SaveChanges();
            */
            //PlayerTwoShootTime = ConnectedPlayersList.connectedPlayers[connectionId].ShootTime;

            //ConnectedPlayersList.SetPlayerMatch(playerOneId, matchNumber);
            //ConnectedPlayersList.SetPlayerMatch(playerTwoId, matchNumber);
            situation = MatchSituation.WFShoot;
            preSituation = PreMatchSituation.TwoPlayerInPlay;
            lastShootTime = DateTime.Now;
            currentTurn = 1;
            string betedMoneyStr = new JavaScriptSerializer().Serialize(betedMoney);
            //SendMassageToPlayers(MatchMassageType.SubBetdMon, betedMoneyStr);
            SendMassageToPlayers(MatchMassageType.GoToMatchi, matchNumber.ToString());
            Log.AddMatchLog(matchNumber, " match started. pl1: " + playerOneIdName.ToString() + " . pl2: " + playerTwoIdName.ToString());
            
            WFShootStartTime = TimeFromStart();
            mainMutex.ReleaseMutex();
        }


        public void AddSecondPlayerStartImideatly(string secondPlayerID, float power)
        {
            Log.AddMatchLog(matchNumber, "symShootMatch.AddSecondPlayerStartImideatly.seconrPlayerID:" + secondPlayerID);            
            playerTwoIdName = secondPlayerID;
            playerTwoLastEventReadTime = TimeFromStart();
            PlayerTwoPower = power;
            isPlayerTwoInMatch = true;
            StartMatch();
        }

        public void AddSecondPlayerAndWaitForFisrtRespond(string seconrPlayerID, float power)
        {
            Log.AddMatchLog(matchNumber, "twoPlayerMatch.AddSecondPlayerAndWaitForFisrtRespond.seconrPlayerID:" + seconrPlayerID);
            mainMutex.WaitOne();
            playerTwoLastEventReadTime = TimeFromStart();
            playerTwoIdName = seconrPlayerID;
            PlayerTwoPower = power;
            //PlayerTwoShootTime = ConnectedPlayersList.connectedPlayers[connectionId].ShootTime;

            //ConnectedPlayersList.SetPlayerMatch(playerOneId, matchNumber);
            //ConnectedPlayersList.SetPlayerMatch(playerTwoId, matchNumber);
            situation = MatchSituation.WFFirstAcceptance;
            preSituation = PreMatchSituation.WFFirstAcceptance;
            startOfWFFirstAcceptance = TimeFromStart();
            AddPlayerEvent(playerOneIdName, MatchMassageType.DoYouPlayy, matchNumber.ToString());
            isPlayerTwoInMatch = true;
            mainMutex.ReleaseMutex();
        }



        #endregion

#region un Expected event

        public void playerLostOrCenceled(string playerId)
        {
            if (preSituation == PreMatchSituation.NonExistance) { return; }
            if(playerId ==null) { return; }
            if (playerId != playerOneIdName && playerId != playerTwoIdName) { return ; }
            if(playerId == playerOneIdName) { isPlayerOneInMatch = false; }
            if (playerId == playerTwoIdName) { isPlayerTwoInMatch = false; }

            switch (preSituation)
            {
                case PreMatchSituation.EndedPlay:
                    if(!isPlayerOneInMatch && !isPlayerTwoInMatch)
                    {
                        RebootMatch();
                    }
                    break;

                case PreMatchSituation.TwoPlayerInPlay:
                    if (playerId == playerOneIdName)
                    {
                        PlayerWin(false);
                    }
                    if (playerId == playerTwoIdName)
                    {
                        PlayerWin(true);
                    }
                    break;

                case PreMatchSituation.WFSecondPlayer:
                    if (playerId == playerOneIdName)
                    {
                        RebootMatch();
                    }
                    break;

                case PreMatchSituation.WFSecondPlayerAtHome:
                    if (playerId == playerOneIdName)
                    {
                        PlayerOneNotAcceptedToPlay(playerOneIdName);
                    }                    
                    break;

                case PreMatchSituation.WFFirstAcceptance:
                    if (playerId == playerOneIdName)
                    {
                        PlayerOneNotAcceptedToPlay(playerOneIdName);
                    }
                    if (playerId == playerTwoIdName)
                    {
                        AddPlayerEvent(playerOneIdName, MatchMassageType.pl2Cancled, matchNumber.ToString());
                        preSituation = PreMatchSituation.WFSecondPlayer;
                    }
                    break;
            }
        }

        public bool IsPreSituationStillValid() 
        {
            if (preSituation == PreMatchSituation.NonExistance) { RebootMatch(); return true; }
            if (Statistics.MaxMatchTimeInSeconds < (float)DateTime.Now.Subtract(startMatchTime).TotalSeconds ) {
                RebootMatch();
                return false;
            }
            PreMatchSituation BeforCheckSituation = preSituation;
            CheckIfPlayersInTheMatch();
            if(BeforCheckSituation != preSituation) { return false; }
            bool result = false ;
            switch (preSituation)
            {                
                case PreMatchSituation.EndedPlay:
                    if(isPlayerOneInMatch || isPlayerTwoInMatch)
                    {                        
                        result = true;
                    }
                    else
                    {
                        RebootMatch();
                        result = false;
                    }
                    break;

                case PreMatchSituation.WFFirstAcceptance:
                    if (isPlayerOneInMatch && isPlayerTwoInMatch && (TimeFromStart()-startOfWFFirstAcceptance)< Statistics.timeForFisrtRespondOnWFFirstAcceptance)
                    {
                        result = true;
                    }
                    else
                    {                        
                        RebootMatch();
                        result = false;
                    }
                    break;

                case PreMatchSituation.WFSecondPlayer:
                    if (isPlayerOneInMatch)
                    {
                        result = true;
                    }
                    else
                    {
                        RebootMatch();
                        result = false;
                    }
                    break;

                case PreMatchSituation.WFSecondPlayerAtHome:
                    if (isPlayerOneInMatch)
                    {
                        result = true;
                    }
                    else
                    {
                        RebootMatch();
                        result = false;
                    }
                    break;

                case PreMatchSituation.TwoPlayerInPlay:
                    if (isPlayerOneInMatch && isPlayerTwoInMatch)
                    {
                        result = true;
                    }
                    else
                    {
                        CheckIfPlayersInTheMatch();
                        result = false;
                    }
                    break;


                default:
                    break;

            }
            return result;
        }

        private void CheckIfPlayersInTheMatch()
        {
            if (Statistics.ConnectionTimeOut < TimeFromStart() - playerOneLastEventReadTime)
            {
                isPlayerOneInMatch = false;
                playerLostOrCenceled(playerOneIdName);
            }
            if (Statistics.ConnectionTimeOut < TimeFromStart() - playerTwoLastEventReadTime)
            {
                isPlayerTwoInMatch = false;
                playerLostOrCenceled(playerTwoIdName);
            }
        }


        #endregion






        #region return value functions

        public PreMatchSituation GivePreSituation()
        {
            mainMutex.WaitOne();
            PreMatchSituation x;
            x = preSituation;
            mainMutex.ReleaseMutex();
            return x;

        }

        public string GivLeague()
        {
            mainMutex.WaitOne();
            string lg;
            lg = league;
            mainMutex.ReleaseMutex();
            return lg;

        }

        public float GivePlayerOnePower()
        {
            mainMutex.WaitOne();
            float x;
            x = PlayerOnePower;
            mainMutex.ReleaseMutex();
            return x;
        }

        public DateTime ReturnConnectionTime()
        {
            mainMutex.WaitOne();
            DateTime time;
            time = CreationTime;
            mainMutex.ReleaseMutex();
            return time;
        }

        public string ReturnFirstPlayerByIdName()
        {
            mainMutex.WaitOne();
            string name;
            name = playerOneIdName;
            mainMutex.ReleaseMutex();
            return name;
        }
        public string ReturnSeccondPlayerByIdName()
        {
            mainMutex.WaitOne();
            string name;
            name = playerTwoIdName;
            mainMutex.ReleaseMutex();
            return name;
        }


        #endregion







        #region inner functions



        /*
        void matchTimeControler()
        {
            TimeControlerMutex.WaitOne();
            switch (situation)
            {
                case MatchSituation.WFShoot:
                    if (Statistics.AcceptedWFShootTime + 20.0f < TimeFromStart() - WFShootStartTime)
                    {
                        checkPlayersConnectivity();
                        StartNextTurn();
                    }
                    break;
                case MatchSituation.WFStationeryPositions:
                    if (playerOnePawnsPositions != null || playerTwoPawnsPositions != null)
                    {
                        if (Statistics.AcceptedTimeofStationeryPositionDifference < TimeFromStart() - TimeofLastStationeryPosition)
                        {
                            if (playerOneGoalClaim != 0 || playerTwoGoalClaim != 0)
                            {
                                AcceptGoalReport();
                            }
                            else
                            {
                                checkPlayersConnectivity();
                                StartNextTurn();
                            }

                        }
                    }
                    break;
            }
            TimeControlerMutex.ReleaseMutex();


        }
        */
        private void SendGainedXpToPlayers()
        {
            string uu = new JavaScriptSerializer().Serialize(pl1GainedXp);
            AddPlayerEvent(playerOneIdName, MatchMassageType.gaindedXps, uu);
            string oo = new JavaScriptSerializer().Serialize(pl2GainedXp);
            AddPlayerEvent(playerTwoIdName, MatchMassageType.gaindedXps, oo);
        }

        

        private void AddXpToTeam(bool isplyerOne, int xpval)
        {
            if (isplyerOne)// is player on?
            {
                pl1GainedXp.xpVAl[pl1GainedXp.xpVAl.Length - 1] += xpval;
            }
            else
            {
                pl2GainedXp.xpVAl[pl2GainedXp.xpVAl.Length - 1] += xpval;
            }
        }


        private void AddXpToShooter(bool isplayerOne ,int xpval)
        {
            int placeInArray = -1;
            if(isplayerOne)// is player on?
            {
                if (pl1PawnShooterAssingedIndex < 0) { return; }
                
                
                for (int i =0; i< pl1GainedXp.AssingedIndex.Length; i++) if (pl1GainedXp.AssingedIndex[i] == pl1PawnShooterAssingedIndex)
                    {
                        placeInArray = i;
                    }
                if (placeInArray < 0)
                {
                    int firstNullPlace = -1;
                    for (int i = pl1GainedXp.AssingedIndex.Length - 1; -1 < i; i--) if (pl1GainedXp.AssingedIndex[i] == -1) { firstNullPlace = i; }
                    pl1GainedXp.AssingedIndex[firstNullPlace] = pl1PawnShooterAssingedIndex;
                    pl1GainedXp.xpVAl[firstNullPlace] = xpval;
                }
                else
                {
                    pl1GainedXp.xpVAl[placeInArray] += xpval;
                }                
            }
            else
            {
                if (pl2PawnShooterAssingedIndex < 0) { return; }
                for (int i = 0; i < pl2GainedXp.AssingedIndex.Length; i++) if (pl2GainedXp.AssingedIndex[i] == pl2PawnShooterAssingedIndex)
                    {
                        placeInArray = i;
                    }
                if (placeInArray < 0)
                {
                    int firstNullPlace = -1;
                    for (int i = pl2GainedXp.AssingedIndex.Length - 1; -1 < i; i--) if (pl2GainedXp.AssingedIndex[i] == -1) { firstNullPlace = i; }
                    pl2GainedXp.AssingedIndex[firstNullPlace] = pl2PawnShooterAssingedIndex;
                    pl2GainedXp.xpVAl[firstNullPlace] = xpval;
                }
                else
                {
                    pl2GainedXp.xpVAl[placeInArray] += xpval;
                }
            }
        }

      

        private void AcceptGoalReport()
        {


            mainMutex.WaitOne();
            if (playerOneGoalClaim != 0)
            {
                playerTwoGoalClaim = -1 * playerOneGoalClaim;
            }
            else
            {
                playerOneGoalClaim = -1 * playerTwoGoalClaim;
            }
            if (playerOneGoalClaim == (-1 * playerTwoGoalClaim))
            {
                if (playerOneGoalClaim == 1)
                {
                    IsgoalHappenInTurn = true;
                    playerOneScore++;
                    Log.AddMatchLog(matchNumber, "twoPlayerMatch.GoalReport.playerOneScore:" + playerOneScore);
                    ThisTurnScorerID = playerOneIdName;
                    isPlayerOneTurn = false;
                    playerOnePawnsPositions = null;
                    playerTwoPawnsPositions = null;
                    isUnSolvedGoalClaim = false;

                }
                if (playerOneGoalClaim == -1)
                {
                    IsgoalHappenInTurn = true;
                    playerTwoScore++;
                    Log.AddMatchLog(matchNumber, "twoPlayerMatch.GoalReport.playerTwoScore:" + playerTwoScore);
                    ThisTurnScorerID = playerTwoIdName;
                    isPlayerOneTurn = true;
                    playerOnePawnsPositions = null;
                    playerTwoPawnsPositions = null;
                    isUnSolvedGoalClaim = false;

                }
                if (goalForWin <= playerOneScore)
                {
                    PlayerWin(true);
                    mainMutex.ReleaseMutex();
                    return;
                }
                if (goalForWin <= playerTwoScore)
                {
                    PlayerWin(false);
                    mainMutex.ReleaseMutex();
                    return;
                }
                if (ThisTurnScorerID == null)
                {
                    Errors.AddSmallError("different claim About Goal in Match nmumber: " + matchNumber.ToString());
                    mainMutex.ReleaseMutex();
                    return;
                }
                SendMassageToPlayers(MatchMassageType.PlayerGoal, ThisTurnScorerID);
                StartNextTurn();
            }
            mainMutex.ReleaseMutex();
        }

        private void StartNextTurn()
        {

            if (isUnSolvedGoalClaim) { return; }
            playerOnePawnsPositions = null;
            playerTwoPawnsPositions = null;
            playerOneGoalClaim = 0;
            playerTwoGoalClaim = 0;
            isPlayerOneTurn = !isPlayerOneTurn;
            lastShootTime = DateTime.Now;
            situation = MatchSituation.WFShoot;
            currentTurn++;
            if (IsgoalHappenInTurn) if (ThisTurnScorerID == playerOneIdName)
                {
                    isPlayerOneTurn = false;
                }
                else
                {
                    isPlayerOneTurn = true;
                }
            IsgoalHappenInTurn = false;
            playerOneGoalClaim = 0;
            playerTwoGoalClaim = 0;
            WFShootStartTime = TimeFromStart();
            pl1PawnShooterAssingedIndex = -1;
            pl2PawnShooterAssingedIndex = -1;
            if (isPlayerOneTurn)
            {
                SendMassageToPlayers(MatchMassageType.ChangeTurn, playerOneIdName);
            }
            else
            {
                SendMassageToPlayers(MatchMassageType.ChangeTurn, playerTwoIdName);
            }
        }

        private void PlayerWin(bool isPlayerOneWinner)
        {
            if(preSituation== PreMatchSituation.EndedPlay) { return; }
            if (isPlayerOneWinner)
            {
                AddXpToTeam(true, NominatedXperiance.WinnerTeamXp);
                AddXpToTeam(false, NominatedXperiance.LosserTeamXp);
                //ConnectedPlayersList.PlayerWined(playerOneId, betedMoney);
                Log.AddMatchLog(matchNumber, "twoPlayerMatch.PlyaerWin. player: " + playerOneIdName + " wined");
                SendGainedXpToPlayers();

                mainMutex.WaitOne();
                PlayerForDatabase player1 = dataBase.playerInfoes.Find(playerOneIdName);                
                if (player1 == null ) { RebootMatch(); return; }
                PlayerForConnectedPlayer pl1 = new PlayerForConnectedPlayer();                
                pl1.reWriteAccordingTo(player1);
                pl1.GainMatchXp(pl1GainedXp);
                pl1.AddProperty(betedMoney);
                pl1.AddProperty(betedMoney);
                Property waitingMoney = new Property();
                waitingMoney.SetZiro();
                waitingMoney.Alminum += GatheredMoneyForWaiting;
                pl1.AddProperty(waitingMoney);
                pl1.SaveChanges();
                /*
                player1.changePlayer(pl1.returnDataBaseVersion());
                dataBase.Entry(player1).State = EntityState.Modified;
                dataBase.SaveChanges();
                */

                PlayerForDatabase player2 = dataBase.playerInfoes.Find(playerTwoIdName);
                if (player2 == null) { RebootMatch(); return; }
                PlayerForConnectedPlayer pl2 = new PlayerForConnectedPlayer();
                pl2.reWriteAccordingTo(player2);
                pl2.GainMatchXp(pl2GainedXp);
                /*
                player2.changePlayer(pl2.returnDataBaseVersion());
                dataBase.Entry(player2).State = EntityState.Modified;
                dataBase.SaveChanges();
                */




                mainMutex.ReleaseMutex();

                SendMassageToPlayers(MatchMassageType.Winnerisii, playerOneIdName);
                situation = MatchSituation.EndedPlay;
                preSituation = PreMatchSituation.EndedPlay;

            }
            else
            {
                AddXpToTeam(false, NominatedXperiance.WinnerTeamXp);
                AddXpToTeam(true, NominatedXperiance.LosserTeamXp);
                //ConnectedPlayersList.PlayerWined(playerTwoId, betedMoney);
                Log.AddMatchLog(matchNumber, "twoPlayerMatch.PlyaerWin. player: " + playerTwoIdName + " wined");
                
                SendGainedXpToPlayers();

                mainMutex.WaitOne();
                
                PlayerForDatabase player2 = dataBase.playerInfoes.Find(playerTwoIdName);
                if (player2 == null) { RebootMatch(); return; }
                
                PlayerForConnectedPlayer pl2 = new PlayerForConnectedPlayer();
                
                pl2.reWriteAccordingTo(player2);
                pl2.GainMatchXp(pl2GainedXp);
                pl2.AddProperty(betedMoney);
                pl2.AddProperty(betedMoney);
                /*
                player2.changePlayer(pl2.returnDataBaseVersion());
                dataBase.Entry(player2).State = EntityState.Modified;
                dataBase.SaveChanges();
                */
                PlayerForDatabase player1 = dataBase.playerInfoes.Find(playerOneIdName);
                if (player1 == null) { RebootMatch(); return; }
                PlayerForConnectedPlayer pl1 = new PlayerForConnectedPlayer();
                pl1.reWriteAccordingTo(player1);
                pl1.GainMatchXp(pl1GainedXp);
                /*
                player1.changePlayer(pl1.returnDataBaseVersion());
                dataBase.Entry(player1).State = EntityState.Modified;
                dataBase.SaveChanges();
                */

                mainMutex.ReleaseMutex();
                SendMassageToPlayers(MatchMassageType.Winnerisii, playerTwoIdName);
                situation = MatchSituation.EndedPlay;
                preSituation = PreMatchSituation.EndedPlay;

            }

        }

        private void SendMassageToPlayers(MatchMassageType type, string massage)
        {
            AddPlayerEvent(playerOneIdName, type, massage);
            AddPlayerEvent(playerTwoIdName, type, massage);
        }

        private void AddPlayerEvent(string Id, MatchMassageType massageType, string eventMassage)
        {
            eventMutex.WaitOne();
            if (Id == playerOneIdName)
            {
                playerOneEvents.EventTypes.Add(massageType);
                playerOneEvents.desitionBodys.Add(eventMassage);
                isUnreadEventForPlayerOne = true;
            }
            else
            {
                playerTwoEvents.EventTypes.Add(massageType);
                playerTwoEvents.desitionBodys.Add(eventMassage);
                isUnreadEventForPlayerTwo = true;
            }
            eventMutex.ReleaseMutex();

        }

        
        private void RebootMatch()
        {
            preSituation = PreMatchSituation.NonExistance;
            matchNumber = -1;
            WFShootStartTime = 0;
            MatchEvents defultEvent = new MatchEvents();
            isPlayerOneInMatch = false;
            isPlayerTwoInMatch = false;
            if (isFirstReboot)
            {
                playerOneEvents.desitionBodys = new List<string>();
                playerOneEvents.EventTypes = new List<MatchMassageType>();
                playerTwoEvents.desitionBodys = new List<string>();
                playerTwoEvents.EventTypes = new List<MatchMassageType>();

                defultEvent.EventTypes = new List<MatchMassageType>();
                defultEvent.desitionBodys = new List<string>();
                defultEvent.EventTypes.Add(MatchMassageType.NothingNew);
                defultEvent.desitionBodys.Add("");
                NothingNewEventString = new JavaScriptSerializer().Serialize(defultEvent);

                pl1GainedXp.xpVAl = new int[15];
                pl1GainedXp.AssingedIndex = new int[14];
                pl2GainedXp.xpVAl = new int[15];
                pl2GainedXp.AssingedIndex = new int[14];
            }
            else
            {
                playerOneEvents.desitionBodys.Clear();
                playerOneEvents.EventTypes.Clear();
                playerTwoEvents.desitionBodys.Clear();
                playerTwoEvents.EventTypes.Clear();
            }
            playerOneShoot = null;
            playerTwoShoot = null;
            playerOneGoalClaim = 0;
            playerTwoGoalClaim = 0;            
            for (int i = 0; i < pl1GainedXp.AssingedIndex.Length; i++)
            {
                pl1GainedXp.AssingedIndex[i] = -1;
                pl2GainedXp.AssingedIndex[i] = -1;
                pl1GainedXp.xpVAl[i] = 0;
                pl2GainedXp.xpVAl[i] = 0;
            }
            pl1GainedXp.xpVAl[14] = 0;
            pl1GainedXp.xpVAl[14] = 0;
            isFirstReboot = false;
            GatheredMoneyForWaiting = 0;
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

        private float TimeFromStart()
        {
            return (float)DateTime.Now.Subtract(startMatchTime).TotalSeconds;
        }




        #endregion


        #region private variables
        bool isPlayerOneInMatch;
        bool isPlayerTwoInMatch;
        //private MatchMassageType playerOneEventType;
        //private MatchMassageType playerTwoEventType;
        private MatchEvents playerOneEvents = new MatchEvents();
        private MatchEvents playerTwoEvents = new MatchEvents();
        //private string playerOneEvent;
        //private string playerTwoEvent;

        private string playerOneIdName;
        //private string IdNameplayerOneId;
        //private string IdNameplayerTwoId;
        private float PlayerOnePower;
        private int playerOneScore = 0;

        private int PlayerOneShootTime;
        private string playerTwoIdName;
        private float PlayerTwoPower;
        private int playerTwoScore = 0;
        private int PlayerTwoShootTime;
        private string league;
        private Property betedMoney;
        private int matchNumber = -1;
        private bool isPlayerOneTurn = true;
        private int currentTurn = 1;
        private DateTime CreationTime;
        private DateTime startMatchTime;

        private DateTime lastShootTime;
        private MatchSituation situation;
        private string playerOnePawnsPositions = null;
        private string playerTwoPawnsPositions = null;
        private string playerOneShoot = null;
        private string playerTwoShoot = null;

        private bool playerOneFinishConfirm = false;
        private bool playerTwoFinishConfirm = false;
        private int playerOneGoalClaim = 0;
        private int playerTwoGoalClaim = 0;
        private int goalForWin = 2;
        private PreMatchSituation preSituation = PreMatchSituation.NonExistance;
        private bool isUnreadEventForPlayerOne = false;
        private bool isUnreadEventForPlayerTwo = false;
        private bool isUnSolvedGoalClaim = false;
        private bool isUnSolvedStationeryPostion = false;
        private bool IsgoalHappenInTurn = false;
        private string ThisTurnScorerID = null;
        private int lastTurnfinishWithTimerUp = 0;
        private float playerOneLastEventReadTime = 0;
        private float playerTwoLastEventReadTime = 0;
        private float WFShootStartTime = 0;
        private float TimeofLastStationeryPosition;
        private int GatheredMoneyForWaiting;
        string NothingNewEventString;
        
        int pl1PawnShooterAssingedIndex=-1;
        int pl2PawnShooterAssingedIndex=-1;
        float startOfWFFirstAcceptance;
        GainedXp pl1GainedXp = new GainedXp();
        GainedXp pl2GainedXp = new GainedXp();
        private bool isFirstReboot = true;
        /*
        int[] pl1PawnsIndex = new int[14];
        int[] pl1PawnsGainedXp = new int[15];
        int[] pl2PawnsIndex = new int[14];
        int[] pl2PawnsGainedXp = new int[15];
        */
        #endregion

    }
}