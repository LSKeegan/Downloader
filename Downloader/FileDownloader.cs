using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Downloader
{
    class FileDownloader
    {
        ResponseGrabber responseGrabber = new ResponseGrabber();
        ExtensionChecker extensionChecker = new ExtensionChecker();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void DownloadArgInput(string text, string destination)
        {
            //If given input is a text file, read each line and add to URI list
            if (Path.GetExtension(text) == ".txt")
            {
                using (StreamReader stream = new StreamReader(text))
                {
                    List<Uri> myUriList = new List<Uri>();
                    string line = stream.ReadLine();

                    while (line != null)
                    {
                        try
                        {
                            //Convert each url to a URI and add to list
                            Uri newUri = new Uri(line);
                            myUriList.Add(newUri);
                            line = stream.ReadLine();
                        }
                        catch
                        {
                            Console.WriteLine("Couldn't convert {0} to uri.", line);
                            line = stream.ReadLine();
                            continue;
                        }
                    }

                    //Download our responses to file
                    DownloadMultipleResponsesToFile(myUriList, destination);
                }
            }
            else
            {
                //If only a single url, then download the single URL to file
                Uri MyUri = new Uri(text);
                DownloadSingleResponseToFile(MyUri, destination);
            }
        }

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
                logger.Error(ex, "Failed to save response of {0} to {1}", url.ToString(), destination);
            }
        }

        public void DownloadMultipleResponsesToFile(IEnumerable<Uri> uriList, string destinationFolder)
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
                    logger.Error(ex, "Failed to save response of {0} to {1}", url, destinationFolder);
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
