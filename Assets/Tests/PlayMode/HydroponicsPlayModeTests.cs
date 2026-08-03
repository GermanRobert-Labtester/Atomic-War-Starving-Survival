using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Shelter;

namespace AtomicWar.Tests.PlayMode
{
    [TestFixture]
    public class HydroponicsPlayModeTests
    {
        private CropSO _potatoes;
        private CropSO _fungi;
        private PlanterBox _planter;
        private ShelterRoom _room;

        [SetUp]
        public void SetUp()
        {
            _potatoes = CropSO.CreatePotatoes();
            _fungi = CropSO.CreateMutatedFungi();
            _planter = new PlanterBox();
            _room = new ShelterRoom("hydroponics_bay", null);
        }

        [UnityTest]
        public IEnumerator PlantSeeds_ProvideWaterAndLight_ReachesMaturityAndYields()
        {
            Assert.That(_planter.PlantSeed(_potatoes), Is.True);
            Assert.That(_planter.Stage, Is.EqualTo(CropLifecycleStage.Seed));

            // Tick for 20 hours (optimal room temp 20°C, grow light powered)
            _planter.Tick(20f, 20f, true, _room);
            yield return null;

            Assert.That(_planter.Stage, Is.EqualTo(CropLifecycleStage.Sprout));

            // Tick remaining hours to reach 48h total
            _planter.Tick(28f, 20f, true, _room);
            yield return null;

            Assert.That(_planter.Stage, Is.EqualTo(CropLifecycleStage.Mature));

            // Harvest
            Assert.That(_planter.Harvest(out float calories, out float contamination), Is.True);
            Assert.That(calories, Is.EqualTo(50f));
            Assert.That(contamination, Is.EqualTo(0f));
        }

        [UnityTest]
        public IEnumerator TemperatureDropTo5C_KillsCrop()
        {
            _planter.PlantSeed(_potatoes);
            _planter.Tick(10f, 20f, true, _room);
            yield return null;

            Assert.That(_planter.Stage, Is.Not.EqualTo(CropLifecycleStage.Dead));

            // Turn off heater -> temp drops to 5°C
            _planter.Tick(1f, 5f, true, _room);
            yield return null;

            Assert.That(_planter.Stage, Is.EqualTo(CropLifecycleStage.Dead), "Crop must die when temperature drops below 10°C.");
        }

        [UnityTest]
        public IEnumerator IrrigatingWithDirtyWater_RuinsCrop_And_IntroducesRoomMold()
        {
            _planter.PlantSeed(_potatoes);
            _planter.Tick(5f, 20f, true, _room);
            yield return null;

            Assert.That(_room.HasMold, Is.False);

            // Water with dirty water
            bool success = _planter.Water(isCleanWater: false, room: _room);
            yield return null;

            Assert.That(success, Is.False);
            Assert.That(_planter.Stage, Is.EqualTo(CropLifecycleStage.Dead), "Dirty water must ruin crop.");
            Assert.That(_room.HasMold, Is.True, "Dirty water must introduce mold to room.");
            Assert.That(_room.MoldLevel, Is.GreaterThan(0f));
        }

        [UnityTest]
        public IEnumerator UnpoweredGrowLightsForMoreThan24Hours_StallsCrop()
        {
            _planter.PlantSeed(_fungi);

            // Power cut for 25 hours
            _planter.Tick(25f, 20f, isLightPowered: false, _room);
            yield return null;

            Assert.That(_planter.IsStalled, Is.True, "Crop must stall after 24h without grow-light power.");
            float growthAtStall = _planter.GrowthHours;

            // Further ticks while stalled do not advance growth
            _planter.Tick(10f, 20f, isLightPowered: false, _room);
            yield return null;

            Assert.That(_planter.GrowthHours, Is.EqualTo(growthAtStall), "Stalled crop must not advance growth.");

            // Restore power
            _planter.Tick(1f, 20f, isLightPowered: true, _room);
            yield return null;

            Assert.That(_planter.IsStalled, Is.False, "Restoring power un-stalls crop.");
        }
    }
}
