using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class AudioConditionSystemTests
    {
        [Fact] public void StartCondition_ValidBus_Succeeds()
        {
            var ac = Create();
            var r = ac.StartCondition("gen_hum", "generator", "shelter_generator");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(ac.State.activeConditions);
        }

        [Fact] public void StartCondition_InvalidBus_Blocks()
        {
            var ac = Create();
            var r = ac.StartCondition("bad", "invalid_bus", "shelter_generator");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void StartCondition_DuplicateId_Blocks()
        {
            var ac = Create();
            ac.StartCondition("gen_hum", "generator", "shelter_generator");
            var r = ac.StartCondition("gen_hum", "generator", "shelter_generator");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void StopCondition_RemovesActive()
        {
            var ac = Create();
            ac.StartCondition("gen_hum", "generator", "shelter_generator");
            var r = ac.StopCondition("gen_hum");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.False(ac.State.activeConditions[0].isActive);
        }

        [Fact] public void StopCondition_NotActive_Blocks()
        {
            var ac = Create();
            var r = ac.StopCondition("nonexistent");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void SetIntensity_UpdatesValue()
        {
            var ac = Create();
            ac.StartCondition("vent_hum", "ventilation", "shelter_ventilation");
            ac.SetIntensity("vent_hum", 0.5f);
            Assert.Equal(0.5f, ac.State.activeConditions[0].intensity);
        }

        [Fact] public void SetIntensity_ClampsToRange()
        {
            var ac = Create();
            ac.StartCondition("vent_hum", "ventilation", "shelter_ventilation");
            ac.SetIntensity("vent_hum", 1.5f);
            Assert.Equal(1f, ac.State.activeConditions[0].intensity);
        }

        [Fact] public void GetActiveConditionsForBus_FiltersByBus()
        {
            var ac = Create();
            ac.StartCondition("gen1", "generator", "shelter_generator");
            ac.StartCondition("vent1", "ventilation", "shelter_ventilation");
            ac.StartCondition("gen2", "generator", "shelter_generator");
            var genConditions = ac.GetActiveConditionsForBus("generator");
            Assert.Equal(2, genConditions.Count);
        }

        [Fact] public void ClearStopped_RemovesInactive()
        {
            var ac = Create();
            ac.StartCondition("gen_hum", "generator", "shelter_generator");
            ac.StopCondition("gen_hum");
            ac.ClearStopped();
            Assert.Empty(ac.State.activeConditions);
        }

        [Fact] public void CaptureRestoreState_PreservesConditions()
        {
            var ac = Create();
            ac.StartCondition("gen_hum", "generator", "shelter_generator", intensity: 0.7f);
            var state = ac.CaptureState();
            Assert.Single(state.activeConditions);

            var ac2 = Create();
            ac2.RestoreState(state);
            Assert.Single(ac2.State.activeConditions);
            Assert.Equal(0.7f, ac2.State.activeConditions[0].intensity);
        }

        [Fact] public void AllBatch2Buses_AreValid()
        {
            var ac = Create();
            var buses = new[] { "generator", "ventilation", "radio", "medical", "surface", "ambient", "music", "sfx", "ui", "alerts", "voice" };
            foreach (var bus in buses)
            {
                var r = ac.StartCondition($"test_{bus}", bus, "shelter_generator");
                Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            }
            Assert.Equal(11, ac.State.activeConditions.Count);
        }

        private static AudioConditionSystem Create() => new AudioConditionSystem();
    }
}
