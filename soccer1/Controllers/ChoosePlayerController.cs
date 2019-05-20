using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using soccer1.Models;
using soccer1.Models.utilites;
using soccer1.Models.main_blocks;
using soccer1.Models.DataBase;

namespace soccer1.Controllers
{



    public class ChoosePlayerController : Controller
    {
        [HttpPost]
        public string OtherPlayerTeam(FormCollection collection)
        {
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);

            string opponentId = new MatchList().ReturnOpponentOf(PlayerId, matchId);

            DataDBContext dataBase = new DataDBContext();
            PlayerForDatabase player = dataBase.playerInfoes.Find(opponentId);

            if (player != null)
            {
                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                TeamForConnectedPlayers opponentTeam = pl.ReturnYourTeam();
                TeamForSerialize opteam = new Convertors().TeamToTeamForSerialize(opponentTeam);
                string teamstring= new Convertors().TeamForSerializeToJson(opteam);
                //testtt test = new testtt();
                //test.CurrentFormation = opteam.CurrentFormation;

                return teamstring;

            }
            return "This Player Not Exist";
        }


        [HttpPost]
        public string OtherPlayerName(FormCollection collection)
        {
            string PlayerId = Request.Form["PlayerId"];
            int matchId = Int32.Parse(Request.Form["MatchId"]);

            string opponentId = new MatchList().ReturnOpponentOf(PlayerId, matchId);

            DataDBContext dataBase = new DataDBContext();
            PlayerForDatabase player = dataBase.playerInfoes.Find(opponentId);
            return player.Name;
        }


    }
    
}