using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class GuiltInsomniaSystemTests
    {
        [Fact]
        public void RecordGuilt_IncreasesSeverity()
        {
            var sys = new GuiltInsomniaSystem();
            sys.RecordGuilt("sv_1", "ration_cutting", 0.5f, 100);
            Assert.Equal(0.5f, sys.GetInsomniaSeverity("sv_1"));
            Assert.Equal(1, sys.GetGuiltSourceCount("sv_1"));
        }

        [Fact]
        public void RecordGuilt_MultipleSources_CapsAt1()
        {
            var sys = new GuiltInsomniaSystem();
            sys.RecordGuilt("sv_1", "source_a", 0.6f, 100);
            sys.RecordGuilt("sv_1", "source_b", 0.6f, 101);
            Assert.Equal(1f, sys.GetInsomniaSeverity("sv_1"));
        }

        [Fact]
        public void RecordGuilt_FiresEvent()
        {
            var sys = new GuiltInsomniaSystem();
            string firedFor = null;
            sys.OnGuiltRecorded += (id, _) => firedFor = id;
            sys.RecordGuilt("sv_1", "source_a", 0.3f, 100);
            Assert.Equal("sv_1", firedFor);
        }

        [Fact]
        public void RecordGuilt_CriticalThreshold_FiresEvent()
        {
            var sys = new GuiltInsomniaSystem();
            string criticalFor = null;
            sys.OnGuiltInsomniaCritical += id => criticalFor = id;
            sys.RecordGuilt("sv_1", "source_a", 0.8f, 100);
            Assert.Equal("sv_1", criticalFor);
        }

        [Fact]
        public void ApplySedative_ReducesSeverity()
        {
            var sys = new GuiltInsomniaSystem();
            sys.RecordGuilt("sv_1", "source_a", 0.8f, 100);
            Assert.True(sys.ApplySedative("sv_1"));
            Assert.Equal(0.4f, sys.GetInsomniaSeverity("sv_1"), 2);
        }

        [Fact]
        public void ApplySedative_NoGuilt_ReturnsFalse()
        {
            var sys = new GuiltInsomniaSystem();
            Assert.False(sys.ApplySedative("sv_1"));
        }

        [Fact]
        public void ResolveDialogue_RemovesMostRecentGuilt()
        {
            var sys = new GuiltInsomniaSystem();
            sys.RecordGuilt("sv_1", "source_a", 0.3f, 100);
            sys.RecordGuilt("sv_1", "source_b", 0.3f, 101);
            Assert.True(sys.ResolveGuiltThroughDialogue("sv_1"));
            Assert.Equal(1, sys.GetGuiltSourceCount("sv_1"));
        }

        [Fact]
        public void ResolveDialogue_LastSource_FiresResolved()
        {
            var sys = new GuiltInsomniaSystem();
            sys.RecordGuilt("sv_1", "source_a", 0.3f, 100);
            string resolvedFor = null;
            sys.OnGuiltResolved += id => resolvedFor = id;
            sys.ResolveGuiltThroughDialogue("sv_1");
            Assert.Equal("sv_1", resolvedFor);
        }

        [Fact]
        public void SleepQuality_LowerWithGuilt()
        {
            var sys = new GuiltInsomniaSystem();
            Assert.Equal(1f, sys.GetSleepQualityMultiplier("sv_1"));
            sys.RecordGuilt("sv_1", "source_a", 0.8f, 100);
            Assert.True(sys.GetSleepQualityMultiplier("sv_1") < 1f);
        }

        [Fact]
        public void SleepQuality_SedativeHalvesPenalty()
        {
            var sys = new GuiltInsomniaSystem();
            sys.RecordGuilt("sv_1", "source_a", 0.8f, 100);
            float before = sys.GetSleepQualityMultiplier("sv_1");
            sys.ApplySedative("sv_1");
            float after = sys.GetSleepQualityMultiplier("sv_1");
            Assert.True(after > before);
        }

        [Fact]
        public void Tick_ExpiresOldGuilt()
        {
            var sys = new GuiltInsomniaSystem();
            sys.RecordGuilt("sv_1", "source_a", 0.5f, 100);
            sys.Tick("sv_1", 1f, 131);
            Assert.Equal(0, sys.GetGuiltSourceCount("sv_1"));
            Assert.Equal(0f, sys.GetInsomniaSeverity("sv_1"));
        }

        [Fact]
        public void Tick_DecaysSedative()
        {
            var sys = new GuiltInsomniaSystem();
            sys.RecordGuilt("sv_1", "source_a", 0.8f, 100);
            sys.ApplySedative("sv_1");
            float withSedative = sys.GetSleepQualityMultiplier("sv_1");
            sys.Tick("sv_1", 13f, 100); // sedative lasts 12h, so expires
            float afterExpiry = sys.GetSleepQualityMultiplier("sv_1");
            Assert.True(afterExpiry < withSedative, $"Expected {afterExpiry} < {withSedative}");
        }

        [Fact]
        public void CaptureRestore_Roundtrip()
        {
            var sys = new GuiltInsomniaSystem();
            sys.RecordGuilt("sv_1", "source_a", 0.5f, 100);
            sys.RecordGuilt("sv_2", "source_b", 0.3f, 101);
            sys.ApplySedative("sv_1");

            var save = sys.CaptureState();
            Assert.Equal(2, save.survivors.Count);

            var restored = new GuiltInsomniaSystem();
            restored.RestoreState(save);
            Assert.Equal(1, restored.GetGuiltSourceCount("sv_1"));
            Assert.Equal(1, restored.GetGuiltSourceCount("sv_2"));
        }

        [Fact]
        public void RestoreNull_DoesNotCrash()
        {
            var sys = new GuiltInsomniaSystem();
            sys.RestoreState(null);
            Assert.Equal(0f, sys.GetInsomniaSeverity("sv_1"));
        }

        [Fact]
        public void RecordGuilt_RejectsEmptyId()
        {
            var sys = new GuiltInsomniaSystem();
            sys.RecordGuilt("", "source", 0.5f, 100);
            sys.RecordGuilt(null, "source", 0.5f, 100);
            Assert.Equal(0, sys.GetGuiltSourceCount(""));
        }
    }
}
