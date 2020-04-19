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

    public enum MatchMassageType { Error, NothingNew, DoYouPlayy, WatForOthr, GoToMatchi, ActTisShot, Winnerisii, FinishDraw, Disconcted, PlayerGoal, PlrTimeOut, Substituti, ElixirUsea, ChangeTurn, SubBetdMon, pl2Cancled, gaindedXps, difrentRes }

    public struct MatchMassage
    {
        public MatchMassageType type;
        public string body;
    }

    public static class Statistics
    {
        public static string AdminCode = "1235813";
        public static string GamePrefernceString = "{*TimeOfEveryshootTurn*:15.0,*GoalSize*:3.0,*RateOfMoneyGatheringOnWaitForOther*:1.0,*shotPowerScaler*:0.009999999776482582,*TimerForAcceptanceStartPoint*:10.0,*Ballmass*:5.0,*BaseRoboSize*:1.2999999523162842,*requirXpForLevel*:[10,50,200,600,1500,3000,4000,5000,6000,7000,8000],*transitionToOtherTypePrice*:[0,500,1000,0,2000,3000,0,5000,6000,0,8000]}";
        //public static MatchCharestristic todayMatchGround;        
        public static string todayMatchGroundString;
        public static int totalTurnOfDefultMatch = 8;
        // public static string GameDataString = "{*Missions*:[{*endGoal*:*score a goal*,*image*:{*instanceID*:0},*code*:0,*preRequisitMisions*:[0,2,3],*rewardInGold*:10},{*endGoal*:*win a game*,*image*:{*instanceID*:0},*code*:1,*preRequisitMisions*:[],*rewardInGold*:10},{*endGoal*:*build a part*,*image*:{*instanceID*:0},*code*:2,*preRequisitMisions*:[],*rewardInGold*:10},{*endGoal*:*build a full golden part*,*image*:{*instanceID*:0},*code*:3,*preRequisitMisions*:[],*rewardInGold*:10}],*Formations*:[{*IdNum*:0,*showName*:*1-1-2-1*,*positions*:[{*x*:87,*y*:50},{*x*:137,*y*:50},{*x*:137,*y*:-50},{*x*:27,*y*:50},{*x*:87,*y*:-50}],*price*:{*level*:0,*fan*:0,*Alminum*:0,*gold*:0},*discription*:**,*ballposition*:{*x*:30,*y*:0}},{*IdNum*:1,*showName*:**,*positions*:[{*x*:137,*y*:-43},{*x*:137,*y*:-20},{*x*:137,*y*:20},{*x*:27,*y*:20},{*x*:27,*y*:-21}],*price*:{*level*:0,*fan*:0,*Alminum*:0,*gold*:0},*discription*:**,*ballposition*:{*x*:30,*y*:0}},{*IdNum*:2,*showName*:**,*positions*:[{*x*:27,*y*:-30},{*x*:27,*y*:30},{*x*:7,*y*:0},{*x*:87,*y*:-20},{*x*:137,*y*:20}],*price*:{*level*:0,*fan*:0,*Alminum*:0,*gold*:0},*discription*:**,*ballposition*:{*x*:30,*y*:0}}],*Sponsers*:[{*name*:*WarmMountain*,*goldPerMinute*:0,*AlminumPerMinute*:1,*goldMaxCap*:100,*AlminumMaxCap*:10000}]}";
        //public static string GameDataString = "{*Missions*:[{*endGoal*:*score a goal*,*image*:{*instanceID*:0},* code*:0,* preRequisitMisions*:[0,2,3],* rewardInGold*:10},{* endGoal*:* win a game*,* image*:{* instanceID*:0},* code*:1,* preRequisitMisions*:[],* rewardInGold*:10},{* endGoal*:* build a part*,* image*:{* instanceID*:0},* code*:2,* preRequisitMisions*:[],* rewardInGold*:10},{* endGoal*:* build a full golden part*,* image*:{* instanceID*:0},* code*:3,* preRequisitMisions*:[],* rewardInGold*:10}],* Formations*:[{*IdNum*:0,*showName*:*1-1-2-1*,*positions*:[{*x*:87,*y*:50},{* x*:137,* y*:50},{* x*:137,* y*:-50},{* x*:27,* y*:50},{* x*:87,* y*:-50}],* price*:{* level*:0,* fan*:0,* Alminum*:0,* gold*:0},* discription*:**,* ballposition*:{* x*:30,* y*:0}},{* IdNum*:1,* showName*:**,* positions*:[{*x*:137,*y*:-43},{* x*:137,* y*:-20},{* x*:137,* y*:20},{* x*:27,* y*:20},{* x*:27,* y*:-21}],* price*:{* level*:0,* fan*:0,* Alminum*:0,* gold*:0},* discription*:**,* ballposition*:{* x*:30,* y*:0}},{* IdNum*:2,* showName*:**,* positions*:[{*x*:27,*y*:-30},{* x*:27,* y*:30},{* x*:7,* y*:0},{* x*:87,* y*:-20},{* x*:137,* y*:20}],* price*:{* level*:0,* fan*:0,* Alminum*:0,* gold*:0},* discription*:**,* ballposition*:{* x*:30,* y*:0}}],* Sponsers*:[{*name*:*WarmMountain*,*goldPerMinute*:0,*AlminumPerMinute*:1,*goldMaxCap*:100,*AlminumMaxCap*:10000}]}";

        public static string GameDataString = " {*Missions*:[{*endGoal*:*score a goal*,*image*:{*instanceID*:0},* code*:0,* preRequisitMisions*:[0,2,3],* rewardInGold*:10},{* endGoal*:* win a game*,* image*:{* instanceID*:0},* code*:1,* preRequisitMisions*:[],* rewardInGold*:10},{* endGoal*:* build a part*,* image*:{* instanceID*:0},* code*:2,* preRequisitMisions*:[],* rewardInGold*:10},{* endGoal*:* build a full golden part*,* image*:{* instanceID*:0},* code*:3,* preRequisitMisions*:[],* rewardInGold*:10}],* Formations*:[{*IdNum*:0,*showName*:*1-1-2-1*,*positions*:[{*x*:87,*y*:50},{* x*:137,* y*:50},{* x*:137,* y*:-50},{* x*:27,* y*:50},{* x*:87,* y*:-50}],* price*:{* level*:0,* fan*:0,* Alminum*:0,* gold*:0},* discription*:**,* ballposition*:{* x*:30,* y*:0}},{* IdNum*:1,* showName*:**,* positions*:[{*x*:137,*y*:-43},{* x*:137,* y*:-20},{* x*:137,* y*:20},{* x*:27,* y*:20},{* x*:27,* y*:-21}],* price*:{* level*:0,* fan*:0,* Alminum*:0,* gold*:0},* discription*:**,* ballposition*:{* x*:30,* y*:0}},{* IdNum*:2,* showName*:**,* positions*:[{*x*:27,*y*:-30},{* x*:27,* y*:30},{* x*:7,* y*:0},{* x*:87,* y*:-20},{* x*:137,* y*:20}],* price*:{* level*:0,* fan*:0,* Alminum*:0,* gold*:0},* discription*:**,* ballposition*:{* x*:30,* y*:0}}],* Sponsers*:[{*name*:*WarmMountain*,*goldPerMinute*:0,*AlminumPerMinute*:1,*goldMaxCap*:100,*AlminumMaxCap*:10000,*prerequisite*:*none*},{* name*:* BlackBound*,* goldPerMinute*:0,* AlminumPerMinute*:1,* goldMaxCap*:100,* AlminumMaxCap*:10000,* prerequisite*:* have a 5 level 2 robo*},{* name*:* TheExpand*,* goldPerMinute*:0,* AlminumPerMinute*:1,* goldMaxCap*:100,* AlminumMaxCap*:10000,* prerequisite*:**}]}";
        public static string GameCodeVersion = "1.0.0";
        public static int GameDataVersion = 0;
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
        public const int StartingsponsorAlmimun = 1000;
        public const int StartingsponsorGold = 50;
        public const int StartingGold = 2000;
        public const int StartingSS = 10;
        public const float maxAcceptedPowerDiference = 1000;
        public const float timeForFisrtRespondOnWFFirstAcceptance = 10;
        public static string BasePrefrance ="";
        public static int[] transitionToOtherTypePrice = { 0, 500, 1000, 0, 2000, 3000, 0, 5000, 6000, 0, 8000 };
        public static float MoneyForWaitingOverSecond = 1.0f;
        //public static Mutex LoadDataBaseMutex = new Mutex();
    }



    public static class NominatedXperiance
    {
        public const int simpleShootXp = 1;
        public const int GoalXp = 6;
        public const int GoalScorerTeamXp = 2;
        public const int WinnerTeamXp = 3;
        public const int LosserTeamXp = 1;
        public const int DrawTeamXp = 2;
    }

    public class LeaugeManager
    {
        public Property LeaugEnterencePice(string LeagName)
        {
            Property BettedMoney = new Property();
            BettedMoney.Alminum = 0;
            BettedMoney.fan = 0;
            BettedMoney.tropy = 0;
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