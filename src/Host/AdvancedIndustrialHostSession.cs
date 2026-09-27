// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : AdvancedIndustrialHostSession
// Core Engines : FischerTropschSynthesisEngine (Plan 118),
//                UvCoronaDetectionEngine (Plan 119),
//                CarbonCompositeEngine (Plan 120),
//                GroundPenetratingRadarEngine (Plan 121)
// Host Caller  : Main.AdvancedIndustrial
// Purpose      : Composition root that gives the four signed Plans 118-121
//                engines a live, persisted, player-driven production surface.
//                The engines remain the calculation authorities; this session
//                owns their construction, catalog binding, canonical-inventory
//                binding, daily progression, and save/restore.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public sealed class AdvancedIndustrialHostSession : HostSessionBase
    {
        private readonly Inventory _inventory;

        public FischerTropschSynthesisEngine Synthesis { get; }
        public CarbonCompositeEngine Composites { get; }
        public UvCoronaDetectionEngine Uv { get; }
        public GroundPenetratingRadarEngine Gpr { get; }

        public int Day { get; private set; }
        public string LastEvent { get; private set; } = string.Empty;

        private AdvancedIndustrialHostSession(
            FischerTropschSynthesisEngine synthesis,
            CarbonCompositeEngine composites,
            UvCoronaDetectionEngine uv,
            GroundPenetratingRadarEngine gpr,
            Inventory inventory)
        {
            Synthesis = synthesis;
            Composites = composites;
            Uv = uv;
            Gpr = gpr;
            _inventory = inventory;
        }

        public static AdvancedIndustrialHostSession Create(
            string dataDirectory,
            Inventory inventory,
            AdvancedIndustrialSaveState? saved = null)
        {
            var synthesis = new FischerTropschSynthesisEngine(new SeededRng(118), FischerTropschCatalogLoader.Load(dataDirectory));
            var composites = new CarbonCompositeEngine(new SeededRng(120), CarbonCompositeCatalogLoader.Load(dataDirectory));
            var uv = new UvCoronaDetectionEngine(new SeededRng(119), UvCoronaDetectionCatalogLoader.Load(dataDirectory));
            var gpr = new GroundPenetratingRadarEngine(new SeededRng(121), GroundPenetratingRadarCatalogLoader.Load(dataDirectory));

            synthesis.BindInventory(inventory);
            composites.BindInventory(inventory);
            uv.BindInventory(inventory);
            gpr.BindInventory(inventory);

            var session = new AdvancedIndustrialHostSession(synthesis, composites, uv, gpr, inventory);
            if (saved != null) session.Restore(saved);
            return session;
        }

        public AdvancedIndustrialSaveState Capture() => new AdvancedIndustrialSaveState
        {
            day = Day,
            synthesis = Synthesis.CaptureState(),
            composites = Composites.CaptureState(),
            uv = Uv.CaptureState(),
            gpr = Gpr.CaptureState()
        };

        public void Restore(AdvancedIndustrialSaveState state)
        {
            if (state == null) return;
            Day = state.day;
            Synthesis.RestoreState(state.synthesis);
            Composites.RestoreState(state.composites);
            Uv.RestoreState(state.uv);
            Gpr.RestoreState(state.gpr);
        }

        // ── Plan 118 — synthetic lubricant ─────────────────────────────
        public ActionResult StartSyntheticLubricantBatch(string reactorProfileId = "ft_reactor_mk1", float operatorSkill = 0.5f, float feedQuality = 0.95f)
        {
            var result = Synthesis.StartBatch(reactorProfileId, operatorSkill, feedQuality);
            LastEvent = result.IsSuccess ? "ft.batch_started" : result.FailureCode;
            RaiseStateChanged();
            return result;
        }

        public ActionResult ClaimSyntheticLubricantOutputs()
        {
            var result = Synthesis.ClaimOutputs();
            LastEvent = result.IsSuccess ? "ft.outputs_claimed" : result.FailureCode;
            RaiseStateChanged();
            return result;
        }

        public ActionResult ServiceLubricantConsumer(string consumerId) => Synthesis.ServiceLubricantConsumer(consumerId, Day);

        public void RegisterLubricantConsumer(MechanicalLubricantConsumer consumer) => Synthesis.RegisterLubricantConsumer(consumer);

        public float GetWearMultiplier(string consumerId) => Synthesis.GetWearMultiplier(consumerId);

        // ── Plan 120 — carbon composites ──────────────────────────────
        public ActionResult StartCarbonCompositeJob(string componentId, string materialProfileId, int materialAgeDays = 0, float operatorSkill = 0.5f)
        {
            var result = Composites.StartJob(componentId, materialProfileId, materialAgeDays, operatorSkill);
            LastEvent = result.IsSuccess ? "composite.job_active" : result.FailureCode;
            RaiseStateChanged();
            return result;
        }

        public ActionResult ClaimCarbonCompositeOutput()
        {
            var result = Composites.ClaimOutput();
            LastEvent = result.IsSuccess ? "composite.claimed" : result.FailureCode;
            RaiseStateChanged();
            return result;
        }

        // ── Plan 119 — UV corona detection ────────────────────────────
        public UvScanResult ScanUvCorona(IReadOnlyList<ElectricalFaultInput> faults, string weather = "clear", float operatorSkill = 0.6f)
        {
            var result = Uv.Scan(faults, weather, operatorSkill, Day);
            LastEvent = result.Success ? "uv.scan" : "uv.scan_failed";
            RaiseStateChanged();
            return result;
        }

        // ── Plan 121 — ground-penetrating radar ───────────────────────
        public ActionResult BeginGprSurvey(string targetId, string anomalyProfileId, string terrainId, string modeId)
        {
            var result = Gpr.BeginSurvey(targetId, anomalyProfileId, terrainId, modeId, Day);
            LastEvent = result.IsSuccess ? "gpr.survey_active" : result.FailureCode;
            RaiseStateChanged();
            return result;
        }

        public bool TryCreateGprLead(string targetId, out BuriedAnomalyLead? lead) => Gpr.TryCreateLead(targetId, out lead);

        public SubsurfaceObservation? FindGprObservation(string targetId) => Gpr.FindObservation(targetId);

        /// <summary>
        /// Advances only already-started work with bounded ambient conditions.
        /// The player starts batches, jobs, scans, and surveys through commands;
        /// the day tick never fabricates new work or consumes inventory.
        /// </summary>
        public void AdvanceDay(int day)
        {
            Day = day;
            if (Synthesis.HasActiveBatch) Synthesis.Tick(0.6f, 0.6f, 1f);
            if (Composites.State.active_job != null) Composites.Tick(0.70f, 0.65f, 1f, 1f);
            if (Gpr.State.active_survey != null) Gpr.Tick();
            Uv.AdvanceDay();
            RaiseStateChanged();
        }

        public AdvancedIndustrialCensus GetCensus() => new AdvancedIndustrialCensus
        {
            Day = Day,
            SynthesisActive = Synthesis.HasActiveBatch,
            SynthesisBuffer = Synthesis.State.output_buffer.Count,
            CatalystCondition = Synthesis.CatalystCondition,
            CompositeActive = Composites.State.active_job != null,
            CompositeBuffer = Composites.State.output_buffer.Count,
            UvObservations = Uv.State.observations.Count,
            GprObservations = Gpr.State.observations.Count,
            GprLeads = Gpr.State.leads.Count
        };

        /// <summary>Read-only composition-root projection for hosts/probes.</summary>
        public Inventory Inventory => _inventory;
    }

    public struct AdvancedIndustrialCensus
    {
        public int Day;
        public bool SynthesisActive;
        public int SynthesisBuffer;
        public float CatalystCondition;
        public bool CompositeActive;
        public int CompositeBuffer;
        public int UvObservations;
        public int GprObservations;
        public int GprLeads;
    }
}
