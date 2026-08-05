using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using Random = System.Random;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Accumulates radiation dose on survivors from the environment and
    /// contaminated items/zones, applies shelter shielding / filtration
    /// mitigation, and triggers chronic-illness effects at high cumulative dose.
    ///
    /// Health loss from acute exposure is applied through NeedsSystem.Modify so
    /// Health stays a single-writer value that always raises OnNeedChanged.
    ///
    /// Exposure model (per tick, per survivor):
    ///   gearProtection   = sum of worn gear radProtection scaled by durability fraction
    ///   effectiveAmbient = max(0, zoneRadLevel - shelterShielding)   (shielding caps ambient)
    ///   exposurePerHour  = max(0, zoneRadLevel - gearProtection - shelterShielding)
    /// Tick is pause-aware and driven with gameHours like NeedsSystem; the per-survivor
    /// zone/shielding/worn-gear inputs come from an injected hook so this system stays
    /// decoupled from FalloutMap / Inventory / Shelter (which supply the hook later).
    ///
    /// Iodine grants a timed RadResistance status that multiplies the dose rate by
    /// RadResistanceFactor while active; anti-rad reduces the current dose directly.
    /// </summary>
    public class RadiationSystem
    {
        /// <summary>Current dose (0..100) at/above this triggers Acute Radiation Sickness.</summary>
        public const float AcuteThreshold = 80f;
        /// <summary>Unclamped lifetime exposure at/above this triggers Chronic Illness.</summary>
        public const float ChronicLifetimeThreshold = 400f;
        /// <summary>Health lost per hour while current dose is at/above AcuteThreshold.</summary>
        public const float HealthLossPerHourAtAcute = 5f;
        /// <summary>Hours of temporary rad resistance granted by iodine pills.</summary>
        public const float IodineResistanceHours = 6f;
        /// <summary>Exposure multiplier while rad resistance is active (0.5 = halves dose rate).</summary>
        public const float RadResistanceFactor = 0.5f;

        private readonly NeedsSystem _needsSystem;
        private readonly Func<Survivor, ExposureContext> _exposureContext;
        private readonly Random _rng;
        private readonly List<Survivor> _survivors = new List<Survivor>();
        private readonly Dictionary<string, Dosimeter> _dosimeters = new Dictionary<string, Dosimeter>();

        /// <summary>When true, Tick accumulates no dose (game paused).</summary>
        public bool IsPaused { get; set; }

        /// <summary>Fired whenever a survivor's current radiation dose changes.</summary>
        public event Action<Survivor, float> OnDoseChanged;

        private PersonalQuestSystem _personalQuests;
        private System.Func<System.Collections.Generic.IReadOnlyList<Survivor>> _getSurvivors;

        /// <summary>Prompt #229/#234 — Synthesizer rad-away mult + Rad-Walker absorb cap.</summary>
        public void BindPersonalQuests(
            PersonalQuestSystem personalQuests,
            System.Func<System.Collections.Generic.IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
        }

        /// <summary>Fired once when a survivor first gains a radiation-driven status.</summary>
        public event Action<Survivor, SurvivorStatus> OnStatusGained;
        /// <summary>Fired once when a timed radiation status (e.g. rad resistance) expires.</summary>
        public event Action<Survivor, SurvivorStatus> OnStatusLost;

        public RadiationSystem(NeedsSystem needsSystem, Func<Survivor, ExposureContext> exposureContext = null, Random rng = null)
        {
            _needsSystem = needsSystem != null ? needsSystem : throw new ArgumentNullException(nameof(needsSystem));
            _exposureContext = exposureContext;
            _rng = rng ?? new Random();
        }

        /// <summary>Register a survivor so bulk Tick(gameHours) advances their dose.</summary>
        public void Register(Survivor survivor)
        {
            if (survivor != null && !_survivors.Contains(survivor))
            {
                _survivors.Add(survivor);
            }
        }

        /// <summary>Stop advancing a survivor's dose via bulk Tick(gameHours).</summary>
        public void Unregister(Survivor survivor)
        {
            _survivors.Remove(survivor);
        }

        /// <summary>
        /// Advance dose accumulation over elapsed game hours for all registered survivors.
        /// Pause-aware: does nothing while IsPaused. For each survivor it resolves the
        /// exposure context via the injected hook, computes the exposure rate, applies any
        /// active rad resistance, degrades worn gear, applies the dose via Expose, updates
        /// the dosimeter, and advances the rad-resistance timer. With no hook registered,
        /// survivors receive no ambient dose -- call Expose(...) directly for scripted exposure.
        /// </summary>
        public void Tick(float gameHours)
        {
            if (IsPaused || gameHours <= 0f)
            {
                return;
            }

            for (int i = 0; i < _survivors.Count; i++)
            {
                var survivor = _survivors[i];
                if (survivor == null || !survivor.IsAlive)
                {
                    continue;
                }

                var context = _exposureContext != null ? _exposureContext(survivor) : null;
                float zone = context != null ? context.ZoneRadLevel : 0f;
                List<WornGear> worn = context != null ? context.WornGear : null;

                float gearProtection = ComputeGearProtection(worn);
                float exposurePerHour;
                if (context != null && context.ShelterRadQuery != null)
                {
                    float interiorRads = context.ShelterRadQuery(zone);
                    exposurePerHour = Mathf.Max(0f, interiorRads - gearProtection);
                }
                else
                {
                    float shielding = context != null ? context.ShelterShielding : 0f;
                    exposurePerHour = ComputeExposurePerHour(zone, gearProtection, shielding);
                }

                if (survivor.HasRadResistance)
                {
                    exposurePerHour *= RadResistanceFactor;
                }

                DegradeWornGear(worn, gameHours);
                Expose(survivor, exposurePerHour, gameHours);

                var dosimeter = GetDosimeter(survivor.Id);
                dosimeter.Record(exposurePerHour * gameHours, gameHours);
                dosimeter.LifetimeDose = survivor.LifetimeRadiationExposure;

                TickRadResistance(survivor, gameHours);
                PrognosisPipeline.Tick(survivor, gameHours, _needsSystem, _rng, GrantStatus);
            }
        }

        /// <summary>
        /// Total protection from worn gear: sum of each piece's radProtection scaled by
        /// its remaining durability fraction. A suit at zero durability contributes nothing.
        /// </summary>
        public static float ComputeGearProtection(IReadOnlyList<WornGear> worn)
        {
            if (worn == null)
            {
                return 0f;
            }

            float total = 0f;
            for (int i = 0; i < worn.Count; i++)
            {
                var piece = worn[i];
                if (piece != null)
                {
                    total += piece.EffectiveProtection();
                }
            }
            return Mathf.Max(0f, total);
        }

        /// <summary>Effective ambient dose-rate after shelter shielding caps it (floored at 0).</summary>
        public static float ComputeEffectiveAmbient(float zoneRadLevel, float shelterShielding)
        {
            return Mathf.Max(0f, zoneRadLevel - Mathf.Max(0f, shelterShielding));
        }

        /// <summary>
        /// Exposure rate for a survivor: max(0, zoneRadLevel - protectionFromGear - shelterShielding).
        /// Negative results clamp to zero.
        /// </summary>
        public static float ComputeExposurePerHour(float zoneRadLevel, float gearProtection, float shelterShielding)
        {
            return Mathf.Max(0f, zoneRadLevel - Mathf.Max(0f, gearProtection) - Mathf.Max(0f, shelterShielding));
        }

        /// <summary>
        /// Ambient dose-rate that a set of contaminated items adds to a space (e.g. the
        /// bunker) if brought inside. Decontaminate items (DecontaminationStation) or let
        /// them decay to lower this before stowing them.
        /// </summary>
        public static float ComputeContaminationAmbient(IEnumerable<Contamination> contaminations)
        {
            if (contaminations == null)
            {
                return 0f;
            }

            float total = 0f;
            foreach (var contamination in contaminations)
            {
                if (contamination != null)
                {
                    total += contamination.AmbientContribution();
                }
            }
            return Mathf.Max(0f, total);
        }

        /// <summary>Get (creating on demand) the dosimeter read-model for a survivor.</summary>
        public Dosimeter GetDosimeter(string survivorId)
        {
            if (!_dosimeters.TryGetValue(survivorId, out var dosimeter))
            {
                dosimeter = new Dosimeter { SurvivorId = survivorId };
                _dosimeters[survivorId] = dosimeter;
            }
            return dosimeter;
        }

        /// <summary>Expose a survivor to a dose rate for a number of hours.</summary>
        public void Expose(Survivor survivor, float radsPerHour, float hours)
        {
            if (survivor == null || !survivor.IsAlive || hours <= 0f)
            {
                return;
            }

            if (radsPerHour != 0f)
            {
                float absorb = 1f;
                if (_personalQuests != null)
                    absorb = _personalQuests.GetRadiationAbsorbFactor(survivor);
                float delta = radsPerHour * hours * absorb;
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

            if (radsPerHour > 0f)
            {
                PrognosisPipeline.OnExposure(survivor, radsPerHour * hours, _needsSystem, GrantStatus);
            }
        }

        /// <summary>
        /// Medical exam / doctor check: the only way a prognosis onset estimate is
        /// revealed to the player. Not pushed automatically like the dosimeter's rate.
        /// </summary>
        public PrognosisEstimate ExaminePrognosis(Survivor survivor)
        {
            return PrognosisPipeline.Examine(survivor);
        }

        /// <summary>
        /// Administer iodine pills: grants temporary rad resistance (a timed status that
        /// scales the dose rate by RadResistanceFactor while active). Re-dosing tops up
        /// the remaining window rather than stacking it.
        ///
        /// Also (re)starts IodineProtectionTimer, a short window used only by
        /// PrognosisPipeline: if a Prodromal-triggering dose lands while this timer is
        /// still active, its latent-damage severity is mitigated. Once the window lapses
        /// -- or if a dose already landed before iodine was ever taken -- administering
        /// iodine afterwards does nothing for that dose. Timing, not a stat stick.
        /// </summary>
        public void AdministerIodine(Survivor survivor)
        {
            if (survivor == null || !survivor.IsAlive)
            {
                return;
            }
            survivor.RadResistanceHoursRemaining = Mathf.Max(survivor.RadResistanceHoursRemaining, IodineResistanceHours);
            survivor.HasRadResistance = true;
            survivor.IodineProtectionTimer = Mathf.Max(survivor.IodineProtectionTimer, PrognosisPipeline.IodineWindowHours);
            GrantStatus(survivor, SurvivorStatus.RadResistance);
        }

        /// <summary>
        /// Administer anti-rad medication: reduces the survivor's current radiation dose
        /// (clamped at 0). Lifetime exposure is deliberately left intact -- it only ever
        /// grows and drives chronic illness.
        /// </summary>
        public void AdministerAntiRad(Survivor survivor, float radsRemoved)
        {
            if (survivor == null || !survivor.IsAlive || radsRemoved <= 0f)
            {
                return;
            }
            float mult = 1f;
            if (_personalQuests != null)
                mult = _personalQuests.GetRadAwayEfficiencyMultiplier(_getSurvivors?.Invoke());
            survivor.RadiationDose = Mathf.Clamp(survivor.RadiationDose - radsRemoved * mult, 0f, 100f);
            OnDoseChanged?.Invoke(survivor, survivor.RadiationDose);
        }

        private void TickRadResistance(Survivor survivor, float gameHours)
        {
            if (!survivor.HasRadResistance)
            {
                return;
            }
            survivor.RadResistanceHoursRemaining -= gameHours;
            if (survivor.RadResistanceHoursRemaining <= 0f)
            {
                survivor.RadResistanceHoursRemaining = 0f;
                survivor.HasRadResistance = false;
                OnStatusLost?.Invoke(survivor, SurvivorStatus.RadResistance);
            }
        }

        private static void DegradeWornGear(List<WornGear> worn, float gameHours)
        {
            if (worn == null)
            {
                return;
            }
            for (int i = 0; i < worn.Count; i++)
            {
                var piece = worn[i];
                if (piece != null)
                {
                    piece.Degrade(gameHours);
                }
            }
        }

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
            else if (status == SurvivorStatus.RadResistance)
            {
                survivor.HasRadResistance = true;
            }
            else if (status == SurvivorStatus.AcuteRadiationSyndrome)
            {
                survivor.HasAcuteRadiationSyndrome = true;
            }

            OnStatusGained?.Invoke(survivor, status);
        }
    }
}
