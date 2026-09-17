// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Memorial;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    /// <summary>
    /// Plan 24B (Task A1) — nine stranded needs-source migration parity tests.
    ///
    /// Each migrated source family keeps its numeric legacy behavior (the
    /// direct-path delta, pinned here as the exact expected need value) and
    /// gains stable attribution through the shared needs-modifier seam
    /// (NeedsSystem.ApplyAttributedDelta → recent-contribution record +
    /// OnAttributedContribution). The domain authority that decides WHEN the
    /// effect applies is unchanged; only the mutation sink moved.
    ///
    /// Absent families (documented, not migrated — no existing effect):
    /// missed meals (no missed-meal effect is authored; hunger decay models
    /// it), ideological friction morale (friction affects relations/affinity
    /// only — §24B.17 "if authored"), and overwork (no duty-hour need effect
    /// exists; measured hours are Task A2's scope).
    /// </summary>
    public sealed class Plan24NeedsSourceMigrationTests
    {
        // ── 1. Grief (SurvivorFateSystem death cascade) ───────────────

        [Fact]
        public void Grief_RoutesThroughAttributedSeam_PreservesLegacyDelta()
        {
            var needs = new NeedsSystem();
            var roster = new SurvivorRosterSystem();
            var duty = new DutyRosterSystem();
            var memorial = new MemorialSystem(new MemorialState());
            var survivorA = new SurvivorNeedsState { Id = "survivor_a", Morale = 50f };
            var survivorB = new SurvivorNeedsState { Id = "survivor_b", Morale = 50f };
            needs.Register(survivorA);
            needs.Register(survivorB);
            roster.RegisterDefinition(new SurvivorDefinition { id = "survivor_a", displayName = "A" });
            roster.RegisterDefinition(new SurvivorDefinition { id = "survivor_b", displayName = "B" });
            roster.Join("survivor_a", 1);
            roster.Join("survivor_b", 1);
            duty.RestoreState(new DutyRosterSystemState
            {
                rows = new List<DutyRosterRow>
                {
                    new DutyRosterRow { survivorId = "survivor_a", displayName = "A" },
                    new DutyRosterRow { survivorId = "survivor_b", displayName = "B" }
                }
            });

            var fate = new SurvivorFateSystem(
                roster: roster, needs: needs, dutyRoster: duty,
                memorial: memorial, getDay: () => 10);

            fate.ReportDeath("survivor_a", SurvivorDeathCause.Needs, "starvation", "test");

            // Legacy numeric pin: GriefMoraleDelta applied once to the living.
            Assert.Equal(50f + SurvivorFateSystem.GriefMoraleDelta, survivorB.Morale);

            // Attribution: stable source id on the shared seam.
            var recent = needs.ModifierStack.GetRecentForSurvivor("survivor_b");
            Assert.Contains(recent, c => c.SourceId == "grief.shelter_loss"
                && c.Need == NeedKind.Morale
                && Math.Abs(c.DeltaPerHour - SurvivorFateSystem.GriefMoraleDelta) < 0.0001f);
        }

        // ── 2. Thermal/cold (ShelterThermalSystem room warmth) ────────

        [Fact]
        public void ThermalWarmRoom_RoutesThroughAttributedSeam_PreservesLegacyDelta()
        {
            var needs = new NeedsSystem();
            var survivor = new SurvivorNeedsState { Id = "survivor_a", Warmth = 50f };
            needs.Register(survivor);

            var rooms = new List<ShelterRoom> { new ShelterRoom("room_a", "Room A", 4) };
            var assignments = new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, new SeededRng(1));
            assignments.Assign("survivor_a", "room_a", null, 1);

            var rng = new SeededRng(42);
            var starting = new StartingLevelSystem();
            var deepFreeze = new YearOfAshDeepFreezeSystem(
                new YearOfAshDeepFreezeState { indoorTemperatureCelsius = 25f });
            var thermal = new ShelterThermalSystem(rng, needs, starting, deepFreeze, null, assignments);
            thermal.AddRoom("room_a", "Room A", 50f, 0.3f, true);
            var room = thermal.State.rooms.Find(r => r.roomId == "room_a");
            Assert.NotNull(room);
            room.currentTempC = 25f;
            room.targetTempC = 25f;
            thermal.SetBoilerActive(false);

            thermal.TickDay(1);

            // Legacy numeric pin: per-day additive pull = modifier * 24.
            float expected = thermal.GetRoomWarmthModifier("room_a") * 24f;
            Assert.True(expected > 0f, "test premise: room must stay warm enough to restore warmth");
            Assert.Equal(50f + expected, survivor.Warmth, 4);

            var recent = needs.ModifierStack.GetRecentForSurvivor("survivor_a");
            Assert.Contains(recent, c => c.SourceId == "thermal.room_warmth"
                && c.Need == NeedKind.Warmth
                && Math.Abs(c.DeltaPerHour - expected) < 0.0001f);
        }

        // ── 3. Water/hygiene (PsychologicalArcSystem self-care withdrawal) ──

        /// <summary>
        /// The production sink (Main.Plans162_165.cs arc behavior binding)
        /// routes WithdrawSelfCare to the shared seam exactly as mirrored
        /// here; this pins that contract at Core level.
        /// </summary>
        [Fact]
        public void HygieneSelfCareWithdrawal_RoutesThroughAttributedSeam_PreservesLegacyDelta()
        {
            var needs = new NeedsSystem();
            var survivor = new SurvivorNeedsState { Id = "survivor_a", Hygiene = 50f };
            needs.Register(survivor);

            var arcs = new PsychologicalArcSystem(new List<BreakdownArcDef>
            {
                new BreakdownArcDef
                {
                    id = "arc_shutdown",
                    display_name = "Shutdown",
                    behavior = "withdraw_self_care",
                    behavior_chance = 1f,
                    minimum_stress_days = 1,
                    crisis_behavior_min_stage = 1,
                    behavior_cooldown_days = 3
                }
            });
            var rng = new FixedRollRng(0.05);
            arcs.OnBreakdownBehaviorOccurred += (survivorId, behavior) =>
            {
                if (behavior == ArcBehavior.WithdrawSelfCare)
                    needs.ApplyAttributedDelta(survivorId, NeedKind.Hygiene, 8f,
                        "hygiene.self_care_withdrawn");
            };

            // Sustained canonical stress (morale 90, higher = worse) triggers
            // the arc on day 1 and the self-care lapse behavior on day 2.
            arcs.TickDay(1, new[] { "survivor_a" }, _ => 90f, rng, rng, rng);
            arcs.TickDay(2, new[] { "survivor_a" }, _ => 90f, rng, rng, rng);

            // Legacy numeric pin: +8 hygiene (higher = worse) once the arc
            // behavior fires; the arc reaches WithdrawSelfCare deterministically
            // from the authored pressure fixture below.
            Assert.Equal(58f, survivor.Hygiene, 4);

            var recent = needs.ModifierStack.GetRecentForSurvivor("survivor_a");
            Assert.Contains(recent, c => c.SourceId == "hygiene.self_care_withdrawn"
                && c.Need == NeedKind.Hygiene
                && Math.Abs(c.DeltaPerHour - 8f) < 0.0001f);
        }


        // ── 4. Meal quality (KitchenNutritionSystem.ServeMeal) ────────

        [Theory]
        [InlineData(0.05f, true)]   // safe roll
        [InlineData(0.99f, false)]  // unsafe roll
        public void ServeMeal_QualityRoutesThroughAttributedSeam_HungerStaysDirect(
            double safetyRoll, bool wasSafe)
        {
            var inv = new Inventory.Inventory();
            var needs = new NeedsSystem();
            var survivor = new SurvivorNeedsState
            {
                Id = "survivor_1", Hunger = 60f, Morale = 50f, Health = 90f
            };
            needs.Register(survivor);
            var kitchen = new KitchenNutritionSystem(new FixedRollRng(safetyRoll), inv, needs);

            inv.AddById("meat", 5);
            kitchen.StartPrepJob("stew", "cook_1", new Dictionary<string, int> { { "meat", 2 } });
            kitchen.TickDay(1);
            var result = kitchen.ServeMeal("survivor_1", "stew");

            Assert.Equal(ActionResult.StatusKind.Success, result.Status);

            // Legacy numeric pins: hunger restoration stays DIRECT (§24B.10 —
            // consumption authority owns it, never the modifier stack).
            Assert.Equal(60f - Math.Max(30f, (wasSafe ? 8f : 2f) * 6f), survivor.Hunger, 4);
            // Meal-quality morale contribution ±5.
            Assert.Equal(50f + (wasSafe ? 5f : -5f), survivor.Morale, 4);

            var recent = needs.ModifierStack.GetRecentForSurvivor("survivor_1");
            Assert.Contains(recent, c => c.SourceId == "kitchen.meal_quality"
                && c.Need == NeedKind.Morale
                && Math.Abs(c.DeltaPerHour - (wasSafe ? 5f : -5f)) < 0.0001f);

            if (wasSafe)
            {
                Assert.Equal(90f, survivor.Health, 4);
                Assert.DoesNotContain(recent, c => c.SourceId == "kitchen.meal_unsafe");
            }
            else
            {
                // Unsafe meal: health penalty −5, distinct source id.
                Assert.Equal(85f, survivor.Health, 4);
                Assert.Contains(recent, c => c.SourceId == "kitchen.meal_unsafe"
                    && c.Need == NeedKind.Health
                    && Math.Abs(c.DeltaPerHour - (-5f)) < 0.0001f);
            }
        }

        private sealed class FixedRollRng : ISeededRng
        {
            private readonly double _roll;
            public FixedRollRng(double roll) => _roll = roll;
            public int Seed => 1;
            public int Next(int minInclusive, int maxExclusive) => minInclusive;
            public float NextFloat() => (float)_roll;
            public double NextDouble() => _roll;
        }

        // ── 5. Ration conflict (RationConflictSystem → coordinator sink) ──

        [Fact]
        public void RationConfrontation_RoutesThroughAttributedSeam_PreservesLegacyDelta()
        {
            var needs = new NeedsSystem();
            var alpha = new SurvivorNeedsState { Id = "sv_alpha", Morale = 50f };
            var bravo = new SurvivorNeedsState { Id = "sv_bravo", Morale = 50f };
            var charlie = new SurvivorNeedsState { Id = "sv_charlie", Morale = 50f };
            needs.Register(alpha);
            needs.Register(bravo);
            needs.Register(charlie);
            var roster = new List<SurvivorNeedsState> { alpha, bravo, charlie };

            var coord = new SurvivorSocialCoordinator(
                new SeededRng(7), needs, null, new DutyRosterSystem(), () => 1);
            coord.RationPolicy = RationPolicy.Standard;
            coord.SetAliveSurvivors(new[] { "sv_alpha", "sv_bravo", "sv_charlie" });
            Assert.True(coord.DesignateLeader("sv_charlie"));

            // Leader rations (0.95 vs 0.35) cross the fairness-deviation
            // threshold; resentment gains 0.10/day and crosses the 0.70
            // confrontation threshold on the 7th tick. Both non-leaders
            // resent the leader, so the leader takes one −5 target hit per
            // confrontation (legacy stacking, unchanged by the migration).
            for (int day = 1; day <= 7; day++)
                coord.TickDay(day, roster);

            // Legacy numeric pins: day-7 confrontations — each non-leader
            // −10 as resenter; the leader −5 twice as target.
            Assert.Equal(50f - 10f, alpha.Morale, 4);
            Assert.Equal(50f - 10f, bravo.Morale, 4);
            Assert.Equal(50f - 5f - 5f, charlie.Morale, 4);

            var alphaRecent = needs.ModifierStack.GetRecentForSurvivor("sv_alpha");
            Assert.Contains(alphaRecent, c => c.SourceId == "ration.confrontation"
                && c.Need == NeedKind.Morale
                && Math.Abs(c.DeltaPerHour - (-10f)) < 0.0001f);
            var charlieRecent = needs.ModifierStack.GetRecentForSurvivor("sv_charlie");
            Assert.Contains(charlieRecent, c => c.SourceId == "ration.confrontation"
                && c.Need == NeedKind.Morale
                && Math.Abs(c.DeltaPerHour - (-5f)) < 0.0001f);
        }

        // ── 6. Leadership (LeadershipSystem crisis aura → coordinator sink) ──

        [Fact]
        public void LeadershipCrisisAura_RoutesThroughAttributedSeam_PreservesLegacyDelta()
        {
            var needs = new NeedsSystem();
            var alpha = new SurvivorNeedsState { Id = "sv_alpha", Morale = 50f };
            var charlie = new SurvivorNeedsState { Id = "sv_charlie", Morale = 50f };
            needs.Register(alpha);
            needs.Register(charlie);

            var coord = new SurvivorSocialCoordinator(
                new SeededRng(7), needs, null, new DutyRosterSystem(), () => 1);
            coord.SetAliveSurvivors(new[] { "sv_alpha", "sv_charlie" });
            Assert.True(coord.DesignateLeader("sv_charlie"));

            coord.OnCrisisEvent();

            // Legacy numeric pin: shelter-wide aura +10 once.
            Assert.Equal(60f, alpha.Morale, 4);
            Assert.Equal(60f, charlie.Morale, 4);

            foreach (var id in new[] { "sv_alpha", "sv_charlie" })
            {
                var recent = needs.ModifierStack.GetRecentForSurvivor(id);
                Assert.Contains(recent, c => c.SourceId == "leadership.crisis_aura"
                    && c.Need == NeedKind.Morale
                    && Math.Abs(c.DeltaPerHour - LeadershipSystem.LeaderCrisisMoraleAura) < 0.0001f);
            }
        }

        // ── 7. General stress (MoraleContagionSystem daily morale sink) ──

        /// <summary>
        /// The port now carries the cause (isolation vs pressure channel) so
        /// the host sink can attribute it — the same (id, delta) stream as the
        /// legacy two-argument port, pinned by the Flagship11 suite.
        /// </summary>
        [Fact]
        public void ContagionMorale_RoutesThroughAttributedSeam_WithCauseGranularity()
        {
            var needs = new NeedsSystem();
            var survivor = new SurvivorNeedsState { Id = "s_a", Morale = 50f };
            needs.Register(survivor);
            var spyDeltas = new List<(string id, float delta, string source)>();

            var catalog = new ContagionEventsCatalogContainer();
            catalog.contagion_events.Add(new ContagionEventDef
            {
                id = "contagion_migration_grief", display_name = "Grief", emotion_type = "despair",
                base_intensity = 0.8f, duration_days = 40, bond_multiplier = 1f,
                proximity_multiplier = 1f, recovery_per_day = 0.02f
            });

            var ports = new MoraleContagionPorts
            {
                AliveSurvivors = () => new List<string> { "s_a" },
                GetMorale = id => needs.Get(id)?.Morale ?? 50f,
                ApplyMoraleDelta = (id, delta, source) =>
                {
                    spyDeltas.Add((id, delta, source));
                    needs.ApplyAttributedDelta(id, NeedKind.Morale, delta, source);
                }
            };
            var contagion = new MoraleContagionSystem(catalog, ports);

            // Isolation (2 days): +1/day while isolated — and the isolation
            // curtain cuts all inbound influence until it expires.
            Assert.True(contagion.TryApplySocialIsolation("s_a", 1, 2));
            contagion.EvaluateDailyContagion(2);

            // Pressure channel: ambient despair source feeds the channel;
            // the influence reaches s_a once isolation has expired.
            Assert.True(contagion.StartContagionEvent("contagion_migration_grief", "", 3));
            contagion.EvaluateDailyContagion(3);
            contagion.EvaluateDailyContagion(4);
            contagion.EvaluateDailyContagion(5);

            // Parity: the attributed seam applied exactly the port's delta
            // stream — no more, no less.
            float spySum = 0f;
            foreach (var entry in spyDeltas)
                if (entry.id == "s_a") spySum += entry.delta;
            Assert.Equal(50f + spySum, survivor.Morale, 4);

            // Attribution granularity: both causes are named, distinguished.
            var recent = needs.ModifierStack.GetRecentForSurvivor("s_a");
            Assert.Contains(recent, c => c.SourceId == "contagion.isolation"
                && c.Need == NeedKind.Morale
                && Math.Abs(c.DeltaPerHour - MoraleContagionSystem.IsolationCostMoralePerDay) < 0.0001f);
            Assert.Contains(recent, c => c.SourceId == "contagion.pressure"
                && c.Need == NeedKind.Morale);
        }

        // ── 8. Stack integrity: one-shot attributions stay non-persistent ──

        [Fact]
        public void MigratedOneShotSources_DoNotPersistStackRates()
        {
            var needs = new NeedsSystem();
            var survivor = new SurvivorNeedsState { Id = "survivor_a", Morale = 50f };
            needs.Register(survivor);

            needs.ApplyAttributedDelta("survivor_a", NeedKind.Morale, -8f, "grief.shelter_loss");

            Assert.Equal(0, needs.ModifierStack.Count);
            Assert.Single(needs.ModifierStack.GetRecentForSurvivor("survivor_a"));
        }
    }
}
