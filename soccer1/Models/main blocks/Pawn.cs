using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using soccer1.Models.main_blocks;
using System.ComponentModel.DataAnnotations;


namespace soccer1.Models.main_blocks
{
    
    public class Pawn
    {
      
    [Key]
        public string IdName { get; set; }
        public int index { get; set; }
        public string ShowName { get; set; }
        public string tired1 { get; set; }
        public string tired2 { get; set; }
        public string tired3 { get; set; }
        public string ForMatch { get; set; }
        public string abilityShower { get; set; }
        public Property price { get; set; }
        public PawnAbility mainAbility { get; set; }
    
}


    public class PawnForDataBase
    {
        [Key]
        public string IdName { get; set; }
        public int index { get; set; }
        public string ShowName { get; set; }
        public string redForSale { get; set; }
        public string blueForSale { get; set; }
        public string ForMatch { get; set; }
        public string abilityShower { get; set; }
        public int level { get; set; }
        public int fan { get; set; }
        public int coin { get; set; }
        public int SoccerSpetial { get; set; }        
        public int shootPower { get; set; }        
        public int endorance { get; set; }        
        public int boddyMass { get; set; }        
        public int aiming { get; set; }
        public string spPower1 { get; set; }
        public int spPower1Level { get; set; }
        public int spPower2Level { get; set; }
        public int spPower3Level { get; set; }
        public string spPower2 { get; set; }
        public string spPower3 { get; set; }
    }
    public struct Property
{

    public int level { get; set; }
        public int fan { get; set; }
        public int coin { get; set; }
        public int SoccerSpetial { get; set; }
    }
public struct PawnAbility
{
    [Range(20, 100)]
    public int shootPower { get; set; }
        [Range(20, 100)]
    public int endorance { get; set; }
        [Range(20, 100)]
    public int boddyMass { get; set; }
        [Range(20, 100)]
    public int aiming { get; set; }
        public SpetialPower spPower1 { get; set; }
        public int spPower1Level { get; set; }
        public int spPower2Level { get; set; }
        public int spPower3Level { get; set; }
        public SpetialPower spPower2 { get; set; }
        public SpetialPower spPower3 { get; set; }
    }
public struct SpetialPower
{
       
    public string IdName { get; set; }
        public string ShowName { get; set; }
        //public PawnSpetialPowerType spPower;
        public string scribtion { get; set; }
        public string image { get; set; }
    }




   
}