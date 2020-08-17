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
using System.Web.Script.Serialization;

namespace soccer1.Models
{
    public  class AssetManager
    {        
        const int arraylengh = 100;
       // private static RoboPart[] RoboPartlist = new RoboPart[arraylengh];
        private static RoboBase[] RoboBaselist = new RoboBase[arraylengh];
        private static Elixir[] Elixirlist = new Elixir[arraylengh];
        private static Formation[] Formationlist = new Formation[arraylengh];
        private static Planet[] Planetlist = new Planet[arraylengh];
        private static ClassData[] ClassDataList = new ClassData[arraylengh];
        private static Offer[] Offerlist = new Offer[arraylengh];
        private static RoboPart[] RoboPartlist = new RoboPart[arraylengh];
        private static Sponsor[] Sponserslist = new Sponsor[arraylengh];
        private static MissionDefinition[] missionslist = new MissionDefinition[arraylengh];
        public static List<int> hostPlayers = new List<int>();
        //private static int pawnsConter = 0;
        //private static int ElixirConter = 0;
        //private static int FormationConter = 0;
        private static int RoboBaselistCounter = 0;
        private static int RoboPartlistCounter = 0;
        private static int ClassDatalistCounter = 0;
        private static int SponserlistCounter = 0;
        private static int FormationlistCounter = 0;
        private static int PlanetlistCounter = 0;
        private static int missionslistCounter = 0;
        private static int OfferConter = 0;
        //give pawnname add return pawnindex
        public static Mutex assentsLoaded = new Mutex();
        //public static AutoResetEvent assentsLoaded = new AutoResetEvent(false);
        private static Mutex AddPawnmutex = new Mutex();
        public static Mutex AddElixirmutex = new Mutex();
        public static Mutex AddFormationmutex = new Mutex();
        public static Mutex AddClassmutex = new Mutex();
        public static Mutex AddMissionmutex = new Mutex();
        public static Mutex AddOffermutex = new Mutex();
        //public static Mutex AddMissionmutex = new Mutex();
        private DataDBContext dataBase = new DataDBContext();
        private static bool isDataBaseLoaded = false;

        public List<string> returnClassAddedClassesAfterThisTime(long timeOfLastUpdate)
        {
            List<string> classesThatShouldUpdate = new List<string>();
            for (int i = 0; i < ClassDatalistCounter; i++)if(timeOfLastUpdate < ClassDataList[i].TimeOfLastUpdate )
            {
                    classesThatShouldUpdate.Add(ClassDataList[i].ClassCodeToString());
            }
            return classesThatShouldUpdate;
        }



        public int ReturnrequiredXpForNextLevel(int idNum) {
            return -1;
           // return Pawnlist[idNum].RequiredXpForUpgrade;
        }

        public int ReturnBaseLevel(int idNum)
        {
            int result = -1;
            for (int i = 0; i < RoboBaselist.Length; i++)if(RoboBaselist[i] != null)
                {
                    if (RoboBaselist[i].BaseID == idNum)
                    {
                        result = RoboBaselist[i].level;
                    }
                }
                
            return result;
            // return Pawnlist[idNum].RequiredXpForUpgrade;
        }

        public RoboBase ReturnBaseInfo(int idNum)
        {
            RoboBase result = null;
            for (int i = 0; i < RoboBaselist.Length; i++) if (RoboBaselist[i] != null)
                {
                    if (RoboBaselist[i].BaseID == idNum)
                    {
                        result = RoboBaselist[i];
                    }
                }

            return result;
            // return Pawnlist[idNum].RequiredXpForUpgrade;
        }


        public int ReturnRoboPartTimeToBuild(short partId)
        {
            for(int i =0; i< RoboPartlist.Length; i++)if(RoboPartlist[i] != null)
            {
                if((RoboPartType)RoboPartlist[i].partType == (RoboPartType)partId )
                    {
                        return RoboPartlist[i].MinuteToBuild;
                    }
            }
            return -1;
        }


        public RoboPart ReturnRoboPart(short partId)
        {
            RoboPart findedpart = null;
            for (int i = 0; i < RoboPartlist.Length; i++) if (RoboPartlist[i] != null)
                {
                    if ((RoboPartType)RoboPartlist[i].partType == (RoboPartType)partId )
                    {
                        findedpart = RoboPartlist[i];
                        return findedpart;
                    }
                }
            return null;
        }


        public Sponsor ReturnSponsor(string name)
        {
            Sponsor findedsp = null;
            for (int i = 0; i < Sponserslist.Length; i++) if (Sponserslist[i] != null)
                {
                    if (Sponserslist[i].NameCode == name )
                    {
                        findedsp = Sponserslist[i];
                        return findedsp;
                    }
                }
            // if could not fine sponser, return defult one;            
            return findedsp;
        }


        public MissionDefinition ReturnMission(short missionId)
        {
            MissionDefinition findedMission = null;
            for (int i = 0; i < missionslist.Length; i++) if (missionslist[i] != null)
                {
                    if (missionslist[i].IdNum == missionId )
                    {
                        findedMission = missionslist[i];
                        return findedMission;
                    }
                }
            return null;
        }


        public void LoadDataFromServerifitsFirstTime()
        {
            if (!isDataBaseLoaded)
            {
                isDataBaseLoaded = true;
                FillArrays();
                SymShootMatchesList.FillArrays();
                
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

        public Property ReturnOrderPrice(RoboPartType partType, short goldLevel)
        {
            Property pp = new Property();
            int index = -1;
            for(int i = 0; i < RoboPartlist.Length; i++)if(RoboPartlist[i] != null)
            {
                if((RoboPartType)RoboPartlist[i].partType == partType)
                {
                    index = i;
                }
            }
            if (index < 0)
            {
                Errors.AddBigError("assetManager. ReturnOrderPrice. order not find");
                return pp;
            }
            else
            {
                Property price = new Property();
                price.Alminum = RoboPartlist[index].requiredAlminiom;
                price.gold = RoboPartlist[index].requiredGold;
                switch (goldLevel)
                {
                    case 0:
                        pp.gold = price.gold;
                        pp.Alminum = price.Alminum;
                        break;
                    case 1:
                        pp.gold = price.gold + (int)Math.Floor(price.Alminum * 0.1f);
                        pp.Alminum = price.Alminum -(int) Math.Floor(price.Alminum * 0.2f);
                        break;
                    case 2:
                        pp.gold = price.gold + (int)Math.Floor(price.Alminum * 0.2f);
                        pp.Alminum = price.Alminum - (int)Math.Floor(price.Alminum * 0.5f);
                        break;
                    default:
                        Errors.AddBigError("assetManager. ReturnOrderPrice. goldLevel not finded ??");
                        break;
                }

            }
            return pp;
        }

        public Property ReturnScrapPrice(short partID, short goldLevel)
        {
            Property pp = new Property();
            int index = -1;
            for (int i = 0; i < RoboPartlist.Length; i++) if (RoboPartlist[i] != null)
                {
                    if ((short)RoboPartlist[i].partType == partID )
                    {
                        index = i;
                    }
                }
            if (index < 0)
            {
                Errors.AddBigError("assetManager. ReturnOrderPrice. order not find");
                return pp;
            }
            else
            {
                Property price = new Property();
                price.Alminum = RoboPartlist[index].requiredAlminiom;
                price.gold = RoboPartlist[index].requiredGold;
                switch (goldLevel)
                {
                    case 0:
                        pp.gold = price.gold;
                        pp.Alminum = price.Alminum;
                        break;
                    case 1:
                        pp.gold = price.gold + (int)Math.Floor(price.Alminum * 0.1f);
                        pp.Alminum = price.Alminum - (int)Math.Floor(price.Alminum * 0.2f);
                        break;
                    case 2:
                        pp.gold = price.gold + (int)Math.Floor(price.Alminum * 0.2f);
                        pp.Alminum = price.Alminum - (int)Math.Floor(price.Alminum * 0.5f);
                        break;
                    default:
                        Errors.AddBigError("assetManager. ReturnOrderPrice. goldLevel not finded ??");
                        break;
                }

            }
            pp.Alminum = (int)Math.Floor(pp.Alminum * 0.5f);
            return pp;
        }



        public Property ReturnAssetPrice(AssetType type, int index)
        {
            
               Property pop = new Property();
            pop.Alminum = int.MaxValue;
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
                   // thisprop = Pawnlist[index].price;
                    break;                
                case AssetType.Elixir:
                   // maxAssined = ElixirConter;
                    thisprop = Elixirlist[index].price;
                    break;
                case AssetType.Formation:
                   // maxAssined = FormationConter;
                    thisprop = Formationlist[index].price();
                    break;
            }
            
            pop.Alminum = thisprop.Alminum;
            pop.fan = thisprop.fan;
            pop.tropy = thisprop.tropy;
            pop.gold = thisprop.gold;
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
            thisprop.Alminum = int.MaxValue;
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
            thisprop.Alminum = cashprop.Alminum;
            thisprop.fan = cashprop.fan;
            thisprop.tropy = cashprop.tropy;
            thisprop.gold = cashprop.gold;
            return thisprop;
        }

        public Property ReturnOfferBuyingMaterial(string IdName)
        {

            Property thisprop = new Property();
            thisprop.Alminum =0;
            thisprop.fan = 0;
            thisprop.tropy = 0;
            thisprop.gold = 0;
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
                    thisprop.Alminum += Offerlist[index].BuyedmoneyAmount;
                    break;
                case 1:
                    thisprop.fan += Offerlist[index].BuyedmoneyAmount;
                    break;
                case 2:
                    thisprop.gold += Offerlist[index].BuyedmoneyAmount;
                    break;
                case 3:
                    thisprop.tropy += Offerlist[index].BuyedmoneyAmount;
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

        public Property returnSponserWinPrise(string spName)
        {
           
           
            Property result = new Property();
            for (int i = 0; i < Sponserslist.Length; i++) if (Sponserslist[i] != null)
                {
                    if (Sponserslist[i].NameCode == spName) {
                        result.Alminum = Sponserslist[i].AlminumPerWin;
                        result.gold = Sponserslist[i].goldPerWin;
                        return result;
                    }
                }
            return result;
        }

        public List<string> PlayerChosableSponsers(PlayerForConnectedPlayer p)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < Sponserslist.Length; i++)if(Sponserslist[i] != null)
                
                {
                    if (Sponserslist[i].NameCode != p.sponsorName && sponsorChosableityCheck(Sponserslist[i].NameCode, p) && Sponserslist[i].minAcceptableTropy <= p.PlayerProperty.tropy && Sponserslist[i].minAcceptableXp <= p.totalXp)
                {
                        
                    result.Add(Sponserslist[i].NameCode);
                }
                    
                }
            return result;
        }

        public bool sponsorChosableityCheck(string spname, PlayerForConnectedPlayer p)
        {
            
            switch (spname)
            {
                case "BlackBound":
                    return true;
                    break;
                case "TheExpand":
                    return true;                   
                    break;
                case "WarmMountain":
                    return true;
                    break;
                case "ColdMountain":
                    return true;
                    break;
                default:
                    return true;
                    break;
            }
        }

        public List<string> PlayerSujestabeSponsers(PlayerForConnectedPlayer p)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < Sponserslist.Length; i++) if (Sponserslist[i] != null)
                {
                    if (Sponserslist[i].NameCode != p.sponsorName)
                    {
                        result.Add(Sponserslist[i].NameCode);
                    }

                }
            return result;
        }


        public void AddClassDataToAssets(ClassData CD, bool IsLoadedFromDatabase)
        {
            
            bool isclassDataNewOrUpdated =true;
            for (int i = 0; i < ClassDatalistCounter; i++)
            {
                if(ClassDataList[i].nameCode == CD.nameCode && ClassDataList[i].type == CD.type && ClassDataList[i].innerData == CD.innerData)
                {
                    
                    isclassDataNewOrUpdated = false;
                    return;
                }
            }

            if (!IsLoadedFromDatabase)
            {
                
                CD.TimeOfLastUpdate = DateTime.UtcNow.ToBinary();
                new DatabaseManager().AddClassDataToDataBase(CD);
            }           
            ClassDataList[ClassDatalistCounter] = CD;
            ClassDatalistCounter++;
            ClassDataType type = (ClassDataType)(CD.type);
            switch (type)
            {
                case ClassDataType.roboBase:
                    RoboBase newRoboBase = new JavaScriptSerializer().Deserialize<RoboBase>(CD.innerData);
                    RoboBaselist[RoboBaselistCounter] = newRoboBase;
                    RoboBaselistCounter++;
                    break;
                case ClassDataType.RoboPart:
                    RoboPart newRoboPart = new JavaScriptSerializer().Deserialize<RoboPart>(CD.innerData);
                    RoboPartlist[RoboPartlistCounter] = newRoboPart;
                    RoboPartlistCounter++;
                    break;
                case ClassDataType.sponser:
                    Sponsor newRoboSponsor = new JavaScriptSerializer().Deserialize<Sponsor>(CD.innerData);
                    Sponserslist[SponserlistCounter] = newRoboSponsor;
                    SponserlistCounter++;
                    break;
                case ClassDataType.formation:
                    Formation newRoboFormation = new JavaScriptSerializer().Deserialize<Formation>(CD.innerData);
                    Formationlist[FormationlistCounter] = newRoboFormation;
                    FormationlistCounter++;
                    break;
                case ClassDataType.planet:
                    Planet newPlanet = new JavaScriptSerializer().Deserialize<Planet>(CD.innerData);
                    Planetlist[PlanetlistCounter] = newPlanet;
                    PlanetlistCounter++;
                    break;
                case ClassDataType.offer:
                    break;
                case ClassDataType.GameInnreData:
                    GameInnreData indata = new JavaScriptSerializer().Deserialize<GameInnreData>(CD.innerData);
                    indata.SetBasePrefrenceAccordingToThis();
                    break;
                default:
                    break;
            }
            
        }

        bool fillArrayRunded = false;
        public void FillArrays()
        {
            
            if (fillArrayRunded) { return; }
            ClassDatalistCounter = 0;
            ClassData[] allClassDatas = dataBase.allClassesData.ToArray();
            for (int i = 0; i < allClassDatas.Length; i++)
            {
                AddClassDataToAssets(allClassDatas[i], true);
            }
            /*
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
            RoboPart[] parts = dataBase.allParts.ToArray();
            for (int i = 0; i < parts.Length; i++)
            {
                RoboPartlist[RoboPartlistCounter] = parts[i];
                RoboPartlistCounter++;
            }

            Sponsor[] sp = dataBase.allSponsor.ToArray();
            for (int i = 0; i < sp.Length; i++)
            {
                Sponserslist[SponserlistCounter] = sp[i];
                SponserlistCounter++;
            }

            MissionDefinition[] missions = dataBase.allMissions.ToArray();
            for (int i = 0; i < missions.Length; i++)
            {
                missionslist[missionslistCounter] = missions[i];
                missionslistCounter++;
            }


            Elixir[] elixirs = dataBase.allElixires.ToArray();
            for (int i = 0; i < elixirs.Length; i++)
            {
                Elixirlist[elixirs[i].IdNum] = elixirs[i];
            }
            RoboBase[] bases = dataBase.allBases.ToArray();
            for (int i = 0; i < bases.Length; i++)
            {
                RoboBaselist[bases[i].IdNum] = bases[i];
            }
            */
            DualString finded = dataBase.GameDataStrings.Find("GamePrefrance");
            if(finded!= null)
            {
                Statistics.GamePrefernceString = finded.value;
                //Statistics.SavedPreference = new JavaScriptSerializer().Deserialize<MatchCharestristic>(finded.value);
            }
            hostPlayers.Clear();
            IntArrayClass[] hosts = dataBase.HostPlayers.ToArray();
            for (int i = 0; i < hosts.Length; i++)
            {
                hostPlayers.Add(hosts[i].ar);
            }
            //dataBase.
            fillArrayRunded = true;
            

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

