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

using AtomicWar._Game.Encounters;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// MapHazard_* wiring: 10 expedition/map hazards — API smoke + Capture/Restore + save slots.
    /// </summary>
    [TestFixture]
    public class MapHazardsWiringTests
    {
        private const float Eps = 1e-3f;

        // ── Acid Geyser ────────────────────────────────────────────────

        [Test]
        public void AcidGeyser_TickToEruption_Capture()
        {
            var g = new MapHazard_AcidGeyser();
            for (int i = 0; i < 3; i++)
                g.TickHour();
            Assert.IsTrue(g.IsErupting());

            var save = g.CaptureState();
            Assert.IsTrue(save.is_erupting);

            var g2 = new MapHazard_AcidGeyser();
            g2.RestoreState(save);
            Assert.IsTrue(g2.IsErupting());
            Assert.AreEqual(save.eruption_timer_minutes, g2.CaptureState().eruption_timer_minutes, Eps);
        }

        // ── Ashlanche ──────────────────────────────────────────────────

        [Test]
        public void Ashlanche_NoiseBuryDig_Capture()
        {
            var a = new MapHazard_Ashlanche();
            a.EnterNode("node_peak");
            a.MakeNoise(0.9f);
            a.BurySurvivors(new[] { "sv_a", "sv_b" });
            Assert.IsTrue(a.DigOut("sv_a", strength: 0.8f));

            var save = a.CaptureState();
            Assert.IsTrue(save.avalanche_triggered);
            Assert.AreEqual(1, save.buried_survivors.Count);
            Assert.AreEqual("sv_b", save.buried_survivors[0]);

            var a2 = new MapHazard_Ashlanche();
            a2.RestoreState(save);
            Assert.AreEqual(1, a2.CaptureState().buried_survivors.Count);
        }

        // ── Biometric Door ─────────────────────────────────────────────

        [Test]
        public void BiometricDoor_Unlock_Capture()
        {
            var d = new MapHazard_BiometricDoor();
            d.SetRequiredCommander("cmd_x");
            Assert.IsFalse(d.TryOpen("sv_a", new List<string> { "item_scrap" }));
            Assert.IsTrue(d.TryOpen("sv_a", new List<string> { "item_severed_hand_cmd_x" }));
            Assert.IsTrue(d.IsUnlocked());

            var save = d.CaptureState();
            var d2 = new MapHazard_BiometricDoor();
            d2.RestoreState(save);
            Assert.IsTrue(d2.IsUnlocked());
            Assert.AreEqual("cmd_x", d2.GetRequiredCommanderId());
        }

        // ── Crater Wall ────────────────────────────────────────────────

        [Test]
        public void CraterWall_PartialClimb_SaveSlot()
        {
            string dir = SaveSystemTestFactory.TempDir("maphaz_crater");
            try
            {
                var a = new MapHazard_CraterWall();
                // Partial progress: 2 of 4 hours
                Assert.IsFalse(a.AttemptClimb("sv_a", hasClimbingGear: true, currentFatigue: 0f, hours: 2f));
                var mid = a.CaptureState();
                Assert.AreEqual(1, mid.climbProgressIds.Length);
                Assert.AreEqual("sv_a", mid.climbProgressIds[0]);
                Assert.AreEqual(2f, mid.climbProgressHours[0], Eps);

                Assert.IsTrue(SaveSystemTestFactory.MakeSave(dir, ss => ss.SetMapHazardCraterWall(a)).Save("slot"));
                var b = new MapHazard_CraterWall();
                Assert.IsTrue(SaveSystemTestFactory.MakeSave(dir, ss => ss.SetMapHazardCraterWall(b)).Load("slot"));
                // Finish climb after restore
                Assert.IsTrue(b.AttemptClimb("sv_a", true, 0f, 2f));
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }

        // ── Crevice ────────────────────────────────────────────────────

        [Test]
        public void Crevice_Bridge_Capture()
        {
            var c = new MapHazard_Crevice();
            Assert.IsTrue(c.IsBlocking());
            Assert.IsTrue(c.BuildScrapBridge("sv_a", 5));
            Assert.IsFalse(c.IsBlocking());

            var save = c.CaptureState();
            Assert.IsTrue(save.bridgeBuilt);

            var c2 = new MapHazard_Crevice();
            c2.RestoreState(save);
            Assert.IsFalse(c2.IsBlocking());
        }

        // ── Flammable Gas ──────────────────────────────────────────────

        [Test]
        public void FlammableGas_Navigate_Capture()
        {
            var g = new MapHazard_FlammableGas();
            Assert.IsTrue(g.NavigateNode("sv_safe", new List<string> { "melee" }));
            Assert.IsFalse(g.NavigateNode("sv_dead", new List<string> { "firearm" }));

            var save = g.CaptureState();
            Assert.AreEqual(1, save.survivors_passed.Count);
            Assert.AreEqual(1, save.survivors_ignited.Count);

            var g2 = new MapHazard_FlammableGas();
            g2.RestoreState(save);
            Assert.AreEqual("sv_safe", g2.CaptureState().survivors_passed[0]);
        }

        // ── Gas Pockets ────────────────────────────────────────────────

        [Test]
        public void GasPockets_Ignite_Capture()
        {
            var g = new MapHazard_GasPockets();
            g.RegisterGasNode("n1");
            float dmg = g.FireWeapon("sv_a", "n1", "firearm", 3);
            Assert.AreEqual(80f, dmg, Eps);
            Assert.IsTrue(g.State.isIgnited);

            var save = g.CaptureState();
            Assert.AreEqual(1, save.gasNodes.Count);
            Assert.AreEqual(1, save.ignitedNodes.Count);

            var g2 = new MapHazard_GasPockets();
            g2.RestoreState(save);
            Assert.AreEqual(0f, g2.FireWeapon("sv_b", "n1", "firearm", 1), Eps); // already ignited
            Assert.AreEqual(1, g2.CaptureState().ignitedNodes.Count);
        }

        // ── Magnetic Anomaly ───────────────────────────────────────────

        [Test]
        public void MagneticAnomaly_EnterExit_Capture()
        {
            var m = new MapHazard_MagneticAnomaly();
            m.EnterNode("n_mag");
            Assert.IsTrue(m.IsActive("n_mag"));
            m.ExitNode("n_mag");
            Assert.IsFalse(m.IsActive("n_mag"));

            m.EnterNode("n_a");
            m.EnterNode("n_b");
            var save = m.CaptureState();
            Assert.AreEqual(2, save.affected_nodes.Count);

            var m2 = new MapHazard_MagneticAnomaly();
            m2.RestoreState(save);
            Assert.IsTrue(m2.IsActive("n_a"));
            Assert.IsTrue(m2.IsActive("n_b"));
        }

        // ── Sinkhole ───────────────────────────────────────────────────

        [Test]
        public void Sinkhole_ForcedCollapse_Capture()
        {
            // collapseChance 1.0 guarantees collapse
            var s = new MapHazard_SinkholeCollapse(new UrbanSinkholeState
            {
                hazardId = "map_hazard_sinkhole_collapse",
                collapseChance = 1f,
                dropsToSubway = true
            });
            string subway = null;
            s.OnCollapseTriggered += (id, node) => subway = node;
            Assert.IsTrue(s.WalkOver("sv_a", "street_1", new System.Random(1)));
            Assert.AreEqual("subway_below_street_1", subway);

            var save = s.CaptureState();
            Assert.AreEqual(1f, save.collapseChance, Eps);

            var s2 = new MapHazard_SinkholeCollapse();
            s2.RestoreState(save);
            Assert.AreEqual(1f, s2.State.collapseChance, Eps);
        }

        // ── Venus Trap ─────────────────────────────────────────────────

        [Test]
        public void VenusTrap_Harvest_Capture()
        {
            var v = new MapHazard_VenusTrap();
            v.EnterNode();
            Assert.IsTrue(v.IsDisguisedAsBerry());
            Assert.IsFalse(v.AttemptHarvest("sv_weak", strength: 0.1f));
            Assert.AreEqual(1, v.GetArmLossResult());
            Assert.IsTrue(v.AttemptHarvest("sv_strong", strength: 0.9f));

            var save = v.CaptureState();
            Assert.AreEqual(1, save.amputations);
            Assert.IsTrue(save.triggered_survivors.Contains("sv_weak"));

            var v2 = new MapHazard_VenusTrap();
            v2.RestoreState(save);
            Assert.AreEqual(1, v2.GetArmLossResult());
        }

        // ── Multi-slot ─────────────────────────────────────────────────

        [Test]
        public void MultiHazard_SaveSlot_RoundTrip()
        {
            string dir = SaveSystemTestFactory.TempDir("maphaz_multi");
            try
            {
                var geyser = new MapHazard_AcidGeyser();
                geyser.TickHour();
                geyser.TickHour();

                var crevice = new MapHazard_Crevice();
                crevice.BuildScrapBridge("builder", 10);

                var gas = new MapHazard_GasPockets();
                gas.RegisterGasNode("gas_a");

                Assert.IsTrue(SaveSystemTestFactory.MakeSave(dir, ss =>
                {
                    ss.SetMapHazardAcidGeyser(geyser);
                    ss.SetMapHazardCrevice(crevice);
                    ss.SetMapHazardGasPockets(gas);
                }).Save("slot"));

                var geyser2 = new MapHazard_AcidGeyser();
                var crevice2 = new MapHazard_Crevice();
                var gas2 = new MapHazard_GasPockets();
                Assert.IsTrue(SaveSystemTestFactory.MakeSave(dir, ss =>
                {
                    ss.SetMapHazardAcidGeyser(geyser2);
                    ss.SetMapHazardCrevice(crevice2);
                    ss.SetMapHazardGasPockets(gas2);
                }).Load("slot"));

                Assert.AreEqual(2f, geyser2.CaptureState().hours_since_eruption, Eps);
                Assert.IsTrue(crevice2.CaptureState().bridgeBuilt);
                Assert.AreEqual(1, gas2.CaptureState().gasNodes.Count);
                Assert.AreEqual("gas_a", gas2.CaptureState().gasNodes[0]);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }
    }
}
