// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plans 118-121 Advanced Industrial / Reconnaissance
// The signed Core engines (Fischer-Tropsch synthesis, UV corona detection,
// carbon composites, ground-penetrating radar) become a live, persisted,
// player-driven production surface under the AdvancedIndustrialHostSession
// composition root. No parallel authority: each engine keeps its own concern.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private AdvancedIndustrialHostSession? _advancedIndustrial;
        private bool _advancedIndustrialDirty;

        public AdvancedIndustrialHostSession? AdvancedIndustrial => _advancedIndustrial;

        public void SetupAdvancedIndustrial()
        {
            if (_advancedIndustrial != null) return;

            var saved = AdvancedIndustrialSaveStore.TryLoad();
            var inventory = _inventory?.Inventory ?? new Inventory();
            _advancedIndustrial = AdvancedIndustrialHostSession.Create(_dataDir, inventory, saved);

            // Plan 118 — the mechanical driveline's lubricant consumer is the
            // canonical in-game beneficiary of synthetic lubricant servicing.
            _advancedIndustrial.RegisterLubricantConsumer(new MechanicalLubricantConsumer
            {
                consumer_id = "mechanical_driveline",
                accepted_grades = new List<string> { "synthetic" },
                wear_multiplier = 0.75f
            });

            _advancedIndustrial.StateChanged += () => _advancedIndustrialDirty = true;
        }

        // ── Plan 118 — synthetic lubricant ─────────────────────────────
        public ActionResult StartSyntheticLubricantBatch(string reactorProfileId = "ft_reactor_mk1", float operatorSkill = 0.5f, float feedQuality = 0.95f)
        {
            SetupAdvancedIndustrial();
            return _advancedIndustrial!.StartSyntheticLubricantBatch(reactorProfileId, operatorSkill, feedQuality);
        }

        public ActionResult ClaimSyntheticLubricantOutputs()
        {
            SetupAdvancedIndustrial();
            return _advancedIndustrial!.ClaimSyntheticLubricantOutputs();
        }

        public ActionResult ServiceLubricantConsumer(string consumerId)
        {
            SetupAdvancedIndustrial();
            return _advancedIndustrial!.ServiceLubricantConsumer(consumerId);
        }

        // ── Plan 119 — UV corona detection ────────────────────────────
        public UvScanResult ScanUvCorona(IReadOnlyList<ElectricalFaultInput> faults, string weather = "clear", float operatorSkill = 0.6f)
        {
            SetupAdvancedIndustrial();
            return _advancedIndustrial!.ScanUvCorona(faults, weather, operatorSkill);
        }

        // ── Plan 120 — carbon composites ──────────────────────────────
        public ActionResult StartCarbonCompositeJob(string componentId, string materialProfileId, int materialAgeDays = 0, float operatorSkill = 0.5f)
        {
            SetupAdvancedIndustrial();
            return _advancedIndustrial!.StartCarbonCompositeJob(componentId, materialProfileId, materialAgeDays, operatorSkill);
        }

        public ActionResult ClaimCarbonCompositeOutput()
        {
            SetupAdvancedIndustrial();
            return _advancedIndustrial!.ClaimCarbonCompositeOutput();
        }

        // ── Plan 121 — ground-penetrating radar ───────────────────────
        public ActionResult BeginGprSurvey(string targetId, string anomalyProfileId, string terrainId, string modeId)
        {
            SetupAdvancedIndustrial();
            return _advancedIndustrial!.BeginGprSurvey(targetId, anomalyProfileId, terrainId, modeId);
        }

        public bool TryCreateGprLead(string targetId, out BuriedAnomalyLead? lead)
        {
            SetupAdvancedIndustrial();
            return _advancedIndustrial!.TryCreateGprLead(targetId, out lead);
        }

        public void AdvanceAdvancedIndustrialDay(int dayNumber)
        {
            SetupAdvancedIndustrial();
            _advancedIndustrial!.AdvanceDay(dayNumber);
        }

        public AdvancedIndustrialCensus GetAdvancedIndustrialCensus() =>
            _advancedIndustrial?.GetCensus() ?? default;

        public void SaveAdvancedIndustrial()
        {
            if (_advancedIndustrial == null) return;
            var state = _advancedIndustrial.Capture();
            AdvancedIndustrialSaveStore.TrySave(state);
            if (CaptureSection(
                    AdvancedIndustrialSaveStore.SectionName,
                    AdvancedIndustrialSaveStore.TryCapturePersisted(state)))
            {
                _advancedIndustrialDirty = false;
            }
        }

        public void FlushAdvancedIndustrialIfDirty()
        {
            if (_advancedIndustrialDirty)
            {
                SaveAdvancedIndustrial();
            }
        }

        public void ResetAdvancedIndustrial()
        {
            _advancedIndustrial = null;
            _advancedIndustrialDirty = false;
        }
    }
}
