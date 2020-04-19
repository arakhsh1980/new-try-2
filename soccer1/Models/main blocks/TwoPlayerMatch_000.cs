using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models;
using System.Threading;
using System.Web.Script.Serialization;



[Serializable]
public struct MatchEvents
{
    public MatchMassageType EventTypes;
    public string desitionBodys;
    public int EventNumber;

}

public enum PlayerRequestTypes { goingHomeAndWaiting, waitingToPlay, IAcceptedToPlay }

[Serializable]
public struct MatchEventsArray
{
   
    public MatchEvents[] Events;
    public string Request;
    public string RequestAnswer;
    public void SetNotingNew()
    {
        Events = new MatchEvents[1];
        Events[0].EventTypes = MatchMassageType.NothingNew;
        Events[0].EventNumber = -1;
    }
    public void SetError()
    {
        Events = new MatchEvents[1];
        Events[0].EventTypes = MatchMassageType.Error;
        Events[0].desitionBodys = "Error . defult Eror";
        Events[0].EventNumber = -1;
    }
}

namespace soccer1.Models
{



    public class TwoPlayerMatch
    {
    }


}