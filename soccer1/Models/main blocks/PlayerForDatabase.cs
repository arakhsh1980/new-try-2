﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using soccer1.Models.main_blocks;
using soccer1.Models.utilites;
using System.ComponentModel.DataAnnotations;

namespace soccer1.Models.main_blocks
{
    public enum playerRequestType { tryToPlayOnline, startPlayOnLine, winOnlinePlay, playChalenge}
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

        public  int lastTimeUpdated { get; set; }
        public float NumberOfTickets { get; set; }

        public string sponsorName { get; set; }

        public float totalXp { get; set; }

        public int Tropy { get; set; }

        public string HistoryCode { get; set; }

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

        public string buyedBluePrints { get; set; }

        public string pawnsInBench { get; set; }
        
        public string UsableFormations { get; set; }

        public string otherElixirs { get; set; }

        public string ElixirInBench { get; set; }

        public string UnAtachedParts { get; set; }

        public string buildOrders { get; set; }

        public string DoneMissions { get; set; }

        public string GoalMemory0 { get; set; }
        public string GoalMemory1 { get; set; }
        public string GoalMemory2 { get; set; }
        public string GoalMemory3 { get; set; }
        public string GoalMemory4 { get; set; }
        public int LastfilledGoalMemory { get; set; }
        public int LastConnectionTime { get; set; }
        public string requestMemory { get; set; }

        public void ChangesAcoordingTo(PlayerForConnectedPlayer pl)
        {
            Convertors convertor = new Convertors();
            AttackFormation = pl.team.AttackFormation;
            StartFomation = pl.team.StartFomation;
            DefienceForation = pl.team.DefienceForation;
            ElixirInBench = convertor.IntArrayToSrting(pl.team.ElixirInBench);
            gold = pl.PlayerProperty.gold;
            //plsrs.id = id;
            Tropy = pl.PlayerProperty.tropy;
            Almimun = pl.PlayerProperty.Alminum;
            Name = pl.Name;
            otherElixirs = convertor.IntArrayToSrting(convertor.listIntToIntArray(pl.elixirOutOfTeam));
            UnAtachedParts = convertor.IntArrayToSrting(convertor.listIntToIntArray(pl.unAttachedPart));
            outOfTeamPawns = convertor.LongIntArrayToSrting(convertor.listLongToLongArray(pl.pawnOutOfTeam));
            pawnsInBench = convertor.LongIntArrayToSrting(pl.team.pawnsInBench);
            PlayeingPawns = convertor.LongIntArrayToSrting(pl.team.PlayeingPawns);
            totalXp = pl.totalXp;
            gold = pl.PlayerProperty.gold;
            sponsorName = pl.sponsorName;
            for (int i = 0; i < pl.buildOrders.Length; i++)
            {
                pl.BuildOrdersIntFormt[i] = -1;
                if (pl.buildOrders[i] != null)
                {
                    pl.BuildOrdersIntFormt[i] = pl.buildOrders[i].ToCode();
                }
            }
            DoneMissions = convertor.ShortListToSrting(pl.DoneMissions);
            buildOrders = convertor.LongIntArrayToSrting(pl.BuildOrdersIntFormt);
            remaingPakages = convertor.ShortArrayToSrting(pl.RemaingPakages);
            buyedBluePrints = convertor.ShortListToSrting(pl.BuyedBluePrints);
            timeOfXpPakageExpiration = pl.TimeOfXpPakageExpiartion;
            NumberOfTickets = pl.NumberOfTickets;
            lastTimeUpdated = pl.lastTimeUpdated;

        }

        public void AddGoalMemory(string memory)
        {
            LastfilledGoalMemory++;
            if (4 < LastfilledGoalMemory)
            {
                LastfilledGoalMemory = 0;
            }
            switch (LastfilledGoalMemory)
            {
                case 0:
                    GoalMemory0 = memory;
                    break;
                case 1:
                    GoalMemory1 = memory;
                    break;
                case 2:
                    GoalMemory2 = memory;
                    break;
                case 3:
                    GoalMemory3 = memory;
                    break;
                case 4:
                    GoalMemory4 = memory;
                    break;
                default:
                    break;
            }
        }

        public void AddRequestToHistory(playerRequestType type)
        {
            int typeint = (int)type;
            int code = (new Utilities().TimePointofNow()- LastConnectionTime) *10+ typeint;
            LastConnectionTime = new Utilities().TimePointofNow();
            requestMemory += "|" + code.ToString();
        }

    }

}