using System;
using System.IO;

namespace Ashfall.Core.Save
{
    /// <summary>
    /// Generic checksummed envelope matching the canonical save store pattern:
    /// { State, Checksum } where State holds the domain DTO.
    /// </summary>
    [Serializable]
    public class SaveEnvelope<T>
    {
        public T? State;
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Shared helper for save stores: handles atomic file writing (.tmp -> rename),
    /// optional backup generation (.bak), checksum calculation & verification,
    /// and graceful legacy bare-state loading.
    /// </summary>
    public static class SaveEnvelopeHelper
    {
        private static readonly IJsonSerializer DefaultSerializer = new SystemTextJsonSerializer();
        private static readonly IFileIO DefaultFileIO = new FileSystemIO();

        /// <summary>
        /// Saves state wrapped in a checksum envelope with atomic file replacement and optional backup.
        /// </summary>
        public static bool TrySaveAtomic<T>(
            string path,
            T state,
            IJsonSerializer? serializer = null,
            IFileIO? fileIO = null,
            bool createBackup = false,
            ILog? log = null,
            string? logTag = null)
        {
            if (state == null) return false;
            if (string.IsNullOrWhiteSpace(path)) return false;

            var json = serializer ?? DefaultSerializer;
            string tag = logTag ?? "SaveEnvelopeHelper";

            try
            {
                var envelope = new SaveEnvelope<T>
                {
                    State = state
                };
                envelope.Checksum = SaveChecksum.Compute(envelope);

                string serialized = json.Serialize(envelope);
                return TryWriteAtomic(path, serialized, fileIO, createBackup, log, tag);
            }
            catch (Exception ex)
            {
                log?.Error($"[{tag}] Save failed for '{path}': {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Writes an already-serialized payload atomically: temp file, optional
        /// backup of the current file, then rename over the target. Shared by
        /// the checksummed envelope path and codec-delegating save stores.
        /// </summary>
        public static bool TryWriteAtomic(
            string path,
            string payload,
            IFileIO? fileIO = null,
            bool createBackup = false,
            ILog? log = null,
            string? logTag = null)
        {
            if (string.IsNullOrWhiteSpace(path)) return false;

            var files = fileIO ?? DefaultFileIO;
            string tag = logTag ?? "SaveEnvelopeHelper";

            try
            {
                string dir = Path.GetDirectoryName(path)!;
                if (!string.IsNullOrEmpty(dir) && !files.DirectoryExists(dir))
                {
                    if (files is FileSystemIO)
                        Directory.CreateDirectory(dir);
                }

                // Create backup if requested and original exists
                if (createBackup && files.FileExists(path))
                {
                    try
                    {
                        string bakPath = path + ".bak";
                        string current = files.ReadAllText(path);
                        files.WriteAllText(bakPath, current);
                    }
                    catch (Exception bakEx)
                    {
                        log?.Warn($"[{tag}] Backup copy failed for '{path}': {bakEx.Message}");
                    }
                }

                // Atomic write via temp file
                string tempPath = path + ".tmp";
                files.WriteAllText(tempPath, payload);

                if (File.Exists(tempPath))
                {
                    File.Move(tempPath, path, overwrite: true);
                }
                else
                {
                    files.WriteAllText(path, payload);
                }

                return true;
            }
            catch (Exception ex)
            {
                log?.Error($"[{tag}] Save failed for '{path}': {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Loads state from a checksummed envelope file, validating integrity.
        /// Falls back to legacy bare-state decoding or custom fallback if pre-checksum save is found.
        /// </summary>
        public static (bool Success, T? State, string? ErrorMessage) TryLoad<T>(
            string path,
            IJsonSerializer? serializer = null,
            IFileIO? fileIO = null,
            Func<string, T?>? legacyFallback = null,
            ILog? log = null,
            string? logTag = null) where T : class
        {
            if (string.IsNullOrWhiteSpace(path))
                return (false, null, "Path is empty or null.");

            var files = fileIO ?? DefaultFileIO;
            if (!files.FileExists(path))
                return (false, null, $"Save file not found at '{path}'.");

            var json = serializer ?? DefaultSerializer;
            string tag = logTag ?? "SaveEnvelopeHelper";

            try
            {
                string raw = files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw))
                    return (false, null, "Save file is empty.");

                // 1. Try envelope deserialization
                SaveEnvelope<T>? envelope = null;
                try
                {
                    envelope = json.Deserialize<SaveEnvelope<T>>(raw);
                }
                catch
                {
                    // Not in standard generic envelope format, try legacy fallback
                }

                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum))
                    {
                        string err = "Checksum field missing in save envelope (corrupt save).";
                        log?.Error($"[{tag}] {err} at '{path}'");
                        return (false, null, err);
                    }

                    string computed = SaveChecksum.Compute(envelope);
                    if (!string.Equals(envelope.Checksum, computed, StringComparison.Ordinal))
                    {
                        string err = "Checksum mismatch in save envelope (corrupt or tampered save).";
                        log?.Error($"[{tag}] {err} at '{path}'");
                        return (false, null, err);
                    }

                    return (true, envelope.State, null);
                }

                // 2. Legacy fallback
                if (legacyFallback != null)
                {
                    var fallbackState = legacyFallback(raw);
                    if (fallbackState != null)
                        return (true, fallbackState, null);
                }

                // 3. Direct raw bare state fallback
                try
                {
                    var rawState = json.Deserialize<T>(raw);
                    if (rawState != null)
                        return (true, rawState, null);
                }
                catch
                {
                    // Deserialization failure
                }

                return (false, null, "Failed to deserialize save payload.");
            }
            catch (Exception ex)
            {
                log?.Error($"[{tag}] Load failed for '{path}': {ex.Message}");
                return (false, null, ex.Message);
            }
        }

        /// <summary>
        /// Captures state into a standalone JSON envelope string without disk I/O.
        /// </summary>
        public static string CaptureEnvelope<T>(T state, IJsonSerializer? serializer = null)
        {
            if (state == null) return string.Empty;
            var json = serializer ?? DefaultSerializer;
            var envelope = new SaveEnvelope<T> { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);
            return json.Serialize(envelope);
        }

        /// <summary>
        /// Restores state from a standalone JSON envelope string without disk I/O.
        /// </summary>
        public static (bool Success, T? State, string? ErrorMessage) RestoreEnvelope<T>(
            string jsonString,
            IJsonSerializer? serializer = null,
            Func<string, T?>? legacyFallback = null,
            bool allowBareFallback = true) where T : class
        {
            if (string.IsNullOrWhiteSpace(jsonString))
                return (false, null, "JSON string is empty.");

            var json = serializer ?? DefaultSerializer;

            try
            {
                var envelope = json.Deserialize<SaveEnvelope<T>>(jsonString);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum))
                        return (false, null, "Checksum field missing in save envelope.");

                    string computed = SaveChecksum.Compute(envelope);
                    if (!string.Equals(envelope.Checksum, computed, StringComparison.Ordinal))
                        return (false, null, "Checksum mismatch in save envelope.");

                    return (true, envelope.State, null);
                }

                if (legacyFallback != null)
                {
                    var fallback = legacyFallback(jsonString);
                    if (fallback != null)
                        return (true, fallback, null);
                }

                // Some sections deliberately dropped their pre-checksum format
                // instead of migrating it; those stores disable this fallback.
                if (allowBareFallback)
                {
                    var raw = json.Deserialize<T>(jsonString);
                    if (raw != null)
                        return (true, raw, null);
                }

                return (false, null, "Failed to deserialize envelope.");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
    }
}
