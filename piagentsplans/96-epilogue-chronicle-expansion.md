# Plan 96 — Epilogue Chronicle Slides and Ending Presentation Reachability

> **Rebuild status:** COMPLETE 20-SLIDE DATA/CORE LOOP — ENDING-TO-PRESENTATION REACHABILITY AUDIT
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-3`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round3-2026-09-25`
>
> **Current-evidence date:** 2026-09-25
>
> **Authority order:** live source and data → `AGENTS.md` → `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` → plan ledgers → this document.
>
> **Authority checksum:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
>
> **Length policy:** 150k–170k is the first quality checkpoint; 250k is an evidence-backed depth target, not a ceiling. The plan may exceed 250k when verified architecture and current evidence justify it, and it must stop rather than pad when that evidence is exhausted.

## 0. Integrity Statement and Plan Status

This file replaces unclaimed generated sections that mixed current evidence, fictional APIs, and unsupported save claims. It is a planning and architecture artifact only. It authorizes no production, data, test, save, generated-index, or UI edits. Every path labeled current must exist at rebuild time. Any future `CREATE` proposal is explicitly hypothetical and belongs to a later, separately claimed implementation package.

The rebuild follows four passes: content/current-reality first; integration framework second; accuracy and contradiction removal third; independent precision and handoff review fourth. Character count is recorded by external verification, not embedded recursively in the document.

# 1. Objective

- The historical baseline was five generic slides. The current `epilogue_chronicle.json` has 20 ordered rows and preserves the original opening/closing identities; the data expansion landed under DEC-269.
- The current builder is a deterministic presentation DTO wrapper: it copies and sorts caller-supplied slides, fate cards and metrics, but it does not itself resolve an ending key or select a branch-specific slide set. That distinction is central to the rebase.
- The safe next outcome is a reachability and truthfulness audit across `MusterSystem`/`EpilogueMatrix`, `CampaignEpilogueEngine`, `EpilogueChronicleBuilder`, `UnifiedEndingHostSession` and `EpiloguePanel`. New prose or new slide-selection authority is out of scope unless a current consumer gap is proven.

**Bounded outcome:** Retire the old pure-data 5→20 proposal as a new implementation project. The current catalog already contains 20 ordered slides and the Core builder deterministically orders supplied slides, fate cards and metrics. The remaining valuable work is a truthful bridge from the current Muster/Verdict ending owners into the chronicle projection and the existing epilogue UI, without inventing a second ending selector.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `Assets/StreamingAssets/Data/epilogue_chronicle.json` is valid schema version 1 with 20 `default_slides` rows ordered 0 through 19.
- `EpilogueChronicleLoader` reads the catalog and `EpilogueChronicleBuilder.Build` sorts slides by order, fate cards by ordinal survivor id and metrics by ordinal metric id.
- `EpilogueContextFactory` and `CampaignEpilogueEngine` own current campaign outcome facts; `EpilogueMatrix` and `MusterSystem` own ending-key resolution. The slide catalog is not an ending authority.
- Historical DEC-269 and the focused tests prove the 20-row catalog/builder contract landed; this planning rebuild does not claim those tests were freshly rerun.

**Master-authority sections applied to this rebase:**

- Part II Factory Protocol: premise sweep, collision check, one lane/cluster, and evidence labels before drafting.
- Part II Step 5 continuity and anti-duplication checklist: data presence is not reachability.
- Part III cluster map: use the live C1–C17 owner map rather than a historical plan title.
- Part IV backlog discipline: consume a verified candidate or record why it is stale; do not widen a bounded outcome.
- Part V Template S/R: subject intent and recommended route remain separate from implementation commitments.
- Part VI Multi-Session Growth Protocol: 250k is a depth target, not permission to manufacture volume.
- Live source/data authority: current catalog, loader, host, save, and focused tests outrank generated prose.
- Anti-padding rule: if the evidence queue is exhausted, stop and report no warranted continuation.
- C13 Endgame/epilogue cluster: keep ending resolution, chronicle presentation and unified-ending ownership distinct.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the old 5→20 data-only brief with a 20-row current catalog census and a precise ending-to-slide reachability matrix.
- Identify whether the current host supplies resolved ending context, survivor fates and metrics to the builder; do not add a second resolver if the current `EpilogueMatrix`/`UnifiedEndingResolver` already owns it.
- Audit the existing `EpiloguePanel` for truthful loading, empty/unknown ending, accessibility, close/back and refresh behavior.
- Preserve deterministic ordering and the existing deterministic build inputs; no RNG, wall-clock or asset lookup belongs in Core.

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
| 20-slide authored presentation metadata | EpilogueChronicleCatalogLoader | `Assets/Ashfall.Core/Endgame/EpilogueChronicleCatalog.cs` | The JSON loader owns catalog parsing and registration; it does not resolve campaign endings. |
| deterministic chronicle DTO ordering | EpilogueChronicleBuilder | `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs` | Owns stable ordering and projection assembly, not ending selection. |
| campaign outcome facts and context | EpilogueContextFactory/CampaignEpilogueEngine | `Assets/Ashfall.Core/Endgame/EpilogueContextFactory.cs; Assets/Ashfall.Core/Campaign/CampaignEpilogueEngine.cs` | Own outcome facts used by the presentation projection. |
| ending-key resolution and epilogue outcome | MusterSystem/EpilogueMatrix | `Assets/Ashfall.Core/Muster/MusterSystem.cs; Assets/Ashfall.Core/Muster/EpilogueMatrix.cs` | Own campaign ending resolution; slide rows cannot override it. |
| host composition and presentation | UnifiedEndingHostSession/EpiloguePanel | `src/Host/UnifiedEndingHostSession.cs; src/UI/EpiloguePanel.cs` | Projects current owner state and presents it; no gameplay decision in the panel. |
| catalog, builder and integration proof | Focused endgame tests | `Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs; Ashfall.Core.Tests/Endgame/EpilogueChronicleBuilderTests.cs; Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs` | Executable evidence surface, not a fresh pass claim. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Epilogue Chronicle Slides and Ending Presentation Reachability
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ EpilogueChronicleCatalogLoader
│   20-slide authored presentation metadata
│ EpilogueChronicleBuilder
│   deterministic chronicle DTO ordering
│ EpilogueContextFactory/CampaignEpilogueEngine
│   campaign outcome facts and context
│ MusterSystem/EpilogueMatrix
│   ending-key resolution and epilogue outcome
│ UnifiedEndingHostSession/EpiloguePanel
│   host composition and presentation
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

1. **Preserve current state ownership.** EpilogueChronicleCatalogLoader owns 20-slide authored presentation metadata: The JSON loader owns catalog parsing and registration; it does not resolve campaign endings.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| 20-slide authored presentation metadata | EpilogueChronicleCatalogLoader | `Assets/Ashfall.Core/Endgame/EpilogueChronicleCatalog.cs` | The JSON loader owns catalog parsing and registration; it does not resolve campaign endings. |
| deterministic chronicle DTO ordering | EpilogueChronicleBuilder | `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs` | Owns stable ordering and projection assembly, not ending selection. |
| campaign outcome facts and context | EpilogueContextFactory/CampaignEpilogueEngine | `Assets/Ashfall.Core/Endgame/EpilogueContextFactory.cs; Assets/Ashfall.Core/Campaign/CampaignEpilogueEngine.cs` | Own outcome facts used by the presentation projection. |
| ending-key resolution and epilogue outcome | MusterSystem/EpilogueMatrix | `Assets/Ashfall.Core/Muster/MusterSystem.cs; Assets/Ashfall.Core/Muster/EpilogueMatrix.cs` | Own campaign ending resolution; slide rows cannot override it. |
| host composition and presentation | UnifiedEndingHostSession/EpiloguePanel | `src/Host/UnifiedEndingHostSession.cs; src/UI/EpiloguePanel.cs` | Projects current owner state and presents it; no gameplay decision in the panel. |
| catalog, builder and integration proof | Focused endgame tests | `Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs; Ashfall.Core.Tests/Endgame/EpilogueChronicleBuilderTests.cs; Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs` | Executable evidence surface, not a fresh pass claim. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load the 20-slide catalog
2. resolve the ending key through the current Muster/Unified Ending owner
3. collect survivor fate cards and metrics from existing projections
4. construct `EpilogueChronicleInput`
5. build and ordinally sort the chronicle
6. project through the existing epilogue panel/unified ending host
7. capture only the existing owner save state

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Slide definitions are immutable catalog rows; the builder result is a deterministic presentation object and is not a second campaign save authority.
- Ending key, survivor fate facts, metrics and build seed must be supplied by their current owners and retain stable identity across restore.
- Unknown or empty input must produce the current documented fallback (`unknown`/empty lists) without inventing a favorable ending.
- A UI refresh may reorder nothing and may not consume RNG or mutate the ending owner.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Catalog order is integer-stable and duplicate order values are rejected or deterministically normalized by the loader before presentation.
- A missing slide asset token is a visible presentation concern, not a reason to invent an asset registry entry in Core.
- Unknown ending keys remain visibly unknown; the builder must not silently map them to a successful ending.
- The same ending facts and input lists produce byte-stable ordering across hosts.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `epilogue_chronicle.json` remains the only slide-definition authority.
- No new ending catalog, art catalog, or slide-selection JSON is justified before a current consumer gap is demonstrated.
- A future row requires an existing owner/host consumer, a stable order and a truthful asset token.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- No new save section is authorized. Use the existing endgame/unified-ending/muster owner state that supplies the projection.
- A future presentation-only field should be derived unless a current player decision requires persistence.
- Any change to ending state must pass the existing owner codec and migration tests, not a chronicle DTO test alone.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Builder ordering is ordinal and deterministic; no dictionary/hash iteration decides sequence.
- The build seed is recorded as an input fact but the current builder does not itself randomize.
- Paired host runs must compare ending key, ordered slide ids, fate-card ids, metric ids and visible text.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Muster/ending owners emit their existing resolution facts.
- The host may emit a chronicle-ready projection event only if the current event vocabulary already has a place for it; otherwise use direct composition.
- UI refresh is presentation-only and does not emit a gameplay fact.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/UnifiedEndingHostSession.cs
- src/Main.Muster.cs
- src/UI/EpiloguePanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Slide titles and prose remain fictional, restrained and grounded in the current campaign outcome.
- An epilogue is a truthful synthesis of state, not a generic victory screen that erases failure, scarcity or unresolved conflict.
- Placeholder art tokens must be labeled honestly in diagnostics and presentation rather than treated as generated final art.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | The host displays a successful ending for an unknown key. | EpilogueChronicleCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A catalog row is visible only in a test fixture and has no host consumer. | EpilogueChronicleBuilder | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Fate cards or metrics reorder after a refresh. | EpilogueContextFactory/CampaignEpilogueEngine | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | The panel mutates the ending owner or consumes a random stream. | MusterSystem/EpilogueMatrix | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new save section stores presentation-only data. | UnifiedEndingHostSession/EpiloguePanel | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Endgame/EpilogueChronicleBuilderTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — current census | Read the 20-row catalog, loader, builder, ending owners and panel. | Current row count and owner boundaries are proven. | No production path until the owning implementation package is separately claimed. |
| 1 — reachability trace | Trace ending key, fate cards and metrics into the host projection. | Every displayed field has a current owner. | No production path until the owning implementation package is separately claimed. |
| 2 — truthfulness/accessibility pass | Audit unknown, loading, empty, close/back and controller states. | No misleading or unreachable surface remains. | No production path until the owning implementation package is separately claimed. |
| 3 — precision handoff | Record only bounded residual work and exact focused commands. | No new slide authority is proposed without evidence. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/epilogue_chronicle.json | READ ONLY; MODIFY only for a proven row/consumer gap | 20-row authority |
| Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs | READ ONLY | Deterministic projection owner |
| Assets/Ashfall.Core/Muster/EpilogueMatrix.cs | READ ONLY | Ending-key owner |
| src/UI/EpiloguePanel.cs | READ ONLY; MODIFY only under a new host/UI claim | Presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Treating slide order as ending selection. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding a second campaign-ending resolver. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Embedding art or engine types in Core. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Claiming current runtime reachability from catalog presence alone. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new epilogue catalog.
- No new ending matrix.
- No production/data/test/UI changes in this planning package.
- No arbitrary slide-count growth.

# 23. Rollback and Recovery

- Revert the isolated planning document.
- Future host changes retain the current owner and prior valid save fixture.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 20 current slide rows and the 0–19 order are documented.
- Builder, ending owner, host and panel responsibilities are separated.
- Unknown/fallback/accessibility and deterministic ordering contracts are explicit.
- Focused test commands are named without claiming fresh execution.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the old 5→20 data-only brief with a 20-row current catalog census and a precise ending-to-slide reachability matrix.
- Identify whether the current host supplies resolved ending context, survivor fates and metrics to the builder; do not add a second resolver if the current `EpilogueMatrix`/`UnifiedEndingResolver` already owns it.
- Audit the existing `EpiloguePanel` for truthful loading, empty/unknown ending, accessibility, close/back and refresh behavior.
- Preserve deterministic ordering and the existing deterministic build inputs; no RNG, wall-clock or asset lookup belongs in Core.

## MUST NOT DO

- No new epilogue catalog.
- No new ending matrix.
- No production/data/test/UI changes in this planning package.
- No arbitrary slide-count growth.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Endgame/EpilogueChronicleBuilderTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — current census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: 20-slide authored presentation metadata → EpilogueChronicleCatalogLoader; deterministic chronicle DTO ordering → EpilogueChronicleBuilder; campaign outcome facts and context → EpilogueContextFactory/CampaignEpilogueEngine; ending-key resolution and epilogue outcome → MusterSystem/EpilogueMatrix; host composition and presentation → UnifiedEndingHostSession/EpiloguePanel; catalog, builder and integration proof → Focused endgame tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 96.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 96 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by EpilogueChronicleCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Endgame/EpilogueChronicleCatalog.cs`

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


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs`

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


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Endgame/EpilogueContextFactory.cs`

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


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/Campaign/CampaignEpilogueEngine.cs`

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


# Appendix B.06 — Current Code Architecture: `Assets/Ashfall.Core/Muster/EpilogueMatrix.cs`

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


# Appendix B.07 — Current Code Architecture: `src/Host/UnifiedEndingHostSession.cs`

### `src/Host/UnifiedEndingHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 112 lines / 4595 bytes.
- SHA-256: `0b9eaa9f93e463b86673fd24e811b3fc2f9e885e2723222a33403f3e8481b2dd`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=1; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class UnifiedEndingSaveStore
public const string FileName = "unified_ending_save.json";
public const string SectionName = "unified_ending";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCapturePersisted(UnifiedEndingSaveState state) => s_store.CaptureBare(state);
public static UnifiedEndingSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
public static bool TrySave(UnifiedEndingSaveState state) => s_store.TrySave(state);
public static UnifiedEndingSaveState? TryLoad() => s_store.TryLoad();
public sealed class UnifiedEndingHostSession : HostSessionBase
public event Action<UnifiedEndingResult>? EndingResolved;
public UnifiedEndingResolver Resolver => _resolver;
public string LastEvent => _lastEvent;
public bool IsResolved => _resolver.IsResolved;
public UnifiedEndingResult? LastResult => _resolver.LastResult;
public UnifiedEndingCensus Census => _resolver.GetCensus();
public static UnifiedEndingHostSession Create(string dataDir, UnifiedEndingResolver? resolver = null) {
public void LoadCatalog(string dataDir) {
public UnifiedEndingResult Resolve(UnifiedEndingContext context) {
public UnifiedEndingSaveState CaptureState() => _resolver.CaptureState();
public void RestoreState(UnifiedEndingSaveState state) {
```


# Appendix B.08 — Current Code Architecture: `src/Main.Muster.cs`

### `src/Main.Muster.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 491 lines / 21914 bytes.
- SHA-256: `57caac9a6e3bc274ef7a8aeefb4b4b56e34a81b6b713a3977d127d2f3c461ad1`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=7; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public void OnColdCountClicked() {
public void OnHydroBaronsClicked() {
public void OnIronRaidersClicked() {
public void OnLongWalkClicked() {
public void OnProvisionedClicked() {
public void OnScavengerGuildClicked() {
public void OnMusterEscalateClicked() {
public bool Deliver(string itemId, int amount) {
```


# Appendix B.09 — Current Code Architecture: `src/UI/EpiloguePanel.cs`

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


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/epilogue_chronicle.json`

### `Assets/StreamingAssets/Data/epilogue_chronicle.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 1951 bytes / 1951 characters.
- SHA-256: `1f190e791f5b90afb9578f9a2b4f263e6e5e52b452ecdb5bb841c2c168b0c9a7`.
- Root keys: `default_slides`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
default_slides: min=20, max=20, observed_paths=1
```

Representative record fields:

- `art_asset_id`
- `order`
- `title`


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/muster_epilogues.json`

### `Assets/StreamingAssets/Data/muster_epilogues.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 11862 bytes / 11856 characters.
- SHA-256: `5a317510babab25b13eaa6865014ed42b1e5578b0ff9201082ea5176ae3a6c6f`.
- Root keys: `epilogues`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
epilogues: min=25, max=25, observed_paths=1
```

Representative record fields:

- `ending_key`
- `prose`
- `title`


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs`

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


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Endgame/EpilogueChronicleBuilderTests.cs`

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


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs`

### `Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 250; SHA-256: `5d83bff7932ba9f38979191888fb0df4091c7646976a396dd2507dfbd2776db6`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan96_EpilogueChronicle_LoadsAllTwentySlides_WithStrictContiguityAndPlaceholderConventions
Plan104_NarrativeQuestlines_LoadsAllTwelveArcs_WithValidFourStageChainsAndBinaryCrisis
CrossSystem_SurvivorArcResolutionAndEndgameChronicle_ExhibitNarrativeCoherence
CrossSystem_DeterministicExecution_UnderRepeatedReloadsPasses
```


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Endgame/EpilogueChronicleCatalog.cs`

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


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs`

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


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/Endgame/EpilogueContextFactory.cs`

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


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/Campaign/CampaignEpilogueEngine.cs`

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


# Appendix E.19 — Supporting Code Evidence: `Assets/Ashfall.Core/Muster/EpilogueMatrix.cs`

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


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs`

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


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/Endgame/EpilogueChronicleBuilderTests.cs`

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


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs`

### `Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 250; SHA-256: `5d83bff7932ba9f38979191888fb0df4091c7646976a396dd2507dfbd2776db6`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan96_EpilogueChronicle_LoadsAllTwentySlides_WithStrictContiguityAndPlaceholderConventions
Plan104_NarrativeQuestlines_LoadsAllTwelveArcs_WithValidFourStageChainsAndBinaryCrisis
CrossSystem_SurvivorArcResolutionAndEndgameChronicle_ExhibitNarrativeCoherence
CrossSystem_DeterministicExecution_UnderRepeatedReloadsPasses
```


# Appendix H.23 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

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
| 20-slide authored presentation metadata | EpilogueChronicleCatalogLoader | deterministic chronicle DTO ordering | EpilogueChronicleBuilder | Owner emits/reads a typed fact; no mirror state. |
| 20-slide authored presentation metadata | EpilogueChronicleCatalogLoader | campaign outcome facts and context | EpilogueContextFactory/CampaignEpilogueEngine | Owner emits/reads a typed fact; no mirror state. |
| 20-slide authored presentation metadata | EpilogueChronicleCatalogLoader | ending-key resolution and epilogue outcome | MusterSystem/EpilogueMatrix | Owner emits/reads a typed fact; no mirror state. |
| 20-slide authored presentation metadata | EpilogueChronicleCatalogLoader | host composition and presentation | UnifiedEndingHostSession/EpiloguePanel | Owner emits/reads a typed fact; no mirror state. |
| 20-slide authored presentation metadata | EpilogueChronicleCatalogLoader | catalog, builder and integration proof | Focused endgame tests | Owner emits/reads a typed fact; no mirror state. |
| deterministic chronicle DTO ordering | EpilogueChronicleBuilder | 20-slide authored presentation metadata | EpilogueChronicleCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| deterministic chronicle DTO ordering | EpilogueChronicleBuilder | campaign outcome facts and context | EpilogueContextFactory/CampaignEpilogueEngine | Owner emits/reads a typed fact; no mirror state. |
| deterministic chronicle DTO ordering | EpilogueChronicleBuilder | ending-key resolution and epilogue outcome | MusterSystem/EpilogueMatrix | Owner emits/reads a typed fact; no mirror state. |
| deterministic chronicle DTO ordering | EpilogueChronicleBuilder | host composition and presentation | UnifiedEndingHostSession/EpiloguePanel | Owner emits/reads a typed fact; no mirror state. |
| deterministic chronicle DTO ordering | EpilogueChronicleBuilder | catalog, builder and integration proof | Focused endgame tests | Owner emits/reads a typed fact; no mirror state. |
| campaign outcome facts and context | EpilogueContextFactory/CampaignEpilogueEngine | 20-slide authored presentation metadata | EpilogueChronicleCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| campaign outcome facts and context | EpilogueContextFactory/CampaignEpilogueEngine | deterministic chronicle DTO ordering | EpilogueChronicleBuilder | Owner emits/reads a typed fact; no mirror state. |
| campaign outcome facts and context | EpilogueContextFactory/CampaignEpilogueEngine | ending-key resolution and epilogue outcome | MusterSystem/EpilogueMatrix | Owner emits/reads a typed fact; no mirror state. |
| campaign outcome facts and context | EpilogueContextFactory/CampaignEpilogueEngine | host composition and presentation | UnifiedEndingHostSession/EpiloguePanel | Owner emits/reads a typed fact; no mirror state. |
| campaign outcome facts and context | EpilogueContextFactory/CampaignEpilogueEngine | catalog, builder and integration proof | Focused endgame tests | Owner emits/reads a typed fact; no mirror state. |
| ending-key resolution and epilogue outcome | MusterSystem/EpilogueMatrix | 20-slide authored presentation metadata | EpilogueChronicleCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| ending-key resolution and epilogue outcome | MusterSystem/EpilogueMatrix | deterministic chronicle DTO ordering | EpilogueChronicleBuilder | Owner emits/reads a typed fact; no mirror state. |
| ending-key resolution and epilogue outcome | MusterSystem/EpilogueMatrix | campaign outcome facts and context | EpilogueContextFactory/CampaignEpilogueEngine | Owner emits/reads a typed fact; no mirror state. |
| ending-key resolution and epilogue outcome | MusterSystem/EpilogueMatrix | host composition and presentation | UnifiedEndingHostSession/EpiloguePanel | Owner emits/reads a typed fact; no mirror state. |
| ending-key resolution and epilogue outcome | MusterSystem/EpilogueMatrix | catalog, builder and integration proof | Focused endgame tests | Owner emits/reads a typed fact; no mirror state. |
| host composition and presentation | UnifiedEndingHostSession/EpiloguePanel | 20-slide authored presentation metadata | EpilogueChronicleCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| host composition and presentation | UnifiedEndingHostSession/EpiloguePanel | deterministic chronicle DTO ordering | EpilogueChronicleBuilder | Owner emits/reads a typed fact; no mirror state. |
| host composition and presentation | UnifiedEndingHostSession/EpiloguePanel | campaign outcome facts and context | EpilogueContextFactory/CampaignEpilogueEngine | Owner emits/reads a typed fact; no mirror state. |
| host composition and presentation | UnifiedEndingHostSession/EpiloguePanel | ending-key resolution and epilogue outcome | MusterSystem/EpilogueMatrix | Owner emits/reads a typed fact; no mirror state. |
| host composition and presentation | UnifiedEndingHostSession/EpiloguePanel | catalog, builder and integration proof | Focused endgame tests | Owner emits/reads a typed fact; no mirror state. |
| catalog, builder and integration proof | Focused endgame tests | 20-slide authored presentation metadata | EpilogueChronicleCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| catalog, builder and integration proof | Focused endgame tests | deterministic chronicle DTO ordering | EpilogueChronicleBuilder | Owner emits/reads a typed fact; no mirror state. |
| catalog, builder and integration proof | Focused endgame tests | campaign outcome facts and context | EpilogueContextFactory/CampaignEpilogueEngine | Owner emits/reads a typed fact; no mirror state. |
| catalog, builder and integration proof | Focused endgame tests | ending-key resolution and epilogue outcome | MusterSystem/EpilogueMatrix | Owner emits/reads a typed fact; no mirror state. |
| catalog, builder and integration proof | Focused endgame tests | host composition and presentation | UnifiedEndingHostSession/EpiloguePanel | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the old 5→20 data-only brief with a 20-row current catalog census and a precise ending-to-slide reachability matrix. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Identify whether the current host supplies resolved ending context, survivor fates and metrics to the builder; do not add a second resolver if the current `EpilogueMatrix`/`UnifiedEndingResolver` already owns it. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Audit the existing `EpiloguePanel` for truthful loading, empty/unknown ending, accessibility, close/back and refresh behavior. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Preserve deterministic ordering and the existing deterministic build inputs; no RNG, wall-clock or asset lookup belongs in Core. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix — Master authority re-read

# Appendix — Master Authority Re-read Record

The following excerpts were selected from the live master authority by this plan's subject terms. They are planning constraints, not claims that the historical backlog is current.

> **DR-04 — The data catalog inventory has grown; several catalogs are absent from the v1.0 inventory. VERIFIED.**
`Assets/StreamingAssets/Data/` currently holds 342 entries. Catalogs observed live but not present in the v1.0 Part 5.4 inventory include: `dive_sites.json`, `hydroponic_crops.json`, `hydraulic_extrusion_catalog.json`, `metrology_standards_catalog.json`, `muster_camp_scenes.json`, `muster_epilogues.json`, `muster_faction_actions.json`, `muster_faction_culture.json`, `muster_witnesses.json`, `utility_actions.json`, `moral_choice_quests_branching.json`, `moral_choice_quests_distress.json`. Consequence: the duplication firewall (v1.0 Part 5) is stale in these domains; a planner could propose a "new" muster or moral-choice catalog that already exists. The ID-collision sweep in Factory Protocol step 1 must always run against the live listing, never against this document.

> - One plan = one bounded outcome riding existing seams (v1.0 Part 10). The factory never widens a plan to reach a size target.
- No plan may create a parallel authority. Every state change names its owning system.
- Data-first preference: if an expansion can be authored as JSON through an existing loader, it must be, and the plan must say so.
- The factory never drafts against decision-blocked items (the current list must be re-read from `INTEGRATION_PLANS.md` each session — DR-06 shows signatures resolve over time).
- Subject plans do not edit files. Implementation happens only in an owning session after plan selection (v1.0 approval-based workflow).
- Every generated plan must state its position relative to each epilogue permutation it touches (v1.0 Part 6.6).

> C1 Shelter operations (rooms, thermal, schedules, fire, decor, barter, noise, prisoners, sanitation, airlock, decon, atmosphere) · C2 Medical pipeline (disease, dose ledger, ARS, surgery, autopsy, pharma, diagnostics, therapies, dependency, crises) · C3 Water, food, agriculture (treatment, condensers, wells, brine, nutrition, kitchen, preservation, grain, greenhouse, crops, aquaponics, apiculture) · C4 Power and industry (grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, coatings, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology) · C5 Expeditions and travel (destinations, scavenging tables, vehicles, waystations, caravans, routes, travel encounters, micro-locations) · C6 Map and geography (wasteland map, damaged zones, fog, route gates, cartography, survey instruments) · C7 Factions and war (stance, doctrines, war chains, tributes, treaties, embargoes, espionage, psyops, infiltration, musters, labor camps, bounties) · C8 Radio and information (stations, programs, intercepts, distress signals, rumors, sound ranging, direction finding, NVIS, heliograph) · C9 Survivors and interiority (needs, skills, traits, arcs, trauma, guilt, therapies, relations, caregiving, beliefs, rituals, memorials, final wishes, lineage, cohorts, apprenticeships) · C10 Quests and moral choice (questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip, branching, bureaucratic morality, expansion quests) · C11 Economy (market, baselines, regional prices, shocks, rumors, black market, debt ledger, tributes, trade screens, tell lines) · C12 Weather and Year of Ash (weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash families, epilogue pressure) · C13 Endgame and epilogue (Reckoning, verdict, epilogue matrix, chronicle, muster epilogues, standing records, census) · C14 Ecology and wildlife (migration, trapping, ecosystem, bestiary, flora, infestations, contagion, pathogens, crop genomes) · C15 Defense and security (perimeter, defense grid, sky defense, ordnance, chemical defense, orbital harrow, interlocks, EMP effects) · C16 Progression and meta (skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, bestiary, L10N, mods, settings, input) · C17 Host surface and UI (panels, shell, focus navigation, snapshots, a11y, briefings, dashboards).

> | Cluster | Opening archetype | Confidence |
|---|---|---|
| C1 | Bureaucratic texture for under-documented rooms: shift notices, maintenance glitch reports, load-shed amendments for rooms lacking corpus coverage | HIGH CONFIDENCE |
| C2 | Casebook and therapy-note expansion for affliction states with thin prose coverage; dose-treatment narrative pairing against `MEDICAL_DOSE_TREATMENT_MATRIX.md` | HIGH CONFIDENCE |
| C3 | Assay/log corpus for preservation and processing chains that have catalogs but no narrative corpus twin (v1.0 Part 16.4 pattern: every process ships technical + prose) | HIGH CONFIDENCE |
| C4 | Same pattern for the newest industrial catalogs confirmed live in DR-04 (`hydraulic_extrusion`, `metrology_standards`): audit records, calibration logs | HIGH CONFIDENCE |
| C5 | Expedition field reports and waypoint notes for destinations with sparse `arrival_description`/`revisit_description` coverage; route-waypoint batches | HIGH CONFIDENCE |
| C6 | Gazetteer entries and damaged-zone survey prose; cartographic marginalia | INFERENCE — verify current coverage |
| C7 | Communiqué, directive, and verdict-corpus expansion for factions with thin public/private language separation | HIGH CONFIDENCE |
| C8 | Radio rundown/transcript batches for stations with thin programming; numbers-station and cipher follow-ups | HIGH CONFIDENCE — but distress-signal content is SEALED under `CF-P1-DISTRESS-CONTENT-SEAL` (DR-06); do not add signal scenarios |
| C9 | Delayed moral-choice callbacks (~100-day returns) via `IFlagLedger` flags; phantom-memory triggers tied to surviving cohorts | HIGH CONFIDENCE (v1.0 Part 7 gap 2) |
| C10 | Quest prose fields (`quest_hook`, `objective_text`, outcome texts) for quest records with skeleton prose; follow Part 9 contracts exactly | HIGH CONFIDENCE |
| C11 | Ledger, statement, and debt-template prose; rumor batches within deterministic bands | HIGH CONFIDENCE |
| C12 | Mid-winter slump pressure (Days 90–180) story arcs; storm-window almanac entries | HIGH CONFIDENCE (v1.0 Part 7 gap 1) |
| C13 | Epilogue-chronicle depth for under-served permutations of the 32-permutation matrix | HIGH CONFIDENCE |
| C14 | Bestiary and natural-history corpus extension; mutated-botanical and limnology follow-on batches | HIGH CONFIDENCE |
| C15 | Defense-log and ordnance-manifest prose; orbital-harrow telemetry transcripts | INFERENCE — verify coverage |
| C16 | Codex and field-guide entries for systems that gained content since the last codex wave | HIGH CONFIDENCE |
| C17 | Ambient environmental text and atmosphere cues for panels rendering newer systems with sparse surface prose | INFERENCE — verify via `--ui-layout-selftest` and snapshot coverage |

> | Cluster | Opening archetype | Confidence |
|---|---|---|
| C1 | Room-level effect extensions routed through `IsRoomPowered`; shelter-failure follow-ons building on the quarantined failure-effects wiring logs observed in `docs/plans/` | HIGH CONFIDENCE |
| C2 | Ward-staffing and recovery-ramp follow-ons are CLOSED (Plan 24, DR-06); open instead: cross-links between medical and cohort/lineage (child health), and between dose ledger and Year-of-Ash fallout windows | PROPOSAL — premise sweep required |
| C3 | Zoonosis-style bridges: kitchen/preservation × disease; cellar-rot × greenhouse economics; apiculture × morale | PROPOSAL |
| C4 | Bind the newest industrial catalogs (DR-04) into consumption/production ledgers through the existing power-grid and foundry seams | PROPOSAL — needs live loader verification |
| C5 | Per-destination scavenging-table parity for destinations beyond the 49-table coverage; vehicle-breakdown consequences into medical and dose ledgers | HIGH CONFIDENCE |
| C6 | Flooded-route topology tags and authored map edges (foreman-flagged open decision — needs the named signature first) | BLOCKED — decision-gated |
| C7 | FactionWar per-strike emitter extension (foreman-flagged open decision — needs signature) | BLOCKED — decision-gated |
| C8 | Radio-signal follow-up chaining is SEALED (DISTRESS-SIGNALS-9-12 COMPLETE, DR-06); open instead: market-rumor band extension and intercept-driven journal depth | HIGH CONFIDENCE |
| C9 | Survivor interiority bridges: belief movements × faction stance; memorial rites × epilogue evidence; chemical dependency × medical ward | PROPOSAL |
| C10 | Quest state reopening after new discoveries (failure-recovery grammar, v1.0 Part 6.7); moral-choice flag consumers beyond the flag ledger | HIGH CONFIDENCE |
| C11 | Black-market funds/goods legs remain decision-gated (canonical funds authority); merchant restock priority is SEALED by DEC-05 (DR-06) | BLOCKED / SEALED |
| C12 | Year-of-Ash tick-window extensions (180–360) for systems not yet producing winter pressure | PROPOSAL |
| C13 | Reckoning evidence enrollment for systems added since the last endgame wave (19A/19B/19C closed, DR-06) | HIGH CONFIDENCE |
| C14 | Trapping→disease zoonosis bridge exists; open: migration × expedition route encounters; infestation × crop economy | PROPOSAL |
| C15 | EMP effects exist (shelter EMP/medical power logs observed); open: defense grid × warlord siege math; sky-armor × orbital harrow telemetry | PROPOSAL |
| C16 | XP Expansion W1 is ACTIVE (DR-06): difficulty-authority consumer binding is the sanctioned open seam in this cluster — extend it, do not parallel it | HIGH CONFIDENCE |
| C17 | Panels rendering stale or missing data for newer systems; verify against `--ui-layout-selftest` before claiming | HIGH CONFIDENCE |

> | Cluster | Opening archetype | Confidence |
|---|---|---|
| Any stateful extension | Codec bump + migration + round-trip per v1.0 Part 12.3; old-save→new-build fixtures through the `SaveSupportWindowTests` corpus | CANON process |
| C9 | Lineage/cohort long-horizon state (3-year simulation exists per 19B closeout, DR-06): verify horizon coverage before extending | HIGH CONFIDENCE |
| C13 | Epilogue evidence persistence: which Day-360+ facts survive into the Day-3650 window | HIGH CONFIDENCE |
| Cross-cutting | Mid-event and mid-combat save round-trips for exactly-once effect classes beyond the rescue-signal runtime (which models the pattern) | PROPOSAL |

> | Cluster | Opening archetype | Confidence |
|---|---|---|
| C17 | High-frequency UI rebuild audits (metric cards, data grids) — measure first via the CI performance gate | Potential hotspot — profile before rewrite |
| C12 | Year-of-Ash tick-window cost concentration (Days 180–360): per-day work spikes during storm windows | Potential hotspot — requires measurement |
| C13 | Epilogue-matrix evaluation cost at Day 360 — one-shot, likely fine; measure only if reported | HYPOTHESIS |
| All | No optimization plan without before/after numbers in `docs/perf/` | CANON process |

> | Cluster | Opening archetype | Confidence |
|---|---|---|
| C2 | Dose-treatment matrix paired tests against `MEDICAL_DOSE_TREATMENT_MATRIX.md` | HIGH CONFIDENCE |
| C11 | Debt-ledger consequence dispatcher coverage; rumor-band determinism pins | HIGH CONFIDENCE |
| C10 | Moral-choice flag consumer coverage for newly added consumers | HIGH CONFIDENCE |
| C13 | Epilogue permutation reachability tests for under-served permutations | HIGH CONFIDENCE |
| Cross | Determinism two-pass proofs for every new simulation; TEST-AGGREGATION metadata for catalog checks | CANON process |

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

The subject is the boundary between a resolved campaign ending and a deterministic, truthful epilogue presentation. The plan expands the evidence and integration route around that boundary; it does not grant the slide catalog authority over campaign outcomes.

- **20-slide authored presentation metadata** remains with `EpilogueChronicleCatalogLoader` at `Assets/Ashfall.Core/Endgame/EpilogueChronicleCatalog.cs`. The JSON loader owns catalog parsing and registration; it does not resolve campaign endings.
- **deterministic chronicle DTO ordering** remains with `EpilogueChronicleBuilder` at `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs`. Owns stable ordering and projection assembly, not ending selection.
- **campaign outcome facts and context** remains with `EpilogueContextFactory/CampaignEpilogueEngine` at `Assets/Ashfall.Core/Endgame/EpilogueContextFactory.cs; Assets/Ashfall.Core/Campaign/CampaignEpilogueEngine.cs`. Own outcome facts used by the presentation projection.
- **ending-key resolution and epilogue outcome** remains with `MusterSystem/EpilogueMatrix` at `Assets/Ashfall.Core/Muster/MusterSystem.cs; Assets/Ashfall.Core/Muster/EpilogueMatrix.cs`. Own campaign ending resolution; slide rows cannot override it.
- **host composition and presentation** remains with `UnifiedEndingHostSession/EpiloguePanel` at `src/Host/UnifiedEndingHostSession.cs; src/UI/EpiloguePanel.cs`. Projects current owner state and presents it; no gameplay decision in the panel.
- **catalog, builder and integration proof** remains with `Focused endgame tests` at `Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs; Ashfall.Core.Tests/Endgame/EpilogueChronicleBuilderTests.cs; Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs`. Executable evidence surface, not a fresh pass claim.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load the 20-slide catalog
2. resolve the ending key through the current Muster/Unified Ending owner
3. collect survivor fate cards and metrics from existing projections
4. construct `EpilogueChronicleInput`
5. build and ordinally sort the chronicle
6. project through the existing epilogue panel/unified ending host
7. capture only the existing owner save state

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Slide definitions are immutable catalog rows; the builder result is a deterministic presentation object and is not a second campaign save authority.
- Ending key, survivor fate facts, metrics and build seed must be supplied by their current owners and retain stable identity across restore.
- Unknown or empty input must produce the current documented fallback (`unknown`/empty lists) without inventing a favorable ending.
- A UI refresh may reorder nothing and may not consume RNG or mutate the ending owner.

- Catalog order is integer-stable and duplicate order values are rejected or deterministically normalized by the loader before presentation.
- A missing slide asset token is a visible presentation concern, not a reason to invent an asset registry entry in Core.
- Unknown ending keys remain visibly unknown; the builder must not silently map them to a successful ending.
- The same ending facts and input lists produce byte-stable ordering across hosts.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/UnifiedEndingHostSession.cs
- src/Main.Muster.cs
- src/UI/EpiloguePanel.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs
- Ashfall.Core.Tests/Endgame/EpilogueChronicleBuilderTests.cs
- Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs

These commands are intentionally small and named. A planning rebuild does not run them and does not convert historical pass counts into fresh evidence. An implementation package records the actual command, result, fixture and limitation.

## F. Precision questions for the next owner

- Which current method is the single mutation point for each durable fact?
- Which loader and validator prove that every authored row is admitted?
- Which host command makes the feature reachable from the live game?
- Which existing save section carries the fact, and what is its frozen legacy shape?
- Which deterministic stream, ordering rule or no-RNG contract governs repeated execution?
- What visible refusal prevents a player from mistaking a projection for authority?
- What focused test would fail if the owner were bypassed?
- What future file or type is explicitly *not* part of this package?


# Appendix — Scenario matrix

# Appendix — Scenario and Negative-Contract Matrix

| ID | Scenario | Precondition | Expected owner outcome | Negative proof | Owner |
| --- | --- | --- | --- | --- | --- |
| S-01 | 96-01 catalog load with all 20 slides | load the 20-slide catalog | Slide definitions are immutable catalog rows; the builder result is a deterministic presentation object and is not a second campaign save authority. | The host displays a successful ending for an unknown key. | EpilogueChronicleCatalogLoader |
| S-02 | 96-02 ending key resolution before builder call | resolve the ending key through the current Muster/Unified Ending owner | Ending key, survivor fate facts, metrics and build seed must be supplied by their current owners and retain stable identity across restore. | A catalog row is visible only in a test fixture and has no host consumer. | EpilogueChronicleCatalogLoader |
| S-03 | 96-03 unknown ending fallback | collect survivor fate cards and metrics from existing projections | Unknown or empty input must produce the current documented fallback (`unknown`/empty lists) without inventing a favorable ending. | Fate cards or metrics reorder after a refresh. | EpilogueChronicleCatalogLoader |
| S-04 | 96-04 empty survivor fate list | construct `EpilogueChronicleInput` | A UI refresh may reorder nothing and may not consume RNG or mutate the ending owner. | The panel mutates the ending owner or consumes a random stream. | EpilogueChronicleCatalogLoader |
| S-05 | 96-05 metric ordering with duplicate labels | build and ordinally sort the chronicle | Slide definitions are immutable catalog rows; the builder result is a deterministic presentation object and is not a second campaign save authority. | A new save section stores presentation-only data. | EpilogueChronicleCatalogLoader |
| S-06 | 96-06 restore followed by rebuild | project through the existing epilogue panel/unified ending host | Ending key, survivor fate facts, metrics and build seed must be supplied by their current owners and retain stable identity across restore. | The host displays a successful ending for an unknown key. | EpilogueChronicleCatalogLoader |
| S-07 | 96-07 panel refresh without mutation | capture only the existing owner save state | Unknown or empty input must produce the current documented fallback (`unknown`/empty lists) without inventing a favorable ending. | A catalog row is visible only in a test fixture and has no host consumer. | EpilogueChronicleCatalogLoader |
| S-08 | 96-08 placeholder asset diagnostic | load the 20-slide catalog | A UI refresh may reorder nothing and may not consume RNG or mutate the ending owner. | Fate cards or metrics reorder after a refresh. | EpilogueChronicleCatalogLoader |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 96-TC-01 catalog schema and row uniqueness | data | catalog schema and row uniqueness; verify the current owner and its negative boundary without inventing a second authority. | EpilogueChronicleCatalogLoader |
| T-02 | 96-TC-02 original five slide preservation | unit | original five slide preservation; verify the current owner and its negative boundary without inventing a second authority. | EpilogueChronicleCatalogLoader |
| T-03 | 96-TC-03 order sorting and duplicate order handling | persistence | order sorting and duplicate order handling; verify the current owner and its negative boundary without inventing a second authority. | EpilogueChronicleCatalogLoader |
| T-04 | 96-TC-04 builder deep-copy isolation | determinism | builder deep-copy isolation; verify the current owner and its negative boundary without inventing a second authority. | EpilogueChronicleCatalogLoader |
| T-05 | 96-TC-05 unknown ending fallback | host | unknown ending fallback; verify the current owner and its negative boundary without inventing a second authority. | EpilogueChronicleCatalogLoader |
| T-06 | 96-TC-06 ordinal fate-card ordering | UI/accessibility | ordinal fate-card ordering; verify the current owner and its negative boundary without inventing a second authority. | EpilogueChronicleCatalogLoader |
| T-07 | 96-TC-07 ordinal metric ordering | cross-system | ordinal metric ordering; verify the current owner and its negative boundary without inventing a second authority. | EpilogueChronicleCatalogLoader |
| T-08 | 96-TC-08 same input deterministic replay | data | same input deterministic replay; verify the current owner and its negative boundary without inventing a second authority. | EpilogueChronicleCatalogLoader |
| T-09 | 96-TC-09 host projection receives owner facts | unit | host projection receives owner facts; verify the current owner and its negative boundary without inventing a second authority. | EpilogueChronicleCatalogLoader |
| T-10 | 96-TC-10 panel empty/error state | persistence | panel empty/error state; verify the current owner and its negative boundary without inventing a second authority. | EpilogueChronicleCatalogLoader |
| T-11 | 96-TC-11 keyboard/controller close and focus | determinism | keyboard/controller close and focus; verify the current owner and its negative boundary without inventing a second authority. | EpilogueChronicleCatalogLoader |
| T-12 | 96-TC-12 no engine references in Core | host | no engine references in Core; verify the current owner and its negative boundary without inventing a second authority. | EpilogueChronicleCatalogLoader |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 91 | `Ashfall.Core.Tests/MusterEpilogueMatrixTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 28 | `Ashfall.Core.Tests/Governance/Plan89_98MusterFactionIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 19 | `Ashfall.Core.Tests/Endgame/Plan19EndingContinuityTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 11 | `Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 11 | `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Ashfall.Core.Tests/Endgame/Plan145UnifiedEndingIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `src/Host/UnifiedEndingHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `Ashfall.Core.Tests/Endgame/EpilogueChronicleBuilderTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/Medical/Plan16_19TriageEpilogueIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/Endgame/Plan19SessionContinuityJourneyTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `src/Host/HostCli.UnifiedEnding.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Endgame/Plan145UnifiedEndingHostIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/MusterContentCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Endgame/EpilogueChronicleCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Muster/EpilogueMatrix.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Main.UnifiedEnding.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/UI/EpiloguePanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Endgame/CampaignOutcomeEvaluator.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Host/HostCli.PanelTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Main.UiPanels.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Assets/Ashfall.Core/Endgame/UnifiedEndingResolver.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Host/ExpansionHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/VerdictIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Endgame/EpilogueContextFactory.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/HostCliRegistry.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Verdict/VerdictEndingEvaluator.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/HostCli.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/MusterHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Main.Endgame.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Main.ExpansionHub.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Main.PlayerSurfaces.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/epilogue_chronicle.json`

### `Assets/StreamingAssets/Data/epilogue_chronicle.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 1951; characters: 1951.
- SHA-256: `1f190e791f5b90afb9578f9a2b4f263e6e5e52b452ecdb5bb841c2c168b0c9a7`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `default_slides`

#### `default_slides` — 20 current rows

- Row 001 `epilogue_opening_placeholder`: `{"art_asset_id":"epilogue_opening_placeholder","order":0,"title":"Opening"}`
- Row 002 `epilogue_exchange_placeholder`: `{"art_asset_id":"epilogue_exchange_placeholder","order":1,"title":"After the Flash"}`
- Row 003 `epilogue_bunker_placeholder`: `{"art_asset_id":"epilogue_bunker_placeholder","order":2,"title":"The Bunker"}`
- Row 004 `epilogue_first_winter_placeholder`: `{"art_asset_id":"epilogue_first_winter_placeholder","order":3,"title":"First Winter"}`
- Row 005 `epilogue_resources_placeholder`: `{"art_asset_id":"epilogue_resources_placeholder","order":4,"title":"Water and Heat"}`
- Row 006 `epilogue_survivors_placeholder`: `{"art_asset_id":"epilogue_survivors_placeholder","order":5,"title":"Survivors"}`
- Row 007 `epilogue_empty_bunks_placeholder`: `{"art_asset_id":"epilogue_empty_bunks_placeholder","order":6,"title":"Empty Bunks"}`
- Row 008 `epilogue_factions_placeholder`: `{"art_asset_id":"epilogue_factions_placeholder","order":7,"title":"The Factions"}`
- Row 009 `epilogue_trade_roads_placeholder`: `{"art_asset_id":"epilogue_trade_roads_placeholder","order":8,"title":"Lines on the Map"}`
- Row 010 `epilogue_radio_placeholder`: `{"art_asset_id":"epilogue_radio_placeholder","order":9,"title":"Voices in Static"}`
- Row 011 `epilogue_investigations_placeholder`: `{"art_asset_id":"epilogue_investigations_placeholder","order":10,"title":"The Verdict"}`
- Row 012 `epilogue_witnesses_placeholder`: `{"art_asset_id":"epilogue_witnesses_placeholder","order":11,"title":"The Witnesses"}`
- Row 013 `epilogue_relics_placeholder`: `{"art_asset_id":"epilogue_relics_placeholder","order":12,"title":"Restored Relics"}`
- Row 014 `epilogue_key_decisions_placeholder`: `{"art_asset_id":"epilogue_key_decisions_placeholder","order":13,"title":"What We Chose"}`
- Row 015 `epilogue_coalition_placeholder`: `{"art_asset_id":"epilogue_coalition_placeholder","order":14,"title":"The Muster"}`
- Row 016 `epilogue_resolution_placeholder`: `{"art_asset_id":"epilogue_resolution_placeholder","order":15,"title":"The Resolution"}`
- Row 017 `epilogue_census_placeholder`: `{"art_asset_id":"epilogue_census_placeholder","order":16,"title":"The Census"}`
- Row 018 `epilogue_remains_placeholder`: `{"art_asset_id":"epilogue_remains_placeholder","order":17,"title":"What Remains"}`
- Row 019 `epilogue_future_placeholder`: `{"art_asset_id":"epilogue_future_placeholder","order":18,"title":"After Us"}`
- Row 020 `epilogue_final_placeholder`: `{"art_asset_id":"epilogue_final_placeholder","order":19,"title":"Final Word"}`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/muster_epilogues.json`

### `Assets/StreamingAssets/Data/muster_epilogues.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 11862; characters: 11856.
- SHA-256: `5a317510babab25b13eaa6865014ed42b1e5578b0ff9201082ea5176ae3a6c6f`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `epilogues`

#### `epilogues` — 25 current rows

- Row 001 `the_open_muster`: `{"ending_key":"the_open_muster","prose":"The coalition held the substation and the ground around it, and when Day 320 came the Garrison chose not to press it. The rally point is a town now, of a kind. Nobody calls it anything official. The…`
- Row 002 `the_amnesty`: `{"ending_key":"the_amnesty","prose":"The petition carried on paper, which in the end mattered more than anything fired. The coalition is absorbed into a demobilized-conscript status, the only path in this document that ends their fugitive …`
- Row 003 `the_corridor`: `{"ending_key":"the_corridor","prose":"There was no siege, because there was no camp. Small groups, quiet routes, a corridor instead of a rally: nobody won a fight that never happened. Whatever was being rallied to, it left Sector 4 a littl…`
- Row 004 `the_blood_price`: `{"ending_key":"the_blood_price","prose":"The location was reported, the standing restored, the resupply collected. The rallied members — Vask among them — are gone. No other faction in the sector finds out how, specifically, unless the jou…`
- Row 005 `the_rate_card_revised`: `{"ending_key":"the_rate_card_revised","prose":"By undercut, audit, or contract, the rate card is finally a published price. Water stops being a weapon anyone can wield unilaterally. Odalen keeps her job in two of the three versions of how …`
- Row 006 `the_administrator`: `{"ending_key":"the_administrator","prose":"Unit 4 has a new administrator, unaccountable to anyone who used to run the queue. The rate card did not get fairer; it got renamed. The thirsty season passed, and the shelter that took the plant …`
- Row 007 `the_measured_truth_contested`: `{"ending_key":"the_measured_truth_contested","prose":"The broadcast fired with a caveat on it, and Garrison heard exactly how credible a caveated finding is. The measurement stands; the consequences are softer, weaker, and still coming. So…`
- Row 008 `the_measured_truth`: `{"ending_key":"the_measured_truth","prose":"The provenance run completed before Day 300, and the broadcast carried no caveat. Whatever the truth is, it now has a date, a lab, and four names attached to it. The war's cause is published. It …`
- Row 009 `unwritten`: `{"ending_key":"unwritten","prose":"The succession was never investigated, and the Muster never triggered. The bulletin signed Harven stays on the board at Checkpoint Gamma; the rally never forms; the ledger nobody signed stays unopened in …`
- Row 010 `ending_verdict_the_sector_recounts`: `{"ending_key":"ending_verdict_the_sector_recounts","prose":"The count is read aloud at the Grain Exchange weighbridge by a trader who does not look up from the scale. It takes three minutes. The last line is the count, presented, in the ma…`
- Row 011 `ending_verdict_the_count_is_held`: `{"ending_key":"ending_verdict_the_count_is_held","prose":"Nobody presents the count. The carrier tone continues on the dead band. The radio screen reads, forever after, CENSUS WINDOW: OPEN. The last image is the tone: one second on, one se…`
- Row 012 `ending_verdict_the_offer_is_a_lease`: `{"ending_key":"ending_verdict_the_offer_is_a_lease","prose":"The count converts into a lease: quarterly maintenance, a reading per season, a census every 1,827 days, enforceable by the machine's own registers. The last image: a quarterly i…`
- Row 013 `ending_garrison_absorbs_coalition`: `{"ending_key":"ending_garrison_absorbs_coalition","prose":"The shelter kept its perimeter wire and lost its old chain of command. Central Garrison stencils appeared above the intake manifold before anyone formally announced the demobilizat…`
- Row 014 `ending_rebuilders_joined`: `{"ending_key":"ending_rebuilders_joined","prose":"The coalition banner was folded into the archive, replaced by the yellow chalk markers of the reconstruction crews. Work gangs clear the culverts along Highway 9 on alternating shifts, mixi…`
- Row 015 `ending_coalition_independent`: `{"ending_key":"ending_coalition_independent","prose":"No regional colors hang over the gate, and no courier brings orders stamped by a distant commander. The shelter trades scrap bar stock and dried mash at its own fence, weighing each tra…`
- Row 016 `ending_foundry_annexation`: `{"ending_key":"ending_foundry_annexation","prose":"The cupola furnace burns day and night, fed by scrap hauled from the southern rail siding. Ingot tallies are stamped directly onto copper tags issued to each shift supervisor, dictating th…`
- Row 017 `ending_water_plant_held`: `{"ending_key":"ending_water_plant_held","prose":"Desalination Unit 4 hums against the bedrock, delivering twelve thousand liters of potable permeate into the valley main each dawn. The shelter guards the intake manifold with fixed machine …`
- Row 018 `ending_grain_silo_captured`: `{"ending_key":"ending_grain_silo_captured","prose":"The corrugated concrete silos hold thirty tons of sealed winter rye, protected from moisture and weevils by nitrogen purging. Every morning, two armed tallymen measure out bushel allocati…`
- Row 019 `ending_fuel_depot_burned`: `{"ending_key":"ending_fuel_depot_burned","prose":"The storage farm at Railhead Six is a field of buckled steel plates and scorched soil that smells of sour kerosene. Without diesel, trucks sit axle-deep in frozen mud along the perimeter, s…`
- Row 020 `ending_mercy_road`: `{"ending_key":"ending_mercy_road","prose":"Those who surrendered were not lined against the revetment; their names were written into the ration rolls with crossed-out unit numbers. Former enforcers work the pump handles alongside the famil…`
- Row 021 `ending_iron_way`: `{"ending_key":"ending_iron_way","prose":"No rival gang approaches within rifle shot of the shelter gate, and the approach cuts remain completely clear of scavengers. Curfew begins at sundown; doors are bolted from the outside by appointed …`
- Row 022 `ending_listeners_thread`: `{"ending_key":"ending_listeners_thread","prose":"The copper aerial array on the ridge picks up signals from nine valley settlements, trading weather soundings and transit notices on scheduled quarter-hours. Disagreements over boundary mark…`
- Row 023 `ending_mercy_water_held`: `{"ending_key":"ending_mercy_water_held","prose":"The desalination plant delivers a daily reserve to the public cistern outside the gate, where any traveler with a clean canteen may drink without paying in brass. Former combatants work the …`
- Row 024 `ending_iron_fuel_ash`: `{"ending_key":"ending_iron_fuel_ash","prose":"The enemies who contested the fuel reserve were wiped out to the last man, but the depot they defended burned down to the concrete footings. In the shelter below, survivors sleep in three layer…`
- Row 025 `ending_shelter_falls`: `{"ending_key":"ending_shelter_falls","prose":"The outer blast door was held ajar by a calcified boot wedged beneath the sill. Inside, water dripping from a burst condensation pipe had frozen into cloudy pillars across the dispensary floor.…`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs`

### `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs` — complete current file

- Size: 125 lines / 4211 bytes.
- SHA-256: `2f9ae2fa6d907c42c54ab2217631b7b9bd348c527be41fd6252334a500ab9358`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: namespace Ashfall.Core.Endgame
00007: {
00008:     /// <summary>
00009:     /// ASHFALL Epilogue Chronicle (item 16).
00010:     ///
00011:     /// Presentation-only DTOs that wrap an authoritative ending key from
00012:     /// <see cref="Ashfall.Core.Muster.MusterSystem.ResolveEndingKey"/>
00013:     /// into an ordered sequence of slides, survivor fate cards, metrics,
00014:     /// and prose. The Core produces the chronicle deterministically from
00015:     /// the same inputs every time so the same campaign state always
00016:     /// yields the same ending slides and metrics.
00017:     /// </summary>
00018:     public sealed class EpilogueChronicleBuilder
00019:     {
00020:         public EpilogueChronicle Build(EpilogueChronicleInput input)
00021:         {
00022:             if (input == null) throw new ArgumentNullException(nameof(input));
00023:             var chronicle = new EpilogueChronicle
00024:             {
00025:                 EndingKey = input.EndingKey ?? "unknown",
00026:                 GeneratedDay = input.Day,
00027:                 BuildSeed = input.BuildSeed,
00028:                 Title = TitleFor(input.EndingKey ?? "unknown"),
00029:                 Metrics = new List<EpilogueMetric>(input.Metrics ?? new List<EpilogueMetric>()),
00030:                 Slides = new List<EpilogueSlide>(input.Slides ?? new List<EpilogueSlide>()),
00031:                 FateCards = new List<SurvivorFateCard>(input.FateCards ?? new List<SurvivorFateCard>())
00032:             };
00033:             // Stable ordering: by slide index, then by survivor id.
00034:             chronicle.Slides.Sort((a, b) => a.Order.CompareTo(b.Order));
00035:             chronicle.FateCards.Sort((a, b) =>
00036:                 string.CompareOrdinal(a.SurvivorId, b.SurvivorId));
00037:             chronicle.Metrics.Sort((a, b) =>
00038:                 string.CompareOrdinal(a.MetricId, b.MetricId));
00039:             return chronicle;
00040:         }
00041:
00042:         private static string TitleFor(string endingKey)
00043:         {
00044:             if (string.IsNullOrEmpty(endingKey)) return "UNKNOWN ENDING";
00045:             return endingKey.ToUpperInvariant() switch
00046:             {
00047:                 "KNOWING" => "Knowing",
00048:                 "CULPABLE" => "Culpable",
00049:                 "REMEMBERING" => "Remembering",
00050:                 "FORGIVING" => "Forgiving",
00051:                 _ => endingKey
00052:             };
00053:         }
00054:     }
00055:
00056:     [Serializable]
00057:     public sealed class EpilogueChronicleInput
00058:     {
00059:         public string EndingKey;
00060:         public int Day;
00061:         public int BuildSeed;
00062:         public List<EpilogueSlide> Slides;
00063:         public List<SurvivorFateCard> FateCards;
00064:         public List<EpilogueMetric> Metrics;
00065:     }
00066:
00067:     [Serializable]
00068:     public sealed class EpilogueChronicle
00069:     {
00070:         public string EndingKey;
00071:         public string Title;
00072:         public int GeneratedDay;
00073:         public int BuildSeed;
00074:         public List<EpilogueSlide> Slides = new List<EpilogueSlide>();
00075:         public List<SurvivorFateCard> FateCards = new List<SurvivorFateCard>();
00076:         public List<EpilogueMetric> Metrics = new List<EpilogueMetric>();
00077:     }
00078:
00079:     [Serializable]
00080:     public sealed class EpilogueSlide
00081:     {
00082:         public int Order;
00083:         public string Title;
00084:         public string Prose;
00085:         public string ArtAssetId;
00086:
00087:         public EpilogueSlide() { }
00088:
00089:         public EpilogueSlide(int order, string title, string prose, string? artAssetId = null)
00090:         {
00091:             Order = order;
00092:             Title = title ?? string.Empty;
00093:             Prose = prose ?? string.Empty;
00094:             ArtAssetId = artAssetId ?? string.Empty;
00095:         }
00096:     }
00097:
00098:     [Serializable]
00099:     public sealed class SurvivorFateCard
00100:     {
00101:         public string SurvivorId;
00102:         public string DisplayName;
00103:         public string Fate;
00104:         public bool Survived;
00105:
00106:         public SurvivorFateCard() { }
00107:     }
00108:
00109:     [Serializable]
00110:     public sealed class EpilogueMetric
00111:     {
00112:         public string MetricId;
00113:         public float Value;
00114:         public string DisplayLabel;
00115:
00116:         public EpilogueMetric() { }
00117:
00118:         public EpilogueMetric(string metricId, float value, string displayLabel)
00119:         {
00120:             MetricId = metricId ?? string.Empty;
00121:             Value = value;
00122:             DisplayLabel = displayLabel ?? string.Empty;
00123:         }
00124:     }
00125: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Endgame/EpilogueChronicleCatalog.cs`

### `Assets/Ashfall.Core/Endgame/EpilogueChronicleCatalog.cs` — complete current file

- Size: 89 lines / 2875 bytes.
- SHA-256: `658a2091baffeb12e5be3a6d5f631a4bd3b79cf0194a1c3aa2da1db64953c126`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ASHFALL Epilogue Chronicle Catalog & Loader (Plan 96).
00003:
00004: using System;
00005: using System.Collections.Generic;
00006: using System.IO;
00007:
00008: namespace Ashfall.Core.Endgame
00009: {
00010:     /// <summary>
00011:     /// Schema definition for a single slide entry in epilogue_chronicle.json.
00012:     /// </summary>
00013:     [Serializable]
00014:     public sealed class EpilogueSlideDefinition
00015:     {
00016:         public int order { get; set; }
00017:         public string title { get; set; } = string.Empty;
00018:         public string art_asset_id { get; set; } = string.Empty;
00019:
00020:         public EpilogueSlideDefinition() { }
00021:
00022:         public EpilogueSlideDefinition(int order, string title, string artAssetId)
00023:         {
00024:             this.order = order;
00025:             this.title = title ?? string.Empty;
00026:             this.art_asset_id = artAssetId ?? string.Empty;
00027:         }
00028:
00029:         public EpilogueSlide ToSlide(string prose = "")
00030:         {
00031:             return new EpilogueSlide(order, title, prose, art_asset_id);
00032:         }
00033:     }
00034:
00035:     /// <summary>
00036:     /// Root envelope for epilogue_chronicle.json.
00037:     /// </summary>
00038:     [Serializable]
00039:     public sealed class EpilogueChronicleCatalogData
00040:     {
00041:         public int schema_version { get; set; } = 1;
00042:         public List<EpilogueSlideDefinition> default_slides { get; set; } = new List<EpilogueSlideDefinition>();
00043:     }
00044:
00045:     /// <summary>
00046:     /// Engine-agnostic loader for the epilogue chronicle catalog.
00047:     /// </summary>
00048:     public static class EpilogueChronicleLoader
00049:     {
00050:         public const string DefaultFileName = "epilogue_chronicle.json";
00051:
00052:         public static EpilogueChronicleCatalogData? Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
00053:         {
00054:             if (string.IsNullOrEmpty(dataDir) || fileIO == null || serializer == null)
00055:                 return null;
00056:
00057:             string path = Path.Combine(dataDir, DefaultFileName);
00058:             if (!fileIO.FileExists(path))
00059:                 return null;
00060:
00061:             try
00062:             {
00063:                 string raw = fileIO.ReadAllText(path);
00064:                 return serializer.Deserialize<EpilogueChronicleCatalogData>(raw);
00065:             }
00066:             catch (Exception ex)
00067:             {
00068:                 Ashfall.Core.IO.CatalogDiagnostics.Warn(path, "EpilogueChronicleCatalogData", ex);
00069:                 return null;
00070:             }
00071:         }
00072:
00073:         public static List<EpilogueSlide> LoadDefaultSlides(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
00074:         {
00075:             var catalog = Load(dataDir, fileIO, serializer);
00076:             var result = new List<EpilogueSlide>();
00077:             if (catalog?.default_slides == null) return result;
00078:
00079:             foreach (var def in catalog.default_slides)
00080:             {
00081:                 if (def != null)
00082:                 {
00083:                     result.Add(def.ToSlide());
00084:                 }
00085:             }
00086:             return result;
00087:         }
00088:     }
00089: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Endgame/EpilogueContextFactory.cs`

### `Assets/Ashfall.Core/Endgame/EpilogueContextFactory.cs` — complete current file

- Size: 60 lines / 2591 bytes.
- SHA-256: `a9844428837f68851804ab2726890aee3b813d33d193867734c2cb4e9ab02707`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004:
00005: namespace Ashfall.Core.Endgame
00006: {
00007:     /// <summary>
00008:     /// Immutable input record gathering campaign facts for epilogue evaluation (Plan 19 / INV-19.1).
00009:     /// Pure Core DTO with zero engine dependencies.
00010:     /// </summary>
00011:     public sealed record EpilogueContextInputs(
00012:         int Days,
00013:         int LivingDwellers,
00014:         int DeathsRecorded,
00015:         bool GrandTreatySigned,
00016:         bool TempestDecommissioned,
00017:         bool DebtLedgersBurned,
00018:         bool ChildrenSurvived,
00019:         bool VelSecretExposed,
00020:         IReadOnlyList<string>? SourceIds = null);
00021:
00022:     /// <summary>
00023:     /// Factory for creating EpilogueEvaluationContext and CampaignOutcomeSnapshot
00024:     /// from live campaign authorities (Plan 19 / FX-01).
00025:     /// </summary>
00026:     public static class EpilogueContextFactory
00027:     {
00028:         public static EpilogueEvaluationContext Build(EpilogueContextInputs inputs)
00029:         {
00030:             if (inputs == null) throw new ArgumentNullException(nameof(inputs));
00031:             if (inputs.Days < 0) throw new ArgumentOutOfRangeException(nameof(inputs), "Days must be non-negative.");
00032:             if (inputs.LivingDwellers < 0) throw new ArgumentOutOfRangeException(nameof(inputs), "LivingDwellers must be non-negative.");
00033:             if (inputs.DeathsRecorded < 0) throw new ArgumentOutOfRangeException(nameof(inputs), "DeathsRecorded must be non-negative.");
00034:
00035:             return new EpilogueEvaluationContext
00036:             {
00037:                 totalDaysSurvived = inputs.Days,
00038:                 livingDwellerCount = inputs.LivingDwellers,
00039:                 totalDeathsRecorded = inputs.DeathsRecorded,
00040:                 grandTreatySigned = inputs.GrandTreatySigned,
00041:                 tempestDecommissioned = inputs.TempestDecommissioned,
00042:                 debtLedgersBurned = inputs.DebtLedgersBurned,
00043:                 childrenSurvived = inputs.ChildrenSurvived,
00044:                 velSecretExposed = inputs.VelSecretExposed
00045:             };
00046:         }
00047:
00048:         public static CampaignOutcomeSnapshot CreateSnapshot(CampaignOutcomeEvaluationInput input)
00049:             => CampaignOutcomeEvaluator.Evaluate(input);
00050:
00051:         public static EpilogueEvaluationContext CreateContext(CampaignOutcomeEvaluationInput input)
00052:             => CampaignOutcomeEvaluator.Evaluate(input).ToContext();
00053:
00054:         public static EpilogueEvaluationContext CreateContext(CampaignOutcomeSnapshot snapshot)
00055:         {
00056:             if (snapshot == null) throw new ArgumentNullException(nameof(snapshot));
00057:             return snapshot.ToContext();
00058:         }
00059:     }
00060: }
```


# Appendix — Current Source Detail: `src/Host/UnifiedEndingHostSession.cs`

### `src/Host/UnifiedEndingHostSession.cs` — complete current file

- Size: 112 lines / 4595 bytes.
- SHA-256: `0b9eaa9f93e463b86673fd24e811b3fc2f9e885e2723222a33403f3e8481b2dd`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // Save Store : UnifiedEndingSaveStore
00004: // Core State : Ashfall.Core.Endgame.UnifiedEndingSaveState
00005: // Host Caller: Main.UnifiedEnding (SetupUnifiedEnding / SaveUnifiedEnding)
00006: // Purpose    : Plan 145 — Unified ending resolution & epilogue personalization:
00007: //              political, social, moral, personal, and expedition resolution.
00008: // ============================================================================
00009:
00010: using System;
00011: using System.IO;
00012: using Ashfall.Core;
00013: using Ashfall.Core.Endgame;
00014: using Ashfall.Core.Save;
00015:
00016: namespace AtomicWar.GodotApp
00017: {
00018:     public static class UnifiedEndingSaveStore
00019:     {
00020:         public const string FileName = "unified_ending_save.json";
00021:         public const string SectionName = "unified_ending";
00022:
00023:         private static readonly SaveStore<UnifiedEndingSaveState> s_store =
00024:             SaveStoreHub.Checksummed<UnifiedEndingSaveState>(FileName, nameof(UnifiedEndingSaveStore));
00025:
00026:         public static string SavePath => s_store.SavePath;
00027:         public static bool Exists => s_store.Exists();
00028:
00029:         public static string TryCapturePersisted(UnifiedEndingSaveState state) => s_store.CaptureBare(state);
00030:         public static UnifiedEndingSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
00031:         public static bool TrySave(UnifiedEndingSaveState state) => s_store.TrySave(state);
00032:         public static UnifiedEndingSaveState? TryLoad() => s_store.TryLoad();
00033:     }
00034:
00035:     /// <summary>
00036:     /// Host session manager for Plan 145 (Unified Ending Resolution & Epilogue Personalization).
00037:     /// Unifies Holdfast endings, Muster epilogues, and Epilogue Matrix into a single coherent
00038:     /// endgame resolution that evaluates full campaign state and generates a personalized chronicle.
00039:     /// </summary>
00040:     public sealed class UnifiedEndingHostSession : HostSessionBase
00041:     {
00042:         private readonly UnifiedEndingResolver _resolver;
00043:         private string _lastEvent = string.Empty;
00044:
00045:         public event Action<UnifiedEndingResult>? EndingResolved;
00046:
00047:         public UnifiedEndingResolver Resolver => _resolver;
00048:         public string LastEvent => _lastEvent;
00049:         public bool IsResolved => _resolver.IsResolved;
00050:         public UnifiedEndingResult? LastResult => _resolver.LastResult;
00051:         public UnifiedEndingCensus Census => _resolver.GetCensus();
00052:
00053:         public UnifiedEndingHostSession(string? dataDir = null, UnifiedEndingResolver? resolver = null)
00054:         {
00055:             _resolver = resolver ?? new UnifiedEndingResolver();
00056:
00057:             _resolver.OnUnifiedEndingResolvedSeam = result =>
00058:             {
00059:                 _lastEvent = $"Ending resolved: {result.overallTitle} ({result.resolutionId})";
00060:                 RaiseStateChanged();
00061:                 EndingResolved?.Invoke(result);
00062:             };
00063:
00064:             _resolver.OnSurvivorEpilogueGeneratedSeam = fate =>
00065:             {
00066:                 _lastEvent = $"Survivor epilogue generated: {fate.survivorName} ({fate.status})";
00067:                 RaiseStateChanged();
00068:             };
00069:
00070:             if (!string.IsNullOrEmpty(dataDir))
00071:             {
00072:                 LoadCatalog(dataDir);
00073:             }
00074:         }
00075:
00076:         public static UnifiedEndingHostSession Create(string dataDir, UnifiedEndingResolver? resolver = null)
00077:         {
00078:             return new UnifiedEndingHostSession(dataDir, resolver);
00079:         }
00080:
00081:         public void LoadCatalog(string dataDir)
00082:         {
00083:             if (string.IsNullOrEmpty(dataDir)) return;
00084:             string path = Path.Combine(dataDir, "epilogue_personalization.json");
00085:             if (File.Exists(path))
00086:             {
00087:                 string json = File.ReadAllText(path);
00088:                 _resolver.LoadCatalog(json);
00089:                 _lastEvent = "Loaded epilogue personalization catalog.";
00090:                 RaiseStateChanged();
00091:             }
00092:         }
00093:
00094:         public UnifiedEndingResult Resolve(UnifiedEndingContext context)
00095:         {
00096:             if (context == null) throw new ArgumentNullException(nameof(context));
00097:             var result = _resolver.ResolveEnding(context);
00098:             _lastEvent = $"Campaign resolved with ending: {result.overallTitle}";
00099:             RaiseStateChanged();
00100:             return result;
00101:         }
00102:
00103:         public UnifiedEndingSaveState CaptureState() => _resolver.CaptureState();
00104:
00105:         public void RestoreState(UnifiedEndingSaveState state)
00106:         {
00107:             _resolver.RestoreState(state);
00108:             _lastEvent = "Restored unified ending state.";
00109:             RaiseStateChanged();
00110:         }
00111:     }
00112: }
```


# Appendix — Current Source Detail: `src/UI/EpiloguePanel.cs`

### `src/UI/EpiloguePanel.cs` — complete current file

- Size: 251 lines / 10695 bytes.
- SHA-256: `40e17093add485b4ca0e323093811deeabc7afdee6e5751975926c2ead73c9ba`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Godot;
00004: using Ashfall.Core.Endgame;
00005: using Ashfall.Core.UI;
00006: using CoreTheme = Ashfall.Core.UI.Theme;
00007:
00008: namespace AtomicWar.GodotApp.UI
00009: {
00010:     /// <summary>
00011:     /// ASHFALL — Endgame Epilogue Panel.
00012:     /// Evaluates whole-saga world state across 32 matrix permutations, generating the
00013:     /// authoritative literary-grade chronicle of the wasteland.
00014:     ///
00015:     /// Presentation only — evaluates EpilogueMatrixRuntime against simulation state.
00016:     /// </summary>
00017:     public partial class EpiloguePanel : Control
00018:     {
00019:         public event Action? OnClose;
00020:
00021:         private readonly EpilogueMatrixRuntime _runtime = new EpilogueMatrixRuntime();
00022:         private EpilogueEvaluationContext _context = new EpilogueEvaluationContext();
00023:         private CampaignOutcomeSnapshot? _snapshot;
00024:         private UnifiedEndingResult? _unifiedResult;
00025:         private VBoxContainer _outcomesContainer = null!;
00026:         private VBoxContainer _traceContainer = null!;
00027:         private Label _narrativeLabel = null!;
00028:         private Label _statusLabel = null!;
00029:
00030:         public override void _Ready()
00031:         {
00032:             SetAnchorsPreset(LayoutPreset.FullRect);
00033:             BuildLayout();
00034:             Visible = false;
00035:         }
00036:
00037:         public override void _UnhandledInput(InputEvent @event)
00038:         {
00039:             if (!Visible) return;
00040:             if (@event is InputEventKey key && key.Pressed && !key.Echo && key.Keycode == Key.Escape)
00041:             {
00042:                 Close();
00043:                 GetViewport().SetInputAsHandled();
00044:             }
00045:         }
00046:
00047:         /// <summary>Authoritative derived binding from live campaign snapshot (FX-01).</summary>
00048:         public void Bind(CampaignOutcomeSnapshot snapshot)
00049:         {
00050:             if (snapshot == null) return;
00051:             _snapshot = snapshot;
00052:             _context = snapshot.ToContext();
00053:             RefreshView();
00054:         }
00055:
00056:         /// <summary>Unified ending resolution binding with personalized prose (Plan 145).</summary>
00057:         public void Bind(UnifiedEndingResult result)
00058:         {
00059:             if (result == null) return;
00060:             _unifiedResult = result;
00061:             RefreshView();
00062:         }
00063:
00064:         /// <summary>Context binding for standalone tests and direct context evaluation.</summary>
00065:         public void Bind(EpilogueEvaluationContext context)
00066:         {
00067:             _context = context ?? new EpilogueEvaluationContext();
00068:             _snapshot = null;
00069:             RefreshView();
00070:         }
00071:
00072:         public void Open()
00073:         {
00074:             Visible = true;
00075:             RefreshView();
00076:         }
00077:
00078:         public void Close() {
00079:             if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
00080:                 Visible = false;
00081:             OnClose?.Invoke();
00082:         }
00083:
00084:         private void BuildLayout()
00085:         {
00086:             var backdrop = new ColorRect
00087:             {
00088:                 Color = new Color(0.03f, 0.04f, 0.05f, 0.95f)
00089:             };
00090:             backdrop.SetAnchorsPreset(LayoutPreset.FullRect);
00091:             AddChild(backdrop);
00092:
00093:             var margin = new MarginContainer();
00094:             margin.SetAnchorsPreset(LayoutPreset.FullRect);
00095:             margin.AddThemeConstantOverride("margin_left", (int)CoreTheme.SpacingLg);
00096:             margin.AddThemeConstantOverride("margin_right", (int)CoreTheme.SpacingLg);
00097:             margin.AddThemeConstantOverride("margin_top", (int)CoreTheme.SpacingLg);
00098:             margin.AddThemeConstantOverride("margin_bottom", (int)CoreTheme.SpacingLg);
00099:             AddChild(margin);
00100:
00101:             var mainVBox = new VBoxContainer();
00102:             mainVBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
00103:             margin.AddChild(mainVBox);
00104:
00105:             // ── Header Card ──
00106:             var headerCard = AshfallUiHelpers.MakeCardFrame(
00107:                 "THE CHRONICLE OF TESSARAT // ENDGAME EPILOGUE MATRIX",
00108:                 "Thirty-two permutation whole-saga evaluation of regional fate, demographic outcomes, moral standing, and wasteland survival legacy."
00109:             );
00110:             mainVBox.AddChild(headerCard);
00111:
00112:             // ── Scrollable Body ──
00113:             var scroll = new ScrollContainer
00114:             {
00115:                 SizeFlagsHorizontal = SizeFlags.ExpandFill,
00116:                 SizeFlagsVertical = SizeFlags.ExpandFill,
00117:                 HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
00118:             };
00119:             mainVBox.AddChild(scroll);
00120:
00121:             var contentBox = new VBoxContainer();
00122:             contentBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
00123:             contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00124:             scroll.AddChild(contentBox);
00125:
00126:             _outcomesContainer = new VBoxContainer();
00127:             _outcomesContainer.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
00128:             contentBox.AddChild(_outcomesContainer);
00129:
00130:             contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("LITERARY CHRONICLE & REGIONAL OUTCOME"));
00131:
00132:             var narrCard = AshfallUiHelpers.MakePanel();
00133:             var narrMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
00134:             narrCard.AddChild(narrMargin);
00135:
00136:             var nBox = new VBoxContainer();
00137:             nBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
00138:             narrMargin.AddChild(nBox);
00139:
00140:             _narrativeLabel = AshfallUiHelpers.MakeBody("Evaluating wasteland chronicle...");
00141:             nBox.AddChild(_narrativeLabel);
00142:
00143:             contentBox.AddChild(narrCard);
00144:
00145:             contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("AUTHORITATIVE CAMPAIGN PROVENANCE & EVALUATION TRACE"));
00146:
00147:             var traceCard = AshfallUiHelpers.MakePanel();
00148:             var traceMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
00149:             traceCard.AddChild(traceMargin);
00150:
00151:             _traceContainer = new VBoxContainer();
00152:             _traceContainer.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingXs);
00153:             traceMargin.AddChild(_traceContainer);
00154:
00155:             contentBox.AddChild(traceCard);
00156:
00157:             // ── Bottom Action Bar ──
00158:             var bottomBar = new HBoxContainer();
00159:             bottomBar.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
00160:             mainVBox.AddChild(bottomBar);
00161:
00162:             var btnClose = AshfallUiHelpers.MakeButton("RETURN TO EXPANSION HUB [ESC]", Close);
00163:             btnClose.CustomMinimumSize = new Vector2(240, 44);
00164:             bottomBar.AddChild(btnClose);
00165:
00166:             _statusLabel = AshfallUiHelpers.MakeMono("Chronicle matrix evaluated against active world ledger flags.");
00167:             _statusLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00168:             bottomBar.AddChild(_statusLabel);
00169:         }
00170:
00171:         public void RefreshView()
00172:         {
00173:             if (_outcomesContainer == null || _narrativeLabel == null || _traceContainer == null) return;
00174:
00175:             ClearContainer(_outcomesContainer);
00176:             ClearContainer(_traceContainer);
00177:
00178:             // Prefer the bound CampaignOutcomeSnapshot classifications/prose so
00179:             // Bind(snapshot) cannot drift from a second matrix re-evaluation.
00180:             var fate = _snapshot?.Fate ?? _runtime.EvaluateRegionalFate(_context);
00181:             var demographics = _snapshot?.Demographics ?? _runtime.EvaluateDemographics(_context);
00182:             var moral = _snapshot?.MoralStanding ?? _runtime.EvaluateMoralStanding(_context);
00183:             string narrative;
00184:             if (_unifiedResult != null && !string.IsNullOrEmpty(_unifiedResult.fullPersonalizedChronicle))
00185:             {
00186:                 narrative = _unifiedResult.fullPersonalizedChronicle;
00187:             }
00188:             else if (!string.IsNullOrEmpty(_snapshot?.NarrativeProse))
00189:             {
00190:                 narrative = _snapshot!.NarrativeProse;
00191:             }
00192:             else
00193:             {
00194:                 narrative = _runtime.GenerateEpilogueNarrative(_context);
00195:             }
00196:
00197:             var outCard = AshfallUiHelpers.MakePanel();
00198:             var outMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
00199:             outCard.AddChild(outMargin);
00200:
00201:             var oBox = new VBoxContainer();
00202:             oBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingXs);
00203:             outMargin.AddChild(oBox);
00204:
00205:             oBox.AddChild(AshfallUiHelpers.MakeSectionHeader("EVALUATED HISTORICAL OUTCOMES"));
00206:             oBox.AddChild(AshfallUiHelpers.MakeDataRow("Regional Fate", FormatEnum(fate), AshfallUiHelpers.ToColor(CoreTheme.Hot)));
00207:             oBox.AddChild(AshfallUiHelpers.MakeDataRow("Demographic Legacy", FormatEnum(demographics), AshfallUiHelpers.ToColor(CoreTheme.Warm)));
00208:             oBox.AddChild(AshfallUiHelpers.MakeDataRow("Moral Standing", FormatEnum(moral), AshfallUiHelpers.ToColor(CoreTheme.Pale)));
00209:             oBox.AddChild(AshfallUiHelpers.MakeDataRow("Days Survived", $"{_context.totalDaysSurvived} Days", AshfallUiHelpers.ToColor(CoreTheme.Pale)));
00210:             oBox.AddChild(AshfallUiHelpers.MakeDataRow("Living Dwellers", $"{_context.livingDwellerCount} Active", AshfallUiHelpers.ToColor(CoreTheme.Pale)));
00211:             oBox.AddChild(AshfallUiHelpers.MakeDataRow("Inscribed Deaths", $"{_context.totalDeathsRecorded} Losses", AshfallUiHelpers.ToColor(CoreTheme.Critical)));
00212:
00213:             _outcomesContainer.AddChild(outCard);
00214:
00215:             _narrativeLabel.Text = narrative;
00216:
00217:             if (_snapshot != null && _snapshot.OutcomeTrace.Count > 0)
00218:             {
00219:                 foreach (var line in _snapshot.OutcomeTrace)
00220:                 {
00221:                     var lbl = AshfallUiHelpers.MakeMono(line);
00222:                     lbl.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00223:                     _traceContainer.AddChild(lbl);
00224:                 }
00225:                 _statusLabel.Text = $"Chronicle drawn from the campaign record ({_snapshot.OutcomeTrace.Count} facts).";
00226:             }
00227:             else
00228:             {
00229:                 _traceContainer.AddChild(AshfallUiHelpers.MakeMono("No extra traces on file."));
00230:                 _statusLabel.Text = "Chronicle checked against the current record.";
00231:             }
00232:         }
00233:
00234:         private static string FormatEnum<T>(T val) where T : struct
00235:         {
00236:             string s = val.ToString() ?? string.Empty;
00237:             var sb = new System.Text.StringBuilder();
00238:             for (int i = 0; i < s.Length; i++)
00239:             {
00240:                 if (i > 0 && char.IsUpper(s[i])) sb.Append(' ');
00241:                 sb.Append(s[i]);
00242:             }
00243:             return sb.ToString();
00244:         }
00245:
00246:         private static void ClearContainer(VBoxContainer container)
00247:         {
00248:             AshfallUiHelpers.EmptyChildren(container);
00249:         }
00250:     }
00251: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Endgame/EpilogueChronicleBuilderTests.cs`

### `Ashfall.Core.Tests/Endgame/EpilogueChronicleBuilderTests.cs` — complete current file

- Size: 147 lines / 5452 bytes.
- SHA-256: `6f99bb4cc40c06f31bcbe94f4a3765fbf0fc184babdf92ba7beb2e4a3b48d9e9`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.Endgame;
00005: using Xunit;
00006:
00007: namespace Ashfall.Core.Tests.Endgame
00008: {
00009:     public class EpilogueChronicleBuilderTests
00010:     {
00011:         [Fact]
00012:         public void Build_ProducesTitleForKnownEnding()
00013:         {
00014:             var b = new EpilogueChronicleBuilder();
00015:             var c = b.Build(new EpilogueChronicleInput
00016:             {
00017:                 EndingKey = "knowing",
00018:                 Day = 365,
00019:                 BuildSeed = 7,
00020:                 Slides = new List<EpilogueSlide>(),
00021:                 FateCards = new List<SurvivorFateCard>(),
00022:                 Metrics = new List<EpilogueMetric>()
00023:             });
00024:             Assert.Equal("Knowing", c.Title);
00025:             Assert.Equal("knowing", c.EndingKey);
00026:         }
00027:
00028:         [Fact]
00029:         public void Build_SortsSlidesByOrder()
00030:         {
00031:             var b = new EpilogueChronicleBuilder();
00032:             var c = b.Build(new EpilogueChronicleInput
00033:             {
00034:                 EndingKey = "knowing",
00035:                 Day = 100,
00036:                 BuildSeed = 1,
00037:                 Slides = new List<EpilogueSlide>
00038:                 {
00039:                     new EpilogueSlide(2, "Second", "..."),
00040:                     new EpilogueSlide(0, "First", "..."),
00041:                     new EpilogueSlide(1, "Middle", "...")
00042:                 },
00043:                 FateCards = new List<SurvivorFateCard>(),
00044:                 Metrics = new List<EpilogueMetric>()
00045:             });
00046:             Assert.Equal(3, c.Slides.Count);
00047:             Assert.Equal("First", c.Slides[0].Title);
00048:             Assert.Equal("Middle", c.Slides[1].Title);
00049:             Assert.Equal("Second", c.Slides[2].Title);
00050:         }
00051:
00052:         [Fact]
00053:         public void Build_SortsFateCardsBySurvivorId()
00054:         {
00055:             var b = new EpilogueChronicleBuilder();
00056:             var c = b.Build(new EpilogueChronicleInput
00057:             {
00058:                 EndingKey = "knowing",
00059:                 Day = 100,
00060:                 BuildSeed = 1,
00061:                 Slides = new List<EpilogueSlide>(),
00062:                 FateCards = new List<SurvivorFateCard>
00063:                 {
00064:                     new SurvivorFateCard { SurvivorId = "zulu", DisplayName = "Zulu", Fate = "Survived", Survived = true },
00065:                     new SurvivorFateCard { SurvivorId = "alpha", DisplayName = "Alpha", Fate = "Died", Survived = false }
00066:                 },
00067:                 Metrics = new List<EpilogueMetric>()
00068:             });
00069:             Assert.Equal("alpha", c.FateCards[0].SurvivorId);
00070:             Assert.Equal("zulu", c.FateCards[1].SurvivorId);
00071:         }
00072:
00073:         [Fact]
00074:         public void Build_SortsMetricsByMetricId()
00075:         {
00076:             var b = new EpilogueChronicleBuilder();
00077:             var c = b.Build(new EpilogueChronicleInput
00078:             {
00079:                 EndingKey = "knowing",
00080:                 Day = 100,
00081:                 BuildSeed = 1,
00082:                 Slides = new List<EpilogueSlide>(),
00083:                 FateCards = new List<SurvivorFateCard>(),
00084:                 Metrics = new List<EpilogueMetric>
00085:                 {
00086:                     new EpilogueMetric("total_deaths", 5, "Deaths"),
00087:                     new EpilogueMetric("days_survived", 365, "Days"),
00088:                     new EpilogueMetric("morale_final", 75, "Morale")
00089:                 }
00090:             });
00091:             Assert.Equal("days_survived", c.Metrics[0].MetricId);
00092:             Assert.Equal("morale_final", c.Metrics[1].MetricId);
00093:             Assert.Equal("total_deaths", c.Metrics[2].MetricId);
00094:         }
00095:
00096:         [Fact]
00097:         public void Build_DeterministicForSameInput()
00098:         {
00099:             var b = new EpilogueChronicleBuilder();
00100:             var input = new EpilogueChronicleInput
00101:             {
00102:                 EndingKey = "culpable",
00103:                 Day = 211,
00104:                 BuildSeed = 99,
00105:                 Slides = new List<EpilogueSlide>
00106:                 {
00107:                     new EpilogueSlide(1, "Slide A", "Prose A"),
00108:                     new EpilogueSlide(0, "Slide B", "Prose B")
00109:                 },
00110:                 FateCards = new List<SurvivorFateCard>
00111:                 {
00112:                     new SurvivorFateCard { SurvivorId = "s2", DisplayName = "S2" },
00113:                     new SurvivorFateCard { SurvivorId = "s1", DisplayName = "S1" }
00114:                 },
00115:                 Metrics = new List<EpilogueMetric>
00116:                 {
00117:                     new EpilogueMetric("m1", 1f, "M1")
00118:                 }
00119:             };
00120:             var c1 = b.Build(input);
00121:             var c2 = b.Build(input);
00122:             Assert.Equal(c1.Title, c2.Title);
00123:             Assert.Equal(c1.Slides[0].Title, c2.Slides[0].Title);
00124:             Assert.Equal(c1.FateCards[0].SurvivorId, c2.FateCards[0].SurvivorId);
00125:             Assert.Equal(c1.Metrics[0].MetricId, c2.Metrics[0].MetricId);
00126:         }
00127:
00128:         [Fact]
00129:         public void Build_HandlesEmptyInput()
00130:         {
00131:             var b = new EpilogueChronicleBuilder();
00132:             var c = b.Build(new EpilogueChronicleInput { EndingKey = "" });
00133:             Assert.Equal("UNKNOWN ENDING", c.Title);
00134:             Assert.Empty(c.Slides);
00135:             Assert.Empty(c.FateCards);
00136:             Assert.Empty(c.Metrics);
00137:         }
00138:
00139:         [Fact]
00140:         public void Build_UnknownEndingFallsBackToKey()
00141:         {
00142:             var b = new EpilogueChronicleBuilder();
00143:             var c = b.Build(new EpilogueChronicleInput { EndingKey = "novel_ending" });
00144:             Assert.Equal("novel_ending", c.Title);
00145:         }
00146:     }
00147: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Campaign/CampaignEpilogueEngine.cs`

### `Assets/Ashfall.Core/Campaign/CampaignEpilogueEngine.cs` — complete current file

- Size: 220 lines / 8621 bytes.
- SHA-256: `c2d142ac86ed87cebfcc9c2c1d8bf85504859f923f3c124315df6862f92851f8`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Text;
00005: using System.Text.Json;
00006:
00007: namespace Ashfall.Core.Campaign
00008: {
00009:     [Serializable]
00010:     public sealed class CampaignEpilogueSnapshot
00011:     {
00012:         public int FinalDay { get; set; }
00013:         public int SurvivorsAlive { get; set; }
00014:         public int TotalCasualties { get; set; }
00015:         public int StarvationDeaths { get; set; }
00016:         public int DiseaseDeaths { get; set; }
00017:         public int ArchivesDecrypted { get; set; }
00018:         public int TechNodesCompleted { get; set; }
00019:         public int CaptivesParoled { get; set; }
00020:         public int CaptivesInterrogated { get; set; }
00021:         public int PenalLaborShiftsRun { get; set; }
00022:         public float AverageFreshnessConsumed { get; set; }
00023:         public Dictionary<string, int> FactionStandings { get; set; } = new Dictionary<string, int>();
00024:         public List<string> HistoricDecisions { get; set; } = new List<string>();
00025:         public ulong CampaignSeed { get; set; } = 42;
00026:     }
00027:
00028:     [Serializable]
00029:     public sealed class EpilogueChapter
00030:     {
00031:         public string Category { get; set; } = string.Empty;
00032:         public string Title { get; set; } = string.Empty;
00033:         public string NarrativeText { get; set; } = string.Empty;
00034:         public List<string> Highlights { get; set; } = new List<string>();
00035:     }
00036:
00037:     [Serializable]
00038:     public sealed class EpilogueChronicle
00039:     {
00040:         public string CampaignId { get; set; } = "ASHFALL_CAMPAIGN";
00041:         public int TotalDays { get; set; }
00042:         public List<EpilogueChapter> Chapters { get; set; } = new List<EpilogueChapter>();
00043:         public CampaignEpilogueSnapshot FinalMetrics { get; set; } = new CampaignEpilogueSnapshot();
00044:
00045:         public string ToJson()
00046:         {
00047:             return JsonSerializer.Serialize(this, new JsonSerializerOptions
00048:             {
00049:                 WriteIndented = true
00050:             });
00051:         }
00052:
00053:         public string ToFormattedReport()
00054:         {
00055:             var sb = new StringBuilder();
00056:             sb.AppendLine("=================================================");
00057:             sb.AppendLine($"         CHRONICLE OF THE VAULT — DAY {TotalDays}");
00058:             sb.AppendLine("=================================================");
00059:             sb.AppendLine();
00060:
00061:             foreach (var chap in Chapters)
00062:             {
00063:                 sb.AppendLine($"--- {chap.Title} ({chap.Category}) ---");
00064:                 sb.AppendLine(chap.NarrativeText);
00065:                 sb.AppendLine();
00066:                 if (chap.Highlights.Count > 0)
00067:                 {
00068:                     sb.AppendLine("Key Historical Markers:");
00069:                     foreach (var h in chap.Highlights)
00070:                     {
00071:                         sb.AppendLine($"  • {h}");
00072:                     }
00073:                     sb.AppendLine();
00074:                 }
00075:             }
00076:
00077:             sb.AppendLine("=================================================");
00078:             sb.AppendLine($"Survivors Living: {FinalMetrics.SurvivorsAlive} | Starvation: {FinalMetrics.StarvationDeaths} | Disease: {FinalMetrics.DiseaseDeaths}");
00079:             sb.AppendLine($"Archives Decrypted: {FinalMetrics.ArchivesDecrypted} | Captives Paroled: {FinalMetrics.CaptivesParoled}");
00080:             sb.AppendLine("=================================================");
00081:             return sb.ToString();
00082:         }
00083:     }
00084:
00085:     public sealed class CampaignEpilogueEngine
00086:     {
00087:         private readonly CampaignEpilogueCatalog _catalog;
00088:
00089:         public CampaignEpilogueEngine(CampaignEpilogueCatalog catalog)
00090:         {
00091:             _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
00092:         }
00093:
00094:         public EpilogueChronicle GenerateChronicle(CampaignEpilogueSnapshot snapshot)
00095:         {
00096:             if (snapshot == null) throw new ArgumentNullException(nameof(snapshot));
00097:
00098:             var chronicle = new EpilogueChronicle
00099:             {
00100:                 TotalDays = snapshot.FinalDay,
00101:                 FinalMetrics = snapshot
00102:             };
00103:
00104:             var rng = new SeededRng((int)(snapshot.CampaignSeed & 0x7FFFFFFF));
00105:
00106:             // 1. Demographics & Survival
00107:             var demoVignette = SelectVignette("demographics", v =>
00108:                 snapshot.SurvivorsAlive >= v.min_survivors &&
00109:                 snapshot.SurvivorsAlive <= v.max_survivors &&
00110:                 snapshot.StarvationDeaths <= v.max_starvation_deaths, rng);
00111:
00112:             if (demoVignette != null)
00113:             {
00114:                 chronicle.Chapters.Add(new EpilogueChapter
00115:                 {
00116:                     Category = "Demographics",
00117:                     Title = demoVignette.title,
00118:                     NarrativeText = demoVignette.narrative,
00119:                     Highlights = new List<string>
00120:                     {
00121:                         $"Final population: {snapshot.SurvivorsAlive} survivors",
00122:                         $"Total casualties across campaign: {snapshot.TotalCasualties}",
00123:                         $"Starvation fatalities: {snapshot.StarvationDeaths}"
00124:                     }
00125:                 });
00126:             }
00127:
00128:             // 2. Governance & Justice
00129:             var govVignette = SelectVignette("governance", v =>
00130:                 snapshot.CaptivesParoled >= v.min_paroled_captives &&
00131:                 snapshot.PenalLaborShiftsRun <= v.max_penal_shifts, rng);
00132:
00133:             if (govVignette != null)
00134:             {
00135:                 chronicle.Chapters.Add(new EpilogueChapter
00136:                 {
00137:                     Category = "Governance",
00138:                     Title = govVignette.title,
00139:                     NarrativeText = govVignette.narrative,
00140:                     Highlights = new List<string>
00141:                     {
00142:                         $"Hostiles paroled and integrated: {snapshot.CaptivesParoled}",
00143:                         $"Penal labor shifts enforced: {snapshot.PenalLaborShiftsRun}",
00144:                         $"Total interrogations logged: {snapshot.CaptivesInterrogated}"
00145:                     }
00146:                 });
00147:             }
00148:
00149:             // 3. Technology & Archives
00150:             var techVignette = SelectVignette("technology", v =>
00151:                 snapshot.ArchivesDecrypted >= v.min_archives_decrypted, rng);
00152:
00153:             if (techVignette != null)
00154:             {
00155:                 chronicle.Chapters.Add(new EpilogueChapter
00156:                 {
00157:                     Category = "Technology",
00158:                     Title = techVignette.title,
00159:                     NarrativeText = techVignette.narrative,
00160:                     Highlights = new List<string>
00161:                     {
00162:                         $"Pre-war archives deciphered: {snapshot.ArchivesDecrypted}",
00163:                         $"Knowledge research milestones: {snapshot.TechNodesCompleted}"
00164:                     }
00165:                 });
00166:             }
00167:
00168:             // 4. Sustenance & Preservation
00169:             var foodVignette = SelectVignette("sustenance", v =>
00170:                 snapshot.StarvationDeaths >= v.min_starvation_deaths &&
00171:                 snapshot.StarvationDeaths <= v.max_starvation_deaths, rng);
00172:
00173:             if (foodVignette != null)
00174:             {
00175:                 chronicle.Chapters.Add(new EpilogueChapter
00176:                 {
00177:                     Category = "Sustenance",
00178:                     Title = foodVignette.title,
00179:                     NarrativeText = foodVignette.narrative,
00180:                     Highlights = new List<string>
00181:                     {
00182:                         $"Starvation fatalities: {snapshot.StarvationDeaths}",
00183:                         $"Disease fatalities: {snapshot.DiseaseDeaths}"
00184:                     }
00185:                 });
00186:             }
00187:
00188:             return chronicle;
00189:         }
00190:
00191:         private EpilogueVignetteDef? SelectVignette(
00192:             string category,
00193:             Func<EpilogueVignetteDef, bool> predicate,
00194:             ISeededRng rng)
00195:         {
00196:             var matches = new List<EpilogueVignetteDef>();
00197:             foreach (var v in _catalog.GetAllVignettes())
00198:             {
00199:                 if (string.Equals(v.category, category, StringComparison.OrdinalIgnoreCase) && predicate(v))
00200:                 {
00201:                     matches.Add(v);
00202:                 }
00203:             }
00204:
00205:             if (matches.Count == 0) return null;
00206:
00207:             // Sort by priority descending
00208:             matches.Sort((a, b) => b.priority.CompareTo(a.priority));
00209:
00210:             int highestPriority = matches[0].priority;
00211:             var topCandidates = matches.FindAll(m => m.priority == highestPriority);
00212:
00213:             if (topCandidates.Count == 1)
00214:                 return topCandidates[0];
00215:
00216:             int pick = rng.Next(0, topCandidates.Count);
00217:             return topCandidates[pick];
00218:         }
00219:     }
00220: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Muster/EpilogueMatrix.cs`

### `Assets/Ashfall.Core/Muster/EpilogueMatrix.cs` — complete current file

- Size: 218 lines / 8297 bytes.
- SHA-256: `52d5e9b662f42660a64b545333f38a51bc06e7af6731edce1dd7411400081884`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003: #pragma warning disable CS0649
00004: #pragma warning disable CS8618
00005:
00006: namespace Ashfall.Core.Muster
00007: {
00008:     /// <summary>One Section XII epilogue-matrix outcome (muster_epilogues.json).</summary>
00009:     public class EndingDefinition
00010:     {
00011:         public string endingKey = string.Empty;
00012:         public string title = string.Empty;
00013:         public string prose = string.Empty;
00014:     }
00015:
00016:     /// <summary>
00017:     /// Engine-agnostic loader for muster_epilogues.json — the eight Day-360
00018:     /// outcomes (Section XII). MusterSystem resolves ending keys at approach
00019:     /// time; this catalog supplies the prose those keys name.
00020:     /// </summary>
00021:     public static class EpilogueMatrixLoader
00022:     {
00023:         public const string FileName = "muster_epilogues.json";
00024:         public const int CurrentSchemaVersion = 1;
00025:
00026:         public static List<EndingDefinition> LoadEpilogues(
00027:             string dataDir, IFileIO fileIO, IJsonSerializer json)
00028:         {
00029:             var result = new List<EndingDefinition>();
00030:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00031:                 return result;
00032:
00033:             string path = fileIO.Combine(dataDir, FileName);
00034:             if (!fileIO.FileExists(path))
00035:                 return result;
00036:
00037:             string raw = fileIO.ReadAllText(path);
00038:             if (string.IsNullOrWhiteSpace(raw))
00039:                 return result;
00040:
00041:             try
00042:             {
00043:                 var root = json.Deserialize<EpilogueCatalogRoot>(raw);
00044:                 if (root == null) return result;
00045:                 if (root.schema_version > CurrentSchemaVersion)
00046:                     return result;
00047:                 var entries = root.epilogues;
00048:                 if (entries == null) return result;
00049:                 for (int i = 0; i < entries.Count; i++)
00050:                 {
00051:                     var e = entries[i];
00052:                     if (e == null || string.IsNullOrEmpty(e.ending_key)) continue;
00053:                     result.Add(new EndingDefinition
00054:                     {
00055:                         endingKey = e.ending_key,
00056:                         title = e.title ?? string.Empty,
00057:                         prose = e.prose ?? string.Empty
00058:                     });
00059:                 }
00060:             }
00061:             catch (System.Exception ex_CATDIAG)
00062:             {
00063:                 Ashfall.Core.IO.CatalogDiagnostics.Warn(path, "EpilogueCatalogRoot", ex_CATDIAG);
00064:                 return result;
00065:             }
00066:             return result;
00067:         }
00068:
00069:         /// <summary>Schema-envelope root for muster_epilogues.json.</summary>
00070:         private class EpilogueCatalogRoot
00071:         {
00072:             public int schema_version = 1;
00073:             public List<EndingEntry> epilogues = new List<EndingEntry>();
00074:         }
00075:
00076:         private class EndingEntry
00077:         {
00078:             public string ending_key;
00079:             public string title;
00080:             public string prose;
00081:         }
00082:     }
00083:
00084:     public enum FactionTerminalOutcome
00085:     {
00086:         None = 0,
00087:         GarrisonAbsorbed,
00088:         RebuildersJoined,
00089:         Independent,
00090:         FoundryAnnexed
00091:     }
00092:
00093:     public sealed class EpilogueMatrixInput
00094:     {
00095:         public bool ShelterFallen { get; set; }
00096:         public bool WaterPlantHeld { get; set; }
00097:         public bool GrainSiloCaptured { get; set; }
00098:         public bool FuelDepotBurned { get; set; }
00099:         public bool MercyPattern { get; set; }
00100:         public bool IronPattern { get; set; }
00101:         public bool DiplomacyPattern { get; set; }
00102:         public string VerdictEndingKey { get; set; } = string.Empty;
00103:         public string MusterEndingKey { get; set; } = string.Empty;
00104:         public FactionTerminalOutcome FactionOutcome { get; set; } = FactionTerminalOutcome.None;
00105:     }
00106:
00107:     public static class EpilogueMatrix
00108:     {
00109:         public const string TheOpenMuster = "the_open_muster";
00110:         public const string TheAmnesty = "the_amnesty";
00111:         public const string TheCorridor = "the_corridor";
00112:         public const string TheBloodPrice = "the_blood_price";
00113:         public const string TheRateCardRevised = "the_rate_card_revised";
00114:         public const string TheAdministrator = "the_administrator";
00115:         public const string TheMeasuredTruthContested = "the_measured_truth_contested";
00116:         public const string TheMeasuredTruth = "the_measured_truth";
00117:         public const string Unwritten = "unwritten";
00118:         public const string VerdictSectorRecounts = "ending_verdict_the_sector_recounts";
00119:         public const string VerdictCountHeld = "ending_verdict_the_count_is_held";
00120:         public const string VerdictOfferLease = "ending_verdict_the_offer_is_a_lease";
00121:
00122:         // Faction (4)
00123:         public const string GarrisonAbsorbsCoalition = "ending_garrison_absorbs_coalition";
00124:         public const string RebuildersJoined = "ending_rebuilders_joined";
00125:         public const string CoalitionIndependent = "ending_coalition_independent";
00126:         public const string FoundryAnnexation = "ending_foundry_annexation";
00127:
00128:         // Resource (3)
00129:         public const string WaterPlantHeld = "ending_water_plant_held";
00130:         public const string GrainSiloCaptured = "ending_grain_silo_captured";
00131:         public const string FuelDepotBurned = "ending_fuel_depot_burned";
00132:
00133:         // Moral (3)
00134:         public const string MercyRoad = "ending_mercy_road";
00135:         public const string IronWay = "ending_iron_way";
00136:         public const string ListenersThread = "ending_listeners_thread";
00137:
00138:         // Compound (2)
00139:         public const string MercyWaterHeld = "ending_mercy_water_held";
00140:         public const string IronFuelAsh = "ending_iron_fuel_ash";
00141:
00142:         // Failure (1)
00143:         public const string ShelterFalls = "ending_shelter_falls";
00144:
00145:         public static readonly string[] AllKeys =
00146:         {
00147:             TheOpenMuster,
00148:             TheAmnesty,
00149:             TheCorridor,
00150:             TheBloodPrice,
00151:             TheRateCardRevised,
00152:             TheAdministrator,
00153:             TheMeasuredTruthContested,
00154:             TheMeasuredTruth,
00155:             Unwritten,
00156:             VerdictSectorRecounts,
00157:             VerdictCountHeld,
00158:             VerdictOfferLease,
00159:             GarrisonAbsorbsCoalition,
00160:             RebuildersJoined,
00161:             CoalitionIndependent,
00162:             FoundryAnnexation,
00163:             WaterPlantHeld,
00164:             GrainSiloCaptured,
00165:             FuelDepotBurned,
00166:             MercyRoad,
00167:             IronWay,
00168:             ListenersThread,
00169:             MercyWaterHeld,
00170:             IronFuelAsh,
00171:             ShelterFalls
00172:         };
00173:
00174:         public static string Evaluate(EpilogueMatrixInput? input)
00175:         {
00176:             if (input == null) return Unwritten;
00177:
00178:             // 1. Terminal Failure / Collapse
00179:             if (input.ShelterFallen) return ShelterFalls;
00180:
00181:             // 2. Specific Compound Endings
00182:             if (input.MercyPattern && input.WaterPlantHeld) return MercyWaterHeld;
00183:             if (input.IronPattern && input.FuelDepotBurned) return IronFuelAsh;
00184:
00185:             // 3. Verdict Specific Outcome
00186:             if (!string.IsNullOrEmpty(input.VerdictEndingKey)) return input.VerdictEndingKey;
00187:
00188:             // 4. Muster Specific Approach
00189:             if (!string.IsNullOrEmpty(input.MusterEndingKey)) return input.MusterEndingKey;
00190:
00191:             // 5. Faction Terminal Outcome
00192:             switch (input.FactionOutcome)
00193:             {
00194:                 case FactionTerminalOutcome.GarrisonAbsorbed:
00195:                     return GarrisonAbsorbsCoalition;
00196:                 case FactionTerminalOutcome.RebuildersJoined:
00197:                     return RebuildersJoined;
00198:                 case FactionTerminalOutcome.Independent:
00199:                     return CoalitionIndependent;
00200:                 case FactionTerminalOutcome.FoundryAnnexed:
00201:                     return FoundryAnnexation;
00202:             }
00203:
00204:             // 6. Strategic Resource Outcome
00205:             if (input.WaterPlantHeld) return WaterPlantHeld;
00206:             if (input.GrainSiloCaptured) return GrainSiloCaptured;
00207:             if (input.FuelDepotBurned) return FuelDepotBurned;
00208:
00209:             // 7. Moral Pattern Outcome
00210:             if (input.MercyPattern) return MercyRoad;
00211:             if (input.IronPattern) return IronWay;
00212:             if (input.DiplomacyPattern) return ListenersThread;
00213:
00214:             // 8. Fallback / Uninvestigated
00215:             return Unwritten;
00216:         }
00217:     }
00218: }
```


# Appendix — Current Source Detail: `src/Main.Muster.cs`

### `src/Main.Muster.cs` — bounded current excerpt (448 of 491 lines)

- Size: 491 lines / 21914 bytes.
- SHA-256: `57caac9a6e3bc274ef7a8aeefb4b4b56e34a81b6b713a3977d127d2f3c461ad1`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using Godot;
00003: using System;
00004: using System.Globalization;
00005: using System.IO;
00006: using System.Linq;
00007: using System.Collections.Generic;
00008: using Ashfall.Core.Combat;
00009: using AtomicWar.Journal;
00010: using Ashfall.Core;
00011: using Ashfall.Core.Campaign;
00012: using Ashfall.Core.Economy;
00013: using Ashfall.Core.Expeditions;
00014: using Ashfall.Core.Foundry;
00015: using Ashfall.Core.Inventory;
00016: using Ashfall.Core.Journal;
00017: using Ashfall.Core.Muster;
00018: using Ashfall.Core.YearOfAsh;
00019: using Ashfall.Core.Radio;
00020: using Ashfall.Core.Survivors;
00021: using AtomicWar.GodotApp.Economy;
00022: using AtomicWar.GodotApp.YearOfAsh;
00023: using AtomicWar.GodotApp.Muster;
00024: using AtomicWar.GodotApp.Dose;
00025: using AtomicWar.GodotApp.UtilityAI;
00026: using AtomicWar.GodotApp.Radio;
00027: using AtomicWar.GodotApp.Audio;
00028: using AtomicWar.GodotApp.UI;
00029:
00030: namespace AtomicWar.GodotApp
00031: {
00032:     public partial class Main : Control
00033:     {
00034:         // ── Muster fields (GAP-ARCH-01 Phase 1) ──
00035:         private MusterHostSession _muster = null!;
00036:         private CurrentsRosterWidget _currentsRoster = null!;
00037:         private ApproachSelectionModal _approachModal = null!;
00038:         private DeserterCoalitionCampWidget _campWidget = null!;
00039:         private JournalWitnessPanel _witnessPanel = null!;
00040:         private FactionActionPanel _factionActionPanel = null!;
00041:
00042:         private void SetupMuster()
00043:         {
00044:             if (_muster != null) return;
00045:             _muster = MusterHostSession.Create(_dataDir);
00046:             _muster.StateChanged += () => SaveMuster();
00047:             _muster.OnQuestlineResolved += OnMusterQuestlineResolved;
00048:             _muster.OnActionResolved += OnMusterActionResolved;
00049:             _muster.SubjectLivingResolver = id => _survivors?.RosterState?.Find(r => r != null && r.Id == id)?.IsAlive ?? true;
00050:             _muster.ItemSink = new ShelterFactionActionItemSink(() => _inventory?.Inventory);
00051:
00052:             // Plan 45 phase 2 — an Iron Raiders raid is "a combat/loss event
00053:             // with no dialogue" (IronRaidersSystem): defend the shelter with
00054:             // the raid composition from the combat catalog (warlord enforcers
00055:             // and their crews), not the legacy template.
00056:             _muster.IronRaiders.OnRaidExecuted += OnIronRaidersRaidExecuted;
00057:
00058:             int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
00059:             _muster.Escalate(day);
00060:
00061:             // Core campaign composition is also used by headless validation.
00062:             // Keep the gameplay session and day state available there without
00063:             // requiring the presentation right column to exist.
00064:             if (_rightColumn == null)
00065:                 return;
00066:
00067:             if (_currentsRoster == null)
00068:             {
00069:                 _currentsRoster = new CurrentsRosterWidget();
00070:                 _rightColumn.AddChild(_currentsRoster);
00071:             }
00072:             _currentsRoster.Bind(_muster.Roster, _muster.Engine);
00073:             _currentsRoster.RefreshView();
00074:
00075:             if (_campWidget == null)
00076:             {
00077:                 _campWidget = new DeserterCoalitionCampWidget();
00078:                 _rightColumn.AddChild(_campWidget);
00079:             }
00080:             _campWidget.Bind(_muster.Camp);
00081:             _campWidget.RefreshView();
00082:
00083:             if (_witnessPanel == null)
00084:             {
00085:                 _witnessPanel = new JournalWitnessPanel();
00086:                 _rightColumn.AddChild(_witnessPanel);
00087:             }
00088:             _witnessPanel.Bind(_muster.Witnesses);
00089:             _witnessPanel.RefreshView(_yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay, _muster.AuthorBias);
00090:
00091:             if (_factionActionPanel == null)
00092:             {
00093:                 _factionActionPanel = new FactionActionPanel();
00094:                 _factionActionPanel.OnChoicePressed += OnMusterFactionChoicePressed;
00095:                 _rightColumn.AddChild(_factionActionPanel);
00096:             }
00097:             _factionActionPanel.Bind(_muster.Board);
00098:             _factionActionPanel.BindCulture(_muster.Culture);
00099:             _factionActionPanel.RefreshView(_yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay);
00100:
00101:             if (_approachModal == null)
00102:             {
00103:                 _approachModal = new ApproachSelectionModal();
00104:                 _approachModal.OnApproachChosen += OnMusterApproachChosen;
00105:                 _approachModal.OnModalClosed += () =>
00106:                 {
00107:                     _approachModal.QueueFree();
00108:                     _approachModal = null!;
00109:                 };
00115:         }
00116:
00117:         public void OnColdCountClicked()
00118:         {
00119:             SetupMuster();
00120:             var cc = _muster.ColdCount;
00121:             _codexViewer.Text =
00131:         }
00132:
00133:         public void OnHydroBaronsClicked()
00134:         {
00135:             SetupMuster();
00136:             var hb = _muster.HydroBarons;
00137:             _codexViewer.Text =
00138:                 "=== FACTION: COASTAL HYDRO-BARONS ===\n" +
00139:                 $"Is Active: {hb.State.isActive}\n" +
00140:                 $"Rate Card Revised: {hb.RateCardRevised}\n" +
00141:                 $"Plant Seized: {hb.PlantSeized}\n" +
00142:                 $"Admin Reform: {hb.AdminReform}\n" +
00143:                 $"Queue Position: {hb.QueuePosition}\n" +
00144:                 $"Trust: {hb.State.trust:F1}\n" +
00145:                 $"Approach: {(string.IsNullOrEmpty(hb.State.approach) ? "Unresolved" : hb.State.approach)}\n\n" +
00146:                 "The Rate Card War at Desalination Unit 4. The iron chit queue governs fresh water allocation.";
00147:             _statusLabel.Text = $"Hydro-Barons: Queue Pos {hb.QueuePosition}, Approach {hb.State.approach}.";
00148:         }
00149:
00150:         /// <summary>
00151:         /// Plan 45 phase 2 — the Iron Raiders raid executes. Plan 163 inserts
00152:         /// the static-defense phase BEFORE direct survivor combat: traps, then
00153:         /// perimeter emplacements, resolve first; only raiders that breach
00154:         /// escalate to the tactical fight (with the enemy count scaled to what
00155:         /// is left). Skipped when a fight is already active — the shelter
00156:         /// defense joins the queue like any other encounter.
00157:         /// </summary>
00158:         private void OnIronRaidersRaidExecuted()
00159:         {
00160:             if (_combat == null) return;
00161:             var cs = _combat.Engine.State;
00162:             bool idle = string.IsNullOrEmpty(cs.EncounterId) || cs.Resolved;
00163:             if (!idle)
00164:             {
00165:                 GD.Print("[Ashfall Godot] Iron Raiders raid resolved as losses — combat already active.");
00166:                 return;
00167:             }
00168:             var ir = _muster.IronRaiders;
00169:             int crewDanger = ir.AggressionLevel >= 0.6f ? 6 : 3;
00170:
00171:             // Plan 163 pre-combat: static defenses engage first. A repelled
00172:             // raid never reaches survivor combat; captives hand off to the
00173:             // prisoner authority from the defense session itself.
00174:             var engagement = ResolveRaidDefenses(_simDay, crewDanger, isNight: false);
00175:             if (engagement.Repelled && engagement.RemainingRaiders <= 0)
00176:             {
00177:                 _combatDirty = true;
00178:                 _journal?.TryAddRawEntry($"raid_repelled_{_simDay}",
00179:                     "The wire held. Traps and emplacements broke the raid before it reached the door.",
00180:                     null!, _simDay);
00181:                 GD.Print($"[Ashfall Godot] Iron Raiders raid repelled by static defenses (trapped {engagement.RaidersNeutralizedByTraps}, captured {engagement.RaidersCaptured}).");
00182:                 return;
00183:             }
00184:
00185:             int remaining = Math.Max(1, engagement.RemainingRaiders);
00186:             int enemyCount = Math.Max(1, Math.Min(remaining, CombatHostSession.DefaultAmbushEnemyCount + 1));
00187:             var enemyIds = EnemyCompositionSelector.SelectRaidComposition(
00188:                 crewDanger, enemyCount);
00189:             _combat.StartCombat(
00190:                 "loc_iron_raiders_den", "The Toll — Den Raid",
00191:                 enemyCombatantIds: enemyIds,
00195:         }
00196:
00197:         public void OnIronRaidersClicked()
00198:         {
00199:             SetupMuster();
00200:             var ir = _muster.IronRaiders;
00201:             // Plan VIII · Task 21.5 — raid pressure reads include the treaty term:
00202:             // ratified security pacts relieve pressure, breached accords raise it.
00203:             float treatyMod = _regionalTreaty != null ? _regionalTreaty.System.GetRaidPressureModifier() : 0f;
00204:             // Compose at the read site (same pattern as DebtConsequenceHostBridge):
00205:             // EvaluateRaidChance owns base+visibility; treaty pressure is an additive term.
00206:             float effectiveChance = System.Math.Clamp(ir.EvaluateRaidChance() + treatyMod, 0f, 1f);
00207:             string treatyNote = MathF.Abs(treatyMod) > 0.0005f
00208:                 ? $"\nRegional treaties: {treatyMod:+0%;-0%} raid pressure\n"
00209:                 : string.Empty;
00210:             _codexViewer.Text =
00211:                 "=== FACTION: IRON RAIDERS (DEN DEFENSE) ===\n" +
00212:                 $"Is Active: {ir.State.isActive}\n" +
00213:                 $"Aggression Level: {ir.AggressionLevel:P0}\n" +
00214:                 $"Shelter Visibility: {ir.State.shelterVisibility:P0}\n" +
00215:                 $"Raid Chance Today: {effectiveChance:P0}\n" +
00216:                 treatyNote +
00217:                 $"Raids This Season: {ir.RaidsThisSeason}\n\n" +
00218:                 "The Toll's den at loc_iron_raiders_den. Fortifying approach routes reduces shelter visibility and raid chance.";
00219:             _statusLabel.Text = $"Iron Raiders: Aggression {ir.AggressionLevel:P0}, Raid Chance {effectiveChance:P0}.";
00220:         }
00221:
00222:         public void OnLongWalkClicked()
00223:         {
00224:             SetupMuster();
00225:             var lw = _muster.LongWalk;
00226:             _codexViewer.Text =
00227:                 "=== FACTION: THE LONG WALK (CIRCUIT TRADER) ===\n" +
00228:                 $"Is Active: {lw.State.isActive}\n" +
00229:                 $"Current Region: {lw.State.currentRegion}\n" +
00230:                 $"Days Until Departure: {lw.State.daysUntilDeparture}\n" +
00231:                 $"Crossings Completed: {lw.State.crossingsCompleted}\n" +
00232:                 $"Escort Count: {lw.State.escortCount}\n" +
00233:                 $"Resupply Count: {lw.State.resupplyCount}\n\n" +
00234:                 "Osric Fane's circuit trader across six regions. Requests return a deliberately stale situation report.";
00235:             _statusLabel.Text = $"Long Walk: in {lw.State.currentRegion}, departs in {lw.State.daysUntilDeparture} days.";
00236:         }
00237:
00238:         public void OnProvisionedClicked()
00239:         {
00240:             SetupMuster();
00241:             var ps = _muster.Provisioned;
00242:             _codexViewer.Text =
00243:                 "=== FACTION: THE PROVISIONED (SECOND WINTER) ===\n" +
00244:                 $"Is Active: {ps.State.isActive}\n" +
00245:                 $"Respect Score: {ps.RespectScore}/{Ashfall.Core.Muster.ProvisionedState.ContactThreshold}\n" +
00246:                 $"Contact Made: {ps.HaveMadeContact}\n" +
00247:                 $"Unlocked Trades: {ps.State.unlockedTradeIds.Count}\n\n" +
00248:                 "Pre-war stockholders behind Quenna Brix at loc_second_winter_homestead. Respect is earned unprompted.";
00249:             _statusLabel.Text = $"The Provisioned: Respect {ps.RespectScore}, Contact: {ps.HaveMadeContact}.";
00250:         }
00251:
00252:         public void OnScavengerGuildClicked()
00253:         {
00254:             SetupMuster();
00255:             var sg = _muster.ScavengerGuild;
00256:             _codexViewer.Text =
00264:         }
00265:
00266:         public void OnMusterEscalateClicked()
00267:         {
00268:             SetupMuster();
00269:             int target = Math.Min(360, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay + 10 : _simDay + 10);
00270:             _statusLabel.Text = _muster.Escalate(target);
00271:             _currentsRoster.RefreshView();
00272:             _campWidget.RefreshView();
00273:         }
00274:
00275:         private void OnMusterRallyClicked()
00276:         {
00277:             SetupMuster();
00278:             _statusLabel.Text = _muster.RallyDeserter();
00279:             _campWidget.RefreshView();
00280:         }
00281:
00282:         private void OnMusterStrategyBClicked()
00283:         {
00284:             SetupMuster();
00285:             _statusLabel.Text = _muster.SetStrategy(QuestApproach.B);
00286:             _campWidget.RefreshView();
00287:         }
00288:
00289:         private void OnMusterStrategyDClicked()
00290:         {
00291:             SetupMuster();
00292:             _statusLabel.Text = _muster.SetStrategy(QuestApproach.D);
00293:             _campWidget.RefreshView();
00294:         }
00295:
00296:         private void OnMusterWitnessesClicked()
00297:         {
00298:             SetupMuster();
00299:             int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
00300:             var delivered = _muster.DeliverWitnesses(day);
00301:             _witnessPanel.RefreshView(day, _muster.AuthorBias);
00302:             _factionActionPanel.RefreshView(day);
00303:             _statusLabel.Text = _muster.Witnesses.Count == 0
00304:                 ? "No witness accounts loaded."
00305:                 : $"{delivered.Count} testimonies delivered at the gathering (day {day}).";
00306:         }
00307:
00308:         private void OnMusterFactionChoicePressed(string actionId, string choiceId)
00309:         {
00310:             if (_muster == null) return;
00311:             int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
00312:             bool ok = _muster.ResolveFactionAction(actionId, choiceId, day);
00313:             _factionActionPanel.RefreshView(day);
00314:             _currentsRoster?.RefreshView();
00315:             _codexViewer.Text = _muster.LastEvent;
00316:         }
00317:
00318:         private void OnMusterActionResolved(FactionActionResolutionRecord record)
00319:         {
00320:             if (record == null) return;
00321:             if (_journal != null)
00322:             {
00323:                 string line = $"[FACTION ACTION] {record.actionId} / {record.choiceId} resolved ({record.band} standing) on day {record.day}.";
00324:                 _journal.TryAddRawEntry(
00325:                     $"faction_action_{record.actionId}_{record.choiceId}",
00326:                     line,
00327:                     null!,
00328:                     record.day);
00329:                 _journalDirty = true;
00330:             }
00331:             _statusLabel?.SetDeferred(Label.PropertyName.Text,
00332:                 $"[FACTION ACTION] {record.actionId} / {record.choiceId} resolved.");
00333:         }
00334:
00335:         private void OnMusterAuthorBiasClicked()
00336:         {
00337:             SetupMuster();
00338:             int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
00339:             _statusLabel.Text = _muster.CycleAuthorBias();
00341:         }
00342:
00343:         private void OnMusterEpiloguesClicked()
00344:         {
00345:             SetupMuster();
00346:             var sb = new System.Text.StringBuilder();
00347:             sb.AppendLine("=== THE EPILOGUE MATRIX (DAY 360) ===");
00349:             {
00350:                 var e = _muster.Epilogues[i];
00351:                 bool resolved = _muster.Engine.EndingKeyForAny(e.endingKey);
00352:                 sb.AppendLine(resolved
00353:                     ? $"[RESOLVED] {e.title}"
00354:                     : $"[open]     {e.title}");
00355:             }
00356:             sb.AppendLine();
00357:             sb.AppendLine("=== RESOLVED OUTCOMES ===");
00358:             bool any = false;
00359:             for (int i = 0; i < _muster.Epilogues.Count; i++)
00360:             {
00361:                 var e = _muster.Epilogues[i];
00368:                 }
00369:             }
00370:             if (!any) sb.AppendLine("None. The Muster has not resolved an outcome yet.");
00371:             _codexViewer.Text = sb.ToString();
00372:             _statusLabel.Text = $"Epilogue matrix: {_muster.Epilogues.Count} outcomes.";
00373:         }
00374:
00375:         private void OnMusterRosterClicked()
00376:         {
00377:             SetupMuster();
00378:             _statusLabel.Text = $"Currents shown: {_muster.Roster.Count} (fifteenth: faction_hydro_barons).";
00379:         }
00380:
00381:         private void OpenMusterApproachModal(string questlineId, IReadOnlyList<ApproachOption> approaches)
00382:         {
00383:             _selectedApproachQuestlineId = questlineId;
00384:             if (_approachModal == null)
00385:             {
00386:                 _approachModal = new ApproachSelectionModal();
00387:                 _approachModal.OnApproachChosen += OnMusterApproachChosen;
00388:                 _approachModal.OnModalClosed += () =>
00389:                 {
00390:                     _approachModal?.QueueFree();
00391:                     _approachModal = null!;
00392:                 };
00397:         }
00398:
00399:         private void OnMusterRateCardClicked()
00400:         {
00401:             SetupMuster();
00402:             var def = _muster.Engine.FindDefinition("quest_the_rate_card_war");
00403:             if (def == null)
00409:         }
00410:
00411:         private void OnMusterApproachChosen(QuestApproach approach)
00412:         {
00413:             if (_muster == null) return;
00414:             string qId = string.IsNullOrEmpty(_selectedApproachQuestlineId) ? "quest_the_rate_card_war" : _selectedApproachQuestlineId;
00415:             _statusLabel.Text = _muster.SelectApproach(qId, approach);
00418:         }
00419:
00420:         private void OnMusterQuestlineResolved(MusterRecord record)
00421:         {
00422:             if (record == null) return;
00423:             string line = $"[MUSTER RESOLVED] {record.questlineId} via {record.selectedApproach} → Ending: {record.endingKey}";
00424:             GD.Print($"[Ashfall Godot] {line}");
00425:             if (_hostEventAdapter != null)
00426:             {
00427:                 string eventId = $"event_muster_{record.questlineId}_{record.selectedApproach}";
00428:                 _hostEventAdapter.TriggerEvent(eventId, _simDay);
00429:             }
00430:             if (_journal != null)
00431:             {
00432:                 _journal.TryAddRawEntry(
00433:                     $"muster_{record.questlineId}_{record.selectedApproach}",
00434:                     line,
00435:                     null!,
00436:                     _simDay);
00437:                 _journalDirty = true;
00438:             }
00439:             _statusLabel?.SetDeferred(Label.PropertyName.Text, line);
00440:         }
00441:
00442:         /// <summary>Auto-escalate the Muster from the Year-of-Ash clock.</summary>
00443:         private void AutoEscalateMuster()
00444:         {
00445:             if (_yearOfAsh == null) return;
00446:             SetupMuster();
00447:             _muster.Escalate(_yearOfAsh.Timeline.CurrentDay);
00448:             _currentsRoster.RefreshView();
00449:             _campWidget.RefreshView();
00450:             _witnessPanel.RefreshView(_yearOfAsh.Timeline.CurrentDay, _muster.AuthorBias);
00451:             _factionActionPanel.RefreshView(_yearOfAsh.Timeline.CurrentDay);
00452:         }
00453:
00454:         private void SaveMuster()
00455:         {
00456:             if (_muster == null) return;
00457:             if (CaptureSection("muster", MusterSaveStore.TryCapturePersisted(_muster.CaptureSave())))
00458:                 GD.Print("[Ashfall Godot] Muster save written.");
00459:         }
00460:
00461:         private void CloseMusterPanel()
00462:         {
00463:             if (_musterPanel != null)
00464:                 _musterPanel.Visible = false;
00465:         }
00466:
00467:         private sealed class ShelterFactionActionItemSink : IFactionActionItemSink
00468:         {
00469:             private readonly Func<Ashfall.Core.Inventory.Inventory?> _inventoryProvider;
00470:             public ShelterFactionActionItemSink(Func<Ashfall.Core.Inventory.Inventory?> inventoryProvider)
00471:             {
00472:                 _inventoryProvider = inventoryProvider;
00473:             }
00474:
00475:             public bool Deliver(string itemId, int amount)
00476:             {
00477:                 if (string.IsNullOrEmpty(itemId) || amount == 0) return true;
00478:                 var inv = _inventoryProvider();
00479:                 if (inv == null) return false;
00480:                 if (amount > 0)
00481:                 {
00482:                     return inv.AddById(itemId, amount);
00483:                 }
00484:                 else
00485:                 {
00486:                     return inv.TryConsume(itemId, -amount);
00487:                 }
00488:             }
00489:         }
00490:     }
00491: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs`

### `Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs` — complete current file

- Size: 243 lines / 9760 bytes.
- SHA-256: `55d27dae68cedb1c691bce71b366ee18a77e69825b85d73bac2e88ee708855d5`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // Comprehensive unit tests for Plan 96 — Epilogue Chronicle Slides Expansion (5 -> 20 slides).
00003:
00004: using System;
00005: using System.Collections.Generic;
00006: using System.IO;
00007: using System.Linq;
00008: using Ashfall.Core.Endgame;
00009: using Ashfall.Core.Muster;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests.Endgame
00013: {
00014:     public class EpilogueChronicleCatalogTests
00015:     {
00016:         private static string FindDataDir()
00017:         {
00018:             string candidate = Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data");
00019:             if (Directory.Exists(candidate)) return candidate;
00020:
00021:             var dir = new DirectoryInfo(AppContext.BaseDirectory);
00022:             while (dir != null)
00023:             {
00024:                 string check = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
00025:                 if (Directory.Exists(check)) return check;
00026:                 dir = dir.Parent;
00027:             }
00028:             return string.Empty;
00029:         }
00030:
00031:         private static EpilogueChronicleCatalogData LoadCatalog()
00032:         {
00033:             string dataDir = FindDataDir();
00034:             Assert.False(string.IsNullOrEmpty(dataDir), "Could not find StreamingAssets/Data directory");
00035:             var io = new FileSystemIO();
00036:             var json = new SystemTextJsonSerializer();
00037:             var catalog = EpilogueChronicleLoader.Load(dataDir, io, json);
00038:             Assert.NotNull(catalog);
00039:             return catalog!;
00040:         }
00041:
00042:         [Fact]
00043:         public void Catalog_LoadsSuccessfully_WithSchemaVersionOne()
00044:         {
00045:             var catalog = LoadCatalog();
00046:             Assert.Equal(1, catalog.schema_version);
00047:             Assert.NotNull(catalog.default_slides);
00048:         }
00049:
00050:         [Fact]
00051:         public void SlideCount_ContainsExactlyTwentySlides()
00052:         {
00053:             var catalog = LoadCatalog();
00054:             Assert.Equal(20, catalog.default_slides.Count);
00055:         }
00056:
00057:         [Fact]
00058:         public void Parity_BaselineFiveSlidesArePreserved()
00059:         {
00060:             var catalog = LoadCatalog();
00061:             var slides = catalog.default_slides;
00062:
00063:             var opening = slides.FirstOrDefault(s => s.title == "Opening");
00064:             Assert.NotNull(opening);
00065:             Assert.Equal("epilogue_opening_placeholder", opening.art_asset_id);
00066:
00067:             var bunker = slides.FirstOrDefault(s => s.title == "The Bunker");
00068:             Assert.NotNull(bunker);
00069:             Assert.Equal("epilogue_bunker_placeholder", bunker.art_asset_id);
00070:
00071:             var remains = slides.FirstOrDefault(s => s.title == "What Remains");
00072:             Assert.NotNull(remains);
00073:             Assert.Equal("epilogue_remains_placeholder", remains.art_asset_id);
00074:
00075:             var survivors = slides.FirstOrDefault(s => s.title == "Survivors");
00076:             Assert.NotNull(survivors);
00077:             Assert.Equal("epilogue_survivors_placeholder", survivors.art_asset_id);
00078:
00079:             var finalWord = slides.FirstOrDefault(s => s.title == "Final Word");
00080:             Assert.NotNull(finalWord);
00081:             Assert.Equal("epilogue_final_placeholder", finalWord.art_asset_id);
00082:         }
00083:
00084:         [Fact]
00085:         public void SlideOrders_AreUniqueAndSequentialZeroToNineteen()
00086:         {
00087:             var catalog = LoadCatalog();
00088:             var orders = catalog.default_slides.Select(s => s.order).ToList();
00089:
00090:             Assert.Equal(20, orders.Distinct().Count());
00091:             for (int i = 0; i < 20; i++)
00092:             {
00093:                 Assert.Contains(i, orders);
00094:             }
00095:         }
00096:
00097:         [Fact]
00098:         public void SlideTitles_AreNonEmptyAndConciseOneToFourWords()
00099:         {
00100:             var catalog = LoadCatalog();
00101:             foreach (var slide in catalog.default_slides)
00102:             {
00103:                 Assert.False(string.IsNullOrWhiteSpace(slide.title), $"Slide order {slide.order} has empty title");
00104:
00105:                 var words = slide.title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
00106:                 Assert.InRange(words.Length, 1, 4);
00107:             }
00108:         }
00109:
00110:         [Fact]
00111:         public void ArtAssetIds_FollowPlaceholderGrammarAndAreUnique()
00112:         {
00113:             var catalog = LoadCatalog();
00114:             var artIds = new HashSet<string>(StringComparer.Ordinal);
00115:
00116:             foreach (var slide in catalog.default_slides)
00117:             {
00118:                 Assert.False(string.IsNullOrWhiteSpace(slide.art_asset_id), $"Slide {slide.title} has empty art_asset_id");
00119:                 Assert.StartsWith("epilogue_", slide.art_asset_id);
00120:                 Assert.EndsWith("_placeholder", slide.art_asset_id);
00121:                 Assert.True(artIds.Add(slide.art_asset_id), $"Duplicate art_asset_id found: {slide.art_asset_id}");
00122:             }
00123:         }
00124:
00125:         [Fact]
00126:         public void SemanticCollisions_NoRedundantOverlappingRoles()
00127:         {
00128:             var catalog = LoadCatalog();
00129:             var titles = catalog.default_slides.Select(s => s.title).ToHashSet(StringComparer.OrdinalIgnoreCase);
00130:
00131:             // Avoided duplicate synonyms
00132:             Assert.DoesNotContain("The Shelter", titles);
00133:             Assert.DoesNotContain("The Last Word", titles);
00134:             Assert.DoesNotContain("The Ending", titles);
00135:             Assert.DoesNotContain("The Future", titles);
00136:         }
00137:
00138:         [Fact]
00139:         public void PillarCoverage_SpansAllMajorCampaignThemes()
00140:         {
00141:             var catalog = LoadCatalog();
00142:             var titles = catalog.default_slides.Select(s => s.title).ToList();
00143:
00144:             // Opening / Catastrophe
00145:             Assert.Contains("Opening", titles);
00146:             Assert.Contains("After the Flash", titles);
00147:             Assert.Contains("The Bunker", titles);
00148:             Assert.Contains("First Winter", titles);
00149:
00150:             // Sustenance & Demographics
00151:             Assert.Contains("Water and Heat", titles);
00152:             Assert.Contains("Survivors", titles);
00153:             Assert.Contains("Empty Bunks", titles);
00154:
00155:             // World & Social Systems
00156:             Assert.Contains("The Factions", titles);
00157:             Assert.Contains("Lines on the Map", titles);
00158:             Assert.Contains("Voices in Static", titles);
00159:
00160:             // Forensic & Historical Records
00161:             Assert.Contains("The Verdict", titles);
00162:             Assert.Contains("The Witnesses", titles);
00163:             Assert.Contains("Restored Relics", titles);
00164:             Assert.Contains("What We Chose", titles);
00165:
00166:             // Culmination & Legacy
00167:             Assert.Contains("The Muster", titles);
00168:             Assert.Contains("The Resolution", titles);
00169:             Assert.Contains("The Census", titles);
00170:             Assert.Contains("What Remains", titles);
00171:             Assert.Contains("After Us", titles);
00172:             Assert.Contains("Final Word", titles);
00173:         }
00174:
00175:         [Fact]
00176:         public void BuilderIntegration_SortsAllTwentySlidesDeterministically()
00177:         {
00178:             string dataDir = FindDataDir();
00179:             var io = new FileSystemIO();
00180:             var json = new SystemTextJsonSerializer();
00181:             var slides = EpilogueChronicleLoader.LoadDefaultSlides(dataDir, io, json);
00182:
00183:             Assert.Equal(20, slides.Count);
00184:
00185:             // Permute input out-of-order to test builder sorting
00186:             var shuffled = slides.OrderBy(s => (s.Order * 7 + 11) % 20).ToList();
00187:
00188:             var builder = new EpilogueChronicleBuilder();
00189:             var chronicle = builder.Build(new EpilogueChronicleInput
00190:             {
00191:                 EndingKey = EpilogueMatrix.TheOpenMuster,
00192:                 Day = 360,
00193:                 BuildSeed = 42,
00194:                 Slides = shuffled
00195:             });
00196:
00197:             Assert.Equal(20, chronicle.Slides.Count);
00198:             for (int i = 0; i < 20; i++)
00199:             {
00200:                 Assert.Equal(i, chronicle.Slides[i].Order);
00201:             }
00202:             Assert.Equal("Opening", chronicle.Slides[0].Title);
00203:             Assert.Equal("Final Word", chronicle.Slides[19].Title);
00204:         }
00205:
00206:         [Fact]
00207:         public void Plan89Bindings_OutcomeMappingTable_MapsToRelevantSlides()
00208:         {
00209:             var catalog = LoadCatalog();
00210:             var failures = new List<string>();
00211:
00212:             foreach (var testCase in new[]
00213:             {
00214:                 (EndingKey: EpilogueMatrix.TheOpenMuster, ExpectedTitle: "The Muster", ExpectedArtId: "epilogue_coalition_placeholder"),
00215:                 (EndingKey: EpilogueMatrix.WaterPlantHeld, ExpectedTitle: "Water and Heat", ExpectedArtId: "epilogue_resources_placeholder"),
00216:                 (EndingKey: EpilogueMatrix.GarrisonAbsorbsCoalition, ExpectedTitle: "The Factions", ExpectedArtId: "epilogue_factions_placeholder"),
00217:                 (EndingKey: EpilogueMatrix.VerdictSectorRecounts, ExpectedTitle: "The Verdict", ExpectedArtId: "epilogue_investigations_placeholder"),
00218:                 (EndingKey: EpilogueMatrix.MercyRoad, ExpectedTitle: "What We Chose", ExpectedArtId: "epilogue_key_decisions_placeholder"),
00219:             })
00220:             {
00221:                 if (!EpilogueMatrix.AllKeys.Contains(testCase.EndingKey))
00222:                 {
00223:                     failures.Add($"ending key '{testCase.EndingKey}' is missing from EpilogueMatrix.AllKeys");
00224:                 }
00225:
00226:                 var matchingSlide = catalog.default_slides.FirstOrDefault(s => s.title == testCase.ExpectedTitle);
00227:                 if (matchingSlide == null)
00228:                 {
00229:                     failures.Add($"slide '{testCase.ExpectedTitle}' is missing from the catalog");
00230:                     continue;
00231:                 }
00232:
00233:                 if (matchingSlide.art_asset_id != testCase.ExpectedArtId)
00234:                 {
00235:                     failures.Add(
00236:                         $"slide '{testCase.ExpectedTitle}' expected art '{testCase.ExpectedArtId}', got '{matchingSlide.art_asset_id}'");
00237:                 }
00238:             }
00239:
00240:             Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
00241:         }
00242:     }
00243: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs`

### `Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs` — complete current file

- Size: 250 lines / 11653 bytes.
- SHA-256: `5d83bff7932ba9f38979191888fb0df4091c7646976a396dd2507dfbd2776db6`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Endgame;
00008: using Ashfall.Core.IO;
00009: using Ashfall.Core.Quests;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests.Narrative
00013: {
00014:     /// <summary>
00015:     /// Wave 40 Batch 8 Cross-System Integration Test:
00016:     /// Validates Plan 96 (Epilogue Chronicle Expansion - 20 ending presentation slides)
00017:     /// alongside Plan 104 (Narrative Questlines Expansion - 12 survivor-specific personal arcs).
00018:     /// </summary>
00019:     public sealed class Plan96_104ChronicleQuestIntegrationTests : CatalogTestBase
00020:     {
00021:         private static readonly (int order, string title, string artToken)[] BaselineFiveSlides =
00022:         {
00023:             (0, "Opening", "epilogue_opening_placeholder"),
00024:             (2, "The Bunker", "epilogue_bunker_placeholder"),
00025:             (5, "Survivors", "epilogue_survivors_placeholder"),
00026:             (17, "What Remains", "epilogue_remains_placeholder"),
00027:             (19, "Final Word", "epilogue_final_placeholder")
00028:         };
00029:
00030:         [Fact]
00031:         public void Plan96_EpilogueChronicle_LoadsAllTwentySlides_WithStrictContiguityAndPlaceholderConventions()
00032:         {
00033:             var files = new FileSystemIO();
00034:             var json = new SystemTextJsonSerializer();
00035:
00036:             var catalogData = EpilogueChronicleLoader.Load(DataDirectory, files, json);
00037:             Assert.NotNull(catalogData);
00038:             Assert.Equal(1, catalogData!.schema_version);
00039:             Assert.NotNull(catalogData.default_slides);
00040:             Assert.Equal(20, catalogData.default_slides.Count);
00041:
00042:             var slides = EpilogueChronicleLoader.LoadDefaultSlides(DataDirectory, files, json);
00043:             Assert.NotNull(slides);
00044:             Assert.Equal(20, slides.Count);
00045:
00046:             var seenOrders = new HashSet<int>();
00047:             for (int i = 0; i < 20; i++)
00048:             {
00049:                 var slide = slides[i];
00050:                 Assert.NotNull(slide);
00051:                 Assert.Equal(i, slide.Order);
00052:                 Assert.True(seenOrders.Add(slide.Order), $"Duplicate slide order: {slide.Order}");
00053:                 Assert.False(string.IsNullOrWhiteSpace(slide.Title), $"Slide order {i} has empty title");
00054:                 Assert.False(string.IsNullOrWhiteSpace(slide.ArtAssetId), $"Slide order {i} has empty art_asset_id");
00055:                 Assert.StartsWith("epilogue_", slide.ArtAssetId, StringComparison.Ordinal);
00056:                 Assert.EndsWith("_placeholder", slide.ArtAssetId, StringComparison.Ordinal);
00057:             }
00058:
00059:             // Verify the 5 baseline slides remain preserved byte-for-byte in their designated sequence slots
00060:             foreach (var (expectedOrder, expectedTitle, expectedToken) in BaselineFiveSlides)
00061:             {
00062:                 var slide = slides.FirstOrDefault(s => s.Order == expectedOrder);
00063:                 Assert.NotNull(slide);
00064:                 Assert.Equal(expectedTitle, slide.Title);
00065:                 Assert.Equal(expectedToken, slide.ArtAssetId);
00066:             }
00067:         }
00068:
00069:         [Fact]
00070:         public void Plan104_NarrativeQuestlines_LoadsAllTwelveArcs_WithValidFourStageChainsAndBinaryCrisis()
00071:         {
00072:             var files = new FileSystemIO();
00073:             var json = new SystemTextJsonSerializer();
00074:
00075:             var questlines = NarrativeQuestlineCatalogLoader.LoadEntries(DataDirectory, files, json);
00076:             Assert.NotNull(questlines);
00077:             Assert.Equal(12, questlines.Count);
00078:
00079:             var seenQuestIds = new HashSet<string>(StringComparer.Ordinal);
00080:             var seenSurvivorIds = new HashSet<string>(StringComparer.Ordinal);
00081:
00082:             foreach (var arc in questlines)
00083:             {
00084:                 Assert.NotNull(arc);
00085:                 Assert.StartsWith("quest_", arc.questId, StringComparison.Ordinal);
00086:                 Assert.True(seenQuestIds.Add(arc.questId), $"Duplicate questId detected: {arc.questId}");
00087:
00088:                 Assert.False(string.IsNullOrWhiteSpace(arc.survivorId), $"Quest '{arc.questId}' missing survivorId");
00089:                 Assert.True(seenSurvivorIds.Add(arc.survivorId), $"Multiple questlines bound to survivorId: {arc.survivorId}");
00090:
00091:                 Assert.False(string.IsNullOrWhiteSpace(arc.title), $"Quest '{arc.questId}' missing title");
00092:                 Assert.StartsWith("loc_", arc.targetLocationId, StringComparison.Ordinal);
00093:
00094:                 Assert.NotNull(arc.stages);
00095:                 Assert.Equal(4, arc.stages.Count);
00096:
00097:                 // Stage 0: Discovery
00098:                 var stage0 = arc.FindStage(0);
00099:                 Assert.NotNull(stage0);
00100:                 Assert.Equal("Discovery", stage0!.name);
00101:                 Assert.False(string.IsNullOrWhiteSpace(stage0.description));
00102:                 Assert.NotNull(stage0.objectiveItems);
00103:                 Assert.NotEmpty(stage0.objectiveItems);
00104:
00105:                 // Stage 1: Investigation
00106:                 var stage1 = arc.FindStage(1);
00107:                 Assert.NotNull(stage1);
00108:                 Assert.Equal("Investigation", stage1!.name);
00109:                 Assert.False(string.IsNullOrWhiteSpace(stage1.description));
00110:
00111:                 // Stage 2: Crisis (Binary Branch)
00112:                 var stage2 = arc.FindStage(2);
00113:                 Assert.NotNull(stage2);
00114:                 Assert.Equal("Crisis", stage2!.name);
00115:                 Assert.True(stage2.HasBranch, $"Quest '{arc.questId}' stage 2 must carry binary crisis branch");
00116:                 Assert.NotNull(stage2.branchA);
00117:                 Assert.NotNull(stage2.branchB);
00118:                 Assert.NotEqual(stage2.branchA!.id, stage2.branchB!.id);
00119:
00120:                 Assert.False(string.IsNullOrWhiteSpace(stage2.branchA.label));
00121:                 Assert.False(string.IsNullOrWhiteSpace(stage2.branchA.description));
00122:                 Assert.False(string.IsNullOrWhiteSpace(stage2.branchA.traitGranted));
00123:
00124:                 Assert.False(string.IsNullOrWhiteSpace(stage2.branchB.label));
00125:                 Assert.False(string.IsNullOrWhiteSpace(stage2.branchB.description));
00126:                 Assert.False(string.IsNullOrWhiteSpace(stage2.branchB.traitGranted));
00127:
00128:                 // Stage 3: Resolution
00129:                 var stage3 = arc.FindStage(3);
00130:                 Assert.NotNull(stage3);
00131:                 Assert.Equal("Resolution", stage3!.name);
00132:                 Assert.False(string.IsNullOrWhiteSpace(stage3.description));
00133:             }
00134:         }
00135:
00136:         [Fact]
00137:         public void CrossSystem_SurvivorArcResolutionAndEndgameChronicle_ExhibitNarrativeCoherence()
00138:         {
00139:             var files = new FileSystemIO();
00140:             var json = new SystemTextJsonSerializer();
00141:
00142:             // 1. Load both catalogs
00143:             var questlines = NarrativeQuestlineCatalogLoader.LoadEntries(DataDirectory, files, json);
00144:             var slides = EpilogueChronicleLoader.LoadDefaultSlides(DataDirectory, files, json);
00145:             Assert.Equal(12, questlines.Count);
00146:             Assert.Equal(20, slides.Count);
00147:
00148:             // 2. Initialize NarrativeQuestlineSystem and advance a representative arc (Marcus Olejnik - Machinist)
00149:             var questSystem = new NarrativeQuestlineSystem(questlines);
00150:             string survivorId = "marcus_olejnik";
00151:             var arcDef = questSystem.GetDefinitionForSurvivor(survivorId);
00152:             Assert.NotNull(arcDef);
00153:             Assert.Equal("quest_the_machinists_regret", arcDef!.questId);
00154:
00155:             // Start arc
00156:             bool started = questSystem.TryBegin(survivorId, day: 10);
00157:             Assert.True(started);
00158:             var arcState = questSystem.GetArc(survivorId);
00159:             Assert.NotNull(arcState);
00160:             Assert.Equal(NarrativeArcStatus.Active, arcState!.status);
00161:             Assert.Equal(0, arcState.currentStage);
00162:
00163:             // Deliver Stage 0 objective item: blueprint_roll
00164:             bool delivered0 = questSystem.TryDeliverItem(survivorId, "blueprint_roll", day: 11);
00165:             Assert.True(delivered0, "Delivering blueprint_roll must advance stage 0");
00166:             Assert.Equal(1, arcState.currentStage);
00167:
00168:             // Deliver Stage 1 objective items: scrap_metal and soldering_kit
00169:             bool delivered1a = questSystem.TryDeliverItem(survivorId, "scrap_metal", day: 12);
00170:             Assert.True(delivered1a);
00171:             Assert.Equal(1, arcState.currentStage);
00172:             bool delivered1b = questSystem.TryDeliverItem(survivorId, "soldering_kit", day: 13);
00173:             Assert.True(delivered1b);
00174:             Assert.Equal(2, arcState.currentStage);
00175:             Assert.Equal(NarrativeArcStatus.AwaitingBranch, arcState.status);
00176:
00177:             // Choose branch A (Scrupulous Machinist: dismantle_weapon)
00178:             bool branchChosen = questSystem.TryChooseBranch(survivorId, "dismantle_weapon", day: 25, out var chosenBranch);
00179:             Assert.True(branchChosen);
00180:             Assert.NotNull(chosenBranch);
00181:             Assert.Equal("trait_scrupulous_machinist", chosenBranch!.traitGranted);
00182:             Assert.Equal(NarrativeArcStatus.Resolved, arcState.status);
00183:             Assert.Equal(3, arcState.currentStage);
00184:
00185:             // 3. Construct Endgame Chronicle reflecting the resolved survivor state
00186:             var fateCards = new List<SurvivorFateCard>
00187:             {
00188:                 new SurvivorFateCard
00189:                 {
00190:                     SurvivorId = survivorId,
00191:                     DisplayName = "Marcus Olejnik",
00192:                     Fate = $"Marcus dismantled the siege weapon, earning the trait {chosenBranch.traitGranted}.",
00193:                     Survived = true
00194:                 }
00195:             };
00196:
00197:             var metrics = new List<EpilogueMetric>
00198:             {
00199:                 new EpilogueMetric("survivors_living", 12f, "Living Dwellers"),
00200:                 new EpilogueMetric("arcs_resolved", 1f, "Survivor Arcs Completed"),
00201:                 new EpilogueMetric("days_survived", 365f, "Days Survived")
00202:             };
00203:
00204:             var builder = new EpilogueChronicleBuilder();
00205:             var chronicle = builder.Build(new EpilogueChronicleInput
00206:             {
00207:                 EndingKey = "knowing",
00208:                 Day = 365,
00209:                 BuildSeed = 42,
00210:                 Slides = slides,
00211:                 FateCards = fateCards,
00212:                 Metrics = metrics
00213:             });
00214:
00215:             Assert.NotNull(chronicle);
00216:             Assert.Equal("Knowing", chronicle.Title);
00217:             Assert.Equal(20, chronicle.Slides.Count);
00218:             Assert.Single(chronicle.FateCards);
00219:             Assert.Equal(survivorId, chronicle.FateCards[0].SurvivorId);
00220:             Assert.Equal(3, chronicle.Metrics.Count);
00221:
00222:             // Verify slide ordering and narrative milestones
00223:             Assert.Equal("Opening", chronicle.Slides[0].Title);
00224:             Assert.Equal("The Bunker", chronicle.Slides[2].Title);
00225:             Assert.Equal("Survivors", chronicle.Slides[5].Title);
00226:             Assert.Equal("Empty Bunks", chronicle.Slides[6].Title);
00227:             Assert.Equal("The Resolution", chronicle.Slides[15].Title);
00228:             Assert.Equal("Final Word", chronicle.Slides[19].Title);
00229:         }
00230:
00231:         [Fact]
00232:         public void CrossSystem_DeterministicExecution_UnderRepeatedReloadsPasses()
00233:         {
00234:             var files = new FileSystemIO();
00235:             var json = new SystemTextJsonSerializer();
00236:
00237:             for (int i = 0; i < 50; i++)
00238:             {
00239:                 var slides = EpilogueChronicleLoader.LoadDefaultSlides(DataDirectory, files, json);
00240:                 Assert.Equal(20, slides.Count);
00241:
00242:                 var questlines = NarrativeQuestlineCatalogLoader.LoadEntries(DataDirectory, files, json);
00243:                 Assert.Equal(12, questlines.Count);
00244:
00245:                 var arc = questlines[0];
00246:                 Assert.Equal(4, arc.stages.Count);
00247:             }
00248:         }
00249:     }
00250: }
```


# Appendix — Focused Evidence Detail: `Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs`

### `Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs` — complete current file

- Size: 243 lines / 9760 bytes.
- SHA-256: `55d27dae68cedb1c691bce71b366ee18a77e69825b85d73bac2e88ee708855d5`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // Comprehensive unit tests for Plan 96 — Epilogue Chronicle Slides Expansion (5 -> 20 slides).
00003:
00004: using System;
00005: using System.Collections.Generic;
00006: using System.IO;
00007: using System.Linq;
00008: using Ashfall.Core.Endgame;
00009: using Ashfall.Core.Muster;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests.Endgame
00013: {
00014:     public class EpilogueChronicleCatalogTests
00015:     {
00016:         private static string FindDataDir()
00017:         {
00018:             string candidate = Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data");
00019:             if (Directory.Exists(candidate)) return candidate;
00020:
00021:             var dir = new DirectoryInfo(AppContext.BaseDirectory);
00022:             while (dir != null)
00023:             {
00024:                 string check = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
00025:                 if (Directory.Exists(check)) return check;
00026:                 dir = dir.Parent;
00027:             }
00028:             return string.Empty;
00029:         }
00030:
00031:         private static EpilogueChronicleCatalogData LoadCatalog()
00032:         {
00033:             string dataDir = FindDataDir();
00034:             Assert.False(string.IsNullOrEmpty(dataDir), "Could not find StreamingAssets/Data directory");
00035:             var io = new FileSystemIO();
00036:             var json = new SystemTextJsonSerializer();
00037:             var catalog = EpilogueChronicleLoader.Load(dataDir, io, json);
00038:             Assert.NotNull(catalog);
00039:             return catalog!;
00040:         }
00041:
00042:         [Fact]
00043:         public void Catalog_LoadsSuccessfully_WithSchemaVersionOne()
00044:         {
00045:             var catalog = LoadCatalog();
00046:             Assert.Equal(1, catalog.schema_version);
00047:             Assert.NotNull(catalog.default_slides);
00048:         }
00049:
00050:         [Fact]
00051:         public void SlideCount_ContainsExactlyTwentySlides()
00052:         {
00053:             var catalog = LoadCatalog();
00054:             Assert.Equal(20, catalog.default_slides.Count);
00055:         }
00056:
00057:         [Fact]
00058:         public void Parity_BaselineFiveSlidesArePreserved()
00059:         {
00060:             var catalog = LoadCatalog();
00061:             var slides = catalog.default_slides;
00062:
00063:             var opening = slides.FirstOrDefault(s => s.title == "Opening");
00064:             Assert.NotNull(opening);
00065:             Assert.Equal("epilogue_opening_placeholder", opening.art_asset_id);
00066:
00067:             var bunker = slides.FirstOrDefault(s => s.title == "The Bunker");
00068:             Assert.NotNull(bunker);
00069:             Assert.Equal("epilogue_bunker_placeholder", bunker.art_asset_id);
00070:
00071:             var remains = slides.FirstOrDefault(s => s.title == "What Remains");
00072:             Assert.NotNull(remains);
00073:             Assert.Equal("epilogue_remains_placeholder", remains.art_asset_id);
00074:
00075:             var survivors = slides.FirstOrDefault(s => s.title == "Survivors");
00076:             Assert.NotNull(survivors);
00077:             Assert.Equal("epilogue_survivors_placeholder", survivors.art_asset_id);
00078:
00079:             var finalWord = slides.FirstOrDefault(s => s.title == "Final Word");
00080:             Assert.NotNull(finalWord);
00081:             Assert.Equal("epilogue_final_placeholder", finalWord.art_asset_id);
00082:         }
00083:
00084:         [Fact]
00085:         public void SlideOrders_AreUniqueAndSequentialZeroToNineteen()
00086:         {
00087:             var catalog = LoadCatalog();
00088:             var orders = catalog.default_slides.Select(s => s.order).ToList();
00089:
00090:             Assert.Equal(20, orders.Distinct().Count());
00091:             for (int i = 0; i < 20; i++)
00092:             {
00093:                 Assert.Contains(i, orders);
00094:             }
00095:         }
00096:
00097:         [Fact]
00098:         public void SlideTitles_AreNonEmptyAndConciseOneToFourWords()
00099:         {
00100:             var catalog = LoadCatalog();
00101:             foreach (var slide in catalog.default_slides)
00102:             {
00103:                 Assert.False(string.IsNullOrWhiteSpace(slide.title), $"Slide order {slide.order} has empty title");
00104:
00105:                 var words = slide.title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
00106:                 Assert.InRange(words.Length, 1, 4);
00107:             }
00108:         }
00109:
00110:         [Fact]
00111:         public void ArtAssetIds_FollowPlaceholderGrammarAndAreUnique()
00112:         {
00113:             var catalog = LoadCatalog();
00114:             var artIds = new HashSet<string>(StringComparer.Ordinal);
00115:
00116:             foreach (var slide in catalog.default_slides)
00117:             {
00118:                 Assert.False(string.IsNullOrWhiteSpace(slide.art_asset_id), $"Slide {slide.title} has empty art_asset_id");
00119:                 Assert.StartsWith("epilogue_", slide.art_asset_id);
00120:                 Assert.EndsWith("_placeholder", slide.art_asset_id);
00121:                 Assert.True(artIds.Add(slide.art_asset_id), $"Duplicate art_asset_id found: {slide.art_asset_id}");
00122:             }
00123:         }
00124:
00125:         [Fact]
00126:         public void SemanticCollisions_NoRedundantOverlappingRoles()
00127:         {
00128:             var catalog = LoadCatalog();
00129:             var titles = catalog.default_slides.Select(s => s.title).ToHashSet(StringComparer.OrdinalIgnoreCase);
00130:
00131:             // Avoided duplicate synonyms
00132:             Assert.DoesNotContain("The Shelter", titles);
00133:             Assert.DoesNotContain("The Last Word", titles);
00134:             Assert.DoesNotContain("The Ending", titles);
00135:             Assert.DoesNotContain("The Future", titles);
00136:         }
00137:
00138:         [Fact]
00139:         public void PillarCoverage_SpansAllMajorCampaignThemes()
00140:         {
00141:             var catalog = LoadCatalog();
00142:             var titles = catalog.default_slides.Select(s => s.title).ToList();
00143:
00144:             // Opening / Catastrophe
00145:             Assert.Contains("Opening", titles);
00146:             Assert.Contains("After the Flash", titles);
00147:             Assert.Contains("The Bunker", titles);
00148:             Assert.Contains("First Winter", titles);
00149:
00150:             // Sustenance & Demographics
00151:             Assert.Contains("Water and Heat", titles);
00152:             Assert.Contains("Survivors", titles);
00153:             Assert.Contains("Empty Bunks", titles);
00154:
00155:             // World & Social Systems
00156:             Assert.Contains("The Factions", titles);
00157:             Assert.Contains("Lines on the Map", titles);
00158:             Assert.Contains("Voices in Static", titles);
00159:
00160:             // Forensic & Historical Records
00161:             Assert.Contains("The Verdict", titles);
00162:             Assert.Contains("The Witnesses", titles);
00163:             Assert.Contains("Restored Relics", titles);
00164:             Assert.Contains("What We Chose", titles);
00165:
00166:             // Culmination & Legacy
00167:             Assert.Contains("The Muster", titles);
00168:             Assert.Contains("The Resolution", titles);
00169:             Assert.Contains("The Census", titles);
00170:             Assert.Contains("What Remains", titles);
00171:             Assert.Contains("After Us", titles);
00172:             Assert.Contains("Final Word", titles);
00173:         }
00174:
00175:         [Fact]
00176:         public void BuilderIntegration_SortsAllTwentySlidesDeterministically()
00177:         {
00178:             string dataDir = FindDataDir();
00179:             var io = new FileSystemIO();
00180:             var json = new SystemTextJsonSerializer();
00181:             var slides = EpilogueChronicleLoader.LoadDefaultSlides(dataDir, io, json);
00182:
00183:             Assert.Equal(20, slides.Count);
00184:
00185:             // Permute input out-of-order to test builder sorting
00186:             var shuffled = slides.OrderBy(s => (s.Order * 7 + 11) % 20).ToList();
00187:
00188:             var builder = new EpilogueChronicleBuilder();
00189:             var chronicle = builder.Build(new EpilogueChronicleInput
00190:             {
00191:                 EndingKey = EpilogueMatrix.TheOpenMuster,
00192:                 Day = 360,
00193:                 BuildSeed = 42,
00194:                 Slides = shuffled
00195:             });
00196:
00197:             Assert.Equal(20, chronicle.Slides.Count);
00198:             for (int i = 0; i < 20; i++)
00199:             {
00200:                 Assert.Equal(i, chronicle.Slides[i].Order);
00201:             }
00202:             Assert.Equal("Opening", chronicle.Slides[0].Title);
00203:             Assert.Equal("Final Word", chronicle.Slides[19].Title);
00204:         }
00205:
00206:         [Fact]
00207:         public void Plan89Bindings_OutcomeMappingTable_MapsToRelevantSlides()
00208:         {
00209:             var catalog = LoadCatalog();
00210:             var failures = new List<string>();
00211:
00212:             foreach (var testCase in new[]
00213:             {
00214:                 (EndingKey: EpilogueMatrix.TheOpenMuster, ExpectedTitle: "The Muster", ExpectedArtId: "epilogue_coalition_placeholder"),
00215:                 (EndingKey: EpilogueMatrix.WaterPlantHeld, ExpectedTitle: "Water and Heat", ExpectedArtId: "epilogue_resources_placeholder"),
00216:                 (EndingKey: EpilogueMatrix.GarrisonAbsorbsCoalition, ExpectedTitle: "The Factions", ExpectedArtId: "epilogue_factions_placeholder"),
00217:                 (EndingKey: EpilogueMatrix.VerdictSectorRecounts, ExpectedTitle: "The Verdict", ExpectedArtId: "epilogue_investigations_placeholder"),
00218:                 (EndingKey: EpilogueMatrix.MercyRoad, ExpectedTitle: "What We Chose", ExpectedArtId: "epilogue_key_decisions_placeholder"),
00219:             })
00220:             {
00221:                 if (!EpilogueMatrix.AllKeys.Contains(testCase.EndingKey))
00222:                 {
00223:                     failures.Add($"ending key '{testCase.EndingKey}' is missing from EpilogueMatrix.AllKeys");
00224:                 }
00225:
00226:                 var matchingSlide = catalog.default_slides.FirstOrDefault(s => s.title == testCase.ExpectedTitle);
00227:                 if (matchingSlide == null)
00228:                 {
00229:                     failures.Add($"slide '{testCase.ExpectedTitle}' is missing from the catalog");
00230:                     continue;
00231:                 }
00232:
00233:                 if (matchingSlide.art_asset_id != testCase.ExpectedArtId)
00234:                 {
00235:                     failures.Add(
00236:                         $"slide '{testCase.ExpectedTitle}' expected art '{testCase.ExpectedArtId}', got '{matchingSlide.art_asset_id}'");
00237:                 }
00238:             }
00239:
00240:             Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
00241:         }
00242:     }
00243: }
```


# Appendix — Focused Evidence Detail: `Ashfall.Core.Tests/Endgame/EpilogueChronicleBuilderTests.cs`

### `Ashfall.Core.Tests/Endgame/EpilogueChronicleBuilderTests.cs` — complete current file

- Size: 147 lines / 5452 bytes.
- SHA-256: `6f99bb4cc40c06f31bcbe94f4a3765fbf0fc184babdf92ba7beb2e4a3b48d9e9`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.Endgame;
00005: using Xunit;
00006:
00007: namespace Ashfall.Core.Tests.Endgame
00008: {
00009:     public class EpilogueChronicleBuilderTests
00010:     {
00011:         [Fact]
00012:         public void Build_ProducesTitleForKnownEnding()
00013:         {
00014:             var b = new EpilogueChronicleBuilder();
00015:             var c = b.Build(new EpilogueChronicleInput
00016:             {
00017:                 EndingKey = "knowing",
00018:                 Day = 365,
00019:                 BuildSeed = 7,
00020:                 Slides = new List<EpilogueSlide>(),
00021:                 FateCards = new List<SurvivorFateCard>(),
00022:                 Metrics = new List<EpilogueMetric>()
00023:             });
00024:             Assert.Equal("Knowing", c.Title);
00025:             Assert.Equal("knowing", c.EndingKey);
00026:         }
00027:
00028:         [Fact]
00029:         public void Build_SortsSlidesByOrder()
00030:         {
00031:             var b = new EpilogueChronicleBuilder();
00032:             var c = b.Build(new EpilogueChronicleInput
00033:             {
00034:                 EndingKey = "knowing",
00035:                 Day = 100,
00036:                 BuildSeed = 1,
00037:                 Slides = new List<EpilogueSlide>
00038:                 {
00039:                     new EpilogueSlide(2, "Second", "..."),
00040:                     new EpilogueSlide(0, "First", "..."),
00041:                     new EpilogueSlide(1, "Middle", "...")
00042:                 },
00043:                 FateCards = new List<SurvivorFateCard>(),
00044:                 Metrics = new List<EpilogueMetric>()
00045:             });
00046:             Assert.Equal(3, c.Slides.Count);
00047:             Assert.Equal("First", c.Slides[0].Title);
00048:             Assert.Equal("Middle", c.Slides[1].Title);
00049:             Assert.Equal("Second", c.Slides[2].Title);
00050:         }
00051:
00052:         [Fact]
00053:         public void Build_SortsFateCardsBySurvivorId()
00054:         {
00055:             var b = new EpilogueChronicleBuilder();
00056:             var c = b.Build(new EpilogueChronicleInput
00057:             {
00058:                 EndingKey = "knowing",
00059:                 Day = 100,
00060:                 BuildSeed = 1,
00061:                 Slides = new List<EpilogueSlide>(),
00062:                 FateCards = new List<SurvivorFateCard>
00063:                 {
00064:                     new SurvivorFateCard { SurvivorId = "zulu", DisplayName = "Zulu", Fate = "Survived", Survived = true },
00065:                     new SurvivorFateCard { SurvivorId = "alpha", DisplayName = "Alpha", Fate = "Died", Survived = false }
00066:                 },
00067:                 Metrics = new List<EpilogueMetric>()
00068:             });
00069:             Assert.Equal("alpha", c.FateCards[0].SurvivorId);
00070:             Assert.Equal("zulu", c.FateCards[1].SurvivorId);
00071:         }
00072:
00073:         [Fact]
00074:         public void Build_SortsMetricsByMetricId()
00075:         {
00076:             var b = new EpilogueChronicleBuilder();
00077:             var c = b.Build(new EpilogueChronicleInput
00078:             {
00079:                 EndingKey = "knowing",
00080:                 Day = 100,
00081:                 BuildSeed = 1,
00082:                 Slides = new List<EpilogueSlide>(),
00083:                 FateCards = new List<SurvivorFateCard>(),
00084:                 Metrics = new List<EpilogueMetric>
00085:                 {
00086:                     new EpilogueMetric("total_deaths", 5, "Deaths"),
00087:                     new EpilogueMetric("days_survived", 365, "Days"),
00088:                     new EpilogueMetric("morale_final", 75, "Morale")
00089:                 }
00090:             });
00091:             Assert.Equal("days_survived", c.Metrics[0].MetricId);
00092:             Assert.Equal("morale_final", c.Metrics[1].MetricId);
00093:             Assert.Equal("total_deaths", c.Metrics[2].MetricId);
00094:         }
00095:
00096:         [Fact]
00097:         public void Build_DeterministicForSameInput()
00098:         {
00099:             var b = new EpilogueChronicleBuilder();
00100:             var input = new EpilogueChronicleInput
00101:             {
00102:                 EndingKey = "culpable",
00103:                 Day = 211,
00104:                 BuildSeed = 99,
00105:                 Slides = new List<EpilogueSlide>
00106:                 {
00107:                     new EpilogueSlide(1, "Slide A", "Prose A"),
00108:                     new EpilogueSlide(0, "Slide B", "Prose B")
00109:                 },
00110:                 FateCards = new List<SurvivorFateCard>
00111:                 {
00112:                     new SurvivorFateCard { SurvivorId = "s2", DisplayName = "S2" },
00113:                     new SurvivorFateCard { SurvivorId = "s1", DisplayName = "S1" }
00114:                 },
00115:                 Metrics = new List<EpilogueMetric>
00116:                 {
00117:                     new EpilogueMetric("m1", 1f, "M1")
00118:                 }
00119:             };
00120:             var c1 = b.Build(input);
00121:             var c2 = b.Build(input);
00122:             Assert.Equal(c1.Title, c2.Title);
00123:             Assert.Equal(c1.Slides[0].Title, c2.Slides[0].Title);
00124:             Assert.Equal(c1.FateCards[0].SurvivorId, c2.FateCards[0].SurvivorId);
00125:             Assert.Equal(c1.Metrics[0].MetricId, c2.Metrics[0].MetricId);
00126:         }
00127:
00128:         [Fact]
00129:         public void Build_HandlesEmptyInput()
00130:         {
00131:             var b = new EpilogueChronicleBuilder();
00132:             var c = b.Build(new EpilogueChronicleInput { EndingKey = "" });
00133:             Assert.Equal("UNKNOWN ENDING", c.Title);
00134:             Assert.Empty(c.Slides);
00135:             Assert.Empty(c.FateCards);
00136:             Assert.Empty(c.Metrics);
00137:         }
00138:
00139:         [Fact]
00140:         public void Build_UnknownEndingFallsBackToKey()
00141:         {
00142:             var b = new EpilogueChronicleBuilder();
00143:             var c = b.Build(new EpilogueChronicleInput { EndingKey = "novel_ending" });
00144:             Assert.Equal("novel_ending", c.Title);
00145:         }
00146:     }
00147: }
```


# Appendix — Focused Evidence Detail: `Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs`

### `Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs` — complete current file

- Size: 250 lines / 11653 bytes.
- SHA-256: `5d83bff7932ba9f38979191888fb0df4091c7646976a396dd2507dfbd2776db6`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Endgame;
00008: using Ashfall.Core.IO;
00009: using Ashfall.Core.Quests;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests.Narrative
00013: {
00014:     /// <summary>
00015:     /// Wave 40 Batch 8 Cross-System Integration Test:
00016:     /// Validates Plan 96 (Epilogue Chronicle Expansion - 20 ending presentation slides)
00017:     /// alongside Plan 104 (Narrative Questlines Expansion - 12 survivor-specific personal arcs).
00018:     /// </summary>
00019:     public sealed class Plan96_104ChronicleQuestIntegrationTests : CatalogTestBase
00020:     {
00021:         private static readonly (int order, string title, string artToken)[] BaselineFiveSlides =
00022:         {
00023:             (0, "Opening", "epilogue_opening_placeholder"),
00024:             (2, "The Bunker", "epilogue_bunker_placeholder"),
00025:             (5, "Survivors", "epilogue_survivors_placeholder"),
00026:             (17, "What Remains", "epilogue_remains_placeholder"),
00027:             (19, "Final Word", "epilogue_final_placeholder")
00028:         };
00029:
00030:         [Fact]
00031:         public void Plan96_EpilogueChronicle_LoadsAllTwentySlides_WithStrictContiguityAndPlaceholderConventions()
00032:         {
00033:             var files = new FileSystemIO();
00034:             var json = new SystemTextJsonSerializer();
00035:
00036:             var catalogData = EpilogueChronicleLoader.Load(DataDirectory, files, json);
00037:             Assert.NotNull(catalogData);
00038:             Assert.Equal(1, catalogData!.schema_version);
00039:             Assert.NotNull(catalogData.default_slides);
00040:             Assert.Equal(20, catalogData.default_slides.Count);
00041:
00042:             var slides = EpilogueChronicleLoader.LoadDefaultSlides(DataDirectory, files, json);
00043:             Assert.NotNull(slides);
00044:             Assert.Equal(20, slides.Count);
00045:
00046:             var seenOrders = new HashSet<int>();
00047:             for (int i = 0; i < 20; i++)
00048:             {
00049:                 var slide = slides[i];
00050:                 Assert.NotNull(slide);
00051:                 Assert.Equal(i, slide.Order);
00052:                 Assert.True(seenOrders.Add(slide.Order), $"Duplicate slide order: {slide.Order}");
00053:                 Assert.False(string.IsNullOrWhiteSpace(slide.Title), $"Slide order {i} has empty title");
00054:                 Assert.False(string.IsNullOrWhiteSpace(slide.ArtAssetId), $"Slide order {i} has empty art_asset_id");
00055:                 Assert.StartsWith("epilogue_", slide.ArtAssetId, StringComparison.Ordinal);
00056:                 Assert.EndsWith("_placeholder", slide.ArtAssetId, StringComparison.Ordinal);
00057:             }
00058:
00059:             // Verify the 5 baseline slides remain preserved byte-for-byte in their designated sequence slots
00060:             foreach (var (expectedOrder, expectedTitle, expectedToken) in BaselineFiveSlides)
00061:             {
00062:                 var slide = slides.FirstOrDefault(s => s.Order == expectedOrder);
00063:                 Assert.NotNull(slide);
00064:                 Assert.Equal(expectedTitle, slide.Title);
00065:                 Assert.Equal(expectedToken, slide.ArtAssetId);
00066:             }
00067:         }
00068:
00069:         [Fact]
00070:         public void Plan104_NarrativeQuestlines_LoadsAllTwelveArcs_WithValidFourStageChainsAndBinaryCrisis()
00071:         {
00072:             var files = new FileSystemIO();
00073:             var json = new SystemTextJsonSerializer();
00074:
00075:             var questlines = NarrativeQuestlineCatalogLoader.LoadEntries(DataDirectory, files, json);
00076:             Assert.NotNull(questlines);
00077:             Assert.Equal(12, questlines.Count);
00078:
00079:             var seenQuestIds = new HashSet<string>(StringComparer.Ordinal);
00080:             var seenSurvivorIds = new HashSet<string>(StringComparer.Ordinal);
00081:
00082:             foreach (var arc in questlines)
00083:             {
00084:                 Assert.NotNull(arc);
00085:                 Assert.StartsWith("quest_", arc.questId, StringComparison.Ordinal);
00086:                 Assert.True(seenQuestIds.Add(arc.questId), $"Duplicate questId detected: {arc.questId}");
00087:
00088:                 Assert.False(string.IsNullOrWhiteSpace(arc.survivorId), $"Quest '{arc.questId}' missing survivorId");
00089:                 Assert.True(seenSurvivorIds.Add(arc.survivorId), $"Multiple questlines bound to survivorId: {arc.survivorId}");
00090:
00091:                 Assert.False(string.IsNullOrWhiteSpace(arc.title), $"Quest '{arc.questId}' missing title");
00092:                 Assert.StartsWith("loc_", arc.targetLocationId, StringComparison.Ordinal);
00093:
00094:                 Assert.NotNull(arc.stages);
00095:                 Assert.Equal(4, arc.stages.Count);
00096:
00097:                 // Stage 0: Discovery
00098:                 var stage0 = arc.FindStage(0);
00099:                 Assert.NotNull(stage0);
00100:                 Assert.Equal("Discovery", stage0!.name);
00101:                 Assert.False(string.IsNullOrWhiteSpace(stage0.description));
00102:                 Assert.NotNull(stage0.objectiveItems);
00103:                 Assert.NotEmpty(stage0.objectiveItems);
00104:
00105:                 // Stage 1: Investigation
00106:                 var stage1 = arc.FindStage(1);
00107:                 Assert.NotNull(stage1);
00108:                 Assert.Equal("Investigation", stage1!.name);
00109:                 Assert.False(string.IsNullOrWhiteSpace(stage1.description));
00110:
00111:                 // Stage 2: Crisis (Binary Branch)
00112:                 var stage2 = arc.FindStage(2);
00113:                 Assert.NotNull(stage2);
00114:                 Assert.Equal("Crisis", stage2!.name);
00115:                 Assert.True(stage2.HasBranch, $"Quest '{arc.questId}' stage 2 must carry binary crisis branch");
00116:                 Assert.NotNull(stage2.branchA);
00117:                 Assert.NotNull(stage2.branchB);
00118:                 Assert.NotEqual(stage2.branchA!.id, stage2.branchB!.id);
00119:
00120:                 Assert.False(string.IsNullOrWhiteSpace(stage2.branchA.label));
00121:                 Assert.False(string.IsNullOrWhiteSpace(stage2.branchA.description));
00122:                 Assert.False(string.IsNullOrWhiteSpace(stage2.branchA.traitGranted));
00123:
00124:                 Assert.False(string.IsNullOrWhiteSpace(stage2.branchB.label));
00125:                 Assert.False(string.IsNullOrWhiteSpace(stage2.branchB.description));
00126:                 Assert.False(string.IsNullOrWhiteSpace(stage2.branchB.traitGranted));
00127:
00128:                 // Stage 3: Resolution
00129:                 var stage3 = arc.FindStage(3);
00130:                 Assert.NotNull(stage3);
00131:                 Assert.Equal("Resolution", stage3!.name);
00132:                 Assert.False(string.IsNullOrWhiteSpace(stage3.description));
00133:             }
00134:         }
00135:
00136:         [Fact]
00137:         public void CrossSystem_SurvivorArcResolutionAndEndgameChronicle_ExhibitNarrativeCoherence()
00138:         {
00139:             var files = new FileSystemIO();
00140:             var json = new SystemTextJsonSerializer();
00141:
00142:             // 1. Load both catalogs
00143:             var questlines = NarrativeQuestlineCatalogLoader.LoadEntries(DataDirectory, files, json);
00144:             var slides = EpilogueChronicleLoader.LoadDefaultSlides(DataDirectory, files, json);
00145:             Assert.Equal(12, questlines.Count);
00146:             Assert.Equal(20, slides.Count);
00147:
00148:             // 2. Initialize NarrativeQuestlineSystem and advance a representative arc (Marcus Olejnik - Machinist)
00149:             var questSystem = new NarrativeQuestlineSystem(questlines);
00150:             string survivorId = "marcus_olejnik";
00151:             var arcDef = questSystem.GetDefinitionForSurvivor(survivorId);
00152:             Assert.NotNull(arcDef);
00153:             Assert.Equal("quest_the_machinists_regret", arcDef!.questId);
00154:
00155:             // Start arc
00156:             bool started = questSystem.TryBegin(survivorId, day: 10);
00157:             Assert.True(started);
00158:             var arcState = questSystem.GetArc(survivorId);
00159:             Assert.NotNull(arcState);
00160:             Assert.Equal(NarrativeArcStatus.Active, arcState!.status);
00161:             Assert.Equal(0, arcState.currentStage);
00162:
00163:             // Deliver Stage 0 objective item: blueprint_roll
00164:             bool delivered0 = questSystem.TryDeliverItem(survivorId, "blueprint_roll", day: 11);
00165:             Assert.True(delivered0, "Delivering blueprint_roll must advance stage 0");
00166:             Assert.Equal(1, arcState.currentStage);
00167:
00168:             // Deliver Stage 1 objective items: scrap_metal and soldering_kit
00169:             bool delivered1a = questSystem.TryDeliverItem(survivorId, "scrap_metal", day: 12);
00170:             Assert.True(delivered1a);
00171:             Assert.Equal(1, arcState.currentStage);
00172:             bool delivered1b = questSystem.TryDeliverItem(survivorId, "soldering_kit", day: 13);
00173:             Assert.True(delivered1b);
00174:             Assert.Equal(2, arcState.currentStage);
00175:             Assert.Equal(NarrativeArcStatus.AwaitingBranch, arcState.status);
00176:
00177:             // Choose branch A (Scrupulous Machinist: dismantle_weapon)
00178:             bool branchChosen = questSystem.TryChooseBranch(survivorId, "dismantle_weapon", day: 25, out var chosenBranch);
00179:             Assert.True(branchChosen);
00180:             Assert.NotNull(chosenBranch);
00181:             Assert.Equal("trait_scrupulous_machinist", chosenBranch!.traitGranted);
00182:             Assert.Equal(NarrativeArcStatus.Resolved, arcState.status);
00183:             Assert.Equal(3, arcState.currentStage);
00184:
00185:             // 3. Construct Endgame Chronicle reflecting the resolved survivor state
00186:             var fateCards = new List<SurvivorFateCard>
00187:             {
00188:                 new SurvivorFateCard
00189:                 {
00190:                     SurvivorId = survivorId,
00191:                     DisplayName = "Marcus Olejnik",
00192:                     Fate = $"Marcus dismantled the siege weapon, earning the trait {chosenBranch.traitGranted}.",
00193:                     Survived = true
00194:                 }
00195:             };
00196:
00197:             var metrics = new List<EpilogueMetric>
00198:             {
00199:                 new EpilogueMetric("survivors_living", 12f, "Living Dwellers"),
00200:                 new EpilogueMetric("arcs_resolved", 1f, "Survivor Arcs Completed"),
00201:                 new EpilogueMetric("days_survived", 365f, "Days Survived")
00202:             };
00203:
00204:             var builder = new EpilogueChronicleBuilder();
00205:             var chronicle = builder.Build(new EpilogueChronicleInput
00206:             {
00207:                 EndingKey = "knowing",
00208:                 Day = 365,
00209:                 BuildSeed = 42,
00210:                 Slides = slides,
00211:                 FateCards = fateCards,
00212:                 Metrics = metrics
00213:             });
00214:
00215:             Assert.NotNull(chronicle);
00216:             Assert.Equal("Knowing", chronicle.Title);
00217:             Assert.Equal(20, chronicle.Slides.Count);
00218:             Assert.Single(chronicle.FateCards);
00219:             Assert.Equal(survivorId, chronicle.FateCards[0].SurvivorId);
00220:             Assert.Equal(3, chronicle.Metrics.Count);
00221:
00222:             // Verify slide ordering and narrative milestones
00223:             Assert.Equal("Opening", chronicle.Slides[0].Title);
00224:             Assert.Equal("The Bunker", chronicle.Slides[2].Title);
00225:             Assert.Equal("Survivors", chronicle.Slides[5].Title);
00226:             Assert.Equal("Empty Bunks", chronicle.Slides[6].Title);
00227:             Assert.Equal("The Resolution", chronicle.Slides[15].Title);
00228:             Assert.Equal("Final Word", chronicle.Slides[19].Title);
00229:         }
00230:
00231:         [Fact]
00232:         public void CrossSystem_DeterministicExecution_UnderRepeatedReloadsPasses()
00233:         {
00234:             var files = new FileSystemIO();
00235:             var json = new SystemTextJsonSerializer();
00236:
00237:             for (int i = 0; i < 50; i++)
00238:             {
00239:                 var slides = EpilogueChronicleLoader.LoadDefaultSlides(DataDirectory, files, json);
00240:                 Assert.Equal(20, slides.Count);
00241:
00242:                 var questlines = NarrativeQuestlineCatalogLoader.LoadEntries(DataDirectory, files, json);
00243:                 Assert.Equal(12, questlines.Count);
00244:
00245:                 var arc = questlines[0];
00246:                 Assert.Equal(4, arc.stages.Count);
00247:             }
00248:         }
00249:     }
00250: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **The subject is the boundary between a resolved campaign ending and a deterministic, truthful epilogue presentation. The plan expands the evidence and integration route around that boundary; it does not grant the slide catalog authority over campaign outcomes.**.

The first polish also checks continuity against the master authority: the plan is one bounded outcome, uses an existing owner seam, names a data authority, and does not widen into unrelated economy, UI, save or content work. Any apparently attractive addition that lacks a current owner is recorded as out of scope rather than smuggled into the architecture.

# Appendix — Polishing Pass 2: Integration and Code Architecture

This pass turns the evidence into an executable route. It distinguishes Core domain rules, host composition, Godot presentation, current save envelopes, deterministic streams, typed events and focused tests. The route is deliberately extend-first. A future builder may add a field, catalog row, read model or host adapter only after claiming the exact path and proving that the existing owner can accept it.

The second pass also reviews the handoff from data to player experience. A row that cannot be reached from a command is an orphan; a command that updates a shadow field is a split authority; a panel that recomputes a result is a presentation bug; a save that restores a display but not the owner is a persistence bug. Each failure is given a focused negative obligation.

# Appendix — Final Precision and Reaccuracy Pass

Before handoff, re-read every current path, hash, catalog count, public declaration, test declaration and save owner named above. Correct stale terminology, remove fictional type names, replace old section pins with current owner names, and downgrade any unsupported pass claim to historical evidence. Re-run the structural verifier after this pass. The final artifact should let a builder execute the first safe step without reinterpreting ownership.

**Precision result:** current implementation claims are separated from future proposals; content is not counted as reachability; Core remains engine-free; UI remains a projection; save and determinism are explicit; and every residual gap has a named verification route. If a future source audit contradicts this record, the source wins and the plan returns `STALE_PLAN` for re-audit.

# Appendix — Quality Assurance Pass Record

This record is part of the planning artifact, not a fresh runtime test result.

## Pass A — premise and content
- Current catalog rows, source owners, historical closeout and remaining residual are separated.
- The old baseline count is not presented as the current count.
- No copied, real-world, fabricated or unowned content is proposed.

## Pass B — integration architecture
- Data → loader → Core owner → host command → UI/event → save → replay is named.
- Existing save sections and codecs are identified; no parallel section is invented.
- Deterministic ordering, no-RNG cases, seeded streams and legacy defaults are explicit.

## Pass C — precision and handoff
- Every referenced current path is hash-pinned in the evidence appendices.
- Proposed future seams are labeled as proposals and excluded from current claims.
- Focused test commands, failure responses, rollback and non-goals are included.
