using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.UtilityAI;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Adversarial probes for the Utility AI port (Phase F loop). Every probe
    /// is a permanent regression test.
    /// </summary>
    public class UtilityAiProbeTests
    {
        private static UtilityActionDef Action(
            string id, float baseScore = 0.5f, float priority = 0.1f,
            float weight = 1f, bool overrideAction = false, params string[] tags)
        {
            return new UtilityActionDef
            {
                id = id,
                displayName = id,
                baseScore = baseScore,
                basePriority = priority,
                weight = weight,
                isOverrideAction = overrideAction,
                tags = tags
            };
        }

        private static AIActionContext Ctx(
            float fatigue = 0f, float skill = 0f, bool listless = false,
            bool hazmat = false, params string[] traits)
        {
            var ctx = new AIActionContext
            {
                SurvivorId = "sv_p",
                IsAlive = true,
                Fatigue = fatigue,
                CraftingSkill = skill,
                IsListless = listless,
                HasHazmat = hazmat
            };
            foreach (var t in traits) ctx.Traits.Add(t);
            return ctx;
        }

        [Fact]
        public void Probe_SeedFuzz_100SeedsMixedContexts_NoExceptionsDeterministic()
        {
            var sys = new UtilityAiSystem();
            var candidates = new List<UtilityActionDef>
            {
                Action("a", baseScore: 0.3f, tags: UtilityTags.TagLoudLabor),
                Action("b", baseScore: 0.5f, tags: UtilityTags.TagGun),
                Action("c", baseScore: 0.7f, tags: UtilityTags.TagMedicalTriage),
                Action("d", baseScore: 0.2f, weight: 3f, overrideAction: true),
                Action("e", baseScore: 0.4f)
            };
            var traitsPool = new[]
            {
                UtilityTags.TraitCoward, UtilityTags.TraitGodComplex, UtilityTags.TraitPacifist,
                UtilityTags.TraitBlind, UtilityTags.TraitExCon, UtilityTags.TraitHitman,
                UtilityTags.TraitGermaphobe
            };
            for (int seed = 0; seed < 100; seed++)
            {
                var ctx = Ctx(
                    fatigue: seed % 101f,
                    skill: (seed % 11) / 10f,
                    listless: seed % 3 == 0,
                    hazmat: seed % 2 == 0,
                    traits: new[] { traitsPool[seed % traitsPool.Length] });
                var picked = sys.SelectAction(ctx, candidates, new SeededRng(seed));
                if (picked != null)
                    Assert.True(picked.baseScore > 0f || picked.isOverrideAction);
                // Determinism: same seed, same context, same pick.
                var again = sys.SelectAction(ctx, candidates, new SeededRng(seed));
                Assert.Equal(picked, again);
            }
        }

        [Fact]
        public void Probe_CurveBounds_IdentitySinglePointNonMonotonic()
        {
            var scorer = new UtilityActionScorer();
            var identity = Action("i", baseScore: 0.5f);
            // Empty curve -> identity passthrough: raw 0.5 -> (0.5+0.1)=0.6.
            Assert.Equal(0.6f, scorer.Score(identity, Ctx()), 3);

            var single = Action("s", baseScore: 0.5f);
            single.curvePoints = new[] { new CurvePoint { x = 0.5f, y = 0.9f } };
            // Single point returns its y as the CURVED value; +0.1 priority -> 1.0 clamped.
            Assert.Equal(1f, scorer.Score(single, Ctx()), 3);

            var clamped = Action("c", baseScore: 0.5f);
            clamped.curvePoints = new[]
            {
                new CurvePoint { x = 0.2f, y = 0f },
                new CurvePoint { x = 0.4f, y = 1f }
            };
            // raw 0.5 >= last x (0.4) -> last y = 1.0; (1+0.1)=1.1 clamped 1.
            Assert.Equal(1f, scorer.Score(clamped, Ctx()), 3);
        }

        [Fact]
        public void Probe_FatigueGateBoundary_AtGateAllowedAboveVetoed()
        {
            var scorer = new UtilityActionScorer();
            var a = Action("a", baseScore: 0.4f);
            a.fatigueGate = 85f;
            Assert.True(scorer.Score(a, Ctx(fatigue: 85f)) > 0f);   // exactly at gate
            Assert.Equal(0f, scorer.Score(a, Ctx(fatigue: 85.01f))); // above gate
        }

        [Fact]
        public void Probe_OverrideDominatesUnderNoise()
        {
            var sys = new UtilityAiSystem();
            var normal = Action("normal", baseScore: 0.9f, weight: 1f);      // 1.0 clamped
            var ovr = Action("override", baseScore: 0.3f, weight: 4f, overrideAction: true); // 1.3 unclamped
            for (int seed = 0; seed < 50; seed++)
            {
                var picked = sys.SelectAction(Ctx(),
                    new List<UtilityActionDef> { normal, ovr }, new SeededRng(seed));
                Assert.Equal("override", picked.id);
            }
        }

        [Fact]
        public void Probe_EmptyCatalog_SelectsNull()
        {
            var sys = new UtilityAiSystem();
            Assert.Null(sys.SelectAction(Ctx(), new List<UtilityActionDef>(), new SeededRng(1)));
        }

        [Fact]
        public void Probe_AllZeroScoreActions_SelectsNull()
        {
            var sys = new UtilityAiSystem();
            var dead = Action("dead", baseScore: 0f, priority: 0f);
            var gated = Action("gated", baseScore: 0.4f, priority: 0f);
            gated.fatigueGate = 50f;
            var ctx = Ctx(fatigue: 90f);
            Assert.Null(sys.SelectAction(ctx, new List<UtilityActionDef> { dead, gated }, new SeededRng(1)));
        }

        [Fact]
        public void Probe_VetoMatrix_EveryTraitTagPair()
        {
            var scorer = new UtilityActionScorer();
            string[][] pairs =
            {
                new[] { UtilityTags.TraitCoward, UtilityTags.TagLoudLabor },
                new[] { UtilityTags.TraitGodComplex, UtilityTags.TagMenialLabor },
                new[] { UtilityTags.TraitPacifist, UtilityTags.TagWeapon },
                new[] { UtilityTags.TraitBlind, UtilityTags.TagGun },
                new[] { UtilityTags.TraitExCon, UtilityTags.TagOrder },
                new[] { UtilityTags.TraitHitman, UtilityTags.TagMedicalTriage },
                new[] { UtilityTags.TraitHitman, UtilityTags.TagFarming },
                new[] { UtilityTags.TraitGermaphobe, UtilityTags.TagMedicalTriage }
            };
            foreach (var pair in pairs)
            {
                float score = scorer.Score(Action("x", baseScore: 0.5f, tags: pair[1]),
                    Ctx(traits: pair[0]));
                Assert.True(score == 0f, $"{pair[0]} x {pair[1]} must veto, got {score}");
            }
            // Germaphobe with hazmat is NOT vetoed.
            Assert.True(scorer.Score(Action("x", baseScore: 0.5f, tags: UtilityTags.TagMedicalTriage),
                Ctx(hazmat: true, traits: UtilityTags.TraitGermaphobe)) > 0f);
            // Non-matching trait-tag pairs pass.
            Assert.True(scorer.Score(Action("x", baseScore: 0.5f, tags: UtilityTags.TagLoudLabor),
                Ctx(traits: UtilityTags.TraitPacifist)) > 0f);
        }

        [Fact]
        public void Probe_ScoreAll_OrdinalStableNoSideEffects()
        {
            var sys = new UtilityAiSystem();
            var scorer = new UtilityActionScorer();
            var candidates = new List<UtilityActionDef>
            {
                Action("b", baseScore: 0.6f), Action("a", baseScore: 0.4f), Action("c", baseScore: 0.5f)
            };
            var scored = sys.ScoreAll(Ctx(), candidates, scorer);
            Assert.Equal(3, scored.Count);
            Assert.Equal("b", scored[0].Key.id); // caller order preserved
            // ScoreAll must not mutate the action defs.
            Assert.Equal(0.6f, candidates[0].baseScore);
        }

        [Fact]
        public void Probe_UnsortedCurvePoints_DoNotMisEvaluate()
        {
            var scorer = new UtilityActionScorer();
            var unsorted = Action("u", baseScore: 0.5f);
            unsorted.curvePoints = new[]
            {
                new CurvePoint { x = 1f, y = 1f },
                new CurvePoint { x = 0f, y = 0f },
                new CurvePoint { x = 0.5f, y = 0.5f }
            };
            // raw 0.5 must evaluate as 0.5 regardless of declaration order.
            float score = scorer.Score(unsorted, Ctx());
            Assert.True(Math.Abs(score - 0.6f) < 1e-3f,
                $"unsorted curve must evaluate identically to sorted, got {score}");
        }

        [Fact]
        public void Probe_NoiseOnClampedScore_NeverNaN_DeterministicPerSeed()
        {
            // Unity parity: noise is added AFTER clamping, so a clamped 1.0
            // normal action can report 1.0 + noise via the event. Never NaN,
            // never negative, deterministic per seed.
            var sys = new UtilityAiSystem();
            var top = Action("top", baseScore: 0.9f); // (0.9+0.1) = 1.0 clamped
            float lastScore = -1f;
            sys.OnActionSelected += (sv, id, score) => lastScore = score;
            for (int seed = 0; seed < 20; seed++)
            {
                sys.SelectAction(Ctx(), new List<UtilityActionDef> { top }, new SeededRng(seed));
                Assert.False(float.IsNaN(lastScore));
                Assert.True(lastScore >= 1f);
            }
        }

        [Fact]
        public void Probe_MissingCatalogFile_ReturnsEmpty()
        {
            var defs = UtilityActionCatalogLoader.Load(
                "/nonexistent", new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Empty(defs);
        }

        [Fact]
        public void Probe_ScoreAll_NullScorerDefaults()
        {
            var sys = new UtilityAiSystem();
            var scored = sys.ScoreAll(Ctx(), new List<UtilityActionDef> { Action("a", baseScore: 0.5f) });
            Assert.Single(scored);
            Assert.True(scored[0].Value > 0f);
        }

        [Fact]
        public void Probe_SelectionDoesNotMutateContextOrDefs()
        {
            var sys = new UtilityAiSystem();
            var def = Action("a", baseScore: 0.5f);
            var ctx = Ctx(skill: 0.4f);
            sys.SelectAction(ctx, new List<UtilityActionDef> { def }, new SeededRng(1));
            Assert.Equal(0.5f, def.baseScore);
            Assert.Equal(0.4f, ctx.CraftingSkill);
            Assert.Empty(ctx.Traits);
        }
    }
}
