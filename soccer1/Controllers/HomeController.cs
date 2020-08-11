using System;
using System.Collections.Generic;
using System.Web.Mvc;
using soccer1.Models.main_blocks;
using soccer1.Models;
using soccer1.Models.DataBase;
using System.Data.Entity;

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
                
                List<string> PlayerChosableSpon = new AssetManager().PlayerChosableSponsers(pl);
                if (PlayerChosableSpon.Contains(SponserName))
                {
                    result = pl.ChangeSponser(SponserName); 
                    if (result)
                    {
                        player.ChangesAcoordingTo(pl);
                        dataBase.Entry(player).State = EntityState.Modified;
                        dataBase.SaveChanges();
                    }
                }   
            }
            return result.ToString();
        }

        [HttpPost]
        public string RequestNewTicket(FormCollection collection)
        {            
            string PlayerId = Request.Form["PlayerId"];
            int NumberOfTickets = Int32.Parse( Request.Form["NumberOfTickets"]);
            PlayerForDatabase player = dataBase.playerInfoes.Find(PlayerId);
            string result = "Error";
            if (player != null)
            {

                PlayerForConnectedPlayer pl = new PlayerForConnectedPlayer();
                pl.reWriteAccordingTo(player);
                pl.UpdateAll();
                int NOT_int =(int) Math.Floor((double)pl.ReturnNumberOfTickets());
                player.ChangesAcoordingTo(pl);
                dataBase.Entry(player).State = EntityState.Modified;
                dataBase.SaveChanges();
                return NOT_int.ToString();
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
                List<string> PlayerSujestableSp = new AssetManager().PlayerSujestabeSponsers(pl);
                List<string> PlayerSujestabeSp2 = new List<string>();
                foreach (string spname in PlayerSujestableSp)
                {
                    if (!PlayerChosableSpon.Contains(spname))
                    {
                        PlayerSujestabeSp2.Add(spname);
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
                foreach (string st in PlayerSujestabeSp2)
                {
                    result += st + "%";
                }
                if (0 < PlayerSujestabeSp2.Count)
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