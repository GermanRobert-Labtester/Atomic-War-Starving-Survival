# Plan 87 — Relic Provenance, Display, Memorial, and Community Memory

> **Rebuild status:** PARTIAL — PROVENANCE AND DOSSIERS EXIST; POST-RESTORATION CHOICE INTEGRATION REMAINS
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

- Current code already records item provenance chains, ownership transfers, lore entries and significance. A separate 32-row relic dossier catalog adds tone/material/location/curator/effect metadata.
- The missing opportunity is a post-restoration decision surface that routes display, study, trade or memorialization through current owners and writes only the facts those owners accept.
- The plan must avoid turning every relic into a hidden permanent bonus and must support legacy restored items that predate provenance state.

**Bounded outcome:** Use `ItemLoreSystem` for instance provenance/history, `RelicProvenanceCatalog` for authored dossiers, `CulturalArchiveVaultSystem` for archive stewardship, research/trade/memorial owners for effects and `RelicCatalogLoader` only for recipe definitions. Do not create a `RelicProvenanceManager` or parallel relic save.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- ItemLoreSystem schema 1 persists lore entries, provenance chains, ownership and significance.
- RelicProvenanceCatalog loads 32 authored dossiers with tone, material, discovery location and curator notes.
- CulturalArchiveVaultSystem owns tome/restoration/archive state and apprenticeship transcription.
- Workshop restoration is complete in `WorkshopReverseEngineeringSystem`.
- Research unlocks and trade/memorial effects have existing owners.

**Master-authority sections applied to this rebase:**

- Volume 30 cultural archive confirmation
- Volume 32 restoration-log contract
- Volume 38 transcription ownership

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Create a post-restoration choice read model/command seam over current ItemLore and workshop facts.
- Route study through ResearchUnlockBridge, trade through Market/trade owners, memorial through Memorial/CulturalArchive.
- Define reversible/terminal disposition semantics explicitly.
- Support legacy items with no provenance record through deterministic backfill-on-observation, not fabricated history.

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
| restoration event and output | WorkshopReverseEngineeringSystem | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` | Recipe/restoration owner. |
| instance provenance, ownership and significance | ItemLoreSystem | `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs` | Runtime provenance owner. |
| authored historical dossier | RelicProvenanceCatalog | `Assets/Ashfall.Core/Narrative/RelicProvenanceCatalog.cs` | Static lore authority. |
| archive stewardship/restoration projects | CulturalArchiveVaultSystem | `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs` | Archive owner. |
| choice consequences | Research/market/memorial | `Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs; Assets/Ashfall.Core/Economy/MarketSystem.cs; Assets/Ashfall.Core/Memorial/MemorialSystem.cs` | No relic-local gameplay authority. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Relic Provenance, Display, Memorial, and Community Memory
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ WorkshopReverseEngineeringSystem
│   restoration event and output
│ ItemLoreSystem
│   instance provenance, ownership and significance
│ RelicProvenanceCatalog
│   authored historical dossier
│ CulturalArchiveVaultSystem
│   archive stewardship/restoration projects
│ Research/market/memorial
│   choice consequences
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

1. **Preserve current state ownership.** WorkshopReverseEngineeringSystem owns restoration event and output: Recipe/restoration owner.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| restoration event and output | WorkshopReverseEngineeringSystem | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` | Recipe/restoration owner. |
| instance provenance, ownership and significance | ItemLoreSystem | `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs` | Runtime provenance owner. |
| authored historical dossier | RelicProvenanceCatalog | `Assets/Ashfall.Core/Narrative/RelicProvenanceCatalog.cs` | Static lore authority. |
| archive stewardship/restoration projects | CulturalArchiveVaultSystem | `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs` | Archive owner. |
| choice consequences | Research/market/memorial | `Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs; Assets/Ashfall.Core/Economy/MarketSystem.cs; Assets/Ashfall.Core/Memorial/MemorialSystem.cs` | No relic-local gameplay authority. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. restore relic through workshop
2. resolve item instance/output identity
3. register or read ItemLore provenance
4. attach dossier metadata if IDs resolve
5. present truthful disposition choices
6. preview target-owner effects
7. execute through owner command
8. record result through ItemLore/journal/archive owner

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- ItemLore state persists instance history.
- Dossier catalog is static.
- Archive state persists vault/transcription facts.
- Disposition needs an explicit owner; do not add an unrepresented flag.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- No fabricated craft history for legacy items.
- A dossier is metadata, not mutable gameplay state.
- Study/trade/memorial effects route to canonical owners.
- One restored output cannot create duplicate provenance records.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- No recipe changes.
- No new relic provenance catalog in the integration tranche.
- Future dossier rows reference existing relic IDs.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use ItemLore and CulturalArchive saves.
- No new relic-provenance save section.
- Legacy items default to unknown/undocumented provenance.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Significance derives from current lore count.
- No random disposition or hidden bonus.
- Stable item/relic ID ordering.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Workshop completion is the source fact.
- ItemLore raises lore/significance/ownership events.
- Choice consequences emit from their target owners.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/UI/WorkshopPanel.cs
- src/Main.FlagshipInstitutions.cs
- src/UI/ArchiveDeskPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Dossier tone and curator notes are separated from runtime facts.
- Community memory choices remain restrained and evidence-based.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A new manager duplicates ItemLore. | WorkshopReverseEngineeringSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A dossier ID fails to resolve. | ItemLoreSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Legacy item receives invented craft history. | RelicProvenanceCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Study effect bypasses research bridge. | CulturalArchiveVaultSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | Trade/memorial choice is reversible in the UI but not owner state. | Research/market/memorial | Focused negative test or static source gate; no broad-suite dependency. |
| F-06 | The same output registers provenance twice. | WorkshopReverseEngineeringSystem | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/ItemLoreSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/RelicProvenanceCatalogTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/CulturalArchiveVaultTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/WorkshopReverseEngineeringSystemTests.cs`
5. `godot --headless --path . -- --data-integrity-selftest` and content-utilization for relic/dossier rows.

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — owner census | Map recipe, ItemLore, dossier, archive, research, trade and memorial owners. | No duplicate provenance state. | No production path until the owning implementation package is separately claimed. |
| 1 — restoration-to-lore bridge | Register exact output instance at completion. | Idempotent and legacy-safe. | No production path until the owning implementation package is separately claimed. |
| 2 — disposition contract | Define previews and target-owner effects. | Every effect has one owner. | No production path until the owning implementation package is separately claimed. |
| 3 — archive/memorial integration | Use current archive and memorial paths. | No relic-local museum. | No production path until the owning implementation package is separately claimed. |
| 4 — UI and prose polish | Expose provenance/dossier/disposition clearly. | No hidden permanent bonuses. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs | MODIFY only for restoration bridge in a future claim | Runtime provenance owner |
| Assets/Ashfall.Core/Narrative/RelicProvenanceCatalog.cs | READ | Dossier authority |
| Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs | READ | Archive owner |
| src/UI/WorkshopPanel.cs | READ; MODIFY only for truthful disposition surface | Presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Parallel relic manager. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Dossier/runtime conflation. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Fabricated legacy history. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Hidden stat inflation. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Non-idempotent registration. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No relic recipe expansion.
- No new provenance catalog.
- No generic museum system.
- No permanent combat bonus by default.

# 23. Rollback and Recovery

- Bridge/UI is reversible.
- Disposition state must use an existing owner or a versioned extension.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- Current provenance/dossier authorities are explicit.
- Legacy behavior is safe.
- Choice effects route to owners.
- No new manager is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Create a post-restoration choice read model/command seam over current ItemLore and workshop facts.
- Route study through ResearchUnlockBridge, trade through Market/trade owners, memorial through Memorial/CulturalArchive.
- Define reversible/terminal disposition semantics explicitly.
- Support legacy items with no provenance record through deterministic backfill-on-observation, not fabricated history.

## MUST NOT DO

- No relic recipe expansion.
- No new provenance catalog.
- No generic museum system.
- No permanent combat bonus by default.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/ItemLoreSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/RelicProvenanceCatalogTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/CulturalArchiveVaultTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/WorkshopReverseEngineeringSystemTests.cs`
5. `godot --headless --path . -- --data-integrity-selftest` and content-utilization for relic/dossier rows.

## FIRST SAFE IMPLEMENTATION STEP

0 — owner census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: restoration event and output → WorkshopReverseEngineeringSystem; instance provenance, ownership and significance → ItemLoreSystem; authored historical dossier → RelicProvenanceCatalog; archive stewardship/restoration projects → CulturalArchiveVaultSystem; choice consequences → Research/market/memorial. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 87.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 87 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by WorkshopReverseEngineeringSystem or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs`

### `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 367 lines / 14913 bytes.
- SHA-256: `b44e502a94833538a4ff3301c9782446cd49c51cf0e37db732a247c09ab972a2`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum LoreTriggerType
public enum SignificanceLevel
public sealed class ItemLoreEntry
public string LoreId { get; set; } = string.Empty;
public string ItemInstanceId { get; set; } = string.Empty;
public LoreTriggerType TriggerType { get; set; } = LoreTriggerType.Crafting;
public string Text { get; set; } = string.Empty;
public int Day { get; set; } = 1;
public string AssociatedSurvivorId { get; set; } = string.Empty;
public string AssociatedLocationId { get; set; } = string.Empty;
public sealed class ItemProvenanceChain
public string ItemInstanceId { get; set; } = string.Empty;
public string CrafterSurvivorId { get; set; } = string.Empty;
public int CraftingDay { get; set; } = 0;
public string DiscoveryLocationId { get; set; } = string.Empty;
public int DiscoveryDay { get; set; } = 0;
public string DiscoveryContext { get; set; } = string.Empty;
public List<string> OwnershipChain { get; set; } = new List<string>();
public List<string> LoreEntryIds { get; set; } = new List<string>();
public SignificanceLevel Significance { get; set; } = SignificanceLevel.Mundane;
public sealed class ItemLoreState
public int SchemaVersion { get; set; } = 1;
public int NextSequence { get; set; } = 1;
public List<ItemLoreEntry> LoreEntries { get; set; } = new List<ItemLoreEntry>();
public List<ItemProvenanceChain> Provenances { get; set; } = new List<ItemProvenanceChain>();
public sealed class ItemLoreSystem
public event Action<ItemLoreEntry>? OnLoreAdded;
public event Action<ItemProvenanceChain, SignificanceLevel>? OnSignificanceChanged;
public event Action<string, string>? OnOwnershipTransferred;
public int TrackedItemCount => _state.Provenances.Count;
public int TotalLoreEntriesCount => _state.LoreEntries.Count;
public ItemProvenanceChain RegisterItem( string itemInstanceId, string? crafterId = null, int craftingDay = 0, string? discoveryLocationId = null, int discoveryDay = 0,
public bool TransferOwnership(string itemInstanceId, string newOwnerId, int day = 1) {
public ItemLoreEntry AddLore( string itemInstanceId, LoreTriggerType trigger, string text, int day, string? survivorId = null,
public ItemProvenanceChain? GetProvenance(string itemInstanceId) {
public IReadOnlyList<ItemLoreEntry> GetLoreEntries(string itemInstanceId) {
public ItemLoreState CaptureState() => CloneState(_state);
public void RestoreState(ItemLoreState state) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Narrative/RelicProvenanceCatalog.cs`

### `Assets/Ashfall.Core/Narrative/RelicProvenanceCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 99 lines / 3161 bytes.
- SHA-256: `097ee3cc2155300c25a604c29b3736360f5b9e5d73b5dc3eca0da9f4f9035761`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RelicDossierEntry
public string relic_id;
public string name;
public string tone;
public string material;
public string discovery_location;
public string curator_note;
public string gameplay_effect;
public string[] tags;
public sealed class RelicProvenanceFile
public int schema_version;
public string collection_id;
public List<RelicDossierEntry> relics = new List<RelicDossierEntry>();
public sealed class RelicProvenanceCatalog
public IReadOnlyList<RelicDossierEntry> AllRelics => _allRelics;
public void Load(string json, IJsonSerializer serializer) {
public RelicDossierEntry? GetById(string relicId) {
public List<RelicDossierEntry> GetByTone(string tone) {
public List<RelicDossierEntry> GetByTag(string tag) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs`

### `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 674 lines / 31034 bytes.
- SHA-256: `44b077c262c9c05de45f09f97809e9067c307d3a0349bdaa8edf5fa008e94c34`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=23; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ArchiveDocumentState
public string document_id = string.Empty;
public int physical_degradation_permille;        // 0..1000
public bool is_chemically_stabilized;
public int transcription_permille;               // 0..1000
public string active_scholar_id = string.Empty;
public int microfiche_copy_count;
public bool knowledge_preserved;                 // the permanent unlock (microfiche)
public string status = "archived";               // archived | transcribing | transcribed | lost
public sealed class ArchiveProjectState
public string document_id = string.Empty;
public string kind = string.Empty;               // restoration | transcription
public string survivor_id = string.Empty;
public int started_day = -1;
public int last_progress_day = -1;
public sealed class ArchiveRecordingState
public string recording_id = string.Empty;
public string category = string.Empty;           // music_performance | oral_history | survivor_testimony | radio_archive | commemorative
public string operator_id = string.Empty;
public int recorded_day = -1;
public sealed class ArchiveSalonState
public bool active;
public string modifier_key = "salon_stress_resistance";
public int start_day = -1;
public int duration_days;
public int cooldown_until_day = -1;
public sealed class ArchiveChronicleEntry
public string chronicle_id = string.Empty;
public int campaign_day;
public string event_type = string.Empty;
public string summary_key = string.Empty;
public List<string> participants = new();
public string author_id = string.Empty;
public string volume_id = string.Empty;
public sealed class CulturalArchiveVaultSave
public int schema_version = 1;
public List<ArchiveDocumentState> documents = new();
public List<ArchiveProjectState> active_projects = new();
public List<ArchiveRecordingState> recordings = new();
public List<ArchiveChronicleEntry> chronicle_entries = new();
public ArchiveSalonState salon = new();
public int next_chronicle_ordinal;
public float degradation_remainder;              // deterministic fractional permille carry
public DocumentationState documentation = new();
public sealed class CulturalArchiveVaultSystem
public const string SystemId = "cultural_archives";
public const string InstitutionId = "institution_cultural_archive";
public const int RestorationReliefPermille = 350;
public const int LegibilityLimitPermille = 900;   // above this, pages cannot be worked
public const int LostThresholdPermille = 1000;
public const float BaseDailyDegradationPermille = 2f;
public const int SalonDefaultDurationDays = 5;
public const int SalonCooldownDays = 10;
public const float SalonMoralePerDay = 2f;
public const string CutDiscCostItemId = "acetate_blank_disc";
public DocumentationSystem Documentation { get; }
public event Action<string>? OnDocumentRestored;               // documentId
public event Action<string>? OnMicroficheCreated;              // documentId
public event Action<string>? OnTomeTranscribed;                // documentId
public event Action<string>? OnDocumentLost;                   // documentId
public event Action<string, VinylRecordDefinition>? OnArchiveRecordingCreated;
public event Action<int>? OnSalonStarted;                      // day
public event Action<int>? OnSalonEnded;                        // day
public event Action<float>? OnSalonMoraleTick;                 // morale delta, once per day while active
public event Action<ArchiveChronicleEntry>? OnChronicleEntryAdded;
public event Action? OnDocumentationChanged;
public void LoadTomeCatalog(List<CulturalArchiveTomeDefinition> tomes) {
public IReadOnlyList<ArchiveDocumentState> Documents => _state.documents.AsReadOnly();
public IReadOnlyList<ArchiveRecordingState> Recordings => _state.recordings.AsReadOnly();
public IReadOnlyList<ArchiveChronicleEntry> Chronicle => _state.chronicle_entries.AsReadOnly();
public ArchiveSalonState Salon => _state.salon;
public ArchiveDocumentState? GetDocument(string documentId) =>
public ActionResult TryRestoreDocument(string documentId) {
public ActionResult TryStartTranscription(string documentId, string scholarId) {
public ActionResult TryCreateMicroficheCopy(string documentId, string operatorId) {
public static readonly string[] LegalRecordingCategories = {
public ActionResult TryCutArchiveDisc(string recordingId, string category, string operatorId, int day) {
public static VinylRecordDefinition BuildRecordDefinition(ArchiveRecordingState recording) {
public ActionResult TryStartSalon(int day) {
public ActionResult TryRecordChronicleEntry( string eventType, int campaignDay, string summaryKey, IReadOnlyList<string>? participants, string authorId = "", string? volumeId = null) {
public void TickDay(int day) {
public ActionResult TryCreatePhotograph( string authorId, string title, string cameraUsed, IEnumerable<string>? subjects, string locationId,
public ActionResult TryCreateSketch( string authorId, string title, string subject, string medium, float artisticQuality,
public ActionResult TryCreateWrittenRecord( string authorId, string title, string recordType, string content, float writingQuality,
public ActionResult TryShareDocumentation(string documentationId, int currentDay, out float moraleBoost) {
public CulturalArchiveVaultSave CaptureState() {
public void RestoreState(CulturalArchiveVaultSave? saved) {
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs`

### `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 764 lines / 36041 bytes.
- SHA-256: `88acf308958529905272f8a5e7b772bf3cd765d80051d6442820b5319944f652`.
- Architecture signals: seeded references=4; save/restore symbols=2; typed event declarations=21; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WorkshopState
public string systemId = WorkshopReverseEngineeringSystem.SystemId;
public string selectedRelicId = string.Empty;
public string assignedResearcherId = string.Empty;
public int workPhase; // 0=idle, 1=examining, 2=dismantling, 3=repairing, 4=researching
public float progressHours;
public float hoursRequired;
public List<string> reservedComponentIds = new List<string>();
public List<int> reservedComponentAmounts = new List<int>();
public bool isComplete;
public string completionUnlockId = string.Empty; // research or recipe unlocked
public List<string> completedRelicIds = new List<string>();
public string activeTechSalvageId = string.Empty;
public string techSourceItemId = string.Empty;
public bool techSourceConsumed;
public int techStartedDay;
public float techEquipmentQuality01;
public List<string> completedTechSalvageIds = new List<string>();
public List<ResearchNoteState> researchNotes = new List<ResearchNoteState>();
public sealed class RelicDefinition
public string relic_id = string.Empty;
public string display_name = string.Empty;
public string description = string.Empty;
public List<string> required_components = new List<string>();
public float repair_time_hours = 8f;
public int morale_bonus;
public string dialogue_event_id = string.Empty;
public string restoration_text = string.Empty;
public string world_flag = string.Empty;
public string research_unlock_id = string.Empty; // knowledge node unlocked on research
public string dismantle_yield_item = string.Empty;
public int dismantle_yield_amount = 1;
public string category = "relic";
public sealed class RelicCatalog
public string schema_version = "1.0";
public List<RelicDefinition> relics = new List<RelicDefinition>();
public List<RelicDefinition> recipes { get => relics; set => relics = value; }
public sealed class ResearchNoteState
public string noteId = string.Empty;
public string techId = string.Empty;
public string researcherId = string.Empty;
public int day;
public string progressBand = string.Empty;
public sealed class WorkshopReverseEngineeringSystem
public const string SystemId = "workshop_reverse_engineering";
public WorkshopState State => _state;
public IReadOnlyDictionary<string, RelicDefinition> Catalog => _relicCatalog;
public event Action<ActionResult> OnActionCompleted;
public event Action OnWorkshopStateChanged;
public void BindSkillEvaluator(Func<string, float> evaluator) {
public void LoadCatalog(RelicCatalog catalog) {
public void LoadTechSalvageCatalog(IEnumerable<PreWarTechDef> definitions) {
public void BindTechSalvageRng(ISeededRng rng) {
public IReadOnlyDictionary<string, PreWarTechDef> TechSalvageCatalog => _techCatalog;
public PreWarTechDef? GetTechSalvage(string techId) {
public bool IsTechSalvageCompleted(string techId) =>
public void RegisterRelic(RelicDefinition relic) {
public RelicDefinition? GetRelic(string relicId) {
public bool IsRelicCompleted(string relicId) =>
public bool IsBusy => _state.workPhase > 0 && !_state.isComplete;
public TechDismantlePreview PreviewTechDismantle( string sourceItemId, string researcherId, ResearchFacilityContext? facility = null) {
public ActionResult StartTechDismantle( string sourceItemId, string researcherId, int day = 0, ResearchFacilityContext? facility = null) {
public ActionResult Examine(string relicId) {
public ActionResult StartDismantle(string relicId, string researcherId) {
public ActionResult StartRepair(string relicId, string researcherId) {
public ActionResult StartResearch(string relicId, string researcherId) {
public ActionResult TickProgress(float hoursElapsed) {
public ActionResult CancelJob() {
public WorkshopState CaptureState() {
public void RestoreState(WorkshopState saved) {
public event Action<TechSalvageFailure>? OnTechSalvageFailure;
public event Action<string>? OnBlueprintInsight;
```


# Appendix B.06 — Current Code Architecture: `Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs`

### `Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 305 lines / 12403 bytes.
- SHA-256: `0fd3512906ad9f269014fdcd010871a07f47d4ae2b105ff0504752ddea752250`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ResearchUnlockDef
public string id { get; set; } = string.Empty;
public string research_node_id { get; set; } = string.Empty;
public string unlock_type { get; set; } = "item"; // item, recipe, expedition, shelter, combat, medical
public string unlock_target_id { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public sealed class ResearchUnlockState
public int schema_version { get; set; } = 1;
public List<string> grantedUnlockIds { get; set; } = new List<string>();
public List<string> grantedItems { get; set; } = new List<string>();
public List<string> unlockedRecipeIds { get; set; } = new List<string>();
public List<string> unlockedCapabilities { get; set; } = new List<string>();
public interface IResearchUnlockSink
public sealed class InventoryResearchUnlockSink : IResearchUnlockSink
public IReadOnlyCollection<string> UnlockedRecipes => _unlockedRecipes;
public IReadOnlyCollection<string> EnabledCapabilities => _enabledCapabilities;
public bool GrantBreakthroughItem(string itemId, int quantity = 1) {
public bool UnlockRecipe(string recipeId) {
public bool EnableCapability(string capabilityId, string domain) {
public sealed class ResearchUnlockBridge
public const string SystemId = "research_unlock_bridge";
public const string DefaultCatalogFileName = "research_unlocks.json";
public Action<ResearchUnlockDef>? OnUnlockGrantedSeam { get; set; }
public Action<string, int>? OnBreakthroughItemAwardedSeam { get; set; }
public Action<string>? OnRecipeUnlockedSeam { get; set; }
public Action<string, string>? OnCapabilityEnabledSeam { get; set; }
public ResearchUnlockState State => _state;
public IReadOnlyList<ResearchUnlockDef> Catalog => _catalog;
public void RegisterUnlock(ResearchUnlockDef def) {
public void LoadCatalog(string json) {
public static ResearchUnlockBridge LoadFromDirectory(string dataDir, IFileIO fileIO) {
public void BindResearchSystem(ResearchSystem system, IResearchUnlockSink? sink = null) {
public int ProcessResearchCompletion(string nodeId, IResearchUnlockSink? sink = null) {
public int SynchronizeCompletedResearch(IEnumerable<string> completedNodeIds, IResearchUnlockSink? sink = null) {
public ResearchUnlockState CaptureState() {
public void RestoreState(ResearchUnlockState? state) {
public bool HasCapability(string capabilityId) {
public bool HasRecipe(string recipeId) {
public bool HasItem(string itemId) {
public bool HasUnlock(string unlockId) {
public IReadOnlyList<string> UnlockedCapabilities => _state.unlockedCapabilities;
public IReadOnlyList<string> UnlockedRecipes => _state.unlockedRecipeIds;
public IReadOnlyList<string> GrantedItems => _state.grantedItems;
public IReadOnlyList<string> GrantedUnlockIds => _state.grantedUnlockIds;
public ResearchUnlockCensus GetCensus() {
public readonly struct ResearchUnlockCensus
public readonly int TotalCatalogUnlocks;
public readonly int GrantedUnlocksCount;
public readonly int GrantedItemsCount;
public readonly int UnlockedRecipesCount;
public readonly int UnlockedCapabilitiesCount;
```


# Appendix B.07 — Current Code Architecture: `src/UI/WorkshopPanel.cs`

### `src/UI/WorkshopPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 506 lines / 23023 bytes.
- SHA-256: `e7dd89dee09c6646242f336611059b508f793db24a25d11f5816371c4a6943a0`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class WorkshopPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _shelterWorkshop != null || _legacyWorkshop != null;
public void Bind( ShelterWorkshopSystem workshop, Ashfall.Core.Inventory.Inventory inventory, EquipmentConditionSystem? equipment = null, ExpeditionVehicleSystem? vehicles = null, SurvivorsHostSession? survivors = null)
public void Bind( WorkshopReverseEngineeringSystem workshop, Ashfall.Core.Inventory.Inventory inventory, SurvivorsHostSession? survivors = null) {
public void BindRelicWorkshop( WorkshopReverseEngineeringSystem workshop, Ashfall.Core.Inventory.Inventory inventory, ItemCatalog? itemCatalog = null, SurvivorsHostSession? survivors = null) {
public void Unbind() {
public override void _Ready() {
public override void _ExitTree() {
public void Open() { Visible = true; RefreshView(); }
public void RefreshView() {
```


# Appendix B.08 — Current Code Architecture: `src/Main.FlagshipInstitutions.cs`

### `src/Main.FlagshipInstitutions.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 579 lines / 26298 bytes.
- SHA-256: `583894a805a235c894e1dfb161df6b1834511001e727b4f0480a25d2162cd433`.
- Architecture signals: seeded references=1; save/restore symbols=8; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public string RecordArchiveChronicle( string eventType, string summaryKey, IReadOnlyList<string>? participants = null, string authorId = "") {
internal sealed class HostFactionStandingPort : IFactionStandingPort
public float GetStanding(string factionId) =>
public void AdjustStanding(string factionId, float delta, string reasonCode) =>
internal sealed class HostSurvivorSkillsPort : ISurvivorSkillsPort
public bool HasSkill(string survivorId, string skillId) =>
internal sealed class HostSurvivorConditionPort : ISurvivorConditionPort
public bool HasCondition(string survivorId, string conditionId) {
public int GetAcuteStressPermille(string survivorId) {
public void ApplyAcuteStressReduction(string survivorId, int permille) {
public void ApplyRecoveryProgress(string survivorId, int progress) {
public void SuppressReversibleCondition(string survivorId, string conditionId) {
public int GetRelationshipTrust(string therapistId, string patientId) => 50;
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
```


# Appendix B.09 — Current Code Architecture: `src/UI/ArchiveDeskPanel.cs`

### `src/UI/ArchiveDeskPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 302 lines / 14057 bytes.
- SHA-256: `ebef244e3e3db9738bed43807cbb6244322a490ba9f17f9d101e4a8d76346e99`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=3; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class ArchiveDeskPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _host != null;
public void Bind(ArchiveDeskHostSession session) {
public void Unbind() {
public override void _Ready() {
public void Open() {
public void RefreshView() {
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
```


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/relic_recipes.json`

### `Assets/StreamingAssets/Data/relic_recipes.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 28485 bytes / 28477 characters.
- SHA-256: `0058b68b94b1a287f6b06639d910afd25c85936a3e5104141d7c67fc18d071b3`.
- Root keys: `recipes`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
recipes: min=39, max=39, observed_paths=1
recipes[].required_components: min=3, max=3, observed_paths=2
```

Representative record fields:

- `category`
- `description`
- `dialogue_event_id`
- `dismantle_yield_amount`
- `dismantle_yield_item`
- `display_name`
- `morale_bonus`
- `relic_id`
- `repair_time_hours`
- `required_components`
- `research_unlock_id`
- `restoration_text`
- `world_flag`

Representative identifiers (ordered, capped for readability):

```text
gramophone
film_projector
ham_radio
music_box
typewriter
camera
mantel_clock
sewing_machine
telescope
hand_printing_press
violin
laboratory_microscope
brass_compass
box_kite
coffee_grinder
relic_hand_crunch_battery_charger
relic_water_filter_cartridge_press
relic_signal_mirror_array
relic_thermoelectric_junction
relic_hand_drill_generator
relic_pressure_lantern_mantle
relic_mechanical_timer_switch
relic_improvised_anemometer
relic_micro_dosimeter_pen
relic_water_condenser_coil
relic_signal_amplifier_stage
relic_battery_reconditioner
relic_hydroponic_nutrient_doser
relic_uv_sterilizer_wand
relic_hand_centrifuge
relic_seismic_geophone
relic_automated_turret_controller
relic_field_encrypted_radio
relic_portable_radar_scope
relic_power_armor_servo
relic_vault_seal_breach_charges
relic_iff_transponder
relic_cbrn_filter_bank
relic_field_surgical_robot_arm
```


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/narrative/relic_provenance_dossiers.json`

### `Assets/StreamingAssets/Data/narrative/relic_provenance_dossiers.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 25448 bytes / 25446 characters.
- SHA-256: `8d1240cd36f1ce80b5d347a66ecea0a1adbd9c995d79b981bbf0fd70fb496695`.
- Root keys: `collection_id`, `relics`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
relics: min=32, max=32, observed_paths=1
relics[].tags: min=5, max=5, observed_paths=2
```

Representative record fields:

- `curator_note`
- `discovery_location`
- `gameplay_effect`
- `material`
- `name`
- `relic_id`
- `tags`
- `tone`

Representative identifiers (ordered, capped for readability):

```text
relic_01_reverse_pocketwatch
relic_02_indestructible_processed_cheese
relic_03_hydraulic_exo_gauntlet
relic_04_lead_lined_music_box
relic_05_frozen_moth_cylinder
relic_06_inflatable_rubber_flamingo
relic_07_tungsten_harpoon_point
relic_08_vel_stained_scalpel
relic_09_the_singing_vacuum_tube
relic_10_the_disco_cassette
relic_11_armored_snow_sled
relic_12_the_black_parchment_contract
relic_13_the_mercury_barometer_flask
relic_14_the_taxidermy_badger
relic_15_portable_gamma_spectrometer
relic_16_the_charred_accordion
relic_17_the_phantom_compass
relic_18_the_novelty_alarm_clock
relic_19_heavy_salvage_plasma_torch
relic_20_the_first_harvest_ear_of_rye
relic_21_the_lead_encased_lens
relic_22_the_rubber_chicken_decoy
relic_23_solid_fuel_cutting_lance
relic_24_the_unspoken_triage_list
relic_25_the_telegraph_key_in_amber
relic_26_the_indestructible_bowler_hat
relic_27_pneumatic_rail_spear
relic_28_the_lead_coffin_nameplate
relic_29_the_glow_in_the_dark_dice
relic_30_the_battery_powered_bubble_gun
relic_31_hydraulic_mine_jack
relic_32_the_valley_constitution_chisel
```


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/cultural_archive_tomes.json`

### `Assets/StreamingAssets/Data/cultural_archive_tomes.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 9530 bytes / 9530 characters.
- SHA-256: `1b6df00edcd9b2271ddeddf7c77b351cd82a2c103709f42f065c0a50f16f111c`.
- Root keys: `schema_version`, `tomes`.

Array-path census (minimum, maximum, observed rows):

```text
tomes: min=12, max=12, observed_paths=1
tomes[].microfiche_costs: min=1, max=2, observed_paths=2
tomes[].restoration_costs: min=2, max=2, observed_paths=2
tomes[].tags: min=2, max=2, observed_paths=2
```

Representative record fields:

- `category`
- `description`
- `display_name`
- `initial_degradation_permille`
- `knowledge_bonus`
- `microfiche_costs`
- `microfiche_frame_density`
- `morale_effect`
- `paper_brittleness_tier`
- `restoration_costs`
- `tags`
- `tome_id`
- `transcription_days`


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Inventory/ItemLoreSystemTests.cs`

### `Ashfall.Core.Tests/Inventory/ItemLoreSystemTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 237; SHA-256: `b508826fb3d3e7c62f953b4e72a2585d36851a8bc8624a16de6f4ae7106bdf40`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Constructor_CapturedState_DoesNotAliasInput
Restore_StaleNextSequence_DoesNotCreateDuplicateLoreIds
Queries_ReturnReadOnlySnapshots
Restore_NullEntries_FailsClosed
TransferOwnership_UsesCanonicalItemIdInEvent
RegisterItem_WithCrafter_AddsInitialCraftingLore
RegisterItem_WithDiscovery_AddsDiscoveryLore
TransferOwnership_UpdatesOwnershipChain_AndAddsLore
AddLore_IncreasesLoreCount_AndUpgradesSignificanceLevel
CaptureState_And_RestoreState_RoundTripsCorrectly
```


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/RelicProvenanceCatalogTests.cs`

### `Ashfall.Core.Tests/RelicProvenanceCatalogTests.cs`

- Current test declarations: Fact=2, Theory=0, InlineData=0.
- File lines: 81; SHA-256: `2c9c709a0ba0c5e0f0a42dd8f3b85c43f93785bbc0ec53d8194fb387d4ea7502`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RelicProvenance_LoadsAll32MasterDossiers
RelicProvenance_AllEntriesHaveValidCuratorNotesAndEffects
```


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/CulturalArchiveVaultTests.cs`

### `Ashfall.Core.Tests/CulturalArchiveVaultTests.cs`

- Current test declarations: Fact=16, Theory=0, InlineData=0.
- File lines: 404; SHA-256: `8e0a51e3c2bbdba239ed8bd4f606c0205a4624940e526f29ed9484b26085d99f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CatalogLoad_SeedsTwelveAuthoritativeDocuments
Restoration_ConsumesAtomically_AndRelieves
Transcription_ProgressesDeterministically_ToCompletion
Microfiche_PreservesKnowledge_AndBlocksDuplicateUnlock
DiscCutting_ConsumesBlank_AndResolvesInPlaybackSystem
Salon_AppliesOncePerDay_NeverStacks_AndCooldowns
Chronicle_RecordsStructuredMilestone_Once
Restoration_UnknownDocument_AndMissingInputs_FailAtomically
Transcription_RejectsUnknownDoc_AndUnavailableScholar
DiscCutting_RejectsUnknownCategory_AndDuplicateIds
Humidity_DrivesDegradation_ThroughAuthoritativeInput
Stabilization_ReducesAuthoredDegradation
LostPaper_DoesNotEraseMicroficheKnowledge
ScholarAssignment_AndProgress_SurviveSaveLoad
OldSave_MissingCultureSection_DefaultsSafely
UninterruptedVsRestored_ContinuationMatches
```


# Appendix D.16 — Existing Focused Test Inventory: `Ashfall.Core.Tests/WorkshopReverseEngineeringSystemTests.cs`

### `Ashfall.Core.Tests/WorkshopReverseEngineeringSystemTests.cs`

- Current test declarations: Fact=17, Theory=0, InlineData=0.
- File lines: 309; SHA-256: `a2eaf2aa8f89cdbfad936009a13666c975fcb7c1e1be214af1561382fdfdc95a`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Examine_ReturnsRelicInfo
Examine_UnknownRelic_Fails
StartDismantle_BeginsJob
StartDismantle_WhenBusy_Blocks
Dismantle_CompletesAndYieldsItems
StartRepair_WithoutComponents_Blocks
StartRepair_WithComponents_ReservesThem
StartRepair_LateComponentMissing_LeavesAllComponentsInInventory
Repair_CompletesAndSetsFlag
StartResearch_UnlocksKnowledgeNode
StartResearch_NoUnlockId_Blocks
TickProgress_ReportsProgress
CancelJob_RefundsComponents
CancelJob_WhenIdle_Blocks
CaptureRestoreState_PreservesCompletedRelics
CaptureRestoreState_PreservesActiveJob
LoadCatalog_PopulatesRelics
```


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/Inventory/InventoryTransaction.cs`

### `Assets/Ashfall.Core/Inventory/InventoryTransaction.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 387 lines / 14478 bytes.
- SHA-256: `2efcf72bae930c0c8ddc43036f44cb41ea02a76a4445233c86319df02578ecdb`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public readonly struct InventoryBillItem : IEquatable<InventoryBillItem>
public string ItemId { get; }
public int Amount { get; }
public ItemDefinition? Definition { get; }
public bool Equals(InventoryBillItem other) =>
public override bool Equals(object? obj) => obj is InventoryBillItem other && Equals(other);
public override int GetHashCode() => StableHash.Combine(StableHash.Of(ItemId), Amount);
public override string ToString() => $"{Amount}x {ItemId}";
public sealed class InventoryBill
public IReadOnlyList<InventoryBillItem> Costs => _costs;
public IReadOnlyList<InventoryBillItem> Grants => _grants;
public bool IsEmpty => _costs.Count == 0 && _grants.Count == 0;
public InventoryBill AddCost(string itemId, int amount, ItemDefinition? def = null) {
public InventoryBill AddCost(ItemDefinition def, int amount) {
public InventoryBill AddGrant(string itemId, int amount, ItemDefinition? def = null) {
public InventoryBill AddGrant(ItemDefinition def, int amount) {
public Dictionary<string, int> GetAggregatedCosts() {
public Dictionary<string, int> GetAggregatedGrants() {
public static InventoryBill FromCosts(IReadOnlyDictionary<string, int> costs) {
public static InventoryBill FromCosts(IEnumerable<KeyValuePair<string, int>> costs) {
public static InventoryBill FromCosts(IEnumerable<string> itemIds) {
public static InventoryBill FromCostsAndGrants( IReadOnlyDictionary<string, int>? costs, IReadOnlyDictionary<string, int>? grants) {
public enum InventoryTransactionStatus
public sealed class InventoryTransactionValidationResult
public bool IsValid => Status == InventoryTransactionStatus.Success;
public InventoryTransactionStatus Status { get; }
public string FailureReason { get; }
public string FailedItemId { get; }
public int RequiredAmount { get; }
public int AvailableAmount { get; }
public static InventoryTransactionValidationResult Success() =>
public static InventoryTransactionValidationResult Insufficient(string itemId, int required, int available) =>
public static InventoryTransactionValidationResult CapacityExceeded(int requiredSlots, int availableSlots) =>
public static InventoryTransactionValidationResult WeightExceeded(float currentWeight, float addedWeight, float maxWeight) =>
public static InventoryTransactionValidationResult Invalid(string reason) =>
public static InventoryTransactionValidationResult Cancelled() =>
public static InventoryTransactionValidationResult CallbackError(string message) =>
public override string ToString() => IsValid ? "Valid" : $"{Status}: {FailureReason}";
public sealed class InventoryTransactionQuote
public InventoryBill Bill { get; }
public IReadOnlyDictionary<string, int> AggregatedCosts { get; }
public IReadOnlyDictionary<string, int> AggregatedGrants { get; }
public float TotalCostWeight { get; }
public float TotalGrantWeight { get; }
public float NetWeightChange => TotalGrantWeight - TotalCostWeight;
public InventoryTransactionValidationResult Validation { get; }
public bool CanExecute => Validation.IsValid;
internal sealed class InventorySnapshot
public int Capacity { get; }
public float MaxWeight { get; }
public List<InventorySlot> Slots { get; }
public List<EquippedItem> Equipped { get; }
public sealed class InventoryTransaction : IDisposable
public InventoryBill Bill { get; }
public InventoryTransactionValidationResult Validation { get; }
public bool IsCommitted => _isCommitted;
public bool IsCancelled => _isCancelled;
public bool IsActive => !_isCommitted && !_isCancelled;
public bool TryCommit(Action? onCommitted = null) {
public void Cancel() {
public void Dispose() {
```


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/Memorial/MemorialSystem.cs`

### `Assets/Ashfall.Core/Memorial/MemorialSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 398 lines / 16582 bytes.
- SHA-256: `84e5e276077295bca654304faa02f9ef4b2cdfa48ec0e811323266f3087c29b1`.
- Architecture signals: seeded references=2; save/restore symbols=4; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum DeathQuality
public enum MemorialOutcome
public interface IGriefSink
public sealed class CapturingGriefSink : IGriefSink
public sealed class DispersionRecord
public string DeceasedId = string.Empty;
public List<string> SurvivngRelationshipIds = new List<string>();
public float GriefApplied;
public DeathQuality Quality;
public int Day;
public float QualityScale;
public List<string> Warnings = new List<string>();
public List<DispersionRecord> Records { get; } = new List<DispersionRecord>();
public void ApplyDispersion( string deceasedId, IReadOnlyList<string> survivingRelationshipIds, float baseGriefAmount, DeathQuality quality, int day)
public static float QualityScale(DeathQuality quality) => quality switch
public sealed class MemorialSystem
public event Action<MemorialEntry>? OnMemorialized;
public event Action<MemorialEntry>? OnMourned;
public IGriefSink? GriefSink { get; set; }
public ProceduralEulogyEngine? EulogyEngine { get; set; }
public GraveEpitaphCatalog? EpitaphCatalog { get; set; }
public ISeededRng? EpitaphRng { get; set; }
public IReadOnlyList<MemorialEntry> Entries => _state.Entries;
public ActionResult Mourn(string deceasedId, int day) {
public MemorialEntry? LatestUnmourned() {
public MemorialEntry Memorialize(MemorialInput input) {
public MemorialState CaptureState() => _state.Capture();
public void RestoreState(MemorialState state) {
public sealed class MemorialEntry
public string SurvivorId;
public string Cause;
public int Day;
public int SurvivedDays;
public bool FinalWishResolved;
public string Epitaph;
public string EulogyText = string.Empty;
public string HeirloomItemId;
public string HeirloomRecipientId;
public float MoraleDelta;
public DeathQuality DeathQuality = DeathQuality.Peaceful;
public MemorialOutcome Outcome = MemorialOutcome.Burial;
public int MournedDay = -1;
public sealed class MemorialInput
public string SurvivorId;
public string Cause;
public int Day;
public int BirthDay;
public bool FinalWishResolved;
public string Epitaph;
public string? EulogyText;
public DwellerLifeRecord? LifeRecord;
public string HeirloomItemId;
public string HeirloomRecipientId;
public float MoraleDelta;
public DeathQuality DeathQuality = DeathQuality.Peaceful;
public MemorialOutcome Outcome = MemorialOutcome.Burial;
public IReadOnlyList<string>? SurvivingRelationshipIds;
public sealed class MemorialState
public List<MemorialEntry> Entries = new List<MemorialEntry>();
public MemorialState Capture() {
public void RestoreInto(MemorialState state) {
```


# Appendix E.19 — Supporting Code Evidence: `Assets/Ashfall.Core/Inventory/Inventory.cs`

### `Assets/Ashfall.Core/Inventory/Inventory.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1352 lines / 53625 bytes.
- SHA-256: `38b5c11e1b92bf7d767b47beb3ff290b3dfc8360233858a7dd6e2854c3e264a9`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=29; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public struct EquippedGearData
public float RadProtection;
public float MaxDurability;
public float CurrentDurability;
public float DegradeRate;
public class WornGear
public EquippedItem? SourceEquipped;
public IEquipmentConditionSink? ConditionSink;
public ItemDefinition? SourceItem;
public float RadProtection;
public float MaxDurability;
public float CurrentDurability;
public float DegradeRate;
public Action<float>? OnDegraded;
public float DurabilityFraction() {
public float EffectiveProtection() {
public void Degrade(float gameHours) {
public class Inventory : IPlayerInventoryPort, IEquipmentConditionSink
public const float ContaminationDosePerUnit = 50f;
public int Capacity = 20;
public float MaxWeight = 100f;
public IReadOnlyList<InventorySlot> Slots => _slots;
public IReadOnlyList<EquippedItem> Equipped => _equipped;
public IReadOnlyList<InventorySlot> GetSlots() => _slots;
public event Action<ItemDefinition, int> OnItemAdded;
public event Action<ItemDefinition, int> OnItemRemoved;
public event Action OnInventoryChanged;
public bool HasSufficient(string itemId, int count) {
public bool TryConsume(string itemId, int count, Action? onCommitted = null) {
public bool TryConsumeById(string itemId, int count) => TryConsume(itemId, count);
public bool TryProduce(string itemId, int count, ItemDefinition? def = null) {
public bool Remove(string itemId, int amount) => RemoveById(itemId, amount);
public int Count(ItemDefinition item) {
public int CountById(string itemId) {
public int CountByType(ItemType type) {
public int RemoveByType(ItemType type, int amount) {
public float FoodFillRatio() {
public float WaterFillRatio() {
public float FuelFillRatio() {
public InventorySlot? FindSlot(string itemId) {
public InventorySlot? FindBestWorkingDevice(string itemId) {
public bool HasWorkingGeiger() {
public DeviceState? GetBestGeigerState() {
public void DriftAllDevices(float days = 1f) {
public bool RechargeDevice(string deviceItemId, ItemDefinition batteryItem) {
public bool RecalibrateDevice(string deviceItemId, ItemDefinition kitItem, int currentDay) {
public float GetCurrentWeight() {
public bool CanAdd(ItemDefinition item, int amount) {
public bool Add(ItemDefinition item, int amount) {
public bool AddById(string itemId, int amount) {
public bool CanAddById(string itemId, int amount) {
public bool Remove(ItemDefinition item, int amount) {
public bool RemoveById(string itemId, int amount) {
public void Clear() {
public InventoryTransactionValidationResult ValidateTransaction( InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
public InventoryTransactionQuote Quote( InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
public InventoryTransactionQuote QuoteTransaction( InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
public InventoryTransaction BeginTransaction( InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
public bool TryExecuteTransaction( InventoryBill bill, Action? onCommitted = null, Func<string, ItemDefinition?>? lookup = null) {
public bool TryConsumeBill(IReadOnlyDictionary<string, int> costs, Action? onCommitted = null) {
public bool TryConsumeBill(IEnumerable<KeyValuePair<string, int>> costs, Action? onCommitted = null) {
public bool TryConsumeBill(IEnumerable<string> itemIds, Action? onCommitted = null) {
internal void ApplyTransactionMutations(InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
internal void NotifyTransactionCommitted(InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
internal void RestoreSnapshot(InventorySnapshot snapshot) {
public bool Transfer(ItemDefinition item, int amount, Inventory destination) {
public bool Equip(ItemDefinition item) {
public bool Equip( ItemDefinition item, IEnumerable<Medical.LimbState>? limbs, Func<string, ItemDefinition?>? catalog = null, Func<string, float>? conditionProvider = null) {
public ItemDefinition? Unequip(EquipSlot slot) {
public bool TryUnequipTo(EquipSlot slot, Inventory destination) {
public EquippedItem? GetEquipped(EquipSlot slot) {
public float GetEquippedProtection() {
public List<WornGear> BuildWornGear() {
public void FillWornGear(List<WornGear> buffer) {
public event Action<EquippedItem, string>? OnProtectiveGearFailed;
public bool TryRepairEquippedGear(EquippedItem item) {
public event Action<EquippedItem, float>? OnProtectiveGearRepaired;
public void RecordWear(EquippedItem item, float wearDelta, string cause = "radiation") {
public void DegradeEquippedGear(float gameHours, float multiplier = 1f, string cause = "use") {
public sealed class ProtectiveLifeEstimate
public string ItemId = string.Empty;
public string DisplayName = string.Empty;
public float CurrentDurability;
public float MaxDurability;
public float DegradeRate;
public float ExposureMultiplier = 1f;
public float HoursRemaining;
public bool TryEstimateWeakestProtectiveLife( float exposureMultiplier, out ProtectiveLifeEstimate? life) {
public bool Consume( ItemDefinition item, Func<ItemType, float, bool>? applyNeed = null, Action<float>? applyRadCleanse = null, Action? applyIodine = null, Action<float>? applyContamination = null,
public InventorySaveState CaptureState() {
public void ResortSlotsByType() {
public bool IsSortedByType() {
public void RestoreState(InventorySaveState state, Func<string, ItemDefinition?> lookup) {
public class InventorySlot
public ItemDefinition Item;
public int Amount;
public DeviceState? Device;
public float CurrentDurability = -1f;
public float GetDurability() {
public bool IsBrokenOrDegraded() {
public class EquippedItem
public ItemDefinition Item;
public float CurrentDurability;
public class InventorySaveState
public int capacity;
public float maxWeight;
public List<SlotSave> slots = new List<SlotSave>();
public List<EquippedSave> equipped = new List<EquippedSave>();
public class SlotSave
public string itemId;
public int amount;
public bool hasDevice;
public float battery;
public float calibration;
public bool broken;
public int lastCalibratedDay;
public class EquippedSave
public string itemId;
public float durability;
```


# Appendix E.20 — Supporting Code Evidence: `Assets/Ashfall.Core/Phantoms/HeirloomSystem.cs`

### `Assets/Ashfall.Core/Phantoms/HeirloomSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 485 lines / 19646 bytes.
- SHA-256: `e7abdb5badd964e7572d252e289b7de4a33ec010c8fada6514abd7f364dbb735`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=14; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class HeirloomProvenanceRecord
public string holder_id = string.Empty;
public string acquisition_cause = string.Empty;
public int start_day;
public int end_day;
public string transfer_reason = string.Empty;
public string fate = string.Empty;
public int unlocked_stage_index = 1;
public sealed class HeirloomInstanceState
public string instance_id = string.Empty;
public string heirloom_id = string.Empty;
public string current_holder_id = string.Empty;
public List<HeirloomProvenanceRecord> provenance = new List<HeirloomProvenanceRecord>();
public List<int> unlocked_stages = new List<int> { 1 };
public bool is_memorialized;
public bool is_completed;
public bool is_legacy_selected;
public sealed class HeirloomSystemState
public string systemId = HeirloomSystem.SystemId;
public List<HeirloomInstanceState> instances = new List<HeirloomInstanceState>();
public sealed class HeirloomSystem
public const string SystemId = "heirloom_system";
public const int MaxProvenanceEntriesPerInstance = 24;
public event Action<string, string, string>? OnHeirloomInherited; // instanceId, deceasedId, newHolderId
public event Action<string, string>? OnHeirloomHolderAssigned;    // instanceId, newHolderId
public event Action<string, int>? OnHeirloomStageUnlocked;        // instanceId, stageIndex
public event Action? OnStateChanged;
public IReadOnlyCollection<HeirloomInstanceState> AllInstances => _instances.Values;
public HeirloomInstanceState? GetInstance(string instanceId) {
public List<HeirloomInstanceState> GetHeirloomsForHolder(string holderId) {
public float GetHolderMoraleModifier(string holderId) {
public float GetHolderFatigueRelief(string holderId) {
public HeirloomInstanceState CreateInstance( string heirloomId, string initialHolderId, int currentDay, string acquisitionCause = "initial_discovery") {
public bool AssignHolder(string instanceId, string newHolderId, int currentDay, string transferReason = "manual_transfer") {
public int HandleSurvivorDeath( string deceasedId, int currentDay, GenerationalLineageExtension? lineage = null, SurvivorRelationsSystem? relations = null) {
public void SetMemorialized(string instanceId, bool memorialized) {
public void SetLegacySelected(string instanceId, bool selected) {
public HeirloomSystemState CaptureState() {
public void RestoreState(HeirloomSystemState state) {
```


# Appendix E.21 — Supporting Code Evidence: `src/Main.Inventory.cs`

### `src/Main.Inventory.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 269 lines / 11760 bytes.
- SHA-256: `7f22de47729189d6c5ab6223cd95c137e04d977b50300fcb8f608a796f456ddc`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/Memorial/MemorialComponentTests.cs`

### `Ashfall.Core.Tests/Memorial/MemorialComponentTests.cs`

- Current test declarations: Fact=21, Theory=0, InlineData=0.
- File lines: 621; SHA-256: `0740549fabcd1133c01ba93cfef51a28d0b6c208e98bdc3a8e7063089258b0e2`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Store_UsesExpectedMetadataAndEmptyHistoryLedger
Record_IsIdempotent_FirstRecordWinsWithoutEventsOrLifecycleAuthority
Store_CaptureIsOrdinalDetachedAndContainsEveryHistoricalField
DetachedState_RoundTripsAndPreservesWireShape
Restore_RejectsNullInvalidAndDuplicateRows_FirstRowWins
Restore_FutureSchemaAndWrongSystemPreserveCurrentState
Restore_NullStateIsTheExplicitEmptyResetForm
ReleaseDoesNotEraseHistory_ButResetDoes
Adapter_ImportsAllFieldsPreservesSurvivedDaysAndOrdersOwners
Adapter_ReportsNullInvalidDuplicateUnknownAndLivingRows
Adapter_MapsNullableLegacyStringsToTypedDefaults
Adapter_DoesNotMutateEntityLifecycleOrRevision
Parity_IsCleanForMatchingLegacyAndTypedRows
Parity_ReportsDuplicateMissingExtraAndStableOrdering
Parity_UsesLegacyFirstDuplicateForFieldComparison
Parity_ReportsEveryHistoricalFieldMismatch
Parity_ReportsNullLegacyFieldsAndMalformedIds
Parity_IsDeterministicRegardlessOfLegacyRegistrationOrder
TypedStore_IntegratesWithReferentialIntegrityWithoutRejectingHistory
RetainedHistorySurvivesLivingOwnerRemoval
MemorialSave_CoreWireFieldsAndChecksumRemainDirectV1
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs`

### `Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 125; SHA-256: `c8dc55caa2fa596cae8d88ff9038bb4d00e330061a76bef7c6c1945030eff046`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Memorialize_AddsEntry
Memorialize_IsIdempotent_NoDuplicate
Memorialize_DifferentSurvivors_BothAdded
Memorialize_DefaultsCauseWhenMissing
Memorialize_BlankCauseIsReplaced
Memorialize_RequiresSurvivorId
Events_FireOnMemorialize
Idempotency_DoesNotFireEventTwice
CaptureRestore_RoundTrip
HeirloomTransfer_AtomicInEntry
```


# Appendix G.24 — Supporting Regression Evidence: `Ashfall.Core.Tests/Memorial/MemorialGriefPortTests.cs`

### `Ashfall.Core.Tests/Memorial/MemorialGriefPortTests.cs`

- Current test declarations: Fact=7, Theory=2, InlineData=6.
- File lines: 234; SHA-256: `2edcb5891c1f0fa106613d599d5c7487a140034e9426d23215c1741c4ea08112`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CapturingGriefSink_QualityScaleMatchesSpec
Memorialize_FiresGriefSink_OnceOnFirstCall
Memorialize_DifferentQualities_ProduceDifferentGrief
Memorialize_WithNullGriefSink_DoesNotThrow
MemorialEntry_RoundTrips_DeathQuality_And_Outcome
MemorialEntry_DefaultsTo_PeacefulAndBurial_WhenInputOmitted
CaptureAndRestore_Preserves_DeathQuality_And_Outcome
LegacyEntry_WithoutDeathQuality_LoadsAsPeaceful_Default
SurvivngRelationshipIds_NullSafe
```


# Appendix G.25 — Supporting Regression Evidence: `Ashfall.Core.Tests/Inventory/UnifiedInventoryOwnershipTests.cs`

### `Ashfall.Core.Tests/Inventory/UnifiedInventoryOwnershipTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 219; SHA-256: `35602f17a17a1925a64f5bc3af71b766c8d0e58b385283f755d75fc2430a749e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CanonicalAliases_MapLegacyAndPrefixedIds_ToAuthoritativeIds
Inventory_ImplementsPlayerInventoryPort_WithAtomicTransactions
HoldfastTrading_TransactsAgainstAuthoritativeInventory_AndMaintainsSeparateMerchantStock
TryRestoreState_WithBackingInventory_DoesNotDiscardUnrelatedPlayerItems
LegacySaveMigration_MergesHoldings_WithoutDuplication
CrossSystemFlow_HarvestToConsumeToTrade_OperatesOnSingleLedger
ProvenanceRecords_TrackMutationCausality
```


# Appendix H.26 — Supporting Authority Document: `docs/culture/PLAN_178_190_CREATION_LORE_AUTHORITY_MAP.md`

### `docs/culture/PLAN_178_190_CREATION_LORE_AUTHORITY_MAP.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 63 lines / 3063 bytes.
- SHA-256: `a7c3a88807a5e2f1a9b26bf2f50fd14e410f0e2b820915a72b225efaa0b77bb1`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.27 — Supporting Authority Document: `docs/crafting/RELIC_RESTORATION_RUNTIME_CONTRACT.md`

### `docs/crafting/RELIC_RESTORATION_RUNTIME_CONTRACT.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 68 lines / 5384 bytes.
- SHA-256: `980f876604d5283317abe07467749943f566ae811409324a0c229a9a1731b553`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| restoration event and output | WorkshopReverseEngineeringSystem | instance provenance, ownership and significance | ItemLoreSystem | Owner emits/reads a typed fact; no mirror state. |
| restoration event and output | WorkshopReverseEngineeringSystem | authored historical dossier | RelicProvenanceCatalog | Owner emits/reads a typed fact; no mirror state. |
| restoration event and output | WorkshopReverseEngineeringSystem | archive stewardship/restoration projects | CulturalArchiveVaultSystem | Owner emits/reads a typed fact; no mirror state. |
| restoration event and output | WorkshopReverseEngineeringSystem | choice consequences | Research/market/memorial | Owner emits/reads a typed fact; no mirror state. |
| instance provenance, ownership and significance | ItemLoreSystem | restoration event and output | WorkshopReverseEngineeringSystem | Owner emits/reads a typed fact; no mirror state. |
| instance provenance, ownership and significance | ItemLoreSystem | authored historical dossier | RelicProvenanceCatalog | Owner emits/reads a typed fact; no mirror state. |
| instance provenance, ownership and significance | ItemLoreSystem | archive stewardship/restoration projects | CulturalArchiveVaultSystem | Owner emits/reads a typed fact; no mirror state. |
| instance provenance, ownership and significance | ItemLoreSystem | choice consequences | Research/market/memorial | Owner emits/reads a typed fact; no mirror state. |
| authored historical dossier | RelicProvenanceCatalog | restoration event and output | WorkshopReverseEngineeringSystem | Owner emits/reads a typed fact; no mirror state. |
| authored historical dossier | RelicProvenanceCatalog | instance provenance, ownership and significance | ItemLoreSystem | Owner emits/reads a typed fact; no mirror state. |
| authored historical dossier | RelicProvenanceCatalog | archive stewardship/restoration projects | CulturalArchiveVaultSystem | Owner emits/reads a typed fact; no mirror state. |
| authored historical dossier | RelicProvenanceCatalog | choice consequences | Research/market/memorial | Owner emits/reads a typed fact; no mirror state. |
| archive stewardship/restoration projects | CulturalArchiveVaultSystem | restoration event and output | WorkshopReverseEngineeringSystem | Owner emits/reads a typed fact; no mirror state. |
| archive stewardship/restoration projects | CulturalArchiveVaultSystem | instance provenance, ownership and significance | ItemLoreSystem | Owner emits/reads a typed fact; no mirror state. |
| archive stewardship/restoration projects | CulturalArchiveVaultSystem | authored historical dossier | RelicProvenanceCatalog | Owner emits/reads a typed fact; no mirror state. |
| archive stewardship/restoration projects | CulturalArchiveVaultSystem | choice consequences | Research/market/memorial | Owner emits/reads a typed fact; no mirror state. |
| choice consequences | Research/market/memorial | restoration event and output | WorkshopReverseEngineeringSystem | Owner emits/reads a typed fact; no mirror state. |
| choice consequences | Research/market/memorial | instance provenance, ownership and significance | ItemLoreSystem | Owner emits/reads a typed fact; no mirror state. |
| choice consequences | Research/market/memorial | authored historical dossier | RelicProvenanceCatalog | Owner emits/reads a typed fact; no mirror state. |
| choice consequences | Research/market/memorial | archive stewardship/restoration projects | CulturalArchiveVaultSystem | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Create a post-restoration choice read model/command seam over current ItemLore and workshop facts. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Route study through ResearchUnlockBridge, trade through Market/trade owners, memorial through Memorial/CulturalArchive. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Define reversible/terminal disposition semantics explicitly. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Support legacy items with no provenance record through deterministic backfill-on-observation, not fabricated history. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.559 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/MemorialComponentParity.cs`

### `Assets/Ashfall.Core/Survivors/MemorialComponentParity.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 387 lines / 15460 bytes.
- SHA-256: `f03db0515f61d1e3c861a54bff66d264f3075bd12fe586dc0d8397c5e42ccb1f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class MemorialParityCode
public const string LegacyRowNull = "legacy_row_null";
public const string LegacyIdInvalid = "legacy_id_invalid";
public const string LegacyDuplicateId = "legacy_duplicate_id";
public const string LegacyFieldNull = "legacy_field_null";
public const string TypedRecordMissing = "typed_record_missing";
public const string TypedRecordExtra = "typed_record_extra";
public const string FieldMismatch = "field_mismatch";
public sealed class MemorialParityFinding
public string Code { get; }
public SurvivorId SurvivorId { get; }
public string RawId { get; }
public string Field { get; }
public string Expected { get; }
public string Actual { get; }
public string Message { get; }
public override string ToString() {
public sealed class MemorialParityReport
public int LegacyRows { get; internal set; }
public int TypedRows { get; internal set; }
public List<MemorialParityFinding> Findings { get; } = new List<MemorialParityFinding>();
public bool IsMatch => Findings.Count == 0;
public int FindingCount => Findings.Count;
public string Describe() {
public override string ToString() => $"[MemorialParity] legacy={LegacyRows} typed={TypedRows} findings={Findings.Count}";
public static class MemorialComponentParity
public static MemorialParityReport Compare( IReadOnlyList<MemorialEntry> legacyEntries, MemorialComponentStore typed) {
```


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/HoldfastTradeSession.cs`

### `Assets/Ashfall.Core/HoldfastTradeSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1130 lines / 51598 bytes.
- SHA-256: `68e3c2b379443e5c780d7aba9b7cdf430a9e95892f44278816ca6412ebc49708`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=1; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum HoldfastFactionStance
public enum HoldfastTradeFailure
public sealed class HoldfastTradeInventorySlot
public HoldfastItemDefinition Item { get; }
public int Amount { get; }
public sealed class HoldfastTradeInventory
public int Capacity { get; set; } = 20;
public float MaxWeight { get; set; } = 100f;
public int OccupiedCount => _backingInventory != null ? _backingInventory.Slots.Count : _items.Count;
public float GetCurrentWeight() {
public bool CanAdd(string itemId, int count) {
public bool AddItem(string itemId, int count) {
public void RemoveItem(string itemId, int count) {
public bool HasSufficient(string itemId, int count) {
public bool ValidateBill(IReadOnlyDictionary<string, int> bill) {
public bool TryConsumeBill(IReadOnlyDictionary<string, int> bill, Action? onCommitted = null) {
public void Clear() {
public sealed class HoldfastTradeResult
public bool Success { get; set; }
public string ItemId { get; set; } = string.Empty;
public int Quantity { get; set; }
public string FactionId { get; set; } = string.Empty;
public int TotalValue { get; set; }
public string Message { get; set; } = string.Empty;
public string WhyLine { get; set; } = string.Empty;
public HoldfastTradeFailure Failure { get; set; } = HoldfastTradeFailure.None;
public int FundsDelta { get; set; }
public Economy.FundsFailure FundsFailure { get; set; } = Economy.FundsFailure.None;
public static HoldfastTradeResult Ok(string itemId, int quantity, string factionId, int totalValue, string whyLine = "", int fundsDelta = 0) => new HoldfastTradeResult { Success = true, ItemId = itemId, Quantity = quantity, FactionId = factionId, TotalValue = totalValue, Message = "Trade completed.", WhyLine = whyLine, FundsDelta = fundsDelta };
public static HoldfastTradeResult Fail(string message, HoldfastTradeFailure failure = HoldfastTradeFailure.None, Economy.FundsFailure fundsFailure = Economy.FundsFailure.None) => new HoldfastTradeResult { Success = false, Message = message, Failure = failure, FundsFailure = fundsFailure };
public sealed class HoldfastTradeSession
public const int DefaultInventoryCapacity = 20;
public HoldfastTradeInventory Inventory { get; }
public Inventory.Inventory? PlayerInventory => _playerInventory;
public string SelectedFactionId { get; private set; } = string.Empty;
public Func<string, bool>? EmbargoQuery { get; set; }
public Func<string, HoldfastFactionStance>? StanceQuery { get; set; }
public event Action StateChanged;
public bool SelectFaction(string factionId) {
public void SeedInventory(string itemId, int count) {
public void ResetToDefaults() {
public int GetHeld(string itemId) {
public int GetStock(string itemId) {
public void SetStock(string itemId, int count) {
public long Value => _value;
public long PlayerValue => _value;
public bool CanDebitValue(long amount) => amount >= 0 && amount <= _value;
public bool CanCreditValue(long amount) =>
public bool TryDebitValue(long amount, Func<bool>? secondLeg = null) =>
public bool TryCreditValue(long amount, Func<bool>? secondLeg = null) =>
internal bool TryDebitValueForSettlement(long amount, Func<bool>? secondLeg = null) =>
internal bool TryCreditValueForSettlement(long amount, Func<bool>? secondLeg = null) =>
internal void NotifyExternalValueSettlement() => StateChanged?.Invoke();
public bool TryGetUnitValue(string itemId, out long unitValue) {
public long GetBuyPrice(string itemId, string factionId, int quantity = 1) {
public long GetSellPrice(string itemId, string factionId, int quantity = 1) {
public string GetWhyLine(string itemId, string factionId, bool isBuy) {
public HoldfastTradeResult Buy(string itemId, int quantity, string factionId) {
public HoldfastTradeResult Sell(string itemId, int quantity, string factionId) {
public static int ChitsFromSettlementUnits(float units) {
public HoldfastTradeResult BuyWithFunds(string itemId, int quantity, string factionId, Economy.FundsLedger ledger, int day, string counterpartyId = "holdfast") {
public HoldfastTradeResult SellWithFunds(string itemId, int quantity, string factionId, Economy.FundsLedger ledger, int day, string counterpartyId = "holdfast") {
public bool TryRestoreState(HoldfastTradeSaveState state, out string error) {
public CommandPreview PreviewBuy(string itemId, int quantity, string factionId, long stateVersion = 0) {
public CommandResult ExecuteBuy(string itemId, int quantity, string factionId, long expectedStateVersion = 0, long currentStateVersion = 0) {
public CommandPreview PreviewSell(string itemId, int quantity, string factionId, long stateVersion = 0) {
public CommandResult ExecuteSell(string itemId, int quantity, string factionId, long expectedStateVersion = 0, long currentStateVersion = 0) {
public HoldfastTradeSaveState CaptureState() {
public class HoldfastTradeSaveState
public int schemaVersion = 0;
public long value;
public Dictionary<string, int> held = new Dictionary<string, int>();
public Dictionary<string, int> stock = new Dictionary<string, int>();
```


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/HydroGeologyProjection.cs`

### `Assets/Ashfall.Core/Narrative/HydroGeologyProjection.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 519 lines / 29325 bytes.
- SHA-256: `4a8ffc2ec9a095f5e65e752dd4d0990545a030cd025174a07ab04c8327e32204`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum HydroGeologyRecordFamily
public enum HydroGeologyProvenanceClass
public enum HydroGeologyContaminantClassification
public sealed class HydroGeologyRecordMetadata
public string RecordId { get; set; } = string.Empty;
public HydroGeologyRecordFamily Family { get; set; }
public HydroGeologyProvenanceClass Provenance { get; set; }
public HydroGeologyContaminantClassification ContaminantClass { get; set; } = HydroGeologyContaminantClassification.None;
public string ProducerId { get; set; } = string.Empty;
public string NamedSubject { get; set; } = string.Empty;
public string Channel { get; set; } = "expedition_survey";
public int MinDay { get; set; } = 1;
public bool IsActivated { get; set; }
public string MeasurementProvenance { get; set; } = string.Empty;
public static class HydroGeologyProjection
public static HydroGeologyRecordMetadata? ResolveMetadata(string recordId) {
public static HydroGeologyRecordFamily ResolveFamily(string recordId) {
public static HydroGeologyProvenanceClass ResolveProvenance(string recordId) {
public static string ResolveProducer(string recordId) {
public static int ResolveMinDay(string recordId) {
public static bool IsActivated(string recordId) {
public static IReadOnlyList<HydroGeologyRecordMetadata> GetAllMetadata() {
public static IReadOnlyList<HydroGeologyRecordMetadata> GetActiveRecordsForProducer(string producerId) {
public static IReadOnlyList<HydroGeologyRecordMetadata> GetActivatedRecords() {
public static IReadOnlyList<HydroGeologyRecordMetadata> GetDeferredRecords() {
```


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/MemorialComponentStore.cs`

### `Assets/Ashfall.Core/Survivors/MemorialComponentStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 300 lines / 11785 bytes.
- SHA-256: `27865d26b30d1f5fa997917f702d0c83be04ded4237783b6a30e32655b59e83b`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MemorialRecordState
public string survivor_id = string.Empty;
public string cause = string.Empty;
public int day;
public int survived_days;
public bool final_wish_resolved;
public string epitaph = string.Empty;
public string heirloom_item_id = string.Empty;
public string heirloom_recipient_id = string.Empty;
public float morale_delta;
internal MemorialRecord ToRecord(SurvivorId owner) => new MemorialRecord(
internal static MemorialRecordState Capture(MemorialRecord source) => new MemorialRecordState
public sealed class MemorialComponentStoreState
public const string CurrentSystemId = MemorialComponentStore.SystemId;
public const int CurrentSchemaVersion = MemorialComponentStore.SchemaVersion;
public int schema_version = CurrentSchemaVersion;
public string system_id = CurrentSystemId;
public List<MemorialRecordState> records = new List<MemorialRecordState>();
public sealed class MemorialComponentRestoreReport
public int Accepted { get; internal set; }
public List<string> Rejected { get; } = new List<string>();
public bool IsFatal { get; internal set; }
public string FatalReason { get; internal set; } = string.Empty;
public bool IsClean => !IsFatal && Rejected.Count == 0;
public override string ToString() => IsFatal
public sealed class MemorialRecord
public SurvivorId SurvivorId { get; }
public SurvivorId OwnerId => SurvivorId;
public string Cause { get; }
public int Day { get; }
public int SurvivedDays { get; }
public bool FinalWishResolved { get; }
public string Epitaph { get; }
public string HeirloomItemId { get; }
public string HeirloomRecipientId { get; }
public float MoraleDelta { get; }
public sealed class MemorialComponentStore : ISurvivorComponentStore
public const string SystemId = "memorial_component";
public const int SchemaVersion = 1;
public string ComponentName => "memorial";
public SurvivorComponentCardinality Cardinality => SurvivorComponentCardinality.ZeroOrOne;
public bool RetainsHistoryAfterDeath => true;
public IEnumerable<SurvivorId> OwnerIds => OrderedOwnerIds();
public int Count => _byOwner.Count;
public bool Contains(SurvivorId owner) => !owner.IsEmpty && _byOwner.ContainsKey(owner);
public bool TryGet(SurvivorId owner, out MemorialRecord? record) {
public MemorialRecord Record(MemorialRecord record) {
public bool TryRecord(MemorialRecord record) {
public bool Release(SurvivorId owner) => false;
public void Reset() => _byOwner.Clear();
public MemorialComponentStoreState CaptureState() {
public MemorialComponentRestoreReport RestoreState(MemorialComponentStoreState? saved) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `src/Host/HostCli.PanelTests.cs`

### `src/Host/HostCli.PanelTests.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 4479 lines / 250824 bytes.
- SHA-256: `23b1c0498d6a8cea2b2f69b49d342aed1d444425b6bee4d6c0b144f195e78443`.
- Architecture signals: seeded references=15; save/restore symbols=102; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=3; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=12.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunYearOfAshSaveSelfTest(string dataDirectory) {
public static int RunDutyRosterSaveSelfTest(string dataDirectory) {
public static int RunExpansionHubSaveSelfTest(string dataDirectory) {
public static int RunExpeditionSelfTest() {
public static int RunBridgeSelfTest() {
public static int RunPowerGridCatalogSelfTest() {
public static int RunExpeditionEncounterBridgeSelfTest() {
public static int RunMedicalSelfTest() {
public static int RunNarrativeSelfTest() {
public static int RunOralLoreSelfTest(string dataDirectory) {
public static int RunSurvivorsSelfTest() {
public static int RunWorldSelfTest() {
public static int RunEconomySelfTest(string dataDirectory) {
public static int RunUtilityAiSelfTest(string dataDirectory) {
public static int RunDoseLedgerSelfTest(string dataDirectory) {
public static int RunBlackFlotillaSelfTest(string dataDirectory) {
public static int RunRadioSelfTest() {
public static int RunHoldfastBriefing(string dataDirectory) {
public static int RunIceRoadTickDemo(string dataDirectory) {
public static int RunHoldfastSaveSelfTest(string dataDirectory) {
public static int RunStandaloneSystemsSelfTest() {
public static int RunPhase0SelfTest() {
public static int RunCaravanSelfTest() {
public static int RunAssetRegistrySelfTest(string dataDirectory) {
public static int RunAssetCoverageReport(string dataDirectory) {
public static int RunDay1PlayableSelfTest(string dataDirectory) {
public static int RunDay1ToDay2MilestoneSelfTest(string dataDirectory) {
public static int RunUiLayoutSelfTest(string dataDirectory) {
public static int RunSettingsSelfTest(string dataDirectory) {
public static int RunPlayableShellSelfTest(string dataDirectory) {
public static int RunShelterHazardLoopSelfTest(string dataDirectory) {
public static int RunShelterOperationsSelfTest(string dataDirectory) {
public static string SnapshotGoldenRoot() {
public static string SnapshotCaptureRoot() {
internal sealed class PanelTestFaultyFileIo : Ashfall.Core.IFileIO
public bool DirectoryExists(string path) => true;
public bool FileExists(string path) => true;
public string ReadAllText(string path) => throw new System.IO.IOException("Simulated I/O disk error");
public void WriteAllText(string path, string contents) { }
public string Combine(params string[] parts) => System.IO.Path.Combine(parts);
internal sealed class PanelTestCorruptJsonFileIo : Ashfall.Core.IFileIO
public bool DirectoryExists(string path) => true;
public bool FileExists(string path) => true;
public string ReadAllText(string path) => "{ not valid json syntax !!!";
public void WriteAllText(string path, string contents) { }
public string Combine(params string[] parts) => System.IO.Path.Combine(parts);
```


# Appendix Q.564 — Additional Current Architecture Evidence: `src/Main.UiTests.RealCampaignJourney.cs`

### `src/Main.UiTests.RealCampaignJourney.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 648 lines / 43276 bytes.
- SHA-256: `54dba7778958e0fd3ef887d36d13af4d7d72c42fd51bde7700cbb967f9490fc1`.
- Architecture signals: seeded references=2; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=1; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix Q.566 — Additional Current Architecture Evidence: `src/Host/InventoryHostSession.cs`

### `src/Host/InventoryHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 721 lines / 38324 bytes.
- SHA-256: `89d74d0fe6814c5dc75f9106ac26714153475da73d5f44a46bd2ba6fb12f7e7e`.
- Architecture signals: seeded references=0; save/restore symbols=5; typed event declarations=3; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class InventoryHostSession
public InventoryContainer Inventory { get; }
public ItemCatalog Catalog { get; }
public ItemDescriptionCatalog DescriptionCatalog { get; set; }
public ExpansionEnrichmentCatalog? EnrichmentCatalog { get; set; }
public SurvivorsHostSession? Survivors { get; set; }
public Func<string, ItemType, float, bool>? ApplyNeedOverride { get; set; }
public Func<string, string, int, int, int, ResourceAllocationDecision?>? RationingAuthorizer { get; set; }
public Action<string, float>? ApplyRadCleanseOverride { get; set; }
public Action<string>? ApplyIodineOverride { get; set; }
public Action<string, float>? ApplyContaminationOverride { get; set; }
public Func<string?>? DefaultSurvivorResolver { get; set; }
public string LastEvent { get; private set; } = string.Empty;
public static InventoryHostSession CreateForFixture( InventoryContainer? inventory = null, ItemCatalog? catalog = null) {
public static void SeedCatalogForTest(ItemCatalog catalog) {
public static InventoryHostSession Create( string? dataDir = null, string? startingSuppliesProfileId = null, bool seedWhenNoSave = true) {
public ItemInspectionModel? GetInspection(string itemId) {
public void LoadOrSeedStartingSupplies( string dataDir, IFileIO fileIO = null!, IJsonSerializer serializer = null!, bool failClosed = true, string? profileId = null)
public void SeedStartingSupplies() {
public bool TryAdd(string itemId, int amount) {
public string Add(string itemId, int amount) {
public string Remove(string itemId, int amount) {
public string Equip(string itemId) => EquipResult(itemId).MessageKey;
public ActionResult EquipResult(string itemId) {
public string Unequip(string slotName) {
public string? ResolveTargetSurvivorId(string? requestedSurvivorId = null) {
public string Consume(string itemId, float therapeuticScale = 1f) => ConsumeResult(itemId, null, therapeuticScale).MessageKey;
public string Consume(string itemId, string? survivorId, float therapeuticScale = 1f) => ConsumeResult(itemId, survivorId, therapeuticScale).MessageKey;
public ActionResult ConsumeResult(string itemId, float therapeuticScale = 1f) => ConsumeResult(itemId, null, therapeuticScale);
public ActionResult ConsumeResult(string itemId, string? survivorId, float therapeuticScale = 1f) {
public Action<string, string>? OnConsumed;
public int CurrentDay { get; set; } = 1;
public Action<string, int>? OnAntiRadAdministered { get; set; }
public Action<string, string, Ashfall.Core.Medical.ChemicalDependencyKind>? OnChemicalSubstanceConsumed { get; set; }
public Ashfall.Core.Medical.MedicalRecordLog? MedicalRecordLog { get; set; }
public List<Ashfall.Core.Campaign.DayStateChangeEvent> PendingDayEvents { get; } = new List<Ashfall.Core.Campaign.DayStateChangeEvent>();
public void DrainDayEvents(List<Ashfall.Core.Campaign.DayStateChangeEvent> target) {
public string InventoryLine() {
public string EquipLine() {
public InventorySaveState CaptureSave() => Inventory.CaptureState();
public void RestoreSave(InventorySaveState state) {
```


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/AbyssalAnomaliesProjection.cs`

### `Assets/Ashfall.Core/Narrative/AbyssalAnomaliesProjection.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 420 lines / 20899 bytes.
- SHA-256: `7741934b472493273d28a57f53889342e5dc7d52b9ce100fbc15f0ddd5d95595`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum AbyssalRecordFamily
public enum AbyssalProvenanceClass
public sealed class AbyssalRecordMetadata
public string RecordId { get; set; } = string.Empty;
public AbyssalRecordFamily Family { get; set; }
public AbyssalProvenanceClass Provenance { get; set; }
public string ProducerId { get; set; } = string.Empty;
public string Channel { get; set; } = "location_inspection";
public int MinDay { get; set; } = 1;
public bool IsActivated { get; set; }
public static class AbyssalAnomaliesProjection
public static AbyssalRecordFamily ResolveFamily(string recordId) {
public static AbyssalProvenanceClass ResolveProvenance(string recordId) {
public static string ResolveProducer(string recordId) {
public static string ResolveChannel(string recordId) {
public static int ResolveMinDay(string recordId) {
public static bool IsActivated(string recordId) {
public static List<AbyssalRecordMetadata> GetAllMetadata() {
public static List<AbyssalRecordMetadata> GetActiveRecordsForProducer(string producerId) {
```


# Appendix Q.568 — Additional Current Architecture Evidence: `src/Host/HoldfastRuntimeSession.cs`

### `src/Host/HoldfastRuntimeSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 763 lines / 32845 bytes.
- SHA-256: `f9755a5dc96e2dd96c5968d5c6fe88c42582ef88dfba04531e406a15f6dde991`.
- Architecture signals: seeded references=0; save/restore symbols=7; typed event declarations=5; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=1; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class HoldfastRuntimeSession
public const long DefaultStartingValue = 100;
public const int MaxHealth = 100;
public const int MaxHunger = 100;
public const int MaxThirst = 100;
public const float RadDamageThreshold = 50f; // mSv/day causes HP loss
public const float StarvationThreshold = 90f; // hunger above this causes HP loss
public const float DehydrationThreshold = 90f; // thirst above this causes HP loss
public CoreDemoSession World { get; }
public HoldfastTradeSession Trade { get; }
public HoldfastCatalog Catalog => World.Catalog;
public string LastPersistenceMessage { get; private set; } = string.Empty;
public bool HasPurchasedThisSession { get; set; }
public string PlayerSurvivorId { get; set; } = "survivor_dr_sarah_chen";
public Ashfall.Core.Inventory.Inventory? Inventory { get; set; }
public Ashfall.Core.Inventory.Inventory? EffectiveInventory =>
public int Health => Survivors?.Find(PlayerSurvivorId) != null
public float Radiation => Survivors != null
public int Hunger => Survivors?.Find(PlayerSurvivorId) != null
public int Thirst => Survivors?.Find(PlayerSurvivorId) != null
public int Day => World.Clock.Day;
public bool IsDead => Health <= 0;
public string DeathCause { get; private set; } = string.Empty;
public bool IsGameWon => World.Quests != null && World.Quests.IsCompleted(HoldfastQuestSystem.Hatch);
public string WinMessage { get; private set; } = string.Empty;
public event Action StateChanged;
public event Action<string> OnPlayerDied; // passes cause of death
public event Action<string> OnGameWon; // passes win message
public static HoldfastRuntimeSession Create( CoreDemoSession world, bool seedDevelopmentState = false, bool loadTradeSave = true, Ashfall.Core.Inventory.Inventory? inventory = null) {
public bool TrySaveToLegacyFiles(string basePathOverride = null!, string tradePathOverride = null!) {
public bool TryReloadFromLegacyFiles(string basePathOverride = null!, string tradePathOverride = null!) {
public void SeedDevelopmentState() {
public string TickDay() {
public bool ConsumeFood(string itemId, int amount = 1) {
public ActionResult ConsumeFoodResult(string itemId, int amount = 1, string? survivorId = null) {
public Action<string, int, string>? FoodConsumed { get; set; }
public bool ConsumeWater(string itemId, int amount = 1) {
public ActionResult ConsumeWaterResult(string itemId, int amount = 1, string? survivorId = null) {
public void ExposeRadiation(float msv) {
public bool UseAntiRad(string itemId, float reduction = 0f) {
public ActionResult UseAntiRadResult(string itemId, string? survivorId = null, float reduction = 0f) {
public ActionResult FeedAllCrewResult(string? itemId = null) {
public string? FindAvailableFoodItemId() {
public string? FindAvailableWaterItemId() {
public string? FindAvailableAntiRadItemId() {
public void Heal(int amount) {
public string VisitLocation(string locationId) {
public string GetQuestSummary() {
public bool ArchiveAndFreshStart(string basePathOverride = null!, string tradePathOverride = null!) {
```


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs`

### `Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 491 lines / 19106 bytes.
- SHA-256: `46dd65cec6e6bc9db45d84638104600fa280f25692799fde8d1031398e9c0db3`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum ArchiveEntryType
public enum ArchiveSignificance
public class ArchiveEntry
public string EntryId { get; set; } = string.Empty;
public int Day { get; set; } = 1;
public ArchiveEntryType Type { get; set; } = ArchiveEntryType.Event;
public ArchiveSignificance Significance { get; set; } = ArchiveSignificance.Notable;
public string Title { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public List<string> Tags { get; set; } = new List<string>();
public List<string> ParticipantIds { get; set; } = new List<string>();
public sealed class ArchiveCategoryDef
public string CategoryId { get; set; } = string.Empty;
public string CategoryName { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public List<string> EntryTypes { get; set; } = new List<string>();
public int DisplayOrder { get; set; } = 1;
public class ShelterArchiveState
public int SchemaVersion { get; set; } = 1;
public int FoundingDay { get; set; } = 1;
public int NextSequence { get; set; } = 1;
public List<ArchiveEntry> Entries { get; set; } = new List<ArchiveEntry>();
public List<ArchiveCategoryDef> AuthoredCategories { get; set; } = new List<ArchiveCategoryDef>();
public sealed class ShelterArchiveSystem
public event Action<ArchiveEntry>? OnEntryRecorded;
public Action<ArchiveEntry>? OnEntryRecordedSeam { get; set; }
public Action<ArchiveEntry>? OnMemorialRecordedSeam { get; set; }
public int FoundingDay => _state.FoundingDay;
public int EntryCount => _state.Entries.Count;
public IReadOnlyList<ArchiveCategoryDef> AuthoredCategories => _state.AuthoredCategories;
public void LoadCatalog(string json) {
public static IReadOnlyList<ArchiveEntry> ProjectCanonicalSources( JournalSystem? journal, MemorialSystem? memorial) {
public ArchiveEntry RecordEntry(ArchiveEntry entry) {
public ArchiveEntry RecordEvent( int day, string title, string description, ArchiveEntryType type = ArchiveEntryType.Event, ArchiveSignificance significance = ArchiveSignificance.Notable,
public ArchiveEntry RecordMemorialLoss(MemorialEntry memorial, string dwellerName = "") {
public void AttachMemorialSystem(MemorialSystem memorialSystem, Func<string, string>? nameLookup = null) {
public IReadOnlyList<ArchiveEntry> GetTimeline( ArchiveEntryType? typeFilter = null, ArchiveSignificance? minSignificance = null) {
public IReadOnlyList<ArchiveEntry> Search( string? keyword = null, string? tag = null, string? participantId = null, int? startDay = null, int? endDay = null)
public ShelterArchiveState CaptureState() {
public void RestoreState(ShelterArchiveState state) {
public ShelterArchiveCensus GetCensus() {
public struct ShelterArchiveCensus
public int EntryCount { get; }
public int CategoryCount { get; }
public int FoundingDay { get; }
public int MilestoneCount { get; }
public int MemorialCount { get; }
public int DecisionCount { get; }
public int DiscoveryCount { get; }
public int EventCount { get; }
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/GrainMillingProjection.cs`

### `Assets/Ashfall.Core/Narrative/GrainMillingProjection.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 585 lines / 34482 bytes.
- SHA-256: `6a78cb265f45d85d24a09f637fb6c53bceb954ffd69372554623ca2f11e2f8e9`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum GrainMillingRecordFamily
public enum GrainMillingProvenanceClass
public enum GrainMillingFacilityType
public sealed class GrainMillingRecordMetadata
public string RecordId { get; set; } = string.Empty;
public GrainMillingRecordFamily Family { get; set; }
public GrainMillingProvenanceClass Provenance { get; set; }
public GrainMillingFacilityType FacilityType { get; set; }
public string FacilityId { get; set; } = string.Empty;
public string NamedSubject { get; set; } = string.Empty;
public string CropOrMaterial { get; set; } = string.Empty;
public string ProducerId { get; set; } = string.Empty;
public string Channel { get; set; } = "mill_inspection";
public int MinDay { get; set; } = 1;
public bool IsActivated { get; set; }
public string MeasurementSummary { get; set; } = string.Empty;
public List<string> RelatedRecordIds { get; set; } = new List<string>();
public static class GrainMillingProjection
public static GrainMillingRecordMetadata? GetMetadata(string recordId) {
public static IEnumerable<GrainMillingRecordMetadata> GetAllMetadata() => Records.Values;
public static GrainMillingRecordFamily? GetFamily(string recordId) {
public static List<GrainMillingRecordMetadata> GetByProducer(string producerId) {
public static List<string> GetRelated(string recordId) {
```


# Appendix Q.571 — Additional Current Architecture Evidence: `src/UI/InventoryDetailPanel.cs`

### `src/UI/InventoryDetailPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 288 lines / 13820 bytes.
- SHA-256: `3442795d2c64a19bc1358f43a62108a6ff34fc7cfc660e979bb579f722fc2db5`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=7; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class InventoryDetailPanel : Control
public event Action? OnClose;
public event Action<string>? OnConsume;
public event Action<string>? OnEquip;
public bool IsBound => _inventory != null && !string.IsNullOrEmpty(_itemId);
public int RenderedRowCount { get; private set; }
public ItemInspectionModel? CurrentInspection { get; private set; }
public void Bind(InventoryHostSession? inventory, string itemId, ItemDescriptionCatalog? descriptions = null, ExpansionEnrichmentCatalog? enrichment = null) => Bind(inventory, itemId, descriptions, enrichment, technicalProvenance: null);
public void Bind(InventoryHostSession? inventory, string itemId, ItemDescriptionCatalog? descriptions, ExpansionEnrichmentCatalog? enrichment, System.Collections.Generic.IReadOnlyList<Ashfall.Core.Narrative.TechnicalMaterialRecord>? technicalProvenance) => Bind(inventory, itemId, descriptions, enrichment, technicalProvenance, leatherProvenance: null);
public void Bind(InventoryHostSession? inventory, string itemId, ItemDescriptionCatalog? descriptions, ExpansionEnrichmentCatalog? enrichment, System.Collections.Generic.IReadOnlyList<Ashfall.Core.Narrative.TechnicalMaterialRecord>? technicalProvenance, System.Collections.Generic.IReadOnlyList<Ashfall.Core.Narrative.LeatherworkRecord>? leatherProvenance) {
public override void _Ready() {
public void RefreshView() {
public void Open() {
public override void _GuiInput(InputEvent @event) {
public override void _UnhandledInput(InputEvent @event) {
```


# Appendix Q.572 — Additional Current Architecture Evidence: `src/Host/HostCli.AdvancedIndustrialRecon.cs`

### `src/Host/HostCli.AdvancedIndustrialRecon.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 417 lines / 27411 bytes.
- SHA-256: `edf24dbb38c174cd7815bb216c75f86822b6556d77c3373365a9542b397966aa`.
- Architecture signals: seeded references=16; save/restore symbols=22; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunSyntheticLubricantSelfTest(string dataDirectory) {
public static int RunUvCoronaSelfTest(string dataDirectory) {
public static int RunCarbonCompositeSelfTest(string dataDirectory) {
public static int RunGprCartographySelfTest(string dataDirectory) {
public static int RunAdvancedIndustrialReconSelfTest(string dataDirectory) {
public string id = string.Empty;
public bool passed;
public string evidence = string.Empty;
public int schema_version;
public int seed;
public int days;
public bool same_seed_byte_equal;
public bool different_seed_diverged;
public bool midpoint_save_load_equal;
public List<AdvancedIndustrialDaySnapshot> snapshots = new List<AdvancedIndustrialDaySnapshot>();
public List<AdvancedCheck> checks = new List<AdvancedCheck>();
public int day;
public FischerTropschSynthesisState synthesis = new FischerTropschSynthesisState();
public CarbonCompositeState composites = new CarbonCompositeState();
public UvCoronaDetectionState uv = new UvCoronaDetectionState();
public GroundPenetratingRadarState gpr = new GroundPenetratingRadarState();
public InventorySaveState inventory = new InventorySaveState();
public int day;
public float catalyst_condition;
public bool synthesis_active;
public int synthesis_buffer;
public bool composite_active;
public int composite_buffer;
public float autoclave_condition;
public int uv_observations;
public int gpr_observations;
public int gpr_leads;
public int fuel_stock;
public int prepreg_stock;
public int battery_stock;
public int battery_pack_stock;
public float latest_uv_confidence;
public float latest_gpr_confidence;
public float latest_gpr_depth_band;
public string latest_gpr_anomaly_class = string.Empty;
public int Seed { get; }
public bool CatalogsLoaded => _fischer != null && _composites != null && _uv != null && _gpr != null;
public List<AdvancedIndustrialDaySnapshot> Snapshots { get; } = new List<AdvancedIndustrialDaySnapshot>();
public int ProductionCompletions { get; private set; }
public int CompositeCompletions { get; private set; }
public int UvObservations => _uvEngine.State.observations.Count;
public int GprObservations => _gprEngine.State.observations.Count;
public static AdvancedIndustrialReconRun Create(string dataDirectory, int seed) => new AdvancedIndustrialReconRun(dataDirectory, seed);
public void AdvanceTo(int day) {
public void PrepareMidpointSave() {
public AdvancedIndustrialSave CaptureSave() => new AdvancedIndustrialSave
public void RestoreSave(AdvancedIndustrialSave save) {
public bool AllBoundsSane() {
```


# Appendix Q.573 — Additional Current Architecture Evidence: `src/Host/ShelterDecorHostSession.cs`

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


# Appendix Q.574 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs`

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


# Appendix Q.575 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs`

### `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 864 lines / 36461 bytes.
- SHA-256: `6493ee6924ace913e975210a1a66500670279fd2ffc0d3099902373b0b4fd982`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class VehicleCustomizationRecord
public string vehicleId = string.Empty;
public Dictionary<string, string> installedSlots = new Dictionary<string, string>(StringComparer.Ordinal);
public int chassisStressPermille;
public int engineFoulingPermille;
public int transmissionWearPermille;
public bool isImmobilized;
public string immobilizedReason = string.Empty;
public string armorGradeId = string.Empty;
public int armorIntegrityPermille;
public int armorIntegrityMaxPermille;
public string armorMaterialProfileId = string.Empty;
public string armorPurity = FoundryPurityNames.Standard;
public sealed class VehicleRecoveryMission
public string missionId = string.Empty;
public string strandedVehicleId = string.Empty;
public string locationId = string.Empty;
public int requiredFuelUnits = 10;
public int progressTicks;
public int requiredTicks = 120;
public bool isComplete;
public sealed class VehicleGarageState
public string systemId = VehicleGarageSystem.SystemId;
public Dictionary<string, VehicleCustomizationRecord> vehicleRecords = new Dictionary<string, VehicleCustomizationRecord>(StringComparer.Ordinal);
public Dictionary<string, VehicleRecoveryMission> activeRecoveries = new Dictionary<string, VehicleRecoveryMission>(StringComparer.Ordinal);
public int nextRecoveryCounter = 1;
public sealed class VehicleGarageSystem
public const string SystemId = "vehicle_garage";
public const string SlotCargo = "cargo";
public const string SlotProtection = "protection";
public const string SlotMobility = "mobility";
public const string SlotEngine = "engine";
public const string SlotUtility = "utility";
public delegate bool TryGetArmorMaterialQuality(out FoundryMaterialQuality quality);
public TryGetArmorMaterialQuality? ArmorMaterialQualitySource { get; set; }
public Func<string, string?>? VehicleTerrainResolver { get; set; }
public void LoadCatalog(VehicleGarageCatalog catalog) {
public bool HasModification(string modId) => _modCatalog.ContainsKey(modId);
public VehicleModificationDefinition? GetModification(string modId) {
public IReadOnlyDictionary<string, VehicleModificationDefinition> GetAllModifications() => _modCatalog;
public void LoadArmorCatalog(VehicleArmorGradeCatalog catalog) {
public bool HasArmorGrade(string gradeId) => !string.IsNullOrEmpty(gradeId) && _armorCatalog.ContainsKey(gradeId);
public VehicleArmorGradeDefinition? GetArmorGrade(string gradeId) {
public IReadOnlyDictionary<string, VehicleArmorGradeDefinition> GetAllArmorGrades() => _armorCatalog;
public VehicleArmorProfile GetArmorProfile(string vehicleId) {
public static string ArmorConditionBand(int integrityPermille, int maxPermille) {
public VehicleCustomizationRecord? GetRecord(string vehicleId) =>
public bool IsImmobilized(string vehicleId) => GetRecord(vehicleId)?.isImmobilized == true;
public IReadOnlyDictionary<string, VehicleRecoveryMission> ActiveRecoveries => _state.activeRecoveries;
public IReadOnlyDictionary<string, string> GetInstalledSlots(string vehicleId) =>
public void DecorateProfile(ExpeditionVehicleProfile? profile) {
public int AdvanceRecoveries(int deltaTicks) {
public VehicleCustomizationRecord GetOrCreateRecord(string vehicleId) {
public bool HasVehicleRecord(string vehicleId) =>
public bool CanInstallArmorGrade(string vehicleId, string gradeId, IPlayerInventoryPort? inventory, out string reason) {
public bool InstallArmorGrade(string vehicleId, string gradeId, IPlayerInventoryPort? inventory, out string reason) {
public bool ReforgeArmorPlate(string vehicleId, IPlayerInventoryPort? inventory, out string reason) {
public bool CanInstallModification(string vehicleId, string slotType, string modId, IPlayerInventoryPort? inventory, out string reason) {
public bool InstallModification(string vehicleId, string slotType, string modId, IPlayerInventoryPort? inventory, out string reason) {
public bool UninstallModification(string vehicleId, string slotType, IPlayerInventoryPort? inventory, out string reason) {
public float GetEffectiveCargoCapacityDelta(string vehicleId) {
public float GetEffectiveSpeedMultiplierDelta(string vehicleId) {
public float GetEffectiveFuelConsumptionMultiplier(string vehicleId) {
public float GetEffectiveWearRateMultiplier(string vehicleId) {
public int GetEffectiveRadiationProtectionPermille(string vehicleId) {
public void RecordTripWear(string vehicleId, float distanceKm, float roadRoughnessMultiplier = 1f) {
public bool ServiceChassis(string vehicleId, IPlayerInventoryPort? inventory, int repairPermille, out string reason) {
public bool ServiceEngine(string vehicleId, IPlayerInventoryPort? inventory, int repairPermille, out string reason) {
public bool ServiceTransmission(string vehicleId, IPlayerInventoryPort? inventory, int repairPermille, out string reason) {
public bool RegisterRecoveryMission(string vehicleId, string locationId, int requiredFuelUnits, out string missionId, out string reason) {
public bool AdvanceRecoveryMission(string missionId, int deltaTicks, out bool completed) {
public bool CompleteRecoveryMission(string missionId, IPlayerInventoryPort? inventory, out string reason) {
public VehicleGarageState CaptureState() {
public void RestoreState(VehicleGarageState? state) {
```


# Appendix Q.576 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Phantoms/HeirloomCatalog.cs`

### `Assets/Ashfall.Core/Phantoms/HeirloomCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 107 lines / 3688 bytes.
- SHA-256: `fc7916235a24f30a3946ccc3f6bd8bb41b4ba20b657e719a0a69c7399fa2f4a5`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class HeirloomHistoricalStage
public int stage_index;
public string period_label;
public string original_holder;
public string historical_fragment;
public sealed class HeirloomHolderMemory
public string affinity_key; // "kin", "profession_medical", "trait_caregiver", "generic", etc.
public string memory_text;
public float morale_effect;
public float guilt_effect;
public sealed class HeirloomDefinition
public string heirloom_id;
public string base_item_id;
public string title;
public string origin;
public bool is_legacy_candidate;
public bool memorial_eligible;
public List<HeirloomHistoricalStage> stages = new List<HeirloomHistoricalStage>();
public List<HeirloomHolderMemory> holder_memories = new List<HeirloomHolderMemory>();
public sealed class HeirloomCatalogJson
public int schema_version;
public List<HeirloomDefinition> items = new List<HeirloomDefinition>();
public sealed class HeirloomCatalog
public IReadOnlyCollection<HeirloomDefinition> AllHeirlooms => _byHeirloomId.Values;
public void Load(string json, IJsonSerializer serializer) {
public HeirloomDefinition? GetById(string heirloomId) {
public HeirloomDefinition? GetByBaseItemId(string baseItemId) {
public bool Contains(string heirloomId) {
```


# Appendix Q.577 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/TravelingCaravanSystem.cs`

### `Assets/Ashfall.Core/TravelingCaravanSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 518 lines / 22717 bytes.
- SHA-256: `9c7a92b23c61fab7d3f7b434eb3a9d78773ce6c37d2e5de21cd3d1723ccbc349`.
- Architecture signals: seeded references=2; save/restore symbols=3; typed event declarations=11; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class CaravanInventoryItem
public string itemId;
public int quantity;
public int priceRations;
public class CaravanEntry
public string caravanId;
public string caravanName;
public string factionId;
public string originRegion; // flotilla, foundry, greenhouse, settlement, traplines
public string currentNodeId;
public List<string> routeNodeIds = new List<string>();
public int routeIndex = 0;
public int daysAtCurrentNode = 0;
public int stayDurationDays = 2;
public int guardCount = 4;
public bool isRobbed = false;
public bool embargoBlocked = false;
public List<CaravanInventoryItem> inventory = new List<CaravanInventoryItem>();
public class TravelingCaravanState
public List<CaravanEntry> activeCaravans = new List<CaravanEntry>();
public int completedTradesCount = 0;
public class TravelingCaravanSystem
public const string SystemId = "traveling_caravan_system";
public GoodsCatalog? Catalog { get; set; }
public Narrative.TravelEncounterSystem? TravelEncounters { get; set; }
public TradeEmbargoSystem? Embargoes { get; set; }
public WastelandMapSystem? Map { get; set; }
public event Action<CaravanEntry, string>? OnCaravanArrivedAtNode;
public event Action<CaravanEntry, string, int>? OnTradeCompleted;
public event Action<CaravanEntry, Narrative.TravelEncounterDefinition>? OnCaravanPatrolEncountered;
public event Action<CaravanEntry, string>? OnCaravanEmbargoed;
public event Action<CaravanEntry>? OnCaravanResumed;
public TravelingCaravanState State => _state;
public int CaravanCount => _state.activeCaravans?.Count ?? 0;
public void SpawnCaravan(string caravanId, string name, string factionId, List<string> route, string originRegion = "settlement") {
public CaravanEntry? GetCaravanAtNode(string nodeId) {
public void DailyTick() => DailyTick(0, null);
public Func<WeatherKind, float>? WeatherAvailabilityProvider { get; set; }
public void DailyTick( int currentDay, ISeededRng? rng = null, string defaultRegion = "the_toll", int dangerLevel = 2, string season = "all",
public Narrative.TravelEncounterDefinition? CheckRouteEncounter( CaravanEntry caravan, string region, int dangerLevel, string season, int currentDay,
public bool ResolveRouteEncounterChoice( string encounterId, string choiceId, int currentDay, out Narrative.TravelEncounterResolutionResult? result) {
public bool TryBuyItem(string caravanId, string itemId, int amount, ref int playerRations) {
public TravelingCaravanState CaptureState() {
public void RestoreState(TravelingCaravanState state) {
```


# Appendix Q.578 — Additional Current Architecture Evidence: `src/Main.Campaign.cs`

### `src/Main.Campaign.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 492 lines / 23212 bytes.
- SHA-256: `d4750e2a3d795b60ee1ff6127291243f468dd5bbcc6b6d4103251db033da8b59`.
- Architecture signals: seeded references=0; save/restore symbols=7; typed event declarations=0; textual Godot mentions=9; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.579 — Additional Current Architecture Evidence: `src/Main.UiTests.Inventory.cs`

### `src/Main.UiTests.Inventory.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 113 lines / 5748 bytes.
- SHA-256: `11df7b77455a7af12edd0aa06c3beea944630dd533e3888c87b50e91e3b82f27`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.580 — Additional Current Architecture Evidence: `src/Host/GreenhouseHostSession.cs`

### `src/Host/GreenhouseHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 450 lines / 19290 bytes.
- SHA-256: `dfb777160557cbc0d0475e4c0bca79b326e6bb35039fa2a78f1ce993f2829a45`.
- Architecture signals: seeded references=1; save/restore symbols=5; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class GreenhouseHostSession
public GreenhouseSystem System { get; }
public ApicultureSystem Apiculture { get; }
public InventoryHostSession? InventoryHost { get; set; }
public string LastEvent { get; private set; } = string.Empty;
public Func<string>? SeasonWindowProvider { get; set; }
public string CurrentSeasonLabel => SeasonWindowProvider?.Invoke() ?? "Standard";
public static GreenhouseHostSession Create(InventoryHostSession? inventoryHost = null) {
public bool Plant(int plotIndex, string seedItemId, int currentDay) {
public bool Water(int plotIndex, float waterUnits, bool tainted) {
public CommandResult PreviewTreatBlight(int plotIndex) {
public CommandResult ExecuteTreatBlight(int plotIndex) {
public bool TreatBlight(int plotIndex) {
public bool Harvest(int plotIndex) {
public bool Clear(int plotIndex) {
public bool ApplyNutrients(int plotIndex) {
public bool InstallHive(string hiveId, string bayId, int currentDay) {
public bool InspectHive(string hiveId, int currentDay) {
public bool FeedHive(string hiveId, float amount = 0.5f) {
public bool HarvestHoney(string hiveId) {
public void TickDay(int currentDay, float growLightHours = 6f, float ashContaminationRate = 0.05f) {
public GreenhouseState CaptureSave() {
public static class GreenhouseSaveStore
public const string FileName = "greenhouse_save.json";
public const string SectionName = "greenhouse";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static bool TrySave(GreenhouseState state) => s_store.TrySave(state);
public static GreenhouseState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(GreenhouseState state) => s_store.CapturePersisted(state);
public sealed class GreenhouseSaveEnvelope
public GreenhouseState? State { get; set; }
public string? Checksum { get; set; }
```


# Appendix Q.581 — Additional Current Architecture Evidence: `src/Main.PlayerSurfaces.cs`

### `src/Main.PlayerSurfaces.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1141 lines / 66230 bytes.
- SHA-256: `359ab8fa9162114b544f2a9629d5ea490a6cf866323880035b63ab47ab8c9056`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.582 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Inventory/InventoryProvenance.cs`

### `Assets/Ashfall.Core/Inventory/InventoryProvenance.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 43 lines / 1252 bytes.
- SHA-256: `0e69f8555e8732b59a3c777310df9696867a16cedec8ef705de4d620a6b58016`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum InventoryMutationSource
public readonly struct InventoryProvenanceRecord
public string ItemId { get; }
public int Delta { get; }
public InventoryMutationSource Source { get; }
public int Day { get; }
public string Context { get; }
public override string ToString() => $"[Day {Day} | {Source}] {ItemId} {(Delta >= 0 ? "+" : "")}{Delta} ({Context})";
```


# Appendix Q.583 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Economy/BlackMarketSettlementService.cs`

### `Assets/Ashfall.Core/Economy/BlackMarketSettlementService.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 372 lines / 19874 bytes.
- SHA-256: `e8914870d671c1b6a02b3ff9abdc2140ef8861e044d437ed5a187262749cf2fe`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public readonly struct BlackMarketActionPreview
public readonly bool IsAvailable;
public readonly string ActionId;
public readonly string SyndicateId;
public readonly string EntryId;
public readonly string DebtId;
public readonly string ItemId;
public readonly int Quantity;
public readonly long SettlementUnits;
public readonly int DueDay;
public readonly string ReasonId;
public readonly string Message;
public readonly struct BlackMarketActionResult
public readonly bool Success;
public readonly string ActionId;
public readonly string SyndicateId;
public readonly string EntryId;
public readonly string DebtId;
public readonly string ItemId;
public readonly int Quantity;
public readonly long WalletDelta;
public readonly int InventoryDelta;
public readonly long SettlementUnits;
public readonly int DueDay;
public readonly string ReasonId;
public readonly string Message;
public sealed class BlackMarketSettlementService
public const string BuyAction = "buy";
public const string SellAction = "sell";
public const string LoanAction = "take_loan";
public const string RepayAction = "repay";
public long WalletValue => _wallet.Value;
public int InventoryCount(string itemId) =>
public BlackMarketActionPreview PreviewBuy(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketActionResult Buy(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketActionPreview PreviewSell(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketActionResult Sell(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketActionPreview PreviewLoan(string syndicateId, long units, int day, int durationDays) {
public BlackMarketActionResult TakeLoan(string syndicateId, long units, int day, int durationDays) {
public BlackMarketActionPreview PreviewRepay(string debtId, long units, int day) {
public BlackMarketActionResult Repay(string debtId, long units, int day) {
public static string ReasonText(string reasonId) {
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.
