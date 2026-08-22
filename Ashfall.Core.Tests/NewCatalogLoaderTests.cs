// BUG-02 regression tests: the 4 newly-authored catalog loaders
// (ShelterSchedule, Autopsy, LibraryStudy, ArchiveDesk) must load real
// JSON from StreamingAssets/Data and register into their Core systems.
// Pattern follows BlackFlotillaTests loader tests: locate data dir via
// CatalogLocator, use FileSystemIO + SystemTextJsonSerializer ports.
#nullable disable

using System;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.Radiation;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Medical;
using Ashfall.Core.Survivors;
using Ashfall.Core.Journal;

namespace Ashfall.Core.Tests
{
    public class ShelterScheduleCatalogLoaderTests
    {
        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        [Fact]
        public void LoadsThreeSchedules_FromRealJson()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var defs = ShelterScheduleCatalogLoader.Load(dataDir, io, json);
            Assert.True(defs.Count >= 3, $"expected >= 3 schedules, got {defs.Count}");
            Assert.Contains(defs, d => d.schedule_id == "schedule_standard");
            Assert.Contains(defs, d => d.schedule_id == "schedule_night_shift");
            Assert.Contains(defs, d => d.schedule_id == "schedule_curfew_locked");
        }

        [Fact]
        public void LoadAndRegister_PopulatesCoreCatalog()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var state = new PowerGridState { GenerationWatts = 800, FuelUnits = 100, BatteryCapacityWh = 4000, BatteryReserveWh = 2000 };
            var rooms = new System.Collections.Generic.List<PowerGridRoom> { new PowerGridRoom("room_main", "Main Vault", 100f) };
            var grid = new PowerGridSystem(state, rooms, new SeededRng(42));
            var system = new ShelterScheduleSystem(grid);

            int count = ShelterScheduleCatalogLoader.LoadAndRegister(system, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(count >= 3, $"expected >= 3 loaded, got {count}");

            // The default baked-in schedule is always present; the catalog adds more.
            var setRes = system.SetSchedule("schedule_standard");
            Assert.True(setRes.IsSuccess, "SetSchedule should succeed after catalog load");
        }

        [Fact]
        public void ReturnsEmpty_WhenMissing()
        {
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var result = ShelterScheduleCatalogLoader.Load("/nonexistent", io, json);
            Assert.Empty(result);
        }
    }

    public class AutopsyProcedureCatalogLoaderTests
    {
        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        [Fact]
        public void LoadsThreeProcedures_FromRealJson()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var defs = AutopsyProcedureCatalogLoader.Load(dataDir, io, json);
            Assert.True(defs.Count >= 3, $"expected >= 3 procedures, got {defs.Count}");
            Assert.Contains(defs, d => d.procedure_id == "procedure_rad_pathology");
            Assert.Contains(defs, d => d.procedure_id == "procedure_toxicology");
            Assert.Contains(defs, d => d.procedure_id == "procedure_containment_autopsy");
        }

        [Fact]
        public void LoadAndRegister_PopulatesCoreCatalog()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var inv = new Ashfall.Core.Inventory.Inventory();
            var rad = new RadiationSystem(seed: 42);
            var starting = new StartingLevelSystem();
            var vent = new VentilationSystem(starting);
            var res = new ResearchSystem();
            var wardState = new MedicalWardState();
            var bed = new MedicalBed("bed_1", "Bed 1", MedicalBedCategory.General);
            var proc = new MedicalProcedureDef("proc_1", "Procedure 1", "MedicalSystem");
            var medical = new MedicalWardSystem(wardState, new[] { bed }, new[] { proc });
            var system = new AutopsySystem(new SeededRng(42), inv, rad, vent, res, medical);



            int count = AutopsyProcedureCatalogLoader.LoadAndRegister(system, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(count >= 3, $"expected >= 3 loaded, got {count}");
        }

        [Fact]
        public void ReturnsEmpty_WhenMissing()
        {
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var result = AutopsyProcedureCatalogLoader.Load("/nonexistent", io, json);
            Assert.Empty(result);
        }
    }

    public class LibraryManualCatalogLoaderTests
    {
        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        [Fact]
        public void LoadsThreeManuals_FromRealJson()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var defs = LibraryManualCatalogLoader.Load(dataDir, io, json);
            Assert.True(defs.Count >= 3, $"expected >= 3 manuals, got {defs.Count}");
            Assert.Contains(defs, d => d.manual_id == "manual_water_filtration");
            Assert.Contains(defs, d => d.manual_id == "manual_rad_first_aid");
            Assert.Contains(defs, d => d.manual_id == "manual_improvised_weapons");
        }

        [Fact]
        public void LoadAndRegister_PopulatesCoreCatalog()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var skills = new SkillProgressionSystem();
            var research = new ResearchSystem();
            var journal = new JournalSystem();
            var roster = new DutyRosterSystem();
            var system = new LibraryStudySystem(skills, research, journal, roster);

            int count = LibraryManualCatalogLoader.LoadAndRegister(system, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(count >= 3, $"expected >= 3 loaded, got {count}");

            var startRes = system.StartStudy("manual_water_filtration", "reader_1");
            Assert.True(startRes.IsSuccess, "StartStudy should succeed after catalog load");
        }

        [Fact]
        public void ReturnsEmpty_WhenMissing()
        {
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var result = LibraryManualCatalogLoader.Load("/nonexistent", io, json);
            Assert.Empty(result);
        }
    }

    public class ArchiveInkCatalogLoaderTests
    {
        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        [Fact]
        public void LoadsThreeInks_FromRealJson()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var defs = ArchiveInkCatalogLoader.Load(dataDir, io, json);
            Assert.True(defs.Count >= 3, $"expected >= 3 inks, got {defs.Count}");
            Assert.Contains(defs, d => d.ink_id == "ink_iron_gall");
            Assert.Contains(defs, d => d.ink_id == "ink_soot_lamp");
            Assert.Contains(defs, d => d.ink_id == "ink_plant_dye");
        }

        [Fact]
        public void LoadAndRegister_PopulatesCoreCatalog()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var journal = new JournalSystem();
            var knowledge = new KnowledgeBase();
            var inv = new Ashfall.Core.Inventory.Inventory();
            var roster = new DutyRosterSystem();
            var system = new ArchiveDeskSystem(journal, knowledge, inv, roster);

            int count = ArchiveInkCatalogLoader.LoadAndRegister(system, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(count >= 3, $"expected >= 3 loaded, got {count}");
        }

        [Fact]
        public void ReturnsEmpty_WhenMissing()
        {
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var result = ArchiveInkCatalogLoader.Load("/nonexistent", io, json);
            Assert.Empty(result);
        }
    }
}
