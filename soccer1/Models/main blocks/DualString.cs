using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace soccer1.Models.main_blocks
{
    public class DualString
    {
        [Key]
        public string key { get; set; }

        //public long TimeOfLastUpdate { get; set; }
        public string value { get; set; }
    }
}