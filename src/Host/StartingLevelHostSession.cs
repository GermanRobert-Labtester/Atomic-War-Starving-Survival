using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.StartingLevel;

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
    /// Uses SystemTextJsonSerializer (Ashfall.Core.HostDefaults) so the serializer
    /// is consistent with every other host save store.
    /// </summary>
    public static class StartingLevelSaveStore
    {
        public const string SaveFileName = "starting_level_save.json";
        public const string SectionName = "starting_level";
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static bool TrySave(StartingLevelSaveState state, string? pathOverride = null)
        {
            try
            {
                if (state == null) return false;
                string path = pathOverride ?? SaveSlotRoot.Resolve(SaveFileName);
                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var envelope = new StartingLevelSaveEnvelope { State = state };
                envelope.Checksum = SaveChecksum.Compute(envelope);

                string json = s_json.Serialize(envelope);
                File.WriteAllText(path, json);
                return true;
            }
            catch (Exception ex)
            {
                GD.PushWarning($"[StartingLevelSaveStore] Failed to save starting level: {ex.Message}");
                return false;
            }
        }

        public static bool SaveExists(string? pathOverride = null)
        {
            string path = pathOverride ?? SaveSlotRoot.Resolve(SaveFileName);
            return File.Exists(path);
        }

        public static StartingLevelSaveState? TryLoad(string? pathOverride = null)
        {
            try
            {
                string path = pathOverride ?? SaveSlotRoot.Resolve(SaveFileName);
                if (!File.Exists(path)) return null;
                string json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json)) return null;

                try
                {
                    var envelope = s_json.Deserialize<StartingLevelSaveEnvelope>(json);
                    if (envelope?.State != null)
                    {
                        if (string.IsNullOrEmpty(envelope.Checksum))
                        {
                            GD.PrintErr("[StartingLevelSaveStore] save envelope missing checksum (corrupt save)");
                            return null;
                        }
                        string computed = SaveChecksum.Compute(envelope);
                        if (!string.Equals(envelope.Checksum, computed, StringComparison.Ordinal))
                        {
                            GD.PrintErr("[StartingLevelSaveStore] checksum mismatch — possible tampering");
                            return null;
                        }
                        return envelope.State;
                    }
                }
                catch
                {
                    // Fall back to legacy bare state decode
                }

                return s_json.Deserialize<StartingLevelSaveState>(json);
            }
            catch (Exception ex)
            {
                GD.PushWarning($"[StartingLevelSaveStore] Failed to load starting level: {ex.Message}");
                return null;
            }
        }
    }

    [Serializable]
    public sealed class StartingLevelSaveEnvelope
    {
        public StartingLevelSaveState? State { get; set; }
        public string? Checksum { get; set; }
    }
}
