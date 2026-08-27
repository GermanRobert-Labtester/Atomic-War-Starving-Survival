// SPDX-License-Identifier: MIT
// ASHFALL Core: unified catalog load result with diagnostics.
//
// Generic result type for catalog loading operations. Carries the loaded data,
// metadata about the source, and structured error/warning information so callers
// can distinguish between:
//   - Success (data loaded, no errors)
//   - Success with warnings (data loaded but with non-fatal issues)
//   - Soft failure (no data loaded but no fatal errors - e.g., optional file missing)
//   - Hard failure (fatal errors that prevent the game from starting)

using System;
using System.Collections.Generic;

namespace Ashfall.Core.IO
{
    /// <summary>
    /// Classification of a catalog for startup behavior.
    /// Required: game cannot start without this catalog. Errors are fatal.
    /// Optional: expansion or non-critical content. Missing is OK, errors are warnings.
    /// DeveloperOnly: internal tools, test data. Missing/errors are silent.
    /// </summary>
    public enum CatalogClassification
    {
        /// <summary>Game cannot start without this catalog. Load errors are fatal.</summary>
        Required = 0,
        /// <summary>Expansion or non-critical content. Load errors are warnings.</summary>
        Optional = 1,
        /// <summary>Internal tools, test data, editor-only. Load errors are silent.</summary>
        DeveloperOnly = 2
    }

    /// <summary>
    /// Severity level for catalog load diagnostics.
    /// </summary>
    public enum CatalogLoadSeverity
    {
        /// <summary>Informational message - not an issue.</summary>
        Info = 0,
        /// <summary>Warning - data loaded but with issues.</summary>
        Warning = 1,
        /// <summary>Error - data could not be loaded or is malformed.</summary>
        Error = 2,
        /// <summary>Fatal - game cannot continue.</summary>
        Fatal = 3
    }

    /// <summary>
    /// Single diagnostic message from a catalog load operation.
    /// </summary>
    public readonly struct CatalogLoadMessage
    {
        public CatalogLoadSeverity Severity { get; }
        public string FilePath { get; }
        public string Shape { get; }
        public string Message { get; }
        public Exception? Exception { get; }

        public CatalogLoadMessage(
            CatalogLoadSeverity severity,
            string filePath,
            string shape,
            string message,
            Exception? exception = null)
        {
            Severity = severity;
            FilePath = filePath ?? "<unknown>";
            Shape = shape ?? "<unknown>";
            Message = message ?? "<no message>";
            Exception = exception;
        }

        public override string ToString() =>
            $"[{Severity}] {FilePath} ({Shape}): {Message}" +
            (Exception != null ? $"\n  Exception: {Exception.Message}" : string.Empty);
    }

    /// <summary>
    /// Result of loading a catalog: carries the loaded entries, source metadata,
    /// and any diagnostic messages (warnings, errors).
    /// </summary>
    /// <typeparam name="T">The entry type in the catalog (e.g., GoodDefinition, ItemDefinition).</typeparam>
    public class CatalogLoadResult<T>
    {
        private readonly List<T> _entries = new List<T>();
        private readonly List<CatalogLoadMessage> _messages = new List<CatalogLoadMessage>();

        /// <summary>The absolute or relative path to the catalog source file.</summary>
        public string FilePath { get; private set; } = "<unknown>";

        /// <summary>The JSON schema or shape that was attempted (e.g., "GoodsCatalogRoot", "array").</summary>
        public string Schema { get; private set; } = "<unknown>";

        /// <summary>The schema version from the catalog file, if available.</summary>
        public int SchemaVersion { get; private set; }

        /// <summary>Number of entries successfully loaded.</summary>
        public int EntryCount => _entries.Count;

        /// <summary>All loaded entries.</summary>
        public IReadOnlyList<T> Entries => _entries.AsReadOnly();

        /// <summary>Classification of this catalog (required, optional, developer-only).</summary>
        public CatalogClassification Classification { get; private set; } = CatalogClassification.Optional;

        /// <summary>All diagnostic messages from the load operation.</summary>
        public IReadOnlyList<CatalogLoadMessage> Messages => _messages.AsReadOnly();

        /// <summary>True if any fatal errors were encountered.</summary>
        public bool HasFatalErrors => HasMessagesOfSeverity(CatalogLoadSeverity.Fatal);

        /// <summary>True if any errors (fatal or non-fatal) were encountered.</summary>
        public bool HasErrors => HasMessagesOfSeverity(CatalogLoadSeverity.Error) || HasFatalErrors;

        /// <summary>True if any warnings were encountered.</summary>
        public bool HasWarnings => HasMessagesOfSeverity(CatalogLoadSeverity.Warning);

        /// <summary>True if the load was completely successful with no messages.</summary>
        public bool IsSuccess => !HasErrors && !HasWarnings;

        private bool HasMessagesOfSeverity(CatalogLoadSeverity severity)
        {
            for (int i = 0; i < _messages.Count; i++)
            {
                if (_messages[i].Severity >= severity)
                    return true;
            }
            return false;
        }

        /// <summary>Create a new empty result with the specified file path.</summary>
        public CatalogLoadResult(string filePath, string schema, CatalogClassification classification = CatalogClassification.Optional)
        {
            FilePath = filePath ?? "<unknown>";
            Schema = schema ?? "<unknown>";
            Classification = classification;
        }

        /// <summary>Create a successful result with the loaded entries.</summary>
        public static CatalogLoadResult<T> Success(
            string filePath,
            string schema,
            IReadOnlyList<T> entries,
            int schemaVersion = 1,
            CatalogClassification classification = CatalogClassification.Optional)
        {
            var result = new CatalogLoadResult<T>(filePath, schema, classification)
            {
                SchemaVersion = schemaVersion
            };
            result._entries.AddRange(entries);
            return result;
        }

        /// <summary>Create a failed result with the error message.</summary>
        public static CatalogLoadResult<T> Fail(
            string filePath,
            string schema,
            string message,
            Exception? exception = null,
            CatalogClassification classification = CatalogClassification.Optional)
        {
            var result = new CatalogLoadResult<T>(filePath, schema, classification);
            var severity = classification == CatalogClassification.Required
                ? CatalogLoadSeverity.Fatal
                : CatalogLoadSeverity.Error;
            result.AddMessage(severity, filePath, schema, message, exception);
            return result;
        }

        /// <summary>Add an entry to the result.</summary>
        public void AddEntry(T entry) => _entries.Add(entry);

        /// <summary>Add entries to the result.</summary>
        public void AddEntries(IEnumerable<T> entries)
        {
            if (entries != null)
                _entries.AddRange(entries);
        }

        /// <summary>Add a diagnostic message.</summary>
        public void AddMessage(
            CatalogLoadSeverity severity,
            string filePath,
            string shape,
            string message,
            Exception? exception = null)
        {
            _messages.Add(new CatalogLoadMessage(severity, filePath, shape, message, exception));
            // Also emit to CatalogDiagnostics for backward compatibility
            if (severity >= CatalogLoadSeverity.Warning)
            {
                CatalogDiagnostics.Warn(filePath, shape, exception ?? new Exception(message));
            }
        }

        /// <summary>Add an info-level message.</summary>
        public void AddInfo(string message) =>
            AddMessage(CatalogLoadSeverity.Info, FilePath, Schema, message);

        /// <summary>Add a warning-level message.</summary>
        public void AddWarning(string message, Exception? ex = null) =>
            AddMessage(CatalogLoadSeverity.Warning, FilePath, Schema, message, ex);

        /// <summary>Add an error-level message.</summary>
        public void AddError(string message, Exception? ex = null) =>
            AddMessage(CatalogLoadSeverity.Error, FilePath, Schema, message, ex);

        /// <summary>Add a fatal-level message (for required catalogs).</summary>
        public void AddFatal(string message, Exception? ex = null) =>
            AddMessage(CatalogLoadSeverity.Fatal, FilePath, Schema, message, ex);

        /// <summary>Set the schema version after parsing.</summary>
        public void SetSchemaVersion(int version) => SchemaVersion = version;

        /// <summary>
        /// Throw if this result contains fatal errors and the catalog is required.
        /// Used at startup to prevent game from starting with missing required data.
        /// </summary>
        public void ThrowIfFatal()
        {
            if (HasFatalErrors)
            {
                var fatalMessages = new System.Text.StringBuilder();
                fatalMessages.AppendLine("Fatal errors loading required catalog:");
                for (int i = 0; i < _messages.Count; i++)
                {
                    if (_messages[i].Severity >= CatalogLoadSeverity.Fatal)
                    {
                        fatalMessages.AppendLine($"  [{_messages[i].Severity}] {_messages[i].Message}");
                    }
                }
                throw new InvalidOperationException(fatalMessages.ToString());
            }
        }

        // ── Static helpers for common loader patterns ──────────────────────────

        /// <summary>
        /// Create a result from a file that may or may not exist. If the file doesn't
        /// exist, returns an empty result with the appropriate classification.
        /// </summary>
        public static CatalogLoadResult<T> FromFile(
            string filePath,
            string schema,
            CatalogClassification classification,
            Func<string, IJsonSerializer, T> deserializer,
            IJsonSerializer json)
        {
            var result = new CatalogLoadResult<T>(filePath, schema, classification);

            IFileIO fileIO = new FileSystemIO();
            if (!fileIO.FileExists(filePath))
            {
                if (classification == CatalogClassification.Required)
                {
                    result.AddFatal("Required catalog file not found: " + filePath);
                }
                else
                {
                    result.AddInfo("Optional catalog file not found (ok): " + filePath);
                }
                return result;
            }

            try
            {
                string raw = fileIO.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(raw))
                {
                    result.AddWarning("Catalog file is empty: " + filePath);
                    return result;
                }

                var data = deserializer(raw, json);
                if (data is System.Collections.IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                        result.AddEntry((T)item);
                }
                else
                {
                    result.AddEntry(data);
                }
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(schema, filePath, ex);
                var severity = classification == CatalogClassification.Required
                    ? CatalogLoadSeverity.Fatal
                    : CatalogLoadSeverity.Error;
                result.AddMessage(severity, filePath, schema, "Failed to load: " + ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// Create a result from a wrapped list file (common pattern: {"schema_version": N, "items": [...]})
        /// </summary>
        public static CatalogLoadResult<T> FromWrappedListFile(
            string filePath,
            string schema,
            CatalogClassification classification,
            IJsonSerializer json)
        {
            var result = new CatalogLoadResult<T>(filePath, schema, classification);

            IFileIO fileIO = new FileSystemIO();
            if (!fileIO.FileExists(filePath))
            {
                if (classification == CatalogClassification.Required)
                {
                    result.AddFatal("Required catalog file not found: " + filePath);
                }
                else
                {
                    result.AddInfo("Optional catalog file not found (ok): " + filePath);
                }
                return result;
            }

            try
            {
                string raw = fileIO.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(raw))
                {
                    result.AddWarning("Catalog file is empty: " + filePath);
                    return result;
                }

                var list = CatalogLocator.LoadWrappedList<T>(raw, SystemTextJsonSerializer.Options);
                if (list != null)
                {
                    result.AddEntries(list);
                }
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(schema, filePath, ex);
                var severity = classification == CatalogClassification.Required
                    ? CatalogLoadSeverity.Fatal
                    : CatalogLoadSeverity.Error;
                result.AddMessage(severity, filePath, schema, "Failed to load wrapped list: " + ex.Message, ex);
            }

            return result;
        }
    }
}
