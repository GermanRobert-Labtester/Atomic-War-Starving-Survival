# Plan 76 — Expedition Destination Dossiers and Pre-Departure Readiness

> **Rebuild status:** PARTIAL — 75 DESTINATIONS EXIST; READ-ONLY DOSSIER PROJECTION REMAINS
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

- The historical plan assumed 53 destinations; the current primary catalog has 75 expedition rows. Destination definitions already include distance, danger, encounter chance, stamina drain, loot categories, scavenging table and discovery gating.
- The live expedition state already carries vehicle, weather, limb, protection, fatigue, camp, cargo and breakdown facts. A dossier should compose these values at read time and distinguish confirmed, estimated and unknown information.
- Persistent learning should use existing discovery/visit state and `DiscoveryConsequenceSystem`, not a parallel dossier manager. Static prose can live in existing narrative expedition briefing/route-note catalogs.

**Bounded outcome:** Build a read-only dossier projection over `ExpeditionDefinitionRegistry`, `ExpeditionSystem`, `WastelandMapSystem`, weather, vehicle, equipment, scavenging and `DiscoveryConsequenceSystem`. Do not create `ExpeditionDossierManager`, duplicate route geometry, or a new dossier save section.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `expeditions.json` schema 1 has 75 expedition rows.
- `scavenging_tables.json` has 54 tables and is consumed by ExpeditionSystem.
- Expedition estimates and runtime state already carry weather, vehicle, protection, limb and breakdown projections.
- DiscoveryConsequenceSystem persists discoveries and consequences with stable IDs.
- Narrative route notes and briefing expansions already exist but need a verified consumer map.

**Master-authority sections applied to this rebase:**

- Volume 8 H-C3/H-C4
- Volume 17 C5/C6 roadmap
- Volume 28 command cookbook

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Define a pure read-only dossier DTO assembled from current owners.
- Render confirmed facts, estimates and unknowns without caching mutable values.
- Bind dossier learning to current expedition/visit/discovery state.
- Validate every dossier/prose key against live destinations and scavenging tables.

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
| static destination definitions | ExpeditionDefinitionRegistry | `Assets/Ashfall.Core/Expeditions/ExpeditionCatalogLoader.cs` | Sole destination definition owner. |
| runtime estimate/state/visit and dispatch | ExpeditionSystem | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | No dossier state. |
| route graph and knowledge gates | WastelandMapSystem | `Assets/Ashfall.Core/World/WastelandMapSystem.cs` | Route geometry owner. |
| persistent discovery consequences | DiscoveryConsequenceSystem | `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` | Learning/consequence owner. |
| live readiness facts | Weather/vehicle/equipment owners | `Assets/Ashfall.Core/World/WeatherSystem.cs; Assets/Ashfall.Core/ExpeditionVehicleSystem.cs; Assets/Ashfall.Core/EquipmentConditionSystem.cs` | Dossier reads, never mutates. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Expedition Destination Dossiers and Pre-Departure Readiness
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ ExpeditionDefinitionRegistry
│   static destination definitions
│ ExpeditionSystem
│   runtime estimate/state/visit and dispatch
│ WastelandMapSystem
│   route graph and knowledge gates
│ DiscoveryConsequenceSystem
│   persistent discovery consequences
│ Weather/vehicle/equipment owners
│   live readiness facts
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

1. **Preserve current state ownership.** ExpeditionDefinitionRegistry owns static destination definitions: Sole destination definition owner.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| static destination definitions | ExpeditionDefinitionRegistry | `Assets/Ashfall.Core/Expeditions/ExpeditionCatalogLoader.cs` | Sole destination definition owner. |
| runtime estimate/state/visit and dispatch | ExpeditionSystem | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | No dossier state. |
| route graph and knowledge gates | WastelandMapSystem | `Assets/Ashfall.Core/World/WastelandMapSystem.cs` | Route geometry owner. |
| persistent discovery consequences | DiscoveryConsequenceSystem | `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` | Learning/consequence owner. |
| live readiness facts | Weather/vehicle/equipment owners | `Assets/Ashfall.Core/World/WeatherSystem.cs; Assets/Ashfall.Core/ExpeditionVehicleSystem.cs; Assets/Ashfall.Core/EquipmentConditionSystem.cs` | Dossier reads, never mutates. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load destination definition
2. resolve map route and discovery gate
3. read live weather/vehicle/equipment/party state
4. resolve scavenging/encounter facts
5. assemble evidence-classified dossier
6. render pre-departure briefing
7. dispatch through ExpeditionSystem

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Destination definitions are catalog data.
- Dossier projection is derived and not persisted.
- Visits/discoveries remain in ExpeditionSystem/DiscoveryConsequenceSystem.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Dossier never duplicates mutable values.
- Unknown facts remain unknown, not zero/default.
- A destination key must resolve to one live definition.
- A scouting note cannot reveal hidden exact state before discovery.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- No new destination rows in the dossier tranche.
- Use existing narrative briefs/route notes after consumer audit.
- Any future dossier catalog references existing destination IDs only.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- No dossier save section.
- Learned facts persist in expedition/discovery owners.
- Old saves without discovery records show baseline uncertainty.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Projection consumes already-sampled dispatch values.
- No dossier RNG.
- Stable ordering by destination ID.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Expedition completion/arrival feeds discovery and narrative consumers.
- Dossier itself emits no gameplay event.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/ExpeditionHostSession.cs
- src/Main.Expeditions.cs
- src/UI/ExpeditionPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Route notes distinguish scout confidence and corrections.
- No spoiler of undiscovered locations or exact hazards.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A dossier key is missing from the live catalog. | ExpeditionDefinitionRegistry | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A cached weather value contradicts dispatch. | ExpeditionSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Unknown is displayed as safe. | WastelandMapSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Scouting note changes route geometry. | DiscoveryConsequenceSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | Dossier state is saved separately and drifts. | Weather/vehicle/equipment owners | Focused negative test or static source gate; no broad-suite dependency. |
| F-06 | Panel blocks a risky but valid expedition. | ExpeditionDefinitionRegistry | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan32ExpeditionDestinationWiringTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan76DestinationLootReferenceTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/DiscoveryConsequenceSystemTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/ExpeditionSystemTests.cs`
5. `godot --headless --path . -- --data-integrity-selftest` when destination/scavenging references change.

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — current destination census | Count and classify all current destinations and tables. | No historical 53-row assumption remains. | No production path until the owning implementation package is separately claimed. |
| 1 — projection contract | Define evidence-classified read-only DTO. | No mutable cache/save. | No production path until the owning implementation package is separately claimed. |
| 2 — owner binding | Compose map, weather, vehicle, equipment, party and scavenging facts. | One value per concern. | No production path until the owning implementation package is separately claimed. |
| 3 — narrative binding | Attach only verified route-note/briefing rows. | Knowledge gates hold. | No production path until the owning implementation package is separately claimed. |
| 4 — UI and runtime | Expose dossier in current expedition panel. | Existing dispatch commands remain canonical. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs | READ; additive query seam only if required | Runtime owner |
| Assets/Ashfall.Core/Expeditions/ExpeditionCatalogLoader.cs | READ | Definition owner |
| Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs | READ | Learning owner |
| src/UI/ExpeditionPanel.cs | MODIFY in a future bounded UI claim | Presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Parallel dossier state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Historical count drift. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Leaking hidden route facts. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Estimate/runtime divergence. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Panel-side dispatch rules. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new destination catalog.
- No route geometry duplication.
- No dossier save section.
- No automatic safety block.

# 23. Rollback and Recovery

- Read-only projection/UI is reversible.
- Any learning state extension belongs to existing owners with migration.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 75 current destinations are recorded.
- Dossier is explicitly derived.
- Evidence classes and unknowns are specified.
- Existing expedition command remains canonical.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Define a pure read-only dossier DTO assembled from current owners.
- Render confirmed facts, estimates and unknowns without caching mutable values.
- Bind dossier learning to current expedition/visit/discovery state.
- Validate every dossier/prose key against live destinations and scavenging tables.

## MUST NOT DO

- No new destination catalog.
- No route geometry duplication.
- No dossier save section.
- No automatic safety block.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan32ExpeditionDestinationWiringTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan76DestinationLootReferenceTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/DiscoveryConsequenceSystemTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/ExpeditionSystemTests.cs`
5. `godot --headless --path . -- --data-integrity-selftest` when destination/scavenging references change.

## FIRST SAFE IMPLEMENTATION STEP

0 — current destination census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: static destination definitions → ExpeditionDefinitionRegistry; runtime estimate/state/visit and dispatch → ExpeditionSystem; route graph and knowledge gates → WastelandMapSystem; persistent discovery consequences → DiscoveryConsequenceSystem; live readiness facts → Weather/vehicle/equipment owners. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 76.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 76 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by ExpeditionDefinitionRegistry or any other authority, hide uncertainty, or introduce a gameplay-only counter.

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


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/World/WastelandMapSystem.cs`

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


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs`

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


# Appendix B.06 — Current Code Architecture: `Assets/Ashfall.Core/Expeditions/ScavengingTableCatalog.cs`

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


# Appendix B.07 — Current Code Architecture: `src/Host/ExpeditionHostSession.cs`

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


# Appendix B.08 — Current Code Architecture: `src/Main.Expeditions.cs`

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


# Appendix B.09 — Current Code Architecture: `src/UI/ExpeditionPanel.cs`

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


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/expeditions.json`

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


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/scavenging_tables.json`

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


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/narrative/expedition_planning_briefs_batch_1.json`

### `Assets/StreamingAssets/Data/narrative/expedition_planning_briefs_batch_1.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 4345 bytes / 4341 characters.
- SHA-256: `8664c1f4ec953f75297a81a5e546ad8308eafc19b55a77d5f008e2e16aa15efa`.
- Root keys: `briefs`, `collection_id`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
briefs: min=1, max=1, observed_paths=1
briefs[].cross_refs: min=4, max=4, observed_paths=1
briefs[].equipment: min=6, max=6, observed_paths=1
briefs[].key_questions: min=4, max=4, observed_paths=1
briefs[].recommendations: min=8, max=8, observed_paths=1
briefs[].risks: min=4, max=4, observed_paths=1
briefs[].success_criteria: min=4, max=4, observed_paths=1
briefs[].tags: min=8, max=8, observed_paths=1
briefs[].team_requirements: min=4, max=4, observed_paths=1
```

Representative record fields:

- `author`
- `brief_id`
- `cross_refs`
- `day`
- `distance_km`
- `equipment`
- `estimated_duration_hours`
- `key_questions`
- `objective`
- `recommendations`
- `reviewed_by`
- `risks`
- `route`
- `success_criteria`
- `surface_dose_estimate_uSv_h`
- `tags`
- `team_requirements`
- `team_size`
- `weather_forecast`


# Appendix C.13 — Catalog Census: `Assets/StreamingAssets/Data/narrative/expedition_route_waypoint_notes_batch_2.json`

### `Assets/StreamingAssets/Data/narrative/expedition_route_waypoint_notes_batch_2.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 28747 bytes / 28733 characters.
- SHA-256: `0d520ce658fc684b386ad63477b2e2bc756e98d3c089057d3f1b9f9ced1e6cef`.
- Root keys: `collection_id`, `description`, `routes`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
routes: min=15, max=15, observed_paths=1
routes[].tags: min=4, max=4, observed_paths=2
routes[].waypoints: min=4, max=5, observed_paths=2
```

Representative record fields:

- `author`
- `difficulty`
- `estimated_travel_time_hours`
- `name`
- `radiation_notes`
- `route_id`
- `tags`
- `total_distance_km`
- `water_sources`
- `waypoints`


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Expeditions/Plan32ExpeditionDestinationWiringTests.cs`

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


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Expeditions/Plan76DestinationLootReferenceTests.cs`

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


# Appendix D.16 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Expeditions/DiscoveryConsequenceSystemTests.cs`

### `Ashfall.Core.Tests/Expeditions/DiscoveryConsequenceSystemTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 135; SHA-256: `60a6e9087575cdcfef787bbfbfb18452c0a3032604f61539a0e2d96f1841a1c7`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RegisterDiscovery_AddsRecord_AndAutoTriggersThreatClearedSafetyBonus
ConcealDiscovery_SetsStatusToConcealed
ExploitDiscovery_TriggersOutcomeWithFactionStanding
EscalateDiscovery_RecordsEscalationOutcome
GetTotalCaravanSafetyBonus_CapsAtFiftyPercent
CaptureState_And_RestoreState_RoundTripsAccurately
RegisterExpeditionDiscovery_IsIdempotent_AndPersistsStableConsequence
```


# Appendix D.17 — Existing Focused Test Inventory: `Ashfall.Core.Tests/ExpeditionSystemTests.cs`

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


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/Radio/DistressDestinationResolver.cs`

### `Assets/Ashfall.Core/Radio/DistressDestinationResolver.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 417 lines / 26793 bytes.
- SHA-256: `871b3c08b74bfca2aa20bb386de1b504602cfe005e63aa7985c41209a5104635`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DistressDestinationResolution
public bool IsValid { get; set; }
public string RawLocationId { get; set; } = string.Empty;
public string DestinationId { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public int DistanceTicks { get; set; }
public int DangerLevel { get; set; }
public string ScavengingTableId { get; set; } = string.Empty;
public List<string> LootCategories { get; set; } = new List<string>();
public string ResolutionMode { get; set; } = string.Empty; // Direct, Aliased, SignalThematic, Fallback
public string? ErrorCode { get; set; }
public sealed class CanonicalDestinationInfo
public string Id { get; }
public string DisplayName { get; }
public int DistanceTicks { get; }
public int DangerLevel { get; }
public string ScavengingTableId { get; }
public IReadOnlyList<string> LootCategories { get; }
public sealed class DistressDestinationResolver
public const int SchemaVersion = 1;
public static DistressDestinationResolver Default => s_defaultInstance.Value;
public int TotalCanonicalDestinations => _canonical.Count;
public int TotalAliases => _aliasMap.Count;
public bool IsCanonicalDestination(string destinationId) {
public CanonicalDestinationInfo? GetCanonicalInfo(string destinationId) {
public IReadOnlyCollection<CanonicalDestinationInfo> GetAllCanonical() => _canonical.Values;
public DistressDestinationResolution Resolve(string? rawLocationId) {
public DistressDestinationResolution ResolveSignal(DistressSignalDefinition? signal) {
```


# Appendix E.19 — Supporting Code Evidence: `Assets/Ashfall.Core/Expeditions/ArmoredCrawlerExpeditionSystem.cs`

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


# Appendix E.20 — Supporting Code Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix E.21 — Supporting Code Evidence: `Assets/Ashfall.Core/Expeditions/ExpeditionHeadlessDemo.cs`

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


# Appendix E.22 — Supporting Code Evidence: `src/Host/HostCli.Collectibles.cs`

### `src/Host/HostCli.Collectibles.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 495 lines / 30626 bytes.
- SHA-256: `3ac21d702fc5aa4eab294bb7503aef4f7638ad980a5e80e49de3d986ac7f942c`.
- Architecture signals: seeded references=5; save/restore symbols=10; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunCollectibleSelfTest(string dataDirectory) {
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/ExpeditionCampSystemTests.cs`

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


# Appendix G.24 — Supporting Regression Evidence: `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs`

### `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs`

- Current test declarations: Fact=18, Theory=0, InlineData=0.
- File lines: 493; SHA-256: `d380d40700907225ffeab265efa5803404fcc33dd1107dae115393d0ca37d8b4`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsTwelveZones_WithUniqueIdsAndConsistentCounts
Catalog_AllRevealedItems_ResolveAgainstItemAuthority
Catalog_EveryFragment_HasScavengingProducer
Catalog_EveryInstallation_HasWastelandMapNodeAndRoute
RegisterFragment_ProgressesAndCompletes_EdgeTriggeredOnce
RegisterFragment_DuplicatesAndUnknowns_NeverDoubleCount
Reveal_PersistsThroughCaptureRestore_AndSurvivesReload
DestinationGate_BlocksUntilRevealed_NeverGatesOtherLocations
OldSave_OriginalZoneProgress_LoadsAndPreserves_UnderExpandedCatalog
CatalogOrder_DoesNotAffectProgression
ExpeditionSystem_FragmentRoll_RegistersDiscovery_AndFragmentOnlyRollYieldsNoItem
LiveCatalog_FragmentTokens_SurfaceUnderSeededSoak_AndResolve
ExpeditionSystem_StartRefused_WhileDestinationLocked
Validator_DuplicateZoneId_IsReported
Validator_FragmentCountMismatch_IsReported
Validator_DuplicateFragmentAcrossZones_IsReported
Validator_UnresolvedAndDuplicateRewards_AreReported
Validator_MissingZoneName_IsReported
```


# Appendix G.25 — Supporting Regression Evidence: `Ashfall.Core.Tests/Expeditions/ScavengingTableCatalogTests.cs`

### `Ashfall.Core.Tests/Expeditions/ScavengingTableCatalogTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 229; SHA-256: `9dfa3765a8743774c61ad1084af0df5defaca4343014406dc1760b0c86ce077f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
LoadCatalog_LoadsAll20AuthoredTables
TableIntegrity_AllTablesHaveValidWeightsAndQuantities
SeededDeterminism_SameSeedSameTable_ProducesIdenticalOutcomes
DistributionSimulation_10000Rolls_ReflectsRelativeWeights
HazardTriggering_RespectsTableHazardChances
CodexUnlockIds_PreservedOnRelevantRolls
ExpeditionSystem_WithScavengingCatalog_RollsLootFromTable
```


# Appendix G.26 — Supporting Regression Evidence: `Ashfall.Core.Tests/WastelandMapPersistenceTests.cs`

### `Ashfall.Core.Tests/WastelandMapPersistenceTests.cs`

- Current test declarations: Fact=11, Theory=0, InlineData=0.
- File lines: 261; SHA-256: `56f8197bd392aac6abd6436789d19d2d15f4e1082e58a1fcd8ccabc63bd694a5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Capture_IsSnapshot_DiscoveredList_A
Capture_IsSnapshot_DiscoveredList_B
RestoreInto_IsDeep
NormalizeAndValidate_KeepsStartingUnlocked
Discovered_SaveLoadRoundTrip_PreservesDiscoveredNodes
Locked_SaveLoadRoundTrip_PreservesStaticAndDynamicLocks
Completed_SaveLoadRoundTrip_PreservesCompletedNodes
MixedState_SaveLoadRoundTrip_ResolvesCorrectNodeStatuses
JsonEnvelope_SerializationAndChecksum_RoundTripsSuccessfully
SnapshotIsolation_MutatingCapturedState_DoesNotAffectActiveSystem
Events_FireOnCompleteAndLockChanged
```


# Appendix H.27 — Supporting Authority Document: `docs/expeditions/EXPEDITION_DESTINATION_SCHEMA.md`

### `docs/expeditions/EXPEDITION_DESTINATION_SCHEMA.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 30 lines / 1930 bytes.
- SHA-256: `e2badd264233f51fcae7580f13f4a7a391fec8a4143ed6cd63c366aace7a9631`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.28 — Supporting Authority Document: `docs/EXPEDITION_BALANCE_BASELINE.md`

### `docs/EXPEDITION_BALANCE_BASELINE.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 36 lines / 1769 bytes.
- SHA-256: `10637966712ee11a19c833ff55127d4faaddf63e98bcddfd61c134fe8153b9fd`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.29 — Supporting Authority Document: `docs/cartography/PLAN76_PLAN85_DESTINATION_RECONCILIATION.md`

### `docs/cartography/PLAN76_PLAN85_DESTINATION_RECONCILIATION.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 26 lines / 2755 bytes.
- SHA-256: `55535ef796640a30ba3d43e6d6d90a09b8a605e15cec2c3a0a8ca4438b79b3df`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| static destination definitions | ExpeditionDefinitionRegistry | runtime estimate/state/visit and dispatch | ExpeditionSystem | Owner emits/reads a typed fact; no mirror state. |
| static destination definitions | ExpeditionDefinitionRegistry | route graph and knowledge gates | WastelandMapSystem | Owner emits/reads a typed fact; no mirror state. |
| static destination definitions | ExpeditionDefinitionRegistry | persistent discovery consequences | DiscoveryConsequenceSystem | Owner emits/reads a typed fact; no mirror state. |
| static destination definitions | ExpeditionDefinitionRegistry | live readiness facts | Weather/vehicle/equipment owners | Owner emits/reads a typed fact; no mirror state. |
| runtime estimate/state/visit and dispatch | ExpeditionSystem | static destination definitions | ExpeditionDefinitionRegistry | Owner emits/reads a typed fact; no mirror state. |
| runtime estimate/state/visit and dispatch | ExpeditionSystem | route graph and knowledge gates | WastelandMapSystem | Owner emits/reads a typed fact; no mirror state. |
| runtime estimate/state/visit and dispatch | ExpeditionSystem | persistent discovery consequences | DiscoveryConsequenceSystem | Owner emits/reads a typed fact; no mirror state. |
| runtime estimate/state/visit and dispatch | ExpeditionSystem | live readiness facts | Weather/vehicle/equipment owners | Owner emits/reads a typed fact; no mirror state. |
| route graph and knowledge gates | WastelandMapSystem | static destination definitions | ExpeditionDefinitionRegistry | Owner emits/reads a typed fact; no mirror state. |
| route graph and knowledge gates | WastelandMapSystem | runtime estimate/state/visit and dispatch | ExpeditionSystem | Owner emits/reads a typed fact; no mirror state. |
| route graph and knowledge gates | WastelandMapSystem | persistent discovery consequences | DiscoveryConsequenceSystem | Owner emits/reads a typed fact; no mirror state. |
| route graph and knowledge gates | WastelandMapSystem | live readiness facts | Weather/vehicle/equipment owners | Owner emits/reads a typed fact; no mirror state. |
| persistent discovery consequences | DiscoveryConsequenceSystem | static destination definitions | ExpeditionDefinitionRegistry | Owner emits/reads a typed fact; no mirror state. |
| persistent discovery consequences | DiscoveryConsequenceSystem | runtime estimate/state/visit and dispatch | ExpeditionSystem | Owner emits/reads a typed fact; no mirror state. |
| persistent discovery consequences | DiscoveryConsequenceSystem | route graph and knowledge gates | WastelandMapSystem | Owner emits/reads a typed fact; no mirror state. |
| persistent discovery consequences | DiscoveryConsequenceSystem | live readiness facts | Weather/vehicle/equipment owners | Owner emits/reads a typed fact; no mirror state. |
| live readiness facts | Weather/vehicle/equipment owners | static destination definitions | ExpeditionDefinitionRegistry | Owner emits/reads a typed fact; no mirror state. |
| live readiness facts | Weather/vehicle/equipment owners | runtime estimate/state/visit and dispatch | ExpeditionSystem | Owner emits/reads a typed fact; no mirror state. |
| live readiness facts | Weather/vehicle/equipment owners | route graph and knowledge gates | WastelandMapSystem | Owner emits/reads a typed fact; no mirror state. |
| live readiness facts | Weather/vehicle/equipment owners | persistent discovery consequences | DiscoveryConsequenceSystem | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Define a pure read-only dossier DTO assembled from current owners. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Render confirmed facts, estimates and unknowns without caching mutable values. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Bind dossier learning to current expedition/visit/discovery state. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Validate every dossier/prose key against live destinations and scavenging tables. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Collectibles/CollectibleEffectDispatcher.cs`

### `Assets/Ashfall.Core/Collectibles/CollectibleEffectDispatcher.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 351 lines / 16641 bytes.
- SHA-256: `8b68c52ae37580be2981c55da7f02b4c4b83dc63480d13f19525b5f38c0e5d93`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class CollectibleDispatchResult
public bool IsCollectible;
public bool AlreadyDiscovered;
public string EffectType = string.Empty;
public bool EffectApplied;
public bool DiscoveryRegistered;
public string FailureReason = string.Empty;
public string? DiscoveryLocationId;
public bool HasDiscoveryEffects => !string.IsNullOrEmpty(EffectType) && EffectType != "none" && EffectApplied;
public class CollectibleEffectDispatcher
public const float MaxMoraleEffectValue = 10f;
public CollectibleDiscoveryState Discovery => _discovery;
public event Action<CollectibleDispatchResult>? OnCollectibleDiscovered;
public CollectibleDispatchResult DispatchOnAcquire(string itemId, string? discoveryLocationId = null) {
public CollectibleMigrationReport ReconcileDiscoveredSubsystemState( Func<VinylMoraleSystem?>? vinylProvider = null, ISeededRng? vinylRng = null) {
public sealed class CollectibleMigrationReport
public int KnowledgeReconciled;
public int LocationReconciled;
public int VinylChecked;
```


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/CipherQuestChainEngine.cs`

### `Assets/Ashfall.Core/Narrative/CipherQuestChainEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 191 lines / 7419 bytes.
- SHA-256: `f2b90b60016554a8459f03732277714baafeff6858be5c56321376ce54bf272a`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CipherQuestState
public string chainId = string.Empty;
public bool isHeard;
public bool isKeyFound;
public bool isDecoded;
public bool isLocationRevealed;
public bool isResolved;
public sealed class CipherChainDefinition
public string ChainId { get; set; } = string.Empty;
public string QuestId { get; set; } = string.Empty;
public string BroadcastId { get; set; } = string.Empty;
public string CipherStationId { get; set; } = string.Empty;
public string RequiredItemId { get; set; } = string.Empty;
public string TargetLocationId { get; set; } = string.Empty;
public string HeardFlag { get; set; } = string.Empty;
public string KeyFoundFlag { get; set; } = string.Empty;
public string DecodedFlag { get; set; } = string.Empty;
public string RevealedFlag { get; set; } = string.Empty;
public string ResolvedFlag { get; set; } = string.Empty;
public sealed class CipherQuestChainEngine
public static readonly List<CipherChainDefinition> Chains = new List<CipherChainDefinition> {
public event Action<string, string>? OnLocationRevealedByCipher;
public event Action<string>? OnCipherDecoded;
public CipherQuestState GetState(string chainId) {
public void RecordBroadcastHeard(string broadcastOrStationId, WastelandMapSystem? map = null) {
public void RecordKeyAcquired(string itemId, WastelandMapSystem? map = null) {
public bool EvaluateDecode(CipherChainDefinition def, WastelandMapSystem? map = null) {
public void MarkResolved(string chainId) {
public List<CipherQuestState> CaptureState() {
public void RestoreState(List<CipherQuestState>? savedStates, WastelandMapSystem? map = null) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/DamagedMapSystem.cs`

### `Assets/Ashfall.Core/World/DamagedMapSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 204 lines / 9326 bytes.
- SHA-256: `b5a78fef932b80b13d08c2d802f18a38157d83aa82ac76294cba48c954709e26`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DamagedMapSystem
public event Action<DamagedMapZone>? OnZoneCompleted;
public event Action<DamagedMapZone, string>? OnInstallationRevealed;
public event Action<string>? OnFragmentRegistered;
public IReadOnlyList<DamagedMapZone> Zones => _zones;
public DamagedMapZone? FindZone(string zoneId) => !string.IsNullOrEmpty(zoneId) && _zonesById.TryGetValue(zoneId, out var z) ? z : null;
public DamagedMapZone? FindZoneByFragment(string fragmentId) => !string.IsNullOrEmpty(fragmentId) && _zonesByFragment.TryGetValue(fragmentId, out var z) ? z : null;
public DamagedMapZone? FindZoneByDestination(string locationId) => !string.IsNullOrEmpty(locationId) && _zonesByDestination.TryGetValue(locationId, out var z) ? z : null;
public static string? ResolveRevealNodeId(string installationId) {
public bool IsFragmentRegistered(string fragmentId) {
public int RegisteredCount(string zoneId) {
public bool IsZoneComplete(string zoneId) {
public bool RegisterFragment(string fragmentId) {
public bool IsInstallationRevealed(string zoneId) {
public bool IsDestinationLocked(string locationId) {
```


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs`

### `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 288 lines / 14894 bytes.
- SHA-256: `8fc5ee3c5b6f8fcae3e9c0811a1da49b093c8c2a9227996a302b867b29cb40f8`.
- Architecture signals: seeded references=4; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class CombatHeadlessReport : HeadlessReport
public CombatState FinalState;
public CombatSnapshot Snapshot;
public static class CombatHeadlessDemo
public const int DefaultSeed = 1337;
public const int EnemyCount = 3;
public const float EnemyHealth = 40f;
public static CombatHeadlessReport Run(ILog? log = null) {
```


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/ContractorRosterSystem.cs`

### `Assets/Ashfall.Core/ContractorRosterSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 235 lines / 9931 bytes.
- SHA-256: `252364e87221157a754d45b97f6bc8fe00167a23a3646fab5fb4d9d526d531ad`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=12; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ContractorRosterState
public string systemId = ContractorRosterSystem.SystemId;
public List<Contractor> contractors = new List<Contractor>();
public List<ContractOffer> activeOffers = new List<ContractOffer>();
public sealed class ContractOffer
public string offerId = string.Empty;
public string candidateId = string.Empty;
public string role = string.Empty;
public int initialFee;
public int dailyHazardPay;
public string paymentCurrency = "scrap_metal";
public int termDays;
public float loyalty = 100f;
public ContractStatus status;
public int proposedDay = -1;
public int startDay = -1;
public int expiryDay = -1;
public List<string> requiredSkills = new List<string>();
public List<string> equipmentIds = new List<string>();
public sealed class Contractor
public string contractorId = string.Empty;
public string displayName = string.Empty;
public string role = string.Empty;
public float loyalty = 100f;
public float trust = 50f;
public ContractStatus status;
public int startDay = -1;
public int expiryDay = -1;
public int missedPayments;
public bool isInjured;
public bool isDeceased;
public List<string> skillIds = new List<string>();
public List<string> equipmentIds = new List<string>();
public enum ContractStatus { Available, Active, Expired, Dismissed, Deceased } public sealed class ContractorRosterSystem { public const string SystemId = "contractor_roster"; private ContractorRosterState _state = new ContractorRosterState(); private readonly ISeededRng _rng; private readonly ILog _log; private readonly Inventory.Inventory _inventory; private readonly DutyRosterSystem _roster; private readonly ExpeditionSystem _expedition; private int _currentDay; public ContractorRosterState State => _state; public event Action<Contractor> OnContractorStatusChanged; public event Action<ContractOffer> OnOfferStatusChanged; public event Action OnRosterChanged; public ContractorRosterSystem( ISeededRng rng, Inventory.Inventory inventory, DutyRosterSystem roster, ExpeditionSystem expedition, ILog? log = null) { _rng = rng ?? throw new ArgumentNullException(nameof(rng)); _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory)); _roster = roster ?? throw new ArgumentNullException(nameof(roster)); _expedition = expedition ?? throw new ArgumentNullException(nameof(expedition)); _log = log ?? NullLog.Instance; }
public ActionResult GenerateOffer(string candidateId, string role, List<string> requiredSkills, int initialFee, int dailyPay, int termDays) {
public ActionResult AcceptOffer(string offerId) {
public ActionResult Dismiss(string contractorId) {
public void TickDay(int day) {
public bool IsAvailableForExpedition(string contractorId) {
public ContractorRosterState CaptureState() => CloneState(_state);
public void RestoreState(ContractorRosterState saved) {
```


# Appendix Q.566 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs`

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


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs`

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


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/WorldEvolutionEngine.cs`

### `Assets/Ashfall.Core/World/WorldEvolutionEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 317 lines / 12588 bytes.
- SHA-256: `cd3b6d132f47a2f36f1bd6d9f13ac513f0f2b046e287db73692e968a8fb32032`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WorldEvolutionCatalogContainer
public int schema_version { get; set; } = 1;
public List<WorldEvolutionEventDef> events { get; set; } = new List<WorldEvolutionEventDef>();
public sealed class WorldEvolutionEventDef
public string id { get; set; } = string.Empty;
public string type { get; set; } = string.Empty; // "blockade", "territory_flip", "site_degradation", "hazard_bloom"
public int trigger_day { get; set; }
public string required_flag { get; set; } = string.Empty;
public string target_location_id { get; set; } = string.Empty;
public string target_node_id { get; set; } = string.Empty;
public string title { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public string recast_text { get; set; } = string.Empty;
public bool locks_node { get; set; }
public string new_owner { get; set; } = string.Empty;
public float danger_delta { get; set; }
public float depletion_amount { get; set; }
public string added_threat { get; set; } = string.Empty;
public float contamination_delta { get; set; }
public sealed class WorldEvolutionState
public int schema_version = 1;
public int lastEvaluatedDay = -1;
public List<string> triggeredEventIds = new List<string>();
public sealed class WorldEvolutionEngine
public const string CatalogFileName = "world_evolution_events.json";
public IReadOnlyList<WorldEvolutionEventDef> Events => _events;
public IReadOnlyCollection<string> TriggeredEventIds => _triggeredEvents;
public event Action<WorldEvolutionEventDef>? OnEvolutionTriggered;
public void TickDay( int day, HashSet<string>? activeWorldFlags, LocationEvolutionSystem? evolution, LandmarkDegradationSystem? landmarks, WastelandMapSystem? map)
public void ApplyEvent( WorldEvolutionEventDef evt, int day, LocationEvolutionSystem? evolution, LandmarkDegradationSystem? landmarks, WastelandMapSystem? map)
public WorldEvolutionState CaptureState() {
public void RestoreState(WorldEvolutionState? saved, WastelandMapSystem? map = null) {
public static List<WorldEvolutionEventDef> GetDefaultEvents() {
```


# Appendix Q.569 — Additional Current Architecture Evidence: `src/Host/HostCli.ExpeditionPlaytest.cs`

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


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentDeepChainGate.cs`

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


# Appendix Q.571 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs`

### `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 393 lines / 15772 bytes.
- SHA-256: `23f329a574d06d8ddda22c32f421f6dcdf6852cea846a86fe2f5628eca4988de`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=7; textual Godot mentions=1; textual Unity/JsonUtility mentions=2; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class ShelterEncounterRecord
public string id;
public string kind;      // visitor / hatch / night / meal / crowd / illness
public int dayStarted;
public int dayResolved = -1;
public string payload;
public string visitorId; // e.g. npc_edor_vale, npc_len_quill
public class ShelterEncounterSystemState
public string systemId = ShelterEncounterSystem.SystemId;
public bool expansionUnlocked;
public int seedSalt = ShelterEncounterSystem.SeedOffset;
public int lastEncounterDay = -1;
public int encountersThisNight;
public float encounterWeightMultiplier = 1f;
public int secondWinterActiveSince = -1;
public List<ShelterEncounterRecord> history = new List<ShelterEncounterRecord>();
public List<string> activeVisitorQueue = new List<string>();
public List<string> resolvedIds = new List<string>();
public class ShelterEncounterSystem
public const string SystemId = "shelter_encounter_system";
public const int SeedOffset = 1208;
public const string KindNightSlate = "night_slate";
public const string KindHatchReturn = "hatch_return";
public const string KindMealShort = "meal_short";
public const string KindIntakeSleep = "intake_sleep";
public const string KindLevyAbsence = "levy_absence";
public const string KindIcePack = "ice_pack";
public const string KindEdorStool = "edor_stool";
public const string KindPellMachine = "pell_machine";
public const string KindStackFever = "stack_fever";
public const string KindChildChart = "child_chart";
public const string KindTinAgain = "tin_again";
public const string KindIntercomOffice = "intercom_office";
public const string KindRoadDarkCrowd = "road_dark_crowd";
public const string KindSelaRow = "sela_row";
public const string VisitorEdor = "npc_edor_vale";
public const string VisitorLen = "npc_len_quill";
public const string VisitorPell = "npc_sergeant_pell";
public const string VisitorOffice = "faction_the_office";
public const string VisitorOverflow = "overflow_runner";
public event Action<ShelterEncounterRecord> OnShelterEncounterStarted;
public event Action<ShelterEncounterRecord> OnShelterEncounterResolved;
public event Action<ShelterEncounterSystemState> OnStateChanged;
public ShelterEncounterSystemState State => _state;
public bool IsUnlocked => _state.expansionUnlocked;
public int LastEncounterDay => _state.lastEncounterDay;
public int EncountersThisNight => _state.encountersThisNight;
public IReadOnlyList<string> ActiveVisitorQueue => _visitorQueue;
public void ResetNightCounter(int day) {
public float EncounterWeightMultiplier => _state.encounterWeightMultiplier;
public bool IsSecondWinterActive => _state.secondWinterActiveSince >= 0;
public void SetSecondWinter(float multiplier, int day) {
public void ClearSecondWinter() {
public void Initialise(int seedSalt) {
public void Unlock(int day) {
public bool QueueVisitor(string visitorId, int day) {
public string? PeekVisitor() {
public bool ResolveVisitor(string visitorId) {
public bool StartEncounter(string id, string kind, int day, string? visitorId = null, string? payload = null) {
public bool StartEncounterCrisis(string id, string kind, int day, string? visitorId = null, string? payload = null) {
public bool BridgeHatchReturn(int day, string? survivorId = null, string? payload = null, bool crisis = false) {
public bool ResolveEncounter(string id, int day) {
public bool IsResolved(string id) {
public ShelterEncounterRecord? GetActive(string id) {
public ShelterEncounterSystemState CaptureState() {
public void RestoreState(ShelterEncounterSystemState saved) {
```


# Appendix Q.572 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs`

### `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 730 lines / 37464 bytes.
- SHA-256: `7a4ce72a45ca3b105dab66e96045b873452f24a027e850a97d4408d2d8c5cc39`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=15; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum CompanionRole
public enum CompanionTrainingLevel
public enum CompanionSicknessState
public sealed class CompanionSpeciesProfile
public string species_id { get; set; } = string.Empty;     // must exist in wildlife_ecosystem.json
public string display_name { get; set; } = string.Empty;
public List<string> role_tags { get; set; } = new List<string>();    // guard | pack | morale
public int base_food_per_day { get; set; }                 // units of canonical food/day
public List<string> preferred_food_tags { get; set; } = new List<string>();
public List<string> fallback_food_item_ids { get; set; } = new List<string>();
public int max_health { get; set; } = 50;
public int trainability { get; set; } = 5;                 // 1..10
public int bond_rate { get; set; } = 5;                    // 1..10
public int guard_rating { get; set; } = 0;                 // 0..100 warning value
public int pack_capacity_kg { get; set; } = 0;             // 0..60
public int morale_support_bp { get; set; } = 0;            // 0..500 bounded
public int disease_resistance { get; set; } = 0;           // 0..10
public List<string> terrain_tags { get; set; } = new List<string>();
public List<string> tags { get; set; } = new List<string>();
public sealed class CompanionCatalogRoot
public int schema_version { get; set; } = 1;
public List<CompanionSpeciesProfile> companions { get; set; } = new List<CompanionSpeciesProfile>();
public sealed class CompanionCatalogLoadResult
public List<CompanionSpeciesProfile> Companions { get; } = new List<CompanionSpeciesProfile>();
public List<string> Errors { get; } = new List<string>();
public bool HasErrors => Errors.Count > 0;
public sealed class CompanionState
public string companion_id { get; set; } = string.Empty;   // = wildlife DomesticAnimalState.animal_id
public string species_id { get; set; } = string.Empty;
public string name { get; set; } = string.Empty;           // presentation/state data, NOT identity
public string assigned_survivor_id { get; set; } = string.Empty;
public int role { get; set; } = (int)CompanionRole.Unassigned;
public int training_progress { get; set; }                 // 0..TrainingProgressPerLevel-1
public int training_level { get; set; }                    // (int) CompanionTrainingLevel
public int bond { get; set; }                              // 0..MaxBond
public int health { get; set; } = 50;
public int hunger { get; set; }                            // 0..100, HIGHER = WORSE (NeedsSystem parity)
public int sickness { get; set; } = 0;                     // (int) CompanionSicknessState
public int last_fed_day { get; set; } = -1;
public int tamed_day { get; set; }
public bool on_expedition { get; set; }
public bool alive { get; set; } = true;
public sealed class CompanionSystemState
public string system_id { get; set; } = "companion_animals";
public int schema_version { get; set; } = 1;
public int last_tick_day { get; set; }
public List<CompanionState> companions { get; set; } = new List<CompanionState>();
public sealed class CompanionAssignResult
public bool Success;
public string ReasonCode = string.Empty;                   // unknown_companion | companion_dead |
public static CompanionAssignResult Fail(string reason) => new CompanionAssignResult { Success = false, ReasonCode = reason };
public sealed class CompanionFeedResult
public bool Fed;
public string FoodItemId = string.Empty;                   // canonical item consumed ("" = none available)
public bool UsedEmergencyFallback;                          // fallback item: reduced benefit
public string ReasonCode = string.Empty;                   // no_food_available | already_fed | companion_dead
public sealed class CompanionAnimalSystem
public const string SystemId = "companion_animals";
public const int MaxBond = 100;
public const int TrainingProgressPerLevel = 10;
public const int HungerDailyGain = 25;             // unfed day
public const int HungerCritical = 80;              // health risk begins (§5.8: never one missed meal → death)
public const int HealthLossPerHungryDay = 6;
public const int HealthLossPerSickDay = 4;
public const int BondDailyCareGain = 2;            // fed + tended day (§5.6)
public const int BondHungerLossPerDay = 3;
public const int BondSickLossPerDay = 2;
public const int BondOnDeathOfHandler = 10;
public const int GriefMoraleShockMaxBp = -1500;    // bounded grief ceiling (§5.15 — morale authority applies)
public const int GriefMoraleFloorBp = -300;        // low-bond companions still register, smaller shock
public const float GuardBenefitHealthFloor = 0.4f; // injured/hungry animals fade (§5.10)
public const float PackBenefitHealthFloor = 0.4f;
public Func<double>? SicknessRoll;
public Func<string, bool>? KnownSpeciesCheck;
public event Action<CompanionState>? OnCompanionRegistered;
public event Action<CompanionState, CompanionRole>? OnRoleChanged;
public event Action<CompanionState, int>? OnHungerChanged;
public event Action<CompanionState, CompanionSicknessState>? OnSicknessChanged;
public event Action<CompanionState>? OnCompanionRecovered;
public event Action<CompanionState>? OnCompanionDied;
public CompanionSystemState State => _state;
public IReadOnlyCollection<CompanionSpeciesProfile> Profiles => _profiles.Values;
public void BindFoodPort(Func<string, int> count, Action<string, int> consume) {
public CompanionSpeciesProfile? Profile(string speciesId) =>
public CompanionAssignResult RegisterCompanion(string companionId, string speciesId, int tamedDay, string? name = null) {
public CompanionState? Companion(string companionId) =>
public float GetGuardModifierTotal() {
public float GetPackCapacityBonusForSurvivor(string survivorId) {
public void SetOnExpedition(string companionId, bool onExpedition) {
public CompanionAssignResult TreatSickness(string companionId, string itemId) {
public CompanionAssignResult Assign(string companionId, string survivorId, CompanionRole role, Func<string, bool>? survivorAlive = null) {
public void TickDay(int day) {
public CompanionFeedResult Feed(CompanionState c, CompanionSpeciesProfile profile, int day, IReadOnlyDictionary<string, string>? preferredTagItemMap = null) {
public float GetGuardModifier(string companionId) {
public float GetPackCapacityBonus(string companionId) {
public int GetMoraleSupportBp(string companionId) {
public int GetGriefMoraleDeltaBp(string companionId) {
public CompanionSystemState CaptureState() {
public void RestoreState(CompanionSystemState? state) {
public static class CompanionAnimalCatalogLoader
public const string FileName = "companion_animals.json";
public const int CurrentSchemaVersion = 1;
public static readonly IReadOnlyList<string> AcceptedRoleTags = new[] { "guard", "pack", "morale" };
public const int MaxFoodPerDay = 10;
public const int MaxHealth = 200;
public const int MaxTrainability = 10;
public const int MaxBondRate = 10;
public const int MaxGuardRating = 100;
public const int MaxPackCapacityKg = 60;
public const int MaxMoraleSupportBp = 500;
public const int MaxDiseaseResistance = 10;
public static CompanionCatalogLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<CompanionSpeciesProfile> ToProfiles(CompanionCatalogLoadResult result) {
```


# Appendix Q.573 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/ReconTelemetrySystem.cs`

### `Assets/Ashfall.Core/Expeditions/ReconTelemetrySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 330 lines / 14462 bytes.
- SHA-256: `dece5f6b24892465862a40dbbd5d85491f0d43da5c52f6a5529bda4f381a5546`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ReconTelemetrySystem
public const string SystemId = "recon_telemetry";
public ReconTelemetryState State => _state;
public IReadOnlyDictionary<string, ReconProbeDef> Platforms => _platforms;
public float GetRadioRangeMultiplier() {
public float GetRouteSpeedMultiplier(string routeId) {
public event Action<string>? OnReconLaunched;            // missionId
public event Action<string>? OnSurveyCompleted;         // missionId
public event Action<string, string>? OnPlatformLost;    // platformId, reason
public event Action<string>? OnPlatformRecovered;       // platformId
public event Action<string>? OnFalloutForecastGenerated;// forecastId
public event Action<string>? OnRouteScouted;            // routeId
public void RegisterPlatform(ReconProbeDef def) {
public void LoadCatalog(ReconTelemetryCatalog? catalog) {
public ActiveReconMissionState? GetMission(string missionId) {
public bool IsPlatformLaunched(string platformId) {
public LaunchResult LaunchMission(string platformId, string targetSectorId) {
public ActionResult RecoverPlatform(string missionId) {
public SurveyResult SurveySectors(string missionId, List<string> sectorIds) {
public ActionResult GenerateForecast(string platformId) {
public ActionResult ScoutRoute(string routeId, string missionId) {
public void TickDay(int day) {
public ReconTelemetryState CaptureState() => CloneState(_state);
public void RestoreState(ReconTelemetryState saved) {
public sealed class LaunchResult
public bool IsSuccess { get; }
public bool IsBlocked => !IsSuccess && FailureCode != null;
public string? FailureCode { get; }
public string MessageKey { get; }
public string MissionId { get; }
public static LaunchResult Failed(string code, string key) => new LaunchResult(false, code, key, string.Empty);
public static LaunchResult Blocked(string code, string key) => new LaunchResult(false, code, key, string.Empty);
public static LaunchResult Success(string missionId) => new LaunchResult(true, null, string.Empty, missionId);
public sealed class SurveyResult
public bool IsSuccess { get; }
public string FailureCode { get; }
public string MessageKey { get; }
public List<string> SurveyedSectors { get; }
public static SurveyResult Failed(string code, string key) => new SurveyResult(false, code, key, new List<string>());
public static SurveyResult Success(List<string> sectors) => new SurveyResult(true, null, string.Empty, sectors);
```


# Appendix Q.574 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/SurvivorIntegrityValidator.cs`

### `Assets/Ashfall.Core/Survivors/SurvivorIntegrityValidator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 405 lines / 18334 bytes.
- SHA-256: `951b61b24f77c559f6e50f0c14829b5f9a1026d563e439847bb6391dcc5d3bda`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum SurvivorIntegritySeverity
public static class SurvivorIntegrityCode
public const string LifecycleIllegalState = "lifecycle_illegal_state";
public const string LifecycleRevisionInvalid = "lifecycle_revision_invalid";
public const string LifecycleDayBeforeJoin = "lifecycle_day_before_join";
public const string DefinitionMissing = "definition_missing";
public const string AwayWithoutExpedition = "away_without_expedition";
public const string ExpeditionOnNonAway = "expedition_on_non_away";
public const string IterationOrderUnstable = "iteration_order_unstable";
public const string ComponentOwnerUnknown = "component_owner_unknown";
public const string ComponentOnDeceased = "component_on_deceased";
public const string ComponentMissingForEligible = "component_missing_for_eligible";
public const string ExpeditionMemberUnknown = "expedition_member_unknown";
public const string ExpeditionMemberNotAway = "expedition_member_not_away";
public const string ExpeditionIdMismatch = "expedition_id_mismatch";
public const string AwayWithoutActiveExpedition = "away_without_active_expedition";
public const string AssignmentOwnerUnknown = "assignment_owner_unknown";
public const string AssignmentLifecycleIneligible = "assignment_lifecycle_ineligible";
public sealed class SurvivorIntegrityFinding
public SurvivorIntegritySeverity Severity { get; }
public string Code { get; }
public SurvivorId SurvivorId { get; }
public string Component { get; }
public string Message { get; }
public override string ToString() {
public sealed class SurvivorIntegrityReport
public List<SurvivorIntegrityFinding> Findings { get; } = new List<SurvivorIntegrityFinding>();
public int SurvivorsChecked { get; internal set; }
public int ComponentStoresChecked { get; internal set; }
public int WarningCount => Findings.Count - ErrorCount;
public bool IsValid => ErrorCount == 0;
public bool IsClean => Findings.Count == 0;
public IEnumerable<SurvivorIntegrityFinding> BySeverity(SurvivorIntegritySeverity severity) {
public string Describe() {
public override string ToString() => $"[SurvivorIntegrity] survivors={SurvivorsChecked} errors={ErrorCount} warnings={WarningCount}";
public sealed class SurvivorIntegrityInputs
public IEnumerable<KeyValuePair<string, SurvivorId>>? ActiveExpeditions { get; set; }
public IEnumerable<SurvivorId>? AssignedSurvivors { get; set; }
public static class SurvivorIntegrityValidator
public static SurvivorIntegrityReport Validate( SurvivorEntityStore store, SurvivorIntegrityInputs? inputs = null) {
```


# Appendix Q.575 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/WastelandMapCatalogLoader.cs`

### `Assets/Ashfall.Core/World/WastelandMapCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 405 lines / 16928 bytes.
- SHA-256: `40b46e728b6d84f602291d2f454f91c7003048bedc37106bf198047ae40d215b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WastelandMapCatalogContainer
public int schema_version { get; set; }
public List<MapNodeDef> nodes { get; set; } = new List<MapNodeDef>();
public List<MapRouteDef> routes { get; set; } = new List<MapRouteDef>();
public List<TrapSiteMapLocationDef> trapSites { get; set; } = new List<TrapSiteMapLocationDef>();
public sealed class MapNodeDef
public string id { get; set; } = string.Empty;
public string displayName { get; set; } = string.Empty;
public string danger { get; set; } = "none";
public string? faction { get; set; }
public string? lootTable { get; set; }
public float positionX { get; set; }
public float positionY { get; set; }
public bool discoverable { get; set; }
public bool startingUnlocked { get; set; }
public sealed class MapRouteDef
public string from { get; set; } = string.Empty;
public string to { get; set; } = string.Empty;
public float distanceKm { get; set; }
public float weatherHazard { get; set; }
public string travelDomain { get; set; } = "land";
public float currentStrength { get; set; } = 0f;
public float toxicContamination { get; set; } = 0f;
public List<string> tags { get; set; } = new List<string>();
public bool HasTag(string tag) {
public bool IsFlooded => HasTag("flooded");
public bool IsAmphibious => HasTag("amphibious");
public sealed class TrapSiteMapLocationDef
public string siteId { get; set; } = string.Empty;
public string anchorNodeId { get; set; } = string.Empty;
public float offsetX { get; set; }
public float offsetY { get; set; }
public enum MapRouteErrorKind
public sealed class MapRouteValidationError
public string RouteDescription { get; set; } = string.Empty;
public string ErrorMessage { get; set; } = string.Empty;
public MapRouteErrorKind Kind { get; set; }
public static class WastelandMapCatalogLoader
public const string DefaultFileName = "wasteland_map_v1.json";
public const string TunnelFileName = "underground_tunnels.json";
public static List<MapRouteValidationError> ValidateRoutes( IReadOnlyList<MapNode> nodes, IReadOnlyList<MapRoute> routes) {
public static WastelandMapSystem CreateSystem( string dataDir, WastelandMapState? state = null, IFileIO? fileIO = null, IJsonSerializer? json = null) {
public static TunnelNetworkCatalogData? LoadTunnelCatalog(string dataDir, IFileIO? fileIO = null) {
public static List<TrapSiteMapLocation> LoadTrapSiteLocations( string dataDir, IFileIO? fileIO = null, IJsonSerializer? json = null) {
```


# Appendix Q.576 — Additional Current Architecture Evidence: `src/Host/SevenDayDeterministicSmokeTest.cs`

### `src/Host/SevenDayDeterministicSmokeTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 622 lines / 34110 bytes.
- SHA-256: `1a71e183eed8e9213203c4d9607bf8a1c622c49dc0ead88132c1fce6613ce081`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=1; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class SevenDayDeterministicSmokeTest
public static int Run(string dataDirectory) {
public int WeatherRollCount;
public WeatherKind FinalWeather;
public int WeatherChangeCount;
public SurvivorNeedsState? SurvivorA;
public SurvivorNeedsState? SurvivorB;
public List<string> MapDiscovered = new List<string>();
public List<string> MapLocked = new List<string>();
public List<string> MapCompleted = new List<string>();
public int WeatherRollCount;
public WeatherKind FinalWeather;
public SurvivorNeedsState? SurvivorA;
public SurvivorNeedsState? SurvivorB;
public List<string> MapDiscovered = new List<string>();
public List<string> MapLocked = new List<string>();
public List<string> MapCompleted = new List<string>();
public string Id = string.Empty;
public float Hunger;
public float Thirst;
public float Fatigue;
public float Warmth;
public float Morale;
public float Health;
public bool IsAlive;
public List<SmokeSurvivorSlice> Survivors = new List<SmokeSurvivorSlice>();
public SmokeRosterSaveState? State;
public string Checksum = string.Empty;
public WorldWeatherState? State;
public string Checksum = string.Empty;
```


# Appendix Q.577 — Additional Current Architecture Evidence: `src/Host/ContentUtilizationRuntimeCollector.cs`

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


# Appendix Q.578 — Additional Current Architecture Evidence: `src/Main.Anomaly.cs`

### `src/Main.Anomaly.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 247 lines / 11511 bytes.
- SHA-256: `dffb24685e8c191f57309453ea809cbe78ac2fe4315642a56a67698397e8b04c`.
- Architecture signals: seeded references=4; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public AnomalyHazardSystem EnsureAnomalyHazard() {
public void TickAnomalyHazard(int day) {
public float GetAnomalyLocationRate(string locationId) {
public float GetAnomalyDetectionCapability() {
public AnomalyLootResolution ResolveAnomalyLootSite(string siteId) {
public IReadOnlyDictionary<string, float>? BuildSectorHazardModifiers() {
```


# Appendix Q.579 — Additional Current Architecture Evidence: `src/Main.Plans202_205.cs`

### `src/Main.Plans202_205.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 304 lines / 14069 bytes.
- SHA-256: `bb5c916bd76d1650dc5fc32be94bfc59e730b4e425049f9d2c22aa0eb337e63f`.
- Architecture signals: seeded references=2; save/restore symbols=4; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public PlasticPyrolysisHostSession EnsurePlasticPyrolysis() {
public CargoAirdropHostSession EnsureCargoAirdrop() {
```


# Appendix Q.580 — Additional Current Architecture Evidence: `src/Audio/AudioEventBridge.cs`

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


# Appendix Q.581 — Additional Current Architecture Evidence: `src/Host/HostCli.WorldPlaytest.cs`

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


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.
