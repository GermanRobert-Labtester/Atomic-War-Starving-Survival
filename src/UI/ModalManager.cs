using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Central manager for stacked modal panels, Escape/Close action handling,
    /// and prior keyboard-focus restoration.
    /// </summary>
    public sealed class ModalManager
    {
        public sealed class ModalEntry
        {
            public IModalPanel Modal { get; }
            public Control? PriorFocus { get; }

            public ModalEntry(IModalPanel modal, Control? priorFocus)
            {
                Modal = modal ?? throw new ArgumentNullException(nameof(modal));
                PriorFocus = priorFocus;
            }
        }

        private readonly Stack<ModalEntry> _stack = new Stack<ModalEntry>();
        private readonly HashSet<IModalPanel> _activeModals = new HashSet<IModalPanel>();
        private bool _isClosing = false;

        public event Action<IModalPanel>? ModalOpened;
        public event Action<IModalPanel>? ModalClosed;

        public bool HasActiveModals => _stack.Count > 0;
        public int ActiveModalCount => _stack.Count;
        public IModalPanel? TopModal => _stack.Count > 0 ? _stack.Peek().Modal : null;

        /// <summary>
        /// Pushes and opens a modal panel onto the stack, capturing the current focus owner for restoration.
        /// </summary>
        public void PushModal(IModalPanel modal, Control? priorFocus = null, Control? initialFocus = null)
        {
            if (modal == null) throw new ArgumentNullException(nameof(modal));

            // Prevent duplicate entries of the same modal instance on the stack
            if (_activeModals.Contains(modal))
            {
                FocusModalControl(modal, initialFocus);
                return;
            }

            // Capture prior focus owner if not provided
            if (priorFocus == null && modal is Control controlNode && controlNode.IsInsideTree())
            {
                priorFocus = controlNode.GetViewport()?.GuiGetFocusOwner();
            }

            var entry = new ModalEntry(modal, priorFocus);
            _stack.Push(entry);
            _activeModals.Add(modal);

            // Subscribe to panel-initiated close
            modal.OnModalClosed += () => HandleModalSelfClosed(modal);

            FocusModalControl(modal, initialFocus);
            ModalOpened?.Invoke(modal);
        }

        /// <summary>
        /// Closes and pops the topmost modal on the stack, restoring focus to the prior control.
        /// </summary>
        public bool PopTopModal()
        {
            if (_stack.Count == 0 || _isClosing) return false;

            _isClosing = true;
            try
            {
                var top = _stack.Pop();
                _activeModals.Remove(top.Modal);

                if (top.Modal.IsModalOpen)
                {
                    top.Modal.CloseModal();
                }

                RestoreFocus(top.PriorFocus);
                ModalClosed?.Invoke(top.Modal);
                return true;
            }
            finally
            {
                _isClosing = false;
            }
        }

        /// <summary>
        /// Closes all active modals from top to bottom, restoring the initial base focus.
        /// </summary>
        public void CloseAll()
        {
            if (_stack.Count == 0 || _isClosing) return;

            _isClosing = true;
            try
            {
                Control? finalPriorFocus = null;

                while (_stack.Count > 0)
                {
                    var entry = _stack.Pop();
                    _activeModals.Remove(entry.Modal);
                    finalPriorFocus = entry.PriorFocus;

                    if (entry.Modal.IsModalOpen)
                    {
                        entry.Modal.CloseModal();
                    }
                    ModalClosed?.Invoke(entry.Modal);
                }

                RestoreFocus(finalPriorFocus);
            }
            finally
            {
                _isClosing = false;
            }
        }

        /// <summary>
        /// Handles unhandled input events. Returns true if an active modal consumed the close/escape action.
        /// </summary>
        public bool HandleInput(InputEvent @event)
        {
            if (!HasActiveModals || @event == null) return false;

            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                if (PopTopModal())
                {
                    if (@event is InputEvent inputEv && inputEv is not null)
                    {
                        // Mark input as handled
                    }
                    return true;
                }
            }

            return false;
        }

        private void HandleModalSelfClosed(IModalPanel modal)
        {
            if (_isClosing || !_activeModals.Contains(modal)) return;

            // If the top modal closed itself, pop and restore
            if (_stack.Count > 0 && _stack.Peek().Modal == modal)
            {
                PopTopModal();
                return;
            }

            // If a modal lower in the stack closed itself, rebuild stack preserving order
            var temp = new Stack<ModalEntry>();
            Control? priorFocusToRestore = null;

            while (_stack.Count > 0)
            {
                var entry = _stack.Pop();
                if (entry.Modal == modal)
                {
                    _activeModals.Remove(modal);
                    priorFocusToRestore = entry.PriorFocus;
                    break;
                }
                temp.Push(entry);
            }

            while (temp.Count > 0)
            {
                _stack.Push(temp.Pop());
            }

            if (priorFocusToRestore != null && _stack.Count == 0)
            {
                RestoreFocus(priorFocusToRestore);
            }

            ModalClosed?.Invoke(modal);
        }

        private static void FocusModalControl(IModalPanel modal, Control? preferredFocus)
        {
            Control? target = preferredFocus ?? modal.InitialFocusControl;
            if (target != null && GodotObject.IsInstanceValid(target) && target.IsInsideTree() && target.Visible)
            {
                target.GrabFocus();
                return;
            }

            if (modal is Control ctrl && GodotObject.IsInstanceValid(ctrl) && ctrl.IsInsideTree())
            {
                var firstInteractive = FindFirstFocusable(ctrl);
                if (firstInteractive != null)
                {
                    firstInteractive.GrabFocus();
                }
            }
        }

        private static void RestoreFocus(Control? target)
        {
            if (target != null && GodotObject.IsInstanceValid(target) && target.IsInsideTree() && target.Visible)
            {
                target.GrabFocus();
            }
        }

        public static Control? FindFirstFocusable(Control parent)
        {
            if (parent == null || !GodotObject.IsInstanceValid(parent)) return null;

            if (parent.FocusMode != Control.FocusModeEnum.None && (parent is Button || parent is LineEdit || parent is OptionButton || parent is CheckButton || parent is ItemList))
            {
                return parent;
            }

            foreach (var child in parent.GetChildren())
            {
                if (child is Control ctrlChild && ctrlChild.Visible)
                {
                    var found = FindFirstFocusable(ctrlChild);
                    if (found != null) return found;
                }
            }

            return null;
        }
    }
}
