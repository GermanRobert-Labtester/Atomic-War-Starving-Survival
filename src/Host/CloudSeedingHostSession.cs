// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : CloudSeedingHostSession
// Purpose      : PLAN-WEATHER-ATMOSPHERE-28 (cloud-seeding package) — wire the
//                authored cloud-seeding instrument to the canonical weather
//                owner. Persists its own install/cooldown state under a
//                checksummed `cloud_seeding` section.
// ============================================================================
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class CloudSeedingSaveStore
    {
        public const string FileName = "cloud_seeding_save.json";
        public const string SectionName = "cloud_seeding";
        private static readonly SaveStore<CloudSeedingSaveState> s_store =
            SaveStoreHub.Checksummed<CloudSeedingSaveState>(FileName, nameof(CloudSeedingSaveStore));
        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static string TryCapturePersisted(CloudSeedingSaveState state) => s_store.CaptureBare(state);
        public static CloudSeedingSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(CloudSeedingSaveState state) => s_store.TrySave(state);
        public static CloudSeedingSaveState? TryLoad() => s_store.TryLoad();
    }

    public sealed class CloudSeedingHostSession : HostSessionBase
    {
        public CloudSeedingSystem? Engine { get; private set; }
        public string LastEvent { get; private set; } = string.Empty;

        public void Bind(WeatherSystem weather, Inventory? inventory, ISeededRng rng)
        {
            Engine = new CloudSeedingSystem(weather, null, inventory, rng);
            Engine.OnStateChanged += () => RaiseStateChanged();
        }

        public bool IsBound => Engine != null;
        public bool IsInstalled => Engine?.IsInstalled ?? false;
        public int CooldownRemaining => Engine?.CooldownRemaining ?? 0;

        public ActionResult Install(int day)
        {
            if (Engine == null) return ActionResult.Blocked("cloud_seeding.not_bound", "cloud_seeding.not_bound");
            var result = Engine.Install(day);
            LastEvent = result.MessageKey;
            RaiseStateChanged();
            return result;
        }

        public (bool CanDeploy, string? Reason, float SuccessChance, Dictionary<string, int> Cost) Preflight(int day, WeatherKind target, int targetDay = 0)
        {
            if (Engine == null) return (false, "cloud_seeding.not_bound", 0f, new Dictionary<string, int>());
            return Engine.PreflightDeploy(day, target, targetDay);
        }

        public CloudSeedingResult? Deploy(int day, WeatherKind target, int targetDay = 0)
        {
            if (Engine == null) return null;
            var result = Engine.Deploy(day, target, targetDay);
            LastEvent = result.Status;
            RaiseStateChanged();
            return result;
        }

        public void TickDay(int day) { Engine?.TickDay(day); RaiseStateChanged(); }
        public CloudSeedingSaveState? CaptureState() => Engine?.CaptureState();
        public void RestoreState(CloudSeedingSaveState? state) => Engine?.RestoreState(state);
    }
}
