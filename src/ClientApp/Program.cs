﻿using System;
using Grpc.Core;
using Appcoreopc.Streamlogging;
using System.Threading;
using System.Linq;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Channel channel = new Channel("127.0.0.1:9999", ChannelCredentials.Insecure);

            var client = new StreamLogging.StreamLoggingClient(channel);  
              
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;  

            var streamCall = client.Log(new CallOptions(null, null, token));

            int x = 0; 

            foreach (var item in Enumerable.Range(1, 2000))
            {
                x = x + 1;
                var logRequest = new LogRequest() { Id = x.ToString(), Content = "Log Content " + DateTime.Now.ToLongTimeString() };

                // I think isssue with this.... need to sync something here ! 
                streamCall.RequestStream.WriteAsync(logRequest);
            }

            var responseFRomServer = streamCall.GetAwaiter().GetResult().Content;
            Console.WriteLine(responseFRomServer);
            Console.WriteLine("Hello World!");
        }
    }
}
