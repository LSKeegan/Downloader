using System;
using System.Collections.Generic;
using System.IO;

namespace Downloader
{
    public class FileDownloader
    {
        ResponseGrabber responseGrabber = new ResponseGrabber();
        ExtensionChecker extensionChecker = new ExtensionChecker();

        private string directory; 

        public FileDownloader(string directory)
        {
            this.directory = directory;
        }

        //If 2+ WebResponses need to be downloaded, this is where they will be saved 
        //private string directory;

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
            //Where we will be saving our responses
            directory = destinationFolder;
     
            //Creates folder in given directory. If folder already exists, this line is ignored. 
            Directory.CreateDirectory(destinationFolder);

            //Download our responses to file 
            responseGrabber.GetMultipleWebResponses(uriList, OnResponseGathered);
        }

        //Once we have a response, this method handles downloading the response 
        private void OnResponseGathered(Uri url, Byte[] data)
        {
            //Variables that hold our mime type and extension type of our web responses 
            string mime, extension;

            //Get Mime type, and convert to its respective extension
            try
            {
                mime = extensionChecker.GetMimeType(url);
                extension = extensionChecker.ConvertMimeToExt(mime);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
     
            //This is what we will name the saved file 
            string fileName = directory + "/" + Path.GetFileNameWithoutExtension(url.ToString()) + extension;

            //Save our data to file
            Console.WriteLine("Downloading {0} to file", url.ToString());
            File.WriteAllBytes(fileName, data);
        }
        
    }
}
