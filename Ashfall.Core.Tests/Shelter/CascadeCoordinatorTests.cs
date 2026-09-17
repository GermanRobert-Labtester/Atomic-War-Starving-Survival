// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// C2[6] 23C — fact-reading cascade authority: rule validation (off-ramp and
    /// warning-floor integrity), deterministic evaluation, and exactly-once
    /// warning/recovery attribution. The coordinator never mutates subsystem
    /// state; these tests assert it is a pure function of the supplied facts.
    /// </summary>
    public class CascadeCoordinatorTests
    {
        private static CascadeRule Rule(string id, List<string> conditions,
            float warningHours = 24f, List<string> offRamps = null)
            => new CascadeRule
            {
                id = id,
                display_name = id,
                requires_all = conditions,
                warning_hours = warningHours,
                effect_tags = new List<string> { "risk" },
                off_ramps = offRamps ?? new List<string> { "restore_power" }
            };

        private static CascadeRuleCatalog Catalog(params CascadeRule[] rules)
            => new CascadeRuleCatalog
            {
                schema_version = 1,
                minimum_warning_hours = 6f,
                rules = new List<CascadeRule>(rules)
            };

        // ---- validation ------------------------------------------------------

        [Fact]
        public void Validate_RejectsRuleWithoutOffRamp()
        {
            var catalog = Catalog(Rule("cascade_a", new List<string> { "power_deficit" },
                offRamps: new List<string>()));
            Assert.False(CascadeRuleCatalogLoader.Validate(catalog, out string error));
            Assert.Contains("off-ramp", error);
        }

        [Fact]
        public void Validate_RejectsWarningBelowFloor()
        {
            var catalog = Catalog(Rule("cascade_a", new List<string> { "power_deficit" }, warningHours: 2f));
            Assert.False(CascadeRuleCatalogLoader.Validate(catalog, out string error));
            Assert.Contains("below the minimum", error);
        }

        [Fact]
        public void Validate_RejectsUnknownConditionAndOffRamp()
        {
            Assert.False(CascadeRuleCatalogLoader.Validate(
                Catalog(Rule("cascade_a", new List<string> { "made_up_fact" })), out string cErr));
            Assert.Contains("unknown condition", cErr);

            Assert.False(CascadeRuleCatalogLoader.Validate(
                Catalog(Rule("cascade_b", new List<string> { "power_deficit" },
                    offRamps: new List<string> { "wishful_thinking" })), out string oErr));
            Assert.Contains("unknown off-ramp", oErr);
        }

        [Fact]
        public void Validate_AcceptsAuthoredCatalog()
        {
            var catalog = Catalog(
                Rule("cascade_brownout_pump_flood",
                    new List<string> { "power_deficit", "pumping_unserved", "sump_rising" }),
                Rule("cascade_filtration_fallout_dose",
                    new List<string> { "filtration_unserved", "fallout_storm" }, warningHours: 6f));
            Assert.True(CascadeRuleCatalogLoader.Validate(catalog, out string error), error);
        }

        // ---- evaluation ------------------------------------------------------

        [Fact]
        public void Evaluate_ReportsActiveStressorAndWarning()
        {
            var coordinator = new CascadeCoordinator(Catalog(
                Rule("cascade_brownout_pump_flood",
                    new List<string> { "power_deficit", "pumping_unserved", "sump_rising" }, warningHours: 24f)));

            var assessment = coordinator.Evaluate(new CascadeFacts
            {
                Day = 3, PowerDeficit = true, PumpingUnserved = true, SumpRising = true
            });

            Assert.Single(assessment.Active);
            Assert.Equal("cascade_brownout_pump_flood", assessment.Active[0].RuleId);
            Assert.Equal(24f, assessment.Active[0].WarningHours);
            Assert.Equal("cascade_brownout_pump_flood", assessment.NextConsequence);
            // First evaluation seeds silently — no replayed transitions.
            Assert.Empty(assessment.Transitions);
        }

        [Fact]
        public void Evaluate_EmitsExactlyOnceWarningThenRecovery()
        {
            var coordinator = new CascadeCoordinator(Catalog(
                Rule("cascade_cold", new List<string> { "power_deficit", "cold_storage_unpowered" })));

            var activeFacts = new CascadeFacts { Day = 1, PowerDeficit = true, ColdStorageUnpowered = true };
            coordinator.Evaluate(activeFacts); // seed

            var second = coordinator.Evaluate(activeFacts);
            Assert.Empty(second.Transitions); // still active, no duplicate warning

            var cleared = coordinator.Evaluate(new CascadeFacts { Day = 2 });
            Assert.Single(cleared.Transitions);
            Assert.Equal("cascade_recovered", cleared.Transitions[0].Kind);
            Assert.Empty(cleared.Active);

            // A later re-activation warns again, exactly once, then stays quiet.
            var reArmed = coordinator.Evaluate(new CascadeFacts { Day = 4, PowerDeficit = true, ColdStorageUnpowered = true });
            Assert.Single(reArmed.Transitions);
            Assert.Equal("cascade_warning", reArmed.Transitions[0].Kind);

            var stillArmed = coordinator.Evaluate(new CascadeFacts { Day = 5, PowerDeficit = true, ColdStorageUnpowered = true });
            Assert.Empty(stillArmed.Transitions);
        }

        [Fact]
        public void Evaluate_OffRampEngaged_RecoversWithAttribution()
        {
            var coordinator = new CascadeCoordinator(Catalog(
                Rule("cascade_brownout_pump_flood",
                    new List<string> { "power_deficit", "pumping_unserved", "sump_rising" },
                    offRamps: new List<string> { "restore_power", "manual_pump" })));

            coordinator.Evaluate(new CascadeFacts { Day = 1, PowerDeficit = true, PumpingUnserved = true, SumpRising = true });

            var facts = new CascadeFacts { Day = 2, PowerDeficit = true, PumpingUnserved = true, SumpRising = true };
            facts.SatisfiedOffRamps.Add("manual_pump");
            var recovered = coordinator.Evaluate(facts);

            Assert.Empty(recovered.Active);
            Assert.Single(recovered.Transitions);
            Assert.Equal("manual_pump", recovered.Transitions[0].OffRamp);
        }

        [Fact]
        public void Evaluate_DeterministicOrder_AndNullSafe()
        {
            var coordinator = new CascadeCoordinator(Catalog(
                Rule("cascade_zeta", new List<string> { "power_deficit" }),
                Rule("cascade_alpha", new List<string> { "power_deficit" })));

            var a = coordinator.Evaluate(new CascadeFacts { Day = 1, PowerDeficit = true });
            var ids = new List<string>();
            foreach (var stage in a.Active) ids.Add(stage.RuleId);
            Assert.Equal(new[] { "cascade_alpha", "cascade_zeta" }, ids);
            Assert.Equal("cascade_alpha", a.NextConsequence);

            // Null facts are safe; a fresh coordinator accepts them.
            var empty = new CascadeCoordinator(Catalog(Rule("cascade_alpha", new List<string> { "power_deficit" })));
            var none = empty.Evaluate(null);
            Assert.Empty(none.Active);
        }

        [Fact]
        public void Evaluate_CriticalDeficit_FlagsCriticalStress()
        {
            var coordinator = new CascadeCoordinator(Catalog(
                Rule("cascade_life_support", new List<string> { "critical_deficit" },
                    offRamps: new List<string> { "restore_power", "shed_other_load" })));
            var assessment = coordinator.Evaluate(new CascadeFacts { Day = 1, CriticalDeficit = true });
            Assert.True(assessment.HasCriticalStress);
        }
    }
}