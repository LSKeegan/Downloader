using System;
using System.IO;
using System.Net;
using Microsoft.Win32;

namespace Downloader
{
    class FileDownloader
    {
        //Logger
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        //Get Mime type of a given url response 
        public string GetMimeType(string url)
        {
                try
                {
                    //Creates our web request
                    var request = HttpWebRequest.Create(url);

                    //Stores our MIME type 
                    string contentType = "";

                    //Save our response
                    var response = request.GetResponse();

                    //Grab content-type from our response 
                    contentType = response.ContentType;

                    return contentType;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Failed to retrieve MimeType of {0}", url);
                    throw;
                }
        }

        public string ConvertMimeToExt(string mimeType)
        {
            string result;
            RegistryKey key;
            object value;

            /*
             * Microsoft registry fails to recognize some html content-types,
             * so if content-type includes 'text/html', we automatically 
             * categorize it as an html file. 
            */
            if (mimeType.Contains("text/html"))
                mimeType = "text/html";

            key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + mimeType, false);
            value = key != null ? key.GetValue("Extension", null) : null;

            //Result is our extension. If it's not null, we return the result as string. If it is null, we return an empty string. 
            result = value != null ? value.ToString() : string.Empty;

            return result;
        }

        //Downloads response of url and returns the data in a byte array
        public byte[] DownloadResponse(string url)
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
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to save response of {0}", url);
                throw;
            }
        }

        public void DownloadResponseToFile(string url, string destination)
        {
            //URL response
            byte[] response = DownloadResponse(url);

            try
            {
                //Save our data to file
                Console.WriteLine("Downloading {0} to file", url);
                File.WriteAllBytes(destination, response);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to save response of {0} to {1}", url, destination);
            }
        }

        public void DownloadMultipleResponsesToFile(string urlsTextDocument, string destinationFolder)
        {

            string url, mime, extension;

            //Creates folder in given directory. If folder already exists, this line is ignored. 
            Directory.CreateDirectory(destinationFolder);
            Console.WriteLine("Saving contents of all url's listed in {0} to folder {1}...", urlsTextDocument, destinationFolder);

            //Saves our .txt to string
            Console.WriteLine("Saving file contents of {0}...", urlsTextDocument);
            using (StreamReader ms = new StreamReader(urlsTextDocument))
            {
                //Read text document until we reach the end
                while((url = ms.ReadLine()) != null)
                {

                    //Get Mime type, and convert to its respective extension
                    try
                    {
                        mime = GetMimeType(url);
                        extension = ConvertMimeToExt(mime);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Failed to save response of {0} to folder {1}", url, destinationFolder);
                        logger.Error(ex, "Failed to save response of {0} to {1}", url, destinationFolder);
                        continue;
                    }

                    //If extension is empty, result to .txt file
                    if (extension.Equals(""))
                        extension = ".txt";
                   
                    //This is what we will name the saved file 
                    string fileName = destinationFolder + "/" + Path.GetFileNameWithoutExtension(url) + extension;

                    //Save url to file 
                    Console.WriteLine("Saving {0} to {1}", url, fileName);
                    DownloadResponseToFile(url, fileName);
                }
            }

        }

        /*
        public void DownloadResponseToFile(string url, string destination)
        {
            try
            {
                //Creates WebClient instance. 
                Console.WriteLine("Creating webconsole");
                WebClient myClient = new WebClient();

                //Download web response to our data buffer
                Console.WriteLine("Saving web response");
                byte[] dataBuffer = myClient.DownloadData(url);

                //Saves our data to file
                Console.WriteLine("Downloading to file");
                File.WriteAllBytes(destination, dataBuffer);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to save response of {0} to {1}", url, destination);
                throw;
            }

        }
        */


    }
}
