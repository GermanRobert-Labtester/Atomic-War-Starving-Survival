// ============================================================================
// Save Store : CounterIntelligenceSaveStore
// Core State : Ashfall.Core.Factions.CounterIntelligenceState
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Factions;

namespace AtomicWar.GodotApp
{
    public static class CounterIntelligenceSaveStore
    {
        public const string FileName = "counter_intelligence_save.json";
        public const string SectionName = "counter_intelligence";

        private static readonly SaveStore<CounterIntelligenceState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(CounterIntelligenceSaveStore),
            Encode,
            Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapture(CounterIntelligenceState state) => s_store.CaptureBare(state);
        public static CounterIntelligenceState? TryRestore(string json) => s_store.RestoreBare(json);
        public static bool TrySave(CounterIntelligenceState state) => s_store.TrySave(state);
        public static CounterIntelligenceState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(CounterIntelligenceState state) => s_store.CapturePersisted(state);

        private static string Encode(CounterIntelligenceState state, IJsonSerializer json) => json.Serialize(state);
        private static CounterIntelligenceState? Decode(string raw, IJsonSerializer json) => json.Deserialize<CounterIntelligenceState>(raw);
    }
}
