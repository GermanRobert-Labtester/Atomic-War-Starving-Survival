using System;
using System.Collections.Generic;

namespace Ashfall.Core.Combat
{
    // ── Enums ────────────────────────────────────────────────────────────

    /// <summary>Lifecycle of a combat encounter.</summary>
    public enum CombatPhase
    {
        Setup = 0,
        PlayerTurn = 1,
        EnemyTurn = 2,
        Resolved = 3,
        Won = 4,
        Lost = 5,
        Retreated = 6
    }

    /// <summary>Left/center/right combat lanes. Index 0/1/2.</summary>
    public enum CombatLane
    {
        Left = 0,
        Center = 1,
        Right = 2
    }

    /// <summary>
    /// Explicit tactical stance. Distinct from expedition travel stances
    /// (Stealth/Speed) which only influence encounter setup.
    /// </summary>
    public enum TacticalStance
    {
        HoldPosition = 0,   // defensive, +defense, -mobility, more ammo-efficient
        Advance = 1,        // aggressive, +accuracy/+damage, -defense, more degradation/noise
        SuppressiveFire = 2,// area fire, pins enemies, heavy ammo + jam risk
        Retreat = 3,        // disengage, -accuracy, higher flee success, injure risk on exit
        LastStand = 4       // deliberate terminal stand, +accuracy/+damage, cannot flee
    }

    /// <summary>Result of a ballistic resolution step / the final outcome.</summary>
    public enum BallisticResult
    {
        DirectHit = 0,   // resolved on intended (or ricochet-selected) target
        Missed = 1,      // shot missed (accuracy)
        Blocked = 2,     // stopped by cover/barrier, no damage to target
        Penetrated = 3,  // passed through cover/barrier, reduced damage reaches target
        Ricocheted = 4,  // deflected, redirected energy to a secondary target
        Stopped = 5      // absorbed by armor / energy exhausted / ricochet lost
    }

    /// <summary>Deterministic reason codes carried by every ballistic outcome.</summary>
    public enum BallisticReason
    {
        None = 0,
        AccuracyFail = 1,
        CoverBlocked = 2,
        BarrierBlocked = 3,
        CoverPenetrated = 4,
        BarrierPenetrated = 5,
        ArmorAbsorbed = 6,
        RicochetedToSecondary = 7,
        RicochetLost = 8,
        EnergyExhausted = 9,
        DownedTarget = 10,
        FlankingBonus = 11
    }

    // ── Data DTOs (engine-agnostic, plain serializable classes) ─────────

    /// <summary>One item of loot / captured supplies from a battle.</summary>
    [Serializable]
    public class CombatLootEntry
    {
        public string itemId = string.Empty;
        public int quantity = 1;
        public float weightKg = 1f;
    }

    /// <summary>Serialized state of a single combatant.</summary>
    [Serializable]
    public class CombatantState
    {
        public string Id = string.Empty;
        public string Name = string.Empty;
        public string SurvivorId = string.Empty; // for players: the survivor the host writes back to
        public bool IsPlayer;
        public string FactionId = string.Empty;
        public int Lane = (int)CombatLane.Center;
        public float Health = 100f;
        public float MaxHealth = 100f;
        public float ArmorRating; // 0..1 damage reduction from worn armor
        public float CoverRating; // 0..1 chance this unit is behind cover
        public bool IsDowned;
        public int BleedTurnsRemaining;
        public bool IsPinned;
        public int PinnedTurnsRemaining;
        public bool IsLastStand;
        public string WeaponInstanceId = string.Empty;
        public bool HasFled;

        // ── AI trait fields populated from CombatCatalog (Plan 10 ─
        // strata populated by <see cref="Ashfall.Core.Combat.CombatantFactory"/>
        // when an encounter setup hands it a `combatant_*` id. Defaults are
        // safe for combatant rows constructed by the legacy hand-coded path
        // so old saves and legacy encounter setups continue to work without
        // behaviour change.
        public string AiStancePreference = "HoldPosition"; // TacticalStance name
        public string AiSpecialMove = "None";            // None|Burrow|Flank|Spore|Charge|SuppressiveFire|TacticalRetreat
        public float AiAccuracyMod = 1f;                 // multiplier on outgoing accuracy
        public float AiDamageMod = 1f;                   // multiplier on outgoing damage
        public float SurrenderThreshold = -1f;           // -1 = never; 0..1 = open path
        public float FleeThreshold = -1f;                // -1 = never; 0..1 = open path
        public string CatalogId = string.Empty;          // combatant_* id when spawned via factory; "" for legacy rows
    }

    /// <summary>Per-lane barrier (sandbags, barricade) blocking fire.</summary>
    [Serializable]
    public class BarrierState
    {
        public string Id = string.Empty;
        public int Lane = (int)CombatLane.Center;
        public bool IsPlayer;
        public string MaterialId = string.Empty;
        public float IntegrityPct = 100f;
        public float ArmorRating; // flat damage absorbed while intact
    }

    /// <summary>
    /// Serializable weapon-instance state token maintained by the sim and
    /// reconciled against the real inventory via host hooks. The condition,
    /// jam, repair and degradation all live here so they survive save/load.
    /// </summary>
    [Serializable]
    public class WeaponInstanceState
    {
        public string InstanceId = string.Empty;
        public string WeaponId = string.Empty;
        public string OwnerSurvivorId = string.Empty;
        public string OwnerCombatantId = string.Empty;
        public float ConditionPct = 1f;
        public bool IsJammed;
        public int JamClearTicksRemaining;
        public int JamsSurvived;
        public int ShotsFired;
        public int BurstCount;
        public float CachedJamChance;   // same value the sim uses for its jam roll
        public float AshFoul;            // persistent environmental fouling (ash/contamination)
        public string AmmoId = string.Empty;
        public int AmmoRemaining = 0;
        public int MagazineCapacity;     // item 6: max rounds in one reload
        public int ScrapRepairCost;     // exposed to the UI
    }

    /// <summary>Combat-log event appended to the encounter history.</summary>
    [Serializable]
    public class CombatEvent
    {
        public string Kind = string.Empty;
        public int Day;
        public int Turn;
        public string SubjectId = string.Empty;
        public string TargetId = string.Empty;
        public string Detail = string.Empty;
        public float Value;
    }

    /// <summary>
    /// Record of weapon wear during a combat encounter.
    /// </summary>
    [Serializable]
    public class CombatWeaponWearRecord
    {
        public string InstanceId = string.Empty;
        public string WeaponId = string.Empty;
        public string OwnerSurvivorId = string.Empty;
        public float StartConditionPct = 1f;
        public float FinalConditionPct = 1f;
        public float WearDeltaPct = 0f;
    }

    /// <summary>
    /// Record of ammunition consumed during a combat encounter.
    /// </summary>
    [Serializable]
    public class CombatAmmoSpentRecord
    {
        public string AmmoId = string.Empty;
        public int RoundsSpent = 0;
    }

    /// <summary>
    /// Exactly-once aftermath report generated upon encounter resolution.
    /// Keyed to a stable ResolutionId to prevent duplicate application on save/reload.
    /// </summary>
    [Serializable]
    public class CombatAftermath
    {
        public string ResolutionId = string.Empty;
        public string EncounterId = string.Empty;
        public string Outcome = string.Empty;
        public List<string> SurvivorInjuries = new List<string>();
        public List<string> SurvivorDeaths = new List<string>();
        public List<CombatWeaponWearRecord> WeaponWear = new List<CombatWeaponWearRecord>();
        public List<CombatAmmoSpentRecord> AmmoSpent = new List<CombatAmmoSpentRecord>();
        public List<CombatLootEntry> LootConsequences = new List<CombatLootEntry>();
        public float MoraleConsequences = 0f;
        public bool IsApplied;
    }

    /// <summary>
    /// Persisted start condition of a weapon instance for wear reconciliation.
    /// </summary>
    [Serializable]
    public class BoundWeaponConditionEntry
    {
        public string instanceId = string.Empty;
        public float conditionPct = 1f;
    }

    /// <summary>
    /// Preflight check for tactical action legality and reason code.
    /// </summary>
    public readonly struct ActionPreflight
    {
        public bool CanExecute { get; }
        public string Reason { get; }

        public ActionPreflight(bool canExecute, string reason = "")
        {
            CanExecute = canExecute;
            Reason = reason ?? string.Empty;
        }

        public static ActionPreflight Ok => new ActionPreflight(true, string.Empty);
        public static ActionPreflight Blocked(string reason) => new ActionPreflight(false, reason);
    }

    /// <summary>Full serialized combat-save state (one active or past encounter).</summary>
    [Serializable]
    public class CombatState
    {
        public const int CurrentSaveVersion = 3;

        public string SystemId = TacticalCombatSystem.SystemId;
        public int SaveVersion = CurrentSaveVersion;
        public string EncounterId = string.Empty;
        public string ExpeditionId = string.Empty;
        public string LocationId = string.Empty;
        public string LocationName = string.Empty;
        public int Day = 1;
        public int Seed;
        public int Turn = 1;
        public int Phase = (int)CombatPhase.Setup;
        public string PlayerStance = TacticalCombatSystem.StanceId(TacticalStance.HoldPosition);
        public int RoundNumber = 0;
        public bool Resolved;
        public string OutcomeText = string.Empty;
        public string ResolutionId = string.Empty;
        public CombatAftermath? Aftermath;
        public List<BoundWeaponConditionEntry> BoundWeaponConditions = new List<BoundWeaponConditionEntry>();
        public List<CombatantState> Combatants = new List<CombatantState>();
        public List<WeaponInstanceState> Weapons = new List<WeaponInstanceState>();
        public List<BarrierState> Barriers = new List<BarrierState>();
        public List<CombatEvent> Events = new List<CombatEvent>();
        public List<CombatLootEntry> Loot = new List<CombatLootEntry>();

        public float GetBoundWeaponStartCondition(string instanceId, float defaultVal = 1f)
        {
            if (string.IsNullOrEmpty(instanceId) || BoundWeaponConditions == null) return defaultVal;
            for (int i = 0; i < BoundWeaponConditions.Count; i++)
            {
                if (string.Equals(BoundWeaponConditions[i].instanceId, instanceId, StringComparison.Ordinal))
                    return BoundWeaponConditions[i].conditionPct;
            }
            return defaultVal;
        }

        public void SetBoundWeaponStartCondition(string instanceId, float condition)
        {
            if (string.IsNullOrEmpty(instanceId)) return;
            BoundWeaponConditions ??= new List<BoundWeaponConditionEntry>();
            for (int i = 0; i < BoundWeaponConditions.Count; i++)
            {
                if (string.Equals(BoundWeaponConditions[i].instanceId, instanceId, StringComparison.Ordinal))
                {
                    BoundWeaponConditions[i].conditionPct = condition;
                    return;
                }
            }
            BoundWeaponConditions.Add(new BoundWeaponConditionEntry
            {
                instanceId = instanceId,
                conditionPct = condition
            });
        }
    }

    /// <summary>Result of a player action — success + message + appended events.</summary>
    public class CombatActionResult
    {
        public bool Success;
        public string Message = string.Empty;
        public List<CombatEvent> AddedEvents = new List<CombatEvent>();
    }

    /// <summary>Typed snapshot the host presents in the combat UI.</summary>
    public class CombatSnapshot
    {
        public string EncounterId = string.Empty;
        public string ResolutionId = string.Empty;
        public string LocationName = string.Empty;
        public int Day;
        public int Turn;
        public string Phase = string.Empty;
        public string StanceId = string.Empty;
        public bool Resolved;
        public string OutcomeText = string.Empty;
        public bool IsActive;
        public CombatAftermath? Aftermath;
        public List<CombatantSnapshot> Combatants = new List<CombatantSnapshot>();
        public List<WeaponSnapshot> Weapons = new List<WeaponSnapshot>();
        public List<CombatEvent> Events = new List<CombatEvent>();
        public List<CombatLootEntry> Loot = new List<CombatLootEntry>();
    }

    /// <summary>A combatant row for the UI.</summary>
    public class CombatantSnapshot
    {
        public string Id = string.Empty;
        public string Name = string.Empty;
        public bool IsPlayer;
        public string FactionId = string.Empty;
        public string Lane = string.Empty;
        public int Health;
        public int MaxHealth;
        public int ArmorRating;
        public int CoverRating;
        public bool IsDowned;
        public bool IsPinned;
        public bool IsLastStand;
        public string Status = string.Empty;
        public string WeaponName = string.Empty;
        public int WeaponConditionPct;
        public bool WeaponJammed;
        public string WeaponAmmo = string.Empty;
    }

    /// <summary>A weapon row for the UI (jury-rigged / armory monitor).</summary>
    public class WeaponSnapshot
    {
        public string InstanceId = string.Empty;
        public string WeaponId = string.Empty;
        public string WeaponName = string.Empty;
        public int ConditionPct;
        public int JamChancePct;
        public bool IsJammed;
        public int ScrapRepairCost;
        public int AmmoRemaining;
        public string OwnerSurvivorId = string.Empty;
    }
}
