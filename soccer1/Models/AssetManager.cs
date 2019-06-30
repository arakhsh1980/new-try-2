﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models.main_blocks;
using soccer1.Models.utilites;
using soccer1.Models.DataBase;
using soccer1.Controllers;
using System.Data.Entity;
using System.Web.Mvc;
using soccer1;
using System.Threading;

namespace soccer1.Models
{
    public  class AssetManager
    {        
        const int arraylengh = 100;
        private static Pawn[] Pawnlist = new Pawn[arraylengh];
        private static Elixir[] Elixirlist = new Elixir[arraylengh];
        private static Formation[] Formationlist = new Formation[arraylengh];
        private static Offer[] Offerlist = new Offer[arraylengh];
        private static int pawnsConter = 0;
        private static int ElixirConter = 0;
        private static int FormationConter = 0;
        private static int OfferConter = 0;
        //give pawnname add return pawnindex
        public static Mutex assentsLoaded = new Mutex();
        //public static AutoResetEvent assentsLoaded = new AutoResetEvent(false);
        private static Mutex AddPawnmutex = new Mutex();
        public static Mutex AddElixirmutex = new Mutex();
        public static Mutex AddFormationmutex = new Mutex();
        public static Mutex AddOffermutex = new Mutex();
        
        public int ReturnAssetIndex(AssetType type, string IdNamePawn)
        {
            int IndexOfAsset=-1;
            switch (type)
            {
                case AssetType.Pawn:
                    for (int i = 0; i < Pawnlist.Length; i++) if (Pawnlist[i] != null) {
                            if(Pawnlist[i].IdName == IdNamePawn) { IndexOfAsset = i; }
                        }
                   
                    break;
                case AssetType.Elixir:
                    for (int i = 0; i < Elixirlist.Length; i++) if (Elixirlist[i] != null)
                        {
                            if (Elixirlist[i].IdName == IdNamePawn) { IndexOfAsset = i; }
                        }
                    break;
                case AssetType.Formation:
                    for (int i = 0; i < Formationlist.Length; i++) if (Formationlist[i] != null)
                        {
                            if (Formationlist[i].IdName == IdNamePawn) { IndexOfAsset = i; }
                        }
                    break;
            }
            if (IdNamePawn == "null") { return -1; }
            if (IndexOfAsset < 0)
            {
                Log.AddLog("Error :Assetmanager. ReturnAssetIndex. Can Not Find This Pawn"+ IdNamePawn);

            }
            return IndexOfAsset;
        }
        
        public  Pawn RetrunPawnByIdName(string IdNamePawn)
        {
            Pawn p = new Pawn();
            p.IdName = "null";            
            for (int i = 0; i < Pawnlist.Length; i++)
            {
                if (Pawnlist[i].IdName == IdNamePawn){  p = Pawnlist[i]; }
            }             
            return p;
        }

        public  Elixir ReturnElixirByIdName(string IdNamePawn)
        {
            Elixir p = new Elixir();
            p.IdName = "null";            
            for (int i = 0; i < Elixirlist.Length; i++)
            {
                if (Elixirlist[i].IdName == IdNamePawn) {  p = Elixirlist[i]; }
            }
            return p;
        }

        public  Formation ReturnFormationByIdName(string IdNamePawn)
        {
            Formation p = new Formation();
            p.IdName = "null";            
            for (int i = 0; i < Formationlist.Length; i++)
            {
                if (Formationlist[i].IdName == IdNamePawn){  p = Formationlist[i]; }
            }
            return p;
        }

        public string ReturnAssetName(AssetType type, int index)
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

        public  Property ReturnAssetPrice(AssetType type, int index)
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

        public  Property ReturnAssetPrice(AssetType type, string  IdName)
        {
            return ReturnAssetPrice(type, ReturnAssetIndex(type, IdName));
        }


        public Property ReturnOfferPrice(string IdName)
        {

            Property thisprop = new Property();
            thisprop.coin = int.MaxValue;
            thisprop.fan = int.MaxValue;
            int index=-1;
            for (int i = 0; i < Offerlist.Length; i++) if (Offerlist[i] != null)
                {
                    if (Offerlist[i].IdName == IdName) { index = i; }
                }                      
            if (arraylengh <= index || index < 0)
            {
                Errors.AddBigError(" ReturnAssetPrice.index out of reng1");
                return thisprop;                
            }          
            if (OfferConter < index)
            {
                Errors.AddBigError(" ReturnAssetPrice. index out of reng2");
                return thisprop;
            }
            Property cashprop = new Property();
            cashprop = Offerlist[index].price;
            thisprop.coin = cashprop.coin;
            thisprop.fan = cashprop.fan;
            thisprop.level = cashprop.level;
            thisprop.SoccerSpetial = cashprop.SoccerSpetial;
            return thisprop;
        }

        public Property ReturnOfferBuyingMaterial(string IdName)
        {

            Property thisprop = new Property();
            thisprop.coin =0;
            thisprop.fan = 0;
            thisprop.level = 0;
            thisprop.SoccerSpetial = 0;
            int index = -1;
            for (int i = 0; i < Offerlist.Length; i++) if (Offerlist[i] != null)
                {
                    if (Offerlist[i].IdName == IdName) { index = i; }
                }
            if (arraylengh <= index || index < 0)
            {
                Errors.AddBigError(" ReturnAssetPrice.index out of reng1");
                return thisprop;
            }
            if (OfferConter < index)
            {
                Errors.AddBigError(" ReturnAssetPrice. index out of reng2");
                return thisprop;
            }           
            switch (Offerlist[index].BuyedmoneyType)
            {
                case 0:
                    thisprop.coin += Offerlist[index].BuyedmoneyAmount;
                    break;
                case 1:
                    thisprop.fan += Offerlist[index].BuyedmoneyAmount;
                    break;
                case 2:
                    thisprop.SoccerSpetial += Offerlist[index].BuyedmoneyAmount;
                    break;
                case 3:
                    thisprop.level += Offerlist[index].BuyedmoneyAmount;
                    break;
            }            
            return thisprop;
        }



        public void AddPawnToAssets(Pawn p)
        {
            AddPawnmutex.WaitOne();
            if (Pawnlist.Length <= pawnsConter) { Errors.AddBigError("AddPawnToAssets. Pawnlist.Length <= pawnsConter"); return; }
            int indexinarray = -1;
            for (int i=0; i<= pawnsConter; i++) if (Pawnlist[i].IdName == p.IdName) { indexinarray = i; }
            if(indexinarray == -1)
            {
                p.index = pawnsConter;
                Pawnlist[pawnsConter] = p;
                //Log.AddLog("AssetManager.AddPawnToAssets:  pawn Added. Name:" + p.IdName + "  index:" + pawnsConter.ToString());
                pawnsConter++;
                DatabaseManager.AddPawnToDataBase(p);
            }
            else
            {
                p.index = indexinarray;
                Pawnlist[indexinarray] = p;
                //Log.AddLog("AssetManager.AddPawnToAssets: pawn Updated. Name:" + p.IdName + "  index:" + indexinarray.ToString());
                DatabaseManager.AddPawnToDataBase(p);
            }
            AddPawnmutex.ReleaseMutex();

        }

        public  void AddFormationToAssets(Formation ff)
        {
            AddFormationmutex.WaitOne();
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
            AddFormationmutex.ReleaseMutex();
        }

        public void AddOfferToAssets(Offer offer)
        {
            AddOffermutex.WaitOne();
            if (Offerlist.Length <= OfferConter) { Errors.AddBigError("Offerlist.Length <= OfferConter"); return; }
            int indexinarray = -1;
            for (int i = 0; i <= OfferConter; i++) if (Offerlist[i].IdName == offer.IdName) { indexinarray = i; }
            if (indexinarray == -1)
            {
                offer.index = OfferConter;
                Offerlist[OfferConter] = offer;
                OfferConter++;
                DatabaseManager.AddOfferToDataBase(offer);
            }
            else
            {
                offer.index = indexinarray;
                Offerlist[indexinarray] = offer;
                DatabaseManager.AddOfferToDataBase(offer);
            }
            AddOffermutex.ReleaseMutex();
        }



        public void AddElixirToAssets(Elixir el)
        {
            AddElixirmutex.WaitOne();
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
            AddElixirmutex.ReleaseMutex();
        }
        
        public static void FillArrays()
        {
            AddElixirmutex.WaitOne();
            AddFormationmutex.WaitOne();
            AddPawnmutex.WaitOne();

            for (int i=0; i< arraylengh; i++)
            {
                Pawnlist[i] = new Pawn();
                Elixirlist[i] = new Elixir();
                Formationlist[i] = new Formation();
                Offerlist[i] = new Offer();
            }
            LoadAssets();

        }

        
        public static void LoadAssets()
        {
            
            assentsLoaded.WaitOne();
            DataDBContext dataBase = new DataDBContext();
            Formation[] formations = dataBase.allFormations.ToArray();
            for(int i =0; i< formations.Length; i++)
            {
                Formationlist[formations[i].index] = formations[i];
                FormationConter++;
            }

            Elixir[] elixirs = dataBase.allElixires.ToArray();
            for (int i = 0; i < elixirs.Length; i++)
            {
                Elixirlist[elixirs[i].index] = elixirs[i];
                ElixirConter++;
            }

            Pawn[] pawns = dataBase.allPawns.ToArray();
            for (int i = 0; i < pawns.Length; i++)
            {
                Pawnlist[pawns[i].index] = pawns[i];
                pawnsConter++;
            }
            AddElixirmutex.ReleaseMutex();
            AddFormationmutex.ReleaseMutex();
            AddPawnmutex.ReleaseMutex();
            assentsLoaded.ReleaseMutex();
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

