// SPDX-License-Identifier: MIT
// Week-1 decision reachability: days 1-7 must surface at least one player
// decision each day through the real per-day decision stream.
//
// The day owner (NarrativeQuestsVerdictDayOwner.TickDay, src/Main.CampaignOwners.cs)
// draws one narrative arc per day and only falls back to the echo stream when
// no arc was selected; the pending decision is presented to the player after
// the daily briefing closes (Main.Campaign.OnBriefingAcknowledged). This test
// replicates that flow against the authored catalogs: character-bound arcs
// stay out of week 1 (their survivors are not in the starting roster), so the
// independent arc events and the early echoes must carry the week.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    public sealed class WeekOneDecisionReachabilityTests
    {
        [Theory]
        [InlineData(9001)]
        [InlineData(1337)]
        [InlineData(77)]
        [InlineData(424242)]
        [InlineData(20260930)]
        public void EveryWeekOneDay_SurfacesAtLeastOneDecision(int seed)
        {
            var arcs = NewArcSystem();
            var echoes = NewEchoSystem();

            for (int day = 1; day <= 7; day++)
            {
                var rng = new SeededRng(seed);
                var arc = arcs.SelectForDay(day, rng);
                if (arc != null)
                {
                    var choice = arc.Choices[0];
                    var committed = arcs.CommitChoice(arc.Id, choice.ChoiceId, day);
                    Assert.True(committed.Status == NarrativeArcChoiceStatus.Committed,
                        $"day {day}: arc choice for '{arc.Id}' did not commit: {committed.Status} {committed.Reason}");
                    continue;
                }

                var echo = echoes.SelectForDay(day, rng);
                Assert.NotNull(echo);
                var echoChoice = echo!.Choices[0];
                var resolved = echoes.Resolve(echo.Id, echoChoice.ChoiceId, day);
                Assert.True(resolved.Status == EchoResolutionStatus.Committed,
                    $"day {day}: echo choice for '{echo.Id}' did not commit: {resolved.Status}");
            }
        }

        [Fact]
        public void WeekOneLadder_IndependentArcsAndEarlyEchoesCarryTheWeek()
        {
            var arcs = NewArcSystem();
            var echoes = NewEchoSystem();

            // The four character arcs require survivors that are not in the
            // starting roster, so they must stay ineligible all week.
            for (int day = 1; day <= 7; day++)
            {
                var arcIds = arcs.GetEligibleCandidates(day).Select(x => x.Event.Id).ToList();
                Assert.All(arcIds, id => Assert.DoesNotContain(id, new[]
                {
                    "narrative_aris_thorne_stage_1", "narrative_maya_lin_stage_1",
                    "narrative_victor_vance_stage_1", "narrative_elena_rostov_stage_1"
                }));
            }

            // Independent arcs cover days 1, 3 and 5.
            var day1 = arcs.GetEligibleCandidates(1).Select(x => x.Event.Id).ToList();
            Assert.Equal("narrative_garrison_defector_intel", Assert.Single(day1));
            Assert.Contains("narrative_militia_council_invitation",
                arcs.GetEligibleCandidates(3).Select(x => x.Event.Id));
            Assert.Contains("narrative_cult_prophet_rumor",
                arcs.GetEligibleCandidates(5).Select(x => x.Event.Id));

            // Early echoes cover the arc-less days 2, 4, 6 and 7.
            var day2 = echoes.GetEligibleCandidates(2).Select(e => e.Id).ToList();
            Assert.Equal("echo_the_nameplates", Assert.Single(day2));
            Assert.Contains("echo_unopened_boots", echoes.GetEligibleCandidates(4).Select(e => e.Id));
            Assert.Contains("echo_the_frying_pan", echoes.GetEligibleCandidates(6).Select(e => e.Id));
            Assert.Contains("echo_school_register", echoes.GetEligibleCandidates(7).Select(e => e.Id));
        }

        private static NarrativeArcEventSystem NewArcSystem()
        {
            var load = NarrativeArcEventCatalogLoader.LoadDetailed(
                FindDataDirectory(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(load.IsSuccess, string.Join(" | ", load.Errors));
            var system = new NarrativeArcEventSystem(load.Events);
            // Fresh week-1 roster: none of the arc-bound survivors is present.
            system.SurvivorIsPresent = _ => false;
            system.Consequences = new PermissivePort();
            return system;
        }

        private static EchoSystem NewEchoSystem()
        {
            var catalog = EchoCatalogLoader.Load(
                FindDataDirectory(), new FileSystemIO(), new SystemTextJsonSerializer());
            var system = new EchoSystem(catalog) { HasWorldFlag = _ => true };
            return system;
        }

        private static string FindDataDirectory()
        {
            string configured = Environment.GetEnvironmentVariable("ASHFALL_DATA_DIR") ?? string.Empty;
            if (Directory.Exists(configured)) return configured;

            string current = AppContext.BaseDirectory;
            while (!string.IsNullOrEmpty(current))
            {
                string candidate = Path.Combine(current, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                current = Directory.GetParent(current)?.FullName ?? string.Empty;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        /// <summary>
        /// Fail-open consequence port: this test exercises the selection and
        /// commit contract of the decision stream, not the consequence owners.
        /// </summary>
        private sealed class PermissivePort : INarrativeArcConsequencePort
        {
            public bool CanApplyMorale(string survivorId, int delta, bool shelterWide, out string reason)
            { reason = string.Empty; return true; }
            public void ApplyMorale(string survivorId, int delta, bool shelterWide) { }
            public bool CanGrantFactionIntel(string canonicalFactionId, out string reason)
            { reason = string.Empty; return true; }
            public void GrantFactionIntel(string canonicalFactionId) { }
            public bool CanOfferExpedition(string locationId, out string reason)
            { reason = string.Empty; return true; }
            public void OfferExpedition(string locationId) { }
            public bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason)
            { reason = string.Empty; return true; }
            public void ApplyFactionStanding(string canonicalFactionId, int delta) { }
        }
    }
}
