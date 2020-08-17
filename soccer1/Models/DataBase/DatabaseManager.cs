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

        public void AddClassDataToDataBase(ClassData CD)
        {
            
            ClassData pp = dataBase.allClassesData.Find(CD.nameCode);
            if (pp == null)
            {
                
                dataBase.allClassesData.Add(CD);
                dataBase.SaveChanges();
            }
            else
            {                
                dataBase.allClassesData.Remove(pp);
                dataBase.SaveChanges();
                dataBase.allClassesData.Add(CD);
                dataBase.SaveChanges();
            }
        }



    }
}