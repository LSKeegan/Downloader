using System;
using System.Net;

namespace Downloader
{
    class ResponseGrabber
    {
        private static NLog.Logger responseGrabberlogger = NLog.LogManager.GetCurrentClassLogger();
        ExtensionChecker getExtension = new ExtensionChecker();

        //Returns reponse of given uri in form of byte[]
        public byte[] GetSingleWebResponse(Uri url)
        {
            try
            {
                //Creates our web client
                WebClient myClient = new WebClient();

                //Download web response to a byte array
                byte[] dataBuffer = myClient.DownloadData(url);

                return dataBuffer;
            }
            catch (Exception ex)
            {
                responseGrabberlogger.Error(ex, "Failed to save response of {0}", url);
                throw;
            }
        }

    }
}
