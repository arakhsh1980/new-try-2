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
            PlayerProperty.coin = Statistics.StartingCoin;
            PlayerProperty.level = 1;
            PlayerProperty.SoccerSpetial = Statistics.StartingSS;
        }

        private DataDBContext dataBase = new DataDBContext();

        public string id { get; set; }

        public static Mutex mainMutex = new Mutex();

        public static Mutex SaveChengesMutex = new Mutex();

        private string Name { get; set; }

        public int connectedId { get; set; }

        public float PowerLevel { get; set; }

        private TeamForConnectedPlayers team { get; set; }

        private Property PlayerProperty;
               
        private List<int> pawnOutOfTeam = new List<int>();

        private List<int> elixirOutOfTeam = new List<int>();

        private Convertors convertor = new Convertors();

        private bool isChanged=false;

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
            SaveChanges();
            
            
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
                       team.AddToUsableFormations(new AssetManager().ReturnAssetIndex(AssetType.Formation, AssetIdNum));
                        break;

                }
                SaveChanges();
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
                newPawn.pawnType = idNum;
                newPawn.playerPawnIndex = playerindex;
                newPawn.requiredXpForNextLevel = new AssetManager().ReturnrequiredXpForNextLevel(idNum);
                int NewPawnCode =new Convertors().PawnOfPlayerDataToPawnCode(newPawn);
                pawnOutOfTeam.Add(NewPawnCode);                
                SaveChanges();
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
                SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool SubtractProperty(Property prop)
        {
            Log.AddPlayerLog(id, "SubtractProperty : coin :" + prop.coin.ToString());
            Utilities utilities = new Utilities();
            if (utilities.CheckIfFirstPropertyIsBigger(PlayerProperty, prop))
            {
                PlayerProperty.coin -= prop.coin;
                PlayerProperty.SoccerSpetial -= prop.SoccerSpetial;
                PlayerProperty.fan -= prop.fan;
                SaveChanges();
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
            Log.AddPlayerLog(id, "AddProperty : coin :" + prop.coin.ToString());
            PlayerProperty.coin += prop.coin;
            PlayerProperty.SoccerSpetial += prop.SoccerSpetial;
            PlayerProperty.fan += prop.fan;
            PlayerProperty.level += prop.level;
            SaveChanges();
        }

        public bool ChangeTeam(TeamForConnectedPlayers newTeammm)
        {
            List<int> pawnBuffer = new List<int>();
            List<int> ElixirBuffer = new List<int>();
            foreach(int i in pawnOutOfTeam) { pawnBuffer.Add(i); }
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
                SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
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
            pp.pawnType = newPawnType;
            pp.requiredXpForNextLevel = new AssetManager().ReturnrequiredXpForNextLevel(newPawnType);
            int newPawnCode = new Convertors().PawnOfPlayerDataToPawnCode(pp);
            bool isFinded = false;
            bool isREmovedPastFromPawnOutOfTeam = false;

            isREmovedPastFromPawnOutOfTeam = pawnOutOfTeam.Remove( pawnCode);
            if (isREmovedPastFromPawnOutOfTeam)
            {
                pawnOutOfTeam.Add(newPawnCode);
                SaveChanges();
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
                    SaveChanges();
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
                    SaveChanges();
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
                SaveChanges();
                result = true;
            }

            return result;
        }

        public  void reWriteAccordingTo(PlayerForDatabase pl)
        {
            mainMutex.WaitOne();
            id = pl.id;
            PlayerProperty.fan = pl.Fan;
            Name = pl.Name;
            PlayerProperty.coin = pl.Money;
            connectedId = -1;
            PowerLevel = pl.PowerLevel;
            PlayerProperty.level = pl.level;
            PlayerProperty.SoccerSpetial = pl.SoccerSpetial;
            team.CurrentFormation = pl.CurrentFormation;
            //convert of Playerfordatabase Class to Player Class 
            //convert string to int
            int[] pawnBuffer = convertor.StringToIntArray(pl.otherPawns);
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

            team.PlayeingPawns = convertor.StringToIntArray(pl.PlayeingPawns);
            team.pawnsInBench = convertor.StringToIntArray(pl.pawnsInBench);
            team.UsableFormations = convertor.StringToIntArray(pl.UsableFormations);
            
            team.ElixirInBench = convertor.StringToIntArray(pl.ElixirInBench);
            mainMutex.ReleaseMutex();
        }


        
        private PlayerForDatabase returnDataBaseVersion()
        {
            mainMutex.WaitOne();
            PlayerForDatabase plsrs = new PlayerForDatabase();
            plsrs.CurrentFormation =team.CurrentFormation;
            plsrs.ElixirInBench = convertor.IntArrayToSrting(team.ElixirInBench);
            plsrs.Fan = PlayerProperty.fan;
            plsrs.id = id;
            plsrs.level = PlayerProperty.level;
            plsrs.Money = PlayerProperty.coin;
            plsrs.Name = Name;
            plsrs.otherElixirs = convertor.IntArrayToSrting(convertor.listIntToIntArray(elixirOutOfTeam));
            plsrs.otherPawns = convertor.IntArrayToSrting(convertor.listIntToIntArray(pawnOutOfTeam));
            plsrs.pawnsInBench = convertor.IntArrayToSrting(team.pawnsInBench);
            plsrs.PlayeingPawns = convertor.IntArrayToSrting(team.PlayeingPawns);
            plsrs.PowerLevel = PowerLevel;
            plsrs.SoccerSpetial = PlayerProperty.SoccerSpetial;
            plsrs.UsableFormations = convertor.IntArrayToSrting(team.UsableFormations);
            mainMutex.ReleaseMutex();
            return plsrs;
        }
        
        public void SaveChanges()
        {
            mainMutex.WaitOne();
            PlayerForDatabase thisPlayerAtServer =dataBase.playerInfoes.Find(id);
            thisPlayerAtServer.CurrentFormation = team.CurrentFormation;
            thisPlayerAtServer.ElixirInBench = convertor.IntArrayToSrting(team.ElixirInBench);
            thisPlayerAtServer.Fan = PlayerProperty.fan;
            //plsrs.id = id;
            thisPlayerAtServer.level = PlayerProperty.level;
            thisPlayerAtServer.Money = PlayerProperty.coin;
            thisPlayerAtServer.Name = Name;
            thisPlayerAtServer.otherElixirs = convertor.IntArrayToSrting(convertor.listIntToIntArray(elixirOutOfTeam));
            thisPlayerAtServer.otherPawns = convertor.IntArrayToSrting(convertor.listIntToIntArray(pawnOutOfTeam));
            thisPlayerAtServer.pawnsInBench = convertor.IntArrayToSrting(team.pawnsInBench);
            thisPlayerAtServer.PlayeingPawns = convertor.IntArrayToSrting(team.PlayeingPawns);
            thisPlayerAtServer.PowerLevel = PowerLevel;
            thisPlayerAtServer.SoccerSpetial = PlayerProperty.SoccerSpetial;
            thisPlayerAtServer.UsableFormations = convertor.IntArrayToSrting(team.UsableFormations);            
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
