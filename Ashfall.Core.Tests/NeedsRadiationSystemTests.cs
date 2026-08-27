using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Ashfall.Core.Survivors;
using Xunit;
using WornGear = Ashfall.Core.Inventory.WornGear;

namespace Ashfall.Core.Tests
{
    public class NeedsSystemTests
    {
        [Fact]
        public void Tick_AdvancesHungerThirstFatigue()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1" };
            sys.Register(s);
            sys.Tick(24f);
            Assert.True(s.Hunger > 0f);
            Assert.True(s.Thirst > 0f);
            Assert.True(s.Fatigue > 0f);
            // Warmth decays in the cold (no heat hook).
            Assert.True(s.Warmth < 100f);
        }

        [Fact]
        public void Tick_NearHeatSource_RestoresWarmth()
        {
            var sys = new NeedsSystem(null, _ => true);
            var s = new SurvivorNeedsState { Id = "sv1", Warmth = 40f };
            sys.Register(s);
            sys.Tick(10f);
            Assert.True(s.Warmth > 40f);
        }

        [Fact]
        public void CriticalHunger_LosesHealthAndFiresEvent()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1", Hunger = 95f, Health = 100f };
            bool critical = false;
            sys.OnNeedCritical += (_, kind) => { if (kind == NeedKind.Hunger) critical = true; };
            sys.Register(s);
            sys.Tick(2f);
            Assert.True(critical);
            Assert.True(s.Health < 100f); // starving hurts
        }

        [Fact]
        public void Modify_ClampsToCapAnd100()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1" };
            sys.Modify(s, NeedKind.Morale, -999f);
            Assert.Equal(0f, s.Morale);
            sys.Modify(s, NeedKind.Hunger, 999f);
            Assert.Equal(100f, s.Hunger);
            sys.Modify(s, NeedKind.Health, -5f);
            Assert.Equal(95f, s.Health);
        }

        [Fact]
        public void HealthZero_FiresDied()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1", Health = 5f };
            bool died = false;
            sys.OnDied += _ => died = true;
            sys.SetHealth(s, -1f);
            Assert.True(died);
            Assert.True(s.IsDead);
        }

        [Fact]
        public void TryDeferDeath_GatesDeathAtZero()
        {
            var sys = new NeedsSystem();
            sys.TryDeferDeath = _ => true;
            var s = new SurvivorNeedsState { Id = "sv1", Health = 1f };
            bool died = false;
            sys.OnDied += _ => died = true;
            sys.Modify(s, NeedKind.Health, -10f);
            Assert.False(died);
            Assert.False(s.IsDead);
            Assert.Equal(0f, s.Health);
        }
    }

    public class RadiationSystemTests
    {
        private static SurvivorRadState Sv(string id) => new SurvivorRadState { Id = id };

        [Fact]
        public void Expose_AccumulatesDoseAndLifetime()
        {
            var sys = new RadiationSystem();
            var s = Sv("sv1");
            sys.Register(s);
            sys.Expose(s, 10f, 2f);
            Assert.Equal(20f, s.LifetimeRadiationExposure, 3);
            Assert.Equal(20f, s.RadiationDose, 3);
        }

        [Fact]
        public void Expose_AcuteThreshold_GrantsStatusAndDamagesHealth()
        {
            float healthDelta = 0f;
            var sys = new RadiationSystem(applyNeed: (s, need, d) => { if (need == "health") healthDelta = d; });
            var s = Sv("sv1");
            sys.Expose(s, 90f, 1f); // 90 ≥ 80 → acute
            Assert.True(s.HasAcuteRadiationSickness);
            Assert.True(healthDelta < 0f);
        }

        [Fact]
        public void ChronicThreshold_OnLifetime_GrantsChronic()
        {
            var sys = new RadiationSystem();
            var s = Sv("sv1");
            sys.SeedLifetimeExposure(s, 450f);
            Assert.True(s.HasChronicIllness);
        }

        [Fact]
        public void AdministerIodine_GrantsTimedResistance_ThatExpires()
        {
            var sys = new RadiationSystem();
            var s = Sv("sv1");
            sys.Register(s);
            sys.AdministerIodine(s);
            Assert.True(s.HasRadResistance);
            Assert.Equal(RadiationSystem.IodineResistanceHours, s.RadResistanceHoursRemaining, 3);
            sys.Tick(2f);   // 2h < 6h window: still active
            Assert.True(s.HasRadResistance);
            Assert.Equal(4f, s.RadResistanceHoursRemaining, 3);
            sys.Tick(20f);  // 22h total > 6h: expired
            Assert.False(s.HasRadResistance);
            Assert.Equal(0f, s.RadResistanceHoursRemaining, 3);
        }

        [Fact]
        public void AdministerAntiRad_LowersDose_KeepsLifetime()
        {
            var sys = new RadiationSystem();
            var s = Sv("sv1");
            sys.Expose(s, 50f, 1f);
            sys.AdministerAntiRad(s, 20f);
            Assert.Equal(30f, s.RadiationDose, 3);
            Assert.Equal(50f, s.LifetimeRadiationExposure, 3); // lifetime untouched
        }

        [Fact]
        public void Tick_WithContext_AppliesGearProtection()
        {
            var gear = new WornGear { RadProtection = 30f, MaxDurability = 100f, CurrentDurability = 100f };
            var sys = new RadiationSystem(exposureContext: s => new ExposureContext
            {
                ZoneRadLevel = 50f,
                WornGear = new System.Collections.Generic.List<WornGear> { gear }
            });
            var s = Sv("sv1");
            sys.Register(s);
            sys.Tick(1f);
            // exposure = 50 - 30 = 20 mSv/hr * 1h
            Assert.Equal(20f, s.LifetimeRadiationExposure, 2);
        }

        [Fact]
        public void Tick_Paused_AccumulatesNothing()
        {
            var sys = new RadiationSystem(exposureContext: _ => new ExposureContext { ZoneRadLevel = 50f });
            sys.IsPaused = true;
            var s = Sv("sv1");
            sys.Register(s);
            sys.Tick(5f);
            Assert.Equal(0f, s.LifetimeRadiationExposure);
        }

        [Fact]
        public void GearProtection_ScalesWithDurability()
        {
            var gear = new WornGear { RadProtection = 40f, MaxDurability = 100f, CurrentDurability = 50f };
            Assert.Equal(20f, gear.EffectiveProtection(), 3);
            // ComputeGearProtection sums effective (already durability-scaled) protection.
            Assert.Equal(20f, RadiationSystem.ComputeGearProtection(
                new System.Collections.Generic.List<WornGear> { gear }), 3);
        }

        [Fact]
        public void MathfCompat_MirrorsUnitySemantics()
        {
            Assert.Equal(0f, MathfCompat.Clamp01(-1f));
            Assert.Equal(1f, MathfCompat.Clamp01(2f));
            Assert.Equal(0.5f, MathfCompat.Clamp01(0.5f));
            Assert.Equal(5f, MathfCompat.Clamp(99f, 0f, 5f));
            Assert.Equal(1.5f, MathfCompat.Lerp(1f, 2f, 0.5f), 3);
            Assert.True(MathfCompat.Approximately(0.1f + 0.2f, 0.3f));
        }
    }

    /// <summary>
    /// H10 hardening: NeedsSystem / RadiationSystem state must survive a
    /// cross-host JSON round-trip losslessly (Invariant 3). The state IS the
    /// save unit (no separate CaptureState), so a field added to the DTO without
    /// being serialized would silently drop — these gate the mutation-sensitive
    /// fields, not just defaults.
    /// </summary>
    public class SaveRoundTripTests
    {
        [Fact]
        public void SurvivorNeedsState_RoundTrips_MutatedValues()
        {
            var json = new SystemTextJsonSerializer();
            var original = new SurvivorNeedsState
            {
                Id = "sv_farmer",
                Hunger = 77.5f,
                Thirst = 66.25f,
                Fatigue = 45f,
                Warmth = 30f,
                Morale = 12f,
                Health = 55f,
                Hygiene = 4f,
                WasHungerCritical = true,
                WasThirstCritical = false,
                WasWarmthCritical = true,
                MaxHealthCap = 70f,
                IsAlive = true,
                IsDead = false
            };

            var restored = json.Deserialize<SurvivorNeedsState>(json.Serialize(original));
            Assert.NotNull(restored);
            Assert.Equal(original.Id, restored.Id);
            Assert.Equal(original.Hunger, restored.Hunger, 3);
            Assert.Equal(original.Thirst, restored.Thirst, 3);
            Assert.Equal(original.Fatigue, restored.Fatigue, 3);
            Assert.Equal(original.Warmth, restored.Warmth, 3);
            Assert.Equal(original.Morale, restored.Morale, 3);
            Assert.Equal(original.Health, restored.Health, 3);
            Assert.Equal(original.Hygiene, restored.Hygiene, 3);
            Assert.Equal(original.WasHungerCritical, restored.WasHungerCritical);
            Assert.Equal(original.WasWarmthCritical, restored.WasWarmthCritical);
            Assert.Equal(original.MaxHealthCap, restored.MaxHealthCap, 3);
            Assert.Equal(original.IsAliveState, restored.IsAliveState);
        }

        [Fact]
        public void SurvivorRadState_RoundTrips_MutatedValues()
        {
            var json = new SystemTextJsonSerializer();
            var original = new SurvivorRadState
            {
                Id = "sv_geiger",
                RadiationDose = 92f,
                LifetimeRadiationExposure = 505f,
                HasRadResistance = true,
                RadResistanceHoursRemaining = 3.5f,
                IodineProtectionTimer = 1.25f,
                HasAcuteRadiationSickness = true,
                HasChronicIllness = true,
                HasAcuteRadiationSyndrome = false,
                IsAlive = true
            };

            var restored = json.Deserialize<SurvivorRadState>(json.Serialize(original));
            Assert.NotNull(restored);
            Assert.Equal(original.Id, restored.Id);
            Assert.Equal(original.RadiationDose, restored.RadiationDose, 3);
            Assert.Equal(original.LifetimeRadiationExposure, restored.LifetimeRadiationExposure, 3);
            Assert.Equal(original.HasRadResistance, restored.HasRadResistance);
            Assert.Equal(original.RadResistanceHoursRemaining, restored.RadResistanceHoursRemaining, 3);
            Assert.Equal(original.IodineProtectionTimer, restored.IodineProtectionTimer, 3);
            Assert.Equal(original.HasAcuteRadiationSickness, restored.HasAcuteRadiationSickness);
            Assert.Equal(original.HasChronicIllness, restored.HasChronicIllness);
            Assert.Equal(original.IsAlive, restored.IsAlive);
        }

        [Fact]
        public void RadiationSystem_RegisteredDoseSurvivesRoundTrip()
        {
            var sys = new RadiationSystem();
            var s = new SurvivorRadState { Id = "sv_probe" };
            sys.Expose(s, 60f, 1f);
            Assert.True(s.RadiationDose > 0f);

            var json = new SystemTextJsonSerializer();
            var restored = json.Deserialize<SurvivorRadState>(json.Serialize(s));
            Assert.NotNull(restored);
            Assert.Equal(s.RadiationDose, restored.RadiationDose, 3);
            Assert.Equal(s.LifetimeRadiationExposure, restored.LifetimeRadiationExposure, 3);
        }
    }

    /// <summary>
    /// Registry gate for the equipped-gear → radiation bridge (AGENTS H2). The
    /// Godot host session currently does NOT populate ExposureContext.WornGear,
    /// so gear protection must still be *enforced at the Core contract*: when a
    /// host supplies worn gear, dose must drop — otherwise a future host wiring
    /// it will silently pass zero just like today.
    /// </summary>
    public class GearProtectionBridgeTests
    {
        private static SurvivorRadState Exposed2hWithGear(List<Ashfall.Core.Inventory.WornGear> worn)
        {
            var sys = new RadiationSystem(exposureContext: _ => new ExposureContext
            {
                ZoneRadLevel = 50f,
                ShelterShielding = 0f,
                WornGear = worn
            });
            var survivor = new SurvivorRadState { Id = "sv_gear", IsAlive = true };
            sys.Register(survivor);
            sys.Tick(2f);
            return survivor;
        }

        [Fact]
        public void EquippedGear_ReducesDose_BelowAcuteThreshold()
        {
            // No gear: zone 50 rad/hr x 2h → ~100 dose → acute sickness.
            var bare = Exposed2hWithGear(new List<WornGear>());
            Assert.True(bare.RadiationDose >= RadiationSystem.AcuteThreshold,
                $"bare exposure should reach acute; dose={bare.RadiationDose}");
            Assert.True(bare.HasAcuteRadiationSickness);

            // Full prot hazmat: effective 40 protection → exposure 10/hr x2h = 20.
            var geared = Exposed2hWithGear(new List<WornGear>
            {
                new WornGear { RadProtection = 40f, MaxDurability = 100f, CurrentDurability = 100f, DegradeRate = 0f }
            });
            Assert.True(geared.RadiationDose < RadiationSystem.AcuteThreshold,
                $"gear should keep dose below acute; dose={geared.RadiationDose}");
            Assert.False(geared.HasAcuteRadiationSickness);
        }

        [Fact]
        public void DegradedGear_ProvidesProportionalProtection()
        {
            // 50% durability → effective protection = half of radProtection.
            var geared = Exposed2hWithGear(new List<WornGear>
            {
                new WornGear { RadProtection = 40f, MaxDurability = 100f, CurrentDurability = 50f, DegradeRate = 0f }
            });
            // exposurePerHour = 50 - (40*0.5) = 30 → 2h = 60, below acute but higher than full gear.
            Assert.True(geared.RadiationDose < RadiationSystem.AcuteThreshold);
            Assert.True(geared.RadiationDose > 30f, $"degraded gear protects less; dose={geared.RadiationDose}");
        }

        // FromInventory method removed during consolidation - Radiation now uses
        // Inventory.WornGear directly, so no conversion is needed

        [Fact]
        public void OldSave_Deserialization_Equivalence()
        {
            // Old saves serialized Radiation.WornGear with fields: RadProtection, MaxDurability,
            // CurrentDurability, DegradeRate. Since Radiation.WornGear inherited from Inventory.WornGear
            // with no additional fields, the serialized JSON is identical.
            // This test verifies that deserializing old save data works correctly.
            var serializer = new SystemTextJsonSerializer();

            // Simulate old save data that had Radiation.WornGear
            string oldSaveJson = "{\"RadProtection\":50.0,\"MaxDurability\":200.0,\"CurrentDurability\":150.0,\"DegradeRate\":1.5}";

            var deserialized = serializer.Deserialize<Ashfall.Core.Inventory.WornGear>(oldSaveJson);
            Assert.NotNull(deserialized);
            Assert.Equal(50f, deserialized.RadProtection, 3);
            Assert.Equal(200f, deserialized.MaxDurability, 3);
            Assert.Equal(150f, deserialized.CurrentDurability, 3);
            Assert.Equal(1.5f, deserialized.DegradeRate, 3);
            Assert.Equal(0.75f, deserialized.DurabilityFraction(), 3);
            Assert.Equal(37.5f, deserialized.EffectiveProtection(), 3);
        }

        [Fact]
        public void DoseCalculation_Equivalence_WithConsolidatedWornGear()
        {
            // Verify that dose calculation produces the same results with the consolidated
            // Inventory.WornGear as it did with the old Radiation.WornGear
            var worn = new List<Ashfall.Core.Inventory.WornGear>
            {
                new Ashfall.Core.Inventory.WornGear
                {
                    RadProtection = 30f,
                    MaxDurability = 100f,
                    CurrentDurability = 100f,
                    DegradeRate = 0f
                },
                new Ashfall.Core.Inventory.WornGear
                {
                    RadProtection = 80f,
                    MaxDurability = 100f,
                    CurrentDurability = 50f,  // 50% durability = 50% protection
                    DegradeRate = 0f
                }
            };

            float totalProtection = RadiationSystem.ComputeGearProtection(worn);
            // 30 + (80 * 0.5) = 30 + 40 = 70
            Assert.Equal(70f, totalProtection, 3);

            // Test with degraded gear
            worn[0].CurrentDurability = 50f; // First gear now at 50%
            worn[1].CurrentDurability = 100f; // Second gear back to 100%
            totalProtection = RadiationSystem.ComputeGearProtection(worn);
            // (30 * 0.5) + 80 = 15 + 80 = 95
            Assert.Equal(95f, totalProtection, 3);
        }
    }

    /// <summary>
    /// Host bridge gate: equipped inventory gear (gas mask 30 / hazmat 80 per
    /// items.json authority) must flow through the Inventory → Radiation WornGear
    /// conversion and measurably reduce dose — the exact path the Godot host
    /// SurvivorsHostSession now wires.
    /// </summary>
    public class InventoryGearBridgeTests
    {
        private static Ashfall.Core.Inventory.ItemDefinition GearDef(
            string id, Ashfall.Core.Inventory.EquipSlot slot, float radProtection)
        {
            return new Ashfall.Core.Inventory.ItemDefinition
            {
                id = id,
                displayName = id,
                type = Ashfall.Core.Inventory.ItemType.Protective,
                stackMax = 1,
                weight = 1f,
                radProtection = radProtection,
                durability = 100f,
                isEquipable = true,
                equipSlot = slot
            };
        }

        private static List<Ashfall.Core.Inventory.WornGear> EquippedGear(params Ashfall.Core.Inventory.ItemDefinition[] defs)
        {
            var inventory = new Ashfall.Core.Inventory.Inventory();
            foreach (var def in defs)
            {
                Assert.True(inventory.Add(def, 1), $"could not add {def.id}");
                Assert.True(inventory.Equip(def), $"could not equip {def.id}");
            }
            var buffer = new List<Ashfall.Core.Inventory.WornGear>();
            inventory.FillWornGear(buffer);
            return buffer;
        }

        [Fact]
        public void EquippedInventoryGear_SumsToAuthorityProtection()
        {
            var worn = EquippedGear(
                GearDef("gas_mask", Ashfall.Core.Inventory.EquipSlot.Face, 30f),
                GearDef("hazmat_suit", Ashfall.Core.Inventory.EquipSlot.Body, 80f));
            Assert.Equal(2, worn.Count);
            Assert.Equal(110f, RadiationSystem.ComputeGearProtection(worn), 3);
        }

        [Fact]
        public void EquippedInventoryGear_ReducesExposureBelowAcute()
        {
            var worn = EquippedGear(GearDef("hazmat_suit", Ashfall.Core.Inventory.EquipSlot.Body, 80f));
            var sys = new RadiationSystem(exposureContext: _ => new ExposureContext
            {
                ZoneRadLevel = 50f,
                ShelterShielding = 0f,
                WornGear = worn
            });
            var survivor = new SurvivorRadState { Id = "sv_geared", IsAlive = true };
            sys.Register(survivor);
            sys.Tick(2f);
            // exposure = 50 - 80 = 0 mSv/hr
            Assert.Equal(0f, survivor.LifetimeRadiationExposure, 3);
            Assert.False(survivor.HasAcuteRadiationSickness);
        }

        [Fact]
        public void NoEquippedGear_StillReachesAcute()
        {
            var sys = new RadiationSystem(exposureContext: _ => new ExposureContext
            {
                ZoneRadLevel = 50f,
                ShelterShielding = 0f,
                WornGear = new List<WornGear>()
            });
            var survivor = new SurvivorRadState { Id = "sv_bare", IsAlive = true };
            sys.Register(survivor);
            sys.Tick(2f);
            Assert.True(survivor.RadiationDose >= RadiationSystem.AcuteThreshold,
                $"bare survivor should reach acute; dose={survivor.RadiationDose}");
            Assert.True(survivor.HasAcuteRadiationSickness);
        }

        [Fact]
        public void SaveLoad_NeedsState_RoundTrip_PreservesAllFields()
        {
            var serializer = new SystemTextJsonSerializer();
            var original = new SurvivorNeedsState
            {
                Id = "survivor_roundtrip_test",
                Hunger = 42.5f,
                Thirst = 33.1f,
                Fatigue = 60.0f,
                Warmth = 85.2f,
                Morale = 70.4f,
                Health = 92.0f,
                Hygiene = 55.5f,
                IsAlive = true,
                IsDead = false
            };

            string json = serializer.Serialize(original);
            Assert.False(string.IsNullOrEmpty(json));

            var restored = serializer.Deserialize<SurvivorNeedsState>(json);
            Assert.NotNull(restored);
            Assert.Equal(original.Id, restored!.Id);
            Assert.Equal(original.Hunger, restored.Hunger, 2);
            Assert.Equal(original.Thirst, restored.Thirst, 2);
            Assert.Equal(original.Fatigue, restored.Fatigue, 2);
            Assert.Equal(original.Warmth, restored.Warmth, 2);
            Assert.Equal(original.Morale, restored.Morale, 2);
            Assert.Equal(original.Health, restored.Health, 2);
            Assert.Equal(original.Hygiene, restored.Hygiene, 2);
            Assert.Equal(original.IsAlive, restored.IsAlive);
            Assert.Equal(original.IsDead, restored.IsDead);
        }

        [Fact]
        public void SaveLoad_RadState_RoundTrip_PreservesAllFields()
        {
            var serializer = new SystemTextJsonSerializer();
            var original = new SurvivorRadState
            {
                Id = "rad_roundtrip_test",
                RadiationDose = 25.5f,
                LifetimeRadiationExposure = 120.0f,
                HasRadResistance = true,
                RadResistanceHoursRemaining = 18.5f,
                HasAcuteRadiationSickness = false,
                HasChronicIllness = true,
                IsAlive = true
            };

            string json = serializer.Serialize(original);
            Assert.False(string.IsNullOrEmpty(json));

            var restored = serializer.Deserialize<SurvivorRadState>(json);
            Assert.NotNull(restored);
            Assert.Equal(original.Id, restored!.Id);
            Assert.Equal(original.RadiationDose, restored.RadiationDose, 2);
            Assert.Equal(original.LifetimeRadiationExposure, restored.LifetimeRadiationExposure, 2);
            Assert.Equal(original.HasRadResistance, restored.HasRadResistance);
            Assert.Equal(original.RadResistanceHoursRemaining, restored.RadResistanceHoursRemaining, 2);
            Assert.Equal(original.HasAcuteRadiationSickness, restored.HasAcuteRadiationSickness);
            Assert.Equal(original.HasChronicIllness, restored.HasChronicIllness);
            Assert.Equal(original.IsAlive, restored.IsAlive);
        }
    }
}
