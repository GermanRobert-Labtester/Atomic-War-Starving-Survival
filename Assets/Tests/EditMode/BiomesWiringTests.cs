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
    /// Biome_* wiring: 6 expedition biomes — API smoke + Capture/Restore + save slots.
    /// </summary>
    [TestFixture]
    public class BiomesWiringTests
    {
        private const float Eps = 1e-3f;

        [Test]
        public void AshSwamp_Enter_Capture()
        {
            var b = new Biome_AshSwamp();
            Assert.AreEqual(0.5f, b.GetMovementSpeedMult(), Eps);
            Assert.IsFalse(b.CanStealth());
            string penalized = null;
            b.OnMovementPenalized += id => penalized = id;
            b.EnterBiome("sv_a", new System.Random(1));
            Assert.AreEqual("sv_a", penalized);
            var save = b.CaptureState();
            Assert.AreEqual("biome_ash_swamp", save.biomeId);
            Assert.AreEqual(0.30f, save.parasiteInfectionChance, Eps);
            var b2 = new Biome_AshSwamp();
            b2.RestoreState(save);
            Assert.AreEqual(0.5f, b2.GetMovementSpeedMult(), Eps);
        }

        [Test]
        public void GlassDesert_EnterScavenge_Capture()
        {
            var b = new Biome_GlassDesert();
            Assert.AreEqual(0f, b.GetCover(), Eps);
            Assert.AreEqual(40f, b.GetTemperature(), Eps);
            string heat = null;
            b.OnHeatStress += id => heat = id;
            b.EnterBiome("sv_b");
            Assert.AreEqual("sv_b", heat);
            // Scavenge is rng — just ensure non-crash and capture defaults
            b.Scavenge("sv_b", new System.Random(42));
            var save = b.CaptureState();
            Assert.AreEqual("biome_glass_desert", save.biomeId);
            Assert.IsTrue(save.yieldsVitrifiedGlass);
            var b2 = new Biome_GlassDesert();
            b2.RestoreState(save);
            Assert.AreEqual(40f, b2.GetTemperature(), Eps);
        }

        [Test]
        public void HighwayTunnel_DarknessSiphon_Capture()
        {
            var b = new Biome_HighwayTunnel();
            Assert.IsTrue(b.IsWeatherImmune());
            string dark = null;
            b.OnDarknessPenalty += id => dark = id;
            b.EnterBiome("sv_c", hasFlashlight: false, hasNVG: false);
            Assert.AreEqual("sv_c", dark);
            dark = null;
            b.EnterBiome("sv_c", hasFlashlight: true, hasNVG: false);
            Assert.IsNull(dark);
            float fuel = b.SiphonGas("sv_c", "veh_1", new System.Random(7));
            Assert.Greater(fuel, 0f);
            var save = b.CaptureState();
            Assert.AreEqual("biome_highway_tunnel", save.biomeId);
            Assert.IsTrue(save.isPitchBlack);
            var b2 = new Biome_HighwayTunnel();
            b2.RestoreState(save);
            Assert.IsTrue(b2.IsWeatherImmune());
        }

        [Test]
        public void SaltFlats_VehicleThirst_Capture()
        {
            var b = new Biome_SaltFlats();
            Assert.IsFalse(b.EnterBiome(has_vehicle: false));
            Assert.IsTrue(b.EnterBiome(has_vehicle: true));
            Assert.AreEqual(5f, b.GetThirstDrain(), Eps);
            Assert.AreEqual(50f, b.GetTemperature(), Eps);
            float thirst = b.TickHour(100f);
            Assert.AreEqual(95f, thirst, Eps);
            Assert.AreEqual(4f, b.HarvestSalt(2f), Eps);
            Assert.IsFalse(b.CanCross(false));
            var save = b.CaptureState();
            Assert.AreEqual("biome_salt_flats", save.biome_id);
            Assert.AreEqual(5f, save.thirst_drain_multiplier, Eps);
            var b2 = new Biome_SaltFlats();
            b2.RestoreState(save);
            Assert.AreEqual(5f, b2.GetThirstDrain(), Eps);
        }

        [Test]
        public void SkyscraperTops_Traverse_Capture()
        {
            var b = new Biome_SkyscraperTops();
            Assert.AreEqual(0.7f, b.GetColdExposure(), Eps);
            // rope + lucky rng should usually survive; assert cold event fires
            string cold = null;
            b.OnColdExposure += id => cold = id;
            b.Traverse("sv_d", hasRope: true, new System.Random(99));
            Assert.AreEqual("sv_d", cold);
            var save = b.CaptureState();
            Assert.AreEqual("biome_skyscraper_tops", save.biomeId);
            Assert.AreEqual(0.15f, save.fallDeathChance, Eps);
            var b2 = new Biome_SkyscraperTops();
            b2.RestoreState(save);
            Assert.AreEqual(0.7f, b2.GetColdExposure(), Eps);
        }

        [Test]
        public void Suburbs_ScoutFog_Capture()
        {
            var b = new Biome_Suburbs();
            string fog = null;
            b.OnFogOfWarRevealed += tile => fog = tile;
            b.RevealFog("tile_3");
            Assert.AreEqual("tile_3", fog);
            var adj = b.GetAdjacentTileIds("t0");
            Assert.AreEqual(8, adj.Count); // radius 1 → 3x3-1
            // ScoutHouse is rng — smoke only
            b.ScoutHouse("sv_e", "house_1", new System.Random(3));
            var save = b.CaptureState();
            Assert.AreEqual("biome_suburbs", save.biomeId);
            Assert.AreEqual(0.8f, save.lootDensity, Eps);
            Assert.AreEqual(1, save.fogOfWarRadius);
            var b2 = new Biome_Suburbs();
            b2.RestoreState(save);
            Assert.AreEqual(8, b2.GetAdjacentTileIds("t0").Count);
        }

        [Test]
        public void MultiBiome_SaveSlot_RoundTrip()
        {
            string dir = SaveSystemTestFactory.TempDir("biome_multi");
            try
            {
                var swamp = new Biome_AshSwamp();
                var salt = new Biome_SaltFlats();
                salt.EnterBiome(true);
                var suburbs = new Biome_Suburbs();
                suburbs.RevealFog("hub");

                // Mutate salt state slightly via restore-of-capture pattern for distinct values
                var saltState = salt.CaptureState();
                saltState.thirst_drain_multiplier = 7f;
                salt.RestoreState(saltState);

                Assert.IsTrue(SaveSystemTestFactory.MakeSave(dir, ss =>
                {
                    ss.SetBiomeAshSwamp(swamp);
                    ss.SetBiomeSaltFlats(salt);
                    ss.SetBiomeSuburbs(suburbs);
                    ss.SetBiomeGlassDesert(new Biome_GlassDesert());
                    ss.SetBiomeHighwayTunnel(new Biome_HighwayTunnel());
                    ss.SetBiomeSkyscraperTops(new Biome_SkyscraperTops());
                }).Save("slot"));

                var swamp2 = new Biome_AshSwamp();
                var salt2 = new Biome_SaltFlats();
                var suburbs2 = new Biome_Suburbs();
                var glass2 = new Biome_GlassDesert();
                var tunnel2 = new Biome_HighwayTunnel();
                var sky2 = new Biome_SkyscraperTops();
                Assert.IsTrue(SaveSystemTestFactory.MakeSave(dir, ss =>
                {
                    ss.SetBiomeAshSwamp(swamp2);
                    ss.SetBiomeSaltFlats(salt2);
                    ss.SetBiomeSuburbs(suburbs2);
                    ss.SetBiomeGlassDesert(glass2);
                    ss.SetBiomeHighwayTunnel(tunnel2);
                    ss.SetBiomeSkyscraperTops(sky2);
                }).Load("slot"));

                Assert.AreEqual("biome_ash_swamp", swamp2.CaptureState().biomeId);
                Assert.AreEqual(7f, salt2.CaptureState().thirst_drain_multiplier, Eps);
                Assert.AreEqual("biome_suburbs", suburbs2.CaptureState().biomeId);
                Assert.AreEqual(40f, glass2.GetTemperature(), Eps);
                Assert.IsTrue(tunnel2.IsWeatherImmune());
                Assert.AreEqual(0.7f, sky2.GetColdExposure(), Eps);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }
    }
}
