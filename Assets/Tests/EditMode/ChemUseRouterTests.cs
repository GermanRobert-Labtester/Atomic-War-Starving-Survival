using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Core;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Shared chem-use routing: inventory-style notify, polypharmacy, AI anti-rad
    /// consume, and Prompt #833 tolerance (duration / effectiveness / save).
    /// </summary>
    [TestFixture]
    public class ChemUseRouterTests
    {
        private const float Eps = 1e-3f;

        private ItemDefinition MakeItem(string id)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = ItemType.Medical;
            item.stackMax = 20;
            item.weight = 0.1f;
            return item;
        }

        private static void BindRouter(
            ChemUseRouter router,
            AddictionSystem addiction = null,
            BloodToxicitySystem blood = null,
            PolypharmacySystem poly = null,
            System_Tolerance tolerance = null,
            System.Func<int> getDay = null,
            System.Func<float> getGameHours = null)
        {
            router.Bind(
                addiction,
                blood,
                poly,
                tolerance,
                getDay ?? (() => 1),
                getGameHours ?? (() => 0f));
        }

        [Test]
        public void Notify_TrackedChem_FeedsAddictionBloodToxicityAndPolypharmacy()
        {
            var addiction = new AddictionSystem();
            addiction.RegisterAddictiveItem("morphine");
            addiction.RegisterAddictiveItem("anti_rad");
            var blood = new BloodToxicitySystem();
            var poly = new PolypharmacySystem();
            var router = new ChemUseRouter();
            BindRouter(router, addiction, blood, poly, getDay: () => 5, getGameHours: () => 12f);

            var sv = new Survivor { Id = "sv_chem", DisplayName = "Chem" };
            router.Notify(sv, "morphine");
            router.Notify(sv, "anti_rad");
            router.Notify(sv, "bandage"); // not poly / not blood-toxic / not addictive

            Assert.AreEqual(2, blood.States["sv_chem"].chemUsageCount);
            Assert.AreEqual(2, poly.RecentDoseCount("sv_chem", 12f),
                "Only polypharmacy drugs should log doses (bandage excluded).");
            Assert.IsNotNull(sv.ConsumptionHistory);
            Assert.AreEqual(2, sv.ConsumptionHistory.Count,
                "Addiction should log only registered addictive chems.");
        }

        [Test]
        public void Notify_Amphetamines_CountsTowardToxicThreshold()
        {
            var blood = new BloodToxicitySystem();
            var router = new ChemUseRouter();
            BindRouter(router, blood: blood);

            var sv = new Survivor { Id = "sv_a", DisplayName = "A" };
            for (int i = 0; i < 5; i++)
                router.Notify(sv, "amphetamines");

            Assert.IsTrue(blood.IsBloodToxic("sv_a"));
            Assert.AreEqual(100f, blood.States["sv_a"].bloodToxicityLevel, Eps);
        }

        [Test]
        public void UseAntiRadAction_ConsumesStock_AndReducesDose()
        {
            var anti = MakeItem("anti_rad");
            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            inv.Add(anti, 2);

            var sv = new Survivor { Id = "sv_rad", DisplayName = "Rad" };
            sv.RadiationDose = 80f;

            var action = ScriptableObject.CreateInstance<UseAntiRadActionSO>();
            var ctx = new AIContext
            {
                Survivor = sv,
                Inventory = inv
            };

            action.Execute(ctx);

            Assert.AreEqual(1, inv.CountById("anti_rad"));
            Assert.AreEqual(50f, sv.RadiationDose, Eps);

            Object.DestroyImmediate(anti);
            Object.DestroyImmediate(action);
        }

        [Test]
        public void UseAntiRadAction_NoStock_DoesNotReduceDose()
        {
            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            var sv = new Survivor { Id = "sv_empty", DisplayName = "Empty" };
            sv.RadiationDose = 80f;

            var action = ScriptableObject.CreateInstance<UseAntiRadActionSO>();
            var ctx = new AIContext { Survivor = sv, Inventory = inv };
            action.Execute(ctx);

            Assert.AreEqual(80f, sv.RadiationDose, Eps,
                "Without anti_rad stock, radiation must not drop.");
            Object.DestroyImmediate(action);
        }

        [Test]
        public void AiAntiRadPath_MirrorsRouterNotifyWhenDoseTaken()
        {
            // Mirrors GameBootstrap AI post-action: notify only if inventory dropped.
            var anti = MakeItem("anti_rad");
            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            inv.Add(anti, 1);

            var blood = new BloodToxicitySystem();
            var poly = new PolypharmacySystem();
            var router = new ChemUseRouter();
            BindRouter(router, blood: blood, poly: poly, getDay: () => 2, getGameHours: () => 48f);

            var sv = new Survivor { Id = "sv_ai", DisplayName = "AI" };
            sv.RadiationDose = 90f;
            var action = ScriptableObject.CreateInstance<UseAntiRadActionSO>();
            var ctx = new AIContext { Survivor = sv, Inventory = inv };

            int before = inv.CountById("anti_rad");
            action.Execute(ctx);
            bool doseTaken = inv.CountById("anti_rad") < before;
            Assert.IsTrue(doseTaken);
            if (doseTaken)
                router.Notify(sv, "anti_rad");

            Assert.AreEqual(1, blood.States["sv_ai"].chemUsageCount);
            Assert.AreEqual(1, poly.RecentDoseCount("sv_ai", 48f));
            Assert.AreEqual(60f, sv.RadiationDose, Eps);

            Object.DestroyImmediate(anti);
            Object.DestroyImmediate(action);
        }

        [Test]
        public void IsPolypharmacyDrug_KnownIds()
        {
            Assert.IsTrue(ChemUseRouter.IsPolypharmacyDrug("morphine"));
            Assert.IsTrue(ChemUseRouter.IsPolypharmacyDrug("ANTI_RAD"));
            Assert.IsFalse(ChemUseRouter.IsPolypharmacyDrug("bandage"));
            Assert.IsFalse(ChemUseRouter.IsPolypharmacyDrug(null));
        }

        // ── Prompt #833 — Tolerance ────────────────────────────────────

        [Test]
        public void Tolerance_FirstDose_FullDurationAndEffectiveness()
        {
            var tol = new System_Tolerance();
            var router = new ChemUseRouter();
            BindRouter(router, tolerance: tol, getGameHours: () => 10f);

            var sv = new Survivor { Id = "sv_t0", DisplayName = "T0" };

            Assert.AreEqual(System_Tolerance.BaseDurationHours, router.PeekDurationHours(sv, "morphine"), Eps);
            Assert.AreEqual(1f, router.PeekEffectiveness(sv, "morphine"), Eps);

            router.Notify(sv, "morphine");

            Assert.AreEqual(System_Tolerance.BaseDurationHours, router.LastAppliedDurationHours, Eps);
            Assert.AreEqual(1f, router.LastAppliedEffectiveness, Eps);
            Assert.AreEqual(1, tol.GetUseCount("sv_t0", "morphine"));
            // After first use, next dose is already reduced.
            Assert.AreEqual(System_Tolerance.DurationFromUseCount(1), tol.GetDuration("sv_t0", "morphine"), Eps);
            Assert.AreEqual(Mathf.Max(0.1f, 1f - 0.15f), tol.GetEffectiveness("sv_t0", "morphine"), Eps);
        }

        [Test]
        public void Tolerance_DurationDecaysWithRepeatedUse()
        {
            var tol = new System_Tolerance();
            var router = new ChemUseRouter();
            BindRouter(router, tolerance: tol, getGameHours: () => 0f);

            var sv = new Survivor { Id = "sv_td", DisplayName = "TD" };
            float[] expected =
            {
                System_Tolerance.DurationFromUseCount(0), // 24
                System_Tolerance.DurationFromUseCount(1), // 16
                System_Tolerance.DurationFromUseCount(2), // 12
                System_Tolerance.DurationFromUseCount(3), // 9.6
            };

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], router.PeekDurationHours(sv, "anti_rad"), Eps,
                    $"Pre-dose duration at use_count={i}");
                router.Notify(sv, "anti_rad");
                Assert.AreEqual(expected[i], router.LastAppliedDurationHours, Eps,
                    $"LastApplied duration at use_count={i}");
            }

            Assert.AreEqual(expected.Length, tol.GetUseCount("sv_td", "anti_rad"));
            Assert.Less(tol.GetDuration("sv_td", "anti_rad"), System_Tolerance.BaseDurationHours);
        }

        [Test]
        public void Tolerance_EffectivenessScalesThenHitsZeroAtSixUses()
        {
            var tol = new System_Tolerance();
            var router = new ChemUseRouter();
            BindRouter(router, tolerance: tol);

            var sv = new Survivor { Id = "sv_te", DisplayName = "TE" };

            // use_count 0..5 → effectiveness 1.0, 0.85, 0.70, 0.55, 0.40, 0.25
            float[] expectedEff = { 1f, 0.85f, 0.70f, 0.55f, 0.40f, 0.25f };
            for (int i = 0; i < expectedEff.Length; i++)
            {
                Assert.AreEqual(expectedEff[i], router.PeekEffectiveness(sv, "amphetamines"), Eps,
                    $"effectiveness at prior uses={i}");
                router.Notify(sv, "amphetamines");
                Assert.AreEqual(expectedEff[i], router.LastAppliedEffectiveness, Eps);
            }

            // 6th prior use → zero therapeutic benefit
            Assert.AreEqual(0f, router.PeekEffectiveness(sv, "amphetamines"), Eps);
            router.Notify(sv, "amphetamines");
            Assert.AreEqual(0f, router.LastAppliedEffectiveness, Eps);
            Assert.AreEqual(7, tol.GetUseCount("sv_te", "amphetamines"));
        }

        [Test]
        public void UseAntiRad_AppliesToleranceScaledCleanseAndResistance()
        {
            var anti = MakeItem("anti_rad");
            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            inv.Add(anti, 3);

            var tol = new System_Tolerance();
            // Two prior uses → effectiveness 0.70, duration 12h
            tol.UseChem("sv_scale", "anti_rad", 0f);
            tol.UseChem("sv_scale", "anti_rad", 1f);

            var router = new ChemUseRouter();
            BindRouter(router, tolerance: tol, getGameHours: () => 20f);

            var sv = new Survivor { Id = "sv_scale", DisplayName = "Scale" };
            sv.RadiationDose = 100f;

            var action = ScriptableObject.CreateInstance<UseAntiRadActionSO>();
            action.GetChemEffectiveness = (s, id) => router.PeekEffectiveness(s, id);
            action.GetChemDurationHours = (s, id) => router.PeekDurationHours(s, id);

            float eff = router.PeekEffectiveness(sv, "anti_rad");
            float dur = router.PeekDurationHours(sv, "anti_rad");
            Assert.AreEqual(0.70f, eff, Eps);
            Assert.AreEqual(System_Tolerance.DurationFromUseCount(2), dur, Eps);

            action.Execute(new AIContext { Survivor = sv, Inventory = inv });
            router.Notify(sv, "anti_rad");

            float expectedCleanse = UseAntiRadActionSO.BaseRadReduction * eff;
            Assert.AreEqual(100f - expectedCleanse, sv.RadiationDose, Eps);
            Assert.AreEqual(dur * eff, sv.RadResistanceHoursRemaining, Eps);
            Assert.IsTrue(sv.HasRadResistance);
            Assert.AreEqual(3, tol.GetUseCount("sv_scale", "anti_rad"));

            Object.DestroyImmediate(anti);
            Object.DestroyImmediate(action);
        }

        [Test]
        public void UseAntiRad_SixPriorUses_NoTherapeuticBenefit()
        {
            var anti = MakeItem("anti_rad");
            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            inv.Add(anti, 1);

            var tol = new System_Tolerance();
            for (int i = 0; i < 6; i++)
                tol.UseChem("sv_tol", "anti_rad", i);

            var router = new ChemUseRouter();
            BindRouter(router, tolerance: tol);

            var sv = new Survivor { Id = "sv_tol", DisplayName = "Tol" };
            sv.RadiationDose = 80f;

            var action = ScriptableObject.CreateInstance<UseAntiRadActionSO>();
            action.GetChemEffectiveness = (s, id) => router.PeekEffectiveness(s, id);
            action.GetChemDurationHours = (s, id) => router.PeekDurationHours(s, id);
            action.Execute(new AIContext { Survivor = sv, Inventory = inv });

            Assert.AreEqual(80f, sv.RadiationDose, Eps, "6+ uses → no cleanse");
            Assert.AreEqual(0f, sv.RadResistanceHoursRemaining, Eps);
            Assert.IsFalse(sv.HasRadResistance);

            Object.DestroyImmediate(anti);
            Object.DestroyImmediate(action);
        }

        [Test]
        public void Tolerance_SaveRoundTrip_PreservesUseCountAndDuration()
        {
            var a = new System_Tolerance();
            a.UseChem("sv_save", "morphine", 12f);
            a.UseChem("sv_save", "morphine", 24f);
            a.UseChem("sv_save", "anti_rad", 30f);

            var state = a.CaptureState();
            Assert.IsNotNull(state);
            Assert.AreEqual("system_tolerance", state.system_id);

            var b = new System_Tolerance();
            b.RestoreState(state);

            Assert.AreEqual(2, b.GetUseCount("sv_save", "morphine"));
            Assert.AreEqual(1, b.GetUseCount("sv_save", "anti_rad"));
            Assert.AreEqual(a.GetDuration("sv_save", "morphine"), b.GetDuration("sv_save", "morphine"), Eps);
            Assert.AreEqual(a.GetEffectiveness("sv_save", "morphine"), b.GetEffectiveness("sv_save", "morphine"), Eps);
            Assert.AreEqual(a.GetDuration("sv_save", "anti_rad"), b.GetDuration("sv_save", "anti_rad"), Eps);

            // Case-insensitive chem id after restore
            Assert.AreEqual(2, b.GetUseCount("sv_save", "MORPHINE"));
        }

        [Test]
        public void Tolerance_NonTrackedChem_FullEffectAndNoRecord()
        {
            var tol = new System_Tolerance();
            var router = new ChemUseRouter();
            BindRouter(router, tolerance: tol);

            var sv = new Survivor { Id = "sv_nt", DisplayName = "NT" };
            router.Notify(sv, "bandage");
            router.Notify(sv, "iodine");

            Assert.AreEqual(0, tol.GetUseCount("sv_nt", "bandage"));
            Assert.AreEqual(1f, router.LastAppliedEffectiveness, Eps);
            Assert.AreEqual(System_Tolerance.BaseDurationHours, router.LastAppliedDurationHours, Eps);
            Assert.IsFalse(System_Tolerance.IsToleranceChem("iodine"));
            Assert.IsTrue(ChemUseRouter.IsToleranceChem("anti_rad"));
            Assert.IsTrue(ChemUseRouter.IsToleranceChem("MORPHINE"));
        }

        [Test]
        public void InventoryConsume_TherapeuticScale_ScalesRadCleanse()
        {
            var item = MakeItem("anti_rad");
            item.healthEffect = 10f;
            item.hungerRestore = 5f;
            item.radCleanse = 40f;

            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            inv.Add(item, 1);

            var sv = new Survivor { Id = "sv_inv", DisplayName = "Inv" };
            sv.RadiationDose = 100f;

            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile);
            var rad = new RadiationSystem(needs);

            float scale = 0.5f;
            bool ok = inv.Consume(item, sv, rad, needs, scale);
            Assert.IsTrue(ok);
            Assert.AreEqual(100f - 40f * scale, sv.RadiationDose, Eps,
                "therapeuticScale must reduce rad cleanse only (hunger path unscaled).");

            Object.DestroyImmediate(item);
            Object.DestroyImmediate(profile);
        }
    }
}
