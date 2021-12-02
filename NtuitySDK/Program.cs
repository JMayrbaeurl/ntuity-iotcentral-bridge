using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace io.ntuity.api
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            while (true)
            {
                ActualValuesModel latest = new NtuityReader() { NtuitySiteId = "[Enter your site id]", NtuityApiKey = "[Enter your API key]" }.
                    GetLatestValues().GetAwaiter().GetResult();

                Console.WriteLine(JsonConvert.SerializeObject(latest, Formatting.Indented));

                Thread.Sleep(5000);
            }
        }

        static void SimpleTest()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            ActualValuesModel latest = JsonConvert.DeserializeObject<ActualValuesModel>(
                File.ReadAllText("C:\\Dev\\IoT\\NtuityIoTCentralBridge\\NtuitySDK\\Samples\\ActualValuesSample1.json"), settings);

            Console.WriteLine(JsonConvert.SerializeObject(latest, Formatting.Indented));
        }
    }
}
