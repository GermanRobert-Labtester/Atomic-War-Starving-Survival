namespace AtomicWar._Game.Core
{
    /// <summary>
    /// H-4: Generic save/load contract. A system that wants its state
    /// persisted across save/load implements this interface and registers
    /// itself with the SaveSystem. The SaveSystem iterates over all
    /// registered saveables on Save and Load, calling CaptureState and
    /// RestoreState on each.
    ///
    /// Why an interface (H-4 audit, the B-6 class of bug):
    /// Before this interface, every new saveable system required editing
    /// 4 places in SaveSystem: a private field, a Set* setter, a
    /// CaptureSnapshot block, and a RestoreFromSnapshot block. Missing
    /// any of the 4 produced a silent failure — the system state
    /// wasn't saved but the production code didn't know. The interface
    /// collapses the diff to 1 line: <c>saveSystem.Register(system)</c>.
    ///
    /// Backward compatibility: the old per-system Set* setters in
    /// SaveSystem are kept for existing callers. They internally call
    /// Register() so the system is now visible to the save/load loop.
    /// Future systems should use Register() directly and skip the Set*
    /// shim.
    /// </summary>
    public interface ISaveable
    {
        /// <summary>
        /// Stable id used to key the save data. Must be unique within a
        /// single SaveSystem. Convention: snake_case matching the
        /// existing field names on SaveData (e.g. "weather", "inventory",
        /// "worldPhase").
        /// </summary>
        string SaveId { get; }

        /// <summary>
        /// Snapshot the current state as an object that JsonUtility can
        /// serialize. The returned object must be a class marked
        /// [Serializable] with public fields (no auto-properties, no
        /// Dictionaries with non-string keys).
        /// </summary>
        object CaptureState();

        /// <summary>
        /// Apply a previously-captured state. The state argument is
        /// the same shape that CaptureState returned. May be null on
        /// legacy saves — implementations should be null-safe.
        /// Idempotent: calling twice with the same state is safe.
        /// </summary>
        void RestoreState(object state);
    }
}
