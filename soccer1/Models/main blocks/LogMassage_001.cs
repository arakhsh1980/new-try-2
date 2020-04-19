using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using soccer1.Models.main_blocks;
using System.ComponentModel.DataAnnotations;


namespace soccer1.Models.main_blocks
{
    public class LogMassage
    {
        [Key]
        public int LogIndex { get; set; }
        public string MassageLog { get; set; }
        public string LogTime { get; set; }
        public string PlayerConnectionTime { get; set; }
        public string MatchCreationTime { get; set; }       
    }

    public class Log3DBContext : DbContext
    {
        public DbSet<LogMassage> GameLog3 { get; set; }
    }
}