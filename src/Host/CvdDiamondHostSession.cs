// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : CvdDiamondHostSession
// Core Source  : Ashfall.Core.Shelter.CvdDiamondSynthesisEngine
// Purpose      : Thin Godot adapter — batch commands + typed consumer-wear
//                seam for workshop/excavation owners. No math, no truth.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public sealed class CvdDiamondHostSession : HostSessionBase
    {
        public CvdDiamondSynthesisEngine System { get; }

        /// <summary>Deterministic presentation batch label minted by the engine.</summary>
        public string NextBatchId() => System.NextBatchId();
        public string LastEvent { get; private set; } = string.Empty;

        /// <summary>Feedstock availability from the canonical inventory owner.</summary>
        public Func<string, bool>? FeedstockAvailableProvider { get; set; }
        /// <summary>Substrate item availability from the canonical inventory owner.</summary>
        public Func<string, bool>? SubstrateItemAvailableProvider { get; set; }
        /// <summary>Operator skill 0..100 from the canonical survivor owners.</summary>
        public Func<float>? OperatorSkillProvider { get; set; }
        /// <summary>Power/cooling readiness from the canonical power grid owner.</summary>
        public Func<bool>? PowerAvailableProvider { get; set; }
        public Func<bool>? CoolingAvailableProvider { get; set; }
        /// <summary>Maintenance/repair item availability.</summary>
        public Func<bool>? RepairPartsAvailableProvider { get; set; }

        public CvdDiamondHostSession(CvdDiamondSynthesisEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public string InstallReactor(string profileId, bool partsAvailable)
        {
            var result = System.Install(profileId, partsAvailable);
            RaiseStateChanged();
            LastEvent = result.IsSuccess ? $"CVD reactor installed ({profileId})." : $"Cannot install reactor ({result.FailureCode}).";
            return LastEvent;
        }

        public string StartBatch(string batchId, string componentId, string feedProfileId, string substrateProfileId)
        {
            var result = System.StartBatch(batchId, componentId, feedProfileId, substrateProfileId,
                PowerAvailableProvider?.Invoke() ?? true,
                CoolingAvailableProvider?.Invoke() ?? true,
                FeedstockAvailableProvider?.Invoke(feedProfileId) ?? true,
                SubstrateItemAvailableProvider?.Invoke(substrateProfileId) ?? true,
                OperatorSkillProvider?.Invoke() ?? 0f);
            RaiseStateChanged();
            LastEvent = result.IsSuccess ? $"Growth batch started ({batchId})." : $"Cannot start batch ({result.FailureCode}).";
            return LastEvent;
        }

        public string AdvanceBatch()
        {
            var result = System.AdvanceBatch();
            RaiseStateChanged();
            LastEvent = result.IsSuccess ? "Growth batch advanced." : $"Batch step ({result.FailureCode}).";
            return LastEvent;
        }

        public string CertifyBatch(string batchId, bool metrologyPassed)
        {
            var result = System.CertifyBatch(batchId, metrologyPassed);
            RaiseStateChanged();
            LastEvent = result.IsSuccess ? $"Batch certified ({batchId})." : $"Certification ({result.FailureCode}).";
            return LastEvent;
        }

        public DiamondToolResult? ConsumeOutput(string batchId)
        {
            var result = System.ConsumeOutput(batchId);
            RaiseStateChanged();
            LastEvent = result is { IsSuccess: true }
                ? $"Tool component produced ({result.Value!.ComponentId}, grade {result.Value.GradeId})."
                : $"Consume failed ({result?.FailureCode}).";
            return result?.Value;
        }

        public string PerformMaintenance()
        {
            var result = System.PerformMaintenance(RepairPartsAvailableProvider?.Invoke() ?? true);
            RaiseStateChanged();
            LastEvent = result.IsSuccess ? "CVD chamber maintained." : $"Cannot maintain chamber ({result.FailureCode}).";
            return LastEvent;
        }

        /// <summary>Registers a typed high-wear consumer via the engine's registry.</summary>
        public void RegisterConsumerThroughSession()
        {
            System.RegisterConsumer("consumer_deep_excavation_cutter");
            System.RegisterConsumer("consumer_precision_lathe_insert");
        }

        /// <summary>
        /// Typed wear-benefit lookup for high-wear consumers (deep excavation
        /// cutter, precision lathe). Returns false with baseline wear for any
        /// unregistered consumer — no global durability buff.
        /// </summary>
        public bool TryGetConsumerWear(string consumerId, string componentId, string gradeId, out int wearFactorBp)
            => System.TryGetWearFactor(consumerId, componentId, gradeId, out wearFactorBp);
    
        public CvdDiamondReactorState CaptureSave() => System.CaptureState();

        public void RestoreSave(CvdDiamondReactorState? save)
        {
            if (save == null) return;
            System.RestoreState(save);
            LastEvent = "State restored from save.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            CvdDiamondSaveStore.TrySave(CaptureSave());
        }
}
}
