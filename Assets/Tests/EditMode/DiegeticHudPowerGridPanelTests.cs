using NUnit.Framework;
using UnityEngine.UIElements;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// PowerGridHUD.Refresh already formats three separate blocks -- a one-line
    /// BudgetSummary, a multi-line SourcesSummary and a multi-line
    /// ConsumersSummary -- so the panel keeps them in three labels and paints
    /// each verbatim. Deliberately not concatenated into one string: that would
    /// allocate a joined copy on every repaint, and the budget line is the only
    /// part that wants the brighter status styling.
    ///
    /// Note this paints the cached properties, never BuildPanelText(), which
    /// calls Refresh() internally and so would recompute the whole model on
    /// every paint.
    /// </summary>
    [TestFixture]
    public class DiegeticHudPowerGridPanelTests
    {
        [Test]
        public void Build_CreatesThePowerGridPanelHidden()
        {
            var view = new DiegeticHudView();
            view.Build();

            Assert.IsNotNull(view.PowerGridPanel, "power grid panel should exist");
            Assert.AreEqual(DisplayStyle.None, view.PowerGridPanel.style.display.value,
                "the power grid panel starts hidden -- closed until it is toggled open");
        }

        [Test]
        public void PaintPowerGrid_ShowsBudgetSourcesAndLoadsVerbatim()
        {
            var view = new DiegeticHudView();
            view.Build();

            const string budget = "POWER  180/240 W  [OK]  CO 2.5 ppm";
            const string sources = "Sources:\n  Diesel generator: 240W [ON] fuel=12.5";
            const string loads = "Loads (priority toggle):\n  P1 Air filtration: 120W [ON]";
            view.PaintPowerGrid(true, budget, sources, loads);

            Assert.AreEqual(DisplayStyle.Flex, view.PowerGridPanel.style.display.value);
            Assert.AreEqual(budget, view.PowerGridBudget.text);
            Assert.AreEqual(sources, view.PowerGridSources.text);
            Assert.AreEqual(loads, view.PowerGridLoads.text);
        }

        [Test]
        public void PaintPowerGrid_ClosedHidesThePanel()
        {
            var view = new DiegeticHudView();
            view.Build();
            view.PaintPowerGrid(true, "POWER  180/240 W  [OK]", "Sources:", "Loads:");

            view.PaintPowerGrid(false, null, null, null);

            Assert.AreEqual(DisplayStyle.None, view.PowerGridPanel.style.display.value);
        }

        [Test]
        public void PaintPowerGrid_WithNullTextDoesNotThrow()
        {
            var view = new DiegeticHudView();
            view.Build();

            Assert.DoesNotThrow(() => view.PaintPowerGrid(true, null, null, null));
        }

        /// <summary>
        /// Drift guard: removes only the power grid panel from an otherwise
        /// complete tree, so it cannot pass vacuously the way the older
        /// empty-root variants do. BindExisting_SucceedsOnACompleteTree in
        /// DiegeticHudEndgamePanelTests is the positive control.
        /// </summary>
        [Test]
        public void BindExisting_FailsWhenOnlyThePowerGridPanelIsMissing()
        {
            var built = new DiegeticHudView();
            var root = built.Build();
            root.Remove(built.PowerGridPanel);

            var view = new DiegeticHudView();

            Assert.IsFalse(view.BindExisting(root),
                "a UXML missing the power grid panel must fall back to Build()");
        }
    }
}
