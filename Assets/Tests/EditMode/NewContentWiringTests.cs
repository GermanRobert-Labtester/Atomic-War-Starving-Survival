using NUnit.Framework;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Crafting;

namespace AtomicWar.Tests.EditMode
{
    // Wiring tests: verify the new content registers ids / states correctly
    // and that the SaveSystem registration shape matches the project convention.

    [TestFixture]
    public class NewWeatherStateIdsTests
    {
        [Test]
        public void AshLightningStateHasCanonicalId()
        {
            var w = new Weather_AshLightning();
            Assert.AreEqual("weather_ash_lightning", w.State.weatherId);
        }
        [Test]
        public void FogStateHasCanonicalId()
        {
            var w = new Weather_FogOfParticulate();
            Assert.AreEqual("weather_fog_of_particulate", w.State.weatherId);
        }
        [Test]
        public void ThermalInversionStateHasCanonicalId()
        {
            var w = new Weather_ThermalInversion();
            Assert.AreEqual("weather_thermal_inversion", w.State.weatherId);
        }
        [Test]
        public void IceStormStateHasCanonicalId()
        {
            var w = new Weather_IceStorm();
            Assert.AreEqual("weather_ice_storm", w.State.weatherId);
        }
        [Test]
        public void SilenceStateHasCanonicalId()
        {
            var w = new Weather_Silence();
            Assert.AreEqual("weather_silence", w.State.weatherId);
        }
    }

    [TestFixture]
    public class NewWeatherSaveRoundTripTests
    {
        [Test]
        public void AshLightningRoundTrip()
        {
            var w = new Weather_AshLightning();
            w.SetActive(true);
            w.Tick(1f, true, true, new System.Random(1));
            var state = w.CaptureState();
            Assert.IsTrue(state.isActive);
            var w2 = new Weather_AshLightning();
            w2.RestoreState(state);
            Assert.IsTrue(w2.State.isActive);
        }
        [Test]
        public void FogRoundTrip()
        {
            var w = new Weather_FogOfParticulate();
            w.SetActive(true);
            var s = w.CaptureState();
            var w2 = new Weather_FogOfParticulate();
            w2.RestoreState(s);
            Assert.AreEqual(2f, w2.State.visibilityMeters, 0.001f);
        }
        [Test]
        public void IceStormRoundTrip()
        {
            var w = new Weather_IceStorm();
            w.SetActive(true);
            w.Tick(1f, 0.5f);
            Assert.IsTrue(w.State.hatchFrozenShut);
            var s = w.CaptureState();
            var w2 = new Weather_IceStorm();
            w2.RestoreState(s);
            Assert.IsTrue(w2.State.hatchFrozenShut);
        }
    }

    [TestFixture]
    public class NewRecipesWiringTests
    {
        [Test]
        public void AllTenRecipeIdsAreUnique()
        {
            var ids = NewRecipesCatalog.Ids;
            var set = new System.Collections.Generic.HashSet<string>();
            set.Add(ids.Tourniquet); set.Add(ids.SalineDrip); set.Add(ids.CookRatMeat);
            set.Add(ids.PressInsectBrick); set.Add(ids.AshBread); set.Add(ids.RepairGasket);
            set.Add(ids.ImprovisedMolotov); set.Add(ids.DistillWater); set.Add(ids.LeadVest);
            set.Add(ids.TallowCandle);
            Assert.AreEqual(10, set.Count);
        }

        [Test]
        public void MaterialiseResolvesItemIdsThroughLookup()
        {
            var knownIds = new System.Collections.Generic.HashSet<string>();
            var lookup = new System.Func<string, AtomicWar._Game.Inventory.ItemDefinition>(id =>
            {
                if (knownIds.Add(id))
                {
                    var def = UnityEngine.ScriptableObject.CreateInstance<AtomicWar._Game.Inventory.ItemDefinition>();
                    def.id = id;
                    def.displayName = id;
                    return def;
                }
                return null;
            });
            var recipes = NewRecipesCatalog.MaterialiseAll(lookup);
            Assert.AreEqual(10, recipes.Count);
        }

        [Test]
        public void DistillWaterProducesDoubleInput()
        {
            var specs = NewRecipesCatalog.BuildAll();
            var distill = specs.Find(s => s.Id == NewRecipesCatalog.Ids.DistillWater);
            Assert.AreEqual(3, distill.Ingredients.Find(i => i.ItemId == "dirty_water").Count);
            Assert.AreEqual(2, distill.ResultAmount);
        }

        [Test]
        public void LeadVestIsLongest()
        {
            var specs = NewRecipesCatalog.BuildAll();
            var lead = specs.Find(s => s.Id == NewRecipesCatalog.Ids.LeadVest);
            Assert.AreEqual(4.0f, lead.CraftingTimeHours, 0.001f);
        }
    }
}
