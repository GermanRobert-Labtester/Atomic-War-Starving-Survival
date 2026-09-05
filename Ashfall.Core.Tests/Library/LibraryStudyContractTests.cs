using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Library
{
    public sealed class LibraryStudyContractTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Data directory not found from " + start);
        }

        private static LibraryStudySystem CreateSystem(
            out SkillProgressionSystem skills,
            out ResearchSystem research,
            out JournalSystem journal,
            out DutyRosterSystem roster)
        {
            skills = new SkillProgressionSystem();
            research = new ResearchSystem();
            journal = new JournalSystem();
            roster = new DutyRosterSystem();
            return new LibraryStudySystem(skills, research, journal, roster);
        }

        [Fact]
        public void B2_001_ManualStudy_CallsUnlockManual_NeverCompleteResearch()
        {
            var sys = CreateSystem(out _, out var research, out _, out _);
            var kdef = new ResearchKnowledgeDef
            {
                id = "knowledge_water_basics",
                displayName = "Water Purification",
                daysToComplete = 5,
                isCompleted = false,
                isUnlocked = false
            };
            research.Register(kdef);

            sys.LoadCatalog(new List<ManualDefinition>
            {
                new ManualDefinition
                {
                    manual_id = "manual_water_filtration",
                    display_name = "Field Water Filtration",
                    category = "survival",
                    studyHoursRequired = 8,
                    researchUnlocks = new List<string> { "knowledge_water_basics" },
                    knowledgeUnlocks = new List<string> { "knowledge_water_basics" }
                }
            });

            var startRes = sys.StartStudy("manual_water_filtration", "survivor_1");
            Assert.True(startRes.IsSuccess);

            // Tick day to complete manual study
            sys.TickDay(1);

            Assert.True(sys.IsManualCompleted("manual_water_filtration"));
            // Architectural invariant: Node is unlocked/revealed, NEVER completed
            Assert.True(research.IsManualUnlocked("knowledge_water_basics"));
            var node = research.GetKnowledge("knowledge_water_basics");
            Assert.NotNull(node);
            Assert.True(node.isUnlocked);
            Assert.False(node.isCompleted); // Must NOT be completed!
        }

        [Fact]
        public void B2_002_And_B2_004_JournalEvidenceAddedAndDedupedWithStableProvenance()
        {
            var sys = CreateSystem(out _, out _, out var journal, out _);
            sys.LoadCatalog(new List<ManualDefinition>
            {
                new ManualDefinition
                {
                    manual_id = "manual_test_evidence",
                    display_name = "Evidence Manual",
                    category = "survival",
                    studyHoursRequired = 8,
                    knowledgeUnlocks = new List<string> { "knowledge_water_basics" }
                }
            });

            sys.StartStudy("manual_test_evidence", "survivor_1");
            sys.TickDay(1);

            // Knowledge key registered in journal knowledge base
            Assert.True(journal.Knowledge.Has("knowledge_water_basics"));

            // Adding same evidence again is idempotent
            int countBefore = journal.Knowledge.Count;
            journal.AddKnowledgeEvidence("survivor_1", "knowledge_water_basics");
            Assert.Equal(countBefore, journal.Knowledge.Count);
        }

        [Fact]
        public void B2_003_DuplicateResearchUnlock_IsIdempotent()
        {
            var sys = CreateSystem(out _, out var research, out _, out _);
            var kdef = new ResearchKnowledgeDef
            {
                id = "knowledge_radio_basics",
                displayName = "Radio Basics",
                daysToComplete = 5
            };
            research.Register(kdef);

            sys.LoadCatalog(new List<ManualDefinition>
            {
                new ManualDefinition
                {
                    manual_id = "manual_radio_1",
                    display_name = "Radio Primer",
                    category = "science",
                    studyHoursRequired = 8,
                    researchUnlocks = new List<string> { "knowledge_radio_basics" }
                },
                new ManualDefinition
                {
                    manual_id = "manual_radio_2",
                    display_name = "Radio Handbook",
                    category = "science",
                    studyHoursRequired = 8,
                    researchUnlocks = new List<string> { "knowledge_radio_basics" }
                }
            });

            sys.StartStudy("manual_radio_1", "survivor_1");
            sys.TickDay(1);

            int unlockCount = research.State.unlockedIds.Count;
            Assert.Contains("knowledge_radio_basics", research.State.unlockedIds);

            // Complete second manual revealing same research
            sys.StartStudy("manual_radio_2", "survivor_1");
            sys.TickDay(2);

            // Count of unlocked IDs should not duplicate
            Assert.Equal(unlockCount, research.State.unlockedIds.Count);
        }

        [Fact]
        public void B2_005_And_B2_006_SkillRaisesStudyRateMonotonically_WithinStrictBounds()
        {
            var sys = CreateSystem(out var skills, out _, out _, out _);
            sys.LoadCatalog(new List<ManualDefinition>
            {
                new ManualDefinition
                {
                    manual_id = "manual_med_test",
                    display_name = "Medical Manual",
                    category = "medical",
                    studyHoursRequired = 20
                }
            });

            // Register medical skill
            skills.RegisterSkill(new SkillDef
            {
                id = "skill_field_dressing",
                disciplineId = "medical",
                xpThreshold = 50f,
                skillBonus = 0.20f
            });

            string noviceId = "novice_reader";
            string skilledId = "skilled_reader";
            string masterId = "master_reader";

            var actorNovice = new SimpleSkillActor(noviceId);
            var actorSkilled = new SimpleSkillActor(skilledId);
            var actorMaster = new SimpleSkillActor(masterId);

            skills.RecordAction(actorNovice, "medical", 0f, 1);
            skills.RecordAction(actorSkilled, "medical", 50f, 1);
            skills.RecordAction(actorMaster, "medical", 500f, 1);

            float rateNovice = sys.GetComprehensionRate(noviceId, "manual_med_test");
            float rateSkilled = sys.GetComprehensionRate(skilledId, "manual_med_test");
            float rateMaster = sys.GetComprehensionRate(masterId, "manual_med_test");

            // Monotonic: Novice <= Skilled <= Master
            Assert.True(rateNovice <= rateSkilled);
            Assert.True(rateSkilled <= rateMaster);

            // Bounds enforced: min 0.75, max 2.0
            Assert.InRange(rateNovice, 0.75f, 2.0f);
            Assert.InRange(rateSkilled, 0.75f, 2.0f);
            Assert.InRange(rateMaster, 0.75f, 2.0f);
        }

        [Fact]
        public void B2_007_InvalidZeroOrNegativeHours_Rejected()
        {
            var sys = CreateSystem(out _, out _, out _, out _);
            sys.LoadCatalog(new List<ManualDefinition>
            {
                new ManualDefinition
                {
                    manual_id = "manual_zero_hours",
                    display_name = "Broken Manual",
                    studyHoursRequired = 0
                },
                new ManualDefinition
                {
                    manual_id = "manual_neg_hours",
                    display_name = "Broken Negative Manual",
                    studyHoursRequired = -5
                }
            });

            var res1 = sys.StartStudy("manual_zero_hours", "survivor_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, res1.Status);
            Assert.Equal("invalid_hours", res1.FailureCode);

            var res2 = sys.StartStudy("manual_neg_hours", "survivor_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, res2.Status);
            Assert.Equal("invalid_hours", res2.FailureCode);
        }

        [Fact]
        public void B2_008_And_B2_009_BidirectionalAvailabilityReservation_DutyRoster()
        {
            var sys = CreateSystem(out _, out _, out _, out var roster);
            roster.Unlock(1);
            roster.WriteName("survivor_busy", "Busy Survivor", "Worker", DutyRosterIds.ScriptPencil, 1, true);
            roster.WriteName("survivor_free", "Free Survivor", "Idle", DutyRosterIds.ScriptPencil, 1, true);

            // Assign survivor_busy to duty roster role
            roster.Assign(DutyRosterIds.RoleHatchOpener, "survivor_busy");
            Assert.Equal(DutyRosterIds.RoleHatchOpener, roster.GetRoleOf("survivor_busy"));

            sys.LoadCatalog(new List<ManualDefinition>
            {
                new ManualDefinition
                {
                    manual_id = "manual_guard",
                    display_name = "Guard Manual",
                    category = "combat",
                    studyHoursRequired = 10
                }
            });

            // B2-008: Reader already on duty roster is blocked from starting study
            var studyRes = sys.StartStudy("manual_guard", "survivor_busy");
            Assert.Equal(ActionResult.StatusKind.Blocked, studyRes.Status);
            Assert.Equal("busy", studyRes.FailureCode);

            // Start study with free survivor
            var freeStudyRes = sys.StartStudy("manual_guard", "survivor_free");
            Assert.True(freeStudyRes.IsSuccess);
            Assert.True(sys.IsReaderStudying("survivor_free"));

            // B2-009: Active reader cannot be assigned to duty roster while studying
            var assignRes = roster.AssignWithResult(DutyRosterIds.RoleNightWatch, "survivor_free");
            Assert.Equal(ActionResult.StatusKind.Blocked, assignRes.Status);
            Assert.Equal("busy", assignRes.FailureCode);

            // Cancel study -> reservation released
            var job = sys.GetActiveJobs().First(j => j.readerId == "survivor_free");
            sys.CancelStudy(job.jobId);
            Assert.False(sys.IsReaderStudying("survivor_free"));

            // Now duty roster assignment succeeds
            var reassignRes = roster.AssignWithResult(DutyRosterIds.RoleNightWatch, "survivor_free");
            Assert.True(reassignRes.IsSuccess);
        }

        [Fact]
        public void B2_010_To_B2_014_AuthoritativeCatalogIntegrity_24Manuals_6Disciplines()
        {
            string dataDir = FindDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var manuals = LibraryManualCatalogLoader.Load(dataDir, fileIO, json);
            Assert.NotNull(manuals);

            // B2-010: >= 24 manuals target
            Assert.True(manuals.Count >= 24, $"Expected >= 24 manuals, got {manuals.Count}");

            // B2-011: Six disciplines represented
            var disciplines = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var m in manuals)
            {
                disciplines.Add(LibraryStudySystem.NormalizeDiscipline(m.category));
            }

            string[] requiredDisciplines = { "survival", "crafting", "medical", "science", "scavenging", "combat" };
            foreach (var req in requiredDisciplines)
            {
                Assert.Contains(req, disciplines);
            }

            // Load research knowledge to verify references
            string kPath = Path.Combine(dataDir, "research_knowledge.json");
            Assert.True(File.Exists(kPath));
            var kData = json.Deserialize<ResearchCatalogContainer>(File.ReadAllText(kPath));
            var knownKnowledgeIds = new HashSet<string>(kData.knowledge_nodes.Select(k => k.id), StringComparer.Ordinal);

            // Verify each manual
            var manualIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var m in manuals)
            {
                Assert.False(string.IsNullOrEmpty(m.manual_id));
                Assert.True(manualIds.Add(m.manual_id), $"Duplicate manual ID: {m.manual_id}");
                Assert.True(m.studyHoursRequired > 0, $"Manual {m.manual_id} has invalid study hours: {m.studyHoursRequired}");

                // B2-012: all research/knowledge refs resolve
                foreach (var r in m.researchUnlocks)
                {
                    Assert.Contains(r, knownKnowledgeIds);
                }
                foreach (var k in m.knowledgeUnlocks)
                {
                    Assert.Contains(k, knownKnowledgeIds);
                }

                // B2-013 & B2-014: every manual has at least one acquisition path
                bool hasAcquisition = m.lootTableIds.Count > 0 ||
                                      m.expeditionRewardIds.Count > 0 ||
                                      m.traderPoolIds.Count > 0 ||
                                      !string.IsNullOrEmpty(m.archiveScribingRecipeId) ||
                                      m.startingOriginIds.Count > 0 ||
                                      !string.IsNullOrEmpty(m.originFacility);
                Assert.True(hasAcquisition, $"Manual {m.manual_id} lacks structured acquisition metadata");
            }

            // Verify prerequisites resolve
            foreach (var m in manuals)
            {
                foreach (var p in m.prerequisites)
                {
                    Assert.Contains(p, manualIds);
                }
            }
        }

        [Fact]
        public void B2_016_And_B2_017_SaveRestore_PreservesJobsAndUnknownCompletedIds()
        {
            var sys1 = CreateSystem(out _, out _, out _, out _);
            sys1.LoadCatalog(new List<ManualDefinition>
            {
                new ManualDefinition
                {
                    manual_id = "manual_known",
                    display_name = "Known Manual",
                    category = "survival",
                    studyHoursRequired = 16
                }
            });

            sys1.StartStudy("manual_known", "survivor_1");
            sys1.TickDay(1);

            // Inject historical/unknown manual ID into state
            sys1.State.completedManualIds.Add("manual_historical_unknown_v1");

            var saved = sys1.CaptureState();

            // Create new system and restore
            var sys2 = CreateSystem(out _, out _, out _, out _);
            sys2.LoadCatalog(new List<ManualDefinition>
            {
                new ManualDefinition
                {
                    manual_id = "manual_known",
                    display_name = "Known Manual",
                    category = "survival",
                    studyHoursRequired = 16
                }
            });

            sys2.RestoreState(saved);

            // B2-016: Preserves active jobs
            var active = sys2.GetActiveJobs();
            Assert.Single(active);
            Assert.Equal("manual_known", active[0].manualId);
            Assert.Equal(8f, active[0].progressHours, 1);

            // B2-017: Preserves unknown completed manual ID
            Assert.True(sys2.IsManualCompleted("manual_historical_unknown_v1"));
        }

        private sealed class ResearchCatalogContainer
        {
            public List<ResearchKnowledgeDef> knowledge_nodes { get; set; } = new List<ResearchKnowledgeDef>();
        }
    }
}
