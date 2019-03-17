using Downloader;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DownloaderTestMain
{
    class DownloaderTest
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void OnDownloadCompleted(Uri Uri, byte[] data)
        {
            Console.WriteLine("Downloaded: {0}  Here's my data: {1}", Uri, data);
        }
        
        static void Main(string[] args)
        {
            ResponseGrabber responseHandler = new ResponseGrabber();

            List<Uri> uriList = new List<Uri>();

            //Arg input for number of iterations through downloader 
            int n = 6;

            //Check for arg input
            if ((args.Length != 0) && (args[0] != null))
            {
                n = Int32.Parse(args[0]);
            }

            //These will be added immediately to our List 
            uriList.Add(new Uri("https://www.reddit.com/r/Saints/comments/a7610k/official_week_15_game_thread_new_orleans_saints/"));
            uriList.Add(new Uri("https://docs.microsoft.com/en-us/dotnet/standard/collections/thread-safe/blockingcollection-overview"));
            uriList.Add(new Uri("https://twitter.com/"));
            uriList.Add(new Uri("https://www.hulu.com/"));
            uriList.Add(new Uri("https://www.reddit.com/r/JUSTNOMIL/comments/aotnk2/slappy_has_finally_been_arrested/"));
            uriList.Add(new Uri("https://www.reddit.com/r/nfl/comments/aoutu2/highlight_brady_micd_up_after_faking_a_block_to/"));

            //Get our web response with yield return implemented
            void GetResponsesWithYield()
            {
                try
                {
                    IEnumerable<Uri> uriEnumerable = GetTestURLs(n);
                    responseHandler.GetMultipleWebResponses(uriEnumerable, OnDownloadCompleted);
                }
                catch(Exception e)
                {
                    log.Error(e);
                }
            }

            //This simulates 'streaming'
            IEnumerable<Uri> GetTestURLs(int streamCount)
            {
                for (var i = 0; i < streamCount; i++)
                {
                    Uri uri = uriList[i];
                    Console.WriteLine("Added {0}", uri);
                    yield return uri;
                }
            }

            Thread downloadResponsesWithYield = new Thread(() => GetResponsesWithYield());
            downloadResponsesWithYield.Start();
              
            Console.ReadKey();
        }
    }
}
