using System;
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host factory for Core <see cref="SaveStore{T}"/> instances — the single
    /// place where the Godot-side ports (FileSystemIO, SystemTextJsonSerializer,
    /// GodotLog) and the <see cref="SaveSlotRoot"/> base-directory router are
    /// injected. Save-store façades call
    /// <see cref="Checksummed{T}"/> (envelope flavor) or
    /// <see cref="FromCodec{T}"/> (codec flavor) and never touch file IO,
    /// serializers, checksums, or path construction themselves.
    ///
    /// The base directory is resolved per operation through
    /// SaveSlotRoot.ResolveBaseDirectory, so slot-root switches and the
    /// ASHFALL_USER_DIR override apply without rebuilding stores.
    /// </summary>
    public static class SaveStoreHub
    {
        /// <summary>
        /// Checksummed-envelope store: writes the canonical
        /// <c>{ State, Checksum }</c> JSON for the section file name, with
        /// atomic replacement and optional .bak rotation. Pass
        /// <paramref name="allowLegacyBareState"/> false for sections that
        /// deliberately dropped their pre-checksum bare-state format.
        /// </summary>
        public static SaveStore<T> Checksummed<T>(
            string fileName,
            string logTag,
            bool createBackup = false,
            bool allowLegacyBareState = true)
            where T : class
        {
            return new SaveStore<T>(
                fileName,
                new FileSystemIO(),
                new SystemTextJsonSerializer(),
                new GodotLog(),
                SaveSlotRoot.ResolveBaseDirectory,
                logTag,
                createBackup,
                allowLegacyBareState);
        }

        /// <summary>
        /// Codec-delegating store: serialization (checksum stamping and
        /// versioned migration) is owned by the Core save codec supplied as
        /// encode/decode delegates; the store adds path resolution, atomic
        /// write, and error handling.
        /// </summary>
        public static SaveStore<T> FromCodec<T>(
            string fileName,
            string logTag,
            Func<T, IJsonSerializer, string> encode,
            Func<string, IJsonSerializer, T?> decode,
            bool createBackup = false)
            where T : class
        {
            return SaveStore<T>.FromCodec(
                fileName,
                new FileSystemIO(),
                new SystemTextJsonSerializer(),
                new GodotLog(),
                SaveSlotRoot.ResolveBaseDirectory,
                logTag,
                encode,
                decode,
                createBackup);
        }
    }
}
