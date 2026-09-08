# Plan 140 Baseline Reconnaissance & Forensic Audit

## 1. Executive Summary

Plan 140 activates ASHFALL's `feedback_messages.json` catalog as the authoritative player-facing transient feedback layer. Prior to Plan 140, the catalog existed solely as a loaded data file and test artifact. While registered as `GAMEPLAY_CONSUMED` in `CATALOG_REGISTRY.md`, no production host or UI system actually resolved its templates or presented them to the player.

Forensic analysis revealed three separate copies of feedback messages across the repository:
1. `Assets/StreamingAssets/Data/feedback_messages.json` (authoritative data, 200 templates).
2. `Assets/Ashfall.Core/Feedback/FeedbackMessageCatalogLoader.cs` (`CreateDefaultContainer()`, hardcoded in Core, exactly 200 templates).
3. `src/UI/FeedbackMessages.cs` (hardcoded in Godot C# with static dictionaries, exactly 200 templates, 0 callers).

Plan 140 reconciles this fragmentation, establishes a strict presentation boundary (`FeedbackEvent`), hooks authoritative domain systems into producer adapters, provides deduplication and spam suppression, and introduces a dedicated `FeedbackPanel` UI overlay in Godot.

---

## 2. Catalog Inventory & Structure

- **Total Templates in JSON:** 200
- **Total Templates in Fallback:** 200
- **Schema Version:** 1
- **Categories:** 20 categories, exactly 10 templates each:
  - `success` (10)
  - `failure` (10)
  - `warning` (10)
  - `error` (10)
  - `confirmation` (10)
  - `progress` (10)
  - `reward` (10)
  - `penalty` (10)
  - `status` (10)
  - `alert` (10)
  - `hint` (10)
  - `spoiler` (10)
  - `time_pressure` (10)
  - `resource_warning` (10)
  - `health_warning` (10)
  - `morale_warning` (10)
  - `relationship` (10)
  - `faction` (10)
  - `world_state` (10)
  - `system` (10)
- **Severities Distribution:**
  - `warning`: 74 templates
  - `info`: 48 templates
  - `critical`: 34 templates
  - `error`: 24 templates
  - `success`: 20 templates
- **Placeholder vs `parameter_count` Alignment:**
  - 200/200 templates strictly match their declared `parameter_count` against `{0}`, `{1}`, `{2}` tokens.
  - Parameter range: 0 to 3 arguments.
- **Duplicate Keys across Categories:**
  - `relationship_improved`: present in `success` (1 arg) and `relationship` (2 args).
  - `relationship_damaged`: present in `failure` (1 arg) and `relationship` (2 args).
  - `storm_approaching`: present in `warning` (1 arg) and `world_state` (1 arg).
  - *Resolution Contract:* `FeedbackMessageCatalog` supports category-scoped resolution (`composite = $"{category}:{key}"`). When resolving with category specified, exact match is guaranteed; flat lookup falls back to the registered entry.

---

## 3. Production Code References & Dead Surface Audit

1. **Production Usages of `FeedbackMessageCatalog`:**
   - Prior to Plan 140: 0 usages outside tests and scanner definitions.
   - `ContentUtilizationScanner.cs` referenced `FeedbackPanel` (missing) and `FeedbackMessageCatalog` (uninstantiated in `Main`).
2. **`src/UI/FeedbackMessages.cs`:**
   - Contains 10 static dictionaries duplicating message strings without duration, severity, or category metadata.
   - Codebase search confirmed 0 references in code (referenced only in design plan notes).
   - *Action:* Modernize `src/UI/FeedbackMessages.cs` into an active, thin Godot wrapper around Core's `FeedbackService` and provide `FeedbackPanel` as the HUD overlay.
3. **Existing Status Surfaces:**
   - Godot panels currently use local `LastEvent` strings on host sessions (e.g. `GreenhouseHostSession.LastEvent`, `DiseaseHostSession.LastEvent`, `SilentFoundryHostSession.LastEvent`).
   - These local status strips serve immediate panel context, but do not provide transient HUD notifications or cross-system alerts.

---

## 4. Baseline Verification Results

Executed prior to any modifications:
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`: **PASS** (10,080 passed, 0 failed, 0 skipped, duration 39s).
- `godot --headless --path . -- --data-integrity-selftest`: **PASS** (0 errors, 0 warnings across 300 catalogs).
- `godot --headless --path . -- --scene-binding-selftest`: **PASS** (25/25 passed, 0 failed).
- `python3 scripts/ci/scene-lint.py`: **PASS** (30 production scenes checked; 0 errors).
- `godot --headless --path . -- --content-utilization-selftest`: **PASS** (CI content utilization gate: PASS).

---

## 5. Architectural Guardrails & Invariants

1. **Invariant 1 (Core Engine-Agnostic):** `FeedbackMessageCatalog`, `FeedbackService`, `FeedbackEvent`, and `FeedbackDeduplicator` reside in `Assets/Ashfall.Core/Feedback/` with zero Godot or Unity references.
2. **Invariant 2 (Ports & Adapters):** Domain systems emit outcomes/events; host adapters translate domain outcomes into presentation `FeedbackEvent` DTOs.
3. **Invariant 3 (Save Compatibility & Ephemeral State):** Feedback toast queues and deduplication caches are ephemeral presentation state. They are never serialized into campaign save envelopes.
4. **Invariant 4 (Determinism):** Message formatting and parameter substitution are deterministic. No `DateTime.Now` or wall-clock dependencies in Core.
5. **Invariant 5 (No Gameplay Logic in Host):** Feedback messages report authoritative state; they never calculate, infer, or alter gameplay outcomes.
6. **Invariant 6 (Data Authority):** `Assets/StreamingAssets/Data/feedback_messages.json` is the sole authored content authority.
