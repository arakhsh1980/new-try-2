using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using soccer1.Models;
using System.Web.Script.Serialization;
using soccer1.Models.main_blocks;
using System.Threading;
using System.Data.Entity;
using soccer1.Models.main_blocks;
using System.ComponentModel.DataAnnotations;
using soccer1.Models.utilites;
using soccer1.Models.DataBase;
using System.Web.Script.Serialization;
using System.Threading;

namespace soccer1.Controllers
{
    public class AdminController : Controller
    {
        public static Mutex AddPawnmutex = new Mutex();
        public static Mutex AddElixirmutex = new Mutex();
        public static Mutex AddFormationmutex = new Mutex();
        public static Mutex AddOffermutex = new Mutex();
        private DataDBContext dataBase = new DataDBContext();

        [HttpPost]
        public string AddRoboPart(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddPawnmutex.WaitOne();
            
            //AssetManager.assentsLoaded.WaitOne();
            RoboPart newpart = new RoboPart();
            //pawn.abilityShower= Request.Form["abilityShower"];            
            newpart.HighGoldEffect = Int32.Parse(Request.Form["HighGoldEffect"]);
            newpart.IdNum = short.Parse(Request.Form["IdNum"]);            
            newpart.lowGoldEffect= Int32.Parse(Request.Form["lowGoldEffect"]);
            newpart.MediomGoldEffect = Int32.Parse(Request.Form["MediomGoldEffect"]);
            RoboPartType tt;
            Enum.TryParse<RoboPartType>(Request.Form["partType"],out tt);
            newpart.partType = tt;
           newpart.key = (newpart.IdNum + (int)tt *256).ToString();
            Property price = new Property();
            price.Alminum = Int32.Parse(Request.Form["price.coin"]);
            price.fan = Int32.Parse(Request.Form["price.fan"]);
            price.level = Int32.Parse(Request.Form["price.level"]);
            price.gold = Int32.Parse(Request.Form["price.SoccerSpetial"]);
            //newpart.price = price;
            newpart.GoldValue = price.gold;
            newpart.AlmimunValue = price.Alminum;
            newpart.minuetToBuild =(int)Math.Ceiling( Int32.Parse(Request.Form["secondToBuild"])/60.0f);
            new AssetManager().AddRoboPartToAssets(newpart);
            AddPawnmutex.ReleaseMutex();            
            return "Pawn Loaded"+ newpart.IdNum;            
        }


        [HttpPost]
        public string AddRoboBase(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddPawnmutex.WaitOne();

            //AssetManager.assentsLoaded.WaitOne();
            RoboBase newRoboBase = new RoboBase();
            //pawn.abilityShower= Request.Form["abilityShower"];
            newRoboBase.IdNum = short.Parse(Request.Form["IdNum"]);
            newRoboBase.level = short.Parse(Request.Form["level"]);
            newRoboBase.requiredXpToUpgrade = Int32.Parse(Request.Form["requiredXpToUpgrade"]);
            newRoboBase.upgradeToId1 = short.Parse(Request.Form["upgradeToId1"]);
            newRoboBase.upgradeToId2 = short.Parse(Request.Form["upgradeToId2"]);
            newRoboBase.key = newRoboBase.IdNum.ToString();
            newRoboBase.upgradeToId3 = short.Parse(Request.Form["upgradeToId3"]);
            //Log.AddLog("AddPawnStarted" + pawn.IdName);
            PawnAbility mainability = new PawnAbility();
            mainability.aiming = float.Parse(Request.Form["mainAbility.aiming"]);
            mainability.boddyMass = float.Parse(Request.Form["mainAbility.boddyMass"]);
            mainability.endorance = float.Parse(Request.Form["mainAbility.endorance"]);
            mainability.shootPower = float.Parse(Request.Form["mainAbility.shootPower"]);
            mainability.force = float.Parse(Request.Form["mainAbility.force"]);
            newRoboBase.mainAbility = mainability;
            //pawn.ShowName = Request.Form["redForSale"];            
            new AssetManager().AddRoboBaseToAssets(newRoboBase);
            AddPawnmutex.ReleaseMutex();
            return "Pawn Loaded" + newRoboBase.IdNum;
        }


        [HttpPost]
        public string AddElixir(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddElixirmutex.WaitOne();
            //AssetManager.assentsLoaded.WaitOne();
            Elixir elixir = new Elixir();
            elixir.forSale = Request.Form["forSale"];            
            elixir.IdNum = Int32.Parse(Request.Form["IdNum"]);
            elixir.key = elixir.IdNum.ToString();
            elixir.showName = Request.Form["showName"];
            //Log.AddLog("AddElixirStarted" + elixir.IdName);
            SpetialPower sp = new SpetialPower();
            sp.IdName = Request.Form["spPower.IdName"];
            sp.image = Request.Form["spPower.image"];
            sp.scribtion = Request.Form["spPower.scribtion"];
            sp.ShowName = Request.Form["spPower.ShowName"];
            elixir.spPower = sp;
            Property price = new Property();
            price.Alminum = Int32.Parse(Request.Form["price.coin"]);
            price.fan = Int32.Parse(Request.Form["price.fan"]);
            price.level = Int32.Parse(Request.Form["price.level"]);
            price.gold = Int32.Parse(Request.Form["price.SoccerSpetial"]);
            elixir.price = price;
            new AssetManager().AddElixirToAssets(elixir);
            AddElixirmutex.ReleaseMutex();           
            return "Elixir Loaded" + elixir.IdNum;
        }

        [HttpPost]
        public string AddFormation(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddFormationmutex.WaitOne();
            //AssetManager.assentsLoaded.WaitOne();
            Formation formation = new Formation();
            formation.discription = Request.Form["discription"];
            formation.IdNum = Int32.Parse(Request.Form["IdNum"]);
            formation.key = formation.IdNum.ToString();
            formation.showName = Request.Form["showName"];
                       
            Property price = new Property();
            price.Alminum = Int32.Parse(Request.Form["price.coin"]);
            price.fan = Int32.Parse(Request.Form["price.fan"]);
            price.level = Int32.Parse(Request.Form["price.level"]);
            price.gold = Int32.Parse(Request.Form["price.SoccerSpetial"]);
            formation.price = price;
            formation.positions = new PawnStartPosition[5];
            for (int i=0; i< formation.positions.Length; i++)
            {
                formation.positions[i].x = Int32.Parse(Request.Form["positions" + i.ToString() + "x"]);
                formation.positions[i].y = Int32.Parse(Request.Form["positions" + i.ToString() + "y"]);
            }

            new AssetManager().AddFormationToAssets(formation);
            AddFormationmutex.ReleaseMutex();
            return "Formation Loaded" + formation.IdNum;
        }


        [HttpPost]
        public string AddOffer(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddOffermutex.WaitOne();
            //AssetManager.assentsLoaded.WaitOne();
            Offer newOffer = new Offer();            
            newOffer.BuyedmoneyAmount = Int32.Parse(Request.Form["BuyedmoneyAmount"]);
            newOffer.BuyedmoneyType = Int32.Parse(Request.Form["BuyedmoneyType"]);
            newOffer.IdName = Request.Form["IdName"];
            newOffer.realDollerPrice = Int32.Parse(Request.Form["realDollerPrice"]);
            Property price = new Property();
            price.Alminum = Int32.Parse(Request.Form["price.coin"]);
            price.fan = Int32.Parse(Request.Form["price.fan"]);
            price.level = Int32.Parse(Request.Form["price.level"]);
            price.gold = Int32.Parse(Request.Form["price.SoccerSpetial"]);
            newOffer.price = price;

            new AssetManager().AddOfferToAssets(newOffer);
            AddOffermutex.ReleaseMutex();
            return "Formation Loaded" + newOffer.IdName;
        }




        [HttpPost]
        public string UpdateGamePreference(FormCollection collection)
        {
            string GamePrefranceString = Request.Form["GamePrefranceString"];
            Statistics.BasePrefrance = GamePrefranceString;
            return true.ToString();
        }

        [HttpPost]
        public string ResetAllPlayersInfos(FormCollection collection)
        {
            
            foreach(PlayerForDatabase p in dataBase.playerInfoes)
            {
                dataBase.playerInfoes.Remove(p);
            }
            dataBase.SaveChanges();
            return true.ToString();
        }




    }
}