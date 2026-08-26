using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.UtilityAI;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class UtilityAiTests
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
            bool alive = true, float fatigue = 0f, float skill = 0f,
            bool listless = false, bool hazmat = false, params string[] traits)
        {
            var ctx = new AIActionContext
            {
                SurvivorId = "sv_1",
                IsAlive = alive,
                Fatigue = fatigue,
                CraftingSkill = skill,
                IsListless = listless,
                HasHazmat = hazmat
            };
            foreach (var t in traits) ctx.Traits.Add(t);
            return ctx;
        }

        [Fact]
        public void Scorer_DeadSurvivorScoresZero()
        {
            var scorer = new UtilityActionScorer();
            Assert.Equal(0f, scorer.Score(Action("a"), Ctx(alive: false)));
        }

        [Fact]
        public void Scorer_FatigueGateZeroesRaw()
        {
            var scorer = new UtilityActionScorer();
            var action = Action("a", baseScore: 0.4f);
            action.fatigueGate = 85f;
            Assert.Equal(0f, scorer.Score(action, Ctx(fatigue: 90f)));
            Assert.True(scorer.Score(action, Ctx(fatigue: 50f)) > 0f);
        }

        [Fact]
        public void Scorer_SkillBonusApplies()
        {
            var scorer = new UtilityActionScorer();
            var action = Action("a", baseScore: 0.4f);
            action.skillBonusFactor = 0.25f;
            float skilled = scorer.Score(action, Ctx(skill: 0.8f));
            float unskilled = scorer.Score(action, Ctx(skill: 0f));
            Assert.True(skilled > unskilled);
            // raw 0.4 + 0.2 = 0.6, clamped 0..1.
            Assert.True(skilled <= 1f && skilled > 0.5f);
        }

        [Fact]
        public void Scorer_CurveTransformsRaw()
        {
            var scorer = new UtilityActionScorer();
            var action = Action("a", baseScore: 0.5f);
            action.curvePoints = new[]
            {
                new CurvePoint { x = 0f, y = 0f },
                new CurvePoint { x = 0.5f, y = 0.1f },
                new CurvePoint { x = 1f, y = 1f }
            };
            // raw 0.5 -> curve 0.1; (0.1 + 0.1) * 1 = 0.2.
            Assert.Equal(0.2f, scorer.Score(action, Ctx()), 3);
        }

        [Fact]
        public void Scorer_ListlessPenaltyFloorsAtZero()
        {
            var scorer = new UtilityActionScorer();
            var tiny = Action("a", baseScore: 0.01f); // raw 0.01 -> ~0.11 -> -0.08 -> 0.03
            Assert.True(scorer.Score(tiny, Ctx(listless: true)) >= 0f);
            var zero = Action("b", baseScore: 0f);
            Assert.Equal(0f, scorer.Score(zero, Ctx(listless: true)));
        }

        [Fact]
        public void Scorer_OverrideActionsPassThroughUnclamped()
        {
            var scorer = new UtilityActionScorer();
            var big = Action("override", baseScore: 0.9f, weight: 2f, overrideAction: true);
            // (0.9 + 0.1) * 2 = 2.0, unclamped.
            Assert.Equal(2f, scorer.Score(big, Ctx()), 3);
            var normal = Action("normal", baseScore: 0.9f, weight: 2f);
            Assert.Equal(1f, scorer.Score(normal, Ctx())); // clamped
        }

        [Fact]
        public void Scorer_VetoMatrix_CowardRefusesLoudLabor()
        {
            var scorer = new UtilityActionScorer();
            var loud = Action("weigh", baseScore: 0.4f, tags: UtilityTags.TagLoudLabor);
            Assert.Equal(0f, scorer.Score(loud, Ctx(traits: UtilityTags.TraitCoward)));
            Assert.True(scorer.Score(loud, Ctx()) > 0f);
        }

        [Fact]
        public void Scorer_VetoMatrix_PacifistRefusesWeapons_BlindRefusesGuns()
        {
            var scorer = new UtilityActionScorer();
            Assert.Equal(0f, scorer.Score(Action("w", tags: UtilityTags.TagWeapon),
                Ctx(traits: UtilityTags.TraitPacifist)));
            Assert.Equal(0f, scorer.Score(Action("g", tags: UtilityTags.TagGun),
                Ctx(traits: UtilityTags.TraitBlind)));
            Assert.Equal(0f, scorer.Score(Action("o", tags: UtilityTags.TagOrder),
                Ctx(traits: UtilityTags.TraitExCon)));
            Assert.Equal(0f, scorer.Score(Action("m", tags: UtilityTags.TagMedicalTriage),
                Ctx(traits: UtilityTags.TraitHitman)));
            Assert.Equal(0f, scorer.Score(Action("f", tags: UtilityTags.TagFarming),
                Ctx(traits: UtilityTags.TraitHitman)));
            Assert.Equal(0f, scorer.Score(Action("m", tags: UtilityTags.TagMedicalTriage),
                Ctx(traits: UtilityTags.TraitGermaphobe)));
            Assert.True(scorer.Score(Action("m", tags: UtilityTags.TagMedicalTriage),
                Ctx(traits: UtilityTags.TraitGermaphobe, hazmat: true)) > 0f);
        }

        [Fact]
        public void Selection_PicksHighestScoringCandidate()
        {
            var sys = new UtilityAiSystem();
            var low = Action("low", baseScore: 0.2f);
            var high = Action("high", baseScore: 0.8f);
            var picked = sys.SelectAction(Ctx(), new List<UtilityActionDef> { low, high }, new SeededRng(1));
            Assert.Equal("high", picked.id);
        }

        [Fact]
        public void Selection_OverrideWinsOverAnyNormalAction()
        {
            var sys = new UtilityAiSystem();
            var normal = Action("normal", baseScore: 1f);
            var overrideAction = Action("override", baseScore: 0.3f, weight: 5f, overrideAction: true);
            var picked = sys.SelectAction(Ctx(),
                new List<UtilityActionDef> { normal, overrideAction }, new SeededRng(1));
            Assert.Equal("override", picked.id);
        }

        [Fact]
        public void Selection_EmptyOrNullCandidatesReturnsNull()
        {
            var sys = new UtilityAiSystem();
            Assert.Null(sys.SelectAction(Ctx(), new List<UtilityActionDef>(), new SeededRng(1)));
            Assert.Null(sys.SelectAction(null, new List<UtilityActionDef> { Action("a") }, new SeededRng(1)));
            Assert.Null(sys.SelectAction(Ctx(), null, new SeededRng(1)));
        }

        [Fact]
        public void Selection_AllVetoedReturnsNull()
        {
            var sys = new UtilityAiSystem();
            var loud = Action("weigh", baseScore: 0.4f, tags: UtilityTags.TagLoudLabor);
            var ctx = Ctx(traits: UtilityTags.TraitCoward);
            Assert.Null(sys.SelectAction(ctx, new List<UtilityActionDef> { loud }, new SeededRng(1)));
        }

        [Fact]
        public void Selection_WithoutNoise_TiesFirstWins()
        {
            // No rng = no noise: strict first-wins on ties.
            var sys = new UtilityAiSystem();
            var a = Action("a", baseScore: 0.5f);
            var b = Action("b", baseScore: 0.5f);
            for (int seed = 0; seed < 20; seed++)
            {
                var picked = sys.SelectAction(Ctx(), new List<UtilityActionDef> { a, b }, null);
                Assert.Equal("a", picked.id);
            }
        }

        [Fact]
        public void Selection_WithNoise_TiePickDeterministicPerSeed()
        {
            // Seeded noise may flip a tie, but the SAME seed must pick the
            // SAME candidate every time (cross-process determinism).
            var sys = new UtilityAiSystem();
            var a = Action("a", baseScore: 0.5f);
            var b = Action("b", baseScore: 0.5f);
            var candidates = new List<UtilityActionDef> { a, b };
            for (int seed = 0; seed < 10; seed++)
            {
                string p1 = sys.SelectAction(Ctx(), candidates, new SeededRng(seed)).id;
                string p2 = sys.SelectAction(Ctx(), candidates, new SeededRng(seed)).id;
                Assert.Equal(p1, p2);
                Assert.Contains(p1, new[] { "a", "b" });
            }
        }

        [Fact]
        public void Selection_DeterministicSameSeedSamePick()
        {
            var sys = new UtilityAiSystem();
            var candidates = new List<UtilityActionDef>
            {
                Action("x", baseScore: 0.3f), Action("y", baseScore: 0.45f), Action("z", baseScore: 0.6f)
            };
            string pickA = sys.SelectAction(Ctx(), candidates, new SeededRng(42)).id;
            string pickB = sys.SelectAction(Ctx(), candidates, new SeededRng(42)).id;
            Assert.Equal(pickA, pickB);
        }

        [Fact]
        public void Selection_FiresEventOnPick()
        {
            var sys = new UtilityAiSystem();
            string pickedId = null;
            sys.OnActionSelected += (sv, id, score) => pickedId = id;
            sys.SelectAction(Ctx(), new List<UtilityActionDef> { Action("a", baseScore: 0.5f) }, new SeededRng(1));
            Assert.Equal("a", pickedId);
        }

        [Fact]
        public void Scorer_NullActionOrContextScoresZero()
        {
            var scorer = new UtilityActionScorer();
            Assert.Equal(0f, scorer.Score(null, Ctx()));
            Assert.Equal(0f, scorer.Score(Action("a"), null));
        }

        // ── Data catalog ───────────────────────────────────────────────

        private static string FindDataDir()
        {
            string search = System.IO.Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = System.IO.Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (System.IO.Directory.Exists(candidate)) return candidate;
                string parent = System.IO.Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return string.Empty;
        }

        [Fact]
        public void Catalog_LoadsFourCrossingActionsWithBoundFields()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var defs = UtilityActionCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(6, defs.Count);
            Assert.Contains(defs, d => d.id == "action_weigh_goods");
            Assert.Contains(defs, d => d.id == "action_read_contract");
            Assert.Contains(defs, d => d.id == "action_canvas_support");
            Assert.Contains(defs, d => d.id == "action_run_vouch");
            Assert.Contains(defs, d => d.id == "action_audit_inventory");
            Assert.Contains(defs, d => d.id == "action_file_report");
            foreach (var d in defs)
            {
                Assert.False(string.IsNullOrEmpty(d.displayName));
                Assert.True(d.baseScore > 0f);
                Assert.True(d.weight > 0f);
                Assert.NotNull(d.tags);
                Assert.NotNull(d.curvePoints);
            }
        }

        [Fact]
        public void Catalog_WeighGoodsUnityParity()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var defs = UtilityActionCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var weigh = defs.Find(d => d.id == "action_weigh_goods");
            Assert.NotNull(weigh);
            // Unity parity: base 0.40, + skill * 0.25, fatigue gate 85.
            Assert.Equal(0.40f, weigh.baseScore);
            Assert.Equal(0.25f, weigh.skillBonusFactor);
            Assert.Equal(85f, weigh.fatigueGate);
            var scorer = new UtilityActionScorer();
            Assert.Equal(0f, scorer.Score(weigh, Ctx(fatigue: 90f)));
            Assert.True(scorer.Score(weigh, Ctx(skill: 0.8f)) >
                        scorer.Score(weigh, Ctx(skill: 0f)));
        }
    }
}
