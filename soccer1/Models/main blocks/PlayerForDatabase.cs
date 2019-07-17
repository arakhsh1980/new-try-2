using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using soccer1.Models.main_blocks;
using System.ComponentModel.DataAnnotations;

namespace soccer1.Models.main_blocks
{
    public class PlayerForDatabase
    {
        public PlayerForDatabase()
        {
            
        }

        [Key]
        public string id { get; set; }

        public int Fan { get; set; }

        public string Name { get; set; }

        public int Money { get; set; }

        public float PowerLevel { get; set; }

        public int level { get; set; }

        public int lastMatchId { get; set; }

        public int SoccerSpetial { get; set; }

        public int CurrentFormation { get; set; }

        public string otherPawns { get; set; }

        public string PlayeingPawns { get; set; }

        public string pawnsInBench { get; set; }
        
        public string UsableFormations { get; set; }

        public string otherElixirs { get; set; }

        public string ElixirInBench { get; set; }

        public void changePlayer(PlayerForDatabase otherplayer)
        {

            Fan = otherplayer.Fan;
            Name = otherplayer.Name;
            Money = otherplayer.Money;
            PowerLevel = otherplayer.PowerLevel;
            level = otherplayer.level;
            lastMatchId = otherplayer.lastMatchId;
            SoccerSpetial = otherplayer.SoccerSpetial;
            CurrentFormation = otherplayer.CurrentFormation;
            otherPawns = otherplayer.otherPawns;
            PlayeingPawns = otherplayer.PlayeingPawns;
            pawnsInBench = otherplayer.pawnsInBench;
            UsableFormations = otherplayer.UsableFormations;
            otherElixirs = otherplayer.otherElixirs;
            ElixirInBench = otherplayer.ElixirInBench;
        }

    }
    
}