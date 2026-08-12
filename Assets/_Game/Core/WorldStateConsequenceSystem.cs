using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion V — The World State Consequence Engine. The map of Tessarat is
    /// not static. It is a living, bleeding organism that reacts to your faction
    /// alignments. Completing major questlines causes ripple effects: roads close,
    /// patrol routes change, safe zones become irradiated.
    ///
    /// Tracks Hegemony Score (-100..+100) with four major factions plus the Warlords.
    /// Major questline completions trigger map mutations.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class WorldStateConsequenceSystem
    {
        // ── Faction ids ───────────────────────────────────────────────
        public const string Faction_Garrison = "faction_central_garrison";
        public const string Faction_Militia = "faction_upland_militia";
        public const string Faction_Cult = "faction_cult_of_the_glow";
        public const string Faction_Warlords = "faction_scavenger_warlords";
        public const string Faction_Rebuilders = "faction_rebuilders";
        public const string Faction_BlackOps = "faction_black_ops";

        // ── Hegemony thresholds ───────────────────────────────────────
        public const int HegemonyMax = 100;
        public const int HegemonyMin = -100;
        public const int HegemonyAllied = 50;
        public const int HegemonyHostile = -50;
        public const int HegemonyPariah = -100;

        // ── Questline ids ─────────────────────────────────────────────
        public const string Quest_IronLedger = "quest_iron_ledger";
        public const string Quest_BurntHarvest = "quest_burnt_harvest";
        public const string Quest_AshenCommunion = "quest_ashen_communion";

        // ── Map mutation ids ──────────────────────────────────────────
        public const string Mutation_Highway9Cleared = "mutation_highway9_cleared";
        public const string Mutation_AgricultureLocked = "mutation_agriculture_locked";
        public const string Mutation_TransitTax = "mutation_transit_tax";
        public const string Mutation_FuelDepotDestroyed = "mutation_fuel_depot_destroyed";
        public const string Mutation_WaterPoisoned = "mutation_water_poisoned";
        public const string Mutation_CultShrineAirIntake = "mutation_cult_shrine_air_intake";
        public const string Mutation_RoadblockCleared = "mutation_roadblock_cleared";
        public const string Mutation_MedicalSupplyGone = "mutation_medical_supply_gone";

        // ── Travel time modifiers ─────────────────────────────────────
        public const float Highway9TravelReduction = 0.40f; // 40% faster
        public const float TransitTaxAmmoPerExpedition = 10f;
        public const float TransitTaxFuelPerExpedition = 5f;
        public const float CultShrineRadIncrease = 5f;     // +5 mSv/h permanent

        // ── Events ────────────────────────────────────────────────────
        public event Action<string, int> OnHegemonyChanged;       // factionId, newValue
        public event Action<string> OnMapMutationApplied;         // mutationId
        public event Action<string> OnQuestlineCompleted;         // questId
        public event Action<string, string> OnFactionStatusChanged; // factionId, newStatus

        private readonly Dictionary<string, int> _hegemony = new Dictionary<string, int>();
        private readonly HashSet<string> _appliedMutations = new HashSet<string>();
        private readonly HashSet<string> _completedQuestlines = new HashSet<string>();
        private readonly List<MapMutation> _mutationLog = new List<MapMutation>();

        public IReadOnlyDictionary<string, int> Hegemony => _hegemony;
        public IReadOnlyCollection<string> AppliedMutations => _appliedMutations;
        public IReadOnlyCollection<string> CompletedQuestlines => _completedQuestlines;
        public IReadOnlyList<MapMutation> MutationLog => _mutationLog;

        public WorldStateConsequenceSystem()
        {
            // Initialize all factions at neutral
            _hegemony[Faction_Garrison] = 0;
            _hegemony[Faction_Militia] = 0;
            _hegemony[Faction_Cult] = 0;
            _hegemony[Faction_Warlords] = 0;
            _hegemony[Faction_Rebuilders] = 0;
        }

        // ── Hegemony queries ──────────────────────────────────────────

        public int GetHegemony(string factionId)
        {
            return _hegemony.TryGetValue(factionId, out var v) ? v : 0;
        }

        public string GetFactionStatus(string factionId)
        {
            int h = GetHegemony(factionId);
            if (h >= HegemonyAllied) return "allied";
            if (h <= HegemonyHostile) return "hostile";
            return "neutral";
        }

        public bool IsAllied(string factionId) => GetHegemony(factionId) >= HegemonyAllied;
        public bool IsHostile(string factionId) => GetHegemony(factionId) <= HegemonyHostile;
        public bool IsPariah() => GetHegemony(Faction_Garrison) <= HegemonyPariah
            && GetHegemony(Faction_Militia) <= HegemonyPariah
            && GetHegemony(Faction_Cult) <= HegemonyPariah;

        /// <summary>Check if a specific map mutation is active.</summary>
        public bool HasMutation(string mutationId) => _appliedMutations.Contains(mutationId);

        // ── Hegemony modification ─────────────────────────────────────

        public void ModifyHegemony(string factionId, int delta)
        {
            if (string.IsNullOrEmpty(factionId)) return;
            _hegemony.TryGetValue(factionId, out var current);
            int newVal = Mathf.Clamp(current + delta, HegemonyMin, HegemonyMax);
            _hegemony[factionId] = newVal;

            string oldStatus = GetFactionStatus(factionId);
            string newStatus = newVal >= HegemonyAllied ? "allied"
                : newVal <= HegemonyHostile ? "hostile" : "neutral";

            OnHegemonyChanged?.Invoke(factionId, newVal);
            if (oldStatus != newStatus)
                OnFactionStatusChanged?.Invoke(factionId, newStatus);
        }

        // ── Questline completion & ripple effects ─────────────────────

        /// <summary>
        /// Complete a major questline and trigger all associated map mutations.
        /// </summary>
        public bool CompleteQuestline(string questId)
        {
            if (string.IsNullOrEmpty(questId) || _completedQuestlines.Contains(questId))
                return false;

            _completedQuestlines.Add(questId);

            switch (questId)
            {
                case Quest_IronLedger:
                    ApplyGarrisonQuestEffects();
                    break;
                case Quest_BurntHarvest:
                    ApplyMilitiaQuestEffects();
                    break;
                case Quest_AshenCommunion:
                    ApplyCultQuestEffects();
                    break;
            }

            OnQuestlineCompleted?.Invoke(questId);
            return true;
        }

        private void ApplyGarrisonQuestEffects()
        {
            // Garrison hegemony rises
            ModifyHegemony(Faction_Garrison, 40);
            // Militia hegemony drops (you're a collaborator)
            ModifyHegemony(Faction_Militia, -50);

            // Roads open: Highway 9 cleared
            ApplyMutation(Mutation_Highway9Cleared,
                "The Garrison clears the ash from Highway 9. Travel time to northern locations reduced by 40%.");

            // Agriculture locked: Militia embargo
            ApplyMutation(Mutation_AgricultureLocked,
                "The Militia declares you a collaborator. All agricultural trade nodes lock.");

            // Transit tax: Garrison checkpoint at your hatch
            ApplyMutation(Mutation_TransitTax,
                "The Garrison establishes a Re-Education Checkpoint outside your hatch.");
        }

        private void ApplyMilitiaQuestEffects()
        {
            // Militia hegemony rises
            ModifyHegemony(Faction_Militia, 40);
            // Garrison hegemony drops (fuel depot destroyed)
            ModifyHegemony(Faction_Garrison, -50);

            // Fuel depot destroyed
            ApplyMutation(Mutation_FuelDepotDestroyed,
                "The Garrison's fuel depot is destroyed. They lose mechanized advantage.");

            // Water poisoned if you refuse militia demands
            ApplyMutation(Mutation_WaterPoisoned,
                "The Militia poisons the surface reservoirs to force your hand.");
        }

        private void ApplyCultQuestEffects()
        {
            // Cult hegemony rises
            ModifyHegemony(Faction_Cult, 40);
            // Rebuilders refuse to trade
            ModifyHegemony(Faction_Rebuilders, -60);

            // Roadblocks cleared (Cult terrifies warlords)
            ApplyMutation(Mutation_RoadblockCleared,
                "The Cult's patrols terrify the Scavenger Warlords. Roadblock encounters drop to 0%.");

            // Medical supplies vanish
            ApplyMutation(Mutation_MedicalSupplyGone,
                "The Rebuilders refuse to trade. Medical supplies vanish from the market.");

            // Cult shrine above air intake
            ApplyMutation(Mutation_CultShrineAirIntake,
                "The Cult sets up a Shrine of the Glow above your bunker's air intake.");
        }

        private void ApplyMutation(string mutationId, string description)
        {
            if (!_appliedMutations.Add(mutationId)) return;
            _mutationLog.Add(new MapMutation
            {
                MutationId = mutationId,
                Description = description,
                DayApplied = 0 // Host provides current day
            });
            OnMapMutationApplied?.Invoke(mutationId);
        }

        // ── Travel time modifiers ─────────────────────────────────────

        /// <summary>
        /// Get travel time multiplier for a given route based on active mutations.
        /// </summary>
        public float GetTravelTimeMultiplier(string routeId)
        {
            float mult = 1f;
            if (_appliedMutations.Contains(Mutation_Highway9Cleared)
                && routeId != null && routeId.Contains("northern"))
                mult *= (1f - Highway9TravelReduction);
            return mult;
        }

        /// <summary>
        /// Get transit tax cost for an expedition through garrison territory.
        /// </summary>
        public (float ammo, float fuel) GetTransitTax()
        {
            if (!_appliedMutations.Contains(Mutation_TransitTax)) return (0f, 0f);
            return (TransitTaxAmmoPerExpedition, TransitTaxFuelPerExpedition);
        }

        /// <summary>
        /// Get ambient radiation increase from cult shrine.
        /// </summary>
        public float GetCultShrineRadPenalty()
        {
            return _appliedMutations.Contains(Mutation_CultShrineAirIntake)
                ? CultShrineRadIncrease : 0f;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public WorldStateSave CaptureState()
        {
            var hegemonyEntries = new HegemonySave[_hegemony.Count];
            int i = 0;
            foreach (var kv in _hegemony)
                hegemonyEntries[i++] = new HegemonySave { FactionId = kv.Key, Value = kv.Value };

            var mutations = new string[_appliedMutations.Count];
            _appliedMutations.CopyTo(mutations);

            var quests = new string[_completedQuestlines.Count];
            _completedQuestlines.CopyTo(quests);

            return new WorldStateSave
            {
                Hegemony = hegemonyEntries,
                AppliedMutations = mutations,
                CompletedQuestlines = quests
            };
        }

        public void RestoreState(WorldStateSave save)
        {
            _hegemony.Clear();
            _appliedMutations.Clear();
            _completedQuestlines.Clear();
            _mutationLog.Clear();
            if (save == null) return;
            if (save.Hegemony != null)
                for (int i = 0; i < save.Hegemony.Length; i++)
                    if (save.Hegemony[i] != null)
                        _hegemony[save.Hegemony[i].FactionId] = save.Hegemony[i].Value;
            if (save.AppliedMutations != null)
                for (int i = 0; i < save.AppliedMutations.Length; i++)
                    if (!string.IsNullOrEmpty(save.AppliedMutations[i]))
                        _appliedMutations.Add(save.AppliedMutations[i]);
            if (save.CompletedQuestlines != null)
                for (int i = 0; i < save.CompletedQuestlines.Length; i++)
                    if (!string.IsNullOrEmpty(save.CompletedQuestlines[i]))
                        _completedQuestlines.Add(save.CompletedQuestlines[i]);
        }
    }

    public class MapMutation
    {
        public string MutationId;
        public string Description;
        public int DayApplied;
    }

    [Serializable]
    public class WorldStateSave
    {
        public HegemonySave[] Hegemony;
        public string[] AppliedMutations;
        public string[] CompletedQuestlines;
    }

    [Serializable]
    public class HegemonySave
    {
        public string FactionId;
        public int Value;
    }
}
