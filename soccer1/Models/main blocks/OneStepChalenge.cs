using System;
using System.ComponentModel.DataAnnotations;

namespace soccer1.Models.main_blocks
{
    public class OneStepChalenge
    {
        [Key]
        public float level { get; set; }
        [Key]
        public string BuilderId { get; set; }
        [Key]
        public short BuilderChalengeNumber { get; set; }
        public string startPositions;
        public string OpponentShoot1;
        public bool isPlayerOneScoredAtEnd;
        public int durablity;
        public int winChanse;
        public int playerTryTwoCertify;
    }

    public class TwoStepChalenge
    {
        [Key]
        public float level { get; set; }
        [Key]
        public string BuilderId { get; set; }
        [Key]
        public short BuilderChalengeNumber { get; set; }
        public string startPositions { get; set; }
    public string endPositionsAfter { get; set; }
    public string OpponentShoot1 { get; set; }
    public string OpponentShoot2 { get; set; }
    public bool isPlayerOneScoredAtEnd { get; set; }
    public int  durablity { get; set; }
    public int winChanse { get; set; }
    public int playerTryTwoCertify { get; set; }
}

    [Serializable]
    public class TwoStepGoalMemory
    {
        public string startPositions;
        public string endPositionsAfter;
        public string plTwoShoot1;
        public string plTwoShoot2;
        public string plOneShoot1;
        public string plOneShoot2;
        public bool isPlayerOneScoredAtEnd;
        public string ScorerId;

    }
}