using System;
using System.Text;
using System.Threading.Tasks;
using io.ntuity.api;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Provisioning.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace io.ntuity.tools
{
    public static class NtuityAPIForwarder
    {
        private static string deviceId = Environment.GetEnvironmentVariable("DEVICEID");

        private static string dpsIdScope = Environment.GetEnvironmentVariable("DPSIDSCOPE");

        private static string deviceSymmetricKey = Environment.GetEnvironmentVariable("DEVICESYMMETRICKEY");

        private static readonly NtuityReader ntuityReader = new NtuityReader() { NtuitySiteId = Environment.GetEnvironmentVariable("NTUITY_SITE_ID"), NtuityApiKey = Environment.GetEnvironmentVariable("NTUITY_API_KEY") };

        private static bool doSimpleForwarding = false;

        [FunctionName("NtuityAPIForwarder")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (doSimpleForwarding)
            {
                await RunSimpleForwarding(log);
            } else
            {
                await RunPnPForwarding(log);
            }

            log.LogInformation("Successfully sent Ntuity values to IoT Hub");
        }

        private static async Task RunPnPForwarding(ILogger log)
        {
            ActualValuesModel actualValues = await ntuityReader.GetLatestValues();
            if (actualValues != null)
            {
                string responseMsg = JsonConvert.SerializeObject(actualValues, Formatting.Indented);
                log.LogInformation($"Ntuity values: {responseMsg}");

                log.LogDebug($"Set up the device client.");
                DeviceClient deviceClient = await SetupDeviceClientAsync(log);

                Message msg = PnpHelpers.PnpConvention.CreateMessage(actualValues.AsDictionary(), default, Encoding.UTF8);

                await deviceClient.SendEventAsync(msg);
                await deviceClient.CloseAsync();
            }
        }

        private static async Task RunSimpleForwarding(ILogger log)
        {
            string responseMsg = await GetLatestValuesfromNtuityAPI();
            log.LogInformation($"Ntuity values: {responseMsg}");

            log.LogDebug($"Set up the device client.");
            DeviceClient deviceClient = await SetupDeviceClientAsync(log);

            await deviceClient.SendEventAsync(new Message(Encoding.UTF8.GetBytes(responseMsg)));
            await deviceClient.CloseAsync();
        }

        private static async Task<string> GetLatestValuesfromNtuityAPI()
        {
            return await ntuityReader.GetLatestValuesAsString();
        }

        private static async Task<DeviceClient> SetupDeviceClientAsync(ILogger log)
        {
            DeviceClient deviceClient;

            log.LogDebug($"Initializing via DPS");
            DeviceRegistrationResult dpsRegistrationResult = await ProvisionDeviceAsync(log);
            var authMethod = new DeviceAuthenticationWithRegistrySymmetricKey(dpsRegistrationResult.DeviceId, deviceSymmetricKey);
            deviceClient = InitializeDeviceClient(dpsRegistrationResult.AssignedHub, authMethod, log);
                    
            return deviceClient;
        }

        // Provision a device via DPS, by sending the PnP model Id as DPS payload.
        private static async Task<DeviceRegistrationResult> ProvisionDeviceAsync(ILogger log)
        {
            SecurityProvider symmetricKeyProvider = new SecurityProviderSymmetricKey(deviceId, deviceSymmetricKey, null);
            ProvisioningTransportHandler mqttTransportHandler = new ProvisioningTransportHandlerMqtt();
            ProvisioningDeviceClient pdc = ProvisioningDeviceClient.Create("global.azure-devices-provisioning.net", dpsIdScope, symmetricKeyProvider, mqttTransportHandler);

            return await pdc.RegisterAsync(null);
        }

        // Initialize the device client instance using symmetric key based authentication, over Mqtt protocol (TCP, with fallback over Websocket)
        // and setting the ModelId into ClientOptions. This method also sets a connection status change callback, that will get triggered any time the device's connection status changes.
        private static DeviceClient InitializeDeviceClient(string hostname, IAuthenticationMethod authenticationMethod, ILogger log)
        {
            DeviceClient deviceClient = DeviceClient.Create(hostname, authenticationMethod, TransportType.Mqtt);
            deviceClient.SetConnectionStatusChangesHandler((status, reason) =>
            {
                log.LogDebug($"Connection status change registered - status={status}, reason={reason}.");
            });

            return deviceClient;
        }
    }
}
