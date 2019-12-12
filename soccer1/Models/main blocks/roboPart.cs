using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace soccer1.Models.main_blocks
{


    [Flags] public enum RoboPartType {  aimer, shild, battry, shooter, engine }
   // public enum RoboPartEffectType { aimer, shild, battry, shooter, engine }
    public enum RoboPartGoldLevel { low, mediom, high }
    public class RoboPart
    {
        [Key]
        public string key { get; set; }
        public RoboPartType partType { get; set; }
        public short IdNum { get; set; }
       // public RoboPartEffectType effectType { get; set; }
        public int lowGoldEffect { get; set; }
        public int MediomGoldEffect { get; set; }
        public int HighGoldEffect { get; set; }
       // public Property price { get; set; }
        public int GoldValue { get; set; }
        public int AlmimunValue { get; set; }

        public int minuetToBuild { get; set; }
    }

    public class RoboBase
    {
        [Key]
        public string key { get; set; }
        public short IdNum { get; set; }
       // public RoboPartEffectType effectType { get; set; }
        public short level { get; set; }
        public short upgradeToId1 { get; set; }
        public short upgradeToId2 { get; set; }
        public short upgradeToId3 { get; set; }
        public int requiredXpToUpgrade { get; set; }
        public PawnAbility mainAbility { get; set; }
    }

    public class BuildedRoboPart
    {
        public BuildedRoboPart(int roboCode)
        {
            short Partsode = (short)(roboCode % 300);
            int partTypeint = (int)Math.Floor(roboCode / 300.0);
            switch (partTypeint)
            {
                case 0:
                    partType = RoboPartType.aimer;
                    break;
                case 1:
                    partType = RoboPartType.shild;
                    break;
                case 2:
                    partType = RoboPartType.battry;
                    break;
                case 3:
                    partType = RoboPartType.shooter;
                    break;
                case 4:
                    partType = RoboPartType.engine;
                    break;
            }
            SetPart(Partsode, partType);
        }
        public BuildedRoboPart()
        {

        }
        public RoboPartType partType;
        public short PartID;
       // public RoboPartEffectType effectType;
        public int effectValue;
        //public float weigh;
        public Price ScrapPrice;
        public RoboPartGoldLevel goldLevel;
        public RoboPart sourcePart;

        public int ReturnCodeWithType()
        {
            short CodeWithOutType = ReturnCodeWithOutType();
            return (int)partType * 300 + CodeWithOutType;
        }

        public short ReturnCodeWithOutType()
        {
            short goldlevelcode = 0;
            switch (goldLevel)
            {
                case RoboPartGoldLevel.low:
                    goldlevelcode = 0;
                    break;
                case RoboPartGoldLevel.mediom:
                    goldlevelcode = 80;
                    break;
                case RoboPartGoldLevel.high:
                    goldlevelcode = 160;
                    break;
            }
            return (short)(goldlevelcode + PartID);
        }


        public void SetPart(short code, RoboPartType SetedpartType )
        {
            if (code == 0) { return; }
            partType = SetedpartType;
            PartID = (short)(code % 80);
            int goldlevelcode = (int)Math.Floor(code / 80.0f);
            switch (goldlevelcode)
            {
                case 0:
                    goldLevel = RoboPartGoldLevel.low;
                    break;
                case 1:
                    goldLevel = RoboPartGoldLevel.mediom;
                    break;
                case 2:
                    goldLevel = RoboPartGoldLevel.high;
                    break;
            }
            sourcePart = new AssetManager().ReturnRoboPart(PartID ,(short)partType );
            //sourcePart = AvalabelObj.ReturnRoboPartInfo(partType, PartID);

            switch (goldLevel)
            {
                case RoboPartGoldLevel.high:
                    effectValue = sourcePart.HighGoldEffect;
                    break;
                case RoboPartGoldLevel.mediom:
                    effectValue = sourcePart.MediomGoldEffect;
                    break;
                case RoboPartGoldLevel.low:
                    effectValue = sourcePart.lowGoldEffect;
                    break;
                default:
                    effectValue = sourcePart.lowGoldEffect;
                    break;

            }
        }



    }
}