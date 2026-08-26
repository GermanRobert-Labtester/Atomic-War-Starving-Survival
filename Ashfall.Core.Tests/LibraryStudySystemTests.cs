using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class LibraryStudySystemTests
    {
        [Fact] public void StartStudy_WithoutPrereq_Blocks()
        {
            var lib = Create(out _, out _, out _, out _);
            lib.LoadCatalog(new System.Collections.Generic.List<ManualDefinition>
            {
                new ManualDefinition { manual_id = "man_advanced", display_name = "Advanced Tech", prerequisites = new System.Collections.Generic.List<string> { "man_basic" } }
            });
            var r = lib.StartStudy("man_advanced", "survivor_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void StartStudy_WithPrereq_StartsJob()
        {
            var lib = Create(out _, out _, out _, out _);
            lib.LoadCatalog(new System.Collections.Generic.List<ManualDefinition>
            {
                new ManualDefinition { manual_id = "man_basic", display_name = "Basic Tech", studyHoursRequired = 5 }
            });
            lib.StartStudy("man_basic", "survivor_1");
            Assert.Single(lib.State.activeJobs);
        }

        [Fact] public void TickDay_CompletesStudy()
        {
            var lib = Create(out _, out _, out _, out _);
            lib.LoadCatalog(new System.Collections.Generic.List<ManualDefinition>
            {
                new ManualDefinition { manual_id = "man_basic", display_name = "Basic Tech", studyHoursRequired = 5, skillXpGrants = new System.Collections.Generic.List<string> { "skill_engineering", "10" } }
            });
            lib.StartStudy("man_basic", "survivor_1");
            lib.TickDay(1);
            Assert.True(lib.State.activeJobs[0].isComplete);
            Assert.Contains("man_basic", lib.State.completedManualIds);
        }

        [Fact] public void CompleteStudy_UnlocksResearch()
        {
            var lib = Create(out _, out var research, out _, out _);
            lib.LoadCatalog(new System.Collections.Generic.List<ManualDefinition>
            {
                new ManualDefinition { manual_id = "man_basic", display_name = "Basic Tech", studyHoursRequired = 5, researchUnlocks = new System.Collections.Generic.List<string> { "tech_water_purifier" } }
            });
            lib.StartStudy("man_basic", "survivor_1");
            lib.TickDay(1);
            Assert.True(research.IsManualUnlocked("tech_water_purifier"));
        }

        [Fact] public void StartStudy_AlreadyCompleted_Blocks()
        {
            var lib = Create(out _, out _, out _, out _);
            lib.LoadCatalog(new System.Collections.Generic.List<ManualDefinition>
            {
                new ManualDefinition { manual_id = "man_basic", display_name = "Basic Tech", studyHoursRequired = 5 }
            });
            lib.StartStudy("man_basic", "survivor_1");
            lib.TickDay(1);
            var r = lib.StartStudy("man_basic", "survivor_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void LoadCatalog_OddLengthSkillGrantList_Throws()
        {
            // Bug-10 regression: a manual with an odd number of skillXpGrants
            // entries would crash the tick loop with IndexOutOfRange when the
            // reader advances 'i' by 2 and reads '[i+1]'. The catalog loader
            // must surface this as invalid before a TickDay ever sees it.
            var lib = Create(out _, out _, out _, out _);
            var bad = new ManualDefinition
            {
                manual_id = "man_bad",
                display_name = "Bad Manual",
                studyHoursRequired = 1,
                skillXpGrants = new System.Collections.Generic.List<string> { "skill_engineering", "10", "orphan" }
            };
            Assert.Throws<System.IO.InvalidDataException>(() =>
                lib.LoadCatalog(new System.Collections.Generic.List<ManualDefinition> { bad }));
        }

        [Fact] public void StartStudy_ZeroStudyHours_Blocks()
        {
            // Bug-15b regression: a manual with studyHoursRequired == 0 (or
            // negative) would complete instantly on TickDay, granting all XP,
            // research unlocks, and knowledge evidence in zero days. The start
            // path must reject such manuals as malformed before they reach the
            // tick loop. The constructor default is 10; an author who overrides
            // it to 0 is setting a trap.
            var lib = Create(out _, out _, out _, out _);
            lib.LoadCatalog(new System.Collections.Generic.List<ManualDefinition>
            {
                new ManualDefinition
                {
                    manual_id = "man_freebie",
                    display_name = "Free Magic",
                    studyHoursRequired = 0
                }
            });
            var r = lib.StartStudy("man_freebie", "survivor_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            Assert.Empty(lib.State.activeJobs);
        }

        [Fact] public void CaptureRestoreState_PreservesJobs()
        {
            var lib = Create(out _, out _, out _, out _);
            lib.LoadCatalog(new System.Collections.Generic.List<ManualDefinition>
            {
                new ManualDefinition { manual_id = "man_basic", display_name = "Basic Tech", studyHoursRequired = 5 }
            });
            lib.StartStudy("man_basic", "survivor_1");
            var state = lib.CaptureState();
            Assert.Single(state.activeJobs);

            var lib2 = Create(out _, out _, out _, out _);
            lib2.LoadCatalog(new System.Collections.Generic.List<ManualDefinition>
            {
                new ManualDefinition { manual_id = "man_basic", display_name = "Basic Tech", studyHoursRequired = 5 }
            });
            lib2.RestoreState(state);
            Assert.Single(lib2.State.activeJobs);
        }

        private static LibraryStudySystem Create(out SkillProgressionSystem skills, out ResearchSystem research, out JournalSystem journal, out DutyRosterSystem roster)
        {
            skills = new SkillProgressionSystem();
            research = new ResearchSystem();
            journal = new JournalSystem();
            roster = new DutyRosterSystem();
            return new LibraryStudySystem(skills, research, journal, roster);
        }
    }
}
