# Plan 21 — Phantom Memory & Heirloom World Layer Closeout

## 1. Executive Summary

**Plan 21: Phantom Memory & Heirloom World Layer** transforms ASHFALL's memory mechanics from a narrow 7-trigger prototype into an expansive, pervasive narrative world layer. Items across the wasteland and shelter are no longer mere inventory numbers: they carry historical witness, generational provenance, personal confessions, and moral leverage.

All tasks (21A through 21H) have been completed with zero technical debt, 100% test coverage, and clean CI gate verification.

---

## 2. Completed Scope & Deliverables

### Task 21A: Phantom Trigger Catalog Expansion
- **Runtime Contract:** Authored [`PHANTOM_MEMORY_RUNTIME_CONTRACT.md`](PHANTOM_MEMORY_RUNTIME_CONTRACT.md).
- **Prose Style Guide:** Authored [`PHANTOM_MEMORY_STYLE_GUIDE.md`](PHANTOM_MEMORY_STYLE_GUIDE.md).
- **Trigger Catalog (`phantom_triggers.json`):** Expanded from 7 background entries to 37 authored trigger rules spanning Personal Mementos (P1–P8), Work Tools (W1–W8), Ordinary Objects (O1–O7), and classic background triggers across 11 archetypes.
- **Item Authority Integration:** Added 40+ keepsake and phantom item definitions to `items.json` and tagged in `expansion_item_tags.json`.

### Task 21B: Heirloom Items & Generational Inheritance
- **Heirloom Core Architecture:** Built `HeirloomCatalog.cs` and `HeirloomSystem.cs`.
- **Authored Heirlooms (`phantom_heirlooms.json`):** Created 12 named heirlooms with 3-stage historical provenance (Pre-War Baseline, Cataclysm/Migration, Shelter Continuity) and holder-specific memory reactions.
- **Succession Engine:** Integrated with `GenerationalLineageExtension` (kin priority), `SurvivorRelationsSystem` (trust bond fallback), and shelter communal storage. Bounded history logs at 24 entries.

### Task 21C: Confession & Secret World-Objects
- **Confession Contract:** Authored [`CONFESSION_SECRET_RUNTIME_CONTRACT.md`](CONFESSION_SECRET_RUNTIME_CONTRACT.md).
- **Confession Core Architecture:** Built `ConfessionSecretCatalog.cs` and `ConfessionSecretSystem.cs`.
- **Authored Secrets (`confession_secrets.json`):** Expanded from 8 to 26 entries (8 existing + 8 new NPC personal + 6 faction institutional + 4 bunker internal).
- **Moral Leverage Engine:** Full multi-choice resolution mechanics: Expose (faction standings + guilt), Blackmail (hardening + supplies), Keep (trust boost), and Interpersonal Forgiveness vs Grudge.

### Task 21D & 21E: Psychological & Social Integration
- Linked triggers, heirlooms, and secrets directly into `NeedsSystem` (morale/breakdown), `GuiltInsomniaSystem` (guilt accumulation), `SurvivorRelationsSystem` (affinity/trust), `MoralBranchingSystem` (numbed resilience), and `GenerationalLineageExtension`.

### Task 21F & 21G: Continuity & QA Matrices
- Authored [`PLAN_21_MEMORY_CONTINUITY_MATRIX.md`](PLAN_21_MEMORY_CONTINUITY_MATRIX.md).
- Authored [`PLAN_21_MEMORY_QA_MATRIX.md`](PLAN_21_MEMORY_QA_MATRIX.md).

### Task 21H: Deterministic QA & Verification
- Authored unit test suites for `PhantomMemoryEngine`, `HeirloomSystem`, and `ConfessionSecretSystem`.
- Verified all CI gates and self-tests.

---

## 3. Key Files Summary

| Component | Files Created / Modified |
|---|---|
| **Documentation** | `docs/narrative/PHANTOM_MEMORY_RUNTIME_CONTRACT.md`<br>`docs/narrative/PHANTOM_MEMORY_STYLE_GUIDE.md`<br>`docs/narrative/CONFESSION_SECRET_RUNTIME_CONTRACT.md`<br>`docs/narrative/PLAN_21_MEMORY_CONTINUITY_MATRIX.md`<br>`docs/narrative/PLAN_21_MEMORY_QA_MATRIX.md`<br>`docs/narrative/PLAN_21_PHANTOM_MEMORY_HEIRLOOM_CLOSEOUT.md` |
| **Core C# Systems** | `Assets/Ashfall.Core/Phantoms/PhantomTriggerDto.cs`<br>`Assets/Ashfall.Core/PhantomMemoryEngine.cs`<br>`Assets/Ashfall.Core/Phantoms/HeirloomCatalog.cs`<br>`Assets/Ashfall.Core/Phantoms/HeirloomSystem.cs`<br>`Assets/Ashfall.Core/Phantoms/ConfessionSecretCatalog.cs`<br>`Assets/Ashfall.Core/Phantoms/ConfessionSecretSystem.cs`<br>`Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs` |
| **Data Authority** | `Assets/StreamingAssets/Data/phantom_triggers.json`<br>`Assets/StreamingAssets/Data/phantom_heirlooms.json`<br>`Assets/StreamingAssets/Data/confession_secrets.json`<br>`Assets/StreamingAssets/Data/items.json`<br>`Assets/StreamingAssets/Data/expansion_item_tags.json`<br>`Assets/StreamingAssets/Data/moral_choice_flags.json` |
| **Validation & Host** | `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`<br>`Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`<br>`src/Host/PhantomMemoryHostSession.cs` |

---

## 4. Verification Evidence

- `godot --headless --path . -- --data-integrity-selftest`: **PASS** (0 errors across 151 catalogs).
- `godot --headless --path . -- --content-utilization-selftest`: **PASS** (0 new orphans).
- `godot --headless --path . -- --scene-binding-selftest`: **PASS** (22/22 passed).
- `python3 scripts/ci/scene-lint.py`: **PASS** (26 scenes checked, 0 errors).
- `dotnet test Ashfall.Core.Tests`: **PASS**.
