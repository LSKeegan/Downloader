using System;
using System.Net;
using System.Collections.Generic;
using System.IO;

namespace Downloader
{
    public class ResponseGrabber
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Returns reponse of given uri in form of byte[]
        public byte[] GetSingleWebResponse(Uri url)
        {
            //Create web request using our Uri
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                //Stream response into MemoryStream, then return a byte array
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            stream.CopyTo(ms);
                            return ms.ToArray();
                        }
                    }
                }
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
