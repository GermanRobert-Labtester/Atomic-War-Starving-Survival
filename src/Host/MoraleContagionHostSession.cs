// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin host session over <see cref="MoraleContagionSystem"/> (Flagship XI —
    /// Plan 154). Forwards player commands, carries presentation state
    /// (LastEvent/StateChanged), and owns the HopeBeacon installation marker.
    /// No gameplay logic lives here.
    /// </summary>
    public sealed class MoraleContagionHostSession : HostSessionBase
    {
        public const string HopeBeaconRoomId = "room_hope_beacon";
        public const int HopeBeaconStaffingThreshold = 1;

        public MoraleContagionSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        private int _hopeBeaconInstalledDay = -1;
        private Func<bool>? _beaconPowerQuery;
        private Func<int>? _beaconOccupancyQuery;

        public MoraleContagionHostSession(MoraleContagionSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));

            System.OnMoraleBreakdown += e =>
            {
                LastEvent = $"{e.SurvivorId} has reached a breaking point ({MoraleEmotionNames.Label(e.DominantEmotion)}).";
                RaiseStateChanged();
            };
            System.OnMoraleSchismTriggered += e =>
            {
                LastEvent = $"The {e.SubgroupId} crew is fracturing — {e.AffectedCount} of {e.MemberCount} have passed the point of despair.";
                RaiseStateChanged();
            };
            System.OnIsolationChanged += (survivorId, isolated) =>
            {
                LastEvent = isolated
                    ? $"{survivorId} has been moved apart from the others."
                    : $"{survivorId} has rejoined the common rooms.";
                RaiseStateChanged();
            };
        }

        /// <summary>Wires the beacon's operating gates (power + staffing) to the host.</summary>
        public void ConfigureHopeBeacon(Func<bool> powerQuery, Func<int> occupancyQuery)
        {
            _beaconPowerQuery = powerQuery;
            _beaconOccupancyQuery = occupancyQuery;
        }

        // ------------------------------------------------------------- day tick

        /// <summary>Runs the daily propagation pass (called from the survivors-needs owner).</summary>
        public void EvaluateDay(int day)
        {
            System.EvaluateDailyContagion(day);
            RaiseStateChanged();
        }

        // --------------------------------------------------------------- beacon

        public bool IsHopeBeaconInstalled => _hopeBeaconInstalledDay >= 0;
        public int HopeBeaconInstalledDay => _hopeBeaconInstalledDay;

        /// <summary>Records the installation (costs are charged by the host before calling).</summary>
        public void InstallHopeBeacon(int day)
        {
            _hopeBeaconInstalledDay = day;
            LastEvent = "The beacon lamp now stands in the common room.";
            RaiseStateChanged();
        }

        /// <summary>
        /// Built AND operating: installed, staffed, and the grid is up. The
        /// contagion system reads this as its standing hope source.
        /// </summary>
        public bool IsHopeBeaconOperating()
        {
            if (_hopeBeaconInstalledDay < 0) return false;
            if ((_beaconOccupancyQuery?.Invoke() ?? 0) < HopeBeaconStaffingThreshold) return false;
            return _beaconPowerQuery?.Invoke() ?? false;
        }

        // ------------------------------------------------------------ commands

        public string IsolateSurvivor(string survivorId, int day, int durationDays)
        {
            bool ok = System.TryApplySocialIsolation(survivorId, day, durationDays);
            RaiseStateChanged();
            return ok
                ? $"{survivorId} moves to the far bunk. The quiet will cost them."
                : $"{survivorId} cannot be isolated right now.";
        }

        public string ReleaseSurvivor(string survivorId, int day)
        {
            bool ok = System.EndSocialIsolation(survivorId, day);
            RaiseStateChanged();
            return ok ? $"{survivorId} rejoins the common rooms." : $"{survivorId} is not in isolation.";
        }

        // ---------------------------------------------------------- read model

        /// <summary>Compact per-survivor influence lines for UI panels (Plan 154.11).</summary>
        public IReadOnlyList<string> GetInfluenceLines(string survivorId)
        {
            var summary = System.GetInfluenceSummary(survivorId);
            var lines = new List<string>();
            if (summary.IsIsolated) lines.Add("Status: kept apart from the others");

            if (summary.DespairPressure > 0.05f)
                lines.Add($"Despair pressure: {DescribeStrength(summary.DespairPressure)}");
            if (summary.PanicPressure > 0.05f)
                lines.Add($"Panic pressure: {DescribeStrength(summary.PanicPressure)}");
            if (summary.HopePressure > 0.05f)
                lines.Add($"Hope pressure: {DescribeStrength(summary.HopePressure)}");

            foreach (var influence in summary.Influences)
            {
                string source = string.IsNullOrEmpty(influence.SourceSurvivorId)
                    ? "the holdfast itself"
                    : influence.SourceSurvivorId;
                lines.Add($"Feels {MoraleEmotionNames.Label(influence.Emotion)} from {source} ({DescribeStrength(influence.Strength)})");
            }
            if (lines.Count == 0) lines.Add("No social pressure worth naming.");
            return lines;
        }

        private static string DescribeStrength(float strength) => strength switch
        {
            > 0.6f => "heavy",
            > 0.3f => "steady",
            _ => "faint"
        };

        // ------------------------------------------------------------ save

        public MoraleContagionSaveState CaptureSave()
        {
            var save = MoraleContagionSaveCodec.ToSaveState(System.CaptureState());
            save.hopeBeaconInstalledDay = _hopeBeaconInstalledDay;
            return save;
        }

        public void RestoreSave(MoraleContagionSaveState save)
        {
            if (save == null) return;
            System.RestoreState(MoraleContagionSaveCodec.FromSaveState(save));
            _hopeBeaconInstalledDay = save.hopeBeaconInstalledDay;
            // Restore never raises domain events; a status line replaces them.
            LastEvent = "Settlement mood restored from save.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            MoraleContagionSaveStore.TrySave(CaptureSave());
        }
    }
}
