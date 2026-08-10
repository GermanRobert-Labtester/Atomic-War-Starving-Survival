using NUnit.Framework;
using UnityEngine.UIElements;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// EndgameSummaryUI already builds both a one-line StatusLine and a
    /// formatted multi-line DetailSummary in its Refresh() (outcome, death
    /// screen label, tallies). Like the workbench panel, this one paints those
    /// strings verbatim rather than re-deriving them from the raw counters.
    ///
    /// Unlike hatch/stores/workbench, EndgameSummaryUI exposes no change event,
    /// so visibility here is push-driven: whoever calls Show()/Hide() is
    /// responsible for the repaint. These tests cover the view contract only.
    /// </summary>
    [TestFixture]
    public class DiegeticHudEndgamePanelTests
    {
        [Test]
        public void Build_CreatesTheEndgamePanelHidden()
        {
            var view = new DiegeticHudView();
            view.Build();

            Assert.IsNotNull(view.EndgamePanel, "endgame panel should exist");
            Assert.AreEqual(DisplayStyle.None, view.EndgamePanel.style.display.value,
                "the endgame panel starts hidden -- the campaign is still running");
        }

        [Test]
        public void PaintEndgame_ShowsTheStatusLineAndSummaryVerbatim()
        {
            var view = new DiegeticHudView();
            view.Build();

            const string status = "ENDGAME [Died]  Day 42  RAD 7  choices 13";
            const string detail = "=== ENDGAME SUMMARY ===\nThe last filter clogged.\n---\nDays survived: 42";
            view.PaintEndgame(true, status, detail);

            Assert.AreEqual(DisplayStyle.Flex, view.EndgamePanel.style.display.value);
            Assert.AreEqual(status, view.EndgameStatus.text);
            Assert.AreEqual(detail, view.EndgameBody.text);
        }

        [Test]
        public void PaintEndgame_HiddenHidesThePanel()
        {
            var view = new DiegeticHudView();
            view.Build();
            view.PaintEndgame(true, "ENDGAME [Died]", "summary");

            view.PaintEndgame(false, null, null);

            Assert.AreEqual(DisplayStyle.None, view.EndgamePanel.style.display.value);
        }

        [Test]
        public void PaintEndgame_WithNullTextDoesNotThrow()
        {
            var view = new DiegeticHudView();
            view.Build();

            Assert.DoesNotThrow(() => view.PaintEndgame(true, null, null));
        }

        /// <summary>
        /// Positive control for the drift guard below. Without this, that test
        /// would still pass if BindExisting rejected every tree for an
        /// unrelated reason.
        /// </summary>
        [Test]
        public void BindExisting_SucceedsOnACompleteTree()
        {
            var built = new DiegeticHudView();
            var root = built.Build();

            var view = new DiegeticHudView();

            Assert.IsTrue(view.BindExisting(root),
                "a tree carrying every panel must bind rather than fall back to Build()");
        }

        /// <summary>
        /// Drift guard: if the UXML ever loses the endgame panel, BindExisting
        /// must fail so the controller falls back to Build(). Note this removes
        /// *only* the endgame panel from an otherwise complete tree -- passing
        /// an empty root (as the older panel tests do) proves nothing here,
        /// because it already fails on the first missing panel.
        /// </summary>
        [Test]
        public void BindExisting_FailsWhenOnlyTheEndgamePanelIsMissing()
        {
            var built = new DiegeticHudView();
            var root = built.Build();
            root.Remove(built.EndgamePanel);

            var view = new DiegeticHudView();

            Assert.IsFalse(view.BindExisting(root),
                "a UXML missing the endgame panel must fall back to Build()");
        }
    }
}
