# Plan 120 — Crossing Faction Catalog and Player-Reachable Political Alternatives

> **Rebuild status:** PARTIAL 8-FACTION CONTENT AUTHORITY — LIVE CROSSING REACHABILITY PLAN
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-2`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round2-2026-09-25`
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

- The original 3→8 data target is complete: `crossing_factions.json` has 8 unique rows, `CrossingCatalogLoader` loads them, and current tests validate identity, ranges, wants/offers, quotes, access rules and icon fallback.
- The live Crossing host currently composes `CrossingQuestSystem`, `VouchAccessSystem` and `CrossingArbitrationSystem`; the catalog itself is loaded by `CrossingSession`/headless paths but the five new faction IDs are not found in current crossing encounters or standing-record data.
- The safe leap forward is a read-only faction directory/projection in the existing Crossing host and panel, with authored encounter/quest references added only when a current quest/encounter owner can consume them. No new faction manager, trust ledger or save section is proposed without a current mechanic owner.

**Bounded outcome:** The eight-row catalog and loader are implemented, but current evidence shows the five added blocs are referenced by the catalog/headless demo/tests rather than the live Crossing quest/encounter/standing-record route. This plan becomes a bounded gameplay-reachability package: bind the existing `CrossingCatalog` to the live Crossing host/session and project the five alternatives through current Crossing surfaces without creating a second faction or trust authority.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `crossing_factions.json` is present with 8 unique `faction_` rows; `CrossingFactionExpansionTests` asserts exact 8, baseline 3 preservation, new 5, valid fields, distinct trade profiles and icon fallback.
- `CrossingCatalog.cs` loads faction/location/quest/item/encounter files; `CrossingSession` demonstrates catalog+vouch composition but is primarily a selftest/headless seam.
- `ExpansionHostSession` is the live Crossing host owner and currently binds `CrossingQuests`, `Vouch`, `Arbitration`, `Ledger` and the expansion hub save.
- Current source search finds the five added faction IDs in the Crossing catalog, constants/headless demo and tests, but not in crossing encounter/quest data or standing-record factions. The content is therefore not yet fully player-reachable.

**Master-authority sections applied to this rebase:**

- Master authority Volume 28 verification cookbook: focused evidence before broad gates.
- Lane D save/state/compatibility guidance: owner DTOs, migration and restore proof.
- Lane E UI/UX/accessibility guidance: truthful projections and keyboard/controller lifecycle.
- Lane G testing guidance: smallest affected target, negative cases and deterministic replay.
- Anti-padding protocol: content exhaustion may end the plan before the character checkpoint.
- Volume 32 faction, narrative and fact-projection guidance.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Retain the eight-row catalog and current loader as immutable content authority.
- Add a single live `CrossingCatalog` binding to `ExpansionHostSession`/the Crossing host composition, without creating a second catalog loader.
- Project faction identity, wants/offers, trust and access rules in the existing Crossing panel as truthful read-only information plus explicit available actions only where current quest/vouch/arbitration owners support them.
- Add authored references for the five new factions only to current quest/encounter/standing-record schemas after verifying their fields and consumer semantics.
- Keep faction trust/access mechanics owned by existing Crossing systems; if persistent faction trust is required, name the existing owner and save seam before implementation.

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
| faction/location/quest/item/encounter definitions | CrossingCatalogLoader | `Assets/Ashfall.Core/CrossingCatalog.cs` | Sole Crossing content catalog; no parallel faction registry. |
| live Crossing quest progression and eligibility | CrossingQuestSystem | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` | Owns quest state and choices. |
| live gate/access state | VouchAccessSystem | `Assets/Ashfall.Core/VouchAccessSystem.cs` | Owns persisted Crossing access; do not add a second faction trust store. |
| political backing/ruling state | CrossingArbitrationSystem | `Assets/Ashfall.Core/CrossingArbitrationSystem.cs` | Owns arbitration/backer state. |
| live host composition and expansion-hub save | ExpansionHostSession | `src/Host/ExpansionHostSession.cs; Assets/Ashfall.Core/ExpansionHubSave.cs` | Bind one catalog to the live session; host does not decide faction mechanics. |
| player-facing quest/access projection | Crossing UI | `src/UI/CrossingQuestPanel.cs; src/Main.GameFlow.cs` | Read-only presentation and current command routing. |
| catalog and cross-system proof | Crossing focused tests | `Ashfall.Core.Tests/CrossingFactionExpansionTests.cs; Ashfall.Core.Tests/World/Plan118_120StandingCrossingIntegrationTests.cs; Ashfall.Core.Tests/World/Plan126_129CrossingFoundryIntegrationTests.cs` | Executable current evidence and future reachability proof. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Crossing Faction Catalog and Player-Reachable Political Alternatives
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ CrossingCatalogLoader
│   faction/location/quest/item/encounter definitions
│ CrossingQuestSystem
│   live Crossing quest progression and eligibility
│ VouchAccessSystem
│   live gate/access state
│ CrossingArbitrationSystem
│   political backing/ruling state
│ ExpansionHostSession
│   live host composition and expansion-hub save
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

1. **Preserve current state ownership.** CrossingCatalogLoader owns faction/location/quest/item/encounter definitions: Sole Crossing content catalog; no parallel faction registry.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| faction/location/quest/item/encounter definitions | CrossingCatalogLoader | `Assets/Ashfall.Core/CrossingCatalog.cs` | Sole Crossing content catalog; no parallel faction registry. |
| live Crossing quest progression and eligibility | CrossingQuestSystem | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` | Owns quest state and choices. |
| live gate/access state | VouchAccessSystem | `Assets/Ashfall.Core/VouchAccessSystem.cs` | Owns persisted Crossing access; do not add a second faction trust store. |
| political backing/ruling state | CrossingArbitrationSystem | `Assets/Ashfall.Core/CrossingArbitrationSystem.cs` | Owns arbitration/backer state. |
| live host composition and expansion-hub save | ExpansionHostSession | `src/Host/ExpansionHostSession.cs; Assets/Ashfall.Core/ExpansionHubSave.cs` | Bind one catalog to the live session; host does not decide faction mechanics. |
| player-facing quest/access projection | Crossing UI | `src/UI/CrossingQuestPanel.cs; src/Main.GameFlow.cs` | Read-only presentation and current command routing. |
| catalog and cross-system proof | Crossing focused tests | `Ashfall.Core.Tests/CrossingFactionExpansionTests.cs; Ashfall.Core.Tests/World/Plan118_120StandingCrossingIntegrationTests.cs; Ashfall.Core.Tests/World/Plan126_129CrossingFoundryIntegrationTests.cs` | Executable current evidence and future reachability proof. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load one CrossingCatalog through the existing loader
2. bind the catalog to the live ExpansionHostSession
3. derive a read-only faction directory for Crossing UI/host consumers
4. show current identity/trade/access information without duplicating state
5. route explicit faction-related actions through existing quest/vouch/arbitration owners
6. add authored references only after schema/consumer verification
7. capture/restore existing expansion-hub state and rederive the directory

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Faction definitions are immutable catalog data; Crossing quest, vouch, arbitration and expansion-hub save state remain in their current owners.
- A faction directory/projection is derived and need not be persisted.
- If faction trust becomes mutable, it must be added to the named current owner’s existing state/save, not stored in the catalog or panel.
- Access rules are explanatory contracts until a current owner exposes a matching action; do not fake mechanical gates.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- The live host uses exactly one CrossingCatalog instance.
- A faction ID shown in the panel resolves to a current catalog row and has a truthful status (available, referenced, locked, or future-only).
- Wants/offers are not treated as inventory item IDs unless the current schema and consumer prove that meaning.
- A faction reference cannot create a new trust/access state outside the named owner.
- Catalog and panel restore/rebind remain deterministic and do not re-fire quest or arbitration transitions.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `crossing_factions.json` is the sole faction catalog.
- Crossing encounters/quests and standing-record data may reference canonical faction IDs only where their current schemas and validators allow it.
- New faction rows require a current consumer, valid home region, distinct trade profile and icon/accessibility fallback.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the existing `expansion_hub` save for vouch, arbitration and Crossing quest state.
- No new save section is justified by a read-only directory or authored catalog row.
- If mutable faction trust is later approved, extend the current named owner state and its existing capture/restore path with migration tests.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Faction directory ordering is stable by canonical ID/authored order.
- No wall-clock/hash iteration determines player-facing faction order.
- The same quest/arbitration command sequence and save state produce the same faction projection and transitions.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Crossing quest choices and stage events come from `CrossingQuestSystem`.
- Vouch/access transitions come from `VouchAccessSystem`; arbitration/backer transitions come from `CrossingArbitrationSystem`.
- A faction reference in an encounter/quest is an authored fact consumed by its existing owner.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/ExpansionHostSession.cs
- src/UI/CrossingQuestPanel.cs
- src/Main.GameFlow.cs
- src/Main.ExpansionHub.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Faction names, quotes and access rules are fictional and restrained.
- Political alternatives should be distinct without copying real countries, wars, people or UI layouts.
- The Crossing should expose trade-offs and values, not a fake reputation bar disconnected from current mechanics.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | The live host never binds CrossingCatalog. | CrossingCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | The panel displays factions that no current quest/encounter can use without labeling them as future-only. | CrossingQuestSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A new faction trust map is persisted outside the existing owner. | VouchAccessSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Wants/offers are treated as inventory IDs without schema proof. | CrossingArbitrationSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A catalog restore creates duplicate quest/arbitration side effects. | ExpansionHostSession | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/CrossingFactionExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan118_120StandingCrossingIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan126_129CrossingFoundryIntegrationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — current census and reachability audit | Read catalog, live ExpansionHostSession, Crossing panel, quest/vouch/arbitration owners and tests. | Five new rows are identified as catalog-only residual. | No production path until the owning implementation package is separately claimed. |
| 1 — one-catalog host binding | Bind the existing loader/catalog to the live host without a new owner. | One instance is observable in the current route. | No production path until the owning implementation package is separately claimed. |
| 2 — truthful panel projection | Add a read-only faction directory with status labels and no shadow mechanics. | All eight rows are visible with accurate availability. | No production path until the owning implementation package is separately claimed. |
| 3 — consumer references | Add only those encounter/quest/standing-record references proven to resolve and be reachable. | Each new faction has at least one truthful live consumer or remains explicitly future-only. | No production path until the owning implementation package is separately claimed. |
| 4 — persistence/replay seal | Verify existing expansion-hub save, restore and command determinism. | No duplicate state or event replay. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/crossing_factions.json | READ ONLY; MODIFY only for proven reference/quality gap | 8-row authority |
| Assets/Ashfall.Core/CrossingCatalog.cs | READ ONLY; extend only at existing loader boundary | Catalog owner |
| src/Host/ExpansionHostSession.cs | PROPOSED MODIFY after a separate implementation claim | Live host composition |
| src/UI/CrossingQuestPanel.cs | PROPOSED MODIFY after a separate implementation claim | Current panel |
| Assets/Ashfall.Core/ExpansionHubSave.cs | READ ONLY; no new section | Existing persistence owner |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Creating a parallel faction/trust authority. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Editing a shared host/UI path without a fresh exact claim. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Treating future-only references as live mechanics. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Overwriting current Crossing quest/arbitration state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No ninth faction or arbitrary catalog growth.
- No new faction trust save store.
- No new Crossing manager.
- No production edits in this documentation rebase.

# 23. Rollback and Recovery

- Revert this plan document.
- Future host/UI work must be isolated and preserve the existing expansion-hub save fixture.
- If live binding is rejected, retain the catalog as explicitly future-only rather than fabricating reachability.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- The 8 current rows and five new IDs are documented.
- The catalog-only residual is explicit.
- One-catalog host/panel/owner/save/replay contracts are precise.
- No parallel faction authority or save section is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Retain the eight-row catalog and current loader as immutable content authority.
- Add a single live `CrossingCatalog` binding to `ExpansionHostSession`/the Crossing host composition, without creating a second catalog loader.
- Project faction identity, wants/offers, trust and access rules in the existing Crossing panel as truthful read-only information plus explicit available actions only where current quest/vouch/arbitration owners support them.
- Add authored references for the five new factions only to current quest/encounter/standing-record schemas after verifying their fields and consumer semantics.
- Keep faction trust/access mechanics owned by existing Crossing systems; if persistent faction trust is required, name the existing owner and save seam before implementation.

## MUST NOT DO

- No ninth faction or arbitrary catalog growth.
- No new faction trust save store.
- No new Crossing manager.
- No production edits in this documentation rebase.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/CrossingFactionExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan118_120StandingCrossingIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan126_129CrossingFoundryIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — current census and reachability audit — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: faction/location/quest/item/encounter definitions → CrossingCatalogLoader; live Crossing quest progression and eligibility → CrossingQuestSystem; live gate/access state → VouchAccessSystem; political backing/ruling state → CrossingArbitrationSystem; live host composition and expansion-hub save → ExpansionHostSession; player-facing quest/access projection → Crossing UI; catalog and cross-system proof → Crossing focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 120.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 120 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by CrossingCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/CrossingCatalog.cs`

### `Assets/Ashfall.Core/CrossingCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 429 lines / 19786 bytes.
- SHA-256: `4f6d4ec15b065b40aa0f44a27c03ce0d190c2387e6f2124830e93ca3ac57e043`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class CrossingFactionEntry
public string id;
public string display_name;
public string alignment;
public string home_region;
public bool is_active;
public float trust;
public string[] wants;
public string[] offers;
public string signature_quote;
public string access_rule;
public string badge_asset_id;
public class CrossingLocationEntry
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
public class CrossingQuestStageEntry
public string id;
public string text;
public class CrossingQuestChoiceEntry
public string id;
public string text;
public string set_flag;
public class CrossingQuestEntry
public string id;
public string display_name;
public string type;
public string briefing;
public string prereq_quest_id;
public int min_day;
public CrossingQuestStageEntry[] stages;
public CrossingQuestChoiceEntry[] choices;
public string knowledge_key;
public string target_location_id;
public int StageCount => stages != null ? stages.Length : 0;
public class CrossingItemEntry
public string id;
public string displayName;
public string description;
public string type;
public int stackMax;
public float weight;
public float tradeValue;
public float thirstRestore;
public float hungerRestore;
public float moraleEffect;
public class CrossingChoiceEntry
public string text;
public string[] cost_items;
public string result;
public class CrossingEncounterEntry
public string id;
public string name;
public string target_location;
public string description;
public string threat_level;
public CrossingChoiceEntry[] choices;
public class CrossingCrisisEntry
public string id;
public string name;
public string[] phases;
public string description;
public string resolution;
public class CrossingEncountersContainer
public CrossingEncounterEntry[] encounters;
public CrossingCrisisEntry[] crises;
public sealed class CrossingCatalog
public List<CrossingFactionEntry> Factions { get; } = new List<CrossingFactionEntry>();
public List<CrossingLocationEntry> Locations { get; } = new List<CrossingLocationEntry>();
public List<CrossingQuestEntry> Quests { get; } = new List<CrossingQuestEntry>();
public List<CrossingItemEntry> Items { get; } = new List<CrossingItemEntry>();
public List<CrossingEncounterEntry> Encounters { get; } = new List<CrossingEncounterEntry>();
public List<CrossingCrisisEntry> Crises { get; } = new List<CrossingCrisisEntry>();
public CrossingFactionEntry? GetFaction(string id) => Find(Factions, id, f => f.id);
public CrossingLocationEntry? GetLocation(string id) => Find(Locations, id, e => e.id);
public CrossingQuestEntry? GetQuest(string id) => Find(Quests, id, q => q.id);
public CrossingItemEntry? GetItem(string id) => Find(Items, id, item => item.id);
public CrossingEncounterEntry? GetEncounter(string id) => Find(Encounters, id, enc => enc.id);
public CrossingCrisisEntry? GetCrisis(string id) => Find(Crises, id, c => c.id);
public sealed class CrossingCatalogLoader
public const string FactionsFile = "crossing_factions.json";
public const string LocationsFile = "crossing_locations.json";
public const string QuestsFile = "crossing_quests.json";
public const string ItemsFile = "crossing_items.json";
public const string EncountersFile = "crossing_encounters.json";
public const float MinDanger = 3f;
public const float MaxDanger = 6f;
public const float MinRads = 8f;
public const float MaxRads = 25f;
public CrossingCatalog Load(string dataDirectory) {
public static class CrossingIds
public const string Expansion = "expansion_nobodys_charter";
public const string Region = "region_crossing";
public const string TheVouch = "quest_crossing_the_vouch";
public const string FirstWeigh = "quest_crossing_first_weigh";
public const string ScaleIntegrity = "quest_crossing_scale_integrity";
public const string TheStanding = "quest_crossing_the_standing";
public const string TheTerms = "quest_crossing_the_terms";
public const string ViaductGate = "loc_crossing_viaduct_gate";
public const string Scalehouse = "loc_crossing_scalehouse";
public const string Weighbridge = "loc_crossing_weighbridge";
public const string NpcMattis = "npc_mattis_cray";
public const string NpcOsran = "npc_osran_kell";
public const string NpcWyn = "npc_wyn_sabler";
public const string NpcIvo = "npc_ivo_fenn";
public const string FactionScale = "faction_the_scale";
public const string FactionUnderwrite = "faction_the_underwrite";
public const string FactionCompact = "faction_the_compact";
public const string FactionLamplighters = "faction_the_lamplighters";
public const string FactionGranaryWardens = "faction_the_granary_wardens";
public const string FactionWaterCommittee = "faction_the_water_committee";
public const string FactionQuarantinePost = "faction_the_quarantine_post";
public const string FactionSmugglersCourt = "faction_the_smugglers_court";
public static class Quests
public const string TheVouch   = "quest_crossing_the_vouch";
public const string FirstWeigh = "quest_crossing_first_weigh";
public const string TheTerms   = "quest_crossing_the_terms";
public const string ThePetition= "quest_crossing_the_petition";
public const string TheStanding= "quest_crossing_the_standing";
public const string TheMarker  = "quest_crossing_the_marker";
public const string TheForfeit = "quest_crossing_the_forfeit";
public const string TheVoteNot = "quest_crossing_the_vote_that_isnt";
public const string ScaleIntegrity = "quest_crossing_scale_integrity";
public const string CharterCore = "quest_crossing_three_dry_pages";
public const string WhoHoldsLedger = "quest_crossing_who_holds_the_ledger";
public const string CompanionMattis = "quest_crossing_companion_mattis";
public static class Locations
public const string ViaductGate  = "loc_crossing_viaduct_gate";
public const string Scalehouse   = "loc_crossing_scalehouse";
public const string Stallrow     = "loc_crossing_stallrow";
public const string Watchtower   = "loc_crossing_watchtower";
public const string Weighbridge  = "loc_crossing_weighbridge";
public const string Underwrite   = "loc_crossing_underwrite_hall";
public const string RecordsRoom  = "loc_crossing_records_room";
public const string Nightfire    = "loc_crossing_nightfire";
public const string TheLockup    = "loc_crossing_the_lockup";
public const string GranaryPledge = "loc_crossing_granary_pledge";
public const string PetitionTent = "loc_crossing_petition_tent";
public const string FoundersMarker = "loc_crossing_founders_marker";
public const string TheAnnex     = "loc_crossing_the_annex";
public static class Items
public const string VouchToken     = "item_vouch_token_crossing";
public const string CalibrationWeight = "item_calibration_weight";
public const string TradedGrain    = "item_crossing_traded_grain";
public const string TradedSalt     = "item_crossing_traded_salt";
public const string PledgeSlip     = "item_crossing_pledge_slip";
public const string CharterPages   = "item_charter_three_pages";
public const string DebtContractCopy = "item_debt_contract_copy";
public const string MarkerRubbing  = "item_marker_rubbing";
public const string DutyLogFragment = "item_duty_log_fragment";
public const string TradeManifestBlank = "item_trade_manifest_blank";
public const string WynReceiptPaid = "item_wyn_receipt_paid";
public static class Flags
public const string VouchedClean   = "flag_crossing_vouched_clean";
public const string VouchBurned    = "flag_crossing_vouch_burned";
public const string AccessSoftened = "flag_crossing_access_softened";
public const string UnderwriteUntested = "flag_crossing_underwrite_untested";
public const string PetitionUnsigned = "flag_crossing_petition_unsigned";
public const string StandingHonest = "flag_crossing_standing_honest";
public const string StandingRigged = "flag_crossing_standing_rigged";
public const string ExpansionUnlocked = "exp_nobodys_charter_unlocked";
public const string VouchRewarded = "flag_crossing_vouch_rewarded";
public static class Npcs
public const string OsranKell = "npc_osran_kell";
public const string MattisCray = "npc_mattis_cray";
public const string DessaVane = "npc_dessa_vane";
public const string PerrinAshby = "npc_perrin_ashby";
public const string IvoFenn = "npc_ivo_fenn";
public const string WynSabler = "npc_wyn_sabler";
public static class Knowledge
public const string TheVouch       = "lore_nc_the_vouch";
public const string ReadAgain      = "lore_nc_read_again";
public const string RubricAgain    = "lore_nc_the_rubric_again";
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs`

### `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 489 lines / 20000 bytes.
- SHA-256: `ae757ef1db3c06fc7c23741ada561df5e2b07ca1585de1c2e6578811bae2e1de`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=16; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class CrossingQuestStage
public class CrossingQuestChoice
public class CrossingQuestDef
public class CrossingStageNarrativeEvent
public string questId { get; set; } = "";
public string questDisplayName { get; set; } = "";
public int stageIndex { get; set; }
public string stageId { get; set; } = "";
public string stageText { get; set; } = "";
public string briefing { get; set; } = "";
public bool isCompletion { get; set; }
public class CrossingQuestProgress
public string questId = "";
public int currentStage;
public bool started;
public bool completed;
public bool failed;
public string chosenChoiceId = "";
public class CrossingQuestSystemState
public string systemId = CrossingQuestSystem.SystemId;
public int lastTickedDay;
public List<CrossingQuestProgress> quests = new();
public HashSet<string> setFlags = new();
public HashSet<string> dispatchedStageEvents = new();
public class CrossingQuestSystem
public const string SystemId = "crossing_quest_system";
public const string OpeningQuest = "quest_crossing_the_vouch";
public event Action<string, int> OnQuestStageChanged;
public event Action<string> OnQuestStarted;
public event Action<string> OnQuestCompleted;
public event Action<string> OnQuestFailed;
public event Action<string, string> OnFlagSet;
public event Action<CrossingStageNarrativeEvent> OnStageNarrativeEmitted;
public event Action<CrossingQuestSystemState> OnStateChanged;
public CrossingQuestSystemState State => _state;
public void BindCatalog(IReadOnlyList<CrossingQuestDef> catalog) {
public void BindMoralSystem(Ashfall.Core.MoralChoice.MoralChoiceSystem? moralSystem) {
public CrossingQuestDef? GetDef(string questId) {
public IReadOnlyList<CrossingQuestDef> Catalog => _catalog;
public void BindConsequenceLedger(IFlagLedger? ledger) {
public List<CrossingQuestDef> GetVisibleQuests(int currentDay) {
public List<CrossingQuestDef> GetEligibleQuests(int currentDay, bool hasVouchAccess = false) {
public List<CrossingQuestDef> GetAvailableQuests(int currentDay) => GetEligibleQuests(currentDay, false);
public bool IsQuestCompleted(string questId) {
public bool IsQuestFailed(string questId) {
public bool IsQuestStarted(string questId) {
public CrossingQuestProgress? GetProgress(string questId) {
public void TickDaily(int currentDay, bool hasVouchAccess = false) {
public bool StartQuest(string questId, int currentDay) {
public bool FailQuest(string questId) {
public int AdvanceStage(string questId) {
public bool MakeChoice(string questId, string choiceId) {
public bool HasFlag(string flag) => _state.setFlags.Contains(flag);
public event Action? OnOpeningQuestCompleted;
public CrossingQuestSystemState CaptureState() {
public void RestoreState(CrossingQuestSystemState? saved) {
public static class CrossingQuestCatalogLoader
public const string FileName = "crossing_quests.json";
public static List<CrossingQuestDef> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/VouchAccessSystem.cs`

### `Assets/Ashfall.Core/VouchAccessSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 120 lines / 4406 bytes.
- SHA-256: `459d9b877ef52ae64dbbd5179fd52e87d8431f8c3119975d640a6bd128261e92`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class VouchAccessSystemState
public string systemId = VouchAccessSystem.SystemId;
public string vouchedBy = "";
public bool vouchBurned;
public bool accessSoftened;
public bool lastResortUsed;
public class VouchAccessSystem
public const string SystemId = "vouch_access_system";
public const string FlagExpUnlocked = "exp_nobodys_charter_unlocked";
public event Action<string> OnVouchGranted;
public event Action OnVouchBurned;
public event Action OnAccessSoftened;
public event Action<VouchAccessSystemState> OnStateChanged;
public VouchAccessSystemState State => _state;
public bool RequiresVouch => !_state.accessSoftened
public bool HasAccess => !RequiresVouch;
public string VouchedBy => _state.vouchedBy;
public bool VouchBurned => _state.vouchBurned;
public bool AccessSoftened => _state.accessSoftened;
public bool LastResortUsed => _state.lastResortUsed;
public bool GrantVouch(string npcId, bool isLastResort = false) {
public bool BurnVouch() {
public bool SoftenAccess() {
public bool NeedsLastResort => RequiresVouch && _state.vouchBurned && !_state.lastResortUsed;
public VouchAccessSystemState CaptureState() {
public void RestoreState(VouchAccessSystemState saved) {
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/CrossingArbitrationSystem.cs`

### `Assets/Ashfall.Core/CrossingArbitrationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 489 lines / 20704 bytes.
- SHA-256: `13a36a288db0b04c398b1774dea0842564b935818e4edb1b099da951d383445f`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=12; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class BackerDef
public string id;
public string displayName;
public string wants;        // what motivates this backer
public string willNot;      // hard limit — will not cross
public bool principled;     // cannot be bribed; caps pure-buy rulings
public bool isAlive = true; // dead backers lose their hold
public class StandingRuling
public string topic;       // dispute subject (quest id or freeform)
public List<string> backers = new List<string>(); // backer ids holding this
public RulingShape shape;
public int dayCalled;
public List<string> bribedBackers = new List<string>(); // bought, not earned
public int bribeMarks; // public refusals / known-bought marks on this ruling
public List<string> refusedBribes = new List<string>(); // principled backers who refused publicly
public enum RulingShape
public enum BribeResult
public class CrossingArbitrationState
public string systemId = CrossingArbitrationSystem.SystemId;
public List<BackerDef> backerPool = new List<BackerDef>();
public List<StandingRuling> rulings = new List<StandingRuling>();
public int rulingsCalled;
public int rulingsOverturned;
public int standingRepeats; // re-Standings called after an overturn
public class CrossingArbitrationSystem
public const string SystemId = "crossing_arbitration_system";
public const int BackersToHold = 3;
public event Action<string> OnStandingCalled;          // topic
public event Action<StandingRuling> OnRulingMade;      // the ruling that now holds
public event Action<StandingRuling> OnRulingOverturned;
public event Action<string, string> OnBribeRefused;    // backerId, topic (public mark)
public event Action<CrossingArbitrationState> OnStateChanged;
public CrossingArbitrationState State => _state;
public IReadOnlyList<BackerDef> BackerPool => _state.backerPool;
public IReadOnlyList<StandingRuling> Rulings => _state.rulings;
public void LoadBackerPool(IReadOnlyList<BackerDef> defs) {
public BackerDef? GetBacker(string id) {
public StandingRuling? GetRuling(string topic) {
public List<StandingRuling> GetRulingHistory(string topic) {
public List<BackerDef> GetAvailableBackers(string topic) {
public bool IsRulingHeld(string topic) {
public bool IsRulingActive(string topic) {
public bool IsRulingOverturned(string topic) {
public bool CallStanding(string topic, int currentDay) {
public bool DeclareBacker(string topic, string backerId) {
public BribeResult TryBribeBacker(string topic, string backerId) {
public bool OverturnRuling(string topic, IReadOnlyList<string> counterBackerIds) {
public bool RemoveBacker(string backerId) {
public CrossingArbitrationState CaptureState() {
public void RestoreState(CrossingArbitrationState saved) {
```


# Appendix B.06 — Current Code Architecture: `Assets/Ashfall.Core/CrossingSession.cs`

### `Assets/Ashfall.Core/CrossingSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 47 lines / 1768 bytes.
- SHA-256: `ddd91860850fe8384837844dcf1a617d1aac8d55208c45416f82da32a0518cc4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CrossingSession
public VouchAccessSystem Vouch { get; }
public CrossingCatalog Catalog { get; }
public static CrossingSession Load(string dataDirectory, ILog? log = null) {
public bool GateAllowsCrossing() => Vouch != null && Vouch.HasAccess;
public static bool IsCrossingNode(string nodeId) =>
public bool IsTravelBlocked(string nodeId) {
public bool TryVouch(string npcId, bool lastResort = false) =>
public bool BurnVouch() => Vouch.BurnVouch();
public void SoftenAccess() => Vouch.SoftenAccess();
```


# Appendix B.07 — Current Code Architecture: `src/Host/ExpansionHostSession.cs`

### `src/Host/ExpansionHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 500 lines / 25698 bytes.
- SHA-256: `9809fca6ed766dfd903f6dcc7df79e68f779d539a85bbd8a66066710d570da45`.
- Architecture signals: seeded references=0; save/restore symbols=5; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpansionHostSession
public const int DefaultSeed = 1117; // greenhouse + vouch demo seed
public WaystationSystem Waystation { get; }
public LocationLayoutSystem Layouts { get; }
public LocationMemorySystem Memory { get; }
public SiteEncounterSystem SiteEncounters { get; }
public StandingRecordCatalog RecordQuests { get; }
public VouchAccessSystem Vouch { get; }
public GreenhouseSystem Greenhouse { get; private set; }
public CrossingArbitrationSystem Arbitration { get; }
public LedgerDebtSystem Ledger { get; }
public CrossingQuestSystem CrossingQuests { get; }
public GenerationalSuccessionEngine Generational { get; }
public EpilogueMatrixRuntime Epilogue { get; }
public DutyRosterSystem DutyRoster { get; private set; }
public Ashfall.Core.Foundry.SilentFoundrySystem SilentFoundry { get; private set; }
public Ashfall.Core.Foundry.SilentFoundryCatalog FoundryData { get; private set; }
public Ashfall.Core.Disease.DiseaseSystem Disease { get; private set; }
public Ashfall.Core.Disease.DiseaseCatalog DiseaseData { get; private set; }
public DebtTemplateCatalog? DebtCatalog { get; private set; }
public DebtConsequenceDispatcher? DebtDispatcher { get; private set; }
public FactionEmbargoLedger Embargoes { get; } = new FactionEmbargoLedger();
public void BindDutyRoster(DutyRosterSystem roster) {
public void BindGreenhouse(GreenhouseSystem shared) {
public event Action<CrossingStageNarrativeEvent>? OnCrossingStageNarrative;
public static ExpansionHostSession Create( string dataDirectory, ILog log = null!, Ashfall.Core.Flags.IFlagLedger? consequenceLedger = null) {
public void ShutdownDebtIntegration() {
public override void Dispose() {
public ExpansionHubSave CaptureSave(int simDay, DebtConsequenceBridgeState? debtBridge = null) =>
public void RestoreSave(ExpansionHubSave save, DebtConsequenceHostBridge? debtBridge = null) =>
public void LoadDefaultBackerPool() {
public string ArbitrationLine() {
public string LedgerLine() {
public void UnlockWaystation() => Waystation.Unlock();
public void SetWaystationWintering(bool wintering) => Waystation.SetWintering(wintering);
public bool AssignWaystationWatch(string[] ids) => Waystation.AssignWatch(ids);
public void ResupplyWaystation() => Waystation.Resupply();
public void TickWaystation(bool iceRoadOpen) => Waystation.TickDaily(iceRoadOpen);
public string WaystationLine() {
public void UnlockRecord() {
public bool ArriveAtSite(string parentId) => Layouts.ArriveAtParent(parentId);
public bool EnterSiteRoom(string roomId) => Layouts.EnterRoom(roomId);
public bool InspectSiteRoom(string roomId) => Layouts.InspectRoom(roomId);
public string RoomLine(string parentId, string roomId) {
public string StandingRecordLine() {
public string RecordQuestLine() {
public bool GrantVouch(string npcId) => Vouch.GrantVouch(npcId, isLastResort: false);
public bool BurnVouch() => Vouch.BurnVouch();
public bool SoftenAccess() => Vouch.SoftenAccess();
public string CrossingLine() {
public bool StartCrossingQuest(string questId, int currentDay) => CrossingQuests.StartQuest(questId, currentDay);
public void TickCrossingQuests(int currentDay) => CrossingQuests.TickDaily(currentDay, hasVouchAccess: Vouch.HasAccess);
public int AdvanceCrossingQuestStage(string questId) => CrossingQuests.AdvanceStage(questId);
public bool MakeCrossingChoice(string questId, string choiceId) => CrossingQuests.MakeChoice(questId, choiceId);
public List<CrossingQuestDef> GetAvailableCrossingQuests(int currentDay) => CrossingQuests.GetAvailableQuests(currentDay);
public bool FailCrossingQuest(string questId) => CrossingQuests.FailQuest(questId);
public bool IsCrossingQuestFailed(string questId) => CrossingQuests.IsQuestFailed(questId);
public bool IsCrossingQuestCompleted(string questId) => CrossingQuests.IsQuestCompleted(questId);
public string CrossingQuestLine() {
public void EnsureGreenhousePlots(int count) => Greenhouse.EnsurePlots(count);
public bool PlantGreenhouse(int plotIndex, string seedItemId, int day) => Greenhouse.Plant(plotIndex, seedItemId, day, out _);
public void WaterGreenhouse(int plotIndex, float units) => Greenhouse.Water(plotIndex, units, tainted: false);
public GreenhouseHarvest HarvestGreenhouse(int plotIndex) => Greenhouse.Harvest(plotIndex);
public void TickGreenhouse(int simDay) =>
public string GreenhouseLine() {
public void RegisterGenerationDweller(string dwellerId, int age, int generation = 0) => Generational.RegisterDweller(dwellerId, age, generation);
public string AdvanceGenerationalTime(int days) {
public string FormMentorshipDemo(string mentorId, string apprenticeId, string traitId) {
public string GenerationalLine() {
public GenerationalSuccessionSaveState CaptureGenerationalSave() => Generational.CaptureState();
public void RestoreGenerationalSave(GenerationalSuccessionSaveState state) => Generational.RestoreState(state);
public string GenerateEpilogueNarrativeDemo(EpilogueEvaluationContext ctx) => Epilogue.GenerateEpilogueNarrative(ctx);
```


# Appendix B.08 — Current Code Architecture: `src/UI/CrossingQuestPanel.cs`

### `src/UI/CrossingQuestPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 493 lines / 22286 bytes.
- SHA-256: `04e9b7fbf0c954b832ccda943f6bb59e98f4bad06478aa9043548f24e92d71d3`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=3; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class CrossingQuestPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _expansions != null;
public void Bind(ExpansionHostSession expansions, VouchAccessSystem? vouch, int currentDay) {
public void Open() {
public override void _Ready() {
public override void _UnhandledInput(InputEvent @event) {
public void RefreshView() {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix B.09 — Current Code Architecture: `src/Main.GameFlow.cs`

### `src/Main.GameFlow.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 950 lines / 43281 bytes.
- SHA-256: `2fdc11c80e9e23418cf05a6d8dbd621c66c8d6aba34503e7cde474922c403685`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=2; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix B.10 — Current Code Architecture: `src/Main.ExpansionHub.cs`

### `src/Main.ExpansionHub.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 350 lines / 15578 bytes.
- SHA-256: `96c98117160a382967b4feba1bea917cf4322bd4d04e46754f7eb082f95b24ec`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=0; textual Godot mentions=5; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/crossing_factions.json`

### `Assets/StreamingAssets/Data/crossing_factions.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 6289 bytes / 6281 characters.
- SHA-256: `71f072a6dd213655a8ea3c2fbebbc617d31491ce3348166509517fe367ef27ec`.
- Root keys: `actions`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
actions: min=8, max=8, observed_paths=1
actions[].offers: min=2, max=3, observed_paths=2
actions[].wants: min=1, max=1, observed_paths=2
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
faction_the_scale
faction_the_underwrite
faction_the_compact
faction_the_lamplighters
faction_the_granary_wardens
faction_the_water_committee
faction_the_quarantine_post
faction_the_smugglers_court
```


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/crossing_quests.json`

### `Assets/StreamingAssets/Data/crossing_quests.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 34791 bytes / 34791 characters.
- SHA-256: `94d5932902ac39bdac0f2caa6c7e56a1bdb33247063e397ca9d2ac5949315734`.
- Root keys: `quests`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
quests: min=23, max=23, observed_paths=1
quests[].choices: min=2, max=3, observed_paths=2
quests[].stages: min=4, max=5, observed_paths=2
```

Representative record fields:

- `briefing`
- `choices`
- `display_name`
- `id`
- `knowledge_key`
- `min_day`
- `prereq_quest_id`
- `stages`
- `target_location_id`
- `type`

Representative identifiers (ordered, capped for readability):

```text
quest_crossing_the_vouch
quest_crossing_first_weigh
quest_crossing_scale_integrity
quest_crossing_the_terms
quest_crossing_the_petition
quest_crossing_the_standing
quest_crossing_the_marker
quest_crossing_the_forfeit
quest_crossing_the_vote_that_isnt
quest_crossing_three_dry_pages
quest_crossing_who_holds_the_ledger
quest_crossing_companion_mattis
quest_crossing_asylum_in_the_truss
quest_crossing_contraband_medical_vial
quest_crossing_vehicle_lien_arbitration
quest_crossing_displaced_kin_roll
quest_crossing_quarantine_breach_trial
quest_crossing_flotilla_docking_rights
quest_crossing_embargo_transit_escort
quest_crossing_the_null_charter_vote
quest_crossing_the_salvaged_accord
quest_crossing_the_registry_dispute
quest_crossing_the_long_toll
```


# Appendix C.13 — Catalog Census: `Assets/StreamingAssets/Data/crossing_encounters.json`

### `Assets/StreamingAssets/Data/crossing_encounters.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 28037 bytes / 28037 characters.
- SHA-256: `02ccdbff76776a6b5653a065640e59f5c1d7ddb9e3cfe31968e89338cb31423c`.
- Root keys: `crises`, `encounters`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
crises: min=12, max=12, observed_paths=1
crises[].phases: min=4, max=4, observed_paths=2
encounters: min=25, max=25, observed_paths=1
encounters[].choices: min=2, max=2, observed_paths=2
encounters[].choices[].cost_items: min=1, max=1, observed_paths=2
```

Representative record fields:

- `choices`
- `description`
- `id`
- `name`
- `target_location`
- `threat_level`

Representative identifiers (ordered, capped for readability):

```text
enc_nc_collector_visit
enc_nc_backer_pressure
enc_nc_lockup_muscle
enc_nc_iron_raiders_scout
enc_nc_deserter_passage
enc_nc_scavenger_dispute
enc_nc_grain_exchange_envoy
enc_nc_sun_seekers_pass
enc_nc_forfeit_witness
enc_nc_standing_ambush
enc_nc_mass_crossing_surge
enc_nc_garrison_iron_blockade
enc_nc_pestilence_quarantine_lockdown
enc_nc_syndicate_bribe_overture
enc_nc_bonded_caravan_ambush
enc_nc_smuggler_checkpoint
enc_nc_frozen_barge
enc_nc_toll_bridge_claim
enc_nc_collapsed_crossing
enc_nc_contaminated_ford
enc_nc_ice_fracture
enc_nc_uxo_field_crossing
enc_nc_refugee_blockade
enc_nc_family_ledger_gate
enc_nc_child_at_gate
```


# Appendix C.14 — Catalog Census: `Assets/StreamingAssets/Data/standing_record_factions.json`

### `Assets/StreamingAssets/Data/standing_record_factions.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 6222 bytes / 6222 characters.
- SHA-256: `65d731c51c28e43ec37bdae1c1f9fd4e3c73ac0e867594ceb82a04e9db54a68b`.
- Root keys: `actions`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
actions: min=8, max=8, observed_paths=1
actions[].offers: min=2, max=3, observed_paths=2
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
faction_the_overlay
faction_the_scale
faction_the_compact
faction_the_underwrite
faction_the_cutters
faction_the_fleet
faction_the_rebuilders
faction_the_garrison
```


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/CrossingFactionExpansionTests.cs`

### `Ashfall.Core.Tests/CrossingFactionExpansionTests.cs`

- Current test declarations: Fact=16, Theory=0, InlineData=0.
- File lines: 353; SHA-256: `1bb5e0d2b6d7c7297e4ef0ef0c990f1f93660599bdea2268843187d64f958520`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsSuccessfully_ContainsExactEightFactions
BaselineThreeFactions_PreservedVerbatim
NewFiveFactions_ArePresentWithExpectedAttributes
FactionIds_AreUnique_AndStartWithPrefix
DisplayNames_AreNonEmpty_AndDistinct
Alignments_AreValid
HomeRegions_AreValidCrossingRegion
IsActive_IsTrueForAll
Trust_ValuesAreWithinValidRange
Wants_AreNonEmptyArrays_WithValidNonEmptyItems
Offers_AreNonEmptyArrays_WithValidNonEmptyItems
SignatureQuotes_AreNonEmpty_SingleSentences
AccessRules_AreNonEmpty_AndSubstantive
TradeProfiles_AreDistinctAcrossAllEight
CrossingIds_ConstantsMatchDataCatalog
FactionIconCatalog_ResolvesOrFallsBackSafely
```


# Appendix D.16 — Existing Focused Test Inventory: `Ashfall.Core.Tests/World/Plan118_120StandingCrossingIntegrationTests.cs`

### `Ashfall.Core.Tests/World/Plan118_120StandingCrossingIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 239; SHA-256: `36858a772bf3a9b288f691c8ce875f00e2dd2d87b2535111569166067220e739`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan118_StandingRecordQuests_LoadsAllAuthoredQuestsAndVerifiesPlan118TenExpansionQuests
Plan120_CrossingFactions_LoadsExactEightFactionsWithDistinctTradeProfiles
Plan118_120_CrossDomainCoherence_BorderTerritoryAndLamplighterLinkages
Plan118_120_DeterministicQuestChoiceResolutionAndFactionTrust
```


# Appendix D.17 — Existing Focused Test Inventory: `Ashfall.Core.Tests/World/Plan126_129CrossingFoundryIntegrationTests.cs`

### `Ashfall.Core.Tests/World/Plan126_129CrossingFoundryIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 233; SHA-256: `aa4f405426b00ae803ede9eb65259a230dcd601dee7ed142ef8d794a56f1beb2`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan126_CrossingItems_LoadsExactTwentyFiveItemsAndValidatesExpansionTier
Plan129_FoundryProduction_LoadsExactThirtyFiveProductsAndValidatesNineExpansionProducts
Plan126_129_CrossDomainCoherence_IndustrialSupplyChainAndBorderCommerceLinkages
Plan126_129_DeterministicInventoryTradeAndProductionSimulation
```


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/ExpansionHubSave.cs`

### `Assets/Ashfall.Core/ExpansionHubSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 507 lines / 28559 bytes.
- SHA-256: `6f9efccbfed1c5101889fb1aeff9b33800d110d217b1f669924bbbfdf2d1e282`.
- Architecture signals: seeded references=0; save/restore symbols=31; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class ExpansionHubSave
public const int CurrentSaveVersion = 6;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public DebtDispatcherState debtDispatcher = new DebtDispatcherState();
public FactionEmbargoLedgerState embargoes = new FactionEmbargoLedgerState();
public DebtConsequenceBridgeState debtBridge = new DebtConsequenceBridgeState();
public SaltMineState saltMine = new SaltMineState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV5
public int saveVersion = 5;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public DebtDispatcherState debtDispatcher = new DebtDispatcherState();
public FactionEmbargoLedgerState embargoes = new FactionEmbargoLedgerState();
public DebtConsequenceBridgeState debtBridge = new DebtConsequenceBridgeState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV1
public int saveVersion = 1;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV2
public int saveVersion = 2;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV3
public int saveVersion = 3;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV4
public int saveVersion = 4;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public string Checksum = string.Empty;
public static class ExpansionHubSaveCodec
public static ExpansionHubSave Capture( int simDay, WaystationSystem waystation, LocationLayoutSystem layouts, LocationMemorySystem memory, SiteEncounterSystem siteEncounters,
public static string Encode(ExpansionHubSave save, IJsonSerializer json) {
public static ExpansionHubSave Decode(string jsonText, IJsonSerializer json) {
public static void Restore( ExpansionHubSave save, WaystationSystem waystation, LocationLayoutSystem layouts, LocationMemorySystem memory, SiteEncounterSystem siteEncounters,
```


# Appendix E.19 — Supporting Code Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix E.20 — Supporting Code Evidence: `src/Host/ExpansionHubSaveStore.cs`

### `src/Host/ExpansionHubSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 58 lines / 2955 bytes.
- SHA-256: `ad69579b59d44fe320c4ffd7d2b7e77242246bd79b7a1e0068d4ceb51aac2945`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class ExpansionHubSaveStore
public const string FileName = "expansion_hub_save.json";
public const string SectionName = "expansion_hub";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(ExpansionHubSave state) => s_store.CaptureBare(state);
public static ExpansionHubSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(ExpansionHubSave state) => s_store.CaptureBare(state);
public static ExpansionHubSave? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(ExpansionHubSave save, string pathOverride = null!) =>
public static ExpansionHubSave? TryLoad(string pathOverride = null!) =>
public static string TryCapturePersisted(ExpansionHubSave save) => s_store.CapturePersisted(save);
```


# Appendix E.21 — Supporting Code Evidence: `Assets/Ashfall.Core/StandingRecord/StandingRecordCatalog.cs`

### `Assets/Ashfall.Core/StandingRecord/StandingRecordCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 160 lines / 5625 bytes.
- SHA-256: `fcdff64804e412e7424336f39d2c688b711770a3804038ccfc8e2a1ad27ee7b7`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class StandingRecordQuestStageEntry
public string id;
public string text;
public class StandingRecordQuestChoiceEntry
public string id;
public string text;
public string set_flag;
public class StandingRecordQuestEntry
public string id;
public string display_name;
public string type;
public string briefing;
public string prereq_quest_id;
public int min_day;
public StandingRecordQuestStageEntry[] stages;
public StandingRecordQuestChoiceEntry[] choices;
public string knowledge_key;
public string target_location_id;
public string complete_mutation;
public string fail_mutation;
public int StageCount => stages != null ? stages.Length : 0;
public class StandingRecordFactionEntry
public string id;
public string display_name;
public string alignment;
public string home_region;
public bool is_active;
public int trust;
public string[] wants;
public string[] offers;
public string signature_quote;
public string access_rule;
public string badge_asset_id;
public sealed class StandingRecordCatalog
public List<StandingRecordQuestEntry> Quests { get; } = new List<StandingRecordQuestEntry>();
public List<StandingRecordFactionEntry> Factions { get; } = new List<StandingRecordFactionEntry>();
public StandingRecordQuestEntry? GetQuest(string id) {
public StandingRecordFactionEntry? GetFaction(string id) {
public sealed class StandingRecordCatalogLoader
public const string QuestsFile = "standing_record_quests.json";
public const string FactionsFile = "standing_record_factions.json";
public StandingRecordCatalog Load(string dataDirectory) {
```


# Appendix E.22 — Supporting Code Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 689 lines / 34425 bytes.
- SHA-256: `0f779f80dada6c90e5cb5bfe305b5a9cfe253a6d12036b85044329a3ffff2caa`.
- Architecture signals: seeded references=6; save/restore symbols=0; typed event declarations=20; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=1; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed partial class SilentFoundrySystem
public const int DefaultSeed = 1009;
public const int MaxWorkers = 8;            // room_bp_11 max_dweller_capacity
public const float BlueprintBasePowerKw = 45f;
public const float BlueprintWaterFlowLpm = 40f;
public const string EventUnlocked = "silent_foundry_unlocked";
public const string EventRepairStarted = "silent_foundry_repair_started";
public const string EventRepaired = "silent_foundry_repaired";
public const string EventMaintenanceDue = "silent_foundry_maintenance_due";
public const string EventHeatPrepared = "silent_foundry_heat_prepared";
public const string EventHeatStarted = "silent_foundry_heat_started";
public const string EventHeatCompleted = "silent_foundry_heat_completed";
public const string EventCastCompleted = "silent_foundry_cast_completed";
public const string EventCastFailed = "silent_foundry_cast_failed";
public const string EventSafetyWarning = "silent_foundry_safety_warning";
public const string EventIncident = "silent_foundry_incident";
public const string EventTreatyQuotaMet = "silent_foundry_treaty_quota_met";
public const string EventTreatyQuotaMissed = "silent_foundry_treaty_quota_missed";
public const string EventConsequenceApplied = "silent_foundry_treaty_consequence_applied";
public const string EventLaborDispute = "silent_foundry_labor_dispute";
public const string EventStrikeStarted = "silent_foundry_strike_started";
public const string EventStrikeResolved = "silent_foundry_strike_resolved";
public const string EventBlueprintReferenced = "silent_foundry_blueprint_referenced";
public const string EventJournalTriggered = "silent_foundry_journal_triggered";
public event Action<SilentFoundryState> OnStateChanged;
public event Action<FoundryProductionRecord> OnProductionCompleted;
public event Action<FoundryFailedCastRecord> OnCastFailed;
public event Action<string> OnSafetyWarning;
public event Action<FoundryIncidentRecord> OnIncident;
public event Action<FoundryTreatyCompliance> OnTreatyQuotaMet;
public event Action<FoundryTreatyCompliance> OnTreatyQuotaMissed;
public event Action<FoundryConsequenceRecord> OnConsequenceApplied;
public event Action<FoundryLaborDispute, int> OnLaborDisputeChanged;
public event Action<FoundryStrikeResolution, int> OnStrikeResolved;
public event Action<FoundryJournalTrigger> OnJournalTriggered;
public event Action<string> OnEventRaised;
public const float StandingMin = -100f;
public const float StandingMax = 100f;
public const float StandingNeutral = 0f;
public void BindCatalog(SilentFoundryCatalog catalog, int maintenanceCycleDaysFromBlueprint) {
public void BindTreaties(IReadOnlyDictionary<string, int> ratificationDaysById) {
public void BindConsequencePolicy(SilentFoundryConsequencePolicyCatalog policy) {
public void BindInventory( Func<string, int> getCount, Func<string, int, bool> canAdd, Action<string, int> addItem, Action<string, int> consume) {
public SilentFoundryState State => _state;
public SilentFoundryCatalog Catalog => _catalog;
public bool IsUnlocked => _state.unlocked;
public FoundryHeatStage HeatStage => _state.heatStage;
public FoundryLaborDispute LaborDispute => _state.laborDispute;
public bool IsMaintenanceOverdue => _state.daysSinceMaintenance > _state.maintenanceCycleDays;
public int DaysOverdue => Math.Max(0, _state.daysSinceMaintenance - _state.maintenanceCycleDays);
public int OverdueCycles => _state.maintenanceCycleDays > 0
public float GetComponentCondition(FoundryFacilityComponent component) {
public float AverageFacilityCondition() {
public bool IsJournalTriggered(string templateId) =>
public FoundryTreatyCompliance? GetTreatyCompliance(string treatyId) {
public IReadOnlyList<FoundryProductionRecord> CompletedProduction => _state.completed;
public IReadOnlyList<FoundryFailedCastRecord> FailedCasts => _state.failed;
public IReadOnlyList<FoundryIncidentRecord> Incidents => _state.incidents;
public int TotalProductionCount => _state.completed.Count;
public int TotalFailedCount => _state.failed.Count;
public float CumulativeStress => _state.cumulativeStress;
public float CumulativeHope => _state.cumulativeHope;
public bool IsHeatActive => HeatStage != FoundryHeatStage.Idle && HeatStage != FoundryHeatStage.Complete;
public float CurrentPowerDemandKw => HeatStage switch
public float CurrentWasteHeatKw => HeatStage switch
public void SuspendHeat(string reason, int day) {
public float GuildStanding => _consequenceState.guildStanding;
public IReadOnlyList<FoundryConsequenceRecord> AppliedConsequences => _consequenceState.applied;
public bool IsConsequenceApplied(string treatyId, int cycleMarker) => _consequenceState.IsApplied(treatyId, cycleMarker);
public FoundryTreatyOutcome GetTreatyOutcome(string treatyId, int day) {
public bool Unlock(int day) {
public string StartRepair(FoundryFacilityComponent component, int day) {
public string PerformMaintenance(int day) {
public string PrepareSand(int waterLitres) {
public string CompactMold(float skill) {
public void AssessTreatyCompliance(int day) {
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/SilentFoundrySystemTests.cs`

### `Ashfall.Core.Tests/SilentFoundrySystemTests.cs`

- Current test declarations: Fact=35, Theory=0, InlineData=0.
- File lines: 916; SHA-256: `4f3c37bec6f3232e20309bddda99525206c3c11d3d34f775792865f5118adfd8`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Identity_ExactIdsResolve
Catalog_LoadsAllAuthoredProductsAndFaction
Catalog_QuotaProductsMapToExactTreaties
JournalTemplates_LoadWithExpansionIsolation
JournalDeltas_MatchAuthoredTemplate
Blueprint_ResolvesAndAnchorsMaintenanceCycle
Treaties_GuildIsExactSignatoryOfTenFoundryTreaties
Unlock_IsIdempotentAndRaisesOnce
Repair_ConsumesFirebrickAndRestoresComponent
Maintenance_FourDayCycleAndOverdueConsequences
SandPrep_ConsumesSandAndWaterAndImprovesBed
SandPrep_BlocksWithoutSandOrWater
Production_StartValidatesChargeAndConsumesResources
Production_MissingChargeGivesVisibleReason
Production_FirstHeatCompletesAndTriggersJournalOnce
Production_SecondHeatDoesNotRetriggerJournal
Production_UntappedHeatBurnsOutAndRecordsFailure
Production_QualityTiersAreDeterministicPerSeed
Safety_WarningsSurfaceBeforeIrreversibleTap
Incident_SameSeedSameOutcome
Incident_IsNeverHiddenAndLeavesARecord
Incident_WellMaintainedFurnaceNeverIncidents
Treaty_RailQuotaMetOnDeadline
Treaty_RailQuotaMissedOnDeadline
Treaty_LaborShiftViolationWhenStrikeOrOvertime
Treaty_RatificationDaysAreNotAssessedBeforeRatification
Strike_FatigueAloneDoesNotTriggerDispute
Strike_ProductionPressurePlusShiftGrievanceTriggersAndEscalates
Strike_ResolutionIsTypedAndOnceOnly
Save_RoundTripPreservesAllFoundryState
Save_ActiveFurnaceSurvivesRoundTrip
Save_MissingFoundryStateDefaultsSafely
Save_ChecksumStableAcrossHostSerializers
Save_ExpansionHubEnvelopeRoundTripsWithMigration
Events_EmitExactlyOncePerOutcome
```


# Appendix G.24 — Supporting Regression Evidence: `Ashfall.Core.Tests/VouchAccessSystemTests.cs`

### `Ashfall.Core.Tests/VouchAccessSystemTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 190; SHA-256: `236377cd22ed66a99e1e9d89da227812caba949bcbb257ed01d20726efa8bda0`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
FreshSystem_GateClosed
GrantVouch_OpensGate_AndRaisesEvent
GrantVouch_AgainWhenClean_IsIdempotent
GrantVouch_NullOrEmpty_IsRejected
BurnVouch_ReclosesGate_AndRaisesEvent
BurnVouch_WhenNeverVouched_IsNoOp
AfterBurn_NewVouch_RestoresAccess_AndLastResortFlagged
SoftenAccess_RequiresAName_AndIsIdempotent
SoftenAccess_AfterABurnedVouch_IsAllowed
NeedsLastResort_OnlyAfterABurnedFirstVouch
SaveRoundTrip_PreservesVouchState
RestoreState_IsIdempotent_AndNullSafe
StateChangedFiresOnGrantBurnSoften
```


# Appendix G.25 — Supporting Regression Evidence: `Ashfall.Core.Tests/ExpansionHubSaveTests.cs`

### `Ashfall.Core.Tests/ExpansionHubSaveTests.cs`

- Current test declarations: Fact=2, Theory=0, InlineData=0.
- File lines: 111; SHA-256: `30dc4f12a27814d92b4b08b6643fe7c146d682b263eae931e97634ceee7a1b9e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RoundTrip_RestoresEveryHubSurface
Decode_RejectsTamperedChecksum
```


# Appendix G.26 — Supporting Regression Evidence: `Ashfall.Core.Tests/ExpansionHubSaveV5Tests.cs`

### `Ashfall.Core.Tests/ExpansionHubSaveV5Tests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 172; SHA-256: `878b1924029b83ae46c9892ee3cf1e352b71892bf73409b704516805cc637404`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
DebtSections_RoundTripThroughTheEnvelope
V4Save_MigratesForward_WithEmptyDebtState
V5Save_MigratesForward_WithEmptySaltMine
SaltMine_RoundTripsThroughTheEnvelope
```


# Appendix H.27 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

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
| faction/location/quest/item/encounter definitions | CrossingCatalogLoader | live Crossing quest progression and eligibility | CrossingQuestSystem | Owner emits/reads a typed fact; no mirror state. |
| faction/location/quest/item/encounter definitions | CrossingCatalogLoader | live gate/access state | VouchAccessSystem | Owner emits/reads a typed fact; no mirror state. |
| faction/location/quest/item/encounter definitions | CrossingCatalogLoader | political backing/ruling state | CrossingArbitrationSystem | Owner emits/reads a typed fact; no mirror state. |
| faction/location/quest/item/encounter definitions | CrossingCatalogLoader | live host composition and expansion-hub save | ExpansionHostSession | Owner emits/reads a typed fact; no mirror state. |
| faction/location/quest/item/encounter definitions | CrossingCatalogLoader | player-facing quest/access projection | Crossing UI | Owner emits/reads a typed fact; no mirror state. |
| faction/location/quest/item/encounter definitions | CrossingCatalogLoader | catalog and cross-system proof | Crossing focused tests | Owner emits/reads a typed fact; no mirror state. |
| live Crossing quest progression and eligibility | CrossingQuestSystem | faction/location/quest/item/encounter definitions | CrossingCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| live Crossing quest progression and eligibility | CrossingQuestSystem | live gate/access state | VouchAccessSystem | Owner emits/reads a typed fact; no mirror state. |
| live Crossing quest progression and eligibility | CrossingQuestSystem | political backing/ruling state | CrossingArbitrationSystem | Owner emits/reads a typed fact; no mirror state. |
| live Crossing quest progression and eligibility | CrossingQuestSystem | live host composition and expansion-hub save | ExpansionHostSession | Owner emits/reads a typed fact; no mirror state. |
| live Crossing quest progression and eligibility | CrossingQuestSystem | player-facing quest/access projection | Crossing UI | Owner emits/reads a typed fact; no mirror state. |
| live Crossing quest progression and eligibility | CrossingQuestSystem | catalog and cross-system proof | Crossing focused tests | Owner emits/reads a typed fact; no mirror state. |
| live gate/access state | VouchAccessSystem | faction/location/quest/item/encounter definitions | CrossingCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| live gate/access state | VouchAccessSystem | live Crossing quest progression and eligibility | CrossingQuestSystem | Owner emits/reads a typed fact; no mirror state. |
| live gate/access state | VouchAccessSystem | political backing/ruling state | CrossingArbitrationSystem | Owner emits/reads a typed fact; no mirror state. |
| live gate/access state | VouchAccessSystem | live host composition and expansion-hub save | ExpansionHostSession | Owner emits/reads a typed fact; no mirror state. |
| live gate/access state | VouchAccessSystem | player-facing quest/access projection | Crossing UI | Owner emits/reads a typed fact; no mirror state. |
| live gate/access state | VouchAccessSystem | catalog and cross-system proof | Crossing focused tests | Owner emits/reads a typed fact; no mirror state. |
| political backing/ruling state | CrossingArbitrationSystem | faction/location/quest/item/encounter definitions | CrossingCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| political backing/ruling state | CrossingArbitrationSystem | live Crossing quest progression and eligibility | CrossingQuestSystem | Owner emits/reads a typed fact; no mirror state. |
| political backing/ruling state | CrossingArbitrationSystem | live gate/access state | VouchAccessSystem | Owner emits/reads a typed fact; no mirror state. |
| political backing/ruling state | CrossingArbitrationSystem | live host composition and expansion-hub save | ExpansionHostSession | Owner emits/reads a typed fact; no mirror state. |
| political backing/ruling state | CrossingArbitrationSystem | player-facing quest/access projection | Crossing UI | Owner emits/reads a typed fact; no mirror state. |
| political backing/ruling state | CrossingArbitrationSystem | catalog and cross-system proof | Crossing focused tests | Owner emits/reads a typed fact; no mirror state. |
| live host composition and expansion-hub save | ExpansionHostSession | faction/location/quest/item/encounter definitions | CrossingCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| live host composition and expansion-hub save | ExpansionHostSession | live Crossing quest progression and eligibility | CrossingQuestSystem | Owner emits/reads a typed fact; no mirror state. |
| live host composition and expansion-hub save | ExpansionHostSession | live gate/access state | VouchAccessSystem | Owner emits/reads a typed fact; no mirror state. |
| live host composition and expansion-hub save | ExpansionHostSession | political backing/ruling state | CrossingArbitrationSystem | Owner emits/reads a typed fact; no mirror state. |
| live host composition and expansion-hub save | ExpansionHostSession | player-facing quest/access projection | Crossing UI | Owner emits/reads a typed fact; no mirror state. |
| live host composition and expansion-hub save | ExpansionHostSession | catalog and cross-system proof | Crossing focused tests | Owner emits/reads a typed fact; no mirror state. |
| player-facing quest/access projection | Crossing UI | faction/location/quest/item/encounter definitions | CrossingCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| player-facing quest/access projection | Crossing UI | live Crossing quest progression and eligibility | CrossingQuestSystem | Owner emits/reads a typed fact; no mirror state. |
| player-facing quest/access projection | Crossing UI | live gate/access state | VouchAccessSystem | Owner emits/reads a typed fact; no mirror state. |
| player-facing quest/access projection | Crossing UI | political backing/ruling state | CrossingArbitrationSystem | Owner emits/reads a typed fact; no mirror state. |
| player-facing quest/access projection | Crossing UI | live host composition and expansion-hub save | ExpansionHostSession | Owner emits/reads a typed fact; no mirror state. |
| player-facing quest/access projection | Crossing UI | catalog and cross-system proof | Crossing focused tests | Owner emits/reads a typed fact; no mirror state. |
| catalog and cross-system proof | Crossing focused tests | faction/location/quest/item/encounter definitions | CrossingCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| catalog and cross-system proof | Crossing focused tests | live Crossing quest progression and eligibility | CrossingQuestSystem | Owner emits/reads a typed fact; no mirror state. |
| catalog and cross-system proof | Crossing focused tests | live gate/access state | VouchAccessSystem | Owner emits/reads a typed fact; no mirror state. |
| catalog and cross-system proof | Crossing focused tests | political backing/ruling state | CrossingArbitrationSystem | Owner emits/reads a typed fact; no mirror state. |
| catalog and cross-system proof | Crossing focused tests | live host composition and expansion-hub save | ExpansionHostSession | Owner emits/reads a typed fact; no mirror state. |
| catalog and cross-system proof | Crossing focused tests | player-facing quest/access projection | Crossing UI | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Retain the eight-row catalog and current loader as immutable content authority. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Add a single live `CrossingCatalog` binding to `ExpansionHostSession`/the Crossing host composition, without creating a second catalog loader. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Project faction identity, wants/offers, trust and access rules in the existing Crossing panel as truthful read-only information plus explicit available actions only where current quest/vouch/arbitration owners support them. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Add authored references for the five new factions only to current quest/encounter/standing-record schemas after verifying their fields and consumer semantics. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-05 | Keep faction trust/access mechanics owned by existing Crossing systems; if persistent faction trust is required, name the existing owner and save seam before implementation. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.559 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Glassworks.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Glassworks.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 19 lines / 711 bytes.
- SHA-256: `3bf5639e932b64fb2b17a8825e81c15243f02ef9d5e57091efb8e96a857352d0`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed partial class SilentFoundrySystem
public void BindGlassworksCatalog(GlassworksCatalog catalog) {
```


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Heat.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Heat.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 548 lines / 25964 bytes.
- SHA-256: `01dd24634d0735a7591acf9174c42c2382bf7d9c7bf8c374e937ebd752d9b7c0`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=7; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed partial class SilentFoundrySystem
public string StartProduction(string productId, int workers, float workerSkill, int day) {
public string TapAndCast(int day) {
public List<string> GetSafetyWarnings() {
public int ComputeIncidentChance() {
public void SetOvertime(bool overtime) { _state.overtimeFlag = overtime; RaiseStateChanged(); }
public void SetChildLaborUsed(bool used) { _state.childLaborUsed = used; RaiseStateChanged(); }
public string BeginLaborDispute(int day) {
public bool EscalateToStrike(int day) {
public string ResolveStrike(FoundryStrikeResolution resolution, int day) {
public void TickDaily(int day) {
public static FoundryQualityTier QualityTier(float quality) {
```


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Material.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Material.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 332 lines / 15293 bytes.
- SHA-256: `056bec67748c50875071fa3b1338257989f4e41e0f4f53091b6f45dfd1f25bb3`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum FoundryPurityTier
public static class FoundryPurityNames
public const string Poor = "Poor";
public const string Standard = "Standard";
public const string High = "High";
public const string Exceptional = "Exceptional";
public static string Name(FoundryPurityTier tier) => tier switch
public enum FoundryForgingCommand
public sealed class FoundryForgingSessionState
public string productOutputItemId = string.Empty;   // the batch output being worked
public string materialProfileId = string.Empty;
public string purity = FoundryPurityNames.Standard;
public List<string> submitted = new List<string>();  // authored command names
public int startedDay = 0;
public bool completed = false;
public int finalQualityPermille = 0;
public readonly struct FoundryForgingResult
public readonly bool Accepted;
public readonly string ProductOutputItemId;
public readonly int FinalQualityPermille;
public readonly FoundryPurityTier Purity;
public readonly int MatchedCommands;
public readonly int ExpectedCommands;
public readonly string Reason;
public readonly struct FoundryMaterialQuality
public readonly string MaterialProfileId;
public readonly FoundryPurityTier Purity;
public readonly int DurabilityModifierBp;
public readonly int ArmorModifierBp;
public readonly int CorrosionModifierBp;
public sealed partial class SilentFoundrySystem
public void BindMaterialProfiles(MaterialProfileCatalog catalog, IReadOnlyDictionary<string, string> outputItemToMaterialId) {
public static FoundryPurityTier DerivePurityTier(float quality, float contamination, float slag) {
public bool TryGetLatestMaterialQualityAny(out FoundryMaterialQuality quality) {
public bool TryGetLatestMaterialQuality(string outputItemId, out FoundryMaterialQuality quality) {
public FoundryForgingSessionState? ActiveForging =>
public string BeginForging(string outputItemId, int day) {
public string SubmitForgingCommand(FoundryForgingCommand command, int day) {
public FoundryForgingResult CompleteForging(int day) {
public event Action<FoundryForgingResult>? OnForgingCompleted;
```


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Metallurgy.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Metallurgy.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 221 lines / 10158 bytes.
- SHA-256: `c625f6ec47f5e01a72e3d7b75083612d4ecf64b8723ae41e862fb1eec4c900ab`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed partial class SilentFoundrySystem
public void BindMetallurgyCatalog(MetallurgyHeavyCatalog catalog) {
public void BindVentilation(VentilationSystem ventilation) {
public float SlagLevel => MathfCompat.Clamp(_state.metallurgySlag, 0f, 100f);
public bool IsHeavyBatchActive => !string.IsNullOrEmpty(_state.activeMetallurgyRecipeId);
public string StartHeavyBatch(string recipeId, int workers, float workerSkill, int day) {
public string SkimSlag(int day) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.TreatyLabor.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.TreatyLabor.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 178 lines / 9066 bytes.
- SHA-256: `a8794132ecd09dd57853a547aeef2ded060d38958993d6d597927025e1e6a5f9`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=1; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed partial class SilentFoundrySystem
public static bool TryGetJournalDeltas(string templateId, out float stressDelta, out float hopeEarned) {
public SilentFoundryState CaptureState() {
public SilentFoundryConsequenceState CaptureConsequenceState() {
public void RestoreConsequenceState(SilentFoundryConsequenceState save) {
public void RestoreState(SilentFoundryState save) {
```


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/CrossingHeadlessDemo.cs`

### `Assets/Ashfall.Core/CrossingHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 112 lines / 7255 bytes.
- SHA-256: `b6a9eebafe527b572bc8d3ad162e9febef4cbf90d8c53421d612c3841b3c2102`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class CrossingHeadlessDemo
public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null) {
```


# Appendix Q.565 — Additional Current Architecture Evidence: `src/Host/HostCli.ExpansionDepth.cs`

### `src/Host/HostCli.ExpansionDepth.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 144 lines / 7946 bytes.
- SHA-256: `c2e979fba2fe17db74263330c5ba2382dfc878788a22ca81b8df117457f89ff8`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunExpansionDepthSelfTest(string dataDirectory) {
public int schema_version { get; set; }
public List<VerdictQuestlineDef>? quests { get; set; }
public string questlineId { get; set; } = string.Empty;
public string title { get; set; } = string.Empty;
public int schema_version { get; set; }
public List<VerdictNpcDef>? items { get; set; }
public string id { get; set; } = string.Empty;
public string name { get; set; } = string.Empty;
public int schema_version { get; set; }
public List<QuestlineMasterEntryDef>? entries { get; set; }
public string id { get; set; } = string.Empty;
```


# Appendix Q.566 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Crossing/CrossingThirdonaryIntegration.cs`

### `Assets/Ashfall.Core/Crossing/CrossingThirdonaryIntegration.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 169 lines / 6213 bytes.
- SHA-256: `e3b4ee33b960819bee7627921350e3ddf4c4015ba68b20ac1dfed97421556beb`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum CovenantStatus
public enum DisputeStatus
public sealed class CovenantEligibilityResult
public CovenantStatus Status;
public string CovenantId = string.Empty;
public string Reason = string.Empty;
public sealed class DisputeEligibilityResult
public DisputeStatus Status;
public string DisputeId = string.Empty;
public string Reason = string.Empty;
public sealed class CrossingThirdonaryIntegration
public CrossingArbitrationSystem Arbitration => _arbitration;
public CrossingQuestSystem Quests => _quests;
public bool IsOpeningQuestComplete() => _quests.IsQuestCompleted(CrossingQuestSystem.OpeningQuest);
public CovenantEligibilityResult GetCovenantEligibility(string covenantId, int currentDay) {
public DisputeEligibilityResult GetDisputeEligibility(string disputeId, int currentDay) {
```


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/ExpansionMasterSession.cs`

### `Assets/Ashfall.Core/ExpansionMasterSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 212 lines / 10386 bytes.
- SHA-256: `568760f21b08597c7cd360ea5f147c07feebc84edac5a7ca2f20addfd7545f0b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpansionMasterSession
public HoldfastSession Holdfast { get; }
public DutyRosterSystem DutyRoster { get; }
public DutyRosterCatalog DutyRosterData { get; }
public LocationLayoutSystem StandingRecord { get; }
public CrossingSession Crossing { get; }
public SimClock Clock { get; }
public ILog Log { get; }
public SilentFoundrySystem SilentFoundry { get; }
public SilentFoundryCatalog FoundryData { get; }
public DiseaseSystem Disease { get; }
public DiseaseCatalog DiseaseData { get; }
public bool AllExpansionsActive =>
public static ExpansionMasterSession Load(string dataDirectory, int seed =808, ILog? log = null) {
public void TickDaily(WeatherKind weather, float outdoorTemp, List<DutyRosterOccupant>? homeOccupants = null, IReadOnlyList<string>? diseaseCandidates = null) {
public static HeadlessReport RunAllSelfTests(string? dataDirectory = null, ILog? log = null) {
```


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentDeepChainGate.cs`

### `Assets/Ashfall.Core/Content/ContentDeepChainGate.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 444 lines / 21947 bytes.
- SHA-256: `c5d60671673ea3333c1f484f8cfd293905a42799056208eada0956173951c301`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DeepChainHopSpec
public string HopId { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public string RequiredFile { get; set; } = string.Empty;
public string? RequiredLoader { get; set; }
public string[]? RequiredSystems { get; set; }
public string? RequiredSurface { get; set; }
public sealed class DeepChainSpec
public string ChainId { get; set; } = string.Empty;
public string Narrative { get; set; } = string.Empty;
public bool IsHardGate { get; set; }
public DeepChainHopSpec[] Hops { get; set; } = Array.Empty<DeepChainHopSpec>();
public sealed class DeepChainFinding
public string ChainId { get; set; } = string.Empty;
public string HopId { get; set; } = string.Empty;
public string MissingCategory { get; set; } = string.Empty;
public string Details { get; set; } = string.Empty;
public string Severity { get; set; } = "HARD"; // HARD | WARN
public string RecommendedFix { get; set; } = string.Empty;
public sealed class DeepChainReport
public string SchemaVersion { get; set; } = "1.0.0";
public List<DeepChainFinding> Findings { get; set; } = new();
public int ChainsEvaluated { get; set; }
public int HardFailures => Findings.Count(f => f.Severity == "HARD");
public int Warnings => Findings.Count(f => f.Severity == "WARN");
public bool HardGatePassed => HardFailures == 0;
public void Stabilize() =>
public static class ContentDeepChainGate
public static readonly DeepChainSpec ResearchToCraft = new() {
public static readonly DeepChainSpec ExpeditionToUse = new() {
public static readonly DeepChainSpec FactionTreatyBriefing = new() {
public static readonly DeepChainSpec[] WarnTierChains = new[] {
public static IEnumerable<DeepChainSpec> AllChains =>
public static DeepChainReport Evaluate(ContentUtilizationGraph graph) {
```


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Foundry/FoundryActionSurface.cs`

### `Assets/Ashfall.Core/Foundry/FoundryActionSurface.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 109 lines / 3939 bytes.
- SHA-256: `659086666a1f48e3dc91373dee772a5635a7c227fab418c356021ed573af15fc`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FoundryActionSurface
public SilentFoundrySystem System => _system;
public FoundryActionResult AddCharge(string materialId, int units) {
public FoundryActionResult SelectRecipe(string recipeId) {
public FoundryActionResult Preheat(int targetTempC) {
public FoundryActionResult TapAndCast(int day) {
public FoundryActionResult ResolveStrike(string resolutionId) {
public sealed class FoundryActionResult
public bool Succeeded;
public string ReasonCode;
public string OutcomeLabel;
public Dictionary<string, int> IntDeltas = new Dictionary<string, int>();
public static FoundryActionResult Ok(Dictionary<string, int> deltas, string label) {
public static FoundryActionResult Fail(string reason) => new FoundryActionResult
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs`

### `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 653 lines / 36876 bytes.
- SHA-256: `5807c6ec3fdad9169ddd6c47d30ae978055a2b1d124f97d3360538513a4b5503`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class MachineConditionKeys
public const string HepaFilterHealth = "hepa.filter_health";
public const string HepaRadon = "hepa.radon_bqm3";
public const string FoundryRefractoryLining = "foundry.refractory_lining";
public const string FoundryHearthTuyeres = "foundry.hearth_tuyeres";
public const string FoundrySandBeds = "foundry.sand_beds";
public const string FoundryStructuralSupports = "foundry.structural_supports";
public const string FoundrySafetyExhaust = "foundry.safety_exhaust";
public const string FoundryAverageCondition = "foundry.average_condition";
public const string PowerFuelUnits = "power.fuel_units";
public const string PowerBatteryReserve = "power.battery_reserve";
public const string VentilationFilterSaturation = "ventilation.filter_saturation";
public const string VentilationDuctIntegrity = "ventilation.duct_integrity";
public const string WaterFilterIntegrity = "water.filter_integrity";
public const string ThermalBoilerFuel = "thermal.boiler_fuel";
public const string AirlockIncidentActive = "airlock.incident_active";
public static readonly string[] All = {
public static string FamilyOf(string machineId) {
public sealed class MachineConditionReadings
public float HepaFilterHealth = 100f;
public float HepaRadon = 12f;
public bool HazardWeather;
public float FoundryRefractoryLining = 100f;
public float FoundryHearthTuyeres = 100f;
public float FoundrySandBeds = 100f;
public float FoundryStructuralSupports = 100f;
public float FoundrySafetyExhaust = 100f;
public float PowerFuelUnits = 100f;
public float PowerBatteryReserve = 100f;
public bool PowerBrownout;
public float VentilationFilterSaturation;
public float VentilationDuctIntegrity = 100f;
public float VentilationSmokeSoot;
public bool VentilationMainDuctOpen = true;
public float WaterFilterIntegrity = 100f;
public float ThermalBoilerFuel = 100f;
public bool AirlockIncidentActive;
public string source = string.Empty;
public float? Get(string conditionKey) {
public bool HasContext(string context) {
public static class MachineQuirkKinds
public const string Diagnostic = "diagnostic";
public const string Personality = "personality";
public enum MachineConditionBand
public sealed class ShelterMachineCatalogData
public int schema_version = 1;
public string collection_id = string.Empty;
public List<MachineIdentityRecord> machines = new List<MachineIdentityRecord>();
public List<MachineQuirkRecord> quirks = new List<MachineQuirkRecord>();
public List<ShelterGlitchEvent>? glitch_events;
public sealed class MachineIdentityRecord
public string id = string.Empty;
public string condition_owner = string.Empty;
public string display_name = string.Empty;
public string nickname = string.Empty;
public string room_id = string.Empty;
public string purpose = string.Empty;
public string age_origin = string.Empty;
public string baseline_sound = string.Empty;
public string condition_key = string.Empty;
public List<string> quirk_ids = new List<string>();
public string audio_cue_family = string.Empty;
public string maintenance_skill_hook = string.Empty;
public string memorial_hook = string.Empty;
public sealed class MachineQuirkRecord
public string id = string.Empty;
public string machine_id = string.Empty;
public string kind = MachineQuirkKinds.Diagnostic;
public string condition_key = string.Empty;
public string comparison = "below";
public float trigger_below = -1f;
public string context = string.Empty;
public string text_cue = string.Empty;
public string audio_cue = string.Empty;
public string severity = "info";
public string maintenance_action = string.Empty;
public string repeat_policy = "continuous";
public sealed class ShelterGlitchEvent
public string id = string.Empty;
public string machine_id = string.Empty;
public string kind = "real_fault";
public string log_code = string.Empty;
public string title = string.Empty;
public string condition_key = string.Empty;
public string comparison = "below";
public float trigger_value = -1f;
public string context = string.Empty;
public string presentation = string.Empty;
public string resolution = string.Empty;
public List<string> repair_kit = new List<string>();
public string severity = "warning";
public string repeat_policy = "continuous";
public int cooldown_days;
public sealed class ShelterMachineTellCatalog
public const string FileName = "shelter_machine_identities.json";
public IReadOnlyList<MachineIdentityRecord> Machines => _machines;
public IReadOnlyList<MachineQuirkRecord> Quirks => _quirks;
public IReadOnlyList<ShelterGlitchEvent> GlitchEvents => _glitchEvents;
public int MachineCount => _machines.Count;
public static ShelterMachineTellCatalog Load(IFileIO files, IJsonSerializer json, string dataDirectory) {
public IReadOnlyList<ShelterGlitchEvent> GetGlitchEventsForMachine(string machineId) {
public ShelterGlitchEvent? GetGlitchEvent(string glitchId) {
public IReadOnlyList<ShelterGlitchEvent> EvaluateGlitchEvents( string machineId, MachineConditionReadings readings, Func<string, bool>? isNoted = null) {
public MachineIdentityRecord? GetMachine(string machineId) {
public MachineQuirkRecord? GetQuirk(string quirkId) {
public IReadOnlyList<MachineQuirkRecord> GetQuirksForMachine(string machineId) {
public IReadOnlyList<MachineQuirkRecord> EvaluateQuirks(string machineId, MachineConditionReadings readings) {
public MachineConditionBand EvaluateBand(string machineId, MachineConditionReadings readings) {
public static MachineConditionBand BandFor(float condition) {
public List<string> Validate() {
```


# Appendix Q.571 — Additional Current Architecture Evidence: `src/Main.UiPanels.cs`

### `src/Main.UiPanels.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1755 lines / 86737 bytes.
- SHA-256: `4c6b58cef5ed68a8ccfa1e2a51ca6da94a201322a745aa4c3b7993fc385782a1`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public Ashfall.Core.Feedback.IFeedbackService FeedbackService => _feedbackService;
public FeedbackPanel FeedbackPanel => _feedbackPanel;
public ConfirmationModal ConfirmationModal => _confirmationModal;
public void PromptConfirmation(string title, string message, Action onConfirm, Action? onCancel = null) {
```


# Appendix Q.572 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/CrossingArbitrationHeadlessDemo.cs`

### `Assets/Ashfall.Core/CrossingArbitrationHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 165 lines / 9926 bytes.
- SHA-256: `0d10d1c53902d50e0307d8bc7ee3dad788d03557d73c6dd6441179d58ec1ce37`.
- Architecture signals: seeded references=0; save/restore symbols=3; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ArbitrationHeadlessReport : HeadlessReport
public CrossingArbitrationState Arbitration;
public int RulingsCalled;
public int RulingsOverturned;
public int BribesRefused;
public static class CrossingArbitrationHeadlessDemo
public static ArbitrationHeadlessReport Run(ILog? log = null) {
```


# Appendix Q.573 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundryHeadlessDemo.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundryHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 187 lines / 11583 bytes.
- SHA-256: `d09f8c8d3f60830df15c9028c4cae67540b0c213d1c543dc0bfb4a419fe8022f`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class SilentFoundryHeadlessDemo
public const int DemoSeed = 1009;
public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null) {
```


# Appendix Q.574 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/HostCliRegistry.cs`

### `Assets/Ashfall.Core/HostCliRegistry.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1912 lines / 102959 bytes.
- SHA-256: `827182dd993f0bff556c0f2e1ba84448391b5b2b728a0d2242780da4594d9940`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum HostCliAction
public sealed class HostCliActionDescriptor
public HostCliAction Action { get; }
public string Category { get; }
public string PrimaryFlag { get; }
public IReadOnlyList<string> Aliases { get; }
public string Description { get; }
public string ValuePlaceholder { get; }
public IReadOnlyList<string> AllFlags { get; }
public bool IsSelfTest { get; }
public bool IsTest { get; }
public bool HeadlessCompatible { get; }
public string TestId { get; }
public string FormatHelpLine() {
public static class HostCliRegistry
public static readonly IReadOnlyList<string> Categories = new ReadOnlyCollection<string>(new[] {
public static IReadOnlyList<HostCliActionDescriptor> AllDescriptors => _descriptors;
public static IReadOnlyDictionary<string, HostCliActionDescriptor> FlagMap => _flagMap;
public static IReadOnlyList<HostCliActionDescriptor> CoreDescriptors => _coreDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> ExpansionDescriptors => _expansionDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> HostDomainDescriptors => _hostDomainDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> UiDescriptors => _uiDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> ConfigDescriptors => _configDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> InfoDescriptors => _infoDescriptors;
public static IReadOnlyDictionary<string, HostCliActionDescriptor> ValidateFlagRegistry() {
public static IReadOnlyDictionary<string, HostCliActionDescriptor> ValidateDescriptors(IEnumerable<HostCliActionDescriptor> descriptors) {
public static HostCliAction Resolve(string[]? args) {
public static void PrintHelp(Action<string> print) {
public static void PrintSelfTests(Action<string> print) {
public static HostSelfTestManifest CreateSelfTestManifest() {
public static string GenerateJsonManifest() {
public static string GenerateMarkdownCatalog(string verifiedDate) {
public sealed class HostSelfTestManifest
public string SchemaVersion { get; set; } = "1.0.0";
public string Description { get; set; } = "";
public int TotalTests { get; set; }
public int HeadlessTestCount { get; set; }
public List<HostSelfTestItem> Tests { get; set; } = new List<HostSelfTestItem>();
public sealed class HostSelfTestItem
public string TestId { get; set; } = "";
public string Action { get; set; } = "";
public string Category { get; set; } = "";
public string PrimaryFlag { get; set; } = "";
public string[] Aliases { get; set; } = Array.Empty<string>();
public string Description { get; set; } = "";
public bool HeadlessCompatible { get; set; }
public string ExpectedSummaryId { get; set; } = "";
public int TimeoutSeconds { get; set; } = 30;
```


# Appendix Q.575 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/LedgerDebtSystem.cs`

### `Assets/Ashfall.Core/LedgerDebtSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 370 lines / 16149 bytes.
- SHA-256: `eff8fc41f7d725c32be1194fdbfde14d98939451d5fa90996bae7fa624fd4242`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=14; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class DebtContract
public string debtorId;
public string creditorId = string.Empty;
public string templateId = string.Empty;
public float principal;
public int termDays;
public float rate;
public string forfeit;
public int readCount;
public bool signed;
public int signedDay = -1;
public int daysRemaining;
public bool paid;
public bool forfeited;
public bool forgiven;
public int forgivenDay = -1;
public class LedgerDebtSystemState
public string systemId = LedgerDebtSystem.SystemId;
public List<DebtContract> contracts = new List<DebtContract>();
public List<DebtContract> closedContracts = new List<DebtContract>();
public bool ledgerTampered;
public class LedgerDebtSystem
public const string SystemId = "ledger_debt_system";
public const int ReadsRequired = 2;
public const int StandingFreshDays = 3;
public event Action<DebtContract> OnContractSigned;
public event Action<DebtContract> OnContractPaid;
public event Action<DebtContract> OnContractForgiven;
public event Action<DebtContract> OnContractRenegotiated;
public event Action<DebtContract> OnForfeitTriggered;
public event Action OnLedgerTampered;
public event Action<LedgerDebtSystemState> OnStateChanged;
public LedgerDebtSystemState State => _state;
public IReadOnlyList<DebtContract> Contracts => _state.contracts;
public IReadOnlyList<DebtContract> ClosedContracts => _state.closedContracts;
public bool LedgerTampered => _state.ledgerTampered;
public DebtContract? GetContract(string debtorId) {
public bool PresentContract(string debtorId, float principal, int termDays, float rate, string forfeit, string creditorId = "", string templateId = "") {
public bool SignContract(string debtorId, int day) {
public bool CancelDraft(string debtorId, string creditorId = "", string templateId = "") {
public void TickDaily(int day) {
public bool PayContract(string debtorId, int day) {
public bool ForgiveContract(string debtorId, int day) {
public bool RenegotiateContract(string debtorId, float newPrincipal, int newTermDays, float newRate, string newForfeit, bool contested =false, Func<bool>? freshStanding = null) {
public bool TamperLedger() {
public float TotalOwed(string debtorId) {
public LedgerDebtSystemState CaptureState() => CopyState(_state);
public void RestoreState(LedgerDebtSystemState saved) {
```


# Appendix Q.576 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/StandingRecord/StandingRecordHeadlessDemo.cs`

### `Assets/Ashfall.Core/StandingRecord/StandingRecordHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 143 lines / 7544 bytes.
- SHA-256: `cce5a40612aafc224ac200384441cf4cbd297b44edbaa43d9a3323be51f32d8b`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class StandingRecordHeadlessDemo
public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null) {
```


# Appendix Q.577 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/UI/FactionIconCatalog.cs`

### `Assets/Ashfall.Core/UI/FactionIconCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 174 lines / 11992 bytes.
- SHA-256: `206183f0ef2c3567a759b892740384cf034015b78a1cb997988629d0ef5722f2`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class FactionIconCatalog
public const string FallbackIconPath = "assets/ui/Icons/icon_unknown_faction.png";
public static IReadOnlyList<string> MappedIds() {
public static string Resolve(string factionId) {
public static bool HasExplicitMapping(string factionId) {
public static IReadOnlyCollection<string> CoveredFactionIds =>
```


# Appendix Q.578 — Additional Current Architecture Evidence: `src/Host/HostCli.PanelTests.cs`

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


# Appendix Q.579 — Additional Current Architecture Evidence: `src/Foundry/SilentFoundryHostSession.cs`

### `src/Foundry/SilentFoundryHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 650 lines / 31351 bytes.
- SHA-256: `bbbed72dc2e0ebf48a735ed8f9f561cd61fd27bc7635edfd15ade6c688e89299`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=1; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SilentFoundryHostSession
public const int DefaultSeed = 1009;
public SilentFoundrySystem Engine { get; }
public SaltMineExtractionSystem SaltMine { get; }
public SilentFoundryCatalog Catalog { get; }
public ItemCatalog FoundryItems { get; }
public SilentFoundryConsequencePolicyCatalog ConsequencePolicy { get; }
public FactionStanceEngine GuildStanceEngine { get; }
public Ashfall.Core.Shelter.PowerGridSystem? PowerGrid { get; set; }
public Ashfall.Core.ShelterThermalSystem? ThermalSystem { get; set; }
public string LastEvent { get; set; } = string.Empty;
public event Action? StateChanged;
public void BindPowerAndThermal(Ashfall.Core.Shelter.PowerGridSystem? powerGrid, Ashfall.Core.ShelterThermalSystem? thermal) {
public string BeginForging(string outputItemId, int day) {
public string SubmitForgingCommand(Ashfall.Core.Foundry.FoundryForgingCommand command, int day) {
public string CompleteForging(int day) {
public void TickDaily(int day) {
public static SilentFoundryHostSession Create( string dataDir, ExpansionHostSession expansions, InventoryHostSession inventory, JournalSystem? journal = null, MarketSystem? market = null,
public float GuildTrust => Engine.GuildStanding;
public TradeStance GuildStance => GuildStanceEngine.GetStance(SilentFoundryIds.FactionId);
public void SyncGuildStanding() {
public void BindStanceProviders(Func<int> campaignDayProvider, Func<float> partyRadiationProvider, Func<SurvivorsHostSession?> survivorsProvider) {
public string Id { get; }
public string DisplayName { get; }
public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;
public string Unlock(int day) => Engine.Unlock(day) ? "The Silent Foundry is open." : "Already open.";
public string Repair(FoundryFacilityComponent component, int day) => Engine.StartRepair(component, day);
public string Maintain(int day) => Engine.PerformMaintenance(day);
public string PrepareSand(int water) => Engine.PrepareSand(water);
public string CompactMold() => Engine.CompactMold(0.6f);
public string StartHeat(string productId, int workers, float skill, int day) {
public string Tap(int day) => Engine.TapAndCast(day);
public string SetOvertime(bool on) { Engine.SetOvertime(on); return on ? "Overtime ordered." : "Overtime rescinded."; }
public string SetChildLabor(bool on) { Engine.SetChildLaborUsed(on); return on ? "Children sent to the charging floor." : "Children returned to lessons."; }
public string OpenDispute(int day) => Engine.BeginLaborDispute(day);
public string ResolveStrike(FoundryStrikeResolution resolution, int day) => Engine.ResolveStrike(resolution, day);
public string OpenSaltMine(string veinId = "vein_salt_01", string displayName = "Main Salt Vein", int initialWorkers = 2) {
public string OpenSaltMineDemo() => OpenSaltMine();
public string TickSaltMine(int day) {
public string TickSaltMineDemo(int day) => TickSaltMine(day);
public string DeliverSaltTreaty(int day) {
public string DeliverSaltTreatyDemo(int day) => DeliverSaltTreaty(day);
public string SaltMineStatusLine() {
public string StatusLine() {
public sealed class FoundryItemJson
public string? id;
public string? displayName;
public string? description;
public string? type;
public int stackMax = 1;
public float weight;
public float tradeValue;
public float durability;
```


# Appendix Q.580 — Additional Current Architecture Evidence: `src/Host/ContentUtilizationRuntimeCollector.cs`

### `src/Host/ContentUtilizationRuntimeCollector.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1230 lines / 64739 bytes.
- SHA-256: `4be289e49ddef6d5dcd29ccdc988a5f397b6153d4d20f1aa585ca9fe39df9f65`.
- Architecture signals: seeded references=3; save/restore symbols=0; typed event declarations=0; textual Godot mentions=47; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class ContentUtilizationRuntimeCollector
public const int DefaultSeed = 9001;
public static ContentUtilizationInstrumentation Collect(string dataDir) {
public bool CanApplyMorale(string survivorId, int delta, bool shelterWide, out string reason) {
public void ApplyMorale(string survivorId, int delta, bool shelterWide) { }
public bool CanGrantFactionIntel(string canonicalFactionId, out string reason) {
public void GrantFactionIntel(string canonicalFactionId) { }
public bool CanOfferExpedition(string locationId, out string reason) {
public void OfferExpedition(string locationId) { }
public bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason) {
public void ApplyFactionStanding(string canonicalFactionId, int delta) { }
```


# Appendix Q.581 — Additional Current Architecture Evidence: `src/Host/ExpeditionHostSession.cs`

### `src/Host/ExpeditionHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1663 lines / 85060 bytes.
- SHA-256: `46c1ce80f030a53183aea292df6246ab3278b2c2d481baf91e91cbf1f1c8af37`.
- Architecture signals: seeded references=6; save/restore symbols=13; typed event declarations=11; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpeditionHostSession : HostSessionBase
public const int DemoSeed = 7071;
public const int VehicleSeed = 7072;
public const float KmPerTravelTick = 2.5f;
public const string StarterVehicleId = "vehicle_utility_quad";
public ExpeditionSystem Engine { get; }
public List<ExpeditionDefinition> Definitions { get; }
public List<ExpeditionDefinition> DemoDefinitions => Definitions;
public DiveInstanceRunner DiveRunner { get; private set; }
public Ashfall.Core.Flags.IFlagLedger Flags { get; set; } = new Ashfall.Core.Flags.CampaignConsequenceLedger();
public DiscoveryConsequenceSystem DiscoveryConsequences { get; }
public event Action<ConsequenceOutcome>? OnDiscoveryConsequenceApplied;
public Action<string, string, int>? ApplyDisease { get; set; }
public JournalSystem? Journal { get; set; }
public Ashfall.Core.Inventory.Inventory? ShelterInventory { get; set; }
public ItemCatalog? Items { get; set; }
public ExpeditionVehicleSystem Vehicles { get; }
public VehicleGarageSystem? Garage { get; set; }
public VouchAccessSystem CrossingGate { get; set; }
public WastelandMapSystem? WastelandMap { get; set; }
public Func<string, bool> ExtraBlocked { get; set; }
public Func<string, FitnessVerdict?>? SurvivorFitnessProvider { get; set; }
public Func<string, RoleFitnessVerdict?>? ExpeditionFitnessProvider { get; set; }
public Func<string, float>? SurvivorMovementSpeedProvider { get; set; }
public Func<string, float>? PackCapacityProvider { get; set; }
public Func<string, WeatherGateBlock?> ExtraGateBlock { get; set; }
public string? GetBlockReason(string locationId) {
public WeatherGateBlock? GetWeatherGateBlock(string locationId) => ExtraGateBlock?.Invoke(locationId);
public void SetEncounterChanceMultiplier(Func<string, float> multiplier) => Engine.SetEncounterChanceMultiplier(multiplier);
public void SetEstimateWeatherInputs(Func<string, ExpeditionWeatherInputs?>? provider) {
public void SetEstimateProtectiveInputs(Func<string, ExpeditionProtectiveInputs?>? provider) {
public void SetEstimateRouteModifiers(Func<string, float>? hazard, Func<string, float>? travel) {
public void AttachDamagedMapFeedback(DamagedMapSystem? damagedMap) {
public string LastEvent { get; private set; } = string.Empty;
public event Action<ExpeditionEncounterBridge.EncounterSurfaced>? OnEncounterSurfaced;
public sealed class TravelCombatTrigger
public string EncounterId = string.Empty;
public string Title = string.Empty;
public string LocationId = string.Empty;
public int DangerLevel;
public IReadOnlyList<string> CombatantIds = Array.Empty<string>();
public event Action<TravelCombatTrigger>? OnTravelEncounterCombatTriggered;
public event Action<string, string, WeatherGateBlock>? OnWeatherGateForced;
public static bool UseEncounterModal { get; set; } = true;
public ExpeditionEncounterBridge Bridge => _bridge;
public Dictionary<string, float> WaterRouteHazards { get; } = new(StringComparer.Ordinal);
public IReadOnlyList<PendingSurfacedEncounter> Pending =>
public EncounterDefinition? FindEncounter(string encounterId) => _narrative?.Find(encounterId);
public void ClearAllPending() => _narrative?.ClearAllPending();
public NarrativeEncounterSystem? NarrativeEngine => _narrative;
public static ExpeditionHostSession Create(string dataDir, NarrativeEncounterSystem narrative = null!, TravelEncounterSystem travel = null!, ICampaignRngManager? campaignRng = null) {
public bool IsLocationBlocked(string locationId) => GetBlockReason(locationId) != null;
public CommandResult StartExpedition( string survivorId, string locationId, ExpeditionStance stance = ExpeditionStance.Stealth, int staminaBudget = 40, string vehicleId = "",
public CommandResult RefuelVehicle(string vehicleId, float units) {
public CommandResult RepairVehicle(string vehicleId, float amount) {
public CommandResult InstallTrackGear(string vehicleId, string gearId, float condition = 100f) {
public CommandResult RemoveTrackGear(string vehicleId) {
public CommandResult RepairTrackGear(string vehicleId, float amount) {
public CommandResult AssembleVehicleFromKit(string kitItemId, Inventory shelterInventory) {
public Action<VehicleBreakdownOutcome>? BreakdownConsequenceSink { get; set; }
public CommandResult StartDemoExpedition(string survivorId, string locationId) => StartExpedition(survivorId, locationId);
public CommandResult DispatchSortie( string survivorId, string locationId, ExpeditionStance stance, int day, string vehicleId = "",
public FitnessVerdict? GetSurvivorFitness(string survivorId) => SurvivorFitnessProvider?.Invoke(survivorId);
public RoleFitnessVerdict? GetExpeditionFitness(string survivorId) => ExpeditionFitnessProvider?.Invoke(survivorId);
public string TickHours(float hours) {
public sealed class EncounterApplicationResult
public string ResolutionId = string.Empty;
public enum Status { NotApplicable, Applied, AlreadyKnown, RejectedCapacity, RejectedInsufficientItems, NoActiveExpedition, SkippedNoAuthority, RejectedUnknownId } public Status Item = Status.NotApplicable; public string ItemId = string.Empty; public int ItemQuantity; public Status Journal = Status.NotApplicable; public string JournalId = string.Empty; public Status Location = Status.NotApplicable; public string LocationId = string.Empty; public Status Flag = Status.NotApplicable; public string FlagId = string.Empty; /// <summary>F17 — micro-location hazard routing outcome. NotApplicable /// for flags without a registered hazard; Applied when the canonical /// disease authority received the consequence exactly once.</summary> public MicroLocationHazardRegistry.HazardStatus Hazard = MicroLocationHazardRegistry.HazardStatus.NotApplicable; public string HazardDiseaseId = string.Empty; }
public EncounterApplicationResult? LastApplication { get; private set; }
public event Action<EncounterApplicationResult>? OnEncounterConsequencesApplied;
public bool EncounterApplyChoice(string encounterId, string choiceId, int day) => EncounterApplyChoice(encounterId, choiceId, day, null!);
public bool EncounterApplyChoice(string encounterId, string choiceId, int day, string locationId) {
public bool ResolveTravelChoiceWithCombat( string encounterId, string choiceId, int day, string locationId, int dangerLevel, int enemyCount) {
public static readonly ExpeditionJournalAuthor Instance = new ExpeditionJournalAuthor();
public string Id => "expedition";
public string DisplayName => "Expedition";
public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;
public string PushLuck(string survivorId) {
public string PushLuckDemo(string survivorId) => PushLuck(survivorId);
public string Retreat(string survivorId) {
public string RetreatDemo(string survivorId) => Retreat(survivorId);
public string EnterCamp( string survivorId, float temperatureC = -10f, string weatherCondition = "Clear", float firewood = 8f, float water = 4f,
public string EnterCampDemo( string survivorId, float temperatureC = -10f, string weatherCondition = "Clear", float firewood = 8f, float water = 4f,
public string CampTick(string survivorId) {
public string CampTickDemo(string survivorId) => CampTick(survivorId);
public string ResolveCampEncounter(string survivorId, string outcome) {
public string ResolveCampEncounterDemo(string survivorId, string outcome) => ResolveCampEncounter(survivorId, outcome);
public string BreakCamp(string survivorId, bool retreat = false) {
public string BreakCampDemo(string survivorId, bool retreat = false) => BreakCamp(survivorId, retreat);
public CampState? GetCampState(string survivorId) => Engine.GetCampState(survivorId);
public string StatusLine() {
public List<ExpeditionState> CaptureSave() => Engine.CaptureState();
public void RestoreSave(List<ExpeditionState> state) => Engine.RestoreState(state);
public ExpeditionAggregateState CaptureSaveAggregate() {
public void RestoreSaveAggregate(ExpeditionAggregateState aggregate) {
public string StartDive(string siteId = "site_exp09_ss_sovereign") {
public string StartDiveDemo(string siteId = "site_exp09_ss_sovereign") => StartDive(siteId);
public string AdvanceDive() {
public string AdvanceDiveDemo() => AdvanceDive();
public string TickDiveOxygen() {
public string TickDiveOxygenDemo() => TickDiveOxygen();
public string CommitDiveChoice(string choice) {
public string CommitDiveChoiceDemo(string choice) => CommitDiveChoice(choice);
public string DiveStatusLine() {
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.


# Appendix S — Quality Assurance Pass Record: Plan 120

This record is part of the planning artifact, not a fresh runtime test result.

## Pass 1 — content and premise accuracy
- Content pass confirmed the eight-row catalog while preserving the five-faction residual.
- The historical baseline is separated from the current source/data/test authority.
- Current row counts and owner boundaries are stated without using count as a quality proxy.

## Pass 2 — integration architecture
- Integration pass identified the missing live ExpansionHostSession/CrossingCatalog binding.
- Core, data, host, UI, save, event and test seams are named with current paths.
- The plan does not authorize a parallel save section, catalog, manager or host cache.

## Final precision and reaccuracy pass
- Precision pass proposes one read-only catalog projection and explicitly labels future-only rows.
- Every embedded current-file hash, focused runner command and master-authority reference is rechecked.
- Any proposed future seam is labeled as requiring a separate claim and premise verification.
