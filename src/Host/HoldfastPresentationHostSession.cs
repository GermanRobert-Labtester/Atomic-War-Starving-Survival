// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 51 / Holdfast presentation slate host adapter.
//
// Authority boundary (deliberate): HoldfastPresentationSlate stays the single
// presentation read model (room/actor/map projections, hazard bands, crisis
// band, motion profile). This session only *composes* it from the existing
// gameplay owners — it never stores or duplicates game state:
//   room power      -> PowerGridSystem.IsRoomServed
//   room temperature-> ShelterThermalSystem thermal room nodes
//   room occupants  -> ShelterAssignmentSystem assignments
//   room flooding   -> SumpFloodingSystem sump nodes (the shelter's only
//                      canonical flood truth; drainage nodes project as
//                      infrastructure rooms, no invented room-level flooding)
//   actor stance    -> DutyRosterSystem status vocabulary
//   actor grief     -> MemorialSystem entries inside the mourning window
//   map knowledge   -> WastelandMapSystem nodes/routes + fog knowledge
//   motion profile  -> the caller's accessibility reduce-motion setting
//
// A projection is admitted only if its category has an active world; without
// it the plan facets report their zero counts instead of fabricating data.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.DutyRoster;
using Ashfall.Core.Memorial;
using Ashfall.Core.Presentation;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Canonical owners a slate projection reads. Every source is optional: a
    /// missing owner leaves the corresponding projection empty (never invented).
    /// </summary>
    public sealed class PresentationSources
    {
        public SurvivorsHostSession? Survivors { get; set; }
        public ShelterAssignmentHostSession? Assignments { get; set; }
        public PowerGridSystem? Power { get; set; }
        public ShelterThermalSystem? Thermal { get; set; }
        public SumpFloodingSystem? Sump { get; set; }
        public WastelandMapSystem? Map { get; set; }
        public MemorialSystem? Memorial { get; set; }
        public DutyRosterHostSession? DutyRoster { get; set; }
        public Func<string, string>? OccupationFor { get; set; }
        public int CurrentDay { get; set; } = 1;
        public bool ReduceMotion { get; set; }
    }

    public sealed class HoldfastPresentationHostSession : HostSessionBase
    {
        /// <summary>Days a memorial loss still marks the survivor visually.</summary>
        public const int MourningWindowDays = 3;

        public HoldfastPresentationSlate Slate { get; private set; } = new HoldfastPresentationSlate();

        /// <summary>Rooms projected from the live owners (rooms + drainage nodes).</summary>
        public int ProjectedRoomCount { get; private set; }

        /// <summary>Actors projected from the live roster.</summary>
        public int ProjectedActorCount { get; private set; }

        /// <summary>Map nodes projected from the live wasteland map.</summary>
        public int ProjectedNodeCount { get; private set; }

        public bool IsMapVisible =>
            Slate.MapNodes.Any(n => n.Knowledge != MapKnowledgeVisualRung.Unknown);

        public bool IsMapEmpty => ProjectedNodeCount == 0;

        /// <summary>
        /// Rebuild the presentation slate from the current canonical owners. A
        /// fresh slate is composed each call so no stale projection survives.
        /// </summary>
        public HoldfastPresentationSlate Compose(PresentationSources sources)
        {
            if (sources == null) throw new ArgumentNullException(nameof(sources));

            var slate = new HoldfastPresentationSlate
            {
                MotionProfile = new PresentationMotionProfile(sources.ReduceMotion)
            };

            ProjectRooms(slate, sources);
            ProjectActors(slate, sources);
            ProjectMap(slate, sources);

            slate.ReevaluateCrisisBand();

            Slate = slate;
            ProjectedRoomCount = slate.Rooms.Count;
            ProjectedActorCount = slate.Actors.Count;
            ProjectedNodeCount = slate.MapNodes.Count;
            RaiseStateChanged();
            return slate;
        }

        private static void ProjectRooms(HoldfastPresentationSlate slate, PresentationSources sources)
        {
            // ── Shelter rooms from the assignment owner ──
            if (sources.Assignments?.System != null)
            {
                foreach (var room in sources.Assignments.System.Rooms)
                {
                    if (room == null || string.IsNullOrWhiteSpace(room.RoomId)) continue;

                    bool powered = sources.Power?.IsRoomServed(room.RoomId) ?? true;
                    float temperature = RoomTemperature(sources, room.RoomId);

                    var snapshot = new RoomPresentationSnapshot(
                        room.RoomId,
                        room.DisplayName ?? room.RoomId,
                        powered,
                        false,
                        temperature,
                        string.Empty);

                    PopulateOccupants(snapshot, sources, room.RoomId);
                    ProjectJob(snapshot, sources, room);
                    slate.AddRoom(snapshot);
                }
            }

            // ── Drainage nodes from the sump owner: the shelter's only
            //    canonical flood truth, projected as infrastructure rooms. ──
            if (sources.Sump?.State?.nodes != null)
            {
                foreach (var node in sources.Sump.State.nodes)
                {
                    if (node == null || string.IsNullOrWhiteSpace(node.nodeId)) continue;

                    slate.AddRoom(new RoomPresentationSnapshot(
                        node.nodeId,
                        string.IsNullOrWhiteSpace(node.displayName) ? node.nodeId : node.displayName,
                        node.pumpPowered,
                        node.isFlooded,
                        NominalTemperature,
                        string.Empty));
                }
            }
        }

        private const float NominalTemperature = 18.0f;

        private static float RoomTemperature(PresentationSources sources, string roomId)
        {
            var nodes = sources.Thermal?.State?.rooms;
            if (nodes == null) return NominalTemperature;
            foreach (var node in nodes)
            {
                if (node != null && string.Equals(node.roomId, roomId, StringComparison.OrdinalIgnoreCase))
                    return node.currentTempC;
            }
            return NominalTemperature;
        }

        private static void PopulateOccupants(RoomPresentationSnapshot snapshot, PresentationSources sources, string roomId)
        {
            var assignments = sources.Assignments?.System?.GetAssignmentsForRoom(roomId);
            if (assignments == null) return;
            foreach (var assignment in assignments)
            {
                if (assignment == null || string.IsNullOrWhiteSpace(assignment.SurvivorId)) continue;
                snapshot.OccupantIds.Add(assignment.SurvivorId);
            }
        }

        /// <summary>
        /// The room's active job label, read from the duty roster's assigned
        /// survivor occupation. Presentation-only; no duty decision is taken.
        /// </summary>
        private static void ProjectJob(RoomPresentationSnapshot snapshot, PresentationSources sources, ShelterRoom room)
        {
            if (sources.Assignments?.System?.GetAssignmentsForRoom(room.RoomId) is not { Count: > 0 } assignments) return;
            foreach (var assignment in assignments)
            {
                if (assignment == null || string.IsNullOrWhiteSpace(assignment.SurvivorId)) continue;
                string occupation = sources.OccupationFor?.Invoke(assignment.SurvivorId) ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(occupation))
                {
                    snapshot.ActiveJobTitle = occupation;
                    return;
                }
            }
        }

        private static void ProjectActors(HoldfastPresentationSlate slate, PresentationSources sources)
        {
            var roster = sources.Survivors?.Roster?.Roster;
            if (roster == null || roster.Count == 0) return;

            foreach (var entry in roster)
            {
                if (entry == null || string.IsNullOrWhiteSpace(entry.survivorId) || !entry.isAlive) continue;

                string roomId = sources.Assignments?.System?.GetAssignmentForSurvivor(entry.survivorId)?.RoomId
                                ?? string.Empty;

                bool grieving = IsInMourning(sources, entry.survivorId);
                var stance = ResolveStance(sources, entry.survivorId, grieving);
                string moraleExpression = MoraleExpression(sources, entry.survivorId);

                var actor = new SurvivorActorPresentationSnapshot(
                    entry.survivorId, roomId, stance, grieving, moraleExpression);
                slate.AddActor(actor);
            }
        }

        private static SurvivorVisualStance ResolveStance(PresentationSources sources, string survivorId, bool grieving)
        {
            if (grieving) return SurvivorVisualStance.Grieving;
            if (sources.DutyRoster?.Roster?.GetRow(survivorId) is { } row)
            {
                // The roster status vocabulary is the canonical stance source;
                // unknown statuses fall back to idling.
                switch (row.status)
                {
                    case DutyRosterIds.StatusLevy:
                    case DutyRosterIds.StatusWaystation:
                        return SurvivorVisualStance.Working;
                    case DutyRosterIds.StatusQuiet:
                        return SurvivorVisualStance.Idling;
                    case DutyRosterIds.StatusMissing:
                        return SurvivorVisualStance.Incapacitated;
                    case DutyRosterIds.StatusDead:
                        return SurvivorVisualStance.Perished;
                    case DutyRosterIds.StatusHome:
                        return SurvivorVisualStance.Sleeping;
                }
            }
            return SurvivorVisualStance.Idling;
        }

        private static bool IsInMourning(PresentationSources sources, string survivorId)
        {
            var entries = sources.Memorial?.Entries;
            if (entries == null) return false;
            int currentDay = Math.Max(1, sources.CurrentDay);
            foreach (var entry in entries)
            {
                if (entry == null || string.IsNullOrWhiteSpace(entry.SurvivorId)) continue;
                if (!string.Equals(entry.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase)) continue;
                if (currentDay - entry.Day <= MourningWindowDays) return true;
            }
            return false;
        }

        private static string MoraleExpression(PresentationSources sources, string survivorId)
        {
            float morale = sources.Survivors?.Needs?.Get(survivorId)?.Morale ?? 50f;
            if (morale <= 20f) return "despairing";
            if (morale <= 40f) return "strained";
            if (morale <= 70f) return "content";
            return "hopeful";
        }

        private static void ProjectMap(HoldfastPresentationSlate slate, PresentationSources sources)
        {
            var map = sources.Map;
            if (map == null) return;

            foreach (var node in map.Nodes)
            {
                if (node == null || string.IsNullOrWhiteSpace(node.Id)) continue;
                slate.AddMapNode(new MapNodePresentationSnapshot(
                    node.Id,
                    string.IsNullOrWhiteSpace(node.DisplayName) ? node.Id : node.DisplayName,
                    MapKnowledgeRung(map, node.Id),
                    Math.Clamp((int)node.Danger + 1, 1, 6),
                    0f,
                    (int)node.PositionX,
                    (int)node.PositionY));
            }

            foreach (var route in map.Routes)
            {
                if (route == null || string.IsNullOrWhiteSpace(route.From) || string.IsNullOrWhiteSpace(route.To)) continue;
                slate.AddMapRoute(new MapRoutePresentationSnapshot(
                    route.From, route.To, route.DistanceKm, route.WeatherHazard, true));
            }
        }

        /// <summary>
        /// Map the canonical fog state onto the presentation knowledge rung.
        /// Undiscovered nodes stay Unknown and are never rendered as known.
        /// </summary>
        private static MapKnowledgeVisualRung MapKnowledgeRung(WastelandMapSystem map, string nodeId)
        {
            MapFogState fog = MapFogState.Unknown;
            foreach (var state in map.Knowledge)
            {
                if (state != null && string.Equals(state.NodeId, nodeId, StringComparison.OrdinalIgnoreCase))
                {
                    fog = state.FogState;
                    break;
                }
            }

            return fog switch
            {
                MapFogState.Unknown => MapKnowledgeVisualRung.Unknown,
                MapFogState.Rumored => MapKnowledgeVisualRung.Rumored,
                MapFogState.Surveyed => MapKnowledgeVisualRung.Scouted,
                MapFogState.Visited => MapKnowledgeVisualRung.Mapped,
                _ => MapKnowledgeVisualRung.Unknown
            };
        }

        public string Describe()
        {
            var slate = Slate;
            return $"holdfast presentation slate: {slate.TotalRooms} room(s) "
                + $"({slate.PoweredRoomsCount} powered, {slate.FloodedRoomsCount} flooded), "
                + $"{slate.Actors.Count} actor(s) ({slate.GrievingSurvivorsCount} grieving), "
                + $"{slate.MapNodes.Count} node(s) ({slate.MapRoutes.Count} route(s)), "
                + $"band {slate.OverallShelterCrisisBand}";
        }
    }
}
