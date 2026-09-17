// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.DutyRoster;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.DutyRoster
{
    /// <summary>
    /// Plan 24B (Task A2) — duty-hour accumulator, measured-overwork needs
    /// routing, and the shared worker-productivity (skill-to-yield) contract.
    /// Legacy parity: the unbound paths are byte-identical to pre-A2 behavior.
    /// </summary>
    public sealed class Plan24DutyHourTests
    {
        private static RoleFitnessVerdict Verdict(
            string survivorId, string roleId, float recommendedHours,
            FitnessLevel level = FitnessLevel.Fit)
        {
            var baseVerdict = new FitnessVerdict(
                survivorId, level,
                level == FitnessLevel.Incapacitated ? new[] { "dead" } : Array.Empty<string>(),
                level == FitnessLevel.Impaired ? new[] { "severe_fatigue" } : Array.Empty<string>(),
                level == FitnessLevel.Impaired ? new[] { NeedKind.Fatigue } : Array.Empty<NeedKind>());
            bool allowed = level != FitnessLevel.Incapacitated;
            return new RoleFitnessVerdict(
                survivorId, roleId, baseVerdict, allowed,
                allowed && level != FitnessLevel.Fit,
                baseVerdict.BlockingReasons,
                baseVerdict.DegradedFactors,
                recommendedHours);
        }

        private sealed class LedgerFixture
        {
            public DutyRosterSystem Roster = NewRoster();
            public Dictionary<string, float> RoleHours = new(StringComparer.Ordinal)
            {
                [DutyRosterIds.RoleNightWatch] = 12f,
                [DutyRosterIds.RoleMess] = 12f,
            };
            public Dictionary<string, RoleFitnessVerdict> Verdicts = new(StringComparer.Ordinal);
            public DutyHourLedger Ledger;

            private static DutyRosterSystem NewRoster()
            {
                var roster = new DutyRosterSystem(1208);
                roster.Unlock(59);
                roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
                roster.TickMorning(61, new List<DutyRosterOccupant>
                {
                    new DutyRosterOccupant { survivorId = "survivor_a", displayName = "A", sleptHere = true },
                    new DutyRosterOccupant { survivorId = "survivor_b", displayName = "B", sleptHere = true }
                });
                return roster;
            }

            public LedgerFixture()
            {
                Roster.EvaluateRoleFitness = (id, role) =>
                    Verdicts.TryGetValue(id + "|" + role, out var v) ? v : null;
                Roster.AssignWithResult(DutyRosterIds.RoleNightWatch, "survivor_a");
                Ledger = new DutyHourLedger(
                    getRoleOf: id => Roster.GetRoleOf(id) ?? string.Empty,
                    getRoleCommittedHours: roleId =>
                        RoleHours.TryGetValue(roleId, out var h) ? h : DutyHourLedger.DefaultRoleHours,
                    previewRoleFitness: (id, role) =>
                        Verdicts.TryGetValue(id + "|" + role, out var v) ? v : null,
                    getAssignedSurvivorIds: () =>
                    {
                        var ids = new List<string>();
                        foreach (var a in Roster.State.assignments)
                            if (a != null && !string.IsNullOrEmpty(a.survivorId))
                                ids.Add(a.survivorId);
                        return ids;
                    });
            }
        }

        // ── 1. Duty-hour accumulator derivation ───────────────────────

        [Fact]
        public void Ledger_CommittedHoursFromAssignment_RecommendationFromVerdict()
        {
            var f = new LedgerFixture();

            // Fit survivor on a 12h role: committed 12, recommended 12 — never overwork.
            f.Verdicts["survivor_a|" + DutyRosterIds.RoleNightWatch] =
                Verdict("survivor_a", DutyRosterIds.RoleNightWatch, 12f);
            var fit = f.Ledger.For("survivor_a");
            Assert.Equal(12f, fit.CommittedHours, 4);
            Assert.Equal(12f, fit.RecommendedHours, 4);
            Assert.False(fit.IsOverworked);
            Assert.Equal(DutyRosterIds.RoleNightWatch, fit.RoleId);

            // Fitness recedes overnight (impaired verdict recommends 8h) while
            // the assignment persists — the measurable overwork window.
            f.Verdicts["survivor_a|" + DutyRosterIds.RoleNightWatch] =
                Verdict("survivor_a", DutyRosterIds.RoleNightWatch, 8f, FitnessLevel.Impaired);
            var over = f.Ledger.For("survivor_a");
            Assert.Equal(12f, over.CommittedHours, 4);
            Assert.Equal(8f, over.RecommendedHours, 4);
            Assert.True(over.IsOverworked);
            Assert.Equal(4f, over.ExcessHours, 4);
        }

        [Fact]
        public void Ledger_UnassignedSurvivorIsNeverOverworked()
        {
            var f = new LedgerFixture();
            var snapshot = f.Ledger.For("survivor_b");
            Assert.Equal(0f, snapshot.CommittedHours, 4);
            Assert.Equal(0f, snapshot.RecommendedHours, 4);
            Assert.False(snapshot.IsOverworked);
            Assert.Equal(string.Empty, snapshot.RoleId);
        }

        [Fact]
        public void Ledger_OverworkedListIsDeterministicallyOrdered()
        {
            var f = new LedgerFixture();
            f.Roster.AssignWithResult(DutyRosterIds.RoleMess, "survivor_b");
            f.Verdicts["survivor_a|" + DutyRosterIds.RoleNightWatch] =
                Verdict("survivor_a", DutyRosterIds.RoleNightWatch, 8f, FitnessLevel.Impaired);
            f.Verdicts["survivor_b|" + DutyRosterIds.RoleMess] =
                Verdict("survivor_b", DutyRosterIds.RoleMess, 6f, FitnessLevel.Impaired);

            var overworked = f.Ledger.Overworked();
            Assert.Equal(2, overworked.Count);
            Assert.Equal("survivor_a", overworked[0].SurvivorId); // ordinal order
            Assert.Equal("survivor_b", overworked[1].SurvivorId);
        }

        // ── 2. Overwork needs rates (the A1 overwork source slot, live) ──

        [Fact]
        public void OverworkRates_ApplyAuthoredDailyTotals_AndRemoveOnResolution()
        {
            // Zero base drift so the assertion isolates the overwork rate.
            var needs = new NeedsSystem(new NeedsProfile
            {
                fatiguePerHour = 0f,
                hungerPerHour = 0f,
                thirstPerHour = 0f,
                warmthLossPerHourInCold = 0f
            });
            var survivor = new SurvivorNeedsState { Id = "survivor_a", Fatigue = 10f, Morale = 50f };
            needs.Register(survivor);

            const float fatiguePerExcessHour = 0.5f;
            const float moralePerExcessHour = -0.25f;
            float excess = 4f; // 12 committed − 8 recommended

            // The host's daily refresh: per-day authored totals as per-hour
            // rates (the shelter-schedule /24 precedent), replace-on-Set.
            needs.SetExternalModifier("survivor_a", "overwork.fatigue", NeedKind.Fatigue,
                excess * fatiguePerExcessHour / 24f, priority: 25);
            needs.SetExternalModifier("survivor_a", "overwork.morale", NeedKind.Morale,
                excess * moralePerExcessHour / 24f, priority: 25);

            needs.Tick(24f); // the campaign's one 24-hour needs step

            Assert.Equal(10f + excess * fatiguePerExcessHour, survivor.Fatigue, 4);
            Assert.Equal(50f + excess * moralePerExcessHour, survivor.Morale, 4);

            // Resolution: the refresh removes the rates when not overworked —
            // replace-on-Set semantics mean no duplicate accumulation.
            needs.RemoveExternalModifier("survivor_a", "overwork.fatigue", NeedKind.Fatigue);
            needs.RemoveExternalModifier("survivor_a", "overwork.morale", NeedKind.Morale);
            needs.Tick(24f);
            Assert.Equal(10f + excess * fatiguePerExcessHour, survivor.Fatigue, 4);
            Assert.Equal(0, needs.ModifierStack.Count);
        }

        // ── 3. Shared worker-productivity contract ────────────────────

        [Fact]
        public void Contract_UnboundOrUnknownSkill_ReturnsNull_LegacyPath()
        {
            var contract = new WorkerProductivityContract();
            Assert.Null(contract.Resolve("survivor_a", "skill_workshop_sense"));

            contract.SkillLevelResolver = (_, skillId) =>
                skillId == "skill_workshop_sense" ? 50f : null;
            Assert.Null(contract.Resolve("survivor_a", "skill_unknown")); // worker has no level
            Assert.Null(contract.Resolve("", "skill_workshop_sense"));
        }

        [Fact]
        public void Contract_SkilledFitWorker_YieldsWithinAuthoredBand()
        {
            var contract = new WorkerProductivityContract
            {
                SkillLevelResolver = (_, _) => 100f,
                FitnessResolver = _ => FitnessLevel.Fit,
                OverworkResolver = _ => false,
            };
            var verdict = contract.Resolve("survivor_a", "skill_workshop_sense");
            Assert.NotNull(verdict);
            Assert.Equal(1050, verdict.YieldModifierPermille); // authored cap
            Assert.Empty(verdict.Notes);

            contract.SkillLevelResolver = (_, _) => 0f;
            verdict = contract.Resolve("survivor_a", "skill_workshop_sense");
            Assert.Equal(750, verdict.YieldModifierPermille); // authored floor
            Assert.Contains(WorkerProductivityContract.NoteSkillLow, verdict.Notes);
        }

        [Fact]
        public void Contract_ImpairedOrOverworked_PenalizesAndStaysBounded()
        {
            var contract = new WorkerProductivityContract
            {
                SkillLevelResolver = (_, _) => 100f,
                FitnessResolver = _ => FitnessLevel.Impaired,
                OverworkResolver = _ => true,
            };
            var verdict = contract.Resolve("survivor_a", "skill_workshop_sense");
            Assert.Equal(1050 - 100 - 150, verdict.YieldModifierPermille);
            Assert.Contains(WorkerProductivityContract.NoteFitnessImpaired, verdict.Notes);
            Assert.Contains(WorkerProductivityContract.NoteOverworked, verdict.Notes);
            Assert.True(verdict.Overworked);

            // Extreme degradation clamps at the authored absolute floor.
            contract.SkillLevelResolver = (_, _) => 0f;
            contract.OverworkRules = new DutyOverworkRules(0f, 0f, 5000); // absurd penalty
            verdict = contract.Resolve("survivor_a", "skill_workshop_sense");
            Assert.Equal(500, verdict.YieldModifierPermille); // absolute floor
        }

        // ── 4. Kitchen quality stamp (skill-to-yield, quality leg) ────

        private sealed class FixedRollRng : ISeededRng
        {
            private readonly double _roll;
            public FixedRollRng(double roll) => _roll = roll;
            public int Seed => 1;
            public int Next(int minInclusive, int maxExclusive) => minInclusive;
            public float NextFloat() => (float)_roll;
            public double NextDouble() => _roll;
        }

        [Fact]
        public void Kitchen_UnstampedBatch_KeepsExactLegacyBehavior()
        {
            var inv = new Inventory.Inventory();
            var needs = new NeedsSystem();
            var survivor = new SurvivorNeedsState { Id = "survivor_1", Hunger = 60f, Morale = 50f, Health = 90f };
            needs.Register(survivor);
            var kitchen = new KitchenNutritionSystem(new FixedRollRng(0.85), inv, needs);
            inv.AddById("meat", 5);
            kitchen.StartPrepJob("stew", "cook_1", new Dictionary<string, int> { { "meat", 2 } });
            kitchen.TickDay(1);

            // Unstamped (no productivity resolver): legacy 3 portions, legacy 0.9 safe chance.
            Assert.Equal(3, kitchen.State.pantry[0].portionCount);
            Assert.Equal(0, kitchen.State.pantry[0].qualityPermille);
            var serve = kitchen.ServeMeal("survivor_1", "stew");
            Assert.Equal(ActionResult.StatusKind.Success, serve.Status);
            Assert.Equal(50f + 5f, survivor.Morale, 4); // 0.85 < 0.9 → safe, +5
        }

        [Fact]
        public void Kitchen_StampedDegradedBatch_ScalesQualityAndYield()
        {
            var inv = new Inventory.Inventory();
            var needs = new NeedsSystem();
            var survivor = new SurvivorNeedsState { Id = "survivor_1", Hunger = 60f, Morale = 50f, Health = 90f };
            needs.Register(survivor);
            var kitchen = new KitchenNutritionSystem(new FixedRollRng(0.85), inv, needs);
            kitchen.CookProductivityResolver = _ => new WorkerProductivityVerdict(
                "cook_1", "skill_iron_chef", 0f, FitnessLevel.Impaired, true,
                yieldModifierPermille: 500, new[] { WorkerProductivityContract.NoteOverworked });

            inv.AddById("meat", 5);
            kitchen.StartPrepJob("stew", "cook_1", new Dictionary<string, int> { { "meat", 2 } });
            kitchen.TickDay(1);

            // Waste leg: degraded cook's 500‰ modifier scales 3 portions → 2
            // (bounded 1..recipe portions; ingredient cost unchanged).
            Assert.Equal(2, kitchen.State.pantry[0].portionCount);
            Assert.Equal(500, kitchen.State.pantry[0].qualityPermille);

            // Quality leg: safe chance 0.9 × 0.5 = 0.45 → clamped to 0.7;
            // a 0.85 roll is now UNSAFE (the legacy 0.9 path called it safe).
            var serve = kitchen.ServeMeal("survivor_1", "stew");
            Assert.Equal(ActionResult.StatusKind.Success, serve.Status);
            Assert.Equal(50f - 5f, survivor.Morale, 4); // unsafe meal
            Assert.Equal(85f, survivor.Health, 4);
        }

        // ── 5. Workshop craft-time slot (composed, never clobbering) ──

        [Fact]
        public void Crafting_ProductivitySlotComposesWithPenaltySlot()
        {
            var inventory = new Inventory.Inventory();
            var system = new CraftingSystem(inventory);
            system.SetDayProvider(() => 1);
            var recipe = new Recipe
            {
                id = "recipe_test",
                result = new ItemDefinition { id = "item_test", type = ItemType.Material, stackMax = 99, weight = 1f },
                resultAmount = 1,
                craftingTimeHours = 2f,
                ingredients = new List<Ingredient>
                {
                    new Ingredient { item = new ItemDefinition { id = "scrap_mechanical", type = ItemType.Material, stackMax = 99, weight = 1f }, amount = 1 }
                }
            };
            inventory.Add(recipe.ingredients[0].item, 10);

            // Unbound slot: exact legacy duration.
            var preview = system.PreviewCraft(recipe, "survivor_a", 1L);
            Assert.Equal(2f, preview.EstimatedDurationHours);

            // Productivity slot alone: 1.3× speed → 2h / 1.3 ≈ 1.54h.
            system.SetCrafterProductivityTimeMultiplier(_ => 1.3f);
            preview = system.PreviewCraft(recipe, "survivor_a", 1L);
            Assert.True(MathF.Abs((preview.EstimatedDurationHours ?? 0f) - 2f / 1.3f) < 0.01f,
                $"expected ~{2f / 1.3f:0.###}h, got {preview.EstimatedDurationHours}");

            // Composed with the Phase0 penalty slot: both apply — never clobber.
            system.SetCrafterCraftTimeMultiplier(_ => 2f); // Phase0-style penalty
            preview = system.PreviewCraft(recipe, "survivor_a", 1L);
            Assert.True(MathF.Abs((preview.EstimatedDurationHours ?? 0f) - 4f / 1.3f) < 0.01f,
                $"expected ~{4f / 1.3f:0.###}h, got {preview.EstimatedDurationHours}");
        }
    }
}
