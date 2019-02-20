using System;
using System.Threading.Tasks;
using Grpc.Core;
using Appcoreopc.Streamlogging;
using Grpc;
using System.Threading;

namespace Server
{
    class LogService : StreamLogging.StreamLoggingBase
    {
        
        public override global::System.Threading.Tasks.Task<global::Appcoreopc.Streamlogging.LogReply> Log(IAsyncStreamReader<LogRequest> requestStream, ServerCallContext context)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            while (requestStream.MoveNext(token).Result)
            {
                var note = requestStream.Current;
                Console.WriteLine(note.Id);
                Console.WriteLine(note.Content);
            }


            System.Console.WriteLine("Incoming rest request");
             return Task.FromResult(new LogReply { Content = "Hello" });
        }
    }

    class Program
    {        
        const int Port = 9999;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var server = new Grpc.Core.Server
            {
                Services = { StreamLogging.BindService(new LogService()) },
                Ports = { new ServerPort("127.0.0.1", Port, ServerCredentials.Insecure) }
            };

            server.Start();

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
