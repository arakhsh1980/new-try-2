using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Web.Mvc;
using soccer1.Models;
using soccer1.Models.utilites;
using soccer1.Models.main_blocks;
using soccer1.Models.DataBase;

namespace soccer1.Models
{
    public enum MatchSituation { WFFirstAcceptance, WFShoot, WFStationeryPositions,   EndedPlay };

    public enum PreMatchSituation { NonExistance, WFSecondPlayer, WFSecondPlayerAtHome, TwoPlayerInPlay, WFFirstAcceptance,  EndedPlay };

    public enum AssetType { Pawn, Elixir, Formation, none };

    public enum MatchMassageType { Error, NothingNew, DoYouPlayy, WatForOthr, GoToMatchi, ActTisShot, Winnerisii, Disconcted, PlayerGoal, PlrTimeOut, Substituti, ElixirUsea, ChangeTurn, SubBetdMon, pl2Cancled, gaindedXps }

    public struct MatchMassage
    {
        public MatchMassageType type;
        public string body;
    }

    public static class Statistics
    {
        public static DateTime BaseStartTime = new DateTime(2019, 1, 1, 1, 1, 1);
        public const float MaxMatchTimeInSeconds=3000;
        public const float AcceptedTimeofStationeryPositionDifference = 50;
        public const float AcceptedWFShootTime = 40;
        public const int ConnectedPlayersMaxNumber = 10000;
        public const int ActiveMatchesMaxNumber = 5000;
        public const float ConnectionTimeOut = 22f;
        public const float AllCleanMinIntervals = 0.9f;
        public const float MinServerCallIntervals = 0.05f;
        public const int MaxPawnOutOfTeam = 30;
        public const int MaxElixirOutOfTeam = 30;
        public const int PlayingPawns = 5;
        public const int pawnsInBenchMax = 4;
        public const int ElixirInBenchMax = 4;
        public const int UsableFormationsMax = 15;
        public const int StartingAlminum = 2000;
        public const int StartingGold = 2000;
        public const int StartingSS = 10;
        public const float maxAcceptedPowerDiference = 10;
        public const float timeForFisrtRespondOnWFFirstAcceptance = 10;
        public static string BasePrefrance ="";
       
    //public static Mutex LoadDataBaseMutex = new Mutex();
}

    public static class NominatedXperiance
    {
        public const int simpleShootXp = 1;
        public const int GoalXp = 6;
        public const int GoalScorerTeamXp = 2;
        public const int WinnerTeamXp = 2;
        public const int LosserTeamXp = 1;
    }

    public class LeaugeManager
    {
        public Property LeaugEnterencePice(string LeagName)
        {
            Property BettedMoney = new Property();
            BettedMoney.Alminum = 0;
            BettedMoney.fan = 0;
            BettedMoney.level = 0;
            BettedMoney.gold = 0;
            switch (LeagName)
            {
                case "Silver":
                    BettedMoney.Alminum = 200;
                    break;
                default:
                    break;

            }
            return BettedMoney;
        }
    }
}