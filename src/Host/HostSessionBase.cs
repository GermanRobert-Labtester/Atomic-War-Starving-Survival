using System;
using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Base class for all Godot host sessions. Provides dirty-tracking so the
    /// host save orchestrator can flush only changed subsystems.
    /// </summary>
    public class HostSessionBase : IDisposable
    {
        public bool IsDirty { get; protected set; }
        private bool _disposed;

        /// <summary>Raised when the session state changes (for UI + save dirty tracking).</summary>
        public event Action? StateChanged;

        /// <summary>Raise StateChanged and mark dirty. Safe to call from derived classes.</summary>
        protected void RaiseStateChanged()
        {
            IsDirty = true;
            StateChanged?.Invoke();
        }

        /// <summary>
        /// Public convenience: mark dirty and raise StateChanged in one call.
        /// </summary>
        public void MarkDirty()
        {
            IsDirty = true;
            StateChanged?.Invoke();
        }

        /// <summary>
        /// Persist state if dirty. Subclasses override to call their save store.
        /// The default implementation only clears the dirty flag.
        /// </summary>
        public virtual void Save()
        {
            if (!IsDirty) return;
            IsDirty = false;
        }

        /// <summary>Unsubscribe from all events to prevent handler accumulation.</summary>
        public virtual void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            StateChanged = null;
            UnsubscribeSystemEvents();
            GC.SuppressFinalize(this);
        }

        /// <summary>Override to unsubscribe from Core system events.</summary>
        protected virtual void UnsubscribeSystemEvents() { }
    }
}
