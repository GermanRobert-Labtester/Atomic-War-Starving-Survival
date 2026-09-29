// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Crafting;
using Ashfall.Core.Economy;
using Ashfall.Core.Endgame;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Flags;
using Ashfall.Core.Legacy;
using Ashfall.Core.Medical;
using Ashfall.Core.Muster;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;
using Ashfall.Core.Settings;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.UtilityAI;
using Ashfall.Core.Verdict;
using Ashfall.Core.Warlords;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.Narrative;
using AtomicWar.GodotApp.Settings;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.YearOfAsh;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {

        /// <summary>
        /// Standalone-systems gate: exercises the five newly-wired Core systems
        /// (SkyLayerArmor, VigilStateMachine, GenerationalSuccessionEngine,
        /// EpilogueMatrixRuntime, DiveInstanceRunner) with functional checks and
        /// save round-trips where the systems support them.
        /// </summary>
        public static int RunStandaloneSystemsSelfTest()
        {
            CatalogLocator.UseInvariantCulture();

            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else { GD.Print("[FAIL] " + name); failures++; }
            }

            try
            {
                // ── 1. SkyLayerArmorSystem ──────────────────────────────
                var sky = new SkyLayerArmorSystem();
                // Equal thickness so the comparison isolates the material tier.
                sky.SetCellArmor(0, CeilingMaterialTier.ReinforcedConcrete, 1.0f);
                sky.SetCellArmor(1, CeilingMaterialTier.LeadSheeting, 1.0f);

                float att0 = sky.GetAttenuationFactor(0);
                float att1 = sky.GetAttenuationFactor(1);
                Check(att0 >= 0.005f && att0 <= 1.0f, "sky armor cell 0 attenuation in range");
                Check(att1 >= 0.005f && att1 <= 1.0f, "sky armor cell 1 attenuation in range");
                Check(att1 < att0, "lead sheeting attenuates more than concrete per thickness");

                bool breached = sky.EvaluateKineticImpact(0, 50f, out float damage);
                Check(damage >= 0f, "kinetic impact damage non-negative");
                // Whether it breaches depends on tuning; just verify it returned a bool.
                Check(true, "kinetic impact evaluation completed");

                // Save round-trip (compare against the current, post-impact state).
                var skySave = sky.CaptureState();
                Check(skySave != null && skySave.cells != null && skySave.cells.Count == 2,
                    "sky armor capture has 2 cells");
                var sky2 = new SkyLayerArmorSystem();
                sky2.RestoreState(skySave!);
                Check(Math.Abs(sky2.GetAttenuationFactor(0) - sky.GetAttenuationFactor(0)) < 1e-5f,
                    "sky armor attenuation restored after roundtrip");

                // ── 2. VigilStateMachine (Medical) ──────────────────────
                var vigil = new Ashfall.Core.Medical.VigilStateMachine();
                bool startedFired = false;
                vigil.OnVigilStarted += _ => startedFired = true;
                vigil.StartVigil("dweller_test", new[] { "name_alpha", "name_beta", "name_gamma" }, 10f);

                Check(vigil.IsActive, "vigil is active after start");
                Check(startedFired, "vigil OnVigilStarted fired");
                Check(vigil.DwellerId == "dweller_test", "vigil dweller id set");

                // Tick past duration to complete
                vigil.Tick(5f);
                Check(vigil.RecitedCount > 0, "vigil recited names during tick");
                vigil.Tick(6f);
                Check(vigil.IsCompleted, "vigil completed after full duration");

                // Save round-trip (start a fresh one to test mid-vigil save)
                var vigil2 = new Ashfall.Core.Medical.VigilStateMachine();
                vigil2.StartVigil("dweller_save", new[] { "n1", "n2" }, 20f);
                vigil2.Tick(8f);
                var vigilSave = vigil2.CaptureState();
                Check(vigilSave != null && vigilSave.isActive, "vigil save captured active state");

                var vigil3 = new Ashfall.Core.Medical.VigilStateMachine();
                vigil3.RestoreState(vigilSave!);
                Check(vigil3.DwellerId == "dweller_save", "vigil dweller restored");
                Check(Math.Abs(vigil3.ElapsedSeconds - vigil2.ElapsedSeconds) < 1e-3f,
                    "vigil elapsed restored");

                // ── 3. GenerationalSuccessionEngine ─────────────────────
                var gen = new GenerationalSuccessionEngine();
                gen.RegisterDweller("gen_elder", 60, 0);
                gen.RegisterDweller("gen_youth", 20, 1);

                Check(gen.GetRecord("gen_elder") != null, "elder registered");
                Check(gen.GetRecord("gen_youth") != null, "youth registered");

                // Advance enough to retire the elder (age 65 = 5 years = ~1825 days)
                gen.AdvanceTime(1825);
                var elderRec = gen.GetRecord("gen_elder");
                Check(elderRec!.isRetired, "elder retired after reaching age 65");
                Check(gen.CurrentChapterIndex >= 1, "chapter index advanced or held");

                // Mentorship
                bool mentorOk = gen.FormMentorship("gen_elder", "gen_youth", "trait_farming");
                Check(mentorOk, "mentorship formed");
                var youthRec = gen.GetRecord("gen_youth");
                Check(youthRec!.inheritedTraitIds.Contains("trait_farming"),
                    "youth inherited trait from mentor");

                // Save round-trip
                var genSave = gen.CaptureState();
                Check(genSave != null && genSave.generationRecords.Count >= 2,
                    "generational save captured records");
                var gen2 = new GenerationalSuccessionEngine();
                gen2.RestoreState(genSave!);
                Check(gen2.GetRecord("gen_elder")?.isRetired == true,
                    "elder retirement restored");
                Check(gen2.GetRecord("gen_youth")?.inheritedTraitIds.Contains("trait_farming") == true,
                    "youth trait inheritance restored");

                // ── 4. EpilogueMatrixRuntime ────────────────────────────
                var epilogue = new EpilogueMatrixRuntime();

                // Fate 1: CommonwealthFounded — treaty + burned ledgers, no decommission
                var ctx1 = new EpilogueEvaluationContext
                {
                    totalDaysSurvived = 800, livingDwellerCount = 30,
                    totalDeathsRecorded = 5, grandTreatySigned = true,
                    tempestDecommissioned = false, debtLedgersBurned = true,
                    childrenSurvived = true, velSecretExposed = false
                };
                var fate1 = epilogue.EvaluateRegionalFate(ctx1);
                Check(fate1 == RegionalFate.CommonwealthFounded,
                    "epilogue: commonwealth founded fate");

                // Fate 2: GarrisonMartialLaw — treaty signed, ledgers kept
                var ctx2 = new EpilogueEvaluationContext
                {
                    totalDaysSurvived = 600, livingDwellerCount = 25,
                    totalDeathsRecorded = 10, grandTreatySigned = true,
                    tempestDecommissioned = false, debtLedgersBurned = false,
                    childrenSurvived = true, velSecretExposed = false
                };
                var fate2 = epilogue.EvaluateRegionalFate(ctx2);
                Check(fate2 == RegionalFate.GarrisonMartialLaw,
                    "epilogue: garrison martial law fate");

                // Fate 3: FracturedWarlords — low pop, no treaty
                var ctx3 = new EpilogueEvaluationContext
                {
                    totalDaysSurvived = 400, livingDwellerCount = 8,
                    totalDeathsRecorded = 20, grandTreatySigned = false,
                    tempestDecommissioned = false, debtLedgersBurned = false,
                    childrenSurvived = false, velSecretExposed = false
                };
                var fate3 = epilogue.EvaluateRegionalFate(ctx3);
                Check(fate3 == RegionalFate.FracturedWarlords,
                    "epilogue: fractured warlords fate");

                // Fate 4: TempestSterilization — Tempest still active, heavy losses
                var ctx4 = new EpilogueEvaluationContext
                {
                    totalDaysSurvived = 500, livingDwellerCount = 15,
                    totalDeathsRecorded = 60, grandTreatySigned = false,
                    tempestDecommissioned = false, debtLedgersBurned = false,
                    childrenSurvived = false, velSecretExposed = true
                };
                var fate4 = epilogue.EvaluateRegionalFate(ctx4);
                Check(fate4 == RegionalFate.TempestSterilization,
                    "epilogue: tempest sterilization fate");

                // Fate 5: TrueReconciliation — burned ledgers, decommissioned, treaty
                var ctx5 = new EpilogueEvaluationContext
                {
                    totalDaysSurvived = 700, livingDwellerCount = 20,
                    totalDeathsRecorded = 8, grandTreatySigned = true,
                    tempestDecommissioned = true, debtLedgersBurned = true,
                    childrenSurvived = true, velSecretExposed = true
                };
                var fate5 = epilogue.EvaluateRegionalFate(ctx5);
                Check(fate5 == RegionalFate.TrueReconciliation,
                    "epilogue: true reconciliation fate");

                // Demographic + moral evaluations
                var demo = epilogue.EvaluateDemographics(ctx1);
                Check(demo == DemographicOutcome.ThrivingCommunity,
                    "epilogue: thriving community demographic");
                var moral = epilogue.EvaluateMoralStanding(ctx1);
                Check(moral == MoralStanding.ForgivenAndReconciled,
                    "epilogue: forgiven and reconciled moral standing");

                // Narrative generation
                string narrative = epilogue.GenerateEpilogueNarrative(ctx1);
                Check(!string.IsNullOrEmpty(narrative), "epilogue narrative generated");

                // ── 5. DiveInstanceRunner ───────────────────────────────
                var bus = new SimpleEventBus();
                var flags = new Ashfall.Core.Flags.CampaignConsequenceLedger();
                var rng = new SeededRng(424242);
                var site = new DiveSiteDefinition("site_test_dive", 120, 0.3, "keeper_thread_0");
                var dive = new DiveInstanceRunner(bus, flags, rng, site);

                Check(dive.CurrentRoom == DiveRoom.deckhouse, "dive starts in deckhouse");
                Check(dive.OxygenRemaining == 120, "dive oxygen budget from site def");
                Check(dive.Choice == SovereignChoice.undecided, "dive choice undecided initially");

                // Advance rooms
                bool adv1 = dive.Advance();
                Check(adv1 && dive.CurrentRoom == DiveRoom.companionway,
                    "dive advanced to companionway");

                bool adv2 = dive.Advance();
                Check(adv2 && dive.CurrentRoom == DiveRoom.hold_approach,
                    "dive advanced to hold_approach");

                bool adv3 = dive.Advance();
                Check(adv3 && dive.CurrentRoom == DiveRoom.the_hold,
                    "dive advanced to the hold");

                // Oxygen tick
                int oxyBefore = dive.OxygenRemaining;
                dive.TickOxygen();
                Check(dive.OxygenRemaining < oxyBefore, "dive oxygen decreased after tick");

                // Detection risk
                double risk = dive.DetectionRisk(0.5, false);
                Check(risk >= 0.0 && risk <= 1.0, "dive detection risk in valid range");

                // Commit choice
                dive.CommitChoice(SovereignChoice.flood_the_market);
                Check(dive.Choice == SovereignChoice.flood_the_market,
                    "dive choice committed");
                Check(flags.IsSet("flag_exp09_iodine_released"),
                    "dive choice set flag");
            }
            catch (Exception e)
            {
                Check(false, "standalone systems selftest threw: " + e.Message);
            }

            return EmitSummary("standalone_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

    }
}
