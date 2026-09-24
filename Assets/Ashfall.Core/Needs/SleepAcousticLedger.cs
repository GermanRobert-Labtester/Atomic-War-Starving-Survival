// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 41 — The Quiet
// Subsystem    : Sleep Quality, Soundproofing & Shelter Crowding Ledger
// Authority    : docs/expansions/wave6/expansion_41_the_quiet_plan.md
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Needs
{
    /// <summary>
    /// Persistent serializable state of shelter sleeping quarters and acoustic rest management.
    /// Extends ShelterNoiseSystem, NeedsSystem, and ShelterAssignmentSystem.
    /// </summary>
    public sealed class SleepAcousticState
    {
        public int SchemaVersion { get; set; } = 1;
        public List<SleepingQuarterState> Quarters { get; set; } = new List<SleepingQuarterState>();
        public int SensoryReliefKitsReserve { get; set; } = 10;
        public bool QuietHoursActive { get; set; } = true;
        public int QuietHoursStartHour { get; set; } = 22;
        public int QuietHoursEndHour { get; set; } = 6;
        public int QuietHoursViolationsCount { get; set; } = 0;
        public int SleepDisturbanceAlertCount { get; set; } = 0;
        public int TotalNightsSlept { get; set; } = 0;

        public SleepAcousticState Clone()
        {
            var clone = new SleepAcousticState
            {
                SchemaVersion = SchemaVersion,
                SensoryReliefKitsReserve = SensoryReliefKitsReserve,
                QuietHoursActive = QuietHoursActive,
                QuietHoursStartHour = QuietHoursStartHour,
                QuietHoursEndHour = QuietHoursEndHour,
                QuietHoursViolationsCount = QuietHoursViolationsCount,
                SleepDisturbanceAlertCount = SleepDisturbanceAlertCount,
                TotalNightsSlept = TotalNightsSlept
            };
            foreach (var q in Quarters)
            {
                if (q != null)
                {
                    clone.Quarters.Add(q.Clone());
                }
            }
            return clone;
        }
    }

    /// <summary>
    /// Read-only snapshot census of shelter acoustic rest and sleeping environments.
    /// </summary>
    public struct SleepAcousticCensus
    {
        public int QuartersCount { get; }
        public int TotalOccupants { get; }
        public int AverageSleepQualityPermille { get; }
        public int RestfulOrSanctuaryCount { get; }
        public int DisturbedOrUnbearableCount { get; }
        public int SensoryKitsReserve { get; }
        public int QuietHoursViolationsCount { get; }

        public SleepAcousticCensus(
            int quartersCount,
            int totalOccupants,
            int averageSleepQualityPermille,
            int restfulOrSanctuaryCount,
            int disturbedOrUnbearableCount,
            int sensoryKitsReserve,
            int quietHoursViolationsCount)
        {
            QuartersCount = Math.Max(0, quartersCount);
            TotalOccupants = Math.Max(0, totalOccupants);
            AverageSleepQualityPermille = Math.Clamp(averageSleepQualityPermille, 0, 1000);
            RestfulOrSanctuaryCount = Math.Max(0, restfulOrSanctuaryCount);
            DisturbedOrUnbearableCount = Math.Max(0, disturbedOrUnbearableCount);
            SensoryKitsReserve = Math.Max(0, sensoryKitsReserve);
            QuietHoursViolationsCount = Math.Max(0, quietHoursViolationsCount);
        }
    }

    /// <summary>
    /// Stateful domain ledger managing sleeping quarters, soundproofing attenuation,
    /// quiet hours compliance, sensory relief kit deployment, and restorative sleep metrics.
    /// </summary>
    public sealed class SleepAcousticLedger
    {
        public const string DefaultQuarterRoomId = "primary_shelter_dormitory";

        private SleepAcousticState _state = new SleepAcousticState();

        public SleepAcousticLedger(SleepAcousticState? state = null)
        {
            RestoreState(state);
        }

        private void EnsureDefaultQuarter()
        {
            if (_state.Quarters.Count == 0)
            {
                _state.Quarters.Add(new SleepingQuarterState
                {
                    RoomId = DefaultQuarterRoomId,
                    RoomAreaSquareMetres = 28,
                    AssignedOccupants = 4,
                    WallSoundproofingPermille = 650,
                    DoorSoundproofingPermille = 550,
                    AmbientNoiseLevelDecibels = 48,
                    DarknessQualityPermille = 800,
                    HasSensoryReliefKit = false
                });
            }
        }

        public void RegisterOrUpdateQuarter(SleepingQuarterState quarter)
        {
            if (quarter == null || string.IsNullOrWhiteSpace(quarter.RoomId)) return;

            int idx = _state.Quarters.FindIndex(q => string.Equals(q.RoomId, quarter.RoomId, StringComparison.OrdinalIgnoreCase));
            if (idx >= 0)
            {
                _state.Quarters[idx] = quarter.Clone();
            }
            else
            {
                _state.Quarters.Add(quarter.Clone());
            }
        }

        public SleepingQuarterState? GetQuarter(string roomId)
        {
            if (string.IsNullOrWhiteSpace(roomId)) return null;
            return _state.Quarters.FirstOrDefault(q => string.Equals(q.RoomId, roomId, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<SleepingQuarterState> Quarters => _state.Quarters;

        public SleepQualityResult EvaluateQuarterSleep(
            string roomId,
            bool isQuietHours,
            QuietHoursCompliance compliance,
            int hoursSlept)
        {
            var quarter = GetQuarter(roomId);
            if (quarter == null)
            {
                EnsureDefaultQuarter();
                quarter = _state.Quarters[0];
            }

            var result = SleepAcousticRestEngine.EvaluateSleepQuality(
                quarter,
                isQuietHours,
                compliance,
                hoursSlept);

            if (compliance != QuietHoursCompliance.Compliant)
            {
                _state.QuietHoursViolationsCount++;
            }

            if (result.EnvironmentBand == SleepEnvironmentBand.Disturbed ||
                result.EnvironmentBand == SleepEnvironmentBand.Unbearable)
            {
                _state.SleepDisturbanceAlertCount++;
            }

            return result;
        }

        public bool InstallSensoryReliefKit(string roomId)
        {
            if (_state.SensoryReliefKitsReserve <= 0) return false;

            var quarter = GetQuarter(roomId);
            if (quarter == null) return false;

            if (quarter.HasSensoryReliefKit) return true; // already installed

            _state.SensoryReliefKitsReserve--;
            quarter.HasSensoryReliefKit = true;
            return true;
        }

        public void RestockSensoryReliefKits(int count)
        {
            if (count > 0)
            {
                _state.SensoryReliefKitsReserve += count;
            }
        }

        public void UpdateRoomSoundproofing(string roomId, int wallPermille, int doorPermille)
        {
            var quarter = GetQuarter(roomId);
            if (quarter == null) return;

            quarter.WallSoundproofingPermille = Math.Clamp(wallPermille, 0, 1000);
            quarter.DoorSoundproofingPermille = Math.Clamp(doorPermille, 0, 1000);
        }

        public void SetQuietHoursSchedule(int startHour, int endHour, bool active)
        {
            _state.QuietHoursStartHour = Math.Clamp(startHour, 0, 23);
            _state.QuietHoursEndHour = Math.Clamp(endHour, 0, 23);
            _state.QuietHoursActive = active;
        }

        public void AdvanceDay(int hoursSlept = 8)
        {
            EnsureDefaultQuarter();
            hoursSlept = Math.Clamp(hoursSlept, 1, 16);

            foreach (var quarter in _state.Quarters)
            {
                var compliance = _state.QuietHoursActive ? QuietHoursCompliance.Compliant : QuietHoursCompliance.ViolatedMinor;
                var result = EvaluateQuarterSleep(quarter.RoomId, _state.QuietHoursActive, compliance, hoursSlept);
            }

            _state.TotalNightsSlept++;
        }

        public SleepAcousticCensus GetCensus()
        {
            EnsureDefaultQuarter();

            int totalQuarters = _state.Quarters.Count;
            int totalOccupants = 0;
            int totalQuality = 0;
            int restfulCount = 0;
            int disturbedCount = 0;

            foreach (var q in _state.Quarters)
            {
                totalOccupants += q.AssignedOccupants;
                var res = SleepAcousticRestEngine.EvaluateSleepQuality(
                    q,
                    _state.QuietHoursActive,
                    QuietHoursCompliance.Compliant,
                    8);

                totalQuality += res.SleepQualityIndexPermille;
                if (res.EnvironmentBand == SleepEnvironmentBand.Restful || res.EnvironmentBand == SleepEnvironmentBand.DeepSanctuary)
                {
                    restfulCount++;
                }
                else if (res.EnvironmentBand == SleepEnvironmentBand.Disturbed || res.EnvironmentBand == SleepEnvironmentBand.Unbearable)
                {
                    disturbedCount++;
                }
            }

            int avgQuality = totalQuarters > 0 ? totalQuality / totalQuarters : 0;

            return new SleepAcousticCensus(
                quartersCount: totalQuarters,
                totalOccupants: totalOccupants,
                averageSleepQualityPermille: avgQuality,
                restfulOrSanctuaryCount: restfulCount,
                disturbedOrUnbearableCount: disturbedCount,
                sensoryKitsReserve: _state.SensoryReliefKitsReserve,
                quietHoursViolationsCount: _state.QuietHoursViolationsCount);
        }

        public SleepAcousticState CaptureState()
        {
            return _state.Clone();
        }

        public void RestoreState(SleepAcousticState? state)
        {
            if (state == null)
            {
                _state = new SleepAcousticState();
            }
            else
            {
                _state = state.Clone();
            }
            EnsureDefaultQuarter();
        }
    }
}
