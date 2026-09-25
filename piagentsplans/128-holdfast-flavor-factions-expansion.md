# Plan 128 — Holdfast Flavor Factions, Dispatch Voices and Transaction Boundaries

> **Rebuild status:** COMPLETE 8-FACTION FLAVOR LOOP — FACTION/ITEM AUTHORITY AND DISPATCH REACHABILITY AUDIT
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

- The historical file was a dict of three flavor entries. The current data has eight faction keys plus forty item keys, and the host dispatch log resolves a faction voice for first purchase, purchase, sale, stock and rejection events.
- The live route is `holdfast_flavor.json` → `HoldfastFlavorCatalog` → `HoldfastDispatchLog` event projections, while item/faction trade semantics remain in `HoldfastCatalog`, `HoldfastItemsCatalog`, `HoldfastFactionsCatalog` and `HoldfastTradeSession`.
- The valuable audit is identity/reference correctness, dispatch truthfulness, tone and transaction boundary: a flavor line may explain a transaction but cannot grant value, stock, trust or access.

**Bounded outcome:** Retire the old 3→8 pure-data brief as a new catalog project. The current `holdfast_flavor.json` has eight keyed factions and the current `HoldfastDispatchLog` consumes their voice/rejected/sold fields through `HoldfastFlavorCatalog`. Keep this flavor overlay distinct from the separate `holdfast_factions.json` action catalog and `HoldfastFactionsCatalog` trade model; do not create a second faction authority.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `holdfast_flavor.json` is valid JSON with eight `factions` entries and forty `items` entries; the current flavor entries contain voice/rejection/sale presentation fields.
- `HoldfastFlavorCatalog` is a host-side loader keyed by faction id; `HoldfastDispatchLog` calls `GetFactionVoice` and emits bounded presentation entries.
- `HoldfastCatalogLoader` separately loads `holdfast_factions.json` into `HoldfastFactionsCatalog`; the two files have different schemas and must not be merged by guesswork.
- Historical DEC-292 tests cover eight flavor factions and identity contracts; this plan does not assert a fresh runtime pass or a new faction trade authority.

**Master-authority sections applied to this rebase:**

- Part II Factory Protocol: premise sweep, collision check, one lane/cluster, and evidence labels before drafting.
- Part II Step 5 continuity and anti-duplication checklist: data presence is not reachability.
- Part III cluster map: use the live C1–C17 owner map rather than a historical plan title.
- Part IV backlog discipline: consume a verified candidate or record why it is stale; do not widen a bounded outcome.
- Part V Template S/R: subject intent and recommended route remain separate from implementation commitments.
- Part VI Multi-Session Growth Protocol: 250k is a depth target, not permission to manufacture volume.
- Live source/data authority: current catalog, loader, host, save, and focused tests outrank generated prose.
- Anti-padding rule: if the evidence queue is exhausted, stop and report no warranted continuation.
- C11/C17 cluster: Holdfast flavor is a bounded host presentation overlay over canonical trade state.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 3→8 data-only brief with an 8-faction flavor census and a separate `holdfast_factions.json` authority map.
- Verify every flavor faction ID referenced by a current dispatch event and every item/faction reference in the separate trade catalog.
- Audit dispatch event ordering, bounded log retention and truthful success/rejection copy.
- Preserve flavor as a presentation overlay with no independent trust, stock, price or quest state.

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
| host flavor loading and keyed voice lookup | HoldfastFlavorCatalog | `src/Host/HoldfastFlavorCatalog.cs` | Owns presentation flavor only. |
| bounded transaction event projection | HoldfastDispatchLog | `src/Host/HoldfastDispatchLog.cs` | Formats current dispatch facts; it does not settle trades. |
| canonical item/faction trade definitions | HoldfastCatalog/HoldfastFactionsCatalog | `Assets/Ashfall.Core/HoldfastCatalog.cs; Assets/Ashfall.Core/HoldfastFactionsCatalog.cs` | Owns trade-facing faction/item metadata. |
| mutable trade values, stock and transaction outcomes | HoldfastTradeSession | `Assets/Ashfall.Core/HoldfastTradeSession.cs` | Sole transaction authority; flavor cannot mutate it. |
| host composition and terminal route | HoldfastRuntimeSession/Main.Holdfast | `src/Host/HoldfastRuntimeSession.cs; src/Main.Holdfast.cs` | Thin host wiring and player route. |
| identity, dispatch and transaction proof | Holdfast flavor/trade tests | `Ashfall.Core.Tests/HoldfastFlavorExpansionTests.cs; Ashfall.Core.Tests/HoldfastFactionIdentityContractTests.cs; Ashfall.Core.Tests/Narrative/Plan117_128HoldfastQuestFlavorIntegrationTests.cs` | Focused evidence surface. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Holdfast Flavor Factions, Dispatch Voices and Transaction Boundaries
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ HoldfastFlavorCatalog
│   host flavor loading and keyed voice lookup
│ HoldfastDispatchLog
│   bounded transaction event projection
│ HoldfastCatalog/HoldfastFactionsCatalog
│   canonical item/faction trade definitions
│ HoldfastTradeSession
│   mutable trade values, stock and transaction outcomes
│ HoldfastRuntimeSession/Main.Holdfast
│   host composition and terminal route
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

1. **Preserve current state ownership.** HoldfastFlavorCatalog owns host flavor loading and keyed voice lookup: Owns presentation flavor only.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| host flavor loading and keyed voice lookup | HoldfastFlavorCatalog | `src/Host/HoldfastFlavorCatalog.cs` | Owns presentation flavor only. |
| bounded transaction event projection | HoldfastDispatchLog | `src/Host/HoldfastDispatchLog.cs` | Formats current dispatch facts; it does not settle trades. |
| canonical item/faction trade definitions | HoldfastCatalog/HoldfastFactionsCatalog | `Assets/Ashfall.Core/HoldfastCatalog.cs; Assets/Ashfall.Core/HoldfastFactionsCatalog.cs` | Owns trade-facing faction/item metadata. |
| mutable trade values, stock and transaction outcomes | HoldfastTradeSession | `Assets/Ashfall.Core/HoldfastTradeSession.cs` | Sole transaction authority; flavor cannot mutate it. |
| host composition and terminal route | HoldfastRuntimeSession/Main.Holdfast | `src/Host/HoldfastRuntimeSession.cs; src/Main.Holdfast.cs` | Thin host wiring and player route. |
| identity, dispatch and transaction proof | Holdfast flavor/trade tests | `Ashfall.Core.Tests/HoldfastFlavorExpansionTests.cs; Ashfall.Core.Tests/HoldfastFactionIdentityContractTests.cs; Ashfall.Core.Tests/Narrative/Plan117_128HoldfastQuestFlavorIntegrationTests.cs` | Focused evidence surface. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load flavor catalog and current trade/faction catalogs
2. accept a current trade command through the trade owner
3. receive a typed success/failure result
4. resolve the keyed flavor voice by faction id
5. format a bounded dispatch entry with actual item/quantity/value data
6. present through the existing Holdfast terminal/UI route
7. capture only the current Holdfast trade/world save state

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Flavor voices are immutable presentation data; trade value, stock, inventory and faction trust are separate owner state.
- Dispatch log entries are bounded to 64 and are presentation history, not a save ledger.
- A rejection line must correspond to a current `HoldfastTradeFailure`; a success line must follow a successful transaction fact.
- Flavor faction IDs must be stable and should not be treated as a second faction registry.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A missing flavor key uses the current catalog fallback and never invents a faction.
- The dispatch line uses actual transaction values and a current failure code; it cannot claim an exchange that did not occur.
- Flavor text cannot alter price, stock, inventory, trust or access.
- The 64-entry presentation log is bounded and does not become a second persistence store.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `holdfast_flavor.json` is the sole flavor overlay authority.
- `holdfast_factions.json` remains the separate action/trade catalog; do not merge schemas or duplicate rows.
- A new flavor faction needs a stable ID, voice/rejected/sold fields and a current dispatch consumer.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- No flavor save section: the overlay is reloadable content and dispatch history is transient presentation.
- Current Holdfast world/trade saves remain authoritative for progress, inventory and value.
- A future persistent dispatch archive would require a current journal/log owner and a new claim.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Dispatch ordering is event order from the current trade owner; flavor selection is keyed lookup, not random.
- The same transaction sequence and faction key produce the same voice fields.
- No wall-clock or frame timing changes the dispatch log.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Holdfast trade/session commands produce current success/failure facts.
- Dispatch log methods translate those facts into bounded player-visible entries.
- No flavor method emits a new economic or faction mutation.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/HoldfastFlavorCatalog.cs
- src/Host/HoldfastDispatchLog.cs
- src/Host/HoldfastRuntimeSession.cs
- src/Main.Holdfast.cs
- src/UI/FactionsPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Flavor voices are distinct institutional routines, not stereotypes or real-world faction analogues.
- Reject/success copy should reflect the actual current failure/success and remain restrained.
- Do not add promises, rewards or access rules to flavor text that the trade owner does not implement.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | Flavor and trade faction IDs diverge. | HoldfastFlavorCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A rejection line is shown after a successful trade. | HoldfastDispatchLog | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A flavor key changes stock, price or trust. | HoldfastCatalog/HoldfastFactionsCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Dispatch log grows into an unowned persistent archive. | HoldfastTradeSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A missing flavor entry fabricates a transaction outcome. | HoldfastRuntimeSession/Main.Holdfast | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/HoldfastFlavorExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/HoldfastFactionIdentityContractTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/Plan117_128HoldfastQuestFlavorIntegrationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — authority census | Read flavor, trade faction/item catalogs, dispatch log and runtime route. | Overlay and mechanics are separated. | No production path until the owning implementation package is separately claimed. |
| 1 — reference/event trace | Map faction IDs, item IDs, success/failure codes and dispatch entries. | Every line reflects a current event. | No production path until the owning implementation package is separately claimed. |
| 2 — boundary/replay proof | Check bounded log, fallback and transaction restore. | No flavor state or economic mutation leaks. | No production path until the owning implementation package is separately claimed. |
| 3 — tone/precision pass | Review institutional voice, UI focus and copied-text risks. | Quality improves without content padding. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/holdfast_flavor.json | READ ONLY; MODIFY only for a proven voice/consumer gap | Flavor authority |
| src/Host/HoldfastDispatchLog.cs | READ ONLY | Presentation adapter |
| Assets/Ashfall.Core/HoldfastFactionsCatalog.cs | READ ONLY | Separate trade faction authority |
| src/Host/HoldfastRuntimeSession.cs | READ ONLY; MODIFY only under a new host claim | Runtime route |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Merging flavor and trade faction schemas. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Letting presentation text alter mechanics. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Creating a persistent dispatch ledger. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Copying real-world institutional stereotypes into fictional voices. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new flavor factions in this package.
- No new faction/trade authority.
- No new save section.
- No production/data/test/UI changes.

# 23. Rollback and Recovery

- Revert the planning document.
- Future flavor/host changes retain the prior valid JSON and current Holdfast trade fixtures.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 8 current flavor entries and 40 item entries are documented separately from trade factions.
- Dispatch event, fallback, bounded log and mechanical-owner boundaries are explicit.
- No duplicate faction or save authority is proposed.
- Focused tests and tone/accessibility obligations are named.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 3→8 data-only brief with an 8-faction flavor census and a separate `holdfast_factions.json` authority map.
- Verify every flavor faction ID referenced by a current dispatch event and every item/faction reference in the separate trade catalog.
- Audit dispatch event ordering, bounded log retention and truthful success/rejection copy.
- Preserve flavor as a presentation overlay with no independent trust, stock, price or quest state.

## MUST NOT DO

- No new flavor factions in this package.
- No new faction/trade authority.
- No new save section.
- No production/data/test/UI changes.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/HoldfastFlavorExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/HoldfastFactionIdentityContractTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/Plan117_128HoldfastQuestFlavorIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — authority census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: host flavor loading and keyed voice lookup → HoldfastFlavorCatalog; bounded transaction event projection → HoldfastDispatchLog; canonical item/faction trade definitions → HoldfastCatalog/HoldfastFactionsCatalog; mutable trade values, stock and transaction outcomes → HoldfastTradeSession; host composition and terminal route → HoldfastRuntimeSession/Main.Holdfast; identity, dispatch and transaction proof → Holdfast flavor/trade tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 128.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 128 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by HoldfastFlavorCatalog or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/HoldfastCatalog.cs`

### `Assets/Ashfall.Core/HoldfastCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 305 lines / 11871 bytes.
- SHA-256: `70d849c6b68037637e38b67197236cf26d96c187a1a8e38062765aa00fc08268`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class HoldfastLocationEntry
public string id;
public string displayName;
public string inspect;
public string description;
public float dangerLevel;
public float travelHours;
public float baseRadsPerHour;
public string region;
public bool overlay_on_unlock;
public bool recast_always;
public class HoldfastQuestStageEntry
public string id;
public string text;
public class HoldfastQuestChoiceEntry
public string id;
public string text;
public string set_flag;
public class HoldfastQuestEntry
public string id;
public string display_name;
public string type;
public string briefing;
public string prereq_quest_id;
public int min_day;
public HoldfastQuestStageEntry[] stages;
public HoldfastQuestChoiceEntry[] choices;
public string knowledge_key;
public string target_location_id;
public int StageCount => stages != null ? stages.Length : 0;
public sealed class HoldfastCatalog
public List<HoldfastLocationEntry> Locations { get; } = new List<HoldfastLocationEntry>();
public List<HoldfastQuestEntry> Quests { get; } = new List<HoldfastQuestEntry>();
public HoldfastItemsCatalog Items { get; set; } = HoldfastItemsCatalog.Empty();
public HoldfastFactionsCatalog Factions { get; set; } = HoldfastFactionsCatalog.Empty();
public HoldfastItemDefinition? GetItem(string id) => Items != null ? Items.GetById(id) : null;
public HoldfastFactionEntry? GetFaction(string id) => Factions != null ? Factions.GetById(id) : null;
public HoldfastLocationEntry? GetLocation(string id) {
public HoldfastQuestEntry? GetQuest(string id) {
public sealed class HoldfastItemDto
public string id { get; set; } = string.Empty;
public string displayName { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public double tradeValue { get; set; } = 0.0;
public double weight { get; set; } = 1.0;
public string type { get; set; } = "resource";
public int stackMax { get; set; } = 99;
public double thirstRestore { get; set; } = 0.0;
public double hungerRestore { get; set; } = 0.0;
public double moraleEffect { get; set; } = 0.0;
public sealed class HoldfastFactionDto
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string alignment { get; set; } = string.Empty;
public string home_region { get; set; } = string.Empty;
public bool is_active { get; set; } = true;
public float trust { get; set; } = 0f;
public string[] wants { get; set; } = Array.Empty<string>();
public string[] offers { get; set; } = Array.Empty<string>();
public string signature_quote { get; set; } = string.Empty;
public string access_rule { get; set; } = string.Empty;
public string badge_asset_id { get; set; } = string.Empty;
public sealed class HoldfastCatalogLoader
public const string LocationsFile = "holdfast_locations.json";
public const string QuestsFile = "holdfast_quests.json";
public const string ItemsFile = "holdfast_items.json";
public const string FactionsFile = "holdfast_factions.json";
public HoldfastCatalog Load(string dataDirectory, bool expansionUnlocked = true) {
public static bool IncludeLocation(HoldfastLocationEntry e, bool expansionUnlocked) {
public static string StripAuthorNotes(string displayName) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/HoldfastFactionsCatalog.cs`

### `Assets/Ashfall.Core/HoldfastFactionsCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 92 lines / 4796 bytes.
- SHA-256: `0ab477ba16de881b921757e1a4b5eda4814b14f8c7a40e572df2ff4a96b384a6`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class HoldfastFactionEntry
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string alignment { get; set; } = string.Empty;
public string home_region { get; set; } = string.Empty;
public bool is_active { get; set; } = true;
public float trust { get; set; } = 0f;
public string[] wants { get; set; } = Array.Empty<string>();
public string[] offers { get; set; } = Array.Empty<string>();
public string signature_quote { get; set; } = string.Empty;
public string access_rule { get; set; } = string.Empty;
public string badge_asset_id { get; set; } = string.Empty;
public string Id => id;
public string DisplayName => display_name;
public string Alignment => alignment;
public string HomeRegion => home_region;
public bool IsActive => is_active;
public float Trust => trust;
public string[] Wants => wants;
public string[] Offers => offers;
public string SignatureQuote => signature_quote;
public string AccessRule => access_rule;
public string BadgeAssetId => badge_asset_id;
public string FactionDescription() {
public sealed class HoldfastFactionsCatalog : IEnumerable<HoldfastFactionEntry>
public int Count => _order.Count;
public static HoldfastFactionsCatalog Empty() => new HoldfastFactionsCatalog();
public void Register(HoldfastFactionEntry entry) {
public HoldfastFactionEntry? GetById(string id) => string.IsNullOrEmpty(id) ? null : (_byId.TryGetValue(id, out var e) ? e : null);
public bool Contains(string id) => GetById(id) != null;
public IEnumerator<HoldfastFactionEntry> GetEnumerator() => _order.GetEnumerator();
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/HoldfastTradeSession.cs`

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


# Appendix B.05 — Current Code Architecture: `src/Host/HoldfastFlavorCatalog.cs`

### `src/Host/HoldfastFlavorCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 118 lines / 4376 bytes.
- SHA-256: `ade571e9a5b98851c94e94ccecb11d33a4197b1772c4ece6db2c9d15d21cea1e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class HoldfastFlavorCatalog
public Dictionary<string, string> ItemMarginalia { get; } = new Dictionary<string, string>(StringComparer.Ordinal);
public Dictionary<string, FactionVoice> FactionVoices { get; } = new Dictionary<string, FactionVoice>(StringComparer.Ordinal);
public string GetItemMarginalia(string itemId) {
public FactionVoice GetFactionVoice(string factionId) {
public static HoldfastFlavorCatalog Load(string dataDirectory, ILog log = null!) {
public const string NeutralItemMarginalia = "No marginalia on file for this item.";
public static readonly FactionVoice NeutralFactionVoice = new FactionVoice {
public sealed class FactionVoice
public string register = "neutral";
public string voice = string.Empty;
public string rejected = string.Empty;
public string sold = string.Empty;
public Dictionary<string, FactionVoice> factions;
public Dictionary<string, string> items;
```


# Appendix B.06 — Current Code Architecture: `src/Host/HoldfastDispatchLog.cs`

### `src/Host/HoldfastDispatchLog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 128 lines / 5288 bytes.
- SHA-256: `eafd43752bd36ca31e37ec99fe9bd099506913a730c991c82312b8c05ab2ae8b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class HoldfastDispatchLog
public IReadOnlyList<string> Entries => _entries;
public string LastDispatch => _lastDispatch;
public void OnSessionOpened(string sessionType) {
public void OnFirstPurchase(string itemId, int quantity, long totalValue, string factionId) {
public void OnPurchase(string itemId, int quantity, long totalValue, string factionId) {
public void OnSale(string itemId, int quantity, long totalValue, string factionId) {
public void OnHoldingEmptied(string itemId, string factionId) {
public void OnStockLow(string itemId, int remaining, string factionId) {
public void OnStockEmpty(string itemId, string factionId) {
public void OnRejected(HoldfastTradeResult result, string factionId) {
public void OnSaveCommitted(string path) {
public void OnReloaded(string path) {
public void OnQuarantine(string corruptPath) {
public void OnNewLedger() {
```


# Appendix B.07 — Current Code Architecture: `src/Host/HoldfastRuntimeSession.cs`

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


# Appendix B.08 — Current Code Architecture: `src/Main.Holdfast.cs`

### `src/Main.Holdfast.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 553 lines / 24184 bytes.
- SHA-256: `741c15a28d8d797a0875ba7294690468adae3db61a4dbc56f4d9587672c9bee7`.
- Architecture signals: seeded references=0; save/restore symbols=3; typed event declarations=0; textual Godot mentions=7; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
internal DayAdvancedEventArgs? AdvanceCampaignDayForValidation(int day) {
```


# Appendix B.09 — Current Code Architecture: `src/UI/FactionsPanel.cs`

### `src/UI/FactionsPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 536 lines / 30554 bytes.
- SHA-256: `a129850bff990e300e3f80308d74c8ecbd029463eda3417438045b1a6892986d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=17; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class FactionsPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<string>? OnFactionDetailRequested;
public event Action? OnMusterPanelRequested;
public event Action? OnFoundryPanelRequested;
public event Action? OnCultureCodexRequested;
public event Action<int>? OnWarlordTributePay;
public event Action? OnWarlordTributeRefuse;
public event Action<string>? OnCommitBranchRequested;
public bool IsBound => _factions != null || _muster != null || _expansions != null || _branchCoordinator != null;
public bool HasGuildCard { get; private set; }
public void Bind( HoldfastFactionsCatalog? factions, HoldfastTradeSession? trade = null, MusterHostSession? muster = null, ExpansionHostSession? expansions = null, YearOfAshHostSession? yearOfAsh = null,
public void RefreshView() {
public override void _Ready() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/holdfast_flavor.json`

### `Assets/StreamingAssets/Data/holdfast_flavor.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 8716 bytes / 8706 characters.
- SHA-256: `e2daadc29e1a5a7e9c4b5143060ba1df4b48418923166c60f8a9bdac2b9a451d`.
- Root keys: `factions`, `items`, `schema_version`.


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/holdfast_factions.json`

### `Assets/StreamingAssets/Data/holdfast_factions.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 6557 bytes / 6557 characters.
- SHA-256: `e2f13c2291cba31c05b7b0dbca06dc37bd9bf5d692e51d9c2250c8738bdcbd44`.
- Root keys: `actions`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
actions: min=9, max=9, observed_paths=1
actions[].offers: min=3, max=3, observed_paths=2
actions[].wants: min=3, max=3, observed_paths=2
```

Representative record fields:

- `access_rule`
- `alignment`
- `badge_asset_id`
- `display_name`
- `home_region`
- `id`
- `is_active`
- `offers`
- `signature_quote`
- `trust`
- `wants`

Representative identifiers (ordered, capped for readability):

```text
faction_the_office
faction_the_cutters
faction_the_fleet
faction_black_flotilla
faction_supply_corps
faction_railway_guild
faction_hydro_barons
faction_ordnance_foundry
faction_scavengers
```


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/holdfast_items.json`

### `Assets/StreamingAssets/Data/holdfast_items.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 26102 bytes / 26097 characters.
- SHA-256: `431a1c9ec7c22d0ff6efa8a0f4e53ce56be618ab97a5624e82cab0784c8e2f4a`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=55, max=55, observed_paths=1
```

Representative record fields:

- `description`
- `displayName`
- `hungerRestore`
- `id`
- `moraleEffect`
- `stackMax`
- `thirstRestore`
- `tradeValue`
- `type`
- `weight`

Representative identifiers (ordered, capped for readability):

```text
item_map_sheet_ice_road
item_census_return_blank
item_order_12c
item_allocation_tag
item_triplicate_carbon
item_ice_spike_bar
item_beacon_oil
item_cutter_ledger_blank
item_ice_tyre_set
item_plant_suit_patched
item_resin_gloves
item_fume_rag
item_shift_whistle
item_work_ticket
item_steam_token
item_block_c_key
item_ro_resin
item_ro_resin_spent
item_iodine_crystal
item_process_barrel
item_schedule_crystal
item_fleet_pad_copy
item_foghorn_key
item_kittiwake_copy
item_weigh_receipt_hf
item_schedule_sector4_copy
item_halvard_kit_notes
item_sole_unsigned
item_playground_seat
item_edor_return_self
item_yara_dark_mark
item_leva_minutes_vol12
item_hearth4_hatch_log
item_alloc7_ration_tin
item_cluster_formulary
item_foghorn_timer
item_tin_fourteenth
item_salt_rash_salve
item_uv_grease
item_electrolyte_salts
item_canned_food
item_fuel
item_medical_kit
item_clean_water
item_water_filter
item_water_purification_tablets_40_of_40
item_diesel_fuel
item_mechanical_parts
item_engine
item_ammo_762
item_soldering_kit
item_gas_mask
item_dried_rations
item_antibiotics
item_dosimeter
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/HoldfastFlavorExpansionTests.cs`

### `Ashfall.Core.Tests/HoldfastFlavorExpansionTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 219; SHA-256: `57676604e6f71701d5aa27c225ed37c9f4f40cff05b4d38e43280f0965e49648`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
HoldfastFlavor_SchemaVersionAndStructure
HoldfastFlavor_BaselineThreeFactions_ParityPreserved
HoldfastFlavor_FiveNewFactions_ArePresentAndCanonical
HoldfastFlavor_AllEightFactions_HaveUniqueRegistersAndVoices
HoldfastFlavor_AllFactionsExistInHoldfastFactionsCatalog
HoldfastFlavor_ItemsDictionary_PreservedExactly
HoldfastFlavor_FallbackBehaviorSimulation
```


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/HoldfastFactionIdentityContractTests.cs`

### `Ashfall.Core.Tests/HoldfastFactionIdentityContractTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 191; SHA-256: `a82fa01d0696fe035aafdc23c259e4230fc69aed601c8174c39e2ddab1664e06`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Roster_PreservesThreeBaselineAndEightAuthoredIdentities
RosterEntries_HaveValidatedIdentityAndTradeFields
FlavorProfiles_AreExactlyTheEightAuthoredIdentities
HoldfastNpcReferences_UseCanonicalFactionIds
TradeSaveState_DoesNotDuplicateStaticFactionTrust
LegacyHoldfastAliases_DoNotRemainInCanonicalSources
```


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Narrative/Plan117_128HoldfastQuestFlavorIntegrationTests.cs`

### `Ashfall.Core.Tests/Narrative/Plan117_128HoldfastQuestFlavorIntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 155; SHA-256: `4dd19bc5138bb228af6e2e94a62aed486e4515984828666700d7a827696a55d7`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
HoldfastCatalog_LoadsAll24Quests_AndValidatesIntegrity
HoldfastFlavor_LoadsAll8Factions_AndValidatesRegisters
HoldfastQuestSystem_ExecutesExpeditionFlow
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/HoldfastCatalog.cs`

### `Assets/Ashfall.Core/HoldfastCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 305 lines / 11871 bytes.
- SHA-256: `70d849c6b68037637e38b67197236cf26d96c187a1a8e38062765aa00fc08268`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class HoldfastLocationEntry
public string id;
public string displayName;
public string inspect;
public string description;
public float dangerLevel;
public float travelHours;
public float baseRadsPerHour;
public string region;
public bool overlay_on_unlock;
public bool recast_always;
public class HoldfastQuestStageEntry
public string id;
public string text;
public class HoldfastQuestChoiceEntry
public string id;
public string text;
public string set_flag;
public class HoldfastQuestEntry
public string id;
public string display_name;
public string type;
public string briefing;
public string prereq_quest_id;
public int min_day;
public HoldfastQuestStageEntry[] stages;
public HoldfastQuestChoiceEntry[] choices;
public string knowledge_key;
public string target_location_id;
public int StageCount => stages != null ? stages.Length : 0;
public sealed class HoldfastCatalog
public List<HoldfastLocationEntry> Locations { get; } = new List<HoldfastLocationEntry>();
public List<HoldfastQuestEntry> Quests { get; } = new List<HoldfastQuestEntry>();
public HoldfastItemsCatalog Items { get; set; } = HoldfastItemsCatalog.Empty();
public HoldfastFactionsCatalog Factions { get; set; } = HoldfastFactionsCatalog.Empty();
public HoldfastItemDefinition? GetItem(string id) => Items != null ? Items.GetById(id) : null;
public HoldfastFactionEntry? GetFaction(string id) => Factions != null ? Factions.GetById(id) : null;
public HoldfastLocationEntry? GetLocation(string id) {
public HoldfastQuestEntry? GetQuest(string id) {
public sealed class HoldfastItemDto
public string id { get; set; } = string.Empty;
public string displayName { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public double tradeValue { get; set; } = 0.0;
public double weight { get; set; } = 1.0;
public string type { get; set; } = "resource";
public int stackMax { get; set; } = 99;
public double thirstRestore { get; set; } = 0.0;
public double hungerRestore { get; set; } = 0.0;
public double moraleEffect { get; set; } = 0.0;
public sealed class HoldfastFactionDto
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string alignment { get; set; } = string.Empty;
public string home_region { get; set; } = string.Empty;
public bool is_active { get; set; } = true;
public float trust { get; set; } = 0f;
public string[] wants { get; set; } = Array.Empty<string>();
public string[] offers { get; set; } = Array.Empty<string>();
public string signature_quote { get; set; } = string.Empty;
public string access_rule { get; set; } = string.Empty;
public string badge_asset_id { get; set; } = string.Empty;
public sealed class HoldfastCatalogLoader
public const string LocationsFile = "holdfast_locations.json";
public const string QuestsFile = "holdfast_quests.json";
public const string ItemsFile = "holdfast_items.json";
public const string FactionsFile = "holdfast_factions.json";
public HoldfastCatalog Load(string dataDirectory, bool expansionUnlocked = true) {
public static bool IncludeLocation(HoldfastLocationEntry e, bool expansionUnlocked) {
public static string StripAuthorNotes(string displayName) {
```


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/HoldfastFactionsCatalog.cs`

### `Assets/Ashfall.Core/HoldfastFactionsCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 92 lines / 4796 bytes.
- SHA-256: `0ab477ba16de881b921757e1a4b5eda4814b14f8c7a40e572df2ff4a96b384a6`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class HoldfastFactionEntry
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string alignment { get; set; } = string.Empty;
public string home_region { get; set; } = string.Empty;
public bool is_active { get; set; } = true;
public float trust { get; set; } = 0f;
public string[] wants { get; set; } = Array.Empty<string>();
public string[] offers { get; set; } = Array.Empty<string>();
public string signature_quote { get; set; } = string.Empty;
public string access_rule { get; set; } = string.Empty;
public string badge_asset_id { get; set; } = string.Empty;
public string Id => id;
public string DisplayName => display_name;
public string Alignment => alignment;
public string HomeRegion => home_region;
public bool IsActive => is_active;
public float Trust => trust;
public string[] Wants => wants;
public string[] Offers => offers;
public string SignatureQuote => signature_quote;
public string AccessRule => access_rule;
public string BadgeAssetId => badge_asset_id;
public string FactionDescription() {
public sealed class HoldfastFactionsCatalog : IEnumerable<HoldfastFactionEntry>
public int Count => _order.Count;
public static HoldfastFactionsCatalog Empty() => new HoldfastFactionsCatalog();
public void Register(HoldfastFactionEntry entry) {
public HoldfastFactionEntry? GetById(string id) => string.IsNullOrEmpty(id) ? null : (_byId.TryGetValue(id, out var e) ? e : null);
public bool Contains(string id) => GetById(id) != null;
public IEnumerator<HoldfastFactionEntry> GetEnumerator() => _order.GetEnumerator();
```


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/HoldfastTradeSession.cs`

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


# Appendix E.19 — Supporting Code Evidence: `src/Host/HoldfastFlavorCatalog.cs`

### `src/Host/HoldfastFlavorCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 118 lines / 4376 bytes.
- SHA-256: `ade571e9a5b98851c94e94ccecb11d33a4197b1772c4ece6db2c9d15d21cea1e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class HoldfastFlavorCatalog
public Dictionary<string, string> ItemMarginalia { get; } = new Dictionary<string, string>(StringComparer.Ordinal);
public Dictionary<string, FactionVoice> FactionVoices { get; } = new Dictionary<string, FactionVoice>(StringComparer.Ordinal);
public string GetItemMarginalia(string itemId) {
public FactionVoice GetFactionVoice(string factionId) {
public static HoldfastFlavorCatalog Load(string dataDirectory, ILog log = null!) {
public const string NeutralItemMarginalia = "No marginalia on file for this item.";
public static readonly FactionVoice NeutralFactionVoice = new FactionVoice {
public sealed class FactionVoice
public string register = "neutral";
public string voice = string.Empty;
public string rejected = string.Empty;
public string sold = string.Empty;
public Dictionary<string, FactionVoice> factions;
public Dictionary<string, string> items;
```


# Appendix E.20 — Supporting Code Evidence: `src/Host/HoldfastDispatchLog.cs`

### `src/Host/HoldfastDispatchLog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 128 lines / 5288 bytes.
- SHA-256: `eafd43752bd36ca31e37ec99fe9bd099506913a730c991c82312b8c05ab2ae8b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class HoldfastDispatchLog
public IReadOnlyList<string> Entries => _entries;
public string LastDispatch => _lastDispatch;
public void OnSessionOpened(string sessionType) {
public void OnFirstPurchase(string itemId, int quantity, long totalValue, string factionId) {
public void OnPurchase(string itemId, int quantity, long totalValue, string factionId) {
public void OnSale(string itemId, int quantity, long totalValue, string factionId) {
public void OnHoldingEmptied(string itemId, string factionId) {
public void OnStockLow(string itemId, int remaining, string factionId) {
public void OnStockEmpty(string itemId, string factionId) {
public void OnRejected(HoldfastTradeResult result, string factionId) {
public void OnSaveCommitted(string path) {
public void OnReloaded(string path) {
public void OnQuarantine(string corruptPath) {
public void OnNewLedger() {
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/HoldfastFlavorExpansionTests.cs`

### `Ashfall.Core.Tests/HoldfastFlavorExpansionTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 219; SHA-256: `57676604e6f71701d5aa27c225ed37c9f4f40cff05b4d38e43280f0965e49648`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
HoldfastFlavor_SchemaVersionAndStructure
HoldfastFlavor_BaselineThreeFactions_ParityPreserved
HoldfastFlavor_FiveNewFactions_ArePresentAndCanonical
HoldfastFlavor_AllEightFactions_HaveUniqueRegistersAndVoices
HoldfastFlavor_AllFactionsExistInHoldfastFactionsCatalog
HoldfastFlavor_ItemsDictionary_PreservedExactly
HoldfastFlavor_FallbackBehaviorSimulation
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/HoldfastFactionIdentityContractTests.cs`

### `Ashfall.Core.Tests/HoldfastFactionIdentityContractTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 191; SHA-256: `a82fa01d0696fe035aafdc23c259e4230fc69aed601c8174c39e2ddab1664e06`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Roster_PreservesThreeBaselineAndEightAuthoredIdentities
RosterEntries_HaveValidatedIdentityAndTradeFields
FlavorProfiles_AreExactlyTheEightAuthoredIdentities
HoldfastNpcReferences_UseCanonicalFactionIds
TradeSaveState_DoesNotDuplicateStaticFactionTrust
LegacyHoldfastAliases_DoNotRemainInCanonicalSources
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/Narrative/Plan117_128HoldfastQuestFlavorIntegrationTests.cs`

### `Ashfall.Core.Tests/Narrative/Plan117_128HoldfastQuestFlavorIntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 155; SHA-256: `4dd19bc5138bb228af6e2e94a62aed486e4515984828666700d7a827696a55d7`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
HoldfastCatalog_LoadsAll24Quests_AndValidatesIntegrity
HoldfastFlavor_LoadsAll8Factions_AndValidatesRegisters
HoldfastQuestSystem_ExecutesExpeditionFlow
```


# Appendix H.24 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

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
| host flavor loading and keyed voice lookup | HoldfastFlavorCatalog | bounded transaction event projection | HoldfastDispatchLog | Owner emits/reads a typed fact; no mirror state. |
| host flavor loading and keyed voice lookup | HoldfastFlavorCatalog | canonical item/faction trade definitions | HoldfastCatalog/HoldfastFactionsCatalog | Owner emits/reads a typed fact; no mirror state. |
| host flavor loading and keyed voice lookup | HoldfastFlavorCatalog | mutable trade values, stock and transaction outcomes | HoldfastTradeSession | Owner emits/reads a typed fact; no mirror state. |
| host flavor loading and keyed voice lookup | HoldfastFlavorCatalog | host composition and terminal route | HoldfastRuntimeSession/Main.Holdfast | Owner emits/reads a typed fact; no mirror state. |
| host flavor loading and keyed voice lookup | HoldfastFlavorCatalog | identity, dispatch and transaction proof | Holdfast flavor/trade tests | Owner emits/reads a typed fact; no mirror state. |
| bounded transaction event projection | HoldfastDispatchLog | host flavor loading and keyed voice lookup | HoldfastFlavorCatalog | Owner emits/reads a typed fact; no mirror state. |
| bounded transaction event projection | HoldfastDispatchLog | canonical item/faction trade definitions | HoldfastCatalog/HoldfastFactionsCatalog | Owner emits/reads a typed fact; no mirror state. |
| bounded transaction event projection | HoldfastDispatchLog | mutable trade values, stock and transaction outcomes | HoldfastTradeSession | Owner emits/reads a typed fact; no mirror state. |
| bounded transaction event projection | HoldfastDispatchLog | host composition and terminal route | HoldfastRuntimeSession/Main.Holdfast | Owner emits/reads a typed fact; no mirror state. |
| bounded transaction event projection | HoldfastDispatchLog | identity, dispatch and transaction proof | Holdfast flavor/trade tests | Owner emits/reads a typed fact; no mirror state. |
| canonical item/faction trade definitions | HoldfastCatalog/HoldfastFactionsCatalog | host flavor loading and keyed voice lookup | HoldfastFlavorCatalog | Owner emits/reads a typed fact; no mirror state. |
| canonical item/faction trade definitions | HoldfastCatalog/HoldfastFactionsCatalog | bounded transaction event projection | HoldfastDispatchLog | Owner emits/reads a typed fact; no mirror state. |
| canonical item/faction trade definitions | HoldfastCatalog/HoldfastFactionsCatalog | mutable trade values, stock and transaction outcomes | HoldfastTradeSession | Owner emits/reads a typed fact; no mirror state. |
| canonical item/faction trade definitions | HoldfastCatalog/HoldfastFactionsCatalog | host composition and terminal route | HoldfastRuntimeSession/Main.Holdfast | Owner emits/reads a typed fact; no mirror state. |
| canonical item/faction trade definitions | HoldfastCatalog/HoldfastFactionsCatalog | identity, dispatch and transaction proof | Holdfast flavor/trade tests | Owner emits/reads a typed fact; no mirror state. |
| mutable trade values, stock and transaction outcomes | HoldfastTradeSession | host flavor loading and keyed voice lookup | HoldfastFlavorCatalog | Owner emits/reads a typed fact; no mirror state. |
| mutable trade values, stock and transaction outcomes | HoldfastTradeSession | bounded transaction event projection | HoldfastDispatchLog | Owner emits/reads a typed fact; no mirror state. |
| mutable trade values, stock and transaction outcomes | HoldfastTradeSession | canonical item/faction trade definitions | HoldfastCatalog/HoldfastFactionsCatalog | Owner emits/reads a typed fact; no mirror state. |
| mutable trade values, stock and transaction outcomes | HoldfastTradeSession | host composition and terminal route | HoldfastRuntimeSession/Main.Holdfast | Owner emits/reads a typed fact; no mirror state. |
| mutable trade values, stock and transaction outcomes | HoldfastTradeSession | identity, dispatch and transaction proof | Holdfast flavor/trade tests | Owner emits/reads a typed fact; no mirror state. |
| host composition and terminal route | HoldfastRuntimeSession/Main.Holdfast | host flavor loading and keyed voice lookup | HoldfastFlavorCatalog | Owner emits/reads a typed fact; no mirror state. |
| host composition and terminal route | HoldfastRuntimeSession/Main.Holdfast | bounded transaction event projection | HoldfastDispatchLog | Owner emits/reads a typed fact; no mirror state. |
| host composition and terminal route | HoldfastRuntimeSession/Main.Holdfast | canonical item/faction trade definitions | HoldfastCatalog/HoldfastFactionsCatalog | Owner emits/reads a typed fact; no mirror state. |
| host composition and terminal route | HoldfastRuntimeSession/Main.Holdfast | mutable trade values, stock and transaction outcomes | HoldfastTradeSession | Owner emits/reads a typed fact; no mirror state. |
| host composition and terminal route | HoldfastRuntimeSession/Main.Holdfast | identity, dispatch and transaction proof | Holdfast flavor/trade tests | Owner emits/reads a typed fact; no mirror state. |
| identity, dispatch and transaction proof | Holdfast flavor/trade tests | host flavor loading and keyed voice lookup | HoldfastFlavorCatalog | Owner emits/reads a typed fact; no mirror state. |
| identity, dispatch and transaction proof | Holdfast flavor/trade tests | bounded transaction event projection | HoldfastDispatchLog | Owner emits/reads a typed fact; no mirror state. |
| identity, dispatch and transaction proof | Holdfast flavor/trade tests | canonical item/faction trade definitions | HoldfastCatalog/HoldfastFactionsCatalog | Owner emits/reads a typed fact; no mirror state. |
| identity, dispatch and transaction proof | Holdfast flavor/trade tests | mutable trade values, stock and transaction outcomes | HoldfastTradeSession | Owner emits/reads a typed fact; no mirror state. |
| identity, dispatch and transaction proof | Holdfast flavor/trade tests | host composition and terminal route | HoldfastRuntimeSession/Main.Holdfast | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 3→8 data-only brief with an 8-faction flavor census and a separate `holdfast_factions.json` authority map. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Verify every flavor faction ID referenced by a current dispatch event and every item/faction reference in the separate trade catalog. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Audit dispatch event ordering, bounded log retention and truthful success/rejection copy. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Preserve flavor as a presentation overlay with no independent trust, stock, price or quest state. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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

> **DR-02 — The docs tree has substantially more subdirectories than the v1.0 map. VERIFIED.**
Live `docs/` subdirectories observed in the audit include (selection; the listing was long and partially truncated): `adr/`, `agents/`, `architecture/`, `archive/`, `balance/`, `bodymind/`, `campaign/`, `cartography/`, `ci/`, `cli/`, `collectibles/`, `combat/`, `content/`, `contracts/`, `crafting/`, `crossing/`, `culture/`, `decisions/`, `design/`, `discovery/`, `duty_roster/`, `ecology/`, `economy/`, `endgame/`, `expansions/`, `expeditions/`, `faction_war/`, `factions/`, `foreman/`, `forensics/`, `foundry/`, `gaps/`, `governance/`, `greenhouse/`, `health/`, `holdfast/`, `hygiene/`, `i18n/`, `implementation/`, `incidents/`, `integration/`, `journal/`, `lore/`, `maritime/`, `medical/`, `memorials/`, `mods/`, `moral/`, `moral_choice/`, `muster/`, `narrative/`, `onboarding/`, `orbital/`, `perf/`, `phantoms/`, `plans/`, `power/`, `process/`, `production/`, and a `player_surface_manifest.json`. Two of these — `gaps/` and `incidents/` — are first-class *expansion feedstock*: directories whose entire purpose is to record what is missing or broken. The Factory Protocol (Part II, step 2) now treats `docs/gaps/` and `docs/incidents/` as mandatory inputs.

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
| C2 | Dose-treatment matrix paired tests against `MEDICAL_DOSE_TREATMENT_MATRIX.md` | HIGH CONFIDENCE |
| C11 | Debt-ledger consequence dispatcher coverage; rumor-band determinism pins | HIGH CONFIDENCE |
| C10 | Moral-choice flag consumer coverage for newly added consumers | HIGH CONFIDENCE |
| C13 | Epilogue permutation reachability tests for under-served permutations | HIGH CONFIDENCE |
| Cross | Determinism two-pass proofs for every new simulation; TEST-AGGREGATION metadata for catalog checks | CANON process |

> | Cluster | Opening archetype | Confidence |
|---|---|---|
| C16 | Difficulty preset scalar consumers (CF-XP01 line, reinforced by active W1, DR-06) | HIGH CONFIDENCE |
| C17 | Daily-briefing surface for newly landed systems; onboarding flow waves | HIGH CONFIDENCE |
| All | Manual playthrough checklists per wave (pattern exists: HoldfastManualPlaytest, expedition playtest report) | CANON process |

> **SB-01 — Delayed moral-choice callbacks (Lane A/C10).** Evidence: v1.0 Part 7 gap 2; `moral_choice_flags.json`, `IFlagLedger`, `DoorEncounterSystem`, `MoralChoiceSaveStore` all confirmed live. Subject: ~100-day delayed visitor/letter/radio/journal returns keyed on persisted flags. Integration route: data-first new catalog through the moral-choice loader family; dispatch through the daily-tick seam; possibly no codec bump if per-flag records already persist. Verification: integrity + utilization selftests, determinism replay, exactly-once dispatch test. Confidence: HIGH CONFIDENCE.

> ### Recommended integration route (Template R)
Tier: DATA-ONLY plus one host-wiring step. Seams: new `moral_choice_delayed_callbacks.json` → moral-choice loader family integrity rules → daily-tick dispatch in the moral-choice host session (named RNG sub-stream `delayed_callback`) → journal / radio strip / relationship delta / rumor routing through existing owners → exactly-once guard keyed on flag id + day. Save impact class: EXISTING-SECTION if per-flag records persist in the moral-choice section (verify); CODEC-BUMP-AND-MIGRATE otherwise. Determinism impact: NEW-RNG-SUBSTREAM. Verification class: data-integrity selftest, content-utilization selftest, focused xUnit (loader gate with per-row failure output, exactly-once dispatch test, two-pass determinism replay, one cross-system consequence test).

> ### Recommended integration route
Tier: DATA-ONLY (three parallel authored tranches, one per owning system). Seams: ecology infestation catalog + crop strain catalog → existing infestation event dispatch; subterranean zones + excavation hazard mitigation catalogs → existing cave-in event path; warlord doctrines + tribute ledger → existing levy/tribute seams with `FactionStanceEngine` for reactions. Save impact class: NONE (events derive from catalogs and campaign state). Determinism: events must use existing seeded event streams — no new simulation. Verification: integrity + utilization selftests, one focused event-dispatch test per arc, balance harness re-run for the levy's economic pressure.

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

The subject is a presentation overlay around the current Holdfast trade transaction. The plan expands identity separation, event truth, bounded logging, tone and accessibility while leaving price, stock, trust and persistence to their current owners.

- **host flavor loading and keyed voice lookup** remains with `HoldfastFlavorCatalog` at `src/Host/HoldfastFlavorCatalog.cs`. Owns presentation flavor only.
- **bounded transaction event projection** remains with `HoldfastDispatchLog` at `src/Host/HoldfastDispatchLog.cs`. Formats current dispatch facts; it does not settle trades.
- **canonical item/faction trade definitions** remains with `HoldfastCatalog/HoldfastFactionsCatalog` at `Assets/Ashfall.Core/HoldfastCatalog.cs; Assets/Ashfall.Core/HoldfastFactionsCatalog.cs`. Owns trade-facing faction/item metadata.
- **mutable trade values, stock and transaction outcomes** remains with `HoldfastTradeSession` at `Assets/Ashfall.Core/HoldfastTradeSession.cs`. Sole transaction authority; flavor cannot mutate it.
- **host composition and terminal route** remains with `HoldfastRuntimeSession/Main.Holdfast` at `src/Host/HoldfastRuntimeSession.cs; src/Main.Holdfast.cs`. Thin host wiring and player route.
- **identity, dispatch and transaction proof** remains with `Holdfast flavor/trade tests` at `Ashfall.Core.Tests/HoldfastFlavorExpansionTests.cs; Ashfall.Core.Tests/HoldfastFactionIdentityContractTests.cs; Ashfall.Core.Tests/Narrative/Plan117_128HoldfastQuestFlavorIntegrationTests.cs`. Focused evidence surface.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load flavor catalog and current trade/faction catalogs
2. accept a current trade command through the trade owner
3. receive a typed success/failure result
4. resolve the keyed flavor voice by faction id
5. format a bounded dispatch entry with actual item/quantity/value data
6. present through the existing Holdfast terminal/UI route
7. capture only the current Holdfast trade/world save state

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Flavor voices are immutable presentation data; trade value, stock, inventory and faction trust are separate owner state.
- Dispatch log entries are bounded to 64 and are presentation history, not a save ledger.
- A rejection line must correspond to a current `HoldfastTradeFailure`; a success line must follow a successful transaction fact.
- Flavor faction IDs must be stable and should not be treated as a second faction registry.

- A missing flavor key uses the current catalog fallback and never invents a faction.
- The dispatch line uses actual transaction values and a current failure code; it cannot claim an exchange that did not occur.
- Flavor text cannot alter price, stock, inventory, trust or access.
- The 64-entry presentation log is bounded and does not become a second persistence store.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/HoldfastFlavorCatalog.cs
- src/Host/HoldfastDispatchLog.cs
- src/Host/HoldfastRuntimeSession.cs
- src/Main.Holdfast.cs
- src/UI/FactionsPanel.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/HoldfastFlavorExpansionTests.cs
- Ashfall.Core.Tests/HoldfastFactionIdentityContractTests.cs
- Ashfall.Core.Tests/Narrative/Plan117_128HoldfastQuestFlavorIntegrationTests.cs

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
| S-01 | 128-01 load eight flavor factions | load flavor catalog and current trade/faction catalogs | Flavor voices are immutable presentation data; trade value, stock, inventory and faction trust are separate owner state. | Flavor and trade faction IDs diverge. | HoldfastFlavorCatalog |
| S-02 | 128-02 unknown faction fallback | accept a current trade command through the trade owner | Dispatch log entries are bounded to 64 and are presentation history, not a save ledger. | A rejection line is shown after a successful trade. | HoldfastFlavorCatalog |
| S-03 | 128-03 first purchase dispatch | receive a typed success/failure result | A rejection line must correspond to a current `HoldfastTradeFailure`; a success line must follow a successful transaction fact. | A flavor key changes stock, price or trust. | HoldfastFlavorCatalog |
| S-04 | 128-04 successful sale dispatch | resolve the keyed flavor voice by faction id | Flavor faction IDs must be stable and should not be treated as a second faction registry. | Dispatch log grows into an unowned persistent archive. | HoldfastFlavorCatalog |
| S-05 | 128-05 stock low/empty dispatch | format a bounded dispatch entry with actual item/quantity/value data | Flavor voices are immutable presentation data; trade value, stock, inventory and faction trust are separate owner state. | A missing flavor entry fabricates a transaction outcome. | HoldfastFlavorCatalog |
| S-06 | 128-06 rejected transaction mapping | present through the existing Holdfast terminal/UI route | Dispatch log entries are bounded to 64 and are presentation history, not a save ledger. | Flavor and trade faction IDs diverge. | HoldfastFlavorCatalog |
| S-07 | 128-07 bounded 64-entry log | capture only the current Holdfast trade/world save state | A rejection line must correspond to a current `HoldfastTradeFailure`; a success line must follow a successful transaction fact. | A rejection line is shown after a successful trade. | HoldfastFlavorCatalog |
| S-08 | 128-08 reload trade state without replaying flavor | load flavor catalog and current trade/faction catalogs | Flavor faction IDs must be stable and should not be treated as a second faction registry. | A flavor key changes stock, price or trust. | HoldfastFlavorCatalog |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 128-TC-01 flavor schema and key count | data | flavor schema and key count; verify the current owner and its negative boundary without inventing a second authority. | HoldfastFlavorCatalog |
| T-02 | 128-TC-02 faction ID cross-check | unit | faction ID cross-check; verify the current owner and its negative boundary without inventing a second authority. | HoldfastFlavorCatalog |
| T-03 | 128-TC-03 item key cross-check | persistence | item key cross-check; verify the current owner and its negative boundary without inventing a second authority. | HoldfastFlavorCatalog |
| T-04 | 128-TC-04 voice/rejected/sold nonempty | determinism | voice/rejected/sold nonempty; verify the current owner and its negative boundary without inventing a second authority. | HoldfastFlavorCatalog |
| T-05 | 128-TC-05 dispatch first purchase | host | dispatch first purchase; verify the current owner and its negative boundary without inventing a second authority. | HoldfastFlavorCatalog |
| T-06 | 128-TC-06 dispatch sale | UI/accessibility | dispatch sale; verify the current owner and its negative boundary without inventing a second authority. | HoldfastFlavorCatalog |
| T-07 | 128-TC-07 dispatch stock states | cross-system | dispatch stock states; verify the current owner and its negative boundary without inventing a second authority. | HoldfastFlavorCatalog |
| T-08 | 128-TC-08 failure-code mapping | data | failure-code mapping; verify the current owner and its negative boundary without inventing a second authority. | HoldfastFlavorCatalog |
| T-09 | 128-TC-09 log bound eviction | unit | log bound eviction; verify the current owner and its negative boundary without inventing a second authority. | HoldfastFlavorCatalog |
| T-10 | 128-TC-10 no trade mutation | persistence | no trade mutation; verify the current owner and its negative boundary without inventing a second authority. | HoldfastFlavorCatalog |
| T-11 | 128-TC-11 no save mutation | determinism | no save mutation; verify the current owner and its negative boundary without inventing a second authority. | HoldfastFlavorCatalog |
| T-12 | 128-TC-12 host terminal projection | host | host terminal projection; verify the current owner and its negative boundary without inventing a second authority. | HoldfastFlavorCatalog |
| T-13 | 128-TC-13 tone/provenance review | UI/accessibility | tone/provenance review; verify the current owner and its negative boundary without inventing a second authority. | HoldfastFlavorCatalog |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 9 | `Ashfall.Core.Tests/TradeCommandTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `Ashfall.Core.Tests/HoldfastTradeArbitrageTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `src/Main.UiTests.Holdfast.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/Economy/HoldfastFundsTradeTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `src/Host/HoldfastRuntimeSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `src/Host/HoldfastTerminalPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `src/Host/HoldfastDispatchLog.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/HoldfastTradeSessionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Remediation/FollowUpRemediationGateTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/UI/FactionsPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Economy/HoldfastTradeIntegrityTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/HoldfastFactionIdentityContractTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/HoldfastFlavorExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Inventory/UnifiedInventoryOwnershipTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Economy/BlackMarketSettlementService.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/HoldfastCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/HoldfastFactionsCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Host/HoldfastFlavorCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Main.UiTests.RealCampaignJourney.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Economy/Plan211BlackMarketSettlementTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/TradeCreditCoordinatorTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Assets/Ashfall.Core/HoldfastTradeSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Host/BlackMarketSelfTest.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Main.GameFlow.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Main.Holdfast.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/UI/GameHudOverlay.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/Disease/FoodborneExposureBridgeTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/BlackMarketHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/ContentUtilizationRuntimeCollector.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/HoldfastSaveStore.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/HoldfastTradeSaveStore.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/UI/BlackMarketSnapshotFixture.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/UI/CaravanBarterLedgerPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/UI/FactionDetailPanel.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/holdfast_flavor.json`

### `Assets/StreamingAssets/Data/holdfast_flavor.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 8716; characters: 8706.
- SHA-256: `e2daadc29e1a5a7e9c4b5143060ba1df4b48418923166c60f8a9bdac2b9a451d`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `factions`, `items`

#### `factions` — 8 keyed entries

- Entry 001 `faction_the_office`: `{"register":"bureaucratic","rejected":"Requisition denied — the authorising stamp is absent or the ledger balance does not cover the line item.","sold":"Accepted for inventory. The Office records the transfer and adjusts the manifest accor…`
- Entry 002 `faction_the_cutters`: `{"register":"salvage","rejected":"No stock to release and no credit to draw against. The Cutters do not float empty requisitions.","sold":"Taken to the pile. The cutter ledger shifts; your credit moves the other way.","voice":"The Cutters …`
- Entry 003 `faction_the_fleet`: `{"register":"maritime","rejected":"The manifest does not clear. Either the berth is closed or the hold cannot accept the transfer.","sold":"Logged and cleared. The Fleet manifest now shows the item transferred to your custody.","voice":"Th…`
- Entry 004 `faction_black_flotilla`: `{"register":"privateer","rejected":"Claim unacknowledged. The Flotilla does not lower tackle or part with raised stock without verified barter in the net.","sold":"Brought across the gunwale. The dive ledger records the exchange and the de…`
- Entry 005 `faction_supply_corps`: `{"register":"allocation","rejected":"Requisition declined. Your allotment chit lacks valid quota authorization or the district issue window has expired.","sold":"Delivered to the depot cage. The supply tally is marked off and credit is pos…`
- Entry 006 `faction_railway_guild`: `{"register":"logistics","rejected":"Waybill refused. The tonnage exceeds your line credit or the destination switch remains clamped.","sold":"Loaded onto the freight flatcar. The waybill is countersigned and transit credit is logged to you…`
- Entry 007 `faction_hydro_barons`: `{"register":"monopoly","rejected":"Tap closed. No discharge authorized until previous draw accounts are cleared or certified filter stock is provided.","sold":"Poured into the cistern manifold. The intake meter ticks and clean-water allowa…`
- Entry 008 `faction_ordnance_foundry`: `{"register":"foundry","rejected":"Batch declined. The forge cannot accept uncertified scrap or float requisitions against unproved alloy.","sold":"Weighed at the charging dock. The casting tally is notched and munitions credit is struck in…`

#### `items` — 40 keyed entries

- Entry 001 `item_map_sheet_ice_road`: `"The sheet is not a map — it is a promise that the ice will hold long enough to matter."`
- Entry 002 `item_census_return_blank`: `"Blanks are cheap until the clerk demands them filled."`
- Entry 003 `item_order_12c`: `"Reconstruction Order 12-C: stamped, filed, and quietly ignored until the ice opens."`
- Entry 004 `item_allocation_tag`: `"Allocation Tag. What is tagged is owned, and what is owned can be reassigned."`
- Entry 005 `item_triplicate_carbon`: `"Triplicate Carbon. One copy for the Office, one for the file, one for the fire."`
- Entry 006 `item_ice_spike_bar`: `"Ice Spike Bar. Cold iron for cold work. It dulls faster than steel."`
- Entry 007 `item_beacon_oil`: `"Beacon Oil. Burns clean in the lamp but leaves a residue the filters do not catch."`
- Entry 008 `item_cutter_ledger_blank`: `"Cutter Ledger (blank). The Cutters do not trust a ledger with writing already on it."`
- Entry 009 `item_ice_tyre_set`: `"Ice Tyre Set. The road claims rubber faster than it claims drivers."`
- Entry 010 `item_plant_suit_patched`: `"Plant Suit (patched). Every patch is a reminder that the membrane cannot hold forever."`
- Entry 011 `item_resin_gloves`: `"Resin Gloves. The resin seals, the gloves chafe, and both are preferable to bare hands on the line."`
- Entry 012 `item_fume_rag`: `"Fume Rag. Cheap, replaceable, and the only thing between a man and the resin fumes."`
- Entry 013 `item_shift_whistle`: `"Shift Whistle. It does not summon help. It announces that help is expected."`
- Entry 014 `item_work_ticket`: `"Work Ticket. Without it, you are just another body in the yard. With it, you are on the roster."`
- Entry 015 `item_steam_token`: `"Steam Token. Not money, not scrip — a promise that the plant will turn today."`
- Entry 016 `item_block_c_key`: `"Block C Key. It opens one door and signals that the holder has been cleared for what lies beyond."`
- Entry 017 `item_ro_resin`: `"RO Resin. The membrane eats it and the plant drinks it. Stock is measured in drums, not grams."`
- Entry 018 `item_ro_resin_spent`: `"Spent RO Resin. Heavier than it looks and useless until the chemists find a second pass."`
- Entry 019 `item_iodine_crystal`: `"Iodine Crystal. Small enough to hide, strong enough to matter when the plume passes."`
- Entry 020 `item_process_barrel`: `"Process Barrel. Sealed, marked, and heavy. What it contains depends on which shift filled it."`
- Entry 021 `item_schedule_crystal`: `"Schedule Crystal. The Office keeps time on these. Break one and you buy the next clerk a coffee."`
- Entry 022 `item_fleet_pad_copy`: `"Fleet Pad Copy. The Fleet trusts copies more than originals — paper rots, copies multiply."`
- Entry 023 `item_foghorn_key`: `"Foghorn Key. Wound once per shift. The sound carries farther than any radio in the valley."`
- Entry 024 `item_kittiwake_copy`: `"Kittiwake Copy. A route map that predates the ice. The Fleet keeps it for sentimental reasons."`
- Entry 025 `item_weigh_receipt_hf`: `"Weigh Receipt. The bridge does not trust scales; it trusts the stamped receipt from the scales."`
- Entry 026 `item_schedule_sector4_copy`: `"The Other Schedule. Sector 4 runs on a different clock. The Office finds this inconvenient."`
- Entry 027 `item_halvard_kit_notes`: `"Improvised Potable. Not clean, not safe, and better than nothing when the barrel runs dry."`
- Entry 028 `item_sole_unsigned`: `"Filed, Not Signed. The gap between filing and signature is where most corruption lives."`
- Entry 029 `item_playground_seat`: `"One Seat. Cold steel, bolted down. The only furniture in the yard that does not get moved."`
- Entry 030 `item_edor_return_self`: `"Clerk's Own Return. Edor does not file this. Edor keeps it."`
- Entry 031 `item_yara_dark_mark`: `"Dark Mark. Not a brand, not a tattoo — a notation in the clerk's private ledger."`
- Entry 032 `item_leva_minutes_vol12`: `"Volume 12. Leva's minutes run to handwriting, not type. The Clerk respects the volume."`
- Entry 033 `item_hearth4_hatch_log`: `"Hatch Log. Hearth 4. The hatch that opens onto the ice. The log is supposed to be signed."`
- Entry 034 `item_alloc7_ration_tin`: `"ALLOC-7 Tin. The label says ration. The content says starch and regret."`
- Entry 035 `item_cluster_formulary`: `"Human Formulary. Written for Cluster 12, applied wherever the medics have paper and light."`
- Entry 036 `item_foghorn_timer`: `"Foghorn Escapement. The mechanism that turns air into sound. Precision engineering in a salt world."`
- Entry 037 `item_tin_fourteenth`: `"The Fourteenth Plate. The fourteenth allocation that never received its tin. The Office still lists it."`
- Entry 038 `item_salt_rash_salve`: `"Salt-Rash Salve. The air here eats skin. The salve slows the eating."`
- Entry 039 `item_uv_grease`: `"UV Grease. Applied to gaskets and seals. It smells like chemistry and looks like grey paste."`
- Entry 040 `item_electrolyte_salts`: `"Electrolyte Salts. The body demands them; the ration does not supply them."`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/holdfast_factions.json`

### `Assets/StreamingAssets/Data/holdfast_factions.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 6557; characters: 6557.
- SHA-256: `e2f13c2291cba31c05b7b0dbca06dc37bd9bf5d692e51d9c2250c8738bdcbd44`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `actions`

#### `actions` — 9 current rows

- Row 001 `faction_the_office`: `{"access_rule":"Access plus named claims. You cannot conquer them. You can lose the Ice Road and gain a file. Threat is tone, not a seventh Power.","alignment":"conditional","badge_asset_id":"","display_name":"The Office","home_region":"th…`
- Row 002 `faction_the_cutters`: `{"access_rule":"Dark and lit are moral words. Will not guide a column onto ice marked dark. Will not blast. Relight-for-a-trap is Ivy's exception, northern: lamps out over eleven days, access gone.","alignment":"conditional","badge_asset_i…`
- Row 003 `faction_the_fleet`: `{"access_rule":"Waiting for a stand-up that uses the same authentication family as a land pad. Some paper only works on land. Blasting is refused. The Ridge is hours and cold.","alignment":"peaceful","badge_asset_id":"","display_name":"The…`
- Row 004 `faction_black_flotilla`: `{"access_rule":"Flag traffic is hailed, boarded, and priced. Marked salvage claims outrange weapons; a claim tag counts as a signature on the water. Unmarked deep work is theirs, not yours.","alignment":"conditional","badge_asset_id":"fact…`
- Row 005 `faction_supply_corps`: `{"access_rule":"Allocation-office discipline: credit against the shelter's next issue, terms read aloud twice, forfeit named before the goods move. Default is remembered in files, not grudges.","alignment":"conditional","badge_asset_id":""…`
- Row 006 `faction_railway_guild`: `{"access_rule":"Keeps the southern rail open for counterparties in good standing. Lends fuel, parts, and capital equipment at fair rates; default closes the track, and the track stays closed.","alignment":"conditional","badge_asset_id":"",…`
- Row 007 `faction_hydro_barons`: `{"access_rule":"Control the deep aquifer and everything that filters it. No negotiation on rates, no second contract while the first is open, and no exceptions to the embargo that follows a default.","alignment":"conditional","badge_asset_…`
- Row 008 `faction_ordnance_foundry`: `{"access_rule":"Production-line counterparty for ammunition, tools, and protective equipment. Rates are honest because capable shelters are repeat customers; defaults are collected by enforcers, not letters.","alignment":"conditional","bad…`
- Row 009 `faction_scavengers`: `{"access_rule":"Move goods nobody else will touch, against collateral they inventory themselves. Payment in full, on the day, or they take what they are owed from what you have.","alignment":"conditional","badge_asset_id":"","display_name"…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/holdfast_items.json`

### `Assets/StreamingAssets/Data/holdfast_items.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 26102; characters: 26097.
- SHA-256: `431a1c9ec7c22d0ff6efa8a0f4e53ce56be618ab97a5624e82cab0784c8e2f4a`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `items`

#### `items` — 55 current rows

- Row 001 `item_map_sheet_ice_road`: `{"description":"A road that is not there in summer. Waxed. Fingerprints kept. Ostrowski will not say who walked it. The Moth will sell you a contradiction. Ivy will confirm a post, not a road.","displayName":"Ice Road Sheet","hungerRestore…`
- Row 002 `item_census_return_blank`: `{"description":"Pink, yellow, white. White stays with them. Occupancy, occupations, dependents, DOB once. Edor will read it again. A blank in your pack is not anonymity. It is a form that wants names.","displayName":"Census Return (blank)"…`
- Row 003 `item_order_12c`: `{"description":"Unlisted occupants of an authenticated facility constitute a labour reserve. Published. Sector 4's copies died. This one did not. Sole will file and not sign. Voss will want the pool. The ice will carry a column.","displayN…`
- Row 004 `item_allocation_tag`: `{"description":"Paper. Not a plate. Block C guest grammar. Curls. Your name in an Office hand, or a Sector 4 occupation they guessed. Morale when visible in the shelter: some people sleep worse near paper that could become metal.","display…`
- Row 005 `item_triplicate_carbon`: `{"description":"The third copy is the one they keep. Three colours. Introductions by mass. A stolen stack makes the next receipt honest only twice. The Tollman will laugh. Ormund will note.","displayName":"Triplicate Carbon","hungerRestore…`
- Row 006 `item_ice_spike_bar`: `{"description":"A bar for ice that is lying. Harbour steel, worn at the bite. Accident chance down on the Cut if someone who can read dark is holding it. Not a weapon. A question you ask the road.","displayName":"Ice Spike Bar","hungerRest…`
- Row 007 `item_beacon_oil`: `{"description":"Finger-widths to the WINDOW line. Tithe and relight. The measuring-stick in the South Beacon cage is the honest clock. Steal it and the next column writes an accident with your mass.","displayName":"Beacon Oil","hungerResto…`
- Row 008 `item_cutter_ledger_blank`: `{"description":"Date, origin, mass, remarks. Remarks are for the dead. A blank book is not hope. It is capacity. Yara will know if you invent a twelfth filter notch in the same hand.","displayName":"Cutter Ledger (blank)","hungerRestore":0…`
- Row 009 `item_ice_tyre_set`: `{"description":"Without these, the Ice Road is walking. Vehicle component. Speed and accident chance. Not a driving game. A crate of rubber that smells like the Recovery Yard and salt.","displayName":"Ice Tyre Set","hungerRestore":0.0,"id"…`
- Row 010 `item_plant_suit_patched`: `{"description":"Never hazmat. Inner-tube at the knees. Grey canvas, visor clouded from the inside. Salt-rash down, fatigue up. Degrades faster in UV. The patch is a bicycle tube from a year that still had bicycles.","displayName":"Plant Su…`
- Row 011 `item_resin_gloves`: `{"description":"Insides powdered. Outsides glazed. Spent stack handling. Bare hands are how Tuesdays get worse. One pair on the jig is communal. Taking it is a shift decision.","displayName":"Resin Gloves","hungerRestore":0.0,"id":"item_re…`
- Row 012 `item_fume_rag`: `{"description":"Wet it. Don't pretend it is a mask. Chest-height fume in Hall 2. A rag is not a filter. It is the difference between a tour and a shift. Iodine after.","displayName":"Fume Rag","hungerRestore":0.0,"id":"item_fume_rag","mora…`
- Row 013 `item_shift_whistle`: `{"description":"The whistle is the limit. The limit is skin. Enforces outfall hours if someone blows it. Fatigue up because limits are work. Leva will give you one. Children should not think steam is a story.","displayName":"Shift Whistle"…`
- Row 014 `item_work_ticket`: `{"description":"The queue is for this, not bread. Indoor access. A day of labour in a district that inventories you while you work. Steam if the pipe is live. Yellow cultivars if you are on trough duty.","displayName":"Work Ticket","hunger…`
- Row 015 `item_steam_token`: `{"description":"Eight hours of waystation warmth, if the substation agrees. Stamped fibre, not coin. Cluster currency-in-kind. You cannot steal heat. You can steal tokens. The wooden box at the valve house will be light.","displayName":"St…`
- Row 016 `item_block_c_key`: `{"description":"A key for a door with a paper tag. Guest housing. The radiator ticks if the canal is honest. Children's boots in C-214 if you have not taken them yet. Home still ticks without you.","displayName":"Block C Key","hungerRestor…`
- Row 017 `item_ro_resin`: `{"description":"Brine becomes process. Process is not clean. Plant repair. Tuesday's short count. Virgin drums are brown-stencilled and heavy. Heat and iodine still required. District 8 will never make Sector 4 thirst irrelevant.","display…`
- Row 018 `item_ro_resin_spent`: `{"description":"Looks dry. Is not. Sample from Hall 2. Recoat yield low. Toxic to handle. Valuable to people who still believe. Ice crows will not land on the stack.","displayName":"Spent RO Resin","hungerRestore":0.0,"id":"item_ro_resin_s…`
- Row 019 `item_iodine_crystal`: `{"description":"Thyroid and water in the same cage. Bulk. Process column, then thyroid, then clinic. The Office has a key. The Salt has a tea-tin. Lot numbers are Continuity. So is the stamp NOT FOR GENERAL ISSUE.","displayName":"Iodine Cr…`
- Row 020 `item_process_barrel`: `{"description":"Transport. Twenty percent spoilage if the ice lies. Thirst at forty percent if drunk raw. Electrolyte salts after. Haul south and lose some to the Cut. Rebuilders still need tablets. You cannot pipe this to Allocation 12.",…`
- Row 021 `item_schedule_crystal`: `{"description":"The hour, not the order. A crystal that keeps Hearth-4's schedule even when the foghorn is stolen. Hearing is not a stand-up. Mire will say so.","displayName":"Schedule Crystal","hungerRestore":0.0,"id":"item_schedule_cryst…`
- Row 022 `item_fleet_pad_copy`: `{"description":"It does not authenticate. Same family as D/9. Wrong door. Show it to Mire and he will be interested. Interest is not a hatch. Voss cannot conscript a ship with it.","displayName":"Fleet Pad Copy","hungerRestore":0.0,"id":"i…`
- Row 023 `item_foghorn_key`: `{"description":"Winds the spring. Does not decide who is coming. Plinth hook. Companion to the escapement. Cutters navigate by sounding. Silence it to hide and something on the water loses the coast as well.","displayName":"Foghorn Key","h…`
- Row 024 `item_kittiwake_copy`: `{"description":"The log continues eleven days past the Exchange. If the chart was copied. Channel markers versus the sheet versus the Moth. Nomi goes quieter, not warmer. She already knew. She had not been paid.","displayName":"Kittiwake C…`
- Row 025 `item_weigh_receipt_hf`: `{"description":"Introduction — twelve kilograms equivalent. Tollman grammar meeting Office grammar. Honest paper. The destination field may say ESTUARY / SEASONAL. Someone may have written the Salt underneath.","displayName":"Weigh Receipt…`
- Row 026 `item_schedule_sector4_copy`: `{"description":"Every name is legible. Including yours, in a column you were not meant to see. Ribbon copy from Ormund's drawer, or a carbon that travelled. Sole is here. Renn is here. Frayne is not. 12-C is a different folder; this is onl…`
- Row 027 `item_halvard_kit_notes`: `{"description":"His handwriting gets smaller toward the end. The diagrams do not. Field notes from Allocation 12-B. Intake, cloth, iodine, heat, a barrel that was never a plant. Water-craft bonus at the waystation if someone can still read…`
- Row 028 `item_sole_unsigned`: `{"description":"She blotted the date. She did not blot the refusal. 12-C, Drown-stamped, unsigned. D/9 stand-down still works; Fleet pad still does not. Completeness versus execution on one sheet. The blot is ink, not tears. Do not describ…`
- Row 029 `item_playground_seat`: `{"description":"The chain is still there. The brass is in your pack. A swing seat, unscrewed. 1× brass_fittings that everyone notices: Quad, Grade Hut, Allotments board, the tin if you know the tin. Children do not ask. Auditors do. You ca…`
- Row 030 `item_edor_return_self`: `{"description":"The birth year is written twice. Once correctly. Pink copy. Two years. Convoy 12's training example in a living person. If the error is left, he will omit a name for you once and hate it. If struck, Ormund will see the stri…`
- Row 031 `item_yara_dark_mark`: `{"description":"She did not raise her voice. The beacon is dark. A lath with black cloth, or the absence of oil in a cage. Ice Road access destroyed. Thick ice will still be ice. It will not be a road. You cannot talk this back on. Eleven …`
- Row 032 `item_leva_minutes_vol12`: `{"description":"Motion: that we keep running. Carried. Binder, tabs, a failed valve-seat that used to paperweight it. Steam-trip warning six hours early if you keep it in the hall or the waystation. The Office would like a copy. The copy w…`
- Row 033 `item_hearth4_hatch_log`: `{"description":"They logged every refusal. There are a lot of refusals. Clipboard, pouch, dates, reasons: NO STAND-UP / NO NUMBER / BLASTING PARTY — DENIED. Icebreaker without a hundred explosives if a number authenticates. Stealing it doe…`
- Row 034 `item_alloc7_ration_tin`: `{"description":"NOT FOR GENERAL ISSUE. The issue is you. Olive, stencil, frozen rim if opened on the Cut. Food. Morale down if opened in Sector 4, where the stamp is a mirror. Accident 12 still has more. Ice crows know the timetable.","dis…`
- Row 035 `item_cluster_formulary`: `{"description":"Dosage for a species the Verge has been approximating. Bound, pre-war, Clinic-kept. Ianov payoff. Surgery odds. They will not send a copy south unless the levy is honoured. A child's correction on the thyroid plate is in pe…`
- Row 036 `item_foghorn_timer`: `{"description":"It sounds whether anyone is coming or not. Brass clockwork from Foghorn 8. Shelf navigation. If owned, a faint sounding on Silence nights. If taken, Yara loses the coast in fog and so does the tender. Quiet is how columns v…`
- Row 037 `item_tin_fourteenth`: `{"description":"The tin is lighter. Nobody mentions it. Only if you sold nameplates north. One plate missing from the fourteen behind the filtration stack. District 8 paid more than the Works. Still no comment.","displayName":"The Fourteen…`
- Row 038 `item_salt_rash_salve`: `{"description":"Grit in the grease. Soothes. Does not cure. Two finger-scoops gone from the waystation tin. Iodine soothes not cures. The clipboard at the outfall will not thank you.","displayName":"Salt-Rash Salve","hungerRestore":0.0,"id…`
- Row 039 `item_uv_grease`: `{"description":"Albedo is a tax. This is a delay. One expedition of blistering down. Coastal ozone, ice shine. Sun-Seekers will want visors more than grease. Grease is what you have.","displayName":"UV Grease","hungerRestore":0.0,"id":"ite…`
- Row 040 `item_electrolyte_salts`: `{"description":"For people who drank the process. Counters process-water drinking. Leva will still count you as a problem if you skip iodine. Salts are not a membrane.","displayName":"Electrolyte Salts","hungerRestore":0.0,"id":"item_elect…`
- Row 041 `item_canned_food`: `{"description":"Sealed rations from a production run nobody dated. The label is stencil only. What is inside stays inside until the tin is open, and after that it is only food.","displayName":"Canned Food","hungerRestore":0.0,"id":"item_ca…`
- Row 042 `item_fuel`: `{"description":"Clear, stable, screened for water. One jerrycan runs the shelter generator for a day and a night. Nobody asks where refined fuel comes from anymore.","displayName":"Refined Fuel","hungerRestore":0.0,"id":"item_fuel","morale…`
- Row 043 `item_medical_kit`: `{"description":"Gauze, sutures, a tourniquet, and a card of instructions printed for a world with functioning hospitals. Sterile until opened. Wasted on anything less than bleeding.","displayName":"Field Medical Kit","hungerRestore":0.0,"i…`
- Row 044 `item_clean_water`: `{"description":"Twelve-litre containers, dosed and sealed. The dosing slip is signed twice. Water you do not have to think about is the most expensive kind.","displayName":"Purified Water","hungerRestore":0.0,"id":"item_clean_water","moral…`
- Row 045 `item_water_filter`: `{"description":"A ceramic candle filter in a padded crate. Slow, heavy, indifferent to what it is given. The Barons lend these out more often than they sell them.","displayName":"Ceramic Water Filter","hungerRestore":0.0,"id":"item_water_f…`
- Row 046 `item_water_purification_tablets_40_of_40`: `{"description":"A full sleeve, count verified at the counter. Each tablet buys a litre and a little peace of mind. Part sleeves are worth nothing to anyone.","displayName":"Water Purification Tablets (40 of 40)","hungerRestore":0.0,"id":"i…`
- Row 047 `item_diesel_fuel`: `{"description":"Depot diesel, drawn off the bottom of a guarded tank. The Guild marks every jerrycan; an unmarked one invites questions at the next halt.","displayName":"Diesel","hungerRestore":0.0,"id":"item_diesel_fuel","moraleEffect":0.…`
- Row 048 `item_mechanical_parts`: `{"description":"Bearings, seals, pins, and grief, sorted by an apprentice with good eyes. Nothing here is new. Everything here works.","displayName":"Salvaged Mechanical Parts","hungerRestore":0.0,"id":"item_mechanical_parts","moraleEffect…`
- Row 049 `item_engine`: `{"description":"A four-cylinder unit, cracked head welded, compression tested by ear. It starts. The Guild's reserve stock holds three, and they know exactly how many they have.","displayName":"Salvaged Engine","hungerRestore":0.0,"id":"it…`
- Row 050 `item_ammo_762`: `{"description":"Production-line rounds from the Foundry, boxed in fives and tens. Counted out loud at every handover. The Foundry does not lose count.","displayName":"7.62mm Ammunition","hungerRestore":0.0,"id":"item_ammo_762","moraleEffec…`
- Row 051 `item_soldering_kit`: `{"description":"Iron, spool, flux, and a brush, in a cloth roll that closes with one hand. Enough to keep a circuit alive past its appointed death.","displayName":"Soldering Kit","hungerRestore":0.0,"id":"item_soldering_kit","moraleEffect"…`
- Row 052 `item_gas_mask`: `{"description":"Rubber, glass, and a filter canister dated within the season. The Foundry's protective stores issue them by name and want them back in working order.","displayName":"Gas Mask","hungerRestore":0.0,"id":"item_gas_mask","moral…`
- Row 053 `item_dried_rations`: `{"description":"Compressed meal blocks from the Scavenger cache. Weight for weight, more food than a tin, and it never tastes like anything at all.","displayName":"Dried Rations","hungerRestore":0.0,"id":"item_dried_rations","moraleEffect"…`
- Row 054 `item_antibiotics`: `{"description":"Blister packs from a black-market stash, lot numbers filed off. The course is the full course, or it is nothing. The Scavengers do not do instalments.","displayName":"Antibiotics","hungerRestore":0.0,"id":"item_antibiotics"…`
- Row 055 `item_dosimeter`: `{"description":"A pen-style dosimeter, zeroed and charged. It tells you what you already suspect, with numbers. The salvage pile yields them more often than anyone likes to think about.","displayName":"Dosimeter","hungerRestore":0.0,"id":"…`


# Appendix — Current Source Detail: `src/Host/HoldfastDispatchLog.cs`

### `src/Host/HoldfastDispatchLog.cs` — complete current file

- Size: 128 lines / 5288 bytes.
- SHA-256: `eafd43752bd36ca31e37ec99fe9bd099506913a730c991c82312b8c05ab2ae8b`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006:
00007: namespace AtomicWar.GodotApp
00008: {
00009:     /// <summary>
00010:     /// The Quartermaster's Line: in-universe dispatch log triggered by verified
00011:     /// runtime events. Every entry references true state (quantities, values,
00012:     /// faction status). Flavor is an overlay; it never touches save files,
00013:     /// transaction logic, or domain assertions.
00014:     /// </summary>
00015:     public sealed class HoldfastDispatchLog
00016:     {
00017:         private const int MaxEntries = 64;
00018:
00019:         private readonly HoldfastFlavorCatalog _flavor;
00020:         private readonly List<string> _entries = new List<string>();
00021:         private string _lastDispatch = string.Empty;
00022:
00023:         public IReadOnlyList<string> Entries => _entries;
00024:         public string LastDispatch => _lastDispatch;
00025:
00026:         public HoldfastDispatchLog(HoldfastFlavorCatalog flavor)
00027:         {
00028:             _flavor = flavor ?? HoldfastFlavorCatalog.Load(string.Empty);
00029:         }
00030:
00031:         public void OnSessionOpened(string sessionType)
00032:         {
00033:             Emit("Ledger reopened (" + sessionType + "). The quartermaster's desk is ready.");
00034:         }
00035:
00036:         public void OnFirstPurchase(string itemId, int quantity, long totalValue, string factionId)
00037:         {
00038:             var voice = _flavor.GetFactionVoice(factionId);
00039:             Emit("First requisition this session: " + quantity + " × " + ItemRef(itemId) +
00040:                  " for " + totalValue + ". " + voice.voice);
00041:         }
00042:
00043:         public void OnPurchase(string itemId, int quantity, long totalValue, string factionId)
00044:         {
00045:             var voice = _flavor.GetFactionVoice(factionId);
00046:             Emit(quantity + " × " + ItemRef(itemId) + " released. " + totalValue + " deducted. " + voice.voice);
00047:         }
00048:
00049:         public void OnSale(string itemId, int quantity, long totalValue, string factionId)
00050:         {
00051:             var voice = _flavor.GetFactionVoice(factionId);
00052:             Emit(quantity + " × " + ItemRef(itemId) + " accepted. " + totalValue + " credited. " + voice.sold);
00053:         }
00054:
00055:         public void OnHoldingEmptied(string itemId, string factionId)
00056:         {
00057:             var voice = _flavor.GetFactionVoice(factionId);
00058:             Emit("The last " + ItemRef(itemId) + " has left the shelf. " + voice.voice);
00059:         }
00060:
00061:         public void OnStockLow(string itemId, int remaining, string factionId)
00062:         {
00063:             var voice = _flavor.GetFactionVoice(factionId);
00064:             Emit("Stock of " + ItemRef(itemId) + " is now " + remaining + ". " + voice.voice);
00065:         }
00066:
00067:         public void OnStockEmpty(string itemId, string factionId)
00068:         {
00069:             var voice = _flavor.GetFactionVoice(factionId);
00070:             Emit("No holdings of " + ItemRef(itemId) + " remain. " + voice.voice);
00071:         }
00072:
00073:         public void OnRejected(HoldfastTradeResult result, string factionId)
00074:         {
00075:             if (result == null) return;
00076:             var voice = _flavor.GetFactionVoice(factionId);
00077:             string detail = result.Failure switch
00078:             {
00079:                 HoldfastTradeFailure.InvalidQuantity => "Quantity must be at least one.",
00080:                 HoldfastTradeFailure.InsufficientFunds => "Available value is below the listed worth.",
00081:                 HoldfastTradeFailure.InsufficientStock => "The selected counterparty has no stock at that quantity.",
00082:                 HoldfastTradeFailure.InsufficientInventory => "No holdings of this item are available for transfer.",
00083:                 HoldfastTradeFailure.InventoryCapacity => "The inventory cannot hold that quantity.",
00084:                 HoldfastTradeFailure.InvalidPrice => "The listed value cannot be represented safely.",
00085:                 HoldfastTradeFailure.UnknownItem => "The selected item is not in the Holdfast catalog.",
00086:                 HoldfastTradeFailure.UnknownFaction => "No valid Holdfast counterparty is selected.",
00087:                 HoldfastTradeFailure.UnavailableOrRestricted => "This supply remains reserved under current Holdfast restrictions.",
00088:                 _ => "Transaction declined."
00089:             };
00090:
00091:             Emit("Requisition refused: " + detail + " " + voice.rejected);
00092:         }
00093:
00094:         public void OnSaveCommitted(string path)
00095:         {
00096:             Emit("Ledger committed to " + System.IO.Path.GetFileName(path) + ". The old state is sealed.");
00097:         }
00098:
00099:         public void OnReloaded(string path)
00100:         {
00101:             Emit("Ledger reopened from " + System.IO.Path.GetFileName(path) + ". Previous state restored.");
00102:         }
00103:
00104:         public void OnQuarantine(string corruptPath)
00105:         {
00106:             Emit("Corrupt ledger quarantined to " + System.IO.Path.GetFileName(corruptPath) +
00107:                  ". A fresh session has been opened.");
00108:         }
00109:
00110:         public void OnNewLedger()
00111:         {
00112:             Emit("New ledger started. Prior records archived. The desk is clean.");
00113:         }
00114:
00115:         private void Emit(string text)
00116:         {
00117:             _lastDispatch = text;
00118:             _entries.Add(text);
00119:             if (_entries.Count > MaxEntries)
00120:                 _entries.RemoveAt(0);
00121:         }
00122:
00123:         private static string ItemRef(string itemId)
00124:         {
00125:             return string.IsNullOrEmpty(itemId) ? "the item" : itemId.Replace("item_", "");
00126:         }
00127:     }
00128: }
```


# Appendix — Current Source Detail: `src/Host/HoldfastFlavorCatalog.cs`

### `src/Host/HoldfastFlavorCatalog.cs` — complete current file

- Size: 118 lines / 4376 bytes.
- SHA-256: `ade571e9a5b98851c94e94ccecb11d33a4197b1772c4ece6db2c9d15d21cea1e`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS0649
00005: #pragma warning disable CS8618
00006: using Godot;
00007: using Ashfall.Core;
00008:
00009: namespace AtomicWar.GodotApp
00010: {
00011:     /// <summary>
00012:     /// Godot-side flavor overlay keyed by canonical Holdfast item and faction IDs.
00013:     /// Loaded from holdfast_flavor.json at startup. Missing keys fall back to neutral
00014:     /// templates — flavor is an overlay, never a domain dependency.
00015:     /// </summary>
00016:     public sealed class HoldfastFlavorCatalog
00017:     {
00018:         private const string FileName = "holdfast_flavor.json";
00019:
00020:         public Dictionary<string, string> ItemMarginalia { get; } = new Dictionary<string, string>(StringComparer.Ordinal);
00021:         public Dictionary<string, FactionVoice> FactionVoices { get; } = new Dictionary<string, FactionVoice>(StringComparer.Ordinal);
00022:
00023:         public string GetItemMarginalia(string itemId)
00024:         {
00025:             if (itemId == null) return NeutralItemMarginalia;
00026:             return ItemMarginalia.TryGetValue(itemId, out string text) && !string.IsNullOrEmpty(text)
00027:                 ? text
00028:                 : NeutralItemMarginalia;
00029:         }
00030:
00031:         public FactionVoice GetFactionVoice(string factionId)
00032:         {
00033:             if (factionId == null) return NeutralFactionVoice;
00034:             return FactionVoices.TryGetValue(factionId, out FactionVoice voice) ? voice : NeutralFactionVoice;
00035:         }
00036:
00037:         public static HoldfastFlavorCatalog Load(string dataDirectory, ILog log = null!)
00038:         {
00039:             log ??= new GodotLog();
00040:             var catalog = new HoldfastFlavorCatalog();
00041:             if (string.IsNullOrEmpty(dataDirectory))
00042:             {
00043:                 log.Warn("[Flavor] dataDirectory is null; flavor catalog is empty.");
00044:                 return catalog;
00045:             }
00046:
00047:             string path = System.IO.Path.Combine(dataDirectory, FileName);
00048:             if (!System.IO.File.Exists(path))
00049:             {
00050:                 log.Warn("[Flavor] " + FileName + " not found at " + path + "; flavor catalog is empty.");
00051:                 return catalog;
00052:             }
00053:
00054:             try
00055:             {
00056:                 string json = System.IO.File.ReadAllText(path);
00057:                 var serializer = new SystemTextJsonSerializer();
00058:                 var root = serializer.Deserialize<FlavorRoot>(json);
00059:                 if (root == null)
00060:                 {
00061:                     log.Warn("[Flavor] " + FileName + " deserialized to null.");
00062:                     return catalog;
00063:                 }
00064:
00065:                 if (root.factions != null)
00066:                 {
00067:                     foreach (var kv in root.factions)
00068:                     {
00069:                         if (!string.IsNullOrEmpty(kv.Key) && kv.Value != null)
00070:                             catalog.FactionVoices[kv.Key] = kv.Value;
00071:                     }
00072:                 }
00073:
00074:                 if (root.items != null)
00075:                 {
00076:                     foreach (var kv in root.items)
00077:                     {
00078:                         if (!string.IsNullOrEmpty(kv.Key) && !string.IsNullOrEmpty(kv.Value))
00079:                             catalog.ItemMarginalia[kv.Key] = kv.Value;
00080:                     }
00081:                 }
00082:
00083:                 log.Info("[Flavor] Loaded " + catalog.ItemMarginalia.Count + " item marginalia, " +
00084:                          catalog.FactionVoices.Count + " faction voices.");
00085:             }
00086:             catch (Exception e)
00087:             {
00088:                 log.Error("[Flavor] Failed to load " + FileName + ": " + e.Message);
00089:             }
00090:
00091:             return catalog;
00092:         }
00093:
00094:         public const string NeutralItemMarginalia = "No marginalia on file for this item.";
00095:
00096:         public static readonly FactionVoice NeutralFactionVoice = new FactionVoice
00097:         {
00098:             register = "neutral",
00099:             voice = "The counterparty has no recorded voice.",
00100:             rejected = "Transaction declined.",
00101:             sold = "Item accepted."
00102:         };
00103:
00104:         public sealed class FactionVoice
00105:         {
00106:             public string register = "neutral";
00107:             public string voice = string.Empty;
00108:             public string rejected = string.Empty;
00109:             public string sold = string.Empty;
00110:         }
00111:
00112:         private sealed class FlavorRoot
00113:         {
00114:             public Dictionary<string, FactionVoice> factions;
00115:             public Dictionary<string, string> items;
00116:         }
00117:     }
00118: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/HoldfastTradeSession.cs`

### `Assets/Ashfall.Core/HoldfastTradeSession.cs` — bounded current excerpt (1051 of 1130 lines)

- Size: 1130 lines / 51598 bytes.
- SHA-256: `68e3c2b379443e5c780d7aba9b7cdf430a9e95892f44278816ca6412ebc49708`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using Ashfall.Core.Inventory;
00006: using Ashfall.Core.PlayerCommand;
00007:
00008: namespace Ashfall.Core
00009: {
00010:     public enum HoldfastFactionStance
00011:     {
00012:         Allied = 0,
00013:         Neutral = 1,
00014:         Hostile = 2,
00015:         Embargoed = 3
00016:     }
00017:
00018:     public enum HoldfastTradeFailure
00019:     {
00020:         None,
00021:         InvalidQuantity,
00022:         InsufficientFunds,
00023:         InsufficientStock,
00024:         InsufficientInventory,
00025:         UnknownItem,
00026:         UnknownFaction,
00027:         UnavailableOrRestricted,
00028:         InventoryCapacity,
00029:         InvalidPrice,
00030:         Embargoed
00031:     }
00032:
00033:     public sealed class HoldfastTradeInventorySlot
00034:     {
00035:         public HoldfastItemDefinition Item { get; }
00036:         public int Amount { get; }
00037:
00038:         public HoldfastTradeInventorySlot(HoldfastItemDefinition item, int amount)
00039:         {
00040:             Item = item;
00041:             Amount = amount;
00042:         }
00043:     }
00044:
00045:     public sealed class HoldfastTradeInventory
00046:     {
00047:         public int Capacity { get; set; } = 20;
00048:         public float MaxWeight { get; set; } = 100f;
00049:         private readonly Dictionary<string, int> _items = new Dictionary<string, int>(StringComparer.Ordinal);
00050:         private readonly HoldfastCatalog? _catalog;
00051:         private readonly Inventory.Inventory? _backingInventory;
00052:
00053:         public HoldfastTradeInventory(HoldfastCatalog? catalog = null, Inventory.Inventory? backingInventory = null)
00054:         {
00055:             _catalog = catalog;
00056:             _backingInventory = backingInventory;
00057:         }
00058:
00059:         public int OccupiedCount => _backingInventory != null ? _backingInventory.Slots.Count : _items.Count;
00060:         public IReadOnlyDictionary<string, int> Items
00061:         {
00062:             get
00063:             {
00064:                 if (_backingInventory != null)
00065:                 {
00066:                     var map = new Dictionary<string, int>(StringComparer.Ordinal);
00067:                     for (int i = 0; i < _backingInventory.Slots.Count; i++)
00068:                     {
00069:                         var s = _backingInventory.Slots[i];
00070:                         if (s?.Item != null && s.Amount > 0)
00071:                         {
00072:                             map.TryGetValue(s.Item.id, out int cur);
00073:                             map[s.Item.id] = cur + s.Amount;
00074:                         }
00075:                     }
00076:                     return map;
00077:                 }
00078:                 return _items;
00079:             }
00080:         }
00081:
00082:         public IReadOnlyList<HoldfastTradeInventorySlot> Slots
00083:         {
00084:             get
00085:             {
00086:                 var list = new List<HoldfastTradeInventorySlot>();
00087:                 if (_backingInventory != null)
00088:                 {
00089:                     for (int i = 0; i < _backingInventory.Slots.Count; i++)
00090:                     {
00091:                         var s = _backingInventory.Slots[i];
00092:                         if (s?.Item != null && s.Amount > 0)
00093:                         {
00094:                             var def = _catalog?.GetItem(s.Item.id) ?? new HoldfastItemDefinition(s.Item.id, s.Item.displayName ?? s.Item.id, "", 1f, s.Item.weight);
00095:                             list.Add(new HoldfastTradeInventorySlot(def, s.Amount));
00096:                         }
00097:                     }
00098:                     return list;
00099:                 }
00100:
00101:                 foreach (var pair in _items)
00102:                 {
00103:                     if (pair.Value <= 0) continue;
00104:                     var def = _catalog?.GetItem(pair.Key) ?? new HoldfastItemDefinition(pair.Key, pair.Key, "", 1f, 1f);
00105:                     list.Add(new HoldfastTradeInventorySlot(def, pair.Value));
00106:                 }
00107:                 return list;
00108:             }
00109:         }
00110:
00111:         public float GetCurrentWeight()
00112:         {
00113:             if (_backingInventory != null)
00114:                 return _backingInventory.GetCurrentWeight();
00115:
00116:             float total = 0f;
00117:             foreach (var pair in _items)
00118:             {
00119:                 if (pair.Value <= 0) continue;
00120:                 var def = _catalog?.GetItem(pair.Key);
00121:                 float unitWeight = def != null ? def.Weight : 1f;
00122:                 total += unitWeight * pair.Value;
00123:             }
00124:             return total;
00125:         }
00126:
00127:         private ItemDefinition CreateItemDefinition(string canonicalId)
00128:         {
00129:             var def = _catalog?.GetItem(canonicalId);
00130:             return new ItemDefinition
00131:             {
00132:                 id = canonicalId,
00133:                 displayName = def?.DisplayName ?? canonicalId,
00134:                 description = def?.Description ?? string.Empty,
00135:                 type = ParseHoldfastItemType(def?.Type),
00136:                 stackMax = def != null && def.StackMax > 0 ? def.StackMax : (def != null && (def.Type == "gear" || def.Type == "equipment") ? 1 : 99),
00137:                 weight = def?.Weight ?? 1f,
00157:         }
00158:
00159:         public bool CanAdd(string itemId, int count)
00160:         {
00161:             if (string.IsNullOrEmpty(itemId) || count <= 0) return false;
00162:             string canonical = ItemAliases.ToCanonical(itemId);
00163:             if (_backingInventory != null)
00164:             {
00165:                 var itemDef = CreateItemDefinition(canonical);
00166:                 return _backingInventory.CanAdd(itemDef, count);
00167:             }
00168:             float currentWeight = GetCurrentWeight();
00169:             var def = _catalog?.GetItem(canonical);
00170:             float unitWeight = def?.Weight ?? 1f;
00171:             if (MaxWeight > 0f && currentWeight + unitWeight * count > MaxWeight) return false;
00172:             _items.TryGetValue(canonical, out int currentCount);
00173:             if ((long)currentCount + count > int.MaxValue) return false;
00174:             if (currentCount == 0 && OccupiedCount >= Capacity) return false;
00175:             return true;
00176:         }
00177:
00178:         public bool AddItem(string itemId, int count)
00179:         {
00180:             if (string.IsNullOrEmpty(itemId) || count <= 0) return false;
00181:             string canonical = ItemAliases.ToCanonical(itemId);
00182:             if (!CanAdd(canonical, count)) return false;
00183:
00184:             if (_backingInventory != null)
00185:             {
00186:                 var itemDef = CreateItemDefinition(canonical);
00187:                 return _backingInventory.Add(itemDef, count);
00188:             }
00189:
00190:             _items.TryGetValue(canonical, out int existing);
00191:             _items[canonical] = existing + count;
00192:             return true;
00193:         }
00194:
00195:         public void RemoveItem(string itemId, int count)
00196:         {
00197:             if (string.IsNullOrEmpty(itemId) || count <= 0) return;
00198:             string canonical = ItemAliases.ToCanonical(itemId);
00199:             if (_backingInventory != null)
00200:             {
00201:                 _backingInventory.RemoveById(canonical, count);
00202:                 return;
00203:             }
00204:             if (_items.TryGetValue(canonical, out int existing))
00205:             {
00206:                 if (existing <= count) _items.Remove(canonical);
00207:                 else _items[canonical] = existing - count;
00208:             }
00209:         }
00210:
00211:         public bool HasSufficient(string itemId, int count)
00212:         {
00213:             if (string.IsNullOrEmpty(itemId) || count <= 0) return true;
00214:             string canonical = ItemAliases.ToCanonical(itemId);
00215:             if (_backingInventory != null)
00216:                 return _backingInventory.CountById(canonical) >= count;
00217:             return _items.TryGetValue(canonical, out int existing) && existing >= count;
00218:         }
00219:
00220:         public bool ValidateBill(IReadOnlyDictionary<string, int> bill)
00221:         {
00222:             if (bill == null || bill.Count == 0) return true;
00223:             foreach (var kv in bill)
00224:             {
00225:                 if (kv.Value <= 0) continue;
00226:                 string canonical = ItemAliases.ToCanonical(kv.Key);
00227:                 if (_backingInventory != null)
00228:                 {
00229:                     if (_backingInventory.CountById(canonical) < kv.Value) return false;
00230:                 }
00231:                 else
00232:                 {
00233:                     if (!_items.TryGetValue(canonical, out int existing) || existing < kv.Value)
00234:                         return false;
00235:                 }
00236:             }
00237:             return true;
00238:         }
00239:
00240:         public bool TryConsumeBill(IReadOnlyDictionary<string, int> bill, Action? onCommitted = null)
00241:         {
00242:             if (!ValidateBill(bill)) return false;
00243:
00244:             if (_backingInventory != null)
00245:             {
00246:                 return _backingInventory.TryConsumeBill(bill, onCommitted);
00247:             }
00248:
00249:             // Standalone fallback: take a backup snapshot for rollback
00250:             var snapshot = new Dictionary<string, int>(_items, StringComparer.Ordinal);
00251:             try
00252:             {
00253:                 if (bill != null)
00254:                 {
00259:                 }
00260:
00261:                 if (onCommitted != null)
00262:                 {
00263:                     onCommitted();
00264:                 }
00265:
00266:                 return true;
00267:             }
00275:         }
00276:
00277:         public void Clear()
00278:         {
00279:             _items.Clear();
00280:             _backingInventory?.Clear();
00281:         }
00283:
00284:     /// <summary>Holdfast trade-state snapshot carried by the terminal's Buy/Sell surface.</summary>
00285:     public sealed class HoldfastTradeResult
00286:     {
00287:         public bool Success { get; set; }
00288:         public string ItemId { get; set; } = string.Empty;
00289:         public int Quantity { get; set; }
00290:         public string FactionId { get; set; } = string.Empty;
00291:         public int TotalValue { get; set; }
00292:         public string Message { get; set; } = string.Empty;
00293:         public string WhyLine { get; set; } = string.Empty;
00294:         public HoldfastTradeFailure Failure { get; set; } = HoldfastTradeFailure.None;
00295:
00296:         /// <summary>
00297:         /// UNBLOCK-02 F13-D: Additive funds delta and failure for funds-denominated legs.
00298:         /// </summary>
00299:         public int FundsDelta { get; set; }
00300:         public Economy.FundsFailure FundsFailure { get; set; } = Economy.FundsFailure.None;
00301:
00302:         public static HoldfastTradeResult Ok(string itemId, int quantity, string factionId, int totalValue, string whyLine = "", int fundsDelta = 0)
00303:             => new HoldfastTradeResult { Success = true, ItemId = itemId, Quantity = quantity, FactionId = factionId, TotalValue = totalValue, Message = "Trade completed.", WhyLine = whyLine, FundsDelta = fundsDelta };
00304:         public static HoldfastTradeResult Fail(string message, HoldfastTradeFailure failure = HoldfastTradeFailure.None, Economy.FundsFailure fundsFailure = Economy.FundsFailure.None)
00305:             => new HoldfastTradeResult { Success = false, Message = message, Failure = failure, FundsFailure = fundsFailure };
00306:     }
00307:
00308:     /// <summary>Engine-agnostic mutable trade state (inventory depth / faction stock).
00309:     /// Terminal-facing; deterministic; host calls Buy/Sell.</summary>
00310:     public sealed class HoldfastTradeSession
00311:     {
00312:         public const int DefaultInventoryCapacity = 20;
00313:         private readonly HoldfastCatalog _catalog = null!;
00314:         private readonly Inventory.Inventory? _playerInventory;
00315:         private readonly Dictionary<string, int> _held = new Dictionary<string, int>(StringComparer.Ordinal);
00316:         private readonly Dictionary<string, int> _stock = new Dictionary<string, int>(StringComparer.Ordinal);
00317:         private long _value;
00318:         private readonly long _initialValue;
00319:
00320:         public HoldfastTradeInventory Inventory { get; }
00321:         public Inventory.Inventory? PlayerInventory => _playerInventory;
00322:         public string SelectedFactionId { get; private set; } = string.Empty;
00323:
00324:         /// <summary>
00325:         /// Canonical embargo authority hook, bound by the host as
00326:         /// factionId → embargoed. When set, a suspended counterparty refuses
00327:         /// both directions of trade; credit eligibility queries the same
00328:         /// ledger, so credit can never bypass what trade cannot.
00329:         /// </summary>
00330:         public Func<string, bool>? EmbargoQuery { get; set; }
00331:
00332:         /// <summary>
00333:         /// Faction stance query delegate. When set, modulates buy and sell prices
00334:         /// and supplies contextual why-lines. Defaults to Neutral if null.
00335:         /// </summary>
00336:         public Func<string, HoldfastFactionStance>? StanceQuery { get; set; }
00337:
00338:         public event Action StateChanged;
00339:
00340:         public HoldfastTradeSession(HoldfastCatalog catalog, long startingValue = 100, Inventory.Inventory? playerInventory = null)
00341:         {
00342:             _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
00343:             _playerInventory = playerInventory;
00344:             Inventory = new HoldfastTradeInventory(_catalog, _playerInventory);
00345:             _value = startingValue;
00346:             _initialValue = startingValue;
00347:             InitializeDefaultStocks();
00348:         }
00350:         private void InitializeDefaultStocks()
00351:         {
00352:             if (_catalog?.Items != null)
00353:             {
00354:                 foreach (var item in _catalog.Items.Items)
00355:                 {
00356:                     _stock[item.Id] = 20;
00357:                 }
00358:             }
00359:         }
00360:
00361:         public bool SelectFaction(string factionId)
00362:         {
00363:             if (string.IsNullOrEmpty(factionId) || _catalog?.GetFaction(factionId) == null)
00364:             {
00365:                 if (factionId == "faction_the_office" || factionId == "faction_the_tempest" || factionId == "faction_the_fleet")
00366:                 {
00367:                     SelectedFactionId = factionId;
00368:                     StateChanged?.Invoke();
00369:                     return true;
00370:                 }
00371:                 return false;
00372:             }
00373:             SelectedFactionId = factionId;
00374:             StateChanged?.Invoke();
00375:             return true;
00376:         }
00377:
00378:         public void SeedInventory(string itemId, int count)
00379:         {
00380:             if (string.IsNullOrEmpty(itemId) || count <= 0) return;
00381:             string canonical = ItemAliases.ToCanonical(itemId);
00382:             int held = GetHeld(canonical);
00383:             if ((long)held + count > int.MaxValue || !Inventory.AddItem(canonical, count)) return;
00384:             if (_playerInventory == null)
00385:                 _held[canonical] = held + count;
00386:             StateChanged?.Invoke();
00387:         }
00388:
00389:         public void ResetToDefaults()
00390:         {
00391:             _held.Clear();
00392:             _stock.Clear();
00393:             Inventory.Clear();
00398:
00399:         /// <summary>Player-held quantity of an item id (0 when never held).</summary>
00400:         public int GetHeld(string itemId)
00401:         {
00402:             if (string.IsNullOrEmpty(itemId)) return 0;
00403:             string canonical = ItemAliases.ToCanonical(itemId);
00404:             if (_playerInventory != null)
00405:             {
00406:                 return _playerInventory.CountById(canonical);
00407:             }
00408:             return _held.TryGetValue(canonical, out int h) ? h : (_held.TryGetValue(itemId, out int raw) ? raw : 0);
00409:         }
00410:
00411:         /// <summary>Faction stock for an item id (0 = none, 1 = low marker).</summary>
00412:         public int GetStock(string itemId)
00413:         {
00414:             string canonical = ItemAliases.ToCanonical(itemId);
00415:             return _stock.TryGetValue(canonical, out int s) ? s : (_stock.TryGetValue(itemId ?? string.Empty, out int raw) ? raw : 0);
00416:         }
00417:
00418:         public void SetStock(string itemId, int count)
00419:         {
00420:             if (!string.IsNullOrEmpty(itemId))
00421:             {
00422:                 string canonical = ItemAliases.ToCanonical(itemId);
00423:                 _stock[canonical] = Math.Max(0, count);
00424:             }
00425:         }
00426:
00427:         public long Value => _value;
00428:         public long PlayerValue => _value;
00429:
00430:         /// <summary>
00431:         /// True when the canonical Holdfast wallet can cover an external
00432:         /// settlement without becoming negative. This is a read-only query.
00433:         /// </summary>
00434:         public bool CanDebitValue(long amount) => amount >= 0 && amount <= _value;
00435:
00436:         /// <summary>True when crediting the canonical wallet cannot overflow.</summary>
00437:         public bool CanCreditValue(long amount) =>
00438:             amount >= 0 && amount <= long.MaxValue - _value;
00439:
00440:         /// <summary>
00441:         /// Atomically debit the existing Holdfast wallet and execute an
00442:         /// optional second transaction leg. A false or throwing second leg
00443:         /// restores the wallet before returning. No parallel currency state
00444:         /// is introduced.
00445:         /// </summary>
00446:         public bool TryDebitValue(long amount, Func<bool>? secondLeg = null) =>
00447:             TryAdjustExternalValue(-amount, amount, secondLeg, notifyOnSuccess: true);
00448:
00449:         /// <summary>
00450:         /// Atomically credit the existing Holdfast wallet and execute an
00451:         /// optional second transaction leg. A false or throwing second leg
00452:         /// restores the wallet before returning.
00453:         /// </summary>
00454:         public bool TryCreditValue(long amount, Func<bool>? secondLeg = null) =>
00455:             TryAdjustExternalValue(amount, amount, secondLeg, notifyOnSuccess: true);
00456:
00457:         internal bool TryDebitValueForSettlement(long amount, Func<bool>? secondLeg = null) =>
00458:             TryAdjustExternalValue(-amount, amount, secondLeg, notifyOnSuccess: false);
00459:
00460:         internal bool TryCreditValueForSettlement(long amount, Func<bool>? secondLeg = null) =>
00461:             TryAdjustExternalValue(amount, amount, secondLeg, notifyOnSuccess: false);
00462:
00463:         internal void NotifyExternalValueSettlement() => StateChanged?.Invoke();
00464:
00465:         private bool TryAdjustExternalValue(long delta, long absoluteAmount,
00466:             Func<bool>? secondLeg, bool notifyOnSuccess)
00467:         {
00468:             if (absoluteAmount < 0) return false;
00469:             if (delta < 0 && !CanDebitValue(absoluteAmount)) return false;
00470:             if (delta >= 0 && !CanCreditValue(absoluteAmount)) return false;
00471:
00472:             long previousValue = _value;
00473:             _value += delta;
00474:             try
00475:             {
00476:                 if (secondLeg != null && !secondLeg())
00477:                 {
00478:                     _value = previousValue;
00479:                     return false;
00480:                 }
00486:             }
00487:
00488:             if (notifyOnSuccess) StateChanged?.Invoke();
00489:             return true;
00490:         }
00491:
00492:         public IReadOnlyDictionary<string, int> Held
00493:         {
00494:             get
00495:             {
00496:                 if (_playerInventory != null)
00497:                 {
00498:                     var map = new Dictionary<string, int>(StringComparer.Ordinal);
00499:                     for (int i = 0; i < _playerInventory.Slots.Count; i++)
00500:                     {
00501:                         var s = _playerInventory.Slots[i];
00502:                         if (s?.Item != null && s.Amount > 0)
00503:                         {
00504:                             string canonical = ItemAliases.ToCanonical(s.Item.id);
00505:                             map.TryGetValue(canonical, out int cur);
00506:                             map[canonical] = cur + s.Amount;
00507:                         }
00508:                     }
00509:                     return map;
00510:                 }
00513:         }
00514:
00515:         public bool TryGetUnitValue(string itemId, out long unitValue)
00516:         {
00517:             string canonical = ItemAliases.ToCanonical(itemId);
00518:             var def = _catalog?.GetItem(canonical) ?? _catalog?.GetItem(itemId);
00519:             if (def != null)
00520:             {
00521:                 unitValue = Math.Max(1, (long)def.TradeValue);
00522:                 return true;
00523:             }
00524:             unitValue = 0;
00525:             return false;
00526:         }
00527:
00528:         public long GetBuyPrice(string itemId, string factionId, int quantity = 1)
00529:         {
00530:             if (quantity <= 0) return 0;
00531:             string canonical = ItemAliases.ToCanonical(itemId);
00532:             var def = _catalog?.GetItem(canonical) ?? _catalog?.GetItem(itemId);
00533:             if (def == null) return 0;
00534:
00535:             int baseVal = Math.Max(1, (int)def.TradeValue);
00536:             var stance = StanceQuery?.Invoke(factionId) ?? HoldfastFactionStance.Neutral;
00537:             float mult = stance switch
00538:             {
00539:                 HoldfastFactionStance.Allied => 0.85f,
00540:                 HoldfastFactionStance.Hostile => 1.25f,
00541:                 _ => 1.0f
00542:             };
00543:             long unitPrice = Math.Max(1, (long)Math.Round(baseVal * mult));
00544:             return unitPrice * quantity;
00545:         }
00546:
00547:         public long GetSellPrice(string itemId, string factionId, int quantity = 1)
00548:         {
00549:             if (quantity <= 0) return 0;
00550:             string canonical = ItemAliases.ToCanonical(itemId);
00551:             var def = _catalog?.GetItem(canonical) ?? _catalog?.GetItem(itemId);
00552:             if (def == null) return 0;
00553:
00554:             int baseVal = Math.Max(1, (int)def.TradeValue);
00555:             var stance = StanceQuery?.Invoke(factionId) ?? HoldfastFactionStance.Neutral;
00556:             float mult = stance switch
00557:             {
00558:                 HoldfastFactionStance.Allied => 1.15f,
00559:                 HoldfastFactionStance.Hostile => 0.75f,
00560:                 _ => 1.0f
00561:             };
00562:             long unitPrice = Math.Max(1, (long)Math.Round(baseVal * mult));
00563:             return unitPrice * quantity;
00564:         }
00565:
00566:         public string GetWhyLine(string itemId, string factionId, bool isBuy)
00567:         {
00568:             string canonical = ItemAliases.ToCanonical(itemId);
00569:             var stance = StanceQuery?.Invoke(factionId) ?? HoldfastFactionStance.Neutral;
00570:             var parts = new List<string>();
00571:
00572:             if (isBuy)
00573:             {
00574:                 if (stance == HoldfastFactionStance.Allied)
00575:                     parts.Add("[Allied discount applied]");
00576:                 else if (stance == HoldfastFactionStance.Hostile)
00577:                     parts.Add($"[Hostile surcharge — {factionId} stance]");
00578:             }
00579:             else
00580:             {
00581:                 if (stance == HoldfastFactionStance.Allied)
00582:                     parts.Add("[Allied bonus applied]");
00583:                 else if (stance == HoldfastFactionStance.Hostile)
00584:                     parts.Add($"[Hostile penalty — {factionId} stance]");
00585:             }
00586:
00587:             int stock = GetStock(canonical);
00588:             if (stock < 3)
00589:                 parts.Add("[Stock critical — limited availability]");
00590:             else if (stock < 8)
00591:                 parts.Add("[Stock low]");
00594:         }
00595:
00596:         public HoldfastTradeResult Buy(string itemId, int quantity, string factionId)
00597:         {
00598:             string canonical = ItemAliases.ToCanonical(itemId);
00599:             var def = _catalog?.GetItem(canonical) ?? _catalog?.GetItem(itemId);
00600:             if (string.IsNullOrEmpty(canonical) || def == null)
00601:                 return HoldfastTradeResult.Fail("Unknown item: " + itemId, HoldfastTradeFailure.UnknownItem);
00602:
00603:             if (!string.IsNullOrEmpty(factionId) && factionId != "none")
00604:             {
00605:                 if (factionId == "faction_nonexistent" || (_catalog?.GetFaction(factionId) == null && factionId != "faction_the_office" && factionId != "faction_the_tempest"))
00606:                     return HoldfastTradeResult.Fail("Unknown faction: " + factionId, HoldfastTradeFailure.UnknownFaction);
00607:
00608:                 if (factionId == "faction_the_fleet")
00609:                     return HoldfastTradeResult.Fail("Unavailable or restricted counterparty: " + factionId, HoldfastTradeFailure.UnavailableOrRestricted);
00610:             }
00611:
00612:             if (!string.IsNullOrEmpty(factionId) && factionId != "none" && EmbargoQuery != null && EmbargoQuery(factionId))
00613:                 return HoldfastTradeResult.Fail("Trade with this faction is suspended (embargo).", HoldfastTradeFailure.Embargoed);
00614:
00615:             if (quantity <= 0)
00616:                 return HoldfastTradeResult.Fail("Quantity must be at least 1.", HoldfastTradeFailure.InvalidQuantity);
00617:
00618:             int currentStock = GetStock(canonical);
00619:             if (currentStock < quantity)
00620:                 return HoldfastTradeResult.Fail("Insufficient merchant stock.", HoldfastTradeFailure.InsufficientStock);
00621:
00622:             long costLong = GetBuyPrice(canonical, factionId, quantity);
00623:             if (!TryGetTradeTotal(costLong, out int cost))
00624:                 return HoldfastTradeResult.Fail("Trade price is outside the supported range.", HoldfastTradeFailure.InvalidPrice);
00625:             if (cost > _value)
00626:                 return HoldfastTradeResult.Fail("Insufficient funds.", HoldfastTradeFailure.InsufficientFunds);
00627:
00628:             if (!Inventory.CanAdd(canonical, quantity))
00629:             {
00630:                 float unitWeight = def.Weight;
00631:                 if (Inventory.MaxWeight > 0f && Inventory.GetCurrentWeight() + unitWeight * quantity > Inventory.MaxWeight)
00632:                     return HoldfastTradeResult.Fail("Inventory weight limit exceeded.", HoldfastTradeFailure.InventoryCapacity);
00633:                 return HoldfastTradeResult.Fail("Inventory capacity reached.", HoldfastTradeFailure.InventoryCapacity);
00634:             }
00635:
00636:             long prevValue = _value;
00637:             int prevStock = currentStock;
00638:             int prevHeld = GetHeld(canonical);
00639:
00640:             bool added = Inventory.AddItem(canonical, quantity);
00641:             if (!added)
00642:                 return HoldfastTradeResult.Fail("Inventory capacity reached.", HoldfastTradeFailure.InventoryCapacity);
00643:
00644:             try
00645:             {
00646:                 _value -= cost;
00647:                 _stock[canonical] = currentStock - quantity;
00648:                 if (_playerInventory == null)
00649:                 {
00650:                     _held[canonical] = prevHeld + quantity;
00651:                 }
00652:                 StateChanged?.Invoke();
00653:                 string whyLine = GetWhyLine(canonical, factionId, true);
00654:                 return HoldfastTradeResult.Ok(canonical, quantity, factionId, cost, whyLine);
00655:             }
00656:             catch (Exception ex)
00657:             {
00658:                 Inventory.RemoveItem(canonical, quantity);
00659:                 _value = prevValue;
00660:                 _stock[canonical] = prevStock;
00661:                 if (_playerInventory == null)
00662:                 {
00663:                     _held[canonical] = prevHeld;
00664:                 }
00665:                 return HoldfastTradeResult.Fail("Trade transaction failed: " + ex.Message, HoldfastTradeFailure.InvalidPrice);
00666:             }
00667:         }
00668:
00669:         public HoldfastTradeResult Sell(string itemId, int quantity, string factionId)
00670:         {
00671:             string canonical = ItemAliases.ToCanonical(itemId);
00672:             var def = _catalog?.GetItem(canonical) ?? _catalog?.GetItem(itemId);
00673:             if (string.IsNullOrEmpty(canonical) || def == null)
00674:                 return HoldfastTradeResult.Fail("Unknown item: " + itemId, HoldfastTradeFailure.UnknownItem);
00675:
00676:             if (!string.IsNullOrEmpty(factionId) && factionId != "none")
00677:             {
00678:                 if (factionId == "faction_nonexistent" || (_catalog?.GetFaction(factionId) == null && factionId != "faction_the_office" && factionId != "faction_the_tempest"))
00679:                     return HoldfastTradeResult.Fail("Unknown faction: " + factionId, HoldfastTradeFailure.UnknownFaction);
00680:
00681:                 if (factionId == "faction_the_fleet")
00682:                     return HoldfastTradeResult.Fail("Unavailable or restricted counterparty: " + factionId, HoldfastTradeFailure.UnavailableOrRestricted);
00683:             }
00684:
00685:             if (!string.IsNullOrEmpty(factionId) && factionId != "none" && EmbargoQuery != null && EmbargoQuery(factionId))
00686:                 return HoldfastTradeResult.Fail("Trade with this faction is suspended (embargo).", HoldfastTradeFailure.Embargoed);
00687:
00688:             if (quantity <= 0)
00689:                 return HoldfastTradeResult.Fail("Quantity must be at least 1.", HoldfastTradeFailure.InvalidQuantity);
00690:
00691:             int currentlyHeld = GetHeld(canonical);
00692:             if (currentlyHeld < quantity)
00693:                 return HoldfastTradeResult.Fail("Insufficient player inventory.", HoldfastTradeFailure.InsufficientInventory);
00694:
00695:             long gainLong = GetSellPrice(canonical, factionId, quantity);
00696:             if (!TryGetTradeTotal(gainLong, out int gain) || !CanCreditValue(gain))
00697:                 return HoldfastTradeResult.Fail("Trade proceeds exceed the supported balance.", HoldfastTradeFailure.InvalidPrice);
00698:             if ((long)GetStock(canonical) + quantity > int.MaxValue)
00699:                 return HoldfastTradeResult.Fail("Merchant stock capacity reached.", HoldfastTradeFailure.InventoryCapacity);
00700:             _value += gain;
00701:             _held[canonical] = currentlyHeld - quantity;
00702:             _stock[canonical] = GetStock(canonical) + quantity;
00703:             Inventory.RemoveItem(canonical, quantity);
00704:             StateChanged?.Invoke();
00705:             string whyLine = GetWhyLine(canonical, factionId, false);
00706:             return HoldfastTradeResult.Ok(canonical, quantity, factionId, gain, whyLine);
00707:         }
00708:
00709:         /// <summary>
00710:         /// UNBLOCK-02 F13-D: Conversion from settlement units to integer chits (floor rounding, no hidden gains).
00711:         /// </summary>
00712:         public static int ChitsFromSettlementUnits(float units)
00713:         {
00714:             if (float.IsNaN(units) || float.IsInfinity(units) || (double)units > int.MaxValue)
00715:                 throw new ArgumentOutOfRangeException(nameof(units), "Settlement units must be finite and fit in integer chits.");
00716:             if (units <= 0f) return 0;
00720:         // Trade results and funds deltas are signed ints. Reject larger quotes
00721:         // before mutation rather than silently discounting or rounding them.
00722:         private static bool TryGetTradeTotal(long quote, out int total)
00723:         {
00724:             total = 0;
00725:             if (quote <= 0 || quote > int.MaxValue) return false;
00726:             total = (int)quote;
00729:
00730:         /// <summary>
00731:         /// UNBLOCK-02 F13-D: Wave-1 funds-denominated buy leg using canonical FundsLedger.
00732:         /// </summary>
00733:         public HoldfastTradeResult BuyWithFunds(string itemId, int quantity, string factionId, Economy.FundsLedger ledger, int day, string counterpartyId = "holdfast")
00734:         {
00735:             if (ledger == null)
00736:                 return HoldfastTradeResult.Fail("No funds ledger provided.", HoldfastTradeFailure.InsufficientFunds, Economy.FundsFailure.UnknownReasonKey);
00737:
00738:             string canonical = ItemAliases.ToCanonical(itemId);
00739:             var def = _catalog?.GetItem(canonical) ?? _catalog?.GetItem(itemId);
00740:             if (string.IsNullOrEmpty(canonical) || def == null)
00741:                 return HoldfastTradeResult.Fail("Unknown item: " + itemId, HoldfastTradeFailure.UnknownItem);
00742:
00743:             if (!string.IsNullOrEmpty(factionId) && factionId != "none")
00744:             {
00745:                 if (factionId == "faction_nonexistent" || (_catalog?.GetFaction(factionId) == null && factionId != "faction_the_office" && factionId != "faction_the_tempest"))
00746:                     return HoldfastTradeResult.Fail("Unknown faction: " + factionId, HoldfastTradeFailure.UnknownFaction);
00747:
00748:                 if (factionId == "faction_the_fleet")
00749:                     return HoldfastTradeResult.Fail("Unavailable or restricted counterparty: " + factionId, HoldfastTradeFailure.UnavailableOrRestricted);
00750:             }
00751:
00752:             if (!string.IsNullOrEmpty(factionId) && factionId != "none" && EmbargoQuery != null && EmbargoQuery(factionId))
00753:                 return HoldfastTradeResult.Fail("Trade with this faction is suspended (embargo).", HoldfastTradeFailure.Embargoed);
00754:
00755:             if (quantity <= 0)
00756:                 return HoldfastTradeResult.Fail("Quantity must be at least 1.", HoldfastTradeFailure.InvalidQuantity);
00757:
00758:             int currentStock = GetStock(canonical);
00759:             if (currentStock < quantity)
00760:                 return HoldfastTradeResult.Fail("Insufficient merchant stock.", HoldfastTradeFailure.InsufficientStock);
00761:
00762:             long costLong = GetBuyPrice(canonical, factionId, quantity);
00763:             if (!TryGetTradeTotal(costLong, out int chitCost))
00764:                 return HoldfastTradeResult.Fail("Trade price is outside the supported range.", HoldfastTradeFailure.InvalidPrice, Economy.FundsFailure.InvalidAmount);
00765:
00766:             if (ledger.Balance < chitCost)
00767:                 return HoldfastTradeResult.Fail("Insufficient funds.", HoldfastTradeFailure.InsufficientFunds, Economy.FundsFailure.InsufficientFunds);
00768:
00769:             if (!Inventory.CanAdd(canonical, quantity))
00770:             {
00771:                 float unitWeight = def.Weight;
00772:                 if (Inventory.MaxWeight > 0f && Inventory.GetCurrentWeight() + unitWeight * quantity > Inventory.MaxWeight)
00773:                     return HoldfastTradeResult.Fail("Inventory weight limit exceeded.", HoldfastTradeFailure.InventoryCapacity);
00774:                 return HoldfastTradeResult.Fail("Inventory capacity reached.", HoldfastTradeFailure.InventoryCapacity);
00775:             }
00776:
00778:             if (!debitResult.Success)
00779:             {
00780:                 return HoldfastTradeResult.Fail("Failed to debit funds: " + debitResult.FailureReason, HoldfastTradeFailure.InsufficientFunds, Economy.FundsFailure.InsufficientFunds);
00781:             }
00782:
00783:             bool added = Inventory.AddItem(canonical, quantity);
00784:             if (!added)
00785:             {
00786:                 ledger.TryCredit(chitCost, "holdfast_buy", counterpartyId, day);
00787:                 return HoldfastTradeResult.Fail("Inventory capacity reached.", HoldfastTradeFailure.InventoryCapacity);
00788:             }
00789:
00790:             _stock[canonical] = currentStock - quantity;
00791:             if (_playerInventory == null)
00792:             {
00793:                 _held[canonical] = GetHeld(canonical) + quantity;
00794:             }
00795:             StateChanged?.Invoke();
00796:             string whyLine = GetWhyLine(canonical, factionId, true);
00797:             return HoldfastTradeResult.Ok(canonical, quantity, factionId, chitCost, whyLine, -chitCost);
00798:         }
00799:
00800:         /// <summary>
00801:         /// UNBLOCK-02 F13-D: Wave-1 funds-denominated sell leg using canonical FundsLedger.
00802:         /// </summary>
00803:         public HoldfastTradeResult SellWithFunds(string itemId, int quantity, string factionId, Economy.FundsLedger ledger, int day, string counterpartyId = "holdfast")
00804:         {
00805:             if (ledger == null)
00806:                 return HoldfastTradeResult.Fail("No funds ledger provided.", HoldfastTradeFailure.InsufficientFunds, Economy.FundsFailure.UnknownReasonKey);
00807:
00808:             string canonical = ItemAliases.ToCanonical(itemId);
00809:             var def = _catalog?.GetItem(canonical) ?? _catalog?.GetItem(itemId);
00810:             if (string.IsNullOrEmpty(canonical) || def == null)
00811:                 return HoldfastTradeResult.Fail("Unknown item: " + itemId, HoldfastTradeFailure.UnknownItem);
00812:
00813:             if (!string.IsNullOrEmpty(factionId) && factionId != "none")
00814:             {
00815:                 if (factionId == "faction_nonexistent" || (_catalog?.GetFaction(factionId) == null && factionId != "faction_the_office" && factionId != "faction_the_tempest"))
00816:                     return HoldfastTradeResult.Fail("Unknown faction: " + factionId, HoldfastTradeFailure.UnknownFaction);
00817:
00818:                 if (factionId == "faction_the_fleet")
00819:                     return HoldfastTradeResult.Fail("Unavailable or restricted counterparty: " + factionId, HoldfastTradeFailure.UnavailableOrRestricted);
00820:             }
00821:
00822:             if (!string.IsNullOrEmpty(factionId) && factionId != "none" && EmbargoQuery != null && EmbargoQuery(factionId))
00823:                 return HoldfastTradeResult.Fail("Trade with this faction is suspended (embargo).", HoldfastTradeFailure.Embargoed);
00824:
00825:             if (quantity <= 0)
00826:                 return HoldfastTradeResult.Fail("Quantity must be at least 1.", HoldfastTradeFailure.InvalidQuantity);
00827:
00828:             int currentlyHeld = GetHeld(canonical);
00829:             if (currentlyHeld < quantity)
00830:                 return HoldfastTradeResult.Fail("Insufficient player inventory.", HoldfastTradeFailure.InsufficientInventory);
00831:
00832:             long gainLong = GetSellPrice(canonical, factionId, quantity);
00833:             if (!TryGetTradeTotal(gainLong, out int chitGain))
00834:                 return HoldfastTradeResult.Fail("Trade price is outside the supported range.", HoldfastTradeFailure.InvalidPrice, Economy.FundsFailure.InvalidAmount);
00835:             if ((long)GetStock(canonical) + quantity > int.MaxValue)
00836:                 return HoldfastTradeResult.Fail("Merchant stock capacity reached.", HoldfastTradeFailure.InventoryCapacity);
00837:
00838:             var creditResult = ledger.TryCredit(chitGain, "holdfast_sell", counterpartyId, day);
00839:             if (!creditResult.Success)
00840:             {
00841:                 return HoldfastTradeResult.Fail("Failed to credit funds: " + creditResult.FailureReason, HoldfastTradeFailure.InvalidPrice, Economy.FundsFailure.InvalidAmount);
00842:             }
00843:
00844:             _held[canonical] = currentlyHeld - quantity;
00845:             _stock[canonical] = GetStock(canonical) + quantity;
00846:             Inventory.RemoveItem(canonical, quantity);
00847:             StateChanged?.Invoke();
00848:             string whyLine = GetWhyLine(canonical, factionId, false);
00849:             return HoldfastTradeResult.Ok(canonical, quantity, factionId, chitGain, whyLine, chitGain);
00850:         }
00851:
00852:         public bool TryRestoreState(HoldfastTradeSaveState state, out string error)
00853:         {
00854:             error = string.Empty;
00855:             if (state == null) { error = "null state"; return false; }
00856:             _held.Clear();
00859:             // When a backing inventory is wired, Inventory IS the shared
00860:             // authoritative player inventory (Inventory = new
00861:             // HoldfastTradeInventory(_catalog, _playerInventory) above) — the
00862:             // same object InventoryHostSession, expeditions, crafting, and
00863:             // every other system read/write. Clearing it here would silently
00864:             // discard everything the player holds that isn't Holdfast trade
00865:             // stock (food, water, medicine, gear) and is not this session's
00866:             // to own. MigrateHoldfastHeld already performs the correct,
00867:             // non-destructive merge into that authoritative inventory below;
00868:             // only the standalone/no-backing-inventory path (tests, or a
00869:             // trade session with its own private ledger) still owns its
00870:             // items outright and may safely clear+rebuild them here.
00871:             if (_playerInventory == null)
00872:                 Inventory.Clear();
00873:
00878:                     foreach (var kv in state.held)
00879:                     {
00880:                         string canonical = ItemAliases.ToCanonical(kv.Key);
00881:                         _held[canonical] = kv.Value;
00882:                         Inventory.AddItem(canonical, kv.Value);
00883:                     }
00884:                 }
00885:                 else
00886:                 {
00889:                         InventoryMigrator.MigrateHoldfastHeld(state, _playerInventory, id =>
00890:                         {
00891:                             var def = _catalog?.GetItem(id);
00892:                             return def != null ? new ItemDefinition { id = id, displayName = def.DisplayName, stackMax = 99, weight = def.Weight } : null;
00893:                         }, allowResurrectLowerPhysicalCount: false);
00894:                     }
00895:                     _held.Clear();
00899:                         if (s?.Item != null && s.Amount > 0)
00900:                         {
00901:                             string canonical = ItemAliases.ToCanonical(s.Item.id);
00902:                             _held.TryGetValue(canonical, out int cur);
00903:                             _held[canonical] = cur + s.Amount;
00904:                         }
00905:                     }
00906:                 }
00907:             }
00908:             if (state.stock != null)
00909:                 foreach (var kv in state.stock) _stock[ItemAliases.ToCanonical(kv.Key)] = kv.Value;
00910:             _value = state.value >= 0 ? state.value : _value;
00911:             StateChanged?.Invoke();
00912:             return true;
00913:         }
00917:             failureCode = "unknown_item";
00918:             messageKey = "trade.unknown_item";
00919:             string canonical = ItemAliases.ToCanonical(itemId);
00920:             def = _catalog?.GetItem(canonical) ?? _catalog?.GetItem(itemId);
00921:             if (string.IsNullOrEmpty(canonical) || def == null)
00922:                 return false;
00923:             return true;
00924:         }
00925:
00926:         private bool ValidateFaction(string factionId, out string failureCode, out string messageKey)
00927:         {
00928:             failureCode = "unknown_faction";
00929:             messageKey = "trade.unknown_faction";
00930:             if (!string.IsNullOrEmpty(factionId) && factionId != "none")
00931:             {
00932:                 if (factionId == "faction_nonexistent" || (_catalog?.GetFaction(factionId) == null && factionId != "faction_the_office" && factionId != "faction_the_tempest"))
00933:                 {
00934:                     failureCode = "unknown_faction";
00935:                     messageKey = "trade.unknown_faction";
00936:                     return false;
00937:                 }
00938:                 if (factionId == "faction_the_fleet")
00939:                 {
00940:                     failureCode = "unavailable_or_restricted";
00941:                     messageKey = "trade.unavailable_or_restricted";
00942:                     return false;
00950:         /// Shares the same validation path as <see cref="Buy"/>.
00951:         /// </summary>
00952:         public CommandPreview PreviewBuy(string itemId, int quantity, string factionId, long stateVersion = 0)
00953:         {
00954:             if (quantity <= 0)
00955:                 return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "invalid_quantity", "trade.invalid_quantity", stateVersion);
00956:
00957:             if (!ValidateTradeItem(itemId, out var def, out var itemFailure, out var itemMessage))
00958:                 return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, itemFailure, itemMessage, stateVersion);
00959:
00960:             if (!ValidateFaction(factionId, out var factionFailure, out var factionMessage))
00961:                 return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, factionFailure, factionMessage, stateVersion);
00962:
00963:             if (!string.IsNullOrEmpty(factionId) && factionId != "none" && EmbargoQuery != null && EmbargoQuery(factionId))
00964:                 return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "embargoed", "trade.embargoed", stateVersion);
00965:
00966:             string canonical = ItemAliases.ToCanonical(itemId);
00967:             int currentStock = GetStock(canonical);
00968:             if (currentStock < quantity)
00969:                 return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "insufficient_stock", "trade.insufficient_stock", stateVersion);
00970:
00971:             long costLong = GetBuyPrice(canonical, factionId, quantity);
00972:             if (!TryGetTradeTotal(costLong, out int cost))
00973:                 return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "invalid_price", "trade.invalid_price", stateVersion);
00974:             if (cost > _value)
00975:                 return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "insufficient_funds", "trade.insufficient_funds", stateVersion);
00976:
00977:             if (!Inventory.CanAdd(canonical, quantity))
00978:                 return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "inventory_capacity", "trade.inventory_capacity", stateVersion);
00979:
00980:             var deltas = new Dictionary<string, double>
00981:             {
00982:                 { "value", -cost },
00983:                 { canonical, quantity },
00984:                 { "stock", -quantity }
00985:             };
00986:
00987:             return CommandPreview.Available(
00988:                 PlayerCommandCode.TradeConfirm,
00989:                 stateVersion,
00990:                 deltas,
00991:                 isIrreversible: false,
00992:                 messageKey: "trade.preview_buy");
00997:         /// Stale previews (state version mismatch) are rejected without mutation.
00998:         /// </summary>
00999:         public CommandResult ExecuteBuy(string itemId, int quantity, string factionId, long expectedStateVersion = 0, long currentStateVersion = 0)
01000:         {
01001:             var preview = PreviewBuy(itemId, quantity, factionId, expectedStateVersion);
01002:             if (!preview.IsAvailable)
01003:                 return CommandResult.FromPreview(preview);
01004:
01005:             if (preview.StateVersion != currentStateVersion)
01006:                 return CommandResult.StalePreview(PlayerCommandCode.TradeConfirm, preview.StateVersion, currentStateVersion);
01007:
01008:             var result = Buy(itemId, quantity, factionId);
01009:             if (!result.Success)
01010:                 return new CommandResult(
01011:                     PlayerCommandCode.TradeConfirm,
01012:                     ActionResult.Failed(result.Failure.ToString(), "trade.buy_failed"),
01013:                     expectedStateVersion,
01014:                     currentStateVersion);
01015:
01016:             var deltas = new Dictionary<string, double>
01017:             {
01018:                 { "value", -result.TotalValue },
01019:                 { result.ItemId, result.Quantity },
01020:                 { "stock", -result.Quantity }
01022:
01023:             return CommandResult.FromSuccess(
01024:                 PlayerCommandCode.TradeConfirm,
01025:                 ActionResult.Success("trade.bought", deltas),
01026:                 expectedStateVersion,
01027:                 currentStateVersion + 1);
01028:         }
01029:
01032:         /// Shares the same validation path as <see cref="Sell"/>.
01033:         /// </summary>
01034:         public CommandPreview PreviewSell(string itemId, int quantity, string factionId, long stateVersion = 0)
01035:         {
01036:             if (quantity <= 0)
01037:                 return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "invalid_quantity", "trade.invalid_quantity", stateVersion);
01038:
01039:             if (!ValidateTradeItem(itemId, out var def, out var itemFailure, out var itemMessage))
01040:                 return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, itemFailure, itemMessage, stateVersion);
01041:
01042:             if (!ValidateFaction(factionId, out var factionFailure, out var factionMessage))
01043:                 return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, factionFailure, factionMessage, stateVersion);
01044:
01045:             if (!string.IsNullOrEmpty(factionId) && factionId != "none" && EmbargoQuery != null && EmbargoQuery(factionId))
01046:                 return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "embargoed", "trade.embargoed", stateVersion);
01047:
01048:             string canonical = ItemAliases.ToCanonical(itemId);
01049:             int currentlyHeld = GetHeld(canonical);
01050:             if (currentlyHeld < quantity)
01051:                 return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "insufficient_inventory", "trade.insufficient_inventory", stateVersion);
01052:
01053:             long gainLong = GetSellPrice(canonical, factionId, quantity);
01054:             if (!TryGetTradeTotal(gainLong, out int gain) || !CanCreditValue(gain))
01055:                 return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "invalid_price", "trade.invalid_price", stateVersion);
01056:             if ((long)GetStock(canonical) + quantity > int.MaxValue)
01057:                 return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "inventory_capacity", "trade.inventory_capacity", stateVersion);
01058:             var deltas = new Dictionary<string, double>
01059:             {
01060:                 { "value", gain },
01061:                 { canonical, -quantity },
01062:                 { "stock", quantity }
01063:             };
01064:
01065:             return CommandPreview.Available(
01066:                 PlayerCommandCode.TradeConfirm,
01067:                 stateVersion,
01068:                 deltas,
01069:                 isIrreversible: false,
01070:                 messageKey: "trade.preview_sell");
01071:         }
01072:
01073:         /// <summary>
01074:         /// Execute a sell command using the same validation path as <see cref="PreviewSell"/>.
01075:         /// Stale previews (state version mismatch) are rejected without mutation.
01076:         /// </summary>
01077:         public CommandResult ExecuteSell(string itemId, int quantity, string factionId, long expectedStateVersion = 0, long currentStateVersion = 0)
01078:         {
01079:             var preview = PreviewSell(itemId, quantity, factionId, expectedStateVersion);
01080:             if (!preview.IsAvailable)
01081:                 return CommandResult.FromPreview(preview);
01082:
01083:             if (preview.StateVersion != currentStateVersion)
01084:                 return CommandResult.StalePreview(PlayerCommandCode.TradeConfirm, preview.StateVersion, currentStateVersion);
01085:
01086:             var result = Sell(itemId, quantity, factionId);
01087:             if (!result.Success)
01088:                 return new CommandResult(
01089:                     PlayerCommandCode.TradeConfirm,
01090:                     ActionResult.Failed(result.Failure.ToString(), "trade.sell_failed"),
01091:                     expectedStateVersion,
01092:                     currentStateVersion);
01093:
01094:             var deltas = new Dictionary<string, double>
01095:             {
01096:                 { "value", result.TotalValue },
01097:                 { result.ItemId, -result.Quantity },
01098:                 { "stock", result.Quantity }
01099:             };
01100:
01101:             return CommandResult.FromSuccess(
01102:                 PlayerCommandCode.TradeConfirm,
01103:                 ActionResult.Success("trade.sold", deltas),
01104:                 expectedStateVersion,
01105:                 currentStateVersion + 1);
01106:         }
01107:
01108:         public HoldfastTradeSaveState CaptureState()
01109:         {
01110:             var heldMap = new Dictionary<string, int>(Held, StringComparer.Ordinal);
01111:             return new HoldfastTradeSaveState
01112:             {
01113:                 schemaVersion = 2,
01114:                 value = _value,
01115:                 held = heldMap,
01116:                 stock = new Dictionary<string, int>(_stock, StringComparer.Ordinal)
01117:             };
01118:         }
01119:     }
01120:
01121:     /// <summary>Serializable trade save envelope (value + held + stock).</summary>
01122:     [Serializable]
01123:     public class HoldfastTradeSaveState
01124:     {
01125:         public int schemaVersion = 0;
01126:         public long value;
01127:         public Dictionary<string, int> held = new Dictionary<string, int>();
01128:         public Dictionary<string, int> stock = new Dictionary<string, int>();
01129:     }
01130: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/HoldfastFactionsCatalog.cs`

### `Assets/Ashfall.Core/HoldfastFactionsCatalog.cs` — complete current file

- Size: 92 lines / 4796 bytes.
- SHA-256: `0ab477ba16de881b921757e1a4b5eda4814b14f8c7a40e572df2ff4a96b384a6`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections;
00004: using System.Collections.Generic;
00005:
00006: namespace Ashfall.Core
00007: {
00008:     /// <summary>Holdfast faction (trade surface). Matches the terminal/catalog contract.</summary>
00009:     public sealed class HoldfastFactionEntry
00010:     {
00011:         public string id { get; set; } = string.Empty;
00012:         public string display_name { get; set; } = string.Empty;
00013:         public string alignment { get; set; } = string.Empty;
00014:         public string home_region { get; set; } = string.Empty;
00015:         public bool is_active { get; set; } = true;
00016:         public float trust { get; set; } = 0f;
00017:         public string[] wants { get; set; } = Array.Empty<string>();
00018:         public string[] offers { get; set; } = Array.Empty<string>();
00019:         public string signature_quote { get; set; } = string.Empty;
00020:         public string access_rule { get; set; } = string.Empty;
00021:         public string badge_asset_id { get; set; } = string.Empty;
00022:
00023:         public string Id => id;
00024:         public string DisplayName => display_name;
00025:         public string Alignment => alignment;
00026:         public string HomeRegion => home_region;
00027:         public bool IsActive => is_active;
00028:         public float Trust => trust;
00029:         public string[] Wants => wants;
00030:         public string[] Offers => offers;
00031:         public string SignatureQuote => signature_quote;
00032:         public string AccessRule => access_rule;
00033:         public string BadgeAssetId => badge_asset_id;
00034:
00035:         public HoldfastFactionEntry() { }
00036:
00037:         public HoldfastFactionEntry(string id, string displayName, string alignment, string homeRegion = "", bool isActive = true, float trust = 0f, string[]? wants = null, string[]? offers = null, string signatureQuote = "", string accessRule = "", string badgeAssetId = "")
00038:         {
00039:             this.id = id ?? string.Empty;
00040:             this.display_name = displayName ?? string.Empty;
00041:             this.alignment = alignment ?? string.Empty;
00042:             this.home_region = homeRegion ?? string.Empty;
00043:             this.is_active = isActive;
00044:             this.trust = trust;
00045:             this.wants = wants ?? Array.Empty<string>();
00046:             this.offers = offers ?? Array.Empty<string>();
00047:             this.signature_quote = signatureQuote ?? string.Empty;
00048:             this.access_rule = accessRule ?? string.Empty;
00049:             this.badge_asset_id = badgeAssetId ?? string.Empty;
00050:         }
00051:
00052:         public string FactionDescription()
00053:         {
00054:             return alignment switch
00055:             {
00056:                 "order" => "A disciplined collective that values structure above all else. Their trade is conducted with military precision. They see the unlisted as either assets to be scheduled or threats to be contained. Their ledgers are immaculate. Their patience is not.",
00057:                 "chaos" => "A loose network of scavengers and opportunists who deal in secrets as much as supplies. They have no patience for bureaucracy, only for leverage. The unlisted are either fresh meat or fresh recruits—either way, they'll be put to work before the ink dries on the ledger.",
00058:                 "neutral" => "Pragmatic survivors who trade with anyone willing to meet their price. Ideology takes a backseat to survival. They'll deal with the unlisted, but they won't vouch for them. Trust is currency, and they're running low.",
00059:                 _ => "A mysterious faction whose true nature remains obscured by the wastes. They may be allies, they may be enemies—either way, they're watching."
00060:             };
00061:         }
00062:
00063:     }
00064:
00065:     /// <summary>Immutable-after-load Holdfast faction catalog.</summary>
00066:     public sealed class HoldfastFactionsCatalog : IEnumerable<HoldfastFactionEntry>
00067:     {
00068:         private readonly Dictionary<string, HoldfastFactionEntry> _byId =
00069:             new Dictionary<string, HoldfastFactionEntry>(StringComparer.Ordinal);
00070:         private readonly List<HoldfastFactionEntry> _order = new List<HoldfastFactionEntry>();
00071:
00072:         public int Count => _order.Count;
00073:         public HoldfastFactionEntry this[int index] => _order[index];
00074:
00075:         public static HoldfastFactionsCatalog Empty() => new HoldfastFactionsCatalog();
00076:
00077:         public void Register(HoldfastFactionEntry entry)
00078:         {
00079:             if (entry == null || string.IsNullOrEmpty(entry.Id) || _byId.ContainsKey(entry.Id)) return;
00080:             _byId[entry.Id] = entry;
00081:             _order.Add(entry);
00082:         }
00083:
00084:         public HoldfastFactionEntry? GetById(string id)
00085:             => string.IsNullOrEmpty(id) ? null : (_byId.TryGetValue(id, out var e) ? e : null);
00086:
00087:         public bool Contains(string id) => GetById(id) != null;
00088:
00089:         public IEnumerator<HoldfastFactionEntry> GetEnumerator() => _order.GetEnumerator();
00090:         IEnumerator IEnumerable.GetEnumerator() => _order.GetEnumerator();
00091:     }
00092: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/HoldfastFlavorExpansionTests.cs`

### `Ashfall.Core.Tests/HoldfastFlavorExpansionTests.cs` — complete current file

- Size: 219 lines / 10537 bytes.
- SHA-256: `57676604e6f71701d5aa27c225ed37c9f4f40cff05b4d38e43280f0965e49648`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Text.Json;
00006: using Xunit;
00007: using Ashfall.Core;
00008:
00009: namespace Ashfall.Core.Tests
00010: {
00011:     public class HoldfastFlavorExpansionTests : CatalogTestBase
00012:     {
00013:         private static string DataDir()
00014:         {
00015:             string start = Directory.GetCurrentDirectory();
00016:             if (CatalogLocator.TryFindDataDirectory(start, out string found))
00017:                 return found;
00018:             if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
00019:                 return found;
00020:             throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
00021:         }
00022:
00023:         private sealed class FlavorDataDto
00024:         {
00025:             public int schema_version { get; set; }
00026:             public Dictionary<string, FactionEntryDto> factions { get; set; } = new Dictionary<string, FactionEntryDto>();
00027:             public Dictionary<string, string> items { get; set; } = new Dictionary<string, string>();
00028:         }
00029:
00030:         private sealed class FactionEntryDto
00031:         {
00032:             public string register { get; set; } = string.Empty;
00033:             public string voice { get; set; } = string.Empty;
00034:             public string rejected { get; set; } = string.Empty;
00035:             public string sold { get; set; } = string.Empty;
00036:         }
00037:
00038:         private static FlavorDataDto LoadFlavorData()
00039:         {
00040:             string path = Path.Combine(DataDir(), "holdfast_flavor.json");
00041:             Assert.True(File.Exists(path), "holdfast_flavor.json must exist at " + path);
00042:             string json = File.ReadAllText(path);
00043:             var data = JsonSerializer.Deserialize<FlavorDataDto>(json);
00044:             Assert.NotNull(data);
00045:             return data!;
00046:         }
00047:
00048:         [Fact]
00049:         public void HoldfastFlavor_SchemaVersionAndStructure()
00050:         {
00051:             var data = LoadFlavorData();
00052:             Assert.Equal(1, data.schema_version);
00053:             Assert.NotNull(data.factions);
00054:             Assert.Equal(8, data.factions.Count);
00055:             Assert.NotNull(data.items);
00056:             Assert.Equal(40, data.items.Count);
00057:         }
00058:
00059:         [Fact]
00060:         public void HoldfastFlavor_BaselineThreeFactions_ParityPreserved()
00061:         {
00062:             var data = LoadFlavorData();
00063:
00064:             // 1. faction_the_office
00065:             Assert.True(data.factions.ContainsKey("faction_the_office"));
00066:             var office = data.factions["faction_the_office"];
00067:             Assert.Equal("bureaucratic", office.register);
00068:             Assert.Equal("The Office speaks in stamps, countersignatures, and triplicate. A rejection is a missing seal, not an insult. Every release is logged against your account.", office.voice);
00069:             Assert.Equal("Requisition denied — the authorising stamp is absent or the ledger balance does not cover the line item.", office.rejected);
00070:             Assert.Equal("Accepted for inventory. The Office records the transfer and adjusts the manifest accordingly.", office.sold);
00071:
00072:             // 2. faction_the_cutters
00073:             Assert.True(data.factions.ContainsKey("faction_the_cutters"));
00074:             var cutters = data.factions["faction_the_cutters"];
00075:             Assert.Equal("salvage", cutters.register);
00076:             Assert.Equal("The Cutters speak in tonnage and debts. Stock running low is weather coming in. Favours are tracked in ledgers, not sentiment.", cutters.voice);
00077:             Assert.Equal("No stock to release and no credit to draw against. The Cutters do not float empty requisitions.", cutters.rejected);
00078:             Assert.Equal("Taken to the pile. The cutter ledger shifts; your credit moves the other way.", cutters.sold);
00079:
00080:             // 3. faction_the_fleet
00081:             Assert.True(data.factions.ContainsKey("faction_the_fleet"));
00082:             var fleet = data.factions["faction_the_fleet"];
00083:             Assert.Equal("maritime", fleet.register);
00084:             Assert.Equal("The Fleet speaks in manifests, tides, and berths. A completed trade is logged like a vessel cleared to sail. Paper trails matter more than cargo.", fleet.voice);
00085:             Assert.Equal("The manifest does not clear. Either the berth is closed or the hold cannot accept the transfer.", fleet.rejected);
00086:             Assert.Equal("Logged and cleared. The Fleet manifest now shows the item transferred to your custody.", fleet.sold);
00087:         }
00088:
00089:         [Fact]
00090:         public void HoldfastFlavor_FiveNewFactions_ArePresentAndCanonical()
00091:         {
00092:             var data = LoadFlavorData();
00093:             string[] expectedNewFactions =
00094:             {
00095:                 "faction_black_flotilla",
00096:                 "faction_supply_corps",
00097:                 "faction_railway_guild",
00098:                 "faction_hydro_barons",
00099:                 "faction_ordnance_foundry"
00100:             };
00101:
00102:             foreach (string factionId in expectedNewFactions)
00103:             {
00104:                 Assert.True(data.factions.ContainsKey(factionId), "Missing expanded faction: " + factionId);
00105:                 var entry = data.factions[factionId];
00106:                 Assert.False(string.IsNullOrWhiteSpace(entry.register), "Empty register for " + factionId);
00107:                 Assert.False(string.IsNullOrWhiteSpace(entry.voice), "Empty voice for " + factionId);
00108:                 Assert.False(string.IsNullOrWhiteSpace(entry.rejected), "Empty rejected for " + factionId);
00109:                 Assert.False(string.IsNullOrWhiteSpace(entry.sold), "Empty sold for " + factionId);
00110:             }
00111:         }
00112:
00113:         [Fact]
00114:         public void HoldfastFlavor_AllEightFactions_HaveUniqueRegistersAndVoices()
00115:         {
00116:             var data = LoadFlavorData();
00117:             var registers = new HashSet<string>(StringComparer.Ordinal);
00118:             var voices = new HashSet<string>(StringComparer.Ordinal);
00119:             var rejecteds = new HashSet<string>(StringComparer.Ordinal);
00120:             var solds = new HashSet<string>(StringComparer.Ordinal);
00121:
00122:             foreach (var kvp in data.factions)
00123:             {
00124:                 Assert.StartsWith("faction_", kvp.Key, StringComparison.Ordinal);
00125:                 Assert.Equal(kvp.Key, kvp.Key.ToLowerInvariant());
00126:
00127:                 Assert.True(registers.Add(kvp.Value.register), "Duplicate register: " + kvp.Value.register);
00128:                 Assert.True(voices.Add(kvp.Value.voice), "Duplicate voice for: " + kvp.Key);
00129:                 Assert.True(rejecteds.Add(kvp.Value.rejected), "Duplicate rejected line for: " + kvp.Key);
00130:                 Assert.True(solds.Add(kvp.Value.sold), "Duplicate sold line for: " + kvp.Key);
00131:
00132:                 // Check text budget constraints: concise transaction prose
00133:                 Assert.InRange(kvp.Value.voice.Length, 50, 250);
00134:                 Assert.InRange(kvp.Value.rejected.Length, 30, 200);
00135:                 Assert.InRange(kvp.Value.sold.Length, 30, 200);
00136:
00137:                 // Disallow template placeholders or code syntax
00138:                 Assert.DoesNotContain("{", kvp.Value.voice);
00139:                 Assert.DoesNotContain("}", kvp.Value.voice);
00140:                 Assert.DoesNotContain("%", kvp.Value.voice);
00141:                 Assert.DoesNotContain("$", kvp.Value.voice);
00142:
00143:                 Assert.DoesNotContain("{", kvp.Value.rejected);
00144:                 Assert.DoesNotContain("}", kvp.Value.rejected);
00145:                 Assert.DoesNotContain("%", kvp.Value.rejected);
00146:
00147:                 Assert.DoesNotContain("{", kvp.Value.sold);
00148:                 Assert.DoesNotContain("}", kvp.Value.sold);
00149:                 Assert.DoesNotContain("%", kvp.Value.sold);
00150:             }
00151:
00152:             Assert.Equal(8, registers.Count);
00153:             Assert.Equal(8, voices.Count);
00154:             Assert.Equal(8, rejecteds.Count);
00155:             Assert.Equal(8, solds.Count);
00156:         }
00157:
00158:         [Fact]
00159:         public void HoldfastFlavor_AllFactionsExistInHoldfastFactionsCatalog()
00160:         {
00161:             var data = LoadFlavorData();
00162:             var loader = new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
00163:             var holdfastCatalog = loader.Load(DataDir());
00164:
00165:             Assert.NotNull(holdfastCatalog.Factions);
00166:             foreach (string factionId in data.factions.Keys)
00167:             {
00168:                 var faction = holdfastCatalog.GetFaction(factionId);
00169:                 Assert.NotNull(faction);
00170:                 Assert.Equal(factionId, faction!.id);
00171:                 Assert.False(string.IsNullOrWhiteSpace(faction.display_name));
00172:             }
00173:         }
00174:
00175:         [Fact]
00176:         public void HoldfastFlavor_ItemsDictionary_PreservedExactly()
00177:         {
00178:             var data = LoadFlavorData();
00179:             Assert.Equal(40, data.items.Count);
00180:             Assert.True(data.items.ContainsKey("item_map_sheet_ice_road"));
00181:             Assert.True(data.items.ContainsKey("item_census_return_blank"));
00182:             Assert.True(data.items.ContainsKey("item_order_12c"));
00183:             Assert.True(data.items.ContainsKey("item_triplicate_carbon"));
00184:             Assert.True(data.items.ContainsKey("item_ice_spike_bar"));
00185:             Assert.True(data.items.ContainsKey("item_beacon_oil"));
00186:             Assert.True(data.items.ContainsKey("item_cutter_ledger_blank"));
00187:             Assert.True(data.items.ContainsKey("item_electrolyte_salts"));
00188:
00189:             foreach (var kvp in data.items)
00190:             {
00191:                 Assert.StartsWith("item_", kvp.Key, StringComparison.Ordinal);
00192:                 Assert.False(string.IsNullOrWhiteSpace(kvp.Value));
00193:             }
00194:         }
00195:
00196:         [Fact]
00197:         public void HoldfastFlavor_FallbackBehaviorSimulation()
00198:         {
00199:             var data = LoadFlavorData();
00200:
00201:             // Verify that unknown factions are not present and simulate default fallback
00202:             string unknownFactionId = "faction_unknown_entity";
00203:             Assert.False(data.factions.ContainsKey(unknownFactionId));
00204:
00205:             // Fallback contract matches HoldfastFlavorCatalog.NeutralFactionVoice
00206:             const string neutralVoice = "The counterparty has no recorded voice.";
00207:             const string neutralRejected = "Transaction declined.";
00208:             const string neutralSold = "Item accepted.";
00209:
00210:             string resolvedVoice = data.factions.TryGetValue(unknownFactionId, out var entry) ? entry.voice : neutralVoice;
00211:             string resolvedRejected = entry != null ? entry.rejected : neutralRejected;
00212:             string resolvedSold = entry != null ? entry.sold : neutralSold;
00213:
00214:             Assert.Equal(neutralVoice, resolvedVoice);
00215:             Assert.Equal(neutralRejected, resolvedRejected);
00216:             Assert.Equal(neutralSold, resolvedSold);
00217:         }
00218:     }
00219: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/HoldfastCatalog.cs`

### `Assets/Ashfall.Core/HoldfastCatalog.cs` — complete current file

- Size: 305 lines / 11871 bytes.
- SHA-256: `70d849c6b68037637e38b67197236cf26d96c187a1a8e38062765aa00fc08268`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using Ashfall.Core.Inventory;
00006:
00007: namespace Ashfall.Core
00008: {
00009:     public class HoldfastLocationEntry
00010:     {
00011:         public string id;
00012:         public string displayName;
00013:         public string inspect;
00014:         public string description;
00015:         public float dangerLevel;
00016:         public float travelHours;
00017:         public float baseRadsPerHour;
00018:         public string region;
00019:         public bool overlay_on_unlock;
00020:         public bool recast_always;
00021:     }
00022:
00023:     public class HoldfastQuestStageEntry
00024:     {
00025:         public string id;
00026:         public string text;
00027:     }
00028:
00029:     public class HoldfastQuestChoiceEntry
00030:     {
00031:         public string id;
00032:         public string text;
00033:         public string set_flag;
00034:     }
00035:
00036:     public class HoldfastQuestEntry
00037:     {
00038:         public string id;
00039:         public string display_name;
00040:         public string type;
00041:         public string briefing;
00042:         public string prereq_quest_id;
00043:         public int min_day;
00044:         public HoldfastQuestStageEntry[] stages;
00045:         public HoldfastQuestChoiceEntry[] choices;
00046:         public string knowledge_key;
00047:         public string target_location_id;
00048:
00049:         public int StageCount => stages != null ? stages.Length : 0;
00050:     }
00051:
00052:     public sealed class HoldfastCatalog
00053:     {
00054:         public List<HoldfastLocationEntry> Locations { get; } = new List<HoldfastLocationEntry>();
00055:         public List<HoldfastQuestEntry> Quests { get; } = new List<HoldfastQuestEntry>();
00056:         public HoldfastItemsCatalog Items { get; set; } = HoldfastItemsCatalog.Empty();
00057:         public HoldfastFactionsCatalog Factions { get; set; } = HoldfastFactionsCatalog.Empty();
00058:
00059:         public HoldfastItemDefinition? GetItem(string id) => Items != null ? Items.GetById(id) : null;
00060:         public HoldfastFactionEntry? GetFaction(string id) => Factions != null ? Factions.GetById(id) : null;
00061:
00062:         public HoldfastLocationEntry? GetLocation(string id)
00063:         {
00064:             if (string.IsNullOrEmpty(id)) return null;
00065:             for (int i = 0; i < Locations.Count; i++)
00066:                 if (Locations[i] != null && Locations[i].id == id)
00067:                     return Locations[i];
00068:             return null;
00069:         }
00070:
00071:         public HoldfastQuestEntry? GetQuest(string id)
00072:         {
00073:             if (string.IsNullOrEmpty(id)) return null;
00074:             for (int i = 0; i < Quests.Count; i++)
00075:                 if (Quests[i] != null && Quests[i].id == id)
00076:                     return Quests[i];
00077:             return null;
00078:         }
00079:     }
00080:
00081:     /// <summary>
00082:     /// JSON DTO for holdfast_items.json. HoldfastItemDefinition is immutable
00083:     /// (no setters), so deserialise into this DTO and convert in the loader.
00084:     /// </summary>
00085:     public sealed class HoldfastItemDto
00086:     {
00087:         public string id { get; set; } = string.Empty;
00088:         public string displayName { get; set; } = string.Empty;
00089:         public string description { get; set; } = string.Empty;
00090:         public double tradeValue { get; set; } = 0.0;
00091:         public double weight { get; set; } = 1.0;
00092:         public string type { get; set; } = "resource";
00093:         public int stackMax { get; set; } = 99;
00094:         public double thirstRestore { get; set; } = 0.0;
00095:         public double hungerRestore { get; set; } = 0.0;
00096:         public double moraleEffect { get; set; } = 0.0;
00097:     }
00098:
00099:     /// <summary>
00100:     /// JSON DTO for holdfast_factions.json. Avoids the alias collision in
00101:     /// HoldfastFactionEntry (it defines both `id` and `Id`, which fall over
00102:     /// when PropertyNameCaseInsensitive is on), so deserialise into this DTO
00103:     /// and convert in the loader.
00104:     /// </summary>
00105:     public sealed class HoldfastFactionDto
00106:     {
00107:         public string id { get; set; } = string.Empty;
00108:         public string display_name { get; set; } = string.Empty;
00109:         public string alignment { get; set; } = string.Empty;
00110:         public string home_region { get; set; } = string.Empty;
00111:         public bool is_active { get; set; } = true;
00112:         public float trust { get; set; } = 0f;
00113:         public string[] wants { get; set; } = Array.Empty<string>();
00114:         public string[] offers { get; set; } = Array.Empty<string>();
00115:         public string signature_quote { get; set; } = string.Empty;
00116:         public string access_rule { get; set; } = string.Empty;
00117:         public string badge_asset_id { get; set; } = string.Empty;
00118:     }
00119:
00120:     /// <summary>
00121:     /// Loads holdfast_locations.json / holdfast_quests.json / holdfast_items.json /
00122:     /// holdfast_factions.json from the shared StreamingAssets/Data directory.
00123:     /// No ScriptableObject materialisation.
00124:     /// </summary>
00125:     public sealed class HoldfastCatalogLoader
00126:     {
00127:         public const string LocationsFile = "holdfast_locations.json";
00128:         public const string QuestsFile = "holdfast_quests.json";
00129:         public const string ItemsFile = "holdfast_items.json";
00130:         public const string FactionsFile = "holdfast_factions.json";
00131:
00132:         private readonly IFileIO _files;
00133:         private readonly IJsonSerializer _json;
00134:         private readonly ILog _log;
00135:
00136:         public HoldfastCatalogLoader(IFileIO files, IJsonSerializer json, ILog? log = null)
00137:         {
00138:             _files = files ?? throw new ArgumentNullException(nameof(files));
00139:             _json = json ?? throw new ArgumentNullException(nameof(json));
00140:             _log = log ?? NullLog.Instance;
00141:         }
00142:
00143:         /// <param name="expansionUnlocked">
00144:         /// When false, the 26 District 8 cards stay dark. recast_always (3 Sector 4 ids)
00145:         /// still load so copy overlays can apply. overlay_on_unlock rows wait on unlock.
00146:         /// </param>
00147:         public HoldfastCatalog Load(string dataDirectory, bool expansionUnlocked = true)
00148:         {
00149:             var catalog = new HoldfastCatalog();
00150:             if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
00151:             {
00152:                 _log.Warn("Holdfast catalog directory missing: " + dataDirectory);
00153:                 return catalog;
00154:             }
00155:
00156:             LoadLocations(_files.Combine(dataDirectory, LocationsFile), catalog.Locations, expansionUnlocked);
00157:             LoadList(_files.Combine(dataDirectory, QuestsFile), catalog.Quests, "quests");
00158:             LoadItems(_files.Combine(dataDirectory, ItemsFile), catalog.Items, "items");
00159:             LoadFactions(_files.Combine(dataDirectory, FactionsFile), catalog.Factions, "factions");
00160:             return catalog;
00161:         }
00162:
00163:         public static bool IncludeLocation(HoldfastLocationEntry e, bool expansionUnlocked)
00164:         {
00165:             if (e == null || string.IsNullOrEmpty(e.id)) return false;
00166:             if (e.recast_always) return true;
00167:             return expansionUnlocked;
00168:         }
00169:
00170:         /// <summary>Strips authoring notes such as "(existing)" / "(recast; existing id)" from display names.</summary>
00171:         public static string StripAuthorNotes(string displayName)
00172:         {
00173:             if (string.IsNullOrEmpty(displayName)) return displayName;
00174:             int idx = displayName.IndexOf(" (existing", StringComparison.Ordinal);
00175:             if (idx > 0) return displayName.Substring(0, idx);
00176:             idx = displayName.IndexOf(" (recast", StringComparison.Ordinal);
00177:             if (idx > 0) return displayName.Substring(0, idx);
00178:             return displayName;
00179:         }
00180:
00181:         private void LoadLocations(string path, List<HoldfastLocationEntry> dest, bool expansionUnlocked)
00182:         {
00183:             if (!_files.FileExists(path))
00184:             {
00185:                 _log.Warn("Holdfast locations file missing: " + path);
00186:                 return;
00187:             }
00188:
00189:             try
00190:             {
00191:                 string json = _files.ReadAllText(path);
00192:                 var items = CatalogLocator.LoadWrappedList<HoldfastLocationEntry>(json, SystemTextJsonSerializer.Options);
00193:                 for (int i = 0; i < items.Count; i++)
00194:                 {
00195:                     var e = items[i];
00196:                     if (e == null) continue;
00197:                     e.displayName = StripAuthorNotes(e.displayName);
00198:                     if (!IncludeLocation(e, expansionUnlocked)) continue;
00199:                     dest.Add(e);
00200:                 }
00201:             }
00202:             catch (Exception e)
00203:             {
00204:                 _log.Error("Holdfast locations parse failed: " + e.Message);
00205:             }
00206:         }
00207:
00208:         private void LoadList<T>(string path, List<T> dest, string label) where T : class
00209:         {
00210:             if (!_files.FileExists(path))
00211:             {
00212:                 _log.Warn("Holdfast " + label + " file missing: " + path);
00213:                 return;
00214:             }
00215:
00216:             try
00217:             {
00218:                 string json = _files.ReadAllText(path);
00219:                 var items = CatalogLocator.LoadWrappedList<T>(json, SystemTextJsonSerializer.Options);
00220:                 for (int i = 0; i < items.Count; i++)
00221:                 {
00222:                     if (items[i] != null)
00223:                         dest.Add(items[i]);
00224:                 }
00225:             }
00226:             catch (Exception e)
00227:             {
00228:                 _log.Error("Holdfast " + label + " parse failed: " + e.Message);
00229:             }
00230:         }
00231:
00232:         /// <summary>
00233:         /// Loads items from holdfast_items.json into the item catalog.
00234:         /// JSON uses camelCase fields (displayName, tradeValue, stackMax, ...).
00235:         /// </summary>
00236:         private void LoadItems(string path, HoldfastItemsCatalog dest, string label)
00237:         {
00238:             if (!_files.FileExists(path))
00239:             {
00240:                 _log.Warn("Holdfast " + label + " file missing: " + path);
00241:                 return;
00242:             }
00243:
00244:             try
00245:             {
00246:                 string json = _files.ReadAllText(path);
00247:                 var dtos = CatalogLocator.LoadWrappedList<HoldfastItemDto>(json, SystemTextJsonSerializer.Options);
00248:                 for (int i = 0; i < dtos.Count; i++)
00249:                 {
00250:                     var dto = dtos[i];
00251:                     if (dto == null || string.IsNullOrEmpty(dto.id)) continue;
00252:                     dest.Register(new HoldfastItemDefinition(
00253:                         dto.id,
00254:                         dto.displayName,
00255:                         dto.description,
00256:                         (float)dto.tradeValue,
00257:                         (float)dto.weight,
00258:                         dto.type,
00259:                         dto.stackMax));
00260:                 }
00261:             }
00262:             catch (Exception e)
00263:             {
00264:                 _log.Error("Holdfast " + label + " parse failed: " + e.Message);
00265:             }
00266:         }
00267:
00268:         private void LoadFactions(string path, HoldfastFactionsCatalog dest, string label)
00269:         {
00270:             if (!_files.FileExists(path))
00271:             {
00272:                 _log.Warn("Holdfast " + label + " file missing: " + path);
00273:                 return;
00274:             }
00275:
00276:             try
00277:             {
00278:                 string json = _files.ReadAllText(path);
00279:                 var dtos = CatalogLocator.LoadWrappedList<HoldfastFactionDto>(json, SystemTextJsonSerializer.Options);
00280:                 for (int i = 0; i < dtos.Count; i++)
00281:                 {
00282:                     var dto = dtos[i];
00283:                     if (dto == null || string.IsNullOrEmpty(dto.id)) continue;
00284:                     dest.Register(new HoldfastFactionEntry(
00285:                         dto.id,
00286:                         dto.display_name,
00287:                         dto.alignment,
00288:                         dto.home_region,
00289:                         dto.is_active,
00290:                         dto.trust,
00291:                         dto.wants,
00292:                         dto.offers,
00293:                         dto.signature_quote,
00294:                         dto.access_rule,
00295:                         dto.badge_asset_id));
00296:                 }
00297:             }
00298:             catch (Exception e)
00299:             {
00300:                 _log.Error("Holdfast " + label + " parse failed: " + e.Message);
00301:             }
00302:         }
00303:     }
00304: }
00305:
```


# Appendix — Current Source Detail: `src/Host/HoldfastRuntimeSession.cs`

### `src/Host/HoldfastRuntimeSession.cs` — bounded current excerpt (695 of 763 lines)

- Size: 763 lines / 32845 bytes.
- SHA-256: `f9755a5dc96e2dd96c5968d5c6fe88c42582ef88dfba04531e406a15f6dde991`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: #pragma warning disable CS8618
00004: using System.Collections.Generic;
00005: using System.IO;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Inventory;
00008: using Ashfall.Core.Survivors;
00009:
00010: namespace AtomicWar.GodotApp
00011: {
00012:     /// <summary>
00013:     /// Godot's playable Holdfast boundary. The world session owns existing
00014:     /// Holdfast systems; the Core trade session owns mutable inventory/value/stock.
00015:     ///
00016:     /// Single-source-of-truth: Survival mechanics (Health/Hunger/Thirst/Radiation)
00017:     /// project directly from Ashfall.Core.Survivors.NeedsSystem and RadiationSystem
00018:     /// via the bound SurvivorsHostSession.
00019:     /// </summary>
00020:     public sealed class HoldfastRuntimeSession
00021:     {
00022:         public const long DefaultStartingValue = 100;
00023:         public const int MaxHealth = 100;
00024:         public const int MaxHunger = 100;
00025:         public const int MaxThirst = 100;
00026:         public const float RadDamageThreshold = 50f; // mSv/day causes HP loss
00027:         public const float StarvationThreshold = 90f; // hunger above this causes HP loss
00028:         public const float DehydrationThreshold = 90f; // thirst above this causes HP loss
00029:
00030:         public CoreDemoSession World { get; }
00031:         public HoldfastTradeSession Trade { get; }
00032:         public HoldfastCatalog Catalog => World.Catalog;
00033:         public string LastPersistenceMessage { get; private set; } = string.Empty;
00034:         public bool HasPurchasedThisSession { get; set; }
00035:
00036:         // ── Authoritative Cohort / Player Binding ────────────────────
00037:         private SurvivorsHostSession? _survivors;
00038:         public SurvivorsHostSession? Survivors
00039:         {
00040:             get => _survivors;
00041:             set
00042:             {
00043:                 _survivors = value;
00044:                 WireInventorySession();
00045:             }
00046:         }
00047:         public string PlayerSurvivorId { get; set; } = "survivor_dr_sarah_chen";
00048:
00049:         private InventoryHostSession? _inventorySession;
00050:         public InventoryHostSession? InventorySession
00051:         {
00052:             get => _inventorySession;
00053:             set
00054:             {
00055:                 _inventorySession = value;
00056:                 WireInventorySession();
00057:             }
00058:         }
00059:
00060:         public Ashfall.Core.Inventory.Inventory? Inventory { get; set; }
00061:
00062:         public Ashfall.Core.Inventory.Inventory? EffectiveInventory =>
00063:             _inventorySession?.Inventory ?? Inventory ?? Trade.PlayerInventory;
00064:
00065:         // ── Fallback survival state (for headless/standalone tests) ──
00066:         private int _fallbackHealth = MaxHealth;
00067:         private int _fallbackHunger = 0;
00068:         private int _fallbackThirst = 0;
00069:         private float _fallbackRadiation = 0f;
00070:
00071:         // ── Survival state projections ───────────────────────────────
00072:         public int Health => Survivors?.Find(PlayerSurvivorId) != null
00073:             ? (int)Math.Clamp(Survivors.Find(PlayerSurvivorId)!.Health, 0f, (float)MaxHealth)
00074:             : _fallbackHealth;
00075:
00076:         public float Radiation => Survivors != null
00077:             ? (Survivors.RadStateFor(PlayerSurvivorId)?.RadiationDose ?? 0f)
00078:             : _fallbackRadiation;
00079:
00080:         public int Hunger => Survivors?.Find(PlayerSurvivorId) != null
00081:             ? (int)Math.Clamp(Survivors.Find(PlayerSurvivorId)!.Hunger, 0f, (float)MaxHunger)
00082:             : _fallbackHunger;
00083:
00084:         public int Thirst => Survivors?.Find(PlayerSurvivorId) != null
00085:             ? (int)Math.Clamp(Survivors.Find(PlayerSurvivorId)!.Thirst, 0f, (float)MaxThirst)
00086:             : _fallbackThirst;
00087:
00088:         // Day is a live projection of the shared campaign clock (World.Clock),
00089:         // not an independent counter — HoldfastRuntimeSession itself has no
00090:         // persisted save state (only its Trade sub-session does), so an
00091:         // independently-incremented field silently reset to 1 on every
00092:         // Continue while World.Clock.Day (the real, persisted campaign day)
00093:         // kept its correct value. Projecting here means the HUD/dashboard/
00094:         // death-and-victory stats and MarkActiveSlotTerminal all agree with
00095:         // the same single day authority across a save/reload round-trip.
00096:         public int Day => World.Clock.Day;
00097:         public bool IsDead => Health <= 0;
00098:         public string DeathCause { get; private set; } = string.Empty;
00099:
00100:         // ── Quest / Win state ────────────────────────────────────────
00101:         public bool IsGameWon => World.Quests != null && World.Quests.IsCompleted(HoldfastQuestSystem.Hatch);
00102:         public string WinMessage { get; private set; } = string.Empty;
00103:
00104:         public event Action StateChanged;
00105:         public event Action<string> OnPlayerDied; // passes cause of death
00106:         public event Action<string> OnGameWon; // passes win message
00107:
00108:         public HoldfastRuntimeSession(CoreDemoSession world, long startingValue = DefaultStartingValue, Ashfall.Core.Inventory.Inventory? inventory = null)
00109:         {
00110:             World = world ?? throw new ArgumentNullException(nameof(world));
00111:             Inventory = inventory;
00112:             Trade = new HoldfastTradeSession(World.Catalog, startingValue, inventory);
00113:             Trade.StateChanged += () => StateChanged?.Invoke();
00114:         }
00115:
00116:         public static HoldfastRuntimeSession Create(
00117:             CoreDemoSession world,
00118:             bool seedDevelopmentState = false,
00119:             bool loadTradeSave = true,
00120:             Ashfall.Core.Inventory.Inventory? inventory = null)
00121:         {
00122:             var session = new HoldfastRuntimeSession(world, DefaultStartingValue, inventory);
00123:             if (loadTradeSave)
00124:             {
00125:                 var saved = HoldfastTradeSaveStore.TryLoad();
00126:                 if (saved != null)
00127:                 {
00128:                     if (!session.Trade.TryRestoreState(saved, out string error))
00129:                         session.LastPersistenceMessage = "Holdfast trade save rejected: " + error;
00130:                     else
00131:                         session.LastPersistenceMessage = "Holdfast player and store state restored.";
00132:                 }
00133:                 else if (seedDevelopmentState)
00134:                 {
00135:                     session.SeedDevelopmentState();
00136:                 }
00137:             }
00138:             else if (seedDevelopmentState)
00139:             {
00140:                 session.SeedDevelopmentState();
00141:             }
00142:             return session;
00143:         }
00144:
00145:         public bool TrySaveToLegacyFiles(string basePathOverride = null!, string tradePathOverride = null!)
00146:         {
00147:             bool baseSaved = HoldfastSaveStore.TrySave(World.CaptureSave(), basePathOverride);
00148:             bool tradeSaved = HoldfastTradeSaveStore.TrySave(Trade.CaptureState(), tradePathOverride);
00149:             bool saved = baseSaved && tradeSaved;
00150:             LastPersistenceMessage = saved
00151:                 ? "Holdfast player, store, and world state saved."
00152:                 : "Holdfast save failed; existing state remains in memory.";
00153:             return saved;
00154:         }
00155:
00156:         public bool TryReloadFromLegacyFiles(string basePathOverride = null!, string tradePathOverride = null!)
00157:         {
00158:             var worldSnapshot = World.CaptureSave();
00159:             var tradeSnapshot = Trade.CaptureState();
00160:
00161:             var baseSave = HoldfastSaveStore.TryLoad(basePathOverride);
00162:             var tradeSave = HoldfastTradeSaveStore.TryLoad(tradePathOverride);
00163:
00164:             if (baseSave == null && tradeSave == null)
00165:             {
00166:                 LastPersistenceMessage = "No Holdfast save was available to reload.";
00167:                 return false;
00168:             }
00169:
00170:             try
00171:             {
00172:                 if (baseSave != null)
00173:                 {
00174:                     World.RestoreSave(baseSave);
00175:                 }
00176:
00177:                 if (tradeSave != null)
00178:                 {
00179:                     if (!Trade.TryRestoreState(tradeSave, out string error))
00180:                     {
00181:                         // Rollback on partial or corrupted trade load
00182:                         World.RestoreSave(worldSnapshot);
00183:                         Trade.TryRestoreState(tradeSnapshot, out _);
00184:                         LastPersistenceMessage = "Holdfast trade reload rejected: " + error;
00185:                         return false;
00186:                     }
00187:                 }
00188:
00189:                 LastPersistenceMessage = "Holdfast state reloaded from disk.";
00190:                 return true;
00191:             }
00192:             catch (Exception ex)
00193:             {
00194:                 // Rollback on any failure
00195:                 World.RestoreSave(worldSnapshot);
00196:                 Trade.TryRestoreState(tradeSnapshot, out _);
00197:                 LastPersistenceMessage = "Holdfast reload failed: " + ex.Message;
00198:                 return false;
00199:             }
00200:         }
00201:
00202:         public void SeedDevelopmentState()
00203:         {
00204:             Trade.SeedInventory("item_triplicate_carbon", 1);
00205:         }
00206:
00208:
00209:         /// <summary>
00210:         /// Advance one day. Advances quest progress and checks game over / win conditions.
00211:         /// Simulation decay (hunger, thirst, radiation, health loss) is driven authoritatively
00212:         /// by Core NeedsSystem and RadiationSystem (ticked via SurvivorsHostSession.TickHour).
00213:         /// </summary>
00214:         public string TickDay()
00215:         {
00216:             if (IsDead) return "The ledger is closed. No more days to count.";
00217:
00218:             // Day is now a live projection of World.Clock.Day (see the Day
00219:             // property above). The campaign's HoldfastCoreDayOwner always
00220:             // calls World.TickDay() (which itself advances World.Clock)
00221:             // immediately before this method, so the clock has already
00222:             // moved — advancing it again here would double-increment.
00223:
00224:             // Fallback decay when running in isolated test harnesses without SurvivorsHostSession
00225:             if (Survivors == null)
00226:             {
00227:                 _fallbackHunger = Math.Min(MaxHunger, _fallbackHunger + 8);
00228:                 _fallbackThirst = Math.Min(MaxThirst, _fallbackThirst + 10);
00231:
00232:                 int hpLoss = 0;
00233:                 if (_fallbackHunger >= StarvationThreshold)
00234:                     hpLoss += (int)((_fallbackHunger - StarvationThreshold) * 0.5f);
00235:                 if (_fallbackThirst >= DehydrationThreshold)
00236:                     hpLoss += (int)((_fallbackThirst - DehydrationThreshold) * 0.6f);
00237:                 if (_fallbackRadiation >= RadDamageThreshold)
00238:                     hpLoss += (int)((_fallbackRadiation - RadDamageThreshold) * 0.1f);
00239:
00240:                 _fallbackHealth = Math.Max(0, _fallbackHealth - hpLoss);
00241:             }
00242:
00245:             {
00246:                 // Check if player has items that unlock quest story gates
00247:                 bool hasMapItem = Trade.GetHeld("item_map_sheet_ice_road") > 0;
00248:                 World.Quests.TickDaily(Day, hasMapItem, false, false);
00249:
00250:                 // Advance quest if player is at a location
00251:                 World.AdvanceQuest();
00252:             }
00256:             {
00257:                 DeathCause = DetermineDeathCause();
00258:                 OnPlayerDied?.Invoke(DeathCause);
00259:                 return $"Day {Day}. {DeathCause}";
00260:             }
00261:
00262:             // Check win condition
00263:             if (IsGameWon)
00264:             {
00265:                 WinMessage = $"The hatch is open. Day {Day}. The Holdfast endures.";
00266:                 OnGameWon?.Invoke(WinMessage);
00267:                 return WinMessage;
00268:             }
00269:
00270:             StateChanged?.Invoke();
00272:         }
00273:
00274:         private void WireInventorySession()
00275:         {
00276:             if (_inventorySession == null) return;
00277:             if (Survivors != null)
00278:             {
00279:                 _inventorySession.Survivors = Survivors;
00280:             }
00281:             else
00282:             {
00283:                 _inventorySession.ApplyNeedOverride = (survivorId, needType, delta) =>
00284:                 {
00285:                     switch (needType)
00286:                     {
00287:                         case ItemType.Food:
00297:                     return true;
00298:                 };
00299:                 _inventorySession.ApplyRadCleanseOverride = (survivorId, rads) =>
00300:                 {
00301:                     _fallbackRadiation = Math.Max(0f, _fallbackRadiation - rads);
00302:                 };
00303:                 _inventorySession.ApplyContaminationOverride = (survivorId, dose) =>
00304:                 {
00305:                     _fallbackRadiation += dose;
00306:                 };
00307:             }
00308:         }
00309:
00310:         private InventoryHostSession? GetOrCreateInventorySession()
00311:         {
00312:             if (_inventorySession != null)
00313:             {
00314:                 WireInventorySession();
00315:                 return _inventorySession;
00316:             }
00317:
00318:             var inv = EffectiveInventory;
00319:             if (inv != null)
00320:             {
00321:                 string dataDir = CatalogPath.ResolveDataDir();
00322:                 var fileIO = CatalogPath.CreateFileIOForDataDir(dataDir);
00323:                 var serializer = new SystemTextJsonSerializer();
00324:                 var catalog = Ashfall.Core.Inventory.ItemCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);
00325:                 var descriptions = Ashfall.Core.Inventory.ItemDescriptionCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);
00326:                 var enrichmentLoader = new ExpansionEnrichmentCatalogLoader(fileIO, serializer);
00327:                 _inventorySession = new InventoryHostSession(inv, catalog, descriptions, enrichmentLoader.Load(dataDir));
00328:                 WireInventorySession();
00329:                 return _inventorySession;
00330:             }
00331:
00332:             return null;
00333:         }
00334:
00335:         /// <summary>
00336:         /// Consume food items from inventory to reduce hunger.
00337:         /// Returns true if food was consumed.
00338:         /// </summary>
00339:         public bool ConsumeFood(string itemId, int amount = 1)
00340:         {
00341:             return ConsumeFoodResult(itemId, amount).IsSuccess;
00342:         }
00343:
00344:         public ActionResult ConsumeFoodResult(string itemId, int amount = 1, string? survivorId = null)
00345:         {
00346:             if (string.IsNullOrEmpty(itemId) || amount <= 0)
00347:                 return ActionResult.Blocked("invalid_args", "Invalid item or amount.");
00348:
00349:             string targetSurvivor = survivorId ?? PlayerSurvivorId;
00350:             var session = GetOrCreateInventorySession();
00351:             if (session != null)
00352:             {
00353:                 int held = session.Inventory.CountById(itemId);
00354:                 if (held < amount)
00355:                     return ActionResult.Blocked("insufficient_inventory", $"Insufficient {itemId} in inventory ({held}/{amount}).");
00356:
00357:                 var aggregatedDeltas = new Dictionary<string, double>(StringComparer.Ordinal);
00358:                 for (int i = 0; i < amount; i++)
00359:                 {
00360:                     var r = session.ConsumeResult(itemId, targetSurvivor);
00361:                     if (!r.IsSuccess) return r;
00362:                     foreach (var kv in r.Deltas)
00363:                     {
00364:                         aggregatedDeltas.TryGetValue(kv.Key, out double cur);
00365:                         aggregatedDeltas[kv.Key] = cur + kv.Value;
00366:                     }
00367:                 }
00368:                 StateChanged?.Invoke();
00369:                 // CORE-MECH W1: one-shot per eating event (not per unit) so a
00370:                 // foodborne exposure attempt can never stack within a single meal.
00371:                 FoodConsumed?.Invoke(itemId, amount, targetSurvivor);
00372:                 return ActionResult.Success($"Ate {amount} × {itemId}.", aggregatedDeltas);
00373:             }
00374:
00375:             var fallback = FallbackConsume(itemId, amount, targetSurvivor, ItemType.Food);
00376:             if (fallback.IsSuccess)
00377:                 FoodConsumed?.Invoke(itemId, amount, targetSurvivor);
00378:             return fallback;
00379:         }
00380:
00381:         /// <summary>
00382:         /// CORE-MECH W1 foodborne-disease bridge seam. Raised exactly once per
00383:         /// successful food consumption event with (itemId, amount, survivorId).
00384:         /// The subscriber (Main) owns the preservation → disease translation; this
00385:         /// session stays ignorant of both systems. Not raised for water, blocked
00386:         /// eats, or failed consumes.
00387:         /// </summary>
00388:         public Action<string, int, string>? FoodConsumed { get; set; }
00389:
00390:         /// <summary>
00391:         /// Consume water items from inventory to reduce thirst.
00392:         /// Returns true if water was consumed.
00393:         /// </summary>
00394:         public bool ConsumeWater(string itemId, int amount = 1)
00395:         {
00396:             return ConsumeWaterResult(itemId, amount).IsSuccess;
00397:         }
00398:
00399:         public ActionResult ConsumeWaterResult(string itemId, int amount = 1, string? survivorId = null)
00400:         {
00401:             if (string.IsNullOrEmpty(itemId) || amount <= 0)
00402:                 return ActionResult.Blocked("invalid_args", "Invalid item or amount.");
00403:
00404:             string targetSurvivor = survivorId ?? PlayerSurvivorId;
00405:             var session = GetOrCreateInventorySession();
00406:             if (session != null)
00407:             {
00408:                 int held = session.Inventory.CountById(itemId);
00409:                 if (held < amount)
00410:                     return ActionResult.Blocked("insufficient_inventory", $"Insufficient {itemId} in inventory ({held}/{amount}).");
00411:
00412:                 var aggregatedDeltas = new Dictionary<string, double>(StringComparer.Ordinal);
00413:                 for (int i = 0; i < amount; i++)
00414:                 {
00415:                     var r = session.ConsumeResult(itemId, targetSurvivor);
00416:                     if (!r.IsSuccess) return r;
00417:                     foreach (var kv in r.Deltas)
00418:                     {
00419:                         aggregatedDeltas.TryGetValue(kv.Key, out double cur);
00420:                         aggregatedDeltas[kv.Key] = cur + kv.Value;
00421:                     }
00422:                 }
00423:                 StateChanged?.Invoke();
00424:                 return ActionResult.Success($"Drank {amount} × {itemId}.", aggregatedDeltas);
00425:             }
00426:
00427:             return FallbackConsume(itemId, amount, targetSurvivor, ItemType.Water);
00428:         }
00429:
00430:         /// <summary>
00431:         /// Take radiation exposure (from events, locations, etc.).
00432:         /// </summary>
00433:         public void ExposeRadiation(float msv)
00434:         {
00435:             if (msv <= 0f) return;
00436:             if (Survivors != null)
00437:             {
00438:                 Survivors.ExposeToZone(PlayerSurvivorId, msv);
00439:             }
00440:             else
00441:             {
00442:                 _fallbackRadiation += msv;
00448:         /// Use anti-rad items to reduce radiation.
00449:         /// </summary>
00450:         public bool UseAntiRad(string itemId, float reduction = 0f)
00451:         {
00452:             return UseAntiRadResult(itemId, reduction: reduction).IsSuccess;
00453:         }
00454:
00455:         public ActionResult UseAntiRadResult(string itemId, string? survivorId = null, float reduction = 0f)
00456:         {
00457:             if (string.IsNullOrEmpty(itemId))
00458:                 return ActionResult.Blocked("invalid_args", "Invalid item.");
00459:
00460:             string targetSurvivor = survivorId ?? PlayerSurvivorId;
00461:             var session = GetOrCreateInventorySession();
00462:             if (session != null)
00463:             {
00464:                 int held = session.Inventory.CountById(itemId);
00465:                 if (held < 1)
00466:                     return ActionResult.Blocked("insufficient_inventory", $"Insufficient {itemId} in inventory ({held}/1).");
00467:
00468:                 var r = session.ConsumeResult(itemId, targetSurvivor);
00469:                 if (r.IsSuccess)
00470:                     StateChanged?.Invoke();
00471:                 return r;
00472:             }
00473:
00474:             return FallbackConsume(itemId, 1, targetSurvivor, ItemType.AntiRad, reduction);
00475:         }
00476:
00477:         private ActionResult FallbackConsume(string itemId, int amount, string targetSurvivor, ItemType expectedType, float customReduction = 0f)
00478:         {
00479:             var session = GetOrCreateInventorySession();
00480:             if (session != null)
00481:             {
00482:                 var aggregatedDeltas = new Dictionary<string, double>(StringComparer.Ordinal);
00483:                 for (int i = 0; i < amount; i++)
00484:                 {
00485:                     var r = session.ConsumeResult(itemId, targetSurvivor);
00486:                     if (!r.IsSuccess) return r;
00487:                     foreach (var kv in r.Deltas)
00488:                     {
00489:                         aggregatedDeltas.TryGetValue(kv.Key, out double cur);
00490:                         aggregatedDeltas[kv.Key] = cur + kv.Value;
00491:                     }
00492:                 }
00493:                 StateChanged?.Invoke();
00494:                 return ActionResult.Success($"Consumed {amount} × {itemId}.", aggregatedDeltas);
00495:             }
00496:
00497:             return ActionResult.Blocked("no_inventory", "Cannot consume: no shelter inventory bound.");
00498:         }
00499:
00500:         /// <summary>
00501:         /// Plan 22 Task 22A §22A.15: Feed all living survivors atomically.
00502:         /// Preflights survivor count and available food portions before mutation.
00503:         /// Shortage returns a detailed failure without partial consumption.
00504:         /// </summary>
00505:         public ActionResult FeedAllCrewResult(string? itemId = null)
00506:         {
00507:             var session = GetOrCreateInventorySession();
00508:             if (session == null)
00509:                 return ActionResult.Blocked("no_inventory", "Cannot feed crew: no shelter inventory bound.");
00510:
00511:             var livingSurvivors = new List<string>();
00512:             if (Survivors != null && Survivors.RosterState.Count > 0)
00513:             {
00525:
00526:             if (livingSurvivors.Count == 0)
00527:                 return ActionResult.Blocked("no_living_survivors", "No living survivors to feed.");
00528:
00529:             string? candidateItem = itemId ?? FindAvailableFoodItemId();
00530:             if (string.IsNullOrEmpty(candidateItem))
00531:                 return ActionResult.Blocked("no_food_available", "No food available in shelter inventory.");
00532:
00533:             var def = session.Catalog.Get(candidateItem);
00534:             int available = session.Inventory.CountById(candidateItem);
00535:             if (available < livingSurvivors.Count)
00536:             {
00537:                 return ActionResult.Blocked(
00538:                     "insufficient_food",
00539:                     $"Insufficient {(def?.displayName ?? candidateItem)}: need {livingSurvivors.Count} portions, but only {available} available.");
00540:             }
00541:
00542:             var aggregatedDeltas = new Dictionary<string, double>(StringComparer.Ordinal);
00543:             for (int i = 0; i < livingSurvivors.Count; i++)
00544:             {
00545:                 var r = session.ConsumeResult(candidateItem, livingSurvivors[i]);
00546:                 if (!r.IsSuccess) return r;
00547:                 foreach (var kv in r.Deltas)
00548:                 {
00549:                     aggregatedDeltas.TryGetValue(kv.Key, out double cur);
00550:                     aggregatedDeltas[kv.Key] = cur + kv.Value;
00551:                 }
00552:             }
00553:
00554:             StateChanged?.Invoke();
00555:             return ActionResult.Success(
00556:                 $"Fed {livingSurvivors.Count} crew members 1 × {(def?.displayName ?? candidateItem)}.",
00557:                 aggregatedDeltas);
00558:         }
00559:
00560:         public string? FindAvailableFoodItemId()
00561:         {
00562:             var inv = EffectiveInventory;
00563:             if (inv == null) return null;
00564:
00566:             {
00567:                 var slot = inv.Slots[i];
00568:                 if (slot?.Item != null && slot.Amount > 0 && slot.Item.IsConsumable())
00569:                 {
00570:                     if (slot.Item.type == ItemType.Food ||
00571:                         slot.Item.type == ItemType.ContaminatedFood ||
00572:                         slot.Item.hungerRestore > 0f)
00573:                         return slot.Item.id;
00574:                 }
00575:             }
00576:             return null;
00577:         }
00578:
00579:         public string? FindAvailableWaterItemId()
00580:         {
00581:             var inv = EffectiveInventory;
00582:             if (inv == null) return null;
00583:
00586:             {
00587:                 var slot = inv.Slots[i];
00588:                 if (slot?.Item != null && slot.Amount > 0 && slot.Item.IsConsumable())
00589:                 {
00590:                     if (slot.Item.type == ItemType.Water ||
00591:                         slot.Item.type == ItemType.IrradiatedWater ||
00592:                         slot.Item.thirstRestore > 0f)
00593:                         return slot.Item.id;
00594:                 }
00595:             }
00596:             return null;
00597:         }
00598:
00599:         public string? FindAvailableAntiRadItemId()
00600:         {
00601:             var inv = EffectiveInventory;
00602:             if (inv == null) return null;
00603:
00608:             {
00609:                 var slot = inv.Slots[i];
00610:                 if (slot?.Item != null && slot.Amount > 0 && slot.Item.IsConsumable())
00611:                 {
00612:                     if (slot.Item.type == ItemType.AntiRad ||
00613:                         slot.Item.type == ItemType.Iodine ||
00614:                         slot.Item.radCleanse > 0f)
00622:         /// Heal health directly.
00623:         /// </summary>
00624:         public void Heal(int amount)
00625:         {
00626:             if (amount <= 0) return;
00627:             if (Survivors != null)
00628:             {
00639:
00640:         /// <summary>
00641:         /// Visit a location. Applies radiation exposure based on zone,
00642:         /// advances quests, and consumes resources. Returns a narrative summary.
00643:         /// </summary>
00644:         public string VisitLocation(string locationId)
00645:         {
00646:             if (IsDead) return "The ledger is closed. No more journeys.";
00647:
00648:             // Look up location in catalog
00649:             var loc = FindLocation(locationId);
00650:
00651:             // Apply radiation from location
00652:             float radExposure = loc?.baseRadsPerHour ?? 4f;
00653:
00654:             // Reduce exposure if wearing protective gear
00655:             int maskCount = Trade.GetHeld("gas_mask");
00656:             int hazmatCount = Trade.GetHeld("hazmat_suit");
00657:             if (hazmatCount > 0) radExposure *= 0.3f;
00658:             else if (maskCount > 0) radExposure *= 0.6f;
00659:
00660:             ExposeRadiation(radExposure);
00668:             }
00669:
00670:             string displayName = loc?.displayName ?? locationId;
00671:             string radNote = radExposure > 0 ? $" Exposure: +{radExposure:F0} mSv." : " No contamination detected.";
00672:             string questNote = questAdvanced ? " Quest progress updated." : "";
00673:
00674:             StateChanged?.Invoke();
00675:             return $"Visited {displayName}.{radNote}{questNote}";
00676:         }
00677:
00678:         private HoldfastLocationEntry? FindLocation(string id)
00679:         {
00680:             var locs = World.Catalog?.Locations;
00681:             if (locs == null) return null;
00682:             for (int i = 0; i < locs.Count; i++)
00683:                 if (locs[i] != null && locs[i].id == id)
00684:                     return locs[i];
00688:         // ── Quest Status ───────────────────────────────────────────────
00689:
00690:         public string GetQuestSummary()
00691:         {
00692:             if (World.Quests == null) return "No quest system available.";
00693:
00694:             var sb = new System.Text.StringBuilder();
00695:             for (int i = 0; i < HoldfastQuestSystem.MainQuestIds.Length; i++)
00696:             {
00697:                 string qid = HoldfastQuestSystem.MainQuestIds[i];
00698:                 string name = World.Quests.GetDisplayName(qid);
00699:                 bool started = World.Quests.IsStarted(qid);
00700:                 bool completed = World.Quests.IsCompleted(qid);
00701:                 string status = completed ? "[DONE]" : (started ? "[IN PROGRESS]" : "[LOCKED]");
00702:                 sb.Append($"{status} {name}\n");
00703:                 if (started && !completed)
00704:                 {
00705:                     string stageText = World.Quests.GetStageText(qid);
00706:                     if (!string.IsNullOrEmpty(stageText))
00707:                         sb.Append($"  → {stageText}\n");
00708:                 }
00709:             }
00710:             return sb.ToString().TrimEnd();
00711:         }
00712:
00713:         private string DetermineDeathCause()
00714:         {
00715:             if (Thirst >= MaxThirst)
00716:                 return "Dehydration. The water ran out three days ago. The body lasted longer than expected.";
00717:             if (Hunger >= MaxHunger)
00718:                 return "Starvation. The shelves were bare. The last can was opened on Day " + (Day - 5) + ".";
00719:             if (Radiation >= 200)
00720:                 return "Acute radiation syndrome. The dosimeter stopped counting. So did you.";
00721:             if (Radiation >= 100)
00722:                 return "Radiation sickness. The symptoms were textbook. The treatment was not available.";
00723:             return "The bunker fell silent. The ledger closes here.";
00724:         }
00725:
00726:         public bool ArchiveAndFreshStart(string basePathOverride = null!, string tradePathOverride = null!)
00727:         {
00728:             try
00729:             {
00730:                 string basePath = basePathOverride ?? HoldfastSaveStore.SavePath;
00731:                 string tradePath = tradePathOverride ?? HoldfastTradeSaveStore.SavePath;
00732:                 string timestamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss", System.Globalization.CultureInfo.InvariantCulture); // DETERMINISM_ALLOWLIST: Archive folder timestamp
00733:                 string archiveDir = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(basePath) ?? string.Empty, "holdfast_archive_" + timestamp);
00734:
00735:                 bool archived = true;
00736:                 if (System.IO.File.Exists(basePath))
00737:                 {
00738:                     try { System.IO.Directory.CreateDirectory(archiveDir); System.IO.File.Move(basePath, System.IO.Path.Combine(archiveDir, System.IO.Path.GetFileName(basePath))); }
00739:                     catch (Exception) { archived = false; }
00740:                 }
00741:                 if (System.IO.File.Exists(tradePath))
00742:                 {
00743:                     try { System.IO.Directory.CreateDirectory(archiveDir); System.IO.File.Move(tradePath, System.IO.Path.Combine(archiveDir, System.IO.Path.GetFileName(tradePath))); }
00744:                     catch (Exception) { archived = false; }
00745:                 }
00746:
00747:                 // Reset mutable state.
00748:                 Trade.ResetToDefaults();
00749:                 HasPurchasedThisSession = false;
00750:                 LastPersistenceMessage = archived
00751:                     ? "New ledger started. Prior records archived to " + System.IO.Path.GetFileName(archiveDir) + "."
00752:                     : "New ledger started. Prior records could not be archived but have been cleared.";
00753:                 StateChanged?.Invoke();
00754:                 return true;
00755:             }
00756:             catch (Exception e)
00757:             {
00758:                 LastPersistenceMessage = "Fresh start failed: " + e.Message;
00759:                 return false;
00760:             }
00761:         }
00762:     }
00763: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **The subject is a presentation overlay around the current Holdfast trade transaction. The plan expands identity separation, event truth, bounded logging, tone and accessibility while leaving price, stock, trust and persistence to their current owners.**.

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
