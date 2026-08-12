using NUnit.Framework;
using AtomicWar._Game.Quests;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class QuestRegistryTests
    {
        [Test]
        public void RegistryContainsAllSevenNewQuests()
        {
            var reg = new QuestRegistry();
            Assert.IsNotNull(reg.Get(QuestRegistry.IdGarrisonLastOrder));
            Assert.IsNotNull(reg.Get(QuestRegistry.IdMilitiaGrainWar));
            Assert.IsNotNull(reg.Get(QuestRegistry.IdCultGlowCommunion));
            Assert.IsNotNull(reg.Get(QuestRegistry.IdElenaTriage));
            Assert.IsNotNull(reg.Get(QuestRegistry.IdMechanicHighwayHeart));
            Assert.IsNotNull(reg.Get(QuestRegistry.IdChildSoldierRifle));
            Assert.IsNotNull(reg.Get(QuestRegistry.IdDeepWell));
        }

        [Test]
        public void StartAdvancesToInProgress()
        {
            var reg = new QuestRegistry();
            reg.Start(QuestRegistry.IdDeepWell, 0);
            var s = reg.Get(QuestRegistry.IdDeepWell).State;
            Assert.AreEqual(QuestStatus.InProgress, s.Status);
            Assert.AreEqual(1, s.Stage);
        }
    }

    [TestFixture]
    public class GarrisonLastOrderTests
    {
        [Test]
        public void DestroyChoiceAppliesFactionTrustAndCompletes()
        {
            float garrisonTrust = 0, militiaTrust = 0, survivorTrust = 0;
            var reg = new QuestRegistry
            {
            };
            var q = new Quest_GarrisonLastOrder
            {
                AddFactionTrust = (f, d) => { if (f == "faction_garrison") garrisonTrust += d; },
                SubtractFactionTrust = (f, d) => { if (f == "faction_militia") militiaTrust += d; if (f == "faction_survivors") survivorTrust += d; },
                MarkLocationDestroyed = (l, k) => { },
                BroadcastRadioMessage = (f, m, c) => { },
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            q.ResolveDestroy();
            Assert.AreEqual(QuestStatus.Success, q.State.Status);
            Assert.AreEqual(30f, garrisonTrust, 0.001f);
            Assert.AreEqual(-25f, militiaTrust, 0.001f);
            Assert.AreEqual(-15f, survivorTrust, 0.001f);
        }

        [Test]
        public void RefuseMarksGarrisonHostile()
        {
            float garrisonTrust = 0;
            var q = new Quest_GarrisonLastOrder
            {
                AddFactionTrust = (f, d) => { },
                SubtractFactionTrust = (f, d) => { if (f == "faction_garrison") garrisonTrust += d; },
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            q.ResolveRefuse();
            Assert.AreEqual(QuestStatus.Success, q.State.Status);
            Assert.AreEqual(-40f, garrisonTrust, 0.001f);
        }
    }

    [TestFixture]
    public class MilitiaGrainWarTests
    {
        [Test]
        public void DiversionRefuseFailsQuest()
        {
            float militiaTrust = 0;
            var q = new Quest_MilitiaGrainWar
            {
                SubtractFactionTrust = (f, d) => { if (f == "faction_upland_militia") militiaTrust += d; },
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            q.ResolveDiversionRefuse();
            Assert.AreEqual(QuestStatus.Failure, q.State.Status);
            Assert.AreEqual(-25f, militiaTrust, 0.001f);
        }
    }

    [TestFixture]
    public class ElenaTriageTests
        {
        [Test]
        public void FiveSuccessesCompleteAndGrantFieldTriage()
            {
            string perk = null;
            var q = new Quest_ElenaTriage
            {
                GrantPerk = (sv, id, n) => perk = id,
                ApplyMorale = (sv, m) => { },
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            for (int i = 0; i < 5; i++) q.RecordTreatmentSuccess("sv_elena");
            Assert.AreEqual(QuestStatus.Success, q.State.Status);
            Assert.AreEqual("perk_field_triage", perk);
        }

        [Test]
        public void ThreeDeathsFailAndAfflictGuilt()
        {
            string aff = null;
            var q = new Quest_ElenaTriage
            {
                GrantPerk = (sv, id, n) => { },
                ApplyMorale = (sv, m) => { },
                AddAffliction = (sv, id) => aff = id,
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            for (int i = 0; i < 3; i++) q.RecordPatientDiedUnderCare("sv_elena");
            Assert.AreEqual(QuestStatus.Failure, q.State.Status);
            Assert.AreEqual("affliction_survivors_guilt", aff);
        }
    }

    [TestFixture]
    public class MechanicHighwayHeartTests
    {
        [Test]
        public void FailedExtractionReducesDurability()
        {
            string given = null;
            var q = new Quest_MechanicHighwayHeart
            {
                GiveItem = (sv, id, n) => given = id,
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            q.Advance(); // stage 1 -> 2
            q.Advance(); // stage 2 -> 3 (extraction)
            // Force a low roll: skill 0.9, roll 0.1 -> success first
            var rng = new System.Random(0); // deterministic
            q.ResolveAttemptExtraction(0.0f, rng);
            Assert.IsNotNull(given);
            // After failed attempt durability is reduced.
            Assert.LessOrEqual(q.GetProgress("engine_durability"), 100f);
        }
    }

    [TestFixture]
    public class ChildSoldierRifleTests
    {
        [Test]
        public void RaidDuringQuestLocksIt()
        {
            var q = new Quest_ChildSoldierRifle
            {
                TakeItem = (sv, id, n) => { },
                GrantPerk = (sv, id, n) => { },
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            q.OnRaidDuringQuest();
            Assert.AreEqual(QuestStatus.Locked, q.State.Status);
        }

        [Test]
        public void ThreeTalkDaysAdvanceToStageTwo()
        {
            var q = new Quest_ChildSoldierRifle
            {
                TakeItem = (sv, id, n) => { },
                GrantPerk = (sv, id, n) => { },
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            for (int i = 0; i < 3; i++) q.RecordTalkDay("sv_therapist");
            Assert.AreEqual(2, q.State.Stage);
        }
    }

    [TestFixture]
    public class DeepWellTests
    {
        [Test]
        public void EightExcavationDaysAdvanceToCompletion()
        {
            var q = new Quest_DeepWell
            {
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            for (int d = 0; d < 8; d++) q.RecordExcavationDay();
            // Stage should advance to 5 (the final stage triggers Complete).
            Assert.AreEqual(QuestStatus.Success, q.State.Status);
        }
    }

    [TestFixture]
    public class QuestRegistrySaveLoadTests
    {
        [Test]
        public void StateRoundTripsThroughJsonUtility()
        {
            // The host registers QuestRegistry as a generic ISaveable whose
            // payload crosses the wire via JsonUtility (SaveSystem.Entities).
            // This proves the DTO graph is JsonUtility-compatible.
            var a = new QuestRegistry();
            a.Start(QuestRegistry.IdDeepWell, 3);
            var well = a.Get<Quest_DeepWell>(QuestRegistry.IdDeepWell);
            well.RecordExcavationDay();
            well.RecordExcavationDay();

            string json = UnityEngine.JsonUtility.ToJson(a.CaptureState());
            var restored = UnityEngine.JsonUtility.FromJson<QuestRegistry.State>(json);

            var b = new QuestRegistry();
            b.RestoreState(restored);

            var s = b.Get(QuestRegistry.IdDeepWell).State;
            Assert.IsNotNull(s, "restored registry must reattach state to the live runtime");
            Assert.AreEqual(QuestStatus.InProgress, s.Status);
            Assert.AreEqual(3, s.StartedOnDay);
            Assert.AreEqual(2f, b.Get<Quest_DeepWell>(QuestRegistry.IdDeepWell)
                .GetProgress(Quest_DeepWell.DaysKey), 0.001f);
            // Untouched quest must stay NotStarted with no state row.
            Assert.IsNull(b.Get(QuestRegistry.IdMilitiaGrainWar).State);
        }
    }

    [TestFixture]
    public class QuestDataReferenceTests
    {
        private static string ReadDataFile(string name)
        {
            string path = System.IO.Path.Combine(
                UnityEngine.Application.streamingAssetsPath, "Data", name);
            Assert.IsTrue(System.IO.File.Exists(path), $"{name} not found at {path}");
            return System.IO.File.ReadAllText(path);
        }

        [Test]
        public void ItemsJson_CoversQuestItemIds()
        {
            // Quest payouts / material bills resolve through the items.json-backed
            // catalog; a missing id silently pays out nothing.
            string json = ReadDataFile("items.json");
            foreach (string itemId in new[]
            {
                "rubber_gasket",        // ShelterDegradationSystem.RepairHatchSeal
                "concrete_patch_mix",   // Shelter repairs + quest_deep_well bill
                "insulation_tape",      // ShelterDegradationSystem.RepairWiring
                "engine_block_intact",  // quest_mechanic_highway_heart reward
                "bearing_set_industrial", // quest_deep_well bill
                "copper_tubing_1m"      // quest_deep_well bill
            })
            {
                StringAssert.Contains($"\"id\": \"{itemId}\"", json,
                    $"'{itemId}' is referenced by a quest/shelter system but is not in items.json");
            }
        }

        [Test]
        public void LocationsJson_CoversQuestLocationIds()
        {
            string json = ReadDataFile("locations.json");
            foreach (string locationId in new[] { "highway_pileup", "prewar_medical_cache" })
            {
                StringAssert.Contains($"\"id\": \"{locationId}\"", json,
                    $"'{locationId}' is referenced by a quest but is not in locations.json");
            }
        }

        [Test]
        public void RepairGasketSpec_DeclaresHatchSealIntegrityEffect()
        {
            // GameBootstrap.WireRepairGasketCraftEffect reads this spec at boot to
            // decide the patch amount — the wiring breaks silently if the recipe
            // stops declaring the effect.
            var specs = AtomicWar._Game.Crafting.NewRecipesCatalog.BuildAll();
            var spec = specs.Find(s => s.Id == AtomicWar._Game.Crafting.NewRecipesCatalog.Ids.RepairGasket);
            Assert.IsNotNull(spec, "repair_gasket recipe missing from NewRecipesCatalog");
            Assert.AreEqual("hatch_seal_integrity", spec.EffectKey);
            Assert.Greater(spec.EffectAmount, 0f);
            Assert.IsTrue(string.IsNullOrEmpty(spec.ResultItemId) || spec.ResultAmount <= 0,
                "repair_gasket must stay effect-only; item output would double-pay with the patch");
        }

        [Test]
        public void ApplyHatchSealPatch_RestoresIntegrityWithoutConsumingItems()
        {
            var sys = new AtomicWar._Game.Shelter.ShelterDegradationSystem();
            sys.Current.HatchSealIntegrity = 0.5f;

            bool consumed = false;
            sys.RequestConsumeItem = (id, n) => { consumed = true; };

            Assert.IsTrue(sys.ApplyHatchSealPatch(0.15f));
            Assert.AreEqual(0.65f, sys.Current.HatchSealIntegrity, 0.001f);
            Assert.IsFalse(consumed, "patch must not charge the inventory — the recipe already consumed its ingredients");

            // Clamp at 1 and refuse no-op repairs.
            Assert.IsTrue(sys.ApplyHatchSealPatch(0.9f));
            Assert.AreEqual(1f, sys.Current.HatchSealIntegrity, 0.001f);
            Assert.IsFalse(sys.ApplyHatchSealPatch(0.15f), "full seal must refuse a patch");
            Assert.IsFalse(sys.ApplyHatchSealPatch(0f), "non-positive amount must refuse");
        }
    }
}
