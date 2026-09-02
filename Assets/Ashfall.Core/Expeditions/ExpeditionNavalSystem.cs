using System;
using System.Collections.Generic;
using Ashfall.Core.World;

namespace Ashfall.Core.Expeditions
{
    [Serializable]
    public sealed class NavalVesselDef
    {
        public string vessel_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public int capacity { get; set; } = 1;
        public float base_speed { get; set; } = 15f;
        public float cargo_capacity { get; set; } = 100f;
        public int crew_min { get; set; } = 1;
        public int crew_max { get; set; } = 2;
        public string propulsion { get; set; } = "Oar";
        public string fuel_item_id { get; set; } = string.Empty;
        public float fuel_rate { get; set; } = 0f;
        public float oar_stamina_cost { get; set; } = 10f;
        public float hull_max_durability { get; set; } = 100f;
        public string corrosion_profile_id { get; set; } = "degrade_profile_watercraft";
        public float draft { get; set; } = 0.5f;
        public float combat_rating { get; set; } = 10f;
        public List<string> repair_materials { get; set; } = new List<string>();
        public List<string> tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class NavalVesselCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<NavalVesselDef> vessels { get; set; } = new List<NavalVesselDef>();
    }

    [Serializable]
    public sealed class NavalVesselInstance
    {
        public string instanceId = string.Empty;
        public string vesselId = string.Empty;
        public string displayName = string.Empty;
        public float hullCondition = 100f;
        public float maxHullCondition = 100f;
        public float currentFuel = 20f;
        public bool isDocked = true;
        public string currentPortId = "loc_holdfast";
    }

    [Serializable]
    public sealed class NavalRouteEstimate
    {
        public string routeId = string.Empty;
        public float effectiveSpeedKmH;
        public float travelHours;
        public float fuelRequired;
        public float staminaRequired;
        public float piracyRisk;
        public bool isClosedByIce;
        public string closureReason = string.Empty;
    }

    public sealed class ExpeditionNavalSystem
    {
        public const string SystemId = "expedition_naval";
        private readonly Dictionary<string, NavalVesselDef> _catalog = new Dictionary<string, NavalVesselDef>(StringComparer.Ordinal);
        private readonly ILog _log;

        public ExpeditionNavalSystem(ILog? log = null)
        {
            _log = log ?? NullLog.Instance;
            RegisterDefaultVessels();
        }

        private void RegisterDefaultVessels()
        {
            RegisterVessel(new NavalVesselDef
            {
                vessel_id = "vessel_improvised_raft",
                display_name = "Scrap-Drum Pontoon Raft",
                capacity = 1,
                base_speed = 12f,
                cargo_capacity = 60f,
                propulsion = "Oar",
                oar_stamina_cost = 15f,
                hull_max_durability = 80f,
                combat_rating = 5f
            });
            RegisterVessel(new NavalVesselDef
            {
                vessel_id = "vessel_rowboat",
                display_name = "Riveted Sheet-Metal Skiff",
                capacity = 2,
                base_speed = 18f,
                cargo_capacity = 150f,
                propulsion = "Oar",
                oar_stamina_cost = 12f,
                hull_max_durability = 120f,
                combat_rating = 12f
            });
            RegisterVessel(new NavalVesselDef
            {
                vessel_id = "vessel_motorboat",
                display_name = "Patrol Interceptor Motorboat",
                capacity = 4,
                base_speed = 42f,
                cargo_capacity = 400f,
                propulsion = "Motor",
                fuel_item_id = "fuel",
                fuel_rate = 2.2f,
                hull_max_durability = 200f,
                combat_rating = 45f
            });
        }

        public void RegisterVessel(NavalVesselDef def)
        {
            if (def != null && !string.IsNullOrEmpty(def.vessel_id))
                _catalog[def.vessel_id] = def;
        }

        public void LoadCatalog(string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent)) return;
            try
            {
                var serializer = new SystemTextJsonSerializer();
                var catalog = serializer.Deserialize<NavalVesselCatalog>(jsonContent);
                if (catalog?.vessels != null)
                {
                    foreach (var v in catalog.vessels)
                        RegisterVessel(v);
                }
            }
            catch (Exception ex)
            {
                _log.Warn($"[NavalSystem] Failed to load naval vessels catalog: {ex.Message}");
            }
        }

        public NavalVesselDef? GetDefinition(string vesselId)
        {
            if (string.IsNullOrEmpty(vesselId)) return null;
            _catalog.TryGetValue(vesselId, out var def);
            return def;
        }

        private int _vesselInstanceCounter;

        public NavalVesselInstance CreateInstance(string vesselId, string instanceId = "")
        {
            var def = GetDefinition(vesselId);
            float maxDurability = def?.hull_max_durability ?? 100f;
            return new NavalVesselInstance
            {
                instanceId = string.IsNullOrEmpty(instanceId) ? $"vessel_{++_vesselInstanceCounter}_{vesselId}" : instanceId,
                vesselId = vesselId,
                displayName = def?.display_name ?? vesselId,
                hullCondition = maxDurability,
                maxHullCondition = maxDurability,
                currentFuel = def?.propulsion == "Motor" ? 20f : 0f,
                isDocked = true
            };
        }

        public NavalRouteEstimate EstimateRoute(NavalVesselInstance vessel, MapRoute route, string freezeState, float weatherFactor = 1.0f)
        {
            var def = GetDefinition(vessel.vesselId);
            var estimate = new NavalRouteEstimate
            {
                routeId = $"{route.From}->{route.To}"
            };

            if (string.Equals(freezeState, "Frozen", StringComparison.OrdinalIgnoreCase))
            {
                estimate.isClosedByIce = true;
                estimate.closureReason = "Waterway frozen solid — vessel navigation impossible.";
                estimate.effectiveSpeedKmH = 0f;
                return estimate;
            }

            float baseSpeed = def?.base_speed ?? 15f;
            float hullFactor = vessel.maxHullCondition > 0f ? Math.Clamp(vessel.hullCondition / vessel.maxHullCondition, 0.1f, 1.0f) : 1.0f;
            float currentMod = 1.0f + (0.35f * Math.Clamp(route.CurrentStrength, -1f, 1f));
            float iceFactor = string.Equals(freezeState, "Restricted", StringComparison.OrdinalIgnoreCase) ? 0.50f : 1.0f;
            float weatherMod = Math.Clamp(weatherFactor, 0.3f, 1.2f);

            float speed = baseSpeed * hullFactor * currentMod * iceFactor * weatherMod;
            estimate.effectiveSpeedKmH = Math.Max(1.0f, speed);

            float travelHours = route.DistanceKm / estimate.effectiveSpeedKmH;
            estimate.travelHours = travelHours;

            if (def != null && def.propulsion == "Motor")
            {
                estimate.fuelRequired = travelHours * def.fuel_rate;
            }
            else if (def != null && def.propulsion == "Oar")
            {
                estimate.staminaRequired = travelHours * def.oar_stamina_cost;
            }

            float combatRating = def?.combat_rating ?? 10f;
            estimate.piracyRisk = Math.Clamp((route.WeatherHazard * 0.4f) + ((100f - combatRating) * 0.002f), 0.02f, 0.70f);

            return estimate;
        }

        public void ApplyWaterCorrosion(NavalVesselInstance vessel, float toxicContamination, EquipmentConditionSystem? conditionSys = null)
        {
            if (vessel == null || toxicContamination <= 0f) return;
            float wear = toxicContamination * 8.0f;
            vessel.hullCondition = Math.Max(0f, vessel.hullCondition - wear);

            if (conditionSys != null)
            {
                conditionSys.ApplyCorrosion(vessel.instanceId, toxicContamination * 10f, "toxic_water");
            }
        }

        public ExpeditionVehicleProfile ProjectToVehicleProfile(NavalVesselInstance vessel)
        {
            var def = GetDefinition(vessel.vesselId);
            float baseSpeed = def?.base_speed ?? 20f;
            float hullRatio = vessel.maxHullCondition > 0f ? Math.Clamp(vessel.hullCondition / vessel.maxHullCondition, 0.2f, 1.0f) : 1.0f;

            return new ExpeditionVehicleProfile
            {
                vehicleId = vessel.vesselId,
                speedMultiplier = (baseSpeed / 20.0f) * hullRatio,
                cargoCapacityKg = def?.cargo_capacity ?? 100f,
                breakdownChancePerTick = vessel.hullCondition < 30f ? (30f - vessel.hullCondition) / 200f : 0.01f,
                fuelPerTravelTick = def?.propulsion == "Motor" ? (def.fuel_rate * 0.5f) : 0f
            };
        }

        public bool RollPiracyEncounter(NavalRouteEstimate estimate, ISeededRng rng)
        {
            if (estimate == null || rng == null) return false;
            return rng.NextDouble() < estimate.piracyRisk;
        }
    }
}
