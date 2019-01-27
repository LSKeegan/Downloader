using System;
using System.Collections.Generic;
using System.IO;

namespace Downloader
{
    public class FileDownloader
    {
        ResponseGrabber responseGrabber = new ResponseGrabber();
        ExtensionChecker extensionChecker = new ExtensionChecker();

        public void DownloadSingleResponseToFile(Uri url, string destination)
        {
            //URL response
            byte[] response = responseGrabber.GetSingleWebResponse(url);

            try
            {
                //Save our data to file
                Console.WriteLine("Downloading {0} to file", url.ToString());
                File.WriteAllBytes(destination, response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public void DownloadMultipleResponsesToDirectory(IEnumerable<Uri> uriList, string destinationFolder)
        {
            //Variables that hold our mime type and extension type of our web responses 
            string mime, extension;

            //Creates folder in given directory. If folder already exists, this line is ignored. 
            Directory.CreateDirectory(destinationFolder);

            foreach(Uri url in uriList)
            {
                //Get Mime type, and convert to its respective extension
                try
                {
                    mime = extensionChecker.GetMimeType(url);
                    extension = extensionChecker.ConvertMimeToExt(mime);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    continue;
                }

                //If extension is empty, result to .txt file
                if (extension.Equals(""))
                    extension = ".txt";

                //This is what we will name the saved file 
                string fileName = destinationFolder + "/" + Path.GetFileNameWithoutExtension(url.ToString()) + extension;

                //Save response to directory 
                Console.WriteLine("Saving {0} to {1}", url, fileName);
                DownloadSingleResponseToFile(url, fileName);
            }
        }

    }
}
