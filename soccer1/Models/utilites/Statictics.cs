using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace soccer1.Models
{
    public enum MatchSituation { WFShoot, WFStationeryPositions, WFGoalClaim };

    public enum PreMatchSituation { NonExistance, WithOnePlayer, WithTwoPlayer };

    public enum AssetType { Pawn, Elixir, Formation, none };

    public enum ServrMasage { NothingNew, WatForOthr, GoToMatchi, ActTisShot, Winnerisii, Disconcted, PlayerGoal, PlrTimeOut, shotIsDown }

    public static class Statistics
    {
        public const int ConnectedPlayersMaxNumber = 10000;
        public const int ActiveMatchesMaxNumber = 5000;
        public const float ConnectionTimeOut = 60f;
        public const float AllCleanMinIntervals = 0.9f;
        public const float MinServerCallIntervals = 0.05f;
        public const int MaxPawnOutOfTeam = 30;
        public const int MaxElixirOutOfTeam = 30;
        public const int PlayingPawns = 5;
        public const int pawnsInBenchMax = 4;
        public const int ElixirInBenchMax = 4;
        public const int UsableFormationsMax = 15;
        public const int StartingCoin = 1000;
        public const int StartingSS = 10;
        public const float maxAcceptedPowerDiference = 10;
        //public static Mutex LoadDataBaseMutex = new Mutex();
    }

    


}