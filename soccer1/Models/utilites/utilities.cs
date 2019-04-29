using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models.main_blocks;

namespace soccer1.Models.utilites
{
    public class Utilities
    {
        public bool CheckIfFirstPropertyIsBigger(Property property1, Property property2)
        {
            bool check = true;
            if (property1.coin <= property2.coin) { check = false; }
            if (property1.fan <= property2.fan) { check = false; }
            if (property1.level <= property2.level) { check = false; }
            if (property1.SoccerSpetial <= property2.SoccerSpetial) { check = false; }
            return check;
        }

        public PlayerForConnectedPlayer ReturnDefultPlayer()
        {

            PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();           
            return pl;
        }



        public TeamForConnectedPlayers returnDefultTeam()
        {
            TeamForConnectedPlayers team = new TeamForConnectedPlayers();
            int defultPawnIndex = AssetManager.ReturnAssetIndex(AssetType.Pawn , "Defult");            
            int defultElixirIndex = AssetManager.ReturnAssetIndex(AssetType.Elixir, "Defult");            
            int defultFormationIndex = AssetManager.ReturnAssetIndex(AssetType.Formation, "Defult");            
            team.CurrentFormation = defultFormationIndex;
            for (int i = 0; i < team.PlayeingPawns.Length; i++) { team.PlayeingPawns[i] = defultPawnIndex; }
            for (int i = 0; i < team.pawnsInBench.Length; i++) { team.pawnsInBench[i] = defultPawnIndex; }
            for (int i = 0; i < team.UsableFormations.Length; i++) { team.UsableFormations[i] = -1; }
            for (int i = 0; i < team.ElixirInBench.Length; i++) { team.ElixirInBench[i] = defultElixirIndex; }
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



    }
}