using System;
using System.Collections.Generic;

namespace Downloader
{ 
    interface IWebResponseHandler
    {
        //Get Webresponses from a collection of URI's
        void GetResponse(IEnumerable<Uri> uriList, Action<Uri, byte[]> onResponseGathered);

        //Download the web response of one URI to file 
        void DownloadSingleResponseToFile(Uri url, string destination);

        //Download the web response of 2+ URI's to file 
        void DownloadMultipleResponsesToFile(IEnumerable<Uri> uriList, string destinationFolder);
    }
}
