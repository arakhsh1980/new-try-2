using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Controllers;
using System.Web.Mvc;
using soccer1.Models.main_blocks;
using System.Web.Script.Serialization;


using soccer1.Models;

namespace soccer1.Models.utilites
{
    public class Convertors
    {
        const short pCMX = 260;
        public PawnOfPlayerData PawnCodeToPawnOfPlayerData(long p)
        {
            if (p < 0) { Errors.AddBigError("Convertors.PawnCodeToPawnOfPlayerData. error in pawn code"); }
            PawnOfPlayerData pp = new PawnOfPlayerData();            
            long code =  p;
            // code += 9000000000000000000;
            //long code = cod;
            long factor = (long)1000 * pCMX * pCMX * pCMX * pCMX * pCMX * 100;
            pp.playerPawnIndex = (int)(code / factor);
            code = code % factor;
            factor = (long)1000 * pCMX * pCMX * pCMX * pCMX * 100;
            int shooterCode = (int)(code / factor);
            pp.parts[4] = shooterCode;
            code = code % factor;
            factor = (long)1000 * pCMX * pCMX * pCMX * 100;
            int shildCode = (int)(code / factor);
            pp.parts[3] = shildCode;
            code = code % factor;
            factor = (long)1000 * pCMX * pCMX * 100;
            int EnginCode = (int)(code / factor);
            pp.parts[2] = EnginCode;
            code = code % factor;
            factor = (long)1000 * pCMX * 100;
            int battryCode = (int)(code / factor);
            pp.parts[1] = battryCode;
            code = code % factor;
            factor = (long)1000 * 100;
            int aimerCode = (int)(code / factor);
            pp.parts[0] = aimerCode;
            code = code % factor;
            factor = (long)1000;
            pp.baceTypeIndex = (short)(code / factor);
            code = code % factor;
            pp.requiredXpForNextLevel = (int)(code);
            return pp;
        }

        public long PawnOfPlayerDataToPawnCode(PawnOfPlayerData pp)
        {
            if (11 < pp.playerPawnIndex)
            {
                //Debug.Log("erooor . decoding will show wrong results");
            }
            long code = 0;
            // code += -9000000000000000000;
            code += (long)pp.requiredXpForNextLevel;
            code += (long)pp.baceTypeIndex * 1000;
            code += (long)pp.parts[0] * 1000*100;
            code += (long)pp.parts[1] * 1000 * 100 * pCMX;
            code += (long)pp.parts[2] * 1000 * 100 * pCMX * pCMX;
            code += (long)pp.parts[3] * 1000 * 100 * pCMX * pCMX * pCMX;
            code += (long)pp.parts[4] * 1000 * 100 * pCMX * pCMX * pCMX * pCMX;
            code += (long)pp.playerPawnIndex * 1000 * 100 * pCMX * pCMX * pCMX * pCMX * pCMX;
            return code;            
        }

        public int GetTypePartOfPawn(int p) {
            if (p < 0) { Errors.AddBigError("Convertors.GetTypePartOfPawn. error in pawn code"); return -1; }
            return (int)Math.Floor((p * 1.0f) / 1000000.0f);
        }

        public int GetPlayerIndexPartOfPawn(int p)
        {
            if (p < 0) { Errors.AddBigError("Convertors.GetTypePartOfPawn. error in pawn code"); return -1; }

            return (int)Math.Floor((p * 1.0f) / 10000.0f) - (GetTypePartOfPawn(p) * 100);
        }

        public int GetRequiredXpPartOfPawn(int p)
        {
            if (p < 0) { Errors.AddBigError("Convertors.GetTypePartOfPawn. error in pawn code"); return -1; }

            return p- (GetTypePartOfPawn(p) * 100) - (GetTypePartOfPawn(p) * 10000);
        }

        public  ShootActionCode FormToShoot(FormCollection collection)
        {
            ShootActionCode shoot = new ShootActionCode();
            /*
    

            
            

            shoot.playerID = Int32.Parse(collection["ConnectionId"]);
            shoot.MatchID = Int32.Parse(collection["ConnectionId"]);
            shoot.ShooterMathNumber = Int32.Parse(collection["ConnectionId"]);
            shoot.pawnNumber = Int32.Parse(collection["ConnectionId"]);
            shoot.power = Int32.Parse(collection["ConnectionId"]);

            shoot.playerID = Int32.Parse(collection["ConnectionId"]);
            */
            return shoot;
            /*
            shoot.MatchID = Int32.Parse(Request.Form["MatchID"]);
            shoot.ShooterMathNumber = Int32.Parse(Request.Form["ShooterMathNumber"]);
            shoot.playerID = Int32.Parse(Request.Form["ConnectionId"]);
            shoot.pawnNumber = Int32.Parse(Request.Form["ConnectionId"]);
            shoot.power = Int32.Parse(Request.Form["ConnectionId"]);
            shoot.rotationPower = Int32.Parse(Request.Form["ConnectionId"]);
            shoot.XDirection = Int32.Parse(Request.Form["ConnectionId"]);
            shoot.YDirection = Int32.Parse(Request.Form["ConnectionId"]);
            */
        }

        public  TeamForSerialize TeamToTeamForSerialize(TeamForConnectedPlayers team)
        {
            TeamForSerialize slteam = new TeamForSerialize();
            slteam.StartFomation = new AssetManager().ReturnAssetName(AssetType.Formation,  team.StartFomation);
            slteam.AttackFormation = new AssetManager().ReturnAssetName(AssetType.Formation, team.AttackFormation);
            slteam.DefienceForation = new AssetManager().ReturnAssetName(AssetType.Formation, team.DefienceForation);
            slteam.ElixirInBench = new int[team.ElixirInBench.Length];
            for (int i=0; i< team.ElixirInBench.Length; i++) { slteam.ElixirInBench[i] = new AssetManager().ReturnAssetName(AssetType.Elixir, team.ElixirInBench[i]); }

            slteam.pawnsInBench = new long[team.pawnsInBench.Length];
            for (int i = 0; i < team.pawnsInBench.Length; i++) { slteam.pawnsInBench[i] =  team.pawnsInBench[i]; }

            slteam.PlayeingPawns = new long[team.PlayeingPawns.Length];
            for (int i = 0; i < team.PlayeingPawns.Length; i++) { slteam.PlayeingPawns[i] = team.PlayeingPawns[i]; }

            
            //int counter = 0;
            //for (int i = 0; i < team.UsableFormations.Length; i++) if (0 < team.UsableFormations[i]) { counter++; }
            //slteam.UsableFormations = new int[counter];
            //int counter2 = 0;
            //for (int i = 0; i < team.UsableFormations.Length; i++) if (0 < team.UsableFormations[i]) {
            //        slteam.UsableFormations[counter2] = new AssetManager().ReturnAssetName(AssetType.Formation, team.UsableFormations[i]);
            //        counter2++;
            //    }
        
            return slteam;
        }

        /*
        private string[] AssetIntStringToNames(string[] inputString, AssetType type)
        {
            
            string[] st = new string[inputString.Length];
            int cash = 0;
            for (int i = 0; i < st.Length; i++)
            {
                cash = Int32.Parse(inputString[i]);
                st[i] = new AssetManager().ReturnAssetName(type, cash);
            }
            return st;
        }

            */
/*
        public PlayerForSerial PForDatabaseToPForSerial(PlayerForDatabase player)
        {
            PlayerForSerial serialplayer = new PlayerForSerial();
            serialplayer.id = player.id;
            serialplayer.Gold = player.gold;
            serialplayer.level = player.level;
            serialplayer.Alminum = player.Almimun;
            serialplayer.Name = player.Name;
            //serialplayer.SoccerSpetial = player.gold;
            serialplayer.OutOfTeamElixirs = StringToIntArray( player.otherElixirs);
            serialplayer.UnAttachedPart = StringToIntArray(player.UnAtachedParts);
            serialplayer.OutOfTeamPawns = StringToLongIntArray(player.outOfTeamPawns);
            //serialplayer.OutOfTeamPawnsRequiredXp = SrtingTointArray(player.otherPawnsRequiredXp);
            serialplayer.PowerLevel = player.PowerLevel;
           // serialplayer.SoccerSpetial = player.SoccerSpetial;
            serialplayer.Team.StartFomation = new AssetManager().ReturnAssetName(AssetType.Formation, player.StartFomation  );
            serialplayer.Team.AttackFormation = new AssetManager().ReturnAssetName(AssetType.Formation, player.AttackFormation);
            serialplayer.Team.DefienceForation = new AssetManager().ReturnAssetName(AssetType.Formation, player.DefienceForation);
            serialplayer.Team.ElixirInBench = StringToIntArray(player.ElixirInBench);
            serialplayer.Team.pawnsInBench = StringToLongIntArray(player.pawnsInBench);
            //serialplayer.PawnsinBenchRequiredXp = SrtingTointArray(player.pawnsInBenchRequiredXp);
            serialplayer.Team.PlayeingPawns = StringToLongIntArray(player.PlayeingPawns);
            //serialplayer.PlayingPawnsRequiredXp = SrtingTointArray(player.PlayeingPawnsRequiredXp);
           // serialplayer.Team.UsableFormations = StringToIntArray(player.UsableFormations);
            serialplayer.UsebaleFormations = StringToIntArray(player.UsableFormations);
            serialplayer.DoneMissions = StringToShorttArray(player.DoneMissions);
            serialplayer.buildOrders = StringToLongIntArray(player.buildOrders);
            serialplayer.lastTimeSponsorAddDone = player.lastTimeSponsorAddDone;
            serialplayer. = player.lastTimeSponsorAddDone;
            return serialplayer;
        }
        */
        public  TeamForConnectedPlayers TeamForSerializeToTeam(TeamForSerialize pl)
        {
            //convert of Teamforserialize Class to Team Class 
            //convert string to int
            TeamForConnectedPlayers plsrs = new TeamForConnectedPlayers();
           
            for (int i = 0; i < pl.PlayeingPawns.Length; i++) { plsrs.PlayeingPawns[i] =    pl.PlayeingPawns[i]; }
            for (int i = 0; i < pl.pawnsInBench.Length; i++) { plsrs.pawnsInBench[i] = pl.pawnsInBench[i]; }
            int usableFormationCounter = 0;
            //for (int i = 0; i < pl.UsableFormations.Length; i++)
            //{
            //    if (-1<pl.UsableFormations[i] )
            //    {
            //        plsrs.UsableFormations[usableFormationCounter] = new AssetManager().ReturnAssetIndex(AssetType.Formation, pl.UsableFormations[i]);
            //        usableFormationCounter++;
            //    }
            //}
            for (int i = 0; i < pl.ElixirInBench.Length; i++) { plsrs.ElixirInBench[i] = new AssetManager().ReturnAssetIndex(AssetType.Elixir, pl.ElixirInBench[i]); }

            plsrs.StartFomation = new AssetManager().ReturnAssetIndex(AssetType.Formation, pl.StartFomation);
            plsrs.AttackFormation = new AssetManager().ReturnAssetIndex(AssetType.Formation, pl.AttackFormation);
            plsrs.DefienceForation = new AssetManager().ReturnAssetIndex(AssetType.Formation, pl.DefienceForation);
            return plsrs;
        }

        public  string IntArrayToSrting(int[] ar)
        {
            string uu;
            uu = new JavaScriptSerializer().Serialize(ar);
            return uu;
        }
        public string LongIntArrayToSrting(long[] ar)
        {
            string uu;
            uu = new JavaScriptSerializer().Serialize(ar);
            return uu;
        }

        public string ShortArrayToSrting(short[] ar)
        {
            string uu;
            uu = new JavaScriptSerializer().Serialize(ar);
            return uu;
        }

        public string ShortListToSrting(List<short> ar)
        {
            string uu;
            if(ar == null)
            {
                uu = "";
                return uu;
            }
            if(ar.Count == 0)
            {
                uu = "";
            }
            else
            {
                uu = new JavaScriptSerializer().Serialize(ar);
            }
            
            return uu;
        }

        public string StringArrayToSrting(string[] ar)
        {
            string uu;
            uu = new JavaScriptSerializer().Serialize(ar);
            return uu;
        }

        public  string ListToString(List<int> list)
        {
            string uu;
            uu = new JavaScriptSerializer().Serialize(list);
            return uu;
        }

        public   int[] listIntToIntArray(List<int> list)
        {
            int[] buffer = new int[Statistics.MaxPawnOutOfTeam];
            for(int i =0; i< buffer.Length; i++)
            {
                buffer[i] = -1;
            }
            int counter = 0;
            foreach (int i in list)
            {
                if (counter < buffer.Length)
                {
                    buffer[counter] = i;
                    counter++;
                }
                else
                {
                    Errors.AddBigError("Eroor - convertors.outOfTeamPawnToIntArray: out of reng");
                }
            }
            return buffer;
        }

        public long[] listLongToLongArray(List<long> list)
        {
            long[] buffer = new long[Statistics.MaxPawnOutOfTeam];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = -1;
            }
            long counter = 0;
            foreach (long longnum in list)
            {
                if (counter < buffer.Length)
                {
                    buffer[counter] = longnum;
                    counter++;
                }
                else
                {
                    Errors.AddBigError("Eroor - convertors.outOfTeamPawnToIntArray: out of reng");
                }
            }
            return buffer;
        }

        public int[] outOfTeamElixirToIntArray(List<int> list)
        {
            int[] buffer = new int[Statistics.MaxElixirOutOfTeam];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = -1;
            }
            int counter = 0;
            foreach (int i in list)
            {
                if (counter < buffer.Length)
                {
                    buffer[counter] = i;
                    counter++;
                }
                else
                {
                    Errors.AddBigError("Eroor - convertors.outOfTeamPawnToIntArray: out of reng");
                }
            }
            return buffer;
        }


        public  int[] StringToIntArray(string ar)
        {
            int[] kk = new JavaScriptSerializer().Deserialize<int[]>(ar);
            return kk;
        }

        public short[] StringToShortArray(string ar)
        {
            short[] kk = new JavaScriptSerializer().Deserialize<short[]>(ar);
            return kk;
        }

        public short[] StringToShorttArray(string ar)
        {
            short[] kk = new JavaScriptSerializer().Deserialize<short[]>(ar);
            return kk;
        }

        public List<short> StringToShortList(string ar)
        {
            List<short> kk= new List<short>();
            if (0 < ar.Length)
            {
                kk = new JavaScriptSerializer().Deserialize<List<short>>(ar);
                return kk;
            }
            else
            {                
                return kk;
            }
            
        }

        public long[] StringToLongIntArray(string ar)
        {
            long[] kk = new JavaScriptSerializer().Deserialize<long[]>(ar);
            return kk;
        }

        public string[] StringToStringArray(string ar)
        {
            string[] ss = new JavaScriptSerializer().Deserialize<string[]>(ar);
            return ss;
        }

       


        public  TeamForConnectedPlayers JsonToTeam(string teamJson)
        {
            TeamForConnectedPlayers kk = new JavaScriptSerializer().Deserialize<TeamForConnectedPlayers>(teamJson);
            return kk;
        }

        /*
        public static PlayerForSerial PlayerToPlayrSerial(PlayerForConnectedPlayer pl)
        {
            PlayerForSerial plsr = new PlayerForSerial();
            plsr.id = pl.id;            
            plsr.Fan = pl.PlayerProperty.fan;          
            plsr.Name = pl.Name;
            plsr.Money = pl.PlayerProperty.coin;
            plsr.CoonId = pl.connectedId;
            plsr.PowerLevel = pl.PowerLevel;
            plsr.level = pl.PlayerProperty.level;
            plsr.SoccerSpetial = pl.PlayerProperty.SoccerSpetial;
            plsr.Team = TeamToTeamForSerialize(pl.team);            
            plsr.OutOfTeamPawns = IntArrayToSrting(pl.pawnOutOfTeam);
            plsr.OutOfTeamElixirs = IntArrayToSrting(pl.elixirOutOfTeam);
            return plsr;
         }
         

        public static PlayerForConnectedPlayer PlayerForDatabaseToPlayer(PlayerForDatabase pl)
        {
            PlayerForConnectedPlayer plsrs = new PlayerForConnectedPlayer();
            plsrs.id = pl.id;
            plsrs.PlayerProperty.fan = pl.Fan;
            plsrs.Name = pl.Name;
            plsrs.PlayerProperty.coin = pl.Money;
            plsrs.connectedId = -1;
            plsrs.PowerLevel = pl.PowerLevel;
            plsrs.PlayerProperty.level = pl.level;
            plsrs.PlayerProperty.SoccerSpetial = pl.SoccerSpetial;
            plsrs.team.CurrentFormation = pl.CurrentFormation;
            //convert of Playerfordatabase Class to Player Class 
            //convert string to int
            plsrs.pawnOutOfTeam = SrtingTointArray(pl.otherPawns);
            plsrs.team.PlayeingPawns = SrtingTointArray(pl.PlayeingPawns); 
            plsrs.team.pawnsInBench = SrtingTointArray(pl.pawnsInBench);
            plsrs.team.UsableFormations = SrtingTointArray(pl.UsableFormations); 
            plsrs.elixirOutOfTeam = SrtingTointArray(pl.otherElixirs); 
            plsrs.team.ElixirInBench = SrtingTointArray(pl.ElixirInBench); 
            return plsrs;
        }
        */

            /*
        public static PlayerForDatabase PlayerToPlayerForDatabase(PlayerForConnectedPlayer pl)
        {
            PlayerForDatabase plsrs = new PlayerForDatabase();
            plsrs.CurrentFormation = pl.team.CurrentFormation;
            plsrs.ElixirInBench = IntArrayToSrting(pl.team.ElixirInBench);
            plsrs.Fan = pl.PlayerProperty.fan;
            plsrs.id = pl.id;
            plsrs.level = pl.PlayerProperty.level;
            plsrs.Money = pl.PlayerProperty.coin;
            plsrs.Name = pl.Name;
            plsrs.otherElixirs = IntArrayToSrting(pl.elixirOutOfTeam);
            plsrs.otherPawns = IntArrayToSrting(pl.pawnOutOfTeam);
            plsrs.pawnsInBench = IntArrayToSrting(pl.team.pawnsInBench);
            plsrs.PlayeingPawns = IntArrayToSrting(pl.team.PlayeingPawns);
            plsrs.PowerLevel = pl.PowerLevel;
            plsrs.SoccerSpetial = pl.PlayerProperty.SoccerSpetial;
            plsrs.UsableFormations = IntArrayToSrting(pl.team.UsableFormations);
            return plsrs;
        }
        */

        public TeamForSerializeSingleString teamCompresor(TeamForSerialize team)
        {
            TeamForSerializeSingleString teamArrayless = new TeamForSerializeSingleString();
            teamArrayless.AttackFormation = team.AttackFormation.ToString();
            teamArrayless.DefienceForation = team.DefienceForation.ToString();
            teamArrayless.StartFomation = team.StartFomation.ToString();
            teamArrayless.ElixirInBench = IntArrayToSrting(team.ElixirInBench);
            teamArrayless.pawnsInBench = LongIntArrayToSrting(team.pawnsInBench);
            teamArrayless.PlayeingPawns = LongIntArrayToSrting(team.PlayeingPawns);
           // teamArrayless.UsableFormations = IntArrayToSrting(team.UsableFormations);
            return teamArrayless;
        }

        public TeamForSerialize teamDecompresor(TeamForSerializeSingleString team)
        {
            TeamForSerialize teamwithArray = new TeamForSerialize();
            teamwithArray.AttackFormation = Int32.Parse( team.AttackFormation);
            teamwithArray.DefienceForation = Int32.Parse(team.DefienceForation);
            teamwithArray.StartFomation = Int32.Parse(team.StartFomation);
            teamwithArray.ElixirInBench = StringToIntArray(team.ElixirInBench);
            teamwithArray.pawnsInBench = StringToLongIntArray(team.pawnsInBench);
            teamwithArray.PlayeingPawns = StringToLongIntArray(team.PlayeingPawns);
           // teamwithArray.UsableFormations = StringToIntArray(team.UsableFormations);            
            return teamwithArray;
        }

        public string TeamForSerializeToJson(TeamForSerialize team)
        {
            string uu;
            //TeamForSerializeSingleString singluarTeam = teamCompresor(team);
            uu = new JavaScriptSerializer().Serialize(team);
            return uu;
        }


        public string[] StringToSrtingArray(string ar)
        {
            string[] kk = new JavaScriptSerializer().Deserialize<string[]>(ar);
            return kk;
        }

    }
}