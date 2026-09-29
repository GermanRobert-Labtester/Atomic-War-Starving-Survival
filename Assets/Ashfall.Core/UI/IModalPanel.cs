// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.UI
{
    /// <summary>
    /// Core engine-agnostic modal panel contract. Implemented by host modal
    /// panels (ConfirmationModal, MoralChoiceModal) and used as a
    /// registration/focus contract by the host UI layer.
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
}
