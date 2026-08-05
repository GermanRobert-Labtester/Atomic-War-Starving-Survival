using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Combat milestone perks (Prompts #182–#188). Earned through jams, stealth
    /// kills, ammo expenditure, confined fights, traps, flees, and human kills —
    /// not XP grind. Plain C#, save/load safe, EditMode-testable.
    /// </summary>
    public class CombatPerkSystem
    {
        // ── Perk ids (snake_case) ────────────────────────────────────────
        public const string TapRackBangId = "perk_tap_rack_bang";
        public const string ColdBoreId = "perk_cold_bore";
        public const string SuppressingFireId = "perk_suppressing_fire";
        public const string CloseQuartersId = "perk_close_quarters";
        public const string TrapSetterId = "perk_trap_setter";
        public const string LootersReflexId = "perk_looters_reflex";
        public const string DesensitizedId = "perk_desensitized";

        // ── Thresholds ───────────────────────────────────────────────────
        public const int JamsForTapRackBang = 3;
        public const int StealthKillsForColdBore = 1;
        public const int AmmoForSuppressingFire = 50;
        public const int ConfinedEncountersForCloseQuarters = 3;
        public const int TrapsForTrapSetter = 10;
        public const int FleesForLootersReflex = 3;
        public const int HumanKillsForDesensitized = 5;

        // ── Effect constants ─────────────────────────────────────────────
        public const int DefaultJamClearTicks = 5;
        public const int TapRackBangJamClearTicks = 1;
        public const float ColdBoreFirstShotCritBonus = 0.50f;
        public const int SuppressingFireAmmoCost = 5;
        public const float SuppressingFireHaltHours = 2f;
        public const float CloseQuartersDamageMultiplier = 2.0f;
        public const float TrapSetterDamageMultiplier = 2.0f;
        public const float DefaultTrapMisfireChance = 0.15f;
        public const float DefaultDisarmSuccess = 0.45f;
        public const float DesensitizedEmpathAffinityDrop = -40f;
        public const float HumanKillMoralePenalty = 8f;

        // ── Map / node tags ──────────────────────────────────────────────
        public const string TagUrban = "urban";
        public const string TagSubway = "subway";
        public const string TagConfinedSpace = "confined_space";

        private SkillProgressionSystem _progression;
        private readonly Dictionary<string, CombatCounters> _bySurvivor =
            new Dictionary<string, CombatCounters>();

        /// <summary>Encounter-scoped first-shot tracking (Cold Bore). Keyed by encounter key.</summary>
        private readonly HashSet<string> _firstShotFired = new HashSet<string>();

        public event Action<Survivor, string> OnCombatPerkEarned; // sv, perkId
        public event Action<Survivor, string, int> OnMilestoneProgress; // sv, counterKey, newValue

        public void Bind(SkillProgressionSystem progression)
        {
            _progression = progression;
            _progression?.RegisterCombatPerks();
        }

        public void RegisterCatalog()
        {
            _progression?.RegisterCombatPerks();
        }

        // ── Queries ──────────────────────────────────────────────────────

        public bool Has(string survivorId, string perkId)
        {
            if (_progression == null || string.IsNullOrEmpty(survivorId)) return false;
            return _progression.HasActivePerk(survivorId, perkId);
        }

        public bool Has(Survivor sv, string perkId) =>
            sv != null && Has(sv.Id, perkId);

        public CombatCounters GetCounters(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return new CombatCounters();
            return GetOrCreate(survivorId).Clone();
        }

        // ── #182 Tap-Rack-Bang ───────────────────────────────────────────

        /// <summary>Record surviving a weapon jam (cleared without dying / dropping out).</summary>
        public void RecordWeaponJamSurvived(Survivor sv, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return;
            var c = GetOrCreate(sv.Id);
            c.JamsSurvived++;
            OnMilestoneProgress?.Invoke(sv, "jams_survived", c.JamsSurvived);
            if (c.JamsSurvived >= JamsForTapRackBang)
                TryGrant(sv, TapRackBangId, currentDay);
        }

        /// <summary>Ticks required to clear a jam for this survivor (1 with perk, else 5).</summary>
        public int GetJamClearTicks(Survivor sv)
        {
            return Has(sv, TapRackBangId) ? TapRackBangJamClearTicks : DefaultJamClearTicks;
        }

        // ── #183 Cold Bore ───────────────────────────────────────────────

        public void RecordStealthKill(Survivor sv, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return;
            var c = GetOrCreate(sv.Id);
            c.StealthKills++;
            OnMilestoneProgress?.Invoke(sv, "stealth_kills", c.StealthKills);
            if (c.StealthKills >= StealthKillsForColdBore)
                TryGrant(sv, ColdBoreId, currentDay);
        }

        /// <summary>
        /// First shot in an encounter: +50% crit chance if Cold Bore is active.
        /// Subsequent shots for the same encounter key get no bonus.
        /// Returns true if this shot is a critical (instant kill).
        /// </summary>
        public bool RollFirstShotCrit(
            Survivor sv,
            string encounterKey,
            System.Random rng,
            float baseCritChance = 0f)
        {
            if (sv == null || string.IsNullOrEmpty(encounterKey)) return false;
            rng ??= new System.Random();

            string key = sv.Id + "|" + encounterKey;
            bool isFirst = !_firstShotFired.Contains(key);
            if (isFirst)
                _firstShotFired.Add(key);

            float chance = baseCritChance;
            if (isFirst && Has(sv, ColdBoreId))
                chance += ColdBoreFirstShotCritBonus;

            return rng.NextDouble() < chance;
        }

        public void ClearEncounterShots(string encounterKey)
        {
            if (string.IsNullOrEmpty(encounterKey)) return;
            var remove = new List<string>();
            foreach (var k in _firstShotFired)
            {
                if (k.EndsWith("|" + encounterKey, StringComparison.Ordinal))
                    remove.Add(k);
            }
            for (int i = 0; i < remove.Count; i++)
                _firstShotFired.Remove(remove[i]);
        }

        // ── #184 Suppressing Fire ────────────────────────────────────────

        public void RecordAmmoExpended(Survivor sv, int rounds, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive || rounds <= 0) return;
            var c = GetOrCreate(sv.Id);
            c.AmmoExpended += rounds;
            OnMilestoneProgress?.Invoke(sv, "ammo_expended", c.AmmoExpended);
            if (c.AmmoExpended >= AmmoForSuppressingFire)
                TryGrant(sv, SuppressingFireId, currentDay);
        }

        public bool CanUseSuppressingFire(Survivor sv) => Has(sv, SuppressingFireId);

        // ── #185 Close Quarters ──────────────────────────────────────────

        public void RecordConfinedEncounterSurvived(Survivor sv, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return;
            var c = GetOrCreate(sv.Id);
            c.ConfinedEncountersSurvived++;
            OnMilestoneProgress?.Invoke(sv, "confined_encounters", c.ConfinedEncountersSurvived);
            if (c.ConfinedEncountersSurvived >= ConfinedEncountersForCloseQuarters)
                TryGrant(sv, CloseQuartersId, currentDay);
        }

        /// <summary>Damage multiplier for shotgun/melee when confined or bunker-breach.</summary>
        public float GetCloseQuartersDamageMultiplier(Survivor sv, bool confinedOrBreach)
        {
            if (!confinedOrBreach || !Has(sv, CloseQuartersId)) return 1f;
            return CloseQuartersDamageMultiplier;
        }

        public static bool IsConfinedNodeTags(IList<string> tags)
        {
            if (tags == null) return false;
            for (int i = 0; i < tags.Count; i++)
            {
                string t = tags[i];
                if (string.IsNullOrEmpty(t)) continue;
                if (string.Equals(t, TagUrban, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(t, TagSubway, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(t, TagConfinedSpace, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public static bool IsUrbanOrSubwayTags(IList<string> tags)
        {
            if (tags == null) return false;
            for (int i = 0; i < tags.Count; i++)
            {
                string t = tags[i];
                if (string.IsNullOrEmpty(t)) continue;
                if (string.Equals(t, TagUrban, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(t, TagSubway, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        // ── #186 Trap Setter ─────────────────────────────────────────────

        public void RecordTrapDeployed(Survivor sv, int count = 1, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive || count <= 0) return;
            var c = GetOrCreate(sv.Id);
            c.TrapsDeployed += count;
            OnMilestoneProgress?.Invoke(sv, "traps_deployed", c.TrapsDeployed);
            if (c.TrapsDeployed >= TrapsForTrapSetter)
                TryGrant(sv, TrapSetterId, currentDay);
        }

        public float GetTrapMisfireChance(Survivor sv)
        {
            return Has(sv, TrapSetterId) ? 0f : DefaultTrapMisfireChance;
        }

        public float GetTrapDamageMultiplier(Survivor sv)
        {
            return Has(sv, TrapSetterId) ? TrapSetterDamageMultiplier : 1f;
        }

        public float GetDisarmSuccessRate(Survivor sv)
        {
            return Has(sv, TrapSetterId) ? 1f : DefaultDisarmSuccess;
        }

        public bool TryDisarmWastelandTrap(Survivor sv, System.Random rng = null)
        {
            if (sv == null || !sv.IsAlive) return false;
            float rate = GetDisarmSuccessRate(sv);
            if (rate >= 1f) return true;
            rng ??= new System.Random();
            return rng.NextDouble() < rate;
        }

        // ── #187 Looter's Reflex ─────────────────────────────────────────

        public void RecordFlee(Survivor sv, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return;
            var c = GetOrCreate(sv.Id);
            c.Flees++;
            OnMilestoneProgress?.Invoke(sv, "flees", c.Flees);
            if (c.Flees >= FleesForLootersReflex)
                TryGrant(sv, LootersReflexId, currentDay);
        }

        /// <summary>
        /// True when this survivor keeps the single best backpack item on flee.
        /// </summary>
        public bool ShouldRetainBestLootOnFlee(Survivor sv) => Has(sv, LootersReflexId);

        /// <summary>
        /// How many items to drop on a normal flee (no Looter's Reflex).
        /// Always at least 1 when lootCount &gt; 0.
        /// </summary>
        public static int GetDefaultFleeDropCount(int lootCount, float defaultDropFraction = 0.5f)
        {
            if (lootCount <= 0) return 0;
            return Mathf.Clamp(
                Mathf.CeilToInt(lootCount * Mathf.Clamp01(defaultDropFraction)),
                1, lootCount);
        }

        /// <summary>
        /// Index of the single most valuable (tradeValue), then heaviest, item.
        /// Inventory-free so Survivors assembly has no Inventory reference cycle.
        /// </summary>
        public static int FindMostValuableOrHeavyIndex(
            int count,
            Func<int, float> tradeValueAt,
            Func<int, float> weightAt)
        {
            if (count <= 0) return -1;
            int best = 0;
            float bestScore = float.MinValue;
            for (int i = 0; i < count; i++)
            {
                float tv = tradeValueAt != null ? tradeValueAt(i) : 0f;
                float w = weightAt != null ? weightAt(i) : 0f;
                float score = tv * 1000f + w;
                if (score > bestScore)
                {
                    bestScore = score;
                    best = i;
                }
            }
            return best;
        }

        /// <summary>
        /// Compute which loot indices to drop when fleeing.
        /// With Looter's Reflex: drop everything except the single best item.
        /// Without: drop a fraction from the end of the list.
        /// Returns sorted descending indices (safe for RemoveAt from end).
        /// </summary>
        public List<int> ComputeFleeDropIndices(
            Survivor sv,
            int lootCount,
            Func<int, float> tradeValueAt,
            Func<int, float> weightAt,
            float defaultDropFraction = 0.5f)
        {
            var drop = new List<int>();
            if (lootCount <= 0) return drop;

            if (ShouldRetainBestLootOnFlee(sv))
            {
                int keep = FindMostValuableOrHeavyIndex(lootCount, tradeValueAt, weightAt);
                for (int i = lootCount - 1; i >= 0; i--)
                {
                    if (i != keep) drop.Add(i);
                }
                return drop;
            }

            int countToDrop = GetDefaultFleeDropCount(lootCount, defaultDropFraction);
            for (int i = 0; i < countToDrop; i++)
                drop.Add(lootCount - 1 - i);
            return drop;
        }

        // ── #188 Desensitized ────────────────────────────────────────────

        /// <summary>
        /// Record a human NPC kill. At 5, grants Desensitized and permanently
        /// drops affinity with every Empath in the bunker.
        /// </summary>
        public void RecordHumanKill(
            Survivor sv,
            int currentDay,
            IReadOnlyList<Survivor> allSurvivors,
            InterpersonalAffinity affinity)
        {
            if (sv == null || !sv.IsAlive) return;
            var c = GetOrCreate(sv.Id);
            c.HumanKills++;
            OnMilestoneProgress?.Invoke(sv, "human_kills", c.HumanKills);

            if (c.HumanKills < HumanKillsForDesensitized) return;
            if (Has(sv, DesensitizedId)) return;

            if (TryGrant(sv, DesensitizedId, currentDay) && allSurvivors != null && affinity != null)
            {
                for (int i = 0; i < allSurvivors.Count; i++)
                {
                    var other = allSurvivors[i];
                    if (other == null || other.Id == sv.Id || !other.IsAlive) continue;
                    if (other.RiskBias != RiskBiasTrait.Empath) continue;
                    affinity.Adjust(sv.Id, other.Id, DesensitizedEmpathAffinityDrop);
                }
            }
        }

        /// <summary>No morale penalty from taking human life.</summary>
        public bool IsImmuneToKillMorale(Survivor sv) => Has(sv, DesensitizedId);

        /// <summary>No morale drain from managing / living with corpses.</summary>
        public bool IsImmuneToCorpseMorale(Survivor sv) => Has(sv, DesensitizedId);

        /// <summary>
        /// Apply kill-related morale loss unless Desensitized.
        /// <paramref name="loss"/> is magnitude (positive = lose that much morale).
        /// Returns the actual delta applied (0 if immune, negative if hit).
        /// </summary>
        public float ApplyHumanKillMorale(Survivor sv, float loss = HumanKillMoralePenalty)
        {
            if (sv?.Needs == null) return 0f;
            if (IsImmuneToKillMorale(sv)) return 0f;
            float before = sv.Needs.Morale;
            float mag = Mathf.Abs(loss);
            sv.Needs.Morale = Mathf.Clamp(before - mag, 0f, 100f);
            return sv.Needs.Morale - before;
        }

        // ── Grant helper ─────────────────────────────────────────────────

        private bool TryGrant(Survivor sv, string perkId, int currentDay)
        {
            if (_progression == null || sv == null) return false;
            if (_progression.HasActivePerk(sv.Id, perkId)
                || _progression.HasDormantPerk(sv.Id, perkId))
                return false;

            bool granted = _progression.TryGrantPerk(sv, perkId, currentDay);
            if (granted)
                OnCombatPerkEarned?.Invoke(sv, perkId);
            return granted;
        }

        private CombatCounters GetOrCreate(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var c))
            {
                c = new CombatCounters();
                _bySurvivor[survivorId] = c;
            }
            return c;
        }

        // ── Save / Load ──────────────────────────────────────────────────

        public CombatPerkSave CaptureState()
        {
            var save = new CombatPerkSave { Entries = new List<CombatCounterSave>() };
            foreach (var kv in _bySurvivor)
            {
                var c = kv.Value;
                save.Entries.Add(new CombatCounterSave
                {
                    SurvivorId = kv.Key,
                    JamsSurvived = c.JamsSurvived,
                    StealthKills = c.StealthKills,
                    AmmoExpended = c.AmmoExpended,
                    ConfinedEncountersSurvived = c.ConfinedEncountersSurvived,
                    TrapsDeployed = c.TrapsDeployed,
                    Flees = c.Flees,
                    HumanKills = c.HumanKills
                });
            }
            return save;
        }

        public void RestoreState(CombatPerkSave save)
        {
            _bySurvivor.Clear();
            _firstShotFired.Clear();
            if (save?.Entries == null) return;
            for (int i = 0; i < save.Entries.Count; i++)
            {
                var e = save.Entries[i];
                if (e == null || string.IsNullOrEmpty(e.SurvivorId)) continue;
                _bySurvivor[e.SurvivorId] = new CombatCounters
                {
                    JamsSurvived = e.JamsSurvived,
                    StealthKills = e.StealthKills,
                    AmmoExpended = e.AmmoExpended,
                    ConfinedEncountersSurvived = e.ConfinedEncountersSurvived,
                    TrapsDeployed = e.TrapsDeployed,
                    Flees = e.Flees,
                    HumanKills = e.HumanKills
                };
            }
        }

        public sealed class CombatCounters
        {
            public int JamsSurvived;
            public int StealthKills;
            public int AmmoExpended;
            public int ConfinedEncountersSurvived;
            public int TrapsDeployed;
            public int Flees;
            public int HumanKills;

            public CombatCounters Clone() => new CombatCounters
            {
                JamsSurvived = JamsSurvived,
                StealthKills = StealthKills,
                AmmoExpended = AmmoExpended,
                ConfinedEncountersSurvived = ConfinedEncountersSurvived,
                TrapsDeployed = TrapsDeployed,
                Flees = Flees,
                HumanKills = HumanKills
            };
        }
    }

    [Serializable]
    public class CombatPerkSave
    {
        public List<CombatCounterSave> Entries = new List<CombatCounterSave>();
    }

    [Serializable]
    public class CombatCounterSave
    {
        public string SurvivorId;
        public int JamsSurvived;
        public int StealthKills;
        public int AmmoExpended;
        public int ConfinedEncountersSurvived;
        public int TrapsDeployed;
        public int Flees;
        public int HumanKills;
    }
}
