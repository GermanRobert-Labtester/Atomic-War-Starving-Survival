# ASHFALL Content Acceptance Pipeline

> **Authority:** Plan 45 / C1[14] — *The Content Acceptance Pipeline: Nothing Authors Itself Anymore*\
> **Orchestrator:** `Ashfall.Core.Content.ContentAcceptancePipeline`\
> **CI Script:** `scripts/ci/content-acceptance-gate.sh`\
> **Status:** Authoritative gate\

---

## 1. Purpose

The Content Acceptance Pipeline enforces that no content file enters or remains in ASHFALL purely as dead JSON without execution evidence. A catalog or content pack is accepted only when it completes the required rungs on the 8-rung ladder, proving that definitions actually reach the simulation loop, produce authoritative effects, or render to the player.

---

## 2. The 8 Rungs of Acceptance

| Rung | Name | Level | Criterion |
|---|---|---|---|
| **1** | `PARSES` | Syntax | Valid schema-conformant JSON without syntax errors. |
| **2** | `IDS_RESOLVE` | Integrity | All ID prefixes conform to naming rules; cross-catalog references resolve. |
| **3** | `LOADED` | Ingestion | A production catalog loader deserializes definitions into memory. |
| **4** | `CONSUMER_EXISTS` | Wiring | At least one domain system or subsystem queries or manages the catalog. |
| **5** | `PLAYER_OR_SIM_REACHABLE` | Reachability | Content can be triggered by player choice, event table, or simulation loop. |
| **6** | `EFFECT_PRODUCED` | Mechanics | Consuming the content mutates domain state, creates modifiers, or inflicts consequences. |
| **7** | `PRESENTED` | Presentation | Content renders on a player UI surface, journal entry, radio broadcast, or HUD. |
| **8** | `SAVE_ROUNDTRIP` | Persistence | Dynamic modifications survive state capture and restore without drift. |

---

## 3. Four-Stage Gate Runner

The canonical gate runner (`scripts/ci/content-acceptance-gate.sh`) composes four verification rungs executed in strict sequence:

1. **Rung 1: Integrity Gate**
   - Command: `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~CatalogIntegrity`
   - Validates schema, prefixes, IDs, and cross-catalog foreign keys across all catalogs.
2. **Rung 2: Utilization Gate**
   - Command: `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~ContentAcceptance`
   - Verifies that loaders, consumer systems, and acceptance rungs are satisfied.
3. **Rung 3: Canon Gate**
   - Command: `python3 scripts/ci/generate-catalog-registry.py --check`
   - Checks that catalog registry counts and documented tables match authored JSON files.
4. **Rung 4: Quality Gate**
   - Command: `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~CollectibleNarrativeQualityTests`
   - Enforces prose standards, narrative tone, punctuation, and voice.

---

## 4. Fail-Fast Execution Semantics

The runner executes with fail-fast semantics:
- The first red rung halts the pipeline immediately.
- The failing rung name and underlying test/diagnostic output are printed to `stderr`.
- The process exits with code 1.
- Only when all 4 rungs pass cleanly does the runner output `CONTENT_ACCEPTANCE_PIPELINE PASS` and exit with code 0.

---

## 5. Content Author Workflow

When adding new content:
1. Author data in `Assets/StreamingAssets/Data/` with valid `schema_version` and snake_case IDs.
2. Provide a loader in `Assets/Ashfall.Core/` conforming to `CatalogLocator` conventions.
3. Wire the content into a consuming system (`Assets/Ashfall.Core/`) that applies effects.
4. Run the gate locally:
   ```bash
   bash scripts/ci/content-acceptance-gate.sh
   ```
5. If a rung fails, resolve the exact deficiency (syntax, missing loader, unreferenced ID, or missing effect) rather than adding an unbacked exemption.
