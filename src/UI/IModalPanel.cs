using Godot;
using System;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Standardized Godot host contract for UI panels and dialogs that can be opened as modals/overlays
    /// with managed focus stack, keyboard dismissal, and focus restoration.
    /// </summary>
    public interface IModalPanel : Ashfall.Core.UI.IModalPanel
    {
        /// <summary>Whether this modal panel is currently visible and active.</summary>
        bool IsModalOpen { get; }

        /// <summary>Event raised whenever the modal is closed (either by user action or manager).</summary>
        event Action? OnModalClosed;

        /// <summary>Closes the modal panel.</summary>
        void CloseModal();

        /// <summary>
        /// Optional preferred control to focus upon opening.
        /// If null, ModalManager focuses the first available interactive child.
        /// </summary>
        Control? InitialFocusControl { get; }
    }
}
