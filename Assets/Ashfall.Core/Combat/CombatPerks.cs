using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Combat
{
    /// <summary>Per-survivor perk counters + active-grant tracking (engine-agnostic).</summary>
    [Serializable]
    public class CombatPerkState
    {
        public List<CombatPerkEntry> Entries = new List<CombatPerkEntry>();
    }

    /// <summary>One survivor's combat-perk progress.</summary>
    [Serializable]
    public class CombatPerkEntry
    {
        public string SurvivorId = string.Empty;
        public int JamsSurvived;
        public int StealthKills;
        public int AmmoExpended;
        public int ConfinedEncountersSurvived;
        public int TrapsDeployed;
        public int Flees;
        public int HumanKills;
        public List<string> ActivePerks = new List<string>();
    }

    /// <summary>
    /// Port of the legacy <c>CombatPerkSystem</c> milestones/effects into engine-
    /// agnostic Core. Perks are earned through combat behaviours (jams, stealth
    /// kills, ammo spent, confined fights, traps, flees, human kills) not XP.
    /// Grants and thresholds mirror the legacy constants; host wires progression.
    /// </summary>
    public class CombatPerks
    {
        public const string TapRackBangId = "perk_tap_rack_bang";
        public const string ColdBoreId = "perk_cold_bore";
        public const string SuppressingFireId = "perk_suppressing_fire";
        public const string CloseQuartersId = "perk_close_quarters";
        public const string TrapSetterId = "perk_trap_setter";
        public const string LootersReflexId = "perk_looters_reflex";
        public const string DesensitizedId = "perk_desensitized";

        public const int JamsForTapRackBang = 3;
        public const int StealthKillsForColdBore = 1;
        public const int AmmoForSuppressingFire = 50;
        public const int ConfinedEncountersForCloseQuarters = 3;
        public const int TrapsForTrapSetter = 10;
        public const int FleesForLootersReflex = 3;
        public const int HumanKillsForDesensitized = 5;

        public const int DefaultJamClearTicks = 5;
        public const int TapRackBangJamClearTicks = 1;
        public const float ColdBoreFirstShotCritBonus = 0.50f;
        public const int SuppressingFireAmmoCost = 5;
        public const float CloseQuartersDamageMultiplier = 2.0f;
        public const float TrapSetterDamageMultiplier = 2.0f;
        public const float DefaultTrapMisfireChance = 0.15f;
        public const float DefaultDisarmSuccess = 0.45f;
        public const float DesensitizedEmpathAffinityDrop = -40f;
        public const float HumanKillMoralePenalty = 8f;

        public event Action<string, string> OnCombatPerkEarned; // survivorId, perkId

        private readonly Dictionary<string, CombatPerkEntry> _bySurvivor =
            new Dictionary<string, CombatPerkEntry>(StringComparer.Ordinal);

        private int _seed;
        public CombatPerks(int seed = 0) { _seed = seed; }

        public void RegisterSurvivor(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            GetOrCreate(survivorId);
        }

        public bool Has(string survivorId, string perkId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (!_bySurvivor.TryGetValue(survivorId, out var e)) return false;
            return e.ActivePerks != null && e.ActivePerks.Contains(perkId);
        }

        public void Grant(string survivorId, string perkId)
        {
            var e = GetOrCreate(survivorId);
            if (e.ActivePerks == null) e.ActivePerks = new List<string>();
            if (!e.ActivePerks.Contains(perkId))
            {
                e.ActivePerks.Add(perkId);
                OnCombatPerkEarned?.Invoke(survivorId, perkId);
            }
        }

        // ── Per-effect queries (mirror the legacy behaviour) ────────────

        public int GetJamClearTicks(string survivorId)
            => Has(survivorId, TapRackBangId) ? TapRackBangJamClearTicks : DefaultJamClearTicks;

        public float GetFirstShotCritBonus(string survivorId)
            => Has(survivorId, ColdBoreId) ? ColdBoreFirstShotCritBonus : 0f;

        public int GetSuppressingFireAmmoCost() => SuppressingFireAmmoCost;

        public bool CanSuppress(string survivorId) => Has(survivorId, SuppressingFireId);

        public float GetCloseQuartersDamageMultiplier(string survivorId, bool confinedOrBreach)
            => (confinedOrBreach && Has(survivorId, CloseQuartersId)) ? CloseQuartersDamageMultiplier : 1f;

        public float GetTrapMisfireChance(string survivorId)
            => Has(survivorId, TrapSetterId) ? 0f : DefaultTrapMisfireChance;

        public float GetTrapDamageMultiplier(string survivorId)
            => Has(survivorId, TrapSetterId) ? TrapSetterDamageMultiplier : 1f;

        public float GetDisarmSuccessRate(string survivorId)
            => Has(survivorId, TrapSetterId) ? 1f : DefaultDisarmSuccess;

        public bool ShouldRetainBestLootOnFlee(string survivorId) => Has(survivorId, LootersReflexId);

        public bool IsImmuneToKillMorale(string survivorId) => Has(survivorId, DesensitizedId);

        /// <summary>Kill morale loss unless Desensitized. Returns 0 when immune.</summary>
        public float ApplyHumanKillMorale(string survivorId)
            => IsImmuneToKillMorale(survivorId) ? 0f : HumanKillMoralePenalty;

        // ── Milestone recording (deterministic, save-safe) ──────────────

        public void RecordWeaponJamSurvived(string survivorId)
        {
            var e = GetOrCreate(survivorId);
            e.JamsSurvived++;
            if (e.JamsSurvived >= JamsForTapRackBang) Grant(survivorId, TapRackBangId);
        }

        public void RecordStealthKill(string survivorId)
        {
            var e = GetOrCreate(survivorId);
            e.StealthKills++;
            if (e.StealthKills >= StealthKillsForColdBore) Grant(survivorId, ColdBoreId);
        }

        public void RecordAmmoExpended(string survivorId, int rounds)
        {
            if (rounds <= 0) return;
            var e = GetOrCreate(survivorId);
            e.AmmoExpended += rounds;
            if (e.AmmoExpended >= AmmoForSuppressingFire) Grant(survivorId, SuppressingFireId);
        }

        public void RecordConfinedEncounterSurvived(string survivorId)
        {
            var e = GetOrCreate(survivorId);
            e.ConfinedEncountersSurvived++;
            if (e.ConfinedEncountersSurvived >= ConfinedEncountersForCloseQuarters) Grant(survivorId, CloseQuartersId);
        }

        public void RecordTrapDeployed(string survivorId, int count = 1)
        {
            if (count <= 0) return;
            var e = GetOrCreate(survivorId);
            e.TrapsDeployed += count;
            if (e.TrapsDeployed >= TrapsForTrapSetter) Grant(survivorId, TrapSetterId);
        }

        public void RecordFlee(string survivorId)
        {
            var e = GetOrCreate(survivorId);
            e.Flees++;
            if (e.Flees >= FleesForLootersReflex) Grant(survivorId, LootersReflexId);
        }

        /// <summary>Record a human kill; grants Desensitized at the threshold.</summary>
        public void RecordHumanKill(string survivorId)
        {
            var e = GetOrCreate(survivorId);
            e.HumanKills++;
            if (e.HumanKills >= HumanKillsForDesensitized) Grant(survivorId, DesensitizedId);
        }

        public CombatPerkEntry? GetEntry(string survivorId)
        {
            return !string.IsNullOrEmpty(survivorId) && _bySurvivor.TryGetValue(survivorId, out var e)
                ? CloneEntry(e) : null;
        }

        private CombatPerkEntry GetOrCreate(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var e))
            {
                e = new CombatPerkEntry { SurvivorId = survivorId, ActivePerks = new List<string>() };
                _bySurvivor[survivorId] = e;
            }
            return e;
        }

        private static CombatPerkEntry CloneEntry(CombatPerkEntry src)
        {
            var c = new CombatPerkEntry
            {
                SurvivorId = src.SurvivorId,
                JamsSurvived = src.JamsSurvived,
                StealthKills = src.StealthKills,
                AmmoExpended = src.AmmoExpended,
                ConfinedEncountersSurvived = src.ConfinedEncountersSurvived,
                TrapsDeployed = src.TrapsDeployed,
                Flees = src.Flees,
                HumanKills = src.HumanKills,
                ActivePerks = new List<string>()
            };
            if (src.ActivePerks != null)
            {
                var ordered = new List<string>(src.ActivePerks);
                ordered.Sort(string.CompareOrdinal);
                c.ActivePerks.AddRange(ordered);
            }
            return c;
        }

        // ── Save / Load ──────────────────────────────────────────────────

        public CombatPerkState CaptureState()
        {
            var save = new CombatPerkState();
            var ids = new List<string>(_bySurvivor.Keys);
            ids.Sort(string.CompareOrdinal);
            for (int i = 0; i < ids.Count; i++)
                save.Entries.Add(CloneEntry(_bySurvivor[ids[i]]));
            return save;
        }

        public void RestoreState(CombatPerkState save)
        {
            _bySurvivor.Clear();
            if (save?.Entries == null) return;
            for (int i = 0; i < save.Entries.Count; i++)
            {
                var e = save.Entries[i];
                if (e == null || string.IsNullOrEmpty(e.SurvivorId)) continue;
                _bySurvivor[e.SurvivorId] = e;
            }
            OnCombatPerkEarned = null!; // do not replay grants on restore
        }
    }
}
