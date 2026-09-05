using System;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public sealed class PrecisionOpticsHostSession : HostSessionBase
    {
        public PrecisionOpticsEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public PrecisionOpticsHostSession(PrecisionOpticsEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnWorkpieceStarted += wp =>
            {
                LastEvent = $"Optics grinding started: {wp.displayName}.";
                RaiseStateChanged();
            };
            System.OnStageCompleted += (wp, stage) =>
            {
                LastEvent = $"Optics stage '{stage}' complete for {wp.displayName}. Quality: {wp.accumulatedQuality:P0}.";
                RaiseStateChanged();
            };
            System.OnWorkpieceCompleted += wp =>
            {
                LastEvent = $"Precision optical element ready: {wp.displayName} (Quality: {wp.accumulatedQuality:P0}).";
                RaiseStateChanged();
            };
            System.OnStateChanged += _ => { RaiseStateChanged(); };
        }

        public ActionResult StartWorkpiece(string recipeId)
        {
            var res = System.StartWorkpiece(recipeId);
            if (res.IsFailure) LastEvent = "Starting optics workpiece blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public ActionResult AdvanceWork(float workUnits, float workerSkillModifier = 1.0f)
        {
            var res = System.AdvanceWork(workUnits, workerSkillModifier);
            if (res.IsFailure) LastEvent = "Advancing optics work blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public ActionResult TestFigure()
        {
            var res = System.TestFigureWithFoucault();
            if (res.IsFailure) LastEvent = "Figure testing blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public ActionResult CompleteOptic(string? outputItemId = null)
        {
            var res = System.CompleteOptic(outputItemId);
            if (res.IsFailure) LastEvent = "Finalizing optic blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public override void Save()
        {
            if (!IsDirty) return;
            PrecisionOpticsSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
