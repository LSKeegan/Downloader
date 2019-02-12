using Downloader;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DownloaderTestMain
{
    class DownloaderTest
    {

        public static void OnDownloadCompleted(Uri Uri, byte[] data)
        {
            Console.WriteLine("Downloaded: {0}  Here's my data: {1}", Uri, data);
        }

        
        static void Main(string[] args)
        {
            ResponseGrabber responseHandler = new ResponseGrabber();
            List<Uri> uriList = new List<Uri>();

            //Arg input for number of iterations through downloader 
            int n = 0;


            //Check for arg input
            if ((args.Length != 0) && (args[0] != null))
            {
                n = Int32.Parse(args[0]);
            }

            //These will be added immediately to our List 
            Uri test1 = new Uri("https://www.reddit.com/r/Saints/comments/a7610k/official_week_15_game_thread_new_orleans_saints/");
            Uri test2 = new Uri("https://docs.microsoft.com/en-us/dotnet/standard/collections/thread-safe/blockingcollection-overview");
            Uri test3 = new Uri("https://twitter.com/");
            Uri test4 = new Uri("https://www.hulu.com/");
            Uri test5 = new Uri("https://www.reddit.com/r/JUSTNOMIL/comments/aotnk2/slappy_has_finally_been_arrested/");
            Uri test6 = new Uri("https://www.reddit.com/r/nfl/comments/aoutu2/highlight_brady_micd_up_after_faking_a_block_to/");

            uriList.Add(test1);
            uriList.Add(test2);
            uriList.Add(test3);
            uriList.Add(test4);
            uriList.Add(test5);
            uriList.Add(test6);

            void GetResponsesWithYield()
            {
                IEnumerable<Uri> uriEnumerable = GetTestURLs(n);
                responseHandler.GetMultipleWebResponses(uriEnumerable, OnDownloadCompleted);
            }

            //This simulates 'streaming'
            IEnumerable<Uri> GetTestURLs(int streamCount)
            {
                for (var i = 0; i < streamCount; i++)
                {
                    Uri uri = uriList[i];
                    yield return uri;
                }
            }

            Thread downloadResponsesWithYield = new Thread(() => GetResponsesWithYield());
            downloadResponsesWithYield.Start();
              

            Console.ReadKey();
        }
    }
}
