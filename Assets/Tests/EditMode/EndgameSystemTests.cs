using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Campaign win/loss: vehicle escape, radio extraction, death screens, summary.
    /// </summary>
    [TestFixture]
    public class EndgameSystemTests
    {
        private GameObject _hudObject;
        private HUD _hud;

        [SetUp]
        public void SetUp()
        {
            _hudObject = new GameObject("EndgameTestHUD");
            _hud = _hudObject.AddComponent<HUD>();
        }

        [TearDown]
        public void TearDown()
        {
            if (_hudObject != null)
                Object.DestroyImmediate(_hudObject);
        }

        private static ItemDefinition MakeItem(string id, ItemType type = ItemType.Material, float durability = 0f, int stackMax = 99)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.stackMax = stackMax;
            item.weight = 0.1f;
            item.durability = durability;
            return item;
        }

        private static Dictionary<string, ItemDefinition> Catalog()
        {
            return new Dictionary<string, ItemDefinition>
            {
                [VictoryProjectManager.MechanicalPartsId] = MakeItem(VictoryProjectManager.MechanicalPartsId, ItemType.Material, 0f, 99),
                [VictoryProjectManager.FuelItemId] = MakeItem(VictoryProjectManager.FuelItemId, ItemType.Fuel, 0f, 99),
                [VictoryProjectManager.EngineItemId] = MakeItem(VictoryProjectManager.EngineItemId, ItemType.Tool, 100f, 1)
            };
        }

        private static ItemDefinition Lookup(Dictionary<string, ItemDefinition> cat, string id)
            => cat.TryGetValue(id, out var d) ? d : null;

        [Test]
        public void VehicleEscape_WithRequiredParts_SetsEscaped_AndLoadsSummary()
        {
            var cat = Catalog();
            var inv = new Inventory { Capacity = 200 };
            Assert.That(inv.Add(cat[VictoryProjectManager.MechanicalPartsId], VictoryProjectManager.VehiclePartsRequired), Is.True);
            Assert.That(inv.Add(cat[VictoryProjectManager.FuelItemId], VictoryProjectManager.VehicleFuelRequired), Is.True);
            Assert.That(inv.Add(cat[VictoryProjectManager.EngineItemId], 1), Is.True);
            // Fresh engine is full durability → repaired.
            Assert.That(VictoryProjectManager.HasRepairedEngine(inv), Is.True);

            var survivors = new List<Survivor>
            {
                new Survivor { Id = "s1", DisplayName = "Mara", LifetimeRadiationExposure = 12f }
            };

            var victory = new VictoryProjectManager();
            EndgameSummaryData shown = null;
            victory.OnEndgameTriggered += s => shown = s;

            var summaryUi = _hud.EnsureEndgameSummary();
            Assert.That(summaryUi.IsLoaded, Is.False);

            var result = victory.TryEscapeByVehicle(
                inv,
                id => Lookup(cat, id),
                day: 44,
                survivors);

            Assert.That(result, Is.Not.Null);
            Assert.That(victory.State, Is.EqualTo(EndgameState.Escaped));
            Assert.That(victory.IsTerminal, Is.True);
            Assert.That(shown, Is.Not.Null);
            Assert.That(shown.State, Is.EqualTo(EndgameState.Escaped));
            Assert.That(shown.DaysSurvived, Is.EqualTo(44));
            Assert.That(shown.TotalRadiationAbsorbed, Is.EqualTo(12f).Within(0.01f));
            Assert.That(shown.VehicleEscapeUsed, Is.True);
            Assert.That(shown.OutcomeTitle, Is.EqualTo("ESCAPED"));

            // Inventory consumed.
            Assert.That(inv.CountById(VictoryProjectManager.MechanicalPartsId), Is.EqualTo(0));
            Assert.That(inv.CountById(VictoryProjectManager.FuelItemId), Is.EqualTo(0));
            Assert.That(inv.CountById(VictoryProjectManager.EngineItemId), Is.EqualTo(0));

            // Summary screen loads (acceptance).
            summaryUi.Show(
                shown.State.ToString(),
                shown.OutcomeTitle,
                shown.OutcomeBody,
                string.Empty,
                shown.DaysSurvived,
                shown.TotalRadiationAbsorbed,
                shown.MoralChoicesMade,
                shown.MilitaryIntelDecrypted,
                shown.ExtractionUnlocked,
                shown.VehicleEscapeUsed);

            Assert.That(summaryUi.IsLoaded, Is.True);
            Assert.That(summaryUi.IsVisible, Is.True);
            Assert.That(summaryUi.StateName, Is.EqualTo("Escaped"));
            Assert.That(summaryUi.DaysSurvived, Is.EqualTo(44));
            Assert.That(summaryUi.DetailSummary, Does.Contain("Days survived"));
            Assert.That(summaryUi.DetailSummary, Does.Contain("ESCAPED"));
        }

        [Test]
        public void VehicleEscape_MissingParts_DoesNotEnd()
        {
            var cat = Catalog();
            var inv = new Inventory { Capacity = 200 };
            inv.Add(cat[VictoryProjectManager.MechanicalPartsId], 10);
            inv.Add(cat[VictoryProjectManager.FuelItemId], 2);
            inv.Add(cat[VictoryProjectManager.EngineItemId], 1);

            var victory = new VictoryProjectManager();
            var result = victory.TryEscapeByVehicle(
                inv, id => Lookup(cat, id), 10,
                new List<Survivor> { new Survivor { Id = "s1" } });

            Assert.That(result, Is.Null);
            Assert.That(victory.State, Is.EqualTo(EndgameState.Ongoing));
            Assert.That(inv.CountById(VictoryProjectManager.MechanicalPartsId), Is.EqualTo(10));
        }

        [Test]
        public void MilitaryIntel_TenNodes_UnlocksExtraction()
        {
            var victory = new VictoryProjectManager();
            bool unlocked = false;
            victory.OnExtractionUnlocked += () => unlocked = true;

            victory.GrantMilitaryIntel(9, day: 12);
            Assert.That(victory.ExtractionUnlocked, Is.False);
            Assert.That(unlocked, Is.False);

            victory.GrantMilitaryIntel(1, day: 13);
            Assert.That(victory.MilitaryIntelDecrypted, Is.EqualTo(10));
            Assert.That(victory.ExtractionUnlocked, Is.True);
            Assert.That(unlocked, Is.True);
        }

        [Test]
        public void Extraction_Day100_WithUnlock_SetsRescued()
        {
            var victory = new VictoryProjectManager();
            victory.GrantMilitaryIntel(VictoryProjectManager.IntelRequiredForExtraction, day: 20);
            var survivors = new List<Survivor>
            {
                new Survivor { Id = "s1", DisplayName = "Ren", LifetimeRadiationExposure = 5f }
            };

            Assert.That(victory.TickDay(99, survivors), Is.Null);
            Assert.That(victory.State, Is.EqualTo(EndgameState.Ongoing));

            var result = victory.TickDay(VictoryProjectManager.ChopperArrivalDay, survivors);
            Assert.That(result, Is.Not.Null);
            Assert.That(victory.State, Is.EqualTo(EndgameState.Rescued));
            Assert.That(result.OutcomeTitle, Is.EqualTo("RESCUED"));
            Assert.That(result.DaysSurvived, Is.EqualTo(100));
        }

        [Test]
        public void Extraction_Day100_WithoutUnlock_StaysOngoing()
        {
            var victory = new VictoryProjectManager();
            var survivors = new List<Survivor> { new Survivor { Id = "s1" } };
            Assert.That(victory.TickDay(100, survivors), Is.Null);
            Assert.That(victory.State, Is.EqualTo(EndgameState.Ongoing));
        }

        [Test]
        public void AllDead_HighRad_SetsIrradiated()
        {
            var victory = new VictoryProjectManager();
            var dead = new List<Survivor>
            {
                new Survivor
                {
                    Id = "s1",
                    State = SurvivorState.Dead,
                    LifetimeRadiationExposure = 200f,
                    HasAcuteRadiationSyndrome = true
                }
            };

            var result = victory.EvaluateLoss(dead, day: 40);
            Assert.That(result, Is.Not.Null);
            Assert.That(victory.State, Is.EqualTo(EndgameState.Irradiated));
            Assert.That(victory.DeathScreen, Is.EqualTo(DeathScreenKind.Radiation));
            Assert.That(result.OutcomeTitle, Is.EqualTo("IRRADIATED"));
        }

        [Test]
        public void AllDead_Hunger_SetsStarved()
        {
            var victory = new VictoryProjectManager();
            var sv = new Survivor { Id = "s1", State = SurvivorState.Dead };
            sv.Needs.Hunger = 100f;
            sv.Needs.WasHungerCritical = true;

            var result = victory.EvaluateLoss(new List<Survivor> { sv }, day: 22);
            Assert.That(result, Is.Not.Null);
            Assert.That(victory.State, Is.EqualTo(EndgameState.Starved));
            Assert.That(victory.DeathScreen, Is.EqualTo(DeathScreenKind.Hunger));
        }

        [Test]
        public void AllDead_Breakdown_SetsStarvedWithBreakdownScreen()
        {
            var victory = new VictoryProjectManager();
            var sv = new Survivor
            {
                Id = "s1",
                State = SurvivorState.Dead,
                currentMentalBreakId = "violent_paranoia"
            };
            sv.Needs.Morale = 0f;

            var result = victory.EvaluateLoss(new List<Survivor> { sv }, day: 18);
            Assert.That(result, Is.Not.Null);
            Assert.That(victory.State, Is.EqualTo(EndgameState.Starved));
            Assert.That(victory.DeathScreen, Is.EqualTo(DeathScreenKind.Breakdowns));
            Assert.That(result.OutcomeTitle, Is.EqualTo("BROKEN"));
        }

        [Test]
        public void Summary_FromSaveData_TalliesDaysAndRadiation()
        {
            var save = new SaveData
            {
                GameState = new GameStateSave { Day = 55, Phase = GamePhase.GameOver },
                Survivors = new List<SurvivorSave>
                {
                    new SurvivorSave
                    {
                        Id = "s1",
                        DisplayName = "Kai",
                        State = SurvivorState.Dead,
                        LifetimeRadiationExposure = 33f
                    },
                    new SurvivorSave
                    {
                        Id = "s2",
                        DisplayName = "Len",
                        State = SurvivorState.Dead,
                        LifetimeRadiationExposure = 17f
                    }
                },
                VictoryProject = new VictoryProjectSave
                {
                    State = EndgameState.Escaped,
                    MoralChoicesMade = 2,
                    MilitaryIntelDecrypted = 4,
                    EngineConsumed = true,
                    TerminalReason = "The engine caught."
                }
            };

            var summary = VictoryProjectManager.FromSaveData(save);
            Assert.That(summary.DaysSurvived, Is.EqualTo(55));
            Assert.That(summary.TotalRadiationAbsorbed, Is.EqualTo(50f).Within(0.01f));
            Assert.That(summary.MoralChoicesMade, Is.EqualTo(2));
            Assert.That(summary.State, Is.EqualTo(EndgameState.Escaped));
            Assert.That(summary.OutcomeTitle, Is.EqualTo("ESCAPED"));
            Assert.That(summary.IsTerminal, Is.True);
        }

        [Test]
        public void IsMilitaryIntel_UsesSourceFrequencyOrType()
        {
            Assert.That(VictoryProjectManager.IsMilitaryIntel(new IntelNode
            {
                SourceFrequencyId = RadioFrequencySO.Ids.Military,
                Type = IntelType.Unknown
            }), Is.True);

            Assert.That(VictoryProjectManager.IsMilitaryIntel(new IntelNode
            {
                SourceFrequencyId = "other",
                Type = IntelType.MortarWarning
            }), Is.True);

            Assert.That(VictoryProjectManager.IsMilitaryIntel(new IntelNode
            {
                SourceFrequencyId = "civilian",
                Type = IntelType.WeatherForecast
            }), Is.False);
        }
    }
}
