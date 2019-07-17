using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models.utilites;

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

        public int[] PlayeingPawnsRequiredXp { get; set; }

        public int[] pawnsInBench { get; set; }

        public int[] pawnsInBenchRequiredXp { get; set; }

        public int[] UsableFormations { get; set; }

        public int[] ElixirInBench { get; set; }

        int UsableFormationsCounter = 0;
        public void AddToUsableFormations(int formationIndex)
        {
            UsableFormations[UsableFormationsCounter] = formationIndex;
            UsableFormationsCounter++;
        }

        public void AddXpToPawn(int pawnAssinedIndex, int xpVal)
        {
            for (int i = 0; i < PlayeingPawns.Length; i++)
            {
                PawnOfPlayerData pawnn = new Convertors().PawnCodeToPawnOfPlayerData(PlayeingPawns[i]);
                if(pawnn.playerPawnIndex == pawnAssinedIndex)
                {
                    if (pawnn.requiredXpForNextLevel < xpVal) { pawnn.requiredXpForNextLevel = 0; }
                    else
                    {
                        pawnn.requiredXpForNextLevel -= xpVal;
                    }
                    PlayeingPawns[i] =  new Convertors().PawnOfPlayerDataToPawnCode(pawnn);
                    Log.AddLog("AddXpToPawn"+ pawnAssinedIndex.ToString());
                }
            }
            for (int i = 0; i < pawnsInBench.Length; i++)
            {
                PawnOfPlayerData pawnn = new Convertors().PawnCodeToPawnOfPlayerData(pawnsInBench[i]);
                if (pawnn.playerPawnIndex == pawnAssinedIndex)
                {
                    if (pawnn.requiredXpForNextLevel < xpVal) { pawnn.requiredXpForNextLevel = 0; }
                    else
                    {
                        pawnn.requiredXpForNextLevel -= xpVal;
                    }
                    pawnsInBench[i] = new Convertors().PawnOfPlayerDataToPawnCode(pawnn);
                }
            }
        }

        public void AddXpToTeam( int xpVal)
        {
            for (int i = 0; i < PlayeingPawns.Length; i++)
            {
                PawnOfPlayerData pawnn = new Convertors().PawnCodeToPawnOfPlayerData(PlayeingPawns[i]);
                
                {
                    if (pawnn.requiredXpForNextLevel < xpVal) { pawnn.requiredXpForNextLevel = 0; }
                    else
                    {
                        pawnn.requiredXpForNextLevel -= xpVal;
                    }
                    PlayeingPawns[i] = new Convertors().PawnOfPlayerDataToPawnCode(pawnn);
                }
            }
            for (int i = 0; i < pawnsInBench.Length; i++)
            {
                PawnOfPlayerData pawnn = new Convertors().PawnCodeToPawnOfPlayerData(pawnsInBench[i]);
                
                {
                    if (pawnn.requiredXpForNextLevel < xpVal) { pawnn.requiredXpForNextLevel = 0; }
                    else
                    {
                        pawnn.requiredXpForNextLevel -= xpVal;
                    }
                    pawnsInBench[i] = new Convertors().PawnOfPlayerDataToPawnCode(pawnn);
                }
            }
        }
    }

    [Serializable]
    public struct TeamForSerialize
    {

        public int CurrentFormation;

        public int[] PlayeingPawns;

        public int[] pawnsInBench;

        public int[] UsableFormations;

        public int[] ElixirInBench;
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