// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : PerimeterEarlyWarningSaveStore
// Core State : Ashfall.Core.Defense.PerimeterEarlyWarningSaveState
// Host Caller: Main.PerimeterEarlyWarning
// Purpose    : Perimeter radar host session & persistence. The Core engine
//              remains the sole classification authority (hostile incursion vs
//              environmental noise vs. false alarm); the host composes it with
//              the canonical weather owner's storm/dust fact and the power grid's
//              radar draw.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Defense;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class PerimeterEarlyWarningSaveStore
    {
        public const string FileName = "perimeter_early_warning_save.json";
        public const string SectionName = "perimeter_early_warning";

        private static readonly SaveStore<PerimeterEarlyWarningSaveState> s_store =
            SaveStoreHub.Checksummed<PerimeterEarlyWarningSaveState>(FileName, nameof(PerimeterEarlyWarningSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(PerimeterEarlyWarningSaveState state) => s_store.CaptureBare(state);
        public static PerimeterEarlyWarningSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(PerimeterEarlyWarningSaveState state) => s_store.TrySave(state);
        public static PerimeterEarlyWarningSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Host session composing the Core <see cref="PerimeterEarlyWarningEngine"/>.</summary>
    public sealed class PerimeterEarlyWarningHostSession : HostSessionBase
    {
        private readonly PerimeterEarlyWarningEngine _engine;

        public PerimeterEarlyWarningEngine Engine => _engine;
        public string LastEvent { get; private set; } = string.Empty;

        public PerimeterEarlyWarningHostSession(PerimeterEarlyWarningSaveState? state = null)
        {
            _engine = new PerimeterEarlyWarningEngine();
            if (state != null) _engine.RestoreState(state);
        }

        public static PerimeterEarlyWarningHostSession Create(PerimeterEarlyWarningSaveState? state = null) =>
            new PerimeterEarlyWarningHostSession(state);

        public void SetMode(RadarOperationalMode mode)
        {
            _engine.SetMode(mode);
            LastEvent = $"Radar mode set to {mode}.";
            RaiseStateChanged();
        }

        public void CalibrateSensors(int deltaPermille)
        {
            _engine.CalibrateSensors(deltaPermille);
            LastEvent = $"Sensor calibration now {_engine.CalibrationPermille} per mille.";
            RaiseStateChanged();
        }

        /// <summary>
        /// Runs one scan sweep. The caller supplies the storm/dust fact from the
        /// canonical weather owner and a deterministic seeded roll in permille.
        /// </summary>
        public RadarContact? ProcessScanSweep(
            string sector, int actualDistanceMeters, bool isHostile, bool isStormOrDust,
            int currentTick, int seededRollPermille)
        {
            var contact = _engine.ProcessScanSweep(
                sector, actualDistanceMeters, isHostile, isStormOrDust, currentTick, seededRollPermille);
            if (contact != null)
            {
                LastEvent = $"Contact {contact.ContactId}: {contact.Classification} in {contact.Sector}.";
                RaiseStateChanged();
            }
            return contact;
        }

        public RadrModeState GetRadarState() => new RadrModeState(_engine.Mode, _engine.CalibrationPermille);

        public (int Contacts, int PowerWatts, int RangeMeters) GetReadout() =>
            (_engine.ActiveContacts.Count, _engine.PowerDrawWatts, _engine.DetectionRangeMeters);

        public PerimeterEarlyWarningSaveState CaptureState() => _engine.CaptureState();

        public void RestoreState(PerimeterEarlyWarningSaveState state)
        {
            _engine.RestoreState(state);
            LastEvent = "Restored perimeter radar state.";
            RaiseStateChanged();
        }

        public bool TrySave() => PerimeterEarlyWarningSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = PerimeterEarlyWarningSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }

    /// <summary>Read-only radar posture projection for status surfaces.</summary>
    public readonly struct RadrModeState
    {
        public RadarOperationalMode Mode { get; }
        public int CalibrationPermille { get; }
        public RadrModeState(RadarOperationalMode mode, int calibrationPermille)
        {
            Mode = mode;
            CalibrationPermille = calibrationPermille;
        }
    }
}
