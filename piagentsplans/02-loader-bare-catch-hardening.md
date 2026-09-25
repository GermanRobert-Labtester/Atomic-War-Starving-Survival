# Plan 02 — Catalog Loader Failure Observability and Silent-Fallback Governance

> **Rebuild status:** COMPLETE H4 HARDENING — MAINTENANCE AND REGRESSION PLAN
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-2026-09-25`
>
> **Current-evidence date:** 2026-09-25
>
> **Authority order:** live source and data → `AGENTS.md` → `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` → plan ledgers → this document.
>
> **Authority checksum:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
>
> **Length policy:** the 150k–170k band is a completeness checkpoint, never a reason to add filler. This plan is allowed to trim below the band if the verified architecture is exhausted.

## 0. Integrity Statement and Plan Status

This file replaces unclaimed generated sections that mixed current evidence, fictional APIs, and unsupported save claims. It is a planning and architecture artifact only. It authorizes no production, data, test, save, generated-index, or UI edits. Every path labeled current must exist at rebuild time. Any future `CREATE` proposal is explicitly hypothetical and belongs to a later, separately claimed implementation package.

The rebuild follows four passes: content/current-reality first; integration framework second; accuracy and contradiction removal third; independent precision and handoff review fourth. Character count is recorded by external verification, not embedded recursively in the document.

# 1. Objective

- The original plan named thirteen silent catch blocks. Current source shows structured warning calls with the attempted shape and exception reason. The shared sink is engine-agnostic and accepts an injected `ILog`.
- The correct future work is governance: keep malformed-present data visible, keep absent optional data non-fatal where the loader contract says optional, and ensure a diagnostics sink failure cannot crash campaign boot.
- The plan also records a known narrow last-resort catch inside `CatalogDiagnostics.Warn`. That catch is intentional sink isolation, not a loader swallowing a data error, and should be documented by a focused failure test rather than removed blindly.

**Bounded outcome:** H4 is closed. `YearOfAshCatalogLoader` and `VerdictCatalogLoader` route malformed-present files through `CatalogDiagnostics.Warn`; missing optional files retain the documented empty fallback. This plan preserves the central diagnostic sink and prevents regression without changing valid-load behavior.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `CatalogDiagnostics` centralizes warnings and defaults to `ConsoleLog` in Core.
- Year of Ash loaders attempt canonical shape then wrapped-list fallback and report both failures.
- Verdict loaders report malformed list/container shapes and return their documented empty result.
- `CatalogLoaderHardeningTests` is the historical regression surface named by the archived H4 seal.

**Master-authority sections applied to this rebase:**

- Part III Lane H tooling
- Part VI anti-padding
- Volume 28 cross-cutting gates

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the obsolete bare-catch count with a current loader-policy matrix.
- Distinguish required, optional, malformed-present and malformed-fallback cases.
- Add sink-failure isolation proof for `CatalogDiagnostics.Warn`.
- Require new loaders to use the shared diagnostic path rather than console output or silent fallback.

Anything beyond this list is a different package. In particular, this plan does not convert a documentation gap into permission to create a second domain owner.

# 4. Current Evidence and Premise Audit

The current evidence set for this plan is enumerated in the appendices with file hashes, declaration digests, catalog schema/counts and focused test inventories. A source declaration proves an API surface exists; it does not prove a fresh test run or live player reachability. Those claims require the verification steps in this document.

**Evidence classes used here:**

- **VERIFIED CURRENT:** the named path exists and its contents were read during this rebuild.
- **HISTORICAL RECORD:** an archived closeout or old plan says a package once landed; it is useful context but is not current pass evidence.
- **PROPOSAL:** a future seam or file shape that requires a new claim and premise recheck.
- **UNKNOWN:** deliberately unresolved because the present plan does not need to invent an answer.

# 5. Existing Extension Seams

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| parse-failure visibility | CatalogDiagnostics | `Assets/Ashfall.Core/IO/CatalogDiagnostics.cs` | Central warning sink; no gameplay state. |
| Year of Ash shape fallback | YearOfAshCatalogLoader | `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs` | Owns parse order and fallback result. |
| Verdict catalog shape loading | VerdictCatalogLoader | `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs` | Owns Verdict parse and empty fallback. |
| failure observability and parity | Catalog loader tests | `Ashfall.Core.Tests/CatalogLoaderHardeningTests.cs` | Executable H4 contract. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Catalog Loader Failure Observability and Silent-Fallback Governance
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ CatalogDiagnostics
│   parse-failure visibility
│ YearOfAshCatalogLoader
│   Year of Ash shape fallback
│ VerdictCatalogLoader
│   Verdict catalog shape loading
│ Catalog loader tests
│   failure observability and parity
                │
                ▼
Host projection → existing command → owner mutation → typed fact
                │
                ├─ UI / briefing / journal / audio presentation
                ├─ existing save envelope and checksum
                └─ focused Core / host / headless verification
```

The architecture is deliberately projection-first where a read model is sufficient, owner-extension-first where new mutable facts are required, and data-first only when an existing catalog can express the content. It does not permit a new subsystem merely to make the plan look larger.

## 6.1 Architectural decisions

1. **Preserve current state ownership.** CatalogDiagnostics owns parse-failure visibility: Central warning sink; no gameplay state.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| parse-failure visibility | CatalogDiagnostics | `Assets/Ashfall.Core/IO/CatalogDiagnostics.cs` | Central warning sink; no gameplay state. |
| Year of Ash shape fallback | YearOfAshCatalogLoader | `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs` | Owns parse order and fallback result. |
| Verdict catalog shape loading | VerdictCatalogLoader | `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs` | Owns Verdict parse and empty fallback. |
| failure observability and parity | Catalog loader tests | `Ashfall.Core.Tests/CatalogLoaderHardeningTests.cs` | Executable H4 contract. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. Resolve data path through IFileIO
2. check optional/required presence
3. read text
4. attempt canonical deserialize
5. attempt declared fallback shape
6. warn with path and shape on failure
7. return documented empty/partial result

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Diagnostics are process configuration, not campaign state.
- Malformed content must not mutate catalog definitions.
- A successful fallback shape returns the same data as before.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Missing optional file: empty result, no warning unless the owning loader explicitly requires it.
- Malformed present file: warning that includes path, shape and exception message.
- Valid canonical file: no warning and byte-equivalent DTO result.
- Diagnostics sink throws: swallow only that sink failure; campaign load continues.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- No catalog rewrite.
- No blanket snake_case conversion.
- No loader DTO shape change.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- No save impact.
- Loader behavior affects boot availability, not save serialization.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Loader ordering is deterministic.
- Fallback attempts are fixed and documented; no random probing.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- No gameplay event.
- ILog warning output is the observable contract.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Main.Application.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Warnings should be operational, not fabricated diegetic messages.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | Malformed required data returns empty without a diagnostic. | CatalogDiagnostics | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | Optional absence becomes an error and prevents boot. | YearOfAshCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A fallback parser masks a canonical schema regression. | VerdictCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | The diagnostic sink throws and escapes. | Catalog loader tests | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new loader writes directly to Console. | CatalogDiagnostics | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/CatalogLoaderHardeningTests.cs`
2. `bash scripts/ci/catch-policy-gate.sh` when changing loader catch structure.

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — current H4 proof | Rerun the hardening suite and inspect diagnostic call sites. | No bare loader catches and focused tests pass. | No production path until the owning implementation package is separately claimed. |
| 1 — policy matrix | Classify each catalog as required, optional or shape-fallback. | Every loader has one documented policy. | No production path until the owning implementation package is separately claimed. |
| 2 — sink isolation | Prove a throwing log sink cannot crash parsing. | Focused isolation test passes. | No production path until the owning implementation package is separately claimed. |
| 3 — enforcement | Add a source/CI check for direct console output or silent parse catches in loaders. | No new violation can merge silently. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/Ashfall.Core/IO/CatalogDiagnostics.cs | MODIFY only for a proven policy gap | Shared diagnostics |
| Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs | READ ONLY unless regression found | Current owner |
| Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs | READ ONLY unless regression found | Current owner |
| Ashfall.Core.Tests/CatalogLoaderHardeningTests.cs | EXTEND for sink isolation if absent | Focused contract |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Overclassifying optional content as required. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Removing the intentional sink-isolation catch. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Changing fallback order and silently altering valid data behavior. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No global exception policy rewrite.
- No data normalization.
- No production change without a reproduced loader defect.

# 23. Rollback and Recovery

- Revert the isolated diagnostic/test change.
- Never roll back by restoring silent swallowing.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- H4 is explicitly closed.
- Current tests and source call sites are named.
- Optional/required behavior is unambiguous.
- No direct console logging is planned in Core loaders.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the obsolete bare-catch count with a current loader-policy matrix.
- Distinguish required, optional, malformed-present and malformed-fallback cases.
- Add sink-failure isolation proof for `CatalogDiagnostics.Warn`.
- Require new loaders to use the shared diagnostic path rather than console output or silent fallback.

## MUST NOT DO

- No global exception policy rewrite.
- No data normalization.
- No production change without a reproduced loader defect.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/CatalogLoaderHardeningTests.cs`
2. `bash scripts/ci/catch-policy-gate.sh` when changing loader catch structure.

## FIRST SAFE IMPLEMENTATION STEP

0 — current H4 proof — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: parse-failure visibility → CatalogDiagnostics; Year of Ash shape fallback → YearOfAshCatalogLoader; Verdict catalog shape loading → VerdictCatalogLoader; failure observability and parity → Catalog loader tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 02.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 02 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by CatalogDiagnostics or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/IO/CatalogDiagnostics.cs`

### `Assets/Ashfall.Core/IO/CatalogDiagnostics.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 70 lines / 2503 bytes.
- SHA-256: `a46378ce016c431bb456f6ff60e171722a39ad5e5a1fdf553e62e8dcf4b227dc`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class CatalogDiagnostics
public static void RegisterLog(ILog sink) {
public static void Warn(string path, string shape, Exception ex) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs`

### `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 462 lines / 19109 bytes.
- SHA-256: `d409b646a69d42b1145131b16fd6ef992025ab3bd739118c48b23e2a1ebed209`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class YearOfAshItemEntry
public string id = string.Empty;
public string name = string.Empty;
public string category = string.Empty;
public string description = string.Empty;
public float tradeValue = 0f;
public float weightKg = 0f;
public class YearOfAshEventEntry
public string id = string.Empty;
public string title = string.Empty;
public string description = string.Empty;
public int day = 180;
public string hazardType = string.Empty;
public string phase = string.Empty;
public float temperatureDeltaC = 0f;
public float exposureMultiplier = 1.0f;
public float pressureMultiplier = 1.0f;
public class YearOfAshLocationEntry
public string id = string.Empty;
public string displayName = string.Empty;
public string sector = string.Empty;
public int riskLevel = 1;
public float radiationUsv = 0f;
public string description = string.Empty;
public class YearOfAshRadioEntry
public string id = string.Empty;
public string frequency = string.Empty;
public int dayTrigger = 180;
public bool isEmergency = false;
public string message = string.Empty;
public string signalStrength = string.Empty; // "S7" etc. — the radio signal scale, not a number
public string source = string.Empty;
public string audio_cue = string.Empty;
public class YearOfAshSurvivorEntry
public string id = string.Empty;
public string name = string.Empty;
public string occupation = string.Empty;
public float rurScore = 10.0f;
public string moralAlignment = string.Empty;
public int age = 30;
public float healthPercent = 100f;
public float radiationDoseMsv = 0f;
public int guiltScore = 0;
public string backstory = string.Empty;
public string confession = string.Empty;
public string factionAffinity = string.Empty;
public List<string> traits = new List<string>();
public class YearOfAshItemContainer { public int schema_version; public List<YearOfAshItemEntry> items = new List<YearOfAshItemEntry>(); }
public class YearOfAshEventContainer { public int schema_version; public List<YearOfAshEventEntry> events = new List<YearOfAshEventEntry>(); }
public class YearOfAshLocationContainer { public int schema_version; public List<YearOfAshLocationEntry> locations = new List<YearOfAshLocationEntry>(); }
public class YearOfAshRadioContainer { public int schema_version; public List<YearOfAshRadioEntry> broadcasts = new List<YearOfAshRadioEntry>(); }
public class YearOfAshSurvivorContainer { public int schema_version; public List<YearOfAshSurvivorEntry> survivors = new List<YearOfAshSurvivorEntry>(); }
public class YearOfAshQuestContainer { public int schema_version; public List<QuestlineDefinition> quests = new List<QuestlineDefinition>(); }
public class RawQuestEntry
public string id = string.Empty;
public string questlineId = string.Empty;
public string title = string.Empty;
public string synopsis = string.Empty;
public string faction = string.Empty;
public string factionTag = string.Empty;
public int minDay = 180;
public int maxDay = 360;
public List<RawQuestStage> stages = new List<RawQuestStage>();
public class RawQuestStage
public string stageId = string.Empty;
public int stageIndex = 0;
public string objective = string.Empty;
public string title = string.Empty;
public string narrativePrompt = string.Empty;
public string requiredItemId = string.Empty;
public bool isCompleted = false;
public class RawQuestContainer
public List<RawQuestEntry> quests = new List<RawQuestEntry>();
public static class YearOfAshCatalogLoader
public const string ItemsFile = "year_of_ash_items.json";
public const string EventsFile = "year_of_ash_events.json";
public const string QuestsFile = "year_of_ash_quests.json";
public const string QuestlinesFile = "year_of_ash_questlines.json";
public const string LocationsFile = "year_of_ash_locations.json";
public const string RadioFile = "year_of_ash_radio.json";
public const string SurvivorsFile = "year_of_ash_survivors.json";
public static List<QuestlineDefinition> LoadQuestlines(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<YearOfAshItemEntry> LoadItems(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<YearOfAshEventEntry> LoadEvents(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<QuestlineDefinition> LoadQuests(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<YearOfAshLocationEntry> LoadLocations(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<YearOfAshRadioEntry> LoadRadioBroadcasts(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<YearOfAshSurvivorEntry> LoadSurvivors(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static int LoadAndRegisterQuests(QuestlineSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs`

### `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 225 lines / 10025 bytes.
- SHA-256: `abee1d19deb019bb058c9435069600549265c68836d2723c607fccd88c23d334`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class VerdictCatalogLoader
public const string DataFile = "verdict_data.json";
public const string LocationsFile = "verdict_locations.json";
public const string ItemsFile = "verdict_items.json";
public const string RadioFile = "verdict_radio.json";
public class VerdictLocationEntry
public string id = string.Empty;
public string displayName = string.Empty;
public string description = string.Empty;
public int dangerLevel = 5;
public float travelHours = 5f;
public float baseRadsPerHour = 30f;
public static List<VerdictLocationEntry> LoadLocations( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public class VerdictItemEffects
public int enrolled_evidence;
public string note = string.Empty;
public class VerdictItemEntry
public string id = string.Empty;
public string displayName = string.Empty;
public float weightKg;
public float tradeValue;
public string category = "story_item";
public string tier = string.Empty;
public string description = string.Empty;
public VerdictItemEffects mechanical_effects = null!;
public string downstream_quest_trigger = string.Empty;
public string faction_affinity = string.Empty;
public string rarity = string.Empty;
public static List<VerdictItemEntry> LoadItems( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public class VerdictRadioEntry
public string id = string.Empty;
public string frequency = string.Empty;
public int dayTrigger = 180;
public string source = string.Empty;
public string message = string.Empty;
public string signalStrength = string.Empty;
public string kind = "telemetry";
public string audio_cue = string.Empty;
public static List<VerdictRadioEntry> LoadRadio( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public class VerdictWorldHistoryLadderEntry
public int layer { get; set; }
public string knowledge_key { get; set; } = string.Empty;
public string title { get; set; } = string.Empty;
public string discovery_location_id { get; set; } = string.Empty;
public string body_summary { get; set; } = string.Empty;
public List<string> corruption_corpus = new List<string>();
public List<VerdictWorldHistoryLadderEntry> world_history_ladder = new List<VerdictWorldHistoryLadderEntry>();
public static List<string> LoadCorruptionCorpus( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<VerdictWorldHistoryLadderEntry> LoadWorldHistoryLadder( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public List<VerdictRadioEntry> broadcasts = new List<VerdictRadioEntry>();
```


# Appendix B.05 — Current Code Architecture: `src/Main.Application.cs`

### `src/Main.Application.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1144 lines / 55746 bytes.
- SHA-256: `ea35c17ff8c236beead0d68584cde85f606db646aa0072a8ca203c74cb1893ed`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=7; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public override void _Ready() {
public override void _Process(double delta) {
public override void _UnhandledKeyInput(InputEvent @event) {
public override void _Notification(int what) {
```


# Appendix C.06 — Catalog Census: `Assets/StreamingAssets/Data/year_of_ash_events.json`

### `Assets/StreamingAssets/Data/year_of_ash_events.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 21786 bytes / 21776 characters.
- SHA-256: `3dbca873f5c54fcfdfd4a2c6efd63b4ab8edd8c58bec2bde3a9707e144e7efa5`.
- Root keys: `events`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
events: min=52, max=52, observed_paths=1
```

Representative record fields:

- `day`
- `description`
- `exposureMultiplier`
- `hazardType`
- `id`
- `phase`
- `pressureMultiplier`
- `temperatureDeltaC`
- `title`

Representative identifiers (ordered, capped for readability):

```text
event_deep_freeze_onset
event_conduit_brittle_fracture
event_sentry_frostbite_wave
event_allotment_cold_frame_collapse
event_diesel_fuel_gelling
event_exhaust_flue_rime_choke
event_periscope_hoarfrost_blindness
event_lead_acid_electrolyte_slush
event_hydraulic_blast_seal_freeze
event_deep_freeze_stasis_cabin_fever
event_garrison_registration_sweep
event_howitzer_shrapnel_concussion
event_ash_sign_penitent_siege
event_hydro_baron_pipeline_throttling
event_d9_culvert_demolition
event_continuity_decree_promulgation
event_warlord_toll_ambush
event_allotment_brass_embargo
event_shrapnel_intake_perforation
event_garrison_firing_squad
event_black_mud_thaw_inundation
event_radon_gas_fissure_breach
event_radio_142_carrier_intercept
event_sump_pump_overrun
event_humid_spore_outbreak
event_thawing_snowpack_corpse_discoveries
event_aquifer_salt_shock
event_coastal_pack_ice_calving_booms
event_aurora_borealis_manifest_call
event_mass_spectrometer_warhead_revelations
event_permafrost_subsidence_shifts
event_geothermal_meltwater_flashing
event_first_green_sprout_sightings
event_d9_carrier_stand_down
event_garrison_headquarters_abandonment
event_final_dawn_year_one
event_granite_arsenal_foundry_explosion
event_railway_guild_handcar_strike
event_penal_battalion_mutiny_trench
event_salt_freeholders_dynamite_blasting
event_supply_corps_armored_breakout
event_cyanide_insulation_fire
event_mustard_gas_pocket_release
event_ash_militia_terrace_fortification
event_mercury_barometer_extreme_drop
event_telegraph_wire_snipping_spree
event_potassium_permanganate_water_panic
event_heavy_howitzer_bunker_direct_hit
event_the_thin_margin_disclosure
event_the_thirsty_season
event_osteophage_explanation
event_measurement_broadcast
```


# Appendix C.07 — Catalog Census: `Assets/StreamingAssets/Data/verdict_data.json`

### `Assets/StreamingAssets/Data/verdict_data.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 9814 bytes / 9776 characters.
- SHA-256: `2c7ef4992e16b4d32e0a3a12af42f4d12bf8460e51a0c69eadc8e0d42799dae2`.
- Root keys: `catalog`, `corruption_corpus`, `currencies`, `description`, `endings`, `facets`, `readout_steps`, `schema_version`, `world_history_ladder`.

Array-path census (minimum, maximum, observed rows):

```text
corruption_corpus: min=25, max=25, observed_paths=1
currencies: min=1, max=1, observed_paths=1
endings: min=3, max=3, observed_paths=1
facets: min=3, max=3, observed_paths=1
facets[].bylines: min=5, max=5, observed_paths=2
readout_steps: min=4, max=4, observed_paths=1
world_history_ladder: min=12, max=12, observed_paths=1
```

Representative record fields:

- `id`
- `label`
- `note`

Representative identifiers (ordered, capped for readability):

```text
enrolled_evidence
```


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/verdict_radio.json`

### `Assets/StreamingAssets/Data/verdict_radio.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 11606 bytes / 11597 characters.
- SHA-256: `2a8a7faeb9d7a9af175f7188311f1c94892a63604c88932e12d068e70ea555aa`.
- Root keys: `broadcasts`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
broadcasts: min=30, max=30, observed_paths=1
```

Representative record fields:

- `audio_cue`
- `dayTrigger`
- `frequency`
- `id`
- `kind`
- `message`
- `signalStrength`
- `source`

Representative identifiers (ordered, capped for readability):

```text
radio_verdict_meter_reads_1142
radio_verdict_fuse_serviced
radio_verdict_wing_sleeps
radio_verdict_off_count_assessed
radio_verdict_eden_was_here
radio_verdict_count_is_open
radio_verdict_clock_disagrees
radio_verdict_geophone_taps
radio_verdict_valve_accessed_36
radio_verdict_reels_matter
radio_verdict_presentation_names_holders
radio_verdict_carrier_on_window
radio_verdict_reckoning_call
radio_verdict_barometric_spread
radio_verdict_service_cycle_greywater
radio_verdict_stilling_well_delta
radio_verdict_subsector_ledger_update
radio_verdict_geophone_offset_recal
radio_verdict_strata_density_drift
radio_verdict_relay_switch_pass4
radio_verdict_unscheduled_burst_88
radio_verdict_river_stage_deviation
radio_verdict_core_vault_desiccant_purge
radio_verdict_unverified_household_tally
radio_verdict_repeater_origin_mismatch
radio_verdict_spectrometry_drift_stjude
radio_verdict_substation_breaker_test
radio_verdict_holding_capacity_parity
radio_verdict_telemetry_phase_inversion
radio_verdict_carrier_override_standby
```


# Appendix D.09 — Existing Focused Test Inventory: `Ashfall.Core.Tests/CatalogLoaderHardeningTests.cs`

### `Ashfall.Core.Tests/CatalogLoaderHardeningTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 155; SHA-256: `cca59747f8003f17231468450f140ea79bc9c4a3b78adde5643c27565c1bde7e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
YearOfAshMalformedOptionalCatalog_IsObservableWithPathAndShape
VerdictMalformedOptionalCatalog_IsObservableWithPathAndShape
YearOfAshValidCatalog_LoadsExpectedEntries
VerdictValidCatalog_LoadsExpectedEntries
YearOfAshMissingOptionalFile_ReturnsEmptyWithoutThrowingOrLogging
VerdictMissingOptionalFile_ReturnsEmptyWithoutThrowingOrLogging
```


# Appendix E.10 — Supporting Code Evidence: `Assets/Ashfall.Core/Endgame/EpilogueChronicleCatalog.cs`

### `Assets/Ashfall.Core/Endgame/EpilogueChronicleCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 89 lines / 2875 bytes.
- SHA-256: `658a2091baffeb12e5be3a6d5f631a4bd3b79cf0194a1c3aa2da1db64953c126`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class EpilogueSlideDefinition
public int order { get; set; }
public string title { get; set; } = string.Empty;
public string art_asset_id { get; set; } = string.Empty;
public EpilogueSlide ToSlide(string prose = "") {
public sealed class EpilogueChronicleCatalogData
public int schema_version { get; set; } = 1;
public List<EpilogueSlideDefinition> default_slides { get; set; } = new List<EpilogueSlideDefinition>();
public static class EpilogueChronicleLoader
public const string DefaultFileName = "epilogue_chronicle.json";
public static EpilogueChronicleCatalogData? Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static List<EpilogueSlide> LoadDefaultSlides(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
```


# Appendix E.11 — Supporting Code Evidence: `Assets/Ashfall.Core/Muster/EpilogueMatrix.cs`

### `Assets/Ashfall.Core/Muster/EpilogueMatrix.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 218 lines / 8297 bytes.
- SHA-256: `52d5e9b662f42660a64b545333f38a51bc06e7af6731edce1dd7411400081884`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class EndingDefinition
public string endingKey = string.Empty;
public string title = string.Empty;
public string prose = string.Empty;
public static class EpilogueMatrixLoader
public const string FileName = "muster_epilogues.json";
public const int CurrentSchemaVersion = 1;
public static List<EndingDefinition> LoadEpilogues( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public int schema_version = 1;
public List<EndingEntry> epilogues = new List<EndingEntry>();
public string ending_key;
public string title;
public string prose;
public enum FactionTerminalOutcome
public sealed class EpilogueMatrixInput
public bool ShelterFallen { get; set; }
public bool WaterPlantHeld { get; set; }
public bool GrainSiloCaptured { get; set; }
public bool FuelDepotBurned { get; set; }
public bool MercyPattern { get; set; }
public bool IronPattern { get; set; }
public bool DiplomacyPattern { get; set; }
public string VerdictEndingKey { get; set; } = string.Empty;
public string MusterEndingKey { get; set; } = string.Empty;
public FactionTerminalOutcome FactionOutcome { get; set; } = FactionTerminalOutcome.None;
public static class EpilogueMatrix
public const string TheOpenMuster = "the_open_muster";
public const string TheAmnesty = "the_amnesty";
public const string TheCorridor = "the_corridor";
public const string TheBloodPrice = "the_blood_price";
public const string TheRateCardRevised = "the_rate_card_revised";
public const string TheAdministrator = "the_administrator";
public const string TheMeasuredTruthContested = "the_measured_truth_contested";
public const string TheMeasuredTruth = "the_measured_truth";
public const string Unwritten = "unwritten";
public const string VerdictSectorRecounts = "ending_verdict_the_sector_recounts";
public const string VerdictCountHeld = "ending_verdict_the_count_is_held";
public const string VerdictOfferLease = "ending_verdict_the_offer_is_a_lease";
public const string GarrisonAbsorbsCoalition = "ending_garrison_absorbs_coalition";
public const string RebuildersJoined = "ending_rebuilders_joined";
public const string CoalitionIndependent = "ending_coalition_independent";
public const string FoundryAnnexation = "ending_foundry_annexation";
public const string WaterPlantHeld = "ending_water_plant_held";
public const string GrainSiloCaptured = "ending_grain_silo_captured";
public const string FuelDepotBurned = "ending_fuel_depot_burned";
public const string MercyRoad = "ending_mercy_road";
public const string IronWay = "ending_iron_way";
public const string ListenersThread = "ending_listeners_thread";
public const string MercyWaterHeld = "ending_mercy_water_held";
public const string IronFuelAsh = "ending_iron_fuel_ash";
public const string ShelterFalls = "ending_shelter_falls";
public static readonly string[] AllKeys = {
public static string Evaluate(EpilogueMatrixInput? input) {
```


# Appendix E.12 — Supporting Code Evidence: `Assets/Ashfall.Core/Campaign/CampaignEpilogueCatalog.cs`

### `Assets/Ashfall.Core/Campaign/CampaignEpilogueCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 73 lines / 2592 bytes.
- SHA-256: `cc8229ed5d5f3b1796c149f26fffe6f7759511405cbabc11811e3e1702dc07e8`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class EpilogueVignetteDef
public string id { get; set; } = string.Empty;
public string category { get; set; } = string.Empty;
public int priority { get; set; } = 1;
public int min_survivors { get; set; } = 0;
public int max_survivors { get; set; } = 999;
public int min_paroled_captives { get; set; } = 0;
public int max_penal_shifts { get; set; } = 999;
public int min_archives_decrypted { get; set; } = 0;
public int min_starvation_deaths { get; set; } = 0;
public int max_starvation_deaths { get; set; } = 999;
public string title { get; set; } = string.Empty;
public string narrative { get; set; } = string.Empty;
public List<string> tags { get; set; } = new List<string>();
public sealed class CampaignEpilogueCatalog
public int schema_version { get; set; } = 1;
public List<EpilogueVignetteDef> vignettes { get; set; } = new List<EpilogueVignetteDef>();
public void Index() {
public EpilogueVignetteDef? GetVignette(string id) {
public IReadOnlyCollection<EpilogueVignetteDef> GetAllVignettes() => vignettes;
public static class CampaignEpilogueCatalogLoader
public static CampaignEpilogueCatalog Load(string dataDir, IFileIO fileIo) {
```


# Appendix E.13 — Supporting Code Evidence: `Assets/Ashfall.Core/Campaign/CampaignEpilogueEngine.cs`

### `Assets/Ashfall.Core/Campaign/CampaignEpilogueEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 220 lines / 8621 bytes.
- SHA-256: `c2d142ac86ed87cebfcc9c2c1d8bf85504859f923f3c124315df6862f92851f8`.
- Architecture signals: seeded references=2; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CampaignEpilogueSnapshot
public int FinalDay { get; set; }
public int SurvivorsAlive { get; set; }
public int TotalCasualties { get; set; }
public int StarvationDeaths { get; set; }
public int DiseaseDeaths { get; set; }
public int ArchivesDecrypted { get; set; }
public int TechNodesCompleted { get; set; }
public int CaptivesParoled { get; set; }
public int CaptivesInterrogated { get; set; }
public int PenalLaborShiftsRun { get; set; }
public float AverageFreshnessConsumed { get; set; }
public Dictionary<string, int> FactionStandings { get; set; } = new Dictionary<string, int>();
public List<string> HistoricDecisions { get; set; } = new List<string>();
public ulong CampaignSeed { get; set; } = 42;
public sealed class EpilogueChapter
public string Category { get; set; } = string.Empty;
public string Title { get; set; } = string.Empty;
public string NarrativeText { get; set; } = string.Empty;
public List<string> Highlights { get; set; } = new List<string>();
public sealed class EpilogueChronicle
public string CampaignId { get; set; } = "ASHFALL_CAMPAIGN";
public int TotalDays { get; set; }
public List<EpilogueChapter> Chapters { get; set; } = new List<EpilogueChapter>();
public CampaignEpilogueSnapshot FinalMetrics { get; set; } = new CampaignEpilogueSnapshot();
public string ToJson() {
public string ToFormattedReport() {
public sealed class CampaignEpilogueEngine
public EpilogueChronicle GenerateChronicle(CampaignEpilogueSnapshot snapshot) {
```


# Appendix E.14 — Supporting Code Evidence: `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs`

### `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 125 lines / 4211 bytes.
- SHA-256: `2f9ae2fa6d907c42c54ab2217631b7b9bd348c527be41fd6252334a500ab9358`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class EpilogueChronicleBuilder
public EpilogueChronicle Build(EpilogueChronicleInput input) {
public sealed class EpilogueChronicleInput
public string EndingKey;
public int Day;
public int BuildSeed;
public List<EpilogueSlide> Slides;
public List<SurvivorFateCard> FateCards;
public List<EpilogueMetric> Metrics;
public sealed class EpilogueChronicle
public string EndingKey;
public string Title;
public int GeneratedDay;
public int BuildSeed;
public List<EpilogueSlide> Slides = new List<EpilogueSlide>();
public List<SurvivorFateCard> FateCards = new List<SurvivorFateCard>();
public List<EpilogueMetric> Metrics = new List<EpilogueMetric>();
public sealed class EpilogueSlide
public int Order;
public string Title;
public string Prose;
public string ArtAssetId;
public sealed class SurvivorFateCard
public string SurvivorId;
public string DisplayName;
public string Fate;
public bool Survived;
public sealed class EpilogueMetric
public string MetricId;
public float Value;
public string DisplayLabel;
```


# Appendix G.15 — Supporting Regression Evidence: `Ashfall.Core.Tests/YearOfAshTests.cs`

### `Ashfall.Core.Tests/YearOfAshTests.cs`

- Current test declarations: Fact=26, Theory=0, InlineData=0.
- File lines: 803; SHA-256: `47f8258fe2617608a3078688d92411d22003c93f181e2c8f6bf6115d540b1adb`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Timeline_AdvancesThroughAllThreePhases
Timeline_IgnoresRepeatedAndOutOfOrderDays
Timeline_RestoreDerivesPhaseFromAuthoritativeDay
DoorEncounters_EvaluatesHumanistVsRuthlessReactionsDeterministically
DoorEncounters_TraumaBondDampensNegativeMoraleImpact
FactionWar_ModifiesStandingAndEnactsDecrees
YearOfAshSave_CapturesAndEncodesDeterministicChecksum
YearOfAshSave_Restore_RebuildsTimelineEncountersAndFactionWar
DefaultFactionRoster_IncludesForwardRoster
YearOfAshSave_V4_CapturesAndRestoresChainRunnerProgress
YearOfAshSave_V3Envelope_MigratesWithFreshChainRunner
RestoreState_WithNullSections_IsANoOp
DoorEncounterCatalogLoader_LoadsJsonEntriesCorrectly
YearOfAshCatalogLoader_LoadsItemsEventsAndQuestsCorrectly
FactionWar_SimulatesDailyFrictionCorrectly
YearOfAshCatalogLoader_LoadsLocationsRadioAndSurvivorsCorrectly
YearOfAshRadonSystem_SimulatesThawInfiltrationAndScrubberReplacement
YearOfAshDeepFreezeSystem_SimulatesSubZeroThermalBalanceAndIntakeIcing
YearOfAshSave_Roundtrip_PreservesDeepFreezeAndRadon
YearOfAshSave_Roundtrip_PreservesQuestlineProgress
YearOfAshSave_V1File_MigratesToCurrentWithFreshSections
GetPlayableQuestlines_NeverOffersAnUnadvanceableQuestline
WithheldQuestlineCount_ReportsTheUnauthoredContentGap
LegacyQuestConverter_ProducesPlayableQuestlines
LegacyQuestConverter_LinearTraversal_ReachesTerminal
VerdictLocations_LoadAndAreQueryable
```


# Appendix G.16 — Supporting Regression Evidence: `Ashfall.Core.Tests/Campaign/CampaignEpilogueEngineTests.cs`

### `Ashfall.Core.Tests/Campaign/CampaignEpilogueEngineTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 195; SHA-256: `a14260aa46c214df06369284f4724f0ab99750f1a070d555a1c33d73cade3e7e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
GenerateChronicle_ThrivingScenario
GenerateChronicle_DesolationScenario
GenerateChronicle_DeterministicReplay
FormattedReport_ContainsExpectedHeaderAndChapters
```


# Appendix G.17 — Supporting Regression Evidence: `Ashfall.Core.Tests/Endgame/EpilogueChronicleBuilderTests.cs`

### `Ashfall.Core.Tests/Endgame/EpilogueChronicleBuilderTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 147; SHA-256: `6f99bb4cc40c06f31bcbe94f4a3765fbf0fc184babdf92ba7beb2e4a3b48d9e9`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Build_ProducesTitleForKnownEnding
Build_SortsSlidesByOrder
Build_SortsFateCardsBySurvivorId
Build_SortsMetricsByMetricId
Build_DeterministicForSameInput
Build_HandlesEmptyInput
Build_UnknownEndingFallsBackToKey
```


# Appendix G.18 — Supporting Regression Evidence: `Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs`

### `Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 243; SHA-256: `55d27dae68cedb1c691bce71b366ee18a77e69825b85d73bac2e88ee708855d5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsSuccessfully_WithSchemaVersionOne
SlideCount_ContainsExactlyTwentySlides
Parity_BaselineFiveSlidesArePreserved
SlideOrders_AreUniqueAndSequentialZeroToNineteen
SlideTitles_AreNonEmptyAndConciseOneToFourWords
ArtAssetIds_FollowPlaceholderGrammarAndAreUnique
SemanticCollisions_NoRedundantOverlappingRoles
PillarCoverage_SpansAllMajorCampaignThemes
BuilderIntegration_SortsAllTwentySlidesDeterministically
Plan89Bindings_OutcomeMappingTable_MapsToRelevantSlides
```


# Appendix H.19 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

### `docs/CURRENT_AUTHORITY.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 102 lines / 9950 bytes.
- SHA-256: `7dea2c12b4863bfc9a3c2ebb161ba51512ebc06475b762abd5d5d205bef47e5c`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=0; textual Godot mentions=6; textual Unity/JsonUtility mentions=2; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| parse-failure visibility | CatalogDiagnostics | Year of Ash shape fallback | YearOfAshCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| parse-failure visibility | CatalogDiagnostics | Verdict catalog shape loading | VerdictCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| parse-failure visibility | CatalogDiagnostics | failure observability and parity | Catalog loader tests | Owner emits/reads a typed fact; no mirror state. |
| Year of Ash shape fallback | YearOfAshCatalogLoader | parse-failure visibility | CatalogDiagnostics | Owner emits/reads a typed fact; no mirror state. |
| Year of Ash shape fallback | YearOfAshCatalogLoader | Verdict catalog shape loading | VerdictCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| Year of Ash shape fallback | YearOfAshCatalogLoader | failure observability and parity | Catalog loader tests | Owner emits/reads a typed fact; no mirror state. |
| Verdict catalog shape loading | VerdictCatalogLoader | parse-failure visibility | CatalogDiagnostics | Owner emits/reads a typed fact; no mirror state. |
| Verdict catalog shape loading | VerdictCatalogLoader | Year of Ash shape fallback | YearOfAshCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| Verdict catalog shape loading | VerdictCatalogLoader | failure observability and parity | Catalog loader tests | Owner emits/reads a typed fact; no mirror state. |
| failure observability and parity | Catalog loader tests | parse-failure visibility | CatalogDiagnostics | Owner emits/reads a typed fact; no mirror state. |
| failure observability and parity | Catalog loader tests | Year of Ash shape fallback | YearOfAshCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| failure observability and parity | Catalog loader tests | Verdict catalog shape loading | VerdictCatalogLoader | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the obsolete bare-catch count with a current loader-policy matrix. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Distinguish required, optional, malformed-present and malformed-fallback cases. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Add sink-failure isolation proof for `CatalogDiagnostics.Warn`. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Require new loaders to use the shared diagnostic path rather than console output or silent fallback. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

Every requirement in the objective and delta must resolve to at least one current owner, one negative condition and one future focused verification. A requirement with no owner is removed or returned as `STALE_PLAN`.

# Appendix K — Status and Evidence Labels

| Claim type | Label | Meaning |
| --- | --- | --- |
| Current source | VERIFIED PATH INVENTORY | Appendices hash current files; declarations are read-only. |
| Historical completion | HISTORICAL, NOT FRESH TEST PROOF | Focused tests must be rerun by an implementation/verification package. |
| Proposed types/paths | PROPOSAL ONLY | Never shown as current evidence; requires a new claim. |
| Character count | COMPLETENESS CHECK ONLY | External verifier records it; no padding or repeated boilerplate. |

# Appendix L — Plan Maintenance and Re-Audit Triggers

Re-run the premise sweep when any of the following occurs:

1. A listed Core owner is renamed, split, merged or removed.
2. A listed catalog changes `schema_version`, root shape or consumer.
3. A save section, checksum contract or campaign-day order changes.
4. A listed test is removed, renamed or moved to quarantine.
5. A live ledger marks a surface sealed, retired, accepted or blocked.
6. A generated architecture/save/catalog matrix changes the owner relationship.
7. A new active claim touches any current or proposed path.

The re-audit records only changed evidence. Historical prose is not rewritten merely to appear current, and current evidence is not deleted merely because an old plan disagrees with it.

# Appendix M — Definition of a Safe No-Change Result

A safe no-change result is valid when the current implementation already satisfies the requested behavior. It records: current owner paths, focused tests that exist, any rerun performed by a future verification package, and the precise condition that would justify reopening. A no-change result does not create a placeholder subsystem, a synthetic integration framework, or a test solely to increase counts.

# Appendix N — Final Precision Checklist

- [ ] Every current path in this document exists or is explicitly labeled unavailable.
- [ ] Every proposed path/type is labeled `PROPOSAL` and excluded from current claims.
- [ ] No current owner is duplicated.
- [ ] Core architecture remains engine-free.
- [ ] JSON is described as data authority, not automatic reachability.
- [ ] Save impact names the current owner and migration behavior.
- [ ] Determinism names streams or explicitly states no randomness.
- [ ] UI remains presentation over owner commands.
- [ ] Tests are focused and current commands use `scripts/run_test.sh` policy.
- [ ] Sealed/retired/blocked ledger decisions are respected.
- [ ] No full-suite result is claimed without a dedicated execution window.
- [ ] No Unity dependency or historical architecture is proposed.
- [ ] Character count is not used as evidence of quality.
- [ ] The first implementation step is a premise recheck, not code creation.
- [ ] The implementation handoff can be executed without reinterpreting ownership.

# Appendix O — Handoff Record Template

```text
Package:
Current status rechecked:
Outcome implemented:
Files changed:
Current owner contract used:
Save section/version touched:
Determinism streams touched:
Focused verification commands and results:
Tests reused/added:
Known limitation or debt:
Shared files intentionally untouched:
Ready for independent sweep: yes/no
```


# Appendix Q.551 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Endgame/EpilogueContextFactory.cs`

### `Assets/Ashfall.Core/Endgame/EpilogueContextFactory.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 60 lines / 2591 bytes.
- SHA-256: `a9844428837f68851804ab2726890aee3b813d33d193867734c2cb4e9ab02707`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed record EpilogueContextInputs(
public static class EpilogueContextFactory
public static EpilogueEvaluationContext Build(EpilogueContextInputs inputs) {
public static CampaignOutcomeSnapshot CreateSnapshot(CampaignOutcomeEvaluationInput input) => CampaignOutcomeEvaluator.Evaluate(input);
public static EpilogueEvaluationContext CreateContext(CampaignOutcomeEvaluationInput input) => CampaignOutcomeEvaluator.Evaluate(input).ToContext();
public static EpilogueEvaluationContext CreateContext(CampaignOutcomeSnapshot snapshot) {
```


# Appendix Q.552 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs`

### `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 150 lines / 7356 bytes.
- SHA-256: `80b3302452337fc1724da033628414aaa6fc1c9909fe341c6539d89c6758f6b4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum RegionalFate { CommonwealthFounded, GarrisonMartialLaw, FracturedWarlords, TempestSterilization, TrueReconciliation } public enum DemographicOutcome { ThrivingCommunity, HardenedSurvivors, GhostShelter, TotalExtinction }
public enum MoralStanding { ForgivenAndReconciled, IndenturedDebtState, RuthlessPragmatists } [Serializable] public sealed class EpilogueEvaluationContext { public int totalDaysSurvived; public int livingDwellerCount; public int totalDeathsRecorded; public bool grandTreatySigned; public bool tempestDecommissioned; public bool debtLedgersBurned; public bool childrenSurvived; public bool velSecretExposed; }
public sealed class EpilogueMatrixRuntime
public RegionalFate EvaluateRegionalFate(EpilogueEvaluationContext ctx) {
public DemographicOutcome EvaluateDemographics(EpilogueEvaluationContext ctx) {
public MoralStanding EvaluateMoralStanding(EpilogueEvaluationContext ctx) {
public string GenerateEpilogueNarrative(EpilogueEvaluationContext ctx) {
```


# Appendix Q.553 — Additional Current Architecture Evidence: `src/Host/VerdictHostSession.cs`

### `src/Host/VerdictHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 284 lines / 14368 bytes.
- SHA-256: `eb9abc73b3557cdfa1d7b975f426399da4cbf2e299387681232521df4b1f6f8b`.
- Architecture signals: seeded references=4; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class VerdictHostSession
public MachineLogSystem MachineLog { get; }
public ReckoningSystem Reckoning { get; }
public EvidenceLedger Evidence { get; }
public VerdictEvidenceChain EvidenceChain { get; }
public VerdictNpcSystem Npcs { get; }
public VerdictCensusBroadcast Census { get; }
public VerdictRadioSystem Radio { get; internal set; }
public QuestlineSystem Quests { get; }
public IReadOnlyList<VerdictCatalogLoader.VerdictLocationEntry> Locations { get; }
public IReadOnlyList<VerdictCatalogLoader.VerdictItemEntry> Items { get; }
public IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> RadioEntries { get; }
public IReadOnlyList<string> CorruptionCorpus { get; private set; }
public System.Collections.Generic.HashSet<string> MaterializedNpcFlags() {
public System.Collections.Generic.List<Ashfall.Core.Verdict.VerdictNpcEntry> AvailableNpcs(string locationId = null!) {
public string LastEvent { get; private set; } = string.Empty;
public static VerdictHostSession Create( string dataDir, ISimClock clock = null!, IEventBus bus = null!, IFlagLedger flags = null!, ISeededRng radioRng = null!,
public int LoadedSaveVersion { get; private set; }
public bool WasSaveMigrated { get; private set; }
public void AdvanceDay(int day, int livingCount, int logReadCount) {
public void TickCensus() {
public System.Collections.Generic.List<string> TickRadio(int day) {
public int EnrollEvidenceFromItems(int day) {
public void TickCorruption(int day) {
public VerdictSave CaptureSave() {
public void RestoreSave(VerdictSave save) {
public string StatusLine() {
public VerdictCatalogLoader.VerdictLocationEntry? FindLocation(string id) {
```


# Appendix Q.554 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

### `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 2073 lines / 130366 bytes.
- SHA-256: `7588dbb7ed053936964371ce06c49160f772cb9fffd2e7d519884ab430f044c4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ContentUtilizationScanner
public static bool IsNarrativeSubdirectoryFile(string relativePath) {
public static bool IsAuthoritativeCatalog(string fileName) {
public ContentUtilizationGraph Scan() {
```


# Appendix Q.555 — Additional Current Architecture Evidence: `src/UI/EpiloguePanel.cs`

### `src/UI/EpiloguePanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 251 lines / 10695 bytes.
- SHA-256: `40e17093add485b4ca0e323093811deeabc7afdee6e5751975926c2ead73c9ba`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class EpiloguePanel : Control
public event Action? OnClose;
public override void _Ready() {
public override void _UnhandledInput(InputEvent @event) {
public void Bind(CampaignOutcomeSnapshot snapshot) {
public void Bind(UnifiedEndingResult result) {
public void Bind(EpilogueEvaluationContext context) {
public void Open() {
public void Close() {
public void RefreshView() {
```


# Appendix Q.556 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/RadioBroadcastCatalog.cs`

### `Assets/Ashfall.Core/Radio/RadioBroadcastCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 678 lines / 36957 bytes.
- SHA-256: `974aed9a7508a15773166e11c7fc336b5e1874935513b16cf99582742b686737`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RadioBroadcastCatalog
public IReadOnlyList<UnifiedRadioBroadcast> AllBroadcasts => _allBroadcasts;
public int TotalCount => _allBroadcasts.Count;
public void Register(UnifiedRadioBroadcast b) {
public UnifiedRadioBroadcast? GetById(string broadcastId) {
public List<UnifiedRadioBroadcast> GetByFrequency(float freqMhz, float toleranceMhz = 0.5f) {
public List<UnifiedRadioBroadcast> GetEligibleBroadcasts(float freqMhz, int day, float toleranceMhz = 0.5f) {
public int LoadFromDataDirectory(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public int LoadFactionRadioCorpusJson(string json) {
public void LoadBaseRadioJson(string json) {
public void LoadYearOfAshRadioJson(string json) {
public void LoadVerdictRadioJson(string json) {
public void LoadFactionWarRadioJson(string json) {
public void RegisterAuthoredGapBroadcasts() {
```


# Appendix Q.557 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs`

### `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 114 lines / 4583 bytes.
- SHA-256: `fb560fa3710ed2ac870f7e1cf16f5dd087e316eb26ffc60d4cd6bc4be960f5db`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class VerdictRadioSystem
public const string SystemId = "verdict_radio_system";
public const int CarrierOpenDay = 210;
public IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> Corpus => _corpus;
public bool HasFired(string id) => _firedIds.Contains(id);
public int FiredCount => _firedIds.Count;
public System.Collections.Generic.List<string> Poll(int day, ReckoningPhase phase) {
public int LoadFrom(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public class VerdictRadioState
public string systemId = SystemId;
public List<string> firedIds = new List<string>();
public VerdictRadioState CaptureState() {
public void RestoreState(VerdictRadioState state) {
```


# Appendix Q.558 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Economy/TradeTextCatalog.cs`

### `Assets/Ashfall.Core/Economy/TradeTextCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 716 lines / 31890 bytes.
- SHA-256: `5de72b6a769d97e144eed6ea2fd26a9697dc6e0cbdc25d361ff5fb3c7bb6b56a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TradeTextTraderDefinition
public string id = string.Empty;
public string display_name = string.Empty;
public string profile = string.Empty;
public Dictionary<string, string> greetings = new(StringComparer.Ordinal);
public Dictionary<string, string> item_examinations = new(StringComparer.Ordinal);
public Dictionary<string, string> offers = new(StringComparer.Ordinal);
public Dictionary<string, string> counter_offers = new(StringComparer.Ordinal);
public Dictionary<string, string> acceptance = new(StringComparer.Ordinal);
public Dictionary<string, string> rejection = new(StringComparer.Ordinal);
public Dictionary<string, string> regret = new(StringComparer.Ordinal);
public Dictionary<string, string> insult = new(StringComparer.Ordinal);
public Dictionary<string, string> flattery = new(StringComparer.Ordinal);
public Dictionary<string, string> threat = new(StringComparer.Ordinal);
public sealed class TradeTextScenarioDefinition
public string description = string.Empty;
public string trader_text = string.Empty;
public string player_text = string.Empty;
public sealed class TradeTextCatalogFile
public int schema_version;
public string collection_id = string.Empty;
public List<TradeTextTraderDefinition> traders = new();
public Dictionary<string, TradeTextScenarioDefinition> trade_scenarios = new(StringComparer.Ordinal);
public sealed class TradeTextCatalog
public int SchemaVersion { get; }
public string CollectionId { get; }
public bool IsFallback { get; }
public int TraderCount => _traders.Count;
public int ScenarioCount => _scenarios.Count;
public IReadOnlyDictionary<string, TradeTextTraderDefinition> Traders => _traders;
public IReadOnlyDictionary<string, TradeTextScenarioDefinition> Scenarios => _scenarios;
public bool TryGetTrader(string id, out TradeTextTraderDefinition trader) =>
public bool TryGetScenario(string id, out TradeTextScenarioDefinition scenario) =>
public sealed class TradeTextCatalogLoadResult
public TradeTextCatalog Catalog { get; }
public IReadOnlyList<string> Errors { get; }
public bool LoadedFromFile { get; }
public bool UsedFallback => Catalog.IsFallback;
public bool IsValid => Errors.Count == 0 && !Catalog.IsFallback;
public static class TradeTextCatalogLoader
public const string FileName = "trade_texts.json";
public const int SupportedSchemaVersion = 1;
public const string CollectionId = "trade_texts";
public static TradeTextCatalogLoadResult Load( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static TradeTextCatalogLoadResult LoadFromJson(string json) {
public static TradeTextCatalog CreateFallbackCatalog() {
public enum TradeVoiceLineFamily
public enum TradeVoiceItemReaction
public sealed class TradeVoiceContext
public string TraderProfileId { get; set; } = string.Empty;
public string ScenarioId { get; set; } = string.Empty;
public string FactionId { get; set; } = string.Empty;
public string CaravanId { get; set; } = string.Empty;
public string CaravanOriginRegion { get; set; } = string.Empty;
public string SpecialtyId { get; set; } = string.Empty;
public TradeStance Stance { get; set; } = TradeStance.Trade;
public float Trust { get; set; }
public string StableContextKey { get; set; } = string.Empty;
public sealed class TradeVoiceResult
public string ProfileId { get; }
public string Band { get; }
public string Text { get; }
public bool UsedFallback { get; }
public sealed class TradeVoiceResolver
public const string DefaultProfileId = "trader_merchant";
public TradeTextCatalog Catalog => _catalog;
public string ResolveProfileId(TradeVoiceContext? context) {
public TradeVoiceResult ResolveGreeting(TradeVoiceContext? context) {
public TradeVoiceResult ResolveItemExamination( TradeVoiceContext? context, TradeVoiceItemReaction reaction, string itemDisplayName) {
public TradeVoiceResult ResolveLine( TradeVoiceContext? context, TradeVoiceLineFamily family, string variant, params string[] itemDisplayNames) {
public TradeVoiceResult ResolveScenarioTraderText(TradeVoiceContext? context, string scenarioId) {
public static string BandForTrust(float trust) {
public static string SanitizeDisplayName(string? displayName) {
```


# Appendix Q.559 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/IO/CatalogLoadResult.cs`

### `Assets/Ashfall.Core/IO/CatalogLoadResult.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 350 lines / 14156 bytes.
- SHA-256: `aef5715d93ad3c484169fb6d9c3c75880fff56d9db7beee9d3f3274b282dad10`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum CatalogClassification
public enum CatalogLoadSeverity
public readonly struct CatalogLoadMessage
public CatalogLoadSeverity Severity { get; }
public string FilePath { get; }
public string Shape { get; }
public string Message { get; }
public Exception? Exception { get; }
public override string ToString() =>
public class CatalogLoadResult<T>
public string FilePath { get; private set; } = "<unknown>";
public string Schema { get; private set; } = "<unknown>";
public int SchemaVersion { get; private set; }
public int EntryCount => _entries.Count;
public IReadOnlyList<T> Entries => _entries.AsReadOnly();
public CatalogClassification Classification { get; private set; } = CatalogClassification.Optional;
public IReadOnlyList<CatalogLoadMessage> Messages => _messages.AsReadOnly();
public bool HasFatalErrors => HasMessagesOfSeverity(CatalogLoadSeverity.Fatal);
public bool HasErrors => HasMessagesOfSeverity(CatalogLoadSeverity.Error) || HasFatalErrors;
public bool HasWarnings => HasMessagesOfSeverity(CatalogLoadSeverity.Warning);
public bool IsSuccess => !HasErrors && !HasWarnings;
public static CatalogLoadResult<T> Success( string filePath, string schema, IReadOnlyList<T> entries, int schemaVersion = 1, CatalogClassification classification = CatalogClassification.Optional)
public static CatalogLoadResult<T> Fail( string filePath, string schema, string message, Exception? exception = null, CatalogClassification classification = CatalogClassification.Optional)
public void AddEntry(T entry) => _entries.Add(entry);
public void AddEntries(IEnumerable<T> entries) {
public void AddMessage( CatalogLoadSeverity severity, string filePath, string shape, string message, Exception? exception = null)
public void AddInfo(string message) =>
public void AddWarning(string message, Exception? ex = null) =>
public void AddError(string message, Exception? ex = null) =>
public void AddFatal(string message, Exception? ex = null) =>
public void SetSchemaVersion(int version) => SchemaVersion = version;
public void ThrowIfFatal() {
public static CatalogLoadResult<T> FromFile( string filePath, string schema, CatalogClassification classification, Func<string, IJsonSerializer, T> deserializer, IJsonSerializer json)
public static CatalogLoadResult<T> FromWrappedListFile( string filePath, string schema, CatalogClassification classification, IJsonSerializer json) {
```


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs`

### `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 782 lines / 31079 bytes.
- SHA-256: `1c3897163c7cd9260c589b5d055f69aeed910b69cbe318aa629523bb5aa5f424`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
internal sealed class ItemJsonDto
public string id { get; set; } = string.Empty;
public string displayName { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public string iconPath { get; set; } = string.Empty;
public string type { get; set; } = string.Empty;
public int stackMax { get; set; } = 1;
public float weight { get; set; }
public float radProtection { get; set; }
public float durability { get; set; }
public float degradeRate { get; set; }
public float degrade_rate { get; set; }
public bool isEquipable { get; set; }
public string equipSlot { get; set; } = string.Empty;
public float contamination { get; set; }
public float hungerRestore { get; set; }
public float thirstRestore { get; set; }
public float healthEffect { get; set; }
public float radCleanse { get; set; }
public float moraleEffect { get; set; }
public float decorLocalizedMoraleDelta { get; set; }
public bool empShielded { get; set; }
public float tradeValue { get; set; }
public int tradeTier { get; set; }
public float disassembleYieldFraction { get; set; } = 0.5f;
public List<string>? tags { get; set; }
public List<ScrapYieldDto>? scrapValue { get; set; }
public RepairRecipeDto? repairRecipe { get; set; }
public LimbRequirementDto? limbRequirements { get; set; }
public LimbRequirementDto? limb_requirements { get; set; }
public LimbProvisionDto? providesLimb { get; set; }
public LimbProvisionDto? provides_limb { get; set; }
internal sealed class LimbRequirementDto
public int hands { get; set; } = 1;
public string? gripClass { get; set; }
public string? grip_class { get; set; }
internal sealed class LimbProvisionDto
public int hands { get; set; }
public int legs { get; set; }
public int qualityPermille { get; set; } = 500;
public int quality_permille { get; set; } = 500;
public string? gripClass { get; set; }
public string? grip_class { get; set; }
internal sealed class ScrapYieldDto
public string materialId { get; set; } = string.Empty;
public int amount { get; set; } = 1;
internal sealed class RepairRecipeDto
public List<ScrapYieldDto>? costs { get; set; }
public float hours { get; set; } = 0.5f;
public bool requiresTools { get; set; } = true;
public float max_repair_condition_fraction { get; set; } = 1.0f;
internal sealed class StartingSupplyJsonDto
public string itemId { get; set; } = string.Empty;
public int amount { get; set; } = 1;
internal sealed class StartingSuppliesProfileJsonDto
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public List<StartingSupplyJsonDto> supplies { get; set; } =
internal sealed class StartingSuppliesRootJsonDto
public int schema_version { get; set; } = 1;
public string default_profile_id { get; set; } = string.Empty;
public List<StartingSupplyJsonDto>? starting_supplies { get; set; }
public List<StartingSuppliesProfileJsonDto>? profiles { get; set; }
public enum StartingSuppliesLoadStatus
public sealed class StartingSuppliesLoadResult
public StartingSuppliesLoadStatus Status { get; set; } = StartingSuppliesLoadStatus.Success;
public string ErrorMessage { get; set; } = string.Empty;
public string SelectedProfileId { get; set; } = StartingSuppliesCatalog.StandardProfileId;
public int AcceptedRowCount => Supplies.Count;
public bool IsSuccess => Status == StartingSuppliesLoadStatus.Success;
public sealed class StartingSuppliesCatalogLoadResult
public StartingSuppliesCatalog Catalog { get; internal set; } =
public List<string> Errors { get; } = new List<string>();
public List<string> Warnings { get; } = new List<string>();
public bool UsedLegacyFallback { get; internal set; }
public bool IsUsable => Catalog.Profiles.Count > 0;
public static class ItemCatalogLoader
public const string PrimaryFileName = "items.json";
public const string StartingSuppliesFileName = "starting_supplies.json";
public const string ItemDescriptionsFileName = ItemDescriptionCatalogLoader.PrimaryFileName;
public static ItemCatalog LoadCatalog(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static List<ItemDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static ItemDescriptionCatalog LoadDescriptionCatalog(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static CatalogLoadResult<ItemDescriptionCatalog> LoadDescriptionCatalogWithResult(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static CatalogLoadResult<ItemCatalog> LoadCatalogWithResult( string dataDir, IFileIO fileIO, IJsonSerializer serializer, ItemCatalog? targetCatalog = null) {
public static void LoadInto(ItemCatalog catalog, string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static StartingSuppliesCatalog LoadStartingSuppliesCatalog( string dataDir, IFileIO fileIO, IJsonSerializer serializer, ItemCatalog? catalog = null) {
public static StartingSuppliesCatalogLoadResult LoadStartingSuppliesCatalogDetailed( string dataDir, IFileIO fileIO, IJsonSerializer serializer, ItemCatalog? catalog = null) {
public static StartingSuppliesLoadResult LoadStartingSuppliesDetailed( string dataDir, IFileIO fileIO, IJsonSerializer serializer, ItemCatalog? catalog = null, string? profileId = null)
internal static ItemDefinition ConvertDto(ItemJsonDto dto) {
```


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Shelter/ShelterPowerGridCatalog.cs`

### `Assets/Ashfall.Core/Shelter/ShelterPowerGridCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 198 lines / 12984 bytes.
- SHA-256: `a1cb17a7d251b86599a5167a0df13aea25500a8c6c0a401ea3f811f36e64853f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterPowerGridRoomDef
public sealed class ShelterPowerGridCatalogDef
public float? EmpStormSeverity { get; set; }
public float? SurgeBatteryDrainFraction { get; set; }
public static class ShelterPowerGridCatalogLoader
public const string FileName = "power_grid.json";
public const int SupportedSchemaVersion = 1;
public static bool TryLoad(string dataDir, IFileIO files, IJsonSerializer serializer, out ShelterPowerGridCatalogDef? catalog, out string error) {
public static ShelterPowerGridCatalogDef LoadOrDefault(string dataDir, IFileIO files, IJsonSerializer serializer) {
public static bool Validate(ShelterPowerGridCatalogDef catalog, out string error) {
public static PowerGridRoomPriority? MapPriority(string roomId, string priority) {
public static ShelterPowerGridCatalogDef FallbackDefault() => new ShelterPowerGridCatalogDef
```


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DoseContentCatalog.cs`

### `Assets/Ashfall.Core/DoseContentCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 223 lines / 8490 bytes.
- SHA-256: `0dfe989872898d06f0482b1b234da837a59eb8f9e8ec76e6263cf30f72429935`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class DoseLocationDef
public string id = string.Empty;
public string displayName = string.Empty;
public string sector = string.Empty;
public int riskLevel;
public float radiationUsv;
public string description = string.Empty;
public class DoseItemDef
public string id = string.Empty;
public string name = string.Empty;
public float weightKg;
public float tradeValue;
public string category = string.Empty;
public string description = string.Empty;
public class DoseQuestDef
public string questlineId = string.Empty;
public string title = string.Empty;
public string synopsis = string.Empty;
public string factionTag = string.Empty;
public int minDay = 40;
public int maxDay = 360;
public List<DoseQuestStage> stages = new List<DoseQuestStage>();
public class DoseQuestStage
public string stageId = string.Empty;
public string title = string.Empty;
public string narrativePrompt = string.Empty;
public bool isTerminal;
public List<DoseQuestChoice> choices = new List<DoseQuestChoice>();
public class DoseQuestChoice
public string choiceId = string.Empty;
public string text = string.Empty;
public string nextStageId = string.Empty;
public int moraleDelta;
public int guiltDelta;
public string grantItemId = string.Empty;
public int grantItemQuantity;
public string outcomeNarrative = string.Empty;
public class DoseContentCatalog
public List<DoseLocationDef> locations = new List<DoseLocationDef>();
public List<DoseItemDef> items = new List<DoseItemDef>();
public List<QuestlineDefinition> quests = new List<QuestlineDefinition>();
internal sealed class DoseLocationsRoot
public int schema_version;
public List<DoseLocationDef> locations = new List<DoseLocationDef>();
internal sealed class DoseItemsRoot
public int schema_version;
public List<DoseItemDef> items = new List<DoseItemDef>();
public static class DoseContentCatalogLoader
public const string LocationsFile = "dose_locations.json";
public const string ItemsFile = "dose_items.json";
public const string QuestsFile = "dose_quests.json";
public static DoseContentCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static QuestlineDefinition? ToQuestlineDefinition(DoseQuestDef rq) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 303 lines / 12451 bytes.
- SHA-256: `f1630ce8db3a4c544cd1889159df443cbc87fa346a587cf679f5858b3021a33a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FoundryIngredientEntry
public string item_id = string.Empty;
public int amount = 1;
public sealed class FoundryProductEntry
public string product_id = string.Empty;
public string display_name = string.Empty;
public string category = string.Empty;
public string result_item_id = string.Empty;
public int result_amount = 1;
public List<FoundryIngredientEntry> ingredients = new List<FoundryIngredientEntry>();
public float labor_hours = 0f;
public float cast_hours = 0f;
public int fuel_units = 0;
public int water_litres = 0;
public float skill_target = 0.5f;
public float quality_target = 70f;
public string treaty_id = string.Empty;
public int quota_amount = 0;
public string sink = string.Empty;
public string notes = string.Empty;
public string[] tags = Array.Empty<string>();
public sealed class FoundryProductionFile
public int schema_version = 1;
public string collection_id = string.Empty;
public List<FoundryProductEntry> products = new List<FoundryProductEntry>();
public sealed class FoundryFactionRelation
public string faction_id = string.Empty;
public string stance = string.Empty;   // ally | trade_partner | rival | internal
public string notes = string.Empty;
public sealed class FoundryFactionEntry
public string faction_id = string.Empty;
public string display_name = string.Empty;
public string short_name = string.Empty;
public string identity = string.Empty;
public string icon_path = string.Empty;
public string[] internal_divisions = Array.Empty<string>();
public List<FoundryFactionRelation> relationships = new List<FoundryFactionRelation>();
public string[] tags = Array.Empty<string>();
public static class SilentFoundryCatalogLoader
public const string ProductionFileName = "foundry_production.json";
public const string FactionFileName = "foundry_faction.json";
public const string AccordsFileName = "foundry_accords.json";
public static Dictionary<string, int> LoadAccordRatificationDays( string dataDirectory, IFileIO? files = null, IJsonSerializer? serializer = null) {
public static FoundryProductionFile LoadProduction( string dataDirectory, IFileIO? files = null, IJsonSerializer? serializer = null) {
public static FoundryFactionEntry? LoadFaction( string dataDirectory, IFileIO? files = null, IJsonSerializer? serializer = null) {
public sealed class SilentFoundryCatalog
public FoundryFactionEntry Faction { get; private set; }
public IReadOnlyList<FoundryProductEntry> AllProducts => _products;
public int ProductCount => _products.Count;
public void Load(FoundryProductionFile production, FoundryFactionEntry faction) {
public void MergeHeavyRecipes(IEnumerable<FoundryProductEntry> heavyProducts) {
public void MergeGlassworksRecipes(IEnumerable<FoundryProductEntry> glassProducts) {
public FoundryProductEntry? GetProduct(string productId) {
public List<FoundryProductEntry> GetByCategory(string category) {
public List<FoundryProductEntry> GetQuotaProducts() {
```


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Warlords/WarlordDoctrineCatalog.cs`

### `Assets/Ashfall.Core/Warlords/WarlordDoctrineCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 484 lines / 23495 bytes.
- SHA-256: `285ba009ef2ffc9b43757c16d6e511fb1ad918dfdd2d07533dbb7801994dbc7c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class WarlordDef
public string faction_id = "warlords_sector_4";
public string leader_name = "The Tollman";
public string home_location_id = "loc_toll_house";
public string starting_doctrine_id = "warlord_doctrine_toll";
public string tribute_currency_item = "canned_food";
public int tribute_interval_days = 7;
public int tribute_base_amount = 6;
public float tribute_escalation_factor = 1.5f;
public float tribute_max_multiplier = 8f;
public float short_payment_threshold = 0.9f;
public int action_interval_days = 5;
public int action_cooldown_days = 5;
public int doctrine_cooldown_days = 10;
public int report_delay_days = 3;
public float doctrine_change_margin = 0.15f;
public class WarlordTerritoryNodeDef
public string location_id = string.Empty;
public bool home;
public int supply_value;
public bool chokepoint;
public int defense_value;
public List<string> neighbors = new List<string>();
public class WarlordDoctrineDef
public string id = string.Empty;
public string display_name = string.Empty;
public string description = string.Empty;
public float risk_tolerance = 0.5f;
public string preferred_goal = string.Empty;
public List<string> eligible_actions = new List<string>();
public Dictionary<string, int> action_weights = new Dictionary<string, int>();
public List<string> resource_priority = new List<string>();
public string target_rule = "nearest_undefended";
public string journal_key = string.Empty;
public string radio_key = string.Empty;
public List<WarlordDoctrineTransitionDef> transitions = new List<WarlordDoctrineTransitionDef>();
public class WarlordDoctrineTransitionDef
public string to = string.Empty;
public string signal = string.Empty;   // supply_ratio | failure_streak | success_streak | contested_count | player_tribute_reliability | environment_hazard
public string condition = "gte";       // gte | lt
public float threshold = 0f;
public class WarlordAliasWarningDef
public string canonical = string.Empty;
public List<string> aliases_not_merged = new List<string>();
public string notes = string.Empty;
public sealed class WarlordDoctrineCatalog
public WarlordDef Warlord { get; set; } = new WarlordDef();
public List<WarlordTerritoryNodeDef> Territory { get; } = new List<WarlordTerritoryNodeDef>();
public List<WarlordDoctrineDef> Doctrines { get; } = new List<WarlordDoctrineDef>();
public List<WarlordAliasWarningDef> AliasWarnings { get; } = new List<WarlordAliasWarningDef>();
public Dictionary<string, List<string>> CollectorVoice { get; } = new Dictionary<string, List<string>>(StringComparer.Ordinal);
public string CollectorLine(string state, int day) {
public WarlordDoctrineDef? GetDoctrine(string id) {
public WarlordTerritoryNodeDef? GetNode(string locationId) {
public List<string> Neighbors(string locationId) {
public static class WarlordDoctrineCatalogLoader
public const string FileName = "warlord_doctrines.json";
public static WarlordDoctrineCatalog Load(string dataDirectory, IFileIO files, IJsonSerializer json) {
public class WarlordDoctrineContainer
public int schema_version = 1;
public WarlordDef warlord = new WarlordDef();
public List<WarlordTerritoryNodeDef> territory = new List<WarlordTerritoryNodeDef>();
public List<WarlordDoctrineDef> doctrines = new List<WarlordDoctrineDef>();
public List<WarlordAliasWarningDef> faction_alias_warnings = new List<WarlordAliasWarningDef>();
public Dictionary<string, List<string>> collector_voice = new Dictionary<string, List<string>>(StringComparer.Ordinal);
public WarlordDoctrineCatalog ToCatalog() {
public static class WarlordCatalogValidator
public sealed class ValidationReport
public readonly List<string> Errors = new List<string>();
public readonly List<string> AliasWarnings = new List<string>();
public bool Clean => Errors.Count == 0;
public static ValidationReport Validate(WarlordDoctrineCatalog catalog, string dataDirectory, IFileIO files) {
public class WarlordIdProbe
public string id = string.Empty;
public class WarlordFactionProbe
public string faction_id = string.Empty;
public class FactionTributeProbe
public string faction_id = string.Empty;
public List<string> tribute_demands = new List<string>();
public class WarlordCatalogWrapperProbe
public System.Collections.Generic.List<WarlordIdProbe> locations = new System.Collections.Generic.List<WarlordIdProbe>();
public System.Collections.Generic.List<WarlordIdProbe> items = new System.Collections.Generic.List<WarlordIdProbe>();
```


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Ecology/EcologicalInfestationCatalog.cs`

### `Assets/Ashfall.Core/Ecology/EcologicalInfestationCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 49 lines / 1897 bytes.
- SHA-256: `594f1e8776d1d013e82d9db808d2865248c4f97a1f193e760941ab946ebbaac9`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class EcologicalInfestationCatalogLoader
public const string DefaultFileName = "ecological_infestations.json";
public static List<EcologicalInfestationDefinition>? Load( string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null) {
public const string FileName = "ecological_infestations.json";
public sealed class EcologicalInfestationFileRaw
public int schema_version = 1;
public string collection_id = string.Empty;
public string description = string.Empty;
public List<EcologicalInfestationDefinition> infestations = new List<EcologicalInfestationDefinition>();
```


# Appendix Q.566 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/RadarEcmCatalog.cs`

### `Assets/Ashfall.Core/Expeditions/RadarEcmCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 95 lines / 4585 bytes.
- SHA-256: `a8c7b633e18222d15f3b4cf2112e382f1bb58423dc2129b02a8504e37d8d6b1a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class EcmProfileDef
public string profile_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public List<string> modes { get; set; } = new List<string>();
public string power_draw_class { get; set; } = "high";
public int heat_rate_per_active_tick { get; set; } = 4;
public int max_heat_permille { get; set; } = 1000;
public int lock_break_bonus_permille { get; set; } = 220;
public int detection_reduction_permille { get; set; } = 120;
public int counter_detection_risk_permille { get; set; } = 120;
public int condition_wear_per_active_tick { get; set; } = 4;
public int cooldown_ticks { get; set; } = 240;
public string install_item_id { get; set; } = string.Empty;
public sealed class EcmModeRuleDef
public string power_draw { get; set; } = "moderate";
public int lock_break_bonus_permille { get; set; } = 0;
public int counter_detection_risk_permille { get; set; } = 0;
public int false_track_pressure { get; set; } = 0;
public int duration_ticks { get; set; } = 0;
public sealed class EcmThreatProfileDef
public string threat_profile_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public int base_detection_permille { get; set; } = 400;
public int lock_rate_permille { get; set; } = 120;
public sealed class EcmInvariantsDef
public int single_mode_lock_break_cap_permille { get; set; } = 400;
public bool no_immunity { get; set; } = true;
public sealed class RadarEcmCatalog
public int schema_version { get; set; } = 1;
public List<EcmProfileDef> ecm_profiles { get; set; } = new List<EcmProfileDef>();
public Dictionary<string, EcmModeRuleDef> mode_rules { get; set; } = new Dictionary<string, EcmModeRuleDef>();
public List<EcmThreatProfileDef> threat_profiles { get; set; } = new List<EcmThreatProfileDef>();
public EcmInvariantsDef invariants { get; set; } = new EcmInvariantsDef();
public EcmProfileDef? FindProfile(string profileId) {
public EcmModeRuleDef? FindModeRule(string modeId) =>
public EcmThreatProfileDef? FindThreatProfile(string threatProfileId) {
public static RadarEcmCatalog? FromJson(string json, string path) {
```


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Feedback/FeedbackMessageCatalogLoader.cs`

### `Assets/Ashfall.Core/Feedback/FeedbackMessageCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 342 lines / 27172 bytes.
- SHA-256: `074963258dbea18686f1fb8b1bbf36f2d299caa7c8fc8e7580f51dfb3706e1ce`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class FeedbackMessageCatalogLoader
public const string FileName = "feedback_messages.json";
public static FeedbackMessageContainer LoadContainer(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static void ValidateContainer(FeedbackMessageContainer container) {
public static FeedbackMessageCatalog LoadCatalog(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static FeedbackMessageContainer CreateDefaultContainer() {
```


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/IO/CatalogBootValidator.cs`

### `Assets/Ashfall.Core/IO/CatalogBootValidator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 338 lines / 16242 bytes.
- SHA-256: `26b4b68d934817c0878be70f9e35e34959d846648476d48575aed3dc85fd4e22`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class CatalogBootValidator
public delegate object CatalogLoader(string dataDir, IFileIO fileIO, IJsonSerializer json);
public class CatalogEntry
public string FileName { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public CatalogClassification Classification { get; set; } = CatalogClassification.Optional;
public CatalogLoader? Loader { get; set; }
public static void RegisterCatalog( string fileName, string displayName, CatalogClassification classification, CatalogLoader? loader = null) {
public static CatalogBootReport Validate(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static void ThrowIfRequiredFailed(CatalogBootReport report) {
public class CatalogBootReport
public int TotalCount { get; set; }
public int RequiredCount { get; set; }
public int OptionalCount { get; set; }
public int DevOnlyCount { get; set; }
public IReadOnlyList<CatalogBootEntry> Entries => _entries.AsReadOnly();
public bool HasErrors => _entries.Exists(e => e.Severity == CatalogLoadSeverity.Error || e.Severity == CatalogLoadSeverity.Fatal);
public bool HasRequiredErrors => _entries.Exists(e =>
public void AddSuccess(string displayName, string fileName) {
public void AddWarning(string displayName, string fileName, string message) {
public void AddError(string displayName, string fileName, string message) {
public override string ToString() {
public readonly struct CatalogBootEntry
public string DisplayName { get; }
public string FileName { get; }
public CatalogLoadSeverity Severity { get; }
public bool IsRequired { get; }
public string Message { get; }
```


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Muster/FactionActionCatalog.cs`

### `Assets/Ashfall.Core/Muster/FactionActionCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 226 lines / 9359 bytes.
- SHA-256: `70361b914037781c0c09952b9e919e3b7ba847cd0ada24d1de1cbaa6a9a8c970`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class FactionActionBands
public const string Hostile = "hostile";
public const string Poor = "poor";
public const string Neutral = "neutral";
public const string Good = "good";
public const string Allied = "allied";
public static readonly string[] All = { Hostile, Poor, Neutral, Good, Allied };
public class FactionActionEffects
public float trustDelta;
public float aggressionDelta;
public int membersDelta;
public int lockoutDelta;
public string itemId = string.Empty;
public int itemAmount;
public List<string> flags = new List<string>();
public string journal = string.Empty;
public class FactionActionChoice
public string choiceId = string.Empty;
public string text = string.Empty;
public FactionActionEffects effects = new FactionActionEffects();
public class FactionActionVariant
public string band = FactionActionBands.Neutral;
public string text = string.Empty;
public List<FactionActionChoice> choices = new List<FactionActionChoice>();
public class FactionActionDefinition
public string id = string.Empty;
public string factionId = string.Empty;
public string title = string.Empty;
public string text = string.Empty;
public int minDay;
public int maxDay;                 // 0 = unbounded
public bool once;
public int cooldownDays;
public List<string> requiresFlags = new List<string>();
public List<string> forbidsFlags = new List<string>();
public List<FactionActionVariant> variants = new List<FactionActionVariant>();
public static class FactionActionCatalogLoader
public const string FileName = "muster_faction_actions.json";
public const int CurrentSchemaVersion = 1;
public static List<FactionActionDefinition> LoadActions( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public int schema_version = 1;
public List<ActionEntry> actions = new List<ActionEntry>();
public string id;
public string faction_id;
public string title;
public string text;
public int min_day;
public int max_day;
public bool once;
public int cooldown_days;
public List<string> requires_flags;
public List<string> forbids_flags;
public List<VariantEntry> variants;
public string band;
public string text;
public List<ChoiceEntry> choices;
public string choice_id;
public string text;
public EffectsEntry effects;
public float trust_delta;
public float aggression_delta;
public int members_delta;
public int lockout_delta;
public string item_id;
public int item_amount;
public List<string> flags;
public string journal;
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/BunkerGraffitiCatalog.cs`

### `Assets/Ashfall.Core/Narrative/BunkerGraffitiCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 208 lines / 7681 bytes.
- SHA-256: `a926a60af7644a64c3edb2433fe55d153eb041103d5349a53ed61fe1cf2b768f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class BunkerGraffitiEntry
public string posting_id;
public int recorded_day;
public string location;
public string medium;
public string author_signature;
public string category;
public string content;
public string morale_effect;
public string[] tags;
public sealed class BunkerGraffitiFile
public int schema_version;
public string collection_id;
public List<BunkerGraffitiEntry> postings = new List<BunkerGraffitiEntry>();
public sealed class BunkerGraffitiCatalog
public IReadOnlyList<BunkerGraffitiEntry> AllPostings => _allPostings;
public int Count => _allPostings.Count;
public void Clear() {
public void Load(string json, IJsonSerializer serializer) {
public static BunkerGraffitiCatalog LoadFromDirectory(string dataDir, IFileIO fileIo, IJsonSerializer serializer) {
public BunkerGraffitiEntry? GetById(string postingId) {
public List<BunkerGraffitiEntry> GetUnlockedByDay(int currentDay) {
public List<BunkerGraffitiEntry> GetByCategory(string categorySnippet) {
public List<BunkerGraffitiEntry> GetPostingsForTarget(string targetId, int currentDay = int.MaxValue) {
public List<BunkerGraffitiEntry> GetPostingsForRoom(string roomId, int currentDay = int.MaxValue) => GetPostingsForTarget(roomId, currentDay);
public List<BunkerGraffitiEntry> GetPostingsForLocation(string locationId, int currentDay = int.MaxValue) => GetPostingsForTarget(locationId, currentDay);
```


# Appendix Q.571 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/DwellerMedicalCatalog.cs`

### `Assets/Ashfall.Core/Narrative/DwellerMedicalCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 152 lines / 5345 bytes.
- SHA-256: `3d3c8e2ca820a73e7d92e956314d1709ce67bf0dc9155d92873c297a30472334`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DwellerMedicalCaseEntry
public string case_id;
public int recorded_day;
public string attending_physician;
public string patient_id;
public string patient_name;
public string category;
public float dose_estimate_msv;
public string symptoms;
public string intervention;
public string outcome;
public string doctor_margin_note;
public string[] tags;
public string? doc_type;
public sealed class DwellerMedicalCasebookFile
public int schema_version;
public string collection_id;
public List<DwellerMedicalCaseEntry> cases = new List<DwellerMedicalCaseEntry>();
public sealed class DwellerMedicalCatalog
public IReadOnlyList<DwellerMedicalCaseEntry> AllCases => _allCases;
public int Count => _allCases.Count;
public void Load(string json, IJsonSerializer serializer) {
public static DwellerMedicalCatalog LoadFromDirectory(string dataDir, IFileIO fileIo, IJsonSerializer serializer) {
public DwellerMedicalCaseEntry? GetById(string caseId) {
public List<DwellerMedicalCaseEntry> GetUnlockedByDay(int currentDay) {
public List<DwellerMedicalCaseEntry> GetByCategory(string categorySnippet) {
public List<DwellerMedicalCaseEntry> GetByPhysician(string physicianSnippet) {
```


# Appendix Q.572 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/PersonalLetterCatalog.cs`

### `Assets/Ashfall.Core/Narrative/PersonalLetterCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 266 lines / 9185 bytes.
- SHA-256: `e738bfc33612ec3b1adafd8e0b04819fd3f97492235f1535c072ff25c147a800`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PersonalLetterEntry
public string letter_id;
public int day;
public string sender;
public string recipient;
public string location;
public string letter_type;
public string tone;
public string[] key_themes;
public string content;
public string[] cross_refs;
public string[] tags;
public sealed class PersonalLetterFile
public int schema_version;
public string collection_id;
public string description;
public List<PersonalLetterEntry> letters = new List<PersonalLetterEntry>();
public sealed class PersonalLetterCatalog
public IReadOnlyList<PersonalLetterEntry> AllLetters => _allLetters;
public void Load(string json, IJsonSerializer serializer) {
public void Clear() {
public static PersonalLetterCatalog LoadFromDirectory(string dataDir, IFileIO fileIo, IJsonSerializer serializer) {
public PersonalLetterEntry? GetById(string letterId) {
public List<PersonalLetterEntry> GetByType(string letterType) {
public List<PersonalLetterEntry> GetBySender(string senderSnippet) {
public List<PersonalLetterEntry> GetByRecipient(string recipientSnippet) {
public List<PersonalLetterEntry> GetByTag(string tag) {
public List<PersonalLetterEntry> GetByTheme(string theme) {
public List<PersonalLetterEntry> GetBySearch(string query) {
public List<PersonalLetterEntry> GetSortedByDay(bool ascending = true) {
```


# Appendix Q.573 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs`

### `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 162 lines / 5893 bytes.
- SHA-256: `b2c2eff4efe46878bdcaa00a6c2f059d38c78fe15905c85b639d0a61696b2ac2`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RadioStationCatalog
public const string StationCivilDefense = "station_civil_defense";
public const string StationGarrisonOverlord = "station_garrison_overlord";
public const string StationVitrifiedCrater = "station_vitrified_crater";
public const string StationOpenClassroom = "station_open_classroom";
public const string StationNumbersSigint = "station_numbers_sigint";
public const string StationAutomatedRelay = "station_automated_relay";
public IReadOnlyCollection<RadioStationDefinition> AllStations => _stations.Values;
public void Clear() {
public int LoadFromJson(string json) {
public int LoadFromDataDirectory(string dataDir) {
public void Register(RadioStationDefinition def) {
public RadioStationDefinition? GetStation(string stationId) {
public RadioStationDefinition? FindStationAtFrequency(float frequencyMhz, float toleranceMhz = 0.5f) {
public RadioStationState GetStationState(string stationId) {
public void SetStationState(string stationId, RadioStationState state) {
public void ResetOverrides() {
public Dictionary<string, RadioStationState> ExportOverrides() {
public void ImportOverrides(IDictionary<string, RadioStationState>? overrides) {
public RadioProgramSlot? GetCurrentSlot(string stationId, int campaignDay, int hour) {
public RadioProgramSlot? GetNextSlot(string stationId, int campaignDay, int hour) {
public RadioSignalStrength ComputeSignalStrength(string stationId, RadioReceptionFactors? factors) {
```


# Appendix Q.574 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Shelter/CellulosicBiofuelCatalog.cs`

### `Assets/Ashfall.Core/Shelter/CellulosicBiofuelCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 107 lines / 5234 bytes.
- SHA-256: `805826d537135bb5739e33dd9f66fdc2e5d9685734cac848eea9f870fcaf0cbc`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class BiofuelMachineDef
public string machine_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public Dictionary<string, int> construction_required_items { get; set; } = new Dictionary<string, int>();
public int construction_labor_days { get; set; } = 3;
public float max_condition { get; set; } = 100f;
public int maintenance_interval_days { get; set; } = 5;
public Dictionary<string, int> maintenance_required_items { get; set; } = new Dictionary<string, int>();
public string room_id { get; set; } = string.Empty;
public string power_class { get; set; } = "high";
public string ventilation_class { get; set; } = "industrial";
public float ventilation_load_per_active_day { get; set; } = 0.06f;
public sealed class BiofuelProcessDef
public string process_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public List<string> accepted_item_ids { get; set; } = new List<string>();
public string feedstock_tag { get; set; } = "cellulose";
public int batch_input_units { get; set; } = 10;
public string conversion_tier { get; set; } = "standard";
public int conversion_efficiency_permille { get; set; } = 480;
public int fermentation_ticks { get; set; } = 2880;
public int refining_ticks { get; set; } = 1440;
public float energy_cost_kwh_per_day { get; set; } = 7f;
public float process_water_units { get; set; } = 4f;
public string fuel_output_id { get; set; } = string.Empty;
public int yield_permille { get; set; } = 520;
public int fouling_risk_permille { get; set; } = 90;
public string vapor_hazard_tier { get; set; } = "elevated";
public sealed class BiofuelGradeDef
public string grade_id { get; set; } = string.Empty;
public string output_item_id { get; set; } = string.Empty;
public float grid_worth_units { get; set; } = 1f;
public float vehicle_wear_multiplier { get; set; } = 1f;
public sealed class BiofuelHazardOutcomes
public List<string> faults { get; set; } = new List<string>();
public string fire_hazard_event { get; set; } = "IndustrialFireHazardRequested";
public sealed class BiofuelStorageDef
public int tank_max_units { get; set; } = 60;
public sealed class CellulosicBiofuelCatalog
public int schema_version { get; set; } = 1;
public BiofuelMachineDef machine { get; set; } = new BiofuelMachineDef();
public List<BiofuelProcessDef> processes { get; set; } = new List<BiofuelProcessDef>();
public List<BiofuelGradeDef> fuel_grades { get; set; } = new List<BiofuelGradeDef>();
public BiofuelHazardOutcomes hazard_outcomes { get; set; } = new BiofuelHazardOutcomes();
public BiofuelStorageDef storage { get; set; } = new BiofuelStorageDef();
public BiofuelProcessDef? FindProcess(string processId) {
public BiofuelGradeDef? FindGrade(string gradeId) {
public static CellulosicBiofuelCatalog? FromJson(string json, string path) {
```


# Appendix Q.575 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Shelter/FogHarvestingCatalog.cs`

### `Assets/Ashfall.Core/Shelter/FogHarvestingCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 83 lines / 4220 bytes.
- SHA-256: `ac667409089f0e0c6433939fe20bfc063d0d53a776477e94ca1efc289c9441ad`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FogHarvesterProfileDef
public string profile_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public int collection_area_units { get; set; } = 20;
public bool passive { get; set; } = true;
public string wind_band { get; set; } = "moderate";
public string humidity_band { get; set; } = "high";
public int base_yield_units { get; set; } = 12;
public int wear_rate_per_day { get; set; } = 2;
public string contamination_profile { get; set; } = "surface_exposed";
public string output_water_type { get; set; } = "raw";
public float power_draw_kwh_per_day { get; set; } = 0f;
public Dictionary<string, int> construction_required_items { get; set; } = new Dictionary<string, int>();
public List<string> siting_tags { get; set; } = new List<string>();
public sealed class FogBandsDef
public Dictionary<string, float> presence_by_weather_kind { get; set; } = new Dictionary<string, float>();
public Dictionary<string, float> wind_band_multipliers { get; set; } = new Dictionary<string, float>();
public Dictionary<string, float> humidity_band_multipliers { get; set; } = new Dictionary<string, float>();
public float powered_assist_multiplier { get; set; } = 1.5f;
public sealed class FogStormEventDef
public int damage_condition_permille { get; set; } = 250;
public int disable_below_condition_permille { get; set; } = 200;
public sealed class FogBufferDef
public int max_units { get; set; } = 40;
public sealed class FogHarvestingCatalog
public int schema_version { get; set; } = 1;
public List<FogHarvesterProfileDef> harvester_profiles { get; set; } = new List<FogHarvesterProfileDef>();
public FogBandsDef fog_bands { get; set; } = new FogBandsDef();
public FogStormEventDef storm_event { get; set; } = new FogStormEventDef();
public FogBufferDef buffer { get; set; } = new FogBufferDef();
public FogHarvesterProfileDef? FindProfile(string profileId) {
public static FogHarvestingCatalog? FromJson(string json, string path) {
```


# Appendix Q.576 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Shelter/PrecisionBroachingCatalog.cs`

### `Assets/Ashfall.Core/Shelter/PrecisionBroachingCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 80 lines / 3822 bytes.
- SHA-256: `46ae897f7e53fb3fa0d5766c41c85c2462f18db1e882433e7b861c988d733320`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class BroachBenchDef
public string bench_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public Dictionary<string, int> construction_required_items { get; set; } = new Dictionary<string, int>();
public int construction_labor_days { get; set; } = 3;
public float max_condition { get; set; } = 100f;
public int maintenance_interval_days { get; set; } = 6;
public Dictionary<string, int> maintenance_required_items { get; set; } = new Dictionary<string, int>();
public string room_id { get; set; } = string.Empty;
public string machine_load_class { get; set; } = "high";
public sealed class BroachOperationDef
public string operation_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string input_item_id { get; set; } = string.Empty;
public string tool_class { get; set; } = string.Empty;
public int labor_ticks { get; set; } = 240;
public string tolerance_target { get; set; } = "standard";
public string output_item_id { get; set; } = string.Empty;
public int tool_wear_per_job { get; set; } = 10;
public int machine_load_permille { get; set; } = 500;
public sealed class BroachQualityTierDef
public string tier_id { get; set; } = string.Empty;
public float quality_value { get; set; } = 0.6f;
public sealed class PrecisionBroachingCatalog
public int schema_version { get; set; } = 1;
public BroachBenchDef bench { get; set; } = new BroachBenchDef();
public List<BroachOperationDef> operations { get; set; } = new List<BroachOperationDef>();
public List<string> failure_states { get; set; } = new List<string>();
public List<BroachQualityTierDef> quality_tiers { get; set; } = new List<BroachQualityTierDef>();
public BroachOperationDef? FindOperation(string operationId) {
public BroachQualityTierDef? FindQualityTier(string tierId) {
public static PrecisionBroachingCatalog? FromJson(string json, string path) {
```


# Appendix Q.577 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/FinalWishCatalogLoader.cs`

### `Assets/Ashfall.Core/Survivors/FinalWishCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 66 lines / 2476 bytes.
- SHA-256: `e84f8d79b22a5ca1f2e2283a6d0672f70d0f36b7396a12d8616539d1777e577e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class FinalWishCatalogLoader
public const string FileName = "final_wishes.json";
public static FinalWishCatalog LoadCatalog(string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix Q.578 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs`

### `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 287 lines / 11071 bytes.
- SHA-256: `685c8560522695338fca5fa6ff27eabb7ac16e2114ca84ecb2087607c2d49096`.
- Architecture signals: seeded references=0; save/restore symbols=3; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class SurvivorDefinition
public string id = string.Empty;
public string displayName = string.Empty;
public string profession = string.Empty;
public string bio = string.Empty;
public float baseHealth = 100f;
public List<string> traitIds = new List<string>();
public string activeQuestlineId = string.Empty;
public class SurvivorRosterEntry
public string survivorId = string.Empty;
public string definitionId = string.Empty;
public int joinedDay = 0;
public bool isAlive = true;
public string deathReason = string.Empty;
public int CampaignAgeDays(int currentDay) => SurvivorLifecycle.CampaignAgeDays(joinedDay, currentDay);
public class SurvivorRosterState
public string systemId = SurvivorRosterSystem.SystemId;
public List<SurvivorRosterEntry> entries = new List<SurvivorRosterEntry>();
public class SurvivorRosterSystem
public const string SystemId = "survivor_roster_system";
public event Action<SurvivorRosterEntry> OnSurvivorJoined;
public event Action<SurvivorRosterEntry, string> OnSurvivorDied; // entry, reason
public event Action<SurvivorRosterState> OnStateChanged;
public SurvivorRosterState State => _state;
public IReadOnlyList<SurvivorDefinition> Catalog => _catalog;
public IReadOnlyList<SurvivorRosterEntry> Roster => _state.entries;
public void RegisterDefinition(SurvivorDefinition def) {
public void RegisterRange(IEnumerable<SurvivorDefinition> defs) {
public SurvivorDefinition? FindDefinition(string definitionId) {
public bool Join(string definitionId, int day) {
public bool Die(string survivorId, string reason) {
public SurvivorRosterEntry? Find(string survivorId) {
public SurvivorRosterState CaptureState() {
public void RestoreState(SurvivorRosterState saved) {
public static class SurvivorCatalogLoader
public const string FileName = "survivors.json";
public static List<SurvivorDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static CatalogLoadResult<SurvivorDefinition> LoadWithResult( string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix Q.579 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/VerdictQuestCatalogLoader.cs`

### `Assets/Ashfall.Core/Verdict/VerdictQuestCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 63 lines / 2293 bytes.
- SHA-256: `5f1a7bc4b7265f09f93b104b8e569dab426eea3aec5e50b14ff4656c7f59c6c4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class VerdictQuestCatalogLoader
public const string FileName = "verdict_questlines.json";
public static int LoadAndRegister( QuestlineSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix Q.580 — Additional Current Architecture Evidence: `src/UI/AshfallUiHelpers.cs`

### `src/UI/AshfallUiHelpers.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 991 lines / 44280 bytes.
- SHA-256: `b8115d3dedf5c786ecb3a1523938a54c016133128cd34e939c68995a1f1fa52d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=6; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class AshfallUiHelpers
public const string FallbackIconPath = "assets/ui/Icons/icon_placeholder.png";
public const string FallbackIconResPath = "res://assets/ui/Icons/icon_placeholder.png";
public const string FallbackSurvivorResPath = "res://assets/sprites/Characters/placeholder_survivor.png";
public const string FallbackSurvivorPath = "assets/sprites/Characters/placeholder_survivor.png";
public static FontFile? LoadFont(string path) {
public static FontFile? FontBarlowRegular =>
public static FontFile? FontBarlowSemiBold =>
public static FontFile? FontBarlowBold =>
public static FontFile? FontShareTechMono =>
public static void ApplyFont(Label label, FontFile? font) {
public static Label MakeTitle(string text, int fontSize = Theme.FontSizeH1) {
public static Label MakeSectionHeader(string text) {
public static Label MakeSubsectionHeader(string text) {
public static Label MakeBody(string text, bool autowrap = true) {
public static Label MakeSmall(string text, bool autowrap = false) {
public static Label MakeMono(string text) {
public static Label MakeLabel(string text) {
public static Label MakeLabel(string text, int fontSize, (float r, float g, float b, float a) colorToken) {
public static Label MakeLabel(string text, int fontSize, Color color) {
public static Label MakeLabel(string text, int fontSize, bool bold) {
public static Label MakeMetadata(string text, bool autowrap = false) {
public static Label MakeWarning(string text) {
public static Label MakeCritical(string text) {
public static Color ColorBackdrop => ToColor(Theme.BackdropOverlay);
public static Color ColorSurface => ToColor(Theme.Surface);
public static Color ColorSurfaceCard => ToColor(Theme.SurfaceCard);
public static Color ColorPrimary => ToColor(Theme.Warm);
public static Color ColorHighlight => ToColor(Theme.Hot);
public static Color ColorText => ToColor(Theme.Pale);
public static Color ColorMuted => ToColor(Theme.Muted);
public static Color ColorDim => ToColor(Theme.Dim);
public static Color ColorSuccess => ToColor(Theme.Success);
public static Color ColorWarning => ToColor(Theme.Warning);
public static Color ColorCritical => ToColor(Theme.Critical);
public static Color ColorRadiation => ToColor(Theme.Radiation);
public static Color ColorRadiationAcute => ToColor(Theme.RadiationAcute);
public static Color ColorInfo => ToColor(Theme.Info);
public static ColorRect MakeBackdropOverlay() {
public static Label MakeSuccess(string text) {
public static Label MakeInfo(string text) {
public static Label MakeRadiation(string text, bool acute = false) {
public static VBoxContainer MakeVBox(int separation = Theme.SpacingSm) {
public static HBoxContainer MakeHBox(int separation = Theme.SpacingSm) {
public static MarginContainer MakeMargins(int all = Theme.HudPanelPadding) {
public static MarginContainer MakeMargins(int left, int top, int right, int bottom) {
public static Control MakeEmptyState( string message, string title = "NO DATA RECORDED", string? actionHint = null, (float r, float g, float b, float a)? accentColor = null) {
public static Label MakeEmptyStateLabel(string message, string? hint = null) {
public static PanelContainer MakePanel(int minWidth = 0, int minHeight = 0) {
public static StyleBox MakePanelFrameStyleBox() {
public static StyleBox MakeHeaderFrameStyleBox() {
public static PanelContainer MakeHeaderBar() {
public static PanelContainer MakeCard(int minW = 0, int minH = 0) => MakePanel(minW, minH);
public static PanelContainer MakeCardFrame(string title, string? subtitle = null, int minW = 0, int minH = 0) {
public static HSeparator MakeSeparator() {
public static Button MakeButton(string text, Action onPressed, bool disabled = false) {
public static Button MakeDisabledButton(string text, string reasonDisabled) {
public static Control MakeSeverityBadge(SeverityLevel level, string text, string? customIcon = null) {
public static Color GetSeverityColor(SeverityLevel level) => level switch
public static string GetSeverityIcon(SeverityLevel level) => level switch
public static HBoxContainer MakeDataRow(string label, string value, Color? valueColor = null, int fontSize = Theme.FontSizeSmall) {
public static Label MakeDimLabel(string text) {
public static Label MakeColoredLabel(string text, (float r, float g, float b, float a) colorToken, int fontSize = Theme.FontSizeBody) {
public static HBoxContainer MakeActionBar(int separation = Theme.SpacingSm) {
public static TextureRect MakeFactionEmblem(string factionId, int size = 40) {
public static TextureRect MakeBadgeIcon(string badgeId, int size = 32) {
public static TextureRect MakeItemIcon(string itemId, int size = 32) {
public static Color ToColor((float r, float g, float b, float a) token) {
public static TextureRect? MakeSurvivorPortrait(string survivorId, int size = 56) {
public static Texture2D? TryLoadTexture(string path) {
public static StyleBoxFlat MakeFlatBg(Color bg, Color? border = null, int borderWidth = 1, int cornerRadius = 0) {
public static void EmptyChildren(Node parent) {
public static void EmptyChildrenExcept(Node parent, Node preservedChild) {
public static string FormatDoseMsv(float msv) {
public static string FormatDosePairMsv(float nominalMsv, float bookedMsv) {
public static string FormatDoseSource(Ashfall.Core.DoseContentCatalog? content, string? sourceId) {
```


# Appendix Q.581 — Additional Current Architecture Evidence: `src/Host/HostCli.SelfTests.cs`

### `src/Host/HostCli.SelfTests.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1232 lines / 59617 bytes.
- SHA-256: `a56bb8a0a9355f3575257da4bc68ff518c6fc9dfa1fdb5c3f2b98ad371fea61d`.
- Architecture signals: seeded references=1; save/restore symbols=1; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=1.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunDataIntegritySelfTest(string dataDirectory) {
public static int RunResearchCatalogSelfTest(string dataDirectory) {
public static int RunRadioCatalogSelfTest(string dataDirectory) {
public static int RunExpansionsSelfTest(string dataDirectory) {
public static int RunDeepCoastSelfTest(string dataDirectory) {
public static int RunWarlordSelfTest(string dataDirectory) {
public static int RunWarlordHostSelfTest(string dataDirectory) {
public static int RunWarlordUiSelfTest(string dataDirectory) {
public static int RunDeepCoastHostSelfTest(string dataDirectory = null!) {
public static int RunGreenhouseSelfTest() {
public static int RunSilentFoundrySelfTest(string dataDirectory) {
public static int RunDiseaseSelfTest(string dataDirectory) {
public static int RunCombatSelfTest(string dataDirectory) {
public static int RunArbitrationSelfTest() {
public static int RunLedgerDebtSelfTest() {
public static int RunPatrolEncounterSelfTest(string dataDirectory) {
public static int RunHoldfastSelfTest(string dataDirectory) {
public static int RunDutyRosterSelfTest(string dataDirectory) {
public static int RunStandingRecordSelfTest(string dataDirectory) {
public static int RunCrossingSelfTest(string dataDirectory) {
public static int RunIceRoadSelfTest(string dataDirectory) {
public static int RunCensusSelfTest() {
public static int RunBrineSelfTest() {
public static int RunMusterSelfTest() {
public static int RunFactionEcologySelfTest(string dataDirectory) {
public static int RunVerdictSelfTest(string dataDirectory) {
public long LivingRegisteredSouls() => _n;
public static int RunClusterSelfTest(string dataDirectory) {
public static int RunEndingsSelfTest() {
public static int RunJournalSaveSelfTest() {
public static int RunChemicalDependencySaveSelfTest() {
public static int RunContrabandStashSelfTest() {
public static int RunMedicalWardSaveSelfTest() {
public static int RunWeatherSaveSelfTest() {
public static int RunJournalWeatherPanelSelfTest() {
public static int RunInventorySaveSelfTest() {
public static int RunSaveLoadUiFailureSelfTest(string dataDirectory) {
public static int RunPanelBindLifecycleSelfTest(string dataDirectory) {
public static int RunSaveStoreChecksumSelfTest(string dataDirectory) {
public static int RunSevenDayDeterministicSmokeSelfTest(string dataDirectory) {
public static int RunUiAccessibilitySelfTest() {
public static int RunCoreSelfTest(string dataDirectory) {
public static int RunCatalogBootPreflight(string dataDirectory) {
public static int RunCampaignFuzzSelfTest(string dataDirectory) {
```


# Appendix R.582 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Medical/Plan16_19TriageEpilogueIntegrationTests.cs`

### `Ashfall.Core.Tests/Medical/Plan16_19TriageEpilogueIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 169; SHA-256: `bdc839c7c30b82baa381ad2e106c035046da5e9ec0a2fee1f555848542161a13`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ClinicalTriage_AssignsAccuratePriorities_AndChecksCapacity
SurgicalPreparation_EvaluatesCleanliness_AndConsumesSterileSupplies
EpilogueContextFactory_BuildsTruthfulContext_FromCampaignInputs
EpilogueMatrixRuntime_BranchesDeterministically_OnCampaignDivergence
```


# Appendix R.583 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/MusterEpilogueMatrixTests.cs`

### `Ashfall.Core.Tests/MusterEpilogueMatrixTests.cs`

- Current test declarations: Fact=28, Theory=0, InlineData=0.
- File lines: 553; SHA-256: `f6e1e013e0072ec7a12230853f26173d201771c8a9b0626ecd2da8f3824ffd16`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
EpilogueCatalog_LoadsExactly25Entries
EpilogueCatalog_All25KeysAreUnique
EpilogueCatalog_Original12KeysArePreserved
EpilogueCatalog_New13KeysArePresent
EpilogueCatalog_BidirectionalKeyCoverage_MatchesEpilogueMatrixAllKeys
EpilogueCatalog_AllTitlesAndProseAreNonEmptyAndRestrained
Evaluate_NullInput_ReturnsUnwritten
Evaluate_DefaultInput_ReturnsUnwritten
Evaluate_FailurePrecedence_ShelterFallenOverridesEverything
Evaluate_CompoundEnding_MercyAndWaterHeld_BeatsComponentEndings
Evaluate_CompoundEnding_IronAndFuelBurned_BeatsComponentEndings
Evaluate_FactionEndings_SelectsCorrectly
Evaluate_ResourceEndings_SelectsCorrectly
Evaluate_MoralEndings_SelectsCorrectly
Evaluate_VerdictEnding_TakesPrecedenceOverGenericFactionOrResource
Evaluate_MusterEnding_TakesPrecedenceOverGenericFactionOrResource
Evaluate_FactionPrecedence_BeatsGenericResourceAndMoral
Evaluate_ResourcePrecedence_BeatsGenericMoral
Evaluate_IsDeterministicAcrossReplays
EveryKey_HasProseInLoadedCatalog
Evaluate_Failure_BeatsPositiveResource_Individually
Evaluate_Failure_BeatsPositiveFaction_Individually
Evaluate_Failure_BeatsPositiveMoral_Individually
Evaluate_Overlap_IndependentAndWater_FactionWins
Evaluate_Overlap_GarrisonAndWater_FactionWins
Evaluate_Overlap_GrainAndListener_ResourceWins
Evaluate_PureFunction_DoesNotMutateInput
Plan96_All25Keys_CanBeBuiltIntoEpilogueChronicle
```


# Appendix R.584 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/VerdictContentWebTests.cs`

### `Ashfall.Core.Tests/VerdictContentWebTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 140; SHA-256: `43a025fb0d48c169b04c95a2ec7fcf6fbe57d26de306fce3ce4108895e533eaf`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
LoadItems_ReturnsAllFifteenRows
LoadItems_IdsAreUniqueAndSnakeCase
LoadItems_EvidenceAndQuestItemsPresent
LoadItems_RowsAlignToRuntimeSchema
LoadLocations_ReturnsFifteenSites
LoadRadio_LoadsThirtyAuthoredBroadcasts
```


# Appendix R.585 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/VerdictRadioSystemTests.cs`

### `Ashfall.Core.Tests/VerdictRadioSystemTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 143; SHA-256: `76b50d8a93d1d6b0598e21ffe84369f3fe1dc3cd61530b17aa0fe51c9f77338b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Poll_GatesOnCulpableWindow_NothingBefore
Poll_FiresCorpusOnceInsideWindow
Poll_FiresAllAtDeadline
FiredBroadcastsPublishToBus
SaveLoad_RoundTripsFiredIds_NoReplay
LoadFrom_LoadsThirtyAuthoredBroadcasts
EvidenceEnrollment_FieldsPresentInItems
```


# Appendix R.586 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/VerdictSystemTests.cs`

### `Ashfall.Core.Tests/VerdictSystemTests.cs`

- Current test declarations: Fact=54, Theory=0, InlineData=0.
- File lines: 723; SHA-256: `f25ace785950670bd5a6ef2436b246b418a95a82c41be19a2745faf76a4bbd06`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Evidence_Enroll_IsIdempotent
Evidence_Enroll_RejectsUnknown_WhenCatalogPopulated
Evidence_Enroll_AllowsAny_WhenCatalogEmpty
Evidence_FiresEvent
Evidence_CaptureRestore_Roundtrip
Evidence_RejectNullEmpty
MachineLog_Post_DuplicateSuppression
MachineLog_Post_DifferentKind_Allowed
MachineLog_ReadEntry_OneWay
MachineLog_ReadEntry_OutOfRange
MachineLog_CorruptionMarker_Deterministic
MachineLog_SpinTape_OnePerDay
MachineLog_CaptureRestore_Roundtrip
MachineLog_CaptureRestore_DoesNotAliasEntries
VerdictEvidenceChain_ReadEnrollsLedgerAndReckoningExactlyOnce
VerdictEvidenceChain_ReconcileIsSafeAfterSaveRestore
VerdictEvidenceChain_ReconcileRepairsDerivedReckoningCount
MachineLog_Post_RejectsEmptyFacility
Reckoning_Dormant_BeforeDay160
Reckoning_Knowing_AtDay160
Reckoning_Culpable_NeedsEvidence
Reckoning_Counted_AtDay240
Reckoning_CallIsOneShot
Reckoning_NeverReverses
Reckoning_SelectEnding_MutuallyExclusive
Reckoning_SelectEnding_RejectsBeforeCounted
Reckoning_SelectEnding_RejectsUnknown
Reckoning_CensusWindow_OpenInCulpable
Reckoning_CaptureRestore_Roundtrip
EndingEvaluator_ResolvedEnding_Priority
EndingEvaluator_NullState_ReturnsNull
EndingEvaluator_DecideEnding_FallsBackByEvidence
EndingEvaluator_DecideEnding_NullBeforeCounted
EndingEvaluator_TempestDecommissioned_OnlyOnRecount
Readout_Dormant_WhenStateNull
Readout_Knowing_InPhase
Readout_NegativeOrOverflowingCounters_StayBounded
Readout_Resolved_WhenCountPresented
Npc_Register_Find
Npc_Speak_OneShot
Npc_GetAvailable_RespectsPhase
Npc_GetAvailable_RespectsGatingFlag
Npc_CaptureRestore_Roundtrip
Save_CaptureEncode_DecodeRestore_Roundtrip
Save_TamperRejection
Save_RejectsEmptyChecksum
Save_RejectsNewerVersion
Census_WindowOpen_Every7DaysAt03
Census_BroadcastOnce_PerWindow
Census_SilentAfterSigning
Census_CanonConstants
CatalogLoader_Locations_ReturnsEmpty_WhenFileMissing
CatalogLoader_Locations_ReturnsEmpty_WhenNullArgs
VerdictItemsJson_MatchesRuntimeSchema
```


# Appendix R.587 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Narrative/Plan114_79YearOfAshAutopsyIntegrationTests.cs`

### `Ashfall.Core.Tests/Narrative/Plan114_79YearOfAshAutopsyIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 152; SHA-256: `cdef258e422dafa3dc452110e279cec8becdc0ab6f04a2461cdec9cd573d74d9`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
YearOfAsh_HasFifteenQuestlinesWithUniqueIds
YearOfAsh_AllStagesAndChoicesAreWellFormed
AutopsyProcedures_HasTwelveProceduresWithUniqueIds
CrossSystem_BothCatalogsLoadIndependentlyAndMeetMinimums
```


# Appendix R.588 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Narrative/YearOfAshQuestJsonParityTests.cs`

### `Ashfall.Core.Tests/Narrative/YearOfAshQuestJsonParityTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 164; SHA-256: `e3d50b8f1763b2722dfca58e89c22efd814191f07ebb200ecd382752c45bdf3b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CanonicalCatalog_ExistingEightQuestlines_MatchBuiltInBaseline
CanonicalCatalog_ContainsExactlySevenPlan114Questlines
PilotQuestline_GarrisonBloodDebt_LoadsAndPlaysThroughChoices
```


# Appendix R.589 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Radio/Plan94_102RadioFoundryIntegrationTests.cs`

### `Ashfall.Core.Tests/Radio/Plan94_102RadioFoundryIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 175; SHA-256: `05c0e99eaf70a8e7b1daca980f33271ec4963763632ccf3c1e25c9474086f962`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan94_VerdictRadioCatalog_LoadsAll30Broadcasts_WithValidAttributes
Plan102_FoundryAccords_LoadsAll18Treaties_WithProperSignatoryAndQuotas
CrossSystem_RadioTelemetryAndIndustrialTreaties_ExhibitCoherentWastelandTimeline
CrossSystem_DeterministicExecution_UnderRepeatedReloadsPasses
```


# Appendix R.590 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Tooling/CatchPolicyLintGateTests.cs`

### `Ashfall.Core.Tests/Tooling/CatchPolicyLintGateTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 242; SHA-256: `1332ad4de2e9b445ce8ad293461d0931dd97677e300101d8b8c5c4c0743ee174`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CatchPolicy_ZeroUndocumentedEmptyCatches
CatchPolicy_DataLoaderCatches_MustLogContextOrBeDocumented
CatchPolicy_CleanupCatches_AreExplicitlyDocumented
```


# Appendix R.591 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Verdict/Plan127VerdictCorpusLadderTests.cs`

### `Ashfall.Core.Tests/Verdict/Plan127VerdictCorpusLadderTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 111; SHA-256: `f7dd421b2dd58f510fb296dd2be3e73d959286e775adfffe63a09e6abae048ad`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
VerdictData_LoadsAll25CorruptionCorpusStrings
VerdictData_LoadsAll12WorldHistoryLadderEntries
MachineLogSystem_InjectsCorruptionMarkersFromExpandedCorpus
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.
