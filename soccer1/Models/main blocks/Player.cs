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
            team = utilities.returnDefultTeam();
            PlayerProperty = new Property();
            PlayerProperty.fan = 0;
            PlayerProperty.coin = Statistics.StartingCoin;
            PlayerProperty.level = 1;
            PlayerProperty.SoccerSpetial = Statistics.StartingSS;
        }

        public string id { get; set; }

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

        public bool BuyAsset(AssetType assetType, string AssetIdName, Property price)
        {
            Utilities utilities = new Utilities();
            if (utilities.CheckIfFirstPropertyIsBigger(PlayerProperty, price)) {
                SubtractProperty(price);
                switch (assetType)
                {
                    case AssetType.Pawn:
                       // pawnOutOfTeam.Add()
                        break;


                }
                return true;
            }
            else{
                return false;
            }
        }
        
        public bool SubtractProperty(Property prop)
        {
            Utilities utilities = new Utilities();
            if (utilities.CheckIfFirstPropertyIsBigger(PlayerProperty, prop))
            {
                PlayerProperty.coin = -prop.coin;
                PlayerProperty.SoccerSpetial = -prop.SoccerSpetial;
                PlayerProperty.fan = -prop.fan;
                return true;
            }
            else
            {
                Errors.AddClientError("Not Enogh property");
                return false;
            }
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
            int[] pawnBuffer = new int[Statistics.MaxPawnOutOfTeam];
            for(int i=0; i< pawnBuffer.Length; i++) { pawnBuffer[i] = -1; }
            int[] elixirBuffer = new int[Statistics.MaxElixirOutOfTeam];
            for (int i = 0; i < elixirBuffer.Length; i++) { elixirBuffer[i] = -1; }
            int pawncounter = 0;
            int elixirCounter = 0;
            foreach(int i in pawnOutOfTeam)
            {
                pawnBuffer[pawncounter] = i;
                pawncounter++;
            }
            foreach (int i in elixirOutOfTeam)
            {
                elixirBuffer[elixirCounter] = i;
                elixirCounter++;
            }
            plsr.OutOfTeamPawns = convertor.IntArrayToSrting(pawnBuffer);
            plsr.OutOfTeamElixirs = convertor.IntArrayToSrting(elixirBuffer);
            return plsr;
        }
        #endregion

        public  void reWriteAccordingTo(PlayerForDatabase pl)
        {
           
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
            int[] pawnBuffer = convertor.SrtingTointArray(pl.otherPawns);
            pawnOutOfTeam.Clear();
            for (int i = 0; i < pawnBuffer.Length; i++) if (0 < pawnBuffer[i])
                {
                    pawnOutOfTeam.Add(pawnBuffer[i]);
                }
            int[] ElixirBuffer = convertor.SrtingTointArray(pl.otherElixirs);
            elixirOutOfTeam.Clear();
            for (int i = 0; i < ElixirBuffer.Length; i++) if (0 < ElixirBuffer[i])
                {
                    elixirOutOfTeam.Add(ElixirBuffer[i]);
                }

            team.PlayeingPawns = convertor.SrtingTointArray(pl.PlayeingPawns);
            team.pawnsInBench = convertor.SrtingTointArray(pl.pawnsInBench);
            team.UsableFormations = convertor.SrtingTointArray(pl.UsableFormations);
            
            team.ElixirInBench = convertor.SrtingTointArray(pl.ElixirInBench);
        }


        PlayerForDatabase returnDataBaseVersion()
        {
            PlayerForDatabase plsrs = new PlayerForDatabase();
            plsrs.CurrentFormation =team.CurrentFormation;
            plsrs.ElixirInBench = convertor.IntArrayToSrting(team.ElixirInBench);
            plsrs.Fan = PlayerProperty.fan;
            plsrs.id = id;
            plsrs.level = PlayerProperty.level;
            plsrs.Money = PlayerProperty.coin;
            plsrs.Name = Name;
            plsrs.otherElixirs = convertor.IntArrayToSrting(convertor.outOfTeamPawnToIntArray(elixirOutOfTeam));
            plsrs.otherPawns = convertor.IntArrayToSrting(convertor.outOfTeamPawnToIntArray(pawnOutOfTeam));
            plsrs.pawnsInBench = convertor.IntArrayToSrting(team.pawnsInBench);
            plsrs.PlayeingPawns = convertor.IntArrayToSrting(team.PlayeingPawns);
            plsrs.PowerLevel = PowerLevel;
            plsrs.SoccerSpetial = PlayerProperty.SoccerSpetial;
            plsrs.UsableFormations = convertor.IntArrayToSrting(team.UsableFormations);
            return plsrs;
        }

        public void SaveChanges()
        {
            DataDBContext dataBase = new DataDBContext();
            PlayerForDatabase playerInfo = returnDataBaseVersion();
            dataBase.Entry(playerInfo).State = EntityState.Modified;
            dataBase.SaveChanges();
        }

        public void AddTodDataBase()
        {
            DataDBContext dataBase = new DataDBContext();
            PlayerForDatabase playerInfo = returnDataBaseVersion();
            dataBase.playerInfoes.Add(playerInfo);
            dataBase.SaveChanges();
        }


        public TeamForConnectedPlayers ReturnYourTeam()
        {
            return team;
        }

        #region inner function
        #endregion
    }

   

}
