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

        public int Fan;

        public string Name;

        public int Money;

        public int CoonId;

        public float PowerLevel;

        public int level;

        public int SoccerSpetial;

        public int[] OutOfTeamPawns;

        public int[] OutOfTeamPawnsRequiredXp;

        public int[] OutOfTeamElixirs;

        public TeamForSerialize Team;

        public int[] PlayingPawnsRequiredXp;

        public int[] PawnsinBenchRequiredXp;
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