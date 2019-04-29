﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using soccer1.Models;
using System.Web.Script.Serialization;
using soccer1.Models.main_blocks;
using System.Threading;

namespace soccer1.Controllers
{
    public class AdminController : Controller
    {
        private static Mutex AddPawnmutex = new Mutex();
        private static Mutex AddElixirmutex = new Mutex();
        private static Mutex AddFormationmutex = new Mutex();

        [HttpPost]
        public string AddPawn(FormCollection collection)
        {
            //AddPawnmutex.WaitOne();
            Pawn pawn = new Pawn();
            pawn.abilityShower= Request.Form["abilityShower"];
            pawn.ForMatch = Request.Form["ForMatch"];
            pawn.blueForSale = Request.Form["blueForSale"];
            pawn.IdName = Request.Form["IdName"];
            PawnAbility pa = new PawnAbility();
            pa.aiming = Int32.Parse(Request.Form["mainAbility.aiming"]);
            pa.boddyMass = Int32.Parse(Request.Form["mainAbility.boddyMass"]);
            pa.endorance = Int32.Parse(Request.Form["mainAbility.endorance"]);
            pa.shootPower = Int32.Parse(Request.Form["mainAbility.shootPower"]);            
            pawn.mainAbility = pa;
            Property price = new Property();
            price.coin= Int32.Parse(Request.Form["price.coin"]);
            price.fan = Int32.Parse(Request.Form["price.fan"]);
            price.level = Int32.Parse(Request.Form["price.level"]);
            price.SoccerSpetial = Int32.Parse(Request.Form["price.SoccerSpetial"]);
            pawn.price = price;
            pawn.redForSale= Request.Form["redForSale"];
            pawn.ShowName = Request.Form["redForSale"];
            //Log.AddLog("AddPawn" + pawn.IdName);
            AssetManager.AddPawnToAssets(pawn);
            //AddPawnmutex.ReleaseMutex();
            return "Pawn Loaded"+ pawn.IdName;            
        }

        [HttpPost]
        public void AddElixir(FormCollection collection)
        {
            //AddElixirmutex.WaitOne(); 
            Elixir elixir = new Elixir();
            elixir.forSale = Request.Form["forSale"];            
            elixir.IdName = Request.Form["IdName"];
            elixir.showName = Request.Form["showName"];
            SpetialPower sp = new SpetialPower();
            sp.IdName = Request.Form["spPower.IdName"];
            sp.image = Request.Form["spPower.image"];
            sp.scribtion = Request.Form["spPower.scribtion"];
            sp.ShowName = Request.Form["spPower.ShowName"];
            elixir.spPower = sp;
            Property price = new Property();
            price.coin = Int32.Parse(Request.Form["price.coin"]);
            price.fan = Int32.Parse(Request.Form["price.fan"]);
            price.level = Int32.Parse(Request.Form["price.level"]);
            price.SoccerSpetial = Int32.Parse(Request.Form["price.SoccerSpetial"]);
            elixir.price = price;
            AssetManager.AddElixirToAssets(elixir);
            //AddElixirmutex.WaitOne();
        }

        [HttpPost]
        public void AddFormation(FormCollection collection)
        {
            //AddFormationmutex.WaitOne();
            Formation formation = new Formation();
            formation.discription = Request.Form["discription"];
            formation.IdName = Request.Form["IdName"];
            formation.showName = Request.Form["showName"];
                       
            Property price = new Property();
            price.coin = Int32.Parse(Request.Form["price.coin"]);
            price.fan = Int32.Parse(Request.Form["price.fan"]);
            price.level = Int32.Parse(Request.Form["price.level"]);
            price.SoccerSpetial = Int32.Parse(Request.Form["price.SoccerSpetial"]);
            formation.price = price;
            formation.positions = new PawnStartPosition[5];
            for (int i=0; i< formation.positions.Length; i++)
            {
                formation.positions[i].x = Int32.Parse(Request.Form["positions" + i.ToString() + "x"]);
                formation.positions[i].y = Int32.Parse(Request.Form["positions" + i.ToString() + "y"]);
            }

            AssetManager.AddFormationToAssets(formation);
            //AddElixirmutex.WaitOne();
        }

    }
}