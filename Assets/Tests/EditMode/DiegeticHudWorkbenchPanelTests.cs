using NUnit.Framework;
using UnityEngine.UIElements;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// WorkbenchUI already builds a formatted, numbered PanelSummary (see
    /// WorkbenchSystem.WorkbenchLine and WorkbenchUI.RebuildPanel) and
    /// PlayerInputHandler already routes [B] and [1-9] to it. Only the
    /// drawing was missing -- this panel paints that string verbatim,
    /// the same way HatchAmmo/HatchArms paint provider-supplied text.
    /// </summary>
    [TestFixture]
    public class DiegeticHudWorkbenchPanelTests
    {
        [Test]
        public void Build_CreatesTheWorkbenchPanelHidden()
        {
            var view = new DiegeticHudView();
            view.Build();

            Assert.IsNotNull(view.WorkbenchPanel, "workbench panel should exist");
            Assert.AreEqual(DisplayStyle.None, view.WorkbenchPanel.style.display.value,
                "the workbench panel starts hidden -- closed until [B] is pressed");
        }

        [Test]
        public void PaintWorkbench_ShowsTheFormattedSummaryVerbatim()
        {
            var view = new DiegeticHudView();
            view.Build();

            const string summary = "WORKBENCH  [B] toggle  ·  [1-9] execute\n1. [OK] Disassemble radio  (2x ElectronicScrap)";
            view.PaintWorkbench(true, summary);

            Assert.AreEqual(DisplayStyle.Flex, view.WorkbenchPanel.style.display.value);
            Assert.AreEqual(summary, view.WorkbenchBody.text);
        }

        [Test]
        public void PaintWorkbench_ClosedHidesThePanel()
        {
            var view = new DiegeticHudView();
            view.Build();
            view.PaintWorkbench(true, "WORKBENCH");

            view.PaintWorkbench(false, null);

            Assert.AreEqual(DisplayStyle.None, view.WorkbenchPanel.style.display.value);
        }

        [Test]
        public void PaintWorkbench_WithNullSummary_DoesNotThrow()
        {
            var view = new DiegeticHudView();
            view.Build();

            Assert.DoesNotThrow(() => view.PaintWorkbench(true, null));
        }

        [Test]
        public void BindExisting_FailsWhenTheWorkbenchPanelIsMissing()
        {
            var root = new VisualElement { name = DiegeticHudView.RootName };
            var view = new DiegeticHudView();

            Assert.IsFalse(view.BindExisting(root));
        }
    }
}
