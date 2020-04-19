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
            StartFomation = 1;
            AttackFormation = 1;
            DefienceForation = 1;
            PlayeingPawns =new long[Statistics.PlayingPawns];
            pawnsInBench = new long[Statistics.pawnsInBenchMax];
           // UsableFormations = new int[Statistics.UsableFormationsMax];
            ElixirInBench = new int[Statistics.ElixirInBenchMax];
            for(int i=0; i< PlayeingPawns.Length; i++) { PlayeingPawns[i] = 1; }
            for (int i = 0; i < pawnsInBench.Length; i++) { pawnsInBench[i] = 1; }
           // for (int i = 0; i < UsableFormations.Length; i++) { UsableFormations[i] = -1; }
            for (int i = 0; i < ElixirInBench.Length; i++) { ElixirInBench[i] = 1; }
        }


        public int StartFomation { get; set; }

        public int AttackFormation { get; set; }
        public int DefienceForation { get; set; }



        public long[] PlayeingPawns { get; set; }

        //public int[] PlayeingPawnsRequiredXp { get; set; }

        public long[] pawnsInBench { get; set; }

        //public int[] pawnsInBenchRequiredXp { get; set; }

       // public int[] UsableFormations { get; set; }

        public int[] ElixirInBench { get; set; }

        int UsableFormationsCounter = 0;
        //public void AddToUsableFormations(int formationIndex)
        //{
        //    UsableFormations[UsableFormationsCounter] = formationIndex;
        //    UsableFormationsCounter++;
        //}

        public int AddXpToPawn(int pawnAssinedIndex, int xpVal)
        {
            int total = 0;
            for (int i = 0; i < PlayeingPawns.Length; i++)
            {
                PawnOfPlayerData pawnn = new Convertors().PawnCodeToPawnOfPlayerData(PlayeingPawns[i]);
                if(pawnn.playerPawnIndex == pawnAssinedIndex)
                {
                    if (pawnn.requiredXpForNextLevel < xpVal) {
                        total += pawnn.requiredXpForNextLevel;
                        pawnn.requiredXpForNextLevel = 0;
                        
                    }
                    else
                    {
                        total += xpVal;
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
                    if (pawnn.requiredXpForNextLevel < xpVal) {
                        total += pawnn.requiredXpForNextLevel;
                        pawnn.requiredXpForNextLevel = 0;
                    }
                    else
                    {
                        total += xpVal;
                        pawnn.requiredXpForNextLevel -= xpVal;
                    }
                    pawnsInBench[i] = new Convertors().PawnOfPlayerDataToPawnCode(pawnn);
                }
            }
            return total;
        }

        public int AddXpToTeam( int xpVal)
        {
            int totoal = 0;
            for (int i = 0; i < PlayeingPawns.Length; i++)
            {
                PawnOfPlayerData pawnn = new Convertors().PawnCodeToPawnOfPlayerData(PlayeingPawns[i]);
                
                {
                    if (pawnn.requiredXpForNextLevel < xpVal) {
                        totoal += pawnn.requiredXpForNextLevel;
                        pawnn.requiredXpForNextLevel = 0;
                        
                    }
                    else
                    {
                        pawnn.requiredXpForNextLevel -= xpVal;
                        totoal += xpVal;
                    }
                    PlayeingPawns[i] = new Convertors().PawnOfPlayerDataToPawnCode(pawnn);
                }
            }
            for (int i = 0; i < pawnsInBench.Length; i++)
            {
                PawnOfPlayerData pawnn = new Convertors().PawnCodeToPawnOfPlayerData(pawnsInBench[i]);
                
                {
                    if (pawnn.requiredXpForNextLevel < xpVal) {
                        totoal += pawnn.requiredXpForNextLevel;
                        pawnn.requiredXpForNextLevel = 0;
                        
                    }
                    else
                    {
                        pawnn.requiredXpForNextLevel -= xpVal;
                        totoal += xpVal;
                    }
                    pawnsInBench[i] = new Convertors().PawnOfPlayerDataToPawnCode(pawnn);
                }
            }
            return totoal;
        }
    }

    [Serializable]
    public struct TeamForSerialize
    {
      
        public int StartFomation;
        public int AttackFormation;
        public int DefienceForation;

        public long[] PlayeingPawns;

        public long[] pawnsInBench;

        //public int[] UsableFormations;

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
        

        public string StartFomation { get; set; }
        public string AttackFormation { get; set; }
        public string DefienceForation { get; set; }

        public string PlayeingPawns { get; set; }

        public string pawnsInBench { get; set; }

       // public string UsableFormations { get; set; }

        public string ElixirInBench { get; set; }
    }



}