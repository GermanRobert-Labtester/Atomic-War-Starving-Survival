using System;
using UnityEngine;
using AtomicWar._Game.Survivors;
using Random = System.Random;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Event data raised when a chronic illness manifests on a survivor.
    /// </summary>
    public struct ChronicIllnessManifestedEvent
    {
        public Survivor Survivor;
        public ChronicIllnessKind Illness;
    }

    /// <summary>
    /// Long-term medical consequences pipeline (Prompt #39).
    /// Tracks latent radiation exposure accumulation (LifetimeRadiationExposure >= 300)
    /// or Acute Radiation Syndrome, assigning chronic diseases and applying stat penalties.
    /// </summary>
    public class AfflictionPipeline
    {
        public const float LifetimeRadThresholdForChronicIllness = 300f;
        public const float DefaultManagementWindowHours = 24f;

        private readonly Random _rng;

        public event Action<Survivor, ChronicIllnessKind> OnChronicIllnessAssigned;

        public AfflictionPipeline(Random rng = null)
        {
            _rng = rng ?? AtomicWar._Game.Utilities.SeededRandom.CreateFixed("affliction_pipeline");
        }

        /// <summary>
        /// Evaluate a survivor for long-term chronic illness manifestation.
        /// Triggers if LifetimeRadiationExposure >= 300 OR AcuteRadiationSyndrome is present.
        /// </summary>
        public bool Evaluate(Survivor survivor, Action<Survivor, SurvivorStatus> grantStatus = null)
        {
            if (survivor == null || !survivor.IsAlive) return false;
            if (survivor.ActiveChronicIllness.HasValue) return false;

            bool triggersPipeline = survivor.LifetimeRadiationExposure >= LifetimeRadThresholdForChronicIllness
                || survivor.HasAcuteRadiationSyndrome
                || survivor.HasStatus(SurvivorStatus.AcuteRadiationSyndrome);

            if (!triggersPipeline) return false;

            // Roll for ChronicIllnessKind
            var kinds = (ChronicIllnessKind[])Enum.GetValues(typeof(ChronicIllnessKind));
            var selectedKind = kinds[_rng.Next(0, kinds.Length)];

            survivor.ActiveChronicIllness = selectedKind;
            survivor.HasChronicIllness = true;

            grantStatus?.Invoke(survivor, SurvivorStatus.ChronicIllness);
            OnChronicIllnessAssigned?.Invoke(survivor, selectedKind);

            return true;
        }

        /// <summary>
        /// Advance chronic illness timers and tick disease effects (e.g. OrganFailure health bleed).
        /// </summary>
        public void Tick(Survivor survivor, float gameHours, NeedsSystem needsSystem = null)
        {
            if (survivor == null || !survivor.IsAlive || gameHours <= 0f) return;

            // Advance medical management countdown
            if (survivor.ChronicIllnessManagedHours > 0f)
            {
                survivor.ChronicIllnessManagedHours = Mathf.Max(0f, survivor.ChronicIllnessManagedHours - gameHours);
            }

            // Organ failure causes continuous health bleed when unmanaged
            if (survivor.ActiveChronicIllness == ChronicIllnessKind.OrganFailure && !survivor.IsChronicIllnessManaged && needsSystem != null)
            {
                needsSystem.Modify(survivor, NeedKind.Health, -1.5f * gameHours);
            }
        }

        /// <summary>
        /// Manage (not cure) a survivor's chronic illness using specific medical supplies
        /// (e.g. AntiRad, Iodine, OxygenCanisters). Grants temporary penalty mitigation.
        /// </summary>
        public bool ManageIllness(Survivor survivor, string medicalSupplyId, float durationHours = DefaultManagementWindowHours)
        {
            if (survivor == null || !survivor.IsAlive || !survivor.ActiveChronicIllness.HasValue) return false;

            survivor.ChronicIllnessManagedHours = Mathf.Max(survivor.ChronicIllnessManagedHours, durationHours);
            return true;
        }
    }
}
