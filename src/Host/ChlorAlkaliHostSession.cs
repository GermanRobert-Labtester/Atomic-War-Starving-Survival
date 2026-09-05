using System;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public sealed class ChlorAlkaliHostSession : HostSessionBase
    {
        public ChlorAlkaliSynthesisEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public ChlorAlkaliHostSession(ChlorAlkaliSynthesisEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnBatchCompleted += (procId, bleach, caustic) =>
            {
                LastEvent = $"Chlor-alkali batch complete ({procId}): +{bleach} bleach, +{caustic} caustic soda.";
                RaiseStateChanged();
            };
            System.OnPlantFault += (faultType, msg) =>
            {
                LastEvent = $"Chlor-alkali plant fault [{faultType}]: {msg}";
                RaiseStateChanged();
            };
            System.OnProcessStateChanged += _ => { RaiseStateChanged(); };
        }

        public ActionResult StartProcess(string processId)
        {
            var res = System.StartProcess(processId);
            if (res.IsFailure) LastEvent = "Chlor-alkali process blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public ActionResult ServicePlant()
        {
            var res = System.ServicePlant();
            if (res.IsFailure) LastEvent = "Plant servicing blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public override void Save()
        {
            if (!IsDirty) return;
            ChlorAlkaliSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
