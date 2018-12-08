using System;
using System.IO;
using System.Net;
using System.Linq;
using Microsoft.Win32;

namespace Downloader
{
    class ReadInput
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


        public void ManageIncomingURLs (string fileInput, string destination)
        {
            //Checks file extension of given input 
            string extension = Path.GetExtension(fileInput);

            if (extension.Equals(".txt"))
            {
                //Saves our .txt to string
                Console.WriteLine("Saving file contents of {0}...", fileInput);
                string[] lines = File.ReadAllLines(fileInput);

                //Check how many files we need to create / how many URLs were provided 
                int lineCount = lines.Count();
                Console.WriteLine("Number of URLs in document: {0}", lineCount);

                //Creates folder in given directory. If folder already exists, this line is ignored. 
                Directory.CreateDirectory(destination);
                Console.WriteLine("Saving contents of all url's listed in {0} to folder {1}...", fileInput, destination);
               
                foreach (string url in lines)
                {
                    string mime;
                    string ext;
                    //Get mime type, and convert to extension
                    try
                    {
                         mime = GetMimeType(url);
                         ext = ConvertMimeToExt(mime);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Failed to save response of {0} to folder {1}", url, destination);
                        logger.Error(ex, "Failed to save response of {0} to {1}", url, destination);
                        continue;
                    }


                    //If extension is empty, result to .txt file
                    if (ext.Equals(""))
                        ext = ".txt";
              
                    //This is what we will name the saved file 
                    string fileName = destination + "/" + Path.GetFileNameWithoutExtension(url) + ext; 

                    //Save url to file 
                    Console.WriteLine("Saving {0} to {1}", url, fileName);
                    DownloadResponseToFile(url, fileName);
                }

            }
            else
            {
                Console.WriteLine("Creating file...");
                Console.WriteLine("Saving contents of {0} to {1}.", fileInput, destination);
                DownloadResponseToFile(fileInput, destination);
            }
        }


    }
}
