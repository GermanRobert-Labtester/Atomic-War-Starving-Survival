using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.UIElements;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// The event panel completes the loop: EventRunner fires, EventModalUI
    /// resolves the body and the visible choices, PlayerInputHandler routes
    /// keys 1/2/3. Only the drawing was missing.
    /// </summary>
    [TestFixture]
    public class DiegeticHudEventModalTests
    {
        static List<EventChoiceLine> Choices(params (string text, bool enabled)[] rows)
        {
            var list = new List<EventChoiceLine>();
            foreach (var (text, enabled) in rows)
                list.Add(new EventChoiceLine(text, enabled));
            return list;
        }

        [Test]
        public void Build_CreatesTheEventPanelHidden()
        {
            var view = new DiegeticHudView();
            view.Build();

            Assert.IsNotNull(view.EventPanel, "event panel should exist");
            Assert.AreEqual(DisplayStyle.None, view.EventPanel.style.display.value,
                "the event panel starts hidden -- there is no event on boot");
        }

        [Test]
        public void PaintEventModal_ShowsTitleBodyAndChoices()
        {
            var view = new DiegeticHudView();
            view.Build();

            view.PaintEventModal(true, "A knock at the hatch", "Someone is outside.",
                Choices(("Open the hatch", true), ("Stay silent", true)));

            Assert.AreEqual(DisplayStyle.Flex, view.EventPanel.style.display.value);
            Assert.AreEqual("A knock at the hatch", view.EventTitle.text);
            Assert.AreEqual("Someone is outside.", view.EventBody.text);
            Assert.AreEqual(2, view.EventChoices.childCount);
        }

        /// <summary>
        /// The numbers are the control scheme, not decoration: PlayerInputHandler
        /// maps Alpha1 to visible index 0. A row that does not show its number
        /// cannot be chosen.
        /// </summary>
        [Test]
        public void PaintEventModal_NumbersChoicesFromOne()
        {
            var view = new DiegeticHudView();
            view.Build();

            view.PaintEventModal(true, "t", "b", Choices(("First", true), ("Second", true)));

            var first = view.EventChoices[0] as Label;
            var second = view.EventChoices[1] as Label;
            StringAssert.StartsWith("[1]", first.text);
            StringAssert.StartsWith("[2]", second.text);
        }

        [Test]
        public void PaintEventModal_MarksUnavailableChoices()
        {
            var view = new DiegeticHudView();
            view.Build();

            view.PaintEventModal(true, "t", "b", Choices(("Bribe them", false)));

            var row = view.EventChoices[0];
            Assert.IsTrue(row.ClassListContains("event-choice--disabled"),
                "an unavailable choice must look different from one you can press");
        }

        [Test]
        public void PaintEventModal_ClosedHidesThePanel()
        {
            var view = new DiegeticHudView();
            view.Build();
            view.PaintEventModal(true, "t", "b", Choices(("a", true)));

            view.PaintEventModal(false, null, null, null);

            Assert.AreEqual(DisplayStyle.None, view.EventPanel.style.display.value);
        }

        [Test]
        public void PaintEventModal_WithNulls_DoesNotThrow()
        {
            var view = new DiegeticHudView();
            view.Build();

            Assert.DoesNotThrow(() => view.PaintEventModal(true, null, null, null));
        }

        [Test]
        public void BindExisting_FailsWhenTheEventPanelIsMissing()
        {
            var root = new VisualElement { name = DiegeticHudView.RootName };
            var view = new DiegeticHudView();

            Assert.IsFalse(view.BindExisting(root));
        }
    }
}
