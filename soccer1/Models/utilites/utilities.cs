using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models.main_blocks;
using soccer1.Models.DataBase;
using soccer1.Models.utilites;

namespace soccer1.Models.utilites
{

    public class Utilities
    {
        public int TimePointofNow()
        {
            //int currentMinute = DateTimeOffset.UtcNow.Subtract(Statistics.BaseStartTime);
            // DateTimeOffset finishOrderDateTime1 = DateTimeOffset.UtcNow;

            // finishOrderDateTime1.AddSeconds(minuteFromNow);
            // TimeSpan remaningTime =   finishOrderDateTime1.Subtract(Statistics.BaseStartTime);
            TimeSpan TimeFromStart = DateTime.UtcNow.Subtract(Statistics.BaseStartTime);
            return (int)TimeFromStart.TotalMinutes ;
        }

       

        public int SecondsRemaindToTimePoint(int timePoint)
        {

            DateTimeOffset finishOrderDateTime1 = DateTimeOffset.UtcNow;
            TimeSpan remaningTime = finishOrderDateTime1.Subtract(Statistics.BaseStartTime);
           int nowTimePoint = (int)remaningTime.TotalSeconds;
            if(timePoint <= nowTimePoint)
            {
                return 0;
            }
            else
            {
                return timePoint - nowTimePoint;
            }
        }

        public long RoboPartBuildOrderToCode( short partID, short partType,short goldLevel)
        {
            long orderCode = goldLevel;
            orderCode += partType * 3;
            orderCode += partID * 3*10;
            int finishTimePoint = TimePointofNow()+new AssetManager().ReturnRoboPartTimeToBuild(partID, partType);
            orderCode += (long)finishTimePoint * 3 * 10 * 300;
            return orderCode;            
        }

        
        public long CodeToRoboPartBuildOrder(long buildOrderCode)
        {
            long factor = 3 * 10 * 300 ;
            long code = buildOrderCode;
            int finishTimePoint = (int)(code / factor);
            code = code % factor;
            factor = 3 * 10 ;
            int partID = (int)(code / factor);
            code = code % factor;
            factor = 3 ;
            int partType = (int)(code / factor);
            code = code % factor;
            factor = 1;
            int goldLevel = (int)(code / factor);
            code = code % factor;


            return code;
        }
        public long RoboPartBuildOrderfinishTime(short partID, short partType)
        {
           
            DateTime finishOrderDateTime = DateTime.Now.AddSeconds(new AssetManager().ReturnRoboPartTimeToBuild(partID, partType));
            DateTimeOffset finishOrderDateTime1 = DateTimeOffset.UtcNow;
            finishOrderDateTime1.AddSeconds(new AssetManager().ReturnRoboPartTimeToBuild(partID, partType));
            finishOrderDateTime1.ToUnixTimeSeconds();
            return finishOrderDateTime.ToBinary();
        }
        public int ReturnRemaningSecondsToTime(long finishTime)
        {
            DateTimeOffset finishOrderDateTime1 = DateTimeOffset.FromUnixTimeSeconds(finishTime);
            if (finishOrderDateTime1.CompareTo(DateTimeOffset.UtcNow) <= 0)
            {
                return 0;
            }
            TimeSpan remainigTime = finishOrderDateTime1.Subtract(DateTimeOffset.UtcNow);
            int remainigSeconds =  (int)Math.Floor(remainigTime.TotalSeconds);
            return remainigSeconds;
        }

        public bool CheckIfFirstPropertyIsBigger(Property property1, Property property2)
        {
            bool check = true;
            if (property1.Alminum < property2.Alminum ) { check = false; }
            if (property1.fan < property2.fan) { check = false; }
            if (property1.tropy < property2.tropy) { check = false; }
            if (property1.gold < property2.gold) { check = false; }
             
            return check;
        }

        public PlayerForDatabase ReturnDefultPlayer()
        {
            PlayerForDatabase player = new PlayerForDatabase();            
            TeamForConnectedPlayers team =returnDefultTeam();
            long[] pawnOutOfTeam = new long[2];
            pawnOutOfTeam[0] = returnDefultPlayerCode(9, 10);
            pawnOutOfTeam[1] = returnDefultPlayerCode(10, 10);

            List<int> elixirOutOfTeam = new List<int>();
            List<int> unattachedParts = new List<int>();
            //unattachedParts.Add(981);
            //unattachedParts.Add(1201);
            //unattachedParts.Add(382);
            //unattachedParts.Add(462);
            player.StartFomation = team.StartFomation;
            player.playPrehibititionFinishTime = TimePointofNow();
            player.AttackFormation = team.AttackFormation;
            player.DefienceForation = team.DefienceForation;
            player.ElixirInBench = new Convertors().IntArrayToSrting(team.ElixirInBench);
            player.gold = Statistics.StartingGold;
            player.level = 1;
            player.Almimun = Statistics.StartingAlminum;
            player.sponsorAlmimun = Statistics.StartingsponsorAlmimun;
            player.sponsorGold = Statistics.StartingsponsorGold;
            player.sponsorName = "WarmMountain";
            player.lastTimeSponsorAddDone = TimePointofNow();
            player.Name = "Defult";
            player.otherElixirs = new Convertors().IntArrayToSrting(new Convertors().listIntToIntArray(elixirOutOfTeam));
            player.UnAtachedParts = new Convertors().IntArrayToSrting(new Convertors().listIntToIntArray(unattachedParts));
            player.outOfTeamPawns = new Convertors().LongIntArrayToSrting(pawnOutOfTeam);
            player.pawnsInBench = new Convertors().LongIntArrayToSrting(team.pawnsInBench);
            player.PlayeingPawns = new Convertors().LongIntArrayToSrting(team.PlayeingPawns);
            player.PowerLevel = 1;
            short[] remaningPK = new short[11];
            player.remaingPakages = new Convertors().ShortArrayToSrting(remaningPK);
            player.timeOfXpPakageExpiration = 0;
            List<short> emptyShortList = new List<short>();
            player.DoneMissions = new Convertors().ShortListToSrting(emptyShortList);

            //player.gold = Statistics.StartingSS;
            int[] usableFormations = new int[3];
            usableFormations[0] = player.StartFomation;
            usableFormations[1] = new AssetManager().ReturnAssetIndex(AssetType.Formation, 1); ;
            usableFormations[2] = new AssetManager().ReturnAssetIndex(AssetType.Formation, 2); ;
            player.UsableFormations = new Convertors().IntArrayToSrting(usableFormations);
            long[] buildOrders = new long[4];
            buildOrders[0] = 0; buildOrders[1] = 0; buildOrders[2] = -1; buildOrders[3] = -1;
            player.buildOrders = new Convertors().LongIntArrayToSrting(buildOrders);            
            return player;
        }

        long returnDefultPlayerCode(int playerAssindedNum, int requiredXpForUpdrade)
        {
            PawnOfPlayerData pp = new PawnOfPlayerData();
            for(int i=0; i<pp.parts.Length; i++) { pp.parts[i] = 0; }
            pp.baceTypeIndex = 1;
            pp.playerPawnIndex = playerAssindedNum;
            pp.requiredXpForNextLevel = requiredXpForUpdrade;
            long newPawnCode = new Convertors().PawnOfPlayerDataToPawnCode(pp);
            return newPawnCode;
        }
        long ReturnPlayerCode(int playerAssindedNum, int requiredXpForUpdrade, int[] parts, short baceTypeIndexx)
        {
            Pawn newpawn = new Pawn();
            newpawn.baseTypeIndex = baceTypeIndexx;
            newpawn.playerAssinedIndex = playerAssindedNum;
            newpawn.requiredXpForUpgrade = requiredXpForUpdrade;
            for (int i = 0; i < newpawn.parts.Length; i++)
            {
                newpawn.parts[i] = new BuildedRoboPart(0);
                if(i< parts.Length)
                {
                    newpawn.parts[i].PartID = (short)parts[i];
                    newpawn.parts[i].goldLevel = 0;
                }
            }
                //newpawn.parts = new BuildedRoboPart[5];
                //for (int i = 0; i < newpawn.parts.Length; i++) {

                //    if (i < parts.Length)
                //    {
                //        newpawn.parts[i] = new BuildedRoboPart(parts[i]);
                //    }
                //    else
                //    {
                //        newpawn.parts[i] = new BuildedRoboPart();
                //        newpawn.parts[i].PartID = -1;
                //    }
                //} 
            //newpawn.parts[1] = new BuildedRoboPart();
            //newpawn.parts[1].goldLevel = 0;
            //newpawn.parts[1].PartID = 1;
            //newpawn.parts[3] = new BuildedRoboPart();
            //newpawn.parts[3].goldLevel = 0;
            //newpawn.parts[3].PartID = 1;
            return newpawn.ReturnRoboCode();

            //PawnOfPlayerData pp = new PawnOfPlayerData();
            //for (int i = 0; i < pp.parts.Length; i++) if (i < parts.Length)
            //    {
            //        pp.parts[i] = parts[i];
            //    }else
            //        { pp.parts[i] = 0; }
            //newpawn.baceTypeIndex = baceTypeIndexx;
            //newpawn.playerPawnIndex = playerAssindedNum;
            //newpawn.requiredXpForNextLevel = requiredXpForUpdrade;
            //long newPawnCode = new Convertors().PawnOfPlayerDataToPawnCode(pp);
            //return newPawnCode;
        }

        public TeamForConnectedPlayers returnDefultTeam()
        {
            TeamForConnectedPlayers team = new TeamForConnectedPlayers();
            //int defultPawnIndex = new AssetManager().ReturnAssetIndex(AssetType.Pawn ,0); 
            
            int defultElixirIndex = new AssetManager().ReturnAssetIndex(AssetType.Elixir, 0);            
            int defultFormationIndex = new AssetManager().ReturnAssetIndex(AssetType.Formation, 0);            
            team.StartFomation = defultFormationIndex;
            team.AttackFormation = defultFormationIndex;
            team.DefienceForation = defultFormationIndex;
            for (int i = 0; i < team.PlayeingPawns.Length; i++) { team.PlayeingPawns[i] = returnDefultPlayerCode(i,10); }
            for (int i = 0; i < team.pawnsInBench.Length; i++) { team.pawnsInBench[i] = returnDefultPlayerCode(i+5, 10); }
           
            //team.PlayeingPawns[0] = ReturnPlayerCode(0, 50, new int[] { 1,0,0,0,0}, 11);
            team.PlayeingPawns[1] = ReturnPlayerCode(1, 50, new int[] { 0, 0, 1, 0,0 }, 21);
            team.PlayeingPawns[2] = ReturnPlayerCode(2, 50, new int[] { 0, 0, 0, 1,0 }, 31);
            team.PlayeingPawns[3] = ReturnPlayerCode(3, 50, new int[] { 0, 0, 0, 0,1 }, 11);
            team.PlayeingPawns[4] = ReturnPlayerCode(4, 10, new int[] { 1, 0, 0, 0,0 }, 1);
            //team.pawnsInBench[0] = ReturnPlayerCode(5, 50, new int[] { 1, 0, 1, 0,0 }, 11);
            //team.pawnsInBench[1] = ReturnPlayerCode(6, 50, new int[] { 0, 0, 0, 0,0 }, 21);
           // team.pawnsInBench[2] = ReturnPlayerCode(7, 50, new int[] { 1, 0, 0, 0,1 }, 31);
            //team.pawnsInBench[3] = ReturnPlayerCode(8, 10, new int[] { 1, 0, 0, 1,0 }, 1);
            // for (int i = 0; i < team.UsableFormations.Length; i++) { team.UsableFormations[i] = -1; }
            for (int i = 0; i < team.ElixirInBench.Length; i++) { team.ElixirInBench[i] = -1; }
            team.ElixirInBench[0] = defultElixirIndex;
           // team.UsableFormations[0] = defultFormationIndex;
            return team;
        }

        public AssetType ReturnAssetTypeByName(string typeName)
        {
            AssetType type = AssetType.none;
            switch (typeName)
            {
                case "Pawn":
                    type = AssetType.Pawn;
                    break;
                case "Elixir":
                    type = AssetType.Elixir;
                    break;
                case "Formation":
                    type = AssetType.Formation;
                    break;
            }
            if (type == AssetType.none) { Errors.AddSmallError("AssetType not found"); }
            return type;
        }

        public Property ReturnPakagePrice(short level, int xpEmount)
        {
            Property pr = new Property();
            if(level<0 || 100 < level) {
                Errors.AddBigError("Utilities.returnPakagePrice. level is unAcceptable ");
                return pr; 
            }
            else
            {
                pr.Alminum = xpEmount * 50;
                pr.gold = xpEmount * 5;
                return pr;
            }            
        }



        public Property ReturnBaseTransaqtionPrice(short oldbase, short newbase)
        {
            Property pr = new Property();
            return pr;           
        }

        public int ReturnBaseUpgradeXp(int level)
        {
            switch (level)
            {
                case 0:
                    return 10;
                    break;
                case 1:
                    return 50;
                    break;
                case 2:
                    return 200;
                    break;
                case 3:
                    return 600;
                    break;
                case 4:
                    return 1500;
                    break;
                case 5:
                    return 3000;
                    break;
                case 6:
                    return 4000;
                    break;
                case 7:
                    return 5000;
                    break;
                case 8:
                    return 6000;
                    break;
                case 9:
                    return 7000;
                    break;
                case 10:
                    return 8000;
                    break;
                default:
                    break;
            }
            return 0;
        }
        public int ReturnPakageXPamunt(short level)
        {            
            if (level < 0 || 100 < level)
            {
                Errors.AddBigError("Utilities.returnPakagePrice. level is unAcceptable ");
                return -1;
            }
            else
            {               
                return (int)Math.Floor( ReturnBaseUpgradeXp(level)*0.25f);
            }
        }
        public AssetType ReturnOfferByName(string typeName)
        {
            AssetType type = AssetType.none;
            switch (typeName)
            {
                case "Pawn":
                    type = AssetType.Pawn;
                    break;
                case "Elixir":
                    type = AssetType.Elixir;
                    break;
                case "Formation":
                    type = AssetType.Formation;
                    break;
            }
            if (type == AssetType.none) { Errors.AddSmallError("AssetType not found"); }
            return type;
        }


        public Property SubtracProperty(Property currentProp , Property subbedProp)
        {
            Property newPro = new Property();
            newPro = currentProp;
            newPro.Alminum -= subbedProp.Alminum;
            newPro.fan -= subbedProp.fan;
            newPro.tropy -= subbedProp.tropy;
            newPro.gold -= subbedProp.gold;
            return newPro;
        }

    }
}