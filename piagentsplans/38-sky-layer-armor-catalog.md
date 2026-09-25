# Plan 38 — Sky-Layer Armor and Orbital Harrow Threat Integration

> **Rebuild status:** COMPLETE ARMOR/EVENT CONTENT AUTHORITY — WORLD ACTIVATION AND SAVE MAINTENANCE
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

- The original plan identified a real defense counter-play but the live implementation now has authored armor, orbital event, telemetry, world save, radio/sky-defense consumers and host/CLI probes.
- The correct authority chain is `sky_layer_armor_catalog.json` → `SkyLayerArmorCatalogLoader` → `SkyLayerArmorSystem`; `orbital_harrow_events.json` → `OrbitalHarrowCatalogLoader` → `OrbitalHarrowTelemetrySystem`; the world host owns the shared instances and save projection.
- The rebase protects one armor/telemetry instance, seeded event resolution, repair costs, false-positive semantics, world save restore and the boundary between sky defense batteries, radio warnings and excavation/weather consumers.

**Bounded outcome:** Retire the old “system exists/no data” premise. Current catalogs contain 6 armor configurations and 12 orbital events, the world-owned sky armor/telemetry loop is live, and focused tests cover installation, attenuation, breach, repair, false positives and restore. The remaining work is activation/reachability QA, not another threat catalog.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `sky_layer_armor_catalog.json` is present with 6 configurations; `orbital_harrow_events.json` is present with 12 events.
- `SkyLayerArmorSystem` owns cells, attenuation, impact and repair; `OrbitalHarrowTelemetrySystem` owns activation, warnings, scheduling, resolution and salvage.
- `WorldHostSession` owns `SkyArmor` and `WeatherIntelligence.Orbital`, exposes capture/restore and host probes; `RadioIntelligencePanel` and sky-defense systems consume the same telemetry seam.
- Focused tests cover all authored rows, material references, hierarchy, mitigation/breach, repair, false positives, dead-hand hooks, save and full defense loop.

**Master-authority sections applied to this rebase:**

- Master authority Volume 28 verification cookbook: focused evidence before broad gates.
- Lane D save/state/compatibility guidance: owner DTOs, migration and restore proof.
- Lane E UI/UX/accessibility guidance: truthful projections and keyboard/controller lifecycle.
- Lane G testing guidance: smallest affected target, negative cases and deterministic replay.
- Anti-padding protocol: content exhaustion may end the plan before the character checkpoint.
- Volume 32 world-event and shared-owner guidance.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 6/10 target with a 6-armor/12-event current census.
- Document the shared world-owned telemetry/armor instance and all current consumers.
- Verify event activation, warning, brace, impact, repair, salvage and save behavior in the live host route.
- Reject a separate threat generator, armor save or UI-local impact calculation.

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
| armor configuration definitions | SkyLayerArmorCatalogLoader | `Assets/Ashfall.Core/Shelter/SkyLayerArmorCatalog.cs` | Sole armor catalog authority. |
| armor cells, attenuation, breach and repair | SkyLayerArmorSystem | `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs` | Owns armor state and impact calculation. |
| authored orbital event definitions | OrbitalHarrowCatalogLoader | `Assets/Ashfall.Core/Shelter/OrbitalHarrowCatalog.cs` | Sole orbital event catalog authority. |
| telemetry, warnings, impacts and salvage | OrbitalHarrowTelemetrySystem | `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` | Owns orbital runtime state. |
| shared instance composition and save | WorldHostSession | `src/Host/WorldHostSession.cs; src/Main.FlagshipInstitutions.cs` | Owns host wiring/world save projection. |
| warning/interception/panel facts | Sky defense/radio consumers | `Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs; src/UI/RadioIntelligencePanel.cs` | Consume the canonical telemetry owner. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Sky-Layer Armor and Orbital Harrow Threat Integration
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ SkyLayerArmorCatalogLoader
│   armor configuration definitions
│ SkyLayerArmorSystem
│   armor cells, attenuation, breach and repair
│ OrbitalHarrowCatalogLoader
│   authored orbital event definitions
│ OrbitalHarrowTelemetrySystem
│   telemetry, warnings, impacts and salvage
│ WorldHostSession
│   shared instance composition and save
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

1. **Preserve current state ownership.** SkyLayerArmorCatalogLoader owns armor configuration definitions: Sole armor catalog authority.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| armor configuration definitions | SkyLayerArmorCatalogLoader | `Assets/Ashfall.Core/Shelter/SkyLayerArmorCatalog.cs` | Sole armor catalog authority. |
| armor cells, attenuation, breach and repair | SkyLayerArmorSystem | `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs` | Owns armor state and impact calculation. |
| authored orbital event definitions | OrbitalHarrowCatalogLoader | `Assets/Ashfall.Core/Shelter/OrbitalHarrowCatalog.cs` | Sole orbital event catalog authority. |
| telemetry, warnings, impacts and salvage | OrbitalHarrowTelemetrySystem | `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` | Owns orbital runtime state. |
| shared instance composition and save | WorldHostSession | `src/Host/WorldHostSession.cs; src/Main.FlagshipInstitutions.cs` | Owns host wiring/world save projection. |
| warning/interception/panel facts | Sky defense/radio consumers | `Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs; src/UI/RadioIntelligencePanel.cs` | Consume the canonical telemetry owner. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load armor and orbital catalogs
2. construct one world-owned armor/telemetry pair
3. activate telemetry and schedule authored event
4. emit warning and allow brace/interception through current consumers
5. resolve impact against shared armor cells
6. apply damage/degradation/breach and salvage opportunity
7. repair through canonical inventory/material bill
8. capture/restore through WorldHostSession

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Armor configurations and event definitions are immutable catalog data; installed cells, durability, warnings and salvage are runtime state.
- A false-positive event resolves without damage/breach and must not create a false salvage claim.
- Armor repair consumes the current material/owner bill and changes one shared instance.
- World restore rehydrates armor and telemetry without replaying past impacts or salvage.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Every armor material reference resolves in the current item catalog.
- A scheduled event has stable warning/impact timing and deterministic resolution for the same seed/state.
- A false positive cannot breach, damage or grant salvage.
- No consumer creates a second armor or telemetry state owner.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `sky_layer_armor_catalog.json` and `orbital_harrow_events.json` are separate current authorities.
- Do not merge threat events into armor rows or duplicate them in a new catalog.
- New events/armor require current consumers, bounded energy/durability values and focused tests.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the existing world save section and WorldHostSession armor/telemetry capture-restore methods.
- No new sky armor save section is justified.
- Legacy missing telemetry/armor fields restore neutral and cannot re-fire old impacts.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Event scheduling/resolution uses the injected seeded RNG.
- Catalog ordering and salvage selection are stable.
- Paired runs and save/restore runs produce identical warnings, damage and salvage.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Telemetry warnings and impact resolution are emitted by the current orbital owner.
- Sky-defense interception calls the canonical telemetry mitigation seam.
- World host dirty/save state marks the existing world section after owner mutations.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/WorldHostSession.cs
- src/Main.FlagshipInstitutions.cs
- src/Main.Plans46_49.cs
- src/UI/RadioIntelligencePanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Orbital events and armor should remain fictional, restrained and consequence-oriented.
- False positives and dead-hand signals must be readable uncertainty, not arbitrary gotchas.
- The threat should interact with shelter preparation rather than create an isolated minigame.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | World and flagship paths construct different armor/telemetry instances. | SkyLayerArmorCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A false positive deals damage or grants salvage. | SkyLayerArmorSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A breach is applied twice to the same cell. | OrbitalHarrowCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A repair consumes materials outside inventory transaction. | OrbitalHarrowTelemetrySystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A restore replays an old orbital event or loses salvage dedupe. | WorldHostSession | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/SkyLayerArmorCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/OrbitalHarrowTelemetrySystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/World/WorldHostSessionTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/SkyDefenseBatteryTests.cs`
6. `bash scripts/run_test.sh Ashfall.Core.Tests/StandaloneCoreSystemTests.cs`
7. `bash scripts/run_test.sh Ashfall.Core.Tests/IslandBridgesTests.cs`
8. `bash scripts/run_test.sh Ashfall.Core.Tests/WeatherIntelligenceCoordinatorTests.cs`
9. `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/CrisisPredictionTests.cs`
10. `bash scripts/run_test.sh Ashfall.Core.Tests/World/ExcavationHazardSystemTests.cs`
11. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/RadioStationCatalogTests.cs`
12. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/RadioStationParityTests.cs`
13. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/ShelterRadioStationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — current census | Read both catalogs, loaders, world host and tests. | 6/12 current authority and owners are proven. | No production path until the owning implementation package is separately claimed. |
| 1 — shared-instance audit | Trace world, flagship, radio and sky-defense references. | Exactly one live armor/telemetry pair. | No production path until the owning implementation package is separately claimed. |
| 2 — impact/repair proof | Trace warning, brace, impact, breach, repair and salvage. | No duplicate or false-positive effects. | No production path until the owning implementation package is separately claimed. |
| 3 — save/UI seal | Verify world restore and accessible presentation. | Current player surface explains the event. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/sky_layer_armor_catalog.json | READ ONLY; MODIFY only for proven row gap | 6-row authority |
| Assets/StreamingAssets/Data/orbital_harrow_events.json | READ ONLY | 12-row event authority |
| Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs | READ ONLY | Armor owner |
| Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs | READ ONLY | Telemetry owner |
| src/Host/WorldHostSession.cs | READ ONLY | Shared host/save owner |
| src/UI/RadioIntelligencePanel.cs | READ ONLY | Current warning projection |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Parallel armor/telemetry instances. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding a second orbital threat generator. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Double-applying impact or salvage. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Editing a dirty host seam without a new claim. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new armor/event count for this rebase.
- No new orbital system.
- No save-section change.
- No production edits in this rebase.

# 23. Rollback and Recovery

- Revert the plan file.
- Future data/host changes retain prior catalogs, world fixture and focused impact/restore tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 6 armor configurations and 12 events are documented.
- Shared instance and save ownership are explicit.
- False-positive, impact, repair and salvage contracts are named.
- No parallel authority is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 6/10 target with a 6-armor/12-event current census.
- Document the shared world-owned telemetry/armor instance and all current consumers.
- Verify event activation, warning, brace, impact, repair, salvage and save behavior in the live host route.
- Reject a separate threat generator, armor save or UI-local impact calculation.

## MUST NOT DO

- No new armor/event count for this rebase.
- No new orbital system.
- No save-section change.
- No production edits in this rebase.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/SkyLayerArmorCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/OrbitalHarrowTelemetrySystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/World/WorldHostSessionTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/SkyDefenseBatteryTests.cs`
6. `bash scripts/run_test.sh Ashfall.Core.Tests/StandaloneCoreSystemTests.cs`
7. `bash scripts/run_test.sh Ashfall.Core.Tests/IslandBridgesTests.cs`
8. `bash scripts/run_test.sh Ashfall.Core.Tests/WeatherIntelligenceCoordinatorTests.cs`
9. `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/CrisisPredictionTests.cs`
10. `bash scripts/run_test.sh Ashfall.Core.Tests/World/ExcavationHazardSystemTests.cs`
11. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/RadioStationCatalogTests.cs`
12. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/RadioStationParityTests.cs`
13. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/ShelterRadioStationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — current census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: armor configuration definitions → SkyLayerArmorCatalogLoader; armor cells, attenuation, breach and repair → SkyLayerArmorSystem; authored orbital event definitions → OrbitalHarrowCatalogLoader; telemetry, warnings, impacts and salvage → OrbitalHarrowTelemetrySystem; shared instance composition and save → WorldHostSession; warning/interception/panel facts → Sky defense/radio consumers. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 38.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 38 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by SkyLayerArmorCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Shelter/SkyLayerArmorCatalog.cs`

### `Assets/Ashfall.Core/Shelter/SkyLayerArmorCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 221 lines / 11073 bytes.
- SHA-256: `c10d386d924cbefdb2ab02a84725411f7e032061be87d420079bbde51a9f7a7a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ArmorMaterialCostDef
public string item_id { get; set; } = string.Empty;
public int quantity { get; set; } = 1;
public sealed class SkyLayerArmorConfigDef
public string id { get; set; } = string.Empty;
public string name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public string tier { get; set; } = "improvised"; // improvised, reinforced, military_grade
public CeilingMaterialTier material_tier { get; set; } = CeilingMaterialTier.Dirt;
public float default_thickness_meters { get; set; } = 1.0f;
public float blast_resistance_mj { get; set; } = 10f;
public float attenuation_factor { get; set; } = 0.5f;
public float degradation_rate { get; set; } = 0.2f;
public List<ArmorMaterialCostDef> composition { get; set; } = new List<ArmorMaterialCostDef>();
public List<ArmorMaterialCostDef> repair_cost { get; set; } = new List<ArmorMaterialCostDef>();
public sealed class SkyLayerArmorCatalogContainer
public int schema_version { get; set; } = 1;
public List<SkyLayerArmorConfigDef> configurations { get; set; } = new List<SkyLayerArmorConfigDef>();
public static class SkyLayerArmorCatalogLoader
public const string CatalogFileName = "sky_layer_armor_catalog.json";
public static List<SkyLayerArmorConfigDef> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null) {
public static List<SkyLayerArmorConfigDef> GetDefaultConfigurations() {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs`

### `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 143 lines / 5219 bytes.
- SHA-256: `15e0b1b127dc15d88f38dd96f25d432cddad739e13fed2897b6349a86d530f39`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum CeilingMaterialTier { Dirt, Wood, ReinforcedConcrete, LeadSheeting, TungstenComposite } [Serializable] public sealed class CeilingCellArmor { public int gridX; public CeilingMaterialTier material; public float thicknessMeters; public float currentDurability; // 0.0 to 100.0 }
public sealed class SkyArmorSaveState
public List<CeilingCellArmor> cells = new List<CeilingCellArmor>();
public sealed class SkyLayerArmorSystem
public void SetCellArmor(int gridX, CeilingMaterialTier material, float thicknessMeters, float durability = 100f) {
public void InstallConfiguration(int gridX, SkyLayerArmorConfigDef config) {
public CeilingCellArmor? GetCell(int gridX) {
public float GetAttenuationFactor(int gridX) {
public void RepairCell(int gridX, float durabilityAmount) {
public bool EvaluateKineticImpact(int gridX, float impactEnergyMegaJoules, out float damageDealtToRoof) {
public SkyArmorSaveState CaptureState() {
public void RestoreState(SkyArmorSaveState state) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Shelter/OrbitalHarrowCatalog.cs`

### `Assets/Ashfall.Core/Shelter/OrbitalHarrowCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 69 lines / 2205 bytes.
- SHA-256: `6d77064ecd5864203314d577462cca2cd002fb00fa2519cef11d407e8a346555`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class OrbitalEventDef
public string id = string.Empty;
public string name = string.Empty;
public string description = string.Empty;
public string severity = "Minor";
public string signal_type = "radar_anomaly";
public bool is_false_positive = false;
public float impact_energy_mj = 10f;
public int lead_time_days = 3;
public int affected_cell_spread = 1;
public float penetration_power_mj = 8f;
public string salvage_yield_item_id = string.Empty;
public int salvage_yield_quantity = 1;
public string revealed_site_id = string.Empty;
public string radio_hook_text = string.Empty;
public sealed class OrbitalHarrowCatalogFile
public int schema_version = 1;
public List<OrbitalEventDef> events = new List<OrbitalEventDef>();
public static class OrbitalHarrowCatalogLoader
public const string FileName = "orbital_harrow_events.json";
public static List<OrbitalEventDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs`

### `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 331 lines / 14047 bytes.
- SHA-256: `818eb4a35846d0e2ae3324a973af8cafe3c4aa1a966c9d5c9b079cbe63185d37`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class OrbitalSalvageOpportunity
public string eventId = string.Empty;
public string itemId = string.Empty;
public int quantity = 1;
public int targetGridX;
public int spawnDay;
public int expiresDay;
public bool isClaimed;
public sealed class OrbitalTelemetryState
public string systemId = OrbitalHarrowTelemetrySystem.SystemId;
public bool telemetryActive;
public int lastImpactDay = -1;
public int nextImpactDay = -1;
public int warningLeadDays = 3;
public int targetGridX = -1;
public int affectedCellSpread = 1;
public float impactEnergyMj = 10f;
public string scheduledEventId = string.Empty;
public string scheduledEventName = string.Empty;
public string revealedSiteId = string.Empty;
public bool isBraced;
public bool braceUsed;
public List<int> impactHistory = new List<int>();
public List<OrbitalWarningEntry> warnings = new List<OrbitalWarningEntry>();
public List<OrbitalSalvageOpportunity> activeSalvage = new List<OrbitalSalvageOpportunity>();
public List<string> revealedSites = new List<string>();
public sealed class OrbitalWarningEntry
public int day;
public int targetGridX;
public float energyMj;
public string eventId = string.Empty;
public string telemetryText = string.Empty;
public string severity = "Minor";
public sealed class OrbitalImpactReport
public int Day;
public string EventId = string.Empty;
public int TargetGridX;
public int CellsAffected;
public float TotalEnergyMj;
public bool AnyBreached;
public float TotalPenetrationDamage;
public float PowerGridDisruption;
public string SalvageItemId = string.Empty;
public int SalvageQuantity;
public string RevealedSiteId = string.Empty;
public sealed class OrbitalHarrowTelemetrySystem
public const string SystemId = "orbital_harrow_telemetry";
public OrbitalTelemetryState State => _state;
public bool HasPendingImpact => _state.nextImpactDay > _currentDay;
public IReadOnlyList<OrbitalSalvageOpportunity> ActiveSalvage => _state.activeSalvage.AsReadOnly();
public IReadOnlyList<string> RevealedSites => _state.revealedSites.AsReadOnly();
public event Action<OrbitalWarningEntry> OnImpactWarning;
public event Action<int, float> OnImpactResolved; // day, energy
public event Action<OrbitalImpactReport> OnImpactDetailed;
public event Action OnTelemetryChanged;
public void ActivateTelemetry(int day) {
public void ScheduleImpact(int day, int gridX, float energyMj) {
public void ScheduleEventDef(OrbitalEventDef def, int day, int gridX) {
public ActionResult Brace(string materialId, int amount) {
public void TickDay(int day) {
public bool ApplyInterceptionMitigation(string eventId, float residualFraction) {
public ActionResult ClaimSalvage(string eventId) {
public OrbitalTelemetryState CaptureState() => CloneState(_state);
public void RestoreState(OrbitalTelemetryState saved) {
```


# Appendix B.06 — Current Code Architecture: `Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs`

### `Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 458 lines / 21449 bytes.
- SHA-256: `54003f6099654f01b0af26393e47e549ab167afab543531e23cb388733359f27`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=10; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CounterBatteryTurretState
public string turret_id = string.Empty;
public int azimuth;                                // 0..359
public int elevation;                              // 0..90
public int barrel_heat;                            // 0..100
public string loaded_ammo_id = string.Empty;
public int magazine_count;                         // rounds in the loaded magazine
public int radar_calibration = 70;                 // 0..100
public int hydraulic_condition = 100;              // 0..100
public int volleys_since_service;
public bool is_operational = true;
public List<string> assigned_crew_ids = new();
public sealed class OrbitalTrackState
public string track_id = string.Empty;             // telemetry event id (dedup key)
public int warning_day = -1;
public int impact_day = -1;
public int target_grid_x;
public float energy_mj;
public string severity = "Moderate";
public int volleys_fired;
public bool resolved;
public sealed class SkyDefenseBatterySave
public int schema_version = 1;
public List<CounterBatteryTurretState> turrets = new();
public List<OrbitalTrackState> tracks = new();
public int total_interceptions;
public int total_volleys;
public sealed class SkyDefenseBatterySystem
public const string SystemId = "sky_defense_battery";
public const string InstitutionId = "institution_sky_defense";
public const int BaseInterceptChance = 45;
public const int MinInterceptChance = 15;
public const int MaxInterceptChance = 85;
public const int HeatSeizureThreshold = 90;
public const int DailyHeatDissipation = 30;
public const int DailyRadarDrift = 2;
public const int VolleysPerService = 10;
public const string ServiceOilItemId = "machine_oil";
public const string DefaultTurretId = "turret_main_battery";
public event Action<OrbitalTrackState>? OnOrbitalTrackAcquired;
public event Action<string, string, int>? OnVolleyFired;             // turret, ammo, magazine left
public event Action<string, string, bool, float>? OnInterceptResolved; // track, ammo, success, residual fraction
public event Action<string>? OnMaintenanceDue;                       // turret
public event Action<string>? OnServiced;                             // turret
public void LoadOrdnanceCatalog(List<SkyDefenseOrdnanceDefinition> ordnance) {
public CounterBatteryTurretState EnsureDefaultTurret() {
public IReadOnlyList<CounterBatteryTurretState> Turrets => _state.turrets.AsReadOnly();
public IReadOnlyList<OrbitalTrackState> Tracks => _state.tracks.AsReadOnly();
public IReadOnlyDictionary<string, SkyDefenseOrdnanceDefinition> OrdnanceCatalog => _ordnance;
public int TotalInterceptions => _state.total_interceptions;
public int TotalVolleys => _state.total_volleys;
public SkyDefenseOrdnanceDefinition? GetOrdnance(string ordnanceId) =>
public CounterBatteryTurretState? GetTurret(string turretId) =>
public OrbitalTrackState? GetTrack(string trackId) =>
public ActionResult TryLoadMagazine(string turretId, string ordnanceId) {
public ActionResult TryAssignCrew(string turretId, string survivorId) {
public ActionResult TryRemoveCrew(string turretId, string survivorId) {
public int PreviewInterceptChance(CounterBatteryTurretState turret, OrbitalTrackState track, SkyDefenseOrdnanceDefinition ordnance) =>
public ActionResult TryFireVolley(string turretId, string trackId) {
public ActionResult TryServiceHydraulics(string turretId) {
public void TickDay(int day) {
public SkyDefenseBatterySave CaptureState() => Clone(_state);
public void RestoreState(SkyDefenseBatterySave? saved) {
```


# Appendix B.07 — Current Code Architecture: `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs`

### `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 355 lines / 15367 bytes.
- SHA-256: `962ab63c433b2bf189343ca17c0807b637189bcc8b1ec4c08d6b82a0725139b5`.
- Architecture signals: seeded references=6; save/restore symbols=10; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WeatherIntelligenceSaveState
public WeatherStationState station = new WeatherStationState();
public OrbitalTelemetryState orbital = new OrbitalTelemetryState();
public SeasonalEventSaveState seasonal = new SeasonalEventSaveState();
public CloudSeedingSaveState? cloudSeeding;
public sealed class WeatherIntelligenceReadModel
public string seasonId = string.Empty;
public string seasonDisplayName = string.Empty;
public bool stationInstalled;
public bool stationCalibrated;
public bool stationOperational;
public WeatherStationTier stationTier;
public string stationTierName = string.Empty;
public float stationAccuracy;
public float stationDurability;
public int forecastHorizonDays;
public int lastForecastDay;
public List<ForecastEntry> forecast = new List<ForecastEntry>();
public bool telemetryActive;
public bool hasPendingImpact;
public int impactDay;
public int warningLeadDays;
public int daysUntilImpact;
public int activeSalvageCount;
public List<OrbitalSalvageOpportunity> activeSalvage = new List<OrbitalSalvageOpportunity>();
public List<ActiveSeasonalEvent> activeSeasonalEvents = new List<ActiveSeasonalEvent>();
public int routeSafeDays;
public int bestTravelDay;
public float bestTravelConfidence;
public string advisory = string.Empty;
public string? predictedCrisisEventId;
public int predictedCrisisDay;
public float predictedCrisisConfidence;
public string? crisisPreparationAdvice;
public WeatherKind? predictedWeatherKind;
public bool hasPredictedCrisis;
public int daysUntilPredictedCrisis;
public string predictionSource = string.Empty;
public bool isStationCalibrated;
public bool cloudSeedingInstalled;
public bool cloudSeedingOnCooldown;
public int cloudSeedingCooldownDays;
public bool cloudSeedingPartialProtectionActive;
public sealed class WeatherIntelligenceCoordinator
public WeatherStationSystem Station { get; }
public OrbitalHarrowTelemetrySystem Orbital { get; }
public SeasonalEventSystem Seasonal { get; }
public CloudSeedingSystem CloudSeeding { get; }
public event Action? OnIntelligenceChanged;
public void TickDay(int day) {
public WeatherIntelligenceReadModel BuildReadModel() {
public WeatherIntelligenceSaveState CaptureState() {
public void RestoreState(WeatherIntelligenceSaveState? saved) {
```


# Appendix B.08 — Current Code Architecture: `Assets/Ashfall.Core/Radio/ShelterRadioStationSystem.cs`

### `Assets/Ashfall.Core/Radio/ShelterRadioStationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 429 lines / 15803 bytes.
- SHA-256: `7f51d2e6e1521e4589ffdf11ee4b62778fc2ebe4f83b638ddee1943f9c41887e`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=21; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RadioEncryptionScheme
public string Scheme { get; set; } = "none";
public int Difficulty { get; set; }
public List<string> RequiredSkillIds { get; set; } = new List<string>();
public sealed class RadioTriangulationData
public int RequiredBearings { get; set; } = 2;
public string RevealedLocationId { get; set; } = string.Empty;
public sealed class RadioInterceptDefinition
public string Id { get; set; } = string.Empty;
public string Callsign { get; set; } = string.Empty;
public int FrequencyKhz { get; set; } = 7000;
public string Band { get; set; } = "hf";
public string SignalClass { get; set; } = "chatter";
public string SourceFactionId { get; set; } = string.Empty;
public float BaseSignalStrength { get; set; } = 0.5f;
public RadioEncryptionScheme Encryption { get; set; } = new RadioEncryptionScheme();
public RadioTriangulationData Triangulation { get; set; } = new RadioTriangulationData();
public int ExpiryDays { get; set; } = 3;
public string Message { get; set; } = string.Empty;
public List<string> Tags { get; set; } = new List<string>();
public sealed class RadioInterceptCatalogData
public int SchemaVersion { get; set; } = 1;
public List<RadioInterceptDefinition> Intercepts { get; set; } = new List<RadioInterceptDefinition>();
public sealed class InterceptProgress
public string InterceptId { get; set; } = string.Empty;
public bool Detected { get; set; }
public int SignalLockPermille { get; set; }
public int DecryptProgressPermille { get; set; }
public int BearingsCollected { get; set; }
public List<int> DistinctAzimuths { get; set; } = new List<int>();
public bool Resolved { get; set; }
public bool IsDecrypted { get; set; }
public int DetectedDay { get; set; }
public int? ExpiresOnDay { get; set; }
public bool IsExpired { get; set; }
public sealed class RadioStationStateSave
public string systemId = ShelterRadioStationSystem.SystemId;
public int schemaVersion = 1;
public int tunedFrequencyKhz = 7115;
public string bandId = "hf";
public int antennaAzimuthDegrees = 0;
public bool isOperational = true;
public List<InterceptProgress> intercepts = new List<InterceptProgress>();
public List<string> discoveredLocationIds = new List<string>();
public List<string> decodedIntelligenceLogs = new List<string>();
public int currentDay;
public sealed record RadioScanResult(
public sealed class ShelterRadioStationSystem
public const string SystemId = "radio_station";
public RadioStationStateSave State => _state;
public IReadOnlyDictionary<string, RadioInterceptDefinition> Catalog => _catalog;
public event Action<string>? OnInterceptDetected;
public event Action<string>? OnInterceptDecrypted;
public event Action<string, string>? OnLocationTriangulated; // interceptId, locationId
public event Action<string>? OnDistressExpired;
public event Action<OrbitalWarningEntry>? OnOrbitalWarningRelayed;
public event Action? OnRadioStateChanged;
public void BindSkillProvider(Func<string, float> provider) => _operatorSkillProvider = provider;
public void BindWeatherNoiseProvider(Func<float> provider) => _weatherNoiseProvider = provider;
public void LoadCatalog(RadioInterceptCatalogData? data) {
public void LoadCatalog(string json) {
public void TuneTo(int frequencyKhz, string band = "hf") {
public void SetAntennaAzimuth(int degrees) {
public InterceptProgress GetOrCreateInterceptProgress(string interceptId) {
public RadioScanResult ScanFrequency(int day) {
public int ProgressDecryption(string interceptId, float skillBonus = 1.0f) {
public bool RecordBearing(string interceptId, int azimuthDegrees) {
public void TickDay(int day) {
public OrbitalWarningEntry? CheckOrbitalEarlyWarning(int currentDay) {
public RadioStationStateSave CaptureState() {
public void RestoreState(RadioStationStateSave? saved) {
```


# Appendix B.09 — Current Code Architecture: `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs`

### `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 482 lines / 19643 bytes.
- SHA-256: `2251ec3acbcde282740467faea55ab88e306def6f56f2df73be2d5484765a42f`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=21; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MitigationItemCost
public string ItemId { get; set; } = string.Empty;
public int Amount { get; set; } = 1;
public sealed class ExcavationMitigationDefinition
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public List<string> HazardTags { get; set; } = new List<string>();
public List<MitigationItemCost> RequiredItems { get; set; } = new List<MitigationItemCost>();
public int LaborTicks { get; set; } = 60;
public Dictionary<string, int> Effect { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
public bool RequiresRespiratoryProtection { get; set; }
public List<string> Tags { get; set; } = new List<string>();
public sealed class ExcavationHazardCatalogData
public int SchemaVersion { get; set; } = 1;
public List<ExcavationMitigationDefinition> Mitigations { get; set; } = new List<ExcavationMitigationDefinition>();
public sealed class ExcavationSectorHazardState
public string SectorId { get; set; } = string.Empty;
public int MethanePpm { get; set; } = 500; // 0 - 10000+
public int FloodLevelPermille { get; set; } = 0; // 0 - 1000
public int SporeConcentrationPermille { get; set; } = 0; // 0 - 1000
public int ShoringHealthPermille { get; set; } = 1000; // 0 - 1000
public bool IsBulkheadSealed { get; set; }
public List<string> InstalledMitigationIds { get; set; } = new List<string>();
public List<string> ActiveTrappedMiners { get; set; } = new List<string>();
public int? RescueDeadlineDay { get; set; }
public int RescueLaborRemaining { get; set; }
public bool RescueCompleted { get; set; }
public bool RescueFailed { get; set; }
public sealed class ExcavationHazardSave
public string systemId = ExcavationHazardSystem.SystemId;
public int schemaVersion = 1;
public Dictionary<string, ExcavationSectorHazardState> sectors = new(StringComparer.Ordinal);
public int currentDay;
public sealed class ExcavationHazardSystem
public const string SystemId = "excavation_hazards";
public ExcavationHazardSave State => _state;
public IReadOnlyDictionary<string, ExcavationMitigationDefinition> Catalog => _catalog;
public const int MethaneIgnitionThresholdPpm = 4000;
public const int FloodCriticalThresholdPermille = 500;
public event Action<string, string>? OnMitigationInstalled; // sectorId, mitigationId
public event Action<string>? OnMethaneIgnition; // sectorId
public event Action<string>? OnSectorFlooded;
public event Action<string, int>? OnRescueStarted; // sectorId, trappedCount
public event Action<string>? OnRescueSucceeded;
public event Action<string>? OnRescueFailed;
public event Action? OnHazardStateChanged;
public void LoadCatalog(ExcavationHazardCatalogData? data) {
public void LoadCatalog(string json) {
public ExcavationSectorHazardState GetOrCreateSector(string sectorId) {
public bool CanApplyMitigation(string sectorId, string mitigationId, out string reason) {
public ActionResult TryApplyMitigation(string sectorId, string mitigationId, IReadOnlyList<string>? workerIds = null) {
public ActionResult TryToggleBulkhead(string sectorId, bool seal, out string reason) {
public void TriggerCaveInRescue( string sectorId, IReadOnlyList<string> trappedSurvivorIds, int deadlineDays = 3, int requiredLabor = 240) {
public void ProgressRescueLabor(string sectorId, int laborAmount) {
public void AddMethane(string sectorId, int deltaPpm, int cap = int.MaxValue) {
public bool AddFloodWater(string sectorId, int deltaPermille) {
public void TickDay(int day) {
public ExcavationHazardSave CaptureState() {
public void RestoreState(ExcavationHazardSave? saved) {
```


# Appendix B.10 — Current Code Architecture: `Assets/Ashfall.Core/SkyDefense/SkyDefenseOrdnanceCatalog.cs`

### `Assets/Ashfall.Core/SkyDefense/SkyDefenseOrdnanceCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 100 lines / 5050 bytes.
- SHA-256: `61b85518aff7b7ffd129c4dc3b1efe61952e13eb34735c96b3b1b11e341699a8`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SkyDefenseOrdnanceDefinition
public string ordnance_id = string.Empty;
public string display_name = string.Empty;
public string ammo_type = string.Empty;
public string item_id = string.Empty;
public int magazine_units;                 // rounds per loaded magazine (>0)
public float tracking_modifier;            // -2..+2 added to intercept chance
public float interception_modifier;        // -0.2..+0.4 added to intercept chance
public int heat_per_volley;                // 0..100 barrel heat per volley
public int recoil_load;                    // 0..10 hydraulic wear per volley
public float burst_radius_units;           // game abstraction units
public float interception_ceiling_units;   // game abstraction units
public int radar_lock_units;               // game ticks of lock time
public float fragmentation_density;        // 0..1 authored effectiveness share
public float propellant_grain_kg;          // authored flavor/logistics figure
public float residual_shrapnel_severity;   // 0..1 fraction of strike kept as shrapnel on success
public List<string>? tags;
public sealed class SkyDefenseOrdnanceCatalogContainer
public List<SkyDefenseOrdnanceDefinition> ordnance = new();
public static class SkyDefenseOrdnanceCatalogLoader
public const string DefaultFileName = "sky_defense_ordnance.json";
public const int ExpectedOrdnanceCount = 6;
public static List<SkyDefenseOrdnanceDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static void Validate(string catalogName, List<SkyDefenseOrdnanceDefinition>? ordnance) {
```


# Appendix B.11 — Current Code Architecture: `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs`

### `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 162 lines / 5893 bytes.
- SHA-256: `b2c2eff4efe46878bdcaa00a6c2f059d38c78fe15905c85b639d0a61696b2ac2`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RadioStationCatalog
public const string StationCivilDefense = "station_civil_defense";
public const string StationGarrisonOverlord = "station_garrison_overlord";
public const string StationVitrifiedCrater = "station_vitrified_crater";
public const string StationOpenClassroom = "station_open_classroom";
public const string StationNumbersSigint = "station_numbers_sigint";
public const string StationAutomatedRelay = "station_automated_relay";
public IReadOnlyCollection<RadioStationDefinition> AllStations => _stations.Values;
public void Clear() {
public int LoadFromJson(string json) {
public int LoadFromDataDirectory(string dataDir) {
public void Register(RadioStationDefinition def) {
public RadioStationDefinition? GetStation(string stationId) {
public RadioStationDefinition? FindStationAtFrequency(float frequencyMhz, float toleranceMhz = 0.5f) {
public RadioStationState GetStationState(string stationId) {
public void SetStationState(string stationId, RadioStationState state) {
public void ResetOverrides() {
public Dictionary<string, RadioStationState> ExportOverrides() {
public void ImportOverrides(IDictionary<string, RadioStationState>? overrides) {
public RadioProgramSlot? GetCurrentSlot(string stationId, int campaignDay, int hour) {
public RadioProgramSlot? GetNextSlot(string stationId, int campaignDay, int hour) {
public RadioSignalStrength ComputeSignalStrength(string stationId, RadioReceptionFactors? factors) {
```


# Appendix B.12 — Current Code Architecture: `Assets/Ashfall.Core/Radio/RadioStationCatalogLoader.cs`

### `Assets/Ashfall.Core/Radio/RadioStationCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 122 lines / 4873 bytes.
- SHA-256: `244d1664c34881812ec29dc4cf5fcf1ba8904cce3b31cbec3a1dd9a75a0706fa`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class RadioStationCatalogLoader
public const string StationsFileName = "radio_stations.json";
public const float MinFrequencyMhz = 0.1f;
public const float MaxFrequencyMhz = 1000.0f;
public static int LoadAndRegister( RadioStationCatalog catalog, string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null) {
public static int LoadFromJsonString(RadioStationCatalog catalog, string json, string sourcePath = "<json>") {
```


# Appendix B.13 — Current Code Architecture: `src/Host/WorldHostSession.cs`

### `src/Host/WorldHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 402 lines / 19415 bytes.
- SHA-256: `b932e2d0b66a2c471eb95d222294abeb1f921424e2d1b157d41c53c32c68a816`.
- Architecture signals: seeded references=1; save/restore symbols=15; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WorldHostSession
public const int DemoSeed = 1234;
public WeatherSystem Weather { get; }
public SkyLayerArmorSystem SkyArmor { get; }
public WeatherIntelligenceCoordinator WeatherIntelligence { get; }
public LocationEvolutionSystem LocationEvolution { get; }
public WildlifeMigrationSystem Wildlife { get; }
public LandmarkDegradationSystem Landmarks { get; }
public WastelandMapSystem WastelandMap { get; }
public DamagedMapSystem? DamagedMap { get; private set; }
public SeasonProfileDef Profile { get; private set; }
public WeatherGateCatalog GateCatalog { get; private set; } = new WeatherGateCatalog();
public AtmosphereTextSystem AtmosphereTexts { get; } = new AtmosphereTextSystem();
public EnvironmentalTextSystem EnvironmentalTexts { get; } = new EnvironmentalTextSystem();
public EvolvingWorldSeedContainer? Seeds { get; private set; }
public string ShelterSectorId => EvolvingWorldSeeder.ShelterSectorId(Seeds);
public string WildlifeSightingFor(string locationId) {
public string HomeSectorWildlifeStatus() {
public string LastEvent { get; private set; } = string.Empty;
public WeatherEffectsCatalog? WeatherEffects { get; private set; }
public static WorldHostSession Create(string dataDir, ICampaignRngManager? campaignRng = null) {
public string FlavorTextForLocation(string locationId, string? weather = null) {
public void TickHours(float hours) {
public string ForceDemo(WeatherKind kind) {
public string StatusLine() {
public WorldWeatherState CaptureSave() => Weather.CaptureState();
public void RestoreSave(WorldWeatherState state) => Weather.RestoreState(state);
public string SetSkyArmorDemo(int gridX, string material, float thickness) {
public string ImpactDemo(int gridX, float energyMJ) {
public string SkyArmorStatusLine() {
public SkyArmorSaveState CaptureSkyArmorSave() => SkyArmor.CaptureState();
public void RestoreSkyArmorSave(SkyArmorSaveState state) => SkyArmor.RestoreState(state);
public WeatherIntelligenceSaveState CaptureWeatherIntelligenceSave() => WeatherIntelligence.CaptureState();
public string InstallWeatherStationDemo(int day) {
public string CalibrateWeatherStationDemo(int day) {
public string ActivateOrbitalTelemetryDemo(int day) {
public string ScheduleOrbitalImpactDemo(int day, int gridX, float energyMj) {
public string WeatherIntelligenceStatusLine() {
internal static bool IsHazardWeather(WeatherKind kind) {
internal bool IsSevereWeather(WeatherKind kind) {
```


# Appendix B.14 — Current Code Architecture: `src/Main.FlagshipInstitutions.cs`

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


# Appendix B.15 — Current Code Architecture: `src/Main.Plans46_49.cs`

### `src/Main.Plans46_49.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 474 lines / 19620 bytes.
- SHA-256: `312d6602edf690061111377cd4798a738c47e6c5c344ed258c88e91167262bb8`.
- Architecture signals: seeded references=4; save/restore symbols=10; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public ShelterWorkshopSystem EnsureShelterWorkshop() {
public ShelterRadioStationSystem EnsureRadioStation() {
public ShelterSocialDynamicsSystem EnsureShelterSocialDynamics() {
public ExcavationHazardSystem EnsureExcavationHazards() {
public DynamicQuestlineSystem EnsureDynamicQuests() {
```


# Appendix B.16 — Current Code Architecture: `src/UI/RadioIntelligencePanel.cs`

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


# Appendix C.17 — Catalog Census: `Assets/StreamingAssets/Data/sky_layer_armor_catalog.json`

### `Assets/StreamingAssets/Data/sky_layer_armor_catalog.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 5475 bytes / 5475 characters.
- SHA-256: `b4085538fc1f4fc58718464d5d5f981becccc16acb71bb74da0c5e80e020d33a`.
- Root keys: `configurations`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
configurations: min=6, max=6, observed_paths=1
configurations[].composition: min=2, max=2, observed_paths=2
configurations[].repair_cost: min=1, max=2, observed_paths=2
```

Representative record fields:

- `attenuation_factor`
- `blast_resistance_mj`
- `composition`
- `default_thickness_meters`
- `degradation_rate`
- `description`
- `id`
- `material_tier`
- `name`
- `repair_cost`
- `tier`

Representative identifiers (ordered, capped for readability):

```text
sky_armor_sandbag_layer
sky_armor_scrap_overlay
sky_armor_reinforced_concrete
sky_armor_steel_hull_plating
sky_armor_composite_military
sky_armor_emergency_blast_canopy
```


# Appendix C.18 — Catalog Census: `Assets/StreamingAssets/Data/orbital_harrow_events.json`

### `Assets/StreamingAssets/Data/orbital_harrow_events.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 9102 bytes / 9088 characters.
- SHA-256: `3b9d4b30dc64569dde7bbc737c7a33a0b33dc013b9ce1c09b965f936919d2a88`.
- Root keys: `events`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
events: min=12, max=12, observed_paths=1
```

Representative record fields:

- `affected_cell_spread`
- `description`
- `id`
- `impact_energy_mj`
- `is_false_positive`
- `lead_time_days`
- `name`
- `penetration_power_mj`
- `radio_hook_text`
- `revealed_site_id`
- `salvage_yield_item_id`
- `salvage_yield_quantity`
- `severity`
- `signal_type`

Representative identifiers (ordered, capped for readability):

```text
event_orbital_kinetic_early_track
event_orbital_kinetic_thermal_descent
event_orbital_kinetic_seismic_precursor
event_orbital_kinetic_fragmented_track
event_orbital_cluster_multiple_returns
event_orbital_cluster_split_track
event_orbital_emp_radio_blackout
event_orbital_emp_signature_mismatch
event_orbital_dead_hand_repeating_ping
event_orbital_dead_hand_broken_checksum
event_orbital_radar_ducting_false_alarm
event_orbital_debris_misclassification
```


# Appendix C.19 — Catalog Census: `Assets/StreamingAssets/Data/items.json`

### `Assets/StreamingAssets/Data/items.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 390056 bytes / 390056 characters.
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=724, max=724, observed_paths=1
```

Representative record fields:

- `category`
- `contamination`
- `degradeRate`
- `description`
- `disassembleYieldFraction`
- `displayName`
- `display_name`
- `durability`
- `empShielded`
- `equipSlot`
- `healthEffect`
- `hungerRestore`
- `id`
- `isEquipable`
- `moraleEffect`
- `radCleanse`
- `radProtection`
- `repairCosts`
- `repairRecipe`
- `scrapValue`
- `stackMax`
- `tags`
- `thirstRestore`
- `tradeValue`
- `type`
- `value`
- `weight`
- `weight_kg`

Representative identifiers (ordered, capped for readability):

```text
item_decon_chelator_concentrate
item_lead_lined_effluent_filter
item_heavy_neoprene_scrub_brush
item_sealed_waste_bin
item_theodolite_brass_precision
item_surveyor_stadia_rod
item_datum_plate_bronze
item_concrete_mix
item_forged_rotor_shaft
item_magnetic_bearing_coil
item_high_vacuum_pump
item_containment_ring_steel
item_reinforced_concrete_vault
item_seismic_damper_pad
item_vacuum_pump_oil
item_bearing_grease
item_rotor_balancing_kit
item_portable_pid_detector
item_detector_sensor_module
item_hermetic_sample_ampoule
item_hot_dust_drum
item_sludge_cake
item_tailings_drum
dosimeter
geiger_counter
iodine_pills
anti_rad
gas_mask
hazmat_suit
water_filter
air_filter
clean_water
irradiated_water
canned_food
fuel
cloth
scrap_metal
bandage
raw_meat
cooked_meat
dirty_water
morphine
chelation_agent
potassium_iodide
medical_kit
battery
calibration_kit
tweezers
splint
antibiotics
jewelry
diamond
currency
mechanical_parts
electronic_scrap
item_radiosonde
solar_cell
chemicals
handheld_radio
engine
roots
berries
vacuum_tube
spring_mechanism
phonograph_needle
projector_bulb
lubricant_oil
film_reel
antenna_coil
soldering_kit
music_box_comb
spring_key
typewriter_ribbon
machine_oil
camera_lens_cleaner
photographic_film
item_acoustic_decoy
item_ammonium_nitrate_sack
item_amnestic_syrup
item_anchor_notes
item_ash_ghillie
item_bio_plastic
item_black_water_vial
item_co2_scrubber_cartridge
item_epoxy_injector
item_faraday_mesh
item_frostbite_salve
item_fungicide_fogger
item_galvanized_rebar
item_glycol_antifreeze_canister
item_hermetic_hatch_silicone_gasket
item_high_tensile_steel_culvert_brace
item_insulated_snowmobile_battery
item_lead_shielded_sample_cask
item_lead_visor
item_lithium_salts
item_mine_prod
item_mycelium_bricks
item_prussian_blue_chelating_pellets
item_radon_detector_electret
item_rebreather_scrubber
item_ro_membrane
item_scopolamine_root
item_sealed_lead_pig
item_snow_goggles_improvised
item_sound_baffling
item_suitcase_locked
item_surgical_bone_chisel
item_teddy_bear
item_thermal_paste
item_welders_glass
aa_batteries
alcohol_wipes_box_10_of_10
ammo_762x54r_jhp_ap
ammo_357
ammo_12g
ammo_308
ammo_556
ammo_762
antiseptic_1l_of_1l
battery_pack
box_of_nails_10
canned_soup
childrens_books
cigarette_lighter
clean_water_jug
cooking_oil
copper_wire_10m_of_10m
diesel_fuel
dried_rations
faraday_pack
field_surgical_kit
fuel_1l
fuel_cell
growing_manual
iodine_tablets
item_cassette_tape
item_pre_war_photo_album
item_vinyl_collection
mechanical_components
medkit
metal_pipe
military_grade_hatchet
military_mre
military_radio
military_rations
military_supply_crate
music_box_fur_elise
night_vision_scope
plastic_material
scrap_plastic
synthetic_fuel_canister
carbon_black_powder
protective_childs_coat
rubber_hose
scrap_wood
sealed_government_document
seed_packets
spirits
steel_rebar
```


# Appendix D.20 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Shelter/SkyLayerArmorCatalogTests.cs`

### `Ashfall.Core.Tests/Shelter/SkyLayerArmorCatalogTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 309; SHA-256: `2522af036681c87b5311c24414087f23485aec6d26ce6c9bfc749a321a70aff4`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ArmorCatalog_LoadsAllSixAuthoredConfigurations
ArmorCatalog_AllMaterialItemReferencesResolveInItemsCatalog
ArmorCatalog_DefaultConfigurationsFallbackMatchesSixConfigs
OrbitalThreatCatalog_LoadsTwelveUniqueEvents
SkyLayerArmor_InstallationAndAttenuationHierarchy
SkyLayerArmor_EvaluateImpact_MitigationAndBreach
SkyLayerArmor_RepairCell_RestoresDurability
SkyLayerArmor_SaveAndRestore_PreservesAllCells
FullDefenseLoop_TelemetryWarning_Brace_Strike_Mitigation_Salvage
```


# Appendix D.21 — Existing Focused Test Inventory: `Ashfall.Core.Tests/OrbitalHarrowTelemetrySystemTests.cs`

### `Ashfall.Core.Tests/OrbitalHarrowTelemetrySystemTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 167; SHA-256: `0a1ab6c8356a96a99ebb8df15b0efef3c009b8967d902f19aaf93481f0390ca3`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ActivateTelemetry_EnablesSystem
ScheduleImpact_CreatesWarning
Brace_MitigatesImpact
TickDay_OnImpactDay_Resolves
Brace_WhenNoImpact_Blocks
CaptureRestoreState_PreservesImpact
TelemetryCatalog_ContainsTwelveCanonicalEvents
FalsePositiveEvents_ResolveWithoutDamageOrBreach
DeadHandEvents_CarryRadioHooksAndRevealSites
```


# Appendix D.22 — Existing Focused Test Inventory: `Ashfall.Core.Tests/World/WorldHostSessionTests.cs`

### `Ashfall.Core.Tests/World/WorldHostSessionTests.cs`

- Current test declarations: Fact=16, Theory=0, InlineData=0.
- File lines: 482; SHA-256: `b329ac86bcc2993a2056bbf9438f455cf20a61a69c8e21ff3aa92c366949377f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RestoreSkyArmorSave_NullState_ClearsExistingCellsGracefully
RestoreSkyArmorSave_EmptyState_ClearsExistingCellsGracefully
RestoreSkyArmorSave_NullCellsProperty_ClearsExistingCells
RestoreSkyArmorSave_PopulatedState_RestoresAllMaterialTiers
RestoreSkyArmorSave_OverwritesExistingCells_RemovesUnrepresentedCells
RestoreSkyArmorSave_PreservesAttenuationCalculations
RestoreSkyArmorSave_UnprotectedGridCellsReturnDefaultBleed
RestoreSkyArmorSave_RestoresKineticImpactAbsorptionBehavior
RestoreSkyArmorSave_RestoresKineticImpactBreachBehavior
RestoreSkyArmorSave_ImpactDemo_ReportsAbsorptionAndBreachAccurately
RestoreSkyArmorSave_AffectsSkyArmorStatusLine_PlatedVsEmpty
RestoreSkyArmorSave_AffectsSkyArmorStatusLine_AvgAttenuationCalculation
RestoreSkyArmorSave_RoundTripFidelity_CaptureRestoreCaptureAreIdentical
RestoreSkyArmorSave_IsIdempotent_SuccessiveRestoresProduceIdenticalState
RestoreSkyArmorSave_PostRestoreDurabilityDegradationAndRepair
RestoreSkyArmorSave_DoesNotMutateOriginalSaveStateObject
```


# Appendix D.23 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs`

### `Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 410; SHA-256: `58130dae9d890fd61d10c85f141e3fa59852d2b910e0cba09663d9b5a5ea7155`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Test1_ReferentialIntegrity_CrossCatalogContracts_AllResolve
Test2_FullDeterministicCampaignJourney_TouchesAllTenSystems_ReplaysIdentically
Test3_Batch1CatalogCountsAndLoaderClassifications_MatchAuthoritativeTruth
```


# Appendix D.24 — Existing Focused Test Inventory: `Ashfall.Core.Tests/SkyDefenseBatteryTests.cs`

### `Ashfall.Core.Tests/SkyDefenseBatteryTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 373; SHA-256: `a5e4fd87631027dbfcb27593b5bf8130004b2baef18f04e7626234089f9a8aa4`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
OrbitalWarning_CreatesSingleTrack_NoDuplicates
MagazineLoad_TransfersAtomically_AndNeverDoubleCounts
MagazineLoad_UnknownOrdnance_OrInsufficientStock_FailsCleanly
Volley_ConsumesMagazineRound_IncrementsHeatAndServiceCounter
Volley_WithoutAmmo_OrUnknownTrack_FailsWithoutConsumption
InterceptChance_ClampsToAuthoredBounds_AndProximityFuseHelps
Firing_SameSeedSameState_IsDeterministic
Interception_ModifiesStrike_RetainedThroughArmorPipeline
Service_IsAtomic_ResetsCounter_RepairsHydraulics
DailyTick_CoolsHeat_DriftsRadar
CrewAssignment_ClaimsAvailability_ThroughAuthority
SaveLoad_PreservesBatteryState_AndContinuationMatches
OldSave_MissingBatterySection_DefaultsSafely
```


# Appendix D.25 — Existing Focused Test Inventory: `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs`

### `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs`

- Current test declarations: Fact=42, Theory=0, InlineData=0.
- File lines: 523; SHA-256: `2ac555cd2f86236e2a88f5b0f1d44ef0c76c7722c7b2f9773792bdb6512df49b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SkyArmor_SetCell_GetAttenuation
SkyArmor_UnprotectedCell_FullBleed
SkyArmor_BetterMaterial_LessAttenuation
SkyArmor_KineticImpact_Absorbed
SkyArmor_KineticImpact_Breached
SkyArmor_KineticImpact_Unprotected
SkyArmor_DurabilityDecreases_OnImpact
SkyArmor_CaptureRestore_Roundtrip
SkyArmor_RestoreNull_DoesNotCrash
Vigil_Start_ActiveAndFiresEvent
Vigil_Tick_ReciteNames
Vigil_PhantomKnock_At95Percent
Vigil_Completes_AtDuration
Vigil_Skip_CompletesEarly
Vigil_CaptureRestore_Roundtrip
Vigil_RestoreNull_DoesNotCrash
GenSuccession_RegisterDweller
GenSuccession_AdvanceTime_AgesDwellers
GenSuccession_Retirement_At65
GenSuccession_Mentorship_TransfersTrait
GenSuccession_Mentorship_RejectsDeceased
GenSuccession_ChapterAdvance_FiresEvent
GenSuccession_CaptureRestore_Roundtrip
Epilogue_TrueReconciliation_AllConditions
Epilogue_Commonwealth_TreatyPlusBurned
Epilogue_Garrison_TreatyWithoutBurned
Epilogue_TempestSterilization_HighDeaths
Epilogue_NullContext_FracturedWarlords
Epilogue_Demographics_Thriving
Epilogue_Demographics_Extinction
Epilogue_Moral_Forgiven
Epilogue_Narrative_ContainsKeyPhrases
DiveRunner_StartsInDeckhouse
DiveRunner_Advance_MovesForward
DiveRunner_Advance_RejectsAtEnd
DiveRunner_TickOxygen_Decrements
DiveRunner_TickOxygen_LowWarning_At30
DiveRunner_CommitChoice_SetsFlag
DiveRunner_CommitChoice_RejectsOutsideHold
DiveRunner_CommitChoice_OneShot
DiveRunner_DetectionRisk_HigherInCompanionway
DiveRunner_DetectionRisk_FearIncreasesInHold
```


# Appendix D.26 — Existing Focused Test Inventory: `Ashfall.Core.Tests/IslandBridgesTests.cs`

### `Ashfall.Core.Tests/IslandBridgesTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 206; SHA-256: `f375690aad3d7dad3c5a0f6b5e88d488a84bab80db8c540440c0cf786d1b9007`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
MaritimeDive_ConductDive_ResolvesAndPreservesState
WorkshopReverseEngineering_ExamineAndRepair_UnlocksRelic
PharmaLab_SynthesizeMedicine_ProducesOutput
WeatherStation_InstallAndCalibrate_GeneratesForecast
OrbitalHarrow_WarningAndImpact_ResolvesDamage
ExpeditionVehicle_RefuelAndTravel_TracksCondition
```


# Appendix D.27 — Existing Focused Test Inventory: `Ashfall.Core.Tests/WeatherIntelligenceCoordinatorTests.cs`

### `Ashfall.Core.Tests/WeatherIntelligenceCoordinatorTests.cs`

- Current test declarations: Fact=12, Theory=0, InlineData=0.
- File lines: 234; SHA-256: `eb589ce41e671c135aedb2c56d25b26cda76b0827423367065a7d4b83cee5427`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ReadModel_WithoutStation_HasNoForecast
ReadModel_AfterInstallOnly_StillNoForecast
ReadModel_AfterCalibrate_GeneratesConfidenceForecast
ReadModel_CalibratedStation_ProducesRouteSafetyAndBestTravelDay
Orbital_ScheduleImpact_SetsWarningLeadTime
Orbital_TickDay_OnImpactDay_Resolves
SaveRoundTrip_PreservesStationCalibration
SaveRoundTrip_PreservesOrbitalImpact
SaveRoundTrip_PreservesForecastEntries
Determinism_SameSeed_YieldsIdenticalForecast
Determinism_SameSeed_YieldsIdenticalOrbitalSequence
MultiDaySeededRoundTrip_SurvivesReload
```


# Appendix D.28 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Campaign/CrisisPredictionTests.cs`

### `Ashfall.Core.Tests/Campaign/CrisisPredictionTests.cs`

- Current test declarations: Fact=16, Theory=0, InlineData=0.
- File lines: 376; SHA-256: `b186f1c02074ab56535663c8722f3e1ab59f5c2b8f842dbfbb7434671c7adc0f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Evaluate_HealthyNeutralInputs_ReturnsEmpty
Evaluate_FoodDepletionTrajectory_PredictsFoodCrisisWithBoundedConfidence
Evaluate_WaterDepletionTrajectory_PredictsWaterCrisis
Evaluate_PowerDepletion_PredictsBlackout
Evaluate_SevereRadiationDose_PredictsRadiationSpike
Evaluate_SanitationSpills_PredictsDiseaseOutbreak
Evaluate_DistressAmbush_PredictsHostileIntercept
Evaluate_ZeroWorkingAdults_PredictsDemographicCollapse
Evaluate_EmptyStock_HandlesWithoutDivideByZero
Evaluate_EmptyRoster_DoesNotStarveDeadSurvivors
Evaluate_ExactThresholdBoundary_FiresAppropriately
Evaluate_MultipleCrises_SortedDeterministicallyByProjectedDayAndConfidence
Evaluate_PureFunction_RepeatedCallsIdentical
MonotonicApproachTest_AcrossDayTransitions_HorizonDecreasesConsistently
WeatherIntelligenceCoordinator_SevereForecast_PopulatesCrisisFields
WeatherIntelligenceCoordinator_NoSevereWeather_HasNoPredictedCrisis
```


# Appendix D.29 — Existing Focused Test Inventory: `Ashfall.Core.Tests/World/ExcavationHazardSystemTests.cs`

### `Ashfall.Core.Tests/World/ExcavationHazardSystemTests.cs`

- Current test declarations: Fact=14, Theory=0, InlineData=0.
- File lines: 365; SHA-256: `2fe1430cc1dd0d50f66a935cc2473b3bd3622c78a8adf037ac3e4534d12a340e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
VentilationBlower_InstallsAndReducesMethane
SporeScrub_RequiresGasMaskInInventory
ShoringReinforcement_RestoresShoringHealth
BulkheadToggle_BlocksSealingWhenMinersTrapped
TrappedMinersRescue_CompletesSuccessfullyWithLabor
TrappedMinersRescue_FailsWhenDeadlineExceeded
BlastMatting_ReducesCollapseRisk
SaveRestore_PreservesSectorHazardsAndInstalledMitigations
DeterministicReplay_YieldsIdenticalHazardEvolution
AddMethane_CrossingIgnitionThreshold_RaisesExactlyOncePerCrossing
TickDay_MethaneAccumulation_RaisesIgnitionOnCrossing
AddFloodWater_CrossingCriticalThreshold_RaisesExactlyOncePerCrossing
InstalledMitigation_PassiveDecay_UsesAuthoredBonus
InstalledDrainagePump_PassiveDecay_LowersFloodLevel
```


# Appendix D.30 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Radio/RadioStationCatalogTests.cs`

### `Ashfall.Core.Tests/Radio/RadioStationCatalogTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 139; SHA-256: `8a3df262d6f4fe50519e209619dc9d46609e1ac21354aecbb487d04eef0d2e20`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RadioStationCatalog_JsonMatchesHardcodedDefaults_ExactParity
RadioStationCatalog_LoadFromDataDirectory_Succeeds
RadioStationCatalog_FindStationAtFrequency_ResolvesNearFrequencies
RadioStationCatalog_StateOverrides_PersistAndRestore
RadioStationCatalog_NoHardcodedStationDefaultsInCore_AuthorityGate
```


# Appendix D.31 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Radio/RadioStationParityTests.cs`

### `Ashfall.Core.Tests/Radio/RadioStationParityTests.cs`

- Current test declarations: Fact=20, Theory=0, InlineData=0.
- File lines: 396; SHA-256: `3329957b141f36045545374367496fd8d2cf91d0384b458225c57e38cac2e61b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
B1_001_HardcodedVsJson_Parity_ZeroMismatch
B1_002_DuplicateStation_Rejected
B1_003_InvalidFrequency_Rejected
B1_004_MissingCatalog_Throws_NoSilentFallback
B1_005_AllProductionConstructors_LoadJson
B1_006_CurrentSchedule_Deterministic
B1_007_NextSlot_Stable
B1_008_ResearchDoesNotGateSchedules
B1_009_EquipmentGatesTuningCapability
B1_010_SignalReasons_ComposeDeterministically
B1_011_VinylBrownout_GivesNoMorale
B1_012_RetryDoesNotDoubleRecord
B1_013_OldRadioSave_Loads
B1_014_UnknownStationOverride_Retained
B1_015_FactionContent_ComesThroughDataPath
B1_016_RestoreEmitsNoTransitionEvent
B1_017_StationSlots_Cover24Hours
B1_018_SignalStrength_ReportsCorrectQualityBands
B1_019_CoreSourceGate_ZeroHardcodedStationDefs
B1_020_RadioCatalogSelftest_ChecksPass
```


# Appendix D.32 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Radio/ShelterRadioStationTests.cs`

### `Ashfall.Core.Tests/Radio/ShelterRadioStationTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 228; SHA-256: `ec4160fe6ca805f9c0495e72c6ab1f3def36beeb7aaacf90136921b57559dbbe`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ScanFrequency_LocksWhenTunedCloseToBroadcast
ScanFrequency_MissesWhenDetuned
Decryption_AdvancesWithOperatorSkill
Triangulation_RequiresDistinctAzimuthsAndUnlocksLocation
SOSDistress_ExpiresWhenDeadlineReached
OrbitalEarlyWarning_RelaysActiveImpactWarning
SaveRestore_PreservesRadioStateAndBearings
DeterministicReplay_ProducesIdenticalRadioScans
```


# Appendix E.33 — Supporting Code Evidence: `src/Main.SkyDefense.cs`

### `src/Main.SkyDefense.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 85 lines / 3332 bytes.
- SHA-256: `6411e5d1d74e56aa3f25f6b95f146fc396716eef5a928e8a9a18ad55b5c70703`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
```


# Appendix E.34 — Supporting Code Evidence: `src/Host/WorldSaveStore.cs`

### `src/Host/WorldSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 142 lines / 6212 bytes.
- SHA-256: `fe797127338983fcbc90f14e220fd99b0fbe5c88daa93c8a6b79fec19c5fb21c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class WorldSaveStore
public const string FileName = "world_save.json";
public const string SectionName = "world";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(WorldHostSave envelope) => s_store.CaptureBare(envelope);
public static WorldHostSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(WorldHostSave envelope) => s_store.CaptureBare(envelope);
public static WorldHostSave? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave( WorldWeatherState state, SkyArmorSaveState skyArmor = null!, WeatherIntelligenceSaveState weatherIntelligence = null!, LocationEvolutionSaveState locationEvolution = null!, WildlifeSaveState wildlife = null!,
public static string TryCapturePersisted( WorldWeatherState state, SkyArmorSaveState skyArmor = null!, WeatherIntelligenceSaveState weatherIntelligence = null!, LocationEvolutionSaveState locationEvolution = null!, WildlifeSaveState wildlife = null!,
public static WorldHostSave? TryLoadEnvelope() => s_store.TryLoad();
public static WorldWeatherState? TryLoad() {
public class WorldHostSave
public WorldWeatherState State;
public SkyArmorSaveState SkyArmor;
public WeatherIntelligenceSaveState WeatherIntelligence;
public LocationEvolutionSaveState LocationEvolution;
public WildlifeSaveState Wildlife;
public LandmarkSaveState Landmark;
public string Checksum = string.Empty;
```


# Appendix E.35 — Supporting Code Evidence: `src/Host/HostCli.DynamicWorld.cs`

### `src/Host/HostCli.DynamicWorld.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 187 lines / 11392 bytes.
- SHA-256: `264e923774ae6e1c35b80f1445e01a04c4419f71fecb044287e8c0f8fba8af05`.
- Architecture signals: seeded references=5; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunDynamicWorldSelfTest(string dataDirectory) {
```


# Appendix E.36 — Supporting Code Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix E.37 — Supporting Code Evidence: `src/Host/HostCli.PanelTests.cs`

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


# Appendix G.38 — Supporting Regression Evidence: `Ashfall.Core.Tests/World/Plan19DynamicWorldTests.cs`

### `Ashfall.Core.Tests/World/Plan19DynamicWorldTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 282; SHA-256: `bb49504442f792a1d3a4a55c962a168047d4ebaa12253c41c01911e15c70aab4`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
WeatherForecast_PeekDoesNotMutateWeatherState
WeatherForecast_SameSeedProducesDeterministicForecast
WeatherStation_TierProgression_AffectsHorizonAndConfidence
WeatherStation_PreparationPayoffs_ProvideActionableAdvice
OrbitalCatalog_LoadsAllFiveTemplates
OrbitalImpact_EvaluatesSkyArmor_GeneratesSalvageAndRevealsSite
SeasonModel_DefinesTenPhasesAcrossYear
SeasonalEvents_LoadCatalog_AndTriggerDeterministically
WeatherIntelligenceCoordinator_SaveRestoreRoundTrip_PreservesAllStates
```


# Appendix G.39 — Supporting Regression Evidence: `Ashfall.Core.Tests/Integration/Plans46_49_CrossSystemIntegrationTests.cs`

### `Ashfall.Core.Tests/Integration/Plans46_49_CrossSystemIntegrationTests.cs`

- Current test declarations: Fact=12, Theory=0, InlineData=0.
- File lines: 609; SHA-256: `7f5b55f6615d624e4670e8bb6ab7ad04b84b5d91acc3956849e5304512c2085a`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
WorkshopAndEquipmentBridge_WeaponRefurbishment_RestoresCombatReadiness
HeavyWorkshopAndVehicleGarage_PowertrainRebuild_EnablesExpeditionReadiness
RadioIntelligenceAndMap_TriangulatesHiddenDepotLocation
CrowdedSleepingQuartersAndMediation_TracksAffinityDriftAndAccord
SubterraneanHazardsAndRescue_EmergencyClearance_SavesTrappedMiners
FullCampaignSaves_RoundTripCaptureAndRestoreAllFourSubsystems
CrossSystemDeterminism_PairedRunsYieldIdenticalStateSnapshots
ScenarioA_ArmoryScarcityLoop_ExecutesReloadServiceAndSaveRoundtrip
ScenarioB_RadioToExpeditionDiscovery_TriangulatesSOSAndSchedulesExpiry
ScenarioC_SocialPressureFromShelterCapacity_BunkFrictionToPrivateRelief
ScenarioD_DeepStrataEmergency_MitigationAndCaveInRescueLifecycle
ScenarioE_CrossSystemShelterCrisis_DeterministicSimulationAcrossAllFourDomains
```


# Appendix G.40 — Supporting Regression Evidence: `Ashfall.Core.Tests/Integration/FullCampaign30DayShelterPlaythroughTests.cs`

### `Ashfall.Core.Tests/Integration/FullCampaign30DayShelterPlaythroughTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 489; SHA-256: `47c57a6a221576f622d2c868fdae421dad2352d703c9d4380a0b72c356fca9a8`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
PreflightAuthoredIds_ExistInAuthoritativeCatalogs
Full30DayCampaign_WithMidRunSaveReloadShock_CompletesWithoutInvariantsDrift
FatalCaveInRescue_ReportsCasualtiesToSurvivorFate_AndMemorial
DualRun_SameSeedDeterminism_ProducesIdenticalCampaignOutcome
```


# Appendix G.41 — Supporting Regression Evidence: `Ashfall.Core.Tests/Tooling/LoaderWiringGateTests.cs`

### `Ashfall.Core.Tests/Tooling/LoaderWiringGateTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 189; SHA-256: `cf130fbfbbf7ff00c3627c76b4cc6050938a9cfd48e63a1ea3feb0e371b56a66`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
EveryLoadAndRegisterLoader_IsCalledFromProduction_OrAllowlisted
FormerlyAllowlistedLoadFeeders_AreProductionWired
RecentSystemCatalogs_AreBoundFromMainHostPartials
AllowlistEntries_StillExist_AsLoaders
```


# Appendix H.42 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

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
| armor configuration definitions | SkyLayerArmorCatalogLoader | armor cells, attenuation, breach and repair | SkyLayerArmorSystem | Owner emits/reads a typed fact; no mirror state. |
| armor configuration definitions | SkyLayerArmorCatalogLoader | authored orbital event definitions | OrbitalHarrowCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| armor configuration definitions | SkyLayerArmorCatalogLoader | telemetry, warnings, impacts and salvage | OrbitalHarrowTelemetrySystem | Owner emits/reads a typed fact; no mirror state. |
| armor configuration definitions | SkyLayerArmorCatalogLoader | shared instance composition and save | WorldHostSession | Owner emits/reads a typed fact; no mirror state. |
| armor configuration definitions | SkyLayerArmorCatalogLoader | warning/interception/panel facts | Sky defense/radio consumers | Owner emits/reads a typed fact; no mirror state. |
| armor cells, attenuation, breach and repair | SkyLayerArmorSystem | armor configuration definitions | SkyLayerArmorCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| armor cells, attenuation, breach and repair | SkyLayerArmorSystem | authored orbital event definitions | OrbitalHarrowCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| armor cells, attenuation, breach and repair | SkyLayerArmorSystem | telemetry, warnings, impacts and salvage | OrbitalHarrowTelemetrySystem | Owner emits/reads a typed fact; no mirror state. |
| armor cells, attenuation, breach and repair | SkyLayerArmorSystem | shared instance composition and save | WorldHostSession | Owner emits/reads a typed fact; no mirror state. |
| armor cells, attenuation, breach and repair | SkyLayerArmorSystem | warning/interception/panel facts | Sky defense/radio consumers | Owner emits/reads a typed fact; no mirror state. |
| authored orbital event definitions | OrbitalHarrowCatalogLoader | armor configuration definitions | SkyLayerArmorCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| authored orbital event definitions | OrbitalHarrowCatalogLoader | armor cells, attenuation, breach and repair | SkyLayerArmorSystem | Owner emits/reads a typed fact; no mirror state. |
| authored orbital event definitions | OrbitalHarrowCatalogLoader | telemetry, warnings, impacts and salvage | OrbitalHarrowTelemetrySystem | Owner emits/reads a typed fact; no mirror state. |
| authored orbital event definitions | OrbitalHarrowCatalogLoader | shared instance composition and save | WorldHostSession | Owner emits/reads a typed fact; no mirror state. |
| authored orbital event definitions | OrbitalHarrowCatalogLoader | warning/interception/panel facts | Sky defense/radio consumers | Owner emits/reads a typed fact; no mirror state. |
| telemetry, warnings, impacts and salvage | OrbitalHarrowTelemetrySystem | armor configuration definitions | SkyLayerArmorCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| telemetry, warnings, impacts and salvage | OrbitalHarrowTelemetrySystem | armor cells, attenuation, breach and repair | SkyLayerArmorSystem | Owner emits/reads a typed fact; no mirror state. |
| telemetry, warnings, impacts and salvage | OrbitalHarrowTelemetrySystem | authored orbital event definitions | OrbitalHarrowCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| telemetry, warnings, impacts and salvage | OrbitalHarrowTelemetrySystem | shared instance composition and save | WorldHostSession | Owner emits/reads a typed fact; no mirror state. |
| telemetry, warnings, impacts and salvage | OrbitalHarrowTelemetrySystem | warning/interception/panel facts | Sky defense/radio consumers | Owner emits/reads a typed fact; no mirror state. |
| shared instance composition and save | WorldHostSession | armor configuration definitions | SkyLayerArmorCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| shared instance composition and save | WorldHostSession | armor cells, attenuation, breach and repair | SkyLayerArmorSystem | Owner emits/reads a typed fact; no mirror state. |
| shared instance composition and save | WorldHostSession | authored orbital event definitions | OrbitalHarrowCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| shared instance composition and save | WorldHostSession | telemetry, warnings, impacts and salvage | OrbitalHarrowTelemetrySystem | Owner emits/reads a typed fact; no mirror state. |
| shared instance composition and save | WorldHostSession | warning/interception/panel facts | Sky defense/radio consumers | Owner emits/reads a typed fact; no mirror state. |
| warning/interception/panel facts | Sky defense/radio consumers | armor configuration definitions | SkyLayerArmorCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| warning/interception/panel facts | Sky defense/radio consumers | armor cells, attenuation, breach and repair | SkyLayerArmorSystem | Owner emits/reads a typed fact; no mirror state. |
| warning/interception/panel facts | Sky defense/radio consumers | authored orbital event definitions | OrbitalHarrowCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| warning/interception/panel facts | Sky defense/radio consumers | telemetry, warnings, impacts and salvage | OrbitalHarrowTelemetrySystem | Owner emits/reads a typed fact; no mirror state. |
| warning/interception/panel facts | Sky defense/radio consumers | shared instance composition and save | WorldHostSession | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 6/10 target with a 6-armor/12-event current census. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Document the shared world-owned telemetry/armor instance and all current consumers. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Verify event activation, warning, brace, impact, repair, salvage and save behavior in the live host route. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Reject a separate threat generator, armor save or UI-local impact calculation. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.574 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Shelter/SeismicDynamicsSystem.cs`

### `Assets/Ashfall.Core/Shelter/SeismicDynamicsSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 456 lines / 19463 bytes.
- SHA-256: `141b8b9112ff1df9709348631d3dadef28134503464810a07397d4156fcc0057`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=11; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SeismicFaultDef
public string fault_id { get; set; } = string.Empty;
public string name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public string depth_layer { get; set; } = "sub_surface";
public float slip_threshold { get; set; } = 100f;
public float base_accumulation_rate { get; set; } = 2.0f;
public float depth_multiplier { get; set; } = 1.0f;
public float strata_attenuation_factor { get; set; } = 0.3f;
public float pipe_shear_probability { get; set; } = 0.5f;
public float radiator_rupture_probability { get; set; } = 0.3f;
public int methane_outgassing_ppm { get; set; } = 500;
public List<string> affected_sectors { get; set; } = new List<string>();
public sealed class SeismicFaultCatalog
public int schema_version { get; set; } = 1;
public List<SeismicFaultDef> faults { get; set; } = new List<SeismicFaultDef>();
public sealed class FaultRuntimeState
public string faultId = string.Empty;
public float currentTension;
public int totalSlips;
public int lastSlipDay = -1;
public int temporaryShoringDays;
public float temporaryShoringStrength;
public sealed class SeismicQuakeEvent
public int day;
public string faultId = string.Empty;
public float magnitude;
public string depthLayer = string.Empty;
public List<string> shearedPipes = new List<string>();
public List<string> rupturedRadiators = new List<string>();
public int outgassedMethanePpm;
public string description = string.Empty;
public sealed class SeismicDynamicsSaveState
public int currentDay;
public Dictionary<string, FaultRuntimeState> faults = new Dictionary<string, FaultRuntimeState>(StringComparer.Ordinal);
public Dictionary<string, int> sectorReinforcementLevel = new Dictionary<string, int>(StringComparer.Ordinal);
public bool seismographOperational = true;
public List<SeismicQuakeEvent> recentQuakes = new List<SeismicQuakeEvent>();
public List<string> activeEarlyWarnings = new List<string>();
public List<string> geophoneSectors = new List<string>();
public Dictionary<string, float> dampenerIntegrity = new Dictionary<string, float>(StringComparer.Ordinal);
public sealed partial class SeismicDynamicsSystem
public const string SystemId = "seismic_dynamics";
public const string CatalogPath = "seismic_fault_catalog.json";
public IReadOnlyDictionary<string, SeismicFaultDef> Catalog => _catalog;
public SeismicDynamicsSaveState State => _state;
public event Action<SeismicQuakeEvent>? OnQuakeOccurred;
public event Action<string, float>? OnEarlyWarning; // faultId, tensionPercent
public event Action? OnSeismicStateChanged;
public void LoadCatalog(string jsonContent) {
public void RegisterFault(SeismicFaultDef def) {
public void SetSeismographStatus(bool operational) {
public ActionResult ApplyEmergencyShoring(string faultId, int durationDays = 5, InventoryContainer? inv = null) {
public ActionResult ReinforceSector(string sectorId, InventoryContainer? inv = null) {
public void InjectKineticShock(float megajoules, string epicenterSector) {
public void TickDay(int day) {
public SeismicDynamicsSaveState CaptureState() => CloneState(_state);
public void RestoreState(SeismicDynamicsSaveState saved) {
```


# Appendix Q.575 — Additional Current Architecture Evidence: `src/Audio/AudioSelfTest.cs`

### `src/Audio/AudioSelfTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1464 lines / 86135 bytes.
- SHA-256: `6f4f36f995ff641502b021ecc7c4d5fe92a6e5695cb6a6d80dc67cacb94b9537`.
- Architecture signals: seeded references=19; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class AudioSelfTest
public static int Run() {
internal sealed class TestShelterOperationsAudioProvider : IShelterOperationsAudioProvider
public ShelterWorkshopSystem? AudioWorkshop { get; set; }
public Ashfall.Core.Radio.ShelterRadioStationSystem? AudioRadioStation { get; set; }
public ShelterSocialDynamicsSystem? AudioSocialDynamics { get; set; }
public ExcavationHazardSystem? AudioExcavationHazards { get; set; }
internal sealed class TestExpansionAudioProvider : IExpansionAudioProvider
public Ashfall.Core.Survivors.DesperationSystem? AudioDesperation { get; set; }
public Ashfall.Core.Medical.MutationSystem? AudioMutation { get; set; }
public Ashfall.Core.Combat.ChemWarfareSystem? AudioChemWarfare { get; set; }
public Ashfall.Core.Expeditions.RailwaySystem? AudioRailway { get; set; }
```


# Appendix Q.576 — Additional Current Architecture Evidence: `src/Audio/ShelterOperationsAudioBridge.cs`

### `src/Audio/ShelterOperationsAudioBridge.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 368 lines / 13504 bytes.
- SHA-256: `5f03f03b2eff927efb8b4886f4dec3d0edd0c28853e27c8a8fc41d9100941569`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public interface IShelterOperationsAudioProvider
public sealed class ShelterOperationsAudioBridge : IDisposable
public void BindAll( ShelterWorkshopSystem? workshop = null, ShelterRadioStationSystem? radio = null, ShelterSocialDynamicsSystem? social = null, ExcavationHazardSystem? excavation = null) {
public void SubscribeAll(IShelterOperationsAudioProvider? provider) {
public void BindWorkshop(ShelterWorkshopSystem? workshop) {
public void BindRadio(ShelterRadioStationSystem? radio) {
public void BindSocial(ShelterSocialDynamicsSystem? social) {
public void BindExcavation(ExcavationHazardSystem? excavation) {
public void NotifyRadioFrequencyChanged(float frequencyKhz, float minKhz = 3000f, float maxKhz = 30000f) {
public void UpdateMessHallOccupancy(int activeOccupants) {
public void NotifyMethaneWarning(string sectorId, int ppm) {
public void NotifyBulkheadToggled(string sectorId, bool sealedBulkhead) {
public void NotifyPumpStateChanged(string sectorId, bool running) {
public void Dispose() {
```


# Appendix Q.577 — Additional Current Architecture Evidence: `src/Host/HostCli.SkyDefense.cs`

### `src/Host/HostCli.SkyDefense.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 130 lines / 7661 bytes.
- SHA-256: `8d76e2eb209afad5b1a8ba5c45de37f3db51f45eee11b933641c6020793f623f`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunSkyDefenseSelfTest(string dataDirectory) {
```


# Appendix Q.578 — Additional Current Architecture Evidence: `src/Main.UiPanels.cs`

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


# Appendix Q.579 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/ReconTelemetrySystem.cs`

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


# Appendix Q.580 — Additional Current Architecture Evidence: `src/Main.World.cs`

### `src/Main.World.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 950 lines / 42412 bytes.
- SHA-256: `4f75bdf8f2f5c5022d744a331f9da271f16d7ab47d1bc3b9605b8a3a8fd95261`.
- Architecture signals: seeded references=0; save/restore symbols=8; typed event declarations=0; textual Godot mentions=9; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.581 — Additional Current Architecture Evidence: `src/Main.CampaignOwners.cs`

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


# Appendix Q.582 — Additional Current Architecture Evidence: `src/UI/SkyDefenseBatteryPanel.cs`

### `src/UI/SkyDefenseBatteryPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 636 lines / 28690 bytes.
- SHA-256: `0ca78200620cba7c2f74b986e7bd9fe7b9ef2386a2ccb83aef74ccd04cb09578`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class SkyDefenseBatteryPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _system != null;
public string LastFeedback { get; private set; } = string.Empty;
public void Bind( SkyDefenseBatterySystem system, Func<IReadOnlyList<string>> livingCrew, Func<string, string> crewName, Func<string, int> itemOnHand) {
public void Unbind() {
public override void _Ready() {
public override void _ExitTree() {
public void RefreshView() {
```


# Appendix Q.583 — Additional Current Architecture Evidence: `src/Main.PlayerSurfaces.cs`

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


# Appendix Q.584 — Additional Current Architecture Evidence: `src/UI/WeatherForecastPanel.cs`

### `src/UI/WeatherForecastPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 364 lines / 15621 bytes.
- SHA-256: `827668f3d79f92b0198d952b4a1223d55791d50448aa3184c7b4614f652f2dc1`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=3; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class WeatherForecastPanel : Control
public event Action? OnClose;
public void Bind(WeatherSystem weather, Ashfall.Core.World.WeatherIntelligenceCoordinator? intelligence = null) {
public override void _ExitTree() {
public void RefreshView() {
public override void _Ready() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
```


# Appendix Q.585 — Additional Current Architecture Evidence: `src/Host/PanelBindLifecycleSelfTest.cs`

### `src/Host/PanelBindLifecycleSelfTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1371 lines / 70877 bytes.
- SHA-256: `a96e666a51d3760bb8e972a3acf6ab4fed7a74328402409cdbe372d614a9fe1c`.
- Architecture signals: seeded references=3; save/restore symbols=0; typed event declarations=0; textual Godot mentions=5; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=1; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class PanelBindLifecycleSelfTest
public static int Run(string dataDirectory = "") {
```


# Appendix Q.586 — Additional Current Architecture Evidence: `src/UI/MapAtlasPanel.cs`

### `src/UI/MapAtlasPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 447 lines / 17711 bytes.
- SHA-256: `59bde9b7fa99cff27b095b688dafbe9f11c7590dafcc949956bfc784437a5d1b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=4; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class MapAtlasPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<string>? OnLocationSelected;
public string Id = string.Empty;
public string Display = string.Empty;
public MapFogState FogState;
public MapNodeStatusKind Status;
public MapNodeDanger Danger;
public float PositionX;
public float PositionY;
public string Intel = string.Empty;
public bool Routable;
public bool IsBound => _expeditionHost != null && _worldHost != null;
public void Bind(ExpeditionHostSession expeditionHost, WorldHostSession? worldHost = null) {
public override void _Ready() {
public void RefreshView() {
internal static List<AshfallDataGrid.Row> BuildFixtureRows() => new()
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix Q.587 — Additional Current Architecture Evidence: `src/UI/MapPanel.cs`

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


# Appendix Q.588 — Additional Current Architecture Evidence: `src/UI/SubterraneanOperationsPanel.cs`

### `src/UI/SubterraneanOperationsPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 298 lines / 13140 bytes.
- SHA-256: `fff2d648bf8a2f1c1ad0985401a30eca88b4a4aa3663e95fefb803e65ba189b8`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class SubterraneanOperationsPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _excavation != null;
public void Bind( ExcavationHazardSystem excavation, Ashfall.Core.Inventory.Inventory? inventory = null, SurvivorsHostSession? survivors = null) {
public void Unbind() {
public override void _Ready() {
public override void _ExitTree() => Unbind();
public void Open() {
public void RefreshView() {
```


# Appendix Q.589 — Additional Current Architecture Evidence: `src/UI/WeatherPanel.cs`

### `src/UI/WeatherPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 485 lines / 22650 bytes.
- SHA-256: `5811f4a177cb9d2fb666a4edb00bb0dfe25de3d07b8ab67d1e6e87cf99a35481`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=3; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class WeatherPanel : Control
public event Action? OnClose;
public WeatherKind? BoundWeather => ActiveWeather?.Current;
public bool IsBound => _worldHost != null || _weatherHost != null;
public int RenderedHazardCount => _advisoryList?.GetChildCount() ?? 0;
public void Bind(WeatherHostSession weather) {
public void Bind(WorldHostSession weather) {
public int RefreshCount { get; private set; }
public void RefreshView() {
public override void _Ready() {
public void Open() {
public void Close() {
public void Unbind() {
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
```


# Appendix Q.590 — Additional Current Architecture Evidence: `src/Host/SceneBindingSelfTest.cs`

### `src/Host/SceneBindingSelfTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 338 lines / 15643 bytes.
- SHA-256: `0344f83f4be93929b3098c958a5022a014cd0185d75be3d961b93bec3ffeee06`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class SceneBindingSelfTest
public sealed class SceneCheck
public string ResPath { get; init; } = string.Empty;
public Type RootType { get; init; } = typeof(Control);
public static void Check(string resPath, Type rootType, params (string, Type)[] contract) {
public static void RegisterMigratedPanels() {
public static int Run() {
```


# Appendix Q.591 — Additional Current Architecture Evidence: `src/Main.Audio.cs`

### `src/Main.Audio.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 51 lines / 3102 bytes.
- SHA-256: `594708b19acc35f69fcfc93017aaade8ec46884eef74c4d92d576b1c9cf895f3`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : IAudioDomainProvider, IExpansionAudioProvider, IShelterOperationsAudioProvider
```


# Appendix Q.592 — Additional Current Architecture Evidence: `src/UI/ExpansionsHubPanel.cs`

### `src/UI/ExpansionsHubPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 468 lines / 22268 bytes.
- SHA-256: `01eff6eb5205c47ea31c84b2a3e55af72a649ad317844e2b4501e7456ec0d14d`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=18; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class ExpansionsHubPanel : Control
public event Action? OnClose;
public event Action<string>? OnOpenExpansionRequested;
public override void _Ready() {
public override void _UnhandledInput(InputEvent @event) {
public void Bind( ExpansionHostSession? expansions, GreenhouseHostSession? greenhouse, DutyRosterHostSession? dutyRoster, MusterHostSession? muster, MaritimeHostSession? maritime,
public void Open() {
public void Close() {
public void RefreshView() {
```


# Appendix Q.593 — Additional Current Architecture Evidence: `src/UI/ExpeditionPanel.cs`

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


# Appendix Q.594 — Additional Current Architecture Evidence: `src/UI/ShelterPanel.cs`

### `src/UI/ShelterPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 496 lines / 24603 bytes.
- SHA-256: `44b789312dfaa95613503bdc0fab1d53282223364e596a8abfe7dfef857b006e`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=4; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class ShelterPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<string>? RoomSelected;
public bool IsBound => _survivorsHost != null && _worldHost != null;
public int RenderedStructureCount => _structureList?.GetChildCount() ?? 0;
public void SetMachineTellCatalog(Ashfall.Core.Shelter.ShelterMachineTellCatalog? catalog) {
public void SetPresentationSlate(Ashfall.Core.Presentation.HoldfastPresentationSlate? slate) {
public void SetLightingPhase(string phase) {
public void SetGraffitiCatalog(Ashfall.Core.Narrative.BunkerGraffitiCatalog? catalog, int currentDay = int.MaxValue) {
public void Bind( SurvivorsHostSession survivors, WorldHostSession world, InventoryHostSession? inventory = null, ShelterRoomIdentityCatalog? roomIdentities = null, Ashfall.Core.Narrative.BunkerGraffitiCatalog? graffiti = null,
public void RefreshView() {
public override void _Ready() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix Q.595 — Additional Current Architecture Evidence: `src/World/WastelandMapView.cs`

### `src/World/WastelandMapView.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 278 lines / 10965 bytes.
- SHA-256: `0b1f0f573f111a8639b4df9d576a3932b9c5d8c873714caafa848eb6d03e0607`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=9; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class WastelandMapView : Node2D
public delegate void NodeSelectedEventHandler(string nodeId);
public override void _Ready() {
public void Bind(WorldHostSession? worldHost) {
public void Bind(WastelandMapSystem? mapSystem) {
public void Initialize() {
public MapLocationMarkerStatus ResolveNodeStatus(MapNode node) {
public override void _ExitTree() {
```


# Appendix R.596 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Balance/ShelterOperationsBalanceSim.cs`

### `Ashfall.Core.Tests/Balance/ShelterOperationsBalanceSim.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 611; SHA-256: `f8333a8946d13b291c2a6a3c9504adefa111c1b1c1e7295cfcd10632bd167942`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Generate100DayBalanceCsv_AndAssertInvariants
DeterministicByteParity_Seed98765
VentilationBlower_ControlsMethaneEquilibrium
ToolingWearSweep_DegradesMachinesPredictably
MonteCarlo_100Seeds_StandardSurvival_InvariantsHold
```


# Appendix R.597 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs`

### `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs`

- Current test declarations: Fact=2, Theory=0, InlineData=0.
- File lines: 204; SHA-256: `afeaa1b6d204749d02296af2fb3b06d75309f18d7957f67148f903e93d8403f1`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Continuity_30DayDeterministicReplay_ProducesIdenticalOutputs
Continuity_Day15SaveRestoreSplit_MatchesContinuousRun
```


# Appendix R.598 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Integration/PlansB66ToB69CrossSystemTests.cs`

### `Ashfall.Core.Tests/Integration/PlansB66ToB69CrossSystemTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 535; SHA-256: `0d6d5a79ba3e2786972740fb0809f3d21fb80758588d06a2e3a1914f00262781`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ScenarioA_BeamBatch_ReinforcesDeepStrata_NoDuplicateAcrossSave
ScenarioB_PreparedShelter_WeakensPulse_Deterministically
ScenarioC_QuakeDuringHeavyBatch_BatchNotDuplicatedOrReset
ScenarioD_PowerCrisisSplitRun_MatchesUninterruptedRun
ScenarioE_SevereQuakeBreach_TriagePreservesProtectedLine
ScenarioF_InterceptResolution_RecoveryReleasesCanonicalSeed
ScenarioG_InfrastructureStress_SplitEqualsStraight
ScenarioG_ThirtyDayCampaign_SplitAndPairedRerunsProduceIdenticalState
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.


# Appendix S — Quality Assurance Pass Record: Plan 38

This record is part of the planning artifact, not a fresh runtime test result.

## Pass 1 — content and premise accuracy
- Content pass replaced the missing-catalog premise with six armor and twelve events.
- The historical baseline is separated from the current source/data/test authority.
- Current row counts and owner boundaries are stated without using count as a quality proxy.

## Pass 2 — integration architecture
- Integration pass traced shared world telemetry → armor → warning/interception → save.
- Core, data, host, UI, save, event and test seams are named with current paths.
- The plan does not authorize a parallel save section, catalog, manager or host cache.

## Final precision and reaccuracy pass
- Precision pass protects one live instance and false-positive/degradation/salvage semantics.
- Every embedded current-file hash, focused runner command and master-authority reference is rechecked.
- Any proposed future seam is labeled as requiring a separate claim and premise verification.
