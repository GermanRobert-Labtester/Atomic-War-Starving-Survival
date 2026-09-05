using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.UtilityAI;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class UtilityAiExpandedCatalogTests
    {
        private static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return string.Empty;
        }

        private static List<UtilityActionDef> LoadCatalog()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Could not find StreamingAssets/Data directory");
            var actions = UtilityActionCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(actions);
            return actions;
        }

        private static AIActionContext Ctx(
            bool alive = true, float fatigue = 0f, float skill = 0f,
            bool listless = false, bool hazmat = false, params string[] traits)
        {
            var ctx = new AIActionContext
            {
                SurvivorId = "sv_test",
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
        public void Catalog_LoadsExact20ActionsWithValidSchema()
        {
            var actions = LoadCatalog();
            Assert.Equal(20, actions.Count);

            foreach (var a in actions)
            {
                Assert.False(string.IsNullOrWhiteSpace(a.id));
                Assert.StartsWith("action_", a.id);
                Assert.False(string.IsNullOrWhiteSpace(a.displayName));
                Assert.True(a.baseScore > 0f);
                Assert.True(a.weight > 0f);
                Assert.True(a.basePriority >= 0f);
                Assert.NotNull(a.tags);
                Assert.NotNull(a.curvePoints);
                Assert.True(a.curvePoints.Length >= 2);
            }
        }

        [Fact]
        public void Catalog_PreservesOriginal6ActionsByteAndFieldParity()
        {
            var actions = LoadCatalog();
            Assert.True(actions.Count >= 6);

            var a0 = actions[0];
            Assert.Equal("action_weigh_goods", a0.id);
            Assert.Equal("Weigh Goods", a0.displayName);
            Assert.Equal(0.40f, a0.baseScore);
            Assert.Equal(85.0f, a0.fatigueGate);
            Assert.Equal(0.25f, a0.skillBonusFactor);
            Assert.Contains(UtilityTags.TagLoudLabor, a0.tags);

            var a1 = actions[1];
            Assert.Equal("action_read_contract", a1.id);
            Assert.Equal("Read Contract", a1.displayName);
            Assert.Equal(0.35f, a1.baseScore);
            Assert.Equal(90.0f, a1.fatigueGate);
            Assert.Equal(0.20f, a1.skillBonusFactor);

            var a2 = actions[2];
            Assert.Equal("action_canvas_support", a2.id);
            Assert.Equal("Canvas Support", a2.displayName);
            Assert.Equal(0.45f, a2.baseScore);
            Assert.Equal(80.0f, a2.fatigueGate);
            Assert.Equal(0.15f, a2.skillBonusFactor);
            Assert.Contains(UtilityTags.TagMenialLabor, a2.tags);

            var a3 = actions[3];
            Assert.Equal("action_run_vouch", a3.id);
            Assert.Equal("Run Vouch", a3.displayName);
            Assert.Equal(0.30f, a3.baseScore);
            Assert.Equal(88.0f, a3.fatigueGate);
            Assert.Equal(0.10f, a3.skillBonusFactor);

            var a4 = actions[4];
            Assert.Equal("action_audit_inventory", a4.id);
            Assert.Equal("Audit Inventory", a4.displayName);
            Assert.Equal(0.35f, a4.baseScore);
            Assert.Equal(80.0f, a4.fatigueGate);

            var a5 = actions[5];
            Assert.Equal("action_file_report", a5.id);
            Assert.Equal("File Report", a5.displayName);
            Assert.Equal(0.35f, a5.baseScore);
            Assert.Equal(80.0f, a5.fatigueGate);
        }

        [Fact]
        public void Catalog_All14NewActionsPresentAndCategorized()
        {
            var actions = LoadCatalog();
            var expectedNewIds = new[]
            {
                "action_repair_equipment", // Maintenance 1
                "action_inspect_housing",  // Maintenance 2
                "action_treat_wounded",    // Medical 1
                "action_seek_treatment",   // Medical 2
                "action_cook_food",        // Food 1
                "action_preserve_food",    // Food 2
                "action_purify_water",     // Water 1
                "action_socialize",        // Social 1
                "action_resolve_conflict", // Social 2
                "action_train_skill",      // Training 1
                "action_teach_skill",      // Training 2
                "action_stand_watch",      // Security 1
                "action_conduct_research", // Research 1
                "action_rest"              // Rest 1
            };

            foreach (var id in expectedNewIds)
            {
                Assert.Contains(actions, a => a.id == id);
            }
        }

        [Fact]
        public void Catalog_ContainsZeroDuplicateIds()
        {
            var actions = LoadCatalog();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var a in actions)
            {
                Assert.True(seen.Add(a.id), $"Duplicate action ID found: {a.id}");
            }
            Assert.Equal(20, seen.Count);
        }

        [Fact]
        public void Scorer_CowardRefusesLoudLaborActions()
        {
            var actions = LoadCatalog();
            var repair = actions.Find(a => a.id == "action_repair_equipment")!;
            var purify = actions.Find(a => a.id == "action_purify_water")!;

            var scorer = new UtilityActionScorer();
            var normalCtx = Ctx(fatigue: 20f, skill: 0.5f);
            var cowardCtx = Ctx(fatigue: 20f, skill: 0.5f, traits: new[] { UtilityTags.TraitCoward });

            Assert.True(scorer.Score(repair, normalCtx) > 0f);
            Assert.Equal(0f, scorer.Score(repair, cowardCtx));

            Assert.True(scorer.Score(purify, normalCtx) > 0f);
            Assert.Equal(0f, scorer.Score(purify, cowardCtx));
        }

        [Fact]
        public void Scorer_GodComplexRefusesMenialLaborActions()
        {
            var actions = LoadCatalog();
            var preserve = actions.Find(a => a.id == "action_preserve_food")!;
            var canvas = actions.Find(a => a.id == "action_canvas_support")!;

            var scorer = new UtilityActionScorer();
            var normalCtx = Ctx(fatigue: 20f);
            var godComplexCtx = Ctx(fatigue: 20f, traits: new[] { UtilityTags.TraitGodComplex });

            Assert.True(scorer.Score(preserve, normalCtx) > 0f);
            Assert.Equal(0f, scorer.Score(preserve, godComplexCtx));

            Assert.True(scorer.Score(canvas, normalCtx) > 0f);
            Assert.Equal(0f, scorer.Score(canvas, godComplexCtx));
        }

        [Fact]
        public void Scorer_PacifistRefusesSecurityWatchWithWeaponTag()
        {
            var actions = LoadCatalog();
            var watch = actions.Find(a => a.id == "action_stand_watch")!;

            var scorer = new UtilityActionScorer();
            var normalCtx = Ctx(fatigue: 20f);
            var pacifistCtx = Ctx(fatigue: 20f, traits: new[] { UtilityTags.TraitPacifist });

            Assert.True(scorer.Score(watch, normalCtx) > 0f);
            Assert.Equal(0f, scorer.Score(watch, pacifistCtx));
        }

        [Fact]
        public void Scorer_HitmanRefusesMedicalTriageActions()
        {
            var actions = LoadCatalog();
            var treat = actions.Find(a => a.id == "action_treat_wounded")!;

            var scorer = new UtilityActionScorer();
            var normalCtx = Ctx(fatigue: 20f);
            var hitmanCtx = Ctx(fatigue: 20f, traits: new[] { UtilityTags.TraitHitman });

            Assert.True(scorer.Score(treat, normalCtx) > 0f);
            Assert.Equal(0f, scorer.Score(treat, hitmanCtx));
        }

        [Fact]
        public void Scorer_GermaphobeGatedOnHazmatForMedicalTriage()
        {
            var actions = LoadCatalog();
            var treat = actions.Find(a => a.id == "action_treat_wounded")!;

            var scorer = new UtilityActionScorer();
            var germaphobeNoHazmat = Ctx(fatigue: 20f, hazmat: false, traits: new[] { UtilityTags.TraitGermaphobe });
            var germaphobeWithHazmat = Ctx(fatigue: 20f, hazmat: true, traits: new[] { UtilityTags.TraitGermaphobe });

            Assert.Equal(0f, scorer.Score(treat, germaphobeNoHazmat));
            Assert.True(scorer.Score(treat, germaphobeWithHazmat) > 0f);
        }

        [Fact]
        public void Scorer_ExConRefusesOrderActions()
        {
            var actions = LoadCatalog();
            var conflict = actions.Find(a => a.id == "action_resolve_conflict")!;

            var scorer = new UtilityActionScorer();
            var normalCtx = Ctx(fatigue: 20f);
            var exConCtx = Ctx(fatigue: 20f, traits: new[] { UtilityTags.TraitExCon });

            Assert.True(scorer.Score(conflict, normalCtx) > 0f);
            Assert.Equal(0f, scorer.Score(conflict, exConCtx));
        }

        [Fact]
        public void Scorer_FatigueGatingDisablesWorkWhileAllowingRest()
        {
            var actions = LoadCatalog();
            var repair = actions.Find(a => a.id == "action_repair_equipment")!;
            var cook = actions.Find(a => a.id == "action_cook_food")!;
            var research = actions.Find(a => a.id == "action_conduct_research")!;
            var rest = actions.Find(a => a.id == "action_rest")!;

            var scorer = new UtilityActionScorer();
            // High fatigue (92)
            var exhaustedCtx = Ctx(fatigue: 92f, skill: 0.8f);

            Assert.Equal(0f, scorer.Score(repair, exhaustedCtx));
            Assert.Equal(0f, scorer.Score(cook, exhaustedCtx));
            Assert.Equal(0f, scorer.Score(research, exhaustedCtx));

            // Rest has fatigueGate == 0 and scores positively under high fatigue
            float restScore = scorer.Score(rest, exhaustedCtx);
            Assert.True(restScore > 0f, "Rest action must remain valid when exhausted");
        }

        [Fact]
        public void Scorer_CraftingSkillScalesRepairEquipment()
        {
            var actions = LoadCatalog();
            var repair = actions.Find(a => a.id == "action_repair_equipment")!;

            var scorer = new UtilityActionScorer();
            float skilled = scorer.Score(repair, Ctx(fatigue: 20f, skill: 1.0f));
            float unskilled = scorer.Score(repair, Ctx(fatigue: 20f, skill: 0.0f));

            Assert.True(skilled > unskilled, "Skilled mechanic must score repair higher than unskilled survivor");
        }

        [Fact]
        public void Scorer_DeadSurvivorScoresZeroAcrossAll20Actions()
        {
            var actions = LoadCatalog();
            var scorer = new UtilityActionScorer();
            var deadCtx = Ctx(alive: false, fatigue: 0f, skill: 1f);

            foreach (var a in actions)
            {
                Assert.Equal(0f, scorer.Score(a, deadCtx));
            }
        }

        [Fact]
        public void Selection_PicksDeterministicallyWithSameSeed()
        {
            var actions = LoadCatalog();
            var sys = new UtilityAiSystem();
            var ctx = Ctx(fatigue: 40f, skill: 0.6f);

            for (int seed = 1; seed <= 20; seed++)
            {
                var pick1 = sys.SelectAction(ctx, actions, new SeededRng(seed * 77));
                var pick2 = sys.SelectAction(ctx, actions, new SeededRng(seed * 77));

                Assert.NotNull(pick1);
                Assert.NotNull(pick2);
                Assert.Equal(pick1.id, pick2.id);
            }
        }
    }
}
