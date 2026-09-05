using System;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Medical;
using Ashfall.Core.Radio;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>Thin Godot adapter for abstract advanced-material production.</summary>
    public sealed class PowderMetallurgyHostSession : HostSessionBase
    {
        public PowderMetallurgySystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public PowderMetallurgyHostSession(PowderMetallurgySystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnBatchCompleted += batch =>
            {
                LastEvent = $"Material batch completed: {batch.output_item_id}.";
                RaiseStateChanged();
            };
            System.OnStateChanged += _ => RaiseStateChanged();
        }

        public ActionResult StartBatch(string processId, int day)
            => System.StartBatch(processId, day);

        public ActionResult TickDay(int day)
            => System.TickDay(day);

        public override void Save()
        {
            if (!IsDirty) return;
            if (PowderMetallurgySaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class PowderMetallurgySaveStore
    {
        public const string FileName = "powder_metallurgy_save.json";
        public const string SectionName = "powder_metallurgy";

        private static readonly SaveStore<PowderMetallurgyState> s_store =
            SaveStoreHub.Checksummed<PowderMetallurgyState>(FileName, nameof(PowderMetallurgySaveStore));

        public static bool TrySave(PowderMetallurgyState state) => s_store.TrySave(state);
        public static PowderMetallurgyState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(PowderMetallurgyState state) => s_store.CapturePersisted(state);
        public static PowderMetallurgyState? TryRestore(string json) => s_store.RestoreBare(json);
    }

    /// <summary>Thin Godot adapter for regional NVIS communications.</summary>
    public sealed class NvisCommunicationsHostSession : HostSessionBase
    {
        public NvisCommunicationsSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public NvisCommunicationsHostSession(NvisCommunicationsSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnTransmissionCompleted += record =>
            {
                LastEvent = record.delivered
                    ? "Regional status transmission delivered."
                    : "Regional status transmission lost in the noise.";
                RaiseStateChanged();
            };
            System.OnRecallRequested += request =>
            {
                LastEvent = $"Recall request queued for {request.survivor_id}.";
                RaiseStateChanged();
            };
            System.OnStateChanged += () => RaiseStateChanged();
        }

        public ActionResult SetPowered(bool powered)
            => System.SetPowered(powered);

        public ActionResult SelectChannel(string channelId)
            => System.SelectChannel(channelId);

        public ActionResult BeginStatusTransmission(string payload, int day, int activeExpeditionCount)
            => System.BeginStatusTransmission(payload, day, activeExpeditionCount);

        public ActionResult RequestRecall(string survivorId, int day)
            => System.RequestRecall(survivorId, day);

        public void TickDay(int day) => System.TickDay(day);

        public override void Save()
        {
            if (!IsDirty) return;
            if (NvisCommunicationsSaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class NvisCommunicationsSaveStore
    {
        public const string FileName = "nvis_communications_save.json";
        public const string SectionName = "nvis_communications";

        private static readonly SaveStore<NvisCommunicationsState> s_store =
            SaveStoreHub.Checksummed<NvisCommunicationsState>(FileName, nameof(NvisCommunicationsSaveStore));

        public static bool TrySave(NvisCommunicationsState state) => s_store.TrySave(state);
        public static NvisCommunicationsState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(NvisCommunicationsState state) => s_store.CapturePersisted(state);
        public static NvisCommunicationsState? TryRestore(string json) => s_store.RestoreBare(json);
    }

    /// <summary>Thin Godot adapter for the preserved-biologic ledger.</summary>
    public sealed class LyophilizationHostSession : HostSessionBase
    {
        public LyophilizationSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public LyophilizationHostSession(LyophilizationSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnBatchCompleted += batch =>
            {
                LastEvent = $"Preserved biologic batch completed: {batch.batch_id}.";
                RaiseStateChanged();
            };
            System.OnStateChanged += () => RaiseStateChanged();
        }

        public ActionResult StartBatch(string recipeId, int day)
            => System.StartBatch(recipeId, day);

        public ActionResult TickDay(int day)
            => System.TickDay(day);

        public override void Save()
        {
            if (!IsDirty) return;
            if (LyophilizationSaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class LyophilizationSaveStore
    {
        public const string FileName = "lyophilization_save.json";
        public const string SectionName = "lyophilization";

        private static readonly SaveStore<LyophilizationState> s_store =
            SaveStoreHub.Checksummed<LyophilizationState>(FileName, nameof(LyophilizationSaveStore));

        public static bool TrySave(LyophilizationState state) => s_store.TrySave(state);
        public static LyophilizationState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(LyophilizationState state) => s_store.CapturePersisted(state);
        public static LyophilizationState? TryRestore(string json) => s_store.RestoreBare(json);
    }

    /// <summary>Thin Godot adapter for canonical armored draisine recovery.</summary>
    public sealed class DraisineRerailingHostSession : HostSessionBase
    {
        public DraisineRerailingSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public DraisineRerailingHostSession(DraisineRerailingSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnRecoveryCompleted += state =>
            {
                LastEvent = $"Draisine {state.train_id} returned to the rail.";
                RaiseStateChanged();
            };
            System.OnStateChanged += () => RaiseStateChanged();
        }

        public ActionResult StartRecovery(string trainId, string equipmentId, int day)
            => System.StartRecovery(trainId, equipmentId, day);

        public ActionResult TickDay(int day)
            => System.TickDay(day);

        public ActionResult Abandon() => System.Abandon();

        public override void Save()
        {
            if (!IsDirty) return;
            if (DraisineRerailingSaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class DraisineRerailingSaveStore
    {
        public const string FileName = "draisine_recovery_save.json";
        public const string SectionName = "draisine_recovery";

        private static readonly SaveStore<DraisineRecoveryState> s_store =
            SaveStoreHub.Checksummed<DraisineRecoveryState>(FileName, nameof(DraisineRerailingSaveStore));

        public static bool TrySave(DraisineRecoveryState state) => s_store.TrySave(state);
        public static DraisineRecoveryState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(DraisineRecoveryState state) => s_store.CapturePersisted(state);
        public static DraisineRecoveryState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
