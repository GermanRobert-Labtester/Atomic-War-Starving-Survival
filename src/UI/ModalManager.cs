// SPDX-License-Identifier: MIT
using Godot;
using System;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Host adapter over Core ModalStackController managing stacked modal panels,
    /// modal input trapping, Escape/Close action handling, and prior keyboard-focus restoration.
    /// </summary>
    public sealed class ModalManager
    {
        private readonly ModalStackController<IModalPanel, Control> _core = new ModalStackController<IModalPanel, Control>();

        public event Action<IModalPanel>? ModalOpened;
        public event Action<IModalPanel>? ModalClosed;

        public bool HasActiveModals => _core.HasActiveModals;
        public int ActiveModalCount => _core.ActiveModalCount;
        public IModalPanel? TopModal => _core.TopModal;

        public ModalManager()
        {
            _core.ModalOpened += modal => ModalOpened?.Invoke(modal);
            _core.ModalClosed += (modal, priorFocus) =>
            {
                RestoreFocus(priorFocus);
                ModalClosed?.Invoke(modal);
            };
        }

        /// <summary>
        /// Pushes and opens a modal panel onto the stack, capturing the current focus owner for restoration.
        /// </summary>
        public void PushModal(IModalPanel modal, Control? priorFocus = null, Control? initialFocus = null)
        {
            if (modal == null) throw new ArgumentNullException(nameof(modal));

            // Capture prior focus owner if not provided
            if (priorFocus == null && modal is Control controlNode && controlNode.IsInsideTree())
            {
                priorFocus = controlNode.GetViewport()?.GuiGetFocusOwner();
            }

            _core.PushModal(modal, priorFocus);
            FocusModalControl(modal, initialFocus);
        }

        /// <summary>
        /// Closes and pops the topmost modal on the stack, restoring focus to the prior control.
        /// </summary>
        public bool PopTopModal()
        {
            return _core.PopTopModal(out _);
        }

        /// <summary>
        /// Closes all active modals from top to bottom, restoring the initial base focus.
        /// </summary>
        public void CloseAll()
        {
            _core.CloseAll();
        }

        /// <summary>
        /// Handles unhandled input events for active modals.
        /// Traps Tab cycling, navigates via arrow/D-pad, and handles Escape/Close.
        /// </summary>
        public bool HandleInput(InputEvent @event)
        {
            if (!HasActiveModals || @event == null) return false;

            var top = TopModal;
            if (top is Control ctrl && GodotObject.IsInstanceValid(ctrl) && ctrl.Visible)
            {
                // 1. Tab trapping inside top modal
                if (AshfallFocusPolicy.TrapFocus(ctrl, @event))
                {
                    return true;
                }

                // 2. Nav input within top modal
                if (AshfallFocusNavigator.HandleNavInput(ctrl, @event))
                {
                    return true;
                }
            }

            // 3. Close on Escape / Cancel
            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                if (PopTopModal())
                {
                    return true;
                }
            }

            return false;
        }

        private static void FocusModalControl(IModalPanel modal, Control? preferredFocus)
        {
            Control? target = preferredFocus ?? modal.InitialFocusControl;
            if (target != null && GodotObject.IsInstanceValid(target) && target.IsInsideTree() && target.Visible)
            {
                target.CallDeferred(Control.MethodName.GrabFocus);
                return;
            }

            if (modal is Control ctrl && GodotObject.IsInstanceValid(ctrl) && ctrl.IsInsideTree())
            {
                var firstInteractive = AshfallFocusPolicy.FindFirstFocusable(ctrl);
                if (firstInteractive != null)
                {
                    firstInteractive.CallDeferred(Control.MethodName.GrabFocus);
                }
            }
        }

        private static void RestoreFocus(Control? target)
        {
            if (target != null && GodotObject.IsInstanceValid(target) && target.IsInsideTree() && target.Visible)
            {
                target.CallDeferred(Control.MethodName.GrabFocus);
            }
        }

        public static Control? FindFirstFocusable(Control parent)
        {
            return AshfallFocusPolicy.FindFirstFocusable(parent);
        }
    }
}
