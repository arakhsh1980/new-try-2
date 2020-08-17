using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using soccer1.Models;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using soccer1.Models.utilites;
using soccer1.Models.main_blocks;
using System.Threading;
using soccer1.Models.DataBase;
using System.Data.Entity;

namespace soccer1.Controllers
{
    public class ChallengeController : Controller
    {
        private DataDBContext dataBase = new DataDBContext();
        private static Mutex AddNew = new Mutex();
        // POST: Loading/LoadPlayerData
        [HttpPost]
        public string MakeChallengeOfGoalMemory(FormCollection collection)
        {
            bool AddChalenge(string input, bool is2StepChalaenge, string builderId )
            {
                if (input.Length < 1) { return false; }
                TwoStepGoalMemory sorce = new JavaScriptSerializer().Deserialize<TwoStepGoalMemory>(input);
                

                if (is2StepChalaenge)
                {
                    TwoStepChalenge newchal = new TwoStepChalenge();
                    newchal.endPositionsAfter = sorce.endPositionsAfter;
                    newchal.startPositions = sorce.startPositions;
                    newchal.OpponentShoot1 = sorce.plTwoShoot1;
                    newchal.OpponentShoot2 = sorce.plTwoShoot2;
                    newchal.isPlayerOneScoredAtEnd = sorce.isPlayerOneScoredAtEnd;
                    newchal.durablity = 100;
                    newchal.winChanse = 50;
                    newchal.BuilderId = builderId;
                    dataBase.twoStepChalenges.Add(newchal);
                    dataBase.SaveChanges();
                }
                else
                {
                    OneStepChalenge newchal = new OneStepChalenge();
                    newchal.startPositions = sorce.endPositionsAfter;
                    newchal.OpponentShoot1 = sorce.plTwoShoot2;
                    newchal.isPlayerOneScoredAtEnd = sorce.isPlayerOneScoredAtEnd;
                    newchal.durablity = 100;
                    newchal.winChanse = 50;
                    newchal.BuilderId = builderId;
                    dataBase.OneStepChalenges.Add(newchal);
                    dataBase.SaveChanges();
                }
                return true;
            }

            string id = Request.Form["PlayerId"];
            string goalNum = Request.Form["goalNum"];
            bool isTwoStepChalaenge = bool.Parse(Request.Form["isToStepChalaenge"]);
            new AssetManager().LoadDataFromServerifitsFirstTime();
            PlayerForDatabase player = dataBase.playerInfoes.Find(id);
            bool result = false;
            switch (goalNum)
            {
                case "0":
                    result = AddChalenge(player.GoalMemory0, isTwoStepChalaenge, id);
                    player.GoalMemory0 = "";
                    break;
                case "1":
                    result = AddChalenge(player.GoalMemory1, isTwoStepChalaenge, id);
                    player.GoalMemory1 = "";
                    break;
                case "2":
                    result = AddChalenge(player.GoalMemory2, isTwoStepChalaenge, id);
                    player.GoalMemory2 = "";
                    break;
                case "3":
                    result = AddChalenge(player.GoalMemory3, isTwoStepChalaenge, id);
                    player.GoalMemory3 = "";
                    break;
                case "4":
                    result = AddChalenge(player.GoalMemory4, isTwoStepChalaenge, id);
                    player.GoalMemory4= "";
                    break;
                default:
                    break;
            }
            dataBase.Entry(player).State = EntityState.Modified;
            dataBase.SaveChanges();

            
            return result.ToString();
        }

        /*
        [HttpPost]
        public string RequestAChalenge(FormCollection collection)
        {
           

            string id = Request.Form["PlayerId"];
            new AssetManager().LoadDataFromServerifitsFirstTime();
            PlayerForDatabase player = dataBase.playerInfoes.Find(id);

            dataBase.OneStepChalenges.Find()

            bool result = false;
            switch (goalNum)
            {
                case "0":
                    result = AddChalenge(player.GoalMemory0, isTwoStepChalaenge, id);
                    player.GoalMemory0 = "";
                    break;
                case "1":
                    result = AddChalenge(player.GoalMemory1, isTwoStepChalaenge, id);
                    player.GoalMemory1 = "";
                    break;
                case "2":
                    result = AddChalenge(player.GoalMemory2, isTwoStepChalaenge, id);
                    player.GoalMemory2 = "";
                    break;
                case "3":
                    result = AddChalenge(player.GoalMemory3, isTwoStepChalaenge, id);
                    player.GoalMemory3 = "";
                    break;
                case "4":
                    result = AddChalenge(player.GoalMemory4, isTwoStepChalaenge, id);
                    player.GoalMemory4 = "";
                    break;
                default:
                    break;
            }
            dataBase.Entry(player).State = EntityState.Modified;
            dataBase.SaveChanges();
            foreach (OneStepChalenge st in dataBase.OneStepChalenges)if()
            {

            }

            return result.ToString();
        }
        */


    }
}