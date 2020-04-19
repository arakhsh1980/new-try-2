using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace soccer1.Models.main_blocks
{
    [Serializable]
    public class UpdateData
    {
        public string lastCodeVersion ;

        public string lastAssetsVersion;

        public List<string> lastAssetsNames;

        public bool isCodeVersionUpdateMandetory;
    }
}