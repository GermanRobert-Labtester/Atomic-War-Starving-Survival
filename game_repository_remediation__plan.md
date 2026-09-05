# ASHFALL Game Repository Remediation Plan


**File:** `game_repository_remediation__plan.md`
**Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival`
**Audited branch:** `main`
**Pinned current snapshot:** `9b4985d0122d707c31f6078050df5877b69b607b`
**Prepared:** 2026-09-04
**Primary objective:** make the repository materially safer for flagship beta by removing or quarantining dead code, wiring intended systems end-to-end, eliminating false-positive quality gates, reducing repository bloat, and proving that player-facing features are live rather than merely present.


---


## 1. Executive decision


ASHFALL does not primarily have a “not enough systems” problem. It has a **liveness, ownership, and evidence problem**.


The repository already contains large amounts of Core logic, authored data, UI, tests, art, audio, and planning material. Multiple existing forensic audits independently converge on the same failure pattern:


> a component, catalog, panel, or method can exist, compile, save, and pass isolated tests while no production path ever calls it.


For beta remediation, the rule should therefore be:


**Do not count a feature as shipped until a player action or deterministic simulation path can prove the full chain:**


`authoritative data -> loader -> canonical system -> host composition -> producer/action -> state transition -> save -> UI/audio/art feedback -> reload -> observable effect`


Anything that fails the chain receives exactly one disposition:


- `REMOVE` — delete dead code/artifact when the feature is not a beta requirement.
- `REMOVE_FROM_SHIP` — keep prototype/source temporarily, but make it unreachable and exclude from shipped presentation.
- `CONSOLIDATE` — merge duplicate authorities or parallel implementations.
- `WIRE` — retain because it is an intended beta capability and connect it end-to-end.
- `DEFER` — keep only with an explicit owner, activation condition, and exclusion from beta claims.
- `FIX_GATE` — repair a test/tool whose “green” result overstates runtime health.


### Flagship-beta engineering goal


Before beta freeze:


- 0 critical or blocker defects.
- 0 compiler errors.
- 0 unexplained compiler/analyzer warnings.
- 0 failed tests.
- 0 critical catalogs with no consumer.
- 0 player-routable panels with no authoritative backing state.
- 0 required integration seams with no producer/consumer.
- 0 required save sections lacking round-trip and corruption tests.
- 0 fallback-only assets counted as strict asset success.
- 0 dead beta code left reachable “just in case”.
- Every deliberate deferral has a machine-readable disposition and expiry/activation rule.


---


# 2. Scope and evidence model


This plan uses the repository's existing forensic material and re-validates selected high-value findings against the pinned current `main` snapshot.


### Primary audit sources


- `docs/debug/10LOOP_UNWIRED_CODE_AUDIT.md`
- `docs/debug/10LOOP_player_ui_ux_BUG_AUDIT.md`
- `Next-steps-plans/Wave5_Continuity_Audit_INDEX.md`
- `Next-steps-plans/Wave6_Continuity_Audit_INDEX.md`
- `Next-steps-plans/Wave7_Continuity_Audit_INDEX.md`
- `Next-steps-plans/Wave8_Continuity_Audit_INDEX.md`
- `Next-steps-plans/Wave9_Continuity_Audit_INDEX.md`
- `Ashfall.Core.Tests/Tooling/LoaderWiringGateTests.cs`
- `AGENTS.md`
- direct current-commit code-search checks at `9b4985d...`


### Evidence confidence tags used below


- **CURRENT-REVALIDATED** — searched against `9b4985d...` during this remediation audit.
- **AUDIT-CARRYFORWARD** — confirmed by a recent repo forensic audit but not individually runtime-executed in this session; first remediation step is to re-run/reproduce it before modification.
- **DESIGNED-DEFERRED** — zero production reach is intentional and documented; not a deletion defect.


This distinction matters because the repository is changing rapidly. A stale audit should never become a deletion script.


---


# 3. Dead / unwired code disposition matrix


This is the most important cleanup table. It deliberately separates **dead** from **unwired but valuable**.


| Candidate | Current evidence | Classification | Beta disposition | Delete now? |
|---|---|---|---|---|
| `AtmosphereCatalogLoader` + `AtmosphereTextSystem` | Current search finds source, tests/docs/audit, but no production consumer | zero-reach feature island | `CONSOLIDATE` with Environmental Text if environmental flavor is a beta pillar; otherwise `REMOVE_FROM_SHIP` and archive data | **Conditional** |
| `EnvironmentalTextCatalogLoader` + `EnvironmentalTextSystem` | Current search finds only system/loader/audit/gate | zero-reach feature island; overlaps Atmosphere shape | consolidate into one environmental flavor pipeline or cut | **Conditional** |
| `HoldfastNpcCatalogLoader` + `HoldfastNpcCatalog` | Current search returns catalog + audit + allowlist, no production caller | genuinely unwired | if Holdfast NPC loops are beta-critical, `WIRE`; otherwise remove loader/data from beta package | **Conditional** |
| `CollectibleCatalogLoader` + `CollectibleCatalog` | Current search returns source + tests + audit + allowlist only; 40 collectibles were previously recorded | genuinely unwired | likely `REMOVE_FROM_SHIP` for beta unless collectibles have a live discovery/reward surface | **Yes, if not beta scope** |
| `DebtTemplateCatalogLoader` | **Now has production callers** via debt/store/session paths | **NOT DEAD**; stale allowlist/gate classification | keep code; remove stale allowlist entry and repair gate detection | **No** |
| `SkyLayerArmorCatalogLoader` + `SkyLayerArmorSystem` | documented Expansion 11 activation boundary | designed dormant | `DEFER`; exclude from shipped feature claims | **No** |
| `SpiritualCatalogLoader` + `SpiritualMeaningCoordinator` | documented Plan 30 activation boundary | designed dormant | `DEFER`; exclude from beta claims | **No** |
| `ProceduralEulogyEngine` | Wave 6 found zero references anywhere | dead implementation but high narrative value | `WIRE` into survivor death pipeline, not delete by default | **No** |
| `DwellerHeirloomCatalog` / grief hooks | test-only / no host consumer in Wave 6 | unwired intended system | `WIRE` to death/inheritance/morale | **No** |
| `LeadershipSystem.DesignateLeader` / `OnCrisisEvent` | no non-test callers in Wave 6 | inert gameplay seam | `WIRE` or explicitly cut leadership from beta | **Conditional** |
| `CohortSystem.TryMaturation` | no non-test caller in Wave 6 | inert progression seam | `WIRE` to campaign calendar if generational play remains beta scope; otherwise defer entire feature | **Conditional** |
| `WastelandMapView` + `MapLocationMarkerView` | Wave 8: map view zero refs; marker only referenced by dead view | dead presentation path | likely `WIRE`, because map/expedition is core; delete only if another map is authoritative | **No by default** |
| 30 unbacked flagship console classes | player-routable, hard-coded telemetry/no Core authority | false gameplay surfaces | immediately `REMOVE_FROM_PLAYER_ROUTING`; then delete or archive each source file unless an accepted system owner exists | **Yes after route quarantine** |
| `RadioStationCatalog.RegisterDefaults()` production defaults | current constructor still invokes it | duplicate/competing data authority | migrate station definitions to JSON, parity-test, then delete hardcoded defaults | **Yes after parity** |
| `InferBeliefProfile(...)` fallback | Wave 6: host infers identity while authored enrichment is unread | fallback authority masking unused authored data | wire enrichment authority, parity/migration test, then delete inference fallback | **Yes after wiring** |
| hardcoded item-id lists where tags exist | Wave 6 names Crafting/ShelterDecor/etc. | string-shaped duplicate authority | load tags, parity-test behavior, delete lists | **Yes after parity** |
| stale `LoaderWiringGateTests` allowlist rows | Debt entry demonstrably stale; method-specific detection can miss `Load(...)` | dead/stale test configuration | remove obsolete allowlist entries and replace heuristic with declared loader policy | **Yes** |
| stale agent rulebook claims (`GameBootstrap`, old asset migration, “IEventBus not used”) | current searches contradict several replicated rulebooks | documentation/config dead authority | regenerate from one canonical source; delete copied hand-maintained variants or make generated-only | **Yes, with generator** |
| runtime design mockups under `assets/ui/Screens` / `HtmlBundles` | Wave 9: 122 mockups, no code refs | shipped artifact bloat | move to `docs/design/mockups/` or external design archive; exclude from PCK | **Yes from runtime tree** |
| Unity-era root test/report/patch artifacts | Wave 9 found Unity playmode XML and one-off root scripts | stale artifact | archive/delete/relocate and add root-hygiene gate | **Yes after provenance check** |


### Important non-deletion rule


A zero-reference class is not automatically trash. `ProceduralEulogyEngine`, map presentation, grief, leadership, maturation, and other systems are examples where the correct fix may be **wiring**, not deletion. Deleting them before a beta-scope decision would erase already-paid implementation work and could cause the project to rebuild the same feature later.


---


# 4. The 20-issue flagship remediation backlog


Exactly 20 issues are selected below across dead code, bugs, silent failures, architecture, UI, data, artifacts, warnings, and quality gates.


---


## R01 — Loader wiring gate can be green while its classification is false


**Severity:** P0 / release-trust
**Type:** silent issue, test/gate defect
**Evidence:** CURRENT-REVALIDATED


`LoaderWiringGateTests` still allowlists `DebtTemplateCatalogLoader` as awaiting integration, while current production code invokes the loader through debt/store/session paths. The existing gate was designed mainly around `LoadAndRegister(...)` reachability and can therefore miss or misclassify `Load(...)`-style loaders.


### Failure mode


A loader can be:
- live but still listed as dead;
- dead but hidden behind an allowlist;
- called through an alternate entry point the source scanner does not recognize.


This contaminates every later dead-code decision.


### Remediation


1. Introduce `loader_wiring_policy.json` or a typed test manifest containing:
   - loader type;
   - accepted production entry points;
   - owning host/session;
   - disposition: `LIVE`, `DEFERRED`, `CUT`;
   - required catalog;
   - activation condition if deferred.
2. Source-scan the declared entry points rather than hardcoding `LoadAndRegister`.
3. Remove `DebtTemplateCatalogLoader` from the dead/unwired allowlist.
4. Require every allowlist row to include owner + reason + expiry/activation condition.
5. Fail CI when:
   - a `LIVE` loader has no production caller;
   - a `DEFERRED` loader becomes called without policy update;
   - an allowlist row has aged beyond its expiry.
6. Add a fixture with one `Load` loader and one `LoadAndRegister` loader to prove both detection paths.


### Acceptance gate


`LoaderWiringGateTests` must deliberately fail on a synthetic orphan and pass on both supported loader patterns.


---


## R02 — `AtmosphereTextSystem` pipeline is implemented but production-dead


**Severity:** P1
**Type:** dead code / dead content
**Evidence:** CURRENT-REVALIDATED


`AtmosphereCatalogLoader` and `AtmosphereTextSystem` exist, with a large authored atmosphere catalog, but current search still finds no production consumer.


### Decision


Do not preserve two environmental prose systems merely because they compile.


### Remediation


1. Compare fields/query semantics against `EnvironmentalTextSystem`.
2. Decide one beta venue:
   - expedition discovery line;
   - briefing collector;
   - map-location inspect text;
   - journal environmental observation.
3. If environmental flavor is a beta pillar:
   - create one canonical `EnvironmentalFlavorCatalog`;
   - migrate both data sets;
   - delete the duplicate loader/system.
4. If not:
   - remove both runtime loader/system pairs from shipping;
   - archive authored JSON outside the runtime data package;
   - retain a manifest entry explaining the cut.
5. Add a reachability test from player/simulation action to emitted flavor text.


### Acceptance gate


No environmental-flavor runtime class exists without a production consumer, and no shipped catalog is present solely because a test loads it.


---


## R03 — `EnvironmentalTextSystem` duplicates the same dead feature shape


**Severity:** P1
**Type:** dead code / duplicate architecture
**Evidence:** CURRENT-REVALIDATED


The system is currently referenced only by its loader/audit/gate and substantially overlaps the atmosphere-text capability.


### Remediation


Treat R02 and R03 as one consolidation transaction:


1. Create a field-by-field diff of both DTOs.
2. Select one canonical query API.
3. Migrate content without losing IDs/tags.
4. Delete the losing system/loader and its tests.
5. Convert the surviving tests from “loader works” to “player path consumes content”.


### Acceptance gate


Exactly one environmental prose authority remains.


---


## R04 — Holdfast NPC catalog is still zero-reach


**Severity:** P1
**Type:** unwired code / content island
**Evidence:** CURRENT-REVALIDATED


Current search finds `HoldfastNpcCatalogLoader`, the catalog, audit docs, and the wiring allowlist, but no production load path.


### Remediation


1. Determine whether Holdfast NPC quest loops are in the beta definition.
2. If **yes**:
   - load once at composition;
   - inject into the Holdfast quest/narrative host;
   - expose NPC lookups through one read-only catalog port;
   - add an integration test: authored NPC -> quest/event -> UI -> save/reload.
3. If **no**:
   - cut the catalog and loader from shipped data/code;
   - leave a deferred design document only.
4. Never keep it in a permanent allowlist.


### Acceptance gate


Either one proven runtime consumer exists or the code/data are absent from the beta package.


---


## R05 — Collectibles catalog and 40 collectible definitions remain unwired


**Severity:** P2
**Type:** dead feature / content bloat
**Evidence:** CURRENT-REVALIDATED


Current search finds `CollectibleCatalogLoader` only in source/tests/audit/gate. The loader also documents a missing-file `null`/silent-empty behavior.


### Remediation


For beta, default to **cut unless discovery/collecting is a declared pillar**.


If cut:
1. remove player-facing references;
2. move JSON/content to an archive/non-runtime content pack;
3. delete loader/system tests that only prove parsing;
4. remove gate exemption.


If kept:
1. make the catalog required;
2. replace silent missing-file behavior with a startup diagnostic/failure in strict mode;
3. wire acquisition -> inventory/codex/journal -> save -> UI.


### Acceptance gate


No collectible content is shipped without acquisition and presentation.


---


## R06 — Radio station definitions have two authorities


**Severity:** P1
**Type:** authority fork / silent drift
**Evidence:** CURRENT-REVALIDATED


`RadioStationCatalog` constructor still calls `RegisterDefaults()`, while JSON is the intended authored authority.


### Failure mode


Hardcoded station defaults can make missing or malformed JSON appear healthy and can drift from radio content.


### Remediation


1. Serialize the six current default station definitions to a parity fixture.
2. Add authoritative station definitions to JSON/schema if not already represented.
3. Load station definitions once at startup.
4. Assert field-for-field parity before deletion.
5. Delete `RegisterDefaults()` and any fallback that repopulates silently.
6. Missing required station definitions must be diagnostic failures in strict beta builds.
7. Add an authority gate forbidding authored station definitions in C#.


### Acceptance gate


Search for production `RegisterDefaults()` in radio returns zero; station state still save/restores.


---


## R07 — Moral choices exist and persist but the player cannot resolve them


**Severity:** P0
**Type:** gameplay blocker / unwired action
**Evidence:** CURRENT-REVALIDATED


Current search finds the private `TryResolveMoralChoice(...)` declaration and planning/audit references, but no production call site.


### Remediation


1. Create a player-facing moral-choice decision surface/modal.
2. Feed it from the canonical moral-choice definition/state.
3. Bind option selection to a host action.
4. The host invokes the canonical resolver exactly once.
5. Surface irreversible/locked state clearly.
6. Persist result and consequences.
7. Add keyboard/controller support.
8. Add one journey test that:
   - reaches a moral choice;
   - selects option;
   - observes world/journal consequence;
   - saves;
   - reloads;
   - proves choice cannot be applied twice.


### Acceptance gate


Repo search shows at least one real production action calls the resolver; a journey test proves it.


---


## R08 — Thirty player-routable flagship consoles are false affordances


**Severity:** P0/P1
**Type:** UI bloat / fake gameplay / dead presentation
**Evidence:** AUDIT-CARRYFORWARD; high-confidence source audit


The UI audit identified 30 registered/openable panels with hard-coded telemetry, feedback-only buttons, empty refreshes, or no authoritative Core/host data contract.


### Immediate action


**Remove all 30 from player routing before beta.**


Do not wait for 30 new subsystems.


### Per-panel triage


Assign each:
- `KEEP_AND_WIRE` — real existing system already exists.
- `MERGE` — capability belongs inside another live panel.
- `PROTOTYPE_ONLY` — move to `docs/design/mockups/` or developer tooling.
- `DELETE` — no accepted gameplay premise.


### Required guard


Add a `PlayerSurfaceLivenessGate`:
- every player-routable panel must declare its authoritative session/system;
- every actionable panel must expose at least one non-feedback-only action delegate or explicitly be `READ_ONLY`;
- literal telemetry fixtures are forbidden in production panels;
- prototype panels cannot appear in `PanelRegistryBootstrap`.


### Acceptance gate


Player-routable fake-console count: **30 -> 0**.


---


## R09 — Four live panels have unsafe event unsubscription/rebind behavior


**Severity:** P1
**Type:** lifecycle bug / leak / duplicate refresh
**Evidence:** AUDIT-CARRYFORWARD


Previously identified:
- `WeatherHistoryPanel`
- `GeigerCalibrationPanel`
- `FireIncidentPanel`
- `TriangulationPanel`


The pattern unsubscribes a newly-created lambda instead of the exact delegate subscribed earlier; Triangulation also left another subscription active.


### Remediation


1. Store every subscription delegate in a field.
2. Centralize `Bind/Unbind`.
3. Make `Bind` idempotent.
4. Call `Unbind` on replacement and node teardown.
5. Add repeated cycle test:
   - bind -> fire event -> exactly 1 refresh;
   - rebind 50 times -> still exactly 1 refresh/event;
   - free node -> no callbacks.
6. Extend lifecycle selftest to every live panel with event subscriptions.


### Acceptance gate


50x bind/unbind cycle produces no duplicate callback and no callback after disposal.


---


## R10 — Routed panels can receive fresh/disconnected systems instead of campaign authority


**Severity:** P0/P1
**Type:** ownership bug / fabricated UI state
**Evidence:** AUDIT-CARRYFORWARD


The UI audit found routes constructing fresh `ShelterFireHazardSystem`, `FactionStanceEngine`, and `SkillProgressionSystem` instances for display instead of using campaign-owned state.


### Failure mode


A panel looks live while showing defaults or a separate universe.


### Remediation


1. Ban `new <GameplaySystem>()` inside player-surface routing/UI binding except explicitly pure stateless helpers.
2. Resolve systems through campaign composition/session ownership.
3. Add a source gate that identifies gameplay-system construction under:
   - `src/UI/`
   - `src/Main.PlayerSurfaces.cs`
   - route lambdas.
4. Add identity tests proving the panel receives the exact same instance/aggregate the simulation updates.


### Acceptance gate


No player route constructs an independent mutable gameplay authority.


---


## R11 — Trade-specialty progression still lacks real craft attribution


**Severity:** P1
**Type:** partially wired system / silent progression gap
**Evidence:** AUDIT-CARRYFORWARD from unwired-code audit


The loader itself was repaired, but production crafting still lacked a canonical survivor/profession attribution path; the demonstrated call used a debug button with hard-coded identity/profession.


### Remediation


1. Add canonical craft context:
   - crafting station;
   - assigned survivor;
   - recipe/item;
   - current profession/specialty context.
2. Derive attribution from duty/workbench assignment, not UI literals.
3. Emit `CraftCompleted(CraftContext)`.
4. `TradeSpecialtySystem` consumes the event/port.
5. Remove hardcoded debug identities from production flow.
6. Test:
   - matching craft progresses correct survivor;
   - wrong survivor does not;
   - unassigned automated craft has explicit policy;
   - save/reload preserves progress.


### Acceptance gate


No production specialty progression depends on a debug button or hardcoded survivor ID.


---


## R12 — Integration seams are not mechanically required to have host callers


**Severity:** P0
**Type:** systemic architecture gap
**Evidence:** AUDIT-CARRYFORWARD


Wave 5 counted 147 integration-shaped Core methods and found 74 with no caller in `src/` before triage. Not every uncalled method is dead, but the number demonstrates that isolated unit-test coverage is not enough.


### Remediation


Build a `PortContractGate`.


1. Enumerate public integration methods matching:
   - `Bind*`
   - `Set*`
   - `Wire*`
   - `Register*`
   - `Apply*`
   - `Enable*`
   - `Configure*`
   - domain-specific transition methods.
2. Require classification:
   - `HOST_REQUIRED`
   - `LIVE_VIA_CORE`
   - `TEST_ONLY`
   - `DEFERRED`
   - `PURE_LIBRARY`.
3. `HOST_REQUIRED` methods must have a production call path.
4. `TEST_ONLY` in a gameplay system is a failure unless explicitly diagnostic.
5. Generate the report into CI artifacts.
6. Seed the mechanism from the existing `CombatHostSession.ValidatePorts` pattern.


### Acceptance gate


No unclassified integration seam. Every required port is mechanically proven bound.


---


## R13 — Wildlife trapping can produce a “catch” that never becomes goods


**Severity:** P0
**Type:** resource-loss bug / broken production chain
**Evidence:** AUDIT-CARRYFORWARD


Wave 5 found trapping state records catch species/yield/preservation, while the host lacked an inventory delivery path; the catch was effectively a panel string.


### Remediation


1. Create one canonical output sink for all producers.
2. Resolve carcass/hide outputs to canonical item IDs.
3. Handle capacity refusal explicitly.
4. Transaction semantics:
   - do not clear catch until delivery succeeds;
   - if partial delivery is allowed, persist remainder;
   - failure reports a reason.
5. Add disease/contamination state to the delivered stack if applicable.
6. Test full, partial, zero-capacity, save-before-claim, save-after-claim, duplicate-claim.


### Acceptance gate


Every accepted catch changes authoritative inventory or remains claimable; no yield disappears.


---


## R14 — Water has parallel authorities and ration consumption is not reliably connected


**Severity:** P0
**Type:** duplicate resource authority / survival-loop bug
**Evidence:** AUDIT-CARRYFORWARD


Wave 5 identified litres in water-treatment state versus `clean_water` inventory items, a nullable bridge, and `ConsumeRation` with no callers at the audited snapshot.


### Remediation


1. Write a one-page ADR naming the single physical water authority.
2. Define explicit conversion operations:
   - draw/package water;
   - pour/unpackage water.
3. Make host dependency non-null for gameplay.
4. Inventory and plant quantities must never both independently represent the same water.
5. Bind thirst/ration consumption to the canonical transaction.
6. Add mass-balance property test across 200 simulated days.
7. Add save/reload test mid-transfer.


### Acceptance gate


One water authority + explicit packaging conversion; mass balance closes exactly within allowed rounding.


---


## R15 — Authored survivor identity is ignored while the host invents beliefs


**Severity:** P1
**Type:** dead data / fallback masking authority
**Evidence:** AUDIT-CARRYFORWARD


Wave 6 reported `expansion_survivor_fields.json` authored identity data with zero consumers and `ExpansionEnrichmentCatalog` with zero references, while the host used `InferBeliefProfile(...)`.


### Remediation


1. Load the enrichment catalog at composition.
2. Establish survivor-field authority:
   - belief profile;
   - profession;
   - keepsake;
   - background;
   - tags.
3. Migrate overlapping fields so each has exactly one owner.
4. Replace inference with authored data.
5. Use inference only as an explicit legacy migration fallback, then delete it after save compatibility window.
6. Add integrity tests for survivor IDs and all referenced item/tag IDs.


### Acceptance gate


Authored survivor identity fields consumed: target **100% of fields intended for beta**; inference fallback absent from normal new-game flow.


---


## R16 — Death-memory pipeline contains dead/test-only systems


**Severity:** P1
**Type:** dead code / emotional-system gap
**Evidence:** AUDIT-CARRYFORWARD


Wave 6 found:
- `ProceduralEulogyEngine` zero refs;
- grief application effectively test-only;
- heirloom catalog without a host consumer.


### Remediation


Wire one canonical death transaction:


`survivor death -> death quality/context -> grief -> relationship/morale effects -> heirloom inheritance -> eulogy -> journal/memorial -> save`


1. Instantiate eulogy engine in the death/narrative pipeline.
2. Apply grief through a required port.
3. Resolve heirlooms deterministically.
4. Prevent duplicate processing across save/load.
5. Journal the outcome.
6. Add deterministic snapshot/journey test.


### Delete rule


Only delete these classes if the entire death-memory feature is explicitly removed from beta scope. Do not leave them test-only.


### Acceptance gate


One survivor death produces all declared consequences exactly once.


---


## R17 — Leadership, affinity, and maturation contain state with little/no gameplay consumption


**Severity:** P1
**Type:** underconnected simulation
**Evidence:** AUDIT-CARRYFORWARD


Wave 6 reported:
- `DesignateLeader` and `OnCrisisEvent` with no non-test caller;
- affinity written by multiple systems but read by no consequential consumer;
- `TryMaturation` with no production caller.


### Remediation


1. Decide beta scope per subsystem independently.
2. Leadership:
   - player/session designation action;
   - crisis producer;
   - observable stress/break-risk consequences.
3. Affinity:
   - expose one `Relations.EffectOf(a,b)` query;
   - consume in duty assignment, expedition party, care, and training.
4. Maturation:
   - call from canonical campaign calendar;
   - update age/ration/duty eligibility atomically.
5. If any subsystem is deferred, disable its player surfaces and content claims.


### Acceptance gate


Every retained state variable has at least one consequential reader outside its own writer/panel.


---


## R18 — Dead-content acceptance is weak: hundreds of definitions can “exist” without reaching gameplay


**Severity:** P0
**Type:** content bloat / gate blind spot
**Evidence:** AUDIT-CARRYFORWARD


Wave 7 measured 29 catalogs / 452 authored definitions reaching nobody at its snapshot. It also found:
- root-array catalogs counted as zero definitions;
- exemptions without enforced expiry;
- very few catalogs reaching an `EFFECT_PRODUCED` tier.


### Remediation


Implement a content acceptance ladder:


1. `PARSES`
2. `IDS_RESOLVE`
3. `LOADED`
4. `CONSUMER_EXISTS`
5. `PLAYER_OR_SIM_REACHABLE`
6. `EFFECT_PRODUCED`
7. `PRESENTED`
8. `SAVE_ROUNDTRIP` where stateful.


Each shipped definition/catalog must declare its required rung.


Also:
- correctly count root-array catalogs;
- require owner/rationale/expiry for exemptions;
- make expired exemptions fail CI;
- archive/cut content not targeted for beta instead of indefinitely exempting it.


### Acceptance gate


No beta-critical catalog remains below its declared acceptance rung; no permanent exemption exists without an activation/expiry rule.


---


## R19 — Asset/presentation gates measure presence more than rendered truth


**Severity:** P1
**Type:** artifact bloat / dead presentation code / weak QA
**Evidence:** AUDIT-CARRYFORWARD


Wave 8 found at its snapshot:
- asset gate checked 50 of 5,563 IDs;
- fallback use counted as valid;
- 1,189 art files plus many icons unreferenced;
- zero-reference `WastelandMapView`;
- `MapLocationMarkerView` only reachable through that dead view;
- orphan scenes;
- no reliable ID->asset manifest.


### Remediation


1. Create `asset_registry.json` mapping canonical IDs to actual asset paths.
2. Make strict beta mode fail on fallback.
3. Produce coverage by asset family.
4. Reconcile every unreferenced runtime asset:
   - wire;
   - archive;
   - delete;
   - move to design-only.
5. Wire the authoritative map view or delete the dead map implementation in favor of the live map.
6. Remove orphan scenes or boot them intentionally.
7. Add populated-state visual snapshots and “no undeclared placeholder” assertions.
8. Move design mockups out of runtime asset folders.


### Acceptance gate


100% of shipped IDs in asset-requiring families are either explicitly mapped or explicitly declared text-only; strict mode has 0 fallback successes.


---


## R20 — “Green” repository health is diluted by warning suppression and stale duplicated rulebooks/artifacts


**Severity:** P1
**Type:** warning debt / repository truth / bloat
**Evidence:** CURRENT-REVALIDATED + AUDIT-CARRYFORWARD


Current authority docs acknowledge three minor analyzer warnings, while current source search shows broad `#pragma warning disable CS8618` usage and at least one `CS0649` suppression. Separately, replicated agent rulebooks still contain disproven/stale claims such as nonexistent/obsolete `GameBootstrap` guidance, outdated asset-migration state, and misleading statements about event-bus usage. Wave 9 also identified stale Unity-era/root artifacts and runtime design mockups.


### Remediation


1. Generate all agent-specific rulebooks from **one** canonical source.
2. Add a sync gate that diffs generated copies and fails on drift.
3. Delete stale references to non-existent architecture.
4. Replace broad nullable suppressions incrementally:
   - Godot node fields: initialized with `null!` only where engine lifecycle guarantees assignment, plus runtime guard;
   - dependencies: constructor required;
   - optional state: nullable and checked.
5. Resolve xUnit analyzer warnings rather than baseline them.
6. Restrict `#pragma warning disable` with a source gate:
   - exact warning code;
   - comment/rationale;
   - local scope where possible;
   - no file-wide blanket suppression for shipping code without allowlist.
7. Move design mockups to docs/design.
8. archive/delete Unity-era root test reports and one-off patch scripts after provenance review.
9. Add a root-hygiene/forbidden-artifact gate.


### Acceptance gate


- build/test analyzer warnings: 0 unexplained;
- generated rulebooks: 0 drift;
- stale architecture claims: 0;
- runtime asset tree: no design-only mockups;
- no forbidden Unity-era artifact at repo root.


---


# 5. Supplemental findings not counted in the 20-issue execution set


These should be tracked, but they are deliberately not allowed to inflate the first remediation batch.


### Duplicate `WornGear`


`AGENTS.md` currently records a duplicate `WornGear` class in Inventory and Radiation with a sanctioned bridge. This should eventually become one canonical DTO/type. Do not remove the bridge first; migrate callers, then delete the duplicate type.


### Journal behavior testing


The current authority file states that `JournalSystem` behavior remains under-tested even though save-store integrity is covered. Add core behavior tests after the P0 wiring work.


### Retention / long-campaign growth


Wave 9 found persisted collections with no retention policy. This is important for 200–400-year campaigns and should become the first nightly-soak follow-up after liveness remediation.


### Save-corpus fixtures


Wave 9 reported genuine historical saves existing outside the repository. Salvage/sanitize representative fixtures and use them to gate future migrations.


### Balance artifact provenance


Wave 7 identified balance CSVs without a reproducible producer/decision trail. Either regenerate from checked-in scenarios or delete obsolete outputs.


### Release/versioning


Existing audits found no meaningful release-tag/changelog discipline at their snapshot. Before public beta, establish a tagged beta baseline and save-compatibility window.


---


# 6. Removal sequence — how to delete safely


Dead-code deletion should never begin with `rm`.


## Phase A — Snapshot and prove


For every deletion candidate:


1. record current commit;
2. capture source references;
3. capture tests referencing it;
4. capture catalog/data IDs;
5. capture save-section involvement;
6. capture routes/panels;
7. capture public API references;
8. classify the candidate.


Required output: `artifacts/remediation/deletion_inventory.json`.


Suggested fields:


```json
{
  "symbol": "AtmosphereTextSystem",
  "files": ["Assets/Ashfall.Core/AtmosphereTextSystem.cs"],
  "production_references": 0,
  "test_references": 0,
  "data_files": ["Assets/StreamingAssets/Data/environmental_atmosphere_expansion.json"],
  "player_routes": [],
  "save_sections": [],
  "disposition": "CONSOLIDATE",
  "reason": "duplicate zero-reach environmental flavor pipeline",
  "owner": "content-runtime",
  "beta_required": false
}
```


## Phase B — Quarantine before delete


For player-visible candidates:
- remove route;
- remove registry descriptor;
- remove button/hotkey;
- keep code for one regression cycle.


For data candidates:
- remove from shipping catalog registry/content pack;
- keep under an archive path for one cycle if provenance matters.


This catches hidden dependencies before source deletion.


## Phase C — Delete


Only after:
- clean build;
- clean tests;
- data integrity;
- content utilization;
- route gate;
- save registry;
- asset registry.


## Phase D — Add anti-regression gate


Every deletion should leave behind a rule that makes the same failure harder to recreate.


Examples:
- fake panel -> liveness gate;
- orphan loader -> loader policy;
- hardcoded defaults -> authority source gate;
- stale rulebook -> generated sync gate;
- dead catalog -> content acceptance ladder.


---


# 7. Proposed execution order


## Wave A — Trust the gates first


Do before changing features.


1. R01 Loader gate repair.
2. R12 Port-contract / integration-seam gate.
3. R18 Content acceptance ladder.
4. R20 rulebook/warning truth cleanup.
5. R19 strict asset-registry semantics.


**Reason:** deleting based on a dishonest scanner is worse than leaving dead code.


---


## Wave B — Remove false player promises


1. R08 quarantine 30 fake consoles.
2. R10 remove disconnected/fresh system bindings.
3. R09 repair panel subscription lifecycle.
4. R07 make moral choice operable.
5. re-run player journey.


At the end of Wave B, everything the player can open must either be live or explicitly read-only.


---


## Wave C — Fix survival/economy production chains


1. R13 trapping output delivery.
2. R14 water authority + consumption.
3. R11 craft attribution.
4. run 30-day and 200-day mass-balance simulations.


---


## Wave D — Decide dead feature islands


In this order:


1. R02/R03 environmental prose consolidation/cut.
2. R04 Holdfast NPC decision.
3. R05 Collectibles decision.
4. R06 radio authority consolidation.


This is where actual source/data deletion should happen.


---


## Wave E — Make people consequential


1. R15 authored identity.
2. R16 death-memory transaction.
3. R17 leadership/affinity/maturation.
4. seeded long-play journey proving outcomes.


---


## Wave F — Artifact and presentation cleanup


1. R19 asset mapping/orphans/map view.
2. R20 mockups/root artifacts/rulebooks/warnings.
3. package-size report.
4. final dead-code scanner.
5. beta release gate.


---


# 8. Required automated gates after remediation


A flagship-beta branch should refuse to merge when any of these fail.


## Build / tests


```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```


Target: **0 errors; 0 unexplained warnings; 0 failed tests.**


## Godot/core selftests


Run the current canonical selftests from the repository, including at minimum:


```bash
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --asset-registry-selftest
godot --headless --path . -- --bridge-selftest
```


Then add:


- `--loader-wiring-selftest`
- `--port-contract-selftest`
- `--player-surface-liveness-selftest`
- `--content-acceptance-selftest`
- `--asset-registry-strict-selftest`
- `--panel-lifecycle-selftest`
- `--resource-mass-balance-selftest`
- `--beta-journey-selftest`


## Source policy gates


Add CI scanners for:


- gameplay `new ...System()` inside UI/routes;
- `RegisterDefaults()` in JSON-authoritative domains;
- dead/expired loader exemptions;
- `#pragma warning disable` without rationale/allowlist;
- player-routable panels without authority declaration;
- root forbidden artifacts;
- design-only mockups under runtime asset paths;
- stale/generated rulebook drift.


---


# 9. Beta journey that proves the seams


A single deterministic player journey should exercise multiple remediation items in one trace.


Suggested scenario:


1. start fresh campaign with fixed seed;
2. open onboarding/guidance;
3. inspect shelter and world map;
4. assign survivor to crafting station;
5. craft item and verify specialty attribution;
6. trap wildlife and collect actual goods;
7. treat/package water and consume it;
8. receive radio transmission;
9. trigger and resolve a moral choice;
10. cause a relationship change;
11. trigger survivor death in controlled fixture;
12. verify grief/eulogy/heirloom;
13. advance calendar to a maturation/season boundary where enabled;
14. save;
15. terminate host;
16. reload;
17. verify every state transition persists;
18. reopen panels repeatedly;
19. assert no duplicate event callbacks;
20. verify no undeclared placeholder/fallback assets.


The trace should emit a machine-readable ledger:


`artifacts/remediation/beta_journey_trace.json`


Each step:
- action;
- authoritative owner;
- before state hash;
- after state hash;
- emitted events;
- saved section(s);
- presented panel/audio/art;
- assertion result.


---


# 10. Dead-code scanner specification


A robust scanner should combine several signals instead of pretending lexical references equal liveness.


For every public Core type/method/catalog:


### Structural signals


- definition exists;
- production call/reference exists;
- test-only reference exists;
- constructed by composition root/session;
- loaded from catalog;
- registered in save section;
- bound to route/panel;
- subscribed to events;
- emits consequential state;
- presented to player.


### Classification


- `LIVE`
- `LIVE_VIA_CORE`
- `TEST_ONLY`
- `DATA_ONLY`
- `PRESENTATION_ONLY`
- `UNWIRED`
- `DEAD`
- `DEFERRED`
- `PROTOTYPE`
- `FALSE_SURFACE`


### Failure policy


For beta:
- `UNWIRED`, `DEAD`, `FALSE_SURFACE` -> fail unless explicitly in a temporary remediation manifest.
- `DEFERRED` -> pass only with owner + reason + activation/expiry.
- `TEST_ONLY` gameplay code -> warning/fail depending on API intent.
- `PROTOTYPE` -> must be excluded from player routes and runtime package.


---


# 11. Repository bloat cleanup policy


Do not judge bloat only by megabytes. In ASHFALL, the more dangerous bloat is **false authority**.


Priority order:


1. stale code that appears live;
2. player-routable prototype UI;
3. duplicate data authorities;
4. stale/generated rulebook copies;
5. runtime design mockups;
6. orphan art/audio/content;
7. one-off scripts/root reports;
8. build/import caches.


Every moved/deleted artifact should receive a receipt in:


`artifacts/remediation/artifact_disposition.csv`


Columns:


- path
- previous role
- reference count
- package inclusion
- disposition
- replacement
- owner
- reason
- commit


---


# 12. Warning/error policy for beta


A “0 warning” build is only meaningful if warnings are not globally silenced.


## Policy


- No project-wide `NoWarn` for shipping code.
- No file-wide `#pragma warning disable CS8618` without a written lifecycle reason.
- Prefer constructor initialization for host services.
- For Godot scene-injected nodes:
  - use a narrowly scoped engine-lifecycle pattern;
  - assert required node exists in `_Ready`;
  - fail fast with panel/node path.
- Resolve xUnit analyzer findings.
- Treat new warnings as CI failures.
- Baselines may exist only for third-party/generated code, never ordinary Core/host code.


---


# 13. Data/content policy


Every runtime JSON catalog must answer:


1. Who loads me?
2. Who owns the parsed state?
3. What game action reaches me?
4. What observable effect can I produce?
5. Do I save state?
6. How is missing/malformed data reported?
7. Am I beta-critical, optional, or deferred?


If any answer is unknown, the catalog should not silently ship.


This directly addresses the historical accumulation of hundreds of definitions that parse but reach nobody.


---


# 14. UI policy


A panel may be player-routable only if its descriptor declares:


```text
panel_id
authority_type
authority_owner
read_only
bind_method
action_count
save_dependency
journey_test
prototype=false
```


A panel with literal demo telemetry is not a game system.


Prototype panels should live outside the player registry and preferably outside the runtime package.


---


# 15. Architecture rules to prevent recurrence


1. **One authority per fact.**
   - water, station definitions, survivor identity, gear, item tags, campaign time.


2. **No silent default that impersonates loaded data.**
   Missing required catalogs should be loud in strict builds.


3. **No new system without a producer and consumer.**
   A class + tests is not a feature.


4. **No new panel without an authority contract.**


5. **No new catalog without an acceptance rung.**


6. **No “deferred forever”.**
   Exemptions need expiry or activation conditions.


7. **No hand-copied rulebook authorities.**
   Generate them.


8. **No player-facing fake state.**
   Panels bind to campaign-owned instances.


9. **Every transition that matters is journey-tested.**


10. **A removal leaves a gate behind.**


---


# 16. Definition of done — repository remediation


The remediation program is complete when:


### Dead/unwired code


- all current zero-reach candidates are classified;
- all beta-cut candidates are removed/quarantined;
- all retained candidates have production reach;
- `DebtTemplateCatalogLoader` and other live loaders are not falsely allowlisted;
- designed dormant systems are explicitly deferred.


### Player liveness


- 0 fake player-routable consoles;
- moral choices are operable;
- no panel binds a fresh gameplay authority;
- event subscriptions survive repeated open/close/rebind.


### Core integration


- required Core integration seams all have proven callers;
- trapping goods arrive;
- water has one authority;
- crafting attribution is real.


### Human/narrative layer


- authored survivor identity is consumed;
- retained grief/eulogy/heirloom systems are live;
- retained leadership/affinity/maturation state has consequential readers.


### Content/artifacts


- no beta-critical catalog is content-dead;
- exemptions expire;
- root arrays are counted;
- asset strict tier fails on fallback;
- unreferenced runtime art is reconciled;
- map presentation is either live or deleted;
- design mockups are not shipped as runtime game assets.


### Warnings/repo truth


- 0 unexplained analyzer warnings;
- warning suppressions are narrow and justified;
- agent rulebooks are generated and synchronized;
- stale architecture claims are gone;
- root artifact hygiene gate is green.


### Final beta gate


One deterministic end-to-end journey passes fresh start -> gameplay -> save -> process restart -> reload -> continued gameplay, with no critical silent fallback and a complete trace.


---


# 17. Recommended first 10 implementation tickets


If this plan is executed incrementally, start here:


1. **REM-001 — Replace LoaderWiringGate heuristic with declared loader policy.**
2. **REM-002 — Add PortContractGate and classify every integration seam.**
3. **REM-003 — Remove 30 unbacked consoles from player routing.**
4. **REM-004 — Fix Moral Choice host/UI action path.**
5. **REM-005 — Repair panel event subscription lifecycle and add 50x rebind test.**
6. **REM-006 — Eliminate fresh gameplay-system construction in player routes.**
7. **REM-007 — Deliver trapping yields through authoritative inventory/output sink.**
8. **REM-008 — Unify water authority and wire ration/thirst consumption.**
9. **REM-009 — Build content acceptance ladder + expiring exemptions.**
10. **REM-010 — Create strict ID->asset registry and reconcile runtime orphans.**


Only after these ten should the team spend substantial effort adding new gameplay systems.


---


# 18. Final recommendation


For flagship beta, perform a **subtractive integration pass**, not another expansion wave.


The repository already has enough breadth to produce a compelling beta. The highest-value engineering work is to:


- delete false surfaces;
- connect the systems already paid for;
- reduce duplicate authorities;
- make missing data loud;
- turn content tests into player-reachability tests;
- make asset tests assert real rendering rather than fallback presence;
- make repo rules generated and current;
- and refuse new systems that do not arrive with their rails.


The desired end state is not “more code”.


It is a repository where a green build means:


**the feature is loaded, owned, callable, observable, persistent, presented, and actually reachable by a player.**
---

# Current HEAD revalidation addendum — 2026-09-04

This addendum preserves the primary backlog above (which contains exactly 20 numbered R01–R20 issues) and records findings revalidated directly against commit 9b4985d0122d707c31f6078050df5877b69b607b. It refines the deletion decisions without creating a second numbered backlog.

## Current checkout facts

- The checkout was clean before this document update.
- Tracked files: 11,955.
- JSON authority files: 487 recursively; 205 at the data root and 279 under narrative.
- Assets/Ashfall.Core contains 618 C# files; src contains 475 C# files; tests contain 541 C# files.
- Fifteen tracked symlinks are broken and point to an absolute path on another machine.
- The largest tracked regular file is approximately 6 MB: Ashfall.Core.Tests/TestResults/results.trx.
- assets/quarantine contains approximately 2,201 tracked files and 150 MB of working-tree bytes.
- dotnet and Godot were not installed in the audit environment. Static gates were run; build/test/headless/export verification remains pending.

## Current revalidation: runtime data and boot

### Recursive runtime coverage is incomplete

CatalogIntegrityValidator.Validate defaults to top-directory-only, and HostCli self-tests call that default. RunCatalogBootPreflight also enumerates only the top directory. The runtime path therefore exercises 205 root JSON files while the repository contains 487 recursive files. The Python schema gate passes all 487, but that does not prove runtime loader/reference coverage.

Required disposition: make recursive, relative-path-preserving enumeration the runtime default; assert the revision-pinned manifest count; validate nested content and cross-references; and report duplicate basenames. Root and narrative copies of bunker_graffiti_postings.json and wasteland_grave_epitaphs.json differ, so basename-only diagnostics are ambiguous.

### Catalog boot is heuristic and can fail silently

CatalogBootValidator registers a small required/optional subset rather than the complete authority set. Filename substring classification can misclassify test/developer files, and its DeveloperOnly behavior does not match its “silent” documentation. HostDefaults.EnumerateFiles catches all exceptions and returns an empty result, allowing partial enumeration failure to resemble empty data.

Required disposition: use a generated manifest with relative path, schema, loader, owner, requiredness, and consumer; make required/developer-only behavior explicit; reject duplicate registration; and surface enumeration exceptions.

## Current revalidation: loader and dead-code decisions

### LoaderWiringGate coverage is too narrow

LoaderWiringGateTests recognizes only a public static int LoadAndRegister(...) shape and searches for a narrow class-qualified production call. Load(...) loaders can evade the check. The current allowlist includes:

- SkyLayerArmorCatalogLoader — designed dormant, but SkyLayerArmorSystem itself has live world/telemetry/hazard consumers;
- SpiritualCatalogLoader — designed dormant;
- AtmosphereCatalogLoader — no consumer surface;
- EnvironmentalTextCatalogLoader — no consumer surface;
- DebtTemplateCatalogLoader — stale exception; ExpansionHostSession now calls Load and Main.DebtCredit.cs consumes the catalog;
- HoldfastNpcCatalogLoader — no consumer surface;
- CollectibleCatalogLoader — no consumer surface.

Required disposition: generalize the scanner to loader contracts/attributes and actual host-to-consumer reachability; remove DebtTemplate from the allowlist; give every remaining exception an owner, reason, activation condition, and expiry revision.

### Confirmed zero-reach or dormant feature islands

The following are removal-or-wire decisions, not indefinite allowlists:

- AtmosphereCatalogLoader plus AtmosphereTextSystem and environmental_atmosphere_expansion.json, 189 entries.
- EnvironmentalTextCatalogLoader plus EnvironmentalTextSystem and environmental_texts_expansion_05.json, 42 entries.
- HoldfastNpcCatalogLoader plus HoldfastNpcCatalog; holdfast_npcs.json is absent and missing data falls back to ten hardcoded NPCs.
- CollectibleCatalogLoader plus CollectibleCatalog and collectibles.json; tests load it, but no production consumer was found.
- SpiritualCatalogLoader plus SpiritualMeaningCoordinator and spiritual_rituals.json, memorial_rites.json, and belief_movements.json; explicitly designed dormant.
- SkyLayerArmorCatalogLoader and its catalog/config data if that loader is not part of the current product; retain SkyLayerArmorSystem and its live consumers unless that separate system is also retired.

For each retained feature, add a production-owned host, a real consumer/action, and content/save/reload evidence. For each cut feature, remove code, data, tests, docs, generated rows, and allowlist entries as one transaction.

### Static Narrative orphan inventory

A source-reference scan found the following 63 Narrative *Catalog classes with no ordinary source reference outside their own declaration/tests. They are triage candidates, not blind-delete targets: reflection, filename dispatch, codex tooling, and dynamic registries must be checked in the manifest first.

AbyssalAnomaliesCatalog, ApicultureBeeCatalog, BlackProjectsCatalog, BoneHornCarvingCatalog, BunkerContrabandCatalog, BunkerCourtCatalog, BunkerGraffitiCatalog, BunkerMaintenanceCatalog, CandleMakingWaxCatalog, CeramicsKilnCatalog, CharcoalPyrolysisCatalog, CordageCableCatalog, CourierDispatchCatalog, CrucibleFoundryCatalog, CryoPreservationCatalog, CulinaryRationCatalog, CurrentsPamphletCatalog, DeadHandDirectiveCatalog, DwellerHeirloomCatalog, DwellerMedicalCatalog, EncounterCatalog, FaunaEntomologyCatalog, FermentationYeastCatalog, FringeCultsCatalog, GeologicalStrataCatalog, GhostTransmissionCatalog, GlassblowingDistillationCatalog, GrainMillingCatalog, HydroGeologyCatalog, IndustrialRuinsCatalog, LostTechManualCatalog, MasonryBrickworksCatalog, MedicalPathologyCatalog, MetallurgyToolingCatalog, MilitaryArmoryCatalog, NightWatchCatalog, OpticsGlassworksCatalog, PaperMakingCatalog, PaperPrintingCatalog, PneumaticTubeDispatchCatalog, PolymerTextileCatalog, RefrigerationFermentationCatalog, RelicProvenanceCatalog, RopeMakingCordageCatalog, SeedBankPreservationCatalog, SignalIntelligenceCatalog, SoapSaponificationCatalog, SteamTurbinePowerCatalog, StructuralFortificationCatalog, SurvivorLetterCatalog, TanningLeatherCatalog, TanningLeatherworkCatalog, TextileSpinningWeavingCatalog, TimberCarpentryCatalog, TimekeepingHorologyCatalog, TradeCaravanCatalog, UndergroundFungiCatalog, VinylRecordCatalog, WastelandCartographyCatalog, WastelandExpeditionCatalog, WastelandGazetteerCatalog, WastelandBestiaryCatalog, WaterTreatmentPotableCatalog, WireConfessionCatalog.

Each must receive one disposition: live gameplay, codex-only, test/tool-only, intentionally deferred, duplicate, or orphaned. Shipped classes need explicit registration and an end-to-end query test. Orphaned classes and their data should be removed or archived with an owner and expiry.

### High-confidence dead declarations

After the final reference check, these are safe cleanup candidates:

- Assets/Ashfall.Core/Performance/PerfTestMarker.cs — only declaration/constant occurrence observed.
- ICampaignSaveSection in Assets/Ashfall.Core/Save/CampaignSaveEnvelope.cs — declaration only; no implementer or caller observed.
- Sentry package reference — no Sentry API usage observed in src, Core, or tests; remove only if telemetry is not an external runtime contract.
- Ashfall.Core.Tests/TestResults/results.trx — tracked generated output.
- Tracked .qwen/tmp files — temporary output pending provenance review.
- Fifteen broken absolute skill symlinks under .codex/skills, .cursor/skills, and .qwen/skills.
- Root one-off authoring scripts with no supported workflow, after provenance review.
- Runtime-only design/quarantine artifacts that fail the package/asset retention decision.

## Current revalidation: player-facing and silent correctness issues

### Moral choice is set up but not reachable

Main.MoralChoice.cs defines setup/persistence and TryResolveMoralChoice, but no production caller to the resolver was found. Either connect it to the actual player choice panel/action and prove faction/journal/save effects, or remove the inaccessible setup/data surface from the shipped product.

### Onboarding is constructed but remains hidden

Main.Onboarding.cs creates and binds OnboardingHintPanel. The panel starts hidden, and no production Show/Visible route was found. Add a new-campaign/help/replay path with persistence, or remove the unreachable surface and its scene references.

### Thirty flagship panels are false affordances

The 30 panels are routed, so they are not dead declarations. They are false-surface candidates:

- The first ten — AnaerobicBiogasDigesterPanel, SubterraneanCartographyPanel, UndergroundPrintingPressPanel, SiliconIngotSlicingPanel, GeothermalSteamTurbinePanel, WarDogKennelPanel, IsotopeSeparatorPanel, PlasmaArcSmeltingPanel, BoreholeSeismographPanel, and HeavyLogisticsAirlockPanel — have local feedback-only handlers, empty RefreshView methods, and no meaningful Core binding.
- Most of the remaining twenty accept an object session only to set IsBound, ignore the authority, show literal/fixture state, and expose buttons with no real action handler.

Remove them from player routing before deletion, or give each a typed campaign-owned authority, real mutation path, and save/reload test. Fixture rows must be rejected on normal production routes.

### UI event unsubscription leaks

WeatherForecastPanel, WeatherHistoryPanel, FireIncidentPanel, GeigerCalibrationPanel, and TriangulationPanel subscribe with lambdas and attempt to unsubscribe with newly-created lambdas. Those delegates are not equal, so repeated Bind/Unbind can multiply callbacks and retain panels. TriangulationPanel also has a location-revealed lambda without a matching unsubscribe. Store named delegates or use named handlers, centralize Unbind, and test bind twice / emit once / exit / emit again.

### Fresh/disconnected authorities are routed into panels

Main.PlayerSurfaces.cs constructs a fresh ShelterFireHazardSystem for fire_incident and fresh FactionStanceEngine instances for faction_matrix and factions_narrative. weather_sonde constructs a new WeatherHostSession adapter. These routes can display defaults rather than campaign state. Route to campaign-owned authorities and add identity continuity tests across open, mutation, close, and reload.

### Gameplay determinism is breached by string GetHashCode

FireIncidentPanel.OnTick seeds CoreSeededRng with incidentId.GetHashCode(). MaritimeHostSession uses safeId.GetHashCode() in three safe-attempt/accessibility/loot seeds. .NET string hash values are process-randomized; identical campaigns can diverge between processes. Replace these with StableHash or explicit campaign RNG streams and add cross-process/replay assertions. No production gameplay RNG seed should use string GetHashCode.

### Craft attribution is only superficially wired

TradeSpecialty is loaded and used, but the only observed production caller of Phase0HostSession.CraftItem supplies hardcoded elena_vasquez and machinist from a debug-style button. Pass a typed producer/workstation context through the normal roster/action path and test specialist, non-specialist, unknown, save, and replay behavior.

### Radio station definitions have competing authorities

RadioHostSession invokes RadioStationCatalog.RegisterDefaults, which creates six hardcoded station definitions, while radio.json supplies broadcast content. Migrate station identity/availability to the authoritative data/manifest, validate broadcasts against it, parity-test, and delete RegisterDefaults.

## Current revalidation: release, warning, and generator drift

### Godot/Unity release paths disagree

launch.sh expects Builds/Linux/ASHFALL.x86_64 and contains a Unity batch-mode build command. The Godot CI workflow emits builds/linux/ashfall.x86_64. Rewrite launch.sh around the actual Godot artifact and remove the stale Unity path. Make missing export artifacts fail clearly.

export_presets.cfg is ignored and absent from the current checkout. Track a sanitized preset or generate one from a checked-in template. CI must fail when presets, expected binaries, or embedded/adjacent authority data are missing.

### Warning suppressions hide silent defects

Ashfall.csproj and Ashfall.Core.csproj suppress CS8603 and CS8604 despite comments claiming those warnings are not suppressed, along with broad CS0108, CS0114, CS0067, and related classes. Directory.Build.props presents a different policy. Centralize the warning policy, remove broad suppressions, and justify only narrow Godot lifecycle exceptions.

### Generator checks can mutate the worktree

The core-systems, UI-panel, and expansions generators write their output before comparing it in check mode. Several generated catalogs are currently reported stale. The catalog registry check can pass while undercounting definitions because it relies on a narrow id regex and a stale utilization baseline.

Make all checks read-only, make the registry schema-aware across all identity fields, add generatedFromCommit/generatedAt provenance, regenerate intended outputs, and remove stale hard-coded counts/paths/branch names from reports and hygiene scripts.

## Current revalidated bloat and artifact inventory

- Sentry appears unused as a dependency.
- Ashfall.Core.Tests/TestResults/results.trx is tracked generated output.
- .qwen/tmp is tracked temporary material.
- .codex/skills, .cursor/skills, and .qwen/skills contain broken absolute symlinks and duplicate mirrors.
- assets/quarantine is a large tracked excluded tree; define whether it is an archive, LFS source, or removable material.
- snapshots, gallery_index.json, gallery .import sidecars, and artifacts/ contain generated or presentation material needing a retention/package decision.
- Root Python files include many one-off analyze/dump/extract/reauthor/rewrite scripts. Keep only documented, reproducible authoring tools.
- .gitignore is primarily a Unity template and does not ignore tracked test-result output. Replace it with an explicit Godot/.NET/artifact policy.
- Duplicate basename data files need relative-path identity and a decision about whether both are authoritative.

## Static gates observed during this audit

These completed successfully in the current checkout:

- asset-orphan-sweep.sh
- forbidden-api-gate.sh
- legacy-reference-gate.sh
- legacy-asset-path-gate.sh
- persistent-filename-gate.sh
- nuget-dependency-gate.sh
- json-schema-policy-gate.sh
- doc-link-gate.sh
- scene-lint.py
- run-gates.py --tier fast --check-only
- lfs-health-check.sh
- generator checks for architecture map, save-store matrix, and docs index

The following were stale/failing or require repair:

- core-systems, UI-panel, and expansions generator checks report drift and currently write during check mode;
- agent-skills and audio catalog checks report stale output;
- catalog-registry passes but its counting/provenance model is not trustworthy;
- dotnet/Godot build/test/headless/export execution was not possible because the tools were unavailable.

A green static gate does not override the runtime blind spots above. Complete the toolchain baseline and rerun all build, test, headless, export, recursive data, liveness, and deterministic replay checks before deleting feature islands.

## Final deletion rule

For every candidate, capture source references, test references, loader/manifest entry, data files and IDs, save involvement, player routes, package inclusion, and replacement/owner. First remove the route or package inclusion, then run the full gate set for one regression cycle, then delete code/data/artifacts in reviewable commits. Every deletion should leave a machine-readable disposition and an anti-regression gate.
