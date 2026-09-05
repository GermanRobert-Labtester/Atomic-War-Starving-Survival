using System;
using System.Collections.Generic;
using System.Text.Json;
using Ashfall.Core.IO;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Radiation
{
    /// <summary>
    /// Logical location/environment where a survivor is currently exposed.
    /// Used by ExposureEnvironmentResolver to compute physics-based radiation doses.
    /// </summary>
    public enum SurvivorExposureLocation
    {
        /// <summary>Inside the shelter: protected by ceiling/wall attenuation.</summary>
        ShelterInterior,

        /// <summary>Directly outside the shelter perimeter: exposed to surface radiation and weather.</summary>
        ShelterPerimeter,

        /// <summary>Out in the open wasteland surface: exposed to full baseline and storm resuspension.</summary>
        WastelandOutdoors,

        /// <summary>Deployed on an expedition to a specific map node or remote location.</summary>
        Expedition
    }

    /// <summary>
    /// Calculated environmental exposure breakdown for a survivor at a point in time.
    /// Pure DTO recording rates, shielding, weather modifier, and human-readable reason.
    /// </summary>
    public sealed class ExposureEnvironment
    {
        public SurvivorExposureLocation LocationKind { get; set; } = SurvivorExposureLocation.ShelterInterior;
        public string LocationId { get; set; } = string.Empty;
        public float BaseRadRate { get; set; }
        public float WeatherRadModifier { get; set; }
        public float FalloutContamination { get; set; }
        public float ShelterShielding { get; set; }
        public float EffectiveZoneRadLevel { get; set; }
        public string ExposureReason { get; set; } = string.Empty;

        public ExposureContext ToExposureContext(List<WornGear>? wornGear = null)
        {
            return new ExposureContext
            {
                ZoneRadLevel = EffectiveZoneRadLevel,
                ShelterShielding = ShelterShielding,
                WornGear = wornGear ?? new List<WornGear>(),
                ExposureReason = ExposureReason,
                Environment = this
            };
        }

        public override string ToString() =>
            $"{ExposureReason}: Zone={EffectiveZoneRadLevel:F1} mSv/h, Shielding={ShelterShielding:F1} mSv/h";
    }

    /// <summary>
    /// Pure, engine-agnostic resolver that calculates radiation exposure from environment,
    /// position, weather, and shelter shielding without branching on survivor identity.
    /// Adheres to Invariant 1 (engine-agnostic) and Invariant 4 (deterministic).
    /// </summary>
    public sealed class ExposureEnvironmentResolver
    {
        public const float DefaultShelterInteriorRadRate = 2.0f;
        public const float DefaultShelterPerimeterRadRate = 20.0f;
        public const float DefaultWastelandOutdoorRadRate = 40.0f;

        public float ShelterInteriorBaseRadRate { get; set; } = DefaultShelterInteriorRadRate;
        public float ShelterPerimeterBaseRadRate { get; set; } = DefaultShelterPerimeterRadRate;
        public float WastelandOutdoorBaseRadRate { get; set; } = DefaultWastelandOutdoorRadRate;

        /// <summary>Provider for shelter ceiling/wall attenuation (0..1+ multiplier).</summary>
        public Func<float>? ShelterAttenuationProvider { get; set; }

        /// <summary>Provider for outdoor weather radiation modifier (e.g. fallout storm / black rain).</summary>
        public Func<float>? WeatherRadModifierProvider { get; set; }

        /// <summary>Optional provider for location base rads per hour (e.g. from locations.json catalog).</summary>
        public Func<string, float>? LocationRadRateProvider { get; set; }

        /// <summary>Optional provider for location-specific fallout contamination.</summary>
        public Func<string, float>? FalloutContaminationProvider { get; set; }

        /// <summary>Optional external query for survivor location kind and target location id.</summary>
        public Func<string, (SurvivorExposureLocation Kind, string LocationId)>? SurvivorLocationQuery { get; set; }

        private readonly Dictionary<string, (SurvivorExposureLocation Kind, string LocationId)> _explicitLocations =
            new(StringComparer.Ordinal);

        public void SetSurvivorLocation(string survivorId, SurvivorExposureLocation kind, string locationId = "")
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            _explicitLocations[survivorId] = (kind, locationId ?? string.Empty);
        }

        public void ClearSurvivorLocation(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            _explicitLocations.Remove(survivorId);
        }

        public (SurvivorExposureLocation Kind, string LocationId) GetSurvivorLocation(string survivorId)
        {
            if (!string.IsNullOrEmpty(survivorId))
            {
                if (SurvivorLocationQuery != null)
                {
                    var query = SurvivorLocationQuery(survivorId);
                    if (query.Kind != SurvivorExposureLocation.ShelterInterior || !string.IsNullOrEmpty(query.LocationId))
                        return query;
                }

                if (_explicitLocations.TryGetValue(survivorId, out var loc))
                    return loc;
            }

            return (SurvivorExposureLocation.ShelterInterior, string.Empty);
        }

        /// <summary>
        /// Resolve the environmental exposure for a survivor.
        /// Pure function of environment and position; never inspects survivor identity literals.
        /// </summary>
        public ExposureEnvironment Resolve(string survivorId)
        {
            var (kind, locId) = GetSurvivorLocation(survivorId);
            return ResolveForEnvironment(kind, locId);
        }

        /// <summary>
        /// Resolve environmental exposure directly for a specified location kind and location id.
        /// </summary>
        public ExposureEnvironment ResolveForEnvironment(SurvivorExposureLocation kind, string locationId = "")
        {
            float baseRate;
            float weatherMod = 0f;
            float fallout = 0f;
            float shielding = 0f;
            string reason;

            switch (kind)
            {
                case SurvivorExposureLocation.ShelterInterior:
                    baseRate = ShelterInteriorBaseRadRate;
                    float attenuation = ShelterAttenuationProvider != null ? ShelterAttenuationProvider() : 0f;
                    shielding = baseRate * attenuation;
                    reason = "Shelter Interior";
                    break;

                case SurvivorExposureLocation.ShelterPerimeter:
                    baseRate = ShelterPerimeterBaseRadRate;
                    weatherMod = WeatherRadModifierProvider != null ? WeatherRadModifierProvider() : 0f;
                    shielding = 0f;
                    reason = weatherMod > 0f
                        ? $"Shelter Perimeter (Weather +{weatherMod:F0})"
                        : "Shelter Perimeter";
                    break;

                case SurvivorExposureLocation.WastelandOutdoors:
                    baseRate = WastelandOutdoorBaseRadRate;
                    weatherMod = WeatherRadModifierProvider != null ? WeatherRadModifierProvider() : 0f;
                    shielding = 0f;
                    reason = weatherMod > 0f
                        ? $"Wasteland Surface (Weather +{weatherMod:F0})"
                        : "Wasteland Surface";
                    break;

                case SurvivorExposureLocation.Expedition:
                    if (!string.IsNullOrEmpty(locationId) && LocationRadRateProvider != null)
                    {
                        float locRate = LocationRadRateProvider(locationId);
                        baseRate = locRate > 0f ? locRate : WastelandOutdoorBaseRadRate;
                    }
                    else
                    {
                        baseRate = WastelandOutdoorBaseRadRate;
                    }

                    if (!string.IsNullOrEmpty(locationId) && FalloutContaminationProvider != null)
                    {
                        fallout = FalloutContaminationProvider(locationId);
                    }

                    weatherMod = WeatherRadModifierProvider != null ? WeatherRadModifierProvider() : 0f;
                    shielding = 0f;
                    reason = string.IsNullOrEmpty(locationId)
                        ? (weatherMod > 0f ? $"Expedition (Weather +{weatherMod:F0})" : "Expedition")
                        : (weatherMod > 0f ? $"Expedition {locationId} (Weather +{weatherMod:F0})" : $"Expedition {locationId}");
                    break;

                default:
                    baseRate = ShelterInteriorBaseRadRate;
                    reason = "Shelter Interior";
                    break;
            }

            float effectiveZone = MathF.Max(0f, baseRate + weatherMod + fallout);

            return new ExposureEnvironment
            {
                LocationKind = kind,
                LocationId = locationId ?? string.Empty,
                BaseRadRate = baseRate,
                WeatherRadModifier = weatherMod,
                FalloutContamination = fallout,
                ShelterShielding = shielding,
                EffectiveZoneRadLevel = effectiveZone,
                ExposureReason = reason
            };
        }

        /// <summary>
        /// Loads location base rad rates from data authority JSON files.
        /// Inspects locations.json and regional location catalogs.
        /// </summary>
        public static Dictionary<string, float> LoadLocationRadRates(
            string dataDir,
            IFileIO? fileIO = null,
            IJsonSerializer? json = null)
        {
            var result = new Dictionary<string, float>(StringComparer.Ordinal);
            if (string.IsNullOrEmpty(dataDir)) return result;

            fileIO ??= new FileSystemIO();

            string[] candidateFiles =
            {
                "locations.json",
                "duty_roster_locations.json",
                "crossing_locations.json",
                "holdfast_locations.json",
                "verdict_locations.json",
                "year_of_ash_locations.json"
            };

            for (int i = 0; i < candidateFiles.Length; i++)
            {
                string path = fileIO.Combine(dataDir, candidateFiles[i]);
                if (!fileIO.FileExists(path)) continue;

                string raw = fileIO.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) continue;

                try
                {
                    using var doc = JsonDocument.Parse(raw);
                    var root = doc.RootElement;
                    JsonElement locationsArray = default;

                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        locationsArray = root;
                    }
                    else if (root.ValueKind == JsonValueKind.Object)
                    {
                        if (root.TryGetProperty("locations", out var locProp) && locProp.ValueKind == JsonValueKind.Array)
                        {
                            locationsArray = locProp;
                        }
                    }

                    if (locationsArray.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var el in locationsArray.EnumerateArray())
                        {
                            if (!el.TryGetProperty("id", out var idProp)) continue;
                            string? id = idProp.GetString();
                            if (string.IsNullOrEmpty(id) || result.ContainsKey(id)) continue;

                            float rads = 0f;
                            if (el.TryGetProperty("baseRadsPerHour", out var radProp) && radProp.TryGetSingle(out float r))
                            {
                                rads = r;
                            }
                            else if (el.TryGetProperty("dangerLevel", out var dProp) && dProp.TryGetSingle(out float d))
                            {
                                rads = d * 5f;
                            }

                            if (rads > 0f)
                            {
                                result[id] = rads;
                            }
                        }
                    }
                }
                catch
                {
                    // Non-fatal parse failure in optional candidate file
                }
            }

            return result;
        }
    }
}
