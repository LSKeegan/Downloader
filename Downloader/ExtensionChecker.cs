using Microsoft.Win32;
using System;
using System.Net;

namespace Downloader
{
    class ExtensionChecker
    {
        //Logger
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    

        //Engine
        //Get Mime type of a given url response 
        public string GetMimeType(Uri url)
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

        //Engine
        //Returns the extension that corresponds to any given mimetype
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
    
        
    }
}
