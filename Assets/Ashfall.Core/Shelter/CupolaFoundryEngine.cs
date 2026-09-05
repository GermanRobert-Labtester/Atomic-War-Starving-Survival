// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Shelter
{
    public enum CupolaHeatBand { Cold, Heating, MeltReady, Overheated }
    public enum CupolaBatchPhase { Idle, Melting, ReadyToPour }
    public enum CupolaFailureState { None, Stalled, Chilled, RefractoryFailure, HazardEvent }
    public enum CastingDefect { Clean, Porous, Misrun, Cracked, Scrap }

    /// <summary>
    /// Persisted cupola furnace condition. Normalized gameplay bands only —
    /// no real furnace operating measurements. Plans 90-93.
    /// </summary>
    [Serializable]
    public sealed class CupolaFurnaceState
    {
        public string furnace_id = "cupola_01";
        public string active_charge_id = string.Empty;
        public string active_mold_id = string.Empty;
        public int batch_phase = (int)CupolaBatchPhase.Idle;
        public int heat_band = (int)CupolaHeatBand.Cold;
        public int melt_progress;
        public int melt_ticks_required;
        public int stalled_ticks;
        public float molten_pool_units;
        public float slag_level;              // 0..100 normalized
        public float refractory_integrity = 100f; // 0..100 normalized
        public bool blower_available = true;
        public int failure_state = (int)CupolaFailureState.None;
        public string assigned_worker_id = string.Empty;
        public int last_tick_day;

        public CupolaFurnaceState Clone() => new CupolaFurnaceState
        {
            furnace_id = furnace_id,
            active_charge_id = active_charge_id,
            active_mold_id = active_mold_id,
            batch_phase = batch_phase,
            heat_band = heat_band,
            melt_progress = melt_progress,
            melt_ticks_required = melt_ticks_required,
            stalled_ticks = stalled_ticks,
            molten_pool_units = molten_pool_units,
            slag_level = slag_level,
            refractory_integrity = refractory_integrity,
            blower_available = blower_available,
            failure_state = failure_state,
            assigned_worker_id = assigned_worker_id,
            last_tick_day = last_tick_day
        };
    }

    /// <summary>Casting outcome record for UI/telemetry. Output grant itself goes through inventory.</summary>
    [Serializable]
    public sealed class CupolaCastResult
    {
        public string charge_id = string.Empty;
        public string mold_id = string.Empty;
        public string defect = nameof(CastingDefect.Clean);
        public float quality_score;
        public int granted_quantity;
        public bool batch_lost;
    }

    [Serializable]
    public sealed class CupolaFoundrySave
    {
        public int state_version = 1;
        public CupolaFurnaceState furnace = new CupolaFurnaceState();
        public int batches_completed;
        public int lifetime_castings;
        public int last_tick_day;
    }

    /// <summary>
    /// Continuous-melt cupola engine (Plan 90). A sibling of the batch-heat
    /// Silent Foundry: this engine owns only furnace condition — refractory
    /// wear, slag burden, blower gating, melt progress, and cast defects.
    /// Inventory owns all material quantities; metal economy authorities own
    /// downstream consumption. All stochastic outcomes use the injected
    /// ISeededRng with a fixed draw order (hazard roll per tick, then defect
    /// roll at tap). RestoreState never produces, wears, or rolls.
    /// </summary>
    public sealed class CupolaFoundryEngine
    {
        public const string SystemId = "cupola_foundry";
        public const string TraitFoundryMaster = "trait_foundry_master";
        public const string TraitPatternmaker = "trait_patternmaker";
        private const float MinRefractoryToFire = 25f;
        private const float MaxSlagBeforeTap = 90f;

        private readonly Inventory.Inventory _inventory;
        private readonly CupolaFoundryCatalog _catalog;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly Func<string, IReadOnlyList<string>>? _traitsOf;
        private readonly Func<float>? _availablePowerWatts;

        private CupolaFurnaceState _furnace = new CupolaFurnaceState();
        private int _batchesCompleted;
        private int _lifetimeCastings;

        public CupolaFoundryEngine(
            Inventory.Inventory inventory,
            CupolaFoundryCatalog catalog,
            ISeededRng rng,
            ILog? log = null,
            Func<string, IReadOnlyList<string>>? traitsOf = null,
            Func<float>? availablePowerWatts = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
            _traitsOf = traitsOf;
            _availablePowerWatts = availablePowerWatts;
        }

        public CupolaFurnaceState Furnace => _furnace;
        public CupolaFoundryCatalog Catalog => _catalog;
        public int BatchesCompleted => _batchesCompleted;
        public int LifetimeCastings => _lifetimeCastings;

        public event Action<string, string>? OnBatchStarted;      // chargeId, moldId
        public event Action? OnMeltReady;
        public event Action<CupolaCastResult>? OnCastCompleted;
        public event Action<string>? OnBatchAborted;              // reason
        public event Action<string>? OnHazardEvent;               // hazard id
        public event Action? OnStateChanged;

        private CupolaBatchPhase Phase => (CupolaBatchPhase)_furnace.batch_phase;
        private CupolaFailureState Failure => (CupolaFailureState)_furnace.failure_state;

        private bool WorkerHasTrait(string workerId, string traitId)
        {
            if (_traitsOf == null || string.IsNullOrEmpty(workerId)) return false;
            var traits = _traitsOf(workerId);
            return traits != null && traits.Contains(traitId);
        }

        private float AvailableBlowerPower(float requiredWatts)
        {
            if (_availablePowerWatts == null) return requiredWatts; // headless default: power assumed
            return _availablePowerWatts();
        }

        /// <summary>
        /// Plan 90.4 — charge the cupola. Builds the authoritative bill
        /// (feedstock + fuel + flux) and commits it atomically; on any
        /// validation failure nothing is consumed and no RNG is drawn.
        /// </summary>
        public bool TryStartFoundryBatch(string chargeId, string moldId, string workerId = "")
        {
            if (Phase != CupolaBatchPhase.Idle) return false;
            if (Failure == CupolaFailureState.RefractoryFailure) return false;
            if (_furnace.refractory_integrity < MinRefractoryToFire) return false;
            if (_furnace.slag_level >= MaxSlagBeforeTap) return false;

            var charge = _catalog.GetCharge(chargeId);
            if (charge == null) return false;
            var mold = _catalog.GetMold(moldId);
            if (mold == null) return false;
            if (!charge.allowed_mold_ids.Contains(moldId, StringComparer.OrdinalIgnoreCase)) return false;

            float blowerNow = AvailableBlowerPower(charge.required_blower_power_w);
            if (blowerNow < charge.required_blower_power_w) return false;

            var bill = new InventoryBill();
            bill.AddCost(charge.feedstock_item_id, charge.feedstock_quantity);
            bill.AddCost(charge.fuel_item_id, charge.fuel_quantity);
            bill.AddCost(charge.flux_item_id, charge.flux_quantity);

            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                _furnace.active_charge_id = charge.id;
                _furnace.active_mold_id = mold.id;
                _furnace.batch_phase = (int)CupolaBatchPhase.Melting;
                _furnace.heat_band = (int)CupolaHeatBand.Heating;
                _furnace.melt_progress = 0;
                _furnace.melt_ticks_required = charge.melt_ticks;
                _furnace.stalled_ticks = 0;
                _furnace.molten_pool_units = charge.feedstock_quantity;
                _furnace.failure_state = (int)CupolaFailureState.None;
                _furnace.assigned_worker_id = workerId ?? string.Empty;
                OnBatchStarted?.Invoke(charge.id, mold.id);
                OnStateChanged?.Invoke();
            });

            return committed;
        }

        /// <summary>
        /// Plan 90.5/90.6 — advance the melt one day. Power loss stalls the
        /// melt (no progress, rising quality risk). High slag and worn
        /// refractory raise hazard; a hazard can chill the batch or damage
        /// the lining. Fixed RNG order: one hazard roll per tick when eligible.
        /// </summary>
        public void TickDay(int currentDay)
        {
            _furnace.last_tick_day = currentDay;
            if (Phase != CupolaBatchPhase.Melting) { OnStateChanged?.Invoke(); return; }

            var charge = _catalog.GetCharge(_furnace.active_charge_id);
            if (charge == null)
            {
                AbortBatch("unknown_charge");
                return;
            }

            float powerNow = AvailableBlowerPower(charge.required_blower_power_w);
            bool powered = powerNow >= charge.required_blower_power_w;
            _furnace.blower_available = powered;

            if (!powered)
            {
                _furnace.stalled_ticks++;
                _furnace.heat_band = (int)CupolaHeatBand.Cold;
                _furnace.failure_state = (int)CupolaFailureState.Stalled;
                _furnace.slag_level = Math.Min(100f, _furnace.slag_level + charge.slag_load);
                OnStateChanged?.Invoke();
                return;
            }

            _furnace.failure_state = (int)CupolaFailureState.None;
            _furnace.heat_band = _furnace.melt_progress + 1 >= _furnace.melt_ticks_required
                ? (int)CupolaHeatBand.MeltReady
                : (int)CupolaHeatBand.Heating;

            // Hazard roll (single seeded draw, fixed order) — slag and a worn
            // lining both raise the odds; traits do not negate them. Charges
            // with a zero hazard class never roll.
            float refractoryRisk = (100f - _furnace.refractory_integrity) / 200f;
            float slagRisk = _furnace.slag_level / 300f;
            double hazardOdds = charge.hazard_rating + refractoryRisk + slagRisk;
            if (charge.hazard_rating > 0f && _rng.NextDouble() < hazardOdds)
            {
                bool chilled = _rng.NextDouble() < 0.5; // second fixed draw: chill vs lining damage
                if (chilled)
                {
                    _furnace.failure_state = (int)CupolaFailureState.Chilled;
                    AbortBatch("chilled_melt");
                    return;
                }
                _furnace.refractory_integrity = Math.Max(0f, _furnace.refractory_integrity - 8f);
                OnHazardEvent?.Invoke("refractory_damage");
            }

            _furnace.melt_progress++;
            _furnace.slag_level = Math.Min(100f, _furnace.slag_level + charge.slag_load);
            _furnace.refractory_integrity = Math.Max(0f, _furnace.refractory_integrity - charge.refractory_wear_per_batch * 0.5f);

            if (_furnace.melt_progress >= _furnace.melt_ticks_required)
            {
                _furnace.batch_phase = (int)CupolaBatchPhase.ReadyToPour;
                _furnace.heat_band = (int)CupolaHeatBand.MeltReady;
                OnMeltReady?.Invoke();
            }

            OnStateChanged?.Invoke();
        }

        /// <summary>
        /// Plan 90.7/90.8 — tap the ready melt into the charged mold. One
        /// seeded defect roll; output grant is atomic. Wear and slag apply
        /// only on a successful tap.
        /// </summary>
        public CupolaCastResult? TryTapMold()
        {
            if (Phase != CupolaBatchPhase.ReadyToPour) return null;

            var charge = _catalog.GetCharge(_furnace.active_charge_id);
            var mold = _catalog.GetMold(_furnace.active_mold_id);
            if (charge == null || mold == null)
            {
                AbortBatch("unknown_charge");
                return null;
            }

            float qualityScore = ComputeQualityScore(charge, mold);

            // Defect roll — the one seeded draw at tap (fixed order).
            double defectRoll = _rng.NextDouble() * 100.0;
            CastingDefect defect;
            if (defectRoll < qualityScore - 20f) defect = CastingDefect.Clean;
            else if (defectRoll < qualityScore) defect = CastingDefect.Porous;
            else if (defectRoll < qualityScore + 15f) defect = CastingDefect.Misrun;
            else if (defectRoll < qualityScore + 30f) defect = CastingDefect.Cracked;
            else defect = CastingDefect.Scrap;

            float multiplier = defect switch
            {
                CastingDefect.Clean => 1f,
                CastingDefect.Porous => 0.75f,
                CastingDefect.Misrun => 0.5f,
                CastingDefect.Cracked => 0.25f,
                _ => 0f
            };
            int granted = (int)MathF.Floor(mold.output_quantity * multiplier);

            var result = new CupolaCastResult
            {
                charge_id = charge.id,
                mold_id = mold.id,
                defect = defect.ToString(),
                quality_score = qualityScore,
                granted_quantity = granted
            };

            bool committed = true;
            if (granted > 0)
            {
                var bill = new InventoryBill();
                bill.AddGrant(mold.output_item_id, granted);
                committed = _inventory.TryExecuteTransaction(bill, () => { });
            }
            if (!committed)
            {
                _log.Warn($"[CupolaFoundry] Output grant failed for {mold.output_item_id}; cast held as {defect}.");
                result.batch_lost = true;
                OnCastCompleted?.Invoke(result);
                OnStateChanged?.Invoke();
                return result;
            }

            // Wear & slag settle only after a completed tap.
            var maintenance = _catalog.Maintenance;
            float wearPerBatch = charge.refractory_wear_per_batch + mold.wear_per_cast;
            _furnace.refractory_integrity = Math.Max(0f, _furnace.refractory_integrity - wearPerBatch);
            _furnace.slag_level = Math.Min(100f, _furnace.slag_level + charge.slag_load * 0.5f);

            if (_furnace.refractory_integrity < MinRefractoryToFire)
            {
                _furnace.failure_state = (int)CupolaFailureState.RefractoryFailure;
                OnHazardEvent?.Invoke("refractory_failure");
            }

            ClearBatch();
            _batchesCompleted++;
            _lifetimeCastings += granted;

            OnCastCompleted?.Invoke(result);
            OnStateChanged?.Invoke();
            return result;
        }

        /// <summary>Abort the active melt without refund — deliberate loss, consumes nothing further.</summary>
        public bool AbortBatch(string reason)
        {
            if (Phase == CupolaBatchPhase.Idle) return false;
            ClearBatch();
            OnBatchAborted?.Invoke(reason);
            OnStateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Plan 90.6 — reline the cupola (and optionally descale with the
        /// pickling reagent from the chemical line: foundry × chemical via
        /// item flow only). Atomic billing; no-op when it cannot be paid.
        /// </summary>
        public bool TryServiceCupola(bool includeDescale, string workerId = "")
        {
            var maintenance = _catalog.Maintenance;
            if (maintenance == null) return false;
            if (_furnace.refractory_integrity >= 99.9f && !includeDescale) return false;
            if (_furnace.slag_level <= 0.01f && !includeDescale) return false;

            var bill = new InventoryBill();
            bool wantsReline = _furnace.refractory_integrity < 99.9f;
            if (wantsReline)
                bill.AddCost(maintenance.refractory_item_id, maintenance.refractory_quantity);
            bool wantsDescale = includeDescale
                && !string.IsNullOrEmpty(maintenance.descale_item_id)
                && _furnace.slag_level > 0.01f;
            if (wantsDescale)
                bill.AddCost(maintenance.descale_item_id, maintenance.descale_quantity);

            if (bill.Costs.Count == 0) return false;

            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                if (wantsReline)
                {
                    _furnace.refractory_integrity = Math.Min(100f, _furnace.refractory_integrity + maintenance.refractory_restore);
                    if (_furnace.failure_state == (int)CupolaFailureState.RefractoryFailure && _furnace.refractory_integrity >= MinRefractoryToFire)
                        _furnace.failure_state = (int)CupolaFailureState.None;
                }
                float slagCut = (wantsDescale ? maintenance.descale_slag_reduction : 0f)
                                + (wantsReline ? maintenance.slag_reduction : 0f);
                _furnace.slag_level = Math.Max(0f, _furnace.slag_level - slagCut);
                OnStateChanged?.Invoke();
            });

            return committed;
        }

        private float ComputeQualityScore(CupolaChargeDefinition charge, FoundryMoldProfile mold)
        {
            float score = mold.quality_target;
            score += (_furnace.refractory_integrity - 70f) * 0.2f;
            score -= _furnace.slag_level * 0.15f;
            score -= _furnace.stalled_ticks * 5f;
            if (WorkerHasTrait(_furnace.assigned_worker_id, TraitFoundryMaster)) score += 8f;
            if (WorkerHasTrait(_furnace.assigned_worker_id, TraitPatternmaker)) score += 5f;
            return Math.Clamp(score, 0f, 100f);
        }

        private void ClearBatch()
        {
            _furnace.active_charge_id = string.Empty;
            _furnace.active_mold_id = string.Empty;
            _furnace.batch_phase = (int)CupolaBatchPhase.Idle;
            _furnace.melt_progress = 0;
            _furnace.melt_ticks_required = 0;
            _furnace.stalled_ticks = 0;
            _furnace.molten_pool_units = 0f;
            _furnace.heat_band = (int)CupolaHeatBand.Cold;
        }

        // ── Persistence (Plan 90.12) — restore is non-operative ─────────

        public CupolaFoundrySave CaptureState() => new CupolaFoundrySave
        {
            state_version = 1,
            furnace = _furnace.Clone(),
            batches_completed = _batchesCompleted,
            lifetime_castings = _lifetimeCastings,
            last_tick_day = _furnace.last_tick_day
        };

        public void RestoreState(CupolaFoundrySave? save)
        {
            if (save == null) return;
            _furnace = (save.furnace ?? new CupolaFurnaceState()).Clone();
            _batchesCompleted = save.batches_completed;
            _lifetimeCastings = save.lifetime_castings;
            // Deliberately no production, wear, rolls, or inventory movement here.
        }
    }
}
