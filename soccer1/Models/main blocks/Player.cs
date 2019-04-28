using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using soccer1.Models.main_blocks;
using System.ComponentModel.DataAnnotations;
using soccer1.Models.utilites;



namespace soccer1.Models.main_blocks
{
    
       
    public class PlayerForConnectedPlayer
    {

        public PlayerForConnectedPlayer()
            {
                connectedId = 0;
                Name = "uuuuuu";
                PowerLevel = 1.0f;
                team = new TeamForConnectedPlayers();
                PlayerProperty = new Property();
            }

           
        public string id { get; set; }
                
        public string Name { get; set; }
        
        public int connectedId { get; set; }

        public float PowerLevel { get; set; }                       
        
        public TeamForConnectedPlayers team { get; set; }

        public Property PlayerProperty;

        public int pawnOutOfTeamCounter =0;
        public int elixirOutOfTeamCounter = 0;

        public int[] pawnOutOfTeam = new int[Statistics.MaxPawnOutOfTeam];

        public int[] elixirOutOfTeam = new int[Statistics.MaxElixirOutOfTeam];


        #region public functions

        public bool BuyAsset(AssetType assetType, string AssetIdName, Property price)
        {
            
            if(utilities.CheckIfFirstPropertyIsBigger(PlayerProperty, price)) {
                SubtractProperty(price);
                switch (assetType)
                {
                    case AssetType.Pawn:
                        pawnOutOfTeam[pawnOutOfTeamCounter] = AssetManager.ReturnAssetIndex(AssetType.Pawn, AssetIdName);
                        pawnOutOfTeamCounter++;
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
            List<int> allPawns = new List<int>();
            List<int> allElixirs = new List<int>();
            allPawns.Clear();
            allElixirs.Clear();
            for(int i=0; i< team.PlayeingPawns.Length; i++)
            {
                allPawns.Add(team.PlayeingPawns[i]);
            }
            for (int i = 0; i < team.pawnsInBench.Length; i++)
            {
                allPawns.Add(team.pawnsInBench[i]);
            }
            for (int i = 0; i < pawnOutOfTeam.Length; i++)
            {
                allPawns.Add(pawnOutOfTeam[i]);
            }

            for (int i = 0; i < newTeammm.PlayeingPawns.Length; i++)
            {
                if (allPawns.Exists(p => p== newTeammm.PlayeingPawns[i]))
                {
                    allPawns.Remove(newTeammm.PlayeingPawns[i]);
                }
                else
                {
                    return false;
                }                
            }

            for (int i = 0; i < newTeammm.pawnsInBench.Length; i++)
            {
                if (allPawns.Exists(p => p == newTeammm.pawnsInBench[i]))
                {
                    allPawns.Remove(newTeammm.pawnsInBench[i]);
                }
                else
                {
                    return false;
                }
            }
            pawnOutOfTeamCounter = 0;
            foreach(int p in allPawns)
            {
                pawnOutOfTeam[pawnOutOfTeamCounter] = p;
                pawnOutOfTeamCounter++;
            }


            
            for (int i = 0; i < team.ElixirInBench.Length; i++)
            {
                allElixirs.Add(team.ElixirInBench[i]);
            }
            for (int i = 0; i < elixirOutOfTeam.Length; i++)
            {
                allElixirs.Add(elixirOutOfTeam[i]);
            }

            for (int i = 0; i < newTeammm.ElixirInBench.Length; i++)
            {
                if (allElixirs.Exists(p => p == newTeammm.ElixirInBench[i]))
                {
                    allElixirs.Remove(newTeammm.ElixirInBench[i]);
                }
                else
                {
                    return false;
                }
            }

            elixirOutOfTeamCounter = 0;
            foreach (int p in allElixirs)
            {
                elixirOutOfTeam[elixirOutOfTeamCounter] = p;
                elixirOutOfTeamCounter++;
            }

            team = newTeammm;
            return true;
        }
        #endregion

        #region inner function
        #endregion
    }

    /*
        public class ReadOnlyPlayer
        {

            public Player()
            {
                connectedId = 0;
                Name = "uuuuuu";
                PowerLevel = 1.0f;
            }


            public string id { get; set; }

            public string Name { get; set; }

            public int connectedId { get; set; }

            public float PowerLevel { get; set; }

            public Team team { get; set; }

            public Property property { get; set; }

            public int[] pawnOutOfTeam = new int[Statistics.MaxPawnOutOfTeam];

            public int[] elixirOutOfTeam = new int[Statistics.MaxElixirOutOfTeam];

        }
        */

}
