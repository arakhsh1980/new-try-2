using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace soccer1.Models.main_blocks
{
    [Serializable]
    public struct PlayerForSerial
    {

        public string id;

        public int Gold;

        public string Name;

        public int Alminum;

        public float numberOfTicket;
        public int lastTimeUpdate;

        public string sponsorName;

        public int CoonId;

        public float totalXp;

        public int totaltropy;

        //public int SoccerSpetial;

        public long[] OutOfTeamPawns;

        public int[] OutOfTeamPawnsRequiredXp;

        public int[] OutOfTeamElixirs;

        public int[] UsebaleFormations;

        public int[] UnAttachedPart;

        public TeamForSerialize Team;

        public int[] PlayingPawnsRequiredXp;

        public int[] PawnsinBenchRequiredXp;

        public short[] DoneMissions;

        public short[] remaningPakages;

        public short[] buyedBluePrints;

        public long TimeOfXpPakageExpiartion;

        public long playPrehibititionFinishTime;

        public long[] buildOrders;
        public bool IsTHisAHostPlayer;

        
        public int LastfilledGoalMemory;
    }

    /*
    [Serializable]
    public struct TeamForSerialize
    {
        public int pawnCounter;
        public PawnForSerialize goalKeeper;
        public List<PawnForSerialize> pawns;
    }
    */


    [Serializable]
    public struct PawnForSerialize
    {
        public string IdName;
        public string ShowName;
        public string redForSale;
        public string blueForSale;
        public string ForMatch;
        public string abilityShower;
        public Property price;
        public PawnAbility mainAbility;

        
    }

    [Serializable]
    public struct MatchForSerialize
    {
        public int matchId; 
        public float MPower;
        public string type;
        public int pawnNum;
        public int OwnerPId;
        public float dPosX;
        public float dPosY;
    }
}