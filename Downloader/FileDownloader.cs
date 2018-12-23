using System;
using System.Collections.Generic;
using System.IO;

namespace Downloader
{
    class FileDownloader
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        ResponseDownloader download = new ResponseDownloader();
        ExtensionChecker getExtension = new ExtensionChecker();


        public void DownloadResponseToFile(Uri url, string destination)
        {
            //URL response
            byte[] response = download.ConvertSingleUriToByte(url);

            try
            {
                //Save our data to file
                Console.WriteLine("Downloading {0} to file", url.ToString());
                File.WriteAllBytes(destination, response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to save response of {0} to {1}", url.ToString(), destination);
            }
        }

        public void DownloadMultipleResponsesToFile(List<Uri> uriList, string destinationFolder)
        {
            string mime, extension;

            //Creates folder in given directory. If folder already exists, this line is ignored. 
            Directory.CreateDirectory(destinationFolder);
            Console.WriteLine("Saving contents of all url's listed in {0} to folder {1}...", uriList.ToString(), destinationFolder);

            //Saves our  to string
            Console.WriteLine("Saving file contents of {0}...", uriList.ToString());
            if (uriList.Count == 0)
                Console.WriteLine("Error: must give a list with at least one element.");
            else
            {
                foreach (Uri url in uriList)
                {
                    //Get Mime type, and convert to its respective extension
                    try
                    {
                        mime = getExtension.GetMimeType(url);
                        extension = getExtension.ConvertMimeToExt(mime);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to save response of {0} to folder {1}", url, destinationFolder);
                        logger.Error(ex, "Failed to save response of {0} to {1}", url, destinationFolder);
                        continue;
                    }

                    //If extension is empty, result to .txt file
                    if (extension.Equals(""))
                        extension = ".txt";

                    //This is what we will name the saved file 
                    string fileName = destinationFolder + "/" + Path.GetFileNameWithoutExtension(url.ToString()) + extension;

                    //Save url to file 
                    Console.WriteLine("Saving {0} to {1}", url, fileName);
                    DownloadResponseToFile(url, fileName);
                }
            }
        }


    }
}
