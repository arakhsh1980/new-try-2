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
    public static class Convertors
    {
        public static ShootActionCode FormToShoot(FormCollection collection)
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

        public static TeamForSerialize TeamToTeamForSerialize(TeamForConnectedPlayers team)
        {
            TeamForSerialize slteam = new TeamForSerialize();
            slteam.CurrentFormation = AssetManager.ReturnAssetName(AssetType.Formation,  team.CurrentFormation);
            slteam.ElixirInBench = new string[team.ElixirInBench.Length];
            for (int i=0; i< team.ElixirInBench.Length; i++) { slteam.ElixirInBench[i] = AssetManager.ReturnAssetName(AssetType.Elixir, team.ElixirInBench[i]); }

            slteam.pawnsInBench = new string[team.pawnsInBench.Length];
            for (int i = 0; i < team.pawnsInBench.Length; i++) { slteam.pawnsInBench[i] = AssetManager.ReturnAssetName(AssetType.Pawn, team.pawnsInBench[i]); }

            slteam.PlayeingPawns = new string[team.PlayeingPawns.Length];
            for (int i = 0; i < team.PlayeingPawns.Length; i++) { slteam.PlayeingPawns[i] = AssetManager.ReturnAssetName(AssetType.Pawn, team.PlayeingPawns[i]); }

            slteam.UsableFormations = new string[team.UsableFormations.Length];
            for (int i = 0; i < team.UsableFormations.Length; i++) { slteam.UsableFormations[i] = AssetManager.ReturnAssetName(AssetType.Formation, team.UsableFormations[i]); }
        
            return slteam;
        }

        public static TeamForConnectedPlayers TeamForSerializeToTeam(TeamForSerialize pl)
        {
            //convert of Teamforserialize Class to Team Class 
            //convert string to int
            TeamForConnectedPlayers plsrs = new TeamForConnectedPlayers();
           
            for (int i = 0; i < pl.PlayeingPawns.Length; i++) { plsrs.PlayeingPawns[i] = AssetManager.ReturnAssetIndex(AssetType.Pawn,   pl.PlayeingPawns[i]); }
            for (int i = 0; i < pl.pawnsInBench.Length; i++) { plsrs.pawnsInBench[i] = AssetManager.ReturnAssetIndex(AssetType.Pawn, pl.pawnsInBench[i]); }
            int usableFormationCounter = 0;
            for (int i = 0; i < pl.UsableFormations.Length; i++)
            {
                if (pl.UsableFormations[i] != "null")
                {
                    plsrs.UsableFormations[usableFormationCounter] = AssetManager.ReturnAssetIndex(AssetType.Formation, pl.UsableFormations[i]);
                    usableFormationCounter++;
                }
            }
            for (int i = 0; i < pl.ElixirInBench.Length; i++) { plsrs.pawnsInBench[i] = AssetManager.ReturnAssetIndex(AssetType.Elixir, pl.ElixirInBench[i]); }

            plsrs.CurrentFormation = AssetManager.ReturnAssetIndex(AssetType.Formation, pl.CurrentFormation); 

            return plsrs;
        }

        public static string IntArrayToSrting(int[] ar)
        {
            string uu;
            uu = new JavaScriptSerializer().Serialize(ar);
            return uu;
        }

        public static int[] SrtingTointArray(string ar)
        {
            int[] kk = new JavaScriptSerializer().Deserialize<int[]>(ar);
            return kk;
        }

        
        public static TeamForConnectedPlayers JsonToTeam(string teamJson)
        {
            TeamForConnectedPlayers kk = new JavaScriptSerializer().Deserialize<TeamForConnectedPlayers>(teamJson);
            return kk;
        }


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


        public static TeamForSerializeSingleString teamCompresor(TeamForSerialize team)
        {
            TeamForSerializeSingleString teamArrayless = new TeamForSerializeSingleString();
            teamArrayless.CurrentFormation = team.CurrentFormation;
            teamArrayless.ElixirInBench = StringArrayToSrting(team.ElixirInBench);
            teamArrayless.pawnsInBench = StringArrayToSrting(team.pawnsInBench);
            teamArrayless.PlayeingPawns = StringArrayToSrting(team.PlayeingPawns);
            teamArrayless.UsableFormations = StringArrayToSrting(team.UsableFormations);
            return teamArrayless;
        }

        public static TeamForSerialize teamDecompresor(TeamForSerializeSingleString team)
        {
            TeamForSerialize teamwithArray = new TeamForSerialize();
            teamwithArray.CurrentFormation = team.CurrentFormation;
            teamwithArray.ElixirInBench = StringToSrtingArray(team.ElixirInBench);
            teamwithArray.pawnsInBench = StringToSrtingArray(team.pawnsInBench);
            teamwithArray.PlayeingPawns = StringToSrtingArray(team.PlayeingPawns);
            teamwithArray.UsableFormations = StringToSrtingArray(team.UsableFormations);            
            return teamwithArray;
        }

        public static string TeamForSerializeToJson(TeamForSerialize team)
        {
            string uu;
            TeamForSerializeSingleString singluarTeam = teamCompresor(team);
            uu = new JavaScriptSerializer().Serialize(singluarTeam);
            return uu;
        }

        public static string StringArrayToSrting(string[] ar)
        {
            string uu;
            uu = new JavaScriptSerializer().Serialize(ar);
            return uu;
        }

        public static string[] StringToSrtingArray(string ar)
        {
            string[] kk = new JavaScriptSerializer().Deserialize<string[]>(ar);
            return kk;
        }

    }
}