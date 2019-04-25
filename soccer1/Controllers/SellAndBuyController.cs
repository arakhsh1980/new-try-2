using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using soccer1.Models;
using soccer1.Models.main_blocks;
using soccer1.Models.utilites;
using System.Data.Entity;


namespace soccer1.Controllers
{
    public class SellAndBuyController : Controller
    {
        // GET: SellAndBuy
        
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

       

    }
}