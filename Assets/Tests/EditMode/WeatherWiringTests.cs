using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Weather_* wiring: 15 special weather events — API smoke + Capture/Restore + save slots.
    /// </summary>
    [TestFixture]
    public class WeatherWiringTests
    {
        private const float Eps = 1e-3f;

        private static string TempDir(string tag)
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_weather_" + tag + "_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static SaveSystem MakeSave(string dir, Action<SaveSystem> wire)
        {
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var weather = new WeatherSystem(null, 3);
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
            wire(ss);
            return ss;
        }

        [Test]
        public void AcidSnow_Corrosion_Capture()
        {
            var w = new Weather_AcidSnow();
            w.SetActive(true);
            float filter = 100f;
            float drain = w.TickCorrosion(1f, true, ref filter);
            Assert.AreEqual(10f, drain, Eps);
            Assert.AreEqual(90f, filter, Eps);
            var save = w.CaptureState();
            Assert.AreEqual("weather_acid_snow", save.weatherId);
            Assert.IsTrue(save.isActive);
            var w2 = new Weather_AcidSnow();
            w2.RestoreState(save);
            Assert.IsTrue(w2.State.isActive);
        }

        [Test]
        public void BioFog_Activate_Capture()
        {
            var w = new Weather_BioFog();
            w.ActivateBioFog();
            Assert.IsTrue(w.State.isActive);
            Assert.AreEqual(2f, w.State.encounterRateMultiplier, Eps);
            Assert.AreEqual(5f, w.CreepIntoAirlock(1f), Eps);
            var save = w.CaptureState();
            Assert.AreEqual("weather_bio_fog", save.weatherId);
            var w2 = new Weather_BioFog();
            w2.RestoreState(save);
            Assert.IsTrue(w2.State.isActive);
        }

        [Test]
        public void BlackSnow_Suit_Capture()
        {
            var w = new Weather_BlackSnow();
            w.HitBySnow("sv_a", has_ammonia: false);
            Assert.IsTrue(w.IsSuitRuined("sv_a"));
            w.HitBySnow("sv_b", has_ammonia: true);
            Assert.IsFalse(w.IsSuitRuined("sv_b"));
            var save = w.CaptureState();
            Assert.AreEqual("weather_black_snow", save.weather_id);
            Assert.Contains("sv_a", save.ruined_suit_survivor_ids);
            var w2 = new Weather_BlackSnow();
            w2.RestoreState(save);
            Assert.IsTrue(w2.IsSuitRuined("sv_a"));
        }

        [Test]
        public void BloodRain_Trigger_Capture()
        {
            var w = new Weather_BloodRain();
            w.Trigger();
            Assert.IsTrue(w.IsActive);
            Assert.AreEqual(24f, w.State.hoursRemaining, Eps);
            w.TickHour();
            Assert.AreEqual(23f, w.State.hoursRemaining, Eps);
            var save = w.CaptureState();
            Assert.AreEqual("weather_blood_rain", save.weatherId);
            var w2 = new Weather_BloodRain();
            w2.RestoreState(save);
            Assert.AreEqual(23f, w2.State.hoursRemaining, Eps);
        }

        [Test]
        public void DeadWind_Activate_Capture()
        {
            var w = new Weather_DeadWind();
            w.ActivateDeadWind();
            Assert.IsTrue(w.State.isActive);
            Assert.AreEqual(0f, w.State.windSpeed, Eps);
            Assert.AreEqual(0.5f, w.State.airFilterEfficiencyMultiplier, Eps);
            // Snapshot via JsonUtility (same path as SaveSystem) so later mutation is independent.
            var save = JsonUtility.FromJson<DeadWindState>(JsonUtility.ToJson(w.CaptureState()));
            Assert.AreEqual("weather_dead_wind", save.weatherId);
            w.DeactivateDeadWind();
            Assert.IsFalse(w.State.isActive);
            var w2 = new Weather_DeadWind();
            w2.RestoreState(save);
            Assert.IsTrue(w2.State.isActive);
        }

        [Test]
        public void DeepFreeze_Trigger_Capture()
        {
            var w = new Weather_DeepFreeze();
            w.TriggerDeepFreeze();
            Assert.IsTrue(w.State.isActive);
            Assert.AreEqual(72f, w.State.durationHoursRemaining, Eps);
            Assert.IsTrue(w.State.cropsKilled);
            w.TickHourly(1f, -5f);
            Assert.AreEqual(71f, w.State.durationHoursRemaining, Eps);
            var save = w.CaptureState();
            Assert.AreEqual("weather_deep_freeze", save.weatherId);
            var w2 = new Weather_DeepFreeze();
            w2.RestoreState(save);
            Assert.AreEqual(71f, w2.State.durationHoursRemaining, Eps);
        }

        [Test]
        public void DustDevil_Tick_Capture()
        {
            var w = new Weather_DustDevil(new List<string> { "n1", "n2", "bunker" });
            w.Tick("n1", "bunker", new System.Random(1));
            Assert.IsFalse(string.IsNullOrEmpty(w.CurrentNodeId));
            var save = w.CaptureState();
            Assert.AreEqual("weather_dust_devil", save.weather_id);
            var w2 = new Weather_DustDevil();
            w2.RestoreState(save);
            Assert.AreEqual(save.current_node_id, w2.CurrentNodeId);
        }

        [Test]
        public void EmpStorm_Burst_Capture()
        {
            var w = new Weather_EMPStorm();
            w.TriggerEMPBurst(new System.Random(5));
            Assert.IsTrue(w.State.isPowerGridTripped);
            Assert.Greater(w.State.blackoutHoursRemaining, 0);
            int hours = w.State.blackoutHoursRemaining;
            w.TickHourly(1);
            Assert.AreEqual(hours - 1, w.State.blackoutHoursRemaining);
            var save = w.CaptureState();
            Assert.AreEqual("weather_emp_storm", save.weatherId);
            var w2 = new Weather_EMPStorm();
            w2.RestoreState(save);
            Assert.AreEqual(hours - 1, w2.State.blackoutHoursRemaining);
        }

        [Test]
        public void FalseSpring_Trigger_Capture()
        {
            var w = new Weather_FalseSpring();
            w.TriggerFalseSpring();
            Assert.IsTrue(w.State.isActive);
            Assert.IsTrue(w.State.isLowestRoomFlooded);
            Assert.AreEqual(48f, w.State.durationHoursRemaining, Eps);
            var save = w.CaptureState();
            Assert.AreEqual("weather_false_spring", save.weatherId);
            var w2 = new Weather_FalseSpring();
            w2.RestoreState(save);
            Assert.IsTrue(w2.State.isCatchmentOverflowing);
        }

        [Test]
        public void GlassStorm_Trigger_Capture()
        {
            var w = new Weather_GlassStorm();
            w.Trigger();
            Assert.IsTrue(w.IsActive);
            float hazmat = 100f, hatch = 50f;
            w.TickHour(ref hazmat, ref hatch);
            Assert.Less(hazmat, 100f);
            var save = w.CaptureState();
            Assert.AreEqual("weather_glass_storm", save.weatherId);
            Assert.AreEqual(11f, save.hoursRemaining, Eps);
            var w2 = new Weather_GlassStorm();
            w2.RestoreState(save);
            Assert.IsTrue(w2.IsActive);
        }

        [Test]
        public void OzoneHole_Summer_Capture()
        {
            var w = new Weather_OzoneHole();
            w.Trigger(season: 0); // not summer
            Assert.IsFalse(w.State.isActive);
            w.Trigger(season: 2); // summer
            Assert.IsTrue(w.State.isActive);
            Assert.AreEqual(5, w.State.daysRemaining);
            var (rad, burn) = w.TickHour(isDaylight: true);
            Assert.AreEqual(20f, rad, Eps);
            Assert.AreEqual(10f, burn, Eps);
            Assert.IsFalse(w.IsScavengingAllowed(isDaylight: true));
            Assert.IsTrue(w.IsScavengingAllowed(isDaylight: false));
            var save = w.CaptureState();
            Assert.AreEqual("weather_ozone_hole", save.weatherId);
            var w2 = new Weather_OzoneHole();
            w2.RestoreState(save);
            Assert.IsTrue(w2.State.isActive);
        }

        [Test]
        public void RadHail_Strike_Capture()
        {
            var w = new Weather_RadHail();
            w.TriggerRadHailStorm();
            Assert.IsTrue(w.State.isActive);
            var (dmg, rads) = w.StrikeSurvivorOutside(hasHardCover: false);
            Assert.AreEqual(35f, dmg, Eps);
            Assert.AreEqual(400f, rads, Eps);
            Assert.AreEqual((0f, 0f), w.StrikeSurvivorOutside(hasHardCover: true));
            var save = w.CaptureState();
            Assert.AreEqual("weather_rad_hail", save.weatherId);
            var w2 = new Weather_RadHail();
            w2.RestoreState(save);
            Assert.IsTrue(w2.State.isActive);
        }

        [Test]
        public void SilentSpring_Trigger_Capture()
        {
            var w = new Weather_SilentSpring();
            w.Trigger();
            Assert.IsTrue(w.IsActive);
            Assert.IsTrue(w.State.audioSilenced);
            Assert.AreEqual(6f, w.State.hoursRemaining, Eps);
            var save = w.CaptureState();
            Assert.AreEqual("weather_silent_spring", save.weatherId);
            var w2 = new Weather_SilentSpring();
            w2.RestoreState(save);
            Assert.IsTrue(w2.IsActive);
        }

        [Test]
        public void SolarFlare_Trigger_Capture()
        {
            var w = new Weather_SolarFlare();
            w.Trigger();
            Assert.IsTrue(w.AreElectronicsDisabled());
            Assert.AreEqual(30f, w.GetMoraleBonus(), Eps);
            Assert.AreEqual(72f, w.State.hoursRemaining, Eps);
            w.TickHour();
            Assert.AreEqual(71f, w.State.hoursRemaining, Eps);
            var save = w.CaptureState();
            Assert.AreEqual("weather_solar_flare", save.weatherId);
            var w2 = new Weather_SolarFlare();
            w2.RestoreState(save);
            Assert.IsTrue(w2.AreElectronicsDisabled());
            Assert.AreEqual(71f, w2.State.hoursRemaining, Eps);
        }

        [Test]
        public void StaticCharge_Modules_Capture()
        {
            var w = new Weather_StaticCharge();
            w.RegisterModule("heater");
            w.RegisterModule("lathe");
            w.Trigger();
            Assert.IsTrue(w.IsActive);
            Assert.IsTrue(w.IsModuleElectrified("heater"));
            float dmg = w.GetShockDamage("heater", hasRubberGloves: false);
            Assert.AreEqual(15f, dmg, Eps);
            Assert.AreEqual(0f, w.GetShockDamage("heater", hasRubberGloves: true), Eps);
            var save = w.CaptureState();
            Assert.AreEqual("weather_static_charge", save.weatherId);
            Assert.AreEqual(2, save.affectedModules.Count);
            var w2 = new Weather_StaticCharge();
            w2.RestoreState(save);
            Assert.IsTrue(w2.IsModuleElectrified("lathe"));
        }

        [Test]
        public void MultiWeather_SaveSlot_RoundTrip()
        {
            string dir = TempDir("multi");
            try
            {
                var acid = new Weather_AcidSnow();
                acid.SetActive(true);

                var snow = new Weather_BlackSnow();
                snow.HitBySnow("sv_x", false);

                var flare = new Weather_SolarFlare();
                flare.Trigger();
                flare.TickHour();

                var staticCh = new Weather_StaticCharge();
                staticCh.RegisterModule("radio");
                staticCh.Trigger();

                var emp = new Weather_EMPStorm();
                emp.TriggerEMPBurst(new System.Random(9));

                Assert.IsTrue(MakeSave(dir, ss =>
                {
                    ss.SetWeatherAcidSnow(acid);
                    ss.SetWeatherBlackSnow(snow);
                    ss.SetWeatherSolarFlare(flare);
                    ss.SetWeatherStaticCharge(staticCh);
                    ss.SetWeatherEmpStorm(emp);
                    ss.SetWeatherBioFog(new Weather_BioFog());
                    ss.SetWeatherBloodRain(new Weather_BloodRain());
                    ss.SetWeatherDeadWind(new Weather_DeadWind());
                    ss.SetWeatherDeepFreeze(new Weather_DeepFreeze());
                    ss.SetWeatherDustDevil(new Weather_DustDevil());
                    ss.SetWeatherFalseSpring(new Weather_FalseSpring());
                    ss.SetWeatherGlassStorm(new Weather_GlassStorm());
                    ss.SetWeatherOzoneHole(new Weather_OzoneHole());
                    ss.SetWeatherRadHail(new Weather_RadHail());
                    ss.SetWeatherSilentSpring(new Weather_SilentSpring());
                }).Save("slot"));

                var acid2 = new Weather_AcidSnow();
                var snow2 = new Weather_BlackSnow();
                var flare2 = new Weather_SolarFlare();
                var static2 = new Weather_StaticCharge();
                var emp2 = new Weather_EMPStorm();
                Assert.IsTrue(MakeSave(dir, ss =>
                {
                    ss.SetWeatherAcidSnow(acid2);
                    ss.SetWeatherBlackSnow(snow2);
                    ss.SetWeatherSolarFlare(flare2);
                    ss.SetWeatherStaticCharge(static2);
                    ss.SetWeatherEmpStorm(emp2);
                    ss.SetWeatherBioFog(new Weather_BioFog());
                    ss.SetWeatherBloodRain(new Weather_BloodRain());
                    ss.SetWeatherDeadWind(new Weather_DeadWind());
                    ss.SetWeatherDeepFreeze(new Weather_DeepFreeze());
                    ss.SetWeatherDustDevil(new Weather_DustDevil());
                    ss.SetWeatherFalseSpring(new Weather_FalseSpring());
                    ss.SetWeatherGlassStorm(new Weather_GlassStorm());
                    ss.SetWeatherOzoneHole(new Weather_OzoneHole());
                    ss.SetWeatherRadHail(new Weather_RadHail());
                    ss.SetWeatherSilentSpring(new Weather_SilentSpring());
                }).Load("slot"));

                Assert.IsTrue(acid2.State.isActive);
                Assert.IsTrue(snow2.IsSuitRuined("sv_x"));
                Assert.AreEqual(71f, flare2.State.hoursRemaining, Eps);
                Assert.IsTrue(static2.IsModuleElectrified("radio"));
                Assert.IsTrue(emp2.State.isPowerGridTripped);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }
    }
}
