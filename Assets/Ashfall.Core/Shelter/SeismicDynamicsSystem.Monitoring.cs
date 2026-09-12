// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Shelter
{
    /// <summary>Authored fault-warning classification (Plan B68.10). The
    /// player receives uncertainty, never a perfect prediction.</summary>
    public enum SeismicWarningStage
    {
        Stable = 0,
        Elevated = 1,
        Swarm = 2,
        Imminent = 3,
        Aftershock = 4
    }

    /// <summary>
    /// Rockburst hazard request (Plan B68.7). Emitted by the seismic layer;
    /// the excavation authority owns site damage, blocked tunnels and
    /// recovery. This payload carries no consequence of its own.
    /// </summary>
    [Serializable]
    public sealed class RockburstRequest
    {
        public int day;
        public string faultId = string.Empty;
        public List<string> sectors = new List<string>();
        public float severity;      // 0..1 normalized
    }

    /// <summary>
    /// Plan B68 — Geological Faultline Seismic Monitoring &amp; Shock Dampening.
    /// Expansion partial of the Plan 56 <see cref="SeismicDynamicsSystem"/>:
    /// P/S two-stage warning window, geophone arrays (earlier detection),
    /// hydraulic dampeners (peak-impulse reduction with integrity wear and
    /// service), derived fault-warning stages, and rockburst handoff.
    ///
    /// Architecture rules honored:
    /// - Structural, excavation and survivor authorities keep ownership of
    ///   all damage; this partial only emits impulses, warnings and requests.
    /// - Geophones and dampeners use existing authored items
    ///   (item_geophone_probe / item_seismic_damper_pad / vibration mounts).
    /// - Warning stages and arrival estimates are derived (recomputed, never
    ///   persisted) per the catalog-truth vs runtime-truth rule.
    /// - Legacy saves normalize to no coverage / no dampeners: with nothing
    ///   installed, all thresholds and damping match Plan 56 exactly.
    /// </summary>
    public sealed partial class SeismicDynamicsSystem
    {
        // Authored detection thresholds (tension ratio 0..1).
        public const float PrimaryWaveRatioBase = 0.60f;
        public const float PrimaryWaveRatioGeophone = 0.45f;
        public const float MainArrivalRatioBase = 0.80f;
        public const float MainArrivalRatioGeophone = 0.65f;

        public const float DampenerMaxDamping = 0.25f;   // peak-impulse reduction at full integrity
        public const float DampenerWearBase = 8f;        // per slip
        public const float DampenerWearSeverityScale = 15f;

        public const string GeophoneItemId = "item_geophone_probe";
        public const string DampenerPadItemId = "item_seismic_damper_pad";
        public const string DampenerMountItemId = "item_vibration_dampening_mount";

        /// <summary>Initial (P-wave) tremor detection — the early-warning window opener.</summary>
        public event Action<string, float>? OnPrimaryWaveDetected; // faultId, tensionRatio

        /// <summary>Rockburst hazard request — consumed by the excavation authority / host.</summary>
        public event Action<RockburstRequest>? OnRockburstRequested;

        private readonly HashSet<string> _primaryWarningsToday = new HashSet<string>(StringComparer.Ordinal);

        // -----------------------------------------------------------------
        // Geophone arrays
        // -----------------------------------------------------------------

        /// <summary>
        /// Install a geophone in a sector. Requires an authored probe item.
        /// Coverage lowers P/S detection thresholds for every fault whose
        /// affected sectors include a covered sector.
        /// </summary>
        public ActionResult InstallGeophone(string sectorId, InventoryContainer? inv = null)
        {
            if (string.IsNullOrEmpty(sectorId))
                return ActionResult.Failed("invalid_sector", "seismic.invalid_sector");
            if (_state.geophoneSectors.Contains(sectorId))
                return ActionResult.Blocked("geophone_already_installed", "seismic.geophone_present");

            var invToUse = inv ?? _inventory;
            if (invToUse != null)
            {
                if (!invToUse.HasSufficient(GeophoneItemId, 1))
                    return ActionResult.Blocked("insufficient_materials", "seismic.insufficient_materials");
                invToUse.TryConsume(GeophoneItemId, 1);
            }

            _state.geophoneSectors.Add(sectorId);
            OnSeismicStateChanged?.Invoke();
            return ActionResult.Success("seismic.geophone_installed",
                new Dictionary<string, double> { { "sectors", _state.geophoneSectors.Count } });
        }

        public bool HasGeophoneCoverage(string faultId)
        {
            if (!_catalog.TryGetValue(faultId, out var def)) return false;
            foreach (var sec in def.affected_sectors)
                if (_state.geophoneSectors.Contains(sec)) return true;
            return false;
        }

        // -----------------------------------------------------------------
        // Hydraulic dampeners
        // -----------------------------------------------------------------

        /// <summary>
        /// Install a hydraulic dampener mount in a sector. Requires an
        /// authored damper pad plus vibration mount. Dampeners reduce the
        /// peak structural impulse of slips hitting that sector and wear on
        /// every slip; service them to restore integrity.
        /// </summary>
        public ActionResult InstallDampener(string sectorId, InventoryContainer? inv = null)
        {
            if (string.IsNullOrEmpty(sectorId))
                return ActionResult.Failed("invalid_sector", "seismic.invalid_sector");
            if (_state.dampenerIntegrity.ContainsKey(sectorId))
                return ActionResult.Blocked("dampener_already_installed", "seismic.dampener_present");

            var invToUse = inv ?? _inventory;
            if (invToUse != null)
            {
                if (!invToUse.HasSufficient(DampenerPadItemId, 1)
                    || !invToUse.HasSufficient(DampenerMountItemId, 1))
                    return ActionResult.Blocked("insufficient_materials", "seismic.insufficient_materials");
                invToUse.TryConsume(DampenerPadItemId, 1);
                invToUse.TryConsume(DampenerMountItemId, 1);
            }

            _state.dampenerIntegrity[sectorId] = 100f;
            OnSeismicStateChanged?.Invoke();
            return ActionResult.Success("seismic.dampener_installed",
                new Dictionary<string, double> { { "integrity", 100f } });
        }

        /// <summary>Service a worn dampener back to full integrity (one pad).</summary>
        public ActionResult ServiceDampener(string sectorId, InventoryContainer? inv = null)
        {
            if (string.IsNullOrEmpty(sectorId) || !_state.dampenerIntegrity.ContainsKey(sectorId))
                return ActionResult.Failed("no_dampener", "seismic.no_dampener");

            float before = _state.dampenerIntegrity[sectorId];
            if (before >= 99f)
                return ActionResult.Blocked("dampener_healthy", "seismic.dampener_healthy");

            var invToUse = inv ?? _inventory;
            if (invToUse != null)
            {
                if (!invToUse.HasSufficient(DampenerPadItemId, 1))
                    return ActionResult.Blocked("insufficient_materials", "seismic.insufficient_materials");
                invToUse.TryConsume(DampenerPadItemId, 1);
            }

            _state.dampenerIntegrity[sectorId] = 100f;
            OnSeismicStateChanged?.Invoke();
            return ActionResult.Success("seismic.dampener_serviced",
                new Dictionary<string, double> { { "before", before }, { "after", 100f } });
        }

        public float GetDampenerIntegrity(string sectorId)
            => _state.dampenerIntegrity.TryGetValue(sectorId, out float v) ? v : 0f;

        /// <summary>Dampener damping contribution for a fault's slip, with wear applied.</summary>
        private float ApplyDampenerDampingAndWear(SeismicFaultDef def, float magnitude)
        {
            if (def.affected_sectors.Count == 0) return 0f;

            float integritySum = 0f;
            int covered = 0;
            foreach (var sec in def.affected_sectors)
            {
                if (_state.dampenerIntegrity.TryGetValue(sec, out float integrity))
                {
                    integritySum += integrity;
                    covered++;
                }
            }
            if (covered == 0) return 0f;

            float avgIntegrity = integritySum / def.affected_sectors.Count;
            float damping = (avgIntegrity / 100f) * DampenerMaxDamping;

            // Wear: every slip degrades installed dampeners, harder at higher magnitude.
            float wear = DampenerWearBase + DampenerWearSeverityScale * ((magnitude - 3.0f) / 3.5f);
            foreach (var sec in def.affected_sectors)
            {
                if (_state.dampenerIntegrity.TryGetValue(sec, out float integrity))
                    _state.dampenerIntegrity[sec] = Math.Max(0f, integrity - wear);
            }

            return damping;
        }

        // -----------------------------------------------------------------
        // P/S two-stage warning window
        // -----------------------------------------------------------------

        private void CheckPrimaryWave(SeismicFaultDef def, FaultRuntimeState fState, float tensionRatio)
        {
            if (!_state.seismographOperational) return;

            float pThreshold = HasGeophoneCoverage(def.fault_id)
                ? PrimaryWaveRatioGeophone : PrimaryWaveRatioBase;

            if (tensionRatio >= pThreshold && tensionRatio < GetMainArrivalRatio(def.fault_id)
                && _primaryWarningsToday.Add(def.fault_id))
            {
                OnPrimaryWaveDetected?.Invoke(def.fault_id, tensionRatio);
            }
        }

        /// <summary>Main-arrival (S) warning threshold — 0.80 base, 0.65 with geophone coverage.</summary>
        public float GetMainArrivalRatio(string faultId)
            => HasGeophoneCoverage(faultId) ? MainArrivalRatioGeophone : MainArrivalRatioBase;

        /// <summary>
        /// Rough lead-time estimate at the current accumulation rate — how
        /// many days until the fault reaches its slip threshold. Derived; the
        /// player receives an uncertain window, not a countdown.
        /// </summary>
        public int EstimateArrivalDays(string faultId)
        {
            if (!_catalog.TryGetValue(faultId, out var def)) return -1;
            var fState = EnsureFaultState(faultId);

            float dailyRate = def.base_accumulation_rate * def.depth_multiplier;
            if (fState.temporaryShoringStrength > 0f)
                dailyRate *= (1f - fState.temporaryShoringStrength);
            if (dailyRate <= 0f) return -1;

            float remaining = def.slip_threshold - fState.currentTension;
            if (remaining <= 0f) return 0;
            return (int)Math.Ceiling(remaining / dailyRate);
        }

        // -----------------------------------------------------------------
        // Derived fault-warning stage (recomputed, never persisted)
        // -----------------------------------------------------------------

        public SeismicWarningStage GetFaultWarningStage(string faultId, int day)
        {
            if (!_catalog.TryGetValue(faultId, out var def)) return SeismicWarningStage.Stable;
            var fState = EnsureFaultState(faultId);

            // A recent slip dominates the readout: the swarm is settling.
            if (fState.totalSlips > 0 && fState.lastSlipDay >= 0 && day - fState.lastSlipDay <= 2)
                return SeismicWarningStage.Aftershock;

            float ratio = fState.currentTension / Math.Max(1f, def.slip_threshold);
            if (ratio >= MainArrivalRatioBase) return SeismicWarningStage.Imminent;
            if (ratio >= PrimaryWaveRatioBase) return SeismicWarningStage.Swarm;
            if (ratio >= 0.40f) return SeismicWarningStage.Elevated;
            return SeismicWarningStage.Stable;
        }
    }
}
