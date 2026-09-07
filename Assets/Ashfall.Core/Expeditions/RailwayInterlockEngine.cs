// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 117 — Railway Interlock, Signal & Route-Control Automation.
//
// AUTHORITY MAP (Flagship Plans 114-117, Wave 3):
//   * RailwaySystem (Ashfall.Core.Expeditions) and the canonical
//     rail_network.json topology remain the ONLY owners of nodes,
//     segments, trains, dispatch and pathfinding. This engine NEVER
//     moves expeditions and NEVER duplicates topology — it stores
//     junction ids and route (segment) ids as references into the
//     canonical catalog and validates them against it.
//   * This engine owns: switch state, route locks, signal aspects,
//     reservations, obstruction/maintenance, tamper abstraction and the
//     fictionalized route-denial device (armed/triggered/spent — resolves
//     through encounter tags, never through any real-world derailment
//     mechanism).
//   * Downstream consumers read the published RailNetworkAvailability
//     snapshot; they decide travel. The engine only reports legality,
//     restrictions and modifiers.
//   * No wall clock: campaign day comes from the host DayProvider.
//   * No System.Random: all stochastic outcomes flow through ISeededRng.
//   * Weather obstruction accumulates from a host-provided abstract
//     severity (0..1) — no real-world meteorology.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Expeditions
{
    // ─────────────────────────────────────────────────────────────────
    // Catalog DTOs (data authority: Assets/StreamingAssets/Data/
    // railway_interlock_catalog.json)
    // ─────────────────────────────────────────────────────────────────

    /// <summary>One controlled junction guarding a set of canonical routes.</summary>
    [Serializable]
    public sealed class RailwayJunctionDef
    {
        public string junction_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        /// <summary>Reference into the canonical rail_network.json node ids.</summary>
        public string map_node_id { get; set; } = string.Empty;
        /// <summary>All canonical segment ids reachable through this junction.</summary>
        public List<string> connected_route_ids { get; set; } = new List<string>();
        public string main_route_id { get; set; } = string.Empty;
        public List<string> siding_route_ids { get; set; } = new List<string>();
        /// <summary>mechanical | relay | lever_frame — abstract interlock class.</summary>
        public string interlock_type { get; set; } = "mechanical";
        public string maintenance_profile_id { get; set; } = string.Empty;
        /// <summary>0..1 — how strongly weather severity drives obstruction.</summary>
        public float weather_sensitivity { get; set; } = 0.5f;
        public string signal_profile_id { get; set; } = string.Empty;
        /// <summary>Items required to restore/activate this junction's interlock.</summary>
        public Dictionary<string, int> unlock_requirements { get; set; } = new Dictionary<string, int>();
        public List<string> tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class RailwayMaintenanceProfile
    {
        public string maintenance_profile_id { get; set; } = string.Empty;
        public Dictionary<string, int> required_items { get; set; } = new Dictionary<string, int>();
        /// <summary>Obstruction accumulation per unit of weather severity per day.</summary>
        public float obstruction_per_severity_day { get; set; } = 0.05f;
        /// <summary>Natural obstruction clearing per fair-weather day.</summary>
        public float obstruction_decay_per_day { get; set; } = 0.02f;
    }

    [Serializable]
    public sealed class RailwayInterlockCatalog
    {
        public int schema_version { get; set; } = 1;
        public string network_id { get; set; } = "rail_interlock_primary";
        public string display_name { get; set; } = "Corridor Signal & Interlock Grid";
        public List<RailwayJunctionDef> junctions { get; set; } = new List<RailwayJunctionDef>();
        public List<RailwayMaintenanceProfile> maintenance_profiles { get; set; } = new List<RailwayMaintenanceProfile>();
        public List<string> tags { get; set; } = new List<string>();
    }

    // ─────────────────────────────────────────────────────────────────
    // State DTOs
    // ─────────────────────────────────────────────────────────────────

    [Serializable]
    public sealed class RailwayJunctionState
    {
        public string junction_id { get; set; } = string.Empty;
        public string selected_route_id { get; set; } = string.Empty;
        /// <summary>aligned | reversing | misaligned</summary>
        public string switch_state { get; set; } = "aligned";
        /// <summary>unlocked | locked</summary>
        public string lock_state { get; set; } = "unlocked";
        /// <summary>clear | caution | stop | fault</summary>
        public string signal_state { get; set; } = "stop";
        /// <summary>nominal | degraded | overdue</summary>
        public string maintenance_state { get; set; } = "nominal";
        /// <summary>clear | obstructed | closed</summary>
        public string weather_obstruction_state { get; set; } = "clear";
        /// <summary>0..1 obstruction accumulation (drives the state above).</summary>
        public float obstruction_level { get; set; } = 0f;
        /// <summary>none | suspected | detected</summary>
        public string tamper_risk_state { get; set; } = "none";
        public string route_reserved_by_expedition_id { get; set; } = string.Empty;
        public int last_inspection_day { get; set; } = -1;
        /// <summary>Fictionalized route-denial device: none | armed | triggered | spent.</summary>
        public string denial_device_state { get; set; } = "none";
        /// <summary>Days of overdue maintenance accumulated (drives degraded/overdue).</summary>
        public int days_since_maintenance { get; set; } = 0;
    }

    [Serializable]
    public sealed class RailwayInterlockState
    {
        public int schema_version { get; set; } = 1;
        public string network_id { get; set; } = string.Empty;
        /// <summary>Junctions restored/activated by the player; absent = uncontrolled (default stop).</summary>
        public Dictionary<string, RailwayJunctionState> junctions { get; set; } = new Dictionary<string, RailwayJunctionState>(StringComparer.Ordinal);
        public int next_reservation_number { get; set; } = 1;
        public int days_monitored { get; set; } = 0;
    }

    // ─────────────────────────────────────────────────────────────────
    // Bridge DTOs
    // ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Published availability snapshot consumed by map/expedition authorities.
    /// Pure projection — safe to recompute any time.
    /// </summary>
    [Serializable]
    public sealed class RailNetworkAvailability
    {
        public List<string> open_route_ids { get; set; } = new List<string>();
        public List<string> blocked_route_ids { get; set; } = new List<string>();
        /// <summary>Passable with penalty (obstruction, caution signal).</summary>
        public List<string> restricted_route_ids { get; set; } = new List<string>();
        /// <summary>Per-route travel-time multipliers (>1 = slower). Only non-1.0 entries included.</summary>
        public Dictionary<string, float> travel_time_modifiers { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);
        /// <summary>Fault/tamper/obstruction tags affecting decisions.</summary>
        public List<string> advisory_tags { get; set; } = new List<string>();
        /// <summary>0..1 aggregate health of the controlled grid.</summary>
        public float grid_confidence { get; set; } = 1f;
    }

    /// <summary>Typed failure codes for the Godot host to format.</summary>
    public static class RailwayInterlockFailures
    {
        public const string UnknownJunction = "railix.unknown_junction";
        public const string UnknownRoute = "railix.unknown_route";
        public const string RouteNotAtJunction = "railix.route_not_at_junction";
        public const string JunctionNotRestored = "railix.junction_not_restored";
        public const string RouteClosed = "railix.route_closed";
        public const string RouteObstructed = "railix.route_obstructed";
        public const string ConflictingReservation = "railix.conflicting_reservation";
        public const string NotReservationOwner = "railix.not_reservation_owner";
        public const string NoReservation = "railix.no_reservation";
        public const string TamperDetected = "railix.tamper_detected";
        public const string MaterialsMissing = "railix.materials_missing";
        public const string AlreadyRestored = "railix.already_restored";
        public const string SignalFault = "railix.signal_fault";
    }

    // ─────────────────────────────────────────────────────────────────
    // Engine
    // ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Plan 117 interlock engine. Deterministic boolean route constraints:
    /// a route can only be cleared when the junction is restored, switches
    /// align, the lock succeeds, no conflicting reservation exists and the
    /// junction is operational. Same junction state + same request ⇒ same
    /// outcome, always.
    /// </summary>
    public sealed class RailwayInterlockEngine
    {
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private RailwayInterlockCatalog _catalog = new RailwayInterlockCatalog();
        private RailwayInterlockState _state = new RailwayInterlockState();

        // Inventory ports (host-bound; deterministic).
        private Func<string, int> _getCount = _ => 0;
        private Action<string, int> _consume = (_, _) => { };

        /// <summary>Host-injected campaign day provider (deterministic).</summary>
        public Func<int>? DayProvider { get; set; }
        /// <summary>Host-injected weather severity 0..1 (host-owned weather authority).</summary>
        public Func<float>? WeatherSeverityProvider { get; set; }
        /// <summary>Signalman skill 0..1 — earlier tamper warning, fault recovery.</summary>
        public Func<float>? SignalmanSkillProvider { get; set; }
        /// <summary>Track-inspector skill 0..1 — maintenance effectiveness, obstruction resistance.</summary>
        public Func<float>? TrackInspectorSkillProvider { get; set; }

        private int CurrentDay() => DayProvider?.Invoke() ?? 0;
        private float WeatherSeverity() => Math.Clamp(WeatherSeverityProvider?.Invoke() ?? 0f, 0f, 1f);
        private float SignalmanSkill() => Math.Clamp(SignalmanSkillProvider?.Invoke() ?? 0f, 0f, 1f);
        private float TrackInspectorSkill() => Math.Clamp(TrackInspectorSkillProvider?.Invoke() ?? 0f, 0f, 1f);

        public RailwayInterlockState State => _state;
        public RailwayInterlockCatalog Catalog => _catalog;

        /// <summary>Raised when a triggered route-denial device closes a route —
        /// the encounter/combat authority resolves the consequence.</summary>
        public event Action<string, string>? OnRouteDenialTriggered; // junctionId, routeId
        public event Action<string>? OnEventRaised;

        public RailwayInterlockEngine(ISeededRng? rng = null, ILog? log = null)
        {
            _rng = rng ?? new SeededRng(2117);
            _log = log ?? NullLog.Instance;
        }

        public void BindCatalog(RailwayInterlockCatalog catalog)
        {
            if (catalog != null) _catalog = catalog;
        }

        public void BindInventory(Func<string, int> getCount, Action<string, int> consume)
        {
            _getCount = getCount ?? _getCount;
            _consume = consume ?? _consume;
        }

        private RailwayJunctionDef? FindJunction(string junctionId)
        {
            foreach (var j in _catalog.junctions)
                if (j.junction_id == junctionId) return j;
            return null;
        }

        private RailwayMaintenanceProfile? FindProfile(string? profileId)
        {
            foreach (var p in _catalog.maintenance_profiles)
                if (p.maintenance_profile_id == profileId) return p;
            return _catalog.maintenance_profiles.Count > 0 ? _catalog.maintenance_profiles[0] : null;
        }

        private RailwayJunctionState GetOrCreateJunctionState(string junctionId)
        {
            if (_state.junctions.TryGetValue(junctionId, out var s)) return s;
            var fresh = new RailwayJunctionState { junction_id = junctionId };
            _state.junctions[junctionId] = fresh;
            return fresh;
        }

        private bool Restored(string junctionId) => _state.junctions.ContainsKey(junctionId);

        // ── Restoration ─────────────────────────────────────────────────

        /// <summary>
        /// Restores a junction's interlock (pays unlock requirements). Old
        /// saves default to uncontrolled junctions — signals sit at stop
        /// and routes through them stay blocked until restored (invariant
        /// 15: no invented degradation, no randomized faults).
        /// </summary>
        public ActionResult RestoreJunction(string junctionId)
        {
            var def = FindJunction(junctionId);
            if (def == null)
                return ActionResult.Failed(RailwayInterlockFailures.UnknownJunction, "railix.unknown_junction");
            if (Restored(junctionId))
                return ActionResult.Blocked(RailwayInterlockFailures.AlreadyRestored, "railix.already_restored");

            foreach (var cost in def.unlock_requirements)
            {
                if (_getCount(cost.Key) < cost.Value)
                    return ActionResult.Blocked(RailwayInterlockFailures.MaterialsMissing, "railix.missing_unlock_material");
            }
            foreach (var cost in def.unlock_requirements)
                _consume(cost.Key, cost.Value);

            var state = GetOrCreateJunctionState(junctionId);
            state.junction_id = junctionId;
            state.last_inspection_day = CurrentDay();
            state.signal_state = "stop";
            state.switch_state = "aligned";
            Raise("railix.junction_restored");
            _log.Info($"[RailInterlock] Junction {junctionId} restored to control.");
            return ActionResult.Success("railix.junction_restored");
        }

        // ── Route requests / locks / reservations ───────────────────────

        /// <summary>
        /// The core interlock invariant as one deterministic predicate.
        /// A route can be cleared only when: the junction is restored, the
        /// route belongs to the junction, switches can align, no conflicting
        /// reservation exists, and the junction is operational (no closure,
        /// tamper hold or signal fault).
        /// </summary>
        public bool CanClearRoute(string junctionId, string routeId)
        {
            var def = FindJunction(junctionId);
            if (def == null) return false;
            if (!def.connected_route_ids.Contains(routeId)) return false;
            if (!Restored(junctionId)) return false;
            var state = _state.junctions[junctionId];

            if (state.weather_obstruction_state == "closed") return false;
            if (state.tamper_risk_state == "detected") return false;
            if (state.signal_state == "fault") return false;
            if (state.denial_device_state == "triggered") return false;
            if (state.weather_obstruction_state == "obstructed" && routeId != def.main_route_id) return false;
            if (!string.IsNullOrEmpty(state.route_reserved_by_expedition_id)) return false;
            return true;
        }

        /// <summary>
        /// Requests a route: validates, locks the junction, aligns the switch,
        /// sets the signal and records the reservation. Deterministic — same
        /// state and request always produce the same outcome.
        /// </summary>
        public ActionResult RequestRoute(string junctionId, string routeId, string expeditionId)
        {
            var def = FindJunction(junctionId);
            if (def == null)
                return ActionResult.Failed(RailwayInterlockFailures.UnknownJunction, "railix.unknown_junction");
            if (!def.connected_route_ids.Contains(routeId))
                return ActionResult.Failed(RailwayInterlockFailures.RouteNotAtJunction, "railix.route_not_at_junction");
            if (!Restored(junctionId))
                return ActionResult.Blocked(RailwayInterlockFailures.JunctionNotRestored, "railix.junction_not_restored");

            var state = _state.junctions[junctionId];

            if (!string.IsNullOrEmpty(state.route_reserved_by_expedition_id) &&
                state.route_reserved_by_expedition_id != expeditionId)
                return ActionResult.Blocked(RailwayInterlockFailures.ConflictingReservation, "railix.conflicting_reservation");
            if (state.weather_obstruction_state == "closed")
                return ActionResult.Blocked(RailwayInterlockFailures.RouteClosed, "railix.route_closed");
            if (state.tamper_risk_state == "detected")
                return ActionResult.Blocked(RailwayInterlockFailures.TamperDetected, "railix.tamper_detected");
            if (state.signal_state == "fault")
                return ActionResult.Blocked(RailwayInterlockFailures.SignalFault, "railix.signal_fault");
            if (state.denial_device_state == "triggered")
                return ActionResult.Blocked(RailwayInterlockFailures.RouteClosed, "railix.route_closed");

            bool sidingRequested = def.siding_route_ids.Contains(routeId);
            if (state.weather_obstruction_state == "obstructed" && sidingRequested)
                return ActionResult.Blocked(RailwayInterlockFailures.RouteObstructed, "railix.route_obstructed");

            state.route_reserved_by_expedition_id = expeditionId;
            state.selected_route_id = routeId;
            state.switch_state = "aligned";
            state.lock_state = "locked";
            state.signal_state = sidingRequested && state.weather_obstruction_state == "obstructed" ? "caution" : "clear";

            Raise("railix.route_reserved");
            return ActionResult.Success("railix.route_reserved");
        }

        /// <summary>
        /// Releases a reservation after passage. Only the owning expedition
        /// (or a forced inspection) may release — prevents deadlocks without
        /// allowing foreign takedowns.
        /// </summary>
        public ActionResult ReleaseRoute(string junctionId, string expeditionId)
        {
            if (!FindJunction(junctionId).HasValueSafe())
                return ActionResult.Failed(RailwayInterlockFailures.UnknownJunction, "railix.unknown_junction");
            if (!_state.junctions.TryGetValue(junctionId, out var state))
                return ActionResult.Blocked(RailwayInterlockFailures.NoReservation, "railix.no_reservation");
            if (string.IsNullOrEmpty(state.route_reserved_by_expedition_id))
                return ActionResult.Blocked(RailwayInterlockFailures.NoReservation, "railix.no_reservation");
            if (state.route_reserved_by_expedition_id != expeditionId)
                return ActionResult.Blocked(RailwayInterlockFailures.NotReservationOwner, "railix.not_reservation_owner");

            state.route_reserved_by_expedition_id = string.Empty;
            state.selected_route_id = string.Empty;
            state.lock_state = "unlocked";
            state.signal_state = state.weather_obstruction_state == "closed" || state.tamper_risk_state == "detected"
                ? "stop"
                : "caution";
            Raise("railix.route_released");
            return ActionResult.Success("railix.route_released");
        }

        // ── Availability snapshot ───────────────────────────────────────

        /// <summary>
        /// Publishes the availability snapshot for map/expedition
        /// authorities. Unrestored junctions keep their routes blocked
        /// (signals at stop) — restoration is the player-facing act of
        /// opening the corridor.
        /// </summary>
        public RailNetworkAvailability BuildAvailability()
        {
            var snapshot = new RailNetworkAvailability();
            float healthSum = 0f;
            int counted = 0;

            foreach (var def in _catalog.junctions)
            {
                bool restored = _state.junctions.TryGetValue(def.junction_id, out var state);
                if (!restored)
                {
                    foreach (var route in def.connected_route_ids)
                        if (!snapshot.blocked_route_ids.Contains(route)) snapshot.blocked_route_ids.Add(route);
                    continue;
                }

                var s = _state.junctions[def.junction_id];
                // Health abstraction: obstruction, maintenance, tamper, faults.
                float health = 1f
                    - s.obstruction_level * 0.4f
                    - (s.maintenance_state == "overdue" ? 0.25f : s.maintenance_state == "degraded" ? 0.1f : 0f)
                    - (s.tamper_risk_state == "detected" ? 0.4f : s.tamper_risk_state == "suspected" ? 0.15f : 0f)
                    - (s.signal_state == "fault" ? 0.3f : 0f);
                healthSum += Math.Clamp(health, 0f, 1f);
                counted++;

                foreach (var route in def.connected_route_ids)
                {
                    // A locked reservation with this route selected is a
                    // cleared route for its owner — project as open.
                    bool reservedAndCleared = s.lock_state == "locked"
                                              && s.selected_route_id == route
                                              && !string.IsNullOrEmpty(s.route_reserved_by_expedition_id);
                    bool clear = reservedAndCleared || CanClearRoute(def.junction_id, route);
                    if (clear && !snapshot.open_route_ids.Contains(route))
                        snapshot.open_route_ids.Add(route);

                    bool routeIsSiding = def.siding_route_ids.Contains(route);
                    if (!clear)
                    {
                        // Tamper/closure blocks outright; mere siding obstruction
                        // restricts rather than hard-blocks the mainline mesh.
                        if (s.weather_obstruction_state == "closed" || s.tamper_risk_state == "detected"
                            || s.denial_device_state == "triggered" || s.signal_state == "fault")
                        {
                            if (!snapshot.blocked_route_ids.Contains(route))
                                snapshot.blocked_route_ids.Add(route);
                        }
                        else if (!snapshot.restricted_route_ids.Contains(route) && !snapshot.blocked_route_ids.Contains(route))
                            snapshot.restricted_route_ids.Add(route);
                    }
                    else if (routeIsSiding && s.weather_obstruction_state == "obstructed")
                    {
                        if (!snapshot.restricted_route_ids.Contains(route))
                            snapshot.restricted_route_ids.Add(route);
                    }

                    // Travel-time modifiers: obstruction and caution signals slow trains.
                    float modifier = 1f;
                    if (s.weather_obstruction_state == "obstructed") modifier += 0.25f;
                    if (s.signal_state == "caution") modifier += 0.1f;
                    if (s.maintenance_state == "overdue") modifier += 0.15f;
                    if (modifier > 1.001f)
                        snapshot.travel_time_modifiers[route] = MathF.Max(
                            snapshot.travel_time_modifiers.TryGetValue(route, out var existing) ? existing : 1f, modifier);
                }

                if (s.tamper_risk_state != "none" && !snapshot.advisory_tags.Contains($"tamper_{s.tamper_risk_state}"))
                    snapshot.advisory_tags.Add($"tamper_{s.tamper_risk_state}");
                if (s.weather_obstruction_state != "clear" && !snapshot.advisory_tags.Contains($"obstruction_{s.junction_id}"))
                    snapshot.advisory_tags.Add($"obstruction_{s.junction_id}");
                if (s.denial_device_state == "armed" && !snapshot.advisory_tags.Contains($"denial_armed_{s.junction_id}"))
                    snapshot.advisory_tags.Add($"denial_armed_{s.junction_id}");
            }

            snapshot.grid_confidence = counted > 0 ? Math.Clamp(healthSum / counted, 0f, 1f) : 0f;
            return snapshot;
        }

        /// <summary>
        /// Validates an expedition path (canonical segment-id sequence)
        /// against the current availability — a path is legal only when
        /// every segment is an open or restricted interlock route (or not
        /// interlock-controlled at all).
        /// </summary>
        public bool IsExpeditionPathLegal(IReadOnlyList<string> segmentIds)
        {
            var snapshot = BuildAvailability();
            foreach (var seg in segmentIds)
            {
                if (snapshot.blocked_route_ids.Contains(seg)) return false;
            }
            return true;
        }

        // ── Daily tick ──────────────────────────────────────────────────

        /// <summary>Daily junction update: weather obstruction, maintenance
        /// drift, tamper risk. Deterministic given state, day and seed.</summary>
        public void TickDay(int day)
        {
            _state.days_monitored++;
            float severity = WeatherSeverity();
            float inspector = TrackInspectorSkill();
            float signalman = SignalmanSkill();

            foreach (var def in _catalog.junctions)
            {
                // Unrestored junctions stay uncontrolled — no drift, no
                // invented faults (invariant 15).
                if (!Restored(def.junction_id))
                    continue;

                var profile = FindProfile(def.maintenance_profile_id);
                var state = GetOrCreateJunctionState(def.junction_id);
                state.junction_id = def.junction_id;

                // Obstruction: weather-driven accumulation, fair-weather decay.
                float resist = 1f - 0.3f * inspector;
                float accumulation = severity * def.weather_sensitivity * (profile?.obstruction_per_severity_day ?? 0.05f) * resist;
                float decay = severity < 0.2f ? (profile?.obstruction_decay_per_day ?? 0.02f) : 0f;
                state.obstruction_level = Math.Clamp(state.obstruction_level + accumulation - decay, 0f, 1f);
                state.weather_obstruction_state = state.obstruction_level >= 0.95f
                    ? "closed"
                    : state.obstruction_level >= 0.4f ? "obstructed" : "clear";

                // Maintenance drift.
                state.days_since_maintenance++;
                state.maintenance_state = state.days_since_maintenance >= 12 ? "overdue"
                    : state.days_since_maintenance >= 6 ? "degraded" : "nominal";

                // Tamper risk: seeded roll, suppressed by signalman vigilance.
                if (state.tamper_risk_state == "none")
                {
                    int rollBp = _rng.Next(0, 10000);
                    int thresholdBp = (int)(25 * (1f - 0.5f * signalman) * (state.maintenance_state == "overdue" ? 1.5f : 1f));
                    if (rollBp < thresholdBp)
                    {
                        state.tamper_risk_state = "suspected";
                        Raise("railix.tamper_suspected");
                    }
                }

                // Triggered denial devices hold until inspected.
                if (state.denial_device_state == "triggered")
                {
                    state.signal_state = "stop";
                    state.lock_state = "unlocked";
                    state.route_reserved_by_expedition_id = string.Empty;
                }
                else if (state.signal_state != "fault" && string.IsNullOrEmpty(state.route_reserved_by_expedition_id))
                {
                    // Idle junction shows stop until a route is requested.
                    state.signal_state = "stop";
                }
            }
        }

        // ── Inspection & maintenance ────────────────────────────────────

        /// <summary>
        /// Inspects a junction: confirms or clears suspected tamper, recovers
        /// signal faults, spends a triggered denial device (encounter tag
        /// raised at trigger time — resolution belongs to the encounter
        /// authority). Deterministic.
        /// </summary>
        public ActionResult InspectJunction(string junctionId)
        {
            var def = FindJunction(junctionId);
            if (def == null)
                return ActionResult.Failed(RailwayInterlockFailures.UnknownJunction, "railix.unknown_junction");
            if (!Restored(junctionId))
                return ActionResult.Blocked(RailwayInterlockFailures.JunctionNotRestored, "railix.junction_not_restored");

            var state = _state.junctions[junctionId];
            state.last_inspection_day = CurrentDay();

            if (state.tamper_risk_state == "suspected" || state.tamper_risk_state == "detected")
            {
                state.tamper_risk_state = "none";
                Raise("railix.tamper_cleared");
            }
            if (state.signal_state == "fault")
            {
                state.signal_state = "stop";
                Raise("railix.signal_recovered");
            }
            if (state.denial_device_state == "triggered")
            {
                state.denial_device_state = "spent";
                Raise("railix.denial_device_spent");
            }
            Raise("railix.junction_inspected");
            return ActionResult.Success("railix.junction_inspected");
        }

        /// <summary>Clears weather obstruction (labor/equipment abstraction).</summary>
        public ActionResult ClearJunctionObstruction(string junctionId)
        {
            var def = FindJunction(junctionId);
            if (def == null)
                return ActionResult.Failed(RailwayInterlockFailures.UnknownJunction, "railix.unknown_junction");
            if (!Restored(junctionId))
                return ActionResult.Blocked(RailwayInterlockFailures.JunctionNotRestored, "railix.junction_not_restored");

            var state = _state.junctions[junctionId];
            if (state.obstruction_level <= 0.01f)
                return ActionResult.Blocked(RailwayInterlockFailures.MaterialsMissing, "railix.no_obstruction");

            var profile = FindProfile(def.maintenance_profile_id);
            foreach (var cost in profile?.required_items ?? new Dictionary<string, int>())
            {
                if (_getCount(cost.Key) < cost.Value)
                    return ActionResult.Blocked(RailwayInterlockFailures.MaterialsMissing, "railix.missing_maintenance_material");
            }
            foreach (var cost in profile?.required_items ?? new Dictionary<string, int>())
                _consume(cost.Key, cost.Value);

            float driller = 0.5f + 0.5f * TrackInspectorSkill();
            state.obstruction_level = Math.Max(0f, state.obstruction_level - driller);
            state.weather_obstruction_state = state.obstruction_level >= 0.95f ? "closed"
                : state.obstruction_level >= 0.4f ? "obstructed" : "clear";
            Raise("railix.obstruction_cleared");
            return ActionResult.Success("railix.obstruction_cleared");
        }

        /// <summary>Full maintenance: clears obstruction, resets maintenance drift.</summary>
        public ActionResult MaintainJunction(string junctionId)
        {
            var result = ClearJunctionObstruction(junctionId);
            if (result.Status == ActionResult.StatusKind.Failed ||
                result.Status == ActionResult.StatusKind.Blocked && result.FailureCode != RailwayInterlockFailures.MaterialsMissing)
                return result;

            var state = _state.junctions[junctionId];
            state.days_since_maintenance = 0;
            state.maintenance_state = "nominal";
            Raise("railix.junction_maintained");
            return ActionResult.Success("railix.junction_maintained");
        }

        // ── Route-denial device (fictionalized control device) ──────────

        /// <summary>
        /// Arms the abstract route-denial device at a junction. Triggering
        /// resolves through the encounter/combat authority (event) — never
        /// through any operationally descriptive mechanism.
        /// </summary>
        public ActionResult ArmRouteDenial(string junctionId)
        {
            if (!FindJunction(junctionId).HasValueSafe())
                return ActionResult.Failed(RailwayInterlockFailures.UnknownJunction, "railix.unknown_junction");
            if (!Restored(junctionId))
                return ActionResult.Blocked(RailwayInterlockFailures.JunctionNotRestored, "railix.junction_not_restored");
            var state = _state.junctions[junctionId];
            if (state.denial_device_state != "none" && state.denial_device_state != "spent")
                return ActionResult.Blocked(RailwayInterlockFailures.AlreadyRestored, "railix.device_already_set");

            state.denial_device_state = "armed";
            Raise("railix.denial_device_armed");
            return ActionResult.Success("railix.denial_device_armed");
        }

        /// <summary>
        /// Detonates/spends the armed device when a hostile move is declared
        /// by the encounter authority. Deterministic state transition only;
        /// consequence resolution is external.
        /// </summary>
        public ActionResult TriggerRouteDenial(string junctionId)
        {
            if (!_state.junctions.TryGetValue(junctionId, out var state))
                return ActionResult.Failed(RailwayInterlockFailures.UnknownJunction, "railix.unknown_junction");
            if (state.denial_device_state != "armed")
                return ActionResult.Blocked(RailwayInterlockFailures.NoReservation, "railix.device_not_armed");

            var def = FindJunction(junctionId);
            state.denial_device_state = "triggered";
            state.signal_state = "stop";
            state.lock_state = "unlocked";
            string route = state.selected_route_id.Length > 0 ? state.selected_route_id : def?.main_route_id ?? string.Empty;
            state.route_reserved_by_expedition_id = string.Empty;
            OnRouteDenialTriggered?.Invoke(junctionId, route);
            Raise("railix.denial_device_triggered");
            return ActionResult.Success("railix.denial_device_triggered");
        }

        // ── Persistence ─────────────────────────────────────────────────

        public RailwayInterlockState CaptureState()
        {
            var clone = new RailwayInterlockState
            {
                schema_version = _state.schema_version,
                network_id = _state.network_id,
                next_reservation_number = _state.next_reservation_number,
                days_monitored = _state.days_monitored,
                junctions = new Dictionary<string, RailwayJunctionState>(_state.junctions, StringComparer.Ordinal)
            };
            // Defensive deep copy.
            var deep = new Dictionary<string, RailwayJunctionState>(StringComparer.Ordinal);
            foreach (var kv in _state.junctions)
            {
                var s = kv.Value;
                deep[kv.Key] = new RailwayJunctionState
                {
                    junction_id = s.junction_id,
                    selected_route_id = s.selected_route_id,
                    switch_state = s.switch_state,
                    lock_state = s.lock_state,
                    signal_state = s.signal_state,
                    maintenance_state = s.maintenance_state,
                    weather_obstruction_state = s.weather_obstruction_state,
                    obstruction_level = s.obstruction_level,
                    tamper_risk_state = s.tamper_risk_state,
                    route_reserved_by_expedition_id = s.route_reserved_by_expedition_id,
                    last_inspection_day = s.last_inspection_day,
                    denial_device_state = s.denial_device_state,
                    days_since_maintenance = s.days_since_maintenance
                };
            }
            clone.junctions = deep;
            return clone;
        }

        public void RestoreState(RailwayInterlockState? state)
        {
            if (state == null) return; // Old saves: uncontrolled grid, no faults invented.
            _state = state;
            if (_state.junctions == null) _state.junctions = new Dictionary<string, RailwayJunctionState>(StringComparer.Ordinal);
            if (_state.schema_version < 1 || _state.schema_version > 1)
                _state.schema_version = 1;
            // Active reservations survive reload by design (invariant: no
            // reservation loss after load; release after passage is explicit).
        }

        private void Raise(string eventId) => OnEventRaised?.Invoke(eventId);
    }

    internal static class RailwayInterlockExtensions
    {
        public static bool HasValueSafe(this RailwayJunctionDef? def) => def != null;
    }
}
