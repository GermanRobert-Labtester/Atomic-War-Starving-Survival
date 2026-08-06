using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Core;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Shared chem-use routing: inventory-style notify, polypharmacy, AI anti-rad consume.
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

        [Test]
        public void Notify_TrackedChem_FeedsAddictionBloodToxicityAndPolypharmacy()
        {
            var addiction = new AddictionSystem();
            addiction.RegisterAddictiveItem("morphine");
            addiction.RegisterAddictiveItem("anti_rad");
            var blood = new BloodToxicitySystem();
            var poly = new PolypharmacySystem();
            var router = new ChemUseRouter();
            router.Bind(addiction, blood, poly, getDay: () => 5, getGameHours: () => 12f);

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
            router.Bind(null, blood, null, () => 1, () => 0f);

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
            router.Bind(null, blood, poly, () => 2, () => 48f);

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
    }
}
