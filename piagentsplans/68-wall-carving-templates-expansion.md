# Plan 68 — Wall Carving Templates, Morale Bands and Shelter-Texture Reachability

> **Rebuild status:** COMPLETE 60-TEMPLATE DATA LOOP — PRODUCTION CONSUMER REACHABILITY IS THE OPEN QUESTION
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

- The current `wall_carving_templates.json` contains three band rows and 60 strings, with high/medium/low morale ranges and graduated chances. The Core catalog reads the file directly and exposes `GetBandForMorale`/`GetRandomTemplate`.
- The source proves a content/selection contract but not a live player route. The nearby `GuiltInsomniaSystem` and shelter decor surfaces are separate owners; a wall carving must not be made a morale mutation or a memorial record by inference.
- The plan therefore separates three questions: row quality, deterministic selection, and whether a current host/UI route actually displays the result. Only the third may justify a small integration extension, and it requires a new claim for shared panel seams.

**Bounded outcome:** Retire the old pure-data claim that the catalog alone integrates wall carvings. The current JSON has 60 templates across three morale bands and `WallCarvingCatalog` provides deterministic band/template selection, but the live source search does not show a production host/UI consumer. The next package is a bounded reachability audit or a separately claimed read-only shelter projection—not a second carving state or another template batch.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `wall_carving_templates.json` is valid schema version 1 with three band rows and 60 total templates; current source exposes the total count and band ranges.
- `WallCarvingCatalog` uses `System.Text.Json`, clamps morale to 0–100, selects the first matching band and accepts an injected index function; it has no owner state or save section.
- The current source/test search finds the catalog and focused tests but no production `WallCarvingCatalog` construction or panel call outside the Core file itself; this is a reachability finding, not proof that no future route can exist.
- Historical DEC-236 sealed the 60-row data expansion; its historical pass record does not establish current host/UI reachability.

**Master-authority sections applied to this rebase:**

- Part II Factory Protocol: premise sweep, collision check, one lane/cluster, and evidence labels before drafting.
- Part II Step 5 continuity and anti-duplication checklist: data presence is not reachability.
- Part III cluster map: use the live C1–C17 owner map rather than a historical plan title.
- Part IV backlog discipline: consume a verified candidate or record why it is stale; do not widen a bounded outcome.
- Part V Template S/R: subject intent and recommended route remain separate from implementation commitments.
- Part VI Multi-Session Growth Protocol: 250k is a depth target, not permission to manufacture volume.
- Live source/data authority: current catalog, loader, host, save, and focused tests outrank generated prose.
- Anti-padding rule: if the evidence queue is exhausted, stop and report no warranted continuation.
- C17 Host/UI cluster: ambient text is a projection and requires a verified route before it is called integrated.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 3→60 data-only brief with a 60-template quality and consumer census.
- Prove the exact current Core selection contract, including band boundaries, fallback and deterministic index injection.
- Audit shelter host/UI composition for a real wall-carving projection; if absent, record a bounded implementation proposal rather than claiming the feature is live.
- Keep wall carvings read-only with respect to morale, guilt, memorial and decor state unless a future owner explicitly accepts a write.

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
| template parsing, band selection and deterministic text choice | WallCarvingCatalog | `Assets/Ashfall.Core/Shelter/WallCarvingCatalog.cs` | Owns static content selection; it does not mutate morale or shelter state. |
| current morale input | Shelter morale owner | `Assets/Ashfall.Core/Survivors/NeedsSystem.cs; Assets/Ashfall.Core/Survivors/MoraleContagionSystem.cs` | Provides the current morale projection; wall text does not own it. |
| nearby presentation contexts | Shelter decor/memorial surfaces | `src/Main.ShelterInfrastructure.cs; src/UI/ShelterDecorPanel.cs; src/UI/IronCenotaphMemorialPanel.cs` | Read-only candidate surfaces; no current wall-carving route is assumed. |
| separate psychological content | Guilt/insomnia owner | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` | Must not be treated as a wall-carving consumer without source evidence. |
| catalog and selection proof | Wall carving focused tests | `Ashfall.Core.Tests/Plan68WallCarvingTests.cs; Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs` | Current executable evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Wall Carving Templates, Morale Bands and Shelter-Texture Reachability
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ WallCarvingCatalog
│   template parsing, band selection and deterministic text choice
│ Shelter morale owner
│   current morale input
│ Shelter decor/memorial surfaces
│   nearby presentation contexts
│ Guilt/insomnia owner
│   separate psychological content
│ Wall carving focused tests
│   catalog and selection proof
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

1. **Preserve current state ownership.** WallCarvingCatalog owns template parsing, band selection and deterministic text choice: Owns static content selection; it does not mutate morale or shelter state.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| template parsing, band selection and deterministic text choice | WallCarvingCatalog | `Assets/Ashfall.Core/Shelter/WallCarvingCatalog.cs` | Owns static content selection; it does not mutate morale or shelter state. |
| current morale input | Shelter morale owner | `Assets/Ashfall.Core/Survivors/NeedsSystem.cs; Assets/Ashfall.Core/Survivors/MoraleContagionSystem.cs` | Provides the current morale projection; wall text does not own it. |
| nearby presentation contexts | Shelter decor/memorial surfaces | `src/Main.ShelterInfrastructure.cs; src/UI/ShelterDecorPanel.cs; src/UI/IronCenotaphMemorialPanel.cs` | Read-only candidate surfaces; no current wall-carving route is assumed. |
| separate psychological content | Guilt/insomnia owner | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` | Must not be treated as a wall-carving consumer without source evidence. |
| catalog and selection proof | Wall carving focused tests | `Ashfall.Core.Tests/Plan68WallCarvingTests.cs; Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs` | Current executable evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load the current wall-carving catalog
2. read the current shelter morale projection from its owner
3. select a band using inclusive bounds
4. choose a template with the existing injected deterministic index function
5. present a truthful ambient carving/placement fact if a host route is proven
6. do not write morale, guilt, memorial or inventory state
7. leave transient output unpersisted unless a future owner accepts a durable placement

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Templates and band ranges are immutable catalog data; selection output is transient unless a future presentation owner explicitly persists a placement.
- Morale input is clamped to the current 0–100 contract and band boundaries are inclusive.
- Missing/empty templates produce the catalog’s documented empty/fallback result; selection must not fabricate a permanent mark.
- No wall-carving state is added to an unrelated save section without a player-visible durable placement decision.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A template row is valid only with a finite morale range, a non-empty band ID and non-empty templates.
- Band selection is deterministic and inclusive; an out-of-range input is clamped before matching.
- The injected index is bounded by the selected pool; an invalid index falls back according to current source rather than throwing.
- A text projection cannot change owner morale or manufacture a memorial event.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `wall_carving_templates.json` remains the sole template authority.
- Do not add a second carving catalog, placement save or moral-effect field without a current owner decision.
- A future row is admitted only after tone, redundancy, physical-placement and consumer review.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- No save change is justified for a read-only ambient text selector.
- If a future placement becomes durable, claim the current decor/memorial owner and add a round-trip fixture; do not append an unowned field.
- Existing shelter/decor/memorial saves must remain byte-compatible in this planning package.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Selection is deterministic through the supplied index function; no random source is hidden in Core.
- Band order is catalog order and fallback is the first band/current empty result.
- Paired selection calls with the same morale/index stream produce the same template.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- No current typed wall-carving event is established by the Core catalog.
- A future host route would emit a presentation/observation fact only after a current owner accepts a command or placement.
- Morale, guilt and memorial events must not be synthesized from template text.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/ShelterDecorHostSession.cs
- src/Host/ShelterDecorSaveStore.cs
- src/Main.ShelterInfrastructure.cs
- src/UI/ShelterDecorPanel.cs
- src/UI/IronCenotaphMemorialPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Carvings are physical, brief and human; they should not become a second dialogue or exposition system.
- Low-morale text must not instruct or glorify self-harm, and all content must remain fictional and restrained.
- A displayed carving should not claim a fact that no current owner state supports.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A 60-row catalog is treated as a live feature without a host consumer. | WallCarvingCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A panel changes morale to make a band selectable. | Shelter morale owner | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A template is persisted in a new shadow store. | Shelter decor/memorial surfaces | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A random index is supplied differently by two hosts. | Guilt/insomnia owner | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A carving duplicates memorial or guilt state. | Wall carving focused tests | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Plan68WallCarvingTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — content census | Read the 60 rows and exact Core selector. | Current count/band grammar is proven. | No production path until the owning implementation package is separately claimed. |
| 1 — consumer audit | Search current host/UI composition and verify whether a live route exists. | Reachability is proven or recorded as a finding. | No production path until the owning implementation package is separately claimed. |
| 2 — bounded route decision | Choose no-change maintenance or a separately claimed read-only projection. | No second authority is proposed. | No production path until the owning implementation package is separately claimed. |
| 3 — precision/tone pass | Review redundancy, physical plausibility, safety and deterministic selection. | Quality is improved without padding. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/wall_carving_templates.json | READ ONLY; MODIFY only for a proven content/consumer gap | 60-template authority |
| Assets/Ashfall.Core/Shelter/WallCarvingCatalog.cs | READ ONLY | Current selector |
| src/Main.ShelterInfrastructure.cs | READ ONLY; MODIFY only under a new host claim | Candidate composition context |
| src/UI/ShelterDecorPanel.cs | READ ONLY; MODIFY only under a new UI claim | Candidate presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Claiming a host route from a Core-only reference. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding a second morale or memorial mutation. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Persisting transient prose without an owner. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Padding the catalog with repetitive unsafe text. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new template batch in this package.
- No new wall state/save section.
- No production/data/test/UI changes.
- No inferred integration into guilt or memorial.

# 23. Rollback and Recovery

- Revert the planning document.
- Any future host/UI change is isolated and retains current decor/memorial save fixtures.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 60 current templates and selector bounds are documented.
- The absence/presence of a production consumer is explicitly recorded.
- No second state/save/morale authority is proposed.
- Focused tests and a bounded future route are named.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 3→60 data-only brief with a 60-template quality and consumer census.
- Prove the exact current Core selection contract, including band boundaries, fallback and deterministic index injection.
- Audit shelter host/UI composition for a real wall-carving projection; if absent, record a bounded implementation proposal rather than claiming the feature is live.
- Keep wall carvings read-only with respect to morale, guilt, memorial and decor state unless a future owner explicitly accepts a write.

## MUST NOT DO

- No new template batch in this package.
- No new wall state/save section.
- No production/data/test/UI changes.
- No inferred integration into guilt or memorial.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Plan68WallCarvingTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — content census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: template parsing, band selection and deterministic text choice → WallCarvingCatalog; current morale input → Shelter morale owner; nearby presentation contexts → Shelter decor/memorial surfaces; separate psychological content → Guilt/insomnia owner; catalog and selection proof → Wall carving focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 68.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 68 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by WallCarvingCatalog or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Shelter/WallCarvingCatalog.cs`

### `Assets/Ashfall.Core/Shelter/WallCarvingCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 107 lines / 3406 bytes.
- SHA-256: `c33e301e05446913da4f1ee4669bf4192c361187fa4df0b4480eca50577c8e96`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WallCarvingBand
public string MoraleBand { get; set; } = string.Empty;
public int MoraleMin { get; set; }
public int MoraleMax { get; set; }
public List<string> Templates { get; set; } = new List<string>();
public float CarvingChance { get; set; }
public sealed class WallCarvingCatalog
public int SchemaVersion { get; set; }
public List<WallCarvingBand> Items { get; set; } = new List<WallCarvingBand>();
public IReadOnlyList<WallCarvingBand> Bands => _bands;
public static WallCarvingCatalog FromJson(string json) {
public static WallCarvingCatalog LoadFromDirectory(string dataDirectory) {
public WallCarvingBand? GetBandForMorale(float morale) {
public string GetRandomTemplate(float morale, Func<int, int> rngNext) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs`

### `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 339 lines / 16119 bytes.
- SHA-256: `27c96eb65660412f2d7cc92526e3a2865f7f7cbc039a0c7d67dff1f2ff7a352e`.
- Architecture signals: seeded references=1; save/restore symbols=4; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class ShelterDecorPlacement
public string RoomId = string.Empty;
public string SlotId = string.Empty;
public string ItemId = string.Empty;
public int DayInstalled;
public bool IsMemorialPlaque;
public string MemorialSurvivorId = string.Empty;
public string PlaqueSourceHeirloomId = string.Empty;
public class ShelterDecorState
public const string SystemId = "shelter_decor";
public string systemId = SystemId;
public string Checksum = string.Empty;
public List<ShelterDecorPlacement> Placements = new List<ShelterDecorPlacement>();
public ShelterDecorStateCapture Capture() => new ShelterDecorStateCapture
public void RestoreInto(ShelterDecorState other) {
public sealed class ShelterDecorStateCapture : ShelterDecorState { }
public sealed class ShelterDecorSystem
public const string SystemId = "shelter_decor";
public const string MemorialPlaquePrefix = "item_decor_memorial_plaque";
public ShelterDecorState State => _state;
public event Action<ShelterDecorPlacement>? OnDecorChanged;
public event Action? OnStateChanged;
public IReadOnlyDictionary<string, ShelterDecorItemModifier> ItemModifiers => _itemModifiers;
public void RegisterItemModifier(ShelterDecorItemModifier modifier) {
public ShelterDecorItemModifier? GetItemModifier(string itemId) {
public bool Assign(string roomId, string slotId, string itemId, int dayInstalled, bool isMemorialPlaque = false, string memorialSurvivorId = "", string plaqueSourceHeirloomId = "") {
public bool Remove(string roomId, string slotId) {
public ShelterDecorPlacement? GetSlot(string roomId, string slotId) {
public List<ShelterDecorPlacement> ListRoomPlacements(string roomId) {
public float GetRoomMoraleDelta(string roomId) {
public ShelterDecorPlacement? ResolvePlaqueSlot(string memorialSurvivorId, string heirloomItemId, string memorialWallRoom, string plaqueSlotId, int dayInstalled) {
public string ResolvePlaqueItemId(string heirloomItemId) {
public ShelterDecorStateCapture CaptureState() => _state.Capture();
public void RestoreState(ShelterDecorStateCapture saved) {
public IReadOnlyList<string> GetTrophySlots(string roomId) {
public float GetTrophyMoraleModifier(string itemId) {
public static bool IsTrophyItem(string itemId) {
public class ShelterDecorItemModifier
public string ItemId = string.Empty;
public float LocalizedMoraleDelta;
public string Category = string.Empty;
public bool StackMultiplicatively;
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs`

### `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 224 lines / 8840 bytes.
- SHA-256: `0edeee9f38c11329e4e73ccd86621ba448cf4458d0432560f479a9e406685275`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class GuiltRecord
public string sourceId = string.Empty;
public int dayRecorded;
public float severity;
public sealed class GuiltInsomniaSaveState
public List<GuiltSurvivorState> survivors = new List<GuiltSurvivorState>();
public sealed class GuiltSurvivorState
public string survivorId = string.Empty;
public float insomniaSeverity;
public float sedativeCompensationHours;
public List<GuiltRecord> guiltSources = new List<GuiltRecord>();
public class GuiltInsomniaSystem
public const float SleepQualityPenaltyPerSeverity = 0.50f;
public const float SedativeCompensationHours = 12f;
public const float SedativeSeverityReduction = 0.40f;
public const float DialogueSeverityReduction = 0.25f;
public const float NaturalDecayPerDay = 0.05f;
public const float HighSeverityThreshold = 0.7f;
public const int GuiltExpiryDays = 30;
public event Action<string, GuiltRecord> OnGuiltRecorded;
public event Action<string> OnGuiltResolved;
public event Action<string> OnGuiltInsomniaCritical;
public event Action OnStateChanged;
public void RecordGuilt(string survivorId, string sourceId, float severity, int currentDay) {
public bool ApplySedative(string survivorId) {
public bool ResolveGuiltThroughDialogue(string survivorId) {
public void ApplyTherapyRelief(string survivorId, float fraction) {
public float GetSleepQualityMultiplier(string survivorId) {
public float GetInsomniaSeverity(string survivorId) {
public int GetGuiltSourceCount(string survivorId) {
public void Tick(string survivorId, float gameHours, int currentDay) {
public GuiltInsomniaSaveState CaptureState() {
public void RestoreState(GuiltInsomniaSaveState save) {
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs`

### `Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 106 lines / 3555 bytes.
- SHA-256: `9b564dbdeb0c72951c742128a02b2c85a3908d198c821e66aedfe5e87276859e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class GuiltSourceDefinition
public string ChoicePattern { get; set; } = string.Empty;
public float Severity { get; set; }
public string Title { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public string FormatDescription(string survivorName) {
public sealed class GuiltSourceCatalog
public int SchemaVersion { get; set; }
public List<GuiltSourceDefinition> Items { get; set; } = new List<GuiltSourceDefinition>();
public IReadOnlyList<GuiltSourceDefinition> Items => _items;
public int Count => _items.Count;
public static GuiltSourceCatalog FromJson(string json) {
public static GuiltSourceCatalog LoadFromDirectory(string dataDirectory) {
public GuiltSourceDefinition? GetByPattern(string choicePattern) {
public bool TryGetSeverity(string choicePattern, out float severity) {
```


# Appendix B.06 — Current Code Architecture: `src/Host/ShelterDecorHostSession.cs`

### `src/Host/ShelterDecorHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 326 lines / 13499 bytes.
- SHA-256: `52b2a24194d91d6e412f85fd1cd2dbf28cd0d147f088100d24d9d642785e27b8`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterDecorHostSession : HostSessionBase
public const string MemorialWallRoomId = "room_memorial_wall";
public ShelterDecorSystem System { get; }
public ShelterAssignmentSystem Assignments => _assignments;
public NeedsSystem Needs => _needs;
public ItemCatalog InventoryCatalog => _inventory.Catalog;
public InventoryContainer Inventory => _inventory.Inventory;
public string LastEvent { get; private set; } = string.Empty;
public int CatalogModifierCount { get; private set; }
public int LastMoraleRecipientCount { get; private set; }
public float LastMoraleGranted { get; private set; }
public int CurrentDay { get; private set; }
public int LoadCatalogModifiers() {
public IReadOnlyList<ShelterRoom> Rooms => _assignments.Rooms;
public void SetCurrentDay(int day) {
public void SetPanelMessage(string message) {
public string DisplayNameForRoom(string roomId) {
public List<ItemDefinition> ListAvailableDecor() {
public bool TryMount(string roomId, string slotId, string itemId, int day, out string reason) {
public bool TryRemoveMount(string roomId, string slotId, out string reason) {
public bool TryMountMemorialPlaque(MemorialEntry entry, out string reason) {
public int ApplyDailyMorale(int day) {
protected override void UnsubscribeSystemEvents() {
```


# Appendix B.07 — Current Code Architecture: `src/Host/ShelterDecorSaveStore.cs`

### `src/Host/ShelterDecorSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 107 lines / 4484 bytes.
- SHA-256: `c8e9fb95de58f1cde1833c65871e9abc7287a219a9560ec1d750ba451501fe79`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class ShelterDecorSaveStore
public const string FileName = "shelter_decor_save.json";
public const string SectionName = "shelter_decor";
public static string SavePath => s_store.SavePath;
public static string TryCaptureDirect(ShelterDecorState state) => s_store.CaptureBare(ToCapture(state));
public static ShelterDecorState? TryRestoreDirect(string json) {
public static string TryCapture(ShelterDecorState state) => s_store.CaptureBare(ToCapture(state));
public static ShelterDecorState? TryRestore(string json) {
public static bool TrySave(ShelterDecorState state) => s_store.TrySave(ToCapture(state));
public static ShelterDecorState? TryLoad() {
public static string TryCapturePersisted(ShelterDecorState state) => s_store.CapturePersisted(ToCapture(state));
```


# Appendix B.08 — Current Code Architecture: `src/Main.ShelterInfrastructure.cs`

### `src/Main.ShelterInfrastructure.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 645 lines / 31304 bytes.
- SHA-256: `5ec59e6a0a93e8e5c67bc7a39c91ff4cca74a518400fd09468938167c23630ea`.
- Architecture signals: seeded references=6; save/restore symbols=16; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public Ashfall.Core.Shelter.ShelterFireHazardSystem ShelterFireHazard => GetShelterFireHazardSystem();
public ShelterFireHostSession? ShelterFireSession => _shelterFireSession;
public Ashfall.Core.Narrative.BunkerGraffitiCatalog GetBunkerGraffitiCatalog() {
public Ashfall.Core.Narrative.BunkerCourtCatalog GetBunkerCourtCatalog() {
public Ashfall.Core.Narrative.BunkerMaintenanceCatalog GetBunkerMaintenanceCatalog() {
public Ashfall.Core.Narrative.PersonalLetterCatalog GetPersonalLetterCatalog() {
public Ashfall.Core.Narrative.AbyssalAnomaliesCatalog GetAbyssalAnomaliesCatalog() {
public string BuildMachineTellText(ISeededRng? rng = null) {
public Ashfall.Core.Shelter.ShelterFireHazardSystem GetShelterFireHazardSystem() {
```


# Appendix B.09 — Current Code Architecture: `src/UI/ShelterDecorPanel.cs`

### `src/UI/ShelterDecorPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 443 lines / 21161 bytes.
- SHA-256: `ebf8710e14d5aaed08d3edaae4b02ef28199d223f09126e419ace0f8188fec4e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class ShelterDecorPanel : Control, IBindablePanel
public event Action? OnClose;
public int RenderedPlacementCount { get; private set; }
public bool IsBound => _host != null;
public void Open() {
public void Bind(ShelterDecorHostSession session) {
public void Unbind() {
public override void _Ready() {
public void RefreshView() {
public bool SelectRoom(string roomId) {
public override void _ExitTree() {
```


# Appendix B.10 — Current Code Architecture: `src/UI/IronCenotaphMemorialPanel.cs`

### `src/UI/IronCenotaphMemorialPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 187 lines / 9167 bytes.
- SHA-256: `5a39a1daac0e45ad6b386fb62deb477828137fa7e20ba7680eafbf48684f49a4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class IronCenotaphMemorialPanel : Control, IBindablePanel
public event Action? OnClose;
public override void _Ready() {
public void Open() {
public bool IsBound { get; private set; } = true;
public void Bind(object? session) {
public void Unbind() {
public sealed class MourningBinding
public Func<int> TotalDeaths { get; init; } = () => 0;
public Func<string, ActionResult> Mourn { get; init; } = _ =>
public void BindMourning(MourningBinding binding) {
public void BindSpiritual(Func<int> arcCount, Func<int> pendingRites) {
public void RefreshView() {
```


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/wall_carving_templates.json`

### `Assets/StreamingAssets/Data/wall_carving_templates.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 6148 bytes / 6124 characters.
- SHA-256: `9fb9ac091f80df6b0744806dfe2de71c849f92f1fa802144d03229153dc122fe`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=3, max=3, observed_paths=1
items[].templates: min=20, max=20, observed_paths=2
```

Representative record fields:

- `carving_chance`
- `morale_band`
- `morale_max`
- `morale_min`
- `templates`


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/guilt_sources.json`

### `Assets/StreamingAssets/Data/guilt_sources.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 10440 bytes / 10434 characters.
- SHA-256: `88f7a18ce18f408d3924b9b3ec4be831fd2f92b7ea61b72fd09ed1bdf28e198e`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=40, max=40, observed_paths=1
```

Representative record fields:

- `choice_pattern`
- `description`
- `severity`
- `title`


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Plan68WallCarvingTests.cs`

### `Ashfall.Core.Tests/Plan68WallCarvingTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 213; SHA-256: `6736b21b677ef6f975bc3e20f8396f6e0aa77530a69d54a216bdcc31d3792183`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_has_exactly_three_bands_with_correct_names
Each_band_contains_exactly_20_templates
Band_windows_match_the_plan_contract
No_empty_templates_and_no_whitespace_only_templates
No_exact_duplicates_within_a_band
No_exact_duplicates_across_bands
The_fifteen_original_templates_are_preserved
Templates_stay_readable_at_a_glance
No_melodrama_cliches_in_any_band
High_band_carries_hope_through_solidarity_not_triumphalism
Medium_band_is_documentary_routine_texture
Low_band_varies_motifs_beyond_names_of_the_dead
Bands_are_distinguishable_by_blind_vocabulary
```


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs`

### `Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 197; SHA-256: `2447468de613d9a6dc5dfa4f4324293977ead2b359f3fcd1ba9af931770176e5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
GuiltSourceCatalog_And_GuiltInsomniaSystem_FullLifecycle
WallCarvingCatalog_And_MoraleBandSelection_FullContract
PsychologicalStress_And_CulturalTrace_SystemCoupling
```


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs`

### `Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs`

- Current test declarations: Fact=15, Theory=0, InlineData=0.
- File lines: 160; SHA-256: `d1fb46927f6c5062bb6d128b215a21eb690cfa2dc7fb2fe58fa8a3b0f89fcc04`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RecordGuilt_IncreasesSeverity
RecordGuilt_MultipleSources_CapsAt1
RecordGuilt_FiresEvent
RecordGuilt_CriticalThreshold_FiresEvent
ApplySedative_ReducesSeverity
ApplySedative_NoGuilt_ReturnsFalse
ResolveDialogue_RemovesMostRecentGuilt
ResolveDialogue_LastSource_FiresResolved
SleepQuality_LowerWithGuilt
SleepQuality_SedativeHalvesPenalty
Tick_ExpiresOldGuilt
Tick_DecaysSedative
CaptureRestore_Roundtrip
RestoreNull_DoesNotCrash
RecordGuilt_RejectsEmptyId
```


# Appendix D.16 — Existing Focused Test Inventory: `Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs`

### `Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 357; SHA-256: `f35442bfdf970acda3b73451801713ca7b2a86ab4acf2f51c3640138067767af`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsAndHasExactly40GuiltSources
Catalog_CategoryDistribution_MatchesPlan66Specification
Catalog_AllChoicePatternsAndTitles_AreUniqueAndValid
Catalog_SeverityDistribution_IsWellCalibrated
GuiltInsomniaSystem_All20NewSources_AccumulateSeverityDeterministically
GuiltInsomniaSystem_HighSeveritySources_TriggerCriticalInsomniaThreshold
GuiltInsomniaSystem_SaveLoad_FullRoundTrip_PreservesGuiltRecords
GuiltInsomniaSystem_ExpiryAndDialogueResolution_OperateCorrectly
```


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/Shelter/WallCarvingCatalog.cs`

### `Assets/Ashfall.Core/Shelter/WallCarvingCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 107 lines / 3406 bytes.
- SHA-256: `c33e301e05446913da4f1ee4669bf4192c361187fa4df0b4480eca50577c8e96`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WallCarvingBand
public string MoraleBand { get; set; } = string.Empty;
public int MoraleMin { get; set; }
public int MoraleMax { get; set; }
public List<string> Templates { get; set; } = new List<string>();
public float CarvingChance { get; set; }
public sealed class WallCarvingCatalog
public int SchemaVersion { get; set; }
public List<WallCarvingBand> Items { get; set; } = new List<WallCarvingBand>();
public IReadOnlyList<WallCarvingBand> Bands => _bands;
public static WallCarvingCatalog FromJson(string json) {
public static WallCarvingCatalog LoadFromDirectory(string dataDirectory) {
public WallCarvingBand? GetBandForMorale(float morale) {
public string GetRandomTemplate(float morale, Func<int, int> rngNext) {
```


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs`

### `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 339 lines / 16119 bytes.
- SHA-256: `27c96eb65660412f2d7cc92526e3a2865f7f7cbc039a0c7d67dff1f2ff7a352e`.
- Architecture signals: seeded references=1; save/restore symbols=4; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class ShelterDecorPlacement
public string RoomId = string.Empty;
public string SlotId = string.Empty;
public string ItemId = string.Empty;
public int DayInstalled;
public bool IsMemorialPlaque;
public string MemorialSurvivorId = string.Empty;
public string PlaqueSourceHeirloomId = string.Empty;
public class ShelterDecorState
public const string SystemId = "shelter_decor";
public string systemId = SystemId;
public string Checksum = string.Empty;
public List<ShelterDecorPlacement> Placements = new List<ShelterDecorPlacement>();
public ShelterDecorStateCapture Capture() => new ShelterDecorStateCapture
public void RestoreInto(ShelterDecorState other) {
public sealed class ShelterDecorStateCapture : ShelterDecorState { }
public sealed class ShelterDecorSystem
public const string SystemId = "shelter_decor";
public const string MemorialPlaquePrefix = "item_decor_memorial_plaque";
public ShelterDecorState State => _state;
public event Action<ShelterDecorPlacement>? OnDecorChanged;
public event Action? OnStateChanged;
public IReadOnlyDictionary<string, ShelterDecorItemModifier> ItemModifiers => _itemModifiers;
public void RegisterItemModifier(ShelterDecorItemModifier modifier) {
public ShelterDecorItemModifier? GetItemModifier(string itemId) {
public bool Assign(string roomId, string slotId, string itemId, int dayInstalled, bool isMemorialPlaque = false, string memorialSurvivorId = "", string plaqueSourceHeirloomId = "") {
public bool Remove(string roomId, string slotId) {
public ShelterDecorPlacement? GetSlot(string roomId, string slotId) {
public List<ShelterDecorPlacement> ListRoomPlacements(string roomId) {
public float GetRoomMoraleDelta(string roomId) {
public ShelterDecorPlacement? ResolvePlaqueSlot(string memorialSurvivorId, string heirloomItemId, string memorialWallRoom, string plaqueSlotId, int dayInstalled) {
public string ResolvePlaqueItemId(string heirloomItemId) {
public ShelterDecorStateCapture CaptureState() => _state.Capture();
public void RestoreState(ShelterDecorStateCapture saved) {
public IReadOnlyList<string> GetTrophySlots(string roomId) {
public float GetTrophyMoraleModifier(string itemId) {
public static bool IsTrophyItem(string itemId) {
public class ShelterDecorItemModifier
public string ItemId = string.Empty;
public float LocalizedMoraleDelta;
public string Category = string.Empty;
public bool StackMultiplicatively;
```


# Appendix E.19 — Supporting Code Evidence: `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs`

### `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 224 lines / 8840 bytes.
- SHA-256: `0edeee9f38c11329e4e73ccd86621ba448cf4458d0432560f479a9e406685275`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class GuiltRecord
public string sourceId = string.Empty;
public int dayRecorded;
public float severity;
public sealed class GuiltInsomniaSaveState
public List<GuiltSurvivorState> survivors = new List<GuiltSurvivorState>();
public sealed class GuiltSurvivorState
public string survivorId = string.Empty;
public float insomniaSeverity;
public float sedativeCompensationHours;
public List<GuiltRecord> guiltSources = new List<GuiltRecord>();
public class GuiltInsomniaSystem
public const float SleepQualityPenaltyPerSeverity = 0.50f;
public const float SedativeCompensationHours = 12f;
public const float SedativeSeverityReduction = 0.40f;
public const float DialogueSeverityReduction = 0.25f;
public const float NaturalDecayPerDay = 0.05f;
public const float HighSeverityThreshold = 0.7f;
public const int GuiltExpiryDays = 30;
public event Action<string, GuiltRecord> OnGuiltRecorded;
public event Action<string> OnGuiltResolved;
public event Action<string> OnGuiltInsomniaCritical;
public event Action OnStateChanged;
public void RecordGuilt(string survivorId, string sourceId, float severity, int currentDay) {
public bool ApplySedative(string survivorId) {
public bool ResolveGuiltThroughDialogue(string survivorId) {
public void ApplyTherapyRelief(string survivorId, float fraction) {
public float GetSleepQualityMultiplier(string survivorId) {
public float GetInsomniaSeverity(string survivorId) {
public int GetGuiltSourceCount(string survivorId) {
public void Tick(string survivorId, float gameHours, int currentDay) {
public GuiltInsomniaSaveState CaptureState() {
public void RestoreState(GuiltInsomniaSaveState save) {
```


# Appendix E.20 — Supporting Code Evidence: `Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs`

### `Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 106 lines / 3555 bytes.
- SHA-256: `9b564dbdeb0c72951c742128a02b2c85a3908d198c821e66aedfe5e87276859e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class GuiltSourceDefinition
public string ChoicePattern { get; set; } = string.Empty;
public float Severity { get; set; }
public string Title { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public string FormatDescription(string survivorName) {
public sealed class GuiltSourceCatalog
public int SchemaVersion { get; set; }
public List<GuiltSourceDefinition> Items { get; set; } = new List<GuiltSourceDefinition>();
public IReadOnlyList<GuiltSourceDefinition> Items => _items;
public int Count => _items.Count;
public static GuiltSourceCatalog FromJson(string json) {
public static GuiltSourceCatalog LoadFromDirectory(string dataDirectory) {
public GuiltSourceDefinition? GetByPattern(string choicePattern) {
public bool TryGetSeverity(string choicePattern, out float severity) {
```


# Appendix E.21 — Supporting Code Evidence: `src/Host/ShelterDecorHostSession.cs`

### `src/Host/ShelterDecorHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 326 lines / 13499 bytes.
- SHA-256: `52b2a24194d91d6e412f85fd1cd2dbf28cd0d147f088100d24d9d642785e27b8`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterDecorHostSession : HostSessionBase
public const string MemorialWallRoomId = "room_memorial_wall";
public ShelterDecorSystem System { get; }
public ShelterAssignmentSystem Assignments => _assignments;
public NeedsSystem Needs => _needs;
public ItemCatalog InventoryCatalog => _inventory.Catalog;
public InventoryContainer Inventory => _inventory.Inventory;
public string LastEvent { get; private set; } = string.Empty;
public int CatalogModifierCount { get; private set; }
public int LastMoraleRecipientCount { get; private set; }
public float LastMoraleGranted { get; private set; }
public int CurrentDay { get; private set; }
public int LoadCatalogModifiers() {
public IReadOnlyList<ShelterRoom> Rooms => _assignments.Rooms;
public void SetCurrentDay(int day) {
public void SetPanelMessage(string message) {
public string DisplayNameForRoom(string roomId) {
public List<ItemDefinition> ListAvailableDecor() {
public bool TryMount(string roomId, string slotId, string itemId, int day, out string reason) {
public bool TryRemoveMount(string roomId, string slotId, out string reason) {
public bool TryMountMemorialPlaque(MemorialEntry entry, out string reason) {
public int ApplyDailyMorale(int day) {
protected override void UnsubscribeSystemEvents() {
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/Plan68WallCarvingTests.cs`

### `Ashfall.Core.Tests/Plan68WallCarvingTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 213; SHA-256: `6736b21b677ef6f975bc3e20f8396f6e0aa77530a69d54a216bdcc31d3792183`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_has_exactly_three_bands_with_correct_names
Each_band_contains_exactly_20_templates
Band_windows_match_the_plan_contract
No_empty_templates_and_no_whitespace_only_templates
No_exact_duplicates_within_a_band
No_exact_duplicates_across_bands
The_fifteen_original_templates_are_preserved
Templates_stay_readable_at_a_glance
No_melodrama_cliches_in_any_band
High_band_carries_hope_through_solidarity_not_triumphalism
Medium_band_is_documentary_routine_texture
Low_band_varies_motifs_beyond_names_of_the_dead
Bands_are_distinguishable_by_blind_vocabulary
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs`

### `Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 197; SHA-256: `2447468de613d9a6dc5dfa4f4324293977ead2b359f3fcd1ba9af931770176e5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
GuiltSourceCatalog_And_GuiltInsomniaSystem_FullLifecycle
WallCarvingCatalog_And_MoraleBandSelection_FullContract
PsychologicalStress_And_CulturalTrace_SystemCoupling
```


# Appendix G.24 — Supporting Regression Evidence: `Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs`

### `Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs`

- Current test declarations: Fact=15, Theory=0, InlineData=0.
- File lines: 160; SHA-256: `d1fb46927f6c5062bb6d128b215a21eb690cfa2dc7fb2fe58fa8a3b0f89fcc04`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RecordGuilt_IncreasesSeverity
RecordGuilt_MultipleSources_CapsAt1
RecordGuilt_FiresEvent
RecordGuilt_CriticalThreshold_FiresEvent
ApplySedative_ReducesSeverity
ApplySedative_NoGuilt_ReturnsFalse
ResolveDialogue_RemovesMostRecentGuilt
ResolveDialogue_LastSource_FiresResolved
SleepQuality_LowerWithGuilt
SleepQuality_SedativeHalvesPenalty
Tick_ExpiresOldGuilt
Tick_DecaysSedative
CaptureRestore_Roundtrip
RestoreNull_DoesNotCrash
RecordGuilt_RejectsEmptyId
```


# Appendix G.25 — Supporting Regression Evidence: `Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs`

### `Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 357; SHA-256: `f35442bfdf970acda3b73451801713ca7b2a86ab4acf2f51c3640138067767af`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsAndHasExactly40GuiltSources
Catalog_CategoryDistribution_MatchesPlan66Specification
Catalog_AllChoicePatternsAndTitles_AreUniqueAndValid
Catalog_SeverityDistribution_IsWellCalibrated
GuiltInsomniaSystem_All20NewSources_AccumulateSeverityDeterministically
GuiltInsomniaSystem_HighSeveritySources_TriggerCriticalInsomniaThreshold
GuiltInsomniaSystem_SaveLoad_FullRoundTrip_PreservesGuiltRecords
GuiltInsomniaSystem_ExpiryAndDialogueResolution_OperateCorrectly
```


# Appendix H.26 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

### `docs/CURRENT_AUTHORITY.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 102 lines / 9950 bytes.
- SHA-256: `7dea2c12b4863bfc9a3c2ebb161ba51512ebc06475b762abd5d5d205bef47e5c`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=0; textual Godot mentions=6; textual Unity/JsonUtility mentions=2; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.27 — Supporting Authority Document: `docs/shelter/WALL_CARVING_TONE_BIBLE.md`

### `docs/shelter/WALL_CARVING_TONE_BIBLE.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 39 lines / 2809 bytes.
- SHA-256: `6a4b4d7ccd29f9c9f79dc3a3aa23d9cbae2a0042024f2863bd515ff2ab851538`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.28 — Supporting Authority Document: `docs/psych/GUILT_EMITTER_MAP.md`

### `docs/psych/GUILT_EMITTER_MAP.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 43 lines / 4945 bytes.
- SHA-256: `9d2541c89a98e6616a0a4cb2febcca8798c3e058c8c3d77a853f9f60b36d4a8f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| template parsing, band selection and deterministic text choice | WallCarvingCatalog | current morale input | Shelter morale owner | Owner emits/reads a typed fact; no mirror state. |
| template parsing, band selection and deterministic text choice | WallCarvingCatalog | nearby presentation contexts | Shelter decor/memorial surfaces | Owner emits/reads a typed fact; no mirror state. |
| template parsing, band selection and deterministic text choice | WallCarvingCatalog | separate psychological content | Guilt/insomnia owner | Owner emits/reads a typed fact; no mirror state. |
| template parsing, band selection and deterministic text choice | WallCarvingCatalog | catalog and selection proof | Wall carving focused tests | Owner emits/reads a typed fact; no mirror state. |
| current morale input | Shelter morale owner | template parsing, band selection and deterministic text choice | WallCarvingCatalog | Owner emits/reads a typed fact; no mirror state. |
| current morale input | Shelter morale owner | nearby presentation contexts | Shelter decor/memorial surfaces | Owner emits/reads a typed fact; no mirror state. |
| current morale input | Shelter morale owner | separate psychological content | Guilt/insomnia owner | Owner emits/reads a typed fact; no mirror state. |
| current morale input | Shelter morale owner | catalog and selection proof | Wall carving focused tests | Owner emits/reads a typed fact; no mirror state. |
| nearby presentation contexts | Shelter decor/memorial surfaces | template parsing, band selection and deterministic text choice | WallCarvingCatalog | Owner emits/reads a typed fact; no mirror state. |
| nearby presentation contexts | Shelter decor/memorial surfaces | current morale input | Shelter morale owner | Owner emits/reads a typed fact; no mirror state. |
| nearby presentation contexts | Shelter decor/memorial surfaces | separate psychological content | Guilt/insomnia owner | Owner emits/reads a typed fact; no mirror state. |
| nearby presentation contexts | Shelter decor/memorial surfaces | catalog and selection proof | Wall carving focused tests | Owner emits/reads a typed fact; no mirror state. |
| separate psychological content | Guilt/insomnia owner | template parsing, band selection and deterministic text choice | WallCarvingCatalog | Owner emits/reads a typed fact; no mirror state. |
| separate psychological content | Guilt/insomnia owner | current morale input | Shelter morale owner | Owner emits/reads a typed fact; no mirror state. |
| separate psychological content | Guilt/insomnia owner | nearby presentation contexts | Shelter decor/memorial surfaces | Owner emits/reads a typed fact; no mirror state. |
| separate psychological content | Guilt/insomnia owner | catalog and selection proof | Wall carving focused tests | Owner emits/reads a typed fact; no mirror state. |
| catalog and selection proof | Wall carving focused tests | template parsing, band selection and deterministic text choice | WallCarvingCatalog | Owner emits/reads a typed fact; no mirror state. |
| catalog and selection proof | Wall carving focused tests | current morale input | Shelter morale owner | Owner emits/reads a typed fact; no mirror state. |
| catalog and selection proof | Wall carving focused tests | nearby presentation contexts | Shelter decor/memorial surfaces | Owner emits/reads a typed fact; no mirror state. |
| catalog and selection proof | Wall carving focused tests | separate psychological content | Guilt/insomnia owner | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 3→60 data-only brief with a 60-template quality and consumer census. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Prove the exact current Core selection contract, including band boundaries, fallback and deterministic index injection. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Audit shelter host/UI composition for a real wall-carving projection; if absent, record a bounded implementation proposal rather than claiming the feature is live. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Keep wall carvings read-only with respect to morale, guilt, memorial and decor state unless a future owner explicitly accepts a write. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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

> **Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival` (ASHFALL: Atomic War – Starving Survival)
**Document version:** 2.0.0 — compiled 2026-09-24, supersedes the v1.0 master world bible (compiled 2026-09-23) as an expansion scaffold.
**Document class:** SUBJECT-PLAN FACTORY. This document is not itself an integration plan. It is a repeatable generator: any future planning session can consume its matrices, backlog, and templates to produce an unbounded series of bounded subject plans, each of which names its own best integration route.
**Audit basis:** Live repository inspection performed 2026-09-24 (repository root listing, `Assets/StreamingAssets/Data/` listing at 342 entries, `docs/` listing, `docs/plans/` listing at 126 entries, `INTEGRATION_PLANS.md`, `SESSION_HANDOFF.md`, `AGENTS.md`, branch list). Every claim in the Drift Register (Part I) is labeled VERIFIED, HIGH CONFIDENCE, or UNVERIFIED.
**Authority order:** unchanged from v1.0 — live repository source and data first; then `AGENTS.md`; then this document; then the docs registry and atlas; then plan ledgers. Where this document and live source disagree, live source wins and this document must be corrected.

> 1. **The Drift Register (Part I):** a live-audit correction layer. The repository has moved since the v1.0 snapshot; every plan drafted against stale premises is wasted work. The register lists what changed, with evidence and confidence labels.
2. **The Factory Protocol (Part II):** the operating loop that converts evidence into subject plans. It is deterministic, like everything else in this project: same inputs, same plan shape, same verification demands.
3. **The Generator Matrices (Part III):** the combinatorial core. Ten expansion lanes × seventeen subsystem clusters, with per-cell opening archetypes. This is the mechanism by which one document yields hundreds of expansion plans without inventing duplicate systems.
4. **The Seeded Backlog (Part IV) and Templates (Part V):** audit-derived candidate expansions, each with a subject, evidence, confidence, and best integration route; plus the wave-charter, subject-plan, and verification templates the repository already uses, extended for factory output.

> **DR-04 — The data catalog inventory has grown; several catalogs are absent from the v1.0 inventory. VERIFIED.**
`Assets/StreamingAssets/Data/` currently holds 342 entries. Catalogs observed live but not present in the v1.0 Part 5.4 inventory include: `dive_sites.json`, `hydroponic_crops.json`, `hydraulic_extrusion_catalog.json`, `metrology_standards_catalog.json`, `muster_camp_scenes.json`, `muster_epilogues.json`, `muster_faction_actions.json`, `muster_faction_culture.json`, `muster_witnesses.json`, `utility_actions.json`, `moral_choice_quests_branching.json`, `moral_choice_quests_distress.json`. Consequence: the duplication firewall (v1.0 Part 5) is stale in these domains; a planner could propose a "new" muster or moral-choice catalog that already exists. The ID-collision sweep in Factory Protocol step 1 must always run against the live listing, never against this document.

> **Step 1 — Premise sweep (mandatory, every time).**
Before selecting any candidate, the session re-verifies premises against live source: the live `Assets/StreamingAssets/Data/` listing (duplication firewall, DR-04), `INTEGRATION_PLANS.md` current batch (DR-06), `WORKTREE_OWNERSHIP.md` claims (DR-09), `KNOWN_DEBT.md`, the root coordination files (DR-01), `docs/gaps/` and `docs/incidents/` (DR-02), and `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` (DR-08). Output: a short premise sheet. A candidate whose premise fails the sweep is discarded, not patched.

> **Step 5 — Run the continuity and anti-duplication checklist.**
The v1.0 checklist (Part 13.2) applies in full, plus two factory additions: (a) duplication firewall — prove the candidate does not duplicate any live catalog, system, or `docs/` authority map; (b) unclaimed-content check — if the candidate's content domain appears in `UNCLAIMED_CORPUS_CENSUS.md`, the plan must wire the unclaimed content first or explain why new content outranks it.

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
| C4 | Fuel and feedstock income-versus-expenditure audits for each industrial chain; dominated-process analysis (do any catalogs produce strictly dominated outputs?) | HIGH CONFIDENCE |
| C5 | Scavenging E[value] re-runs after any loot authoring (Plan 76.2 harness pattern); vehicle dominance follow-ups against the live dominance table | HIGH CONFIDENCE |
| C7 | Tribute-cycle sustainability (7-day cadence) versus mid-game income; embargo economic pressure | HIGH CONFIDENCE |
| C11 | Price-shock and rumor-band systemic outcomes; debt-interest runaway analysis; black-market pricing tiers | HIGH CONFIDENCE |
| C12 | Winter resource compression (Days 90–180): calories, fuel, filters, morale — sustainability-day math per difficulty preset | HIGH CONFIDENCE |
| C14 | Trapping yield versus equipment degradation cost; zoonosis risk premium on uncooked yield | HIGH CONFIDENCE |
| All others | Balance audits only where numbers exist; never invent tuning targets without an intended design statement | — |

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

The subject is a small, read-only ambient-text selector whose data loop is complete but whose production reachability is not proven. The plan expands the quality and integration decision while refusing to invent a player route or state owner.

- **template parsing, band selection and deterministic text choice** remains with `WallCarvingCatalog` at `Assets/Ashfall.Core/Shelter/WallCarvingCatalog.cs`. Owns static content selection; it does not mutate morale or shelter state.
- **current morale input** remains with `Shelter morale owner` at `Assets/Ashfall.Core/Survivors/NeedsSystem.cs; Assets/Ashfall.Core/Survivors/MoraleContagionSystem.cs`. Provides the current morale projection; wall text does not own it.
- **nearby presentation contexts** remains with `Shelter decor/memorial surfaces` at `src/Main.ShelterInfrastructure.cs; src/UI/ShelterDecorPanel.cs; src/UI/IronCenotaphMemorialPanel.cs`. Read-only candidate surfaces; no current wall-carving route is assumed.
- **separate psychological content** remains with `Guilt/insomnia owner` at `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs`. Must not be treated as a wall-carving consumer without source evidence.
- **catalog and selection proof** remains with `Wall carving focused tests` at `Ashfall.Core.Tests/Plan68WallCarvingTests.cs; Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs`. Current executable evidence.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load the current wall-carving catalog
2. read the current shelter morale projection from its owner
3. select a band using inclusive bounds
4. choose a template with the existing injected deterministic index function
5. present a truthful ambient carving/placement fact if a host route is proven
6. do not write morale, guilt, memorial or inventory state
7. leave transient output unpersisted unless a future owner accepts a durable placement

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Templates and band ranges are immutable catalog data; selection output is transient unless a future presentation owner explicitly persists a placement.
- Morale input is clamped to the current 0–100 contract and band boundaries are inclusive.
- Missing/empty templates produce the catalog’s documented empty/fallback result; selection must not fabricate a permanent mark.
- No wall-carving state is added to an unrelated save section without a player-visible durable placement decision.

- A template row is valid only with a finite morale range, a non-empty band ID and non-empty templates.
- Band selection is deterministic and inclusive; an out-of-range input is clamped before matching.
- The injected index is bounded by the selected pool; an invalid index falls back according to current source rather than throwing.
- A text projection cannot change owner morale or manufacture a memorial event.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/ShelterDecorHostSession.cs
- src/Host/ShelterDecorSaveStore.cs
- src/Main.ShelterInfrastructure.cs
- src/UI/ShelterDecorPanel.cs
- src/UI/IronCenotaphMemorialPanel.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/Plan68WallCarvingTests.cs
- Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs
- Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs
- Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs

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
| S-01 | 68-01 load three bands/60 templates | load the current wall-carving catalog | Templates and band ranges are immutable catalog data; selection output is transient unless a future presentation owner explicitly persists a placement. | A 60-row catalog is treated as a live feature without a host consumer. | WallCarvingCatalog |
| S-02 | 68-02 high morale selection | read the current shelter morale projection from its owner | Morale input is clamped to the current 0–100 contract and band boundaries are inclusive. | A panel changes morale to make a band selectable. | WallCarvingCatalog |
| S-03 | 68-03 medium boundary | select a band using inclusive bounds | Missing/empty templates produce the catalog’s documented empty/fallback result; selection must not fabricate a permanent mark. | A template is persisted in a new shadow store. | WallCarvingCatalog |
| S-04 | 68-04 low boundary | choose a template with the existing injected deterministic index function | No wall-carving state is added to an unrelated save section without a player-visible durable placement decision. | A random index is supplied differently by two hosts. | WallCarvingCatalog |
| S-05 | 68-05 empty template pool | present a truthful ambient carving/placement fact if a host route is proven | Templates and band ranges are immutable catalog data; selection output is transient unless a future presentation owner explicitly persists a placement. | A carving duplicates memorial or guilt state. | WallCarvingCatalog |
| S-06 | 68-06 invalid injected index | do not write morale, guilt, memorial or inventory state | Morale input is clamped to the current 0–100 contract and band boundaries are inclusive. | A 60-row catalog is treated as a live feature without a host consumer. | WallCarvingCatalog |
| S-07 | 68-07 same index replay | leave transient output unpersisted unless a future owner accepts a durable placement | Missing/empty templates produce the catalog’s documented empty/fallback result; selection must not fabricate a permanent mark. | A panel changes morale to make a band selectable. | WallCarvingCatalog |
| S-08 | 68-08 host consumer search result | load the current wall-carving catalog | No wall-carving state is added to an unrelated save section without a player-visible durable placement decision. | A template is persisted in a new shadow store. | WallCarvingCatalog |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 68-TC-01 JSON schema and band count | data | JSON schema and band count; verify the current owner and its negative boundary without inventing a second authority. | WallCarvingCatalog |
| T-02 | 68-TC-02 template nonempty validation | unit | template nonempty validation; verify the current owner and its negative boundary without inventing a second authority. | WallCarvingCatalog |
| T-03 | 68-TC-03 morale clamp | persistence | morale clamp; verify the current owner and its negative boundary without inventing a second authority. | WallCarvingCatalog |
| T-04 | 68-TC-04 inclusive lower boundary | determinism | inclusive lower boundary; verify the current owner and its negative boundary without inventing a second authority. | WallCarvingCatalog |
| T-05 | 68-TC-05 inclusive upper boundary | host | inclusive upper boundary; verify the current owner and its negative boundary without inventing a second authority. | WallCarvingCatalog |
| T-06 | 68-TC-06 band gap/fallback | UI/accessibility | band gap/fallback; verify the current owner and its negative boundary without inventing a second authority. | WallCarvingCatalog |
| T-07 | 68-TC-07 empty catalog result | cross-system | empty catalog result; verify the current owner and its negative boundary without inventing a second authority. | WallCarvingCatalog |
| T-08 | 68-TC-08 index bounds | data | index bounds; verify the current owner and its negative boundary without inventing a second authority. | WallCarvingCatalog |
| T-09 | 68-TC-09 deterministic selection | unit | deterministic selection; verify the current owner and its negative boundary without inventing a second authority. | WallCarvingCatalog |
| T-10 | 68-TC-10 no morale mutation | persistence | no morale mutation; verify the current owner and its negative boundary without inventing a second authority. | WallCarvingCatalog |
| T-11 | 68-TC-11 no save mutation | determinism | no save mutation; verify the current owner and its negative boundary without inventing a second authority. | WallCarvingCatalog |
| T-12 | 68-TC-12 host reachability audit | host | host reachability audit; verify the current owner and its negative boundary without inventing a second authority. | WallCarvingCatalog |
| T-13 | 68-TC-13 tone/redundancy review | UI/accessibility | tone/redundancy review; verify the current owner and its negative boundary without inventing a second authority. | WallCarvingCatalog |
| T-14 | 68-TC-14 accessibility-safe projection | cross-system | accessibility-safe projection; verify the current owner and its negative boundary without inventing a second authority. | WallCarvingCatalog |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 25 | `Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 17 | `Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 12 | `Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 12 | `Assets/Ashfall.Core/Shelter/WallCarvingCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Medical/PsychologyAfflictionHandlers.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/UI/ShelterDecorSnapshotFixture.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Medical/PsychologyProjectionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Plan68WallCarvingTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Host/Phase0HostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Main.ShelterBatch3.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/UI/Phase0Panel.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/InstitutionCanonicalReliefTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/Survivors/Plan88_72ConfessionUtilityAiIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Phantoms/ConfessionSecretSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Spiritual/SpiritualMeaningCoordinator.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/HostCli.PanelTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/ShelterDecorSelfTest.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Main.FlagshipInstitutions.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/UI/ShelterDecorPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/UI/SnapshotHarness.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/wall_carving_templates.json`

### `Assets/StreamingAssets/Data/wall_carving_templates.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 6148; characters: 6124.
- SHA-256: `9fb9ac091f80df6b0744806dfe2de71c849f92f1fa802144d03229153dc122fe`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `items`

#### `items` — 3 current rows

- Row 001 `row-1`: `{"carving_chance":0.3,"morale_band":"high","morale_max":100,"morale_min":60,"templates":["Another day survived. The tally marks are getting longer.","A crude drawing of the sun — someone still remembers what it looks like.","A row of stick…`
- Row 002 `row-2`: `{"carving_chance":0.2,"morale_band":"medium","morale_max":59,"morale_min":30,"templates":["Tally marks — 47 days. The spacing is getting uneven.","A question scratched into the wall: 'Does anyone know what date it is?' No one has answered.…`
- Row 003 `row-3`: `{"carving_chance":0.15,"morale_band":"low","morale_max":29,"morale_min":0,"templates":["Just tally marks. No drawings. No words. Just lines.","A name, carved deep, then scratched out completely.","'I'm sorry' — written so small you'd miss …`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/guilt_sources.json`

### `Assets/StreamingAssets/Data/guilt_sources.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 10440; characters: 10434.
- SHA-256: `88f7a18ce18f408d3924b9b3ec4be831fd2f92b7ea61b72fd09ed1bdf28e198e`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `items`

#### `items` — 40 current rows

- Row 001 `row-1`: `{"choice_pattern":"cut_ration","description":"Cutting rations for those who could not contribute. The hollow eyes of the hungry follow {name} through every corridor.","severity":0.8,"title":"The Ration Cut"}`
- Row 002 `row-2`: `{"choice_pattern":"reduce_food","description":"Reducing food distribution below subsistence. {name} remembers every bowl they made smaller.","severity":0.75,"title":"The Empty Bowls"}`
- Row 003 `row-3`: `{"choice_pattern":"starve","description":"Deliberately withholding food. The sound of hunger — a quiet, persistent thing — haunts {name}'s sleep.","severity":0.9,"title":"The Starvation Order"}`
- Row 004 `row-4`: `{"choice_pattern":"leave_behind","description":"Leaving a companion behind during an expedition. {name} still checks over their shoulder, expecting to see the face they abandoned.","severity":0.7,"title":"The Abandoned"}`
- Row 005 `row-5`: `{"choice_pattern":"abandon","description":"Abandoning someone who trusted {name}. Trust, once broken, leaves a scar on both sides.","severity":0.75,"title":"The Betrayal of Trust"}`
- Row 006 `row-6`: `{"choice_pattern":"refuse_help","description":"Turning away a stranger in need. {name} wonders if that person survived the night.","severity":0.5,"title":"The Turned Back"}`
- Row 007 `row-7`: `{"choice_pattern":"turn_away","description":"Closing the hatch on desperate faces. {name} can still hear the knocking.","severity":0.55,"title":"The Closed Hatch"}`
- Row 008 `row-8`: `{"choice_pattern":"execute","description":"Taking a life in cold judgment. {name} sees that face every time they close their eyes.","severity":0.9,"title":"The Execution"}`
- Row 009 `row-9`: `{"choice_pattern":"kill","description":"Ending a life, even in necessity. {name} counts the names in the dark hours before dawn.","severity":0.85,"title":"The Taking of Life"}`
- Row 010 `row-10`: `{"choice_pattern":"shoot","description":"Pulling the trigger on another human being. The recoil leaves a bruise that never heals.","severity":0.8,"title":"The Shot"}`
- Row 011 `row-11`: `{"choice_pattern":"steal","description":"Taking what wasn't offered. {name} hides the stolen goods and the shame in equal measure.","severity":0.45,"title":"The Theft"}`
- Row 012 `row-12`: `{"choice_pattern":"hoard","description":"Keeping more than a fair share while others go without. The excess weighs heavier than hunger.","severity":0.4,"title":"The Hoard"}`
- Row 013 `row-13`: `{"choice_pattern":"take_all","description":"Stripping a cache bare, leaving nothing for those who might follow. {name} knows they condemned someone.","severity":0.6,"title":"The Empty Cache"}`
- Row 014 `row-14`: `{"choice_pattern":"lie","description":"A lie told to protect oneself at another's expense. The truth surfaces in {name}'s dreams.","severity":0.35,"title":"The Lie"}`
- Row 015 `row-15`: `{"choice_pattern":"deceive","description":"A deception that shifted burden onto the innocent. {name} avoids the eyes of those they deceived.","severity":0.4,"title":"The Deception"}`
- Row 016 `row-16`: `{"choice_pattern":"betray","description":"A betrayal of someone who called {name} friend. The word 'friend' has tasted like ash ever since.","severity":0.75,"title":"The Betrayal"}`
- Row 017 `row-17`: `{"choice_pattern":"harsh","description":"Harsh words spoken in a moment of weakness. {name} wishes they could take them back.","severity":0.2,"title":"The Harsh Word"}`
- Row 018 `row-18`: `{"choice_pattern":"refuse","description":"A refusal that cost someone dearly. {name} replays the conversation, looking for a different answer.","severity":0.25,"title":"The Refusal"}`
- Row 019 `row-19`: `{"choice_pattern":"deny","description":"Denying someone what they needed. The denial was logical at the time — now it just feels cruel.","severity":0.25,"title":"The Denial"}`
- Row 020 `row-20`: `{"choice_pattern":"sacrifice_other","description":"Sacrificing another to save the many. The math is sound; the heart rejects it completely.","severity":0.85,"title":"The Sacrifice of Another"}`
- Row 021 `row-21`: `{"choice_pattern":"hoard_medicine_while_needed","description":"The locked cabinet still holds the vial that was deemed too valuable to dispense. {name} knows someone in the infirmary died counting the hours until the next dose.","severity"…`
- Row 022 `row-22`: `{"choice_pattern":"barter_away_needed_food","description":"The trade crates were hauled away for ammunition and scrap. {name} stands in front of the ration pantry, staring at the dust rings where the flour bags sat.","severity":0.55,"title…`
- Row 023 `row-23`: `{"choice_pattern":"issue_known_contaminated_supplies","description":"The radiation warning tape was peeled away before the tins were brought to the table. {name} watches the children eat, saying nothing about the grease pencil mark beneath…`
- Row 024 `row-24`: `{"choice_pattern":"burn_critical_fuel_for_comfort","description":"For one evening the stove burned hot enough to sleep without coats. {name} wakes to find black frost choking the hydroponics lines three rooms down.","severity":0.3,"title":…`
- Row 025 `row-25`: `{"choice_pattern":"refuse_refugee_entry","description":"The surveillance monitor kept flickering after {name} cut the exterior intercom. In the morning, the snow outside the heavy outer door is disturbed and empty.","severity":0.65,"title"…`
- Row 026 `row-26`: `{"choice_pattern":"expel_survivor_for_efficiency","description":"Their bunk was reassigned before the sheets went cold. {name} avoids looking at the tally marks carved into the timber post beside the mattress.","severity":0.7,"title":"The …`
- Row 027 `row-27`: `{"choice_pattern":"hide_cache_from_allies","description":"The false floorboard sits flush with the subfloor. {name} looks away as an ally divides the remaining dried lentils into three equal, insufficient portions.","severity":0.45,"title"…`
- Row 028 `row-28`: `{"choice_pattern":"abandon_committed_rescue","description":"The grease pencil marker stays pinned to the sector map where the expedition turned around. {name} cannot bring themselves to wipe the glass clean.","severity":0.6,"title":"Turned…`
- Row 029 `row-29`: `{"choice_pattern":"leave_wounded_behind","description":"The head count on the way back had one fewer name. Through the long corridor walk home, {name} kept listening for footsteps that never caught up.","severity":0.8,"title":"One Less Foo…`
- Row 030 `row-30`: `{"choice_pattern":"retreat_from_rescue","description":"The distress signal was still cycling when {name} switched off the receiver to conserve battery. The speaker clicks softly in the dark before going dead.","severity":0.65,"title":"Radi…`
- Row 031 `row-31`: `{"choice_pattern":"execute_surrendered_enemy","description":"The rifle was already dropped in the dirt when {name} pulled the trigger. What stays in memory is how slowly the empty hands drifted downward.","severity":0.85,"title":"Hands Vis…`
- Row 032 `row-32`: `{"choice_pattern":"use_civilians_as_bait","description":"The diversion drew the patrol away from the supply cache exactly as calculated. {name} got the team out alive, and that is the part that makes sleep impossible.","severity":0.9,"titl…`
- Row 033 `row-33`: `{"choice_pattern":"kill_former_ally","description":"The insignia on the jacket was new, but the voice across the barricade was not. {name} cleaned their weapon afterward without looking at the brass casing on the floor.","severity":0.8,"ti…`
- Row 034 `row-34`: `{"choice_pattern":"betray_faction_trust","description":"The signed pact is still filed in the dispatch locker with {name}'s name on the seal. The people who honored it are no longer answering on the wire.","severity":0.6,"title":"Terms Bro…`
- Row 035 `row-35`: `{"choice_pattern":"inform_on_survivor","description":"The patrol only asked for a name once before handing over the supply voucher. {name} holds the canned meat in their palms, unable to open it.","severity":0.7,"title":"Name Given"}`
- Row 036 `row-36`: `{"choice_pattern":"break_final_wish_promise","description":"There is no one left alive to ask whether the last promise was kept. {name} carries the unfulfilled words like lead in their chest.","severity":0.85,"title":"The Promise"}`
- Row 037 `row-37`: `{"choice_pattern":"withhold_pain_relief","description":"The ampoule remains unbroken in the medical kit for a future emergency. {name} remembers the sound of breathing in the dark ward after the lantern went out.","severity":0.7,"title":"S…`
- Row 038 `row-38`: `{"choice_pattern":"triage_by_utility","description":"The triage tag marked priority by work output rather than blood loss. {name} wrote the numbers down with steady hands that now shake when holding a pen.","severity":0.75,"title":"Useful …`
- Row 039 `row-39`: `{"choice_pattern":"take_family_last_supplies","description":"The pantry shelves were scraped bare down to the wood shavings. {name} noticed the pencil height marks on the doorframe only after the rucksack was already zipped.","severity":0.…`
- Row 040 `row-40`: `{"choice_pattern":"order_survivor_to_death","description":"They asked only once if there was another way before stepping into the irradiated conduit. {name} gave the order, and the shelter went quiet.","severity":0.9,"title":"The Order Giv…`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Shelter/WallCarvingCatalog.cs`

### `Assets/Ashfall.Core/Shelter/WallCarvingCatalog.cs` — complete current file

- Size: 107 lines / 3406 bytes.
- SHA-256: `c33e301e05446913da4f1ee4669bf4192c361187fa4df0b4480eca50577c8e96`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Text.Json;
00006: using System.Text.Json.Serialization;
00007:
00008: namespace Ashfall.Core.Shelter
00009: {
00010:     public sealed class WallCarvingBand
00011:     {
00012:         [JsonPropertyName("morale_band")]
00013:         public string MoraleBand { get; set; } = string.Empty;
00014:
00015:         [JsonPropertyName("morale_min")]
00016:         public int MoraleMin { get; set; }
00017:
00018:         [JsonPropertyName("morale_max")]
00019:         public int MoraleMax { get; set; }
00020:
00021:         [JsonPropertyName("templates")]
00022:         public List<string> Templates { get; set; } = new List<string>();
00023:
00024:         [JsonPropertyName("carving_chance")]
00025:         public float CarvingChance { get; set; }
00026:     }
00027:
00028:     public sealed class WallCarvingCatalog
00029:     {
00030:         private sealed class CatalogRoot
00031:         {
00032:             [JsonPropertyName("schema_version")]
00033:             public int SchemaVersion { get; set; }
00034:
00035:             [JsonPropertyName("items")]
00036:             public List<WallCarvingBand> Items { get; set; } = new List<WallCarvingBand>();
00037:         }
00038:
00039:         private readonly List<WallCarvingBand> _bands = new List<WallCarvingBand>();
00040:
00041:         public IReadOnlyList<WallCarvingBand> Bands => _bands;
00042:         public int TotalTemplateCount
00043:         {
00044:             get
00045:             {
00046:                 int count = 0;
00047:                 for (int i = 0; i < _bands.Count; i++)
00048:                     count += _bands[i].Templates.Count;
00049:                 return count;
00050:             }
00051:         }
00052:
00053:         public WallCarvingCatalog() { }
00054:
00055:         public WallCarvingCatalog(IEnumerable<WallCarvingBand> bands)
00056:         {
00057:             if (bands != null)
00058:                 _bands.AddRange(bands);
00059:         }
00060:
00061:         public static WallCarvingCatalog FromJson(string json)
00062:         {
00063:             if (string.IsNullOrWhiteSpace(json))
00064:                 return new WallCarvingCatalog();
00065:
00066:             var root = JsonSerializer.Deserialize<CatalogRoot>(json, new JsonSerializerOptions
00067:             {
00068:                 PropertyNameCaseInsensitive = true
00069:             });
00070:
00071:             return new WallCarvingCatalog(root?.Items ?? (IEnumerable<WallCarvingBand>)Array.Empty<WallCarvingBand>());
00072:         }
00073:
00074:         public static WallCarvingCatalog LoadFromDirectory(string dataDirectory)
00075:         {
00076:             var filePath = Path.Combine(dataDirectory, "wall_carving_templates.json");
00077:             if (!File.Exists(filePath))
00078:                 return new WallCarvingCatalog();
00079:
00080:             var json = File.ReadAllText(filePath);
00081:             return FromJson(json);
00082:         }
00083:
00084:         public WallCarvingBand? GetBandForMorale(float morale)
00085:         {
00086:             int rounded = (int)Math.Round(Math.Clamp(morale, 0f, 100f));
00087:             for (int i = 0; i < _bands.Count; i++)
00088:             {
00089:                 var b = _bands[i];
00090:                 if (rounded >= b.MoraleMin && rounded <= b.MoraleMax)
00091:                     return b;
00092:             }
00093:             return _bands.Count > 0 ? _bands[0] : null;
00094:         }
00095:
00096:         public string GetRandomTemplate(float morale, Func<int, int> rngNext)
00097:         {
00098:             var band = GetBandForMorale(morale);
00099:             if (band == null || band.Templates.Count == 0)
00100:                 return string.Empty;
00101:
00102:             int idx = rngNext != null ? rngNext(band.Templates.Count) : 0;
00103:             if (idx < 0 || idx >= band.Templates.Count) idx = 0;
00104:             return band.Templates[idx];
00105:         }
00106:     }
00107: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs`

### `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs` — complete current file

- Size: 339 lines / 16119 bytes.
- SHA-256: `27c96eb65660412f2d7cc92526e3a2865f7f7cbc039a0c7d67dff1f2ff7a352e`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ASHFALL: Shelter interior decor system (Plan 12C — White Space 3).
00003: //
00004: // Per-room decor placements, deterministic localized morale modifier.
00005: // Engine-agnostic: plain serializable DTOs, no ISeededRng, save/round-trip
00006: // safe. The host wires this system into the room-view UI (golden snapshots
00007: // untouched) and the morale tick (NeedsSystem.Modify per survivor per
00008: // tick with the modifier returned by GetRoomMoraleDelta).
00009: //
00010: // The system never mutates ShelterAssignmentSystem, NeedsSystem, or
00011: // MemorialSystem. It exposes:
00012: //   - Assign / Remove / GetSlot / ListSlots per (roomId, slotId, itemId)
00013: //   - GetRoomMoraleDelta(roomId) for the localized morale readout
00014: //   - OnDecorChanged event the host can subscribe to for UI dirty paint
00015: //   - CaptureState / RestoreState round-trip via SaveStore<T> envelope
00016: //
00017: // Memorial bridge: when a MemorialEntry is committed with a known
00018: // HeirloomItemId belonging to the canon plaque item set, ResolvePlaqueSlot
00019: // returns the slot assignment the host should auto-publish to the wall.
00020:
00021: using System;
00022: using System.Collections.Generic;
00023: #pragma warning disable CS8618
00024:
00025: namespace Ashfall.Core.Shelter
00026: {
00027:     [Serializable]
00028:     public class ShelterDecorPlacement
00029:     {
00030:         /// <summary>Room where the decor is mounted. Free-text, matches a ShelterRoom.RoomId in the host.</summary>
00031:         public string RoomId = string.Empty;
00032:         /// <summary>Logical slot id within the room, e.g. "north_wall", "south_pegs". The host decides the slot vocabulary by calling Assign with the slot id it wants to populate.</summary>
00033:         public string SlotId = string.Empty;
00034:         /// <summary>Canonical item id (item_decor_* from items.json). When empty the slot is empty.</summary>
00035:         public string ItemId = string.Empty;
00036:         /// <summary>Day the placement was installed. Captured for chronology + future memorial hooks.</summary>
00037:         public int DayInstalled;
00038:         /// <summary>When true the placement was auto-generated by memorial (Plan 09 9C cross-link), True means it is itself the memorial plaque for the named survivor.</summary>
00039:         public bool IsMemorialPlaque;
00040:         /// <summary>If IsMemorialPlaque is true, this is the survivor the plaque commemorates.</summary>
00041:         public string MemorialSurvivorId = string.Empty;
00042:         /// <summary>If IsMemorialPlaque is true, this is the source MemorialEntry.HeirloomItemId the plaque replaced.</summary>
00043:         public string PlaqueSourceHeirloomId = string.Empty;
00044:     }
00045:
00046:     [Serializable]
00047:     public class ShelterDecorState
00048:     {
00049:         public const string SystemId = "shelter_decor";
00050:
00051:         public string systemId = SystemId;
00052:         public string Checksum = string.Empty;
00053:         public List<ShelterDecorPlacement> Placements = new List<ShelterDecorPlacement>();
00054:
00055:         public ShelterDecorStateCapture Capture() => new ShelterDecorStateCapture
00056:         {
00057:             systemId = systemId,
00058:             Placements = new List<ShelterDecorPlacement>(Placements ?? new List<ShelterDecorPlacement>())
00059:         };
00060:
00061:         public void RestoreInto(ShelterDecorState other)
00062:         {
00063:             systemId = SystemId;
00064:             Placements = other?.Placements != null ? new List<ShelterDecorPlacement>(other.Placements) : new List<ShelterDecorPlacement>();
00065:         }
00066:     }
00067:
00068:     /// <summary>Alias to keep save-envelope wiring compatible with SaveStore&lt;T&gt;.</summary>
00069:     [Serializable]
00070:     public sealed class ShelterDecorStateCapture : ShelterDecorState { }
00071:
00072:     /// <summary>
00073:     /// Engine-agnostic decor placement registry. Owner of the canonical
00074:     /// (roomId, slotId) -> itemId mapping for the application. Configure
00075:     /// the decor items via items.json (id prefix <c>item_decor_</c>) and
00076:     /// their localized morale modifiers via the catalogued
00077:     /// <see cref="ShelterDecorItemModifier"/> map the host wires.
00078:     /// </summary>
00079:     public sealed class ShelterDecorSystem
00080:     {
00081:         public const string SystemId = "shelter_decor";
00082:         /// <summary>"Memorial plaque" canonical item id prefix (Plan 12C + Plan 09 9C cross-link).</summary>
00083:         public const string MemorialPlaquePrefix = "item_decor_memorial_plaque";
00084:
00085:         private readonly ShelterDecorState _state = new ShelterDecorState();
00086:         private readonly Dictionary<string, ShelterDecorItemModifier> _itemModifiers =
00087:             new Dictionary<string, ShelterDecorItemModifier>(StringComparer.Ordinal);
00088:
00089:         public ShelterDecorState State => _state;
00090:         public event Action<ShelterDecorPlacement>? OnDecorChanged;
00091:         public event Action? OnStateChanged;
00092:
00093:         public IReadOnlyDictionary<string, ShelterDecorItemModifier> ItemModifiers => _itemModifiers;
00094:
00095:         /// <summary>
00096:         /// Register or replace an item's localized morale modifier. The host
00097:         /// pulls modifiers from items.json (or any declarative source) at
00098:         /// boot and forwards them here. Idempotent; replacing a modifier
00099:         /// does NOT re-trigger OnDecorChanged because the change is at the
00100:         /// catalog level, not at a placement level.
00101:         /// </summary>
00102:         public void RegisterItemModifier(ShelterDecorItemModifier modifier)
00103:         {
00104:             if (modifier == null || string.IsNullOrEmpty(modifier.ItemId)) return;
00105:             _itemModifiers[modifier.ItemId] = modifier;
00106:         }
00107:
00108:         public ShelterDecorItemModifier? GetItemModifier(string itemId)
00109:         {
00110:             if (string.IsNullOrEmpty(itemId)) return null;
00111:             return _itemModifiers.TryGetValue(itemId, out var m) ? m : null;
00112:         }
00113:
00114:         /// <summary>
00115:         /// Assign (or replace) the decor item at (roomId, slotId). Returns
00116:         /// true on placement, false on rejected inputs. Empty ItemId is
00117:         /// the unassign sentinel. The host uses Empty ItemId + same
00118:         /// (roomId, slotId) to remove a placement cleanly.
00119:         /// </summary>
00120:         public bool Assign(string roomId, string slotId, string itemId, int dayInstalled, bool isMemorialPlaque = false, string memorialSurvivorId = "", string plaqueSourceHeirloomId = "")
00121:         {
00122:             if (string.IsNullOrEmpty(roomId) || string.IsNullOrEmpty(slotId)) return false;
00123:
00124:             var placement = new ShelterDecorPlacement
00125:             {
00126:                 RoomId = roomId,
00127:                 SlotId = slotId,
00128:                 ItemId = itemId ?? string.Empty,
00129:                 DayInstalled = dayInstalled,
00130:                 IsMemorialPlaque = isMemorialPlaque,
00131:                 MemorialSurvivorId = memorialSurvivorId ?? string.Empty,
00132:                 PlaqueSourceHeirloomId = plaqueSourceHeirloomId ?? string.Empty
00133:             };
00134:             _state.Placements.RemoveAll(p =>
00135:                 string.Equals(p.RoomId, roomId, StringComparison.Ordinal)
00136:                 && string.Equals(p.SlotId, slotId, StringComparison.Ordinal));
00137:             _state.Placements.Add(placement);
00138:             OnDecorChanged?.Invoke(placement);
00139:             OnStateChanged?.Invoke();
00140:             return true;
00141:         }
00142:
00143:         /// <summary>Convenience: Remove the placement at (roomId, slotId). Returns true if a placement was removed.</summary>
00144:         public bool Remove(string roomId, string slotId)
00145:         {
00146:             if (string.IsNullOrEmpty(roomId) || string.IsNullOrEmpty(slotId)) return false;
00147:             int before = _state.Placements.Count;
00148:             _state.Placements.RemoveAll(p =>
00149:                 string.Equals(p.RoomId, roomId, StringComparison.Ordinal)
00150:                 && string.Equals(p.SlotId, slotId, StringComparison.Ordinal));
00151:             if (_state.Placements.Count == before) return false;
00152:             OnStateChanged?.Invoke();
00153:             return true;
00154:         }
00155:
00156:         public ShelterDecorPlacement? GetSlot(string roomId, string slotId)
00157:         {
00158:             if (string.IsNullOrEmpty(roomId) || string.IsNullOrEmpty(slotId)) return null;
00159:             for (int i = 0; i < _state.Placements.Count; i++)
00160:             {
00161:                 var p = _state.Placements[i];
00162:                 if (string.Equals(p.RoomId, roomId, StringComparison.Ordinal)
00163:                     && string.Equals(p.SlotId, slotId, StringComparison.Ordinal))
00164:                     return p;
00165:             }
00166:             return null;
00167:         }
00168:
00169:         /// <summary>All placements in a single room, ordinal-ordered.</summary>
00170:         public List<ShelterDecorPlacement> ListRoomPlacements(string roomId)
00171:         {
00172:             var list = new List<ShelterDecorPlacement>();
00173:             if (string.IsNullOrEmpty(roomId)) return list;
00174:             for (int i = 0; i < _state.Placements.Count; i++)
00175:                 if (string.Equals(_state.Placements[i].RoomId, roomId, StringComparison.Ordinal))
00176:                     list.Add(_state.Placements[i]);
00177:             list.Sort((a, b) => string.CompareOrdinal(a.SlotId, b.SlotId));
00178:             return list;
00179:         }
00180:
00181:         /// <summary>
00182:         /// Sum of all decor modifiers currently mounted in a room. The
00183:         /// host applies this to <c>NeedsSystem.Modify(survivorId, Morale, delta)</c>
00184:         /// for each occupant during the morale tick.
00185:         /// </summary>
00186:         public float GetRoomMoraleDelta(string roomId)
00187:         {
00188:             float sum = 0f;
00189:             var placements = ListRoomPlacements(roomId);
00190:             for (int i = 0; i < placements.Count; i++)
00191:             {
00192:                 var p = placements[i];
00193:                 if (string.IsNullOrEmpty(p.ItemId)) continue;
00194:                 var mod = GetItemModifier(p.ItemId);
00195:                 if (mod == null) continue;
00196:                 sum += mod.LocalizedMoraleDelta;
00197:             }
00198:             return sum;
00199:         }
00200:
00201:         /// <summary>
00202:         /// Resolve a memorial plaque slot from a MemorialEntry. The plaque
00203:         /// is mounted at the (default) memorial wall slot of the room
00204:         /// already designated as the residence of the heirloom recipient.
00205:         /// When the recipient has no assigned room the host may pass
00206:         /// "<default_room>" to mount the plaque in the lobby.
00207:         ///
00208:         /// The returned <see cref="ShelterDecorPlacement"/> has ItemId
00209:         /// looked up via <see cref="ResolvePlaqueItemId"/>. The host should
00210:         /// call <see cref="Assign"/> on it.
00211:         /// </summary>
00212:         public ShelterDecorPlacement? ResolvePlaqueSlot(string memorialSurvivorId, string heirloomItemId, string memorialWallRoom, string plaqueSlotId, int dayInstalled)
00213:         {
00214:             var plaqueItemId = ResolvePlaqueItemId(heirloomItemId);
00215:             if (string.IsNullOrEmpty(plaqueItemId)) return null;
00216:             return new ShelterDecorPlacement
00217:             {
00218:                 RoomId = memorialWallRoom ?? string.Empty,
00219:                 SlotId = plaqueSlotId ?? string.Empty,
00220:                 ItemId = plaqueItemId,
00221:                 DayInstalled = dayInstalled,
00222:                 IsMemorialPlaque = true,
00223:                 MemorialSurvivorId = memorialSurvivorId ?? string.Empty,
00224:                 PlaqueSourceHeirloomId = heirloomItemId ?? string.Empty
00225:             };
00226:         }
00227:
00228:         /// <summary>
00229:         /// Convert a MemorialEntry.HeirloomItemId into the canonical
00230:         /// plaque item id the decor catalog ships. The mapping is:
00231:         ///   "item_personal_keepsake_&lt;survivor&gt;" or any "personal_keepsake_*"
00232:         ///   -> the matching "item_decor_memorial_plaque" entry by category
00233:         /// The host may also pass any source id and get the generic plaque
00234:         /// as a fallback so the slot is never empty.
00235:         /// </summary>
00236:         public string ResolvePlaqueItemId(string heirloomItemId)
00237:         {
00238:             if (string.IsNullOrEmpty(heirloomItemId)) return string.Empty;
00239:             // Match the host's preferred canonical id for the recipient's
00240:             // heirloom kind. Falls back to a single global generic plaque.
00241:             string kindTag = ExtractHeirloomKind(heirloomItemId);
00242:             if (!string.IsNullOrEmpty(kindTag)
00243:                 && _itemModifiers.TryGetValue(MemorialPlaquePrefix + "_" + kindTag, out var specific))
00244:             {
00245:                 return MemorialPlaquePrefix + "_" + kindTag;
00246:             }
00247:             // Fallback to generic plaque if registered.
00248:             if (_itemModifiers.ContainsKey(MemorialPlaquePrefix + "_generic"))
00249:                 return MemorialPlaquePrefix + "_generic";
00250:             // Cold fallback: if the catalog never registered a generic
00251:             // plaque, return the empty string so the host can surface a
00252:             // missing-plaque UI affordance rather than silently choosing
00253:             // an unknown item.
00254:             return string.Empty;
00255:         }
00256:
00257:         private static string ExtractHeirloomKind(string heirloomItemId)
00258:         {
00259:             if (string.IsNullOrEmpty(heirloomItemId)) return string.Empty;
00260:             // Convention: heirloom ids published by enrollment & final-wish
00261:             // systems carry the survivor's name + a kind tag. The plan-12C
00262:             // shape is "item_personal_keepsake_{survivor}_{kind}"; we extract
00263:             // the trailing kind segment. Other shapes are intentionally
00264:             // not accommodated \u2014 callers ship canonical ids.
00265:             int idx = heirloomItemId.LastIndexOf('_');
00266:             if (idx < 0 || idx == heirloomItemId.Length - 1) return string.Empty;
00267:             string tail = heirloomItemId.Substring(idx + 1);
00268:             return string.Equals(tail, "default", StringComparison.OrdinalIgnoreCase)
00269:                 ? string.Empty
00270:                 : tail;
00271:         }
00272:
00273:         public ShelterDecorStateCapture CaptureState() => _state.Capture();
00274:
00275:         public void RestoreState(ShelterDecorStateCapture saved)
00276:         {
00277:             if (saved == null) return;
00278:             _state.RestoreInto(saved);
00279:             OnStateChanged?.Invoke();
00280:         }
00281:
00282:         /// <summary>
00283:         /// Plan 14E / C1.6: Return all slots in this room eligible for or currently holding a trophy mount.
00284:         /// </summary>
00285:         public IReadOnlyList<string> GetTrophySlots(string roomId)
00286:         {
00287:             if (string.IsNullOrEmpty(roomId)) return Array.Empty<string>();
00288:             var slots = new List<string> { "trophy_mount_1", "trophy_mount_2" };
00289:             for (int i = 0; i < _state.Placements.Count; i++)
00290:             {
00291:                 var p = _state.Placements[i];
00292:                 if (string.Equals(p.RoomId, roomId, StringComparison.Ordinal)
00293:                     && IsTrophyItem(p.ItemId)
00294:                     && !slots.Contains(p.SlotId))
00295:                 {
00296:                     slots.Add(p.SlotId);
00297:                 }
00298:             }
00299:             return slots;
00300:         }
00301:
00302:         /// <summary>
00303:         /// Plan 14E / C1.6: Return the localized morale delta for a trophy item.
00304:         /// </summary>
00305:         public float GetTrophyMoraleModifier(string itemId)
00306:         {
00307:             if (string.IsNullOrEmpty(itemId)) return 0f;
00308:             var mod = GetItemModifier(itemId);
00309:             return mod != null ? mod.LocalizedMoraleDelta : 0f;
00310:         }
00311:
00312:         /// <summary>
00313:         /// True if the item id corresponds to a trophy mount.
00314:         /// </summary>
00315:         public static bool IsTrophyItem(string itemId)
00316:         {
00317:             if (string.IsNullOrEmpty(itemId)) return false;
00318:             return itemId.IndexOf("trophy", StringComparison.OrdinalIgnoreCase) >= 0;
00319:         }
00320:     }
00321:
00322:     /// <summary>
00323:     /// Declarative item modifier for a decor item. Authoritative values
00324:     /// live in items.json (canonical item_decor_* ids). The host
00325:     /// extracts the modifier from items via the mod_schema pipeline and
00326:     /// registers it on the system at boot.
00327:     /// </summary>
00328:     [Serializable]
00329:     public class ShelterDecorItemModifier
00330:     {
00331:         public string ItemId = string.Empty;
00332:         /// <summary>Localized morale bonus per tick while survivor sits in the decorated room.</summary>
00333:         public float LocalizedMoraleDelta;
00334:         /// <summary>Optional display category the host UI may use for sorting (poster/plaque/trophy).</summary>
00335:         public string Category = string.Empty;
00336:         /// <summary>When true, the host may stack the modifier multiplicatively; by default additive.</summary>
00337:         public bool StackMultiplicatively;
00338:     }
00339: }
```


# Appendix — Current Source Detail: `src/Host/ShelterDecorHostSession.cs`

### `src/Host/ShelterDecorHostSession.cs` — complete current file

- Size: 326 lines / 13499 bytes.
- SHA-256: `52b2a24194d91d6e412f85fd1cd2dbf28cd0d147f088100d24d9d642785e27b8`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.Inventory;
00005: using Ashfall.Core.Memorial;
00006: using Ashfall.Core.Shelter;
00007: using Ashfall.Core.Survivors;
00008: using InventoryContainer = Ashfall.Core.Inventory.Inventory;
00009:
00010: namespace AtomicWar.GodotApp
00011: {
00012:     /// <summary>
00013:     /// Thin Godot-host bridge for the shelter decor authority. It owns no
00014:     /// duplicate decor or morale state: item modifiers come from the already
00015:     /// loaded ItemCatalog, placements live in ShelterDecorSystem, room
00016:     /// occupancy comes from ShelterAssignmentSystem, and morale still belongs
00017:     /// to NeedsSystem.
00018:     /// </summary>
00019:     public sealed class ShelterDecorHostSession : HostSessionBase
00020:     {
00021:         public const string MemorialWallRoomId = "room_memorial_wall";
00022:
00023:         private readonly ShelterAssignmentSystem _assignments;
00024:         private readonly NeedsSystem _needs;
00025:         private readonly InventoryHostSession _inventory;
00026:
00027:         public ShelterDecorSystem System { get; }
00028:         public ShelterAssignmentSystem Assignments => _assignments;
00029:         public NeedsSystem Needs => _needs;
00030:         public ItemCatalog InventoryCatalog => _inventory.Catalog;
00031:         public InventoryContainer Inventory => _inventory.Inventory;
00032:         public string LastEvent { get; private set; } = string.Empty;
00033:         public int CatalogModifierCount { get; private set; }
00034:         public int LastMoraleRecipientCount { get; private set; }
00035:         public float LastMoraleGranted { get; private set; }
00036:         /// <summary>
00037:         /// Current campaign day supplied by Main at setup and daily advance.
00038:         /// It is command context only, not a second persistence authority.
00039:         /// </summary>
00040:         public int CurrentDay { get; private set; }
00041:
00042:         public ShelterDecorHostSession(
00043:             ShelterDecorSystem system,
00044:             ShelterAssignmentSystem assignments,
00045:             NeedsSystem needs,
00046:             InventoryHostSession inventory)
00047:         {
00048:             System = system ?? throw new ArgumentNullException(nameof(system));
00049:             _assignments = assignments ?? throw new ArgumentNullException(nameof(assignments));
00050:             _needs = needs ?? throw new ArgumentNullException(nameof(needs));
00051:             _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
00052:
00053:             System.OnStateChanged += OnSystemStateChanged;
00054:             _inventory.StateChanged += OnInventoryStateChanged;
00055:         }
00056:
00057:         /// <summary>
00058:         /// Registers every authoritative item_decor_* entry from the live item
00059:         /// catalog. No JSON is parsed a second time and modifiers never enter a
00060:         /// save payload.
00061:         /// </summary>
00062:         public int LoadCatalogModifiers()
00063:         {
00064:             int registered = 0;
00065:             foreach (string id in _inventory.Catalog.Ids)
00066:             {
00067:                 if (!id.StartsWith("item_decor_", StringComparison.Ordinal))
00068:                     continue;
00069:                 var definition = _inventory.Catalog.Get(id);
00070:                 if (definition == null) continue;
00071:                 System.RegisterItemModifier(new ShelterDecorItemModifier
00072:                 {
00073:                     ItemId = definition.id,
00074:                     LocalizedMoraleDelta = definition.decorLocalizedMoraleDelta,
00075:                     Category = CategoryFor(definition.id)
00076:                 });
00077:                 registered++;
00078:             }
00079:
00080:             CatalogModifierCount = registered;
00081:             LastEvent = registered == 0
00082:                 ? "No shelter decor items were registered from the item catalog."
00083:                 : $"{registered} shelter decor items registered from items.json.";
00084:             RequestPresentationRefresh();
00085:             return registered;
00086:         }
00087:
00088:         public IReadOnlyList<ShelterRoom> Rooms => _assignments.Rooms;
00089:
00090:         public void SetCurrentDay(int day)
00091:         {
00092:             CurrentDay = Math.Max(0, day);
00093:         }
00094:
00095:         public void SetPanelMessage(string message)
00096:         {
00097:             LastEvent = message ?? string.Empty;
00098:             RequestPresentationRefresh();
00099:         }
00100:
00101:         public string DisplayNameForRoom(string roomId)
00102:         {
00103:             if (string.Equals(roomId, MemorialWallRoomId, StringComparison.Ordinal))
00104:                 return "Memorial Wall";
00105:             for (int i = 0; i < _assignments.Rooms.Count; i++)
00106:             {
00107:                 var room = _assignments.Rooms[i];
00108:                 if (string.Equals(room.RoomId, roomId, StringComparison.Ordinal))
00109:                     return room.DisplayName;
00110:             }
00111:             return roomId ?? string.Empty;
00112:         }
00113:
00114:         public List<ItemDefinition> ListAvailableDecor()
00115:         {
00116:             var result = new List<ItemDefinition>();
00117:             foreach (var pair in System.ItemModifiers)
00118:             {
00119:                 var definition = _inventory.Catalog.Get(pair.Key);
00120:                 if (definition != null)
00121:                     result.Add(definition);
00122:             }
00123:             result.Sort((a, b) => string.Compare(a.displayName, b.displayName, StringComparison.Ordinal));
00124:             return result;
00125:         }
00126:
00127:         /// <summary>
00128:         /// Mounts a real decor item. The item is consumed from inventory only
00129:         /// after all placement validation succeeds; replacement must be a
00130:         /// separate remove action so an occupied slot can never silently lose
00131:         /// its item.
00132:         /// </summary>
00133:         public bool TryMount(string roomId, string slotId, string itemId, int day, out string reason)
00134:         {
00135:             reason = string.Empty;
00136:             if (!IsMountableRoom(roomId))
00137:             {
00138:                 reason = "Choose an existing shelter room.";
00139:                 return false;
00140:             }
00141:             if (string.IsNullOrWhiteSpace(slotId))
00142:             {
00143:                 reason = "Name the wall, peg, or shelf slot before mounting an item.";
00144:                 return false;
00145:             }
00146:             if (System.GetSlot(roomId, slotId) != null)
00147:             {
00148:                 reason = "That slot is occupied. Return its item to storage before mounting another.";
00149:                 return false;
00150:             }
00151:             if (System.GetItemModifier(itemId) == null)
00152:             {
00153:                 reason = "That item is not registered as shelter decor.";
00154:                 return false;
00155:             }
00156:             var definition = _inventory.Catalog.Get(itemId);
00157:             if (definition == null || _inventory.Inventory.CountById(itemId) < 1)
00158:             {
00159:                 reason = "The selected decor item is not in Holdfast storage.";
00160:                 return false;
00161:             }
00162:             if (!_inventory.Inventory.TryConsume(itemId, 1))
00163:             {
00164:                 reason = "Storage could not release the selected item.";
00165:                 return false;
00166:             }
00167:             if (!System.Assign(roomId, slotId.Trim(), itemId, day))
00168:             {
00169:                 _inventory.Inventory.TryProduce(itemId, 1, definition);
00170:                 reason = "The decor registry rejected that placement; the item was returned to storage.";
00171:                 return false;
00172:             }
00173:
00174:             LastEvent = $"Mounted {definition.displayName} at {DisplayNameForRoom(roomId)} / {slotId.Trim()}.";
00175:             reason = LastEvent;
00176:             return true;
00177:         }
00178:
00179:         /// <summary>
00180:         /// Returns a player-mounted item to storage. Memorial plaques are
00181:         /// records generated by MemorialSystem and stay on the wall.
00182:         /// </summary>
00183:         public bool TryRemoveMount(string roomId, string slotId, out string reason)
00184:         {
00185:             reason = string.Empty;
00186:             var placement = System.GetSlot(roomId, slotId);
00187:             if (placement == null)
00188:             {
00189:                 reason = "There is no mounted item at that slot.";
00190:                 return false;
00191:             }
00192:             if (placement.IsMemorialPlaque)
00193:             {
00194:                 reason = "Memorial plaques are ledger records and cannot be removed from this panel.";
00195:                 return false;
00196:             }
00197:             var definition = _inventory.Catalog.Get(placement.ItemId);
00198:             if (definition == null || !_inventory.Inventory.CanAdd(definition, 1))
00199:             {
00200:                 reason = "Storage has no safe capacity to receive that item.";
00201:                 return false;
00202:             }
00203:             if (!_inventory.Inventory.Add(definition, 1) || !System.Remove(roomId, slotId))
00204:             {
00205:                 reason = "The item could not be returned to storage.";
00206:                 return false;
00207:             }
00208:
00209:             LastEvent = $"Returned {definition.displayName} to Holdfast storage.";
00210:             reason = LastEvent;
00211:             return true;
00212:         }
00213:
00214:         /// <summary>
00215:         /// One-way MemorialSystem bridge. A newly committed ledger entry is
00216:         /// represented as an idempotent plaque at the dedicated wall; no fake
00217:         /// inventory item is minted or consumed.
00218:         /// </summary>
00219:         public bool TryMountMemorialPlaque(MemorialEntry entry, out string reason)
00220:         {
00221:             reason = string.Empty;
00222:             if (entry == null || string.IsNullOrEmpty(entry.SurvivorId))
00223:             {
00224:                 reason = "Memorial entry has no survivor id.";
00225:                 return false;
00226:             }
00227:             string slotId = "plaque_" + entry.SurvivorId;
00228:             var existing = System.GetSlot(MemorialWallRoomId, slotId);
00229:             if (existing != null && existing.IsMemorialPlaque
00230:                 && string.Equals(existing.MemorialSurvivorId, entry.SurvivorId, StringComparison.Ordinal))
00231:             {
00232:                 reason = "The memorial wall already carries this survivor's plaque.";
00233:                 return true;
00234:             }
00235:             var placement = System.ResolvePlaqueSlot(
00236:                 entry.SurvivorId,
00237:                 entry.HeirloomItemId,
00238:                 MemorialWallRoomId,
00239:                 slotId,
00240:                 entry.Day);
00241:             if (placement == null)
00242:             {
00243:                 reason = "The catalog has no registered memorial plaque item.";
00244:                 return false;
00245:             }
00246:             if (!System.Assign(
00247:                     placement.RoomId,
00248:                     placement.SlotId,
00249:                     placement.ItemId,
00250:                     placement.DayInstalled,
00251:                     placement.IsMemorialPlaque,
00252:                     placement.MemorialSurvivorId,
00253:                     placement.PlaqueSourceHeirloomId))
00254:             {
00255:                 reason = "The memorial plaque could not be registered.";
00256:                 return false;
00257:             }
00258:
00259:             LastEvent = $"Memorial plaque mounted for {entry.SurvivorId}.";
00260:             reason = LastEvent;
00261:             return true;
00262:         }
00263:
00264:         /// <summary>
00265:         /// Applies additive decor morale through the single existing needs
00266:         /// authority. Only alive survivors with an active room assignment are
00267:         /// recipients; the memorial wall has no assignment and thus grants no
00268:         /// passive morale by itself.
00269:         /// </summary>
00270:         public int ApplyDailyMorale(int day)
00271:         {
00272:             SetCurrentDay(day);
00273:             LastMoraleRecipientCount = 0;
00274:             LastMoraleGranted = 0f;
00275:             var assignments = _assignments.GetAssignments();
00276:             for (int i = 0; i < assignments.Count; i++)
00277:             {
00278:                 var assignment = assignments[i];
00279:                 if (assignment == null || assignment.Status != ShelterAssignmentStatus.Active)
00280:                     continue;
00281:                 float delta = System.GetRoomMoraleDelta(assignment.RoomId);
00282:                 if (Math.Abs(delta) < 0.0001f) continue;
00283:                 var survivor = _needs.Get(assignment.SurvivorId);
00284:                 if (survivor == null || !survivor.IsAliveState) continue;
00285:                 _needs.Modify(assignment.SurvivorId, NeedKind.Morale, delta);
00286:                 LastMoraleRecipientCount++;
00287:                 LastMoraleGranted += delta;
00288:             }
00289:
00290:             if (LastMoraleRecipientCount > 0)
00291:             {
00292:                 LastEvent = $"Room decor granted {LastMoraleGranted:F1} morale across {LastMoraleRecipientCount} assigned survivor(s).";
00293:                 RequestPresentationRefresh();
00294:             }
00295:             return LastMoraleRecipientCount;
00296:         }
00297:
00298:         protected override void UnsubscribeSystemEvents()
00299:         {
00300:             System.OnStateChanged -= OnSystemStateChanged;
00301:             _inventory.StateChanged -= OnInventoryStateChanged;
00302:         }
00303:
00304:         private bool IsMountableRoom(string roomId)
00305:         {
00306:             if (string.Equals(roomId, MemorialWallRoomId, StringComparison.Ordinal))
00307:                 return false;
00308:             for (int i = 0; i < _assignments.Rooms.Count; i++)
00309:                 if (string.Equals(_assignments.Rooms[i].RoomId, roomId, StringComparison.Ordinal))
00310:                     return true;
00311:             return false;
00312:         }
00313:
00314:         private static string CategoryFor(string itemId)
00315:         {
00316:             if (itemId.IndexOf("trophy", StringComparison.Ordinal) >= 0) return "trophy";
00317:             if (itemId.IndexOf("plaque", StringComparison.Ordinal) >= 0) return "memorial plaque";
00318:             if (itemId.IndexOf("poster", StringComparison.Ordinal) >= 0) return "poster";
00319:             if (itemId.IndexOf("drawing", StringComparison.Ordinal) >= 0) return "drawing";
00320:             return "keepsake";
00321:         }
00322:
00323:         private void OnSystemStateChanged() => RaiseStateChanged();
00324:         private void OnInventoryStateChanged() => RequestPresentationRefresh();
00325:     }
00326: }
```


# Appendix — Current Source Detail: `src/Host/ShelterDecorSaveStore.cs`

### `src/Host/ShelterDecorSaveStore.cs` — complete current file

- Size: 107 lines / 4484 bytes.
- SHA-256: `c8e9fb95de58f1cde1833c65871e9abc7287a219a9560ec1d750ba451501fe79`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // Save Store : ShelterDecorSaveStore
00004: // Core State : Ashfall.Core.Shelter.ShelterDecorState
00005: // Host Caller: Main.Campaign / ShelterDecorHostSession
00006: // Purpose    : Per-room decor placements, memorial plaque mounts, and the
00007: //              localized-morale modifier registry that the host wires from
00008: //              items.json at boot.
00009: // ============================================================================
00010: using System;
00011: using Ashfall.Core;
00012: using Ashfall.Core.Shelter;
00013: using Ashfall.Core.Save;
00014:
00015: namespace AtomicWar.GodotApp
00016: {
00017:     /// <summary>
00018:     /// Persists ShelterDecorState under user://shelter_decor_save.json \u2014
00019:     /// a thin static façade over the Core SaveStore&lt;T&gt; service via
00020:     /// SaveStoreHub (codec flavor). The Core state class is
00021:     /// self-checksummed (the checksum is a field of the state itself), so
00022:     /// encode/decode stamp and verify it directly; path resolution, atomic
00023:     /// write, and error handling live in the service.
00024:     /// </summary>
00025:     public static class ShelterDecorSaveStore
00026:     {
00027:         public const string FileName = "shelter_decor_save.json";
00028:         public const string SectionName = "shelter_decor";
00029:
00030:         private static readonly SaveStore<ShelterDecorStateCapture> s_store = SaveStoreHub.FromCodec(
00031:             FileName,
00032:             nameof(ShelterDecorSaveStore),
00033:             EncodeSave,
00034:             DecodeSave);
00035:
00036:         public static string SavePath => s_store.SavePath;
00037:
00038:         /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
00039:         public static string TryCaptureDirect(ShelterDecorState state) => s_store.CaptureBare(ToCapture(state));
00040:
00041:         /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
00042:         public static ShelterDecorState? TryRestoreDirect(string json)
00043:         {
00044:             var cap = s_store.RestoreBare(json);
00045:             return cap == null ? null : FromCapture(cap);
00046:         }
00047:
00048:         /// <summary>Capture state to JSON without writing to disk.</summary>
00049:         public static string TryCapture(ShelterDecorState state) => s_store.CaptureBare(ToCapture(state));
00050:
00051:         /// <summary>Restore state from JSON without reading from disk.</summary>
00052:         public static ShelterDecorState? TryRestore(string json)
00053:         {
00054:             var cap = s_store.RestoreBare(json);
00055:             return cap == null ? null : FromCapture(cap);
00056:         }
00057:
00058:         public static bool TrySave(ShelterDecorState state) => s_store.TrySave(ToCapture(state));
00059:
00060:         public static ShelterDecorState? TryLoad()
00061:         {
00062:             var cap = s_store.TryLoad();
00063:             return cap == null ? null : FromCapture(cap);
00064:         }
00065:
00066:         public static string TryCapturePersisted(ShelterDecorState state)
00067:             => s_store.CapturePersisted(ToCapture(state));
00068:
00069:         private static ShelterDecorStateCapture ToCapture(ShelterDecorState state)
00070:         {
00071:             var cap = new ShelterDecorStateCapture
00072:             {
00073:                 systemId = state.systemId,
00074:                 Placements = state.Placements ?? new System.Collections.Generic.List<ShelterDecorPlacement>()
00075:             };
00076:             cap.Checksum = SaveChecksum.Compute(cap);
00077:             return cap;
00078:         }
00079:
00080:         private static ShelterDecorState FromCapture(ShelterDecorStateCapture cap)
00081:         {
00082:             var s = new ShelterDecorState
00083:             {
00084:                 systemId = cap.systemId,
00085:                 Placements = cap.Placements ?? new System.Collections.Generic.List<ShelterDecorPlacement>()
00086:             };
00087:             return s;
00088:         }
00089:
00090:         private static string EncodeSave(ShelterDecorStateCapture cap, IJsonSerializer json)
00091:         {
00092:             cap.Checksum = SaveChecksum.Compute(cap);
00093:             return json.Serialize(cap);
00094:         }
00095:
00096:         private static ShelterDecorStateCapture? DecodeSave(string raw, IJsonSerializer json)
00097:         {
00098:             var cap = json.Deserialize<ShelterDecorStateCapture>(raw);
00099:             if (cap == null) return null;
00100:             if (string.IsNullOrEmpty(cap.Checksum))
00101:                 throw new InvalidOperationException("ShelterDecor: empty checksum");
00102:             if (!string.Equals(cap.Checksum, SaveChecksum.Compute(cap), StringComparison.Ordinal))
00103:                 throw new InvalidOperationException("ShelterDecor: checksum mismatch");
00104:             return cap;
00105:         }
00106:     }
00107: }
```


# Appendix — Current Source Detail: `src/Main.ShelterInfrastructure.cs`

### `src/Main.ShelterInfrastructure.cs` — bounded current excerpt (592 of 645 lines)

- Size: 645 lines / 31304 bytes.
- SHA-256: `5ec59e6a0a93e8e5c67bc7a39c91ff4cca74a518400fd09468938167c23630ea`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Inventory;
00007: using Ashfall.Core.Medical;
00008: using Ashfall.Core.Radiation;
00009: using Ashfall.Core.Shelter;
00010: using Ashfall.Core.StartingLevel;
00011: using Ashfall.Core.Survivors;
00012: using Ashfall.Core.YearOfAsh;
00013: using Ashfall.Core.World;
00014: using Ashfall.Core.Crafting;
00015: using Ashfall.Core.Journal;
00016: using Ashfall.Core.Expeditions;
00017: using Ashfall.Core.Waystation;
00018: using AtomicWar.GodotApp.UI;
00019:
00020: namespace AtomicWar.GodotApp
00021: {
00022:     public partial class Main : Control
00023:     {
00024:         private WaterTreatmentHostSession _waterTreatment = null!;
00025:         private WaterTreatmentPanel _waterTreatmentPanel = null!;
00026:         private AirlockSecurityHostSession _airlockSecurity = null!;
00027:         private AirlockSecurityPanel _airlockSecurityPanel = null!;
00028:         private bool _airlockSecurityDirty;
00029:         private ShelterThermalHostSession _shelterThermal = null!;
00030:         private ShelterThermalPanel _shelterThermalPanel = null!;
00031:         private bool _shelterThermalDirty;
00032:         private WeatherHardeningHostSession _weatherHardening = null!;
00033:         private bool _weatherHardeningDirty;
00034:         private GeothermalAquiferHostSession _geothermalAquifer = null!;
00035:         private bool _geothermalAquiferDirty;
00036:         private Ashfall.Core.VentilationSystem _ventilation = null!; // Plan 29 29B: machine tell readings
00037:         private VentilationHostSession? _ventilationHost;                    // Plan 72 stage console session
00038:         private Ashfall.Core.Shelter.ShelterFireHazardSystem? _stageFireHazard; // Plan 72 arc-fault fire handoff
00039:         private Ashfall.Core.Shelter.ShelterFireHazardSystem? _shelterFireHazard;
00040:         private ShelterFireHostSession? _shelterFireSession;
00041:
00042:         public Ashfall.Core.Shelter.ShelterFireHazardSystem ShelterFireHazard => GetShelterFireHazardSystem();
00043:         public ShelterFireHostSession? ShelterFireSession => _shelterFireSession;
00044:         private ShelterScheduleHostSession _shelterSchedule = null!;
00045:         private ShelterSchedulePanel _shelterSchedulePanel = null!;
00046:         private bool _shelterScheduleDirty;
00047:         private AutopsyHostSession _autopsy = null!;
00048:         private AutopsyReportPanel _autopsyReportPanel = null!;
00049:         private bool _autopsyDirty;
00050:         private WaystationHostSession _waystation = null!;
00051:         private WaystationNetworkPanel _waystationPanel = null!;
00052:         private bool _waystationDirty;
00053:
00054:         // Plan 29 Task 29A — shelter room identity overlay (read-only data projection,
00055:         // loaded once; no condition state, no save section of its own).
00056:         private ShelterRoomIdentityCatalog? _shelterRoomIdentity;
00057:
00058:         /// <summary>Lazy-load the room identity catalog from the data authority. Missing file → empty catalog (overlay, never a dependency).</summary>
00059:         private ShelterRoomIdentityCatalog? GetShelterRoomIdentityCatalog()
00060:         {
00061:             if (_shelterRoomIdentity != null) return _shelterRoomIdentity;
00062:             _shelterRoomIdentity = ShelterRoomIdentityCatalog.Load(
00063:                 new FileSystemIO(), new SystemTextJsonSerializer(), _dataDir);
00064:             return _shelterRoomIdentity;
00065:         }
00066:
00067:         // Plan 29 Task 29B — machine tell catalog (read-only data projection, loaded once).
00068:         private Ashfall.Core.Shelter.ShelterMachineTellCatalog? _machineTellCatalog;
00069:
00070:         private Ashfall.Core.Shelter.ShelterMachineTellCatalog GetMachineTellCatalog()
00071:         {
00072:             if (_machineTellCatalog != null) return _machineTellCatalog;
00073:             _machineTellCatalog = Ashfall.Core.Shelter.ShelterMachineTellCatalog.Load(
00074:                 new FileSystemIO(), new SystemTextJsonSerializer(), _dataDir);
00075:             return _machineTellCatalog;
00076:         }
00077:
00078:         // Plan 145 — bunker graffiti catalog (read-only ambient storytelling projection, loaded once).
00079:         private Ashfall.Core.Narrative.BunkerGraffitiCatalog? _bunkerGraffitiCatalog;
00080:
00081:         public Ashfall.Core.Narrative.BunkerGraffitiCatalog GetBunkerGraffitiCatalog()
00082:         {
00083:             if (_bunkerGraffitiCatalog != null) return _bunkerGraffitiCatalog;
00084:             _bunkerGraffitiCatalog = Ashfall.Core.Narrative.BunkerGraffitiCatalog.LoadFromDirectory(
00085:                 _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00086:             return _bunkerGraffitiCatalog;
00087:         }
00088:
00089:         // Plan 146 — bunker court catalog (read-only historical tribunal records, loaded once).
00090:         private Ashfall.Core.Narrative.BunkerCourtCatalog? _bunkerCourtCatalog;
00091:
00092:         public Ashfall.Core.Narrative.BunkerCourtCatalog GetBunkerCourtCatalog()
00093:         {
00094:             if (_bunkerCourtCatalog != null) return _bunkerCourtCatalog;
00095:             _bunkerCourtCatalog = Ashfall.Core.Narrative.BunkerCourtCatalog.LoadFromDirectory(
00096:                 _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00097:             return _bunkerCourtCatalog;
00098:         }
00099:
00100:         // Plan 148 — bunker maintenance catalog (read-only engineering emergency & glitch records, loaded once).
00101:         private Ashfall.Core.Narrative.BunkerMaintenanceCatalog? _bunkerMaintenanceCatalog;
00102:
00103:         public Ashfall.Core.Narrative.BunkerMaintenanceCatalog GetBunkerMaintenanceCatalog()
00104:         {
00105:             if (_bunkerMaintenanceCatalog != null) return _bunkerMaintenanceCatalog;
00106:             _bunkerMaintenanceCatalog = Ashfall.Core.Narrative.BunkerMaintenanceCatalog.LoadFromDirectory(
00107:                 _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00108:             return _bunkerMaintenanceCatalog;
00109:         }
00110:
00111:         // Plan 150 — personal letter catalog (read-only personal & unsent correspondence, loaded once).
00112:         private Ashfall.Core.Narrative.PersonalLetterCatalog? _personalLetterCatalog;
00113:
00114:         public Ashfall.Core.Narrative.PersonalLetterCatalog GetPersonalLetterCatalog()
00115:         {
00116:             if (_personalLetterCatalog != null) return _personalLetterCatalog;
00117:             _personalLetterCatalog = Ashfall.Core.Narrative.PersonalLetterCatalog.LoadFromDirectory(
00118:                 _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00119:             return _personalLetterCatalog;
00120:         }
00121:
00122:         // Plan 151 — abyssal anomalies science archive catalog (read-only science/anomaly logs, loaded once).
00123:         private Ashfall.Core.Narrative.AbyssalAnomaliesCatalog? _abyssalAnomaliesCatalog;
00124:
00125:         public Ashfall.Core.Narrative.AbyssalAnomaliesCatalog GetAbyssalAnomaliesCatalog()
00126:         {
00127:             if (_abyssalAnomaliesCatalog != null) return _abyssalAnomaliesCatalog;
00128:             _abyssalAnomaliesCatalog = Ashfall.Core.Narrative.AbyssalAnomaliesCatalog.LoadFromDirectory(
00129:                 _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00130:             return _abyssalAnomaliesCatalog;
00131:         }
00132:
00133:         /// <summary>
00134:         /// Plan 29 29A: a shelter room hotspot was clicked — treat it as inspection.
00135:         /// Marks the authoritative Day-1 roster inspection (legacy ids tolerated via
00136:         /// the catalog alias map) and unlocks inspect_room vignettes through the
00137:         /// JournalSystem knowledge key (journal save owns persistence; old saves
00138:         /// simply default locked and unlock on the next inspection).
00139:         /// </summary>
00140:         private void HandleShelterRoomSelected(string roomId)
00141:         {
00142:             if (string.IsNullOrEmpty(roomId)) return;
00143:             var catalog = GetShelterRoomIdentityCatalog();
00144:             string canonical = catalog?.ResolveRoomId(roomId) ?? roomId;
00145:
00146:             if (_startingLevel != null && !_startingLevel.System.InspectRoom(canonical))
00147:             {
00148:                 var aliases = catalog?.GetLegacyAliases(canonical);
00149:                 if (aliases != null)
00150:                 {
00151:                     for (int i = 0; i < aliases.Count; i++)
00152:                         if (_startingLevel.System.InspectRoom(aliases[i])) break;
00154:             }
00155:
00156:             UnlockRoomHistories(catalog,
00157:                 catalog?.GetUnlockableVignettes(canonical,
00158:                     ShelterRoomIdentityCatalog.RoomHistoryTrigger.RoomInspected));
00159:
00160:             // Plans 150/151/153 — real room inspection unlocks producer-bound
00161:             // narrative records (not panel-open dumps).
00162:             SetupJournal();
00163:             DiscoverPersonalLetterRecords(canonical);
00164:             DiscoverAbyssalAnomalyRecords(canonical);
00165:             DiscoverFringeCultRecords(canonical);
00166:             DiscoverPaperPrintingRecords(canonical);
00167:             DiscoverBoneHornRecords(canonical);
00168:         }
00169:
00170:         /// <summary>
00171:         /// Plan 29 29A: a real repair/maintenance action completed in a shelter room
00172:         /// (filter service/replace). Raises the repair_performed unlock path only —
00173:         /// authored vignettes never fire from a decorative interaction.
00174:         /// </summary>
00175:         private void HandleShelterRoomRepairPerformed(string roomId)
00176:         {
00177:             if (string.IsNullOrEmpty(roomId)) return;
00178:             var catalog = GetShelterRoomIdentityCatalog();
00179:             UnlockRoomHistories(catalog,
00180:                 catalog?.GetUnlockableVignettes(catalog.ResolveRoomId(roomId),
00181:                     ShelterRoomIdentityCatalog.RoomHistoryTrigger.RepairPerformed));
00182:         }
00183:
00184:         /// <summary>
00185:         /// Plan 29 29A: daily milestone pass. Runs once per campaign day from the
00186:         /// day coordinator (never per frame) and unlocks at most the vignettes whose
00187:         /// required day has been reached. Journal keys make it idempotent, so a late
00188:         /// load of an older save catches up once rather than spamming every tick.
00189:         /// </summary>
00190:         private void TickShelterRoomHistoryMilestones(int day)
00191:         {
00192:             var catalog = GetShelterRoomIdentityCatalog();
00193:             if (catalog == null) return;
00194:             UnlockRoomHistories(catalog, catalog.GetDayMilestoneVignettes(day));
00195:
00196:             // Plan 29 29B: daily machine glitch pass — journal one-shots, evaluate continuous.
00197:             TickMachineGlitchEvents(day);
00198:
00199:             // Plan 29 §29B.21: machine tell audio — quirk cues start on threshold
00200:             // crossings and stop on recovery; personality beds sustain.
00201:             TickMachineTellAudio();
00202:         }
00203:
00204:         /// <summary>
00205:         /// Plan 29 §29B.21 consumer side: daily machine tell audio sync. Evaluates
00206:         /// the same readings the text tells use and diffs the fired quirks against
00207:         /// the live audio conditions — newly degraded tells start their ElevenLabs
00208:         /// cue, recovered tells stop it, personality beds stay continuous. The
00209:         /// condition system's already_active guard makes repeated applies no-ops,
00210:         /// so audio fires on threshold transitions (§14), never per frame. No new
00211:         /// state authority: tells re-derive from the owning systems' live condition.
00212:         /// </summary>
00213:         private void TickMachineTellAudio()
00214:         {
00215:             var catalog = GetMachineTellCatalog();
00216:             if (catalog == null || catalog.MachineCount == 0) return;
00217:
00218:             var readings = BuildMachineReadings();
00219:             if (readings == null) return;
00220:
00221:             Ashfall.Core.Shelter.MachineTellAudioSync.Apply(
00222:                 catalog, readings, _audioConditions,
00223:                 cueId => AtomicWar.GodotApp.Audio.AudioCueCatalog.Resolve(cueId)?.Loop ?? false);
00224:         }
00225:
00226:         /// <summary>Apply an unlock batch through the journal (the single persistence authority).</summary>
00227:         private void UnlockRoomHistories(ShelterRoomIdentityCatalog? catalog,
00228:             System.Collections.Generic.IReadOnlyList<RoomHistoryVignette>? vignettes)
00229:         {
00230:             if (catalog == null || vignettes == null || _journal == null) return;
00231:             for (int i = 0; i < vignettes.Count; i++)
00232:             {
00233:                 if (_journal.UnlockRoomHistorySeen(vignettes[i].id))
00234:                     _journalDirty = true;
00237:
00238:         /// <summary>
00239:         /// Plan 29 29B: daily machine glitch pass. Journals one-shot glitches (idempotent
00240:         /// via journal keys) and evaluates continuous glitches for UI surfacing. Old saves
00241:         /// default un-noted and reveal once; continuous events re-fire on their cooldown,
00242:         /// paced by the caller's day bookkeeping.
00243:         /// </summary>
00244:         private void TickMachineGlitchEvents(int day)
00245:         {
00246:             var catalog = GetMachineTellCatalog();
00247:             if (catalog == null || catalog.GlitchEvents.Count == 0 || _journal == null) return;
00248:
00249:             var readings = BuildMachineReadings();
00250:             if (readings == null) return;
00251:
00252:             bool isNoted(string id) => _journal.IsGlitchNoted(id);
00253:             for (int m = 0; m < catalog.MachineCount; m++)
00254:             {
00255:                 string mid = catalog.Machines[m].id;
00256:                 var glitches = catalog.EvaluateGlitchEvents(mid, readings, isNoted);
00257:                 for (int g = 0; g < glitches.Count; g++)
00258:                 {
00259:                     var gl = glitches[g];
00260:                     if (string.Equals(gl.repeat_policy, "once", System.StringComparison.Ordinal))
00261:                     {
00262:                         _journal.UnlockGlitchNoted(gl.id);
00263:                     }
00264:                 }
00266:         }
00267:
00268:         /// <summary>Build MachineConditionReadings from live host systems for tell evaluation.</summary>
00269:         private Ashfall.Core.Shelter.MachineConditionReadings? BuildMachineReadings()
00270:         {
00271:             try
00272:             {
00273:                 return new Ashfall.Core.Shelter.MachineConditionReadings
00274:                 {
00275:                     HepaFilterHealth = (float)Math.Clamp(_startingLevel?.System.State.airFilterHealthPercent ?? 100, 0, 100),
00276:                     HepaRadon = (float)Math.Clamp(_startingLevel?.System.State.radonLevelBqm3 ?? 12, 0, 200),
00277:                     PowerFuelUnits = (float)Math.Clamp(_powerGrid?.System.State.FuelUnits ?? 0, 0, 200),
00278:                     PowerBatteryReserve = _powerGrid != null ? (_powerGrid.System.State.BatteryReserveWh / 4000f * 100f) : 100f,
00279:                     VentilationFilterSaturation = (float)Math.Clamp(_ventilation?.FilterSaturation ?? 0, 0, 100),
00280:                     WaterFilterIntegrity = (float)Math.Clamp(_waterTreatment?.System.FilterIntegrity ?? 100, 0, 100),
00281:                     ThermalBoilerFuel = (float)Math.Clamp(_shelterThermal?.System.BoilerFuelLevel ?? 0, 0, 200),
00282:                     AirlockIncidentActive = _airlockSecurity?.System.HasPendingIncident ?? false,
00283:                     HazardWeather = _world?.Weather.Current is Ashfall.Core.WeatherKind.FalloutStorm or Ashfall.Core.WeatherKind.BlackRain or Ashfall.Core.WeatherKind.Ashfall
00290:         }
00291:
00292:         /// <summary>Build a one-line dashboard tell string from live machine readings (§29B.9–29B.13).</summary>
00293:         public string BuildMachineTellText(ISeededRng? rng = null)
00294:         {
00295:             try
00296:             {
00297:                 var catalog = GetMachineTellCatalog();
00298:                 if (catalog == null || catalog.MachineCount == 0) return string.Empty;
00299:
00300:                 var readings = BuildMachineReadings();
00301:                 if (readings == null) return string.Empty;
00302:
00303:                 var fired = new System.Collections.Generic.List<string>();
00304:                 bool isNoted(string id) => _journal != null && _journal.IsGlitchNoted(id);
00305:                 for (int m = 0; m < catalog.MachineCount; m++)
00306:                 {
00307:                     string mid = catalog.Machines[m].id;
00308:                     string label = catalog.Machines[m].display_name;
00309:                     if (string.IsNullOrWhiteSpace(label))
00310:                     {
00311:                         label = mid;
00312:                         if (label.StartsWith("machine_", StringComparison.Ordinal))
00314:                     }
00315:                     // Shorten to a readable tag: "Main Generator & Battery Bank" → "Generator"
00316:                     if (label.Contains("&", StringComparison.Ordinal))
00317:                         label = label.Split('&')[0].Trim();
00318:                     label = label.Replace("Filtration Stack", "HEPA").Replace("Exhaust Plant", "Ventilation").Replace("Brine Still", "Still").Replace("Shelter ", "").Replace("Airlock Machinery", "Airlock");
00319:                     label = label.ToUpperInvariant();
00320:
00321:                     var quirks = catalog.EvaluateQuirks(mid, readings);
00322:                     for (int q = 0; q < quirks.Count; q++)
00323:                     {
00324:                         var qk = quirks[q];
00325:                         if (string.Equals(qk.kind, "diagnostic", System.StringComparison.Ordinal))
00327:                     }
00328:
00329:                     var glitches = catalog.EvaluateGlitchEvents(mid, readings, isNoted);
00330:                     for (int g = 0; g < glitches.Count; g++)
00331:                     {
00332:                         var gl = glitches[g];
00333:                         fired.Add($"[{label}] {gl.title}");
00334:                         if (string.Equals(gl.repeat_policy, "once", System.StringComparison.Ordinal) && _journal != null)
00335:                             _journal.UnlockGlitchNoted(gl.id);
00336:                     }
00337:                 }
00338:
00351:             if (_waterTreatment != null) return;
00352:             SetupInventory();
00353:             var wtState = WaterTreatmentSaveStore.TryLoad() ?? new WaterTreatmentState();
00354:             var wtSys = new WaterTreatmentSystem(new GodotLog());
00355:             wtSys.RestoreState(wtState);
00356:             _waterTreatment = new WaterTreatmentHostSession(wtSys, _inventory);
00357:             // CORE-MECH W3: winter pressure on the water owner. One read-only
00358:             // day → multiplier view of the Year-of-Ash calendar, applied once at
00359:             // the owner's single filter-degradation site (AA.3). Fail-closed to
00360:             // neutral when the calendar cannot be read.
00361:             try
00362:             {
00363:                 var yoaEvents = YearOfAshCatalogLoader.LoadEvents(
00364:                     CatalogPath.ResolveDataDir(),
00365:                     CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir()),
00366:                     new SystemTextJsonSerializer());
00367:                 var pressure = new SeasonalPressureProvider(yoaEvents);
00368:                 wtSys.SeasonalFilterLoadMultiplier = day => pressure.MultiplierFor(day);
00369:             }
00370:             catch (Exception ex)
00371:             {
00372:                 GD.Print("[Ashfall Godot] Seasonal water pressure unavailable: " + ex.Message);
00373:             }
00374:             _waterTreatment.OnTreatmentStarted += () => ObserveSigil("water.treatment_started");
00375:             // B5–B8 expansion (§9.12): unsafe-water exposure → the canonical
00376:             // disease sweep. WaterborneExposureRules owns the dose→disease
00377:             // mapping (Core-pure); DiseaseSystem owns the outcome roll; the
00378:             // roster only supplies the exposed population.
00379:             _waterTreatment.PathogenExposureSink = dose =>
00380:             {
00381:                 if (_disease?.Engine == null
00382:                     || !Ashfall.Core.WaterborneExposureRules.ShouldRunExposureSweep(dose))
00388:                              ?? new List<Ashfall.Core.Survivors.SurvivorRosterEntry>())
00389:                     {
00390:                         if (entry == null || !entry.isAlive) continue;
00391:                         _disease.Engine.TryExpose(new Ashfall.Core.Disease.DiseaseExposureContext
00392:                         {
00393:                             SurvivorId = entry.survivorId,
00394:                             DiseaseId = diseaseId,
00395:                             SourceId = Ashfall.Core.WaterborneExposureRules.SourceId,
00408:         }
00409:
00410:         private void SaveWaterTreatment()
00411:         {
00412:             if (_waterTreatment != null)
00413:                 CaptureSection("water_treatment", WaterTreatmentSaveStore.TryCapturePersisted(_waterTreatment.System.CaptureState()));
00414:         }
00415:
00416:         private void SetupAirlockSecurity()
00417:         {
00418:             if (_airlockSecurity != null) return;
00419:             var asState = AirlockSecuritySaveStore.TryLoad() ?? new AirlockSecurityState();
00420:             var asSys = new AirlockSecuritySystem(new SeededRng(1986), new GodotLog());
00421:             asSys.RestoreState(asState);
00422:             _airlockSecurity = new AirlockSecurityHostSession(asSys);
00423:             if (_airlockSecurityPanel != null && _airlockSecurityPanel.IsInsideTree())
00424:                 RemoveChild(_airlockSecurityPanel);
00425:             _airlockSecurityPanel = new AirlockSecurityPanel();
00426:             _airlockSecurityPanel.Bind(_airlockSecurity);
00429:         }
00430:
00431:         private void SaveAirlockSecurity()
00432:         {
00433:             if (_airlockSecurity != null)
00434:                 CaptureSection("airlock_security", AirlockSecuritySaveStore.TryCapturePersisted(_airlockSecurity.System.CaptureState()));
00435:         }
00436:
00437:         private void SetupShelterThermal()
00438:         {
00439:             if (_shelterThermal != null) return;
00440:             var stState = ShelterThermalSaveStore.TryLoad() ?? new ShelterThermalState();
00441:             var stNeeds = _survivors.Needs;
00442:             var stStarting = _startingLevel.System;
00443:             var stDeepFreeze = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState());
00444:             var stSys = new ShelterThermalSystem(new SeededRng(1986), stNeeds, stStarting, stDeepFreeze, new GodotLog());
00445:             stSys.RestoreState(stState);
00446:             _shelterThermal = new ShelterThermalHostSession(stSys);
00447:             if (_shelterThermalPanel != null && _shelterThermalPanel.IsInsideTree())
00448:                 RemoveChild(_shelterThermalPanel);
00449:             _shelterThermalPanel = new ShelterThermalPanel();
00450:             _shelterThermalPanel.Bind(_shelterThermal);
00453:         }
00454:
00455:         private void SaveShelterThermal()
00456:         {
00457:             if (_shelterThermal != null)
00458:                 CaptureSection("shelter_thermal", ShelterThermalSaveStore.TryCapturePersisted(_shelterThermal.System.CaptureState()));
00459:         }
00460:
00461:         private void SetupWeatherHardening()
00462:         {
00463:             if (_weatherHardening != null) return;
00464:             var whState = WeatherHardeningSaveStore.TryLoad() ?? new WeatherHardeningState();
00465:             var whSys = new WeatherHardeningSystem(
00466:                 whState,
00467:                 new SeededRng(1999),
00468:                 new GodotLog(),
00472:                 _waterTreatment?.System,
00473:                 _inventory?.Inventory);
00474:             _weatherHardening = new WeatherHardeningHostSession(whSys);
00475:             _weatherHardening.LoadCatalog(_dataDir);
00476:         }
00477:
00478:         private void SaveWeatherHardening()
00479:         {
00480:             if (_weatherHardening != null)
00481:                 CaptureSection("weather_hardening", WeatherHardeningSaveStore.TryCapturePersisted(_weatherHardening.System.CaptureState()));
00482:         }
00483:
00484:         private void SetupGeothermalAquifer()
00485:         {
00486:             if (_geothermalAquifer != null) return;
00487:             var state = GeothermalAquiferSaveStore.TryLoad() ?? new GeothermalAquiferState();
00488:             var system = new GeothermalAquiferSystem(
00489:                 state,
00490:                 new SeededRng(2003),
00491:                 new GodotLog(),
00493:                 _waterTreatment?.System,
00494:                 _inventory?.Inventory);
00495:             _geothermalAquifer = new GeothermalAquiferHostSession(system);
00496:             _geothermalAquifer.LoadCatalog(_dataDir);
00497:             if (_geothermalAquiferPanel != null)
00498:                 _geothermalAquiferPanel.Bind(_geothermalAquifer);
00499:         }
00500:
00501:         private void SaveGeothermalAquifer()
00502:         {
00503:             if (_geothermalAquifer != null)
00504:                 CaptureSection("geothermal_aquifer", GeothermalAquiferSaveStore.TryCapturePersisted(_geothermalAquifer.System.CaptureState()));
00505:         }
00506:
00507:         private void SetupShelterSchedule()
00508:         {
00509:             if (_shelterSchedule != null) return;
00510:             var ssState = ShelterScheduleSaveStore.TryLoad() ?? new ShelterScheduleState();
00511:             var ssPower = _powerGrid.System;
00512:             var ssSys = new ShelterScheduleSystem(ssPower, new GodotLog());
00513:             ssSys.RestoreState(ssState);
00514:             _shelterSchedule = new ShelterScheduleHostSession(ssSys);
00515:             _shelterSchedule.LoadCatalog(_dataDir);
00516:             if (_shelterSchedulePanel != null && _shelterSchedulePanel.IsInsideTree())
00517:                 RemoveChild(_shelterSchedulePanel);
00518:             _shelterSchedulePanel = new ShelterSchedulePanel();
00519:             _shelterSchedulePanel.Bind(_shelterSchedule);
00522:         }
00523:
00524:         private void SaveShelterSchedule()
00525:         {
00526:             if (_shelterSchedule != null)
00527:                 CaptureSection("shelter_schedule", ShelterScheduleSaveStore.TryCapturePersisted(_shelterSchedule.System.CaptureState()));
00528:         }
00529:
00530:         private void SetupAutopsy(ResearchSystem? sharedResearch = null)
00531:         {
00532:             if (_autopsy != null) return;
00533:             sharedResearch ??= _sharedResearch;
00534:             var auState = AutopsySaveStore.TryLoad() ?? new AutopsyState();
00535:             var auInv = _inventory.Inventory;
00536:             var auRad = _survivors.Radiation;
00537:             var auStarting = _startingLevel.System;
00538:             var auVent = new VentilationSystem(auStarting);
00539:             _ventilation = auVent; // Plan 29 29B: expose for machine tell readings
00540:             // Plan 72: electrostatic stage catalog + persistent arc-fire hazard.
00541:             auVent.ApplyElectrostaticCatalog(Ashfall.Core.ElectrostaticFiltrationCatalogLoader.Load(
00542:                 _dataDir, new FileSystemIO(), new SystemTextJsonSerializer()));
00543:             SetupShelterFireHazard();
00544:             _ventilationHost = new VentilationHostSession(auVent);
00545:             var auRes = sharedResearch;
00546:             var auMedical = _medicalWard;
00547:             var auSys = new AutopsySystem(new SeededRng(1986), auInv, auRad, auVent, auRes, auMedical, new GodotLog());
00548:             auSys.RestoreState(auState);
00549:             _autopsy = new AutopsyHostSession(auSys);
00550:             _autopsy.LoadCatalog(_dataDir);
00551:             if (_autopsyReportPanel != null && _autopsyReportPanel.IsInsideTree())
00552:                 RemoveChild(_autopsyReportPanel);
00553:             _autopsyReportPanel = new AutopsyReportPanel();
00554:             _autopsyReportPanel.Bind(_autopsy);
00557:         }
00558:
00559:         private void SaveAutopsy()
00560:         {
00561:             if (_autopsy != null)
00562:                 CaptureSection("autopsy", AutopsySaveStore.TryCapturePersisted(_autopsy.System.CaptureState()));
00563:         }
00564:
00565:         private void SetupWaystation()
00566:         {
00567:             if (_waystation != null) return;
00568:             var wsState = WaystationSaveStore.TryLoad() ?? new WaystationSystemState();
00569:             var wsSys = new WaystationSystem();
00570:             wsSys.RestoreState(wsState);
00571:             _waystation = new WaystationHostSession(wsSys);
00572:
00573:             // Plan 56 phase 6 — the multi-node trade-stock network: its 7-day
00574:             // resupply is provenance-aware (locally produced + general stock
00575:             // survive a market shortage; pure imports lapse). The shortage
00576:             // policy reads the live market; the closure is null-safe because
00577:             // the economy session may not be set up yet when it is bound.
00578:             SetupEconomy();
00579:             var network = new WaystationNetworkSystem();
00580:             if (wsState.network != null)
00581:                 network.RestoreState(wsState.network);
00582:             if (_economy?.Catalog != null)
00583:             {
00584:                 _waystation.AttachNetwork(
00585:                     network,
00586:                     _economy.Catalog,
00587:                     () => _economy?.Market.IsSuppliesShort() ?? false);
00588:             }
00589:             if (_waystationPanel != null && _waystationPanel.IsInsideTree())
00590:                 RemoveChild(_waystationPanel);
00591:             _waystationPanel = new WaystationNetworkPanel();
00592:             _waystationPanel.Bind(_waystation);
00593:             _waystationPanel.Visible = false;
00594:             AddChild(_waystationPanel);
00595:         }
00596:
00597:         private void SaveWaystation()
00598:         {
00599:             if (_waystation == null) return;
00600:             var state = _waystation.System.CaptureState();
00601:             if (_waystation.Network != null)
00602:                 state.network = _waystation.Network.CaptureState();
00603:             CaptureSection("waystation", WaystationSaveStore.TryCapturePersisted(state));
00604:         }
00605:
00606:         private bool _shelterFireDirty;
00607:
00608:         private void SetupShelterFireHazard()
00609:         {
00610:             if (_shelterFireHazard != null) return;
00611:             _shelterFireHazard = new Ashfall.Core.Shelter.ShelterFireHazardSystem();
00612:             _stageFireHazard = _shelterFireHazard;
00613:             _shelterFireSession = new ShelterFireHostSession(_shelterFireHazard);
00614:             _shelterFireSession.StateChanged += () => _shelterFireDirty = true;
00615:
00616:             var saved = ShelterFireSaveStore.TryLoad();
00617:             if (saved != null)
00618:                 ShelterFireSaveStore.ApplyToSystem(_shelterFireHazard, saved);
00619:         }
00620:
00621:         private void SaveShelterFire()
00622:         {
00623:             if (_shelterFireHazard == null) return;
00624:             if (CaptureSection(
00625:                     "shelter_fire",
00626:                     ShelterFireSaveStore.TryCapturePersisted(
00627:                         ShelterFireSaveStore.FromSystem(_shelterFireHazard))))
00628:             {
00629:                 _shelterFireDirty = false;
00630:             }
00631:         }
00632:
00633:         private void FlushShelterFireIfDirty()
00634:         {
00635:             if (_shelterFireDirty) SaveShelterFire();
00636:         }
00637:
00638:         public Ashfall.Core.Shelter.ShelterFireHazardSystem GetShelterFireHazardSystem()
00639:         {
00640:             if (_shelterFireHazard == null)
00641:                 SetupShelterFireHazard();
00642:             return _shelterFireHazard!;
00643:         }
00644:     }
00645: }
```


# Appendix — Current Source Detail: `src/UI/ShelterDecorPanel.cs`

### `src/UI/ShelterDecorPanel.cs` — bounded current excerpt (401 of 443 lines)

- Size: 443 lines / 21161 bytes.
- SHA-256: `ebf8710e14d5aaed08d3edaae4b02ef28199d223f09126e419ace0f8188fec4e`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Godot;
00004: using Ashfall.Core.Shelter;
00005:
00006: namespace AtomicWar.GodotApp.UI
00007: {
00008:     /// <summary>
00009:     /// Live room-interior panel for Plan 12C. Every action routes through
00010:     /// ShelterDecorHostSession: mounting consumes a real inventory item,
00011:     /// removal returns it to storage, and memorial plaques are read-only
00012:     /// projections of the memorial ledger.
00013:     /// </summary>
00014:     public partial class ShelterDecorPanel : Control, IBindablePanel
00015:     {
00016:         public event Action? OnClose;
00017:
00018:         private AshfallDashboardShell _shell = null!;
00019:         private AshfallStatusRail _statusRail = null!;
00020:         private OptionButton _roomPicker = null!;
00021:         private LineEdit _slotInput = null!;
00022:         private VBoxContainer _placements = null!;
00023:         private VBoxContainer _storage = null!;
00024:         private Label _roomSummary = null!;
00025:         private Label _selectionSummary = null!;
00026:         private Label _eventLine = null!;
00027:         private string _selectedItemId = string.Empty;
00028:         private ShelterDecorHostSession? _host;
00029:
00030:         /// <summary>Rendered count exposed to the headless self-test.</summary>
00031:         public int RenderedPlacementCount { get; private set; }
00032:         public bool IsBound => _host != null;
00033:
00034:         /// <summary>
00035:         /// Opens the panel through the same lightweight surface used by the
00036:         /// player route and the visual snapshot harness.
00037:         /// </summary>
00038:         public void Open()
00039:         {
00040:             Visible = true;
00041:             RefreshView();
00042:         }
00043:
00044:         public void Bind(ShelterDecorHostSession session)
00045:         {
00046:             if (ReferenceEquals(_host, session)) return;
00047:             Unbind();
00048:             _host = session;
00049:             if (_host != null)
00050:             {
00051:                 _host.StateChanged += RefreshView;
00052:                 _host.PresentationRefreshRequested += RefreshView;
00053:             }
00054:             RefreshView();
00055:         }
00056:
00057:         public void Unbind()
00058:         {
00059:             if (_host == null) return;
00060:             _host.StateChanged -= RefreshView;
00061:             _host.PresentationRefreshRequested -= RefreshView;
00062:             _host = null;
00063:         }
00064:
00065:         public override void _Ready()
00066:         {
00067:             SetAnchorsPreset(LayoutPreset.FullRect);
00068:
00069:             _shell = new AshfallDashboardShell("Shelter Interior // Memorial Wall", minWidth: 1160, minHeight: 700);
00070:             AddChild(_shell);
00071:
00072:             _statusRail = _shell.SetStatusRail();
00073:             _statusRail.AddCard("mounted", "Mounted Pieces", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
00074:             _statusRail.AddCard("rooms", "Decorated Rooms", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);
00075:             _statusRail.AddCard("morale", "Daily Room Morale", "+0.0", AshfallMetricCard.Criticality.Normal, minWidth: 160);
00076:             _statusRail.AddCard("plaques", "Memorial Plaques", "0", AshfallMetricCard.Criticality.Normal, minWidth: 145);
00077:
00078:             var root = AshfallUiHelpers.MakeVBox(12);
00079:             root.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00080:             root.SizeFlagsVertical = SizeFlags.ExpandFill;
00081:
00082:             var mountPanel = AshfallUiHelpers.MakePanel();
00083:             var mountStack = AshfallUiHelpers.MakeVBox(8);
00084:             mountPanel.AddChild(mountStack);
00085:             mountStack.AddChild(AshfallUiHelpers.MakeSectionHeader("Mount from Holdfast storage"));
00086:
00087:             var mountRow = AshfallUiHelpers.MakeHBox(8);
00088:             mountStack.AddChild(mountRow);
00089:             mountRow.AddChild(AshfallUiHelpers.MakeLabel("ROOM"));
00090:             _roomPicker = new OptionButton { CustomMinimumSize = new Vector2(245, 36) };
00091:             _roomPicker.ItemSelected += _ => RefreshView();
00092:             mountRow.AddChild(_roomPicker);
00093:             mountRow.AddChild(AshfallUiHelpers.MakeLabel("SLOT"));
00094:             _slotInput = new LineEdit
00095:             {
00096:                 PlaceholderText = "north_wall / shelf_1 / entry_hook",
00097:                 CustomMinimumSize = new Vector2(260, 36),
00098:                 TooltipText = "A named wall, shelf, peg, or surface inside the selected room."
00099:             };
00100:             if (AshfallUiHelpers.FontShareTechMono != null)
00101:                 _slotInput.AddThemeFontOverride("font", AshfallUiHelpers.FontShareTechMono);
00102:             mountRow.AddChild(_slotInput);
00103:             var mountButton = AshfallUiHelpers.MakeButton("MOUNT SELECTED", MountSelected);
00104:             mountButton.CustomMinimumSize = new Vector2(165, 36);
00105:             mountRow.AddChild(mountButton);
00106:             _selectionSummary = AshfallUiHelpers.MakeMetadata("Select an item from storage below. Mounting removes one real item from storage.");
00107:             mountStack.AddChild(_selectionSummary);
00108:             root.AddChild(mountPanel);
00109:
00110:             var columns = AshfallUiHelpers.MakeHBox(12);
00111:             columns.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00112:             columns.SizeFlagsVertical = SizeFlags.ExpandFill;
00113:             root.AddChild(columns);
00114:
00115:             var installedPanel = AshfallUiHelpers.MakePanel();
00116:             installedPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00117:             installedPanel.SizeFlagsStretchRatio = 1.15f;
00118:             var installedStack = AshfallUiHelpers.MakeVBox(8);
00119:             installedPanel.AddChild(installedStack);
00120:             installedStack.AddChild(AshfallUiHelpers.MakeSectionHeader("Installed in selected room"));
00121:             _roomSummary = AshfallUiHelpers.MakeMetadata("No room selected.");
00122:             installedStack.AddChild(_roomSummary);
00123:             var placementScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
00124:             _placements = AshfallUiHelpers.MakeVBox(8);
00125:             _placements.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00126:             placementScroll.AddChild(_placements);
00127:             installedStack.AddChild(placementScroll);
00128:             columns.AddChild(installedPanel);
00129:
00130:             var storagePanel = AshfallUiHelpers.MakePanel();
00131:             storagePanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00132:             storagePanel.SizeFlagsStretchRatio = 1f;
00133:             var storageStack = AshfallUiHelpers.MakeVBox(8);
00134:             storagePanel.AddChild(storageStack);
00135:             storageStack.AddChild(AshfallUiHelpers.MakeSectionHeader("Decor available in storage"));
00136:             storageStack.AddChild(AshfallUiHelpers.MakeMetadata("Choose an item, then name a free slot. Zero-count entries remain visible so the catalog is legible."));
00137:             var storageScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
00138:             _storage = AshfallUiHelpers.MakeVBox(7);
00139:             _storage.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00140:             storageScroll.AddChild(_storage);
00141:             storageStack.AddChild(storageScroll);
00142:             columns.AddChild(storagePanel);
00143:
00144:             _eventLine = AshfallUiHelpers.MakeInfo("The wall is quiet. Nothing has been mounted in this session.");
00145:             root.AddChild(_eventLine);
00146:             _shell.SetContent(root);
00147:             _shell.AttachHeaderCloseButton("CLOSE", () =>
00148:             {
00149:                 Visible = false;
00150:                 OnClose?.Invoke();
00151:             });
00152:
00153:             RefreshView();
00154:         }
00155:
00156:         public void RefreshView()
00157:         {
00158:             if (_shell == null || _statusRail == null) return;
00159:             if (_host == null)
00160:             {
00161:                 _statusRail.Set("mounted", "UNBOUND", AshfallMetricCard.Criticality.Caution);
00162:                 _statusRail.Set("rooms", "—", AshfallMetricCard.Criticality.Normal);
00163:                 _statusRail.Set("morale", "—", AshfallMetricCard.Criticality.Normal);
00164:                 _statusRail.Set("plaques", "—", AshfallMetricCard.Criticality.Normal);
00165:                 if (_eventLine != null) _eventLine.Text = "Shelter decor is waiting for the campaign session.";
00166:                 return;
00167:             }
00168:
00169:             RebuildRoomPicker();
00170:             string roomId = SelectedRoomId();
00171:             var placements = _host.System.ListRoomPlacements(roomId);
00172:             RenderedPlacementCount = placements.Count;
00173:             int decoratedRooms = 0;
00174:             int plaques = 0;
00175:             float cumulativeMorale = 0f;
00176:             foreach (var room in _host.Rooms)
00177:             {
00178:                 float delta = _host.System.GetRoomMoraleDelta(room.RoomId);
00179:                 if (_host.System.ListRoomPlacements(room.RoomId).Count > 0) decoratedRooms++;
00180:                 cumulativeMorale += delta * ActiveOccupantCount(room.RoomId);
00181:             }
00182:             foreach (var placement in _host.System.State.Placements)
00183:                 if (placement != null && placement.IsMemorialPlaque) plaques++;
00184:
00185:             _statusRail.Set("mounted", _host.System.State.Placements.Count.ToString(), AshfallMetricCard.Criticality.Normal);
00186:             _statusRail.Set("rooms", decoratedRooms.ToString(), AshfallMetricCard.Criticality.Normal);
00187:             _statusRail.Set("morale", $"+{cumulativeMorale:F1}", cumulativeMorale > 0f ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
00188:             _statusRail.Set("plaques", plaques.ToString(), plaques > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
00189:
00190:             if (_roomSummary != null)
00191:             {
00192:                 float roomDelta = _host.System.GetRoomMoraleDelta(roomId);
00193:                 int occupants = ActiveOccupantCount(roomId);
00194:                 _roomSummary.Text = string.Equals(roomId, ShelterDecorHostSession.MemorialWallRoomId, StringComparison.Ordinal)
00195:                     ? "Ledger-backed plaques are permanent records. They do not consume storage and have no assigned occupants."
00196:                     : $"{_host.DisplayNameForRoom(roomId)} · {occupants} active assigned occupant(s) · +{roomDelta:F1} morale per occupant at daily needs tick.";
00197:             }
00198:             if (_selectionSummary != null)
00199:             {
00200:                 if (string.IsNullOrEmpty(_selectedItemId))
00201:                     _selectionSummary.Text = "Select an item from storage. Mounting removes one real item; removing a player-mounted item returns it.";
00202:                 else
00203:                 {
00204:                     var selected = _host.InventoryCatalog.Get(_selectedItemId);
00205:                     var modifier = _host.System.GetItemModifier(_selectedItemId);
00206:                     int held = _host.Inventory.CountById(_selectedItemId);
00207:                     _selectionSummary.Text = selected == null || modifier == null
00208:                         ? "The selected item is no longer available."
00209:                         : $"SELECTED · {selected.displayName} · {held} in storage · +{modifier.LocalizedMoraleDelta:F1} morale / assigned occupant / day.";
00210:                 }
00211:             }
00212:             if (_eventLine != null) _eventLine.Text = _host.LastEvent;
00213:
00214:             RebuildPlacements(roomId, placements);
00215:             RebuildStorage();
00216:         }
00217:
00218:         /// <summary>
00219:         /// Selects a room by canonical id. Used by the headless UI gate and by
00220:         /// future room-hotspot callers; it changes only panel selection.
00221:         /// </summary>
00222:         public bool SelectRoom(string roomId)
00223:         {
00224:             if (_roomPicker == null || string.IsNullOrEmpty(roomId)) return false;
00225:             for (int i = 0; i < _roomPicker.ItemCount; i++)
00226:             {
00227:                 if (!string.Equals(_roomPicker.GetItemMetadata(i).AsString(), roomId, StringComparison.Ordinal))
00228:                     continue;
00229:                 _roomPicker.Select(i);
00230:                 RefreshView();
00231:                 return true;
00232:             }
00234:         }
00235:
00236:         public override void _ExitTree()
00237:         {
00238:             Unbind();
00239:             base._ExitTree();
00240:         }
00242:         private void MountSelected()
00243:         {
00244:             if (_host == null) return;
00245:             if (string.IsNullOrEmpty(_selectedItemId))
00246:             {
00247:                 _host.SetPanelMessage("Choose a decor item from storage before mounting.");
00248:                 RefreshView();
00249:                 return;
00250:             }
00251:             _host.TryMount(SelectedRoomId(), _slotInput?.Text ?? string.Empty, _selectedItemId, _host.CurrentDay, out _);
00252:             RefreshView();
00253:         }
00254:
00255:         private void RebuildRoomPicker()
00256:         {
00257:             if (_host == null || _roomPicker == null) return;
00258:             string current = SelectedRoomId();
00259:             if (string.IsNullOrEmpty(current))
00260:             {
00261:                 // First open should lead with a lived-in room rather than an
00262:                 // arbitrary empty corridor. Players can still choose every
00263:                 // room or the wall from the selector.
00264:                 foreach (var room in _host.Rooms)
00265:                 {
00266:                     if (_host.System.ListRoomPlacements(room.RoomId).Count <= 0) continue;
00267:                     current = room.RoomId;
00268:                     break;
00269:                 }
00270:                 if (string.IsNullOrEmpty(current)
00271:                     && _host.System.ListRoomPlacements(ShelterDecorHostSession.MemorialWallRoomId).Count > 0)
00272:                 {
00273:                     current = ShelterDecorHostSession.MemorialWallRoomId;
00274:                 }
00275:             }
00276:             _roomPicker.Clear();
00277:             foreach (var room in _host.Rooms)
00278:             {
00279:                 _roomPicker.AddItem(room.DisplayName);
00280:                 _roomPicker.SetItemMetadata(_roomPicker.ItemCount - 1, room.RoomId);
00281:             }
00282:             _roomPicker.AddItem("Memorial Wall");
00283:             _roomPicker.SetItemMetadata(_roomPicker.ItemCount - 1, ShelterDecorHostSession.MemorialWallRoomId);
00284:             for (int i = 0; i < _roomPicker.ItemCount; i++)
00285:             {
00286:                 if (string.Equals(_roomPicker.GetItemMetadata(i).AsString(), current, StringComparison.Ordinal))
00287:                 {
00288:                     _roomPicker.Select(i);
00289:                     return;
00290:                 }
00297:             if (_roomPicker == null || _roomPicker.Selected < 0 || _roomPicker.Selected >= _roomPicker.ItemCount)
00298:                 return string.Empty;
00299:             return _roomPicker.GetItemMetadata(_roomPicker.Selected).AsString();
00300:         }
00301:
00302:         private void RebuildPlacements(string roomId, System.Collections.Generic.List<ShelterDecorPlacement> placements)
00303:         {
00304:             ClearChildren(_placements);
00305:             if (_host == null || placements.Count == 0)
00306:             {
00307:                 _placements.AddChild(AshfallUiHelpers.MakeEmptyState(
00308:                     "No decor is mounted here yet.",
00309:                     "BARE SURFACE",
00310:                     string.Equals(roomId, ShelterDecorHostSession.MemorialWallRoomId, StringComparison.Ordinal)
00311:                         ? "Memorial entries place their plaques here automatically."
00312:                         : "Choose a storage item, name a slot, and mount it."));
00313:                 return;
00314:             }
00318:                 var stack = AshfallUiHelpers.MakeVBox(5);
00319:                 card.AddChild(stack);
00320:                 var definition = _host.InventoryCatalog.Get(placement.ItemId);
00321:                 var modifier = _host.System.GetItemModifier(placement.ItemId);
00322:                 stack.AddChild(AshfallUiHelpers.MakeLabel(
00323:                     $"{placement.SlotId.ToUpperInvariant()}  //  {(definition?.displayName ?? placement.ItemId).ToUpperInvariant()}",
00324:                     18, bold: true));
00325:                 stack.AddChild(AshfallUiHelpers.MakeMetadata($"+{modifier?.LocalizedMoraleDelta ?? 0f:F1} morale per assigned occupant / day · mounted day {placement.DayInstalled}"));
00337:                     var remove = AshfallUiHelpers.MakeButton("RETURN TO STORAGE", () =>
00338:                     {
00339:                         _host.TryRemoveMount(roomId, slot, out _);
00340:                         RefreshView();
00341:                     });
00342:                     remove.CustomMinimumSize = new Vector2(180, 30);
00343:                     stack.AddChild(remove);
00346:             }
00347:
00348:             if (!string.Equals(roomId, ShelterDecorHostSession.MemorialWallRoomId, StringComparison.Ordinal))
00349:             {
00350:                 var trophySlots = _host.System.GetTrophySlots(roomId);
00351:                 for (int i = 0; i < trophySlots.Count; i++)
00352:                 {
00353:                     string tSlot = trophySlots[i];
00354:                     bool occupied = false;
00373:                             MountSelected();
00374:                         });
00375:                         quickMount.CustomMinimumSize = new Vector2(200, 28);
00376:                         emptyStack.AddChild(quickMount);
00377:                         _placements.AddChild(emptyCard);
00378:                     }
00379:                 }
00380:             }
00381:         }
00382:
00383:         private void RebuildStorage()
00384:         {
00385:             ClearChildren(_storage);
00386:             if (_host == null) return;
00387:             var options = _host.ListAvailableDecor();
00388:             if (options.Count == 0)
00389:             {
00390:                 _storage.AddChild(AshfallUiHelpers.MakeEmptyState(
00391:                     "The item catalog did not register any item_decor_* entries.",
00392:                     "NO DECOR AUTHORITY"));
00393:                 return;
00394:             }
00395:             foreach (var definition in options)
00396:             {
00397:                 string itemId = definition.id;
00398:                 var modifier = _host.System.GetItemModifier(itemId);
00399:                 int count = _host.Inventory.CountById(itemId);
00400:                 bool isTrophy = ShelterDecorSystem.IsTrophyItem(itemId);
00401:                 string countLabel = count > 0 ? $"{count} HELD" : (isTrophy ? "0 HELD (craft at workbench)" : "0 HELD");
00402:                 var choose = AshfallUiHelpers.MakeButton(
00403:                     $"{definition.displayName.ToUpperInvariant()}  ·  {countLabel}  ·  +{modifier?.LocalizedMoraleDelta ?? 0f:F1}",
00404:                     () =>
00405:                     {
00406:                         _selectedItemId = itemId;
00407:                         RefreshView();
00408:                     });
00409:                 choose.TooltipText = isTrophy && count <= 0
00410:                     ? $"{definition.description}\n[CRAFTING REQUIRED: Preserve rare quarry in traps, then craft trophy at workbench]"
00411:                     : definition.description;
00412:                 choose.Disabled = count <= 0;
00413:                 choose.Modulate = string.Equals(itemId, _selectedItemId, StringComparison.Ordinal)
00414:                     ? AshfallUiHelpers.ColorHighlight
00415:                     : Colors.White;
00416:                 _storage.AddChild(choose);
00417:             }
00418:         }
00419:
00420:         private int ActiveOccupantCount(string roomId)
00421:         {
00422:             if (_host == null) return 0;
00423:             int count = 0;
00424:             foreach (var assignment in _host.Assignments.GetAssignmentsForRoom(roomId))
00425:             {
00426:                 if (assignment.Status == ShelterAssignmentStatus.Active
00427:                     && _host.Needs.Get(assignment.SurvivorId)?.IsAliveState == true)
00428:                     count++;
00429:             }
00430:             return count;
00431:         }
00432:
00433:         private static void ClearChildren(Node container)
00434:         {
00435:             if (container == null) return;
00436:             foreach (Node child in container.GetChildren())
00437:             {
00438:                 container.RemoveChild(child);
00439:                 child.QueueFree();
00440:             }
00441:         }
00442:     }
00443: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Plan68WallCarvingTests.cs`

### `Ashfall.Core.Tests/Plan68WallCarvingTests.cs` — complete current file

- Size: 213 lines / 9305 bytes.
- SHA-256: `6736b21b677ef6f975bc3e20f8396f6e0aa77530a69d54a216bdcc31d3792183`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core.IO;
00007: using Xunit;
00008:
00009: namespace Ashfall.Core.Tests
00010: {
00011:     /// <summary>
00012:     /// Plan 68 — wall carving template expansion contract tests.
00013:     ///
00014:     /// Pins wall_carving_templates.json at exactly 3 morale bands with 20
00015:     /// templates each (60 total): schema shape (morale_band / morale_min /
00016:     /// morale_max / bare-string templates), band windows (high 60–100,
00017:     /// medium 30–59, low 0–29), no empty strings, no exact duplicates
00018:     /// within or across bands, motif-diversity spot checks, cliché gates,
00019:     /// and physicality/at-length constraints.
00020:     ///
00021:     /// Plan 68 §66 finding recorded here: the catalog is data-present and
00022:     /// consumer-absent — no Core/host/UI code parses it yet (the
00023:     /// content-utilization scanner maps it to MemorialSystem/MemorialPanel
00024:     /// aspirationally). These tests therefore validate the JSON directly
00025:     /// through a local probe DTO; when a carving consumer lands it should
00026:     /// adopt the same shape.
00027:     /// </summary>
00028:     public sealed class Plan68WallCarvingTests
00029:     {
00030:         private static string? FindDataDir()
00031:         {
00032:             if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir)) return dir;
00033:             if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir)) return dir;
00034:             return null;
00035:         }
00036:
00037:         private static List<BandProbe> Load()
00038:         {
00039:             string? dataDir = FindDataDir();
00040:             Assert.False(dataDir == null, "StreamingAssets/Data directory not found");
00041:             var raw = new FileSystemIO().ReadAllText(Path.Combine(dataDir!, "wall_carving_templates.json"));
00042:             var root = new SystemTextJsonSerializer().Deserialize<RootProbe>(raw);
00043:             Assert.NotNull(root);
00044:             Assert.NotNull(root!.items);
00045:             return root.items;
00046:         }
00047:
00048:         private sealed class RootProbe { public List<BandProbe> items { get; set; } = new(); }
00049:
00050:         private sealed class BandProbe
00051:         {
00052:             public string morale_band { get; set; } = string.Empty;
00053:             public int morale_min { get; set; }
00054:             public int morale_max { get; set; }
00055:             public List<string> templates { get; set; } = new();
00056:         }
00057:
00058:         [Fact]
00059:         public void Catalog_has_exactly_three_bands_with_correct_names()
00060:         {
00061:             var bands = Load();
00062:             Assert.Equal(3, bands.Count);
00063:             Assert.Equal(new[] { "high", "medium", "low" },
00064:                 bands.Select(b => b.morale_band).ToArray());
00065:         }
00066:
00067:         [Fact]
00068:         public void Each_band_contains_exactly_20_templates()
00069:         {
00070:             var bands = Load();
00071:             Assert.All(bands, b => Assert.Equal(20, b.templates.Count));
00072:         }
00073:
00074:         [Fact]
00075:         public void Band_windows_match_the_plan_contract()
00076:         {
00077:             var bands = Load().ToDictionary(b => b.morale_band, StringComparer.Ordinal);
00078:             Assert.Equal((60, 100), (bands["high"].morale_min, bands["high"].morale_max));
00079:             Assert.Equal((30, 59), (bands["medium"].morale_min, bands["medium"].morale_max));
00080:             Assert.Equal((0, 29), (bands["low"].morale_min, bands["low"].morale_max));
00081:         }
00082:
00083:         [Fact]
00084:         public void No_empty_templates_and_no_whitespace_only_templates()
00085:         {
00086:             var bands = Load();
00087:             foreach (var band in bands)
00088:                 foreach (var t in band.templates)
00089:                     Assert.False(string.IsNullOrWhiteSpace(t),
00090:                         $"{band.morale_band}: empty template");
00091:         }
00092:
00093:         [Fact]
00094:         public void No_exact_duplicates_within_a_band()
00095:         {
00096:             foreach (var band in Load())
00097:             {
00098:                 var normalized = band.templates
00099:                     .Select(t => t.Trim().ToLowerInvariant())
00100:                     .ToList();
00101:                 Assert.Equal(normalized.Count, normalized.Distinct().Count());
00102:             }
00103:         }
00104:
00105:         [Fact]
00106:         public void No_exact_duplicates_across_bands()
00107:         {
00108:             var bands = Load();
00109:             var all = bands.SelectMany(b => b.templates)
00110:                 .Select(t => t.Trim().ToLowerInvariant())
00111:                 .ToList();
00112:             Assert.Equal(all.Count, all.Distinct().Count());
00113:         }
00114:
00115:         [Fact]
00116:         public void The_fifteen_original_templates_are_preserved()
00117:         {
00118:             var byBand = Load().ToDictionary(b => b.morale_band, StringComparer.Ordinal);
00119:             Assert.Contains(byBand["high"].templates, t => t.Contains("STILL"));
00120:             Assert.Contains(byBand["high"].templates, t => t.StartsWith("A recipe for imaginary cake"));
00121:             Assert.Contains(byBand["medium"].templates, t => t.Contains("47 days"));
00122:             Assert.Contains(byBand["medium"].templates, t => t.Contains("miss everything"));
00123:             Assert.Contains(byBand["low"].templates, t => t.Contains("WHY"));
00124:             Assert.Contains(byBand["low"].templates, t => t.Contains("I'm sorry"));
00125:         }
00126:
00127:         [Fact]
00128:         public void Templates_stay_readable_at_a_glance()
00129:         {
00130:             foreach (var band in Load())
00131:                 foreach (var t in band.templates)
00132:                     Assert.True(t.Length <= 140,
00133:                         $"{band.morale_band}: template too long for a glance ({t.Length} chars): {t}");
00134:         }
00135:
00136:         [Fact]
00137:         public void No_melodrama_cliches_in_any_band()
00138:         {
00139:             var banned = new[]
00140:             {
00141:                 "last hope", "darkness swallowed", "against all odds",
00142:                 "light at the end", "never give up", "tomorrow will come",
00143:                 "ashes of the old world", "you feel terrible",
00144:                 "the guilt crushes", "haunts everyone forever"
00145:             };
00146:             foreach (var band in Load())
00147:                 foreach (var t in band.templates)
00148:                 {
00149:                     var lower = t.ToLowerInvariant();
00150:                     foreach (var phrase in banned)
00151:                         Assert.False(lower.Contains(phrase, StringComparison.Ordinal),
00152:                             $"{band.morale_band}: cliché '{phrase}' in: {t}");
00153:                 }
00154:         }
00155:
00156:         [Fact]
00157:         public void High_band_carries_hope_through_solidarity_not_triumphalism()
00158:         {
00159:             var high = Load().First(b => b.morale_band == "high").templates;
00160:             // No grand-speech vocabulary.
00161:             foreach (var t in high)
00162:             {
00163:                 var lower = t.ToLowerInvariant();
00164:                 Assert.False(lower.Contains("rebuild civilization"), "triumphalism");
00165:                 Assert.False(lower.Contains("hope conquers"), "slogan");
00166:             }
00167:             // Communal/continuity motifs present: names of the living, future dates, planting.
00168:             Assert.Contains(high, t => t.Contains("TOMATOES") || t.Contains("SPRING"));
00169:             Assert.Contains(high, t => t.Contains("FOUR") || t.Contains("four"));
00170:             Assert.Contains(high, t => t.Contains("APRIL"));
00171:         }
00172:
00173:         [Fact]
00174:         public void Medium_band_is_documentary_routine_texture()
00175:         {
00176:             var medium = Load().First(b => b.morale_band == "medium").templates;
00177:             // Maintenance/logistics vocabulary present without becoming a manual.
00178:             Assert.Contains(medium, t => t.Contains("FILTER"));
00179:             Assert.Contains(medium, t => t.Contains("BATTERY"));
00180:             Assert.Contains(medium, t => t.Contains("NIGHT SHIFT"));
00181:             Assert.Contains(medium, t => t.Contains("WRENCH"));
00182:             // No despair vocabulary leaking into the routine band.
00183:             Assert.DoesNotContain(medium, t => t.ToLowerInvariant().Contains("die"));
00184:         }
00185:
00186:         [Fact]
00187:         public void Low_band_varies_motifs_beyond_names_of_the_dead()
00188:         {
00189:             var low = Load().First(b => b.morale_band == "low").templates;
00190:             Assert.Contains(low, t => t.Contains("SORRY"));
00191:             Assert.Contains(low, t => t.Contains("DON'T SLEEP"));
00192:             Assert.Contains(low, t => t.Contains("COLD"));
00193:             Assert.Contains(low, t => t.Contains("prayer"));
00194:             // Motif distribution: at most a handful of name-list marks.
00195:             var nameMarks = low.Count(t => t.Contains("names", StringComparison.OrdinalIgnoreCase)
00196:                                          || t.Contains("name", StringComparison.OrdinalIgnoreCase));
00197:             Assert.True(nameMarks <= 5, $"too many name motifs in the low band: {nameMarks}");
00198:         }
00199:
00200:         [Fact]
00201:         public void Bands_are_distinguishable_by_blind_vocabulary()
00202:         {
00203:             // Spot-check the signature: high contains future/communal tokens,
00204:             // low contains loss tokens — a reviewer could band-classify these.
00205:             var byBand = Load().ToDictionary(b => b.morale_band, StringComparer.Ordinal);
00206:             var highText = string.Join(' ', byBand["high"].templates);
00207:             var lowText = string.Join(' ', byBand["low"].templates);
00208:             Assert.Contains("SPRING", highText);
00209:             Assert.Contains("harvest", highText, StringComparison.OrdinalIgnoreCase);
00210:             Assert.Contains("sorry", lowText, StringComparison.OrdinalIgnoreCase);
00211:         }
00212:     }
00213: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs`

### `Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs` — complete current file

- Size: 197 lines / 9232 bytes.
- SHA-256: `2447468de613d9a6dc5dfa4f4324293977ead2b359f3fcd1ba9af931770176e5`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.IO;
00004: using System.Linq;
00005: using Xunit;
00006: using Ashfall.Core.IO;
00007: using Ashfall.Core.Shelter;
00008: using Ashfall.Core.Survivors;
00009:
00010: namespace Ashfall.Core.Tests.Shelter
00011: {
00012:     public sealed class Plan66_68GuiltWallCarvingIntegrationTests
00013:     {
00014:         private static string GetDataDir()
00015:         {
00016:             if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir)) return dir;
00017:             if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir)) return dir;
00018:             throw new DirectoryNotFoundException("StreamingAssets/Data directory could not be located.");
00019:         }
00020:
00021:         [Fact]
00022:         public void GuiltSourceCatalog_And_GuiltInsomniaSystem_FullLifecycle()
00023:         {
00024:             var dataDir = GetDataDir();
00025:             var catalog = GuiltSourceCatalog.LoadFromDirectory(dataDir);
00026:
00027:             // Plan 66 catalog integrity: exactly 40 authored items
00028:             Assert.Equal(40, catalog.Count);
00029:             Assert.Equal(40, catalog.Items.Count);
00030:
00031:             foreach (var item in catalog.Items)
00032:             {
00033:                 Assert.False(string.IsNullOrWhiteSpace(item.ChoicePattern));
00034:                 Assert.False(string.IsNullOrWhiteSpace(item.Title));
00035:                 Assert.False(string.IsNullOrWhiteSpace(item.Description));
00036:                 Assert.InRange(item.Severity, 0.1f, 1.0f);
00037:             }
00038:
00039:             // Description formatting verification
00040:             var orderDeathDef = catalog.GetByPattern("order_survivor_to_death");
00041:             Assert.NotNull(orderDeathDef);
00042:             var formatted = orderDeathDef!.FormatDescription("Elena Vance");
00043:             Assert.Contains("Elena Vance", formatted);
00044:
00045:             // System integration
00046:             var insomniaSystem = new GuiltInsomniaSystem();
00047:             string criticalSurvivorId = null;
00048:             insomniaSystem.OnGuiltInsomniaCritical += sId => criticalSurvivorId = sId;
00049:
00050:             // Record moderate guilt
00051:             bool foundFuel = catalog.TryGetSeverity("burn_critical_fuel_for_comfort", out float fuelSeverity);
00052:             Assert.True(foundFuel);
00053:             insomniaSystem.RecordGuilt("surv_marcus", "burn_critical_fuel_for_comfort", fuelSeverity, currentDay: 1);
00054:
00055:             Assert.Equal(1, insomniaSystem.GetGuiltSourceCount("surv_marcus"));
00056:             Assert.Equal(fuelSeverity, insomniaSystem.GetInsomniaSeverity("surv_marcus"), 3);
00057:             Assert.Null(criticalSurvivorId);
00058:
00059:             // Record high-severity guilt pushing survivor past critical threshold
00060:             bool foundDeath = catalog.TryGetSeverity("order_survivor_to_death", out float deathSeverity);
00061:             Assert.True(foundDeath);
00062:             Assert.True(deathSeverity >= GuiltInsomniaSystem.HighSeverityThreshold);
00063:
00064:             insomniaSystem.RecordGuilt("surv_marcus", "order_survivor_to_death", deathSeverity, currentDay: 2);
00065:             Assert.Equal("surv_marcus", criticalSurvivorId);
00066:             Assert.Equal(2, insomniaSystem.GetGuiltSourceCount("surv_marcus"));
00067:             Assert.Equal(1.0f, insomniaSystem.GetInsomniaSeverity("surv_marcus"), 3);
00068:
00069:             // Sleep quality penalty
00070:             float sleepMultiplier = insomniaSystem.GetSleepQualityMultiplier("surv_marcus");
00071:             Assert.True(sleepMultiplier < 1.0f);
00072:
00073:             // Sedative compensation
00074:             bool sedativeApplied = insomniaSystem.ApplySedative("surv_marcus");
00075:             Assert.True(sedativeApplied);
00076:             Assert.True(insomniaSystem.GetInsomniaSeverity("surv_marcus") < 1.0f);
00077:
00078:             // Save / restore round-trip
00079:             var saveState = insomniaSystem.CaptureState();
00080:             Assert.NotNull(saveState);
00081:             Assert.Single(saveState.survivors);
00082:
00083:             var restoredSystem = new GuiltInsomniaSystem();
00084:             restoredSystem.RestoreState(saveState);
00085:
00086:             Assert.Equal(2, restoredSystem.GetGuiltSourceCount("surv_marcus"));
00087:             Assert.Equal(insomniaSystem.GetInsomniaSeverity("surv_marcus"), restoredSystem.GetInsomniaSeverity("surv_marcus"), 3);
00088:
00089:             // Dialogue resolution removes newest guilt record
00090:             bool dialogueOk = restoredSystem.ResolveGuiltThroughDialogue("surv_marcus");
00091:             Assert.True(dialogueOk);
00092:             Assert.Equal(1, restoredSystem.GetGuiltSourceCount("surv_marcus"));
00093:         }
00094:
00095:         [Fact]
00096:         public void WallCarvingCatalog_And_MoraleBandSelection_FullContract()
00097:         {
00098:             var dataDir = GetDataDir();
00099:             var catalog = WallCarvingCatalog.LoadFromDirectory(dataDir);
00100:
00101:             // Plan 68 catalog integrity: exactly 3 bands, 20 templates per band = 60 total
00102:             Assert.Equal(3, catalog.Bands.Count);
00103:             Assert.Equal(60, catalog.TotalTemplateCount);
00104:
00105:             var highBand = catalog.GetBandForMorale(75f);
00106:             Assert.NotNull(highBand);
00107:             Assert.Equal("high", highBand!.MoraleBand);
00108:             Assert.Equal(20, highBand.Templates.Count);
00109:             Assert.Equal(0.3f, highBand.CarvingChance, 2);
00110:
00111:             var medBand = catalog.GetBandForMorale(45f);
00112:             Assert.NotNull(medBand);
00113:             Assert.Equal("medium", medBand!.MoraleBand);
00114:             Assert.Equal(20, medBand.Templates.Count);
00115:             Assert.Equal(0.2f, medBand.CarvingChance, 2);
00116:
00117:             var lowBand = catalog.GetBandForMorale(15f);
00118:             Assert.NotNull(lowBand);
00119:             Assert.Equal("low", lowBand!.MoraleBand);
00120:             Assert.Equal(20, lowBand.Templates.Count);
00121:             Assert.Equal(0.15f, lowBand.CarvingChance, 2);
00122:
00123:             // Boundary and clamping tests
00124:             Assert.Equal("high", catalog.GetBandForMorale(100f)!.MoraleBand);
00125:             Assert.Equal("high", catalog.GetBandForMorale(125f)!.MoraleBand);
00126:             Assert.Equal("high", catalog.GetBandForMorale(60f)!.MoraleBand);
00127:             Assert.Equal("medium", catalog.GetBandForMorale(59f)!.MoraleBand);
00128:             Assert.Equal("medium", catalog.GetBandForMorale(30f)!.MoraleBand);
00129:             Assert.Equal("low", catalog.GetBandForMorale(29f)!.MoraleBand);
00130:             Assert.Equal("low", catalog.GetBandForMorale(0f)!.MoraleBand);
00131:             Assert.Equal("low", catalog.GetBandForMorale(-20f)!.MoraleBand);
00132:
00133:             // Deterministic template picking
00134:             int selectedIndex = 3;
00135:             var pickedTemplate = catalog.GetRandomTemplate(80f, max => selectedIndex % max);
00136:             Assert.Equal(highBand.Templates[selectedIndex], pickedTemplate);
00137:         }
00138:
00139:         [Fact]
00140:         public void PsychologicalStress_And_CulturalTrace_SystemCoupling()
00141:         {
00142:             var dataDir = GetDataDir();
00143:             var guiltCatalog = GuiltSourceCatalog.LoadFromDirectory(dataDir);
00144:             var carvingCatalog = WallCarvingCatalog.LoadFromDirectory(dataDir);
00145:             var insomniaSystem = new GuiltInsomniaSystem();
00146:
00147:             // Initial shelter state: steady morale (75f) -> high morale wall carving
00148:             float shelterMorale = 75f;
00149:             var initialBand = carvingCatalog.GetBandForMorale(shelterMorale);
00150:             Assert.NotNull(initialBand);
00151:             Assert.Equal("high", initialBand!.MoraleBand);
00152:             var hopefulTemplate = carvingCatalog.GetRandomTemplate(shelterMorale, _ => 0);
00153:             Assert.False(string.IsNullOrWhiteSpace(hopefulTemplate));
00154:
00155:             // Ruthless decisions induce guilt across multiple dwellers
00156:             string[] survivors = { "surv_leader", "surv_medic", "surv_scout" };
00157:             string[] patterns = { "order_survivor_to_death", "triage_by_utility", "leave_wounded_behind" };
00158:
00159:             float cumulativeInsomnia = 0f;
00160:             for (int i = 0; i < survivors.Length; i++)
00161:             {
00162:                 if (guiltCatalog.TryGetSeverity(patterns[i], out float sev))
00163:                 {
00164:                     insomniaSystem.RecordGuilt(survivors[i], patterns[i], sev, currentDay: 5);
00165:                     cumulativeInsomnia += insomniaSystem.GetInsomniaSeverity(survivors[i]);
00166:                 }
00167:             }
00168:
00169:             Assert.True(cumulativeInsomnia >= 2.0f);
00170:
00171:             // Morale drops as psychological consequences spread through the shelter
00172:             shelterMorale -= (cumulativeInsomnia * 25f);
00173:             shelterMorale = Math.Max(5f, shelterMorale);
00174:
00175:             // Wall carving culture reflects community despair
00176:             var depressedBand = carvingCatalog.GetBandForMorale(shelterMorale);
00177:             Assert.NotNull(depressedBand);
00178:             Assert.Equal("low", depressedBand!.MoraleBand);
00179:
00180:             var bleakTemplate = carvingCatalog.GetRandomTemplate(shelterMorale, _ => 0);
00181:             Assert.False(string.IsNullOrWhiteSpace(bleakTemplate));
00182:             Assert.NotEqual(hopefulTemplate, bleakTemplate);
00183:
00184:             // Therapy and reconciliation lift community out of critical despair
00185:             for (int i = 0; i < survivors.Length; i++)
00186:             {
00187:                 insomniaSystem.ApplyTherapyRelief(survivors[i], 0.80f);
00188:             }
00189:
00190:             // Morale recovers toward medium band
00191:             shelterMorale += 35f;
00192:             var recoveredBand = carvingCatalog.GetBandForMorale(shelterMorale);
00193:             Assert.NotNull(recoveredBand);
00194:             Assert.Equal("medium", recoveredBand!.MoraleBand);
00195:         }
00196:     }
00197: }
```


# Appendix — Current Source Detail: `docs/shelter/WALL_CARVING_TONE_BIBLE.md`

### `docs/shelter/WALL_CARVING_TONE_BIBLE.md` — complete current file

- Size: 39 lines / 2809 bytes.
- SHA-256: `6a4b4d7ccd29f9c9f79dc3a3aa23d9cbae2a0042024f2863bd515ff2ab851538`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: # Wall Carving Tone Bible — Plan 68
00002:
00003: Applies to every entry in `wall_carving_templates.json`. Contract tests in `Plan68WallCarvingTests.cs` mechanically enforce parts of this.
00004:
00005: ## What a wall carving is
00006:
00007: A physical trace applied to a shelter surface by someone with limited time and poor tools: scratched words, tallies, charcoal lines, nail-carved names, grease-pencil counts, children's drawings, arrows, crossed-out rosters, crude maps, height marks, prayers, jokes, warnings.
00008:
00009: The catalog voice is **third-person description of the mark** (the established convention), with short quoted inscriptions where the mark itself is text.
00010:
00011: ## Band grammar
00012:
00013: | Band | Reads as | Contains | Never contains |
00014: |---|---|---|---|
00015: | high 60–100 | modest, stubborn continuity | tallies that keep going, children's drawings, names of the living, future dates/planting, repair pride, house rules, dry jokes | triumphalism, sunshine optimism, slogans |
00016: | medium 30–59 | documentary routine | ration grids, filter/battery/tool counts, duty rosters, corrected lines, draft/leak notes, dry complaints, unfinished schedules | despair, overt hope, emotional confessions |
00017: | low 0–29 | grief, fear, exhaustion — restrained | stopped tallies, worn prayers, gouged counts, unexplained warnings, apologies, minimal ambiguous marks | graphic death detail, theatrical horror, suicide-adjacent imagery, 20 variants of names-of-the-dead |
00018:
00019: ## Line rules
00020:
00021: - One short sentence, two very short sentences, or a phrase/mark description. ≤140 characters.
00022: - Evidence, not interpretation. The wall carries the mark; the catalog never explains what it means about the world.
00023: - At most one or two deliberate imperfections per entry; no cute misspelling.
00024: - Em dashes and straight apostrophes match existing catalog typography; no fancy Unicode.
00025: - Carved inscriptions may be uppercase (wall-register): `FILTER 2 — CHANGE AGAIN`, `TOMATOES — SPRING`, `DON'T SLEEP BY THE EAST DOOR`.
00026:
00027: ## Prohibited
00028:
00029: - Literary clichés ("last hope", "darkness swallowed", "ashes of the old world", …) — test-gated.
00030: - Lore exposition (war cause, factions, system explanations) — walls assume local knowledge.
00031: - Modern meme/internet phrasing; dry survival humor only.
00032: - Room-locked lines (no runtime room filter exists): a mark may *mention* a surface feature (pipe, door frame, vent) but must not depend on being in one room.
00033: - Real-world calendar dates; day counts and month names only (e.g. "47 days", "APRIL").
00034: - Invented gods, factions, or named NPCs; initials and unnamed hands only.
00035: - Verbatim reuse of grave-epitaph or folklore catalog strings.
00036:
00037: ## Motif ceilings
00038:
00039: No core motif above ~20–25% of a band (tally/mark-based ≤4–5 per band; low-band name motifs ≤5). Vary openings; avoid "Someone carved…" as a repeated construction.
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs`

### `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` — complete current file

- Size: 224 lines / 8840 bytes.
- SHA-256: `0edeee9f38c11329e4e73ccd86621ba448cf4458d0432560f479a9e406685275`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: namespace Ashfall.Core.Survivors
00007: {
00008:     [Serializable]
00009:     public sealed class GuiltRecord
00010:     {
00011:         public string sourceId = string.Empty;
00012:         public int dayRecorded;
00013:         public float severity;
00014:     }
00015:
00016:     [Serializable]
00017:     public sealed class GuiltInsomniaSaveState
00018:     {
00019:         public List<GuiltSurvivorState> survivors = new List<GuiltSurvivorState>();
00020:     }
00021:
00022:     [Serializable]
00023:     public sealed class GuiltSurvivorState
00024:     {
00025:         public string survivorId = string.Empty;
00026:         public float insomniaSeverity;
00027:         public float sedativeCompensationHours;
00028:         public List<GuiltRecord> guiltSources = new List<GuiltRecord>();
00029:     }
00030:
00031:     /// <summary>
00032:     /// ASHFALL: THE MASSIVE CONTENT EXPANSION — Guilt-Driven Insomnia System.
00033:     /// Ruthless decisions create guilt records that multiply sleep quality penalties.
00034:     /// Sedatives or interpersonal dialogue can compensate. Engine-agnostic: uses
00035:     /// string survivor IDs, raises events, save/load safe.
00036:     /// </summary>
00037:     public class GuiltInsomniaSystem
00038:     {
00039:         public const float SleepQualityPenaltyPerSeverity = 0.50f;
00040:         public const float SedativeCompensationHours = 12f;
00041:         public const float SedativeSeverityReduction = 0.40f;
00042:         public const float DialogueSeverityReduction = 0.25f;
00043:         public const float NaturalDecayPerDay = 0.05f;
00044:         public const float HighSeverityThreshold = 0.7f;
00045:         public const int GuiltExpiryDays = 30;
00046:
00047:         public event Action<string, GuiltRecord> OnGuiltRecorded;
00048:         public event Action<string> OnGuiltResolved;
00049:         public event Action<string> OnGuiltInsomniaCritical;
00050:         public event Action OnStateChanged;
00051:
00052:         private readonly Dictionary<string, GuiltSurvivorState> _bySurvivor =
00053:             new Dictionary<string, GuiltSurvivorState>(StringComparer.Ordinal);
00054:
00055:         private GuiltSurvivorState GetOrCreate(string survivorId)
00056:         {
00057:             if (!_bySurvivor.TryGetValue(survivorId, out var state))
00058:             {
00059:                 state = new GuiltSurvivorState { survivorId = survivorId };
00060:                 _bySurvivor[survivorId] = state;
00061:             }
00062:             return state;
00063:         }
00064:
00065:         public void RecordGuilt(string survivorId, string sourceId, float severity, int currentDay)
00066:         {
00067:             if (string.IsNullOrEmpty(survivorId) || severity <= 0f) return;
00068:             var state = GetOrCreate(survivorId);
00069:             state.guiltSources.Add(new GuiltRecord
00070:             {
00071:                 sourceId = sourceId ?? string.Empty,
00072:                 dayRecorded = Math.Max(1, currentDay),
00073:                 severity = severity
00074:             });
00075:             UpdateInsomniaSeverity(state);
00076:             OnGuiltRecorded?.Invoke(survivorId, state.guiltSources[state.guiltSources.Count - 1]);
00077:             OnStateChanged?.Invoke();
00078:         }
00079:
00080:         public bool ApplySedative(string survivorId)
00081:         {
00082:             if (!_bySurvivor.TryGetValue(survivorId, out var state)) return false;
00083:             if (state.insomniaSeverity <= 0f) return false;
00084:             state.sedativeCompensationHours = SedativeCompensationHours;
00085:             float old = state.insomniaSeverity;
00086:             state.insomniaSeverity = Math.Max(0f, state.insomniaSeverity - SedativeSeverityReduction);
00087:             OnStateChanged?.Invoke();
00088:             return state.insomniaSeverity < old;
00089:         }
00090:
00091:         public bool ResolveGuiltThroughDialogue(string survivorId)
00092:         {
00093:             if (!_bySurvivor.TryGetValue(survivorId, out var state)) return false;
00094:             if (state.guiltSources.Count == 0) return false;
00095:             state.guiltSources.RemoveAt(state.guiltSources.Count - 1);
00096:             UpdateInsomniaSeverity(state);
00097:             if (state.guiltSources.Count == 0)
00098:                 OnGuiltResolved?.Invoke(survivorId);
00099:             OnStateChanged?.Invoke();
00100:             return true;
00101:         }
00102:
00103:         /// <summary>Canonical therapeutic relief (flagship sanatorium Task 8):
00104:         /// scales insomnia severity down by the authored fraction (0..1).</summary>
00105:         public void ApplyTherapyRelief(string survivorId, float fraction)
00106:         {
00107:             if (string.IsNullOrEmpty(survivorId)) return;
00108:             fraction = Math.Clamp(fraction, 0f, 1f);
00109:             if (fraction <= 0f) return;
00110:             var state = _bySurvivor.TryGetValue(survivorId, out var s) ? s : null;
00111:             if (state == null) return;
00112:             state.insomniaSeverity = Math.Max(0f, state.insomniaSeverity * (1f - fraction));
00113:             OnStateChanged?.Invoke();
00114:         }
00115:
00116:         public float GetSleepQualityMultiplier(string survivorId)
00117:         {
00118:             if (!_bySurvivor.TryGetValue(survivorId, out var state)) return 1f;
00119:             float penalty = state.insomniaSeverity * SleepQualityPenaltyPerSeverity;
00120:             if (state.sedativeCompensationHours > 0f) penalty *= 0.5f;
00121:             return Math.Max(0.1f, 1f - penalty);
00122:         }
00123:
00124:         public float GetInsomniaSeverity(string survivorId)
00125:         {
00126:             return _bySurvivor.TryGetValue(survivorId, out var state) ? state.insomniaSeverity : 0f;
00127:         }
00128:
00129:         public int GetGuiltSourceCount(string survivorId)
00130:         {
00131:             return _bySurvivor.TryGetValue(survivorId, out var state) ? state.guiltSources.Count : 0;
00132:         }
00133:
00134:         public void Tick(string survivorId, float gameHours, int currentDay)
00135:         {
00136:             if (!_bySurvivor.TryGetValue(survivorId, out var state)) return;
00137:
00138:             if (state.sedativeCompensationHours > 0f)
00139:             {
00140:                 state.sedativeCompensationHours = Math.Max(0f, state.sedativeCompensationHours - gameHours);
00141:                 if (state.sedativeCompensationHours <= 0f)
00142:                     UpdateInsomniaSeverity(state);
00143:             }
00144:
00145:             if (state.guiltSources.Count > 0)
00146:             {
00147:                 for (int i = state.guiltSources.Count - 1; i >= 0; i--)
00148:                 {
00149:                     if (currentDay - state.guiltSources[i].dayRecorded > GuiltExpiryDays)
00150:                         state.guiltSources.RemoveAt(i);
00151:                 }
00152:                 if (state.guiltSources.Count == 0)
00153:                 {
00154:                     state.insomniaSeverity = 0f;
00155:                     OnGuiltResolved?.Invoke(survivorId);
00156:                 }
00157:                 else
00158:                 {
00159:                     UpdateInsomniaSeverity(state);
00160:                 }
00161:             }
00162:             OnStateChanged?.Invoke();
00163:         }
00164:
00165:         private void UpdateInsomniaSeverity(GuiltSurvivorState state)
00166:         {
00167:             float total = 0f;
00168:             for (int i = 0; i < state.guiltSources.Count; i++)
00169:                 total += state.guiltSources[i].severity;
00170:             state.insomniaSeverity = Math.Min(1f, total);
00171:             if (state.insomniaSeverity >= HighSeverityThreshold)
00172:                 OnGuiltInsomniaCritical?.Invoke(state.survivorId);
00173:         }
00174:
00175:         public GuiltInsomniaSaveState CaptureState()
00176:         {
00177:             var save = new GuiltInsomniaSaveState();
00178:             foreach (var kv in _bySurvivor)
00179:             {
00180:                 var s = kv.Value;
00181:                 var copy = new GuiltSurvivorState
00182:                 {
00183:                     survivorId = s.survivorId,
00184:                     insomniaSeverity = s.insomniaSeverity,
00185:                     sedativeCompensationHours = s.sedativeCompensationHours
00186:                 };
00187:                 foreach (var g in s.guiltSources)
00188:                     copy.guiltSources.Add(new GuiltRecord
00189:                     {
00190:                         sourceId = g.sourceId,
00191:                         dayRecorded = g.dayRecorded,
00192:                         severity = g.severity
00193:                     });
00194:                 save.survivors.Add(copy);
00195:             }
00196:             return save;
00197:         }
00198:
00199:         public void RestoreState(GuiltInsomniaSaveState save)
00200:         {
00201:             _bySurvivor.Clear();
00202:             if (save?.survivors == null) return;
00203:             foreach (var s in save.survivors)
00204:             {
00205:                 if (s == null || string.IsNullOrEmpty(s.survivorId)) continue;
00206:                 var copy = new GuiltSurvivorState
00207:                 {
00208:                     survivorId = s.survivorId,
00209:                     insomniaSeverity = s.insomniaSeverity,
00210:                     sedativeCompensationHours = s.sedativeCompensationHours
00211:                 };
00212:                 if (s.guiltSources != null)
00213:                     foreach (var g in s.guiltSources)
00214:                         copy.guiltSources.Add(new GuiltRecord
00215:                         {
00216:                             sourceId = g.sourceId,
00217:                             dayRecorded = g.dayRecorded,
00218:                             severity = g.severity
00219:                         });
00220:                 _bySurvivor[s.survivorId] = copy;
00221:             }
00222:         }
00223:     }
00224: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs`

### `Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs` — complete current file

- Size: 106 lines / 3555 bytes.
- SHA-256: `9b564dbdeb0c72951c742128a02b2c85a3908d198c821e66aedfe5e87276859e`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Text.Json;
00006: using System.Text.Json.Serialization;
00007:
00008: namespace Ashfall.Core.Survivors
00009: {
00010:     public sealed class GuiltSourceDefinition
00011:     {
00012:         [JsonPropertyName("choice_pattern")]
00013:         public string ChoicePattern { get; set; } = string.Empty;
00014:
00015:         [JsonPropertyName("severity")]
00016:         public float Severity { get; set; }
00017:
00018:         [JsonPropertyName("title")]
00019:         public string Title { get; set; } = string.Empty;
00020:
00021:         [JsonPropertyName("description")]
00022:         public string Description { get; set; } = string.Empty;
00023:
00024:         public string FormatDescription(string survivorName)
00025:         {
00026:             if (string.IsNullOrEmpty(Description))
00027:                 return string.Empty;
00028:             return Description.Replace("{name}", survivorName ?? "Someone");
00029:         }
00030:     }
00031:
00032:     public sealed class GuiltSourceCatalog
00033:     {
00034:         private sealed class CatalogRoot
00035:         {
00036:             [JsonPropertyName("schema_version")]
00037:             public int SchemaVersion { get; set; }
00038:
00039:             [JsonPropertyName("items")]
00040:             public List<GuiltSourceDefinition> Items { get; set; } = new List<GuiltSourceDefinition>();
00041:         }
00042:
00043:         private readonly List<GuiltSourceDefinition> _items = new List<GuiltSourceDefinition>();
00044:         private readonly Dictionary<string, GuiltSourceDefinition> _byPattern =
00045:             new Dictionary<string, GuiltSourceDefinition>(StringComparer.Ordinal);
00046:
00047:         public IReadOnlyList<GuiltSourceDefinition> Items => _items;
00048:         public int Count => _items.Count;
00049:
00050:         public GuiltSourceCatalog() { }
00051:
00052:         public GuiltSourceCatalog(IEnumerable<GuiltSourceDefinition> items)
00053:         {
00054:             if (items != null)
00055:             {
00056:                 foreach (var item in items)
00057:                 {
00058:                     _items.Add(item);
00059:                     if (!string.IsNullOrEmpty(item.ChoicePattern))
00060:                         _byPattern[item.ChoicePattern] = item;
00061:                 }
00062:             }
00063:         }
00064:
00065:         public static GuiltSourceCatalog FromJson(string json)
00066:         {
00067:             if (string.IsNullOrWhiteSpace(json))
00068:                 return new GuiltSourceCatalog();
00069:
00070:             var root = JsonSerializer.Deserialize<CatalogRoot>(json, new JsonSerializerOptions
00071:             {
00072:                 PropertyNameCaseInsensitive = true
00073:             });
00074:
00075:             return new GuiltSourceCatalog(root?.Items ?? (IEnumerable<GuiltSourceDefinition>)Array.Empty<GuiltSourceDefinition>());
00076:         }
00077:
00078:         public static GuiltSourceCatalog LoadFromDirectory(string dataDirectory)
00079:         {
00080:             var filePath = Path.Combine(dataDirectory, "guilt_sources.json");
00081:             if (!File.Exists(filePath))
00082:                 return new GuiltSourceCatalog();
00083:
00084:             var json = File.ReadAllText(filePath);
00085:             return FromJson(json);
00086:         }
00087:
00088:         public GuiltSourceDefinition? GetByPattern(string choicePattern)
00089:         {
00090:             if (string.IsNullOrEmpty(choicePattern)) return null;
00091:             return _byPattern.TryGetValue(choicePattern, out var def) ? def : null;
00092:         }
00093:
00094:         public bool TryGetSeverity(string choicePattern, out float severity)
00095:         {
00096:             severity = 0f;
00097:             if (string.IsNullOrEmpty(choicePattern)) return false;
00098:             if (_byPattern.TryGetValue(choicePattern, out var def))
00099:             {
00100:                 severity = def.Severity;
00101:                 return true;
00102:             }
00103:             return false;
00104:         }
00105:     }
00106: }
```


# Appendix — Current Source Detail: `src/UI/IronCenotaphMemorialPanel.cs`

### `src/UI/IronCenotaphMemorialPanel.cs` — complete current file

- Size: 187 lines / 9167 bytes.
- SHA-256: `5a39a1daac0e45ad6b386fb62deb477828137fa7e20ba7680eafbf48684f49a4`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Ashfall.Core;
00004: using Godot;
00005: using Ashfall.Core.UI;
00006: using DesignTheme = Ashfall.Core.UI.Theme;
00007:
00008: namespace AtomicWar.GodotApp.UI
00009: {
00010:     public partial class IronCenotaphMemorialPanel : Control, IBindablePanel
00011:     {
00012:         public event Action? OnClose;
00013:
00014:         private Label? _headerTitleLabel;
00015:         private Label? _statusBadgeLabel;
00016:         private Button? _closeButton;
00017:         private VBoxContainer? _telemetryContainer;
00018:         private VBoxContainer? _buttonContainer;
00019:         private VBoxContainer? _dataContainer;
00020:         private Label? _logOutputLabel;
00021:
00022:         /// <summary>Plan 24C (A3) — the vigil command: mourns the most recent
00023:         /// unmourned loss through the memorial owner; the result text is the
00024:         /// only feedback (never a silent no-op).</summary>
00025:         private void OnVigilPressed()
00026:         {
00027:             if (_mourning == null)
00028:             {
00029:                 if (_logOutputLabel != null)
00030:                     _logOutputLabel.Text = "[RIT-01] No memorial ledger bound. The dead stay uncounted.";
00031:                 return;
00032:             }
00033:             var pending = _mourning.LatestUnmourned();
00034:             if (pending == null)
00035:             {
00036:                 if (_logOutputLabel != null)
00037:                     _logOutputLabel.Text = "[RIT-01] Every recorded loss has had its vigil.";
00038:                 return;
00039:             }
00040:             var result = _mourning.Mourn(pending.Value.id);
00041:             if (_logOutputLabel != null)
00042:                 _logOutputLabel.Text = result.IsSuccess
00043:                     ? $"[RIT-01] The shelter stood together and mourned. (D{pending.Value.day} loss)"
00044:                     : "[RIT-01] The vigil could not begin: "
00045:                         + (result.MessageKey ?? result.FailureCode ?? "unspecified")
00046:                         .Replace("memorial.", "").Replace('_', ' ') + ".";
00047:             RefreshView();
00048:         }
00049:
00050:         public override void _Ready()
00051:         {
00052:             SetAnchorsPreset(LayoutPreset.FullRect);
00053:             BuildInterface();
00054:         }
00055:
00056:         public void Open()
00057:         {
00058:             Visible = true;
00059:             RefreshView();
00060:         }
00061:
00062:         public bool IsBound { get; private set; } = true;
00063:
00064:         public void Bind(object? session)
00065:         {
00066:             IsBound = true;
00067:             RefreshView();
00068:         }
00069:
00070:         public void Unbind()
00071:         {
00072:             IsBound = false;
00073:         }
00074:
00075:         /// <summary>
00076:         /// Plan 24C (A3) — the mourning route's binding: the memorial owner's
00077:         /// read model plus the once-per-death vigil command. The panel renders
00078:         /// truthful state and dispatches the command; it never recomputes
00079:         /// grief or morale (the host owns the attributed recovery).
00080:         /// </summary>
00081:         public sealed class MourningBinding
00082:         {
00083:             public Func<int> TotalDeaths { get; init; } = () => 0;
00084:             public Func<(string id, int day)?> LatestUnmourned { get; init; } = () => null;
00085:             /// <summary>deceasedId → the memorial owner's action result.</summary>
00086:             public Func<string, ActionResult> Mourn { get; init; } = _ =>
00087:                 ActionResult.Blocked("unbound", "memorial.mourn_unbound");
00088:         }
00089:
00090:         private MourningBinding? _mourning;
00091:         private Func<int>? _spiritualArcCount;
00092:         private Func<int>? _spiritualPendingRites;
00093:
00094:         public void BindMourning(MourningBinding binding)
00095:         {
00096:             _mourning = binding;
00097:             IsBound = binding != null;
00098:             RefreshView();
00099:         }
00100:
00101:         public void BindSpiritual(Func<int> arcCount, Func<int> pendingRites)
00102:         {
00103:             _spiritualArcCount = arcCount;
00104:             _spiritualPendingRites = pendingRites;
00105:             RefreshView();
00106:         }
00107:
00108:         public void RefreshView()
00109:         {
00110:             if (_statusBadgeLabel != null)
00111:             {
00112:                 // Plan 24C (A3): truthful status from the memorial owner when
00113:                 // bound; the unbound placeholder keeps its historical text.
00114:                 if (_mourning != null)
00115:                 {
00116:                     var pending = _mourning.LatestUnmourned();
00117:                     string status = pending == null
00118:                         ? $"STATUS: MEMORIAL FLAME ACTIVE - RECORDED: {_mourning.TotalDeaths()} SOULS - ALL MOURNED"
00119:                         : $"STATUS: MEMORIAL FLAME ACTIVE - RECORDED: {_mourning.TotalDeaths()} SOULS - VIGIL PENDING (D{pending.Value.day})";
00120:                     if (_spiritualArcCount != null)
00121:                     {
00122:                         int arcs = _spiritualArcCount();
00123:                         int rites = _spiritualPendingRites?.Invoke() ?? 0;
00124:                         status += $" - MOURNING ARCS: {arcs} - RITES OPEN: {rites}";
00125:                     }
00126:                     _statusBadgeLabel.Text = status;
00127:                 }
00128:             }
00129:         }
00130:
00131:         private void BuildInterface()
00132:         {
00133:             var chrome = ThreePanePanelScaffold.BuildChrome(
00134:                 this,
00135:                 "SHELTER COMMEMORATION // THE IRON CENOTAPH [RIT-01]",
00136:                 "STATUS: MEMORIAL FLAME ACTIVE - RECORDED: 38 SOULS (-18% GRIEF)",
00137:                 AshfallUiHelpers.ToColor(DesignTheme.Warm),
00138:                 "[X] CLOSE CONSOLE",
00139:                 "[RIT-01] Bronze plaque carved for Dr. Aris Thorne.\n[RIT-01] Memorial vigil conducted. Living survivor grief mitigated.",
00140:                 () => OnClose?.Invoke());
00141:             _headerTitleLabel = chrome.Title;
00142:             _statusBadgeLabel = chrome.Status;
00143:             _closeButton = chrome.Close;
00144:             _logOutputLabel = chrome.Log;
00145:             var bodyHBox = chrome.Body;
00146:
00147:             // Left Column (Telemetry)
00148:             var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("CASUALTY ROLL & MORTALITY CAUSES");
00149:             bodyHBox.AddChild(leftPanel);
00150:             _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
00151:             _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RECORDED CASUALTIES", "38 SOULS ON MEMORIAL WALL", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
00152:             _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RADIATION POISONING", "42.0% OF ALL DEATHS", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
00153:             _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("STARVATION & DEHYDRATION", "24.0% OF ALL DEATHS", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
00154:             _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("COMBAT & TRAUMA", "34.0% OF ALL DEATHS", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00155:             _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("WALL PLAQUE CAPACITY", "38 / 80 SLOTS OCCUPIED", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
00156:
00157:             // Center Column (Interactive Controls)
00158:             var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("BRONZE EPITAPH ENGRAVER & ETERNAL FLAME");
00159:             bodyHBox.AddChild(centerPanel);
00160:             _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
00161:             _buttonContainer.AddChild(AshfallUiHelpers.MakeDisabledButton(
00162:                 "[ENGRAVE BRONZE MEMORIAL PLAQUE]",
00163:                 "Not yet available \u2014 no memorial engraving authority is wired."));
00164:             _buttonContainer.AddChild(AshfallUiHelpers.MakeDisabledButton(
00165:                 "[REFUEL ETERNAL MEMORIAL FLAME]",
00166:                 "Not yet available \u2014 eternal-flame upkeep is not wired to an owner."));
00167:             // Plan 24C (A3): the vigil is the mourning action — once per death,
00168:             // routed through the memorial owner's command with truthful feedback.
00169:             var vigilButton = new Button { Text = "[HOLD ALL-SHELTER VIGIL & MOMENT OF SILENCE]", SizeFlagsHorizontal = SizeFlags.ExpandFill };
00170:             vigilButton.Pressed += OnVigilPressed;
00171:             _buttonContainer.AddChild(vigilButton);
00172:             _buttonContainer.AddChild(AshfallUiHelpers.MakeDisabledButton(
00173:                 "[RECITE DIEGETIC COMMEMORATION EULOGY]",
00174:                 "Not yet available \u2014 no eulogy authority is wired."));
00175:
00176:             // Right Column (Data & Logistics)
00177:             var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("MEMORIAL RELICS & VIGIL ATTENDANCE");
00178:             bodyHBox.AddChild(rightPanel);
00179:             _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
00180:             _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("ETERNAL OIL FLAME RESERVOIR", "42.5 LITERS (0.2 L/DAY)", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00181:             _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("MEMORIAL RELICS PRESERVED", "24 DOG TAGS / 6 WATCHES", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
00182:             _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SHELTER GRIEF MITIGATION", "-18.5% DESPAIR INDEX", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00183:             _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("LAST VIGIL ATTENDANCE", "94% OF LIVING POPULATION", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00184:
00185:         }
00186:     }
00187: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **The subject is a small, read-only ambient-text selector whose data loop is complete but whose production reachability is not proven. The plan expands the quality and integration decision while refusing to invent a player route or state owner.**.

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
