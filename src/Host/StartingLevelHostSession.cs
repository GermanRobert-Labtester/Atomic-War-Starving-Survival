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
    {
        public StartingLevelSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public event Action? StateChanged;

        public StartingLevelHostSession(StartingLevelSystem? system = null)
        {
            System = system ?? new StartingLevelSystem();
            System.OnStateChanged += () => StateChanged?.Invoke();
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
            StateChanged?.Invoke();
        }

        public void ResolveMiddayMaintenance(MaintenanceDirective directive)
        {
            System.ResolveMiddayMaintenance(directive);
            StateChanged?.Invoke();
        }

        public void ResolveEveningRadio(RadioProtocol protocol)
        {
            System.ResolveEveningRadio(protocol);
            StateChanged?.Invoke();
        }

        public void InspectRoom(string roomId)
        {
            System.InspectRoom(roomId);
            StateChanged?.Invoke();
        }

        public bool ServiceAirFilter()
        {
            bool success = System.ServiceAirFilter();
            if (success) StateChanged?.Invoke();
            return success;
        }

        public bool ReplaceAirFilter()
        {
            bool success = System.ReplaceAirFilter();
            if (success) StateChanged?.Invoke();
            return success;
        }

        public void TickDay() => TickDay(false, Ashfall.Core.WeatherKind.Clear);

        public void TickDay(bool isFilterDutyAssigned, Ashfall.Core.WeatherKind outdoorWeather)
        {
            System.TickDay(isFilterDutyAssigned, outdoorWeather);
            StateChanged?.Invoke();
        }

        public StartingLevelSaveState CaptureState() => System.CaptureState();

        public void RestoreState(StartingLevelSaveState state)
        {
            System.RestoreState(state);
            StateChanged?.Invoke();
        }
    }

    /// <summary>
    /// Save store for the starting level state in Godot user:// directory.
    /// Uses SystemTextJsonSerializer (Ashfall.Core.HostDefaults) so the serializer
    /// is consistent with every other host save store.
    /// </summary>
    public static class StartingLevelSaveStore
    {
        private const string SaveFileName = "starting_level_save.json";
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static bool TrySave(StartingLevelSaveState state, string? pathOverride = null)
        {
            try
            {
                string path = pathOverride ?? Path.Combine(ProjectSettings.GlobalizePath("user://"), SaveFileName);
                string json = s_json.Serialize(state);
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
            string path = pathOverride ?? Path.Combine(ProjectSettings.GlobalizePath("user://"), SaveFileName);
            return File.Exists(path);
        }

        public static StartingLevelSaveState? TryLoad(string? pathOverride = null)
        {
            try
            {
                string path = pathOverride ?? Path.Combine(ProjectSettings.GlobalizePath("user://"), SaveFileName);
                if (!File.Exists(path)) return null;
                string json = File.ReadAllText(path);
                return s_json.Deserialize<StartingLevelSaveState>(json);
            }
            catch (Exception ex)
            {
                GD.PushWarning($"[StartingLevelSaveStore] Failed to load starting level: {ex.Message}");
                return null;
            }
        }
    }
}
