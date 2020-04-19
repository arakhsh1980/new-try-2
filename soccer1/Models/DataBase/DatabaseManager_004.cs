using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models.main_blocks;
using soccer1.Models.utilites;
using soccer1.Models.DataBase;
using System.Data.Entity;
using System.Threading;



namespace soccer1.Models
{
    public class DatabaseManager
    {








        private DataDBContext dataBase = new DataDBContext();

        public void AddPartToDataBase(RoboPart p)
        {


            RoboPart pp = dataBase.allParts.Find(p.key);
            if (pp == null)
            {
                dataBase.allParts.Add(p);
                dataBase.SaveChanges();
            }
            else
            {
                dataBase.allParts.Remove(pp);
                dataBase.SaveChanges();
                dataBase.allParts.Add(p);
                dataBase.SaveChanges();
            }
        }



        public void AddSponsorToDataBase(Sponsor p)
        {
            Sponsor pp = dataBase.allSponsor.Find(p.key);
            if (pp == null)
            {
                dataBase.allSponsor.Add(p);
                dataBase.SaveChanges();
            }
            else
            {
                dataBase.allSponsor.Remove(pp);
                dataBase.SaveChanges();
                dataBase.allSponsor.Add(p);
                dataBase.SaveChanges();
            }
        }


        public void AddRoboBaseToDataBase(RoboBase p)
        {


            RoboBase pp = dataBase.allBases.Find(p.key);
            if (pp == null)
            {
                dataBase.allBases.Add(p);
                dataBase.SaveChanges();
            }
            else
            {
                dataBase.allBases.Remove(pp);
                dataBase.SaveChanges();
                dataBase.allBases.Add(p);
                dataBase.SaveChanges();
            }
        }


        public void AddMissionToDataBase(MissionDefinition mission)
        {


            MissionDefinition pp = dataBase.allMissions.Find(mission.key);
            if (pp == null)
            {
                dataBase.allMissions.Add(mission);
                dataBase.SaveChanges();
            }
            else
            {
                dataBase.allMissions.Remove(pp);
                dataBase.SaveChanges();
                dataBase.allMissions.Add(mission);
                dataBase.SaveChanges();
            }
        }


        public void AddFormationToDataBase(Formation ff)
        {
            
            //dataBase.
            Formation pp = dataBase.allFormations.Find(ff.key);
            if (pp == null)
            {
                dataBase.allFormations.Add(ff);
                dataBase.SaveChanges();
            }
            else
            {
                dataBase.allFormations.Remove(pp);
                dataBase.SaveChanges();
                dataBase.allFormations.Add(ff);
                dataBase.SaveChanges();                
            }
        }


        public void AddOfferToDataBase(Offer ff)
        {
            
            Offer pp = dataBase.allOffers.Find(ff.IdName);
            if (pp == null)
            {
                dataBase.allOffers.Add(ff);
                dataBase.SaveChanges();
            }
            else
            {
                dataBase.allOffers.Remove(pp);
                dataBase.SaveChanges();
                dataBase.allOffers.Add(ff);
                dataBase.SaveChanges();
            }
        }


        public void AddElixirToDataBase(Elixir el)
        {
           
            Elixir elold = dataBase.allElixires.Find(el.key);
            if (elold == null)
            {
                dataBase.allElixires.Add(el);
                dataBase.SaveChanges();
            }
            else
            {
                dataBase.allElixires.Remove(elold);
                dataBase.SaveChanges();
                dataBase.allElixires.Add(el);
                dataBase.SaveChanges();
            }
        }


    }
}