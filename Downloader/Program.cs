using System;
using System.Collections.Generic;
using System.IO;

namespace Downloader
{
    class Program
    {
        static void Main(string[] args)
        {
            FileDownloader fileDownload = new FileDownloader();

            //Check for command line input 
            if( (args[0] != null) && (args[1] != null) )
            {
                string text = args[0];
                string destination = args[1];

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
                        fileDownload.DownloadMultipleResponsesToFile(myUriList, destination);
                    }
                }
                else
                {
                    //If only a single url, then download the single URL to file
                    Uri MyUri = new Uri(text);
                    fileDownload.DownloadSingleResponseToFile(MyUri, destination);
                }
            }

        }
    }
}