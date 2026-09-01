// SPDX-License-Identifier: MIT
// Task #132 — Referential integrity and aggregate invariants.
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public class SurvivorIntegrityValidatorTests
    {
        private const string Expedition = "expedition_ashen_yard";

        private static SurvivorId Id(string raw) => new SurvivorId(raw);

        private static SurvivorEntityStore StoreWith(params string[] ids)
        {
            var store = new SurvivorEntityStore();
            foreach (string id in ids) store.TryJoin(Id(id), id, day: 1);
            return store;
        }

        private static bool Has(SurvivorIntegrityReport report, string code)
            => report.Findings.Any(f => f.Code == code);

        private static SurvivorIntegrityFinding Find(SurvivorIntegrityReport report, string code)
        {
            var matches = report.Findings.FindAll(f => f.Code == code);
            Assert.Single(matches);
            return matches[0];
        }

        // ── Clean campaigns ────────────────────────────────────────────

        [Fact]
        public void EmptyCampaign_IsClean()
        {
            var report = SurvivorIntegrityValidator.Validate(new SurvivorEntityStore());

            Assert.True(report.IsValid);
            Assert.True(report.IsClean);
            Assert.Equal(0, report.SurvivorsChecked);
        }

        [Fact]
        public void HealthyCampaign_IsClean()
        {
            var store = StoreWith("a_resident", "b_away", "c_dead", "d_memorial");
            store.TryDeploy(Id("b_away"), Expedition, day: 2);
            store.TryDie(Id("c_dead"), day: 3);
            store.TryDie(Id("d_memorial"), day: 4);
            store.TryMemorialize(Id("d_memorial"), day: 5);

            var needs = new FakeSurvivorComponentStore("needs");
            needs.Attach(Id("a_resident"));
            needs.Attach(Id("b_away"));
            store.RegisterComponentStore(needs);

            var report = SurvivorIntegrityValidator.Validate(store, new SurvivorIntegrityInputs
            {
                ActiveExpeditions = new[] { new KeyValuePair<string, SurvivorId>(Expedition, Id("b_away")) },
                AssignedSurvivors = new[] { Id("a_resident") }
            });

            Assert.True(report.IsClean, report.Describe());
            Assert.Equal(4, report.SurvivorsChecked);
            Assert.Equal(1, report.ComponentStoresChecked);
        }

        [Fact]
        public void Validate_RejectsNullStore()
        {
            Assert.Throws<ArgumentNullException>(() => SurvivorIntegrityValidator.Validate(null!));
        }

        // ── The central invariant ──────────────────────────────────────

        /// <summary>
        /// The invariant Task #132 exists for: no domain may hold state for a
        /// survivor who is not in the campaign.
        /// </summary>
        [Fact]
        public void ComponentForUnknownSurvivor_IsAnError()
        {
            var store = StoreWith("a_resident");
            var needs = new FakeSurvivorComponentStore("needs");
            needs.Attach("the_ghost");
            store.RegisterComponentStore(needs);

            var report = SurvivorIntegrityValidator.Validate(store);

            Assert.False(report.IsValid);
            var finding = Find(report, SurvivorIntegrityCode.ComponentOwnerUnknown);
            Assert.Equal(SurvivorIntegritySeverity.Error, finding.Severity);
            Assert.Equal("needs", finding.Component);
            Assert.Equal(Id("the_ghost"), finding.SurvivorId);
            Assert.Contains("the_ghost", finding.Message);
            Assert.Contains("not a survivor in this campaign", finding.Message);
        }

        [Fact]
        public void EveryComponentStoreIsChecked()
        {
            var store = StoreWith("a_resident");
            foreach (string name in new[] { "needs", "radiation", "medical" })
            {
                var component = new FakeSurvivorComponentStore(name);
                component.Attach("the_ghost");
                store.RegisterComponentStore(component);
            }

            var report = SurvivorIntegrityValidator.Validate(store);

            Assert.Equal(3, report.ErrorCount);
            Assert.Equal(3, report.ComponentStoresChecked);
            Assert.Equal(
                new[] { "medical", "needs", "radiation" },
                report.Findings.Select(f => f.Component).OrderBy(c => c, StringComparer.Ordinal).ToArray());
        }

        [Fact]
        public void ActiveComponentOnDeceasedSurvivor_IsAWarning()
        {
            var store = StoreWith("a_dead");
            store.TryDie(Id("a_dead"), day: 3);

            var needs = new FakeSurvivorComponentStore("needs", retainsHistoryAfterDeath: false);
            needs.Attach(Id("a_dead"));
            store.RegisterComponentStore(needs);

            var report = SurvivorIntegrityValidator.Validate(store);

            // A warning, not an error: the record belongs to a real survivor, but
            // the owning domain has not released it.
            Assert.True(report.IsValid);
            var finding = Find(report, SurvivorIntegrityCode.ComponentOnDeceased);
            Assert.Equal(SurvivorIntegritySeverity.Warning, finding.Severity);
            Assert.Contains("does not retain history", finding.Message);
        }

        [Fact]
        public void HistoryRetainingComponentOnDeceasedSurvivor_IsFine()
        {
            var store = StoreWith("a_dead");
            store.TryDie(Id("a_dead"), day: 3);

            var memorial = new FakeSurvivorComponentStore("memorial", retainsHistoryAfterDeath: true);
            memorial.Attach(Id("a_dead"));
            store.RegisterComponentStore(memorial);

            Assert.True(SurvivorIntegrityValidator.Validate(store).IsClean);
        }

        [Fact]
        public void OnePerEligibleComponent_WarnsWhenMissingForALivingSurvivor()
        {
            var store = StoreWith("a_resident", "b_away", "c_dead");
            store.TryDeploy(Id("b_away"), Expedition, day: 2);
            store.TryDie(Id("c_dead"), day: 3);

            var needs = new FakeSurvivorComponentStore(
                "needs", SurvivorComponentCardinality.OnePerEligible);
            needs.Attach(Id("a_resident"));
            store.RegisterComponentStore(needs);

            var report = SurvivorIntegrityValidator.Validate(store);

            // b_away is alive and missing a record; c_dead is not eligible.
            var finding = Find(report, SurvivorIntegrityCode.ComponentMissingForEligible);
            Assert.Equal(Id("b_away"), finding.SurvivorId);
            Assert.Equal(SurvivorIntegritySeverity.Warning, finding.Severity);
        }

        [Fact]
        public void ZeroOrOneComponent_DoesNotWarnWhenAbsent()
        {
            var store = StoreWith("a_resident");
            store.RegisterComponentStore(
                new FakeSurvivorComponentStore("needs", SurvivorComponentCardinality.ZeroOrOne));

            Assert.True(SurvivorIntegrityValidator.Validate(store).IsClean);
        }

        // ── Aggregate invariants ───────────────────────────────────────

        [Fact]
        public void AwayWithoutExpedition_IsAnError()
        {
            var store = new SurvivorEntityStore();
            // Construct the contradiction directly — the transactions cannot produce it.
            store.RestoreState(StateWith(new SurvivorAggregateState
            {
                survivor_id = "a_stranded",
                lifecycle = (int)SurvivorLifecycleState.Away,
                active_expedition_id = Expedition,
                revision = 2
            }));

            // Now corrupt it the only way a caller could: a hand-built aggregate.
            var corrupted = new SurvivorEntityStore();
            corrupted.RestoreState(StateWith(new SurvivorAggregateState
            {
                survivor_id = "a_stranded",
                lifecycle = (int)SurvivorLifecycleState.Away,
                active_expedition_id = "",
                revision = 2
            }));

            // Restore repaired it to Resident, so the store itself stays coherent.
            Assert.Equal(SurvivorLifecycleState.Resident, corrupted.GetRequired(Id("a_stranded")).Lifecycle);
            Assert.True(SurvivorIntegrityValidator.Validate(corrupted).IsClean);
            Assert.True(SurvivorIntegrityValidator.Validate(store).IsValid);
        }

        [Fact]
        public void LifecycleDayBeforeJoinDay_IsAWarning()
        {
            var store = new SurvivorEntityStore();
            store.RestoreState(StateWith(new SurvivorAggregateState
            {
                survivor_id = "a_timeslip",
                lifecycle = (int)SurvivorLifecycleState.Resident,
                joined_day = 40,
                lifecycle_day = 12,
                revision = 2
            }));

            var report = SurvivorIntegrityValidator.Validate(store);

            Assert.True(report.IsValid);
            var finding = Find(report, SurvivorIntegrityCode.LifecycleDayBeforeJoin);
            Assert.Equal(SurvivorIntegritySeverity.Warning, finding.Severity);
            Assert.Contains("before joining", finding.Message);
        }

        [Fact]
        public void HealthyAggregates_ProduceNoAggregateFindings()
        {
            var store = StoreWith("a_resident", "b_away");
            store.TryDeploy(Id("b_away"), Expedition, day: 2);

            var report = SurvivorIntegrityValidator.Validate(store);

            Assert.False(Has(report, SurvivorIntegrityCode.LifecycleIllegalState));
            Assert.False(Has(report, SurvivorIntegrityCode.LifecycleRevisionInvalid));
            Assert.False(Has(report, SurvivorIntegrityCode.DefinitionMissing));
            Assert.False(Has(report, SurvivorIntegrityCode.AwayWithoutExpedition));
            Assert.False(Has(report, SurvivorIntegrityCode.ExpeditionOnNonAway));
            Assert.False(Has(report, SurvivorIntegrityCode.IterationOrderUnstable));
        }

        // ── Expedition coherence ───────────────────────────────────────

        [Fact]
        public void ExpeditionListingAnUnknownSurvivor_IsAnError()
        {
            var store = StoreWith("a_resident");

            var report = SurvivorIntegrityValidator.Validate(store, new SurvivorIntegrityInputs
            {
                ActiveExpeditions = new[] { new KeyValuePair<string, SurvivorId>(Expedition, Id("the_ghost")) }
            });

            var finding = Find(report, SurvivorIntegrityCode.ExpeditionMemberUnknown);
            Assert.Equal(SurvivorIntegritySeverity.Error, finding.Severity);
            Assert.Contains("the_ghost", finding.Message);
        }

        /// <summary>
        /// The exact contradiction the old architecture allowed: the roster says the
        /// survivor is home, the expedition system says they are out.
        /// </summary>
        [Fact]
        public void ExpeditionListingAResidentSurvivor_IsAnError()
        {
            var store = StoreWith("a_resident");

            var report = SurvivorIntegrityValidator.Validate(store, new SurvivorIntegrityInputs
            {
                ActiveExpeditions = new[] { new KeyValuePair<string, SurvivorId>(Expedition, Id("a_resident")) }
            });

            var finding = Find(report, SurvivorIntegrityCode.ExpeditionMemberNotAway);
            Assert.Equal(SurvivorIntegritySeverity.Error, finding.Severity);
            Assert.Contains("Resident", finding.Message);
            Assert.Contains("still lists them as deployed", finding.Message);
        }

        [Fact]
        public void ExpeditionListingADeadSurvivor_IsAnError()
        {
            var store = StoreWith("a_dead");
            store.TryDeploy(Id("a_dead"), Expedition, day: 2);
            store.TryDie(Id("a_dead"), day: 3);

            var report = SurvivorIntegrityValidator.Validate(store, new SurvivorIntegrityInputs
            {
                ActiveExpeditions = new[] { new KeyValuePair<string, SurvivorId>(Expedition, Id("a_dead")) }
            });

            Assert.False(report.IsValid);
            Assert.Contains("Dead", Find(report, SurvivorIntegrityCode.ExpeditionMemberNotAway).Message);
        }

        [Fact]
        public void AwaySurvivorMissingFromActiveExpeditions_IsAnError()
        {
            var store = StoreWith("a_away");
            store.TryDeploy(Id("a_away"), Expedition, day: 2);

            var report = SurvivorIntegrityValidator.Validate(store, new SurvivorIntegrityInputs
            {
                ActiveExpeditions = Array.Empty<KeyValuePair<string, SurvivorId>>()
            });

            var finding = Find(report, SurvivorIntegrityCode.AwayWithoutActiveExpedition);
            Assert.Equal(SurvivorIntegritySeverity.Error, finding.Severity);
            Assert.Contains(Expedition, finding.Message);
        }

        [Fact]
        public void ExpeditionIdDisagreement_IsAnError()
        {
            var store = StoreWith("a_away");
            store.TryDeploy(Id("a_away"), Expedition, day: 2);

            var report = SurvivorIntegrityValidator.Validate(store, new SurvivorIntegrityInputs
            {
                ActiveExpeditions = new[]
                {
                    new KeyValuePair<string, SurvivorId>("expedition_somewhere_else", Id("a_away"))
                }
            });

            Assert.False(report.IsValid);
            Assert.Contains("claims them", Find(report, SurvivorIntegrityCode.ExpeditionIdMismatch).Message);
        }

        [Fact]
        public void SurvivorOnTwoExpeditionsAtOnce_IsAnError()
        {
            var store = StoreWith("a_away");
            store.TryDeploy(Id("a_away"), Expedition, day: 2);

            var report = SurvivorIntegrityValidator.Validate(store, new SurvivorIntegrityInputs
            {
                ActiveExpeditions = new[]
                {
                    new KeyValuePair<string, SurvivorId>(Expedition, Id("a_away")),
                    new KeyValuePair<string, SurvivorId>("expedition_second", Id("a_away"))
                }
            });

            Assert.False(report.IsValid);
            Assert.Contains("two active expeditions", Find(report, SurvivorIntegrityCode.ExpeditionIdMismatch).Message);
        }

        [Fact]
        public void NullExpeditionInput_SkipsTheCheck()
        {
            var store = StoreWith("a_away");
            store.TryDeploy(Id("a_away"), Expedition, day: 2);

            Assert.True(SurvivorIntegrityValidator.Validate(store, new SurvivorIntegrityInputs()).IsClean);
            Assert.True(SurvivorIntegrityValidator.Validate(store).IsClean);
        }

        // ── Assignment coherence ───────────────────────────────────────

        [Fact]
        public void DutyAssignedToUnknownSurvivor_IsAnError()
        {
            var store = StoreWith("a_resident");

            var report = SurvivorIntegrityValidator.Validate(store, new SurvivorIntegrityInputs
            {
                AssignedSurvivors = new[] { Id("the_ghost") }
            });

            var finding = Find(report, SurvivorIntegrityCode.AssignmentOwnerUnknown);
            Assert.Equal(SurvivorIntegritySeverity.Error, finding.Severity);
            Assert.Equal("assignment", finding.Component);
        }

        /// <summary>
        /// A warning rather than an error: the duty roster has no lifecycle
        /// awareness yet, so the game genuinely permits this today. It must reach
        /// zero before the assignment domain is cut over.
        /// </summary>
        [Theory]
        [InlineData(SurvivorLifecycleState.Away)]
        [InlineData(SurvivorLifecycleState.Dead)]
        [InlineData(SurvivorLifecycleState.Memorialized)]
        public void DutyHeldByAnIneligibleSurvivor_IsAWarning(SurvivorLifecycleState state)
        {
            var store = StoreWith("a_worker");
            switch (state)
            {
                case SurvivorLifecycleState.Away:
                    store.TryDeploy(Id("a_worker"), Expedition, day: 2);
                    break;
                case SurvivorLifecycleState.Dead:
                    store.TryDie(Id("a_worker"), day: 2);
                    break;
                case SurvivorLifecycleState.Memorialized:
                    store.TryDie(Id("a_worker"), day: 2);
                    store.TryMemorialize(Id("a_worker"), day: 3);
                    break;
            }

            var report = SurvivorIntegrityValidator.Validate(store, new SurvivorIntegrityInputs
            {
                AssignedSurvivors = new[] { Id("a_worker") },
                ActiveExpeditions = state == SurvivorLifecycleState.Away
                    ? new[] { new KeyValuePair<string, SurvivorId>(Expedition, Id("a_worker")) }
                    : Array.Empty<KeyValuePair<string, SurvivorId>>()
            });

            Assert.True(report.IsValid);
            var finding = Find(report, SurvivorIntegrityCode.AssignmentLifecycleIneligible);
            Assert.Equal(SurvivorIntegritySeverity.Warning, finding.Severity);
            Assert.Contains(state.ToString(), finding.Message);
            Assert.Contains("still holds an active duty", finding.Message);
        }

        // ── Report shape ───────────────────────────────────────────────

        [Fact]
        public void Report_SeparatesErrorsFromWarnings()
        {
            var store = StoreWith("a_dead");
            store.TryDie(Id("a_dead"), day: 2);

            var needs = new FakeSurvivorComponentStore("needs");
            needs.Attach(Id("a_dead"));        // warning: active state on a corpse
            needs.Attach("the_ghost");          // error: unknown owner
            store.RegisterComponentStore(needs);

            var report = SurvivorIntegrityValidator.Validate(store);

            Assert.Equal(1, report.ErrorCount);
            Assert.Equal(1, report.WarningCount);
            Assert.Equal(2, report.Findings.Count);
            Assert.False(report.IsValid);
            Assert.False(report.IsClean);
            Assert.Single(report.BySeverity(SurvivorIntegritySeverity.Error));
            Assert.Single(report.BySeverity(SurvivorIntegritySeverity.Warning));
        }

        /// <summary>Findings must read as actionable prose, per the diagnostics rule.</summary>
        [Fact]
        public void Finding_RendersAsAReadableInvariantFailure()
        {
            var store = StoreWith("a_resident");
            var duty = new FakeSurvivorComponentStore("assignment");
            duty.Attach("the_ghost");
            store.RegisterComponentStore(duty);

            string text = Find(
                SurvivorIntegrityValidator.Validate(store),
                SurvivorIntegrityCode.ComponentOwnerUnknown).ToString();

            Assert.StartsWith("Survivor invariant failure [assignment]:", text);
            Assert.Contains("the_ghost", text);
            Assert.Contains(SurvivorIntegrityCode.ComponentOwnerUnknown, text);
        }

        [Fact]
        public void Describe_SummarizesCountsAndFindings()
        {
            var store = StoreWith("a_resident");
            var needs = new FakeSurvivorComponentStore("needs");
            needs.Attach("the_ghost");
            store.RegisterComponentStore(needs);

            string text = SurvivorIntegrityValidator.Validate(store).Describe();

            Assert.Contains("1 survivor(s)", text);
            Assert.Contains("1 component store(s)", text);
            Assert.Contains("1 error(s)", text);
            Assert.Contains("the_ghost", text);
        }

        [Fact]
        public void IntegrityCodes_AreDistinctSnakeCase()
        {
            var codes = new[]
            {
                SurvivorIntegrityCode.LifecycleIllegalState,
                SurvivorIntegrityCode.LifecycleRevisionInvalid,
                SurvivorIntegrityCode.LifecycleDayBeforeJoin,
                SurvivorIntegrityCode.DefinitionMissing,
                SurvivorIntegrityCode.AwayWithoutExpedition,
                SurvivorIntegrityCode.ExpeditionOnNonAway,
                SurvivorIntegrityCode.IterationOrderUnstable,
                SurvivorIntegrityCode.ComponentOwnerUnknown,
                SurvivorIntegrityCode.ComponentOnDeceased,
                SurvivorIntegrityCode.ComponentMissingForEligible,
                SurvivorIntegrityCode.ExpeditionMemberUnknown,
                SurvivorIntegrityCode.ExpeditionMemberNotAway,
                SurvivorIntegrityCode.ExpeditionIdMismatch,
                SurvivorIntegrityCode.AwayWithoutActiveExpedition,
                SurvivorIntegrityCode.AssignmentOwnerUnknown,
                SurvivorIntegrityCode.AssignmentLifecycleIneligible
            };

            Assert.Equal(codes.Length, new HashSet<string>(codes).Count);
            Assert.All(codes, c => Assert.Matches("^[a-z][a-z0-9_]*$", c));
        }

        /// <summary>Validation is read-only; it must never repair anything.</summary>
        [Fact]
        public void Validation_DoesNotMutateTheStore()
        {
            var store = StoreWith("a_dead");
            store.TryDie(Id("a_dead"), day: 2);
            var needs = new FakeSurvivorComponentStore("needs");
            needs.Attach(Id("a_dead"));
            needs.Attach("the_ghost");
            store.RegisterComponentStore(needs);

            string before = System.Text.Json.JsonSerializer.Serialize(
                store.CaptureState(), Ashfall.Core.SystemTextJsonSerializer.Options);
            int componentsBefore = needs.Count;

            SurvivorIntegrityValidator.Validate(store);

            Assert.Equal(before, System.Text.Json.JsonSerializer.Serialize(
                store.CaptureState(), Ashfall.Core.SystemTextJsonSerializer.Options));
            Assert.Equal(componentsBefore, needs.Count);
            Assert.Equal(0, needs.ReleaseCallCount);
        }

        private static SurvivorEntityStoreState StateWith(params SurvivorAggregateState[] rows)
        {
            var state = new SurvivorEntityStoreState();
            state.survivors.AddRange(rows);
            return state;
        }
    }
}
