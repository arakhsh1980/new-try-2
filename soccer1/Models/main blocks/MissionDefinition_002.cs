using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace soccer1.Models.main_blocks
{
    public class MissionDefinition
    {
        [Key]
        public string key { get; set; }
        public short IdNum { get; set; }
        public int rewardInGold { get; set; }
        public string preRequisite { get; set; }
        
    }

    public class IntArrayClass
    {
        [Key]
        public int key { get; set; }

        public int ar { get; set; }
    }
}