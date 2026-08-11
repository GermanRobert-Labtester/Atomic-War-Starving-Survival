using NUnit.Framework;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Crafting;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class WeatherAshLightningTests
    {
        [Test]
        public void InactiveWeatherDoesNotDamage()
        {
            var w = new Weather_AshLightning();
            bool fired = w.Tick(1f, false, true, new System.Random(1));
            Assert.IsFalse(fired);
        }

        [Test]
        public void ActiveWeatherRollsFireOnUnshieldedElectronics()
        {
            var w = new Weather_AshLightning();
            w.SetActive(true);
            // Run many ticks; with 5% per hour, the cumulative chance over a long stretch
            // is non-trivial.
            bool everFired = false;
            for (int i = 0; i < 200; i++)
            {
                if (w.Tick(1f, false, true, new System.Random(i)))
                {
                    everFired = true; break;
                }
            }
            Assert.IsTrue(everFired);
        }

        [Test]
        public void BlocksSurfaceExpeditionsWhileActive()
        {
            var w = new Weather_AshLightning();
            Assert.IsFalse(w.BlocksSurfaceExpeditions);
            w.SetActive(true);
            Assert.IsTrue(w.BlocksSurfaceExpeditions);
        }
    }

    [TestFixture]
    public class WeatherFogOfParticulateTests
    {
        [Test]
        public void UnmaskedOutsideAppliesDose()
        {
            var w = new Weather_FogOfParticulate();
            w.SetActive(true);
            float dose = w.Tick(1f, isOutside: true, hasMask: false, rng: new System.Random(1));
            Assert.AreEqual(5f, dose, 0.001f);
        }

        [Test]
        public void MaskedOutsideAppliesZeroDose()
        {
            var w = new Weather_FogOfParticulate();
            w.SetActive(true);
            float dose = w.Tick(1f, isOutside: true, hasMask: true, rng: new System.Random(1));
            Assert.AreEqual(0f, dose, 0.001f);
        }
    }

    [TestFixture]
    public class WeatherThermalInversionTests
    {
        [Test]
        public void NoisePropagationTripledWhileActive()
        {
            var w = new Weather_ThermalInversion();
            Assert.AreEqual(1f, w.GetNoisePropagationMultiplier(), 0.001f);
            w.SetActive(true);
            Assert.AreEqual(3f, w.GetNoisePropagationMultiplier(), 0.001f);
        }

        [Test]
        public void SurfaceRadiationDoubledWhileActive()
        {
            var w = new Weather_ThermalInversion();
            w.SetActive(true);
            Assert.AreEqual(2f, w.GetSurfaceRadiationMultiplier(), 0.001f);
        }
    }

    [TestFixture]
    public class WeatherIceStormTests
    {
        [Test]
        public void FirstTickFreezesHatch()
        {
            var w = new Weather_IceStorm();
            w.SetActive(true);
            w.Tick(1f, 0.5f);
            Assert.IsTrue(w.State.hatchFrozenShut);
        }

        [Test]
        public void DeactivationUnfreezesHatch()
        {
            var w = new Weather_IceStorm();
            w.SetActive(true);
            w.Tick(1f, 0.5f);
            w.SetActive(false);
            Assert.IsFalse(w.State.hatchFrozenShut);
        }

        [Test]
        public void BlocksSolarAndSurface()
        {
            var w = new Weather_IceStorm();
            w.SetActive(true);
            Assert.IsTrue(w.BlocksSolar);
            Assert.IsTrue(w.BlocksSurfaceAccess);
        }
    }

    [TestFixture]
    public class WeatherSilenceTests
    {
        [Test]
        public void ActiveFiresEvents()
        {
            var w = new Weather_Silence();
            w.SetActive(true);
            int clearSkyCount = 0;
            w.OnClearSkyObserved += _ => clearSkyCount++;
            w.Tick(1f);
            Assert.Greater(clearSkyCount, 0);
        }

        [Test]
        public void RecordingSurfaceVenturerRaisesEvent()
        {
            var w = new Weather_Silence();
            w.SetActive(true);
            string recorded = null;
            w.OnSurfaceVentured += (s, id) => recorded = id;
            w.RecordSurfaceVentured("sv_alice");
            Assert.AreEqual("sv_alice", recorded);
        }
    }

    [TestFixture]
    public class NewRecipesCatalogTests
    {
        [Test]
        public void CatalogHasTenRecipes()
        {
            var specs = NewRecipesCatalog.BuildAll();
            Assert.AreEqual(10, specs.Count);
        }

        [Test]
        public void EverySpecHasAStation()
        {
            var specs = NewRecipesCatalog.BuildAll();
            for (int i = 0; i < specs.Count; i++)
                Assert.IsFalse(string.IsNullOrEmpty(specs[i].StationId), "missing station: " + specs[i].Id);
        }

        [Test]
        public void RepairGasketProducesEffectNotItem()
        {
            var specs = NewRecipesCatalog.BuildAll();
            var repair = specs.Find(s => s.Id == NewRecipesCatalog.Ids.RepairGasket);
            Assert.IsNotNull(repair);
            Assert.AreEqual("hatch_seal_integrity", repair.EffectKey);
            Assert.AreEqual(0.15f, repair.EffectAmount, 0.001f);
        }

        [Test]
        public void KnownIdsResolveToMaterialisedRecipes()
        {
            var lookup = new System.Func<string, AtomicWar._Game.Inventory.ItemDefinition>(id =>
            {
                var def = UnityEngine.ScriptableObject.CreateInstance<AtomicWar._Game.Inventory.ItemDefinition>();
                def.id = id;
                def.displayName = id;
                return def;
            });
            var recipes = NewRecipesCatalog.MaterialiseAll(lookup);
            Assert.AreEqual(10, recipes.Count);
            for (int i = 0; i < recipes.Count; i++)
            {
                Assert.IsNotNull(recipes[i]);
                Assert.IsFalse(string.IsNullOrEmpty(recipes[i].id));
            }
        }
    }
}
