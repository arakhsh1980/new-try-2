using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace soccer1.Models
{
    public static class Errors
    {
        static string[] Bigerrors = new string[1000];
        static string[] SmallErrors = new string[1000];
        static string[] clientErrors = new string[1000];
        static int errorCounter = 0;
        static int clientErrorCounter = 0;
        static int SmallErrorsCounter = 0;
        static int returnErrorsCounter = 0;

        
        public static void AddBigError(string ErrorBody)
        {
            if (errorCounter >= 1000) { errorCounter = 0; }
            
            
                if (Bigerrors[errorCounter] == null) { Bigerrors[errorCounter] = ErrorBody; }
                Bigerrors[errorCounter] = ErrorBody;
                errorCounter++;
            
        }
        
        public static void AddClientError(string ErrorBody)
        {
            if (clientErrorCounter >= 1000) { clientErrorCounter = 0; }
            
            
                if (clientErrors[clientErrorCounter] == null) { clientErrors[clientErrorCounter] = ErrorBody; }
                clientErrors[clientErrorCounter] = ErrorBody;
                clientErrorCounter++;
            
        }

        public static void AddSmallError(string ErrorBody)
        {
            if (SmallErrorsCounter >= 1000) { SmallErrorsCounter = 0; }
            
            
                if (SmallErrors[SmallErrorsCounter] == null) { SmallErrors[SmallErrorsCounter] = ErrorBody; }
                SmallErrors[SmallErrorsCounter] = ErrorBody;
                SmallErrorsCounter++;
            
        }

        public static string ReturnBigError()
        {
            if (returnErrorsCounter >= 1000) { returnErrorsCounter = 0; }
            if (returnErrorsCounter == errorCounter)
            {
                return "NoNew";
            }
            else
            {
                returnErrorsCounter++;
                if (returnErrorsCounter >= 1001) { returnErrorsCounter = 1; }
                return Bigerrors[returnErrorsCounter - 1];
            }
        }




    }
}