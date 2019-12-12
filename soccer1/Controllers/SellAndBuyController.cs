﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using soccer1.Models;
using soccer1.Models.main_blocks;
using soccer1.Models.utilites;
using soccer1.Models.DataBase;
using System.Data.Entity;


namespace soccer1.Controllers
{
    public class SellAndBuyController : Controller
    {
        // GET: SellAndBuy
        private DataDBContext dataBase = new DataDBContext();
        [HttpPost]
        public string BuyAsset(FormCollection collection)
        {            
            string PlayerId = Request.Form["PlayerId"];
            int assetId =Int32.Parse( Request.Form["IdName"]);
            string AssetTypestring = Request.Form["AssetType"];
            bool result = false;
            //int IdCode =-1;
            int PlayerIndex=-1;

            
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            if(player != null)
            {
                AssetType assetType = new Utilities().ReturnAssetTypeByName(AssetTypestring);
                if(assetType == AssetType.Pawn)
                {                    
                    PlayerIndex = int.Parse(Request.Form["PlayerIndex"]);
                }
                else
                {

                }
                Property AssetPrice = new AssetManager().ReturnAssetPrice(assetType,assetId);
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                if(assetType == AssetType.Pawn) {
                    result = pl.BuyPawnAsset(assetId, PlayerIndex, AssetPrice );
                } else
                {
                    result = pl.BuyAsset(assetType, assetId, AssetPrice);
                }
                if (result)
                {
                    pl.SaveChanges();
                }
                /*
                if (result) {
                    player.changePlayer(pl.returnDataBaseVersion());
                    dataBase.Entry(player).State = EntityState.Modified;
                    dataBase.SaveChanges();
                }
                */

                //Log.AddLog("Error : reusult:" + result.ToString());
                return result.ToString();
            }
            return result.ToString();
        }


        [HttpPost]
        public string BuyOffer(FormCollection collection)
        {
            string PlayerId = Request.Form["PlayerId"];
            string OfferIdName = Request.Form["IdName"];
            
            bool result = false;

            
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            if (player != null)
            {
                //AssetType assetType = new Utilities().ReturnAssetTypeByName(AssetTypestring);
                Property AssetPrice = new AssetManager().ReturnOfferPrice(OfferIdName);
                Property BuyingMaterial = new AssetManager().ReturnOfferBuyingMaterial(OfferIdName);
                
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                result = pl.BuyOffer(BuyingMaterial , AssetPrice);
                if (result)
                {
                    pl.SaveChanges();
                }
                /*
                if (result)
                {
                    player.changePlayer(pl.returnDataBaseVersion());
                    dataBase.Entry(player).State = EntityState.Modified;
                    dataBase.SaveChanges();
                }
                */

                //Log.AddLog("Error : reusult:" + result.ToString());
                return result.ToString();
            }
            return result.ToString();
        }

        /*

        [HttpPost]
        public string BuyPawn(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            string PlayerId = Request.Form["PlayerId"];
            string IdName = Request.Form["IdName"];
            
            if (ConnectedPlayersList.IsConnectedById(ConnectionId, PlayerId))
            {
                Pawn pawn = AssetManager.RetrunPawnByIdName(IdName);
                Property playersProperty = ConnectedPlayersList.ReturnPropertyOfPlayer(ConnectionId);

                //cheching prices for buying
                if(utilities.CheckIfFirstPropertyIsBigger(playersProperty, pawn.price))
                {
                    ConnectedPlayersList.AddPawnToPlayer(ConnectionId, pawn.index);
                    ConnectedPlayersList.SubtractProperty(ConnectionId, pawn.price);
                }
                return true.ToString();
            }
            else
            {
                Log.AddLog("Error : Can Not Add Pawn");
                return false.ToString();                
            }
        }


        [HttpPost]
        public string BuyElixir(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            string PlayerId = Request.Form["PlayerId"];
            string IdName = Request.Form["IdName"];

            if (ConnectedPlayersList.IsConnectedById(ConnectionId, PlayerId))
            {
                Elixir elixir = AssetManager.ReturnElixirByIdName(IdName);
                Property playersProperty = ConnectedPlayersList.ReturnPropertyOfPlayer(ConnectionId);

                //cheching prices for buying
                if (utilities.CheckIfFirstPropertyIsBigger(playersProperty, elixir.price))
                {
                    ConnectedPlayersList.AddElixirToPlayer(ConnectionId, elixir.index);
                    ConnectedPlayersList.SubtractProperty(ConnectionId, elixir.price);
                }
                return true.ToString();
            }
            else
            {
                Log.AddLog("Error : Can Not Add Pawn");
                return false.ToString();
            }
        }


        [HttpPost]
        public string BuyFormation(FormCollection collection)
        {
            int ConnectionId = Int32.Parse(Request.Form["ConnectionId"]);
            string PlayerId = Request.Form["PlayerId"];
            string IdName = Request.Form["IdName"];

            if (ConnectedPlayersList.IsConnectedById(ConnectionId, PlayerId))
            {
                Formation form = AssetManager.ReturnFormationByIdName(IdName);
                Property playersProperty = ConnectedPlayersList.ReturnPropertyOfPlayer(ConnectionId);

                //cheching prices for buying
                if (utilities.CheckIfFirstPropertyIsBigger(playersProperty, form.price))
                {
                    ConnectedPlayersList.AddFormationToPlayer(ConnectionId, form.index);
                    ConnectedPlayersList.SubtractProperty(ConnectionId, form.price);
                }
                return true.ToString();
            }
            else
            {
                Log.AddLog("Error : Can Not Add Pawn");
                return false.ToString();
            }
        }

       */

    }
}