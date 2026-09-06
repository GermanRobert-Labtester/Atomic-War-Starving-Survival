using System;
using System.Collections.Generic;

namespace Ashfall.Core.Muster
{
    /// <summary>
    /// Lifecycle phases of a Muster military campaign.
    /// </summary>
    public enum MusterConflictPhase
    {
        Idle = 0,
        Mobilizing = 1,
        Ready = 2,
        Deployed = 3,
        Engaged = 4,
        Resolving = 5,
        Aftermath = 6,
        Recovering = 7,
        Cancelled = 8,
        Withdrawn = 9,
        Routed = 10
    }

    /// <summary>
    /// Combat role assigned to a mustered survivor.
    /// </summary>
    public enum MusterSoldierRole
    {
        Infantry = 0,
        Scout = 1,
        Heavy = 2,
        Medic = 3
    }

    /// <summary>
    /// Represents one living shelter combatant committed to the active muster force.
    /// Invariant: SurvivorId must resolve to an authentic living survivor in ISurvivorRoster.
    /// </summary>
    [Serializable]
    public sealed class MusterSoldier
    {
        public string SurvivorId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public MusterSoldierRole Role { get; set; }
        public string AssignedWeaponId { get; set; } = string.Empty;
        public string AssignedArmorId { get; set; } = string.Empty;
        public float Readiness { get; set; } = 1.0f;
        public float CombatStrength { get; set; } = 10.0f;
        public bool IsCasualty { get; set; }
        public bool IsWounded { get; set; }

        public MusterSoldier Clone() => new MusterSoldier
        {
            SurvivorId = SurvivorId,
            DisplayName = DisplayName,
            Role = Role,
            AssignedWeaponId = AssignedWeaponId,
            AssignedArmorId = AssignedArmorId,
            Readiness = Readiness,
            CombatStrength = CombatStrength,
            IsCasualty = IsCasualty,
            IsWounded = IsWounded
        };
    }

    /// <summary>
    /// Atomically reserved shelter munitions and supplies committed to the operation.
    /// Invariant: Issued + Remaining + Lost + Returned == InitialReservedAmount.
    /// </summary>
    [Serializable]
    public sealed class MusterSupplyReservation
    {
        public int FoodRations { get; set; }
        public int CleanWaterUnits { get; set; }
        public int AmmoUnits { get; set; }
        public int MedicalUnits { get; set; }
        public int FuelUnits { get; set; }

        public int RationsConsumed { get; set; }
        public int WaterConsumed { get; set; }
        public int AmmoConsumed { get; set; }
        public int MedsConsumed { get; set; }
        public int FuelConsumed { get; set; }

        public MusterSupplyReservation Clone() => new MusterSupplyReservation
        {
            FoodRations = FoodRations,
            CleanWaterUnits = CleanWaterUnits,
            AmmoUnits = AmmoUnits,
            MedicalUnits = MedicalUnits,
            FuelUnits = FuelUnits,
            RationsConsumed = RationsConsumed,
            WaterConsumed = WaterConsumed,
            AmmoConsumed = AmmoConsumed,
            MedsConsumed = MedsConsumed,
            FuelConsumed = FuelConsumed
        };
    }

    /// <summary>
    /// Typed combat modifiers derived from active military or warlord doctrine.
    /// </summary>
    [Serializable]
    public sealed class MusterDoctrineModifier
    {
        public string DoctrineId { get; set; } = "warlord_doctrine_toll";
        public float RecruitmentCostMultiplier { get; set; } = 1.0f;
        public float SupplyBurnMultiplier { get; set; } = 1.0f;
        public float RiskTolerance { get; set; } = 0.5f;
        public float RetreatThreshold { get; set; } = 0.3f;
        public float CasualtyTolerance { get; set; } = 0.2f;
        public int EscalationWeight { get; set; } = 15;

        public static MusterDoctrineModifier FromDoctrineId(string? doctrineId)
        {
            var m = new MusterDoctrineModifier();
            if (string.IsNullOrEmpty(doctrineId)) return m;

            m.DoctrineId = doctrineId;
            switch (doctrineId)
            {
                case "warlord_doctrine_procedure":
                    m.RecruitmentCostMultiplier = 1.2f;
                    m.SupplyBurnMultiplier = 1.1f;
                    m.RiskTolerance = 0.45f;
                    m.RetreatThreshold = 0.4f;
                    m.CasualtyTolerance = 0.15f;
                    m.EscalationWeight = 12;
                    break;
                case "warlord_doctrine_consolidation":
                    m.RecruitmentCostMultiplier = 0.9f;
                    m.SupplyBurnMultiplier = 0.8f;
                    m.RiskTolerance = 0.3f;
                    m.RetreatThreshold = 0.5f;
                    m.CasualtyTolerance = 0.1f;
                    m.EscalationWeight = 8;
                    break;
                case "warlord_doctrine_annexation":
                    m.RecruitmentCostMultiplier = 1.3f;
                    m.SupplyBurnMultiplier = 1.5f;
                    m.RiskTolerance = 0.8f;
                    m.RetreatThreshold = 0.15f;
                    m.CasualtyTolerance = 0.4f;
                    m.EscalationWeight = 25;
                    break;
                case "warlord_doctrine_withdrawal":
                    m.RecruitmentCostMultiplier = 0.7f;
                    m.SupplyBurnMultiplier = 0.5f;
                    m.RiskTolerance = 0.15f;
                    m.RetreatThreshold = 0.6f;
                    m.CasualtyTolerance = 0.05f;
                    m.EscalationWeight = -5;
                    break;
                case "warlord_doctrine_toll":
                default:
                    m.RecruitmentCostMultiplier = 1.0f;
                    m.SupplyBurnMultiplier = 1.0f;
                    m.RiskTolerance = 0.6f;
                    m.RetreatThreshold = 0.3f;
                    m.CasualtyTolerance = 0.2f;
                    m.EscalationWeight = 15;
                    break;
            }
            return m;
        }
    }

    /// <summary>
    /// Typed outcome package returned upon engagement resolution.
    /// </summary>
    [Serializable]
    public sealed class MusterEngagementResult
    {
        public string Outcome { get; set; } = "Victory"; // Victory, Defeat, Stalemate, Withdrawn, Routed
        public List<string> Casualties { get; set; } = new List<string>();
        public List<string> Wounded { get; set; } = new List<string>();
        public int TensionDelta { get; set; }
        public int StandingDelta { get; set; }
        public string FactionId { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string EscalationEvent { get; set; } = string.Empty;
        public int Day { get; set; }

        public MusterEngagementResult Clone() => new MusterEngagementResult
        {
            Outcome = Outcome,
            Casualties = new List<string>(Casualties),
            Wounded = new List<string>(Wounded),
            TensionDelta = TensionDelta,
            StandingDelta = StandingDelta,
            FactionId = FactionId,
            Summary = Summary,
            EscalationEvent = EscalationEvent,
            Day = Day
        };
    }

    /// <summary>
    /// Persisted state for the tactical warfare and muster rally layer.
    /// </summary>
    [Serializable]
    public sealed class MusterWarfareState
    {
        public MusterConflictPhase Phase { get; set; } = MusterConflictPhase.Idle;
        public string CommittedBranchId { get; set; } = string.Empty;
        public string ActiveDoctrineId { get; set; } = "warlord_doctrine_toll";
        public string TargetSectorId { get; set; } = string.Empty;
        public List<MusterSoldier> ActiveRoster { get; set; } = new List<MusterSoldier>();
        public MusterSupplyReservation Supplies { get; set; } = new MusterSupplyReservation();
        public int EngagementSeed { get; set; }
        public int DaysInCurrentPhase { get; set; }
        public int RecoveryDaysRemaining { get; set; }
        public MusterEngagementResult? LastEngagementResult { get; set; }

        public MusterWarfareState Clone() => new MusterWarfareState
        {
            Phase = Phase,
            CommittedBranchId = CommittedBranchId,
            ActiveDoctrineId = ActiveDoctrineId,
            TargetSectorId = TargetSectorId,
            ActiveRoster = ActiveRoster.ConvertAll(s => s.Clone()),
            Supplies = Supplies.Clone(),
            EngagementSeed = EngagementSeed,
            DaysInCurrentPhase = DaysInCurrentPhase,
            RecoveryDaysRemaining = RecoveryDaysRemaining,
            LastEngagementResult = LastEngagementResult?.Clone()
        };

        public void RestoreFrom(MusterWarfareState? other)
        {
            if (other == null)
            {
                Phase = MusterConflictPhase.Idle;
                CommittedBranchId = string.Empty;
                ActiveDoctrineId = "warlord_doctrine_toll";
                TargetSectorId = string.Empty;
                ActiveRoster.Clear();
                Supplies = new MusterSupplyReservation();
                EngagementSeed = 0;
                DaysInCurrentPhase = 0;
                RecoveryDaysRemaining = 0;
                LastEngagementResult = null;
                return;
            }

            Phase = other.Phase;
            CommittedBranchId = other.CommittedBranchId ?? string.Empty;
            ActiveDoctrineId = other.ActiveDoctrineId ?? "warlord_doctrine_toll";
            TargetSectorId = other.TargetSectorId ?? string.Empty;
            ActiveRoster.Clear();
            if (other.ActiveRoster != null)
            {
                for (int i = 0; i < other.ActiveRoster.Count; i++)
                    ActiveRoster.Add(other.ActiveRoster[i].Clone());
            }
            Supplies = other.Supplies?.Clone() ?? new MusterSupplyReservation();
            EngagementSeed = other.EngagementSeed;
            DaysInCurrentPhase = other.DaysInCurrentPhase;
            RecoveryDaysRemaining = other.RecoveryDaysRemaining;
            LastEngagementResult = other.LastEngagementResult?.Clone();
        }
    }
}
