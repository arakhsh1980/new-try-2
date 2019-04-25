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





namespace soccer1.Controllers
{
    public class LoadingController : Controller
    {
        

        // POST: Loading/LoadPlayerData
        [HttpPost]
        public string LoadPlayerData(FormCollection collection)
        {           
            string id = Request.Form["PlayerId"];
            
            PlayerForSerial plsr = ConnectedPlayersList.LoadPlayerDataFromServer(id);
            
            //TeamForConnectedPlayers thisteam = new TeamForConnectedPlayers();
            //plsr.testTeam = Convertors.TeamToTeamForSerialize(thisteam); 
            Log.AddPlayerLog(plsr.CoonId, "player"+ plsr.CoonId.ToString() + " added by " + plsr.id + " ID");
            string uu = new JavaScriptSerializer().Serialize(plsr);
            return uu;        

        }



    }
}
