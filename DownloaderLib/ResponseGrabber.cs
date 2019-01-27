using System;
using System.Net;

namespace Downloader
{
    public class ResponseGrabber
    {

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
            catch(Exception ex)
            {
                throw;
            }
        }

    }
}
