using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using soccer1.Models.utilites;

namespace soccer1.Models.main_blocks
{
    [Serializable]
    public struct MatchCharestristic
    {

        public int TotalTurnOfMatch;
        public float TimeOfEveryshootTurn;
        public int GoalSize;
        public int GoalSizeIncresePerTurn;
        public int shotPowerScaler;
        public int ballShootScaler;
        public int roboColissionEffectScaler;
        public int roboAimerScaler;
        public float Ballmass;
        public float BaseRoboSize;
        public float BallSize;
        public int ballAirDrag;
        public int ballLineDrag;
        public int pawnLineDrag;
        public int ballCircleDrag;
        public int pawnpawnDrag;
        public int pawnballDrag;
        public int StopTreshhold;
        public int IncreasedDrag;
        public int Physics_collisionDamageScale;

        public int TimeofExpieration ;
        public string thisGroundCode;

        // other vars
        public float Effect_minCollisionToShow;
        public float TimerForAcceptanceStartPoint;
        public float RateOfMoneyGatheringOnWaitForOther;   
        public string requirXpForLevel;
        public string transitionToOtherTypePrice;

        public void RandomChandedMatchChar(int changes, int MunitedurablityTime)
        {
            

            int totalNumberOfCards = 10;
            int numnum;
            var rand = new Random();
            int[] selected = new int[changes];
            bool[] DoneChanges = new bool[totalNumberOfCards];
            for (int i = 0; i < DoneChanges.Length; i++)
            {
                DoneChanges[i] = false;
            }
            for (int i = 0; i < changes; i++)
            {
                do
                {
                    numnum = rand.Next(totalNumberOfCards);
                } while (DoneChanges[numnum]);
                DoneChanges[numnum] = true;
                selected[i] = numnum;
                switch (numnum)
                {
                    case 0:
                        ballAirDrag = ballAirDrag * 2;
                        break;
                    case 1:
                        Ballmass *= 2;
                        break;
                    case 2:
                        ballShootScaler *= 2;
                        break;
                    case 3:
                        Physics_collisionDamageScale *= 2;
                        break;
                    case 4:
                        shotPowerScaler = (int)Math.Floor(shotPowerScaler * 0.8f);
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
            thisGroundCode = "";
            if (0 < changes)
            {
                thisGroundCode = selected[0].ToString();
            }
            
            for (int i = 1; i < selected.Length; i++)
            {
                thisGroundCode += "&" + selected[i].ToString();
            }

            TimeofExpieration = new Utilities().TimePointofNow() + MunitedurablityTime;
        }
    }
}