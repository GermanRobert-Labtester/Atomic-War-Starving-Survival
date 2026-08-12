using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Expansion II — Hydrostatic Pressure &amp; The Black Aquifer.
    /// Replaces the binary "water generation" of the DeepWellProject with
    /// a fluid dynamics simulation. Clean water floats on top of the denser
    /// Black Aquifer sludge. The player must manage the PumpRate: pump too
    /// aggressively and the ToxicityIndex spikes, destroying purifier
    /// filters and poisoning survivors.
    ///
    /// The Lens Mechanic:
    ///   - CleanLensDepth: metres of potable water above the sludge line.
    ///   - SludgePressure: upward pressure from the Black Aquifer (kPa).
    ///   - PumpRate: player-set extraction speed (L/hour).
    ///   - If PumpRate exceeds the lens recharge rate, sludge is drawn
    ///     into the purifier, degrading FilterHealth 300% faster and
    ///     applying Affliction_ChemicalToxicity to consumers.
    ///
    /// Save/load safe. Plain C#. No MonoBehaviour.
    /// </summary>
    [Serializable]
    public class HydrostaticPressureSave
    {
        public string systemId = "hydrostatic_pressure";
        public float cleanLensDepth = 2.5f;
        public float sludgePressureKpa = 40f;
        public float pumpRateLitersPerHour = 3f;
        public float toxicityIndex;
        public float filterDamageAccumulated;
        public float totalWaterExtracted;
        public float totalSludgeDrawn;
        public bool bulkheadSealed;
        public int daysSinceLastSludgeSpike;
    }

    public struct HydrostaticThresholdEvent
    {
        public float ToxicityIndex;
        public float SludgePressureKpa;
        public bool IsSludgeBreakthrough;
        public bool IsFilterDamage;
    }

    public struct HydrostaticPumpChangedEvent
    {
        public float OldRate;
        public float NewRate;
        public float CleanLensDepth;
    }

    public class HydrostaticPressureSystem
    {
        /// <summary>Maximum clean water lens depth in metres.</summary>
        public const float MaxLensDepth = 5f;

        /// <summary>Lens recharge rate from natural percolation (L/hour).</summary>
        public const float LensRechargeRatePerHour = 3f;

        /// <summary>Sludge pressure increase per hour from deep aquifer (kPa).</summary>
        public const float SludgePressureGrowthPerHour = 0.8f;

        /// <summary>Pressure relieved per litre pumped below recharge rate.</summary>
        public const float PressureReliefPerLiter = 0.5f;

        /// <summary>Toxicity threshold that triggers filter damage.</summary>
        public const float ToxicityFilterDamageThreshold = 0.3f;

        /// <summary>Toxicity threshold that poisons consumers.</summary>
        public const float ToxicityPoisonThreshold = 0.6f;

        /// <summary>Filter damage multiplier when toxicity exceeds threshold.</summary>
        public const float FilterDamageMultiplier = 3f;

        /// <summary>Sludge drawn per hour when pump exceeds recharge (litres).</summary>
        public const float SludgeDrawRatePerHour = 2f;

        /// <summary>Toxicity added per litre of sludge drawn.</summary>
        public const float ToxicityPerSludgeLiter = 0.05f;

        /// <summary>Natural toxicity decay per hour (settling).</summary>
        public const float ToxicityDecayPerHour = 0.01f;

        /// <summary>Days between natural sludge pressure spikes.</summary>
        public const int SludgeSpikeIntervalDays = 7;

        // ── Events ────────────────────────────────────────────────────
        public event Action<HydrostaticThresholdEvent> OnThresholdCrossed;
        public event Action<HydrostaticPumpChangedEvent> OnPumpRateChanged;
        public event Action OnSludgeBreakthrough;
        public event Action<float> OnToxicityChanged;

        // ── State ─────────────────────────────────────────────────────
        private float _cleanLensDepth = 2.5f;
        private float _sludgePressureKpa = 40f;
        private float _pumpRateLitersPerHour = 3f;
        private float _toxicityIndex;
        private float _filterDamageAccumulated;
        private float _totalWaterExtracted;
        private float _totalSludgeDrawn;
        private bool _bulkheadSealed;
        private int _daysSinceLastSludgeSpike;
        private bool _sludgeBreakthroughTriggered;
        private bool _filterDamageTriggered;

        public float CleanLensDepth => _cleanLensDepth;
        public float SludgePressureKpa => _sludgePressureKpa;
        public float PumpRateLitersPerHour => _pumpRateLitersPerHour;
        public float ToxicityIndex => _toxicityIndex;
        public float FilterDamageAccumulated => _filterDamageAccumulated;
        public float TotalWaterExtracted => _totalWaterExtracted;
        public float TotalSludgeDrawn => _totalSludgeDrawn;
        public bool BulkheadSealed => _bulkheadSealed;
        public bool IsSludgeBreakthrough => _toxicityIndex >= ToxicityPoisonThreshold;
        public bool IsFilterDamageActive => _toxicityIndex >= ToxicityFilterDamageThreshold;

        /// <summary>
        /// Whether the current pump rate exceeds the safe recharge rate,
        /// drawing sludge into the water supply.
        /// </summary>
        public bool IsOverPumping => _pumpRateLitersPerHour > LensRechargeRatePerHour;

        /// <summary>
        /// Safe pump rate ceiling — the maximum rate that does not draw sludge.
        /// </summary>
        public float SafePumpRateCeiling => LensRechargeRatePerHour;

        // ── Tick ──────────────────────────────────────────────────────
        /// <summary>
        /// Called every game-hour. Advances the fluid dynamics simulation:
        /// lens depletion/recharge, sludge pressure, toxicity accumulation.
        /// </summary>
        public void Tick(float gameHours)
        {
            if (gameHours <= 0f) return;

            // Phase 1: Pump extraction
            float litersPumped = _pumpRateLitersPerHour * gameHours;
            float lensDepleted = litersPumped * 0.01f; // 100L per metre of lens
            _cleanLensDepth = Mathf.Max(0f, _cleanLensDepth - lensDepleted);
            _totalWaterExtracted += litersPumped;

            // Phase 2: Lens recharge from natural percolation
            float recharge = LensRechargeRatePerHour * gameHours * 0.01f;
            _cleanLensDepth = Mathf.Min(MaxLensDepth, _cleanLensDepth + recharge);

            // Phase 3: Sludge pressure dynamics
            if (_bulkheadSealed)
            {
                // Sealed bulkhead: pressure slowly builds behind the seal
                _sludgePressureKpa += SludgePressureGrowthPerHour * gameHours * 0.5f;
            }
            else
            {
                _sludgePressureKpa += SludgePressureGrowthPerHour * gameHours;
            }

            // Pumping below recharge relieves pressure
            if (_pumpRateLitersPerHour < LensRechargeRatePerHour)
            {
                float relief = PressureReliefPerLiter *
                    (LensRechargeRatePerHour - _pumpRateLitersPerHour) * gameHours;
                _sludgePressureKpa = Mathf.Max(10f, _sludgePressureKpa - relief);
            }

            // Phase 4: Sludge draw when over-pumping
            if (IsOverPumping && !_bulkheadSealed)
            {
                float excessRate = _pumpRateLitersPerHour - LensRechargeRatePerHour;
                float sludgeDrawn = SludgeDrawRatePerHour * (excessRate / LensRechargeRatePerHour) * gameHours;
                _totalSludgeDrawn += sludgeDrawn;
                _toxicityIndex = Mathf.Clamp01(_toxicityIndex + ToxicityPerSludgeLiter * sludgeDrawn);
            }

            // Phase 5: Natural toxicity decay (settling)
            _toxicityIndex = Mathf.Max(0f, _toxicityIndex - ToxicityDecayPerHour * gameHours);

            // Phase 6: Filter damage accumulation
            if (_toxicityIndex >= ToxicityFilterDamageThreshold)
            {
                _filterDamageAccumulated += FilterDamageMultiplier * gameHours;
            }

            OnToxicityChanged?.Invoke(_toxicityIndex);
            CheckThresholds();
        }

        /// <summary>
        /// Daily tick — handles periodic sludge pressure spikes and
        /// the sludge spike interval counter.
        /// </summary>
        public void TickDaily(int currentDay)
        {
            _daysSinceLastSludgeSpike++;

            // Periodic pressure spike from deep-earth fracturing
            if (_daysSinceLastSludgeSpike >= SludgeSpikeIntervalDays)
            {
                _sludgePressureKpa += 15f;
                _daysSinceLastSludgeSpike = 0;
            }

            // High pressure forces sludge through micro-fractures
            if (_sludgePressureKpa > 80f && !_bulkheadSealed)
            {
                _toxicityIndex = Mathf.Clamp01(_toxicityIndex + 0.1f);
            }
        }

        private void CheckThresholds()
        {
            bool newBreakthrough = _toxicityIndex >= ToxicityPoisonThreshold && !_sludgeBreakthroughTriggered;
            bool newFilterDamage = _toxicityIndex >= ToxicityFilterDamageThreshold && !_filterDamageTriggered;

            if (newBreakthrough || newFilterDamage)
            {
                _sludgeBreakthroughTriggered = _sludgeBreakthroughTriggered || newBreakthrough;
                _filterDamageTriggered = _filterDamageTriggered || newFilterDamage;

                OnThresholdCrossed?.Invoke(new HydrostaticThresholdEvent
                {
                    ToxicityIndex = _toxicityIndex,
                    SludgePressureKpa = _sludgePressureKpa,
                    IsSludgeBreakthrough = newBreakthrough,
                    IsFilterDamage = newFilterDamage
                });

                if (newBreakthrough)
                    OnSludgeBreakthrough?.Invoke();
            }

            // Reset triggers when toxicity drops below hysteresis
            if (_toxicityIndex < ToxicityPoisonThreshold * 0.5f) _sludgeBreakthroughTriggered = false;
            if (_toxicityIndex < ToxicityFilterDamageThreshold * 0.5f) _filterDamageTriggered = false;
        }

        // ── Actions ───────────────────────────────────────────────────

        /// <summary>
        /// Set the pump rate. Returns false if rate is negative.
        /// Higher rates extract more water but risk drawing sludge.
        /// </summary>
        public bool SetPumpRate(float litersPerHour)
        {
            if (litersPerHour < 0f) return false;
            float old = _pumpRateLitersPerHour;
            _pumpRateLitersPerHour = Mathf.Clamp(litersPerHour, 0f, 20f);

            OnPumpRateChanged?.Invoke(new HydrostaticPumpChangedEvent
            {
                OldRate = old,
                NewRate = _pumpRateLitersPerHour,
                CleanLensDepth = _cleanLensDepth
            });
            return true;
        }

        /// <summary>
        /// Seal the bulkhead — prevents sludge from entering the clean lens
        /// but stops water production entirely. Used in "The Black Vein"
        /// questline choice.
        /// </summary>
        public void SealBulkhead()
        {
            _bulkheadSealed = true;
            _pumpRateLitersPerHour = 0f;
        }

        /// <summary>Unseal the bulkhead to resume pumping.</summary>
        public void UnsealBulkhead()
        {
            _bulkheadSealed = false;
        }

        /// <summary>
        /// Install an RO membrane to strip chemical toxicity from the
        /// water supply. Reduces toxicity index by 50%.
        /// </summary>
        public bool InstallROMembrane()
        {
            _toxicityIndex *= 0.5f;
            OnToxicityChanged?.Invoke(_toxicityIndex);
            return true;
        }

        /// <summary>
        /// Vent the Black Aquifer into surface ash-swamps. Reduces sludge
        /// pressure by 30 kPa but poisons the local biome permanently.
        /// Used in "The Black Vein" questline choice.
        /// </summary>
        public void VentSludgeToSurface()
        {
            _sludgePressureKpa = Mathf.Max(10f, _sludgePressureKpa - 30f);
            _toxicityIndex = Mathf.Max(0f, _toxicityIndex - 0.3f);
            OnToxicityChanged?.Invoke(_toxicityIndex);
        }

        /// <summary>
        /// Apply filter damage to an external purifier module.
        /// Returns the accumulated damage and resets the counter.
        /// </summary>
        public float ConsumeFilterDamage()
        {
            float damage = _filterDamageAccumulated;
            _filterDamageAccumulated = 0f;
            return damage;
        }

        // ── Save / Load ────────────────────────────────────────────────
        public HydrostaticPressureSave CaptureState()
        {
            return new HydrostaticPressureSave
            {
                cleanLensDepth = _cleanLensDepth,
                sludgePressureKpa = _sludgePressureKpa,
                pumpRateLitersPerHour = _pumpRateLitersPerHour,
                toxicityIndex = _toxicityIndex,
                filterDamageAccumulated = _filterDamageAccumulated,
                totalWaterExtracted = _totalWaterExtracted,
                totalSludgeDrawn = _totalSludgeDrawn,
                bulkheadSealed = _bulkheadSealed,
                daysSinceLastSludgeSpike = _daysSinceLastSludgeSpike
            };
        }

        public void RestoreState(HydrostaticPressureSave save)
        {
            if (save == null)
            {
                _cleanLensDepth = 2.5f;
                _sludgePressureKpa = 40f;
                _pumpRateLitersPerHour = LensRechargeRatePerHour;
                _toxicityIndex = 0f;
                _filterDamageAccumulated = 0f;
                _totalWaterExtracted = 0f;
                _totalSludgeDrawn = 0f;
                _bulkheadSealed = false;
                _daysSinceLastSludgeSpike = 0;
                _sludgeBreakthroughTriggered = false;
                _filterDamageTriggered = false;
                return;
            }

            _cleanLensDepth = save.cleanLensDepth;
            _sludgePressureKpa = save.sludgePressureKpa;
            _pumpRateLitersPerHour = save.pumpRateLitersPerHour;
            _toxicityIndex = save.toxicityIndex;
            _filterDamageAccumulated = save.filterDamageAccumulated;
            _totalWaterExtracted = save.totalWaterExtracted;
            _totalSludgeDrawn = save.totalSludgeDrawn;
            _bulkheadSealed = save.bulkheadSealed;
            _daysSinceLastSludgeSpike = save.daysSinceLastSludgeSpike;
        }
    }
}
