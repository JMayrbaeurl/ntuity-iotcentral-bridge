using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace io.ntuity.api
{
    public class NtuityReader
    {
        private readonly HttpClient httpClient = new HttpClient();

        private string ntuitySiteId;

        private readonly string baseAPIURL = "https://api.ntuity.io/v1/";

        public string NtuitySiteId
        {
            get { return ntuitySiteId; }
            set { ntuitySiteId = value; }
        }

        private string ntuityApiKey;

        public string NtuityApiKey
        {
            get { return ntuityApiKey; }
            set { ntuityApiKey = value; }
        }

        public async Task<string> GetLatestValuesAsString()
        {
            if (!httpClient.DefaultRequestHeaders.Contains("Authorization"))
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ntuityApiKey}");

            return await httpClient.GetStringAsync($"{baseAPIURL}/sites/{ntuitySiteId}/energy-flow/latest");
        }

        public async Task<ActualValuesModel> GetLatestValues()
        {
            string asString = await GetLatestValuesAsString();

            if (asString != null && asString.Length > 0)
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                return JsonConvert.DeserializeObject<ActualValuesModel>(asString, settings);
            } else
            {
                return null;
            }
        }
    }
}
