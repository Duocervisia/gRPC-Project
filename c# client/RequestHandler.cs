using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Net.Client;
using System.Windows;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Waterlevel
{
    public class RequestHandler
    {
        public ClientRequest oClientRequest;
        public static List<ClientRequest> lClientRequest = new List<ClientRequest>();
        public string sJson = "";
        private Window oMainWindow;

        public RequestHandler(Window window) : base ()
        {
            oMainWindow = window;
        }
        public List<ClientRequest> GetClientData()
        {
            return lClientRequest; //just to get list to main window
        }


        public async Task doRequest()
        {
            using var channel = GrpcChannel.ForAddress("http://141.45.61.181:50051"); //create the channel with ip address
            var client = new ClientLibrary.WaterLevel.WaterLevelClient(channel); 
            try
            {
                var reply = await client.GetDataAsync(new ClientLibrary.GetDataRequest { BeginningTimestamp = GetLastDayToUnix() }); //actual server request
                sJson = reply.Json;
                lClientRequest = JsonConvert.DeserializeObject<List<ClientRequest>>(sJson); //deserialize json reply into string
                TransformData(lClientRequest);
                MainWindow._instance.requestData();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: "+ex);
                
            }
        }
        public List<ClientRequest> TransformData(List<ClientRequest> clientRequest) //transforms the UnixTimestamp into real Date and Time
        {
            foreach (ClientRequest value in clientRequest)
            {
                value.dateTime = UnixTimestampToDateTime(value.timestamp);
            }
            return clientRequest;
        }

        public static DateTime UnixTimestampToDateTime(double unixTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            return new DateTime(unixStart.Ticks + unixTimeStampInTicks, DateTimeKind.Utc);

        }
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            long unixTimeStampInTicks = (dateTime.ToUniversalTime() - unixStart).Ticks;
            return (double)unixTimeStampInTicks / TimeSpan.TicksPerSecond;
        }
        public static int GetLastDayToUnix() //sets the beginning timestamp
        {
            DateTime dToday = DateTime.Now;
            //DateTime yesterday = dToday.AddDays(-1);
            DateTime yesterday = dToday.AddHours(-6);
            double dYesterday = DateTimeToUnixTimestamp(yesterday);
            return Convert.ToInt32(dYesterday);

        }

    }
}
