# Plan 100 — Moral Choice Faction Reactions and Threshold Feedback

> **Rebuild status:** COMPLETE 6-ROW CATALOG — MAIN MORAL-CHOICE REACTION PATH EXISTS; CROSS-SYSTEM AUDIT REMAINS
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-5`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round5-2026-09-25`
>
> **Current-evidence date:** 2026-09-25
>
> **Authority order:** live source and data → `AGENTS.md` → `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` → plan ledgers → this document.
>
> **Authority checksum:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
>
> **Length policy:** 150k–170k is the first quality checkpoint; 250k is an evidence-backed depth target, not a ceiling. The plan may exceed 250k when verified current architecture and evidence justify it, and it must stop rather than pad when that evidence is exhausted.

## 0. Integrity Statement and Plan Status

This file replaces unclaimed generated sections that mixed current evidence, fictional APIs, and unsupported save claims. It is a planning and architecture artifact only. It authorizes no production, data, test, save, generated-index, or UI edits. Every path labeled current must exist at rebuild time. Any future `CREATE` proposal is explicitly hypothetical and belongs to a later, separately claimed implementation package.

The rebuild follows four passes: content/current-reality first; integration framework second; accuracy and contradiction removal third; independent precision and handoff review fourth. Character count is recorded by external verification, not embedded recursively in the document.

# 1. Objective

- Six keys cover bounty, contract-taking, contract-raising, patrol defense, positive legend and negative legend thresholds.
- MoralChoiceFactionReactionsData and its loader own static dialogue records; Main.MoralChoice owns host loading and journal projection.
- The content scanner names MoralChoiceSystem/MoralChoicePanel as related consumers, so the plan must inspect the actual current call graph before expanding behavior.

**Bounded outcome:** Retire the 1-to-6 data target. The current catalog has six threshold reactions, the loader maps them, and Main.MoralChoice loads and journals the authored reaction. The remaining work is to prove threshold event semantics, deduplication, UI visibility and continuity with the current moral-choice flag/gossip owners, not to add a second reaction engine.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- moral_choice_faction_reactions.json is schema version 1 with six threshold_reactions keyed by moral_event_* IDs.
- MoralChoiceFactionReactionsCatalogLoader maps nested faction dialogue records into Core data.
- Main.MoralChoice loads the catalog, resolves a threshold event and journals the reaction text; moral choice, flags and gossip remain separate owners.

**Master-authority sections applied to this rebase:**

- Part II factory protocol: establish current reality, reject duplicate authority, and name one safe extension seam before design.
- Part II continuity checklist: data presence, a loader, a host route, a player-visible outcome, and persistence are separate proofs.
- Part III cluster map: preserve the current Core owner and route cross-system effects through typed facts rather than panel copies.
- Part VI Multi-Session Growth Protocol: 250k is an evidence-backed depth target, not a mandate to manufacture prose or row count.
- Live source/data authority outranks this plan; a future audit that contradicts a current declaration returns the package to STALE_PLAN.
- Anti-padding rule: preserve completed work as maintenance scope and spend detail only on proven residual gaps.
- Part II current-reality rule: JSON presence, a loader, a host route, a player-visible outcome, and persistence are separate proofs.
- Part III one-authority rule: extend the existing owner and route typed facts through it; do not create a second ledger, save store, registry, simulation, or panel cache.
- Part VI replay rule: any new randomness must use the existing seeded campaign stream and stable ordinal ordering; no System.Random or wall-clock decision path.
- Part VI save rule: a new mutable field is incomplete until CaptureState, RestoreState, old-save defaults, and checksum migration are specified.
- Part VII UI rule: presentation projects owner state and routes real commands; it never becomes a gameplay authority or a fake operational route.
- Part VIII quality rule: the 150k–170k band is an initial completeness checkpoint; 250k is an evidence-backed depth target, not a ceiling or a reason to pad.
- Live source/data evidence outranks the original plan. If a future audit contradicts a declaration here, the package returns to STALE_PLAN rather than reviving an obsolete API.
- C7 moral cluster: choice state, flag state, gossip propagation and authored reaction feedback have separate owners.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the old count target with a six-row threshold/event reachability census.
- Define event-ID normalization, one-shot/dedup behavior and exact journal/panel projection against the current moral-choice owner.
- Audit whether every threshold is emitted by a current choice/flag event and whether all faction dialogue variants are visible.
- No new moral state, reaction save or duplicate faction dialogue registry.

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
| static threshold reaction parsing | MoralChoiceFactionReactionsCatalogLoader | `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsCatalogLoader.cs` | Static content owner. |
| Core reaction DTO and lookup shape | MoralChoiceFactionReactionsData | `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsData.cs` | No mutable reaction state. |
| host loading, event resolution and journal projection | Main.MoralChoice | `src/Main.MoralChoice.cs` | Thin host composition and presentation fact. |
| choice resolution and flag ownership | MoralChoiceSystem | `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` | Owns choices, not static dialogue. |
| current visible feedback surface | Moral choice panel | `src/UI/MoralChoiceModal.cs` | Read-only projection; verify path exists before implementation. |
| catalog and reaction contract | MoralChoiceFactionReactionsExpansionTests | `Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs` | Focused evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Moral Choice Faction Reactions and Threshold Feedback
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ MoralChoiceFactionReactionsCatalogLoader
│   static threshold reaction parsing
│ MoralChoiceFactionReactionsData
│   Core reaction DTO and lookup shape
│ Main.MoralChoice
│   host loading, event resolution and journal projection
│ MoralChoiceSystem
│   choice resolution and flag ownership
│ Moral choice panel
│   current visible feedback surface
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

1. **Preserve current state ownership.** MoralChoiceFactionReactionsCatalogLoader owns static threshold reaction parsing: Static content owner.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| static threshold reaction parsing | MoralChoiceFactionReactionsCatalogLoader | `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsCatalogLoader.cs` | Static content owner. |
| Core reaction DTO and lookup shape | MoralChoiceFactionReactionsData | `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsData.cs` | No mutable reaction state. |
| host loading, event resolution and journal projection | Main.MoralChoice | `src/Main.MoralChoice.cs` | Thin host composition and presentation fact. |
| choice resolution and flag ownership | MoralChoiceSystem | `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` | Owns choices, not static dialogue. |
| current visible feedback surface | Moral choice panel | `src/UI/MoralChoiceModal.cs` | Read-only projection; verify path exists before implementation. |
| catalog and reaction contract | MoralChoiceFactionReactionsExpansionTests | `Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs` | Focused evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load six reaction records
2. normalize and validate event IDs
3. resolve a current threshold event through Main.MoralChoice
4. look up authored faction dialogue
5. project the current speaker/location/lines
6. journal or display once through the existing route
7. preserve current moral/gossip/flag state

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Threshold reaction definitions are immutable catalog rows.
- Resolved moral choices, flags, gossip and reputation remain with their current systems.
- A reaction is a fact attached to a threshold event, not a second lasting faction relationship.
- No reaction-specific save section is justified unless a current owner adopts a durable seen-state.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Only a current emitted event may trigger a reaction.
- Unknown event IDs return no reaction without mutating any owner.
- Dialogue content is presentation; it cannot change trust, guilt or faction state.
- Same flags, event and day produce the same reaction selection.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- moral_choice_faction_reactions.json is the sole reaction catalog.
- moral_choice_flags.json, moral_choice_chains.json and gossip catalogs own their separate semantics.
- No duplicated dialogue file is justified.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use current moral-choice/flag/gossip save owners.
- No new reaction save section.
- If one-shot delivery is later needed, persist it through the existing flag or consequence ledger, with a migration proof.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Reaction lookup is dictionary access by canonical event ID.
- Any ordering of dialogue variants must be catalog order or seeded selection through the current RNG.
- No wall-clock timestamps enter the reaction record.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- MoralChoiceSystem emits the current choice/flag fact.
- Main.MoralChoice resolves the matching authored reaction and journals it.
- A repeated event must follow current flag/event deduplication and cannot spam the journal.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Main.MoralChoice.cs
- src/UI/MoralChoiceModal.cs
- src/Main.Application.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Three faction voices remain fictional, restrained and distinct.
- Reactions acknowledge player action without becoming moral lectures or exposition dumps.
- A missing speaker/location is a data defect, not permission to invent a live NPC.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A reaction is displayed for an event the current system never emitted. | MoralChoiceFactionReactionsCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A journal entry repeats on every panel refresh. | MoralChoiceFactionReactionsData | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Reaction text mutates faction trust. | Main.MoralChoice | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A missing key crashes a save restore. | MoralChoiceSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new panel caches the entire moral state. | Moral choice panel | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceSystemTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — six-row event census | Map each event key to its current emitter and visible result. | No phantom threshold remains. | No production path until the owning implementation package is separately claimed. |
| 1 — host reachability trace | Trace Main.MoralChoice and panel calls through the current owner. | Reaction feedback is proven or marked residual. | No production path until the owning implementation package is separately claimed. |
| 2 — dedup/restore audit | Check repeat, reload, missing key and invalid data behavior. | No journal spam or state leak. | No production path until the owning implementation package is separately claimed. |
| 3 — prose/UI polish | Review faction voice, focus, overflow and truth labels. | Content improves without a new authority. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/moral_choice_faction_reactions.json | READ ONLY; MODIFY only for proven content/reference defect | Six reaction rows |
| Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsCatalogLoader.cs | READ ONLY | Loader |
| src/Main.MoralChoice.cs | READ ONLY | Host projection |
| src/UI/MoralChoiceModal.cs | READ ONLY; verify before future change | Visible feedback |
| Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs | READ ONLY | Focused contract |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Creating a second moral reaction state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Assuming a test name proves a live caller. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Journal spam from repeated threshold evaluation. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Dialogue being treated as a gameplay command. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding a save section for presentation-only content. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new reaction rows without a current event gap.
- No new moral engine.
- No trust/guilt mutation from dialogue.
- No production/data/test/UI changes in this planning package.

# 23. Rollback and Recovery

- Revert the planning artifact.
- Future visibility changes are removable without save migration if they remain projection-only.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- Six current reactions and their event ownership are explicit.
- The plan distinguishes loader success from live threshold reachability.
- Dedup, save, UI and failure behavior are specified.
- Focused runner commands are exact.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the old count target with a six-row threshold/event reachability census.
- Define event-ID normalization, one-shot/dedup behavior and exact journal/panel projection against the current moral-choice owner.
- Audit whether every threshold is emitted by a current choice/flag event and whether all faction dialogue variants are visible.
- No new moral state, reaction save or duplicate faction dialogue registry.

## MUST NOT DO

- No new reaction rows without a current event gap.
- No new moral engine.
- No trust/guilt mutation from dialogue.
- No production/data/test/UI changes in this planning package.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceSystemTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — six-row event census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: static threshold reaction parsing → MoralChoiceFactionReactionsCatalogLoader; Core reaction DTO and lookup shape → MoralChoiceFactionReactionsData; host loading, event resolution and journal projection → Main.MoralChoice; choice resolution and flag ownership → MoralChoiceSystem; current visible feedback surface → Moral choice panel; catalog and reaction contract → MoralChoiceFactionReactionsExpansionTests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 100.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 100 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by MoralChoiceFactionReactionsCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsData.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsData.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 33 lines / 1479 bytes.
- SHA-256: `a78b6348a5c75bf28cd49264bd249fd92ec07a0c3f32f75c4c90855247ff8a13`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MoralChoiceFactionReactionsData
public Dictionary<string, MoralThresholdReaction> ThresholdReactions { get; set; }
public sealed class MoralThresholdReaction
public string EventDescription { get; set; } = string.Empty;
public List<MoralFactionDialogue> PeacekeeperDialogue { get; set; } = new List<MoralFactionDialogue>();
public List<MoralFactionDialogue> RaiderDialogue { get; set; } = new List<MoralFactionDialogue>();
public List<MoralFactionDialogue> KnowledgeKeeperDialogue { get; set; } = new List<MoralFactionDialogue>();
public List<MoralFactionDialogue> CivilianDialogue { get; set; } = new List<MoralFactionDialogue>();
public string JournalEntry { get; set; } = string.Empty;
public sealed class MoralFactionDialogue
public string Speaker { get; set; } = string.Empty;
public string Location { get; set; } = string.Empty;
public List<string> Lines { get; set; } = new List<string>();
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsCatalogLoader.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 95 lines / 4036 bytes.
- SHA-256: `f99e21f5283583177721051f5cde3736ac899f01939f033e1dad9b9d03b3877e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MoralChoiceFactionReactionsContainer
public int schema_version = 1;
public string description = string.Empty;
public sealed class MoralThresholdReactionRecord
public string event_description = string.Empty;
public List<MoralFactionDialogueRecord> peacekeeper_dialogue = new List<MoralFactionDialogueRecord>();
public List<MoralFactionDialogueRecord> raider_dialogue = new List<MoralFactionDialogueRecord>();
public List<MoralFactionDialogueRecord> knowledge_keeper_dialogue = new List<MoralFactionDialogueRecord>();
public List<MoralFactionDialogueRecord> civilian_dialogue = new List<MoralFactionDialogueRecord>();
public string journal_entry = string.Empty;
public sealed class MoralFactionDialogueRecord
public string speaker = string.Empty;
public string location = string.Empty;
public List<string> lines = new List<string>();
public static class MoralChoiceFactionReactionsCatalogLoader
public const string DefaultFileName = "moral_choice_faction_reactions.json";
public static MoralChoiceFactionReactionsData Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 687 lines / 30717 bytes.
- SHA-256: `4cb9adafbbbdfc153d1c80ac2595f1e6c870a6669e2c9e85844a6a416133c9c1`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum MoralPathBand
public enum MoralEndingKind
public sealed class MoralChoiceQuestDefinition
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string Category { get; set; } = string.Empty;
public string Trigger { get; set; } = string.Empty;
public string Discovery { get; set; } = string.Empty;
public string LocationId { get; set; } = string.Empty;
public int MinDay { get; set; }
public int MaxDay { get; set; }
public List<MoralChoiceOption> Choices { get; set; } = new List<MoralChoiceOption>();
public sealed class MoralChoiceOption
public string Label { get; set; } = string.Empty;
public int MoralDelta { get; set; }
public int EmpathyDelta { get; set; }
public string SetFlag { get; set; } = string.Empty;
public string OutcomeText { get; set; } = string.Empty;
public string Epitaph { get; set; } = string.Empty;
public sealed class MoralChoiceSystem
public const string SystemId = "moral_choice";
public const string QuestIdPrefix = "quest_moral_";
public const int MinScore = -200;
public const int MaxScore = 200;
public const int ListenerEmpathyThreshold = 15;
public const int ConfidantEmpathyThreshold = 30;
public const int StorykeeperEmpathyThreshold = 45;
public const int StorykeeperQuestThreshold = 25;
public const int EndingLockMinQuests = 20;
public const string EventLegendPositive = "moral_event_legend_positive";
public const string EventLegendNegative = "moral_event_legend_negative";
public const string EventBountyIssued = "moral_event_bounty_issued";
public const string EventContractTaken = "moral_event_contract_taken";
public const string EventContractRaised = "moral_event_contract_raised";
public const string EventPatrolDefense = "moral_event_patrol_defense";
public const int LegendPositiveFlag = 1;
public const int LegendNegativeFlag = 2;
public event Action<MoralChoiceResolution>? OnQuestResolved;
public event Action<string>? OnThresholdEventFired;
public event Action<string>? OnBranchLocked;
public MoralChoiceState State => _state;
public int MoralScore => _state.moralScore;
public int EmpathyPoints => _state.empathyPoints;
public int QuestsResolved => _state.resolutions.Count;
public MoralPathBand CurrentBand => BandForScore(_state.moralScore);
public IReadOnlyList<MoralChoiceResolution> Resolutions => _state.resolutions;
public bool IsListener => _state.empathyPoints >= ListenerEmpathyThreshold;
public bool IsConfidant => _state.empathyPoints >= ConfidantEmpathyThreshold;
public MoralChoiceChainData? ChainData => _chainData;
public void InitializeChainData(MoralChoiceChainData chainData) {
public static bool IsCanonicalQuestId(string questId) =>
public static bool IsAvailableOnDay(MoralChoiceQuestDefinition quest, int day) =>
public bool IsResolved(string questId) => TryGetResolution(questId, out _);
public bool TryGetResolution(string questId, out MoralChoiceResolution? resolution) {
public void RegisterQuest(MoralChoiceQuestDefinition def) {
public void RegisterQuests(IEnumerable<MoralChoiceQuestDefinition> defs) {
public IReadOnlyDictionary<string, MoralChoiceQuestDefinition> Catalog => _catalog;
public int CatalogCount => _catalog.Count;
public MoralChoiceQuestDefinition? GetQuest(string id) =>
public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyOffers(int day, int maxOffers = 1) {
public bool TryResolve(string questId, int choiceIndex, string locationId, int day, out MoralResolveResult result) {
public bool IsBranchLocked(string branchId) =>
public int GetBranchProgress(string branchId) =>
public string GetQuestBranch(string questId) =>
public bool IsChainQuestAccessible(string questId, int day) {
public bool EvaluateGate(MoralQuestGate gate) {
public void SetFlag(string flagId) {
public bool HasFlag(string flagId) =>
public List<MoralEchoQuestDefinition> FindAvailableEchoQuests(int currentDay) {
public void MarkEchoQuestFired(string echoQuestId) {
public MoralChoiceResolution Resolve(MoralChoiceQuestDefinition quest, int choiceIndex, string locationId, int day) {
public void Reconcile(int day) {
public MoralEndingKind SelectEnding() =>
public static MoralEndingKind SelectEnding(int moralScore, int empathyPoints, int questsResolved) {
public static MoralPathBand BandForScore(int score) {
public MoralChoiceState CaptureState() => Clone(_state);
public void RestoreState(MoralChoiceState state) {
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagDefinitions.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagDefinitions.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 20 lines / 634 bytes.
- SHA-256: `764eda5eb3dce4983316d0a4b40463a539e49aa5ff7cd4a1031e0f733be970de`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MoralChoiceFlagDefinitions
public List<MoralFlagDefinition> Flags { get; set; } = new List<MoralFlagDefinition>();
public sealed class MoralFlagDefinition
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
```


# Appendix B.06 — Current Code Architecture: `src/Main.MoralChoice.cs`

### `src/Main.MoralChoice.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 344 lines / 16147 bytes.
- SHA-256: `e12534c6d2a8cd86c90d7283de173c51bf07cd68e46bb52f6129bbeb05375b8d`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=4; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public MoralChoiceSystem MoralChoice => _moralChoice;
public IReadOnlyList<MoralChoiceQuestDefinition> MoralChoiceDefs => _moralChoiceDefs;
public MoralChoiceQuestDefinition? GetMoralChoiceDef(string questId) {
public List<MoralChoiceQuestDefinition> GetAvailableMoralChoices() {
public const string TrappingMoralQuestIdPrefix = "quest_moral_trap_prey_";
public List<MoralChoiceQuestDefinition> GetResolvedMoralChoices() {
public MoralChoiceResolution? GetMoralChoiceResolution(string questId) {
public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyMoralOffers(int maxOffers = 1) {
public bool TryResolveMoralChoice(string questId, int choiceIndex) {
```


# Appendix B.07 — Current Code Architecture: `src/UI/MoralChoiceModal.cs`

### `src/UI/MoralChoiceModal.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 317 lines / 14077 bytes.
- SHA-256: `ef9e9b6e34bb952eb6ac37133cccd919dfc0ef4135b4831ff0085ff6662da334`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=6; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class MoralChoiceModal : Control, IModalPanel
public event Action<string, int>? OnChoiceSelected;
public event Action? OnClose;
public event Action? OnModalClosed;
public bool IsModalOpen => Visible;
public Control? InitialFocusControl => _firstInteractiveButton ?? _closeButton;
public override void _Ready() {
public void Bind( MoralChoiceQuestDefinition quest, MoralChoiceSystem? moralChoiceSystem = null, Action<string, int>? onChoiceCallback = null) {
public void RefreshContent() {
public void Open() {
public void SelectChoiceForTest(int choiceIndex) => ExecuteChoice(choiceIndex);
public void CloseModal() {
public override void _UnhandledInput(InputEvent @event) {
```


# Appendix B.08 — Current Code Architecture: `src/Main.Application.cs`

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


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/moral_choice_faction_reactions.json`

### `Assets/StreamingAssets/Data/moral_choice_faction_reactions.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 14988 bytes / 14950 characters.
- SHA-256: `fd9f4c31b6dd1c9779bc820d7b02923901257b704f930c3572e6d2d57933950d`.
- Root keys: `description`, `schema_version`, `threshold_reactions`.

Array-path census (minimum, maximum, observed rows):

```text
threshold_reactions.moral_event_bounty_issued.knowledge_keeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_bounty_issued.knowledge_keeper_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_bounty_issued.peacekeeper_dialogue: min=2, max=2, observed_paths=1
threshold_reactions.moral_event_bounty_issued.peacekeeper_dialogue[].lines: min=3, max=5, observed_paths=2
threshold_reactions.moral_event_bounty_issued.raider_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_bounty_issued.raider_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_contract_raised.knowledge_keeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_contract_raised.knowledge_keeper_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_contract_raised.peacekeeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_contract_raised.peacekeeper_dialogue[].lines: min=6, max=6, observed_paths=1
threshold_reactions.moral_event_contract_raised.raider_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_contract_raised.raider_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_contract_taken.knowledge_keeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_contract_taken.knowledge_keeper_dialogue[].lines: min=3, max=3, observed_paths=1
threshold_reactions.moral_event_contract_taken.peacekeeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_contract_taken.peacekeeper_dialogue[].lines: min=5, max=5, observed_paths=1
threshold_reactions.moral_event_contract_taken.raider_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_contract_taken.raider_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_legend_negative.civilian_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_legend_negative.civilian_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_legend_negative.knowledge_keeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_legend_negative.knowledge_keeper_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_legend_negative.peacekeeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_legend_negative.peacekeeper_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_legend_negative.raider_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_legend_negative.raider_dialogue[].lines: min=5, max=5, observed_paths=1
threshold_reactions.moral_event_legend_positive.civilian_dialogue: min=2, max=2, observed_paths=1
threshold_reactions.moral_event_legend_positive.civilian_dialogue[].lines: min=3, max=4, observed_paths=2
threshold_reactions.moral_event_legend_positive.knowledge_keeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_legend_positive.knowledge_keeper_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_legend_positive.peacekeeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_legend_positive.peacekeeper_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_legend_positive.raider_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_legend_positive.raider_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_patrol_defense.knowledge_keeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_patrol_defense.knowledge_keeper_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_patrol_defense.peacekeeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_patrol_defense.peacekeeper_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_patrol_defense.raider_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_patrol_defense.raider_dialogue[].lines: min=4, max=4, observed_paths=1
```


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/moral_choice_flags.json`

### `Assets/StreamingAssets/Data/moral_choice_flags.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 2090 bytes / 2090 characters.
- SHA-256: `e5a95235ce28f9d2a1c9c8bcaeb72789122d9d77e1d38b0658cc86ae4d6a4db4`.
- Root keys: `description`, `flags`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
flags: min=25, max=25, observed_paths=1
```

Representative record fields:

- `display_name`
- `id`

Representative identifiers (ordered, capped for readability):

```text
flag_betrayed_ally
flag_betrayed_faction
flag_betrayed_trust
flag_broken_pact
flag_become_warlord
flag_throne_of_ash
flag_branch_mercy_road_locked
flag_branch_iron_way_locked
flag_branch_listener_locked
flag_branch_broken_compact_locked
flag_spared_raider
flag_executed_prisoner
flag_shared_rations
flag_hoarded_medicine
flag_sheltered_refugee
flag_expelled_survivor
flag_repaired_infrastructure
flag_sabotaged_rival
flag_broke_treaty
flag_honored_debt
flag_ignored_distress
flag_responded_distress
flag_forged_record
flag_preserved_archive
flag_chosen_faction_side
```


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/moral_choice_chains.json`

### `Assets/StreamingAssets/Data/moral_choice_chains.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 31788 bytes / 31784 characters.
- SHA-256: `2a30c42686baef9f1e64cdda74fdb13cbe9204c641790d47212e5aeb48fc2135`.
- Root keys: `branches`, `description`, `echo_quests`, `faction_reactions`, `gossip_propagation`, `lockout_rules`, `merge_rules`, `quest_gates`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
branches: min=4, max=4, observed_paths=1
branches[].entry_quests: min=3, max=3, observed_paths=2
branches[].locks_out: min=2, max=2, observed_paths=2
branches[].merge_allowed: min=1, max=1, observed_paths=2
echo_quests.quests: min=60, max=60, observed_paths=1
quest_gates: min=88, max=88, observed_paths=1
quest_gates[].requires: min=1, max=1, observed_paths=2
```

Representative record fields:

- `description`
- `display_name`
- `entry_quests`
- `id`
- `lock_threshold`
- `locked_flag`
- `locks_out`
- `merge_allowed`

Representative identifiers (ordered, capped for readability):

```text
branch_mercy_road
branch_iron_way
branch_listener_thread
branch_broken_compact
```


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs`

### `Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 248; SHA-256: `71802e0882c69e830cd7c2bd3d0530f55223a56ba01afa2dfb6c8b6c9d51be5f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsExactSixCanonicalReactions
Catalog_ContainsAllSixCanonicalEventIds
Catalog_EveryReactionHasNonEmptyEventDescription
Catalog_EveryReactionHasAllThreeFactionDialogues
Catalog_AllDialogueBlocksHaveValidSpeakerLocationAndLines
Catalog_EveryReactionHasNonEmptyJournalEntry
Catalog_PreservesBountyIssuedBaselineParity
Runtime_BandCrossings_TriggerAllCanonicalReactionsOnce
Runtime_OverflowLegends_TriggerOnceOvernight
Runtime_StateCaptureAndRestore_PreservesFiredEvents
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/MoralChoiceSystemTests.cs`

### `Ashfall.Core.Tests/MoralChoiceSystemTests.cs`

- Current test declarations: Fact=22, Theory=0, InlineData=0.
- File lines: 428; SHA-256: `43c39778ac33913ad3ea4cb255b57fb54511909b6560e169bb599e20dc6ed4ec`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
InitialStateNeutralAndEmpty
BandEdgesPinned
ResolveAppliesDeltasAndRaisesEvent
ResolveIsIdempotentPerQuest
ScoreClampsAndLegendSettlesAtReconcileOncePerDirection
ResolveRejectsNonCanonicalQuestId
ResolveRejectsOutOfRangeChoice
ImpactMarksFollowDeltaSign
SameSeedSameRolls
ReconcileFiresExtremeBandEventsOnce
ReconcileFiresAllCrossedBandsOnBigJump
ReconcileFiresContractAtPositiveBand
ReconcileIgnoresOutOfOrderDays
EndingStorykeeperOverridesBand
EndingSelectionRules
StorykeeperNeedsBothThresholds
ListenerAndConfidantThresholdsPinnedAtBoundary
RestoreRejectsMismatchedSystemAndBadSchema
PendingLegendFlagsSurviveRoundTrip
SaveRoundTripPreservesLedger
CapturedStateIsDetached
AvailabilityWindow
```


# Appendix E.14 — Supporting Code Evidence: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsData.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsData.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 33 lines / 1479 bytes.
- SHA-256: `a78b6348a5c75bf28cd49264bd249fd92ec07a0c3f32f75c4c90855247ff8a13`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MoralChoiceFactionReactionsData
public Dictionary<string, MoralThresholdReaction> ThresholdReactions { get; set; }
public sealed class MoralThresholdReaction
public string EventDescription { get; set; } = string.Empty;
public List<MoralFactionDialogue> PeacekeeperDialogue { get; set; } = new List<MoralFactionDialogue>();
public List<MoralFactionDialogue> RaiderDialogue { get; set; } = new List<MoralFactionDialogue>();
public List<MoralFactionDialogue> KnowledgeKeeperDialogue { get; set; } = new List<MoralFactionDialogue>();
public List<MoralFactionDialogue> CivilianDialogue { get; set; } = new List<MoralFactionDialogue>();
public string JournalEntry { get; set; } = string.Empty;
public sealed class MoralFactionDialogue
public string Speaker { get; set; } = string.Empty;
public string Location { get; set; } = string.Empty;
public List<string> Lines { get; set; } = new List<string>();
```


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsCatalogLoader.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 95 lines / 4036 bytes.
- SHA-256: `f99e21f5283583177721051f5cde3736ac899f01939f033e1dad9b9d03b3877e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MoralChoiceFactionReactionsContainer
public int schema_version = 1;
public string description = string.Empty;
public sealed class MoralThresholdReactionRecord
public string event_description = string.Empty;
public List<MoralFactionDialogueRecord> peacekeeper_dialogue = new List<MoralFactionDialogueRecord>();
public List<MoralFactionDialogueRecord> raider_dialogue = new List<MoralFactionDialogueRecord>();
public List<MoralFactionDialogueRecord> knowledge_keeper_dialogue = new List<MoralFactionDialogueRecord>();
public List<MoralFactionDialogueRecord> civilian_dialogue = new List<MoralFactionDialogueRecord>();
public string journal_entry = string.Empty;
public sealed class MoralFactionDialogueRecord
public string speaker = string.Empty;
public string location = string.Empty;
public List<string> lines = new List<string>();
public static class MoralChoiceFactionReactionsCatalogLoader
public const string DefaultFileName = "moral_choice_faction_reactions.json";
public static MoralChoiceFactionReactionsData Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 687 lines / 30717 bytes.
- SHA-256: `4cb9adafbbbdfc153d1c80ac2595f1e6c870a6669e2c9e85844a6a416133c9c1`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum MoralPathBand
public enum MoralEndingKind
public sealed class MoralChoiceQuestDefinition
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string Category { get; set; } = string.Empty;
public string Trigger { get; set; } = string.Empty;
public string Discovery { get; set; } = string.Empty;
public string LocationId { get; set; } = string.Empty;
public int MinDay { get; set; }
public int MaxDay { get; set; }
public List<MoralChoiceOption> Choices { get; set; } = new List<MoralChoiceOption>();
public sealed class MoralChoiceOption
public string Label { get; set; } = string.Empty;
public int MoralDelta { get; set; }
public int EmpathyDelta { get; set; }
public string SetFlag { get; set; } = string.Empty;
public string OutcomeText { get; set; } = string.Empty;
public string Epitaph { get; set; } = string.Empty;
public sealed class MoralChoiceSystem
public const string SystemId = "moral_choice";
public const string QuestIdPrefix = "quest_moral_";
public const int MinScore = -200;
public const int MaxScore = 200;
public const int ListenerEmpathyThreshold = 15;
public const int ConfidantEmpathyThreshold = 30;
public const int StorykeeperEmpathyThreshold = 45;
public const int StorykeeperQuestThreshold = 25;
public const int EndingLockMinQuests = 20;
public const string EventLegendPositive = "moral_event_legend_positive";
public const string EventLegendNegative = "moral_event_legend_negative";
public const string EventBountyIssued = "moral_event_bounty_issued";
public const string EventContractTaken = "moral_event_contract_taken";
public const string EventContractRaised = "moral_event_contract_raised";
public const string EventPatrolDefense = "moral_event_patrol_defense";
public const int LegendPositiveFlag = 1;
public const int LegendNegativeFlag = 2;
public event Action<MoralChoiceResolution>? OnQuestResolved;
public event Action<string>? OnThresholdEventFired;
public event Action<string>? OnBranchLocked;
public MoralChoiceState State => _state;
public int MoralScore => _state.moralScore;
public int EmpathyPoints => _state.empathyPoints;
public int QuestsResolved => _state.resolutions.Count;
public MoralPathBand CurrentBand => BandForScore(_state.moralScore);
public IReadOnlyList<MoralChoiceResolution> Resolutions => _state.resolutions;
public bool IsListener => _state.empathyPoints >= ListenerEmpathyThreshold;
public bool IsConfidant => _state.empathyPoints >= ConfidantEmpathyThreshold;
public MoralChoiceChainData? ChainData => _chainData;
public void InitializeChainData(MoralChoiceChainData chainData) {
public static bool IsCanonicalQuestId(string questId) =>
public static bool IsAvailableOnDay(MoralChoiceQuestDefinition quest, int day) =>
public bool IsResolved(string questId) => TryGetResolution(questId, out _);
public bool TryGetResolution(string questId, out MoralChoiceResolution? resolution) {
public void RegisterQuest(MoralChoiceQuestDefinition def) {
public void RegisterQuests(IEnumerable<MoralChoiceQuestDefinition> defs) {
public IReadOnlyDictionary<string, MoralChoiceQuestDefinition> Catalog => _catalog;
public int CatalogCount => _catalog.Count;
public MoralChoiceQuestDefinition? GetQuest(string id) =>
public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyOffers(int day, int maxOffers = 1) {
public bool TryResolve(string questId, int choiceIndex, string locationId, int day, out MoralResolveResult result) {
public bool IsBranchLocked(string branchId) =>
public int GetBranchProgress(string branchId) =>
public string GetQuestBranch(string questId) =>
public bool IsChainQuestAccessible(string questId, int day) {
public bool EvaluateGate(MoralQuestGate gate) {
public void SetFlag(string flagId) {
public bool HasFlag(string flagId) =>
public List<MoralEchoQuestDefinition> FindAvailableEchoQuests(int currentDay) {
public void MarkEchoQuestFired(string echoQuestId) {
public MoralChoiceResolution Resolve(MoralChoiceQuestDefinition quest, int choiceIndex, string locationId, int day) {
public void Reconcile(int day) {
public MoralEndingKind SelectEnding() =>
public static MoralEndingKind SelectEnding(int moralScore, int empathyPoints, int questsResolved) {
public static MoralPathBand BandForScore(int score) {
public MoralChoiceState CaptureState() => Clone(_state);
public void RestoreState(MoralChoiceState state) {
```


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagDefinitions.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagDefinitions.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 20 lines / 634 bytes.
- SHA-256: `764eda5eb3dce4983316d0a4b40463a539e49aa5ff7cd4a1031e0f733be970de`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MoralChoiceFlagDefinitions
public List<MoralFlagDefinition> Flags { get; set; } = new List<MoralFlagDefinition>();
public sealed class MoralFlagDefinition
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
```


# Appendix E.18 — Supporting Code Evidence: `src/Main.MoralChoice.cs`

### `src/Main.MoralChoice.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 344 lines / 16147 bytes.
- SHA-256: `e12534c6d2a8cd86c90d7283de173c51bf07cd68e46bb52f6129bbeb05375b8d`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=4; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public MoralChoiceSystem MoralChoice => _moralChoice;
public IReadOnlyList<MoralChoiceQuestDefinition> MoralChoiceDefs => _moralChoiceDefs;
public MoralChoiceQuestDefinition? GetMoralChoiceDef(string questId) {
public List<MoralChoiceQuestDefinition> GetAvailableMoralChoices() {
public const string TrappingMoralQuestIdPrefix = "quest_moral_trap_prey_";
public List<MoralChoiceQuestDefinition> GetResolvedMoralChoices() {
public MoralChoiceResolution? GetMoralChoiceResolution(string questId) {
public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyMoralOffers(int maxOffers = 1) {
public bool TryResolveMoralChoice(string questId, int choiceIndex) {
```


# Appendix F.19 — Supporting Data Evidence: `Assets/StreamingAssets/Data/moral_choice_faction_reactions.json`

### `Assets/StreamingAssets/Data/moral_choice_faction_reactions.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 14988 bytes / 14950 characters.
- SHA-256: `fd9f4c31b6dd1c9779bc820d7b02923901257b704f930c3572e6d2d57933950d`.
- Root keys: `description`, `schema_version`, `threshold_reactions`.

Array-path census (minimum, maximum, observed rows):

```text
threshold_reactions.moral_event_bounty_issued.knowledge_keeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_bounty_issued.knowledge_keeper_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_bounty_issued.peacekeeper_dialogue: min=2, max=2, observed_paths=1
threshold_reactions.moral_event_bounty_issued.peacekeeper_dialogue[].lines: min=3, max=5, observed_paths=2
threshold_reactions.moral_event_bounty_issued.raider_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_bounty_issued.raider_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_contract_raised.knowledge_keeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_contract_raised.knowledge_keeper_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_contract_raised.peacekeeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_contract_raised.peacekeeper_dialogue[].lines: min=6, max=6, observed_paths=1
threshold_reactions.moral_event_contract_raised.raider_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_contract_raised.raider_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_contract_taken.knowledge_keeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_contract_taken.knowledge_keeper_dialogue[].lines: min=3, max=3, observed_paths=1
threshold_reactions.moral_event_contract_taken.peacekeeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_contract_taken.peacekeeper_dialogue[].lines: min=5, max=5, observed_paths=1
threshold_reactions.moral_event_contract_taken.raider_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_contract_taken.raider_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_legend_negative.civilian_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_legend_negative.civilian_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_legend_negative.knowledge_keeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_legend_negative.knowledge_keeper_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_legend_negative.peacekeeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_legend_negative.peacekeeper_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_legend_negative.raider_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_legend_negative.raider_dialogue[].lines: min=5, max=5, observed_paths=1
threshold_reactions.moral_event_legend_positive.civilian_dialogue: min=2, max=2, observed_paths=1
threshold_reactions.moral_event_legend_positive.civilian_dialogue[].lines: min=3, max=4, observed_paths=2
threshold_reactions.moral_event_legend_positive.knowledge_keeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_legend_positive.knowledge_keeper_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_legend_positive.peacekeeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_legend_positive.peacekeeper_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_legend_positive.raider_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_legend_positive.raider_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_patrol_defense.knowledge_keeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_patrol_defense.knowledge_keeper_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_patrol_defense.peacekeeper_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_patrol_defense.peacekeeper_dialogue[].lines: min=4, max=4, observed_paths=1
threshold_reactions.moral_event_patrol_defense.raider_dialogue: min=1, max=1, observed_paths=1
threshold_reactions.moral_event_patrol_defense.raider_dialogue[].lines: min=4, max=4, observed_paths=1
```


# Appendix F.20 — Supporting Data Evidence: `Assets/StreamingAssets/Data/moral_choice_flags.json`

### `Assets/StreamingAssets/Data/moral_choice_flags.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 2090 bytes / 2090 characters.
- SHA-256: `e5a95235ce28f9d2a1c9c8bcaeb72789122d9d77e1d38b0658cc86ae4d6a4db4`.
- Root keys: `description`, `flags`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
flags: min=25, max=25, observed_paths=1
```

Representative record fields:

- `display_name`
- `id`

Representative identifiers (ordered, capped for readability):

```text
flag_betrayed_ally
flag_betrayed_faction
flag_betrayed_trust
flag_broken_pact
flag_become_warlord
flag_throne_of_ash
flag_branch_mercy_road_locked
flag_branch_iron_way_locked
flag_branch_listener_locked
flag_branch_broken_compact_locked
flag_spared_raider
flag_executed_prisoner
flag_shared_rations
flag_hoarded_medicine
flag_sheltered_refugee
flag_expelled_survivor
flag_repaired_infrastructure
flag_sabotaged_rival
flag_broke_treaty
flag_honored_debt
flag_ignored_distress
flag_responded_distress
flag_forged_record
flag_preserved_archive
flag_chosen_faction_side
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs`

### `Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 248; SHA-256: `71802e0882c69e830cd7c2bd3d0530f55223a56ba01afa2dfb6c8b6c9d51be5f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsExactSixCanonicalReactions
Catalog_ContainsAllSixCanonicalEventIds
Catalog_EveryReactionHasNonEmptyEventDescription
Catalog_EveryReactionHasAllThreeFactionDialogues
Catalog_AllDialogueBlocksHaveValidSpeakerLocationAndLines
Catalog_EveryReactionHasNonEmptyJournalEntry
Catalog_PreservesBountyIssuedBaselineParity
Runtime_BandCrossings_TriggerAllCanonicalReactionsOnce
Runtime_OverflowLegends_TriggerOnceOvernight
Runtime_StateCaptureAndRestore_PreservesFiredEvents
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/MoralChoiceSystemTests.cs`

### `Ashfall.Core.Tests/MoralChoiceSystemTests.cs`

- Current test declarations: Fact=22, Theory=0, InlineData=0.
- File lines: 428; SHA-256: `43c39778ac33913ad3ea4cb255b57fb54511909b6560e169bb599e20dc6ed4ec`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
InitialStateNeutralAndEmpty
BandEdgesPinned
ResolveAppliesDeltasAndRaisesEvent
ResolveIsIdempotentPerQuest
ScoreClampsAndLegendSettlesAtReconcileOncePerDirection
ResolveRejectsNonCanonicalQuestId
ResolveRejectsOutOfRangeChoice
ImpactMarksFollowDeltaSign
SameSeedSameRolls
ReconcileFiresExtremeBandEventsOnce
ReconcileFiresAllCrossedBandsOnBigJump
ReconcileFiresContractAtPositiveBand
ReconcileIgnoresOutOfOrderDays
EndingStorykeeperOverridesBand
EndingSelectionRules
StorykeeperNeedsBothThresholds
ListenerAndConfidantThresholdsPinnedAtBoundary
RestoreRejectsMismatchedSystemAndBadSchema
PendingLegendFlagsSurviveRoundTrip
SaveRoundTripPreservesLedger
CapturedStateIsDetached
AvailabilityWindow
```


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| static threshold reaction parsing | MoralChoiceFactionReactionsCatalogLoader | Core reaction DTO and lookup shape | MoralChoiceFactionReactionsData | Owner emits/reads a typed fact; no mirror state. |
| static threshold reaction parsing | MoralChoiceFactionReactionsCatalogLoader | host loading, event resolution and journal projection | Main.MoralChoice | Owner emits/reads a typed fact; no mirror state. |
| static threshold reaction parsing | MoralChoiceFactionReactionsCatalogLoader | choice resolution and flag ownership | MoralChoiceSystem | Owner emits/reads a typed fact; no mirror state. |
| static threshold reaction parsing | MoralChoiceFactionReactionsCatalogLoader | current visible feedback surface | Moral choice panel | Owner emits/reads a typed fact; no mirror state. |
| static threshold reaction parsing | MoralChoiceFactionReactionsCatalogLoader | catalog and reaction contract | MoralChoiceFactionReactionsExpansionTests | Owner emits/reads a typed fact; no mirror state. |
| Core reaction DTO and lookup shape | MoralChoiceFactionReactionsData | static threshold reaction parsing | MoralChoiceFactionReactionsCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| Core reaction DTO and lookup shape | MoralChoiceFactionReactionsData | host loading, event resolution and journal projection | Main.MoralChoice | Owner emits/reads a typed fact; no mirror state. |
| Core reaction DTO and lookup shape | MoralChoiceFactionReactionsData | choice resolution and flag ownership | MoralChoiceSystem | Owner emits/reads a typed fact; no mirror state. |
| Core reaction DTO and lookup shape | MoralChoiceFactionReactionsData | current visible feedback surface | Moral choice panel | Owner emits/reads a typed fact; no mirror state. |
| Core reaction DTO and lookup shape | MoralChoiceFactionReactionsData | catalog and reaction contract | MoralChoiceFactionReactionsExpansionTests | Owner emits/reads a typed fact; no mirror state. |
| host loading, event resolution and journal projection | Main.MoralChoice | static threshold reaction parsing | MoralChoiceFactionReactionsCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| host loading, event resolution and journal projection | Main.MoralChoice | Core reaction DTO and lookup shape | MoralChoiceFactionReactionsData | Owner emits/reads a typed fact; no mirror state. |
| host loading, event resolution and journal projection | Main.MoralChoice | choice resolution and flag ownership | MoralChoiceSystem | Owner emits/reads a typed fact; no mirror state. |
| host loading, event resolution and journal projection | Main.MoralChoice | current visible feedback surface | Moral choice panel | Owner emits/reads a typed fact; no mirror state. |
| host loading, event resolution and journal projection | Main.MoralChoice | catalog and reaction contract | MoralChoiceFactionReactionsExpansionTests | Owner emits/reads a typed fact; no mirror state. |
| choice resolution and flag ownership | MoralChoiceSystem | static threshold reaction parsing | MoralChoiceFactionReactionsCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| choice resolution and flag ownership | MoralChoiceSystem | Core reaction DTO and lookup shape | MoralChoiceFactionReactionsData | Owner emits/reads a typed fact; no mirror state. |
| choice resolution and flag ownership | MoralChoiceSystem | host loading, event resolution and journal projection | Main.MoralChoice | Owner emits/reads a typed fact; no mirror state. |
| choice resolution and flag ownership | MoralChoiceSystem | current visible feedback surface | Moral choice panel | Owner emits/reads a typed fact; no mirror state. |
| choice resolution and flag ownership | MoralChoiceSystem | catalog and reaction contract | MoralChoiceFactionReactionsExpansionTests | Owner emits/reads a typed fact; no mirror state. |
| current visible feedback surface | Moral choice panel | static threshold reaction parsing | MoralChoiceFactionReactionsCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| current visible feedback surface | Moral choice panel | Core reaction DTO and lookup shape | MoralChoiceFactionReactionsData | Owner emits/reads a typed fact; no mirror state. |
| current visible feedback surface | Moral choice panel | host loading, event resolution and journal projection | Main.MoralChoice | Owner emits/reads a typed fact; no mirror state. |
| current visible feedback surface | Moral choice panel | choice resolution and flag ownership | MoralChoiceSystem | Owner emits/reads a typed fact; no mirror state. |
| current visible feedback surface | Moral choice panel | catalog and reaction contract | MoralChoiceFactionReactionsExpansionTests | Owner emits/reads a typed fact; no mirror state. |
| catalog and reaction contract | MoralChoiceFactionReactionsExpansionTests | static threshold reaction parsing | MoralChoiceFactionReactionsCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| catalog and reaction contract | MoralChoiceFactionReactionsExpansionTests | Core reaction DTO and lookup shape | MoralChoiceFactionReactionsData | Owner emits/reads a typed fact; no mirror state. |
| catalog and reaction contract | MoralChoiceFactionReactionsExpansionTests | host loading, event resolution and journal projection | Main.MoralChoice | Owner emits/reads a typed fact; no mirror state. |
| catalog and reaction contract | MoralChoiceFactionReactionsExpansionTests | choice resolution and flag ownership | MoralChoiceSystem | Owner emits/reads a typed fact; no mirror state. |
| catalog and reaction contract | MoralChoiceFactionReactionsExpansionTests | current visible feedback surface | Moral choice panel | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the old count target with a six-row threshold/event reachability census. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Define event-ID normalization, one-shot/dedup behavior and exact journal/panel projection against the current moral-choice owner. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Audit whether every threshold is emitted by a current choice/flag event and whether all faction dialogue variants are visible. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | No new moral state, reaction save or duplicate faction dialogue registry. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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

> **DR-04 — The data catalog inventory has grown; several catalogs are absent from the v1.0 inventory. VERIFIED.**
`Assets/StreamingAssets/Data/` currently holds 342 entries. Catalogs observed live but not present in the v1.0 Part 5.4 inventory include: `dive_sites.json`, `hydroponic_crops.json`, `hydraulic_extrusion_catalog.json`, `metrology_standards_catalog.json`, `muster_camp_scenes.json`, `muster_epilogues.json`, `muster_faction_actions.json`, `muster_faction_culture.json`, `muster_witnesses.json`, `utility_actions.json`, `moral_choice_quests_branching.json`, `moral_choice_quests_distress.json`. Consequence: the duplication firewall (v1.0 Part 5) is stale in these domains; a planner could propose a "new" muster or moral-choice catalog that already exists. The ID-collision sweep in Factory Protocol step 1 must always run against the live listing, never against this document.

> The following v1.0 structures were confirmed by the audit and remain authoritative: the four-tier architecture (Tier 1 data authority in `Assets/StreamingAssets/Data/`; Tier 2 engine-free Core; Tier 3 `src/Host` + `src/UI`; Tier 4 xUnit plus the `HostCli` selftest surface); the `AGENTS.md` non-negotiable rules (Godot authoritative, Core engine-free, JSON authoritative, one authority per concern, focused verification); the narrative corpus under `Assets/StreamingAssets/Data/narrative/` (present in the live listing); the faction, economy, weather, Year-of-Ash, moral-choice, muster, and verdict catalog families (all present live); and the plan-discipline artifacts (`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `SESSION_HANDOFF.md`) at root.

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
| C4 | Fuel and feedstock income-versus-expenditure audits for each industrial chain; dominated-process analysis (do any catalogs produce strictly dominated outputs?) | HIGH CONFIDENCE |
| C5 | Scavenging E[value] re-runs after any loot authoring (Plan 76.2 harness pattern); vehicle dominance follow-ups against the live dominance table | HIGH CONFIDENCE |
| C7 | Tribute-cycle sustainability (7-day cadence) versus mid-game income; embargo economic pressure | HIGH CONFIDENCE |
| C11 | Price-shock and rumor-band systemic outcomes; debt-interest runaway analysis; black-market pricing tiers | HIGH CONFIDENCE |
| C12 | Winter resource compression (Days 90–180): calories, fuel, filters, morale — sustainability-day math per difficulty preset | HIGH CONFIDENCE |
| C14 | Trapping yield versus equipment degradation cost; zoonosis risk premium on uncooked yield | HIGH CONFIDENCE |
| All others | Balance audits only where numbers exist; never invent tuning targets without an intended design statement | — |

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

The safe subject is six authored threshold reactions and their current moral-choice host path, not a generalized dialogue framework.

- **static threshold reaction parsing** remains with `MoralChoiceFactionReactionsCatalogLoader` at `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsCatalogLoader.cs`. Static content owner.
- **Core reaction DTO and lookup shape** remains with `MoralChoiceFactionReactionsData` at `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsData.cs`. No mutable reaction state.
- **host loading, event resolution and journal projection** remains with `Main.MoralChoice` at `src/Main.MoralChoice.cs`. Thin host composition and presentation fact.
- **choice resolution and flag ownership** remains with `MoralChoiceSystem` at `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`. Owns choices, not static dialogue.
- **current visible feedback surface** remains with `Moral choice panel` at `src/UI/MoralChoiceModal.cs`. Read-only projection; verify path exists before implementation.
- **catalog and reaction contract** remains with `MoralChoiceFactionReactionsExpansionTests` at `Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs`. Focused evidence.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load six reaction records
2. normalize and validate event IDs
3. resolve a current threshold event through Main.MoralChoice
4. look up authored faction dialogue
5. project the current speaker/location/lines
6. journal or display once through the existing route
7. preserve current moral/gossip/flag state

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Threshold reaction definitions are immutable catalog rows.
- Resolved moral choices, flags, gossip and reputation remain with their current systems.
- A reaction is a fact attached to a threshold event, not a second lasting faction relationship.
- No reaction-specific save section is justified unless a current owner adopts a durable seen-state.

- Only a current emitted event may trigger a reaction.
- Unknown event IDs return no reaction without mutating any owner.
- Dialogue content is presentation; it cannot change trust, guilt or faction state.
- Same flags, event and day produce the same reaction selection.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Main.MoralChoice.cs
- src/UI/MoralChoiceModal.cs
- src/Main.Application.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs
- Ashfall.Core.Tests/MoralChoiceSystemTests.cs

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
| S-01 | 100-01 The safe subject is six authored threshold reactions and their current moral-choice host path, not a generalized dialogue framework. | load six reaction records | Threshold reaction definitions are immutable catalog rows. | A reaction is displayed for an event the current system never emitted. | MoralChoiceFactionReactionsCatalogLoader |
| S-02 | 100-02 current owner boundary | normalize and validate event IDs | Resolved moral choices, flags, gossip and reputation remain with their current systems. | A journal entry repeats on every panel refresh. | MoralChoiceFactionReactionsCatalogLoader |
| S-03 | 100-03 missing reference refusal | resolve a current threshold event through Main.MoralChoice | A reaction is a fact attached to a threshold event, not a second lasting faction relationship. | Reaction text mutates faction trust. | MoralChoiceFactionReactionsCatalogLoader |
| S-04 | 100-04 save continuation | look up authored faction dialogue | No reaction-specific save section is justified unless a current owner adopts a durable seen-state. | A missing key crashes a save restore. | MoralChoiceFactionReactionsCatalogLoader |
| S-05 | 100-05 same-seed replay | project the current speaker/location/lines | Threshold reaction definitions are immutable catalog rows. | A new panel caches the entire moral state. | MoralChoiceFactionReactionsCatalogLoader |
| S-06 | 100-06 UI truthfulness | journal or display once through the existing route | Resolved moral choices, flags, gossip and reputation remain with their current systems. | A reaction is displayed for an event the current system never emitted. | MoralChoiceFactionReactionsCatalogLoader |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 100-TC-01 schema and count | data | schema and count; verify the named current owner and its negative boundary without inventing a second authority. | MoralChoiceFactionReactionsCatalogLoader |
| T-02 | 100-TC-02 reference validation | unit | reference validation; verify the named current owner and its negative boundary without inventing a second authority. | MoralChoiceFactionReactionsCatalogLoader |
| T-03 | 100-TC-03 current owner boundary | persistence | current owner boundary; verify the named current owner and its negative boundary without inventing a second authority. | MoralChoiceFactionReactionsCatalogLoader |
| T-04 | 100-TC-04 failure refusal | determinism | failure refusal; verify the named current owner and its negative boundary without inventing a second authority. | MoralChoiceFactionReactionsCatalogLoader |
| T-05 | 100-TC-05 save continuation | host | save continuation; verify the named current owner and its negative boundary without inventing a second authority. | MoralChoiceFactionReactionsCatalogLoader |
| T-06 | 100-TC-06 deterministic replay | UI/accessibility | deterministic replay; verify the named current owner and its negative boundary without inventing a second authority. | MoralChoiceFactionReactionsCatalogLoader |
| T-07 | 100-TC-07 host/UI truth | cross-system | host/UI truth; verify the named current owner and its negative boundary without inventing a second authority. | MoralChoiceFactionReactionsCatalogLoader |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 38 | `Ashfall.Core.Tests/MoralChoiceSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 31 | `Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 22 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 18 | `Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `Ashfall.Core.Tests/PrpfStandingSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `Ashfall.Core.Tests/World/Plan100_48MoralReactionsWeatherGatesIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsCatalogLoader.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `src/Main.MoralChoice.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/IndependentBranchSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/Journeys/MoralChoiceJourneyTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Assets/Ashfall.Core/Factions/MilitaryBranchSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/MoralChoiceCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/MoralChoice/Plan15_18MoralCodexIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Factions/RebelBranchSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/MilitaryBranchSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/MoralChoice/MoralChoiceDailyOfferTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/MoralChoice/Plan144MoralChoiceStubClosureTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/MoralChoiceFlagPlan125Tests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/RebelBranchSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/WorldFlagConsumerIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Factions/PrpfStandingSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Host/HostCli.MoralChoice.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/UI/QuestsPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Campaign/CampaignConsequenceLedgerTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/CampaignContinuityFlagshipTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/FactionBranchCoordinatorTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Factions/Plan121_122FactionBranchIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/MilitaryBranchExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Radio/DistressSignalCampaignSmokeTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Radio/DistressSignalFactionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Radio/DistressSignalMoralChoiceTests.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/moral_choice_faction_reactions.json`

### `Assets/StreamingAssets/Data/moral_choice_faction_reactions.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 14988; characters: 14950.
- SHA-256: `fd9f4c31b6dd1c9779bc820d7b02923901257b704f930c3572e6d2d57933950d`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `description`, `threshold_reactions`

#### `threshold_reactions` — 6 keyed entries

- Entry 001 `moral_event_bounty_issued`: `{"event_description":"Fires when the player enters VeryEvil band (-100 or below). Peacekeepers issue a bounty.","journal_entry":"The Peacekeepers have issued a bounty with my face on it. The sergeant put it up himself. Every patrol knows m…`
- Entry 002 `moral_event_contract_taken`: `{"event_description":"Fires when the player enters Positive band (+50 to +99). Peacekeepers offer a standing contract — protection for cooperation.","journal_entry":"The Peacekeepers offered me a contract — not employment, a partnership. T…`
- Entry 003 `moral_event_contract_raised`: `{"event_description":"Fires when the player enters VeryPositive band (+100 or above). The contract deepens — the player becomes a recognized community figure.","journal_entry":"The Peacekeepers raised my contract to full support — logistic…`
- Entry 004 `moral_event_patrol_defense`: `{"event_description":"Fires alongside contract_raised at VeryPositive. Patrols actively defend the player's shelter.","journal_entry":"A Peacekeeper patrol has taken position at my shelter perimeter. Captain's orders. They've got the east …`
- Entry 005 `moral_event_legend_positive`: `{"civilian_dialogue":[{"lines":["I tell the children about you. Not as a person — as a lesson.","'Be like that one,' I say. 'Give when it costs. Listen when it hurts. Stay when it's easier to go.'","You don't have to be perfect. You just h…`
- Entry 006 `moral_event_legend_negative`: `{"civilian_dialogue":[{"lines":["Shh. Be quiet now.","See that one? The one walking past? Don't look. Don't look at them.","If they ever come to our door, you hide. You hide and you don't make a sound.","That's what happens when someone st…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/moral_choice_flags.json`

### `Assets/StreamingAssets/Data/moral_choice_flags.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 2090; characters: 2090.
- SHA-256: `e5a95235ce28f9d2a1c9c8bcaeb72789122d9d77e1d38b0658cc86ae4d6a4db4`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `description`, `flags`

#### `flags` — 25 current rows

- Row 001 `flag_betrayed_ally`: `{"display_name":"Betrayed an Ally","id":"flag_betrayed_ally"}`
- Row 002 `flag_betrayed_faction`: `{"display_name":"Betrayed a Faction","id":"flag_betrayed_faction"}`
- Row 003 `flag_betrayed_trust`: `{"display_name":"Broke Trust","id":"flag_betrayed_trust"}`
- Row 004 `flag_broken_pact`: `{"display_name":"Broken Pact","id":"flag_broken_pact"}`
- Row 005 `flag_become_warlord`: `{"display_name":"Became Warlord","id":"flag_become_warlord"}`
- Row 006 `flag_throne_of_ash`: `{"display_name":"Throne of Ash","id":"flag_throne_of_ash"}`
- Row 007 `flag_branch_mercy_road_locked`: `{"display_name":"Mercy Road Locked","id":"flag_branch_mercy_road_locked"}`
- Row 008 `flag_branch_iron_way_locked`: `{"display_name":"Iron Way Locked","id":"flag_branch_iron_way_locked"}`
- Row 009 `flag_branch_listener_locked`: `{"display_name":"Listener's Thread Locked","id":"flag_branch_listener_locked"}`
- Row 010 `flag_branch_broken_compact_locked`: `{"display_name":"Broken Compact Locked","id":"flag_branch_broken_compact_locked"}`
- Row 011 `flag_spared_raider`: `{"display_name":"Spared a Raider","id":"flag_spared_raider"}`
- Row 012 `flag_executed_prisoner`: `{"display_name":"Executed a Prisoner","id":"flag_executed_prisoner"}`
- Row 013 `flag_shared_rations`: `{"display_name":"Shared Rations","id":"flag_shared_rations"}`
- Row 014 `flag_hoarded_medicine`: `{"display_name":"Hoarded Medicine","id":"flag_hoarded_medicine"}`
- Row 015 `flag_sheltered_refugee`: `{"display_name":"Sheltered a Refugee","id":"flag_sheltered_refugee"}`
- Row 016 `flag_expelled_survivor`: `{"display_name":"Expelled a Survivor","id":"flag_expelled_survivor"}`
- Row 017 `flag_repaired_infrastructure`: `{"display_name":"Repaired Shared Infrastructure","id":"flag_repaired_infrastructure"}`
- Row 018 `flag_sabotaged_rival`: `{"display_name":"Sabotaged a Rival","id":"flag_sabotaged_rival"}`
- Row 019 `flag_broke_treaty`: `{"display_name":"Broke a Treaty","id":"flag_broke_treaty"}`
- Row 020 `flag_honored_debt`: `{"display_name":"Honored a Debt","id":"flag_honored_debt"}`
- Row 021 `flag_ignored_distress`: `{"display_name":"Ignored a Distress Call","id":"flag_ignored_distress"}`
- Row 022 `flag_responded_distress`: `{"display_name":"Responded to Distress","id":"flag_responded_distress"}`
- Row 023 `flag_forged_record`: `{"display_name":"Forged a Record","id":"flag_forged_record"}`
- Row 024 `flag_preserved_archive`: `{"display_name":"Preserved an Archive","id":"flag_preserved_archive"}`
- Row 025 `flag_chosen_faction_side`: `{"display_name":"Chose a Faction Side","id":"flag_chosen_faction_side"}`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/moral_choice_chains.json`

### `Assets/StreamingAssets/Data/moral_choice_chains.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 31788; characters: 31784.
- SHA-256: `2a30c42686baef9f1e64cdda74fdb13cbe9204c641790d47212e5aeb48fc2135`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `description`, `branches`, `merge_rules`, `lockout_rules`, `quest_gates`, `echo_quests`, `gossip_propagation`, `faction_reactions`

#### `branches` — 4 current rows

- Row 001 `branch_mercy_road`: `{"description":"Compassion as a permanent stance. Helping becomes habit, then identity, then burden. Unlocks cooperative storylines; permanently closes the Iron Way and Broken Compact after the third mercy quest.","display_name":"The Mercy…`
- Row 002 `branch_iron_way`: `{"description":"Pragmatism hardens into ruthlessness. Survival at the cost of others becomes a doctrine. Permanently closes the Mercy Road and Listener's Thread after the third iron quest.","display_name":"The Iron Way","entry_quests":["qu…`
- Row 003 `branch_listener_thread`: `{"description":"Understanding over action. The player accumulates stories and wisdom instead of taking sides. Permanently closes the Iron Way and Broken Compact after the third listener quest.","display_name":"The Listener's Thread","entry…`
- Row 004 `branch_broken_compact`: `{"description":"Betrayal as a survival strategy. Trust becomes a weapon. Permanently closes the Mercy Road and Listener's Thread after the third betrayal quest.","display_name":"The Broken Compact","entry_quests":["quest_moral_chain_betray…`

#### `merge_rules` — 4 keyed entries

- Entry 001 `description`: `"A player on Branch A can access merge quests from Branch B only if merge_allowed includes B. Merge quests are marked with 'merge_from' in their prerequisites. Merging does NOT unlock the merged branch's exclusive content — only the shared…`
- Entry 002 `merge_quest_prefix`: `"quest_moral_merge_"`
- Entry 003 `merge_quests_require_min_progress`: `5`
- Entry 004 `merge_never_unlocks_exclusive`: `true`

#### `lockout_rules` — 4 keyed entries

- Entry 001 `description`: `"When a branch locks, all quests belonging to the locked-out branches become permanently inaccessible. The player can never return to those storylines. This creates meaningful permanent consequences."`
- Entry 002 `lockout_is_permanent`: `true`
- Entry 003 `lockout_fires_journal_entry`: `true`
- Entry 004 `lockout_journal_template`: `"A door has closed. The path of {locked_branch_name} is no longer open to you."`

#### `quest_gates` — 88 current rows

- Row 001 `quest_moral_chain_mercy_04`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_04","requires":["quest_moral_chain_mercy_03"],"requires_choice_index":null,"requires_min_moral":15}`
- Row 002 `quest_moral_chain_mercy_05`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_05","requires":["quest_moral_chain_mercy_04"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 003 `quest_moral_chain_mercy_06`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_06","requires":["quest_moral_chain_mercy_05"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 004 `quest_moral_chain_mercy_07`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_07","requires":["quest_moral_chain_mercy_06"],"requires_choice_index":null,"requires_min_moral":30}`
- Row 005 `quest_moral_chain_mercy_08`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_08","requires":["quest_moral_chain_mercy_07"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 006 `quest_moral_chain_mercy_09`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_09","requires":["quest_moral_chain_mercy_08"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 007 `quest_moral_chain_mercy_10`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_10","requires":["quest_moral_chain_mercy_09"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 008 `quest_moral_chain_mercy_11`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_11","requires":["quest_moral_chain_mercy_10"],"requires_choice_index":null,"requires_min_moral":50}`
- Row 009 `quest_moral_chain_mercy_12`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_12","requires":["quest_moral_chain_mercy_11"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 010 `quest_moral_chain_mercy_13`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_13","requires":["quest_moral_chain_mercy_12"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 011 `quest_moral_chain_mercy_14`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_14","requires":["quest_moral_chain_mercy_13"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 012 `quest_moral_chain_mercy_15`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_15","requires":["quest_moral_chain_mercy_14"],"requires_choice_index":null,"requires_min_moral":75}`
- Row 013 `quest_moral_chain_mercy_16`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_16","requires":["quest_moral_chain_mercy_15"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 014 `quest_moral_chain_mercy_17`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_17","requires":["quest_moral_chain_mercy_16"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 015 `quest_moral_chain_mercy_18`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_18","requires":["quest_moral_chain_mercy_17"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 016 `quest_moral_chain_mercy_19`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_19","requires":["quest_moral_chain_mercy_18"],"requires_choice_index":null,"requires_min_moral":90}`
- Row 017 `quest_moral_chain_mercy_20`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_20","requires":["quest_moral_chain_mercy_19"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 018 `quest_moral_chain_mercy_21`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_21","requires":["quest_moral_chain_mercy_20"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 019 `quest_moral_chain_mercy_22`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_22","requires":["quest_moral_chain_mercy_21"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 020 `quest_moral_chain_mercy_23`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_23","requires":["quest_moral_chain_mercy_22"],"requires_choice_index":null,"requires_min_moral":100}`
- Row 021 `quest_moral_chain_mercy_24`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_24","requires":["quest_moral_chain_mercy_23"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 022 `quest_moral_chain_mercy_25`: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_25","requires":["quest_moral_chain_mercy_24"],"requires_choice_index":null,"requires_min_moral":null}`
- Row 023 `quest_moral_chain_iron_04`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_04","requires":["quest_moral_chain_iron_03"],"requires_choice_index":null,"requires_max_moral":-15}`
- Row 024 `quest_moral_chain_iron_05`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_05","requires":["quest_moral_chain_iron_04"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 025 `quest_moral_chain_iron_06`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_06","requires":["quest_moral_chain_iron_05"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 026 `quest_moral_chain_iron_07`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_07","requires":["quest_moral_chain_iron_06"],"requires_choice_index":null,"requires_max_moral":-30}`
- Row 027 `quest_moral_chain_iron_08`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_08","requires":["quest_moral_chain_iron_07"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 028 `quest_moral_chain_iron_09`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_09","requires":["quest_moral_chain_iron_08"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 029 `quest_moral_chain_iron_10`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_10","requires":["quest_moral_chain_iron_09"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 030 `quest_moral_chain_iron_11`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_11","requires":["quest_moral_chain_iron_10"],"requires_choice_index":null,"requires_max_moral":-50}`
- Row 031 `quest_moral_chain_iron_12`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_12","requires":["quest_moral_chain_iron_11"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 032 `quest_moral_chain_iron_13`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_13","requires":["quest_moral_chain_iron_12"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 033 `quest_moral_chain_iron_14`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_14","requires":["quest_moral_chain_iron_13"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 034 `quest_moral_chain_iron_15`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_15","requires":["quest_moral_chain_iron_14"],"requires_choice_index":null,"requires_max_moral":-75}`
- Row 035 `quest_moral_chain_iron_16`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_16","requires":["quest_moral_chain_iron_15"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 036 `quest_moral_chain_iron_17`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_17","requires":["quest_moral_chain_iron_16"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 037 `quest_moral_chain_iron_18`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_18","requires":["quest_moral_chain_iron_17"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 038 `quest_moral_chain_iron_19`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_19","requires":["quest_moral_chain_iron_18"],"requires_choice_index":null,"requires_max_moral":-90}`
- Row 039 `quest_moral_chain_iron_20`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_20","requires":["quest_moral_chain_iron_19"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 040 `quest_moral_chain_iron_21`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_21","requires":["quest_moral_chain_iron_20"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 041 `quest_moral_chain_iron_22`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_22","requires":["quest_moral_chain_iron_21"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 042 `quest_moral_chain_iron_23`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_23","requires":["quest_moral_chain_iron_22"],"requires_choice_index":null,"requires_max_moral":-100}`
- Row 043 `quest_moral_chain_iron_24`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_24","requires":["quest_moral_chain_iron_23"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 044 `quest_moral_chain_iron_25`: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_25","requires":["quest_moral_chain_iron_24"],"requires_choice_index":null,"requires_max_moral":null}`
- Row 045 `quest_moral_chain_listen_04`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_04","requires":["quest_moral_chain_listen_03"],"requires_choice_index":null,"requires_min_empathy":8}`
- Row 046 `quest_moral_chain_listen_05`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_05","requires":["quest_moral_chain_listen_04"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 047 `quest_moral_chain_listen_06`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_06","requires":["quest_moral_chain_listen_05"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 048 `quest_moral_chain_listen_07`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_07","requires":["quest_moral_chain_listen_06"],"requires_choice_index":null,"requires_min_empathy":15}`
- Row 049 `quest_moral_chain_listen_08`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_08","requires":["quest_moral_chain_listen_07"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 050 `quest_moral_chain_listen_09`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_09","requires":["quest_moral_chain_listen_08"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 051 `quest_moral_chain_listen_10`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_10","requires":["quest_moral_chain_listen_09"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 052 `quest_moral_chain_listen_11`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_11","requires":["quest_moral_chain_listen_10"],"requires_choice_index":null,"requires_min_empathy":22}`
- Row 053 `quest_moral_chain_listen_12`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_12","requires":["quest_moral_chain_listen_11"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 054 `quest_moral_chain_listen_13`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_13","requires":["quest_moral_chain_listen_12"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 055 `quest_moral_chain_listen_14`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_14","requires":["quest_moral_chain_listen_13"],"requires_choice_index":null,"requires_min_empathy":30}`
- Row 056 `quest_moral_chain_listen_15`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_15","requires":["quest_moral_chain_listen_14"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 057 `quest_moral_chain_listen_16`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_16","requires":["quest_moral_chain_listen_15"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 058 `quest_moral_chain_listen_17`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_17","requires":["quest_moral_chain_listen_16"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 059 `quest_moral_chain_listen_18`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_18","requires":["quest_moral_chain_listen_17"],"requires_choice_index":null,"requires_min_empathy":38}`
- Row 060 `quest_moral_chain_listen_19`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_19","requires":["quest_moral_chain_listen_18"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 061 `quest_moral_chain_listen_20`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_20","requires":["quest_moral_chain_listen_19"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 062 `quest_moral_chain_listen_21`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_21","requires":["quest_moral_chain_listen_20"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 063 `quest_moral_chain_listen_22`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_22","requires":["quest_moral_chain_listen_21"],"requires_choice_index":null,"requires_min_empathy":45}`
- Row 064 `quest_moral_chain_listen_23`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_23","requires":["quest_moral_chain_listen_22"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 065 `quest_moral_chain_listen_24`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_24","requires":["quest_moral_chain_listen_23"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 066 `quest_moral_chain_listen_25`: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_25","requires":["quest_moral_chain_listen_24"],"requires_choice_index":null,"requires_min_empathy":null}`
- Row 067 `quest_moral_chain_betray_04`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_04","requires":["quest_moral_chain_betray_03"],"requires_choice_index":null,"requires_flag":"flag_betrayed_trust"}`
- Row 068 `quest_moral_chain_betray_05`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_05","requires":["quest_moral_chain_betray_04"],"requires_choice_index":null,"requires_flag":null}`
- Row 069 `quest_moral_chain_betray_06`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_06","requires":["quest_moral_chain_betray_05"],"requires_choice_index":null,"requires_flag":null}`
- Row 070 `quest_moral_chain_betray_07`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_07","requires":["quest_moral_chain_betray_06"],"requires_choice_index":null,"requires_flag":"flag_betrayed_ally"}`
- Row 071 `quest_moral_chain_betray_08`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_08","requires":["quest_moral_chain_betray_07"],"requires_choice_index":null,"requires_flag":null}`
- Row 072 `quest_moral_chain_betray_09`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_09","requires":["quest_moral_chain_betray_08"],"requires_choice_index":null,"requires_flag":null}`
- Row 073 `quest_moral_chain_betray_10`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_10","requires":["quest_moral_chain_betray_09"],"requires_choice_index":null,"requires_flag":null}`
- Row 074 `quest_moral_chain_betray_11`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_11","requires":["quest_moral_chain_betray_10"],"requires_choice_index":null,"requires_flag":"flag_betrayed_faction"}`
- Row 075 `quest_moral_chain_betray_12`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_12","requires":["quest_moral_chain_betray_11"],"requires_choice_index":null,"requires_flag":null}`
- Row 076 `quest_moral_chain_betray_13`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_13","requires":["quest_moral_chain_betray_12"],"requires_choice_index":null,"requires_flag":null}`
- Row 077 `quest_moral_chain_betray_14`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_14","requires":["quest_moral_chain_betray_13"],"requires_choice_index":null,"requires_flag":null}`
- Row 078 `quest_moral_chain_betray_15`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_15","requires":["quest_moral_chain_betray_14"],"requires_choice_index":null,"requires_flag":"flag_broken_pact"}`
- Row 079 `quest_moral_chain_betray_16`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_16","requires":["quest_moral_chain_betray_15"],"requires_choice_index":null,"requires_flag":null}`
- Row 080 `quest_moral_chain_betray_17`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_17","requires":["quest_moral_chain_betray_16"],"requires_choice_index":null,"requires_flag":null}`
- Row 081 `quest_moral_chain_betray_18`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_18","requires":["quest_moral_chain_betray_17"],"requires_choice_index":null,"requires_flag":null}`
- Row 082 `quest_moral_chain_betray_19`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_19","requires":["quest_moral_chain_betray_18"],"requires_choice_index":null,"requires_flag":"flag_become_warlord"}`
- Row 083 `quest_moral_chain_betray_20`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_20","requires":["quest_moral_chain_betray_19"],"requires_choice_index":null,"requires_flag":null}`
- Row 084 `quest_moral_chain_betray_21`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_21","requires":["quest_moral_chain_betray_20"],"requires_choice_index":null,"requires_flag":null}`
- Row 085 `quest_moral_chain_betray_22`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_22","requires":["quest_moral_chain_betray_21"],"requires_choice_index":null,"requires_flag":null}`
- Row 086 `quest_moral_chain_betray_23`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_23","requires":["quest_moral_chain_betray_22"],"requires_choice_index":null,"requires_flag":"flag_throne_of_ash"}`
- Row 087 `quest_moral_chain_betray_24`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_24","requires":["quest_moral_chain_betray_23"],"requires_choice_index":null,"requires_flag":null}`
- Row 088 `quest_moral_chain_betray_25`: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_25","requires":["quest_moral_chain_betray_24"],"requires_choice_index":null,"requires_flag":null}`

#### `echo_quests` — 2 keyed entries

- Entry 001 `description`: `"Echo quests fire when a specific earlier quest was resolved a certain way. They reference the prior choice and present consequences or callbacks. Accessible regardless of branch unless their branch field restricts them."`
- Entry 002 `quests`: `[{"branch":null,"min_days_after":30,"quest_id":"quest_moral_echo_child_returns","triggered_by":"quest_moral_share_child","triggered_by_choice":0},{"branch":null,"min_days_after":20,"quest_id":"quest_moral_echo_child_steals","triggered_by":…`

#### `gossip_propagation` — 2 keyed entries

- Entry 001 `description`: `"After propagatesOnDay fires, gossip text plays through camp chatter, NPC greeting changes, and faction stance shifts. Each band has a tone and set of templates."`
- Entry 002 `gossip_file`: `"moral_choice_gossip.json"`

#### `faction_reactions` — 2 keyed entries

- Entry 001 `description`: `"Threshold event dialogues — what faction NPCs say when band-crossing events fire."`
- Entry 002 `reactions_file`: `"moral_choice_faction_reactions.json"`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsCatalogLoader.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsCatalogLoader.cs` — complete current file

- Size: 95 lines / 4036 bytes.
- SHA-256: `f99e21f5283583177721051f5cde3736ac899f01939f033e1dad9b9d03b3877e`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005:
00006: namespace Ashfall.Core.MoralChoice
00007: {
00008:     // ── JSON wire records ──
00009:
00010:     [Serializable]
00011:     public sealed class MoralChoiceFactionReactionsContainer
00012:     {
00013:         public int schema_version = 1;
00014:         public string description = string.Empty;
00015:         public Dictionary<string, MoralThresholdReactionRecord> threshold_reactions
00016:             = new Dictionary<string, MoralThresholdReactionRecord>();
00017:     }
00018:
00019:     [Serializable]
00020:     public sealed class MoralThresholdReactionRecord
00021:     {
00022:         public string event_description = string.Empty;
00023:         public List<MoralFactionDialogueRecord> peacekeeper_dialogue = new List<MoralFactionDialogueRecord>();
00024:         public List<MoralFactionDialogueRecord> raider_dialogue = new List<MoralFactionDialogueRecord>();
00025:         public List<MoralFactionDialogueRecord> knowledge_keeper_dialogue = new List<MoralFactionDialogueRecord>();
00026:         public List<MoralFactionDialogueRecord> civilian_dialogue = new List<MoralFactionDialogueRecord>();
00027:         public string journal_entry = string.Empty;
00028:     }
00029:
00030:     [Serializable]
00031:     public sealed class MoralFactionDialogueRecord
00032:     {
00033:         public string speaker = string.Empty;
00034:         public string location = string.Empty;
00035:         public List<string> lines = new List<string>();
00036:     }
00037:
00038:     /// <summary>
00039:     /// Loads moral_choice_faction_reactions.json — faction NPC dialogues for
00040:     /// each moral threshold event. Engine-agnostic.
00041:     /// </summary>
00042:     public static class MoralChoiceFactionReactionsCatalogLoader
00043:     {
00044:         public const string DefaultFileName = "moral_choice_faction_reactions.json";
00045:
00046:         public static MoralChoiceFactionReactionsData Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
00047:         {
00048:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00049:                 return new MoralChoiceFactionReactionsData();
00050:
00051:             string path = fileIO.Combine(dataDir, DefaultFileName);
00052:             if (!fileIO.FileExists(path))
00053:                 return new MoralChoiceFactionReactionsData();
00054:
00055:             string raw = fileIO.ReadAllText(path);
00056:             if (string.IsNullOrWhiteSpace(raw))
00057:                 return new MoralChoiceFactionReactionsData();
00058:
00059:             var container = json.Deserialize<MoralChoiceFactionReactionsContainer>(raw);
00060:             if (container?.threshold_reactions == null)
00061:                 return new MoralChoiceFactionReactionsData();
00062:
00063:             var data = new MoralChoiceFactionReactionsData();
00064:             foreach (var kvp in container.threshold_reactions)
00065:             {
00066:                 if (kvp.Value == null) continue;
00067:                 data.ThresholdReactions[kvp.Key] = MapReaction(kvp.Value);
00068:             }
00069:             return data;
00070:         }
00071:
00072:         private static MoralThresholdReaction MapReaction(MoralThresholdReactionRecord r) =>
00073:             new MoralThresholdReaction
00074:             {
00075:                 EventDescription = r.event_description ?? string.Empty,
00076:                 PeacekeeperDialogue = r.peacekeeper_dialogue?.Select(MapDialogue).ToList()
00077:                     ?? new List<MoralFactionDialogue>(),
00078:                 RaiderDialogue = r.raider_dialogue?.Select(MapDialogue).ToList()
00079:                     ?? new List<MoralFactionDialogue>(),
00080:                 KnowledgeKeeperDialogue = r.knowledge_keeper_dialogue?.Select(MapDialogue).ToList()
00081:                     ?? new List<MoralFactionDialogue>(),
00082:                 CivilianDialogue = r.civilian_dialogue?.Select(MapDialogue).ToList()
00083:                     ?? new List<MoralFactionDialogue>(),
00084:                 JournalEntry = r.journal_entry ?? string.Empty
00085:             };
00086:
00087:         private static MoralFactionDialogue MapDialogue(MoralFactionDialogueRecord d) =>
00088:             new MoralFactionDialogue
00089:             {
00090:                 Speaker = d.speaker ?? string.Empty,
00091:                 Location = d.location ?? string.Empty,
00092:                 Lines = d.lines ?? new List<string>()
00093:             };
00094:     }
00095: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsData.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceFactionReactionsData.cs` — complete current file

- Size: 33 lines / 1479 bytes.
- SHA-256: `a78b6348a5c75bf28cd49264bd249fd92ec07a0c3f32f75c4c90855247ff8a13`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003:
00004: namespace Ashfall.Core.MoralChoice
00005: {
00006:     /// <summary>
00007:     /// Wire shape for moral_choice_faction_reactions.json — faction NPC
00008:     /// dialogues triggered by moral threshold events. Each event fires once
00009:     /// per save when the player crosses a moral band boundary overnight.
00010:     /// </summary>
00011:     public sealed class MoralChoiceFactionReactionsData
00012:     {
00013:         public Dictionary<string, MoralThresholdReaction> ThresholdReactions { get; set; }
00014:             = new Dictionary<string, MoralThresholdReaction>();
00015:     }
00016:
00017:     public sealed class MoralThresholdReaction
00018:     {
00019:         public string EventDescription { get; set; } = string.Empty;
00020:         public List<MoralFactionDialogue> PeacekeeperDialogue { get; set; } = new List<MoralFactionDialogue>();
00021:         public List<MoralFactionDialogue> RaiderDialogue { get; set; } = new List<MoralFactionDialogue>();
00022:         public List<MoralFactionDialogue> KnowledgeKeeperDialogue { get; set; } = new List<MoralFactionDialogue>();
00023:         public List<MoralFactionDialogue> CivilianDialogue { get; set; } = new List<MoralFactionDialogue>();
00024:         public string JournalEntry { get; set; } = string.Empty;
00025:     }
00026:
00027:     public sealed class MoralFactionDialogue
00028:     {
00029:         public string Speaker { get; set; } = string.Empty;
00030:         public string Location { get; set; } = string.Empty;
00031:         public List<string> Lines { get; set; } = new List<string>();
00032:     }
00033: }
```


# Appendix — Current Source Detail: `src/Main.MoralChoice.cs`

### `src/Main.MoralChoice.cs` — complete current file

- Size: 344 lines / 16147 bytes.
- SHA-256: `e12534c6d2a8cd86c90d7283de173c51bf07cd68e46bb52f6129bbeb05375b8d`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using Godot;
00003: using System;
00004: using System.Collections.Generic;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.MoralChoice;
00008:
00009: namespace AtomicWar.GodotApp
00010: {
00011:     public partial class Main : Control
00012:     {
00013:         // ── Moral choice ("The Weight of Survival") host wiring ──
00014:         // The score is invisible by design: hosts read CurrentBand and the
00015:         // threshold events, never the raw number.
00016:         private MoralChoiceSystem _moralChoice = null!;
00017:         private List<MoralChoiceQuestDefinition> _moralChoiceDefs = new List<MoralChoiceQuestDefinition>();
00018:         private bool _moralChoiceDirty;
00019:
00020:         // ── Branching / gossip / faction reactions (Phase 2 data) ──
00021:         private MoralChoiceChainData _moralChainData = new MoralChoiceChainData();
00022:         private MoralChoiceGossipData _moralGossipData = new MoralChoiceGossipData();
00023:         private MoralChoiceFactionReactionsData _moralFactionReactions = new MoralChoiceFactionReactionsData();
00024:         private MoralChoiceFlagDefinitions _moralFlagDefs = new MoralChoiceFlagDefinitions();
00025:         private MoralChoiceGossipRuntime _moralGossipRuntime = null!;
00026:
00027:         /// <summary>
00028:         /// Fixed world seed so every host agrees on unseeded rolls; per-save
00029:         /// outcome rolls and propagation days are stored in the ledger DTO.
00030:         /// </summary>
00031:         private const int MoralChoiceSeed = 20260825;
00032:
00033:         private void SetupMoralChoice()
00034:         {
00035:             if (_moralChoice != null) return;
00036:             SetupJournal();
00037:             SetupCampaignDay();
00038:             var fileIO = new FileSystemIO();
00039:             var json = new SystemTextJsonSerializer();
00040:
00041:             _moralChoice = new MoralChoiceSystem(_campaignDay.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.MoralChoice).Rng, flags: _consequenceLedger);
00042:             _moralChoiceDefs = MoralChoiceCatalogLoader.Load(_dataDir, fileIO, json);
00043:
00044:             // Load branching chain quests and merge into the catalog
00045:             var chainQuests = MoralChoiceBranchQuestCatalogLoader.Load(_dataDir, fileIO, json);
00046:             _moralChoiceDefs.AddRange(chainQuests);
00047:
00048:             // Load expansion quests and merge into the catalog
00049:             var expansionQuests = MoralChoiceExpansionQuestCatalogLoader.Load(_dataDir, fileIO, json);
00050:             _moralChoiceDefs.AddRange(expansionQuests);
00051:
00052:             // Register all definitions into the Core system's authoritative catalog
00053:             _moralChoice.RegisterQuests(_moralChoiceDefs);
00054:
00055:             // Load chain architecture (branches, gates, echo quests)
00056:             _moralChainData = MoralChoiceChainCatalogLoader.Load(_dataDir, fileIO, json);
00057:             _moralChoice.InitializeChainData(_moralChainData);
00058:
00059:             // Load gossip, faction reactions, and flag definitions
00060:             _moralGossipData = MoralChoiceGossipCatalogLoader.Load(_dataDir, fileIO, json);
00061:             _moralFactionReactions = MoralChoiceFactionReactionsCatalogLoader.Load(_dataDir, fileIO, json);
00062:             _moralFlagDefs = MoralChoiceFlagCatalogLoader.Load(_dataDir, fileIO, json);
00063:             _moralGossipRuntime = new MoralChoiceGossipRuntime(_moralGossipData, _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.MoralChoice, 0, 1));
00064:
00065:             _moralChoice.OnQuestResolved += WriteMoralChoiceJournalEntry;
00066:             _moralChoice.OnQuestResolved += _ => _moralChoiceDirty = true;
00067:             // Plan IV Task 5: a resolved trapping dilemma acks its pending
00068:             // outbox fact so the outbox never re-surfaces it after restore.
00069:             _moralChoice.OnQuestResolved += resolution =>
00070:             {
00071:                 if (resolution.questId != null && resolution.questId.StartsWith(TrappingMoralQuestIdPrefix, StringComparison.Ordinal))
00072:                     MarkTrappingMoralEventDelivered(resolution.questId);
00073:             };
00074:             _moralChoice.OnThresholdEventFired += WriteThresholdEventJournalEntry;
00075:             _moralChoice.OnThresholdEventFired += _ => _moralChoiceDirty = true;
00076:             _moralChoice.OnBranchLocked += WriteBranchLockoutJournalEntry;
00077:             _moralChoice.OnBranchLocked += _ => _moralChoiceDirty = true;
00078:
00079:             var save = MoralChoiceSaveStore.TryLoad();
00080:             if (save != null)
00081:             {
00082:                 try
00083:                 {
00084:                     _moralChoice.RestoreState(save);
00085:                     GD.Print($"[Ashfall Godot] Moral choice ledger restored " +
00086:                              $"(day {save.lastReconciledDay}, {_moralChoice.QuestsResolved} resolved).");
00087:                 }
00088:                 catch (Exception e)
00089:                 {
00090:                     GD.PrintErr($"[Ashfall Godot] Moral choice restore rejected: {e.Message}");
00091:                 }
00092:             }
00093:             GD.Print($"[Ashfall Godot] Moral choice ready. {_moralChoiceDefs.Count} quests " +
00094:                      $"({_moralChainData.Branches.Count} branches, " +
00095:                      $"{_moralGossipData.CampChatter.Neutral.Count} neutral chatter lines).");
00096:         }
00097:
00098:         public MoralChoiceSystem MoralChoice => _moralChoice;
00099:         public IReadOnlyList<MoralChoiceQuestDefinition> MoralChoiceDefs => _moralChoiceDefs;
00100:
00101:         public MoralChoiceQuestDefinition? GetMoralChoiceDef(string questId)
00102:         {
00103:             SetupMoralChoice();
00104:             return _moralChoiceDefs.FirstOrDefault(
00105:                 d => string.Equals(d.Id, questId, StringComparison.Ordinal));
00106:         }
00107:
00108:         public List<MoralChoiceQuestDefinition> GetAvailableMoralChoices()
00109:         {
00110:             SetupMoralChoice();
00111:             var list = new List<MoralChoiceQuestDefinition>();
00112:             foreach (var d in _moralChoiceDefs)
00113:             {
00114:                 // Plan IV Task 5: trapping-sourced dilemmas are excluded from
00115:                 // the standing offer pass — they surface only while their
00116:                 // moral-consequence fact is pending in the trapping outbox.
00117:                 if (d.Id != null && d.Id.StartsWith(TrappingMoralQuestIdPrefix, StringComparison.Ordinal))
00118:                     continue;
00119:                 if (!_moralChoice.IsResolved(d.Id) &&
00120:                     MoralChoiceSystem.IsAvailableOnDay(d, _simDay) &&
00121:                     _moralChoice.IsChainQuestAccessible(d.Id, _simDay))
00122:                 {
00123:                     list.Add(d);
00124:                 }
00125:             }
00126:
00127:             // Plan IV Task 5: surface pending trapping dilemmas in outbox
00128:             // sequence order. The pending fact is the persistence owner until
00129:             // the player resolves the dilemma in the moral ledger.
00130:             if (_wildlifeTrapping != null)
00131:             {
00132:                 var pending = _wildlifeTrapping.System.GetPendingEvents();
00133:                 foreach (var ev in pending)
00134:                 {
00135:                     if (ev == null || !string.Equals(ev.kind, WildlifeTrappingEventKinds.MoralConsequence, StringComparison.Ordinal))
00136:                         continue;
00137:                     var def = GetMoralChoiceDef(ev.payloadId);
00138:                     if (def == null || _moralChoice.IsResolved(def.Id)) continue;
00139:                     if (!list.Any(l => string.Equals(l.Id, def.Id, StringComparison.Ordinal)))
00140:                         list.Add(def);
00141:                 }
00142:             }
00143:             return list;
00144:         }
00145:
00146:         /// <summary>Catalog id prefix shared by all trapping-sourced moral dilemmas.</summary>
00147:         public const string TrappingMoralQuestIdPrefix = "quest_moral_trap_prey_";
00148:
00149:         /// <summary>
00150:         /// Plan IV Task 5: ack every pending moral-consequence fact that maps
00151:         /// to the given quest id. Called from the resolution event so the
00152:         /// trapping outbox marks the fact delivered only after the moral
00153:         /// ledger committed the resolution.
00154:         /// </summary>
00155:         private void MarkTrappingMoralEventDelivered(string questId)
00156:         {
00157:             if (_wildlifeTrapping == null) return;
00158:             var pending = _wildlifeTrapping.System.GetPendingEvents();
00159:             foreach (var ev in pending)
00160:             {
00161:                 if (ev == null || !string.Equals(ev.kind, WildlifeTrappingEventKinds.MoralConsequence, StringComparison.Ordinal))
00162:                     continue;
00163:                 if (!string.Equals(ev.payloadId, questId, StringComparison.Ordinal)) continue;
00164:                 _wildlifeTrapping.System.MarkEventDelivered(ev.eventId);
00165:             }
00166:         }
00167:
00168:         public List<MoralChoiceQuestDefinition> GetResolvedMoralChoices()
00169:         {
00170:             SetupMoralChoice();
00171:             var list = new List<MoralChoiceQuestDefinition>();
00172:             foreach (var d in _moralChoiceDefs)
00173:             {
00174:                 if (_moralChoice.IsResolved(d.Id))
00175:                     list.Add(d);
00176:             }
00177:             return list;
00178:         }
00179:
00180:         public MoralChoiceResolution? GetMoralChoiceResolution(string questId)
00181:         {
00182:             SetupMoralChoice();
00183:             if (_moralChoice.TryGetResolution(questId, out var res))
00184:                 return res;
00185:             return null;
00186:         }
00187:
00188:         public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyMoralOffers(int maxOffers = 1)
00189:         {
00190:             SetupMoralChoice();
00191:             return _moralChoice.GetDailyOffers(_simDay, maxOffers);
00192:         }
00193:
00194:         /// <summary>
00195:         /// Resolve a catalog quest by id. Returns false when the id is unknown
00196:         /// or the quest is already resolved; the journal line is written by
00197:         /// the event hook, and overnight settlement lands in TickSimDay.
00198:         /// </summary>
00199:         public bool TryResolveMoralChoice(string questId, int choiceIndex)
00200:         {
00201:             SetupMoralChoice();
00202:             var def = _moralChoiceDefs.FirstOrDefault(
00203:                 d => string.Equals(d.Id, questId, StringComparison.Ordinal));
00204:             if (def == null) return false;
00205:             if (!_moralChoice.TryResolve(questId, choiceIndex, def.LocationId, _simDay, out var resolveResult))
00206:                 return false;
00207:
00208:             // CORE-MECH W8: a choice with a public footprint seeds the canonical
00209:             // rumor network. RumorSystem stays the only rumor authority; the seed
00210:             // builder is pure; the one-shot trigger guarantees one seed per choice.
00211:             SeedMoralChoiceGossip(resolveResult?.Resolution);
00212:
00213:             _moralChoiceDirty = true;
00214:             AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.UiConfirm);
00215:             return true;
00216:         }
00217:
00218:         /// <summary>
00219:         /// CORE-MECH W8 — turn a resolved choice into a rumor seed and hand it to
00220:         /// the existing RumorSystem. Uses the authored <c>propagatesOnDay</c> hook
00221:         /// (gossip leaves the witnessing circle on resolvedDay + 1..3). Fails closed
00222:         /// when the info owner is not set up: the choice still resolves.
00223:         /// </summary>
00224:         private void SeedMoralChoiceGossip(Ashfall.Core.MoralChoice.MoralChoiceResolution? resolution)
00225:         {
00226:             if (resolution == null || string.IsNullOrEmpty(resolution.questId)) return;
00227:
00228:             float impact = Math.Clamp(Math.Abs(resolution.moralDelta) / 20f, 0f, 1f);
00229:             if (resolution.empathyDelta > 0) impact = Math.Clamp(impact + 0.15f, 0f, 1f);
00230:
00231:             var seed = Ashfall.Core.MoralChoice.MoralChoiceGossipSeed.Build(
00232:                 resolution.questId,
00233:                 resolution.locationId,
00234:                 resolution.propagatesOnDay > 0 ? resolution.propagatesOnDay : resolution.resolvedDay,
00235:                 resolution.epitaph,
00236:                 impact,
00237:                 isPrivate: false);
00238:             if (seed == null) return;
00239:
00240:             SetupRumorNetwork();
00241:             if (_rumorNetwork == null) return;
00242:
00243:             // One seed per choice, ever — the W11 primitive rebuilt inline to clear
00244:             // the W8→W11 ordering dependency.
00245:             var triggers = GossipTriggers();
00246:             if (!triggers.TryFire("moral." + seed.SubjectId, Math.Max(1, seed.OriginDay)))
00247:                 return;
00248:
00249:             var rumor = _rumorNetwork.System.GenerateRumor(
00250:                 seed.OriginLocationId,
00251:                 Ashfall.Core.InformationFlow.RumorSubjectType.Faction,
00252:                 seed.SubjectId,
00253:                 seed.Headline,
00254:                 seed.Description,
00255:                 seed.Truthfulness,
00256:                 seed.OriginDay);
00257:             rumor.DecayRate = seed.DecayRate;
00258:             rumor.PropagationSpeed = seed.PropagationSpeed;
00259:             // The rumor host raises its own StateChanged on generation (Main binds
00260:             // that to the dirty flag), so the seed needs no extra bookkeeping.
00261:             _rumorNetworkDirty = true;
00262:         }
00263:
00264:         /// <summary>W8/W11 shared trigger ledger over the campaign consequence ledger.</summary>
00265:         private Ashfall.Core.Flags.OneShotTriggerLedger GossipTriggers()
00266:             => _gossipTriggers ??= new Ashfall.Core.Flags.OneShotTriggerLedger(_consequenceLedger);
00267:
00268:         private Ashfall.Core.Flags.OneShotTriggerLedger? _gossipTriggers;
00269:
00270:         /// <summary>Journal integration: one entry per resolution, arrow only — never the number.</summary>
00271:         private void WriteMoralChoiceJournalEntry(MoralChoiceResolution resolution)
00272:         {
00273:             SetupJournal();
00274:             string arrow = resolution.impactMark == "up" ? "🔺"
00275:                 : resolution.impactMark == "down" ? "🔻" : "⚪";
00276:             _journal.TryAddRawEntry(resolution.questId, $"{arrow} {resolution.epitaph}", null!, resolution.resolvedDay);
00277:             _journalDirty = true;
00278:         }
00279:
00280:         /// <summary>Branch lockout journal entry: a door has closed.</summary>
00281:         private void WriteBranchLockoutJournalEntry(string lockedBranchId)
00282:         {
00283:             if (_moralChainData?.LockoutRules == null) return;
00284:             var branch = _moralChainData.Branches.FirstOrDefault(
00285:                 b => string.Equals(b.Id, lockedBranchId, StringComparison.Ordinal));
00286:             string branchName = branch?.DisplayName ?? lockedBranchId;
00287:             string template = _moralChainData.LockoutRules.LockoutJournalTemplate;
00288:             string text = template.Replace("{locked_branch_name}", branchName);
00289:
00290:             SetupJournal();
00291:             _journal.TryAddRawEntry($"branch_lockout_{lockedBranchId}", text, null!, _simDay);
00292:             _journalDirty = true;
00293:         }
00294:
00295:         /// <summary>
00296:         /// Get the faction reaction dialogue for a threshold event.
00297:         /// Returns null if no reaction data exists for the event.
00298:         /// </summary>
00299:         private MoralThresholdReaction? GetFactionReaction(string eventId)
00300:         {
00301:             SetupMoralChoice();
00302:             if (_moralFactionReactions.ThresholdReactions.TryGetValue(eventId, out var reaction))
00303:                 return reaction;
00304:             return null;
00305:         }
00306:
00307:         /// <summary>
00308:         /// Journal the authored faction reaction when a moral threshold fires.
00309:         /// Uses the catalog journal line when present; otherwise a restrained fallback.
00310:         /// </summary>
00311:         private void WriteThresholdEventJournalEntry(string eventId)
00312:         {
00313:             var reaction = GetFactionReaction(eventId);
00314:             string text = reaction != null && !string.IsNullOrWhiteSpace(reaction.JournalEntry)
00315:                 ? reaction.JournalEntry
00316:                 : $"Threshold crossed: {eventId}.";
00317:
00318:             SetupJournal();
00319:             _journal.TryAddRawEntry($"moral_threshold_{eventId}", text, null!, _simDay);
00320:             _journalDirty = true;
00321:         }
00322:
00323:         /// <summary>
00324:         /// Get the current gossip band (with decay) for NPC interactions.
00325:         /// </summary>
00326:         private MoralPathBand GetCurrentGossipBand()
00327:         {
00328:             SetupMoralChoice();
00329:             return _moralGossipRuntime.GetEffectiveGossipBand(_moralChoice, _simDay);
00330:         }
00331:
00332:         private void SaveMoralChoice()
00333:         {
00334:             if (_moralChoice == null) return;
00335:             if (CaptureSection("moral_choice", MoralChoiceSaveStore.TryCapturePersisted(_moralChoice.CaptureState())))
00336:                 _moralChoiceDirty = false;
00337:         }
00338:
00339:         private void FlushMoralChoiceIfDirty()
00340:         {
00341:             if (_moralChoiceDirty) SaveMoralChoice();
00342:         }
00343:     }
00344: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs`

### `Ashfall.Core.Tests/MoralChoiceFactionReactionsExpansionTests.cs` — complete current file

- Size: 248 lines / 10617 bytes.
- SHA-256: `71802e0882c69e830cd7c2bd3d0530f55223a56ba01afa2dfb6c8b6c9d51be5f`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core.MoralChoice;
00007: using Ashfall.Core.Random;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests
00011: {
00012:     public sealed class MoralChoiceFactionReactionsExpansionTests : CatalogTestBase
00013:     {
00014:         private static readonly IFileIO s_files = new FileSystemIO();
00015:         private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
00016:
00017:         private static readonly string[] CanonicalEventIds = new[]
00018:         {
00019:             MoralChoiceSystem.EventBountyIssued,
00020:             MoralChoiceSystem.EventContractTaken,
00021:             MoralChoiceSystem.EventContractRaised,
00022:             MoralChoiceSystem.EventPatrolDefense,
00023:             MoralChoiceSystem.EventLegendPositive,
00024:             MoralChoiceSystem.EventLegendNegative
00025:         };
00026:
00027:         [Fact]
00028:         public void Catalog_LoadsExactSixCanonicalReactions()
00029:         {
00030:             var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
00031:             Assert.NotNull(data);
00032:             Assert.Equal(6, data.ThresholdReactions.Count);
00033:         }
00034:
00035:         [Fact]
00036:         public void Catalog_ContainsAllSixCanonicalEventIds()
00037:         {
00038:             var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
00039:             foreach (var expectedId in CanonicalEventIds)
00040:             {
00041:                 Assert.True(data.ThresholdReactions.ContainsKey(expectedId),
00042:                     $"Expected threshold reaction '{expectedId}' to be present in catalog.");
00043:             }
00044:         }
00045:
00046:         [Fact]
00047:         public void Catalog_EveryReactionHasNonEmptyEventDescription()
00048:         {
00049:             var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
00050:             foreach (var kvp in data.ThresholdReactions)
00051:             {
00052:                 Assert.False(string.IsNullOrWhiteSpace(kvp.Value.EventDescription),
00053:                     $"Event '{kvp.Key}' has missing or empty EventDescription.");
00054:             }
00055:         }
00056:
00057:         [Fact]
00058:         public void Catalog_EveryReactionHasAllThreeFactionDialogues()
00059:         {
00060:             var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
00061:             foreach (var kvp in data.ThresholdReactions)
00062:             {
00063:                 var reaction = kvp.Value;
00064:                 Assert.NotEmpty(reaction.PeacekeeperDialogue);
00065:                 Assert.NotEmpty(reaction.RaiderDialogue);
00066:                 Assert.NotEmpty(reaction.KnowledgeKeeperDialogue);
00067:             }
00068:         }
00069:
00070:         [Fact]
00071:         public void Catalog_AllDialogueBlocksHaveValidSpeakerLocationAndLines()
00072:         {
00073:             var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
00074:             foreach (var kvp in data.ThresholdReactions)
00075:             {
00076:                 var factionLists = new[]
00077:                 {
00078:                     ("Peacekeeper", kvp.Value.PeacekeeperDialogue),
00079:                     ("Raider", kvp.Value.RaiderDialogue),
00080:                     ("KnowledgeKeeper", kvp.Value.KnowledgeKeeperDialogue),
00081:                     ("Civilian", kvp.Value.CivilianDialogue)
00082:                 };
00083:
00084:                 foreach (var (factionName, blocks) in factionLists)
00085:                 {
00086:                     if (blocks == null) continue;
00087:                     foreach (var block in blocks)
00088:                     {
00089:                         Assert.False(string.IsNullOrWhiteSpace(block.Speaker),
00090:                             $"Event '{kvp.Key}' [{factionName}] has an empty Speaker.");
00091:                         Assert.False(string.IsNullOrWhiteSpace(block.Location),
00092:                             $"Event '{kvp.Key}' [{factionName}] has an empty Location.");
00093:                         Assert.InRange(block.Lines.Count, 3, 6);
00094:                         foreach (var line in block.Lines)
00095:                         {
00096:                             Assert.False(string.IsNullOrWhiteSpace(line),
00097:                                 $"Event '{kvp.Key}' [{factionName}] has an empty dialogue line.");
00098:                         }
00099:                     }
00100:                 }
00101:             }
00102:         }
00103:
00104:         [Fact]
00105:         public void Catalog_EveryReactionHasNonEmptyJournalEntry()
00106:         {
00107:             var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
00108:             foreach (var kvp in data.ThresholdReactions)
00109:             {
00110:                 Assert.False(string.IsNullOrWhiteSpace(kvp.Value.JournalEntry),
00111:                     $"Event '{kvp.Key}' is missing JournalEntry.");
00112:             }
00113:         }
00114:
00115:         [Fact]
00116:         public void Catalog_PreservesBountyIssuedBaselineParity()
00117:         {
00118:             var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
00119:             var bounty = data.ThresholdReactions[MoralChoiceSystem.EventBountyIssued];
00120:
00121:             Assert.Equal("Fires when the player enters VeryEvil band (-100 or below). Peacekeepers issue a bounty.", bounty.EventDescription);
00122:             var pkSergeant = bounty.PeacekeeperDialogue.FirstOrDefault(d => d.Speaker.Contains("Veill"));
00123:             Assert.NotNull(pkSergeant);
00124:             Assert.Contains("Your face is on the board now. I put it there myself.", pkSergeant.Lines);
00125:
00126:             var raiderLookout = bounty.RaiderDialogue.FirstOrDefault();
00127:             Assert.NotNull(raiderLookout);
00128:             Assert.Contains("The Peacekeepers put a price on you. You know what that means to us?", raiderLookout.Lines);
00129:
00130:             var archivist = bounty.KnowledgeKeeperDialogue.FirstOrDefault();
00131:             Assert.NotNull(archivist);
00132:             Assert.Contains("We record everything. You know that.", archivist.Lines);
00133:         }
00134:
00135:         [Fact]
00136:         public void Runtime_BandCrossings_TriggerAllCanonicalReactionsOnce()
00137:         {
00138:             var rng = new SeededRng(42);
00139:             var sys = new MoralChoiceSystem(rng);
00140:             var firedEvents = new List<string>();
00141:             sys.OnThresholdEventFired += firedEvents.Add;
00142:
00143:             // Day 1: Drive into Positive (+60)
00144:             sys.Resolve(CreateQuest("quest_moral_pos_1", 60), 0, "loc_a", 1);
00145:             sys.Reconcile(1);
00146:             Assert.Single(firedEvents);
00147:             Assert.Contains(MoralChoiceSystem.EventContractTaken, firedEvents);
00148:
00149:             // Day 2: Drive into VeryPositive (+120)
00150:             sys.Resolve(CreateQuest("quest_moral_pos_2", 60), 0, "loc_b", 2);
00151:             sys.Reconcile(2);
00152:             Assert.Equal(3, firedEvents.Count);
00153:             Assert.Contains(MoralChoiceSystem.EventContractRaised, firedEvents);
00154:             Assert.Contains(MoralChoiceSystem.EventPatrolDefense, firedEvents);
00155:
00156:             // Day 3: Reconcile again without score change — no new events
00157:             sys.Reconcile(3);
00158:             Assert.Equal(3, firedEvents.Count);
00159:
00160:             // Day 4: Drive into VeryEvil (-120 total)
00161:             sys.Resolve(CreateQuest("quest_moral_neg_1", -240), 0, "loc_c", 4);
00162:             sys.Reconcile(4);
00163:             Assert.Equal(4, firedEvents.Count);
00164:             Assert.Contains(MoralChoiceSystem.EventBountyIssued, firedEvents);
00165:
00166:             // Day 5: Re-crossing back into VeryPositive does NOT refire already fired events
00167:             sys.Resolve(CreateQuest("quest_moral_pos_3", 240), 0, "loc_d", 5);
00168:             sys.Reconcile(5);
00169:             Assert.Equal(4, firedEvents.Count);
00170:         }
00171:
00172:         [Fact]
00173:         public void Runtime_OverflowLegends_TriggerOnceOvernight()
00174:         {
00175:             var rng = new SeededRng(1337);
00176:             var sys = new MoralChoiceSystem(rng);
00177:             var firedEvents = new List<string>();
00178:             sys.OnThresholdEventFired += firedEvents.Add;
00179:
00180:             // Push past +200 MaxScore
00181:             sys.Resolve(CreateQuest("quest_moral_overflow_pos", 250), 0, "loc_sanctum", 1);
00182:             Assert.Equal(MoralChoiceSystem.MaxScore, sys.MoralScore);
00183:             Assert.Empty(firedEvents); // Pending until overnight Reconcile
00184:
00185:             sys.Reconcile(2);
00186:             Assert.Contains(MoralChoiceSystem.EventLegendPositive, firedEvents);
00187:
00188:             // Second positive overflow does not refire
00189:             sys.Resolve(CreateQuest("quest_moral_overflow_pos_2", 50), 0, "loc_sanctum", 3);
00190:             sys.Reconcile(4);
00191:             Assert.Equal(1, firedEvents.Count(id => id == MoralChoiceSystem.EventLegendPositive));
00192:
00193:             // Push past -200 MinScore
00194:             sys.Resolve(CreateQuest("quest_moral_overflow_neg", -500), 0, "loc_ruins", 5);
00195:             Assert.Equal(MoralChoiceSystem.MinScore, sys.MoralScore);
00196:             sys.Reconcile(6);
00197:             Assert.Contains(MoralChoiceSystem.EventLegendNegative, firedEvents);
00198:
00199:             // Second negative overflow does not refire
00200:             sys.Resolve(CreateQuest("quest_moral_overflow_neg_2", -50), 0, "loc_ruins", 7);
00201:             sys.Reconcile(8);
00202:             Assert.Equal(1, firedEvents.Count(id => id == MoralChoiceSystem.EventLegendNegative));
00203:         }
00204:
00205:         [Fact]
00206:         public void Runtime_StateCaptureAndRestore_PreservesFiredEvents()
00207:         {
00208:             var rng = new SeededRng(999);
00209:             var original = new MoralChoiceSystem(rng);
00210:             original.Resolve(CreateQuest("quest_moral_init_a", 150), 0, "loc_1", 1);
00211:             original.Reconcile(2);
00212:
00213:             var state = original.CaptureState();
00214:             Assert.Contains(MoralChoiceSystem.EventContractTaken, state.firedThresholdEvents);
00215:             Assert.Contains(MoralChoiceSystem.EventContractRaised, state.firedThresholdEvents);
00216:             Assert.Contains(MoralChoiceSystem.EventPatrolDefense, state.firedThresholdEvents);
00217:
00218:             var restored = new MoralChoiceSystem(new SeededRng(999));
00219:             restored.RestoreState(state);
00220:
00221:             var firedInRestored = new List<string>();
00222:             restored.OnThresholdEventFired += firedInRestored.Add;
00223:
00224:             // Reconcile on future day with same band
00225:             restored.Reconcile(3);
00226:             Assert.Empty(firedInRestored);
00227:         }
00228:
00229:         private static MoralChoiceQuestDefinition CreateQuest(string id, int moralDelta)
00230:         {
00231:             return new MoralChoiceQuestDefinition
00232:             {
00233:                 Id = id,
00234:                 DisplayName = $"Test Quest {id}",
00235:                 Category = "trust",
00236:                 Choices = new List<MoralChoiceOption>
00237:                 {
00238:                     new MoralChoiceOption
00239:                     {
00240:                         MoralDelta = moralDelta,
00241:                         EmpathyDelta = 0,
00242:                         Epitaph = "tested choice"
00243:                     }
00244:                 }
00245:             };
00246:         }
00247:     }
00248: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` — complete current file

- Size: 687 lines / 30717 bytes.
- SHA-256: `4cb9adafbbbdfc153d1c80ac2595f1e6c870a6669e2c9e85844a6a416133c9c1`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005:
00006: namespace Ashfall.Core.MoralChoice
00007: {
00008:     public enum MoralPathBand
00009:     {
00010:         VeryEvil,
00011:         Evil,
00012:         SlightlyEvil,
00013:         Neutral,
00014:         SlightlyPositive,
00015:         Positive,
00016:         VeryPositive
00017:     }
00018:
00019:     public enum MoralEndingKind
00020:     {
00021:         Warlord,
00022:         SurvivorKing,
00023:         NeutralSurvivor,
00024:         BalancedSurvivor,
00025:         CommunityBuilder,
00026:         Savior,
00027:         SaintOfWasteland,
00028:         Storykeeper
00029:     }
00030:
00031:     /// <summary>
00032:     /// In-code quest shape. Phase 2 loads these from
00033:     /// Assets/StreamingAssets/Data/moral_choice_quests.json; ids must use the
00034:     /// canonical quest_moral_ prefix (the design doc drafts them as
00035:     /// qst_moral_* — that spelling is rejected on purpose).
00036:     /// </summary>
00037:     public sealed class MoralChoiceQuestDefinition
00038:     {
00039:         public string Id { get; set; } = string.Empty;
00040:         public string DisplayName { get; set; } = string.Empty;
00041:
00042:         /// <summary>share | listen | comfort | dead | trust</summary>
00043:         public string Category { get; set; } = string.Empty;
00044:
00045:         public string Trigger { get; set; } = string.Empty;
00046:
00047:         /// <summary>Encounter prose shown when the quest is discovered.</summary>
00048:         public string Discovery { get; set; } = string.Empty;
00049:
00050:         public string LocationId { get; set; } = string.Empty;
00051:
00052:         /// <summary>First day the quest may be offered; 0 = always.</summary>
00053:         public int MinDay { get; set; }
00054:
00055:         /// <summary>Last day the quest may be offered; 0 or negative = unbounded.</summary>
00056:         public int MaxDay { get; set; }
00057:
00058:         public List<MoralChoiceOption> Choices { get; set; } = new List<MoralChoiceOption>();
00059:     }
00060:
00061:     public sealed class MoralChoiceOption
00062:     {
00063:         /// <summary>UI text for the choice, e.g. "Give all your food".</summary>
00064:         public string Label { get; set; } = string.Empty;
00065:
00066:         public int MoralDelta { get; set; }
00067:         public int EmpathyDelta { get; set; }
00068:         /// <summary>Optional canonical historical flag written after this choice commits.</summary>
00069:         public string SetFlag { get; set; } = string.Empty;
00070:         public string OutcomeText { get; set; } = string.Empty;
00071:         public string Epitaph { get; set; } = string.Empty;
00072:     }
00073:
00074:     /// <summary>
00075:     /// Engine-agnostic moral choice ledger: invisible moral + empathy
00076:     /// accumulators, band computation, overnight reconciliation with
00077:     /// one-time threshold events, ending selection, branch tracking,
00078:     /// quest gating, and echo quest availability. The score is never
00079:     /// surfaced to the player — the world is the UI (host layers read
00080:     /// CurrentBand / events, never the raw number).
00081:     /// </summary>
00082:     public sealed class MoralChoiceSystem
00083:     {
00084:         public const string SystemId = "moral_choice";
00085:         public const string QuestIdPrefix = "quest_moral_";
00086:
00087:         public const int MinScore = -200;
00088:         public const int MaxScore = 200;
00089:
00090:         public const int ListenerEmpathyThreshold = 15;
00091:         public const int ConfidantEmpathyThreshold = 30;
00092:         public const int StorykeeperEmpathyThreshold = 45;
00093:         public const int StorykeeperQuestThreshold = 25;
00094:
00095:         /// <summary>Below this many resolved quests no ending band locks; mild endings fire instead.</summary>
00096:         public const int EndingLockMinQuests = 20;
00097:
00098:         public const string EventLegendPositive = "moral_event_legend_positive";
00099:         public const string EventLegendNegative = "moral_event_legend_negative";
00100:         public const string EventBountyIssued = "moral_event_bounty_issued";
00101:         public const string EventContractTaken = "moral_event_contract_taken";
00102:         public const string EventContractRaised = "moral_event_contract_raised";
00103:         public const string EventPatrolDefense = "moral_event_patrol_defense";
00104:
00105:         /// <summary>Pending-overflow bits settled at the next Reconcile (bit 1 = positive, bit 2 = negative).</summary>
00106:         public const int LegendPositiveFlag = 1;
00107:         public const int LegendNegativeFlag = 2;
00108:
00109:         private readonly ISeededRng _rng;
00110:         private readonly ILog _log;
00111:         private readonly Flags.IFlagLedger? _flags;
00112:         private MoralChoiceState _state = new MoralChoiceState();
00113:
00114:         /// <summary>Branch architecture from moral_choice_chains.json; null until InitializeChainData.</summary>
00115:         private MoralChoiceChainData? _chainData;
00116:         private readonly Dictionary<string, MoralChoiceQuestDefinition> _catalog = new Dictionary<string, MoralChoiceQuestDefinition>(StringComparer.Ordinal);
00117:         private Dictionary<string, string> _questToBranch = new Dictionary<string, string>();
00118:         private HashSet<string> _entryQuestSet = new HashSet<string>();
00119:
00120:         public event Action<MoralChoiceResolution>? OnQuestResolved;
00121:         public event Action<string>? OnThresholdEventFired;
00122:         public event Action<string>? OnBranchLocked;
00123:
00124:         public MoralChoiceSystem(ISeededRng rng, ILog? log = null, Flags.IFlagLedger? flags = null)
00125:         {
00126:             _rng = rng ?? throw new ArgumentNullException(nameof(rng));
00127:             _log = log ?? NullLog.Instance;
00128:             _flags = flags;
00129:         }
00130:
00131:         public MoralChoiceState State => _state;
00132:         public int MoralScore => _state.moralScore;
00133:         public int EmpathyPoints => _state.empathyPoints;
00134:         public int QuestsResolved => _state.resolutions.Count;
00135:         public MoralPathBand CurrentBand => BandForScore(_state.moralScore);
00136:         public IReadOnlyList<MoralChoiceResolution> Resolutions => _state.resolutions;
00137:
00138:         public bool IsListener => _state.empathyPoints >= ListenerEmpathyThreshold;
00139:         public bool IsConfidant => _state.empathyPoints >= ConfidantEmpathyThreshold;
00140:
00141:         /// <summary>Chain data reference; null if not yet initialized.</summary>
00142:         public MoralChoiceChainData? ChainData => _chainData;
00143:
00144:         /// <summary>
00145:         /// Load the branching architecture (moral_choice_chains.json). Builds
00146:         /// internal lookup maps for branch ownership and entry-quest tracking.
00147:         /// Safe to call once at startup; subsequent calls are no-ops.
00148:         /// </summary>
00149:         public void InitializeChainData(MoralChoiceChainData chainData)
00150:         {
00151:             if (chainData == null || _chainData != null) return;
00152:             _chainData = chainData;
00153:
00154:             foreach (var branch in chainData.Branches)
00155:             {
00156:                 foreach (var entryQuest in branch.EntryQuests)
00157:                 {
00158:                     _questToBranch[entryQuest] = branch.Id;
00159:                     _entryQuestSet.Add(entryQuest);
00160:                 }
00161:             }
00162:             foreach (var gate in chainData.QuestGates)
00163:             {
00164:                 if (!string.IsNullOrEmpty(gate.Branch) && !string.IsNullOrEmpty(gate.QuestId))
00165:                 {
00166:                     _questToBranch[gate.QuestId] = gate.Branch;
00167:                 }
00168:             }
00169:         }
00170:
00171:         public static bool IsCanonicalQuestId(string questId) =>
00172:             questId.StartsWith(QuestIdPrefix, StringComparison.Ordinal);
00173:
00174:         /// <summary>MaxDay &lt;= 0 means unbounded; a malformed window (max &lt; min) is never available.</summary>
00175:         public static bool IsAvailableOnDay(MoralChoiceQuestDefinition quest, int day) =>
00176:             day >= quest.MinDay && (quest.MaxDay <= 0 || (day <= quest.MaxDay && quest.MaxDay >= quest.MinDay));
00177:
00178:         public bool IsResolved(string questId) => TryGetResolution(questId, out _);
00179:
00180:         public bool TryGetResolution(string questId, out MoralChoiceResolution? resolution)
00181:         {
00182:             resolution = _state.resolutions.FirstOrDefault(r => string.Equals(r.questId, questId, StringComparison.Ordinal));
00183:             return resolution != null;
00184:         }
00185:
00186:         // ── Catalog registration ───────────────────────────────────────
00187:
00188:         public void RegisterQuest(MoralChoiceQuestDefinition def)
00189:         {
00190:             if (def == null || string.IsNullOrEmpty(def.Id)) return;
00191:             _catalog[def.Id] = def;
00192:         }
00193:
00194:         public void RegisterQuests(IEnumerable<MoralChoiceQuestDefinition> defs)
00195:         {
00196:             if (defs == null) return;
00197:             foreach (var def in defs)
00198:                 RegisterQuest(def);
00199:         }
00200:
00201:         public IReadOnlyDictionary<string, MoralChoiceQuestDefinition> Catalog => _catalog;
00202:         public int CatalogCount => _catalog.Count;
00203:
00204:         public MoralChoiceQuestDefinition? GetQuest(string id) =>
00205:             !string.IsNullOrEmpty(id) && _catalog.TryGetValue(id, out var def) ? def : null;
00206:
00207:         // ── Seeded daily offers ────────────────────────────────────────
00208:
00209:         /// <summary>
00210:         /// Returns deterministic daily moral choice offers for the given day.
00211:         /// Uses seed formula: unchecked((ulong)_rng.Seed * 31337UL + (ulong)day * 1009UL + 0x5EEDUL).
00212:         /// Only returns unresolved quests whose day window is active and whose chain prerequisites/gates are met.
00213:         /// </summary>
00214:         public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyOffers(int day, int maxOffers = 1)
00215:         {
00216:             if (maxOffers <= 0 || _catalog.Count == 0) return Array.Empty<MoralChoiceQuestDefinition>();
00217:
00218:             var candidates = new List<MoralChoiceQuestDefinition>();
00219:             foreach (var kv in _catalog)
00220:             {
00221:                 var q = kv.Value;
00222:                 if (!IsResolved(q.Id) && IsAvailableOnDay(q, day) && IsChainQuestAccessible(q.Id, day))
00223:                 {
00224:                     candidates.Add(q);
00225:                 }
00226:             }
00227:
00228:             if (candidates.Count == 0) return Array.Empty<MoralChoiceQuestDefinition>();
00229:
00230:             // Sort deterministically by Id
00231:             candidates.Sort((a, b) => string.Compare(a.Id, b.Id, StringComparison.Ordinal));
00232:
00233:             if (candidates.Count <= maxOffers) return candidates;
00234:
00235:             ulong dailySeedRaw = unchecked((ulong)_rng.Seed * 31337UL + (ulong)day * 1009UL + 0x5EEDUL);
00236:             int dailySeed = unchecked((int)(dailySeedRaw ^ (dailySeedRaw >> 32)));
00237:             var dailyRng = new SeededRng(dailySeed);
00238:
00239:             var pool = new List<MoralChoiceQuestDefinition>(candidates);
00240:             var selected = new List<MoralChoiceQuestDefinition>(maxOffers);
00241:             for (int i = 0; i < maxOffers && pool.Count > 0; i++)
00242:             {
00243:                 int idx = dailyRng.Next(0, pool.Count);
00244:                 selected.Add(pool[idx]);
00245:                 pool.RemoveAt(idx);
00246:             }
00247:
00248:             return selected;
00249:         }
00250:
00251:         // ── Structured resolution ──────────────────────────────────────
00252:
00253:         /// <summary>
00254:         /// Attempts to resolve a registered moral choice quest.
00255:         /// Enforces strict single-resolution, catalog validity, day window, and choice index bounds
00256:         /// returning structured status codes without throwing on invalid player/client input.
00257:         /// </summary>
00258:         public bool TryResolve(string questId, int choiceIndex, string locationId, int day, out MoralResolveResult result)
00259:         {
00260:             if (string.IsNullOrEmpty(questId) || !_catalog.TryGetValue(questId, out var def))
00261:             {
00262:                 result = MoralResolveResult.Failed(MoralResolveResultCode.UnknownChoice,
00263:                     $"Quest '{questId}' is not registered in moral choice catalog.");
00264:                 return false;
00265:             }
00266:
00267:             if (TryGetResolution(questId, out var existing))
00268:             {
00269:                 result = MoralResolveResult.Failed(MoralResolveResultCode.AlreadyResolved,
00270:                     $"Quest '{questId}' was already resolved on day {existing!.resolvedDay}.", existing);
00271:                 return false;
00272:             }
00273:
00274:             if (!IsAvailableOnDay(def, day))
00275:             {
00276:                 result = MoralResolveResult.Failed(MoralResolveResultCode.ChoiceNotAvailable,
00277:                     $"Quest '{questId}' is not available on day {day} (window: {def.MinDay}..{def.MaxDay}).");
00278:                 return false;
00279:             }
00280:
00281:             if (!IsChainQuestAccessible(questId, day))
00282:             {
00283:                 result = MoralResolveResult.Failed(MoralResolveResultCode.RequirementMissing,
00284:                     $"Quest '{questId}' chain gate or branch requirements are not met.");
00285:                 return false;
00286:             }
00287:
00288:             if (choiceIndex < 0 || choiceIndex >= def.Choices.Count)
00289:             {
00290:                 result = MoralResolveResult.Failed(MoralResolveResultCode.UnknownOption,
00291:                     $"Choice index {choiceIndex} is out of bounds for quest '{questId}' (0..{def.Choices.Count - 1}).");
00292:                 return false;
00293:             }
00294:
00295:             var resolution = Resolve(def, choiceIndex, locationId, day);
00296:             result = MoralResolveResult.Succeeded(resolution);
00297:             return true;
00298:         }
00299:
00300:         // ── Branch tracking ────────────────────────────────────────────
00301:
00302:         /// <summary>Whether a branch has been permanently locked by the lockout mechanic.</summary>
00303:         public bool IsBranchLocked(string branchId) =>
00304:             _state.lockedBranches.Contains(branchId);
00305:
00306:         /// <summary>How many entry quests the player has resolved for a branch.</summary>
00307:         public int GetBranchProgress(string branchId) =>
00308:             _state.branchProgress.TryGetValue(branchId, out int v) ? v : 0;
00309:
00310:         /// <summary>Which branch owns a quest (by chain data); empty string if not a chain quest.</summary>
00311:         public string GetQuestBranch(string questId) =>
00312:             _questToBranch.TryGetValue(questId, out var b) ? b : string.Empty;
00313:
00314:         /// <summary>
00315:         /// Whether a chain quest is accessible: branch not locked, gate
00316:         /// prerequisites met, day window valid, and not already resolved.
00317:         /// Non-chain quests (base/expansion) only check day + resolved.
00318:         /// </summary>
00319:         public bool IsChainQuestAccessible(string questId, int day)
00320:         {
00321:             if (IsResolved(questId)) return false;
00322:
00323:             if (_questToBranch.TryGetValue(questId, out var branchId))
00324:             {
00325:                 if (IsBranchLocked(branchId)) return false;
00326:             }
00327:
00328:             var gate = _chainData?.QuestGates.FirstOrDefault(
00329:                 g => string.Equals(g.QuestId, questId, StringComparison.Ordinal));
00330:             if (gate != null && !EvaluateGate(gate)) return false;
00331:
00332:             return true;
00333:         }
00334:
00335:         /// <summary>
00336:         /// Evaluate a quest gate's prerequisites: prior quests resolved,
00337:         /// moral/empathy thresholds, and flag requirements.
00338:         /// </summary>
00339:         public bool EvaluateGate(MoralQuestGate gate)
00340:         {
00341:             if (gate == null) return true;
00342:
00343:             foreach (var req in gate.Requires)
00344:             {
00345:                 if (!IsResolved(req)) return false;
00346:             }
00347:
00348:             if (gate.RequiresMinMoral.HasValue && _state.moralScore < gate.RequiresMinMoral.Value) return false;
00349:             if (gate.RequiresMaxMoral.HasValue && _state.moralScore > gate.RequiresMaxMoral.Value) return false;
00350:             if (gate.RequiresMinEmpathy.HasValue && _state.empathyPoints < gate.RequiresMinEmpathy.Value) return false;
00351:
00352:             if (!string.IsNullOrEmpty(gate.RequiresFlag) && !_state.activeFlags.Contains(gate.RequiresFlag)) return false;
00353:
00354:             return true;
00355:         }
00356:
00357:         /// <summary>Set a moral flag (idempotent).</summary>
00358:         public void SetFlag(string flagId)
00359:         {
00360:             if (string.IsNullOrEmpty(flagId)) return;
00361:             if (!_state.activeFlags.Contains(flagId))
00362:                 _state.activeFlags.Add(flagId);
00363:             _flags?.Set(flagId, "moral_choice");
00364:         }
00365:
00366:         /// <summary>Whether a moral flag is currently set.</summary>
00367:         public bool HasFlag(string flagId) =>
00368:             !string.IsNullOrEmpty(flagId) && (_flags != null ? (_flags.IsSet(flagId) || _state.activeFlags.Contains(flagId)) : _state.activeFlags.Contains(flagId));
00369:
00370:         // ── Echo quests ────────────────────────────────────────────────
00371:
00372:         /// <summary>
00373:         /// Find echo quests that should fire given the current state and day.
00374:         /// An echo quest fires when: its trigger quest was resolved with the
00375:         /// matching choice, enough days have passed, it hasn't fired yet, and
00376:         /// its branch (if any) is not locked.
00377:         /// </summary>
00378:         public List<MoralEchoQuestDefinition> FindAvailableEchoQuests(int currentDay)
00379:         {
00380:             if (_chainData == null) return new List<MoralEchoQuestDefinition>();
00381:
00382:             var result = new List<MoralEchoQuestDefinition>();
00383:             foreach (var echo in _chainData.EchoQuests)
00384:             {
00385:                 if (_state.firedEchoQuests.Contains(echo.QuestId)) continue;
00386:                 if (!TryGetResolution(echo.TriggeredBy, out var trigger)) continue;
00387:                 if (trigger!.choiceIndex != echo.TriggeredByChoice) continue;
00388:                 if (currentDay < trigger.resolvedDay + echo.MinDaysAfter) continue;
00389:
00390:                 if (!string.IsNullOrEmpty(echo.Branch) && IsBranchLocked(echo.Branch)) continue;
00391:
00392:                 result.Add(echo);
00393:             }
00394:             return result;
00395:         }
00396:
00397:         /// <summary>Mark an echo quest as fired (called by the host when the echo is presented).</summary>
00398:         public void MarkEchoQuestFired(string echoQuestId)
00399:         {
00400:             if (string.IsNullOrEmpty(echoQuestId)) return;
00401:             if (!_state.firedEchoQuests.Contains(echoQuestId))
00402:                 _state.firedEchoQuests.Add(echoQuestId);
00403:         }
00404:
00405:         // ── Quest resolution ───────────────────────────────────────────
00406:
00407:         /// <summary>
00408:         /// Resolve a quest choice. One resolution per quest per save: repeat
00409:         /// calls return the stored resolution without re-applying deltas or
00410:         /// re-rolling. Band-crossing consequences never land here — they
00411:         /// settle overnight in Reconcile.
00412:         /// </summary>
00413:         public MoralChoiceResolution Resolve(MoralChoiceQuestDefinition quest, int choiceIndex, string locationId, int day)
00414:         {
00415:             if (quest == null) throw new ArgumentNullException(nameof(quest));
00416:             if (!_catalog.ContainsKey(quest.Id)) _catalog[quest.Id] = quest;
00417:             if (!IsCanonicalQuestId(quest.Id))
00418:             {
00419:                 throw new ArgumentException(
00420:                     $"Moral quest id '{quest.Id}' must use the canonical '{QuestIdPrefix}' prefix " +
00421:                     "(the design doc drafts ids as 'qst_moral_*'; register them as 'quest_moral_*').",
00422:                     nameof(quest));
00423:             }
00424:             if (choiceIndex < 0 || choiceIndex >= quest.Choices.Count)
00425:             {
00426:                 throw new ArgumentOutOfRangeException(nameof(choiceIndex),
00427:                     $"Choice index {choiceIndex} is outside 0..{quest.Choices.Count - 1} for '{quest.Id}'.");
00428:             }
00429:             if (day < 0) throw new ArgumentOutOfRangeException(nameof(day));
00430:
00431:             if (TryGetResolution(quest.Id, out var existing))
00432:             {
00433:                 _log.Warn($"Moral quest '{quest.Id}' already resolved on day {existing!.resolvedDay}; replaying stored outcome.");
00434:                 return existing;
00435:             }
00436:
00437:             var choice = quest.Choices[choiceIndex];
00438:             int unclamped = _state.moralScore + choice.MoralDelta;
00439:             int clamped = Math.Clamp(unclamped, MinScore, MaxScore);
00440:             _state.moralScore = clamped;
00441:             _state.empathyPoints += choice.EmpathyDelta;
00442:
00443:             var resolution = new MoralChoiceResolution
00444:             {
00445:                 questId = quest.Id,
00446:                 locationId = locationId ?? string.Empty,
00447:                 resolvedDay = day,
00448:                 choiceIndex = choiceIndex,
00449:                 moralDelta = choice.MoralDelta,
00450:                 empathyDelta = choice.EmpathyDelta,
00451:                 impactMark = MarkFor(choice.MoralDelta),
00452:                 outcomeRoll = _rng.Next(0, 100),
00453:                 propagatesOnDay = day + 1 + _rng.Next(0, 3),
00454:                 epitaph = choice.Epitaph
00455:             };
00456:             _state.resolutions.Add(resolution);
00457:
00458:             // Historical moral memory is part of the committed resolution.
00459:             // SetFlag is idempotent, and the existing save state remains the
00460:             // sole persistence authority for the resulting active flag set.
00461:             if (!string.IsNullOrEmpty(choice.SetFlag))
00462:                 SetFlag(choice.SetFlag);
00463:
00464:             OnQuestResolved?.Invoke(resolution);
00465:
00466:             // Overflow never lands mid-scene: flag it, settle it overnight.
00467:             if (unclamped > MaxScore) _state.pendingLegendFlags |= LegendPositiveFlag;
00468:             else if (unclamped < MinScore) _state.pendingLegendFlags |= LegendNegativeFlag;
00469:
00470:             // Track branch progress for entry quests and check lockout.
00471:             TrackBranchProgress(quest.Id);
00472:
00473:             return resolution;
00474:         }
00475:
00476:         /// <summary>
00477:         /// If the resolved quest is a branch entry quest, increment that
00478:         /// branch's progress. When the lock threshold is reached, lock out
00479:         /// the opposing branches and set the branch-locked flags.
00480:         /// </summary>
00481:         private void TrackBranchProgress(string questId)
00482:         {
00483:             if (!_entryQuestSet.Contains(questId)) return;
00484:             if (!_questToBranch.TryGetValue(questId, out var branchId)) return;
00485:             if (_chainData == null) return;
00486:
00487:             var branch = _chainData.Branches.FirstOrDefault(
00488:                 b => string.Equals(b.Id, branchId, StringComparison.Ordinal));
00489:             if (branch == null) return;
00490:
00491:             if (!_state.branchProgress.ContainsKey(branchId))
00492:                 _state.branchProgress[branchId] = 0;
00493:             _state.branchProgress[branchId]++;
00494:
00495:             if (_state.branchProgress[branchId] >= branch.LockThreshold)
00496:             {
00497:                 foreach (var lockedId in branch.LocksOut)
00498:                 {
00499:                     if (!_state.lockedBranches.Contains(lockedId))
00500:                     {
00501:                         _state.lockedBranches.Add(lockedId);
00502:
00503:                         var lockedBranch = _chainData.Branches.FirstOrDefault(
00504:                             b => string.Equals(b.Id, lockedId, StringComparison.Ordinal));
00505:                         if (lockedBranch != null && !string.IsNullOrEmpty(lockedBranch.LockedFlag))
00506:                         {
00507:                             SetFlag(lockedBranch.LockedFlag);
00508:                         }
00509:
00510:                         OnBranchLocked?.Invoke(lockedId);
00511:                     }
00512:                 }
00513:             }
00514:         }
00515:
00516:         /// <summary>
00517:         /// Overnight settlement: pending legend overflow, then band crossings
00518:         /// and their one-time faction events, so an act's consequences always
00519:         /// land overnight, never mid-scene. Every band crossed between the
00520:         /// last reconcile and now settles its event (dedup keeps each
00521:         /// one-time). Out-of-order days are ignored. A never-reconciled save
00522:         /// starts from the Neutral band.
00523:         /// </summary>
00524:         public void Reconcile(int day)
00525:         {
00526:             if (day < _state.lastReconciledDay) return;
00527:             _state.lastReconciledDay = day;
00528:
00529:             if ((_state.pendingLegendFlags & LegendPositiveFlag) != 0) FireThresholdEvent(EventLegendPositive);
00530:             if ((_state.pendingLegendFlags & LegendNegativeFlag) != 0) FireThresholdEvent(EventLegendNegative);
00531:             _state.pendingLegendFlags = 0;
00532:
00533:             int from = _state.bandAtLastReconcile < 0 ? (int)MoralPathBand.Neutral : _state.bandAtLastReconcile;
00534:             int to = (int)CurrentBand;
00535:             if (to == from) return;
00536:
00537:             int step = to > from ? 1 : -1;
00538:             for (int band = from + step; ; band += step)
00539:             {
00540:                 FireBandEvents((MoralPathBand)band);
00541:                 if (band == to) break;
00542:             }
00543:             _state.bandAtLastReconcile = to;
00544:         }
00545:
00546:         private void FireBandEvents(MoralPathBand band)
00547:         {
00548:             switch (band)
00549:             {
00550:                 case MoralPathBand.VeryEvil:
00551:                     FireThresholdEvent(EventBountyIssued);
00552:                     break;
00553:                 case MoralPathBand.Positive:
00554:                     FireThresholdEvent(EventContractTaken);
00555:                     break;
00556:                 case MoralPathBand.VeryPositive:
00557:                     FireThresholdEvent(EventContractRaised);
00558:                     FireThresholdEvent(EventPatrolDefense);
00559:                     break;
00560:             }
00561:         }
00562:
00563:         public MoralEndingKind SelectEnding() =>
00564:             SelectEnding(_state.moralScore, _state.empathyPoints, _state.resolutions.Count);
00565:
00566:         /// <summary>
00567:         /// Priority: Storykeeper threshold overrides band; below the quest
00568:         /// lock the mild endings fire (the band has not earned the right to
00569:         /// define the run yet); otherwise band decides.
00570:         /// </summary>
00571:         public static MoralEndingKind SelectEnding(int moralScore, int empathyPoints, int questsResolved)
00572:         {
00573:             if (empathyPoints >= StorykeeperEmpathyThreshold && questsResolved >= StorykeeperQuestThreshold)
00574:             {
00575:                 return MoralEndingKind.Storykeeper;
00576:             }
00577:
00578:             if (questsResolved < EndingLockMinQuests)
00579:             {
00580:                 return moralScore < 0 ? MoralEndingKind.NeutralSurvivor
00581:                     : moralScore == 0 ? MoralEndingKind.BalancedSurvivor
00582:                     : MoralEndingKind.CommunityBuilder;
00583:             }
00584:
00585:             return BandForScore(moralScore) switch
00586:             {
00587:                 MoralPathBand.VeryEvil => MoralEndingKind.Warlord,
00588:                 MoralPathBand.Evil => MoralEndingKind.SurvivorKing,
00589:                 MoralPathBand.SlightlyEvil => MoralEndingKind.NeutralSurvivor,
00590:                 MoralPathBand.Neutral => MoralEndingKind.BalancedSurvivor,
00591:                 MoralPathBand.SlightlyPositive => MoralEndingKind.CommunityBuilder,
00592:                 MoralPathBand.Positive => MoralEndingKind.Savior,
00593:                 _ => MoralEndingKind.SaintOfWasteland
00594:             };
00595:         }
00596:
00597:         public static MoralPathBand BandForScore(int score)
00598:         {
00599:             score = Math.Clamp(score, MinScore, MaxScore);
00600:             if (score <= -100) return MoralPathBand.VeryEvil;
00601:             if (score <= -50) return MoralPathBand.Evil;
00602:             if (score < 0) return MoralPathBand.SlightlyEvil;
00603:             if (score == 0) return MoralPathBand.Neutral;
00604:             if (score < 50) return MoralPathBand.SlightlyPositive;
00605:             if (score < 100) return MoralPathBand.Positive;
00606:             return MoralPathBand.VeryPositive;
00607:         }
00608:
00609:         public MoralChoiceState CaptureState() => Clone(_state);
00610:
00611:         public void RestoreState(MoralChoiceState state)
00612:         {
00613:             if (state == null) throw new ArgumentNullException(nameof(state));
00614:             if (!string.Equals(state.systemId, SystemId, StringComparison.Ordinal))
00615:             {
00616:                 throw new ArgumentException(
00617:                     $"State belongs to system '{state.systemId}', expected '{SystemId}'.", nameof(state));
00618:             }
00619:             if (state.schemaVersion > 1)
00620:             {
00621:                 throw new NotSupportedException(
00622:                     $"Future moral choice save schema {state.schemaVersion}; supported schema is 1.");
00623:             }
00624:             if (state.schemaVersion < 1)
00625:             {
00626:                 throw new ArgumentException("Moral choice save is missing a valid schemaVersion.", nameof(state));
00627:             }
00628:             _state = Clone(state);
00629:             if (_flags != null && _state.activeFlags != null)
00630:             {
00631:                 foreach (var f in _state.activeFlags)
00632:                     _flags.Set(f, "moral_choice");
00633:             }
00634:         }
00635:
00636:         private void FireThresholdEvent(string eventId)
00637:         {
00638:             if (_state.firedThresholdEvents.Contains(eventId)) return;
00639:             _state.firedThresholdEvents.Add(eventId);
00640:             OnThresholdEventFired?.Invoke(eventId);
00641:         }
00642:
00643:         private static string MarkFor(int moralDelta) =>
00644:             moralDelta > 0 ? "up" : moralDelta < 0 ? "down" : "flat";
00645:
00646:         /// <summary>Deep copy so captured/restored states never alias the live ledger.</summary>
00647:         private static MoralChoiceState Clone(MoralChoiceState source)
00648:         {
00649:             var copy = new MoralChoiceState
00650:             {
00651:                 systemId = source.systemId,
00652:                 schemaVersion = source.schemaVersion,
00653:                 moralScore = source.moralScore,
00654:                 empathyPoints = source.empathyPoints,
00655:                 lastReconciledDay = source.lastReconciledDay,
00656:                 bandAtLastReconcile = source.bandAtLastReconcile,
00657:                 pendingLegendFlags = source.pendingLegendFlags,
00658:                 firedThresholdEvents = new List<string>(source.firedThresholdEvents ?? new List<string>()),
00659:                 resolutions = new List<MoralChoiceResolution>(),
00660:                 branchProgress = new Dictionary<string, int>(source.branchProgress ?? new Dictionary<string, int>()),
00661:                 lockedBranches = new List<string>(source.lockedBranches ?? new List<string>()),
00662:                 firedEchoQuests = new List<string>(source.firedEchoQuests ?? new List<string>()),
00663:                 activeFlags = new List<string>(source.activeFlags ?? new List<string>())
00664:             };
00665:             if (source.resolutions != null)
00666:             {
00667:                 foreach (var r in source.resolutions)
00668:                 {
00669:                     copy.resolutions.Add(new MoralChoiceResolution
00670:                     {
00671:                         questId = r.questId,
00672:                         locationId = r.locationId,
00673:                         resolvedDay = r.resolvedDay,
00674:                         choiceIndex = r.choiceIndex,
00675:                         moralDelta = r.moralDelta,
00676:                         empathyDelta = r.empathyDelta,
00677:                         impactMark = r.impactMark,
00678:                         outcomeRoll = r.outcomeRoll,
00679:                         propagatesOnDay = r.propagatesOnDay,
00680:                         epitaph = r.epitaph
00681:                     });
00682:                 }
00683:             }
00684:             return copy;
00685:         }
00686:     }
00687: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagDefinitions.cs`

### `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagDefinitions.cs` — complete current file

- Size: 20 lines / 634 bytes.
- SHA-256: `764eda5eb3dce4983316d0a4b40463a539e49aa5ff7cd4a1031e0f733be970de`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003:
00004: namespace Ashfall.Core.MoralChoice
00005: {
00006:     /// <summary>
00007:     /// Wire shape for moral_choice_flags.json — persistent moral-history
00008:     /// definitions used by quest access, branch locking, and later predicates.
00009:     /// </summary>
00010:     public sealed class MoralChoiceFlagDefinitions
00011:     {
00012:         public List<MoralFlagDefinition> Flags { get; set; } = new List<MoralFlagDefinition>();
00013:     }
00014:
00015:     public sealed class MoralFlagDefinition
00016:     {
00017:         public string Id { get; set; } = string.Empty;
00018:         public string DisplayName { get; set; } = string.Empty;
00019:     }
00020: }
```


# Appendix — Current Source Detail: `src/UI/MoralChoiceModal.cs`

### `src/UI/MoralChoiceModal.cs` — complete current file

- Size: 317 lines / 14077 bytes.
- SHA-256: `ef9e9b6e34bb952eb6ac37133cccd919dfc0ef4135b4831ff0085ff6662da334`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006: using Ashfall.Core.MoralChoice;
00007: using Ashfall.Core.UI;
00008: using DesignTheme = Ashfall.Core.UI.Theme;
00009:
00010: namespace AtomicWar.GodotApp.UI
00011: {
00012:     /// <summary>
00013:     /// ASHFALL — Moral Choice Decision Modal ("The Weight of Survival").
00014:     /// Surfaces authored moral dilemmas, narrative encounters, and irrevocable tactical options
00015:     /// to the player without exposing underlying numeric moral/empathy metrics.
00016:     /// Implements <see cref="IModalPanel"/> for focus management and keyboard handling.
00017:     /// </summary>
00018:     public partial class MoralChoiceModal : Control, IModalPanel
00019:     {
00020:         public event Action<string, int>? OnChoiceSelected;
00021:         public event Action? OnClose;
00022:         public event Action? OnModalClosed;
00023:
00024:         public bool IsModalOpen => Visible;
00025:         public Control? InitialFocusControl => _firstInteractiveButton ?? _closeButton;
00026:
00027:         private Label _titleLabel = null!;
00028:         private Label _subtitleLabel = null!;
00029:         private VBoxContainer _encounterContainer = null!;
00030:         private VBoxContainer _choicesContainer = null!;
00031:         private VBoxContainer _feedbackContainer = null!;
00032:         private Button _closeButton = null!;
00033:         private Control? _firstInteractiveButton;
00034:
00035:         private MoralChoiceQuestDefinition? _currentQuest;
00036:         private MoralChoiceSystem? _moralChoiceSystem;
00037:         private Action<string, int>? _onChoiceCallback;
00038:
00039:         public override void _Ready()
00040:         {
00041:             SetAnchorsPreset(LayoutPreset.FullRect);
00042:             BuildLayout();
00043:             Visible = false;
00044:         }
00045:
00046:         private void BuildLayout()
00047:         {
00048:             AshfallUiHelpers.EmptyChildren(this);
00049:
00050:             // Dark semi-transparent scrim backdrop
00051:             var scrim = new ColorRect
00052:             {
00053:                 Color = new Color(0.02f, 0.02f, 0.04f, 0.88f)
00054:             };
00055:             scrim.SetAnchorsPreset(LayoutPreset.FullRect);
00056:             AddChild(scrim);
00057:
00058:             // Center dialog container (max width 1100, centered)
00059:             var center = new CenterContainer();
00060:             center.SetAnchorsPreset(LayoutPreset.FullRect);
00061:             AddChild(center);
00062:
00063:             var panelCard = AshfallUiHelpers.MakeCardFrame("THE WEIGHT OF SURVIVAL", "ETHICAL DIRECTIVE & TACTICAL CHOICE");
00064:             panelCard.CustomMinimumSize = new Vector2(1040, 680);
00065:             center.AddChild(panelCard);
00066:
00067:             var margin = panelCard.GetChild<MarginContainer>(0);
00068:             var mainVBox = margin.GetChild<VBoxContainer>(0);
00069:
00070:             // Title & category header
00071:             _titleLabel = AshfallUiHelpers.MakeTitle("MORAL DILEMMA // UNRESOLVED");
00072:             _titleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
00073:             mainVBox.AddChild(_titleLabel);
00074:
00075:             _subtitleLabel = AshfallUiHelpers.MakeSmall("CATEGORY: UNKNOWN · LOCATION: GENERAL SECTOR");
00076:             _subtitleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
00077:             mainVBox.AddChild(_subtitleLabel);
00078:
00079:             mainVBox.AddChild(AshfallUiHelpers.MakeSeparator());
00080:
00081:             // Scrollable central content
00082:             var scroll = new ScrollContainer
00083:             {
00084:                 CustomMinimumSize = new Vector2(980, 440),
00085:                 SizeFlagsVertical = SizeFlags.ExpandFill,
00086:                 HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
00087:             };
00088:             mainVBox.AddChild(scroll);
00089:
00090:             var scrollContent = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
00091:             scrollContent.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00092:             scroll.AddChild(scrollContent);
00093:
00094:             _encounterContainer = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
00095:             scrollContent.AddChild(_encounterContainer);
00096:
00097:             _choicesContainer = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
00098:             scrollContent.AddChild(_choicesContainer);
00099:
00100:             _feedbackContainer = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
00101:             scrollContent.AddChild(_feedbackContainer);
00102:
00103:             mainVBox.AddChild(AshfallUiHelpers.MakeSeparator());
00104:
00105:             // Bottom bar with close/return
00106:             var bottomBar = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingMd);
00107:             bottomBar.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00108:
00109:             _closeButton = AshfallUiHelpers.MakeButton("RETURN TO OVERVIEW // [ESC]", () => CloseModal());
00110:             _closeButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00111:             bottomBar.AddChild(_closeButton);
00112:
00113:             mainVBox.AddChild(bottomBar);
00114:         }
00115:
00116:         public void Bind(
00117:             MoralChoiceQuestDefinition quest,
00118:             MoralChoiceSystem? moralChoiceSystem = null,
00119:             Action<string, int>? onChoiceCallback = null)
00120:         {
00121:             _currentQuest = quest ?? throw new ArgumentNullException(nameof(quest));
00122:             _moralChoiceSystem = moralChoiceSystem;
00123:             _onChoiceCallback = onChoiceCallback;
00124:             _firstInteractiveButton = null;
00125:
00126:             RefreshContent();
00127:         }
00128:
00129:         public void RefreshContent()
00130:         {
00131:             if (_currentQuest == null) return;
00132:
00133:             // The choice buttons are rebuilt on every refresh. Clear the
00134:             // cached focus target before freeing the old button tree.
00135:             _firstInteractiveButton = null;
00136:
00137:             bool isResolved = _moralChoiceSystem?.IsResolved(_currentQuest.Id) ?? false;
00138:             MoralChoiceResolution? resolution = null;
00139:             _moralChoiceSystem?.TryGetResolution(_currentQuest.Id, out resolution);
00140:
00141:             // Header titles
00142:             string statusTag = isResolved ? "RESOLVED & RECORDED" : "TACTICAL ACTION REQUIRED";
00143:             _titleLabel.Text = $"ETHICAL PROTOCOL // {_currentQuest.DisplayName.ToUpperInvariant()}";
00144:             _subtitleLabel.Text = $"CATEGORY: {_currentQuest.Category.ToUpperInvariant()} · STATUS: {statusTag} · LOCATION: {(string.IsNullOrEmpty(_currentQuest.LocationId) ? "SECTOR PERIMETER" : _currentQuest.LocationId)}";
00145:
00146:             // 1. Encounter / Narrative briefing
00147:             AshfallUiHelpers.EmptyChildren(_encounterContainer);
00148:             var encounterCard = AshfallUiHelpers.MakeCardFrame("FIELD ENCOUNTER DOSSIER", _currentQuest.Category.ToUpperInvariant());
00149:             var encBox = encounterCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00150:
00151:             if (!string.IsNullOrWhiteSpace(_currentQuest.Trigger))
00152:             {
00153:                 var trigLabel = AshfallUiHelpers.MakeBody($"► SITUATION: {_currentQuest.Trigger}");
00154:                 trigLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
00155:                 encBox.AddChild(trigLabel);
00156:                 encBox.AddChild(AshfallUiHelpers.MakeSeparator());
00157:             }
00158:
00159:             string encounterText = !string.IsNullOrWhiteSpace(_currentQuest.Discovery)
00160:                 ? _currentQuest.Discovery
00161:                 : "A critical dilemma confronts the shelter cohort. Survival calculations require immediate leadership action.";
00162:
00163:             var bodyLbl = AshfallUiHelpers.MakeBody(encounterText);
00164:             encBox.AddChild(bodyLbl);
00165:             _encounterContainer.AddChild(encounterCard);
00166:
00167:             // 2. Choices section
00168:             AshfallUiHelpers.EmptyChildren(_choicesContainer);
00169:             var choicesCard = AshfallUiHelpers.MakeCardFrame("AUTHORITATIVE DECISION GATES", isResolved ? "RESOLUTION RECORDED" : "SELECT ACTION");
00170:             var chBox = choicesCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00171:
00172:             if (!isResolved)
00173:             {
00174:                 var warnNotice = AshfallUiHelpers.MakeSmall("ATTENTION: Ethical choices permanently alter survivor morale, camp chatter, and regional branch viability. Once committed, a choice cannot be rescinded.");
00175:                 warnNotice.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warning));
00176:                 chBox.AddChild(warnNotice);
00177:                 chBox.AddChild(AshfallUiHelpers.MakeSeparator());
00178:             }
00179:
00180:             for (int i = 0; i < _currentQuest.Choices.Count; i++)
00181:             {
00182:                 int choiceIndex = i;
00183:                 var opt = _currentQuest.Choices[i];
00184:                 bool wasChosen = isResolved && resolution != null && resolution.choiceIndex == i;
00185:
00186:                 var optBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
00187:
00188:                 if (isResolved)
00189:                 {
00190:                     if (wasChosen)
00191:                     {
00192:                         var row = AshfallUiHelpers.MakeDataRow($"[COMMITTED RESOLUTION] Option {i + 1}", opt.Label, AshfallUiHelpers.ToColor(DesignTheme.Hot));
00193:                         optBox.AddChild(row);
00194:
00195:                         if (!string.IsNullOrEmpty(opt.OutcomeText))
00196:                         {
00197:                             var outLbl = AshfallUiHelpers.MakeSmall($"Consequence: {opt.OutcomeText}");
00198:                             outLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
00199:                             optBox.AddChild(outLbl);
00200:                         }
00201:
00202:                         if (!string.IsNullOrEmpty(opt.Epitaph))
00203:                         {
00204:                             var epiLbl = AshfallUiHelpers.MakeSmall($"Camp Chronicle: \"{opt.Epitaph}\"");
00205:                             epiLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
00206:                             optBox.AddChild(epiLbl);
00207:                         }
00208:                     }
00209:                     else
00210:                     {
00211:                         var row = AshfallUiHelpers.MakeDataRow($"[UNSELECTED] Option {i + 1}", opt.Label, AshfallUiHelpers.ToColor(DesignTheme.Dim));
00212:                         optBox.AddChild(row);
00213:                     }
00214:                 }
00215:                 else
00216:                 {
00217:                     // Active unresolved option: interactive button without exposing numeric scores
00218:                     var btn = AshfallUiHelpers.MakeButton($"[{i + 1}] COMMIT PATH // {opt.Label.ToUpperInvariant()}", () =>
00219:                     {
00220:                         ExecuteChoice(choiceIndex);
00221:                     });
00222:                     btn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00223:                     optBox.AddChild(btn);
00224:
00225:                     if (_firstInteractiveButton == null)
00226:                         _firstInteractiveButton = btn;
00227:                 }
00228:
00229:                 chBox.AddChild(optBox);
00230:                 if (i < _currentQuest.Choices.Count - 1)
00231:                     chBox.AddChild(AshfallUiHelpers.MakeSeparator());
00232:             }
00233:
00234:             _choicesContainer.AddChild(choicesCard);
00235:
00236:             // 3. Feedback / consequence strip
00237:             AshfallUiHelpers.EmptyChildren(_feedbackContainer);
00238:             if (isResolved && resolution != null)
00239:             {
00240:                 var fbCard = AshfallUiHelpers.MakeCardFrame("RESOLUTION ARCHIVE & CONSEQUENCE RECORD", $"RESOLVED DAY {resolution.resolvedDay}");
00241:                 var fbBox = fbCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00242:
00243:                 string arrow = resolution.impactMark == "up" ? "🔺 Positive Social Trajectory"
00244:                     : resolution.impactMark == "down" ? "🔻 Hardened Survival Stance" : "⚪ Neutral Pragmatic Shift";
00245:
00246:                 fbBox.AddChild(AshfallUiHelpers.MakeDataRow("Moral Resonance", arrow, AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00247:                 fbBox.AddChild(AshfallUiHelpers.MakeDataRow("Camp Record", resolution.epitaph, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
00248:                 fbBox.AddChild(AshfallUiHelpers.MakeDataRow("Journal Status", "Archived to permanent Holdfast survival chronicle.", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
00249:
00250:                 _feedbackContainer.AddChild(fbCard);
00251:             }
00252:         }
00253:
00254:         private void ExecuteChoice(int choiceIndex)
00255:         {
00256:             if (_currentQuest == null) return;
00257:             string questId = _currentQuest.Id;
00258:
00259:             // Prefer Bind callback when present so host paths cannot double-resolve
00260:             // via both OnChoiceSelected and the Bind delegate.
00261:             if (_onChoiceCallback != null)
00262:                 _onChoiceCallback.Invoke(questId, choiceIndex);
00263:             else
00264:                 OnChoiceSelected?.Invoke(questId, choiceIndex);
00265:
00266:             // Re-render in place
00267:             RefreshContent();
00268:         }
00269:
00270:         public void Open()
00271:         {
00272:             Visible = true;
00273:             RefreshContent();
00274:             _firstInteractiveButton?.GrabFocus();
00275:         }
00276:
00277:         public void SelectChoiceForTest(int choiceIndex) => ExecuteChoice(choiceIndex);
00278:
00279:         public void CloseModal()
00280:         {
00281:             Visible = false;
00282:             OnModalClosed?.Invoke();
00283:             OnClose?.Invoke();
00284:         }
00285:
00286:         public override void _UnhandledInput(InputEvent @event)
00287:         {
00288:             if (!Visible) return;
00289:
00290:             if (@event is InputEventKey key && key.Pressed)
00291:             {
00292:                 if (key.Keycode == Key.Escape)
00293:                 {
00294:                     CloseModal();
00295:                     GetViewport().SetInputAsHandled();
00296:                     return;
00297:                 }
00298:
00299:                 // Keyboard quick-selection for options 1-9 if unresolved
00300:                 if (_currentQuest != null && (_moralChoiceSystem == null || !_moralChoiceSystem.IsResolved(_currentQuest.Id)))
00301:                 {
00302:                     int number = -1;
00303:                     if (key.Keycode >= Key.Key1 && key.Keycode <= Key.Key9)
00304:                         number = (int)(key.Keycode - Key.Key1);
00305:                     else if (key.Keycode >= Key.Kp1 && key.Keycode <= Key.Kp9)
00306:                         number = (int)(key.Keycode - Key.Kp1);
00307:
00308:                     if (number >= 0 && number < _currentQuest.Choices.Count)
00309:                     {
00310:                         ExecuteChoice(number);
00311:                         GetViewport().SetInputAsHandled();
00312:                     }
00313:                 }
00314:             }
00315:         }
00316:     }
00317: }
```


# Appendix — Current Source Detail: `src/Main.Application.cs`

### `src/Main.Application.cs` — bounded current excerpt (1131 of 1144 lines)

- Size: 1144 lines / 55746 bytes.
- SHA-256: `ea35c17ff8c236beead0d68584cde85f606db646aa0072a8ca203c74cb1893ed`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using Godot;
00003: using System;
00004: using System.Globalization;
00005: using System.IO;
00006: using System.Linq;
00007: using System.Collections.Generic;
00008: using AtomicWar.Journal;
00009: using AtomicWar.GodotApp.Host;
00010: using Ashfall.Core;
00011: using Ashfall.Core.Campaign;
00012: using Ashfall.Core.Economy;
00013: using Ashfall.Core.Expeditions;
00014: using Ashfall.Core.Foundry;
00015: using Ashfall.Core.IO;
00016: using Ashfall.Core.Inventory;
00017: using Ashfall.Core.Journal;
00018: using Ashfall.Core.Muster;
00019: using Ashfall.Core.YearOfAsh;
00020: using Ashfall.Core.Radio;
00021: using Ashfall.Core.Survivors;
00022: using AtomicWar.GodotApp.Economy;
00023: using AtomicWar.GodotApp.YearOfAsh;
00024: using AtomicWar.GodotApp.Muster;
00025: using AtomicWar.GodotApp.Dose;
00026: using AtomicWar.GodotApp.UtilityAI;
00027: using AtomicWar.GodotApp.Radio;
00028: using AtomicWar.GodotApp.Audio;
00029: using AtomicWar.GodotApp.UI;
00030:
00031: namespace AtomicWar.GodotApp
00032: {
00033:     public partial class Main : Control
00034:     {
00035:         public override void _Ready()
00036:         {
00037:             GD.Print("[Ashfall Godot] Initializing ASHFALL: Atomic War - Starving Survival...");
00038:
00039:             // Register all player-navigable panel descriptors before any navigation occurs.
00040:             Ashfall.Core.UI.PanelRegistryBootstrap.RegisterAll();
00041:
00042:             // Parse first so --user-data-dir and other host overrides are in
00043:             // place before settings and mod discovery resolve their roots.
00044:             var cliAction = HostCli.Parse(OS.GetCmdlineUserArgs());
00045:
00046:             ResolveDataDir();
00047:             _cliStartingSuppliesProfileId =
00048:                 HostCli.ExtractArgValue(
00049:                     OS.GetCmdlineUserArgs(),
00050:                     "--starting-supplies-profile");
00051:             _startingSuppliesProfileId =
00052:                 _cliStartingSuppliesProfileId ??
00053:                 StartingSuppliesCatalog.StandardProfileId;
00054:
00055:             // Validate required catalogs before any systems are initialized.
00056:             // This ensures the game cannot start with missing or malformed required data.
00057:             ValidateRequiredCatalogs();
00058:
00059:             // Parse once and keep the action: the catch below needs to name the
00060:             // gate that threw, and re-parsing there could disagree with what ran.
00061:             // Every CLI self-test runs inside this guard. Without it an exception
00062:             // thrown by a gate escapes _Ready(), skipping GetTree().Quit(), so the
00063:             // process emits no FAIL line and hangs until CI kills it — while any
00064:             // PASS already printed stays on stdout as the apparent verdict.
00065:             // A throw must always become a reported FAIL with a non-zero exit.
00066:             try
00067:             {
00068:             switch (cliAction)
00069:             {
00070:                 case HostCliAction.Help:
00071:                     HostCli.PrintHelp();
00072:                     GetTree().Quit(0);
00073:                     return;
00074:                 case HostCliAction.Version:
00075:                     HostCli.PrintVersion(_dataDir);
00076:                     GetTree().Quit(0);
00077:                     return;
00078:                 case HostCliAction.ExpansionsSelfTest:
00079:                     GetTree().Quit(HostCli.RunExpansionsSelfTest(_dataDir));
00080:                     return;
00081:                 case HostCliAction.HoldfastSelfTest:
00082:                     GetTree().Quit(HostCli.RunHoldfastSelfTest(_dataDir));
00083:                     return;
00084:                 case HostCliAction.DutyRosterSelfTest:
00085:                     GetTree().Quit(HostCli.RunDutyRosterSelfTest(_dataDir));
00086:                     return;
00087:                 case HostCliAction.StandingRecordSelfTest:
00088:                     GetTree().Quit(HostCli.RunStandingRecordSelfTest(_dataDir));
00089:                     return;
00090:                 case HostCliAction.CrossingSelfTest:
00091:                     GetTree().Quit(HostCli.RunCrossingSelfTest(_dataDir));
00092:                     return;
00093:                 case HostCliAction.ArbitrationSelfTest:
00094:                     GetTree().Quit(HostCli.RunArbitrationSelfTest());
00095:                     return;
00096:                 case HostCliAction.LedgerDebtSelfTest:
00097:                     GetTree().Quit(HostCli.RunLedgerDebtSelfTest());
00098:                     return;
00099:                 case HostCliAction.GreenhouseSelfTest:
00100:                     GetTree().Quit(HostCli.RunGreenhouseSelfTest());
00101:                     return;
00102:                 case HostCliAction.AgricultureSelfTest:
00103:                     GetTree().Quit(HostCli.RunAgricultureSelfTest(_dataDir));
00104:                     return;
00105:                 case HostCliAction.OrphanSealWave1SelfTest:
00106:                     GetTree().Quit(HostCli.RunOrphanSealWave1SelfTest(_dataDir));
00107:                     return;
00108:                 case HostCliAction.CommitmentsSelfTest:
00109:                     GetTree().Quit(HostCli.RunCommitmentsSelfTest(_dataDir));
00110:                     return;
00111:                 case HostCliAction.SessionDurabilitySelfTest:
00112:                     GetTree().Quit(HostCli.RunSessionDurabilitySelfTest(_dataDir));
00113:                     return;
00114:                 case HostCliAction.PlayMetricsSelfTest:
00115:                     GetTree().Quit(HostCli.RunPlayMetricsSelfTest(_dataDir));
00116:                     return;
00117:                 case HostCliAction.SurvivorVoiceSelfTest:
00118:                     GetTree().Quit(HostCli.RunSurvivorVoiceSelfTest(_dataDir));
00119:                     return;
00120:                 case HostCliAction.ContentCertificationSelfTest:
00121:                     GetTree().Quit(HostCli.RunContentCertificationSelfTest(_dataDir));
00122:                     return;
00123:                 case HostCliAction.HoldfastPresentationSelfTest:
00124:                     GetTree().Quit(HostCli.RunHoldfastPresentationSelfTest(_dataDir));
00125:                     return;
00126:                 case HostCliAction.ScarcityAudioSelfTest:
00127:                     GetTree().Quit(HostCli.RunScarcityAudioSelfTest(_dataDir));
00128:                     return;
00129:                 case HostCliAction.SliceScenarioSelfTest:
00130:                     GetTree().Quit(HostCli.RunSliceScenarioSelfTest(_dataDir));
00131:                     return;
00132:                 case HostCliAction.RetentionSelfTest:
00133:                     GetTree().Quit(HostCli.RunRetentionSelfTest(_dataDir));
00134:                     return;
00135:                 case HostCliAction.OutpostSettlementSelfTest:
00136:                     GetTree().Quit(HostCli.RunOutpostSettlementSelfTest(_dataDir));
00137:                     return;
00138:                 case HostCliAction.WeatherCascadeSelfTest:
00139:                     GetTree().Quit(HostCli.RunWeatherCascadeSelfTest(_dataDir));
00140:                     return;
00141:                 case HostCliAction.StandingGatesSelfTest:
00142:                     GetTree().Quit(HostCli.RunStandingGatesSelfTest(
00143:                         _dataDir, ProjectSettings.GlobalizePath("res://")));
00144:                     return;
00145:                 case HostCliAction.TerritoryControlSelfTest:
00146:                     GetTree().Quit(HostCli.RunTerritoryControlSelfTest(_dataDir));
00147:                     return;
00148:                 case HostCliAction.CookingSelfTest:
00149:                     GetTree().Quit(HostCli.RunCookingSelfTest(_dataDir));
00150:                     return;
00151:                 case HostCliAction.NeedsPerformanceSelfTest:
00152:                     GetTree().Quit(HostCli.RunNeedsPerformanceSelfTest(_dataDir));
00153:                     return;
00154:                 case HostCliAction.CampaignLegacySelfTest:
00155:                     GetTree().Quit(HostCli.RunCampaignLegacySelfTest(_dataDir));
00156:                     return;
00157:                 case HostCliAction.DefenseSelfTest:
00158:                     GetTree().Quit(HostCli.RunDefenseSelfTest(_dataDir));
00159:                     return;
00160:                 case HostCliAction.PsychologySelfTest:
00161:                     GetTree().Quit(HostCli.RunPsychologySelfTest(_dataDir));
00162:                     return;
00163:                 case HostCliAction.WildlifeSelfTest:
00164:                     GetTree().Quit(HostCli.RunWildlifeSelfTest(_dataDir));
00165:                     return;
00166:                 case HostCliAction.TrappingHostSelfTest:
00167:                     GetTree().Quit(HostCli.RunTrappingHostSelfTest(_dataDir));
00168:                     return;
00169:                 case HostCliAction.PrecisionMetrologySelfTest:
00170:                     GetTree().Quit(HostCli.RunPrecisionMetrologySelfTest(_dataDir));
00171:                     return;
00172:                 case HostCliAction.DirectionFindingSelfTest:
00173:                     GetTree().Quit(HostCli.RunDirectionFindingSelfTest(_dataDir));
00174:                     return;
00175:                 case HostCliAction.AquaponicsSelfTest:
00176:                     GetTree().Quit(HostCli.RunAquaponicsSelfTest(_dataDir));
00177:                     return;
00178:                 case HostCliAction.CombatBreachingSelfTest:
00179:                     GetTree().Quit(HostCli.RunCombatBreachingSelfTest(_dataDir));
00180:                     return;
00181:                 case HostCliAction.SilentFoundrySelfTest:
00182:                     GetTree().Quit(HostCli.RunSilentFoundrySelfTest(_dataDir));
00183:                     return;
00184:                 case HostCliAction.DiseaseSelfTest:
00185:                     GetTree().Quit(HostCli.RunDiseaseSelfTest(_dataDir));
00186:                     return;
00187:                 case HostCliAction.DifficultySelfTest:
00188:                     GetTree().Quit(HostCli.RunDifficultySelfTest(_dataDir));
00189:                     return;
00190:                 case HostCliAction.JournalSaveSelfTest:
00191:                     GetTree().Quit(HostCli.RunJournalSaveSelfTest());
00192:                     return;
00193:                 case HostCliAction.MoralChoiceSelfTest:
00194:                     GetTree().Quit(HostCli.RunMoralChoiceSelfTest(_dataDir));
00195:                     return;
00196:                 case HostCliAction.EvolvingWorldSelfTest:
00197:                     GetTree().Quit(HostCli.RunEvolvingWorldSelfTest(_dataDir));
00198:                     return;
00199:                 case HostCliAction.WorldPlaytestSelfTest:
00200:                     GetTree().Quit(HostCli.RunWorldPlaytestSelfTest(_dataDir, this));
00201:                     return;
00202:                 case HostCliAction.SyntheticLubricantSelfTest:
00203:                     GetTree().Quit(HostCli.RunSyntheticLubricantSelfTest(_dataDir));
00204:                     return;
00205:                 case HostCliAction.UvCoronaSelfTest:
00206:                     GetTree().Quit(HostCli.RunUvCoronaSelfTest(_dataDir));
00207:                     return;
00208:                 case HostCliAction.CarbonCompositeSelfTest:
00209:                     GetTree().Quit(HostCli.RunCarbonCompositeSelfTest(_dataDir));
00210:                     return;
00211:                 case HostCliAction.GprCartographySelfTest:
00212:                     GetTree().Quit(HostCli.RunGprCartographySelfTest(_dataDir));
00213:                     return;
00214:                 case HostCliAction.AdvancedIndustrialReconSelfTest:
00215:                     GetTree().Quit(HostCli.RunAdvancedIndustrialReconSelfTest(_dataDir));
00216:                     return;
00217:                 case HostCliAction.SelfTestManifest:
00218:                     GetTree().Quit(HostCli.RunSelfTestManifest(_dataDir));
00219:                     return;
00220:                 case HostCliAction.ListSelfTests:
00221:                     GetTree().Quit(HostCli.RunListSelfTests(_dataDir));
00222:                     return;
00223:                 case HostCliAction.ChemicalDependencySaveSelfTest:
00224:                     GetTree().Quit(HostCli.RunChemicalDependencySaveSelfTest());
00225:                     return;
00226:                 case HostCliAction.ContrabandStashSelfTest:
00227:                     GetTree().Quit(HostCli.RunContrabandStashSelfTest());
00228:                     return;
00229:                 case HostCliAction.MedicalWardSaveSelfTest:
00230:                     GetTree().Quit(HostCli.RunMedicalWardSaveSelfTest());
00231:                     return;
00232:                 case HostCliAction.WeatherSaveSelfTest:
00233:                     GetTree().Quit(HostCli.RunWeatherSaveSelfTest());
00234:                     return;
00235:                 case HostCliAction.SaveLoadUiFailureSelfTest:
00236:                     GetTree().Quit(HostCli.RunSaveLoadUiFailureSelfTest(_dataDir));
00237:                     return;
00238:                 case HostCliAction.PanelBindLifecycleSelfTest:
00239:                     GetTree().Quit(HostCli.RunPanelBindLifecycleSelfTest(_dataDir));
00240:                     return;
00241:                 case HostCliAction.SaveStoreChecksumSelfTest:
00242:                     GetTree().Quit(HostCli.RunSaveStoreChecksumSelfTest(_dataDir));
00243:                     return;
00244:                 case HostCliAction.RuntimeScaleSelfTest:
00245:                     GetTree().Quit(HostCli.RunRuntimeScaleSelfTest(_dataDir));
00246:                     return;
00247:                 case HostCliAction.SevenDayDeterministicSmokeSelfTest:
00248:                     GetTree().Quit(HostCli.RunSevenDayDeterministicSmokeSelfTest(_dataDir));
00249:                     return;
00250:                 case HostCliAction.CampaignFuzzSelfTest:
00251:                     GetTree().Quit(HostCli.RunCampaignFuzzSelfTest(_dataDir));
00252:                     return;
00253:                 case HostCliAction.CompositionRootSelfTest:
00254:                     RunCompositionRootUiTestAndQuit();
00255:                     return;
00256:                 case HostCliAction.RealCampaignJourneySelfTest:
00257:                     RunRealCampaignJourneySelfTestAndQuit();
00258:                     return;
00259:                 case HostCliAction.StartingCohortLifecycleSelfTest:
00260:                     RunStartingCohortLifecycleSelfTestAndQuit();
00261:                     return;
00262:                 case HostCliAction.UiAccessibilitySelfTest:
00263:                     GetTree().Quit(HostCli.RunUiAccessibilitySelfTest());
00264:                     return;
00265:                 case HostCliAction.SceneBindingSelfTest:
00266:                     GetTree().Quit(SceneBindingSelfTest.Run());
00267:                     return;
00268:                 case HostCliAction.PortContractSelfTest:
00269:                     GetTree().Quit(PortContractSelfTest.Run(_dataDir));
00270:                     return;
00271:                 case HostCliAction.CombatSelfTest:
00272:                     GetTree().Quit(HostCli.RunCombatSelfTest(_dataDir));
00273:                     return;
00274:                 case HostCliAction.DeconAirlockUiTest:
00275:
00276:                     RunDeconAirlockUiTestAndQuit();
00277:
00278:                     return;
00279:
00280:                 case HostCliAction.WorkshopRelicUiTest:
00281:
00282:                     RunWorkshopRelicUiTestAndQuit();
00283:
00284:                     return;
00285:
00286:                 case HostCliAction.GeodeticSurveyUiTest:
00287:
00288:                     RunGeodeticSurveyUiTestAndQuit();
00289:
00290:                     return;
00291:
00292:                 case HostCliAction.KineticStorageUiTest:
00293:
00294:                     RunKineticStorageUiTestAndQuit();
00295:
00296:                     return;
00297:
00298:                 case HostCliAction.ChemicalReconUiTest:
00299:
00300:                     RunChemicalReconUiTestAndQuit();
00301:
00302:                     return;
00303:
00304:                 case HostCliAction.Plans198To201UiTest:
00305:
00306:                     RunPlans198To201UiTestAndQuit();
00307:
00308:                     return;
00309:
00310:                 case HostCliAction.EbPvdCoatingUiTest:
00311:
00312:                     RunEbPvdCoatingUiTestAndQuit();
00313:
00314:                     return;
00315:
00316:                 case HostCliAction.MicrofluidicDiagnosticUiTest:
00317:
00318:                     RunMicrofluidicDiagnosticUiTestAndQuit();
00319:
00320:                     return;
00321:
00322:                 case HostCliAction.MineFlailUiTest:
00323:
00324:                     RunMineFlailUiTestAndQuit();
00325:
00326:                     return;
00327:
00328:                 case HostCliAction.RailGrindingUiTest:
00329:
00330:                     RunRailGrindingUiTestAndQuit();
00331:
00332:                     return;
00333:                 case HostCliAction.GeothermalAquiferSelfTest:
00334:
00335:                     RunGeothermalAquiferUiTestAndQuit();
00336:
00337:                     return;
00338:                 case HostCliAction.ReconTelemetrySelfTest:
00339:
00340:                     RunReconTelemetryUiTestAndQuit();
00341:
00342:                     return;
00343:                 case HostCliAction.SilentFoundryUiTest:
00344:                     RunSilentFoundryUiTestAndQuit();
00345:                     return;
00346:                 case HostCliAction.DutyRosterUiTest:
00347:                     RunDutyRosterUiTestAndQuit();
00348:                     return;
00349:                 case HostCliAction.IceRoadSelfTest:
00350:                     GetTree().Quit(HostCli.RunIceRoadSelfTest(_dataDir));
00351:                     return;
00352:                 case HostCliAction.CensusSelfTest:
00353:                     GetTree().Quit(HostCli.RunCensusSelfTest());
00354:                     return;
00355:                 case HostCliAction.CoreSelfTest:
00356:                     GetTree().Quit(HostCli.RunCoreSelfTest(_dataDir));
00357:                     return;
00358:                 case HostCliAction.HoldfastBriefing:
00359:                     GetTree().Quit(HostCli.RunHoldfastBriefing(_dataDir));
00360:                     return;
00361:                 case HostCliAction.IceRoadTickDemo:
00362:                     GetTree().Quit(HostCli.RunIceRoadTickDemo(_dataDir));
00363:                     return;
00364:                 case HostCliAction.HoldfastSaveSelfTest:
00365:                     GetTree().Quit(HostCli.RunHoldfastSaveSelfTest(_dataDir));
00366:                     return;
00367:                 case HostCliAction.HoldfastTradeSaveSelfTest:
00368:                     GetTree().Quit(HoldfastTradeSaveStoreSelfTest.Run());
00369:                     return;
00370:                 case HostCliAction.HiddenAgendaSelfTest:
00371:                     GetTree().Quit(HiddenAgendaSelfTest.Run(_dataDir));
00372:                     return;
00373:                 case HostCliAction.ShelterReputationSelfTest:
00374:                     GetTree().Quit(ShelterReputationSelfTest.Run(_dataDir));
00375:                     return;
00376:                 case HostCliAction.HoldfastRuntimeUiTest:
00377:                     RunHoldfastRuntimeUiTestAndQuit();
00378:                     return;
00379:                 case HostCliAction.BrineSelfTest:
00380:                     GetTree().Quit(HostCli.RunBrineSelfTest());
00381:                     return;
00382:                 case HostCliAction.MusterSelfTest:
00383:                     GetTree().Quit(HostCli.RunMusterSelfTest());
00384:                     return;
00385:                 case HostCliAction.FactionEcologySelfTest:
00386:                     GetTree().Quit(HostCli.RunFactionEcologySelfTest(_dataDir));
00387:                     return;
00388:                 case HostCliAction.VerdictSelfTest:
00389:                     GetTree().Quit(HostCli.RunVerdictSelfTest(_dataDir));
00390:                     return;
00391:                 case HostCliAction.ClusterSelfTest:
00392:                     GetTree().Quit(HostCli.RunClusterSelfTest(_dataDir));
00393:                     return;
00394:                 case HostCliAction.EndingsSelfTest:
00395:                     GetTree().Quit(HostCli.RunEndingsSelfTest());
00396:                     return;
00397:                 case HostCliAction.JournalSelfTest:
00398:                     RunSelfTestAndQuit();
00399:                     return;
00400:                 case HostCliAction.JournalWeatherPanelSelfTest:
00401:                     GetTree().Quit(HostCli.RunJournalWeatherPanelSelfTest());
00402:                     return;
00403:                 case HostCliAction.JournalUiTest:
00404:                     RunJournalUiTestAndQuit();
00405:                     return;
00406:                 case HostCliAction.DashboardUiTest:
00407:                     RunDashboardUiTestAndQuit();
00408:                     return;
00409:                 case HostCliAction.PlayerPanelsUiTest:
00410:                     RunPlayerPanelsUiTestAndQuit();
00411:                     return;
00412:                 case HostCliAction.MusterUiTest:
00413:                     RunMusterUiTestAndQuit();
00414:                     return;
00415:                 case HostCliAction.DoseUiTest:
00416:                     RunDoseUiTestAndQuit();
00417:                     return;
00418:                 case HostCliAction.VerdictUiTest:
00419:                     RunVerdictUiTestAndQuit();
00420:                     return;
00421:                 case HostCliAction.EconomyUiTest:
00422:                     RunEconomyUiTestAndQuit();
00423:                     return;
00424:                 case HostCliAction.UtilityAiSelfTest:
00425:                     GetTree().Quit(HostCli.RunUtilityAiSelfTest(_dataDir));
00426:                     return;
00427:                 case HostCliAction.UtilityAiUiTest:
00428:                     RunUtilityAiUiTestAndQuit();
00429:                     return;
00430:                 case HostCliAction.InventoryUiTest:
00431:                     RunInventoryUiTestAndQuit();
00432:                     return;
00433:                 case HostCliAction.InventorySaveSelfTest:
00434:                     GetTree().Quit(HostCli.RunInventorySaveSelfTest());
00435:                     return;
00436:                 case HostCliAction.StartingSuppliesSelfTest:
00437:                     GetTree().Quit(HostCli.RunStartingSuppliesSelfTest(_dataDir));
00438:                     return;
00439:                 case HostCliAction.ExpeditionPanelUiTest:
00440:                     RunExpeditionPanelUiTestAndQuit();
00441:                     return;
00442:                 case HostCliAction.SurvivorsUiTest:
00443:                     RunSurvivorsUiTestAndQuit();
00444:                     return;
00445:                 case HostCliAction.Phase0UiTest:
00446:                     RunPhase0UiTestAndQuit();
00447:                     return;
00448:                 case HostCliAction.YearOfAshSaveSelfTest:
00449:                     GetTree().Quit(HostCli.RunYearOfAshSaveSelfTest(_dataDir));
00450:                     return;
00451:                 case HostCliAction.DutyRosterSaveSelfTest:
00452:                     GetTree().Quit(HostCli.RunDutyRosterSaveSelfTest(_dataDir));
00453:                     return;
00454:                 case HostCliAction.ExpansionHubSaveSelfTest:
00455:                     GetTree().Quit(HostCli.RunExpansionHubSaveSelfTest(_dataDir));
00456:                     return;
00457:                 case HostCliAction.DoseLedgerSelfTest:
00458:                     GetTree().Quit(HostCli.RunDoseLedgerSelfTest(_dataDir));
00459:                     return;
00460:                 case HostCliAction.ExpeditionSelfTest:
00461:                     GetTree().Quit(HostCli.RunExpeditionSelfTest());
00462:                     return;
00463:                 case HostCliAction.ExpeditionPlaytestSelfTest:
00464:                     GetTree().Quit(HostCli.RunExpeditionPlaytestSelfTest(_dataDir));
00465:                     return;
00466:                 case HostCliAction.BridgeSelfTest:
00467:                     GetTree().Quit(HostCli.RunBridgeSelfTest());
00468:                     return;
00469:                 case HostCliAction.PowerGridCatalogSelfTest:
00470:                     GetTree().Quit(HostCli.RunPowerGridCatalogSelfTest());
00471:                     return;
00472:                 case HostCliAction.ExpeditionEncounterBridgeSelfTest:
00473:                     GetTree().Quit(HostCli.RunExpeditionEncounterBridgeSelfTest());
00474:                     return;
00475:                 case HostCliAction.PatrolEncounterSelfTest:
00476:                     GetTree().Quit(HostCli.RunPatrolEncounterSelfTest(_dataDir));
00477:                     return;
00478:                 case HostCliAction.MedicalSelfTest:
00479:                     GetTree().Quit(HostCli.RunMedicalSelfTest());
00480:                     return;
00481:                 case HostCliAction.NarrativeSelfTest:
00482:                     GetTree().Quit(HostCli.RunNarrativeSelfTest());
00483:                     return;
00484:                 case HostCliAction.NpcArcSelfTest:
00485:                     GetTree().Quit(HostCli.RunNpcArcSelfTest());
00486:                     return;
00487:                 case HostCliAction.SurvivorsSelfTest:
00488:                     GetTree().Quit(HostCli.RunSurvivorsSelfTest());
00489:                     return;
00490:                 case HostCliAction.WorldSelfTest:
00491:                     GetTree().Quit(HostCli.RunWorldSelfTest());
00492:                     return;
00493:                 case HostCliAction.WorldExplorationSelfTest:
00494:                     GetTree().Quit(HostCli.RunWorldExplorationSelfTest(_dataDir));
00495:                     return;
00496:                 case HostCliAction.CartographySelfTest:
00497:                     GetTree().Quit(HostCli.RunCartographySelfTest(_dataDir));
00498:                     return;
00499:                 case HostCliAction.ExpansionDepthSelfTest:
00500:                     GetTree().Quit(HostCli.RunExpansionDepthSelfTest(_dataDir));
00501:                     return;
00502:                 case HostCliAction.EconomySelfTest:
00503:                     GetTree().Quit(HostCli.RunEconomySelfTest(_dataDir));
00504:                     return;
00505:                 case HostCliAction.DataIntegritySelfTest:
00506:                     GetTree().Quit(HostCli.RunDataIntegritySelfTest(_dataDir));
00507:                     return;
00508:                 case HostCliAction.ExportParitySelfTest:
00509:                     GetTree().Quit(HostCli.RunExportParitySelfTest(_dataDir, HostCli.GetOptionValue(OS.GetCmdlineUserArgs(), "--parity-target")));
00510:                     return;
00511:                 case HostCliAction.ResearchCatalogSelfTest:
00512:                     GetTree().Quit(HostCli.RunResearchCatalogSelfTest(_dataDir));
00513:                     return;
00514:                 case HostCliAction.RadioCatalogSelfTest:
00515:                     GetTree().Quit(HostCli.RunRadioCatalogSelfTest(_dataDir));
00516:                     return;
00517:                 case HostCliAction.CatalogBootPreflight:
00518:                     GetTree().Quit(HostCli.RunCatalogBootPreflight(_dataDir));
00519:                     return;
00520:                 case HostCliAction.CaravanSelfTest:
00521:                     GetTree().Quit(HostCli.RunCaravanSelfTest());
00522:                     return;
00523:                 case HostCliAction.AssetRegistrySelfTest:
00524:                     GetTree().Quit(HostCli.RunAssetRegistrySelfTest(_dataDir));
00525:                     return;
00526:                 case HostCliAction.AssetCoverageReport:
00527:                     GetTree().Quit(HostCli.RunAssetCoverageReport(_dataDir));
00528:                     return;
00529:                 case HostCliAction.StandaloneSystemsSelfTest:
00530:                     GetTree().Quit(HostCli.RunStandaloneSystemsSelfTest());
00531:                     return;
00532:                 case HostCliAction.Plans139To141SelfTest:
00533:                     GetTree().Quit(HostCli.RunPlans139To141SelfTest(_dataDir));
00534:                     return;
00535:                 case HostCliAction.Plans122to125SelfTest:
00536:                     GetTree().Quit(HostCli.RunPlans122to125SelfTest(_dataDir));
00537:                     return;
00538:                 case HostCliAction.LateTechMobilitySelfTest:
00539:                     GetTree().Quit(HostCli.RunLateTechMobilitySelfTest(_dataDir));
00540:                     return;
00541:                 case HostCliAction.Plans122to125BalanceSoak:
00542:                     GetTree().Quit(HostCli.RunPlans122to125BalanceSoak(_dataDir));
00543:                     return;
00544:                 case HostCliAction.SkyDefenseSelfTest:
00545:                     GetTree().Quit(HostCli.RunSkyDefenseSelfTest(_dataDir));
00546:                     return;
00547:                 case HostCliAction.VehicleGarageSelfTest:
00548:                     GetTree().Quit(HostCli.RunVehicleGarageSelfTest(_dataDir));
00549:                     return;
00550:                 case HostCliAction.DeepCoastSelfTest:
00551:                     GetTree().Quit(HostCli.RunDeepCoastSelfTest(_dataDir));
00552:                     return;
00553:                 case HostCliAction.DeepCoastHostSelfTest:
00554:                     GetTree().Quit(HostCli.RunDeepCoastHostSelfTest());
00555:                     return;
00556:                 case HostCliAction.WarlordSelfTest:
00557:                     GetTree().Quit(HostCli.RunWarlordSelfTest(_dataDir));
00558:                     return;
00559:                 case HostCliAction.WarlordHostSelfTest:
00560:                     GetTree().Quit(HostCli.RunWarlordHostSelfTest(_dataDir));
00561:                     return;
00562:                 case HostCliAction.WarlordUiSelfTest:
00563:                     GetTree().Quit(HostCli.RunWarlordUiSelfTest(_dataDir));
00564:                     return;
00565:                 case HostCliAction.FactionCommuniqueBoardSelfTest:
00566:                     GetTree().Quit(HostCli.RunFactionCommuniqueBoardSelfTest(_dataDir));
00567:                     return;
00568:                 case HostCliAction.Phase0SelfTest:
00569:                     GetTree().Quit(HostCli.RunPhase0SelfTest());
00570:                     return;
00571:                 case HostCliAction.Day1PlayableSelfTest:
00572:                     GetTree().Quit(HostCli.RunDay1PlayableSelfTest(_dataDir));
00573:                     return;
00574:                 case HostCliAction.Day1ToDay2MilestoneSelfTest:
00575:                     GetTree().Quit(HostCli.RunDay1ToDay2MilestoneSelfTest(_dataDir));
00576:                     return;
00577:                 case HostCliAction.ModSelfTest:
00578:                     GetTree().Quit(HostCli.RunModSelfTest());
00579:                     return;
00580:                 case HostCliAction.UiLayoutSelfTest:
00581:                     GetTree().Quit(HostCli.RunUiLayoutSelfTest(_dataDir));
00582:                     return;
00583:                 case HostCliAction.SettingsSelfTest:
00584:                     GetTree().Quit(HostCli.RunSettingsSelfTest(_dataDir));
00585:                     return;
00586:                 case HostCliAction.PlayableShellSelfTest:
00587:                     GetTree().Quit(HostCli.RunPlayableShellSelfTest(_dataDir));
00588:                     return;
00589:                 case HostCliAction.ShelterHazardLoopSelfTest:
00590:                     GetTree().Quit(HostCli.RunShelterHazardLoopSelfTest(_dataDir));
00591:                     return;
00592:                 case HostCliAction.ShelterOperationsSelfTest:
00593:                     GetTree().Quit(HostCli.RunShelterOperationsSelfTest(_dataDir));
00594:                     return;
00595:                 case HostCliAction.WaterSourcesSelfTest:
00596:                     GetTree().Quit(HostCli.RunWaterSourcesSelfTest(_dataDir));
00597:                     return;
00598:                 case HostCliAction.ShelterDecorSelfTest:
00599:                     GetTree().Quit(ShelterDecorSelfTest.Run(_dataDir));
00600:                     return;
00601:                 case HostCliAction.ShelterAtmosphereSelfTest:
00602:                     GetTree().Quit(ShelterAtmosphereSelfTest.Run(_dataDir));
00603:                     return;
00604:                 case HostCliAction.ShelterPhysicsSelfTest:
00605:                     RunShelterPhysicsSelfTestAndQuit();
00606:                     return;
00607:                 case HostCliAction.AudioSelfTest:
00608:                     GetTree().Quit(AtomicWar.GodotApp.Audio.AudioSelfTest.Run());
00609:                     return;
00610:                 case HostCliAction.BlackFlotillaSelfTest:
00611:                     GetTree().Quit(HostCli.RunBlackFlotillaSelfTest(_dataDir));
00612:                     return;
00613:                 case HostCliAction.RadioSelfTest:
00614:                     GetTree().Quit(HostCli.RunRadioSelfTest());
00615:                     return;
00616:                 case HostCliAction.UiSnapshotSelfTest:
00617:                     BeginSnapshotRun(regenerate: false);
00618:                     return;
00619:                 case HostCliAction.UiSnapshotRegenerate:
00620:                     BeginSnapshotRun(regenerate: true);
00621:                     return;
00622:                 case HostCliAction.OnboardingJourneySelfTest:
00623:                     GetTree().Quit(HostCli.RunOnboardingJourneySelfTest(_dataDir));
00624:                     return;
00625:                 case HostCliAction.DynamicWorldSelfTest:
00626:                     GetTree().Quit(HostCli.RunDynamicWorldSelfTest(_dataDir));
00627:                     return;
00628:                 case HostCliAction.WastelandInhabitantsSelfTest:
00629:                     GetTree().Quit(HostCli.RunWastelandInhabitantsSelfTest(_dataDir));
00630:                     return;
00631:                 case HostCliAction.OralLoreSelfTest:
00632:                     GetTree().Quit(HostCli.RunOralLoreSelfTest(_dataDir));
00633:                     return;
00634:                 case HostCliAction.ContentUtilizationSelfTest:
00635:                     GetTree().Quit(ContentUtilizationSelfTest.Run(
00636:                         ProjectSettings.GlobalizePath("res://"), _dataDir,
00637:                         ProjectSettings.GlobalizePath("res://") + "Assets/Ashfall.Core",
00638:                         ProjectSettings.GlobalizePath("res://src")));
00639:                     return;
00640:                 case HostCliAction.NarrativeContinuitySelfTest:
00641:                     GetTree().Quit(NarrativeContinuitySelfTest.Run(
00642:                         ProjectSettings.GlobalizePath("res://"), _dataDir));
00643:                     return;
00644:                 case HostCliAction.PropagandaSelfTest:
00645:                     GetTree().Quit(PropagandaSelfTest.Run(_dataDir));
00646:                     return;
00647:                 case HostCliAction.RumorNetworkSelfTest:
00648:                     GetTree().Quit(RumorNetworkSelfTest.Run(_dataDir));
00649:                     return;
00650:                 case HostCliAction.ShelterSecuritySelfTest:
00651:                     GetTree().Quit(ShelterSecuritySelfTest.Run(_dataDir));
00652:                     return;
00653:                 case HostCliAction.PersonalQuestSelfTest:
00654:                     GetTree().Quit(PersonalQuestSelfTest.Run(_dataDir));
00655:                     return;
00656:                 case HostCliAction.TimeCapsuleSelfTest:
00657:                     GetTree().Quit(TimeCapsuleSelfTest.Run(_dataDir));
00658:                     return;
00659:                 case HostCliAction.DeathLegacySelfTest:
00660:                     GetTree().Quit(SurvivorDeathLegacySelfTest.Run(_dataDir));
00661:                     return;
00662:                 case HostCliAction.RelationshipDecaySelfTest:
00663:                     GetTree().Quit(RelationshipDecaySelfTest.Run(_dataDir));
00664:                     return;
00665:                 case HostCliAction.VisitorIntegrationSelfTest:
00666:                     GetTree().Quit(VisitorIntegrationSelfTest.Run(_dataDir));
00667:                     return;
00668:                 case HostCliAction.PersonalBelongingsSelfTest:
00669:                     GetTree().Quit(PersonalBelongingsSelfTest.Run(_dataDir));
00670:                     return;
00671:                 case HostCliAction.ResearchUnlockSelfTest:
00672:                     GetTree().Quit(HostCliResearchUnlock.RunSelfTest(_dataDir));
00673:                     return;
00674:                 case HostCliAction.UnifiedEndingSelfTest:
00675:                     GetTree().Quit(HostCliUnifiedEnding.RunSelfTest(_dataDir));
00676:                     return;
00677:                 case HostCliAction.NpcMemorySelfTest:
00678:                     GetTree().Quit(HostCliNpcMemory.RunSelfTest(_dataDir));
00679:                     return;
00680:                 case HostCliAction.IdeologicalFrictionSelfTest:
00681:                     GetTree().Quit(HostCliIdeologicalFriction.RunSelfTest(_dataDir));
00682:                     return;
00683:                 case HostCliAction.RomanceFamilySelfTest:
00684:                     GetTree().Quit(HostCliRomanceFamily.RunSelfTest(_dataDir));
00685:                     return;
00686:                 case HostCliAction.VehicleCustomizationSelfTest:
00687:                     GetTree().Quit(HostCliVehicleCustomization.RunSelfTest(_dataDir));
00688:                     return;
00689:                 case HostCliAction.BackstorySelfTest:
00690:                     GetTree().Quit(HostCliBackstory.RunSelfTest(_dataDir));
00691:                     return;
00692:                 case HostCliAction.MetaProgressionSelfTest:
00693:                     GetTree().Quit(HostCliMetaProgression.RunSelfTest(_dataDir));
00694:                     return;
00695:                 case HostCliAction.TradeRoutesSelfTest:
00696:                     GetTree().Quit(HostCliTradeRoutes.RunSelfTest(_dataDir));
00697:                     return;
00698:                 case HostCliAction.HumanMigrationSelfTest:
00699:                     GetTree().Quit(HostCliHumanMigration.RunSelfTest(_dataDir));
00700:                     return;
00701:                 case HostCliAction.TunnelNetworkSelfTest:
00702:                     GetTree().Quit(HostCliTunnelNetwork.RunSelfTest(_dataDir));
00703:                     return;
00704:                 case HostCliAction.AudioAccessibilitySelfTest:
00705:                     GetTree().Quit(HostCliAudioAccessibility.RunSelfTest(_dataDir));
00706:                     return;
00707:                 case HostCliAction.ModSupportSelfTest:
00708:                     GetTree().Quit(HostCliModSupport.RunSelfTest(_dataDir));
00709:                     return;
00710:                 case HostCliAction.ShelterIdentitySelfTest:
00711:                     GetTree().Quit(HostCliShelterIdentity.RunSelfTest(_dataDir));
00712:                     return;
00713:                 case HostCliAction.OriginMechanicsSelfTest:
00714:                     GetTree().Quit(HostCliOriginMechanics.RunSelfTest(_dataDir));
00715:                     return;
00716:                 case HostCliAction.DynamicQuestSelfTest:
00717:                     GetTree().Quit(HostCliDynamicQuest.RunSelfTest(_dataDir));
00718:                     return;
00719:                 case HostCliAction.ShelterGovernanceSelfTest:
00720:                     GetTree().Quit(HostCliShelterGovernance.RunSelfTest(_dataDir));
00721:                     return;
00722:                 case HostCliAction.AgingSelfTest:
00723:                     GetTree().Quit(HostCliAging.RunSelfTest(_dataDir));
00724:                     return;
00725:                 case HostCliAction.DifficultySettingsSelfTest:
00726:                     GetTree().Quit(HostCliDifficultySettings.RunSelfTest(_dataDir));
00727:                     return;
00728:                 case HostCliAction.RailTrackMaintenanceSelfTest:
00729:                     GetTree().Quit(HostCliRailTrackMaintenance.RunSelfTest(_dataDir));
00730:                     return;
00731:                 case HostCliAction.GlassworksSelfTest:
00732:                     GetTree().Quit(HostCliGlassworks.RunSelfTest(_dataDir));
00733:                     return;
00734:                 case HostCliAction.BroadsheetPressSelfTest:
00735:                     GetTree().Quit(HostCliBroadsheetPress.RunSelfTest(_dataDir));
00736:                     return;
00737:                 case HostCliAction.KilnworksSelfTest:
00738:                     GetTree().Quit(HostCliKilnworks.RunSelfTest(_dataDir));
00739:                     return;
00740:                 case HostCliAction.WildlifeHarvestSelfTest:
00741:                     GetTree().Quit(HostCliWildlifeHarvest.RunSelfTest(_dataDir));
00742:                     return;
00743:                 case HostCliAction.StormForecastSelfTest:
00744:                     GetTree().Quit(HostCliStormForecast.RunSelfTest(_dataDir));
00745:                     return;
00746:                 case HostCliAction.DependencyTaperWithdrawalSelfTest:
00747:                     GetTree().Quit(HostCliDependencyTaperWithdrawal.RunSelfTest(_dataDir));
00748:                     return;
00749:                 case HostCliAction.AntenatalMaternalHealthSelfTest:
00750:                     GetTree().Quit(HostCliAntenatalMaternalHealth.RunSelfTest(_dataDir));
00751:                     return;
00752:                 case HostCliAction.ClinicalWardTriageSelfTest:
00753:                     GetTree().Quit(HostCliClinicalWardTriage.RunSelfTest(_dataDir));
00754:                     return;
00755:                 case HostCliAction.ChemicalReagentSynthesisSelfTest:
00756:                     GetTree().Quit(HostCliChemicalReagentSynthesis.RunSelfTest(_dataDir));
00757:                     return;
00758:                 case HostCliAction.MechanicalDrivelineSelfTest:
00759:                     GetTree().Quit(HostCliMechanicalDriveline.RunSelfTest(_dataDir));
00760:                     return;
00761:                 case HostCliAction.SleepAcousticRestSelfTest:
00762:                     GetTree().Quit(HostCliSleepAcousticRest.RunSelfTest(_dataDir));
00763:                     return;
00764:                 case HostCliAction.ShelterArchiveSelfTest:
00765:                     GetTree().Quit(HostCliShelterArchive.RunSelfTest(_dataDir));
00766:                     return;
00767:                 case HostCliAction.DreamSystemSelfTest:
00768:                     GetTree().Quit(HostCliDreamSystem.RunSelfTest(_dataDir));
00769:                     return;
00770:                 case HostCliAction.AccessibilitySettingsSelfTest:
00771:                     GetTree().Quit(HostCliAccessibilitySettings.RunSelfTest(_dataDir));
00772:                     return;
00773:                 case HostCliAction.MemoryDecaySelfTest:
00774:                     GetTree().Quit(HostCliMemoryDecay.RunSelfTest(_dataDir));
00775:                     return;
00776:                 case HostCliAction.InterpersonalConflictSelfTest:
00777:                     GetTree().Quit(HostCliInterpersonalConflict.RunSelfTest(_dataDir));
00778:                     return;
00779:                 case HostCliAction.ExerciseSelfTest:
00780:                     GetTree().Quit(HostCliExercise.RunSelfTest(_dataDir));
00781:                     return;
00782:                 case HostCliAction.ShelterMaintenanceSelfTest:
00783:
00784:                     GetTree().Quit(HostCliShelterMaintenance.RunSelfTest(_dataDir));
00785:                     return;
00786:                 case HostCliAction.SurvivorRoutinesSelfTest:
00787:                     GetTree().Quit(HostCliSurvivorRoutines.RunSelfTest(_dataDir));
00788:                     return;
00789:                 case HostCliAction.AfflictionBridgeSelfTest:
00790:                     GetTree().Quit(AfflictionBridgeSelfTest.Run(_dataDir));
00791:                     return;
00792:                 case HostCliAction.RadiationMutationSelfTest:
00793:                     GetTree().Quit(HostCliRadiationMutation.RunSelfTest(_dataDir));
00794:                     return;
00795:                 case HostCliAction.RadioProductionSelfTest:
00796:                     GetTree().Quit(HostCliRadioProduction.RunSelfTest(_dataDir));
00797:                     return;
00798:                 case HostCliAction.WorkingAnimalsSelfTest:
00799:                     GetTree().Quit(HostCliWorkingAnimals.RunSelfTest(_dataDir));
00800:                     return;
00801:                 case HostCliAction.BlackMarketSelfTest:
00802:                     GetTree().Quit(HostCliBlackMarket.RunSelfTest(_dataDir));
00803:                     return;
00804:                 case HostCliAction.CultureCreationSelfTest:
00805:                     GetTree().Quit(CultureCreationSelfTest.Run(_dataDir));
00806:                     return;
00807:                 case HostCliAction.PsychologicalProfileSelfTest:
00808:                     GetTree().Quit(PsychologicalProfileSelfTest.Run(_dataDir));
00809:                     return;
00810:                 case HostCliAction.SkillCertificationSelfTest:
00811:                     GetTree().Quit(SkillCertificationSelfTest.Run(_dataDir));
00812:                     return;
00813:                 case HostCliAction.ChildDevelopmentSelfTest:
00814:                     GetTree().Quit(ChildDevelopmentSelfTest.Run(_dataDir));
00815:                     return;
00816:                 case HostCliAction.BestiarySelfTest:
00817:                     GetTree().Quit(BestiarySelfTest.Run(_dataDir));
00818:                     return;
00819:                 case HostCliAction.HealthHistorySelfTest:
00820:                     GetTree().Quit(HealthHistorySelfTest.Run(_dataDir));
00821:                     return;
00822:                 case HostCliAction.LeadershipSuccessionSelfTest:
00823:                     GetTree().Quit(LeadershipSuccessionSelfTest.Run(_dataDir));
00824:                     return;
00825:             }
00826:             }
00827:             catch (System.Exception ex)
00829:                 // A gate threw. Report FAIL against the action that was running and
00830:                 // quit non-zero so this can never be scraped as PASS or hang.
00831:                 GetTree().Quit(HostCli.EmitUnhandledSelfTestFailure(
00832:                     HostCli.SelfTestNameFor(cliAction), ex));
00833:                 return;
00834:             }
00835:
00836:             AtomicWar.GodotApp.Settings.UserSettingsStore.Apply(AtomicWar.GodotApp.Settings.UserSettingsStore.Current);
00837:
00838:             // ── Save/Load host session ───────────────────────────────────────
00839:             _saveLoadHost = new SaveLoadHostSession();
00840:             _saveLoadHost.Initialize(ProjectSettings.GlobalizePath("user://"));
00841:             AddChild(_saveLoadHost);
00842:
00843:             BuildUserInterface();
00844:             _saveLoadPanel.Bind(_saveLoadHost);
00845:             _saveLoadHost.SlotsChanged += UpdateContinueButton;
00846:
00847:             SetupJournal();
00848:             SetupIceRoad();
00849:             SetupDutyRoster();
00850:             // Questline master registry: loaded early so expansion quest catalogs
00851:             // can validate their quest IDs against the canonical list.
00852:             _questlineMaster = new QuestlineMasterCatalogLoader(
00853:                 new FileSystemIO(), new SystemTextJsonSerializer()).Load(_dataDir);
00854:             GD.Print($"[Ashfall Godot] Questline master: {_questlineMaster.Count} quest IDs registered");
00855:             SetupExpansions();
00856:             // Year of Ash used to initialise lazily on first button press, so its save
00857:             // was not restored at boot and it was the only subsystem with no banner line.
00858:             SetupYearOfAsh();
00859:             // Moral choice ledger ("The Weight of Survival"): constructed at boot so
00860:             // its save restores before any encounter can resolve against a blank ledger.
00861:             SetupMoralChoice();
00862:             // Endgame phase authority + collectible/unique ledgers restore at boot
00863:             // so day-tick triggers and loot channels never start from blank state.
00864:             SetupEndgame();
00865:             SetupCollectibles();
00866:             SetupShelterFireHazard();
00867:             // Personal quests + chemical synthesis restore at boot so SaveAll /
00868:             // day-tick never capture blank instances after a Continue/slot switch.
00869:             SetupPersonalQuests();
00870:             SetupNarrativeQuestlines();
00871:             SetupChemicalSynthesis();
00872:
00873:             if (DisplayServer.GetName() == "headless")
00874:             {
00875:                 string[] userArgs = OS.GetCmdlineUserArgs();
00876:                 if (userArgs != null && userArgs.Length > 0)
00877:                 {
00878:                     GD.PrintErr($"[Ashfall Godot] Unrecognized headless argument(s): {string.Join(" ", userArgs)}. Run with --host-help to see valid flags.");
00879:                     GetTree().Quit(1);
00880:                     return;
00881:                 }
00882:
00883:                 GD.Print("[Ashfall Godot] Headless interactive boot completed. Exiting cleanly.");
00884:                 GetTree().Quit(0);
00885:                 return;
00886:             }
00887:         }
00888:
00889:         public override void _Process(double delta)
00890:         {
00891:             // Plan 60 / D6 — the bedside vigil is the only thing in the game allowed to
00892:             // run on wall-clock time, and only because its <em>duration</em> is the
00893:             // point. What it changes in the simulation is a boolean (kept / not kept),
00894:             // so frame rate can never move a campaign outcome.
00895:             _medical?.TickVigil(delta);
00896:             // The diagnostics strip used to rebuild its string every frame AND call
00897:             // Engine.GetVersionInfo(), which allocates a Godot Dictionary — 60 allocations
00898:             // a second for a version that never changes. Cache the version, refresh ~4x/sec.
00899:             _diagnosticsAccum += delta;
00900:             if (_diagnosticsAccum < DiagnosticsRefreshSeconds) return;
00901:             double elapsed = _diagnosticsAccum;
00902:             _diagnosticsAccum = 0.0;
00903:
00904:             if (_diagnosticsLabel == null || !IsInstanceValid(_diagnosticsLabel)) return;
00905:             double fps = Engine.GetFramesPerSecond();
00906:             double memMb = (long)OS.GetStaticMemoryUsage() / (1024.0 * 1024.0);
00907:             string verdictSave = _verdict != null
00908:                 ? $" | VerdictSave v{_verdict.LoadedSaveVersion}{( _verdict.WasSaveMigrated ? " (migrated)" : "")}"
00909:                 : string.Empty;
00910:             _diagnosticsLabel.Text = $"FPS: {fps:F0} | Static Mem: {memMb:F1} MB | Godot {s_engineVersion}{verdictSave}";
00911:
00912:             _diagnosticsLogAccum += elapsed;
00913:             if (_diagnosticsLogAccum >= 1.0)
00914:             {
00916:             }
00917:
00918:             // Flush any journal writes that were coalesced since the last tick.
00919:             FlushJournalIfDirty();
00920:             // Flush the Holdfast S1 save the same way — one write per burst, not per event.
00921:             FlushHoldfastIfDirty();
00922:             FlushDutyRosterIfDirty();
00923:             FlushExpansionQuestsIfDirty();
00924:             FlushThirdonaryIfDirty();
00925:             FlushExpansionHubIfDirty();
00926:             FlushVerdictIfDirty();
00927:             FlushMaritimeIfDirty();
00928:             FlushExpeditionIfDirty();
00929:             FlushTravelEncountersIfDirty();
00930:             FlushNarrativeIfDirty();
00931:             FlushEventAdapterIfDirty();
00932:             FlushMedicalIfDirty();
00939:             FlushEndgameIfDirty();
00940:             FlushShelterFireIfDirty();
00941:             FlushPersonalQuestsIfDirty();
00942:             FlushNarrativeQuestlinesIfDirty();
00943:             FlushChemicalSynthesisIfDirty();
00944:             FlushCollectiblesIfDirty();
00945:             FlushCampaignDayIfDirty();
00946:             FlushOutpostSettlementIfDirty();
00947:             FlushTerritoryControlIfDirty();
00948:             FlushCookingIfDirty();
00949:             FlushRetentionIfDirty();
00950:             FlushCampaignLegacyIfDirty();
00951:             FlushShelterGovernanceIfDirty();
00952:             FlushAgingIfDirty();
00953:
00954:             // ── Sleep / End Day countdown timer (Phase 2 continuation)
00955:             if (_advanceTimerRemaining > 0 && !_advanceCancelled)
00956:             {
00957:                 _advanceTimerRemaining -= delta;
00958:                 if (_advanceTimerRemaining <= 0)
00960:                     _advanceTimerRemaining = 0;
00961:                     _statusLabel.Text = "Sleep accepted — advancing day …";
00962:                     CommitAdvance();
00963:                 }
00964:                 else if (_statusLabel != null)
00965:                 {
00966:                     _statusLabel.Text = $"Sleep in progress … {_advanceTimerRemaining:F0}s remaining";
00969:         }
00970:
00971:         public override void _UnhandledKeyInput(InputEvent @event)
00972:         {
00973:             if (!@event.IsPressed() || @event.IsEcho()) return;
00974:
00975:             if (AshfallInputActions.IsForecast(@event) && _state == GameState.Playing)
00976:             {
00977:                 OpenPlayerPanel("weather_forecast");
00978:                 GetViewport().SetInputAsHandled();
00979:             }
00980:             else if (AshfallInputActions.IsWeatherHistory(@event) && _state == GameState.Playing)
00981:             {
00982:                 OpenPlayerPanel("weather_history");
00983:                 GetViewport().SetInputAsHandled();
00984:             }
00985:             else if (AshfallInputActions.IsJournal(@event))
00986:             {
00987:                 if (_state == GameState.Playing && _dashboard.Visible)
00988:                     OpenPlayerPanel("journal");
00989:                 else
00990:                     ToggleJournal();
00991:                 GetViewport().SetInputAsHandled();
00992:             }
00993:             else if (AshfallInputActions.IsHelp(@event) && _state == GameState.Playing)
00994:             {
00995:                 OpenPlayerPanel("help");
00996:                 GetViewport().SetInputAsHandled();
00997:             }
00998:             else if (AshfallInputActions.IsGuidance(@event) && _state == GameState.Playing)
00999:             {
01000:                 // C2 / Plan 17B Phase D — F2 toggles guidance: open when closed,
01001:                 // close when open, never permanently disabled. Veteran mode is
01002:                 // handled by the route's open action (status notice, no panel).
01003:                 if (_onboardingHintPanel != null && _onboardingHintPanel.IsOpen)
01004:                     Ashfall.Core.UI.PanelRegistry.TryClose("guidance");
01005:                 else
01006:                     OpenPlayerPanel("guidance");
01007:                 GetViewport().SetInputAsHandled();
01008:             }
01009:             else if (AshfallInputActions.IsHoldfast(@event) && _state == GameState.Playing)
01010:             {
01011:                 OpenPlayerPanel("holdfast");
01012:                 GetViewport().SetInputAsHandled();
01013:             }
01014:             else if (AshfallInputActions.IsExpeditions(@event) && _state == GameState.Playing)
01015:             {
01016:                 OpenPlayerPanel("expeditions");
01017:                 GetViewport().SetInputAsHandled();
01018:             }
01019:             else if (AshfallInputActions.IsEvents(@event) && _state == GameState.Playing)
01020:             {
01021:                 OpenPlayerPanel("events_log");
01022:                 GetViewport().SetInputAsHandled();
01023:             }
01024:             else if (_state == GameState.Playing && AtomicWar.GodotApp.UI.AshfallFocusNavigator.HandleNavInput(this, @event))
01025:             {
01026:                 GetViewport().SetInputAsHandled();
01027:             }
01028:             else if (AshfallInputActions.IsCloseOrCancel(@event) && _state == GameState.Playing)
01029:             {
01030:                 // Global dismiss for keyboard-driven UI: Esc closes any open
01031:                 // overlay panel or modal (panels also handle Esc locally).
01032:                 // If no overlay is open, returns to main menu.
01033:                 CancelAdvanceConfirmation();
01034:                 if (AnyOverlayPanelOpen())
01035:                 {
01036:                     CloseAllOverlayPanels();
01037:                 }
01040:                     ReturnToMenu();
01041:                 }
01042:                 GetViewport().SetInputAsHandled();
01043:             }
01044:             else if (_journalBook != null && _journalBook.IsOpen)
01045:             {
01046:                 if (AshfallInputActions.GetJournalTabNumber(@event, out int tab))
01047:                 {
01048:                     _journal.SwitchTab(tab - 1);
01049:                     GetViewport().SetInputAsHandled();
01050:                 }
01051:                 else if (AshfallInputActions.IsCloseOrCancel(@event))
01052:                 {
01053:                     // Cancel a pending sleep advance before closing the journal.
01054:                     CancelAdvanceConfirmation();
01055:                     _journalBook.Close();
01056:                     GetViewport().SetInputAsHandled();
01057:                 }
01058:             }
01059:         }
01060:
01061:         public override void _Notification(int what)
01062:         {
01063:             if (what == NotificationWMCloseRequest)
01064:             {
01065:                 // Always cancel any in-progress sleep advance on teardown so stale
01066:                 // countdowns don't tick after the window closes.
01067:                 CancelAdvanceConfirmation();
01068:
01069:                 // GAP-ARCH-01 Phase 0: save ALL 34 stores on window close, not just
01070:                 // the original 11. The partial list silently dropped Verdict, Maritime,
01071:                 // Expeditions, Combat, Narrative, Medical, World, Crafting, Caravans,
01072:                 // YearOfAsh, Phase0, StartingLevel, Greenhouse, Radio, DailyBriefing,
01073:                 // PowerGrid, MedicalWard, Memorial, SilentFoundry, Disease, WastelandMap,
01074:                 // EncounterChoice, and all 21 ExpandedShelter stores.
01075:                 SaveAll();
01076:                 ShutdownDebtConsequenceIntegration();
01077:
01078:                 GetTree().Quit();
01079:             }
01080:         }
01081:
01082:         private void ResolveDataDir()
01083:         {
01084:             _dataDir = ModRuntime.Prepare(CatalogPath.ResolveDataDir());
01085:         }
01086:
01087:         /// <summary>
01088:         /// Validate that all required catalogs are present and well-formed.
01089:         /// Throws if any required catalog is missing or malformed, preventing the game from starting.
01090:         /// </summary>
01091:         private void ValidateRequiredCatalogs()
01092:         {
01093:             var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
01094:             var json = new SystemTextJsonSerializer();
01095:
01096:             // Use CatalogBootValidator to check all registered catalogs
01097:             var report = CatalogBootValidator.Validate(_dataDir, fileIO, json);
01098:
01099:             GD.Print(report.ToString());
01100:
01101:             // Throw if any required catalogs failed to load
01102:             CatalogBootValidator.ThrowIfRequiredFailed(report);
01103:         }
01104:
01105:         /// <summary>
01106:         /// Snapshot regression driver. Mounts SnapshotOrchestrator into the
01107:         /// tree (it needs process frames to render each panel in a SubViewport
01108:         /// and quits the app when the run completes):
01109:         ///   diff mode      — capture into snapshot-capture/ and compare against
01110:         ///                    snapshots/ goldens; per-panel MATCH/NEW/DRIFT/FAIL;
01111:         ///                    exit 1 on any drift or capture failure
01112:         ///   regenerate mode — capture straight into snapshots/ (overwrites goldens)
01113:         /// SubViewport texture reads need a real renderer; with --headless every
01114:         /// target reports FAIL (renderer unavailable) instead of writing blanks.
01115:         /// </summary>
01116:         private void BeginSnapshotRun(bool regenerate)
01117:         {
01118:             string goldenRoot = HostCli.SnapshotGoldenRoot();
01119:             var orch = new SnapshotOrchestrator();
01120:             AddChild(orch);
01121:             if (regenerate)
01122:             {
01123:                 GD.Print($"[UiSnapshot] REGENERATE — overwriting goldens in {goldenRoot}");
01124:                 orch.BeginRegenerate(SnapshotHarness.Targets, goldenRoot);
01125:             }
01126:             else
01127:             {
01128:                 string captureRoot = HostCli.SnapshotCaptureRoot();
01129:                 GD.Print($"[UiSnapshot] DIFF — captures in {captureRoot}, goldens in {goldenRoot}");
01130:                 orch.BeginDiff(SnapshotHarness.Targets, goldenRoot, captureRoot);
01131:             }
01132:         }
01133:
01134:         private void UpdateStatus()
01135:         {
01136:             if (_statusLabel == null || _journal == null) return;
01137:             _statusLabel.Text =
01138:                 $"Ready: {_dataDir}\n" +
01139:                 $"Journal: {_journal.EntryCount} pages · " +
01140:                 $"{(_journal.HasUnread ? "unread" : "nothing new")} · " +
01141:                 $"Day {_simDay} · [J] toggles the ledger · [E] opens events log.";
01142:         }
01143:     }
01144: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **The safe subject is six authored threshold reactions and their current moral-choice host path, not a generalized dialogue framework.**.

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
