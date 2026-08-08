using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Shelter-engineering milestone perks (Prompts #195–#200).
    /// Earned through jury-rigs, shoring, air-filter storm work, scrapping,
    /// excavation, and keeping the bunker warm — not XP grind.
    /// Plain C#, save/load safe.
    /// </summary>
    public class ShelterPerkSystem
    {
        // ── Perk ids ─────────────────────────────────────────────────────
        public const string JuryRiggerId = "perk_jury_rigger";
        public const string StructuralEngineerId = "perk_structural_engineer";
        public const string HvacTechId = "perk_hvac_tech";
        public const string ScrapperId = "perk_scrapper";
        public const string SandhogId = "perk_sandhog";
        public const string ThermodynamicsId = "perk_thermodynamics";

        // ── Thresholds ───────────────────────────────────────────────────
        public const int JuryRigsForJuryRigger = 5;
        public const int StrutsForStructuralEngineer = 10;
        public const int StormFilterOpsForHvac = 3;
        public const int DisassemblesForScrapper = 50;
        public const int RoomsClearedForSandhog = 5;
        public const int WarmDaysForThermodynamics = 20;
        public const float WarmDayTemperatureC = 15f;

        // ── Effect constants ─────────────────────────────────────────────
        public const float StructuralCeilingLoadMultiplier = 2f;
        public const float StructuralWoodCostMultiplier = 0.5f;
        public const float HvacVentilationSpeedBonus = 0.30f; // +30% clear rate
        public const float ScrapperRareComponentChance = 0.20f;
        public const float SandhogFatigueMultiplier = 0.5f;
        public const float ThermodynamicsFuelDurationBonus = 0.20f; // +20% burn time
        public const float ThermodynamicsFuelBurnMultiplier = 0.80f; // consume at 80%
        public const float BaseExcavationCaveInChance = 0.05f;

        public const string BatteryId = "battery";
        public const string SpringId = "spring";
        public const string WoodId = "wood";
        public const string ElectronicScrapId = "electronic_scrap";
        public const string MechanicalPartsId = "mechanical_parts";

        private SkillProgressionSystem _progression;
        private readonly Dictionary<string, ShelterCounters> _bySurvivor =
            new Dictionary<string, ShelterCounters>();

        /// <summary>Global consecutive warm-day streak for Thermodynamics (bunker-wide).</summary>
        public int ConsecutiveWarmDays { get; private set; }

        public event Action<Survivor, string> OnShelterPerkEarned;
        public event Action<Survivor, string, int> OnMilestoneProgress;

        public void Bind(SkillProgressionSystem progression)
        {
            _progression = progression;
            _progression?.RegisterShelterPerks();
        }

        public void RegisterCatalog() => _progression?.RegisterShelterPerks();

        // ── Queries ──────────────────────────────────────────────────────

        public bool Has(string survivorId, string perkId)
        {
            if (_progression == null || string.IsNullOrEmpty(survivorId)) return false;
            return _progression.HasActivePerk(survivorId, perkId);
        }

        public bool Has(Survivor sv, string perkId) =>
            sv != null && Has(sv.Id, perkId);

        public ShelterCounters GetCounters(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return new ShelterCounters();
            return GetOrCreate(survivorId).Clone();
        }

        /// <summary>True if any living survivor in the list holds the perk.</summary>
        public bool AnyLivingHas(IReadOnlyList<Survivor> survivors, string perkId)
        {
            if (survivors == null) return false;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv != null && sv.IsAlive && Has(sv, perkId))
                    return true;
            }
            return false;
        }

        // ── #195 Jury-Rigger ─────────────────────────────────────────────

        public void RecordJuryRigOrOverclock(Survivor sv, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return;
            var c = GetOrCreate(sv.Id);
            c.JuryRigActions++;
            OnMilestoneProgress?.Invoke(sv, "jury_rig_actions", c.JuryRigActions);
            if (c.JuryRigActions >= JuryRigsForJuryRigger)
                TryGrant(sv, JuryRiggerId, currentDay);
        }

        public bool CanSubstituteScrap(Survivor sv) => Has(sv, JuryRiggerId);

        /// <summary>
        /// MechanicalParts ↔ ElectronicScrap substitution for repair costs.
        /// Returns true when the required material id can be satisfied by its twin.
        /// </summary>
        public static bool IsScrapSubstitutePair(string requiredId, string availableId)
        {
            if (string.IsNullOrEmpty(requiredId) || string.IsNullOrEmpty(availableId)) return false;
            bool reqE = string.Equals(requiredId, ElectronicScrapId, StringComparison.OrdinalIgnoreCase)
                        || string.Equals(requiredId, "electronic_scrap", StringComparison.OrdinalIgnoreCase);
            bool reqM = string.Equals(requiredId, MechanicalPartsId, StringComparison.OrdinalIgnoreCase)
                        || string.Equals(requiredId, "mechanical_parts", StringComparison.OrdinalIgnoreCase);
            bool avE = string.Equals(availableId, ElectronicScrapId, StringComparison.OrdinalIgnoreCase)
                       || string.Equals(availableId, "electronic_scrap", StringComparison.OrdinalIgnoreCase);
            bool avM = string.Equals(availableId, MechanicalPartsId, StringComparison.OrdinalIgnoreCase)
                       || string.Equals(availableId, "mechanical_parts", StringComparison.OrdinalIgnoreCase);
            return (reqE && avM) || (reqM && avE);
        }

        public static string GetScrapSubstituteId(string materialId)
        {
            if (string.Equals(materialId, ElectronicScrapId, StringComparison.OrdinalIgnoreCase)
                || string.Equals(materialId, "electronic_scrap", StringComparison.OrdinalIgnoreCase))
                return MechanicalPartsId;
            if (string.Equals(materialId, MechanicalPartsId, StringComparison.OrdinalIgnoreCase)
                || string.Equals(materialId, "mechanical_parts", StringComparison.OrdinalIgnoreCase))
                return ElectronicScrapId;
            return null;
        }

        // ── #196 Structural Engineer ─────────────────────────────────────

        public void RecordShoringStrutBuilt(Survivor sv, int count = 1, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive || count <= 0) return;
            var c = GetOrCreate(sv.Id);
            c.ShoringStrutsBuilt += count;
            OnMilestoneProgress?.Invoke(sv, "shoring_struts", c.ShoringStrutsBuilt);
            if (c.ShoringStrutsBuilt >= StrutsForStructuralEngineer)
                TryGrant(sv, StructuralEngineerId, currentDay);
        }

        public bool IsStructuralEngineer(Survivor sv) => Has(sv, StructuralEngineerId);

        public float GetCeilingLoadMultiplier(Survivor reinforcer) =>
            IsStructuralEngineer(reinforcer) ? StructuralCeilingLoadMultiplier : 1f;

        public float GetShoringWoodCostMultiplier(Survivor builder) =>
            IsStructuralEngineer(builder) ? StructuralWoodCostMultiplier : 1f;

        // ── #197 HVAC Technician ─────────────────────────────────────────

        public void RecordStormAirFilterOp(Survivor sv, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return;
            var c = GetOrCreate(sv.Id);
            c.StormFilterOps++;
            OnMilestoneProgress?.Invoke(sv, "storm_filter_ops", c.StormFilterOps);
            if (c.StormFilterOps >= StormFilterOpsForHvac)
                TryGrant(sv, HvacTechId, currentDay);
        }

        public bool HasHvacTechInBunker(IReadOnlyList<Survivor> survivors) =>
            AnyLivingHas(survivors, HvacTechId);

        /// <summary>Clear-rate multiplier for CO2/mold when HVAC tech is present (1.3).</summary>
        public float GetVentilationClearMultiplier(IReadOnlyList<Survivor> survivors) =>
            HasHvacTechInBunker(survivors) ? 1f + HvacVentilationSpeedBonus : 1f;

        // ── #198 Scrapper ────────────────────────────────────────────────

        public void RecordDisassemble(Survivor sv, int count = 1, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive || count <= 0) return;
            var c = GetOrCreate(sv.Id);
            c.Disassembles += count;
            OnMilestoneProgress?.Invoke(sv, "disassembles", c.Disassembles);
            if (c.Disassembles >= DisassemblesForScrapper)
                TryGrant(sv, ScrapperId, currentDay);
        }

        public bool CanRecoverRareComponents(Survivor sv) => Has(sv, ScrapperId);

        /// <summary>
        /// High-tier disassemble: 20% chance to recover battery or spring.
        /// Returns item id or null.
        /// </summary>
        public string RollRareComponent(Survivor sv, bool isHighTier, System.Random rng = null)
        {
            if (!isHighTier || !CanRecoverRareComponents(sv)) return null;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("shelterperksystem");
            if (rng.NextDouble() >= ScrapperRareComponentChance) return null;
            return rng.NextDouble() < 0.5 ? BatteryId : SpringId;
        }

        private static readonly string[] HighTierKeywords = new[] { "radio", "gun", "rifle", "pistol", "geiger", "dosimeter" };

        public static bool IsHighTierDisassembleTarget(string itemId, int itemTypeOrdinal = -1)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            for (int i = 0; i < HighTierKeywords.Length; i++)
            {
                if (itemId.IndexOf(HighTierKeywords[i], StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
            return false;
        }

        // ── #199 Sandhog ─────────────────────────────────────────────────

        public void RecordRoomCleared(Survivor sv, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return;
            var c = GetOrCreate(sv.Id);
            c.RoomsCleared++;
            OnMilestoneProgress?.Invoke(sv, "rooms_cleared", c.RoomsCleared);
            if (c.RoomsCleared >= RoomsClearedForSandhog)
                TryGrant(sv, SandhogId, currentDay);
        }

        public bool IsSandhog(Survivor sv) => Has(sv, SandhogId);

        public float GetExcavationFatigueMultiplier(Survivor sv) =>
            IsSandhog(sv) ? SandhogFatigueMultiplier : 1f;

        public bool SuppressesCaveInWhileDigging(Survivor sv) => IsSandhog(sv);

        /// <summary>Roll cave-in while digging; Sandhog always returns false.</summary>
        public bool RollDigCaveIn(Survivor sv, System.Random rng = null, float baseChance = BaseExcavationCaveInChance)
        {
            if (SuppressesCaveInWhileDigging(sv)) return false;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("shelterperksystem");
            return rng.NextDouble() < baseChance;
        }

        // ── #200 Thermodynamics ──────────────────────────────────────────

        /// <summary>
        /// Call once per campaign day with bunker indoor temperature.
        /// After 20 consecutive days ≥ 15°C, grants Thermodynamics to the
        /// primary heater operator (or any living survivor if none specified).
        /// </summary>
        public void RecordWarmDay(float indoorTempC, IReadOnlyList<Survivor> survivors, int currentDay = 0)
        {
            if (indoorTempC >= WarmDayTemperatureC)
            {
                ConsecutiveWarmDays++;
                OnMilestoneProgress?.Invoke(null, "warm_days", ConsecutiveWarmDays);
                if (ConsecutiveWarmDays >= WarmDaysForThermodynamics && survivors != null)
                {
                    for (int i = 0; i < survivors.Count; i++)
                    {
                        var sv = survivors[i];
                        if (sv != null && sv.IsAlive)
                            TryGrant(sv, ThermodynamicsId, currentDay);
                    }
                }
            }
            else
            {
                ConsecutiveWarmDays = 0;
            }
        }

        public bool HasThermodynamics(Survivor sv) => Has(sv, ThermodynamicsId);

        /// <summary>
        /// Fuel burn rate multiplier when this survivor loaded the fuel (0.8 = burns 20% longer).
        /// </summary>
        public float GetFuelBurnMultiplier(Survivor loader) =>
            HasThermodynamics(loader) ? ThermodynamicsFuelBurnMultiplier : 1f;

        /// <summary>Effective fuel units added (loader with perk banks coals → more duration).</summary>
        public float GetEffectiveFuelLoad(Survivor loader, float baseFuel)
        {
            if (!HasThermodynamics(loader) || baseFuel <= 0f) return baseFuel;
            // 20% longer burn ≈ 25% more effective fuel at same burn rate,
            // or equivalently burn at 0.8 — both encoded via burn mult on module.
            return baseFuel;
        }

        // ── Grant / storage ──────────────────────────────────────────────

        private bool TryGrant(Survivor sv, string perkId, int currentDay)
        {
            if (_progression == null || sv == null) return false;
            if (_progression.HasActivePerk(sv.Id, perkId)
                || _progression.HasDormantPerk(sv.Id, perkId))
                return false;

            bool granted = _progression.TryGrantPerk(sv, perkId, currentDay);
            if (granted)
                OnShelterPerkEarned?.Invoke(sv, perkId);
            return granted;
        }

        private ShelterCounters GetOrCreate(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var c))
            {
                c = new ShelterCounters();
                _bySurvivor[survivorId] = c;
            }
            return c;
        }

        public ShelterPerkSave CaptureState()
        {
            var save = new ShelterPerkSave
            {
                ConsecutiveWarmDays = ConsecutiveWarmDays,
                Entries = new List<ShelterCounterSave>()
            };
            foreach (var kv in _bySurvivor)
            {
                var c = kv.Value;
                save.Entries.Add(new ShelterCounterSave
                {
                    SurvivorId = kv.Key,
                    JuryRigActions = c.JuryRigActions,
                    ShoringStrutsBuilt = c.ShoringStrutsBuilt,
                    StormFilterOps = c.StormFilterOps,
                    Disassembles = c.Disassembles,
                    RoomsCleared = c.RoomsCleared
                });
            }
            return save;
        }

        public void RestoreState(ShelterPerkSave save)
        {
            _bySurvivor.Clear();
            ConsecutiveWarmDays = 0;
            if (save == null) return;
            ConsecutiveWarmDays = save.ConsecutiveWarmDays;
            if (save.Entries == null) return;
            for (int i = 0; i < save.Entries.Count; i++)
            {
                var e = save.Entries[i];
                if (e == null || string.IsNullOrEmpty(e.SurvivorId)) continue;
                _bySurvivor[e.SurvivorId] = new ShelterCounters
                {
                    JuryRigActions = e.JuryRigActions,
                    ShoringStrutsBuilt = e.ShoringStrutsBuilt,
                    StormFilterOps = e.StormFilterOps,
                    Disassembles = e.Disassembles,
                    RoomsCleared = e.RoomsCleared
                };
            }
        }

        public sealed class ShelterCounters
        {
            public int JuryRigActions;
            public int ShoringStrutsBuilt;
            public int StormFilterOps;
            public int Disassembles;
            public int RoomsCleared;

            public ShelterCounters Clone() => new ShelterCounters
            {
                JuryRigActions = JuryRigActions,
                ShoringStrutsBuilt = ShoringStrutsBuilt,
                StormFilterOps = StormFilterOps,
                Disassembles = Disassembles,
                RoomsCleared = RoomsCleared
            };
        }
    }

    [Serializable]
    public class ShelterPerkSave
    {
        public int ConsecutiveWarmDays;
        public List<ShelterCounterSave> Entries = new List<ShelterCounterSave>();
    }

    [Serializable]
    public class ShelterCounterSave
    {
        public string SurvivorId;
        public int JuryRigActions;
        public int ShoringStrutsBuilt;
        public int StormFilterOps;
        public int Disassembles;
        public int RoomsCleared;
    }
}
