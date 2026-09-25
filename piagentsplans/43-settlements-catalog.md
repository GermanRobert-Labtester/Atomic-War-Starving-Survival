# Plan 43 — Settlement Gazetteer, Allegiance and Living Community Reachability

> **Rebuild status:** COMPLETE 12-SETTLEMENT CATALOG — SETTLEMENT CATALOG AND TERRITORY CROSS-VALIDATION EXIST; DIRECT TRAVEL REMAINS A DISTINCT OWNER QUESTION
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

- The current catalog has 12 settlement definitions with location, allegiance, trade profile, threat and descriptive fields.
- SettlementCatalog owns static settlement/quest data; OutpostSettlementSystem owns player-founded outposts; ColonySystem owns colonies; TerritoryControlSystem owns mutable faction control.
- Host CLI and current world/settlement routes consume the catalog, but direct travel to every settlement is not implied by catalog presence.

**Bounded outcome:** Retire the no-settlements premise. settlements.json currently has 12 rows, SettlementCatalog loads it, and Plan43_44 tests cross-validate settlement allegiance, location links and territory control points. The plan must not turn a read-only gazetteer into a second population/economy simulation or duplicate outpost/colony owners.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- Replace the old “12 living settlements” proposal with a current 12-row catalog/reachability audit.
- Map settlement catalog, outpost, colony, territory, trade and travel owners and identify what direct player-facing route is actually intended.
- Preserve catalog quests/state in SettlementCatalog; no new settlement population/food/inventory ledger.

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
- The plan now distinguishes living-world reference data from actual player travel and outpost systems.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Current evidence requires a bounded owner/reachability audit; no new authority is implied.

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
| settlement definitions, NPCs, quests and catalog state | SettlementCatalog | `Assets/Ashfall.Core/World/SettlementCatalog.cs` | Static/world-reference owner. |
| player-founded outposts, supply, garrison and persistence | OutpostSettlementSystem | `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs` | Separate mutable outpost owner. |
| current faction control and territory shifts | TerritoryControlSystem | `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs` | Separate mutable control owner. |
| host load/commands/save | OutpostSettlementHostSession | `src/Host/OutpostSettlementHostSession.cs` | Thin host seam. |
| current catalog/settlement diagnostic path | HostCli.WastelandInhabitants | `src/Host/HostCli.WastelandInhabitants.cs` | Diagnostic/consumer evidence. |
| settlement/territory cross-validation | Plan43_44 tests | `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs` | Focused evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Settlement Gazetteer, Allegiance and Living Community Reachability
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ SettlementCatalog
│   settlement definitions, NPCs, quests and catalog state
│ OutpostSettlementSystem
│   player-founded outposts, supply, garrison and persistence
│ TerritoryControlSystem
│   current faction control and territory shifts
│ OutpostSettlementHostSession
│   host load/commands/save
│ HostCli.WastelandInhabitants
│   current catalog/settlement diagnostic path
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

1. **Preserve current state ownership.** SettlementCatalog owns settlement definitions, NPCs, quests and catalog state: Static/world-reference owner.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| settlement definitions, NPCs, quests and catalog state | SettlementCatalog | `Assets/Ashfall.Core/World/SettlementCatalog.cs` | Static/world-reference owner. |
| player-founded outposts, supply, garrison and persistence | OutpostSettlementSystem | `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs` | Separate mutable outpost owner. |
| current faction control and territory shifts | TerritoryControlSystem | `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs` | Separate mutable control owner. |
| host load/commands/save | OutpostSettlementHostSession | `src/Host/OutpostSettlementHostSession.cs` | Thin host seam. |
| current catalog/settlement diagnostic path | HostCli.WastelandInhabitants | `src/Host/HostCli.WastelandInhabitants.cs` | Diagnostic/consumer evidence. |
| settlement/territory cross-validation | Plan43_44 tests | `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs` | Focused evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load 12 settlement definitions
2. validate locations, allegiances and trade references
3. read current territory/outpost state
4. present settlement information and quest availability
5. route actual trade/travel/outpost commands through owners
6. capture catalog quest state and current owner envelopes

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Settlement catalog rows and catalog quest state are one static/current catalog concern.
- Outposts, colonies and territory control retain separate mutable state.
- Trade goods and threat fields are not executable until current trade/patrol owners consume them.
- No new settlement save section.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A settlement location must resolve before travel/trade presentation.
- Catalog allegiance does not overwrite current territory control.
- A settlement is not automatically a player outpost/colony.
- Quest availability uses current day/cooldown state.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- settlements.json, settlement gazetteer/NPC files, faction territory, outposts and colonies are separate authorities.
- No duplicate settlement population or trade ledger.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use SettlementCatalog state/quest persistence and current outpost/colony/territory saves.
- No Plan-43 save section for catalog-only changes.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Settlement enumeration is stable; current travel/patrol randomness remains with seeded owners.
- Catalog projections are pure.
- Same catalog, day and owner state produce same availability.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Settlement catalog emits reference/quest facts.
- Outpost and territory systems emit their own state facts.
- Travel/trade consumers read current owners and do not write catalog rows.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/OutpostSettlementHostSession.cs
- src/Host/HostCli.WastelandInhabitants.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Settlement descriptions are fictional, restrained and grounded.
- No real countries, copied communities or political claims.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | Catalog settlement is presented as a player colony. | SettlementCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | Static allegiance overwrites territory control. | OutpostSettlementSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A second population ledger is created. | TerritoryControlSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Trade/travel buttons have no command. | OutpostSettlementHostSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new save section duplicates current state. | HostCli.WastelandInhabitants | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/World/SettlementCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 | 12-row settlement census. | Static rows and current owners are explicit. | No production path until the owning implementation package is separately claimed. |
| 1 | Travel/trade/outpost/territory route trace. | No catalog presence is mislabeled as direct travel. | No production path until the owning implementation package is separately claimed. |
| 2 | Quest/save/replay audit. | Static and mutable state stay separate. | No production path until the owning implementation package is separately claimed. |
| 3 | UI/accessibility polish. | World-reference and live-route labels are truthful. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/settlements.json | READ ONLY; MODIFY only for approved content/reference delta | 12 settlements |
| Assets/Ashfall.Core/World/SettlementCatalog.cs | READ ONLY | Settlement catalog |
| src/Host/OutpostSettlementHostSession.cs | READ ONLY | Separate outpost seam |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Parallel settlement simulation. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Conflating catalog/outpost/colony. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Static allegiance overwriting control. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Fake travel route. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new settlement rows.
- No new population/economy owner.
- No production/data/test/UI changes here.

# 23. Rollback and Recovery

- Revert planning artifact.
- Future route changes use current travel/outpost/territory seams.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 12 rows and owner boundaries are explicit.
- Travel reachability is treated as a separate question from catalog existence.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Current evidence requires a bounded owner/reachability audit; no new authority is implied.

## MUST NOT DO

- No new settlement rows.
- No new population/economy owner.
- No production/data/test/UI changes here.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/World/SettlementCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: settlement definitions, NPCs, quests and catalog state → SettlementCatalog; player-founded outposts, supply, garrison and persistence → OutpostSettlementSystem; current faction control and territory shifts → TerritoryControlSystem; host load/commands/save → OutpostSettlementHostSession; current catalog/settlement diagnostic path → HostCli.WastelandInhabitants; settlement/territory cross-validation → Plan43_44 tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 43.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 43 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by SettlementCatalog or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/World/SettlementCatalog.cs`

### `Assets/Ashfall.Core/World/SettlementCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 463 lines / 17110 bytes.
- SHA-256: `492fea4b385a078e5e94ed407fe2d74a9876a8143a92b87144db32bcb0d9d29e`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SettlementEconomy
public string PrimaryExport { get; set; } = string.Empty;
public string PrimaryImport { get; set; } = string.Empty;
public string TradeSpecialty { get; set; } = string.Empty;
public float PriceModifierExports { get; set; } = 1.0f;
public float PriceModifierImports { get; set; } = 1.0f;
public List<string> StockItemIds { get; set; } = new List<string>();
public sealed class SettlementSociety
public string Governance { get; set; } = string.Empty;
public int Population { get; set; } = 50;
public string CoreValue { get; set; } = string.Empty;
public string InternalTension { get; set; } = string.Empty;
public sealed class SettlementFactionRelation
public string PrimaryFaction { get; set; } = string.Empty;
public string StandingGateFaction { get; set; } = string.Empty;
public int MinStandingToEnter { get; set; } = 0;
public int HostileStandingThreshold { get; set; } = -40;
public sealed class SettlementDefinition
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string Archetype { get; set; } = string.Empty;
public string Region { get; set; } = string.Empty;
public string LocationId { get; set; } = string.Empty;
public string RouteNode { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public string SurvivalAdaptation { get; set; } = string.Empty;
public SettlementEconomy Economy { get; set; } = new SettlementEconomy();
public SettlementSociety Society { get; set; } = new SettlementSociety();
public SettlementFactionRelation FactionRelation { get; set; } = new SettlementFactionRelation();
public string LocationLink { get; set; } = string.Empty;
public int Population { get; set; } = 0;
public string Allegiance { get; set; } = string.Empty;
public int ThreatLevel { get; set; } = 2;
public string Attitude { get; set; } = "neutral";
public List<string> TradeGoods { get; set; } = new List<string>();
public List<string> TradeNeeds { get; set; } = new List<string>();
public string KeeperNpcId { get; set; } = string.Empty;
public string TraderNpcId { get; set; } = string.Empty;
public string FixtureNpcId { get; set; } = string.Empty;
public string SideworkQuestId { get; set; } = string.Empty;
public string GetEffectiveLocationId() => !string.IsNullOrEmpty(LocationLink) ? LocationLink : LocationId;
public int GetEffectivePopulation() => Population > 0 ? Population : (Society?.Population ?? 50);
public string GetEffectiveAllegiance() => !string.IsNullOrEmpty(Allegiance) ? Allegiance : (FactionRelation?.PrimaryFaction ?? "none");
public sealed class SettlementNpcGreeting
public string LowStanding { get; set; } = string.Empty;
public string Neutral { get; set; } = string.Empty;
public string HighStanding { get; set; } = string.Empty;
public sealed class SettlementNpcEntry
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string SettlementId { get; set; } = string.Empty;
public string Role { get; set; } = string.Empty; // "Keeper", "Trader", "Fixture"
public string Profession { get; set; } = string.Empty;
public string Faction { get; set; } = "none";
public string TradeSpecialty { get; set; } = string.Empty;
public string PhysicalAnchor { get; set; } = string.Empty;
public string Value { get; set; } = string.Empty;
public string Fear { get; set; } = string.Empty;
public string Contradiction { get; set; } = string.Empty;
public string PersonalThread { get; set; } = string.Empty;
public SettlementNpcGreeting Greetings { get; set; } = new SettlementNpcGreeting();
public List<string> TradeTells { get; set; } = new List<string>();
public string SideworkQuestId { get; set; } = string.Empty;
public string PortraitId { get; set; } = string.Empty;
public sealed class RepeatableQuestStage
public string Id { get; set; } = string.Empty;
public string Text { get; set; } = string.Empty;
public sealed class RepeatableQuestEntry
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string ProviderNpcId { get; set; } = string.Empty;
public string SettlementId { get; set; } = string.Empty;
public string Type { get; set; } = string.Empty;
public string Briefing { get; set; } = string.Empty;
public string PrereqQuestId { get; set; } = string.Empty;
public string TargetLocationId { get; set; } = string.Empty;
public int CooldownDays { get; set; } = 7;
public string RewardItemId { get; set; } = string.Empty;
public int RewardCount { get; set; } = 1;
public int StandingDelta { get; set; } = 5;
public List<RepeatableQuestStage> Stages { get; set; } = new List<RepeatableQuestStage>();
public sealed class SettlementState
public Dictionary<string, int> QuestAvailableDay { get; set; } = new Dictionary<string, int>();
public Dictionary<string, int> CompletedQuestCounts { get; set; } = new Dictionary<string, int>();
public sealed class SettlementCatalog
public int SettlementCount => _allSettlements.Count;
public int NpcCount => _allNpcs.Count;
public int QuestCount => _allQuests.Count;
public IReadOnlyList<SettlementDefinition> Settlements => _allSettlements;
public IReadOnlyList<SettlementNpcEntry> Npcs => _allNpcs;
public IReadOnlyList<RepeatableQuestEntry> Quests => _allQuests;
public static SettlementCatalog LoadFromDirectory(string directoryPath, IFileIO fileIO) {
public void LoadSettlementsJson(string json) {
public void LoadNpcsJson(string json) {
public void LoadQuestsJson(string json) {
public bool TryGetSettlement(string id, out SettlementDefinition settlement) {
public bool TryGetNpc(string id, out SettlementNpcEntry npc) {
public bool TryGetQuest(string id, out RepeatableQuestEntry quest) {
public string GetNpcGreeting(string npcId, float standing) {
public bool IsQuestAvailable(string questId, int currentDay) {
public void CompleteQuest(string questId, int currentDay) {
public int GetCompletedQuestCount(string questId) {
public SettlementState CaptureState() {
public void RestoreState(SettlementState? state) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs`

### `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 496 lines / 21443 bytes.
- SHA-256: `35df3dad7070ede8b234fe0745daf4a4f0ee5d5d2751aebf7635b9a100105213`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=9; textual Godot mentions=1; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class OutpostDef
public string Id { get; set; } = string.Empty;
public string Name { get; set; } = string.Empty;
public string GraphNodeId { get; set; } = string.Empty;
public int MaxGarrisonBunks { get; set; } = 4;
public int DailySupplyDemand { get; set; } = 4;
public int DefenseRating { get; set; } = 50;
public int RadioRelayRange { get; set; } = 10;
public Dictionary<string, int> BuildCost { get; set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
public sealed class OutpostInstance
public string OutpostId { get; set; } = string.Empty;
public string GraphNodeId { get; set; } = string.Empty;
public bool IsEstablished { get; set; }
public int ConditionPermille { get; set; } = 1000;
public List<string> GarrisonSurvivorIds { get; set; } = new List<string>();
public int DaysSinceSupply { get; set; }
public bool IsStarving { get; set; }
public bool IsOverrun { get; set; }
public int RationReserve { get; set; }
public sealed class OutpostInstanceState
public string outpost_id { get; set; } = string.Empty;
public bool is_established { get; set; }
public int condition_permille { get; set; } = 1000;
public List<string> garrison_survivor_ids { get; set; } = new List<string>();
public int days_since_supply { get; set; }
public bool is_starving { get; set; }
public bool is_overrun { get; set; }
public int ration_reserve { get; set; }
public sealed class OutpostSettlementState
public int schema_version { get; set; } = 1;
public List<OutpostInstanceState> outposts { get; set; } = new List<OutpostInstanceState>();
public sealed class OutpostSettlementSystem
public const string SystemId = "outpost_settlement_system";
public Action<string, string>? OnOutpostEstablishedSeam { get; set; }
public Action<string, int>? OnOutpostSuppliedSeam { get; set; }
public Action<string>? OnOutpostOverrunSeam { get; set; }
public Action<string, string>? OnGarrisonAssignedSeam { get; set; }
public Action<string, string>? OnGarrisonRelievedSeam { get; set; }
public Action<string>? OnOutpostStarvingSeam { get; set; }
public static OutpostSettlementSystem FromJson(string json) {
public OutpostDef? GetDefinition(string outpostId) {
public OutpostInstance? GetInstance(string outpostId) {
public IReadOnlyList<OutpostDef> GetAllDefinitions() => _definitions.Values.ToList();
public IReadOnlyList<OutpostInstance> GetAllInstances() => _instances.Values.ToList();
public bool EstablishOutpost(string outpostId, Func<string, int, bool>? costConsumer = null) {
public bool TryEstablishOutpost(string outpostId, IPlayerInventoryPort? inventory) {
public bool AssignGarrison(string outpostId, string survivorId, Func<string, bool>? fitnessCheck = null) {
public bool RelieveGarrison(string outpostId, string survivorId) {
public bool SupplyOutpost(string outpostId, int rationsDelivered) {
public bool TrySupplyOutpost( string outpostId, string rationItemId, int rationsDelivered, IPlayerInventoryPort? inventory) {
public void TickDay(Func<string, int, int>? centralRationSupplyProvider = null) {
public bool SimulateRisk(string outpostId, int dangerRating, ISeededRng rng) {
public bool AbandonOutpost(string outpostId) {
public OutpostSettlementState CaptureState() {
public bool RestoreState(OutpostSettlementState? state) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs`

### `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 622 lines / 28809 bytes.
- SHA-256: `bc27b87874570a17bb5d1990ebce70451fac2737edbc6cb4bbc76f05ba31dbff`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=7; textual Godot mentions=1; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum SupplyLineStatus
public sealed class FactionTerritoryDef
public string Id { get; set; } = string.Empty;
public string Faction { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string Classification { get; set; } = string.Empty;
public string TerritoryScale { get; set; } = string.Empty;
public string PrimaryResourceInterest { get; set; } = string.Empty;
public List<string> ControlledNodes { get; set; } = new List<string>();
public List<string> ControlPoints { get; set; } = new List<string>();
public List<string> ContestedWith { get; set; } = new List<string>();
public int BaseControlStrength { get; set; } = 50;
public double TradeTax { get; set; } = 0.05;
public double TravelSafety { get; set; } = 0.75;
public string ShiftTrigger { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public sealed class SupplyLineDef
public string Id { get; set; } = string.Empty;
public string OwningFactionId { get; set; } = string.Empty;
public string OriginLocationId { get; set; } = string.Empty;
public string DestinationLocationId { get; set; } = string.Empty;
public List<string> RouteWaypoints { get; set; } = new List<string>();
public string CargoType { get; set; } = string.Empty;
public int ThroughputCapacity { get; set; } = 30;
public int TravelDays { get; set; } = 2;
public sealed class LocationTerritoryState
public string LocationId { get; set; } = string.Empty;
public string ControllingFactionId { get; set; } = string.Empty;
public int ControlStrength { get; set; } = 50;
public bool IsContested { get; set; }
public int FortificationLevel { get; set; }
public int GarrisonStrength { get; set; }
public int LastContestDay { get; set; }
public sealed class SupplyLineState
public string SupplyLineId { get; set; } = string.Empty;
public string OwningFactionId { get; set; } = string.Empty;
public SupplyLineStatus Status { get; set; } = SupplyLineStatus.Active;
public int LastDeliveredDay { get; set; }
public int TotalDelivered { get; set; }
public sealed class TerritoryControlSystem
public const string SystemId = "territory_control_system";
public Action<string, string, string>? OnTerritoryControlChangedSeam { get; set; }
public Action<string, string, string>? OnTerritoryContestedSeam { get; set; }
public Action<string, SupplyLineStatus>? OnSupplyLineStatusChangedSeam { get; set; }
public Action<string, int>? OnSupplyLineDeliveredSeam { get; set; }
public Action<string, int>? OnLocationFortifiedSeam { get; set; }
public static TerritoryControlSystem FromJson(string territoryJson, string supplyLineJson) {
public FactionTerritoryDef? GetTerritory(string territoryId) {
public LocationTerritoryState? GetLocationState(string locationId) {
public SupplyLineState? GetSupplyLineState(string lineId) {
public IReadOnlyList<FactionTerritoryDef> GetAllTerritories() => _territories.Values.ToList();
public IReadOnlyList<LocationTerritoryState> GetAllLocationStates() => _locationStates.Values.ToList();
public IReadOnlyList<SupplyLineState> GetAllSupplyLines() => _supplyLineStates.Values.ToList();
public bool FortifyLocation(string locationId, int levelDelta = 1) {
public bool AssignGarrison(string locationId, int garrisonDelta) {
public bool ContestLocation(string locationId, string attackingFactionId, int attackPower, ISeededRng rng, int currentDay = 0) {
public bool RaidSupplyLine(string supplyLineId, int raidIntensity, ISeededRng rng) {
public bool RestoreSupplyLine(string supplyLineId) {
public void TickDay(int currentDay, ISeededRng? rng = null) {
public TerritoryControlSaveState CaptureState() {
public bool RestoreState(TerritoryControlSaveState? state) {
public TerritoryCensus ReadCensus() {
public sealed class LocationTerritorySaveState
public string location_id { get; set; } = string.Empty;
public string controlling_faction_id { get; set; } = string.Empty;
public int control_strength { get; set; } = 50;
public bool is_contested { get; set; }
public int fortification_level { get; set; }
public int garrison_strength { get; set; }
public int last_contest_day { get; set; }
public sealed class SupplyLineSaveState
public string supply_line_id { get; set; } = string.Empty;
public string owning_faction_id { get; set; } = string.Empty;
public string status { get; set; } = "Active";
public int last_delivered_day { get; set; }
public int total_delivered { get; set; }
public sealed class TerritoryControlSaveState
public int schema_version { get; set; } = 1;
public List<LocationTerritorySaveState> locations { get; set; } = new List<LocationTerritorySaveState>();
public List<SupplyLineSaveState> supply_lines { get; set; } = new List<SupplyLineSaveState>();
public readonly struct TerritoryCensus
public readonly int TerritoriesCount;
public readonly int LocationsCount;
public readonly int ContestedLocationsCount;
public readonly int TotalSupplyLines;
public readonly int ActiveSupplyLines;
public readonly int DisruptedSupplyLines;
public readonly int SeveredSupplyLines;
public int TotalTerritories => TerritoriesCount;
public int TotalNodes => LocationsCount;
public int ContestedLocations => ContestedLocationsCount;
public string Describe() =>
```


# Appendix B.05 — Current Code Architecture: `src/Host/OutpostSettlementHostSession.cs`

### `src/Host/OutpostSettlementHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 214 lines / 9863 bytes.
- SHA-256: `bf8575bf7708e497711c6e5e06c6448c230055b52b358da3552a4646bd0a5d52`.
- Architecture signals: seeded references=1; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class OutpostSettlementHostSession : HostSessionBase
public const string CatalogFile = "outposts.json";
public OutpostSettlementSystem System { get; }
public bool CatalogLoaded { get; private set; }
public static OutpostSettlementHostSession Load(string dataDirectory, IFileIO files) {
public OutpostCensus ReadCensus() {
public bool Establish(string outpostId, Func<string, int, bool>? costConsumer = null) {
public bool TryEstablish(string outpostId, IPlayerInventoryPort? inventory) {
public bool AssignGarrison(string outpostId, string survivorId, Func<string, bool>? fitnessCheck = null) {
public bool RelieveGarrison(string outpostId, string survivorId) {
public bool Supply(string outpostId, int rationsDelivered) {
public bool TrySupply(string outpostId, string rationItemId, int rationsDelivered, IPlayerInventoryPort? inventory) {
public bool Abandon(string outpostId) {
public void TickDay(Func<string, int, int>? centralRationSupplyProvider = null) {
public bool SimulateRisk(string outpostId, int dangerRating, ISeededRng rng) => System.SimulateRisk(outpostId ?? string.Empty, dangerRating, rng);
public OutpostSettlementState CaptureState() => System.CaptureState();
public bool RestoreState(OutpostSettlementState? state) => System.RestoreState(state);
public void AppendReportLine(string line) {
public IReadOnlyList<string> ReportLines => _rawJsonLines;
public string Describe() => ReadCensus().Describe();
public readonly struct OutpostCensus
public readonly int Authored;
public readonly int Established;
public readonly int Overrun;
public readonly int Starving;
public readonly int Garrisoned;
public readonly int RationReserve;
public string Describe() => $"outposts: {Established}/{Authored} established, {Garrisoned} garrisoned, "
public static class OutpostSettlementSaveStore
public const string FileName = "outpost_settlement_save.json";
public const string SectionName = "outpost_settlement";
public static bool TrySave(OutpostSettlementState state) => s_store.TrySave(state);
public static OutpostSettlementState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(OutpostSettlementState state) => s_store.CapturePersisted(state);
public static OutpostSettlementState? TryRestore(string json) => s_store.RestoreEnvelope(json);
public static OutpostSettlementState? TryRestoreBare(string json) => s_store.RestoreBare(json);
```


# Appendix B.06 — Current Code Architecture: `src/Host/OutpostSettlementHostSession.cs`

### `src/Host/OutpostSettlementHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 214 lines / 9863 bytes.
- SHA-256: `bf8575bf7708e497711c6e5e06c6448c230055b52b358da3552a4646bd0a5d52`.
- Architecture signals: seeded references=1; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class OutpostSettlementHostSession : HostSessionBase
public const string CatalogFile = "outposts.json";
public OutpostSettlementSystem System { get; }
public bool CatalogLoaded { get; private set; }
public static OutpostSettlementHostSession Load(string dataDirectory, IFileIO files) {
public OutpostCensus ReadCensus() {
public bool Establish(string outpostId, Func<string, int, bool>? costConsumer = null) {
public bool TryEstablish(string outpostId, IPlayerInventoryPort? inventory) {
public bool AssignGarrison(string outpostId, string survivorId, Func<string, bool>? fitnessCheck = null) {
public bool RelieveGarrison(string outpostId, string survivorId) {
public bool Supply(string outpostId, int rationsDelivered) {
public bool TrySupply(string outpostId, string rationItemId, int rationsDelivered, IPlayerInventoryPort? inventory) {
public bool Abandon(string outpostId) {
public void TickDay(Func<string, int, int>? centralRationSupplyProvider = null) {
public bool SimulateRisk(string outpostId, int dangerRating, ISeededRng rng) => System.SimulateRisk(outpostId ?? string.Empty, dangerRating, rng);
public OutpostSettlementState CaptureState() => System.CaptureState();
public bool RestoreState(OutpostSettlementState? state) => System.RestoreState(state);
public void AppendReportLine(string line) {
public IReadOnlyList<string> ReportLines => _rawJsonLines;
public string Describe() => ReadCensus().Describe();
public readonly struct OutpostCensus
public readonly int Authored;
public readonly int Established;
public readonly int Overrun;
public readonly int Starving;
public readonly int Garrisoned;
public readonly int RationReserve;
public string Describe() => $"outposts: {Established}/{Authored} established, {Garrisoned} garrisoned, "
public static class OutpostSettlementSaveStore
public const string FileName = "outpost_settlement_save.json";
public const string SectionName = "outpost_settlement";
public static bool TrySave(OutpostSettlementState state) => s_store.TrySave(state);
public static OutpostSettlementState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(OutpostSettlementState state) => s_store.CapturePersisted(state);
public static OutpostSettlementState? TryRestore(string json) => s_store.RestoreEnvelope(json);
public static OutpostSettlementState? TryRestoreBare(string json) => s_store.RestoreBare(json);
```


# Appendix B.07 — Current Code Architecture: `src/Host/HostCli.WastelandInhabitants.cs`

### `src/Host/HostCli.WastelandInhabitants.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 143 lines / 9671 bytes.
- SHA-256: `0176f30e13d97c54e4f5b5fcd6e88e5b7ee11bc5eae9c2ee076223da53510183`.
- Architecture signals: seeded references=1; save/restore symbols=6; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunWastelandInhabitantsSelfTest(string dataDirectory) {
```


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/settlements.json`

### `Assets/StreamingAssets/Data/settlements.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 24974 bytes / 24968 characters.
- SHA-256: `bd77179e5c1ff6878f087579168ded303a2b78679b93e86920f38a7c28924ec3`.
- Root keys: `collection_id`, `schema_version`, `settlements`.

Array-path census (minimum, maximum, observed rows):

```text
settlements: min=12, max=12, observed_paths=1
settlements[].economy.stock_item_ids: min=3, max=3, observed_paths=2
settlements[].trade_goods: min=4, max=4, observed_paths=2
settlements[].trade_needs: min=3, max=4, observed_paths=2
```

Representative record fields:

- `allegiance`
- `archetype`
- `attitude`
- `description`
- `display_name`
- `economy`
- `faction_relation`
- `fixture_npc_id`
- `id`
- `keeper_npc_id`
- `location_id`
- `location_link`
- `population`
- `region`
- `route_node`
- `sidework_quest_id`
- `society`
- `survival_adaptation`
- `threat_level`
- `trade_goods`
- `trade_needs`
- `trader_npc_id`

Representative identifiers (ordered, capped for readability):

```text
settlement_brine_pans
settlement_iron_siding
settlement_cape_beacon
settlement_slate_hollow
settlement_pilgrim_hearth
settlement_tinkers_notch
settlement_ferry_crossing
settlement_nine_rails
settlement_fort_karkov
settlement_lock_seven
settlement_silo_burrow
settlement_st_nicholas
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/narrative/wasteland_settlement_gazetteer.json`

### `Assets/StreamingAssets/Data/narrative/wasteland_settlement_gazetteer.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 19523 bytes / 19522 characters.
- SHA-256: `e9592f6ee42a5ceaf867b96764dd97a3aa6869aa574882549d614035a6975536`.
- Root keys: `collection_id`, `schema_version`, `settlements`.

Array-path census (minimum, maximum, observed rows):

```text
settlements: min=20, max=20, observed_paths=1
settlements[].tags: min=6, max=6, observed_paths=2
```

Representative record fields:

- `colloquial_name`
- `controlling_faction`
- `defense_fortifications`
- `estimated_population`
- `geographic_coordinates`
- `harlan_scout_survey`
- `primary_export`
- `settlement_id`
- `settlement_name`
- `tags`
- `water_source_radiation_mrh`


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/wasteland_settlement_npcs.json`

### `Assets/StreamingAssets/Data/wasteland_settlement_npcs.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 30940 bytes / 30940 characters.
- SHA-256: `7bc358802a64c6baeb817f7cb44d3cdea5e1ec1cfa4a0af6665141aafdff23c1`.
- Root keys: `collection_id`, `npcs`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
npcs: min=18, max=18, observed_paths=1
npcs[].trade_tells: min=2, max=2, observed_paths=2
```

Representative record fields:

- `contradiction`
- `display_name`
- `faction`
- `fear`
- `greetings`
- `id`
- `personal_thread`
- `physical_anchor`
- `portrait_id`
- `profession`
- `role`
- `settlement_id`
- `sidework_quest_id`
- `trade_specialty`
- `trade_tells`
- `value`

Representative identifiers (ordered, capped for readability):

```text
npc_salt_marshal_varn
npc_salt_trader_elena
npc_salt_boiler_petyr
npc_switch_master_korov
npc_rail_chandler_bess
npc_rivet_smith_milos
npc_beacon_keeper_maren
npc_coastal_chandler_orlov
npc_net_mender_kira
npc_quarry_steward_darek
npc_stone_cutter_valya
npc_driller_jarek
npc_prior_silas
npc_almoner_hanna
npc_wayfarer_tobias
npc_market_warden_grimm
npc_junk_broker_solomon
npc_grease_monkey_tess
```


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/World/SettlementCatalogTests.cs`

### `Ashfall.Core.Tests/World/SettlementCatalogTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 268; SHA-256: `384153915207250d1af28a55243b54d7cc9a5e494f9410fe9e62e83db4cbfa5b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SettlementCatalog_LoadsAllTwelveAuthoredSettlements
SettlementCatalog_ArchetypeDistribution_ThreePerArchetype
SettlementCatalog_AllSettlementIds_UniqueAndPrefixed
SettlementCatalog_AllLocationLinks_ResolveInLocationsJson
SettlementCatalog_AllFactionAllegiances_Resolve
SettlementCatalog_AllTradeGoodsAndNeeds_ResolveInItemsJson
SettlementCatalog_TradeGoodsAndNeeds_HaveNoContradictoryOverlaps
SettlementCatalog_CaravanIntegration_FourCaravanRoutesIncludeSettlements
SettlementCatalog_ExpeditionIntegration_ThreeFriendlyStopsExist
```


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs`

### `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 158; SHA-256: `d3a4f1ba72662b18aa76830b5845a39dd7c25969398117089b6b0246c728a099`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SettlementAndTerritoryCatalogs_CrossValidateAllegiancesAndControlPoints
TerritoryControlSystem_SimulatesSettlementContestAndFortification
SettlementCatalog_QuestLifecycle_AndStateSaveRestoreRoundTrip
```


# Appendix E.13 — Supporting Code Evidence: `Assets/Ashfall.Core/World/SettlementCatalog.cs`

### `Assets/Ashfall.Core/World/SettlementCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 463 lines / 17110 bytes.
- SHA-256: `492fea4b385a078e5e94ed407fe2d74a9876a8143a92b87144db32bcb0d9d29e`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SettlementEconomy
public string PrimaryExport { get; set; } = string.Empty;
public string PrimaryImport { get; set; } = string.Empty;
public string TradeSpecialty { get; set; } = string.Empty;
public float PriceModifierExports { get; set; } = 1.0f;
public float PriceModifierImports { get; set; } = 1.0f;
public List<string> StockItemIds { get; set; } = new List<string>();
public sealed class SettlementSociety
public string Governance { get; set; } = string.Empty;
public int Population { get; set; } = 50;
public string CoreValue { get; set; } = string.Empty;
public string InternalTension { get; set; } = string.Empty;
public sealed class SettlementFactionRelation
public string PrimaryFaction { get; set; } = string.Empty;
public string StandingGateFaction { get; set; } = string.Empty;
public int MinStandingToEnter { get; set; } = 0;
public int HostileStandingThreshold { get; set; } = -40;
public sealed class SettlementDefinition
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string Archetype { get; set; } = string.Empty;
public string Region { get; set; } = string.Empty;
public string LocationId { get; set; } = string.Empty;
public string RouteNode { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public string SurvivalAdaptation { get; set; } = string.Empty;
public SettlementEconomy Economy { get; set; } = new SettlementEconomy();
public SettlementSociety Society { get; set; } = new SettlementSociety();
public SettlementFactionRelation FactionRelation { get; set; } = new SettlementFactionRelation();
public string LocationLink { get; set; } = string.Empty;
public int Population { get; set; } = 0;
public string Allegiance { get; set; } = string.Empty;
public int ThreatLevel { get; set; } = 2;
public string Attitude { get; set; } = "neutral";
public List<string> TradeGoods { get; set; } = new List<string>();
public List<string> TradeNeeds { get; set; } = new List<string>();
public string KeeperNpcId { get; set; } = string.Empty;
public string TraderNpcId { get; set; } = string.Empty;
public string FixtureNpcId { get; set; } = string.Empty;
public string SideworkQuestId { get; set; } = string.Empty;
public string GetEffectiveLocationId() => !string.IsNullOrEmpty(LocationLink) ? LocationLink : LocationId;
public int GetEffectivePopulation() => Population > 0 ? Population : (Society?.Population ?? 50);
public string GetEffectiveAllegiance() => !string.IsNullOrEmpty(Allegiance) ? Allegiance : (FactionRelation?.PrimaryFaction ?? "none");
public sealed class SettlementNpcGreeting
public string LowStanding { get; set; } = string.Empty;
public string Neutral { get; set; } = string.Empty;
public string HighStanding { get; set; } = string.Empty;
public sealed class SettlementNpcEntry
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string SettlementId { get; set; } = string.Empty;
public string Role { get; set; } = string.Empty; // "Keeper", "Trader", "Fixture"
public string Profession { get; set; } = string.Empty;
public string Faction { get; set; } = "none";
public string TradeSpecialty { get; set; } = string.Empty;
public string PhysicalAnchor { get; set; } = string.Empty;
public string Value { get; set; } = string.Empty;
public string Fear { get; set; } = string.Empty;
public string Contradiction { get; set; } = string.Empty;
public string PersonalThread { get; set; } = string.Empty;
public SettlementNpcGreeting Greetings { get; set; } = new SettlementNpcGreeting();
public List<string> TradeTells { get; set; } = new List<string>();
public string SideworkQuestId { get; set; } = string.Empty;
public string PortraitId { get; set; } = string.Empty;
public sealed class RepeatableQuestStage
public string Id { get; set; } = string.Empty;
public string Text { get; set; } = string.Empty;
public sealed class RepeatableQuestEntry
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string ProviderNpcId { get; set; } = string.Empty;
public string SettlementId { get; set; } = string.Empty;
public string Type { get; set; } = string.Empty;
public string Briefing { get; set; } = string.Empty;
public string PrereqQuestId { get; set; } = string.Empty;
public string TargetLocationId { get; set; } = string.Empty;
public int CooldownDays { get; set; } = 7;
public string RewardItemId { get; set; } = string.Empty;
public int RewardCount { get; set; } = 1;
public int StandingDelta { get; set; } = 5;
public List<RepeatableQuestStage> Stages { get; set; } = new List<RepeatableQuestStage>();
public sealed class SettlementState
public Dictionary<string, int> QuestAvailableDay { get; set; } = new Dictionary<string, int>();
public Dictionary<string, int> CompletedQuestCounts { get; set; } = new Dictionary<string, int>();
public sealed class SettlementCatalog
public int SettlementCount => _allSettlements.Count;
public int NpcCount => _allNpcs.Count;
public int QuestCount => _allQuests.Count;
public IReadOnlyList<SettlementDefinition> Settlements => _allSettlements;
public IReadOnlyList<SettlementNpcEntry> Npcs => _allNpcs;
public IReadOnlyList<RepeatableQuestEntry> Quests => _allQuests;
public static SettlementCatalog LoadFromDirectory(string directoryPath, IFileIO fileIO) {
public void LoadSettlementsJson(string json) {
public void LoadNpcsJson(string json) {
public void LoadQuestsJson(string json) {
public bool TryGetSettlement(string id, out SettlementDefinition settlement) {
public bool TryGetNpc(string id, out SettlementNpcEntry npc) {
public bool TryGetQuest(string id, out RepeatableQuestEntry quest) {
public string GetNpcGreeting(string npcId, float standing) {
public bool IsQuestAvailable(string questId, int currentDay) {
public void CompleteQuest(string questId, int currentDay) {
public int GetCompletedQuestCount(string questId) {
public SettlementState CaptureState() {
public void RestoreState(SettlementState? state) {
```


# Appendix E.14 — Supporting Code Evidence: `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs`

### `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 496 lines / 21443 bytes.
- SHA-256: `35df3dad7070ede8b234fe0745daf4a4f0ee5d5d2751aebf7635b9a100105213`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=9; textual Godot mentions=1; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class OutpostDef
public string Id { get; set; } = string.Empty;
public string Name { get; set; } = string.Empty;
public string GraphNodeId { get; set; } = string.Empty;
public int MaxGarrisonBunks { get; set; } = 4;
public int DailySupplyDemand { get; set; } = 4;
public int DefenseRating { get; set; } = 50;
public int RadioRelayRange { get; set; } = 10;
public Dictionary<string, int> BuildCost { get; set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
public sealed class OutpostInstance
public string OutpostId { get; set; } = string.Empty;
public string GraphNodeId { get; set; } = string.Empty;
public bool IsEstablished { get; set; }
public int ConditionPermille { get; set; } = 1000;
public List<string> GarrisonSurvivorIds { get; set; } = new List<string>();
public int DaysSinceSupply { get; set; }
public bool IsStarving { get; set; }
public bool IsOverrun { get; set; }
public int RationReserve { get; set; }
public sealed class OutpostInstanceState
public string outpost_id { get; set; } = string.Empty;
public bool is_established { get; set; }
public int condition_permille { get; set; } = 1000;
public List<string> garrison_survivor_ids { get; set; } = new List<string>();
public int days_since_supply { get; set; }
public bool is_starving { get; set; }
public bool is_overrun { get; set; }
public int ration_reserve { get; set; }
public sealed class OutpostSettlementState
public int schema_version { get; set; } = 1;
public List<OutpostInstanceState> outposts { get; set; } = new List<OutpostInstanceState>();
public sealed class OutpostSettlementSystem
public const string SystemId = "outpost_settlement_system";
public Action<string, string>? OnOutpostEstablishedSeam { get; set; }
public Action<string, int>? OnOutpostSuppliedSeam { get; set; }
public Action<string>? OnOutpostOverrunSeam { get; set; }
public Action<string, string>? OnGarrisonAssignedSeam { get; set; }
public Action<string, string>? OnGarrisonRelievedSeam { get; set; }
public Action<string>? OnOutpostStarvingSeam { get; set; }
public static OutpostSettlementSystem FromJson(string json) {
public OutpostDef? GetDefinition(string outpostId) {
public OutpostInstance? GetInstance(string outpostId) {
public IReadOnlyList<OutpostDef> GetAllDefinitions() => _definitions.Values.ToList();
public IReadOnlyList<OutpostInstance> GetAllInstances() => _instances.Values.ToList();
public bool EstablishOutpost(string outpostId, Func<string, int, bool>? costConsumer = null) {
public bool TryEstablishOutpost(string outpostId, IPlayerInventoryPort? inventory) {
public bool AssignGarrison(string outpostId, string survivorId, Func<string, bool>? fitnessCheck = null) {
public bool RelieveGarrison(string outpostId, string survivorId) {
public bool SupplyOutpost(string outpostId, int rationsDelivered) {
public bool TrySupplyOutpost( string outpostId, string rationItemId, int rationsDelivered, IPlayerInventoryPort? inventory) {
public void TickDay(Func<string, int, int>? centralRationSupplyProvider = null) {
public bool SimulateRisk(string outpostId, int dangerRating, ISeededRng rng) {
public bool AbandonOutpost(string outpostId) {
public OutpostSettlementState CaptureState() {
public bool RestoreState(OutpostSettlementState? state) {
```


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs`

### `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 622 lines / 28809 bytes.
- SHA-256: `bc27b87874570a17bb5d1990ebce70451fac2737edbc6cb4bbc76f05ba31dbff`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=7; textual Godot mentions=1; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum SupplyLineStatus
public sealed class FactionTerritoryDef
public string Id { get; set; } = string.Empty;
public string Faction { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string Classification { get; set; } = string.Empty;
public string TerritoryScale { get; set; } = string.Empty;
public string PrimaryResourceInterest { get; set; } = string.Empty;
public List<string> ControlledNodes { get; set; } = new List<string>();
public List<string> ControlPoints { get; set; } = new List<string>();
public List<string> ContestedWith { get; set; } = new List<string>();
public int BaseControlStrength { get; set; } = 50;
public double TradeTax { get; set; } = 0.05;
public double TravelSafety { get; set; } = 0.75;
public string ShiftTrigger { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public sealed class SupplyLineDef
public string Id { get; set; } = string.Empty;
public string OwningFactionId { get; set; } = string.Empty;
public string OriginLocationId { get; set; } = string.Empty;
public string DestinationLocationId { get; set; } = string.Empty;
public List<string> RouteWaypoints { get; set; } = new List<string>();
public string CargoType { get; set; } = string.Empty;
public int ThroughputCapacity { get; set; } = 30;
public int TravelDays { get; set; } = 2;
public sealed class LocationTerritoryState
public string LocationId { get; set; } = string.Empty;
public string ControllingFactionId { get; set; } = string.Empty;
public int ControlStrength { get; set; } = 50;
public bool IsContested { get; set; }
public int FortificationLevel { get; set; }
public int GarrisonStrength { get; set; }
public int LastContestDay { get; set; }
public sealed class SupplyLineState
public string SupplyLineId { get; set; } = string.Empty;
public string OwningFactionId { get; set; } = string.Empty;
public SupplyLineStatus Status { get; set; } = SupplyLineStatus.Active;
public int LastDeliveredDay { get; set; }
public int TotalDelivered { get; set; }
public sealed class TerritoryControlSystem
public const string SystemId = "territory_control_system";
public Action<string, string, string>? OnTerritoryControlChangedSeam { get; set; }
public Action<string, string, string>? OnTerritoryContestedSeam { get; set; }
public Action<string, SupplyLineStatus>? OnSupplyLineStatusChangedSeam { get; set; }
public Action<string, int>? OnSupplyLineDeliveredSeam { get; set; }
public Action<string, int>? OnLocationFortifiedSeam { get; set; }
public static TerritoryControlSystem FromJson(string territoryJson, string supplyLineJson) {
public FactionTerritoryDef? GetTerritory(string territoryId) {
public LocationTerritoryState? GetLocationState(string locationId) {
public SupplyLineState? GetSupplyLineState(string lineId) {
public IReadOnlyList<FactionTerritoryDef> GetAllTerritories() => _territories.Values.ToList();
public IReadOnlyList<LocationTerritoryState> GetAllLocationStates() => _locationStates.Values.ToList();
public IReadOnlyList<SupplyLineState> GetAllSupplyLines() => _supplyLineStates.Values.ToList();
public bool FortifyLocation(string locationId, int levelDelta = 1) {
public bool AssignGarrison(string locationId, int garrisonDelta) {
public bool ContestLocation(string locationId, string attackingFactionId, int attackPower, ISeededRng rng, int currentDay = 0) {
public bool RaidSupplyLine(string supplyLineId, int raidIntensity, ISeededRng rng) {
public bool RestoreSupplyLine(string supplyLineId) {
public void TickDay(int currentDay, ISeededRng? rng = null) {
public TerritoryControlSaveState CaptureState() {
public bool RestoreState(TerritoryControlSaveState? state) {
public TerritoryCensus ReadCensus() {
public sealed class LocationTerritorySaveState
public string location_id { get; set; } = string.Empty;
public string controlling_faction_id { get; set; } = string.Empty;
public int control_strength { get; set; } = 50;
public bool is_contested { get; set; }
public int fortification_level { get; set; }
public int garrison_strength { get; set; }
public int last_contest_day { get; set; }
public sealed class SupplyLineSaveState
public string supply_line_id { get; set; } = string.Empty;
public string owning_faction_id { get; set; } = string.Empty;
public string status { get; set; } = "Active";
public int last_delivered_day { get; set; }
public int total_delivered { get; set; }
public sealed class TerritoryControlSaveState
public int schema_version { get; set; } = 1;
public List<LocationTerritorySaveState> locations { get; set; } = new List<LocationTerritorySaveState>();
public List<SupplyLineSaveState> supply_lines { get; set; } = new List<SupplyLineSaveState>();
public readonly struct TerritoryCensus
public readonly int TerritoriesCount;
public readonly int LocationsCount;
public readonly int ContestedLocationsCount;
public readonly int TotalSupplyLines;
public readonly int ActiveSupplyLines;
public readonly int DisruptedSupplyLines;
public readonly int SeveredSupplyLines;
public int TotalTerritories => TerritoriesCount;
public int TotalNodes => LocationsCount;
public int ContestedLocations => ContestedLocationsCount;
public string Describe() =>
```


# Appendix E.16 — Supporting Code Evidence: `src/Host/OutpostSettlementHostSession.cs`

### `src/Host/OutpostSettlementHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 214 lines / 9863 bytes.
- SHA-256: `bf8575bf7708e497711c6e5e06c6448c230055b52b358da3552a4646bd0a5d52`.
- Architecture signals: seeded references=1; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class OutpostSettlementHostSession : HostSessionBase
public const string CatalogFile = "outposts.json";
public OutpostSettlementSystem System { get; }
public bool CatalogLoaded { get; private set; }
public static OutpostSettlementHostSession Load(string dataDirectory, IFileIO files) {
public OutpostCensus ReadCensus() {
public bool Establish(string outpostId, Func<string, int, bool>? costConsumer = null) {
public bool TryEstablish(string outpostId, IPlayerInventoryPort? inventory) {
public bool AssignGarrison(string outpostId, string survivorId, Func<string, bool>? fitnessCheck = null) {
public bool RelieveGarrison(string outpostId, string survivorId) {
public bool Supply(string outpostId, int rationsDelivered) {
public bool TrySupply(string outpostId, string rationItemId, int rationsDelivered, IPlayerInventoryPort? inventory) {
public bool Abandon(string outpostId) {
public void TickDay(Func<string, int, int>? centralRationSupplyProvider = null) {
public bool SimulateRisk(string outpostId, int dangerRating, ISeededRng rng) => System.SimulateRisk(outpostId ?? string.Empty, dangerRating, rng);
public OutpostSettlementState CaptureState() => System.CaptureState();
public bool RestoreState(OutpostSettlementState? state) => System.RestoreState(state);
public void AppendReportLine(string line) {
public IReadOnlyList<string> ReportLines => _rawJsonLines;
public string Describe() => ReadCensus().Describe();
public readonly struct OutpostCensus
public readonly int Authored;
public readonly int Established;
public readonly int Overrun;
public readonly int Starving;
public readonly int Garrisoned;
public readonly int RationReserve;
public string Describe() => $"outposts: {Established}/{Authored} established, {Garrisoned} garrisoned, "
public static class OutpostSettlementSaveStore
public const string FileName = "outpost_settlement_save.json";
public const string SectionName = "outpost_settlement";
public static bool TrySave(OutpostSettlementState state) => s_store.TrySave(state);
public static OutpostSettlementState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(OutpostSettlementState state) => s_store.CapturePersisted(state);
public static OutpostSettlementState? TryRestore(string json) => s_store.RestoreEnvelope(json);
public static OutpostSettlementState? TryRestoreBare(string json) => s_store.RestoreBare(json);
```


# Appendix E.17 — Supporting Code Evidence: `src/Host/OutpostSettlementHostSession.cs`

### `src/Host/OutpostSettlementHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 214 lines / 9863 bytes.
- SHA-256: `bf8575bf7708e497711c6e5e06c6448c230055b52b358da3552a4646bd0a5d52`.
- Architecture signals: seeded references=1; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class OutpostSettlementHostSession : HostSessionBase
public const string CatalogFile = "outposts.json";
public OutpostSettlementSystem System { get; }
public bool CatalogLoaded { get; private set; }
public static OutpostSettlementHostSession Load(string dataDirectory, IFileIO files) {
public OutpostCensus ReadCensus() {
public bool Establish(string outpostId, Func<string, int, bool>? costConsumer = null) {
public bool TryEstablish(string outpostId, IPlayerInventoryPort? inventory) {
public bool AssignGarrison(string outpostId, string survivorId, Func<string, bool>? fitnessCheck = null) {
public bool RelieveGarrison(string outpostId, string survivorId) {
public bool Supply(string outpostId, int rationsDelivered) {
public bool TrySupply(string outpostId, string rationItemId, int rationsDelivered, IPlayerInventoryPort? inventory) {
public bool Abandon(string outpostId) {
public void TickDay(Func<string, int, int>? centralRationSupplyProvider = null) {
public bool SimulateRisk(string outpostId, int dangerRating, ISeededRng rng) => System.SimulateRisk(outpostId ?? string.Empty, dangerRating, rng);
public OutpostSettlementState CaptureState() => System.CaptureState();
public bool RestoreState(OutpostSettlementState? state) => System.RestoreState(state);
public void AppendReportLine(string line) {
public IReadOnlyList<string> ReportLines => _rawJsonLines;
public string Describe() => ReadCensus().Describe();
public readonly struct OutpostCensus
public readonly int Authored;
public readonly int Established;
public readonly int Overrun;
public readonly int Starving;
public readonly int Garrisoned;
public readonly int RationReserve;
public string Describe() => $"outposts: {Established}/{Authored} established, {Garrisoned} garrisoned, "
public static class OutpostSettlementSaveStore
public const string FileName = "outpost_settlement_save.json";
public const string SectionName = "outpost_settlement";
public static bool TrySave(OutpostSettlementState state) => s_store.TrySave(state);
public static OutpostSettlementState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(OutpostSettlementState state) => s_store.CapturePersisted(state);
public static OutpostSettlementState? TryRestore(string json) => s_store.RestoreEnvelope(json);
public static OutpostSettlementState? TryRestoreBare(string json) => s_store.RestoreBare(json);
```


# Appendix F.18 — Supporting Data Evidence: `Assets/StreamingAssets/Data/settlements.json`

### `Assets/StreamingAssets/Data/settlements.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 24974 bytes / 24968 characters.
- SHA-256: `bd77179e5c1ff6878f087579168ded303a2b78679b93e86920f38a7c28924ec3`.
- Root keys: `collection_id`, `schema_version`, `settlements`.

Array-path census (minimum, maximum, observed rows):

```text
settlements: min=12, max=12, observed_paths=1
settlements[].economy.stock_item_ids: min=3, max=3, observed_paths=2
settlements[].trade_goods: min=4, max=4, observed_paths=2
settlements[].trade_needs: min=3, max=4, observed_paths=2
```

Representative record fields:

- `allegiance`
- `archetype`
- `attitude`
- `description`
- `display_name`
- `economy`
- `faction_relation`
- `fixture_npc_id`
- `id`
- `keeper_npc_id`
- `location_id`
- `location_link`
- `population`
- `region`
- `route_node`
- `sidework_quest_id`
- `society`
- `survival_adaptation`
- `threat_level`
- `trade_goods`
- `trade_needs`
- `trader_npc_id`

Representative identifiers (ordered, capped for readability):

```text
settlement_brine_pans
settlement_iron_siding
settlement_cape_beacon
settlement_slate_hollow
settlement_pilgrim_hearth
settlement_tinkers_notch
settlement_ferry_crossing
settlement_nine_rails
settlement_fort_karkov
settlement_lock_seven
settlement_silo_burrow
settlement_st_nicholas
```


# Appendix F.19 — Supporting Data Evidence: `Assets/StreamingAssets/Data/narrative/wasteland_settlement_gazetteer.json`

### `Assets/StreamingAssets/Data/narrative/wasteland_settlement_gazetteer.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 19523 bytes / 19522 characters.
- SHA-256: `e9592f6ee42a5ceaf867b96764dd97a3aa6869aa574882549d614035a6975536`.
- Root keys: `collection_id`, `schema_version`, `settlements`.

Array-path census (minimum, maximum, observed rows):

```text
settlements: min=20, max=20, observed_paths=1
settlements[].tags: min=6, max=6, observed_paths=2
```

Representative record fields:

- `colloquial_name`
- `controlling_faction`
- `defense_fortifications`
- `estimated_population`
- `geographic_coordinates`
- `harlan_scout_survey`
- `primary_export`
- `settlement_id`
- `settlement_name`
- `tags`
- `water_source_radiation_mrh`


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/World/SettlementCatalogTests.cs`

### `Ashfall.Core.Tests/World/SettlementCatalogTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 268; SHA-256: `384153915207250d1af28a55243b54d7cc9a5e494f9410fe9e62e83db4cbfa5b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SettlementCatalog_LoadsAllTwelveAuthoredSettlements
SettlementCatalog_ArchetypeDistribution_ThreePerArchetype
SettlementCatalog_AllSettlementIds_UniqueAndPrefixed
SettlementCatalog_AllLocationLinks_ResolveInLocationsJson
SettlementCatalog_AllFactionAllegiances_Resolve
SettlementCatalog_AllTradeGoodsAndNeeds_ResolveInItemsJson
SettlementCatalog_TradeGoodsAndNeeds_HaveNoContradictoryOverlaps
SettlementCatalog_CaravanIntegration_FourCaravanRoutesIncludeSettlements
SettlementCatalog_ExpeditionIntegration_ThreeFriendlyStopsExist
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs`

### `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 158; SHA-256: `d3a4f1ba72662b18aa76830b5845a39dd7c25969398117089b6b0246c728a099`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SettlementAndTerritoryCatalogs_CrossValidateAllegiancesAndControlPoints
TerritoryControlSystem_SimulatesSettlementContestAndFortification
SettlementCatalog_QuestLifecycle_AndStateSaveRestoreRoundTrip
```


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| settlement definitions, NPCs, quests and catalog state | SettlementCatalog | player-founded outposts, supply, garrison and persistence | OutpostSettlementSystem | Owner emits/reads a typed fact; no mirror state. |
| settlement definitions, NPCs, quests and catalog state | SettlementCatalog | current faction control and territory shifts | TerritoryControlSystem | Owner emits/reads a typed fact; no mirror state. |
| settlement definitions, NPCs, quests and catalog state | SettlementCatalog | host load/commands/save | OutpostSettlementHostSession | Owner emits/reads a typed fact; no mirror state. |
| settlement definitions, NPCs, quests and catalog state | SettlementCatalog | current catalog/settlement diagnostic path | HostCli.WastelandInhabitants | Owner emits/reads a typed fact; no mirror state. |
| settlement definitions, NPCs, quests and catalog state | SettlementCatalog | settlement/territory cross-validation | Plan43_44 tests | Owner emits/reads a typed fact; no mirror state. |
| player-founded outposts, supply, garrison and persistence | OutpostSettlementSystem | settlement definitions, NPCs, quests and catalog state | SettlementCatalog | Owner emits/reads a typed fact; no mirror state. |
| player-founded outposts, supply, garrison and persistence | OutpostSettlementSystem | current faction control and territory shifts | TerritoryControlSystem | Owner emits/reads a typed fact; no mirror state. |
| player-founded outposts, supply, garrison and persistence | OutpostSettlementSystem | host load/commands/save | OutpostSettlementHostSession | Owner emits/reads a typed fact; no mirror state. |
| player-founded outposts, supply, garrison and persistence | OutpostSettlementSystem | current catalog/settlement diagnostic path | HostCli.WastelandInhabitants | Owner emits/reads a typed fact; no mirror state. |
| player-founded outposts, supply, garrison and persistence | OutpostSettlementSystem | settlement/territory cross-validation | Plan43_44 tests | Owner emits/reads a typed fact; no mirror state. |
| current faction control and territory shifts | TerritoryControlSystem | settlement definitions, NPCs, quests and catalog state | SettlementCatalog | Owner emits/reads a typed fact; no mirror state. |
| current faction control and territory shifts | TerritoryControlSystem | player-founded outposts, supply, garrison and persistence | OutpostSettlementSystem | Owner emits/reads a typed fact; no mirror state. |
| current faction control and territory shifts | TerritoryControlSystem | host load/commands/save | OutpostSettlementHostSession | Owner emits/reads a typed fact; no mirror state. |
| current faction control and territory shifts | TerritoryControlSystem | current catalog/settlement diagnostic path | HostCli.WastelandInhabitants | Owner emits/reads a typed fact; no mirror state. |
| current faction control and territory shifts | TerritoryControlSystem | settlement/territory cross-validation | Plan43_44 tests | Owner emits/reads a typed fact; no mirror state. |
| host load/commands/save | OutpostSettlementHostSession | settlement definitions, NPCs, quests and catalog state | SettlementCatalog | Owner emits/reads a typed fact; no mirror state. |
| host load/commands/save | OutpostSettlementHostSession | player-founded outposts, supply, garrison and persistence | OutpostSettlementSystem | Owner emits/reads a typed fact; no mirror state. |
| host load/commands/save | OutpostSettlementHostSession | current faction control and territory shifts | TerritoryControlSystem | Owner emits/reads a typed fact; no mirror state. |
| host load/commands/save | OutpostSettlementHostSession | current catalog/settlement diagnostic path | HostCli.WastelandInhabitants | Owner emits/reads a typed fact; no mirror state. |
| host load/commands/save | OutpostSettlementHostSession | settlement/territory cross-validation | Plan43_44 tests | Owner emits/reads a typed fact; no mirror state. |
| current catalog/settlement diagnostic path | HostCli.WastelandInhabitants | settlement definitions, NPCs, quests and catalog state | SettlementCatalog | Owner emits/reads a typed fact; no mirror state. |
| current catalog/settlement diagnostic path | HostCli.WastelandInhabitants | player-founded outposts, supply, garrison and persistence | OutpostSettlementSystem | Owner emits/reads a typed fact; no mirror state. |
| current catalog/settlement diagnostic path | HostCli.WastelandInhabitants | current faction control and territory shifts | TerritoryControlSystem | Owner emits/reads a typed fact; no mirror state. |
| current catalog/settlement diagnostic path | HostCli.WastelandInhabitants | host load/commands/save | OutpostSettlementHostSession | Owner emits/reads a typed fact; no mirror state. |
| current catalog/settlement diagnostic path | HostCli.WastelandInhabitants | settlement/territory cross-validation | Plan43_44 tests | Owner emits/reads a typed fact; no mirror state. |
| settlement/territory cross-validation | Plan43_44 tests | settlement definitions, NPCs, quests and catalog state | SettlementCatalog | Owner emits/reads a typed fact; no mirror state. |
| settlement/territory cross-validation | Plan43_44 tests | player-founded outposts, supply, garrison and persistence | OutpostSettlementSystem | Owner emits/reads a typed fact; no mirror state. |
| settlement/territory cross-validation | Plan43_44 tests | current faction control and territory shifts | TerritoryControlSystem | Owner emits/reads a typed fact; no mirror state. |
| settlement/territory cross-validation | Plan43_44 tests | host load/commands/save | OutpostSettlementHostSession | Owner emits/reads a typed fact; no mirror state. |
| settlement/territory cross-validation | Plan43_44 tests | current catalog/settlement diagnostic path | HostCli.WastelandInhabitants | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Current evidence requires a bounded owner/reachability audit; no new authority is implied. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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

> ### Recommended integration route
Tier: audit (DOCS-ONLY) then DATA-ONLY corpus authoring, with HOST-WIRING only for confirmed unconsumed catalogs. Seams: corpus twins into `Assets/StreamingAssets/Data/narrative/` following the assay-report genre contracts; consumption wiring through the named host sessions (`HydraulicExtrusion` session confirmed live in the v1.0 host inventory). Verification: data-integrity selftest, content-utilization selftest (the decisive gate — presence is not reachability), focused loader tests.

> ### Recommended integration route
Tier: DATA-ONLY with HOST-WIRING verification. Seams: muster catalog family → existing muster loaders (verify integrity rules cover the newer catalogs — if the loader family predates them, extend it) → `MusterSystem` action selection → witness evidence into the Reckoning enrollment path → muster epilogue evaluation. Save impact class: NONE expected (catalog-driven), unless witness state persists — then EXISTING-SECTION with verification. Verification: integrity + utilization selftests, focused muster tests, one epilogue-reachability test for the new witness chains.

> ### Recommended integration route
Tier: DOCS-ONLY audit then DATA-ONLY prose. Seams: audit report into `docs/endgame/`; chronicle entries into the epilogue chronicle catalog through its existing loader; enrollment through existing Reckoning owners only where an audit row shows a system whose evidence cannot reach any permutation. Verification: integrity selftest, utilization selftest, focused endgame tests, epilogue-reachability tests for touched permutations.

> ### Premise evidence
VERIFIED: the census document exists (DR-08). VERIFIED: the content-utilization selftest exists and encodes the principle "presence in JSON is not reachability" (v1.0 Invariant 6).

> **B-11 · C7 · Black-market funds legs.** Subject: canonical funds authority for black-market settlement. Evidence: decision-blocked (needs canonical funds authority — v1.0 Part 7 gap 4); the actions surface itself was sealed by `WAVE8-PART2-C1-BLACK-MARKET-ACTIONS` (DR-06). Status: GATE. Confidence: VERIFIED as blocked.

> **C-06 · C7 · Embargo pressure modeling.** Subject: `trade_embargoes.json` impact on settlement price bands; verify embargoes produce legible price signal, not noise. Evidence: embargo catalog verified live. Route: harness. Confidence: HIGH CONFIDENCE.

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

Current evidence and safe integration boundary for Plan 43: Settlement Gazetteer, Allegiance and Living Community Reachability.

- **settlement definitions, NPCs, quests and catalog state** remains with `SettlementCatalog` at `Assets/Ashfall.Core/World/SettlementCatalog.cs`. Static/world-reference owner.
- **player-founded outposts, supply, garrison and persistence** remains with `OutpostSettlementSystem` at `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs`. Separate mutable outpost owner.
- **current faction control and territory shifts** remains with `TerritoryControlSystem` at `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs`. Separate mutable control owner.
- **host load/commands/save** remains with `OutpostSettlementHostSession` at `src/Host/OutpostSettlementHostSession.cs`. Thin host seam.
- **current catalog/settlement diagnostic path** remains with `HostCli.WastelandInhabitants` at `src/Host/HostCli.WastelandInhabitants.cs`. Diagnostic/consumer evidence.
- **settlement/territory cross-validation** remains with `Plan43_44 tests` at `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs`. Focused evidence.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load 12 settlement definitions
2. validate locations, allegiances and trade references
3. read current territory/outpost state
4. present settlement information and quest availability
5. route actual trade/travel/outpost commands through owners
6. capture catalog quest state and current owner envelopes

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Settlement catalog rows and catalog quest state are one static/current catalog concern.
- Outposts, colonies and territory control retain separate mutable state.
- Trade goods and threat fields are not executable until current trade/patrol owners consume them.
- No new settlement save section.

- A settlement location must resolve before travel/trade presentation.
- Catalog allegiance does not overwrite current territory control.
- A settlement is not automatically a player outpost/colony.
- Quest availability uses current day/cooldown state.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/OutpostSettlementHostSession.cs
- src/Host/HostCli.WastelandInhabitants.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/World/SettlementCatalogTests.cs
- Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs

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
| S-01 | 43-01 12 rows load | load 12 settlement definitions | Settlement catalog rows and catalog quest state are one static/current catalog concern. | Catalog settlement is presented as a player colony. | SettlementCatalog |
| S-02 | 43-02 location refs | validate locations, allegiances and trade references | Outposts, colonies and territory control retain separate mutable state. | Static allegiance overwrites territory control. | SettlementCatalog |
| S-03 | 43-03 allegiance refs | read current territory/outpost state | Trade goods and threat fields are not executable until current trade/patrol owners consume them. | A second population ledger is created. | SettlementCatalog |
| S-04 | 43-04 quest availability | present settlement information and quest availability | No new settlement save section. | Trade/travel buttons have no command. | SettlementCatalog |
| S-05 | 43-05 cooldown save | route actual trade/travel/outpost commands through owners | Settlement catalog rows and catalog quest state are one static/current catalog concern. | A new save section duplicates current state. | SettlementCatalog |
| S-06 | 43-06 territory cross-check | capture catalog quest state and current owner envelopes | Outposts, colonies and territory control retain separate mutable state. | Catalog settlement is presented as a player colony. | SettlementCatalog |
| S-07 | 43-07 outpost distinction | load 12 settlement definitions | Trade goods and threat fields are not executable until current trade/patrol owners consume them. | Static allegiance overwrites territory control. | SettlementCatalog |
| S-08 | 43-08 travel unavailable state | validate locations, allegiances and trade references | No new settlement save section. | A second population ledger is created. | SettlementCatalog |
| S-09 | 43-09 UI truth | read current territory/outpost state | Settlement catalog rows and catalog quest state are one static/current catalog concern. | Trade/travel buttons have no command. | SettlementCatalog |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 43-TC-01 schema/count | data | schema/count; verify the named current owner and its negative boundary without inventing a second authority. | SettlementCatalog |
| T-02 | 43-TC-02 reference validation | unit | reference validation; verify the named current owner and its negative boundary without inventing a second authority. | SettlementCatalog |
| T-03 | 43-TC-03 catalog quest lifecycle | persistence | catalog quest lifecycle; verify the named current owner and its negative boundary without inventing a second authority. | SettlementCatalog |
| T-04 | 43-TC-04 save round trip | determinism | save round trip; verify the named current owner and its negative boundary without inventing a second authority. | SettlementCatalog |
| T-05 | 43-TC-05 territory cross-check | host | territory cross-check; verify the named current owner and its negative boundary without inventing a second authority. | SettlementCatalog |
| T-06 | 43-TC-06 outpost separation | UI/accessibility | outpost separation; verify the named current owner and its negative boundary without inventing a second authority. | SettlementCatalog |
| T-07 | 43-TC-07 travel label | cross-system | travel label; verify the named current owner and its negative boundary without inventing a second authority. | SettlementCatalog |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 18 | `Ashfall.Core.Tests/World/SettlementCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 11 | `src/Host/OutpostSettlementHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `src/Host/HostCli.OutpostSettlement.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `src/Main.OutpostSettlement.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/Settlements/Plan58OutpostSettlementIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Settlements/Plan58OutpostHostIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/World/Plan20WastelandInhabitantsTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/TerritoryControlHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/World/SettlementCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Campaign/Plan55RetentionHostIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Factions/Plan134TerritoryControlHostIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Settlements/OutpostAtomicBillTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Host/HostCli.WastelandInhabitants.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Host/ShelterOperationsHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Main.TerritoryControl.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/Integration/ShelterOperationsBoardWiringTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/World/NightWatchReadinessProjection.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/HostCli.ShelterOperations.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/settlements.json`

### `Assets/StreamingAssets/Data/settlements.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 24974; characters: 24968.
- SHA-256: `bd77179e5c1ff6878f087579168ded303a2b78679b93e86920f38a7c28924ec3`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `collection_id`, `settlements`

#### `settlements` — 12 current rows

- Row 001 `settlement_brine_pans`: `{"allegiance":"faction_the_cutters","archetype":"Salt Camp","attitude":"friendly","description":"A fortified evaporation basin in the tidal estuary, where salt freeholders boil brine in salvaged boiler pans and make the only preservative t…`
- Row 002 `settlement_iron_siding`: `{"allegiance":"faction_deserter_coalition","archetype":"Rail Siding Town","attitude":"wary","description":"A rail-car community fitted inside an armored siding under the earthworks, where artisans forge hardware and armor plate from decomm…`
- Row 003 `settlement_cape_beacon`: `{"allegiance":"faction_black_flotilla","archetype":"Coastal Lighthouse Commune","attitude":"wary","description":"A maritime outpost clustered around an intact lighthouse on the coastal bluff, running water condensers, kelp beds, and the co…`
- Row 004 `settlement_slate_hollow`: `{"allegiance":"faction_cold_count","archetype":"Quarry Enclave","attitude":"neutral","description":"A subterranean quarry redoubt cut into impermeable slate in the High Scarp, where pit crews split roofing slate, millstones, and hones by t…`
- Row 005 `settlement_pilgrim_hearth`: `{"allegiance":"faction_long_walk","archetype":"Religious / Monastic Sanctuary","attitude":"friendly","description":"A mountain priory built over geothermal steam vents at Switchback Pass, offering warm lodging, herbal medicine, and sanctua…`
- Row 006 `settlement_tinkers_notch`: `{"allegiance":"faction_scavenger_guild","archetype":"Free Trader Scrap Market","attitude":"neutral","description":"A sprawling swap meet built from shipping containers and bus chassis behind an electrified fence — the regional crossroads f…`
- Row 007 `settlement_ferry_crossing`: `{"allegiance":"faction_undertow","archetype":"Trade Post","attitude":"wary","description":"A river ferry landing and floating barge exchange holding the western estuary crossings, where salters, trappers, and haulers barter passage and wat…`
- Row 008 `settlement_nine_rails`: `{"allegiance":"faction_the_office","archetype":"Trade Post","attitude":"neutral","description":"A sheltered railway concourse where four industrial spurs converge: allocation clerks register freight weights while armed merchants barter scr…`
- Row 009 `settlement_fort_karkov`: `{"allegiance":"faction_deserter_coalition","archetype":"Faction Stronghold","attitude":"hostile","description":"A militarized railhead bastion guarded by sandbagged diesel engines and perimeter gun towers, where sentries demand transit pas…`
- Row 010 `settlement_lock_seven`: `{"allegiance":"faction_the_tally","archetype":"Faction Stronghold","attitude":"wary","description":"A reinforced concrete sluice fortress controlling the canal waterway — and everyone who uses it. Tally enforcers take the gate fee in fuel …`
- Row 011 `settlement_silo_burrow`: `{"allegiance":"faction_grain_exchange","archetype":"Refugee Camp","attitude":"friendly","description":"An agrarian refugee commune in three pre-war grain silos joined by earthen trenches, raising winter grain under constant raider threat. …`
- Row 012 `settlement_st_nicholas`: `{"allegiance":"faction_quiet_house","archetype":"Religious / Ideological Community","attitude":"friendly","description":"A quiet monastic sanctuary over a subterranean artesian spring beneath old stone crypts, where silent caretakers offer…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/narrative/wasteland_settlement_gazetteer.json`

### `Assets/StreamingAssets/Data/narrative/wasteland_settlement_gazetteer.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 19523; characters: 19522.
- SHA-256: `e9592f6ee42a5ceaf867b96764dd97a3aa6869aa574882549d614035a6975536`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `collection_id`, `settlements`

#### `settlements` — 20 current rows

- Row 001 `settlement_01_fort_karkov_rail_garrison`: `{"colloquial_name":"The Iron Gantry","controlling_faction":"faction_the_garrison","defense_fortifications":"Armor-plated DR-1 diesel railcars positioned as perimeter bastions, razor wire entanglement, twin DShK heavy machine gun nests in t…`
- Row 002 `settlement_02_lock_seven_canal_tollgate`: `{"colloquial_name":"The Sluice Gate Barony","controlling_faction":"faction_the_canal_smugglers","defense_fortifications":"Reinforced concrete sluice gate control bunker, submerged spike chains across the channel, swivel-mounted 12-gauge sc…`
- Row 003 `settlement_03_the_sunken_grain_silo_collective`: `{"colloquial_name":"The Silo Burrow","controlling_faction":"faction_the_commune","defense_fortifications":"Three 40-meter cylindrical concrete grain silos linked by underground trenches, earthen berms, and trained Caucasian shepherd guard …`
- Row 004 `settlement_04_church_of_the_broken_bell`: `{"colloquial_name":"The Silent Crypts","controlling_faction":"faction_the_sisterhood","defense_fortifications":"Thick stone monastery walls, collapsed bell tower lookout, concealed punji pit traps in the pine grove.","estimated_population"…`
- Row 005 `settlement_05_the_black_cinder_slag_camp`: `{"colloquial_name":"Slag City","controlling_faction":"faction_the_miners_union","defense_fortifications":"Earthen coal slag ramparts 6 meters high, acetylene flame projectors, pneumatic rock-drill spears.","estimated_population":120,"geogr…`
- Row 006 `settlement_06_the_drowned_sanatorium_baths`: `{"colloquial_name":"The Steam Ruins","controlling_faction":"faction_the_leechers","defense_fortifications":"Flooded marble colonnades, scalding steam vent traps, barricaded granite hydrotherapy halls.","estimated_population":45,"geographic…`
- Row 007 `settlement_07_the_crashed_antheus_cargo_hull`: `{"colloquial_name":"The Aluminium Whale","controlling_faction":"faction_the_scavengers","defense_fortifications":"Fuselage bulkheads converted into multi-tier watch cabins, 23mm tail turret adapted to hand crank operation.","estimated_popu…`
- Row 008 `settlement_08_hydro_dam_nine_overlook`: `{"colloquial_name":"The Concrete Wall","controlling_faction":"faction_the_grid_engineers","defense_fortifications":"Monolithic 80-meter concrete gravity dam crest, spillway sluice gates, searchlight towers with high-voltage perimeter fence…`
- Row 009 `settlement_09_the_rust_crawler_convoy_camp`: `{"colloquial_name":"The Steel Sled","controlling_faction":"faction_the_nomad_clans","defense_fortifications":"Circle of six tracked logging tractors with armored cab plates, spiked snowplow blades, and mobile sniper cradles.","estimated_po…`
- Row 010 `settlement_10_the_blind_substation_market`: `{"colloquial_name":"The Neutral Spark","controlling_faction":"faction_the_free_traders","defense_fortifications":"Demilitarized 500-meter perimeter enforced by neutral mercenary snipers; all weapons tied with wire seals at the gate.","esti…`
- Row 011 `settlement_11_the_quarry_pit_dungeon`: `{"colloquial_name":"The Sledge Floor","controlling_faction":"faction_the_quarry_barons","defense_fortifications":"40-meter vertical sheer rock faces with single winch elevator hoist, floodlight platforms, and guard towers armed with huntin…`
- Row 012 `settlement_12_the_glass_desert_observatory`: `{"colloquial_name":"The Cloud Eyrie","controlling_faction":"faction_the_watchers","defense_fortifications":"Sheer icy cliff approach accessible only via a hand-cranked cable car basket; reinforced copper dome.","estimated_population":18,"g…`
- Row 013 `settlement_13_the_flooded_subway_terminal`: `{"colloquial_name":"The Tile Vaults","controlling_faction":"faction_the_tunnel_rats","defense_fortifications":"Steel blast gates wedged half-open with sandbag parapets, tripwire tin-can alarm lines, and subterranean flooded culvert barrier…`
- Row 014 `settlement_14_the_leaded_glass_greenhouse_dome`: `{"colloquial_name":"The Winter Eden","controlling_faction":"faction_the_botanists","defense_fortifications":"Geodesic double-walled lead crystal glass domes, thermal curtain shutters, armed botanist militia with hunting shotguns.","estimat…`
- Row 015 `settlement_15_the_asphalt_refinery_still`: `{"colloquial_name":"The Tar Cauldron","controlling_faction":"faction_the_pitch_burners","defense_fortifications":"Motes of sticky liquid asphalt, boiling pitch spray nozzles, and corrugated iron palisades.","estimated_population":40,"geogr…`
- Row 016 `settlement_16_the_radioactive_graveyard_of_cranes`: `{"colloquial_name":"The Iron Graveyard","controlling_faction":"faction_none_ghost_zone","defense_fortifications":"Lethal 1.8 R/h gamma radiation fields, collapsing crane booms, and packs of two-headed steppe wolves.","estimated_population"…`
- Row 017 `settlement_17_the_telegraph_relay_station_eight`: `{"colloquial_name":"The Wire Shack","controlling_faction":"faction_the_wire_tappers","defense_fortifications":"Log blockhouse with concrete firing slit foundation, razor wire perimeter, and tripwire flares.","estimated_population":12,"geog…`
- Row 018 `settlement_18_the_collapsed_brewery_cellars`: `{"colloquial_name":"The Yeast Cellars","controlling_faction":"faction_the_brewers_guild","defense_fortifications":"Vaulted brick beer cellars 10 meters underground, oak barrel barricades, and pressure-fed boiling water hoses.","estimated_p…`
- Row 019 `settlement_19_the_border_bridge_checkpoint`: `{"colloquial_name":"The Iron Span","controlling_faction":"faction_joint_treaty_guard","defense_fortifications":"Concrete bridgehead pillboxes with 14.5mm KPV heavy machine guns, demolition charges wired to center span piers.","estimated_po…`
- Row 020 `settlement_20_the_valley_sunken_atrium_republic`: `{"colloquial_name":"The Green Basin","controlling_faction":"faction_the_free_republic","defense_fortifications":"Terraced granite retaining walls, solar skylight atriums, automated pneumatic blast gates, and the Citizen Militia Brigade.","…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/wasteland_settlement_npcs.json`

### `Assets/StreamingAssets/Data/wasteland_settlement_npcs.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 30940; characters: 30940.
- SHA-256: `7bc358802a64c6baeb817f7cb44d3cdea5e1ec1cfa4a0af6665141aafdff23c1`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `collection_id`, `npcs`

#### `npcs` — 18 current rows

- Row 001 `npc_salt_marshal_varn`: `{"contradiction":"Enforces strict salt rationing, but occasionally trades unlogged brine reserves to maintain personal leverage over the quartermaster.","display_name":"Marshal Varn","faction":"none","fear":"A sudden ashfall surge choking …`
- Row 002 `npc_salt_trader_elena`: `{"contradiction":"Maintains precise measurement standards for outsiders, but regularly skims fractional weights from outgoing settlement shipments.","display_name":"Elena Kosh","faction":"none","fear":"Contaminated river sludge fouling the…`
- Row 003 `npc_salt_boiler_petyr`: `{"contradiction":"Deaf in his left ear from steam blasts, he refuses to train replacements, securing his position through specialized, unshared knowledge.","display_name":"Petyr the Boiler","faction":"none","fear":"A catastrophic boiler fl…`
- Row 004 `npc_switch_master_korov`: `{"contradiction":"Meticulously logs every scrap of incoming wire, yet fails to report structural degradation on the primary turntable.","display_name":"Master Korov","faction":"faction_railway_guild","fear":"A coordinated rail assault by r…`
- Row 005 `npc_rail_chandler_bess`: `{"contradiction":"Enforces a zero-credit policy on all salvage sales, yet frequently accepts heavily degraded barter items if they contain trace copper.","display_name":"Bess the Chandler","faction":"faction_railway_guild","fear":"Running …`
- Row 006 `npc_rivet_smith_milos`: `{"contradiction":"Requires massive caloric intake to maintain forge labor, forcing a significant drain on the settlement's overall winter reserves.","display_name":"Milos the Riveter","faction":"none","fear":"Losing his remaining eyesight …`
- Row 007 `npc_beacon_keeper_maren`: `{"contradiction":"Maintains the beacon meticulously, not for navigational aid, but to ensure the settlement remains visible to specific black-market supply drops.","display_name":"Lightkeeper Maren","faction":"none","fear":"A sudden night …`
- Row 008 `npc_coastal_chandler_orlov`: `{"contradiction":"Complains continuously about the freezing surf, yet refuses assignment to warmer indoor duties due to the unmonitored salvage opportunities in the water.","display_name":"Orlov the Diver","faction":"none","fear":"Getting …`
- Row 009 `npc_net_mender_kira`: `{"contradiction":"Demonstrates high efficiency in repairing nets, but frequently repurposes the strongest nylon strands for personal structural reinforcement.","display_name":"Kira the Weaver","faction":"none","fear":"A severe toxic red-ti…`
- Row 010 `npc_quarry_steward_darek`: `{"contradiction":"Advocates for safer extraction limits in public, while privately authorizing hazardous deep-seam blasting to meet monthly quotas.","display_name":"Steward Darek","faction":"none","fear":"A massive seismic tremor collapsin…`
- Row 011 `npc_stone_cutter_valya`: `{"contradiction":"Complains about the wear on the channel saws, yet continues to under-report the actual volume of stone cut to avoid central taxation.","display_name":"Valya the Factor","faction":"none","fear":"Water seepage freezing insi…`
- Row 012 `npc_driller_jarek`: `{"contradiction":"Handles high explosives daily, but consistently fails to log misfires, resulting in unexploded ordnance scattered throughout the extraction zones.","display_name":"Jarek the Blaster","faction":"none","fear":"A damp fuse m…`
- Row 013 `npc_prior_silas`: `{"contradiction":"Preaches non-violence and asceticism, while functionally controlling the settlement's largest reserve of sanitized medical supplies.","display_name":"Prior Silas","faction":"none","fear":"Violent faction skirmishes desecr…`
- Row 014 `npc_almoner_hanna`: `{"contradiction":"Strict about not wasting medicinal tinctures, she prioritizes treatment for units capable of heavy labor over dependents.","display_name":"Almoner Hanna","faction":"none","fear":"An outbreak of infectious lung rot overwhe…`
- Row 015 `npc_wayfarer_tobias`: `{"contradiction":"Takes solemn vows of poverty, but maintains exclusive access to a cache of pre-war combat knives, claiming them as 'relics'.","display_name":"Tobias the Pilgrim","faction":"none","fear":"Getting caught on the open pass du…`
- Row 016 `npc_market_warden_grimm`: `{"contradiction":"A stern enforcer of market regulations who actively ignores contraband sales if they generate sufficient gate tax.","display_name":"Warden Grimm","faction":"none","fear":"An internal gang war erupting inside the crowded c…`
- Row 017 `npc_junk_broker_solomon`: `{"contradiction":"Haggles aggressively for pre-war electronics, not to sell them, but to strip them for rare earth metals to trade with the Garrison.","display_name":"Solomon the Broker","faction":"none","fear":"An electromagnetic pulse st…`
- Row 018 `npc_grease_monkey_tess`: `{"contradiction":"Patiently rewinds burnt electric motors by hand, but intentionally installs minor flaws to guarantee future maintenance contracts.","display_name":"Tess the Grease-Monkey","faction":"none","fear":"A sudden mechanical seiz…`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/World/SettlementCatalog.cs`

### `Assets/Ashfall.Core/World/SettlementCatalog.cs` — complete current file

- Size: 463 lines / 17110 bytes.
- SHA-256: `492fea4b385a078e5e94ed407fe2d74a9876a8143a92b87144db32bcb0d9d29e`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Text.Json;
00006: using System.Text.Json.Serialization;
00007:
00008: namespace Ashfall.Core.World
00009: {
00010:     [Serializable]
00011:     public sealed class SettlementEconomy
00012:     {
00013:         [JsonPropertyName("primary_export")]
00014:         public string PrimaryExport { get; set; } = string.Empty;
00015:
00016:         [JsonPropertyName("primary_import")]
00017:         public string PrimaryImport { get; set; } = string.Empty;
00018:
00019:         [JsonPropertyName("trade_specialty")]
00020:         public string TradeSpecialty { get; set; } = string.Empty;
00021:
00022:         [JsonPropertyName("price_modifier_exports")]
00023:         public float PriceModifierExports { get; set; } = 1.0f;
00024:
00025:         [JsonPropertyName("price_modifier_imports")]
00026:         public float PriceModifierImports { get; set; } = 1.0f;
00027:
00028:         [JsonPropertyName("stock_item_ids")]
00029:         public List<string> StockItemIds { get; set; } = new List<string>();
00030:     }
00031:
00032:     [Serializable]
00033:     public sealed class SettlementSociety
00034:     {
00035:         [JsonPropertyName("governance")]
00036:         public string Governance { get; set; } = string.Empty;
00037:
00038:         [JsonPropertyName("population")]
00039:         public int Population { get; set; } = 50;
00040:
00041:         [JsonPropertyName("core_value")]
00042:         public string CoreValue { get; set; } = string.Empty;
00043:
00044:         [JsonPropertyName("internal_tension")]
00045:         public string InternalTension { get; set; } = string.Empty;
00046:     }
00047:
00048:     [Serializable]
00049:     public sealed class SettlementFactionRelation
00050:     {
00051:         [JsonPropertyName("primary_faction")]
00052:         public string PrimaryFaction { get; set; } = string.Empty;
00053:
00054:         [JsonPropertyName("standing_gate_faction")]
00055:         public string StandingGateFaction { get; set; } = string.Empty;
00056:
00057:         [JsonPropertyName("min_standing_to_enter")]
00058:         public int MinStandingToEnter { get; set; } = 0;
00059:
00060:         [JsonPropertyName("hostile_standing_threshold")]
00061:         public int HostileStandingThreshold { get; set; } = -40;
00062:     }
00063:
00064:     [Serializable]
00065:     public sealed class SettlementDefinition
00066:     {
00067:         [JsonPropertyName("id")]
00068:         public string Id { get; set; } = string.Empty;
00069:
00070:         [JsonPropertyName("display_name")]
00071:         public string DisplayName { get; set; } = string.Empty;
00072:
00073:         [JsonPropertyName("archetype")]
00074:         public string Archetype { get; set; } = string.Empty;
00075:
00076:         [JsonPropertyName("region")]
00077:         public string Region { get; set; } = string.Empty;
00078:
00079:         [JsonPropertyName("location_id")]
00080:         public string LocationId { get; set; } = string.Empty;
00081:
00082:         [JsonPropertyName("route_node")]
00083:         public string RouteNode { get; set; } = string.Empty;
00084:
00085:         [JsonPropertyName("description")]
00086:         public string Description { get; set; } = string.Empty;
00087:
00088:         [JsonPropertyName("survival_adaptation")]
00089:         public string SurvivalAdaptation { get; set; } = string.Empty;
00090:
00091:         [JsonPropertyName("economy")]
00092:         public SettlementEconomy Economy { get; set; } = new SettlementEconomy();
00093:
00094:         [JsonPropertyName("society")]
00095:         public SettlementSociety Society { get; set; } = new SettlementSociety();
00096:
00097:         [JsonPropertyName("faction_relation")]
00098:         public SettlementFactionRelation FactionRelation { get; set; } = new SettlementFactionRelation();
00099:
00100:         [JsonPropertyName("location_link")]
00101:         public string LocationLink { get; set; } = string.Empty;
00102:
00103:         [JsonPropertyName("population")]
00104:         public int Population { get; set; } = 0;
00105:
00106:         [JsonPropertyName("allegiance")]
00107:         public string Allegiance { get; set; } = string.Empty;
00108:
00109:         [JsonPropertyName("threat_level")]
00110:         public int ThreatLevel { get; set; } = 2;
00111:
00112:         [JsonPropertyName("attitude")]
00113:         public string Attitude { get; set; } = "neutral";
00114:
00115:         [JsonPropertyName("trade_goods")]
00116:         public List<string> TradeGoods { get; set; } = new List<string>();
00117:
00118:         [JsonPropertyName("trade_needs")]
00119:         public List<string> TradeNeeds { get; set; } = new List<string>();
00120:
00121:         [JsonPropertyName("keeper_npc_id")]
00122:         public string KeeperNpcId { get; set; } = string.Empty;
00123:
00124:         [JsonPropertyName("trader_npc_id")]
00125:         public string TraderNpcId { get; set; } = string.Empty;
00126:
00127:         [JsonPropertyName("fixture_npc_id")]
00128:         public string FixtureNpcId { get; set; } = string.Empty;
00129:
00130:         [JsonPropertyName("sidework_quest_id")]
00131:         public string SideworkQuestId { get; set; } = string.Empty;
00132:
00133:         public string GetEffectiveLocationId() => !string.IsNullOrEmpty(LocationLink) ? LocationLink : LocationId;
00134:         public int GetEffectivePopulation() => Population > 0 ? Population : (Society?.Population ?? 50);
00135:         public string GetEffectiveAllegiance() => !string.IsNullOrEmpty(Allegiance) ? Allegiance : (FactionRelation?.PrimaryFaction ?? "none");
00136:     }
00137:
00138:     [Serializable]
00139:     public sealed class SettlementNpcGreeting
00140:     {
00141:         [JsonPropertyName("low_standing")]
00142:         public string LowStanding { get; set; } = string.Empty;
00143:
00144:         [JsonPropertyName("neutral")]
00145:         public string Neutral { get; set; } = string.Empty;
00146:
00147:         [JsonPropertyName("high_standing")]
00148:         public string HighStanding { get; set; } = string.Empty;
00149:     }
00150:
00151:     [Serializable]
00152:     public sealed class SettlementNpcEntry
00153:     {
00154:         [JsonPropertyName("id")]
00155:         public string Id { get; set; } = string.Empty;
00156:
00157:         [JsonPropertyName("display_name")]
00158:         public string DisplayName { get; set; } = string.Empty;
00159:
00160:         [JsonPropertyName("settlement_id")]
00161:         public string SettlementId { get; set; } = string.Empty;
00162:
00163:         [JsonPropertyName("role")]
00164:         public string Role { get; set; } = string.Empty; // "Keeper", "Trader", "Fixture"
00165:
00166:         [JsonPropertyName("profession")]
00167:         public string Profession { get; set; } = string.Empty;
00168:
00169:         [JsonPropertyName("faction")]
00170:         public string Faction { get; set; } = "none";
00171:
00172:         [JsonPropertyName("trade_specialty")]
00173:         public string TradeSpecialty { get; set; } = string.Empty;
00174:
00175:         [JsonPropertyName("physical_anchor")]
00176:         public string PhysicalAnchor { get; set; } = string.Empty;
00177:
00178:         [JsonPropertyName("value")]
00179:         public string Value { get; set; } = string.Empty;
00180:
00181:         [JsonPropertyName("fear")]
00182:         public string Fear { get; set; } = string.Empty;
00183:
00184:         [JsonPropertyName("contradiction")]
00185:         public string Contradiction { get; set; } = string.Empty;
00186:
00187:         [JsonPropertyName("personal_thread")]
00188:         public string PersonalThread { get; set; } = string.Empty;
00189:
00190:         [JsonPropertyName("greetings")]
00191:         public SettlementNpcGreeting Greetings { get; set; } = new SettlementNpcGreeting();
00192:
00193:         [JsonPropertyName("trade_tells")]
00194:         public List<string> TradeTells { get; set; } = new List<string>();
00195:
00196:         [JsonPropertyName("sidework_quest_id")]
00197:         public string SideworkQuestId { get; set; } = string.Empty;
00198:
00199:         [JsonPropertyName("portrait_id")]
00200:         public string PortraitId { get; set; } = string.Empty;
00201:     }
00202:
00203:     [Serializable]
00204:     public sealed class RepeatableQuestStage
00205:     {
00206:         [JsonPropertyName("id")]
00207:         public string Id { get; set; } = string.Empty;
00208:
00209:         [JsonPropertyName("text")]
00210:         public string Text { get; set; } = string.Empty;
00211:     }
00212:
00213:     [Serializable]
00214:     public sealed class RepeatableQuestEntry
00215:     {
00216:         [JsonPropertyName("id")]
00217:         public string Id { get; set; } = string.Empty;
00218:
00219:         [JsonPropertyName("display_name")]
00220:         public string DisplayName { get; set; } = string.Empty;
00221:
00222:         [JsonPropertyName("provider_npc_id")]
00223:         public string ProviderNpcId { get; set; } = string.Empty;
00224:
00225:         [JsonPropertyName("settlement_id")]
00226:         public string SettlementId { get; set; } = string.Empty;
00227:
00228:         [JsonPropertyName("type")]
00229:         public string Type { get; set; } = string.Empty;
00230:
00231:         [JsonPropertyName("briefing")]
00232:         public string Briefing { get; set; } = string.Empty;
00233:
00234:         [JsonPropertyName("prereq_quest_id")]
00235:         public string PrereqQuestId { get; set; } = string.Empty;
00236:
00237:         [JsonPropertyName("target_location_id")]
00238:         public string TargetLocationId { get; set; } = string.Empty;
00239:
00240:         [JsonPropertyName("cooldown_days")]
00241:         public int CooldownDays { get; set; } = 7;
00242:
00243:         [JsonPropertyName("reward_item_id")]
00244:         public string RewardItemId { get; set; } = string.Empty;
00245:
00246:         [JsonPropertyName("reward_count")]
00247:         public int RewardCount { get; set; } = 1;
00248:
00249:         [JsonPropertyName("standing_delta")]
00250:         public int StandingDelta { get; set; } = 5;
00251:
00252:         [JsonPropertyName("stages")]
00253:         public List<RepeatableQuestStage> Stages { get; set; } = new List<RepeatableQuestStage>();
00254:     }
00255:
00256:     [Serializable]
00257:     public sealed class SettlementState
00258:     {
00259:         [JsonPropertyName("cooldowns")]
00260:         public Dictionary<string, int> QuestAvailableDay { get; set; } = new Dictionary<string, int>();
00261:
00262:         [JsonPropertyName("completed_quest_counts")]
00263:         public Dictionary<string, int> CompletedQuestCounts { get; set; } = new Dictionary<string, int>();
00264:     }
00265:
00266:     public sealed class SettlementCatalog
00267:     {
00268:         private readonly Dictionary<string, SettlementDefinition> _settlementsById = new(StringComparer.OrdinalIgnoreCase);
00269:         private readonly Dictionary<string, SettlementNpcEntry> _npcsById = new(StringComparer.OrdinalIgnoreCase);
00270:         private readonly Dictionary<string, RepeatableQuestEntry> _questsById = new(StringComparer.OrdinalIgnoreCase);
00271:         private readonly List<SettlementDefinition> _allSettlements = new();
00272:         private readonly List<SettlementNpcEntry> _allNpcs = new();
00273:         private readonly List<RepeatableQuestEntry> _allQuests = new();
00274:
00275:         private readonly Dictionary<string, int> _questAvailableDay = new(StringComparer.OrdinalIgnoreCase);
00276:         private readonly Dictionary<string, int> _completedQuestCounts = new(StringComparer.OrdinalIgnoreCase);
00277:
00278:         public int SettlementCount => _allSettlements.Count;
00279:         public int NpcCount => _allNpcs.Count;
00280:         public int QuestCount => _allQuests.Count;
00281:
00282:         public IReadOnlyList<SettlementDefinition> Settlements => _allSettlements;
00283:         public IReadOnlyList<SettlementNpcEntry> Npcs => _allNpcs;
00284:         public IReadOnlyList<RepeatableQuestEntry> Quests => _allQuests;
00285:
00286:         public static SettlementCatalog LoadFromDirectory(string directoryPath, IFileIO fileIO)
00287:         {
00288:             var catalog = new SettlementCatalog();
00289:             if (string.IsNullOrEmpty(directoryPath) || fileIO == null) return catalog;
00290:
00291:             string settlementsPath = Path.Combine(directoryPath, "settlements.json");
00292:             if (fileIO.FileExists(settlementsPath))
00293:             {
00294:                 catalog.LoadSettlementsJson(fileIO.ReadAllText(settlementsPath));
00295:             }
00296:
00297:             string npcsPath = Path.Combine(directoryPath, "wasteland_settlement_npcs.json");
00298:             if (fileIO.FileExists(npcsPath))
00299:             {
00300:                 catalog.LoadNpcsJson(fileIO.ReadAllText(npcsPath));
00301:             }
00302:
00303:             string questsPath = Path.Combine(directoryPath, "repeatable_quests.json");
00304:             if (fileIO.FileExists(questsPath))
00305:             {
00306:                 catalog.LoadQuestsJson(fileIO.ReadAllText(questsPath));
00307:             }
00308:
00309:             return catalog;
00310:         }
00311:
00312:         public void LoadSettlementsJson(string json)
00313:         {
00314:             if (string.IsNullOrWhiteSpace(json)) return;
00315:             using var doc = JsonDocument.Parse(json);
00316:             if (doc.RootElement.TryGetProperty("settlements", out var settlementsElem) && settlementsElem.ValueKind == JsonValueKind.Array)
00317:             {
00318:                 foreach (var item in settlementsElem.EnumerateArray())
00319:                 {
00320:                     var settlement = JsonSerializer.Deserialize<SettlementDefinition>(item.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
00321:                     if (settlement != null && !string.IsNullOrEmpty(settlement.Id))
00322:                     {
00323:                         _settlementsById[settlement.Id] = settlement;
00324:                         _allSettlements.Add(settlement);
00325:                     }
00326:                 }
00327:             }
00328:         }
00329:
00330:         public void LoadNpcsJson(string json)
00331:         {
00332:             if (string.IsNullOrWhiteSpace(json)) return;
00333:             using var doc = JsonDocument.Parse(json);
00334:             if (doc.RootElement.TryGetProperty("npcs", out var npcsElem) && npcsElem.ValueKind == JsonValueKind.Array)
00335:             {
00336:                 foreach (var item in npcsElem.EnumerateArray())
00337:                 {
00338:                     var npc = JsonSerializer.Deserialize<SettlementNpcEntry>(item.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
00339:                     if (npc != null && !string.IsNullOrEmpty(npc.Id))
00340:                     {
00341:                         _npcsById[npc.Id] = npc;
00342:                         _allNpcs.Add(npc);
00343:                     }
00344:                 }
00345:             }
00346:         }
00347:
00348:         public void LoadQuestsJson(string json)
00349:         {
00350:             if (string.IsNullOrWhiteSpace(json)) return;
00351:             using var doc = JsonDocument.Parse(json);
00352:             if (doc.RootElement.TryGetProperty("quests", out var questsElem) && questsElem.ValueKind == JsonValueKind.Array)
00353:             {
00354:                 foreach (var item in questsElem.EnumerateArray())
00355:                 {
00356:                     var quest = JsonSerializer.Deserialize<RepeatableQuestEntry>(item.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
00357:                     if (quest != null && !string.IsNullOrEmpty(quest.Id))
00358:                     {
00359:                         _questsById[quest.Id] = quest;
00360:                         _allQuests.Add(quest);
00361:                     }
00362:                 }
00363:             }
00364:         }
00365:
00366:         public bool TryGetSettlement(string id, out SettlementDefinition settlement)
00367:         {
00368:             if (string.IsNullOrEmpty(id))
00369:             {
00370:                 settlement = null!;
00371:                 return false;
00372:             }
00373:             return _settlementsById.TryGetValue(id, out settlement!);
00374:         }
00375:
00376:         public bool TryGetNpc(string id, out SettlementNpcEntry npc)
00377:         {
00378:             if (string.IsNullOrEmpty(id))
00379:             {
00380:                 npc = null!;
00381:                 return false;
00382:             }
00383:             return _npcsById.TryGetValue(id, out npc!);
00384:         }
00385:
00386:         public bool TryGetQuest(string id, out RepeatableQuestEntry quest)
00387:         {
00388:             if (string.IsNullOrEmpty(id))
00389:             {
00390:                 quest = null!;
00391:                 return false;
00392:             }
00393:             return _questsById.TryGetValue(id, out quest!);
00394:         }
00395:
00396:         public string GetNpcGreeting(string npcId, float standing)
00397:         {
00398:             if (!TryGetNpc(npcId, out var npc)) return string.Empty;
00399:             if (standing <= -15f) return npc.Greetings.LowStanding;
00400:             if (standing >= 25f) return npc.Greetings.HighStanding;
00401:             return npc.Greetings.Neutral;
00402:         }
00403:
00404:         public bool IsQuestAvailable(string questId, int currentDay)
00405:         {
00406:             if (!_questsById.ContainsKey(questId)) return false;
00407:             if (_questAvailableDay.TryGetValue(questId, out int nextAvailable))
00408:             {
00409:                 return currentDay >= nextAvailable;
00410:             }
00411:             return true;
00412:         }
00413:
00414:         public void CompleteQuest(string questId, int currentDay)
00415:         {
00416:             if (!TryGetQuest(questId, out var quest)) return;
00417:
00418:             _questAvailableDay[questId] = currentDay + Math.Max(1, quest.CooldownDays);
00419:             if (!_completedQuestCounts.ContainsKey(questId))
00420:             {
00421:                 _completedQuestCounts[questId] = 0;
00422:             }
00423:             _completedQuestCounts[questId]++;
00424:         }
00425:
00426:         public int GetCompletedQuestCount(string questId)
00427:         {
00428:             return _completedQuestCounts.TryGetValue(questId, out int count) ? count : 0;
00429:         }
00430:
00431:         public SettlementState CaptureState()
00432:         {
00433:             return new SettlementState
00434:             {
00435:                 QuestAvailableDay = new Dictionary<string, int>(_questAvailableDay, StringComparer.OrdinalIgnoreCase),
00436:                 CompletedQuestCounts = new Dictionary<string, int>(_completedQuestCounts, StringComparer.OrdinalIgnoreCase)
00437:             };
00438:         }
00439:
00440:         public void RestoreState(SettlementState? state)
00441:         {
00442:             _questAvailableDay.Clear();
00443:             _completedQuestCounts.Clear();
00444:             if (state == null) return;
00445:
00446:             if (state.QuestAvailableDay != null)
00447:             {
00448:                 foreach (var kvp in state.QuestAvailableDay)
00449:                 {
00450:                     _questAvailableDay[kvp.Key] = kvp.Value;
00451:                 }
00452:             }
00453:
00454:             if (state.CompletedQuestCounts != null)
00455:             {
00456:                 foreach (var kvp in state.CompletedQuestCounts)
00457:                 {
00458:                     _completedQuestCounts[kvp.Key] = kvp.Value;
00459:                 }
00460:             }
00461:         }
00462:     }
00463: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs`

### `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs` — complete current file

- Size: 158 lines / 7138 bytes.
- SHA-256: `d3a4f1ba72662b18aa76830b5845a39dd7c25969398117089b6b0246c728a099`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.IO;
00004: using System.Linq;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Factions;
00007: using Ashfall.Core.World;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests.World
00011: {
00012:     public sealed class Plan43_44SettlementTerritoryIntegrationTests
00013:     {
00014:         private static string ResolveDataDir()
00015:         {
00016:             string baseDir = AppContext.BaseDirectory;
00017:             string probe = Path.Combine(baseDir, "StreamingAssets", "Data");
00018:             if (Directory.Exists(probe)) return probe;
00019:
00020:             probe = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
00021:             if (Directory.Exists(probe)) return Path.GetFullPath(probe);
00022:
00023:             probe = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
00024:             if (Directory.Exists(probe)) return Path.GetFullPath(probe);
00025:
00026:             return Path.Combine(baseDir, "Assets", "StreamingAssets", "Data");
00027:         }
00028:
00029:         [Fact]
00030:         public void SettlementAndTerritoryCatalogs_CrossValidateAllegiancesAndControlPoints()
00031:         {
00032:             string dataDir = ResolveDataDir();
00033:             var fileIO = new FileSystemIO();
00034:
00035:             var settlementCatalog = SettlementCatalog.LoadFromDirectory(dataDir, fileIO);
00036:             var territoryCatalog = FactionTerritoryCatalog.LoadFromDirectory(dataDir, fileIO);
00037:
00038:             Assert.NotNull(settlementCatalog);
00039:             Assert.NotNull(territoryCatalog);
00040:             Assert.Equal(12, settlementCatalog.SettlementCount);
00041:             Assert.Equal(19, territoryCatalog.TerritoryCount);
00042:             Assert.Equal(5, territoryCatalog.ContestedZoneCount);
00043:
00044:             // Verify every settlement maps directly to a territory control point owned by its allegiant faction
00045:             foreach (var settlement in settlementCatalog.Settlements)
00046:             {
00047:                 string locId = settlement.GetEffectiveLocationId();
00048:                 string allegianceFaction = settlement.GetEffectiveAllegiance();
00049:
00050:                 Assert.False(string.IsNullOrWhiteSpace(locId), $"Settlement {settlement.Id} must have an effective location ID");
00051:                 Assert.False(string.IsNullOrWhiteSpace(allegianceFaction), $"Settlement {settlement.Id} must have an effective allegiance");
00052:
00053:                 bool territoryFound = territoryCatalog.TryGetTerritoryByFaction(allegianceFaction, out var territory);
00054:                 Assert.True(territoryFound, $"Allegiant faction '{allegianceFaction}' for settlement '{settlement.Id}' must have a territory definition");
00055:
00056:                 // The settlement location must be registered in the territory's control points
00057:                 Assert.Contains(locId, territory.control_points);
00058:
00059:                 // Trade tax and travel safety should be within valid bounds
00060:                 Assert.InRange(territory.trade_tax, 0.0f, 1.0f);
00061:                 Assert.InRange(territory.travel_safety, 0.0f, 1.0f);
00062:             }
00063:         }
00064:
00065:         [Fact]
00066:         public void TerritoryControlSystem_SimulatesSettlementContestAndFortification()
00067:         {
00068:             string dataDir = ResolveDataDir();
00069:             string territoryJson = File.ReadAllText(Path.Combine(dataDir, "faction_territory.json"));
00070:             string supplyLineJson = File.ReadAllText(Path.Combine(dataDir, "supply_lines.json"));
00071:
00072:             var territorySystem = TerritoryControlSystem.FromJson(territoryJson, supplyLineJson);
00073:             Assert.NotNull(territorySystem);
00074:
00075:             // Nine Rails settlement location check
00076:             string settlementLoc = "loc_settlement_nine_rails";
00077:             var locState = territorySystem.GetLocationState(settlementLoc);
00078:             Assert.NotNull(locState);
00079:             Assert.Equal("faction_the_office", locState!.ControllingFactionId);
00080:
00081:             // Fortify Nine Rails
00082:             string? fortifiedLoc = null;
00083:             int fortifiedLevel = -1;
00084:             territorySystem.OnLocationFortifiedSeam = (loc, lvl) =>
00085:             {
00086:                 fortifiedLoc = loc;
00087:                 fortifiedLevel = lvl;
00088:             };
00089:
00090:             bool fortSuccess = territorySystem.FortifyLocation(settlementLoc, 1);
00091:             Assert.True(fortSuccess);
00092:             Assert.Equal(settlementLoc, fortifiedLoc);
00093:             Assert.Equal(1, fortifiedLevel);
00094:             Assert.Equal(1, locState.FortificationLevel);
00095:
00096:             // Contest Nine Rails with overwhelming power
00097:             string? contestedLoc = null;
00098:             string? capturedFrom = null;
00099:             string? capturedBy = null;
00100:             territorySystem.OnTerritoryControlChangedSeam = (loc, oldF, newF) =>
00101:             {
00102:                 contestedLoc = loc;
00103:                 capturedFrom = oldF;
00104:                 capturedBy = newF;
00105:             };
00106:
00107:             var rng = new SeededRng(1337);
00108:             bool shifted = territorySystem.ContestLocation(
00109:                 settlementLoc,
00110:                 attackingFactionId: "faction_iron_raiders",
00111:                 attackPower: 500, // overwhelming force
00112:                 rng: rng,
00113:                 currentDay: 5);
00114:
00115:             Assert.True(shifted);
00116:             Assert.Equal(settlementLoc, contestedLoc);
00117:             Assert.Equal("faction_the_office", capturedFrom);
00118:             Assert.Equal("faction_iron_raiders", capturedBy);
00119:             Assert.Equal("faction_iron_raiders", locState.ControllingFactionId);
00120:         }
00121:
00122:         [Fact]
00123:         public void SettlementCatalog_QuestLifecycle_AndStateSaveRestoreRoundTrip()
00124:         {
00125:             string dataDir = ResolveDataDir();
00126:             var fileIO = new FileSystemIO();
00127:
00128:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, fileIO);
00129:             Assert.NotNull(catalog);
00130:
00131:             // Check all quests are available on day 1
00132:             if (catalog.Quests.Count > 0)
00133:             {
00134:                 var sampleQuest = catalog.Quests.First();
00135:                 Assert.True(catalog.IsQuestAvailable(sampleQuest.Id, currentDay: 1));
00136:
00137:                 // Complete quest on day 1
00138:                 catalog.CompleteQuest(sampleQuest.Id, currentDay: 1);
00139:                 Assert.Equal(1, catalog.GetCompletedQuestCount(sampleQuest.Id));
00140:                 Assert.False(catalog.IsQuestAvailable(sampleQuest.Id, currentDay: 1));
00141:                 Assert.True(catalog.IsQuestAvailable(sampleQuest.Id, currentDay: 1 + sampleQuest.CooldownDays + 1));
00142:
00143:                 // Round-trip state capture & restore
00144:                 var savedState = catalog.CaptureState();
00145:                 Assert.NotNull(savedState);
00146:                 Assert.True(savedState.CompletedQuestCounts.ContainsKey(sampleQuest.Id));
00147:                 Assert.Equal(1, savedState.CompletedQuestCounts[sampleQuest.Id]);
00148:
00149:                 var restoredCatalog = SettlementCatalog.LoadFromDirectory(dataDir, fileIO);
00150:                 restoredCatalog.RestoreState(savedState);
00151:
00152:                 Assert.Equal(1, restoredCatalog.GetCompletedQuestCount(sampleQuest.Id));
00153:                 Assert.False(restoredCatalog.IsQuestAvailable(sampleQuest.Id, currentDay: 1));
00154:                 Assert.True(restoredCatalog.IsQuestAvailable(sampleQuest.Id, currentDay: 1 + sampleQuest.CooldownDays + 1));
00155:             }
00156:         }
00157:     }
00158: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs`

### `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs` — complete current file

- Size: 496 lines / 21443 bytes.
- SHA-256: `35df3dad7070ede8b234fe0745daf4a4f0ee5d5d2751aebf7635b9a100105213`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: #nullable enable
00002: // SPDX-License-Identifier: MIT
00003: using System;
00004: using System.Collections.Generic;
00005: using System.Linq;
00006: using System.Text.Json;
00007: using Ashfall.Core.Inventory;
00008:
00009: namespace Ashfall.Core.Settlements
00010: {
00011:     /// <summary>
00012:     /// Definition of an authored outpost or secondary holdfast position.
00013:     /// Loaded from StreamingAssets/Data/outposts.json.
00014:     /// </summary>
00015:     public sealed class OutpostDef
00016:     {
00017:         public string Id { get; set; } = string.Empty;
00018:         public string Name { get; set; } = string.Empty;
00019:         public string GraphNodeId { get; set; } = string.Empty;
00020:         public int MaxGarrisonBunks { get; set; } = 4;
00021:         public int DailySupplyDemand { get; set; } = 4;
00022:         public int DefenseRating { get; set; } = 50;
00023:         public int RadioRelayRange { get; set; } = 10;
00024:         public Dictionary<string, int> BuildCost { get; set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
00025:     }
00026:
00027:     /// <summary>
00028:     /// Runtime instance state of an outpost.
00029:     /// Preserves single authority: does not duplicate population or food stores.
00030:     /// </summary>
00031:     public sealed class OutpostInstance
00032:     {
00033:         public string OutpostId { get; set; } = string.Empty;
00034:         public string GraphNodeId { get; set; } = string.Empty;
00035:         public bool IsEstablished { get; set; }
00036:         public int ConditionPermille { get; set; } = 1000;
00037:         public List<string> GarrisonSurvivorIds { get; set; } = new List<string>();
00038:         public int DaysSinceSupply { get; set; }
00039:         public bool IsStarving { get; set; }
00040:         public bool IsOverrun { get; set; }
00041:         public int RationReserve { get; set; }
00042:     }
00043:
00044:     /// <summary>
00045:     /// Persisted projection of one outpost instance. Definitions are never
00046:     /// duplicated here: they are re-derived from the authored catalog on
00047:     /// restore, so a definition edit can never be frozen into a save.
00048:     /// </summary>
00049:     public sealed class OutpostInstanceState
00050:     {
00051:         public string outpost_id { get; set; } = string.Empty;
00052:         public bool is_established { get; set; }
00053:         public int condition_permille { get; set; } = 1000;
00054:         public List<string> garrison_survivor_ids { get; set; } = new List<string>();
00055:         public int days_since_supply { get; set; }
00056:         public bool is_starving { get; set; }
00057:         public bool is_overrun { get; set; }
00058:         public int ration_reserve { get; set; }
00059:     }
00060:
00061:     /// <summary>
00062:     /// Plan 58 capture state for the outpost settlement section.
00063:     /// </summary>
00064:     public sealed class OutpostSettlementState
00065:     {
00066:         public int schema_version { get; set; } = 1;
00067:         public List<OutpostInstanceState> outposts { get; set; } = new List<OutpostInstanceState>();
00068:     }
00069:
00070:     /// <summary>
00071:     /// Pure domain engine for Plan 58: The Continuation — Outposts, Waystations, and a Second Holdfast.
00072:     /// Provides lifecycle (establish, staff, supply, risk, overrun, abandon) without duplicating core authorities.
00073:     /// Zero engine references (Godot/UnityEngine free).
00074:     /// </summary>
00075:     public sealed class OutpostSettlementSystem
00076:     {
00077:         public const string SystemId = "outpost_settlement_system";
00078:
00079:         private readonly Dictionary<string, OutpostDef> _definitions = new Dictionary<string, OutpostDef>(StringComparer.OrdinalIgnoreCase);
00080:         private readonly Dictionary<string, OutpostInstance> _instances = new Dictionary<string, OutpostInstance>(StringComparer.OrdinalIgnoreCase);
00081:
00082:         // Seam delegates
00083:         public Action<string, string>? OnOutpostEstablishedSeam { get; set; }
00084:         public Action<string, int>? OnOutpostSuppliedSeam { get; set; }
00085:         public Action<string>? OnOutpostOverrunSeam { get; set; }
00086:         public Action<string, string>? OnGarrisonAssignedSeam { get; set; }
00087:         public Action<string, string>? OnGarrisonRelievedSeam { get; set; }
00088:         public Action<string>? OnOutpostStarvingSeam { get; set; }
00089:
00090:         public OutpostSettlementSystem(IEnumerable<OutpostDef>? definitions = null)
00091:         {
00092:             if (definitions != null)
00093:             {
00094:                 foreach (var def in definitions)
00095:                 {
00096:                     if (def != null && !string.IsNullOrEmpty(def.Id))
00097:                     {
00098:                         _definitions[def.Id] = def;
00099:                         _instances[def.Id] = new OutpostInstance
00100:                         {
00101:                             OutpostId = def.Id,
00102:                             GraphNodeId = def.GraphNodeId,
00103:                             IsEstablished = false,
00104:                             ConditionPermille = 1000,
00105:                             DaysSinceSupply = 0,
00106:                             IsStarving = false,
00107:                             IsOverrun = false,
00108:                             RationReserve = 0
00109:                         };
00110:                     }
00111:                 }
00112:             }
00113:         }
00114:
00115:         /// <summary>
00116:         /// Factory parser for outposts.json catalog.
00117:         /// </summary>
00118:         public static OutpostSettlementSystem FromJson(string json)
00119:         {
00120:             if (string.IsNullOrWhiteSpace(json))
00121:                 throw new ArgumentException("Outpost JSON cannot be null or empty", nameof(json));
00122:
00123:             using var doc = JsonDocument.Parse(json);
00124:             var root = doc.RootElement;
00125:             var list = new List<OutpostDef>();
00126:
00127:             if (root.TryGetProperty("outposts", out var outpostsElem) && outpostsElem.ValueKind == JsonValueKind.Array)
00128:             {
00129:                 foreach (var item in outpostsElem.EnumerateArray())
00130:                 {
00131:                     var def = new OutpostDef
00132:                     {
00133:                         Id = item.TryGetProperty("id", out var idElem) ? idElem.GetString() ?? string.Empty : string.Empty,
00134:                         Name = item.TryGetProperty("name", out var nameElem) ? nameElem.GetString() ?? string.Empty : string.Empty,
00135:                         GraphNodeId = item.TryGetProperty("graph_node_id", out var nodeElem) ? nodeElem.GetString() ?? string.Empty : string.Empty,
00136:                         MaxGarrisonBunks = item.TryGetProperty("max_garrison_bunks", out var bunksElem) ? bunksElem.GetInt32() : 4,
00137:                         DailySupplyDemand = item.TryGetProperty("daily_supply_demand", out var demandElem) ? demandElem.GetInt32() : 4,
00138:                         DefenseRating = item.TryGetProperty("defense_rating", out var defElem) ? defElem.GetInt32() : 50,
00139:                         RadioRelayRange = item.TryGetProperty("radio_relay_range", out var radioElem) ? radioElem.GetInt32() : 10
00140:                     };
00141:
00142:                     if (item.TryGetProperty("build_cost", out var costElem) && costElem.ValueKind == JsonValueKind.Object)
00143:                     {
00144:                         foreach (var prop in costElem.EnumerateObject())
00145:                         {
00146:                             def.BuildCost[prop.Name] = prop.Value.GetInt32();
00147:                         }
00148:                     }
00149:
00150:                     list.Add(def);
00151:                 }
00152:             }
00153:
00154:             return new OutpostSettlementSystem(list);
00155:         }
00156:
00157:         public OutpostDef? GetDefinition(string outpostId)
00158:         {
00159:             if (string.IsNullOrEmpty(outpostId)) return null;
00160:             _definitions.TryGetValue(outpostId, out var def);
00161:             return def;
00162:         }
00163:
00164:         public OutpostInstance? GetInstance(string outpostId)
00165:         {
00166:             if (string.IsNullOrEmpty(outpostId)) return null;
00167:             _instances.TryGetValue(outpostId, out var inst);
00168:             return inst;
00169:         }
00170:
00171:         public IReadOnlyList<OutpostDef> GetAllDefinitions() => _definitions.Values.ToList();
00172:         public IReadOnlyList<OutpostInstance> GetAllInstances() => _instances.Values.ToList();
00173:
00174:         /// <summary>
00175:         /// Establishes an outpost. Can optionally consume resources through a caller-provided delegate.
00176:         /// </summary>
00177:         public bool EstablishOutpost(string outpostId, Func<string, int, bool>? costConsumer = null)
00178:         {
00179:             if (!_definitions.TryGetValue(outpostId, out var def)) return false;
00180:             if (!_instances.TryGetValue(outpostId, out var inst)) return false;
00181:             if (inst.IsEstablished) return false;
00182:
00183:             if (costConsumer != null)
00184:             {
00185:                 foreach (var cost in def.BuildCost)
00186:                 {
00187:                     if (!costConsumer(cost.Key, cost.Value))
00188:                         return false;
00189:                 }
00190:             }
00191:
00192:             inst.IsEstablished = true;
00193:             inst.ConditionPermille = 1000;
00194:             inst.DaysSinceSupply = 0;
00195:             inst.IsStarving = false;
00196:             inst.IsOverrun = false;
00197:             inst.RationReserve = 0;
00198:
00199:             OnOutpostEstablishedSeam?.Invoke(outpostId, def.GraphNodeId);
00200:             return true;
00201:         }
00202:
00203:         /// <summary>
00204:         /// Establishes an outpost and consumes its complete authored build bill
00205:         /// through the canonical inventory transaction boundary.
00206:         /// </summary>
00207:         public bool TryEstablishOutpost(string outpostId, IPlayerInventoryPort? inventory)
00208:         {
00209:             if (string.IsNullOrWhiteSpace(outpostId)
00210:                 || !_definitions.TryGetValue(outpostId, out var def)
00211:                 || !_instances.TryGetValue(outpostId, out var inst)
00212:                 || inst.IsEstablished
00213:                 || inventory == null)
00214:                 return false;
00215:
00216:             if (!inventory.TryConsumeBill(def.BuildCost)) return false;
00217:             inst.IsEstablished = true;
00218:             inst.ConditionPermille = 1000;
00219:             inst.DaysSinceSupply = 0;
00220:             inst.IsStarving = false;
00221:             inst.IsOverrun = false;
00222:             inst.RationReserve = 0;
00223:             OnOutpostEstablishedSeam?.Invoke(outpostId, def.GraphNodeId);
00224:             return true;
00225:         }
00226:
00227:         /// <summary>
00228:         /// Assigns a survivor from the central roster to garrison an outpost.
00229:         /// Respects bunks capacity and optional fitness gating.
00230:         /// </summary>
00231:         public bool AssignGarrison(string outpostId, string survivorId, Func<string, bool>? fitnessCheck = null)
00232:         {
00233:             if (string.IsNullOrEmpty(survivorId)) return false;
00234:             if (!_definitions.TryGetValue(outpostId, out var def)) return false;
00235:             if (!_instances.TryGetValue(outpostId, out var inst)) return false;
00236:             if (!inst.IsEstablished || inst.IsOverrun) return false;
00237:
00238:             if (inst.GarrisonSurvivorIds.Contains(survivorId)) return false;
00239:             if (inst.GarrisonSurvivorIds.Count >= def.MaxGarrisonBunks) return false;
00240:             foreach (var other in _instances.Values)
00241:                 if (other.IsEstablished && other.GarrisonSurvivorIds.Contains(survivorId))
00242:                     return false;
00243:
00244:             if (fitnessCheck != null && !fitnessCheck(survivorId))
00245:                 return false;
00246:
00247:             inst.GarrisonSurvivorIds.Add(survivorId);
00248:             OnGarrisonAssignedSeam?.Invoke(outpostId, survivorId);
00249:             return true;
00250:         }
00251:
00252:         /// <summary>
00253:         /// Relieves a survivor from an outpost garrison, returning them to central holdfast duty.
00254:         /// </summary>
00255:         public bool RelieveGarrison(string outpostId, string survivorId)
00256:         {
00257:             if (string.IsNullOrEmpty(survivorId)) return false;
00258:             if (!_instances.TryGetValue(outpostId, out var inst)) return false;
00259:
00260:             if (inst.GarrisonSurvivorIds.Remove(survivorId))
00261:             {
00262:                 OnGarrisonRelievedSeam?.Invoke(outpostId, survivorId);
00263:                 return true;
00264:             }
00265:             return false;
00266:         }
00267:
00268:         /// <summary>
00269:         /// Delivers rations to an outpost reserve from a caravan or supply route.
00270:         /// </summary>
00271:         public bool SupplyOutpost(string outpostId, int rationsDelivered)
00272:         {
00273:             if (rationsDelivered <= 0) return false;
00274:             if (!_instances.TryGetValue(outpostId, out var inst)) return false;
00275:             if (!inst.IsEstablished) return false;
00276:
00277:             inst.RationReserve += rationsDelivered;
00278:             inst.DaysSinceSupply = 0;
00279:             inst.IsStarving = false;
00280:
00281:             OnOutpostSuppliedSeam?.Invoke(outpostId, rationsDelivered);
00282:             return true;
00283:         }
00284:
00285:         /// <summary>
00286:         /// Supplies an established outpost from canonical inventory in one
00287:         /// atomic bill, updating the reserve only after the bill commits.
00288:         /// </summary>
00289:         public bool TrySupplyOutpost(
00290:             string outpostId,
00291:             string rationItemId,
00292:             int rationsDelivered,
00293:             IPlayerInventoryPort? inventory)
00294:         {
00295:             if (string.IsNullOrWhiteSpace(outpostId)
00296:                 || string.IsNullOrWhiteSpace(rationItemId)
00297:                 || rationsDelivered <= 0
00298:                 || inventory == null
00299:                 || !_instances.TryGetValue(outpostId, out var inst)
00300:                 || !inst.IsEstablished
00301:                 || inst.IsOverrun
00302:                 || inst.RationReserve > int.MaxValue - rationsDelivered)
00303:                 return false;
00304:
00305:             var bill = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
00306:             {
00307:                 [rationItemId] = rationsDelivered
00308:             };
00309:             if (!inventory.TryConsumeBill(bill)) return false;
00310:             inst.RationReserve = checked(inst.RationReserve + rationsDelivered);
00311:             inst.DaysSinceSupply = 0;
00312:             inst.IsStarving = false;
00313:             OnOutpostSuppliedSeam?.Invoke(outpostId, rationsDelivered);
00314:             return true;
00315:         }
00316:
00317:         /// <summary>
00318:         /// Advances the daily outpost lifecycle. Consumes rations and checks for starvation.
00319:         /// Central ration supplier can be passed to draw from main supply or route.
00320:         /// </summary>
00321:         public void TickDay(Func<string, int, int>? centralRationSupplyProvider = null)
00322:         {
00323:             foreach (var kvp in _instances)
00324:             {
00325:                 var inst = kvp.Value;
00326:                 if (!inst.IsEstablished || inst.IsOverrun) continue;
00327:
00328:                 if (!_definitions.TryGetValue(inst.OutpostId, out var def)) continue;
00329:
00330:                 int demand = inst.GarrisonSurvivorIds.Count > 0
00331:                     ? inst.GarrisonSurvivorIds.Count
00332:                     : Math.Max(1, def.DailySupplyDemand / 2);
00333:
00334:                 if (centralRationSupplyProvider != null)
00335:                 {
00336:                     int drawn = centralRationSupplyProvider(inst.OutpostId, demand);
00337:                     if (drawn > 0) inst.RationReserve += drawn;
00338:                 }
00339:
00340:                 if (inst.RationReserve >= demand)
00341:                 {
00342:                     inst.RationReserve -= demand;
00343:                     inst.DaysSinceSupply = 0;
00344:                     inst.IsStarving = false;
00345:                     inst.ConditionPermille = Math.Min(1000, inst.ConditionPermille + 5);
00346:                 }
00347:                 else
00348:                 {
00349:                     inst.RationReserve = 0;
00350:                     inst.DaysSinceSupply++;
00351:                     inst.IsStarving = true;
00352:                     inst.ConditionPermille = Math.Max(0, inst.ConditionPermille - 50);
00353:                     OnOutpostStarvingSeam?.Invoke(inst.OutpostId);
00354:                 }
00355:             }
00356:         }
00357:
00358:         /// <summary>
00359:         /// Simulates hostile wasteland pressure against an outpost.
00360:         /// Returns true if the outpost was overrun.
00361:         /// </summary>
00362:         public bool SimulateRisk(string outpostId, int dangerRating, ISeededRng rng)
00363:         {
00364:             if (rng == null) throw new ArgumentNullException(nameof(rng));
00365:             if (!_definitions.TryGetValue(outpostId, out var def)) return false;
00366:             if (!_instances.TryGetValue(outpostId, out var inst)) return false;
00367:             if (!inst.IsEstablished || inst.IsOverrun) return false;
00368:
00369:             int effectiveDefense = def.DefenseRating + (inst.GarrisonSurvivorIds.Count * 10) + (inst.ConditionPermille / 20);
00370:
00371:             if (dangerRating > effectiveDefense)
00372:             {
00373:                 double roll = rng.NextDouble();
00374:                 double overrunChance = Math.Min(0.9, (dangerRating - effectiveDefense) / 100.0);
00375:                 if (roll < overrunChance)
00376:                 {
00377:                     inst.IsOverrun = true;
00378:                     inst.ConditionPermille = Math.Max(0, inst.ConditionPermille - 300);
00379:                     OnOutpostOverrunSeam?.Invoke(outpostId);
00380:                     return true;
00381:                 }
00382:             }
00383:
00384:             return false;
00385:         }
00386:
00387:         /// <summary>
00388:         /// Abandons an established outpost, removing its garrison and clearing its active state.
00389:         /// </summary>
00390:         public bool AbandonOutpost(string outpostId)
00391:         {
00392:             if (!_instances.TryGetValue(outpostId, out var inst)) return false;
00393:             if (!inst.IsEstablished) return false;
00394:
00395:             inst.IsEstablished = false;
00396:             foreach (string survivorId in inst.GarrisonSurvivorIds)
00397:                 OnGarrisonRelievedSeam?.Invoke(outpostId, survivorId);
00398:             inst.GarrisonSurvivorIds.Clear();
00399:             inst.RationReserve = 0;
00400:             inst.IsStarving = false;
00401:             inst.DaysSinceSupply = 0;
00402:             return true;
00403:         }
00404:
00405:         /// <summary>
00406:         /// Plan 58 persistence — capture the live outpost state.
00407:         /// Only instance state is projected: definitions stay in the authored
00408:         /// <c>outposts.json</c> authority and are never frozen into a save.
00409:         /// Deterministic: no RNG and no wall-clock is read.
00410:         /// </summary>
00411:         public OutpostSettlementState CaptureState()
00412:         {
00413:             var state = new OutpostSettlementState { schema_version = 1 };
00414:             foreach (var def in _definitions.Values)
00415:             {
00416:                 // Definitions are emitted in authored (catalog) order so a
00417:                 // capture is byte-stable regardless of dictionary ordering.
00418:                 state.outposts.Add(new OutpostInstanceState
00419:                 {
00420:                     outpost_id = def.Id,
00421:                     is_established = false,
00422:                     condition_permille = 1000,
00423:                     garrison_survivor_ids = new List<string>(),
00424:                     days_since_supply = 0,
00425:                     is_starving = false,
00426:                     is_overrun = false,
00427:                     ration_reserve = 0
00428:                 });
00429:             }
00430:
00431:             foreach (var inst in _instances.Values)
00432:             {
00433:                 if (string.IsNullOrEmpty(inst.OutpostId)) continue;
00434:                 var row = state.outposts.Find(o => string.Equals(o.outpost_id, inst.OutpostId, StringComparison.OrdinalIgnoreCase));
00435:                 if (row == null) continue;
00436:                 row.is_established = inst.IsEstablished;
00437:                 row.condition_permille = inst.ConditionPermille;
00438:                 row.garrison_survivor_ids = inst.GarrisonSurvivorIds != null
00439:                     ? new List<string>(inst.GarrisonSurvivorIds)
00440:                     : new List<string>();
00441:                 row.days_since_supply = inst.DaysSinceSupply;
00442:                 row.is_starving = inst.IsStarving;
00443:                 row.is_overrun = inst.IsOverrun;
00444:                 row.ration_reserve = inst.RationReserve;
00445:             }
00446:
00447:             return state;
00448:         }
00449:
00450:         /// <summary>
00451:         /// Plan 58 persistence — restore a captured state over the current
00452:         /// instances. The state is authoritative for every authored outpost;
00453:         /// rows naming an unknown outpost or over-full garrison are rejected
00454:         /// deterministically rather than inventing a phantom outpost.
00455:         /// </summary>
00456:         public bool RestoreState(OutpostSettlementState? state)
00457:         {
00458:             if (state == null || state.outposts == null) return false;
00459:             if (state.schema_version != 1) return false;
00460:
00461:             // Missing authored rows default to unestablished so a partial save
00462:             // can never resurrect an outpost the catalog no longer defines.
00463:             foreach (var inst in _instances.Values)
00464:             {
00465:                 inst.IsEstablished = false;
00466:                 inst.IsOverrun = false;
00467:                 inst.IsStarving = false;
00468:                 inst.ConditionPermille = 1000;
00469:                 inst.DaysSinceSupply = 0;
00470:                 inst.RationReserve = 0;
00471:                 inst.GarrisonSurvivorIds.Clear();
00472:             }
00473:
00474:             foreach (var row in state.outposts)
00475:             {
00476:                 if (row == null || string.IsNullOrWhiteSpace(row.outpost_id)) continue;
00477:                 if (!_instances.TryGetValue(row.outpost_id, out var inst)) continue;  // phantom outpost
00478:                 if (!_definitions.TryGetValue(row.outpost_id, out var def)) continue;
00479:
00480:                 var garrison = row.garrison_survivor_ids ?? new List<string>();
00481:                 var cap = Math.Max(1, def.MaxGarrisonBunks);
00482:                 var bounded = garrison.Count > cap ? garrison.GetRange(0, cap) : garrison;
00483:
00484:                 inst.IsEstablished = row.is_established;
00485:                 inst.ConditionPermille = Math.Max(0, Math.Min(1000, row.condition_permille));
00486:                 inst.GarrisonSurvivorIds = new List<string>(bounded);
00487:                 inst.DaysSinceSupply = Math.Max(0, row.days_since_supply);
00488:                 inst.IsStarving = row.is_starving;
00489:                 inst.IsOverrun = row.is_overrun;
00490:                 inst.RationReserve = Math.Max(0, row.ration_reserve);
00491:             }
00492:
00493:             return true;
00494:         }
00495:     }
00496: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs`

### `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs` — complete current file

- Size: 622 lines / 28809 bytes.
- SHA-256: `bc27b87874570a17bb5d1990ebce70451fac2737edbc6cb4bbc76f05ba31dbff`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: #nullable enable
00002: // SPDX-License-Identifier: MIT
00003: using System;
00004: using System.Collections.Generic;
00005: using System.Linq;
00006: using System.Text.Json;
00007:
00008: namespace Ashfall.Core.Factions
00009: {
00010:     public enum SupplyLineStatus
00011:     {
00012:         Active,
00013:         Disrupted,
00014:         Severed
00015:     }
00016:
00017:     /// <summary>
00018:     /// Authored territory definition from StreamingAssets/Data/faction_territory.json.
00019:     /// </summary>
00020:     public sealed class FactionTerritoryDef
00021:     {
00022:         public string Id { get; set; } = string.Empty;
00023:         public string Faction { get; set; } = string.Empty;
00024:         public string DisplayName { get; set; } = string.Empty;
00025:         public string Classification { get; set; } = string.Empty;
00026:         public string TerritoryScale { get; set; } = string.Empty;
00027:         public string PrimaryResourceInterest { get; set; } = string.Empty;
00028:         public List<string> ControlledNodes { get; set; } = new List<string>();
00029:         public List<string> ControlPoints { get; set; } = new List<string>();
00030:         public List<string> ContestedWith { get; set; } = new List<string>();
00031:         public int BaseControlStrength { get; set; } = 50;
00032:         public double TradeTax { get; set; } = 0.05;
00033:         public double TravelSafety { get; set; } = 0.75;
00034:         public string ShiftTrigger { get; set; } = string.Empty;
00035:         public string Description { get; set; } = string.Empty;
00036:     }
00037:
00038:     /// <summary>
00039:     /// Authored supply line definition from StreamingAssets/Data/supply_lines.json.
00040:     /// </summary>
00041:     public sealed class SupplyLineDef
00042:     {
00043:         public string Id { get; set; } = string.Empty;
00044:         public string OwningFactionId { get; set; } = string.Empty;
00045:         public string OriginLocationId { get; set; } = string.Empty;
00046:         public string DestinationLocationId { get; set; } = string.Empty;
00047:         public List<string> RouteWaypoints { get; set; } = new List<string>();
00048:         public string CargoType { get; set; } = string.Empty;
00049:         public int ThroughputCapacity { get; set; } = 30;
00050:         public int TravelDays { get; set; } = 2;
00051:     }
00052:
00053:     /// <summary>
00054:     /// Runtime dynamic state for a location under territorial control.
00055:     /// </summary>
00056:     public sealed class LocationTerritoryState
00057:     {
00058:         public string LocationId { get; set; } = string.Empty;
00059:         public string ControllingFactionId { get; set; } = string.Empty;
00060:         public int ControlStrength { get; set; } = 50;
00061:         public bool IsContested { get; set; }
00062:         public int FortificationLevel { get; set; }
00063:         public int GarrisonStrength { get; set; }
00064:         public int LastContestDay { get; set; }
00065:     }
00066:
00067:     /// <summary>
00068:     /// Runtime state for a faction supply line.
00069:     /// </summary>
00070:     public sealed class SupplyLineState
00071:     {
00072:         public string SupplyLineId { get; set; } = string.Empty;
00073:         public string OwningFactionId { get; set; } = string.Empty;
00074:         public SupplyLineStatus Status { get; set; } = SupplyLineStatus.Active;
00075:         public int LastDeliveredDay { get; set; }
00076:         public int TotalDelivered { get; set; }
00077:     }
00078:
00079:     /// <summary>
00080:     /// Pure domain engine for Plan 134: Dynamic Faction Territory & Supply Line Control.
00081:     /// Manages territorial expansion, contested locations, fortifications, and supply line corridors.
00082:     /// Zero engine references (Godot/UnityEngine free).
00083:     /// </summary>
00084:     public sealed class TerritoryControlSystem
00085:     {
00086:         public const string SystemId = "territory_control_system";
00087:
00088:         private readonly Dictionary<string, FactionTerritoryDef> _territories = new Dictionary<string, FactionTerritoryDef>(StringComparer.OrdinalIgnoreCase);
00089:         private readonly Dictionary<string, SupplyLineDef> _supplyLineDefs = new Dictionary<string, SupplyLineDef>(StringComparer.OrdinalIgnoreCase);
00090:         private readonly Dictionary<string, LocationTerritoryState> _locationStates = new Dictionary<string, LocationTerritoryState>(StringComparer.OrdinalIgnoreCase);
00091:         private readonly Dictionary<string, SupplyLineState> _supplyLineStates = new Dictionary<string, SupplyLineState>(StringComparer.OrdinalIgnoreCase);
00092:
00093:         // Seam delegates
00094:         public Action<string, string, string>? OnTerritoryControlChangedSeam { get; set; }
00095:         public Action<string, string, string>? OnTerritoryContestedSeam { get; set; }
00096:         public Action<string, SupplyLineStatus>? OnSupplyLineStatusChangedSeam { get; set; }
00097:         public Action<string, int>? OnSupplyLineDeliveredSeam { get; set; }
00098:         public Action<string, int>? OnLocationFortifiedSeam { get; set; }
00099:
00100:         public TerritoryControlSystem(
00101:             IEnumerable<FactionTerritoryDef>? territories = null,
00102:             IEnumerable<SupplyLineDef>? supplyLines = null)
00103:         {
00104:             if (territories != null)
00105:             {
00106:                 foreach (var t in territories)
00107:                 {
00108:                     if (t != null && !string.IsNullOrEmpty(t.Id))
00109:                     {
00110:                         _territories[t.Id] = t;
00111:
00112:                         // Initialize locations from controlled nodes and points
00113:                         var allNodes = t.ControlledNodes.Concat(t.ControlPoints).Distinct(StringComparer.OrdinalIgnoreCase);
00114:                         foreach (var node in allNodes)
00115:                         {
00116:                             if (!string.IsNullOrEmpty(node) && !_locationStates.ContainsKey(node))
00117:                             {
00118:                                 _locationStates[node] = new LocationTerritoryState
00119:                                 {
00120:                                     LocationId = node,
00121:                                     ControllingFactionId = t.Faction,
00122:                                     ControlStrength = t.BaseControlStrength,
00123:                                     IsContested = false,
00124:                                     FortificationLevel = 0,
00125:                                     GarrisonStrength = 10,
00126:                                     LastContestDay = 0
00127:                                 };
00128:                             }
00129:                         }
00130:                     }
00131:                 }
00132:             }
00133:
00134:             if (supplyLines != null)
00135:             {
00136:                 foreach (var line in supplyLines)
00137:                 {
00138:                     if (line != null && !string.IsNullOrEmpty(line.Id))
00139:                     {
00140:                         _supplyLineDefs[line.Id] = line;
00141:                         _supplyLineStates[line.Id] = new SupplyLineState
00142:                         {
00143:                             SupplyLineId = line.Id,
00144:                             OwningFactionId = line.OwningFactionId,
00145:                             Status = SupplyLineStatus.Active,
00146:                             LastDeliveredDay = 0,
00147:                             TotalDelivered = 0
00148:                         };
00149:                     }
00150:                 }
00151:             }
00152:         }
00153:
00154:         /// <summary>
00155:         /// Factory parser for faction_territory.json and supply_lines.json catalogs.
00156:         /// </summary>
00157:         public static TerritoryControlSystem FromJson(string territoryJson, string supplyLineJson)
00158:         {
00159:             var territories = ParseTerritories(territoryJson);
00160:             var supplyLines = ParseSupplyLines(supplyLineJson);
00161:             return new TerritoryControlSystem(territories, supplyLines);
00162:         }
00163:
00164:         private static List<FactionTerritoryDef> ParseTerritories(string json)
00165:         {
00166:             var list = new List<FactionTerritoryDef>();
00167:             if (string.IsNullOrWhiteSpace(json)) return list;
00168:
00169:             using var doc = JsonDocument.Parse(json);
00170:             var root = doc.RootElement;
00171:             if (root.TryGetProperty("territories", out var arr) && arr.ValueKind == JsonValueKind.Array)
00172:             {
00173:                 foreach (var el in arr.EnumerateArray())
00174:                 {
00175:                     var def = new FactionTerritoryDef
00176:                     {
00177:                         Id = el.TryGetProperty("id", out var idElem) ? idElem.GetString() ?? string.Empty : string.Empty,
00178:                         Faction = el.TryGetProperty("faction", out var facElem) ? facElem.GetString() ?? string.Empty : string.Empty,
00179:                         DisplayName = el.TryGetProperty("display_name", out var nameElem) ? nameElem.GetString() ?? string.Empty : string.Empty,
00180:                         Classification = el.TryGetProperty("classification", out var clsElem) ? clsElem.GetString() ?? string.Empty : string.Empty,
00181:                         TerritoryScale = el.TryGetProperty("territory_scale", out var sclElem) ? sclElem.GetString() ?? string.Empty : string.Empty,
00182:                         PrimaryResourceInterest = el.TryGetProperty("primary_resource_interest", out var priElem) ? priElem.GetString() ?? string.Empty : string.Empty,
00183:                         BaseControlStrength = el.TryGetProperty("control_strength", out var strElem) ? strElem.GetInt32() : 50,
00184:                         TradeTax = el.TryGetProperty("trade_tax", out var taxElem) ? taxElem.GetDouble() : 0.05,
00185:                         TravelSafety = el.TryGetProperty("travel_safety", out var safeElem) ? safeElem.GetDouble() : 0.75,
00186:                         ShiftTrigger = el.TryGetProperty("shift_trigger", out var shfElem) ? shfElem.GetString() ?? string.Empty : string.Empty,
00187:                         Description = el.TryGetProperty("description", out var descElem) ? descElem.GetString() ?? string.Empty : string.Empty
00188:                     };
00189:
00190:                     if (el.TryGetProperty("controlled_nodes", out var cnElem) && cnElem.ValueKind == JsonValueKind.Array)
00191:                     {
00192:                         foreach (var node in cnElem.EnumerateArray())
00193:                         {
00194:                             var s = node.GetString();
00195:                             if (!string.IsNullOrEmpty(s)) def.ControlledNodes.Add(s);
00196:                         }
00197:                     }
00198:
00199:                     if (el.TryGetProperty("control_points", out var cpElem) && cpElem.ValueKind == JsonValueKind.Array)
00200:                     {
00201:                         foreach (var cp in cpElem.EnumerateArray())
00202:                         {
00203:                             var s = cp.GetString();
00204:                             if (!string.IsNullOrEmpty(s)) def.ControlPoints.Add(s);
00205:                         }
00206:                     }
00207:
00208:                     if (el.TryGetProperty("contested_with", out var cwElem) && cwElem.ValueKind == JsonValueKind.Array)
00209:                     {
00210:                         foreach (var cw in cwElem.EnumerateArray())
00211:                         {
00212:                             var s = cw.GetString();
00213:                             if (!string.IsNullOrEmpty(s)) def.ContestedWith.Add(s);
00214:                         }
00215:                     }
00216:
00217:                     list.Add(def);
00218:                 }
00219:             }
00220:
00221:             return list;
00222:         }
00223:
00224:         private static List<SupplyLineDef> ParseSupplyLines(string json)
00225:         {
00226:             var list = new List<SupplyLineDef>();
00227:             if (string.IsNullOrWhiteSpace(json)) return list;
00228:
00229:             using var doc = JsonDocument.Parse(json);
00230:             var root = doc.RootElement;
00231:             if (root.TryGetProperty("supply_lines", out var arr) && arr.ValueKind == JsonValueKind.Array)
00232:             {
00233:                 foreach (var el in arr.EnumerateArray())
00234:                 {
00235:                     var def = new SupplyLineDef
00236:                     {
00237:                         Id = el.TryGetProperty("id", out var idElem) ? idElem.GetString() ?? string.Empty : string.Empty,
00238:                         OwningFactionId = el.TryGetProperty("owning_faction_id", out var ownElem) ? ownElem.GetString() ?? string.Empty : string.Empty,
00239:                         OriginLocationId = el.TryGetProperty("origin_location_id", out var orgElem) ? orgElem.GetString() ?? string.Empty : string.Empty,
00240:                         DestinationLocationId = el.TryGetProperty("destination_location_id", out var dstElem) ? dstElem.GetString() ?? string.Empty : string.Empty,
00241:                         CargoType = el.TryGetProperty("cargo_type", out var crgElem) ? crgElem.GetString() ?? string.Empty : string.Empty,
00242:                         ThroughputCapacity = el.TryGetProperty("throughput_capacity", out var capElem) ? capElem.GetInt32() : 30,
00243:                         TravelDays = el.TryGetProperty("travel_days", out var dayElem) ? dayElem.GetInt32() : 2
00244:                     };
00245:
00246:                     if (el.TryGetProperty("route_waypoints", out var rwElem) && rwElem.ValueKind == JsonValueKind.Array)
00247:                     {
00248:                         foreach (var wp in rwElem.EnumerateArray())
00249:                         {
00250:                             var s = wp.GetString();
00251:                             if (!string.IsNullOrEmpty(s)) def.RouteWaypoints.Add(s);
00252:                         }
00253:                     }
00254:
00255:                     list.Add(def);
00256:                 }
00257:             }
00258:
00259:             return list;
00260:         }
00261:
00262:         public FactionTerritoryDef? GetTerritory(string territoryId)
00263:         {
00264:             if (string.IsNullOrEmpty(territoryId)) return null;
00265:             _territories.TryGetValue(territoryId, out var def);
00266:             return def;
00267:         }
00268:
00269:         public LocationTerritoryState? GetLocationState(string locationId)
00270:         {
00271:             if (string.IsNullOrEmpty(locationId)) return null;
00272:             _locationStates.TryGetValue(locationId, out var state);
00273:             return state;
00274:         }
00275:
00276:         public SupplyLineState? GetSupplyLineState(string lineId)
00277:         {
00278:             if (string.IsNullOrEmpty(lineId)) return null;
00279:             _supplyLineStates.TryGetValue(lineId, out var state);
00280:             return state;
00281:         }
00282:
00283:         public IReadOnlyList<FactionTerritoryDef> GetAllTerritories() => _territories.Values.ToList();
00284:         public IReadOnlyList<LocationTerritoryState> GetAllLocationStates() => _locationStates.Values.ToList();
00285:         public IReadOnlyList<SupplyLineState> GetAllSupplyLines() => _supplyLineStates.Values.ToList();
00286:
00287:         /// <summary>
00288:         /// Upgrades or modifies the fortification level of a controlled location (0..3).
00289:         /// </summary>
00290:         public bool FortifyLocation(string locationId, int levelDelta = 1)
00291:         {
00292:             if (!_locationStates.TryGetValue(locationId, out var state)) return false;
00293:
00294:             int newLevel = Math.Clamp(state.FortificationLevel + levelDelta, 0, 3);
00295:             if (newLevel == state.FortificationLevel) return false;
00296:
00297:             state.FortificationLevel = newLevel;
00298:             state.ControlStrength = Math.Clamp(state.ControlStrength + (levelDelta * 10), 0, 100);
00299:             OnLocationFortifiedSeam?.Invoke(locationId, newLevel);
00300:             return true;
00301:         }
00302:
00303:         /// <summary>
00304:         /// Assigns or modifies garrison strength at a location.
00305:         /// </summary>
00306:         public bool AssignGarrison(string locationId, int garrisonDelta)
00307:         {
00308:             if (!_locationStates.TryGetValue(locationId, out var state)) return false;
00309:
00310:             state.GarrisonStrength = Math.Max(0, state.GarrisonStrength + garrisonDelta);
00311:             state.ControlStrength = Math.Clamp(state.ControlStrength + (garrisonDelta > 0 ? 5 : -5), 10, 100);
00312:             return true;
00313:         }
00314:
00315:         /// <summary>
00316:         /// Resolves a territorial contest or attack against a location.
00317:         /// Returns true if the location changes controlling faction.
00318:         /// </summary>
00319:         public bool ContestLocation(string locationId, string attackingFactionId, int attackPower, ISeededRng rng, int currentDay = 0)
00320:         {
00321:             if (rng == null) throw new ArgumentNullException(nameof(rng));
00322:             if (string.IsNullOrEmpty(attackingFactionId)) return false;
00323:             if (!_locationStates.TryGetValue(locationId, out var state)) return false;
00324:
00325:             if (string.Equals(state.ControllingFactionId, attackingFactionId, StringComparison.OrdinalIgnoreCase))
00326:                 return false;
00327:
00328:             state.LastContestDay = currentDay;
00329:             OnTerritoryContestedSeam?.Invoke(locationId, state.ControllingFactionId, attackingFactionId);
00330:
00331:             int effectiveDefense = state.ControlStrength + (state.FortificationLevel * 15) + (state.GarrisonStrength * 3);
00332:
00333:             if (attackPower >= effectiveDefense * 2)
00334:             {
00335:                 // Overwhelming victory guarantees capture
00336:                 string oldFaction = state.ControllingFactionId;
00337:                 state.ControllingFactionId = attackingFactionId;
00338:                 state.ControlStrength = Math.Clamp(attackPower - effectiveDefense + 20, 25, 80);
00339:                 state.IsContested = false;
00340:                 state.FortificationLevel = Math.Max(0, state.FortificationLevel - 1);
00341:                 state.GarrisonStrength = Math.Max(5, attackPower / 10);
00342:
00343:                 OnTerritoryControlChangedSeam?.Invoke(locationId, oldFaction, attackingFactionId);
00344:                 return true;
00345:             }
00346:
00347:             if (attackPower > effectiveDefense)
00348:             {
00349:                 double winChance = Math.Clamp((attackPower - effectiveDefense) / 100.0 + 0.45, 0.20, 0.95);
00350:                 if (rng.NextDouble() < winChance)
00351:                 {
00352:                     string oldFaction = state.ControllingFactionId;
00353:                     state.ControllingFactionId = attackingFactionId;
00354:                     state.ControlStrength = Math.Clamp(attackPower - effectiveDefense + 20, 25, 80);
00355:                     state.IsContested = false;
00356:                     state.FortificationLevel = Math.Max(0, state.FortificationLevel - 1);
00357:                     state.GarrisonStrength = Math.Max(5, attackPower / 10);
00358:
00359:                     OnTerritoryControlChangedSeam?.Invoke(locationId, oldFaction, attackingFactionId);
00360:                     return true;
00361:                 }
00362:                 else
00363:                 {
00364:                     // Defense held, but garrison and control suffered
00365:                     state.ControlStrength = Math.Max(10, state.ControlStrength - 20);
00366:                     state.GarrisonStrength = Math.Max(1, state.GarrisonStrength - 5);
00367:                     state.IsContested = true;
00368:                     return false;
00369:                 }
00370:             }
00371:             else
00372:             {
00373:                 // Defense clearly held
00374:                 state.ControlStrength = Math.Max(15, state.ControlStrength - 5);
00375:                 state.IsContested = false;
00376:                 return false;
00377:             }
00378:         }
00379:
00380:         /// <summary>
00381:         /// Raids an active supply line. Returns true if the line was disrupted or severed.
00382:         /// </summary>
00383:         public bool RaidSupplyLine(string supplyLineId, int raidIntensity, ISeededRng rng)
00384:         {
00385:             if (rng == null) throw new ArgumentNullException(nameof(rng));
00386:             if (!_supplyLineStates.TryGetValue(supplyLineId, out var state)) return false;
00387:             if (state.Status == SupplyLineStatus.Severed) return false;
00388:
00389:             double disruptionChance = Math.Clamp(raidIntensity / 100.0, 0.1, 0.95);
00390:             if (rng.NextDouble() < disruptionChance)
00391:             {
00392:                 var newStatus = raidIntensity >= 80 ? SupplyLineStatus.Severed : SupplyLineStatus.Disrupted;
00393:                 state.Status = newStatus;
00394:
00395:                 // Disrupting a line degrades destination location control
00396:                 if (_supplyLineDefs.TryGetValue(supplyLineId, out var def))
00397:                 {
00398:                     if (_locationStates.TryGetValue(def.DestinationLocationId, out var locState))
00399:                     {
00400:                         locState.ControlStrength = Math.Max(10, locState.ControlStrength - 15);
00401:                     }
00402:                 }
00403:
00404:                 OnSupplyLineStatusChangedSeam?.Invoke(supplyLineId, newStatus);
00405:                 return true;
00406:             }
00407:
00408:             return false;
00409:         }
00410:
00411:         /// <summary>
00412:         /// Restores a disrupted or severed supply line back to active service.
00413:         /// </summary>
00414:         public bool RestoreSupplyLine(string supplyLineId)
00415:         {
00416:             if (!_supplyLineStates.TryGetValue(supplyLineId, out var state)) return false;
00417:             if (state.Status == SupplyLineStatus.Active) return false;
00418:
00419:             state.Status = SupplyLineStatus.Active;
00420:             OnSupplyLineStatusChangedSeam?.Invoke(supplyLineId, SupplyLineStatus.Active);
00421:             return true;
00422:         }
00423:
00424:         /// <summary>
00425:         /// Daily advance: delivers supply shipments along active lines and stabilizes control.
00426:         /// </summary>
00427:         public void TickDay(int currentDay, ISeededRng? rng = null)
00428:         {
00429:             foreach (var kvp in _supplyLineStates)
00430:             {
00431:                 var state = kvp.Value;
00432:                 if (!_supplyLineDefs.TryGetValue(state.SupplyLineId, out var def)) continue;
00433:
00434:                 if (state.Status == SupplyLineStatus.Active)
00435:                 {
00436:                     state.TotalDelivered += def.ThroughputCapacity;
00437:                     state.LastDeliveredDay = currentDay;
00438:
00439:                     // Reinforce destination control strength
00440:                     if (_locationStates.TryGetValue(def.DestinationLocationId, out var locState))
00441:                     {
00442:                         locState.ControlStrength = Math.Min(100, locState.ControlStrength + 2);
00443:                     }
00444:
00445:                     OnSupplyLineDeliveredSeam?.Invoke(state.SupplyLineId, def.ThroughputCapacity);
00446:                 }
00447:             }
00448:         }
00449:
00450:         /// <summary>
00451:         /// Captures the persistent runtime state of all locations and supply lines.
00452:         /// Serialized in stable order for deterministic round-trips.
00453:         /// </summary>
00454:         public TerritoryControlSaveState CaptureState()
00455:         {
00456:             var state = new TerritoryControlSaveState { schema_version = 1 };
00457:
00458:             foreach (var loc in _locationStates.Values.OrderBy(l => l.LocationId, StringComparer.Ordinal))
00459:             {
00460:                 state.locations.Add(new LocationTerritorySaveState
00461:                 {
00462:                     location_id = loc.LocationId,
00463:                     controlling_faction_id = loc.ControllingFactionId,
00464:                     control_strength = loc.ControlStrength,
00465:                     is_contested = loc.IsContested,
00466:                     fortification_level = loc.FortificationLevel,
00467:                     garrison_strength = loc.GarrisonStrength,
00468:                     last_contest_day = loc.LastContestDay
00469:                 });
00470:             }
00471:
00472:             foreach (var line in _supplyLineStates.Values.OrderBy(s => s.SupplyLineId, StringComparer.Ordinal))
00473:             {
00474:                 state.supply_lines.Add(new SupplyLineSaveState
00475:                 {
00476:                     supply_line_id = line.SupplyLineId,
00477:                     owning_faction_id = line.OwningFactionId,
00478:                     status = line.Status.ToString(),
00479:                     last_delivered_day = line.LastDeliveredDay,
00480:                     total_delivered = line.TotalDelivered
00481:                 });
00482:             }
00483:
00484:             return state;
00485:         }
00486:
00487:         /// <summary>
00488:         /// Restores persistent state over the current catalog instances.
00489:         /// Phantom locations or supply lines not present in the catalog are safely ignored.
00490:         /// </summary>
00491:         public bool RestoreState(TerritoryControlSaveState? state)
00492:         {
00493:             if (state == null || state.schema_version != 1) return false;
00494:
00495:             if (state.locations != null)
00496:             {
00497:                 foreach (var locSave in state.locations)
00498:                 {
00499:                     if (locSave == null || string.IsNullOrWhiteSpace(locSave.location_id)) continue;
00500:                     if (!_locationStates.TryGetValue(locSave.location_id, out var locState)) continue;
00501:
00502:                     if (!string.IsNullOrEmpty(locSave.controlling_faction_id))
00503:                     {
00504:                         locState.ControllingFactionId = locSave.controlling_faction_id;
00505:                     }
00506:                     locState.ControlStrength = Math.Clamp(locSave.control_strength, 0, 100);
00507:                     locState.IsContested = locSave.is_contested;
00508:                     locState.FortificationLevel = Math.Clamp(locSave.fortification_level, 0, 3);
00509:                     locState.GarrisonStrength = Math.Max(0, locSave.garrison_strength);
00510:                     locState.LastContestDay = Math.Max(0, locSave.last_contest_day);
00511:                 }
00512:             }
00513:
00514:             if (state.supply_lines != null)
00515:             {
00516:                 foreach (var lineSave in state.supply_lines)
00517:                 {
00518:                     if (lineSave == null || string.IsNullOrWhiteSpace(lineSave.supply_line_id)) continue;
00519:                     if (!_supplyLineStates.TryGetValue(lineSave.supply_line_id, out var lineState)) continue;
00520:
00521:                     if (Enum.TryParse<SupplyLineStatus>(lineSave.status, true, out var parsedStatus))
00522:                     {
00523:                         lineState.Status = parsedStatus;
00524:                     }
00525:                     lineState.LastDeliveredDay = Math.Max(0, lineSave.last_delivered_day);
00526:                     lineState.TotalDelivered = Math.Max(0, lineSave.total_delivered);
00527:                 }
00528:             }
00529:
00530:             return true;
00531:         }
00532:
00533:         /// <summary>
00534:         /// Produces a read-only census summary of territories, locations, and supply line statuses.
00535:         /// </summary>
00536:         public TerritoryCensus ReadCensus()
00537:         {
00538:             int contestedCount = _locationStates.Values.Count(l => l.IsContested);
00539:             int activeLines = _supplyLineStates.Values.Count(s => s.Status == SupplyLineStatus.Active);
00540:             int disruptedLines = _supplyLineStates.Values.Count(s => s.Status == SupplyLineStatus.Disrupted);
00541:             int severedLines = _supplyLineStates.Values.Count(s => s.Status == SupplyLineStatus.Severed);
00542:
00543:             return new TerritoryCensus(
00544:                 _territories.Count,
00545:                 _locationStates.Count,
00546:                 contestedCount,
00547:                 _supplyLineStates.Count,
00548:                 activeLines,
00549:                 disruptedLines,
00550:                 severedLines);
00551:         }
00552:     }
00553:
00554:     /// <summary>
00555:     /// Save DTO for a location under territorial control.
00556:     /// </summary>
00557:     public sealed class LocationTerritorySaveState
00558:     {
00559:         public string location_id { get; set; } = string.Empty;
00560:         public string controlling_faction_id { get; set; } = string.Empty;
00561:         public int control_strength { get; set; } = 50;
00562:         public bool is_contested { get; set; }
00563:         public int fortification_level { get; set; }
00564:         public int garrison_strength { get; set; }
00565:         public int last_contest_day { get; set; }
00566:     }
00567:
00568:     /// <summary>
00569:     /// Save DTO for a faction supply line.
00570:     /// </summary>
00571:     public sealed class SupplyLineSaveState
00572:     {
00573:         public string supply_line_id { get; set; } = string.Empty;
00574:         public string owning_faction_id { get; set; } = string.Empty;
00575:         public string status { get; set; } = "Active";
00576:         public int last_delivered_day { get; set; }
00577:         public int total_delivered { get; set; }
00578:     }
00579:
00580:     /// <summary>
00581:     /// Plan 134 persistence envelope for TerritoryControlSystem.
00582:     /// </summary>
00583:     public sealed class TerritoryControlSaveState
00584:     {
00585:         public int schema_version { get; set; } = 1;
00586:         public List<LocationTerritorySaveState> locations { get; set; } = new List<LocationTerritorySaveState>();
00587:         public List<SupplyLineSaveState> supply_lines { get; set; } = new List<SupplyLineSaveState>();
00588:     }
00589:
00590:     /// <summary>
00591:     /// Read-only snapshot of territorial control metrics.
00592:     /// </summary>
00593:     public readonly struct TerritoryCensus
00594:     {
00595:         public readonly int TerritoriesCount;
00596:         public readonly int LocationsCount;
00597:         public readonly int ContestedLocationsCount;
00598:         public readonly int TotalSupplyLines;
00599:         public readonly int ActiveSupplyLines;
00600:         public readonly int DisruptedSupplyLines;
00601:         public readonly int SeveredSupplyLines;
00602:
00603:         public int TotalTerritories => TerritoriesCount;
00604:         public int TotalNodes => LocationsCount;
00605:         public int ContestedLocations => ContestedLocationsCount;
00606:
00607:         public TerritoryCensus(int territories, int locations, int contested, int totalLines, int activeLines, int disruptedLines, int severedLines)
00608:         {
00609:             TerritoriesCount = territories;
00610:             LocationsCount = locations;
00611:             ContestedLocationsCount = contested;
00612:             TotalSupplyLines = totalLines;
00613:             ActiveSupplyLines = activeLines;
00614:             DisruptedSupplyLines = disruptedLines;
00615:             SeveredSupplyLines = severedLines;
00616:         }
00617:
00618:         public string Describe() =>
00619:             $"territories: {TerritoriesCount} defined, {LocationsCount} node(s) ({ContestedLocationsCount} contested); "
00620:             + $"supply lines: {ActiveSupplyLines}/{TotalSupplyLines} active, {DisruptedSupplyLines} disrupted, {SeveredSupplyLines} severed";
00621:     }
00622: }
```


# Appendix — Current Source Detail: `src/Host/OutpostSettlementHostSession.cs`

### `src/Host/OutpostSettlementHostSession.cs` — complete current file

- Size: 214 lines / 9863 bytes.
- SHA-256: `bf8575bf7708e497711c6e5e06c6448c230055b52b358da3552a4646bd0a5d52`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Inventory;
00008: using Ashfall.Core.Settlements;
00009: using Ashfall.Core.Save;
00010:
00011: namespace AtomicWar.GodotApp
00012: {
00013:     /// <summary>
00014:     /// Plan 58 / The Continuation — host session for the authored outpost and
00015:     /// secondary-holdfast network.
00016:     /// <para>
00017:     /// Custody boundary (signed premise, recorded in
00018:     /// <c>docs/plans/ORPHAN_SEAL_PRIORITY_W1_BOUNDARIES.md</c> §2):
00019:     /// <list type="bullet">\n    /// <item><see cref=\"OutpostSettlementSystem\"/> is the single authority for\n    /// authored outposts and secondary positions.</item>\n    /// <item><c>WaystationSystem</c> (save section <c>waystation</c>) remains the\n    /// holdfast S2 waystation authority — separate, not merged.</item>\n    /// <item><c>ColonySystem</c> (section <c>colony</c>) remains the player-founded\n    /// colony authority — separate, not merged.</item>\n    /// <item><c>SettlementCatalog</c> remains the world settlement catalog — a\n    /// read-only graph/geography reference for an outpost's parent node.</item>\n    /// </list>\n    /// No second population, food, inventory, or settlement ledger is created
00020:     /// here: the session draws garrison from the canonical roster and rations
00021:     /// from the canonical inventory through caller-supplied providers, and it
00022:     /// stores only outpost state (establishment, condition, garrison ids, supply
00023:     /// reserve, starvation/overrun flags).\n    /// </para>\n    /// </summary>
00024:     public sealed class OutpostSettlementHostSession : HostSessionBase
00025:     {
00026:         /// <summary>Authored outpost catalog in the data authority.</summary>
00027:         public const string CatalogFile = "outposts.json";
00028:
00029:         private readonly List<string> _rawJsonLines = new List<string>();
00030:
00031:         public OutpostSettlementSystem System { get; }
00032:
00033:         /// <summary>True when the authored catalog loaded through the Core parser.</summary>
00034:         public bool CatalogLoaded { get; private set; }
00035:
00036:         /// <summary>Constructor for a live, bound outpost session.</summary>
00037:         public OutpostSettlementHostSession(OutpostSettlementSystem system)
00038:         {
00039:             System = system ?? throw new ArgumentNullException(nameof(system));
00040:         }
00041:
00042:         /// <summary>
00043:         /// Load the authored outpost catalog. A missing or malformed file is a
00044:         /// hard failure: an outpost that is not authored is not invented here.
00045:         /// </summary>
00046:         public static OutpostSettlementHostSession Load(string dataDirectory, IFileIO files)
00047:         {
00048:             string path = Path.Combine(dataDirectory, CatalogFile);
00049:             if (files == null || !files.FileExists(path))
00050:                 throw new InvalidOperationException(
00051:                     $"OutpostSettlementHostSession: {CatalogFile} does not exist at '{path}'.");
00052:
00053:             var system = OutpostSettlementSystem.FromJson(files.ReadAllText(path));
00054:             if (system.GetAllDefinitions().Count == 0)
00055:                 throw new InvalidOperationException(
00056:                     $"OutpostSettlementHostSession: {CatalogFile} defines no outposts.");
00057:
00058:             return new OutpostSettlementHostSession(system);
00059:         }
00060:
00061:         /// <summary>
00062:         /// A census of the current network. Read-only: no state is mutated, so
00063:         /// this is safe to call from a UI projection at any time.
00064:         /// </summary>
00065:         public OutpostCensus ReadCensus()
00066:         {
00067:             int established = 0, overrun = 0, starving = 0, garrisoned = 0, rations = 0;
00068:             foreach (var inst in System.GetAllInstances())
00069:             {
00070:                 if (inst == null) continue;
00071:                 if (inst.IsEstablished) established++;
00072:                 if (inst.IsOverrun) overrun++;
00073:                 if (inst.IsStarving) starving++;
00074:                 garrisoned += inst.GarrisonSurvivorIds?.Count ?? 0;
00075:                 rations += inst.RationReserve;
00076:             }
00077:
00078:             return new OutpostCensus(
00079:                 System.GetAllDefinitions().Count,
00080:                 established,
00081:                 overrun,
00082:                 starving,
00083:                 garrisoned,
00084:                 rations);
00085:         }
00086:
00087:         /// <summary>Establish an outpost, consuming build cost through the provider.</summary>
00088:         public bool Establish(string outpostId, Func<string, int, bool>? costConsumer = null)
00089:         {
00090:             bool established = System.EstablishOutpost(outpostId, costConsumer);
00091:             if (established) RaiseStateChanged();
00092:             return established;
00093:         }
00094:
00095:         /// <summary>Establish an outpost against the atomic canonical inventory bill.</summary>
00096:         public bool TryEstablish(string outpostId, IPlayerInventoryPort? inventory)
00097:         {
00098:             bool established = System.TryEstablishOutpost(outpostId, inventory);
00099:             if (established) RaiseStateChanged();
00100:             return established;
00101:         }
00102:
00103:         /// <summary>Assign a roster survivor to an outpost garrison.</summary>
00104:         public bool AssignGarrison(string outpostId, string survivorId, Func<string, bool>? fitnessCheck = null)
00105:         {
00106:             bool assigned = System.AssignGarrison(outpostId, survivorId, fitnessCheck);
00107:             if (assigned) RaiseStateChanged();
00108:             return assigned;
00109:         }
00110:
00111:         /// <summary>Relieve a garrison survivor back to central holdfast duty.</summary>
00112:         public bool RelieveGarrison(string outpostId, string survivorId)
00113:         {
00114:             bool relieved = System.RelieveGarrison(outpostId, survivorId);
00115:             if (relieved) RaiseStateChanged();
00116:             return relieved;
00117:         }
00118:
00119:         /// <summary>Deliver rations to an outpost reserve.</summary>
00120:         public bool Supply(string outpostId, int rationsDelivered)
00121:         {
00122:             bool supplied = System.SupplyOutpost(outpostId, rationsDelivered);
00123:             if (supplied) RaiseStateChanged();
00124:             return supplied;
00125:         }
00126:
00127:         /// <summary>Supply an established outpost from canonical inventory.</summary>
00128:         public bool TrySupply(string outpostId, string rationItemId, int rationsDelivered,
00129:             IPlayerInventoryPort? inventory)
00130:         {
00131:             bool supplied = System.TrySupplyOutpost(outpostId, rationItemId, rationsDelivered, inventory);
00132:             if (supplied) RaiseStateChanged();
00133:             return supplied;
00134:         }
00135:
00136:         /// <summary>Abandon an established outpost.</summary>
00137:         public bool Abandon(string outpostId)
00138:         {
00139:             bool abandoned = System.AbandonOutpost(outpostId ?? string.Empty);
00140:             if (abandoned) RaiseStateChanged();
00141:             return abandoned;
00142:         }
00143:
00144:         /// <summary>Advance the daily outpost lifecycle.</summary>
00145:         public void TickDay(Func<string, int, int>? centralRationSupplyProvider = null)
00146:         {
00147:             System.TickDay(centralRationSupplyProvider);
00148:             RaiseStateChanged();
00149:         }
00150:
00151:         /// <summary>Resolve hostile pressure against one outpost.</summary>
00152:         public bool SimulateRisk(string outpostId, int dangerRating, ISeededRng rng)
00153:             => System.SimulateRisk(outpostId ?? string.Empty, dangerRating, rng);
00154:
00155:         /// <summary>Capture the outpost section (definitions are never persisted).</summary>
00156:         public OutpostSettlementState CaptureState() => System.CaptureState();
00157:
00158:         /// <summary>Restore the outpost section over the live instances.</summary>
00159:         public bool RestoreState(OutpostSettlementState? state) => System.RestoreState(state);
00160:
00161:         /// <summary>Append one diegetic line for the host report (bounded).</summary>
00162:         public void AppendReportLine(string line)
00163:         {
00164:             if (string.IsNullOrWhiteSpace(line)) return;
00165:             _rawJsonLines.Add(line);
00166:             RaiseStateChanged();
00167:         }
00168:
00169:         public IReadOnlyList<string> ReportLines => _rawJsonLines;
00170:
00171:         public string Describe() => ReadCensus().Describe();
00172:     }
00173:
00174:     /// <summary>Read-model census of the outpost network. No mutable state.</summary>
00175:     public readonly struct OutpostCensus
00176:     {
00177:         public readonly int Authored;
00178:         public readonly int Established;
00179:         public readonly int Overrun;
00180:         public readonly int Starving;
00181:         public readonly int Garrisoned;
00182:         public readonly int RationReserve;
00183:
00184:         public OutpostCensus(int authored, int established, int overrun, int starving,
00185:             int garrisoned, int rationReserve)
00186:         {
00187:             Authored = authored;
00188:             Established = established;
00189:             Overrun = overrun;
00190:             Starving = starving;
00191:             Garrisoned = garrisoned;
00192:             RationReserve = rationReserve;
00193:         }
00194:
00195:         public string Describe()
00196:             => $"outposts: {Established}/{Authored} established, {Garrisoned} garrisoned, "
00197:                 + $"{RationReserve} ration(s) in reserve, {Overrun} overrun, {Starving} starving";
00198:     }
00199:
00200:     public static class OutpostSettlementSaveStore
00201:     {
00202:         public const string FileName = "outpost_settlement_save.json";
00203:         public const string SectionName = "outpost_settlement";
00204:
00205:         private static readonly SaveStore<OutpostSettlementState> s_store =
00206:             SaveStoreHub.Checksummed<OutpostSettlementState>(FileName, nameof(OutpostSettlementSaveStore));
00207:
00208:         public static bool TrySave(OutpostSettlementState state) => s_store.TrySave(state);
00209:         public static OutpostSettlementState? TryLoad() => s_store.TryLoad();
00210:         public static string TryCapturePersisted(OutpostSettlementState state) => s_store.CapturePersisted(state);
00211:         public static OutpostSettlementState? TryRestore(string json) => s_store.RestoreEnvelope(json);
00212:         public static OutpostSettlementState? TryRestoreBare(string json) => s_store.RestoreBare(json);
00213:     }
00214: }
```


# Appendix — Current Source Detail: `src/Host/HostCli.WastelandInhabitants.cs`

### `src/Host/HostCli.WastelandInhabitants.cs` — complete current file

- Size: 143 lines / 9671 bytes.
- SHA-256: `0176f30e13d97c54e4f5b5fcd6e88e5b7ee11bc5eae9c2ee076223da53510183`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using Godot;
00003: using System;
00004: using System.Collections.Generic;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Narrative;
00008: using Ashfall.Core.World;
00009:
00010: namespace AtomicWar.GodotApp
00011: {
00012:     public static partial class HostCli
00013:     {
00014:         /// <summary>
00015:         /// --wasteland-inhabitants-selftest / --plan20-selftest:
00016:         /// Verifies Plan 20 Wasteland Inhabitants:
00017:         /// 1. Field Guide (32 entries: 20 fauna + 12 flora, actionable intel, unlock tracking)
00018:         /// 2. Wasteland Settlements (6 settlements across 6 archetypes, 18 named NPCs, standing greetings, trade tells)
00019:         /// 3. Repeatable Side-Work (6 quest templates, cooldowns, completion persistence)
00020:         /// 4. Route-Aware Travel Encounters (24 encounters + 4 multi-stage chains, stance weighting, deterministic selection)
00021:         /// </summary>
00022:         public static int RunWastelandInhabitantsSelfTest(string dataDirectory)
00023:         {
00024:             CatalogLocator.UseInvariantCulture();
00025:             int failures = 0;
00026:             int totalAssertions = 0;
00027:
00028:             void Check(bool ok, string label)
00029:             {
00030:                 totalAssertions++;
00031:                 GD.Print($"[{(ok ? "PASS" : "FAIL")}] {label}");
00032:                 if (!ok) failures++;
00033:             }
00034:
00035:             GD.Print("[WastelandInhabitantsHeadlessDemo] begin Plan 20 verification...");
00036:
00037:             var fileIO = new FileSystemIO();
00038:
00039:             // ── 1. Field Guide Catalog & Intel ──────────────────────────────
00040:             var fieldGuide = FieldGuideCatalog.LoadFromDirectory(dataDirectory, fileIO);
00041:             Check(fieldGuide.Count == 32, $"Field guide entry count == 32 (got {fieldGuide.Count})");
00042:
00043:             var fauna = fieldGuide.GetEntriesByCategory("Fauna");
00044:             var flora = fieldGuide.GetEntriesByCategory("Flora");
00045:             Check(fauna.Count == 20, $"Field guide fauna count == 20 (got {fauna.Count})");
00046:             Check(flora.Count == 12, $"Field guide flora count == 12 (got {flora.Count})");
00047:
00048:             Check(fieldGuide.TryGetEntry("field_fauna_two_headed_wolf", out var wolf) && wolf.ThreatLevel == 3, "Two-Headed Wolf threat level == 3");
00049:             Check(fieldGuide.TryGetEntry("field_fauna_cave_bear", out var bear) && bear.ThreatLevel == 5, "Cave Bear threat level == 5");
00050:             Check(fieldGuide.TryGetEntry("field_flora_nitrogen_mushroom", out var shroom) && !string.IsNullOrEmpty(shroom.Edibility), "Nitrogen Mushroom edibility documented");
00051:             Check(fieldGuide.TryGetEntry("field_flora_glowing_rad_rye", out var rye) && rye.Tags.Contains("grain"), "Glowing Rad Rye tagged with 'grain'");
00052:
00053:             // Unlock tracking & roundtrip
00054:             fieldGuide.UnlockEntry("field_fauna_two_headed_wolf");
00055:             fieldGuide.UnlockEntry("field_flora_nitrogen_mushroom");
00056:             Check(fieldGuide.UnlockedCount == 2, "Field guide unlocked count == 2");
00057:             Check(fieldGuide.IsUnlocked("field_fauna_two_headed_wolf"), "Wolf unlocked in guide");
00058:
00059:             var fgState = fieldGuide.CaptureState();
00060:             var fgNew = FieldGuideCatalog.LoadFromDirectory(dataDirectory, fileIO);
00061:             fgNew.RestoreState(fgState);
00062:             Check(fgNew.UnlockedCount == 2 && fgNew.IsUnlocked("field_flora_nitrogen_mushroom"), "Field guide unlock state restored cleanly");
00063:
00064:             // ── 2. Wasteland Settlements & 18 NPCs ──────────────────────────
00065:             var settlements = SettlementCatalog.LoadFromDirectory(dataDirectory, fileIO);
00066:             Check(settlements.SettlementCount == 6, $"Settlement count == 6 (got {settlements.SettlementCount})");
00067:             Check(settlements.NpcCount == 18, $"Settlement NPC count == 18 (got {settlements.NpcCount})");
00068:             Check(settlements.QuestCount == 6, $"Repeatable quest count == 6 (got {settlements.QuestCount})");
00069:
00070:             // Check each settlement archetype
00071:             Check(settlements.TryGetSettlement("settlement_brine_pans", out var brine) && brine.Archetype == "Salt Camp", "Brine-Pan Hollow is Salt Camp");
00072:             Check(settlements.TryGetSettlement("settlement_iron_siding", out var siding) && siding.Archetype == "Rail Siding Town", "Iron Siding is Rail Siding Town");
00073:             Check(settlements.TryGetSettlement("settlement_cape_beacon", out var cape) && cape.Archetype == "Coastal Lighthouse Commune", "Cape Beacon is Coastal Lighthouse Commune");
00074:             Check(settlements.TryGetSettlement("settlement_slate_hollow", out var slate) && slate.Archetype == "Quarry Enclave", "Slate Hollow is Quarry Enclave");
00075:             Check(settlements.TryGetSettlement("settlement_pilgrim_hearth", out var pilgrim) && pilgrim.Archetype == "Religious / Monastic Sanctuary", "Pilgrim's Hearth is Monastic Sanctuary");
00076:             Check(settlements.TryGetSettlement("settlement_tinkers_notch", out var tinker) && tinker.Archetype == "Free Trader Scrap Market", "Tinker's Notch is Free Trader Scrap Market");
00077:
00078:             // Check NPC roles & standing greetings
00079:             Check(settlements.TryGetNpc("npc_salt_marshal_varn", out var varn) && varn.Role == "Keeper", "Marshal Varn is Keeper at Brine-Pan");
00080:             Check(settlements.TryGetNpc("npc_salt_trader_elena", out var elena) && elena.Role == "Trader", "Elena Kosh is Trader at Brine-Pan");
00081:             Check(settlements.TryGetNpc("npc_salt_boiler_petyr", out var petyr) && petyr.Role == "Fixture", "Petyr is Fixture at Brine-Pan");
00082:
00083:             string lowGreeting = settlements.GetNpcGreeting("npc_salt_marshal_varn", -30f);
00084:             string neutralGreeting = settlements.GetNpcGreeting("npc_salt_marshal_varn", 0f);
00085:             string highGreeting = settlements.GetNpcGreeting("npc_salt_marshal_varn", 50f);
00086:             Check(!string.IsNullOrEmpty(lowGreeting) && !string.IsNullOrEmpty(neutralGreeting) && !string.IsNullOrEmpty(highGreeting), "Varn has 3 standing-reactive greetings");
00087:             Check(lowGreeting != neutralGreeting && neutralGreeting != highGreeting, "Greetings differ across standing tiers");
00088:
00089:             // ── 3. Repeatable Side-Work Quests ──────────────────────────────
00090:             string questId = "quest_repeat_salt_boiler_scum";
00091:             Check(settlements.IsQuestAvailable(questId, 1), "Side-work quest initially available on Day 1");
00092:             settlements.CompleteQuest(questId, 1);
00093:             Check(!settlements.IsQuestAvailable(questId, 2), "Side-work quest in cooldown on Day 2");
00094:             Check(!settlements.IsQuestAvailable(questId, 7), "Side-work quest in cooldown on Day 7");
00095:             Check(settlements.IsQuestAvailable(questId, 8), "Side-work quest available again on Day 8");
00096:             Check(settlements.GetCompletedQuestCount(questId) == 1, "Completed quest count == 1");
00097:
00098:             var stState = settlements.CaptureState();
00099:             var stNew = SettlementCatalog.LoadFromDirectory(dataDirectory, fileIO);
00100:             stNew.RestoreState(stState);
00101:             Check(!stNew.IsQuestAvailable(questId, 5) && stNew.IsQuestAvailable(questId, 10), "Settlement quest state roundtrips accurately");
00102:
00103:             // ── 4. Travel Encounters & Chained Events ───────────────────────
00104:             var encCatalog = TravelEncounterCatalog.LoadFromDirectory(dataDirectory, fileIO);
00105:             Check(encCatalog.Count >= 28, $"Travel encounter count >= 28 (got {encCatalog.Count})");
00106:
00107:             var encSystem = new TravelEncounterSystem(encCatalog);
00108:             var rng = new SeededRng(42);
00109:
00110:             // Deterministic encounter selection
00111:             var selectedEnc = encSystem.SelectEncounter("the_toll", 2.0f, "Balanced", "all", 1, rng);
00112:             Check(selectedEnc != null, "Deterministic encounter selection returned an encounter");
00113:
00114:             // Stance weight differentiation
00115:             if (encCatalog.TryGetEncounter("enc_travel_bristleback_charge", out var boarEnc))
00116:             {
00117:                 float cautiousWeight = encSystem.GetEffectiveWeight(boarEnc, "Cautious");
00118:                 float aggressiveWeight = encSystem.GetEffectiveWeight(boarEnc, "Aggressive");
00119:                 Check(aggressiveWeight > cautiousWeight, "Aggressive stance increases boar encounter weight");
00120:             }
00121:
00122:             // Chain progression
00123:             string chainId = "chain_wandering_pilgrim";
00124:             Check(encSystem.GetChainStage(chainId) == 0, "Chain initial stage == 0");
00125:             encSystem.ResolveChoice("enc_chain_pilgrim_stage1", "choice_give_wood_and_water", 1, out int mDelta, out int gDelta, out string unlockedId);
00126:             Check(encSystem.GetChainStage(chainId) == 2, "Chain stage advanced to 2 after resolving choice");
00127:             Check(mDelta == 4, "Morale delta == +4 on kind pilgrim choice");
00128:
00129:             // Field guide unlock from encounter choice
00130:             encSystem.ResolveChoice("enc_travel_wolf_pack_crossing", "choice_throw_flare", 1, out _, out _, out string fgUnlock);
00131:             Check(fgUnlock == "field_fauna_two_headed_wolf", "Encounter choice unlocked two-headed wolf field guide entry");
00132:
00133:             // Save/load state of encounter system
00134:             var encState = encSystem.CaptureState();
00135:             var encSysNew = new TravelEncounterSystem(encCatalog);
00136:             encSysNew.RestoreState(encState);
00137:             Check(encSysNew.GetChainStage(chainId) == 2, "Encounter chain state restored accurately");
00138:
00139:             GD.Print($"[Plan20Summary] total assertions: {totalAssertions}, failures: {failures}");
00140:             return failures == 0 ? 0 : 1;
00141:         }
00142:     }
00143: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/World/SettlementCatalogTests.cs`

### `Ashfall.Core.Tests/World/SettlementCatalogTests.cs` — complete current file

- Size: 268 lines / 12374 bytes.
- SHA-256: `384153915207250d1af28a55243b54d7cc9a5e494f9410fe9e62e83db4cbfa5b`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using System.Text.Json;
00007: using Ashfall.Core;
00008: using Ashfall.Core.World;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests.World
00012: {
00013:     public class SettlementCatalogTests
00014:     {
00015:         private static string ResolveDataDir()
00016:         {
00017:             string baseDir = AppContext.BaseDirectory;
00018:             string probe = Path.Combine(baseDir, "StreamingAssets", "Data");
00019:             if (Directory.Exists(probe)) return probe;
00020:
00021:             probe = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
00022:             if (Directory.Exists(probe)) return Path.GetFullPath(probe);
00023:
00024:             probe = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
00025:             if (Directory.Exists(probe)) return Path.GetFullPath(probe);
00026:
00027:             return string.Empty;
00028:         }
00029:
00030:         [Fact]
00031:         public void SettlementCatalog_LoadsAllTwelveAuthoredSettlements()
00032:         {
00033:             string dataDir = ResolveDataDir();
00034:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00035:
00036:             Assert.NotNull(catalog);
00037:             Assert.Equal(12, catalog.SettlementCount);
00038:
00039:             var expectedIds = new[]
00040:             {
00041:                 "settlement_tinkers_notch",
00042:                 "settlement_ferry_crossing",
00043:                 "settlement_nine_rails",
00044:                 "settlement_iron_siding",
00045:                 "settlement_fort_karkov",
00046:                 "settlement_lock_seven",
00047:                 "settlement_brine_pans",
00048:                 "settlement_silo_burrow",
00049:                 "settlement_slate_hollow",
00050:                 "settlement_pilgrim_hearth",
00051:                 "settlement_cape_beacon",
00052:                 "settlement_st_nicholas"
00053:             };
00054:
00055:             foreach (var id in expectedIds)
00056:             {
00057:                 Assert.True(catalog.TryGetSettlement(id, out var settlement), $"Settlement '{id}' should exist in catalog.");
00058:                 Assert.NotNull(settlement);
00059:                 Assert.False(string.IsNullOrWhiteSpace(settlement.DisplayName));
00060:                 Assert.False(string.IsNullOrWhiteSpace(settlement.Description));
00061:                 Assert.False(string.IsNullOrWhiteSpace(settlement.Region));
00062:                 Assert.True(settlement.GetEffectivePopulation() >= 12 && settlement.GetEffectivePopulation() <= 200);
00063:                 Assert.True(settlement.ThreatLevel >= 1 && settlement.ThreatLevel <= 5);
00064:                 Assert.NotEmpty(settlement.GetEffectiveLocationId());
00065:                 Assert.NotEmpty(settlement.GetEffectiveAllegiance());
00066:             }
00067:         }
00068:
00069:         [Fact]
00070:         public void SettlementCatalog_ArchetypeDistribution_ThreePerArchetype()
00071:         {
00072:             string dataDir = ResolveDataDir();
00073:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00074:
00075:             var tradePosts = catalog.Settlements.Where(s => s.Archetype.Contains("Trade") || s.Archetype.Contains("Market")).ToList();
00076:             var strongholds = catalog.Settlements.Where(s => s.Archetype.Contains("Stronghold") || s.Archetype.Contains("Town") || s.Archetype.Contains("Yard")).ToList();
00077:             var refugeeCamps = catalog.Settlements.Where(s => s.Archetype.Contains("Camp") || s.Archetype.Contains("Enclave") || s.Archetype.Contains("Collective")).ToList();
00078:             var religiousCommunities = catalog.Settlements.Where(s => s.Archetype.Contains("Religious") || s.Archetype.Contains("Sanctuary") || s.Archetype.Contains("Commune")).ToList();
00079:
00080:             Assert.Equal(3, tradePosts.Count);
00081:             Assert.Equal(3, strongholds.Count);
00082:             Assert.Equal(3, refugeeCamps.Count);
00083:             Assert.Equal(3, religiousCommunities.Count);
00084:         }
00085:
00086:         [Fact]
00087:         public void SettlementCatalog_AllSettlementIds_UniqueAndPrefixed()
00088:         {
00089:             string dataDir = ResolveDataDir();
00090:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00091:
00092:             var idSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00093:             foreach (var settlement in catalog.Settlements)
00094:             {
00095:                 Assert.StartsWith("settlement_", settlement.Id);
00096:                 Assert.True(idSet.Add(settlement.Id), $"Duplicate settlement ID found: {settlement.Id}");
00097:             }
00098:         }
00099:
00100:         [Fact]
00101:         public void SettlementCatalog_AllLocationLinks_ResolveInLocationsJson()
00102:         {
00103:             string dataDir = ResolveDataDir();
00104:             string locationsPath = Path.Combine(dataDir, "locations.json");
00105:             Assert.True(File.Exists(locationsPath), "locations.json must exist.");
00106:
00107:             using var doc = JsonDocument.Parse(File.ReadAllText(locationsPath));
00108:             var validLocationIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00109:             if (doc.RootElement.TryGetProperty("locations", out var locsElem))
00110:             {
00111:                 foreach (var loc in locsElem.EnumerateArray())
00112:                 {
00113:                     if (loc.TryGetProperty("id", out var idElem))
00114:                     {
00115:                         validLocationIds.Add(idElem.GetString() ?? string.Empty);
00116:                     }
00117:                 }
00118:             }
00119:
00120:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00121:             foreach (var settlement in catalog.Settlements)
00122:             {
00123:                 string locId = settlement.GetEffectiveLocationId();
00124:                 Assert.True(validLocationIds.Contains(locId), $"Location '{locId}' for settlement '{settlement.Id}' does not exist in locations.json.");
00125:             }
00126:         }
00127:
00128:         [Fact]
00129:         public void SettlementCatalog_AllFactionAllegiances_Resolve()
00130:         {
00131:             string dataDir = ResolveDataDir();
00132:             var knownFactions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
00133:             {
00134:                 "faction_the_office", "faction_the_cutters", "faction_the_fleet", "faction_black_flotilla",
00135:                 "faction_archivists", "faction_lamplighters", "faction_quiet_house", "faction_grain_exchange",
00136:                 "faction_sun_seekers", "faction_osteophages", "faction_the_tally", "faction_undertow",
00137:                 "faction_cold_count", "faction_deserter_coalition", "faction_the_provisioned", "faction_long_walk",
00138:                 "faction_scavenger_guild", "faction_iron_raiders", "faction_hydro_barons", "faction_the_tempest",
00139:                 "faction_blank_rows", "faction_the_scale", "faction_the_underwrite", "faction_the_compact",
00140:                 "faction_the_garrison", "faction_unaligned", "none"
00141:             };
00142:
00143:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00144:             foreach (var settlement in catalog.Settlements)
00145:             {
00146:                 string faction = settlement.GetEffectiveAllegiance();
00147:                 Assert.True(knownFactions.Contains(faction), $"Faction '{faction}' for settlement '{settlement.Id}' is not recognized.");
00148:             }
00149:         }
00150:
00151:         [Fact]
00152:         public void SettlementCatalog_AllTradeGoodsAndNeeds_ResolveInItemsJson()
00153:         {
00154:             string dataDir = ResolveDataDir();
00155:             var validItemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00156:             foreach (var itemFile in Directory.EnumerateFiles(dataDir, "*item*.json"))
00157:             {
00158:                 using var doc = JsonDocument.Parse(File.ReadAllText(itemFile));
00159:                 if (doc.RootElement.TryGetProperty("items", out var itemsElem) && itemsElem.ValueKind == JsonValueKind.Array)
00160:                 {
00161:                     foreach (var item in itemsElem.EnumerateArray())
00162:                     {
00163:                         if (item.TryGetProperty("id", out var idElem))
00164:                         {
00165:                             validItemIds.Add(idElem.GetString() ?? string.Empty);
00166:                         }
00167:                     }
00168:                 }
00169:             }
00170:
00171:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00172:             foreach (var settlement in catalog.Settlements)
00173:             {
00174:                 foreach (var good in settlement.TradeGoods)
00175:                 {
00176:                     Assert.True(validItemIds.Contains(good), $"Export good '{good}' in settlement '{settlement.Id}' not found in items.json.");
00177:                 }
00178:                 foreach (var need in settlement.TradeNeeds)
00179:                 {
00180:                     Assert.True(validItemIds.Contains(need), $"Import need '{need}' in settlement '{settlement.Id}' not found in items.json.");
00181:                 }
00182:             }
00183:         }
00184:
00185:         [Fact]
00186:         public void SettlementCatalog_TradeGoodsAndNeeds_HaveNoContradictoryOverlaps()
00187:         {
00188:             string dataDir = ResolveDataDir();
00189:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00190:
00191:             foreach (var settlement in catalog.Settlements)
00192:             {
00193:                 var exports = new HashSet<string>(settlement.TradeGoods, StringComparer.OrdinalIgnoreCase);
00194:                 foreach (var need in settlement.TradeNeeds)
00195:                 {
00196:                     Assert.False(exports.Contains(need), $"Settlement '{settlement.Id}' exports and imports the same item '{need}'.");
00197:                 }
00198:             }
00199:         }
00200:
00201:         [Fact]
00202:         public void SettlementCatalog_CaravanIntegration_FourCaravanRoutesIncludeSettlements()
00203:         {
00204:             string dataDir = ResolveDataDir();
00205:             string caravansPath = Path.Combine(dataDir, "caravans.json");
00206:             Assert.True(File.Exists(caravansPath), "caravans.json must exist.");
00207:
00208:             using var doc = JsonDocument.Parse(File.ReadAllText(caravansPath));
00209:             var caravanElem = doc.RootElement.GetProperty("caravans");
00210:             Assert.True(caravanElem.GetArrayLength() >= 4);
00211:
00212:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00213:             var settlementNodes = catalog.Settlements.Select(s => s.RouteNode)
00214:                 .Concat(catalog.Settlements.Select(s => s.GetEffectiveLocationId()))
00215:                 .Where(n => !string.IsNullOrEmpty(n))
00216:                 .ToHashSet(StringComparer.OrdinalIgnoreCase);
00217:
00218:             int caravansWithSettlements = 0;
00219:             foreach (var caravan in caravanElem.EnumerateArray())
00220:             {
00221:                 if (caravan.TryGetProperty("route_node_ids", out var nodesElem))
00222:                 {
00223:                     bool hasSettlement = false;
00224:                     foreach (var node in nodesElem.EnumerateArray())
00225:                     {
00226:                         string nodeId = node.GetString() ?? string.Empty;
00227:                         if (settlementNodes.Contains(nodeId))
00228:                         {
00229:                             hasSettlement = true;
00230:                             break;
00231:                         }
00232:                     }
00233:                     if (hasSettlement) caravansWithSettlements++;
00234:                 }
00235:             }
00236:
00237:             // All 4 caravans route through at least one settlement node
00238:             Assert.Equal(4, caravansWithSettlements);
00239:         }
00240:
00241:         [Fact]
00242:         public void SettlementCatalog_ExpeditionIntegration_ThreeFriendlyStopsExist()
00243:         {
00244:             string dataDir = ResolveDataDir();
00245:             string expeditionsPath = Path.Combine(dataDir, "expeditions.json");
00246:             Assert.True(File.Exists(expeditionsPath), "expeditions.json must exist.");
00247:
00248:             using var doc = JsonDocument.Parse(File.ReadAllText(expeditionsPath));
00249:             var expElem = doc.RootElement.GetProperty("expeditions");
00250:
00251:             var settlementExpDestinations = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00252:             foreach (var exp in expElem.EnumerateArray())
00253:             {
00254:                 if (exp.TryGetProperty("id", out var idElem))
00255:                 {
00256:                     string id = idElem.GetString() ?? string.Empty;
00257:                     if (id.StartsWith("loc_settlement_"))
00258:                     {
00259:                         settlementExpDestinations.Add(id);
00260:                     }
00261:                 }
00262:             }
00263:
00264:             // At least 3 settlement expedition stops exist
00265:             Assert.True(settlementExpDestinations.Count >= 3, $"Expected at least 3 settlement expedition stops, found {settlementExpDestinations.Count}.");
00266:         }
00267:     }
00268: }
```


# Appendix — Focused Current Evidence: `Ashfall.Core.Tests/World/SettlementCatalogTests.cs`

### `Ashfall.Core.Tests/World/SettlementCatalogTests.cs` — complete current file

- Size: 268 lines / 12374 bytes.
- SHA-256: `384153915207250d1af28a55243b54d7cc9a5e494f9410fe9e62e83db4cbfa5b`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using System.Text.Json;
00007: using Ashfall.Core;
00008: using Ashfall.Core.World;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests.World
00012: {
00013:     public class SettlementCatalogTests
00014:     {
00015:         private static string ResolveDataDir()
00016:         {
00017:             string baseDir = AppContext.BaseDirectory;
00018:             string probe = Path.Combine(baseDir, "StreamingAssets", "Data");
00019:             if (Directory.Exists(probe)) return probe;
00020:
00021:             probe = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
00022:             if (Directory.Exists(probe)) return Path.GetFullPath(probe);
00023:
00024:             probe = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
00025:             if (Directory.Exists(probe)) return Path.GetFullPath(probe);
00026:
00027:             return string.Empty;
00028:         }
00029:
00030:         [Fact]
00031:         public void SettlementCatalog_LoadsAllTwelveAuthoredSettlements()
00032:         {
00033:             string dataDir = ResolveDataDir();
00034:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00035:
00036:             Assert.NotNull(catalog);
00037:             Assert.Equal(12, catalog.SettlementCount);
00038:
00039:             var expectedIds = new[]
00040:             {
00041:                 "settlement_tinkers_notch",
00042:                 "settlement_ferry_crossing",
00043:                 "settlement_nine_rails",
00044:                 "settlement_iron_siding",
00045:                 "settlement_fort_karkov",
00046:                 "settlement_lock_seven",
00047:                 "settlement_brine_pans",
00048:                 "settlement_silo_burrow",
00049:                 "settlement_slate_hollow",
00050:                 "settlement_pilgrim_hearth",
00051:                 "settlement_cape_beacon",
00052:                 "settlement_st_nicholas"
00053:             };
00054:
00055:             foreach (var id in expectedIds)
00056:             {
00057:                 Assert.True(catalog.TryGetSettlement(id, out var settlement), $"Settlement '{id}' should exist in catalog.");
00058:                 Assert.NotNull(settlement);
00059:                 Assert.False(string.IsNullOrWhiteSpace(settlement.DisplayName));
00060:                 Assert.False(string.IsNullOrWhiteSpace(settlement.Description));
00061:                 Assert.False(string.IsNullOrWhiteSpace(settlement.Region));
00062:                 Assert.True(settlement.GetEffectivePopulation() >= 12 && settlement.GetEffectivePopulation() <= 200);
00063:                 Assert.True(settlement.ThreatLevel >= 1 && settlement.ThreatLevel <= 5);
00064:                 Assert.NotEmpty(settlement.GetEffectiveLocationId());
00065:                 Assert.NotEmpty(settlement.GetEffectiveAllegiance());
00066:             }
00067:         }
00068:
00069:         [Fact]
00070:         public void SettlementCatalog_ArchetypeDistribution_ThreePerArchetype()
00071:         {
00072:             string dataDir = ResolveDataDir();
00073:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00074:
00075:             var tradePosts = catalog.Settlements.Where(s => s.Archetype.Contains("Trade") || s.Archetype.Contains("Market")).ToList();
00076:             var strongholds = catalog.Settlements.Where(s => s.Archetype.Contains("Stronghold") || s.Archetype.Contains("Town") || s.Archetype.Contains("Yard")).ToList();
00077:             var refugeeCamps = catalog.Settlements.Where(s => s.Archetype.Contains("Camp") || s.Archetype.Contains("Enclave") || s.Archetype.Contains("Collective")).ToList();
00078:             var religiousCommunities = catalog.Settlements.Where(s => s.Archetype.Contains("Religious") || s.Archetype.Contains("Sanctuary") || s.Archetype.Contains("Commune")).ToList();
00079:
00080:             Assert.Equal(3, tradePosts.Count);
00081:             Assert.Equal(3, strongholds.Count);
00082:             Assert.Equal(3, refugeeCamps.Count);
00083:             Assert.Equal(3, religiousCommunities.Count);
00084:         }
00085:
00086:         [Fact]
00087:         public void SettlementCatalog_AllSettlementIds_UniqueAndPrefixed()
00088:         {
00089:             string dataDir = ResolveDataDir();
00090:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00091:
00092:             var idSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00093:             foreach (var settlement in catalog.Settlements)
00094:             {
00095:                 Assert.StartsWith("settlement_", settlement.Id);
00096:                 Assert.True(idSet.Add(settlement.Id), $"Duplicate settlement ID found: {settlement.Id}");
00097:             }
00098:         }
00099:
00100:         [Fact]
00101:         public void SettlementCatalog_AllLocationLinks_ResolveInLocationsJson()
00102:         {
00103:             string dataDir = ResolveDataDir();
00104:             string locationsPath = Path.Combine(dataDir, "locations.json");
00105:             Assert.True(File.Exists(locationsPath), "locations.json must exist.");
00106:
00107:             using var doc = JsonDocument.Parse(File.ReadAllText(locationsPath));
00108:             var validLocationIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00109:             if (doc.RootElement.TryGetProperty("locations", out var locsElem))
00110:             {
00111:                 foreach (var loc in locsElem.EnumerateArray())
00112:                 {
00113:                     if (loc.TryGetProperty("id", out var idElem))
00114:                     {
00115:                         validLocationIds.Add(idElem.GetString() ?? string.Empty);
00116:                     }
00117:                 }
00118:             }
00119:
00120:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00121:             foreach (var settlement in catalog.Settlements)
00122:             {
00123:                 string locId = settlement.GetEffectiveLocationId();
00124:                 Assert.True(validLocationIds.Contains(locId), $"Location '{locId}' for settlement '{settlement.Id}' does not exist in locations.json.");
00125:             }
00126:         }
00127:
00128:         [Fact]
00129:         public void SettlementCatalog_AllFactionAllegiances_Resolve()
00130:         {
00131:             string dataDir = ResolveDataDir();
00132:             var knownFactions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
00133:             {
00134:                 "faction_the_office", "faction_the_cutters", "faction_the_fleet", "faction_black_flotilla",
00135:                 "faction_archivists", "faction_lamplighters", "faction_quiet_house", "faction_grain_exchange",
00136:                 "faction_sun_seekers", "faction_osteophages", "faction_the_tally", "faction_undertow",
00137:                 "faction_cold_count", "faction_deserter_coalition", "faction_the_provisioned", "faction_long_walk",
00138:                 "faction_scavenger_guild", "faction_iron_raiders", "faction_hydro_barons", "faction_the_tempest",
00139:                 "faction_blank_rows", "faction_the_scale", "faction_the_underwrite", "faction_the_compact",
00140:                 "faction_the_garrison", "faction_unaligned", "none"
00141:             };
00142:
00143:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00144:             foreach (var settlement in catalog.Settlements)
00145:             {
00146:                 string faction = settlement.GetEffectiveAllegiance();
00147:                 Assert.True(knownFactions.Contains(faction), $"Faction '{faction}' for settlement '{settlement.Id}' is not recognized.");
00148:             }
00149:         }
00150:
00151:         [Fact]
00152:         public void SettlementCatalog_AllTradeGoodsAndNeeds_ResolveInItemsJson()
00153:         {
00154:             string dataDir = ResolveDataDir();
00155:             var validItemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00156:             foreach (var itemFile in Directory.EnumerateFiles(dataDir, "*item*.json"))
00157:             {
00158:                 using var doc = JsonDocument.Parse(File.ReadAllText(itemFile));
00159:                 if (doc.RootElement.TryGetProperty("items", out var itemsElem) && itemsElem.ValueKind == JsonValueKind.Array)
00160:                 {
00161:                     foreach (var item in itemsElem.EnumerateArray())
00162:                     {
00163:                         if (item.TryGetProperty("id", out var idElem))
00164:                         {
00165:                             validItemIds.Add(idElem.GetString() ?? string.Empty);
00166:                         }
00167:                     }
00168:                 }
00169:             }
00170:
00171:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00172:             foreach (var settlement in catalog.Settlements)
00173:             {
00174:                 foreach (var good in settlement.TradeGoods)
00175:                 {
00176:                     Assert.True(validItemIds.Contains(good), $"Export good '{good}' in settlement '{settlement.Id}' not found in items.json.");
00177:                 }
00178:                 foreach (var need in settlement.TradeNeeds)
00179:                 {
00180:                     Assert.True(validItemIds.Contains(need), $"Import need '{need}' in settlement '{settlement.Id}' not found in items.json.");
00181:                 }
00182:             }
00183:         }
00184:
00185:         [Fact]
00186:         public void SettlementCatalog_TradeGoodsAndNeeds_HaveNoContradictoryOverlaps()
00187:         {
00188:             string dataDir = ResolveDataDir();
00189:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00190:
00191:             foreach (var settlement in catalog.Settlements)
00192:             {
00193:                 var exports = new HashSet<string>(settlement.TradeGoods, StringComparer.OrdinalIgnoreCase);
00194:                 foreach (var need in settlement.TradeNeeds)
00195:                 {
00196:                     Assert.False(exports.Contains(need), $"Settlement '{settlement.Id}' exports and imports the same item '{need}'.");
00197:                 }
00198:             }
00199:         }
00200:
00201:         [Fact]
00202:         public void SettlementCatalog_CaravanIntegration_FourCaravanRoutesIncludeSettlements()
00203:         {
00204:             string dataDir = ResolveDataDir();
00205:             string caravansPath = Path.Combine(dataDir, "caravans.json");
00206:             Assert.True(File.Exists(caravansPath), "caravans.json must exist.");
00207:
00208:             using var doc = JsonDocument.Parse(File.ReadAllText(caravansPath));
00209:             var caravanElem = doc.RootElement.GetProperty("caravans");
00210:             Assert.True(caravanElem.GetArrayLength() >= 4);
00211:
00212:             var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00213:             var settlementNodes = catalog.Settlements.Select(s => s.RouteNode)
00214:                 .Concat(catalog.Settlements.Select(s => s.GetEffectiveLocationId()))
00215:                 .Where(n => !string.IsNullOrEmpty(n))
00216:                 .ToHashSet(StringComparer.OrdinalIgnoreCase);
00217:
00218:             int caravansWithSettlements = 0;
00219:             foreach (var caravan in caravanElem.EnumerateArray())
00220:             {
00221:                 if (caravan.TryGetProperty("route_node_ids", out var nodesElem))
00222:                 {
00223:                     bool hasSettlement = false;
00224:                     foreach (var node in nodesElem.EnumerateArray())
00225:                     {
00226:                         string nodeId = node.GetString() ?? string.Empty;
00227:                         if (settlementNodes.Contains(nodeId))
00228:                         {
00229:                             hasSettlement = true;
00230:                             break;
00231:                         }
00232:                     }
00233:                     if (hasSettlement) caravansWithSettlements++;
00234:                 }
00235:             }
00236:
00237:             // All 4 caravans route through at least one settlement node
00238:             Assert.Equal(4, caravansWithSettlements);
00239:         }
00240:
00241:         [Fact]
00242:         public void SettlementCatalog_ExpeditionIntegration_ThreeFriendlyStopsExist()
00243:         {
00244:             string dataDir = ResolveDataDir();
00245:             string expeditionsPath = Path.Combine(dataDir, "expeditions.json");
00246:             Assert.True(File.Exists(expeditionsPath), "expeditions.json must exist.");
00247:
00248:             using var doc = JsonDocument.Parse(File.ReadAllText(expeditionsPath));
00249:             var expElem = doc.RootElement.GetProperty("expeditions");
00250:
00251:             var settlementExpDestinations = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00252:             foreach (var exp in expElem.EnumerateArray())
00253:             {
00254:                 if (exp.TryGetProperty("id", out var idElem))
00255:                 {
00256:                     string id = idElem.GetString() ?? string.Empty;
00257:                     if (id.StartsWith("loc_settlement_"))
00258:                     {
00259:                         settlementExpDestinations.Add(id);
00260:                     }
00261:                 }
00262:             }
00263:
00264:             // At least 3 settlement expedition stops exist
00265:             Assert.True(settlementExpDestinations.Count >= 3, $"Expected at least 3 settlement expedition stops, found {settlementExpDestinations.Count}.");
00266:         }
00267:     }
00268: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **Current evidence and safe integration boundary for Plan 43: Settlement Gazetteer, Allegiance and Living Community Reachability.**.

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
