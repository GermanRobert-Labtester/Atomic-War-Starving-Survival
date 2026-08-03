using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Instrument reliability + radiation fog-of-war acceptance tests.
    /// Radiation is invisible; knowledge only comes from devices that can fail.
    /// </summary>
    [TestFixture]
    public class InstrumentReliabilityTests
    {
        private const float Eps = 1e-3f;

        private static ItemDefinition MakeDevice(string id)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = ItemType.Device;
            item.stackMax = 1;
            item.weight = 1f;
            return item;
        }

        private static ItemDefinition MakeMaterial(string id)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = ItemType.Material;
            item.stackMax = 10;
            item.weight = 0.2f;
            return item;
        }

        // -----------------------------------------------------------------
        // Device reliability
        // -----------------------------------------------------------------

        [Test]
        public void BrokenDevice_RecordsNoMeasurement()
        {
            var device = DeviceState.CreateDefault();
            device.Broken = true;

            Assert.IsFalse(InstrumentDevice.CanMeasure(device));
            Assert.IsFalse(InstrumentDevice.TryRead(device, 50f, out float reading));
            Assert.That(reading, Is.EqualTo(0f).Within(Eps));

            var map = new RadiationKnowledgeMap();
            map.SeedTile("rural_gas_station", trueRad: 15f, rumoredRad: 5f, initialUncertainty: 0.5f);

            var inv = new AtomicWar._Game.Inventory.Inventory { Capacity = 10, MaxWeight = 100f };
            var geiger = MakeDevice("geiger_counter");
            inv.Add(geiger, 1);
            var slot = inv.FindSlot("geiger_counter");
            slot.Device.Broken = true;

            var scav = new LocationScavengingSystem(null, inv, null, knowledge: map, getCurrentDay: () => 1);
            Assert.IsFalse(scav.TryImmediateSurvey("rural_gas_station", out _));

            var tile = map.GetTile("rural_gas_station");
            Assert.IsFalse(tile.Surveyed);
            Assert.That(tile.MeasuredAtDay, Is.EqualTo(-1));
        }

        [Test]
        public void MisCalibratedSurvey_RecordsBiasedMeasuredRad()
        {
            const float trueRad = 100f;
            var map = new RadiationKnowledgeMap();
            map.SeedTile("abandoned_hospital", trueRad, rumoredRad: 20f, initialUncertainty: 1f);

            var inv = new AtomicWar._Game.Inventory.Inventory { Capacity = 10, MaxWeight = 100f };
            var geiger = MakeDevice("geiger_counter");
            inv.Add(geiger, 1);
            var slot = inv.FindSlot("geiger_counter");
            slot.Device.Calibration = 0f; // max under-report

            float expected = trueRad * (1f - InstrumentDevice.MaxCalibrationBias);
            Assert.That(InstrumentDevice.ReadBiased(slot.Device, trueRad), Is.EqualTo(expected).Within(Eps));

            var scav = new LocationScavengingSystem(null, inv, null, knowledge: map, getCurrentDay: () => 2);
            Assert.IsTrue(scav.TryImmediateSurvey("abandoned_hospital", out float measured));
            Assert.That(measured, Is.EqualTo(expected).Within(Eps));
            Assert.That(measured, Is.LessThan(trueRad));

            var tile = map.GetTile("abandoned_hospital");
            Assert.IsTrue(tile.Surveyed);
            Assert.That(tile.MeasuredRad, Is.EqualTo(expected).Within(Eps));
            Assert.That(tile.MeasuredWithCalibration, Is.EqualTo(0f).Within(Eps));

            // The lie persists in the player view even while "fresh"
            var view = map.GetPlayerView("abandoned_hospital", currentDay: 2, hasWorkingGeiger: true);
            Assert.IsTrue(view.IsUnreliable);
            Assert.That(view.DisplayedRad, Is.EqualTo(expected).Within(Eps));

            // Re-survey with a good device corrects the lie
            slot.Device.Calibration = 1f;
            slot.Device.Battery = 1f;
            slot.Device.Broken = false;
            Assert.IsTrue(scav.TryImmediateSurvey("abandoned_hospital", out float corrected));
            Assert.That(corrected, Is.EqualTo(trueRad).Within(Eps));
            Assert.That(map.GetTile("abandoned_hospital").MeasuredRad, Is.EqualTo(trueRad).Within(Eps));
        }

        [Test]
        public void BatteryDrain_EmptiesPower_ButIsNotHardBroken()
        {
            var device = DeviceState.CreateDefault();
            InstrumentDevice.DrainBattery(device, 1f);
            Assert.That(device.Battery, Is.EqualTo(0f).Within(Eps));
            Assert.IsFalse(device.Broken, "Empty battery must not set Broken — recharge can restore it.");
            Assert.IsFalse(InstrumentDevice.CanMeasure(device));

            InstrumentDevice.Recharge(device);
            Assert.That(device.Battery, Is.EqualTo(1f).Within(Eps));
            Assert.IsTrue(InstrumentDevice.CanMeasure(device));
        }

        [Test]
        public void HardBreak_CannotMeasure_EvenWithBattery()
        {
            var device = DeviceState.CreateDefault();
            InstrumentDevice.Break(device);
            Assert.IsTrue(device.Broken);
            Assert.IsFalse(InstrumentDevice.CanMeasure(device));
            InstrumentDevice.Recharge(device);
            Assert.IsFalse(InstrumentDevice.CanMeasure(device), "Recharge must not clear hard break.");
        }

        // -----------------------------------------------------------------
        // Fog-of-war / map knowledge
        // -----------------------------------------------------------------

        [Test]
        public void StaleReading_BlendsTowardRumor_AndUncertaintyGrows()
        {
            var map = new RadiationKnowledgeMap();
            map.SeedTile("suburban_house", trueRad: 10f, rumoredRad: 40f, initialUncertainty: 0f);

            // Fresh reliable survey
            Assert.IsTrue(map.RecordSurvey("suburban_house", measuredRad: 10f, deviceCalibration: 1f, day: 1));
            var fresh = map.GetPlayerView("suburban_house", currentDay: 1, hasWorkingGeiger: true);
            Assert.IsFalse(fresh.IsUnreliable);
            Assert.That(fresh.DisplayedRad, Is.EqualTo(10f).Within(Eps));
            Assert.That(fresh.Confidence, Is.GreaterThan(0.9f));

            // Age past freshness; grow uncertainty each day
            for (int d = 2; d <= 6; d++)
            {
                map.TickDay(d);
            }

            var stale = map.GetPlayerView("suburban_house", currentDay: 6, hasWorkingGeiger: true);
            Assert.IsTrue(stale.IsUnreliable);
            // Blended toward rumor (40) away from measured (10)
            Assert.That(stale.DisplayedRad, Is.GreaterThan(10f + Eps));
            Assert.That(stale.DisplayedRad, Is.LessThan(40f + Eps));
            Assert.That(stale.Confidence, Is.LessThan(fresh.Confidence));

            var tile = map.GetTile("suburban_house");
            Assert.That(tile.RumorUncertainty, Is.GreaterThan(0f));
        }

        [Test]
        public void NoWorkingGeiger_MapViewIsDark()
        {
            var map = new RadiationKnowledgeMap();
            map.SeedTile("government_bunker", trueRad: 60f, rumoredRad: 10f, initialUncertainty: 0.2f);
            map.RecordSurvey("government_bunker", 60f, 1f, day: 1);

            var dark = map.GetPlayerView("government_bunker", currentDay: 1, hasWorkingGeiger: false);
            Assert.IsTrue(dark.IsDark);
            Assert.IsTrue(dark.IsUnknown);
            Assert.That(dark.Confidence, Is.EqualTo(0f).Within(Eps));

            var inv = new AtomicWar._Game.Inventory.Inventory { Capacity = 10, MaxWeight = 100f };
            Assert.IsFalse(inv.HasWorkingGeiger());

            inv.Add(MakeDevice("geiger_counter"), 1);
            Assert.IsTrue(inv.HasWorkingGeiger());

            inv.FindSlot("geiger_counter").Device.Broken = true;
            Assert.IsFalse(inv.HasWorkingGeiger());
        }

        [Test]
        public void Inventory_DeviceState_RoundTripsThroughSave()
        {
            var inv = new AtomicWar._Game.Inventory.Inventory { Capacity = 10, MaxWeight = 100f };
            var geiger = MakeDevice("geiger_counter");
            inv.Add(geiger, 1);
            var slot = inv.FindSlot("geiger_counter");
            slot.Device.Battery = 0.4f;
            slot.Device.Calibration = 0.7f;
            slot.Device.LastCalibratedDay = 3;

            var state = inv.CaptureState();
            var inv2 = new AtomicWar._Game.Inventory.Inventory();
            inv2.RestoreState(state, id => id == "geiger_counter" ? geiger : null);

            var restored = inv2.FindSlot("geiger_counter").Device;
            Assert.That(restored.Battery, Is.EqualTo(0.4f).Within(Eps));
            Assert.That(restored.Calibration, Is.EqualTo(0.7f).Within(Eps));
            Assert.That(restored.LastCalibratedDay, Is.EqualTo(3));
            Assert.IsFalse(restored.Broken);
        }

        [Test]
        public void MapKnowledge_SaveLoad_PreservesMeasuredLie()
        {
            var map = new RadiationKnowledgeMap();
            map.SeedTile("rural_gas_station", 15f, 5f, 0.3f);
            map.RecordSurvey("rural_gas_station", measuredRad: 9f, deviceCalibration: 0.5f, day: 4);

            var save = map.CaptureState();
            var map2 = new RadiationKnowledgeMap();
            map2.RestoreState(save);

            var tile = map2.GetTile("rural_gas_station");
            Assert.IsTrue(tile.Surveyed);
            Assert.That(tile.MeasuredRad, Is.EqualTo(9f).Within(Eps));
            Assert.That(tile.MeasuredWithCalibration, Is.EqualTo(0.5f).Within(Eps));
            Assert.That(tile.MeasuredAtDay, Is.EqualTo(4));
            Assert.That(tile.TrueRad, Is.EqualTo(15f).Within(Eps));
        }

        [Test]
        public void GeigerCounter_SilentWhenBroken()
        {
            var geiger = new GeigerCounter
            {
                Device = new DeviceState { Battery = 1f, Calibration = 1f, Broken = true }
            };
            geiger.SetTrueRadLevel(100f);
            Assert.That(geiger.CurrentRadLevel, Is.EqualTo(0f).Within(Eps));

            int clicks = 0;
            geiger.OnClick += () => clicks++;
            geiger.Tick(1f);
            Assert.That(clicks, Is.EqualTo(0));
        }

        [Test]
        public void MapKnowledgeHUD_CalibrationLabel()
        {
            var go = new GameObject("hud_test");
            try
            {
                var hud = go.AddComponent<AtomicWar._Game.UI.MapKnowledgeHUD>();
                hud.SetCalibrationAge(3);
                Assert.That(hud.GetCalibrationLabel(), Is.EqualTo("last calibrated: 3 days ago"));
                hud.SetCalibrationAge(0);
                Assert.That(hud.GetCalibrationLabel(), Is.EqualTo("last calibrated: today"));
                hud.SetCalibrationAge(-1);
                Assert.That(hud.GetCalibrationLabel(), Is.EqualTo("no geiger"));
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        [Test]
        public void SurveyMission_ExposesTrueRad_NotBiasedReading()
        {
            const float trueRad = 80f;
            var map = new RadiationKnowledgeMap();
            map.SeedTile("abandoned_hospital", trueRad, rumoredRad: 10f, initialUncertainty: 1f);

            var inv = new AtomicWar._Game.Inventory.Inventory { Capacity = 10, MaxWeight = 100f };
            inv.Add(MakeDevice("geiger_counter"), 1);
            inv.FindSlot("geiger_counter").Device.Calibration = 0f; // would under-report to 48

            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile);
            var survivor = new Survivor { Id = "sv_survey", DisplayName = "Surveyor" };
            needs.Register(survivor);
            var rad = new RadiationSystem(needs);
            rad.Register(survivor);

            var loc = ScriptableObject.CreateInstance<LocationDefinitionSO>();
            loc.id = "abandoned_hospital";
            loc.displayName = "Hospital";
            loc.travelHours = 0.5f;
            loc.baseRadsPerHour = trueRad;
            loc.dangerLevel = 1f;

            var scav = new LocationScavengingSystem(rad, inv, null, knowledge: map, getCurrentDay: () => 1);
            Assert.IsTrue(scav.StartSurvey(survivor, loc));

            // Run full mission duration
            float missionHours = loc.travelHours + LocationScavengingSystem.SurveyHours;
            scav.Tick(missionHours + 0.1f);

            // Exposure must use true rad, not the biased 48 reading.
            // LifetimeRadiationExposure is the unclamped cumulative (RadiationDose is
            // clamped 0..100 and may be cleared by PrognosisPipeline at high acute load).
            float expectedDose = trueRad * missionHours;
            Assert.That(survivor.LifetimeRadiationExposure, Is.EqualTo(expectedDose).Within(0.5f));
            // Biased reading would have been ~48 * hours ≈ 72 — ensure we got the true path
            float biasedWouldBe = trueRad * (1f - InstrumentDevice.MaxCalibrationBias) * missionHours;
            Assert.That(survivor.LifetimeRadiationExposure, Is.GreaterThan(biasedWouldBe + 1f));

            // Map still recorded the biased lie
            Assert.That(map.GetTile("abandoned_hospital").MeasuredRad,
                Is.EqualTo(trueRad * (1f - InstrumentDevice.MaxCalibrationBias)).Within(Eps));
        }

        [Test]
        public void MisCalibratedFresh_ViewHasReducedConfidence()
        {
            var map = new RadiationKnowledgeMap();
            map.SeedTile("rural_gas_station", 15f, 5f, 1f);
            map.RecordSurvey("rural_gas_station", measuredRad: 9f, deviceCalibration: 0.5f, day: 1);

            var view = map.GetPlayerView("rural_gas_station", currentDay: 1, hasWorkingGeiger: true);
            Assert.IsTrue(view.IsUnreliable);
            Assert.That(view.Confidence, Is.EqualTo(0.5f).Within(Eps));
            Assert.That(view.Confidence, Is.LessThan(1f));
        }
    }
}
