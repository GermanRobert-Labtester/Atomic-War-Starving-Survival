using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation; // CompostSystem, SterilizationSystem, etc. (audit C-3 split)
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.AI;
using AtomicWar._Game.Events;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Data;
using AtomicWar._Game.UI;
// Aliases: the Shelter/Inventory namespaces collide with the class types.
using ShelterClass = AtomicWar._Game.Shelter.Shelter;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    public class SaveDtoRoundTripTests
    {
        // =============================================================
        // SECTION 1: Reflective DTO equality helper
        // =============================================================

        /// <summary>
        /// Recursive field-by-field equality for any save DTO. Two objects are
        /// "DTO-equal" if every serializable field (and nested DTO field) has
        /// the same value, with float tolerance for accumulated numerics.
        /// </summary>
        public static void AssertDtoEqual(object expected, object actual, string path = "root", float tolerance = 1e-4f)
        {
            if (ReferenceEquals(expected, actual)) return;
            if (expected == null && actual == null) return;
            if (expected == null) Assert.Fail($"{path}: expected null but was {actual?.GetType().Name ?? "null"}");
            if (actual == null) Assert.Fail($"{path}: expected {expected.GetType().Name} but was null");
            Assert.AreEqual(expected.GetType(), actual.GetType(), $"{path}: type mismatch");

            // JsonUtility does not serialize auto-properties; the save DTOs
            // use plain public fields, so we walk fields.
            var fields = expected.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var f in fields)
                AssertFieldEqual(f, f.GetValue(expected), f.GetValue(actual), $"{path}.{f.Name}", tolerance);
        }

        private static void AssertFieldEqual(FieldInfo f, object ev, object av, string sub, float tolerance)
        {
            if (ev == null && av == null) return;
            if (ev == null || av == null)
            {
                Assert.Fail($"{sub}: expected {(ev == null ? "null" : ev.ToString())} but was {(av == null ? "null" : av.ToString())}");
                return;
            }

            if (f.FieldType == typeof(float))
                Assert.AreEqual((float)ev, (float)av, tolerance, sub);
            else if (f.FieldType == typeof(double))
                Assert.AreEqual((double)ev, (double)av, (double)tolerance, sub);
            else if (IsExactComparable(f.FieldType))
                Assert.AreEqual(ev, av, sub);
            else if (f.FieldType.IsArray)
                AssertArraysEqual((Array)ev, (Array)av, sub, tolerance);
            else if (IsNestedDtoOrList(f.FieldType))
                AssertDtoCollectionEqual(ev, av, sub, tolerance);
            else if (f.FieldType.IsValueType)
                AssertDtoEqual(ev, av, sub, tolerance);
            else if (typeof(UnityEngine.Object).IsAssignableFrom(f.FieldType))
                Assert.AreSame(ev, av, sub);
            else
                Assert.Fail($"{sub}: unsupported DTO field type {f.FieldType.Name}");
        }

        private static bool IsExactComparable(Type t)
            => t.IsPrimitive || t.IsEnum || t == typeof(string);

        private static bool IsNestedDtoOrList(Type t)
            => t.IsClass || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>));

        private static void AssertArraysEqual(Array exp, Array act, string path, float tolerance)
        {
            Assert.AreEqual(exp.Length, act.Length, $"{path}.Length");
            for (int i = 0; i < exp.Length; i++)
            {
                var ev = exp.GetValue(i);
                var av = act.GetValue(i);
                if (ev == null && av == null) continue;
                // Element-wise; recurse for nested DTOs/arrays.
                if (ev is float fE && av is float fA) Assert.AreEqual(fE, fA, tolerance, $"{path}[{i}]");
                else if (ev is double dE && av is double dA) Assert.AreEqual(dE, dA, (double)tolerance, $"{path}[{i}]");
                else if (ev is Array ea && av is Array aa) AssertArraysEqual(ea, aa, $"{path}[{i}]", tolerance);
                else if (exp.GetType().GetElementType().IsClass || (exp.GetType().GetElementType().IsGenericType && exp.GetType().GetElementType().GetGenericTypeDefinition() == typeof(List<>)))
                    AssertDtoCollectionEqual(ev, av, $"{path}[{i}]", tolerance);
                else
                    Assert.AreEqual(ev, av, $"{path}[{i}]");
            }
        }

        private static void AssertDtoCollectionEqual(object exp, object act, string path, float tolerance)
        {
            if (exp is IList expList && act is IList actList)
            {
                Assert.AreEqual(expList.Count, actList.Count, $"{path}.Count");
                for (int i = 0; i < expList.Count; i++)
                    AssertDtoEqual(expList[i], actList[i], $"{path}[{i}]", tolerance);
                return;
            }
            // Both are nested DTO objects.
            AssertDtoEqual(exp, act, path, tolerance);
        }

        // =============================================================
        // SECTION 2: Per-system Capture/Restore round-trip
        // =============================================================

        /// <summary>
        /// Helper: invoke a parameterless CaptureState via reflection. Returns
        /// null if the system has no CaptureState method. Throws if the method
        /// exists but throws.
        /// </summary>
        public static object CaptureState(object system)
        {
            if (system == null) return null;
            var m = system.GetType().GetMethod("CaptureState", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
            return m?.Invoke(system, null);
        }

        public static void RestoreState(object system, object save)
        {
            if (system == null) return;
            var m = system.GetType().GetMethod("RestoreState", BindingFlags.Public | BindingFlags.Instance, null, new[] { save?.GetType() ?? typeof(object) }, null);
            if (m == null) return;
            m.Invoke(system, new[] { save });
        }

        // -------------------------------------------------------------
        // Test 1: Every save DTO defined in the SimulationSystems module
        // round-trips through its system's Capture/Restore.
        // -------------------------------------------------------------

        [Test]
        public void SimulationSystems_AllSaveDtos_RoundTripEqual()
        {
            // (System ctor args, state-mutation call, expected non-default)
            var cases = new (string name, System.Func<object> ctor, System.Action<object> mutate)[]
            {
                ("ResilienceSystem",
                    () => new ResilienceSystem(),
                    s => ((ResilienceSystem)s).OnTraumaSurvived("sv1")),
                ("CompostSystem",
                    () => new CompostSystem(),
                    s => ((CompostSystem)s).AddWaste(7.5f)),
                ("SterilizationSystem",
                    () => new SterilizationSystem(),
                    s => ((SterilizationSystem)s).UseTools()),
                ("ChelationSystem",
                    () => new ChelationSystem(),
                    s => ((ChelationSystem)s).BeginChelation("sv1")),
                ("WindTurbineSystem",
                    () => new WindTurbineSystem(),
                    s => ((WindTurbineSystem)s).Build()),
                ("AntibioticResistanceSystem",
                    () => new AntibioticResistanceSystem(),
                    s => ((AntibioticResistanceSystem)s).TryUseExpired("sv1", new System.Random(1))),
                ("InternalHaulingSystem",
                    () => new InternalHaulingSystem(),
                    s => ((InternalHaulingSystem)s).DumpLootInAirlock(50f)),
                ("WeaponMaintenanceSystem",
                    () => new WeaponMaintenanceSystem(),
                    s => ((WeaponMaintenanceSystem)s).Fire("rifle_1")),
                ("RoomAestheticsSystem",
                    () => new RoomAestheticsSystem(),
                    s => ((RoomAestheticsSystem)s).SetDecor("quarters", 0.7f)),
                ("HamRadioSystem",
                    () => new HamRadioSystem(),
                    s => ((HamRadioSystem)s).TickBroadcast(48f, true)),
                ("TriageBoardSystem",
                    () => new TriageBoardSystem(),
                    s => ((TriageBoardSystem)s).SetPermission("sv1", TriageBoardSystem.TriageLevel.Basic)),
                ("PolypharmacySystem",
                    () => new PolypharmacySystem(),
                    s => ((PolypharmacySystem)s).RecordDose("sv1", "iodine", 0f)),
                ("FuelDecaySystem",
                    () => new FuelDecaySystem(),
                    s => ((FuelDecaySystem)s).TickDaily(90)),
                ("AddictionSystem",
                    () => new AddictionSystem(new System.Random(7)),
                    s =>
                    {
                        // Seed recovery progress via Capture/Restore shape (system-owned dict).
                        ((AddictionSystem)s).RestoreState(new AddictionSave
                        {
                            Keys = new[] { "sv_addict" },
                            Values = new[] { 120.5f }
                        });
                    }),
            };

            foreach (var (name, ctor, mutate) in cases)
            {
                var sysA = ctor();
                mutate(sysA);
                var save = CaptureState(sysA);
                Assert.IsNotNull(save, $"{name}.CaptureState returned null after mutation.");

                var sysB = ctor();
                RestoreState(sysB, save);
                var save2 = CaptureState(sysB);

                AssertDtoEqual(save, save2, name, tolerance: 1e-4f);
            }
        }

        // -------------------------------------------------------------
        // Test 2: Every save DTO defined in the Shelter module
        // round-trips.
        // -------------------------------------------------------------

        [Test]
        public void ShelterSystems_AllSaveDtos_RoundTripEqual()
        {
            var cases = new (string name, System.Func<object> ctor, System.Action<object> mutate)[]
            {
                ("ExcavationSystem",
                    () => new ExcavationSystem(new System.Random(119)),
                    s => ((ExcavationSystem)s).SealRoom("r1", 100f)),
                ("RoomFloodingSystem",
                    () => new RoomFloodingSystem(),
                    s => { /* mark some rooms flooded by capturing then restoring */
                        var sys = (RoomFloodingSystem)s;
                        // FloodedRooms is read-only; use the roomflooding internal API.
                        // We round-trip the empty state since there's no public "Flood"
                        // method in this version. (The Save class covers the state.)
                    }),
                ("HiddenStorageSystem",
                    () => new HiddenStorageSystem(),
                    s => ((HiddenStorageSystem)s).HideItem("canned_food", 5)),
                ("CeilingCollapseSystem",
                    () => new CeilingCollapseSystem(),
                    s => ((CeilingCollapseSystem)s).RegisterRoom("r1", 5f)),
                ("PerimeterTrapSystem",
                    () => new PerimeterTrapSystem(),
                    s => ((PerimeterTrapSystem)s).DeployTrap(PerimeterTrapSystem.BearTrapItemId, 3)),
                ("TunnelingSystem",
                    () => new TunnelingSystem(),
                    s => ((TunnelingSystem)s).SeedNeighbor(new System.Random(124))),
                ("HatchVisibilitySystem",
                    () => new HatchVisibilitySystem(),
                    s => { /* Visibility is a getter; the save covers the state */ }),
                ("EscapeHatchSystem",
                    () => new EscapeHatchSystem(),
                    s => ((EscapeHatchSystem)s).Excavate(12f)),
                ("MaterialShieldingSystem",
                    () => new MaterialShieldingSystem(),
                    s => ((MaterialShieldingSystem)s).UpgradeCeiling("quarters", MaterialShieldingSystem.WallMaterial.Concrete)),
                ("AirlockSystem",
                    () => new AirlockSystem(),
                    s => ((AirlockSystem)s).BuildAirlock()),
                ("NoiseSystem",
                    () => new NoiseSystem(),
                    s => { /* default state */ }),
                ("PetSystem",
                    () => new PetSystem(new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>())),
                    s =>
                    {
                        ((PetSystem)s).AddPet(new PetState
                        {
                            Id = "dog_1",
                            DisplayName = "Ash",
                            Traits = PetTraits.RatCatcher,
                            Hunger = 12f,
                            Thirst = 8f,
                            Radiation = 3f,
                            FurContamination = 1.5f,
                            CurrentRoomId = "quarters",
                            IsAlive = true,
                            OwnerSurvivorId = "sv1",
                            EatsSpoiledMeatOnly = true
                        });
                    }),
            };

            foreach (var (name, ctor, mutate) in cases)
            {
                var sysA = ctor();
                mutate(sysA);
                var save = CaptureState(sysA);
                Assert.IsNotNull(save, $"{name}.CaptureState returned null.");

                var sysB = ctor();
                RestoreState(sysB, save);
                var save2 = CaptureState(sysB);

                AssertDtoEqual(save, save2, name, tolerance: 1e-4f);
            }
        }

        // -------------------------------------------------------------
        // Test 3: New systems in Core/SimulationSystems.cs already covered
        // by SimulationSystems_AllSaveDtos_RoundTripEqual above. Add a
        // dedicated test for ChelationSystem because its save DTO uses
        // a string[]/float[] dictionary that needs careful validation.
        // -------------------------------------------------------------

        [Test]
        public void ChelationSystem_MultipleSurvivors_RoundTripEqual()
        {
            var sysA = new ChelationSystem();
            sysA.BeginChelation("sv1");
            sysA.BeginChelation("sv2");
            sysA.AdvanceDay("sv1"); // sv1 closer to done
            var save = sysA.CaptureState();

            var sysB = new ChelationSystem();
            sysB.RestoreState(save);
            var save2 = sysB.CaptureState();

            AssertDtoEqual(save, save2, "ChelationSystem", tolerance: 1e-4f);
            // Both survivors should still be active (1 day < 5 day duration).
            Assert.Greater(sysB.GetRemainingHours("sv1"), 0f, "sv1 should still be chelated");
            Assert.Greater(sysB.GetRemainingHours("sv2"), 0f, "sv2 should still be chelated");
            // sv1 was advanced by 1 day; sv2 was not. sv1's remaining is 24h less.
            Assert.AreEqual(sysA.GetRemainingHours("sv1"), sysB.GetRemainingHours("sv1"), 0.001f);
            Assert.AreEqual(sysA.GetRemainingHours("sv2"), sysB.GetRemainingHours("sv2"), 0.001f);
        }

        // -------------------------------------------------------------
        // Test 4: PolypharmacySystem uses a jagged array (float[][])
        // — explicitly verify the round-trip works for that shape.
        // -------------------------------------------------------------

        [Test]
        public void PolypharmacySystem_JaggedDoseArray_RoundTripEqual()
        {
            var sysA = new PolypharmacySystem();
            sysA.RecordDose("sv1", "iodine", 0f);
            sysA.RecordDose("sv1", "morphine", 5f);
            sysA.RecordDose("sv2", "anti_rad", 10f);
            var save = (PolypharmSave)sysA.CaptureState();
            Assert.AreEqual(2, save.Keys.Length);
            Assert.AreEqual(2, save.ValuesJagged[0].Length);
            Assert.AreEqual(1, save.ValuesJagged[1].Length);

            var sysB = new PolypharmacySystem();
            sysB.RestoreState(save);
            var save2 = (PolypharmSave)sysB.CaptureState();
            AssertDtoEqual(save, save2, "PolypharmacySystem", tolerance: 1e-4f);
        }

        // -------------------------------------------------------------
        // Test 5: Save DTO null-safety — RestoreState(null) must not throw
        // for any system that documents the contract.
        // -------------------------------------------------------------

        [Test]
        public void AllSystems_RestoreNull_DoesNotThrow()
        {
            var systems = new object[]
            {
                new ResilienceSystem(),
                new CompostSystem(),
                new SterilizationSystem(),
                new ChelationSystem(),
                new WindTurbineSystem(),
                new AntibioticResistanceSystem(),
                new InternalHaulingSystem(),
                new WeaponMaintenanceSystem(),
                new RoomAestheticsSystem(),
                new HamRadioSystem(),
                new TriageBoardSystem(),
                new PolypharmacySystem(),
                new ExcavationSystem(),
                new HiddenStorageSystem(),
                new CeilingCollapseSystem(),
                new PerimeterTrapSystem(),
                new TunnelingSystem(),
                new EscapeHatchSystem(),
                new MaterialShieldingSystem(),
                new AirlockSystem(),
                new NoiseSystem(),
                new FuelDecaySystem(),
                new PetSystem(new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>())),
                new AddictionSystem(),
                new RadioTunerSystem(new System.Random(1)),
            };
            foreach (var sys in systems)
            {
                Assert.DoesNotThrow(() => RestoreState(sys, null), $"{sys.GetType().Name}.RestoreState(null) should be a no-op, not an exception.");
            }
        }

        // -------------------------------------------------------------
        // Test 6: PetSystem + FuelDecaySystem via SaveSystem ISaveable path
        // -------------------------------------------------------------

        [Test]
        public void PetAndFuelDecay_SaveSystemAdapter_RoundTrip()
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_pet_fuel_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                var profile = ScriptableObject.CreateInstance<NeedsProfile>();
                var needs = new NeedsSystem(profile, sv => true);
                var petsA = new PetSystem(needs);
                petsA.AddPet(new PetState
                {
                    Id = "cat_1",
                    DisplayName = "Ember",
                    Hunger = 22f,
                    Thirst = 11f,
                    IsAlive = true,
                    CurrentRoomId = "airlock"
                });
                var fuelA = new FuelDecaySystem();
                fuelA.TickDaily(60);

                SaveSystem Make(PetSystem pets, FuelDecaySystem fuel)
                {
                    var weather = new WeatherSystem(null, 7);
                    var temp = new TemperatureSystem(null, weather);
                    var rad = new RadiationSystem(needs);
                    var ss = new SaveSystem(new SaveSystem.CoreDeps
                    {
                        GameState = new GameState(),
                        WeatherSystem = weather,
                        TemperatureSystem = temp,
                        NeedsSystem = needs,
                        RadiationSystem = rad,
                        Shelter = new ShelterClass(),
                        GetSurvivors = () => new List<Survivor>(),
                        ItemLookup = id => null,
                        ModuleLookup = id => null,
                        SavesDir = dir
                    });
                    ss.SetPetSystem(pets);
                    ss.SetFuelDecaySystem(fuel);
                    return ss;
                }

                var writer = Make(petsA, fuelA);
                Assert.IsTrue(writer.Save("pet_fuel_slot"));

                var petsB = new PetSystem(new NeedsSystem(profile, sv => true));
                var fuelB = new FuelDecaySystem();
                var reader = Make(petsB, fuelB);
                Assert.IsTrue(reader.Load("pet_fuel_slot"));

                AssertDtoEqual(petsA.CaptureState(), petsB.CaptureState(), "pets_via_save_system");
                AssertDtoEqual(fuelA.CaptureState(), fuelB.CaptureState(), "fuel_via_save_system");
                Assert.AreEqual(1, petsB.Pets.Count);
                Assert.AreEqual("cat_1", petsB.Pets[0].Id);
                Assert.AreEqual(0.80f, fuelB.State.fuelEfficiencyMultiplier, 0.001f);
            }
            finally
            {
                try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { /* best-effort */ }
            }
        }

        // -------------------------------------------------------------
        // Test 7: Top ISaveable gaps — radio_tuner, addiction, keepsakes
        // -------------------------------------------------------------

        [Test]
        public void RadioTunerSystem_CaptureRestore_RoundTripEqual()
        {
            var sysA = new RadioTunerSystem(new System.Random(42));
            sysA.State.AvailableFuel = 17.5f;
            sysA.State.SignalStrength = 0.65f;
            sysA.State.EmpDamage = 0.2f;
            sysA.State.CurrentFrequencyId = "freq_civilian";
            sysA.State.TuningProgress = 0.4f;
            sysA.State.TuningHoursSpent = 1.25f;
            // Inject extracted intel via restore of a partial save then re-capture.
            var seed = sysA.CaptureState();
            seed.ExtractedIntel = new List<IntelNode>
            {
                new IntelNode
                {
                    Id = "intel_1",
                    Type = IntelType.LootLocation,
                    SourceFrequencyId = "freq_civilian",
                    ExtractedDay = 3,
                    ExpirationDay = 10,
                    TargetLocationId = "node_ruins",
                    Confidence = 0.8f,
                    NumericValue = 0.5f,
                    Text = "Supplies under the overpass.",
                    IsConsumed = false
                }
            };
            sysA.RestoreState(seed);

            var save = sysA.CaptureState();
            var sysB = new RadioTunerSystem(new System.Random(99));
            sysB.RestoreState(save);
            AssertDtoEqual(save, sysB.CaptureState(), "RadioTunerSystem");
            Assert.AreEqual(17.5f, sysB.State.AvailableFuel, 0.001f);
            Assert.AreEqual("freq_civilian", sysB.State.CurrentFrequencyId);
            Assert.AreEqual(1, sysB.ExtractedIntel.Count);
            Assert.AreEqual("intel_1", sysB.ExtractedIntel[0].Id);
        }

        [Test]
        public void AddictionAndRadioTuner_SaveSystemAdapter_RoundTrip()
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_addict_radio_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                var profile = ScriptableObject.CreateInstance<NeedsProfile>();
                var needs = new NeedsSystem(profile, sv => true);
                var weather = new WeatherSystem(null, 3);
                var temp = new TemperatureSystem(null, weather);
                var rad = new RadiationSystem(needs);

                var addictA = new AddictionSystem(new System.Random(1));
                addictA.RestoreState(new AddictionSave
                {
                    Keys = new[] { "sv1", "sv2" },
                    Values = new[] { 48f, 200f }
                });
                var radioA = new RadioTunerSystem(new System.Random(2));
                radioA.State.AvailableFuel = 9f;
                radioA.State.CurrentFrequencyId = "freq_mil";
                radioA.State.TuningProgress = 0.9f;

                var writer = new SaveSystem(new SaveSystem.CoreDeps
                {
                    GameState = new GameState(),
                    WeatherSystem = weather,
                    TemperatureSystem = temp,
                    NeedsSystem = needs,
                    RadiationSystem = rad,
                    Shelter = new ShelterClass(),
                    GetSurvivors = () => new List<Survivor>(),
                    ItemLookup = id => null,
                    ModuleLookup = id => null,
                    SavesDir = dir
                });
                writer.SetAddictionSystem(addictA);
                writer.SetRadioTunerSystem(radioA);
                Assert.IsTrue(writer.Save("gap_slot"));

                var addictB = new AddictionSystem();
                var radioB = new RadioTunerSystem();
                var reader = new SaveSystem(new SaveSystem.CoreDeps
                {
                    GameState = new GameState(),
                    WeatherSystem = new WeatherSystem(null, 3),
                    TemperatureSystem = new TemperatureSystem(null, weather),
                    NeedsSystem = needs,
                    RadiationSystem = rad,
                    Shelter = new ShelterClass(),
                    GetSurvivors = () => new List<Survivor>(),
                    ItemLookup = id => null,
                    ModuleLookup = id => null,
                    SavesDir = dir
                });
                reader.SetAddictionSystem(addictB);
                reader.SetRadioTunerSystem(radioB);
                Assert.IsTrue(reader.Load("gap_slot"));

                Assert.AreEqual(48f, addictB.GetRecoveryHours("sv1"), 0.001f);
                Assert.AreEqual(200f, addictB.GetRecoveryHours("sv2"), 0.001f);
                Assert.AreEqual(9f, radioB.State.AvailableFuel, 0.001f);
                Assert.AreEqual("freq_mil", radioB.State.CurrentFrequencyId);
                Assert.AreEqual(0.9f, radioB.State.TuningProgress, 0.001f);
            }
            finally
            {
                try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { /* best-effort */ }
            }
        }

        [Test]
        public void SurvivorKeepsakeItemIds_SaveLoad_RoundTrip()
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_keepsake_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                var profile = ScriptableObject.CreateInstance<NeedsProfile>();
                var needs = new NeedsSystem(profile, sv => true);
                var weather = new WeatherSystem(null, 5);
                var survivors = new List<Survivor>
                {
                    new Survivor
                    {
                        Id = "sv_keep",
                        DisplayName = "Mara",
                        State = SurvivorState.Idle,
                        KeepsakeItemIds = new List<string> { "item_watch", "item_ring" }
                    }
                };

                var writer = new SaveSystem(new SaveSystem.CoreDeps
                {
                    GameState = new GameState(),
                    WeatherSystem = weather,
                    TemperatureSystem = new TemperatureSystem(null, weather),
                    NeedsSystem = needs,
                    RadiationSystem = new RadiationSystem(needs),
                    Shelter = new ShelterClass(),
                    GetSurvivors = () => survivors,
                    ItemLookup = id => null,
                    ModuleLookup = id => null,
                    SavesDir = dir
                });
                Assert.IsTrue(writer.Save("keepsake_slot"));

                var survivorsB = new List<Survivor>
                {
                    new Survivor
                    {
                        Id = "sv_keep",
                        DisplayName = "Mara",
                        State = SurvivorState.Idle,
                        KeepsakeItemIds = new List<string>()
                    }
                };
                var reader = new SaveSystem(new SaveSystem.CoreDeps
                {
                    GameState = new GameState(),
                    WeatherSystem = weather,
                    TemperatureSystem = new TemperatureSystem(null, weather),
                    NeedsSystem = needs,
                    RadiationSystem = new RadiationSystem(needs),
                    Shelter = new ShelterClass(),
                    GetSurvivors = () => survivorsB,
                    ItemLookup = id => null,
                    ModuleLookup = id => null,
                    SavesDir = dir
                });
                Assert.IsTrue(reader.Load("keepsake_slot"));
                Assert.IsNotNull(survivorsB[0].KeepsakeItemIds);
                Assert.AreEqual(2, survivorsB[0].KeepsakeItemIds.Count);
                Assert.Contains("item_watch", survivorsB[0].KeepsakeItemIds);
                Assert.Contains("item_ring", survivorsB[0].KeepsakeItemIds);
            }
            finally
            {
                try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { /* best-effort */ }
            }
        }
    }
}
