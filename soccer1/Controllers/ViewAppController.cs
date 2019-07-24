using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using soccer1.Models;

namespace soccer1.Controllers
{
    public class ViewAppController : Controller
    {
        // GET: ViewApp
        public string Viewlogs()
        {
            //ConnectedPlayersList.checkTimeOutForAll();
            string st = "Error: "+Errors.ReturnBigError();
            if(st== "Error: NoNew") {
                return Log.RuternLog();
            }
            else
            {
                return st;
            }            
        }

        public string ViewActiveMatches()
        {
            string result = new SymShootMatchesList().ReturnActiveMatches();
            return result; 
        }


    }
}
