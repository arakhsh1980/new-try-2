using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net;
using System.IO;
using System.Text;
using soccer1.Models.main_blocks;
using System.Web.Script.Serialization;
using soccer1.Models.utilites;
using soccer1.Models;
using soccer1.Models.DataBase;

namespace soccer1.Controllers
{
    
    public class HomeController : Controller
    {
        private DataDBContext dataBase = new DataDBContext();


        [HttpPost]
        public string ChangeSponser(FormCollection collection)
        {
            string SponserName = Request.Form["SponserName"];
            string PlayerId = Request.Form["PlayerId"];
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            bool result = false;
            if (player != null)
            {

                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                result = pl.ChangeSponser(SponserName);
                if (result)
                {
                    pl.SaveChanges();
                }
                return result.ToString();
            }
            return result.ToString();
        }

        [HttpPost]
        public string ReturnSponserChoses(FormCollection collection)
        {
            
            string PlayerId = Request.Form["PlayerId"];
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            string result = "";
            if (player != null)
            {

                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                List<string> PlayerChosableSpon = new AssetManager().PlayerChosableSponsers(pl);
                List<string> PlayerSujestabeSp = new AssetManager().PlayerSujestabeSponsers(pl);
                foreach (string spname in PlayerSujestabeSp)
                {
                    if (PlayerChosableSpon.Contains(spname))
                    {
                        PlayerSujestabeSp.Remove(spname);
                    }
                }
                foreach (string st  in PlayerChosableSpon)
                {
                    result += st + "%";
                }
                if (0 < PlayerChosableSpon.Count)
                {
                    result = result.Remove(result.Length - 1);
                }
                
                result += "^";
                foreach (string st in PlayerSujestabeSp)
                {
                    result += st + "%";
                }
                if (0 < PlayerSujestabeSp.Count)
                {
                    result = result.Remove(result.Length - 1);
                }
                return result;
            }
            else
            {
                return "Error.p layer not found";
            }
            
        }

    }
}