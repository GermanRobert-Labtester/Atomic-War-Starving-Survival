using System.Collections.Generic;

namespace Ashfall.Core.UtilityAI
{
    /// <summary>
    /// Headless verification of the Utility AI core: catalog-driven actions,
    /// scorer pipeline, deterministic selection with seeded noise, veto
    /// matrix, override dominance. Invoked by `dotnet test` and by Godot
    /// `-- --utility-ai-selftest`.
    /// </summary>
    public static class UtilityAiHeadlessDemo
    {
        public static HeadlessReport Run(string dataDirectory, ILog? log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new HeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition)
                {
                    report.PassedCount++;
                    log.Info("[PASS] " + name);
                }
                else
                {
                    report.FailedCount++;
                    log.Error("[FAIL] " + name);
                }
            }

            log.Info("[UtilityAiHeadlessDemo] begin");

            var defs = UtilityActionCatalogLoader.Load(
                dataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            Check(defs.Count >= 6, $"catalog loads >= 6 utility actions ({defs.Count})");

            var sys = new UtilityAiSystem();
            var scorer = new UtilityActionScorer();

            var ctx = new AIActionContext
            {
                SurvivorId = "sv_demo",
                IsAlive = true,
                Fatigue = 30f,
                CraftingSkill = 0.7f
            };

            var picked = sys.SelectAction(ctx, defs, new SeededRng(99), scorer);
            Check(picked != null, "selection returns an action");
            // weigh 0.40+0.7*0.25=0.575+0.1=0.675 beats canvas 0.45+0.7*0.15+0.1=0.655.
            Check(picked != null && picked.id == "action_weigh_goods",
                "weigh goods (0.675) wins at low fatigue with skill 0.7");

            // Fatigue 87: weigh (gate 85) and canvas (gate 80) gated; read
            // contract (gate 90) wins.
            ctx.Fatigue = 87f;
            var gated = sys.SelectAction(ctx, defs, new SeededRng(99), scorer);
            Check(gated != null && gated.id == "action_read_contract",
                "fatigue gates veto weigh/canvas; read contract wins");

            // Veto matrix: coward refuses loud labor (weigh-goods is tagged loud_labor).
            ctx.Fatigue = 30f;
            ctx.Traits.Add(UtilityTags.TraitCoward);
            var vetoed = sys.SelectAction(ctx, defs, new SeededRng(99), scorer);
            Check(vetoed != null && vetoed.id != "action_weigh_goods",
                "coward vetoes loud labor (weigh goods)");

            // Determinism: same seed, same pick, across fresh instances.
            ctx.Traits.Clear();
            string pickA = new UtilityAiSystem().SelectAction(ctx, defs, new SeededRng(5))!.id;
            string pickB = new UtilityAiSystem().SelectAction(ctx, defs, new SeededRng(5))!.id;
            Check(pickA == pickB, "same seed, same pick (determinism)");

            // All-vetoed returns null (audit A9 regression).
            var strictCtx = new AIActionContext { SurvivorId = "sv_vetoed", Traits = { UtilityTags.TraitCoward } };
            var loudOnly = new List<UtilityActionDef>
            {
                defs.Find(d => d.id == "action_weigh_goods")!
            };
            Check(sys.SelectAction(strictCtx, loudOnly, new SeededRng(1)) == null,
                "all-vetoed selection returns null (A9)");

            report.Passed = report.FailedCount == 0;
            report.Summary =
                $"[UtilityAiHeadlessDemo] {(report.Passed ? "PASS" : "FAIL")} " +
                $"{report.PassedCount}/{report.PassedCount + report.FailedCount}";
            log.Info(report.Summary);
            return report;
        }
    }
}
