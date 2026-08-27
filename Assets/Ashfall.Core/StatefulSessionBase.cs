// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core
{
    /// <summary>
    /// Base class for all stateful domain and host sessions. Standardizes state-change,
    /// dirty tracking, monotonically increasing state versioning, presentation
    /// refresh separation, and save flush semantics (Task 108).
    /// </summary>
    public class StatefulSessionBase : IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// True if there are unpersisted domain mutations since the last save flush.
        /// </summary>
        public bool IsDirty { get; protected set; }

        /// <summary>
        /// Monotonically increasing state version. Increments exactly once per
        /// actual domain mutation.
        /// </summary>
        public long StateVersion { get; protected set; }

        /// <summary>
        /// Total number of successful Save() flushes executed by this session.
        /// </summary>
        public int SaveCount { get; protected set; }

        /// <summary>
        /// Raised when persistent domain state mutates (for UI re-bind and dirty-flush tracking).
        /// </summary>
        public event Action? StateChanged;

        /// <summary>
        /// Raised with the new state version whenever persistent state mutates.
        /// </summary>
        public event Action<long>? StateVersionChanged;

        /// <summary>
        /// Raised when presentational elements change (selection, filter, hover, UI tabs)
        /// without mutating any persistent gameplay or domain state.
        /// </summary>
        public event Action? PresentationRefreshRequested;

        /// <summary>
        /// Records an explicit persistent domain mutation: increments StateVersion,
        /// sets IsDirty = true, and raises StateChanged / StateVersionChanged.
        /// Returns true if mutation was recorded.
        /// </summary>
        public bool RaiseStateChanged()
        {
            IsDirty = true;
            StateVersion++;
            StateChanged?.Invoke();
            StateVersionChanged?.Invoke(StateVersion);
            return true;
        }

        /// <summary>
        /// Conditional state change helper: only marks dirty, increments StateVersion,
        /// and emits events if condition is true (e.g. action succeeded).
        /// </summary>
        public bool RaiseStateChangedIf(bool condition)
        {
            if (!condition) return false;
            return RaiseStateChanged();
        }

        /// <summary>
        /// Helper for ActionResult commands: only emits StateChanged if result is Success or Partial.
        /// Failed, Blocked, or Cancelled actions will NOT mark dirty or increment StateVersion.
        /// </summary>
        public ActionResult HandleActionResult(ActionResult result)
        {
            if (result.IsSuccess || result.IsSuccessOrPartial)
            {
                RaiseStateChanged();
            }
            return result;
        }

        /// <summary>
        /// Public convenience: mark dirty and raise StateChanged in one call.
        /// </summary>
        public bool MarkDirty()
        {
            return RaiseStateChanged();
        }

        /// <summary>
        /// Clears the dirty flag without incrementing StateVersion or firing StateChanged.
        /// Useful during save restore or clean initializations.
        /// </summary>
        public void ClearDirty()
        {
            IsDirty = false;
        }

        /// <summary>
        /// Requests a presentation-only refresh (e.g. selection, filter, or tab change).
        /// Does NOT modify IsDirty, does NOT increment StateVersion, and does NOT trigger save.
        /// </summary>
        public void RequestPresentationRefresh()
        {
            PresentationRefreshRequested?.Invoke();
        }

        /// <summary>
        /// Persist state if dirty. Subclasses override to call their save store.
        /// The base implementation increments SaveCount and clears IsDirty.
        /// </summary>
        public virtual void Save()
        {
            if (!IsDirty) return;
            SaveCount++;
            IsDirty = false;
        }

        /// <summary>Unsubscribe from all events to prevent handler accumulation.</summary>
        public virtual void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            StateChanged = null;
            StateVersionChanged = null;
            PresentationRefreshRequested = null;
            UnsubscribeSystemEvents();
            GC.SuppressFinalize(this);
        }

        /// <summary>Override to unsubscribe from Core system events.</summary>
        protected virtual void UnsubscribeSystemEvents() { }
    }
}
