using System;

namespace soccer1.Models.main_blocks
{
    [Serializable]
    public struct BuyableXpPakage
    {
        public long expireTime;
        public int[] requiredAl;
        public int[] requiredGold;
        public int[] xpValue;
        public short[] numberOfAvalablePakages;
        public string special;
    }
}