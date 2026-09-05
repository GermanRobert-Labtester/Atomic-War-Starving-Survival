// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class RouteInfrastructureSaveStore
    {
        public const string FileName = "route_infrastructure_save.json";
        public const string SectionName = "route_infrastructure";

        private static readonly SaveStore<RouteInfrastructureState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(RouteInfrastructureSaveStore),
            Encode,
            Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapture(RouteInfrastructureState state) => s_store.CaptureBare(state);
        public static RouteInfrastructureState? TryRestore(string json) => s_store.RestoreBare(json);
        public static bool TrySave(RouteInfrastructureState state) => s_store.TrySave(state);
        public static RouteInfrastructureState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(RouteInfrastructureState state) => s_store.CapturePersisted(state);

        private static string Encode(RouteInfrastructureState state, IJsonSerializer json) => json.Serialize(state);
        private static RouteInfrastructureState? Decode(string raw, IJsonSerializer json) => json.Deserialize<RouteInfrastructureState>(raw);
    }
}
