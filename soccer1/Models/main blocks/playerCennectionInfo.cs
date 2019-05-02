using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace soccer1.Models
{
    public class playerCennectionInfo
    {   public playerCennectionInfo()
        {
             EventMassage = MatchMassageType.NothingNew.ToString();
             PlayerId = "?";
             connected = false;
             lastTimeConnecttion = DateTime.MinValue;
             ConnecttionTime = DateTime.MinValue;
             ActiveMatchId = -1;
             HaveAdditionalEventMassage = false;
        }


        public string EventMassage = MatchMassageType.NothingNew.ToString();
        public string PlayerId = "?";
        public bool connected =false;
        //public float powerLevel = 10.0f;
        public DateTime lastTimeConnecttion= DateTime.MinValue;
        public DateTime ConnecttionTime = DateTime.MinValue;
        public int ActiveMatchId=-1;
        public bool HaveAdditionalEventMassage =  false;
        //public PlayerInfo playerInfo;
    }
}