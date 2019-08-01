using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models.main_blocks;
using soccer1.Models.DataBase;
using soccer1.Models.utilites;

namespace soccer1.Models.utilites
{
    public class Utilities
    {
        public bool CheckIfFirstPropertyIsBigger(Property property1, Property property2)
        {
            bool check = true;
            if (property1.coin < property2.coin ) { check = false; }
            if (property1.fan < property2.fan) { check = false; }
            if (property1.level < property2.level) { check = false; }
            if (property1.SoccerSpetial < property2.SoccerSpetial) { check = false; }
             
            return check;
        }

        public PlayerForDatabase ReturnDefultPlayer()
        {
            PlayerForDatabase player = new PlayerForDatabase();            
            TeamForConnectedPlayers team =returnDefultTeam();
            List<int> pawnOutOfTeam = new List<int>();
            pawnOutOfTeam.Add(10);
            List<int> elixirOutOfTeam = new List<int>();
            player.CurrentFormation = team.CurrentFormation;
            player.ElixirInBench = new Convertors().IntArrayToSrting(team.ElixirInBench);
            player.Fan = 0;            
            player.level = 1;
            player.Money = Statistics.StartingCoin;
            player.Name = "Defult";
            player.otherElixirs = new Convertors().IntArrayToSrting(new Convertors().listIntToIntArray(elixirOutOfTeam));
            player.otherPawns = new Convertors().IntArrayToSrting(new Convertors().listIntToIntArray(pawnOutOfTeam));
            player.pawnsInBench = new Convertors().IntArrayToSrting(team.pawnsInBench);
            player.PlayeingPawns = new Convertors().IntArrayToSrting(team.PlayeingPawns);
            player.PowerLevel = 1;
            player.SoccerSpetial = Statistics.StartingSS;
            player.UsableFormations = new Convertors().IntArrayToSrting(team.UsableFormations);           
            return player;
        }



        public TeamForConnectedPlayers returnDefultTeam()
        {
            TeamForConnectedPlayers team = new TeamForConnectedPlayers();
            //int defultPawnIndex = new AssetManager().ReturnAssetIndex(AssetType.Pawn ,0); 
            int defultPawnIndex = 10;
            int defultElixirIndex = new AssetManager().ReturnAssetIndex(AssetType.Elixir, 0);            
            int defultFormationIndex = new AssetManager().ReturnAssetIndex(AssetType.Formation, 0);            
            team.CurrentFormation = defultFormationIndex;
            for (int i = 0; i < team.PlayeingPawns.Length; i++) { team.PlayeingPawns[i] = defultPawnIndex+(10000*i); }
            for (int i = 0; i < team.pawnsInBench.Length; i++) { team.pawnsInBench[i] = defultPawnIndex+(10000*(i+ team.PlayeingPawns.Length)); }
            for (int i = 0; i < team.UsableFormations.Length; i++) { team.UsableFormations[i] = -1; }
            for (int i = 0; i < team.ElixirInBench.Length; i++) { team.ElixirInBench[i] = -1; }
            team.ElixirInBench[0] = defultElixirIndex;
            return team;
        }

        public AssetType ReturnAssetTypeByName(string typeName)
        {
            AssetType type = AssetType.none;
            switch (typeName)
            {
                case "Pawn":
                    type = AssetType.Pawn;
                    break;
                case "Elixir":
                    type = AssetType.Elixir;
                    break;
                case "Formation":
                    type = AssetType.Formation;
                    break;
            }
            if (type == AssetType.none) { Errors.AddSmallError("AssetType not found"); }
            return type;
        }


        public AssetType ReturnOfferByName(string typeName)
        {
            AssetType type = AssetType.none;
            switch (typeName)
            {
                case "Pawn":
                    type = AssetType.Pawn;
                    break;
                case "Elixir":
                    type = AssetType.Elixir;
                    break;
                case "Formation":
                    type = AssetType.Formation;
                    break;
            }
            if (type == AssetType.none) { Errors.AddSmallError("AssetType not found"); }
            return type;
        }


        public Property SubtracProperty(Property currentProp , Property subbedProp)
        {
            Property newPro = new Property();
            newPro = currentProp;
            newPro.coin -= subbedProp.coin;
            newPro.fan -= subbedProp.fan;
            newPro.level -= subbedProp.level;
            newPro.SoccerSpetial -= subbedProp.SoccerSpetial;
            return newPro;
        }

    }
}