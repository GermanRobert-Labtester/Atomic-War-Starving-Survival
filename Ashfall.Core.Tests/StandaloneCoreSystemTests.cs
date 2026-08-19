using System.Collections.Generic;
using Ashfall.Core.Endgame;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Flags;
using Ashfall.Core.Legacy;
using Ashfall.Core.Medical;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Tests for 5 standalone Core systems that previously had zero test coverage:
    /// SkyLayerArmorSystem, VigilStateMachine, GenerationalSuccessionEngine,
    /// EpilogueMatrixRuntime, DiveInstanceRunner.
    /// </summary>
    public class StandaloneCoreSystemTests
    {
        // ── SkyLayerArmorSystem ─────────────────────────────────────────────────

        [Fact]
        public void SkyArmor_SetCell_GetAttenuation()
        {
            var sys = new SkyLayerArmorSystem();
            sys.SetCellArmor(0, CeilingMaterialTier.LeadSheeting, 0.5f);
            float atten = sys.GetAttenuationFactor(0);
            Assert.True(atten < 1.0f, "Lead should attenuate");
            Assert.True(atten > 0.0f, "Attenuation should be positive");
        }

        [Fact]
        public void SkyArmor_UnprotectedCell_FullBleed()
        {
            var sys = new SkyLayerArmorSystem();
            Assert.Equal(1.0f, sys.GetAttenuationFactor(99));
        }

        [Fact]
        public void SkyArmor_BetterMaterial_LessAttenuation()
        {
            var sys = new SkyLayerArmorSystem();
            sys.SetCellArmor(0, CeilingMaterialTier.Dirt, 1f);
            sys.SetCellArmor(1, CeilingMaterialTier.TungstenComposite, 1f);
            Assert.True(sys.GetAttenuationFactor(1) < sys.GetAttenuationFactor(0));
        }

        [Fact]
        public void SkyArmor_KineticImpact_Absorbed()
        {
            var sys = new SkyLayerArmorSystem();
            sys.SetCellArmor(0, CeilingMaterialTier.ReinforcedConcrete, 2f);
            bool breached = sys.EvaluateKineticImpact(0, 10f, out float damage);
            Assert.False(breached, "Low energy should be absorbed");
            Assert.Equal(0f, damage);
        }

        [Fact]
        public void SkyArmor_KineticImpact_Breached()
        {
            var sys = new SkyLayerArmorSystem();
            sys.SetCellArmor(0, CeilingMaterialTier.Dirt, 0.5f);
            bool breached = sys.EvaluateKineticImpact(0, 100f, out float damage);
            Assert.True(breached);
            Assert.True(damage > 0f);
        }

        [Fact]
        public void SkyArmor_KineticImpact_Unprotected()
        {
            var sys = new SkyLayerArmorSystem();
            bool breached = sys.EvaluateKineticImpact(99, 10f, out float damage);
            Assert.True(breached);
            Assert.True(damage > 0f);
        }

        [Fact]
        public void SkyArmor_DurabilityDecreases_OnImpact()
        {
            var sys = new SkyLayerArmorSystem();
            sys.SetCellArmor(0, CeilingMaterialTier.Wood, 1f, 100f);
            sys.EvaluateKineticImpact(0, 1f, out _);
            var save = sys.CaptureState();
            Assert.True(save.cells[0].currentDurability < 100f);
        }

        [Fact]
        public void SkyArmor_CaptureRestore_Roundtrip()
        {
            var sys = new SkyLayerArmorSystem();
            sys.SetCellArmor(0, CeilingMaterialTier.LeadSheeting, 0.3f, 80f);
            sys.SetCellArmor(5, CeilingMaterialTier.Dirt, 1f, 50f);
            var save = sys.CaptureState();
            Assert.Equal(2, save.cells.Count);

            var restored = new SkyLayerArmorSystem();
            restored.RestoreState(save);
            var c0 = restored.CaptureState().cells.Find(c => c.gridX == 0);
            var c5 = restored.CaptureState().cells.Find(c => c.gridX == 5);
            Assert.NotNull(c0);
            Assert.NotNull(c5);
            Assert.Equal(80f, c0.currentDurability);
            Assert.Equal(50f, c5.currentDurability);
        }

        [Fact]
        public void SkyArmor_RestoreNull_DoesNotCrash()
        {
            var sys = new SkyLayerArmorSystem();
            sys.SetCellArmor(0, CeilingMaterialTier.Dirt, 1f);
            sys.RestoreState(null);
            Assert.Empty(sys.CaptureState().cells);
        }

        // ── VigilStateMachine ───────────────────────────────────────────────────

        [Fact]
        public void Vigil_Start_ActiveAndFiresEvent()
        {
            var vigil = new VigilStateMachine();
            string startedFor = null;
            vigil.OnVigilStarted += id => startedFor = id;
            vigil.StartVigil("dweller_1", new[] { "Alice", "Bob" });
            Assert.True(vigil.IsActive);
            Assert.Equal("dweller_1", startedFor);
        }

        [Fact]
        public void Vigil_Tick_ReciteNames()
        {
            var vigil = new VigilStateMachine();
            var recited = new List<string>();
            vigil.OnNameRecited += (name, _) => recited.Add(name);
            vigil.StartVigil("dweller_1", new[] { "Alice", "Bob" }, 100f);

            vigil.Tick(50f);
            Assert.True(recited.Count >= 1);

            vigil.Tick(50f);
            Assert.Equal(2, recited.Count);
        }

        [Fact]
        public void Vigil_PhantomKnock_At95Percent()
        {
            var vigil = new VigilStateMachine();
            bool knockFired = false;
            vigil.OnPhantomKnock += () => knockFired = true;
            vigil.StartVigil("dweller_1", new[] { "Alice" }, 100f);

            vigil.Tick(90f);
            Assert.False(knockFired);

            vigil.Tick(10f);
            Assert.True(knockFired);
        }

        [Fact]
        public void Vigil_Completes_AtDuration()
        {
            var vigil = new VigilStateMachine();
            bool completed = false;
            bool wasSkipped = true;
            vigil.OnVigilCompleted += skipped => { completed = true; wasSkipped = skipped; };
            vigil.StartVigil("dweller_1", new[] { "Alice" }, 100f);

            vigil.Tick(100f);
            Assert.True(completed);
            Assert.False(wasSkipped);
            Assert.False(vigil.IsActive);
            Assert.True(vigil.IsCompleted);
        }

        [Fact]
        public void Vigil_Skip_CompletesEarly()
        {
            var vigil = new VigilStateMachine();
            bool completed = false;
            bool wasSkipped = false;
            vigil.OnVigilCompleted += skipped => { completed = true; wasSkipped = skipped; };
            vigil.StartVigil("dweller_1", new[] { "Alice" }, 100f);

            vigil.Skip();
            Assert.True(completed);
            Assert.True(wasSkipped);
        }

        [Fact]
        public void Vigil_CaptureRestore_Roundtrip()
        {
            var vigil = new VigilStateMachine();
            vigil.StartVigil("dweller_1", new[] { "Alice", "Bob" }, 200f);
            vigil.Tick(50f);

            var save = vigil.CaptureState();
            Assert.True(save.isActive);
            Assert.Equal(2, save.namesToRecite.Count);

            var restored = new VigilStateMachine();
            restored.RestoreState(save);
            Assert.True(restored.IsActive);
            Assert.Equal("dweller_1", restored.DwellerId);
            Assert.Equal(2, restored.Names.Count);
        }

        [Fact]
        public void Vigil_RestoreNull_DoesNotCrash()
        {
            var vigil = new VigilStateMachine();
            vigil.RestoreState(null);
            Assert.False(vigil.IsActive);
        }

        // ── GenerationalSuccessionEngine ────────────────────────────────────────

        [Fact]
        public void GenSuccession_RegisterDweller()
        {
            var engine = new GenerationalSuccessionEngine();
            engine.RegisterDweller("founder_1", 30);
            var rec = engine.GetRecord("founder_1");
            Assert.NotNull(rec);
            Assert.Equal(30, rec.inGameAgeYears);
            Assert.Equal(0, rec.generationIndex);
        }

        [Fact]
        public void GenSuccession_AdvanceTime_AgesDwellers()
        {
            var engine = new GenerationalSuccessionEngine();
            engine.RegisterDweller("founder_1", 30);
            engine.AdvanceTime(365);
            Assert.Equal(31, engine.GetRecord("founder_1").inGameAgeYears);
            Assert.Equal(1, engine.TotalYearsElapsed);
        }

        [Fact]
        public void GenSuccession_Retirement_At65()
        {
            var engine = new GenerationalSuccessionEngine();
            engine.RegisterDweller("founder_1", 63);
            string retiredId = null;
            engine.OnDwellerRetired += (id, _) => retiredId = id;

            engine.AdvanceTime(365);
            Assert.Null(retiredId);

            engine.AdvanceTime(365);
            Assert.Equal("founder_1", retiredId);
            Assert.True(engine.GetRecord("founder_1").isRetired);
        }

        [Fact]
        public void GenSuccession_Mentorship_TransfersTrait()
        {
            var engine = new GenerationalSuccessionEngine();
            engine.RegisterDweller("mentor", 50);
            engine.RegisterDweller("apprentice", 20);

            string inheritedTrait = null;
            engine.OnTraitInherited += (_, __, trait) => inheritedTrait = trait;

            Assert.True(engine.FormMentorship("mentor", "apprentice", "trait_farming"));
            Assert.Equal("trait_farming", inheritedTrait);
            Assert.Contains("trait_farming", engine.GetRecord("apprentice").inheritedTraitIds);
        }

        [Fact]
        public void GenSuccession_Mentorship_RejectsDeceased()
        {
            var engine = new GenerationalSuccessionEngine();
            engine.RegisterDweller("mentor", 50);
            engine.RegisterDweller("apprentice", 20);
            engine.GetRecord("mentor").isDeceased = true;
            Assert.False(engine.FormMentorship("mentor", "apprentice", "trait_x"));
        }

        [Fact]
        public void GenSuccession_ChapterAdvance_FiresEvent()
        {
            var engine = new GenerationalSuccessionEngine();
            int advancedTo = 0;
            engine.OnChapterAdvanced += ch => advancedTo = ch;
            engine.AdvanceTime(365);
            Assert.Equal(2, advancedTo);
        }

        [Fact]
        public void GenSuccession_CaptureRestore_Roundtrip()
        {
            var engine = new GenerationalSuccessionEngine();
            engine.RegisterDweller("founder_1", 30);
            engine.RegisterDweller("apprentice", 20, 1);
            engine.FormMentorship("founder_1", "apprentice", "trait_x");
            engine.AdvanceTime(365);

            var save = engine.CaptureState();
            Assert.Equal(2, save.generationRecords.Count);

            var restored = new GenerationalSuccessionEngine();
            restored.RestoreState(save);
            Assert.Equal(31, restored.GetRecord("founder_1").inGameAgeYears);
            Assert.Contains("trait_x", restored.GetRecord("apprentice").inheritedTraitIds);
        }

        // ── EpilogueMatrixRuntime ───────────────────────────────────────────────

        [Fact]
        public void Epilogue_TrueReconciliation_AllConditions()
        {
            var rt = new EpilogueMatrixRuntime();
            var ctx = new EpilogueEvaluationContext
            {
                grandTreatySigned = true,
                tempestDecommissioned = true,
                debtLedgersBurned = true,
                livingDwellerCount = 10,
                childrenSurvived = true
            };
            Assert.Equal(RegionalFate.TrueReconciliation, rt.EvaluateRegionalFate(ctx));
        }

        [Fact]
        public void Epilogue_Commonwealth_TreatyPlusBurned()
        {
            var rt = new EpilogueMatrixRuntime();
            var ctx = new EpilogueEvaluationContext
            {
                grandTreatySigned = true,
                tempestDecommissioned = false,
                debtLedgersBurned = true,
                livingDwellerCount = 5
            };
            Assert.Equal(RegionalFate.CommonwealthFounded, rt.EvaluateRegionalFate(ctx));
        }

        [Fact]
        public void Epilogue_Garrison_TreatyWithoutBurned()
        {
            var rt = new EpilogueMatrixRuntime();
            var ctx = new EpilogueEvaluationContext
            {
                grandTreatySigned = true,
                debtLedgersBurned = false,
                livingDwellerCount = 5
            };
            Assert.Equal(RegionalFate.GarrisonMartialLaw, rt.EvaluateRegionalFate(ctx));
        }

        [Fact]
        public void Epilogue_TempestSterilization_HighDeaths()
        {
            var rt = new EpilogueMatrixRuntime();
            var ctx = new EpilogueEvaluationContext
            {
                tempestDecommissioned = false,
                totalDeathsRecorded = 60,
                livingDwellerCount = 5
            };
            Assert.Equal(RegionalFate.TempestSterilization, rt.EvaluateRegionalFate(ctx));
        }

        [Fact]
        public void Epilogue_NullContext_FracturedWarlords()
        {
            var rt = new EpilogueMatrixRuntime();
            Assert.Equal(RegionalFate.FracturedWarlords, rt.EvaluateRegionalFate(null));
        }

        [Fact]
        public void Epilogue_Demographics_Thriving()
        {
            var rt = new EpilogueMatrixRuntime();
            var ctx = new EpilogueEvaluationContext { livingDwellerCount = 10, childrenSurvived = true };
            Assert.Equal(DemographicOutcome.ThrivingCommunity, rt.EvaluateDemographics(ctx));
        }

        [Fact]
        public void Epilogue_Demographics_Extinction()
        {
            var rt = new EpilogueMatrixRuntime();
            var ctx = new EpilogueEvaluationContext { livingDwellerCount = 0 };
            Assert.Equal(DemographicOutcome.TotalExtinction, rt.EvaluateDemographics(ctx));
        }

        [Fact]
        public void Epilogue_Moral_Forgiven()
        {
            var rt = new EpilogueMatrixRuntime();
            var ctx = new EpilogueEvaluationContext { debtLedgersBurned = true, childrenSurvived = true };
            Assert.Equal(MoralStanding.ForgivenAndReconciled, rt.EvaluateMoralStanding(ctx));
        }

        [Fact]
        public void Epilogue_Narrative_ContainsKeyPhrases()
        {
            var rt = new EpilogueMatrixRuntime();
            var ctx = new EpilogueEvaluationContext
            {
                totalDaysSurvived = 365,
                livingDwellerCount = 10,
                totalDeathsRecorded = 5,
                grandTreatySigned = true,
                tempestDecommissioned = true,
                debtLedgersBurned = true,
                childrenSurvived = true
            };
            string narrative = rt.GenerateEpilogueNarrative(ctx);
            Assert.Contains("CHRONICLE OF TESSARAT", narrative);
            Assert.Contains("365 Days", narrative);
        }

        // ── DiveInstanceRunner ──────────────────────────────────────────────────

        private static DiveInstanceRunner MakeRunner()
        {
            var site = new DiveSiteDefinition("site_test", 120, 0.5, "q_test");
            return new DiveInstanceRunner(new SimpleEventBus(), new InMemoryFlagLedger(), new SeededRng(42), site);
        }

        [Fact]
        public void DiveRunner_StartsInDeckhouse()
        {
            var runner = MakeRunner();
            Assert.Equal(DiveRoom.deckhouse, runner.CurrentRoom);
            Assert.Equal(120, runner.OxygenRemaining);
        }

        [Fact]
        public void DiveRunner_Advance_MovesForward()
        {
            var runner = MakeRunner();
            Assert.True(runner.Advance());
            Assert.Equal(DiveRoom.companionway, runner.CurrentRoom);
            Assert.True(runner.Advance());
            Assert.Equal(DiveRoom.hold_approach, runner.CurrentRoom);
            Assert.True(runner.Advance());
            Assert.Equal(DiveRoom.the_hold, runner.CurrentRoom);
        }

        [Fact]
        public void DiveRunner_Advance_RejectsAtEnd()
        {
            var runner = MakeRunner();
            runner.Advance(); runner.Advance(); runner.Advance();
            Assert.False(runner.Advance());
            Assert.Equal(DiveRoom.the_hold, runner.CurrentRoom);
        }

        [Fact]
        public void DiveRunner_TickOxygen_Decrements()
        {
            var runner = MakeRunner();
            runner.TickOxygen();
            Assert.Equal(119, runner.OxygenRemaining);
        }

        [Fact]
        public void DiveRunner_TickOxygen_LowWarning_At30()
        {
            var bus = new SimpleEventBus();
            var site = new DiveSiteDefinition("site_test", 31, 0.5, "q_test");
            var runner = new DiveInstanceRunner(bus, new InMemoryFlagLedger(), new SeededRng(42), site);

            bool lowFired = false;
            bus.Subscribe("dive.oxygen.low", _ => lowFired = true);
            runner.TickOxygen();
            Assert.True(lowFired);
        }

        [Fact]
        public void DiveRunner_CommitChoice_SetsFlag()
        {
            var bus = new SimpleEventBus();
            var flags = new InMemoryFlagLedger();
            var site = new DiveSiteDefinition("site_test", 120, 0.5, "q_test");
            var runner = new DiveInstanceRunner(bus, flags, new SeededRng(42), site);
            runner.Advance(); runner.Advance(); runner.Advance();

            runner.CommitChoice(SovereignChoice.flood_the_market);
            Assert.True(flags.IsSet("flag_exp09_iodine_released"));
        }

        [Fact]
        public void DiveRunner_CommitChoice_RejectsOutsideHold()
        {
            var runner = MakeRunner();
            runner.CommitChoice(SovereignChoice.burn_the_hold);
            Assert.Equal(SovereignChoice.undecided, runner.Choice);
        }

        [Fact]
        public void DiveRunner_CommitChoice_OneShot()
        {
            var runner = MakeRunner();
            runner.Advance(); runner.Advance(); runner.Advance();
            runner.CommitChoice(SovereignChoice.flood_the_market);
            runner.CommitChoice(SovereignChoice.burn_the_hold);
            Assert.Equal(SovereignChoice.flood_the_market, runner.Choice);
        }

        [Fact]
        public void DiveRunner_DetectionRisk_HigherInCompanionway()
        {
            var runner = MakeRunner();
            double riskDeck = runner.DetectionRisk(0.8, false);
            runner.Advance();
            double riskComp = runner.DetectionRisk(0.8, false);
            Assert.True(riskComp > riskDeck, "Companionway should be riskier (less storm masking)");
        }

        [Fact]
        public void DiveRunner_DetectionRisk_FearIncreasesInHold()
        {
            var runner = MakeRunner();
            runner.Advance(); runner.Advance();
            double calm = runner.DetectionRisk(0.5, false);
            double fear = runner.DetectionRisk(0.5, true);
            Assert.True(fear > calm);
        }
    }
}
