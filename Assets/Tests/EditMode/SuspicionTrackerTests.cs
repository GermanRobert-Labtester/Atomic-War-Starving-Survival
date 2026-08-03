using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #45 — internal mysteries under resource pressure.
    /// Food/water &lt; 10% capacity → ResourceStarved → Missing Rations chain.
    /// </summary>
    [TestFixture]
    public class SuspicionTrackerTests
    {
        private EventRunner _runner;
        private SuspicionTracker _tracker;
        private Inventory _inventory;
        private ItemDefinition _food;
        private ItemDefinition _water;
        private ItemDefinition _parts;
        private List<GameEvent> _createdEvents;
        private MentalBreakSystem _mentalBreak;

        [SetUp]
        public void SetUp()
        {
            _runner = new EventRunner();
            _tracker = new SuspicionTracker();
            _tracker.Bind(_runner);
            _inventory = new Inventory { Capacity = 20, MaxWeight = 200f };
            _createdEvents = new List<GameEvent>();
            _mentalBreak = new MentalBreakSystem();

            _food = ScriptableObject.CreateInstance<ItemDefinition>();
            _food.id = "canned_food";
            _food.displayName = "Canned Food";
            _food.type = ItemType.Food;
            _food.stackMax = 99;
            _food.weight = 0.5f;

            _water = ScriptableObject.CreateInstance<ItemDefinition>();
            _water.id = "clean_water";
            _water.displayName = "Clean Water";
            _water.type = ItemType.Water;
            _water.stackMax = 99;
            _water.weight = 0.5f;

            _parts = ScriptableObject.CreateInstance<ItemDefinition>();
            _parts.id = SuspicionTracker.MechanicalPartsId;
            _parts.displayName = "Mechanical Parts";
            _parts.type = ItemType.Material;
            _parts.stackMax = 99;
            _parts.weight = 1f;
        }

        [TearDown]
        public void TearDown()
        {
            _tracker?.Unbind(_runner);
            if (_food != null) Object.DestroyImmediate(_food);
            if (_water != null) Object.DestroyImmediate(_water);
            if (_parts != null) Object.DestroyImmediate(_parts);
            if (_createdEvents != null)
            {
                for (int i = 0; i < _createdEvents.Count; i++)
                {
                    if (_createdEvents[i] != null)
                        Object.DestroyImmediate(_createdEvents[i]);
                }
            }
            if (_runner?.Pool != null)
            {
                for (int i = 0; i < _runner.Pool.Count; i++)
                {
                    if (_runner.Pool[i] != null)
                        Object.DestroyImmediate(_runner.Pool[i]);
                }
            }
        }

        private static Survivor MakeSurvivor(
            string id,
            RiskBiasTrait bias = RiskBiasTrait.Realist,
            float morale = 75f,
            string trait = null)
        {
            var s = new Survivor
            {
                Id = id,
                DisplayName = id,
                RiskBias = bias
            };
            s.Needs.Morale = morale;
            s.Needs.Hunger = 40f;
            if (!string.IsNullOrEmpty(trait))
            {
                s.Traits = new List<string> { trait };
            }
            return s;
        }

        private EventContext MakeContext(IList<Survivor> crew, string playerId = "leader")
        {
            var primary = crew != null && crew.Count > 0
                ? crew.FirstOrDefault(s => s.Id == playerId) ?? crew[0]
                : null;
            var ctx = new EventContext(primary, inventory: _inventory)
            {
                CurrentDay = 5,
                CurrentHour = 12f,
                AllSurvivors = crew != null ? new List<Survivor>(crew) : null,
                PlayerSurvivorId = playerId,
                MentalBreak = _mentalBreak,
                Suspicion = _tracker,
                // Deterministic: always pick first weighted candidate path via low roll.
                Random = new System.Random(1)
            };
            _tracker.RefreshStarved(_inventory);
            ctx.IsResourceStarved = _tracker.IsResourceStarved;
            return ctx;
        }

        private void SeedFoodAtFivePercent()
        {
            // Capacity 20 → 1 food unit = 5% fill (under 10% starved threshold).
            Assert.That(_inventory.Add(_food, 1), Is.True);
            Assert.That(_inventory.FoodFillRatio(), Is.EqualTo(0.05f).Within(0.001f));
        }

        // ── ResourceStarved evaluation ───────────────────────────────────

        [Test]
        public void FoodFillRatio_FivePercent_IsResourceStarved()
        {
            SeedFoodAtFivePercent();
            Assert.That(SuspicionTracker.EvaluateResourceStarved(_inventory), Is.True);
            Assert.That(_inventory.FoodFillRatio(), Is.LessThan(SuspicionTracker.ResourceStarvedThreshold));
        }

        [Test]
        public void FoodFillRatio_FullStock_NotResourceStarved()
        {
            // Both axes must clear the 10% floor — empty water alone is starved.
            _inventory.Add(_food, 10);  // 50% food
            _inventory.Add(_water, 10); // 50% water
            Assert.That(SuspicionTracker.EvaluateResourceStarved(_inventory), Is.False);
        }

        [Test]
        public void EventCondition_RequireResourceStarved_GatesCanTrigger()
        {
            SeedFoodAtFivePercent();
            var crew = new List<Survivor>
            {
                MakeSurvivor("leader"),
                MakeSurvivor("scav")
            };
            var ctx = MakeContext(crew);
            var ev = SuspicionTracker.CreateMissingRationsEvent(crew[1]);
            _createdEvents.Add(ev);

            Assert.That(ev.CanTrigger(ctx), Is.True, "Starved context should pass RequireResourceStarved.");

            ctx.IsResourceStarved = false;
            Assert.That(ev.CanTrigger(ctx), Is.False, "Non-starved context must fail RequireResourceStarved.");
        }

        // ── Suspect weighting ────────────────────────────────────────────

        [Test]
        public void PickSuspect_ExcludesPlayerPov()
        {
            var crew = new List<Survivor>
            {
                MakeSurvivor("leader", RiskBiasTrait.Fatalist, morale: 10f),
                MakeSurvivor("other", RiskBiasTrait.Realist, morale: 80f)
            };
            var pick = SuspicionTracker.PickSuspect(crew, "leader", new System.Random(42));
            Assert.That(pick, Is.Not.Null);
            Assert.That(pick.Id, Is.EqualTo("other"), "POV/player must be excluded from suspect pool.");
        }

        [Test]
        public void WeightFor_BingeEaterAndFatalist_HigherThanBaseline()
        {
            var baseline = MakeSurvivor("a", RiskBiasTrait.Realist, morale: 75f);
            var fatalist = MakeSurvivor("b", RiskBiasTrait.Fatalist, morale: 75f);
            var binge = MakeSurvivor("c", RiskBiasTrait.Realist, morale: 75f, trait: "binge_eater");

            float wBase = SuspicionTracker.WeightFor(baseline);
            float wFatal = SuspicionTracker.WeightFor(fatalist);
            float wBinge = SuspicionTracker.WeightFor(binge);

            Assert.That(wFatal, Is.GreaterThan(wBase));
            Assert.That(wBinge, Is.GreaterThan(wBase));
            Assert.That(wBinge, Is.GreaterThan(wFatal), "Binge eater weight should dominate.");
        }

        // ── Acceptance: 5% food → 24h → Missing Rations → Ignore → 48h vanish ─

        [Test]
        public void FoodAtFivePercent_After24Hours_FiresMissingRations()
        {
            SeedFoodAtFivePercent();
            var crew = new List<Survivor>
            {
                MakeSurvivor("leader"),
                MakeSurvivor("mira", RiskBiasTrait.Fatalist, morale: 20f),
                MakeSurvivor("jon", RiskBiasTrait.Realist, morale: 60f)
            };
            var ctx = MakeContext(crew);

            GameEvent fired = null;
            _runner.OnEventTriggered += (ev, c) => fired = ev;
            _tracker.OnMysteryEventReady += (ev, c) =>
            {
                if (ev != null && !_createdEvents.Contains(ev))
                    _createdEvents.Add(ev);
            };

            // Under 24h — no fire
            _tracker.Tick(23f, ctx, _runner);
            Assert.That(fired, Is.Null);
            Assert.That(_tracker.MysteryOpen, Is.False);

            // Cross 24h threshold
            _tracker.Tick(1f, ctx, _runner);
            Assert.That(fired, Is.Not.Null, "Missing Rations must fire after 24 starved hours.");
            Assert.That(fired.id, Is.EqualTo(SuspicionTracker.MissingRationsEventId));
            Assert.That(fired.title, Is.EqualTo("Missing Rations"));
            Assert.That(fired.bodyText, Does.Contain("can of beans").IgnoreCase);
            Assert.That(_tracker.MysteryOpen, Is.True);
            Assert.That(_tracker.TrueThiefId, Is.Not.Empty);
            Assert.That(_tracker.TrueThiefId, Is.Not.EqualTo("leader"));
            // First beat does not delete stock (avoid emptying the sole 5% unit before choices).
            Assert.That(_inventory.CountByType(ItemType.Food), Is.EqualTo(1));
        }

        [Test]
        public void Ignore_After48Hours_DeletesOneFood_AndFiresFollowUp()
        {
            SeedFoodAtFivePercent();
            // Extra unit so vanish leaves something measurable and doesn't starve inventory to empty-only.
            _inventory.Add(_food, 1); // now 2 food = 10% exactly — still need under 10% for initial fire
            // Spec: set food to 5%. Use 1 unit for starve fire, then top up after fire for vanish assertion.
            // Reset to exact 5% for starve trigger:
            while (_inventory.CountByType(ItemType.Food) > 1)
                _inventory.RemoveByType(ItemType.Food, 1);

            var crew = new List<Survivor>
            {
                MakeSurvivor("leader"),
                MakeSurvivor("mira", RiskBiasTrait.Fatalist, morale: 15f),
                MakeSurvivor("jon")
            };
            var ctx = MakeContext(crew);

            var fired = new List<GameEvent>();
            _runner.OnEventTriggered += (ev, c) =>
            {
                if (ev != null) fired.Add(ev);
            };
            _tracker.OnMysteryEventReady += (ev, c) =>
            {
                if (ev != null && !_createdEvents.Contains(ev))
                    _createdEvents.Add(ev);
            };

            // Fire Missing Rations at 24h
            _tracker.Tick(SuspicionTracker.HoursUntilMystery, ctx, _runner);
            Assert.That(fired.Count, Is.GreaterThanOrEqualTo(1));
            Assert.That(fired[0].id, Is.EqualTo(SuspicionTracker.MissingRationsEventId));

            // Add a second food so the 48h vanish has something to delete while keeping pressure
            // (inventory already has 1; add 1 more → 2 units before ignore vanish).
            Assert.That(_inventory.Add(_food, 1), Is.True);
            int foodBeforeIgnore = _inventory.CountByType(ItemType.Food);
            Assert.That(foodBeforeIgnore, Is.EqualTo(2));

            // Pick Ignore
            var mystery = fired[0];
            var ignore = mystery.choices.First(c => c.ChoiceId == SuspicionTracker.ChoiceIgnore);
            _runner.ApplyChoice(mystery, ignore, ctx);

            Assert.That(_tracker.IgnoreActive, Is.True);
            Assert.That(_tracker.MysteryOpen, Is.False);

            // Wait 47h — no vanish yet
            _tracker.Tick(47f, ctx, _runner);
            Assert.That(_inventory.CountByType(ItemType.Food), Is.EqualTo(foodBeforeIgnore));
            Assert.That(_tracker.VanishCount, Is.EqualTo(0));

            // Cross 48h — one food deleted + follow-up event
            _tracker.Tick(1f, ctx, _runner);
            Assert.That(_inventory.CountByType(ItemType.Food), Is.EqualTo(foodBeforeIgnore - 1),
                "Ignore path must delete 1 food item every 48 hours.");
            Assert.That(_tracker.VanishCount, Is.EqualTo(1));

            var followUp = fired.LastOrDefault(e => e.id == SuspicionTracker.MissingRationsAgainEventId);
            Assert.That(followUp, Is.Not.Null, "Follow-up Missing Rations event must fire after vanish.");
            Assert.That(followUp.title, Does.Contain("Again").IgnoreCase);
        }

        // ── Choices: interrogate / lock / banish / forgive ───────────────

        [Test]
        public void Interrogate_Innocent_MassiveMoralePenalty()
        {
            SeedFoodAtFivePercent();
            var accused = MakeSurvivor("innocent", RiskBiasTrait.Realist, morale: 70f);
            var thief = MakeSurvivor("thief", RiskBiasTrait.Fatalist, morale: 20f);
            var leader = MakeSurvivor("leader");
            var crew = new List<Survivor> { leader, accused, thief };
            var ctx = MakeContext(crew);

            _tracker.OnMysteryEventReady += (ev, c) =>
            {
                if (ev != null && !_createdEvents.Contains(ev))
                    _createdEvents.Add(ev);
            };

            _tracker.Tick(SuspicionTracker.HoursUntilMystery, ctx, _runner);
            // Force wrong accusation
            _tracker.TrueThiefId = "thief";
            _tracker.AccusedId = "innocent";

            var mystery = _runner.FindInPool(SuspicionTracker.MissingRationsEventId);
            Assert.That(mystery, Is.Not.Null);
            var choice = mystery.choices.First(c => c.ChoiceId == SuspicionTracker.ChoiceInterrogate);
            float moraleBefore = accused.Needs.Morale;

            _runner.ApplyChoice(mystery, choice, ctx);

            Assert.That(accused.Needs.Morale, Is.EqualTo(moraleBefore + SuspicionTracker.InnocentMoralePenalty).Within(0.01f));
            Assert.That(_tracker.ThiefCaught, Is.False);
            Assert.That(_mentalBreak.Affinity.Get("leader", "innocent"),
                Is.EqualTo(SuspicionTracker.AffinityInterrogateHit).Within(0.01f));
        }

        [Test]
        public void Interrogate_Guilty_PresentsBanishOrForgive()
        {
            SeedFoodAtFivePercent();
            var thief = MakeSurvivor("thief", RiskBiasTrait.Fatalist, morale: 10f);
            var leader = MakeSurvivor("leader");
            var crew = new List<Survivor> { leader, thief, MakeSurvivor("jon") };
            var ctx = MakeContext(crew);

            GameEvent caught = null;
            _tracker.OnMysteryEventReady += (ev, c) =>
            {
                if (ev != null && !_createdEvents.Contains(ev))
                    _createdEvents.Add(ev);
                if (ev != null && ev.id == SuspicionTracker.MissingRationsCaughtEventId)
                    caught = ev;
            };

            _tracker.Tick(SuspicionTracker.HoursUntilMystery, ctx, _runner);
            _tracker.TrueThiefId = "thief";
            _tracker.AccusedId = "thief";

            var mystery = _runner.FindInPool(SuspicionTracker.MissingRationsEventId);
            var choice = mystery.choices.First(c => c.ChoiceId == SuspicionTracker.ChoiceInterrogate);
            _runner.ApplyChoice(mystery, choice, ctx);

            Assert.That(_tracker.ThiefCaught, Is.True);
            Assert.That(caught, Is.Not.Null);
            Assert.That(caught.choices.Any(c => c.ChoiceId == SuspicionTracker.ChoiceBanish), Is.True);
            Assert.That(caught.choices.Any(c => c.ChoiceId == SuspicionTracker.ChoiceForgive), Is.True);
        }

        [Test]
        public void Banish_KillsThief_AndTraumatizesGroup()
        {
            SeedFoodAtFivePercent();
            var thief = MakeSurvivor("thief", RiskBiasTrait.Fatalist, morale: 50f);
            var witness = MakeSurvivor("jon", RiskBiasTrait.Realist, morale: 60f);
            var leader = MakeSurvivor("leader", morale: 70f);
            var crew = new List<Survivor> { leader, thief, witness };
            var ctx = MakeContext(crew);

            _tracker.OnMysteryEventReady += (ev, c) =>
            {
                if (ev != null && !_createdEvents.Contains(ev))
                    _createdEvents.Add(ev);
            };

            _tracker.Tick(SuspicionTracker.HoursUntilMystery, ctx, _runner);
            _tracker.TrueThiefId = "thief";
            _tracker.AccusedId = "thief";

            var mystery = _runner.FindInPool(SuspicionTracker.MissingRationsEventId);
            _runner.ApplyChoice(mystery,
                mystery.choices.First(c => c.ChoiceId == SuspicionTracker.ChoiceInterrogate), ctx);

            var caught = _tracker.PendingCaughtEvent
                         ?? _runner.FindInPool(SuspicionTracker.MissingRationsCaughtEventId);
            Assert.That(caught, Is.Not.Null);

            float witnessMoraleBefore = witness.Needs.Morale;
            _runner.ApplyChoice(caught,
                caught.choices.First(c => c.ChoiceId == SuspicionTracker.ChoiceBanish), ctx);

            Assert.That(thief.IsAlive, Is.False);
            Assert.That(thief.State, Is.EqualTo(SurvivorState.Dead));
            Assert.That(ctx.HasEventFlag(SuspicionTracker.FlagThiefBanished), Is.True);
            Assert.That(witness.Needs.Morale,
                Is.EqualTo(witnessMoraleBefore + SuspicionTracker.GroupTraumaMorale).Within(0.01f));
        }

        [Test]
        public void Forgive_SetsPermanentFracturedStatus()
        {
            SeedFoodAtFivePercent();
            var thief = MakeSurvivor("thief", RiskBiasTrait.Fatalist, morale: 40f);
            var leader = MakeSurvivor("leader");
            var crew = new List<Survivor> { leader, thief, MakeSurvivor("jon") };
            var ctx = MakeContext(crew);

            _tracker.OnMysteryEventReady += (ev, c) =>
            {
                if (ev != null && !_createdEvents.Contains(ev))
                    _createdEvents.Add(ev);
            };

            _tracker.Tick(SuspicionTracker.HoursUntilMystery, ctx, _runner);
            _tracker.TrueThiefId = "thief";
            _tracker.AccusedId = "thief";

            var mystery = _runner.FindInPool(SuspicionTracker.MissingRationsEventId);
            _runner.ApplyChoice(mystery,
                mystery.choices.First(c => c.ChoiceId == SuspicionTracker.ChoiceInterrogate), ctx);

            var caught = _tracker.PendingCaughtEvent
                         ?? _runner.FindInPool(SuspicionTracker.MissingRationsCaughtEventId);
            _runner.ApplyChoice(caught,
                caught.choices.First(c => c.ChoiceId == SuspicionTracker.ChoiceForgive), ctx);

            Assert.That(thief.IsAlive, Is.True);
            Assert.That(thief.IsFractured, Is.True);
            Assert.That(thief.HasStatus(SurvivorStatus.Fractured), Is.True);
            Assert.That(thief.HasDisability(SuspicionTracker.DisabilityFractured), Is.True);
            Assert.That(_tracker.BunkerFractured, Is.True);
            Assert.That(ctx.HasEventFlag(SuspicionTracker.FlagBunkerFractured), Is.True);
        }

        [Test]
        public void LockPantry_ConsumesMechanicalParts_StopsVanish()
        {
            SeedFoodAtFivePercent();
            _inventory.Add(_parts, 1);
            var crew = new List<Survivor>
            {
                MakeSurvivor("leader"),
                MakeSurvivor("mira", RiskBiasTrait.Fatalist, morale: 10f)
            };
            var ctx = MakeContext(crew);

            _tracker.OnMysteryEventReady += (ev, c) =>
            {
                if (ev != null && !_createdEvents.Contains(ev))
                    _createdEvents.Add(ev);
            };

            _tracker.Tick(SuspicionTracker.HoursUntilMystery, ctx, _runner);
            var mystery = _runner.FindInPool(SuspicionTracker.MissingRationsEventId);
            _runner.ApplyChoice(mystery,
                mystery.choices.First(c => c.ChoiceId == SuspicionTracker.ChoiceLockPantry), ctx);

            Assert.That(_tracker.PantryLocked, Is.True);
            Assert.That(_inventory.CountById(SuspicionTracker.MechanicalPartsId), Is.EqualTo(0));
            Assert.That(ctx.HasEventFlag(SuspicionTracker.FlagPantryLocked), Is.True);

            // Even if ignore were active, locked pantry blocks further vanish ticks.
            _tracker.IgnoreActive = true;
            int foodBefore = _inventory.CountByType(ItemType.Food);
            _tracker.Tick(SuspicionTracker.IgnoreVanishHours, ctx, _runner);
            Assert.That(_inventory.CountByType(ItemType.Food), Is.EqualTo(foodBefore));
            Assert.That(_tracker.VanishCount, Is.EqualTo(0));
        }

        [Test]
        public void CaptureRestore_PreservesMysteryState()
        {
            _tracker.StarvedHours = 12f;
            _tracker.MysteryOpen = true;
            _tracker.IgnoreActive = true;
            _tracker.IgnoreHoursAccum = 30f;
            _tracker.TrueThiefId = "mira";
            _tracker.AccusedId = "jon";
            _tracker.ThiefCaught = false;
            _tracker.BunkerFractured = true;
            _tracker.VanishCount = 2;
            _tracker.PantryLocked = false;

            var snap = _tracker.CaptureState();
            var other = new SuspicionTracker();
            other.RestoreState(snap);

            Assert.That(other.StarvedHours, Is.EqualTo(12f));
            Assert.That(other.MysteryOpen, Is.True);
            Assert.That(other.IgnoreActive, Is.True);
            Assert.That(other.IgnoreHoursAccum, Is.EqualTo(30f));
            Assert.That(other.TrueThiefId, Is.EqualTo("mira"));
            Assert.That(other.AccusedId, Is.EqualTo("jon"));
            Assert.That(other.BunkerFractured, Is.True);
            Assert.That(other.VanishCount, Is.EqualTo(2));
        }
    }
}
