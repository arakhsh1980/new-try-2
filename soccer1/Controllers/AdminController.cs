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
using soccer1.Models.utilites;

namespace soccer1.Controllers
{
    public class AdminController : Controller
    {
        public static Mutex AddPawnmutex = new Mutex();
        public static Mutex AddElixirmutex = new Mutex();
        public static Mutex AddFormationmutex = new Mutex();
        public static Mutex AddOffermutex = new Mutex();
        public static Mutex AddAdminControlermutex = new Mutex();
        private DataDBContext dataBase = new DataDBContext();


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
            return "done :";
        }



        [HttpPost]
        public string ReturnPlayerHistory(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddPawnmutex.WaitOne();
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                AddPawnmutex.ReleaseMutex();
                return "wrong Admin Code";
            }
            string playerId = Request.Form["playerId"];
            PlayerForDatabase player = dataBase.playerInfoes.Find(playerId);

            if (player != null)
            {
                AddPawnmutex.ReleaseMutex();
                return player.HistoryCode;
            }
            else
            {
                AddPawnmutex.ReleaseMutex();
                return "id not finded :";
            }
            

        }

        [HttpPost]
        public string ReturnListOfplayersThatLeft(FormCollection collection)
        {
            new AssetManager().LoadDataFromServerifitsFirstTime();
            AddPawnmutex.WaitOne();
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                AddPawnmutex.ReleaseMutex();
                return "wrong Admin Code";
            }
            int TimeFromlastConnection =int.Parse( Request.Form["TimeFromlastConnection"]);
            if (TimeFromlastConnection < 10)
            {
                AddPawnmutex.ReleaseMutex();
                return "invalidTime :";
            }
            List<string> leftedPlayers = new List<string>();
            int TimePointofNow = new Utilities().TimePointofNow();
            foreach (PlayerForDatabase p in dataBase.playerInfoes.ToList())
            {
                if(TimeFromlastConnection < TimePointofNow - p.LastConnectionTime)
                {
                    leftedPlayers.Add(p.id);
                }
            }
            if(leftedPlayers.Count == 0)
            {
                AddPawnmutex.ReleaseMutex();
                return "noOneleft :";
            }
            else
            {
                string res = leftedPlayers[0];
                for (int i = 1; i < leftedPlayers.Count; i++)
                {
                    res += "|"+leftedPlayers[i];
                }
                AddPawnmutex.ReleaseMutex();
                return res;
            }
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


        public class ShortArrayClass
        {
            public short[] ar;
        }



        [HttpPost]
        public string AddClassData(FormCollection collection)
        {
            AddAdminControlermutex.WaitOne();
            new AssetManager().LoadDataFromServerifitsFirstTime();            
            string AdminCode = Request.Form["AdminCode"];
            if (AdminCode != Statistics.AdminCode)
            {
                AddAdminControlermutex.ReleaseMutex();
                return "wrong Admin Code";
            }
            //AssetManager.assentsLoaded.WaitOne();
            ClassData newClassData = new ClassData();
            newClassData.nameCode = Request.Form["CodeName"];
            newClassData.innerData = Request.Form["innerData"];
            newClassData.type = Int32.Parse(Request.Form["type"]);
            new AssetManager().AddClassDataToAssets(newClassData, false);
            

            AddAdminControlermutex.ReleaseMutex();
            return "ClassData Added Loaded" + newClassData.nameCode;
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