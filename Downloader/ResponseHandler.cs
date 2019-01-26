using System;
using System.Collections.Generic;
using System.IO;

namespace Downloader
{
    class ResponseHandler : IWebResponseHandler
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        ResponseGrabber responseGrabber = new ResponseGrabber();
        FileDownloader fileDownloader = new FileDownloader();


        public void GetResponse(IEnumerable<Uri> uriList, Action<Uri, byte[]> onResponseGathered)
        {
            //Loops over each URI in our list
            foreach (Uri url in uriList)
            {
                try
                {
                    onResponseGathered(url, responseGrabber.GetSingleWebResponse(url));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public void DownloadResponseToDirectory(IEnumerable<Uri> uriList, string directory)
        {
            try
            {
                Console.WriteLine("Downloading web responses in {0} to {1}...", uriList.ToString(), directory);
                fileDownloader.DownloadMultipleResponsesToFile(uriList, directory);
                Console.WriteLine("Downloader has finished downloading web responses contained in {0} to {1}", uriList.ToString(), directory);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        public void DownloadResponseToFile(Uri url, string destination)
        {
            try
            {
                Console.WriteLine("Downloading {0} to {1}...", url, destination);
                fileDownloader.DownloadSingleResponseToFile(url, destination);
                Console.WriteLine("The web response of {0} has successfully been saved to {1}", url, destination);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

    }
}
