using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// Headless verification of the narrative encounter core port.
    /// Invoked by `dotnet test` and by Godot `-- --narrative-selftest`.
    /// </summary>
    public static class NarrativeHeadlessDemo
    {
        public static HeadlessReport Run(ILog? log = null)
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

            log.Info("[NarrativeHeadlessDemo] begin");

            var sys = new NarrativeEncounterSystem();
            sys.RegisterEncounter(new EncounterDefinition
            {
                id = "enc_dead_letter_office", title = "Dead Letter Office", category = "Discovery",
                baseWeight = 2f, minDangerLevel = 0f,
                choices = new List<EncounterChoiceDefinition>
                {
                    new EncounterChoiceDefinition { choiceId = "read_letters", text = "Read", moraleDelta = 3, guiltDelta = 0 },
                    new EncounterChoiceDefinition { choiceId = "burn_van", text = "Burn", moraleDelta = 0, guiltDelta = 4 }
                }
            });
            sys.RegisterEncounter(new EncounterDefinition
            {
                id = "enc_pianist", title = "Pianist", category = "Social",
                baseWeight = 1.5f, minDangerLevel = 3f,
                choices = new List<EncounterChoiceDefinition>
                {
                    new EncounterChoiceDefinition { choiceId = "listen", text = "Listen", moraleDelta = 4, guiltDelta = 0 }
                }
            });

            Check(sys.Catalog.Count == 2, "catalog registers two encounters");
            Check(sys.Find("enc_missing") == null, "unknown id not found");

            var picked = sys.SelectEncounter("Stealth", 1f, null!, new SeededRng(5));
            Check(picked != null && picked.id == "enc_dead_letter_office",
                "danger 1 excludes the pianist (min 3); dead letter offered");
            if (picked == null) return report;
            Check(sys.SelectEncounter("Stealth", 1f, null!, new SeededRng(5))!.id == picked.id,
                "same seed picks the same encounter (determinism)");

            Check(sys.Resolve("enc_dead_letter_office", "burn_van", "loc_ring_road", 40),
                "resolve burn_van");
            Check(sys.State.cumulativeMorale == 0 && sys.State.cumulativeGuilt == 4,
                "choice magnitudes recorded");
            Check(!sys.Resolve("enc_dead_letter_office", "missing_choice", null!, 40),
                "unknown choice refused");
            Check(sys.TotalResolved == 1, "resolution history counts one");

            string before = SaveChecksum.Compute(sys.CaptureState());
            var restored = new NarrativeEncounterSystem();
            restored.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(restored.CaptureState());
            Check(before == after, "save/load checksum stable");

            var snapshot = sys.CaptureState();
            snapshot.history[0].guiltDelta = 99;
            Check(sys.State.cumulativeGuilt == 4, "capture returns snapshot, not live state");

            report.Passed = report.FailedCount == 0;
            report.Summary =
                $"[NarrativeHeadlessDemo] {(report.Passed ? "PASS" : "FAIL")} " +
                $"{report.PassedCount}/{report.PassedCount + report.FailedCount}";
            log.Info(report.Summary);
            return report;
        }
    }
}
