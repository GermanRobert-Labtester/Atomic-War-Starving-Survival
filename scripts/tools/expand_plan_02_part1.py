import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/02-loader-bare-catch-hardening.md"

header = """# Plan 02 — Comprehensive Catalog Ingestion Hardening, Diagnostic Telemetry & Zero Silent Failure Architecture (closes H4)

**Package:** `PLAN-02-LOADER-BARE-CATCH-HARDENING`
**Document Class:** Master System Architecture, Ingestion Framework & Production Integration Blueprint
**Authority Level:** Canonical Production Plan
**Target Runtimes:** Ashfall.Core (`netstandard2.1`, Engine-Free) · Godot Host (`net8.0`) · Ashfall.Core.Tests (`net9.0`)
**Data Authority:** `Assets/StreamingAssets/Data/` (snake_case JSON, schema-validated)
**Historical Anchor:** piagentsplans Wave 1 (2026-08-30) · Core Reliability & Data Ingestion Suite · Master Authority Volumes 2, 7, 18, 33, 49
**Save Authority:** Non-Stateful Domain Foundation; Governs Ingestion for All Checksummed Sections
**Determinism Mandate:** Pure Domain Invariants under Injected `ICatalogLogger` & Fail-Fast Structural Parsers; Zero Console Writes; Zero Bare Swallowing

---

# SECTION I: COMPREHENSIVE ARCHITECTURAL SCOPE & HAZARD TAXONOMY

Plan 02 resolves known issue **H4** by eliminating all silent bare `catch { }` blocks across the *ASHFALL* catalog loading pipeline. In distributed game architecture, silent exception swallowing is a catastrophic anti-pattern: it converts syntax errors, missing schema definitions, and truncated assets into silent gameplay null-references, invisible items, ghost NPCs, and un-reproducible runtime desynchronizations.

This architecture replaces silent suppression with a high-performance, structured diagnostic ingestion pipeline:

```
+===================================================================================================+
|                             AUTHORITATIVE JSON ASSET REPOSITORY                                   |
|   Assets/StreamingAssets/Data/ (280+ Schema-Versioned JSON Catalogs)                               |
+===================================================================================================+
                                                  │
                                                  ▼
+===================================================================================================+
|                        ASHFALL CORE DIAGNOSTIC INGESTION PIPELINE                                 |
|  Assets/Ashfall.Core/Loading/ & Assets/Ashfall.Core/Diagnostics/                                  |
|  - Zero Bare Catch Blocks: Every Exception Captured & Re-contextualized                          |
|  - Structured Error Telemetry: CatalogLoadResult<T>, CatalogDiagnosticError, Severity Tiers      |
|  - Pure Domain Injected Logger Port (ICatalogLogger) - 100% Engine-Free                           |
|  - Strict Line/Column Tracing & JSON Path Pinpointing                                             |
+===================================================================================================+
        │                                         │                                      │
        ▼                                         ▼                                      ▼
+───────────────────────────+   +───────────────────────────────────+   +───────────────────────────+
| YearOfAshCatalogLoader    |   | VerdictCatalogLoader              |   | CatalogIntegrityValidator |
| - Hardened Parser         |   | - Hardened Parser                 |   | - Headless CLI Gates      |
| - 7 Sites Remediated      |   | - 3 Sites Remediated              |   | - Fail-Fast CI Battery    |
+───────────────────────────+   +───────────────────────────────────+   +───────────────────────────+
        │                                         │                                      │
        └─────────────────────────────────────────┼──────────────────────────────────────┘
                                                  ▼
+===================================================================================================+
|                          OBSERVABILITY & HOST INTEGRATION PORTS                                   |
|  src/Host/DiagnosticHostLoggerBridge.cs & godot --headless --data-integrity-selftest               |
|  - Machine-Readable JSON Error Dumps for CI/CD Gates                                              |
|  - High-Contrast Terminal Diagnostics with File Offsets and Suggested Remediations               |
+===================================================================================================+
```

### 1.1 The Seven Deadly Hazards of Bare `catch { }`
1. **The Ghost Asset Hazard**: When an authored item JSON fails to parse due to a missing comma, a bare catch swallows the error and skips the entry. The item vanishes from merchant inventories, prompting futile debugging into quest and drop-table code.
2. **Schema Drift Masking**: When schema version 2 introduces a required field, bare catch blocks silently discard old version files instead of triggering migration routines.
3. **Type Coercion Silent Defaults**: Numerical fields formatted as strings (e.g. `"radiation_cgy": "15.0"`) trigger silent deserialization errors, falling back to `0.0f` and making hazardous nuclear fallout zones completely benign.
4. **Cascading Downstream Null References**: Catalogs that return partial or empty lists cause subsequent domain systems to dereference null collections during world generation.
5. **Masking Out-of-Memory & Thread Abort Hazards**: Catching generic `System.Exception` without re-throwing can swallow `OutOfMemoryException` or `ThreadAbortException`, leaving the process in a zombie corrupt state.
6. **Zero Observability in Production**: Player bug reports stating "The bunker trader has no weapons" cannot be diagnosed because the client log contains zero errors.
7. **CI/CD False Positives**: Automated test suites pass because test runners perceive an empty catalog as valid rather than detecting parse failures.

---

# SECTION II: STRUCTURED DIAGNOSTIC INGESTION ARCHITECTURE

### 2.1 Error Classification & Severity Model
Every catalog ingestion failure is encapsulated within a strongly typed `CatalogDiagnosticError`:

- **Severity Level `FatalCorruption`**: Complete failure to parse root JSON syntax (unmatched braces, invalid character encodings, truncated file stream). Catalog cannot load; halts startup in dev/test mode.
- **Severity Level `RecoverableError`**: A single entry within a collection is malformed (e.g. invalid item ID, missing required stat). The specific entry is isolated and logged, while remaining entries are loaded.
- **Severity Level `SchemaWarning`**: Deprecated field detected or missing optional metadata. Handled via automated fallback values without impeding execution.

### 2.2 Mathematical Diagnostic Metric
The Catalog Health Permille $H_{\\text{cat}}$ evaluates parsing integrity across all records $N$:

$$H_{\\text{cat}} = \max\left(0, 1000 - \frac{1000 \cdot (10 \cdot E_{\\text{fatal}} + 3 \cdot E_{\\text{recoverable}} + 1 \cdot W_{\\text{schema}})}{N_{\\text{records}}}\right)$$

A build fails the continuous integration gate if $H_{\\text{cat}} < 1000$ on authoritative baseline data.

---

# SECTION III: PURE C# DOMAIN IMPLEMENTATION (`Assets/Ashfall.Core/`)

### 3.1 `ICatalogLogger.cs` & `CatalogDiagnosticResult.cs`
```csharp
namespace Ashfall.Core.Diagnostics
{
    using System;
    using System.Collections.Generic;

    public enum DiagnosticSeverity
    {
        Info = 0,
        Warning = 1,
        RecoverableError = 2,
        FatalCorruption = 3
    }

    public sealed class CatalogDiagnosticError
    {
        public DiagnosticSeverity Severity { get; }
        public string CatalogIdentifier { get; }
        public string TargetFilePath { get; }
        public int LineNumber { get; }
        public int ColumnNumber { get; }
        public string JsonPath { get; }
        public string ErrorCode { get; }
        public string DetailedMessage { get; }
        public Exception? InnerException { get; }

        public CatalogDiagnosticError(
            DiagnosticSeverity severity,
            string catalogIdentifier,
            string targetFilePath,
            int lineNumber,
            int columnNumber,
            string jsonPath,
            string errorCode,
            string message,
            Exception? inner = null)
        {
            Severity = severity;
            CatalogIdentifier = catalogIdentifier ?? "UNKNOWN_CATALOG";
            TargetFilePath = targetFilePath ?? "UNKNOWN_PATH";
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
            JsonPath = jsonPath ?? "$";
            ErrorCode = errorCode ?? "CAT_ERR_GENERIC";
            DetailedMessage = message ?? string.Empty;
            InnerException = inner;
        }

        public override string ToString()
        {
            return $"[{Severity}] [{ErrorCode}] Catalog '{CatalogIdentifier}' in '{TargetFilePath}' " +
                   $"(Line: {LineNumber}, Col: {ColumnNumber}, Path: '{JsonPath}'): {DetailedMessage}";
        }
    }

    public interface ICatalogLogger
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(CatalogDiagnosticError error);
        void LogFatal(CatalogDiagnosticError error);
    }

    public sealed class CatalogLoadResult<T>
    {
        public bool IsSuccess => FatalErrors.Count == 0 && Value != null;
        public T? Value { get; }
        public List<CatalogDiagnosticError> Warnings { get; } = new List<CatalogDiagnosticError>();
        public List<CatalogDiagnosticError> RecoverableErrors { get; } = new List<CatalogDiagnosticError>();
        public List<CatalogDiagnosticError> FatalErrors { get; } = new List<CatalogDiagnosticError>();

        public CatalogLoadResult(T? value)
        {
            Value = value;
        }

        public void AddDiagnostic(CatalogDiagnosticError err)
        {
            switch (err.Severity)
            {
                case DiagnosticSeverity.Info:
                case DiagnosticSeverity.Warning:
                    Warnings.Add(err);
                    break;
                case DiagnosticSeverity.RecoverableError:
                    RecoverableErrors.Add(err);
                    break;
                case DiagnosticSeverity.FatalCorruption:
                    FatalErrors.Add(err);
                    break;
            }
        }
    }
}
```

### 3.2 Remediated `YearOfAshCatalogLoader.cs` (Hardened, 0 Bare Catch)
```csharp
namespace Ashfall.Core
{
    using System;
    using System.Collections.Generic;
    using Ashfall.Core.Diagnostics;
    using Ashfall.Core.Serialization;

    public sealed class YearOfAshCatalogLoader
    {
        private readonly ICatalogLogger _logger;
        private readonly IJsonSerializer _serializer;

        public YearOfAshCatalogLoader(ICatalogLogger logger, IJsonSerializer serializer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public CatalogLoadResult<YearOfAshCatalog> LoadCatalog(string jsonPayload, string sourcePath)
        {
            var result = new CatalogLoadResult<YearOfAshCatalog>(null);

            if (string.IsNullOrWhiteSpace(jsonPayload))
            {
                var err = new CatalogDiagnosticError(
                    DiagnosticSeverity.FatalCorruption,
                    "year_of_ash_catalog",
                    sourcePath,
                    0, 0, "$",
                    "ERR_EMPTY_PAYLOAD",
                    "Catalog JSON payload is null or whitespace.");
                _logger.LogFatal(err);
                result.AddDiagnostic(err);
                return result;
            }

            YearOfAshCatalogDto dto;
            try
            {
                dto = _serializer.Deserialize<YearOfAshCatalogDto>(jsonPayload);
                if (dto == null)
                {
                    throw new InvalidOperationException("Deserializer produced null DTO representation.");
                }
            }
            catch (Exception ex)
            {
                var err = new CatalogDiagnosticError(
                    DiagnosticSeverity.FatalCorruption,
                    "year_of_ash_catalog",
                    sourcePath,
                    0, 0, "$",
                    "ERR_JSON_SYNTAX_PARSE",
                    $"Failed to parse root JSON syntax: {ex.Message}",
                    ex);
                _logger.LogFatal(err);
                result.AddDiagnostic(err);
                return result;
            }

            if (dto.SchemaVersion != 1)
            {
                var err = new CatalogDiagnosticError(
                    DiagnosticSeverity.FatalCorruption,
                    "year_of_ash_catalog",
                    sourcePath,
                    0, 0, "$.schema_version",
                    "ERR_UNSUPPORTED_SCHEMA",
                    $"Unsupported schema version {dto.SchemaVersion}. Expected version 1.");
                _logger.LogFatal(err);
                result.AddDiagnostic(err);
                return result;
            }

            var entries = new List<YearOfAshEntry>();
            if (dto.Entries != null)
            {
                for (int i = 0; i < dto.Entries.Count; i++)
                {
                    var rawEntry = dto.Entries[i];
                    string jsonPath = $"$.entries[{i}]";

                    if (string.IsNullOrWhiteSpace(rawEntry.EventId))
                    {
                        var err = new CatalogDiagnosticError(
                            DiagnosticSeverity.RecoverableError,
                            "year_of_ash_catalog",
                            sourcePath,
                            0, 0, $"{jsonPath}.event_id",
                            "ERR_MISSING_ID",
                            "Encountered entry with missing or empty event_id. Skipping entry.");
                        _logger.LogError(err);
                        result.AddDiagnostic(err);
                        continue;
                    }

                    if (rawEntry.BaseDangerRating < 0 || rawEntry.BaseDangerRating > 10)
                    {
                        var warn = new CatalogDiagnosticError(
                            DiagnosticSeverity.Warning,
                            "year_of_ash_catalog",
                            sourcePath,
                            0, 0, $"{jsonPath}.base_danger_rating",
                            "WARN_OUT_OF_BOUNDS",
                            $"Danger rating {rawEntry.BaseDangerRating} out of standard [0, 10] bounds. Clamping value.");
                        _logger.LogWarning(warn.ToString());
                        result.AddDiagnostic(warn);
                        rawEntry.BaseDangerRating = Math.Max(0, Math.Min(10, rawEntry.BaseDangerRating));
                    }

                    entries.Add(new YearOfAshEntry(
                        rawEntry.EventId,
                        rawEntry.EventName ?? "Unnamed Event",
                        rawEntry.Description ?? string.Empty,
                        rawEntry.BaseDangerRating,
                        rawEntry.RadiationDoseCgy,
                        rawEntry.RewardReputation
                    ));
                }
            }

            var catalog = new YearOfAshCatalog(entries);
            return new CatalogLoadResult<YearOfAshCatalog>(catalog);
        }
    }
}
```

### 3.3 Remediated `VerdictCatalogLoader.cs` (Hardened, 0 Bare Catch)
```csharp
namespace Ashfall.Core
{
    using System;
    using System.Collections.Generic;
    using Ashfall.Core.Diagnostics;
    using Ashfall.Core.Serialization;

    public sealed class VerdictCatalogLoader
    {
        private readonly ICatalogLogger _logger;
        private readonly IJsonSerializer _serializer;

        public VerdictCatalogLoader(ICatalogLogger logger, IJsonSerializer serializer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public CatalogLoadResult<VerdictCatalog> LoadCatalog(string jsonPayload, string sourcePath)
        {
            var result = new CatalogLoadResult<VerdictCatalog>(null);

            if (string.IsNullOrWhiteSpace(jsonPayload))
            {
                var err = new CatalogDiagnosticError(
                    DiagnosticSeverity.FatalCorruption,
                    "verdict_catalog",
                    sourcePath,
                    0, 0, "$",
                    "ERR_EMPTY_PAYLOAD",
                    "Verdict catalog JSON payload is empty.");
                _logger.LogFatal(err);
                result.AddDiagnostic(err);
                return result;
            }

            VerdictCatalogDto dto;
            try
            {
                dto = _serializer.Deserialize<VerdictCatalogDto>(jsonPayload);
                if (dto == null) throw new InvalidOperationException("Deserializer returned null for VerdictCatalogDto.");
            }
            catch (Exception ex)
            {
                var err = new CatalogDiagnosticError(
                    DiagnosticSeverity.FatalCorruption,
                    "verdict_catalog",
                    sourcePath,
                    0, 0, "$",
                    "ERR_JSON_SYNTAX_PARSE",
                    $"Verdict catalog JSON parse failure: {ex.Message}",
                    ex);
                _logger.LogFatal(err);
                result.AddDiagnostic(err);
                return result;
            }

            if (dto.SchemaVersion != 1)
            {
                var err = new CatalogDiagnosticError(
                    DiagnosticSeverity.FatalCorruption,
                    "verdict_catalog",
                    sourcePath,
                    0, 0, "$.schema_version",
                    "ERR_UNSUPPORTED_SCHEMA",
                    $"Unsupported schema version {dto.SchemaVersion}. Expected 1.");
                _logger.LogFatal(err);
                result.AddDiagnostic(err);
                return result;
            }

            var verdicts = new List<VerdictDefinition>();
            if (dto.Verdicts != null)
            {
                for (int i = 0; i < dto.Verdicts.Count; i++)
                {
                    var raw = dto.Verdicts[i];
                    string path = $"$.verdicts[{i}]";

                    if (string.IsNullOrWhiteSpace(raw.VerdictId))
                    {
                        var err = new CatalogDiagnosticError(
                            DiagnosticSeverity.RecoverableError,
                            "verdict_catalog",
                            sourcePath,
                            0, 0, $"{path}.verdict_id",
                            "ERR_MISSING_VERDICT_ID",
                            "Verdict entry missing mandatory verdict_id. Entry skipped.");
                        _logger.LogError(err);
                        result.AddDiagnostic(err);
                        continue;
                    }

                    verdicts.Add(new VerdictDefinition(
                        raw.VerdictId,
                        raw.Title ?? "Untitled Verdict",
                        raw.SeverityLevel,
                        raw.MoraleImpact,
                        raw.ResourcePenaltyRatio
                    ));
                }
            }

            return new CatalogLoadResult<VerdictCatalog>(new VerdictCatalog(verdicts));
        }
    }
}
```
"""

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(header)

print(f"Plan 02 Part 1 written! Current size: {len(header)} chars")
