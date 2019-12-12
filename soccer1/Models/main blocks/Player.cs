using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using soccer1.Models.main_blocks;
using System.ComponentModel.DataAnnotations;
using soccer1.Models.utilites;
using soccer1.Models.DataBase;
using System.Web.Script.Serialization;
using System.Threading;




namespace soccer1.Models.main_blocks
{
    public class BuildPartOrder
    {
        public short partID;
        public short partType;
        public short goldLevel;
        public int finishTimePoint;
        public BuildPartOrder(short setpartID, short setpartType, short setgoldLevel)
        {
            partID = setpartID;
            partType = setpartType;
            goldLevel = setgoldLevel;
            int minuteToBuild = new AssetManager().ReturnRoboPartTimeToBuild(partID, partType);
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
            result += (long)(partType * 3);
            result += (long)(partID * 3 * 10);
            result += (long)(finishTimePoint) * 3 * 10 * 300;
            return result; 
        }
        public void FromCode(long buildOrderCode)
        {
            long factor = 3 * 10 * 300;
            long code = buildOrderCode;
            finishTimePoint = (int)(code / factor);
            code = code % factor;
            factor = 3 * 10;
            partID = (short)(code / factor);
            code = code % factor;
            factor = 3;
            partType = (short)(code / factor);
            code = code % factor;
            factor = 1;
            goldLevel = (short)(code / factor);
        }

        public int FinishCode()
        {
            int orderCode = goldLevel*80;
            orderCode += partType * 300;
            orderCode += partID;
            return orderCode;
        }


    }

    public class PlayerForConnectedPlayer
    {

        public PlayerForConnectedPlayer()
        {
            connectedId = 0;
            Name = "Defult";
            PowerLevel = 1.0f;
            team = new TeamForConnectedPlayers();
            PlayerProperty = new Property();
            Utilities utilities = new Utilities();
            //team = utilities.returnDefultTeam();
            PlayerProperty = new Property();
            PlayerProperty.fan = 0;
            PlayerProperty.Alminum = Statistics.StartingAlminum;
            PlayerProperty.level = 1;
            PlayerProperty.gold = Statistics.StartingSS;
            buildOrders = new BuildPartOrder[4];
    }

        private DataDBContext dataBase = new DataDBContext();

        public string id { get; set; }

        public static Mutex mainMutex = new Mutex();

        public static Mutex SaveChengesMutex = new Mutex();

        long[] BuildOrdersIntFormt = new long[4];
        private string Name { get; set; }

        public int connectedId { get; set; }

        public float PowerLevel { get; set; }

        private TeamForConnectedPlayers team { get; set; }

        private Property PlayerProperty;
               
        private List<long> pawnOutOfTeam = new List<long>();

        private List<int> elixirOutOfTeam = new List<int>();

        private List<int> unAttachedPart = new List<int>();

        private Convertors convertor = new Convertors();

        private BuildPartOrder[] buildOrders ; 

        private bool isChanged=false;

        private short[] DoneMissions;

        #region public functions

        public void GainMatchXp(GainedXp gainedXp)
        {
            
            team.AddXpToTeam(gainedXp.xpVAl[gainedXp.xpVAl.Length - 1]);
            for (int i = 0; i < gainedXp.AssingedIndex.Length; i++)
            {
                if (0 < gainedXp.AssingedIndex[i])
                {
                    team.AddXpToPawn(gainedXp.AssingedIndex[i], gainedXp.xpVAl[i]);
                }
            }
            //SaveChanges();
            
            
        }


        public bool AddBuildOrder(short partID, short partType, short goldLevel, short builderId)
        {
            Property requairedProperty = new AssetManager().ReturnOrderPrice(partID, partType, goldLevel);
            if(buildOrders[builderId] == null) { return false; }
           if(SubtractProperty(requairedProperty))
            {
                buildOrders[builderId] = new BuildPartOrder(partID, partType, goldLevel);
                //SaveChanges();
                return true;
                
            }
            else
            {
                return false;
            }
        }


        public bool ScrapPartOrder(short partID, short partType, short goldLevel)
        {
           int partcode = goldLevel * 80 + partID + partType * 300;
            if (unAttachedPart.Contains(partcode))
            {
                unAttachedPart.Remove(partcode);
                Property ReturnedProperty = new AssetManager().ReturnScrapPrice(partID, partType, goldLevel);
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
            Property ReturnedProperty = new AssetManager().ReturnOrderPrice(buildOrders[builderId].partID, buildOrders[builderId].partType, buildOrders[builderId].goldLevel);
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
            PlayerProperty.level += prop.level;
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
                        if (0 < pawn.parts[i].PartID)
                        {
                            partsOfOldPawns.Add(pawn.parts[i].ReturnCodeWithType());
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
                            if (0 < pawn.parts[j].PartID)
                            {
                                partsOfNewPawns.Add(pawn.parts[j].ReturnCodeWithType());
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
                        if (0 < pawn.parts[i].PartID)
                        {
                            partsOfNewPawns.Add(pawn.parts[i].ReturnCodeWithType());
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

        public bool UpgradePawnto(int pawnCode, int newPawnType) {
            
            PawnOfPlayerData pp = new PawnOfPlayerData();
            pp = new Convertors().PawnCodeToPawnOfPlayerData(pawnCode);
            pp.parts[0] = newPawnType;
            pp.requiredXpForNextLevel = new AssetManager().ReturnrequiredXpForNextLevel(newPawnType);
            long newPawnCode = new Convertors().PawnOfPlayerDataToPawnCode(pp);
            bool isFinded = false;
            bool isREmovedPastFromPawnOutOfTeam = false;

            isREmovedPastFromPawnOutOfTeam = pawnOutOfTeam.Remove( pawnCode);
            if (isREmovedPastFromPawnOutOfTeam)
            {
                pawnOutOfTeam.Add(newPawnCode);
                //SaveChanges();
                return true;
            }
            for (int i = 0; i < team.PlayeingPawns.Length; i++)
            {
                int isFindedPlace = -1;
                if (team.PlayeingPawns[i] == pawnCode)
                {
                    isFindedPlace = i;                   
                }
                if (-1 < isFindedPlace)
                {
                    team.PlayeingPawns[isFindedPlace] = newPawnCode;
                    //SaveChanges();
                    return true;
                }
            }
            for (int i = 0; i < team.pawnsInBench.Length; i++)
            {
                int isFindedPlace = -1;
                if (team.pawnsInBench[i] == pawnCode)
                {
                    isFindedPlace = i;
                    
                }
                if (-1 < isFindedPlace)
                {
                    team.pawnsInBench[i] = newPawnCode;
                    //SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool ElixirUse(int ElixirType)
        {
            bool result = false;
            int elixirPlace = -1;
            for (int i=0; i < team.ElixirInBench.Length; i++)if(team.ElixirInBench[i]== ElixirType) { elixirPlace = i; }
            if (-1 < elixirPlace)
            {
                team.ElixirInBench[elixirPlace] = -1;
                //SaveChanges();
                result = true;
            }

            return result;
        }


        public  void reWriteAccordingTo(PlayerForDatabase pl)
        {
            mainMutex.WaitOne();
            id = pl.id;
            PlayerProperty.gold = pl.gold;
            Name = pl.Name;
            PlayerProperty.Alminum = pl.Almimun;
            connectedId = -1;
            PowerLevel = pl.PowerLevel;
            PlayerProperty.level = pl.level;
            PlayerProperty.gold = pl.gold;
            team.StartFomation = pl.StartFomation;
            team.AttackFormation = pl.AttackFormation;
            team.DefienceForation = pl.DefienceForation;
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
            DoneMissions = convertor.StringToShorttArray(pl.DoneMissions);
            mainMutex.ReleaseMutex();
        }


        
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
          //  plsrs.UsableFormations = convertor.IntArrayToSrting(team.UsableFormations);
            mainMutex.ReleaseMutex();
            return plsrs;
        }
        
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
            thisPlayerAtServer.level = PlayerProperty.level;
            thisPlayerAtServer.Almimun = PlayerProperty.Alminum;
            thisPlayerAtServer.Name = Name;
            thisPlayerAtServer.otherElixirs = convertor.IntArrayToSrting(convertor.listIntToIntArray(elixirOutOfTeam));
            thisPlayerAtServer.UnAtachedParts = convertor.IntArrayToSrting(convertor.listIntToIntArray(unAttachedPart));
            thisPlayerAtServer.outOfTeamPawns = convertor.LongIntArrayToSrting(convertor.listLongToLongArray(pawnOutOfTeam));
            thisPlayerAtServer.pawnsInBench = convertor.LongIntArrayToSrting(team.pawnsInBench);
            thisPlayerAtServer.PlayeingPawns = convertor.LongIntArrayToSrting(team.PlayeingPawns);
            thisPlayerAtServer.PowerLevel = PowerLevel;
            thisPlayerAtServer.gold = PlayerProperty.gold;
            for(int i =0; i <buildOrders.Length; i++) {
                BuildOrdersIntFormt[i] = -1;
                if(buildOrders[i] != null)
                {
                    BuildOrdersIntFormt[i] = buildOrders[i].ToCode();
                }
            }
            thisPlayerAtServer.buildOrders = convertor.LongIntArrayToSrting(BuildOrdersIntFormt);
            //  thisPlayerAtServer.UsableFormations = convertor.IntArrayToSrting(team.UsableFormations);            
            dataBase.Entry(thisPlayerAtServer).State = EntityState.Modified;
            dataBase.SaveChanges();
            mainMutex.ReleaseMutex();
        }

        public void AddTodDataBase()
        {
            mainMutex.WaitOne();
            PlayerForDatabase playerInfo = returnDataBaseVersion();
            dataBase.playerInfoes.Add(playerInfo);
            dataBase.SaveChanges();
            mainMutex.ReleaseMutex();
        }


        public TeamForConnectedPlayers ReturnYourTeam()
        {
            return team;
        }

        #region inner function
        #endregion
    }

   

}
