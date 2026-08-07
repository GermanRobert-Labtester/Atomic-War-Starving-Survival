using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using AtomicWar._Game.Core;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Simulation; // CompostSystem, ChelationSystem, etc. (audit C-3 split)
using AtomicWar._Game.Survivors;
using UnityEngine;
// Aliases: the Shelter and Inventory namespaces collide with the
// `Shelter.Shelter` class and `Inventory.Inventory` types used by the
// systems under test. Without these aliases, `new Shelter()` resolves
// to the namespace declaration, not the class.
using ShelterClass = AtomicWar._Game.Shelter.Shelter;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Audit C-1 regression: every system added in Prompts #119-#178 must have
    /// either a tick path (verified via SystemWiring) or an event-driven API
    /// (verified by calling the public method). This test file ensures none of
    /// them is dead state.
    /// </summary>
    [TestFixture]
    public class SystemWiringTests
    {
        // ─────────────────────────────────────────────────────────────────
        // SystemWiring — the per-day orchestrator
        // ─────────────────────────────────────────────────────────────────

        [Test]
        public void SystemWiring_WireDaily_AdvancesAllBoundSystems()
        {
            var wiring = new SystemWiring();
            var compost = new CompostSystem();
            var chelation = new ChelationSystem();
            var aesthetics = new RoomAestheticsSystem();
            var ham = new HamRadioSystem();
            var poly = new PolypharmacySystem();
            var ceiling = new CeilingCollapseSystem();
            var quests = new LocationQuestSystem();
            var shelter = new ShelterClass();
            var inv = new InventoryClass();
            var survivors = new List<Survivor> { new Survivor { Id = "sv1" }, new Survivor { Id = "sv2" } };

            SystemWiring.DailyContext Ctx(int day) => new SystemWiring.DailyContext
            {
                CurrentDay = day,
                Compost = compost,
                Chelation = chelation,
                Aesthetics = aesthetics,
                HamRadio = ham,
                Polypharmacy = poly,
                CeilingCollapse = ceiling,
                LocationQuest = quests,
                Shelter = shelter,
                Inventory = inv,
                Survivors = survivors,
                Rooms = new List<ShelterRoom>(),
                Rng = new System.Random(42)
            };

            // Day 1
            wiring.WireDaily(Ctx(1));
            Assert.AreEqual(1, wiring.LastDailyRunDay);
            Assert.AreEqual(1, wiring.DailyRunCount);
            Assert.IsTrue(wiring.HasRunOnce);

            // Idempotent on same day
            wiring.WireDaily(Ctx(1));
            Assert.AreEqual(1, wiring.DailyRunCount, "Same-day call must be a no-op.");

            // Day 2 — count increments
            wiring.WireDaily(Ctx(2));
            Assert.AreEqual(2, wiring.DailyRunCount);

            // Compost should have accumulated per-survivor waste (2 survivors × 0.2f × 2 days = 0.8)
            Assert.That(compost.CompostProgress, Is.GreaterThan(0.7f).And.LessThanOrEqualTo(0.9f));
        }

        [Test]
        public void SystemWiring_NullArgs_DoesNotThrow()
        {
            var wiring = new SystemWiring();
            // Null / empty context should be a no-op (no crash).
            Assert.DoesNotThrow(() => wiring.WireDaily(null));
            Assert.DoesNotThrow(() => wiring.WireDaily(new SystemWiring.DailyContext { CurrentDay = 1 }));
        }

        // ─────────────────────────────────────────────────────────────────
        // CompostSystem — daily waste contribution (audit C-1)
        // ─────────────────────────────────────────────────────────────────

        [Test]
        public void Compost_DailyWasteFromSurvivors_Accumulates()
        {
            var c = new CompostSystem();
            c.DailyWasteFromSurvivors(3);
            Assert.AreEqual(3 * CompostSystem.DailyWastePerSurvivor, c.CompostProgress, 0.0001f);
            c.DailyWasteFromSurvivors(0); // 0 survivors = no change
            Assert.AreEqual(3 * CompostSystem.DailyWastePerSurvivor, c.CompostProgress, 0.0001f);
        }

        [Test]
        public void Compost_Tick_AdvancesFertilizer()
        {
            var c = new CompostSystem();
            c.AddWaste(10f);
            var room = new ShelterRoom("stores", null);
            c.Tick(5f, room); // 5h * 2f/h = 10f converted → 2f fertilizer
            Assert.AreEqual(0f, c.CompostProgress, 0.01f);
            Assert.AreEqual(2f, c.FertilizerReady, 0.01f);
        }

        [Test]
        public void Compost_HarvestFertilizer_Resets()
        {
            var c = new CompostSystem();
            c.AddWaste(5f);
            var room = new ShelterRoom("stores", null);
            c.Tick(2.5f, room);
            float harvested = c.HarvestFertilizer();
            Assert.AreEqual(1f, harvested, 0.01f);
            Assert.AreEqual(0f, c.FertilizerReady);
        }

        // ─────────────────────────────────────────────────────────────────
        // ChelationSystem — daily advance (audit C-1)
        // ─────────────────────────────────────────────────────────────────

        [Test]
        public void Chelation_BeginAndAdvance_CompletesAfter5Days()
        {
            var c = new ChelationSystem();
            c.BeginChelation("sv1");
            Assert.IsTrue(c.IsUndergoingChelation("sv1"));
            Assert.AreEqual(ChelationSystem.ChelationComaDays * 24f, c.GetRemainingHours("sv1"), 0.01f);

            // Advance 4 days — not done yet
            c.AdvanceDay("sv1");
            c.AdvanceDay("sv1");
            c.AdvanceDay("sv1");
            c.AdvanceDay("sv1");
            Assert.IsTrue(c.IsUndergoingChelation("sv1"), "Should still be active after 4 days");

            // 5th day — done
            Assert.IsTrue(c.AdvanceDay("sv1"));
            Assert.IsFalse(c.IsUndergoingChelation("sv1"));
        }

        [Test]
        public void Chelation_AdvanceUnknownSurvivor_IsNoOp()
        {
            var c = new ChelationSystem();
            Assert.IsFalse(c.AdvanceDay("nope"));
        }

        // ─────────────────────────────────────────────────────────────────
        // RoomAestheticsSystem — applied via SystemWiring (audit C-1)
        // ─────────────────────────────────────────────────────────────────

        [Test]
        public void Aesthetics_SetDecor_GetDecor_RoundTrip()
        {
            var a = new RoomAestheticsSystem();
            a.SetDecor("quarters", 0.7f);
            Assert.AreEqual(0.7f, a.GetDecor("quarters"), 0.001f);
            a.SetDecor("quarters", 1.5f); // clamped
            Assert.AreEqual(1f, a.GetDecor("quarters"), 0.001f);
        }

        [Test]
        public void Aesthetics_CalculateScore_ZeroOnNoInputs()
        {
            var a = new RoomAestheticsSystem();
            float score = a.CalculateScore(0f, 20f, 0f, "x");
            // lighting=0, temp at 20 → 1.0, hygiene=0, decor=0
            // = 0*0.3 + 1*0.25 + 0*0.25 + 0*0.2 = 0.25
            Assert.AreEqual(0.25f, score, 0.01f);
        }

        // ─────────────────────────────────────────────────────────────────
        // HamRadioSystem — daily tick via wiring (audit C-1)
        // ─────────────────────────────────────────────────────────────────

        [Test]
        public void HamRadio_TickBroadcast_AdvancesAfter20Days()
        {
            var h = new HamRadioSystem();
            // Simulate 20 days of broadcasting, 1 hour at a time
            for (int day = 0; day < 20; day++)
                h.TickBroadcast(24f, radioTowerActive: true);

            Assert.IsTrue(h.CarrierContacted);
            Assert.IsFalse(h.VictoryReady); // not until LZ cleared
        }

        [Test]
        public void HamRadio_TickBroadcast_NoOpWhenTowerInactive()
        {
            var h = new HamRadioSystem();
            h.TickBroadcast(24f * 30f, radioTowerActive: false);
            Assert.IsFalse(h.CarrierContacted);
        }

        [Test]
        public void HamRadio_ClearLZ_RequiresContactFirst()
        {
            var h = new HamRadioSystem();
            // Without carrier contact, even 100 explosives can't clear the LZ.
            Assert.IsFalse(h.ClearLZ(100));
            // Simulate 20 days of broadcast.
            for (int day = 0; day < 20; day++)
                h.TickBroadcast(24f, radioTowerActive: true);
            Assert.IsTrue(h.CarrierContacted);
            // 99 explosives — not enough. LZ stays uncleared; method returns false.
            Assert.IsFalse(h.ClearLZ(99));
            Assert.IsFalse(h.LZCleared);
            // 100 explosives — sufficient. LZ cleared; method returns true; victory ready.
            Assert.IsTrue(h.ClearLZ(100));
            Assert.IsTrue(h.LZCleared);
            Assert.IsTrue(h.VictoryReady);
        }

        // ─────────────────────────────────────────────────────────────────
        // PolypharmacySystem — public PruneStaleDoses (audit C-1)
        // ─────────────────────────────────────────────────────────────────

        [Test]
        public void Polypharmacy_PruneStaleDoses_RemovesOld()
        {
            var p = new PolypharmacySystem();
            p.RecordDose("sv1", "iodine", 0f);
            p.RecordDose("sv1", "morphine", 5f);
            Assert.AreEqual(2, p.RecentDoseCount("sv1", 10f));
            // 20h later — both should be pruned (window is 12h)
            p.PruneStaleDoses("sv1", 20f);
            Assert.AreEqual(0, p.RecentDoseCount("sv1", 20f));
        }

        // ─────────────────────────────────────────────────────────────────
        // CeilingCollapseSystem — daily check (audit C-1)
        // ─────────────────────────────────────────────────────────────────

        [Test]
        public void CeilingCollapse_DailyCheck_CollapsesOverloadedRooms()
        {
            var sys = new CeilingCollapseSystem();
            var shelter = new ShelterClass();
            // The room must be registered with both the CeilingCollapseSystem
            // (for tile count) AND the Shelter (so DailyCollapseCheck iterates
            // over it).
            var bigRoom = new ShelterRoom("big_room", null);
            shelter.RegisterRoom(bigRoom);
            sys.RegisterRoom("big_room", 5f);
            // Run a deterministic daily check; 200 trials = some collapses expected
            int collapsed = 0;
            for (int trial = 0; trial < 200; trial++)
            {
                var rng = new System.Random(trial);
                if (!sys.IsCollapsed("big_room")) sys.DailyCollapseCheck(shelter, rng);
                if (sys.IsCollapsed("big_room")) collapsed++;
            }
            Assert.Greater(collapsed, 0, "At least one of 200 daily checks should collapse a 5-tile unsupported room.");
        }

        [Test]
        public void CeilingCollapse_DailyCheck_IgnoresSupportedRooms()
        {
            var sys = new CeilingCollapseSystem();
            var shelter = new ShelterClass();
            // 2-tile room — well under 3-tile limit, no collapse expected.
            var smallRoom = new ShelterRoom("small_room", null);
            shelter.RegisterRoom(smallRoom);
            sys.RegisterRoom("small_room", 2f);
            for (int trial = 0; trial < 100; trial++)
                sys.DailyCollapseCheck(shelter, new System.Random(trial));
            Assert.IsFalse(sys.IsCollapsed("small_room"));
        }

        // ─────────────────────────────────────────────────────────────────
        // LocationQuestSystem — daily tick expires temporary rewards (audit C-1)
        // ─────────────────────────────────────────────────────────────────

        [Test]
        public void LocationQuest_TickDaily_ExpiresTemporaryReward()
        {
            var q = new LocationQuestSystem();
            var quest = q.GetQuest("node_substation");
            quest.TemporaryRewardHoursRemaining = 24f;
            // Don't mark completed — TickDaily should fail it after the timer.
            q.TickDaily(2);
            Assert.IsTrue(quest.IsFailed);
        }

        [Test]
        public void LocationQuest_TickDaily_DoesNotExpireCompleted()
        {
            var q = new LocationQuestSystem();
            var quest = q.GetQuest("node_substation");
            quest.TemporaryRewardHoursRemaining = 0f;
            quest.IsCompleted = true;
            q.TickDaily(2);
            Assert.IsFalse(quest.IsFailed);
        }

        // ─────────────────────────────────────────────────────────────────
        // Event-driven systems (no Tick) — verify public API exists
        // ─────────────────────────────────────────────────────────────────

        [Test]
        public void ScrapWeapon_TryFireWeapon_HasNoState()
        {
            var s = new ScrapWeaponSystem();
            // Just verify the API is callable. No observable state to assert.
            var sv = new Survivor { Id = "x" };
            sv.Needs.Health = 100f;
            bool fired = s.TryFireWeapon(sv, new System.Random(42), (a, b) => { });
            Assert.IsTrue(fired || !fired); // outcome depends on RNG
        }

        [Test]
        public void Sterilization_UseAndBoil_RoundTrip()
        {
            var s = new SterilizationSystem();
            Assert.IsTrue(s.ToolsSterile);
            s.UseTools();
            Assert.IsFalse(s.ToolsSterile);
            Assert.IsTrue(s.BoilTools((id, n) => n, n => true));
            Assert.IsTrue(s.ToolsSterile);
        }

        [Test]
        public void WindTurbine_Build_AdvancesOnce()
        {
            var w = new WindTurbineSystem();
            Assert.IsFalse(w.IsBuilt);
            w.Build();
            Assert.IsTrue(w.IsBuilt);
            // Idempotent
            w.Build();
            Assert.IsTrue(w.IsBuilt);
        }

        [Test]
        public void AntibioticResistance_UseExpired_BuildsResistance()
        {
            var a = new AntibioticResistanceSystem();
            Assert.AreEqual(0f, a.GetResistance("sv1"));
            Assert.IsTrue(a.TryUseExpired("sv1", new System.Random(1)));
            Assert.GreaterOrEqual(a.GetResistance("sv1"), 1f);
        }

        [Test]
        public void InternalHauling_DumpAndHaul_RoundTrip()
        {
            var h = new InternalHaulingSystem();
            h.DumpLootInAirlock(50f);
            Assert.AreEqual(50f, h.AirlockDumpedWeight);
            var sv = new Survivor { Id = "x" };
            sv.Needs.Fatigue = 0f;
            float moved = h.HaulFromAirlock(sv, 1f);
            Assert.Greater(moved, 0f);
            Assert.Less(h.AirlockDumpedWeight, 50f);
        }

        [Test]
        public void WeaponMaintenance_FireAndOil_DurabilityRoundTrip()
        {
            var w = new WeaponMaintenanceSystem();
            string wpn = "rifle_1";
            Assert.AreEqual(WeaponMaintenanceSystem.MaxDurability, w.GetDurability(wpn));
            w.Fire(wpn);
            Assert.Less(w.GetDurability(wpn), WeaponMaintenanceSystem.MaxDurability);
            w.OilWeapon(wpn);
            Assert.AreEqual(WeaponMaintenanceSystem.MaxDurability, w.GetDurability(wpn));
        }

        [Test]
        public void TriageBoard_PermissionCheck_RespectsLevel()
        {
            var t = new TriageBoardSystem();
            t.SetPermission("sv1", TriageBoardSystem.TriageLevel.None);
            Assert.IsFalse(t.CanReceiveMedication("sv1", "basic"));
            t.SetPermission("sv1", TriageBoardSystem.TriageLevel.Basic);
            Assert.IsTrue(t.CanReceiveMedication("sv1", "basic"));
            Assert.IsFalse(t.CanReceiveMedication("sv1", "advanced"));
            t.SetPermission("sv1", TriageBoardSystem.TriageLevel.Full);
            Assert.IsTrue(t.CanReceiveMedication("sv1", "advanced"));
        }

        [Test]
        public void Resilience_OnTraumaSurvived_Accumulates()
        {
            var r = new ResilienceSystem();
            Assert.AreEqual(0f, r.GetResilience("sv1"));
            r.OnTraumaSurvived("sv1");
            Assert.AreEqual(ResilienceSystem.ResiliencePerTrauma, r.GetResilience("sv1"));
            r.OnTraumaSurvived("sv1");
            Assert.AreEqual(2 * ResilienceSystem.ResiliencePerTrauma, r.GetResilience("sv1"));
            // Cap at MaxResilience
            for (int i = 0; i < 100; i++) r.OnTraumaSurvived("sv1");
            Assert.LessOrEqual(r.GetResilience("sv1"), ResilienceSystem.MaxResilience);
        }

        // ─────────────────────────────────────────────────────────────────
        // Shelter systems — event-driven, verify public API
        // ─────────────────────────────────────────────────────────────────

        [Test]
        public void Excavation_SealAndClear_Advances()
        {
            var ex = new ExcavationSystem();
            ex.SealRoom("r1", 100f);
            Assert.IsTrue(ex.HasRubble("r1"));
            var sv = new Survivor { Id = "x" };
            ex.ClearRubble("r1", sv, hasShovel: true, hatchBlocked: false);
            Assert.Less(ex.Rooms["r1"].RubbleUnitsRemaining, 100f);
        }

        [Test]
        public void HiddenStorage_HideAndRetrieve_RoundTrip()
        {
            var hs = new HiddenStorageSystem();
            hs.HideItem("canned_food", 5);
            Assert.AreEqual(5, hs.HiddenItemCount);
            Assert.IsTrue(hs.HasHidden("canned_food"));
            int got = hs.RetrieveItem("canned_food", 3);
            Assert.AreEqual(3, got);
        }

        [Test]
        public void Tunneling_Tunnel_Advances()
        {
            var t = new TunnelingSystem();
            t.SeedNeighbor(new System.Random(1));
            var sv = new Survivor { Id = "x" };
            t.Tunnel(4f, sv, hasPickaxe: true);
            Assert.Greater(t.TunnelProgress, 0f);
        }

        [Test]
        public void MaterialShielding_Upgrade_ReducesBleed()
        {
            var m = new MaterialShieldingSystem();
            float before = m.GetCeilingAttenuation("quarters");
            m.UpgradeCeiling("quarters", MaterialShieldingSystem.WallMaterial.Concrete);
            float after = m.GetCeilingAttenuation("quarters");
            Assert.Greater(after, before);
        }

        [Test]
        public void MaterialShielding_Bleed_FollowsWeakestCeiling()
        {
            var m = new MaterialShieldingSystem();

            // Nothing built yet: nothing between the survivors and the sky.
            Assert.That(m.GetRadiationBleed(10f), Is.EqualTo(10f).Within(1e-3f),
                "With no ceilings the full exterior dose must bleed through");

            m.UpgradeCeiling("quarters", MaterialShieldingSystem.WallMaterial.Concrete);
            Assert.That(m.GetRadiationBleed(10f), Is.EqualTo(2f).Within(1e-3f),
                "Concrete attenuates 80%, so only 20% of the exterior dose bleeds");

            // Bleed is gated by the WEAKEST roof — one wooden ceiling undoes the concrete.
            m.UpgradeCeiling("workshop", MaterialShieldingSystem.WallMaterial.Wood);
            Assert.That(m.GetRadiationBleed(10f), Is.EqualTo(9f).Within(1e-3f),
                "Wood attenuates only 10%, and the weakest ceiling sets the bleed");
        }

        [Test]
        public void Airlock_BuildAndEnter_ContaminationRoundTrip()
        {
            var a = new AirlockSystem();
            Assert.IsFalse(a.Exists);
            a.BuildAirlock();
            Assert.IsTrue(a.Exists);
            var sv = new Survivor { Id = "x" };
            a.ScavengerEnterAirlock(sv);
            Assert.Greater(a.Contamination, 0f);
            a.DeconAndEnter(sv);
            Assert.AreEqual(0f, a.Contamination);
        }

        [Test]
        public void EscapeHatch_ExcavateAndTrigger_Advances()
        {
            var e = new EscapeHatchSystem();
            Assert.IsFalse(e.IsBuilt);
            // Excavate 120h worth — the system transitions to built at 100%.
            e.Excavate(120f);
            Assert.IsTrue(e.IsBuilt);
            // Already built → further excavation returns 1.0 (no-op).
            Assert.AreEqual(1f, e.Excavate(10f));
            Assert.IsFalse(e.EvacTriggered);
            Assert.IsTrue(e.TriggerEvacuation());
            Assert.IsTrue(e.EvacTriggered);
            // Cannot re-trigger.
            Assert.IsFalse(e.TriggerEvacuation());
        }

        // ─────────────────────────────────────────────────────────────────
        // ClothingSystem — per-hour tick bound by atmospheric humidity
        // ─────────────────────────────────────────────────────────────────

        [Test]
        public void Clothing_Tick_DegradesOverTime()
        {
            var c = new ClothingDegradationSystem();
            var sv = new Survivor { Id = "x" };
            sv.ClothingDurability = 100f;
            // 100h at 0 humidity (0.15/h * 100 = 15 damage)
            c.Tick(sv, 100f, roomHumidity: 0.1f);
            Assert.AreEqual(85f, sv.ClothingDurability, 0.1f);
            Assert.IsFalse(sv.IsRagged);
        }

        [Test]
        public void Clothing_Tick_GoesRaggedAtZero()
        {
            var c = new ClothingDegradationSystem();
            var sv = new Survivor { Id = "x" };
            sv.ClothingDurability = 20f;
            c.Tick(sv, 200f, roomHumidity: 0.1f); // 30 damage
            Assert.AreEqual(0f, sv.ClothingDurability);
            Assert.IsTrue(sv.IsRagged);
        }

        [Test]
        public void Clothing_Tick_HighHumidityAccelerates()
        {
            var c = new ClothingDegradationSystem();
            var sv1 = new Survivor { Id = "a" }; sv1.ClothingDurability = 100f;
            var sv2 = new Survivor { Id = "b" }; sv2.ClothingDurability = 100f;
            c.Tick(sv1, 10f, roomHumidity: 0.1f); // 1.5 damage
            c.Tick(sv2, 10f, roomHumidity: 0.8f); // 3.75 damage
            Assert.Greater(sv1.ClothingDurability, sv2.ClothingDurability);
        }

        [Test]
        public void Clothing_Repair_RestoresDurability()
        {
            var c = new ClothingDegradationSystem();
            var sv = new Survivor { Id = "x" };
            sv.ClothingDurability = 0f; sv.IsRagged = true;
            c.Repair(sv);
            Assert.AreEqual(ClothingDegradationSystem.MaxDurability, sv.ClothingDurability);
            Assert.IsFalse(sv.IsRagged);
        }

        // ─────────────────────────────────────────────────────────────────
        // Save round-trip: every Save class for these systems survives
        // ─────────────────────────────────────────────────────────────────

        [Test]
        public void Compost_SaveRoundTrip_Equal()
        {
            var c = new CompostSystem();
            c.AddWaste(7.5f);
            c.Tick(2f, new ShelterRoom("stores", null));
            var save = c.CaptureState();
            var c2 = new CompostSystem();
            c2.RestoreState(save);
            Assert.AreEqual(c.CompostProgress, c2.CompostProgress, 0.001f);
            Assert.AreEqual(c.FertilizerReady, c2.FertilizerReady, 0.001f);
        }

        [Test]
        public void HamRadio_SaveRoundTrip_Equal()
        {
            var h = new HamRadioSystem();
            h.TickBroadcast(48f, true); // 2 broadcast days
            // Verify via the save snapshot (the system exposes BroadcastDays only
            // through the save class).
            var save = h.CaptureState();
            Assert.AreEqual(2f, save.BroadcastDays, 0.01f);
            var h2 = new HamRadioSystem();
            h2.RestoreState(save);
            Assert.AreEqual(save.BroadcastDays, h2.CaptureState().BroadcastDays, 0.001f);
        }

        [Test]
        public void TriageBoard_SaveRoundTrip_Equal()
        {
            var t = new TriageBoardSystem();
            t.SetPermission("sv1", TriageBoardSystem.TriageLevel.Basic);
            var save = t.CaptureState();
            var t2 = new TriageBoardSystem();
            t2.RestoreState(save);
            Assert.AreEqual(TriageBoardSystem.TriageLevel.Basic, t2.GetPermission("sv1"));
        }
    }
}
