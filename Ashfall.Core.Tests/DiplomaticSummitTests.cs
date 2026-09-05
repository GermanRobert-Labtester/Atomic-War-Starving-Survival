using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Diplomacy;
using Ashfall.Core.Institutions;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship Task 6 — DiplomaticSummitSystem behaviour gates: scheduling
    /// validation, deterministic negotiation, atomic ratification, guarantee
    /// personhood, DMZ rules, violation routing, expiry and save continuation.
    /// </summary>
    public class DiplomaticSummitTests
    {
        private const string Site = DiplomaticSummitSystem.NeutralSummitSiteId;
        private const string Framework = "treaty_non_aggression_compact";
        private const string DmzFramework = "treaty_demilitarized_trade_corridor";

        private static List<DiplomaticTreatyDefinition> LoadFrameworks()
        {
            string dataDir = CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out string found)
                ? found
                : throw new InvalidOperationException("data dir not found");
            return DiplomaticTreatyCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private sealed class TrackingAvailability : IInstitutionAvailability
        {
            public readonly HashSet<string> Claims = new(StringComparer.Ordinal);
            public bool IsAvailable(string survivorId) => !Claims.Contains(survivorId);
            public bool TryClaim(string survivorId, string institutionId, string roleId) => Claims.Add(survivorId);
            public void Release(string survivorId, string institutionId, string roleId) => Claims.Remove(survivorId);
        }

        private sealed class RecordingStanding : IFactionStandingPort
        {
            public readonly Dictionary<string, float> Standing = new();
            public readonly List<(string Faction, float Delta, string Reason)> Calls = new();
            public float GetStanding(string factionId) => Standing.GetValueOrDefault(factionId, 50f);
            public void AdjustStanding(string factionId, float delta, string reasonCode)
            {
                Calls.Add((factionId, delta, reasonCode));
                Standing[factionId] = GetStanding(factionId) + delta;
            }
        }

        private sealed class StaticFactions : IFactionContextPort
        {
            public static readonly Dictionary<string, string> Tags = new(StringComparer.Ordinal)
            {
                ["faction_a"] = "militia",
                ["faction_b"] = "civic",
                ["faction_c"] = "settlement",
                ["faction_d"] = "caravan",
            };
            public string? GetFactionTag(string factionId) => Tags.GetValueOrDefault(factionId);
            public bool IsHostile(string factionId) => false;
        }

        private sealed class Fixture
        {
            public Inventory.Inventory Inventory = new();
            public TrackingAvailability Availability = new();
            public RecordingStanding Standing = new();
            public StaticFactions Factions = new();
            public DiplomaticSummitSystem Diplomacy = null!;
            public List<ActiveTreatyState> Ratified = new();
            public List<(ActiveTreatyState T, string Reason)> Ended = new();
            public List<TreatyViolationRecord> Violations = new();
            public List<GuaranteeState> GuaranteeEvents = new();

            public static Fixture Create(int masterSeed = 42, List<DiplomaticTreatyDefinition>? frameworks = null)
            {
                var f = new Fixture();
                f.Diplomacy = new DiplomaticSummitSystem(
                    masterSeed,
                    inventory: f.Inventory,
                    availability: f.Availability,
                    standing: f.Standing,
                    factions: f.Factions);
                f.Diplomacy.LoadTreatyCatalog(frameworks ?? LoadFrameworks());
                f.Diplomacy.OnTreatyRatified += t => f.Ratified.Add(t);
                f.Diplomacy.OnTreatyEnded += (t, r) => f.Ended.Add((t, r));
                f.Diplomacy.OnTreatyViolationRecorded += v => f.Violations.Add(v);
                f.Diplomacy.OnGuaranteeReleased += g => f.GuaranteeEvents.Add(g);
                foreach (var id in new[] { "clean_water", "fuel", "mechanical_parts", "bandage", "battery", "scrap_metal", "item_preservation_salt", "scrap_electronic", "copper_wire_10m_of_10m" })
                    f.Inventory.TryProduce(id, 10);
                return f;
            }
        }

        private static ActionResult NegotiateToRatifiable(Fixture f, string summitId, int maxRounds = 30)
        {
            ActionResult last = ActionResult.Blocked("none", "none");
            for (int i = 0; i < maxRounds; i++)
            {
                last = f.Diplomacy.AdvanceNegotiation(summitId, offerConcession: true);
                var summit = f.Diplomacy.GetSummit(summitId)!;
                if (summit.status != "negotiating" || summit.negotiation_stability >= DiplomaticSummitSystem.RatificationThreshold)
                    break;
            }
            return last;
        }

        // ------------------------------------------------------------------
        // SCHEDULING
        // ------------------------------------------------------------------

        [Fact]
        public void Scheduling_EnforcesNeutralSite_MinimumSignatories_AndKnownFactions()
        {
            var f = Fixture.Create();
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Diplomacy.TryScheduleSummit("loc_settlement_ferry_crossing",
                    new[] { "faction_a", "faction_b" }, new[] { "survivor_envoy" }, Framework, 10).Status);

            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Diplomacy.TryScheduleSummit(Site, new[] { "faction_a" },
                    new[] { "survivor_envoy" }, Framework, 10).Status); // min 2 signatories

            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Diplomacy.TryScheduleSummit(Site, new[] { "faction_a", "faction_ghost" },
                    new[] { "survivor_envoy" }, Framework, 10).Status);

            Assert.Equal(ActionResult.StatusKind.Success,
                f.Diplomacy.TryScheduleSummit(Site, new[] { "faction_a", "faction_b" },
                    new[] { "survivor_envoy" }, Framework, 10).Status);
            Assert.Single(f.Diplomacy.Summits);
        }

        [Fact]
        public void Scheduling_RejectsUnavailableDelegate()
        {
            var f = Fixture.Create();
            Assert.True(f.Availability.TryClaim("survivor_envoy", "institution_elsewhere", "other"));
            var blocked = f.Diplomacy.TryScheduleSummit(Site,
                new[] { "faction_a", "faction_b" }, new[] { "survivor_envoy" }, Framework, 10);
            Assert.Equal(ActionResult.StatusKind.Blocked, blocked.Status);
        }

        [Fact]
        public void Scheduling_UnknownFramework_Fails()
        {
            var f = Fixture.Create();
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Diplomacy.TryScheduleSummit(Site, new[] { "faction_a", "faction_b" },
                    new[] { "survivor_envoy" }, "treaty_not_authored", 10).Status);
        }

        // ------------------------------------------------------------------
        // NEGOTIATION DETERMINISM + RATIFICATION
        // ------------------------------------------------------------------

        [Fact]
        public void Negotiation_SameSeedAndMoves_AreDeterministic()
        {
            var moves = new[] { true, false, true, true, false };
            var a = RunScriptedNegotiation(42, moves);
            var b = RunScriptedNegotiation(42, moves);
            Assert.Equal(a, b);
        }

        private static string RunScriptedNegotiation(int seed, bool[] moves)
        {
            var f = Fixture.Create(seed);
            f.Diplomacy.TryScheduleSummit(Site, new[] { "faction_a", "faction_b" },
                new[] { "survivor_envoy" }, Framework, 10);
            string summitId = f.Diplomacy.Summits[0].summit_id;
            var trace = new List<string>();
            for (int i = 0; i < moves.Length; i++)
            {
                var r = f.Diplomacy.AdvanceNegotiation(summitId, moves[i]);
                trace.Add($"{r.Status}:{(r.Deltas.TryGetValue("stability", out double s) ? s : -1)}:{(r.Deltas.TryGetValue("roll", out double roll) ? roll : -1)}");
            }
            return string.Join("|", trace);
        }

        [Fact]
        public void Ratification_CreatesActiveTreaty_WithAuthoredExpiry_AndPaysConcessions()
        {
            var f = Fixture.Create();
            f.Diplomacy.TryScheduleSummit(Site, new[] { "faction_a", "faction_b" },
                new[] { "survivor_envoy" }, Framework, 10);
            string summitId = f.Diplomacy.Summits[0].summit_id;
            NegotiateToRatifiable(f, summitId);

            int waterBefore = f.Inventory.CountById("clean_water");
            var result = f.Diplomacy.TryRatifyTreaty(summitId, 20);
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);

            var treaty = f.Ratified.Single();
            Assert.Equal("active", treaty.status);
            Assert.Equal(20 + 30, treaty.expiry_day); // framework duration_days = 30
            Assert.Equal(waterBefore - 4, f.Inventory.CountById("clean_water")); // authored concession
            Assert.Equal("ratified", f.Diplomacy.GetSummit(summitId)!.status);
            Assert.False(f.Availability.Claims.Contains("survivor_envoy")); // delegates released
        }

        [Fact]
        public void Ratification_BelowThreshold_DoesNotPartiallyRatify()
        {
            var f = Fixture.Create();
            f.Diplomacy.TryScheduleSummit(Site, new[] { "faction_a", "faction_b" },
                new[] { "survivor_envoy" }, Framework, 10);
            string summitId = f.Diplomacy.Summits[0].summit_id;

            // a couple of rounds, below threshold
            f.Diplomacy.AdvanceNegotiation(summitId, true);
            var blocked = f.Diplomacy.TryRatifyTreaty(summitId, 20);
            Assert.Equal(ActionResult.StatusKind.Blocked, blocked.Status);
            Assert.Empty(f.Ratified);
            Assert.Empty(f.Diplomacy.Treaties);
            Assert.Equal(10, f.Inventory.CountById("clean_water")); // nothing consumed
        }

        // ------------------------------------------------------------------
        // GUARANTEES
        // ------------------------------------------------------------------

        [Fact]
        public void Guarantee_ClaimsAvailability_ButSurvivorStaysIdentified()
        {
            var f = Fixture.Create();
            RatifyQuick(f, out ActiveTreatyState treaty);

            var result = f.Diplomacy.TryExchangeGuarantee(treaty.treaty_id, "survivor_envoy", "faction_a", 21);
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.True(f.Diplomacy.IsGuaranteeHeld("survivor_envoy"));
            Assert.False(f.Availability.IsAvailable("survivor_envoy")); // blocked from other work

            var guarantee = f.Diplomacy.Guarantees.Single();
            Assert.Equal("survivor_envoy", guarantee.survivor_id);   // identity retained
            Assert.Equal("exchanged", guarantee.status);

            // save/load retains the held guarantee
            var saved = f.Diplomacy.CaptureState();
            var fresh = Fixture.Create();
            fresh.Diplomacy.RestoreState(saved);
            Assert.True(fresh.Diplomacy.IsGuaranteeHeld("survivor_envoy"));
            Assert.Equal("survivor_envoy", fresh.Diplomacy.Guarantees.Single().survivor_id);
        }

        [Fact]
        public void Guarantee_Release_RestoresAvailability()
        {
            var f = Fixture.Create();
            RatifyQuick(f, out ActiveTreatyState treaty);
            f.Diplomacy.TryExchangeGuarantee(treaty.treaty_id, "survivor_envoy", "faction_a", 21);

            var guarantee = f.Diplomacy.Guarantees.Single();
            var result = f.Diplomacy.TryReleaseGuarantee(guarantee.guarantee_id, 30);
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.True(f.Availability.IsAvailable("survivor_envoy"));
            Assert.Equal("released", f.Diplomacy.Guarantees.Single().status);
            Assert.Single(f.GuaranteeEvents);

            // double release rejected
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Diplomacy.TryReleaseGuarantee(guarantee.guarantee_id, 31).Status);
        }

        [Fact]
        public void Guarantee_Rejected_WhenFrameworkDisallows()
        {
            var f = Fixture.Create();
            // aquifer sharing framework has guarantee_allowed = false
            f.Diplomacy.TryScheduleSummit(Site, new[] { "faction_b", "faction_c" },
                new[] { "survivor_envoy" }, "treaty_aquifer_water_sharing", 10);
            string summitId = f.Diplomacy.Summits[0].summit_id;
            NegotiateToRatifiable(f, summitId);
            Assert.Equal(ActionResult.StatusKind.Success, f.Diplomacy.TryRatifyTreaty(summitId, 20).Status);

            var treaty = f.Ratified.Single();
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Diplomacy.TryExchangeGuarantee(treaty.treaty_id, "survivor_envoy", "faction_a", 21).Status);
        }

        // ------------------------------------------------------------------
        // DMZ + VIOLATIONS
        // ------------------------------------------------------------------

        [Fact]
        public void Dmz_RestrictsArmedPatrols_OnlyForSignatories_InZones()
        {
            var f = Fixture.Create();
            RatifyQuick(f, out ActiveTreatyState treaty, DmzFramework);

            Assert.False(f.Diplomacy.IsArmedPatrolAllowed("faction_a", "high_scarp_ridgeline"));
            Assert.True(f.Diplomacy.IsArmedPatrolAllowed("faction_none_such", "high_scarp_ridgeline")); // non-signatory
            Assert.True(f.Diplomacy.IsArmedPatrolAllowed("faction_a", "suburban_heights"));     // non-DMZ zone
        }

        [Fact]
        public void DmzPatrol_ProducesOneViolation_RoutedToStandingAuthority()
        {
            var f = Fixture.Create();
            RatifyQuick(f, out ActiveTreatyState treaty, DmzFramework);
            float standingBefore = f.Standing.GetStanding("faction_a");

            var first = f.Diplomacy.ReportArmedPatrol("faction_a", "high_scarp_ridgeline", 25);
            Assert.Equal("diplomacy.violation_recorded", first.MessageKey);
            Assert.Single(f.Violations);
            Assert.True(treaty.stability < 45, "stability reduced by violation");

            // same patrol reported again the same day → recorded once
            var repeat = f.Diplomacy.ReportArmedPatrol("faction_a", "high_scarp_ridgeline", 25);
            Assert.Equal("diplomacy.violation_already_recorded", repeat.MessageKey);
            Assert.Single(f.Violations);

            // routed to the canonical standing authority with the authored penalty
            Assert.Contains(f.Standing.Calls, c => c.Faction == "faction_a" && c.Reason == "diplomacy.dmz_armed_patrol");
            Assert.True(f.Standing.GetStanding("faction_a") < standingBefore);
        }

        [Fact]
        public void ViolationToleranceExceeded_CollapsesTreaty_AndUnregistersDmz()
        {
            var f = Fixture.Create();
            RatifyQuick(f, out ActiveTreatyState treaty, DmzFramework);

            // corridor treaty tolerates 1 violation → 2nd collapses it
            f.Diplomacy.ReportArmedPatrol("faction_a", "high_scarp_ridgeline", 25);
            f.Diplomacy.ReportArmedPatrol("faction_b", "industrial_district", 26);

            Assert.Equal("collapsed", treaty.status);
            Assert.True(f.Diplomacy.IsArmedPatrolAllowed("faction_a", "high_scarp_ridgeline"));
            Assert.Contains(f.Ended, e => e.T.treaty_id == treaty.treaty_id && e.Reason == "collapsed");
        }

        [Fact]
        public void Treaty_Expiry_UnregistersDmz_FiresOnce_ReleasesGuarantees()
        {
            var f = Fixture.Create();
            RatifyQuick(f, out ActiveTreatyState treaty, DmzFramework);
            f.Diplomacy.TryExchangeGuarantee(treaty.treaty_id, "survivor_envoy", "faction_a", 21);
            Assert.False(f.Availability.IsAvailable("survivor_envoy"));

            int ended = 0;
            f.Diplomacy.OnTreatyEnded += (_, _) => ended++;

            f.Diplomacy.TickDay(treaty.expiry_day);
            Assert.Equal("expired", treaty.status);
            Assert.True(f.Diplomacy.IsArmedPatrolAllowed("faction_a", "high_scarp_ridgeline"));
            Assert.True(f.Availability.IsAvailable("survivor_envoy"));
            Assert.Equal("released", f.Diplomacy.Guarantees.Single().status);
            Assert.Equal(1, ended); // exactly one expiry event

            f.Diplomacy.TickDay(treaty.expiry_day + 1);
            f.Diplomacy.TickDay(treaty.expiry_day + 2);
            Assert.Equal(1, ended); // no duplicate expiry events
        }

        // ------------------------------------------------------------------
        // SAVE / RESTORE
        // ------------------------------------------------------------------

        [Fact]
        public void SaveLoad_PreservesTreatyDuration_AndComplianceContinuation()
        {
            var f = Fixture.Create();
            RatifyQuick(f, out ActiveTreatyState treaty, DmzFramework);
            f.Diplomacy.TickDay(treaty.start_day + 5);

            var saved = f.Diplomacy.CaptureState();
            var fresh = Fixture.Create();
            fresh.Diplomacy.RestoreState(saved);
            var restoredTreaty = fresh.Diplomacy.Treaties.Single();

            Assert.Equal(treaty.expiry_day, restoredTreaty.expiry_day);
            Assert.Equal(treaty.status, restoredTreaty.status);
            Assert.Equal(treaty.stability, restoredTreaty.stability);

            // post-restore next compliance outcome matches uninterrupted run
            var a = f.Diplomacy.ReportArmedPatrol("faction_a", "high_scarp_ridgeline", treaty.start_day + 6);
            var b = fresh.Diplomacy.ReportArmedPatrol("faction_a", "high_scarp_ridgeline", treaty.start_day + 6);
            Assert.Equal(a.MessageKey, b.MessageKey);
            Assert.Equal(f.Violations.Single().violation_id, fresh.Violations.Single().violation_id);
            Assert.Equal(treaty.stability, restoredTreaty.stability);
        }

        [Fact]
        public void OldSave_MissingDiplomacySection_DefaultsSafely()
        {
            var f = Fixture.Create();
            f.Diplomacy.RestoreState(null);
            Assert.Empty(f.Diplomacy.Summits);
            Assert.Empty(f.Diplomacy.Treaties);
            Assert.Empty(f.Diplomacy.Guarantees);
            Assert.True(f.Diplomacy.IsArmedPatrolAllowed("faction_a", "high_scarp_ridgeline"));
        }

        // ------------------------------------------------------------------
        // helpers
        // ------------------------------------------------------------------

        private static void RatifyQuick(Fixture f, out ActiveTreatyState treaty, string frameworkId = Framework)
        {
            // Pick signatories by the framework's own tag eligibility so the
            // helper works for any framework (corridor needs 3, others 2).
            var framework = LoadFrameworks().First(t => t.treaty_id == frameworkId);
            var factions = StaticFactions.Tags
                .Where(kv => framework.EligibleForTag(kv.Value))
                .Select(kv => kv.Key)
                .OrderBy(k => k, StringComparer.Ordinal)
                .Take(framework.minimum_signatories)
                .ToArray();
            Assert.True(factions.Length >= framework.minimum_signatories,
                $"fixture lacks eligible factions for {frameworkId}");
            f.Diplomacy.TryScheduleSummit(Site, factions,
                new[] { "survivor_envoy" }, frameworkId, 10);
            string summitId = f.Diplomacy.Summits[0].summit_id;
            NegotiateToRatifiable(f, summitId);
            var result = f.Diplomacy.TryRatifyTreaty(summitId, 20);
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            treaty = f.Ratified.Single();
        }
    }
}
