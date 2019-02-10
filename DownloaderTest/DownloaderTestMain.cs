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
        static void Main(string[] args)
        {
            ResponseGrabber responseHandler = new ResponseGrabber();

            //These will be added immediately to our List 
            Uri test1 = new Uri("https://www.reddit.com/r/Saints/comments/a7610k/official_week_15_game_thread_new_orleans_saints/");
            Uri test2 = new Uri("https://docs.microsoft.com/en-us/dotnet/standard/collections/thread-safe/blockingcollection-overview");
            Uri test3 = new Uri("https://twitter.com/");

            //These will be added after our Downloader.exe runs, simulating a 'stream'
            Uri test4 = new Uri("https://www.hulu.com/");
            Uri test5 = new Uri("https://www.reddit.com/r/JUSTNOMIL/comments/aotnk2/slappy_has_finally_been_arrested/");
            Uri test6 = new Uri("https://www.reddit.com/r/nfl/comments/aoutu2/highlight_brady_micd_up_after_faking_a_block_to/");

            ConcurrentQueue<Uri> uriQueue = new ConcurrentQueue<Uri>();
            uriQueue.Enqueue(test1);
            uriQueue.Enqueue(test2);
            uriQueue.Enqueue(test3);

            void OnDownloadCompleted(Uri Uri, byte[] data)
            {
                Console.WriteLine("Downloaded: {0}  Here's my data: {1}", Uri, data);
            }

            void AddURLsToQueue()
            {
                uriQueue.Enqueue(test4);
                uriQueue.Enqueue(test5);
                uriQueue.Enqueue(test6);
            }

            //This thread will run downloader.exe
            Thread downloadResponses = new Thread( () => responseHandler.GetMultipleWebResponses(uriQueue, OnDownloadCompleted));
            //This thread will be 'streaming' URLs into our List 
            Thread streamInput = new Thread( () => AddURLsToQueue());
           
            //Start Downloader.exe
            downloadResponses.Start();
            //Start 'streaming' Urls into our List
            streamInput.Start();
            downloadResponses.Join();
            streamInput.Join();

            

            Console.ReadKey();
        }
    }
}
