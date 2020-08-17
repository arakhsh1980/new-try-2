using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace soccer1.Models.main_blocks
{


    
    
    // public enum RoboPartEffectType { aimer, shild, battry, shooter, engine }
    public enum RoboPartGoldLevel { low, mediom, high }

    public class BuildedRoboPart
    {
       
        public BuildedRoboPart()
        {

        }
        public RoboPartType partType;
        //public short PartID;
       // public RoboPartEffectType effectType;
        public int effectValue;
        //public float weigh;
        public Price ScrapPrice;
        public RoboPartGoldLevel goldLevel;
        //public RoboPart sourcePart;

        public int ReturnCode()
        {
            short goldlevelcode = 0;
            switch (goldLevel)
            {
                case RoboPartGoldLevel.low:
                    goldlevelcode = 0;
                    break;
                case RoboPartGoldLevel.mediom:
                    goldlevelcode = 1;
                    break;
                case RoboPartGoldLevel.high:
                    goldlevelcode = 2;
                    break;
            }
            return (int)partType * 3 + goldlevelcode;
        }

      


        public void SetPartByCode(int code  )
        {
            if (code == 0) { return; }
            short factor = 3;
            partType = (RoboPartType)(code / factor);
            code = code % factor;
            switch (code)
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
        }



    }
}