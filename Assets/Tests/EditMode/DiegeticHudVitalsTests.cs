using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.UIElements;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// The vitals panel is the only on-screen report of the core loop: time,
    /// dose and the four needs. DiegeticHudView builds a VisualElement tree with
    /// no UIDocument, so it can be painted and read back here.
    /// </summary>
    [TestFixture]
    public class DiegeticHudVitalsTests
    {
        static Dictionary<string, NeedBarData> Needs(params (string id, float value)[] entries)
        {
            var d = new Dictionary<string, NeedBarData>();
            foreach (var (id, value) in entries)
            {
                d[id] = new NeedBarData
                {
                    NeedId = id,
                    DisplayName = id.ToUpperInvariant(),
                    CurrentValue = value,
                    MaxValue = 100f
                };
            }
            return d;
        }

        [Test]
        public void Build_CreatesTheVitalsPanel()
        {
            var view = new DiegeticHudView();
            view.Build();

            Assert.IsNotNull(view.VitalsPanel, "vitals panel should exist");
            Assert.IsNotNull(view.VitalsClock, "clock label should exist");
            Assert.IsNotNull(view.VitalsDose, "dose label should exist");
        }

        [Test]
        public void PaintVitals_ShowsDayTimeAndDose()
        {
            var view = new DiegeticHudView();
            view.Build();

            view.PaintVitals(3, 4f, 0.42f, 1.5f, Needs(("hunger", 62f)));

            StringAssert.Contains("DAY 3", view.VitalsClock.text);
            StringAssert.Contains("04:00", view.VitalsClock.text);
            StringAssert.Contains("0.42", view.VitalsDose.text);
        }

        [Test]
        public void PaintVitals_RendersOneRowPerNeed()
        {
            var view = new DiegeticHudView();
            view.Build();

            view.PaintVitals(1, 0f, 0f, 0f,
                Needs(("hunger", 62f), ("thirst", 48f), ("fatigue", 25f), ("warmth", 71f)));

            Assert.AreEqual(4, view.VitalsNeeds.childCount, "one row per need");
        }

        /// <summary>
        /// Zero is a meaningful reading. A need the model does not carry is not
        /// the same as a need at zero, and must not be drawn as starvation.
        /// </summary>
        [Test]
        public void PaintVitals_RendersAbsentNeedAsPlaceholder_NotZero()
        {
            var view = new DiegeticHudView();
            view.Build();

            view.PaintVitals(1, 0f, 0f, 0f, Needs(("hunger", 0f)));

            var row = view.VitalsNeeds.Q<Label>("vitals-need-thirst-value");
            Assert.IsNotNull(row, "every core need gets a row even when absent from the model");
            Assert.AreEqual("--", row.text, "absent need must not read as 0%");
        }

        [Test]
        public void PaintVitals_WithNulls_DoesNotThrow()
        {
            var view = new DiegeticHudView();
            view.Build();

            Assert.DoesNotThrow(() => view.PaintVitals(1, 0f, 0f, 0f, null));
        }

        /// <summary>
        /// Drift guard. The UXML and Build() describe the same tree twice; if the
        /// UXML loses an element, BindExisting must fail so the controller falls
        /// back to Build() rather than binding a half-tree and rendering nothing.
        /// </summary>
        [Test]
        public void BindExisting_FailsWhenTheVitalsPanelIsMissing()
        {
            var root = new VisualElement { name = DiegeticHudView.RootName };
            var view = new DiegeticHudView();

            Assert.IsFalse(view.BindExisting(root),
                "a tree without the vitals panel must not bind successfully");
        }
    }
}
