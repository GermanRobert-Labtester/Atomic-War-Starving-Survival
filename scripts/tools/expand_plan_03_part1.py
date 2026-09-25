import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/03-schema-version-data-sweep.md"

header = """# Plan 03 — `schema_version` & Data-Authority Hygiene Sweep, Migration Envelopes & Automated Integrity Gating

**Package:** `PLAN-03-SCHEMA-VERSION-DATA-SWEEP`
**Document Class:** Master System Architecture, Data Migration Framework & Production Integration Blueprint
**Authority Level:** Canonical Production Plan
**Target Runtimes:** Ashfall.Core (`netstandard2.1`, Engine-Free) · Godot Host (`net8.0`) · Ashfall.Core.Tests (`net9.0`)
**Data Authority:** `Assets/StreamingAssets/Data/` (280+ snake_case JSON files, schema-validated)
**Historical Anchor:** piagentsplans Wave 1 (2026-08-30) · Core Reliability & Data Ingestion Suite · Master Authority Volumes 3, 11, 29, 38, 51
**Save Authority:** Foundation Data Contract; Enforces Schema Integrity for Save/Load Deserializers
**Determinism Mandate:** Pure Domain Invariants under Injected `CatalogIntegrityValidator`; Zero Ingestion Drift; Strict `snake_case`

---

# SECTION I: COMPREHENSIVE ARCHITECTURAL SCOPE & DATA CONSTITUTION

Plan 03 establishes the definitive data-authority hygiene constitution for *ASHFALL*. Authoritative gameplay data resides exclusively within `Assets/StreamingAssets/Data/`. In historical development, disparate files lacked explicit schema versioning, exhibited mixed naming conventions (`camelCase` vs `snake_case`), and lacked automated regression gates to prevent corrupt files from entering version control.

This plan establishes an automated, forward-compatible schema versioning and validation architecture across all ~280 JSON data files:

```
+===================================================================================================+
|                             AUTHORITATIVE DATA REPOSITORY (280+ FILES)                            |
|  Assets/StreamingAssets/Data/                                                                     |
|  - 12 Functional Data Families: Items, Economy, Quests, Factions, Survival, Radio, Tech, etc.   |
|  - Strict snake_case Key Formatting & schema_version: 1 Root Envelope                             |
+===================================================================================================+
                                                  │
                                                  ▼
+===================================================================================================+
|                        ASHFALL CORE SCHEMA MIGRATION & VALIDATION ENGINE                          |
|  Assets/Ashfall.Core/Diagnostics/ & Assets/Ashfall.Core/Data/                                     |
|  - CatalogIntegrityValidator (Root schema_version Assertion, Foreign Key Traversal)                |
|  - SchemaVersionMigrationPipeline (V1 -> V2 -> V3 Multi-Version Envelope Upgraders)               |
|  - SnakeCaseKeyEnforcer & Deprecation Warning Telemetry                                          |
|  - Pure Domain Logic - 100% Engine-Free (Zero Godot/UnityEngine References)                       |
+===================================================================================================+
        │                                         │                                      │
        ▼                                         ▼                                      ▼
+───────────────────────────+   +───────────────────────────────────+   +───────────────────────────+
| ROOT CATALOG ENVELOPES    |   | MIGRATION ADAPTERS                |   | CI INTEGRITY GATE         |
| - schema_version: 1       |   | - Backward-Compatible Decoders    |   | - Headless Godot Probes   |
| - catalog_id: snake_case  |   | - Field Defaults & Polyfills      |   | - 0-Error Fail-Fast Gate  |
| - items / records array   |   | - Type Coercion Normalizers       |   | - Commit Pre-Hook Checks  |
+───────────────────────────+   +───────────────────────────────────+   +───────────────────────────+
        │                                         │                                      │
        └─────────────────────────────────────────┼──────────────────────────────────────┘
                                                  ▼
+===================================================================================================+
|                          GODOT HEADLESS INTEGRATION & TEST HARNESS                                |
|  godot --headless --path . -- --data-integrity-selftest & Ashfall.Core.Tests                      |
|  - 100 Unit Tests Verifying Migration Rules, Key Casing, and Foreign Key Integrity                |
|  - Zero Regressions on Authoritative Game Data; Continuous CI/CD Gate Enforcement                 |
+===================================================================================================+
```

### 1.1 Non-Negotiable Rules of the Data Constitution
1. **Mandatory Schema Version**: Every root JSON file in `Assets/StreamingAssets/Data/` must declare `"schema_version": 1` (or its family's active version). Absence of this key is a fatal CI error.
2. **Universal snake_case**: All JSON dictionary keys, attribute names, catalog identifiers, and enum string literals must strictly conform to lowercase `snake_case` (`^[a-z0-9_]+$`).
3. **No Blind Bulk Rewrites**: Modifying key names in data without updating the owning C# loader DTO in the exact same commit is strictly forbidden, as it breaks data binding silently.
4. **Idempotent Migration Upgraders**: A migration pipeline step from Version $N$ to $N+1$ must be deterministic, pure, and idempotent.
5. **Referential Integrity**: Cross-catalog references (e.g. `audio_cue_id`, `faction_id`, `item_id`, `location_id`) must resolve to valid primary keys in their owning catalogs.

---

# SECTION II: THE 12 FUNCTIONAL DATA DOMAIN FAMILIES

The ~280 JSON files within `Assets/StreamingAssets/Data/` are partitioned into 12 functional domains:

1. **Items & Equipment (45 files)**: Weapons, ammunition, armor plates, ballistic helmets, radiation gear, survival tools, backpacks, containers.
2. **Medical & Pharmacology (28 files)**: Chelating agents, antibiotics, surgical sutures, blood plasma, anti-radiation drugs, psychotropics, bandages.
3. **Economy & Trade (24 files)**: Merchant inventories, barter exchange matrices, scrap scrap-value coefficients, regional supply/demand curves.
4. **Factions & Diplomacy (18 files)**: Faction ideologies, rank progression, hostility thresholds, truce treaties, tribute demands.
5. **Quests & Encounters (35 files)**: Branching mission graphs, tactical ambush encounters, investigative crime scenes, dialogue trees.
6. **Narrative & Lore (42 files)**: Pre-war journals, last letters, terminal logs, survivor diaries, bunker maintenance manifests.
7. **Ecology & Wildlife (16 files)**: Mutant beast stat blocks, hunting yields, toxic flora, predator-prey trophic food webs.
8. **Radio & Signals Intelligence (22 files)**: Broadcast schedules, number stations, frequency bands, Morse beacons, distress coordinates.
9. **Atmosphere, Weather & Radiation (14 files)**: Dust storm cycles, radioactive fallout plumes, solar flare tables, ambient temperature curves.
10. **Technology & Relic Blueprints (18 files)**: Pre-war schematic recovery, workshop machine tiers, reverse-engineering formulas.
11. **Bunker Architecture & Systems (12 files)**: Room modules, ventilation air duct networks, electrical grid wiring, hydroponics basins.
12. **Audio & Diegetic Sound (10 files)**: Sound cues, acoustic reverberation presets, phonograph vinyl record catalogs.

---

# SECTION III: PURE C# DOMAIN IMPLEMENTATION (`Assets/Ashfall.Core/Data/`)

The following domain implementation resides in `Assets/Ashfall.Core/Diagnostics/` and `Assets/Ashfall.Core/Data/` (`netstandard2.1`):

### 3.1 `CatalogIntegrityValidator.cs`
```csharp
namespace Ashfall.Core.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    public sealed class DataCatalogIntegrityReport
    {
        public int TotalCatalogsEvaluated { get; set; }
        public int TotalKeysInspected { get; set; }
        public List<string> SchemaVersionMissingErrors { get; } = new List<string>();
        public List<string> CamelCaseKeyWarnings { get; } = new List<string>();
        public List<string> DuplicateIdErrors { get; } = new List<string>();
        public List<string> OrphanForeignKeyErrors { get; } = new List<string>();

        public bool HasFatalErrors => SchemaVersionMissingErrors.Count > 0 || DuplicateIdErrors.Count > 0 || OrphanForeignKeyErrors.Count > 0;
    }

    public sealed class CatalogIntegrityValidator
    {
        private static readonly Regex SnakeCaseRegex = new Regex("^[a-z0-9_]+$", RegexOptions.Compiled);
        private static readonly Regex CamelCaseRegex = new Regex("^[a-z]+[A-Z0-9][a-zA-Z0-9]*$", RegexOptions.Compiled);

        private readonly HashSet<string> _knownItemIds = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _knownAudioCueIds = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _knownFactionIds = new HashSet<string>(StringComparer.Ordinal);

        public void RegisterAuthoritativeIds(IEnumerable<string> itemIds, IEnumerable<string> audioCueIds, IEnumerable<string> factionIds)
        {
            if (itemIds != null) foreach (var id in itemIds) _knownItemIds.Add(id);
            if (audioCueIds != null) foreach (var id in audioCueIds) _knownAudioCueIds.Add(id);
            if (factionIds != null) foreach (var id in factionIds) _knownFactionIds.Add(id);
        }

        public void ValidateCatalogFile(string filePath, string fileContent, DataCatalogIntegrityReport report)
        {
            if (string.IsNullOrWhiteSpace(fileContent))
            {
                report.SchemaVersionMissingErrors.Add($"[{filePath}] File is empty or whitespace.");
                return;
            }

            report.TotalCatalogsEvaluated++;

            // Rule 1: Root schema_version presence
            if (!fileContent.Contains("\"schema_version\""))
            {
                report.SchemaVersionMissingErrors.Add($"[{filePath}] Missing mandatory 'schema_version' attribute at root.");
            }

            // Rule 2: Tokenize keys and detect camelCase drift
            var keyMatches = Regex.Matches(fileContent, "\"([a-zA-Z0-9_]+)\"\\s*:");
            foreach (Match match in keyMatches)
            {
                report.TotalKeysInspected++;
                string key = match.Groups[1].Value;

                if (CamelCaseRegex.IsMatch(key))
                {
                    report.CamelCaseKeyWarnings.Add($"[{filePath}] Key '{key}' violates snake_case convention (camelCase detected).");
                }
            }
        }

        public bool ValidateForeignKeyReference(string foreignKey, string expectedDomain, out string error)
        {
            error = string.Empty;
            switch (expectedDomain)
            {
                case "item":
                    if (!_knownItemIds.Contains(foreignKey))
                    {
                        error = $"Orphan item foreign key '{foreignKey}' does not exist in items catalog.";
                        return false;
                    }
                    break;
                case "audio_cue":
                    if (!_knownAudioCueIds.Contains(foreignKey))
                    {
                        error = $"Orphan audio cue foreign key '{foreignKey}' does not exist in audio cues catalog.";
                        return false;
                    }
                    break;
                case "faction":
                    if (!_knownFactionIds.Contains(foreignKey))
                    {
                        error = $"Orphan faction foreign key '{foreignKey}' does not exist in factions catalog.";
                        return false;
                    }
                    break;
            }
            return true;
        }
    }
}
```

### 3.2 `SchemaVersionMigrationPipeline.cs`
```csharp
namespace Ashfall.Core.Data
{
    using System;
    using System.Collections.Generic;

    public interface ISchemaUpgrader
    {
        int SourceVersion { get; }
        int TargetVersion { get; }
        string UpgradePayload(string jsonInput);
    }

    public sealed class SchemaVersionMigrationPipeline
    {
        private readonly List<ISchemaUpgrader> _upgraders = new List<ISchemaUpgrader>();

        public void RegisterUpgrader(ISchemaUpgrader upgrader)
        {
            if (upgrader == null) throw new ArgumentNullException(nameof(upgrader));
            _upgraders.Add(upgrader);
        }

        public string MigratePayloadToLatest(string jsonInput, int currentPayloadVersion, int targetLatestVersion)
        {
            if (currentPayloadVersion == targetLatestVersion) return jsonInput;
            if (currentPayloadVersion > targetLatestVersion)
            {
                throw new InvalidOperationException($"Payload version {currentPayloadVersion} is newer than latest supported target version {targetLatestVersion}.");
            }

            string currentJson = jsonInput;
            int v = currentPayloadVersion;

            while (v < targetLatestVersion)
            {
                var upgrader = _upgraders.Find(u => u.SourceVersion == v && u.TargetVersion == v + 1);
                if (upgrader == null)
                {
                    throw new InvalidOperationException($"No registered schema upgrader found from version {v} to {v + 1}.");
                }

                currentJson = upgrader.UpgradePayload(currentJson);
                v = upgrader.TargetVersion;
            }

            return currentJson;
        }
    }
}
```
"""

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(header)

print(f"Plan 03 Part 1 written! Current size: {len(header)} chars")
