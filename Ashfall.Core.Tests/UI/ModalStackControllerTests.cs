using System;
using System.Collections.Generic;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public class ModalStackControllerTests
    {
        private class TestModal : IModalPanel
        {
            public string Name { get; }
            public bool IsModalOpen { get; private set; }
            public event Action? OnModalClosed;
            public int CloseCallCount { get; private set; }

            public TestModal(string name, bool initiallyOpen = true)
            {
                Name = name;
                IsModalOpen = initiallyOpen;
            }

            public void Open()
            {
                IsModalOpen = true;
            }

            public void CloseModal()
            {
                CloseCallCount++;
                IsModalOpen = false;
                OnModalClosed?.Invoke();
            }

            public void SimulateInternalClose()
            {
                IsModalOpen = false;
                OnModalClosed?.Invoke();
            }
        }

        [Fact]
        public void PushModal_SingleModal_BecomesTopModal()
        {
            var controller = new ModalStackController<TestModal, string>();
            var modal = new TestModal("Settings");

            Assert.False(controller.HasActiveModals);
            Assert.Equal(0, controller.ActiveModalCount);

            controller.PushModal(modal, "btn_settings");

            Assert.True(controller.HasActiveModals);
            Assert.Equal(1, controller.ActiveModalCount);
            Assert.Same(modal, controller.TopModal);
        }

        [Fact]
        public void PopTopModal_ClosesModal_AndReturnsPriorFocus()
        {
            var controller = new ModalStackController<TestModal, string>();
            var modal = new TestModal("Settings");

            controller.PushModal(modal, "btn_settings");
            bool popped = controller.PopTopModal(out string? focusToRestore);

            Assert.True(popped);
            Assert.Equal("btn_settings", focusToRestore);
            Assert.False(controller.HasActiveModals);
            Assert.Equal(0, controller.ActiveModalCount);
            Assert.Null(controller.TopModal);
            Assert.False(modal.IsModalOpen);
            Assert.Equal(1, modal.CloseCallCount);
        }

        [Fact]
        public void StackedModals_PopInLIFOOrder_WithCorrespondingFocus()
        {
            var controller = new ModalStackController<TestModal, string>();
            var modalA = new TestModal("Survivors");
            var modalB = new TestModal("SurvivorDetail");
            var modalC = new TestModal("AfflictionDetail");

            controller.PushModal(modalA, "nav_survivors");
            controller.PushModal(modalB, "row_survivor_1");
            controller.PushModal(modalC, "item_affliction_burn");

            Assert.Equal(3, controller.ActiveModalCount);
            Assert.Same(modalC, controller.TopModal);

            // Pop top (modalC)
            Assert.True(controller.PopTopModal(out string? focus1));
            Assert.Equal("item_affliction_burn", focus1);
            Assert.Equal(2, controller.ActiveModalCount);
            Assert.Same(modalB, controller.TopModal);
            Assert.False(modalC.IsModalOpen);
            Assert.True(modalB.IsModalOpen);

            // Pop next (modalB)
            Assert.True(controller.PopTopModal(out string? focus2));
            Assert.Equal("row_survivor_1", focus2);
            Assert.Equal(1, controller.ActiveModalCount);
            Assert.Same(modalA, controller.TopModal);
            Assert.False(modalB.IsModalOpen);
            Assert.True(modalA.IsModalOpen);

            // Pop last (modalA)
            Assert.True(controller.PopTopModal(out string? focus3));
            Assert.Equal("nav_survivors", focus3);
            Assert.Equal(0, controller.ActiveModalCount);
            Assert.Null(controller.TopModal);
            Assert.False(modalA.IsModalOpen);
        }

        [Fact]
        public void SelfClose_WhenUserClicksCloseButton_UnwindsStackCorrectly()
        {
            var controller = new ModalStackController<TestModal, string>();
            var modalA = new TestModal("PanelA");
            var modalB = new TestModal("PanelB");

            controller.PushModal(modalA, "focus_a");
            controller.PushModal(modalB, "focus_b");

            Assert.Equal(2, controller.ActiveModalCount);
            Assert.Same(modalB, controller.TopModal);

            // Modal B closes itself from an internal button
            modalB.SimulateInternalClose();

            Assert.Equal(1, controller.ActiveModalCount);
            Assert.Same(modalA, controller.TopModal);
        }

        [Fact]
        public void CloseAll_ClosesAllModalsInReverseOrder_AndReturnsInitialFocus()
        {
            var controller = new ModalStackController<TestModal, string>();
            var modalA = new TestModal("PanelA");
            var modalB = new TestModalPanel("PanelB");
            var modalC = new TestModalPanel("PanelC");

            controller.PushModal(modalA, "focus_root");
            controller.PushModal(modalB, "focus_a");
            controller.PushModal(modalC, "focus_b");

            var closedOrder = new List<string>();
            controller.ModalClosed += (m, f) => closedOrder.Add(m.Name);

            string? finalFocus = controller.CloseAll();

            Assert.False(controller.HasActiveModals);
            Assert.Equal(0, controller.ActiveModalCount);
            Assert.False(modalA.IsModalOpen);
            Assert.False(modalB.IsModalOpen);
            Assert.False(modalC.IsModalOpen);

            Assert.Equal("focus_root", finalFocus);
            Assert.Equal(new[] { "PanelC", "PanelB", "PanelA" }, closedOrder);
        }

        [Fact]
        public void PreventDuplicatePush_SameModalInstance_DoesNotDuplicateOnStack()
        {
            var controller = new ModalStackController<TestModal, string>();
            var modal = new TestModal("Settings");

            controller.PushModal(modal, "focus_settings");
            controller.PushModal(modal, "focus_settings");

            Assert.Equal(1, controller.ActiveModalCount);
            Assert.Same(modal, controller.TopModal);
        }

        [Fact]
        public void EmptyStack_PopTopModal_ReturnsFalseWithoutThrowing()
        {
            var controller = new ModalStackController<TestModal, string>();
            Assert.False(controller.PopTopModal(out string? focus));
            Assert.Null(focus);
            Assert.Equal(0, controller.ActiveModalCount);
        }

        private class TestModalPanel : TestModal
        {
            public TestModalPanel(string name) : base(name) { }
        }
    }
}
