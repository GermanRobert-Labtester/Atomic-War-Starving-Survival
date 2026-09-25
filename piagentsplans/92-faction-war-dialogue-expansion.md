# Plan 92 — Faction-War Dialogue Corpus, Location/Day Gates and Living-World Projection

> **Rebuild status:** COMPLETE 40-SNIPPET CONTENT CATALOG — PLAYER-REACHABILITY AND PRESENTATION AUDIT
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

- The current `faction_war_dialogue.json` has 40 rows with canonical-looking IDs, location IDs, minDay values, speaker tags and dialogue bodies across the authored war context.
- The live data path is catalog loader → `FactionWarContentCatalog.DialogueSnippets`/`GetDialogueForLocation` → Year-of-Ash host/catalog binding. The current source proves query capability, not a dedicated player-facing dialogue panel.
- The plan preserves natural overheard texture, location/day legality and stable ordering while identifying the smallest truthful host/presentation seam if a reachability gap is confirmed.

**Bounded outcome:** Retire the old 18→40 pure-data brief. The current catalog has 40 snippets, `FactionWarContentCatalogLoader` loads the faction-war family and exposes `GetDialogueForLocation`, and the Year-of-Ash host binds the catalog. The remaining plan is a route/selection audit for whether snippets are actually surfaced to players, plus reference and tone checks—not another dialogue catalog or a new conversation system.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `faction_war_dialogue.json` is valid JSON with 40 `snippets`; rows include `id`, `locationId`, `minDay`, `speakerTag` and `body`.
- `FactionWarContentCatalogLoader` loads the six faction-war content families, and `GetDialogueForLocation(locationId, day)` returns rows whose day gate is satisfied.
- `YearOfAshHostSession.Create` loads the faction-war catalog and binds it to the current runner/session; `FactionWarMapWidget` and related panels are possible presentation context, not assumed dialogue owners.
- Historical DEC-261 tests cover the 40-row corpus and location/day query; this plan does not assert a fresh runtime pass or a currently open dialogue panel.

**Master-authority sections applied to this rebase:**

- Part II Factory Protocol: premise sweep, collision check, one lane/cluster, and evidence labels before drafting.
- Part II Step 5 continuity and anti-duplication checklist: data presence is not reachability.
- Part III cluster map: use the live C1–C17 owner map rather than a historical plan title.
- Part IV backlog discipline: consume a verified candidate or record why it is stale; do not widen a bounded outcome.
- Part V Template S/R: subject intent and recommended route remain separate from implementation commitments.
- Part VI Multi-Session Growth Protocol: 250k is a depth target, not permission to manufacture volume.
- Live source/data authority: current catalog, loader, host, save, and focused tests outrank generated prose.
- Anti-padding rule: if the evidence queue is exhausted, stop and report no warranted continuation.
- C7/C8 cluster: faction-war dialogue is content projection; radio and journal remain separate information owners.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 18→40 target with a 40-row current corpus census and a location/day/faction-context matrix.
- Verify every location reference against current location/expedition/Verdict route families before calling a row reachable.
- Audit the current host/UI route for an overheard-dialogue surface and record a bounded integration proposal if absent.
- Preserve authored dialogue as ambient prose; do not turn it into hidden quest flags or a second faction-war simulation.

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
| faction-war content file loading and family assembly | FactionWarContentCatalogLoader | `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs` | Owns catalog parsing; it does not decide player reachability. |
| dialogue row storage and location/day query | FactionWarContentCatalog | `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs` | Owns static content and deterministic filtering. |
| war state and current event/runner context | FactionWarChainRunner/System | `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs; Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` | Owns war simulation; dialogue cannot alter standing or territory. |
| catalog composition and current day/host state | YearOfAshHostSession | `src/YearOfAsh/YearOfAshHostSession.cs` | Binds the catalog; it must not duplicate dialogue selection. |
| map/briefing/radio surfaces | Faction war presentation | `src/Main.YearOfAsh.cs; src/YearOfAsh/FactionWarMapWidget.cs; src/UI/RadioIntelligencePanel.cs` | Read-only candidate projections; current source does not prove a dedicated dialogue panel. |
| row, gate and integration proof | Dialogue focused tests | `Ashfall.Core.Tests/FactionWarDialogueExpansionTests.cs; Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs` | Focused evidence surface. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Faction-War Dialogue Corpus, Location/Day Gates and Living-World Projection
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ FactionWarContentCatalogLoader
│   faction-war content file loading and family assembly
│ FactionWarContentCatalog
│   dialogue row storage and location/day query
│ FactionWarChainRunner/System
│   war state and current event/runner context
│ YearOfAshHostSession
│   catalog composition and current day/host state
│ Faction war presentation
│   map/briefing/radio surfaces
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

1. **Preserve current state ownership.** FactionWarContentCatalogLoader owns faction-war content file loading and family assembly: Owns catalog parsing; it does not decide player reachability.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| faction-war content file loading and family assembly | FactionWarContentCatalogLoader | `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs` | Owns catalog parsing; it does not decide player reachability. |
| dialogue row storage and location/day query | FactionWarContentCatalog | `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs` | Owns static content and deterministic filtering. |
| war state and current event/runner context | FactionWarChainRunner/System | `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs; Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` | Owns war simulation; dialogue cannot alter standing or territory. |
| catalog composition and current day/host state | YearOfAshHostSession | `src/YearOfAsh/YearOfAshHostSession.cs` | Binds the catalog; it must not duplicate dialogue selection. |
| map/briefing/radio surfaces | Faction war presentation | `src/Main.YearOfAsh.cs; src/YearOfAsh/FactionWarMapWidget.cs; src/UI/RadioIntelligencePanel.cs` | Read-only candidate projections; current source does not prove a dedicated dialogue panel. |
| row, gate and integration proof | Dialogue focused tests | `Ashfall.Core.Tests/FactionWarDialogueExpansionTests.cs; Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs` | Focused evidence surface. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load the 40-row corpus through the current loader
2. bind the catalog to Year-of-Ash host state
3. read current player location and campaign day
4. query eligible snippets by canonical location and minDay
5. select/present an ambient line without changing war state
6. optionally route a stable knowledge/journal fact only if an existing owner accepts it
7. leave faction standing/territory untouched

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Snippet rows are immutable catalog data; selection is a pure query over location and current day.
- `minDay` is an availability lower bound, not a hidden outcome or a persisted heard-once flag.
- The catalog preserves family order and returns only rows matching the current location/day contract.
- A missing/empty location result is an honest ambient absence, not a reason to invent dialogue.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Every row has a unique ID, non-empty speaker/body, finite day gate and a resolvable current location.
- A location with no eligible row returns an empty result; it does not fall back to a row from another location.
- The same location/day query returns the same ordered rows regardless of host/frame timing.
- Dialogue cannot grant standing, items, quests or flags without an explicit existing owner command.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `faction_war_dialogue.json` is the sole dialogue corpus.
- Do not duplicate snippets into a radio catalog, journal database or faction panel.
- A new row needs a current location route, day gate, speaker context and a real presentation consumer.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- No save section is required for an ambient query. If a future heard-once memory is desired, claim the current journal/knowledge owner first and add a round-trip test.
- Existing Year-of-Ash save state must not be polluted by presentation-only selection.
- A failed content load should not corrupt or reset war state.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Filtering is deterministic list order plus explicit minDay comparison.
- No random selection is required for a location/day projection; a future rotation would need a named seeded stream.
- Catalog order and host query order must be stable across paired runs.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Faction war state emits current war facts; a future presentation route may observe them without mutating them.
- Catalog selection is not an event and does not emit a gameplay consequence.
- If a future “heard” fact is introduced, it needs a named save/idempotency owner; this plan does not create one.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/YearOfAsh/YearOfAshHostSession.cs
- src/Main.YearOfAsh.cs
- src/YearOfAsh/FactionWarMapWidget.cs
- src/UI/RadioIntelligencePanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Overheard dialogue should remain naturalistic, specific and grounded in logistics, weather and interpersonal friction.
- It should not dump plot exposition, reveal hidden deterministic outcomes or speak as a tutorial.
- Use fictional locations/factions and avoid copied real-world political dialogue.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A row references a location family that no current route exposes. | FactionWarContentCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A panel displays a snippet from the wrong day/location. | FactionWarContentCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A line changes faction standing or flags without an owner command. | FactionWarChainRunner/System | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Catalog iteration order differs between hosts. | YearOfAshHostSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new dialogue save store duplicates journal/knowledge state. | Faction war presentation | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/FactionWarDialogueExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — corpus census | Read 40 rows, loader, query and Year-of-Ash host. | Current row grammar and owner are known. | No production path until the owning implementation package is separately claimed. |
| 1 — reference/reachability sweep | Resolve each location against current route families. | Orphan/unreachable rows are explicit. | No production path until the owning implementation package is separately claimed. |
| 2 — presentation decision | Verify current panel/radio/map route or scope a read-only projection. | No new dialogue manager is assumed. | No production path until the owning implementation package is separately claimed. |
| 3 — tone/precision pass | Review day gates, naturalness, stable order and no hidden effects. | Quality and honesty are both preserved. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/faction_war_dialogue.json | READ ONLY; MODIFY only for a proven reference/content gap | 40-row authority |
| Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs | READ ONLY | Loader/query owner |
| src/YearOfAsh/YearOfAshHostSession.cs | READ ONLY | Host binding |
| src/YearOfAsh/FactionWarMapWidget.cs | READ ONLY; MODIFY only under a new UI claim | Candidate presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Claiming a player route from a loader-only reference. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Turning ambient prose into hidden mechanics. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Inventing a second heard-once save. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Using day/location filters that disagree across hosts. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new dialogue rows in this package.
- No new conversation/quest system.
- No new save section.
- No production/data/test/UI changes.

# 23. Rollback and Recovery

- Revert the planning document.
- Future host/data changes retain the prior valid JSON and focused location/day tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 40 current rows and their location/day grammar are documented.
- Loader/query/host/presentation reachability is explicitly separated.
- No parallel dialogue authority is proposed.
- Focused commands and residual decision gates are named.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 18→40 target with a 40-row current corpus census and a location/day/faction-context matrix.
- Verify every location reference against current location/expedition/Verdict route families before calling a row reachable.
- Audit the current host/UI route for an overheard-dialogue surface and record a bounded integration proposal if absent.
- Preserve authored dialogue as ambient prose; do not turn it into hidden quest flags or a second faction-war simulation.

## MUST NOT DO

- No new dialogue rows in this package.
- No new conversation/quest system.
- No new save section.
- No production/data/test/UI changes.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/FactionWarDialogueExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — corpus census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: faction-war content file loading and family assembly → FactionWarContentCatalogLoader; dialogue row storage and location/day query → FactionWarContentCatalog; war state and current event/runner context → FactionWarChainRunner/System; catalog composition and current day/host state → YearOfAshHostSession; map/briefing/radio surfaces → Faction war presentation; row, gate and integration proof → Dialogue focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 92.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 92 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by FactionWarContentCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`

### `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 451 lines / 20575 bytes.
- SHA-256: `4dfa917553bf8239ff0ed5799cd97fb81954caa3f0f24d32137f4e220bc092ef`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FactionWarContentCatalog
public IReadOnlyList<FactionWarEventChain> EventChains => _eventChains;
public IReadOnlyList<FactionWarJournalEntry> JournalEntries => _journalEntries;
public IReadOnlyList<FactionWarBroadcast> Broadcasts => _broadcasts;
public IReadOnlyList<FactionWarDialogueSnippet> DialogueSnippets => _dialogueSnippets;
public IReadOnlyList<FactionWarCommunique> Communiques => _communiques;
public IReadOnlyList<FactionWarLocationOverride> LocationOverrides => _locationOverrides;
public int EventChainCount => _eventChains.Count;
public int JournalEntryCount => _journalEntries.Count;
public int BroadcastCount => _broadcasts.Count;
public int DialogueSnippetCount => _dialogueSnippets.Count;
public int CommuniqueCount => _communiques.Count;
public int LocationOverrideCount => _locationOverrides.Count;
public void AddEventChain(FactionWarEventChain chain) => _eventChains.Add(chain);
public void AddJournalEntry(FactionWarJournalEntry entry) => _journalEntries.Add(entry);
public void AddBroadcast(FactionWarBroadcast broadcast) => _broadcasts.Add(broadcast);
public void AddDialogueSnippet(FactionWarDialogueSnippet snippet) => _dialogueSnippets.Add(snippet);
public void AddCommunique(FactionWarCommunique communique) => _communiques.Add(communique);
public void AddLocationOverride(FactionWarLocationOverride entry) => _locationOverrides.Add(entry);
public List<FactionWarEventChain> GetEligibleChains(int day) {
public List<FactionWarJournalEntry> GetJournalForDay(int day) {
public List<FactionWarBroadcast> GetBroadcastsForDay(int day) {
public List<FactionWarDialogueSnippet> GetDialogueForLocation(string locationId, int day) {
public List<FactionWarCommunique> GetCommuniquesForFaction(string factionId, int day) {
public FactionWarLocationOverride? GetActiveLocationOverride(string locationId, int day) {
public sealed class FactionWarEventChain
public string chainId = string.Empty;
public string band = string.Empty;
public string title = string.Empty;
public List<string> factionsInvolved = new List<string>();
public string locationId = string.Empty;
public List<FactionWarEventStage> stages = new List<FactionWarEventStage>();
public sealed class FactionWarEventStage
public string stageId = string.Empty;
public int minDay;
public string triggerCondition = string.Empty;
public string title = string.Empty;
public string bodyText = string.Empty;
public List<FactionWarEventChoice> choices = new List<FactionWarEventChoice>();
public string requiresFlag = string.Empty;
public string producesFlag = string.Empty;
public sealed class FactionWarEventChoice
public string choiceId = string.Empty;
public string text = string.Empty;
public int moraleDelta;
public string leadsToStageId = string.Empty;
public string requiresFlag = string.Empty;
public string producesFlag = string.Empty;
public string standingFactionId = string.Empty;
public int standingDelta;
public sealed class FactionWarJournalEntry
public string id = string.Empty;
public string authorName = string.Empty;
public int day;
public string locationId = string.Empty;
public string voice = string.Empty;
public string body = string.Empty;
public sealed class FactionWarBroadcast
public string id = string.Empty;
public string frequency = string.Empty;
public int dayTrigger;
public string source = string.Empty;
public string message = string.Empty;
public string signalStrength = string.Empty;
public bool isEmergency;
public string audio_cue = string.Empty;
public sealed class FactionWarDialogueSnippet
public string id = string.Empty;
public string locationId = string.Empty;
public int minDay;
public string speakerTag = string.Empty;
public string body = string.Empty;
public sealed class FactionWarCommunique
public string id = string.Empty;
public string eventChainId = string.Empty;
public string factionId = string.Empty;
public int day;
public string title = string.Empty;
public string body = string.Empty;
public string authorNote = string.Empty;
public sealed class FactionWarLocationOverride
public string id = string.Empty;
public string locationId = string.Empty;
public string overrideType = string.Empty;
public int activeFromDay;
public int activeUntilDay;
public string displayName = string.Empty;
public string description = string.Empty;
public sealed class FactionWarEventChainRoot
public int schema_version;
public List<FactionWarEventChain> chains = new List<FactionWarEventChain>();
public sealed class FactionWarJournalRoot
public int schema_version;
public List<FactionWarJournalEntry> entries = new List<FactionWarJournalEntry>();
public sealed class FactionWarBroadcastRoot
public int schema_version;
public List<FactionWarBroadcast> broadcasts = new List<FactionWarBroadcast>();
public sealed class FactionWarDialogueRoot
public int schema_version;
public List<FactionWarDialogueSnippet> snippets = new List<FactionWarDialogueSnippet>();
public sealed class FactionWarCommuniqueRoot
public int schema_version;
public List<FactionWarCommunique> communiques = new List<FactionWarCommunique>();
public sealed class FactionWarLocationOverrideRoot
public int schema_version;
public List<FactionWarLocationOverride> locationOverrides = new List<FactionWarLocationOverride>();
public sealed class FactionWarContentCatalogLoader
public const string EventsFile = "faction_war_events.json";
public const string JournalFile = "faction_war_journal.json";
public const string RadioFile = "faction_war_radio.json";
public const string DialogueFile = "faction_war_dialogue.json";
public const string CommuniquesFile = "faction_war_communiques.json";
public const string LocationOverridesFile = "faction_war_location_overrides.json";
public FactionWarContentCatalog Load(string dataDirectory) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs`

### `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 603 lines / 31953 bytes.
- SHA-256: `5294c96557d10b243115c35fd87fff34a5e8bfaf8ebc546b29723b04ebd2e7b6`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public abstract class FactionWarTrigger
public abstract bool IsSatisfied(FactionWarTriggerContext ctx);
public sealed class FactionWarTriggerContext
public int CurrentDay;
public Func<string, bool> IsChainResolved = _ => false;
public Func<string, bool> HasVisitedLocation = _ => false;
public Func<string, int> StageResolvedDay = _ => -1; // -1 = not yet resolved
public Func<string, bool> IsFlagSet = _ => false;
public sealed class FlagTrigger : FactionWarTrigger
public override bool IsSatisfied(FactionWarTriggerContext ctx) => ctx.IsFlagSet(_flagId);
public sealed class PlayerVisitedTrigger : FactionWarTrigger
public override bool IsSatisfied(FactionWarTriggerContext ctx) => ctx.HasVisitedLocation(_locationId);
public sealed class ChainResolvedTrigger : FactionWarTrigger
public override bool IsSatisfied(FactionWarTriggerContext ctx) => ctx.IsChainResolved(_chainId);
public sealed class DayOffsetTrigger : FactionWarTrigger
public override bool IsSatisfied(FactionWarTriggerContext ctx) {
public sealed class AndTrigger : FactionWarTrigger
public override bool IsSatisfied(FactionWarTriggerContext ctx) {
public sealed class AlwaysTrigger : FactionWarTrigger
public static readonly AlwaysTrigger Instance = new AlwaysTrigger();
public override bool IsSatisfied(FactionWarTriggerContext ctx) => true;
public static class FactionWarTriggerTable
public static readonly Dictionary<string, FactionWarTrigger> ByStageId = Build();
public static FactionWarTrigger For(string stageId) =>
public sealed class FactionWarChainProgress
public string chainId = string.Empty;
public string currentStageId = string.Empty;
public bool resolved;
public List<StageResolution> stageResolutions = new List<StageResolution>();
public sealed class StageResolution
public string stageId = string.Empty;
public int day;
public sealed class FactionWarChainRunnerState
public string systemId = FactionWarChainRunner.SystemId;
public int schemaVersion = 1;
public List<FactionWarChainProgress> chains = new List<FactionWarChainProgress>();
public List<string> visitedLocations = new List<string>();
public int cumulativeMoraleDelta;
public List<string> producedFlags = new List<string>();
public sealed class FactionWarChainRunner
public const string SystemId = "faction_war_chain_runner";
public const int AuthoredEpochStart = 480;
public const int PlayableEpochStart = 180;
public static int ToAuthoredDay(int playableDay) {
public event Action<FactionWarEventChain, FactionWarEventStage>? OnStageSurfaced;
public event Action<FactionWarEventChain, FactionWarEventStage, FactionWarEventChoice>? OnStageResolved;
public event Action<FactionWarEventChain>? OnChainResolved;
public FactionWarChainRunnerState State => _state;
public FactionWarContentCatalog Catalog => _catalog;
public int CumulativeMoraleDelta => _state.cumulativeMoraleDelta;
public Func<string, bool>? ExternalFlagProbe;
public Action<string, int>? StandingDeltaApplier;
public void RecordLocationVisited(string locationId) {
public bool HasVisited(string locationId) => _state.visitedLocations.Contains(locationId);
public bool IsChainResolved(string chainId) {
public FactionWarEventStage? GetSurfacedStage(string chainId, int currentDay) {
public bool IsChoiceAvailable(FactionWarEventStage stage, FactionWarEventChoice choice, int currentDay) {
public bool IsFlagSet(string flagId) {
public void TickDay(int currentDay) {
public void ResolveChoice(string chainId, string stageId, string choiceId, int currentDay) {
public FactionWarChainRunnerState CaptureState() => Clone(_state);
public void RestoreState(FactionWarChainRunnerState state) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs`

### `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 348 lines / 13947 bytes.
- SHA-256: `79467fd76488c2f4747cd21ba11833fdcad6d795a8e86f5a62db166941aa6e75`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class FactionStandingRecord
public string factionId = string.Empty;
public int standing = 0; // -100 (Blood Feud) to +100 (Allied)
public int territorialControlPercent = 20; // 0 to 100
public bool isHostile = false;
public bool isAllied = false;
public class FactionDefenseReadinessPressure
public string sourceId = string.Empty;
public string factionId = string.Empty;
public float magnitude;
public int startDay;
public int endDay;
public class FactionWarSystemState
public List<FactionStandingRecord> factions = new List<FactionStandingRecord>();
public int activeWarTension = 50; // 0 to 100
public string dominantFactionId = "faction_central_garrison";
public List<string> enactedDecrees = new List<string>();
public int totalArtilleryStrikesLogged = 0;
public bool isWarActive = false;
public List<FactionDefenseReadinessPressure> defenseReadinessPressures = new List<FactionDefenseReadinessPressure>();
public class FactionWarSystem
public const string SystemId = "faction_war_system";
public const int HostileStandingThreshold = -50;
public FactionWarSystemState State => _state;
public int WarTension => _state.activeWarTension;
public string DominantFactionId => _state.dominantFactionId;
public bool IsAtWar => _state.isWarActive;
public void SetWarActive(bool active) {
public event Action<string, int> OnFactionStandingChanged;
public event Action<string> OnDecreeEnacted;
public event Action<string, string> OnTerritorialClashOccurred;
public int GetStanding(string factionId) {
public void ModifyStanding(string factionId, int delta) {
public void EnactDecree(string decreeId) {
public bool TryApplyDefenseReadinessPressure( string factionId, float magnitude, int startDay, int endDay, string sourceId)
public float GetDefenseReadiness01(string factionId, int day) {
public void SimulateDailyFriction(int day) {
public FactionWarSystemState CaptureState() {
public void RestoreState(FactionWarSystemState state) {
public int ApplyRetention(Records.RetentionPolicyCatalog? catalog) {
```


# Appendix B.05 — Current Code Architecture: `src/YearOfAsh/YearOfAshHostSession.cs`

### `src/YearOfAsh/YearOfAshHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 345 lines / 16205 bytes.
- SHA-256: `ccefd687193231434f67fa00f2a465d6a8558f7d6c3c7748f94a35fb28e4b6d2`.
- Architecture signals: seeded references=2; save/restore symbols=3; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class YearOfAshHostSession
public YearOfAshTimelineSystem Timeline => _timeline;
public DoorEncounterSystem Encounters => _encounters;
public FactionWarSystem FactionWar => _factionWar;
public QuestlineSystem Quests => _quests;
public YearOfAshDeepFreezeSystem DeepFreeze => _deepFreeze;
public YearOfAshRadonSystem Radon => _radon;
public WarlordDoctrineSystem Warlord => _warlord;
public FactionWarChainRunner WarRunner => _warRunner;
public IReadOnlyList<SurvivorOccupantSnapshot> DemoRoster => _demoRoster;
public void BindWarlord(WarlordDoctrineSystem warlord) {
public static YearOfAshHostSession Create(string dataDir = "", bool loadExistingSave = true) {
public void TickDay(int day) {
public void RecordWarLocationVisited(string locationId) {
public void ResolveWarChoice(string chainId, string stageId, string choiceId, int currentDay) {
public string GetStatusSummary() {
public int CurrentTributeAsk =>
public bool SettleWarlordTribute(int amountPaid, int day, out int nextAsk) {
public string CollectorLine(string state, int day) => _warlord.Catalog.CollectorLine(state, day);
public string WarlordLine() {
public YearOfAshSave CaptureSave() {
public void RestoreSave(YearOfAshSave save) {
```


# Appendix B.06 — Current Code Architecture: `src/Main.YearOfAsh.cs`

### `src/Main.YearOfAsh.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 524 lines / 25850 bytes.
- SHA-256: `bcaecfe610dc6eda1bea88bcc0f55db8f5acca3cbcdc8cc22555af705c27a63e`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=5; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix B.07 — Current Code Architecture: `src/YearOfAsh/FactionWarMapWidget.cs`

### `src/YearOfAsh/FactionWarMapWidget.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 142 lines / 4851 bytes.
- SHA-256: `19aceb69125c811f96de5681f71168b9a517c3a5d8c15920155d419d61dc2741`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class FactionWarMapWidget : PanelContainer
public override void _Ready() {
public void BindSession(YearOfAshHostSession session) {
public override void _ExitTree() {
public void RefreshView() {
```


# Appendix B.08 — Current Code Architecture: `src/UI/RadioIntelligencePanel.cs`

### `src/UI/RadioIntelligencePanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 259 lines / 11526 bytes.
- SHA-256: `d7293f0eac3095a47a53e32128dccaf895175c2f5466bb5893bbefdb20bf05dd`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class RadioIntelligencePanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _radio != null;
public void Bind(ShelterRadioStationSystem radio, OrbitalHarrowTelemetrySystem? harrow = null, int currentDay = 0) {
public void Unbind() {
public override void _Ready() {
public override void _ExitTree() => Unbind();
public void Open() {
public void RefreshView() {
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/faction_war_dialogue.json`

### `Assets/StreamingAssets/Data/faction_war_dialogue.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 21823 bytes / 21823 characters.
- SHA-256: `450b0ac1a2e77f766dec098c54f6ace28bd68e72da874c99e24c0411971ef963`.
- Root keys: `schema_version`, `snippets`.

Array-path census (minimum, maximum, observed rows):

```text
snippets: min=40, max=40, observed_paths=1
```

Representative record fields:

- `body`
- `id`
- `locationId`
- `minDay`
- `speakerTag`

Representative identifiers (ordered, capped for readability):

```text
dlg_d482_checkpoint_quartermasters
dlg_d483_exchange_lean_pool
dlg_d488_understory_relay_move
dlg_d490_switchback_pilgrims
dlg_d493_weighbridge_toll_grumble
dlg_d497_scavengers_clean_crater
dlg_d505_conscription_office_clerks
dlg_d512_weighbridge_reroute
dlg_d526_exchange_roster_kid
dlg_d538_checkpoint_awkward_small_talk
dlg_d552_deserter_hunters
dlg_d549_children_after_the_plaza
dlg_d580_shrine_keepers_doubt
dlg_d568_toll_syndicate_cynicism
dlg_d571_forward_roster_checkpoint
dlg_d573_forward_roster_identity
dlg_d584_d9_cell_debate
dlg_d591_switchback_waystation_doubt
dlg_d485_exchange_wet_grain_scale
dlg_d486_garrison_crate_seal
dlg_d487_civilian_parsnip_stew_scrap
dlg_d489_exchange_drum_bung_dispute
dlg_d492_understory_porcelain_insulator
dlg_d494_garrison_boot_leather
dlg_d498_independent_chalk_boundary
dlg_d502_foundry_cracked_flask_sand
dlg_d508_exchange_axle_grease_delay
dlg_d516_garrison_kerosene_stove
dlg_d518_understory_log_overrun
dlg_d520_civilian_valve_handle_toy
dlg_d528_foundry_crucible_heat_window
dlg_d530_exchange_stamped_chits
dlg_d534_independent_tripwire_slack
dlg_d542_garrison_sick_list_billet
dlg_d546_understory_smudged_pad_entry
dlg_d556_foundry_slag_billet_reject
dlg_d562_garrison_fuel_drum_tare
dlg_d566_independent_blanket_tally
dlg_d574_civilian_kettle_scouring_mutter
dlg_d576_understory_copper_splice_tale
```


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/faction_war_events.json`

### `Assets/StreamingAssets/Data/faction_war_events.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 93855 bytes / 93786 characters.
- SHA-256: `3ec09e02415a45ee4015120041651756e2cf7ada84fd70701abd5e289ef6e455`.
- Root keys: `chains`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
chains: min=38, max=38, observed_paths=1
chains[].factionsInvolved: min=2, max=2, observed_paths=2
chains[].stages: min=2, max=2, observed_paths=2
chains[].stages[].choices: min=2, max=3, observed_paths=4
```

Representative record fields:

- `band`
- `chainId`
- `factionsInvolved`
- `locationId`
- `stages`
- `title`


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/faction_war_journal.json`

### `Assets/StreamingAssets/Data/faction_war_journal.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 17412 bytes / 17398 characters.
- SHA-256: `9fa6c10d07cfcf89781584c5fdad148a92ad95a2c2f191add17e7245662b293c`.
- Root keys: `entries`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
entries: min=26, max=26, observed_paths=1
```

Representative record fields:

- `authorName`
- `body`
- `day`
- `id`
- `locationId`
- `voice`

Representative identifiers (ordered, capped for readability):

```text
journal_d482_mira_queue_count
journal_d486_fennick_ledger_entry
journal_d490_fossey_bean_row
journal_d502_denner_the_list
journal_d509_denner_gone_to_ground
journal_d518_mira_the_almshouse
journal_d528_adaeze_the_coats
journal_d536_fennick_the_new_checkpoint
journal_d543_mira_the_star
journal_d546_mira_after
journal_d555_adaeze_the_split
journal_d560_selwyn_the_frequency
journal_d567_fennick_the_pumphouse
journal_d572_forward_roster_recruit
journal_d575_sella_the_toll_math
journal_d580_toma_the_broken_pattern
journal_d584_d9_cell_leader
journal_d592_vashti_the_scale_holds
journal_d595_mira_the_quiet
journal_d598_denner_the_pause
journal_d601_toma_after_the_theory
journal_d606_mira_the_quiet_peace
journal_warlord_toll_doctrine
journal_warlord_consolidation_doctrine
journal_warlord_annexation_doctrine
journal_warlord_withdrawal_doctrine
```


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/faction_war_radio.json`

### `Assets/StreamingAssets/Data/faction_war_radio.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 17501 bytes / 17481 characters.
- SHA-256: `ed5851965227c0feb5e5b373d25185984eb7cceba7f50829caf3b353fee1b6f7`.
- Root keys: `broadcasts`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
broadcasts: min=33, max=33, observed_paths=1
```

Representative record fields:

- `dayTrigger`
- `frequency`
- `id`
- `isEmergency`
- `message`
- `signalStrength`
- `source`

Representative identifiers (ordered, capped for readability):

```text
radio_d480_span44_automated_loop
radio_d481_garrison_continuity_bulletin
radio_d484_exchange_roster_wire_rebuttal
radio_d487_unsigned_supply_figures
radio_d488_garrison_grain_rebuttal
radio_d490_ash_sign_shrine_transmission
radio_d493_toll_syndicate_rate_notice
radio_d496_understory_clean_strike
radio_d504_garrison_conscription_notice
radio_d507_exchange_roster_wire_conscription
radio_d510_understory_span44_standoff
radio_d516_ash_sign_warning
radio_d518_garrison_almshouse_bulletin
radio_d522_ash_sign_reading_shift
radio_d525_exchange_roster_wire_prices
radio_d534_garrison_exchange_order
radio_d542_understory_something_coming
radio_d546_garrison_plaza_communique
radio_d547_rebuilders_plaza_communique
radio_d559_lima_november_burst
radio_d566_toll_syndicate_quiet_line
radio_d571_forward_roster_checkpoint_notice
radio_d579_ash_sign_shrine_anomaly
radio_d585_understory_spur_road_notice
radio_d589_garrison_ceasefire_notice
radio_d590_rebuilders_ceasefire_notice
radio_d596_forward_roster_holding_position
radio_d600_understory_closing
radio_d606_forward_roster_recognition_notice
radio_warlord_toll_standing
radio_warlord_consolidation
radio_warlord_annexation
radio_warlord_withdrawal
```


# Appendix C.13 — Catalog Census: `Assets/StreamingAssets/Data/faction_war_communiques.json`

### `Assets/StreamingAssets/Data/faction_war_communiques.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 41590 bytes / 41494 characters.
- SHA-256: `cafbdc4c94cfd2b28a63076b397b7d469b3cca06edcf35510dba11b026afbca1`.
- Root keys: `communiques`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
communiques: min=40, max=40, observed_paths=1
```

Representative record fields:

- `authorNote`
- `body`
- `day`
- `eventChainId`
- `factionId`
- `id`
- `title`

Representative identifiers (ordered, capped for readability):

```text
comm_d489_garrison_manifest_inspection
comm_d490_rebuilders_two_scales
comm_d497_garrison_clean_strike
comm_d497_rebuilders_clean_strike
comm_d498_ash_sign_clean_strike
comm_d505_garrison_labor_quota
comm_d507_rebuilders_quota_arithmetic
comm_d513_garrison_span_readiness
comm_d514_rebuilders_span_record
comm_d519_garrison_almshouse
comm_d520_rebuilders_almshouse
comm_d521_ash_sign_almshouse
comm_d523_ash_sign_pilgrim_toll
comm_d526_garrison_waystation_fee
comm_d527_rebuilders_guard_detail
comm_d530_garrison_recorded_concern
comm_d537_garrison_exchange_checkpoint
comm_d538_rebuilders_exchange_checkpoint
comm_d549_garrison_ration_plaza
comm_d550_rebuilders_ration_plaza
comm_d552_ash_sign_ration_plaza
comm_d556_rebuilders_fracture
comm_d561_ash_sign_dead_channel
comm_d568_rebuilders_pumphouse_questions
comm_d570_garrison_pumphouse_coordination
comm_d573_forward_roster_checkpoint
comm_d575_forward_roster_origin
comm_d576_forward_roster_passage_rules
comm_d577_ash_sign_restless_numbers
comm_d581_garrison_shrine_strike
comm_d582_rebuilders_shrine_strike
comm_d583_ash_sign_shrine_strike
comm_d591_ash_sign_ceasefire_pause
comm_d592_garrison_standdown
comm_d593_forward_roster_ceasefire_toll
comm_d594_rebuilders_ceasefire_benchmark
comm_d599_forward_roster_crates
comm_d602_ash_sign_the_question
comm_d607_garrison_forward_roster_recognition
comm_d608_forward_roster_non_recognition
```


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/FactionWarDialogueExpansionTests.cs`

### `Ashfall.Core.Tests/FactionWarDialogueExpansionTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 203; SHA-256: `4830cf7a9e3d5eba535f2f4da8be8f01f8ee6df3e2a48e78e468f8a31d3fc839`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_Loads_Exactly_40_Snippets
All_Dialogue_IDs_Are_Unique
All_Dialogue_IDs_Have_Dlg_Prefix
All_18_Baseline_Snippets_Preserved_With_Original_Keys_And_Bodies
All_22_New_Snippets_Present
All_Snippets_Have_NonEmpty_Fields_And_Valid_MinDay
GetDialogueForLocation_Filters_Correctly_At_Day_Boundaries
GetDialogueForLocation_Wrong_Location_Returns_No_Snippets_For_That_Location
Faction_Context_Distribution_Satisfied
```


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs`

### `Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 219; SHA-256: `d51416b81e386bc4e81b3f415a5e968f25250c2d1904b92beddb5d9b6afd6829`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan92_FactionWarDialogue_LoadsAll40Snippets_WithValidStructureAndGating
Plan99_HardcoreEconomyTuning_LoadsAllTiersFactionPrefsAndPriceShocks
CrossSystem_WartimeDialogue_ReflectsEconomicScarcityAndPriceShocks
CrossSystem_DynamicEconomyAndDialogueGating_ArePureCoreAndDeterministic
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`

### `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 451 lines / 20575 bytes.
- SHA-256: `4dfa917553bf8239ff0ed5799cd97fb81954caa3f0f24d32137f4e220bc092ef`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FactionWarContentCatalog
public IReadOnlyList<FactionWarEventChain> EventChains => _eventChains;
public IReadOnlyList<FactionWarJournalEntry> JournalEntries => _journalEntries;
public IReadOnlyList<FactionWarBroadcast> Broadcasts => _broadcasts;
public IReadOnlyList<FactionWarDialogueSnippet> DialogueSnippets => _dialogueSnippets;
public IReadOnlyList<FactionWarCommunique> Communiques => _communiques;
public IReadOnlyList<FactionWarLocationOverride> LocationOverrides => _locationOverrides;
public int EventChainCount => _eventChains.Count;
public int JournalEntryCount => _journalEntries.Count;
public int BroadcastCount => _broadcasts.Count;
public int DialogueSnippetCount => _dialogueSnippets.Count;
public int CommuniqueCount => _communiques.Count;
public int LocationOverrideCount => _locationOverrides.Count;
public void AddEventChain(FactionWarEventChain chain) => _eventChains.Add(chain);
public void AddJournalEntry(FactionWarJournalEntry entry) => _journalEntries.Add(entry);
public void AddBroadcast(FactionWarBroadcast broadcast) => _broadcasts.Add(broadcast);
public void AddDialogueSnippet(FactionWarDialogueSnippet snippet) => _dialogueSnippets.Add(snippet);
public void AddCommunique(FactionWarCommunique communique) => _communiques.Add(communique);
public void AddLocationOverride(FactionWarLocationOverride entry) => _locationOverrides.Add(entry);
public List<FactionWarEventChain> GetEligibleChains(int day) {
public List<FactionWarJournalEntry> GetJournalForDay(int day) {
public List<FactionWarBroadcast> GetBroadcastsForDay(int day) {
public List<FactionWarDialogueSnippet> GetDialogueForLocation(string locationId, int day) {
public List<FactionWarCommunique> GetCommuniquesForFaction(string factionId, int day) {
public FactionWarLocationOverride? GetActiveLocationOverride(string locationId, int day) {
public sealed class FactionWarEventChain
public string chainId = string.Empty;
public string band = string.Empty;
public string title = string.Empty;
public List<string> factionsInvolved = new List<string>();
public string locationId = string.Empty;
public List<FactionWarEventStage> stages = new List<FactionWarEventStage>();
public sealed class FactionWarEventStage
public string stageId = string.Empty;
public int minDay;
public string triggerCondition = string.Empty;
public string title = string.Empty;
public string bodyText = string.Empty;
public List<FactionWarEventChoice> choices = new List<FactionWarEventChoice>();
public string requiresFlag = string.Empty;
public string producesFlag = string.Empty;
public sealed class FactionWarEventChoice
public string choiceId = string.Empty;
public string text = string.Empty;
public int moraleDelta;
public string leadsToStageId = string.Empty;
public string requiresFlag = string.Empty;
public string producesFlag = string.Empty;
public string standingFactionId = string.Empty;
public int standingDelta;
public sealed class FactionWarJournalEntry
public string id = string.Empty;
public string authorName = string.Empty;
public int day;
public string locationId = string.Empty;
public string voice = string.Empty;
public string body = string.Empty;
public sealed class FactionWarBroadcast
public string id = string.Empty;
public string frequency = string.Empty;
public int dayTrigger;
public string source = string.Empty;
public string message = string.Empty;
public string signalStrength = string.Empty;
public bool isEmergency;
public string audio_cue = string.Empty;
public sealed class FactionWarDialogueSnippet
public string id = string.Empty;
public string locationId = string.Empty;
public int minDay;
public string speakerTag = string.Empty;
public string body = string.Empty;
public sealed class FactionWarCommunique
public string id = string.Empty;
public string eventChainId = string.Empty;
public string factionId = string.Empty;
public int day;
public string title = string.Empty;
public string body = string.Empty;
public string authorNote = string.Empty;
public sealed class FactionWarLocationOverride
public string id = string.Empty;
public string locationId = string.Empty;
public string overrideType = string.Empty;
public int activeFromDay;
public int activeUntilDay;
public string displayName = string.Empty;
public string description = string.Empty;
public sealed class FactionWarEventChainRoot
public int schema_version;
public List<FactionWarEventChain> chains = new List<FactionWarEventChain>();
public sealed class FactionWarJournalRoot
public int schema_version;
public List<FactionWarJournalEntry> entries = new List<FactionWarJournalEntry>();
public sealed class FactionWarBroadcastRoot
public int schema_version;
public List<FactionWarBroadcast> broadcasts = new List<FactionWarBroadcast>();
public sealed class FactionWarDialogueRoot
public int schema_version;
public List<FactionWarDialogueSnippet> snippets = new List<FactionWarDialogueSnippet>();
public sealed class FactionWarCommuniqueRoot
public int schema_version;
public List<FactionWarCommunique> communiques = new List<FactionWarCommunique>();
public sealed class FactionWarLocationOverrideRoot
public int schema_version;
public List<FactionWarLocationOverride> locationOverrides = new List<FactionWarLocationOverride>();
public sealed class FactionWarContentCatalogLoader
public const string EventsFile = "faction_war_events.json";
public const string JournalFile = "faction_war_journal.json";
public const string RadioFile = "faction_war_radio.json";
public const string DialogueFile = "faction_war_dialogue.json";
public const string CommuniquesFile = "faction_war_communiques.json";
public const string LocationOverridesFile = "faction_war_location_overrides.json";
public FactionWarContentCatalog Load(string dataDirectory) {
```


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs`

### `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 603 lines / 31953 bytes.
- SHA-256: `5294c96557d10b243115c35fd87fff34a5e8bfaf8ebc546b29723b04ebd2e7b6`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public abstract class FactionWarTrigger
public abstract bool IsSatisfied(FactionWarTriggerContext ctx);
public sealed class FactionWarTriggerContext
public int CurrentDay;
public Func<string, bool> IsChainResolved = _ => false;
public Func<string, bool> HasVisitedLocation = _ => false;
public Func<string, int> StageResolvedDay = _ => -1; // -1 = not yet resolved
public Func<string, bool> IsFlagSet = _ => false;
public sealed class FlagTrigger : FactionWarTrigger
public override bool IsSatisfied(FactionWarTriggerContext ctx) => ctx.IsFlagSet(_flagId);
public sealed class PlayerVisitedTrigger : FactionWarTrigger
public override bool IsSatisfied(FactionWarTriggerContext ctx) => ctx.HasVisitedLocation(_locationId);
public sealed class ChainResolvedTrigger : FactionWarTrigger
public override bool IsSatisfied(FactionWarTriggerContext ctx) => ctx.IsChainResolved(_chainId);
public sealed class DayOffsetTrigger : FactionWarTrigger
public override bool IsSatisfied(FactionWarTriggerContext ctx) {
public sealed class AndTrigger : FactionWarTrigger
public override bool IsSatisfied(FactionWarTriggerContext ctx) {
public sealed class AlwaysTrigger : FactionWarTrigger
public static readonly AlwaysTrigger Instance = new AlwaysTrigger();
public override bool IsSatisfied(FactionWarTriggerContext ctx) => true;
public static class FactionWarTriggerTable
public static readonly Dictionary<string, FactionWarTrigger> ByStageId = Build();
public static FactionWarTrigger For(string stageId) =>
public sealed class FactionWarChainProgress
public string chainId = string.Empty;
public string currentStageId = string.Empty;
public bool resolved;
public List<StageResolution> stageResolutions = new List<StageResolution>();
public sealed class StageResolution
public string stageId = string.Empty;
public int day;
public sealed class FactionWarChainRunnerState
public string systemId = FactionWarChainRunner.SystemId;
public int schemaVersion = 1;
public List<FactionWarChainProgress> chains = new List<FactionWarChainProgress>();
public List<string> visitedLocations = new List<string>();
public int cumulativeMoraleDelta;
public List<string> producedFlags = new List<string>();
public sealed class FactionWarChainRunner
public const string SystemId = "faction_war_chain_runner";
public const int AuthoredEpochStart = 480;
public const int PlayableEpochStart = 180;
public static int ToAuthoredDay(int playableDay) {
public event Action<FactionWarEventChain, FactionWarEventStage>? OnStageSurfaced;
public event Action<FactionWarEventChain, FactionWarEventStage, FactionWarEventChoice>? OnStageResolved;
public event Action<FactionWarEventChain>? OnChainResolved;
public FactionWarChainRunnerState State => _state;
public FactionWarContentCatalog Catalog => _catalog;
public int CumulativeMoraleDelta => _state.cumulativeMoraleDelta;
public Func<string, bool>? ExternalFlagProbe;
public Action<string, int>? StandingDeltaApplier;
public void RecordLocationVisited(string locationId) {
public bool HasVisited(string locationId) => _state.visitedLocations.Contains(locationId);
public bool IsChainResolved(string chainId) {
public FactionWarEventStage? GetSurfacedStage(string chainId, int currentDay) {
public bool IsChoiceAvailable(FactionWarEventStage stage, FactionWarEventChoice choice, int currentDay) {
public bool IsFlagSet(string flagId) {
public void TickDay(int currentDay) {
public void ResolveChoice(string chainId, string stageId, string choiceId, int currentDay) {
public FactionWarChainRunnerState CaptureState() => Clone(_state);
public void RestoreState(FactionWarChainRunnerState state) {
```


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs`

### `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 348 lines / 13947 bytes.
- SHA-256: `79467fd76488c2f4747cd21ba11833fdcad6d795a8e86f5a62db166941aa6e75`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class FactionStandingRecord
public string factionId = string.Empty;
public int standing = 0; // -100 (Blood Feud) to +100 (Allied)
public int territorialControlPercent = 20; // 0 to 100
public bool isHostile = false;
public bool isAllied = false;
public class FactionDefenseReadinessPressure
public string sourceId = string.Empty;
public string factionId = string.Empty;
public float magnitude;
public int startDay;
public int endDay;
public class FactionWarSystemState
public List<FactionStandingRecord> factions = new List<FactionStandingRecord>();
public int activeWarTension = 50; // 0 to 100
public string dominantFactionId = "faction_central_garrison";
public List<string> enactedDecrees = new List<string>();
public int totalArtilleryStrikesLogged = 0;
public bool isWarActive = false;
public List<FactionDefenseReadinessPressure> defenseReadinessPressures = new List<FactionDefenseReadinessPressure>();
public class FactionWarSystem
public const string SystemId = "faction_war_system";
public const int HostileStandingThreshold = -50;
public FactionWarSystemState State => _state;
public int WarTension => _state.activeWarTension;
public string DominantFactionId => _state.dominantFactionId;
public bool IsAtWar => _state.isWarActive;
public void SetWarActive(bool active) {
public event Action<string, int> OnFactionStandingChanged;
public event Action<string> OnDecreeEnacted;
public event Action<string, string> OnTerritorialClashOccurred;
public int GetStanding(string factionId) {
public void ModifyStanding(string factionId, int delta) {
public void EnactDecree(string decreeId) {
public bool TryApplyDefenseReadinessPressure( string factionId, float magnitude, int startDay, int endDay, string sourceId)
public float GetDefenseReadiness01(string factionId, int day) {
public void SimulateDailyFriction(int day) {
public FactionWarSystemState CaptureState() {
public void RestoreState(FactionWarSystemState state) {
public int ApplyRetention(Records.RetentionPolicyCatalog? catalog) {
```


# Appendix E.19 — Supporting Code Evidence: `src/YearOfAsh/YearOfAshHostSession.cs`

### `src/YearOfAsh/YearOfAshHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 345 lines / 16205 bytes.
- SHA-256: `ccefd687193231434f67fa00f2a465d6a8558f7d6c3c7748f94a35fb28e4b6d2`.
- Architecture signals: seeded references=2; save/restore symbols=3; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class YearOfAshHostSession
public YearOfAshTimelineSystem Timeline => _timeline;
public DoorEncounterSystem Encounters => _encounters;
public FactionWarSystem FactionWar => _factionWar;
public QuestlineSystem Quests => _quests;
public YearOfAshDeepFreezeSystem DeepFreeze => _deepFreeze;
public YearOfAshRadonSystem Radon => _radon;
public WarlordDoctrineSystem Warlord => _warlord;
public FactionWarChainRunner WarRunner => _warRunner;
public IReadOnlyList<SurvivorOccupantSnapshot> DemoRoster => _demoRoster;
public void BindWarlord(WarlordDoctrineSystem warlord) {
public static YearOfAshHostSession Create(string dataDir = "", bool loadExistingSave = true) {
public void TickDay(int day) {
public void RecordWarLocationVisited(string locationId) {
public void ResolveWarChoice(string chainId, string stageId, string choiceId, int currentDay) {
public string GetStatusSummary() {
public int CurrentTributeAsk =>
public bool SettleWarlordTribute(int amountPaid, int day, out int nextAsk) {
public string CollectorLine(string state, int day) => _warlord.Catalog.CollectorLine(state, day);
public string WarlordLine() {
public YearOfAshSave CaptureSave() {
public void RestoreSave(YearOfAshSave save) {
```


# Appendix E.20 — Supporting Code Evidence: `src/Main.YearOfAsh.cs`

### `src/Main.YearOfAsh.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 524 lines / 25850 bytes.
- SHA-256: `bcaecfe610dc6eda1bea88bcc0f55db8f5acca3cbcdc8cc22555af705c27a63e`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=5; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/FactionWarDialogueExpansionTests.cs`

### `Ashfall.Core.Tests/FactionWarDialogueExpansionTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 203; SHA-256: `4830cf7a9e3d5eba535f2f4da8be8f01f8ee6df3e2a48e78e468f8a31d3fc839`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_Loads_Exactly_40_Snippets
All_Dialogue_IDs_Are_Unique
All_Dialogue_IDs_Have_Dlg_Prefix
All_18_Baseline_Snippets_Preserved_With_Original_Keys_And_Bodies
All_22_New_Snippets_Present
All_Snippets_Have_NonEmpty_Fields_And_Valid_MinDay
GetDialogueForLocation_Filters_Correctly_At_Day_Boundaries
GetDialogueForLocation_Wrong_Location_Returns_No_Snippets_For_That_Location
Faction_Context_Distribution_Satisfied
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs`

### `Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 219; SHA-256: `d51416b81e386bc4e81b3f415a5e968f25250c2d1904b92beddb5d9b6afd6829`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan92_FactionWarDialogue_LoadsAll40Snippets_WithValidStructureAndGating
Plan99_HardcoreEconomyTuning_LoadsAllTiersFactionPrefsAndPriceShocks
CrossSystem_WartimeDialogue_ReflectsEconomicScarcityAndPriceShocks
CrossSystem_DynamicEconomyAndDialogueGating_ArePureCoreAndDeterministic
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
| faction-war content file loading and family assembly | FactionWarContentCatalogLoader | dialogue row storage and location/day query | FactionWarContentCatalog | Owner emits/reads a typed fact; no mirror state. |
| faction-war content file loading and family assembly | FactionWarContentCatalogLoader | war state and current event/runner context | FactionWarChainRunner/System | Owner emits/reads a typed fact; no mirror state. |
| faction-war content file loading and family assembly | FactionWarContentCatalogLoader | catalog composition and current day/host state | YearOfAshHostSession | Owner emits/reads a typed fact; no mirror state. |
| faction-war content file loading and family assembly | FactionWarContentCatalogLoader | map/briefing/radio surfaces | Faction war presentation | Owner emits/reads a typed fact; no mirror state. |
| faction-war content file loading and family assembly | FactionWarContentCatalogLoader | row, gate and integration proof | Dialogue focused tests | Owner emits/reads a typed fact; no mirror state. |
| dialogue row storage and location/day query | FactionWarContentCatalog | faction-war content file loading and family assembly | FactionWarContentCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| dialogue row storage and location/day query | FactionWarContentCatalog | war state and current event/runner context | FactionWarChainRunner/System | Owner emits/reads a typed fact; no mirror state. |
| dialogue row storage and location/day query | FactionWarContentCatalog | catalog composition and current day/host state | YearOfAshHostSession | Owner emits/reads a typed fact; no mirror state. |
| dialogue row storage and location/day query | FactionWarContentCatalog | map/briefing/radio surfaces | Faction war presentation | Owner emits/reads a typed fact; no mirror state. |
| dialogue row storage and location/day query | FactionWarContentCatalog | row, gate and integration proof | Dialogue focused tests | Owner emits/reads a typed fact; no mirror state. |
| war state and current event/runner context | FactionWarChainRunner/System | faction-war content file loading and family assembly | FactionWarContentCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| war state and current event/runner context | FactionWarChainRunner/System | dialogue row storage and location/day query | FactionWarContentCatalog | Owner emits/reads a typed fact; no mirror state. |
| war state and current event/runner context | FactionWarChainRunner/System | catalog composition and current day/host state | YearOfAshHostSession | Owner emits/reads a typed fact; no mirror state. |
| war state and current event/runner context | FactionWarChainRunner/System | map/briefing/radio surfaces | Faction war presentation | Owner emits/reads a typed fact; no mirror state. |
| war state and current event/runner context | FactionWarChainRunner/System | row, gate and integration proof | Dialogue focused tests | Owner emits/reads a typed fact; no mirror state. |
| catalog composition and current day/host state | YearOfAshHostSession | faction-war content file loading and family assembly | FactionWarContentCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| catalog composition and current day/host state | YearOfAshHostSession | dialogue row storage and location/day query | FactionWarContentCatalog | Owner emits/reads a typed fact; no mirror state. |
| catalog composition and current day/host state | YearOfAshHostSession | war state and current event/runner context | FactionWarChainRunner/System | Owner emits/reads a typed fact; no mirror state. |
| catalog composition and current day/host state | YearOfAshHostSession | map/briefing/radio surfaces | Faction war presentation | Owner emits/reads a typed fact; no mirror state. |
| catalog composition and current day/host state | YearOfAshHostSession | row, gate and integration proof | Dialogue focused tests | Owner emits/reads a typed fact; no mirror state. |
| map/briefing/radio surfaces | Faction war presentation | faction-war content file loading and family assembly | FactionWarContentCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| map/briefing/radio surfaces | Faction war presentation | dialogue row storage and location/day query | FactionWarContentCatalog | Owner emits/reads a typed fact; no mirror state. |
| map/briefing/radio surfaces | Faction war presentation | war state and current event/runner context | FactionWarChainRunner/System | Owner emits/reads a typed fact; no mirror state. |
| map/briefing/radio surfaces | Faction war presentation | catalog composition and current day/host state | YearOfAshHostSession | Owner emits/reads a typed fact; no mirror state. |
| map/briefing/radio surfaces | Faction war presentation | row, gate and integration proof | Dialogue focused tests | Owner emits/reads a typed fact; no mirror state. |
| row, gate and integration proof | Dialogue focused tests | faction-war content file loading and family assembly | FactionWarContentCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| row, gate and integration proof | Dialogue focused tests | dialogue row storage and location/day query | FactionWarContentCatalog | Owner emits/reads a typed fact; no mirror state. |
| row, gate and integration proof | Dialogue focused tests | war state and current event/runner context | FactionWarChainRunner/System | Owner emits/reads a typed fact; no mirror state. |
| row, gate and integration proof | Dialogue focused tests | catalog composition and current day/host state | YearOfAshHostSession | Owner emits/reads a typed fact; no mirror state. |
| row, gate and integration proof | Dialogue focused tests | map/briefing/radio surfaces | Faction war presentation | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 18→40 target with a 40-row current corpus census and a location/day/faction-context matrix. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Verify every location reference against current location/expedition/Verdict route families before calling a row reachable. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Audit the current host/UI route for an overheard-dialogue surface and record a bounded integration proposal if absent. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Preserve authored dialogue as ambient prose; do not turn it into hidden quest flags or a second faction-war simulation. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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

> **Scale honesty clause.** The requested target for this expansion effort is two million characters. A single authoring pass cannot responsibly produce two million characters of *verified* planning content, and the repository's own constitution (Part 0.4 of v1.0; `AGENTS.md` rules 7–8) forbids manufacturing padded work. v2.0 therefore defines a Multi-Session Growth Protocol (Part VI): the factory is designed to be *appended* session by session, each session adding one or more verified volumes (expanded subsystem deep maps, prose spec libraries, backlog batches), until the corpus reaches the target size organically. The Part VI protocol is the only sanctioned path to the target; bulk generation of unverified prose is a NON-CANON act.

> **DR-05 — A process script lives inside the data authority. VERIFIED (hygiene finding).**
`Assets/StreamingAssets/Data/` contains `rewrite.py` alongside the JSON catalogs. The data directory is canonically "the sole authored JSON data authority" (`AGENTS.md` rule 3); a Python rewrite script inside it is a process artifact in a content directory. Recommended handling: a Tooling-lane (Lane H) subject plan proposing relocation of the script to `scripts/` or `tools/` with a documented rationale, after verifying what the script rewrites and who calls it. Do not move it without call-site verification; it may be load-bearing for a historical catalog migration.

> **DR-06 — Integration ledger state differs from the v1.0 queue snapshot. VERIFIED.**
Live `INTEGRATION_PLANS.md` (read 2026-09-24) shows, at minimum: the **XP Expansion W1** batch ACTIVE (difficulty authority package `XP-WAVE1-DIFFICULTY-AUTHORITY`, with a premise correction recorded against Plan 122 SOFC fuel); the **DISTRESS-SIGNALS-9-12** flagship COMPLETE and presented for acceptance; **Plan 24 CLOSED** (Wave 8, 2026-09-17, signatures resolved 2026-09-18, ward staffing sealed under option b, `DEBT-PLAN24-MEDICAL-WARD-STAFFING` RETIRED); **19A/19B/19C** waves closed with evidence (Endgame 84/84 PASS, focused suites 47 PASS, `verify-fast.sh` reported ALL 47 GATES PASSED); **C2[2] Plan 17** legibility executed with a documented not-executed list (Plan 31 semantic-kind authority, 17C audio phases, 17B deep test matrix remain open gaps); **PR 3 content seal SEALED 2026-09-19** (`CF-P1-DISTRESS-CONTENT-SEAL`); the **availability consumer RETIRED** (Wave 9 Part 2, Option B approved; `SignalTrustAvailability` retained as a pure-math specification pin). Consequence: subject plans in the radio/distress domain must treat the rescue-signal runtime as *sealed and closed*, not as an open expansion surface, unless they extend it through its recorded seams.

> **DR-07 — Gate-count and test-total drift. HIGH CONFIDENCE.**
The v1.0 bible states 57 CI gates at v1.1.0 (53 fast + 3 full + 1 performance) and quotes both 11,098 and 11,697 full-suite totals from different handoffs. The live 19-wave closeout evidence in `INTEGRATION_PLANS.md` records `verify-fast.sh` ALL 47 GATES PASSED at that batch's close. These figures cannot all describe the same instant. Factory rule: any subject plan that names a gate count or test total must re-verify the number against the live gate inventory at drafting time and cite the closeout it came from. Never carry counts forward from this or any prior document.

> **DR-08 — Wave directories beyond the v1.0 history. VERIFIED.**
`docs/plans/` (126 entries) contains wave directories `wave8_part2/`, `wave9_part2/`, `wave10_part1/`, `wave10_part2/`, `wave11_part1/`, `wave11_part2/`, `wave12_part1_1/`, `flagship_b5_b8/`, and `xp/`, plus `UNCLAIMED_CORPUS_CENSUS.md`, `UNBLOCKED_PLANS_AUDIT_2026-09-19.md`, and `WAVE10_MICRO_DEFERRAL_SWEEP.md`. Two of these are standing expansion inputs: `UNCLAIMED_CORPUS_CENSUS.md` (authored content no system consumes — a utilization-seam backlog) and the unblocked-plans audit. The Factory Protocol consumes both.

> The following v1.0 structures were confirmed by the audit and remain authoritative: the four-tier architecture (Tier 1 data authority in `Assets/StreamingAssets/Data/`; Tier 2 engine-free Core; Tier 3 `src/Host` + `src/UI`; Tier 4 xUnit plus the `HostCli` selftest surface); the `AGENTS.md` non-negotiable rules (Godot authoritative, Core engine-free, JSON authoritative, one authority per concern, focused verification); the narrative corpus under `Assets/StreamingAssets/Data/narrative/` (present in the live listing); the faction, economy, weather, Year-of-Ash, moral-choice, muster, and verdict catalog families (all present live); and the plan-discipline artifacts (`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `SESSION_HANDOFF.md`) at root.

> **Step 1 — Premise sweep (mandatory, every time).**
Before selecting any candidate, the session re-verifies premises against live source: the live `Assets/StreamingAssets/Data/` listing (duplication firewall, DR-04), `INTEGRATION_PLANS.md` current batch (DR-06), `WORKTREE_OWNERSHIP.md` claims (DR-09), `KNOWN_DEBT.md`, the root coordination files (DR-01), `docs/gaps/` and `docs/incidents/` (DR-02), and `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` (DR-08). Output: a short premise sheet. A candidate whose premise fails the sweep is discarded, not patched.

> **Step 5 — Run the continuity and anti-duplication checklist.**
The v1.0 checklist (Part 13.2) applies in full, plus two factory additions: (a) duplication firewall — prove the candidate does not duplicate any live catalog, system, or `docs/` authority map; (b) unclaimed-content check — if the candidate's content domain appears in `UNCLAIMED_CORPUS_CENSUS.md`, the plan must wire the unclaimed content first or explain why new content outranks it.

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

The subject is a living-world dialogue corpus whose loader/query contract is live but whose dedicated player route must be proven. The plan expands reference integrity, temporal gating, presentation truthfulness and stable ambient selection without inventing mechanics.

- **faction-war content file loading and family assembly** remains with `FactionWarContentCatalogLoader` at `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`. Owns catalog parsing; it does not decide player reachability.
- **dialogue row storage and location/day query** remains with `FactionWarContentCatalog` at `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`. Owns static content and deterministic filtering.
- **war state and current event/runner context** remains with `FactionWarChainRunner/System` at `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs; Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs`. Owns war simulation; dialogue cannot alter standing or territory.
- **catalog composition and current day/host state** remains with `YearOfAshHostSession` at `src/YearOfAsh/YearOfAshHostSession.cs`. Binds the catalog; it must not duplicate dialogue selection.
- **map/briefing/radio surfaces** remains with `Faction war presentation` at `src/Main.YearOfAsh.cs; src/YearOfAsh/FactionWarMapWidget.cs; src/UI/RadioIntelligencePanel.cs`. Read-only candidate projections; current source does not prove a dedicated dialogue panel.
- **row, gate and integration proof** remains with `Dialogue focused tests` at `Ashfall.Core.Tests/FactionWarDialogueExpansionTests.cs; Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs`. Focused evidence surface.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load the 40-row corpus through the current loader
2. bind the catalog to Year-of-Ash host state
3. read current player location and campaign day
4. query eligible snippets by canonical location and minDay
5. select/present an ambient line without changing war state
6. optionally route a stable knowledge/journal fact only if an existing owner accepts it
7. leave faction standing/territory untouched

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Snippet rows are immutable catalog data; selection is a pure query over location and current day.
- `minDay` is an availability lower bound, not a hidden outcome or a persisted heard-once flag.
- The catalog preserves family order and returns only rows matching the current location/day contract.
- A missing/empty location result is an honest ambient absence, not a reason to invent dialogue.

- Every row has a unique ID, non-empty speaker/body, finite day gate and a resolvable current location.
- A location with no eligible row returns an empty result; it does not fall back to a row from another location.
- The same location/day query returns the same ordered rows regardless of host/frame timing.
- Dialogue cannot grant standing, items, quests or flags without an explicit existing owner command.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/YearOfAsh/YearOfAshHostSession.cs
- src/Main.YearOfAsh.cs
- src/YearOfAsh/FactionWarMapWidget.cs
- src/UI/RadioIntelligencePanel.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/FactionWarDialogueExpansionTests.cs
- Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs

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
| S-01 | 92-01 load 40 snippets | load the 40-row corpus through the current loader | Snippet rows are immutable catalog data; selection is a pure query over location and current day. | A row references a location family that no current route exposes. | FactionWarContentCatalogLoader |
| S-02 | 92-02 location before minDay | bind the catalog to Year-of-Ash host state | `minDay` is an availability lower bound, not a hidden outcome or a persisted heard-once flag. | A panel displays a snippet from the wrong day/location. | FactionWarContentCatalogLoader |
| S-03 | 92-03 location at minDay | read current player location and campaign day | The catalog preserves family order and returns only rows matching the current location/day contract. | A line changes faction standing or flags without an owner command. | FactionWarContentCatalogLoader |
| S-04 | 92-04 unknown location | query eligible snippets by canonical location and minDay | A missing/empty location result is an honest ambient absence, not a reason to invent dialogue. | Catalog iteration order differs between hosts. | FactionWarContentCatalogLoader |
| S-05 | 92-05 empty eligible set | select/present an ambient line without changing war state | Snippet rows are immutable catalog data; selection is a pure query over location and current day. | A new dialogue save store duplicates journal/knowledge state. | FactionWarContentCatalogLoader |
| S-06 | 92-06 stable query order | optionally route a stable knowledge/journal fact only if an existing owner accepts it | `minDay` is an availability lower bound, not a hidden outcome or a persisted heard-once flag. | A row references a location family that no current route exposes. | FactionWarContentCatalogLoader |
| S-07 | 92-07 host bind year-of-ash | leave faction standing/territory untouched | The catalog preserves family order and returns only rows matching the current location/day contract. | A panel displays a snippet from the wrong day/location. | FactionWarContentCatalogLoader |
| S-08 | 92-08 ambient presentation no state change | load the 40-row corpus through the current loader | A missing/empty location result is an honest ambient absence, not a reason to invent dialogue. | A line changes faction standing or flags without an owner command. | FactionWarContentCatalogLoader |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 92-TC-01 schema and unique IDs | data | schema and unique IDs; verify the current owner and its negative boundary without inventing a second authority. | FactionWarContentCatalogLoader |
| T-02 | 92-TC-02 location reference validation | unit | location reference validation; verify the current owner and its negative boundary without inventing a second authority. | FactionWarContentCatalogLoader |
| T-03 | 92-TC-03 day gate lower bound | persistence | day gate lower bound; verify the current owner and its negative boundary without inventing a second authority. | FactionWarContentCatalogLoader |
| T-04 | 92-TC-04 empty query result | determinism | empty query result; verify the current owner and its negative boundary without inventing a second authority. | FactionWarContentCatalogLoader |
| T-05 | 92-TC-05 location filtering | host | location filtering; verify the current owner and its negative boundary without inventing a second authority. | FactionWarContentCatalogLoader |
| T-06 | 92-TC-06 stable ordering | UI/accessibility | stable ordering; verify the current owner and its negative boundary without inventing a second authority. | FactionWarContentCatalogLoader |
| T-07 | 92-TC-07 loader malformed family behavior | cross-system | loader malformed family behavior; verify the current owner and its negative boundary without inventing a second authority. | FactionWarContentCatalogLoader |
| T-08 | 92-TC-08 host binding | data | host binding; verify the current owner and its negative boundary without inventing a second authority. | FactionWarContentCatalogLoader |
| T-09 | 92-TC-09 no standing mutation | unit | no standing mutation; verify the current owner and its negative boundary without inventing a second authority. | FactionWarContentCatalogLoader |
| T-10 | 92-TC-10 no quest/flag mutation | persistence | no quest/flag mutation; verify the current owner and its negative boundary without inventing a second authority. | FactionWarContentCatalogLoader |
| T-11 | 92-TC-11 tone/provenance review | determinism | tone/provenance review; verify the current owner and its negative boundary without inventing a second authority. | FactionWarContentCatalogLoader |
| T-12 | 92-TC-12 future projection contract | host | future projection contract; verify the current owner and its negative boundary without inventing a second authority. | FactionWarContentCatalogLoader |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 19 | `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs` | current reference count; inspect the caller before treating it as a live route |
| 18 | `Ashfall.Core.Tests/FactionWarFlagExtensionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 17 | `Ashfall.Core.Tests/FactionWarChainRunnerTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 16 | `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 15 | `Ashfall.Core.Tests/FactionWarCommuniqueExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 14 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 13 | `src/YearOfAsh/YearOfAshHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Ashfall.Core.Tests/Economy/Plan92_99WarEconomyIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Ashfall.Core.Tests/FactionWarDialogueExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `Ashfall.Core.Tests/FactionWarContentCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `Ashfall.Core.Tests/DataWiringIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/World/Plan124_125WarMoralIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/YearOfAshTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Muster/FactionEcologyHeadlessDemo.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/HostCli.SelfTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Main.YearOfAsh.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/FactionWarClockTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/FactionWarLocationOverridesExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/UI/BasalRadonMigrationPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/YearOfAsh/FactionWarMapWidget.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/NarrativeAndFactionWarIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Host/HostCli.PanelTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/UI/FactionCommuniqueBoardPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/UI/FactionsPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/UI/MapPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/YearOfAsh/GeothermalHeatingWidget.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/YearOfAsh/RadonVentilationWidget.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/CatalogIntegrityRules.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/HostCli.FactionCommuniqueSelfTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/YearOfAsh/YearOfAshSaveStore.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/faction_war_dialogue.json`

### `Assets/StreamingAssets/Data/faction_war_dialogue.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 21823; characters: 21823.
- SHA-256: `450b0ac1a2e77f766dec098c54f6ace28bd68e72da874c99e24c0411971ef963`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `snippets`

#### `snippets` — 40 current rows

- Row 001 `dlg_d482_checkpoint_quartermasters`: `{"body":"\"Exchange numbers don't match ours again.\" \"They never match. That's what a second ledger's for — so both sides can be right at the same time.\" \"Harven doesn't see it that way.\" \"Harven doesn't do inventory. Harven does dec…`
- Row 002 `dlg_d483_exchange_lean_pool`: `{"body":"\"Still hasn't picked a direction.\" \"Three years. I've got money down it falls east.\" \"Everyone's got money down east, that's why it hasn't fallen east yet — this whole silo's just waiting to make somebody wrong on purpose.\" …`
- Row 003 `dlg_d488_understory_relay_move`: `{"body":"\"You're moving it again.\" \"Moving it is the whole point. A relay that sits still is just a target with a schedule.\" \"Nobody's shot at it yet.\" \"Nobody's found it yet. Those aren't the same sentence.\" \"Seems like a lot of …`
- Row 004 `dlg_d490_switchback_pilgrims`: `{"body":"\"Numbers were steady last time I read them.\" \"Steady's not the same as good.\" \"Steady's the only thing I've got faith is supposed to mean anymore. Good went out with the first winter.\" \"Keep climbing. You'll feel better at …`
- Row 005 `dlg_d493_weighbridge_toll_grumble`: `{"body":"\"Rate's up again.\" \"Rate's always up. Man's never once lowered it in three years, not even the season the road washed out and half his customers couldn't reach the booth.\" \"Why do we still cross here, then?\" \"Because the ot…`
- Row 006 `dlg_d497_scavengers_clean_crater`: `{"body":"\"Rads are low right at the lip. Lower than the field around it.\" \"Wrong way round. Should be hot at the point of impact, not scoured clean.\" \"Maybe it wasn't meant to hurt anybody.\" \"Everything out here's meant to hurt some…`
- Row 007 `dlg_d505_conscription_office_clerks`: `{"body":"\"Anyone taking his shift proper, or is it just us trading it back and forth?\" \"Just us. Nobody wants the counter he used to run. Feels like taking his coat off a body that isn't cold yet.\" \"He'll turn up on a list somewhere. …`
- Row 008 `dlg_d512_weighbridge_reroute`: `{"body":"\"Half of what used to cross here goes around now.\" \"Around costs three extra days and people still take it.\" \"Why?\" \"Because the long way doesn't run through a Garrison checkpoint asking where you're bound and why. We ask t…`
- Row 009 `dlg_d526_exchange_roster_kid`: `{"body":"\"Feels wrong, doesn't it. The coats.\" \"Feels like three years of not needing them, ending on a Tuesday for no better reason than everyone else already stopped believing we didn't.\" \"Adaeze hates it.\" \"Adaeze hating it is th…`
- Row 010 `dlg_d538_checkpoint_awkward_small_talk`: `{"body":"\"Weather's turning.\" \"It's ash. It doesn't turn, it just falls slightly differently.\" \"Trying to make conversation.\" \"I know. I appreciate it, actually. Doesn't make the clipboard less of a clipboard.\" \"No. It doesn't. So…`
- Row 011 `dlg_d552_deserter_hunters`: `{"body":"\"Fourth one this month I've had to write up as absconded rather than chase.\" \"Why write up instead of chase?\" \"Because chasing means finding, and finding means deciding what happens next, and I've decided what happens next of…`
- Row 012 `dlg_d549_children_after_the_plaza`: `{"body":"\"My mum says don't go past the rope they put up.\" \"Mira gave her token away right before. To the radio lady.\" \"Was it lucky?\" \"Mira says stars are only lucky if you don't need them to be. I don't think that's how luck works…`
- Row 013 `dlg_d580_shrine_keepers_doubt`: `{"body":"\"Toma says we stop teaching the sparing.\" \"Stop teaching it, or admit we never understood it?\" \"Does it matter, at this point?\" \"It matters to the pilgrims still climbing up here expecting an answer. It mattered to us three…`
- Row 014 `dlg_d568_toll_syndicate_cynicism`: `{"body":"\"Fennick's water deal holding?\" \"Held long enough for him to collect. Whether it holds for the slope's actual growing season is somebody else's arithmetic.\" \"Doesn't bother you?\" \"Everything that crosses this bridge is some…`
- Row 015 `dlg_d571_forward_roster_checkpoint`: `{"body":"\"First column paid without a word.\" \"Word'll come. Give it a week, someone'll ask us who put us in charge of the road.\" \"What do we tell them?\" \"Nobody. That's the whole answer. Nobody put us here. We just got here first an…`
- Row 016 `dlg_d573_forward_roster_identity`: `{"body":"\"Someone in that column called us bandits.\" \"Bandits don't log the count.\" \"We log the count.\" \"Every crate, every head, in a hand that reads like a receipt. That's not a bandit, that's a customs post that opened early.\" \…`
- Row 017 `dlg_d584_d9_cell_debate`: `{"body":"\"Order says deny what can't be held.\" \"Order was written before there was a checkpoint out there worth counting. Doctrine doesn't update itself.\" \"So we update it.\" \"So somebody above my clearance updates it. Until then I l…`
- Row 018 `dlg_d591_switchback_waystation_doubt`: `{"body":"\"They still haven't repainted the board at the cairn.\" \"What would they even write? The old line wasn't wrong for three years, and then one morning it was.\" \"Toma won't say 'spared' anymore. I asked her straight and she just …`
- Row 019 `dlg_d485_exchange_wet_grain_scale`: `{"body":"\"Bag's damp at the corner. That's water weight, not rye.\" \"Rye pulled out of the frost has dew on the husk, that's natural moisture.\" \"Natural moisture doesn't pay the transit levy. We deduct four ounces for drying or you hau…`
- Row 020 `dlg_d486_garrison_crate_seal`: `{"body":"\"Wire's cut on this third one.\" \"Cut or snapped?\" \"Clean shears. Snapped leaves a burr you can feel with your thumb.\" \"Then someone took their four clips before the truck crossed the culvert.\" \"Who do I bill for the seal?…`
- Row 021 `dlg_d487_civilian_parsnip_stew_scrap`: `{"body":"\"Take the thick heel of the bread.\" \"I had the turnip end already. You take it.\" \"I dropped it in the dirt anyway. It's half ash. Eat it before the wind steals the warmth.\" \"Tastes better with ash. Cuts the bitterness.\"","…`
- Row 022 `dlg_d489_exchange_drum_bung_dispute`: `{"body":"\"Your bung's leaking into the drip trough.\" \"It's just splash from the fill pipe.\" \"It's grey water running back into the clean intake. Pull your barrel back before the supervisor sees it.\" \"Supervisor's inside warming his …`
- Row 023 `dlg_d492_understory_porcelain_insulator`: `{"body":"\"Hairline crack right across the ceramic bell.\" \"Wrap it with vulcanized tape and haul it back up.\" \"Tape won't stop fifty watts of RF arc when the frost melts.\" \"Then keep the transmitter down at twenty watts.\" \"Twenty w…`
- Row 024 `dlg_d494_garrison_boot_leather`: `{"body":"\"Left heel's giving again.\" \"Grease it with tallow before you turn in.\" \"Tallow's for the breech blocks now, Corporal says.\" \"Corporal sleeps in the interior bunk with dry socks. Take the lard from the mess tin and don't te…`
- Row 025 `dlg_d498_independent_chalk_boundary`: `{"body":"\"Mark says three crosses. That's Miller's crew.\" \"Miller's crew hasn't been north of the Cut since the second blizzard.\" \"Chalk doesn't wash off in snow. Mark's still dry.\" \"Mark's on a rusted door with no hinges. Take the …`
- Row 026 `dlg_d502_foundry_cracked_flask_sand`: `{"body":"\"Look at that scab. What did I tell you about the vent needle?\" \"I vented every two inches like the board says.\" \"You vented two inches from the pattern, not the wall. Steam had nowhere to go but straight into the molten bron…`
- Row 027 `dlg_d508_exchange_axle_grease_delay`: `{"body":"\"Three hours behind schedule. The team should have cleared the pass at dawn.\" \"Axle grease froze in the hubs again. Happens every third cold snap.\" \"You'd think after three winters they'd learn to cut the grease with kerosene…`
- Row 028 `dlg_d516_garrison_kerosene_stove`: `{"body":"\"Staff tent's got the blue heater going again.\" \"You smell it from here?\" \"Paraffin smoke has a drift to it. You don't mistake that for coal.\" \"Let them have the heater. Cold keeps a sentry from dozing off.\" \"That's what …`
- Row 029 `dlg_d518_understory_log_overrun`: `{"body":"\"The 21:00 window was ninety seconds over.\" \"The casualty names ran long. You want me to cut someone's kin off mid-syllable?\" \"I want the battery pack to last until midnight. Every second past the mark is plate voltage we don…`
- Row 030 `dlg_d520_civilian_valve_handle_toy`: `{"body":"\"What did the big wheel turn before the noise, Ma?\" \"Turned water into the basement heaters, sweet. Made the radiators clink in the morning.\" \"Can it turn the water back on now?\" \"No, love. The pipe's broken under the stree…`
- Row 031 `dlg_d528_foundry_crucible_heat_window`: `{"body":"\"Flame's pale straw. We're at temperature.\" \"Billet team's still lining the ingot troughs with loam.\" \"Tell them they have four minutes before the pot cools off. I'm not blowing another hour of bellows work because their shov…`
- Row 032 `dlg_d530_exchange_stamped_chits`: `{"body":"\"Garrison stamp is purple this month. Last month was green.\" \"Means the old passes are scrap paper.\" \"I've got forty bushels cleared under the green mark.\" \"Then you sell twenty to pay for the purple mark, or you sit here u…`
- Row 033 `dlg_d534_independent_tripwire_slack`: `{"body":"\"Line's dragging in the thistle again.\" \"Wind caught the tin cans. Sounded like a patrol three times last watch.\" \"Tie a stone to the dropper to keep the tension even.\" \"Only got three stones left that aren't frozen to the …`
- Row 034 `dlg_d542_garrison_sick_list_billet`: `{"body":"\"Vane's got blood in his spit. He can't stand the cut watch.\" \"He's marked fit on the morning roster.\" \"He's marked fit because the pencil was in your hand, not because his lungs cleared.\" \"Three posts are empty at the culv…`
- Row 035 `dlg_d546_understory_smudged_pad_entry`: `{"body":"\"Is that a seven or a one?\" \"Graphite smeared when the condensation dripped off the pipe.\" \"One means the cache is at the switchback; seven means the culvert.\" \"Run the culvert first. If the charges are primed, you'll know …`
- Row 036 `dlg_d556_foundry_slag_billet_reject`: `{"body":"\"Inspector flagged the second rail bundle. Says there's sulfur inclusion.\" \"Everything from the yard has sulfur. The coal was unwashed.\" \"He says it won't weld for truss beams.\" \"It'll weld fine for fence stakes. Stack it o…`
- Row 037 `dlg_d562_garrison_fuel_drum_tare`: `{"body":"\"Drum's two gallons light on the dipstick.\" \"Sludge at the bottom takes up volume. Sits heavy as lard.\" \"Sludge doesn't ignite in the injector, does it?\" \"It ignites if you heat the lines first.\" \"You want me to build a c…`
- Row 038 `dlg_d566_independent_blanket_tally`: `{"body":"\"Four wool blankets for six cots.\" \"Double up the children. They generate more heat than the stove does anyway.\" \"Mother's coughing into a rag.\" \"Give her the canvas tarp over the blanket. Keeps the damp out. If we open ano…`
- Row 039 `dlg_d574_civilian_kettle_scouring_mutter`: `{"body":"\"Left flue first. Always the left flue, then the damper, then the ash drawer... No, three turns on the damper, Arthur said... Three turns and the smoke stays out of the larder. If you don't clean the throat, the frost gets in the…`
- Row 040 `dlg_d576_understory_copper_splice_tale`: `{"body":"\"Always three wraps on the standing wire, then solder, then pitch.\" \"Old man at the terminal just twists them and knots the cord.\" \"Old man at the terminal has never climbed a hundred meters in a gale to find a loose joint bu…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/faction_war_events.json`

### `Assets/StreamingAssets/Data/faction_war_events.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 93855; characters: 93786.
- SHA-256: `3ec09e02415a45ee4015120041651756e2cf7ada84fd70701abd5e289ef6e455`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `chains`

#### `chains` — 38 current rows

- Row 001 `row-1`: `{"band":"cold_war","chainId":"evt_d480_grain_tally_dispute","factionsInvolved":["faction_central_garrison","faction_rebuilders"],"locationId":"loc_grain_silo","stages":[{"bodyText":"A Garrison quartermaster's runner sets a sack on the Exch…`
- Row 002 `row-2`: `{"band":"cold_war","chainId":"evt_d485_checkpoint_notice_war","factionsInvolved":["faction_central_garrison","faction_rebuilders"],"locationId":"loc_garrison_checkpoint_gamma","stages":[{"bodyText":"A fresh Garrison bulletin blames the sea…`
- Row 003 `row-3`: `{"band":"cold_war","chainId":"evt_d488_manifest_holdup","factionsInvolved":["faction_central_garrison","faction_rebuilders"],"locationId":"loc_garrison_checkpoint_gamma","stages":[{"bodyText":"A Rebuilders grain cart bound for the Exchange…`
- Row 004 `row-4`: `{"band":"cold_war","chainId":"evt_d491_toll_hike","factionsInvolved":["warlords_sector_4"],"locationId":"loc_weighbridge","stages":[{"bodyText":"The chalk board by the Weighbridge booth shows the toll rate struck through and rewritten high…`
- Row 005 `row-5`: `{"band":"cold_war","chainId":"evt_d495_the_clean_strike","factionsInvolved":["faction_central_garrison","faction_rebuilders"],"locationId":"loc_railway_span_44_alpha","stages":[{"bodyText":"Something hits the empty ground short of Railway …`
- Row 006 `row-6`: `{"band":"open_conflict","chainId":"evt_d503_conscription_lists","factionsInvolved":["faction_central_garrison"],"locationId":"loc_conscription_office","stages":[{"bodyText":"The month's conscription list goes up with a wider quota than las…`
- Row 007 `row-7`: `{"band":"open_conflict","chainId":"evt_d509_border_clash_span44","factionsInvolved":["faction_central_garrison","faction_rebuilders"],"locationId":"loc_railway_span_44_alpha","stages":[{"bodyText":"A Garrison patrol and a Rebuilders escort…`
- Row 008 `row-8`: `{"band":"open_conflict","chainId":"evt_d517_almshouse_shelling","factionsInvolved":["faction_central_garrison","faction_rebuilders"],"locationId":"loc_st_brigids_almshouse","stages":[{"bodyText":"A scavenger passing through mentions, almos…`
- Row 009 `row-9`: `{"band":"open_conflict","chainId":"evt_d522_switchback_toll","factionsInvolved":["faction_central_garrison","faction_ash_sign"],"locationId":"loc_shrine_switchback_waystation","stages":[{"bodyText":"A Garrison squad has planted itself at t…`
- Row 010 `row-10`: `{"band":"open_conflict","chainId":"evt_d524_market_price_spike","factionsInvolved":["faction_rebuilders"],"locationId":"loc_grain_silo","stages":[{"bodyText":"With a supply route disrupted by the almshouse strike, prices at the Exchange cl…`
- Row 011 `row-11`: `{"band":"the_offensive","chainId":"evt_d533_garrison_offensive_grain_silo","factionsInvolved":["faction_central_garrison","faction_rebuilders"],"locationId":"loc_grain_silo","stages":[{"bodyText":"Colonel Harven's office issues an order to…`
- Row 012 `row-12`: `{"band":"the_offensive","chainId":"evt_d541_evacuation_window_plaza","factionsInvolved":["faction_central_garrison","faction_rebuilders"],"locationId":"loc_ration_queue_plaza","stages":[{"bodyText":"A source you trust — a runner rattled by…`
- Row 013 `row-13`: `{"band":"the_offensive","chainId":"evt_d545_ration_plaza_strike","factionsInvolved":["faction_central_garrison","faction_rebuilders"],"locationId":"loc_ration_queue_plaza","stages":[{"bodyText":"The strike lands at 0600, exactly when the q…`
- Row 014 `row-14`: `{"band":"the_offensive","chainId":"evt_d552_rebuilders_fracture","factionsInvolved":["faction_rebuilders"],"locationId":"loc_grain_silo","stages":[{"bodyText":"In the plaza strike's shadow, the roster members who pushed hardest to arm the …`
- Row 015 `row-15`: `{"band":"the_offensive","chainId":"evt_d558_ln74_signal_intercept","factionsInvolved":["faction_central_garrison"],"locationId":"loc_railway_span_44_alpha","stages":[{"bodyText":"A radio scavenger camped near Railway Span 44-Alpha, listeni…`
- Row 016 `row-16`: `{"band":"culmination","chainId":"evt_d565_hydro_leverage_break","factionsInvolved":["faction_central_garrison","faction_rebuilders"],"locationId":"loc_terrace_pumphouse","stages":[{"bodyText":"Squeezed from both sides for priority access, …`
- Row 017 `row-17`: `{"band":"culmination","chainId":"evt_d570_forward_roster_first_action","factionsInvolved":["faction_forward_roster","faction_central_garrison","faction_rebuilders"],"locationId":"loc_forward_roster_camp","stages":[{"bodyText":"Twelve peopl…`
- Row 018 `row-18`: `{"band":"culmination","chainId":"evt_d578_shrine_strike_anomaly","factionsInvolved":["faction_ash_sign"],"locationId":"loc_ash_sign_shrine","stages":[{"bodyText":"The shrine-keepers tending the survey cairn report the mounted dosimeter's r…`
- Row 019 `row-19`: `{"band":"culmination","chainId":"evt_d583_d9_reassessment","factionsInvolved":["faction_black_ops"],"locationId":"loc_d9_cache_bunker_delta","stages":[{"bodyText":"The Ninth Denial Detachment's standing order has held for three years witho…`
- Row 020 `row-20`: `{"band":"culmination","chainId":"evt_d588_ceasefire_by_exhaustion","factionsInvolved":["faction_central_garrison","faction_rebuilders"],"locationId":"loc_grain_silo","stages":[{"bodyText":"Bled thin by the offensive and rattled by a shrine…`
- Row 021 `row-21`: `{"band":"culmination","chainId":"evt_d600_theory_surfaces","factionsInvolved":["faction_central_garrison","faction_rebuilders","faction_ash_sign"],"locationId":"loc_railway_span_44_alpha","stages":[{"bodyText":"In different rooms, on the s…`
- Row 022 `row-22`: `{"band":"culmination","chainId":"evt_d605_post_ceasefire_forward_roster","factionsInvolved":["faction_forward_roster","faction_central_garrison","faction_rebuilders"],"locationId":"loc_forward_roster_camp","stages":[{"bodyText":"Five days …`
- Row 023 `row-23`: `{"band":"escalation","chainId":"evt_p25_marked_ruin","factionsInvolved":["faction_scavenger_guild","faction_central_garrison"],"locationId":"loc_grain_silo","stages":[{"bodyText":"Somebody has chalked a Garrison stamp across the Guild's do…`
- Row 024 `row-24`: `{"band":"escalation","chainId":"evt_p25_stopped_convoy","factionsInvolved":["faction_central_garrison","faction_rebuilders"],"locationId":"loc_garrison_checkpoint_gamma","stages":[{"bodyText":"A Barons' water convoy never made it past Chec…`
- Row 025 `row-25`: `{"band":"escalation","chainId":"evt_p25_bitter_water","factionsInvolved":["faction_hydro_barons","faction_rebuilders"],"locationId":"loc_terrace_pumphouse","stages":[{"bodyText":"Three families at the terrace settlement drank from the lowe…`
- Row 026 `row-26`: `{"band":"escalation","chainId":"evt_p25_empty_chair","factionsInvolved":["faction_central_garrison","faction_black_ops"],"locationId":"loc_d9_cache_bunker_delta","stages":[{"bodyText":"The Garrison called a district summit about the checkp…`
- Row 027 `row-27`: `{"band":"escalation","chainId":"evt_p25_cistern_toll_blockade","factionsInvolved":["faction_hydro_barons","faction_ash_sign"],"locationId":"loc_terrace_pumphouse","stages":[{"bodyText":"The Barons deny, in writing, that the terrace cistern…`
- Row 028 `row-28`: `{"band":"escalation","chainId":"evt_p25_prisoner_at_the_gate","factionsInvolved":["faction_central_garrison"],"locationId":"loc_iron_raiders_den","stages":[{"bodyText":"The Toll took a Garrison courier at the crossing the shelter fought ov…`
- Row 029 `row-29`: `{"band":"war_context","chainId":"evt_p25_refugees_from_the_line","factionsInvolved":["faction_central_garrison","faction_rebuilders"],"locationId":"loc_st_brigids_almshouse","stages":[{"bodyText":"They come up the almshouse road in family …`
- Row 030 `row-30`: `{"band":"war_context","chainId":"evt_p25_requisition","factionsInvolved":["faction_central_garrison"],"locationId":"loc_garrison_checkpoint_gamma","stages":[{"bodyText":"The requisition notice is polite the way a summons is polite: the she…`
- Row 031 `row-31`: `{"band":"war_context","chainId":"evt_p25_broken_route","factionsInvolved":["faction_rebuilders","faction_forward_roster"],"locationId":"loc_shrine_switchback_waystation","stages":[{"bodyText":"The switchback waystation is a crater with a r…`
- Row 032 `row-32`: `{"band":"war_context","chainId":"evt_p25_field_hospital_overflow","factionsInvolved":["faction_central_garrison","faction_rebuilders"],"locationId":"loc_st_brigids_almshouse","stages":[{"bodyText":"The almshouse has run out of almshouse. C…`
- Row 033 `row-33`: `{"band":"war_context","chainId":"evt_p25_deserter_column","factionsInvolved":["faction_central_garrison","faction_rebuilders"],"locationId":"loc_denial_cut_substation","stages":[{"bodyText":"They come down the denial cut in loose file, no …`
- Row 034 `row-34`: `{"band":"war_context","chainId":"evt_p25_retaliation","factionsInvolved":["faction_rebuilders","faction_ash_sign"],"locationId":"loc_grain_silo","stages":[{"bodyText":"The Rebuilders' fracture did not stay political. A crew that chose the …`
- Row 035 `row-35`: `{"band":"weariness","chainId":"evt_p25_no_more_volunteers","factionsInvolved":["faction_central_garrison"],"locationId":"loc_conscription_office","stages":[{"bodyText":"The conscription office has stopped pretending. The volunteer ledger, …`
- Row 036 `row-36`: `{"band":"weariness","chainId":"evt_p25_bread_before_bullets","factionsInvolved":["faction_rebuilders","faction_central_garrison"],"locationId":"loc_grain_silo","stages":[{"bodyText":"The grain queue outside the Exchange is the longest in d…`
- Row 037 `row-37`: `{"band":"weariness","chainId":"evt_p25_quiet_faction","factionsInvolved":["faction_rebuilders","faction_central_garrison"],"locationId":"loc_forward_roster_camp","stages":[{"bodyText":"It starts as a tea circle and becomes something with r…`
- Row 038 `row-38`: `{"band":"weariness","chainId":"evt_p25_refusal_at_dawn","factionsInvolved":["faction_central_garrison"],"locationId":"loc_railway_span_44_alpha","stages":[{"bodyText":"At first light, at Span 44, the offensive's Fresh battalion does not fo…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/faction_war_journal.json`

### `Assets/StreamingAssets/Data/faction_war_journal.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 17412; characters: 17398.
- SHA-256: `9fa6c10d07cfcf89781584c5fdad148a92ad95a2c2f191add17e7245662b293c`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `entries`

#### `entries` — 26 current rows

- Row 001 `journal_d482_mira_queue_count`: `{"authorName":"Mira","body":"I counted the line today and it was two hundred and six people which is the most I ever counted. I chalked a star on my token because Bettine at the Exchange said stars are lucky and I want to see if it's true …`
- Row 002 `journal_d486_fennick_ledger_entry`: `{"authorName":"Barrow Fennick","body":"Owed, this week: two Garrison quartermasters, one Exchange weigher, and — new entry, underline it — the Tollman himself, who does not extend credit to anyone, which means I have found the one thing in…`
- Row 003 `journal_d490_fossey_bean_row`: `{"authorName":"Nadia Fossey","body":"Plot fourteen, second row, mine two years now. Spent the morning thinning carrots that didn't need it because my hands wanted work that wasn't listening to people arguing about toll rates up at the gate…`
- Row 004 `journal_d502_denner_the_list`: `{"authorName":"Denner","body":"I have typed four hundred names onto that list in a year and never once let myself read them as anything but ink. Tonight I read my own the way everyone else must read theirs, which is to say I read it eleven…`
- Row 005 `journal_d509_denner_gone_to_ground`: `{"authorName":"Denner","body":"Five nights out and the loudspeaker on the bridge still says remain in shelter to nobody, on a loop, and I have started finding it almost companionable, which probably means I've been out here too long alread…`
- Row 006 `journal_d518_mira_the_almshouse`: `{"authorName":"Mira","body":"Everyone at the line was quiet today because of the almshouse and I didn't understand why since nobody lived there, but my mother said quiet the way she says it when she wants me to stop asking. So I stopped as…`
- Row 007 `journal_d528_adaeze_the_coats`: `{"authorName":"Adaeze Okonkwo","body":"Four coats, heavier than they used to be, standing where the rope used to be enough on its own. I told the roster it changes nothing about how the scale is run, and I believe that, mostly. What I do n…`
- Row 008 `journal_d536_fennick_the_new_checkpoint`: `{"authorName":"Barrow Fennick","body":"There is now a man with a clipboard standing between the traders and the tables, inspecting goods that used to just get weighed. I have already worked out that if I fill his clipboard with the right n…`
- Row 009 `journal_d543_mira_the_star`: `{"authorName":"Mira","body":"A woman two spots ahead of us in line, the one who always let me look at her radio even though it doesn't work anymore, said she was short a token today and I gave her mine. The one with the star. I don't know …`
- Row 010 `journal_d546_mira_after`: `{"authorName":"Mira","body":"We were not there. I keep starting sentences with we were not there because I don't know how to get to the next word yet. The woman with the radio was there. I am not going to write her name because writing it …`
- Row 011 `journal_d555_adaeze_the_split`: `{"authorName":"Adaeze Okonkwo","body":"Twelve of them walked out of the evening meeting together and I did not stop them, because I have spent three years teaching this roster that nobody here answers to a boss, and I find I cannot suddenl…`
- Row 012 `journal_d560_selwyn_the_frequency`: `{"authorName":"Selwyn Task","body":"I ran signals for the Garrison for two years before I walked, and I know a dead system when I hear one, because I decommissioned three of them myself. LN-74 is not dead. I have the header written down in…`
- Row 013 `journal_d567_fennick_the_pumphouse`: `{"authorName":"Barrow Fennick","body":"Brokered water for debt forgiveness today, which is, on paper, the cleanest trade I have ever made. On paper. I keep thinking about the south slope going dry this season for people who never once aske…`
- Row 014 `journal_d572_forward_roster_recruit`: `{"authorName":"Sella Krenn","body":"First column through the checkpoint paid without an argument, which surprised everyone but Toma — not that Toma, a different one, we've got two now, it's a whole thing. I keep waiting to feel like a raid…`
- Row 015 `journal_d575_sella_the_toll_math`: `{"authorName":"Sella Krenn","body":"Three days on the toll line and I've started recognizing faces in the column before they're close enough to owe us anything, which Toma — ours, not the shrine one — says means I'm getting good at this. I…`
- Row 016 `journal_d580_toma_the_broken_pattern`: `{"authorName":"Toma Reyes","body":"I have worn this dosimeter for three years and told pilgrims, in the shrine's own words, that it does not warn, it measures. I stood in a fresh crater two days ago and understood for the first time that I…`
- Row 017 `journal_d584_d9_cell_leader`: `{"authorName":"D/9 Cell Three","body":"Manifest review, cache Delta. Standing denial order re-read against current activity table. Finding: designated asset (spur-road checkpoint, unaffiliated party, est. thirty personnel) does not meet hi…`
- Row 018 `journal_d592_vashti_the_scale_holds`: `{"authorName":"Oren Vashti","body":"Set the scale up same as every morning, and for the first time in longer than I want to count, nobody set a sidearm near the rope while they haggled. The Garrison's post is still up at the entrance, same…`
- Row 019 `journal_d595_mira_the_quiet`: `{"authorName":"Mira","body":"The line is shorter now and there's a new woman two spots ahead who lets me look at things in her bag, though nothing that doesn't work like the radio did. I started a new token. No star yet. My mother says I d…`
- Row 020 `journal_d598_denner_the_pause`: `{"authorName":"Denner","body":"Word reached the bridge that the two sides have stopped shooting at each other, more or less, for now. The loudspeaker has not gotten the news. It is still telling an empty grid to remain in shelter, on the s…`
- Row 021 `journal_d601_toma_after_the_theory`: `{"authorName":"Toma Reyes","body":"Three strangers came up the switchback in the same week, none of them pilgrims, all of them asking versions of the same question I no longer have a doctrinal answer for: who chose. I told each of them the…`
- Row 022 `journal_d606_mira_the_quiet_peace`: `{"authorName":"Mira","body":"My mother let me put a star on the new token today. She says it's allowed to mean something different this time, not lucky, just remembered. There's a man at the edge of the plaza now with a clipboard who isn't…`
- Row 023 `journal_warlord_toll_doctrine`: `{"authorName":"The Tollman","body":"The boom is up and the price is known, and that is the whole of the contract with the road: pay, pass, and nobody learns your name. I have kept that contract through two governors and a war that forgot t…`
- Row 024 `journal_warlord_consolidation_doctrine`: `{"authorName":"The Tollman","body":"Too many fires on the cut this season, and ground I do not hold is ground I do not have to defend, so I am holding less of it. The checkpoints stay; the ambition goes into a drawer with the maps. A colum…`
- Row 025 `journal_warlord_annexation_doctrine`: `{"authorName":"The Tollman","body":"The weighbridge answers to the Toll House now. It was the natural next step: the scale, then the road it serves, then the ground under both. They will call it a land grab, as if the ground could object. …`
- Row 026 `journal_warlord_withdrawal_doctrine`: `{"authorName":"The Tollman","body":"The lamps are out and the door is locked, and this is not surrender. It is arithmetic: the weather has a column of its own and it does not pay tolls. I will be here when the freeze lifts, if the freeze l…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/faction_war_radio.json`

### `Assets/StreamingAssets/Data/faction_war_radio.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 17501; characters: 17481.
- SHA-256: `ed5851965227c0feb5e5b373d25185984eb7cceba7f50829caf3b353fee1b6f7`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `broadcasts`

#### `broadcasts` — 33 current rows

- Row 001 `radio_d480_span44_automated_loop`: `{"dayTrigger":480,"frequency":"96.100 MHz","id":"radio_d480_span44_automated_loop","isEmergency":false,"message":"Station null. Target grid primed. Remain in shelter. Station null. Target grid primed. Remain in shelter.","signalStrength":"…`
- Row 002 `radio_d481_garrison_continuity_bulletin`: `{"dayTrigger":481,"frequency":"88.400 MHz","id":"radio_d481_garrison_continuity_bulletin","isEmergency":false,"message":"Bulletin 481-C. Reports of irregular weighing practices at unregulated exchange points are noted and under continued o…`
- Row 003 `radio_d484_exchange_roster_wire_rebuttal`: `{"dayTrigger":484,"frequency":"104.200 MHz","id":"radio_d484_exchange_roster_wire_rebuttal","isEmergency":false,"message":"To our neighbors on both sides of the rope: the scale at the Exchange has never once favored a uniform, and the ledg…`
- Row 004 `radio_d487_unsigned_supply_figures`: `{"dayTrigger":487,"frequency":"88.400 MHz","id":"radio_d487_unsigned_supply_figures","isEmergency":false,"message":"Corrected totals for Sector grain movement, week ending the 483rd day: eleven hundred kilos logged by the Continuity Office…`
- Row 005 `radio_d488_garrison_grain_rebuttal`: `{"dayTrigger":488,"frequency":"88.400 MHz","id":"radio_d488_garrison_grain_rebuttal","isEmergency":false,"message":"Bulletin 488-B. The Continuity Office rejects unsigned claims of discrepancy in grain tonnage circulated on unlicensed freq…`
- Row 006 `radio_d490_ash_sign_shrine_transmission`: `{"dayTrigger":490,"frequency":"142.850 MHz","id":"radio_d490_ash_sign_shrine_transmission","isEmergency":false,"message":"The ash keeps its own ledger. It does not answer to the Continuity Office and it does not answer to the roster. What …`
- Row 007 `radio_d493_toll_syndicate_rate_notice`: `{"dayTrigger":493,"frequency":"61.900 MHz","id":"radio_d493_toll_syndicate_rate_notice","isEmergency":false,"message":"Rate revision effective immediate. Crossing rate: two units standard goods or equivalent. No exceptions, no faction disc…`
- Row 008 `radio_d496_understory_clean_strike`: `{"dayTrigger":496,"frequency":"104.200 MHz","id":"radio_d496_understory_clean_strike","isEmergency":false,"message":"Something hit empty ground out past the rail span last night and neither uniform in this district will own it. Funny thing…`
- Row 009 `radio_d504_garrison_conscription_notice`: `{"dayTrigger":504,"frequency":"88.400 MHz","id":"radio_d504_garrison_conscription_notice","isEmergency":true,"message":"Notice 504-A. Labor quota for the coming quarter is revised upward per Continuity Decree Seven. All eligible residents …`
- Row 010 `radio_d507_exchange_roster_wire_conscription`: `{"dayTrigger":507,"frequency":"104.200 MHz","id":"radio_d507_exchange_roster_wire_conscription","isEmergency":false,"message":"Half our roster's stall-holders got quota notices this week, which means half our roster isn't at their stalls t…`
- Row 011 `radio_d510_understory_span44_standoff`: `{"dayTrigger":510,"frequency":"104.200 MHz","id":"radio_d510_understory_span44_standoff","isEmergency":false,"message":"Two patrols stood on the rail span yesterday, rifles slung, staring at each other over a rumor of a train that never ca…`
- Row 012 `radio_d516_ash_sign_warning`: `{"dayTrigger":516,"frequency":"142.850 MHz","id":"radio_d516_ash_sign_warning","isEmergency":true,"message":"The reading from the almshouse quarter has turned. We do not warn. We measure, and today the measure says: do not linger there. Ta…`
- Row 013 `radio_d518_garrison_almshouse_bulletin`: `{"dayTrigger":518,"frequency":"88.400 MHz","id":"radio_d518_garrison_almshouse_bulletin","isEmergency":false,"message":"The Continuity Office confirms an ordnance impact at the former almshouse site. No Garrison unit logged a fire mission …`
- Row 014 `radio_d522_ash_sign_reading_shift`: `{"dayTrigger":522,"frequency":"142.850 MHz","id":"radio_d522_ash_sign_reading_shift","isEmergency":true,"message":"The readings have not settled since the almshouse. We measure daily now instead of weekly, because daily is what the ground …`
- Row 015 `radio_d525_exchange_roster_wire_prices`: `{"dayTrigger":525,"frequency":"104.200 MHz","id":"radio_d525_exchange_roster_wire_prices","isEmergency":false,"message":"To the roster and everyone who trades with us: yes, we've put four people on the door with coats a little heavier than…`
- Row 016 `radio_d534_garrison_exchange_order`: `{"dayTrigger":534,"frequency":"88.400 MHz","id":"radio_d534_garrison_exchange_order","isEmergency":true,"message":"Order 534. In light of confirmed armed presence at the grain exchange, a Garrison inspection post is established at the entr…`
- Row 017 `radio_d542_understory_something_coming`: `{"dayTrigger":542,"frequency":"104.200 MHz","id":"radio_d542_understory_something_coming","isEmergency":true,"message":"We're not going to say where, because we don't know for certain, and we'd rather be wrong than wrong and specific. But …`
- Row 018 `radio_d546_garrison_plaza_communique`: `{"dayTrigger":546,"frequency":"88.400 MHz","id":"radio_d546_garrison_plaza_communique","isEmergency":true,"message":"The Continuity Office confirms the loss at Ration Plaza was the direct result of a Rebuilders convoy improperly routed thr…`
- Row 019 `radio_d547_rebuilders_plaza_communique`: `{"dayTrigger":547,"frequency":"104.200 MHz","id":"radio_d547_rebuilders_plaza_communique","isEmergency":true,"message":"No convoy of ours has moved through Ration Plaza in a year and every trader on our roster can tell you why: it was neve…`
- Row 020 `radio_d559_lima_november_burst`: `{"dayTrigger":559,"frequency":"96.100 MHz","id":"radio_d559_lima_november_burst","isEmergency":false,"message":"LN74 LN74 grid confirmed hold pattern extend three six repeat three six LN74 out.","signalStrength":"S1","source":"Unidentified…`
- Row 021 `radio_d566_toll_syndicate_quiet_line`: `{"dayTrigger":566,"frequency":"61.900 MHz","id":"radio_d566_toll_syndicate_quiet_line","isEmergency":false,"message":"To whoever's been asking about arrangements at the pumphouse: the Weighbridge doesn't discuss its clients, past, present,…`
- Row 022 `radio_d571_forward_roster_checkpoint_notice`: `{"dayTrigger":571,"frequency":"71.500 MHz","id":"radio_d571_forward_roster_checkpoint_notice","isEmergency":false,"message":"Spur road, west checkpoint, effective yesterday. Two units standard goods or equivalent, no faction discount, no e…`
- Row 023 `radio_d579_ash_sign_shrine_anomaly`: `{"dayTrigger":579,"frequency":"142.850 MHz","id":"radio_d579_ash_sign_shrine_anomaly","isEmergency":true,"message":"The shrine was struck yesterday. We will not pretend otherwise on this frequency, whatever gets said at the cairn itself. F…`
- Row 024 `radio_d585_understory_spur_road_notice`: `{"dayTrigger":585,"frequency":"104.200 MHz","id":"radio_d585_understory_spur_road_notice","isEmergency":false,"message":"Something's been moving crates out past the old rail spur at night, quiet, organized, gone by dawn. Not looters — loot…`
- Row 025 `radio_d589_garrison_ceasefire_notice`: `{"dayTrigger":589,"frequency":"88.400 MHz","id":"radio_d589_garrison_ceasefire_notice","isEmergency":false,"message":"The Continuity Office confirms a stand-down of offensive operations pending further notice. This is not a treaty and shou…`
- Row 026 `radio_d590_rebuilders_ceasefire_notice`: `{"dayTrigger":590,"frequency":"104.200 MHz","id":"radio_d590_rebuilders_ceasefire_notice","isEmergency":false,"message":"To everyone asking if it's really over: no. To everyone asking if it's quieter than it was: yes. We'll take quieter. T…`
- Row 027 `radio_d596_forward_roster_holding_position`: `{"dayTrigger":596,"frequency":"71.500 MHz","id":"radio_d596_forward_roster_holding_position","isEmergency":false,"message":"Spur road checkpoint's still up despite what the Garrison's calling a stand-down. Nobody's told us to stand down an…`
- Row 028 `radio_d600_understory_closing`: `{"dayTrigger":600,"frequency":"104.200 MHz","id":"radio_d600_understory_closing","isEmergency":false,"message":"Six hundred days, for whoever's counting, and we know some of you are, because you keep calling in to correct our arithmetic. H…`
- Row 029 `radio_d606_forward_roster_recognition_notice`: `{"dayTrigger":606,"frequency":"71.500 MHz","id":"radio_d606_forward_roster_recognition_notice","isEmergency":false,"message":"To the column that stood down at the spur road yesterday: appreciated. The checkpoint isn't going anywhere. We to…`
- Row 030 `radio_warlord_toll_standing`: `{"dayTrigger":210,"frequency":"94.200 MHz","id":"radio_warlord_toll_standing","isEmergency":false,"message":"This is the Toll House. The boom is up and the price is the price — same as last week, same as next week, higher if you make it hi…`
- Row 031 `radio_warlord_consolidation`: `{"dayTrigger":220,"frequency":"94.200 MHz","id":"radio_warlord_consolidation","isEmergency":false,"message":"Toll House relay. Nothing moving, nothing burning. We hold what we hold and we are not interested in what we do not. If you see sm…`
- Row 032 `radio_warlord_annexation`: `{"dayTrigger":230,"frequency":"94.200 MHz","id":"radio_warlord_annexation","isEmergency":false,"message":"Toll House relay. New ground, new checkpoints. The weighbridge answers to the Toll House now, and the convoy apron answers to the wei…`
- Row 033 `radio_warlord_withdrawal`: `{"dayTrigger":240,"frequency":"94.200 MHz","id":"radio_warlord_withdrawal","isEmergency":false,"message":"Toll House relay. The boom is down and the lamps are out, and the road is yours again — all of it, every frozen kilometre. Enjoy it w…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/faction_war_communiques.json`

### `Assets/StreamingAssets/Data/faction_war_communiques.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 41590; characters: 41494.
- SHA-256: `cafbdc4c94cfd2b28a63076b397b7d469b3cca06edcf35510dba11b026afbca1`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `communiques`

#### `communiques` — 40 current rows

- Row 001 `comm_d489_garrison_manifest_inspection`: `{"authorNote":"Accurate but incomplete — the count difference was real on paper, but the Office omits that the checkpoint's platform scale reads consistently heavy in the Office's favor, a calibration no outside party has ever been permitt…`
- Row 002 `comm_d490_rebuilders_two_scales`: `{"authorNote":"Honest and correct — the Exchange's count was right and the checkpoint platform does read heavy; the Rebuilders suspect an honest calibration drift rather than a policy, and have no proof of intent.","body":"The Continuity O…`
- Row 003 `comm_d497_garrison_clean_strike`: `{"body":"The Continuity Office states plainly: no Garrison fire mission was logged against that grid, on that night, or any night this quarter. Speculation attributing the impact to Garrison ordnance is factually unsupported and residents …`
- Row 004 `comm_d497_rebuilders_clean_strike`: `{"body":"To be clear, since apparently it needs saying: the Rebuilders have never fielded anything that could put a crater in open ground from range, and everyone who trades with us knows exactly what's in our stores. We're not claiming th…`
- Row 005 `comm_d498_ash_sign_clean_strike`: `{"body":"Where the ash lands and does not spread is a choice, and the shrine has taught this for years to congregations that mostly nodded politely. Perhaps now the district will hear it differently. We do not know whose hand made this cho…`
- Row 006 `comm_d505_garrison_labor_quota`: `{"authorNote":"Accurate as stated, misleading in emphasis — the quota arithmetic is real, but the revision was drafted to backfill desertion from the previous quarter, which the notice does not say and the Office does not intend to say.","…`
- Row 007 `comm_d507_rebuilders_quota_arithmetic`: `{"authorNote":"Accurate — the Exchange counted the notices and the carriers, and the arithmetic is exactly as stated; the Office never answered it, only posted the next notice.","body":"The Continuity Office calls its labor quota arithmeti…`
- Row 008 `comm_d513_garrison_span_readiness`: `{"authorNote":"False in framing — there was a genuine standoff; both patrols stood on the bridge for a full watch, visibly uncertain whether the other would fire, and the charges were re-armed on the Office's order the same day as an act o…`
- Row 009 `comm_d514_rebuilders_span_record`: `{"authorNote":"Accurate — the Exchange's carriers used the span for three years without incident, and the Rebuilders' account of what the re-arming costs is the true one: the bridge's shared neutrality is gone, whatever either patrol says.…`
- Row 010 `comm_d519_garrison_almshouse`: `{"authorNote":"False. Internal Garrison range logs (not player-accessible) show an artillery emplacement near Checkpoint Gamma test-fired that week; the almshouse round was an overshoot the Office chose to bury rather than admit.","body":"…`
- Row 011 `comm_d520_rebuilders_almshouse`: `{"authorNote":"True by accident — the Rebuilders' guess is correct, though they have no proof, only the same geometry any careful observer could work out.","body":"We'd love to know which insurgents the Continuity Office thinks were operat…`
- Row 012 `comm_d521_ash_sign_almshouse`: `{"authorNote":"True and mundane — the shrine's preachers are attentive to real tension patterns (troop movements, emplacement activity) and dress ordinary vigilance in doctrinal language; no supernatural foreknowledge is involved.","body":…`
- Row 013 `comm_d523_ash_sign_pilgrim_toll`: `{"authorNote":"Accurate — the shrine neither paid nor contested the toll; the advice to keep counts is ordinary practical vigilance dressed in doctrinal language, and it is why the pilgrims' toll tallies later matter to the district's memo…`
- Row 014 `comm_d526_garrison_waystation_fee`: `{"authorNote":"True as stated, incomplete in spirit — the fee was rescinded because the corporal at Gamma had exceeded his standing orders and the shrine's neutrality was worth more than the toll income; the Office declined to say so, sinc…`
- Row 015 `comm_d527_rebuilders_guard_detail`: `{"authorNote":"Accurate — the vote was genuinely close and the Exchange's own leadership recorded its dissent against it; the statement is the Exchange telling its own story before the Garrison's 'armed civilians' framing could define it."…`
- Row 016 `comm_d530_garrison_recorded_concern`: `{"authorNote":"False by intent — the 'concern' memo was authored to build the paper trail the inspection-post order later cites; the Office had already drafted the order for the exchange when this statement was released.","body":"The Conti…`
- Row 017 `comm_d537_garrison_exchange_checkpoint`: `{"body":"The inspection post at the grain exchange ensures fair and continuous access to food resources for all district residents, regardless of faction affiliation, and corrects the documented armed drift the exchange's own roster permit…`
- Row 018 `comm_d538_rebuilders_exchange_checkpoint`: `{"body":"There is a squad standing at our own entrance rope inspecting goods that used to move on the scale's word alone, and the Continuity Office wants us to call that fairness. We built this market from rubble crews and an honor rule th…`
- Row 019 `comm_d549_garrison_ration_plaza`: `{"authorNote":"False. No Rebuilders convoy moved that morning; the claim is fabricated to assign blame.","body":"Investigation confirms the impact coincided with an unauthorized Rebuilders supply movement improperly routed through a design…`
- Row 020 `comm_d550_rebuilders_ration_plaza`: `{"authorNote":"Also incomplete — the Rebuilders' guess (a missed strike on the inspection post) is a reasonable inference but wrong; the strike was precisely aimed at the plaza itself, not a near miss on anything Garrison.","body":"Every t…`
- Row 021 `comm_d552_ash_sign_ration_plaza`: `{"authorNote":"Closest to true in spirit, though the preachers don't yet know the strike shares an origin with the day-495 clean strike; they read the plaza's precision correctly without understanding its source.","body":"We will not prete…`
- Row 022 `comm_d556_rebuilders_fracture`: `{"authorNote":"Accurate and quietly wounded — the Exchange's account matches what the meeting survivors know, including the part where they decline to disown the twelve; the Exchange's own leadership records not sleeping that night.","body…`
- Row 023 `comm_d561_ash_sign_dead_channel`: `{"authorNote":"Honest uncertainty — the shrine has no radio and knows the burst only from pilgrim accounts; the statement deliberately neither claims nor denies a pattern because the shrine's own count of the reports had not settled. What …`
- Row 024 `comm_d568_rebuilders_pumphouse_questions`: `{"authorNote":"Accurate — the Exchange genuinely has no answers yet; the arrangement was brokered quietly and every request for its terms has been refused, which is why the statement asks instead of asserting.","body":"The Terrace Pumphous…`
- Row 025 `comm_d570_garrison_pumphouse_coordination`: `{"authorNote":"False by omission — the arrangement redirected terrace water toward garrison-linked accounts in exchange for forgiven debt; the slope's season without water is the payment, and 'seasonal' is doing more work in this statement…`
- Row 026 `comm_d573_forward_roster_checkpoint`: `{"body":"We are not the Garrison, whatever the coats we're wearing might suggest at a distance. We are not raiders, whatever the checkpoint might suggest to anyone who's had to pay it. We are twelve people who became thirty who watched a s…`
- Row 027 `comm_d575_forward_roster_origin`: `{"authorNote":"Accurate — the Roster's account of its origin matches what the meeting survivors know; the 'bill' framing is the Roster's own, and the twelve were the harder line of the old guard detail.","body":"People keep asking if we're…`
- Row 028 `comm_d576_forward_roster_passage_rules`: `{"authorNote":"Accurate — the Roster genuinely runs the post exactly as posted; the medical exemption and count-only search rules are real, and the comparison to the Weighbridge Toll is deliberate self-definition, not rivalry.","body":"Sin…`
- Row 029 `comm_d577_ash_sign_restless_numbers`: `{"authorNote":"True but misread in direction — the shrine read the restlessness as danger to the district's low paths, not to itself; the ground that was never supposed to be hit was struck the next day, and no reading anyone kept predicte…`
- Row 030 `comm_d581_garrison_shrine_strike`: `{"authorNote":"False and cynical — the Garrison's insinuation that the Ash Sign staged the strike for sympathy is baseless; the strike was real and shares the mystery actor's signature with the day-495 and day-545 strikes.","body":"The Con…`
- Row 031 `comm_d582_rebuilders_shrine_strike`: `{"authorNote":"Honest agnosticism — the Rebuilders genuinely don't know, and unlike their almshouse and plaza statements, make no incorrect factual claim here.","body":"We have no stake in whether the shrine was struck by chance, by someon…`
- Row 032 `comm_d583_ash_sign_shrine_strike`: `{"authorNote":"True and central to the mystery — this is the closest any faction voice comes to correctly intuiting that the sparing was never providence, though the Ash Sign still doesn't know it was a person (the mystery actor's control …`
- Row 033 `comm_d591_ash_sign_ceasefire_pause`: `{"body":"Pilgrims are returning to the cairn and asking what the pause means, and we owe them the same answer we have owed since the strike: we do not fully know. The dosimeter still reads. It no longer tells us who is spared and who is no…`
- Row 034 `comm_d592_garrison_standdown`: `{"authorNote":"Accurate — and the 'review of earlier impact assessments' clause is the Office quietly preparing a path to retract the ration plaza convoy claim without ever saying one was made.","body":"This office records, by formal state…`
- Row 035 `comm_d593_forward_roster_ceasefire_toll`: `{"body":"Whatever gets signed, or doesn't, under that silo roof isn't ours to weigh in on and we won't pretend otherwise. The spur road checkpoint went up before anyone called this a war worth pausing and it'll still be standing after some…`
- Row 036 `comm_d594_rebuilders_ceasefire_benchmark`: `{"authorNote":"Accurate — the Exchange means the benchmark; the post does not come down anywhere in the arc, and they never do take up the word.","body":"The Office has issued its stand-down, and the district is asking us whether it's peac…`
- Row 037 `comm_d599_forward_roster_crates`: `{"authorNote":"Accurate — the crates were the camp's winter build; the Roster is touchy about smuggling rumors precisely because their coats are ex-Garrison salvage and the comparison is one they cannot afford.","body":"The civilian relay …`
- Row 038 `comm_d602_ash_sign_the_question`: `{"authorNote":"Doctrinal humility, deliberately precise — the shrine's keeper has heard the theory that one hand directed both the sparing and the strikes, and neither confirms nor teaches it yet; 'hold the meaning loosely' is the closest …`
- Row 039 `comm_d607_garrison_forward_roster_recognition`: `{"body":"The Continuity Office is aware of an unlicensed toll operation at the western supply spur and notes, without endorsing, that its conduct to date has not required a punitive response. This should not be read as recognition, toleran…`
- Row 040 `comm_d608_forward_roster_non_recognition`: `{"authorNote":"Accurate — the Roster neither sought nor wanted recognition; 'the war is tired, not over' is the most honest reading of the ceasefire any faction has published, and they didn't need a bulletin to know it.","body":"The Contin…`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`

### `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs` — bounded current excerpt (447 of 451 lines)

- Size: 451 lines / 20575 bytes.
- SHA-256: `4dfa917553bf8239ff0ed5799cd97fb81954caa3f0f24d32137f4e220bc092ef`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004:
00005: namespace Ashfall.Core.YearOfAsh
00006: {
00007:     /// <summary>
00008:     /// Content catalog for the faction war narrative layer (Days 240-360).
00009:     /// Loads and indexes the five faction_war_* JSON files: event chains,
00010:     /// journal entries, radio broadcasts, dialogue snippets, and communiques.
00011:     ///
00012:     /// This is the content side of <see cref="FactionWarSystem"/> (which handles
00013:     /// simulation: standing, territory, tension). The catalog provides the
00014:     /// narrative surface — what the player reads, hears, and experiences as the
00015:     /// war escalates.
00016:     /// </summary>
00017:     public sealed class FactionWarContentCatalog
00018:     {
00019:         private readonly List<FactionWarEventChain> _eventChains = new List<FactionWarEventChain>();
00020:         private readonly List<FactionWarJournalEntry> _journalEntries = new List<FactionWarJournalEntry>();
00021:         private readonly List<FactionWarBroadcast> _broadcasts = new List<FactionWarBroadcast>();
00022:         private readonly List<FactionWarDialogueSnippet> _dialogueSnippets = new List<FactionWarDialogueSnippet>();
00023:         private readonly List<FactionWarCommunique> _communiques = new List<FactionWarCommunique>();
00024:         private readonly List<FactionWarLocationOverride> _locationOverrides = new List<FactionWarLocationOverride>();
00025:
00026:         public IReadOnlyList<FactionWarEventChain> EventChains => _eventChains;
00027:         public IReadOnlyList<FactionWarJournalEntry> JournalEntries => _journalEntries;
00028:         public IReadOnlyList<FactionWarBroadcast> Broadcasts => _broadcasts;
00029:         public IReadOnlyList<FactionWarDialogueSnippet> DialogueSnippets => _dialogueSnippets;
00030:         public IReadOnlyList<FactionWarCommunique> Communiques => _communiques;
00031:         public IReadOnlyList<FactionWarLocationOverride> LocationOverrides => _locationOverrides;
00032:
00033:         public int EventChainCount => _eventChains.Count;
00034:         public int JournalEntryCount => _journalEntries.Count;
00035:         public int BroadcastCount => _broadcasts.Count;
00036:         public int DialogueSnippetCount => _dialogueSnippets.Count;
00037:         public int CommuniqueCount => _communiques.Count;
00038:         public int LocationOverrideCount => _locationOverrides.Count;
00039:
00040:         public void AddEventChain(FactionWarEventChain chain) => _eventChains.Add(chain);
00041:         public void AddJournalEntry(FactionWarJournalEntry entry) => _journalEntries.Add(entry);
00042:         public void AddBroadcast(FactionWarBroadcast broadcast) => _broadcasts.Add(broadcast);
00043:         public void AddDialogueSnippet(FactionWarDialogueSnippet snippet) => _dialogueSnippets.Add(snippet);
00044:         public void AddCommunique(FactionWarCommunique communique) => _communiques.Add(communique);
00045:         public void AddLocationOverride(FactionWarLocationOverride entry) => _locationOverrides.Add(entry);
00046:
00047:         /// <summary>Returns event chains eligible on or before the given day.</summary>
00048:         public List<FactionWarEventChain> GetEligibleChains(int day)
00049:         {
00050:             var eligible = new List<FactionWarEventChain>();
00051:             for (int i = 0; i < _eventChains.Count; i++)
00052:             {
00053:                 var chain = _eventChains[i];
00054:                 if (chain?.stages == null) continue;
00055:                 for (int j = 0; j < chain.stages.Count; j++)
00056:                 {
00057:                     if (chain.stages[j] != null && chain.stages[j].minDay <= day)
00058:                     {
00059:                         eligible.Add(chain);
00060:                         break;
00061:                     }
00062:                 }
00063:             }
00064:             return eligible;
00065:         }
00066:
00067:         /// <summary>Returns journal entries for a specific day.</summary>
00068:         public List<FactionWarJournalEntry> GetJournalForDay(int day)
00069:         {
00070:             var result = new List<FactionWarJournalEntry>();
00071:             for (int i = 0; i < _journalEntries.Count; i++)
00072:             {
00073:                 if (_journalEntries[i] != null && _journalEntries[i].day == day)
00074:                     result.Add(_journalEntries[i]);
00075:             }
00076:             return result;
00077:         }
00078:
00079:         /// <summary>Returns broadcasts triggered on or before the given day.</summary>
00080:         public List<FactionWarBroadcast> GetBroadcastsForDay(int day)
00081:         {
00082:             var result = new List<FactionWarBroadcast>();
00083:             for (int i = 0; i < _broadcasts.Count; i++)
00084:             {
00085:                 if (_broadcasts[i] != null && _broadcasts[i].dayTrigger <= day)
00086:                     result.Add(_broadcasts[i]);
00087:             }
00088:             return result;
00089:         }
00090:
00091:         /// <summary>Returns dialogue snippets available at a location on or after minDay.</summary>
00092:         public List<FactionWarDialogueSnippet> GetDialogueForLocation(string locationId, int day)
00093:         {
00094:             var result = new List<FactionWarDialogueSnippet>();
00095:             for (int i = 0; i < _dialogueSnippets.Count; i++)
00096:             {
00097:                 var s = _dialogueSnippets[i];
00098:                 if (s != null && s.minDay <= day &&
00099:                     string.Equals(s.locationId, locationId, StringComparison.Ordinal))
00100:                     result.Add(s);
00101:             }
00102:             return result;
00103:         }
00104:
00105:         /// <summary>Returns communiques issued by a faction on or before the given day.</summary>
00106:         public List<FactionWarCommunique> GetCommuniquesForFaction(string factionId, int day)
00107:         {
00108:             var result = new List<FactionWarCommunique>();
00109:             for (int i = 0; i < _communiques.Count; i++)
00110:             {
00111:                 var c = _communiques[i];
00112:                 if (c != null && c.day <= day &&
00113:                     string.Equals(c.factionId, factionId, StringComparison.Ordinal))
00114:                     result.Add(c);
00115:             }
00116:             return result;
00117:         }
00118:
00119:         /// <summary>
00120:         /// Returns the single active location override for a locationId on the
00121:         /// given day, or null if none applies. If multiple overrides for the
00122:         /// same location are simultaneously active (authoring error — should
00123:         /// not occur in shipped data), the most recently-started one
00124:         /// (highest activeFromDay) wins, matching "the latest thing that
00125:         /// happened to this place is what's currently true."
00126:         /// </summary>
00127:         public FactionWarLocationOverride? GetActiveLocationOverride(string locationId, int day)
00128:         {
00129:             FactionWarLocationOverride? best = null;
00130:             for (int i = 0; i < _locationOverrides.Count; i++)
00131:             {
00132:                 var o = _locationOverrides[i];
00133:                 if (o == null) continue;
00134:                 if (!string.Equals(o.locationId, locationId, StringComparison.Ordinal)) continue;
00135:                 if (day < o.activeFromDay) continue;
00136:                 if (o.activeUntilDay > 0 && day > o.activeUntilDay) continue;
00137:                 if (best == null || o.activeFromDay > best.activeFromDay) best = o;
00138:             }
00139:             return best;
00140:         }
00144:
00145:     [Serializable]
00146:     public sealed class FactionWarEventChain
00147:     {
00148:         public string chainId = string.Empty;
00149:         public string band = string.Empty;
00150:         public string title = string.Empty;
00151:         public List<string> factionsInvolved = new List<string>();
00152:         public string locationId = string.Empty;
00153:         public List<FactionWarEventStage> stages = new List<FactionWarEventStage>();
00154:     }
00155:
00156:     [Serializable]
00157:     public sealed class FactionWarEventStage
00158:     {
00159:         public string stageId = string.Empty;
00160:         public int minDay;
00161:         public string triggerCondition = string.Empty;
00162:         public string title = string.Empty;
00163:         public string bodyText = string.Empty;
00164:         public List<FactionWarEventChoice> choices = new List<FactionWarEventChoice>();
00165:
00166:         // ── Plan 25 additive (empty on every pre-Plan-25 stage) ─────────
00167:         /// <summary>Stage stays unsurfaced until this campaign flag is set
00168:         /// (e.g. a Plan 25 grievance flag). Empty = no flag gate.</summary>
00169:         public string requiresFlag = string.Empty;
00170:
00171:         /// <summary>Produced into the runner's flag store when the stage resolves
00172:         /// (auto-advance or player choice). Empty = produces nothing.</summary>
00173:         public string producesFlag = string.Empty;
00174:     }
00175:
00176:     [Serializable]
00177:     public sealed class FactionWarEventChoice
00178:     {
00179:         public string choiceId = string.Empty;
00180:         public string text = string.Empty;
00181:         public int moraleDelta;
00182:         public string leadsToStageId = string.Empty;
00183:
00184:         // ── Plan 25 additive (empty/0 on every pre-Plan-25 choice) ──────
00185:         /// <summary>Choice is only offered while this flag is set. Empty = always offered.</summary>
00186:         public string requiresFlag = string.Empty;
00187:
00188:         /// <summary>Produced into the runner's flag store when the choice is taken.</summary>
00189:         public string producesFlag = string.Empty;
00190:
00191:         /// <summary>Standing adjustment routed to the host's FactionWarSystem
00192:         /// (via FactionWarChainRunner.StandingDeltaApplier). Empty faction = no-op.</summary>
00193:         public string standingFactionId = string.Empty;
00194:         public int standingDelta;
00195:     }
00196:
00197:     [Serializable]
00198:     public sealed class FactionWarJournalEntry
00199:     {
00200:         public string id = string.Empty;
00201:         public string authorName = string.Empty;
00202:         public int day;
00203:         public string locationId = string.Empty;
00204:         public string voice = string.Empty;
00205:         public string body = string.Empty;
00206:     }
00207:
00208:     [Serializable]
00209:     public sealed class FactionWarBroadcast
00210:     {
00211:         public string id = string.Empty;
00212:         public string frequency = string.Empty;
00213:         public int dayTrigger;
00214:         public string source = string.Empty;
00215:         public string message = string.Empty;
00216:         public string signalStrength = string.Empty;
00217:         public bool isEmergency;
00218:         public string audio_cue = string.Empty;
00219:     }
00220:
00221:     [Serializable]
00222:     public sealed class FactionWarDialogueSnippet
00223:     {
00224:         public string id = string.Empty;
00225:         public string locationId = string.Empty;
00226:         public int minDay;
00227:         public string speakerTag = string.Empty;
00228:         public string body = string.Empty;
00229:     }
00230:
00231:     [Serializable]
00232:     public sealed class FactionWarCommunique
00233:     {
00234:         public string id = string.Empty;
00235:         public string eventChainId = string.Empty;
00236:         public string factionId = string.Empty;
00237:         public int day;
00238:         public string title = string.Empty;
00239:         public string body = string.Empty;
00240:
00241:         /// <summary>Optional in-world-reliability annotation (true/false/partial), present on
00242:         /// ~half of authored communiques. Empty when the source JSON omits it.</summary>
00243:         public string authorNote = string.Empty;
00244:     }
00245:
00246:     /// <summary>
00247:     /// A location description override active for a bounded or open-ended
00248:     /// day window, layered over the base locations.json entry for display
00249:     /// purposes only (see NARRATIVE_NEEDS.md §3 — base mechanical fields
00250:     /// like dangerLevel/travelHours/baseRadsPerHour are never overridden
00251:     /// here). Three overrideType values: pre_strike (bounded window,
00252:     /// foreshadowing), post_strike (open-ended, permanent aftermath),
00253:     /// ambient_addendum (open-ended, minor flavor — authored today as a full
00255:     /// </summary>
00256:     [Serializable]
00257:     public sealed class FactionWarLocationOverride
00258:     {
00259:         public string id = string.Empty;
00260:         public string locationId = string.Empty;
00261:         public string overrideType = string.Empty;
00262:         public int activeFromDay;
00263:
00264:         /// <summary>0 (unset) means open-ended — present only on pre_strike
00265:         /// entries in the shipped data, per NARRATIVE_NEEDS.md's documented
00266:         /// convention.</summary>
00267:         public int activeUntilDay;
00268:         public string displayName = string.Empty;
00269:         public string description = string.Empty;
00270:     }
00271:
00272:     // ── Root DTOs for deserialization ─────────────────────────────────────
00273:
00274:     [Serializable]
00275:     public sealed class FactionWarEventChainRoot
00276:     {
00277:         public int schema_version;
00278:         public List<FactionWarEventChain> chains = new List<FactionWarEventChain>();
00279:     }
00280:
00281:     [Serializable]
00282:     public sealed class FactionWarJournalRoot
00283:     {
00284:         public int schema_version;
00285:         public List<FactionWarJournalEntry> entries = new List<FactionWarJournalEntry>();
00286:     }
00287:
00288:     [Serializable]
00289:     public sealed class FactionWarBroadcastRoot
00290:     {
00291:         public int schema_version;
00292:         public List<FactionWarBroadcast> broadcasts = new List<FactionWarBroadcast>();
00293:     }
00294:
00295:     [Serializable]
00296:     public sealed class FactionWarDialogueRoot
00297:     {
00298:         public int schema_version;
00299:         public List<FactionWarDialogueSnippet> snippets = new List<FactionWarDialogueSnippet>();
00300:     }
00301:
00302:     [Serializable]
00303:     public sealed class FactionWarCommuniqueRoot
00304:     {
00305:         public int schema_version;
00306:         public List<FactionWarCommunique> communiques = new List<FactionWarCommunique>();
00307:     }
00308:
00309:     [Serializable]
00310:     public sealed class FactionWarLocationOverrideRoot
00311:     {
00312:         public int schema_version;
00313:         public List<FactionWarLocationOverride> locationOverrides = new List<FactionWarLocationOverride>();
00314:     }
00315:
00316:     // ── Loader ───────────────────────────────────────────────────────────
00317:
00318:     /// <summary>
00319:     /// Loads all five faction_war_* JSON files into a <see cref="FactionWarContentCatalog"/>.
00320:     /// Tolerant of missing files (logs warning, continues); parse failures in one file
00321:     /// do not prevent loading the others.
00322:     /// </summary>
00323:     public sealed class FactionWarContentCatalogLoader
00324:     {
00325:         public const string EventsFile = "faction_war_events.json";
00326:         public const string JournalFile = "faction_war_journal.json";
00327:         public const string RadioFile = "faction_war_radio.json";
00328:         public const string DialogueFile = "faction_war_dialogue.json";
00329:         public const string CommuniquesFile = "faction_war_communiques.json";
00330:         public const string LocationOverridesFile = "faction_war_location_overrides.json";
00331:
00332:         private readonly IFileIO _files;
00333:         private readonly IJsonSerializer _json;
00334:         private readonly ILog _log;
00335:
00336:         public FactionWarContentCatalogLoader(IFileIO files, IJsonSerializer json, ILog? log = null)
00337:         {
00338:             _files = files ?? throw new ArgumentNullException(nameof(files));
00339:             _json = json ?? throw new ArgumentNullException(nameof(json));
00340:             _log = log ?? NullLog.Instance;
00341:         }
00342:
00343:         public FactionWarContentCatalog Load(string dataDirectory)
00344:         {
00345:             var catalog = new FactionWarContentCatalog();
00346:             if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
00347:             {
00348:                 _log.Warn("Faction war content directory missing: " + dataDirectory);
00349:                 return catalog;
00350:             }
00351:
00352:             LoadEventChains(_files.Combine(dataDirectory, EventsFile), catalog);
00353:             LoadJournalEntries(_files.Combine(dataDirectory, JournalFile), catalog);
00354:             LoadBroadcasts(_files.Combine(dataDirectory, RadioFile), catalog);
00355:             LoadDialogueSnippets(_files.Combine(dataDirectory, DialogueFile), catalog);
00356:             LoadCommuniques(_files.Combine(dataDirectory, CommuniquesFile), catalog);
00357:             LoadLocationOverrides(_files.Combine(dataDirectory, LocationOverridesFile), catalog);
00358:
00359:             _log.Info($"Faction war content loaded: {catalog.EventChainCount} chains, " +
00360:                       $"{catalog.JournalEntryCount} journal, {catalog.BroadcastCount} broadcasts, " +
00361:                       $"{catalog.DialogueSnippetCount} dialogue, {catalog.CommuniqueCount} communiques, " +
00362:                       $"{catalog.LocationOverrideCount} location overrides");
00363:
00364:             return catalog;
00365:         }
00366:
00367:         private void LoadEventChains(string path, FactionWarContentCatalog catalog)
00368:         {
00369:             if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
00370:             try
00371:             {
00372:                 var root = _json.Deserialize<FactionWarEventChainRoot>(_files.ReadAllText(path));
00373:                 if (root?.chains == null) return;
00374:                 for (int i = 0; i < root.chains.Count; i++)
00375:                     if (root.chains[i] != null && !string.IsNullOrEmpty(root.chains[i].chainId))
00376:                         catalog.AddEventChain(root.chains[i]);
00377:             }
00378:             catch (Exception ex) { _log.Warn("Parse failed " + path + ": " + ex.Message); }
00379:         }
00380:
00381:         private void LoadJournalEntries(string path, FactionWarContentCatalog catalog)
00382:         {
00383:             if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
00384:             try
00385:             {
00386:                 var root = _json.Deserialize<FactionWarJournalRoot>(_files.ReadAllText(path));
00387:                 if (root?.entries == null) return;
00388:                 for (int i = 0; i < root.entries.Count; i++)
00389:                     if (root.entries[i] != null && !string.IsNullOrEmpty(root.entries[i].id))
00390:                         catalog.AddJournalEntry(root.entries[i]);
00391:             }
00392:             catch (Exception ex) { _log.Warn("Parse failed " + path + ": " + ex.Message); }
00393:         }
00394:
00395:         private void LoadBroadcasts(string path, FactionWarContentCatalog catalog)
00396:         {
00397:             if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
00398:             try
00399:             {
00400:                 var root = _json.Deserialize<FactionWarBroadcastRoot>(_files.ReadAllText(path));
00401:                 if (root?.broadcasts == null) return;
00402:                 for (int i = 0; i < root.broadcasts.Count; i++)
00403:                     if (root.broadcasts[i] != null && !string.IsNullOrEmpty(root.broadcasts[i].id))
00404:                         catalog.AddBroadcast(root.broadcasts[i]);
00405:             }
00406:             catch (Exception ex) { _log.Warn("Parse failed " + path + ": " + ex.Message); }
00407:         }
00408:
00409:         private void LoadDialogueSnippets(string path, FactionWarContentCatalog catalog)
00410:         {
00411:             if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
00412:             try
00413:             {
00414:                 var root = _json.Deserialize<FactionWarDialogueRoot>(_files.ReadAllText(path));
00415:                 if (root?.snippets == null) return;
00416:                 for (int i = 0; i < root.snippets.Count; i++)
00417:                     if (root.snippets[i] != null && !string.IsNullOrEmpty(root.snippets[i].id))
00418:                         catalog.AddDialogueSnippet(root.snippets[i]);
00419:             }
00420:             catch (Exception ex) { _log.Warn("Parse failed " + path + ": " + ex.Message); }
00421:         }
00422:
00423:         private void LoadCommuniques(string path, FactionWarContentCatalog catalog)
00424:         {
00425:             if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
00426:             try
00427:             {
00428:                 var root = _json.Deserialize<FactionWarCommuniqueRoot>(_files.ReadAllText(path));
00429:                 if (root?.communiques == null) return;
00430:                 for (int i = 0; i < root.communiques.Count; i++)
00431:                     if (root.communiques[i] != null && !string.IsNullOrEmpty(root.communiques[i].id))
00432:                         catalog.AddCommunique(root.communiques[i]);
00433:             }
00434:             catch (Exception ex) { _log.Warn("Parse failed " + path + ": " + ex.Message); }
00435:         }
00436:
00437:         private void LoadLocationOverrides(string path, FactionWarContentCatalog catalog)
00438:         {
00439:             if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
00440:             try
00441:             {
00442:                 var root = _json.Deserialize<FactionWarLocationOverrideRoot>(_files.ReadAllText(path));
00443:                 if (root?.locationOverrides == null) return;
00444:                 for (int i = 0; i < root.locationOverrides.Count; i++)
00445:                     if (root.locationOverrides[i] != null && !string.IsNullOrEmpty(root.locationOverrides[i].id))
00446:                         catalog.AddLocationOverride(root.locationOverrides[i]);
00447:             }
00448:             catch (Exception ex) { _log.Warn("Parse failed " + path + ": " + ex.Message); }
00449:         }
00450:     }
00451: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs`

### `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs` — bounded current excerpt (555 of 603 lines)

- Size: 603 lines / 31953 bytes.
- SHA-256: `5294c96557d10b243115c35fd87fff34a5e8bfaf8ebc546b29723b04ebd2e7b6`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005:
00006: namespace Ashfall.Core.YearOfAsh
00007: {
00008:     /// <summary>
00009:     /// Boolean trigger-condition grammar for FactionWarEventStage.triggerCondition,
00010:     /// per NARRATIVE_NEEDS.md §2's explicit requirement ("Needs a real boolean
00011:     /// grammar ... before this can drive anything other than a human reading
00012:     /// the JSON"). The 45 stages in faction_war_events.json are authored prose
00013:     /// ("Fires automatically once the player has visited X", "Fires N days
00014:     /// after stage Y", etc.) that were deliberately kept close to what such a
00015:     /// grammar would need to express. This is a closed set of condition node
00016:     /// types evaluated by a runner, not a free-text parser — the source data
00017:     /// is static, authored JSON, not user input, so a real parser would be
00018:     /// over-engineering; each stage is annotated with its condition once,
00019:     /// here, in a lookup keyed by stageId.
00020:     /// </summary>
00021:     public abstract class FactionWarTrigger
00022:     {
00023:         public abstract bool IsSatisfied(FactionWarTriggerContext ctx);
00024:     }
00025:
00026:     /// <summary>Everything a trigger needs to evaluate itself, supplied by the runner.</summary>
00027:     public sealed class FactionWarTriggerContext
00028:     {
00029:         public int CurrentDay;
00030:         public Func<string, bool> IsChainResolved = _ => false;
00031:         public Func<string, bool> HasVisitedLocation = _ => false;
00032:         public Func<string, int> StageResolvedDay = _ => -1; // -1 = not yet resolved
00033:
00034:         /// <summary>Plan 25: campaign flag probe (runner-produced flags plus the
00035:         /// host's external flag store). Defaults to never-set.</summary>
00036:         public Func<string, bool> IsFlagSet = _ => false;
00037:     }
00038:
00039:     /// <summary>
00040:     /// Plan 25: fires while the named campaign flag is set. Escalation chains
00041:     /// gate their opening stages on grievance flags authored by peacetime
00042:     /// faction actions and earlier war events. Part of the closed trigger
00043:     /// grammar — still one explicit FactionWarTriggerTable entry per stage.
00044:     /// </summary>
00045:     public sealed class FlagTrigger : FactionWarTrigger
00046:     {
00047:         private readonly string _flagId;
00048:         public FlagTrigger(string flagId) => _flagId = flagId;
00049:         public override bool IsSatisfied(FactionWarTriggerContext ctx) => ctx.IsFlagSet(_flagId);
00050:     }
00051:
00052:     /// <summary>Fires once the player has visited the given location (any day).</summary>
00053:     public sealed class PlayerVisitedTrigger : FactionWarTrigger
00054:     {
00055:         private readonly string _locationId;
00056:         public PlayerVisitedTrigger(string locationId) => _locationId = locationId;
00057:         public override bool IsSatisfied(FactionWarTriggerContext ctx) => ctx.HasVisitedLocation(_locationId);
00058:     }
00059:
00060:     /// <summary>Fires once the named chain has fully resolved (its terminal stage reached).</summary>
00061:     public sealed class ChainResolvedTrigger : FactionWarTrigger
00062:     {
00063:         private readonly string _chainId;
00064:         public ChainResolvedTrigger(string chainId) => _chainId = chainId;
00065:         public override bool IsSatisfied(FactionWarTriggerContext ctx) => ctx.IsChainResolved(_chainId);
00066:     }
00067:
00068:     /// <summary>
00069:     /// Fires offsetDays after the earliest-resolving of the given source
00070:     /// stages resolves. A single source stage is the common linear-progression
00071:     /// case (pattern 2 in the design inventory); multiple sources cover the
00072:     /// one "regardless of which s2 variant the player reached" case (pattern
00073:     /// 7, evt_d545_ration_plaza_strike_s1) where any one of several fan-out
00074:     /// terminal stages starts the same countdown.
00075:     /// </summary>
00076:     public sealed class DayOffsetTrigger : FactionWarTrigger
00077:     {
00078:         private readonly string[] _fromStageIds;
00079:         private readonly int _offsetDays;
00080:
00081:         public DayOffsetTrigger(int offsetDays, params string[] fromStageIds)
00082:         {
00083:             _offsetDays = offsetDays;
00084:             _fromStageIds = fromStageIds ?? Array.Empty<string>();
00085:         }
00086:
00087:         public override bool IsSatisfied(FactionWarTriggerContext ctx)
00088:         {
00089:             int earliest = -1;
00090:             for (int i = 0; i < _fromStageIds.Length; i++)
00091:             {
00092:                 int resolvedDay = ctx.StageResolvedDay(_fromStageIds[i]);
00093:                 if (resolvedDay < 0) continue;
00094:                 if (earliest < 0 || resolvedDay < earliest) earliest = resolvedDay;
00095:             }
00096:             if (earliest < 0) return false;
00097:             return ctx.CurrentDay >= earliest + _offsetDays;
00098:         }
00099:     }
00100:
00101:     /// <summary>All sub-conditions must be satisfied. Covers the two authored
00102:     /// AND-of-two-conditions stages (chain-resolved+visited; chain-resolved+chain-resolved).</summary>
00103:     public sealed class AndTrigger : FactionWarTrigger
00104:     {
00105:         private readonly FactionWarTrigger[] _conditions;
00106:         public AndTrigger(params FactionWarTrigger[] conditions) => _conditions = conditions ?? Array.Empty<FactionWarTrigger>();
00107:
00108:         public override bool IsSatisfied(FactionWarTriggerContext ctx)
00109:         {
00110:             for (int i = 0; i < _conditions.Length; i++)
00111:                 if (!_conditions[i].IsSatisfied(ctx)) return false;
00112:             return true;
00113:         }
00114:     }
00115:
00116:     /// <summary>
00117:     /// Always-true trigger for a chain's very first stage when its own
00118:     /// minDay is the only real gate (the runner still enforces minDay
00119:     /// separately — this exists so every stage has SOME trigger object,
00120:     /// keeping the lookup total rather than partial).
00121:     /// </summary>
00122:     public sealed class AlwaysTrigger : FactionWarTrigger
00123:     {
00124:         public static readonly AlwaysTrigger Instance = new AlwaysTrigger();
00125:         public override bool IsSatisfied(FactionWarTriggerContext ctx) => true;
00126:     }
00127:
00128:     /// <summary>
00129:     /// Maps every one of the 45 authored triggerCondition prose strings in
00130:     /// faction_war_events.json to its real FactionWarTrigger, by stageId.
00131:     /// This is a content-shaped lookup (one entry per stage), not a parser —
00132:     /// per this file's class doc, the prose was authored to be translatable
00133:     /// 1:1 into these five node types, and this table IS that translation.
00134:     /// Extend when NEW stages are authored; do not attempt to auto-derive
00135:     /// this from the prose string at runtime.
00136:     /// </summary>
00137:     public static class FactionWarTriggerTable
00138:     {
00139:         public static readonly Dictionary<string, FactionWarTrigger> ByStageId = Build();
00140:
00141:         public static FactionWarTrigger For(string stageId) =>
00142:             ByStageId.TryGetValue(stageId, out var t) ? t : AlwaysTrigger.Instance;
00143:
00144:         private static Dictionary<string, FactionWarTrigger> Build()
00145:         {
00146:             var t = new Dictionary<string, FactionWarTrigger>(StringComparer.Ordinal);
00147:
00148:             // Pattern 1: chain-opening stages gated on visiting the chain's location.
00149:             t["evt_d480_grain_tally_dispute_s1"] = new PlayerVisitedTrigger("loc_grain_silo");
00150:             t["evt_d485_checkpoint_notice_war_s1"] = new PlayerVisitedTrigger("loc_garrison_checkpoint_gamma");
00152:             t["evt_d491_toll_hike_s1"] = new PlayerVisitedTrigger("loc_weighbridge");
00153:             t["evt_d495_the_clean_strike_s1"] = new PlayerVisitedTrigger("loc_railway_span_44_alpha");
00154:             t["evt_d503_conscription_lists_s1"] = new PlayerVisitedTrigger("loc_conscription_office");
00155:             t["evt_d517_almshouse_shelling_s1"] = new PlayerVisitedTrigger("loc_st_brigids_almshouse");
00156:             t["evt_d522_switchback_toll_s1"] = new PlayerVisitedTrigger("loc_shrine_switchback_waystation");
00157:             t["evt_d565_hydro_leverage_break_s1"] = new PlayerVisitedTrigger("loc_terrace_pumphouse");
00158:
00163:             t["evt_d491_toll_hike_s2"] = new DayOffsetTrigger(3, "evt_d491_toll_hike_s1");
00164:             t["evt_d495_the_clean_strike_s2"] = new DayOffsetTrigger(3, "evt_d495_the_clean_strike_s1");
00165:             t["evt_d503_conscription_lists_s2"] = new DayOffsetTrigger(5, "evt_d503_conscription_lists_s1");
00166:             t["evt_d509_border_clash_span44_s2"] = new DayOffsetTrigger(3, "evt_d509_border_clash_span44_s1");
00167:             t["evt_d517_almshouse_shelling_s2"] = new DayOffsetTrigger(2, "evt_d517_almshouse_shelling_s1");
00168:             t["evt_d517_almshouse_shelling_s3"] = new DayOffsetTrigger(2, "evt_d517_almshouse_shelling_s2");
00169:             t["evt_d522_switchback_toll_s2"] = new DayOffsetTrigger(3, "evt_d522_switchback_toll_s1");
00190:                 "evt_d541_evacuation_window_plaza_s2_silent");
00191:
00192:             // Pattern 3: plain cross-chain "once {chainId} has resolved" dependency.
00193:             t["evt_d509_border_clash_span44_s1"] = new ChainResolvedTrigger("evt_d495_the_clean_strike");
00194:             t["evt_d524_market_price_spike_s1"] = new ChainResolvedTrigger("evt_d517_almshouse_shelling");
00195:             t["evt_d533_garrison_offensive_grain_silo_s1"] = new ChainResolvedTrigger("evt_d524_market_price_spike");
00196:             t["evt_d541_evacuation_window_plaza_s1"] = new ChainResolvedTrigger("evt_d533_garrison_offensive_grain_silo");
00197:             t["evt_d552_rebuilders_fracture_s1"] = new ChainResolvedTrigger("evt_d545_ration_plaza_strike");
00198:             t["evt_d558_ln74_signal_intercept_s1"] = new ChainResolvedTrigger("evt_d509_border_clash_span44");
00199:             t["evt_d578_shrine_strike_anomaly_s1"] = new ChainResolvedTrigger("evt_d558_ln74_signal_intercept");
00200:             t["evt_d583_d9_reassessment_s1"] = new ChainResolvedTrigger("evt_d570_forward_roster_first_action");
00201:             t["evt_d588_ceasefire_by_exhaustion_s1"] = new ChainResolvedTrigger("evt_d578_shrine_strike_anomaly");
00202:             t["evt_d600_theory_surfaces_s1"] = new ChainResolvedTrigger("evt_d588_ceasefire_by_exhaustion");
00203:
00204:             // Pattern 4: chain-resolved AND player-visited.
00205:             t["evt_d570_forward_roster_first_action_s1"] = new AndTrigger(
00206:                 new ChainResolvedTrigger("evt_d552_rebuilders_fracture"),
00207:                 new PlayerVisitedTrigger("loc_forward_roster_camp"));
00208:
00209:             // Pattern 5: chain-resolved AND chain-resolved.
00210:             t["evt_d605_post_ceasefire_forward_roster_s1"] = new AndTrigger(
00211:                 new ChainResolvedTrigger("evt_d588_ceasefire_by_exhaustion"),
00212:                 new ChainResolvedTrigger("evt_d570_forward_roster_first_action"));
00213:
00214:             // ── Plan 25 escalation chains (band: escalation) ─────────────
00215:             // E-P1 The Marked Ruin: opens only while the shelter's disputed
00216:             // Guild salvage claim grievance is on the ledger; then a plain
00217:             // three-day narration tail.
00218:             t["evt_p25_marked_ruin_s1"] = new FlagTrigger("flag_grievance_scavenger_claim_disputed");
00219:             t["evt_p25_marked_ruin_s2"] = new DayOffsetTrigger(3, "evt_p25_marked_ruin_s1");
00220:
00221:             // ── Plan 25 escalation (E-P2..P6): grievance-gated openings ──
00222:             // E-P2 The Stopped Convoy: the shelter's defaulted Hydro toll.
00223:             t["evt_p25_stopped_convoy_s1"] = new FlagTrigger("flag_grievance_hydro_toll_defaulted");
00224:             // E-P3 Bitter Water: the shelter's refused emergency appeal.
00225:             t["evt_p25_bitter_water_s1"] = new FlagTrigger("flag_grievance_hydro_appeal_refused");
00226:             // E-P4 The Empty Chair: the broken raider parley in the den's memory.
00227:             t["evt_p25_empty_chair_s1"] = new FlagTrigger("flag_grievance_raider_parley_broken");
00228:             // E-P5 Cistern Toll: siding against the intake audit.
00229:             t["evt_p25_cistern_toll_blockade_s1"] = new FlagTrigger("flag_grievance_hydro_intake_disputed");
00230:             // E-P6 Prisoner at the Gate: the fought-over crossing, unsettled.
00231:             t["evt_p25_prisoner_at_the_gate_s1"] = new FlagTrigger("flag_grievance_raider_passage_fought");
00232:
00233:             // ── Plan 25 mid-war context (E-W1..W6): chained to real 06C battles ──
00234:             t["evt_p25_refugees_from_the_line_s1"] = new ChainResolvedTrigger("evt_d509_border_clash_span44");
00235:             t["evt_p25_requisition_s1"] = new ChainResolvedTrigger("evt_d503_conscription_lists");
00236:             t["evt_p25_broken_route_s1"] = new ChainResolvedTrigger("evt_d522_switchback_toll");
00237:             t["evt_p25_field_hospital_overflow_s1"] = new ChainResolvedTrigger("evt_d533_garrison_offensive_grain_silo");
00238:             t["evt_p25_deserter_column_s1"] = new ChainResolvedTrigger("evt_d545_ration_plaza_strike");
00239:             t["evt_p25_retaliation_s1"] = new ChainResolvedTrigger("evt_d552_rebuilders_fracture");
00240:
00241:             // ── Plan 25 war weariness (E-R1..R4): culmination pressure ───
00242:             t["evt_p25_no_more_volunteers_s1"] = new ChainResolvedTrigger("evt_d565_hydro_leverage_break");
00243:             t["evt_p25_bread_before_bullets_s1"] = new FlagTrigger("flag_war_refugees_arrived");
00244:             t["evt_p25_quiet_faction_s1"] = new ChainResolvedTrigger("evt_d578_shrine_strike_anomaly");
00245:             t["evt_p25_refusal_at_dawn_s1"] = new FlagTrigger("flag_peace_faction_forms");
00246:
00247:             return t;
00248:         }
00250:
00251:     /// <summary>Per-chain progress: which stage is current (empty = chain fully
00252:     /// resolved / chain not yet started is distinguished by absence from the
00253:     /// dictionary), and the day each stage actually resolved on (for
00254:     /// DayOffsetTrigger's fromStageId lookups and for "chain resolved" checks).</summary>
00255:     [Serializable]
00256:     public sealed class FactionWarChainProgress
00257:     {
00258:         public string chainId = string.Empty;
00259:         public string currentStageId = string.Empty;
00260:         public bool resolved;
00261:
00262:         /// <summary>stageId -> day it resolved on (a choice was made, or the
00263:         /// stage had no choices and simply advanced). Needed by DayOffsetTrigger
00264:         /// even after currentStageId has moved past that stage.</summary>
00265:         public List<StageResolution> stageResolutions = new List<StageResolution>();
00266:     }
00267:
00268:     [Serializable]
00269:     public sealed class StageResolution
00270:     {
00271:         public string stageId = string.Empty;
00272:         public int day;
00273:     }
00274:
00275:     [Serializable]
00276:     public sealed class FactionWarChainRunnerState
00277:     {
00278:         public string systemId = FactionWarChainRunner.SystemId;
00279:         public int schemaVersion = 1;
00280:         public List<FactionWarChainProgress> chains = new List<FactionWarChainProgress>();
00281:         public List<string> visitedLocations = new List<string>();
00282:         public int cumulativeMoraleDelta;
00283:
00284:         /// <summary>Plan 25 additive: flags produced by stage/choice producesFlag
00285:         /// fields. Old saves deserialize with null; the runner re-initializes.</summary>
00286:         public List<string> producedFlags = new List<string>();
00287:     }
00288:
00289:     /// <summary>
00290:     /// Advances every chain in a FactionWarContentCatalog day by day: tracks
00291:     /// which stage is current per chain, evaluates that stage's
00292:     /// FactionWarTrigger (via FactionWarTriggerTable), surfaces the stage to
00293:     /// the host when its trigger fires, and — once the host reports the
00294:     /// player's choice — applies moraleDelta and advances to leadsToStageId
00295:     /// (or marks the chain resolved when leadsToStageId is empty). Stages
00296:     /// with zero choices auto-advance the instant their trigger fires (no
00297:     /// player input required — matches the many "Fires N days after X" plain
00298:     /// narration-only stages in the authored data).
00299:     ///
00300:     /// Zero engine dependencies; deterministic (no randomness at all — pure
00301:     /// day/state advancement, matching the authored content's own lack of
00302:     /// dice rolls).
00303:     /// </summary>
00304:     public sealed class FactionWarChainRunner
00305:     {
00306:         public const string SystemId = "faction_war_chain_runner";
00307:
00308:         /// <summary>
00309:         /// Authored war-chain minDay values begin at 480. The playable Year of Ash
00310:         /// window is 180–360. Hosts pass campaign day through
00311:         /// <see cref="ToAuthoredDay"/> so chains surface in a live campaign.
00312:         /// </summary>
00313:         public const int AuthoredEpochStart = 480;
00314:         public const int PlayableEpochStart = 180;
00315:
00316:         public static int ToAuthoredDay(int playableDay)
00317:         {
00318:             return playableDay + (AuthoredEpochStart - PlayableEpochStart);
00319:         }
00320:
00321:         private readonly FactionWarContentCatalog _catalog;
00322:         private FactionWarChainRunnerState _state;
00323:
00324:         public event Action<FactionWarEventChain, FactionWarEventStage>? OnStageSurfaced;
00325:         public event Action<FactionWarEventChain, FactionWarEventStage, FactionWarEventChoice>? OnStageResolved;
00326:         public event Action<FactionWarEventChain>? OnChainResolved;
00327:
00328:         public FactionWarChainRunner(FactionWarContentCatalog catalog, FactionWarChainRunnerState? state = null)
00329:         {
00330:             _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
00331:             _state = state ?? new FactionWarChainRunnerState();
00332:             if (_state.chains == null) _state.chains = new List<FactionWarChainProgress>();
00333:             if (_state.visitedLocations == null) _state.visitedLocations = new List<string>();
00334:             if (_state.producedFlags == null) _state.producedFlags = new List<string>();
00335:         }
00336:
00337:         public FactionWarChainRunnerState State => _state;
00338:         public FactionWarContentCatalog Catalog => _catalog;
00339:         public int CumulativeMoraleDelta => _state.cumulativeMoraleDelta;
00340:
00341:         // ── Plan 25 injection points (host-owned effects, no Core coupling) ──
00342:
00343:         /// <summary>Optional probe into the host's campaign flag store, consulted
00344:         /// in addition to the runner's own produced flags (e.g. Plan 25 grievance
00345:         /// flags authored by the FactionActionBoard).</summary>
00346:         public Func<string, bool>? ExternalFlagProbe;
00347:
00348:         /// <summary>Optional sink for choice standing adjustments — the host binds
00349:         /// FactionWarSystem.ModifyStanding here. Core never touches war standing
00350:         /// on its own.</summary>
00351:         public Action<string, int>? StandingDeltaApplier;
00352:
00353:         /// <summary>Records that the player has visited a location — feeds
00354:         /// PlayerVisitedTrigger for every chain, present and future.</summary>
00355:         public void RecordLocationVisited(string locationId)
00356:         {
00357:             if (string.IsNullOrEmpty(locationId)) return;
00358:             if (!_state.visitedLocations.Contains(locationId))
00359:                 _state.visitedLocations.Add(locationId);
00360:         }
00361:
00362:         public bool HasVisited(string locationId) => _state.visitedLocations.Contains(locationId);
00363:
00364:         public bool IsChainResolved(string chainId)
00365:         {
00366:             var progress = FindProgress(chainId);
00367:             return progress != null && progress.resolved;
00368:         }
00369:
00370:         /// <summary>The stage currently awaiting the player's choice for a chain,
00371:         /// or null if the chain hasn't started, is resolved, or its current
00372:         /// stage's trigger hasn't fired yet for the given day.</summary>
00373:         public FactionWarEventStage? GetSurfacedStage(string chainId, int currentDay)
00374:         {
00375:             var chain = FindChain(chainId);
00376:             if (chain == null) return null;
00377:
00381:                 stageId = chain.stages.Count > 0 ? chain.stages[0].stageId : string.Empty;
00382:             if (string.IsNullOrEmpty(stageId)) return null;
00383:             if (progress != null && progress.resolved) return null;
00384:
00385:             var stage = chain.stages.FirstOrDefault(s => s.stageId == stageId);
00386:             if (stage == null) return null;
00387:             if (currentDay < stage.minDay) return null;
00388:
00389:             var trigger = FactionWarTriggerTable.For(stageId);
00390:             var ctx = BuildContext(currentDay);
00391:             if (!trigger.IsSatisfied(ctx)) return null;
00392:
00393:             // Plan 25: authored flag gate on the stage itself.
00394:             if (!string.IsNullOrEmpty(stage.requiresFlag) && !ctx.IsFlagSet(stage.requiresFlag))
00400:         /// <summary>
00401:         /// Plan 25: whether a choice may be offered for the given stage — a
00402:         /// choice carrying requiresFlag is hidden until that flag is set. Hosts
00403:         /// must not render (or speculatively resolve) unavailable choices.
00404:         /// </summary>
00405:         public bool IsChoiceAvailable(FactionWarEventStage stage, FactionWarEventChoice choice, int currentDay)
00406:         {
00407:             if (stage == null || choice == null) return false;
00408:             if (string.IsNullOrEmpty(choice.requiresFlag)) return true;
00409:             return IsFlagSet(choice.requiresFlag);
00411:
00412:         /// <summary>True when the flag was produced by this runner or reported by
00413:         /// the host's external probe.</summary>
00414:         public bool IsFlagSet(string flagId)
00415:         {
00416:             if (string.IsNullOrEmpty(flagId)) return false;
00417:             if (_state.producedFlags.Contains(flagId)) return true;
00418:             return ExternalFlagProbe?.Invoke(flagId) ?? false;
00419:         }
00420:
00421:         /// <summary>
00422:         /// Advances every registered chain for the given day: for each chain
00423:         /// whose current stage's trigger just became satisfied, surfaces it
00424:         /// (OnStageSurfaced) and, if that stage has zero choices, immediately
00425:         /// auto-resolves it (no player input needed) and advances. Call once
00426:         /// per simulated day, same cadence as YearOfAshHostSession.TickDay.
00427:         /// </summary>
00428:         public void TickDay(int currentDay)
00429:         {
00430:             foreach (var chain in _catalog.EventChains)
00431:             {
00432:                 if (chain?.stages == null || chain.stages.Count == 0) continue;
00433:                 if (IsChainResolved(chain.chainId)) continue;
00434:
00435:                 var stage = GetSurfacedStage(chain.chainId, currentDay);
00436:                 if (stage == null) continue;
00437:
00438:                 OnStageSurfaced?.Invoke(chain, stage);
00439:
00440:                 if (stage.choices == null || stage.choices.Count == 0)
00441:                 {
00442:                     // Zero-choice stages narrate and advance automatically —
00448:
00449:         /// <summary>
00450:         /// The host calls this once the player has picked a choice for the
00451:         /// currently-surfaced stage of the given chain. Applies moraleDelta
00452:         /// and advances to the choice's leadsToStageId (empty = chain
00453:         /// resolved). Throws if the chain/stage/choice don't match what is
00454:         /// actually surfaced — the host must not call this speculatively.
00455:         /// </summary>
00456:         public void ResolveChoice(string chainId, string stageId, string choiceId, int currentDay)
00457:         {
00458:             var chain = FindChain(chainId) ?? throw new ArgumentException($"Unknown chain '{chainId}'.", nameof(chainId));
00459:             var stage = chain.stages.FirstOrDefault(s => s.stageId == stageId)
00460:                 ?? throw new ArgumentException($"Unknown stage '{stageId}' in chain '{chainId}'.", nameof(stageId));
00462:                 ?? throw new ArgumentException($"Unknown choice '{choiceId}' in stage '{stageId}'.", nameof(choiceId));
00463:             if (!IsChoiceAvailable(stage, choice, currentDay))
00464:                 throw new InvalidOperationException(
00465:                     $"Choice '{choiceId}' in stage '{stageId}' is gated by flag '{choice.requiresFlag}', which is not set.");
00466:
00467:             var progress = FindProgress(chainId);
00468:             string surfacedStageId = progress?.currentStageId;
00470:             if (!string.Equals(surfacedStageId, stageId, StringComparison.Ordinal))
00471:             {
00472:                 throw new InvalidOperationException(
00473:                     $"Chain '{chainId}' is currently at stage '{surfacedStageId}', not '{stageId}'.");
00474:             }
00475:
00476:             AdvancePastStage(chain, stage, currentDay, choice);
00477:         }
00478:
00479:         private void AdvancePastStage(FactionWarEventChain chain, FactionWarEventStage stage, int currentDay, FactionWarEventChoice? choice)
00480:         {
00481:             var progress = FindOrCreateProgress(chain.chainId);
00482:             RecordStageResolution(progress, stage.stageId, currentDay);
00483:
00484:             // Plan 25: a resolving stage may author a flag regardless of how it resolves.
00485:             if (!string.IsNullOrEmpty(stage.producesFlag))
00486:                 ProduceFlag(stage.producesFlag);
00487:
00488:             if (choice != null)
00491:                 if (!string.IsNullOrEmpty(choice.producesFlag))
00492:                     ProduceFlag(choice.producesFlag);
00493:                 if (choice.standingDelta != 0 && !string.IsNullOrEmpty(choice.standingFactionId))
00494:                     StandingDeltaApplier?.Invoke(choice.standingFactionId, choice.standingDelta);
00495:                 OnStageResolved?.Invoke(chain, stage, choice);
00496:             }
00497:
00498:             string next = choice?.leadsToStageId ?? string.Empty;
00499:             if (string.IsNullOrEmpty(next))
00500:             {
00501:                 progress.resolved = true;
00502:                 progress.currentStageId = string.Empty;
00503:                 OnChainResolved?.Invoke(chain);
00504:             }
00505:             else
00506:             {
00507:                 progress.currentStageId = next;
00512:         {
00513:             if (string.IsNullOrEmpty(flagId)) return;
00514:             if (!_state.producedFlags.Contains(flagId))
00515:                 _state.producedFlags.Add(flagId);
00516:         }
00517:
00518:         private static void RecordStageResolution(FactionWarChainProgress progress, string stageId, int day)
00519:         {
00520:             var existing = progress.stageResolutions.FirstOrDefault(r => r.stageId == stageId);
00521:             if (existing != null) { existing.day = day; return; }
00522:             progress.stageResolutions.Add(new StageResolution { stageId = stageId, day = day });
00523:         }
00524:
00525:         private FactionWarTriggerContext BuildContext(int currentDay) => new FactionWarTriggerContext
00526:         {
00527:             CurrentDay = currentDay,
00528:             IsChainResolved = IsChainResolved,
00529:             HasVisitedLocation = HasVisited,
00530:             IsFlagSet = IsFlagSet,
00531:             StageResolvedDay = stageId =>
00532:             {
00533:                 foreach (var progress in _state.chains)
00534:                 {
00535:                     var r = progress.stageResolutions.FirstOrDefault(x => x.stageId == stageId);
00536:                     if (r != null) return r.day;
00537:                 }
00538:                 return -1;
00539:             }
00540:         };
00541:
00542:         private FactionWarEventChain? FindChain(string chainId) =>
00543:             _catalog.EventChains.FirstOrDefault(c => c.chainId == chainId);
00544:
00545:         private FactionWarChainProgress? FindProgress(string chainId) =>
00546:             _state.chains.FirstOrDefault(p => p.chainId == chainId);
00547:
00548:         private FactionWarChainProgress FindOrCreateProgress(string chainId)
00549:         {
00550:             var existing = FindProgress(chainId);
00551:             if (existing != null) return existing;
00552:             var created = new FactionWarChainProgress { chainId = chainId };
00553:             _state.chains.Add(created);
00554:             return created;
00555:         }
00556:
00557:         public FactionWarChainRunnerState CaptureState() => Clone(_state);
00558:
00559:         public void RestoreState(FactionWarChainRunnerState state)
00560:         {
00561:             if (state == null) return;
00562:             if (!string.Equals(state.systemId, SystemId, StringComparison.Ordinal))
00563:                 throw new ArgumentException($"State belongs to system '{state.systemId}', expected '{SystemId}'.", nameof(state));
00564:             if (state.schemaVersion > 1)
00565:                 throw new NotSupportedException($"Future FactionWarChainRunner save schema {state.schemaVersion}; supported schema is 1.");
00566:             _state = Clone(state);
00567:         }
00568:
00569:         private static FactionWarChainRunnerState Clone(FactionWarChainRunnerState source)
00570:         {
00571:             var copy = new FactionWarChainRunnerState
00572:             {
00573:                 systemId = source.systemId,
00574:                 schemaVersion = source.schemaVersion,
00575:                 cumulativeMoraleDelta = source.cumulativeMoraleDelta,
00576:                 visitedLocations = new List<string>(source.visitedLocations ?? new List<string>()),
00577:                 producedFlags = new List<string>(source.producedFlags ?? new List<string>()),
00578:                 chains = new List<FactionWarChainProgress>()
00579:             };
00580:             if (source.chains != null)
00581:             {
00582:                 foreach (var p in source.chains)
00583:                 {
00584:                     if (p == null) continue;
00585:                     var pCopy = new FactionWarChainProgress
00586:                     {
00587:                         chainId = p.chainId,
00588:                         currentStageId = p.currentStageId,
00589:                         resolved = p.resolved,
00590:                         stageResolutions = new List<StageResolution>()
00591:                     };
00592:                     if (p.stageResolutions != null)
00593:                     {
00594:                         foreach (var r in p.stageResolutions)
00595:                             if (r != null) pCopy.stageResolutions.Add(new StageResolution { stageId = r.stageId, day = r.day });
00596:                     }
00597:                     copy.chains.Add(pCopy);
00598:                 }
00599:             }
00600:             return copy;
00601:         }
00602:     }
00603: }
```


# Appendix — Current Source Detail: `src/YearOfAsh/YearOfAshHostSession.cs`

### `src/YearOfAsh/YearOfAshHostSession.cs` — complete current file

- Size: 345 lines / 16205 bytes.
- SHA-256: `ccefd687193231434f67fa00f2a465d6a8558f7d6c3c7748f94a35fb28e4b6d2`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Warlords;
00007: using Ashfall.Core.YearOfAsh;
00008:
00009: namespace AtomicWar.GodotApp.YearOfAsh
00010: {
00011:     /// <summary>
00012:     /// Godot host coordinator for Expansion 05: The Year of Ash (Days 180 to 360).
00013:     /// Manages the timeline system, door encounter evaluations, faction war state, branching questlines,
00014:     /// deep freeze thermodynamics, and thaw radon ventilation.
00015:     /// </summary>
00016:     public class YearOfAshHostSession
00017:     {
00018:         private readonly YearOfAshTimelineSystem _timeline;
00019:         private readonly DoorEncounterSystem _encounters;
00020:         private readonly FactionWarSystem _factionWar;
00021:         private readonly QuestlineSystem _quests;
00022:         private readonly YearOfAshDeepFreezeSystem _deepFreeze;
00023:         private readonly YearOfAshRadonSystem _radon;
00024:         private WarlordDoctrineSystem _warlord;
00025:         private FactionWarChainRunner _warRunner;
00026:         private readonly List<SurvivorOccupantSnapshot> _demoRoster;
00027:         private readonly SeededRng _warlordRng;
00028:
00029:         public YearOfAshTimelineSystem Timeline => _timeline;
00030:         public DoorEncounterSystem Encounters => _encounters;
00031:         public FactionWarSystem FactionWar => _factionWar;
00032:         public QuestlineSystem Quests => _quests;
00033:         public YearOfAshDeepFreezeSystem DeepFreeze => _deepFreeze;
00034:         public YearOfAshRadonSystem Radon => _radon;
00035:         public WarlordDoctrineSystem Warlord => _warlord;
00036:         public FactionWarChainRunner WarRunner => _warRunner;
00037:         public IReadOnlyList<SurvivorOccupantSnapshot> DemoRoster => _demoRoster;
00038:
00039:         public YearOfAshHostSession(
00040:             YearOfAshTimelineSystem timeline = null!,
00041:             DoorEncounterSystem encounters = null!,
00042:             FactionWarSystem factionWar = null!,
00043:             QuestlineSystem quests = null!,
00044:             YearOfAshDeepFreezeSystem deepFreeze = null!,
00045:             YearOfAshRadonSystem radon = null!,
00046:             WarlordDoctrineSystem warlord = null!,
00047:             FactionWarChainRunner warRunner = null!)
00048:         {
00049:             _timeline = timeline ?? new YearOfAshTimelineSystem();
00050:             _encounters = encounters ?? new DoorEncounterSystem();
00051:             _factionWar = factionWar ?? new FactionWarSystem();
00052:             _quests = quests ?? new QuestlineSystem();
00053:             _deepFreeze = deepFreeze ?? new YearOfAshDeepFreezeSystem();
00054:             _radon = radon ?? new YearOfAshRadonSystem();
00055:             _warlord = warlord ?? new WarlordDoctrineSystem();
00056:             _warRunner = warRunner ?? new FactionWarChainRunner(new FactionWarContentCatalog());
00057:             _warlordRng = new SeededRng(2026);
00058:             _demoRoster = CreateDefaultDemoRoster();
00059:             WireWarlordConsequences();
00060:             WireWarRunner();
00061:         }
00062:
00063:         private void WireWarRunner()
00064:         {
00065:             if (_warRunner == null) return;
00066:             _warRunner.StandingDeltaApplier = (factionId, delta) =>
00067:             {
00068:                 _factionWar.ModifyStanding(factionId, delta);
00069:             };
00070:         }
00071:
00072:         /// <summary>
00073:         /// Swaps in the catalog-bound warlord (loaded after construction) and
00074:         /// re-wires the consequence subscriptions onto it.
00075:         /// </summary>
00076:         public void BindWarlord(WarlordDoctrineSystem warlord)
00077:         {
00078:             _warlord = warlord ?? _warlord;
00079:             WireWarlordConsequences();
00080:         }
00081:
00082:         /// <summary>
00083:         /// Thin consequence wiring (host-owned, no rules): hostile warlord
00084:         /// actions and tribute short-payments move the canonical
00085:         /// FactionWarSystem standing for warlords_sector_4, which persists with
00086:         /// the factionWar envelope section.
00087:         /// </summary>
00088:         private void WireWarlordConsequences()
00089:         {
00090:             _warlord.OnActionExecuted += result =>
00091:             {
00092:                 if (result == null) return;
00093:                 bool hostile = result.Action == WarlordStrategicAction.Raid
00094:                     || result.Action == WarlordStrategicAction.Annex
00095:                     || result.Action == WarlordStrategicAction.Contest;
00096:                 if (!hostile) return;
00097:                 _factionWar.ModifyStanding(WarlordDoctrineSystem.CanonicalFactionId,
00098:                     result.Success ? 3 : -2);
00099:             };
00100:             _warlord.OnTributeSettled += (paidFull, _) =>
00101:             {
00102:                 if (!paidFull)
00103:                     _factionWar.ModifyStanding(WarlordDoctrineSystem.CanonicalFactionId, -2);
00104:             };
00105:         }
00106:
00107:         public static YearOfAshHostSession Create(string dataDir = "", bool loadExistingSave = true)
00108:         {
00109:             var session = new YearOfAshHostSession();
00110:             if (!string.IsNullOrEmpty(dataDir))
00111:             {
00112:                 var fileIO = AtomicWar.GodotApp.CatalogPath.CreateFileIOForDataDir(dataDir);
00113:                 var serializer = new SystemTextJsonSerializer();
00114:                 DoorEncounterCatalogLoader.LoadAndRegister(session.Encounters, dataDir, fileIO, serializer);
00115:                 YearOfAshCatalogLoader.LoadAndRegisterQuests(session.Quests, dataDir, fileIO, serializer);
00116:                 // Shelter campaign dynamic questlines (Plan 59) share the YoA
00117:                 // questline runtime; register after the expansion catalog so
00118:                 // dynamic_questlines.json is reachable from the same host.
00119:                 DynamicQuestlineCatalogLoader.LoadAndRegister(session.Quests, dataDir, fileIO, serializer);
00120:                 // ASHFALL: THE VERDICT (Expansion 08) questlines are NOT registered
00121:                 // here — Verdict is the sole owner of its quest progress, registered
00122:                 // and persisted via VerdictHostSession / VerdictSave (v3+). Older
00123:                 // Year-of-Ash-carried quest_verdict_* records are adopted into the
00124:                 // Verdict envelope on load (VerdictQuestMigration).
00125:                 // ASHFALL: THE DOSE (Expansion 07) questlines are NOT registered
00126:                 // here either — Dose owns its quest progress via
00127:                 // DoseLedgerHostSession / DoseLedgerSave (v2+). Older
00128:                 // Year-of-Ash-carried Dose quest records are adopted into the Dose
00129:                 // envelope on load (DoseQuestMigration).
00130:                 // Adaptive Warlord AI (proposed model): load + validate the doctrine
00131:                 // catalog, then bind the warlord to the warlords_sector_4 identity.
00132:                 var warlordCatalog = WarlordDoctrineCatalogLoader.Load(dataDir, fileIO, serializer);
00133:                 var validation = WarlordCatalogValidator.Validate(warlordCatalog, dataDir, fileIO);
00134:                 if (!validation.Clean)
00135:                 {
00136:                     var sb = new System.Text.StringBuilder("WarlordDoctrineCatalog validation failed:");
00137:                     for (int i = 0; i < validation.Errors.Count; i++)
00138:                         sb.Append("\n  ").Append(validation.Errors[i]);
00139:                     throw new InvalidOperationException(sb.ToString());
00140:                 }
00141:                 session._warlord = new WarlordDoctrineSystem(warlordCatalog, seedSalt: 2026);
00142:                 for (int i = 0; i < validation.AliasWarnings.Count; i++)
00143:                     GD.Print("[warlord] " + validation.AliasWarnings[i]);
00144:                 session.BindWarlord(session._warlord);
00145:
00146:                 // Load Faction War content catalog and bind runner
00147:                 var warCatalogLoader = new FactionWarContentCatalogLoader(fileIO, serializer, new GodotLog());
00148:                 var warCatalog = warCatalogLoader.Load(dataDir);
00149:                 session._warRunner = new FactionWarChainRunner(warCatalog);
00150:                 session.WireWarRunner();
00151:             }
00152:
00153:             var existingSave = loadExistingSave ? YearOfAshSaveStore.TryLoad() : null;
00154:             if (existingSave != null)
00155:             {
00156:                 session.RestoreSave(existingSave);
00157:             }
00158:             return session;
00159:         }
00160:
00161:         public void TickDay(int day)
00162:         {
00163:             _timeline.AdvanceDay(day);
00164:             _factionWar.SimulateDailyFriction(day);
00165:             _warRunner.TickDay(FactionWarChainRunner.ToAuthoredDay(day));
00166:             _deepFreeze.TickDailyThermal(day, _timeline.AmbientTemperatureCelsius);
00167:             _radon.TickDailyRadon(day, _timeline.AmbientTemperatureCelsius);
00168:             TickWarlord(day);
00169:         }
00170:
00171:         /// <summary>
00172:         /// One warlord operation tick per day (idempotent in Core). The world
00173:         /// view the warlord acts on is explicit and non-omniscient: the host
00174:         /// reports only what the warlord could plausibly learn (scouts on
00175:         /// adjacent chokepoints), plus environment/rival/player context.
00176:         /// </summary>
00177:         private void TickWarlord(int day)
00178:         {
00179:             // Scouts report the chokepoints within reach of warlord ground.
00180:             var catalog = _warlord.Catalog;
00181:             for (int i = 0; i < catalog.Territory.Count; i++)
00182:             {
00183:                 var node = catalog.Territory[i];
00184:                 if (node == null) continue;
00185:                 // Only nodes adjacent to warlord ground get observed — the
00186:                 // warlord is not omniscient.
00187:                 if (!IsAdjacentToWarlordGround(node.location_id) && !node.home) continue;
00188:                 _warlord.Observe(node.location_id, _warlord.TerritoryState(node.location_id), day, confidence: 1f);
00189:             }
00190:
00191:             float environmentHazard = Math.Clamp(
00192:                 (Math.Abs(_timeline.AmbientTemperatureCelsius) + _deepFreeze.State.intakeIceThicknessMm) / 60f, 0f, 1f);
00193:             float rivalPressure = Math.Clamp(_factionWar.WarTension / 100f, 0f, 1f);
00194:             int playerStanding = _factionWar.GetStanding(WarlordDoctrineSystem.CanonicalFactionId);
00195:             var context = new WarlordContext
00196:             {
00197:                 EnvironmentHazard = environmentHazard,
00198:                 RivalPressure = rivalPressure,
00199:                 PlayerStanding = playerStanding
00200:             };
00201:             _warlord.TickDaily(day, _warlordRng, context);
00202:         }
00203:
00204:         private bool IsAdjacentToWarlordGround(string locationId)
00205:         {
00206:             var neighbors = _warlord.Catalog.Neighbors(locationId);
00207:             for (int i = 0; i < neighbors.Count; i++)
00208:             {
00209:                 if (_warlord.TerritoryState(neighbors[i]) == WarlordTerritoryState.Controlled)
00210:                     return true;
00211:             }
00212:             return false;
00213:         }
00214:
00215:         public void RecordWarLocationVisited(string locationId)
00216:         {
00217:             _warRunner.RecordLocationVisited(locationId);
00218:         }
00219:
00220:         public void ResolveWarChoice(string chainId, string stageId, string choiceId, int currentDay)
00221:         {
00222:             _warRunner.ResolveChoice(chainId, stageId, choiceId, currentDay);
00223:         }
00224:
00225:         public string GetStatusSummary()
00226:         {
00227:             return $"[Year of Ash] Day {_timeline.CurrentDay} ({_timeline.CurrentPhase}) | " +
00228:                    $"Surface: {_timeline.AmbientTemperatureCelsius:F1}°C | " +
00229:                    $"Bunker: {_deepFreeze.IndoorTempCelsius:F1}°C | " +
00230:                    $"Radon: {_radon.IndoorRadonBqm3:F0} Bq/m³ | " +
00231:                    $"War Tension: {_factionWar.WarTension}/100 | " +
00232:                    $"Active Quests: {_quests.State.active.Count}";
00233:         }
00234:
00235:         /// <summary>The current collector's ask (base × escalation multiplier).</summary>
00236:         public int CurrentTributeAsk =>
00237:             Math.Max(1, (int)(_warlord.Catalog.Warlord.tribute_base_amount * _warlord.TributeMultiplier));
00238:
00239:         /// <summary>
00240:         /// Player settles the current tribute ask through Core (no rules here).
00241:         /// Returns false when the payment is refused/zeroed; nextAsk is always
00242:         /// the resulting ask for display.
00243:         /// </summary>
00244:         public bool SettleWarlordTribute(int amountPaid, int day, out int nextAsk)
00245:         {
00246:             if (amountPaid <= 0)
00247:             {
00248:                 _warlord.SettleTribute(0, day, out nextAsk);
00249:                 return false;
00250:             }
00251:             return _warlord.SettleTribute(amountPaid, day, out nextAsk);
00252:         }
00253:
00254:         /// <summary>Authored collector prose for the given outcome state (demand/paid/short/refused).</summary>
00255:         public string CollectorLine(string state, int day) => _warlord.Catalog.CollectorLine(state, day);
00256:
00257:         /// <summary>Player-facing warlord readout (doctrine, territory, tribute, ledger).</summary>
00258:         public string WarlordLine()
00259:         {
00260:             var w = _warlord;
00261:             var sb = new System.Text.StringBuilder();
00262:             sb.Append("Warlord: ").Append(w.DoctrineId).Append(" · supply ").Append(w.Supply)
00263:                 .Append('/').Append(w.SupplyNeed)
00264:                 .Append(" · ops ").Append(w.TotalOperations)
00265:                 .Append(" · tribute ×").Append(w.TributeMultiplier.ToString("0.##"));
00266:             var territory = w.State.territory;
00267:             if (territory != null)
00268:             {
00269:                 for (int i = 0; i < territory.Count; i++)
00270:                 {
00271:                     var rec = territory[i];
00272:                     if (rec == null) continue;
00273:                     sb.Append("\n  ").Append(rec.locationId).Append(": ")
00274:                         .Append(((WarlordTerritoryState)rec.state).ToString());
00275:                 }
00276:             }
00277:             return sb.ToString();
00278:         }
00279:
00280:         /// <summary>
00281:         /// Snapshots every system the session ticks. Deep-freeze, radon and questline
00282:         /// were ticked daily but left out of the envelope, so a reload handed the player
00283:         /// a fresh scrubber, a clear intake and no quest history at whatever day the
00284:         /// timeline restored to.
00285:         /// </summary>
00286:         public YearOfAshSave CaptureSave()
00287:         {
00288:             return YearOfAshSaveCodec.Capture(
00289:                 _timeline,
00290:                 _encounters,
00291:                 _factionWar,
00292:                 null!,
00293:                 _deepFreeze,
00294:                 _radon,
00295:                 _quests,
00296:                 _warlord,
00297:                 _warRunner);
00298:         }
00299:
00300:         public void RestoreSave(YearOfAshSave save)
00301:         {
00302:             if (save == null) return;
00303:             YearOfAshSaveCodec.Restore(
00304:                 save, _timeline, _encounters, _factionWar,
00305:                 _deepFreeze, _radon, _quests, _warlord,
00306:                 _warRunner);
00307:             // Verdict quest progress is owned by the Verdict envelope (v3+) and
00308:             // Dose quest progress by the Dose envelope (v2+). After the one-time
00309:             // adoption in their host sessions, strip any quest_verdict_* /
00310:             // Dose quest records a legacy save still carries so this envelope
00311:             // stops re-serializing them (one persisted owner per expansion).
00312:             Ashfall.Core.Verdict.VerdictQuestMigration.StripFromYearOfAsh(_quests.State);
00313:             Ashfall.Core.DoseQuestMigration.StripFromYearOfAsh(_quests.State);
00314:         }
00315:
00316:         private List<SurvivorOccupantSnapshot> CreateDefaultDemoRoster()
00317:         {
00318:             return new List<SurvivorOccupantSnapshot>
00319:             {
00320:                 // Ids come from the year_of_ash_survivors.json master list — never
00321:                 // invented locally (AGENTS.md id rule).
00322:                 new SurvivorOccupantSnapshot
00323:                 {
00324:                     survivorId = "survivor_dr_sarah_chen",
00325:                     name = "Dr. Sarah Chen (Trauma Surgeon)",
00326:                     moralBranch = "humanist",
00327:                     guiltLevel = 25,
00328:                     traits = new List<string> { "trait_medic", "trait_altruistic" },
00329:                     hasRespiratoryDegeneration = true,
00330:                     hasTraumaBondWithLeader = true
00331:                 },
00332:                 new SurvivorOccupantSnapshot
00333:                 {
00334:                     survivorId = "survivor_gunner_mikhail",
00335:                     name = "Gunner Mikhail (Heavy Artillery Loader)",
00336:                     moralBranch = "ruthless",
00337:                     guiltLevel = 10,
00338:                     traits = new List<string> { "trait_veteran", "trait_pragmatist" },
00339:                     hasChemicalDependency = false,
00340:                     hasTraumaBondWithLeader = false
00341:                 }
00342:             };
00343:         }
00344:     }
00345: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/FactionWarDialogueExpansionTests.cs`

### `Ashfall.Core.Tests/FactionWarDialogueExpansionTests.cs` — complete current file

- Size: 203 lines / 9075 bytes.
- SHA-256: `4830cf7a9e3d5eba535f2f4da8be8f01f8ee6df3e2a48e78e468f8a31d3fc839`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005: using Ashfall.Core;
00006: using Ashfall.Core.YearOfAsh;
00007: using Xunit;
00008:
00009: namespace Ashfall.Core.Tests
00010: {
00011:     public class FactionWarDialogueExpansionTests : CatalogTestBase
00012:     {
00013:         private static FactionWarContentCatalog LoadCatalog()
00014:         {
00015:             var files = new FileSystemIO();
00016:             var json = new SystemTextJsonSerializer();
00017:             var loader = new FactionWarContentCatalogLoader(files, json);
00018:             return loader.Load(DataDirectory);
00019:         }
00020:
00021:         [Fact]
00022:         public void Catalog_Loads_Exactly_40_Snippets()
00023:         {
00024:             var catalog = LoadCatalog();
00025:             Assert.Equal(40, catalog.DialogueSnippetCount);
00026:             Assert.Equal(40, catalog.DialogueSnippets.Count);
00027:         }
00028:
00029:         [Fact]
00030:         public void All_Dialogue_IDs_Are_Unique()
00031:         {
00032:             var catalog = LoadCatalog();
00033:             var ids = catalog.DialogueSnippets.Select(s => s.id).ToList();
00034:             var distinctIds = ids.Distinct(StringComparer.Ordinal).ToList();
00035:             Assert.Equal(ids.Count, distinctIds.Count);
00036:         }
00037:
00038:         [Fact]
00039:         public void All_Dialogue_IDs_Have_Dlg_Prefix()
00040:         {
00041:             var catalog = LoadCatalog();
00042:             foreach (var snippet in catalog.DialogueSnippets)
00043:             {
00044:                 Assert.True(snippet.id.StartsWith("dlg_"), $"Snippet {snippet.id} must start with dlg_ prefix");
00045:             }
00046:         }
00047:
00048:         [Fact]
00049:         public void All_18_Baseline_Snippets_Preserved_With_Original_Keys_And_Bodies()
00050:         {
00051:             var catalog = LoadCatalog();
00052:             var baselineIds = new[]
00053:             {
00054:                 "dlg_d482_checkpoint_quartermasters",
00055:                 "dlg_d483_exchange_lean_pool",
00056:                 "dlg_d488_understory_relay_move",
00057:                 "dlg_d490_switchback_pilgrims",
00058:                 "dlg_d493_weighbridge_toll_grumble",
00059:                 "dlg_d497_scavengers_clean_crater",
00060:                 "dlg_d505_conscription_office_clerks",
00061:                 "dlg_d512_weighbridge_reroute",
00062:                 "dlg_d526_exchange_roster_kid",
00063:                 "dlg_d538_checkpoint_awkward_small_talk",
00064:                 "dlg_d552_deserter_hunters",
00065:                 "dlg_d549_children_after_the_plaza",
00066:                 "dlg_d580_shrine_keepers_doubt",
00067:                 "dlg_d568_toll_syndicate_cynicism",
00068:                 "dlg_d571_forward_roster_checkpoint",
00069:                 "dlg_d573_forward_roster_identity",
00070:                 "dlg_d584_d9_cell_debate",
00071:                 "dlg_d591_switchback_waystation_doubt"
00072:             };
00073:
00074:             var byId = catalog.DialogueSnippets.ToDictionary(s => s.id, StringComparer.Ordinal);
00075:             foreach (var bId in baselineIds)
00076:             {
00077:                 Assert.True(byId.ContainsKey(bId), $"Baseline id {bId} must be preserved");
00078:                 Assert.False(string.IsNullOrWhiteSpace(byId[bId].body), $"Body for {bId} must be non-empty");
00079:             }
00080:         }
00081:
00082:         [Fact]
00083:         public void All_22_New_Snippets_Present()
00084:         {
00085:             var catalog = LoadCatalog();
00086:             var newIds = new[]
00087:             {
00088:                 // Garrison (5)
00089:                 "dlg_d486_garrison_crate_seal",
00090:                 "dlg_d494_garrison_boot_leather",
00091:                 "dlg_d516_garrison_kerosene_stove",
00092:                 "dlg_d542_garrison_sick_list_billet",
00093:                 "dlg_d562_garrison_fuel_drum_tare",
00094:                 // Exchange (4)
00095:                 "dlg_d485_exchange_wet_grain_scale",
00096:                 "dlg_d508_exchange_axle_grease_delay",
00097:                 "dlg_d530_exchange_stamped_chits",
00098:                 "dlg_d489_exchange_drum_bung_dispute",
00099:                 // Understory (4)
00100:                 "dlg_d492_understory_porcelain_insulator",
00101:                 "dlg_d518_understory_log_overrun",
00102:                 "dlg_d546_understory_smudged_pad_entry",
00103:                 "dlg_d576_understory_copper_splice_tale",
00104:                 // Independent (3)
00105:                 "dlg_d498_independent_chalk_boundary",
00106:                 "dlg_d534_independent_tripwire_slack",
00107:                 "dlg_d566_independent_blanket_tally",
00108:                 // Foundry (3)
00109:                 "dlg_d502_foundry_cracked_flask_sand",
00110:                 "dlg_d528_foundry_crucible_heat_window",
00111:                 "dlg_d556_foundry_slag_billet_reject",
00112:                 // Civilian (3)
00113:                 "dlg_d487_civilian_parsnip_stew_scrap",
00114:                 "dlg_d520_civilian_valve_handle_toy",
00115:                 "dlg_d574_civilian_kettle_scouring_mutter"
00116:             };
00117:
00118:             var byId = catalog.DialogueSnippets.ToDictionary(s => s.id, StringComparer.Ordinal);
00119:             foreach (var nId in newIds)
00120:             {
00121:                 Assert.True(byId.ContainsKey(nId), $"New snippet id {nId} must be present");
00122:                 Assert.False(string.IsNullOrWhiteSpace(byId[nId].body), $"Body for {nId} must be non-empty");
00123:                 Assert.False(string.IsNullOrWhiteSpace(byId[nId].speakerTag), $"Speaker tag for {nId} must be non-empty");
00124:             }
00125:         }
00126:
00127:         [Fact]
00128:         public void All_Snippets_Have_NonEmpty_Fields_And_Valid_MinDay()
00129:         {
00130:             var catalog = LoadCatalog();
00131:             foreach (var s in catalog.DialogueSnippets)
00132:             {
00133:                 Assert.False(string.IsNullOrWhiteSpace(s.id), "Snippet id must not be empty");
00134:                 Assert.False(string.IsNullOrWhiteSpace(s.locationId), $"locationId for {s.id} must not be empty");
00135:                 Assert.True(s.locationId.StartsWith("loc_"), $"locationId for {s.id} must start with loc_");
00136:                 Assert.False(string.IsNullOrWhiteSpace(s.speakerTag), $"speakerTag for {s.id} must not be empty");
00137:                 Assert.False(string.IsNullOrWhiteSpace(s.body), $"body for {s.id} must not be empty");
00138:                 Assert.True(s.minDay >= 480 && s.minDay <= 605, $"minDay {s.minDay} for {s.id} must be in Faction War range [480, 605]");
00139:             }
00140:         }
00141:
00142:         [Fact]
00143:         public void GetDialogueForLocation_Filters_Correctly_At_Day_Boundaries()
00144:         {
00145:             var catalog = LoadCatalog();
00146:             // Test with a new snippet: dlg_d502_foundry_cracked_flask_sand at loc_granite_arsenal_foundry, minDay 502
00147:             const string loc = "loc_granite_arsenal_foundry";
00148:             const int onsetDay = 502;
00149:
00150:             var before = catalog.GetDialogueForLocation(loc, onsetDay - 1);
00151:             Assert.DoesNotContain(before, s => s.id == "dlg_d502_foundry_cracked_flask_sand");
00152:
00153:             var at = catalog.GetDialogueForLocation(loc, onsetDay);
00154:             Assert.Contains(at, s => s.id == "dlg_d502_foundry_cracked_flask_sand");
00155:
00156:             var after = catalog.GetDialogueForLocation(loc, onsetDay + 50);
00157:             Assert.Contains(after, s => s.id == "dlg_d502_foundry_cracked_flask_sand");
00158:         }
00159:
00160:         [Fact]
00161:         public void GetDialogueForLocation_Wrong_Location_Returns_No_Snippets_For_That_Location()
00162:         {
00163:             var catalog = LoadCatalog();
00164:             var list = catalog.GetDialogueForLocation("loc_nonexistent_location_xyz", 600);
00165:             Assert.Empty(list);
00166:         }
00167:
00168:         [Fact]
00169:         public void Faction_Context_Distribution_Satisfied()
00170:         {
00171:             var catalog = LoadCatalog();
00172:             var baselineIds = new HashSet<string>
00173:             {
00174:                 "dlg_d482_checkpoint_quartermasters", "dlg_d483_exchange_lean_pool",
00175:                 "dlg_d488_understory_relay_move", "dlg_d490_switchback_pilgrims",
00176:                 "dlg_d493_weighbridge_toll_grumble", "dlg_d497_scavengers_clean_crater",
00177:                 "dlg_d505_conscription_office_clerks", "dlg_d512_weighbridge_reroute",
00178:                 "dlg_d526_exchange_roster_kid", "dlg_d538_checkpoint_awkward_small_talk",
00179:                 "dlg_d552_deserter_hunters", "dlg_d549_children_after_the_plaza",
00180:                 "dlg_d580_shrine_keepers_doubt", "dlg_d568_toll_syndicate_cynicism",
00181:                 "dlg_d571_forward_roster_checkpoint", "dlg_d573_forward_roster_identity",
00182:                 "dlg_d584_d9_cell_debate", "dlg_d591_switchback_waystation_doubt"
00183:             };
00184:
00185:             var newSnippets = catalog.DialogueSnippets.Where(s => !baselineIds.Contains(s.id)).ToList();
00186:             Assert.Equal(22, newSnippets.Count);
00187:
00188:             var newGarrison = newSnippets.Count(s => s.id.Contains("_garrison_"));
00189:             var newExchange = newSnippets.Count(s => s.id.Contains("_exchange_"));
00190:             var newUnderstory = newSnippets.Count(s => s.id.Contains("_understory_"));
00191:             var newIndependent = newSnippets.Count(s => s.id.Contains("_independent_"));
00192:             var newFoundry = newSnippets.Count(s => s.id.Contains("_foundry_"));
00193:             var newCivilian = newSnippets.Count(s => s.id.Contains("_civilian_"));
00194:
00195:             Assert.Equal(5, newGarrison);
00196:             Assert.Equal(4, newExchange);
00197:             Assert.Equal(4, newUnderstory);
00198:             Assert.Equal(3, newIndependent);
00199:             Assert.Equal(3, newFoundry);
00200:             Assert.Equal(3, newCivilian);
00201:         }
00202:     }
00203: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs`

### `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` — complete current file

- Size: 348 lines / 13947 bytes.
- SHA-256: `79467fd76488c2f4747cd21ba11833fdcad6d795a8e86f5a62db166941aa6e75`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.Factions;
00005: #pragma warning disable CS8618
00006:
00007: namespace Ashfall.Core.YearOfAsh
00008: {
00009:     [Serializable]
00010:     public class FactionStandingRecord
00011:     {
00012:         public string factionId = string.Empty;
00013:         public int standing = 0; // -100 (Blood Feud) to +100 (Allied)
00014:         public int territorialControlPercent = 20; // 0 to 100
00015:         public bool isHostile = false;
00016:         public bool isAllied = false;
00017:     }
00018:
00019:     /// <summary>
00020:     /// Plan 167: timed defense-readiness pressure against a target faction.
00021:     /// </summary>
00022:     [Serializable]
00023:     public class FactionDefenseReadinessPressure
00024:     {
00025:         public string sourceId = string.Empty;
00026:         public string factionId = string.Empty;
00027:         public float magnitude;
00028:         public int startDay;
00029:         public int endDay;
00030:     }
00031:
00032:     [Serializable]
00033:     public class FactionWarSystemState
00034:     {
00035:         public List<FactionStandingRecord> factions = new List<FactionStandingRecord>();
00036:         public int activeWarTension = 50; // 0 to 100
00037:         public string dominantFactionId = "faction_central_garrison";
00038:         public List<string> enactedDecrees = new List<string>();
00039:         public int totalArtilleryStrikesLogged = 0;
00040:         public bool isWarActive = false;
00041:         public List<FactionDefenseReadinessPressure> defenseReadinessPressures = new List<FactionDefenseReadinessPressure>();
00042:     }
00043:
00044:     /// <summary>
00045:     /// Engine-agnostic multi-faction geopolitical war simulation for Days 180 to 360.
00046:     /// Simulates territorial battles, supply-line cutoffs, and decree enforcement.
00047:     /// Zero engine dependencies; deterministic.
00048:     /// </summary>
00049:     public class FactionWarSystem
00050:     {
00051:         public const string SystemId = "faction_war_system";
00052:         /// <summary>Standing at or below this is hostile (isHostile). Credit and
00053:         /// other trust-gated offers must use this constant, never a copied -50.</summary>
00054:         public const int HostileStandingThreshold = -50;
00055:
00056:         private readonly FactionWarSystemState _state;
00057:
00058:         public FactionWarSystemState State => _state;
00059:         public int WarTension => _state.activeWarTension;
00060:         public string DominantFactionId => _state.dominantFactionId;
00061:         public bool IsAtWar => _state.isWarActive;
00062:
00063:         public void SetWarActive(bool active)
00064:         {
00065:             _state.isWarActive = active;
00066:         }
00067:
00068:         public event Action<string, int> OnFactionStandingChanged;
00069:         public event Action<string> OnDecreeEnacted;
00070:         public event Action<string, string> OnTerritorialClashOccurred;
00071:
00072:         public FactionWarSystem(FactionWarSystemState? state = null)
00073:         {
00074:             _state = state ?? new FactionWarSystemState();
00075:             if (_state.enactedDecrees == null) _state.enactedDecrees = new List<string>();
00076:             if (_state.factions == null) _state.factions = new List<FactionStandingRecord>();
00077:             EnsureDefaultFactions();
00078:         }
00079:
00080:         public int GetStanding(string factionId)
00081:         {
00082:             if (string.IsNullOrWhiteSpace(factionId)) return 0;
00083:             string canonical = FactionStandingIdResolver.ToSystemsId(factionId);
00084:             var record = _state.factions.Find(f => f.factionId == canonical || f.factionId == factionId);
00085:             return record != null ? record.standing : 0;
00086:         }
00087:
00088:         public void ModifyStanding(string factionId, int delta)
00089:         {
00090:             if (string.IsNullOrWhiteSpace(factionId)) return;
00091:             string canonical = FactionStandingIdResolver.ToSystemsId(factionId);
00092:             var record = _state.factions.Find(f => f.factionId == canonical || f.factionId == factionId);
00093:             if (record == null)
00094:             {
00095:                 record = new FactionStandingRecord { factionId = canonical, standing = 0 };
00096:                 _state.factions.Add(record);
00097:             }
00098:             else
00099:             {
00100:                 record.factionId = canonical;
00101:             }
00102:
00103:             record.standing += delta;
00104:             if (record.standing > 100) record.standing = 100;
00105:             if (record.standing < -100) record.standing = -100;
00106:
00107:             record.isHostile = record.standing <= -50;
00108:             record.isAllied = record.standing >= 50;
00109:
00110:             OnFactionStandingChanged?.Invoke(canonical, record.standing);
00111:         }
00112:
00113:         public void EnactDecree(string decreeId)
00114:         {
00115:             if (!_state.enactedDecrees.Contains(decreeId))
00116:             {
00117:                 _state.enactedDecrees.Add(decreeId);
00118:                 _state.activeWarTension = Math.Min(100, _state.activeWarTension + 15);
00119:                 OnDecreeEnacted?.Invoke(decreeId);
00120:             }
00121:         }
00122:
00123:         /// <summary>
00124:         /// Plan 167: apply timed defense-readiness pressure. Idempotent on
00125:         /// <paramref name="sourceId"/>. Queried via
00126:         /// <see cref="GetDefenseReadiness01"/>.
00127:         /// </summary>
00128:         public bool TryApplyDefenseReadinessPressure(
00129:             string factionId,
00130:             float magnitude,
00131:             int startDay,
00132:             int endDay,
00133:             string sourceId)
00134:         {
00135:             if (string.IsNullOrWhiteSpace(factionId) || string.IsNullOrWhiteSpace(sourceId))
00136:                 return false;
00137:             if (endDay <= startDay) return false;
00138:             magnitude = Math.Clamp(magnitude, 0.05f, 0.75f);
00139:             string canonical = FactionStandingIdResolver.ToSystemsId(factionId);
00140:
00141:             if (_state.defenseReadinessPressures == null)
00142:                 _state.defenseReadinessPressures = new List<FactionDefenseReadinessPressure>();
00143:             for (int i = 0; i < _state.defenseReadinessPressures.Count; i++)
00144:             {
00145:                 if (string.Equals(_state.defenseReadinessPressures[i].sourceId, sourceId, StringComparison.Ordinal))
00146:                     return false;
00147:             }
00148:
00149:             _state.defenseReadinessPressures.Add(new FactionDefenseReadinessPressure
00150:             {
00151:                 sourceId = sourceId,
00152:                 factionId = canonical,
00153:                 magnitude = magnitude,
00154:                 startDay = startDay,
00155:                 endDay = endDay
00156:             });
00157:             return true;
00158:         }
00159:
00160:         /// <summary>
00161:         /// Effective defense readiness in [0.2, 1.0]. Active pressures reduce
00162:         /// readiness by their magnitude for the target faction.
00163:         /// </summary>
00164:         public float GetDefenseReadiness01(string factionId, int day)
00165:         {
00166:             if (string.IsNullOrWhiteSpace(factionId)) return 1f;
00167:             string canonical = FactionStandingIdResolver.ToSystemsId(factionId);
00168:             float pressure = 0f;
00169:             if (_state.defenseReadinessPressures != null)
00170:             {
00171:                 for (int i = 0; i < _state.defenseReadinessPressures.Count; i++)
00172:                 {
00173:                     var p = _state.defenseReadinessPressures[i];
00174:                     if (p == null) continue;
00175:                     if (day < p.startDay || day >= p.endDay) continue;
00176:                     if (!string.Equals(p.factionId, canonical, StringComparison.Ordinal)
00177:                         && !string.Equals(p.factionId, factionId, StringComparison.Ordinal))
00178:                         continue;
00179:                     if (p.magnitude > pressure) pressure = p.magnitude;
00180:                 }
00181:             }
00182:             return Math.Clamp(1f - pressure, 0.2f, 1f);
00183:         }
00184:
00185:         public void SimulateDailyFriction(int day)
00186:         {
00187:             if (day <= 240) return; // Full war kicks off in Phase 5 (day 241+)
00188:
00189:             _state.activeWarTension = Math.Min(100, _state.activeWarTension + 1);
00190:
00191:             // Shifting territories deterministically based on day modulo
00192:             if (day % 15 == 0)
00193:             {
00194:                 var garrison = _state.factions.Find(f => f.factionId == "faction_central_garrison");
00195:                 var rebuilders = _state.factions.Find(f => f.factionId == "faction_rebuilders");
00196:
00197:                 if (garrison != null && rebuilders != null)
00198:                 {
00199:                     garrison.territorialControlPercent += 3;
00200:                     rebuilders.territorialControlPercent = Math.Max(5, rebuilders.territorialControlPercent - 3);
00201:                     _state.totalArtilleryStrikesLogged++;
00202:                     OnTerritorialClashOccurred?.Invoke("faction_central_garrison", "faction_rebuilders");
00203:                 }
00204:             }
00205:         }
00206:
00207:         private void EnsureDefaultFactions()
00208:         {
00209:             string[] defaultFactions = new[]
00210:             {
00211:                 "faction_central_garrison",
00212:                 "faction_rebuilders",
00213:                 "faction_black_ops",
00214:                 "faction_ash_sign",
00215:                 "faction_hydro_barons",
00216:                 "faction_forward_roster"
00217:             };
00218:
00219:             foreach (var fId in defaultFactions)
00220:             {
00221:                 if (!_state.factions.Exists(f => f.factionId == fId))
00222:                 {
00223:                     _state.factions.Add(new FactionStandingRecord
00224:                     {
00225:                         factionId = fId,
00226:                         standing = 0,
00227:                         territorialControlPercent = 20,
00228:                         isHostile = false,
00229:                         isAllied = false
00230:                     });
00231:                 }
00232:             }
00233:         }
00234:
00235:         public FactionWarSystemState CaptureState()
00236:         {
00237:             var copy = new FactionWarSystemState
00238:             {
00239:                 activeWarTension = _state.activeWarTension,
00240:                 dominantFactionId = _state.dominantFactionId,
00241:                 totalArtilleryStrikesLogged = _state.totalArtilleryStrikesLogged,
00242:                 isWarActive = _state.isWarActive,
00243:                 enactedDecrees = _state.enactedDecrees != null
00244:                     ? new List<string>(_state.enactedDecrees)
00245:                     : new List<string>(),
00246:                 factions = new List<FactionStandingRecord>(),
00247:                 defenseReadinessPressures = new List<FactionDefenseReadinessPressure>()
00248:             };
00249:
00250:             if (_state.factions != null)
00251:             {
00252:                 foreach (var f in _state.factions)
00253:                 {
00254:                     if (f == null) continue;
00255:                     copy.factions.Add(new FactionStandingRecord
00256:                     {
00257:                         factionId = f.factionId,
00258:                         standing = f.standing,
00259:                         territorialControlPercent = f.territorialControlPercent,
00260:                         isHostile = f.isHostile,
00261:                         isAllied = f.isAllied
00262:                     });
00263:                 }
00264:             }
00265:
00266:             if (_state.defenseReadinessPressures != null)
00267:             {
00268:                 foreach (var p in _state.defenseReadinessPressures)
00269:                 {
00270:                     if (p == null) continue;
00271:                     copy.defenseReadinessPressures.Add(new FactionDefenseReadinessPressure
00272:                     {
00273:                         sourceId = p.sourceId,
00274:                         factionId = p.factionId,
00275:                         magnitude = p.magnitude,
00276:                         startDay = p.startDay,
00277:                         endDay = p.endDay
00278:                     });
00279:                 }
00280:             }
00281:
00282:             return copy;
00283:         }
00284:
00285:         /// <summary>
00286:         /// Restores a captured faction-war snapshot into the live state, then
00287:         /// re-applies the default faction rows so a roster added after the save
00288:         /// was written still has a record. A null state is a no-op.
00289:         /// </summary>
00290:         public void RestoreState(FactionWarSystemState state)
00291:         {
00292:             if (state == null) return;
00293:             _state.activeWarTension = state.activeWarTension;
00294:             _state.dominantFactionId = state.dominantFactionId ?? "faction_central_garrison";
00295:             _state.totalArtilleryStrikesLogged = state.totalArtilleryStrikesLogged;
00296:             _state.isWarActive = state.isWarActive;
00297:             _state.enactedDecrees = state.enactedDecrees != null
00298:                 ? new List<string>(state.enactedDecrees)
00299:                 : new List<string>();
00300:
00301:             _state.factions.Clear();
00302:             if (state.factions != null)
00303:             {
00304:                 foreach (var f in state.factions)
00305:                 {
00306:                     if (f == null || string.IsNullOrEmpty(f.factionId)) continue;
00307:                     _state.factions.Add(new FactionStandingRecord
00308:                     {
00309:                         factionId = f.factionId,
00310:                         standing = f.standing,
00311:                         territorialControlPercent = f.territorialControlPercent,
00312:                         isHostile = f.isHostile,
00313:                         isAllied = f.isAllied
00314:                     });
00315:                 }
00316:             }
00317:
00318:             _state.defenseReadinessPressures = new List<FactionDefenseReadinessPressure>();
00319:             if (state.defenseReadinessPressures != null)
00320:             {
00321:                 foreach (var p in state.defenseReadinessPressures)
00322:                 {
00323:                     if (p == null || string.IsNullOrWhiteSpace(p.sourceId)) continue;
00324:                     _state.defenseReadinessPressures.Add(new FactionDefenseReadinessPressure
00325:                     {
00326:                         sourceId = p.sourceId,
00327:                         factionId = p.factionId,
00328:                         magnitude = p.magnitude,
00329:                         startDay = p.startDay,
00330:                         endDay = p.endDay
00331:                     });
00332:                 }
00333:             }
00334:             EnsureDefaultFactions();
00335:         }
00336:
00337:         /// <summary>
00338:         /// Plan 55 / Task 55A — apply the retention catalog to the enacted-decree
00339:         /// history. The decree ledger stays owned here.
00340:         /// </summary>
00341:         public int ApplyRetention(Records.RetentionPolicyCatalog? catalog)
00342:         {
00343:             if (catalog == null) return 0;
00344:             catalog.ApplyRetention("faction_war_decrees", _state.enactedDecrees, out int pruned);
00345:             return pruned;
00346:         }
00347:     }
00348: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **The subject is a living-world dialogue corpus whose loader/query contract is live but whose dedicated player route must be proven. The plan expands reference integrity, temporal gating, presentation truthfulness and stable ambient selection without inventing mechanics.**.

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
