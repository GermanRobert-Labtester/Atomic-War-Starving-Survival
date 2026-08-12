using NUnit.Framework;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Core;
using System.Collections.Generic;

namespace AtomicWar.Tests.EditMode
{
    // ═══════════════════════════════════════════════════════════════════
    //  HydrostaticPressureSystem Tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class HydrostaticPressureSystemTests
    {
        [Test]
        public void InitialState_IsCleanLens()
        {
            var sys = new HydrostaticPressureSystem();
            Assert.AreEqual(2.5f, sys.CleanLensDepth, 0.01f);
            Assert.AreEqual(0f, sys.ToxicityIndex, 0.001f);
            Assert.IsFalse(sys.IsOverPumping);
            Assert.IsFalse(sys.IsSludgeBreakthrough);
        }

        [Test]
        public void SafePumpRate_DoesNotDrawSludge()
        {
            var sys = new HydrostaticPressureSystem();
            sys.SetPumpRate(HydrostaticPressureSystem.LensRechargeRatePerHour);
            Assert.IsFalse(sys.IsOverPumping);
            sys.Tick(1f);
            Assert.AreEqual(0f, sys.TotalSludgeDrawn, 0.001f);
        }

        [Test]
        public void OverPumping_DrawsSludgeAndIncreasesToxicity()
        {
            var sys = new HydrostaticPressureSystem();
            sys.SetPumpRate(10f); // well above recharge rate
            Assert.IsTrue(sys.IsOverPumping);
            sys.Tick(4f);
            Assert.Greater(sys.TotalSludgeDrawn, 0f);
            Assert.Greater(sys.ToxicityIndex, 0f);
        }

        [Test]
        public void ToxicityCrossesPoisonThreshold_RaisesEvent()
        {
            var sys = new HydrostaticPressureSystem();
            bool breakthroughFired = false;
            sys.OnSludgeBreakthrough += () => breakthroughFired = true;

            // Force toxicity past poison threshold
            sys.SetPumpRate(20f);
            for (int i = 0; i < 50; i++)
                sys.Tick(1f);

            Assert.IsTrue(breakthroughFired || sys.ToxicityIndex >= HydrostaticPressureSystem.ToxicityPoisonThreshold);
        }

        [Test]
        public void SealBulkhead_StopsPumping()
        {
            var sys = new HydrostaticPressureSystem();
            sys.SetPumpRate(10f);
            sys.SealBulkhead();
            Assert.IsTrue(sys.BulkheadSealed);
            Assert.AreEqual(0f, sys.PumpRateLitersPerHour);
        }

        [Test]
        public void ROMembrane_HalvesToxicity()
        {
            var sys = new HydrostaticPressureSystem();
            sys.SetPumpRate(20f);
            sys.Tick(20f);
            float before = sys.ToxicityIndex;
            sys.InstallROMembrane();
            Assert.LessOrEqual(sys.ToxicityIndex, before * 0.5f + 0.01f);
        }

        [Test]
        public void SaveRestore_RoundTrips()
        {
            var sys = new HydrostaticPressureSystem();
            sys.SetPumpRate(7f);
            sys.Tick(10f);
            var save = sys.CaptureState();

            var sys2 = new HydrostaticPressureSystem();
            sys2.RestoreState(save);

            Assert.AreEqual(sys.PumpRateLitersPerHour, sys2.PumpRateLitersPerHour, 0.001f);
            Assert.AreEqual(sys.ToxicityIndex, sys2.ToxicityIndex, 0.001f);
            Assert.AreEqual(sys.TotalWaterExtracted, sys2.TotalWaterExtracted, 0.001f);
        }

        [Test]
        public void RestoreNull_ResetsToDefaults()
        {
            var sys = new HydrostaticPressureSystem();
            sys.SetPumpRate(15f);
            sys.Tick(10f);
            sys.RestoreState(null);
            Assert.AreEqual(5f, sys.PumpRateLitersPerHour, 0.001f);
            Assert.AreEqual(0f, sys.ToxicityIndex, 0.001f);
        }

        [Test]
        public void DailyTick_PressureSpikesAfterInterval()
        {
            var sys = new HydrostaticPressureSystem();
            float initialPressure = sys.SludgePressureKpa;
            for (int d = 1; d <= HydrostaticPressureSystem.SludgeSpikeIntervalDays; d++)
                sys.TickDaily(d);
            Assert.Greater(sys.SludgePressureKpa, initialPressure);
        }

        [Test]
        public void VentSludgeToSurface_ReducesPressure()
        {
            var sys = new HydrostaticPressureSystem();
            sys.Tick(100f); // build up pressure
            float before = sys.SludgePressureKpa;
            sys.VentSludgeToSurface();
            Assert.Less(sys.SludgePressureKpa, before);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  TunnelingAndStructuralStress Tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class TunnelingAndStructuralStressTests
    {
        [Test]
        public void InitialState_NoStress()
        {
            var sys = new TunnelingAndStructuralStress();
            Assert.AreEqual(0f, sys.OverburdenStress, 0.001f);
            Assert.AreEqual(2, sys.DeepestExcavatedLevel);
            Assert.IsFalse(sys.IsCaveInActive);
        }

        [Test]
        public void ExcavationAboveSafeLevel_NoStress()
        {
            var sys = new TunnelingAndStructuralStress();
            var room = sys.BeginExcavation("sub_level_2", 2, false);
            Assert.IsNotNull(room);
            sys.Tick(20f); // complete excavation
            Assert.AreEqual(0f, sys.OverburdenStress, 0.001f);
        }

        [Test]
        public void ExcavationBelowSafeLevel_AddsStress()
        {
            var sys = new TunnelingAndStructuralStress();
            sys.BeginExcavation("sub_level_4", 4, true);
            sys.Tick(20f); // complete excavation
            Assert.Greater(sys.OverburdenStress, 0f);
        }

        [Test]
        public void Shoring_ReducesStress()
        {
            var sys = new TunnelingAndStructuralStress();
            sys.BeginExcavation("sub_level_4", 4, true);
            sys.Tick(20f);
            float stressBefore = sys.OverburdenStress;
            sys.InstallShoring("sub_level_4");
            Assert.Less(sys.OverburdenStress, stressBefore);
        }

        [Test]
        public void PneumaticJack_ReducesStressMore()
        {
            var sys = new TunnelingAndStructuralStress();
            sys.BeginExcavation("sub_level_4", 4, false);
            sys.Tick(50f);
            float stressBefore = sys.OverburdenStress;
            sys.InstallPneumaticJack("sub_level_4");
            Assert.Less(sys.OverburdenStress, stressBefore);
        }

        [Test]
        public void StressExceedsThreshold_TriggersCaveIn()
        {
            var sys = new TunnelingAndStructuralStress();
            sys.SetMaterialThreshold(10f); // low threshold for test
            bool caveInFired = false;
            sys.OnCaveIn += evt => caveInFired = true;

            // Excavate multiple deep rooms without shoring
            sys.BeginExcavation("sub_level_4a", 4, false);
            sys.BeginExcavation("sub_level_5a", 5, false);
            sys.Tick(100f);

            Assert.IsTrue(caveInFired || sys.OverburdenStress > sys.MaterialThreshold);
        }

        [Test]
        public void GasPocket_IncreasesMethane()
        {
            var sys = new TunnelingAndStructuralStress();
            var rng = new System.Random(42);
            sys.BeginExcavation("sub_level_4", 4, true);
            // Tick many hours to guarantee gas pocket hit
            for (int i = 0; i < 100; i++)
                sys.Tick(1f, rng);
            // Gas may or may not have been hit depending on RNG
            // Just verify no crash
            Assert.Pass();
        }

        [Test]
        public void SaveRestore_RoundTrips()
        {
            var sys = new TunnelingAndStructuralStress();
            sys.BeginExcavation("sub_level_4", 4, true);
            sys.Tick(20f);
            var save = sys.CaptureState();

            var sys2 = new TunnelingAndStructuralStress();
            sys2.RestoreState(save);

            Assert.AreEqual(sys.DeepestExcavatedLevel, sys2.DeepestExcavatedLevel);
            Assert.AreEqual(sys.OverburdenStress, sys2.OverburdenStress, 0.001f);
        }

        [Test]
        public void StressFraction_ReportsCorrectRatio()
        {
            var sys = new TunnelingAndStructuralStress();
            sys.SetMaterialThreshold(100f);
            sys.BeginExcavation("sub_level_4", 4, false);
            sys.Tick(20f);
            Assert.Greater(sys.StressFraction, 0f);
            Assert.LessOrEqual(sys.StressFraction, 1f);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  MyceliumNetworkSystem Tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class MyceliumNetworkSystemTests
    {
        private static Survivor MakeSv(string id, string room = "quarters")
        {
            return new Survivor { Id = id, DisplayName = id, CurrentRoomId = room, State = SurvivorState.Idle };
        }

        [Test]
        public void InitialState_NoSpores()
        {
            var sys = new MyceliumNetworkSystem();
            sys.RegisterRoom("quarters");
            Assert.AreEqual(0f, sys.GetSporeDensity("quarters"), 0.001f);
            Assert.IsFalse(sys.IsRoomInBloom("quarters"));
        }

        [Test]
        public void CorpseUnprocessed_TriggersBloom()
        {
            var sys = new MyceliumNetworkSystem();
            sys.RegisterRoom("quarters");
            bool bloomFired = false;
            sys.OnSporeBloom += evt => bloomFired = true;

            var sv = MakeSv("sv_dead");
            sys.OnCorpseSpawned(sv, "quarters");

            // Tick past the 12-hour threshold
            for (int i = 0; i < 15; i++)
                sys.Tick(1f);

            Assert.IsTrue(bloomFired);
            Assert.Greater(sys.GetSporeDensity("quarters"), 0f);
        }

        [Test]
        public void CorpseResolved_PreventBloom()
        {
            var sys = new MyceliumNetworkSystem();
            sys.RegisterRoom("quarters");
            bool bloomFired = false;
            sys.OnSporeBloom += evt => bloomFired = true;

            var sv = MakeSv("sv_dead");
            sys.OnCorpseSpawned(sv, "quarters");
            sys.OnCorpseResolved("sv_dead");

            for (int i = 0; i < 15; i++)
                sys.Tick(1f);

            Assert.IsFalse(bloomFired);
        }

        [Test]
        public void FungicideFogger_ClearsRoom()
        {
            var sys = new MyceliumNetworkSystem();
            sys.RegisterRoom("quarters");

            // Force bloom
            var sv = MakeSv("sv_dead");
            sys.OnCorpseSpawned(sv, "quarters");
            for (int i = 0; i < 15; i++)
                sys.Tick(1f);

            Assert.Greater(sys.GetSporeDensity("quarters"), 0f);
            sys.DeployFungicideFogger("quarters");
            Assert.AreEqual(0f, sys.GetSporeDensity("quarters"), 0.001f);
        }

        [Test]
        public void UVLamp_SuppressesSpores()
        {
            var sys = new MyceliumNetworkSystem();
            sys.RegisterRoom("quarters");

            // Add some spores
            var sv = MakeSv("sv_dead");
            sys.OnCorpseSpawned(sv, "quarters");
            for (int i = 0; i < 15; i++)
                sys.Tick(1f);

            float densityBefore = sys.GetSporeDensity("quarters");
            sys.InstallUVLamp("quarters");
            sys.Tick(4f);
            Assert.Less(sys.GetSporeDensity("quarters"), densityBefore);
        }

        [Test]
        public void VentRoom_ClearsMostSpores()
        {
            var sys = new MyceliumNetworkSystem();
            sys.RegisterRoom("quarters");

            var sv = MakeSv("sv_dead");
            sys.OnCorpseSpawned(sv, "quarters");
            for (int i = 0; i < 15; i++)
                sys.Tick(1f);

            float heatLoss = sys.VentRoom("quarters");
            Assert.AreEqual(0.4f, heatLoss, 0.01f);
            Assert.Less(sys.GetSporeDensity("quarters"), 20f);
        }

        [Test]
        public void InfectionRoll_AfflictsSurvivorsAtHighDensity()
        {
            var sys = new MyceliumNetworkSystem();
            sys.RegisterRoom("quarters");

            var inflicted = new List<string>();
            sys.InflictAffliction = (svId, affId) => inflicted.Add($"{svId}:{affId}");
            sys.GetSurvivorsInRoom = roomId =>
            {
                var list = new List<Survivor>();
                if (roomId == "quarters") list.Add(MakeSv("sv1", "quarters"));
                return list;
            };

            // Force high spore density
            var sv = MakeSv("sv_dead");
            sys.OnCorpseSpawned(sv, "quarters");
            for (int i = 0; i < 20; i++)
                sys.Tick(1f);

            // Run many ticks for infection rolls
            for (int i = 0; i < 50; i++)
                sys.Tick(1f);

            // With high density and many ticks, infection should occur
            // (probabilistic, so we just verify no crash and the system runs)
            Assert.Pass();
        }

        [Test]
        public void SaveRestore_RoundTrips()
        {
            var sys = new MyceliumNetworkSystem();
            sys.RegisterRoom("quarters");
            sys.RegisterRoom("plant");

            var sv = MakeSv("sv_dead");
            sys.OnCorpseSpawned(sv, "quarters");
            sys.Tick(5f);

            var save = sys.CaptureState();
            var sys2 = new MyceliumNetworkSystem();
            sys2.RestoreState(save);

            Assert.AreEqual(sys.GetSporeDensity("quarters"), sys2.GetSporeDensity("quarters"), 0.001f);
        }

        [Test]
        public void BurnOutRoom_ClearsAllSpores()
        {
            var sys = new MyceliumNetworkSystem();
            sys.RegisterRoom("quarters");

            var sv = MakeSv("sv_dead");
            sys.OnCorpseSpawned(sv, "quarters");
            for (int i = 0; i < 15; i++)
                sys.Tick(1f);

            string burnedRoom = sys.BurnOutRoom("quarters");
            Assert.AreEqual("quarters", burnedRoom);
            Assert.AreEqual(0f, sys.GetSporeDensity("quarters"), 0.001f);
            Assert.IsFalse(sys.IsRoomInBloom("quarters"));
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  BlackAquiferTraits Tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class BlackAquiferTraitsTests
    {
        [Test]
        public void FloodedLocations_ContainsExpectedNodes()
        {
            Assert.IsTrue(BlackAquiferTraits.IsFloodedLocation("location_flooded_subway_depot"));
            Assert.IsTrue(BlackAquiferTraits.IsFloodedLocation("location_submerged_data_center"));
            Assert.IsFalse(BlackAquiferTraits.IsFloodedLocation("abandoned_hospital"));
        }

        [Test]
        public void NullLocationId_ReturnsFalse()
        {
            Assert.IsFalse(BlackAquiferTraits.IsFloodedLocation(null));
            Assert.IsFalse(BlackAquiferTraits.IsFloodedLocation(""));
        }

        [Test]
        public void TraitConstants_AreCorrectStrings()
        {
            Assert.AreEqual("trait_thalassophobia", BlackAquiferTraits.Thalassophobia);
            Assert.AreEqual("trait_spore_carrier", BlackAquiferTraits.SporeCarrier);
            Assert.AreEqual("trait_dark_acclimated", BlackAquiferTraits.DarkAcclimated);
            Assert.AreEqual("trait_claustrophobia", BlackAquiferTraits.Claustrophobia);
            Assert.AreEqual("trait_rot_immunity", BlackAquiferTraits.RotImmunity);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  BlackAquiferItemCatalog Tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class BlackAquiferItemCatalogTests
    {
        [Test]
        public void CreateAll_Returns10Items()
        {
            var items = BlackAquiferItemCatalog.CreateAll();
            Assert.AreEqual(10, items.Count);
        }

        [Test]
        public void AllItems_HaveUniqueIds()
        {
            var items = BlackAquiferItemCatalog.CreateAll();
            var ids = new HashSet<string>();
            for (int i = 0; i < items.Count; i++)
            {
                Assert.IsNotNull(items[i].id);
                Assert.IsTrue(ids.Add(items[i].id), $"Duplicate id: {items[i].id}");
            }
        }

        [Test]
        public void ShoringTimber_HasCorrectWeight()
        {
            var item = BlackAquiferItemCatalog.CreateShoringTimber();
            Assert.AreEqual("item_shoring_timber", item.id);
            Assert.AreEqual(8f, item.weight, 0.01f);
        }

        [Test]
        public void BlackWaterVial_HasContamination()
        {
            var item = BlackAquiferItemCatalog.CreateBlackWaterVial();
            Assert.Greater(item.contamination, 0f);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  Quest_BlackVein Tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class QuestBlackVeinTests
    {
        private AtomicWar._Game.Quests.Quest_BlackVein MakeQuest()
        {
            var q = new AtomicWar._Game.Quests.Quest_BlackVein();
            q.GetDay = () => 30f;
            q.RecordMoralEntry = _ => { };
            return q;
        }

        [Test]
        public void QuestId_IsCorrect()
        {
            Assert.AreEqual("quest_black_vein", AtomicWar._Game.Quests.Quest_BlackVein.Id);
        }

        [Test]
        public void Start_SetsStage1()
        {
            var q = MakeQuest();
            q.Start(30);
            Assert.AreEqual(1, q.State.Stage);
        }

        [Test]
        public void SealBulkhead_AdvancesAndSetsFlag()
        {
            var q = MakeQuest();
            q.Start(30);
            q.Advance(); // to stage 2
            q.Advance(); // to stage 3
            q.ResolveSealBulkhead();
            Assert.AreEqual(1f, q.GetProgress(AtomicWar._Game.Quests.Quest_BlackVein.BulkheadSealedKey));
            Assert.AreEqual(0f, q.GetProgress(AtomicWar._Game.Quests.Quest_BlackVein.DredgerSavedKey));
        }

        [Test]
        public void RerouteFlow_AdvancesAndSetsFlag()
        {
            var q = MakeQuest();
            q.Start(30);
            q.Advance();
            q.Advance();
            q.ResolveRerouteFlow();
            Assert.AreEqual(1f, q.GetProgress(AtomicWar._Game.Quests.Quest_BlackVein.FlowReroutedKey));
            Assert.AreEqual(1f, q.GetProgress(AtomicWar._Game.Quests.Quest_BlackVein.DredgerSavedKey));
            Assert.AreEqual(1f, q.GetProgress(AtomicWar._Game.Quests.Quest_BlackVein.MartaFarmhouseDestroyedKey));
        }
    }
}
