using System;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Accumulates radiation dose on survivors from the environment and
    /// contaminated items/zones, applies shelter shielding / filtration
    /// mitigation, and triggers chronic-illness effects at high cumulative dose.
    ///
    /// Health loss from acute exposure is applied through NeedsSystem.Modify so
    /// Health stays a single-writer value that always raises OnNeedChanged.
    /// </summary>
    public class RadiationSystem
    {
        /// <summary>Current dose (0..100) at/above this triggers Acute Radiation Sickness.</summary>
        public const float AcuteThreshold = 80f;
        /// <summary>Unclamped lifetime exposure at/above this triggers Chronic Illness.</summary>
        public const float ChronicLifetimeThreshold = 400f;
        /// <summary>Health lost per hour while current dose is at/above AcuteThreshold.</summary>
        public const float HealthLossPerHourAtAcute = 5f;

        private readonly NeedsSystem _needsSystem;

        /// <summary>Fired whenever a survivor's current radiation dose changes.</summary>
        public event Action<Survivor, float> OnDoseChanged;
        /// <summary>Fired once when a survivor first gains a radiation-driven status.</summary>
        public event Action<Survivor, SurvivorStatus> OnStatusGained;

        public RadiationSystem(NeedsSystem needsSystem)
        {
            _needsSystem = needsSystem != null ? needsSystem : throw new ArgumentNullException(nameof(needsSystem));
        }

        /// <summary>
        /// Advance dose accumulation over elapsed game hours for all survivors from
        /// ambient sources. Not yet implemented: needs a survivor registry and a
        /// zone/contamination + worn-protection lookup (FalloutMap, Inventory),
        /// neither of which exist yet. Call Expose(...) directly per survivor with
        /// an already-computed rate until those systems land.
        /// </summary>
        public void Tick(float gameHours) => throw new NotImplementedException(
            "RadiationSystem.Tick needs a survivor registry and a zone/protection " +
            "rate lookup that don't exist yet. Call Expose(survivor, radsPerHour, hours) directly.");

        /// <summary>Expose a survivor to a dose rate for a number of hours.</summary>
        public void Expose(Survivor survivor, float radsPerHour, float hours)
        {
            if (survivor == null || !survivor.IsAlive || hours <= 0f)
            {
                return;
            }

            if (radsPerHour != 0f)
            {
                float delta = radsPerHour * hours;
                survivor.LifetimeRadiationExposure = Mathf.Max(0f, survivor.LifetimeRadiationExposure + delta);
                survivor.RadiationDose = Mathf.Clamp(survivor.RadiationDose + delta, 0f, 100f);
                OnDoseChanged?.Invoke(survivor, survivor.RadiationDose);
            }

            if (survivor.RadiationDose >= AcuteThreshold)
            {
                _needsSystem.Modify(survivor, NeedKind.Health, -HealthLossPerHourAtAcute * hours);
                GrantStatus(survivor, SurvivorStatus.AcuteRadiationSickness);
            }

            if (survivor.LifetimeRadiationExposure >= ChronicLifetimeThreshold)
            {
                GrantStatus(survivor, SurvivorStatus.ChronicIllness);
            }
        }

        /// <summary>Administer iodine pills to blunt thyroid uptake for a window of time.</summary>
        public void AdministerIodine(Survivor survivor) => throw new NotImplementedException();

        /// <summary>Administer anti-rad medication to reduce cumulative dose.</summary>
        public void AdministerAntiRad(Survivor survivor, float radsRemoved) => throw new NotImplementedException();

        private void GrantStatus(Survivor survivor, SurvivorStatus status)
        {
            if (survivor.HasStatus(status))
            {
                return;
            }

            if (status == SurvivorStatus.AcuteRadiationSickness)
            {
                survivor.HasAcuteRadiationSickness = true;
            }
            else if (status == SurvivorStatus.ChronicIllness)
            {
                survivor.HasChronicIllness = true;
            }

            OnStatusGained?.Invoke(survivor, status);
        }
    }
}
