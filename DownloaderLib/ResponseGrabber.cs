using System;
using System.Net;
using System.Collections.Generic;

namespace Downloader
{
    public class ResponseGrabber
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            catch(Exception e)
            {
                log.Error(e);
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
                catch(Exception e)
                {
                    log.Error(e);
                    continue;
                }
            }
        }

    }
}
