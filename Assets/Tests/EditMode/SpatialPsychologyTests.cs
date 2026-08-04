using NUnit.Framework;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class SpatialPsychologyTests
    {
        private SpatialPsychologySystem _spatialSystem;
        private Survivor _claustrophobicSurvivor;
        private Survivor _agoraphobicSurvivor;

        [SetUp]
        public void SetUp()
        {
            _spatialSystem = new SpatialPsychologySystem();

            _claustrophobicSurvivor = new Survivor
            {
                Id = "claustrophobe",
                DisplayName = "Indoor Sufferer",
                IsOnExpedition = false
            };
            _claustrophobicSurvivor.Traits.Add(SpatialPsychologySystem.TraitClaustrophobic);
            _claustrophobicSurvivor.Needs.Morale = 50f;

            _agoraphobicSurvivor = new Survivor
            {
                Id = "agoraphobe",
                DisplayName = "Outdoor Fear",
                RadiationAnxiety = 0.1f,
                IsOnExpedition = false
            };
            _agoraphobicSurvivor.Traits.Add(SpatialPsychologySystem.TraitAgoraphobic);
        }

        [Test]
        public void Tick_ClaustrophobicInsideBunker_DrainsMoraleOverTime()
        {
            _spatialSystem.Tick(24f, new[] { _claustrophobicSurvivor });

            Assert.AreEqual(45f, _claustrophobicSurvivor.Needs.Morale, 0.01f);
        }

        [Test]
        public void Tick_ClaustrophobicOnExpedition_DoesNotDrainMorale()
        {
            _claustrophobicSurvivor.IsOnExpedition = true;

            _spatialSystem.Tick(24f, new[] { _claustrophobicSurvivor });

            Assert.AreEqual(50f, _claustrophobicSurvivor.Needs.Morale, 0.01f);
        }

        [Test]
        public void OnExpeditionStarted_Agoraphobic_SpikesRadiationAnxiety()
        {
            _spatialSystem.OnExpeditionStarted(_agoraphobicSurvivor);

            Assert.IsTrue(_agoraphobicSurvivor.IsOnExpedition);
            Assert.AreEqual(0.3f, _agoraphobicSurvivor.RadiationAnxiety, 0.01f);
        }
    }
}
