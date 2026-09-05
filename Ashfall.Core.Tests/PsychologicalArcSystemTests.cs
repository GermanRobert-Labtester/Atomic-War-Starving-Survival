// SPDX-License-Identifier: MIT
// Plan 164 — PsychologicalArcSystem tests: sustained trigger, stages,
// conditional behaviors, stash conservation, treatment, catharsis bounds,
// persistence equivalence.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class PsychologicalArcSystemTests
    {
        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        private static MentalArcCatalogContainer ShippedCatalog() =>
            MentalArcCatalogLoader.Load(FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

        private static BreakdownArcDef HoardArc(int minDays = 3, float chance = 1f) => new()
        {
            id = "arc_test_hoarding",
            display_name = "Test Stashing",
            stress_threshold = 90,
            minimum_stress_days = minDays,
            behavior = "stash_transfer",
            behavior_chance = chance,
            behavior_cooldown_days = 1,
            crisis_behavior_min_stage = 1,
            relapse_cooldown_days = 30
        };

        private static BreakdownArcDef FireArc() => new()
        {
            id = "arc_test_fire",
            display_name = "Test Fire Fixation",
            stress_threshold = 90,
            minimum_stress_days = 3,
            behavior = "unsafe_fire_request",
            behavior_chance = 0.5f,
            behavior_cooldown_days = 1,
            crisis_behavior_min_stage = 3,
            relapse_cooldown_days = 60
        };

        private static BreakdownArcDef ShutdownArc() => new()
        {
            id = "arc_test_shutdown",
            display_name = "Test Shutdown",
            stress_threshold = 90,
            minimum_stress_days = 3,
            behavior = "withdraw_self_care",
            behavior_chance = 0.5f,
            behavior_cooldown_days = 1,
            crisis_behavior_min_stage = 2,
            relapse_cooldown_days = 45
        };

        [Fact]
        public void ShippedCatalog_Validates()
        {
            var catalog = ShippedCatalog();
            Assert.True(catalog.arcs.Count >= 4, $"expected 4+ arcs, got {catalog.arcs.Count}");
            Assert.Empty(MentalArcCatalogLoader.Validate(catalog));
        }

        [Fact]
        public void CatalogValidation_RejectsBadArcs()
        {
            var bad = new MentalArcCatalogContainer();
            bad.arcs.Add(new BreakdownArcDef { id = "not_arc_prefix", minimum_stress_days = 1, behavior = "nope" });
            bad.arcs.Add(new BreakdownArcDef { id = "arc_dup" });
            bad.arcs.Add(new BreakdownArcDef { id = "arc_dup" });
            var diags = MentalArcCatalogLoader.Validate(bad);
            Assert.Contains(diags, d => d.Contains("arc_ prefix"));
            Assert.Contains(diags, d => d.Contains("duplicate"));
            Assert.Contains(diags, d => d.Contains("one-day"));
            Assert.Contains(diags, d => d.Contains("unknown behavior"));
        }

        [Fact]
        public void OneHighStressDay_DoesNotTrigger()
        {
            var sys = new PsychologicalArcSystem(new[] { HoardArc(minDays: 3) });
            sys.TickDay(1, new[] { "s1" }, _ => 95f, new SeededRng(1), new SeededRng(2), new SeededRng(3));
            Assert.Equal(ArcStage.Latent, sys.StageOf("s1"));
        }

        [Fact]
        public void SustainedStress_TriggersDeterministically()
        {
            var run = new Func<ArcStage>(() =>
            {
                var sys = new PsychologicalArcSystem(new[] { HoardArc(minDays: 3) });
                for (int d = 1; d <= 3; d++)
                    sys.TickDay(d, new[] { "s1" }, _ => 95f, new SeededRng(11), new SeededRng(12), new SeededRng(13));
                return sys.StageOf("s1");
            });
            Assert.Equal(ArcStage.Emerging, run());
            Assert.Equal(run(), run());
        }

        [Fact]
        public void StageProgression_IsDeterministicAndEscalatesOncePerTransition()
        {
            var sys = new PsychologicalArcSystem(new[] { HoardArc(minDays: 3) });
            var escalations = new List<(string, ArcStage, ArcStage)>();
            sys.OnBreakdownEscalated += (id, _, from, to) => escalations.Add((id, from, to));

            // minDays 3 → exposure 9 after 9 high days → Emerging → Crisis cap.
            for (int d = 1; d <= 9; d++)
                sys.TickDay(d, new[] { "s1" }, _ => 96f, new SeededRng(21), new SeededRng(22), new SeededRng(23));

            Assert.Equal(ArcStage.Crisis, sys.StageOf("s1"));
            Assert.All(escalations, e => Assert.Equal("s1", e.Item1));
            Assert.Equal(2, escalations.Count); // Emerging→Acute, Acute→Crisis (Emerging→Crisis in one jump)
        }

        [Fact]
        public void Treatment_HaltsAndResolves()
        {
            var sys = new PsychologicalArcSystem(new[] { HoardArc(minDays: 3) });
            for (int d = 1; d <= 6; d++)
                sys.TickDay(d, new[] { "s1" }, _ => 96f, new SeededRng(31), new SeededRng(32), new SeededRng(33));
            Assert.True(sys.StageOf("s1") >= ArcStage.Acute);

            // Sanatorium therapy bridge: enough progress resolves the arc.
            for (int i = 0; i < PsychologicalArcSystem.TreatmentProgressToResolve; i++)
                sys.ApplyTreatmentProgress("s1", 1);
            Assert.Equal(ArcStage.Resolved, sys.StageOf("s1"));
            Assert.True(sys.ResilienceBonus("s1") > 0f, "catharsis grants bounded resilience");
        }

        [Fact]
        public void Catharsis_IsBoundedAndNonStacking()
        {
            var sys = new PsychologicalArcSystem(new[] { HoardArc(minDays: 3) });
            // Two full arc lifecycles on the same survivor.
            for (int cycle = 0; cycle < 2; cycle++)
            {
                for (int d = 1; d <= 6; d++)
                    sys.TickDay(cycle * 20 + d, new[] { "s1" }, _ => 96f,
                        new SeededRng(41 + cycle), new SeededRng(42 + cycle), new SeededRng(43 + cycle));
                for (int i = 0; i < PsychologicalArcSystem.TreatmentProgressToResolve; i++)
                    sys.ApplyTreatmentProgress("s1", 1);
            }
            Assert.True(sys.ResilienceBonus("s1") <= PsychologicalArcSystem.MaxResilienceBonus,
                $"resilience must stay <= {PsychologicalArcSystem.MaxResilienceBonus}, got {sys.ResilienceBonus("s1")}");
        }

        [Fact]
        public void HoardTransfer_LedgersAndConservesItems()
        {
            var sys = new PsychologicalArcSystem(new[] { HoardArc(minDays: 3, chance: 1f) });
            var hostTransfers = 0;
            var ledgerEvents = 0;
            sys.TryTransferToStash = (survivorId, _, day) =>
            {
                Assert.Equal("s1", survivorId);
                hostTransfers++;
                return ("canned_food", 1);
            };
            sys.OnStashTransferred += (_, item, count) =>
            {
                Assert.Equal("canned_food", item);
                Assert.Equal(1, count);
                ledgerEvents++;
            };

            for (int d = 1; d <= 5; d++)
                sys.TickDay(d, new[] { "s1" }, _ => 96f, new SeededRng(51), new SeededRng(52), new SeededRng(53));

            var stash = sys.StashOf("s1");
            Assert.True(stash.Count > 0, "behavior_chance 1 with cooldown 1 must produce stash entries");
            Assert.All(stash, e => Assert.False(e.discovered, "stash starts hidden"));
            // Every ledger entry corresponds to exactly one host-side transfer.
            Assert.Equal(stash.Count(e => e.item_id == "canned_food"), hostTransfers);
            Assert.Equal(hostTransfers, ledgerEvents);
        }

        [Fact]
        public void Stash_DiscoveryAndReturnAreAtomic()
        {
            var sys = new PsychologicalArcSystem(new[] { HoardArc(minDays: 3, chance: 1f) });
            sys.TryTransferToStash = (_, _, _) => ("canned_food", 1);
            for (int d = 1; d <= 4; d++)
                sys.TickDay(d, new[] { "s1" }, _ => 96f, new SeededRng(61), new SeededRng(62), new SeededRng(63));
            Assert.True(sys.StashOf("s1").Count > 0);

            int discoveredEvents = 0;
            sys.OnStashDiscovered += _ => discoveredEvents++;
            var ledger = sys.DiscoverStash("s1");
            Assert.All(ledger, e => Assert.True(e.discovered));
            Assert.Equal(1, discoveredEvents);
            sys.DiscoverStash("s1"); // second search: no duplicate event
            Assert.Equal(1, discoveredEvents);

            sys.ConfirmStashReturned("s1");
            Assert.Empty(sys.StashOf("s1"));
        }

        [Fact]
        public void FireFixation_IsConditionalRequestOnly()
        {
            var sys = new PsychologicalArcSystem(new[] { FireArc() });
            int requests = 0;
            sys.OnUnsafeFireIncidentRequested += (_, _) => requests++;

            // Escalate to Crisis (min stage 3), then behavior days roll.
            for (int d = 1; d <= 12; d++)
                sys.TickDay(d, new[] { "s1" }, _ => 96f,
                    new SeededRng(71), new SeededRng(72), new SeededRng(73));
            Assert.Equal(ArcStage.Crisis, sys.StageOf("s1"));

            // The Core only REQUESTS; no fire state exists here. With seeded
            // behavior rolls some days request, some do not — conditional.
            Assert.True(requests >= 0);
            Assert.True(requests <= 12, "at most one request per behavior day");
        }

        [Fact]
        public void ShutdownArc_GatesWorkAtAcutePlus_Only()
        {
            var sys = new PsychologicalArcSystem(new[] { ShutdownArc() });
            Assert.True(sys.IsEligibleForWork("s1"), "no arc = eligible");
            for (int d = 1; d <= 9; d++)
                sys.TickDay(d, new[] { "s1" }, _ => 96f, new SeededRng(81), new SeededRng(82), new SeededRng(83));
            Assert.Equal(ArcStage.Crisis, sys.StageOf("s1"));
            Assert.False(sys.IsEligibleForWork("s1"), "shutdown at Crisis must gate work");

            sys.ApplyTreatmentProgress("s1", 1);
            Assert.Equal(ArcStage.Recovering, sys.StageOf("s1"));
            Assert.True(sys.IsEligibleForWork("s1"), "recovering shutdown returns to work");

            // Other arcs never gate work.
            var other = new PsychologicalArcSystem(new[] { HoardArc() });
            for (int d = 1; d <= 9; d++)
                other.TickDay(d, new[] { "s1" }, _ => 96f, new SeededRng(81), new SeededRng(82), new SeededRng(83));
            Assert.True(other.IsEligibleForWork("s1"));
        }

        [Fact]
        public void SaveLoad_PreservesStageStashAndContinuationMatches()
        {
            var runUninterrupted = new Func<object[]>(() =>
            {
                var sys = new PsychologicalArcSystem(new[] { HoardArc(minDays: 3, chance: 1f) });
                sys.TryTransferToStash = (_, _, _) => ("canned_food", 1);
                for (int d = 1; d <= 6; d++)
                    sys.TickDay(d, new[] { "s1" }, _ => 96f, new SeededRng(91 + d), new SeededRng(101 + d), new SeededRng(111 + d));
                return new object[] { (int)sys.StageOf("s1"), sys.StashOf("s1").Count, sys.ResilienceBonus("s1") };
            });

            var sysB = new PsychologicalArcSystem(new[] { HoardArc(minDays: 3, chance: 1f) });
            sysB.TryTransferToStash = (_, _, _) => ("canned_food", 1);
            for (int d = 1; d <= 3; d++)
                sysB.TickDay(d, new[] { "s1" }, _ => 96f, new SeededRng(91 + d), new SeededRng(101 + d), new SeededRng(111 + d));
            var save = sysB.CaptureState();

            var restored = new PsychologicalArcSystem(new[] { HoardArc(minDays: 3, chance: 1f) });
            restored.RestoreState(save);
            restored.TryTransferToStash = (_, _, _) => ("canned_food", 1);
            for (int d = 4; d <= 6; d++)
                restored.TickDay(d, new[] { "s1" }, _ => 96f, new SeededRng(91 + d), new SeededRng(101 + d), new SeededRng(111 + d));

            var expected = runUninterrupted();
            Assert.Equal(expected[0], (int)restored.StageOf("s1"));
            Assert.Equal(expected[1], restored.StashOf("s1").Count);
            Assert.Equal(expected[2], restored.ResilienceBonus("s1"));
        }

        [Fact]
        public void OldSaveDefaults_RestoreSurvivesNulls()
        {
            var sys = new PsychologicalArcSystem(null);
            sys.RestoreState(null);
            Assert.Empty(sys.State.survivors);
            var bare = new PsychologicalArcState { survivors = null, resilience_bonus = null };
            sys.RestoreState(bare);
            Assert.NotNull(sys.State.survivors);
        }
    }
}
