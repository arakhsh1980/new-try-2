using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace soccer1.Models.main_blocks
{
    
    public class TeamForConnectedPlayers
    {
        
        public TeamForConnectedPlayers()
        {
            CurrentFormation = 1;
            PlayeingPawns =new int[Statistics.PlayingPawns];
            pawnsInBench = new int[Statistics.pawnsInBenchMax];
            UsableFormations = new int[Statistics.UsableFormationsMax];
            ElixirInBench = new int[Statistics.ElixirInBenchMax];
            for(int i=0; i< PlayeingPawns.Length; i++) { PlayeingPawns[i] = 1; }
            for (int i = 0; i < pawnsInBench.Length; i++) { pawnsInBench[i] = 1; }
            for (int i = 0; i < UsableFormations.Length; i++) { UsableFormations[i] = -1; }
            for (int i = 0; i < ElixirInBench.Length; i++) { ElixirInBench[i] = 1; }
        }


        public int CurrentFormation { get; set; }



        public int[] PlayeingPawns { get; set; }

        public int[] pawnsInBench { get; set; }

        public int[] UsableFormations { get; set; }

        public int[] ElixirInBench { get; set; }

        int UsableFormationsCounter = 0;
        public void AddToUsableFormations(int formationIndex)
        {
            UsableFormations[UsableFormationsCounter] = formationIndex;
            UsableFormationsCounter++;
        }
    }

    [Serializable]
    public struct TeamForSerialize
    {

        public string CurrentFormation;

        public string[] PlayeingPawns;

        public string[] pawnsInBench;

        public string[] UsableFormations;

        public string[] ElixirInBench;
    }

    [Serializable]
    public struct testtt
    {
        public string CurrentFormation;
    }


    [Serializable]
    public struct TeamForSerializeSingleString
    {

        public string CurrentFormation { get; set; }

        public string PlayeingPawns { get; set; }

        public string pawnsInBench { get; set; }

        public string UsableFormations { get; set; }

        public string ElixirInBench { get; set; }
    }



}