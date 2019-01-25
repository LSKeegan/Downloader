using System;
using System.Collections.Generic;
using System.IO;

namespace Downloader
{
    class ResponseHandler : IWebResponseHandler
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public delegate void del(Uri url);


        public void GetResponse(IEnumerable<Uri> uriList, Action<Uri, byte[]> onResponseGathered)
        {
            //Loops over each URI in our list
            foreach (Uri url in uriList)
            {
                try
                {
                    onResponseGathered(url, ConvertSingleUriToByte(url));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public void DownloadSingleResponseToFile(Uri url, string destination)
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

        public void DownloadMultipleResponsesToFile(IEnumerable<Uri> uriList, string destinationFolder)

        {
            string mime, extension;

            //Creates folder in given directory. If folder already exists, this line is ignored. 
            System.IO.Directory.CreateDirectory(destinationFolder);
            Console.WriteLine("Saving contents of all url's listed in {0} to folder {1}...", uriList, destinationFolder);

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
                    DownloadSingleResponseToFile(url, fileName);
                }
            }
        }

        /*
        //Returns reponse of given uri in form of byte[]
        public byte[] ConvertSingleUriToByte(Uri url)
        {
            try
            {
                //Creates our web client
                Console.WriteLine("Creating webconsole");
                WebClient myClient = new WebClient();

                //Download web response to a byte array
                Console.WriteLine("Saving web response");
                byte[] dataBuffer = myClient.DownloadData(url);

                return dataBuffer;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to save response of {0}", url);
                throw;
            }
        }
        */

        /*
    public void GetResponse(IEnumerable<Uri> uriList, Action<Uri,byte[]> onResponseGathered)
    {
        //Loops over each URI in our list
        foreach (Uri url in uriList)
        {
            try
            { 
                onResponseGathered(url, ConvertSingleUriToByte(url));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
    */

        /*
    public void DownloadResponseToFile(IEnumerable<Uri> uriList, string destination)
    {
        foreach(Uri url in uriList)
        {
            try
            {

            }
            catch
            {

            }
        }
    }
    */
    }
}
