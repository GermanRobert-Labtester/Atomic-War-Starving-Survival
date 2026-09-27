// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : SurgicalGraftSaveStore
// Core State : Ashfall.Core.Medical.SurgicalGraftRejectionEngineSaveState
// Host Caller: Main.SurgicalGraft
// Purpose    : PLAN-SURGICAL-WARD-TRUTH-213 — graft record persistence under
//              its own checksummed save key. The ward bed authority stays with
//              MedicalWardSystem; this owner only persists the graft records
//              the Core rejection engine is authoritative for.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class SurgicalGraftSaveStore
    {
        public const string FileName = "surgical_graft_save.json";
        public const string SectionName = "surgical_graft";

        private static readonly SaveStore<SurgicalGraftRejectionEngineSaveState> s_store =
            SaveStoreHub.Checksummed<SurgicalGraftRejectionEngineSaveState>(FileName, nameof(SurgicalGraftSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static string TryCapturePersisted(SurgicalGraftRejectionEngineSaveState state) => s_store.CaptureBare(state);
        public static SurgicalGraftRejectionEngineSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(SurgicalGraftRejectionEngineSaveState state) => s_store.TrySave(state);
        public static SurgicalGraftRejectionEngineSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Read-only graft projection for the ward surface.</summary>
    public sealed class SurgicalGraftSnapshot
    {
        public int TotalGrafts { get; set; }
        public int Integrating { get; set; }
        public int Integrated { get; set; }
        public int Rejected { get; set; }
        public List<(string GraftId, string SurvivorId, string Limb, string Status, int RiskPermille)> Grafts { get; } = new();
    }

    /// <summary>
    /// PLAN-SURGICAL-WARD-TRUTH-213 host session. Wraps the Core
    /// <see cref="SurgicalGraftRejectionEngine"/>: daily deterministic tick,
    /// immunosuppressant administration, graft placement, save custody.
    /// </summary>
    public sealed class SurgicalGraftHostSession : HostSessionBase
    {
        private readonly SurgicalGraftRejectionEngine _engine = new();

        public SurgicalGraftRejectionEngine Engine => _engine;
        public string LastEvent { get; private set; } = string.Empty;

        public SurgicalGraftHostSession()
        {
            _engine.OnGraftIntegrated += g =>
            {
                LastEvent = $"Graft integrated: {g.GraftId} ({g.SurvivorId}/{g.LimbKey}).";
                RaiseStateChanged();
            };
            _engine.OnGraftRejected += g =>
            {
                LastEvent = $"Graft rejected: {g.GraftId} ({g.SurvivorId}/{g.LimbKey}).";
                RaiseStateChanged();
            };
            _engine.OnRejectionRiskIncreased += (g, risk) =>
            {
                LastEvent = $"Rejection risk raised for {g.GraftId} ({risk} permille).";
                RaiseStateChanged();
            };
        }

        public SurgicalGraftRecord? RecordGraft(string survivorId, string limbKey, string donorSourceId,
            GraftBiocompatibilityTier tier, int currentDay)
        {
            if (string.IsNullOrWhiteSpace(survivorId) || string.IsNullOrWhiteSpace(limbKey)) return null;
            var record = _engine.PerformGraft(survivorId, limbKey, donorSourceId ?? string.Empty, tier, currentDay);
            LastEvent = $"Graft placed: {record.GraftId} ({tier}).";
            RaiseStateChanged();
            return record;
        }

        public bool AdministerImmunosuppressant(string graftId, int dosePermille, int currentDay)
        {
            bool ok = _engine.AdministerImmunosuppressant(graftId, dosePermille, currentDay);
            if (ok)
            {
                LastEvent = $"Immunosuppressant administered to {graftId} (+{dosePermille} permille).";
                RaiseStateChanged();
            }
            return ok;
        }

        /// <summary>
        /// Deterministic daily tick. The rejection roll is supplied by the
        /// caller (campaign forked RNG); the engine never seeds from wall clock.
        /// </summary>
        public void TickDay(int currentDay, Func<string, int, int>? seededRngRoll = null)
        {
            _engine.ProcessDailyTick(currentDay, seededRngRoll);
            RaiseStateChanged();
        }

        public SurgicalGraftSnapshot GetSnapshot()
        {
            var snapshot = new SurgicalGraftSnapshot
            {
                TotalGrafts = _engine.Grafts.Count,
                Integrating = _engine.Grafts.Count(g => g.Status == GraftStatus.Integrating),
                Integrated = _engine.Grafts.Count(g => g.Status == GraftStatus.FullyIntegrated),
                Rejected = _engine.Grafts.Count(g => g.Status == GraftStatus.Rejected)
            };
            foreach (var g in _engine.Grafts.TakeLast(8))
            {
                snapshot.Grafts.Add((g.GraftId, g.SurvivorId, g.LimbKey, g.Status.ToString(), g.RejectionRiskPermille));
            }
            return snapshot;
        }

        public SurgicalGraftRejectionEngineSaveState CaptureState() => _engine.CaptureState();

        public void RestoreState(SurgicalGraftRejectionEngineSaveState? state)
        {
            _engine.RestoreState(state);
            LastEvent = "Surgical graft state restored.";
            RaiseStateChanged();
        }
    }
}
