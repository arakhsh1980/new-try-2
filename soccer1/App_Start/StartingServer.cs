﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using soccer1.Models;

namespace soccer1.App_Start
{
    public static class StartingServer
    {
        public static void StartAll()
        {
            //ConnectedPlayersList.FillArrays();
            new AssetManager().FillArrays();
            new SymShootMatchesList().FillArrays();
        }
    }
}