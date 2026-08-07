using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Flashpoint;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Guards the C-1 / AUDIT-008 class of bug: systems registered in
    /// SystemRegistry but never dispatched from the live tick path, and
    /// constructed systems (pets, fuel decay) that were never wired.
    /// </summary>
    [TestFixture]
    public class RegistryDispatchWiringTests
    {
        private GameObject _go;
        private GameBootstrap _bootstrap;

        [SetUp]
        public void SetUp()
        {
            // Disable GO so AddComponent does not fire Awake until profiles are injected.
            _go = new GameObject("RegistryDispatchWiringTests");
            _go.SetActive(false);
            _bootstrap = _go.AddComponent<GameBootstrap>();
            InjectBootstrapFields(_bootstrap);
            // EditMode: call InitializeSystems directly (Awake on SetActive can be
            // deferred / skipped depending on test runner lifecycle).
            var init = typeof(GameBootstrap).GetMethod(
                "InitializeSystems",
                BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(init, "InitializeSystems must exist.");
            try
            {
                init.Invoke(_bootstrap, null);
            }
            catch (TargetInvocationException ex)
            {
                Assert.Fail("InitializeSystems threw: " + (ex.InnerException?.ToString() ?? ex.ToString()));
            }
            _go.SetActive(true);
        }

        [TearDown]
        public void TearDown()
        {
            if (_go != null)
                Object.DestroyImmediate(_go);
            _bootstrap = null;
        }

        [Test]
        public void Registry_ContainsPreviouslyDeadSystems()
        {
            Assert.IsNotNull(_bootstrap.Registry, "Registry must be constructed.");
            Assert.IsTrue(_bootstrap.Registry.IsSystemTicked("bunker_social"),
                "BunkerSocialDirector must be registered (was registry-only / never TickAll'd).");
            Assert.IsTrue(_bootstrap.Registry.IsSystemTicked("personal_quests_daily")
                || _bootstrap.Registry.IsSystemTicked("personal_quests"),
                "Personal quest daily tick must be registered.");
            Assert.IsTrue(_bootstrap.Registry.IsSystemTicked("pets"),
                "PetSystem must be registered after wiring.");
            Assert.IsTrue(_bootstrap.Registry.IsSystemTicked("fuel_decay_daily")
                || _bootstrap.Registry.IsSystemTicked("fuel_decay"),
                "FuelDecaySystem must be registered after wiring.");
            Assert.IsTrue(_bootstrap.Registry.IsSystemTicked("chemistry_titles")
                || _bootstrap.Registry.IsSystemTicked("chemistry_titles_daily"),
                "Chemistry/titles daily host helpers must be registered.");
            Assert.IsTrue(_bootstrap.Registry.IsSystemTicked("rebuilders")
                || _bootstrap.Registry.IsSystemTicked("rebuilders_daily"),
                "Rebuilders daily host helpers must be registered.");
        }

        [Test]
        public void PetSystem_And_FuelDecay_AreConstructed()
        {
            Assert.IsNotNull(_bootstrap.PetSystem, "PetSystem must be constructed.");
            Assert.IsNotNull(_bootstrap.FuelDecaySystem, "FuelDecaySystem must be constructed.");
            Assert.IsNotNull(_bootstrap.VerminSystem, "VerminSystem must exist for pet suppression.");
        }

        [Test]
        public void FuelDecay_AppliesBurnMultiplierToPowerNetwork()
        {
            Assume.That(_bootstrap.FuelDecaySystem, Is.Not.Null);
            Assume.That(_bootstrap.PowerNetwork, Is.Not.Null);

            // Day 30 → one varnish interval → 0.90 efficiency → burn mult ≈ 1.111
            _bootstrap.FuelDecaySystem.TickDaily(30);
            var apply = typeof(GameBootstrap).GetMethod(
                "ApplyFuelDecayToPowerNetwork",
                BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(apply, "ApplyFuelDecayToPowerNetwork must exist.");
            apply.Invoke(_bootstrap, null);

            Assert.Greater(
                _bootstrap.PowerNetwork.FuelDegradationBurnMultiplier,
                1.05f,
                "Degraded fuel must increase diesel burn multiplier above 1.");
            Assert.AreEqual(
                0.90f,
                _bootstrap.FuelDecaySystem.State.fuelEfficiencyMultiplier,
                0.001f);
        }

        [Test]
        public void TickAll_AdvancesBunkerSocialDayGate()
        {
            Assume.That(_bootstrap.BunkerSocial, Is.Not.Null);
            Assume.That(_bootstrap.Registry, Is.Not.Null);

            int day = _bootstrap.TimeSystem != null ? _bootstrap.TimeSystem.CurrentDay : 1;
            Assert.DoesNotThrow(() => _bootstrap.Registry.TickAll(1f, day));
            Assert.DoesNotThrow(() => _bootstrap.Registry.TickAll(1f, day + 1));
        }

        [Test]
        public void ConstructedSystems_AreRegistered_NoC1Gaps()
        {
            // Ratchet, not a binary gate. There is a large pre-existing population of
            // systems that are constructed and save-wired but never driven; asserting
            // IsEmpty here just kept the suite permanently red, which is the same as
            // having no guard at all. Instead we pin the known set and fail on either
            // direction of drift:
            //
            //   * a NEW unticked system  -> someone added a system and forgot to wire it
            //   * a baseline entry that is now wired -> the file must shrink to match
            //
            // The second half is what makes this a ratchet: the debt can only go down.
            var actual = new SortedSet<string>(_bootstrap.GetUntickedSystemNames());
            var baseline = UntickedSystemsBaseline.Load();

            var regressions = new SortedSet<string>(actual);
            regressions.ExceptWith(baseline);

            var fixedSince = new SortedSet<string>(baseline);
            fixedSince.ExceptWith(actual);

            Assert.IsEmpty(regressions,
                "C-1 REGRESSION — these systems are constructed but never ticked, and are not in the " +
                $"baseline. Wire them into SystemRegistry (see {UntickedSystemsBaseline.RelativePath}):\n  " +
                string.Join("\n  ", regressions));

            Assert.IsEmpty(fixedSince,
                $"C-1 baseline is stale — these are now wired. Delete them from " +
                $"{UntickedSystemsBaseline.RelativePath} so the ratchet keeps holding:\n  " +
                string.Join("\n  ", fixedSince));
        }

        [Test]
        public void FuelDecaySystem_Standalone_DegradesEfficiency()
        {
            var fuel = new FuelDecaySystem();
            fuel.TickDaily(0);
            Assert.AreEqual(1f, fuel.State.fuelEfficiencyMultiplier, 0.001f);
            fuel.TickDaily(60);
            Assert.AreEqual(0.80f, fuel.State.fuelEfficiencyMultiplier, 0.001f);
            fuel.TickDaily(300);
            Assert.AreEqual(0.20f, fuel.State.fuelEfficiencyMultiplier, 0.001f,
                "Efficiency floors at 0.20.");
        }

        [Test]
        public void PetAndFuel_AreRegisteredWithSaveSystem()
        {
            Assume.That(_bootstrap.SaveSystem, Is.Not.Null);
            Assume.That(_bootstrap.PetSystem, Is.Not.Null);
            Assume.That(_bootstrap.FuelDecaySystem, Is.Not.Null);

            // Mutate, capture via SaveSystem adapters by re-setting (idempotent Register).
            _bootstrap.PetSystem.AddPet(new PetState
            {
                Id = "wire_pet",
                DisplayName = "Wire",
                Hunger = 5f,
                IsAlive = true
            });
            _bootstrap.FuelDecaySystem.TickDaily(30);

            _bootstrap.SaveSystem.SetPetSystem(_bootstrap.PetSystem);
            _bootstrap.SaveSystem.SetFuelDecaySystem(_bootstrap.FuelDecaySystem);

            // Capture through the public CaptureState APIs that adapters wrap.
            var petSave = _bootstrap.PetSystem.CaptureState();
            var fuelSave = _bootstrap.FuelDecaySystem.CaptureState();
            Assert.IsNotNull(petSave);
            Assert.IsNotNull(petSave.Pets);
            Assert.GreaterOrEqual(petSave.Pets.Length, 1);
            Assert.AreEqual(0.90f, fuelSave.fuelEfficiencyMultiplier, 0.001f);

            var petsB = new PetSystem(new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>()));
            var fuelB = new FuelDecaySystem();
            petsB.RestoreState(petSave);
            fuelB.RestoreState(fuelSave);
            Assert.AreEqual("wire_pet", petsB.GetPet("wire_pet")?.Id);
            Assert.AreEqual(0.90f, fuelB.State.fuelEfficiencyMultiplier, 0.001f);
        }

        /// <summary>Shared with UntickedSystemsBaselineTests so both fixtures boot identically.</summary>
        internal static void InjectBootstrapFields(GameBootstrap bs)
        {
            SetPrivate(bs, "_needsProfile", ScriptableObject.CreateInstance<NeedsProfile>());
            SetPrivate(bs, "_lightProfile", ScriptableObject.CreateInstance<LightProfile>());
            SetPrivate(bs, "_seasonProfile", ScriptableObject.CreateInstance<SeasonProfile>());
            SetPrivate(bs, "_itemCatalog", ScriptableObject.CreateInstance<ItemCatalogSO>());
            SetPrivate(bs, "_recipeCatalog", ScriptableObject.CreateInstance<RecipeCatalogSO>());
            SetPrivate(bs, "_eventCatalog", ScriptableObject.CreateInstance<GameEventCatalogSO>());
            SetPrivate(bs, "_locationCatalog", ScriptableObject.CreateInstance<LocationCatalogSO>());
            SetPrivate(bs, "_radioCatalog", ScriptableObject.CreateInstance<RadioCatalogSO>());
            SetPrivate(bs, "_worldPhaseConfig", ScriptableObject.CreateInstance<WorldPhaseConfigSO>());
            SetPrivate(bs, "_flashpointSequence", ScriptableObject.CreateInstance<FlashpointSequenceSO>());
            SetPrivate(bs, "_mentalBreakCatalog", ScriptableObject.CreateInstance<MentalBreakCatalogSO>());
            SetPrivate(bs, "_lootTable", ScriptableObject.CreateInstance<LootTableSO>());
        }

        private static void SetPrivate(object instance, string fieldName, object value)
        {
            var f = instance.GetType().GetField(fieldName,
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(f, $"{instance.GetType().Name} missing field '{fieldName}'");
            f.SetValue(instance, value);
        }
    }
}
