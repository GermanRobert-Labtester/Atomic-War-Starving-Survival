namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Serializes/deserializes every save-safe system snapshot to disk (JSON)
    /// under a named slot. Coordinates with each system's serializable state so
    /// a load fully reconstructs a prior session.
    /// </summary>
    public class SaveSystem
    {
        /// <summary>Write the current world state to the given slot.</summary>
        public void Save(string slotId) => throw new System.NotImplementedException();

        /// <summary>Replace the current world state from the given slot.</summary>
        public void Load(string slotId) => throw new System.NotImplementedException();

        /// <summary>Whether a save exists for the given slot.</summary>
        public bool SlotExists(string slotId) => throw new System.NotImplementedException();

        /// <summary>Delete a save slot.</summary>
        public void Delete(string slotId) => throw new System.NotImplementedException();
    }
}
