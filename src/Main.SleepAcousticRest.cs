// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Expansion 41 — The Quiet: Sleep Quality, Soundproofing & Shelter
// Crowding host wiring.
// The signed pure SleepAcousticRestEngine is the calculation authority.
// Existing shelter noise, needs, and shelter assignment systems remain their
// own authorities; this host owns sleeping quarter soundproofing attenuation,
// bunk decibels, crowding density penalties, quiet hours compliance, sensory
// relief kits, and restorative sleep multipliers.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Needs;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SleepAcousticRestHostSession? _sleepAcousticRest;
        private bool _sleepAcousticRestDirty;

        public SleepAcousticRestHostSession? SleepAcousticRest => _sleepAcousticRest;

        public void SetupSleepAcousticRest()
        {
            if (_sleepAcousticRest != null) return;

            var saved = SleepAcousticRestSaveStore.TryLoad();
            _sleepAcousticRest = SleepAcousticRestHostSession.Create(saved);
            _sleepAcousticRest.StateChanged += () => _sleepAcousticRestDirty = true;
        }

        public SleepQualityResult EvaluateSleepingQuarterSleep(
            string roomId,
            bool isQuietHours,
            QuietHoursCompliance compliance,
            int hoursSlept)
        {
            SetupSleepAcousticRest();
            return _sleepAcousticRest!.EvaluateQuarterSleep(roomId, isQuietHours, compliance, hoursSlept);
        }

        public bool InstallSensoryReliefKit(string roomId)
        {
            SetupSleepAcousticRest();
            return _sleepAcousticRest!.InstallSensoryReliefKit(roomId);
        }

        public void RestockSensoryReliefKits(int count)
        {
            SetupSleepAcousticRest();
            _sleepAcousticRest!.RestockSensoryReliefKits(count);
        }

        public void UpdateSleepingQuarterSoundproofing(string roomId, int wallPermille, int doorPermille)
        {
            SetupSleepAcousticRest();
            _sleepAcousticRest!.UpdateRoomSoundproofing(roomId, wallPermille, doorPermille);
        }

        public void SetQuietHoursSchedule(int startHour, int endHour, bool active)
        {
            SetupSleepAcousticRest();
            _sleepAcousticRest!.SetQuietHoursSchedule(startHour, endHour, active);
        }

        public void AdvanceSleepAcousticRestDay(int hoursSlept = 8)
        {
            SetupSleepAcousticRest();
            _sleepAcousticRest!.AdvanceDay(hoursSlept);
        }

        public SleepAcousticCensus GetSleepAcousticCensus() =>
            _sleepAcousticRest?.Census ?? default;

        public void SaveSleepAcousticRest()
        {
            if (_sleepAcousticRest == null) return;
            var state = _sleepAcousticRest.Ledger.CaptureState();
            SleepAcousticRestSaveStore.TrySave(state);
            if (CaptureSection(
                    SleepAcousticRestSaveStore.SectionName,
                    SleepAcousticRestSaveStore.TryCapturePersisted(state)))
            {
                _sleepAcousticRestDirty = false;
            }
        }

        public void FlushSleepAcousticRestIfDirty()
        {
            if (_sleepAcousticRestDirty)
            {
                SaveSleepAcousticRest();
            }
        }

        public void ResetSleepAcousticRest()
        {
            _sleepAcousticRest = null;
            _sleepAcousticRestDirty = false;
        }
    }
}
