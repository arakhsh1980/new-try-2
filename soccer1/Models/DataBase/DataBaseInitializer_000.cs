using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace soccer1.Models.DataBase
{
    public class databaseGameInitial : System.Data.Entity.DropCreateDatabaseIfModelChanges<DataDBContext>
    {
        protected override void Seed(DataDBContext context)
        {           
            context.SaveChanges();
        }
    }
}




namespace ContosoUniversity.DAL
{
    
}