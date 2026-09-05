using System;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot adapter for the Core chemical recon authority (Plan 81).
    /// Presentation (ChemicalReconPanel) is a Wave 6 google-stitch deliverable —
    /// see the "Missing UI panels" registry in AGENTS.md. UNKNOWN HAZARD /
    /// FILTER BREAKTHROUGH / CRITICAL EXPOSURE must render as text labels,
    /// never color-only semantics.
    /// </summary>
    public sealed class ChemicalReconHostSession : HostSessionBase
    {
        public ChemicalReconEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public ChemicalReconHostSession(ChemicalReconEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnHazardIdentified += obs =>
            {
                LastEvent = $"Hazard {obs.discoveryState}: {obs.hazardId} at {obs.locationNodeId}.";
                RaiseStateChanged();
            };
            System.OnSampleCollected += s =>
            {
                LastEvent = $"Sample sealed: {s.sampleId} (quality {s.quality:F2}).";
                RaiseStateChanged();
            };
            System.OnSafeCorridorDiscovered += corridorId =>
            {
                LastEvent = $"Safe corridor mapped: {corridorId}.";
                RaiseStateChanged();
            };
            System.OnFilterBreakthrough += locationId =>
            {
                LastEvent = $"FILTER BREAKTHROUGH risk at {locationId}.";
                RaiseStateChanged();
            };
            System.OnReconChanged += () => { RaiseStateChanged(); };
        }

        public ChemicalDetectionResult Scan(string locationNodeId, string detectorBand, float surveyorSkill = 0.5f)
        {
            var result = System.ScanLocation(locationNodeId, detectorBand, surveyorSkill);
            LastEvent = result.Detected
                ? $"Detector: {result.HazardClass} ({result.SafeExposureBand}) — filter {result.RecommendedFilterCategory}."
                : "Detector: no hazard above threshold.";
            RaiseStateChanged();
            return result;
        }

        public ActionResult CollectSample(string hazardId, string locationNodeId, float surveyorSkill = 0.5f, Func<string, int, bool>? consumeItem = null)
        {
            var res = System.CollectSample(hazardId, locationNodeId, surveyorSkill, consumeItem);
            if (res.IsFailure) LastEvent = "Sample collection blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public void RechargeBattery()
        {
            System.RechargeBattery();
            LastEvent = "Detector battery recharged.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            ChemicalReconSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
