using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class VinylMoraleSystemTests
    {
        [Fact] public void AcquireRecord_AddsToCollection()
        {
            var vm = Create();
            vm.AcquireRecord("record_quartet");
            Assert.Contains("record_quartet", vm.State.ownedRecordIds);
        }

        [Fact] public void Play_OwnedRecord_Succeeds()
        {
            var vm = CreateWithRecords();
            var r = vm.Play("record_quartet");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.True(vm.IsPlaying);
        }

        [Fact] public void Play_NotOwned_Blocks()
        {
            var vm = Create();
            var r = vm.Play("record_quartet");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void Stop_StopsPlayback()
        {
            var vm = CreateWithRecords();
            vm.Play("record_quartet");
            var r = vm.Stop();
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.False(vm.IsPlaying);
        }

        [Fact] public void ApplyDailyEffect_AppliesMorale()
        {
            var vm = CreateWithRecords();
            vm.Play("record_quartet");
            float morale = 0;
            vm.OnMoraleApplied += (m) => morale = m;
            vm.ApplyDailyEffect(1);
            Assert.True(morale > 0);
            Assert.Equal(1, vm.State.totalPlays);
        }

        [Fact] public void ApplyDailyEffect_OncePerDay()
        {
            var vm = CreateWithRecords();
            vm.Play("record_quartet");
            vm.ApplyDailyEffect(1);
            vm.ApplyDailyEffect(1); // same day
            Assert.Equal(1, vm.State.totalPlays);
        }

        [Fact] public void ApplyDailyEffect_NewDay_AppliesAgain()
        {
            var vm = CreateWithRecords();
            vm.Play("record_quartet");
            vm.ApplyDailyEffect(1);
            vm.ApplyDailyEffect(2); // next day
            Assert.Equal(2, vm.State.totalPlays);
        }

        [Fact] public void CaptureRestoreState_PreservesCollection()
        {
            var vm = CreateWithRecords();
            var state = vm.CaptureState();
            Assert.Contains("record_quartet", state.ownedRecordIds);

            var vm2 = Create();
            vm2.LoadCatalog(MakeRecords());
            vm2.RestoreState(state);
            Assert.Contains("record_quartet", vm2.State.ownedRecordIds);
        }

        [Fact]
        public void VinylArchive_Loads30Records_WithDistinctMoraleEffects()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);

            string path = System.IO.Path.Combine(dataDir, "narrative", "vinyl_record_archive.json");
            Assert.True(System.IO.File.Exists(path), $"vinyl_record_archive.json not found at {path}");

            string json = System.IO.File.ReadAllText(path);
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var recordsArray = doc.RootElement.GetProperty("records");
            Assert.Equal(30, recordsArray.GetArrayLength());

            var defs = new System.Collections.Generic.List<VinylRecordDefinition>();
            foreach (var r in recordsArray.EnumerateArray())
            {
                string rid = r.GetProperty("record_id").GetString()!;
                string title = r.GetProperty("title").GetString()!;
                float morale = r.GetProperty("daily_morale_modifier").GetSingle();
                string genre = r.GetProperty("tags")[0].GetString()!;
                defs.Add(new VinylRecordDefinition
                {
                    record_id = rid,
                    display_name = title,
                    genre = genre,
                    morale_daily_bonus = morale
                });
            }

            var vm = Create();
            vm.LoadCatalog(defs);

            // Verify record 1 (morale = 6) vs record 3 (morale = 8)
            vm.AcquireRecord("record_01_valse_triste_sibelius_78rpm");
            vm.AcquireRecord("record_03_rachmaninoff_piano_concerto_2_adagio");

            vm.Play("record_01_valse_triste_sibelius_78rpm");
            float appliedMorale1 = 0;
            vm.OnMoraleApplied += m => appliedMorale1 = m;
            vm.ApplyDailyEffect(1);
            Assert.Equal(6f, appliedMorale1);

            vm.Play("record_03_rachmaninoff_piano_concerto_2_adagio");
            float appliedMorale3 = 0;
            vm.OnMoraleApplied += m => appliedMorale3 = m;
            vm.ApplyDailyEffect(2);
            Assert.Equal(8f, appliedMorale3);
        }

        private static VinylMoraleSystem CreateWithRecords()
        {
            var vm = Create();
            vm.LoadCatalog(MakeRecords());
            vm.AcquireRecord("record_quartet");
            return vm;
        }

        private static VinylMoraleSystem Create() => new VinylMoraleSystem();

        private static System.Collections.Generic.List<VinylRecordDefinition> MakeRecords()
        {
            return new System.Collections.Generic.List<VinylRecordDefinition>
            {
                new VinylRecordDefinition
                {
                    record_id = "record_quartet", display_name = "String Quartet No. 14",
                    genre = "classical", morale_daily_bonus = 3f, flashback_suppression = 0.2f
                }
            };
        }
    }
}
