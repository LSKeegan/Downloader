using System;
using System.Collections.Generic;
using DownloaderLib;
using System.IO;

namespace Downloader
{
    public class ResponseHandler
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

        public void DownloadResponsesToDirectory(IEnumerable<Uri> uriList, string directory)
        {
            try
            {
                fileDownloader.DownloadMultipleResponsesToFile(uriList, directory);
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
                fileDownloader.DownloadSingleResponseToFile(url, destination);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

    }
}
