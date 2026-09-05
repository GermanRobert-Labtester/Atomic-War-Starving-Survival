using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Flags;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// F5 / Section 5.12 — Tests for world-flag mutation after encounter choice resolution.
    /// Verifies canonical IFlagLedger mutation, idempotency, failure-safety, determinism,
    /// persistence, and cross-system consumption.
    /// </summary>
    public class MicroLocationWorldFlagTests
    {
        private const string DeadLivestockId = "micro_dead_livestock";
        private const string ScavengeLivestockChoiceId = "scavenge_livestock";
        private const string AvoidLivestockChoiceId = "avoid_livestock";
        private const string ContaminationFlag = "micro_contamination_exposure";

        private const string AbandonedGeneratorId = "micro_abandoned_generator";
        private const string MarkGeneratorChoiceId = "mark_generator";
        private const string GeneratorFlag = "micro_generator_marked";

        private static string DataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        private static NarrativeEncounterSystem CreateProductionNarrativeSystem()
        {
            var sys = new NarrativeEncounterSystem();
            string dataDir = DataDir();
            var defs = NarrativeEncounterCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            sys.RegisterRange(defs);
            return sys;
        }

        [Fact]
        public void F5_01_DeadLivestock_ScavengeChoice_SetsContaminationFlag()
        {
            var sys = CreateProductionNarrativeSystem();
            var ledger = new CampaignConsequenceLedger();

            var res = sys.TryResolve(DeadLivestockId, ScavengeLivestockChoiceId, "loc_suburban_ruins", 5);
            Assert.NotNull(res);
            Assert.Equal(ContaminationFlag, res!.SetWorldFlagId);

            var dispatchRes = EncounterChoiceEffectDispatcher.ApplyWorldFlag(res, ledger);
            Assert.Equal(ContaminationFlag, dispatchRes.FlagId);
            Assert.Equal(EncounterChoiceEffectDispatcher.EffectStatus.Applied, dispatchRes.Status);
            Assert.True(ledger.IsSet(ContaminationFlag));
        }

        [Fact]
        public void F5_02_AbandonedGenerator_MarkChoice_SetsGeneratorFlag()
        {
            var sys = CreateProductionNarrativeSystem();
            var ledger = new CampaignConsequenceLedger();

            var res = sys.TryResolve(AbandonedGeneratorId, MarkGeneratorChoiceId, "loc_industrial_belt", 10);
            Assert.NotNull(res);
            Assert.Equal(GeneratorFlag, res!.SetWorldFlagId);

            var dispatchRes = EncounterChoiceEffectDispatcher.ApplyWorldFlag(res, ledger);
            Assert.Equal(GeneratorFlag, dispatchRes.FlagId);
            Assert.Equal(EncounterChoiceEffectDispatcher.EffectStatus.Applied, dispatchRes.Status);
            Assert.True(ledger.IsSet(GeneratorFlag));
        }

        [Fact]
        public void F5_03_ChoiceWithEmptySetWorldFlag_LeavesFlagsUnchanged()
        {
            var sys = CreateProductionNarrativeSystem();
            var ledger = new CampaignConsequenceLedger();

            var res = sys.TryResolve(DeadLivestockId, AvoidLivestockChoiceId, "loc_suburban_ruins", 5);
            Assert.NotNull(res);
            Assert.True(string.IsNullOrEmpty(res!.SetWorldFlagId));

            var dispatchRes = EncounterChoiceEffectDispatcher.ApplyWorldFlag(res, ledger);
            Assert.Equal(EncounterChoiceEffectDispatcher.EffectStatus.NotApplicable, dispatchRes.Status);
            Assert.False(ledger.IsSet(ContaminationFlag));
            Assert.False(ledger.IsSet(GeneratorFlag));
        }

        [Fact]
        public void F5_04_FailedResolution_DoesNotSetFlag()
        {
            var sys = CreateProductionNarrativeSystem();
            var ledger = new CampaignConsequenceLedger();

            // Unknown choice ID fails resolution (returns null)
            var res = sys.TryResolve(DeadLivestockId, "invalid_choice_id", "loc_suburban_ruins", 5);
            Assert.Null(res);

            var dispatchRes = EncounterChoiceEffectDispatcher.ApplyWorldFlag(res, ledger);
            Assert.Equal(EncounterChoiceEffectDispatcher.EffectStatus.NotApplicable, dispatchRes.Status);
            Assert.False(ledger.IsSet(ContaminationFlag));
        }

        [Fact]
        public void F5_05_AlreadySetFlag_RemainsSafe_Idempotent()
        {
            var sys = CreateProductionNarrativeSystem();
            var ledger = new CampaignConsequenceLedger();

            var res = sys.TryResolve(AbandonedGeneratorId, MarkGeneratorChoiceId, "loc_industrial_belt", 10);
            Assert.NotNull(res);

            // First application
            var res1 = EncounterChoiceEffectDispatcher.ApplyWorldFlag(res, ledger);
            Assert.Equal(EncounterChoiceEffectDispatcher.EffectStatus.Applied, res1.Status);
            Assert.True(ledger.IsSet(GeneratorFlag));

            // Second application: already known, remains set without duplication
            var res2 = EncounterChoiceEffectDispatcher.ApplyWorldFlag(res, ledger);
            Assert.Equal(EncounterChoiceEffectDispatcher.EffectStatus.AlreadyKnown, res2.Status);
            Assert.True(ledger.IsSet(GeneratorFlag));
        }

        [Fact]
        public void F5_06_SaveLoad_PreservesFlag()
        {
            var sys = CreateProductionNarrativeSystem();
            var ledger = new CampaignConsequenceLedger();

            var res = sys.TryResolve(DeadLivestockId, ScavengeLivestockChoiceId, "loc_suburban_ruins", 5);
            EncounterChoiceEffectDispatcher.ApplyWorldFlag(res, ledger);

            // Capture state and serialize
            var serializer = new SystemTextJsonSerializer();
            var savedState = ledger.CaptureState();
            string json = serializer.Serialize(savedState);

            // Fresh ledger restores state
            var restoredState = serializer.Deserialize<CampaignConsequenceSaveState>(json);
            Assert.NotNull(restoredState);
            var restoredLedger = new CampaignConsequenceLedger();
            restoredLedger.RestoreState(restoredState!);

            Assert.True(restoredLedger.IsSet(ContaminationFlag));
        }

        [Fact]
        public void F5_07_EventOrQuestCondition_CanReadFlag()
        {
            var sys = CreateProductionNarrativeSystem();
            var ledger = new CampaignConsequenceLedger();

            var res = sys.TryResolve(AbandonedGeneratorId, MarkGeneratorChoiceId, "loc_industrial_belt", 10);
            EncounterChoiceEffectDispatcher.ApplyWorldFlag(res, ledger);

            // Simulation of a downstream quest / event condition evaluator
            Func<IFlagLedger, bool> generatorEventCondition = flags => flags.IsSet("micro_generator_marked");
            Assert.True(generatorEventCondition(ledger));

            Func<IFlagLedger, bool> otherCondition = flags => flags.IsSet("some_unmet_flag");
            Assert.False(otherCondition(ledger));
        }

        [Fact]
        public void F5_08_ContaminationDiseaseQuery_CanReadRequestedFlag()
        {
            var sys = CreateProductionNarrativeSystem();
            var ledger = new CampaignConsequenceLedger();

            var res = sys.TryResolve(DeadLivestockId, ScavengeLivestockChoiceId, "loc_suburban_ruins", 5);
            EncounterChoiceEffectDispatcher.ApplyWorldFlag(res, ledger);

            // Downstream disease / contamination gating query
            bool hasContaminationExposure = ledger.IsSet("micro_contamination_exposure");
            Assert.True(hasContaminationExposure);
        }

        [Fact]
        public void F5_09_DeterministicSameEncounterChoice_ProducesSameFlagState()
        {
            var sys = CreateProductionNarrativeSystem();
            var ledgerA = new CampaignConsequenceLedger();
            var ledgerB = new CampaignConsequenceLedger();

            var resA = sys.TryResolve(AbandonedGeneratorId, MarkGeneratorChoiceId, "loc_industrial_belt", 10);
            var resB = sys.TryResolve(AbandonedGeneratorId, MarkGeneratorChoiceId, "loc_industrial_belt", 10);

            EncounterChoiceEffectDispatcher.ApplyWorldFlag(resA, ledgerA);
            EncounterChoiceEffectDispatcher.ApplyWorldFlag(resB, ledgerB);

            Assert.True(ledgerA.IsSet(GeneratorFlag));
            Assert.True(ledgerB.IsSet(GeneratorFlag));
            Assert.Equal(ledgerA.IsSet(GeneratorFlag), ledgerB.IsSet(GeneratorFlag));
        }

        [Fact]
        public void F5_10_DuplicateFlagSet_DoesNotCreateDuplicateUnintendedEffects()
        {
            var ledger = new CampaignConsequenceLedger();
            int eventFireCount = 0;
            ledger.OnConsequenceRecorded += _ => eventFireCount++;

            ledger.Set(GeneratorFlag);
            Assert.Equal(1, eventFireCount);

            // Duplicate call must NOT fire event again or create redundant history records
            ledger.Set(GeneratorFlag);
            Assert.Equal(1, eventFireCount);
            Assert.True(ledger.IsSet(GeneratorFlag));
        }

        [Fact]
        public void F5_11_MicroFlagNamespaces_FollowConvention_NoWhitespace_Unique()
        {
            var dataDir = DataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var allEncounters = NarrativeEncounterCatalogLoader.Load(dataDir, fileIO, json);

            var microFlags = new HashSet<string>(StringComparer.Ordinal);
            foreach (var enc in allEncounters)
            {
                if (enc.choices == null) continue;
                foreach (var choice in enc.choices)
                {
                    if (string.IsNullOrWhiteSpace(choice.setWorldFlag)) continue;
                    string flag = choice.setWorldFlag;

                    // Micro-location choices must follow micro_ namespace convention
                    if (enc.id.StartsWith("micro_", StringComparison.Ordinal))
                    {
                        Assert.StartsWith("micro_", flag, StringComparison.Ordinal);
                    }

                    // No leading or trailing whitespace
                    Assert.Equal(flag.Trim(), flag);
                    Assert.DoesNotContain(" ", flag);

                    microFlags.Add(flag);
                }
            }

            Assert.Contains(ContaminationFlag, microFlags);
            Assert.Contains(GeneratorFlag, microFlags);
        }

        [Fact]
        public void F5_12_OldEncountersWithoutField_BehaveUnchanged()
        {
            var sys = CreateProductionNarrativeSystem();
            var ledger = new CampaignConsequenceLedger();

            // Roadside memorial leave choice has no world flag
            var res = sys.TryResolve("micro_roadside_memorial", "leave_memorial", "loc_any", 1);
            Assert.NotNull(res);
            Assert.Empty(res!.SetWorldFlagId);

            var dispatchRes = EncounterChoiceEffectDispatcher.ApplyWorldFlag(res, ledger);
            Assert.Equal(EncounterChoiceEffectDispatcher.EffectStatus.NotApplicable, dispatchRes.Status);
            Assert.False(dispatchRes.IsApplied);
        }
    }
}
