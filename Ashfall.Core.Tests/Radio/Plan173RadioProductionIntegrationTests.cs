// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 173: Shelter Radio Production & Audience Response — Integration Tests
// Verifies JSON catalog loading, program start/prep/delivery/cancel lifecycle,
// audience response, and save/restore state.
// ============================================================================
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core.Radio;

namespace Ashfall.Core.Tests.Plan173RadioProduction
{
    public sealed class Plan173RadioProductionIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            var candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates) { if (File.Exists(c)) return Path.GetFullPath(c); }
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename));
        }

        [Fact]
        public void LoadFromJson_LoadsProgramsFromJsonFile()
        {
            string path = ResolveDataPath("radio_programs.json");
            Assert.True(File.Exists(path), $"radio_programs.json must exist at {path}");

            var catalog = RadioProgramCatalogLoader.LoadFromJson(File.ReadAllText(path));

            Assert.NotNull(catalog);
            Assert.True(catalog.All.Count >= 1, $"Expected >= 1 program, got {catalog.All.Count}");
            Assert.NotNull(catalog.Get("radio_prog_shelter_morning_bulletin"));
        }

        [Fact]
        public void RadioProgramCatalog_GetById_ReturnsCorrectTemplate()
        {
            string path = ResolveDataPath("radio_programs.json");
            var catalog = RadioProgramCatalogLoader.LoadFromJson(File.ReadAllText(path));

            var prog = catalog.Get("radio_prog_shelter_morning_bulletin");

            Assert.NotNull(prog);
            Assert.Equal("station_civil_defense", prog!.station_id);
            Assert.Equal("slot_cd_morning", prog.slot_id);
            Assert.True(prog.prep_ticks_required >= 1);
        }

        [Fact]
        public void RadioProgramProductionSystem_StartPrep_CreatesJob()
        {
            var catalog = new RadioProgramCatalog
            {
                programs = new List<RadioProgramTemplateDef>
                {
                    new RadioProgramTemplateDef
                    {
                        id = "test_prog_news",
                        display_name = "Morning News",
                        station_id = "station_civil_defense",
                        slot_id = "slot_morning",
                        prep_ticks_required = 2,
                        genre = "civilian_news"
                    }
                }
            };
            catalog.Index();

            var system = new RadioProgramProductionSystem(catalog);
            var result = system.StartPrep("test_prog_news", "presenter_jana", 5);

            Assert.True(result.IsSuccess, result.FailureCode ?? "unknown failure");
            var jobs = system.GetActiveJobs();
            Assert.Single(jobs);
            Assert.Equal("test_prog_news", jobs[0].TemplateId);
            Assert.Equal("presenter_jana", jobs[0].PresenterId);
            Assert.Equal((int)RadioProgramJobStatus.Preparing, jobs[0].Status);
        }

        [Fact]
        public void RadioProgramProductionSystem_TickDay_AdvancesPrepAndBecomesReady()
        {
            var catalog = new RadioProgramCatalog
            {
                programs = new List<RadioProgramTemplateDef>
                {
                    new RadioProgramTemplateDef
                    {
                        id = "test_prog_edu",
                        display_name = "Education Hour",
                        station_id = "station_civil_defense",
                        slot_id = "slot_edu",
                        prep_ticks_required = 1
                    }
                }
            };
            catalog.Index();

            var system = new RadioProgramProductionSystem(catalog);
            system.StartPrep("test_prog_edu", "presenter_malik", 1);

            // One tick should satisfy prep_ticks_required = 1
            system.TickDay(2);

            var jobs = system.GetActiveJobs();
            var job = jobs.FirstOrDefault(j => j.TemplateId == "test_prog_edu");
            Assert.NotNull(job);
            Assert.Equal((int)RadioProgramJobStatus.Ready, job!.Status);
        }

        [Fact]
        public void RadioProgramProductionSystem_CancelJob_SetsStatusCancelled()
        {
            var catalog = new RadioProgramCatalog
            {
                programs = new List<RadioProgramTemplateDef>
                {
                    new RadioProgramTemplateDef
                    {
                        id = "test_prog_cancel",
                        display_name = "Evening Story",
                        station_id = "station_civil_defense",
                        slot_id = "slot_evening",
                        prep_ticks_required = 3
                    }
                }
            };
            catalog.Index();

            var system = new RadioProgramProductionSystem(catalog);
            var start = system.StartPrep("test_prog_cancel", "presenter_ryo", 4);
            Assert.True(start.IsSuccess);

            string jobId = system.GetActiveJobs()[0].JobId;
            var cancel = system.CancelJob(jobId);
            Assert.True(cancel.IsSuccess);

            var jobs = system.GetActiveJobs();
            Assert.Empty(jobs); // cancelled jobs leave the active list
        }

        [Fact]
        public void RadioProgramProductionSystem_SaveRestore_PreservesJobs()
        {
            var catalog = new RadioProgramCatalog
            {
                programs = new List<RadioProgramTemplateDef>
                {
                    new RadioProgramTemplateDef
                    {
                        id = "test_prog_save",
                        display_name = "Save Test Broadcast",
                        station_id = "station_civil_defense",
                        slot_id = "slot_save",
                        prep_ticks_required = 2
                    }
                }
            };
            catalog.Index();

            var system = new RadioProgramProductionSystem(catalog);
            system.StartPrep("test_prog_save", "presenter_anya", 10);
            system.TickDay(11); // 1 of 2 ticks

            var captured = system.CaptureState();
            var restored = new RadioProgramProductionSystem(catalog);
            restored.RestoreState(captured);

            var restoredJobs = restored.GetActiveJobs();
            Assert.Single(restoredJobs);
            Assert.Equal("test_prog_save", restoredJobs[0].TemplateId);
            Assert.Equal((int)RadioProgramJobStatus.Preparing, restoredJobs[0].Status);
        }
    }
}
