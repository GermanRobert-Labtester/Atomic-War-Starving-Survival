using System;
using System.IO;
using UnityEngine;

namespace AtomicWar._Game.Utilities
{
    /// <summary>
    /// Single source of truth for where save slots live on disk and what their
    /// files are called.
    ///
    /// This lives in Utilities (the root assembly) rather than next to
    /// SaveSystem because two assemblies need it and they cannot see each
    /// other: SaveSystem is in AtomicWar._Game.Core, while the main menu's
    /// "Continue" probe is in AtomicWar._Game.UI, and the reference direction
    /// is Core -> UI. Without a shared home the naming convention would have to
    /// be duplicated, and a future rename would silently break Continue while
    /// leaving saving itself working.
    ///
    /// The main menu deliberately probes with plain File.Exists instead of
    /// constructing a SaveSystem: SaveSystem needs a fully-built world
    /// (CoreDeps) which does not exist yet on the start screen.
    /// </summary>
    public static class SaveSlotPaths
    {
        /// <summary>Folder name under Application.persistentDataPath.</summary>
        public const string SavesFolderName = "saves";

        private const string FilePrefix = "save_";
        private const string FileExtension = ".json";
        private const string BackupExtension = ".bak";

        /// <summary>
        /// Default saves directory. Not cached: Application.persistentDataPath
        /// is only valid on the main thread and differs between the Editor and
        /// a player, so callers get a fresh value each time.
        /// </summary>
        public static string DefaultSavesDir =>
            Path.Combine(Application.persistentDataPath, SavesFolderName);

        /// <summary>File name (no directory) for a slot, e.g. "save_autosave.json".</summary>
        public static string SlotFileName(string slotId) => FilePrefix + slotId + FileExtension;

        /// <summary>Full path to a slot's save file inside <paramref name="savesDir"/>.</summary>
        public static string SlotPath(string savesDir, string slotId) =>
            Path.Combine(savesDir, SlotFileName(slotId));

        /// <summary>Full path to a slot's rotating backup (written by the atomic save).</summary>
        public static string BakPath(string savesDir, string slotId) =>
            SlotPath(savesDir, slotId) + BackupExtension;

        /// <summary>
        /// Recover the slot id from a save file name, or null if the name does
        /// not match the convention. Inverse of <see cref="SlotFileName"/>.
        /// </summary>
        public static string SlotIdFromFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            if (!fileName.StartsWith(FilePrefix, StringComparison.Ordinal)) return null;
            if (!fileName.EndsWith(FileExtension, StringComparison.Ordinal)) return null;

            int length = fileName.Length - FilePrefix.Length - FileExtension.Length;
            return length > 0 ? fileName.Substring(FilePrefix.Length, length) : null;
        }

        /// <summary>
        /// Of the given candidate slots, return the one whose save file was
        /// written most recently, or null if none of them exist.
        ///
        /// Used by the main menu to decide what "Continue" resumes: the player
        /// means "the game I was last playing", which is whichever of the
        /// autosave / quicksave slots is newer -- not a fixed preference order.
        /// Unreadable timestamps are treated as "oldest" rather than throwing,
        /// so a permissions problem on one slot cannot hide a healthy one.
        /// </summary>
        public static string NewestExistingSlot(string savesDir, params string[] candidateSlotIds)
        {
            if (string.IsNullOrEmpty(savesDir) || candidateSlotIds == null) return null;
            if (!Directory.Exists(savesDir)) return null;

            string newestSlot = null;
            DateTime newestWrite = DateTime.MinValue;

            foreach (string slotId in candidateSlotIds)
            {
                if (string.IsNullOrEmpty(slotId)) continue;

                string path = SlotPath(savesDir, slotId);
                if (!File.Exists(path)) continue;

                DateTime written;
                try
                {
                    written = File.GetLastWriteTimeUtc(path);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(
                        $"[SaveSlotPaths] Could not read write time for '{path}': {ex.Message}. " +
                        "Treating it as the oldest candidate.");
                    written = DateTime.MinValue;
                }

                // Strict > keeps the earlier candidate on an exact tie, which
                // makes the result deterministic for callers that pass a
                // meaningful preference order.
                if (newestSlot == null || written > newestWrite)
                {
                    newestSlot = slotId;
                    newestWrite = written;
                }
            }

            return newestSlot;
        }
    }
}
