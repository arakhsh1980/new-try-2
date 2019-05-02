using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models.main_blocks;
using soccer1.Models.utilites;
using System.Data.Entity;
using System.Web.Mvc;
using soccer1;



namespace soccer1.Models
{

    //class ConnectedPlayersList
    //{
    //    private class eventtt
    //    {
    //        public int ConnectedId = -1;
    //        public string eventBody = "";
    //        public bool isReal = false;
    //    }


    //    private static playerCennectionInfo[] connectionInfos = new playerCennectionInfo[Statistics.ActiveMatchesMaxNumber];

    //    private static PlayerForConnectedPlayer[] connectedPlayers = new PlayerForConnectedPlayer[Statistics.ActiveMatchesMaxNumber];

    //    public readonly static Player[] PlayersInfo = new Player[Statistics.ActiveMatchesMaxNumber];

    //    private static eventtt[] eventStogage = new eventtt[Statistics.ActiveMatchesMaxNumber];

    //    #region return functions


    //    public DateTime LastTimeConection(int playerId)
    //    {
    //        DateTime time = new DateTime();
    //        time = connectionInfos[playerId].lastTimeConnecttion;
    //        return time;
    //    }

    //    public TeamForConnectedPlayers ReturnPlayerTeam(int connectionId)
    //    {
    //        TeamForConnectedPlayers team = connectedPlayers[connectionId].ReturnYourTeam();
    //        return team;
    //    }
    //    public static int ReturnPlayerActiveMatch(int connectionId)
    //    {
    //        int match = connectionInfos[connectionId].ActiveMatchId;
    //        int match = connectionInfos[connectionId].ActiveMatchId;
    //        return match;
    //    }

    //    public float ReturnPlayerPowerLevel(int connectionId)
    //    {
    //        return connectedPlayers[connectionId].PowerLevel;
    //    }

    //    public string ReturnIdbyConnId(int ConnId)
    //    {
    //        if (connectedPlayers[ConnId] == null) { return ""; }
    //        else { return connectedPlayers[ConnId].id; }
    //    }

    //    public DateTime ReturnPlayerConnectionTime(int playerId)
    //    {
    //        if (connectionInfos[playerId] == null) { return DateTime.MinValue; }
    //        else
    //        {
    //            return connectionInfos[playerId].ConnecttionTime;
    //        }
    //    }

    //    #endregion



    //    /*
    //    public static void AddPawnToPlayer(int ConnectionId, int pawnIndex)
    //    {
    //        connectedPlayers[ConnectionId].pawnOutOfTeam[connectedPlayers[ConnectionId].pawnOutOfTeamCounter] = pawnIndex;
    //        connectedPlayers[ConnectionId].pawnOutOfTeamCounter++;            
    //    }
    //    */
    //    public bool BuyAssetForPlayer(int ConnectionId, AssetType type, string AssetName)
    //    {
    //        Property price = AssetManager.ReturnAssetPrice(type, AssetName);
    //        Log.AddLog("Error : price of asset:" + price);
    //        return connectedPlayers[ConnectionId].BuyAsset(type, AssetName, price, ConnectionId);

    //    }

    //    /*
    //    public static void AddElixirToPlayer(int ConnectionId, int ElixirIndex)
    //    {
    //        connectedPlayers[ConnectionId].elixirOutOfTeam[connectedPlayers[ConnectionId].elixirOutOfTeamCounter] = ElixirIndex;
    //        connectedPlayers[ConnectionId].elixirOutOfTeamCounter++;
    //    }
    //    */

    //    /*
    //public static void AddFormationToPlayer(int ConnectionId, int FormationIndex)
    //{
    //    connectedPlayers[ConnectionId].team.AddToUsableFormations(FormationIndex) ;
    //}
    //*/

    //    public void SubtractProperty(int ConnectionId, Property prop)
    //    {
    //        connectedPlayers[ConnectionId].SubtractProperty(prop);
    //    }


    //    public bool IsConnected(int ConnectionId)
    //    {
    //        if (!connectionInfos[ConnectionId].connected)
    //        {
    //            CleanId(ConnectionId);
    //            return false;
    //        }

    //        if (ConnectionTimeOut(ConnectionId))
    //        {
    //            CleanId(ConnectionId);
    //            return false;
    //        }
    //        else
    //        {

    //            return true;
    //        }

    //    }

    //    public bool IsConnectedById(int ConnectionId, string PlayerId)
    //    {
    //        if (!connectionInfos[ConnectionId].connected)
    //        {
    //            CleanId(ConnectionId);
    //            return false;
    //        }

    //        if (ConnectionTimeOut(ConnectionId))
    //        {
    //            CleanId(ConnectionId);
    //            return false;
    //        }
    //        if (connectionInfos[ConnectionId].PlayerId == PlayerId)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            CleanId(ConnectionId);
    //            return false;
    //        }
    //    }

    //    public bool IsConnectedByIdAndMatch(int ConnectionId, string PlayerId, int matchNum)
    //    {
    //        if (IsConnectedById(ConnectionId, PlayerId) && connectionInfos[ConnectionId].ActiveMatchId == matchNum)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            CleanId(ConnectionId);
    //            return false;
    //        }
    //    }

    //    /*
    //    public static Player ACopyOfPlayerInformation(int ConnectionId)
    //    {
    //        Player thisplayer = new Player();
    //        thisplayer.Coin = connectedPlayers[ConnectionId].Coin;
    //        thisplayer.connectedId = connectedPlayers[ConnectionId].connectedId;
    //        thisplayer.CurrentFormation = connectedPlayers[ConnectionId].CurrentFormation;            
    //        thisplayer.Fan = connectedPlayers[ConnectionId].Fan;
    //        thisplayer.id = connectedPlayers[ConnectionId].id;
    //        thisplayer.level = connectedPlayers[ConnectionId].level;
    //        thisplayer.Name = connectedPlayers[ConnectionId].Name;
    //        thisplayer. = connectedPlayers[ConnectionId].id;

    //        return thisplayer;
    //    }
    //    */


    //    #region event functions
    //    public static string ReadPlayerEvent(int ConnectionId)
    //    {
    //        if (connectedPlayersList[ConnectionId] == null) { connectedPlayersList[ConnectionId] = new playerCennectionInfo(); }
    //        connectionInfos[ConnectionId].lastTimeConnecttion = DateTime.Now;
    //        string st = "";
    //        st = connectionInfos[ConnectionId].EventMassage;
    //        string TypePart = st.Substring(0, 10);
    //        connectionInfos[ConnectionId].EventMassage = ServrMasage.NothingNew.ToString();
    //        if (connectionInfos[ConnectionId].HaveAdditionalEventMassage)
    //        {
    //            connectionInfos[ConnectionId].EventMassage = ReadEventFromEventStorg(ConnectionId);
    //        }
    //        return st;
    //    }

    //    public static void AddPlayerEvent(int ConnectionId, ServrMasage massageType, string eventMassage)
    //    {

    //        if (massageType == ServrMasage.NothingNew) { return; }
    //        if (connectionInfos[ConnectionId].EventMassage == ServrMasage.Disconcted.ToString()) { return; }
    //        Log.AddPlayerLog(ConnectionId, " adding event:  " + massageType.ToString() + "st: " + eventMassage);
    //        if (connectionInfos[ConnectionId].EventMassage == ServrMasage.NothingNew.ToString() || connectionInfos[ConnectionId].EventMassage == "")
    //        {
    //            connectionInfos[ConnectionId].EventMassage = massageType.ToString() + eventMassage;
    //        }
    //        else
    //        {
    //            AddEventToEventStorg(ConnectionId, massageType.ToString() + eventMassage);

    //        }
    //    }

    //    private static void AddEventToEventStorg(int connectionId, string stBody)
    //    {
    //        Errors.AddClientError(" adding event on unreaded event");
    //        Log.AddLog(" adding event on unreaded event: id " + connectionId.ToString() + " event: " + stBody);
    //        int counter = 0;
    //        while ((eventStogage[counter].ConnectedId == -1) && counter < eventStogage.Length - 1) { counter++; }
    //        if (counter < eventStogage.Length)
    //        {
    //            eventStogage[counter].ConnectedId = connectionId;
    //            eventStogage[counter].eventBody = stBody;
    //            connectionInfos[connectionId].HaveAdditionalEventMassage = true;
    //        }
    //        else
    //        {
    //            Errors.AddBigError(" Event Lost because of not enogh space");
    //        }
    //    }

    //    private static string ReadEventFromEventStorg(int connectionId)
    //    {
    //        Log.AddPlayerLog(connectionId, " Read Event From event Storg");
    //        int counter = 0;
    //        string st
    //        while (eventStogage[counter].ConnectedId != connectionId && counter < eventStogage.Length - 1) { counter++; }
    //        if (counter < eventStogage.Length)
    //        {
    //            eventStogage[counter].ConnectedId = -1;
    //            connectionInfos[connectionId].HaveAdditionalEventMassage = false;
    //            for (int i = 0; i < eventStogage.Length; i++) if (eventStogage[i].ConnectedId == connectionId)
    //                {
    //                    connectionInfos[connectionId].HaveAdditionalEventMassage = true;
    //                }
    //            return eventStogage[counter].eventBody;
    //        }
    //        else
    //        {
    //            Errors.AddBigError(" Event not finded in eventstorage");
    //            return "ErrorError";
    //        }

    //    }

    //    #endregion

    //    public static bool IsShootValid(ShootActionCode shoot)
    //    {
    //        /*
    //        if(shoot.power > connectedPlayersInfos[shoot.playerID].pawns[shoot.pawnNumber].MaxPower)
    //        {
    //            return false;
    //        }
    //        if (shoot.rotationPower > connectedPlayersInfos[shoot.playerID].pawns[shoot.pawnNumber].MaxRotationPower) {
    //            return false;
    //        }
    //        */
    //        if (MatchList.matchList[shoot.MatchID].isPlayerOnTurn && shoot.playerID != MatchList.matchList[shoot.MatchID].playerOneId)
    //        {
    //            return false;
    //        }
    //        if (!MatchList.matchList[shoot.MatchID].isPlayerOnTurn && shoot.playerID != MatchList.matchList[shoot.MatchID].playerTwoId)
    //        {
    //            return false;
    //        }
    //        return true;
    //    }

    //    public static PlayerForSerial LoadPlayerDataFromServer(string PlayerId)
    //    {

    //        PlayerForConnectedPlayer playerInfo = DatabaseManager.LoadPlayerData(PlayerId);
    //        ClearConnectionById(playerInfo.id);
    //        int AddPlace = 0;
    //        while (IsConnected(AddPlace)) { AddPlace++; }
    //        playerInfo.connectedId = AddPlace;
    //        connectedPlayers[playerInfo.connectedId] = playerInfo;
    //        AddPlayerEvent(playerInfo.connectedId, ServrMasage.NothingNew, "");
    //        connectionInfos[playerInfo.connectedId].lastTimeConnecttion = DateTime.Now;
    //        connectionInfos[playerInfo.connectedId].ConnecttionTime = DateTime.Now;
    //        connectionInfos[playerInfo.connectedId].connected = true;
    //        connectionInfos[playerInfo.connectedId].PlayerId = playerInfo.id;
    //        connectionInfos[playerInfo.connectedId].ActiveMatchId = -1;
    //        PlayerForSerial plsr = playerInfo.ReturnPlayrSerial();
    //        return plsr;
    //    }


    //    #region coonection functions
    //    private static void ClearConnectionById(string playerid)
    //    {
    //        for (int i = 0; i < connectionInfos.Length; i++) if (connectionInfos[i].PlayerId == playerid)
    //            {
    //                CleanId(i);
    //            }
    //    }

    //    public static void SetPlayerMatch(int playerId, int matchId)
    //    {
    //        connectionInfos[playerId].ActiveMatchId = matchId;
    //    }

    //    public static void PlayerWined(int playerId, int BetedMoney)
    //    {
    //        connectedPlayersList[playerId].
    //    }


    //    private static bool ConnectionTimeOut(int ConnectionId)
    //    {

    //        TimeSpan deferentTime = DateTime.Now.Subtract(connectionInfos[ConnectionId].lastTimeConnecttion);
    //        if (1 < deferentTime.TotalDays)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            if (Statistics.ConnectionTimeOut < deferentTime.TotalSeconds)
    //            {
    //                return true;
    //            }
    //            return false;
    //        }
    //    }

    //    private static void CleanId(int connectionId)
    //    {
    //        if (0 <= connectionInfos[connectionId].ActiveMatchId) { MatchList.playerOfMatchLost(connectionInfos[connectionId].ActiveMatchId, connectionId); }
    //        if (connectionInfos[connectionId].connected != false) { Log.AddPlayerLog(connectionId, "player " + connectionId.ToString() + " DisConnected with id:" + connectedPlayers[connectionId].id + " Id Cleaned"); }
    //        connectionInfos[connectionId].connected = false;
    //        connectionInfos[connectionId].ConnecttionTime = DateTime.MinValue;
    //        AddPlayerEvent(connectionId, ServrMasage.Disconcted, "");
    //        connectionInfos[connectionId].lastTimeConnecttion = DateTime.MinValue;
    //        connectionInfos[connectionId].ActiveMatchId = -1;
    //    }
    //     this function will be called ones at start of server
    //    #endregion

    //    /*
    //    public static DateTime ReturnPlayerConnectionTime(int playerId)
    //    {
    //        if (connectionInfos[playerId] == null) { return DateTime.MinValue; }
    //        else
    //        {
    //            return connectionInfos[playerId].ConnecttionTime;
    //        }            
    //    }
    //    */


    //    /*
    //public static void ChangePlayerProperty(Property newProperty , int PlayerId)
    //{
    //    connectedPlayers[PlayerId].PlayerProperty = newProperty;
    //    connectedPlayers[PlayerId].PlayerProperty = newProperty;
    //}
    //*/


    //    public static void DecreasePropertyForBuyingThisPawn(int PlayerId, int NumberOfPawnArray)
    //    {

    //        connectedPlayers[PlayerId].Coin = connectedPlayers[PlayerId].Coin - Pawnlist[NumberOfPawnArray].price.coin;
    //        connectedPlayers[PlayerId].Fan = connectedPlayers[PlayerId].Fan - Pawnlist[NumberOfPawnArray].price.fan;

    //    }

    //    /*
    //    private static void CleanConnectionInfo(int id)
    //    {
    //        connectionInfos[id].EventMassage = "nonew";            
    //    }
    //    */

    //    public static void FillArrays()
    //    {
    //        / Log.AddLog("Connected palyer list. FillArrays: fill array is done");
    //        for (int i = 0; i < connectionInfos.Length; i++)
    //        {
    //            connectionInfos[i] = new playerCennectionInfo();
    //            connectionInfos[i].HaveAdditionalEventMassage = false;
    //            connectionInfos[i].EventMassage = ServrMasage.NothingNew.ToString();
    //            connectedPlayers[i] = new PlayerForConnectedPlayer();
    //            eventStogage[i] = new eventtt();
    //        }
    //    }

    //    private static DateTime lastcheckTimeOutForAll = DateTime.MinValue;

    //    public static void checkTimeOutForAll()
    //    {
    //        bool check;
    //        TimeSpan deferentTime = DateTime.Now - lastcheckTimeOutForAll;
    //        if (deferentTime.TotalSeconds < Statistics.AllCleanMinIntervals) { return; }
    //        for (int i = 0; i < connectionInfos.Length; i++)
    //        {
    //            check = IsConnected(i);
    //        }

    //    }

    //    public static bool changePlayerTeam(TeamForConnectedPlayers playerteam, int ConnectionId)
    //    {
    //        bool result = false;
    //        result = connectedPlayers[ConnectionId].ChangeTeam(playerteam);
    //        return result;

    //        /*
    //        //////////////change Pawn info
    //        convert array to list
    //        List<int> pawnOutOfTeamlist = new List<int>(connectedPlayers[ConnectionId].pawnOutOfTeam);
    //        List<int> pawnsInBenchlist = new List<int>(connectedPlayers[ConnectionId].team.PlayeingPawns);
    //        List<int> PlayeingPawnslist = new List<int>(connectedPlayers[ConnectionId].team.pawnsInBench);
    //        List<int> pawnsInBenchlist2 = new List<int>(playerteam.pawnsInBench);
    //        List<int> PlayeingPawnslist2 = new List<int>(playerteam.PlayeingPawns);
    //        make allpawnList of player
    //        pawnOutOfTeamlist.AddRange(pawnsInBenchlist);
    //        pawnOutOfTeamlist.AddRange(PlayeingPawnslist);
    //        delete new team of AllPawnList to show pawnOutOfTeam
    //        foreach (int i in pawnsInBenchlist2)
    //        {
    //            pawnOutOfTeamlist.Remove(i);
    //        }
    //        foreach (int i in PlayeingPawnslist2)
    //        {
    //            pawnOutOfTeamlist.Remove(i);
    //        }
            
    //        connectedPlayers[ConnectionId].pawnOutOfTeam = pawnOutOfTeamlist.ToArray();
    //        connectedPlayers[ConnectionId].team.PlayeingPawns = PlayeingPawnslist2.ToArray();
    //        connectedPlayers[ConnectionId].team.pawnsInBench = pawnsInBenchlist2.ToArray();
    //        //////////////change elixir info
    //        List<int> ElixirOutOfTeamlist = new List<int>(connectedPlayers[ConnectionId].elixirOutOfTeam);
    //        List<int> ElixirInBenchlist = new List<int>(connectedPlayers[ConnectionId].team.ElixirInBench);
    //        List<int> elixirInBenchlist2 = new List<int>(playerteam.ElixirInBench);
    //        ElixirOutOfTeamlist.AddRange(ElixirInBenchlist);
    //        delete new team of AllPawnList to show pawnOutOfTeam
    //        foreach (int i in elixirInBenchlist2)
    //        {
    //            ElixirOutOfTeamlist.Remove(i);
    //        }
    //        connectedPlayers[ConnectionId].elixirOutOfTeam = ElixirOutOfTeamlist.ToArray();
    //        connectedPlayers[ConnectionId].team.ElixirInBench = elixirInBenchlist2.ToArray();
    //        //////////////change formation info
    //        List<int> UsableFormationTeamlist = new List<int>(connectedPlayers[ConnectionId].team.UsableFormations);
    //        int CurentFormationTeamlist = connectedPlayers[ConnectionId].team.CurrentFormation;
    //        int CurentFormationTeamlistNew = playerteam.CurrentFormation;
    //        UsableFormationTeamlist.Add(CurentFormationTeamlist);
    //        UsableFormationTeamlist.Remove(CurentFormationTeamlistNew);
    //        connectedPlayers[ConnectionId].team.UsableFormations = UsableFormationTeamlist.ToArray();
    //        connectedPlayers[ConnectionId].team.CurrentFormation = CurentFormationTeamlistNew;
    //        //////////////save changes of Pawn&Formation&elixir in database
    //        DatabaseManager.SaveChangesOnPlayer(connectedPlayers[ConnectionId]);
    //        */

    //    }
    //}

}