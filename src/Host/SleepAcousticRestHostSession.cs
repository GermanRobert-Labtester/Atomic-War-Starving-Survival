// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : SleepAcousticRestSaveStore
// Core State : Ashfall.Core.Needs.SleepAcousticState
// Host Caller: Main.SleepAcousticRest
// Purpose    : Expansion 41 — Sleep Quality, Soundproofing & Shelter Crowding
//              host session and persistence.
// ============================================================================

using System;
using Ashfall.Core.Needs;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class SleepAcousticRestSaveStore
    {
        public const string FileName = "sleep_acoustic_rest_save.json";
        public const string SectionName = "sleep_acoustic_rest";

        private static readonly SaveStore<SleepAcousticState> s_store =
            SaveStoreHub.Checksummed<SleepAcousticState>(FileName, nameof(SleepAcousticRestSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(SleepAcousticState state) => s_store.CaptureBare(state);
        public static SleepAcousticState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(SleepAcousticState state) => s_store.TrySave(state);
        public static SleepAcousticState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Expansion 41 host session. Wraps the stateful <see cref="SleepAcousticLedger"/>
    /// over the signed pure calculation engine <see cref="SleepAcousticRestEngine"/>.
    /// Extends ShelterNoiseSystem, NeedsSystem, and ShelterAssignmentSystem.
    /// Governs shelter sleeping quarters, acoustic soundproofing, quiet hours, and sensory relief.
    /// </summary>
    public sealed class SleepAcousticRestHostSession : HostSessionBase
    {
        private readonly SleepAcousticLedger _ledger;

        public SleepAcousticLedger Ledger => _ledger;
        public SleepAcousticCensus Census => _ledger.GetCensus();
        public string LastEvent { get; private set; } = string.Empty;

        public SleepAcousticRestHostSession(SleepAcousticState? state = null)
        {
            _ledger = new SleepAcousticLedger(state);
        }

        public static SleepAcousticRestHostSession Create(SleepAcousticState? state = null) =>
            new SleepAcousticRestHostSession(state);

        public void RegisterOrUpdateQuarter(SleepingQuarterState quarter)
        {
            _ledger.RegisterOrUpdateQuarter(quarter);
            LastEvent = $"Registered/updated sleeping quarter '{quarter.RoomId}'.";
            RaiseStateChanged();
        }

        public SleepQualityResult EvaluateQuarterSleep(
            string roomId,
            bool isQuietHours,
            QuietHoursCompliance compliance,
            int hoursSlept)
        {
            var result = _ledger.EvaluateQuarterSleep(roomId, isQuietHours, compliance, hoursSlept);
            LastEvent = $"Sleeping quarter '{roomId}' evaluated: Quality {result.SleepQualityIndexPermille}\u2030 ({result.EnvironmentBand}), Fatigue recovery: {result.FatigueRestorationMultiplierPermille}\u2030.";
            RaiseStateChanged();
            return result;
        }

        public bool InstallSensoryReliefKit(string roomId)
        {
            bool success = _ledger.InstallSensoryReliefKit(roomId);
            LastEvent = success
                ? $"Sensory relief kit installed in quarter '{roomId}'."
                : $"Failed to install sensory relief kit in quarter '{roomId}': Reserve depleted or quarter missing!";
            RaiseStateChanged();
            return success;
        }

        public void RestockSensoryReliefKits(int count)
        {
            _ledger.RestockSensoryReliefKits(count);
            LastEvent = $"Restocked sensory relief kits (+{count}). Total reserve: {_ledger.GetCensus().SensoryKitsReserve}.";
            RaiseStateChanged();
        }

        public void UpdateRoomSoundproofing(string roomId, int wallPermille, int doorPermille)
        {
            _ledger.UpdateRoomSoundproofing(roomId, wallPermille, doorPermille);
            LastEvent = $"Updated soundproofing for quarter '{roomId}' (Walls: {wallPermille}\u2030, Doors: {doorPermille}\u2030).";
            RaiseStateChanged();
        }

        public void SetQuietHoursSchedule(int startHour, int endHour, bool active)
        {
            _ledger.SetQuietHoursSchedule(startHour, endHour, active);
            LastEvent = $"Quiet hours set to {startHour:D2}:00 - {endHour:D2}:00 (Active: {active}).";
            RaiseStateChanged();
        }

        public void AdvanceDay(int hoursSlept = 8)
        {
            _ledger.AdvanceDay(hoursSlept);
            LastEvent = $"Advanced sleeping quarters rest for day ({hoursSlept}h slept).";
            RaiseStateChanged();
        }

        public SleepAcousticState CaptureState() => _ledger.CaptureState();
        public void RestoreState(SleepAcousticState? state) => _ledger.RestoreState(state);
    }
}
