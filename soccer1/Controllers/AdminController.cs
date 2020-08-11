using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using soccer1.Models;
using System.Web.Script.Serialization;
using soccer1.Models.main_blocks;
using System.Threading;
using soccer1.Models.DataBase;

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
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                AddPawnmutex.ReleaseMutex();
                return "wrong Admin Code" ;
            }
                //AssetManager.assentsLoaded.WaitOne();
                RoboPart newpart = new RoboPart();
            //pawn.abilityShower= Request.Form["abilityShower"];            
            newpart.HighGoldEffect = Int32.Parse(Request.Form["HighGoldEffect"]);
            short IdNum = short.Parse(Request.Form["IdNum"]);
            newpart.partType = (RoboPartType)IdNum;
            newpart.lowGoldEffect= Int32.Parse(Request.Form["lowGoldEffect"]);
            newpart.MediomGoldEffect = Int32.Parse(Request.Form["MediomGoldEffect"]);            
            //Enum.TryParse<RoboPartType>(Request.Form["partType"],out tt);
            newpart.key =  newpart.partType.ToString();
            Property price = new Property();
            price.Alminum = Int32.Parse(Request.Form["price.coin"]);
            price.fan = Int32.Parse(Request.Form["price.fan"]);
            price.tropy = Int32.Parse(Request.Form["price.level"]);
            price.gold = Int32.Parse(Request.Form["price.SoccerSpetial"]);

            Property BluePrintprice = new Property();
            BluePrintprice.Alminum = Int32.Parse(Request.Form["BluePrintPrice.coin"]);
            BluePrintprice.fan = Int32.Parse(Request.Form["BluePrintPrice.fan"]);
            BluePrintprice.tropy = Int32.Parse(Request.Form["BluePrintPrice.level"]);
            BluePrintprice.gold = Int32.Parse(Request.Form["BluePrintPrice.SoccerSpetial"]);
            newpart.numberOfBuildableParts = Int32.Parse(Request.Form["numberOfBuildableParts"]);
            newpart.BluePrintAlmimunValue = BluePrintprice.Alminum;
            newpart.GoldValue= BluePrintprice.gold;
            //newpart.price = price;
            newpart.GoldValue = price.gold;
            newpart.AlmimunValue = price.Alminum;
            newpart.minuetToBuild = Int32.Parse(Request.Form["minuteToBuild"]);
            new AssetManager().AddRoboPartToAssets(newpart);
            AddPawnmutex.ReleaseMutex();            
            return "RoboPart Loaded" + newpart.partType.ToString();            
        }

        /*
        [HttpPost]
        public string AddOrUpdateAnString(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddPawnmutex.WaitOne();
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                AddPawnmutex.ReleaseMutex();
                return "wrong Admin Code";
            }
            string StringLastName = Request.Form["StringLastName"];
            string StringNewName = Request.Form["StringNewName"];
            string newdata = Request.Form["newdata"];           

            if (newdata != null)
            {

                DualString finded = dataBase.GameDataStrings.Find(StringLastName);
                if (finded != null)
                {
                    finded.key = StringNewName;
                    finded.TimeOfLastUpdate = DateTime.UtcNow.ToBinary();
                    finded.value = newdata;
                }
                else
                {
                    DualString DataForsave = new DualString();
                    DataForsave.key = StringNewName;
                    DataForsave.TimeOfLastUpdate = DateTime.UtcNow.ToBinary();
                    DataForsave.value = newdata;
                    dataBase.GameDataStrings.Add(DataForsave);
                }
                dataBase.SaveChanges();
                DualString[] allstrings = dataBase.GameDataStrings.ToArray();
                string newlist= "";
                for (int i = 0; i < allstrings.Length; i++)if(allstrings[i].key != "ListOfStrings")
                {
                        newlist +=  allstrings[i].key+ "^"+allstrings[i].TimeOfLastUpdate.ToString()+"*";
                }
                DualString liststring = dataBase.GameDataStrings.Find("ListOfStrings");
                liststring.value = newlist;
                liststring.TimeOfLastUpdate = DateTime.UtcNow.ToBinary();
                //dataBase.GameDataStrings.
                dataBase.SaveChanges();
            }
            else
            {

            }
            AddPawnmutex.ReleaseMutex();
            return "done";
        }

        */


        [HttpPost]
        public string UpdateAnPlanetData(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddPawnmutex.WaitOne();
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                AddPawnmutex.ReleaseMutex();
                return "wrong Admin Code";
            }
            string newdata = Request.Form["PlanetData"];
            string part2 = Request.Form["PlanetName"];
            string nameCode = "PlanetData" + part2;

            if (newdata != null)
            {
                
                DualString finded = dataBase.GameDataStrings.Find(nameCode);
                if (finded != null)
                {
                    finded.value = newdata;
                }
                else
                {
                    DualString DataForsave = new DualString();
                    DataForsave.key = nameCode;
                    DataForsave.value = newdata;
                    dataBase.GameDataStrings.Add(DataForsave);
                }
                //dataBase.GameDataStrings.
                dataBase.SaveChanges();
            }
            else
            {

            }
            AddPawnmutex.ReleaseMutex();
            return "done";
        }



        [HttpPost]
        public string UpdateGameData(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddPawnmutex.WaitOne();
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                AddPawnmutex.ReleaseMutex();
                return "wrong Admin Code";
            }
            string newdata = Request.Form["GameData"];
            
            if (newdata != null)
            {               
                DualString finded = dataBase.GameDataStrings.Find("GameData");
                if (finded != null)
                {
                    finded.value = newdata;
                    DualString dataVersion = dataBase.GameDataStrings.Find("GameDataVersion");
                    int dataVersionnum = Int32.Parse(dataVersion.value);
                    dataVersion.value = (dataVersionnum + 1).ToString();                    
                }
                else
                {
                    DualString DataForsave = new DualString();
                    DataForsave.key = "GameData";
                    DataForsave.value = newdata;
                    dataBase.GameDataStrings.Add(DataForsave);
                    DualString dataVersion = new DualString();
                    dataVersion.key = "GameDataVersion";
                    dataVersion.value = 0.ToString();
                    dataBase.GameDataStrings.Add(dataVersion);
                }
                //dataBase.GameDataStrings.
                dataBase.SaveChanges();
                Statistics.GameDataVersion++;
            }
           
            AddPawnmutex.ReleaseMutex();
            return "done :" ;
        }


        [HttpPost]
        public string UpdateGamePreference(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddPawnmutex.WaitOne();
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                AddPawnmutex.ReleaseMutex();
                return "wrong Admin Code";
            }
            string newdata = Request.Form["GamePrefranceString"];
            string preferenceName = Request.Form["preferenceName"];
            string nameCode = "GamePrefrance:" + preferenceName;
            if (newdata != null)
            {
                Statistics.GamePrefernceString = newdata;
                // string yy = newdata.Replace('*', '"');
                //Statistics.todayMatchGround = new JavaScriptSerializer().Deserialize<MatchCharestristic>(yy);
                DualString finded = dataBase.GameDataStrings.Find("GamePrefrance");
                if (finded != null)
                {
                    finded.value = newdata;
                }
                else
                {
                    DualString DataForsave = new DualString();
                    DataForsave.key = nameCode;
                    DataForsave.value = newdata;
                    dataBase.GameDataStrings.Add(DataForsave);
                }
                //dataBase.GameDataStrings.
                dataBase.SaveChanges();
            }
            else
            {

            }
            AddPawnmutex.ReleaseMutex();
            return "done";
        }

        [HttpPost]
        public string UpdateNewMatchcharestristic(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddPawnmutex.WaitOne();
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                AddPawnmutex.ReleaseMutex();
                return "wrong Admin Code";
            }
            string NewMatchchar = Request.Form["NewMatchcharestristic"];
            if (NewMatchchar != null)
            {

                //Statistics.todayMatchGround = new JavaScriptSerializer().Deserialize<MatchCharestristic>(yy);
                DualString finded = dataBase.GameDataStrings.Find("NewMatchchar");
                if (finded != null)
                {
                    finded.value = NewMatchchar;
                }
                else
                {
                    DualString DataForsave = new DualString();
                    DataForsave.key = "NewMatchchar";
                    DataForsave.value = NewMatchchar;
                    dataBase.GameDataStrings.Add(DataForsave);
                }
                //dataBase.GameDataStrings.
                dataBase.SaveChanges();
            }
            else
            {

            }
            AddPawnmutex.ReleaseMutex();
            return "done";
        }


        [HttpPost]
        public string AddSponser(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddPawnmutex.WaitOne();
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                AddPawnmutex.ReleaseMutex();
                return "wrong Admin Code";
            }
            //AssetManager.assentsLoaded.WaitOne();
            Sponsor newsp = new Sponsor();
            //pawn.abilityShower= Request.Form["abilityShower"];
            newsp.MaxTickets = Int32.Parse(Request.Form["MaxTickets"]);
            newsp.goldPerWin = Int32.Parse(Request.Form["goldPerWin"]);
            newsp.MaxTickets = Int32.Parse(Request.Form["MaxTickets"]);
            newsp.minAcceptableTropy = Int32.Parse(Request.Form["minAcceptableTropy"]);
            newsp.minAcceptableXp = Int32.Parse(Request.Form["minAcceptableXp"]);
            newsp.name = Request.Form["name"];
            newsp.prerequisite = Request.Form["prerequisite"];
            newsp.prerequisiteCode = Request.Form["prerequisiteCode"];
            newsp.TimeBetweenTickets = Int32.Parse(Request.Form["TimeBetweenTickets"]);
            new AssetManager().AddSponserToAssets(newsp);
            AddPawnmutex.ReleaseMutex();
            return "Sponser saved :" + newsp.name;
        }


        public class ShortArrayClass
        {
            public short[] ar;
        }


        [HttpPost]
        public string AddMissionToDatabase(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddPawnmutex.WaitOne();
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                AddPawnmutex.ReleaseMutex();
                return "wrong Admin Code";
            }


            //AssetManager.assentsLoaded.WaitOne();
            MissionDefinition newMission = new MissionDefinition();
            //pawn.abilityShower= Request.Form["abilityShower"];            
            newMission.IdNum = short.Parse(Request.Form["IdNum"]);
            string shortsss = Request.Form["preRequisite"];
            string[] shotrss = shortsss.Split('*');

            if (1 < shotrss.Length)
            {
                short[] inputs = new short[shotrss.Length - 1];
                for (int i = 0; i < shotrss.Length - 1; i++)
                {
                    inputs[i] = short.Parse(shotrss[i]);
                }
                newMission.preRequisite = new JavaScriptSerializer().Serialize(inputs);
            }
            else
            {
                newMission.preRequisite = "";
            }

            //ShortArrayClass prereq =(ShortArrayClass) new JavaScriptSerializer().DeserializeObject(shortsss);            

            newMission.rewardInGold = Int32.Parse(Request.Form["rewardInGold"]);

            newMission.key = newMission.IdNum.ToString();

            new AssetManager().AddMissionsToAssets(newMission);
            AddPawnmutex.ReleaseMutex();
            return "Mission Loaded" + newMission.key + " " + newMission.IdNum;
        }


        [HttpPost]
        public string AddRoboBase(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddPawnmutex.WaitOne();
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                AddPawnmutex.ReleaseMutex();
                return "wrong Admin Code";
            }

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
            newRoboBase.SpaceForAddOn = short.Parse(Request.Form["SpaceForAddOn"]);
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
            return "Base Loaded" + newRoboBase.IdNum;
        }


        [HttpPost]
        public string AddElixir(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddElixirmutex.WaitOne();
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                AddElixirmutex.ReleaseMutex();
                return "wrong Admin Code";
            }
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
            price.tropy = Int32.Parse(Request.Form["price.level"]);
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
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                AddFormationmutex.ReleaseMutex();
                return "wrong Admin Code";
            }
            //AssetManager.assentsLoaded.WaitOne();
            Formation formation = new Formation();
            formation.discription = Request.Form["discription"];
            formation.IdNum = Int32.Parse(Request.Form["IdNum"]);
            formation.key = formation.IdNum.ToString();
            formation.showName = Request.Form["showName"];

            Property price = new Property();
            price.Alminum = Int32.Parse(Request.Form["price.coin"]);
            price.fan = Int32.Parse(Request.Form["price.fan"]);
            price.tropy = Int32.Parse(Request.Form["price.level"]);
            price.gold = Int32.Parse(Request.Form["price.SoccerSpetial"]);
            formation.price = price;
            formation.positions = new PawnStartPosition[5];
            for (int i = 0; i < formation.positions.Length; i++)
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
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                AddOffermutex.ReleaseMutex();
                return "wrong Admin Code";
            }
            //AssetManager.assentsLoaded.WaitOne();
            Offer newOffer = new Offer();
            newOffer.BuyedmoneyAmount = Int32.Parse(Request.Form["BuyedmoneyAmount"]);
            newOffer.BuyedmoneyType = Int32.Parse(Request.Form["BuyedmoneyType"]);
            newOffer.IdName = Request.Form["IdName"];
            newOffer.realDollerPrice = Int32.Parse(Request.Form["realDollerPrice"]);
            Property price = new Property();
            price.Alminum = Int32.Parse(Request.Form["price.coin"]);
            price.fan = Int32.Parse(Request.Form["price.fan"]);
            price.tropy = Int32.Parse(Request.Form["price.level"]);
            price.gold = Int32.Parse(Request.Form["price.SoccerSpetial"]);
            newOffer.price = price;

            new AssetManager().AddOfferToAssets(newOffer);
            AddOffermutex.ReleaseMutex();
            return "Formation Loaded" + newOffer.IdName;
        }






        [HttpPost]
        public string ResetAllPlayersInfos(FormCollection collection)
        {
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                return "wrong Admin Code";
            }
            foreach (PlayerForDatabase p in dataBase.playerInfoes)
            {
                dataBase.playerInfoes.Remove(p);
            }
            dataBase.SaveChanges();
            return true.ToString();
        }

        #region host players

        [HttpPost]
        public string AddRemoveHostPlayer(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddOffermutex.WaitOne();
            string AdminCode = Request.Form["AdminCode"];
            int HostPlayerID = int.Parse(Request.Form["HostPlayerID"]);
            bool BoolVal = bool.Parse(Request.Form["BoolVal"]);

            bool isthisAlredyAFost = AssetManager.hostPlayers.Contains(HostPlayerID);
            if (AdminCode != Statistics.AdminCode)
            {
                AddOffermutex.ReleaseMutex();
                return "wrong Admin Code";
            }
            if (BoolVal)
            {
                if (isthisAlredyAFost)
                {
                    AddOffermutex.ReleaseMutex();
                    return "this Player alredy is a host";

                }
                else
                {
                    IntArrayClass newintar = new IntArrayClass();
                    newintar.ar = HostPlayerID;
                    newintar.key = dataBase.HostPlayers.Count<IntArrayClass>() + 1;
                    dataBase.HostPlayers.Add(newintar);
                    dataBase.SaveChanges();
                    AssetManager.hostPlayers.Add(HostPlayerID);
                    AddOffermutex.ReleaseMutex();
                    return "Done";
                }

            }
            else
            {
                if (isthisAlredyAFost)
                {
                    IntArrayClass findedPlayer = null;
                    IntArrayClass[] allhosts = dataBase.HostPlayers.ToArray();
                    for (int i = 0; i < allhosts.Length; i++) if (allhosts[i].ar == HostPlayerID)
                        {
                            findedPlayer = allhosts[i];
                        }
                    dataBase.HostPlayers.Remove(findedPlayer);
                    dataBase.SaveChanges();
                    AssetManager.hostPlayers.Remove(HostPlayerID);
                    AddOffermutex.ReleaseMutex();
                    return "Done";

                }
                else
                {
                    AddOffermutex.ReleaseMutex();
                    return "player not find";
                }
            }

        }


        [HttpPost]
        public string readAllHosts(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddOffermutex.WaitOne();
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                AddOffermutex.ReleaseMutex();
                return "wrong Admin Code";
            }
            string res = "";
            foreach (int plId in AssetManager.hostPlayers)
            {
                res += plId.ToString() + " . ";
            }
            AddOffermutex.WaitOne();
            return res;
        }



        #endregion

    }

}