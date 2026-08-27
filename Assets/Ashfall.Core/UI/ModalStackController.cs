using System;
using System.Collections.Generic;

namespace Ashfall.Core.UI
{
    /// <summary>
    /// Core engine-agnostic modal panel contract.
    /// </summary>
    public interface IModalPanel
    {
        /// <summary>Whether this modal panel is currently visible and active.</summary>
        bool IsModalOpen { get; }

        /// <summary>Event raised whenever the modal is closed.</summary>
        event Action? OnModalClosed;

        /// <summary>Closes the modal panel.</summary>
        void CloseModal();
    }

    /// <summary>
    /// Pure state machine managing modal stacking, LIFO closing, self-close tracking,
    /// and prior-focus reference retention across modal transitions.
    /// </summary>
    /// <typeparam name="TModal">Modal panel contract type.</typeparam>
    /// <typeparam name="TFocus">Host focus element representation (e.g. Control reference or identifier).</typeparam>
    public sealed class ModalStackController<TModal, TFocus> where TModal : class, IModalPanel where TFocus : class
    {
        public sealed class Entry
        {
            public TModal Modal { get; }
            public TFocus? PriorFocus { get; }

            public Entry(TModal modal, TFocus? priorFocus)
            {
                Modal = modal ?? throw new ArgumentNullException(nameof(modal));
                PriorFocus = priorFocus;
            }
        }

        private readonly Stack<Entry> _stack = new Stack<Entry>();
        private readonly HashSet<TModal> _activeModals = new HashSet<TModal>();
        private bool _isClosing;

        public event Action<TModal>? ModalOpened;
        public event Action<TModal, TFocus?>? ModalClosed;

        public bool HasActiveModals => _stack.Count > 0;
        public int ActiveModalCount => _stack.Count;
        public TModal? TopModal => _stack.Count > 0 ? _stack.Peek().Modal : null;

        public void PushModal(TModal modal, TFocus? priorFocus = null)
        {
            if (modal == null) throw new ArgumentNullException(nameof(modal));

            if (_activeModals.Contains(modal))
            {
                return;
            }

            var entry = new Entry(modal, priorFocus);
            _stack.Push(entry);
            _activeModals.Add(modal);

            modal.OnModalClosed += () => HandleModalSelfClosed(modal);
            ModalOpened?.Invoke(modal);
        }

        public bool PopTopModal(out TFocus? focusToRestore)
        {
            focusToRestore = null;
            if (_stack.Count == 0 || _isClosing) return false;

            _isClosing = true;
            try
            {
                var top = _stack.Pop();
                _activeModals.Remove(top.Modal);
                focusToRestore = top.PriorFocus;

                if (top.Modal.IsModalOpen)
                {
                    top.Modal.CloseModal();
                }

                ModalClosed?.Invoke(top.Modal, focusToRestore);
                return true;
            }
            finally
            {
                _isClosing = false;
            }
        }

        public TFocus? CloseAll()
        {
            if (_stack.Count == 0 || _isClosing) return null;

            _isClosing = true;
            try
            {
                TFocus? finalPriorFocus = null;

                while (_stack.Count > 0)
                {
                    var entry = _stack.Pop();
                    _activeModals.Remove(entry.Modal);
                    finalPriorFocus = entry.PriorFocus;

                    if (entry.Modal.IsModalOpen)
                    {
                        entry.Modal.CloseModal();
                    }
                    ModalClosed?.Invoke(entry.Modal, entry.PriorFocus);
                }

                return finalPriorFocus;
            }
            finally
            {
                _isClosing = false;
            }
        }

        private void HandleModalSelfClosed(TModal modal)
        {
            if (_isClosing || !_activeModals.Contains(modal)) return;

            if (_stack.Count > 0 && _stack.Peek().Modal == modal)
            {
                PopTopModal(out _);
                return;
            }

            var temp = new Stack<Entry>();
            TFocus? priorFocusToRestore = null;

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

            ModalClosed?.Invoke(modal, priorFocusToRestore);
        }
    }
}
