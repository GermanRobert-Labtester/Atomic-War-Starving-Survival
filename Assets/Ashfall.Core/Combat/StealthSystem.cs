// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 181: Stealth & Camouflage Mechanics
// Pure deterministic simulation: detection risk from weather, light, terrain,
// camouflage ratings, and weapon noise profiles; encounter bypass state machine;
// bounded opening ambushes; and Night Ops tradeoffs.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Random;

namespace Ashfall.Core.Combat
{
    public enum StealthTravelMode
    {
        Standard = 0,
        CarefulStalking = 1,
        NightOps = 2
    }

    [Serializable]
    public sealed class CamouflageGearDef
    {
        public string camo_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string item_id { get; set; } = string.Empty;
        public float camo_rating { get; set; } = 0.5f;
        public List<string> terrain_tags { get; set; } = new List<string>();
        public List<string> weather_tags { get; set; } = new List<string>();
        public float night_modifier { get; set; } = 0.2f;
        public float movement_penalty { get; set; } = 0.05f;
        public float durability_cost { get; set; } = 1.0f;
        public float noise_modifier { get; set; } = 0.0f;
        public string slot { get; set; } = "Chest";
    }

    [Serializable]
    public sealed class CamouflageGearCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<CamouflageGearDef> gear { get; set; } = new List<CamouflageGearDef>();
    }

    [Serializable]
    public sealed class WeaponNoiseProfile
    {
        public string weapon_id { get; set; } = string.Empty;
        public float handling_noise { get; set; } = 0.10f;
        public float melee_noise { get; set; } = 0.20f;
        public float fired_noise { get; set; } = 0.85f;
        public bool is_suppressed { get; set; } = false;
    }

    public sealed class DetectionProfile
    {
        public float BaseDetectionRisk { get; set; }
        public float VisualMod { get; set; }
        public float NoiseMod { get; set; }
        public float WeatherMod { get; set; }
        public float NightMod { get; set; }
        public float FinalProbability { get; set; }
    }

    [Serializable]
    public sealed class PartyStealthState
    {
        public string expeditionId { get; set; } = string.Empty;
        public StealthTravelMode travelMode { get; set; } = StealthTravelMode.Standard;
        public float accumulatedNoise { get; set; } = 0.20f;
        public int consecutiveBypasses { get; set; } = 0;
        public bool isDetected { get; set; } = false;
        public bool hasAmbushAdvantage { get; set; } = false;
        public bool nightOpsActive { get; set; } = false;
        public List<string> equippedCamoIds { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class StealthState
    {
        public int schema_version { get; set; } = 1;
        public Dictionary<string, PartyStealthState> expeditionStealthMap { get; set; } =
            new Dictionary<string, PartyStealthState>(StringComparer.Ordinal);
        public int totalBypasses { get; set; } = 0;
        public int totalAmbushes { get; set; } = 0;
        public int totalDetections { get; set; } = 0;
    }

    public sealed class StealthSystem
    {
        private readonly ISeededRng _rng;
        private readonly Inventory.Inventory _inventory;
        private readonly ILog _log;

        private readonly Dictionary<string, CamouflageGearDef> _gear =
            new Dictionary<string, CamouflageGearDef>(StringComparer.Ordinal);

        private readonly Dictionary<string, WeaponNoiseProfile> _weaponNoise =
            new Dictionary<string, WeaponNoiseProfile>(StringComparer.Ordinal);

        private StealthState _state = new StealthState();

        public event Action<string, string>? OnStealthBroken;
        public event Action<string>? OnBypassSucceeded;
        public event Action<string>? OnAmbushTriggered;

        public StealthState State => _state;

        public StealthSystem(
            ISeededRng rng,
            Inventory.Inventory inventory,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _log = log ?? new NullLog();
        }

        public void RegisterCamouflageGear(CamouflageGearDef gearDef)
        {
            if (gearDef == null || string.IsNullOrWhiteSpace(gearDef.camo_id)) return;
            _gear[gearDef.camo_id] = gearDef;
        }

        public void RegisterWeaponNoise(WeaponNoiseProfile profile)
        {
            if (profile == null || string.IsNullOrWhiteSpace(profile.weapon_id)) return;
            _weaponNoise[profile.weapon_id] = profile;
        }

        public PartyStealthState EnsurePartyStealth(string expeditionId)
        {
            if (_state.expeditionStealthMap.TryGetValue(expeditionId, out var existing))
                return existing;

            var created = new PartyStealthState
            {
                expeditionId = expeditionId,
                travelMode = StealthTravelMode.Standard,
                accumulatedNoise = 0.20f
            };
            _state.expeditionStealthMap[expeditionId] = created;
            return created;
        }

        public void SetTravelMode(string expeditionId, StealthTravelMode mode)
        {
            var party = EnsurePartyStealth(expeditionId);
            party.travelMode = mode;
            party.nightOpsActive = (mode == StealthTravelMode.NightOps);
        }

        public void EquipCamoGear(string expeditionId, string camoId)
        {
            var party = EnsurePartyStealth(expeditionId);
            if (!_gear.ContainsKey(camoId)) return;
            if (!party.equippedCamoIds.Contains(camoId))
            {
                party.equippedCamoIds.Add(camoId);
            }
        }

        public DetectionProfile CalculateDetectionRisk(
            string expeditionId,
            string weatherKind,
            bool isNight,
            string terrainTag,
            List<string>? observerSenses = null)
        {
            var party = EnsurePartyStealth(expeditionId);
            observerSenses ??= new List<string>();

            float baseRisk = 0.50f;
            float visualMod = 0.0f;
            float noiseMod = party.accumulatedNoise;
            float weatherMod = 0.0f;
            float nightMod = isNight ? -0.25f : 0.0f;

            // Travel mode modifiers
            if (party.travelMode == StealthTravelMode.CarefulStalking)
            {
                visualMod -= 0.15f;
                noiseMod -= 0.10f;
            }
            else if (party.travelMode == StealthTravelMode.NightOps)
            {
                nightMod -= 0.20f;
                noiseMod += 0.05f; // Night clumsiness without illumination
            }

            // Weather concealment modifiers
            if (weatherKind.Contains("fog", StringComparison.OrdinalIgnoreCase) ||
                weatherKind.Contains("ash", StringComparison.OrdinalIgnoreCase))
            {
                weatherMod -= 0.20f;
            }
            else if (weatherKind.Contains("rain", StringComparison.OrdinalIgnoreCase))
            {
                weatherMod -= 0.10f;
                noiseMod -= 0.10f; // Rain masks footstep noises
            }

            // Camouflage gear contributions
            float totalCamo = 0.0f;
            foreach (var camoId in party.equippedCamoIds)
            {
                if (_gear.TryGetValue(camoId, out var def))
                {
                    float matchMult = 1.0f;
                    if (def.terrain_tags.Count > 0 && def.terrain_tags.Contains(terrainTag))
                        matchMult += 0.5f;

                    totalCamo += def.camo_rating * matchMult;
                    noiseMod += def.noise_modifier;

                    if (isNight) totalCamo += def.night_modifier;
                }
            }
            visualMod -= Math.Min(0.50f, totalCamo * 0.40f);

            // Observer senses adaptations
            if (observerSenses.Contains("sense_hearing"))
            {
                noiseMod *= 1.6f;
            }
            if (observerSenses.Contains("sense_scent"))
            {
                // Scent ignores visual camouflage
                visualMod = Math.Max(-0.10f, visualMod);
                baseRisk += 0.15f;
            }

            float finalProb = Math.Clamp(baseRisk + visualMod + noiseMod + weatherMod + nightMod, 0.05f, 0.95f);

            return new DetectionProfile
            {
                BaseDetectionRisk = baseRisk,
                VisualMod = visualMod,
                NoiseMod = noiseMod,
                WeatherMod = weatherMod,
                NightMod = nightMod,
                FinalProbability = finalProb
            };
        }

        public bool BypassEncounter(
            string expeditionId,
            string weatherKind,
            bool isNight,
            string terrainTag,
            List<string>? observerSenses = null)
        {
            var party = EnsurePartyStealth(expeditionId);
            var profile = CalculateDetectionRisk(expeditionId, weatherKind, isNight, terrainTag, observerSenses);

            float roll = (float)_rng.NextDouble();
            if (roll > profile.FinalProbability)
            {
                // Successful stealth bypass!
                party.consecutiveBypasses++;
                party.hasAmbushAdvantage = true;
                _state.totalBypasses++;
                OnBypassSucceeded?.Invoke(expeditionId);
                return true;
            }
            else
            {
                // Stealth broken!
                party.isDetected = true;
                party.hasAmbushAdvantage = false;
                _state.totalDetections++;
                OnStealthBroken?.Invoke(expeditionId, "detected_by_observer");
                return false;
            }
        }

        public bool TriggerAmbush(string expeditionId)
        {
            var party = EnsurePartyStealth(expeditionId);
            if (!party.hasAmbushAdvantage || party.isDetected) return false;

            party.hasAmbushAdvantage = false; // Single opening round advantage
            _state.totalAmbushes++;
            OnAmbushTriggered?.Invoke(expeditionId);
            return true;
        }

        public StealthState CaptureState() => _state;

        public void RestoreState(StealthState state)
        {
            if (state == null) return;
            _state = state;
        }
    }
}
