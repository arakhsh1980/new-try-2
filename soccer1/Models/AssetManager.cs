using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models.main_blocks;
using soccer1.Models.utilites;
using System.Data.Entity;
using System.Web.Mvc;
using soccer1;

namespace soccer1.Models
{
    public static class AssetManager
    {

        const int arraylengh = 100;
        private static Pawn[] Pawnlist = new Pawn[arraylengh];
        private static Elixir[] Elixirlist = new Elixir[arraylengh];
        private static Formation[] Formationlist = new Formation[arraylengh];
        private static int pawnsConter = 0;
        private static int ElixirConter = 0;
        private static int FormationConter = 0;
        //give pawnname add return pawnindex
        public static int ReturnIndexOfPawn(string IdNamePawn)
        {
            int IndexOfPawn=-1;
            for (int i = 1; i < Pawnlist.Length; i++)
            {
                if (Pawnlist[i].IdName == IdNamePawn)
                {
                    IndexOfPawn = i;
                }
            }
            if (IndexOfPawn < 0)
            {
                Log.AddLog("Error : Can Not Find This Pawn");
            }
            return IndexOfPawn;
        }
        public static Pawn RetrunPawnByIdName(string IdNamePawn)
        {
            Pawn p = new Pawn();
            p.IdName = "null";            
            for (int i = 1; i < Pawnlist.Length; i++)
            {
                if (Pawnlist[i].IdName == IdNamePawn){  p = Pawnlist[i]; }
            }             
            return p;
        }

        public static Elixir ReturnElixirByIdName(string IdNamePawn)
        {
            Elixir p = new Elixir();
            p.IdName = "null";            
            for (int i = 1; i < Elixirlist.Length; i++)
            {
                if (Elixirlist[i].IdName == IdNamePawn) {  p = Elixirlist[i]; }
            }
            return p;
        }

        public static Formation ReturnFormationByIdName(string IdNamePawn)
        {
            Formation p = new Formation();
            p.IdName = "null";            
            for (int i = 1; i < Formationlist.Length; i++)
            {
                if (Formationlist[i].IdName == IdNamePawn){  p = Formationlist[i]; }
            }
            return p;
        }

        public static string ReturnPawnName(int pawnId)
        {
            if(pawnId<0 || pawnsConter < pawnId) {
                Errors.AddBigError("pawn not find");
                return "null";
            }
            else
            {
                return Pawnlist[pawnId].IdName;
            }            
        }

        public static string ReturnElixirName(int ElixirId)
        {
            if (ElixirId < 0 || ElixirConter < ElixirId)
            {
                Errors.AddBigError("Elixir not find");
                return "null";
            }
            else
            {
                return Elixirlist[ElixirId].IdName;
            }
        }
        
        public static string ReturnFormationName(int formId)
        {
            if (formId < 0 || FormationConter < formId)
            {
                Errors.AddBigError("Elixir not find");
                return "null";
            }
            else
            {
                return Formationlist[formId].IdName;
            }
        }

        //public static add

        //give index of pawn and return price of that pawn
        public static Property ReturnPawnPrice(int index)
        {
            Property pop = new Property();
            pop.coin = Pawnlist[index].price.coin;
            pop.fan = Pawnlist[index].price.fan;
            pop.level = Pawnlist[index].price.level;
            pop.SoccerSpetial = Pawnlist[index].price.SoccerSpetial;
            return pop;
        }

        public static Property ReturnElixirPrice(int index)
        {
            Property pop = new Property();
            pop.coin = Elixirlist[index].price.coin;
            pop.fan = Elixirlist[index].price.fan;
            pop.level = Elixirlist[index].price.level;
            pop.SoccerSpetial = Elixirlist[index].price.SoccerSpetial;
            return pop;
        }

        public static Property ReturnFormationPrice(int index)
        {
            Property pop = new Property();
            pop.coin = Formationlist[index].price.coin;
            pop.fan = Formationlist[index].price.fan;
            pop.level = Formationlist[index].price.level;
            pop.SoccerSpetial = Formationlist[index].price.SoccerSpetial;
            return pop;
        }

        public static  TeamForConnectedPlayers returnDefultTeam()
        {
            TeamForConnectedPlayers team = new TeamForConnectedPlayers();
            int defultPawnIndex = 1;
            for(int i=0; i< Pawnlist.Length; i++) if (Pawnlist[i].IdName == "Defult") { defultPawnIndex = i; }
            int defultElixirIndex = 1;
            for (int i = 0; i < Elixirlist.Length; i++) if (Elixirlist[i].IdName == "Defult") { defultElixirIndex = i; }
            int defultFormationIndex = 1;
            for (int i = 0; i < Formationlist.Length; i++) if (Formationlist[i].IdName == "Defult") { defultFormationIndex = i; }
            team.CurrentFormation = defultFormationIndex;
            for (int i = 0; i < team.PlayeingPawns.Length; i++) { team.PlayeingPawns[i] = defultPawnIndex; }
            for (int i = 0; i < team.pawnsInBench.Length; i++) { team.pawnsInBench[i] = defultPawnIndex; }
            for (int i = 0; i < team.UsableFormations.Length; i++) { team.UsableFormations[i] = -1; }
            for (int i = 0; i < team.ElixirInBench.Length; i++) { team.ElixirInBench[i] = defultElixirIndex; }
            return team;
        }

        public static void AddPawnToAssets(Pawn p)
        {
            if(Pawnlist.Length <= pawnsConter) { Errors.AddBigError("Pawnlist.Length <= pawnsConter"); return; }
            int indexinarray = -1;
            for (int i=0; i<= pawnsConter; i++) if (Pawnlist[i].IdName == p.IdName) { indexinarray = i; }
            if(indexinarray == -1)
            {
                p.index = pawnsConter;
                Pawnlist[pawnsConter] = p;
                pawnsConter++;
                //DatabaseManager.AddPawnToDataBase(p);
            }
            else
            {
                p.index = indexinarray;
                Pawnlist[indexinarray] = p;                
                //DatabaseManager.AddPawnToDataBase(p);
            }
        }




        public static void AddFormationToAssets(Formation ff)
        {
            if (Formationlist.Length <= FormationConter) { Errors.AddBigError("Formationlist.Length <= FormationConter"); return; }
            int indexinarray = -1;
            for (int i = 0; i <= FormationConter; i++) if (Formationlist[i].IdName == ff.IdName) { indexinarray = i; }
            if (indexinarray == -1)
            {
                ff.index = FormationConter;
                Formationlist[FormationConter] = ff;
                FormationConter++;
                //DatabaseManager.AddFormationToDataBase(ff);
            }
            else
            {
                ff.index = indexinarray;
                Formationlist[indexinarray] = ff;
                //DatabaseManager.AddFormationToDataBase(ff);
            }
        }


        public static void AddElixirToAssets(Elixir el)
        {
            if (Elixirlist.Length <= ElixirConter) { Errors.AddBigError("Elixirlist.Length <= ElixirConter"); return; }
            int indexinarray = -1;
            for (int i = 0; i <= ElixirConter; i++) if (Elixirlist[i].IdName == el.IdName) { indexinarray = i; }
            if (indexinarray == -1)
            {
                el.index = ElixirConter;
                Elixirlist[ElixirConter] = el;
                ElixirConter++;
                //DatabaseManager.AddElixirToDataBase(el);
            }
            else
            {
                el.index = indexinarray;
                Elixirlist[indexinarray] = el;
                //DatabaseManager.AddElixirToDataBase(el);
            }
        }


        public static void FillArrays()
        {
            for(int i=0; i< arraylengh; i++)
            {
                Pawnlist[i] = new Pawn();
                Elixirlist[i] = new Elixir();
                Formationlist[i] = new Formation();
            }
        }
    }
    
}

