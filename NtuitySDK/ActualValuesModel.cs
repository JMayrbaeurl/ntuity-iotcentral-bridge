using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace io.ntuity.api
{
    public class ActualValuesModel
    {
        [JsonProperty("power_consumption")]
        public TimeseriesEntry PowerConsumption { get; set; }

        [JsonProperty("power_consumption_calc")]
        public TimeseriesEntry PowerConsumptionCalc { get; set; }

        [JsonProperty("power_production")]
        public TimeseriesEntry PowerProduction { get; set; }

        [JsonProperty("power_storage")]
        public TimeseriesEntry PowerStorage { get; set; }

        [JsonProperty("power_grid")]
        public TimeseriesEntry PowerGrid { get; set; }

        [JsonProperty("power_charging_stations")]
        public TimeseriesEntry PowerChargingStations { get; set; }

        [JsonProperty("power_heating")]
        public TimeseriesEntry PowerHeating { get; set; }

        [JsonProperty("power_appliances")]
        public TimeseriesEntry PowerAppliances { get; set; }

        [JsonProperty("state_of_charge")]
        public TimeseriesIntEntry StateOfCharge { get; set; }

        [JsonProperty("self_sufficiency")]
        public TimeseriesEntry SelfSufficiency { get; set; }

        [JsonProperty("max_network_utilization")]
        public double MaxNetworkUtilization { get; set; }

        [JsonProperty("consumers_total_count")]
        public int ConsumersTotalCount { get; set; }

        [JsonProperty("consumers_online_count")]
        public int ConsumersOnlineCount { get; set; }

        [JsonProperty("producers_total_count")]
        public int ProducersTotalCount { get; set; }

        [JsonProperty("producers_online_count")]
        public int ProducersOnlineCount { get; set; }

        [JsonProperty("storages_total_count")]
        public int StoragesTotalCount { get; set; }

        [JsonProperty("storages_online_count")]
        public int StoragesOnlineCount { get; set; }

        [JsonProperty("heatings_total_count")]
        public int HeatingsTotalCount { get; set; }

        [JsonProperty("heatings_online_count")]
        public int HeatingsOnlineCount { get; set; }

        [JsonProperty("charging_points_total_count")]
        public int ChargingPointsTotalCount { get; set; }

        [JsonProperty("charging_points_online_count")]
        public int ChargingPointsOnlineCount { get; set; }

        [JsonProperty("grids_total_count")]
        public int GridsTotalCount { get; set; }

        [JsonProperty("grids_online_count")]
        public int GridsOnlineCount { get; set; }

        public IDictionary<string, object> AsDictionary()
        {
            return new Dictionary<string, object>()
            {
                {"power_consumption", PowerConsumption.Value },
                {"power_consumption_calc", PowerConsumptionCalc.Value },
                {"power_production", PowerProduction.Value },
                {"power_storage", PowerStorage.Value },
                {"power_grid", PowerGrid.Value },
                {"power_charging_stations", PowerChargingStations.Value },
                {"power_heating", PowerHeating.Value },
                {"power_appliances", PowerAppliances.Value },
                {"state_of_charge", StateOfCharge.Value },
                {"self_sufficiency", SelfSufficiency.Value },
                {"max_network_utilization", MaxNetworkUtilization },
                {"consumers_total_count", ConsumersTotalCount },
                {"consumers_online_count", ConsumersOnlineCount },
                {"producers_total_count", ProducersTotalCount },
                {"producers_online_count", ProducersOnlineCount },
                {"storages_total_count", StoragesTotalCount },
                {"storages_online_count", StoragesOnlineCount },
                {"heatings_total_count", HeatingsTotalCount },
                {"heatings_online_count", HeatingsOnlineCount },
                {"charging_points_total_count", ChargingPointsTotalCount },
                {"charging_points_online_count", ChargingPointsOnlineCount },
                {"grids_total_count", GridsTotalCount },
                {"grids_online_count", GridsOnlineCount },
                {"ts", DateTime.Now }
            };
        }
    }

    public abstract class AbstractTimeseriesEntry
    {
        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }

    public class TimeseriesEntry : AbstractTimeseriesEntry
    {
        [JsonProperty("value")]
        public Nullable<double> Value { get; set; }
    }
    public class TimeseriesIntEntry : AbstractTimeseriesEntry
    {
        [JsonProperty("value")]
        public Nullable<int> Value { get; set; }
    }
}
