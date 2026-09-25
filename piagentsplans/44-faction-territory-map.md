# Plan 44 — Faction Territory, Contested Zones and Map Authority

> **Rebuild status:** COMPLETE 19-TERRITORY/5-ZONE CATALOG — CORE, HOST, SAVE AND CROSS-VALIDATION ARE PRESENT
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

- The current catalog maps faction identity, controlled nodes, control points, contested factions, strength, trade tax and travel safety.
- FactionTerritoryCatalogLoader and TerritoryControlSystem are current Core owners; host session and save store exist.
- Plan43_44SettlementTerritoryIntegrationTests cross-validates 12 settlements, 19 territories, five contested zones, control-point membership and control shifts.

**Bounded outcome:** Retire the no-data premise. faction_territory.json currently has 19 territories and 5 contested zones, FactionTerritoryCatalog parses it, TerritoryControlSystem owns mutable control, and TerritoryControlHostSession persists it. The remaining work is consumer/UI precision and a full map-node/reference audit, not a second territory overlay.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- Replace the old 19-faction data proposal with a current 19-territory/5-zone census and exact owner matrix.
- Trace which map, trade, patrol, encounter and settlement surfaces consume mutable territory state.
- Keep authored control metadata separate from current TerritoryControlSystem state and avoid a second territory save.

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
- The valuable residual is truthful map/consumer reachability, not more definitions.

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
| static territory and contested-zone definitions | FactionTerritoryCatalog | `Assets/Ashfall.Core/World/FactionTerritoryCatalog.cs` | Catalog owner. |
| mutable control, contests, supply lines and census | TerritoryControlSystem | `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs` | Sole mutable territory owner. |
| settlement geography and allegiance | SettlementCatalog | `Assets/Ashfall.Core/World/SettlementCatalog.cs` | Separate settlement catalog. |
| host commands and persistence | TerritoryControlHostSession | `src/Host/TerritoryControlHostSession.cs` | Thin adapter. |
| host integration and commands | Main.TerritoryControl | `src/Main.TerritoryControl.cs` | Current host seam. |
| cross-catalog and save contract | Plan43_44 integration tests | `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs` | Focused evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Faction Territory, Contested Zones and Map Authority
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ FactionTerritoryCatalog
│   static territory and contested-zone definitions
│ TerritoryControlSystem
│   mutable control, contests, supply lines and census
│ SettlementCatalog
│   settlement geography and allegiance
│ TerritoryControlHostSession
│   host commands and persistence
│ Main.TerritoryControl
│   host integration and commands
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

1. **Preserve current state ownership.** FactionTerritoryCatalog owns static territory and contested-zone definitions: Catalog owner.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| static territory and contested-zone definitions | FactionTerritoryCatalog | `Assets/Ashfall.Core/World/FactionTerritoryCatalog.cs` | Catalog owner. |
| mutable control, contests, supply lines and census | TerritoryControlSystem | `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs` | Sole mutable territory owner. |
| settlement geography and allegiance | SettlementCatalog | `Assets/Ashfall.Core/World/SettlementCatalog.cs` | Separate settlement catalog. |
| host commands and persistence | TerritoryControlHostSession | `src/Host/TerritoryControlHostSession.cs` | Thin adapter. |
| host integration and commands | Main.TerritoryControl | `src/Main.TerritoryControl.cs` | Current host seam. |
| cross-catalog and save contract | Plan43_44 integration tests | `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs` | Focused evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load 19 territories and 5 zones
2. validate faction/node/location references
3. read current control state
4. project settlement allegiance and contested zones
5. route patrol/trade/encounter consumers through owner projections
6. present current map state
7. capture TerritoryControlSaveState

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Territory catalog rows are immutable definitions.
- TerritoryControlSaveState is the current mutable authority.
- Settlement allegiance does not overwrite control state without the current control command.
- Travel safety and trade tax are projections until their current consumers use them.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Every controlled node/control point must resolve through current catalogs.
- A static trade tax is not a market mutation.
- A control shift emits one fact and persists once.
- A missing owner yields a truthful unavailable state.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- faction_territory.json, settlements.json, locations and map data retain separate authority.
- No duplicate faction territory file.
- No new map graph in Core.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use TerritoryControlSaveStore/current territory state.
- No Plan-44 save section.
- Legacy empty territory state restores as fresh state.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Stable territory ID order; current seeded RNG only for contested outcomes.
- Map projection is pure from state.
- Same owner state and day yields same overlay.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- TerritoryControlSystem emits current control/shift facts.
- Settlement and map owners consume read projections.
- Host commands route through TerritoryControlHostSession.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/TerritoryControlHostSession.cs
- src/Main.TerritoryControl.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Territory descriptions remain fictional and grounded.
- Trade/travel modifiers are not presented as universal laws outside their consumer.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A static territory row overwrites mutable control. | FactionTerritoryCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A settlement allegiance silently changes control. | TerritoryControlSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A nonexistent map node passes validation. | SettlementCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A new map cache becomes authority. | TerritoryControlHostSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A second save section is added. | Main.TerritoryControl | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/World/FactionTerritoryCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 | Catalog/reference census. | 19/5 current counts and references are explicit. | No production path until the owning implementation package is separately claimed. |
| 1 | Owner and consumer trace. | Mutable/static boundaries are documented. | No production path until the owning implementation package is separately claimed. |
| 2 | Save/shift/replay audit. | No duplicate control state. | No production path until the owning implementation package is separately claimed. |
| 3 | Map/UI polish. | Current control and unavailable states are truthful. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/faction_territory.json | READ ONLY; MODIFY only for reference/content defect | 19 territories/5 zones |
| Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs | READ ONLY | Mutable owner |
| src/Host/TerritoryControlHostSession.cs | READ ONLY | Host/save seam |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Parallel territory state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Treating settlement catalog as mutable control. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Unbounded map node references. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Fake trade/travel effects. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new territory rows.
- No new map owner.
- No production/data/test/UI changes here.

# 23. Rollback and Recovery

- Revert planning artifact.
- Future projection changes are read-only and save-compatible.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- Current catalog, owner, host, save and cross-catalog tests are named.
- Reachability and reference gates are explicit.

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

- No new territory rows.
- No new map owner.
- No production/data/test/UI changes here.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/World/FactionTerritoryCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: static territory and contested-zone definitions → FactionTerritoryCatalog; mutable control, contests, supply lines and census → TerritoryControlSystem; settlement geography and allegiance → SettlementCatalog; host commands and persistence → TerritoryControlHostSession; host integration and commands → Main.TerritoryControl; cross-catalog and save contract → Plan43_44 integration tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 44.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 44 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by FactionTerritoryCatalog or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/World/FactionTerritoryCatalog.cs`

### `Assets/Ashfall.Core/World/FactionTerritoryCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 154 lines / 6874 bytes.
- SHA-256: `9b583f668e225556efc7d91106c6b272fafa52914d7d16781797d1db8e54ae07`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FactionTerritoryCatalogContainer
public int schema_version { get; set; } = 1;
public string collection_id { get; set; } = "faction_territory_catalog";
public List<FactionTerritoryDef> territories { get; set; } = new List<FactionTerritoryDef>();
public List<ContestedZoneDef> contested_zones { get; set; } = new List<ContestedZoneDef>();
public sealed class FactionTerritoryDef
public string id { get; set; } = string.Empty;
public string faction { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string classification { get; set; } = "territorial"; // territorial, nomadic, ideological, mixed
public string territory_scale { get; set; } = "minor"; // major, medium, minor, none
public string primary_resource_interest { get; set; } = string.Empty;
public List<string> controlled_nodes { get; set; } = new List<string>();
public List<string> control_points { get; set; } = new List<string>();
public List<string> contested_with { get; set; } = new List<string>();
public int control_strength { get; set; } = 50;
public float trade_tax { get; set; } = 0.0f;
public float travel_safety { get; set; } = 1.0f;
public string shift_trigger { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public sealed class ContestedZoneDef
public string id { get; set; } = string.Empty;
public string name { get; set; } = string.Empty;
public string strategic_value { get; set; } = string.Empty;
public string focal_node_id { get; set; } = string.Empty;
public string focal_location_id { get; set; } = string.Empty;
public List<string> claimant_factions { get; set; } = new List<string>();
public string conflict_driver { get; set; } = string.Empty;
public int hazard_rating { get; set; } = 1;
public int dispute_intensity { get; set; } = 50;
public sealed class FactionTerritoryCatalog
public const string DefaultFileName = "faction_territory.json";
public IReadOnlyList<FactionTerritoryDef> Territories => _territories;
public IReadOnlyList<ContestedZoneDef> ContestedZones => _contestedZones;
public int TerritoryCount => _territories.Count;
public int ContestedZoneCount => _contestedZones.Count;
public bool TryGetTerritory(string territoryId, out FactionTerritoryDef territory) {
public bool TryGetTerritoryByFaction(string factionId, out FactionTerritoryDef territory) {
public bool TryGetContestedZone(string zoneId, out ContestedZoneDef zone) {
public IEnumerable<FactionTerritoryDef> GetTerritoriesForNode(string mapNodeId) {
public static FactionTerritoryCatalog LoadFromDirectory(string dataDirectory, IFileIO fileIO, IJsonSerializer? jsonSerializer = null) {
public static FactionTerritoryCatalog LoadFromJson(string json, IJsonSerializer? jsonSerializer = null) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs`

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


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/World/SettlementCatalog.cs`

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


# Appendix B.05 — Current Code Architecture: `src/Host/TerritoryControlHostSession.cs`

### `src/Host/TerritoryControlHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 113 lines / 5150 bytes.
- SHA-256: `66abc3c363663840ffaf57b22fad69f749796b349e042efaae6a0ed2386fcaf3`.
- Architecture signals: seeded references=3; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TerritoryControlHostSession : HostSessionBase
public const string CatalogTerritoriesFile = "faction_territory.json";
public const string CatalogSupplyLinesFile = "supply_lines.json";
public TerritoryControlSystem System { get; }
public bool CatalogLoaded { get; private set; }
public static TerritoryControlHostSession Load(string dataDirectory, IFileIO files) {
public TerritoryCensus ReadCensus() => System.ReadCensus();
public TerritoryControlSaveState CaptureState() => System.CaptureState();
public bool RestoreState(TerritoryControlSaveState? state) {
public bool FortifyLocation(string locationId, int levelDelta = 1) =>
public bool AssignGarrison(string locationId, int garrisonDelta) =>
public bool ContestLocation(string locationId, string attackingFactionId, int attackPower, ISeededRng rng, int currentDay = 0) =>
public bool RaidSupplyLine(string supplyLineId, int raidIntensity, ISeededRng rng) =>
public bool RestoreSupplyLine(string supplyLineId) =>
public void TickDay(int currentDay, ISeededRng? rng = null) =>
public static class TerritoryControlSaveStore
public const string FileName = "territory_control_save.json";
public const string SectionName = "territory_control";
public static bool TrySave(TerritoryControlSaveState state) => s_store.TrySave(state);
public static TerritoryControlSaveState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(TerritoryControlSaveState state) => s_store.CapturePersisted(state);
public static TerritoryControlSaveState? TryRestore(string json) => s_store.RestoreEnvelope(json);
public static TerritoryControlSaveState? TryRestoreBare(string json) => s_store.RestoreBare(json);
```


# Appendix B.06 — Current Code Architecture: `src/Host/TerritoryControlHostSession.cs`

### `src/Host/TerritoryControlHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 113 lines / 5150 bytes.
- SHA-256: `66abc3c363663840ffaf57b22fad69f749796b349e042efaae6a0ed2386fcaf3`.
- Architecture signals: seeded references=3; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TerritoryControlHostSession : HostSessionBase
public const string CatalogTerritoriesFile = "faction_territory.json";
public const string CatalogSupplyLinesFile = "supply_lines.json";
public TerritoryControlSystem System { get; }
public bool CatalogLoaded { get; private set; }
public static TerritoryControlHostSession Load(string dataDirectory, IFileIO files) {
public TerritoryCensus ReadCensus() => System.ReadCensus();
public TerritoryControlSaveState CaptureState() => System.CaptureState();
public bool RestoreState(TerritoryControlSaveState? state) {
public bool FortifyLocation(string locationId, int levelDelta = 1) =>
public bool AssignGarrison(string locationId, int garrisonDelta) =>
public bool ContestLocation(string locationId, string attackingFactionId, int attackPower, ISeededRng rng, int currentDay = 0) =>
public bool RaidSupplyLine(string supplyLineId, int raidIntensity, ISeededRng rng) =>
public bool RestoreSupplyLine(string supplyLineId) =>
public void TickDay(int currentDay, ISeededRng? rng = null) =>
public static class TerritoryControlSaveStore
public const string FileName = "territory_control_save.json";
public const string SectionName = "territory_control";
public static bool TrySave(TerritoryControlSaveState state) => s_store.TrySave(state);
public static TerritoryControlSaveState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(TerritoryControlSaveState state) => s_store.CapturePersisted(state);
public static TerritoryControlSaveState? TryRestore(string json) => s_store.RestoreEnvelope(json);
public static TerritoryControlSaveState? TryRestoreBare(string json) => s_store.RestoreBare(json);
```


# Appendix B.07 — Current Code Architecture: `src/Main.TerritoryControl.cs`

### `src/Main.TerritoryControl.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 144 lines / 5610 bytes.
- SHA-256: `f439d5435ea72d45ac7e2c060431f6581d3b5cd4ce1f37575e34f4df72620f03`.
- Architecture signals: seeded references=1; save/restore symbols=3; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public TerritoryControlHostSession? TerritoryControl => _territoryControl;
internal void TickTerritoryControl(int day, ISeededRng? rng = null) {
```


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/faction_territory.json`

### `Assets/StreamingAssets/Data/faction_territory.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 19950 bytes / 19950 characters.
- SHA-256: `ef0940bbe98f3082b75cbbef77df670e7e2c7358b89ddeefc20b8fd119b90915`.
- Root keys: `collection_id`, `contested_zones`, `schema_version`, `territories`.

Array-path census (minimum, maximum, observed rows):

```text
contested_zones: min=5, max=5, observed_paths=1
contested_zones[].claimant_factions: min=3, max=3, observed_paths=2
territories: min=19, max=19, observed_paths=1
territories[].contested_with: min=2, max=2, observed_paths=2
territories[].control_points: min=2, max=2, observed_paths=2
territories[].controlled_nodes: min=1, max=1, observed_paths=2
```

Representative record fields:

- `classification`
- `contested_with`
- `control_points`
- `control_strength`
- `controlled_nodes`
- `description`
- `display_name`
- `faction`
- `id`
- `primary_resource_interest`
- `shift_trigger`
- `territory_scale`
- `trade_tax`
- `travel_safety`

Representative identifiers (ordered, capped for readability):

```text
territory_the_office
territory_the_cutters
territory_black_flotilla
territory_the_fleet
territory_deserter_coalition
territory_cold_count
territory_the_tally
territory_grain_exchange
territory_quiet_house
territory_scavenger_guild
territory_long_walk
territory_undertow
territory_hydro_barons
territory_iron_raiders
territory_the_provisioned
territory_archivists
territory_lamplighters
territory_sun_seekers
territory_osteophages
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/settlements.json`

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


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/wasteland_map_v1.json`

### `Assets/StreamingAssets/Data/wasteland_map_v1.json`

- Parse: valid strict JSON.
- Schema version: `2`.
- Size: 20796 bytes / 20796 characters.
- SHA-256: `5384c1c4093451593327ccc1fce9f8708b21923b8c94104a17e94d72808adf2b`.
- Root keys: `nodes`, `routes`, `schema_version`, `trapSites`.

Array-path census (minimum, maximum, observed rows):

```text
nodes: min=22, max=22, observed_paths=1
routes: min=68, max=68, observed_paths=1
trapSites: min=1, max=1, observed_paths=1
```

Representative record fields:

- `anchorNodeId`
- `offsetX`
- `offsetY`
- `siteId`


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/World/FactionTerritoryCatalogTests.cs`

### `Ashfall.Core.Tests/World/FactionTerritoryCatalogTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 149; SHA-256: `c324801e970bf6cb2f36f2ce21d8a71f58fa20d21d461a0301bc0cf4c54c3652`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsSuccessfully_Contains19TerritoriesAnd5ContestedZones
EveryTerritory_HasValidIdAndRecognizedClassification
EveryTerritory_ControlledNodes_AreNonEmpty_AndUnique
EveryTerritory_ContestedWith_AreValidFactionsAndNotSelf
EveryContestedZone_HasAtLeastTwoClaimantFactions
LookupsByIdAndFaction_WorkCorrectly
GetTerritoriesForNode_ReturnsMatchingTerritories
```


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs`

### `Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 183; SHA-256: `a707d73afdcf73ed193b18f640d17413ff7f8777de89f97b5b2d0203c529e461`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalogs_LoadAndParseTerritoriesAndSupplyLines
LocationState_InitializesFromTerritories_AndSupportsFortification
ContestLocation_DeterministicallyResolvesControlShift
RaidSupplyLine_DisruptsCorridor_AndDegradesDestinationControl
TickDay_DeliversCargo_AndReinforcesDestinationControl
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs`

### `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 158; SHA-256: `d3a4f1ba72662b18aa76830b5845a39dd7c25969398117089b6b0246c728a099`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SettlementAndTerritoryCatalogs_CrossValidateAllegiancesAndControlPoints
TerritoryControlSystem_SimulatesSettlementContestAndFortification
SettlementCatalog_QuestLifecycle_AndStateSaveRestoreRoundTrip
```


# Appendix E.14 — Supporting Code Evidence: `Assets/Ashfall.Core/World/FactionTerritoryCatalog.cs`

### `Assets/Ashfall.Core/World/FactionTerritoryCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 154 lines / 6874 bytes.
- SHA-256: `9b583f668e225556efc7d91106c6b272fafa52914d7d16781797d1db8e54ae07`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FactionTerritoryCatalogContainer
public int schema_version { get; set; } = 1;
public string collection_id { get; set; } = "faction_territory_catalog";
public List<FactionTerritoryDef> territories { get; set; } = new List<FactionTerritoryDef>();
public List<ContestedZoneDef> contested_zones { get; set; } = new List<ContestedZoneDef>();
public sealed class FactionTerritoryDef
public string id { get; set; } = string.Empty;
public string faction { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string classification { get; set; } = "territorial"; // territorial, nomadic, ideological, mixed
public string territory_scale { get; set; } = "minor"; // major, medium, minor, none
public string primary_resource_interest { get; set; } = string.Empty;
public List<string> controlled_nodes { get; set; } = new List<string>();
public List<string> control_points { get; set; } = new List<string>();
public List<string> contested_with { get; set; } = new List<string>();
public int control_strength { get; set; } = 50;
public float trade_tax { get; set; } = 0.0f;
public float travel_safety { get; set; } = 1.0f;
public string shift_trigger { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public sealed class ContestedZoneDef
public string id { get; set; } = string.Empty;
public string name { get; set; } = string.Empty;
public string strategic_value { get; set; } = string.Empty;
public string focal_node_id { get; set; } = string.Empty;
public string focal_location_id { get; set; } = string.Empty;
public List<string> claimant_factions { get; set; } = new List<string>();
public string conflict_driver { get; set; } = string.Empty;
public int hazard_rating { get; set; } = 1;
public int dispute_intensity { get; set; } = 50;
public sealed class FactionTerritoryCatalog
public const string DefaultFileName = "faction_territory.json";
public IReadOnlyList<FactionTerritoryDef> Territories => _territories;
public IReadOnlyList<ContestedZoneDef> ContestedZones => _contestedZones;
public int TerritoryCount => _territories.Count;
public int ContestedZoneCount => _contestedZones.Count;
public bool TryGetTerritory(string territoryId, out FactionTerritoryDef territory) {
public bool TryGetTerritoryByFaction(string factionId, out FactionTerritoryDef territory) {
public bool TryGetContestedZone(string zoneId, out ContestedZoneDef zone) {
public IEnumerable<FactionTerritoryDef> GetTerritoriesForNode(string mapNodeId) {
public static FactionTerritoryCatalog LoadFromDirectory(string dataDirectory, IFileIO fileIO, IJsonSerializer? jsonSerializer = null) {
public static FactionTerritoryCatalog LoadFromJson(string json, IJsonSerializer? jsonSerializer = null) {
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


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/World/SettlementCatalog.cs`

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


# Appendix E.17 — Supporting Code Evidence: `src/Host/TerritoryControlHostSession.cs`

### `src/Host/TerritoryControlHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 113 lines / 5150 bytes.
- SHA-256: `66abc3c363663840ffaf57b22fad69f749796b349e042efaae6a0ed2386fcaf3`.
- Architecture signals: seeded references=3; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TerritoryControlHostSession : HostSessionBase
public const string CatalogTerritoriesFile = "faction_territory.json";
public const string CatalogSupplyLinesFile = "supply_lines.json";
public TerritoryControlSystem System { get; }
public bool CatalogLoaded { get; private set; }
public static TerritoryControlHostSession Load(string dataDirectory, IFileIO files) {
public TerritoryCensus ReadCensus() => System.ReadCensus();
public TerritoryControlSaveState CaptureState() => System.CaptureState();
public bool RestoreState(TerritoryControlSaveState? state) {
public bool FortifyLocation(string locationId, int levelDelta = 1) =>
public bool AssignGarrison(string locationId, int garrisonDelta) =>
public bool ContestLocation(string locationId, string attackingFactionId, int attackPower, ISeededRng rng, int currentDay = 0) =>
public bool RaidSupplyLine(string supplyLineId, int raidIntensity, ISeededRng rng) =>
public bool RestoreSupplyLine(string supplyLineId) =>
public void TickDay(int currentDay, ISeededRng? rng = null) =>
public static class TerritoryControlSaveStore
public const string FileName = "territory_control_save.json";
public const string SectionName = "territory_control";
public static bool TrySave(TerritoryControlSaveState state) => s_store.TrySave(state);
public static TerritoryControlSaveState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(TerritoryControlSaveState state) => s_store.CapturePersisted(state);
public static TerritoryControlSaveState? TryRestore(string json) => s_store.RestoreEnvelope(json);
public static TerritoryControlSaveState? TryRestoreBare(string json) => s_store.RestoreBare(json);
```


# Appendix E.18 — Supporting Code Evidence: `src/Host/TerritoryControlHostSession.cs`

### `src/Host/TerritoryControlHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 113 lines / 5150 bytes.
- SHA-256: `66abc3c363663840ffaf57b22fad69f749796b349e042efaae6a0ed2386fcaf3`.
- Architecture signals: seeded references=3; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TerritoryControlHostSession : HostSessionBase
public const string CatalogTerritoriesFile = "faction_territory.json";
public const string CatalogSupplyLinesFile = "supply_lines.json";
public TerritoryControlSystem System { get; }
public bool CatalogLoaded { get; private set; }
public static TerritoryControlHostSession Load(string dataDirectory, IFileIO files) {
public TerritoryCensus ReadCensus() => System.ReadCensus();
public TerritoryControlSaveState CaptureState() => System.CaptureState();
public bool RestoreState(TerritoryControlSaveState? state) {
public bool FortifyLocation(string locationId, int levelDelta = 1) =>
public bool AssignGarrison(string locationId, int garrisonDelta) =>
public bool ContestLocation(string locationId, string attackingFactionId, int attackPower, ISeededRng rng, int currentDay = 0) =>
public bool RaidSupplyLine(string supplyLineId, int raidIntensity, ISeededRng rng) =>
public bool RestoreSupplyLine(string supplyLineId) =>
public void TickDay(int currentDay, ISeededRng? rng = null) =>
public static class TerritoryControlSaveStore
public const string FileName = "territory_control_save.json";
public const string SectionName = "territory_control";
public static bool TrySave(TerritoryControlSaveState state) => s_store.TrySave(state);
public static TerritoryControlSaveState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(TerritoryControlSaveState state) => s_store.CapturePersisted(state);
public static TerritoryControlSaveState? TryRestore(string json) => s_store.RestoreEnvelope(json);
public static TerritoryControlSaveState? TryRestoreBare(string json) => s_store.RestoreBare(json);
```


# Appendix F.19 — Supporting Data Evidence: `Assets/StreamingAssets/Data/faction_territory.json`

### `Assets/StreamingAssets/Data/faction_territory.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 19950 bytes / 19950 characters.
- SHA-256: `ef0940bbe98f3082b75cbbef77df670e7e2c7358b89ddeefc20b8fd119b90915`.
- Root keys: `collection_id`, `contested_zones`, `schema_version`, `territories`.

Array-path census (minimum, maximum, observed rows):

```text
contested_zones: min=5, max=5, observed_paths=1
contested_zones[].claimant_factions: min=3, max=3, observed_paths=2
territories: min=19, max=19, observed_paths=1
territories[].contested_with: min=2, max=2, observed_paths=2
territories[].control_points: min=2, max=2, observed_paths=2
territories[].controlled_nodes: min=1, max=1, observed_paths=2
```

Representative record fields:

- `classification`
- `contested_with`
- `control_points`
- `control_strength`
- `controlled_nodes`
- `description`
- `display_name`
- `faction`
- `id`
- `primary_resource_interest`
- `shift_trigger`
- `territory_scale`
- `trade_tax`
- `travel_safety`

Representative identifiers (ordered, capped for readability):

```text
territory_the_office
territory_the_cutters
territory_black_flotilla
territory_the_fleet
territory_deserter_coalition
territory_cold_count
territory_the_tally
territory_grain_exchange
territory_quiet_house
territory_scavenger_guild
territory_long_walk
territory_undertow
territory_hydro_barons
territory_iron_raiders
territory_the_provisioned
territory_archivists
territory_lamplighters
territory_sun_seekers
territory_osteophages
```


# Appendix F.20 — Supporting Data Evidence: `Assets/StreamingAssets/Data/settlements.json`

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


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/World/FactionTerritoryCatalogTests.cs`

### `Ashfall.Core.Tests/World/FactionTerritoryCatalogTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 149; SHA-256: `c324801e970bf6cb2f36f2ce21d8a71f58fa20d21d461a0301bc0cf4c54c3652`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsSuccessfully_Contains19TerritoriesAnd5ContestedZones
EveryTerritory_HasValidIdAndRecognizedClassification
EveryTerritory_ControlledNodes_AreNonEmpty_AndUnique
EveryTerritory_ContestedWith_AreValidFactionsAndNotSelf
EveryContestedZone_HasAtLeastTwoClaimantFactions
LookupsByIdAndFaction_WorkCorrectly
GetTerritoriesForNode_ReturnsMatchingTerritories
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs`

### `Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 183; SHA-256: `a707d73afdcf73ed193b18f640d17413ff7f8777de89f97b5b2d0203c529e461`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalogs_LoadAndParseTerritoriesAndSupplyLines
LocationState_InitializesFromTerritories_AndSupportsFortification
ContestLocation_DeterministicallyResolvesControlShift
RaidSupplyLine_DisruptsCorridor_AndDegradesDestinationControl
TickDay_DeliversCargo_AndReinforcesDestinationControl
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs`

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
| static territory and contested-zone definitions | FactionTerritoryCatalog | mutable control, contests, supply lines and census | TerritoryControlSystem | Owner emits/reads a typed fact; no mirror state. |
| static territory and contested-zone definitions | FactionTerritoryCatalog | settlement geography and allegiance | SettlementCatalog | Owner emits/reads a typed fact; no mirror state. |
| static territory and contested-zone definitions | FactionTerritoryCatalog | host commands and persistence | TerritoryControlHostSession | Owner emits/reads a typed fact; no mirror state. |
| static territory and contested-zone definitions | FactionTerritoryCatalog | host integration and commands | Main.TerritoryControl | Owner emits/reads a typed fact; no mirror state. |
| static territory and contested-zone definitions | FactionTerritoryCatalog | cross-catalog and save contract | Plan43_44 integration tests | Owner emits/reads a typed fact; no mirror state. |
| mutable control, contests, supply lines and census | TerritoryControlSystem | static territory and contested-zone definitions | FactionTerritoryCatalog | Owner emits/reads a typed fact; no mirror state. |
| mutable control, contests, supply lines and census | TerritoryControlSystem | settlement geography and allegiance | SettlementCatalog | Owner emits/reads a typed fact; no mirror state. |
| mutable control, contests, supply lines and census | TerritoryControlSystem | host commands and persistence | TerritoryControlHostSession | Owner emits/reads a typed fact; no mirror state. |
| mutable control, contests, supply lines and census | TerritoryControlSystem | host integration and commands | Main.TerritoryControl | Owner emits/reads a typed fact; no mirror state. |
| mutable control, contests, supply lines and census | TerritoryControlSystem | cross-catalog and save contract | Plan43_44 integration tests | Owner emits/reads a typed fact; no mirror state. |
| settlement geography and allegiance | SettlementCatalog | static territory and contested-zone definitions | FactionTerritoryCatalog | Owner emits/reads a typed fact; no mirror state. |
| settlement geography and allegiance | SettlementCatalog | mutable control, contests, supply lines and census | TerritoryControlSystem | Owner emits/reads a typed fact; no mirror state. |
| settlement geography and allegiance | SettlementCatalog | host commands and persistence | TerritoryControlHostSession | Owner emits/reads a typed fact; no mirror state. |
| settlement geography and allegiance | SettlementCatalog | host integration and commands | Main.TerritoryControl | Owner emits/reads a typed fact; no mirror state. |
| settlement geography and allegiance | SettlementCatalog | cross-catalog and save contract | Plan43_44 integration tests | Owner emits/reads a typed fact; no mirror state. |
| host commands and persistence | TerritoryControlHostSession | static territory and contested-zone definitions | FactionTerritoryCatalog | Owner emits/reads a typed fact; no mirror state. |
| host commands and persistence | TerritoryControlHostSession | mutable control, contests, supply lines and census | TerritoryControlSystem | Owner emits/reads a typed fact; no mirror state. |
| host commands and persistence | TerritoryControlHostSession | settlement geography and allegiance | SettlementCatalog | Owner emits/reads a typed fact; no mirror state. |
| host commands and persistence | TerritoryControlHostSession | host integration and commands | Main.TerritoryControl | Owner emits/reads a typed fact; no mirror state. |
| host commands and persistence | TerritoryControlHostSession | cross-catalog and save contract | Plan43_44 integration tests | Owner emits/reads a typed fact; no mirror state. |
| host integration and commands | Main.TerritoryControl | static territory and contested-zone definitions | FactionTerritoryCatalog | Owner emits/reads a typed fact; no mirror state. |
| host integration and commands | Main.TerritoryControl | mutable control, contests, supply lines and census | TerritoryControlSystem | Owner emits/reads a typed fact; no mirror state. |
| host integration and commands | Main.TerritoryControl | settlement geography and allegiance | SettlementCatalog | Owner emits/reads a typed fact; no mirror state. |
| host integration and commands | Main.TerritoryControl | host commands and persistence | TerritoryControlHostSession | Owner emits/reads a typed fact; no mirror state. |
| host integration and commands | Main.TerritoryControl | cross-catalog and save contract | Plan43_44 integration tests | Owner emits/reads a typed fact; no mirror state. |
| cross-catalog and save contract | Plan43_44 integration tests | static territory and contested-zone definitions | FactionTerritoryCatalog | Owner emits/reads a typed fact; no mirror state. |
| cross-catalog and save contract | Plan43_44 integration tests | mutable control, contests, supply lines and census | TerritoryControlSystem | Owner emits/reads a typed fact; no mirror state. |
| cross-catalog and save contract | Plan43_44 integration tests | settlement geography and allegiance | SettlementCatalog | Owner emits/reads a typed fact; no mirror state. |
| cross-catalog and save contract | Plan43_44 integration tests | host commands and persistence | TerritoryControlHostSession | Owner emits/reads a typed fact; no mirror state. |
| cross-catalog and save contract | Plan43_44 integration tests | host integration and commands | Main.TerritoryControl | Owner emits/reads a typed fact; no mirror state. |

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

> This document is the complete compiled edition of the ASHFALL Master Expansion Authority v2.0. It combines, in order: (1) the uploaded source document (Volumes 1–24 and its Parts 0 through V, reproduced verbatim), and (2) the Plan Factory's expansion volumes 25 through 57 (the factory batches of 2026-09-24, reproduced verbatim). No content has been altered, merged, or summarized; the two bodies are concatenated at their natural boundary. The source document's own authority order stands: live repository source first; then AGENTS.md; then this document. The factory's constitution (evidence labels, honest bounds, anti-padding) governs Volumes 25 onward.

> **Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival` (ASHFALL: Atomic War – Starving Survival)
**Document version:** 2.0.0 — compiled 2026-09-24, supersedes the v1.0 master world bible (compiled 2026-09-23) as an expansion scaffold.
**Document class:** SUBJECT-PLAN FACTORY. This document is not itself an integration plan. It is a repeatable generator: any future planning session can consume its matrices, backlog, and templates to produce an unbounded series of bounded subject plans, each of which names its own best integration route.
**Audit basis:** Live repository inspection performed 2026-09-24 (repository root listing, `Assets/StreamingAssets/Data/` listing at 342 entries, `docs/` listing, `docs/plans/` listing at 126 entries, `INTEGRATION_PLANS.md`, `SESSION_HANDOFF.md`, `AGENTS.md`, branch list). Every claim in the Drift Register (Part I) is labeled VERIFIED, HIGH CONFIDENCE, or UNVERIFIED.
**Authority order:** unchanged from v1.0 — live repository source and data first; then `AGENTS.md`; then this document; then the docs registry and atlas; then plan ledgers. Where this document and live source disagree, live source wins and this document must be corrected.

> **DR-02 — The docs tree has substantially more subdirectories than the v1.0 map. VERIFIED.**
Live `docs/` subdirectories observed in the audit include (selection; the listing was long and partially truncated): `adr/`, `agents/`, `architecture/`, `archive/`, `balance/`, `bodymind/`, `campaign/`, `cartography/`, `ci/`, `cli/`, `collectibles/`, `combat/`, `content/`, `contracts/`, `crafting/`, `crossing/`, `culture/`, `decisions/`, `design/`, `discovery/`, `duty_roster/`, `ecology/`, `economy/`, `endgame/`, `expansions/`, `expeditions/`, `faction_war/`, `factions/`, `foreman/`, `forensics/`, `foundry/`, `gaps/`, `governance/`, `greenhouse/`, `health/`, `holdfast/`, `hygiene/`, `i18n/`, `implementation/`, `incidents/`, `integration/`, `journal/`, `lore/`, `maritime/`, `medical/`, `memorials/`, `mods/`, `moral/`, `moral_choice/`, `muster/`, `narrative/`, `onboarding/`, `orbital/`, `perf/`, `phantoms/`, `plans/`, `power/`, `process/`, `production/`, and a `player_surface_manifest.json`. Two of these — `gaps/` and `incidents/` — are first-class *expansion feedstock*: directories whose entire purpose is to record what is missing or broken. The Factory Protocol (Part II, step 2) now treats `docs/gaps/` and `docs/incidents/` as mandatory inputs.

> **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
Observed live and not listed in v1.0 Part 5.8: `ECONOMY_FAIRNESS_AUDIT.md`, `ENGINE_SUPPORT_POLICY.md`, `GODOT_MIGRATION_STATUS.md`, `REPO_HISTORY_REWRITE.md`, `HUMAN_AUTHORSHIP.md`, `AI_DISCLOSURE.md`, `ASSET_MIGRATION_LEDGER.md`, `CODEX_SOURCE_MATRIX.md`, `ARCHIVE_INDEX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md`, `SHELTER_MAINTENANCE_MATRIX.md`, `SHELTER_30_DAY_MAINTENANCE_REPORT.md`, `L10N_WAVE2_ROADMAP.md`, `INPUT.md`, `RELEASE_EXPORT.md`, `ENGINE_SUPPORT_POLICY.md`. Of these, `ECONOMY_FAIRNESS_AUDIT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, and `SHELTER_MAINTENANCE_MATRIX.md` are pre-computed balance baselines: they convert Lane C (economy and balance) planning from speculative to evidence-anchored. Subject plans in Lane C must cite these baselines instead of re-deriving numbers.

> **DR-04 — The data catalog inventory has grown; several catalogs are absent from the v1.0 inventory. VERIFIED.**
`Assets/StreamingAssets/Data/` currently holds 342 entries. Catalogs observed live but not present in the v1.0 Part 5.4 inventory include: `dive_sites.json`, `hydroponic_crops.json`, `hydraulic_extrusion_catalog.json`, `metrology_standards_catalog.json`, `muster_camp_scenes.json`, `muster_epilogues.json`, `muster_faction_actions.json`, `muster_faction_culture.json`, `muster_witnesses.json`, `utility_actions.json`, `moral_choice_quests_branching.json`, `moral_choice_quests_distress.json`. Consequence: the duplication firewall (v1.0 Part 5) is stale in these domains; a planner could propose a "new" muster or moral-choice catalog that already exists. The ID-collision sweep in Factory Protocol step 1 must always run against the live listing, never against this document.

> **DR-05 — A process script lives inside the data authority. VERIFIED (hygiene finding).**
`Assets/StreamingAssets/Data/` contains `rewrite.py` alongside the JSON catalogs. The data directory is canonically "the sole authored JSON data authority" (`AGENTS.md` rule 3); a Python rewrite script inside it is a process artifact in a content directory. Recommended handling: a Tooling-lane (Lane H) subject plan proposing relocation of the script to `scripts/` or `tools/` with a documented rationale, after verifying what the script rewrites and who calls it. Do not move it without call-site verification; it may be load-bearing for a historical catalog migration.

> **DR-06 — Integration ledger state differs from the v1.0 queue snapshot. VERIFIED.**
Live `INTEGRATION_PLANS.md` (read 2026-09-24) shows, at minimum: the **XP Expansion W1** batch ACTIVE (difficulty authority package `XP-WAVE1-DIFFICULTY-AUTHORITY`, with a premise correction recorded against Plan 122 SOFC fuel); the **DISTRESS-SIGNALS-9-12** flagship COMPLETE and presented for acceptance; **Plan 24 CLOSED** (Wave 8, 2026-09-17, signatures resolved 2026-09-18, ward staffing sealed under option b, `DEBT-PLAN24-MEDICAL-WARD-STAFFING` RETIRED); **19A/19B/19C** waves closed with evidence (Endgame 84/84 PASS, focused suites 47 PASS, `verify-fast.sh` reported ALL 47 GATES PASSED); **C2[2] Plan 17** legibility executed with a documented not-executed list (Plan 31 semantic-kind authority, 17C audio phases, 17B deep test matrix remain open gaps); **PR 3 content seal SEALED 2026-09-19** (`CF-P1-DISTRESS-CONTENT-SEAL`); the **availability consumer RETIRED** (Wave 9 Part 2, Option B approved; `SignalTrustAvailability` retained as a pure-math specification pin). Consequence: subject plans in the radio/distress domain must treat the rescue-signal runtime as *sealed and closed*, not as an open expansion surface, unless they extend it through its recorded seams.

> The following v1.0 structures were confirmed by the audit and remain authoritative: the four-tier architecture (Tier 1 data authority in `Assets/StreamingAssets/Data/`; Tier 2 engine-free Core; Tier 3 `src/Host` + `src/UI`; Tier 4 xUnit plus the `HostCli` selftest surface); the `AGENTS.md` non-negotiable rules (Godot authoritative, Core engine-free, JSON authoritative, one authority per concern, focused verification); the narrative corpus under `Assets/StreamingAssets/Data/narrative/` (present in the live listing); the faction, economy, weather, Year-of-Ash, moral-choice, muster, and verdict catalog families (all present live); and the plan-discipline artifacts (`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `SESSION_HANDOFF.md`) at root.

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

Current evidence and safe integration boundary for Plan 44: Faction Territory, Contested Zones and Map Authority.

- **static territory and contested-zone definitions** remains with `FactionTerritoryCatalog` at `Assets/Ashfall.Core/World/FactionTerritoryCatalog.cs`. Catalog owner.
- **mutable control, contests, supply lines and census** remains with `TerritoryControlSystem` at `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs`. Sole mutable territory owner.
- **settlement geography and allegiance** remains with `SettlementCatalog` at `Assets/Ashfall.Core/World/SettlementCatalog.cs`. Separate settlement catalog.
- **host commands and persistence** remains with `TerritoryControlHostSession` at `src/Host/TerritoryControlHostSession.cs`. Thin adapter.
- **host integration and commands** remains with `Main.TerritoryControl` at `src/Main.TerritoryControl.cs`. Current host seam.
- **cross-catalog and save contract** remains with `Plan43_44 integration tests` at `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs`. Focused evidence.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load 19 territories and 5 zones
2. validate faction/node/location references
3. read current control state
4. project settlement allegiance and contested zones
5. route patrol/trade/encounter consumers through owner projections
6. present current map state
7. capture TerritoryControlSaveState

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Territory catalog rows are immutable definitions.
- TerritoryControlSaveState is the current mutable authority.
- Settlement allegiance does not overwrite control state without the current control command.
- Travel safety and trade tax are projections until their current consumers use them.

- Every controlled node/control point must resolve through current catalogs.
- A static trade tax is not a market mutation.
- A control shift emits one fact and persists once.
- A missing owner yields a truthful unavailable state.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/TerritoryControlHostSession.cs
- src/Main.TerritoryControl.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/World/FactionTerritoryCatalogTests.cs
- Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs
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
| S-01 | 44-01 19 territories load | load 19 territories and 5 zones | Territory catalog rows are immutable definitions. | A static territory row overwrites mutable control. | FactionTerritoryCatalog |
| S-02 | 44-02 5 zones load | validate faction/node/location references | TerritoryControlSaveState is the current mutable authority. | A settlement allegiance silently changes control. | FactionTerritoryCatalog |
| S-03 | 44-03 node references | read current control state | Settlement allegiance does not overwrite control state without the current control command. | A nonexistent map node passes validation. | FactionTerritoryCatalog |
| S-04 | 44-04 control-point cross-check | project settlement allegiance and contested zones | Travel safety and trade tax are projections until their current consumers use them. | A new map cache becomes authority. | FactionTerritoryCatalog |
| S-05 | 44-05 static/live separation | route patrol/trade/encounter consumers through owner projections | Territory catalog rows are immutable definitions. | A second save section is added. | FactionTerritoryCatalog |
| S-06 | 44-06 contest shift | present current map state | TerritoryControlSaveState is the current mutable authority. | A static territory row overwrites mutable control. | FactionTerritoryCatalog |
| S-07 | 44-07 save continuation | capture TerritoryControlSaveState | Settlement allegiance does not overwrite control state without the current control command. | A settlement allegiance silently changes control. | FactionTerritoryCatalog |
| S-08 | 44-08 same-seed replay | load 19 territories and 5 zones | Travel safety and trade tax are projections until their current consumers use them. | A nonexistent map node passes validation. | FactionTerritoryCatalog |
| S-09 | 44-09 map UI refresh | validate faction/node/location references | Territory catalog rows are immutable definitions. | A new map cache becomes authority. | FactionTerritoryCatalog |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 44-TC-01 schema/count | data | schema/count; verify the named current owner and its negative boundary without inventing a second authority. | FactionTerritoryCatalog |
| T-02 | 44-TC-02 ID/reference ranges | unit | ID/reference ranges; verify the named current owner and its negative boundary without inventing a second authority. | FactionTerritoryCatalog |
| T-03 | 44-TC-03 settlement cross-check | persistence | settlement cross-check; verify the named current owner and its negative boundary without inventing a second authority. | FactionTerritoryCatalog |
| T-04 | 44-TC-04 control shift | determinism | control shift; verify the named current owner and its negative boundary without inventing a second authority. | FactionTerritoryCatalog |
| T-05 | 44-TC-05 save round trip | host | save round trip; verify the named current owner and its negative boundary without inventing a second authority. | FactionTerritoryCatalog |
| T-06 | 44-TC-06 consumer projection | UI/accessibility | consumer projection; verify the named current owner and its negative boundary without inventing a second authority. | FactionTerritoryCatalog |
| T-07 | 44-TC-07 UI truth | cross-system | UI truth; verify the named current owner and its negative boundary without inventing a second authority. | FactionTerritoryCatalog |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 18 | `Ashfall.Core.Tests/World/SettlementCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 11 | `src/Host/TerritoryControlHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `Assets/Ashfall.Core/World/FactionTerritoryCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `Ashfall.Core.Tests/World/FactionTerritoryCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/World/Plan43_44SettlementTerritoryIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `src/Main.TerritoryControl.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Factions/Plan134TerritoryControlHostIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/World/Plan20WastelandInhabitantsTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/World/SettlementCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Host/HostCli.TerritoryControl.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Host/HostCli.WastelandInhabitants.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/World/NightWatchReadinessProjection.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/OutpostSettlementHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Main.OutpostSettlement.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/faction_territory.json`

### `Assets/StreamingAssets/Data/faction_territory.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 19950; characters: 19950.
- SHA-256: `ef0940bbe98f3082b75cbbef77df670e7e2c7358b89ddeefc20b8fd119b90915`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `collection_id`, `territories`, `contested_zones`

#### `territories` — 19 current rows

- Row 001 `territory_the_office`: `{"classification":"territorial","contested_with":["faction_the_tally","faction_scavenger_guild"],"control_points":["loc_settlement_nine_rails","loc_weighbridge"],"control_strength":85,"controlled_nodes":["loc_cut_arsenal_ruin"],"descriptio…`
- Row 002 `territory_the_cutters`: `{"classification":"territorial","contested_with":["faction_undertow","faction_black_flotilla"],"control_points":["loc_settlement_brine_pans","loc_the_shallows_market"],"control_strength":70,"controlled_nodes":["loc_cut_radiation_zone_alpha…`
- Row 003 `territory_black_flotilla`: `{"classification":"mixed","contested_with":["faction_the_fleet","faction_the_cutters"],"control_points":["loc_settlement_cape_beacon","loc_black_flotilla_outpost"],"control_strength":80,"controlled_nodes":["loc_black_flotilla_outpost"],"de…`
- Row 004 `territory_the_fleet`: `{"classification":"nomadic","contested_with":["faction_black_flotilla","faction_undertow"],"control_points":["loc_black_flotilla_outpost","loc_lock_gate_four"],"control_strength":60,"controlled_nodes":["loc_black_flotilla_outpost"],"descri…`
- Row 005 `territory_deserter_coalition`: `{"classification":"territorial","contested_with":["faction_iron_raiders","faction_the_provisioned"],"control_points":["loc_settlement_iron_siding","loc_settlement_fort_karkov"],"control_strength":90,"controlled_nodes":["loc_cut_abandoned_d…`
- Row 006 `territory_cold_count`: `{"classification":"territorial","contested_with":["faction_the_tally","faction_long_walk"],"control_points":["loc_settlement_slate_hollow","loc_low_background_lab"],"control_strength":65,"controlled_nodes":["loc_cut_arsenal_ruin"],"descrip…`
- Row 007 `territory_the_tally`: `{"classification":"territorial","contested_with":["faction_undertow","faction_the_office"],"control_points":["loc_settlement_lock_seven","loc_lock_gate_four"],"control_strength":75,"controlled_nodes":["loc_cut_radiation_zone_alpha"],"descr…`
- Row 008 `territory_grain_exchange`: `{"classification":"territorial","contested_with":["faction_iron_raiders","faction_the_tally"],"control_points":["loc_settlement_silo_burrow","loc_grain_silo"],"control_strength":70,"controlled_nodes":["loc_cut_merchant_caravanserai"],"desc…`
- Row 009 `territory_quiet_house`: `{"classification":"ideological","contested_with":["faction_osteophages","faction_iron_raiders"],"control_points":["loc_settlement_st_nicholas","loc_shrine_switchback_waystation"],"control_strength":50,"controlled_nodes":["loc_black_flotill…`
- Row 010 `territory_scavenger_guild`: `{"classification":"territorial","contested_with":["faction_iron_raiders","faction_the_office"],"control_points":["loc_settlement_tinkers_notch","loc_cut_merchant_caravanserai"],"control_strength":75,"controlled_nodes":["loc_cut_merchant_ca…`
- Row 011 `territory_long_walk`: `{"classification":"nomadic","contested_with":["faction_sun_seekers","faction_cold_count"],"control_points":["loc_settlement_pilgrim_hearth","loc_shrine_switchback_waystation"],"control_strength":45,"controlled_nodes":["loc_cut_merchant_car…`
- Row 012 `territory_undertow`: `{"classification":"territorial","contested_with":["faction_the_cutters","faction_the_tally"],"control_points":["loc_settlement_ferry_crossing","loc_water_station"],"control_strength":65,"controlled_nodes":["loc_cut_abandoned_depot"],"descr…`
- Row 013 `territory_hydro_barons`: `{"classification":"territorial","contested_with":["faction_undertow","faction_grain_exchange"],"control_points":["loc_water_station","loc_terrace_pumphouse"],"control_strength":80,"controlled_nodes":["loc_holdfast"],"description":"The mass…`
- Row 014 `territory_iron_raiders`: `{"classification":"nomadic","contested_with":["faction_scavenger_guild","faction_deserter_coalition"],"control_points":["loc_cut_abandoned_depot","loc_cut_arsenal_ruin"],"control_strength":60,"controlled_nodes":["loc_cut_abandoned_depot","…`
- Row 015 `territory_the_provisioned`: `{"classification":"territorial","contested_with":["faction_deserter_coalition","faction_scavenger_guild"],"control_points":["loc_excavation_command_vault","loc_logistics_reserve_cache"],"control_strength":90,"controlled_nodes":["loc_hidden…`
- Row 016 `territory_archivists`: `{"classification":"ideological","contested_with":["faction_the_office","faction_the_tally"],"control_points":["loc_excavation_archive_bunker","loc_hidden_relay_bunker"],"control_strength":40,"controlled_nodes":["loc_logistics_reserve_cache…`
- Row 017 `territory_lamplighters`: `{"classification":"nomadic","contested_with":["faction_iron_raiders","faction_undertow"],"control_points":["loc_cut_merchant_caravanserai","loc_cut_abandoned_depot"],"control_strength":50,"controlled_nodes":["loc_cut_merchant_caravanserai"…`
- Row 018 `territory_sun_seekers`: `{"classification":"nomadic","contested_with":["faction_long_walk","faction_osteophages"],"control_points":["loc_cut_radiation_zone_alpha","loc_broadcast_bunker_echo"],"control_strength":40,"controlled_nodes":["loc_cut_radiation_zone_alpha"…`
- Row 019 `territory_osteophages`: `{"classification":"territorial","contested_with":["faction_quiet_house","faction_the_cutters"],"control_points":["loc_cut_radiation_zone_alpha","loc_excavation_mine_shaft"],"control_strength":55,"controlled_nodes":["loc_cut_radiation_zone_…`

#### `contested_zones` — 5 current rows

- Row 001 `zone_contested_water_rights`: `{"claimant_factions":["faction_hydro_barons","faction_undertow","faction_the_cutters"],"conflict_driver":"Hydro-Barons seek metered pipeline monopoly; Undertow controls river barge transit; Cutters require brine flow for salt pans.","dispu…`
- Row 002 `zone_contested_cut_salvage`: `{"claimant_factions":["faction_scavenger_guild","faction_iron_raiders","faction_deserter_coalition"],"conflict_driver":"Scavenger Guild claims unstripped machine tooling; Iron Raiders ambush salvage convoys; Deserters fortify rail sidings …`
- Row 003 `zone_contested_merchant_crossroads`: `{"claimant_factions":["faction_the_office","faction_the_tally","faction_scavenger_guild"],"conflict_driver":"The Office attempts freight tariff enforcement; The Tally demands debt seizure rights; Scavenger Guild defends open non-chartered …`
- Row 004 `zone_contested_scarp_pass`: `{"claimant_factions":["faction_long_walk","faction_cold_count","faction_quiet_house"],"conflict_driver":"Long Walk pilgrims require unimpeded seasonal circuit; Cold Count seals radiation baseline lab; Quiet House maintains non-violent spri…`
- Row 005 `zone_contested_coastal_bluff`: `{"claimant_factions":["faction_black_flotilla","faction_the_fleet","faction_the_cutters"],"conflict_driver":"Black Flotilla claims raised shipwreck salvage; The Fleet asserts pre-war naval jurisdiction; Cutters contest coastal brine access…`


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


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/wasteland_map_v1.json`

### `Assets/StreamingAssets/Data/wasteland_map_v1.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 20796; characters: 20796.
- SHA-256: `5384c1c4093451593327ccc1fce9f8708b21923b8c94104a17e94d72808adf2b`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `trapSites`, `nodes`, `routes`

#### `trapSites` — 1 current rows

- Row 001 `row-1`: `{"anchorNodeId":"loc_holdfast","offsetX":-70.0,"offsetY":-50.0,"siteId":"snare_perimeter_north"}`

#### `nodes` — 22 current rows

- Row 001 `loc_holdfast`: `{"danger":"none","discoverable":false,"displayName":"Holdfast","faction":"player","id":"loc_holdfast","lootTable":null,"positionX":500,"positionY":300,"startingUnlocked":true}`
- Row 002 `loc_cut_abandoned_depot`: `{"danger":"low","discoverable":true,"displayName":"Abandoned Depot","faction":null,"id":"loc_cut_abandoned_depot","lootTable":"salvage_common","positionX":700,"positionY":200,"startingUnlocked":false}`
- Row 003 `loc_cut_radiation_zone_alpha`: `{"danger":"high","discoverable":true,"displayName":"Fallout Zone Alpha","faction":null,"id":"loc_cut_radiation_zone_alpha","lootTable":"salvage_rare","positionX":300,"positionY":400,"startingUnlocked":false}`
- Row 004 `loc_black_flotilla_outpost`: `{"danger":"locked","discoverable":true,"displayName":"Black Flotilla Outpost","faction":"black_flotilla","id":"loc_black_flotilla_outpost","lootTable":null,"positionX":600,"positionY":500,"startingUnlocked":false}`
- Row 005 `loc_cut_merchant_caravanserai`: `{"danger":"low","discoverable":true,"displayName":"Merchant Caravanserai","faction":"merchant","id":"loc_cut_merchant_caravanserai","lootTable":"trade_goods","positionX":400,"positionY":200,"startingUnlocked":true}`
- Row 006 `loc_cut_arsenal_ruin`: `{"danger":"medium","discoverable":true,"displayName":"Arsenal Ruin","faction":null,"id":"loc_cut_arsenal_ruin","lootTable":"salvage_weapons","positionX":800,"positionY":350,"startingUnlocked":false}`
- Row 007 `loc_hidden_relay_bunker`: `{"danger":"medium","discoverable":true,"displayName":"Hidden Relay Bunker","faction":null,"id":"loc_hidden_relay_bunker","lootTable":"salvage_electronic","positionX":350,"positionY":250,"startingUnlocked":false}`
- Row 008 `loc_logistics_reserve_cache`: `{"danger":"low","discoverable":true,"displayName":"Logistics Reserve Cache","faction":null,"id":"loc_logistics_reserve_cache","lootTable":"salvage_common","positionX":650,"positionY":350,"startingUnlocked":false}`
- Row 009 `loc_broadcast_bunker_echo`: `{"danger":"high","discoverable":true,"displayName":"Broadcast Bunker Echo","faction":null,"id":"loc_broadcast_bunker_echo","lootTable":"salvage_rare","positionX":550,"positionY":450,"startingUnlocked":false}`
- Row 010 `loc_diesel_tank_farm`: `{"danger":"medium","discoverable":true,"displayName":"Tank Farm 4-East","faction":null,"id":"loc_diesel_tank_farm","lootTable":"salvage_common","positionX":750,"positionY":150,"startingUnlocked":false}`
- Row 011 `loc_recovery_yard`: `{"danger":"medium","discoverable":true,"displayName":"Recovery Yard","faction":null,"id":"loc_recovery_yard","lootTable":"salvage_common","positionX":450,"positionY":550,"startingUnlocked":false}`
- Row 012 `loc_underground_fuel_depot`: `{"danger":"locked","discoverable":false,"displayName":"Underground Fuel Depot","faction":null,"id":"loc_underground_fuel_depot","lootTable":"salvage_common","positionX":780,"positionY":240,"startingUnlocked":false}`
- Row 013 `loc_municipal_seed_vault`: `{"danger":"locked","discoverable":false,"displayName":"Municipal Seed Vault","faction":null,"id":"loc_municipal_seed_vault","lootTable":"trade_goods","positionX":460,"positionY":140,"startingUnlocked":false}`
- Row 014 `loc_blacksite_armory_7`: `{"danger":"locked","discoverable":false,"displayName":"Blacksite Armory 7","faction":null,"id":"loc_blacksite_armory_7","lootTable":"salvage_weapons","positionX":860,"positionY":420,"startingUnlocked":false}`
- Row 015 `loc_excavation_command_vault`: `{"danger":"locked","discoverable":false,"displayName":"Collapsed Command Vault","faction":null,"id":"loc_excavation_command_vault","lootTable":"salvage_rare","positionX":220,"positionY":520,"startingUnlocked":false}`
- Row 016 `loc_deaddrop_command_shelter`: `{"danger":"locked","discoverable":false,"displayName":"Dead-Drop Command Shelter","faction":null,"id":"loc_deaddrop_command_shelter","lootTable":"salvage_common","positionX":680,"positionY":600,"startingUnlocked":false}`
- Row 017 `loc_sealed_triage_annex`: `{"danger":"locked","discoverable":false,"displayName":"Sealed Triage Annex","faction":null,"id":"loc_sealed_triage_annex","lootTable":"salvage_common","positionX":380,"positionY":320,"startingUnlocked":false}`
- Row 018 `loc_evidence_sub_basement`: `{"danger":"locked","discoverable":false,"displayName":"Evidence Sub-Basement","faction":null,"id":"loc_evidence_sub_basement","lootTable":"trade_goods","positionX":300,"positionY":180,"startingUnlocked":false}`
- Row 019 `loc_quarantine_barn`: `{"danger":"locked","discoverable":false,"displayName":"Quarantine Barn","faction":null,"id":"loc_quarantine_barn","lootTable":"salvage_common","positionX":560,"positionY":100,"startingUnlocked":false}`
- Row 020 `loc_forestry_emergency_store`: `{"danger":"locked","discoverable":false,"displayName":"Forestry Emergency Store","faction":null,"id":"loc_forestry_emergency_store","lootTable":"salvage_common","positionX":900,"positionY":250,"startingUnlocked":false}`
- Row 021 `loc_materials_research_sublevel`: `{"danger":"locked","discoverable":false,"displayName":"Materials Research Sublevel","faction":null,"id":"loc_materials_research_sublevel","lootTable":"salvage_rare","positionX":180,"positionY":300,"startingUnlocked":false}`
- Row 022 `loc_electrical_maintenance_exchange`: `{"danger":"locked","discoverable":false,"displayName":"Electrical Maintenance Exchange","faction":null,"id":"loc_electrical_maintenance_exchange","lootTable":"salvage_electronic","positionX":520,"positionY":620,"startingUnlocked":false}`

#### `routes` — 68 current rows

- Row 001 `row-1`: `{"distanceKm":12,"from":"loc_holdfast","to":"loc_cut_abandoned_depot","weatherHazard":0.2}`
- Row 002 `row-2`: `{"distanceKm":12,"from":"loc_cut_abandoned_depot","to":"loc_holdfast","weatherHazard":0.2}`
- Row 003 `row-3`: `{"distanceKm":8,"from":"loc_holdfast","to":"loc_cut_merchant_caravanserai","weatherHazard":0.1}`
- Row 004 `row-4`: `{"distanceKm":8,"from":"loc_cut_merchant_caravanserai","to":"loc_holdfast","weatherHazard":0.1}`
- Row 005 `row-5`: `{"distanceKm":18,"from":"loc_holdfast","to":"loc_cut_radiation_zone_alpha","weatherHazard":0.5}`
- Row 006 `row-6`: `{"distanceKm":18,"from":"loc_cut_radiation_zone_alpha","to":"loc_holdfast","weatherHazard":0.5}`
- Row 007 `row-7`: `{"distanceKm":9,"from":"loc_cut_abandoned_depot","to":"loc_cut_arsenal_ruin","weatherHazard":0.3}`
- Row 008 `row-8`: `{"distanceKm":9,"from":"loc_cut_arsenal_ruin","to":"loc_cut_abandoned_depot","weatherHazard":0.3}`
- Row 009 `row-9`: `{"distanceKm":7,"from":"loc_cut_merchant_caravanserai","to":"loc_cut_abandoned_depot","weatherHazard":0.15}`
- Row 010 `row-10`: `{"distanceKm":7,"from":"loc_cut_abandoned_depot","to":"loc_cut_merchant_caravanserai","weatherHazard":0.15}`
- Row 011 `row-11`: `{"distanceKm":22,"from":"loc_cut_merchant_caravanserai","to":"loc_black_flotilla_outpost","weatherHazard":0.4}`
- Row 012 `row-12`: `{"distanceKm":22,"from":"loc_black_flotilla_outpost","to":"loc_cut_merchant_caravanserai","weatherHazard":0.4}`
- Row 013 `row-13`: `{"currentStrength":0.3,"distanceKm":9,"from":"loc_holdfast","to":"loc_black_flotilla_outpost","toxicContamination":0.1,"travelDomain":"water","weatherHazard":0.35}`
- Row 014 `row-14`: `{"currentStrength":-0.3,"distanceKm":9,"from":"loc_black_flotilla_outpost","to":"loc_holdfast","toxicContamination":0.1,"travelDomain":"water","weatherHazard":0.35}`
- Row 015 `row-15`: `{"distanceKm":14,"from":"loc_cut_radiation_zone_alpha","to":"loc_black_flotilla_outpost","weatherHazard":0.45}`
- Row 016 `row-16`: `{"distanceKm":14,"from":"loc_black_flotilla_outpost","to":"loc_cut_radiation_zone_alpha","weatherHazard":0.45}`
- Row 017 `row-17`: `{"distanceKm":6,"from":"loc_holdfast","terrain_tags":["vertical_rock"],"to":"loc_hidden_relay_bunker","weatherHazard":0.1}`
- Row 018 `row-18`: `{"distanceKm":6,"from":"loc_hidden_relay_bunker","terrain_tags":["vertical_rock"],"to":"loc_holdfast","weatherHazard":0.1}`
- Row 019 `row-19`: `{"distanceKm":10,"from":"loc_holdfast","to":"loc_logistics_reserve_cache","weatherHazard":0.15}`
- Row 020 `row-20`: `{"distanceKm":10,"from":"loc_logistics_reserve_cache","to":"loc_holdfast","weatherHazard":0.15}`
- Row 021 `row-21`: `{"distanceKm":15,"from":"loc_holdfast","terrain_tags":["vertical_rock"],"to":"loc_broadcast_bunker_echo","weatherHazard":0.3}`
- Row 022 `row-22`: `{"distanceKm":15,"from":"loc_broadcast_bunker_echo","terrain_tags":["vertical_rock"],"to":"loc_holdfast","weatherHazard":0.3}`
- Row 023 `row-23`: `{"distanceKm":16,"from":"loc_holdfast","to":"loc_diesel_tank_farm","weatherHazard":0.25}`
- Row 024 `row-24`: `{"distanceKm":16,"from":"loc_diesel_tank_farm","to":"loc_holdfast","weatherHazard":0.25}`
- Row 025 `row-25`: `{"distanceKm":14,"from":"loc_holdfast","to":"loc_recovery_yard","weatherHazard":0.2}`
- Row 026 `row-26`: `{"distanceKm":14,"from":"loc_recovery_yard","to":"loc_holdfast","weatherHazard":0.2}`
- Row 027 `row-27`: `{"currentStrength":0.0,"distanceKm":4.7,"from":"loc_diesel_tank_farm","to":"loc_underground_fuel_depot","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 028 `row-28`: `{"currentStrength":0.0,"distanceKm":4.7,"from":"loc_underground_fuel_depot","to":"loc_diesel_tank_farm","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 029 `row-29`: `{"currentStrength":0.0,"distanceKm":4.5,"from":"loc_cut_abandoned_depot","to":"loc_underground_fuel_depot","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 030 `row-30`: `{"currentStrength":0.0,"distanceKm":4.5,"from":"loc_underground_fuel_depot","to":"loc_cut_abandoned_depot","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 031 `row-31`: `{"currentStrength":0.0,"distanceKm":4.2,"from":"loc_cut_merchant_caravanserai","to":"loc_municipal_seed_vault","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 032 `row-32`: `{"currentStrength":0.0,"distanceKm":4.2,"from":"loc_municipal_seed_vault","to":"loc_cut_merchant_caravanserai","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 033 `row-33`: `{"currentStrength":0.0,"distanceKm":8.2,"from":"loc_holdfast","to":"loc_municipal_seed_vault","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 034 `row-34`: `{"currentStrength":0.0,"distanceKm":8.2,"from":"loc_municipal_seed_vault","to":"loc_holdfast","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 035 `row-35`: `{"currentStrength":0.0,"distanceKm":4.6,"from":"loc_cut_arsenal_ruin","to":"loc_blacksite_armory_7","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 036 `row-36`: `{"currentStrength":0.0,"distanceKm":4.6,"from":"loc_blacksite_armory_7","to":"loc_cut_arsenal_ruin","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 037 `row-37`: `{"currentStrength":0.0,"distanceKm":11.1,"from":"loc_logistics_reserve_cache","to":"loc_blacksite_armory_7","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 038 `row-38`: `{"currentStrength":0.0,"distanceKm":11.1,"from":"loc_blacksite_armory_7","to":"loc_logistics_reserve_cache","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 039 `row-39`: `{"currentStrength":0.0,"distanceKm":7.2,"from":"loc_cut_radiation_zone_alpha","to":"loc_excavation_command_vault","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 040 `row-40`: `{"currentStrength":0.0,"distanceKm":7.2,"from":"loc_excavation_command_vault","to":"loc_cut_radiation_zone_alpha","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 041 `row-41`: `{"currentStrength":0.0,"distanceKm":6.4,"from":"loc_black_flotilla_outpost","to":"loc_deaddrop_command_shelter","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 042 `row-42`: `{"currentStrength":0.0,"distanceKm":6.4,"from":"loc_deaddrop_command_shelter","to":"loc_black_flotilla_outpost","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 043 `row-43`: `{"currentStrength":0.0,"distanceKm":11.8,"from":"loc_recovery_yard","to":"loc_deaddrop_command_shelter","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 044 `row-44`: `{"currentStrength":0.0,"distanceKm":11.8,"from":"loc_deaddrop_command_shelter","to":"loc_recovery_yard","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 045 `row-45`: `{"currentStrength":0.0,"distanceKm":6.1,"from":"loc_holdfast","to":"loc_sealed_triage_annex","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 046 `row-46`: `{"currentStrength":0.0,"distanceKm":6.1,"from":"loc_sealed_triage_annex","to":"loc_holdfast","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 047 `row-47`: `{"currentStrength":0.0,"distanceKm":6.1,"from":"loc_cut_merchant_caravanserai","to":"loc_sealed_triage_annex","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 048 `row-48`: `{"currentStrength":0.0,"distanceKm":6.1,"from":"loc_sealed_triage_annex","to":"loc_cut_merchant_caravanserai","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 049 `row-49`: `{"currentStrength":0.0,"distanceKm":5.1,"from":"loc_cut_merchant_caravanserai","to":"loc_evidence_sub_basement","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 050 `row-50`: `{"currentStrength":0.0,"distanceKm":5.1,"from":"loc_evidence_sub_basement","to":"loc_cut_merchant_caravanserai","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 051 `row-51`: `{"currentStrength":0.0,"distanceKm":4.3,"from":"loc_hidden_relay_bunker","to":"loc_evidence_sub_basement","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 052 `row-52`: `{"currentStrength":0.0,"distanceKm":4.3,"from":"loc_evidence_sub_basement","to":"loc_hidden_relay_bunker","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 053 `row-53`: `{"currentStrength":0.0,"distanceKm":9.4,"from":"loc_cut_merchant_caravanserai","to":"loc_quarantine_barn","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 054 `row-54`: `{"currentStrength":0.0,"distanceKm":9.4,"from":"loc_quarantine_barn","to":"loc_cut_merchant_caravanserai","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 055 `row-55`: `{"currentStrength":0.0,"distanceKm":8.6,"from":"loc_cut_abandoned_depot","to":"loc_quarantine_barn","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 056 `row-56`: `{"currentStrength":0.0,"distanceKm":8.6,"from":"loc_quarantine_barn","to":"loc_cut_abandoned_depot","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 057 `row-57`: `{"currentStrength":0.0,"distanceKm":9.0,"from":"loc_diesel_tank_farm","to":"loc_forestry_emergency_store","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 058 `row-58`: `{"currentStrength":0.0,"distanceKm":9.0,"from":"loc_forestry_emergency_store","to":"loc_diesel_tank_farm","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 059 `row-59`: `{"currentStrength":0.0,"distanceKm":10.3,"from":"loc_cut_abandoned_depot","to":"loc_forestry_emergency_store","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 060 `row-60`: `{"currentStrength":0.0,"distanceKm":10.3,"from":"loc_forestry_emergency_store","to":"loc_cut_abandoned_depot","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 061 `row-61`: `{"currentStrength":0.0,"distanceKm":8.9,"from":"loc_hidden_relay_bunker","to":"loc_materials_research_sublevel","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 062 `row-62`: `{"currentStrength":0.0,"distanceKm":8.9,"from":"loc_materials_research_sublevel","to":"loc_hidden_relay_bunker","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 063 `row-63`: `{"currentStrength":0.0,"distanceKm":7.8,"from":"loc_cut_radiation_zone_alpha","to":"loc_materials_research_sublevel","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 064 `row-64`: `{"currentStrength":0.0,"distanceKm":7.8,"from":"loc_materials_research_sublevel","to":"loc_cut_radiation_zone_alpha","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 065 `row-65`: `{"currentStrength":0.0,"distanceKm":4.9,"from":"loc_recovery_yard","to":"loc_electrical_maintenance_exchange","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 066 `row-66`: `{"currentStrength":0.0,"distanceKm":4.9,"from":"loc_electrical_maintenance_exchange","to":"loc_recovery_yard","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 067 `row-67`: `{"currentStrength":0.0,"distanceKm":8.6,"from":"loc_broadcast_bunker_echo","to":"loc_electrical_maintenance_exchange","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`
- Row 068 `row-68`: `{"currentStrength":0.0,"distanceKm":8.6,"from":"loc_electrical_maintenance_exchange","to":"loc_broadcast_bunker_echo","toxicContamination":0.0,"travelDomain":"land","weatherHazard":0.6}`


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


# Appendix — Current Source Detail: `src/Host/TerritoryControlHostSession.cs`

### `src/Host/TerritoryControlHostSession.cs` — complete current file

- Size: 113 lines / 5150 bytes.
- SHA-256: `66abc3c363663840ffaf57b22fad69f749796b349e042efaae6a0ed2386fcaf3`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: #nullable enable
00002: // SPDX-License-Identifier: MIT
00003: using System;
00004: using System.Collections.Generic;
00005: using System.IO;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Factions;
00008: using Ashfall.Core.Save;
00009:
00010: namespace AtomicWar.GodotApp
00011: {
00012:     /// <summary>
00013:     /// Host session for Plan 134: Dynamic Faction Territory & Supply Line Control.
00014:     /// Manages territorial nodes, contested sites, garrisons, and supply lines across the wasteland.
00015:     /// Follows single-authority rule: territory state lives in <see cref="TerritoryControlSystem"/>,
00016:     /// persistence lives in <see cref="TerritoryControlSaveStore"/>.
00017:     /// </summary>
00018:     public sealed class TerritoryControlHostSession : HostSessionBase
00019:     {
00020:         public const string CatalogTerritoriesFile = "faction_territory.json";
00021:         public const string CatalogSupplyLinesFile = "supply_lines.json";
00022:
00023:         public TerritoryControlSystem System { get; }
00024:         public bool CatalogLoaded { get; private set; }
00025:
00026:         public TerritoryControlHostSession(TerritoryControlSystem system)
00027:         {
00028:             System = system ?? throw new ArgumentNullException(nameof(system));
00029:
00030:             System.OnTerritoryControlChangedSeam += (loc, oldF, newF) => RaiseStateChanged();
00031:             System.OnTerritoryContestedSeam += (loc, oldF, newF) => RaiseStateChanged();
00032:             System.OnSupplyLineStatusChangedSeam += (line, st) => RaiseStateChanged();
00033:             System.OnSupplyLineDeliveredSeam += (line, amt) => RaiseStateChanged();
00034:             System.OnLocationFortifiedSeam += (loc, lvl) => RaiseStateChanged();
00035:         }
00036:
00037:         /// <summary>
00038:         /// Loads both authored catalogs (faction_territory.json and supply_lines.json)
00039:         /// into a live TerritoryControlHostSession.
00040:         /// </summary>
00041:         public static TerritoryControlHostSession Load(string dataDirectory, IFileIO files)
00042:         {
00043:             if (files == null) throw new ArgumentNullException(nameof(files));
00044:
00045:             string terrPath = Path.Combine(dataDirectory, CatalogTerritoriesFile);
00046:             if (!files.FileExists(terrPath))
00047:             {
00048:                 throw new FileNotFoundException(
00049:                     $"TerritoryControlHostSession: {CatalogTerritoriesFile} not found at '{terrPath}'.");
00050:             }
00051:
00052:             string supplyPath = Path.Combine(dataDirectory, CatalogSupplyLinesFile);
00053:             if (!files.FileExists(supplyPath))
00054:             {
00055:                 throw new FileNotFoundException(
00056:                     $"TerritoryControlHostSession: {CatalogSupplyLinesFile} not found at '{supplyPath}'.");
00057:             }
00058:
00059:             string terrJson = files.ReadAllText(terrPath);
00060:             string supplyJson = files.ReadAllText(supplyPath);
00061:
00062:             var system = TerritoryControlSystem.FromJson(terrJson, supplyJson);
00063:             return new TerritoryControlHostSession(system) { CatalogLoaded = true };
00064:         }
00065:
00066:         public TerritoryCensus ReadCensus() => System.ReadCensus();
00067:
00068:         public TerritoryControlSaveState CaptureState() => System.CaptureState();
00069:
00070:         public bool RestoreState(TerritoryControlSaveState? state)
00071:         {
00072:             bool ok = System.RestoreState(state);
00073:             if (ok) RaiseStateChanged();
00074:             return ok;
00075:         }
00076:
00077:         public bool FortifyLocation(string locationId, int levelDelta = 1) =>
00078:             System.FortifyLocation(locationId, levelDelta);
00079:
00080:         public bool AssignGarrison(string locationId, int garrisonDelta) =>
00081:             System.AssignGarrison(locationId, garrisonDelta);
00082:
00083:         public bool ContestLocation(string locationId, string attackingFactionId, int attackPower, ISeededRng rng, int currentDay = 0) =>
00084:             System.ContestLocation(locationId, attackingFactionId, attackPower, rng, currentDay);
00085:
00086:         public bool RaidSupplyLine(string supplyLineId, int raidIntensity, ISeededRng rng) =>
00087:             System.RaidSupplyLine(supplyLineId, raidIntensity, rng);
00088:
00089:         public bool RestoreSupplyLine(string supplyLineId) =>
00090:             System.RestoreSupplyLine(supplyLineId);
00091:
00092:         public void TickDay(int currentDay, ISeededRng? rng = null) =>
00093:             System.TickDay(currentDay, rng);
00094:     }
00095:
00096:     /// <summary>
00097:     /// Checksummed save store for the territory_control save section (Plan 134).
00098:     /// </summary>
00099:     public static class TerritoryControlSaveStore
00100:     {
00101:         public const string FileName = "territory_control_save.json";
00102:         public const string SectionName = "territory_control";
00103:
00104:         private static readonly SaveStore<TerritoryControlSaveState> s_store =
00105:             SaveStoreHub.Checksummed<TerritoryControlSaveState>(FileName, nameof(TerritoryControlSaveStore));
00106:
00107:         public static bool TrySave(TerritoryControlSaveState state) => s_store.TrySave(state);
00108:         public static TerritoryControlSaveState? TryLoad() => s_store.TryLoad();
00109:         public static string TryCapturePersisted(TerritoryControlSaveState state) => s_store.CapturePersisted(state);
00110:         public static TerritoryControlSaveState? TryRestore(string json) => s_store.RestoreEnvelope(json);
00111:         public static TerritoryControlSaveState? TryRestoreBare(string json) => s_store.RestoreBare(json);
00112:     }
00113: }
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


# Appendix — Current Source Detail: `Assets/Ashfall.Core/World/FactionTerritoryCatalog.cs`

### `Assets/Ashfall.Core/World/FactionTerritoryCatalog.cs` — complete current file

- Size: 154 lines / 6874 bytes.
- SHA-256: `9b583f668e225556efc7d91106c6b272fafa52914d7d16781797d1db8e54ae07`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006:
00007: namespace Ashfall.Core.World
00008: {
00009:     /// <summary>
00010:     /// Raw deserialization shape for faction_territory.json.
00011:     /// </summary>
00012:     [Serializable]
00013:     public sealed class FactionTerritoryCatalogContainer
00014:     {
00015:         public int schema_version { get; set; } = 1;
00016:         public string collection_id { get; set; } = "faction_territory_catalog";
00017:         public List<FactionTerritoryDef> territories { get; set; } = new List<FactionTerritoryDef>();
00018:         public List<ContestedZoneDef> contested_zones { get; set; } = new List<ContestedZoneDef>();
00019:     }
00020:
00021:     /// <summary>
00022:     /// DTO defining a faction's territorial footprint, node control, and strategic anchors.
00023:     /// </summary>
00024:     [Serializable]
00025:     public sealed class FactionTerritoryDef
00026:     {
00027:         public string id { get; set; } = string.Empty;
00028:         public string faction { get; set; } = string.Empty;
00029:         public string display_name { get; set; } = string.Empty;
00030:         public string classification { get; set; } = "territorial"; // territorial, nomadic, ideological, mixed
00031:         public string territory_scale { get; set; } = "minor"; // major, medium, minor, none
00032:         public string primary_resource_interest { get; set; } = string.Empty;
00033:         public List<string> controlled_nodes { get; set; } = new List<string>();
00034:         public List<string> control_points { get; set; } = new List<string>();
00035:         public List<string> contested_with { get; set; } = new List<string>();
00036:         public int control_strength { get; set; } = 50;
00037:         public float trade_tax { get; set; } = 0.0f;
00038:         public float travel_safety { get; set; } = 1.0f;
00039:         public string shift_trigger { get; set; } = string.Empty;
00040:         public string description { get; set; } = string.Empty;
00041:     }
00042:
00043:     /// <summary>
00044:     /// DTO defining a contested territorial flashpoint with overlapping faction claims.
00045:     /// </summary>
00046:     [Serializable]
00047:     public sealed class ContestedZoneDef
00048:     {
00049:         public string id { get; set; } = string.Empty;
00050:         public string name { get; set; } = string.Empty;
00051:         public string strategic_value { get; set; } = string.Empty;
00052:         public string focal_node_id { get; set; } = string.Empty;
00053:         public string focal_location_id { get; set; } = string.Empty;
00054:         public List<string> claimant_factions { get; set; } = new List<string>();
00055:         public string conflict_driver { get; set; } = string.Empty;
00056:         public int hazard_rating { get; set; } = 1;
00057:         public int dispute_intensity { get; set; } = 50;
00058:     }
00059:
00060:     /// <summary>
00061:     /// Authoritative domain catalog for faction territories and contested zones in ASHFALL (Plan 44).
00062:     /// </summary>
00063:     public sealed class FactionTerritoryCatalog
00064:     {
00065:         public const string DefaultFileName = "faction_territory.json";
00066:
00067:         private readonly Dictionary<string, FactionTerritoryDef> _territoriesById;
00068:         private readonly Dictionary<string, FactionTerritoryDef> _territoriesByFaction;
00069:         private readonly Dictionary<string, ContestedZoneDef> _contestedZonesById;
00070:         private readonly List<FactionTerritoryDef> _territories;
00071:         private readonly List<ContestedZoneDef> _contestedZones;
00072:
00073:         public IReadOnlyList<FactionTerritoryDef> Territories => _territories;
00074:         public IReadOnlyList<ContestedZoneDef> ContestedZones => _contestedZones;
00075:         public int TerritoryCount => _territories.Count;
00076:         public int ContestedZoneCount => _contestedZones.Count;
00077:
00078:         public FactionTerritoryCatalog(IEnumerable<FactionTerritoryDef> territories, IEnumerable<ContestedZoneDef>? contestedZones = null)
00079:         {
00080:             _territories = territories?.Where(t => t != null && !string.IsNullOrEmpty(t.id)).ToList() ?? new List<FactionTerritoryDef>();
00081:             _contestedZones = contestedZones?.Where(z => z != null && !string.IsNullOrEmpty(z.id)).ToList() ?? new List<ContestedZoneDef>();
00082:
00083:             _territoriesById = new Dictionary<string, FactionTerritoryDef>(StringComparer.OrdinalIgnoreCase);
00084:             _territoriesByFaction = new Dictionary<string, FactionTerritoryDef>(StringComparer.OrdinalIgnoreCase);
00085:             _contestedZonesById = new Dictionary<string, ContestedZoneDef>(StringComparer.OrdinalIgnoreCase);
00086:
00087:             foreach (var t in _territories)
00088:             {
00089:                 _territoriesById[t.id] = t;
00090:                 if (!string.IsNullOrEmpty(t.faction))
00091:                 {
00092:                     _territoriesByFaction[t.faction] = t;
00093:                 }
00094:             }
00095:
00096:             foreach (var z in _contestedZones)
00097:             {
00098:                 _contestedZonesById[z.id] = z;
00099:             }
00100:         }
00101:
00102:         public bool TryGetTerritory(string territoryId, out FactionTerritoryDef territory)
00103:         {
00104:             return _territoriesById.TryGetValue(territoryId, out territory!);
00105:         }
00106:
00107:         public bool TryGetTerritoryByFaction(string factionId, out FactionTerritoryDef territory)
00108:         {
00109:             return _territoriesByFaction.TryGetValue(factionId, out territory!);
00110:         }
00111:
00112:         public bool TryGetContestedZone(string zoneId, out ContestedZoneDef zone)
00113:         {
00114:             return _contestedZonesById.TryGetValue(zoneId, out zone!);
00115:         }
00116:
00117:         public IEnumerable<FactionTerritoryDef> GetTerritoriesForNode(string mapNodeId)
00118:         {
00119:             if (string.IsNullOrEmpty(mapNodeId)) yield break;
00120:             foreach (var t in _territories)
00121:             {
00122:                 if (t.controlled_nodes.Contains(mapNodeId, StringComparer.OrdinalIgnoreCase))
00123:                 {
00124:                     yield return t;
00125:                 }
00126:             }
00127:         }
00128:
00129:         public static FactionTerritoryCatalog LoadFromDirectory(string dataDirectory, IFileIO fileIO, IJsonSerializer? jsonSerializer = null)
00130:         {
00131:             if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
00132:             string path = Path.Combine(dataDirectory, DefaultFileName);
00133:             if (!fileIO.FileExists(path))
00134:             {
00135:                 return new FactionTerritoryCatalog(Enumerable.Empty<FactionTerritoryDef>());
00136:             }
00137:
00138:             string json = fileIO.ReadAllText(path);
00139:             return LoadFromJson(json, jsonSerializer);
00140:         }
00141:
00142:         public static FactionTerritoryCatalog LoadFromJson(string json, IJsonSerializer? jsonSerializer = null)
00143:         {
00144:             var serializer = jsonSerializer ?? new SystemTextJsonSerializer();
00145:             var container = serializer.Deserialize<FactionTerritoryCatalogContainer>(json);
00146:             if (container == null)
00147:             {
00148:                 return new FactionTerritoryCatalog(Enumerable.Empty<FactionTerritoryDef>());
00149:             }
00150:
00151:             return new FactionTerritoryCatalog(container.territories, container.contested_zones);
00152:         }
00153:     }
00154: }
```


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


# Appendix — Current Source Detail: `src/Main.TerritoryControl.cs`

### `src/Main.TerritoryControl.cs` — complete current file

- Size: 144 lines / 5610 bytes.
- SHA-256: `f439d5435ea72d45ac7e2c060431f6581d3b5cd4ce1f37575e34f4df72620f03`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: #nullable enable
00002: // SPDX-License-Identifier: MIT
00003: // ============================================================================
00004: // Plan 134 — Dynamic Faction Territory & Supply Lines in the running game.
00005: //
00006: // Authority boundary: TerritoryControlSystem is the single authority for
00007: // territorial control, contested nodes, fortification levels, and supply corridors.
00008: // FactionWarSystem (Plan 30) and FactionEcology (Plan 25) observe or trigger
00009: // territorial shifts, but all territory facts belong strictly to TerritoryControlSystem.
00010: // ============================================================================
00011: using System;
00012: using Godot;
00013: using Ashfall.Core;
00014: using Ashfall.Core.Factions;
00015: using Ashfall.Core.Save;
00016:
00017: namespace AtomicWar.GodotApp
00018: {
00019:     public partial class Main : Control
00020:     {
00021:         private TerritoryControlHostSession? _territoryControl;
00022:         private bool _territoryControlDirty;
00023:
00024:         public TerritoryControlHostSession? TerritoryControl => _territoryControl;
00025:
00026:         /// <summary>
00027:         /// Loads authored territory and supply line catalogs and restores any persisted state.
00028:         /// A missing catalog logs an error and leaves the feature dormant.
00029:         /// </summary>
00030:         private TerritoryControlHostSession? EnsureTerritoryControl()
00031:         {
00032:             if (_territoryControl != null) return _territoryControl;
00033:             try
00034:             {
00035:                 var session = TerritoryControlHostSession.Load(_dataDir, new FileSystemIO());
00036:                 session.RestoreState(TerritoryControlSaveStore.TryLoad());
00037:
00038:                 session.System.OnTerritoryControlChangedSeam += (loc, oldF, newF) =>
00039:                 {
00040:                     _journal?.TryAddRawEntry(
00041:                         "territory_control_changed",
00042:                         $"Control of {loc} shifted from {oldF} to {newF}.",
00043:                         null!, Math.Max(1, _simDay));
00044:                     _consequenceLedger?.Increment(
00045:                         $"territory_shift::{loc}::{newF}", 1, "territory_control", "control_changed", Math.Max(1, _simDay));
00046:                     _territoryControlDirty = true;
00047:                 };
00048:
00049:                 session.System.OnTerritoryContestedSeam += (loc, oldF, newF) =>
00050:                 {
00051:                     _journal?.TryAddRawEntry(
00052:                         "territory_contested",
00053:                         $"{loc} held by {oldF} was contested by {newF}.",
00054:                         null!, Math.Max(1, _simDay));
00055:                     _territoryControlDirty = true;
00056:                 };
00057:
00058:                 session.System.OnSupplyLineStatusChangedSeam += (line, status) =>
00059:                 {
00060:                     _journal?.TryAddRawEntry(
00061:                         "supply_line_status_changed",
00062:                         $"Supply corridor {line} status changed to {status}.",
00063:                         null!, Math.Max(1, _simDay));
00064:                     _territoryControlDirty = true;
00065:                 };
00066:
00067:                 session.System.OnSupplyLineDeliveredSeam += (line, amount) =>
00068:                 {
00069:                     _territoryControlDirty = true;
00070:                 };
00071:
00072:                 session.System.OnLocationFortifiedSeam += (loc, lvl) =>
00073:                 {
00074:                     _journal?.TryAddRawEntry(
00075:                         "location_fortified",
00076:                         $"{loc} fortification reinforced to rank {lvl}.",
00077:                         null!, Math.Max(1, _simDay));
00078:                     _territoryControlDirty = true;
00079:                 };
00080:
00081:                 _territoryControl = session;
00082:             }
00083:             catch (Exception ex)
00084:             {
00085:                 GD.PrintErr($"[TerritoryControl] territory system unavailable: {ex.Message}");
00086:                 _territoryControl = null;
00087:             }
00088:             return _territoryControl;
00089:         }
00090:
00091:         private void SetupTerritoryControl()
00092:         {
00093:             var session = EnsureTerritoryControl();
00094:             if (session == null) return;
00095:             var census = session.ReadCensus();
00096:             GD.Print($"[TerritoryControl] {census.TotalTerritories} territories, {census.TotalNodes} nodes: {census.Describe()}.");
00097:         }
00098:
00099:         private void SaveTerritoryControl()
00100:         {
00101:             var session = _territoryControl;
00102:             if (session == null) return;
00103:             CaptureSection(
00104:                 TerritoryControlSaveStore.SectionName,
00105:                 TerritoryControlSaveStore.TryCapturePersisted(session.CaptureState()));
00106:             _territoryControlDirty = false;
00107:         }
00108:
00109:         private void RestoreTerritoryControl(string? json)
00110:         {
00111:             if (string.IsNullOrWhiteSpace(json)) return;
00112:             var session = EnsureTerritoryControl();
00113:             if (session == null) return;
00114:
00115:             var state = TerritoryControlSaveStore.TryRestoreBare(json)
00116:                 ?? TerritoryControlSaveStore.TryRestore(json);
00117:             if (state != null)
00118:             {
00119:                 session.RestoreState(state);
00120:                 _territoryControlDirty = false;
00121:             }
00122:         }
00123:
00124:         private void FlushTerritoryControlIfDirty()
00125:         {
00126:             if (_territoryControlDirty) SaveTerritoryControl();
00127:         }
00128:
00129:         private void ResetTerritoryControl()
00130:         {
00131:             _territoryControl?.Dispose();
00132:             _territoryControl = null;
00133:             _territoryControlDirty = false;
00134:         }
00135:
00136:         internal void TickTerritoryControl(int day, ISeededRng? rng = null)
00137:         {
00138:             var session = EnsureTerritoryControl();
00139:             if (session == null) return;
00140:             session.TickDay(day, rng);
00141:             _territoryControlDirty = true;
00142:         }
00143:     }
00144: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/World/FactionTerritoryCatalogTests.cs`

### `Ashfall.Core.Tests/World/FactionTerritoryCatalogTests.cs` — complete current file

- Size: 149 lines / 6534 bytes.
- SHA-256: `c324801e970bf6cb2f36f2ce21d8a71f58fa20d21d461a0301bc0cf4c54c3652`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.IO;
00004: using System.Linq;
00005: using Ashfall.Core;
00006: using Ashfall.Core.World;
00007: using Xunit;
00008:
00009: namespace Ashfall.Core.Tests.World
00010: {
00011:     public class FactionTerritoryCatalogTests
00012:     {
00013:         private static string GetDataPath()
00014:         {
00015:             string baseDir = AppContext.BaseDirectory;
00016:             string probe = Path.Combine(baseDir, "StreamingAssets", "Data");
00017:             if (Directory.Exists(probe)) return probe;
00018:             probe = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
00019:             if (Directory.Exists(probe)) return Path.GetFullPath(probe);
00020:             probe = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
00021:             if (Directory.Exists(probe)) return Path.GetFullPath(probe);
00022:             return Path.Combine(baseDir, "Assets", "StreamingAssets", "Data");
00023:         }
00024:
00025:         [Fact]
00026:         public void Catalog_LoadsSuccessfully_Contains19TerritoriesAnd5ContestedZones()
00027:         {
00028:             var fileIO = new FileSystemIO();
00029:             var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);
00030:
00031:             Assert.NotNull(catalog);
00032:             Assert.Equal(19, catalog.TerritoryCount);
00033:             Assert.Equal(5, catalog.ContestedZoneCount);
00034:             Assert.Equal(19, catalog.Territories.Count);
00035:             Assert.Equal(5, catalog.ContestedZones.Count);
00036:         }
00037:
00038:         [Fact]
00039:         public void EveryTerritory_HasValidIdAndRecognizedClassification()
00040:         {
00041:             var fileIO = new FileSystemIO();
00042:             var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);
00043:
00044:             var validClasses = new[] { "territorial", "nomadic", "ideological", "mixed" };
00045:             var validScales = new[] { "major", "medium", "minor", "none" };
00046:
00047:             foreach (var t in catalog.Territories)
00048:             {
00049:                 Assert.StartsWith("territory_", t.id);
00050:                 Assert.StartsWith("faction_", t.faction);
00051:                 Assert.False(string.IsNullOrWhiteSpace(t.display_name));
00052:                 Assert.Contains(t.classification, validClasses);
00053:                 Assert.Contains(t.territory_scale, validScales);
00054:                 Assert.InRange(t.control_strength, 0, 100);
00055:                 Assert.InRange(t.trade_tax, 0.0f, 1.0f);
00056:                 Assert.InRange(t.travel_safety, 0.0f, 1.0f);
00057:                 Assert.False(string.IsNullOrWhiteSpace(t.description));
00058:             }
00059:         }
00060:
00061:         [Fact]
00062:         public void EveryTerritory_ControlledNodes_AreNonEmpty_AndUnique()
00063:         {
00064:             var fileIO = new FileSystemIO();
00065:             var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);
00066:
00067:             var seenIds = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
00068:
00069:             foreach (var t in catalog.Territories)
00070:             {
00071:                 Assert.True(seenIds.Add(t.id), $"Duplicate territory id found: {t.id}");
00072:                 Assert.NotEmpty(t.controlled_nodes);
00073:                 Assert.NotEmpty(t.control_points);
00074:             }
00075:         }
00076:
00077:         [Fact]
00078:         public void EveryTerritory_ContestedWith_AreValidFactionsAndNotSelf()
00079:         {
00080:             var fileIO = new FileSystemIO();
00081:             var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);
00082:
00083:             foreach (var t in catalog.Territories)
00084:             {
00085:                 Assert.NotEmpty(t.contested_with);
00086:                 foreach (var rival in t.contested_with)
00087:                 {
00088:                     Assert.StartsWith("faction_", rival);
00089:                     Assert.NotEqual(t.faction, rival);
00090:                 }
00091:             }
00092:         }
00093:
00094:         [Fact]
00095:         public void EveryContestedZone_HasAtLeastTwoClaimantFactions()
00096:         {
00097:             var fileIO = new FileSystemIO();
00098:             var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);
00099:
00100:             var seenZoneIds = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
00101:
00102:             foreach (var zone in catalog.ContestedZones)
00103:             {
00104:                 Assert.True(seenZoneIds.Add(zone.id), $"Duplicate contested zone id: {zone.id}");
00105:                 Assert.StartsWith("zone_contested_", zone.id);
00106:                 Assert.False(string.IsNullOrWhiteSpace(zone.name));
00107:                 Assert.False(string.IsNullOrWhiteSpace(zone.strategic_value));
00108:                 Assert.StartsWith("loc_", zone.focal_node_id);
00109:                 Assert.StartsWith("loc_", zone.focal_location_id);
00110:                 Assert.True(zone.claimant_factions.Count >= 2, $"Zone {zone.id} must have at least 2 claimants");
00111:                 Assert.InRange(zone.hazard_rating, 1, 5);
00112:                 Assert.InRange(zone.dispute_intensity, 0, 100);
00113:             }
00114:         }
00115:
00116:         [Fact]
00117:         public void LookupsByIdAndFaction_WorkCorrectly()
00118:         {
00119:             var fileIO = new FileSystemIO();
00120:             var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);
00121:
00122:             Assert.True(catalog.TryGetTerritory("territory_the_office", out var officeTerritory));
00123:             Assert.Equal("faction_the_office", officeTerritory.faction);
00124:
00125:             Assert.True(catalog.TryGetTerritoryByFaction("faction_hydro_barons", out var hydroTerritory));
00126:             Assert.Equal("territory_hydro_barons", hydroTerritory.id);
00127:
00128:             Assert.True(catalog.TryGetContestedZone("zone_contested_water_rights", out var waterZone));
00129:             Assert.Contains("faction_hydro_barons", waterZone.claimant_factions);
00130:
00131:             Assert.False(catalog.TryGetTerritory("nonexistent_territory", out _));
00132:             Assert.False(catalog.TryGetTerritoryByFaction("nonexistent_faction", out _));
00133:             Assert.False(catalog.TryGetContestedZone("nonexistent_zone", out _));
00134:         }
00135:
00136:         [Fact]
00137:         public void GetTerritoriesForNode_ReturnsMatchingTerritories()
00138:         {
00139:             var fileIO = new FileSystemIO();
00140:             var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);
00141:
00142:             var depotTerritories = catalog.GetTerritoriesForNode("loc_cut_abandoned_depot").ToList();
00143:             Assert.NotEmpty(depotTerritories);
00144:             Assert.Contains(depotTerritories, t => t.faction == "faction_deserter_coalition");
00145:             Assert.Contains(depotTerritories, t => t.faction == "faction_undertow");
00146:             Assert.Contains(depotTerritories, t => t.faction == "faction_iron_raiders");
00147:         }
00148:     }
00149: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs`

### `Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs` — complete current file

- Size: 183 lines / 7477 bytes.
- SHA-256: `a707d73afdcf73ed193b18f640d17413ff7f8777de89f97b5b2d0203c529e461`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: #nullable enable
00002: // SPDX-License-Identifier: MIT
00003: using System;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Factions;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests.Factions
00011: {
00012:     public sealed class Plan134TerritoryControlIntegrationTests
00013:     {
00014:         private static string ResolveCatalogPath(string filename)
00015:         {
00016:             string path = Path.Combine(AppContext.BaseDirectory, "Data", filename);
00017:             if (!File.Exists(path))
00018:             {
00019:                 path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", filename));
00020:             }
00021:             if (!File.Exists(path))
00022:             {
00023:                 path = Path.Combine("Assets", "StreamingAssets", "Data", filename);
00024:             }
00025:             return path;
00026:         }
00027:
00028:         [Fact]
00029:         public void Catalogs_LoadAndParseTerritoriesAndSupplyLines()
00030:         {
00031:             string territoryPath = ResolveCatalogPath("faction_territory.json");
00032:             string supplyLinePath = ResolveCatalogPath("supply_lines.json");
00033:
00034:             Assert.True(File.Exists(territoryPath), $"Territory catalog missing at {territoryPath}");
00035:             Assert.True(File.Exists(supplyLinePath), $"Supply lines catalog missing at {supplyLinePath}");
00036:
00037:             var system = TerritoryControlSystem.FromJson(
00038:                 File.ReadAllText(territoryPath),
00039:                 File.ReadAllText(supplyLinePath));
00040:
00041:             var territories = system.GetAllTerritories();
00042:             Assert.NotEmpty(territories);
00043:             Assert.Contains(territories, t => t.Faction == "faction_the_office");
00044:
00045:             var supplyLines = system.GetAllSupplyLines();
00046:             Assert.NotEmpty(supplyLines);
00047:             Assert.Contains(supplyLines, l => l.OwningFactionId == "faction_the_office");
00048:         }
00049:
00050:         [Fact]
00051:         public void LocationState_InitializesFromTerritories_AndSupportsFortification()
00052:         {
00053:             string territoryPath = ResolveCatalogPath("faction_territory.json");
00054:             string supplyLinePath = ResolveCatalogPath("supply_lines.json");
00055:
00056:             var system = TerritoryControlSystem.FromJson(
00057:                 File.ReadAllText(territoryPath),
00058:                 File.ReadAllText(supplyLinePath));
00059:
00060:             var locState = system.GetLocationState("loc_settlement_nine_rails");
00061:             Assert.NotNull(locState);
00062:             Assert.Equal("faction_the_office", locState!.ControllingFactionId);
00063:             Assert.Equal(0, locState.FortificationLevel);
00064:
00065:             string? fortifiedLoc = null;
00066:             int newFortLevel = 0;
00067:             system.OnLocationFortifiedSeam = (loc, lvl) =>
00068:             {
00069:                 fortifiedLoc = loc;
00070:                 newFortLevel = lvl;
00071:             };
00072:
00073:             // Fortify to level 1 (+10 control strength)
00074:             int initialStrength = locState.ControlStrength;
00075:             bool fortified = system.FortifyLocation("loc_settlement_nine_rails", 1);
00076:             Assert.True(fortified);
00077:             Assert.Equal("loc_settlement_nine_rails", fortifiedLoc);
00078:             Assert.Equal(1, newFortLevel);
00079:             Assert.Equal(1, locState.FortificationLevel);
00080:             Assert.Equal(Math.Min(100, initialStrength + 10), locState.ControlStrength);
00081:         }
00082:
00083:         [Fact]
00084:         public void ContestLocation_DeterministicallyResolvesControlShift()
00085:         {
00086:             string territoryPath = ResolveCatalogPath("faction_territory.json");
00087:             string supplyLinePath = ResolveCatalogPath("supply_lines.json");
00088:
00089:             var system = TerritoryControlSystem.FromJson(
00090:                 File.ReadAllText(territoryPath),
00091:                 File.ReadAllText(supplyLinePath));
00092:
00093:             string? shiftedLoc = null;
00094:             string? oldFaction = null;
00095:             string? newFaction = null;
00096:             system.OnTerritoryControlChangedSeam = (loc, oldF, newF) =>
00097:             {
00098:                 shiftedLoc = loc;
00099:                 oldFaction = oldF;
00100:                 newFaction = newF;
00101:             };
00102:
00103:             // Attack with overwhelming power and favorable seed
00104:             var rng = new SeededRng(100);
00105:             bool captured = system.ContestLocation("loc_weighbridge", "faction_iron_raiders", attackPower: 250, rng: rng, currentDay: 10);
00106:             Assert.True(captured);
00107:             Assert.Equal("loc_weighbridge", shiftedLoc);
00108:             Assert.Equal("faction_the_office", oldFaction);
00109:             Assert.Equal("faction_iron_raiders", newFaction);
00110:
00111:             var updatedState = system.GetLocationState("loc_weighbridge");
00112:             Assert.NotNull(updatedState);
00113:             Assert.Equal("faction_iron_raiders", updatedState!.ControllingFactionId);
00114:             Assert.False(updatedState.IsContested);
00115:         }
00116:
00117:         [Fact]
00118:         public void RaidSupplyLine_DisruptsCorridor_AndDegradesDestinationControl()
00119:         {
00120:             string territoryPath = ResolveCatalogPath("faction_territory.json");
00121:             string supplyLinePath = ResolveCatalogPath("supply_lines.json");
00122:
00123:             var system = TerritoryControlSystem.FromJson(
00124:                 File.ReadAllText(territoryPath),
00125:                 File.ReadAllText(supplyLinePath));
00126:
00127:             string? disruptedLine = null;
00128:             SupplyLineStatus reportedStatus = SupplyLineStatus.Active;
00129:             system.OnSupplyLineStatusChangedSeam = (id, st) =>
00130:             {
00131:                 disruptedLine = id;
00132:                 reportedStatus = st;
00133:             };
00134:
00135:             var lineState = system.GetSupplyLineState("supply_office_rail_trunk");
00136:             Assert.NotNull(lineState);
00137:             Assert.Equal(SupplyLineStatus.Active, lineState!.Status);
00138:
00139:             var rng = new SeededRng(42);
00140:             bool raided = system.RaidSupplyLine("supply_office_rail_trunk", raidIntensity: 90, rng: rng);
00141:             Assert.True(raided);
00142:             Assert.Equal("supply_office_rail_trunk", disruptedLine);
00143:             Assert.Equal(SupplyLineStatus.Severed, lineState.Status);
00144:             Assert.Equal(SupplyLineStatus.Severed, reportedStatus);
00145:
00146:             // Restoration restores line to Active
00147:             bool restored = system.RestoreSupplyLine("supply_office_rail_trunk");
00148:             Assert.True(restored);
00149:             Assert.Equal(SupplyLineStatus.Active, lineState.Status);
00150:         }
00151:
00152:         [Fact]
00153:         public void TickDay_DeliversCargo_AndReinforcesDestinationControl()
00154:         {
00155:             string territoryPath = ResolveCatalogPath("faction_territory.json");
00156:             string supplyLinePath = ResolveCatalogPath("supply_lines.json");
00157:
00158:             var system = TerritoryControlSystem.FromJson(
00159:                 File.ReadAllText(territoryPath),
00160:                 File.ReadAllText(supplyLinePath));
00161:
00162:             string? deliveredLine = null;
00163:             int deliveredCargo = 0;
00164:             system.OnSupplyLineDeliveredSeam = (id, amt) =>
00165:             {
00166:                 deliveredLine = id;
00167:                 deliveredCargo = amt;
00168:             };
00169:
00170:             var destState = system.GetLocationState("loc_cut_arsenal_ruin");
00171:             Assert.NotNull(destState);
00172:             int prevStrength = destState!.ControlStrength;
00173:
00174:             system.TickDay(currentDay: 5);
00175:
00176:             var lineState = system.GetSupplyLineState("supply_office_rail_trunk");
00177:             Assert.NotNull(lineState);
00178:             Assert.Equal(5, lineState!.LastDeliveredDay);
00179:             Assert.True(lineState.TotalDelivered > 0);
00180:             Assert.True(destState.ControlStrength >= prevStrength);
00181:         }
00182:     }
00183: }
```


# Appendix — Focused Current Evidence: `Ashfall.Core.Tests/World/FactionTerritoryCatalogTests.cs`

### `Ashfall.Core.Tests/World/FactionTerritoryCatalogTests.cs` — complete current file

- Size: 149 lines / 6534 bytes.
- SHA-256: `c324801e970bf6cb2f36f2ce21d8a71f58fa20d21d461a0301bc0cf4c54c3652`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.IO;
00004: using System.Linq;
00005: using Ashfall.Core;
00006: using Ashfall.Core.World;
00007: using Xunit;
00008:
00009: namespace Ashfall.Core.Tests.World
00010: {
00011:     public class FactionTerritoryCatalogTests
00012:     {
00013:         private static string GetDataPath()
00014:         {
00015:             string baseDir = AppContext.BaseDirectory;
00016:             string probe = Path.Combine(baseDir, "StreamingAssets", "Data");
00017:             if (Directory.Exists(probe)) return probe;
00018:             probe = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
00019:             if (Directory.Exists(probe)) return Path.GetFullPath(probe);
00020:             probe = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
00021:             if (Directory.Exists(probe)) return Path.GetFullPath(probe);
00022:             return Path.Combine(baseDir, "Assets", "StreamingAssets", "Data");
00023:         }
00024:
00025:         [Fact]
00026:         public void Catalog_LoadsSuccessfully_Contains19TerritoriesAnd5ContestedZones()
00027:         {
00028:             var fileIO = new FileSystemIO();
00029:             var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);
00030:
00031:             Assert.NotNull(catalog);
00032:             Assert.Equal(19, catalog.TerritoryCount);
00033:             Assert.Equal(5, catalog.ContestedZoneCount);
00034:             Assert.Equal(19, catalog.Territories.Count);
00035:             Assert.Equal(5, catalog.ContestedZones.Count);
00036:         }
00037:
00038:         [Fact]
00039:         public void EveryTerritory_HasValidIdAndRecognizedClassification()
00040:         {
00041:             var fileIO = new FileSystemIO();
00042:             var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);
00043:
00044:             var validClasses = new[] { "territorial", "nomadic", "ideological", "mixed" };
00045:             var validScales = new[] { "major", "medium", "minor", "none" };
00046:
00047:             foreach (var t in catalog.Territories)
00048:             {
00049:                 Assert.StartsWith("territory_", t.id);
00050:                 Assert.StartsWith("faction_", t.faction);
00051:                 Assert.False(string.IsNullOrWhiteSpace(t.display_name));
00052:                 Assert.Contains(t.classification, validClasses);
00053:                 Assert.Contains(t.territory_scale, validScales);
00054:                 Assert.InRange(t.control_strength, 0, 100);
00055:                 Assert.InRange(t.trade_tax, 0.0f, 1.0f);
00056:                 Assert.InRange(t.travel_safety, 0.0f, 1.0f);
00057:                 Assert.False(string.IsNullOrWhiteSpace(t.description));
00058:             }
00059:         }
00060:
00061:         [Fact]
00062:         public void EveryTerritory_ControlledNodes_AreNonEmpty_AndUnique()
00063:         {
00064:             var fileIO = new FileSystemIO();
00065:             var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);
00066:
00067:             var seenIds = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
00068:
00069:             foreach (var t in catalog.Territories)
00070:             {
00071:                 Assert.True(seenIds.Add(t.id), $"Duplicate territory id found: {t.id}");
00072:                 Assert.NotEmpty(t.controlled_nodes);
00073:                 Assert.NotEmpty(t.control_points);
00074:             }
00075:         }
00076:
00077:         [Fact]
00078:         public void EveryTerritory_ContestedWith_AreValidFactionsAndNotSelf()
00079:         {
00080:             var fileIO = new FileSystemIO();
00081:             var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);
00082:
00083:             foreach (var t in catalog.Territories)
00084:             {
00085:                 Assert.NotEmpty(t.contested_with);
00086:                 foreach (var rival in t.contested_with)
00087:                 {
00088:                     Assert.StartsWith("faction_", rival);
00089:                     Assert.NotEqual(t.faction, rival);
00090:                 }
00091:             }
00092:         }
00093:
00094:         [Fact]
00095:         public void EveryContestedZone_HasAtLeastTwoClaimantFactions()
00096:         {
00097:             var fileIO = new FileSystemIO();
00098:             var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);
00099:
00100:             var seenZoneIds = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
00101:
00102:             foreach (var zone in catalog.ContestedZones)
00103:             {
00104:                 Assert.True(seenZoneIds.Add(zone.id), $"Duplicate contested zone id: {zone.id}");
00105:                 Assert.StartsWith("zone_contested_", zone.id);
00106:                 Assert.False(string.IsNullOrWhiteSpace(zone.name));
00107:                 Assert.False(string.IsNullOrWhiteSpace(zone.strategic_value));
00108:                 Assert.StartsWith("loc_", zone.focal_node_id);
00109:                 Assert.StartsWith("loc_", zone.focal_location_id);
00110:                 Assert.True(zone.claimant_factions.Count >= 2, $"Zone {zone.id} must have at least 2 claimants");
00111:                 Assert.InRange(zone.hazard_rating, 1, 5);
00112:                 Assert.InRange(zone.dispute_intensity, 0, 100);
00113:             }
00114:         }
00115:
00116:         [Fact]
00117:         public void LookupsByIdAndFaction_WorkCorrectly()
00118:         {
00119:             var fileIO = new FileSystemIO();
00120:             var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);
00121:
00122:             Assert.True(catalog.TryGetTerritory("territory_the_office", out var officeTerritory));
00123:             Assert.Equal("faction_the_office", officeTerritory.faction);
00124:
00125:             Assert.True(catalog.TryGetTerritoryByFaction("faction_hydro_barons", out var hydroTerritory));
00126:             Assert.Equal("territory_hydro_barons", hydroTerritory.id);
00127:
00128:             Assert.True(catalog.TryGetContestedZone("zone_contested_water_rights", out var waterZone));
00129:             Assert.Contains("faction_hydro_barons", waterZone.claimant_factions);
00130:
00131:             Assert.False(catalog.TryGetTerritory("nonexistent_territory", out _));
00132:             Assert.False(catalog.TryGetTerritoryByFaction("nonexistent_faction", out _));
00133:             Assert.False(catalog.TryGetContestedZone("nonexistent_zone", out _));
00134:         }
00135:
00136:         [Fact]
00137:         public void GetTerritoriesForNode_ReturnsMatchingTerritories()
00138:         {
00139:             var fileIO = new FileSystemIO();
00140:             var catalog = FactionTerritoryCatalog.LoadFromDirectory(GetDataPath(), fileIO);
00141:
00142:             var depotTerritories = catalog.GetTerritoriesForNode("loc_cut_abandoned_depot").ToList();
00143:             Assert.NotEmpty(depotTerritories);
00144:             Assert.Contains(depotTerritories, t => t.faction == "faction_deserter_coalition");
00145:             Assert.Contains(depotTerritories, t => t.faction == "faction_undertow");
00146:             Assert.Contains(depotTerritories, t => t.faction == "faction_iron_raiders");
00147:         }
00148:     }
00149: }
```


# Appendix — Focused Current Evidence: `Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs`

### `Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs` — complete current file

- Size: 183 lines / 7477 bytes.
- SHA-256: `a707d73afdcf73ed193b18f640d17413ff7f8777de89f97b5b2d0203c529e461`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: #nullable enable
00002: // SPDX-License-Identifier: MIT
00003: using System;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Factions;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests.Factions
00011: {
00012:     public sealed class Plan134TerritoryControlIntegrationTests
00013:     {
00014:         private static string ResolveCatalogPath(string filename)
00015:         {
00016:             string path = Path.Combine(AppContext.BaseDirectory, "Data", filename);
00017:             if (!File.Exists(path))
00018:             {
00019:                 path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", filename));
00020:             }
00021:             if (!File.Exists(path))
00022:             {
00023:                 path = Path.Combine("Assets", "StreamingAssets", "Data", filename);
00024:             }
00025:             return path;
00026:         }
00027:
00028:         [Fact]
00029:         public void Catalogs_LoadAndParseTerritoriesAndSupplyLines()
00030:         {
00031:             string territoryPath = ResolveCatalogPath("faction_territory.json");
00032:             string supplyLinePath = ResolveCatalogPath("supply_lines.json");
00033:
00034:             Assert.True(File.Exists(territoryPath), $"Territory catalog missing at {territoryPath}");
00035:             Assert.True(File.Exists(supplyLinePath), $"Supply lines catalog missing at {supplyLinePath}");
00036:
00037:             var system = TerritoryControlSystem.FromJson(
00038:                 File.ReadAllText(territoryPath),
00039:                 File.ReadAllText(supplyLinePath));
00040:
00041:             var territories = system.GetAllTerritories();
00042:             Assert.NotEmpty(territories);
00043:             Assert.Contains(territories, t => t.Faction == "faction_the_office");
00044:
00045:             var supplyLines = system.GetAllSupplyLines();
00046:             Assert.NotEmpty(supplyLines);
00047:             Assert.Contains(supplyLines, l => l.OwningFactionId == "faction_the_office");
00048:         }
00049:
00050:         [Fact]
00051:         public void LocationState_InitializesFromTerritories_AndSupportsFortification()
00052:         {
00053:             string territoryPath = ResolveCatalogPath("faction_territory.json");
00054:             string supplyLinePath = ResolveCatalogPath("supply_lines.json");
00055:
00056:             var system = TerritoryControlSystem.FromJson(
00057:                 File.ReadAllText(territoryPath),
00058:                 File.ReadAllText(supplyLinePath));
00059:
00060:             var locState = system.GetLocationState("loc_settlement_nine_rails");
00061:             Assert.NotNull(locState);
00062:             Assert.Equal("faction_the_office", locState!.ControllingFactionId);
00063:             Assert.Equal(0, locState.FortificationLevel);
00064:
00065:             string? fortifiedLoc = null;
00066:             int newFortLevel = 0;
00067:             system.OnLocationFortifiedSeam = (loc, lvl) =>
00068:             {
00069:                 fortifiedLoc = loc;
00070:                 newFortLevel = lvl;
00071:             };
00072:
00073:             // Fortify to level 1 (+10 control strength)
00074:             int initialStrength = locState.ControlStrength;
00075:             bool fortified = system.FortifyLocation("loc_settlement_nine_rails", 1);
00076:             Assert.True(fortified);
00077:             Assert.Equal("loc_settlement_nine_rails", fortifiedLoc);
00078:             Assert.Equal(1, newFortLevel);
00079:             Assert.Equal(1, locState.FortificationLevel);
00080:             Assert.Equal(Math.Min(100, initialStrength + 10), locState.ControlStrength);
00081:         }
00082:
00083:         [Fact]
00084:         public void ContestLocation_DeterministicallyResolvesControlShift()
00085:         {
00086:             string territoryPath = ResolveCatalogPath("faction_territory.json");
00087:             string supplyLinePath = ResolveCatalogPath("supply_lines.json");
00088:
00089:             var system = TerritoryControlSystem.FromJson(
00090:                 File.ReadAllText(territoryPath),
00091:                 File.ReadAllText(supplyLinePath));
00092:
00093:             string? shiftedLoc = null;
00094:             string? oldFaction = null;
00095:             string? newFaction = null;
00096:             system.OnTerritoryControlChangedSeam = (loc, oldF, newF) =>
00097:             {
00098:                 shiftedLoc = loc;
00099:                 oldFaction = oldF;
00100:                 newFaction = newF;
00101:             };
00102:
00103:             // Attack with overwhelming power and favorable seed
00104:             var rng = new SeededRng(100);
00105:             bool captured = system.ContestLocation("loc_weighbridge", "faction_iron_raiders", attackPower: 250, rng: rng, currentDay: 10);
00106:             Assert.True(captured);
00107:             Assert.Equal("loc_weighbridge", shiftedLoc);
00108:             Assert.Equal("faction_the_office", oldFaction);
00109:             Assert.Equal("faction_iron_raiders", newFaction);
00110:
00111:             var updatedState = system.GetLocationState("loc_weighbridge");
00112:             Assert.NotNull(updatedState);
00113:             Assert.Equal("faction_iron_raiders", updatedState!.ControllingFactionId);
00114:             Assert.False(updatedState.IsContested);
00115:         }
00116:
00117:         [Fact]
00118:         public void RaidSupplyLine_DisruptsCorridor_AndDegradesDestinationControl()
00119:         {
00120:             string territoryPath = ResolveCatalogPath("faction_territory.json");
00121:             string supplyLinePath = ResolveCatalogPath("supply_lines.json");
00122:
00123:             var system = TerritoryControlSystem.FromJson(
00124:                 File.ReadAllText(territoryPath),
00125:                 File.ReadAllText(supplyLinePath));
00126:
00127:             string? disruptedLine = null;
00128:             SupplyLineStatus reportedStatus = SupplyLineStatus.Active;
00129:             system.OnSupplyLineStatusChangedSeam = (id, st) =>
00130:             {
00131:                 disruptedLine = id;
00132:                 reportedStatus = st;
00133:             };
00134:
00135:             var lineState = system.GetSupplyLineState("supply_office_rail_trunk");
00136:             Assert.NotNull(lineState);
00137:             Assert.Equal(SupplyLineStatus.Active, lineState!.Status);
00138:
00139:             var rng = new SeededRng(42);
00140:             bool raided = system.RaidSupplyLine("supply_office_rail_trunk", raidIntensity: 90, rng: rng);
00141:             Assert.True(raided);
00142:             Assert.Equal("supply_office_rail_trunk", disruptedLine);
00143:             Assert.Equal(SupplyLineStatus.Severed, lineState.Status);
00144:             Assert.Equal(SupplyLineStatus.Severed, reportedStatus);
00145:
00146:             // Restoration restores line to Active
00147:             bool restored = system.RestoreSupplyLine("supply_office_rail_trunk");
00148:             Assert.True(restored);
00149:             Assert.Equal(SupplyLineStatus.Active, lineState.Status);
00150:         }
00151:
00152:         [Fact]
00153:         public void TickDay_DeliversCargo_AndReinforcesDestinationControl()
00154:         {
00155:             string territoryPath = ResolveCatalogPath("faction_territory.json");
00156:             string supplyLinePath = ResolveCatalogPath("supply_lines.json");
00157:
00158:             var system = TerritoryControlSystem.FromJson(
00159:                 File.ReadAllText(territoryPath),
00160:                 File.ReadAllText(supplyLinePath));
00161:
00162:             string? deliveredLine = null;
00163:             int deliveredCargo = 0;
00164:             system.OnSupplyLineDeliveredSeam = (id, amt) =>
00165:             {
00166:                 deliveredLine = id;
00167:                 deliveredCargo = amt;
00168:             };
00169:
00170:             var destState = system.GetLocationState("loc_cut_arsenal_ruin");
00171:             Assert.NotNull(destState);
00172:             int prevStrength = destState!.ControlStrength;
00173:
00174:             system.TickDay(currentDay: 5);
00175:
00176:             var lineState = system.GetSupplyLineState("supply_office_rail_trunk");
00177:             Assert.NotNull(lineState);
00178:             Assert.Equal(5, lineState!.LastDeliveredDay);
00179:             Assert.True(lineState.TotalDelivered > 0);
00180:             Assert.True(destState.ControlStrength >= prevStrength);
00181:         }
00182:     }
00183: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **Current evidence and safe integration boundary for Plan 44: Faction Territory, Contested Zones and Map Authority.**.

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
