using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using soccer1.Models.main_blocks;
using System.ComponentModel.DataAnnotations;
using soccer1.Models.utilites;
using System.Web.Script.Serialization;
using System.Threading;




namespace soccer1.Models.main_blocks
{
    public class BuildPartOrder
    {
        public short partID;
        public RoboPartType partType;
        public short goldLevel;
        public int finishTimePoint;
        public BuildPartOrder( RoboPartType setpartType, short setgoldLevel)
        {            
            partType = setpartType;
            goldLevel = setgoldLevel;
            int minuteToBuild = new AssetManager().ReturnRoboPartTimeToBuild(partID);
            finishTimePoint = new Utilities().TimePointofNow()+ minuteToBuild;
            int testTime = new Utilities().TimePointofNow();
        }
        public BuildPartOrder()
        {
        }

        public long ToCode()//short partID, short partType, short goldLevel)
        {
            // return (int)partType * 300 + goldLevel * 80 + (int)partID;
            long result = 0;
            result += (long)goldLevel;
            result += (long)((short)partType * 3 );
            result += (long)(finishTimePoint) * 3 * 300;
            return result; 
        }
        public void FromCode(long buildOrderCode)
        {
            long factor = 3  * 300;
            long code = buildOrderCode;
            finishTimePoint = (int)(code / factor);
            code = code % factor;
            factor = 3 ;
            partType = (RoboPartType)(code / factor);
            code = code % factor;
            factor = 1;
            goldLevel = (short)(code / factor);
        }

        public int FinishCode()
        {
           
            BuildedRoboPart newpart = new BuildedRoboPart();
            newpart.partType = partType;
            switch (goldLevel)
            {
                case 0:
                    newpart.goldLevel = RoboPartGoldLevel.low;
                    break;
                case 1:
                    newpart.goldLevel = RoboPartGoldLevel.mediom;
                    break;
                case 2:
                    newpart.goldLevel = RoboPartGoldLevel.high;
                    break;
            }   
            return newpart.ReturnCode();
        }


    }

    public class PlayerForConnectedPlayer
    {

        public PlayerForConnectedPlayer()
        {
            connectedId = 0;
            Name = "Defult";
            totalXp = 1.0f;
            team = new TeamForConnectedPlayers();
            PlayerProperty = new Property();
            Utilities utilities = new Utilities();
            //team = utilities.returnDefultTeam();
            PlayerProperty = new Property();
            PlayerProperty.fan = 0;
            PlayerProperty.Alminum = Statistics.StartingAlminum;
            PlayerProperty.tropy = 1;
            PlayerProperty.gold = Statistics.StartingSS;
            buildOrders = new BuildPartOrder[4];
            RemaingPakages = new short[11];
            DoneMissions = new List<short>();
            DoneMissions.Clear();
    }

       // private DataDBContext dataBase = new DataDBContext();

        
        public string id { get; set; }

        public  Mutex mainMutex = new Mutex();

        public  Mutex SaveChengesMutex = new Mutex();

        public long[] BuildOrdersIntFormt = new long[4];
        public string Name { get; set; }

        public int connectedId { get; set; }

        public float totalXp { get; set; }
        
        public int totaltropy { get; set; }

       // public short[] BuyedBluePrints { get; set; }

        public string HistoryCode;

        public short[] RemaingPakages { get; set; }

        public long TimeOfXpPakageExpiartion { get; set; }
        public TeamForConnectedPlayers team { get; set; }

        public Property PlayerProperty;
        public int lastTimeUpdated;
        public float NumberOfTickets;

        public List<long> pawnOutOfTeam = new List<long>();

        public List<int> elixirOutOfTeam = new List<int>();

        public List<int> unAttachedPart = new List<int>();

        public List<int> useableFormations = new List<int>();

        public Convertors convertor = new Convertors();

        public BuildPartOrder[] buildOrders ;

        public bool isChanged=false;

        public List<short> DoneMissions = new List<short>();

        public List<short> BuyedBluePrints = new List<short>();

        public string sponsorName;
        

        public long playPrehibititionFinishTime;

        //public bool IsTHisAHostPlayer;

        #region public functions

        public bool UpgradePawnto(long pawnCode, int newBaseId)
        {

            PawnOfPlayerData pp = new PawnOfPlayerData();
            pp = new Convertors().PawnCodeToPawnOfPlayerData(pawnCode);
            RoboBase oldBase = new AssetManager().ReturnBaseInfo(pp.baceTypeIndex);
            RoboBase newbase = new AssetManager().ReturnBaseInfo(newBaseId);
            if (0 < pp.requiredXpForNextLevel)
            {
                Errors.AddClientError("player.UpgradePawnto.0 < requiredXpForNextLevel ");
                return false;
            }
            if (oldBase.upgradeToId1 != newBaseId && oldBase.upgradeToId2 != newBaseId && oldBase.upgradeToId3 != newBaseId)
            {
                Errors.AddClientError("player.UpgradePawnto. wrong transition ");
                return false;
            }
            Property transaqtionPrice = new Utilities().ReturnBaseTransaqtionPrice(oldBase.IdNum, newbase.IdNum);
            if (!new Utilities().CheckIfFirstPropertyIsBigger(PlayerProperty, transaqtionPrice))
            {
                Errors.AddClientError("player.BuyXpPakage. not enogth property");
                return false;
            }
            SubtractProperty(transaqtionPrice);
            pp.baceTypeIndex = newbase.IdNum;
            pp.requiredXpForNextLevel = new Utilities().ReturnBaseUpgradeXp(newbase.level);
            long newPawnCode = new Convertors().PawnOfPlayerDataToPawnCode(pp);
            PawnOfPlayerData forTest = new Convertors().PawnCodeToPawnOfPlayerData(newPawnCode);
            return SubstitutePawn(pawnCode, newPawnCode);
        }


        public bool BuyXpPakage(long pawnCode)
        {

            PawnOfPlayerData pp = new PawnOfPlayerData();
            pp = new Convertors().PawnCodeToPawnOfPlayerData(pawnCode);
            int baseLevel = new AssetManager().ReturnBaseLevel(pp.baceTypeIndex);
            if (baseLevel < 0)
            {
                Errors.AddBigError("player.BuyXpPakage.baseLevel is lower than 0  ");
                return false;
            }
            if (RemaingPakages.Length < baseLevel)
            {
                Errors.AddBigError("player.BuyXpPakage.baseLevel is higher than RemaingPakages .Length  ");
                return false;
            }
            if (RemaingPakages[baseLevel] < 1)
            {
                Errors.AddClientError("player.BuyXpPakage. RemaingPakages[" + baseLevel.ToString() + "] is zero");
                return false;
            }
            //int baseRXNL =  new AssetManager().ReturnBaseLevel()
            int xpAmunt = new Utilities().ReturnPakageXPamunt((short)baseLevel);
            Property pakagePrice = new Utilities().ReturnPakagePrice((short)baseLevel, xpAmunt);
            if (!new Utilities().CheckIfFirstPropertyIsBigger(PlayerProperty, pakagePrice))
            {
                Errors.AddClientError("player.BuyXpPakage. not enogth property");
                return false;
            }
            SubtractProperty(pakagePrice);
            pp.requiredXpForNextLevel -= xpAmunt;
            if (pp.requiredXpForNextLevel < 0) { pp.requiredXpForNextLevel = 0; }
            RemaingPakages[baseLevel] -= 1;
            long newPawnCode = new Convertors().PawnOfPlayerDataToPawnCode(pp);
            return SubstitutePawn(pawnCode, newPawnCode);
        }


        public BuyableXpPakage RequestPlayerPakage()
        {
            if (TimeOfXpPakageExpiartion <= new Utilities().TimePointofNow())
            {
                // renew Player pakage
                TimeOfXpPakageExpiartion = new Utilities().TimePointofNow() + 14400;
                short[] levels = ReturnlevelsOfPlayerPawns();
                for (int i = 0; i < RemaingPakages.Length; i++)
                {
                    RemaingPakages[i] = 0;
                }
                short minLevel = 0;
                while (levels[minLevel] < 1 && minLevel < (levels.Length - 1)) { minLevel++; }
                RemaingPakages[minLevel] = levels[minLevel];
                if (levels[minLevel] < 10)
                {
                    RemaingPakages[minLevel + 1] = (short)(10 - levels[minLevel]);
                }
                //SaveChanges();
            }
            BuyableXpPakage playerPack = new BuyableXpPakage();
            playerPack.expireTime = TimeOfXpPakageExpiartion;
            playerPack.special = "Defult";
            playerPack.requiredAl = new int[10];
            playerPack.requiredGold = new int[10];
            playerPack.xpValue = new int[10];
            Property vall;
            for (int i = 0; i < playerPack.requiredAl.Length; i++)
            {
                playerPack.xpValue[i] = new Utilities().ReturnPakageXPamunt((short)i);
                vall = new Utilities().ReturnPakagePrice((short)i, playerPack.xpValue[i]);
                playerPack.requiredAl[i] = vall.Alminum;
                playerPack.requiredGold[i] = vall.gold;
            }

            playerPack.numberOfAvalablePakages = new short[RemaingPakages.Length];
            for (int i = 0; i < playerPack.numberOfAvalablePakages.Length; i++)
            {
                playerPack.numberOfAvalablePakages[i] = RemaingPakages[i];
            }
            return playerPack;
        }


        public bool ElixirUse(int ElixirType)
        {
            bool result = false;
            int elixirPlace = -1;
            for (int i = 0; i < team.ElixirInBench.Length; i++) if (team.ElixirInBench[i] == ElixirType) { elixirPlace = i; }
            if (-1 < elixirPlace)
            {
                team.ElixirInBench[elixirPlace] = -1;
                //SaveChanges();
                result = true;
            }

            return result;
        }
        public void GainMatchResult(GainedFromMatch gainedResult)
        { 

            totalXp += team.AddXpToTeam(gainedResult.xpVAl[gainedResult.xpVAl.Length - 1]);
            for (int i = 0; i < gainedResult.AssingedIndex.Length; i++)
            {
                if (0 < gainedResult.AssingedIndex[i])
                {
                    totalXp += team.AddXpToPawn(gainedResult.AssingedIndex[i], gainedResult.xpVAl[i]);
                }
            }
            AddProperty(gainedResult.gained);
            
            

            //SaveChanges();


        }


        public bool MissionDone(short missionId)
        {
            if(DoneMissions!= null)
            {
                foreach (short s in DoneMissions)
                {
                    if (s == missionId)
                    {
                        return false;
                    }
                }
            }
            else
            {
                DoneMissions = new List<short>();
            }
            
           
            MissionDefinition donemission = new AssetManager().ReturnMission(missionId);
            if(donemission== null) { return false; }
            short[] preReq = convertor.StringToShorttArray(donemission.preRequisite);
            bool resultcheck = false;
            if(preReq != null)
            {
                for (int i = 0; i < preReq.Length; i++)
                {
                    resultcheck = false;
                    foreach (short s in DoneMissions)
                    {
                        if (s == preReq[i])
                        {
                            resultcheck = true;
                        }
                    }
                    if (!resultcheck)
                    {
                        return false;
                    }
                }
            }
            DoneMissions.Add(missionId);
            Property toAdd = new Property();
            toAdd.Alminum = 0;
            toAdd.fan = 0;
            toAdd.tropy = 0;
            toAdd.gold = donemission.rewardInGold;
            AddProperty(toAdd);
            return true;           
        }


        public bool AddBuildOrder(RoboPartType partType, short goldLevel, short builderId)
        {
            Property requairedProperty = new AssetManager().ReturnOrderPrice(partType, goldLevel);
            if(buildOrders[builderId] == null) { return false; }
           if(SubtractProperty(requairedProperty))
            {
                buildOrders[builderId] = new BuildPartOrder( partType, goldLevel);
                //SaveChanges();
                return true;
                
            }
            else
            {
                return false;
            }
        }

       // public void AddSponserVAlue

        public bool ChangeSponser(string newSponserName)
        {
                UpdateAll();        
                sponsorName = newSponserName;
                NumberOfTickets = 3;
                //SaveChanges();
                return true;
        }

        public float ReturnNumberOfTickets() { return NumberOfTickets; }

        public bool ScrapPartOrder(short partID, short partType, short goldLevel)
        {
           int partcode = goldLevel * 80 + partID + partType * 300;
            if (unAttachedPart.Contains(partcode))
            {
                unAttachedPart.Remove(partcode);
                Property ReturnedProperty = new AssetManager().ReturnScrapPrice(partID, goldLevel);
                AddProperty(ReturnedProperty);
                //SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }



        public string FinishBuildOrderIfFinishTimeReached(short builderId)
        {
            if(builderId<0 || buildOrders.Length<= builderId)
            {
                Errors.AddBigError("player. FinishBuildOrderIfFinishTimeReached. builderId out of rang");
                return "builderId out of rang";// just for not making bugs
            }
            if (buildOrders[builderId] == null) {
                Errors.AddBigError("player. FinishBuildOrderIfFinishTimeReached. builderId in empty");
                return "builderId in empty";// just for not making bugs
            }

            if (buildOrders[builderId].finishTimePoint - new Utilities().TimePointofNow() <=1)                
            {
                unAttachedPart.Add(buildOrders[builderId].FinishCode());
                buildOrders[builderId].partID = 0;
                buildOrders[builderId].finishTimePoint = 0;
                buildOrders[builderId].goldLevel = 0;
                buildOrders[builderId].partType = 0;
                //SaveChanges();
                return "Done";
            }
            else
            {
                return buildOrders[builderId].finishTimePoint.ToString();
            }
        }


        public string CancelBuildPartOrder(short builderId)
        {
            if (builderId < 0 || buildOrders.Length <= builderId)
            {
                Errors.AddBigError("player. CancelBuildPartOrder. builderId out of rang");
                return "builderId out of rang";// just for not making bugs
            }
            if (buildOrders[builderId] == null)
            {
                Errors.AddBigError("player. CancelBuildPartOrder. builderId in empty");
                return "builderId in empty";// just for not making bugs
            }
            Property ReturnedProperty = new AssetManager().ReturnOrderPrice(buildOrders[builderId].partType, buildOrders[builderId].goldLevel);
            ReturnedProperty.Alminum =(int) Math.Floor(ReturnedProperty.Alminum * 0.5f);
            AddProperty(ReturnedProperty);
            buildOrders[builderId] = null;
            //SaveChanges();
            return "Done";
        }




        public bool BuyAsset(AssetType assetType, int AssetIdNum, Property price)
        {
            Utilities utilities = new Utilities();
            if (utilities.CheckIfFirstPropertyIsBigger(PlayerProperty, price)) {
                SubtractProperty(price);
                //Log.AddLog("Error : assetType of asset:" + assetType);
                int newEntity = AssetIdNum;
                switch (assetType)
                {

                    case AssetType.Pawn:
                        pawnOutOfTeam.Add(new AssetManager().ReturnAssetIndex(AssetType.Pawn, AssetIdNum));
                     
                        break;
                    case AssetType.Elixir:
                        int tset = elixirOutOfTeam.Count;
                        newEntity = new AssetManager().ReturnAssetIndex(AssetType.Elixir, AssetIdNum);
                        

                        elixirOutOfTeam.Add(newEntity);
                        break;
                    case AssetType.Formation:
                      // team.AddToUsableFormations(new AssetManager().ReturnAssetIndex(AssetType.Formation, AssetIdNum));
                        break;

                }
                //SaveChanges();
                return true;
            }
            else{
                return false;
            }
        }

        public bool BuyPawnAsset(int idNum, int playerindex, Property price )
        {

            if(idNum<0 || playerindex < 0)
            {
                Errors.AddBigError("player.BuyPawnAsset");
                return false;
            }
            Utilities utilities = new Utilities();
            if (utilities.CheckIfFirstPropertyIsBigger(PlayerProperty, price))
            {
                SubtractProperty(price);
                // Pawn newPawn = new AssetManager().ReturnAssetIndex(AssetType.Pawn, idCode);
                PawnOfPlayerData newPawn = new PawnOfPlayerData();
               // newPawn.pawnType = idNum;
                newPawn.playerPawnIndex = playerindex;
                newPawn.requiredXpForNextLevel = new AssetManager().ReturnrequiredXpForNextLevel(idNum);
               //int NewPawnCode =new Convertors().PawnOfPlayerDataToPawnCode(newPawn);
              //  pawnOutOfTeam.Add(NewPawnCode);                
               // SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool BuyOffer(Property buyedMaterial, Property price)
        {
            Utilities utilities = new Utilities();
            if (utilities.CheckIfFirstPropertyIsBigger(PlayerProperty, price))
            {
                SubtractProperty(price);
                AddProperty(buyedMaterial);              
                //SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool SubtractProperty(Property prop)
        {
            Log.AddPlayerLog(id, "SubtractProperty : coin :" + prop.Alminum.ToString());
            Utilities utilities = new Utilities();
            if (utilities.CheckIfFirstPropertyIsBigger(PlayerProperty, prop))
            {
                PlayerProperty.Alminum -= prop.Alminum;
                PlayerProperty.gold -= prop.gold;
                PlayerProperty.fan -= prop.fan;
                //SaveChanges();
                return true;
            }
            else
            {
                Errors.AddClientError("Not Enogh property");
                return false;
            }
        }


        public void AddProperty(Property prop)
        {
            Log.AddPlayerLog(id, "AddProperty : coin :" + prop.Alminum.ToString());
            PlayerProperty.Alminum += prop.Alminum;
            PlayerProperty.gold += prop.gold;
            PlayerProperty.fan += prop.fan;
            PlayerProperty.tropy += prop.tropy;
           // SaveChanges();
        }

        public bool ChangeTeam(TeamForConnectedPlayers newTeammm)
        {
            List<long> pawnBuffer = new List<long>();
            List<int> ElixirBuffer = new List<int>();
            foreach(long i in pawnOutOfTeam) { pawnBuffer.Add(i); }
            foreach (int i in elixirOutOfTeam) { ElixirBuffer.Add(i); }
            bool result=true;
            for(int i=0; i< team.PlayeingPawns.Length; i++)
            {
                pawnBuffer.Add(team.PlayeingPawns[i]);
            }
            for (int i = 0; i < team.pawnsInBench.Length; i++)
            {
                pawnBuffer.Add(team.pawnsInBench[i]);
            }
           
            

            for (int i = 0; i < newTeammm.PlayeingPawns.Length; i++)
            {
                if (pawnBuffer.Exists(p => p== newTeammm.PlayeingPawns[i]))
                {
                    pawnBuffer.Remove(newTeammm.PlayeingPawns[i]);
                }
                else
                {
                    result= false;
                }                
            }

            for (int i = 0; i < newTeammm.pawnsInBench.Length; i++)
            {
                if (pawnBuffer.Exists(p => p == newTeammm.pawnsInBench[i]))
                {
                    pawnBuffer.Remove(newTeammm.pawnsInBench[i]);
                }
                else
                {
                    result= false;
                }
            }
           

            
            for (int i = 0; i < team.ElixirInBench.Length; i++)
            {
                ElixirBuffer.Add(team.ElixirInBench[i]);
            }
           
            

            for (int i = 0; i < newTeammm.ElixirInBench.Length; i++)
            {
                if (ElixirBuffer.Exists(p => p == newTeammm.ElixirInBench[i]))
                {
                    ElixirBuffer.Remove(newTeammm.ElixirInBench[i]);
                }
                else
                {
                    result= false;
                }
            }

            if (result)
            {
                pawnOutOfTeam = pawnBuffer;
                elixirOutOfTeam = ElixirBuffer;
                team = newTeammm;
                //SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool ChangeRoboParts(long[] teamRobos, List<long> outofTeams )
        {
            List<long> changingOLdPawns = new List<long>();
            List<int> partsOfOldPawns = new List<int>();
            List<int> basesOfOldPawns = new List<int>();
            List<int> partsOfNewPawns = new List<int>();
            List<int> basesOfNewPawns = new List<int>();            
            // dis assemble parts of old Pawns
            for (int i=0; i< teamRobos.Length; i++) if (0 <= teamRobos[i])
                {
                    if (i < 5)
                    {
                        changingOLdPawns.Add(team.PlayeingPawns[i]);
                    }
                    if (4<i )
                    {
                        changingOLdPawns.Add(team.pawnsInBench[i-5]);
                    }
            }

            if (0 < outofTeams.Count)
            {
                foreach (long l in pawnOutOfTeam)
                {
                    changingOLdPawns.Add(l);
                }
            }
            foreach (int p in unAttachedPart)
            {
                partsOfOldPawns.Add(p);
            }
            
            foreach (long pCode in changingOLdPawns)
            {
                Pawn pawn = new Pawn();
                pawn.SetRoboByCode(pCode);
                basesOfOldPawns.Add(pawn.baseTypeIndex);
                for(int i=0; i<pawn.parts.Length; i++)if(pawn.parts[i] != null)
                {
                        if (pawn.parts[i].partType != RoboPartType.none)
                        {
                            partsOfOldPawns.Add(pawn.parts[i].ReturnCode());
                        }
                }
            }

            // dis assemble parts of new Pawns
            for(int i =0; i< teamRobos.Length; i++) if (0 <= teamRobos[i])
                {
                    Pawn pawn = new Pawn();
                    pawn.SetRoboByCode(teamRobos[i]);
                    basesOfNewPawns.Add(pawn.baseTypeIndex);
                    for (int j = 0; j< pawn.parts.Length; j++) if (pawn.parts[j] != null)
                        {
                            if (pawn.parts[j].partType != RoboPartType.none)
                            {
                                partsOfNewPawns.Add(pawn.parts[j].ReturnCode());
                            }
                        }
                }

            foreach (long pCode in outofTeams)
            {
                Pawn pawn = new Pawn();
                pawn.SetRoboByCode(pCode);
                basesOfNewPawns.Add(pawn.baseTypeIndex);
                for (int i = 0; 0 < pawn.parts.Length; i++) if (pawn.parts[i] != null)
                    {
                        if ( pawn.parts[i].partType != RoboPartType.none)
                        {
                            partsOfNewPawns.Add(pawn.parts[i].ReturnCode());
                        }
                    }
            }

            // all parts are dis assembled;
            foreach(int partCode in partsOfNewPawns)
            {
                if (partsOfOldPawns.Contains(partCode))
                {
                    partsOfOldPawns.Remove(partCode);
                }
                else
                {
                    return false;
                    // error in parts
                }
            }
            foreach (int baseid in basesOfNewPawns)
            {
                if (basesOfOldPawns.Contains(baseid))
                {
                    basesOfOldPawns.Remove(baseid);
                }
                else
                {
                    return false;
                    // error in bases
                }
            }

            // changes accepted
            for (int i = 0; i < teamRobos.Length; i++) if (0 <= teamRobos[i])
                {

                    if (i < 5)
                    {
                        team.PlayeingPawns[i] = teamRobos[i];
                    }
                    if (4 < i )
                    {
                        team.pawnsInBench[i-5] = teamRobos[i];
                    }
                }
            if (0 < outofTeams.Count)
            {
                pawnOutOfTeam = outofTeams;
            }

            unAttachedPart = partsOfOldPawns;
            return true;
        }

        /*
        public PlayerForSerial ReturnPlayrSerial()
        {
            PlayerForSerial plsr = new PlayerForSerial();
            plsr.id =id;
            plsr.Fan = PlayerProperty.fan;
            plsr.Name = Name;
            plsr.Money = PlayerProperty.coin;
            plsr.CoonId = connectedId;
            plsr.PowerLevel =PowerLevel;
            plsr.level = PlayerProperty.level;
            plsr.SoccerSpetial = PlayerProperty.SoccerSpetial;
            
            plsr.Team = convertor.TeamToTeamForSerialize(team);
            int[] pawnsarray = new int[pawnOutOfTeam.Count];
            int pawncounter = 0;
            foreach (int i in pawnOutOfTeam)
            {
                pawnsarray[pawncounter]= new AssetManager().ReturnAssetName(AssetType.Pawn, i);
                pawncounter++;
            }
            int[] elixirsarray = new int[elixirOutOfTeam.Count];
            int elixirCounter = 0;
            foreach (int i in elixirOutOfTeam)
            {
                elixirsarray[elixirCounter] = new AssetManager().ReturnAssetName(AssetType.Elixir, i); ;
                elixirCounter++;
            }

            
            plsr.OutOfTeamPawns = pawnsarray; 
            plsr.OutOfTeamElixirs = elixirsarray; 
            return plsr;
        }
    */

        #endregion



        public  void reWriteAccordingTo(PlayerForDatabase pl)
        {
            mainMutex.WaitOne();
            id = pl.id;
            PlayerProperty.gold = pl.gold;
            Name = pl.Name;
            PlayerProperty.Alminum = pl.Almimun;
            connectedId = -1;
            totalXp = pl.totalXp;
            HistoryCode = pl.HistoryCode;
            //totaltropy = pl.Tropy;
            PlayerProperty.tropy = pl.Tropy;
            PlayerProperty.gold = pl.gold;
            team.StartFomation = pl.StartFomation;
            team.AttackFormation = pl.AttackFormation;
            team.DefienceForation = pl.DefienceForation;
            TimeOfXpPakageExpiartion = pl.timeOfXpPakageExpiration;
            RemaingPakages = convertor.StringToShortArray(pl.remaingPakages);
            BuyedBluePrints = convertor.StringToShortList(pl.buyedBluePrints);
            NumberOfTickets = pl.NumberOfTickets;
            lastTimeUpdated = pl.lastTimeUpdated;
            sponsorName = pl.sponsorName;
            playPrehibititionFinishTime = pl.playPrehibititionFinishTime;
            //DoneMissions = pl.DoneMissions
            //convert of Playerfordatabase Class to Player Class 
            //convert string to int
            //int[] pawnBuffer = convertor.StringToIntArray(pl.outOfTeamPawns);
            long[] pawnBuffer = convertor.StringToLongIntArray(pl.outOfTeamPawns);
            pawnOutOfTeam.Clear();
            for (int i = 0; i < pawnBuffer.Length; i++) if (0 <= pawnBuffer[i])
                {
                    pawnOutOfTeam.Add(pawnBuffer[i]);
                }
            int[] ElixirBuffer = convertor.StringToIntArray(pl.otherElixirs);
            
            elixirOutOfTeam.Clear();
            for (int i = 0; i < ElixirBuffer.Length; i++) if (0 <= ElixirBuffer[i])
                {
                    elixirOutOfTeam.Add(ElixirBuffer[i]);
                }
            int[] partBuffer = convertor.StringToIntArray(pl.UnAtachedParts);
            unAttachedPart.Clear();
            for (int i = 0; i < partBuffer.Length; i++) if (0 <= partBuffer[i])
                {
                    unAttachedPart.Add(partBuffer[i]);
                }

            int[] useableFormationsBuffer = convertor.StringToIntArray(pl.UsableFormations);
            useableFormations.Clear();
            for (int i = 0; i < useableFormationsBuffer.Length; i++) if (0 <= useableFormationsBuffer[i])
                {
                    useableFormations.Add(useableFormationsBuffer[i]);
                }
            team.PlayeingPawns = convertor.StringToLongIntArray(pl.PlayeingPawns);
            team.pawnsInBench = convertor.StringToLongIntArray(pl.pawnsInBench);
            //team.UsableFormations = convertor.StringToIntArray(pl.UsableFormations);
            
            team.ElixirInBench = convertor.StringToIntArray(pl.ElixirInBench);
            BuildOrdersIntFormt = convertor.StringToLongIntArray(pl.buildOrders);
            for (int i=0; i<buildOrders.Length; i++)
            {
                if (0 <= BuildOrdersIntFormt[i])
                {
                    buildOrders[i] = new BuildPartOrder();
                    buildOrders[i].FromCode(BuildOrdersIntFormt[i]);
                }
            }
            DoneMissions = convertor.StringToShortList(pl.DoneMissions);
           // IsTHisAHostPlayer = pl.IsTHisAHostPlayer;
            mainMutex.ReleaseMutex();
        }


        /*
        private PlayerForDatabase returnDataBaseVersion()
        {
            mainMutex.WaitOne();
            PlayerForDatabase plsrs = new PlayerForDatabase();
            plsrs.StartFomation =team.StartFomation;
            plsrs.AttackFormation = team.AttackFormation;
            plsrs.DefienceForation = team.DefienceForation;
            plsrs.ElixirInBench = convertor.IntArrayToSrting(team.ElixirInBench);
            plsrs.gold = PlayerProperty.fan;
            plsrs.id = id;
            plsrs.level = PlayerProperty.level;
            plsrs.Almimun = PlayerProperty.Alminum;
            plsrs.Name = Name;
            plsrs.otherElixirs = convertor.IntArrayToSrting(convertor.listIntToIntArray(elixirOutOfTeam));
            plsrs.UnAtachedParts = convertor.IntArrayToSrting(convertor.listIntToIntArray(unAttachedPart));
            plsrs.outOfTeamPawns = convertor.LongIntArrayToSrting(convertor.listLongToLongArray(pawnOutOfTeam));
            plsrs.pawnsInBench = convertor.LongIntArrayToSrting(team.pawnsInBench);
            plsrs.PlayeingPawns = convertor.LongIntArrayToSrting(team.PlayeingPawns);
            plsrs.PowerLevel = PowerLevel;
            plsrs.gold = PlayerProperty.gold;
            plsrs.lastTimeSponsorAddDone = lastTimeSponsorAddDone;
            plsrs.sponsorAlmimun = sponsorAlmimun;
            plsrs.sponsorGold = sponsorGold;
            plsrs.sponsorId = sponsorId;
            //  plsrs.UsableFormations = convertor.IntArrayToSrting(team.UsableFormations);
            mainMutex.ReleaseMutex();
            return plsrs;
        }
        */

       

        public void SubtractSponserAticket()
        {
            if (0 < NumberOfTickets)
            {
                NumberOfTickets = NumberOfTickets-1.0f;
                //SaveChanges();                
            }
            else
            {
                Errors.AddClientError("player.SubtractSponserAticket.");
            }
        }

        public void UpdateAll()
        {
            
            int CurrentTime = new Utilities().TimePointofNow();
            int clapsedTime = CurrentTime - lastTimeUpdated;            
            Sponsor thisplayerSponser = new AssetManager().ReturnSponsor(sponsorName);
            if (thisplayerSponser != null)
            {
                NumberOfTickets += (clapsedTime*1.0f) / (thisplayerSponser.TimeBetweenTickets * 1.0f);
                if (thisplayerSponser.MaxTickets < NumberOfTickets)
                {
                    NumberOfTickets = thisplayerSponser.MaxTickets;
                }                
            }
            lastTimeUpdated = CurrentTime;
            //SaveChanges();
        }



        public PlayerForSerial ReturnForSerialize()
        {
            PlayerForSerial serialplayer = new PlayerForSerial();
            serialplayer.id = id;
            serialplayer.Gold = PlayerProperty.gold;
            serialplayer.totaltropy = PlayerProperty.tropy;
            serialplayer.Alminum = PlayerProperty.Alminum;
            serialplayer.Name = Name;
            //serialplayer.SoccerSpetial = player.gold;
            serialplayer.OutOfTeamElixirs =elixirOutOfTeam.ToArray();
            serialplayer.UnAttachedPart = unAttachedPart.ToArray();
            serialplayer.OutOfTeamPawns = pawnOutOfTeam.ToArray();
            //serialplayer.OutOfTeamPawnsRequiredXp = SrtingTointArray(player.otherPawnsRequiredXp);
            serialplayer.totalXp = totalXp;
            // serialplayer.SoccerSpetial = player.SoccerSpetial;
            serialplayer.Team.StartFomation =team.StartFomation;
            serialplayer.Team.AttackFormation = team.AttackFormation;
            serialplayer.Team.DefienceForation = team.DefienceForation;
            serialplayer.Team.ElixirInBench =team.ElixirInBench;
            serialplayer.Team.pawnsInBench = team.pawnsInBench;
            serialplayer.buyedBluePrints = BuyedBluePrints.ToArray();
            //serialplayer.PawnsinBenchRequiredXp = SrtingTointArray(player.pawnsInBenchRequiredXp);
            serialplayer.Team.PlayeingPawns = team.PlayeingPawns;
            //serialplayer.PlayingPawnsRequiredXp = SrtingTointArray(player.PlayeingPawnsRequiredXp);
            // serialplayer.Team.UsableFormations = StringToIntArray(player.UsableFormations);
            serialplayer.UsebaleFormations = useableFormations.ToArray();

            serialplayer.DoneMissions = DoneMissions.ToArray();
            serialplayer.buildOrders = new long[buildOrders.Length];
            for (int i = 0; i < buildOrders.Length; i++)
            {
                if (buildOrders[i] != null)
                {
                    serialplayer.buildOrders[i] = buildOrders[i].ToCode();
                }
                else
                {
                    serialplayer.buildOrders[i] = -1;
                }
            }
            serialplayer.remaningPakages = RemaingPakages;
            serialplayer.TimeOfXpPakageExpiartion = TimeOfXpPakageExpiartion;
            serialplayer.sponsorName = sponsorName;
            serialplayer.lastTimeUpdate = lastTimeUpdated;
            serialplayer.numberOfTicket = NumberOfTickets;
                



            //serialplayer.sponsorId = sponsorId;
            
            
            serialplayer.playPrehibititionFinishTime = playPrehibititionFinishTime;
            //serialplayer.IsTHisAHostPlayer = IsTHisAHostPlayer;
            return serialplayer;
        }


        /*
                public void SaveChanges()
        {
            mainMutex.WaitOne();
            PlayerForDatabase thisPlayerAtServer =dataBase.playerInfoes.Find(id);
            thisPlayerAtServer.AttackFormation = team.AttackFormation;
            thisPlayerAtServer.StartFomation = team.StartFomation;
            thisPlayerAtServer.DefienceForation = team.DefienceForation;
            thisPlayerAtServer.ElixirInBench = convertor.IntArrayToSrting(team.ElixirInBench);
            thisPlayerAtServer.gold = PlayerProperty.gold;
            //plsrs.id = id;
            thisPlayerAtServer.Tropy = totaltropy;
            thisPlayerAtServer.Almimun = PlayerProperty.Alminum;
            thisPlayerAtServer.Name = Name;
            thisPlayerAtServer.otherElixirs = convertor.IntArrayToSrting(convertor.listIntToIntArray(elixirOutOfTeam));
            thisPlayerAtServer.UnAtachedParts = convertor.IntArrayToSrting(convertor.listIntToIntArray(unAttachedPart));
            thisPlayerAtServer.outOfTeamPawns = convertor.LongIntArrayToSrting(convertor.listLongToLongArray(pawnOutOfTeam));
            thisPlayerAtServer.pawnsInBench = convertor.LongIntArrayToSrting(team.pawnsInBench);
            thisPlayerAtServer.PlayeingPawns = convertor.LongIntArrayToSrting(team.PlayeingPawns);
            thisPlayerAtServer.totalXp = totalXp;
            thisPlayerAtServer.gold = PlayerProperty.gold;
            thisPlayerAtServer.sponsorName = sponsorName;
            for (int i =0; i <buildOrders.Length; i++) {
                BuildOrdersIntFormt[i] = -1;
                if(buildOrders[i] != null)
                {
                    BuildOrdersIntFormt[i] = buildOrders[i].ToCode();
                }
            }
            thisPlayerAtServer.DoneMissions = convertor.ShortListToSrting(DoneMissions);
            thisPlayerAtServer.buildOrders = convertor.LongIntArrayToSrting(BuildOrdersIntFormt);
            thisPlayerAtServer.remaingPakages = convertor.ShortArrayToSrting(RemaingPakages);
            thisPlayerAtServer.timeOfXpPakageExpiration = TimeOfXpPakageExpiartion;
            thisPlayerAtServer.NumberOfTickets = NumberOfTickets;
            thisPlayerAtServer.lastTimeUpdated = lastTimeUpdated;

            //thisPlayerAtServer.IsTHisAHostPlayer = IsTHisAHostPlayer;
            //  thisPlayerAtServer.UsableFormations = convertor.IntArrayToSrting(team.UsableFormations);            
            dataBase.Entry(thisPlayerAtServer).State = EntityState.Modified;
            dataBase.SaveChanges();
            mainMutex.ReleaseMutex();
        }
        */
        /*
        public void AddTodDataBase()
        {
            mainMutex.WaitOne();
            PlayerForDatabase playerInfo = returnDataBaseVersion();
            dataBase.playerInfoes.Add(playerInfo);
            dataBase.SaveChanges();
            mainMutex.ReleaseMutex();
        }
        */

        public TeamForConnectedPlayers ReturnYourTeam()
        {
            return team;
        }

        #region inner function
        private short[] ReturnlevelsOfPlayerPawns()
        {

            short[] levels = new short[20];
            PawnOfPlayerData pp;
            int baseLevel;
            foreach (long p in pawnOutOfTeam)
            {
                pp = new Convertors().PawnCodeToPawnOfPlayerData(p);
                baseLevel = new AssetManager().ReturnBaseLevel(pp.baceTypeIndex);
                levels[baseLevel]++;
            }           
            for (int i = 0; i < team.PlayeingPawns.Length; i++)
            {
                pp = new Convertors().PawnCodeToPawnOfPlayerData(team.PlayeingPawns[i]);
                baseLevel = new AssetManager().ReturnBaseLevel(pp.baceTypeIndex);
                levels[baseLevel]++;
            }
            for (int i = 0; i < team.pawnsInBench.Length; i++)
            {
                pp = new Convertors().PawnCodeToPawnOfPlayerData(team.pawnsInBench[i]);
                baseLevel = new AssetManager().ReturnBaseLevel(pp.baceTypeIndex);
                levels[baseLevel]++;
            }
            return levels;
        }

        private bool SubstitutePawn(long OldCode , long newCode)
        {
            bool isREmovedPastFromPawnOutOfTeam = false;
            isREmovedPastFromPawnOutOfTeam = pawnOutOfTeam.Remove(OldCode);
            if (isREmovedPastFromPawnOutOfTeam)
            {
                pawnOutOfTeam.Add(newCode);
                //SaveChanges();
                return true;
            }
            for (int i = 0; i < team.PlayeingPawns.Length; i++)
            {
                int isFindedPlace = -1;
                if (team.PlayeingPawns[i] == OldCode)
                {
                    isFindedPlace = i;
                }
                if (-1 < isFindedPlace)
                {
                    team.PlayeingPawns[isFindedPlace] = newCode;
                    //SaveChanges();
                    return true;
                }
            }
            for (int i = 0; i < team.pawnsInBench.Length; i++)
            {
                int isFindedPlace = -1;
                if (team.pawnsInBench[i] == OldCode)
                {
                    isFindedPlace = i;

                }
                if (-1 < isFindedPlace)
                {
                    team.pawnsInBench[i] = newCode;
                    //SaveChanges();
                    return true;
                }
            }
            return false;
        }
        #endregion
    }

   

}
