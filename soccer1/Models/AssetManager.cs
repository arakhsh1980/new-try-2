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

        public static int ReturnAssetIndex(AssetType type, string IdNamePawn)
        {
            int IndexOfAsset=-1;
            switch (type)
            {
                case AssetType.Pawn:
                    for (int i = 0; i < Pawnlist.Length; i++) if (Pawnlist[i].IdName == IdNamePawn) { IndexOfAsset = i; }
                   
                    break;
                case AssetType.Elixir:
                    for (int i = 0; i < Elixirlist.Length; i++) if (Elixirlist[i].IdName == IdNamePawn) { IndexOfAsset = i; }
                    break;
                case AssetType.Formation:
                    for (int i = 0; i < Formationlist.Length; i++) if (Formationlist[i].IdName == IdNamePawn) { IndexOfAsset = i; }
                    break;
            }
            if (IdNamePawn == "null") { return -1; }
            if (IndexOfAsset < 0)
            {
                Log.AddLog("Error :Assetmanager. ReturnAssetIndex. Can Not Find This Pawn");
            }
            return IndexOfAsset;
        }
        
        public static Pawn RetrunPawnByIdName(string IdNamePawn)
        {
            Pawn p = new Pawn();
            p.IdName = "null";            
            for (int i = 0; i < Pawnlist.Length; i++)
            {
                if (Pawnlist[i].IdName == IdNamePawn){  p = Pawnlist[i]; }
            }             
            return p;
        }

        public static Elixir ReturnElixirByIdName(string IdNamePawn)
        {
            Elixir p = new Elixir();
            p.IdName = "null";            
            for (int i = 0; i < Elixirlist.Length; i++)
            {
                if (Elixirlist[i].IdName == IdNamePawn) {  p = Elixirlist[i]; }
            }
            return p;
        }

        public static Formation ReturnFormationByIdName(string IdNamePawn)
        {
            Formation p = new Formation();
            p.IdName = "null";            
            for (int i = 0; i < Formationlist.Length; i++)
            {
                if (Formationlist[i].IdName == IdNamePawn){  p = Formationlist[i]; }
            }
            return p;
        }

        public static string ReturnAssetName(AssetType type, int index)
        {
            if(index == -1)
            {
                return "null";
            }
            if(arraylengh<= index || index < -1)
            {
                Errors.AddBigError("AssetManager. ReturnAssetName.  index out of reng1");
                return "Error";
            }
            int maxAssined=0;
            string idName= "Error";
            switch (type)
            {
                case AssetType.Pawn:
                    maxAssined = pawnsConter;
                    idName= Pawnlist[index].IdName;
                    break;
                case AssetType.Elixir:
                    maxAssined = ElixirConter;
                    idName = Elixirlist[index].IdName;
                    break;
                case AssetType.Formation:
                    maxAssined = FormationConter;
                    idName = Formationlist[index].IdName;
                    break;
            }
            if (maxAssined < index )
            {
                Errors.AddBigError("AssetManager. ReturnAssetName. index out of reng2");
                return "Error";
            }
            return idName;
        }

        public static Property ReturnAssetPrice(AssetType type, int index)
        {
            Property pop = new Property();
            pop.coin = int.MaxValue;
            pop.fan = int.MaxValue;
            Property thisprop = new Property();
            if (arraylengh <= index || index < 0)
            {
                Errors.AddBigError(" ReturnAssetPrice.index out of reng1");
                return pop;
            }

            int maxAssined = 0;           
            switch (type)
            {
                case AssetType.Pawn:
                    maxAssined = pawnsConter;
                    thisprop = Pawnlist[index].price;
                    break;
                case AssetType.Elixir:
                    maxAssined = ElixirConter;
                    thisprop = Elixirlist[index].price;
                    break;
                case AssetType.Formation:
                    maxAssined = FormationConter;
                    thisprop = Formationlist[index].price;
                    break;
            }
            if (maxAssined < index)
            {
                Errors.AddBigError(" ReturnAssetPrice. index out of reng2");
                return pop;
            }
            pop.coin = thisprop.coin;
            pop.fan = thisprop.fan;
            pop.level = thisprop.level;
            pop.SoccerSpetial = thisprop.SoccerSpetial;
            return pop;
        }

        public static Property ReturnAssetPrice(AssetType type, string  IdName)
        {
            return ReturnAssetPrice(type, ReturnAssetIndex(type, IdName));
        }

        public static void AddPawnToAssets(Pawn p)
        {
            if(Pawnlist.Length <= pawnsConter) { Errors.AddBigError("AddPawnToAssets. Pawnlist.Length <= pawnsConter"); return; }
            int indexinarray = -1;
            for (int i=0; i<= pawnsConter; i++) if (Pawnlist[i].IdName == p.IdName) { indexinarray = i; }
            if(indexinarray == -1)
            {
                p.index = pawnsConter;
                Pawnlist[pawnsConter] = p;
                pawnsConter++;
                
                DatabaseManager.AddPawnToDataBase(p);
            }
            else
            {
                p.index = indexinarray;
                Pawnlist[indexinarray] = p;                
                DatabaseManager.AddPawnToDataBase(p);
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
                DatabaseManager.AddFormationToDataBase(ff);
            }
            else
            {
                ff.index = indexinarray;
                Formationlist[indexinarray] = ff;
                DatabaseManager.AddFormationToDataBase(ff);
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
                DatabaseManager.AddElixirToDataBase(el);
            }
            else
            {
                el.index = indexinarray;
                Elixirlist[indexinarray] = el;
                DatabaseManager.AddElixirToDataBase(el);
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



        /*
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
        */



        //public static add

        //give index of pawn and return price of that pawn

        /*
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
            */


    }

}

