// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : NightWatchHostSession
// Core authority : NightWatchPatrolReadinessEngine + canonical owner adapters
// Purpose : live Watch board, patrol/readiness commands, and daily projection.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Defense;
using Ashfall.Core.Narrative;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin host adapter for Expansion 36. It owns no gameplay authority:
    /// staffing is written through DutyRosterSystem, perimeter/alarm state
    /// through PerimeterDefenseSystem, gate state through ShelterSecuritySystem,
    /// and acoustic facts through SoundRangingThreatEngine.
    /// </summary>
    public sealed class NightWatchHostSession : HostSessionBase
    {
        private readonly PerimeterDefenseSystem _perimeter;
        private readonly DutyRosterSystem _roster;
        private readonly ShelterSecuritySystem _security;
        private readonly NightWatchOperationsCatalog? _catalog;
        private readonly NightWatchCatalog? _narrativeCatalog;
        private readonly SoundRangingThreatEngine? _soundRanging;
        private int _currentDay;

        public NightWatchOperationsCatalog? Catalog => _catalog;
        public PerimeterDefenseSystem Perimeter => _perimeter;
        public DutyRosterSystem Roster => _roster;
        public ShelterSecuritySystem Security => _security;
        public NightWatchCatalog? NarrativeCatalog => _narrativeCatalog;
        public int CurrentDay => _currentDay;
        public string LastEvent { get; private set; } = string.Empty;

        /// <summary>Canonical survivor fatigue read model; the watch never mutates NeedsSystem.</summary>
        public Func<string, int>? SurvivorFatigueProvider { get; set; }
        /// <summary>Canonical territory contested read model.</summary>
        public Func<string, bool>? TerritoryContestedProvider { get; set; }
        /// <summary>Journal sink: key, human-readable fact, day.</summary>
        public Action<string, string, int>? JournalWriter { get; set; }
        /// <summary>Alarm/notification sink for a real alarm protocol transition.</summary>
        public Action<string>? AlarmWriter { get; set; }

        public NightWatchHostSession(
            PerimeterDefenseSystem perimeter,
            DutyRosterSystem roster,
            ShelterSecuritySystem security,
            NightWatchOperationsCatalog? catalog,
            NightWatchCatalog? narrativeCatalog = null,
            SoundRangingThreatEngine? soundRanging = null)
        {
            _perimeter = perimeter ?? throw new ArgumentNullException(nameof(perimeter));
            _roster = roster ?? throw new ArgumentNullException(nameof(roster));
            _security = security ?? throw new ArgumentNullException(nameof(security));
            _catalog = catalog;
            _narrativeCatalog = narrativeCatalog;
            _soundRanging = soundRanging;
            if (_catalog != null) _perimeter.BindWatchCatalog(_catalog);
        }

        public void SetCurrentDay(int day) => _currentDay = Math.Max(0, day);

        public IReadOnlyList<NightWatchPostDefinition> Posts =>
            _catalog?.posts ?? (IReadOnlyList<NightWatchPostDefinition>)Array.Empty<NightWatchPostDefinition>();
        public IReadOnlyList<NightWatchRouteDefinition> Routes =>
            _catalog?.routes ?? (IReadOnlyList<NightWatchRouteDefinition>)Array.Empty<NightWatchRouteDefinition>();
        public IReadOnlyList<NightWatchGateRuleDefinition> GateRules =>
            _catalog?.gate_rules ?? (IReadOnlyList<NightWatchGateRuleDefinition>)Array.Empty<NightWatchGateRuleDefinition>();
        public IReadOnlyList<NightWatchDetectionProfileDefinition> DetectionProfiles =>
            _catalog?.detection_profiles ?? (IReadOnlyList<NightWatchDetectionProfileDefinition>)Array.Empty<NightWatchDetectionProfileDefinition>();
        public IReadOnlyList<NightWatchAlarmProtocolDefinition> AlarmProtocols =>
            _catalog?.alarm_protocols ?? (IReadOnlyList<NightWatchAlarmProtocolDefinition>)Array.Empty<NightWatchAlarmProtocolDefinition>();
        public IReadOnlyList<NightWatchDrillDefinition> Drills =>
            _catalog?.drills ?? (IReadOnlyList<NightWatchDrillDefinition>)Array.Empty<NightWatchDrillDefinition>();

        public IReadOnlyList<string> GetEligibleSurvivorIds()
        {
            var result = new List<string>();
            for (int i = 0; i < _roster.Rows.Count; i++)
            {
                var row = _roster.Rows[i];
                if (row == null || string.IsNullOrWhiteSpace(row.survivorId)) continue;
                if (row.status == DutyRosterIds.StatusDead || row.status == DutyRosterIds.StatusMissing ||
                    row.status == DutyRosterIds.StatusQuiet || row.status == DutyRosterIds.StatusLevy ||
                    row.status == DutyRosterIds.StatusWaystation) continue;
                result.Add(row.survivorId);
            }
            result.Sort(StringComparer.Ordinal);
            return result;
        }

        public NightWatchReadinessSnapshot EvaluateSector(string sectorId)
        {
            string sector = NormalizeSector(sectorId);
            int perimeterMetres = SectorPerimeterMetres(sector);
            int staff = 0;
            int fatigueTotal = 0;
            int nightVision = 0;
            int activePosts = 0;
            int postCondition = 0;
            int gateStaff = 0;

            if (_catalog != null)
            {
                for (int i = 0; i < _catalog.posts.Count; i++)
                {
                    var post = _catalog.posts[i];
                    if (post == null || !string.Equals(post.sector_id, sector, StringComparison.OrdinalIgnoreCase)) continue;
                    int count = _roster.GetWatchShiftCount(post.post_id, _currentDay);
                    staff += count;
                    if (count > 0) fatigueTotal += _roster.GetAverageWatchFatigue(post.post_id, _currentDay) * count;
                    if (string.Equals(sector, "gate", StringComparison.OrdinalIgnoreCase)) gateStaff += count;
                }
                activePosts = _perimeter.GetWatchPostCount(sector);
                postCondition = _perimeter.GetAverageWatchPostCondition(sector);
                nightVision = _perimeter.GetAverageWatchPostNightVision(sector);
            }

            // The canonical general night-watch role remains a valid fallback
            // assignment when no watch-specific shift has been entered yet.
            string legacyRoleAssignee = _roster.GetAssignment(DutyRosterIds.RoleNightWatch);
            if (staff == 0 && !string.IsNullOrWhiteSpace(legacyRoleAssignee))
            {
                staff = 1;
                fatigueTotal = SurvivorFatigueProvider?.Invoke(legacyRoleAssignee) ?? 0;
                if (string.Equals(sector, "gate", StringComparison.OrdinalIgnoreCase)) gateStaff = 1;
            }

            int averageFatigue = staff <= 0 ? 0 : fatigueTotal / staff;
            int mechanism = CalculateGateMechanismCondition(sector);
            bool alarmOnline = CalculateAlarmOnline(sector);
            int drill = CalculateGateDrillRecency();
            bool contested = TerritoryContestedProvider?.Invoke(sector) ?? false;
            int acousticConfidence = 0;
            int sensorCount = 0;
            if (_soundRanging != null)
            {
                var threat = _soundRanging.GetActiveThreat();
                acousticConfidence = threat == null ? 0 : Math.Clamp(threat.ConfidenceBp / 10, 0, 1000);
                sensorCount = _soundRanging.State.Nodes.Count;
            }

            var facts = new NightWatchSectorFacts
            {
                SectorId = sector,
                PerimeterMetres = perimeterMetres,
                AssignedPatrollers = staff,
                AverageFatiguePermille = averageFatigue,
                NightVisionPermille = nightVision,
                ActivePostCount = activePosts,
                PostConditionPermille = postCondition,
                AlarmOnline = alarmOnline,
                GateStaffPermille = gateStaff <= 0 ? 0 : Math.Min(1000, gateStaff * 500),
                GateMechanismPermille = mechanism,
                GateDrillRecencyPermille = drill,
                TerritoryContested = contested,
                AcousticConfidencePermille = acousticConfidence,
                SoundSensorCount = sensorCount
            };
            return NightWatchReadinessProjection.Evaluate(facts);
        }

        public IReadOnlyList<NightWatchReadinessSnapshot> EvaluateAllSectors()
        {
            var sectors = new List<string> { "gate", "north", "east", "south", "west" };
            var snapshots = new List<NightWatchReadinessSnapshot>();
            for (int i = 0; i < sectors.Count; i++)
            {
                var snapshot = EvaluateSector(sectors[i]);
                snapshots.Add(snapshot);
                _perimeter.RecordWatchReadiness(_currentDay, snapshot);
            }
            LastEvent = $"Watch readiness evaluated across {snapshots.Count} sectors.";
            return snapshots;
        }

        public void TickDay(int day, bool severeWeather = false)
        {
            _currentDay = Math.Max(0, day);
            CompletePastShifts(_currentDay);
            // Physical perimeter maintenance is owned and ticked by the existing
            // perimeter day path; evaluating here ensures the new watch state is
            // always projected after that owner has advanced.
            var snapshots = EvaluateAllSectors();
            if (snapshots.Count > 0)
            {
                var gate = snapshots[0];
                LastEvent = $"Watch {gate.Band} ({gate.OverallReadinessPermille}‰), gate {gate.GateReadiness.ReadinessPermille}‰.";
            }
            RaiseStateChanged();
        }

        public ActionResult AssignWatchShift(string postId, string survivorId, int startHour, int durationHours)
        {
            if (_catalog?.Post(postId) == null) return ActionResult.Failed("unknown_watch_post", "watch.unknown_post");
            var postState = _perimeter.FindWatchPost(postId);
            if (postState == null || !postState.active) return ActionResult.Blocked("watch_post_inactive", "watch.post_inactive");
            // The roster rejects duplicate shift ids before testing overlap, so the
            // host-generated id must encode the full time window. Otherwise a
            // second non-overlapping shift for the same survivor/post/day would be
            // misreported as a duplicate instead of being overlap-checked.
            string id = $"watch_shift_{_currentDay}_{postId}_{survivorId}_{startHour}h{durationHours}";
            int before = Math.Clamp(SurvivorFatigueProvider?.Invoke(survivorId) ?? 0, 0, 1000);
            var result = _roster.AssignWatchShift(
                id,
                postId,
                survivorId,
                _currentDay,
                startHour,
                durationHours,
                fatigueBeforePermille: before);
            if (result.IsSuccess)
            {
                LastEvent = $"Watch shift assigned at {postId} for day {_currentDay}.";
                WriteJournal("watch_shift_assigned", $"{survivorId} is assigned to {postId} on day {_currentDay}.");
                RaiseStateChanged();
            }
            return result;
        }

        public ActionResult CompleteWatchShift(string shiftId)
        {
            var shift = _roster.FindWatchShift(shiftId);
            if (shift == null) return ActionResult.Failed("unknown_watch_shift", "watch.unknown_shift");
            int before = SurvivorFatigueProvider?.Invoke(shift.survivorId) ?? shift.fatigue_before_permille;
            int after = NightWatchPatrolReadinessEngine.AdvanceWatchFatigue(before, shift.duration_hours, 500);
            var result = _roster.CompleteWatchShift(shiftId, after);
            if (result.IsSuccess)
            {
                _perimeter.MarkWatchShiftCompleted(shift.post_id);
                LastEvent = $"Watch shift completed: {shift.post_id}.";
                WriteJournal("watch_shift_completed", $"{shift.survivorId} completed the {shift.post_id} shift.");
                RaiseStateChanged();
            }
            return result;
        }

        public ActionResult WalkRoute(string routeId, int day, bool debriefed = true)
        {
            var result = _perimeter.RecordWatchRoute(routeId, day, debriefed);
            if (result.IsSuccess)
            {
                LastEvent = debriefed ? $"Patrol route walked: {routeId}." : $"Patrol route walked; debrief pending: {routeId}.";
                WriteJournal("watch_route_completed", $"{routeId} was walked on day {day}.");
                RaiseStateChanged();
            }
            return result;
        }

        public ActionResult RecordDebrief(string routeId, int day)
        {
            var result = _perimeter.RecordWatchDebrief(routeId, day);
            if (result.IsSuccess)
            {
                LastEvent = $"Debrief recorded for {routeId}.";
                WriteJournal("watch_route_debriefed", $"Debrief recorded for {routeId} on day {day}.");
                RaiseStateChanged();
            }
            return result;
        }

        public ActionResult RunDrill(string drillId, int day, bool passed)
        {
            var result = _perimeter.RecordWatchDrill(drillId, day, passed);
            if (result.IsSuccess)
            {
                LastEvent = $"{drillId} {(passed ? "passed" : "failed")}.";
                WriteJournal(passed ? "watch_drill_passed" : "watch_drill_failed", $"{drillId} recorded on day {day}.");
                RaiseStateChanged();
            }
            return result;
        }

        public ActionResult SetPostActive(string postId, bool active)
        {
            var result = _perimeter.SetWatchPostActive(postId, active);
            if (result.IsSuccess)
            {
                LastEvent = $"{postId} {(active ? "activated" : "deactivated")}.";
                WriteJournal("watch_post_status_changed", $"Watch post {postId} {(active ? "activated" : "deactivated")}.");
                RaiseStateChanged();
            }
            return result;
        }

        public ActionResult RepairPost(string postId, int day)
        {
            var result = _perimeter.RepairWatchPost(postId, day);
            if (result.IsSuccess)
            {
                LastEvent = $"{postId} repaired.";
                WriteJournal("watch_post_repaired", $"Watch post {postId} was repaired on day {day}.");
                RaiseStateChanged();
            }
            return result;
        }

        public ActionResult SealGate(int day)
        {
            var snapshot = EvaluateSector("gate");
            if (!snapshot.GateReadiness.IsGateReady)
                return ActionResult.Blocked("gate_not_ready", snapshot.GateReadiness.PrimaryBottleneck);
            _security.SetShelterLockdown(true, day);
            LastEvent = "Gate sealed through ShelterSecurity.";
            WriteJournal("watch_gate_sealed", $"The watch sealed the gate on day {day}; readiness {snapshot.GateReadiness.ReadinessPermille}‰.");
            AlarmWriter?.Invoke("watch_alarm_lockdown");
            RaiseStateChanged();
            return ActionResult.Success("watch_gate_sealed");
        }

        public ActionResult OpenGate(int day)
        {
            _security.SetShelterLockdown(false, day);
            LastEvent = "Gate lockdown lifted through ShelterSecurity.";
            WriteJournal("watch_gate_opened", $"The watch lifted the gate lockdown on day {day}.");
            AlarmWriter?.Invoke("watch_alarm_all_clear");
            RaiseStateChanged();
            return ActionResult.Success("watch_gate_opened");
        }

        public ActionResult ToggleSectorAlarm(string sectorId)
        {
            string sectorIdValue = sectorId?.Trim().ToLowerInvariant() ?? string.Empty;
            if (!NightWatchOperationsCatalogLoader.Sectors.Contains(sectorIdValue))
                return ActionResult.Failed("unknown_sector", "defense.unknown_sector");
            var result = _perimeter.ToggleSectorAlarm(sectorIdValue);
            if (result.IsSuccess)
            {
                var sector = _perimeter.FindSector(sectorIdValue);
                LastEvent = $"{sectorIdValue} alarm {(sector?.alarm_armed == true ? "armed" : "disarmed")}.";
                WriteJournal("watch_alarm_toggle", $"The {sectorIdValue} perimeter alarm was toggled.");
                RaiseStateChanged();
            }
            return result;
        }

        public void RecordAcousticContact(SoundRangingThreatEngine.AcousticThreatEstimate estimate, int day)
        {
            if (estimate == null) return;
            string sector = EstimateSector(estimate.BearingDeg);
            int confidence = Math.Clamp(estimate.ConfidenceBp / 10, 0, 1000);
            _perimeter.RecordWatchDetection(sector, day, $"Bearing {estimate.BearingDeg}° ±{estimate.BearingErrorDeg}°", confidence);
            LastEvent = $"Acoustic contact recorded in {sector} at {confidence}‰ confidence.";
            WriteJournal("watch_acoustic_contact", $"Acoustic estimate: bearing {estimate.BearingDeg}° ±{estimate.BearingErrorDeg}°, confidence {estimate.ConfidenceBp / 100.0:0.0}%.");
            var protocol = SelectAlarmProtocol(confidence);
            if (protocol != null)
            {
                AlarmWriter?.Invoke($"watch_alarm_{protocol.protocol_id}: {protocol.response}");
                WriteJournal("watch_alarm_protocol", $"{protocol.display_name}: {protocol.response}");
            }
            RaiseStateChanged();
        }

        public IReadOnlyList<NightWatchEntry> GetNarrativeIncidents() =>
            _narrativeCatalog?.AllIncidents ?? (IReadOnlyList<NightWatchEntry>)Array.Empty<NightWatchEntry>();

        private void CompletePastShifts(int day)
        {
            var shifts = _roster.GetWatchShifts();
            for (int i = 0; i < shifts.Count; i++)
            {
                var shift = shifts[i];
                if (shift == null || shift.completed || shift.day >= day) continue;
                int before = SurvivorFatigueProvider?.Invoke(shift.survivorId) ?? shift.fatigue_before_permille;
                int after = NightWatchPatrolReadinessEngine.AdvanceWatchFatigue(before, shift.duration_hours, 500);
                if (_roster.CompleteWatchShift(shift.shift_id, after).IsSuccess)
                {
                    _perimeter.MarkWatchShiftCompleted(shift.post_id);
                    WriteJournal("watch_shift_completed", $"{shift.survivorId}'s {shift.post_id} shift closed at dawn.");
                }
            }
        }

        private int CalculateGateMechanismCondition(string sector)
        {
            var state = _perimeter.FindSector(sector);
            if (state != null && state.emplacement_ids.Count > 0)
            {
                int total = 0;
                int count = 0;
                for (int i = 0; i < state.emplacement_ids.Count; i++)
                {
                    var emplacement = _perimeter.FindEmplacement(state.emplacement_ids[i]);
                    if (emplacement == null || emplacement.max_hp <= 0 || !emplacement.is_active) continue;
                    total += (int)((long)Math.Max(0, emplacement.current_hp) * 1000 / emplacement.max_hp);
                    count++;
                }
                if (count > 0) return total / count;
            }
            return _perimeter.GetAverageWatchPostCondition(sector);
        }

        private bool CalculateAlarmOnline(string sector)
        {
            var state = _perimeter.FindSector(sector);
            return state != null && state.alarm_armed && !state.alarm_spent;
        }

        private int CalculateGateDrillRecency()
        {
            if (_catalog == null) return 0;
            int recency = 0;
            string[] ids = { "watch_drill_gate_lock", "watch_drill_alarm_bell", "watch_drill_intruder_call" };
            for (int i = 0; i < ids.Length; i++)
                recency = Math.Max(recency, _perimeter.GetWatchDrillRecencyPermille(ids[i], _currentDay));
            return recency;
        }

        private NightWatchAlarmProtocolDefinition? SelectAlarmProtocol(int confidencePermille)
        {
            if (_catalog == null || confidencePermille <= 0) return null;
            return _catalog.alarm_protocols
                .Where(x => x != null && x.trigger_confidence_permille > 0 &&
                            x.trigger_confidence_permille <= confidencePermille)
                .OrderByDescending(x => x.trigger_confidence_permille)
                .FirstOrDefault();
        }

        private void WriteJournal(string key, string text)
        {
            JournalWriter?.Invoke(key, text, _currentDay);
        }

        private static string NormalizeSector(string sectorId)
        {
            if (string.IsNullOrWhiteSpace(sectorId)) return "gate";
            string value = sectorId.Trim().ToLowerInvariant();
            return NightWatchOperationsCatalogLoader.Sectors.Contains(value) ? value : "gate";
        }

        private static int SectorPerimeterMetres(string sectorId) => sectorId switch
        {
            "north" => 1800,
            "east" => 1600,
            "south" => 1400,
            "west" => 2000,
            _ => 1000
        };

        private static string EstimateSector(float bearing)
        {
            float normalized = bearing % 360f;
            if (normalized < 0f) normalized += 360f;
            if (normalized >= 315f || normalized < 45f) return "north";
            if (normalized < 135f) return "east";
            if (normalized < 225f) return "south";
            return "west";
        }
    }
}
