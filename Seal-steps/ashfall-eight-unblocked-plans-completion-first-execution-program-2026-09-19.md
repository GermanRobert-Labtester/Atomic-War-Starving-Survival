# ASHFALL — Eight Unblocked Plans: Completion-First Execution & Integration Program

**Date:** 2026-09-19
**Series:** Seal-steps integration programs (successor in genre to
`ashfall-fifteen-unblocked-partial-integrations-completion-first-full-integration-and-enhanced-expansion-program.md`).
**Repo state inspected:** branch `Zcode_Branch`, HEAD `fc73a306`
(2026-09-19 02:27) plus the current uncommitted completion-first session
(166 dirty files verified untouched by this document's author).
**Upstream audit:** `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` — this
program is the execution-grade follow-up to that audit's §3 frontier.
**Document role:** planning authority only. Nothing in this file edits code,
data, ledgers, or claims. Every package below names the evidence a builder
must re-verify before editing, per AGENTS.md Rule 7 ("use current evidence").

---

## 0 · Program summary card

| Field | Value |
|---|---|
| Roster size | 8 integration packages (all signature-free) |
| Inherited completions this wave builds on | 6 of the 15 completion-first plans + 10 debt seals |
| Queue baseline | `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` (117→112 census drain; 8 available) |
| Recommended first moves | Wave D1 truth lane (Plan 01, Plan 02) — read-only P0s today |
| Companion document | `Seal-steps/ashfall-enhanced-expansion-program-en-01-en-08-and-xp-pillar-rescoping-2026-09-19.md` (EN layer + XP re-scoping) |
| Authorization status | proposals only — claim + premise evidence per package before any edit |
| Production changes made by this document | none |

Reading paths by role: **foreman** — §0, Part B (roster), Part D
(decision queue), Part F (signature lines). **builder** — Part C (your
one package), Part E (sequencing + claim template), Part G (verification
pattern). **cheap sweep agent** — any P0 phase (all read-only) plus Part H
citations to re-verify. **integrator** — Part A.5/A.8 ownership routing,
Part D owner-routed ledger edits, G.5 exit criteria.

---

## How to read this document

- **Part A** establishes current reality: the ten debt seals, the executed
  completion-first plans, the measured queue, and the active ownership map.
- **Part B** explains the selection, lists the eight roster entries with
  their unblocking cause and entry gate, and names the deliberate exclusions.
- **Part C** is the integration program proper: one implementation-ready plan
  per roster entry, each following the ashfall-plan contract (current
  reality, delta, ownership, state/save/determinism, host wiring, failure
  modes, phases, file impact map, risks, rollback, handoff).
- **Part D** lists the decision-gated tier: work that exists, is scoped, and
  waits only on a named foreman signature — so the decision queue is visible.
- **Part E** sequences everything (dependency DAG, wave order).
- **Part F** consolidates every foreman line this program needs.
- **Part G** closes with program-level verification, rollback, out-of-scope
  list, and the final implementation handoff.
- **Part H** is the evidence index (file:line citations used in Part C).

**Authority compliance baked into every plan below:**

1. Godot is authoritative; Core (`Assets/Ashfall.Core/`) stays engine-free.
2. JSON in `Assets/StreamingAssets/Data/` stays the sole authored authority.
3. One authority per concern: every plan extends the verified current owner;
   no plan creates a parallel ledger, registry, save store, or manager.
4. Deterministic Core behavior uses the existing seeded RNG contract; no
   wall-clock or hash-order seeding anywhere.
5. Signed decisions are respected, not reversed: DEC-05 (restock priority)
   is ratified, not re-decided; DEC-06 (signal-trust availability consumer)
   stays retired; XP-09/XP-10 remain premise-check-first against the retired
   DEC-17/DEC-18 rows.
6. Panels present existing commands and truthful state; they never recompute
   Core outcomes.
7. A green compile is not integration: each plan's definition of done
   includes the observable outcome, save/restore where stateful, and focused
   tests.
8. No full-suite run by default; focused verification per `TEST_POLICY.md`
   (`bash scripts/run_test.sh <target>`, 180-second cap) plus the named
   headless selftests only.

---

# Part A — Current Reality (verified 2026-09-19)

## A.1 The local game directory

The inspected game directory is
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. Layout that
matters to this program:

| Area | Role |
|---|---|
| `Assets/Ashfall.Core/` | engine-free domain logic (netstandard2.1) |
| `src/` | Godot host: `Main.*.cs` orchestrator partials, host sessions, panels |
| `Ashfall.Core.Tests/` | Core contract tests (net9.0) |
| `Assets/StreamingAssets/Data/` | authoritative authored JSON catalogs |
| `Seal-steps/` | wave/integration plan documents (this series) |
| `docs/plans/`, `docs/governance/`, `docs/radio/`, … | evidence, closeouts, decisions |
| `C-integration-plans/` | 131-file historical corpus (census-managed) |
| `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `KNOWN_DEBT.md`, `TEST_POLICY.md` | live ledgers |

## A.2 The ten debt seals (2026-09-18 → 2026-09-19) — the unblocking wave

Each was verified in source during the upstream audit; tests re-run
2026-09-19 are marked **(re-verified)**:

| # | Debt sealed | Evidence |
|---|---|---|
| 1 | `DEBT-PLAN24-MEDICAL-WARD-STAFFING` | `duty_roles.json` `ward` role, `MedicalWardSystem.StaffingPreflight`, binding in `src/Main.Medical.cs`; `MedicalWardSystemTests` 14/14; `IntegrityScratchFixture.cs:56` seeds `ward`; `PLAN_24_CLOSEOUT.md` header: CLOSED |
| 2 | `DEBT-PLAN28-MAIN-CONSTRUCTOR-MIGRATION` | `SubsystemDescriptor.SetupAction` + `ExecuteSubsystemManifestBootstrap` (`src/Main.Lifecycle.cs:542`), invoked from `RestoreAllSubsystemsFromDisk()` (`src/Main.SaveOrchestrator.cs:163`) |
| 3 | `DEBT-PLAN30-CONSEQUENCE-REACH` | `WireFactionWarConsequenceRouting()` in `src/Main.YearOfAsh.cs`: clash/decree → radio intercept + journal + sound-ranging; stage/chain events routed in the 2026-09-19 session |
| 4 | `DEBT-PLAN30-RUNTIME-CLOCK` | `FactionWarChainRunner.ToAuthoredDay` maps playable 180→authored 480 (offset 300); `FactionWarClockTests` 1/1 **(re-verified)** |
| 5 | `DEBT-PLAN32-MAP-ORPHANS` | 10 authored `loc_*` stubs in `locations.json`; loader gate `AllMapNodes_ExistInLocationsCatalog` |
| 6 | `DEBT-PLAN32-GRAPH-TRAVEL` | expeditions consume map distance + Unknown-fog dispatch refusal; caravans expand hops through `WastelandMapSystem.PlanRoute` (`Assets/Ashfall.Core/World/WastelandMapSystem.cs:581`); trade-network `travel_days` overlay; `WildlifeMapOverlayTests` 1/1 **(re-verified)** |
| 7 | `DEBT-PLAN34-DIFFICULTY-CHRONICLE-AUTHORITY` | completion-history schema v2 stamps `difficultyPresetId` (`Assets/Ashfall.Core/Endgame/CampaignCompletionHistory.cs:44`); `CampaignCompletionHistoryTests` 11/11 **(re-verified)** |
| 8 | `DEBT-PLAN36-PORT-CONTRACT-CLOSURE` | `generate-port-contract.py --check` PASS: 262 seams, 180 HOST_REQUIRED, **0 DEFERRED** **(re-verified)**; `BindCraftResultGate` bound via `CraftingHostSession.Create`; spiritual coordinator wired (DX-02 closed) |
| 9 | `DEBT-PLAN123-SOUND-RANGING-PRODUCER` | `FactionWarSystem` per-strike `HostileFireObservation` → `SoundRangingHostSession.RecordHostileFire` |
| 10 | `DEBT-PLAN125-SOFC-INVENTORY-FUEL` | `ConsumeSofcFuel` (clean→treated→dirty canister tiers, grid-reserve fallback) in `src/Main.Plans122to125.cs` |

Additional completed work verified outside the debt ledger:

- **Plan 26A tranche-2 is complete**: `CatalogPathForbiddenGateTests` allowlist
  is now only the authority itself (`src/Host/CatalogPath.cs`); 2/2 **(re-verified)**.
  Observation carried forward: two lowercase `res://assets/StreamingAssets/Data`
  references (`src/Audio/AudioCueCatalog.cs:407`,
  `src/Main.FlagshipInstitutions.cs:45`) escape the gate's ordinal
  `Assets/StreamingAssets/Data` pattern — the first truth-lane package
  (Plan 01 below) re-checks them.
- **Plan 24 is CLOSED** with only the environment-blocked snapshot
  rebaseline recorded as residual (renderer-capable session required).
- **The active batch is XP Expansion W1** (`XP-WAVE1-DIFFICULTY-AUTHORITY`):
  catalog/director/provider shipped, host setup resolves the default preset,
  one consumer is live, completion-history stamps the preset id. The full
  binding is roster entry 5 below.

## A.3 Completion-first roster scorecard (inherited from the fifteen program)

| Roster # | Package | Verdict at 2026-09-19 |
|---|---|---|
| 01 | `CF-P24-CLOSURE` | **EXECUTED** (closeout CLOSED; snapshot rebaseline environment-blocked) |
| 06 | `CF-P30-WAR-PROJECTION-CONSUMERS` | **EXECUTED** (clock + routing + wildlife overlay) |
| 07 | `CF-P32-GRAPH-TRAVEL` | **EXECUTED** (expedition + caravan + trade network + fog gating; aviation/naval residual documented) |
| 08 | `CF-P34-DIFFICULTY-CHRONICLE` | **EXECUTED** |
| 09 | `CF-P36C-PORT-SEAM-SWEEP` | **EXECUTED** (0 DEFERRED) |
| 11 | `CF-P26A-FORBIDDEN-PATH-SWEEP` | **EXECUTED** (tranche-2 complete) |
| 02, 03, 04, 10, 13 | — | **AVAILABLE** — the first five entries of this program |
| 05, 12, 14, 15 | — | **DECISION-BLOCKED** (Part D) |

## A.4 The measured queue

Per the upstream audit: the census drain moves from **117 → 112 nonterminal
rows** (111 `AUDIT-PENDING` + 1 `READY-UNCLAIMED`) once anchors C2[9]–C2[13]
flip to `SEALED`; **8 plans are available for integration now** with no new
foreman signature (2 of them behind the standard premise audit). The
companion document
`Seal-steps/ashfall-enhanced-expansion-program-en-01-en-08-and-xp-pillar-rescoping-2026-09-19.md`
expands the EN-01…EN-08 enhancement layer and the XP pillar re-scoping on
top of this program.

## A.5 Active ownership map (read before any claim)

| Active claim | Owns | Constraint on this program |
|---|---|---|
| `claim-xp-wave1-difficulty-2026-09-18` | `Assets/Ashfall.Core/Difficulty/`, `difficulty_presets.json`, `Ashfall.Core.Tests/Difficulty/`, `docs/plans/xp/w1/` | Roster entry 5 (XP-01 full binding) executes inside this active claim or a successor claim; the wave11-owned completion-history paths are explicitly excluded |
| `claim-wave11-part2-execution-2026-09-18` | `INTEGRATION_PLANS.md`, `KNOWN_DEBT.md`, `DECISION_REGISTER.md`, census, `docs/plans/wave11_part2/`, completion-history + port-policy surfaces | Ledger edits in entry 2 route through this owner (or a user-authorized transfer); rule 6: claimed paths are read-only to everyone else |
| Unclaimed | everything else this program touches | normal claim discipline in `WORKTREE_OWNERSHIP.md` |

## A.6 Premise corrections already banked (do not re-litigate)

- **XP-05 SOFC fuel is live** (`ConsumeSofcFuel`); no duplicate fuel catalog.
- **Difficulty starter items** are `canned_food` / `iodine_pills` (the
  proposal's `item_canned_rations` / `item_iodine` never existed).
- **The war clock is an authored-day mapping** (`ToAuthoredDay`), not an
  uncapped `_simDay`; chains/communiqués fire inside the Year-of-Ash window.
- **The restock priority implementation is live** in production
  (`ShelterBarterSystem.ComputeItemPriorityScore`, `Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs:283`);
  the remainder is ledger truth, not code.
- **The corpus plan files predate the execution wave**: any corpus-derived
  plan (entries 7 and 8) must pass its premise audit before the first edit.

## A.7 Verification baseline (from the 2026-09-19 audit)

| Command | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan147RestockPriorityTests.cs` | 6/6 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/FactionWarClockTests.cs` | 1/1 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Endgame/CampaignCompletionHistoryTests.cs` | 11/11 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/World/WildlifeMapOverlayTests.cs` | 1/1 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/CatalogPathForbiddenGateTests.cs` | 2/2 PASS |
| `python3 scripts/ci/generate-port-contract.py --check` | PASS — 262 seams, 180 HOST_REQUIRED, 0 deferred |

## A.8 Census anchor sealing evidence (the five rows that leave the queue)

| Anchor | Corpus title | Sealing evidence (verified) | Census action owed |
|---|---|---|---|
| `C2[9]`/Plan 28 | Orchestration Spine | 28A manifest (Core, `SubsystemManifest.cs`, 6/6 tests); 28B decomposition sealed elsewhere; 28C constructor migration sealed (`Main.Lifecycle.cs:542`, `Main.SaveOrchestrator.cs:163`) | flip to `SEALED`, cite this program's Plan 04 for the one-path residual |
| `C2[10]`/Plan 30 | Autonomous Outside World | clock (`ToAuthoredDay`, `FactionWarClockTests` 1/1), consequence reach (radio/journal/sound-ranging), stage/chain routing, wildlife overlay (`WildlifeMapOverlayTests` 1/1) | flip to `SEALED` |
| `C2[11]`/Plan 32 | One Map, Three Notions of Place | orphan stubs + loader gate; expedition/caravan graph travel + Unknown-fog gating; trade-network overlay | flip to `SEALED`; aviation/naval residual recorded as debt-free documentation only |
| `C2[12]`/Plan 34 | The Long Arc | difficulty authority live (XP-01); completion-history v2 stamps `difficultyPresetId` (`CampaignCompletionHistoryTests` 11/11) | flip to `SEALED` |
| `C2[13]`/Plan 36 | The Port Contract | `generate-port-contract.py --check`: 262 seams, 180 HOST_REQUIRED, 0 DEFERRED; `BindCraftResultGate` bound; DX-02 closed | flip to `SEALED` |

The census file itself is inside `claim-wave11-part2-execution-2026-09-18`'s
exact paths; builders cite this table, the claim owner flips the rows.

## A.9 Builder hygiene rules for this program

1. **Read `WORKTREE_OWNERSHIP.md` immediately before each claim** — two
   claims are ACTIVE (§A.5) and shared roots are sole-active-builder paths.
2. **Preserve the 166 in-flight dirty files** — they are the uncommitted
   2026-09-18/19 session; never revert, reformat, or fold them into a
   package commit.
3. **Generated files are regenerated, never hand-edited** (`docs/INDEX.md`,
   `PORT_CONTRACT.md`, architecture map, registries).
4. **Focused verification only** — `bash scripts/run_test.sh <target>`
   (180-second cap) plus named selftests; a broader run needs a new
   hypothesis and an explicit reason (TEST_POLICY.md).
5. **Godot headless at 15 FPS** if a runtime session is needed.

---

## A.10 Selftest inventory relevant to this program

| Selftest verb | Gates | Used by |
|---|---|---|
| `--data-integrity-selftest` | catalog validation incl. difficulty + distress rules | Plans 01, 03, 05 |
| `--content-utilization-selftest` | 0 orphans | Plan 01 |
| `--audio-selftest` | cue registry | Plan 01 |
| `--vehicle-garage-selftest` | garage surface (19/19 baseline) | Plan 03 |
| `--panel-bind-lifecycle-selftest` | panel bind/unbind ×100 | Plans 03, 05, 07 |
| `--player-panels-uitest` | panel routes | Plans 04, 07 |
| `--7day-smoke-selftest` | campaign smoke (10/10 baseline) | Plan 04 |
| `--settings-selftest` | settings incl. a11y flags | Plan 07 |
| `--ui-layout-selftest` | layout/legibility | Plan 07 |
| `--endings-selftest` | endings + completion history | Plan 05 |
| `--difficulty-selftest` | (new — spec C.5.11) | Plan 05 |
| export parity + performance | ship gates | Plan 08 |

Baseline counts were green at the 2026-09-19 audit; each package re-runs
its own subset at claim time and records the results in its log.

---

# Part B — Selection

## B.1 Selection rule

A roster entry must satisfy all four:

1. **Not yet executed** — no sealed closeout exists for the named remainder.
2. **Unblocked** — every recorded blocker (debt, dependency, owner gap) is
   verifiably gone, or the item is a pure verify-and-seal whose evidence
   already exists.
3. **No new foreman product/schema/design signature is required** — the
   decision, where one was needed, is already signed (e.g. DEC-05), or the
   item is bounded engineering (claim + premise note).
4. **Governance-safe** — it extends a current owner, reverses no signed or
   retired decision, and respects the active claim map (§A.5).

## B.2 The roster

| # | Package | Anchor | What remains | Unblocked by | Entry gate |
|---|---|---|---|---|---|
| 01 | `CF-P1-DISTRESS-CONTENT-SEAL` | Distress P1 (Waves 3–5 contracts) | 3 validator rules, population replay, audio-cue registry verification, PR3 closeout | content shipped (17+7 follow-ups, 66+36 cues) | none (verify-and-seal) |
| 02 | `CF-P5-RESTOCK-RECONCILE` | Merchant restock P5 / DEC-05 | ledger truth across three ledgers; production binding re-verified | DEC-05 signed; `ComputeItemPriorityScore` live; tests 6/6 | foreman ratification line (wording only) |
| 03 | `CF-P6-VEHICLE-ARMOR-GRADES` | Plan 213 D6 / P6 | 4 armor grade tiers unimplemented | vehicle owner + decoration seam landed (`--vehicle-garage-selftest` 19/19) | new claim + premise note |
| 04 | `CF-P28-ONE-BOOTSTRAP-PATH` | Plan 28 / C2[9] 28C residual | manifest bootstrap runs only on the restore path | setup-action migration sealed 2026-09-18 | none (bounded host change) |
| 05 | `CF-XP01-DIFFICULTY-FULL-BINDING` | XP-01 (active W1) | preset selection, persistence, 7 remaining scalar consumers, panel, selftest | W1 authorization (D1 signed, narrowed) | per-consumer premise checks |
| 06 | `E1-PLAN53-AMBITION-GOVERNANCE` | census `READY-UNCLAIMED` | E1A–E1P governance programme | prerequisite Plan 29 sealed (C1[7]) | claim per census protocol |
| 07 | `C2[15]-PLAN37-INPUT-REALITY` | Plan 37 (37A→37B→37C) | every declared action real, focus order, controller parity, rebinding | declared prerequisites sealed (C2[9], C1[8]); corpus dependency audit outstanding | **premise audit first** |
| 08 | `C2[21]-PLAN48-RELEASE-CRAFT` | Plan 48 (48A→48B→48C) | compatibility policy, tagged release path, hotfix rehearsal | prerequisite C1[7] sealed; corpus dependency audit outstanding | **premise audit first** |

## B.3 Premise-audit protocol (mandatory for entries 07 and 08; recommended for all)

The corpus plan files predate the 2026-09-18/19 execution wave. The audit
below is the entry gate for any corpus-derived plan; it is read-only and
produces a verdict table that becomes the plan's P0 artifact:

| Step | Check | Pass condition | Fail action |
|---|---|---|---|
| 1 | Header contract | order + hard gate + dependencies re-read from the corpus file at HEAD | record divergence |
| 2 | Per-dependency status | each named dependency mapped to a sealed/audited/blocked verdict with evidence | a blocked dependency ⇒ plan stays queued, do not improvise |
| 3 | Gap re-verification | every stated gap grepped/verified against current source (the wave may have closed or widened it) | stale gap ⇒ strike it with evidence; new gap ⇒ add it |
| 4 | Owner map | the current owner of every touched concern named (file:line) | unknown owner ⇒ stop, report (Rule 10) |
| 5 | Save/determinism surface | state, capture/restore, RNG streams, checksum culture re-verified | unsaved mutable state ⇒ design a save seam or stop |
| 6 | Claim overlap | exact paths diffed against every ACTIVE claim | overlap ⇒ route through the owner |
| 7 | Test surface | existing focused files + selftests that will gate the work | missing gate ⇒ add one to the plan's delta |
| 8 | Blast radius | file impact map drafted and sized | shared-root edits ⇒ sequence, never parallel |
| 9 | Acceptance | definition of done written as observable outcomes | cannot state an outcome ⇒ the plan is not ready |
| 10 | Verdict | `READY` / `NEEDS-RESCOPE` / `STILL-BLOCKED` recorded with evidence | any verdict other than READY returns to the queue |

The XP-05 precedent is the standing warning: a proposal premise
(`FuelConsumer = units => true`) was already false at execution time; the
wave log records the correction instead of bending the plan.

## B.4 Deliberate exclusions (with reasons)

- **`CF-P3-SEMANTIC-KIND-AUTHORITY` (old roster 05)** — D11 unsigned; the
  pinned `GenericSectionTitle` no-silent-drop contract in
  `DayEventVocabularyTests` must be amended by signature, not by builder.
- **`CF-QD-QUARANTINE-DRAIN-TRANCHE` (old roster 12)** — D21/F11 unsigned;
  48 active `Compile Remove` exclusions remain (count verified 2026-09-19).
- **`CF-XP04-ECONOMY-LEGS` (old roster 14)** — F13 design sign-off.
- **`CF-XP06-BODY-INTEGRITY` (old roster 15)** — F14 schema sign-off
  (DEC-03 successor).
- **EN-01…EN-08 and the XP pillar re-scoping** — proposals; expanded in the
  companion document, not here, to keep this program signature-free.
- **XP-02 / XP-03 / XP-05** — premise-consumed: graph travel, war clock, and
  SOFC fuel landed through canonical owners; the companion document re-scopes
  any legitimate remainder.
- **109 census `AUDIT-PENDING` rows** — audit-only, not integration.
- **Wave 12 Seal-steps parts (1.2/2.2/3.1/3.2, Unblocking-tasks 4–10)** —
  uncertified documents with their own predecessor chains.
- **Plan 24 snapshot rebaseline** — environment-blocked (renderer-capable
  session); appears only as a named step inside no plan here.

---

# Part C — The Eight Integration Plans

Reading key for every plan below: "Current reality" cites file evidence a
builder must re-verify before editing; "Phases" are dependency-ordered with a
completion gate each; "Focused verify" uses `bash scripts/run_test.sh
<target>` (180-second cap) plus the named headless selftests, per
TEST_POLICY.md. No plan here authorizes a full-suite run by default.

---

## Plan 01 — Distress Follow-Up & Audio Content Seal (`CF-P1-DISTRESS-CONTENT-SEAL`)

**Anchor:** Waves 3–5 distress contracts; PR2 content tranche.
**Status:** mechanism sealed and green since 2026-09-13; content committed;
the seal tail (validator rules, population replay, utilization, closeout)
never ran.
**Unblocked by:** content landing in shipped catalogs — this is a
verify-and-seal package, not authoring.

### C.1.1 Current reality (verified)

- `DistressFollowUpScheduler` is sealed Core: closed trigger grammar
  (answered / rescue_success / rescue_failed / expired / trap_fallen_for —
  every value maps to one exactly-once mission transition), self-contained
  payloads (chains/cycles structurally impossible), campaign-day scheduling
  with host-set `CurrentDay`, exactly-once pending + fired ledgers,
  deterministic same-day ordinal ordering, V5→V6 frozen-shape migration.
- `DistressAudioCueResolver` is sealed Core: stage override → signal default
  → text-only fallback, explicit cue ids, zero RNG; playback is
  Intercept-gated with the persisted `playedBroadcastKeys` dedupe
  (`distress:{id}:{cue}`); follow-up transmissions resolve their own cue
  exactly once.
- Catalog content is shipped: `radio_distress_signals.json` carries **17
  `follow_up_signals`** and **66 `audio_cue` occurrences**;
  `radio_distress_signals_expansion.json` carries **7** and **36**.
- `CatalogIntegrityValidator` already validates stage contracts, follow-up
  shape, and a structural cue-id rule. The three deferred PR3 rules are
  absent (grep-verified 2026-09-19): no expired-with-no-consequence rule, no
  trap-grammar-on-genuine-only rule, no max-2-follow-ups-per-signal rule.
- No PR3 closeout document exists in `docs/radio/` (verified: only the
  original `DISTRESS_SIGNAL_TASKS_9_12_CLOSEOUT.md` and the PR2 hint tranche).
- Existing focused surface: `DistressFollowUpTests` (19),
  `DistressAudioCueTests` (10), `DistressSignalTasks912ReplayTests` (5),
  `SignalTrustTests` (21), `RadioSaveMigrationTests` (V6 pin).

### C.1.2 Required delta

1. **Three validator rules** in `CatalogIntegrityValidator` (data-shape only,
   no runtime behavior change):
   - a follow-up with trigger `expired` must define a consequence distinct
     from silence (at minimum a next-stage fragment or a trust/standing
     delta is already implied by the grammar — the rule forbids
     no-consequence expired entries);
   - `trap_fallen_for` follow-ups may not be attached to genuine-only
     signals (the trap grammar belongs to lure-class identities);
   - at most 2 `follow_up_signals` entries per signal definition.
2. **Population replay tests** exercising every authored follow-up chain and
   cue through the real V6 codec: for each of the 24 authored follow-up
   entries, drive its trigger transition and assert the fire day, the
   exactly-once ledger, and the resolved audio cue id (or explicit
   text-only).
3. **Audio-cue id registry verification**: every referenced `audio_cue` id
   either resolves in the host-side registry or is explicitly documented
   text-only per the existing semantic-resolution fallback policy (missing
   cue logs once, text continues, never crashes).
4. **Utilization seal**: `--content-utilization-selftest` PASS with 0 orphans
   over the distress layer.
5. **PR3 closeout document** + `INTEGRATION_PLANS.md` row marking P1 sealed
   (ledger edit routed through the wave11-part2 owner or an authorized
   transfer).

### C.1.3 Ownership & seams

| Concern | Owner (unchanged) |
|---|---|
| Follow-up scheduling / exactly-once | `DistressFollowUpScheduler` |
| Audio cue resolution | `DistressAudioCueResolver` |
| Catalog validation | `CatalogIntegrityValidator` (additive hooks only) |
| Radio persistence | `RadioSave` V6 (no schema change — no version bump) |
| Playback dedupe | `RadioHostSession` `playedBroadcastKeys` |

### C.1.4 State, save, determinism

No save change: the rules are validators, the replay is a test harness over
the existing V6 codec, and the registry check is a host-side verification.
Determinism is inherited: the scheduler is day-keyed and ordinal-ordered;
the replay asserts stable fingerprints.

### C.1.5 Host wiring

None beyond the registry verification command (host CLI already exposes
`--audio-selftest` and `--content-utilization-selftest`).

### C.1.6 Failure modes

A validator rule that fires on currently-shipped rows (the catalogs were
authored before the rules existed) — resolve by fixing the data or amending
the rule's boundary with a documented reason, never by deleting the rule;
replay tests that depend on RNG ordering — the scheduler is deterministic,
so any nondeterminism is a defect to surface, not a flake to pin.

### C.1.7 Phases

- **P0 — Census (read-only):** enumerate all authored follow-up entries and
  cue ids into a fixture table; classify each against the three rules.
  Gate: the table exists and names every row's expected verdict.
- **P1 — Validator rules:** implement the three rules with per-row failure
  messages (catalog/signal/path/id/field/value/rule naming per house style);
  fix any shipped rows that violate them. Gate: data-integrity selftest PASS
  with 0 errors; new validator unit tests green.
- **P2 — Population replay:** one replay test file driving every authored
  chain through the V6 codec. Gate: 24/24 chains exercised, fingerprints
  stable across two runs.
- **P3 — Registry + utilization:** run `--audio-selftest` and
  `--content-utilization-selftest`; document the text-only set.
  Gate: both PASS, 0 orphans.
- **P4 — Closeout:** PR3 document + ledger row. Gate: docs index regenerated
  by the integrator; Radio suite green.

### C.1.8 Test strategy & focused verify

`bash scripts/run_test.sh Ashfall.Core.Tests/Radio/` (full radio dir);
new `DistressFollowUpPopulationReplayTests.cs` runs alone first;
`godot --headless --path . -- --data-integrity-selftest`;
`--content-utilization-selftest`; `--audio-selftest`.

### C.1.9 File impact map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | MODIFY (additive) | 3 rules | low |
| `Assets/StreamingAssets/Data/radio_distress_signals*.json` | MODIFY (data fixes only if P1 fires) | rule compliance | low |
| `Ashfall.Core.Tests/Radio/DistressFollowUpPopulationReplayTests.cs` | NEW | population replay | low |
| `docs/radio/DISTRESS_SIGNAL_PR3_CLOSEOUT.md` | NEW | seal evidence | low |

### C.1.10 Validator rule specifications

The three rules follow the house error format (catalog / signal / path /
id / field / value / rule) and are additive hooks in
`CatalogIntegrityValidator`:

| Rule id | Check | Error shape | Fixture cases |
|---|---|---|---|
| `distress_followup_expired_requires_consequence` | a `follow_up_signals` entry with trigger `expired` must carry a payload that changes observable state (next-stage fragment, trust delta, or authored aftermath line) — silence-with-a-row is forbidden | `radio_distress_signals.json :: <signal_id> :: follow_up_signals[<i>] :: trigger=expired :: no consequence field` | one negative (silent expired), one positive (aftermath line) |
| `distress_followup_trap_only_on_lures` | `trap_fallen_for` follow-ups may attach only to identities whose classification admits traps (lure/trap classes); genuine-only signals never author trap aftermath | `… :: <signal_id> :: follow_up_signals[<i>] :: trigger=trap_fallen_for :: signal is genuine-only` | genuine+trap ⇒ error; lure+trap ⇒ pass |
| `distress_followup_max_two` | at most 2 `follow_up_signals` entries per signal definition | `… :: <signal_id> :: follow_up_signals :: count=<n> :: max=2` | 3 entries ⇒ error; 2 ⇒ pass |

Rule order note: run after the existing structural follow-up validation so
the grammar is already proven when these semantic rules fire. Shipped-row
remediation (if any row fails) is a data fix in the same commit as the
rule — never a rule relaxation.

### C.1.11 Population replay matrix

| Chain class | Authored rows | Replay assertion | Fingerprint fields |
|---|---:|---|---|
| answered → follow-up | 17 (primary catalog) | fire day = trigger day + authored delay; exactly-once fired ledger; cue resolved | pending/fired ledgers, trust, cue id |
| rescue_success → aftermath | subset | same, plus mission terminal state | mission ledger, trust |
| rescue_failed → aftermath | subset | same | mission ledger, trust |
| expired → consequence | subset | consequence observable (per rule 1) | trust / journal key |
| trap_fallen_for | expansion subset | suppression of genuine follow-ups; trap aftermath fires once | fired ledger, trust |
| follow-up audio cue | 66+36 occurrences | resolved id ∈ registry ∪ {text-only}; playback dedupe key stable | playedBroadcastKeys |

Two consecutive runs of the whole matrix must produce identical
fingerprints (determinism guard for the harness itself).

**Out of scope:** any new follow-up content tranche; any save version bump;
any runtime scheduler change.

### C.1.12 PR3 closeout document outline

`docs/radio/DISTRESS_SIGNAL_PR3_CLOSEOUT.md` must contain: (1) scope — the
three rules with their fixture counts; (2) the P0 census table verbatim;
(3) population replay results (chains exercised, fingerprints, two-run
stability); (4) audio registry verification (resolved ids, text-only set,
missing-cue policy confirmation); (5) content-utilization result; (6) any
data fixes with before/after; (7) the ledger row diff; (8) limitations
(what remains un-authored, e.g. future content tranches). The Wave 5
closeout (`DISTRESS_SIGNAL_TASKS_9_12_CLOSEOUT.md`) is the structural
precedent — mirror its section order so the radio trilogy reads uniformly.

**Handoff — MUST PRESERVE:** the exactly-once ledgers, V6 save shape,
Intercept-gated playback. **MUST ADD:** validators + replay + closeout only.
**MUST NOT DO:** relax a rule to make data pass silently; invent a second
audio registry. **VERIFY WITH:** §C.1.8. **FIRST SAFE STEP:** P0 census
(read-only, one cheap agent, immediately useful).

---

## Plan 02 — Merchant Restock Priority Reconcile-and-Complete (`CF-P5-RESTOCK-RECONCILE`)

**Anchor:** `DEC-05` (SIGNED), `Plan147RestockPriorityTests`, the
`FOLLOWUPS-210-213` row in `INTEGRATION_PLANS.md`.
**Status:** implementation live and green; ledgers disagree.
**Unblocked by:** DEC-05 already signed; nothing to decide.

### C.2.1 Current reality (verified)

- Production binding is live: `ShelterBarterSystem.ComputeItemPriorityScore`
  (`Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs:283`) is consumed by
  the restock sort (`:307-316` inside `RestockCaravan`), with the
  `PriorityScorer` extension seam (`:136`) and restock call sites at `:273`
  and `:532`.
- `Plan147RestockPriorityTests` passes **6/6** (re-run 2026-09-19). The
  decision register's DEC-05 evidence field still says "14/14 PASS" — count
  drift, verdict unchanged.
- `INTEGRATION_PLANS.md` (FOLLOWUPS-210-213 row) still says merchant restock
  priority is "Still deferred with authority question" — stale against the
  signed DEC-05 and the live code.
- `KNOWN_DEBT.md` carries no open restock row (the wave9_part2 C1 seal is
  recorded as closed).

### C.2.2 Required delta

1. **One foreman ratification line** (F2): ratify DEC-05's wording across
   the three ledgers — priority-weighted tier restock with deterministic
   PRNG; trade ledger updates canonically.
2. **Ledger truth pass:** update the `INTEGRATION_PLANS.md` row (deferred →
   sealed, with evidence pointer), the register's DEC-05 evidence field
   (14/14 → 6/6 with a drift note), and confirm no KNOWN_DEBT row is owed.
3. **Binding re-verification note:** record in the reconcile that the score
   is consumed on the arrival/restock path (call sites `:273`/`:532`) and
   that the stay-pinned stock semantics (restock evaluated once per arrival,
   stock pinned for the stay) are unchanged.

### C.2.3 Ownership & seams

| Concern | Owner |
|---|---|
| Restock priority arithmetic | `ShelterBarterSystem` (unchanged) |
| Ledger truth | foreman/integrator only — `INTEGRATION_PLANS.md` and `DECISION_REGISTER.md` are inside `claim-wave11-part2-execution-2026-09-18`'s exact paths |

### C.2.4 State, save, determinism

None: zero production change. The existing determinism contract (seeded
`SeededRng(42)` fixtures; campaign-forked restock RNG in production) is
already pinned by the tests.

### C.2.5 Failure modes

A builder editing the claimed ledgers without the owner (rule 6) — route the
edit through the integrator or obtain a user-authorized transfer; recounting
drift (any future count change must update the register evidence field in
the same commit).

### C.2.6 Phases

- **P0 — Re-verify:** re-run `Plan147RestockPriorityTests` (6/6) and re-grep
  the production consumers (`:283`, `:307-316`, `:273`, `:532`).
  Gate: evidence re-confirmed at the claiming HEAD.
- **P1 — Ratify:** obtain the foreman line (F2).
  Gate: the line is recorded verbatim where the reconcile lands.
- **P2 — Reconcile:** apply the three ledger edits.
  Gate: `python3 scripts/ci/generate-docs-index.py --check` after the
  integrator regenerates; no other row touched.

### C.2.7 Test strategy & focused verify

`bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan147RestockPriorityTests.cs`
(6/6). Nothing else — this package reconciles truth, it does not add
behavior.

### C.2.8 Ledger-row reconciliation spec (before → after)

| Ledger | Current text (stale) | Reconciled text |
|---|---|---|
| `INTEGRATION_PLANS.md` FOLLOWUPS-210-213 row | "**Still deferred with authority question:** merchant restock 'priority' … a priority ordering needs a signed design" | "**Sealed (DEC-05):** priority-weighted tier restock live — `ComputeItemPriorityScore` consumed by `RestockCaravan`; `Plan147RestockPriorityTests` 6/6; evidence …" |
| `DECISION_REGISTER.md` DEC-05 evidence field | "`Plan147RestockPriorityTests.cs` (14/14 PASS)" | "(6/6 PASS; count drift 14→6 recorded 2026-09-19, verdict unchanged)" |
| `KNOWN_DEBT.md` | no open row | no row owed — confirmed, not invented |

One more ratification sub-item rides F1: the wave9_part2 C1 decision memo's
blank signature block (decision packet G3) is back-filled with the DEC-05
ratification line so the register, memo, and ledger agree.

### C.2.9 Failure-mode table

| Failure | Class | Mitigation |
|---|---|---|
| Builder edits claimed ledgers directly | HIGH | owner-routed edit or user-authorized transfer (Rule 6) |
| Future count drift re-breaks the register | LOW | any count change updates the evidence field in the same commit |
| Re-verification finds the binding regressed | HIGH | stop the reconcile; the package becomes a repair package, not a truth pass |

### C.2.10 File impact map

| File | Action | Reason | Risk |
|---|---|---|---|
| `INTEGRATION_PLANS.md` | MODIFY (one row) | stale deferred wording | low (owner-routed) |
| `docs/governance/DECISION_REGISTER.md` | MODIFY (one evidence field) | count drift | low (owner-routed) |

**Out of scope:** any restock behavior change; any new priority feature.

**Handoff — MUST PRESERVE:** `ComputeItemPriorityScore` purity and the
`PriorityScorer` seam. **MUST ADD:** ledger truth only. **MUST NOT DO:**
re-decide DEC-05 or edit ledgers outside the owning claim.
**VERIFY WITH:** §C.2.7. **FIRST SAFE STEP:** P0 (one command + one grep).

---

## Plan 03 — Vehicle Armor Decoration Grades (`CF-P6-VEHICLE-ARMOR-GRADES`)

**Anchor:** Plans 210–213 authority map §4 D6 (approved deferral), Plan 50
vehicle garage seam, Plan 213 metallurgy profiles.
**Status:** 4 armor grade tiers unimplemented; every prerequisite owner is
live.
**Unblocked by:** the D6 deferral condition ("no vehicle owner exists") is
false — `VehicleGarageSystem` + decoration seam + recovery missions +
`VehicleGaragePanel` shipped with `--vehicle-garage-selftest` 19/19.

### C.3.1 Current reality (verified)

- `VehicleGarageSystem` (Core) owns per-vehicle customization records
  (`VehicleCustomizationRecord`: `installedSlots`, `chassisStressPermille`,
  `engineFoulingPermille`, `transmissionWearPermille`, `isImmobilized`,
  `immobilizedReason`), recovery missions, `DecorateProfile`,
  `AdvanceRecoveries`, `GetInstalledSlots`, and the immobilized-dispatch
  gate consumed by `ExpeditionHostSession`.
- The Plan 50 decoration seam is complete: mod effects decorate the
  expedition profile, trip distance feeds component wear, immobilized
  vehicles are refused dispatch, recovery advances over campaign days.
- No armor-grade implementation exists anywhere (grep for
  `armor_grade|ArmorGrade|armorGrade` across `Assets/` + `src/`: zero hits).
- Precedents to copy: the run-flat install kits (compatible-only install,
  workshop + real inventory gate/consume, bounded hazard reduction never
  immunity, save round-trip) and the Plans 146–149 additive
  `InstalledCoatedPartItemIds` field pattern.

### C.3.2 Required delta

1. **Authored grade catalog** (`vehicle_armor_grades.json` — 4 tiers, e.g.
   `grade_0_stock` / `grade_1_plates` / `grade_2_composite` /
   `grade_3_slayer`): per-grade bounded damage-reduction and
   wear-decay multipliers (permille, clamped), install cost (existing item
   ids + workshop time), and a display name. Strict loader following the
   `GoodsCatalogLoader` pattern (schema envelope, snake_case, nullable DTOs,
   collected errors) + `CatalogIntegrityValidator` hook (references must
   resolve to real items and to the vehicle slot vocabulary).
2. **Core model**: additive `armorGradeId` on `VehicleCustomizationRecord`
   (legacy default = stock grade, parity 1.0); `VehicleGarageSystem` exposes
   a pure `GetArmorProfile(vehicleId)` read model consumed by
   `DecorateProfile` so the expedition estimate and runtime share one
   sampled grade (the established share-the-sampled-value pattern).
3. **Install/upgrade command** through the existing garage command surface:
   compatible-only install, workshop + real inventory gate/consume,
   never immunity — bounded mitigation only, and severe incidents can still
   damage components (hazard reduction never zero).
4. **Panel strip** on `VehicleGaragePanel`: grade name, mitigation
   percentage, next-tier cost; words and numbers, never color-only.
5. **Save**: additive field with legacy default; capture/restore round-trip
   test; old saves deserialize to stock grade.
6. **Selftest extension**: `--vehicle-garage-selftest` cases for install,
   mitigation math, dispatch parity at stock grade.

### C.3.3 Ownership & seams

| Concern | Owner |
|---|---|
| Vehicle records / wear / recovery | `VehicleGarageSystem` (extended, not forked) |
| Expedition consumption | `ExpeditionHostSession` garage seam + `ExpeditionSystem` estimate fields |
| Install economy | canonical `Inventory` (AddById/TryConsume) + workshop duty |
| Panel | `VehicleGaragePanel` |
| Persistence | existing vehicle garage save path (additive field) |

### C.3.4 State, save, determinism

One additive state field; no new save section; no RNG in grade effects
(pure multipliers — deterministic by construction); grades sampled at
dispatch like the other multipliers so estimate == runtime.

### C.3.5 Failure modes

Grade stacking exploits (cap at one grade per vehicle, install replaces and
consumes); estimate/runtime divergence (share the sampled grade, add the
parity guard); double-charging on interrupted installs (atomic
gate/consume like the run-flat kits).

### C.3.6 Phases

- **P0 — Premise note + claim:** record that D6's condition is false, name
  the four tiers, claim exact paths. Gate: claim row exists.
- **P1 — Core + catalog + validator:** grade catalog, loader, validator
  hook, read model, record field. Gate: catalog tests + validator green;
  data-integrity PASS.
- **P2 — Host + persistence:** install command, save additive field,
  round-trip + legacy parity tests. Gate: garage tests green; save
  round-trip exact.
- **P3 — Panel + selftest:** strip + selftest cases. Gate: panel lifecycle
  + a11y PASS; `--vehicle-garage-selftest` extended and green.
- **P4 — Balance soak:** 30-day seeded soak asserting mitigation bounds and
  dispatch parity at stock. Gate: monotonic, bounded, no underflow.

### C.3.7 Test strategy & focused verify

`bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs`
(+ new `Plan213VehicleArmorGradeTests.cs` alone first);
`godot --headless --path . -- --vehicle-garage-selftest`;
`--panel-bind-lifecycle-selftest`; `--data-integrity-selftest`.

### C.3.8 File impact map

| File/area | Action | Risk |
|---|---|---|
| `Assets/StreamingAssets/Data/vehicle_armor_grades.json` | NEW | low |
| `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs` | MODIFY (additive) | medium (shared with recovery logic) |
| `src/Host/ExpeditionHostSession.cs` | MODIFY (profile consumption) | medium |
| `src/UI/VehicleGaragePanel.cs` | MODIFY (strip) | low |
| `src/Host/HostCli.VehicleGarage.cs` | MODIFY (selftest cases) | low |

### C.3.9 Risk table & balance bounds

| Risk | Class | Evidence | Mitigation |
|---|---|---|---|
| Grade stacking (multiple grades applied) | HIGH | install replaces, not adds | single `armorGradeId` field; install consumes the prior kit |
| Estimate/runtime divergence | HIGH | precedent: weather/amputation multipliers share a sampled value | sample once at dispatch; parity test |
| Wear-chain double-count | MEDIUM | `DecorateProfile` already multiplies component wear | grade affects the damage path only, never the wear path, unless the authored grade says so — one seam, one owner |
| Immunity illusion | MEDIUM | run-flat precedent: “hazard reduction never immunity” | cap mitigation ≤ authored maximum (< 100%); severe incidents still damage |
| Legacy save drift | LOW | additive field precedent (Plans 146–149) | legacy default stock grade; round-trip test |

Balance bounds for the 30-day soak: mitigation strictly monotonic across
grades; dispatch parity at stock grade (fingerprint equality); no
underflow/overflow in wear permille; install cost consumed atomically.

### C.3.10 Acceptance / definition of done

| # | Outcome | Proof |
|---|---|---|
| 1 | 4 authored tiers load, validate, and cross-reference cleanly | catalog tests + data-integrity selftest |
| 2 | Install/upgrade consumes real items through the canonical inventory | inventory assertions |
| 3 | Mitigation bounded, monotonic, never immunity | soak + unit bounds |
| 4 | Dispatch estimate ≡ runtime at every grade | parity test |
| 5 | Old saves deserialize to stock grade with no migration | round-trip legacy test |
| 6 | Panel strip truthful and operable without mouse | panel lifecycle + a11y |

**Out of scope:** aviation/naval armor; combat rework; any new damage
authority (DamageSystem/expedition estimate remain owners).

**Handoff — MUST PRESERVE:** immobilized-dispatch gate, recovery missions,
wear chain. **MUST ADD:** grades as data + one read model + one command.
**MUST NOT DO:** immunity, RNG rolls on grades, a second wear ledger.
**VERIFY WITH:** §C.3.7. **FIRST SAFE STEP:** P0 premise note (one page).

---

## Plan 04 — Plan 28 One-Bootstrap-Path Closure (`CF-P28-ONE-BOOTSTRAP-PATH`)

**Anchor:** `DEBT-PLAN28-MAIN-CONSTRUCTOR-MIGRATION` (RETIRED), census
C2[9] 28C residual, program Plan 10.
**Status:** manifest bootstrap executes only on the restore path; the fresh
path composes sessions directly.
**Unblocked by:** the setup-action migration seal (18 idempotent delegates).

### C.4.1 Current reality (verified)

- `ExecuteSubsystemManifestBootstrap(LifecyclePhase? phase = null)` exists at
  `src/Main.Lifecycle.cs:542`.
- Its **sole call site** is `RestoreAllSubsystemsFromDisk()`
  (`src/Main.SaveOrchestrator.cs:163`) — the load path.
- `ComposeCampaign()` (`src/Main.CampaignServices.cs:25+`) builds the fresh
  campaign by calling the individual `Setup*` methods directly and **never**
  invokes the bootstrap (verified: no other call site exists).
- `MainTriadDriftGateTests` 7/7, `player_panels_uitest` 17/17, and
  `7day_smoke_selftest` 10/10 gate the current migration state.

### C.4.2 Required delta

1. **Fresh-path parity:** invoke `ExecuteSubsystemManifestBootstrap()` from
   the fresh-composition path so both lifecycle entry points execute the
   same registered setup set (idempotent registration makes the double call
   safe; the phase parameter allows scoping).
2. **Lifecycle parity assertion:** extend the lifecycle selftest family with
   a gate asserting the executed setup set is identical across fresh-boot
   and restore paths (the EN-06 composite invariant, landed early because
   its contributing plans 09/11 are already sealed).
3. **Census truth:** flip C2[9] to `SEALED` with this evidence (ledger edit
   routed through the wave11-part2 owner).

### C.4.3 Ownership & seams

| Concern | Owner |
|---|---|
| Manifest + setup actions | `SubsystemManifest` (Core) + `Main.Lifecycle.cs` |
| Lifecycle entry points | `Main.SaveOrchestrator.cs` (restore), `Main.CampaignServices.cs` (fresh) |
| Parity gate | new focused test + selftest manifest row |

### C.4.4 State, save, determinism

No save change. Determinism untouched (setup is deterministic
registration).

### C.4.5 Failure modes

Ordering drift (a setup action that must run before a panel opens — the
manifest order is authoritative; the parity gate catches divergence);
double execution side effects (guarded by the idempotent registration; the
gate asserts the executed set, not the call count).

### C.4.6 Phases

- **P0 — Verify:** dump the manifest's registered action set and the
  fresh-path `Setup*` call list; diff them. Gate: the diff is written down
  before any edit.
- **P1 — Parity edit:** add the bootstrap invocation to the fresh path at
  the manifest-documented position. Gate: build 0/0; smoke selftests green.
- **P2 — Parity gate:** add the lifecycle-parity test.
  Gate: new test green; existing triad/panel/7-day gates green.
- **P3 — Truth:** census/ledger row flip (owner-routed).

### C.4.7 Test strategy & focused verify

`bash scripts/run_test.sh Ashfall.Core.Tests/Orchestration/SubsystemManifestTests.cs`;
new `LifecycleParityGateTests.cs` alone first;
`godot --headless --path . -- --7day-smoke-selftest`;
`--player-panels-uitest`.

### C.4.8 File impact map

| File | Action | Risk |
|---|---|---|
| `src/Main.CampaignServices.cs` | MODIFY (one invocation) | medium (shared root — sole active builder) |
| `src/Main.Lifecycle.cs` | MODIFY (gate helper if needed) | low |
| `Ashfall.Core.Tests/Tooling/LifecycleParityGateTests.cs` | NEW | low |

### C.4.9 Phase-gate checklist & risk table

| Phase | Gate command | Must also hold |
|---|---|---|
| P0 | (read-only diff, saved to the plan log) | manifest action set listed; fresh-path Setup* list listed |
| P1 | `dotnet build Ashfall.csproj` 0/0; `--7day-smoke-selftest` | no double-registration warnings |
| P2 | new `LifecycleParityGateTests` green | `MainTriadDriftGateTests` 7/7; `player_panels_uitest` 17/17 |
| P3 | census flip recorded by the claim owner | docs index regenerated |

| Risk | Class | Mitigation |
|---|---|---|
| Setup-order regression on restore path | HIGH | bootstrap stays at its current position; only the fresh path gains the call |
| Idempotency break (duplicate side effects) | MEDIUM | registration is idempotent by seal design; parity gate asserts the executed set |
| Shared-root race on `Main.CampaignServices.cs` | MEDIUM | sole-active-builder sequencing |

**Out of scope:** migrating more `Setup*` calls into the manifest; any
save-orchestrator reordering.

**Handoff — MUST PRESERVE:** restore-path ordering; idempotent registration.
**MUST ADD:** one invocation + one gate. **MUST NOT DO:** rewrite
ComposeCampaign. **VERIFY WITH:** §C.4.7. **FIRST SAFE STEP:** P0 diff
(read-only).

---

## Plan 05 — XP-01 Difficulty Full Binding (`CF-XP01-DIFFICULTY-FULL-BINDING`)

**Anchor:** active batch `XP-WAVE1-DIFFICULTY-AUTHORITY`
(`claim-xp-wave1-difficulty-2026-09-18`), W1 implementation log.
**Status:** catalog/director/provider + default-preset host setup + one
consumer + completion-history tag live; selection, persistence, six scalar
consumers, panel, and selftest missing.
**Unblocked by:** the W1 authorization (D1 signed, narrowed to per-consumer
premise checks).

### C.5.1 Current reality (verified)

- `difficulty_presets.json` ships **4 presets** (`difficulty_sparing`,
  `difficulty_standard`, `difficulty_austere`, `difficulty_dirge`) with
  **8 scalars each**: `hunger_rate_mult`, `thirst_rate_mult`,
  `radiation_gain_mult`, `disease_onset_mult`, `hostile_encounter_mult`,
  `market_price_mult`, `equipment_decay_mult`, `crisis_deadline_mult`, plus
  `starting_bonus_item_ids` (`canned_food`, `iodine_pills` on sparing).
- Core: `DifficultyPresetCatalog.cs` (fail-closed loader), `DifficultyDirector.cs`
  (`ResolveProvider` — explicit unknown id fails closed),
  `DifficultyScalarsProvider.Legacy` (all-ones parity). Tests 8/8;
  `CatalogIntegrityValidator` validates the catalog and starter-item
  references.
- Host: `SetupDifficulty` (`src/Main.Difficulty.cs`) resolves
  **`ResolveProvider(null)` — the catalog default only**; no preset choice
  exists at campaign creation.
- Live consumers: `HostileEncounterMult` (`src/Main.EvolvingWorld.cs:193`)
  and the completion-history preset stamp (`src/Main.Endgame.cs:74`,
  `CampaignCompletionHistory.cs:44`). No other scalar has a consumer.
- Persistence: **no difficulty save section and no preset id in any save
  state** (grep over `SaveSectionRegistry.cs` and `Main.SaveOrchestrator.cs`:
  zero hits). No panel row in `PanelRegistryBootstrap.cs`; no CLI selftest.

### C.5.2 Required delta

1. **Per-consumer premise notes** (one evidence line per site, before any
   edit — the XP-05 precedent): NeedsSystem hunger/thirst accumulation site;
   RadiationSystem dose composition; DiseaseSystem onset/exposure; MarketSystem
   value path (ExplainPrice clamps); EquipmentConditionSystem wear;
   CrisisPredictor deadline input (`Campaign/CrisisPredictionModel.cs`).
2. **Preset selection at campaign creation**: new-game flow (and only the
   new-game flow — mid-campaign lock per the W1 packet) selects a preset id;
   the id is validated fail-closed by the director.
3. **Persistence**: additive campaign-level `difficulty_preset_id`
   (save-owner integration; legacy default `difficulty_standard`; restore
   resolves through the director; unknown id on load → fail-closed error,
   not a silent fallback).
4. **Six remaining scalar consumers**, each additive, each with the parity
   guard: scalars at 1.0 ⇒ byte-identical behavior (the Legacy provider pins
   this).
5. **Starting bonus items**: granted once at campaign creation through the
   canonical inventory (`AddById`), exactly the authored ids.
6. **Panel surface**: preset selection at new game + a read-only current
   difficulty line (words + numbers; never color-only); keyboard/controller
   operable.
7. **Selftest + determinism**: `--difficulty-selftest` (catalog load, resolve,
   legacy parity, one consumer per scalar, save round-trip); determinism
   coverage (same seed, two presets, distinct-but-bounded outcomes).

### C.5.3 Ownership & seams

| Concern | Owner |
|---|---|
| Catalog/director/scalars | `Assets/Ashfall.Core/Difficulty/` (active W1 claim) |
| Campaign creation + persistence | new-game flow + save-section owner (additive field, no new section unless the save owner requires one) |
| Consumers | each named system's existing mutation/estimate site |
| Completion history | `claim-wave11-part2-execution` (excluded from the W1 claim — coordinate, do not edit) |
| Panel | new-game surface + settings read model |

### C.5.4 State, save, determinism

One additive id persisted with campaign state; restore resolves through the
fail-closed director; checksum discipline (culture-invariant) inherited; no
RNG introduced (scalars are multipliers).

### C.5.5 Failure modes

Consumer drift (a calculation site bypassing the provider — parity guard +
premise notes); save with an id that a future catalog removed (fail-closed
error with a migration note, never silent standard); double bonus grants
(grant exactly once at creation, idempotent by campaign state).

### C.5.6 Phases

- **P0 — Premise notes:** seven evidence lines (six consumers + new-game
  flow). Gate: notes recorded in the W1 log before edits.
- **P1 — Selection + persistence + bonus items.** Gate: save round-trip +
  legacy parity tests green.
- **P2 — Consumers (one phase per seam, parity guard each).** Gate: per-seam
  focused tests green; 1.0 parity asserted per consumer.
- **P3 — Panel + selftest.** Gate: panel lifecycle + a11y PASS;
  `--difficulty-selftest` green.
- **P4 — Determinism soak.** Gate: paired-seed run documented in the W1 log.

### C.5.7 Test strategy & focused verify

`bash scripts/run_test.sh Ashfall.Core.Tests/Difficulty/`; per-consumer
focused files alone first; `godot --headless --path . -- --difficulty-selftest`
(new); `--endings-selftest` (completion history untouched);
`--data-integrity-selftest`.

### C.5.8 File impact map

| File/area | Action | Risk |
|---|---|---|
| `src/Main.Difficulty.cs` | MODIFY (selection, persistence hooks) | low |
| `src/Main.GameFlow.cs` (new-game) | MODIFY (preset choice + bonus grant) | medium (shared root) |
| six consumer sites | MODIFY (additive multiplier reads) | medium each |
| `src/UI/` new-game/settings surface | MODIFY/NEW | low |
| `src/Host/HostCli.*` | MODIFY (selftest) | low |

### C.5.9 Per-scalar consumer matrix (the XP-01 binding map)

| Scalar | Owner (premise site to re-verify) | Application seam | Parity guard | Focused test |
|---|---|---|---|---|
| `hunger_rate_mult` | `NeedsSystem` hunger accumulation | multiply the per-tick hunger delta at the single accumulation site | 1.0 ⇒ byte-identical day ticks | Needs suite + scalar unit |
| `thirst_rate_mult` | `NeedsSystem` thirst accumulation | same seam, thirst column | same | same file |
| `radiation_gain_mult` | `RadiationSystem` dose composition (`ComputeEffectiveRate` chain) | multiply effective rate before clamp; clamps unchanged | bounded by existing clamp tests | Radiation suite |
| `disease_onset_mult` | `DiseaseSystem` exposure/onset roll | multiply onset probability; bounds [0,1] preserved | seed-pinned roll parity at 1.0 | Disease slice |
| `hostile_encounter_mult` | **live already** (`Main.EvolvingWorld.cs:193`) | — (premise note only) | existing | existing |
| `market_price_mult` | `MarketSystem` value path (`ExplainPrice` input, before authored clamps) | multiply base value; category bounds still clamp | PriceExplanation 3/3 parity | Economy suite |
| `equipment_decay_mult` | `EquipmentConditionSystem` wear application | multiply wear delta; condition floor/ceil unchanged | round-trip + wear bounds | Equipment tests |
| `crisis_deadline_mult` | `CrisisPredictor` deadline input (`Campaign/CrisisPredictionModel.cs`) | multiply deadline runway (mult >1 = more time) | `CrisisPredictionTests` 16/16 | Campaign suite |

Every row lands as its own phase with its own parity assertion; a row whose
premise site has drifted stops that row (record, do not bend).

### C.5.10 Acceptance / definition of done

| # | Outcome | Proof |
|---|---|---|
| 1 | New game offers exactly the 4 authored presets, keyboard/controller operable | panel lifecycle + a11y selftests |
| 2 | Chosen preset persists; reload resolves identically; unknown id fails closed | save round-trip + corrupted-id test |
| 3 | Sparing grants `canned_food` + `iodine_pills` exactly once | inventory assertion in selftest |
| 4 | All 8 scalars consumed at their owner sites | per-row focused tests green |
| 5 | Standard preset ≡ pre-XP-01 behavior | Legacy-provider parity harness (byte-identical day fingerprints) |
| 6 | Completion history v2 unchanged | `CampaignCompletionHistoryTests` 11/11 + `--endings-selftest` |
| 7 | Panel shows truthful current difficulty (words + numbers) | a11y + panel contract tests |
| 8 | Determinism: same seed × two presets ⇒ distinct bounded outcomes | paired-seed soak in W1 log |

**Out of scope:** New Game+ composition (Plan 175 boundary, DEC-20); any
chronicle read model beyond the sealed v2 stamp; mid-campaign difficulty
changes.

### C.5.11 `--difficulty-selftest` specification

Cases (headless, deterministic, 15 FPS): (1) catalog loads and validates
(4 presets, 8 scalars each, strict starter-item references); (2) director
resolves each preset id and the default; unknown id fails closed with the
exact error; (3) Legacy provider is all-ones (parity pin); (4) new-game
selection persists and reloads identically (save round-trip); (5) bonus
items granted exactly once on sparing; (6) each scalar consumer applied at
its site (one assertion per row of the C.5.9 matrix); (7) standard preset
produces the pre-XP-01 day fingerprint (parity); (8) two presets, same
seed, distinct-but-bounded outcomes. Register in `SELFTEST_MANIFEST.json`
and the CLI help contract (`HostCliHelpContractTests` pins the verb).

**Handoff — MUST PRESERVE:** fail-closed loader/director; Legacy parity;
DEC-20 boundary. **MUST ADD:** selection, persistence, six consumers, panel,
selftest. **MUST NOT DO:** bypass a consumer's owner; invent a second
difficulty authority; touch the wave11-owned history paths.
**VERIFY WITH:** §C.5.7. **FIRST SAFE STEP:** P0 premise notes (read-only).

---

## Plan 06 — E1/Plan 53 Ambition Governance Programme (`E1-PLAN53-AMBITION-GOVERNANCE`)

**Anchor:** census row `E1` (`READY-UNCLAIMED`), Plan 53 source scope.
**Status:** prerequisites sealed; the 16-step governance programme is
unclaimed.
**Unblocked by:** Plan 29 (C1[7]) sealed — `CLAIMS.json` (24 claims),
`scripts/ci/verify-capability-claims.py`, `docs/roadmap/README.md`,
`docs/roadmap/WAVE_LEDGER.md`.

### C.6.1 Current reality (verified)

- The census row classifies E1 `READY-UNCLAIMED` with the full E1A–E1P
  order: baseline → schema/register → metadata migration → freshness
  verifier → overlap clusters → pillars/rubric → ambition audit → rails
  readiness → intake checker → UI authority gate → numbering/archive/co-author
  rules → CI rollout → roadmap publication → metrics/review → independent
  audit → closure.
- The prerequisite rails exist and are green: capability-claims registry +
  verifier, roadmap flow-of-truth governance, docs index generator,
  `CI_GATE_MANIFEST.json` (48+ gates; the 482913 audit measures 53 total /
  50 fast-tier on `main` — reconcile locally at claim time).
- The completion-first execution wave has just produced exactly the kind of
  fresh truth (ten seals, six closed roster plans) that an ambition audit
  must consume — the programme's read model is richer now than at census
  time.

### C.6.2 Required delta (per census order, compressed to deliverables)

1. **E1A baseline + E1B schema/register:** a machine-readable ambition
   register (proposed-vs-sealed state per corpus row, citing the census).
2. **E1C metadata migration + E1D freshness verifier:** generator that
   stamps plan metadata (status/date/evidence) and a CI check that fails on
   stale entries.
3. **E1E overlap clusters + E1F pillars/rubric:** cluster the 111
   `AUDIT-PENDING` rows by domain; publish the pillar rubric used to rank
   them.
4. **E1G ambition audit + E1H rails readiness + E1I intake checker:** the
   ranked queue with readiness verdicts and an intake script that validates
   any newly proposed plan against the register.
5. **E1J UI authority gate + E1K numbering rules:** enforce the
   numbering-collision rule (the census's 39-collision finding) and the
   panel-authority gate in CI.
6. **E1L CI rollout + E1M roadmap publication + E1N metrics + E1O
   independent audit + E1P closure.**

### C.6.3 Ownership & seams

Governance-owned: `docs/roadmap/`, `docs/governance/`, `scripts/ci/`,
`docs/INDEX.md` (generator). No gameplay code. The census itself is inside
the wave11-part2 claim — read-only input until that claim closes.

### C.6.4 State, save, determinism

Not applicable (no runtime state). CI gates are additive to
`CI_GATE_MANIFEST.json` and must pass `--check` modes.

### C.6.5 Failure modes

Register drift vs the live ledgers (the freshness verifier exists to catch
this); double-governance (do not create a second census — E1 consumes the
census, it does not replace it).

### C.6.6 Phases

- **P0 — Claim + baseline (E1A/E1B).** Gate: register skeleton + census
  import committed.
- **P1 — Verifiers (E1C/E1D/E1I).** Gate: new CI gates green in `--check`.
- **P2 — Clusters + rubric + audit (E1E–E1G).** Gate: ranked queue
  published with per-cluster evidence.
- **P3 — Rules + rollout + roadmap (E1J–E1M).** Gate: gates in the manifest;
  roadmap regenerated.
- **P4 — Metrics, independent audit, closure (E1N–E1P).** Gate: closure
  doc; register counts agree with the census refresh.

### C.6.7 Test strategy & focused verify

`python3 scripts/ci/verify-capability-claims.py`;
`python3 scripts/ci/generate-docs-index.py --check`; any new gate's
`--check`; `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/` for
gate-contract tests.

### C.6.8 File impact map

| File/area | Action | Risk |
|---|---|---|
| `docs/governance/` + `docs/roadmap/` | NEW/MODIFY | low |
| `scripts/ci/` (verifiers) | NEW | low |
| `docs/ci/CI_GATE_MANIFEST.json` | MODIFY (additive rows) | low |

### C.6.9 E1A–E1P deliverable table

| Step | Deliverable | Gate |
|---|---|---|
| E1A | ambition baseline snapshot (corpus + ledgers at claim HEAD) | committed snapshot with counts |
| E1B | machine-readable ambition register schema + import of the census | schema validated; import diff = 0 |
| E1C | plan metadata migration (status/date/evidence stamps) | generator `--check` green |
| E1D | freshness verifier (fails on stale entries) | new CI gate green; failure fixture proves it |
| E1E | overlap clusters over the 111 AUDIT-PENDING rows | cluster map published |
| E1F | pillars/rubric used to rank clusters | rubric doc with worked examples |
| E1G | ambition audit (ranked queue with readiness verdicts) | queue cites census + this program's audit |
| E1H | rails-readiness verdict per pillar | each verdict names its gate evidence |
| E1I | intake checker for new proposals | script rejects a malformed proposal fixture |
| E1J | UI authority gate in CI | gate green; violation fixture fails |
| E1K | numbering/archive/co-author rules (39-collision rule enforced) | collision detector wired to the register |
| E1L | CI rollout of the new gates | manifest rows + fast-tier green |
| E1M | roadmap publication (flow of truth refreshed) | roadmap regen `--check` |
| E1N | metrics/review cadence | metrics doc with drift triggers |
| E1O | independent audit pass | second-agent read-only verification |
| E1P | closure | closure doc; register counts == census refresh counts |

### C.6.10 Risk table

| Risk | Class | Mitigation |
|---|---|---|
| Register drift vs live ledgers | HIGH | E1D freshness verifier fails CI on staleness |
| Double-governance (second census) | HIGH | E1 consumes the census read-only; sealing stays with execution evidence |
| Gate sprawl in the fast tier | MEDIUM | each new gate has a failure fixture proving it can fail |
| Claim overlap with wave11 census owner | MEDIUM | census is read-only input; flips stay owner-routed |

### C.6.11 Acceptance / definition of done

| # | Outcome | Proof |
|---|---|---|
| 1 | Register imports the census with a zero diff | import check green |
| 2 | Freshness verifier catches an injected stale row | failure fixture |
| 3 | 111 AUDIT-PENDING rows clustered and ranked | cluster map + rubric doc |
| 4 | Intake checker rejects a malformed proposal | failure fixture |
| 5 | New gates live in CI manifest, fast tier green | manifest `--check` |
| 6 | Roadmap regenerated and truthful | roadmap `--check` + counts agree with the census refresh |

**Out of scope:** any corpus plan execution; census rewrites (owner's).

**Handoff — MUST PRESERVE:** census + ledgers as inputs.
**MUST ADD:** governance tooling only. **MUST NOT DO:** seal corpus rows
from the register (sealing stays with execution evidence).
**VERIFY WITH:** §C.6.7. **FIRST SAFE STEP:** P0 claim + register skeleton.

---

## Plan 07 — Plan 37 Input Reality, Focus & Controller Parity (`C2[15]-PLAN37-INPUT-REALITY`)

**Anchor:** `C-integration-plans/C2_planintegration[15].md` (37A → 37B →
37C with a hard gate: no controller bindings before keyboard/focus
navigation is functional).
**Status:** corpus contract intact; declared prerequisite wave sealed; the
corpus predates the 2026-09-18/19 execution wave.
**Unblocked by:** census DAG prerequisites C2[9] (Plan 28, sealed) and C1[8]
(Plan 31, sealed); the corpus's own dependency list (17B guidance overlay,
16A live-panel verdicts, 31B interaction parity, 28A manifest validation)
is satisfied except localization (DEC-13 deferred — keyed strings are a
boundary, not a blocker, per the corpus's own scope discipline).

### C.7.1 Current reality (partially verified — P0 completes it)

- `AshfallInputActions` declares the management-UI action vocabulary:
  `Close`/`Confirm`/`NextTab` (`:15-19`), `NavUp/Down/Left/Right`
  (`:22-25`), panel hotkeys (`:28-39`), journal tabs (`:42-44`), plus the
  guidance toggle (`ashfall_guidance`, `:34`, landed under Plan 17).
- Plan 16's honest-navigation pass removed fixture defaults and added
  live-selection resolvers + explicit empty states across the geiger /
  triangulation / weather panels; panel lifecycle selftest gates 17/17.
- The corpus's stated gaps (predating the wave): navigation actions
  declared but navigating nothing in places, focus order effectively absent,
  typed predicates without callers, controller bindings absent, rebinding
  absent, no generated input help.
- **P0 mandate:** re-verify every one of those gap claims at current HEAD
  (the 2026-09-18/19 session touched `HostCli.PanelTests.cs`,
  `HostCli.Summary.cs`, panel grids, and lifecycle paths — the gap list may
  have drifted in both directions). No edit before this audit passes.

### C.7.2 Required delta (per corpus order)

1. **37A:** every declared action real (a handler for each `ashfall_*`
   action; delete or bind predicates with no caller), centralized dispatch,
   deterministic focus order across live player surfaces, and the
   mouseless-day proof (a full campaign day drivable without a mouse,
   pinned by a selftest).
2. **37B:** controller bindings for the deliberately supported
   management-UI interactions (scope discipline: no scope creep beyond
   them).
3. **37C:** user rebinding surface, text/UI scaling hooks, safe settings
   recovery, generated live input help.

### C.7.3 Ownership & seams

| Concern | Owner |
|---|---|
| Action vocabulary | `AshfallInputActions` + `project.godot` input map (no parallel vocabulary) |
| Dispatch | central game-flow dispatcher (extend, do not fork) |
| Focus order | panel base + registry order |
| Settings | existing `UserSettings*` store (rebind persistence) |

### C.7.4 State, save, determinism

Rebindings persist through the existing `UserSettings*` owner (versioned,
safe-recovery path already proven by Plan 184). No campaign-state change;
no RNG.

### C.7.5 Failure modes

Per-panel shortcut sniffing (forbidden by scope discipline); focus traps in
modals (the existing modal focus capture is the pattern to extend);
controller parity claims without the hard gate (37B before 37A is
prohibited).

### C.7.6 Phases

- **P0 — Premise audit (read-only).** Gate: per-clause verdict table for
  every corpus gap claim at current HEAD.
- **P1 — 37A actions + dispatch.** Gate: action-to-handler parity test
  green.
- **P2 — 37A focus order + mouseless proof.** Gate: focus-order selftest +
  mouseless-day selftest green.
- **P3 — 37B controller.** Gate: controller parity cases in the panel
  lifecycle selftest.
- **P4 — 37C rebinding/scaling/help.** Gate: settings selftest asserts
  rebind persistence + recovery; generated help matches the action table.

### C.7.7 Test strategy & focused verify

`bash scripts/run_test.sh Ashfall.Core.Tests/UI/PanelRouteGateTests.cs`;
new input/focus test files alone first;
`godot --headless --path . -- --player-panels-uitest`;
`--panel-bind-lifecycle-selftest`; `--settings-selftest`; `--ui-layout-selftest`.

### C.7.8 File impact map

| File/area | Action | Risk |
|---|---|---|
| `src/Host/AshfallInputActions.cs` | MODIFY (only if predicates are dead) | medium |
| `project.godot` (input map) | MODIFY (controller rows in 37B) | medium |
| panel base / focus helpers | MODIFY | medium |
| `src/UI/*` (focus order declarations) | MODIFY (bounded per panel) | medium |
| settings store + panel | MODIFY (37C) | low |

### C.7.9 Corpus gap re-verification template (the P0 artifact)

| Corpus gap claim | Verification probe | 2026-09-19 status |
|---|---|---|
| "directional navigation actions navigate nothing" | grep `NavUp/NavDown/NavLeft/NavRight` call sites in panels + run `--player-panels-uitest` | **UNVERIFIED — P0 must establish** (2026-09-19 session touched panel grids/lifecycle) |
| "focus order effectively absent" | inspect focus neighbors on 3 representative panels (dashboard, expanded, modal) | **UNVERIFIED — P0** |
| "typed input predicates have no caller" | per-predicate grep in `src/` | **UNVERIFIED — P0** (Plan 16's resolvers removed several dead paths) |
| "controller bindings do not exist" | `project.godot` input map controller events | **UNVERIFIED — P0** |
| "rebinding has no user surface" | settings panel + `UserSettings*` fields | **UNVERIFIED — P0** (Plan 184 a11y work touched settings) |
| "no generated live input help" | help surface + action table generator | **UNVERIFIED — P0** |

37A acceptance matrix: (a) action↔handler parity test (every `ashfall_*`
action has a live consumer; dead actions deleted with evidence); (b) focus
order deterministic and documented per surface; (c) mouseless-day selftest
green; (d) existing hotkeys unchanged. 37B: controller cases in the panel
lifecycle selftest + parity with keyboard routes. 37C: rebind persistence
round-trip + safe-recovery (corrupt settings file ⇒ defaults, never crash)
+ generated help matching the action table exactly.

**Out of scope:** localization keying (DEC-13); new input frameworks; any
gameplay action (combat etc.) beyond the declared management-UI set.

**Handoff — MUST PRESERVE:** existing hotkeys, modal capture, a11y gates.
**MUST ADD:** handlers, focus order, controller, rebinding, help.
**MUST NOT DO:** per-panel shortcut sniffing; parallel action vocabulary.
**VERIFY WITH:** §C.7.7. **FIRST SAFE STEP:** P0 audit (read-only).

---

## Plan 08 — Plan 48 Release Craft (`C2[21]-PLAN48-RELEASE-CRAFT`)

**Anchor:** `C-integration-plans/C2_planintegration[21].md`
(29A → 39A → 48A → 48B → 48C; hard gate: no release tag until 48A
compatibility tests and the 39A release gate are green).
**Status:** corpus contract intact; the release machinery exists in pieces;
two corpus dependencies (39A, 46A/46B) need premise verification.
**Unblocked by:** prerequisite C1[7]/Plan 29 sealed; Plan 47A/47C mod
contract sealed; export presets, version report, save/data schema
versioning, and CI gates all live.

### C.8.1 Current reality (partially verified — P0 completes it)

- Version surface: project version metadata + composed version report
  (`VersionReportContractTests` pins codec/section counts).
- Schema surface: per-store save schema versions, data `schema_version`
  envelopes, save migrations, mod `game_range`/`mod_contract_range`
  compatibility evaluation (`ModCompatibilityEvaluator`, 38/38 tests).
- Build surface: export presets (Linux/Windows), export staging + parity
  gate (Plan 26B), performance budgets (`BUDGETS.md` +
  PerformanceSelfTest), CI gate manifest with fast tier.
- Release-adjacent skills/infra exist: release checklist, changelog file,
  `ashfall-release-captain` skill (version bump, changelog from git
  history, full pre-release gate, export smoke).
- **Corpus-declared dependencies needing P0 verification:** Plan 39A
  release gate (C2[16] census row is `AUDIT-PENDING`; the corpus itself
  says "release-gate work from Plan 39" already exists) and Plan 46A/46B
  balance/metrics reporting (C2[20] `AUDIT-PENDING`, explicitly held by the
  Wave 12 Part 1.1 audit). Plan 29A and 47A/47C are sealed.
- The 482913 audit (main branch) records release-relevant defects worth
  folding into P0 locally: the port-contract generator's stale
  `generated_at` stamp and the missing docs-regeneration trigger for the
  port policy.

### C.8.2 Required delta (per corpus order)

1. **48A — one compatibility policy + fixture-backed compatibility tests:**
   a written policy (save schema, data schema, mod contract ranges) plus a
   test that fails when a version claim is unproven.
2. **48B — one scripted tagged release path + machine-readable release
   manifest:** a script that bumps, assembles the changelog from git
   history, runs the pre-release gate, exports, smoke-boots, and emits a
   manifest (version, commit, gates, artifacts, checksums).
3. **48C — one rehearsed hotfix/rollback procedure that preserves campaign
   saves:** documented + rehearsed; no save-schema migration inside a
   hotfix (corpus hard rule).

### C.8.3 Ownership & seams

| Concern | Owner |
|---|---|
| Version truth | project metadata + version report composer |
| Compatibility enforcement | save/data/mod validators + new fixture tests |
| Release path | new script under `scripts/` + CI manifest rows |
| Hotfix procedure | docs + rehearsal evidence |

### C.8.4 State, save, determinism

No runtime change. The release manifest is generated (never hand-edited);
the hotfix procedure must prove save preservation with round-trip tests.

### C.8.5 Failure modes

Tagging before the gate (hard gate forbids); hand-edited generated reports
(forbidden); a hotfix that migrates saves (forbidden by the corpus and by
the frozen-shape discipline).

### C.8.6 Phases

- **P0 — Premise audit (read-only).** Establish the true state of 39A and
  46A/46B dependencies at HEAD; decide whether the 48A entry is
  dependency-blocked (record it, stop) or clear.
  Gate: written verdict with file:line evidence.
- **P1 — 48A policy + tests.** Gate: compatibility fixtures green.
- **P2 — 48B release script + manifest.** Gate: one dry-run release passes
  the full pre-release gate headlessly; manifest generated.
- **P3 — 48C hotfix rehearsal.** Gate: documented rehearsal with
  before/after save round-trip equality.

### C.8.7 Test strategy & focused verify

`bash scripts/run_test.sh Ashfall.Core.Tests/VersionReportContractTests.cs`;
`ModCompatibilityEvaluator` tests; export parity + performance selftests;
`python3 scripts/ci/generate-docs-index.py --check`.

### C.8.8 File impact map

| File/area | Action | Risk |
|---|---|---|
| `docs/governance/RELEASE_*.md` | NEW | low |
| `scripts/release/*.sh` | NEW | low |
| `docs/ci/CI_GATE_MANIFEST.json` | MODIFY (additive) | low |
| compatibility test fixtures | NEW | low |

### C.8.9 Release manifest sketch & compatibility policy surface

Machine-readable release manifest (generated, never hand-edited):

```json
{
  "release_id": "vX.Y.Z",
  "commit": "<sha>",
  "gates": { "fast_tier": "PASS", "data_integrity": "PASS", "export_parity": "PASS", "performance": "PASS" },
  "artifacts": [ { "preset": "Linux/X11", "path": "…", "sha256": "…" }, { "preset": "Windows Desktop", "path": "…", "sha256": "…" } ],
  "schemas": { "save_codecs": <n>, "save_sections": <n>, "data_catalogs": <n>, "mod_contract": "v<range>" },
  "changelog": { "source": "git history", "entries": <n> }
}
```

Compatibility policy surface (48A): one table mapping each compatibility
axis (save codec versions, data `schema_version` envelopes, mod
`game_range`/`mod_contract_range`) to its enforcing test + fixture. Any
version claim without a fixture-backed test fails the gate. 48C rehearsal:
create a save on the release build, apply the rehearsed hotfix build, prove
field-by-field round-trip equality and no event replay on load.

### C.8.10 Risk table

| Risk | Class | Mitigation |
|---|---|---|
| Tagging past a red gate | HIGH | corpus hard gate: release script refuses on any failed gate |
| Generated manifest drift | MEDIUM | manifest generated only; `--check` mode in CI; docs-regen workflow triggers on the release path |
| Hotfix save damage | CRITICAL | 48C rehearsal proves round-trip equality; no save migration inside a hotfix (frozen shapes) |
| Compatibility claim without proof | HIGH | 48A requires a fixture-backed test per claim |

**Out of scope:** CI ruleset changes on the remote; save migrations; any
gameplay change.

**Handoff — MUST PRESERVE:** frozen save shapes; generated-file discipline.
**MUST ADD:** policy, script, manifest, rehearsal. **MUST NOT DO:** tag
past a red gate; migrate saves in a hotfix. **VERIFY WITH:** §C.8.7.
**FIRST SAFE STEP:** P0 dependency verdict (read-only).

---

# Part D — Decision-gated tier (exists, scoped, awaiting signature)

| Item | Scope ready | Signature needed | Condition to lift |
|---|---|---|---|
| `CF-P3-SEMANTIC-KIND-AUTHORITY` | 10 `SemanticKind` briefing sections + 17C-I ducking + 17C-E sweep + 17B matrix | **D11** | one line choosing (i) flat pinned contract or (ii) re-grouping + amend the pin |
| `CF-QD-QUARANTINE-DRAIN-TRANCHE` | 48 `Compile Remove` exclusions; per-file reinstatement pattern proven 5× | **D21/F11** | authorize a named per-file batch |
| `CF-XP04-ECONOMY-LEGS` | funds ledger, black-market legs, heat, counterfeit, restock design | **F13** | design sign-off (D3/D4-class) |
| `CF-XP06-BODY-INTEGRITY` | limb-requirement gate, prosthetic rehab arc, body state | **F14** | schema package sign-off (DEC-03 successor) |
| XP-07 / XP-08 | provenance; trade routes + seasonal migration | — | sequenced after XP-04 (funds) |
| XP-09 / XP-10 | presenter skills; phobia growth | — | premise checks vs RETIRED DEC-17/DEC-18 — reversal is a foreman decision |
| EN-01…EN-08 | expanded in the companion document | one authorization each | see companion Part F |
| D3 / D4 / D13 / D16 / D19a–c / D20 / D22 | decision packet items | per packet | per packet sign-off lines |
| Plan 24 snapshot rebaseline | enumerated render intents | **F16** | renderer-capable session |

---

# Part E — Sequencing & Dependency Order

## E.1 DAG (roster)

```
P02 P1 content seal ──┐
P03 P5 restock        ├── truth lane (no signatures; P03 owner-routed)
P04 P6 armor grades   │
P10 P28 bootstrap     ├── surface lane (bounded engineering)
P13 XP-01 binding ────┤   (feeds EN-01 in the companion program)
P53 E1 governance ────┤   (consumes the census; feeds EN-07)
P37 input reality ────┤   (premise audit first; 37A→37B→37C)
P48 release craft ────┘   (premise audit first; 48A hard gate)
```

No hard interdependencies among the eight; the only ordering rules are the
two premise-audit-first entries (P37/P48) and the ownership routing for
P03/P53 ledger surfaces.

## E.2 Recommended wave order

1. **Wave D1 — Truth & sealing (cheap, zero signature):** Plan 01 (P0–P4),
   Plan 02 (P0–P2). Everything here makes later ledger claims trustworthy.
2. **Wave D2 — Active-batch completion:** Plan 05 (XP-01 full binding,
   P0–P4). Completes the current INTEGRATION_PLANS batch.
3. **Wave D3 — Surface integrity:** Plan 04 (bootstrap parity), Plan 03
   (armor grades).
4. **Wave D4 — Governance & corpus frontier:** Plan 06 (E1), then the two
   premise audits (Plan 07 P0, Plan 08 P0) and their plans if the audits
   pass.
5. **Wave D5 (after signatures):** Part D items in packet order; EN layer
   per the companion program.

## E.3 Concurrency rules

- At most three concurrent packages with disjoint ownership (ledger
  operating rule).
- Claim exact paths in `WORKTREE_OWNERSHIP.md` before the first edit of
  every package.
- Shared roots (`Main.CampaignServices.cs`, `Main.GameFlow.cs`,
  `ExpeditionHostSession.cs`) are sole-active-builder paths: sequence
  packages that touch them; never parallel-edit.

## E.4 Effort & verification recipes

| Package | Phase count | Shared roots touched | Full gate set (beyond focused files) |
|---|---:|---|---|
| Plan 01 | 5 | none | data-integrity + content-utilization + audio selftests |
| Plan 02 | 3 | ledgers (owner-routed) | docs-index `--check` |
| Plan 03 | 5 | `ExpeditionHostSession` | vehicle-garage selftest + panel lifecycle + a11y |
| Plan 04 | 4 | `Main.CampaignServices.cs` | 7day-smoke + player-panels-uitest + triad |
| Plan 05 | 5 | `Main.GameFlow.cs` + 6 consumer sites | difficulty selftest (new) + endings + data-integrity |
| Plan 06 | 5 | governance only | claims verifier + docs-index + new gates |
| Plan 07 | 5 | panel base + many panels | player-panels-uitest + settings + ui-layout |
| Plan 08 | 4 | scripts/CI only | export parity + performance + version report tests |

Recipe (every package, every phase): `bash scripts/run_test.sh <focused
target>` → named selftests → `dotnet build Ashfall.csproj` → record results
in the package log → only then proceed. A compile-green result is not
integration proof (standing rule).

## E.5 Claim-row template (per package)

```
| claim-<program>-<package>-<date> | <PACKAGE> | <builder role> |
  **Core:** <exact Core paths> · **Host:** <exact src paths> ·
  **Data:** <exact JSON paths> · **Tests:** <exact test paths> ·
  **Docs:** <log path> · **Governance:** WORKTREE_OWNERSHIP.md |
  P0 evidence: <verdict-table path> · Phase gates: <per-phase results> |
```

---

# Part F — Consolidated Foreman Decision Checklist

| # | Decision | Needed by | Recommended shape |
|---|---|---|---|
| F1 | P5 ratification line | Plan 02 | ratify DEC-05 wording across the three ledgers (wording-only) |
| F2 | P6 claim note (deferral reversal) | Plan 03 | new claim + one-line premise note naming the four tiers |
| F3 | XP-01 consumer list per seam | Plan 05 | one evidence note per consumer, pairs bound (already required by W1) |
| F4 | E1 claim confirmation | Plan 06 | claim per census protocol |
| F5 | P37 scope confirmation | Plan 07 | confirm the corpus order + hard gate after P0 audit |
| F6 | P48 dependency verdict | Plan 08 | confirm or defer based on the 39A/46A/46B premise audit |
| F7–F16 | carried from the fifteen program and the 2026-09-18 packet | Part D | D11, D21, F13, F14, D3, D4, D13, D16, D19a–c, D20, D22, F16 |

Standing obligation: every consumer bind (Plan 05's six seams, Plan 03's
profile consumption) re-verifies the live calculation site in source before
editing — the XP-05 correction is the precedent for why.

---

# Part G — Program Verification, Rollback, Out of Scope

## G.1 Program-level verification pattern

Every roster plan closes with: focused suite green (its own files), owning
regional suite green, `dotnet build Ashfall.csproj` 0 errors, and where
runtime paths changed, the matching headless selftest(s). Save-touching
plans (03, 05) add round-trip + legacy-parity + continuous-vs-mid-reload
equality tests. Full-suite runs remain exception-gated per TEST_POLICY.md.

## G.2 Rollback strategy

Each roster plan lands as small reversible commits (Core-first, host
second, panel third). Data changes are additive with neutral legacy
defaults, so revert = code revert; no save is ever migrated irreversibly
(frozen prior shapes everywhere). Plans touching shared seams (05's
consumers, 04's composition root) carry a named parity guard whose removal
blocks the revert review.

## G.3 Out of scope for the whole program

- Reversing any signed or retired decision (DEC-06, DEC-17, DEC-18, C3
  HOLDs) without a new signature.
- Census tranche-2 audits as stealth integration.
- Any new parallel authority — difficulty, funds, armor, input, release, or
  otherwise — regardless of convenience.
- Ledger edits outside the owning claim (Plan 02 and the census flips route
  through `claim-wave11-part2-execution-2026-09-18` or a user-authorized
  transfer).
- Full-suite runs by default.

## G.4 Final implementation handoff

**MUST PRESERVE:** every existing owner named in Part C; signed decisions
DEC-05/DEC-06/DEC-20 and the W1 packet; deterministic RNG contracts; frozen
save-shape discipline; the fail-closed loader pattern.
**MUST ADD:** only what each plan's delta names, behind its gate, in phase
order, with the parity guards listed.
**MUST NOT DO:** bind consumers without premise notes; run broad suites by
default; edit generated files by hand; race claimed paths (check
`WORKTREE_OWNERSHIP.md` immediately before each claim).
**VERIFY WITH:** each plan's Focused-verify block; the program-exit census
rerank (EN-08 in the companion program) as the closing artifact.
**FIRST SAFE STEP:** Wave D1's Plan 01 P0 census or Plan 02 P0 re-verify —
both are read-only evidence passes a single cheap agent can run today.

## G.5 Program exit criteria (how this program knows it is done)

| # | Criterion | Measured by |
|---|---|---|
| 1 | All eight roster plans sealed or explicitly re-queued with evidence | per-plan closeout rows in `INTEGRATION_PLANS.md` |
| 2 | XP W1 batch presented for acceptance | W1 acceptance block updated by the claim owner |
| 3 | Census drain re-measured and truthful | EN-08-style rerank: counts, DAG, claims check at then-current HEAD |
| 4 | Every flipped census row cites sealing evidence | census §1 row updates (owner-routed) |
| 5 | Decision queue drained or explicitly re-queued | Part F table all-signed or re-queued with conditions |
| 6 | Zero new parallel authorities | architecture map `--check`; port contract `--check` |
| 7 | Zero regressions in the standing gate battery | verify-fast tier green in a dedicated window (exception-gated) |

## G.6 Handoff to the next program

When this program exits, the natural successors are: the companion
EN/XP-rescope program (its gates mature as Plans 05/06 complete), the
census tranche-2 audits (E1's ranked queue feeds them), and the Wave 12
Seal-steps documents once their own predecessor chains are certified. The
2026-09-19 unblocked-plans audit remains the queue baseline until the next
re-rank; nothing in this program supersedes it — it executes the frontier
the audit measured.

---

# Part H — Evidence index (citations used in Part C)

| Claim | Evidence |
|---|---|
| Distress mechanism sealed; trigger grammar; V5→V6 | `Assets/Ashfall.Core/Radio/DistressFollowUpScheduler.cs`; `RadioSave.cs`; `INTEGRATION_PLANS.md` Wave 3 row |
| Distress content counts (17+7 follow-ups; 66+36 cues) | `radio_distress_signals.json`; `radio_distress_signals_expansion.json` (grep counts, 2026-09-19) |
| PR3 rules absent; no PR3 closeout | grep over `CatalogIntegrityValidator.cs`; `docs/radio/` listing |
| Restock implementation live | `Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs:136,273,283,307-316,532`; `Plan147RestockPriorityTests.cs` (6/6 re-run) |
| DEC-05 signed; count drift 14→6 | `docs/governance/DECISION_REGISTER.md` DEC-05; focused test run 2026-09-19 |
| Vehicle owner + seam; no armor grades | `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs:9-33`; `WORKTREE_OWNERSHIP.md` Wave 8 B2 row; grep `armor_grade|ArmorGrade|armorGrade` = 0 hits |
| Bootstrap single call site; fresh path lacks it | `src/Main.Lifecycle.cs:542`; `src/Main.SaveOrchestrator.cs:163`; `src/Main.CampaignServices.cs:25+` |
| Difficulty catalog 4 presets × 8 scalars; default-only resolve; one consumer; no persistence/panel | `Assets/StreamingAssets/Data/difficulty_presets.json`; `src/Main.Difficulty.cs:28`; `src/Main.EvolvingWorld.cs:193`; `src/Main.Endgame.cs:74`; `Assets/Ashfall.Core/Endgame/CampaignCompletionHistory.cs:44`; grep over `SaveSectionRegistry.cs`/`PanelRegistryBootstrap.cs` |
| E1 READY-UNCLAIMED; E1A–E1P order | `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` E1 row |
| Plan 37 corpus contract, order, hard gate, dependencies | `C-integration-plans/C2_planintegration[15].md` header + §0 |
| Plan 48 corpus contract, order, hard gate, dependencies | `C-integration-plans/C2_planintegration[21].md` header + §0 |
| Input action vocabulary | `src/Host/AshfallInputActions.cs:15-44` |
| Port contract 0 deferred | `python3 scripts/ci/generate-port-contract.py --check` (2026-09-19) |
| Ten seals; census drain 117→112; 8 available | `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` §1–§6 |
| Quarantine count 48 | `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` `Compile Remove` count (2026-09-19) |
| Active claims | `WORKTREE_OWNERSHIP.md` §Active claims (rows 11–12) |

---

*End of program document. Planning authority only: register in `docs/INDEX.md`
when the integrator next regenerates the index (this document intentionally
does not edit it — the index and the live ledgers have in-flight edits as of
2026-09-19).*
