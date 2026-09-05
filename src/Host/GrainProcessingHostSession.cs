using System;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Inventory;

namespace AtomicWar.GodotApp
{
    /// <summary>Thin Godot adapter for the Core grain-processing authority.</summary>
    public sealed class GrainProcessingHostSession : HostSessionBase
    {
        public GrainProcessingSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public GrainProcessingHostSession(GrainProcessingSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnJobCompleted += job =>
            {
                LastEvent = $"Grain batch completed: {job.output_granted} output units.";
                RaiseStateChanged();
            };
            System.OnJobBlocked += _ =>
            {
                LastEvent = "Grain output blocked: storage is full.";
                RaiseStateChanged();
            };
            System.OnStateChanged += () => { RaiseStateChanged(); };
        }

        public ActionResult StartMilling(string recipeId, string siloId, string workerId = "")
            => System.StartMilling(recipeId, siloId, workerId);

        public ActionResult TreatSilo(string siloId, string treatmentItemId, int quantity, float reduction)
            => System.TreatSilo(siloId, treatmentItemId, quantity, reduction);

        public void TickDay(int day) => System.TickDay(day);

        public override void Save()
        {
            if (!IsDirty) return;
            if (GrainProcessingSaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class GrainProcessingSaveStore
    {
        public const string FileName = "grain_processing_save.json";
        public const string SectionName = "grain_processing";

        private static readonly SaveStore<GrainProcessingState> s_store =
            SaveStoreHub.Checksummed<GrainProcessingState>(FileName, nameof(GrainProcessingSaveStore));

        public static bool TrySave(GrainProcessingState state) => s_store.TrySave(state);
        public static GrainProcessingState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(GrainProcessingState state) => s_store.CapturePersisted(state);
        public static GrainProcessingState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
