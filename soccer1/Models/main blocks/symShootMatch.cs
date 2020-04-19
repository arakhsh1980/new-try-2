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
using soccer1.Models.utilites;


namespace soccer1.Models.main_blocks
{
    public struct GainedFromMatch
    {
        public string WinnerId;
        public int[] AssingedIndex  ;
        public int[] xpVAl ;
        public Property gained;        

        
    }
    public struct GainedFromMatchForSerilize
    {
        public string WinnerId;
        public string AssingedIndex;
        public string xpVAl;
        public Property gained;


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
                pl1Substitution = jsonpart;
               // AddPlayerEvent(playerTwoIdName, MatchMassageType.Substituti, jsonpart);
            }
            if (PlayerId == playerTwoIdName)
            {
                pl2Substitution = jsonpart;
               // AddPlayerEvent(playerOneIdName, MatchMassageType.Substituti, jsonpart);
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
            Log.AddMatchLog(matchNumber, "ShootHappeded.shooter:" + playerIDName);
            if (TurnNumber != currentTurn) { Errors.AddSmallError("untiming shoot resived. out of turn"); return false; }
            if (situation != MatchSituation.WFShoot)
            {
                Errors.AddSmallError("ShootHappeded.untiming Stationery Positions resived");
                return false;
            }
            mainMutex.WaitOne();
            if (playerIDName == playerOneIdName && playerOneShoot== "NULL" )
            {
                playerOneShoot = shoot;
                //pl1PawnShooterAssingedIndex = PawnAssignIndex;
                AddXpToShooter(true, NominatedXperiance.simpleShootXp);
                
                
                Log.AddPlayerLog(playerOneIdName, " ShootHappeded resived");
                result = true;
            }
            if (playerIDName == playerTwoIdName && playerTwoShoot == "NULL" )
            {
                playerTwoShoot = shoot;
                //pl2PawnShooterAssingedIndex = PawnAssignIndex;
                AddXpToShooter(false, NominatedXperiance.simpleShootXp);                
                Log.AddPlayerLog(playerTwoIdName, " ShootHappeded resived");
                result = true;
            }
            if ( playerOneShoot != "NULL" && playerTwoShoot != "NULL")
            {
                eventMutex.WaitOne();
                situation = MatchSituation.WFStationeryPositions;
                lastShootTime = DateTime.Now;
                AddPlayerEvent(playerOneIdName, MatchMassageType.ActTisShot, playerOneShoot + "*" + playerTwoShoot+ "*"+pl1Substitution + "*" + pl2Substitution);
                AddPlayerEvent(playerTwoIdName, MatchMassageType.ActTisShot, playerOneShoot + "*" + playerTwoShoot + "*" + pl1Substitution + "*" + pl2Substitution);               
                playerOneShoot = "NULL";
                playerTwoShoot = "NULL";
                eventMutex.ReleaseMutex();
            }
           
            mainMutex.ReleaseMutex();
            return result;
        }

        public void GetStationeryPostion(string SenderId, int TurnNumber, string jsonpart)
        {
            Log.AddMatchLog(matchNumber, "GetStationeryPostion.SenderId:" + SenderId);
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
            if (playerOnePawnsPositions != "" && playerTwoPawnsPositions != "")
            {
                if (isBothPositionsSimilar(playerOnePawnsPositions, playerTwoPawnsPositions))
                {
                    situation = MatchSituation.WFShoot;

                    Log.AddMatchLog(matchNumber, " GetStationeryPostion.Stationery position Accepted");
                    playerOnePawnsPositions = "";
                    playerTwoPawnsPositions = "";
                    isUnSolvedStationeryPostion = false;
                    StartNextTurn();
                    //SendMassageToPlayers(MatchMassageType.shotIsDown, "");
                }
                else
                {
                    Log.AddMatchLog(matchNumber, "GetStationeryPostion. Error Eroor different result of shoots");
                    Errors.AddBigError("different result of shoots");
                    situation = MatchSituation.WFShoot;
                    int firstDifferent = 0;
                    while (playerOnePawnsPositions[firstDifferent] == playerTwoPawnsPositions[firstDifferent])
                    {
                        if (firstDifferent == playerOnePawnsPositions.Length - 1 || firstDifferent == playerTwoPawnsPositions.Length - 1)
                        {
                            SendMassageToPlayers(MatchMassageType.difrentRes, "different lenght");
                        }
                        else
                        {
                            firstDifferent++;
                        }

                    }
                    SendMassageToPlayers(MatchMassageType.difrentRes, firstDifferent.ToString());
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
            Log.AddMatchLog(matchNumber, "GoalReport.SenderId:" + SenderId);
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
                    Log.AddMatchLog(matchNumber, "GoalReport.playerOneScore:" + playerOneScore);
                    ThisTurnScorerID = playerOneIdName;
                    isPlayerOneTurn = false;
                    playerOnePawnsPositions = "";
                    playerTwoPawnsPositions = "";
                    isUnSolvedGoalClaim = false;

                }
                if (playerOneGoalClaim == -1)
                {
                    IsgoalHappenInTurn = true;
                    AddXpToShooter(false, NominatedXperiance.GoalXp);
                    AddXpToTeam(false, NominatedXperiance.GoalScorerTeamXp);
                    playerTwoScore++;
                    Log.AddMatchLog(matchNumber, "GoalReport.playerTwoScore:" + playerTwoScore);
                    ThisTurnScorerID = playerTwoIdName;
                    isPlayerOneTurn = true;
                    playerOnePawnsPositions = "";
                    playerTwoPawnsPositions = "";
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

        public string ReturnEvent(string playerIdName, int lastREsiverNumber, string request)
        {
            if (preSituation == PreMatchSituation.TwoPlayerInPlay) { 
                CheckIfPlayersInTheMatch();
            }
            if(preSituation != PreMatchSituation.WFSecondPlayerAtHome && preSituation != PreMatchSituation.WFSecondPlayer)
            {
               // IsPreSituationStillValid();
            }
            
            eventMutex.WaitOne();
            MatchEventsArray result = new MatchEventsArray();
            result.SetError();           

            if (playerIdName == playerOneIdName)
            {
                playerOneLastEventReadTime = TimeFromStart();
                if (PlayerOneeventCounter <= lastREsiverNumber || PlayerOneeventCounter<0)
                {
                    result.SetNotingNew();
                    if (request == PlayerRequestTypes.goingHomeAndWaiting.ToString()  )
                    {
                        if (preSituation != PreMatchSituation.WFSecondPlayer) {
                            result.RequestAnswer = "Error. preSituation != PreMatchSituation.WFSecondPlayer"; 
                        }
                        preSituation = PreMatchSituation.WFSecondPlayerAtHome;
                        result.Events[0].EventTypes = MatchMassageType.WatForOthr;                        
                    }
                    if(request == PlayerRequestTypes.IAcceptedToPlay.ToString())
                    {
                        Log.AddMatchLog(matchNumber, "PlayerOnePlayAccepted");
                        //GatheredMoneyForWaiting = GatheredMoney;
                        StartMatch();
                        isPlayerOneInMatch = true;
                    }
                }
                else
                {
                    result.Events = new MatchEvents[PlayerOneeventCounter - lastREsiverNumber];                    
                    for (int i = 0; i < result.Events.Length; i++)
                    {
                        result.Events[i] = playerOneEvents[i + lastREsiverNumber+1];
                    }                                        
                } 
            }
            else
            {
                if (playerIdName == playerTwoIdName)
                {
                    playerTwoLastEventReadTime = TimeFromStart();
                    if (PlayerTwoeventCounter <= lastREsiverNumber || PlayerTwoeventCounter<0)
                    {
                        result.SetNotingNew();
                        if (request == PlayerRequestTypes.goingHomeAndWaiting.ToString())
                        {
                            if (preSituation != PreMatchSituation.WFSecondPlayer)
                            {
                                result.RequestAnswer = "Error. preSituation != PreMatchSituation.WFSecondPlayer";
                            }
                            preSituation = PreMatchSituation.WFSecondPlayerAtHome;
                            result.RequestAnswer = true.ToString();
                        }
                    }
                    else
                    {
                         result.Events = new MatchEvents[PlayerTwoeventCounter - lastREsiverNumber ];
                        for (int i = 0; i < result.Events.Length; i++)
                        {
                            result.Events[i] = playerTwoEvents[i + lastREsiverNumber+1];
                        }                     
                    }
                }
                else
                {
                    result.Events[0].desitionBodys = "Error . player with that id Not finded";                    
                    Errors.AddBigError("symShootMatch.ReturnEvent. player id is not playerOneIdName or playerTwoEvents ");
                }
            }
            eventMutex.ReleaseMutex();
            if (!isPlayerOneInMatch) { isUnreadEventForPlayerOne = false; }
            if (!isPlayerTwoInMatch) { isUnreadEventForPlayerTwo = false; }
            if (!isUnreadEventForPlayerOne && !isUnreadEventForPlayerTwo && situation == MatchSituation.EndedPlay)
            {
                //SymShootMatchesList.DelletMatch(matchNumber);
                RebootMatch(); 
            }
            if (result.Events[0].EventTypes == MatchMassageType.NothingNew )
            {
                if(request == PlayerRequestTypes.goingHomeAndWaiting.ToString())
                {
                                   
                }
            }
            result.Request = request; 
            
            string uu = new JavaScriptSerializer().Serialize(result);
            return uu;
        }

       

        #endregion


        #region Resiving funcitons

        public void TimerIsUp(string PlayerId, int turn)
        {
            if(situation == MatchSituation.EndedPlay) { return ; }
            Log.AddMatchLog(matchNumber, "ItsMyTimeClaim.SenderId:" + PlayerId);
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
            Log.AddMatchLog(matchNumber, "PlayerOneNotAcceptedToPlay:" + playerTwoIdName);
            string oldplayerTwoIdName = playerTwoIdName;
            float oldPlayerTwoPower = PlayerTwoPower;
            int  oldmatchNumber = matchNumber;
            string  oldleague = league;
            int oldNumberOfTurn = matchTotalTurn;
            RebootMatch();
            InitiatMatchWithOnePlayer(oldplayerTwoIdName, oldPlayerTwoPower, oldmatchNumber, oldleague,"OldOne", oldNumberOfTurn);
            preSituation = PreMatchSituation.WFSecondPlayerAtHome;
            AddPlayerEvent(playerTwoIdName, MatchMassageType.WatForOthr, "");
            return true.ToString();
        }


        public int AddMoneyToPlayerForWaiting(string playerId)
        {
            if (playerOneIdName != playerId) { return 0; }
            
            UpdateGatheredMoneyForWaiting();
            if (GatheredMoneyForWaiting < betedMoney.Alminum *2) { return 0; }
            GatheredMoneyForWaiting = 0;
            StartOfGatheredMoneyTime = TimeFromStart();
            return (betedMoney.Alminum * 2);
        }


        


        public string PlayerOnePlayAccepted(string playerId, int GatheredMoney)
        {
            if (playerOneIdName != playerId) { return false.ToString(); }
            Log.AddMatchLog(matchNumber, "PlayerOnePlayAccepted" );
            GatheredMoneyForWaiting = GatheredMoney;
            StartMatch();
            isPlayerOneInMatch = true;
            return true.ToString();
        }

        
        public string GoingHomeAndWaiting(string playerId)
        {
            if (playerOneIdName != playerId) {
                if (12 < TimeOutOfPlayer(true))
                {
                    PlayerOneNotAcceptedToPlay(playerOneIdName);
                    preSituation = PreMatchSituation.WFSecondPlayerAtHome;
                    return true.ToString();
                }
                else
                {
                    return "Wait";
                }
            }
            if(preSituation != PreMatchSituation.WFSecondPlayer) { return "Error. preSituation != PreMatchSituation.WFSecondPlayer"; }
            preSituation = PreMatchSituation.WFSecondPlayerAtHome;
            return true.ToString();
        }

        public void InitiatMatchWithOnePlayer(string StarterId, float StarterPower, int Matchnum, string SelectedLeage, string groundCharSt, int numberOfTurns)
        {
            RebootMatch();
            Log.AddMatchLog(matchNumber, "InitiatMatchWithOnePlayer.PlayerID:" + StarterId);
            startMatchTime = DateTime.Now;
            mainMutex.WaitOne();
            playerOneLastEventReadTime = TimeFromStart();
            playerOneIdName = StarterId; 
            playerTwoIdName = "";
            PlayerOnePower = StarterPower;
            matchTotalTurn = numberOfTurns;
            // PlayerOneShootTime = ConnectedPlayersList.connectedPlayers[StarterId].ShootTime;
            playerOnePawnsPositions = "";
            playerTwoPawnsPositions = "";
            if(groundCharSt != "OldOne")
            {
                groundCharString = groundCharSt;
            }
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
            //Log.AddMatchLog(matchNumber, "startMatch" );
            mainMutex.WaitOne();
            
            PlayerForDatabase player1 = dataBase.playerInfoes.Find(playerOneIdName);
            PlayerForDatabase player2 = dataBase.playerInfoes.Find(playerTwoIdName);
            if (player1 == null || player2 == null) {
                RebootMatch(); return;
            }
            PlayerForConnectedPlayer pl1 = new PlayerForConnectedPlayer();
            PlayerForConnectedPlayer pl2 = new PlayerForConnectedPlayer();
            pl1.reWriteAccordingTo(player1);
            pl2.reWriteAccordingTo(player2);            
            Property p1Eprice = new Property();
            p1Eprice.DeepCopey(betedMoney);
            p1Eprice.Alminum -= GatheredMoneyForWaiting;
            if (p1Eprice.Alminum < 0) { p1Eprice.Alminum = 0; }
            pl1.SubtractSponserPorperty(p1Eprice.Alminum, p1Eprice.gold);
            pl2.SubtractSponserPorperty(betedMoney.Alminum, betedMoney.gold);
            pl1.playPrehibititionFinishTime =new Utilities().TimePointofNow() + 5;
            pl2.playPrehibititionFinishTime = new Utilities().TimePointofNow() + 5;
            //player1.changePlayer(pl1.returnDataBaseVersion());
            dataBase.Entry(player1).State = EntityState.Modified;
            //player2.changePlayer(pl2.returnDataBaseVersion());
            dataBase.Entry(player2).State = EntityState.Modified;
            dataBase.SaveChanges();
            playerOneShoot = "NULL";
            playerTwoShoot = "NULL";
            //PlayerTwoShootTime = ConnectedPlayersList.connectedPlayers[connectionId].ShootTime;

            //ConnectedPlayersList.SetPlayerMatch(playerOneId, matchNumber);
            //ConnectedPlayersList.SetPlayerMatch(playerTwoId, matchNumber);
            situation = MatchSituation.WFShoot;
            preSituation = PreMatchSituation.TwoPlayerInPlay;
            lastShootTime = DateTime.Now;
            currentTurn = 1;
            string betedMoneyStr = new JavaScriptSerializer().Serialize(betedMoney);
            //SendMassageToPlayers(MatchMassageType.SubBetdMon, betedMoneyStr);
           // MatchCharestristic thisMatchCharecture = new JavaScriptSerializer().Deserialize<MatchCharestristic>(Statistics.GamePrefernceString);
           // thisMatchCharecture.TotalTurnOfMatch = Statistics.totalTurnOfDefultMatch;
           // string matchPhysicCharecture = RandomChandedMatchChar(thisMatchCharecture,0);
            //matchTotalTurn = thisMatchCharecture.TotalTurnOfMatch;
            //SendMassageToPlayers(MatchMassageType.GoToMatchi, matchPhysicCharecture);

            TeamForConnectedPlayers pl1Team = pl1.ReturnYourTeam();
            TeamForSerialize oppl1team = new Convertors().TeamToTeamForSerialize(pl1Team);
            string pl1TeamSrting = new Convertors().TeamForSerializeToJson(oppl1team);
            TeamForConnectedPlayers pl2Team = pl2.ReturnYourTeam();
            TeamForSerialize oppl2team = new Convertors().TeamToTeamForSerialize(pl2Team);
            string pl2TeamSrting = new Convertors().TeamForSerializeToJson(oppl2team);

            AddPlayerEvent(playerOneIdName, MatchMassageType.GoToMatchi, pl1.Name + "^" + pl1.sponsorName + "^" + pl1TeamSrting+ "^" + pl2.Name + "^" + pl2.sponsorName + "^" + pl2TeamSrting);
            AddPlayerEvent(playerTwoIdName, MatchMassageType.GoToMatchi, pl1.Name + "^" + pl1.sponsorName + "^" + pl1TeamSrting + "^" + pl2.Name + "^" + pl2.sponsorName + "^" + pl2TeamSrting);
            Log.AddMatchLog(matchNumber, " match started. pl1: " + playerOneIdName.ToString() + " . pl2: " + playerTwoIdName.ToString());
            
            WFShootStartTime = TimeFromStart();
            mainMutex.ReleaseMutex();
        }

/*

        string RandomChandedMatchChar(MatchCharestristic thisMatchCharecture,int changes)
        {
            
            int totalNumberOfCards = 10;
            int numnum;
            var rand = new Random();
            int[] selected =new int[changes];
            bool[] DoneChanges = new bool[totalNumberOfCards];
            for (int i = 0; i < DoneChanges.Length; i++)
            {
                DoneChanges[i] = false;
            }
            for (int i = 0; i < changes; i++)
            {
                do{
                    numnum = rand.Next(totalNumberOfCards);
                } while (DoneChanges[numnum]);
                DoneChanges[numnum] = true;
                selected[i] = numnum;
                switch (numnum)
                {
                    case 0:
                        thisMatchCharecture.ballAirDrag = thisMatchCharecture.ballAirDrag*2;
                        break;
                    case 1:
                        thisMatchCharecture.Ballmass *=  2;
                        break;
                    case 2:
                        thisMatchCharecture.ballShootScaler *= 2;
                        break;
                    case 3:
                        thisMatchCharecture.Physics_collisionDamageScale *= 2;
                        break;
                    case 4:
                        thisMatchCharecture.shotPowerScaler =(int)Math.Floor(thisMatchCharecture.shotPowerScaler * 0.8f);
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:
                        break;
                    case 8:
                        break;
                    case 9:
                        break;
                    case 10:
                        break;
                    default:
                        break;
                }
            }
            string part1 = new JavaScriptSerializer().Serialize(thisMatchCharecture);
            string part2 = "";
            if (0 < changes)
            {
                part2 = selected[0].ToString();
                for (int i = 1; i < selected.Length; i++)
                {
                    part2 += "&" + selected[i].ToString();
                }
            }
            return part1+"*"+ part2;
        }
        */


        string RandomCards(int numberOfCards, int totalNumberOfCards)
        {
            string res = "";
            var rand = new Random();           
            int num = rand.Next(totalNumberOfCards + 1);
            res += num.ToString();
            for (int i = 1; i < numberOfCards; i++)
            {
                num = rand.Next(totalNumberOfCards + 1);
                res+="&"+ num.ToString();
            }
            return res;
        }
        string RandomCards2(int numberOfCards, int totalNumberOfCards)
        {
            int[] res = new int[numberOfCards];
            for (int i = 1; i < numberOfCards; i++)
            {
                res[0] = new Random().Next(totalNumberOfCards + 1);
            }
            
            return new Convertors().IntArrayToSrting(res); 
        }

        public void AddSecondPlayerStartImideatly(string secondPlayerID, float power)
        {
            Log.AddMatchLog(matchNumber, "symShootMatch.AddSecondPlayerStartImideatly.secondPlayerID:" + secondPlayerID);            
            playerTwoIdName = secondPlayerID;
            playerTwoLastEventReadTime = TimeFromStart();
            PlayerTwoPower = power;
            isPlayerTwoInMatch = true;
            StartMatch();
        }

        public void AddSecondPlayerAndWaitForFisrtRespond(string seconrPlayerID, float power)
        {
            Log.AddMatchLog(matchNumber, "AddSecondPlayerAndWaitForFisrtRespond.secondPlayerID:" + seconrPlayerID);
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
            Log.AddMatchLog(matchNumber, "playerLostOrCenceled:" + playerId);
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
                        //PlayerOneNotAcceptedToPlay(playerOneIdName);
                        RebootMatch();
                    }
                    if (playerId == playerTwoIdName)
                    {
                        if(playerOneIdName =="")
                        {
                            RebootMatch();
                        }
                        //PlayerOneNotAcceptedToPlay(playerOneIdName);
                        
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

        private float TimeOutOfPlayer(bool isPlayerOne)
        {
            if (isPlayerOne)
            {
                return TimeFromStart() - playerOneLastEventReadTime;
            }
            else
            {
                return TimeFromStart() - playerTwoLastEventReadTime;
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
        public void playerOneGroundExpired()
        {

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
        private void SendGameResultToPlayers()
        {
            //GainedFromMatchForSerilize pl1GainedFromMatchForS = new GainedFromMatchForSerilize();
            //GainedFromMatchForSerilize pl2GainedFromMatchForS = new GainedFromMatchForSerilize();
            //pl1GainedFromMatchForS.AssingedIndex = new Convertors().IntArrayToSrting(pl1GainedFromMatch.AssingedIndex);
            //pl2GainedFromMatchForS.AssingedIndex = new Convertors().IntArrayToSrting(pl2GainedFromMatch.AssingedIndex);
            //pl1GainedFromMatchForS.AssingedIndex = new Convertors().IntArrayToSrting(pl1GainedFromMatch.xpVAl);
            //pl2GainedFromMatchForS.AssingedIndex = new Convertors().IntArrayToSrting(pl2GainedFromMatch.xpVAl);
            //pl1GainedFromMatchForS.gained = pl1GainedFromMatch.gained;
            //pl1GainedFromMatchForS.WinnerId = pl1GainedFromMatch.WinnerId;
            //pl2GainedFromMatchForS.gained = pl1GainedFromMatch.gained;
            //pl2GainedFromMatchForS.WinnerId = pl1GainedFromMatch.WinnerId;
            string uu = new JavaScriptSerializer().Serialize(pl1GainedFromMatch);
            AddPlayerEvent(playerOneIdName, MatchMassageType.Winnerisii, uu);
            string oo = new JavaScriptSerializer().Serialize(pl2GainedFromMatch);
            AddPlayerEvent(playerTwoIdName, MatchMassageType.Winnerisii, oo);
        }

        

        private void AddXpToTeam(bool isplyerOne, int xpval)
        {
            if (isplyerOne)// is player on?
            {
                pl1GainedFromMatch.xpVAl[pl1GainedFromMatch.xpVAl.Length - 1] += xpval;
            }
            else
            {
                pl2GainedFromMatch.xpVAl[pl2GainedFromMatch.xpVAl.Length - 1] += xpval;
            }
        }


        private void AddXpToShooter(bool isplayerOne ,int xpval)
        {
            int placeInArray = -1;
            if(isplayerOne)// is player on?
            {
                if (pl1PawnShooterAssingedIndex < 0) { return; }
                
                
                for (int i =0; i< pl1GainedFromMatch.AssingedIndex.Length; i++) if (pl1GainedFromMatch.AssingedIndex[i] == pl1PawnShooterAssingedIndex)
                    {
                        placeInArray = i;
                    }
                if (placeInArray < 0)
                {
                    int firstNullPlace = -1;
                    for (int i = pl1GainedFromMatch.AssingedIndex.Length - 1; -1 < i; i--) if (pl1GainedFromMatch.AssingedIndex[i] == -1) { firstNullPlace = i; }
                    pl1GainedFromMatch.AssingedIndex[firstNullPlace] = pl1PawnShooterAssingedIndex;
                    pl1GainedFromMatch.xpVAl[firstNullPlace] = xpval;
                }
                else
                {
                    pl1GainedFromMatch.xpVAl[placeInArray] += xpval;
                }                
            }
            else
            {
                if (pl2PawnShooterAssingedIndex < 0) { return; }
                for (int i = 0; i < pl2GainedFromMatch.AssingedIndex.Length; i++) if (pl2GainedFromMatch.AssingedIndex[i] == pl2PawnShooterAssingedIndex)
                    {
                        placeInArray = i;
                    }
                if (placeInArray < 0)
                {
                    int firstNullPlace = -1;
                    for (int i = pl2GainedFromMatch.AssingedIndex.Length - 1; -1 < i; i--) if (pl2GainedFromMatch.AssingedIndex[i] == -1) { firstNullPlace = i; }
                    pl2GainedFromMatch.AssingedIndex[firstNullPlace] = pl2PawnShooterAssingedIndex;
                    pl2GainedFromMatch.xpVAl[firstNullPlace] = xpval;
                }
                else
                {
                    pl2GainedFromMatch.xpVAl[placeInArray] += xpval;
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
                    Log.AddMatchLog(matchNumber, "GoalReport.playerOneScore:" + playerOneScore);
                    ThisTurnScorerID = playerOneIdName;
                    isPlayerOneTurn = false;
                    playerOnePawnsPositions = "";
                    playerTwoPawnsPositions = "";
                    isUnSolvedGoalClaim = false;

                }
                if (playerOneGoalClaim == -1)
                {
                    IsgoalHappenInTurn = true;
                    playerTwoScore++;
                    Log.AddMatchLog(matchNumber, "GoalReport.playerTwoScore:" + playerTwoScore);
                    ThisTurnScorerID = playerTwoIdName;
                    isPlayerOneTurn = true;
                    playerOnePawnsPositions = "";
                    playerTwoPawnsPositions = "";
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
            pl1Substitution = "null";
            pl2Substitution = "null";
            if (isUnSolvedGoalClaim) { return; }
            currentTurn++;
            if (matchTotalTurn < currentTurn)
            {
                if(playerTwoScore < playerOneScore)
                {
                    PlayerWin(true);
                    return;
                }
                if (playerOneScore < playerTwoScore)
                {
                    PlayerWin(false);
                    return;
                }
                if (playerOneScore == playerTwoScore)
                {
                    MatchFinishedAtDraw();
                    return;
                }
            }
            playerOnePawnsPositions = "";
            playerTwoPawnsPositions = "";
            playerOneGoalClaim = 0;
            playerTwoGoalClaim = 0;
            isPlayerOneTurn = !isPlayerOneTurn;
            lastShootTime = DateTime.Now;
            situation = MatchSituation.WFShoot;
            
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
            mainMutex.WaitOne();
            PlayerForDatabase player1 = dataBase.playerInfoes.Find(playerOneIdName);
            PlayerForDatabase player2 = dataBase.playerInfoes.Find(playerTwoIdName);
            if (player1 == null || player2 == null)
            {
                mainMutex.ReleaseMutex();
                RebootMatch();
                return;
            }
            PlayerForConnectedPlayer pl1 = new PlayerForConnectedPlayer();
            PlayerForConnectedPlayer pl2 = new PlayerForConnectedPlayer();
            pl1.reWriteAccordingTo(player1);
            pl2.reWriteAccordingTo(player2);
            
            if (isPlayerOneWinner)
            {
                AddXpToTeam(true, NominatedXperiance.WinnerTeamXp);
                AddXpToTeam(false, NominatedXperiance.LosserTeamXp);
                Log.AddMatchLog(matchNumber, "PlyaerWin. player: " + playerOneIdName + " wined");
                pl1GainedFromMatch.WinnerId = playerOneIdName;
                pl2GainedFromMatch.WinnerId = playerOneIdName;
                //pl1.AddProperty(betedMoney);
                //pl1.AddProperty(betedMoney);
                Property winedMonry = new AssetManager().returnSponserWinPrise(pl1.sponsorName);
                pl1GainedFromMatch.gained.AddToThis(winedMonry);
                Property waitingMoney = new Property();
                waitingMoney.SetZiro();
                waitingMoney.Alminum += GatheredMoneyForWaiting;
                //pl1.AddProperty(waitingMoney);
                pl1GainedFromMatch.gained.AddToThis(waitingMoney); 
                
                if (pl1GainedFromMatch.gained.tropy < pl2GainedFromMatch.gained.tropy)
                {
                    pl1GainedFromMatch.gained.tropy += 1;
                }
                pl1GainedFromMatch.gained.tropy += 3;
            }
            else
            {
                AddXpToTeam(false, NominatedXperiance.WinnerTeamXp);
                AddXpToTeam(true, NominatedXperiance.LosserTeamXp);
                //ConnectedPlayersList.PlayerWined(playerTwoId, betedMoney);
                Log.AddMatchLog(matchNumber, "PlyaerWin. player: " + playerTwoIdName + " wined");

                //SendGameResultToPlayers();


                pl1GainedFromMatch.WinnerId = playerTwoIdName;
                pl2GainedFromMatch.WinnerId = playerTwoIdName;

                //pl2.AddProperty(betedMoney);
                //pl2.AddProperty(betedMoney);
                Property winedMonry = new AssetManager().returnSponserWinPrise(pl2.sponsorName);
                pl2GainedFromMatch.gained.AddToThis(winedMonry);
                if (pl2GainedFromMatch.gained.tropy < pl1GainedFromMatch.gained.tropy)
                {
                    pl2GainedFromMatch.gained.tropy += 1;
                }
                pl2GainedFromMatch.gained.tropy += 3;
            }
            situation = MatchSituation.EndedPlay;
            preSituation = PreMatchSituation.EndedPlay;
            pl1.GainMatchResult(pl1GainedFromMatch);
            pl2.GainMatchResult(pl2GainedFromMatch);
            pl1.SaveChanges();
            pl2.SaveChanges();
            SendGameResultToPlayers();
            mainMutex.ReleaseMutex();
        }

        private void MatchFinishedAtDraw()
        {
            if (preSituation == PreMatchSituation.EndedPlay) { return; }
            mainMutex.WaitOne();
            PlayerForDatabase player1 = dataBase.playerInfoes.Find(playerOneIdName);
            PlayerForDatabase player2 = dataBase.playerInfoes.Find(playerTwoIdName);
            if (player1 == null || player2 == null)
            {
                mainMutex.ReleaseMutex();
                RebootMatch();
                return;
            }
            PlayerForConnectedPlayer pl1 = new PlayerForConnectedPlayer();
            PlayerForConnectedPlayer pl2 = new PlayerForConnectedPlayer();
            pl1.reWriteAccordingTo(player1);
            pl2.reWriteAccordingTo(player2);
            pl1GainedFromMatch.WinnerId = "Draw";
            pl2GainedFromMatch.WinnerId = "Draw";
            
                AddXpToTeam(true, NominatedXperiance.DrawTeamXp);
                AddXpToTeam(false, NominatedXperiance.DrawTeamXp);
                Log.AddMatchLog(matchNumber, "Gamefinished At Draw.");


            //pl1.AddProperty(betedMoney);
            //pl1.AddProperty(betedMoney);
                Property winedMonry1 = new AssetManager().returnSponserWinPrise(pl1.sponsorName);
                Property winedMonry2 = new AssetManager().returnSponserWinPrise(pl2.sponsorName);
                winedMonry1.Alminum = winedMonry1.Alminum/2;
            winedMonry1.gold = winedMonry1.gold / 2;
            winedMonry2.Alminum = winedMonry2.Alminum / 2;
            winedMonry2.gold = winedMonry2.gold / 2;
            pl1GainedFromMatch.gained.AddToThis(winedMonry1);
                pl2GainedFromMatch.gained.AddToThis(winedMonry2);
                Property waitingMoney = new Property();
                waitingMoney.SetZiro();
                waitingMoney.Alminum += GatheredMoneyForWaiting;
                //pl1.AddProperty(waitingMoney);
                pl1GainedFromMatch.gained.AddToThis(waitingMoney);

                
                pl1GainedFromMatch.gained.tropy += 2;
            pl2GainedFromMatch.gained.tropy += 2;

            situation = MatchSituation.EndedPlay;
            preSituation = PreMatchSituation.EndedPlay;
            pl1.GainMatchResult(pl1GainedFromMatch);
            pl2.GainMatchResult(pl2GainedFromMatch);
            pl1.SaveChanges();
            pl2.SaveChanges();
            SendGameResultToPlayers();
            mainMutex.ReleaseMutex();
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
                PlayerOneeventCounter++;
                playerOneEvents[PlayerOneeventCounter].EventTypes=massageType;
                playerOneEvents[PlayerOneeventCounter].desitionBodys=eventMassage;
                playerOneEvents[PlayerOneeventCounter].EventNumber = PlayerOneeventCounter;


                isUnreadEventForPlayerOne = true;
                
            }
            else
            {
                PlayerTwoeventCounter++;
                playerTwoEvents[PlayerTwoeventCounter].EventTypes=massageType;
                playerTwoEvents[PlayerTwoeventCounter].desitionBodys=eventMassage;
                playerTwoEvents[PlayerTwoeventCounter].EventNumber = PlayerTwoeventCounter;
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
            playerOneIdName = "";
            playerOneIdName = "";
            PlayerOneeventCounter = -1;
            PlayerTwoeventCounter = -1;
            NothingNewMatchEvent = new MatchEventsArray();
            NothingNewMatchEvent.Events = new MatchEvents[1];
            NothingNewMatchEvent.Events[0].EventNumber = -1;
            NothingNewMatchEvent.Events[0].desitionBodys = "";
            NothingNewMatchEvent.Events[0].EventTypes = MatchMassageType.NothingNew;
            if (isFirstReboot)
            {
                //playerOneEvents.desitionBodys = new List<string>();
                //playerOneEvents.EventTypes = new List<MatchMassageType>();
                //playerTwoEvents.desitionBodys = new List<string>();
                //playerTwoEvents.EventTypes = new List<MatchMassageType>();

                //defultEvent.EventTypes = new List<MatchMassageType>();
                //defultEvent.desitionBodys = new List<string>();
                //defultEvent.EventTypes.Add(MatchMassageType.NothingNew);
                //defultEvent.desitionBodys.Add("");
                NothingNewEventString = new JavaScriptSerializer().Serialize(defultEvent);

                pl1GainedFromMatch.xpVAl = new int[15];
                pl1GainedFromMatch.AssingedIndex = new int[14];
                pl2GainedFromMatch.xpVAl = new int[15];
                pl2GainedFromMatch.AssingedIndex = new int[14];
            }
            else
            {
                //playerOneEvents.desitionBodys.Clear();
                //playerOneEvents.EventTypes.Clear();
                //playerTwoEvents.desitionBodys.Clear();
                //playerTwoEvents.EventTypes.Clear();
            }
            playerOneShoot = "NULL";
            playerTwoShoot = "NULL";
            playerOneGoalClaim = 0;
            playerTwoGoalClaim = 0;            
            for (int i = 0; i < pl1GainedFromMatch.AssingedIndex.Length; i++)
            {
                pl1GainedFromMatch.AssingedIndex[i] = -1;
                pl2GainedFromMatch.AssingedIndex[i] = -1;
                pl1GainedFromMatch.xpVAl[i] = 0;
                pl2GainedFromMatch.xpVAl[i] = 0;
            }
            pl1GainedFromMatch.xpVAl[14] = 0;
            pl1GainedFromMatch.xpVAl[14] = 0;
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

        private void UpdateGatheredMoneyForWaiting()
        {
            GatheredMoneyForWaiting =(int)Math.Ceiling( (TimeFromStart()- StartOfGatheredMoneyTime) * Statistics.MoneyForWaitingOverSecond);
        }


        #endregion


        #region private variables
        bool isPlayerOneInMatch;
        bool isPlayerTwoInMatch;
        string pl1Substitution = "null";
        string pl2Substitution = "null";
        //private MatchMassageType playerOneEventType;
        //private MatchMassageType playerTwoEventType;
        private MatchEvents[] playerOneEvents = new MatchEvents[50];
        private MatchEvents[] playerTwoEvents = new MatchEvents[50];
        //private string playerOneEvent;
        //private string playerTwoEvent;

        private string playerOneIdName;
        //private string IdNameplayerOneId;
        //private string IdNameplayerTwoId;
        private float PlayerOnePower;
        private int playerOneScore = 0;
        private int matchTotalTurn=8;
        private int PlayerOneShootTime;
        private string playerTwoIdName;
        private float PlayerTwoPower;
        private int playerTwoScore = 0;
        private int PlayerTwoShootTime;
        private int PlayerOneeventCounter = -1;
        private int PlayerTwoeventCounter = -1;
        private string league;
        private Property betedMoney;
        private int matchNumber = -1;
        private bool isPlayerOneTurn = true;
        private int currentTurn = 1;
        private DateTime CreationTime;
        private DateTime startMatchTime;
        private float StartOfGatheredMoneyTime;
        private DateTime lastShootTime;
        private MatchSituation situation;
        private string playerOnePawnsPositions = "";
        private string playerTwoPawnsPositions = "";
        private string playerOneShoot = "NULL";
        private string playerTwoShoot = "NULL";

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
        MatchEventsArray NothingNewMatchEvent;
        public string groundCharString= "";
        int pl1PawnShooterAssingedIndex=-1;
        int pl2PawnShooterAssingedIndex=-1;
        float startOfWFFirstAcceptance;
        GainedFromMatch pl1GainedFromMatch = new GainedFromMatch();
        GainedFromMatch pl2GainedFromMatch = new GainedFromMatch();
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