// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Maritime
{
    // ── Catalog DTOs ────────────────────────────────────────────────────────

    [Serializable]
    public sealed class MaritimeZoneDef
    {
        public string zone_id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string zone_type { get; set; } = "coastal";
        public float water_temp_celsius { get; set; } = 12.0f;
        public float radiation_level { get; set; } = 0.0f;
        public float current_strength { get; set; } = 20.0f;
        public float visibility { get; set; } = 80.0f;
        public List<string> dive_sites { get; set; } = new List<string>();
        public string required_equipment_type { get; set; } = "none";
        public float min_depth_meters { get; set; } = 5.0f;
        public float max_depth_meters { get; set; } = 30.0f;
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class MaritimeZonesCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<MaritimeZoneDef> zones { get; set; } = new List<MaritimeZoneDef>();
    }

    // ── Enums ────────────────────────────────────────────────────────────────

    public enum DiveSiteType
    {
        CoastalShallows = 0,
        DeepOcean = 1,
        SunkenVessel = 2,
        UnderwaterCave = 3,
        ContaminatedZone = 4,
        FloodedBunker = 5
    }

    public enum DiveSiteDiscoveryStatus
    {
        Undiscovered = 0,
        Discovered = 1,
        Explored = 2,
        FullySalvaged = 3
    }

    public enum MaritimeExpeditionStatus
    {
        Planned = 0,
        InProgress = 1,
        Completed = 2,
        Failed = 3,
        Aborted = 4
    }

    public enum MaritimeHazardType
    {
        StrongCurrent = 0,
        UnderwaterCollapse = 1,
        RadiationHotspot = 2,
        Entrapment = 3,
        PressureDepth = 4,
        ContaminatedWater = 5,
        MarineCreature = 6,
        EquipmentFailure = 7
    }

    public enum MaritimeHazardOutcome
    {
        Avoided = 0,
        MinorInjury = 1,
        MajorInjury = 2,
        Fatal = 3,
        EquipmentLost = 4
    }

    // ── State Records & Entities ─────────────────────────────────────────────

    [Serializable]
    public sealed class DiveSiteRecord
    {
        public string SiteId { get; set; } = string.Empty;
        public string SiteName { get; set; } = string.Empty;
        public string ZoneId { get; set; } = string.Empty;
        public DiveSiteType Type { get; set; } = DiveSiteType.CoastalShallows;
        public float DepthMeters { get; set; } = 15.0f;
        public float HazardLevel { get; set; } = 20.0f;
        public DiveSiteDiscoveryStatus Status { get; set; } = DiveSiteDiscoveryStatus.Undiscovered;
        public int DiscoveredDay { get; set; } = 1;
        public int ExplorationCount { get; set; } = 0;
        public int MaxExplorations { get; set; } = 3;
        public List<string> LootItemIds { get; set; } = new List<string>();
        public string Coordinates { get; set; } = string.Empty;

        public bool IsFullySalvaged => ExplorationCount >= MaxExplorations;
    }

    [Serializable]
    public sealed class MaritimeHazardEvent
    {
        public string EventId { get; set; } = string.Empty;
        public MaritimeHazardType HazardType { get; set; } = MaritimeHazardType.StrongCurrent;
        public MaritimeHazardOutcome Outcome { get; set; } = MaritimeHazardOutcome.Avoided;
        public string DiverId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float RadiationDose { get; set; } = 0.0f;
        public float DamageAmount { get; set; } = 0.0f;
        public int Day { get; set; } = 1;
    }

    [Serializable]
    public sealed class MaritimeExpeditionRecord
    {
        public string ExpeditionId { get; set; } = string.Empty;
        public string TargetSiteId { get; set; } = string.Empty;
        public List<string> AssignedDivers { get; set; } = new List<string>();
        public List<string> EquipmentIds { get; set; } = new List<string>();
        public int StartDay { get; set; } = 1;
        public int CompletedDay { get; set; } = 1;
        public MaritimeExpeditionStatus Status { get; set; } = MaritimeExpeditionStatus.Planned;
        public float DurationHours { get; set; } = 6.0f;
        public List<string> LootCollected { get; set; } = new List<string>();
        public List<MaritimeHazardEvent> HazardsEncountered { get; set; } = new List<MaritimeHazardEvent>();
    }

    [Serializable]
    public sealed class DivingEquipmentRecord
    {
        public string EquipmentId { get; set; } = string.Empty;
        public string EquipmentType { get; set; } = "basic_dive_suit";
        public float Condition { get; set; } = 100.0f;
        public float MaxDepthRating { get; set; } = 50.0f;
        public float ProtectionRating { get; set; } = 50.0f;
        public bool IsEquipped { get; set; } = false;
    }

    [Serializable]
    public sealed class MaritimeExplorationState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<string> DiscoveredZoneIds { get; set; } = new List<string>();
        public List<DiveSiteRecord> Sites { get; set; } = new List<DiveSiteRecord>();
        public List<MaritimeExpeditionRecord> Expeditions { get; set; } = new List<MaritimeExpeditionRecord>();
        public List<DivingEquipmentRecord> Equipment { get; set; } = new List<DivingEquipmentRecord>();
        public List<MaritimeHazardEvent> RecentHazards { get; set; } = new List<MaritimeHazardEvent>();
    }

    // ── Pure Domain Authority ────────────────────────────────────────────────

    /// <summary>
    /// Plan 207 — Maritime &amp; Underwater Exploration Expansion System.
    /// Manages discoverable maritime coastal/oceanic zones, dive site registers with finite
    /// salvage tracking, expedition planning with diver and depth-gear validation,
    /// deterministic hazard evaluations, and loot/radiation handoffs.
    /// </summary>
    public sealed class MaritimeExplorationSystem
    {
        private readonly MaritimeExplorationState _state;
        private readonly Dictionary<string, MaritimeZoneDef> _zoneDefs = new Dictionary<string, MaritimeZoneDef>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, DiveSiteRecord> _siteLookup = new Dictionary<string, DiveSiteRecord>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, DivingEquipmentRecord> _equipmentLookup = new Dictionary<string, DivingEquipmentRecord>(StringComparer.OrdinalIgnoreCase);

        public event Action<DiveSiteRecord>? OnSiteDiscovered;
        public event Action<MaritimeHazardEvent>? OnHazardEncountered;
        public event Action<MaritimeExpeditionRecord>? OnExpeditionCompleted;

        public Action<string, List<string>>? InventoryLootDeliverer;
        public Action<string, float>? RadiationApplier;
        public Action<string, float>? InjuryApplier;

        public int DiscoveredZoneCount => _state.DiscoveredZoneIds.Count;
        public int DiscoveredSiteCount => _state.Sites.Count(s => s.Status != DiveSiteDiscoveryStatus.Undiscovered);
        public int TotalSiteCount => _state.Sites.Count;
        public int CompletedExpeditionCount => _state.Expeditions.Count(e => e.Status == MaritimeExpeditionStatus.Completed);
        public IReadOnlyList<DiveSiteRecord> Sites => _state.Sites;
        public IReadOnlyList<MaritimeExpeditionRecord> Expeditions => _state.Expeditions;
        public IReadOnlyList<DivingEquipmentRecord> Equipment => _state.Equipment;
        public IReadOnlyList<MaritimeHazardEvent> RecentHazards => _state.RecentHazards;

        public MaritimeExplorationSystem(MaritimeExplorationState? state = null)
        {
            _state = state ?? new MaritimeExplorationState();
            RebuildLookups();
        }

        private void RebuildLookups()
        {
            _siteLookup.Clear();
            if (_state.Sites != null)
            {
                foreach (var s in _state.Sites)
                {
                    if (!string.IsNullOrEmpty(s.SiteId))
                        _siteLookup[s.SiteId] = s;
                }
            }

            _equipmentLookup.Clear();
            if (_state.Equipment != null)
            {
                foreach (var eq in _state.Equipment)
                {
                    if (!string.IsNullOrEmpty(eq.EquipmentId))
                        _equipmentLookup[eq.EquipmentId] = eq;
                }
            }
        }

        // ── Catalog Management ───────────────────────────────────────────────

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Catalog JSON cannot be null or empty", nameof(json));

            var catalog = JsonSerializer.Deserialize<MaritimeZonesCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (catalog?.zones == null) return;

            _zoneDefs.Clear();
            foreach (var zone in catalog.zones)
            {
                if (!string.IsNullOrEmpty(zone.zone_id))
                {
                    _zoneDefs[zone.zone_id] = zone;
                }
            }
        }

        public IReadOnlyList<MaritimeZoneDef> GetAllZoneDefs() => _zoneDefs.Values.ToList();

        public MaritimeZoneDef? GetZoneDef(string zoneId)
        {
            if (string.IsNullOrEmpty(zoneId)) return null;
            _zoneDefs.TryGetValue(zoneId, out var def);
            return def;
        }

        public bool DiscoverZone(string zoneId)
        {
            if (string.IsNullOrEmpty(zoneId)) return false;
            if (_state.DiscoveredZoneIds.Contains(zoneId, StringComparer.OrdinalIgnoreCase))
                return false;

            _state.DiscoveredZoneIds.Add(zoneId);

            // Auto-discover contained sites if in zone def
            if (_zoneDefs.TryGetValue(zoneId, out var def) && def.dive_sites != null)
            {
                foreach (var siteId in def.dive_sites)
                {
                    if (_siteLookup.TryGetValue(siteId, out var site) && site.Status == DiveSiteDiscoveryStatus.Undiscovered)
                    {
                        site.Status = DiveSiteDiscoveryStatus.Discovered;
                        OnSiteDiscovered?.Invoke(site);
                    }
                }
            }

            return true;
        }

        public bool IsZoneDiscovered(string zoneId) =>
            !string.IsNullOrEmpty(zoneId) && _state.DiscoveredZoneIds.Contains(zoneId, StringComparer.OrdinalIgnoreCase);

        // ── Dive Site Management ─────────────────────────────────────────────

        public DiveSiteRecord RegisterDiveSite(
            string siteId,
            string siteName,
            string zoneId,
            DiveSiteType type,
            float depthMeters,
            float hazardLevel,
            int maxExplorations = 3,
            IEnumerable<string>? lootItemIds = null)
        {
            if (string.IsNullOrWhiteSpace(siteId)) throw new ArgumentNullException(nameof(siteId));

            if (_siteLookup.TryGetValue(siteId, out var existing))
                return existing;

            var site = new DiveSiteRecord
            {
                SiteId = siteId,
                SiteName = siteName,
                ZoneId = zoneId,
                Type = type,
                DepthMeters = Math.Clamp(depthMeters, 1.0f, 500.0f),
                HazardLevel = Math.Clamp(hazardLevel, 0.0f, 100.0f),
                Status = DiveSiteDiscoveryStatus.Undiscovered,
                MaxExplorations = Math.Max(1, maxExplorations),
                LootItemIds = lootItemIds != null ? new List<string>(lootItemIds) : new List<string>()
            };

            _state.Sites.Add(site);
            _siteLookup[siteId] = site;
            return site;
        }

        public bool DiscoverSite(string siteId, int day = 1)
        {
            if (string.IsNullOrEmpty(siteId)) return false;
            if (!_siteLookup.TryGetValue(siteId, out var site)) return false;

            if (site.Status != DiveSiteDiscoveryStatus.Undiscovered)
                return false;

            site.Status = DiveSiteDiscoveryStatus.Discovered;
            site.DiscoveredDay = day;
            OnSiteDiscovered?.Invoke(site);
            return true;
        }

        public DiveSiteRecord? GetSite(string siteId)
        {
            if (string.IsNullOrEmpty(siteId)) return null;
            _siteLookup.TryGetValue(siteId, out var s);
            return s;
        }

        // ── Equipment Management ─────────────────────────────────────────────

        public DivingEquipmentRecord RegisterEquipment(
            string equipmentId,
            string equipmentType,
            float depthRating = 50.0f,
            float protectionRating = 50.0f)
        {
            if (string.IsNullOrWhiteSpace(equipmentId)) throw new ArgumentNullException(nameof(equipmentId));

            if (_equipmentLookup.TryGetValue(equipmentId, out var existing))
                return existing;

            var eq = new DivingEquipmentRecord
            {
                EquipmentId = equipmentId,
                EquipmentType = equipmentType,
                Condition = 100.0f,
                MaxDepthRating = depthRating,
                ProtectionRating = protectionRating,
                IsEquipped = false
            };

            _state.Equipment.Add(eq);
            _equipmentLookup[equipmentId] = eq;
            return eq;
        }

        public DivingEquipmentRecord? GetEquipment(string equipmentId)
        {
            if (string.IsNullOrEmpty(equipmentId)) return null;
            _equipmentLookup.TryGetValue(equipmentId, out var eq);
            return eq;
        }

        // ── Expedition Planning & Execution ──────────────────────────────────

        public bool ValidateExpedition(
            string siteId,
            IReadOnlyList<string> diverIds,
            IReadOnlyList<string> equipmentIds,
            out string validationMessage)
        {
            if (diverIds == null || diverIds.Count == 0)
            {
                validationMessage = "Expedition requires at least one assigned diver.";
                return false;
            }

            if (!_siteLookup.TryGetValue(siteId, out var site))
            {
                validationMessage = $"Target dive site '{siteId}' does not exist.";
                return false;
            }

            if (site.IsFullySalvaged)
            {
                validationMessage = $"Dive site '{site.SiteName}' is fully salvaged.";
                return false;
            }

            // Verify depth rating of equipped gear
            float maxGearDepth = 0f;
            if (equipmentIds != null)
            {
                foreach (var eqId in equipmentIds)
                {
                    if (_equipmentLookup.TryGetValue(eqId, out var eq))
                    {
                        if (eq.MaxDepthRating > maxGearDepth)
                            maxGearDepth = eq.MaxDepthRating;
                    }
                }
            }

            if (site.DepthMeters > 30.0f && maxGearDepth < site.DepthMeters)
            {
                validationMessage = $"Equipped diving gear rating ({maxGearDepth}m) is insufficient for site depth ({site.DepthMeters}m).";
                return false;
            }

            // Zone access gear gate
            if (!string.IsNullOrEmpty(site.ZoneId) && _zoneDefs.TryGetValue(site.ZoneId, out var zone))
            {
                if (!string.Equals(zone.required_equipment_type, "none", StringComparison.OrdinalIgnoreCase))
                {
                    bool hasRequiredGear = equipmentIds != null && equipmentIds.Any(eqId =>
                        _equipmentLookup.TryGetValue(eqId, out var eq) &&
                        string.Equals(eq.EquipmentType, zone.required_equipment_type, StringComparison.OrdinalIgnoreCase));

                    if (!hasRequiredGear)
                    {
                        validationMessage = $"Zone '{zone.name}' requires '{zone.required_equipment_type}' gear.";
                        return false;
                    }
                }
            }

            validationMessage = "Expedition ready.";
            return true;
        }

        public (bool success, string message, MaritimeExpeditionRecord? expedition) PlanExpedition(
            string targetSiteId,
            IReadOnlyList<string> diverIds,
            IReadOnlyList<string> equipmentIds,
            int day)
        {
            if (!ValidateExpedition(targetSiteId, diverIds, equipmentIds, out var reason))
            {
                return (false, reason, null);
            }

            var exp = new MaritimeExpeditionRecord
            {
                ExpeditionId = $"m_exp_{_state.NextSequence++}",
                TargetSiteId = targetSiteId,
                AssignedDivers = new List<string>(diverIds),
                EquipmentIds = equipmentIds != null ? new List<string>(equipmentIds) : new List<string>(),
                StartDay = day,
                Status = MaritimeExpeditionStatus.Planned,
                DurationHours = 6.0f
            };

            _state.Expeditions.Add(exp);
            return (true, "Maritime expedition successfully planned.", exp);
        }

        public (bool success, string message) ExecuteExpedition(
            string expeditionId,
            int currentDay,
            int diverSkill = 50,
            int seed = 12345)
        {
            var exp = _state.Expeditions.FirstOrDefault(e => string.Equals(e.ExpeditionId, expeditionId, StringComparison.OrdinalIgnoreCase));
            if (exp == null) return (false, "Expedition not found.");

            if (exp.Status != MaritimeExpeditionStatus.Planned)
                return (false, $"Expedition is already in status '{exp.Status}'.");

            if (!_siteLookup.TryGetValue(exp.TargetSiteId, out var site))
                return (false, "Target site not found.");

            _zoneDefs.TryGetValue(site.ZoneId, out var zone);

            exp.Status = MaritimeExpeditionStatus.InProgress;
            exp.CompletedDay = currentDay;

            // 1. Evaluate Hazards Deterministically
            var hazards = EvaluateHazards(site, zone, exp.AssignedDivers, exp.EquipmentIds, diverSkill, seed);
            exp.HazardsEncountered.AddRange(hazards);
            _state.RecentHazards.AddRange(hazards);

            bool criticalFailure = hazards.Any(h => h.Outcome == MaritimeHazardOutcome.Fatal);

            // 2. Resolve Loot & Salvage if not catastrophic
            if (!criticalFailure)
            {
                exp.Status = MaritimeExpeditionStatus.Completed;
                site.ExplorationCount++;
                if (site.ExplorationCount >= site.MaxExplorations)
                {
                    site.Status = DiveSiteDiscoveryStatus.FullySalvaged;
                }
                else
                {
                    site.Status = DiveSiteDiscoveryStatus.Explored;
                }

                // Collect loot from site loot table
                if (site.LootItemIds.Count > 0)
                {
                    int lootCount = Math.Min(2, site.LootItemIds.Count);
                    for (int i = 0; i < lootCount; i++)
                    {
                        int lootIdx = StableHash.NonNegativeRemainder(
                            unchecked(seed + i * 37),
                            site.LootItemIds.Count);
                        string item = site.LootItemIds[lootIdx];
                        exp.LootCollected.Add(item);
                    }

                    if (exp.AssignedDivers.Count > 0 && InventoryLootDeliverer != null)
                    {
                        InventoryLootDeliverer(exp.AssignedDivers[0], exp.LootCollected);
                    }
                }
            }
            else
            {
                exp.Status = MaritimeExpeditionStatus.Failed;
            }

            // 3. Degrade Equipment
            foreach (var eqId in exp.EquipmentIds)
            {
                if (_equipmentLookup.TryGetValue(eqId, out var eq))
                {
                    eq.Condition = Math.Max(0.0f, eq.Condition - 10.0f);
                }
            }

            OnExpeditionCompleted?.Invoke(exp);
            return (true, exp.Status == MaritimeExpeditionStatus.Completed ? "Expedition completed successfully." : "Expedition encountered critical failure.");
        }

        private List<MaritimeHazardEvent> EvaluateHazards(
            DiveSiteRecord site,
            MaritimeZoneDef? zone,
            IReadOnlyList<string> divers,
            IReadOnlyList<string> equipmentIds,
            int diverSkill,
            int seed)
        {
            var results = new List<MaritimeHazardEvent>();
            float totalHazardRisk = site.HazardLevel;
            if (zone != null)
            {
                totalHazardRisk += (zone.current_strength * 0.3f);
            }

            // Skill mitigates hazard
            float effectiveRisk = Math.Clamp(totalHazardRisk - (diverSkill * 0.4f), 5.0f, 95.0f);

            // Deterministic pseudo-random roll. StableHash keeps the outcome
            // identical across processes; the remainder helper is int.Min-safe.
            int hash = unchecked((seed * 397) ^ StableHash.Of(site.SiteId));
            int roll = StableHash.NonNegativeRemainder(hash, 100);

            if (roll < effectiveRisk)
            {
                // Hazard triggered
                string leadDiver = divers.Count > 0 ? divers[0] : "diver_unknown";
                var hazardType = DetermineHazardType(site.Type, hash);
                var outcome = MaritimeHazardOutcome.Avoided;

                float radiationDose = (zone != null ? zone.radiation_level * 0.5f : 0f);

                if (diverSkill >= 75)
                {
                    outcome = MaritimeHazardOutcome.Avoided;
                }
                else if (diverSkill >= 40)
                {
                    outcome = MaritimeHazardOutcome.MinorInjury;
                    InjuryApplier?.Invoke(leadDiver, 15.0f);
                }
                else
                {
                    outcome = MaritimeHazardOutcome.MajorInjury;
                    InjuryApplier?.Invoke(leadDiver, 40.0f);
                }

                if (radiationDose > 0f)
                {
                    RadiationApplier?.Invoke(leadDiver, radiationDose);
                }

                var ev = new MaritimeHazardEvent
                {
                    EventId = $"m_haz_{_state.NextSequence++}",
                    HazardType = hazardType,
                    Outcome = outcome,
                    DiverId = leadDiver,
                    Description = $"Diver encountered {hazardType} at depth {site.DepthMeters}m.",
                    RadiationDose = radiationDose,
                    DamageAmount = outcome == MaritimeHazardOutcome.MinorInjury ? 15f : (outcome == MaritimeHazardOutcome.MajorInjury ? 40f : 0f),
                    Day = 1
                };

                results.Add(ev);
                OnHazardEncountered?.Invoke(ev);
            }

            return results;
        }

        private static MaritimeHazardType DetermineHazardType(DiveSiteType siteType, int hash)
        {
            int subRoll = hash % 4;
            return siteType switch
            {
                DiveSiteType.ContaminatedZone => MaritimeHazardType.RadiationHotspot,
                DiveSiteType.DeepOcean => MaritimeHazardType.PressureDepth,
                DiveSiteType.UnderwaterCave => MaritimeHazardType.Entrapment,
                DiveSiteType.SunkenVessel => subRoll == 0 ? MaritimeHazardType.UnderwaterCollapse : MaritimeHazardType.EquipmentFailure,
                _ => MaritimeHazardType.StrongCurrent
            };
        }

        // ── Save / Restore State ─────────────────────────────────────────────

        public MaritimeExplorationState CaptureState()
        {
            var state = new MaritimeExplorationState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                DiscoveredZoneIds = new List<string>(_state.DiscoveredZoneIds),
                Sites = new List<DiveSiteRecord>(_state.Sites.Count),
                Expeditions = new List<MaritimeExpeditionRecord>(_state.Expeditions.Count),
                Equipment = new List<DivingEquipmentRecord>(_state.Equipment.Count),
                RecentHazards = new List<MaritimeHazardEvent>(_state.RecentHazards.Count)
            };

            foreach (var s in _state.Sites)
            {
                state.Sites.Add(new DiveSiteRecord
                {
                    SiteId = s.SiteId,
                    SiteName = s.SiteName,
                    ZoneId = s.ZoneId,
                    Type = s.Type,
                    DepthMeters = s.DepthMeters,
                    HazardLevel = s.HazardLevel,
                    Status = s.Status,
                    DiscoveredDay = s.DiscoveredDay,
                    ExplorationCount = s.ExplorationCount,
                    MaxExplorations = s.MaxExplorations,
                    LootItemIds = new List<string>(s.LootItemIds),
                    Coordinates = s.Coordinates
                });
            }

            foreach (var e in _state.Expeditions)
            {
                state.Expeditions.Add(new MaritimeExpeditionRecord
                {
                    ExpeditionId = e.ExpeditionId,
                    TargetSiteId = e.TargetSiteId,
                    AssignedDivers = new List<string>(e.AssignedDivers),
                    EquipmentIds = new List<string>(e.EquipmentIds),
                    StartDay = e.StartDay,
                    CompletedDay = e.CompletedDay,
                    Status = e.Status,
                    DurationHours = e.DurationHours,
                    LootCollected = new List<string>(e.LootCollected),
                    HazardsEncountered = new List<MaritimeHazardEvent>(e.HazardsEncountered)
                });
            }

            foreach (var eq in _state.Equipment)
            {
                state.Equipment.Add(new DivingEquipmentRecord
                {
                    EquipmentId = eq.EquipmentId,
                    EquipmentType = eq.EquipmentType,
                    Condition = eq.Condition,
                    MaxDepthRating = eq.MaxDepthRating,
                    ProtectionRating = eq.ProtectionRating,
                    IsEquipped = eq.IsEquipped
                });
            }

            foreach (var h in _state.RecentHazards)
            {
                state.RecentHazards.Add(new MaritimeHazardEvent
                {
                    EventId = h.EventId,
                    HazardType = h.HazardType,
                    Outcome = h.Outcome,
                    DiverId = h.DiverId,
                    Description = h.Description,
                    RadiationDose = h.RadiationDose,
                    DamageAmount = h.DamageAmount,
                    Day = h.Day
                });
            }

            return state;
        }

        public void RestoreState(MaritimeExplorationState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.DiscoveredZoneIds.Clear();
            if (state.DiscoveredZoneIds != null)
                _state.DiscoveredZoneIds.AddRange(state.DiscoveredZoneIds);

            _state.Sites.Clear();
            if (state.Sites != null)
                _state.Sites.AddRange(state.Sites);

            _state.Expeditions.Clear();
            if (state.Expeditions != null)
                _state.Expeditions.AddRange(state.Expeditions);

            _state.Equipment.Clear();
            if (state.Equipment != null)
                _state.Equipment.AddRange(state.Equipment);

            _state.RecentHazards.Clear();
            if (state.RecentHazards != null)
                _state.RecentHazards.AddRange(state.RecentHazards);

            RebuildLookups();
        }
    }
}
