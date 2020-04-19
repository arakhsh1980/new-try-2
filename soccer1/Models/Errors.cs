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
        static int bigErrorCounter = 0;
        static int clientErrorCounter = 0;
        static int SmallErrorsCounter = 0;
        static int returnBErrorsCounter = 0;
        static int returnCErrorsCounter = 0;
        static int returnSErrorsCounter = 0;


        public static void AddBigError(string ErrorBody)
        {
            if (bigErrorCounter >= 1000) { bigErrorCounter = 0; }
            
            
                if (Bigerrors[bigErrorCounter] == null) { Bigerrors[bigErrorCounter] = ErrorBody; }
                Bigerrors[bigErrorCounter] = ErrorBody;
                bigErrorCounter++;
            
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
            if (returnBErrorsCounter >= 1000) { returnBErrorsCounter = 0; }
            if (returnBErrorsCounter == bigErrorCounter)
            {
                return "NoNew";
            }
            else
            {
                returnBErrorsCounter++;
                if (returnBErrorsCounter >= 1001) { returnBErrorsCounter = 1; }
                return Bigerrors[returnBErrorsCounter - 1];
            }
        }

        public static string ReturnClientError()
        {
            if (returnCErrorsCounter >= 1000) { returnCErrorsCounter = 0; }
            if (returnCErrorsCounter == clientErrorCounter )
            {
                return "NoNew";
            }
            else
            {
                returnCErrorsCounter++;
                if (returnCErrorsCounter >= 1001) { returnCErrorsCounter = 1; }
                return clientErrors[returnCErrorsCounter - 1];
            }
        }


        public static string ReturnSmallError()
        {
            if (returnSErrorsCounter >= 1000) { returnSErrorsCounter = 0; }
            if (returnSErrorsCounter == SmallErrorsCounter)
            {
                return "NoNew";
            }
            else
            {
                returnSErrorsCounter++;
                if (returnSErrorsCounter >= 1001) { returnSErrorsCounter = 1; }
                return SmallErrors[returnSErrorsCounter - 1];
            }
        }



    }
}