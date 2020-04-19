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

        public string DeviceID { get; set; }
        public int gold { get; set; }

        public string Name { get; set; }

        public int Almimun { get; set; }

        public int sponsorAlmimun { get; set; }

        public string sponsorName { get; set; }

        public int sponsorGold { get; set; }

        public int lastTimeSponsorAddDone { get; set; }
        public float PowerLevel { get; set; }

        public int level { get; set; }

        public int lastMatchId { get; set; }

        //public int SoccerSpetial { get; set; }

        public int StartFomation { get; set; }
        public int AttackFormation { get; set; }
        public int DefienceForation { get; set; }

        public string remaingPakages { get; set; }

        public long playPrehibititionFinishTime { get; set; }

        public long timeOfXpPakageExpiration { get; set; }

        public string outOfTeamPawns { get; set; }

        public string PlayeingPawns { get; set; }

        public string pawnsInBench { get; set; }
        
        public string UsableFormations { get; set; }

        public string otherElixirs { get; set; }

        public string ElixirInBench { get; set; }

        public string UnAtachedParts { get; set; }

        public string buildOrders { get; set; }

        public string DoneMissions { get; set; }

        /*
        public void changePlayer(PlayerForDatabase otherplayer)
        {

            gold = otherplayer.gold;
            Name = otherplayer.Name;
            Almimun = otherplayer.Almimun;
            PowerLevel = otherplayer.PowerLevel;
            level = otherplayer.level;
            lastMatchId = otherplayer.lastMatchId;
            gold = otherplayer.gold;
            StartFomation = otherplayer.StartFomation;
            AttackFormation = otherplayer.AttackFormation;
            DefienceForation = otherplayer.DefienceForation;
            outOfTeamPawns = otherplayer.outOfTeamPawns;
            PlayeingPawns = otherplayer.PlayeingPawns;
            pawnsInBench = otherplayer.pawnsInBench;
            UsableFormations = otherplayer.UsableFormations;
            UnAtachedParts = otherplayer.UnAtachedParts;
            otherElixirs = otherplayer.otherElixirs;
            ElixirInBench = otherplayer.ElixirInBench;
        }
        */

    }
    
}