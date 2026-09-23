// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.IO;

namespace Ashfall.Core.Legacy
{
    [Serializable]
    public sealed class CampaignLegacy
    {
        public string campaignId { get; set; } = string.Empty;
        public string endingId { get; set; } = string.Empty;
        public int daysSurvived { get; set; }
        public int survivorCount { get; set; }
        public int deathsRecorded { get; set; }
        public List<string> shelterImprovements { get; set; } = new List<string>();
        public List<string> legacyTraits { get; set; } = new List<string>();
        public Dictionary<string, float> factionStandings { get; set; } = new Dictionary<string, float>();
        public List<string> campaignFlags { get; set; } = new List<string>();
        public int completionDay { get; set; }
    }

    [Serializable]
    public sealed class LegacyTrait
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string source { get; set; } = "survivor"; // survivor, shelter, faction, ending
        public string effect_type { get; set; } = "morale_bonus";
        public float magnitude { get; set; }
        public string evolution_target_id { get; set; } = string.Empty;
        public int generation { get; set; }
        public bool inherited { get; set; }
    }

    [Serializable]
    public sealed class CampaignLegacyState
    {
        public int schema_version { get; set; } = 1;
        public List<CampaignLegacy> completedCampaigns { get; set; } = new List<CampaignLegacy>();
        public List<LegacyTrait> activeLegacyTraits { get; set; } = new List<LegacyTrait>();
        public List<string> inheritedImprovements { get; set; } = new List<string>();
        public List<string> campaignHistoryLog { get; set; } = new List<string>();
    }

    public sealed class StartingCampaignContext
    {
        public float startingResourcesBonus { get; set; }
        public float startingMoraleBonus { get; set; }
        public float startingDefenseBonus { get; set; }
        public int startingCapacityBonus { get; set; }
        public Dictionary<string, float> factionModifiers { get; set; } = new Dictionary<string, float>();
        public List<LegacyTrait> inheritedTraits { get; set; } = new List<LegacyTrait>();
    }

    public struct CampaignLegacyCensus
    {
        public readonly int CompletedCampaignsCount;
        public readonly int ActiveTraitsCount;
        public readonly int InheritedImprovementsCount;
        public readonly int CatalogTraitsCount;

        public CampaignLegacyCensus(int completed, int activeTraits, int inheritedImprovements, int catalogTraits)
        {
            CompletedCampaignsCount = completed;
            ActiveTraitsCount = activeTraits;
            InheritedImprovementsCount = inheritedImprovements;
            CatalogTraitsCount = catalogTraits;
        }
    }

    /// <summary>
    /// Pure domain authority governing cross-campaign legacy traits, heirloom succession,
    /// shelter persistence, and New Game+ multi-generational continuity (Plan 140 / C2[30] / DEC-141).
    /// </summary>
    public sealed class CampaignLegacySystem
    {
        public const string SystemId = "campaign_legacy_system";
        public const string DefaultCatalogFileName = "legacy_traits.json";

        public CampaignLegacyCensus GetCensus()
        {
            return new CampaignLegacyCensus(
                _state.completedCampaigns.Count,
                _state.activeLegacyTraits.Count,
                _state.inheritedImprovements.Count,
                _catalog.Count);
        }


        private readonly Dictionary<string, LegacyTrait> _catalog = new Dictionary<string, LegacyTrait>(StringComparer.OrdinalIgnoreCase);
        private readonly ISeededRng? _rng;
        private CampaignLegacyState _state;

        public Action<CampaignLegacy>? OnCampaignArchivedSeam { get; set; }
        public Action<LegacyTrait>? OnLegacyTraitInheritedSeam { get; set; }
        public Action<string, string>? OnTraitEvolvedSeam { get; set; }
        public Action<StartingCampaignContext>? OnNewGameLegacyAppliedSeam { get; set; }

        public CampaignLegacySystem(CampaignLegacyState? state = null, ISeededRng? rng = null)
        {
            _state = state ?? new CampaignLegacyState();
            _rng = rng;
        }

        public CampaignLegacyState State => _state;
        public IReadOnlyCollection<LegacyTrait> CatalogTraits => _catalog.Values;

        public void RegisterTrait(LegacyTrait trait)
        {
            if (trait == null || string.IsNullOrEmpty(trait.id)) return;
            _catalog[trait.id] = trait;
        }

        public bool TryGetTrait(string traitId, out LegacyTrait? trait)
        {
            return _catalog.TryGetValue(traitId, out trait);
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var list = CatalogLocator.LoadWrappedList<LegacyTrait>(json, SystemTextJsonSerializer.Options);
                if (list != null)
                {
                    foreach (var t in list)
                    {
                        RegisterTrait(t);
                    }
                }
            }
            catch
            {
                // Catalog parse fallback
            }
        }

        public static CampaignLegacySystem LoadFromDirectory(string dataDir, IFileIO fileIO, ISeededRng? rng = null)
        {
            var system = new CampaignLegacySystem(null, rng);
            if (fileIO != null && !string.IsNullOrEmpty(dataDir))
            {
                string path = fileIO.Combine(dataDir, DefaultCatalogFileName);
                if (fileIO.FileExists(path))
                {
                    system.LoadCatalog(fileIO.ReadAllText(path));
                }
            }
            return system;
        }

        /// <summary>
        /// Archives a completed campaign and rolls inheritance for legacy traits,
        /// carrying forward shelter improvements, faction memory, and survivor heritage.
        /// </summary>
        public void ArchiveCampaign(CampaignLegacy legacy, ISeededRng? rng = null)
        {
            if (legacy == null) return;
            var activeRng = rng ?? _rng;

            _state.completedCampaigns.Add(legacy);
            _state.campaignHistoryLog.Add($"Campaign {legacy.campaignId} concluded on day {legacy.completionDay} with ending '{legacy.endingId}'. Survived {legacy.daysSurvived} days with {legacy.survivorCount} survivors.");

            // 1. Shelter improvements persist 100%
            if (legacy.shelterImprovements != null)
            {
                foreach (var imp in legacy.shelterImprovements)
                {
                    if (!_state.inheritedImprovements.Contains(imp))
                    {
                        _state.inheritedImprovements.Add(imp);
                    }
                }
            }

            // 2. Trait inheritance evaluation
            if (legacy.legacyTraits != null)
            {
                foreach (var traitId in legacy.legacyTraits)
                {
                    if (!_catalog.TryGetValue(traitId, out var template))
                        continue;

                    bool shouldInherit = false;
                    if (string.Equals(template.source, "survivor", StringComparison.OrdinalIgnoreCase))
                    {
                        // Survivor traits have 50% inheritance chance
                        double roll = activeRng != null ? activeRng.NextDouble() : 0.6;
                        shouldInherit = roll < 0.50;
                    }
                    else
                    {
                        // Shelter, Faction, and Ending traits inherit 100%
                        shouldInherit = true;
                    }

                    if (shouldInherit)
                    {
                        var inherited = CloneTrait(template);
                        inherited.inherited = true;
                        inherited.generation = _state.completedCampaigns.Count;

                        // Check for multi-campaign trait evolution (after 3 generations)
                        if (!string.IsNullOrEmpty(inherited.evolution_target_id) && inherited.generation >= 3)
                        {
                            if (_catalog.TryGetValue(inherited.evolution_target_id, out var evolvedTemplate))
                            {
                                string prevId = inherited.id;
                                inherited = CloneTrait(evolvedTemplate);
                                inherited.inherited = true;
                                inherited.generation = _state.completedCampaigns.Count;
                                _state.activeLegacyTraits.RemoveAll(t => t.id == prevId);
                                OnTraitEvolvedSeam?.Invoke(prevId, inherited.id);
                            }
                        }

                        // Avoid exact duplicates in active legacy traits
                        if (!_state.activeLegacyTraits.Any(t => t.id == inherited.id))
                        {
                            _state.activeLegacyTraits.Add(inherited);
                            OnLegacyTraitInheritedSeam?.Invoke(inherited);
                        }
                    }
                }
            }

            // 3. Faction historical memory (allied >= 50, hostile <= -50)
            if (legacy.factionStandings != null)
            {
                foreach (var kvp in legacy.factionStandings)
                {
                    if (kvp.Value >= 50f && !_state.activeLegacyTraits.Any(t => t.id == "trait_old_alliance_vanguard"))
                    {
                        if (_catalog.TryGetValue("trait_old_alliance_vanguard", out var allianceTrait))
                        {
                            var inherited = CloneTrait(allianceTrait);
                            inherited.inherited = true;
                            inherited.generation = _state.completedCampaigns.Count;
                            _state.activeLegacyTraits.Add(inherited);
                            OnLegacyTraitInheritedSeam?.Invoke(inherited);
                        }
                    }
                    else if (kvp.Value <= -50f && !_state.activeLegacyTraits.Any(t => t.id == "trait_ancient_grudge"))
                    {
                        if (_catalog.TryGetValue("trait_ancient_grudge", out var grudgeTrait))
                        {
                            var inherited = CloneTrait(grudgeTrait);
                            inherited.inherited = true;
                            inherited.generation = _state.completedCampaigns.Count;
                            _state.activeLegacyTraits.Add(inherited);
                            OnLegacyTraitInheritedSeam?.Invoke(inherited);
                        }
                    }
                }
            }

            OnCampaignArchivedSeam?.Invoke(legacy);
        }

        /// <summary>
        /// Prepares the starting context and bonuses for a New Game+ run based on all active legacy traits.
        /// </summary>
        public StartingCampaignContext PrepareNewGameContext()
        {
            var ctx = new StartingCampaignContext();
            ctx.inheritedTraits = _state.activeLegacyTraits.Select(CloneTrait).ToList();

            foreach (var trait in _state.activeLegacyTraits)
            {
                switch (trait.effect_type)
                {
                    case "starting_supplies":
                        ctx.startingResourcesBonus += trait.magnitude;
                        break;
                    case "morale_bonus":
                        ctx.startingMoraleBonus += trait.magnitude;
                        break;
                    case "shelter_defense":
                        ctx.startingDefenseBonus += trait.magnitude;
                        break;
                    case "capacity_bonus":
                        ctx.startingCapacityBonus += (int)trait.magnitude;
                        break;
                    case "faction_standing":
                        ctx.factionModifiers[trait.source] = trait.magnitude;
                        break;
                }
            }

            // Historical faction memory carried forward into starting standings
            foreach (var campaign in _state.completedCampaigns)
            {
                if (campaign.factionStandings != null)
                {
                    foreach (var kvp in campaign.factionStandings)
                    {
                        if (kvp.Value >= 50f)
                        {
                            ctx.factionModifiers[kvp.Key] = 20f;
                        }
                        else if (kvp.Value <= -50f)
                        {
                            ctx.factionModifiers[kvp.Key] = -20f;
                        }
                    }
                }
            }

            OnNewGameLegacyAppliedSeam?.Invoke(ctx);
            return ctx;
        }

        public CampaignLegacyState CaptureState()
        {
            return new CampaignLegacyState
            {
                schema_version = _state.schema_version,
                completedCampaigns = _state.completedCampaigns.Select(CloneCampaign).ToList(),
                activeLegacyTraits = _state.activeLegacyTraits.Select(CloneTrait).ToList(),
                inheritedImprovements = new List<string>(_state.inheritedImprovements),
                campaignHistoryLog = new List<string>(_state.campaignHistoryLog)
            };
        }

        public void RestoreState(CampaignLegacyState? state)
        {
            if (state == null)
            {
                _state = new CampaignLegacyState();
                return;
            }
            _state = new CampaignLegacyState
            {
                schema_version = state.schema_version,
                completedCampaigns = state.completedCampaigns != null ? state.completedCampaigns.Select(CloneCampaign).ToList() : new List<CampaignLegacy>(),
                activeLegacyTraits = state.activeLegacyTraits != null ? state.activeLegacyTraits.Select(CloneTrait).ToList() : new List<LegacyTrait>(),
                inheritedImprovements = state.inheritedImprovements != null ? new List<string>(state.inheritedImprovements) : new List<string>(),
                campaignHistoryLog = state.campaignHistoryLog != null ? new List<string>(state.campaignHistoryLog) : new List<string>()
            };
        }

        private static CampaignLegacy CloneCampaign(CampaignLegacy c)
        {
            return new CampaignLegacy
            {
                campaignId = c.campaignId,
                endingId = c.endingId,
                daysSurvived = c.daysSurvived,
                survivorCount = c.survivorCount,
                deathsRecorded = c.deathsRecorded,
                completionDay = c.completionDay,
                shelterImprovements = c.shelterImprovements != null ? new List<string>(c.shelterImprovements) : new List<string>(),
                legacyTraits = c.legacyTraits != null ? new List<string>(c.legacyTraits) : new List<string>(),
                campaignFlags = c.campaignFlags != null ? new List<string>(c.campaignFlags) : new List<string>(),
                factionStandings = c.factionStandings != null ? new Dictionary<string, float>(c.factionStandings) : new Dictionary<string, float>()
            };
        }

        private static LegacyTrait CloneTrait(LegacyTrait t)
        {
            return new LegacyTrait
            {
                id = t.id,
                name = t.name,
                description = t.description,
                source = t.source,
                effect_type = t.effect_type,
                magnitude = t.magnitude,
                evolution_target_id = t.evolution_target_id,
                generation = t.generation,
                inherited = t.inherited
            };
        }
    }
}
