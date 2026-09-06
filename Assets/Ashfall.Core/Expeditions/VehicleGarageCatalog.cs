using System;
using System.Collections.Generic;

namespace Ashfall.Core.Expeditions
{
    [Serializable]
    public sealed class VehicleModificationCost
    {
        public string item_id = string.Empty;
        public int amount;
    }

    [Serializable]
    public sealed class VehicleModificationEffects
    {
        public float cargo_capacity_delta;
        public float speed_multiplier_delta;
        public float fuel_consumption_multiplier = 1.0f;
        public float wear_rate_multiplier = 1.0f;
        public int radiation_protection_permille;
    }

    [Serializable]
    public sealed class VehicleModificationDefinition
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public string slot_type = string.Empty;
        public List<string> compatible_vehicle_tags = new List<string>();
        public List<VehicleModificationCost> install_cost = new List<VehicleModificationCost>();
        public int install_labor_ticks;
        public VehicleModificationEffects effects = new VehicleModificationEffects();
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public sealed class VehicleGarageCatalog
    {
        public int schema_version = 1;
        public List<VehicleModificationDefinition> modifications = new List<VehicleModificationDefinition>();
    }

    public static class VehicleGarageCatalogLoader
    {
        public static VehicleGarageCatalog Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Vehicle modifications JSON string cannot be null or empty.", nameof(json));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            var catalog = serializer.Deserialize<VehicleGarageCatalog>(json);
            if (catalog == null)
                throw new InvalidOperationException("Failed to deserialize vehicle modifications catalog.");

            return catalog;
        }
    }
}
