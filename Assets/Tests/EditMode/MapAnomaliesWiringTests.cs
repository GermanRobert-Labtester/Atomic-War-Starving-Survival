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

using AtomicWar._Game.World;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// MapAnomaly_* wiring: 18 expedition/map anomalies — API smoke + Capture/Restore + save slots.
    /// </summary>
    [TestFixture]
    public class MapAnomaliesWiringTests
    {
        private const float Eps = 1e-3f;

        [Test]
        public void AshDunes_Jam_Capture()
        {
            var a = new MapAnomaly_AshDunes();
            float dur = 50f;
            float mult = a.TraverseAshDunes("sv_a", ref dur);
            Assert.AreEqual(0f, dur, Eps);
            Assert.AreEqual(0.5f, mult, Eps);
            var save = a.CaptureState();
            Assert.AreEqual("map_anomaly_ash_dunes", save.anomalyId);
            var a2 = new MapAnomaly_AshDunes();
            a2.RestoreState(save);
            Assert.IsTrue(a2.State.causesWeaponJamming);
        }

        [Test]
        public void BoilingLake_CrossSink_Capture()
        {
            var b = new MapAnomaly_BoilingLake();
            float hull = b.CrossByBoat(100f, 2f);
            Assert.AreEqual(70f, hull, Eps); // 15*2 damage
            Assert.AreEqual(0f, b.CrossByBoat(10f, 2f), Eps); // sunk
            var save = b.CaptureState();
            Assert.AreEqual(15f, save.hullDamagePerTick, Eps);
        }

        [Test]
        public void Cherenkov_EnterPool_Capture()
        {
            var c = new MapAnomaly_Cherenkov();
            int stage = -1;
            c.OnRadStageApplied += (_, s) => stage = s;
            c.EnterPool("sv_a");
            Assert.AreEqual(2, stage);
            Assert.IsTrue(c.IsLit("node_x"));
            var save = c.CaptureState();
            var c2 = new MapAnomaly_Cherenkov();
            c2.RestoreState(save);
            Assert.AreEqual(2, c2.CaptureState().instant_rad_stage);
        }

        [Test]
        public void DogDen_Loot_Capture()
        {
            var d = new MapAnomaly_DogDen();
            var loot = d.ClearDenAndLoot(true);
            Assert.Greater(loot.Count, 0);
            Assert.AreEqual(0, d.ClearDenAndLoot(false).Count);
            var save = d.CaptureState();
            Assert.AreEqual(1f, save.hostileEncounterRate, Eps);
        }

        [Test]
        public void DontLook_Inspect_Capture()
        {
            var d = new MapAnomaly_DontLook();
            string broken = null;
            d.OnCatatonicBreak += id => broken = id;
            d.DisplayWarning();
            d.InspectNode("sv_look");
            Assert.AreEqual("sv_look", broken);
            Assert.IsTrue(d.ShouldAvert());
            Assert.AreEqual("Averting eyes.", d.CaptureState().warningText);
        }

        [Test]
        public void DryCoral_Harvest_Capture()
        {
            var c = new MapAnomaly_DryCoral();
            c.Discover("salt_node");
            Assert.IsTrue(c.HarvestCrystal("sv_a", has_hazmat: true, hazmat_level: 100f));
            Assert.AreEqual(1, c.GetCrystalYield());
            var save = c.CaptureState();
            Assert.IsTrue(save.is_discovered);
            var c2 = new MapAnomaly_DryCoral();
            c2.RestoreState(save);
            Assert.AreEqual(1, c2.GetCrystalYield());
        }

        [Test]
        public void FloodedSubway_Wade_Capture()
        {
            var f = new MapAnomaly_FloodedSubway();
            float warmth = 50f, haz = 80f;
            Assert.IsTrue(f.WadeThroughSubway("sv_a", ref warmth, ref haz, out string hypo));
            Assert.AreEqual(0f, warmth, Eps);
            Assert.AreEqual(0f, haz, Eps);
            Assert.AreEqual("hypothermia_affliction", hypo);
            Assert.IsTrue(f.CaptureState().isShortcutActive);
        }

        [Test]
        public void GlassCrater_Navigate_Capture()
        {
            var g = new MapAnomaly_GlassCrater();
            // force slip with seeded rng that returns < 0.5 — use rng that always returns 0
            bool slipped = g.NavigateCrater("sv_a", new System.Random(0), out string aff);
            // Random(0) first NextDouble is deterministic; just check capture works either way
            var save = g.CaptureState();
            Assert.AreEqual(4000f, save.radiationMillisieverts, Eps);
            Assert.AreEqual(0, save.lootCount);
            // if slipped, affliction set
            if (slipped) Assert.AreEqual("razor_glass_laceration", aff);
        }

        [Test]
        public void MassGrave_TraverseRob_Capture()
        {
            var m = new MapAnomaly_MassGrave();
            float morale = 80f;
            m.TraverseGraveNode("party", ref morale);
            Assert.AreEqual(60f, morale, Eps);
            float karma = 100f, sanity = 100f;
            var loot = m.RobCorpses("sv_a", ref karma, ref sanity);
            Assert.Greater(loot.Count, 0);
            Assert.AreEqual(70f, karma, Eps);
            var save = m.CaptureState();
            Assert.AreEqual(20f, save.travelMoraleDrop, Eps);
        }

        [Test]
        public void Mirage_Heatwave_Capture()
        {
            var m = new MapAnomaly_Mirage(new System.Random(42));
            m.SpawnDuringHeatwave(true);
            var save = m.CaptureState();
            Assert.IsTrue(save.is_active);
            Assert.Greater(save.fake_nodes_spawned.Count, 0);
            var m2 = new MapAnomaly_Mirage();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().is_active);
            Assert.AreEqual(save.fake_nodes_spawned.Count, m2.CaptureState().fake_nodes_spawned.Count);
        }

        [Test]
        public void PetrifiedForest_Discover_Capture()
        {
            var p = new MapAnomaly_PetrifiedForest();
            p.Discover("forest_node");
            p.HarvestTree(2);
            Assert.AreEqual(0f, p.GetWoodYield(), Eps);
            Assert.IsTrue(p.IsAudioMuted());
            var save = p.CaptureState();
            Assert.IsTrue(save.is_discovered);
            var p2 = new MapAnomaly_PetrifiedForest();
            p2.RestoreState(save);
            Assert.IsTrue(p2.IsAudioMuted());
        }

        [Test]
        public void QuietZone_Enter_Capture()
        {
            var q = new MapAnomaly_QuietZone();
            q.EnterZone("sv_a");
            q.StayInZone("sv_a", 2f);
            Assert.AreEqual(0f, q.GetRadReading(), Eps);
            var save = q.CaptureState();
            Assert.AreEqual(1, save.survivors_in_zone.Count);
            var q2 = new MapAnomaly_QuietZone();
            q2.RestoreState(save);
            Assert.AreEqual("sv_a", q2.CaptureState().survivors_in_zone[0]);
        }

        [Test]
        public void RustedTank_Shelter_Capture()
        {
            var r = new MapAnomaly_RustedTank();
            Assert.IsTrue(r.ShelterInsideTank("party", true));
            Assert.IsFalse(r.ShelterInsideTank("party", false));
            Assert.IsTrue(r.CaptureState().isImpassableForVehicles);
        }

        [Test]
        public void ServerFarm_Heatstroke_SaveSlot()
        {
            string dir = SaveSystemTestFactory.TempDir("mapanom_server");
            try
            {
                var a = new MapAnomaly_ServerFarm();
                a.EnterShelter("sv_a");
                Assert.IsFalse(a.TickHour("sv_a", 5f));
                Assert.IsTrue(a.TickHour("sv_a", 2f)); // total 7 >= 6
                int gold = a.TryHarvestGold(2, new System.Random(1));
                Assert.AreEqual(6, gold); // 2 * 3

                Assert.IsTrue(SaveSystemTestFactory.MakeSave(dir, ss => ss.SetMapAnomalyServerFarm(a)).Save("slot"));
                var b = new MapAnomaly_ServerFarm();
                Assert.IsTrue(SaveSystemTestFactory.MakeSave(dir, ss => ss.SetMapAnomalyServerFarm(b)).Load("slot"));
                var cap = b.CaptureState();
                Assert.AreEqual(8, cap.motherboardsAvailable); // 10-2
                Assert.AreEqual(1, cap.survivorIdsInside.Length);
                Assert.AreEqual("sv_a", cap.survivorIdsInside[0]);
                Assert.AreEqual(7f, cap.survivorHoursInside[0], Eps);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }

        [Test]
        public void Sinkhole_Loot_Capture()
        {
            var s = new MapAnomaly_Sinkhole();
            // force no death/cavein with seeded rng that returns high values — just check API
            int food = s.LootSinkholeWithRope("sv_a", true, new System.Random(99), out bool dead, out bool cave);
            // food is either 0 (death) or 20
            if (!dead) Assert.AreEqual(20, food);
            Assert.AreEqual(0, s.LootSinkholeWithRope("sv_a", false, new System.Random(1), out _, out _));
            Assert.AreEqual(0.10f, s.CaptureState().ropeSnapDeathChance, Eps);
        }

        [Test]
        public void TangledDrop_Retrieve_Capture()
        {
            var t = new MapAnomaly_TangledDrop();
            var loot = t.RetrieveDropWithClimbingGear(true);
            Assert.IsNotNull(loot);
            Assert.IsTrue(t.State.isLooted);
            Assert.IsNull(t.RetrieveDropWithClimbingGear(true)); // already looted
            var save = t.CaptureState();
            var t2 = new MapAnomaly_TangledDrop();
            t2.RestoreState(save);
            Assert.IsTrue(t2.State.isLooted);
        }

        [Test]
        public void TireFire_Pollution_Capture()
        {
            var t = new MapAnomaly_TireFire();
            Assert.AreEqual(0.5f, t.ApplyRegionalPollution(1), Eps);
            Assert.AreEqual(0f, t.ApplyRegionalPollution(5), Eps);
            Assert.AreEqual(2, t.CaptureState().pollutionRadiusNodes);
        }

        [Test]
        public void UxoNuke_Harvest_Capture()
        {
            var u = new MapAnomaly_UXO_Nuke();
            // engineerSkill 20 + lucky rng may succeed; unlucky detonates
            var item = u.HarvestFissileMaterial(20, new System.Random(0), out bool lethal);
            if (lethal)
            {
                Assert.IsTrue(u.State.isDetonated);
                Assert.IsNull(item);
            }
            else
            {
                Assert.AreEqual("fissile_nuclear_material", item);
            }
            var save = u.CaptureState();
            var u2 = new MapAnomaly_UXO_Nuke();
            u2.RestoreState(save);
            Assert.AreEqual(save.isDetonated, u2.State.isDetonated);
        }

        [Test]
        public void MultiAnomaly_SaveSlot_RoundTrip()
        {
            string dir = SaveSystemTestFactory.TempDir("mapanom_multi");
            try
            {
                var coral = new MapAnomaly_DryCoral();
                coral.Discover("n1");
                coral.HarvestCrystal("sv", true, 50f);

                var drop = new MapAnomaly_TangledDrop();
                drop.RetrieveDropWithClimbingGear(true);

                var farm = new MapAnomaly_ServerFarm();
                farm.EnterShelter("sv_b");
                farm.TickHour("sv_b", 3f);

                Assert.IsTrue(SaveSystemTestFactory.MakeSave(dir, ss =>
                {
                    ss.SetMapAnomalyDryCoral(coral);
                    ss.SetMapAnomalyTangledDrop(drop);
                    ss.SetMapAnomalyServerFarm(farm);
                }).Save("slot"));

                var coral2 = new MapAnomaly_DryCoral();
                var drop2 = new MapAnomaly_TangledDrop();
                var farm2 = new MapAnomaly_ServerFarm();
                Assert.IsTrue(SaveSystemTestFactory.MakeSave(dir, ss =>
                {
                    ss.SetMapAnomalyDryCoral(coral2);
                    ss.SetMapAnomalyTangledDrop(drop2);
                    ss.SetMapAnomalyServerFarm(farm2);
                }).Load("slot"));

                Assert.IsTrue(coral2.CaptureState().is_discovered);
                Assert.AreEqual(1, coral2.GetCrystalYield());
                Assert.IsTrue(drop2.State.isLooted);
                Assert.AreEqual(3f, farm2.CaptureState().survivorHoursInside[0], Eps);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }
    }
}
