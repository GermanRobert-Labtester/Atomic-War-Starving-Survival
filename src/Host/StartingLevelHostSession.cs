using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for ASHFALL's starting level: The Holdfast (Day 1).
    /// Bridges StartingLevelSystem simulation to Godot UI and persists to user://.
    /// </summary>
    public sealed class StartingLevelHostSession
    : HostSessionBase{
        public StartingLevelSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public StartingLevelHostSession(StartingLevelSystem? system = null)
        {
            System = system ?? new StartingLevelSystem();
            System.OnStateChanged += () => RaiseStateChanged();
            System.OnDirectiveLogged += msg => LastEvent = msg;
        }

        public static StartingLevelHostSession Create()
        {
            var session = new StartingLevelHostSession();
            var save = StartingLevelSaveStore.TryLoad();
            if (save != null)
            {
                session.System.RestoreState(save);
                session.LastEvent = "Holdfast starting state restored from disk.";
            }
            return session;
        }

        public void ResolveMorningRationTriage(RationPolicy policy)
        {
            System.ResolveMorningRationTriage(policy);
            RaiseStateChanged();
        }

        public void ResolveMiddayMaintenance(MaintenanceDirective directive)
        {
            System.ResolveMiddayMaintenance(directive);
            RaiseStateChanged();
        }

        public void ResolveEveningRadio(RadioProtocol protocol)
        {
            System.ResolveEveningRadio(protocol);
            RaiseStateChanged();
        }

        public void InspectRoom(string roomId)
        {
            System.InspectRoom(roomId);
            RaiseStateChanged();
        }

        public bool ServiceAirFilter()
        {
            bool success = System.ServiceAirFilter();
            if (success) RaiseStateChanged();
            return success;
        }

        public bool ReplaceAirFilter()
        {
            bool success = System.ReplaceAirFilter();
            if (success) RaiseStateChanged();
            return success;
        }

        public void TickDay() => TickDay(false, Ashfall.Core.WeatherKind.Clear);

        public void TickDay(bool isFilterDutyAssigned, Ashfall.Core.WeatherKind outdoorWeather)
        {
            System.TickDay(isFilterDutyAssigned, outdoorWeather);
            RaiseStateChanged();
        }

        public StartingLevelSaveState CaptureState() => System.CaptureState();

        public void RestoreState(StartingLevelSaveState state)
        {
            System.RestoreState(state);
            RaiseStateChanged();
        }
    }

    /// <summary>
    /// Save store for the starting level state in Godot user:// directory.
    /// Thin façade over the Core SaveStore&lt;T&gt; service (via SaveStoreHub,
    /// codec flavor): this section's property envelope
    /// <c>{ State, Checksum }</c> and its verify-then-bare-fallback load
    /// semantics are preserved verbatim by local encode/decode delegates;
    /// path resolution, atomic write, and error handling live in the service.
    /// </summary>
    public static class StartingLevelSaveStore
    {
        public const string SaveFileName = "starting_level_save.json";
        public const string SectionName = "starting_level";

        private static readonly SaveStore<StartingLevelSaveState> s_store = SaveStoreHub.FromCodec(
            SaveFileName,
            nameof(StartingLevelSaveStore),
            EncodeSave,
            DecodeSave);

        public static bool TrySave(StartingLevelSaveState state, string? pathOverride = null) => s_store.TrySave(state, pathOverride);

        public static bool SaveExists(string? pathOverride = null) => File.Exists(pathOverride ?? s_store.SavePath);

        public static StartingLevelSaveState? TryLoad(string? pathOverride = null) => s_store.TryLoad(pathOverride);

        private static string EncodeSave(StartingLevelSaveState state, IJsonSerializer json)
        {
            var envelope = new StartingLevelSaveEnvelope { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);
            return json.Serialize(envelope);
        }

        private static StartingLevelSaveState? DecodeSave(string raw, IJsonSerializer json)
        {
            try
            {
                var envelope = json.Deserialize<StartingLevelSaveEnvelope>(raw);
                if (envelope?.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum))
                        throw new InvalidOperationException("save envelope missing checksum (corrupt save)");
                    if (!string.Equals(envelope.Checksum, SaveChecksum.Compute(envelope), StringComparison.Ordinal))
                        throw new InvalidOperationException("checksum mismatch — possible tampering");
                    return envelope.State;
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch
            {
                // Fall back to legacy bare state decode
            }

            return json.Deserialize<StartingLevelSaveState>(raw);
        }
    }

    [Serializable]
    public sealed class StartingLevelSaveEnvelope
    {
        public StartingLevelSaveState? State { get; set; }
        public string? Checksum { get; set; }
    }
}
