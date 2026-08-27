# ASHFALL — Ordinal vs Case-Insensitive Boundary Architecture Audit

**Date:** 2026-08-27<br>
**Scope:** Forensic audit of string comparison, case normalization, and `StringComparer` usages across Core, Host sessions, Save Stores, and CLI parsers.

---

## 1. Architectural Directive & Determinism Invariant

In ASHFALL, **Invariant 4 (Determinism)** and **Invariant 3 (Save Compatibility)** require that save files and checksum hashes remain 100% deterministic regardless of runtime platform, operating system, or host engine.

Because [`SaveChecksum`](../../Assets/Ashfall.Core/SaveChecksum.cs) computes SHA-256 hashes over serialized field strings, any case variation in persisted IDs (e.g. `"item_medkit"` vs `"Item_Medkit"`) will alter the checksum envelope and cause corrupt save rejections.

---

## 2. Three Distinct String Comparison Boundaries

To balance strict persistence determinism with developer/player usability, ASHFALL enforces a clear three-tier boundary model:

```
┌────────────────────────────────────────────────────────────────────────┐
│ TIER 1: PERSISTENCE & CHECKSUM BOUNDARY (STRICT ORDINAL)               │
│ - Save stores (*SaveStore*.cs), DTOs, codecs, and SaveChecksum         │
│ - MUST use StringComparer.Ordinal and canonical snake_case values      │
│ - ZERO OrdinalIgnoreCase allowed in save store dictionaries or hashes  │
└────────────────────────────────────────────────────────────────────────┘
                                    ▲
                                    │ normalized at ingestion
┌────────────────────────────────────────────────────────────────────────┐
│ TIER 2: CORE SIMULATION & LEDGER BOUNDARY (NORMALIZED ORDINAL)         │
│ - InMemoryFlagLedger, FactionRadioEngine, TradeSpecialtySystem         │
│ - Ingested IDs are deliberately normalized via id.Trim().ToLower()     │
│ - Internal lookups use StringComparer.Ordinal                          │
└────────────────────────────────────────────────────────────────────────┘
                                    ▲
                                    │ user/CLI/search input
┌────────────────────────────────────────────────────────────────────────┐
│ TIER 3: PRESENTATION & CLI TOLERANCE BOUNDARY (ORDINAL IGNORE CASE)    │
│ - HostCliRegistry, HostTestSummary, UI search filters, legacy aliases  │
│ - Uses StringComparison.OrdinalIgnoreCase for user convenience         │
│ - NEVER serialized directly into save state without normalization      │
└────────────────────────────────────────────────────────────────────────┘
```

---

## 3. Audited Subsystem Inventory

| Subsystem / Class | Location | Comparer / Method | Boundary Classification | Rationale |
|---|---|---|---|---|
| `InMemoryFlagLedger` | `Assets/Ashfall.Core/Flags/IFlagLedger.cs` | `StringComparer.Ordinal` + `ToLowerInvariant()` | Tier 2 (Core Simulation) | Normalizes all incoming flag IDs to avoid case drift across hosts. |
| `FactionRadioEngine` | `Assets/Ashfall.Core/Radio/FactionRadioEngine.cs` | `StringComparer.Ordinal` + `ToLowerInvariant()` | Tier 2 (Core Simulation) | Normalizes faction IDs on channel registration and frequency lookups. |
| `VerdictHostSession` | `src/Host/VerdictHostSession.cs` | `StringComparer.Ordinal` | Tier 2 (Core Simulation) | Materialized NPC flags use strict ordinal hashing for snake_case constants. |
| `SaveChecksum` | `Assets/Ashfall.Core/SaveChecksum.cs` | Field order: `StringComparer.Ordinal` | Tier 1 (Persistence) | Normalizes field names in ordinal order for reproducible cross-platform hashing. |
| `HostCliRegistry` | `Assets/Ashfall.Core/HostCliRegistry.cs` | `StringComparer.OrdinalIgnoreCase` | Tier 3 (CLI Presentation) | Allows case-insensitive CLI flag recognition (e.g. `--host-help`, `--HOST-HELP`). |
| `HostTestSummary` | `Assets/Ashfall.Core/HostTestSummary.cs` | `StringComparison.OrdinalIgnoreCase` | Tier 3 (CLI Presentation) | Parses human-readable and structured test status (`"PASS"`, `"pass"`). |
| `ItemDefinitions` | `Assets/Ashfall.Core/Inventory/ItemDefinitions.cs` | `StringComparison.OrdinalIgnoreCase` | Tier 3 (Legacy Migration) | Maps deprecated legacy item names to canonical snake_case IDs. |
| `TradeCaravanCatalog` | `Assets/Ashfall.Core/Narrative/TradeCaravanCatalog.cs` | `StringComparer.OrdinalIgnoreCase` | Tier 3 (Query Interface) | Facilitates search queries over static narrative catalog routes. |

---

## 4. Verification & Automated CI Gates

1. **`Ashfall.Core.Tests/Save/PersistenceBoundaryDeterminismTests.cs`**:
   - Validates that save stores contain zero `OrdinalIgnoreCase` in their persistence paths.
   - Proves that `InMemoryFlagLedger` and `FactionRadioEngine` normalize IDs deterministically.
   - Enforces that case variations produce distinct checksums, confirming that non-normalized IDs are prevented from entering save stores.
