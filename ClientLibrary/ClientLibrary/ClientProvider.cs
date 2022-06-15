using Grpc.Net.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientLibrary
{
    class ClientProvider
    {
        static async Task Main() //die Methode ist an sich bei der Bibliothek unnötig, aber falls eine Konsolenanwendung gemacht werden soll, würde es so aussehen
        {
            //um auch eine "unsichere" http verbindung zuzulassen
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            // The port number(50051) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("http://localhost:50051");
            var client = new WaterLevel.WaterLevelClient(channel);
            try
            {
                var reply = await client.GetDataAsync(
                    new GetDataRequest { BeginningTimestamp = 1651826769 }); //unix timestamp, zeit ab 1.1.1970
                Console.WriteLine("Greeting: " + reply.Json);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
    }
}