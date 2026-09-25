# Plan 32 — Expedition Destination Authority, Scavenging References and Mid-Trip Recovery

> **Rebuild status:** COMPLETE 75-DESTINATION AUTHORITY — REFERENCE AND REACHABILITY MAINTENANCE
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

- The original plan correctly identified a reachability gap, but the live authority is now a curated 75-destination catalog with 179 canonical locations and additional location families. Destination count is not the quality metric; dispatch eligibility, route safety, scavenging references and recovery are.
- The current route is `expeditions.json` → `ExpeditionCatalogLoader`/`ExpeditionDefinitionRegistry` → `ExpeditionSystem` → expedition host/panel → map, encounter, scavenging and vehicle owners → existing expedition save.
- The rebase protects the single destination authority, handles micro/extension locations explicitly, and requires a current evidence check before any new destination is wired.

**Bounded outcome:** Retire the old 2→50 wiring premise. The current expedition catalog has 75 destinations, preserves the original two, and focused tests prove canonical IDs, valid bounds, scavenging references and mid-expedition save/restore. Do not expand every `locations.json` row into a dispatchable expedition without a current route/gate decision.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `expeditions.json` contains 75 unique destinations; `Plan32ExpeditionDestinationWiringTests` asserts the count, original two, canonical IDs, valid bounds, representative dispatch and mid-expedition restore.
- `Plan76DestinationLootReferenceTests` checks all loot categories against the merged item catalog and all scavenging-table references against the current authority.
- Some expedition IDs are not in root `locations.json`; this is not automatically an error because the current source has multiple location families and explicit destination registries.
- The expedition owner is `ExpeditionDefinitionRegistry`/`ExpeditionSystem`; panels and maps project it and must not create parallel destination catalogs.

**Master-authority sections applied to this rebase:**

- Master authority Volume 28 verification cookbook: focused evidence before broad gates.
- Lane D save/state/compatibility guidance: owner DTOs, migration and restore proof.
- Lane E UI/UX/accessibility guidance: truthful projections and keyboard/controller lifecycle.
- Lane G testing guidance: smallest affected target, negative cases and deterministic replay.
- Anti-padding protocol: content exhaustion may end the plan before the character checkpoint.
- Volume 32 current authority and cross-domain fact guidance.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 2→50 count target with a 75-row destination census and reference-family matrix.
- Document the difference between canonical locations, dispatchable destinations, micro locations and route-only nodes.
- Require every destination to have valid bounds, a current consumer and an explicit reference resolution path.
- Preserve mid-expedition save/restore and scavenging behavior as the real integration proof.

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
| authored destination definitions and registry | ExpeditionCatalogLoader/ExpeditionSystem | `Assets/Ashfall.Core/Expeditions/ExpeditionCatalogLoader.cs; Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | Sole expedition destination authority. |
| dispatch, travel, encounters and state | ExpeditionSystem | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | Owns active expedition state. |
| loot table references | ScavengingTableCatalog | `Assets/Ashfall.Core/Expeditions/ScavengingTableCatalog.cs; Assets/StreamingAssets/Data/scavenging_tables.json` | Owns table definitions; destinations reference them. |
| route projection and dispatch commands | Expedition host/UI | `src/Host/ExpeditionHostSession.cs; src/UI/ExpeditionPanel.cs` | Host and presentation only. |
| wiring, references and restore | Expedition focused tests | `Ashfall.Core.Tests/Expeditions/Plan32ExpeditionDestinationWiringTests.cs; Ashfall.Core.Tests/Expeditions/Plan76DestinationLootReferenceTests.cs` | Executable current proof. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Expedition Destination Authority, Scavenging References and Mid-Trip Recovery
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ ExpeditionCatalogLoader/ExpeditionSystem
│   authored destination definitions and registry
│ ExpeditionSystem
│   dispatch, travel, encounters and state
│ ScavengingTableCatalog
│   loot table references
│ Expedition host/UI
│   route projection and dispatch commands
│ Expedition focused tests
│   wiring, references and restore
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

1. **Preserve current state ownership.** ExpeditionCatalogLoader/ExpeditionSystem owns authored destination definitions and registry: Sole expedition destination authority.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| authored destination definitions and registry | ExpeditionCatalogLoader/ExpeditionSystem | `Assets/Ashfall.Core/Expeditions/ExpeditionCatalogLoader.cs; Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | Sole expedition destination authority. |
| dispatch, travel, encounters and state | ExpeditionSystem | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | Owns active expedition state. |
| loot table references | ScavengingTableCatalog | `Assets/Ashfall.Core/Expeditions/ScavengingTableCatalog.cs; Assets/StreamingAssets/Data/scavenging_tables.json` | Owns table definitions; destinations reference them. |
| route projection and dispatch commands | Expedition host/UI | `src/Host/ExpeditionHostSession.cs; src/UI/ExpeditionPanel.cs` | Host and presentation only. |
| wiring, references and restore | Expedition focused tests | `Ashfall.Core.Tests/Expeditions/Plan32ExpeditionDestinationWiringTests.cs; Ashfall.Core.Tests/Expeditions/Plan76DestinationLootReferenceTests.cs` | Executable current proof. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load expedition definitions and reference catalogs
2. resolve destination/route/vehicle/scavenging prerequisites
3. preview dispatch with exact deltas and refusal reasons
4. commit one active expedition through ExpeditionSystem
5. advance travel/encounters/scavenging through existing owners
6. arrive/return and update map/journal facts
7. capture/restore the current expedition state

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Destination definitions are immutable catalog data; active expedition progress is `ExpeditionSystem` state.
- Known/discovered location state belongs to the canonical map/location owner.
- A destination is dispatchable only when its current definition, route gate, crew/vehicle requirements and encounter/scavenging references are valid.
- Mid-expedition restore must preserve destination identity, progress, encounter state and pending completion.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Every destination ID is unique and resolves through the current location/destination authority.
- Loot categories and scavenging table IDs must resolve through the merged current catalogs.
- A dispatch preview and execute cannot disagree on blockers or costs.
- A mid-trip save restore continues the same destination and does not duplicate completion or rewards.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `expeditions.json` is the sole dispatchable destination catalog.
- Do not copy root `locations.json` rows into expeditions without route and gameplay evidence.
- A new row needs bounds, danger, encounter/scavenging references, a current location source and focused tests.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the existing expedition save section and aggregate codec.
- No new destination save section is justified.
- Legacy expedition state with unknown destination IDs must fail visibly or normalize only when the current contract allows.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Encounter rolls and travel use existing seeded streams.
- Tie-breaks are ordinal stable and not based on dictionary/hash order.
- Continuous and mid-trip restored runs produce the same final expedition state and event trace.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Expedition dispatch/travel/arrival/return facts are emitted by the current expedition owner.
- Map discovery events are consumed by the canonical map owner; they do not redefine destination eligibility.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/ExpeditionHostSession.cs
- src/Main.Expeditions.cs
- src/UI/ExpeditionPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Destination display names come from current location/expedition authority.
- The plan should communicate route trade-offs without copying real places or real conflict text.
- Arrival and consequence prose must remain downstream of owner facts.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A destination exists in JSON but cannot be dispatched. | ExpeditionCatalogLoader/ExpeditionSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A location is treated as an expedition solely because it is in locations.json. | ExpeditionSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A loot category or scavenging reference is unresolved. | ScavengingTableCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Map, route or vehicle owners receive shadow destination state. | Expedition host/UI | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A save restore loses the active destination or duplicates return rewards. | Expedition focused tests | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan32ExpeditionDestinationWiringTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan76DestinationLootReferenceTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — current census | Read expedition, map, scavenging and host owners. | 75 destinations and reference families are explicit. | No production path until the owning implementation package is separately claimed. |
| 1 — eligibility matrix | Classify canonical location, destination, route and micro-location roles. | No category is confused. | No production path until the owning implementation package is separately claimed. |
| 2 — dispatch/replay proof | Trace representative and mid-trip restored journeys. | Current tests and focused host path agree. | No production path until the owning implementation package is separately claimed. |
| 3 — expansion gate | Require evidence before any new destination row. | No count-only expansion remains. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/expeditions.json | READ ONLY; MODIFY only for proven route gap | 75-row authority |
| Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs | READ ONLY | Destination registry and runtime owner |
| Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs | READ ONLY | Runtime owner |
| src/UI/ExpeditionPanel.cs | READ ONLY | Current presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Treating all locations as dispatchable. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding a second destination registry. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Breaking merged item/scavenging references. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Claiming route reachability from a row count. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No automatic conversion of every location.
- No new expedition save section.
- No new destination manager.
- No production edits in this rebase.

# 23. Rollback and Recovery

- Revert the plan file.
- Future data changes retain the prior valid expedition/scavenging fixtures and focused restore tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 75 destinations and the original two are documented.
- Location-family boundaries are explicit.
- Dispatch, scavenging and mid-trip restore contracts are named.
- No parallel authority is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 2→50 count target with a 75-row destination census and reference-family matrix.
- Document the difference between canonical locations, dispatchable destinations, micro locations and route-only nodes.
- Require every destination to have valid bounds, a current consumer and an explicit reference resolution path.
- Preserve mid-expedition save/restore and scavenging behavior as the real integration proof.

## MUST NOT DO

- No automatic conversion of every location.
- No new expedition save section.
- No new destination manager.
- No production edits in this rebase.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan32ExpeditionDestinationWiringTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan76DestinationLootReferenceTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — current census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: authored destination definitions and registry → ExpeditionCatalogLoader/ExpeditionSystem; dispatch, travel, encounters and state → ExpeditionSystem; loot table references → ScavengingTableCatalog; route projection and dispatch commands → Expedition host/UI; wiring, references and restore → Expedition focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 32.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 32 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by ExpeditionCatalogLoader/ExpeditionSystem or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Expeditions/ExpeditionCatalogLoader.cs`

### `Assets/Ashfall.Core/Expeditions/ExpeditionCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 129 lines / 5194 bytes.
- SHA-256: `c1fd0d78b710437cda679310c98cafb229e1a58e0ea3dede187277f66a98dd8e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
internal sealed class ExpeditionJsonDto
public string id { get; set; } = string.Empty;
public string displayName { get; set; } = string.Empty;
public int distanceTicks { get; set; } = 8;
public float travelHours { get; set; }
public float dangerLevel { get; set; } = 1f;
public float encounterChancePerTick { get; set; } = 0.12f;
public float baseStaminaDrainPerHour { get; set; } = 2.0f;
public List<string>? lootCategories { get; set; }
public string? scavenging_table_id { get; set; }
public bool requiresDiscovery { get; set; }
public static class ExpeditionCatalogLoader
public const string PrimaryFileName = "expeditions.json";
public static List<ExpeditionDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`

### `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1579 lines / 71512 bytes.
- SHA-256: `6a179c24dead762831790f2f5aa3d322792b33de20ba91fe5cd651ffcbd31ed7`.
- Architecture signals: seeded references=7; save/restore symbols=2; typed event declarations=45; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum ExpeditionStance
public enum ExpeditionPhase
public class ExpeditionLootEntry
public string itemId = string.Empty;
public int quantity = 0;
public float weightKg = 0f;
public class CampShelterAssignment
public string survivorId = string.Empty;
public bool hasTent = false;
public bool hasBedroll = false;
public string shelterType = "none"; // none, lean_to, tent, cave
public class CampWatchShift
public string survivorId = string.Empty;
public int shiftIndex = 0;       // 0 = first half, 1 = second half
public float alertness = 1.0f;   // 0..1, degrades with fatigue
public bool isActive = false;
public class CampState
public int campStartDay = 0;
public float campStartHour = 0f;
public int nightSegmentsCompleted = 0;
public int totalNightSegments = 4;  // 4 segments = one night
public float firewoodRemaining = 0f;
public float firewoodConsumed = 0f;
public float heatOutput = 0f;       // degrees C added
public float waterReserved = 0f;
public float waterConsumed = 0f;
public float foodReserved = 0f;
public float foodConsumed = 0f;
public float temperatureC = 0f;    // ambient at camp
public string weatherCondition = "Clear";
public float coldExposure = 0f;    // accumulated cold damage
public float radiationExposure = 0f;
public int wildlifeThreatLevel = 0;
public bool encounterTriggered = false;
public string encounterKey = string.Empty;
public bool encounterResolved = false;
public string campOutcome = string.Empty; // resume, retreat, injury, loss, failed
public List<CampShelterAssignment> shelterAssignments = new List<CampShelterAssignment>();
public List<CampWatchShift> watchShifts = new List<CampWatchShift>();
public sealed class ExpeditionVehicleProfile
public string vehicleId = string.Empty;
public float speedMultiplier = 1f;
public float cargoCapacityKg = 0f;
public float breakdownChancePerTick = 0f;
public float fuelPerTravelTick = 0f;
public sealed class ExpeditionEstimate
public string locationId = string.Empty;
public string stance = string.Empty;
public int distanceTicks;
public float outboundTicks;
public float inboundTicks;
public float lootingTicks;
public float totalTicks;
public float cargoCapacityKg;
public float fuelRequired;
public float breakdownRiskPerTick;
public float breakdownRiskTotal;
public float encounterRiskPerTick;
public float weaponReadiness = 1f;
public float weaponJamRisk;
public bool usingVehicle;
public float partyProtection;
public int unprotectedCount;
public float projectedDosePerHour;
public float projectedDoseTotal;
public float projectedTripHours;
public float projectedGearWear;
public float protectiveLifeHours;
public bool predictsMidRouteFailure;
public float weatherSpeedMultiplier = 1f;
public float weatherEncounterMultiplier = 1f;
public float survivorSpeedMultiplier = 1f;
public sealed class ExpeditionProtectiveInputs
public float LocationRadRatePerHour;
public float WorkingProtection;
public int UnprotectedCount;
public float WeakestGearDegradeRate;
public float WeakestGearDurability;
public float WearMultiplier = 1f;
public float HoursPerTick = 1f;
public sealed class ExpeditionWeatherInputs
public float SpeedMultiplier = 1.0f;
public float EncounterMultiplier = 1.0f;
public class ExpeditionState
public string systemId = ExpeditionSystem.SystemId;
public string expeditionId = string.Empty;
public string survivorId = string.Empty;
public string locationId = string.Empty;
public string displayName = string.Empty;
public string stance = "Stealth";
public int phase = (int)ExpeditionPhase.Outbound;
public int startedDay = 0;
public int distanceTicks = 0;
public int travelTicksCompleted = 0;
public int lootingTicksCompleted = 0;
public float stamina = 100f;
public float maxLootCapacityKg = 40f;
public float currentWeightKg = 0f;
public int dangerLevel = 1;
public float encounterChancePerTick = 0.12f;
public int encounterCount = 0;
public bool isPushingLuck = false;
public bool isNightScavenge = false;
public bool hasBicycle = false;
public bool hasFlashlight = false;
public string vehicleId = string.Empty;
public float vehicleSpeedMultiplier = 1f;
public float weatherSpeedMultiplier = 1f;
public float survivorSpeedMultiplier = 1f;
public float vehicleBreakdownChancePerTick = 0f;
public bool vehicleBrokenDown = false;
public string outcomeText = string.Empty;
public List<ExpeditionLootEntry> loot = new List<ExpeditionLootEntry>();
public CampState campState = new CampState();
public class ExpeditionDefinition
public string id = string.Empty;
public string displayName = string.Empty;
public int distanceTicks = 8;
public int dangerLevel = 1;
public float encounterChancePerTick = 0.12f;
public float baseStaminaDrainPerHour = 2.0f;
public List<string> lootCategories = new List<string>();
public string scavenging_table_id = string.Empty;
public bool requiresDiscovery = false;
public class ExpeditionSystem
public const string SystemId = "expedition_system";
public const float MaxStamina = 100f;
public const int AutoRetreatAfterLootTicks = 3;
public const float EncumberPenaltyPerTickMax = 15f;
public const int CampNightSegments = 4;
public const float CampFirewoodPerSegment = 2.0f;
public const float CampHeatPerFirewood = 3.0f;     // degrees C per unit
public const float CampWaterPerSegment = 0.5f;
public const float CampFoodPerSegment = 0.5f;
public const float CampColdDamageThresholdC = -5f;  // below this, cold damage
public const float CampColdDamagePerSegment = 5f;   // HP per segment below threshold
public const float CampStaminaRecoveryPerSegment = 8f;
public const float CampEncounterChanceBase = 0.15f;
public const float CampSentryDetectionBonus = 0.3f; // reduces encounter chance
public ScavengingTableCatalog? ScavengingCatalog { get; set; }
public Func<string, bool>? IsItemGenerationAvailable { get; set; }
public Action<string>? OnItemGenerationCommitted { get; set; }
public DamagedMapSystem? DamagedMap { get; set; }
public event Action<ExpeditionState> OnExpeditionStarted;
public event Action<string>? OnLocationDiscovered;
public event Action<ExpeditionState> OnExpeditionTick;
public event Action<ExpeditionState> OnPhaseChanged;
public event Action<ExpeditionState> OnLootAdded;                 // state, itemId, qty
public event Action<ExpeditionState> OnEncounterTriggered;
public event Action<ExpeditionState> OnVehicleBreakdown;
public event Action<ExpeditionState> OnExpeditionCompleted;
public event Action<ExpeditionState, string> OnExpeditionFailed;
public event Action<ExpeditionState> OnStateChanged;
public event Action<ExpeditionState> OnCampEntered;
public event Action<ExpeditionState> OnCampSuppliesReserved;
public event Action<ExpeditionState> OnCampNightSegmentResolved;
public event Action<ExpeditionState> OnCampEncounterSurfaced;
public event Action<ExpeditionState> OnCampEncounterResolved;
public event Action<ExpeditionState> OnCampDawnResolved;
public void SetStaminaDrainMultiplier(Func<string, float> multiplier) {
public void SetSurvivorSpeedMultiplierQuery(Func<string, float> query) {
public void SetPackCapacityBonusQuery(Func<string, float>? query) {
public void SetEncounterChanceMultiplier(Func<string, float> multiplier) {
public IReadOnlyDictionary<string, ExpeditionState> Active => _active;
public int ActiveCount => _active.Count;
public int CompletedCount => _completedCount;
public bool Start( ExpeditionDefinition def, string survivorId, int day, ExpeditionStance stance = ExpeditionStance.Stealth, bool isNightScavenge = false,
public CommandPreview PreviewStart( ExpeditionDefinition def, string survivorId, int day, ExpeditionStance stance = ExpeditionStance.Stealth, bool isNightScavenge = false,
public CommandResult ExecuteStart( ExpeditionDefinition def, string survivorId, int day, ExpeditionStance stance = ExpeditionStance.Stealth, bool isNightScavenge = false,
public CommandPreview PreviewPushLuck(string survivorId, long stateVersion = 0) {
public CommandResult ExecutePushLuck(string survivorId, long expectedStateVersion = 0, long currentStateVersion = 0) {
public CommandPreview PreviewRetreat(string survivorId, long stateVersion = 0) {
public CommandResult ExecuteRetreat(string survivorId, long expectedStateVersion = 0, long currentStateVersion = 0) {
public static ExpeditionEstimate Estimate( ExpeditionDefinition def, ExpeditionStance stance, bool isNightScavenge = false, ExpeditionVehicleProfile? vehicle = null, float weaponReadiness = 1f,
public void TickHours(float hours, ISeededRng rng) {
public bool PushLuck(string survivorId) {
public bool Retreat(string survivorId) {
public bool EnterCamp( string survivorId, int day, float hour, float temperatureC, string weatherCondition,
public bool ReserveCampSupplies( string survivorId, float firewood, float water, float food) {
public bool CampTick(string survivorId, ISeededRng rng) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Expeditions/ScavengingTableCatalog.cs`

### `Assets/Ashfall.Core/Expeditions/ScavengingTableCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 208 lines / 8514 bytes.
- SHA-256: `7209d613e76a35b35037de1165b265e181cd8ea53186faa5543d864d38f5e45d`.
- Architecture signals: seeded references=2; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ScavengingTableCatalogContainer
public int schema_version { get; set; } = 1;
public string collection_id { get; set; } = "scavenging_tables_catalog";
public List<ScavengingTableDef> tables { get; set; } = new List<ScavengingTableDef>();
public sealed class ScavengingTableDef
public string id { get; set; } = string.Empty;
public string location_type { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public string depletion_model { get; set; } = "finite"; // finite, renewable, one_time, slow_regeneration
public float base_hazard_chance { get; set; } = 0.0f;
public string primary_hazard_type { get; set; } = string.Empty;
public List<ScavengingLootEntryDef> entries { get; set; } = new List<ScavengingLootEntryDef>();
public int TotalWeight => entries?.Sum(e => Math.Max(0, e.weight)) ?? 0;
public sealed class ScavengingLootEntryDef
public string item_id { get; set; } = string.Empty;
public int weight { get; set; } = 10;
public int min_quantity { get; set; } = 1;
public int max_quantity { get; set; } = 1;
public string rarity_tier { get; set; } = "common"; // common, uncommon, rare, unique
public float hazard_chance { get; set; } = 0.0f;
public string hazard_type { get; set; } = string.Empty;
public string codex_unlock_id { get; set; } = string.Empty;
public string map_fragment_id { get; set; } = string.Empty;
public sealed class ScavengingRollResult
public string TableId { get; set; } = string.Empty;
public string ItemId { get; set; } = string.Empty;
public int Quantity { get; set; } = 1;
public string RarityTier { get; set; } = "common";
public bool HazardTriggered { get; set; }
public string HazardType { get; set; } = string.Empty;
public string CodexUnlockId { get; set; } = string.Empty;
public string MapFragmentId { get; set; } = string.Empty;
public sealed class ScavengingTableCatalog
public const string DefaultFileName = "scavenging_tables.json";
public IReadOnlyList<ScavengingTableDef> Tables => _tables;
public int TableCount => _tables.Count;
public bool TryGetTable(string tableId, out ScavengingTableDef table) {
public bool TryGetTableByLocationType(string locationType, out ScavengingTableDef table) {
public ScavengingRollResult? RollLoot(string tableId, ISeededRng rng, Func<string, bool>? itemFilter = null) {
public static ScavengingTableCatalog LoadFromDirectory(string dataDirectory, IFileIO fileIO, IJsonSerializer? jsonSerializer = null) {
public static ScavengingTableCatalog LoadFromJson(string json, IJsonSerializer? jsonSerializer = null) {
```


# Appendix B.05 — Current Code Architecture: `src/Host/ExpeditionHostSession.cs`

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


# Appendix B.06 — Current Code Architecture: `src/Main.Expeditions.cs`

### `src/Main.Expeditions.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 831 lines / 38747 bytes.
- SHA-256: `0e87d7d7f3605ba923f739c7bc36ab51edecc431781fe0014122a453422b300b`.
- Architecture signals: seeded references=2; save/restore symbols=7; typed event declarations=0; textual Godot mentions=13; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
internal void FlushTravelEncountersIfDirty() {
internal void FlushEncounterChoiceIfDirty() {
```


# Appendix B.07 — Current Code Architecture: `src/UI/ExpeditionPanel.cs`

### `src/UI/ExpeditionPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1418 lines / 66949 bytes.
- SHA-256: `197f8f4f3a68198d00c408ad97825a3658aa4ff5c983ae5353801cc62afc0f5a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=11; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class ExpeditionPanel : Control
public event Action? OnClose;
public event Action? OnExpeditionUpdated;
public event Action<List<ExpeditionLootEntry>>? OnLootDeposited;
public Label? EncounterTitleLabel => _encounterTitle;
public Label? EncounterContextLabel => _encounterContext;
public Label? EncounterBodyLabel => _encounterBody;
public Control? EncounterModal => _encounterModal;
public VBoxContainer? ChoicesContainer => _choicesContainer;
public ExpeditionEncounterBridge.EncounterSurfaced? LastSurfaced => _lastSurfaced;
public bool IsBound => _expeditionHost != null;
public void Bind( ExpeditionHostSession expeditionHost, SurvivorsHostSession? survivorsHost = null, InventoryHostSession? inventoryHost = null, Ashfall.Core.EquipmentConditionSystem? equipment = null, WorldHostSession? world = null)
public void Unbind() {
public void SetLightingPhase(string phase) => BackdropArt.SetTexture(this, BackdropArt.ExpeditionDepartureFor(phase));
public override void _Ready() {
public void RefreshView() {
public void Open() {
public void Close() {
public int TotalEncounterNotices { get; private set; }
public bool ChoiceButtonsRendered => _choicesContainer != null;
public void ShowEncounterNotice(ExpeditionEncounterBridge.EncounterSurfaced surfaced) {
public override void _Process(double delta) {
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
```


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/expeditions.json`

### `Assets/StreamingAssets/Data/expeditions.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 30255 bytes / 30255 characters.
- SHA-256: `0a79beaaeaa01fb21befbdeb3f2878b6716ee4bf8c974e5bf41a8c24ddc7468c`.
- Root keys: `expeditions`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
expeditions: min=75, max=75, observed_paths=1
expeditions[].lootCategories: min=4, max=4, observed_paths=2
```

Representative record fields:

- `baseStaminaDrainPerHour`
- `dangerLevel`
- `displayName`
- `distanceTicks`
- `encounterChancePerTick`
- `id`
- `lootCategories`
- `requiresDiscovery`
- `scavenging_table_id`

Representative identifiers (ordered, capped for readability):

```text
loc_the_allotments
loc_denial_cut_substation
suburban_house
rural_gas_station
concert_hall_ruins
family_bunker_backyard_shed
old_library_cache
ruined_garage
collapsed_building
loc_grange_hall
loc_apiary_rows
loc_school_gymnasium
loc_water_station
prewar_medical_cache
electrical_substation
checkpoint_kilo_armory
convoy_echo7_cache
loc_seed_library_annex
loc_veterinary_surgery
loc_cider_press
loc_ration_queue_plaza
loc_conscription_office
loc_municipal_archive
loc_dentists_row
loc_printworks
loc_weighbridge
loc_motel_verity
hospital_pharmacy
loc_terrace_pumphouse
loc_garrison_checkpoint_gamma
loc_grain_silo
abandoned_hospital
loc_transit_authority_hq
loc_department_store
loc_public_swimming_baths
loc_recovery_yard
loc_diesel_tank_farm
loc_radio_relay_mast
loc_st_brigids_almshouse
loc_ordnance_shoulder
loc_lock_gate_four
loc_pump_station_nine
loc_the_shallows_market
location_flooded_subway_depot
government_bunker
location_geo_thermal_plant_ruins
location_silent_observatory
location_arcology_sector_4
location_ministry_of_truth_bunker
location_the_dead_hand_core
loc_settlement_tinkers_notch
loc_settlement_pilgrim_hearth
loc_settlement_brine_pans
loc_forestry_compound
loc_warehouse_district
loc_vulcan_works_chemical
loc_north_freight_yard
loc_blackridge_ammunition_depot
loc_birchline_weather_station
loc_west_ridge_survey
loc_mirrim_forest_edge
loc_marsh_hollow
loc_charcoat_burns
loc_underground_fuel_depot
loc_municipal_seed_vault
loc_blacksite_armory_7
loc_excavation_command_vault
loc_deaddrop_command_shelter
loc_hidden_relay_bunker
loc_sealed_triage_annex
loc_evidence_sub_basement
loc_quarantine_barn
loc_forestry_emergency_store
loc_materials_research_sublevel
loc_electrical_maintenance_exchange
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/locations.json`

### `Assets/StreamingAssets/Data/locations.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 112815 bytes / 112801 characters.
- SHA-256: `97543ade61b6458f5b31bcffb85a96ad5d3b322b80294deb158d1ae29da3386b`.
- Root keys: `locations`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
locations: min=179, max=179, observed_paths=1
```

Representative record fields:

- `ambushFlag`
- `baseRadsPerHour`
- `cleanWaterRewardFlag`
- `dangerLevel`
- `description`
- `displayName`
- `id`
- `requiredFlagId`
- `travelHours`

Representative identifiers (ordered, capped for readability):

```text
abandoned_hospital
rural_gas_station
suburban_house
government_bunker
stranger_cache
location_geo_thermal_plant_ruins
location_arcology_sector_4
location_frozen_river_barge
location_crashed_icebreaker_convoy
location_silent_observatory
location_subterranean_seed_vault
location_ministry_of_truth_bunker
location_ash_dune_cemetery
location_abandoned_ski_resort
location_geothermal_borehole_site
location_flooded_subway_depot
location_sub_level_4_transit
location_municipal_sewage
location_collapsed_salt_mine
location_bio_remediation_lab
location_submerged_data_center
location_geothermal_vent_shaft
location_the_sump_cathedral
location_abandoned_desalination
location_deep_core_borehole
location_uxo_highway_choke
location_radar_array_spire
location_drone_hive_silo
location_automated_mortar_pit
location_scrap_neuromancer_camp
location_magnetic_anomaly_crater
location_abandoned_convoy_yard
location_acoustic_testing_facility
location_substation_omega
location_the_dead_hand_core
location_lethe_water_treatment
location_observatory_dome
location_submerged_arcology
location_concrete_batching_plant
location_seed_vault_antechamber
location_hospital_psych_wing
location_mirror_factory
location_radio_telescope_array
location_ash_whale_carcass
location_the_memory_vault
highway_pileup
prewar_medical_cache
loc_grange_hall
loc_apiary_rows
loc_seed_library_annex
loc_veterinary_surgery
loc_school_gymnasium
loc_cider_press
loc_terrace_pumphouse
loc_ration_queue_plaza
loc_conscription_office
loc_municipal_archive
loc_dentists_row
loc_transit_authority_hq
loc_printworks
loc_department_store
loc_public_swimming_baths
loc_st_brigids_almshouse
loc_weighbridge
loc_motel_verity
loc_bridge_seven
loc_recovery_yard
loc_ordnance_shoulder
loc_bus_reversal_loop
loc_diesel_tank_farm
loc_radio_relay_mast
loc_ash_sign_shrine
loc_pilgrim_switchbacks
loc_snowline_station
loc_low_background_lab
loc_ice_core_store
loc_avalanche_gallery
loc_summit_relay
loc_the_vessels_cell
loc_lock_gate_four
loc_pump_station_nine
loc_alloc_12b
loc_records_annex
loc_drowned_cinema
loc_cold_store_atlantic
loc_bathymetric_boat
loc_the_shallows_market
loc_the_allotments
checkpoint_kilo_armory
hospital_pharmacy
family_bunker_backyard_shed
old_library_cache
convoy_echo7_cache
raider_ambush_site
collapsed_building
raider_trap_location
electrical_substation
ruined_garage
concert_hall_ruins
loc_grain_silo
loc_garrison_checkpoint_gamma
loc_railway_span_44_alpha
loc_forward_roster_camp
loc_shrine_switchback_waystation
loc_understory_transmitter
loc_shelter_gate
loc_shelter_meeting
loc_shelter_infirmary
loc_shelter_storage
loc_shelter_quarters
loc_shelter_fire
loc_shelter_perimeter
loc_eastern_road
loc_neutral_ground
loc_water_station
loc_excavation_command_vault
loc_excavation_utility_tunnels
loc_excavation_metro_interchange
loc_excavation_mine_shaft
loc_excavation_archive_bunker
loc_excavation_drainage_network
loc_excavation_storage_chamber
loc_excavation_civilian_shelter
loc_hidden_relay_bunker
loc_logistics_reserve_cache
loc_deaddrop_command_shelter
loc_holdfast
loc_cut_radiation_zone_alpha
loc_cut_merchant_caravanserai
loc_cut_abandoned_depot
loc_cut_arsenal_ruin
loc_black_flotilla_outpost
loc_settlement_brine_pans
loc_settlement_iron_siding
loc_settlement_cape_beacon
loc_settlement_slate_hollow
loc_settlement_pilgrim_hearth
loc_settlement_tinkers_notch
location_quarry_overlook
loc_grain_exchange
loc_automated_abattoir
loc_flooded_subway_depot
loc_scavenger_camp
loc_iron_garrison
loc_dead_zone
loc_settlement_ferry_crossing
loc_settlement_nine_rails
loc_settlement_fort_karkov
loc_settlement_lock_seven
loc_settlement_silo_burrow
loc_settlement_st_nicholas
loc_iron_crest
loc_ash_needle
loc_wind_gap_ridge
loc_signal_hill_tower
loc_river_bend_outpost
loc_rusted_span_bridge
loc_old_crematory_stacks
loc_north_gate_water_tower
loc_junction_box_rail
```


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/scavenging_tables.json`

### `Assets/StreamingAssets/Data/scavenging_tables.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 146802 bytes / 146778 characters.
- SHA-256: `e8362d3a73c64ed1000cdd3e5ae9200a955c6fd93f9183b91362ea0cbaec3e41`.
- Root keys: `collection_id`, `schema_version`, `tables`.

Array-path census (minimum, maximum, observed rows):

```text
tables: min=54, max=54, observed_paths=1
tables[].entries: min=13, max=19, observed_paths=2
```

Representative record fields:

- `base_hazard_chance`
- `depletion_model`
- `description`
- `display_name`
- `entries`
- `id`
- `location_type`
- `primary_hazard_type`

Representative identifiers (ordered, capped for readability):

```text
table_loot_hospital
table_loot_rail_yard
table_loot_school
table_loot_military_depot
table_loot_apartment_block
table_loot_fire_station
table_loot_metro_station
table_loot_police_station
table_loot_industrial_district
table_loot_shopping_center
table_loot_power_substation
table_loot_chemical_plant
table_loot_warehouse
table_loot_farm
table_loot_forestry_compound
table_loot_hunting_cabin
table_loot_monastery
table_loot_clinic
table_loot_observatory
table_loot_greenhouse
table_loot_veterinary_surgery
table_loot_dentists_row
table_loot_hospice_ward
table_loot_collapsed_structure
table_loot_weighbridge
table_loot_recovery_yard
table_loot_tank_farm
table_loot_concert_hall
table_loot_ration_plaza
table_loot_checkpoint
table_loot_conscription_office
table_loot_ordnance_shoulder
table_loot_relay_mast
table_loot_transit_depot
table_loot_geo_thermal_plant
table_loot_arcology_sector_4
table_loot_tinkers_notch
table_loot_waterworks
table_loot_swimming_baths
table_loot_apiary_rows
table_loot_convoy_cache
table_loot_municipal_archive
table_loot_printworks
table_loot_ministry_bunker
table_loot_government_bunker
table_loot_dead_hand_core
table_loot_shallows_market
table_loot_pilgrim_hearth
table_loot_brine_pans
table_loot_weather_station
table_loot_geological_survey
table_loot_forest_edge
table_loot_frozen_wetland
table_loot_burned_woodland
```


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/micro_locations.json`

### `Assets/StreamingAssets/Data/micro_locations.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 35356 bytes / 35344 characters.
- SHA-256: `8d6098ece0060ef012e5f3871c324f606ca520a844c14da2ed614187b80820a0`.
- Root keys: `collection_id`, `encounters`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
encounters: min=28, max=28, observed_paths=1
encounters[].choices: min=2, max=3, observed_paths=2
```

Representative record fields:

- `baseWeight`
- `category`
- `choices`
- `description`
- `id`
- `minDangerLevel`
- `requiredLocationId`
- `speedWeightMultiplier`
- `stealthWeightMultiplier`
- `title`

Representative identifiers (ordered, capped for readability):

```text
micro_roadside_memorial
micro_crashed_truck
micro_frozen_bus
micro_improvised_grave
micro_collapsed_bridge
micro_drainage_pipe
micro_rail_siding
micro_dead_livestock
micro_ruined_greenhouse
micro_shell_crater
micro_field_kitchen
micro_abandoned_generator
micro_shrine
micro_emergency_cache
micro_observation_post
micro_abandoned_barricade
micro_hunting_blind
micro_radio_tower
micro_destroyed_checkpoint
micro_abandoned_tent
micro_makeshift_clinic
micro_crashed_drone
micro_fuel_cache
micro_water_source
micro_supply_drop
micro_hospital_chapel_ledger
micro_depot_undertow_raft_line
micro_gamma_levy_board
```


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Expeditions/Plan32ExpeditionDestinationWiringTests.cs`

### `Ashfall.Core.Tests/Expeditions/Plan32ExpeditionDestinationWiringTests.cs`

- Current test declarations: Fact=5, Theory=1, InlineData=8.
- File lines: 236; SHA-256: `4b0e06c9c762217970c71c6de8a89b50502bb24ca2a39c18e7decd9c30f18d27`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ExpeditionsCatalog_LoadsAllAuthoredDestinations
Expeditions_AllAuthoredDestinationsHaveUniqueCanonicalIdsAndValidBounds
Expeditions_OriginalTwoRecordsArePreserved
Expeditions_TierDistributionMatchesPlan
RepresentativeDestinations_CanDispatchAndComplete
MidExpeditionSaveAndRestore_MaintainsDestinationIntegrity
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Expeditions/Plan76DestinationLootReferenceTests.cs`

### `Ashfall.Core.Tests/Expeditions/Plan76DestinationLootReferenceTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 209; SHA-256: `d672ccee8fcf0e53fadc4c95d84bbe2d80d2362d48d671f4568a9ff5ca2aa71c`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AuthoredCatalog_LoadsAndKeepsOriginalTwoParity
AllLootCategories_ResolveAgainstMergedItemCatalog
AllScavengingTableReferences_ResolveAgainstPlan46Authority
AuthoredCatalog_FullyMigrated_NoDestinationLeftUnbound
RepairedReferences_NeverRegress
```


# Appendix E.14 — Supporting Code Evidence: `src/Main.UiTests.Expeditions.cs`

### `src/Main.UiTests.Expeditions.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 299 lines / 16836 bytes.
- SHA-256: `e84e7a43853072820193ea1173394bfc2d756490ad7c0deb386f94091656bd7c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix E.15 — Supporting Code Evidence: `src/Audio/AudioEventBridge.cs`

### `src/Audio/AudioEventBridge.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 633 lines / 26686 bytes.
- SHA-256: `840935ff49fb41437d3afaf2892e049e4a0f298cc25ae16650b66df0c07139e7`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public interface IAudioDomainProvider
public sealed class AudioEventBridge : IDisposable
public void SubscribeAll( RadiationSystem? radiation = null, WeatherSystem? weather = null, TacticalCombatSystem? combat = null, CraftingSystem? crafting = null, ExpeditionSystem? expeditions = null,
public void BindRadiation(RadiationSystem? radiation) {
public void BindWeather(WeatherSystem? weather) {
public void BindCombat(TacticalCombatSystem? combat) {
public void BindCrafting(CraftingSystem? crafting) {
public void BindExpeditions(ExpeditionSystem? expeditions) {
public void BindDisease(DiseaseSystem? disease) {
public void BindSurvivorFate(SurvivorFateSystem? survivorFate) {
public void BindFlashbacks(SomaticFlashbackSystem? flashbacks) {
public void BindEchoes(EchoSystem? echoes) {
public void NotifyGameFlow(string cueId) {
public void Dispose() {
internal bool HasRadiationBinding => _radiation != null;
internal bool HasWeatherBinding => _weather != null;
internal bool HasCombatBinding => _combat != null;
internal bool HasCraftingBinding => _crafting != null;
internal bool HasExpeditionsBinding => _expeditions != null;
internal bool HasDiseaseBinding => _disease != null;
internal bool HasSurvivorFateBinding => _survivorFate != null;
internal bool HasFlashbacksBinding => _flashbacks != null;
internal bool HasEchoesBinding => _echoes != null;
```


# Appendix E.16 — Supporting Code Evidence: `src/Host/HostCli.PanelTests.cs`

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


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/Expeditions/ArmoredCrawlerExpeditionSystem.cs`

### `Assets/Ashfall.Core/Expeditions/ArmoredCrawlerExpeditionSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 401 lines / 14627 bytes.
- SHA-256: `820c63d9d7872ae352eaeedf3b1164ea0ca3b16756fe65cec67f353604970b6e`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RemoteCampState
public string campId = string.Empty;
public string locationId = string.Empty;
public string crawlerId = string.Empty;
public int establishedDay;
public bool hasWorkshop;
public float defenseBonus;
public RemoteCampState Clone() {
public sealed class ArmoredCrawlerState
public string crawlerId = string.Empty;
public string chassisId = "crawler_heavy_chassis_mk1";
public List<string> installedModuleIds = new List<string>();
public float hullIntegrity = 100.0f; // 0..100
public float trackCondition = 100.0f; // 0..100
public float fuelOnboard = 150.0f;
public List<string> crewRoster = new List<string>();
public string currentRouteId = string.Empty;
public string currentLocationId = string.Empty;
public bool isImmobilized;
public string deployedCampId = string.Empty;
public int maxSlots = 6;
public float maxMass = 3500.0f;
public ArmoredCrawlerState Clone() {
public sealed class ArmoredCrawlerExpeditionSave
public List<ArmoredCrawlerState> crawlers = new List<ArmoredCrawlerState>();
public List<RemoteCampState> remoteCamps = new List<RemoteCampState>();
public int lastTickDay;
public ArmoredCrawlerExpeditionSave Clone() {
public sealed class ArmoredCrawlerExpeditionSystem
public IReadOnlyList<ArmoredCrawlerState> Crawlers => _crawlers;
public IReadOnlyList<RemoteCampState> RemoteCamps => _remoteCamps;
public int LastTickDay => _lastTickDay;
public event Action<string, string>? OnModuleInstalled;
public event Action<string, string>? OnModuleRemoved;
public event Action<string>? OnTrackThrown;
public event Action<string>? OnCrawlerRepaired;
public event Action<string, string>? OnCampDeployed;
public event Action<string>? OnCampDismantled;
public ArmoredCrawlerState? GetCrawler(string crawlerId) {
public float ComputeTotalMass(string crawlerId) {
public int GetEffectiveCrewBerths(string crawlerId) {
public bool HasWorkshopCapability(string crawlerId) {
public bool CanTraverseTerrain(string crawlerId, string terrainTag) {
public bool TryInstallModule(string crawlerId, string moduleId) {
public bool TryUninstallModule(string crawlerId, string moduleId) {
public ExpeditionVehicleProfile ProjectToVehicleProfile(string crawlerId) {
public bool TryRepairTrack(string crawlerId) {
public bool TryDeployCamp(string crawlerId, string locationId) {
public bool TryDismantleCamp(string crawlerId) {
public void TickDay(int currentDay, ISeededRng? tickRng = null) {
public ArmoredCrawlerExpeditionSave CaptureState() {
public void RestoreState(ArmoredCrawlerExpeditionSave? save) {
```


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/Survivors/NeedsPerformanceBridge.cs`

### `Assets/Ashfall.Core/Survivors/NeedsPerformanceBridge.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 518 lines / 20468 bytes.
- SHA-256: `c54ae9e5188ec63c9df278d975d98016c6306d9d5cea6cc9ba54e110816ac2c6`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum PerformanceBand
public sealed class NeedPerformanceContribution
public NeedKind Need { get; }
public float RawValue { get; }
public float NormalizedSeverity { get; }
public PerformanceBand Band { get; }
public float CombatAccuracyMod { get; }
public float CombatDamageMod { get; }
public float WorkSpeedMod { get; }
public float ExpeditionSpeedMod { get; }
public float StaminaDrainMod { get; }
public string ReasonKey { get; }
public sealed class NeedsPerformanceModifiers
public float CombatAccuracyMultiplier { get; }
public float CombatDamageMultiplier { get; }
public float WorkSpeedMultiplier { get; }
public float ExpeditionSpeedMultiplier { get; }
public float ExpeditionStaminaDrainMultiplier { get; }
public PerformanceBand OverallBand { get; }
public IReadOnlyList<NeedPerformanceContribution> Contributions { get; }
public static NeedsPerformanceModifiers Neutral { get; } = new NeedsPerformanceModifiers(
public struct NeedsPerformanceCensus
public readonly int OptimalCount;
public readonly int ImpairedCount;
public readonly int SevereCount;
public readonly int CriticalCount;
public int TotalSurvivorsEvaluated => OptimalCount + ImpairedCount + SevereCount + CriticalCount;
public interface ICombatPerformanceModifier
public interface IWorkEfficiencyModifier
public interface IExpeditionPerformanceModifier
public sealed class NeedsPerformanceConfig
public float MinCombatAccuracyMultiplier { get; set; } = 0.25f;
public float MinCombatDamageMultiplier { get; set; } = 0.30f;
public float MinWorkSpeedMultiplier { get; set; } = 0.20f;
public float MinExpeditionSpeedMultiplier { get; set; } = 0.30f;
public float MaxExpeditionStaminaDrainMultiplier { get; set; } = 2.50f;
public float DemoralizedThreshold { get; set; } = 30.0f;
public float DemoralizedPenaltyAmplification { get; set; } = 0.10f;
public float HighMoraleThreshold { get; set; } = 70.0f;
public float HighMoralePenaltyMitigation { get; set; } = 0.10f;
public static NeedsPerformanceConfig Default { get; } = new NeedsPerformanceConfig();
public static class NeedsPerformanceBridge
public static NeedsPerformanceConfig ActiveConfig => s_config;
public static void SetConfig(NeedsPerformanceConfig config) {
public static void LoadFromJson(string json) {
public static NeedsPerformanceModifiers Project(SurvivorNeedsState? state) {
public static NeedsPerformanceModifiers Project( float hunger, float thirst, float fatigue, float warmth, float morale)
```


# Appendix G.19 — Supporting Regression Evidence: `Ashfall.Core.Tests/ExpeditionSystemTests.cs`

### `Ashfall.Core.Tests/ExpeditionSystemTests.cs`

- Current test declarations: Fact=17, Theory=0, InlineData=0.
- File lines: 273; SHA-256: `7069804fc263d254a1e05c3155030343028fc64c988e9fe776359cc05721d311`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Start_GuardsInvalidInputs
Start_OneExpeditionPerSurvivor
Outbound_ArrivesAfterDistanceTicks
SpeedStance_TravelsOneAndAHalfTimesFaster
Looting_AutoRetreatsAfterThreeTicksUnlessPushingLuck
PushLuck_ExtendsLooting
Loot_IsCappedByCarryingCapacity
Completion_ReturnsToShelterAndClearsActive
StaminaExhaustion_FailsTheExpedition
Determinism_SameSeedSameLoot
Encounters_RollOnEveryLegIncludingTravel
ExpeditionId_IsUniquePerSurvivorAndTarget
Retreat_RaisesStateChanged
CaptureState_ReturnsSnapshotNotLiveState
CaptureState_EmitsInOrdinalOrder
SaveLoad_RoundTripsAllState
SaveLoad_ChecksumStable
```


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/ExpeditionCampSystemTests.cs`

### `Ashfall.Core.Tests/ExpeditionCampSystemTests.cs`

- Current test declarations: Fact=30, Theory=0, InlineData=0.
- File lines: 472; SHA-256: `d85cba9f68e26cd01a8bf254d0fae2a7001f240a84333f28ffd6187672a85735`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
EnterCamp_OnlyFromOutbound
EnterCamp_RejectsLootingPhase
EnterCamp_SetsPhaseToCamp
EnterCamp_RaisesOnCampEntered
CampTick_ConsumesFirewood
CampTick_ConsumesWaterAndFood
CampTick_ReturnsTrueWhenDawn
CampTick_NightSegmentsCompletedIncrements
CampTick_RaisesOnCampNightSegmentResolved
CampTick_ColdExposureAccumulatesBelowThreshold
CampTick_NoColdExposureWithFireAndShelter
CampTick_RecoversStamina
CampTick_CanTriggerEncounter
CampTick_SentryReducesEncounterChance
ResolveCampEncounter_RequiresUnresolvedEncounter
ResolveCampEncounter_MarksResolved
ResolveCampEncounter_InjuryReducesStamina
BreakCamp_RequiresDawn
BreakCamp_ResumesOutbound
BreakCamp_RetreatsToInbound
BreakCamp_SetsCampOutcome
BreakCamp_RaisesOnCampDawnResolved
CampState_SurvivesSaveLoad
CampState_ChecksumStable
CampState_CollectionOrderStable
CampTick_SameSeedSameOutcome
TickHours_SkipsCampPhase
ReserveCampSupplies_UpdatesQuantities
GetCampState_ReturnsNullWhenNotInCamp
GetCampState_ReturnsNullForUnknownSurvivor
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/Expeditions/C2AmputationTravelTests.cs`

### `Ashfall.Core.Tests/Expeditions/C2AmputationTravelTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 127; SHA-256: `294d1e4ca5fdea77bc61ab57dc48fc8d41409a81809bb06b400df8afcc0b3a33`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ImplicitAndExplicitOne_AreIntactParity
AmputatedLeg_SlowsTravel_AndRecordsFactor
MedicalOwner_MovementFactor_FeedsTheEstimate
Start_SamplesFactor_OntoState_AndLegacyStateDefaultsToOne
Runtime_AppliesFactorOncePerTravelStep
Factor_IsBounded_AndAbsentMeansIntact
State_RoundTripsThroughSerializer
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs`

### `Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs`

- Current test declarations: Fact=17, Theory=0, InlineData=0.
- File lines: 493; SHA-256: `2f574c226a8a522a1d4925d66d9c507ecb75d09ebb8593d907b37741f4991752`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
VehicleSpeed_ShortensOutboundTravel
VehicleBreakdown_IsDeterministic_AndRevertsToFoot
VehicleBreakdown_FiresEventOnce
NeutralVehicle_NeverConsumesBreakdownRolls
Estimate_MirrorsTickMath_ForFootAndVehicle
Bridge_ProjectsAuthorityConditionIntoCombatToken
Bridge_PrefersOwnerAndBestCondition
Bridge_WriteBack_ConvertsUnits_ThroughAuthority
Bridge_Readiness_FollowsAuthorityRisks
AggregateCodec_RoundTripsExpeditionsAndGarage
RestoreCompletedCount_SurvivesAggregateRestore
AggregateCodec_MigratesLegacyEnvelopeAndBareList
AggregateState_RestoreContinuesInFlightVehicleExpedition
VehicleCatalogLoader_ReadsDataAuthority_AndToleratesAbsence
TrackGear_InstallsRepairsAndPersistsNormalizedEffects
TrackGear_ProfileProjectionKeepsTravelMathInCore
TrackGear_RejectsInvalidFactsWithoutMutation
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
| authored destination definitions and registry | ExpeditionCatalogLoader/ExpeditionSystem | dispatch, travel, encounters and state | ExpeditionSystem | Owner emits/reads a typed fact; no mirror state. |
| authored destination definitions and registry | ExpeditionCatalogLoader/ExpeditionSystem | loot table references | ScavengingTableCatalog | Owner emits/reads a typed fact; no mirror state. |
| authored destination definitions and registry | ExpeditionCatalogLoader/ExpeditionSystem | route projection and dispatch commands | Expedition host/UI | Owner emits/reads a typed fact; no mirror state. |
| authored destination definitions and registry | ExpeditionCatalogLoader/ExpeditionSystem | wiring, references and restore | Expedition focused tests | Owner emits/reads a typed fact; no mirror state. |
| dispatch, travel, encounters and state | ExpeditionSystem | authored destination definitions and registry | ExpeditionCatalogLoader/ExpeditionSystem | Owner emits/reads a typed fact; no mirror state. |
| dispatch, travel, encounters and state | ExpeditionSystem | loot table references | ScavengingTableCatalog | Owner emits/reads a typed fact; no mirror state. |
| dispatch, travel, encounters and state | ExpeditionSystem | route projection and dispatch commands | Expedition host/UI | Owner emits/reads a typed fact; no mirror state. |
| dispatch, travel, encounters and state | ExpeditionSystem | wiring, references and restore | Expedition focused tests | Owner emits/reads a typed fact; no mirror state. |
| loot table references | ScavengingTableCatalog | authored destination definitions and registry | ExpeditionCatalogLoader/ExpeditionSystem | Owner emits/reads a typed fact; no mirror state. |
| loot table references | ScavengingTableCatalog | dispatch, travel, encounters and state | ExpeditionSystem | Owner emits/reads a typed fact; no mirror state. |
| loot table references | ScavengingTableCatalog | route projection and dispatch commands | Expedition host/UI | Owner emits/reads a typed fact; no mirror state. |
| loot table references | ScavengingTableCatalog | wiring, references and restore | Expedition focused tests | Owner emits/reads a typed fact; no mirror state. |
| route projection and dispatch commands | Expedition host/UI | authored destination definitions and registry | ExpeditionCatalogLoader/ExpeditionSystem | Owner emits/reads a typed fact; no mirror state. |
| route projection and dispatch commands | Expedition host/UI | dispatch, travel, encounters and state | ExpeditionSystem | Owner emits/reads a typed fact; no mirror state. |
| route projection and dispatch commands | Expedition host/UI | loot table references | ScavengingTableCatalog | Owner emits/reads a typed fact; no mirror state. |
| route projection and dispatch commands | Expedition host/UI | wiring, references and restore | Expedition focused tests | Owner emits/reads a typed fact; no mirror state. |
| wiring, references and restore | Expedition focused tests | authored destination definitions and registry | ExpeditionCatalogLoader/ExpeditionSystem | Owner emits/reads a typed fact; no mirror state. |
| wiring, references and restore | Expedition focused tests | dispatch, travel, encounters and state | ExpeditionSystem | Owner emits/reads a typed fact; no mirror state. |
| wiring, references and restore | Expedition focused tests | loot table references | ScavengingTableCatalog | Owner emits/reads a typed fact; no mirror state. |
| wiring, references and restore | Expedition focused tests | route projection and dispatch commands | Expedition host/UI | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 2→50 count target with a 75-row destination census and reference-family matrix. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Document the difference between canonical locations, dispatchable destinations, micro locations and route-only nodes. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Require every destination to have valid bounds, a current consumer and an explicit reference resolution path. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Preserve mid-expedition save/restore and scavenging behavior as the real integration proof. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.555 — Additional Current Architecture Evidence: `src/Main.CampaignOwners.cs`

### `src/Main.CampaignOwners.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 2957 lines / 146936 bytes.
- SHA-256: `6c612e267459f02941f6c11c6536eaba89417aff5cf2a99aa28350aba04b7930`.
- Architecture signals: seeded references=0; save/restore symbols=121; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* retention is idempotent; captured via save section */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* derived read projection */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) => _m._commitments?.System.CapturePreDaySnapshot(day);
public void RestorePreDaySnapshot(int day) => _m._commitments?.System.RestorePreDaySnapshot(day);
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* no snapshot: campaign days are day-local */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* jobs are day-local; capture via save section */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* no snapshot: hazards are day-local */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
```


# Appendix Q.556 — Additional Current Architecture Evidence: `src/Main.PlayerSurfaces.cs`

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


# Appendix Q.557 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/ExpeditionHeadlessDemo.cs`

### `Assets/Ashfall.Core/Expeditions/ExpeditionHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 119 lines / 5168 bytes.
- SHA-256: `66658ebea87955978d19fbb429516d69cfc235d01ff57ed8465cb1716dad114c`.
- Architecture signals: seeded references=11; save/restore symbols=5; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class ExpeditionHeadlessDemo
public static HeadlessReport Run(ILog? log = null) {
```


# Appendix Q.558 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/WastelandMapSystem.cs`

### `Assets/Ashfall.Core/World/WastelandMapSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1274 lines / 50735 bytes.
- SHA-256: `4edb79c28b4d8baec664fdac3bdcbc2084b4bbd5524f48dec1f6e2e1029bdce3`.
- Architecture signals: seeded references=0; save/restore symbols=8; typed event declarations=21; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WastelandMapSystem
public TunnelNetworkSystem Tunnels { get; }
public event Action<string>? OnNodeDiscovered;
public event Action<string, MapFogState>? OnNodeKnowledgeChanged;
public event Action<string>? OnNodeCompleted;
public event Action<string, bool>? OnNodeLockChanged;
public event Action? OnMarkersChanged;
public WastelandMapState State => _state;
public IReadOnlyList<MapNode> Nodes => _nodes;
public IReadOnlyList<MapRoute> Routes => _routes;
public IReadOnlyList<string> DiscoveredNodes => _state.Discovered;
public IReadOnlyList<string> CompletedNodes => _state.Completed;
public IReadOnlyList<string> LockedNodes => _state.Locked;
public IReadOnlyList<MapNodeKnowledgeState> Knowledge => _state.Knowledge;
public IReadOnlyList<MapMarkerState> Markers => _state.Markers;
public void RegisterTrapSiteLocation(TrapSiteMapLocation location) {
public bool TryResolveTrapSitePosition(string siteId, out float positionX, out float positionY) {
public bool UpsertTrapMarker(string siteId, string trapId, string trapType, float positionX, float positionY, bool broken) {
public bool EnsureTrapMarker(string siteId, string trapId, string trapType, bool broken) {
public bool RemoveTrapMarker(string siteId) {
public void ReconcileTrapMarkers(IEnumerable<TrapMapMarkerSource> activeSources) {
public static string TrapMarkerId(string siteId) => $"trap:{siteId ?? string.Empty}";
public bool IsDiscovered(string nodeId) {
public MapFogState GetFogState(string nodeId) {
public MapNodeKnowledgeState? GetNodeKnowledge(string nodeId) {
public bool Discover(string nodeId) => DiscoverVisited(nodeId, "legacy", 1);
public bool DiscoverRumor(string nodeId, string sourceId, int day, InformationConfidence confidence = InformationConfidence.Medium) {
public bool DiscoverSurvey(string nodeId, string surveySourceId, int day, IEnumerable<string>? traits = null) {
public bool DiscoverVisited(string nodeId, string survivorId, int day) {
public MapNodeIntelView? GetNodeIntel(string nodeId) {
public bool IsCompleted(string nodeId) {
public bool Complete(string nodeId) {
public bool IsLocked(string nodeId) {
public bool SetLocked(string nodeId, bool locked) {
public bool Unlock(string nodeId) => SetLocked(nodeId, false);
public bool Lock(string nodeId) => SetLocked(nodeId, true);
public MapNodeStatusKind ResolveNodeStatus(string nodeId) {
public MapNode? GetNode(string nodeId) {
public IReadOnlyList<MapRoute> GetRoutesFrom(string nodeId) {
public MapRoute? GetRoute(string fromId, string toId) {
public bool IsRouteFlooded(string fromId, string toId) {
public bool HasRouteTag(string fromId, string toId, string tag) {
public List<string> PlanRoute(string fromId, string toId) {
public LivingMapRouteProjection ProjectLivingMapRoute(string fromId, string toId) {
public ExpeditionEstimate EstimateRoute( ExpeditionDefinition def, ExpeditionStance stance = ExpeditionStance.Stealth, bool isNightScavenge = false, ExpeditionVehicleProfile? vehicle = null, float weaponReadiness = 1f,
public ExpeditionEstimate? EstimateRoute( string fromId, string toId, ExpeditionStance stance = ExpeditionStance.Stealth, bool isNightScavenge = false, ExpeditionVehicleProfile? vehicle = null,
public WastelandMapState CaptureState() {
public void RestoreState(WastelandMapState state) {
public void EnsureCanonicalTunnels() {
public bool DiscoverTunnel(string segmentId) {
public bool RepairTunnel(string segmentId, float repairAmount = 50f) {
public IReadOnlyList<TunnelSegment> GetDiscoveredTunnels() => Tunnels.GetDiscoveredSegments();
public enum MapFogState
public sealed class MapNodeKnowledgeState
public string NodeId = string.Empty;
public MapFogState FogState = MapFogState.Unknown;
public CampaignProvenanceRecord? Provenance;
public int LastConfirmedDay;
public List<string> Traits = new List<string>();
public MapNodeKnowledgeState Clone() => new MapNodeKnowledgeState
public sealed class MapNodeIntelView
public string NodeId { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public MapFogState FogState { get; set; }
public float PositionX { get; set; }
public float PositionY { get; set; }
public MapNodeDanger Danger { get; set; }
public string DangerBand { get; set; } = string.Empty;
public string FactionId { get; set; } = string.Empty;
public string LootDescription { get; set; } = string.Empty;
public IReadOnlyList<string> Traits { get; set; } = Array.Empty<string>();
public CampaignProvenanceRecord? Provenance { get; set; }
public int LastConfirmedDay { get; set; }
public bool Routable { get; set; }
public enum MapNodeStatusKind
public sealed class MapNode
public string Id;
public string DisplayName;
public MapNodeDanger Danger;
public string FactionId;
public string LootTableId;
public float PositionX;
public float PositionY;
public bool Discoverable;
public bool StartingUnlocked;
public enum MapNodeDanger
public sealed class MapRoute
public string From;
public string To;
public float DistanceKm;
public float WeatherHazard;
public string TravelDomain = "land";
public float CurrentStrength = 0f;
public float ToxicContamination = 0f;
public List<string> Tags = new List<string>();
public bool HasTag(string tag) {
public bool IsFlooded => HasTag("flooded");
public bool IsAmphibious => HasTag("amphibious");
public sealed class WastelandMapState
public List<string> Discovered = new List<string>();
public List<string> Completed = new List<string>();
public List<string> Locked = new List<string>();
public List<string> Unlocked = new List<string>();
public List<string> RegisteredMapFragments = new List<string>();
public List<MapNodeKnowledgeState> Knowledge = new List<MapNodeKnowledgeState>();
public List<MapMarkerState> Markers = new List<MapMarkerState>();
public TunnelNetworkState Tunnels = new TunnelNetworkState();
public void NormalizeAndValidate(IReadOnlyList<MapNode> nodes) {
public WastelandMapState Capture() => new WastelandMapState
public void RestoreInto(WastelandMapState state, IReadOnlyList<MapNode> nodes) {
public sealed class MapMarkerState
public string MarkerId = string.Empty;
public string Category = string.Empty;
public string SourceId = string.Empty;
public string DefinitionId = string.Empty;
public string TrapType = string.Empty;
public string LabelKey = string.Empty;
public string IconKey = string.Empty;
public string Condition = "healthy";
public float PositionX;
public float PositionY;
public MapMarkerState Clone() => new MapMarkerState
public sealed class TrapMapMarkerSource
public string SiteId = string.Empty;
public string TrapId = string.Empty;
public string TrapType = string.Empty;
public float PositionX;
public float PositionY;
public bool IsBroken;
public sealed class TrapSiteMapLocation
public string SiteId = string.Empty;
public string AnchorNodeId = string.Empty;
public float OffsetX;
public float OffsetY;
```


# Appendix Q.559 — Additional Current Architecture Evidence: `src/Host/HostCli.WorldPlaytest.cs`

### `src/Host/HostCli.WorldPlaytest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1008 lines / 56167 bytes.
- SHA-256: `96d58936693cbbb9fc62a949e176a97480f7ce3654f83151cec2af518576e3c9`.
- Architecture signals: seeded references=8; save/restore symbols=26; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunWorldPlaytestSelfTest(string dataDirectory, Main? productionMain = null) {
public string id = string.Empty;
public bool passed;
public string evidence = string.Empty;
public int schema_version = 1;
public string source_commit = string.Empty;
public string day_owner = "world_evolution";
public int master_seed;
public int snapshot_count;
public List<WorldPlaytestBand> target_bands = new List<WorldPlaytestBand>();
public int first_migration_day = -1;
public int first_degradation_transition_day = -1;
public float wildlife_density_min;
public float wildlife_density_max;
public float wildlife_density_delta;
public float encounter_multiplier_min;
public float encounter_multiplier_max;
public float canned_food_scarcity_min;
public float canned_food_scarcity_max;
public int briefing_event_count;
public int journal_event_count;
public int radio_event_count;
public int trapping_density_reads;
public int encounter_multiplier_reads;
public int dead_seed_count;
public bool same_seed_byte_equal;
public bool midpoint_save_load_byte_equal;
public bool different_seed_diverged;
public bool duplicate_narrative_events_after_restore;
public List<WorldPlaytestInfluenceRow> influence_map = new List<WorldPlaytestInfluenceRow>();
public List<WorldPlaytestCheck> checks = new List<WorldPlaytestCheck>();
public List<WorldPlaytestSnapshot> snapshots = new List<WorldPlaytestSnapshot>();
public string tuning_decision = string.Empty;
public string measure = string.Empty;
public string band = string.Empty;
public string producer = string.Empty;
public string value = string.Empty;
public string consumer = string.Empty;
public string read_cadence = string.Empty;
public string player_visible_consequence = string.Empty;
public int day;
public float global_wildlife_ratio;
public float total_wildlife_density;
public List<WorldPlaytestLocation> locations = new List<WorldPlaytestLocation>();
public List<WorldPlaytestWildlife> wildlife = new List<WorldPlaytestWildlife>();
public WorldPlaytestEconomy economy = new WorldPlaytestEconomy();
public List<WorldPlaytestSurfaceEvent> surfaced_events = new List<WorldPlaytestSurfaceEvent>();
public string location_id = string.Empty;
public string owner = string.Empty;
public string degradation_tier = string.Empty;
public float contamination;
public float loot_depletion;
public float encounter_multiplier;
public string pack_id = string.Empty;
public string species_id = string.Empty;
public string region_id = string.Empty;
public int density;
public float starvation;
public bool rabid;
public float canned_food_scarcity_proxy;
public List<WorldPlaytestGood> scarcity_goods = new List<WorldPlaytestGood>();
public string item_id = string.Empty;
public float demand_multiplier;
public string channel = string.Empty;
public string kind = string.Empty;
public string primary_id = string.Empty;
public string secondary_id = string.Empty;
public float numeric;
public string expedition_id = string.Empty;
public string location_id = string.Empty;
public int phase;
public int schema_version = 1;
public WorldWeatherState weather = new WorldWeatherState();
public LocationEvolutionSaveState locations = new LocationEvolutionSaveState();
public WildlifeSaveState wildlife = new WildlifeSaveState();
public LandmarkSaveState landmarks = new LandmarkSaveState();
public MarketState market = new MarketState();
public WildlifeTrappingState trapping = new WildlifeTrappingState();
public List<ExpeditionState> expeditions = new List<ExpeditionState>();
public int expedition_completed_count;
public CampaignDaySave coordinator = new CampaignDaySave();
public List<string> previous_sectors = new List<string>();
public List<string> surfaced_event_keys = new List<string>();
public int last_trapping_catch;
public WeatherSystem Weather { get; }
public LocationEvolutionSystem Locations { get; }
public WildlifeMigrationSystem Wildlife { get; }
public LandmarkDegradationSystem Landmarks { get; }
public MarketSystem Market { get; }
public WildlifeTrappingSystem Trapping { get; }
public ExpeditionSystem Expeditions { get; }
public CampaignDayCoordinator Coordinator { get; }
public List<WorldPlaytestSnapshot> Snapshots { get; } = new List<WorldPlaytestSnapshot>();
public int Migrations { get; private set; }
public int DegradationTransitions { get; private set; }
public int TrappingDayTicks { get; private set; }
public int TrappingDensityReads { get; private set; }
public int EncounterMultiplierReads { get; private set; }
public int EncounterEvaluationTicks { get; private set; }
public int SurfacedBriefingEvents { get; private set; }
public int SurfacedJournalEvents { get; private set; }
public int SurfacedRadioEvents { get; private set; }
public bool NoDuplicateNarrativeEvents { get; private set; } = true;
public int DeadSeedCount { get; private set; }
public static WorldPlaytestRun Create(string dataDirectory, int seed) {
public void AdvanceThrough(int firstDay, int lastDay) {
public WorldPlaytestSave CaptureSave() {
public void RestoreSave(WorldPlaytestSave save) {
public float EncounterMultiplierMovement => Snapshots.Count == 0
public float ScarcityMovement => Snapshots.Count < 2
public bool AllSnapshotsSane() {
public bool NoDuplicateSnapshotRecords() {
public bool RuinedStatesAreSticky() {
public WorldPlaytestArtifact BuildArtifact(string dataDirectory, List<WorldPlaytestCheck> checks, bool sameSeedEqual, bool midpointEqual, bool differentSeedDiverged) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
```


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs`

### `Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 354 lines / 16880 bytes.
- SHA-256: `a9653182ddac1d1221b396712dbedb33cb2629671656027bb3421b1a437ad88e`.
- Architecture signals: seeded references=6; save/restore symbols=0; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpeditionEncounterBridge
public sealed class EncounterSurfaced
public string encounter_id;
public string title;
public string description;
public string category;
public List<EncounterChoiceDefinition> choices;
public bool is_patrol;
public bool is_micro_location;
public string faction_id;
public string territory_state;
public string patrol_archetype;
public string recognition_label;
public int patrol_chain_stage;
public ExpeditionState trigger;
public bool? resolved_at_lead;
public string encounter_record_resolution_id;
public event Action<EncounterSurfaced> OnSurfaced;
public TravelEncounterSystem? TravelEngine { get; set; }
public int CurrentDay { get; set; } = 1;
public string CurrentSeason { get; set; } = "all";
public Func<string, string>? RegionResolver { get; set; }
public EncounterSurfaced? LastSurfaced => _lastSurfaced;
public NarrativeEncounterResolutionResult? LastResolution { get; private set; }
public void SetRng(ISeededRng rng) {
public void Surface(ExpeditionState state) {
public bool ResolveChoice(string encounterId, string choiceId, int day) => ResolveChoice(encounterId, choiceId, day, null!);
public bool ResolveChoice(string encounterId, string choiceId, int day, string locationId) {
```


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Maritime/MaritimeExplorationSystem.cs`

### `Assets/Ashfall.Core/Maritime/MaritimeExplorationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 735 lines / 29736 bytes.
- SHA-256: `927d33207135d89320ecb518b09aa9a469c00ad223c8023544b12a9cda375080`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=7; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MaritimeZoneDef
public string zone_id { get; set; } = string.Empty;
public string name { get; set; } = string.Empty;
public string zone_type { get; set; } = "coastal";
public float water_temp_celsius { get; set; } = 12.0f;
public float radiation_level { get; set; } = 0.0f;
public float current_strength { get; set; } = 20.0f;
public float visibility { get; set; } = 80.0f;
public List<string> dive_sites { get; set; } = new List<string>();
public string required_equipment_type { get; set; } = "none";
public float min_depth_meters { get; set; } = 5.0f;
public float max_depth_meters { get; set; } = 30.0f;
public string description { get; set; } = string.Empty;
public sealed class MaritimeZonesCatalog
public int schema_version { get; set; } = 1;
public List<MaritimeZoneDef> zones { get; set; } = new List<MaritimeZoneDef>();
public enum DiveSiteType
public enum DiveSiteDiscoveryStatus
public enum MaritimeExpeditionStatus
public enum MaritimeHazardType
public enum MaritimeHazardOutcome
public sealed class DiveSiteRecord
public string SiteId { get; set; } = string.Empty;
public string SiteName { get; set; } = string.Empty;
public string ZoneId { get; set; } = string.Empty;
public DiveSiteType Type { get; set; } = DiveSiteType.CoastalShallows;
public float DepthMeters { get; set; } = 15.0f;
public float HazardLevel { get; set; } = 20.0f;
public DiveSiteDiscoveryStatus Status { get; set; } = DiveSiteDiscoveryStatus.Undiscovered;
public int DiscoveredDay { get; set; } = 1;
public int ExplorationCount { get; set; } = 0;
public int MaxExplorations { get; set; } = 3;
public List<string> LootItemIds { get; set; } = new List<string>();
public string Coordinates { get; set; } = string.Empty;
public bool IsFullySalvaged => ExplorationCount >= MaxExplorations;
public sealed class MaritimeHazardEvent
public string EventId { get; set; } = string.Empty;
public MaritimeHazardType HazardType { get; set; } = MaritimeHazardType.StrongCurrent;
public MaritimeHazardOutcome Outcome { get; set; } = MaritimeHazardOutcome.Avoided;
public string DiverId { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public float RadiationDose { get; set; } = 0.0f;
public float DamageAmount { get; set; } = 0.0f;
public int Day { get; set; } = 1;
public sealed class MaritimeExpeditionRecord
public string ExpeditionId { get; set; } = string.Empty;
public string TargetSiteId { get; set; } = string.Empty;
public List<string> AssignedDivers { get; set; } = new List<string>();
public List<string> EquipmentIds { get; set; } = new List<string>();
public int StartDay { get; set; } = 1;
public int CompletedDay { get; set; } = 1;
public MaritimeExpeditionStatus Status { get; set; } = MaritimeExpeditionStatus.Planned;
public float DurationHours { get; set; } = 6.0f;
public List<string> LootCollected { get; set; } = new List<string>();
public List<MaritimeHazardEvent> HazardsEncountered { get; set; } = new List<MaritimeHazardEvent>();
public sealed class DivingEquipmentRecord
public string EquipmentId { get; set; } = string.Empty;
public string EquipmentType { get; set; } = "basic_dive_suit";
public float Condition { get; set; } = 100.0f;
public float MaxDepthRating { get; set; } = 50.0f;
public float ProtectionRating { get; set; } = 50.0f;
public bool IsEquipped { get; set; } = false;
public sealed class MaritimeExplorationState
public int SchemaVersion { get; set; } = 1;
public int NextSequence { get; set; } = 1;
public List<string> DiscoveredZoneIds { get; set; } = new List<string>();
public List<DiveSiteRecord> Sites { get; set; } = new List<DiveSiteRecord>();
public List<MaritimeExpeditionRecord> Expeditions { get; set; } = new List<MaritimeExpeditionRecord>();
public List<DivingEquipmentRecord> Equipment { get; set; } = new List<DivingEquipmentRecord>();
public List<MaritimeHazardEvent> RecentHazards { get; set; } = new List<MaritimeHazardEvent>();
public sealed class MaritimeExplorationSystem
public event Action<DiveSiteRecord>? OnSiteDiscovered;
public event Action<MaritimeHazardEvent>? OnHazardEncountered;
public event Action<MaritimeExpeditionRecord>? OnExpeditionCompleted;
public Action<string, List<string>>? InventoryLootDeliverer;
public Action<string, float>? RadiationApplier;
public Action<string, float>? InjuryApplier;
public int DiscoveredZoneCount => _state.DiscoveredZoneIds.Count;
public int DiscoveredSiteCount => _state.Sites.Count(s => s.Status != DiveSiteDiscoveryStatus.Undiscovered);
public int TotalSiteCount => _state.Sites.Count;
public int CompletedExpeditionCount => _state.Expeditions.Count(e => e.Status == MaritimeExpeditionStatus.Completed);
public IReadOnlyList<DiveSiteRecord> Sites => _state.Sites;
public IReadOnlyList<MaritimeExpeditionRecord> Expeditions => _state.Expeditions;
public IReadOnlyList<DivingEquipmentRecord> Equipment => _state.Equipment;
public IReadOnlyList<MaritimeHazardEvent> RecentHazards => _state.RecentHazards;
public void LoadCatalog(string json) {
public IReadOnlyList<MaritimeZoneDef> GetAllZoneDefs() => _zoneDefs.Values.ToList();
public MaritimeZoneDef? GetZoneDef(string zoneId) {
public bool DiscoverZone(string zoneId) {
public bool IsZoneDiscovered(string zoneId) =>
public DiveSiteRecord RegisterDiveSite( string siteId, string siteName, string zoneId, DiveSiteType type, float depthMeters,
public bool DiscoverSite(string siteId, int day = 1) {
public DiveSiteRecord? GetSite(string siteId) {
public DivingEquipmentRecord RegisterEquipment( string equipmentId, string equipmentType, float depthRating = 50.0f, float protectionRating = 50.0f) {
public DivingEquipmentRecord? GetEquipment(string equipmentId) {
public bool ValidateExpedition( string siteId, IReadOnlyList<string> diverIds, IReadOnlyList<string> equipmentIds, out string validationMessage) {
public MaritimeExplorationState CaptureState() {
public void RestoreState(MaritimeExplorationState state) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs`

### `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 115 lines / 5002 bytes.
- SHA-256: `513c8d961b30670f32c5040ee0facf4cdb5e869da06f129c6f607f0378be88d9`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpeditionAggregateState
public string systemId = "expedition_aggregate";
public List<ExpeditionState> expeditions = new List<ExpeditionState>();
public ExpeditionVehicleState vehicles = new ExpeditionVehicleState();
public List<string>? knownLocationIds = new List<string>();
public DiscoveryConsequenceState? discoveryConsequences;
public int completedCount;
public static class VehicleCatalogLoader
public const string FileName = "vehicles.json";
public static VehicleCatalog Load(string dataDir, IFileIO files, IJsonSerializer json) {
public static VehicleCatalog Load(string dataDir) => Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
public static class ExpeditionAggregateCodec
public static string Encode(ExpeditionAggregateState aggregate, IJsonSerializer json) => SaveEnvelopeHelper.CaptureEnvelope(aggregate, json);
public static ExpeditionAggregateState? Decode(string raw, IJsonSerializer json) {
```


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/ExpeditionLootValidator.cs`

### `Assets/Ashfall.Core/Expeditions/ExpeditionLootValidator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 114 lines / 4389 bytes.
- SHA-256: `331660cd2fa77b39f5434b8583c65be907f6fd44091d680e6d968be8aaba776a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpeditionLootValidationError
public string ExpeditionId { get; set; } = string.Empty;
public string FieldPath { get; set; } = string.Empty;
public string UnresolvedValue { get; set; } = string.Empty;
public string ExpectedNamespaces { get; set; } = "item_id or expedition_loot_category_id";
public string FormattedMessage =>
public override string ToString() => FormattedMessage;
public sealed class ExpeditionLootValidationResult
public bool IsValid => Errors.Count == 0;
public List<ExpeditionLootValidationError> Errors { get; } = new List<ExpeditionLootValidationError>();
public static class ExpeditionLootValidator
public static ExpeditionLootValidationResult Validate( IEnumerable<ExpeditionDefinition> expeditions, IExpeditionLootReferenceResolver resolver, string sourceCatalogName = "expeditions.json") {
public static ExpeditionLootValidationResult ValidateCatalog( string dataDir, IFileIO fileIO, IJsonSerializer serializer, IExpeditionLootReferenceResolver? resolver = null) {
public List<ItemRecordRaw>? items { get; set; }
public string id { get; set; } = string.Empty;
```


# Appendix Q.565 — Additional Current Architecture Evidence: `src/Main.UiPanels.cs`

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


# Appendix Q.566 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs`

### `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 403 lines / 14637 bytes.
- SHA-256: `364d311490e3a73f9e9d2d31861f78f70ecb80a4615f9b48ade364cb2330e7a8`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PerformanceCampaignHarness : IDisposable
public CampaignDayCoordinator Coordinator { get; }
public SurvivorRosterSystem Survivors { get; }
public Inventory.Inventory Inventory { get; }
public JournalSystem Journal { get; }
public WeatherSystem Weather { get; }
public ExpeditionSystem Expeditions { get; }
public LocationEvolutionSystem LocationEvolution { get; }
public WildlifeMigrationSystem Wildlife { get; }
public LandmarkDegradationSystem Landmark { get; }
public int CurrentDay => Coordinator.Calendar is Ashfall.Core.Clock.ISimClock simClock ? simClock.DayIndex : Coordinator.LastAdvancedDay;
public ISeededRng Rng { get; }
public double AdvanceDays(int days) {
public string CaptureSavePayload() {
public double MeasureSaveLatency() {
public double MeasureLoadLatency(string payload) {
public static double MeasureChecksumLatency(string payload) {
public long MeasureRetainedMemoryAfterNewGame() {
public void Dispose() {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public string Id => "perf_author";
public string DisplayName => "Perf";
public Ashfall.Core.Journal.RiskBiasTrait RiskBias => Ashfall.Core.Journal.RiskBiasTrait.Realist;
```


# Appendix Q.567 — Additional Current Architecture Evidence: `src/Host/ContentUtilizationRuntimeCollector.cs`

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


# Appendix Q.569 — Additional Current Architecture Evidence: `src/UI/MapPanel.cs`

### `src/UI/MapPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 606 lines / 32236 bytes.
- SHA-256: `1c4936f7bc26b62192d42fd8045a14b9234795d278f7df7f4a71f436980b4935`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=5; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class MapPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<string>? OnLocationDetailRequested;
public bool IsBound => _core != null || _expeditions != null || _catalogs != null;
public void Bind( CoreDemoSession? core, ExpeditionHostSession? expeditions = null, ExpansionHostSession? expansions = null, WorldHostSession? world = null, JournalCatalogs? catalogs = null,
public void RefreshView() {
public void SetLightingPhase(string phase) => BackdropArt.SetTexture(this, BackdropArt.WastelandSkyFor(phase));
public override void _Ready() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs`

### `Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1150 lines / 50198 bytes.
- SHA-256: `89e132b6253ae2c5c11629d2fcdae4544b4b03a004d12bf0de89f37a0a7fd2bf`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=7; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum ChoiceRequirementFailureType
public sealed class ChoiceRequirementFailure
public ChoiceRequirementFailureType FailureType { get; set; }
public string ItemId { get; set; } = string.Empty;
public int RequiredQuantity { get; set; }
public int AvailableQuantity { get; set; }
public string Reason { get; set; } = string.Empty;
public sealed class ChoiceAvailabilityResult
public bool IsAvailable => Failures.Count == 0;
public List<ChoiceRequirementFailure> Failures { get; } = new List<ChoiceRequirementFailure>();
public sealed class TravelEncounterResolutionPlan
public string EncounterId { get; init; } = string.Empty;
public string ChoiceId { get; init; } = string.Empty;
public int CurrentDay { get; init; }
public string CooldownKey { get; init; } = string.Empty;
public int CooldownExpiryDay { get; init; }
public bool IsOnCooldown { get; init; }
public int MoraleDelta { get; init; }
public int GuiltDelta { get; init; }
public string RawFactionId { get; init; } = string.Empty;
public string CanonicalFactionId { get; init; } = string.Empty;
public int FactionStandingDelta { get; init; }
public string UnlocksFieldGuideId { get; init; } = string.Empty;
public int AdvancesChainStage { get; init; }
public string? ChainId { get; init; }
public IReadOnlyList<NormalizedItemCost> Costs { get; init; } = Array.Empty<NormalizedItemCost>();
public string RequiredItemId { get; init; } = string.Empty;
public int RequiredItemQuantity { get; init; }
public string RequiredFlag { get; init; } = string.Empty;
public ChoiceAvailabilityResult Availability { get; init; } = new ChoiceAvailabilityResult();
public bool CanExecute => !IsOnCooldown && Availability.IsAvailable;
public sealed class TravelEncounterResolutionResult
public string EncounterId { get; set; } = string.Empty;
public string ChoiceId { get; set; } = string.Empty;
public int Day { get; set; }
public int MoraleDelta { get; set; }
public int GuiltDelta { get; set; }
public string FactionId { get; set; } = string.Empty;
public string CanonicalFactionId { get; set; } = string.Empty;
public int FactionStandingDelta { get; set; }
public string UnlocksFieldGuideId { get; set; } = string.Empty;
public int ChainStageAdvanced { get; set; }
public List<NormalizedItemCost> DeductedCosts { get; set; } = new List<NormalizedItemCost>();
public string CooldownKey { get; set; } = string.Empty;
public FactionBountyRecord? BountyRecord { get; set; }
public sealed class PatrolRecognitionContext
public string FactionId { get; init; } = string.Empty;
public string EncounterId { get; init; } = string.Empty;
public IReadOnlyList<string> PriorChoiceIds { get; init; } = Array.Empty<string>();
public int EncountersWithFaction { get; init; }
public int PaidTollCount { get; init; }
public int FoughtPatrolCount { get; init; }
public int CurrentChainStage { get; init; }
public int CurrentStanding { get; init; }
public IReadOnlyList<string> RecognitionTags { get; init; } = Array.Empty<string>();
public bool HasChoice(string choiceId) {
public bool HasTag(string tag) {
public sealed class PatrolChoicePresentation
public string ChoiceId { get; init; } = string.Empty;
public string Text { get; init; } = string.Empty;
public bool IsAvailable { get; init; }
public string DisabledReasonCode { get; init; } = string.Empty;
public string RequiredItemId { get; init; } = string.Empty;
public int RequiredItemQuantity { get; init; }
public IReadOnlyList<NormalizedItemCost> Costs { get; init; } = Array.Empty<NormalizedItemCost>();
public int MoraleDelta { get; init; }
public int GuiltDelta { get; init; }
public string FactionId { get; init; } = string.Empty;
public int FactionStandingDelta { get; init; }
public IReadOnlyList<ChoiceRequirementFailure> Failures { get; init; } = Array.Empty<ChoiceRequirementFailure>();
public sealed class PatrolEncounterPresentation
public string EncounterId { get; init; } = string.Empty;
public string FactionId { get; init; } = string.Empty;
public string DisplayFactionId { get; init; } = string.Empty;
public string TerritoryState { get; init; } = string.Empty;
public string PatrolArchetype { get; init; } = string.Empty;
public int CurrentChainStage { get; init; }
public string RecognitionLabel { get; init; } = string.Empty;
public IReadOnlyList<PatrolChoicePresentation> Choices { get; init; } = Array.Empty<PatrolChoicePresentation>();
public sealed class TravelEncounterSystem
public TravelEncounterCatalog Catalog => _catalog;
public event Action<string, string>? OnChoiceResolved;
public event Action<string, int>? OnChainStageAdvanced;
public event Action<string, string>? OnPatrolHistoryRecorded;
public static string GetCooldownKey(TravelEncounterDefinition encounter) {
public int GetCooldownExpiry(string cooldownKey) {
public int GetCooldownExpiry(TravelEncounterDefinition encounter) {
public int GetChainStage(string chainId) {
public void SetChainStage(string chainId, int stage) {
public int GetPatrolChainStage(string factionId, string chainId) {
public int GetPatrolChainStage(TravelEncounterDefinition encounter) {
public PatrolRecognitionContext GetRecognitionContext(string factionId, string encounterId = "", string chainId = "") {
public PatrolRecognitionContext GetRecognitionContext(TravelEncounterDefinition encounter) {
public bool IsEncounterEligible( TravelEncounterDefinition encounter, string region, float dangerLevel, string currentSeason, int currentDay,
public bool IsEncounterEligible(TravelEncounterDefinition encounter, TravelEncounterSelectionContext context) {
public float GetEffectiveWeight(TravelEncounterDefinition encounter, string stance) {
public const float MigrationEncounterBiasK = 0.5f;
public Func<string, float>? RegionEncounterPressureProvider { get; set; }
public float GetEffectiveWeight(TravelEncounterDefinition encounter, string stance, string region) {
public static float MigrationSusceptibility(TravelEncounterDefinition encounter) {
public TravelEncounterDefinition? SelectEncounter( string region, float dangerLevel, string stance, string currentSeason, int currentDay,
public TravelEncounterDefinition? SelectEncounter(TravelEncounterSelectionContext context) {
public ChoiceAvailabilityResult EvaluateChoiceAvailability( TravelEncounterChoice choice, Inventory.Inventory? inventory = null, Func<string, bool>? flagEvaluator = null) {
public ChoiceAvailabilityResult EvaluateChoiceAvailability( TravelEncounterDefinition encounter, TravelEncounterChoice choice, Inventory.Inventory? inventory = null, Func<string, bool>? flagEvaluator = null) {
public PatrolEncounterPresentation? BuildPatrolPresentation( string encounterId, Inventory.Inventory? inventory = null, Func<string, bool>? flagEvaluator = null) {
public bool TryBuildResolutionPlan( string encounterId, string choiceId, int currentDay, out TravelEncounterResolutionPlan? plan, Inventory.Inventory? inventory = null,
public bool ResolveChoice(string encounterId, string choiceId, int currentDay, out TravelEncounterResolutionResult? result) {
public bool ResolveChoice(string encounterId, string choiceId, int currentDay, out int moraleDelta, out int guiltDelta, out string unlockedFieldGuideId) {
public TravelEncounterState CaptureState() {
public void RestoreState(TravelEncounterState? state) {
```


# Appendix Q.571 — Additional Current Architecture Evidence: `src/Host/HostCli.ExpeditionPlaytest.cs`

### `src/Host/HostCli.ExpeditionPlaytest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 542 lines / 28977 bytes.
- SHA-256: `c9e8a9935c3281af8ecfbb31d467605c9eb29e774afb1b6206fa4c4569710586`.
- Architecture signals: seeded references=5; save/restore symbols=6; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunExpeditionPlaytestSelfTest(string dataDirectory) {
public string id = string.Empty;
public bool passed;
public string evidence = string.Empty;
public int schema_version = 1;
public int master_seed;
public int snapshot_count;
public int sorties_launched;
public int sorties_completed;
public int completed_loot_entries;
public int returned_loot_entries;
public int estimate_tick_mismatches;
public int breakdown_events;
public int profiles_exercised;
public bool same_seed_byte_equal;
public bool midpoint_save_load_byte_equal;
public bool different_seed_diverged;
public List<ExpeditionPlaytestSortie> sorties = new List<ExpeditionPlaytestSortie>();
public List<ExpeditionPlaytestSnapshot> snapshots = new List<ExpeditionPlaytestSnapshot>();
public List<ExpeditionPlaytestCheck> checks = new List<ExpeditionPlaytestCheck>();
public int sortie_index;
public int launch_day;
public string survivor_id = string.Empty;
public string location_id = string.Empty;
public string vehicle_id = "foot";
public float estimate_ticks;
public int actual_ticks;
public float estimate_fuel;
public float fuel_spent;
public int encounters;
public int breakdowns;
public float loot_gross_value;
public int loot_entries;
public string result = "active";
public int day;
public int active_sorties;
public int launched_sorties;
public int completed_sorties;
public float fuel_spent;
public int encounters;
public int breakdowns;
public float returned_loot_value;
public List<string> active_survivors = new List<string>();
public int schema_version = 1;
public List<ExpeditionState> active = new List<ExpeditionState>();
public List<ExpeditionPlaytestSortie> sorties = new List<ExpeditionPlaytestSortie>();
public ulong rng_state;
public int next_launch_index;
public int next_sortie_index;
public Dictionary<string, float> fuel = new Dictionary<string, float>(StringComparer.Ordinal);
public int completed_count;
public Dictionary<string, int> ticks_by_survivor = new Dictionary<string, int>(StringComparer.Ordinal);
public List<ExpeditionPlaytestSnapshot> Snapshots { get; } = new List<ExpeditionPlaytestSnapshot>();
public List<ExpeditionPlaytestSortie> Sorties { get; } = new List<ExpeditionPlaytestSortie>();
public ExpeditionSystem System { get; }
public int LaunchedSorties => Sorties.Count;
public int CompletedSorties => Sorties.Count(s => s.result == "completed");
public int CompletedLootEntries => Sorties.Where(s => s.result == "completed").Sum(s => s.loot_entries);
public int ReturnedLootEntries => Sorties.Where(s => s.result == "completed").Sum(s => s.loot_entries);
public int EstimateTickMismatches => Sorties.Count(s => s.result == "completed" && Math.Abs(s.estimate_ticks - s.actual_ticks) > 0.01f);
public int NegativeResourceCount { get; private set; }
public int BreakdownEvents => Sorties.Sum(s => s.breakdowns);
public int ProfilesExercised => Sorties.Select(s => s.vehicle_id).Distinct(StringComparer.Ordinal).Count();
public bool NoEngagementTravelRolls { get; private set; } = true;
public static ExpeditionPlaytestRun Create(string dataDirectory, int seed) {
public void AdvanceThrough(int firstDay, int lastDay) {
public ExpeditionPlaytestSave CaptureSave() {
public void RestoreSave(ExpeditionPlaytestSave save) {
public bool AllRecordsSane() {
public ExpeditionPlaytestArtifact BuildArtifact(List<ExpeditionPlaytestCheck> checks, bool same, bool saveParity, bool different) => new ExpeditionPlaytestArtifact
```


# Appendix Q.572 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs`

### `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 389 lines / 15731 bytes.
- SHA-256: `123fda81129fb64960aaa15122d94869f52587d85ba2cf4e8f9986098636d1d4`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=5; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum DiscoveryType
public enum ConsequenceStatus
public sealed class DiscoveryRecord
public string DiscoveryId { get; set; } = string.Empty;
public string LocationId { get; set; } = string.Empty;
public DiscoveryType Type { get; set; } = DiscoveryType.ResourceDeposit;
public string Title { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public int DiscoveredDay { get; set; } = 1;
public ConsequenceStatus Status { get; set; } = ConsequenceStatus.Discovered;
public List<string> Tags { get; set; } = new List<string>();
public string AssociatedFactionId { get; set; } = string.Empty;
public string ResourceYield { get; set; } = string.Empty;
public sealed class ConsequenceOutcome
public string ConsequenceId { get; set; } = string.Empty;
public string DiscoveryId { get; set; } = string.Empty;
public int Day { get; set; } = 1;
public string Description { get; set; } = string.Empty;
public float CaravanSafetyBonus { get; set; }
public float FactionStandingDelta { get; set; }
public string FactionId { get; set; } = string.Empty;
public sealed class DiscoveryConsequenceState
public int SchemaVersion { get; set; } = 1;
public int NextSequence { get; set; } = 1;
public List<DiscoveryRecord> Discoveries { get; set; } = new List<DiscoveryRecord>();
public List<ConsequenceOutcome> TriggeredConsequences { get; set; } = new List<ConsequenceOutcome>();
public sealed class ConsequenceTypeDef
public string type_id { get; set; } = string.Empty;
public string discovery_type { get; set; } = string.Empty;
public string label { get; set; } = string.Empty;
public float caravan_safety_bonus { get; set; }
public float faction_standing_delta { get; set; }
public string description_template { get; set; } = string.Empty;
public int auto_escalate_days { get; set; } = 30;
public sealed class DiscoveryConsequenceCatalog
public int schema_version { get; set; } = 1;
public List<ConsequenceTypeDef> consequence_types { get; set; } = new List<ConsequenceTypeDef>();
public sealed class DiscoveryConsequenceSystem
public event Action<DiscoveryRecord>? OnDiscoveryRegistered;
public event Action<ConsequenceOutcome>? OnConsequenceTriggered;
public int DiscoveryCount => _state.Discoveries.Count;
public int ConsequenceCount => _state.TriggeredConsequences.Count;
public void LoadCatalog(string json) {
public IReadOnlyList<ConsequenceTypeDef> GetAllConsequenceTypes() => _consequenceTypes;
public ConsequenceTypeDef? GetConsequenceType(DiscoveryType type) {
public DiscoveryRecord? RegisterExpeditionDiscovery(string locationId, int day) {
public DiscoveryRecord RegisterDiscovery( string locationId, DiscoveryType type, string title, string description, int day,
public bool ConcealDiscovery(string discoveryId) {
public ConsequenceOutcome? ExploitDiscovery(string discoveryId, int day) {
public ConsequenceOutcome? EscalateDiscovery(string discoveryId, int day) {
public float GetTotalCaravanSafetyBonus() {
public IReadOnlyList<DiscoveryRecord> GetDiscoveriesAt(string locationId) {
public DiscoveryConsequenceState CaptureState() {
public void RestoreState(DiscoveryConsequenceState state) {
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.


# Appendix S — Quality Assurance Pass Record: Plan 32

This record is part of the planning artifact, not a fresh runtime test result.

## Pass 1 — content and premise accuracy
- Content pass replaced the stale 2→50 target with the curated 75-destination authority.
- The historical baseline is separated from the current source/data/test authority.
- Current row counts and owner boundaries are stated without using count as a quality proxy.

## Pass 2 — integration architecture
- Integration pass traced destination → route/vehicle → scavenging → encounter → restore.
- Core, data, host, UI, save, event and test seams are named with current paths.
- The plan does not authorize a parallel save section, catalog, manager or host cache.

## Final precision and reaccuracy pass
- Precision pass rejects automatic conversion of every location and documents reference families.
- Every embedded current-file hash, focused runner command and master-authority reference is rechecked.
- Any proposed future seam is labeled as requiring a separate claim and premise verification.
