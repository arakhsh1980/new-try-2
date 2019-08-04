using System;
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
        //private static int pawnsConter = 0;
        //private static int ElixirConter = 0;
        //private static int FormationConter = 0;
        private static int OfferConter = 0;
        //give pawnname add return pawnindex
        public static Mutex assentsLoaded = new Mutex();
        //public static AutoResetEvent assentsLoaded = new AutoResetEvent(false);
        private static Mutex AddPawnmutex = new Mutex();
        public static Mutex AddElixirmutex = new Mutex();
        public static Mutex AddFormationmutex = new Mutex();
        public static Mutex AddOffermutex = new Mutex();
        private DataDBContext dataBase = new DataDBContext();
        private bool isDataBaseLoaded = false;
        public int ReturnrequiredXpForNextLevel(int idNum) {
            return Pawnlist[idNum].RequiredXpForUpgrade;
        }

        public void LoadDataFromServerifitsFirstTime()
        {
            if (!isDataBaseLoaded)
            {
                FillArrays();
                new SymShootMatchesList().FillArrays();
            }
        }


        public int ReturnAssetIndex(AssetType type, int IdNum)
        {
            /*
            int IndexOfAsset=-1;
            switch (type)
            {
                case AssetType.Pawn:
                    for (int i = 0; i < Pawnlist.Length; i++) if (Pawnlist[i] != null) {
                            if(Pawnlist[i].IdNum == IdNum) { IndexOfAsset = i; }
                        }
                   
                    break;
                case AssetType.Elixir:
                    for (int i = 0; i < Elixirlist.Length; i++) if (Elixirlist[i] != null)
                        {
                            if (Elixirlist[i].IdNum == IdNum) { IndexOfAsset = i; }
                        }
                    break;
                case AssetType.Formation:
                    for (int i = 0; i < Formationlist.Length; i++) if (Formationlist[i] != null)
                        {
                            if (Formationlist[i].IdNum == IdNum) { IndexOfAsset = i; }
                        }
                    break;
            }
            if (IdNum <0) { return -1; }
            if (IndexOfAsset < 0)
            {
                Log.AddLog("Error :Assetmanager. ReturnAssetIndex. Can Not Find This Pawn"+ IdNum);

            }
            return IndexOfAsset;
            */
            return IdNum;
        }
        
        public  Pawn RetrunPawnByIdName(int IdNamePawn)
        {
            return Pawnlist[IdNamePawn];
            /*
            Pawn p = new Pawn();
            p.IdNum = 0;            
            for (int i = 0; i < Pawnlist.Length; i++)
            {
                if (Pawnlist[i].IdNum == IdNamePawn){  p = Pawnlist[i]; }
            }             
            return p;
            */
        }

        public  Elixir ReturnElixirByIdName(int IdNamePawn)
        {
            return Elixirlist[IdNamePawn];
            /*
            Elixir p = new Elixir();
            p.IdNum =0;            
            for (int i = 0; i < Elixirlist.Length; i++)
            {
                if (Elixirlist[i].IdNum == IdNamePawn) {  p = Elixirlist[i]; }
            }
            return p;
            */
        }

        public  Formation ReturnFormationByIdName(int IdNamePawn)
        {
            return Formationlist[IdNamePawn];
            /*
            Formation p = new Formation();
            p.IdNum = 0;            
            for (int i = 0; i < Formationlist.Length; i++)
            {
                if (Formationlist[i].IdNum == IdNamePawn){  p = Formationlist[i]; }
            }
            return p;
            */
        }

        public int ReturnAssetName(AssetType type, int index)
        {
            return index;
            /*
            if(index == -1)
            {
                return -1;
            }
            if(arraylengh<= index || index < -1)
            {
                Errors.AddBigError("AssetManager. ReturnAssetName.  index out of reng1");
                return -1;
            }
            int maxAssined=0;
            int idName= 0;
            switch (type)
            {
                case AssetType.Pawn:
                    maxAssined = pawnsConter;
                    idName= Pawnlist[index].IdNum;
                    break;
                case AssetType.Elixir:
                    maxAssined = ElixirConter;
                    idName = Elixirlist[index].IdNum;
                    break;
                case AssetType.Formation:
                    maxAssined = FormationConter;
                    idName = Formationlist[index].IdNum;
                    break;
            }
            if (maxAssined < index )
            {
                Errors.AddBigError("AssetManager. ReturnAssetName. index out of reng2");
                return -1;
            }
            return idName;
            */
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

            //int maxAssined = 0;           
            switch (type)
            {
                case AssetType.Pawn:
                   // maxAssined = pawnsConter;
                    thisprop = Pawnlist[index].price;
                    break;
                case AssetType.Elixir:
                   // maxAssined = ElixirConter;
                    thisprop = Elixirlist[index].price;
                    break;
                case AssetType.Formation:
                   // maxAssined = FormationConter;
                    thisprop = Formationlist[index].price;
                    break;
            }
            
            pop.coin = thisprop.coin;
            pop.fan = thisprop.fan;
            pop.level = thisprop.level;
            pop.SoccerSpetial = thisprop.SoccerSpetial;
            return pop;
        }

        /*
        public  Property ReturnAssetPrice(AssetType type, int  IdNum)
        {
            return ReturnAssetPrice(type, ReturnAssetIndex(type, IdNum));
        }
        */

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


        /*
        public void AddPawnToAssetsOld(Pawn p)
        {
            AddPawnmutex.WaitOne();
            if (Pawnlist.Length <= pawnsConter) { Errors.AddBigError("AddPawnToAssets. Pawnlist.Length <= pawnsConter"); return; }
            int indexinarray = -1;
            for (int i=0; i<= pawnsConter; i++) if (Pawnlist[i].IdNum == p.IdNum) { indexinarray = i; }
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

        public  void AddFormationToAssetsOld(Formation ff)
        {
            AddFormationmutex.WaitOne();
            if (Formationlist.Length <= FormationConter) { Errors.AddBigError("Formationlist.Length <= FormationConter"); return; }
            int indexinarray = -1;
            for (int i = 0; i <= FormationConter; i++) if (Formationlist[i].IdNum == ff.IdNum) { indexinarray = i; }
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

        public void AddElixirToAssetsOld(Elixir el)
        {

            AddElixirmutex.WaitOne();
            if (Elixirlist.Length <= ElixirConter) { Errors.AddBigError("Elixirlist.Length <= ElixirConter"); return; }
            int indexinarray = -1;
            for (int i = 0; i <= ElixirConter; i++) if (Elixirlist[i].IdNum == el.IdNum) { indexinarray = i; }
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
        */

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
                new DatabaseManager().AddOfferToDataBase(offer);
            }
            else
            {
                offer.index = indexinarray;
                Offerlist[indexinarray] = offer;
                new DatabaseManager().AddOfferToDataBase(offer);
            }
            AddOffermutex.ReleaseMutex();
        }


        public void AddPawnToAssets(Pawn p)
        {
            AddPawnmutex.WaitOne();
            if (p.IdNum<0 || 99<p.IdNum) { Errors.AddBigError("AddPawnToAssets. Pawnlist.Length <= pawnsConter"); return; }
            
            p.key = p.IdNum.ToString();
            Pawnlist[p.IdNum] = p;
            new DatabaseManager().AddPawnToDataBase(p);
            AddPawnmutex.ReleaseMutex();
        }

        public void AddFormationToAssets(Formation ff)
        {
            AddFormationmutex.WaitOne();
            if (ff.IdNum < 0 || 99 < ff.IdNum) { Errors.AddBigError("Formationlist.Length <= FormationConter"); return; }
            
            ff.key = ff.IdNum.ToString();
            Formationlist[ff.IdNum] = ff;
            new DatabaseManager().AddFormationToDataBase(ff);
            AddFormationmutex.ReleaseMutex();
        }



        public void AddElixirToAssets(Elixir el)
        {

            AddElixirmutex.WaitOne();
            if (el.IdNum < 0 || 99 < el.IdNum) { Errors.AddBigError("Elixirlist.Length <= ElixirConter"); return; }
            
            el.key = el.IdNum.ToString();
            Elixirlist[el.IdNum] = el;
            new DatabaseManager().AddElixirToDataBase(el);
            AddElixirmutex.ReleaseMutex();
        }
        
        public void FillArrays()
        {
            AddElixirmutex.WaitOne();
            AddFormationmutex.WaitOne();
            AddPawnmutex.WaitOne();
            assentsLoaded.WaitOne();
            for (int i=0; i< arraylengh; i++)
            {
                //Pawnlist[i] = new Pawn();
                //Elixirlist[i] = new Elixir();
                //Formationlist[i] = new Formation();
                Offerlist[i] = new Offer();
            }
            Formation[] formations = dataBase.allFormations.ToArray();
            for (int i = 0; i < formations.Length; i++)
            {
                Formationlist[formations[i].IdNum] = formations[i];
            }

            Elixir[] elixirs = dataBase.allElixires.ToArray();
            for (int i = 0; i < elixirs.Length; i++)
            {
                Elixirlist[elixirs[i].IdNum] = elixirs[i];
            }

            Pawn[] pawns = dataBase.allPawns.ToArray();
            for (int i = 0; i < pawns.Length; i++)
            {
                Pawnlist[pawns[i].IdNum] = pawns[i];
            }
            //dataBase.
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

