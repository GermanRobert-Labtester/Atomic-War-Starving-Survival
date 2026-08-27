using System;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Contract for UI panels that bind to host sessions, stateful models, or engines.
    /// Enforces clean lifecycle management: rebind safety, event unsubscription, and tree exit cleanup.
    /// </summary>
    public interface IBindablePanel
    {
        /// <summary>True if the panel is currently bound to an active session/model.</summary>
        bool IsBound { get; }

        /// <summary>
        /// Explicitly unbinds the panel from any active session, unsubscribing all event delegates.
        /// </summary>
        void Unbind();
    }
}
