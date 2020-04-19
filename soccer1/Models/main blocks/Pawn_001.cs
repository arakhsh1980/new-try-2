using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using soccer1.Models.main_blocks;
using System.ComponentModel.DataAnnotations;


namespace soccer1.Models.main_blocks
{

  

    [Serializable]
    public class Pawn
    {

        const short pCMX = 260; //partCodeMaxSize
        public BuildedRoboPart[] parts = new BuildedRoboPart[5];
        public short baseTypeIndex;
        public string ShowName;
        public int requiredXpForUpgrade;
        public int updrageFrom;
        public int playerAssinedIndex;
        public int translationCode;
        public long allPartCode;
        public PawnAbility mainAbility = new PawnAbility();
        [Range(0, 100)]
        public float RemaingEnergy;
        [Range(0, 100)]
        public float health;

        public void AddXp(int xpval)
        {
            if (requiredXpForUpgrade < xpval)
            {
                requiredXpForUpgrade = 0; return;
            }
            else
            {
                requiredXpForUpgrade -= xpval;
            }
        }



        public long ReturnRoboCode()
        {
            if (11 < playerAssinedIndex)
            {
               // Debug.Log("erooor . decoding will show wrong results");
            }
            long code = 0;
            //long code = 0;
            code += (long)requiredXpForUpgrade;
            code += (long)baseTypeIndex * 1000;
            code += (long)parts[0].ReturnCodeWithOutType() * 1000 * 100;
            code += (long)parts[1].ReturnCodeWithOutType() * 1000 * 100 * pCMX;
            code += (long)parts[2].ReturnCodeWithOutType() * 1000 * 100 * pCMX * pCMX;
            code += (long)parts[3].ReturnCodeWithOutType() * 1000 * 100 * pCMX * pCMX * pCMX;
            code += (long)parts[4].ReturnCodeWithOutType() * 1000 * 100 * pCMX * pCMX * pCMX * pCMX;
            code += (long)playerAssinedIndex * 1000 * 100 * pCMX * pCMX * pCMX * pCMX * pCMX ;
            return code;
        }

        public void SetRoboByCode(long cod)
        {
            allPartCode = cod;

            for (int i = 0; i < parts.Length; i++) { parts[i] = new BuildedRoboPart(); }
            long code = cod;
            //Debug.Log(" SetRoboByCode :" + code);
            //long code = cod;
            long factor = (long)1000 * pCMX * pCMX * pCMX * pCMX * pCMX * 100;
            playerAssinedIndex = (int)(code / factor);
            code = code % factor;
            factor = (long)1000 * pCMX * pCMX * pCMX * pCMX * 100;
            short shooterCode = (short)(code / factor);
            parts[4].SetPart(shooterCode, RoboPartType.shooter);
            code = code % factor;
            factor = (long)1000 * pCMX * pCMX * pCMX * 100;
            short shildCode = (short)(code / factor);
            parts[3].SetPart(shildCode, RoboPartType.shild);
            code = code % factor;
            factor = (long)1000 * pCMX * pCMX * 100;
            short EnginCode = (short)(code / factor);
            parts[2].SetPart(EnginCode, RoboPartType.engine);
            code = code % factor;
            factor = (long)1000 * pCMX * 100;
            short battryCode = (short)(code / factor);
            parts[1].SetPart(battryCode, RoboPartType.battry);
            code = code % factor;
            factor = (long)1000 * 100;
            short aimerCode = (short)(code / factor);
            parts[0].SetPart(aimerCode, RoboPartType.aimer);
            code = code % factor;
            factor = (long)1000;
            baseTypeIndex = (short)(code / factor);
            code = code % factor;
            requiredXpForUpgrade = (int)(code);
        }


        public void DeepCopy(Pawn otherPawn)
        {
            ShowName = otherPawn.ShowName;
            mainAbility = otherPawn.mainAbility;
            requiredXpForUpgrade = otherPawn.requiredXpForUpgrade;
            updrageFrom = otherPawn.updrageFrom;
            playerAssinedIndex = otherPawn.playerAssinedIndex;
            RemaingEnergy = otherPawn.RemaingEnergy;
        }

        public void clculateMainAbility()
        {
            // RoboBaseInfo baseInfo = AvalabelObj.ReturnRoboBaseInfo(baseTypeIndex);
            // mainAbility.aiming = baseInfo.mainAblity.aiming+ parts[]
        }


    }


    public class PawnOfPlayerData
    {
        public long[] parts = new long[5];
        public short baceTypeIndex;
        //public int pawnType;
        public int playerPawnIndex;
        public int requiredXpForNextLevel;
    }

    public class PawnForDataBase
    {
        [Key]
        public int IdNum { get; set; }
        public int index { get; set; }
        public string ShowName { get; set; }
        public string redForSale { get; set; }
        public string blueForSale { get; set; }
        public string ForMatch { get; set; }
        public string abilityShower { get; set; }
        public int level { get; set; }
        public int fan { get; set; }
        public int coin { get; set; }
        public int SoccerSpetial { get; set; }        
        public int shootPower { get; set; }        
        public int endorance { get; set; }        
        public int boddyMass { get; set; }        
        public int aiming { get; set; }
        public string spPower1 { get; set; }
        public int spPower1Level { get; set; }
        public int spPower2Level { get; set; }
        public int spPower3Level { get; set; }
        public string spPower2 { get; set; }
        public string spPower3 { get; set; }
    }

    public struct Property
{
        public void SetZiro()
        {
            level = 0;
            fan = 0;
            Alminum = 0;
            gold = 0;
        }

        public void DeepCopey(Property pp)
        {
            level = pp.level;
            fan = pp.fan;
            Alminum = pp.Alminum;
            gold = pp.gold;
        }



        public int level { get; set; }
        public int fan { get; set; }
        public int Alminum { get; set; }
        public int gold { get; set; }
    }


    public struct Price
    {
        public int alminum { get; set; }
        public int gold { get; set; }
    }

    public struct PawnAbility
{
    
    public float shootPower { get; set; }       
    public float endorance { get; set; }        
    public float boddyMass { get; set; }        
    public float aiming { get; set; }
        public float force { get; set; }
        public SpetialPower spPower1 { get; set; }
        public int spPower1Level { get; set; }
        public int spPower2Level { get; set; }
        public int spPower3Level { get; set; }
        public SpetialPower spPower2 { get; set; }
        public SpetialPower spPower3 { get; set; }
    }
public struct SpetialPower
{
       
    public string IdName { get; set; }
        public string ShowName { get; set; }
        //public PawnSpetialPowerType spPower;
        public string scribtion { get; set; }
        public string image { get; set; }
    }




   
}