using System;
using System.Threading.Tasks;
using Grpc.Core;
using Appcoreopc.Streamlogging;
using Grpc;

namespace Server
{
    class LogService : StreamLogging.StreamLoggingBase
    {
        // Server side handler of the SayHello RPC
        public override Task<LogReply> LogRequest(LogRequest request, ServerCallContext context)
        {
            return Task.FromResult(new LogReply { Content = "Hello " + request.Name });
        }      
    }


    class Program
    {        
        const int Port = 50051;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Server server = new Server
            {
                Services = { Greeter.BindService(new LogService()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };

            server.Start();

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
