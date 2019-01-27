using System;
using System.Net;
using System.Collections.Generic;

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

        //Returns response of each URI in IEnumerable
        public void GetMultipleWebResponses(IEnumerable<Uri> uriList, Action<Uri, byte[]> onResponseGathered)
        {
            foreach(Uri url in uriList)
            {
                try
                {
                    onResponseGathered(url, GetSingleWebResponse(url));
                }
                catch
                {
                    continue;
                }
            }
        }

    }
}
