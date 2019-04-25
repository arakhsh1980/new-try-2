using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models.main_blocks;

namespace soccer1.Models.utilites
{
    public static class utilities
    {
        public static bool CheckIfFirstPropertyIsBigger(Property property1, Property property2)
        {
            bool check = true;
            if (property1.coin < property2.coin) { check = false; }
            if (property1.fan < property2.fan) { check = false; }
            if (property1.level < property2.level) { check = false; }
            if (property1.SoccerSpetial < property2.SoccerSpetial) { check = false; }
            return check;
        }

        public static PlayerForConnectedPlayer ReturnDefultPlayer()
        {
            PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
            pl.Name = "Defult";
            pl.PowerLevel = 0;
            pl.elixirOutOfTeam = new int[Statistics.MaxElixirOutOfTeam];
            for(int i=0; i< pl.elixirOutOfTeam.Length; i++) { pl.elixirOutOfTeam[i] = -1; }
            pl.pawnOutOfTeam = new int[Statistics.MaxPawnOutOfTeam];
            for (int i = 0; i < pl.pawnOutOfTeam.Length; i++) { pl.pawnOutOfTeam[i] = -1; }
            pl.team = AssetManager.returnDefultTeam();
            pl.property = new Property();
            pl.property.fan = 0;
            pl.property.coin = Statistics.StartingCoin;
            pl.property.level = 1;
            pl.property.SoccerSpetial = Statistics.StartingSS;
            return pl;
        }

       



    }
}