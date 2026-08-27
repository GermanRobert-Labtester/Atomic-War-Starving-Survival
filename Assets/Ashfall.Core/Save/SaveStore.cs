using System;
using System.IO;

namespace Ashfall.Core.Save
{
    /// <summary>
    /// Generic, port-injected persistence service for one save section.
    ///
    /// Replaces the per-store boilerplate previously copy-pasted across the
    /// host save stores: path resolution, checksum envelope, atomic write,
    /// backup rotation, legacy bare-state loading, and error logging all live
    /// here (delegating to <see cref="SaveEnvelopeHelper"/> for the envelope
    /// mechanics). Host stores become thin façades that preserve their public
    /// static signatures and delegate here.
    ///
    /// Two flavors:
    /// <list type="bullet">
    /// <item>Checksummed (default) — wraps state in the canonical
    /// <c>{ State, Checksum }</c> envelope via <see cref="SaveChecksum"/>.</item>
    /// <item>Codec (via <see cref="FromCodec"/>) — delegates serialization to
    /// a Core save codec that owns checksum stamping and versioned migration
    /// itself (e.g. HoldfastSaveCodec).</item>
    /// </list>
    ///
    /// The base directory is resolved through an injected provider on every
    /// operation, so a slot-root switch mid-session takes effect on the next
    /// save/load without rebuilding stores.
    /// </summary>
    public sealed class SaveStore<T> where T : class
    {
        private readonly string _fileName;
        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;
        private readonly ILog _log;
        private readonly Func<string> _baseDirProvider;
        private readonly string _logTag;
        private readonly bool _createBackup;
        private readonly bool _allowLegacyBareState;
        private readonly Func<T, IJsonSerializer, string>? _encode;
        private readonly Func<string, IJsonSerializer, T?>? _decode;

        /// <summary>
        /// Checksummed-envelope store. Writes <c>{ State, Checksum }</c> JSON
        /// identical to the canonical per-store envelope pattern.
        /// <paramref name="allowLegacyBareState"/> keeps (default) or drops the
        /// fallback to parsing a pre-checksum bare-state file; sections that
        /// deliberately abandoned their pre-envelope format pass false.
        /// </summary>
        public SaveStore(
            string fileName,
            IFileIO files,
            IJsonSerializer json,
            ILog log,
            Func<string> baseDirProvider,
            string logTag,
            bool createBackup = false,
            bool allowLegacyBareState = true)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name must not be null or whitespace.", nameof(fileName));
            _fileName = fileName;
            _files = files ?? throw new ArgumentNullException(nameof(files));
            _json = json ?? throw new ArgumentNullException(nameof(json));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _baseDirProvider = baseDirProvider ?? throw new ArgumentNullException(nameof(baseDirProvider));
            _logTag = string.IsNullOrEmpty(logTag) ? typeof(SaveStore<>).Name : logTag;
            _createBackup = createBackup;
            _allowLegacyBareState = allowLegacyBareState;
        }

        private SaveStore(
            string fileName,
            IFileIO files,
            IJsonSerializer json,
            ILog log,
            Func<string> baseDirProvider,
            string logTag,
            bool createBackup,
            Func<T, IJsonSerializer, string> encode,
            Func<string, IJsonSerializer, T?> decode)
            : this(fileName, files, json, log, baseDirProvider, logTag, createBackup)
        {
            _encode = encode ?? throw new ArgumentNullException(nameof(encode));
            _decode = decode ?? throw new ArgumentNullException(nameof(decode));
        }

        /// <summary>
        /// Codec-delegating store: serialization is owned by a Core save codec
        /// (checksum stamping and versioned migration happen inside it); this
        /// store adds path resolution, atomic write, and error handling.
        /// </summary>
        public static SaveStore<T> FromCodec(
            string fileName,
            IFileIO files,
            IJsonSerializer json,
            ILog log,
            Func<string> baseDirProvider,
            string logTag,
            Func<T, IJsonSerializer, string> encode,
            Func<string, IJsonSerializer, T?> decode,
            bool createBackup = false)
        {
            return new SaveStore<T>(fileName, files, json, log, baseDirProvider, logTag, createBackup, encode, decode);
        }

        /// <summary>Section file name (e.g. "weather_save.json").</summary>
        public string FileName => _fileName;

        /// <summary>
        /// Creates a new SaveStore for the same section with a custom base directory provider.
        /// </summary>
        public SaveStore<T> WithBaseDirectory(Func<string> baseDirProvider)
        {
            if (baseDirProvider == null) throw new ArgumentNullException(nameof(baseDirProvider));
            if (_encode != null && _decode != null)
                return new SaveStore<T>(_fileName, _files, _json, _log, baseDirProvider, _logTag, _createBackup, _encode, _decode);
            return new SaveStore<T>(_fileName, _files, _json, _log, baseDirProvider, _logTag, _createBackup, _allowLegacyBareState);
        }

        /// <summary>
        /// Full save path. The base directory provider is evaluated on every
        /// access so slot-root changes apply to the next operation.
        /// </summary>
        public string SavePath => Path.Combine(_baseDirProvider(), _fileName);

        /// <summary>Backup path used when backup rotation is enabled.</summary>
        public string BackupPath => SavePath + ".bak";

        /// <summary>Whether a save file exists for this section.</summary>
        public bool Exists() => _files.FileExists(SavePath);

        /// <summary>
        /// Persist state. Checksummed flavor wraps it in the integrity
        /// envelope; codec flavor delegates to the codec's Encode. Writes are
        /// atomic (temp file + rename) and optionally rotate a .bak first.
        /// Returns false (and logs) on failure — never throws.
        /// </summary>
        public bool TrySave(T state, string? pathOverride = null)
        {
            if (state == null) return false;
            string path = pathOverride ?? SavePath;

            if (_encode != null)
            {
                try
                {
                    string payload = _encode(state, _json);
                    return SaveEnvelopeHelper.TryWriteAtomic(path, payload, _files, _createBackup, _log, _logTag);
                }
                catch (Exception e)
                {
                    _log.Error($"[{_logTag}] save failed: " + e.Message);
                    return false;
                }
            }

            return SaveEnvelopeHelper.TrySaveAtomic(path, state, _json, _files, _createBackup, _log, _logTag);
        }

        /// <summary>
        /// Load state. Checksummed flavor validates the envelope (a new-format
        /// save with a missing/empty checksum is rejected as corrupt, never
        /// silently trusted) and falls back to legacy bare-state parsing for
        /// pre-checksum saves. Codec flavor delegates to the codec's Decode,
        /// which owns versioned migration. Returns null on any failure —
        /// never throws.
        /// </summary>
        public T? TryLoad(string? pathOverride = null)
        {
            string path = pathOverride ?? SavePath;

            if (_decode != null)
            {
                try
                {
                    if (!_files.FileExists(path)) return null;
                    string raw = _files.ReadAllText(path);
                    if (string.IsNullOrWhiteSpace(raw)) return null;
                    return _decode(raw, _json);
                }
                catch (Exception e)
                {
                    _log.Error($"[{_logTag}] load failed: " + e.Message);
                    return null;
                }
            }

            // Missing and empty files are normal "no save yet" states and stay
            // silent, matching the per-store behavior this replaces; every
            // real integrity failure is logged once with the store's own tag.
            try
            {
                if (!_files.FileExists(path)) return null;
                string raw = _files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var (ok, state, error) = SaveEnvelopeHelper.RestoreEnvelope<T>(raw, _json, null, _allowLegacyBareState);
                if (!ok && !string.IsNullOrEmpty(error))
                    _log.Error($"[{_logTag}] load failed: " + error);
                return ok ? state : null;
            }
            catch (Exception e)
            {
                _log.Error($"[{_logTag}] load failed: " + e.Message);
                return null;
            }
        }

        /// <summary>Serialize bare state (no envelope) — the aggregate campaign
        /// envelope path packs section payloads this way. Returns an empty
        /// string on failure.
        /// </summary>
        public string CaptureBare(T state)
        {
            try
            {
                if (state == null) return string.Empty;
                return _json.Serialize(state);
            }
            catch (Exception e)
            {
                _log.Error($"[{_logTag}] capture failed: " + e.Message);
                return string.Empty;
            }
        }

        /// <summary>Serialize state through the codec's Encode without touching
        /// disk — for sections whose aggregate capture is codec-shaped rather
        /// than bare. Codec flavor only. Returns an empty string on failure.
        /// </summary>
        public string CaptureEncoded(T state)
        {
            if (_encode == null)
                throw new InvalidOperationException("CaptureEncoded requires the codec flavor of SaveStore<T>.");
            try
            {
                if (state == null) return string.Empty;
                return _encode(state, _json);
            }
            catch (Exception e)
            {
                _log.Error($"[{_logTag}] capture failed: " + e.Message);
                return string.Empty;
            }
        }

        /// <summary>Restore state through the codec's Decode without touching
        /// disk. Codec flavor only. Null on any failure.</summary>
        public T? RestoreEncoded(string json)
        {
            if (_decode == null)
                throw new InvalidOperationException("RestoreEncoded requires the codec flavor of SaveStore<T>.");
            try
            {
                if (string.IsNullOrWhiteSpace(json)) return null;
                return _decode(json, _json);
            }
            catch (Exception e)
            {
                _log.Error($"[{_logTag}] restore failed: " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Serialize state into EXACTLY the bytes <see cref="TrySave"/> would
        /// write to disk — the checksummed envelope for the default flavor,
        /// the codec's JSON for the codec flavor. This is what the campaign
        /// envelope packs as a section payload, so a section's bytes are
        /// identical whether they arrived via a file or via the envelope.
        /// Returns an empty string on failure.
        /// </summary>
        public string CapturePersisted(T state) => _encode != null ? CaptureEncoded(state) : CaptureEnvelope(state);

        /// <summary>Deserialize bare state written by <see cref="CaptureBare"/>. Null on failure.</summary>
        public T? RestoreBare(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json)) return null;
                return _json.Deserialize<T>(json);
            }
            catch (Exception e)
            {
                _log.Error($"[{_logTag}] restore failed: " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Serialize state inside a checksummed envelope without touching
        /// disk. Returns an empty string on failure.
        /// </summary>
        public string CaptureEnvelope(T state) => SaveEnvelopeHelper.CaptureEnvelope(state, _json);

        /// <summary>
        /// Restore state from a checksummed envelope string without touching
        /// disk. Null on any integrity failure.
        /// </summary>
        public T? RestoreEnvelope(string json)
        {
            var (ok, state, _) = SaveEnvelopeHelper.RestoreEnvelope<T>(json, _json);
            return ok ? state : null;
        }
    }
}
