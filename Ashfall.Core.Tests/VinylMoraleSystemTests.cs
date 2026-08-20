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
