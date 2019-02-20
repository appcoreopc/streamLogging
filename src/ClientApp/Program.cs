using System;
using Grpc.Core;
using Appcoreopc.Streamlogging;
using System.Threading;

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
            var x = streamCall.GetAwaiter().GetResult().Content;
            
            Console.WriteLine(x);
            Console.WriteLine("Hello World!");
        }
    }
}
