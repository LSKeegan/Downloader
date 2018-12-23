using System;
using System.Collections.Generic;
using System.Net;

namespace Downloader
{
    class ResponseDownloader
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        //Returns reponse of given uri in form of byte[]
        public byte[] ConvertSingleUriToByte(Uri url)
        {
            try
            {
                //Creates our web client
                Console.WriteLine("Creating webconsole");
                WebClient myClient = new WebClient();

                //Download web response to a byte array
                Console.WriteLine("Saving web response");
                byte[] dataBuffer = myClient.DownloadData(url);

                return dataBuffer;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to save response of {0}", url);
                throw;
            }
        }

        public byte[] GetResponse(List<Uri> uriList)
        {
            //Loops over each URI in our list
            foreach (Uri url in uriList)
            {
                try
                {
                    //Convert our URI to a Byte[]
                    ConvertSingleUriToByte(url);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return null;
        }



    }
}
