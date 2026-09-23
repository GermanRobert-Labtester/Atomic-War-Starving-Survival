// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 51 host probe.
//
// --holdfast-presentation-selftest proves, headlessly, the presentation slate
// composition over canonical owners: room power/temperature/occupancy from the
// assignment + power + thermal owners, drainage flooding from the sump owner,
// actor stance/room from the roster + duty owner, map knowledge from the
// wasteland map, hazard bands, crisis-band escalation, and the motion
// profile's reduce-motion contract.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.DutyRoster;
using Ashfall.Core.Memorial;
using Ashfall.Core.Presentation;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunHoldfastPresentationSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; GD.Print($"[PASS] holdfast-presentation/{gate}"); }
                else { fail++; GD.Print($"[FAIL] holdfast-presentation/{gate}{(note.Length > 0 ? " — " + note : "")}"); }
            }

            // ── Canonical owners ──
            var power = new PowerGridSystem(
                new PowerGridState { GenerationWatts = 800f, BatteryReserveWh = 4000f },
                new[]
                {
                    new PowerGridRoom("room_bunks", "Bunks", 40f),
                    new PowerGridRoom("room_filtration", "Air Filtration", 180f, PowerGridRoomPriority.Critical)
                },
                new SeededRng(19));
            // Open the bunks circuit so the room reads unpowered.
            Check("grid_breakers_set", power.SetBreaker("room_bunks", false));

            var thermal = new ShelterThermalSystem(
                new SeededRng(21), new NeedsSystem(), new StartingLevelSystem(), new YearOfAshDeepFreezeSystem());
            Check("thermal_room_added", thermal.AddRoom("room_filtration", "Air Filtration", 60f).IsSuccess);
            foreach (var node in thermal.State.rooms)
            {
                if (node.roomId == "room_filtration") node.currentTempC = -4f;
            }

            var sump = new SumpFloodingSystem(
                new SeededRng(23), new WeatherSystem(), power, new YearOfAshDeepFreezeSystem());
            Check("sump_node_created", sump.AddNode("sump_lower_deck", "Lower Deck Sump", 200f).IsSuccess);

            var assignments = new ShelterAssignmentHostSession(
                new List<ShelterRoom>
                {
                    new("room_bunks", "Bunks", 4),
                    new("room_filtration", "Air Filtration", 2)
                },
                new ShelterAssignmentState(),
                new SeededRng(29));
            Check("shelter_rooms_registered", assignments.System.Rooms.Count == 2, assignments.System.Rooms.Count.ToString());
            Check("survivor_assigned", assignments.System.Assign("survivor_dr_sarah_chen", "room_bunks", null, 1).Succeeded);

            var survivors = new SurvivorsHostSession();
            survivors.SeedDemoRoster();
            var dutyRoster = DutyRosterHostSession.Create(dataDirectory);
            dutyRoster.Roster.SetStatus("surv_00", DutyRosterIds.StatusQuiet);

            var session = new HoldfastPresentationHostSession();
            var slate = session.Compose(new PresentationSources
            {
                Assignments = assignments,
                Power = power,
                Thermal = thermal,
                Sump = sump,
                Survivors = survivors,
                DutyRoster = dutyRoster,
                CurrentDay = 12,
                ReduceMotion = false
            });

            // ── Room projection: power, temperature, occupancy, hazard. ──
            Check("rooms_projected", slate.Rooms.Count == 3, session.Describe());
            var bunks = slate.Rooms.Find(r => r.RoomId == "room_bunks");
            var filtration = slate.Rooms.Find(r => r.RoomId == "room_filtration");
            Check("unpowered_room_projected", bunks != null && !bunks!.IsPowered);
            Check("unpowered_hazard_band", bunks != null
                && bunks.PrimaryHazard == RoomHazardVisualBand.UnpoweredDark);
            Check("powered_room_projected", filtration != null && filtration!.IsPowered);
            Check("thermal_hazard_band", filtration != null
                && filtration.PrimaryHazard == RoomHazardVisualBand.FreezingCold
                && filtration.TemperatureCelsius == -4f);
            Check("occupants_projected", bunks != null && bunks.OccupantIds.Contains("survivor_dr_sarah_chen"));
            Check("sump_node_projected_as_room", slate.Rooms.Exists(r => r.RoomId == "sump_lower_deck"));
            Check("powered_room_count", slate.PoweredRoomsCount == 1, slate.PoweredRoomsCount.ToString());
            Check("sump_pump_unpowered_until_allocated", slate.Rooms.Exists(r => r.RoomId == "sump_lower_deck" && !r.IsPowered));

            // ── Actor projection: roster, stance, room. ──
            Check("actors_projected", slate.Actors.Count > 0, slate.Actors.Count.ToString());
            var assigned = slate.Actors.Find(a => a.SurvivorId == "survivor_dr_sarah_chen");
            Check("assigned_actor_room", assigned != null && assigned!.CurrentRoomId == "room_bunks");
            Check("actor_has_room_or_empty", slate.Actors.TrueForAll(a =>
                a.CurrentRoomId.Length == 0 || assignments.System.GetAssignmentForSurvivor(a.SurvivorId) != null));

            // ── A missing memorial owner never invents grief. ──
            var noMemorialSlate = session.Compose(new PresentationSources
            {
                Assignments = assignments,
                Power = power,
                Survivors = survivors,
                CurrentDay = 12
            });
            Check("missing_owner_leaves_grief_empty", noMemorialSlate.GrievingSurvivorsCount == 0);

            // ── Map projection: undiscovered nodes stay unknown/hidden. ──
            var map = new WastelandMapSystem(
                new WastelandMapState(),
                new[] { new MapNode { Id = "node_mill", DisplayName = "The Mill", Danger = MapNodeDanger.Low } },
                System.Array.Empty<MapRoute>(),
                null);
            var mappedSlate = session.Compose(new PresentationSources
            {
                Assignments = assignments,
                Power = power,
                Survivors = survivors,
                Map = map,
                CurrentDay = 12
            });
            Check("map_node_projected", mappedSlate.MapNodes.Count == 1, mappedSlate.MapNodes.Count.ToString());
            Check("undiscovered_node_is_unknown",
                mappedSlate.MapNodes[0].Knowledge == MapKnowledgeVisualRung.Unknown);

            // ── Crisis band escalation. ──
            Check("crisis_band_brownout", slate.OverallShelterCrisisBand == ShelterVisualCrisisBand.Brownout,
                slate.OverallShelterCrisisBand.ToString());

            var flooded = new SumpFloodingSystem(
                new SeededRng(43), new WeatherSystem(), power, new YearOfAshDeepFreezeSystem());
            flooded.AddNode("sump_a", "Sump A", 200f);
            flooded.AddNode("sump_b", "Sump B", 200f);
            flooded.State.nodes[0].isFlooded = true;
            flooded.State.nodes[1].isFlooded = true;
            var floodedSlate = session.Compose(new PresentationSources
            {
                Assignments = assignments,
                Sump = flooded,
                CurrentDay = 12
            });
            Check("two_flooded_rooms_escalate_to_crisis",
                floodedSlate.OverallShelterCrisisBand == ShelterVisualCrisisBand.Crisis,
                floodedSlate.OverallShelterCrisisBand.ToString());

            // ── Motion profile contract. ──
            var reduced = session.Compose(new PresentationSources { CurrentDay = 12, ReduceMotion = true });
            Check("reduce_motion_zero_duration", reduced.MotionProfile.ReduceMotion
                && reduced.MotionProfile.TransitionDurationSeconds == 0f
                && reduced.MotionProfile.EasingCurve == "instant");
            var normal = session.Compose(new PresentationSources { CurrentDay = 12 });
            Check("motion_profile_default", !normal.MotionProfile.ReduceMotion
                && normal.MotionProfile.EasingCurve == "cubic_ease_in_out");

            // ── Empty composition stays honest (zero counts, calm band). ──
            var empty = session.Compose(new PresentationSources { CurrentDay = 12 });
            Check("empty_slate_is_honest", empty.Rooms.Count == 0 && empty.Actors.Count == 0
                && empty.OverallShelterCrisisBand == ShelterVisualCrisisBand.Calm);

            Check("describe_non_empty", !string.IsNullOrWhiteSpace(session.Describe()));

            GD.Print($"[holdfast-presentation-selftest] {pass} passed, {fail} failed");
            return fail == 0 ? 0 : 1;
        }
    }
}
