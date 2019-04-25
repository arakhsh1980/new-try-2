using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using soccer1.Models.main_blocks;
using System.ComponentModel.DataAnnotations;



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
                property = new Property();
            }

           
        public string id { get; set; }
                
        public string Name { get; set; }
        
        public int connectedId { get; set; }

        public float PowerLevel { get; set; }                       
        
        public TeamForConnectedPlayers team { get; set; }

        public Property property;

        public int pawnOutOfTeamCounter =0;
        public int elixirOutOfTeamCounter = 0;

        public int[] pawnOutOfTeam = new int[Statistics.MaxPawnOutOfTeam];

        public int[] elixirOutOfTeam = new int[Statistics.MaxElixirOutOfTeam];
        public void  AddPawnOutOfTeam(int pawnindex) {
            
        }
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
