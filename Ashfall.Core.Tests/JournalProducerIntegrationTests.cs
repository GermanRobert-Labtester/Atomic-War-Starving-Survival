using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Flags;
using Ashfall.Core.Journal;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class JournalProducerIntegrationTests
    {
        private sealed class TestSurvivorAuthor : ISurvivorAuthor
        {
            public TestSurvivorAuthor(string id, string name, RiskBiasTrait bias)
            {
                Id = id;
                DisplayName = name;
                RiskBias = bias;
            }

            public string Id { get; }
            public string DisplayName { get; }
            public RiskBiasTrait RiskBias { get; }
        }

        [Fact]
        public void AutopsyProducer_WritesJournalEntry_AndEnforcesDedupOnRepeat()
        {
            var journal = new JournalSystem();
            var rng = new SeededRng(42);
            var inv = new Ashfall.Core.Inventory.Inventory();
            inv.AddById("item_scalpel", 5);
            inv.AddById("item_formalin", 5);
            var radiation = new Ashfall.Core.Radiation.RadiationSystem(seed: 42);
            var starting = new Ashfall.Core.StartingLevel.StartingLevelSystem();
            var ventilation = new Ashfall.Core.VentilationSystem(starting);
            var research = new Ashfall.Core.ResearchSystem();
            var medical = new Ashfall.Core.Medical.MedicalWardSystem(
                new Ashfall.Core.Medical.MedicalWardState(),
                new[] { new Ashfall.Core.Medical.MedicalBed("bed_1", "Bed 1", Ashfall.Core.Medical.MedicalBedCategory.General) },
                new[] { new Ashfall.Core.Medical.MedicalProcedureDef("proc_1", "Proc", "Med") });

            var autopsy = new AutopsySystem(rng, inv, radiation, ventilation, research, medical);
            autopsy.LoadCatalog(new List<AutopsyProcedure>
            {
                new AutopsyProcedure
                {
                    procedure_id = "proc_standard",
                    display_name = "Standard Screen",
                    requiredTools = new List<string> { "item_scalpel" },
                    requiredConsumables = new List<string> { "item_formalin" },
                    possibleFindings = new List<string> { "radiation_tissue_necrosis" }
                }
            });

            // Wire producer hook as in host
            autopsy.OnCaseCompleted += c =>
            {
                journal.TryAddRawEntry(
                    "autopsy_finding_" + c.caseId,
                    $"Autopsy complete for {c.specimenId}: {c.finding}",
                    new TestSurvivorAuthor(c.assignedMedicId, "Dr. Medic", RiskBiasTrait.Cautious),
                    day: 5,
                    hour: 14f);
            };

            autopsy.QueueAutopsy("specimen_alpha", "proc_standard", "medic_bob");
            var c1 = autopsy.State.cases[0];
            autopsy.BeginAutopsy(c1.caseId);
            autopsy.TickDay(5);

            Assert.Equal(1, journal.EntryCount);
            Assert.Contains("specimen_alpha", journal.Entries[0].Text);
            Assert.Equal("autopsy_finding_" + c1.caseId, journal.Entries[0].KnowledgeKey);
            Assert.Equal("Dr. Medic", journal.Entries[0].AuthorName);
            Assert.True(journal.HasUnread);

            // Attempting to log the identical key again (e.g. duplicate notification) must be blocked
            var duplicateEntry = journal.TryAddRawEntry(
                "autopsy_finding_" + c1.caseId,
                "Duplicate report text",
                new TestSurvivorAuthor("medic_bob", "Bob", RiskBiasTrait.Realist),
                day: 6);

            Assert.Null(duplicateEntry);
            Assert.Equal(1, journal.EntryCount);
        }

        [Fact]
        public void LibraryStudyProducer_GrantsKnowledgeUnlocks_ViaAddKnowledgeEvidence()
        {
            var skills = new SkillProgressionSystem();
            var research = new ResearchSystem();
            var journal = new JournalSystem();
            var roster = new DutyRosterSystem();
            var library = new LibraryStudySystem(skills, research, journal, roster);

            var manual = new ManualDefinition
            {
                manual_id = "man_radiation_physics",
                display_name = "Principles of Radiation Shielding",
                category = "technical",
                studyHoursRequired = 8,
                knowledgeUnlocks = new List<string> { "k_lead_baffling", "k_dosimeter_calibration" }
            };
            library.LoadCatalog(new List<ManualDefinition> { manual });

            Assert.Equal(0, journal.CodexUnlockCount);
            Assert.False(journal.Knowledge.Has("k_lead_baffling"));
            Assert.False(journal.Knowledge.Has("k_dosimeter_calibration"));

            var result = library.StartStudy("man_radiation_physics", "survivor_clara");
            Assert.True(result.IsSuccess);

            // Tick 1 day (8 hours) -> completes manual
            library.TickDay(1);

            Assert.True(library.IsManualCompleted("man_radiation_physics"));
            Assert.Equal(2, journal.CodexUnlockCount);
            Assert.True(journal.Knowledge.Has("k_lead_baffling"));
            Assert.True(journal.Knowledge.Has("k_dosimeter_calibration"));

            // Repeated call to AddKnowledgeEvidence for same key returns false and does not increment unlocks
            bool repeatUnlock = journal.AddKnowledgeEvidence("survivor_clara", "k_lead_baffling");
            Assert.False(repeatUnlock);
            Assert.Equal(2, journal.CodexUnlockCount);
        }

        [Fact]
        public void MoralChoiceProducer_WritesJournalEntry_OnQuestResolved()
        {
            var journal = new JournalSystem();
            var flags = new InMemoryFlagLedger();
            var rng = new SeededRng(1337);
            var moralSystem = new MoralChoiceSystem(rng, flags: flags);

            moralSystem.OnQuestResolved += r =>
            {
                journal.TryAddRawEntry(
                    r.questId,
                    $"Resolution: {r.epitaph}",
                    new TestSurvivorAuthor("lead_survivor", "Commander", RiskBiasTrait.Realist),
                    r.resolvedDay);
            };

            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_ration_rationing",
                DisplayName = "Ration Distribution",
                Category = "resource",
                Trigger = "Food stores are low.",
                Discovery = "A choice must be made.",
                LocationId = "loc_bunker",
                MinDay = 0,
                MaxDay = 10,
                Choices = new List<MoralChoiceOption>
                {
                    new MoralChoiceOption
                    {
                        Label = "Equal portions for all",
                        MoralDelta = 5,
                        EmpathyDelta = 1,
                        OutcomeText = "Equal portions for all.",
                        Epitaph = "Everyone ate equal crumbs."
                    }
                }
            };

            var resolution = moralSystem.Resolve(quest, choiceIndex: 0, quest.LocationId, day: 3);

            Assert.NotNull(resolution);
            Assert.Equal(1, journal.EntryCount);
            Assert.Equal("quest_moral_ration_rationing", journal.Entries[0].KnowledgeKey);
            Assert.Contains("crumbs", journal.Entries[0].Text);
            Assert.True(journal.HasUnread);
        }

        [Fact]
        public void ProceduralEulogyEngine_ComposesFullMemorial_AndSupportsLosslessSaveRestore()
        {
            var engine = new ProceduralEulogyEngine();
            string lastDwellerId = null;
            string lastEulogyText = null;
            engine.OnEulogySpoken += (id, text) =>
            {
                lastDwellerId = id;
                lastEulogyText = text;
            };

            var record = new DwellerLifeRecord
            {
                dwellerId = "dw_marcus",
                dwellerName = "Marcus Vance",
                preWarProfession = "Locomotive Mechanic",
                daysSurvived = 142,
                shiftsCompleted = 84,
                mealsPrepared = 12,
                radDoseAbsorbedMsv = 450,
                causeOfDeath = "Acute Radiation Sickness",
                favoriteRelicName = "a rusted brass caliper",
                memorableBarkSnippets = new List<string>
                {
                    "Keep the valves greased.",
                    "If the seam leaks, do not look at it."
                }
            };

            string composed = engine.ComposeEulogy(record);

            Assert.NotNull(composed);
            Assert.Equal("dw_marcus", lastDwellerId);
            Assert.Equal(composed, lastEulogyText);
            Assert.Contains("MARCUS VANCE", composed);
            Assert.Contains("Locomotive Mechanic", composed);
            Assert.Contains("142 days", composed);
            Assert.Contains("84 watches", composed);
            Assert.Contains("If the seam leaks, do not look at it.", composed);
            Assert.Contains("a rusted brass caliper", composed);
            Assert.Contains("Acute Radiation Sickness", composed);
            Assert.Single(engine.ArchivedEulogies);

            // Save and restore
            var save = engine.CaptureState();
            Assert.Single(save.archivedEulogyTexts);

            var restored = new ProceduralEulogyEngine();
            int restoreEventsFired = 0;
            restored.OnEulogySpoken += (_, _) => restoreEventsFired++;

            restored.RestoreState(save);

            // Restore suppression: 0 events emitted on restore
            Assert.Equal(0, restoreEventsFired);
            Assert.Single(restored.ArchivedEulogies);
            Assert.Equal(composed, restored.ArchivedEulogies[0]);
        }

        [Fact]
        public void JournalVoice_ToneShiftsWithRiskBias_AndFormatsCleanly()
        {
            var variants = new Dictionary<string, JournalVoiceProseEntry>
            {
                ["k_radiation_alarm"] = new JournalVoiceProseEntry
                {
                    paranoid = "The needle is lying. It is ten times worse than the clicker says.",
                    cautious = "Dosimeter spiked on the eastern perimeter. Double the lead curtains.",
                    realist = "Perimeter radiation increased by 15 mSv. Rotate the guard roster.",
                    reckless = "Just a little static on the tube. The dust will blow south.",
                    denialist = "Old sensors always flicker in high humidity. Nothing to worry about.",
                    fatalist = "The cloud came anyway. It was always going to come.",
                    empath = "Everyone feels the heaviness in the air today.",
                    sociopath = "If someone gets dosed, their rations can be redistributed.",
                    @default = "Radiation levels changed."
                }
            };

            var catalog = new JournalVoiceProseCatalog(variants);
            JournalVoice.BindCatalog(catalog);

            try
            {
                // Verify distinct prose per trait
                string paranoidProse = JournalVoice.ComposeBody("k_radiation_alarm", RiskBiasTrait.Paranoid);
                string realistProse = JournalVoice.ComposeBody("k_radiation_alarm", RiskBiasTrait.Realist);
                string recklessProse = JournalVoice.ComposeBody("k_radiation_alarm", RiskBiasTrait.Reckless);

                Assert.Contains("needle is lying", paranoidProse);
                Assert.Contains("Rotate the guard roster", realistProse);
                Assert.Contains("little static", recklessProse);
                Assert.NotEqual(paranoidProse, realistProse);

                // Verify ComposeFullText prepends Day stamp
                string fullText = JournalVoice.ComposeFullText("k_radiation_alarm", RiskBiasTrait.Realist, day: 12);
                Assert.StartsWith("Day 12. ", fullText);
                Assert.Contains("Rotate the guard roster", fullText);

                // Fallback for unknown key
                string fallback = JournalVoice.ComposeBody("k_completely_unknown_key", RiskBiasTrait.Realist);
                Assert.Equal("Something changed. I wrote it down so I would not forget.", fallback);

                // Timestamp formatting
                Assert.Equal("Day 1", JournalVoice.FormatTimestamp(1, -1f));
                Assert.Equal("Day 7, 08h", JournalVoice.FormatTimestamp(7, 8.2f));
                Assert.Equal("Day 1, 00h", JournalVoice.FormatTimestamp(-3, 0f));
                Assert.Equal("Day 5, 23h", JournalVoice.FormatTimestamp(5, 23f));
            }
            finally
            {
                JournalVoice.BindCatalog(null);
            }
        }
    }
}
