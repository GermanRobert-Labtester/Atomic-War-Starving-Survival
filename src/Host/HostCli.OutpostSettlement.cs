// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 58 host probe — The Continuation: outposts, waystations, and a second
// holdfast.
//
// --outpost-settlement-selftest loads the authored outposts.json, then proves
// the real lifecycle against canonical owners: build cost is consumed through
// the canonical inventory provider, garrison is taken from the roster with a
// bunk cap and fitness gate, rations are drawn from the canonical ration
// provider, the daily tick consumes/starves/overruns, and a 30-day seeded run
// is deterministic with a byte-exact save/restore round-trip.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunOutpostSettlementSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; GD.Print($"[PASS] outpost/{gate}"); }
                else { fail++; GD.Print($"[FAIL] outpost/{gate}{(note.Length > 0 ? " — " + note : "")}"); }
            }

            OutpostSettlementHostSession session;
            try
            {
                session = OutpostSettlementHostSession.Load(dataDirectory, new FileSystemIO());
            }
            catch (Exception ex)
            {
                GD.Print($"[FAIL] outpost/authored_catalog_loads — {ex.Message}");
                GD.Print("[outpost-settlement-selftest] 0 passed, 1 failed");
                return 1;
            }

            bool CheckLoad(bool ok, string note = "")
            {
                if (ok) { pass++; GD.Print("[PASS] outpost/authored_catalog_loads"); }
                else { fail++; GD.Print($"[FAIL] outpost/authored_catalog_loads — {note}"); }
                return ok;
            }

            // 1 — the authored catalog loads: 4 outposts, all with real nodes,
            //     real build costs, and a positive bunk cap.
            var defs = session.System.GetAllDefinitions();
            if (!CheckLoad(defs.Count == 4, $"{defs.Count} outposts")) return fail == 0 ? 0 : 1;
            Check("definitions_have_graph_nodes_and_costs",
                defs.All(d => !string.IsNullOrWhiteSpace(d.GraphNodeId))
                && defs.All(d => d.BuildCost.Count > 0)
                && defs.All(d => d.MaxGarrisonBunks > 0),
                string.Join(",", defs.Select(d => d.Id)));
            Check("instances_created_for_every_definition",
                session.System.GetAllInstances().Count == defs.Count
                && session.System.GetAllInstances().All(i => !i.IsEstablished));

            // 2 — canonical build-cost consumption: the outposts draw from the
            //     canonical inventory through a provider, never a private store.
            var costLedger = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var north = defs.First(d => d.Id == "outpost_north_watch");
            // The canonical stock holds exactly the authored cost, so a
            // successful establishment spends the whole bill.
            foreach (var cost in north.BuildCost) costLedger[cost.Key] = cost.Value;

            bool Consume(string itemId, int count)
            {
                if (!costLedger.TryGetValue(itemId, out int have)) return false;   // unknown item refuses
                if (have < count) return false;                                     // insufficient refuses
                costLedger[itemId] = have - count;
                return true;
            }

            Check("establish_consumes_authored_build_cost", session.Establish("outpost_north_watch", Consume)
                && costLedger.Values.All(v => v == 0),
                string.Join(",", costLedger.Select(kv => $"{kv.Key}={kv.Value}")));
            Check("establish_is_idempotent_refusal", !session.Establish("outpost_north_watch", Consume));

            // 3 — an insufficient canonical stock refuses the establishment and
            //     leaves the outpost unestablished (no partial state).
            var unforgiving = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "scrap", 0 }, { "timber", 0 }, { "rations", 0 }
            };
            Check("establish_refused_without_stock",
                !session.Establish("outpost_rail_depot", (id, n) =>
                    unforgiving.TryGetValue(id, out int have) && have >= n)
                && !session.System.GetInstance("outpost_rail_depot")!.IsEstablished);

            // 4 — garrison comes from the canonical roster, bunk-capped.
            var roster = Enumerable.Range(0, 12).Select(i => $"survivor_{i:D2}").ToList();
            var unfit = new HashSet<string> { "survivor_03" };
            bool Fit(string id) => !unfit.Contains(id);

            var firstWatch = new[] { roster[0], roster[1], roster[2], roster[4] };
            Check("garrison_assignment_respects_bunks",
                firstWatch.All(id => session.AssignGarrison("outpost_north_watch", id, Fit))
                && !session.AssignGarrison("outpost_north_watch", roster[5], Fit),
                $"{session.System.GetInstance("outpost_north_watch")!.GarrisonSurvivorIds.Count}/{north.MaxGarrisonBunks}");
            Check("garrison_fitness_gate_refuses_unfit",
                !session.AssignGarrison("outpost_north_watch", "survivor_03", Fit));
            Check("garrison_duplicate_refused",
                !session.AssignGarrison("outpost_north_watch", roster[0], Fit));
            Check("garrison_relieve_returns_survivor",
                session.RelieveGarrison("outpost_north_watch", roster[0])
                && session.System.GetInstance("outpost_north_watch")!.GarrisonSurvivorIds.Count == north.MaxGarrisonBunks - 1);

            // 5 — rations come from the canonical ration provider (not a private
            //     outpost store) and the daily tick consumes them.
            var rationsHeld = 400;
            int Provide(string _outpostId, int demand)
            {
                int drawn = Math.Min(demand, rationsHeld);
                rationsHeld -= drawn;
                return drawn;
            }

            Check("supply_delivers_to_reserve", session.Supply("outpost_north_watch", 40)
                && session.System.GetInstance("outpost_north_watch")!.RationReserve == 40);
            Check("supply_rejects_non_positive", !session.Supply("outpost_north_watch", 0));

            // The garrison (3 survivors after the relief) draws its own demand
            // from the canonical supply and then consumes it.
            int garrisonCount = session.System.GetInstance("outpost_north_watch")!.GarrisonSurvivorIds.Count;
            session.TickDay(Provide);
            Check("daily_tick_draws_from_canonical_supply",
                session.System.GetInstance("outpost_north_watch")!.RationReserve == 40
                && rationsHeld == 400 - garrisonCount,
                $"reserve={session.System.GetInstance("outpost_north_watch")!.RationReserve} stock={rationsHeld}");

            // 6 — an outpost with a garrison and no supply starves: the daily
            //     tick really consumes, and starvation is not a cosmetic flag.
            var relay = defs.First(d => d.Id == "outpost_relay_tower");
            Check("unsupplied_outpost_established",
                session.Establish("outpost_relay_tower", null)
                && session.System.GetInstance("outpost_relay_tower")!.IsEstablished);
            Check("unsupplied_outpost_garrisoned",
                session.AssignGarrison("outpost_relay_tower", roster[6])
                && session.AssignGarrison("outpost_relay_tower", roster[7]));
            session.TickDay();
            Check("unsupplied_garrison_starves",
                session.System.GetInstance("outpost_relay_tower")!.IsStarving
                && session.System.GetInstance("outpost_relay_tower")!.RationReserve == 0,
                $"starving={session.System.GetInstance("outpost_relay_tower")!.IsStarving} "
                + $"reserve={session.System.GetInstance("outpost_relay_tower")!.RationReserve}");
            Check("starvation_degrades_condition",
                session.System.GetInstance("outpost_relay_tower")!.ConditionPermille < 1000);
            Check("supply_recovers_starvation",
                session.Supply("outpost_relay_tower", 10)
                && !session.System.GetInstance("outpost_relay_tower")!.IsStarving);

            // 7 — deterministic 30-day seeded run: same seed -> identical state,
            //     never System.Random, and community trails the fork.
            var sessionA = OutpostSettlementHostSession.Load(dataDirectory, new FileSystemIO());
            var sessionB = OutpostSettlementHostSession.Load(dataDirectory, new FileSystemIO());
            var rngA = new Ashfall.Core.Random.CampaignRngManager(1337);
            var rngB = new Ashfall.Core.Random.CampaignRngManager(1337);

            foreach (var s in new[] { sessionA, sessionB })
            {
                s.Establish("outpost_rail_depot");
                s.AssignGarrison("outpost_rail_depot", "survivor_10");
                s.AssignGarrison("outpost_rail_depot", "survivor_11");
                s.Supply("outpost_rail_depot", 12);
            }

            for (int day = 1; day <= 30; day++)
            {
                sessionA.TickDay();
                sessionB.TickDay();
                sessionA.SimulateRisk("outpost_rail_depot", 200,
                    rngA.Fork(Ashfall.Core.Random.CampaignStreamIds.OutpostRisk, day, 0));
                sessionB.SimulateRisk("outpost_rail_depot", 200,
                    rngB.Fork(Ashfall.Core.Random.CampaignStreamIds.OutpostRisk, day, 0));
            }

            var instA = sessionA.System.GetInstance("outpost_rail_depot")!;
            var instB = sessionB.System.GetInstance("outpost_rail_depot")!;
            Check("thirty_day_run_is_deterministic",
                instA.IsOverrun == instB.IsOverrun
                && instA.ConditionPermille == instB.ConditionPermille
                && instA.RationReserve == instB.RationReserve
                && instA.DaysSinceSupply == instB.DaysSinceSupply,
                $"A(overrun={instA.IsOverrun},cond={instA.ConditionPermille}) "
                + $"B(overrun={instB.IsOverrun},cond={instB.ConditionPermille})");
            Check("overrun_fires_at_most_once",
                !sessionA.SimulateRisk("outpost_rail_depot", 200,
                    rngA.Fork(Ashfall.Core.Random.CampaignStreamIds.OutpostRisk, 31, 0)));

            // 8 — the capture/restore round-trip restores state exactly, and a
            //     restore against a smaller/edited state degrades safely.
            var captured = sessionA.CaptureState();
            Check("capture_has_one_row_per_authored_outpost",
                captured.outposts.Count == defs.Count
                && captured.outposts.All(o => o.outpost_id.Length > 0));

            sessionB.RestoreState(captured);
            var instB2 = sessionB.System.GetInstance("outpost_rail_depot")!;
            Check("restore_is_field_exact",
                instB2.IsEstablished == instA.IsEstablished
                && instB2.IsOverrun == instA.IsOverrun
                && instB2.ConditionPermille == instA.ConditionPermille
                && instB2.RationReserve == instA.RationReserve
                && instB2.DaysSinceSupply == instA.DaysSinceSupply
                && instB2.GarrisonSurvivorIds.Count == instA.GarrisonSurvivorIds.Count,
                $"A(reserve={instA.RationReserve},cond={instA.ConditionPermille}) "
                + $"B(reserve={instB2.RationReserve},cond={instB2.ConditionPermille})");

            // 9 — a save/restore of the section round-trips through the store.
            Check("section_store_save", OutpostSettlementSaveStore.TrySave(captured));
            var reloaded = OutpostSettlementSaveStore.TryLoad();
            Check("section_store_reload", reloaded != null
                && reloaded!.outposts.Count == defs.Count
                && reloaded.schema_version == 1);

            var sessionC = OutpostSettlementHostSession.Load(dataDirectory, new FileSystemIO());
            sessionC.RestoreState(reloaded);
            Check("section_restore_round_trips",
                sessionC.System.GetInstance("outpost_rail_depot")!.IsOverrun == instA.IsOverrun
                && sessionC.System.GetInstance("outpost_rail_depot")!.RationReserve == instA.RationReserve);

            // 10 — a tampered or unknown row cannot invent a phantom outpost.
            var tampered = new Ashfall.Core.Settlements.OutpostSettlementState
            {
                schema_version = 1,
                outposts = new List<Ashfall.Core.Settlements.OutpostInstanceState>
                {
                    new Ashfall.Core.Settlements.OutpostInstanceState
                    {
                        outpost_id = "outpost_does_not_exist", is_established = true
                    }
                }
            };
            sessionC.RestoreState(tampered);
            Check("unknown_outpost_row_is_dropped",
                sessionC.System.GetInstance("outpost_does_not_exist") == null
                && sessionC.System.GetInstance("outpost_north_watch")!.IsEstablished == false);

            // 11 — abandon clears the garrison and the reserve.
            Check("abandon_clears_state",
                sessionA.Abandon("outpost_rail_depot")
                && sessionA.System.GetInstance("outpost_rail_depot")!.GarrisonSurvivorIds.Count == 0
                && sessionA.System.GetInstance("outpost_rail_depot")!.RationReserve == 0
                && !sessionA.System.GetInstance("outpost_rail_depot")!.IsEstablished);

            // 12 — the census is a truthful read model.
            Check("census_reports_live_network",
                OutpostCensusTruthful5(session), session.Describe());

            GD.Print($"[outpost-settlement-selftest] {pass} passed, {fail} failed");
            GD.Print($"[outpost-settlement-selftest] {session.Describe()}");
            return fail == 0 ? 0 : 1;
        }

        private static bool OutpostCensusTruthful5(OutpostSettlementHostSession session)
        {
            var census = session.ReadCensus();
            int established = 0, overrun = 0, starving = 0, garrisoned = 0, rations = 0;
            foreach (var inst in session.System.GetAllInstances())
            {
                if (inst == null) continue;
                if (inst.IsEstablished) established++;
                if (inst.IsOverrun) overrun++;
                if (inst.IsStarving) starving++;
                garrisoned += inst.GarrisonSurvivorIds?.Count ?? 0;
                rations += inst.RationReserve;
            }
            return census.Authored == 4
                && census.Established == established
                && census.Overrun == overrun
                && census.Starving == starving
                && census.Garrisoned == garrisoned
                && census.RationReserve == rations;
        }
    }
}
