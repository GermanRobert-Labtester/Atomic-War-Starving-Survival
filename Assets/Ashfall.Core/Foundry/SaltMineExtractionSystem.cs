using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Foundry
{
    // ── Mine state ──────────────────────────────────────────────────

    /// <summary>State of one salt mine vein/sector.</summary>
    [Serializable]
    public class SaltMineVeinState
    {
        public string veinId = string.Empty;
        public string displayName = string.Empty;
        public bool isUnlocked = false;
        public float remainingOre = 1000f;     // kg of extractable ore
        public float extractionRate = 10f;     // kg per worker per day
        public int maxWorkers = 4;
        public int assignedWorkers = 0;
        public float drillCondition = 1.0f;    // 0..1
        public float pumpPressure = 1.0f;      // 0..1
        public float contamination = 0f;       // 0..1, respiratory hazard
        public bool isShutdown = false;
        public int lastExtractionDay = -1;
    }

    /// <summary>Treaty delivery record.</summary>
    [Serializable]
    public class TreatyDeliveryRecord
    {
        public string treatyId = string.Empty;
        public string itemId = string.Empty;
        public float quantityDelivered = 0f;
        public int deliveryDay = 0;
        public bool accepted = false;
    }

    /// <summary>System-wide salt mine state (save DTO).</summary>
    [Serializable]
    public class SaltMineState
    {
        public string systemId = SaltMineExtractionSystem.SystemId;
        public List<SaltMineVeinState> veins = new List<SaltMineVeinState>();
        public float saltStorage = 0f;         // kg in storage
        public float brineStorage = 0f;        // barrels in storage
        public float sulfurStorage = 0f;       // kg in storage
        public float powerDraw = 0f;           // units per day
        public bool isPowered = true;
        public List<TreatyDeliveryRecord> deliveries = new List<TreatyDeliveryRecord>();
        public float totalSaltProduced = 0f;
        public float totalBrineProduced = 0f;
        public float totalSulfurProduced = 0f;
    }

    // ── System ──────────────────────────────────────────────────────

    /// <summary>
    /// ASHFALL — Subterranean Salt Mine and Mineral Brine Extraction system.
    /// Produces bounded rock salt, iodized brine, and sulfur by consuming
    /// labor, power/fuel, drill condition, ventilation capacity, and
    /// maintenance resources. Production fulfills District 8/Silent Foundry
    /// obligations only when the required goods are delivered.
    ///
    /// Key invariant: production alone does NOT fulfill a treaty.
    /// Delivery does.
    /// </summary>
    public class SaltMineExtractionSystem
    {
        public const string SystemId = "salt_mine_extraction_system";

        // Production constants
        public const float SaltPerKgOre = 0.6f;       // kg salt per kg ore
        public const float BrinePerKgOre = 0.3f;      // barrels brine per kg ore
        public const float SulfurPerKgOre = 0.05f;    // kg sulfur per kg ore
        public const float DrillWearPerDay = 0.02f;   // condition loss per day
        public const float PumpWearPerDay = 0.01f;    // pressure loss per day
        public const float ContaminationPerWorker = 0.01f; // per worker per day
        public const float ContaminationDecayPerDay = 0.005f;
        public const float PowerDrawPerWorker = 0.5f; // units per worker per day
        public const float MaxContamination = 0.5f;   // above this, shutdown risk

        // Treaty constants
        public const string TreatyBrinePipe = "treaty_brine_pipe_and_iodine_exchange";
        public const float TreatyBrineQuotaBarrels = 20f;  // barrels per delivery
        public const float TreatySaltQuotaKg = 50f;        // kg per delivery

        private readonly SaltMineState _state = new SaltMineState();
        private readonly Dictionary<string, SaltMineVeinState> _veins = new Dictionary<string, SaltMineVeinState>();

        // Events
        public event Action<string> OnMineOpened;            // veinId
        public event Action<string> OnMineClosed;            // veinId
        public event Action<string, float> OnExtractionBatchProduced; // veinId, kg
        public event Action<string> OnOutputContaminated;    // veinId
        public event Action<string, float> OnWorkerExposure; // veinId, contamination
        public event Action<string> OnDrillFailure;          // veinId
        public event Action<string> OnPumpFailure;           // veinId
        public event Action<TreatyDeliveryRecord> OnTreatyDeliveryAccepted;
        public event Action<TreatyDeliveryRecord> OnTreatyDeliveryMissed;
        public event Action<SaltMineState> OnStateChanged;

        public SaltMineState State => _state;
        public IReadOnlyDictionary<string, SaltMineVeinState> Veins => _veins;

        public SaltMineExtractionSystem()
        {
        }

        // ── Vein management ──────────────────────────────────────────

        /// <summary>Register a mine vein.</summary>
        public bool RegisterVein(SaltMineVeinState vein)
        {
            if (vein == null || string.IsNullOrEmpty(vein.veinId)) return false;
            if (_veins.ContainsKey(vein.veinId)) return false;
            _veins[vein.veinId] = vein;
            _state.veins.Add(vein);
            RaiseChanged();
            return true;
        }

        /// <summary>Unlock a vein for extraction.</summary>
        public bool UnlockVein(string veinId)
        {
            if (!_veins.TryGetValue(veinId, out var vein)) return false;
            if (vein.isUnlocked) return false;
            vein.isUnlocked = true;
            OnMineOpened?.Invoke(veinId);
            RaiseChanged();
            return true;
        }

        /// <summary>Assign workers to a vein.</summary>
        public bool AssignWorkers(string veinId, int count)
        {
            if (!_veins.TryGetValue(veinId, out var vein)) return false;
            if (!vein.isUnlocked || vein.isShutdown) return false;
            vein.assignedWorkers = Math.Clamp(count, 0, vein.maxWorkers);
            RaiseChanged();
            return true;
        }

        // ── Daily tick ───────────────────────────────────────────────

        /// <summary>
        /// Advance one day of extraction. Produces salt, brine, and sulfur
        /// based on assigned workers, drill condition, and power state.
        /// </summary>
        public void TickDaily(int day, ISeededRng rng)
        {
            if (!_state.isPowered) return;

            float totalPowerDraw = 0f;
            foreach (var vein in _veins.Values)
            {
                if (!vein.isUnlocked || vein.isShutdown || vein.assignedWorkers <= 0)
                    continue;

                // Drill wear
                vein.drillCondition = Math.Max(0f, vein.drillCondition - DrillWearPerDay);
                if (vein.drillCondition <= 0f)
                {
                    OnDrillFailure?.Invoke(vein.veinId);
                    vein.isShutdown = true;
                    OnMineClosed?.Invoke(vein.veinId);
                    continue;
                }

                // Pump wear
                vein.pumpPressure = Math.Max(0f, vein.pumpPressure - PumpWearPerDay);
                if (vein.pumpPressure <= 0.2f)
                {
                    OnPumpFailure?.Invoke(vein.veinId);
                    vein.isShutdown = true;
                    OnMineClosed?.Invoke(vein.veinId);
                    continue;
                }

                // Calculate extraction
                float effectiveRate = vein.extractionRate
                    * vein.drillCondition
                    * vein.pumpPressure
                    * vein.assignedWorkers;

                float extractedKg = Math.Min(effectiveRate, vein.remainingOre);
                if (extractedKg <= 0f) continue;

                vein.remainingOre -= extractedKg;
                vein.lastExtractionDay = day;

                // Convert to outputs
                float salt = extractedKg * SaltPerKgOre;
                float brine = extractedKg * BrinePerKgOre;
                float sulfur = extractedKg * SulfurPerKgOre;

                // Contamination check
                vein.contamination = Math.Min(1f,
                    vein.contamination + ContaminationPerWorker * vein.assignedWorkers);
                if (vein.contamination > MaxContamination)
                {
                    OnOutputContaminated?.Invoke(vein.veinId);
                    // Contaminated output is reduced
                    salt *= 0.5f;
                    brine *= 0.5f;
                    sulfur *= 0.5f;
                }

                // Worker exposure
                if (vein.contamination > 0.1f)
                    OnWorkerExposure?.Invoke(vein.veinId, vein.contamination);

                // Store outputs
                _state.saltStorage += salt;
                _state.brineStorage += brine;
                _state.sulfurStorage += sulfur;
                _state.totalSaltProduced += salt;
                _state.totalBrineProduced += brine;
                _state.totalSulfurProduced += sulfur;

                // Power draw
                totalPowerDraw += PowerDrawPerWorker * vein.assignedWorkers;

                OnExtractionBatchProduced?.Invoke(vein.veinId, extractedKg);

                // Contamination decay
                vein.contamination = Math.Max(0f, vein.contamination - ContaminationDecayPerDay);
            }

            _state.powerDraw = totalPowerDraw;
            RaiseChanged();
        }

        // ── Treaty delivery ──────────────────────────────────────────

        /// <summary>
        /// Deliver brine/salt to fulfill the treaty quota.
        /// Returns the delivery record. Only delivery fulfills the treaty.
        /// </summary>
        public TreatyDeliveryRecord? DeliverToTreaty(int day)
        {
            float brineAvailable = _state.brineStorage;
            float saltAvailable = _state.saltStorage;

            if (brineAvailable < TreatyBrineQuotaBarrels && saltAvailable < TreatySaltQuotaKg)
            {
                var missed = new TreatyDeliveryRecord
                {
                    treatyId = TreatyBrinePipe,
                    itemId = "item_iodized_brine",
                    quantityDelivered = 0f,
                    deliveryDay = day,
                    accepted = false
                };
                _state.deliveries.Add(missed);
                OnTreatyDeliveryMissed?.Invoke(missed);
                RaiseChanged();
                return missed;
            }

            // Deliver what we can
            float brineDelivered = Math.Min(brineAvailable, TreatyBrineQuotaBarrels);
            float saltDelivered = Math.Min(saltAvailable, TreatySaltQuotaKg);

            _state.brineStorage -= brineDelivered;
            _state.saltStorage -= saltDelivered;

            var record = new TreatyDeliveryRecord
            {
                treatyId = TreatyBrinePipe,
                itemId = "item_iodized_brine",
                quantityDelivered = brineDelivered,
                deliveryDay = day,
                accepted = brineDelivered >= TreatyBrineQuotaBarrels
            };
            _state.deliveries.Add(record);

            if (record.accepted)
                OnTreatyDeliveryAccepted?.Invoke(record);
            else
                OnTreatyDeliveryMissed?.Invoke(record);

            RaiseChanged();
            return record;
        }

        // ── Maintenance ──────────────────────────────────────────────

        /// <summary>Replace drill in a vein.</summary>
        public bool ReplaceDrill(string veinId)
        {
            if (!_veins.TryGetValue(veinId, out var vein)) return false;
            vein.drillCondition = 1.0f;
            if (vein.isShutdown && vein.pumpPressure > 0.2f)
            {
                vein.isShutdown = false;
                OnMineOpened?.Invoke(veinId);
            }
            RaiseChanged();
            return true;
        }

        /// <summary>Repair pump in a vein.</summary>
        public bool RepairPump(string veinId)
        {
            if (!_veins.TryGetValue(veinId, out var vein)) return false;
            vein.pumpPressure = 1.0f;
            if (vein.isShutdown && vein.drillCondition > 0f)
            {
                vein.isShutdown = false;
                OnMineOpened?.Invoke(veinId);
            }
            RaiseChanged();
            return true;
        }

        /// <summary>Toggle power to the mine.</summary>
        public void SetPower(bool powered)
        {
            _state.isPowered = powered;
            RaiseChanged();
        }

        // ── Queries ──────────────────────────────────────────────────

        public SaltMineVeinState? GetVein(string veinId)
        {
            return _veins.TryGetValue(veinId, out var vein) ? vein : null;
        }

        public bool IsTreatyFulfilled(int day)
        {
            for (int i = _state.deliveries.Count - 1; i >= 0; i--)
            {
                if (_state.deliveries[i].deliveryDay <= day && _state.deliveries[i].accepted)
                    return true;
            }
            return false;
        }

        public int GetDeliveryCount()
        {
            int count = 0;
            foreach (var d in _state.deliveries)
                if (d.accepted) count++;
            return count;
        }

        // ── Save / Load ──────────────────────────────────────────────

        public SaltMineState CaptureState()
        {
            var copy = new SaltMineState
            {
                systemId = _state.systemId,
                saltStorage = _state.saltStorage,
                brineStorage = _state.brineStorage,
                sulfurStorage = _state.sulfurStorage,
                powerDraw = _state.powerDraw,
                isPowered = _state.isPowered,
                totalSaltProduced = _state.totalSaltProduced,
                totalBrineProduced = _state.totalBrineProduced,
                totalSulfurProduced = _state.totalSulfurProduced
            };
            // Ordinal-ordered vein copies
            var sorted = new List<SaltMineVeinState>(_state.veins);
            sorted.Sort((a, b) => string.CompareOrdinal(a.veinId, b.veinId));
            foreach (var v in sorted)
            {
                copy.veins.Add(new SaltMineVeinState
                {
                    veinId = v.veinId,
                    displayName = v.displayName,
                    isUnlocked = v.isUnlocked,
                    remainingOre = v.remainingOre,
                    extractionRate = v.extractionRate,
                    maxWorkers = v.maxWorkers,
                    assignedWorkers = v.assignedWorkers,
                    drillCondition = v.drillCondition,
                    pumpPressure = v.pumpPressure,
                    contamination = v.contamination,
                    isShutdown = v.isShutdown,
                    lastExtractionDay = v.lastExtractionDay
                });
            }
            foreach (var d in _state.deliveries)
            {
                copy.deliveries.Add(new TreatyDeliveryRecord
                {
                    treatyId = d.treatyId,
                    itemId = d.itemId,
                    quantityDelivered = d.quantityDelivered,
                    deliveryDay = d.deliveryDay,
                    accepted = d.accepted
                });
            }
            return copy;
        }

        public void RestoreState(SaltMineState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _veins.Clear();
            _state.veins.Clear();
            _state.deliveries.Clear();

            _state.saltStorage = Math.Max(0f, saved.saltStorage);
            _state.brineStorage = Math.Max(0f, saved.brineStorage);
            _state.sulfurStorage = Math.Max(0f, saved.sulfurStorage);
            _state.powerDraw = saved.powerDraw;
            _state.isPowered = saved.isPowered;
            _state.totalSaltProduced = saved.totalSaltProduced;
            _state.totalBrineProduced = saved.totalBrineProduced;
            _state.totalSulfurProduced = saved.totalSulfurProduced;

            if (saved.veins != null)
            {
                foreach (var v in saved.veins)
                {
                    if (v == null || string.IsNullOrEmpty(v.veinId)) continue;
                    var copy = new SaltMineVeinState
                    {
                        veinId = v.veinId,
                        displayName = v.displayName,
                        isUnlocked = v.isUnlocked,
                        remainingOre = Math.Max(0f, v.remainingOre),
                        extractionRate = v.extractionRate,
                        maxWorkers = Math.Max(1, v.maxWorkers),
                        assignedWorkers = Math.Clamp(v.assignedWorkers, 0, v.maxWorkers),
                        drillCondition = Math.Clamp(v.drillCondition, 0f, 1f),
                        pumpPressure = Math.Clamp(v.pumpPressure, 0f, 1f),
                        contamination = Math.Clamp(v.contamination, 0f, 1f),
                        isShutdown = v.isShutdown,
                        lastExtractionDay = v.lastExtractionDay
                    };
                    _veins[copy.veinId] = copy;
                    _state.veins.Add(copy);
                }
            }
            if (saved.deliveries != null)
            {
                foreach (var d in saved.deliveries)
                {
                    if (d == null || string.IsNullOrEmpty(d.treatyId)) continue;
                    _state.deliveries.Add(d);
                }
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
