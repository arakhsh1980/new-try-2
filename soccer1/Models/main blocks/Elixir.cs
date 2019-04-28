using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using soccer1.Models.main_blocks;
using System.ComponentModel.DataAnnotations;

namespace soccer1.Models.main_blocks
{
    public class Elixir
    {

        [Key]
        public string IdName { get; set; }
        public int index { get; set; }
        public string showName { get; set; }
        public string forSale { get; set; }
        //public Sprite forMatch;
        public Property price { get; set; }
        public SpetialPower spPower { get; set; }

    }
    
    


   
}