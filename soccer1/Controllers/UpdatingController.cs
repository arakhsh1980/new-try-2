using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net;
using System.IO;
using System.Text;
using soccer1.Models.main_blocks;
using System.Web.Script.Serialization;
using soccer1.Models.utilites;
using soccer1.Models;
using soccer1.Models.DataBase;
using System.Data.Entity;


namespace soccer1.Controllers
{
    public class UpdatingController : Controller
    {
        //string filename = "test2";
        string Bundlespath = "D:/N/";
        public ActionResult ReturnBundle(string filename)
        {
            string path = Bundlespath + filename;

            if (System.IO.File.Exists(path))
            {
                byte[] fileBytes = GetFile(path);
                return File(fileBytes, "bondle");
            }
            return null;
        }
        private DataDBContext dataBase = new DataDBContext();

        [HttpPost]
        public string VersionChack(FormCollection collection)
        {
            string calledAssetsVersion = Request.Form["AssetsVersion"];

            string calledCodesVersion = Request.Form["CodesVersion"];

            UpdateData Udata = new UpdateData();

            Udata.lastCodeVersion = Statistics.GameCodeVersion;
            DualString finded = dataBase.GameDataStrings.Find("GameDataVersion");
            if (finded != null)
            {
                Udata.lastAssetsVersion = finded.value;
            }

            Udata.isCodeVersionUpdateMandetory = false;
            Udata.lastAssetsNames = new List<string>();
            string uu = new JavaScriptSerializer().Serialize(Udata);
            return uu;



        }


        [HttpGet]
        public string ReturnPrefrence(FormCollection collection)
        {
            //string preferenceName = Request.Form["PerformanceType"];
            //string nameCode = "GamePrefrance:" + preferenceName;
            DualString finded = dataBase.GameDataStrings.Find("GamePrefrance");

            if (finded != null)
            {
                return finded.value;
            }
            else
            {
                return "Not Finded";
            }

        }

        [HttpGet]
        public string ReturnLastMatchCharPrefrence()
        {
            DualString finded = dataBase.GameDataStrings.Find("NewMatchchar");
            if (finded != null)
            {
                return finded.value;
            }
            else
            {
                return "Not Finded";
            }
        }



        [HttpPost]
        public string ReturnFormationAndOthers()
        {
            DualString finded = dataBase.GameDataStrings.Find("GameData");
            if (finded == null)
            {
                return null;
            }
            else
            {
                return finded.value;
            }
        }


        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        public FileResult Download()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"c:\folder\myfile.ext");
            string fileName = "myfile.ext";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        // GET: Updating
        [HttpGet]
        public string DawnLoadAsset()
        {
            string filename = "1.jpg";
            string path = "D:/N/" + filename;

            if (System.IO.File.Exists(path))
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                //stream.Position = 0;
                //response.Content = new StreamContent(stream);
                //response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = filename };
                //response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/jpg");
                //response.Content.Headers.ContentDisposition.FileName = filename;
                byte[] result = ReadFully(stream);
                StreamReader reader = new StreamReader(stream);
                //string text = reader.ReadToEnd();
                string text = System.IO.File.ReadAllText(path);
                return text;
            }
            else
            {

                return null;
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }







    }

}