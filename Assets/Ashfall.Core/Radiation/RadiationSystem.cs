using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Radiation
{
    using InventoryWornGear = Ashfall.Core.Inventory.WornGear;
    /// <summary>
    /// Minimal, engine-agnostic per-survivor radiation state the RadiationSystem
    /// mutates. Kept as a plain DTO so hosts (Godot or Unity) can map it onto
    /// their own survivor objects.
    /// </summary>
    public class SurvivorRadState
    {
        public string Id = string.Empty;
        public float RadiationDose;              // 0..100 current dose (acute scale)
        public float LifetimeRadiationExposure;  // unclamped lifetime mSv
        public bool HasRadResistance;
        public float RadResistanceHoursRemaining;
        public float IodineProtectionTimer;

        public bool HasAcuteRadiationSickness;
        public bool HasChronicIllness;
        public bool HasAcuteRadiationSyndrome;

        public bool IsAlive = true;

        public bool HasStatus(SurvivorStatus status)
        {
            switch (status)
            {
                case SurvivorStatus.AcuteRadiationSickness: return HasAcuteRadiationSickness;
                case SurvivorStatus.ChronicIllness: return HasChronicIllness;
                case SurvivorStatus.AcuteRadiationSyndrome: return HasAcuteRadiationSyndrome;
                case SurvivorStatus.RadResistance: return HasRadResistance;
                default: return false;
            }
        }
    }

    public enum SurvivorStatus
    {
        AcuteRadiationSickness,
        ChronicIllness,
        AcuteRadiationSyndrome,
        RadResistance
    }

    /// <summary>
    /// Environmental/protective context for a radiation tick: zone dose rate,
    /// shelter shielding, optional shelter interior-rad query, worn gear.
    /// Ported from Unity's ExposureContext minus the reflection-based Shelter hook.
    /// </summary>
    public class ExposureContext
    {
        public float ZoneRadLevel;
        public float ShelterShielding;
        public Func<float, float> ShelterRadQuery; // zone -> interior rads/hr
        public List<InventoryWornGear> WornGear = new List<InventoryWornGear>();
    }

    /// <summary>
    /// Radioactive contamination carried on a surface, zone, item, or survivor:
    /// current dose-rate, natural decay, and active-shedding flag. Ported from
    /// Unity's Contamination.
    /// </summary>
    public class Contamination
    {
        public float RadsPerHour;
        public float DecayPerHour;
        public bool IsActive;

        public void Decay(float gameHours)
        {
            if (gameHours <= 0f) return;
            RadsPerHour = MathfCompat.Max(0f, RadsPerHour - DecayPerHour * gameHours);
            IsActive = RadsPerHour > 0f;
        }

        public float AmbientContribution()
        {
            return IsActive ? MathfCompat.Max(0f, RadsPerHour) : 0f;
        }
    }

    /// <summary>
    /// Engine-agnostic port of Unity's RadiationSystem. Accumulates dose from the
    /// environment and contaminated sources, applies shelter shielding and worn-gear
    /// protection, tracks chronic/acute thresholds, and drives iodine / anti-rad /
    /// resistance timers. Originally wired through a Survivor object; here it operates
    /// on SurvivorRadState with pluggable delegates for personal-quest modifiers.
    /// </summary>
    public class RadiationSystem
    {
        public const float AcuteThreshold = 80f;
        public const float ChronicLifetimeThreshold = 400f;
        public const float HealthLossPerHourAtAcute = 5f;
        public const float IodineResistanceHours = 6f;
        public const float RadResistanceFactor = 0.5f;

        private readonly List<SurvivorRadState> _survivors = new List<SurvivorRadState>();
        private readonly Dictionary<string, Dosimeter> _dosimeters = new Dictionary<string, Dosimeter>();
        private readonly Func<SurvivorRadState, ExposureContext> _exposureContext;
        private readonly Action<SurvivorRadState, string, float> _applyNeed; // survivor, needId, delta
        private readonly Action<SurvivorRadState, float> _onExposed;          // survivor, doseDelta
        private readonly Func<SurvivorRadState, float> _hazmatDegradeMultiplier;
        private readonly Func<SurvivorRadState, bool> _radiotrophic;
        private readonly int _seed;

        public bool IsPaused { get; set; }

        public event Action<SurvivorRadState, float> OnDoseChanged;
        public event Action<SurvivorRadState, SurvivorStatus> OnStatusGained;
        public event Action<SurvivorRadState, SurvivorStatus> OnStatusLost;

        public RadiationSystem(
Func<SurvivorRadState, ExposureContext>? exposureContext = null,
Action<SurvivorRadState, string, float>? applyNeed = null,
Action<SurvivorRadState, float>? onExposed = null,
Func<SurvivorRadState, float>? hazmatDegradeMultiplier = null,
Func<SurvivorRadState, bool>? radiotrophic = null,
            int seed = 1401)
        {
            _exposureContext = exposureContext;
            _applyNeed = applyNeed;
            _onExposed = onExposed;
            _hazmatDegradeMultiplier = hazmatDegradeMultiplier;
            _radiotrophic = radiotrophic;
            _seed = seed;
        }

        public void Register(SurvivorRadState survivor)
        {
            if (survivor != null && !_survivors.Contains(survivor))
                _survivors.Add(survivor);
        }

        public void Unregister(SurvivorRadState survivor)
        {
            _survivors.Remove(survivor);
        }

        public void Tick(float gameHours)
        {
            if (IsPaused || gameHours <= 0f) return;
            for (int i = 0; i < _survivors.Count; i++)
            {
                var survivor = _survivors[i];
                if (survivor == null || !survivor.IsAlive) continue;

                var context = _exposureContext != null ? _exposureContext(survivor) : null;
                float zone = context != null ? context.ZoneRadLevel : 0f;
                var worn = context != null ? context.WornGear : null;

                float gearProtection = ComputeGearProtection(worn!);
                float exposurePerHour;
                if (context != null && context.ShelterRadQuery != null)
                {
                    float interiorRads = context.ShelterRadQuery(zone);
                    exposurePerHour = MathfCompat.Max(0f, interiorRads - gearProtection);
                }
                else
                {
                    float shielding = context != null ? context.ShelterShielding : 0f;
                    exposurePerHour = ComputeExposurePerHour(zone, gearProtection, shielding);
                }

                if (survivor.HasRadResistance)
                    exposurePerHour *= RadResistanceFactor;

                DegradeWornGear(worn!, gameHours);
                Expose(survivor, exposurePerHour, gameHours);

                var dosimeter = GetDosimeter(survivor.Id);
                dosimeter.Record(exposurePerHour * gameHours, gameHours);
                dosimeter.LifetimeDose = survivor.LifetimeRadiationExposure;

                TickRadResistance(survivor, gameHours);
            }
        }

        public static float ComputeGearProtection(IReadOnlyList<InventoryWornGear> worn)
        {
            if (worn == null) return 0f;
            float total = 0f;
            for (int i = 0; i < worn.Count; i++)
                if (worn[i] != null) total += worn[i].EffectiveProtection();
            return MathfCompat.Max(0f, total);
        }

        public static float ComputeEffectiveAmbient(float zoneRadLevel, float shelterShielding)
        {
            return MathfCompat.Max(0f, zoneRadLevel - MathfCompat.Max(0f, shelterShielding));
        }

        public static float ComputeExposurePerHour(float zoneRadLevel, float gearProtection, float shelterShielding)
        {
            return MathfCompat.Max(0f, zoneRadLevel
                - MathfCompat.Max(0f, gearProtection)
                - MathfCompat.Max(0f, shelterShielding));
        }

        public static float ComputeContaminationAmbient(System.Collections.Generic.IEnumerable<Contamination> contaminations)
        {
            if (contaminations == null) return 0f;
            float total = 0f;
            foreach (var c in contaminations)
                if (c != null) total += c.AmbientContribution();
            return MathfCompat.Max(0f, total);
        }

        public Dosimeter GetDosimeter(string survivorId)
        {
            if (!_dosimeters.TryGetValue(survivorId, out var dosimeter))
            {
                dosimeter = new Dosimeter { SurvivorId = survivorId };
                _dosimeters[survivorId] = dosimeter;
            }
            return dosimeter;
        }

        public void Expose(SurvivorRadState survivor, float radsPerHour, float hours)
        {
            if (survivor == null || !survivor.IsAlive || hours <= 0f) return;

            // Radiotrophic: high-rad zones heal instead of damaging.
            bool radiotrophic = _radiotrophic != null && _radiotrophic(survivor) && radsPerHour >= 50f;
            if (radiotrophic)
            {
                survivor.LifetimeRadiationExposure = MathfCompat.Max(
                    0f, survivor.LifetimeRadiationExposure + radsPerHour * hours * 0.1f);
                OnDoseChanged?.Invoke(survivor, survivor.RadiationDose);
                return;
            }

            if (radsPerHour != 0f)
            {
                float delta = radsPerHour * hours;
                survivor.LifetimeRadiationExposure = MathfCompat.Max(0f, survivor.LifetimeRadiationExposure + delta);
                survivor.RadiationDose = MathfCompat.Clamp(survivor.RadiationDose + delta, 0f, 100f);
                OnDoseChanged?.Invoke(survivor, survivor.RadiationDose);
                _onExposed?.Invoke(survivor, delta);
            }

            if (survivor.RadiationDose >= AcuteThreshold)
            {
                _applyNeed?.Invoke(survivor, "health", -HealthLossPerHourAtAcute * hours);
                GrantStatus(survivor, SurvivorStatus.AcuteRadiationSickness);
            }

            if (survivor.LifetimeRadiationExposure >= ChronicLifetimeThreshold)
                GrantStatus(survivor, SurvivorStatus.ChronicIllness);
        }

        public void AdministerIodine(SurvivorRadState survivor)
        {
            if (survivor == null || !survivor.IsAlive) return;
            survivor.RadResistanceHoursRemaining = MathfCompat.Max(survivor.RadResistanceHoursRemaining, IodineResistanceHours);
            survivor.HasRadResistance = true;
            survivor.IodineProtectionTimer = MathfCompat.Max(survivor.IodineProtectionTimer, IodineWindowHours);
            GrantStatus(survivor, SurvivorStatus.RadResistance);
        }

        public const float IodineWindowHours = 24f;

        public void AdministerAntiRad(SurvivorRadState survivor, float radsRemoved)
        {
            if (survivor == null || !survivor.IsAlive || radsRemoved <= 0f) return;
            survivor.RadiationDose = MathfCompat.Clamp(survivor.RadiationDose - radsRemoved, 0f, 100f);
            OnDoseChanged?.Invoke(survivor, survivor.RadiationDose);
        }

        public void SetDose(SurvivorRadState survivor, float dose)
        {
            if (survivor == null || !survivor.IsAlive) return;
            survivor.RadiationDose = MathfCompat.Clamp(dose, 0f, 100f);
            OnDoseChanged?.Invoke(survivor, survivor.RadiationDose);
        }

        public void AdjustDose(SurvivorRadState survivor, float delta)
        {
            if (survivor == null || !survivor.IsAlive || delta == 0f) return;
            SetDose(survivor, survivor.RadiationDose + delta);
        }

        public void SeedLifetimeExposure(SurvivorRadState survivor, float lifetime)
        {
            if (survivor == null || lifetime <= 0f) return;
            survivor.LifetimeRadiationExposure = MathfCompat.Max(survivor.LifetimeRadiationExposure, lifetime);
            if (survivor.IsAlive && survivor.LifetimeRadiationExposure >= ChronicLifetimeThreshold)
                GrantStatus(survivor, SurvivorStatus.ChronicIllness);
            OnDoseChanged?.Invoke(survivor, survivor.RadiationDose);
        }

        private void TickRadResistance(SurvivorRadState survivor, float gameHours)
        {
            if (!survivor.HasRadResistance) return;
            survivor.RadResistanceHoursRemaining -= gameHours;
            if (survivor.RadResistanceHoursRemaining <= 0f)
            {
                survivor.RadResistanceHoursRemaining = 0f;
                survivor.HasRadResistance = false;
                OnStatusLost?.Invoke(survivor, SurvivorStatus.RadResistance);
            }
        }

        private void DegradeWornGear(List<InventoryWornGear> worn, float gameHours)
        {
            if (worn == null) return;
            float mult = _hazmatDegradeMultiplier != null ? _hazmatDegradeMultiplier(null!) : 1f;
            for (int i = 0; i < worn.Count; i++)
                worn[i]?.Degrade(gameHours * mult);
        }

        private void GrantStatus(SurvivorRadState survivor, SurvivorStatus status)
        {
            if (survivor.HasStatus(status)) return;
            switch (status)
            {
                case SurvivorStatus.AcuteRadiationSickness: survivor.HasAcuteRadiationSickness = true; break;
                case SurvivorStatus.ChronicIllness: survivor.HasChronicIllness = true; break;
                case SurvivorStatus.AcuteRadiationSyndrome: survivor.HasAcuteRadiationSyndrome = true; break;
                case SurvivorStatus.RadResistance: survivor.HasRadResistance = true; break;
            }
            OnStatusGained?.Invoke(survivor, status);
        }
    }

    /// <summary>Read-model dosimeter: recorded dose rate and lifetime dose.</summary>
    public class Dosimeter
    {
        public string SurvivorId = string.Empty;
        public float CurrentReading;
        public float LifetimeDose;

        public void Record(float doseRecorded, float hours)
        {
            if (hours <= 0f) return;
            CurrentReading = doseRecorded / hours;
        }
    }
}
