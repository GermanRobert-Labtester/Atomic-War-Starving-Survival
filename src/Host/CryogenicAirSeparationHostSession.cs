using System;
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>Thin Godot adapter for abstract cryogenic gas production.</summary>
    public sealed class CryogenicAirSeparationHostSession : HostSessionBase
    {
        public CryogenicAirSeparationSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public CryogenicAirSeparationHostSession(CryogenicAirSeparationSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnCycleCompleted += () =>
            {
                LastEvent = "Air-separation cycle completed.";
                RaiseStateChanged();
            };
            System.OnFailure += reason =>
            {
                LastEvent = $"Air-separation cycle blocked: {reason}.";
                RaiseStateChanged();
            };
            System.OnStateChanged += () => { RaiseStateChanged(); };
        }

        public bool SetRunning(bool running) => System.SetRunning(running);
        public bool Repair(float plantIntegrity, float filterCondition)
            => System.Repair(plantIntegrity, filterCondition);
        public void TickDay(int day) => System.TickDay(day);

        public override void Save()
        {
            if (!IsDirty) return;
            if (CryogenicAirSeparationSaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class CryogenicAirSeparationSaveStore
    {
        public const string FileName = "cryogenic_air_separation_save.json";
        public const string SectionName = "cryogenic_air_separation";

        private static readonly SaveStore<CryogenicAirSeparationState> s_store =
            SaveStoreHub.Checksummed<CryogenicAirSeparationState>(
                FileName, nameof(CryogenicAirSeparationSaveStore));

        public static bool TrySave(CryogenicAirSeparationState state) => s_store.TrySave(state);
        public static CryogenicAirSeparationState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(CryogenicAirSeparationState state) => s_store.CapturePersisted(state);
        public static CryogenicAirSeparationState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
