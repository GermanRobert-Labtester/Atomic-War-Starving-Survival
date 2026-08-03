using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// TraitGates, FactionTrustGates, and stateful eventFlags on event choices.
    /// Acceptance: crew makeup + trust + past flags alter narrative options (The Emissary).
    /// </summary>
    [TestFixture]
    public class TraitGateEventTests
    {
        private GameEvent _emissary;
        private GameObject _uiObject;
        private EventModalUI _modal;

        [SetUp]
        public void SetUp()
        {
            _emissary = EventRunner.CreateEmissaryEvent();
            _uiObject = new GameObject("TraitGateEventModal");
            _modal = _uiObject.AddComponent<EventModalUI>();
        }

        [TearDown]
        public void TearDown()
        {
            if (_emissary != null)
                Object.DestroyImmediate(_emissary);
            if (_uiObject != null)
                Object.DestroyImmediate(_uiObject);
        }

        private static Survivor MakeSurvivor(string id, RiskBiasTrait trait)
        {
            return new Survivor
            {
                Id = id,
                DisplayName = id,
                RiskBias = trait
                // State defaults to Idle → IsAlive
            };
        }

        private static EventContext MakeContext(
            IList<Survivor> crew,
            float scavengerTrust = 0f,
            Dictionary<string, bool> flags = null)
        {
            var primary = crew != null && crew.Count > 0 ? crew[0] : null;
            var ctx = new EventContext(primary)
            {
                CurrentDay = 10,
                CurrentHour = 12f,
                AllSurvivors = crew != null ? new List<Survivor>(crew) : null,
                GetFactionTrust = fid =>
                    fid == EventRunner.EmissaryFactionId ? scavengerTrust : 0f
            };
            if (flags != null)
                ctx.ImportFlags(flags);
            return ctx;
        }

        private static bool AvailableHas(IList<EventChoice> available, string choiceId)
        {
            if (available == null) return false;
            for (int i = 0; i < available.Count; i++)
            {
                if (available[i] != null && available[i].ChoiceId == choiceId)
                    return true;
            }
            return false;
        }

        private static PresentedEventChoice FindPresented(IList<PresentedEventChoice> list, string choiceId)
        {
            if (list == null) return null;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null && list[i].ChoiceId == choiceId)
                    return list[i];
            }
            return null;
        }

        [Test]
        public void Emissary_AllCautiousCrew_LieChoiceIsHidden()
        {
            var crew = new List<Survivor>
            {
                MakeSurvivor("c1", RiskBiasTrait.Cautious),
                MakeSurvivor("c2", RiskBiasTrait.Cautious),
                MakeSurvivor("c3", RiskBiasTrait.Cautious)
            };
            var ctx = MakeContext(crew, scavengerTrust: 0f);

            var available = EventRunner.GetAvailableChoices(_emissary, ctx);
            var presented = EventRunner.GetPresentedChoices(_emissary, ctx);
            var visible = EventRunner.GetVisibleChoices(_emissary, ctx);

            Assert.That(AvailableHas(available, EventRunner.EmissaryLieChoiceId), Is.False,
                "Lie choice must be hidden with an all-Cautious crew.");
            Assert.That(AvailableHas(available, EventRunner.EmissaryFireChoiceId), Is.False);

            var liePresented = FindPresented(presented, EventRunner.EmissaryLieChoiceId);
            Assert.That(liePresented, Is.Not.Null);
            Assert.That(liePresented.IsHidden, Is.True);
            Assert.That(liePresented.IsAvailable, Is.False);

            Assert.That(FindPresented(visible, EventRunner.EmissaryLieChoiceId), Is.Null,
                "Hidden choices must not appear in visible UI rows.");

            // Baseline choices still present.
            Assert.That(AvailableHas(available, EventRunner.EmissaryShareChoiceId), Is.True);
            Assert.That(AvailableHas(available, EventRunner.EmissaryRefuseChoiceId), Is.True);
        }

        [Test]
        public void Emissary_AddParanoidSurvivor_LieChoiceBecomesAvailable()
        {
            var crew = new List<Survivor>
            {
                MakeSurvivor("c1", RiskBiasTrait.Cautious),
                MakeSurvivor("c2", RiskBiasTrait.Cautious)
            };
            var ctx = MakeContext(crew, scavengerTrust: 0f);

            Assert.That(
                AvailableHas(EventRunner.GetAvailableChoices(_emissary, ctx), EventRunner.EmissaryLieChoiceId),
                Is.False);

            // Add a Paranoid survivor mid-scenario.
            crew.Add(MakeSurvivor("p1", RiskBiasTrait.Paranoid));
            ctx.AllSurvivors = new List<Survivor>(crew);

            var available = EventRunner.GetAvailableChoices(_emissary, ctx);
            Assert.That(AvailableHas(available, EventRunner.EmissaryLieChoiceId), Is.True,
                "Lie choice unlocks when any bunker survivor is Paranoid.");
            Assert.That(AvailableHas(available, EventRunner.EmissaryFireChoiceId), Is.False,
                "At trust >= -20 the fire choice stays gated off.");

            var lie = EventRunner.FindAvailableChoice(_emissary, ctx, EventRunner.EmissaryLieChoiceId);
            Assert.That(lie, Is.Not.Null);
            Assert.That(lie.Text, Does.Contain("purifier").IgnoreCase);
        }

        [Test]
        public void Emissary_TrustBelowMinus20_ReplacesLieWithPreemptiveFire()
        {
            var crew = new List<Survivor>
            {
                MakeSurvivor("p1", RiskBiasTrait.Paranoid)
            };
            var ctx = MakeContext(crew, scavengerTrust: -25f);

            var available = EventRunner.GetAvailableChoices(_emissary, ctx);
            Assert.That(AvailableHas(available, EventRunner.EmissaryLieChoiceId), Is.False,
                "Threatening trust hides the lie.");
            Assert.That(AvailableHas(available, EventRunner.EmissaryFireChoiceId), Is.True,
                "Threatening trust unlocks preemptive fire for Paranoid crew.");

            Assert.That(_emissary.ResolveBodyText(ctx), Does.Contain("Not asking").IgnoreCase);
        }

        [Test]
        public void Emissary_TrustAtMinus20_ShowsLieNotFire()
        {
            var crew = new List<Survivor> { MakeSurvivor("p1", RiskBiasTrait.Paranoid) };
            var ctx = MakeContext(crew, scavengerTrust: -20f);

            var available = EventRunner.GetAvailableChoices(_emissary, ctx);
            Assert.That(AvailableHas(available, EventRunner.EmissaryLieChoiceId), Is.True);
            Assert.That(AvailableHas(available, EventRunner.EmissaryFireChoiceId), Is.False);
            Assert.That(_emissary.ResolveBodyText(ctx), Does.Not.Contain("Not asking"));
        }

        [Test]
        public void ApplyChoice_SetsEventFlags_AndPushesToSaveCallback()
        {
            var crew = new List<Survivor> { MakeSurvivor("p1", RiskBiasTrait.Paranoid) };
            var saved = new Dictionary<string, bool>();
            var ctx = MakeContext(crew, scavengerTrust: 5f);
            ctx.OnEventFlagChanged = (id, val) => saved[id] = val;

            var runner = new EventRunner();
            var lie = EventRunner.FindAvailableChoice(_emissary, ctx, EventRunner.EmissaryLieChoiceId);
            Assert.That(lie, Is.Not.Null);

            runner.ApplyChoice(_emissary, lie, ctx);

            Assert.That(ctx.HasEventFlag(EventRunner.FlagLiedPurifierBroken), Is.True);
            Assert.That(saved.ContainsKey(EventRunner.FlagLiedPurifierBroken), Is.True);
            Assert.That(saved[EventRunner.FlagLiedPurifierBroken], Is.True);
            Assert.That(ctx.GetEventFlags(), Does.Contain(EventRunner.FlagLiedPurifierBroken));
        }

        [Test]
        public void ApplyChoice_BlocksGatedChoice_EvenIfForced()
        {
            var crew = new List<Survivor> { MakeSurvivor("c1", RiskBiasTrait.Cautious) };
            var ctx = MakeContext(crew, scavengerTrust: 0f);
            var runner = new EventRunner();

            var lie = _emissary.choices.First(c => c.ChoiceId == EventRunner.EmissaryLieChoiceId);
            runner.ApplyChoice(_emissary, lie, ctx);

            Assert.That(ctx.HasEventFlag(EventRunner.FlagLiedPurifierBroken), Is.False,
                "Gated choices must not apply effects or flags.");
        }

        [Test]
        public void RequiredEventFlags_GateFutureChoice()
        {
            var crew = new List<Survivor> { MakeSurvivor("r1", RiskBiasTrait.Realist) };
            var followUp = ScriptableObject.CreateInstance<GameEvent>();
            followUp.id = "emissary_aftermath";
            followUp.bodyText = "Word travels.";
            followUp.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "cash_in_favor",
                    Text = "Call in the favor from the water you shared.",
                    RequiredEventFlags = new List<string> { EventRunner.FlagSharedWaterWithEmissary },
                    HideIfGatesFail = true
                },
                new EventChoice
                {
                    ChoiceId = "say_nothing",
                    Text = "Say nothing.",
                    HideIfGatesFail = true
                }
            };

            try
            {
                var ctxNoFlag = MakeContext(crew);
                Assert.That(
                    AvailableHas(EventRunner.GetAvailableChoices(followUp, ctxNoFlag), "cash_in_favor"),
                    Is.False);

                var ctxWithFlag = MakeContext(crew, flags: new Dictionary<string, bool>
                {
                    [EventRunner.FlagSharedWaterWithEmissary] = true
                });
                Assert.That(
                    AvailableHas(EventRunner.GetAvailableChoices(followUp, ctxWithFlag), "cash_in_favor"),
                    Is.True);
            }
            finally
            {
                Object.DestroyImmediate(followUp);
            }
        }

        [Test]
        public void EventModalUI_HidesLie_ForCautious_ShowsForParanoid()
        {
            var cautious = new List<Survivor> { MakeSurvivor("c1", RiskBiasTrait.Cautious) };
            var ctx = MakeContext(cautious, scavengerTrust: 0f);
            _modal.ShowEvent(_emissary, ctx);

            Assert.That(_modal.IsOpen, Is.True);
            Assert.That(_modal.VisibleChoices.Any(c => c.ChoiceId == EventRunner.EmissaryLieChoiceId), Is.False);
            Assert.That(_modal.VisibleChoices.Count, Is.EqualTo(2)); // share + refuse

            var mixed = new List<Survivor>
            {
                MakeSurvivor("c1", RiskBiasTrait.Cautious),
                MakeSurvivor("p1", RiskBiasTrait.Paranoid)
            };
            ctx = MakeContext(mixed, scavengerTrust: 0f);
            _modal.ShowEvent(_emissary, ctx);

            Assert.That(_modal.VisibleChoices.Any(c => c.ChoiceId == EventRunner.EmissaryLieChoiceId), Is.True);
            Assert.That(_modal.VisibleChoices.Any(c => c.IsAvailable && c.ChoiceId == EventRunner.EmissaryLieChoiceId), Is.True);
        }

        [Test]
        public void EventModalUI_CannotSelectGrayedOrHidden()
        {
            // Grayed path: HideIfGatesFail = false
            var gated = ScriptableObject.CreateInstance<GameEvent>();
            gated.id = "gray_test";
            gated.bodyText = "x";
            gated.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "need_medical",
                    Text = "Perform field surgery.",
                    RequiredTrait = "Medical",
                    HideIfGatesFail = false
                },
                new EventChoice
                {
                    ChoiceId = "walk_away",
                    Text = "Walk away.",
                    HideIfGatesFail = true
                }
            };

            try
            {
                var crew = new List<Survivor> { MakeSurvivor("c1", RiskBiasTrait.Cautious) };
                // MedicalSkill default 0 < 0.5
                var ctx = MakeContext(crew);
                var runner = new EventRunner();
                EventChoice selected = null;
                _modal.OnChoiceSelected += (_, c) => selected = c;

                _modal.ShowEvent(gated, ctx);
                Assert.That(_modal.VisibleChoices.Count, Is.EqualTo(2));
                var medicalRow = _modal.VisibleChoices.First(c => c.ChoiceId == "need_medical");
                Assert.That(medicalRow.IsGrayedOut, Is.True);

                _modal.SelectChoice(0, runner); // grayed medical
                Assert.That(selected, Is.Null);
                Assert.That(_modal.IsOpen, Is.True);

                _modal.SelectChoiceById("need_medical", runner);
                Assert.That(selected, Is.Null);

                _modal.SelectChoiceById("walk_away", runner);
                Assert.That(selected, Is.Not.Null);
                Assert.That(selected.ChoiceId, Is.EqualTo("walk_away"));
                Assert.That(_modal.IsOpen, Is.False);
            }
            finally
            {
                Object.DestroyImmediate(gated);
            }
        }

        [Test]
        public void HasTraitInBunker_MedicalSkill_Threshold()
        {
            var medic = MakeSurvivor("m1", RiskBiasTrait.Realist);
            medic.MedicalSkill = 0.6f;
            var nonMedic = MakeSurvivor("n1", RiskBiasTrait.Realist);
            nonMedic.MedicalSkill = 0.2f;

            var ctx = MakeContext(new List<Survivor> { nonMedic, medic });
            Assert.That(ctx.HasTraitInBunker("Medical"), Is.True);
            Assert.That(ctx.HasTraitInBunker("Paranoid"), Is.False);

            ctx = MakeContext(new List<Survivor> { nonMedic });
            Assert.That(ctx.HasTraitInBunker("Medical"), Is.False);
        }

        [Test]
        public void CreateEmissaryEvent_IdAndChoiceSchema()
        {
            Assert.That(_emissary.id, Is.EqualTo(EventRunner.EmissaryEventId));
            Assert.That(_emissary.choices.Count, Is.EqualTo(4));
            Assert.That(_emissary.choices.Any(c => c.RequiredTrait == "Paranoid"), Is.True);
            Assert.That(_emissary.choices.Any(c => c.SetEventFlags != null && c.SetEventFlags.Count > 0), Is.True);
        }
    }
}
