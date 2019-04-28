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
