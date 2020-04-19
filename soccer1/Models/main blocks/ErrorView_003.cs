using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using soccer1.Models.main_blocks;
using System.ComponentModel.DataAnnotations;

namespace soccer1.Models.main_blocks
{
    public class ErrorView
    {
        [Key]
        public int k { get; set; }
        public string st1 { get; set; }
        public string st2 { get; set; }
        public string st3 { get; set; }
        public string st4 { get; set; }
        public string st5 { get; set; }

        public string st6 { get; set; }
        public string st7 { get; set; }
        public string st8 { get; set; }
        public string st9 { get; set; }
        public string st0 { get; set; }
    }
    public class ErrorDBContext : DbContext
    {
        public DbSet<ErrorView> ErrorView { get; set; }
    }
}