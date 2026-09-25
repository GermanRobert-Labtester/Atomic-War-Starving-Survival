# Plan 10 — Combat, Expedition, Bestiary, Armory, and Fleet Architecture

> **Rebuild status:** COMPLETE ORIGINAL SCOPE — CURRENT AUTHORITY AND REALTIME-EXPANSION MAINTENANCE PLAN
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

- Current data contains 20 weapons, 14 ammunition rows, 7 materials and 12 combatants. The combat catalog is schema 2 and is consumed through the registry/factory and tactical runtime.
- The live architecture has expanded beyond the historical plan into realtime combat partials, arena data, vehicle armor grades, garage systems, route integration and deterministic tests. The plan now protects those newer authorities.
- The remaining work is verification and balance: ensure every authored row is reachable, every combat entry point uses the canonical system, vehicle consequences route correctly, and new realtime mechanics preserve determinism and accessibility.

**Bounded outcome:** The original Plan 10 scope is integrated. Current combat is owned by `TacticalCombatSystem` and its partials, `CombatCatalog`, `BallisticsSystem`, `WeaponConditionSystem`, expedition vehicle/garage owners, dive systems and bestiary. No generic `CombatSystem`, parallel armory, vehicle fleet or dive-site manager is allowed.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `TacticalCombatSystem` owns tactical and realtime command surfaces through partial class files.
- `CombatCatalog` and `CombatantFactory` convert authored definitions into runtime combatants.
- The current combat catalog has 20 weapons, 14 ammo, 7 materials and 12 combatants.
- Vehicle authority is split across `ExpeditionVehicleSystem`, `VehicleGarageSystem`, armor grades and host composition.
- Bestiary knowledge is owned by `BestiarySystem` and wildlife catalogs; combat does not duplicate species state.

**Master-authority sections applied to this rebase:**

- Volumes 7–8 contracts/harness
- Volume 17 C5/C15 roadmaps
- Volume 28 verification cookbook

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Treat the original plan as complete and maintain the current architecture map.
- Audit all combat entry points for canonical TacticalCombatSystem use.
- Verify vehicle/armor/dive consequences route to medical, dose and expedition owners.
- Add balance and reachability evidence for newly added realtime arenas and weapon cadence fields.

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
| tactical/realtime encounter state and commands | TacticalCombatSystem | `Assets/Ashfall.Core/Combat/TacticalCombatSystem*.cs` | Sole combat runtime. |
| weapon, ammo, material and combatant definitions | Combat catalog | `Assets/StreamingAssets/Data/combat_catalog.json` | Authoritative combat data. |
| definition-to-runtime conversion | CombatantFactory | `Assets/Ashfall.Core/Combat/CombatCatalog.cs` | Sole conversion point. |
| vehicle condition, garage, armor and expedition preparation | Expedition vehicle owners | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs; Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs` | Combat consumes vehicle facts; it does not re-own them. |
| species discovery and player knowledge | BestiarySystem | `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` | Wildlife knowledge authority. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Combat, Expedition, Bestiary, Armory, and Fleet Architecture
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ TacticalCombatSystem
│   tactical/realtime encounter state and commands
│ Combat catalog
│   weapon, ammo, material and combatant definitions
│ CombatantFactory
│   definition-to-runtime conversion
│ Expedition vehicle owners
│   vehicle condition, garage, armor and expedition preparation
│ BestiarySystem
│   species discovery and player knowledge
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

1. **Preserve current state ownership.** TacticalCombatSystem owns tactical/realtime encounter state and commands: Sole combat runtime.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| tactical/realtime encounter state and commands | TacticalCombatSystem | `Assets/Ashfall.Core/Combat/TacticalCombatSystem*.cs` | Sole combat runtime. |
| weapon, ammo, material and combatant definitions | Combat catalog | `Assets/StreamingAssets/Data/combat_catalog.json` | Authoritative combat data. |
| definition-to-runtime conversion | CombatantFactory | `Assets/Ashfall.Core/Combat/CombatCatalog.cs` | Sole conversion point. |
| vehicle condition, garage, armor and expedition preparation | Expedition vehicle owners | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs; Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs` | Combat consumes vehicle facts; it does not re-own them. |
| species discovery and player knowledge | BestiarySystem | `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` | Wildlife knowledge authority. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. select expedition destination/stance
2. assemble combatant and weapon definitions
3. create deterministic encounter
4. execute turn/realtime command
5. resolve ballistics/condition/AI
6. emit injury/noise/loot facts
7. persist combat and expedition outcomes

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Combat state is save-owned by the existing combat persistence partial.
- Vehicle condition remains vehicle-owned.
- Bestiary observations remain bestiary-owned.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Every combatant ID resolves through CombatantFactory.
- Every weapon/ammo/material reference resolves to the catalog.
- Unknown combatant rows produce an explicit fallback event, not silent success.
- Realtime commands use the same state/version contract as tactical commands.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- Maintain current combat schema 2.
- New arenas and cadence fields extend existing rows.
- No duplicate vehicle or dive catalogs.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use current CombatState persistence and expedition/vehicle sections.
- Realtime action state must round-trip at command boundaries.
- No new combat save section.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- All random resolution uses injected seeded streams.
- Command order and RNG consumption are part of the public replay contract.
- No wall-clock frame timing decides gameplay truth.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Combat state/event callbacks feed expedition, injury, trauma, noise and journal owners.
- Vehicle breakdown consequences route through the canonical expedition host.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/CombatHostSession.cs
- src/UI/CombatPanel.cs
- src/Host/ExpeditionHostSession.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Bestiary and encounter prose must describe modeled behavior.
- No real-world faction/war imitation.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A new entry point constructs ad hoc enemies. | TacticalCombatSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A vehicle consequence is applied twice. | Combat catalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Realtime input bypasses state-version checks. | CombatantFactory | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | An unreachable weapon/ammo row passes integrity. | Expedition vehicle owners | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | UI claims an outcome the simulation did not produce. | BestiarySystem | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/TacticalCombatSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Combat/TacticalCombatDeterminismTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/CombatSaveRoundTripTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Combat/RealtimeFireCadenceTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/ExpeditionVehicleSystemTests.cs`
6. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan213VehicleArmorGradeTests.cs`
7. `godot --headless --path . -- --data-integrity-selftest` and the focused headless combat probe when runtime wiring changes.

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — current architecture census | Inventory all combat partials, data and host callers. | One runtime owner and current row counts. | No production path until the owning implementation package is separately claimed. |
| 1 — entry-point audit | Find ad hoc encounter construction and route it to canonical systems. | No parallel combat path remains. | No production path until the owning implementation package is separately claimed. |
| 2 — vehicle/dive consequence audit | Verify injury/dose/noise consequences use owners. | No duplicate drain or exposure. | No production path until the owning implementation package is separately claimed. |
| 3 — realtime persistence | Prove save/restore at action boundaries. | Paired replay is identical. | No production path until the owning implementation package is separately claimed. |
| 4 — balance and accessibility | Measure TTK/cadence and run a11y/controller checks. | Tuning proposals are evidence-backed. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/Ashfall.Core/Combat/TacticalCombatSystem*.cs | READ; MODIFY only through a future combat claim | Runtime authority |
| Assets/StreamingAssets/Data/combat_catalog.json | DATA-ONLY maintenance | Combat definitions |
| Assets/Ashfall.Core/ExpeditionVehicleSystem.cs | READ ONLY | Vehicle authority |
| src/UI/CombatPanel.cs | READ; MODIFY only for truth/accessibility gap | Presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Recreating CombatSystem. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Changing RNG cadence during UI work. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Vehicle and combat both charging wear/fuel. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Treating catalog presence as reachability. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new generic combat engine.
- No new vehicle authority.
- No balance numbers in the documentation package.
- No Unity restoration.

# 23. Rollback and Recovery

- Combat runtime changes require isolated commits and deterministic replay proof.
- Data additions revert by row.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- Current combat ownership is explicit.
- Catalog counts are current.
- Entry points and consequences are audited.
- No parallel authority is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Treat the original plan as complete and maintain the current architecture map.
- Audit all combat entry points for canonical TacticalCombatSystem use.
- Verify vehicle/armor/dive consequences route to medical, dose and expedition owners.
- Add balance and reachability evidence for newly added realtime arenas and weapon cadence fields.

## MUST NOT DO

- No new generic combat engine.
- No new vehicle authority.
- No balance numbers in the documentation package.
- No Unity restoration.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/TacticalCombatSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Combat/TacticalCombatDeterminismTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/CombatSaveRoundTripTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Combat/RealtimeFireCadenceTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/ExpeditionVehicleSystemTests.cs`
6. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan213VehicleArmorGradeTests.cs`
7. `godot --headless --path . -- --data-integrity-selftest` and the focused headless combat probe when runtime wiring changes.

## FIRST SAFE IMPLEMENTATION STEP

0 — current architecture census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: tactical/realtime encounter state and commands → TacticalCombatSystem; weapon, ammo, material and combatant definitions → Combat catalog; definition-to-runtime conversion → CombatantFactory; vehicle condition, garage, armor and expedition preparation → Expedition vehicle owners; species discovery and player knowledge → BestiarySystem. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 10.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 10 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by TacticalCombatSystem or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs`

### `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 440 lines / 21741 bytes.
- SHA-256: `a2f1de89201692d23312afa55cc4f532506f56299928bd00c13774c877b7c25b`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public struct StanceMods
public float Accuracy;      // player fire accuracy
public float Damage;        // player fire damage
public float Defense;       // reduces enemy accuracy (0..1)
public float AmmoUse;       // rounds per burst multiplier
public float Degrade;       // weapon degradation multiplier
public float JamRisk;       // jam chance multiplier
public float Noise;         // encounter exposure / noise
public float Mobility;      // chance to successfully flee
public float MoraleDelta;   // stance morale impact
public bool CanFlee;        // may this stance retreat?
public bool DeathIsInstant; // last stand: 0 HP = instant death + mutual kill
public partial class TacticalCombatSystem
public const string SystemId = "combat_system";
public const int DefaultBleedTurns = 3;
public const int DefaultSuppressDuration = 1;
public const int MaxRicochetBounces = BallisticsSystem.MaxRicochetCount;
public event Action<CombatState> OnStateChanged;
public event Action<CombatState, CombatEvent> OnCombatEvent;
public event Action<CombatState> OnEncounterEnded;
public CombatState State => _state;
public CombatHostPorts Ports { get => _ports; set => _ports = value; }
public CombatDoctrineCapability DoctrineCapability { get; set; } = CombatDoctrineCapability.None;
public CombatPerks? PerksFor(string survivorId, int seed) {
public static string StanceId(TacticalStance s) => "combat_stance_" + s.ToString().ToLowerInvariant();
public static bool TryParseStance(string id, out TacticalStance stance) {
public static StanceMods GetStanceMods(TacticalStance s) {
public bool BeginEncounter( string encounterId, string expeditionId, string locationId, string locationName, int day,
public float GetBoundWeaponStartCondition(string instanceId, float defaultVal = 1f) {
public void SetBoundWeaponStartCondition(string instanceId, float condition) {
public ActionPreflight EvaluateFire(string targetId) {
public ActionPreflight EvaluateSuppress() {
public ActionPreflight EvaluateClearJam(string subjectId) {
public ActionPreflight EvaluateReload(string subjectId) {
public ActionPreflight EvaluateRepair(string subjectId) {
public ActionPreflight EvaluateRetreat() {
public ActionPreflight EvaluateEndTurn() {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Realtime.cs`

### `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Realtime.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 477 lines / 18454 bytes.
- SHA-256: `019cbfe91c0aebb9f09655966988c7ab86820f3bd08ec9f86b9f257d4e1cf305`.
- Architecture signals: seeded references=4; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class TacticalCombatSystem
public const float RealtimeSimDt = 1f / 20f;
public bool EnableRealtime(string? arenaId = null, ISeededRng? rng = null) {
public CombatArenaDefinition ActiveArena =>
public CombatActionResult TickRealtime(float dt, CombatInputFrame? input, ISeededRng? rng) {
public static float EffectiveRoundsPerMinute(CombatWeaponDefinition? def) {
public static float FireCooldownSeconds(CombatWeaponDefinition? def) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.RealtimeAi.cs`

### `Assets/Ashfall.Core/Combat/TacticalCombatSystem.RealtimeAi.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 407 lines / 17079 bytes.
- SHA-256: `9182a381a24b3494b5c5781fca7a93b2a1de586711bfb52f17690cc215b74ffb`.
- Architecture signals: seeded references=3; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class TacticalCombatSystem
public static bool IsBurrowHidden(CombatantState c) =>
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/Combat/CombatCatalog.cs`

### `Assets/Ashfall.Core/Combat/CombatCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 677 lines / 32526 bytes.
- SHA-256: `d16451f581da6da106751f340f2aa5ad50a52ddea05c5634508bf6846365bb68`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class CombatWeaponDefinition
public string id = string.Empty;
public string displayName = string.Empty;
public float accuracy = 0.6f;        // base chance to connect
public float damage = 12f;
public float range = 1f;             // range modifier (1 = standard)
public string caliber = string.Empty;// ammo id the weapon takes
public int burst = 1;                // rounds per trigger pull
public bool isJuryRigged;            // pipe_ / improvised firearm
public bool isSuppressionCapable;    // rifle/LMG can lay suppressive fire
public float degradePerShot = 0.015f;// condition loss per shot fired
public float jamBase = 0.04f;        // base jam chance at pristine condition
public int scrapRepairCost = 3;      // scrap metal to field-repair to full
public float conditionThreshold = 0.25f; // below this, jam risk rises steeply
public float roundsPerMinute;
public float aimBraceBonus;
public class CombatAmmoDefinition
public string id = string.Empty;     // caliber id
public string displayName = string.Empty;
public float damageMod = 1f;
public float rangeMod = 1f;
public bool isMilitaryTier;          // military ammo in a jury weapon risks burst failure
public class CombatMaterialDefinition
public string id = string.Empty;
public string displayName = string.Empty;
public string kind = "cover";        // cover | armor | barrier
public float armorReduction;         // fraction of damage absorbed
public float ricochetChance;         // chance to deflect a hit
public float ricochetEnergyRetained = 0.6f;
public class CombatantDefinition
public string id = string.Empty;
public string displayName = string.Empty;
public string kind = "human";            // human | mutant | fauna
public string factionId = string.Empty;  // canonical faction id (faction_*), blank = unaligned
public string description = string.Empty;
public float baseHealth = 100f;
public float baseArmorRating;            // 0..1
public float baseCoverRating;            // 0..1
public int preferredLane;                // CombatLane value
public string aiStancePreference = "HoldPosition"; // TacticalStance name
public string aiSpecialMove = "None";    // None | Burrow | Flank | Spore | Charge | SuppressiveFire | TacticalRetreat
public float aiAccuracyMod = 1f;         // multiplier on weapon accuracy
public float aiDamageMod = 1f;           // multiplier on outgoing damage
public float surrenderThreshold = -1f;   // -1 = never; otherwise 0..1
public float fleeThreshold = -1f;        // -1 = never; otherwise 0..1
public int journalKey = 0;               // optional narrative hook id
public static class CombatCatalog
public static void SeedDefaults() {
public static void Register(CombatWeaponDefinition def) {
public static void Register(CombatAmmoDefinition def) {
public static void Register(CombatMaterialDefinition def) {
public static CombatWeaponDefinition GetWeapon(string id) {
public static CombatAmmoDefinition GetAmmo(string id) {
public static CombatMaterialDefinition GetMaterial(string id) {
public static CombatantDefinition? GetCombatant(string id) {
public static bool HasWeapon(string id) => GetWeapon(id) != null;
public static bool HasAmmo(string id) => GetAmmo(id) != null;
public static bool HasMaterial(string id) => GetMaterial(id) != null;
public static bool HasCombatant(string id) => GetCombatant(id) != null;
public static IReadOnlyCollection<string> WeaponIds => s_weapons.Keys;
public static IReadOnlyCollection<string> AmmoIds => s_ammo.Keys;
public static IReadOnlyCollection<string> MaterialIds => s_materials.Keys;
public static IReadOnlyCollection<string> CombatantIds => s_combatants.Keys;
public static void Register(CombatantDefinition def) {
public static void Clear() {
public enum CombatantSpawnStatus
public static class CombatantFactory
public const string SystemId = "combatant_factory";
public static CombatantState? SpawnFromCatalog(string combatantId) {
public static CombatantState SpawnFromCatalogOrThrow(string combatantId) {
public static bool TrySpawnFromCatalog(string combatantId, out CombatantState? result, out CombatantSpawnStatus status) {
internal sealed class CombatWeaponJson
public string id;
public string display_name;
public float accuracy;
public float damage;
public float range;
public string caliber;
public int burst;
public bool is_jury_rigged;
public bool is_suppression_capable;
public float degrade_per_shot;
public float jam_base;
public int scrap_repair_cost;
public float condition_threshold;
public float rounds_per_minute;
public float aim_brace_bonus;
internal sealed class CombatAmmoJson
public string id;
public string display_name;
public float damage_mod;
public float range_mod;
public bool is_military_tier;
internal sealed class CombatMaterialJson
public string id;
public string display_name;
public string kind;
public float armor_reduction;
public float ricochet_chance;
public float ricochet_energy_retained;
internal sealed class CombatantJson
public string id;
public string display_name;
public string kind;             // human | mutant | fauna
public string faction_id;       // optional
public string description;
public float base_health;
public float base_armor_rating;
public float base_cover_rating;
public int preferred_lane;      // 0/1/2
public string ai_stance_preference;
public string ai_special_move;
public float ai_accuracy_mod;
public float ai_damage_mod;
public float surrender_threshold;
public float flee_threshold;
public int journal_key;
internal sealed class CombatCatalogRoot
public int schema_version = 1;
public string collection_id = "combat_catalog";
public List<CombatWeaponJson> weapons = new List<CombatWeaponJson>();
public List<CombatAmmoJson> ammo = new List<CombatAmmoJson>();
public List<CombatMaterialJson> materials = new List<CombatMaterialJson>();
public List<CombatantJson> combatants = new List<CombatantJson>();
public static class CombatCatalogLoader
public const string FileName = "combat_catalog.json";
public const int CurrentSchemaVersion = 2;
public static bool Load(string dataDirectory, IFileIO files, IJsonSerializer json) {
public string faction_id = string.Empty;
```


# Appendix B.06 — Current Code Architecture: `Assets/Ashfall.Core/Combat/BallisticsSystem.cs`

### `Assets/Ashfall.Core/Combat/BallisticsSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 330 lines / 14771 bytes.
- SHA-256: `cb82487673a5c437bec30e4f577088dae9f5845a2eb8e1aaf6dc3cd5a1ed7119`.
- Architecture signals: seeded references=3; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class BallisticOutcome
public string ShooterId = string.Empty;
public string ShooterName = string.Empty;
public string WeaponId = string.Empty;
public string WeaponName = string.Empty;
public string AmmoId = string.Empty;
public string AmmoName = string.Empty;
public string IntendedTargetId = string.Empty;
public string ResolvedTargetId = string.Empty;
public string ResolvedSurfaceId = string.Empty; // cover/barrier material
public bool IsPlayerShooter;
public BallisticResult Result;
public BallisticReason Reason;
public string ReasonCode => Reason.ToString();
public float InitialEnergy;
public float DamageDealt;
public float ArmorAbsorbed;
public float CoverAbsorbed;
public float BarrierAbsorbed;
public float EnergyRemaining;      // redirected/outgoing energy after the path
public bool IsCritical;
public List<string> Path = new List<string>();
public void PushStep(string step) => Path.Add(step);
public class BallisticContext
public string ShooterId = string.Empty;
public string ShooterName = string.Empty;
public bool IsPlayerShooter;
public string WeaponId = string.Empty;
public string WeaponName = string.Empty;
public float WeaponAccuracy = 0.6f;
public float WeaponDamage = 12f;
public float WeaponRangeMod = 1f;
public string AmmoId = string.Empty;
public string AmmoName = string.Empty;
public float AmmoDamageMod = 1f;
public float AmmoRangeMod = 1f;
public float StanceAccuracyMod = 1f;
public float StanceDamageMod = 1f;
public float ExternalAccuracyMod = 1f;   // perks / condition
public float ExternalDamageMod = 1f;     // perks / close quarters / flanking
public bool IsFirstShotCritBonus;        // Cold Bore
public float ExtraCritChance;
public CombatantState IntendedTarget;
public CombatMaterialDefinition CoverMaterial;   // cover in target's lane (optional)
public CombatMaterialDefinition ArmorMaterial;   // worn armor (optional)
public CombatMaterialDefinition BarrierMaterial; // lane barrier (optional)
public float BarrierIntegrityPct = 100f;
public List<CombatantState> RicochetTargets = new List<CombatantState>(); // adjacent-lane living enemies
public static class BallisticsSystem
public const int MaxRicochetCount = 2;
public const float MinResidualEnergy = 2f;
public static BallisticOutcome Resolve(BallisticContext ctx, ISeededRng rng) {
```


# Appendix B.07 — Current Code Architecture: `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs`

### `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 464 lines / 20023 bytes.
- SHA-256: `c897d11e111c3ff4e559d01a4be0241b2e291de005cd4fce076090f3ac450eb2`.
- Architecture signals: seeded references=4; save/restore symbols=2; typed event declarations=12; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum VehicleBreakdownKind
public sealed class VehicleBreakdownOutcome
public bool BrokeDown { get; }
public VehicleBreakdownKind Kind { get; }
public float Severity01 { get; }
public float NominalMsV { get; }
public string Cause { get; }
public string VehicleId { get; }
public string SurvivorId { get; set; } = string.Empty;
public static VehicleBreakdownOutcome None(string vehicleId, string reason) => new VehicleBreakdownOutcome(VehicleBreakdownKind.None, 0f, 0f, reason, vehicleId);
public sealed class ExpeditionVehicleState
public string systemId = ExpeditionVehicleSystem.SystemId;
public Dictionary<string, VehicleInstance> ownedVehicles = new Dictionary<string, VehicleInstance>();
public string activeExpeditionVehicleId = string.Empty;
public sealed class VehicleInstance
public string vehicleId = string.Empty;
public string displayName = string.Empty;
public float condition = 100f;
public float fuel;
public float maxFuel = 50f;
public float cargoCapacity = 100f;
public float speedMultiplier = 1f;
public string terrainType = "road";
public bool isBrokenDown;
public string breakdownCause = string.Empty;
public List<string> attachments = new List<string>();
public VehicleTrackGearState trackGear = new VehicleTrackGearState();
public sealed class VehicleTrackGearState
public string gearId = string.Empty;
public float condition = 100f;
public float tractionMultiplier = 1f;
public float breakdownRiskMultiplier = 1f;
public bool IsInstalled => !string.IsNullOrEmpty(gearId);
public float EffectiveTractionMultiplier() {
public float EffectiveBreakdownRiskMultiplier() {
public sealed class VehicleDefinition
public string vehicle_id = string.Empty;
public string display_name = string.Empty;
public float max_fuel = 50f;
public float cargo_capacity = 100f;
public float speed_multiplier = 1f;
public string terrain_type = "road";
public float condition_max = 100f;
public float fuel_consumption_per_km = 0.5f;
public float breakdown_threshold = 0.2f;
public List<string> default_attachments = new List<string>();
public sealed class VehicleTrackGearDefinition
public string gear_id = string.Empty;
public string display_name = string.Empty;
public float traction_multiplier = 1.1f;
public float breakdown_risk_multiplier = 0.9f;
public sealed class VehicleCatalog
public int schema_version = 1;
public List<VehicleDefinition> vehicles = new List<VehicleDefinition>();
public List<VehicleTrackGearDefinition> track_gear = new List<VehicleTrackGearDefinition>();
public sealed class ExpeditionVehicleSystem
public const string SystemId = "expedition_vehicle";
public ExpeditionVehicleState State => _state;
public event Action OnVehicleStateChanged;
public void LoadCatalog(VehicleCatalog catalog) {
public VehicleDefinition? GetDefinition(string id) {
public VehicleTrackGearDefinition? GetTrackGearDefinition(string id) {
public ActionResult AcquireVehicle(string vehicleId) {
public VehicleInstance? GetVehicle(string vehicleId) {
public ActionResult Refuel(string vehicleId, float amount) {
public ActionResult Repair(string vehicleId, float amount) {
public ActionResult AttachEquipment(string vehicleId, string equipmentId) {
public ActionResult InstallTrackGear( string vehicleId, string gearId, float tractionMultiplier, float breakdownRiskMultiplier, float condition = 100f)
public ActionResult InstallTrackGear(string vehicleId, string gearId, float condition = 100f) {
public ActionResult RemoveTrackGear(string vehicleId) {
public ActionResult RepairTrackGear(string vehicleId, float amount) {
public ExpeditionVehicleProfile? CreateExpeditionProfile( string vehicleId, float kmPerTravelTick = 2.5f) {
public event Action<VehicleBreakdownOutcome>? OnBreakdownResolved;
public VehicleBreakdownOutcome ResolvePrepBreakdown(string vehicleId, float distanceKm, ISeededRng? rng = null) {
public ExpeditionVehicleState CaptureState() => CloneState(_state);
public void RestoreState(ExpeditionVehicleState saved) {
```


# Appendix B.08 — Current Code Architecture: `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs`

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


# Appendix B.09 — Current Code Architecture: `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs`

### `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 764 lines / 30593 bytes.
- SHA-256: `5a1949b6e1e1822ebaa20bb08297b3d7e236d6cdcb80b6c8bae1358c73aa0542`.
- Architecture signals: seeded references=3; save/restore symbols=3; typed event declarations=21; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum DiveRoomType { Deckhouse, Companionway, HoldApproach, DeepHold } public enum DiveResult { Success, Partial, Contaminated, Failed, CrewLost }
public sealed class DiveRoomNode
public DiveRoomType roomType;
public float searchProgress; // 0.0 to 100.0
public bool isLooted;
public int hazardLevel; // 1 to 5
public sealed class DiveSite
public string siteId = string.Empty;
public string displayName = string.Empty;
public float depthMeters;
public float hazardLevel;     // 0-1
public bool isExplored;
public bool isHazardous;
public float radiationLevel;
public sealed class DiveOutcome
public string siteId = string.Empty;
public int day;
public DiveResult result;
public string recoveredItemId = string.Empty;
public float radiationDose;
public string notes = string.Empty;
public class StealthDiveSaveState
public string systemId = "maritime_dive";
public bool isActive;
public string siteId = string.Empty;
public string diverDwellerId = string.Empty;
public string compressorOperatorDwellerId = string.Empty;
public float airSupplySeconds = 120f;
public float maxAirSupplySeconds = 120f;
public int currentRoomIndex;
public int noiseLevel; // 0 to 100
public bool isCompromised;
public float decompressionRequiredSeconds;
public float decompressionProgressSeconds;
public bool isDecompressing;
public bool hasDecompressionSickness;
public float accumulatedRadiationDose;
public bool diverLost;
public List<DiveRoomNode> rooms = new List<DiveRoomNode>();
public List<DiveSite> sites = new List<DiveSite>();
public List<DiveOutcome> outcomes = new List<DiveOutcome>();
public sealed class MaritimeDiveState : StealthDiveSaveState
public class MaritimeDiveSystem
public const string SystemId = "maritime_dive";
public const float BaseAirPerCrank = 30f; // Seconds of air gained per manual operator crank
public DiveSiteContainer Catalog { get; private set; } = new DiveSiteContainer();
public bool IsActive { get; private set; }
public string CurrentSiteId { get; private set; } = string.Empty;
public string DiverDwellerId { get; private set; } = string.Empty;
public string CompressorOperatorDwellerId { get; private set; } = string.Empty;
public float AirSupplySeconds { get; private set; }
public float MaxAirSupplySeconds { get; private set; } = 120f;
public int CurrentRoomIndex { get; private set; }
public int NoiseLevel { get; private set; }
public bool IsCompromised { get; private set; }
public float DecompressionRequiredSeconds { get; private set; }
public float DecompressionProgressSeconds { get; private set; }
public bool IsDecompressing { get; private set; }
public bool HasDecompressionSickness { get; private set; }
public float AccumulatedRadiationDose { get; private set; }
public bool DiverLost { get; private set; }
public IReadOnlyList<DiveRoomNode> Rooms => _rooms;
public IReadOnlyList<DiveSite> Sites => _sites;
public IReadOnlyList<DiveOutcome> Outcomes => _outcomes;
public MaritimeDiveState State => CaptureState();
public event Action<float>? OnAirWarning;
public event Action<int>? OnRoomEntered;
public event Action<bool>? OnDiveEnded;
public event Action<float>? OnDecompressionStarted;
public event Action? OnDecompressionCompleted;
public event Action<string>? OnDiverLost;
public event Action<DiveOutcome>? OnDiveCompleted;
public event Action? OnSitesChanged;
public void LoadCatalog(DiveSiteContainer catalog) {
public void SeedDefaultSites() {
public ActionResult RegisterSite(string siteId, string displayName, float depthMeters, float hazardLevel) {
public void StartDive(string diverId, string operatorId, float initialAir = 120f, string siteId = "") {
public float CurrentSiteNoiseFloor { get; private set; }
public bool StartDiveAtSite(string diverId, string operatorId, string siteId) {
public bool CanStartDive(string siteId, IEnumerable<string>? ownedItemIds, out string missingItem) {
public bool CanLaunch(string siteId, int campaignDay, IEnumerable<string>? ownedItemIds, out string blocker) {
public IReadOnlyList<SafeDefinition> GetSafesForSite(string siteId) {
public IReadOnlyList<VariableLootNode> GetLootTableForSite(string siteId) {
public void Tick(float deltaSeconds) {
public void CrankCompressor() {
public bool AdvanceToNextRoom(int addedNoise) {
public void StartDecompression() {
public void AbortDive(bool emergency = false) {
public void EndDive(bool success) {
public void EndDive(bool success, bool diverLost, string reason) {
public ActionResult ConductDive(string siteId, string diverId, float equipmentQuality) {
public void TickDay(int day) {
public MaritimeDiveState CaptureState() {
public void RestoreState(StealthDiveSaveState? state) {
```


# Appendix B.10 — Current Code Architecture: `src/Host/CombatHostSession.cs`

### `src/Host/CombatHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 832 lines / 36246 bytes.
- SHA-256: `bbf3298128d237590932b551ca7d02cb5f36c41b3bee5b94fd58bcbdbc5aa64a`.
- Architecture signals: seeded references=13; save/restore symbols=6; typed event declarations=1; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CombatHostSession : HostSessionBase, IWiringReporter
public const int DemoSeed = 4242;
public TacticalCombatSystem Engine { get; }
public event Action<CombatFactionConsequence>? FactionConsequenceApplied;
public InventoryHostSession Inventory { get; set; }
public SurvivorsHostSession Survivors { get; set; }
public Ashfall.Core.EquipmentConditionSystem? Equipment { get; set; }
public BallisticsWorkbenchSystem? Ballistics { get; set; }
public ChemWarfareSystem? ChemWarfare { get; set; }
public StealthSystem? Stealth { get; set; }
public string StealthExpeditionId { get; set; } = "combat_active";
public string LastEvent { get; private set; } = string.Empty;
public void ConfigureFactionStanding(Action<string, int>? applyStanding) {
public void WireRealState( Action<string>? markCombatSurvived = null, Action<string, string>? onSurvivorDeath = null) {
public void ValidatePorts() {
public static CombatHostSession Create(string dataDir, ICampaignRngManager? campaignRng = null) {
public void ConfigureBreachingLogistics(bool vehicleAvailable = false) {
public const int DefaultAmbushEnemyCount = 3;
public string StartCombat( string locationId, string locationName, IReadOnlyList<CombatantState>? roster = null, IReadOnlyList<WeaponInstanceState>? weapons = null, int enemyCount = 0,
public string StartDemoCombat(string locationId, string locationName) => StartCombat(locationId, locationName);
public int ScheduleDay() {
public string ActionStance(string stanceId) {
public string ActionFire(string targetId) {
public string ActionSuppress() {
public string ActionClearJam(string subjectId) {
public string ActionReload(string subjectId) {
public CommandResult ActionRepair(string subjectId) {
public string ActionMoveLane(string subjectId, CombatLane lane) {
public string ActionDeployTrap() {
public string ActionDecontaminate() {
public string ActionBandage(string rescuerId, string downedId) {
public string ActionRetreat() {
public string ActionLastStand(string subjectId) {
public string ActionEndTurn() {
public void SetRealtimePumpEnabled(bool enabled) => _realtimePumpEnabled = enabled;
public void SetInputFrame(CombatInputFrame frame) =>
public bool TryEnableRealtime(string? arenaId = null) {
public int PumpRealtime(float wallDt) {
public string ActionEnvironmental(float severity) {
public ActionPreflight EvaluateFire(string targetId) => Engine.EvaluateFire(targetId);
public ActionPreflight EvaluateSuppress() => Engine.EvaluateSuppress();
public ActionPreflight EvaluateClearJam(string subjectId) => Engine.EvaluateClearJam(subjectId);
public ActionPreflight EvaluateReload(string subjectId) => Engine.EvaluateReload(subjectId);
public ActionPreflight EvaluateRepair(string subjectId) => Engine.EvaluateRepair(subjectId);
public ActionPreflight EvaluateRetreat() => Engine.EvaluateRetreat();
public ActionPreflight EvaluateEndTurn() => Engine.EvaluateEndTurn();
public string DefaultHostileTargetId() {
public string DefaultPlayerSubjectId() {
public CombatSnapshot Snapshot() => Engine.BuildSnapshot();
public string StatusLine() {
public CombatState CaptureSave() => Engine.CaptureState();
public void RestoreSave(CombatState state) => Engine.RestoreState(state);
public bool TryPersist() => CombatSaveStore.TrySave(Engine.CaptureState());
public CombatState? TryRestorePersisted() => CombatSaveStore.TryLoad();
public WiringReport GetWiringReport() {
```


# Appendix B.11 — Current Code Architecture: `src/UI/CombatPanel.cs`

### `src/UI/CombatPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 491 lines / 22471 bytes.
- SHA-256: `329125dc5858e70419b0b7c87bcb4d7a9f757affc3af2c21f5eab3677b50a4a4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=3; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class CombatPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _bound;
public void Bind(CombatHostSession combat) {
public void RefreshView() {
public override void _Ready() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix B.12 — Current Code Architecture: `src/Host/ExpeditionHostSession.cs`

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


# Appendix C.13 — Catalog Census: `Assets/StreamingAssets/Data/combat_catalog.json`

### `Assets/StreamingAssets/Data/combat_catalog.json`

- Parse: valid strict JSON.
- Schema version: `2`.
- Size: 20773 bytes / 20771 characters.
- SHA-256: `e837b53cbab1b7595a12f58c09cd7dbac753ef1736a4fbf69757a4d83f36a1c6`.
- Root keys: `ammo`, `collection_id`, `combatants`, `materials`, `schema_version`, `weapons`.

Array-path census (minimum, maximum, observed rows):

```text
ammo: min=14, max=14, observed_paths=1
combatants: min=12, max=12, observed_paths=1
materials: min=7, max=7, observed_paths=1
weapons: min=20, max=20, observed_paths=1
```

Representative record fields:

- `accuracy`
- `burst`
- `caliber`
- `condition_threshold`
- `damage`
- `degrade_per_shot`
- `display_name`
- `id`
- `is_jury_rigged`
- `is_suppression_capable`
- `jam_base`
- `range`
- `scrap_repair_cost`

Representative identifiers (ordered, capped for readability):

```text
weapon_pipe_rifle
weapon_scrap_shotgun
weapon_bolt_rifle
weapon_assault_rifle
weapon_lmg
weapon_pipe_shotgun
weapon_nail_driver
weapon_rebar_spear
weapon_molotov_thrower
weapon_service_rifle
weapon_marksman_rifle
weapon_smg
weapon_sidearm
weapon_rust_mosin
weapon_farm_carbine
weapon_revolver
weapon_coach_shotgun
weapon_trail_carbine
weapon_battle_rifle
weapon_quiet_carbine
```


# Appendix C.14 — Catalog Census: `Assets/StreamingAssets/Data/combat_arenas.json`

### `Assets/StreamingAssets/Data/combat_arenas.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 1133 bytes / 1133 characters.
- SHA-256: `de283146d88276f7cbaa247aa582a630d65dc87941502e96e7f44de478b475c6`.
- Root keys: `arenas`, `collection_id`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
arenas: min=1, max=1, observed_paths=1
arenas[].climb_segments: min=1, max=1, observed_paths=1
arenas[].cover_nodes: min=2, max=2, observed_paths=1
arenas[].enemy_spawns: min=3, max=3, observed_paths=1
arenas[].lane_spines: min=3, max=3, observed_paths=1
arenas[].player_spawns: min=3, max=3, observed_paths=1
```

Representative record fields:

- `climb_segments`
- `climb_speed`
- `cover_nodes`
- `enemy_spawns`
- `extract_volume`
- `flee_speed`
- `height`
- `id`
- `lane_spines`
- `player_spawns`
- `run_speed`
- `walk_speed`
- `width`

Representative identifiers (ordered, capped for readability):

```text
arena_lane_spine_default
```


# Appendix C.15 — Catalog Census: `Assets/StreamingAssets/Data/vehicles.json`

### `Assets/StreamingAssets/Data/vehicles.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 3044 bytes / 3044 characters.
- SHA-256: `cdec845708de1d2d3181767aeed9f4b4565a48596835415340ed557fd9b9ce82`.
- Root keys: `schema_version`, `track_gear`, `vehicles`.

Array-path census (minimum, maximum, observed rows):

```text
track_gear: min=1, max=1, observed_paths=1
vehicles: min=8, max=8, observed_paths=1
vehicles[].default_attachments: min=0, max=0, observed_paths=2
```

Representative record fields:

- `breakdown_threshold`
- `cargo_capacity`
- `condition_max`
- `default_attachments`
- `display_name`
- `fuel_consumption_per_km`
- `max_fuel`
- `speed_multiplier`
- `terrain_type`
- `vehicle_id`


# Appendix C.16 — Catalog Census: `Assets/StreamingAssets/Data/vehicle_armor_grades.json`

### `Assets/StreamingAssets/Data/vehicle_armor_grades.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 4568 bytes / 4568 characters.
- SHA-256: `8e62dc6b78f7e97b107078bd3802577edfdba0b399abc8e70d1479dd76d03faa`.
- Root keys: `default_grade_id`, `grades`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
grades: min=5, max=5, observed_paths=1
grades[].compatible_terrain_types: min=3, max=3, observed_paths=2
grades[].install_cost: min=0, max=2, observed_paths=2
grades[].reforge_cost: min=0, max=1, observed_paths=2
grades[].tags: min=2, max=3, observed_paths=2
```

Representative record fields:

- `compatible_terrain_types`
- `description`
- `display_name`
- `fuel_consumption_multiplier`
- `id`
- `install_cost`
- `install_labor_ticks`
- `integrity_pool_permille`
- `is_default`
- `mitigation_permille`
- `reforge_cost`
- `speed_multiplier_delta`
- `tags`
- `tier`
- `wear_absorption_permille`

Representative identifiers (ordered, capped for readability):

```text
grade_0_stock
grade_1_scrap_plate
grade_2_sheet_plate
grade_3_composite_plate
grade_4_alloyed_heavy_plate
```


# Appendix C.17 — Catalog Census: `Assets/StreamingAssets/Data/dive_sites.json`

### `Assets/StreamingAssets/Data/dive_sites.json`

- Parse: valid strict JSON.
- Schema version: `2`.
- Size: 23434 bytes / 23434 characters.
- SHA-256: `f4f273ab223859ce24fec441d2ba9612cf284d885fd4055d18cab2832832a7e9`.
- Root keys: `dive_sites`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
dive_sites: min=14, max=14, observed_paths=1
dive_sites[].loot_table: min=3, max=3, observed_paths=2
dive_sites[].rooms: min=4, max=4, observed_paths=2
dive_sites[].safes: min=1, max=1, observed_paths=1
dive_sites[].safes[].loot: min=2, max=2, observed_paths=1
```

Representative record fields:

- `base_noise_floor`
- `contamination_key`
- `discovery`
- `keeper_thread_id`
- `location_id`
- `loot_table`
- `name`
- `oxygen_budget_ticks`
- `required_item_count`
- `required_item_id`
- `rooms`
- `safes`
- `site_id`
- `tide_window`


# Appendix C.18 — Catalog Census: `Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json`

### `Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 19491 bytes / 19488 characters.
- SHA-256: `e8a2feda7886c8ffa53710bb86de837825fe36ad1faaf32f51d2474047cb66b9`.
- Root keys: `collection_id`, `creatures`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
creatures: min=24, max=24, observed_paths=1
creatures[].harvestable_materials: min=3, max=3, observed_paths=2
creatures[].tags: min=6, max=6, observed_paths=2
```

Representative record fields:

- `acoustic_lure_frequency_hz`
- `butchered_meat_calories`
- `colloquial_name`
- `common_name`
- `creature_id`
- `harlan_scout_notes`
- `harvestable_materials`
- `pack_size_range`
- `primary_habitat`
- `tags`
- `threat_level`


# Appendix D.19 — Existing Focused Test Inventory: `Ashfall.Core.Tests/TacticalCombatSystemTests.cs`

### `Ashfall.Core.Tests/TacticalCombatSystemTests.cs`

- Current test declarations: Fact=14, Theory=0, InlineData=0.
- File lines: 232; SHA-256: `552b3dbdf4b0687a50adc624fb594a4340c15fbfd271ed4a68252f0d686c4c8a`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
BeginEncounter_StartsInPlayerTurn
SetStance_IsSerialized
StanceMods_ProduceDistinctTradeoffs
FiringConsumesAmmoAndDegradesWeapon
Suppression_PinsEnemiesAndStopsTheirFire
MoveLane_ChangesPosition
Retreat_Succeeds_EndsEncounter
Retreat_Failure_Injures
LastStand_SetsFlagAndTerminalStance
DownedPlayer_BleedsOutAndDies
Victory_GrantsLootAndMoraleAndSurvivorSurvival
EnvironmentalAsh_JamsEquippedWeapons
ClearJam_WorksAfterJam
BuildSnapshot_ReflectsState
```


# Appendix D.20 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Combat/TacticalCombatDeterminismTests.cs`

### `Ashfall.Core.Tests/Combat/TacticalCombatDeterminismTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 315; SHA-256: `8b55510b5dd18342adf5083f62981cb640ad4d53678cb08cd1627557292339c7`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
B3_005_SameSeedReplayProducesIdenticalEventSequence
B3_007_WeaponConditionConversionPinned
B3_009_CombatDoctrineCapability_PureProjectionFromResearch
B3_010_TraumaAndMoraleConsequencesAppliedExactlyOnce
B3_011_WeaponWearTrackedInAftermathAndPreservedAcrossSave
B3_012_AmmoDeductedDuringAction_AftermathSummarizesWithoutDoubleCharge
B3_016_MidEncounterSaveParity_ContinuationProducesIdenticalOutcome
B3_018_ActionPreflight_ExplainsUnavailableActionReasons
```


# Appendix D.21 — Existing Focused Test Inventory: `Ashfall.Core.Tests/CombatSaveRoundTripTests.cs`

### `Ashfall.Core.Tests/CombatSaveRoundTripTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 123; SHA-256: `84d1047ff4d8721849585d7cd533aae13077c8e73d20ffa6824eb3690cfdca0a`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CaptureRestore_PreservesFullState
JsonRoundTrip_ThroughPort_PreservesState
Migrate_ClampsAndDefaultsForeignSaves
DeterministicReplay_FromSameStateAndSeed
```


# Appendix D.22 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Combat/RealtimeFireCadenceTests.cs`

### `Ashfall.Core.Tests/Combat/RealtimeFireCadenceTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 176; SHA-256: `55d36ffe48e2afcefa599fdf49a6d021b7164c9cb58066f6c910511b3e6441f3`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
FireHeld_ConsumesAmmoOnCadence_Deterministic
FireCooldown_GatesShotsBelowWeaponRpm
Climb_BlocksRealtimeFire
AimTarget_UpdatesAimRadTowardHostile
EvaluateFire_AllowsActiveRealtimePhase
```


# Appendix D.23 — Existing Focused Test Inventory: `Ashfall.Core.Tests/ExpeditionVehicleSystemTests.cs`

### `Ashfall.Core.Tests/ExpeditionVehicleSystemTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 78; SHA-256: `cdee8fe5833317af3ceb2fa99d7735384e30026bf3e29ee7ef845536cf8da3c3`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.


# Appendix D.24 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Expeditions/Plan213VehicleArmorGradeTests.cs`

### `Ashfall.Core.Tests/Expeditions/Plan213VehicleArmorGradeTests.cs`

- Current test declarations: Fact=21, Theory=0, InlineData=0.
- File lines: 407; SHA-256: `cc7d9629d100b077da5aaa599e03ddec7f195b39f8a7d9a3975ca3aebaefc3c2`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
T01_RealCatalogLoadsFiveRowsAndContiguousTiers
T02_DefaultRowIsNeutral
T03ToT09_DataGateReportsEachRepresentativeViolation
T10_LegacyProfileAndWearRemainIdentical
T11_InstallStampsAndInsufficientBillDoesNotMutate
T12_ImmobilizedVehicleRefusesInstallAndReforge
T13_TerrainGateRefusesCoastalForCompositeAndUnsetResolverPasses
T14_ReplaceRefundsHalfOldScrapAndResetsIntegrity
T15_MitigationIsExactAndNeverImmunity
T16_AbsorptionSplitsChassisAndClampsToIntegrity
T17_EngineAndTransmissionUseFullBaseWear
T18_DepletedPlateLosesMitigationButKeepsMassPenalty
T19_ReforgeRestoresStampedPoolAndRejectsFullOrStock
T20_FoundryQualityStampsPoolButNotMitigation
T21_StampedPoolIsNotRecomputedAfterProviderChanges
T22_SaveRoundtripPreservesArmorStateMidWear
T23_LegacyRecordDefaultsToStock
T24_SameOperationsProduceIdenticalStateFingerprint
T25_ExistingModsComposeBeforeArmorAndRadiationIsSeparate
T26_GetArmorProfileIsTotalAndUnknownInstallRefuses
Soak_ThirtyDaysKeepsIntegrityBoundedAndWearMonotonic
```


# Appendix E.25 — Supporting Code Evidence: `Assets/Ashfall.Core/Combat/BallisticsWorkbenchSystem.cs`

### `Assets/Ashfall.Core/Combat/BallisticsWorkbenchSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 571 lines / 25477 bytes.
- SHA-256: `4ca13c88fe2f3c15c4b848658793d5ff89541af34ce9aab87487675df633810b`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=9; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum WeaponHeadspaceState
public sealed class BallisticsWorkbenchDefinition
public string ProfileId { get; set; } = string.Empty;
public string WeaponTag { get; set; } = string.Empty;
public float BaseDispersionMoa { get; set; } = 4f;
public float WearPerShotFactor { get; set; } = 0.1f;
public float CorrosiveAmmoWearModifier { get; set; } = 1.5f;
public float OverpressureWearModifier { get; set; } = 1.8f;
public float HeadspaceWarningThreshold { get; set; } = 0.55f;
public float HeadspaceFailureThreshold { get; set; } = 0.85f;
public float MaxCalibrationBonus { get; set; } = 0.2f;
public float MaxOpticBonus { get; set; } = 0.15f;
public string MaintenanceRecipeId { get; set; } = string.Empty;
public List<string> SupportedAmmoTags { get; set; } = new List<string>();
public sealed class BallisticsWorkbenchCatalog
public int SchemaVersion { get; set; } = 1;
public List<BallisticsWorkbenchDefinition> Profiles { get; set; } =
public sealed class WeaponBallisticsProfile
public string WeaponInstanceId = string.Empty;
public string ProfileId = string.Empty;
public float ThroatWearIndex;
public float HeadspaceIndex;
public float MuzzleVelocityVariance;
public float DispersionMoa;
public float CrownCondition = 1f;
public float RiflingCondition = 1f;
public float CalibrationQuality;
public float OpticQuality;
public WeaponHeadspaceState HeadspaceState;
public bool Inspected;
public bool CatastrophicFailureResolved;
public int LastServiceDay = -1;
public int TotalRounds;
public string LastAmmoBatchId = string.Empty;
public List<string> ResolvedFailureEventIds = new List<string>();
public sealed class CustomAmmoBatchState
public string BatchId = string.Empty;
public string AmmoItemId = string.Empty;
public float ChargeQuality;
public float ProjectileUniformity;
public float VelocityConsistency;
public float PressureRisk;
public string CreatorSurvivorId = string.Empty;
public int CreatedDay;
public sealed class BallisticsWorkbenchState
public string SystemId = BallisticsWorkbenchSystem.SystemId;
public int NextBatchSequence = 1;
public Dictionary<string, WeaponBallisticsProfile> Profiles = new Dictionary<string, WeaponBallisticsProfile>(StringComparer.Ordinal);
public List<CustomAmmoBatchState> AmmoBatches = new List<CustomAmmoBatchState>();
public sealed class WeaponCombatModifier
public float AccuracyMultiplier = 1f;
public float RangeMultiplier = 1f;
public float PenetrationMultiplier = 1f;
public float CriticalMultiplier = 1f;
public float MalfunctionMultiplier = 1f;
public float DispersionMoa = 0f;
public sealed class BallisticsFireResult
public bool Accepted;
public bool CatastrophicFailure;
public string FailureCode = string.Empty;
public WeaponHeadspaceState HeadspaceState;
public float DispersionMoa;
public sealed class BallisticsWorkbenchSystem
public const string SystemId = "ballistics_workbench";
public BallisticsWorkbenchState State => _state;
public IReadOnlyDictionary<string, BallisticsWorkbenchDefinition> Definitions => _definitions;
public IReadOnlyDictionary<string, WeaponBallisticsProfile> Profiles => _state.Profiles;
public IReadOnlyList<CustomAmmoBatchState> AmmoBatches => _state.AmmoBatches;
public event Action<WeaponBallisticsProfile>? OnProfileChanged;
public event Action<string>? OnCatastrophicFailure;
public void LoadCatalog(BallisticsWorkbenchCatalog? catalog) {
public void RegisterDefinition(BallisticsWorkbenchDefinition definition) {
public WeaponBallisticsProfile EnsureProfile(string weaponInstanceId, string profileId) {
public ActionResult Inspect(string weaponInstanceId, int day) {
public ActionResult Calibrate( string weaponInstanceId, float operatorSkill, float toolingCalibration, int day) {
public ActionResult AttachOptic(string weaponInstanceId, float opticQuality) {
public ActionResult Refurbish( string weaponInstanceId, IReadOnlyList<string>? parts, float serviceQuality, int day) {
public BallisticsFireResult RecordFiring( string weaponInstanceId, string eventId, int rounds, bool corrosiveAmmo, bool overpressureAmmo)
public CustomAmmoBatchState? CreateAmmoBatch( string ammoItemId, string creatorSurvivorId, float skill, float toolingQuality, int day,
public WeaponCombatModifier GetCombatModifier( string weaponInstanceId, string ammoBatchId = "") {
public void ApplyToCombatWeapon( WeaponInstanceState weapon, string ammoBatchId = "") {
public WeaponBallisticsProfile? FindProfile(string weaponInstanceId) {
public BallisticsWorkbenchState CaptureState() {
public void RestoreState(BallisticsWorkbenchState? saved) {
public static class BallisticsWorkbenchCatalogLoader
public const string FileName = "ballistics_workbench_catalog.json";
public static BallisticsWorkbenchCatalog? Load( string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
```


# Appendix E.26 — Supporting Code Evidence: `Assets/Ashfall.Core/Combat/BallisticShieldEngine.cs`

### `Assets/Ashfall.Core/Combat/BallisticShieldEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 436 lines / 17133 bytes.
- SHA-256: `e38d943b45b45c1cc017dbcccce2af9769296df8803f27b46fd56b90bef61213`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=26; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum ShieldStance
public enum ShieldDamageResultType
public sealed class BallisticShieldDef
public string shield_id = string.Empty;
public string display_name = string.Empty;
public float coverage_arc_deg = 90.0f;
public float frontal_block_rating = 0.70f;
public float integrity_max = 80.0f;
public float viewport_clarity = 0.90f;
public float stamina_cost_per_tick = 2.5f;
public bool anchor_supported;
public float movement_multiplier = 0.80f;
public float suppression_resistance = 0.55f;
public List<string> tags = new List<string>();
public sealed class BallisticShieldCatalog
public int schema_version = 1;
public List<BallisticShieldDef> shields = new List<BallisticShieldDef>();
public sealed class BallisticShieldState
public string equippedShieldId = string.Empty;
public ShieldStance stance = ShieldStance.Stowed;
public float currentIntegrity;
public float viewportIntegrity = 1.0f;
public bool isAnchored;
public int anchorSpikesRemaining = 4;
public float staminaStrain;
public int phalanxLinkedCount;
public int totalShotsBlocked;
public float totalDamageAbsorbed;
public sealed class ShieldBlockResult
public bool success;
public float absorbedDamage;
public float penetratingDamage;
public ShieldDamageResultType resultType;
public float remainingIntegrity;
public bool shattered;
public static class BallisticShieldCatalogLoader
public const string DefaultFileName = "ballistic_shield_catalog.json";
public static BallisticShieldCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null) {
public sealed class BallisticShieldEngine
public const string SystemId = "ballistic_shield";
public const string ItemAnchorSpikes = "item_hardened_ground_anchor_spikes";
public const string ItemViewportGlass = "item_laminated_ballistic_viewport_glass";
public event Action<BallisticShieldState>? OnStateChanged;
public event Action<ShieldStance>? OnStanceChanged;
public event Action<ShieldBlockResult>? OnDamageBlocked;
public event Action<string>? OnShieldBroken;
public event Action<float>? OnViewportCracked;
public BallisticShieldState State => _state;
public BallisticShieldCatalog Catalog => _catalog;
public void LoadCatalog(BallisticShieldCatalog catalog) {
public ActionResult EquipShield(string shieldId) {
public ActionResult SetStance(ShieldStance newStance) {
public ActionResult AnchorToGround() {
public ActionResult Unanchor() {
public ActionResult JoinPhalanx(int allyCount) {
public ShieldBlockResult InterceptDamage(float incomingDamage, float hitAngleDeg, bool isPiercing = false) {
public ActionResult RepairShield(float amount) {
public ActionResult ReplaceViewport() {
public void TickCombatStamina(float staminaAvailable) {
public BallisticShieldState CaptureState() {
public void RestoreState(BallisticShieldState? state) {
```


# Appendix E.27 — Supporting Code Evidence: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Persistence.cs`

### `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Persistence.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 458 lines / 21061 bytes.
- SHA-256: `976900f9a0545ba959e38530abb0c6addafd71956995542b20c21054c8e1c498`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class TacticalCombatSystem
public CombatSnapshot BuildSnapshot() {
public CombatState CaptureState() {
public void RestoreState(CombatState saved) {
public static CombatState Migrate(CombatState s) {
public static CombatAftermath? CloneAftermath(CombatAftermath? src) {
public static List<string> CloneIncidentIds(List<string>? source) {
public static List<CombatFactionConsequence> CloneFactionConsequences( List<CombatFactionConsequence>? source) {
public static List<BoundWeaponConditionEntry> CloneBoundWeaponConditions(List<BoundWeaponConditionEntry>? src) {
```


# Appendix E.28 — Supporting Code Evidence: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs`

### `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 583 lines / 26404 bytes.
- SHA-256: `70dc102c1569f7bc447dfba3592a71373f5f3a76bee1366b489c872e9aa6db76`.
- Architecture signals: seeded references=11; save/restore symbols=0; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class TacticalCombatSystem
public CombatActionResult SetStance(TacticalStance stance, string? subjectSurvivorId = null) {
public CombatActionResult PlayerFire(string targetId, ISeededRng rng, string? subjectId = null, float motionAccuracyScale = 1f) {
public CombatActionResult PlayerSuppress(ISeededRng rng) {
public CombatActionResult PlayerClearJam(string survivorIdOrCombatantId, ISeededRng rng) {
public CombatActionResult PlayerReload(string survivorIdOrCombatantId) {
public CommandPreview PreviewPlayerFieldRepair(string survivorIdOrCombatantId, long stateVersion = 0) {
public CommandResult ExecutePlayerFieldRepair(string survivorIdOrCombatantId, long expectedStateVersion = 0, long currentStateVersion = 0) {
public CombatActionResult PlayerFieldRepair(string survivorIdOrCombatantId, ISeededRng rng) {
public CombatActionResult PlayerMoveLane(string survivorIdOrCombatantId, CombatLane lane, ISeededRng rng) {
public CombatActionResult PlayerDeployTrap(ISeededRng rng) {
public CombatActionResult PlayerDecontaminate(ISeededRng rng) {
public CombatActionResult PlayerBandage(string rescuerId, string downedId, ISeededRng rng) {
public CombatActionResult PlayerRetreat(ISeededRng rng) {
public CombatActionResult PlayerLastStand(string survivorIdOrCombatantId, ISeededRng rng) {
public static string Describe(BallisticOutcome o) {
```


# Appendix E.29 — Supporting Code Evidence: `src/Main.Plans74_77.cs`

### `src/Main.Plans74_77.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 432 lines / 18567 bytes.
- SHA-256: `c5e104747ae4b5267a503edafb32c8eb4baa4ac0c2d5cf39cf3a075ae663f405`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
```


# Appendix G.30 — Supporting Regression Evidence: `Ashfall.Core.Tests/Plan54CombatCatalogTests.cs`

### `Ashfall.Core.Tests/Plan54CombatCatalogTests.cs`

- Current test declarations: Fact=15, Theory=0, InlineData=0.
- File lines: 339; SHA-256: `678ce466638c151d1b14d5bea4679b8a01603adc967d38ce08601b28be4eec45`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_Loads20Weapons
Catalog_PreservesThePlansOriginalFive
Catalog_PreservesAll15BaselineWeaponsWithCalibers
Catalog_RegistersThe5Plan54Weapons
Catalog_AllCalibersResolve
Catalog_NoTwoWeaponsAreStatClones
Catalog_OrphanCalibersAreDocumented
CombatantCatalog_Loads12Definitions
CombatantCatalog_PreservesAll10BaselineDefinitions
CombatantCatalog_RegistersThe2Plan54Archetypes
CombatantFactory_SpawnsPlan54ArchetypesWithCatalogTraits
Encounter_NewWeaponFires_NewEnemiesSpawnFromCatalog
Encounter_Plan54CombatReplaysIdentically
Encounter_AllPlan54WeaponsResolveAndFire
SaveRoundTrip_Plan54WeaponAndEnemiesSuriveReload
```


# Appendix G.31 — Supporting Regression Evidence: `Ashfall.Core.Tests/CombatWeaponConditionTests.cs`

### `Ashfall.Core.Tests/CombatWeaponConditionTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 146; SHA-256: `ebc05eec5113fde0abeb5dcc23ee0935b79cf614d537abe7142e193807828d78`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
JamChance_RisesAsConditionDrops
JamChance_IsBoundedToOne
JamRoll_MarksWeaponJammed
ClearingJam_TakesTicks
FieldRepair_ConsumesScrap_AndRestores
FieldRepair_FailsWithoutScrap
AshDunes_JamAndDegradeFirearm
PipeWithMilitaryAmmo_CanBurst
MilitaryAmmoInRifle_DoesNotBurst
ScrapCost_ScalesWithDamage
```


# Appendix G.32 — Supporting Regression Evidence: `Ashfall.Core.Tests/CombatSystemTests.cs`

### `Ashfall.Core.Tests/CombatSystemTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 227; SHA-256: `ab911032b1cde17766c75b52e590d9b5fd9cbbed3b7feb38d5315f4cd57f1564`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsFromDataAuthority_WithCanonicalIds
Catalog_SeedDefaults_LoadsTheJsonAuthority
HeadlessDemo_AllChecksPass
SameSeed_ReproducesIdenticalCombat
Save_RoundTrips_ThroughSerializer
Migrate_ClampsLegacyShape
CombatCatalog_IntroducesNoDataIntegrityErrors
EveryWeaponCaliber_ResolvesToARealInventoryItem
```


# Appendix G.33 — Supporting Regression Evidence: `Ashfall.Core.Tests/CombatBallisticsTests.cs`

### `Ashfall.Core.Tests/CombatBallisticsTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 191; SHA-256: `56a14ecea89e28332b617a962ffb013da01250f931138a6ad012b30705135f8e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
DirectHit_DealsFullEnergy
Missed_OnZeroAccuracy
Blocked_ByCover
CoverPenetrates_AndReducesDamage
Barrier_Blocks
Ricochet_RedirectsEnergyToSecondary
RicochetChains_AreBounded
Armor_CanStop
```


# Appendix H.34 — Supporting Authority Document: `docs/combat/PLAN10_COMPLETION_REPORT.md`

### `docs/combat/PLAN10_COMPLETION_REPORT.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 4242 lines / 300384 bytes.
- SHA-256: `ccf8c9ec38400a7b89b7ae0ab317c54cf6c0cd38b10caee274a146d9a59db7e8`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=157; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class Plan10SubsystemAuditRecord
public string SubsystemId { get; }
public string WorkstreamTask { get; }
public int DeliveredCount { get; }
public string CatalogPath { get; }
public bool IsCertified { get; }
public sealed class Plan10CompletionVerificationOrchestrator
public IReadOnlyDictionary<string, Plan10SubsystemAuditRecord> Subsystems =>
public void RegisterSubsystem(string id, string task, int count, string path, bool certified) {
public bool ValidateUnifiedPlan10Completion(out string summary) {
public string ComputeUnifiedCertificationDigest() {
public sealed class Plan10CompletionReportVerificationTests
public void Test_001_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_002_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_003_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_004_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_005_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_006_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_007_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_008_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_009_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_010_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_011_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_012_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_013_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_014_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_015_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_016_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_017_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_018_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_019_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_020_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_021_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_022_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_023_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_024_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_025_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_026_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_027_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_028_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_029_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_030_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_031_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_032_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_033_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_034_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_035_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_036_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_037_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_038_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_039_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_040_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_041_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_042_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_043_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_044_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_045_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_046_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_047_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_048_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_049_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_050_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_051_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_052_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_053_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_054_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_055_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_056_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_057_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_058_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_059_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_060_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_061_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_062_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_063_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_064_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_065_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_066_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_067_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_068_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_069_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_070_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_071_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_072_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_073_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_074_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_075_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_076_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_077_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_078_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_079_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_080_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_081_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_082_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_083_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_084_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_085_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_086_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_087_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_088_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_089_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_090_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_091_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_092_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_093_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_094_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_095_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_096_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_097_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_098_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_099_Plan10_CompletionReport_SubsystemAudit_Verification() {
public void Test_100_Plan10_CompletionReport_SubsystemAudit_Verification() {
```


# Appendix H.35 — Supporting Authority Document: `docs/EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`

### `docs/EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 23 lines / 1439 bytes.
- SHA-256: `e1aa8c14f3a76331136d049dcdba1efa8fcab499aaa97b774f0ddb77b70efd5c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.36 — Supporting Authority Document: `docs/combat/COMBAT_ENCOUNTER_COVERAGE.md`

### `docs/combat/COMBAT_ENCOUNTER_COVERAGE.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 4706 lines / 267407 bytes.
- SHA-256: `f5c53b9eb343cb026899e11fd2da994e698d11d4ae1dba40891ac9320bd447a0`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=151; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum TacticalLane
public enum CoverTier
public sealed class CombatantInstance
public string CombatantId { get; }
public TacticalLane CurrentLane { get; set; }
public float HealthPoints { get; set; }
public float MaxHealthPoints { get; }
public float SuppressionLevel { get; set; }
public float Morale { get; set; }
public bool IsSurrendered { get; set; }
public void ApplySuppression(float volume) {
public sealed class TacticalEncounterOrchestrator
public IReadOnlyList<CombatantInstance> ActiveCombatants => _activeCombatants.AsReadOnly();
public void SpawnCombatant(string id, TacticalLane lane, float health, float morale) {
public void EvaluateSquadMoraleAndSurrender(float surrenderThreshold) {
public string ComputeTacticalStateDigest() {
public sealed class CombatEncounterCoverageVerificationTests
public void Test_001_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_002_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_003_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_004_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_005_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_006_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_007_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_008_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_009_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_010_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_011_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_012_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_013_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_014_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_015_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_016_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_017_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_018_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_019_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_020_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_021_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_022_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_023_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_024_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_025_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_026_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_027_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_028_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_029_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_030_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_031_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_032_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_033_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_034_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_035_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_036_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_037_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_038_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_039_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_040_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_041_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_042_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_043_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_044_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_045_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_046_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_047_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_048_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_049_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_050_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_051_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_052_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_053_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_054_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_055_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_056_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_057_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_058_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_059_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_060_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_061_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_062_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_063_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_064_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_065_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_066_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_067_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_068_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_069_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_070_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_071_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_072_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_073_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_074_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_075_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_076_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_077_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_078_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_079_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_080_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_081_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_082_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_083_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_084_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_085_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_086_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_087_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_088_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_089_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_090_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_091_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_092_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_093_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_094_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_095_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_096_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_097_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_098_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_099_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
public void Test_100_CombatEncounter_LaneGeometry_And_Suppression_Verification() {
```


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| tactical/realtime encounter state and commands | TacticalCombatSystem | weapon, ammo, material and combatant definitions | Combat catalog | Owner emits/reads a typed fact; no mirror state. |
| tactical/realtime encounter state and commands | TacticalCombatSystem | definition-to-runtime conversion | CombatantFactory | Owner emits/reads a typed fact; no mirror state. |
| tactical/realtime encounter state and commands | TacticalCombatSystem | vehicle condition, garage, armor and expedition preparation | Expedition vehicle owners | Owner emits/reads a typed fact; no mirror state. |
| tactical/realtime encounter state and commands | TacticalCombatSystem | species discovery and player knowledge | BestiarySystem | Owner emits/reads a typed fact; no mirror state. |
| weapon, ammo, material and combatant definitions | Combat catalog | tactical/realtime encounter state and commands | TacticalCombatSystem | Owner emits/reads a typed fact; no mirror state. |
| weapon, ammo, material and combatant definitions | Combat catalog | definition-to-runtime conversion | CombatantFactory | Owner emits/reads a typed fact; no mirror state. |
| weapon, ammo, material and combatant definitions | Combat catalog | vehicle condition, garage, armor and expedition preparation | Expedition vehicle owners | Owner emits/reads a typed fact; no mirror state. |
| weapon, ammo, material and combatant definitions | Combat catalog | species discovery and player knowledge | BestiarySystem | Owner emits/reads a typed fact; no mirror state. |
| definition-to-runtime conversion | CombatantFactory | tactical/realtime encounter state and commands | TacticalCombatSystem | Owner emits/reads a typed fact; no mirror state. |
| definition-to-runtime conversion | CombatantFactory | weapon, ammo, material and combatant definitions | Combat catalog | Owner emits/reads a typed fact; no mirror state. |
| definition-to-runtime conversion | CombatantFactory | vehicle condition, garage, armor and expedition preparation | Expedition vehicle owners | Owner emits/reads a typed fact; no mirror state. |
| definition-to-runtime conversion | CombatantFactory | species discovery and player knowledge | BestiarySystem | Owner emits/reads a typed fact; no mirror state. |
| vehicle condition, garage, armor and expedition preparation | Expedition vehicle owners | tactical/realtime encounter state and commands | TacticalCombatSystem | Owner emits/reads a typed fact; no mirror state. |
| vehicle condition, garage, armor and expedition preparation | Expedition vehicle owners | weapon, ammo, material and combatant definitions | Combat catalog | Owner emits/reads a typed fact; no mirror state. |
| vehicle condition, garage, armor and expedition preparation | Expedition vehicle owners | definition-to-runtime conversion | CombatantFactory | Owner emits/reads a typed fact; no mirror state. |
| vehicle condition, garage, armor and expedition preparation | Expedition vehicle owners | species discovery and player knowledge | BestiarySystem | Owner emits/reads a typed fact; no mirror state. |
| species discovery and player knowledge | BestiarySystem | tactical/realtime encounter state and commands | TacticalCombatSystem | Owner emits/reads a typed fact; no mirror state. |
| species discovery and player knowledge | BestiarySystem | weapon, ammo, material and combatant definitions | Combat catalog | Owner emits/reads a typed fact; no mirror state. |
| species discovery and player knowledge | BestiarySystem | definition-to-runtime conversion | CombatantFactory | Owner emits/reads a typed fact; no mirror state. |
| species discovery and player knowledge | BestiarySystem | vehicle condition, garage, armor and expedition preparation | Expedition vehicle owners | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Treat the original plan as complete and maintain the current architecture map. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Audit all combat entry points for canonical TacticalCombatSystem use. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Verify vehicle/armor/dive consequences route to medical, dose and expedition owners. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Add balance and reachability evidence for newly added realtime arenas and weapon cadence fields. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Maritime/MaritimeExplorationSystem.cs`

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


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/VehicleArmorGradeCatalog.cs`

### `Assets/Ashfall.Core/Expeditions/VehicleArmorGradeCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 154 lines / 5802 bytes.
- SHA-256: `90350ba205dd3cba31fd937e322ba7bcf39c82b810f4691d0f179f45e4192693`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class VehicleArmorGradeCost
public string item_id = string.Empty;
public int amount;
public sealed class VehicleArmorGradeDefinition
public string id = string.Empty;
public string display_name = string.Empty;
public string description = string.Empty;
public int tier;
public bool is_default;
public int mitigation_permille;
public int wear_absorption_permille;
public int integrity_pool_permille;
public float speed_multiplier_delta;
public float fuel_consumption_multiplier = 1.0f;
public List<string> compatible_terrain_types = new List<string>();
public List<VehicleArmorGradeCost> install_cost = new List<VehicleArmorGradeCost>();
public int install_labor_ticks;
public List<VehicleArmorGradeCost> reforge_cost = new List<VehicleArmorGradeCost>();
public List<string> tags = new List<string>();
public sealed class VehicleArmorGradeCatalog
public int schema_version = 1;
public string default_grade_id = string.Empty;
public List<VehicleArmorGradeDefinition> grades = new List<VehicleArmorGradeDefinition>();
public sealed class VehicleArmorProfile
public string GradeId = string.Empty;
public string DisplayName = string.Empty;
public int Tier;
public bool IsDefault;
public int MitigationPermille;
public int WearAbsorptionPermille;
public int IntegrityPermille;
public int IntegrityMaxPermille;
public string MaterialProfileId = string.Empty;
public string Purity = "Standard";
public string ConditionBand = "none";
public float SpeedMultiplierDelta;
public float FuelConsumptionMultiplier = 1f;
public sealed class VehicleArmorGradeLoadResult
public VehicleArmorGradeCatalog? Catalog { get; internal set; }
public List<string> Errors { get; } = new List<string>();
public bool HasErrors => Errors.Count > 0;
public static class VehicleArmorGradeCatalogLoader
public const string FileName = "vehicle_armor_grades.json";
public const int CurrentSchemaVersion = 1;
public static VehicleArmorGradeLoadResult Load(string dataDir, IFileIO files, IJsonSerializer json) {
public static VehicleArmorGradeLoadResult LoadJson(string raw, IJsonSerializer json) {
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Maritime/DiveSiteCatalog.cs`

### `Assets/Ashfall.Core/Maritime/DiveSiteCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 135 lines / 5514 bytes.
- SHA-256: `c07019d3c727ffe07458698a2e77b269114113668564bb12efefcd05fa072dc6`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class DiveSiteRoom
public string room_type = string.Empty;
public int hazard_level;
public float search_difficulty;
public class DiveSiteDefinition
public string site_id = string.Empty;
public string name = string.Empty;
public int oxygen_budget_ticks;
public float base_noise_floor;
public string keeper_thread_id = string.Empty;
public List<DiveSiteRoom> rooms = new List<DiveSiteRoom>();
public string location_id = string.Empty;
public string required_item_id = string.Empty;
public int required_item_count = 1;
public string contamination_key = string.Empty;
public List<SafeDefinition> safes = new List<SafeDefinition>();
public List<VariableLootNode> loot_table = new List<VariableLootNode>();
public string discovery = string.Empty;
public string tide_window = "any";
public static class DiveSiteTideWindows
public static TideWindow Parse(string? raw) => raw switch
public class DiveSiteContainer
public int schema_version;
public List<DiveSiteDefinition> dive_sites = new List<DiveSiteDefinition>();
public static class DiveSiteCatalogLoader
public const string FileName = "dive_sites.json";
public static DiveSiteContainer Load( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static DiveSiteDefinition? FindById(DiveSiteContainer container, string siteId) {
```


# Appendix Q.571 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Combat/CombatTypes.cs`

### `Assets/Ashfall.Core/Combat/CombatTypes.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 458 lines / 18326 bytes.
- SHA-256: `12ece45da14bdf6d0d7648998528cc4d0c86eea3d1e34010ca3a8d2fb7e828bf`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum CombatPhase
public enum CombatMotionMode
public enum CombatLane
public enum TacticalStance
public enum BallisticResult
public enum BallisticReason
public class CombatLootEntry
public string itemId = string.Empty;
public int quantity = 1;
public float weightKg = 1f;
public class CombatantState
public string Id = string.Empty;
public string Name = string.Empty;
public string SurvivorId = string.Empty; // for players: the survivor the host writes back to
public bool IsPlayer;
public string FactionId = string.Empty;
public int Lane = (int)CombatLane.Center;
public float Health = 100f;
public float MaxHealth = 100f;
public float ArmorRating; // 0..1 damage reduction from worn armor
public float CoverRating; // 0..1 chance this unit is behind cover
public bool IsDowned;
public int BleedTurnsRemaining;
public bool IsPinned;
public int PinnedTurnsRemaining;
public bool IsLastStand;
public string WeaponInstanceId = string.Empty;
public bool HasFled;
public string AiStancePreference = "HoldPosition"; // TacticalStance name
public string AiSpecialMove = "None";            // None|Burrow|Flank|Spore|Charge|SuppressiveFire|TacticalRetreat
public float AiAccuracyMod = 1f;                 // multiplier on outgoing accuracy
public float AiDamageMod = 1f;                   // multiplier on outgoing damage
public float SurrenderThreshold = -1f;           // -1 = never; 0..1 = open path
public float FleeThreshold = -1f;                // -1 = never; 0..1 = open path
public string CatalogId = string.Empty;          // combatant_* id when spawned via factory; "" for legacy rows
public float PosX;
public float PosY;
public float VelX;
public float VelY;
public float FacingRad;
public float AimRad;
public int MotionMode = (int)CombatMotionMode.Idle;
public float Stamina01 = 1f;
public float FireCooldown;
public float AiThinkCooldown;
public string AiBehaviorPhase = string.Empty;
public float AiPhaseTimer;
public float ExtractProgress01;
public bool PoseSeeded;
public class BarrierState
public string Id = string.Empty;
public int Lane = (int)CombatLane.Center;
public bool IsPlayer;
public string MaterialId = string.Empty;
public float IntegrityPct = 100f;
public float ArmorRating; // flat damage absorbed while intact
public string ObstacleProfileId = string.Empty;
public string ActiveBreachToolId = string.Empty;
public string BreachPhase = BreachPhaseIds.Available;
public int BreachSetupTicksRemaining;
public int BreachClearTicksRemaining;
public int BreachClearTicksTotal;
public float BreachProgress01;
public float PathBlocking;
public float CoverContribution;
public bool BreachDestroysCover;
public static class BreachPhaseIds
public const string Available = "Available";
public const string SettingUp = "SettingUp";
public const string Clearing = "Clearing";
public const string Cleared = "Cleared";
public const string Interrupted = "Interrupted";
public const string Failed = "Failed";
public const string Abandoned = "Abandoned";
public class WeaponInstanceState
public string InstanceId = string.Empty;
public string WeaponId = string.Empty;
public string OwnerSurvivorId = string.Empty;
public string OwnerCombatantId = string.Empty;
public float ConditionPct = 1f;
public bool IsJammed;
public int JamClearTicksRemaining;
public int JamsSurvived;
public int ShotsFired;
public int BurstCount;
public float CachedJamChance;   // same value the sim uses for its jam roll
public float AshFoul;            // persistent environmental fouling (ash/contamination)
public string AmmoId = string.Empty;
public int AmmoRemaining = 0;
public int MagazineCapacity;     // item 6: max rounds in one reload
public int ScrapRepairCost;     // exposed to the UI
public float BallisticsAccuracyMultiplier = 1f;
public float BallisticsRangeMultiplier = 1f;
public float BallisticsPenetrationMultiplier = 1f;
public float BallisticsCriticalMultiplier = 1f;
public float BallisticsMalfunctionMultiplier = 1f;
public class CombatEvent
public string Kind = string.Empty;
public int Day;
public int Turn;
public string SubjectId = string.Empty;
public string TargetId = string.Empty;
public string Detail = string.Empty;
public float Value;
public class CombatWeaponWearRecord
public string InstanceId = string.Empty;
public string WeaponId = string.Empty;
public string OwnerSurvivorId = string.Empty;
public float StartConditionPct = 1f;
public float FinalConditionPct = 1f;
public float WearDeltaPct = 0f;
public class CombatAmmoSpentRecord
public string AmmoId = string.Empty;
public int RoundsSpent = 0;
public class CombatAftermath
public string ResolutionId = string.Empty;
public string EncounterId = string.Empty;
public string Outcome = string.Empty;
public List<string> SurvivorInjuries = new List<string>();
public List<string> SurvivorDeaths = new List<string>();
public List<CombatWeaponWearRecord> WeaponWear = new List<CombatWeaponWearRecord>();
public List<CombatAmmoSpentRecord> AmmoSpent = new List<CombatAmmoSpentRecord>();
public List<CombatLootEntry> LootConsequences = new List<CombatLootEntry>();
public float MoraleConsequences = 0f;
public bool IsApplied;
public class BoundWeaponConditionEntry
public string instanceId = string.Empty;
public float conditionPct = 1f;
public readonly struct ActionPreflight
public bool CanExecute { get; }
public string Reason { get; }
public static ActionPreflight Ok => new ActionPreflight(true, string.Empty);
public static ActionPreflight Blocked(string reason) => new ActionPreflight(false, reason);
public class CombatState
public const int CurrentSaveVersion = 5;
public string SystemId = TacticalCombatSystem.SystemId;
public int SaveVersion = CurrentSaveVersion;
public string EncounterId = string.Empty;
public string ExpeditionId = string.Empty;
public string LocationId = string.Empty;
public string LocationName = string.Empty;
public int Day = 1;
public int Seed;
public int Turn = 1;
public int Phase = (int)CombatPhase.Setup;
public string PlayerStance = TacticalCombatSystem.StanceId(TacticalStance.HoldPosition);
public int RoundNumber = 0;
public bool Resolved;
public string OutcomeText = string.Empty;
public string ResolutionId = string.Empty;
public bool IsSelfDefense;
public CombatAftermath? Aftermath;
public List<string> AppliedFactionConsequenceIds = new List<string>();
public List<CombatFactionConsequence> FactionConsequences = new List<CombatFactionConsequence>();
public List<BoundWeaponConditionEntry> BoundWeaponConditions = new List<BoundWeaponConditionEntry>();
public List<CombatantState> Combatants = new List<CombatantState>();
public List<WeaponInstanceState> Weapons = new List<WeaponInstanceState>();
public List<BarrierState> Barriers = new List<BarrierState>();
public List<CombatEvent> Events = new List<CombatEvent>();
public List<CombatLootEntry> Loot = new List<CombatLootEntry>();
public bool RealtimeActive;
public float SimTime;
public int SimTick;
public string ArenaId = string.Empty;
public float GetBoundWeaponStartCondition(string instanceId, float defaultVal = 1f) {
public void SetBoundWeaponStartCondition(string instanceId, float condition) {
public class CombatInputFrame
public string SubjectId = string.Empty;
public float MoveX;
public float MoveY;
public bool Sprint;
public bool Climb;
public bool Brace;
public bool FireHeld;
public bool FirePressed;
public bool Reload;
public bool Suppress;
public bool Flee;
public float AimRad;
public string AimTargetId = string.Empty;
```


# Appendix Q.572 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix Q.573 — Additional Current Architecture Evidence: `src/Host/BallisticShieldSaveStore.cs`

### `src/Host/BallisticShieldSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 36 lines / 1868 bytes.
- SHA-256: `7bd77fb2ff372d6410e359eb6fbfb539939c9341c3df4abf5c2774029a634e84`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class BallisticShieldSaveStore
public const string FileName = "ballistic_shield_save.json";
public const string SectionName = "ballistic_shield";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(BallisticShieldState state) => s_store.CaptureBare(state);
public static BallisticShieldState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(BallisticShieldState state) => s_store.CaptureBare(state);
public static BallisticShieldState? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(BallisticShieldState state) => s_store.TrySave(state);
public static BallisticShieldState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(BallisticShieldState state) => s_store.CapturePersisted(state);
```


# Appendix Q.574 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Combat/WeaponConditionSystem.cs`

### `Assets/Ashfall.Core/Combat/WeaponConditionSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 319 lines / 15293 bytes.
- SHA-256: `a870ad96a12001a9821b7ac883fe2606e0e3241333b4cfe4b8abf86f312561ba`.
- Architecture signals: seeded references=3; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CombatHostPorts
public Func<string, float, float> DamageSurvivor { get; }
public Func<string, float, float> HealSurvivor { get; }
public Action<string, float> ApplyMoraleDelta { get; }
public Func<string, int, int> ConsumeAmmo { get; }
public Func<string, int, bool> ConsumeItem { get; }
public Action<CombatLootEntry> GrantLoot { get; }
public Action<string> MarkCombatSurvived { get; }
public Action<string, string, float> RaiseTrauma { get; }
public Action<float>? EmitBreachNoise { get; }
public Action<string, float>? ApplyBreachToolWear { get; }
public IReadOnlyList<string> UnboundRequiredEffects => _unboundRequired;
public static CombatHostPorts NoOp() =>
public static readonly Func<string, float, float> NoOpDamageSurvivor = (_, d) => d;
public static readonly Func<string, float, float> NoOpHealSurvivor = (_, h) => h;
public static readonly Action<string, float> NoOpApplyMoraleDelta = (_, __) => { };
public class WeaponConditionSystem
public const float Pristine = 1f;
public const float Ruined = 0f;
public const int DefaultJamClearTicks = 5;
public const int BurstJamBaseTicks = 1; // pipe + military ammo burst failure needs one clear
public const string ScrapMaterialId = "scrap_metal";
public static float ComputeJamChance(WeaponInstanceState weapon) {
public static float ComputeDegradePerBurst(WeaponInstanceState weapon) {
public static bool TryJammed(WeaponInstanceState weapon, ISeededRng rng) {
public static float Degrade(WeaponInstanceState weapon, float amount) {
public static float ExposeToAsh(WeaponInstanceState weapon, float severity = 1f) {
public static bool TryWeaponBurst(WeaponInstanceState weapon, ISeededRng rng) {
public static int GetScrapRepairCost(WeaponInstanceState weapon) {
public bool TryFieldRepair(WeaponInstanceState weapon, CombatHostPorts ports, Func<string, int, bool>? consume = null) {
public static bool TickJamClear(WeaponInstanceState weapon, int ticksToClear) {
public static void ClearJam(WeaponInstanceState weapon) {
```


# Appendix Q.575 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs`

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


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.
