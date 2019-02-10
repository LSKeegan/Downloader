using System;
using System.Collections.Generic;
using System.IO;

namespace Downloader
{
    public class FileDownloader
    {
        ResponseGrabber responseGrabber = new ResponseGrabber();
        ExtensionChecker extensionChecker = new ExtensionChecker();

        private string _destinationFolder;

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

        //Saves responses of our URI list to directory 
        public void DownloadMultipleResponsesToDirectory(IEnumerable<Uri> uriList, string destinationFolder)
        {
            //Creates folder in given directory. If folder already exists, this line is ignored. 
            Directory.CreateDirectory(destinationFolder);

            _destinationFolder = destinationFolder;

            //Get all of our web responses and save them to our directory
            responseGrabber.GetMultipleWebResponses(uriList, OnDownloadCompleted);
        }

        //Saves our web response to directory
        public void OnDownloadCompleted(Uri url, Byte[] response)
        {
            //Variables that hold our mime type and extension type of our web responses 
            string mime, extension;

            //Get Mime type, and convert to its respective extension
            try
            {
                mime = extensionChecker.GetMimeType(url);
                extension = extensionChecker.ConvertMimeToExt(mime);

                //This is what we will name the saved file 
                string fileName = _destinationFolder + "/" + Path.GetFileNameWithoutExtension(url.ToString()) + extension;

                //Save response to directory 
                Console.WriteLine("Saving {0} to {1}", url, fileName);
                File.WriteAllBytes(fileName, response);
                Console.WriteLine("Successfully Saved {0} to {1}", url, fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        
    }
}
