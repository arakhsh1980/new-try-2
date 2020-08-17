using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using soccer1.Models.utilites;


namespace soccer1.Models.main_blocks
{

    public static class GamePreference
    {        
        
        public static float TimeOfEveryshootTurn = 15.0f;
        public static int TimeOfGroundExpiretion;
        public static float RateOfMoneyGatheringOnWaitForOther = 1.0f;
        public static float TimerForAcceptanceStartPoint = 10;
        public static float Effect_minCollisionToShow = 0.01f;
        public static int[] DamageEffectTreshholds = { 0, 20, 40, 60, 80, 100 };
        public static int[] bluePrintBuildAllowence = { 0, 1, 2, 2, 2, 1 };
        //public static float XpPriceAlminium = 500.0f;
        //public static float XpPriceGold = 5.0f;
        public static int[] requirXpForLevel = { 10, 50, 200, 600, 1500, 3000, 4000, 5000, 6000, 7000, 8000 };
        public static int[] transitionToOtherTypePrice = { 0, 500, 1000, 0, 2000, 3000, 0, 5000, 6000, 0, 8000 };

    }



    [Serializable]
    public struct GameInnreData
    {
        public string NameCode;
        public int TimeofExpieration;
        public string thisGroundCode;
        public float Effect_minCollisionToShow;
        public float TimerForAcceptanceStartPoint;
        public float RateOfMoneyGatheringOnWaitForOther;
        public string requirXpForLevel;
        public string transitionToOtherTypePrice;
        public string bluePrintBuildAllowence;
        public void SetBasePrefrenceAccordingToThis()
        {
            

            GamePreference.TimeOfGroundExpiretion = TimeofExpieration;

            string[] dualPart = thisGroundCode.Split('&');
            int[] arr = new int[dualPart.Length];
            for (int y = 0; y < dualPart.Length; y++) if (0 < dualPart[y].Length)
                {
                    arr[y] = int.Parse(dualPart[y]);
                }
            

            GamePreference.RateOfMoneyGatheringOnWaitForOther = RateOfMoneyGatheringOnWaitForOther;

            GamePreference.Effect_minCollisionToShow = Effect_minCollisionToShow;

            GamePreference.TimerForAcceptanceStartPoint = TimerForAcceptanceStartPoint;

            //GamePreference.requirXpForLevel =Convertion.StringToIntArray( requirXpForLevel);
            //GamePreference.transitionToOtherTypePrice = Convertion.StringToIntArray(transitionToOtherTypePrice);
            GamePreference.requirXpForLevel =Convertors.StringToIntArrayWithChar(requirXpForLevel, '|');
            GamePreference.transitionToOtherTypePrice = Convertors.StringToIntArrayWithChar(transitionToOtherTypePrice, '|');
            GamePreference.bluePrintBuildAllowence = Convertors.StringToIntArrayWithChar(bluePrintBuildAllowence, '|');

        }
        public void SetAccordingToPreference()
        {

            //Physics_collisionDamageScale = Mathf.FloorToInt((float)GamePreference.Physics_collisionDamageScale * 1000);
            Effect_minCollisionToShow = GamePreference.Effect_minCollisionToShow;
            TimerForAcceptanceStartPoint = GamePreference.TimerForAcceptanceStartPoint;

            //requirXpForLevel = Convertion.IntArrayToSrting(GamePreference.requirXpForLevel);
            //transitionToOtherTypePrice = Convertion.IntArrayToSrting(GamePreference.transitionToOtherTypePrice);       
            requirXpForLevel = Convertors.IntArrayToSrtingWithChar(GamePreference.requirXpForLevel, '|');
            transitionToOtherTypePrice = Convertors.IntArrayToSrtingWithChar(GamePreference.transitionToOtherTypePrice, '|');
            bluePrintBuildAllowence = Convertors.IntArrayToSrtingWithChar(GamePreference.bluePrintBuildAllowence, '|');
            TimeofExpieration = -1;
            thisGroundCode = "";
        }

    }






    [Serializable]
    public class RoboPart
    {
        public string NameCode;
        public int partType;
        public int lowGoldEffect;
        public int MediomGoldEffect;
        public int HighGoldEffect;
        public string discription;
        public int requiredAlminiom;
        public int requiredGold;
        public int requiredTropy;
        public int MinuteToBuild;
        public int BluePrintrequiredAlminiom;
        public int BluePrintrequiredGold;
        public int BluePrintrequiredTropy;
        public int BluePrintMinuteToBuild;
        public short MinBaselevel;
        public short numberOfBuildableParts;

        public Property returnProperty()
        {
            Property property = new Property();
            property.gold = requiredGold;
            property.tropy = requiredTropy;
            property.Alminum = requiredAlminiom;
            return property;
        }

        public Property returnBluPrintProperty()
        {
            Property property = new Property();
            property.gold = BluePrintrequiredGold;
            property.tropy = BluePrintrequiredTropy;
            property.Alminum = BluePrintrequiredAlminiom;
            return property;
        }
    }

    [Serializable]
    public class RoboBase
    {
        //public string Name ;
    public string NameCode;
    public int type;
    public short BaseID;
    public short level;
    public short[] upgradeToId;
    public int requiredXpToUpgrade;
    public float mainAblity_shooter;
    public float mainAblity_shild;// after eath shot new power would be power*(100-endorance)/100
    public float mainAblity_wieght;
    public float mainAblity_aiming;
    public float mainAblity_force;
    public float mainAblity_energyConsume;
    public int SpaceForAddOn;
    public PawnAbility mainAblity()
    {
        PawnAbility pawnAbility = new PawnAbility();
        pawnAbility.aiming = mainAblity_aiming;
        pawnAbility.energyConsume = mainAblity_energyConsume;
        pawnAbility.force = mainAblity_force;
        pawnAbility.shild = mainAblity_shild;
        pawnAbility.shooter = mainAblity_shooter;
        pawnAbility.wieght = mainAblity_wieght;
        return pawnAbility;
    }
    }


    [Serializable]
    public class Planet
    {
        public string NameCode;
        public string fieldDiscription;
        public int aimerMFV; ///minimumEffectValue = 30;
        public int shildMFV;
        public int battryMFV;
        public int shooterMFV;
        public int engineMFV;
        public int TotalTurnOfMatch;
        public float TimeOfEveryshootTurn;
        public int GoalSize;
        public int GoalSizeIncresePerTurn;
        public int shotPowerScaler;//شوت مهره ها چقد قدرت داره کل شوت های کل مهره ها 
        public int ballShootScaler;//شوتی که مهره به توپ میزنه 
        public int roboColissionEffectScaler;//ربات با چه سرعتی خراب میشه
        public int roboAimerScaler;
        public int Ballmass;
        public float BaseRoboSize;
        public float BallSize;
        public int ballAirDrag;//اصطکاک توپ با هوا
        public int PawnAirDrag;//مهره ها چه اصطکاکی با هوا دارند 
        public int ballLineDrag;//توپ به خط کنار بخوره چقد سرعتش کم میشه
        public int pawnLineDrag;//مهره ها به خط کنار بخورند چقد سرعتشون کم میشه
        public int ballCircleDrag;//توپ وقتی به مهره بخوره چقد سرعتش کم میشه
        public int pawnpawnDrag;//مهره ها به هم بخورن چقد سرعتشونکم میشه
        public int pawnballDrag;//مهره به توپ بخوره چقد سرعتش کم میشه
        public int StopTreshhold;
        public int IncreasedDrag;//از ی جا به بعد اصطکاک زیاد میشه تا مهره ها وایسن
        public int Physics_collisionDamageScale;//ربات با چه سرعتی خراب میشه
        public int ShootsOnItsTurns;
        public int ShootsOnOpponentTurns;
        public bool AllowdToHittBallOnItsTurns;
        public bool AllowdToHittBallOnOpponentsTurns;
       
    }



    [Serializable]
    public class Sponsor
    {
        public Sponsor()
        {

        }

        public string NameCode;
        public int goldPerWin;
        public int AlminumPerWin;
        public int MaxTickets;
        public int TimeBetweenTickets;
        public string prerequisite;
        public string prerequisiteCode;
        public int minAcceptableTropy;
        public int minAcceptableXp;
    }

    [Serializable]
    public class Formation
    {

        public int IdNum;
        public string NameCode;
        public int requiredAlminiom;
        public int requiredGold;
        public int requiredTropy;
        public string discription;
        public int BallPosition_x;
        public int BallPosition_y;
        public int[] pawnPositions_x;
        public int[] pawnPositions_y;

        public Property price()
        {
            Property property = new Property();
            property.gold = requiredGold;
            property.tropy = requiredTropy;
            property.Alminum = requiredAlminiom;
            return property;
        }

        public PawnStartPosition ballposition()
        {
            PawnStartPosition psp = new PawnStartPosition();
            psp.x = BallPosition_x;
            psp.y = BallPosition_y;
            return psp;
        }

        public PawnStartPosition[] positions()
        {
            PawnStartPosition[] poses = new PawnStartPosition[pawnPositions_x.Length];
            for (int i = 0; i < pawnPositions_x.Length; i++)
            {
                PawnStartPosition psp = new PawnStartPosition();
                psp.x = pawnPositions_x[i];
                psp.y = pawnPositions_y[i];
                poses[i] = psp;
            }
            return poses;
        }

    }


    [Serializable]
    public class Offer
    {

        [Key]
        public string IdName { get; set; }

        public int index { get; set; }
        public Property price { get; set; }
        public int realDollerPrice { get; set; }
        public int BuyedmoneyType { get; set; }
        public int BuyedmoneyAmount { get; set; }
    }


    public enum ClassDataType { roboBase, RoboPart, sponser, formation , planet, offer, GameInnreData }
    public class ClassData
    {
        [Key]
        public string nameCode { get; set; }
        public int type { get; set; }
        public long TimeOfLastUpdate { get; set; }
        public string innerData { get; set; }

        public string ClassCodeToString()
        {
            string sum = type.ToString() + "+" + nameCode + "+" + TimeOfLastUpdate.ToString() + "+" + innerData;
            return sum;
        }


        
    }
}