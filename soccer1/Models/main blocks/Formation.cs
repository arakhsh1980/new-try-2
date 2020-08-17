using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using soccer1.Models.main_blocks;
using System.ComponentModel.DataAnnotations;

namespace soccer1.Models.main_blocks
{
    public static class GameConstants
    {
        public const int canvaseSizeX = 1920;
        public const int canvaseSizeY = 1080;
        public const int formationMaxX = 100;
        public const int formationMinX = 0;
        public const int formationMaxY = 50;
        public const int formationMinY = -50;
        public const int PlayerInFild = 5;
        public const int MaxPlayerInMatch = 10;
        public const float SpeedOfDragFreeadPrefab = 3.0f;
        public const float ObjectInPanelTreshhold = 10.0f;
    }

    public struct PawnStartPosition
    {
        [Range(GameConstants.formationMinX, GameConstants.formationMaxX)]
        public int x;
        [Range(GameConstants.formationMinY, GameConstants.formationMaxY)]
        public int y;
    }





}