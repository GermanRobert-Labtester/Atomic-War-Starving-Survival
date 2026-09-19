# ASHFALL — Fifteen Unblocked Partial Integrations: Completion-First Full Integration & Enhanced Expansion Program

**Date:** 2026-09-18
**Series:** Seal-steps integration programs (successor in genre to
`ashfall-six-unblocked-partial-integration-plans-full-implementation-plan.md`)
**Repo state inspected:** branch `Zcode_Branch`, working tree as of 2026-09-18
(six freshly sealed debts staged; XP Expansion W1 active).
**Document role:** planning authority only. Nothing in this file edits code,
data, ledgers, or claims. Every package below names the evidence a builder must
re-verify before editing, per AGENTS.md rule 7 ("use current evidence").

---

## How to read this document

- **Part A** establishes current reality: what was just sealed, what the active
  batch is, and what the measured queue says.
- **Part B** explains the completion-first selection, lists the fifteen
  roster entries with their unblocking cause and signature gate, and names the
  deliberate exclusions with reasons.
- **Part C** is the integration program proper: one implementation-ready plan
  per roster entry, each following the ashfall-plan contract (current reality,
  delta, ownership, state/save/determinism, host wiring, failure modes, tests,
  dependency-ordered phases, file impact map, risks, rollback, handoff).
- **Part D** contains the purpose-enhanced expansion plans (EN-01 … EN-08):
  new capability proposals that only become buildable **on top of** the
  completed fifteen, each grounded in seams the fifteen create or finish.
- **Part E** sequences everything (dependency DAG, wave order, follow-on
  horizon for XP-07 … XP-10).
- **Part F** consolidates every foreman signature this program needs, so the
  decision queue is visible in one place.
- **Part G** closes with program-level verification, rollback, out-of-scope
  list, and the final implementation handoff.

**Authority compliance baked into every plan below:**

1. Godot is authoritative; Core (`Assets/Ashfall.Core/`) stays engine-free.
2. JSON in `Assets/StreamingAssets/Data/` stays the sole authored authority.
3. One authority per concern: every plan extends the verified current owner;
   no plan creates a parallel ledger, registry, save store, or manager.
4. Deterministic Core behavior uses the existing seeded RNG contract; no
   wall-clock or hash-order seeding anywhere.
5. Signed decisions are respected, not reversed: DEC-06 (signal-trust
   availability consumer retired) stays retired; XP-09/XP-10 are explicitly
  premise-check-first because the Wave 10 Part 1 A3 micro-deferral sweep
  recorded presenter skills and phobia growth as STALE/RETIRED deferral rows.
6. Panels present existing commands and truthful state; they never recompute
   Core outcomes.
7. A green compile is not integration: each plan's definition of done includes
   the observable outcome, save/restore where stateful, and focused tests.

---

# Part A — Current Reality (verified 2026-09-18)

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

## A.2 Six debts sealed in this tree on 2026-09-18 (the unblocking wave)

These seals are the reason a completion-first program is now possible; each is
verified in source, staged but mostly uncommitted:

1. **`DEBT-PLAN24-MEDICAL-WARD-STAFFING` — RETIRED (sealed).** `ward` role
   authored in `duty_roles.json` (skill `skill_paramedic`, hazard class
   `medical`), `DutyRosterIds.RoleWard` added to `AssignmentRoles`,
   `FitnessForDutyModel` knows hazard class `medical`,
   `MedicalWardSystem.StaffingPreflight` gates `RunProcedure` with
   `Fail("ward_unstaffed")`, and `src/Main.Medical.cs` binds the preflight to
   the duty roster's ward assignment. Tests: `MedicalWardSystemTests` 14/14.
2. **`DEBT-PLAN28-MAIN-CONSTRUCTOR-MIGRATION` — RETIRED (sealed).**
   `SubsystemDescriptor.SetupAction`, `RegisterSetupAction`,
   `ExecuteSubsystemManifestBootstrap` in `src/Main.Lifecycle.cs`; 18 host
   setup delegates registered idempotently; invoked from
   `RestoreAllSubsystemsFromDisk()` in `src/Main.SaveOrchestrator.cs`.
   *Residual gap this program adopts:* the bootstrap currently runs only on
   the restore-from-disk path (see Plan 10).
3. **`DEBT-PLAN32-MAP-ORPHANS` — RETIRED (sealed).** Ten authored `loc_*`
   stubs added to `locations.json`; loader gate
   `AllMapNodes_ExistInLocationsCatalog` (5/5) makes orphan graph nodes a
   build-time failure.
4. **`DEBT-PLAN125-SOFC-INVENTORY-FUEL` — RETIRED (sealed).**
   `src/Main.Plans122to125.cs` binds `FuelConsumer = ConsumeSofcFuel`
   (clean→treated→dirty canister tiers, 25 units per canister, power-grid
   reserve fallback). This seal is also the **premise correction** that
   reshaped XP-05 (A.4).
5. **`DEBT-PLAN30-CONSEQUENCE-REACH` (+ Plan 123 sound-ranging producer) —
   RETIRED (sealed).** `WireFactionWarConsequenceRouting()` in
   `src/Main.YearOfAsh.cs`: territorial clash → radio intercept + permanent
   journal entry + sound-ranging hostile-fire record; decree enacted → radio
   intercept. This proves the routing template Plan 30's remaining projection
   consumers will reuse (Plan 06).
6. **`DEBT-PLAN34-DIFFICULTY-CHRONICLE-AUTHORITY` — still DECISION-BLOCKED**,
   but its blocker ("no canonical difficulty authority exists") is removed the
   moment XP-01's director is bound — that is Plan 08/Plan 13's job.

## A.3 Active batch: XP Expansion W1

`INTEGRATION_PLANS.md` declares **XP Expansion W1 — ACTIVE (2026-09-18)**,
package `XP-WAVE1-DIFFICULTY-AUTHORITY`, claim
`claim-xp-wave1-difficulty-2026-09-18` (owns
`Assets/Ashfall.Core/Difficulty/`, `difficulty_presets.json`,
`Ashfall.Core.Tests/Difficulty/`; explicitly excludes the Wave-11-owned
completion-history paths).

Implemented and verified (untracked): `DifficultyPresetCatalog.cs` (251
lines), `DifficultyDirector.cs`, `DifficultyScalarsProvider.cs`,
`difficulty_presets.json` (presets `difficulty_sparing` / `difficulty_standard`
/ `difficulty_austere`; eight scalars each; `starting_bonus_item_ids` uses the
real ids `canned_food` / `iodine_pills`), `DifficultyPresetCatalogTests` +
`DifficultyDirectorTests` (8/8), and `CatalogIntegrityValidator` integration
(fail-closed loader, strict cross-catalog reference key).

Deliberately not yet present: campaign binding, `difficulty_preset_id`
persistence, all scalar consumers, `DifficultyHostSession`, chronicle
projection. The decision packet D1 keeps the consumer list closed until each
live calculation site is premise-checked. **Plan 13 completes this batch.**

## A.4 Premise corrections already banked (do not re-litigate)

- **XP-05 SOFC fuel is already live.** The proposal's `FuelConsumer = units =>
  true` placeholder does not exist at the current revision;
  `Main.Plans122to125.cs:51` binds real inventory fuel with grid fallback, and
  `sofc_power_catalog.json` owns the fuel-quality profiles. W1 records this
  and adds nothing. No duplicate fuel catalog, buffer, or save section.
- **Difficulty starter items corrected.** The proposal's
  `item_canned_rations` / `item_iodine` do not exist; the implemented catalog
  uses `canned_food` / `iodine_pills` under a strict reference key.
- **Known open wart (recorded in W1 evidence):** the shared scratch fixture
  behind `CatalogIntegrityValidatorTests` does not yet seed the new `ward`
  role, producing four unrelated fixture failures; shipped-data integrity
  passes. Plan 01's reconciliation phase absorbs this.

## A.5 The measured queue

Per `docs/plans/wave11_part2/C2_CENSUS_REFRESH.md`: **117 nonterminal corpus
rows** (111 AUDIT-PENDING + 5 PARTIALLY-SEALED + 1 READY-UNCLAIMED). No Wave
12 head is certified. The five PARTIALLY-SEALED rows include the Plan 30/32/34
remainders this program completes; the READY-UNCLAIMED row is E1/Plan 53
(governance programme, tied into EN-07 rather than a slot). Wave 11 Part 2
C1/C2 governance is blocked pending foreman signatures — the consolidated
signature queue is Part F of this document.

---

# Part B — Completion-First Selection

## B.1 Selection rule

A roster entry must satisfy all four:

1. **Partially integrated today** — a real system/contract exists in source
   or data, and a named remainder keeps it from full integration.
2. **Unblocked** — the blocker that held it is verifiably gone (a debt sealed,
   an owner created, an authority landed), or it is a pure verify-and-reconcile
   item whose evidence already exists.
3. **Completion-first value** — finishing it retires a ledger row, unseals a
   census row, or completes the active batch, rather than opening new green
   field.
4. **Governance-safe** — it extends a current owner and requires no reversal
   of a signed decision.

## B.2 The roster

| # | Package | Anchor | What is partial | Unblocked by | Signature gate |
|---|---|---|---|---|---|
| 01 | `CF-P24-CLOSURE` | Plan 24 / C1[5] | D2 recovery-ramp decision; ledger/commit reconciliation of the sealed ward work; snapshot rebaseline | Ward staffing sealed in-tree 2026-09-18 | D2 (packet option ii recommended) |
| 02 | `CF-P1-DISTRESS-CONTENT-SEAL` | Distress P1 | Follow-up/audio content committed but validator rules, population replay, utilization seal, PR3 closeout missing | Contracts frozen since Waves 3–5; content now in catalogs | none (verify-and-seal) |
| 03 | `CF-P5-RESTOCK-RECONCILE` | Merchant restock P5 | DEC-05 signed and tests green, but two ledgers still say deferred/pending; binding of the ordering into the restock path to be re-verified | DEC-05 signature + `Plan147RestockPriorityTests` 14/14 | foreman line to reconcile ledger rows |
| 04 | `CF-P6-VEHICLE-ARMOR-GRADES` | Plan 213 D6 / P6 | 4 armor grade tiers unimplemented | Vehicle owner + decoration seam landed (`--vehicle-garage-selftest` 19/19) | new claim + premise note (D6 was an approved deferral) |
| 05 | `CF-P3-SEMANTIC-KIND-AUTHORITY` | Plan 31 / P3 | 17C-I alert ducking, 17C-E acquisition sweep (26 routes), 17B deep matrix | `DayEventVocabulary` + parity gate landed; census done | D11 (`GenericSectionTitle` pin) |
| 06 | `CF-P30-WAR-PROJECTION-CONSUMERS` | Plan 30 / C2[10] | 3 projection events have zero subscribers; runtime-clock mismatch (content days 480–607 vs campaign 180–360) | Consequence-reach seal 2026-09-18 provides the routing template | D5/D6/D7/D14 (sign together) |
| 07 | `CF-P32-GRAPH-TRAVEL` | Plan 32B/32C ⇄ XP-02 | Expeditions/caravans never query `WastelandMapSystem`; 32C knowledge gating unbuilt | 32A orphan hygiene sealed; 68-route save persistence green | D8 (+D10 for 32C) |
| 08 | `CF-P34-DIFFICULTY-CHRONICLE` | Plan 34B/34C / C2[12] | Difficulty chronicle consumer + append-only-compatible history shape | XP-01 director (Plan 13) creates the missing authority | claim transfer from Wave 11; shape decision |
| 09 | `CF-P36C-PORT-SEAM-SWEEP` | Plan 36C / C2[13] | 17 DEFERRED port seams remain | 248-seam policy, CI generator, `--port-contract-selftest` all green | none (promotion condition stated) |
| 10 | `CF-P28-ONE-BOOTSTRAP-PATH` | Plan 28 / C2[9] | Census row stale; manifest bootstrap runs only on the restore path | Setup-action migration sealed 2026-09-18 | none (reconcile + bounded host change) |
| 11 | `CF-P26A-FORBIDDEN-PATH-SWEEP` | Plan 26A tranche 2 | ~40 files still read `res://…/Data` paths directly | Tranche-1 pattern + green shrinking gate exist | none |
| 12 | `CF-QD-QUARANTINE-DRAIN-TRANCHE` | Test quarantine D21 | ~50 `Compile Remove` exclusions; formal quarantine manifest empty | Per-file reinstatement pattern proven 5× | D21 foreman authorization per file |
| 13 | `CF-XP01-DIFFICULTY-FULL-BINDING` | XP-01 (active W1) | Core/catalog/tests done; campaign binding, persistence, consumers, host, panel missing | W1 authorization (D1 signed, narrowed) | per-consumer premise checks |
| 14 | `CF-XP04-ECONOMY-LEGS` | XP-04 (W2) | Funds authority undefined for black-market legs; heat/counterfeit/restock design unsigned | Black-market settlement surface sealed; DEC-05 restock precedent | D3/D4-class design sign-off |
| 15 | `CF-XP06-BODY-INTEGRITY` | XP-06 ⇄ DEC-03 split | Equipment half of amputation restriction blocked on schema | Sealed `AmputationSystem` + `EquipmentConditionSystem` + crafting chains live | schema package sign-off (DEC-03 successor) |

## B.3 Deliberate exclusions (with reasons)

- **P4 signal-trust availability consumer** — would reverse signed DEC-06
  (retired to avoid a parallel trust authority). Not planned. Reversal is a
  foreman decision, not a builder package.
- **Plans 42/46 (C2[18]/C2[20]) and Plan 49** — AUDIT-PENDING / DECIDED-DEFERRED
  per the Wave 12 Part 1.1 audit; promotion requires their own audits.
- **Census tranche-2 per-clause audits (111 rows)** — real unblocked work, but
  audit-only, not integration; EN-08 folds the census rerank into the ledger
  truth program instead.
- **E1/Plan 53 (READY-UNCLAIMED)** — a governance programme, not a partial
  integration; tied in as EN-07's readiness condition.
- **XP-07/08/09/10** — not completion-first (green-field pillars). They remain
  the sequenced follow-on horizon in Part E, with mandatory premise checks
  (XP-09/XP-10 especially, given the STALE/RETIRED micro-deferral history).
- **Snapshot rebaseline** — environment-blocked (needs a renderer-capable
  session); appears only as a named step inside Plan 01.

---

# Part C — The Fifteen Integration Plans

Reading key for every plan below: "Current reality" cites file evidence a
builder must re-verify before editing; "Phases" are dependency-ordered with a
completion gate each; "Focused verify" uses `bash scripts/run_test.sh <target>`
(180-second cap) plus the named headless selftests, per TEST_POLICY.md. No
plan here authorizes a full-suite run by default.

---

## Plan 01 — Plan 24 Closure: Recovery Ramp & Ledger Reconciliation (`CF-P24-CLOSURE`)

**Anchor:** `docs/plans/PLAN_24_CLOSEOUT.md`, decision packet item D2.
**Status:** 12 of 14 Plan 24 gates closed-with-evidence; ward staffing sealed
in-tree but unreconciled; recovery ramp unsigned; snapshots environment-blocked.
**Unblocked by:** the 2026-09-18 ward-staffing seal (A.2.1).

### C.1.1 Current reality (verified)

- Ward staffing is implemented end-to-end: `duty_roles.json` `ward` role,
  `DutyRosterIds.RoleWard`, `MedicalWardSystem.StaffingPreflight` gate,
  `src/Main.Medical.cs` roster binding, `MedicalWardSystemTests` 14/14 — but
  the work is staged-uncommitted and `PLAN_24_CLOSEOUT.md` /
  `INTEGRATION_PLANS.md` still describe the item as open/awaiting signature.
- No `recovery_ramp` code or data exists anywhere (grep-verified).
- The D2 packet records three options; **option ii is zero-code** (accept the
  current single recovery-rate authority as the closed decision and record it),
  option i authors affliction-specific ramps as data, option iii defers again.
- The shared scratch fixture behind `CatalogIntegrityValidatorTests` misses
  the new `ward` role (4 unrelated failures) — A.4's wart.

### C.1.2 Required delta

1. A foreman line on D2 (recommend option ii; option i becomes Plan 15-adjacent
   data work only if signed).
2. Ledger truth: closeout + `INTEGRATION_PLANS.md` + census reflect the sealed
   ward staffing; the staged files land in a commit.
3. The scratch fixture seeds `ward` so the validator fixture is green again.
4. Snapshot rebaseline executed when a renderer-capable session exists
   (recorded as environment-blocked, not silently dropped).

### C.1.3 Ownership & seams

| Concern | Owner |
|---|---|
| Recovery-rate arithmetic | existing affliction/medical authority (unchanged under option ii) |
| Ward role data | `duty_roles.json` (done) |
| Staffing gate | `MedicalWardSystem.StaffingPreflight` (done) |
| Closeout/ledger rows | foreman/integrator only |

### C.1.4 Phases

- **P0 — Verify:** re-run `MedicalWardSystemTests` (14/14), confirm staged
  diff still matches HEAD+index; re-grep for `recovery_ramp` (must be absent).
  Gate: evidence re-confirmed.
- **P1 — Fixture repair:** seed `ward` in the shared scratch fixture; run
  `CatalogIntegrityValidatorTests`. Gate: fixture green, shipped-data check
  unchanged.
- **P2 — Ledger reconcile (foreman/integrator):** record D2's signed option;
  flip closeout/ledger wording to sealed; commit the staged ward work as one
  reviewable unit. Gate: `git status` clean for those paths; docs index check
  `python3 scripts/ci/generate-docs-index.py --check`.
- **P3 — Snapshot rebaseline (when environment allows):** rerun the UI
  snapshot QA pass (xvfb render command per the established vision-QA flow);
  record DRIFT interpretation. Gate: snapshots reflect current panels.

### C.1.5 Failure modes

Old saves without `ward` assignments (preflight falls open by design when the
roster is null — verified); fixture drift re-introduced by future role adds
(kept honest by P1); D2 left unsigned (package stops at P1 — do not improvise
a ramp, rule 10).

### C.1.6 Test strategy & focused verify

`bash scripts/run_test.sh Ashfall.Core.Tests/Medical/MedicalWardSystemTests.cs`
(14/14); `CatalogIntegrityValidatorTests` after fixture repair; docs-index
`--check`. No new tests — this package reconciles truth, it does not add
behavior.

### C.1.7 File impact map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `Ashfall.Core.Tests` shared scratch fixture | MODIFY | seed `ward` | low |
| `docs/plans/PLAN_24_CLOSEOUT.md`, `INTEGRATION_PLANS.md` | MODIFY | record D2 + sealed staffing | low (integrator) |
| staged medical/roster/data files | COMMIT (no content change) | land the seal | low |

**Out of scope:** any new recovery mechanics (unless D2 signs option i as a
separate package); census row rewrites beyond Plan 24's own.

**Handoff — MUST PRESERVE:** existing preflight fall-open semantics. **MUST
ADD:** ledger truth only. **MUST NOT DO:** author ramp data without a signed
D2. **VERIFY WITH:** the three commands above. **FIRST SAFE STEP:** P1 fixture
repair (test-only, immediately green).

---

## Plan 02 — Distress Follow-Up & Audio Content Seal (`CF-P1-DISTRESS-CONTENT-SEAL`)

**Anchor:** Seal-steps P1 (distress follow-up/audio), Waves 3–5 contracts.
**Status:** mechanism frozen and green since 2026-09-13; content now committed
(`radio_distress_signals.json`: 17 `follow_up_signals`, 66 `audio_cue`
occurrences; expansion 7/36); the seal tail (validator rules, population
replay, utilization, closeout) never ran.
**Unblocked by:** content landing in shipped catalogs — this is now a
verify-and-seal package, not authoring.

### C.2.1 Current reality (verified)

- `DistressFollowUpScheduler` (closed trigger grammar: answered /
  rescue_success / rescue_failed / expired / trap_fallen_for; exactly-once
  ledgers; V5→V6 frozen save) and `DistressAudioCueResolver`
  (Intercept-gated playback, persisted dedupe) are sealed Core.
- Catalogs contain real follow-up and cue content; the third companion doc
  additionally holds 23 + 16 identity-aligned entries marked `[drafted]` —
  treat those as **candidate** content, not shipped, until walked through the
  validator.
- The deferred tail names exactly: forbid expired-with-no-consequence, forbid
  trap grammar on genuine-only signals, max-2 follow-ups per signal, population
  replay tests, audio registry verification, `--content-utilization-selftest`
  with 0 orphans, PR3 closeout + ledger row.

### C.2.2 Required delta

1. Three validator rules in `CatalogIntegrityValidator` (data-shape only, no
   runtime behavior change).
2. Population replay tests exercising every authored follow-up chain and cue
   through the real V6 codec.
3. Audio-cue id registry verification (every referenced cue id resolvable or
   explicitly text-only).
4. PR3 closeout document + `INTEGRATION_PLANS.md` row marking P1 sealed.

### C.2.3 Ownership & seams

| Concern | Owner |
|---|---|
| Follow-up grammar/semantics | `DistressFollowUpScheduler` (extend nothing) |
| Cue resolution | `DistressAudioCueResolver` (unchanged) |
| Content rules | `CatalogIntegrityValidator` (additive rules) |
| Replay evidence | new focused test file |

### C.2.4 State/save/determinism

No new state. Replay tests must reuse the existing deterministic replay
harness pattern (Scenario A–D precedent) — same seed, same days, byte-stable
fingerprints. No RNG additions.

### C.2.5 Host & UI wiring

None. Playback and Intercept gating already work; this plan changes catalogs,
validator, and tests only.

### C.2.6 Failure modes

Authored chain that can never fire (trigger unreachable in current mission
flow) — the population replay must fail loudly on it; cue id drift between
catalog and registry — caught by the registry walk; a genuine signal
authoring trap-only follow-ups — forbidden by rule 2; >2 follow-ups stacked —
forbidden by rule 3.

### C.2.7 Phases

- **P0 — Census:** list every `follow_up_signals` / `audio_cue` row in shipped
  catalogs vs the `[drafted]` companion set; classify shipped vs candidate.
  Gate: written census.
- **P1 — Validator rules:** add the three rules with per-row error strings
  (catalog/signal/path/id/field/rule). Gate: `--data-integrity-selftest` green
  on shipped data; a deliberately-broken fixture fails each rule.
- **P2 — Population replay:** one focused test file driving every chain
  through the real codec. Gate: new file alone first, then Radio directory.
- **P3 — Utilization & registry:** `--content-utilization-selftest` 0 orphans;
  audio registry walk clean. Gate: both green.
- **P4 — Closeout:** PR3 document + ledger row; optionally fold the
  `[drafted]` candidates through P1–P3 as a second content tranche.

### C.2.8 Focused verify

`godot --headless --path . -- --data-integrity-selftest`;
`--content-utilization-selftest`; `--audio-selftest`;
`bash scripts/run_test.sh Ashfall.Core.Tests/Radio/` (expect ~321+ baseline to
grow by the new replay file only).

### C.2.9 File impact map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | MODIFY | 3 additive content rules | low |
| `Ashfall.Core.Tests/Radio/` new replay file | CREATE | population evidence | low |
| `radio_distress_signals*.json` | READ ONLY (unless P4 tranche) | authority | low |
| PR3 closeout + `INTEGRATION_PLANS.md` | MODIFY | seal record | low (integrator) |

**Out of scope:** new mechanics, new cue resolution semantics, trust changes.

**Handoff — MUST PRESERVE:** frozen V6 save shape; Intercept-only playback.
**MUST ADD:** validator rules + replay evidence + closeout. **MUST NOT DO:**
runtime behavior changes for content's sake. **VERIFY WITH:** C.2.8.
**FIRST SAFE STEP:** P0 census (read-only).

---

## Plan 03 — Merchant Restock Priority Reconcile-and-Complete (`CF-P5-RESTOCK-RECONCILE`)

**Anchor:** P5 / DEC-05 / Plan 147 restock.
**Status:** DEC-05 is **SIGNED** ("priority-weighted tier restock with
deterministic PRNG… Wave 9 Part 2 C1 (`SEALED`)"), `Plan147RestockPriorityTests`
exists (14/14), `ShelterBarterSystem.ComputeItemPriorityScore` lives in Core
and changed in the latest commit — yet `INTEGRATION_PLANS.md` still says
"Still deferred with authority question" and
`docs/plans/wave9_part2/C1_DECISION.md` shows "[PENDING FOREMAN DECISION]".
**Unblocked by:** the signature and the tests already existing. This is a
verify-and-reconcile package first, completion second.

### C.3.1 Required delta

1. **Verify the binding:** confirm the priority ordering is actually consumed
   by the live restock path (day-gated Plan 147 restock), not just scored in
   isolation. If a call site is missing, wire it (bounded, single seam).
2. **Ledger truth:** three documents disagree; the reconciled state (DEC-05
   signed; tests green; binding verified or completed) is recorded once.
3. **XP-04 hand-off note:** XP-04's deterministic restock priority must
   reference this sealed design, not re-specify it (Plan 14 depends on this
   note to avoid a duplicate design).

### C.3.2 Ownership & seams

| Concern | Owner |
|---|---|
| Priority scoring | `ShelterBarterSystem.ComputeItemPriorityScore` (Core) |
| Restock cadence/day gate | Plan 147 restock owner |
| Determinism | existing seeded PRNG stream for restock |
| Ledger rows | integrator |

### C.3.3 Phases

- **P0 — Evidence pass:** read `Plan147RestockPriorityTests` + the restock
  call path; write one paragraph of premise evidence (bound or not bound).
  Gate: written verdict.
- **P1 — (conditional) bind:** if unbound, add the single ordering call at the
  restock site; extend the focused test to prove ordering changes seeded
  outcomes. Gate: tests green including determinism pins.
- **P2 — Reconcile:** update the two stale docs + `INTEGRATION_PLANS.md` row.
  Gate: no document still claims "pending".
- **P3 — Cross-reference:** add the one-line note into the XP-04 planning
  context (Plan 14's P0 will consume it).

### C.3.4 Focused verify

`bash scripts/run_test.sh Ashfall.Core.Tests/<Plan147RestockPriorityTests path>`
(14/14 baseline); determinism seed-sweep for the restock stream if P1 runs.

### C.3.5 Failure modes

Reconciling wording without verifying the binding (the exact failure mode this
package exists to prevent); re-implementing scoring (duplicate authority);
changing PRNG stream order (determinism break — pinned by sweep).

**Handoff — MUST PRESERVE:** DEC-05's signed design verbatim. **MUST ADD:**
verified binding + reconciled ledgers. **MUST NOT DO:** a second priority
system (also binding for XP-04). **VERIFY WITH:** C.3.4. **FIRST SAFE STEP:**
P0 evidence pass (read-only).

---

## Plan 04 — Vehicle Armor Decoration Grades (`CF-P6-VEHICLE-ARMOR-GRADES`)

**Anchor:** Plan 213 D6 / Seal-steps P6 + deep-dive Part B.
**Status:** D6 was deferred because "no vehicle owner exists"; the owner now
exists — `VehicleGaragePanel`, the signed Plan 50 decoration seam (mod effects
decorate the expedition profile, trip distance feeds component wear,
immobilized vehicles refuse dispatch), `--vehicle-garage-selftest` 19/19. No
`armor_plates` content exists yet.
**Unblocked by:** the vehicle owner + decoration seam landing (Wave 8 B2).

### C.4.1 Current reality (verified)

- `SilentFoundrySystem.TryGetLatestMaterialQuality` is the read-only
  material-quality handoff query created for exactly this deferral (purity +
  craft quality permille ride `FoundryProductionRecord`).
- Deep-dive Part B specifies 4 grade tiers grounded in
  `commodity_baselines.json` / `scavenging_tables.json` (no invented
  economies); grades decorate the expedition profile through the existing
  seam — they do not create a second defense system.
- `DefenseSystem`, wear, and dispatch preflight are existing owners this must
  extend, not duplicate.

### C.4.2 Required delta

1. `vehicle_armor_grades.json` (or additive rows in the vehicle catalog's
   owning file): 4 tiers, material/grade prerequisites, bounded damage
   reduction, wear sensitivity.
2. Core: grade resolution + application through the decoration seam (pure,
   engine-free, deterministic).
3. Crafting/installation commands through the foundry/garage owners.
4. Panel presentation: grade shown in `VehicleGaragePanel` (truthful state
   only).

### C.4.3 Ownership & seams

| Concern | Owner |
|---|---|
| Grade catalog | new JSON under `Assets/StreamingAssets/Data/` |
| Grade arithmetic | Core extension beside the vehicle/expedition profile |
| Application | existing decoration seam (Plan 50) |
| Material quality input | `SilentFoundrySystem` query (read-only) |
| UI | `VehicleGaragePanel` |

### C.4.4 State/save/determinism

Per-vehicle installed grade + condition rides the existing vehicle/garage
save section (additive fields; old saves = no armor, never fabricated). Wear
accrues from the existing trip-distance feed. No new RNG; grade outcomes are
deterministic functions of material quality + tier.

### C.4.5 Failure modes

Old saves (no grade → neutral); grade on immobilized vehicle (dispatch already
refused — presentation must not imply protection); material-quality missing
(foundry never ran → default Standard precedent from Plan 213); overpowered
tiers (bounded reduction, never immunity — hazard-reduction precedent from
run-flat); duplicate defense arithmetic (forbidden — `DefenseSystem` stays the
combat authority).

### C.4.6 Phases

- **P0 — Claim + premise note:** new claim (D6 was an approved deferral);
  re-verify the decoration seam API and garage selftest. Gate: claim recorded.
- **P1 — Catalog + Core:** grades JSON + loader + grade resolution; catalog
  integrity rules (tier ranges, cross-references). Gate: loader tests +
  data-integrity selftest.
- **P2 — Seam application:** decorate expedition profile via the Plan 50 seam;
  wear integration; save additive fields + round-trip tests.
- **P3 — Craft/install commands:** foundry-quality-gated installation through
  existing command owners.
- **P4 — Panel + selftest:** garage panel grade display; extend
  `--vehicle-garage-selftest`. Gate: 19/19 baseline grows, all green.

### C.4.7 Focused verify

New focused test file alone first; `--vehicle-garage-selftest`;
`--data-integrity-selftest`; `bash scripts/run_test.sh` on the owning test
directory.

### C.4.8 File impact map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `Assets/StreamingAssets/Data/` grades JSON | CREATE | authority | low |
| Core vehicle/expedition profile area | MODIFY | grade resolution | medium (shared seam) |
| garage save section | MODIFY (additive) | persistence | low |
| `src/UI/VehicleGaragePanel.cs` | MODIFY | presentation | low |

**Out of scope:** new combat math, new wear system, aviation/naval armor.

**Handoff — MUST PRESERVE:** DefenseSystem as combat authority; decoration
seam semantics. **MUST ADD:** bounded grade decoration. **MUST NOT DO:**
immunity tiers or a parallel protection ledger. **VERIFY WITH:** C.4.7.
**FIRST SAFE STEP:** P0 claim + premise verification.

---

## Plan 05 — Semantic-Kind Authority Completion (`CF-P3-SEMANTIC-KIND-AUTHORITY`)

**Anchor:** Plan 31 / C2[2]-era deferrals; Seal-steps P3 + deep-dive Part A.
**Status:** foundation landed (`DayEventVocabulary`, parity-matrix gate
`EVENT_SEMANTIC_PARITY_MATRIX.md`); the briefing re-grouping is contract-gated
because `DayEventVocabularyTests` pins `GenericSectionTitle` for unhandled
kinds; the deep-dive enumerates 26 acquisition routes (5 KEEP / 6 EMIT-ADD /
11 EMIT-COMPLETE / 2 DEDUPE-RULE); 17C-I (alert ducking/concurrency), 17C-E
(acquisition sweep), and 17B (deep test matrix) remain.
**Unblocked by:** D11 once signed — it is the only packet item with no
recorded recommendation, so this plan **proposes** one.

### C.5.1 Proposal for D11 (the missing recommendation)

Keep the `GenericSectionTitle` pin as the **failure signal**, not the
destination: every semantic kind that reaches production must be handled by
the vocabulary (authored section title) or be reclassified at emit time.
Concretely: (a) `DayEventVocabulary` gains authored section titles for the 11
EMIT-COMPLETE kinds; (b) the parity gate grows a rule that any new kind
without a title fails integrity (fail-closed, same pattern as the orphan-node
gate); (c) `GenericSectionTitle` remains only for genuinely unclassifiable
legacy rows, which the census must count and drive to zero over time.

### C.5.2 Required delta

1. D11 signed with the recommendation above (or a foreman variant).
2. 17C-E acquisition sweep executed per the 26-route census (emit-side
   completions; dedupe rules for the 2 DEDUPE rows).
3. 17C-I: alert ducking/concurrency policy for audio events (extends the
   existing audio bridge owner; radiation exposure-end lifecycle precedent).
4. 17B: deep test matrix over briefing grouping (kind × section × owner
   failure visibility).

### C.5.3 Ownership & seams

| Concern | Owner |
|---|---|
| Kind vocabulary & titles | `DayEventVocabulary` (Core) |
| Briefing grouping | existing briefing builder (consumer of vocabulary) |
| Audio ducking | existing audio event bridge |
| Parity enforcement | parity-matrix gate + integrity rules |

### C.5.4 Phases

- **P0 — Signature:** D11 decision recorded. Gate: foreman line.
- **P1 — Vocabulary completion:** titles for the 11 kinds; integrity rule
  (new-kind-must-have-title); census count of remaining generic rows.
- **P2 — Emit sweep:** the 6 EMIT-ADD + 11 EMIT-COMPLETE routes wired at
  their emit sites; 2 DEDUPE rules authored. Gate: parity matrix green;
  generic-row count shrinks measurably.
- **P3 — 17C-I ducking:** concurrency policy in the audio bridge; focused
  tests for overlapping alert classes.
- **P4 — 17B matrix:** briefing deep tests. Gate: all green.

### C.5.5 Focused verify

`bash scripts/run_test.sh` on the vocabulary/parity test files; briefing
builder tests; `--data-integrity-selftest`.

### C.5.6 Failure modes

Silent re-introduction of unhandled kinds (P1's fail-closed rule is the
guard); ducking policy starving priority audio (bounded, precedence-ordered);
emit-site edits drifting from census (each route cites its census row).

**Handoff — MUST PRESERVE:** parity gate semantics; briefing builder as
consumer. **MUST ADD:** fail-closed title rule + emit completions. **MUST NOT
DO:** reclassify kinds inside panels. **VERIFY WITH:** C.5.5. **FIRST SAFE
STEP:** P1 vocabulary titles (Core + data, no host change).

---

## Plan 06 — Plan 30 War-Projection Consumers & Runtime Clock (`CF-P30-WAR-PROJECTION-CONSUMERS`)

**Anchor:** Plan 30 / C2[10]; `docs/plans/wave11_part1/B1_PLAN30_IMPLEMENTATION_LOG.md`;
XP-03 is the consequence-layer vehicle this anchors.
**Status:** `SimulateDailyFriction` + `FactionWarChainRunner.TickDay` wired;
consequence-reach sealed 2026-09-18 (clash + decree routed to radio/journal/
sound-ranging); **three projection events still have zero subscribers**
(`OnStageSurfaced`, `OnStageResolved`, `OnChainResolved`,
`FactionWarChainRunner.cs:311-313`); the runtime clock is the headline blocker:
war content authored for days 480–607 while the campaign clamps at 180–360
(`Main.CampaignOwners.cs:1396-1399`).
**Unblocked by:** the sealed consequence-routing template (A.2.5) proving the
radio/journal/sound-ranging seams accept war events.
**Gate:** D5/D6/D7/D14 — the packet says sign these four together.

### C.6.1 Current reality (verified)

- The producer exists and ticks; the war map widget consumes standing changes;
  the routing pattern for clash/decree is sealed and testable.
- Nothing consumes stage surfacing/resolution or chain resolution — the war
  chain currently resolves silently.
- Authored war-chain stages live outside the playable window for most campaign
  lengths; no `WorldClockHorizon`-style mapping exists.

### C.6.2 Required delta

1. **D5 (clock):** map authored war days onto the live campaign window. XP-03's
   `WorldClockHorizon` with `linear_compress` policy + a 2-day storytelling
   floor is the recommended shape; the foreman signs the policy before any
   consumer lands.
2. **Projection consumers:** `OnStageSurfaced` → daily briefing crisis line +
   journal entry; `OnStageResolved` → journal + faction-standing consequence
   through the standing engine; `OnChainResolved` → journal epilogue entry +
   (post-Plan 08) chronicle line. All reuse the sealed routing template.
3. **D6 (30B scope) / D7 (30C autonomy):** decide the boundary of simulated
   versus authored war behavior before deeper autonomy work.
4. **D14:** the Plan 123 §4.1 emitter (sound-ranging producer beyond the
   synthetic-bearing clash route already sealed).
5. A no-action integration test: with no subscribers' side effects, a full
   chain run must be byte-identical to today (guard against accidental
   behavior change while wiring).

### C.6.3 Ownership & seams

| Concern | Owner |
|---|---|
| War chain production | `FactionWarChainRunner` (unchanged producer) |
| Clock mapping | new Core `WorldClockHorizon` (pure function) |
| Radio intercepts | `RadioHostSession.InterceptWarlordWarning` (existing seam) |
| Journal | `JournalSystem.TryAddRawEntry` (existing seam) |
| Sound ranging | `SoundRangingHostSession.RecordHostileFire` (existing seam) |
| Standing | faction standing engine (existing) |

### C.6.4 State/save/determinism

Clock mapping must be a pure function of (campaign day, authored day, policy)
— no state. Projection consumers are exactly-once where they mutate (journal
dedupe keys, fired-set ledgers) and must persist those ledgers in the war/
year-of-ash save section additively. No new RNG; stage timing follows the
authored chain deterministically under the signed mapping.

### C.6.5 Host & UI wiring

`WireFactionWarConsequenceRouting()` in `src/Main.YearOfAsh.cs` gains the
three subscriptions (same file, same pattern). Daily briefing surfaces stage
lines through the existing crisis-warning builder. No new panels.

### C.6.6 Failure modes

Chain resolves before campaign end under compression (floor guarantees the
final stage is reachable — test it at 180/270/360 lengths); double journal
entries on save/load replay (fired-set persisted); consumer exceptions killing
the tick (subscriptions are host-side; failure behavior = log + continue, never
halt the day); D5 unsigned (package stops — no improvised mapping).

### C.6.7 Phases

- **P0 — Signature cluster:** D5 policy + D6/D7/D14 lines recorded. Gate:
  written decisions.
- **P1 — `WorldClockHorizon` (Core):** pure mapping + tests at all campaign
  lengths; floor invariant; monotonic stage order preserved.
- **P2 — Consumers:** three subscriptions + exactly-once ledgers + save
  additive fields; the no-action parity test. Gate: focused tests + reload
  replay.
- **P3 — D14 emitter:** Plan 123 §4.1 sound-ranging producer per its signed
  scope.
- **P4 — Census unseal evidence:** the C2[10] unseal requires clock decision +
  consumer wiring + no-action test — assembled here.

### C.6.8 Focused verify

War/year-of-ash owning test directory via `scripts/run_test.sh`;
`WarChainReloadReplayTests`-class evidence (continuous == mid-reload); the
no-action parity test; `--data-integrity-selftest` if catalogs change.

### C.6.9 File impact map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/WorldConsequences/` or war-chain area | CREATE/MODIFY | clock + consumer policy | medium |
| `src/Main.YearOfAsh.cs` | MODIFY | 3 subscriptions | low (sealed pattern) |
| year-of-ash save section | MODIFY (additive) | fired-set | low |
| briefing builder | MODIFY | stage crisis lines | low |

**Out of scope:** rewriting the war chain; XP-03's economy/rumor legs (those
are the follow-on vehicle, sequenced after this plan in Part E); 30C autonomy
beyond the signed D7 boundary.

**Handoff — MUST PRESERVE:** producer API; sealed clash/decree routing.
**MUST ADD:** signed clock mapping + three consumers + parity guard. **MUST
NOT DO:** retime authored content in place (mapping only). **VERIFY WITH:**
C.6.8. **FIRST SAFE STEP:** P1 `WorldClockHorizon` (pure Core, zero host
impact).

---

## Plan 07 — Graph-Native Travel & Knowledge Gating (`CF-P32-GRAPH-TRAVEL`)

**Anchor:** Plan 32B/32C / C2[11]; `B2_PLAN32_IMPLEMENTATION_LOG.md`;
reconciled with XP-02 (single plan — no duplicate planner).
**Status:** `loc_*` identity, 68 route distances, save persistence (68/68),
and 32A orphan hygiene all sealed. Expeditions/caravans still never query
`WastelandMapSystem`; 32C geographic-knowledge dispatch gating unbuilt.
**Unblocked by:** 32A seal (graph is complete and gated) + the sealed
amputation speed multiplier (per-survivor travel input precedent).
**Gate:** D8 (largest blast radius in the packet); D10 gates 32C.

### C.7.1 Current reality (verified)

- `WastelandMapSystem` owns the graph; ten formerly-orphan nodes now have
  authored `loc_*` records and a loader gate.
- Expedition dispatch has an advisory preflight; caravans dispatch through
  their own owner; aviation/naval routes exist as separate dispatch families.
- XP-02's spec (edge vocabulary `clear/degraded/flooded/irradiated/ash-choked/
  blocked`, `passage_class`, `maintenance_day`; stateless integer-Dijkstra
  `GraphTravelPlanner`; survey uncertainty bands ±35%/±12%/exact; dynamic edge
  closure from sump floods/storms/war fronts; `EdgeConditionChanged` event)
  is the designed vehicle and must remain the **only** planner.

### C.7.2 Required delta

1. **D8 signature** on graph-native travel binding (expedition + caravan first).
2. Edge-condition vocabulary + planner (stateless Core) + the parity guard:
   *all-clear + fully-surveyed route ⇒ estimate equals the legacy formula*
   (byte-compat pin).
3. Survey-knowledge store per location/edge (persistence; uncertainty bands
   feed estimates).
4. Dynamic closure producers (sump flood, storm, war front — the last reuses
   Plan 06's chain) emitting `EdgeConditionChanged`.
5. 32C (after D10): dispatch gating on geographic knowledge (unknown
   destination requires survey or costs uncertainty).
6. Aviation/naval deferred as recorded debt `DEBT-XP-32C-AVIATION-NAVAL`
   (explicit, not silent).

### C.7.3 Ownership & seams

| Concern | Owner |
|---|---|
| Graph + edge state | `WastelandMapSystem` (extended additively) |
| Path planning | new `GraphTravelPlanner` (stateless Core, integer costs) |
| Survey knowledge | new `SurveyKnowledgeStore` (persisted) |
| Dispatch | existing expedition/caravan owners (consume planner) |
| Save | existing map save section + additive survey fields |

### C.7.4 State/save/determinism

Edge conditions are derived state recomputed from owners (weather, sumps, war)
— never a second authority. Survey knowledge persists (additive section or
fields; old saves = unsurveyed). Planner is pure: same graph + same requests ⇒
same route. Uncertainty bands use the campaign RNG stream keyed by day+route
with persisted anti-reroll rolls (precedent: broadcast-quality persisted
rolls). Composition rule for multipliers (edge condition × survivor speed ×
vehicle wear) must be a single documented Core function so save/multiplier
composition cannot drift per consumer.

### C.7.5 Host & UI wiring

A `GraphTravelHostSession` binds producers → edge state → planner and feeds
expedition/caravan dispatch advisories. Route panels present estimates with
their uncertainty band as text (never a fabricated exact number for unsurveyed
edges).

### C.7.6 Failure modes

Disconnected graph under closures (planner must fail-closed with a typed
"no_route" advisory, never a zero-cost path); stale survey bands after closure
reopening (band recency rule); old saves (unsurveyed ⇒ legacy formula via the
parity guard); aviation/naval accidentally routed through the land planner
(type-keyed dispatch refusal).

### C.7.7 Phases

- **P0 — Signature + reconciliation:** D8 signed; XP-02 doc annotated "executed
  under `CF-P32-GRAPH-TRAVEL`". Gate: one planner, one plan.
- **P1 — Vocabulary + planner (Core):** edge model + Dijkstra + parity-guard
  tests against the legacy formula.
- **P2 — Survey store:** persistence + uncertainty bands + anti-reroll.
- **P3 — Producers:** sump/storm/war-front closure events (war reuses Plan 06).
- **P4 — Dispatch binding:** expedition + caravan consume the planner;
  estimates + advisories flow to panels.
- **P5 — 32C (D10):** knowledge-gated dispatch.
- **P6 — Debt record:** `DEBT-XP-32C-AVIATION-NAVAL` written with a measurable
  recheck condition.

### C.7.8 Focused verify

New planner/survey test files alone first; map-owning test directory;
reload-replay (continuous == mid-reload for a 30-day route-using campaign);
`--data-integrity-selftest` for catalog additions.

### C.7.9 File impact map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/Expeditions/` planner + topology | CREATE | planning authority | high (blast radius) |
| `WastelandMapSystem` | MODIFY (additive) | edge conditions | medium |
| `SurveyKnowledgeStore` + save | CREATE | knowledge persistence | medium |
| expedition/caravan owners + panels | MODIFY | consume planner | medium |
| aviation/naval | READ ONLY | deferred, typed refusal | — |

**Out of scope:** re-authoring `locations.json` travel hours; vehicle legs of
fuel; XP-08 route risk (follow-on, Part E).

**Handoff — MUST PRESERVE:** legacy formula as the all-clear parity oracle.
**MUST ADD:** one planner + survey state + closure events. **MUST NOT DO:**
per-consumer route math or a second map authority. **VERIFY WITH:** C.7.8.
**FIRST SAFE STEP:** P1 planner behind an unbound provider (zero dispatch
change until P4).

---

## Plan 08 — Difficulty Chronicle & Completion-History Binding (`CF-P34-DIFFICULTY-CHRONICLE`)

**Anchor:** Plan 34B/34C / C2[12]; `B3_PLAN34_IMPLEMENTATION_LOG.md`; DEC-20.
**Status:** 34A sealed (append-only, checksum-validated user-level history at
`user://completion_history.json`; `CampaignCompletionHistory.cs`,
`src/Host/CompletionHistoryStore.cs`; tests 9/9; `--endings-selftest` green).
34B is DECISION-BLOCKED only on the missing difficulty authority — which
Plan 13 creates. 34C additionally needs the Wave-11 claim transfer and an
append-only-compatible shape ("runs_started" is not projectable from the
current store).
**Unblocked by:** Plan 13 landing the director (hard dependency).

### C.8.1 Required delta

1. **34B:** chronicle projection as a read model over (a) the completion
   history and (b) the campaign's difficulty preset id (from Plan 13's
   persisted header). No new facts are invented; the projection joins two
   existing authorities.
2. **34C (shape decision):** if difficulty-tagged run counts including
   *started* runs are required, the store gains an append-only
   `runs_started` record written once at campaign creation — never a
   rewrite of completed-run rows. Alternatively the foreman scopes the
   chronicle to completed runs only (zero store change).
3. **Claim transfer:** Wave 11's `claim-wave11-part2-execution-2026-09-18`
   must transfer `CampaignCompletionHistory.cs`,
   `CompletionHistoryStore.cs`, `src/Main.Endgame.cs` before this package
   edits them.

### C.8.2 Ownership & seams

| Concern | Owner |
|---|---|
| Completion facts | `CampaignCompletionHistory` (append-only, unchanged write path) |
| Difficulty facts | Plan 13's persisted `difficulty_preset_id` |
| Projection | new read-only `CompletionChronicleProjection` (Core) |
| Presentation | campaign panel LEDGER strip (Plan 13 builds it; this fills chronicle rows) |

### C.8.3 State/save/determinism

The store stays append-only and checksummed. A `runs_started` record (if
signed) is one row at campaign creation with the preset id — idempotent,
replay-safe. The projection is pure: same inputs ⇒ same rows; it never owns
RNG, endings, or UI state (mirrors the director's invariant).

### C.8.4 Failure modes

Old histories without difficulty ids (rows project as "untagged" — never
guessed); corrupted checksum (existing store behavior: reject with explicit
error, projection surfaces "history unavailable" rather than crashing); store
read during campaign creation (lazy load); claim not transferred (package is
blocked — rule 10, no shadow edits).

### C.8.5 Phases

- **P0 — Claim transfer + shape decision.** Gate: foreman lines.
- **P1 — Projection (Core):** join + formatting; unit tests over fixture
  histories (empty, untagged, tagged, mixed, corrupt).
- **P2 — (conditional) `runs_started`:** append-only record + idempotency
  tests.
- **P3 — Panel rows:** chronicle lines in the LEDGER strip; a11y-safe text.
- **P4 — Census:** C2[12] unseal evidence assembled (34B/34C closed).

### C.8.6 Focused verify

`bash scripts/run_test.sh Ashfall.Core.Tests/Endgame/` (84/84 baseline +
new); `--endings-selftest`; panel lifecycle + a11y selftests after P3.

### C.8.7 File impact map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/Endgame/` projection | CREATE | read model | low |
| `CompletionHistoryStore` (only if `runs_started` signed) | MODIFY (append-only) | started-run facts | medium |
| campaign panel | MODIFY | chronicle rows | low |

**Out of scope:** New Game+ / profile rewards (Plan 175 boundary, retained);
any write path into completed rows.

**Handoff — MUST PRESERVE:** append-only + checksum semantics. **MUST ADD:**
read-model join (+ optional single append record). **MUST NOT DO:** mutate
history or invent difficulty tags for old rows. **VERIFY WITH:** C.8.6.
**FIRST SAFE STEP:** P1 projection over fixtures (no store change at all).

---

## Plan 09 — Port-Contract Long-Tail Sweep (`CF-P36C-PORT-SEAM-SWEEP`)

**Anchor:** Plan 36C / C2[13]; `B4_PLAN36_IMPLEMENTATION_LOG.md`.
**Status:** 36A/36B sealed — engine-free `PortContract` vocabulary, 248-seam
policy (176 `HOST_REQUIRED` with 100% active callers), CI generator in
`CI_GATE_MANIFEST.json` with `--check`, runtime `HostSessionContracts` +
`IWiringReporter`, `--port-contract-selftest` green, `PortContractGateTests`
8/8. Remainder: **17 DEFERRED seams** whose promotion condition is already
stated.
**Unblocked by:** the promotion condition being met per seam; infrastructure
is fully green.

### C.9.1 Required delta

For each of the 17 DEFERRED seams, in bounded tranches: either promote to
`HOST_REQUIRED` with an active caller (host wiring) or retire the seam with a
written reason (dead API). The policy file shrinks monotonically; no new
DEFERRED rows may be added by this sweep.

### C.9.2 Method

1. Enumerate the 17 seams from `docs/ci/port_contract_policy.json`.
2. Classify: (a) owner exists, caller missing → wire the caller (host session
   bind, the proven pattern); (b) API dead → retire with evidence (zero
   consumers, no planned consumer among the fifteen / follow-ons).
3. Per-tranche verification: regenerate the policy, run
   `--port-contract-selftest`, `PortContractGateTests`, and the owning
   subsystem's focused tests.

### C.9.3 Failure modes

Promoting a seam whose Core side changed since audit (premise-check each seam
against current source before wiring); retiring a seam a follow-on plan needs
(check against Part D/E consumers first); generator drift (always run the
owning generator + `--check`, never hand-edit generated output).

### C.9.4 Phases

- **P0 — Classification table** (17 rows: seam / owner / caller status /
  verdict). Gate: table reviewed.
- **P1..Pn — Tranches of 3-5 seams**, each: wire or retire, regenerate,
  verify. Gate per tranche: selftest + gate tests green; DEFERRED count
  strictly decreases.
- **P-final — Census:** C2[13] fully-sealed evidence; `KNOWN_DEBT.md`
  `DEBT-PLAN36-PORT-CONTRACT-CLOSURE` retired.

### C.9.5 Focused verify

`godot --headless --path . -- --port-contract-selftest`;
`bash scripts/run_test.sh Ashfall.Core.Tests/<PortContractGateTests path>`
(8/8); per-tranche owning-subsystem tests; CI generator `--check`.

### C.9.6 File impact map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `docs/ci/port_contract_policy.json` (generated) | REGENERATE | shrink-only | low |
| host sessions for promoted seams | MODIFY | active callers | low-medium each |
| retired seam APIs | DELETE (with evidence) | dead code | medium (verify zero consumers) |

**Out of scope:** new port vocabulary; policy for seams not in the 17.

**Handoff — MUST PRESERVE:** generator as the only policy editor. **MUST
ADD:** callers or retirement evidence. **MUST NOT DO:** hand-edit the policy
file. **VERIFY WITH:** C.9.5. **FIRST SAFE STEP:** P0 table (read-only).

---

## Plan 10 — Plan 28 One-Bootstrap-Path Closure (`CF-P28-ONE-BOOTSTRAP-PATH`)

**Anchor:** Plan 28 / C2[9]; the 2026-09-18 setup-action seal.
**Status:** Core complete; host registers 18 setup delegates; invoked from
`RestoreAllSubsystemsFromDisk()` only — **the fresh-boot path does not go
through the manifest bootstrap** (repo-wide grep confirms a single call site).
The census still lists C2[9] as PARTIALLY-SEALED (stale row).
**Unblocked by:** the seal itself; the remaining gap is one host call site +
row reconciliation.

### C.10.1 Required delta

1. Decide and implement the single bootstrap truth: either the fresh-boot path
   also runs `ExecuteSubsystemManifestBootstrap()` (recommended — one code
   path for subsystem setup ordering), or the split is documented as intended
   with a test pinning each path's executed set.
2. Idempotency proof across both paths (the registration guard exists;
   execution must be safe under boot-then-restore sequences).
3. Census C2[9] row reconciled to sealed with evidence.

### C.10.2 Failure modes

Double execution on boot→restore (idempotency guard test); a setup delegate
that must NOT run on fresh boot (per-descriptor phase filtering exists —
`ExecuteSubsystemManifestBootstrap(LifecyclePhase?)`); ordering drift between
paths (pin both orders in one test).

### C.10.3 Phases

- **P0 — Path audit:** enumerate fresh-boot vs restore setup order today.
  Gate: written audit.
- **P1 — Unify or pin:** implement the chosen truth + idempotency/ordering
  tests. Gate: `MainTriadDriftGateTests` (7/7 baseline), panels lifecycle,
  `7day_smoke_selftest` (10/10 baseline).
- **P2 — Census reconcile:** C2[9] → SEALED with evidence links.

### C.10.4 Focused verify

`bash scripts/run_test.sh Ashfall.Core.Tests/Orchestration/SubsystemManifestTests.cs`;
triad + smoke selftests per P1 gate; docs index `--check` after P2.

### C.10.5 File impact map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `src/Main.Lifecycle.cs` / `Main.SaveOrchestrator.cs` / boot path | MODIFY | one bootstrap path | medium (lifecycle) |
| orchestration tests | MODIFY/CREATE | idempotency + order pins | low |
| census + `KNOWN_DEBT.md` | MODIFY | row truth | low (integrator) |

**Out of scope:** new descriptors; setup-logic migrations beyond the 18.

**Handoff — MUST PRESERVE:** existing registration idempotency. **MUST ADD:**
one verified path + pins. **MUST NOT DO:** reorder subsystem semantics while
unifying. **VERIFY WITH:** C.10.4. **FIRST SAFE STEP:** P0 audit (read-only).

---

## Plan 11 — CatalogPath Tranche-2 Forbidden-Path Sweep (`CF-P26A-FORBIDDEN-PATH-SWEEP`)

**Anchor:** Plan 26A / C2[8]; Wave 10 Part 1 tranche 1.
**Status:** `CatalogPath` (`ResolveCatalog`/`ResolveSub`/validation/
resolution-source) landed; `EventsHostSession`, `Main.FactionBranch`,
`SilentFoundryHostSession` migrated; a **green shrinking forbidden-path gate**
exists with a **~40-file tranche-2 remainder** of direct `res://…/Data`
readers.
**Unblocked by:** the gate itself — every migration strictly shrinks it.

### C.11.1 Required delta

Migrate the remaining direct-path readers to `CatalogPath` in reviewable
tranches until the gate can be tightened to zero (or to an explicitly
allowlisted tools set, per the tranche-1 precedent for build tooling that
legitimately constructs `Assets/StreamingAssets/Data` paths).

### C.11.2 Method

Per tranche (5-8 files): list readers from the gate output; classify host
session vs tool vs test; migrate host sessions to `CatalogPath`; tools either
migrate or join a documented allowlist; tests use the existing fixture path
helpers. Run the gate; the count must strictly decrease.

### C.11.3 Failure modes

Path resolution differences between environments (that is the point of
`CatalogPath` — resolution-source tests exist); allowlist abuse (each
allowlist row needs a written reason); regeneration conflicts (gate output is
generated — run the owner, never hand-edit).

### C.11.4 Phases

- **P0 — Gate snapshot:** current forbidden-path list exported as the
  baseline. Gate: list archived.
- **P1..Pn — Tranches:** migrate + verify; gate count strictly decreasing.
- **P-final — Tighten:** reduce the gate's tolerated set to the documented
  minimum; record completion in the Plan 26A log.

### C.11.5 Focused verify

The forbidden-path gate (CI gate in `CI_GATE_MANIFEST.json`); per-tranche
owning-subsystem focused tests; `dotnet build Ashfall.csproj` 0 errors.

### C.11.6 File impact map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| ~40 host/tool files | MODIFY | path migration | low each |
| gate config (generated) | REGENERATE | shrink-only | low |

**Out of scope:** new path APIs; data moves.

**Handoff — MUST PRESERVE:** `CatalogPath` API. **MUST ADD:** migrations
only. **MUST NOT DO:** broaden the gate. **VERIFY WITH:** C.11.5. **FIRST
SAFE STEP:** P0 snapshot (read-only).

---

## Plan 12 — Test-Quarantine Drain Tranche (`CF-QD-QUARANTINE-DRAIN-TRANCHE`)

**Anchor:** `DEBT-TEST-QUARANTINE-2026-09-12`; decision D21; manifest
`Twin_ASHFall/quarantine/manifests/2026-09-12-quarantined-tests.md`.
**Status:** ~50 `Compile Remove` exclusions remain in the test csproj; the
formal runtime `scripts/ci/quarantine.json` is empty; the per-file
reinstatement pattern is proven five times (UtilityAI, ArchiveInks,
HoldfastNpc, AutopsyProcedures, +1 earlier).
**Unblocked by:** D21 foreman authorization per file, with current API/content
evidence — the standing rule (never silently re-enable) is the gate, not a
blocker.

### C.12.1 Required delta

A bounded tranche (5-8 files) of quarantined test files reinstated: each with
(a) current-API evidence the file's contract still exists, (b) a repair of
stale assertions against current truth (the dominant historical cause), (c) a
passing focused run, (d) the `Compile Remove` dropped for that file only.

### C.12.2 Method

Prioritize files whose subject systems are touched by this program (medical,
orchestration, map/travel, economy, radio) — reinstating their quarantined
contracts pays double by guarding the new work. Per file: read the manifest's
recorded quarantine reason; re-verify against source; repair or, if the
contract is genuinely retired, record RETIRED-permanently in the manifest
instead of reinstating.

### C.12.3 Failure modes

Re-enabling a test whose authority moved (must retarget to the current owner,
not weaken asserts); batch re-enables (forbidden — per file only); flaky
reinstatements (a reinstated file must pass twice consecutively before the
`Compile Remove` drops).

### C.12.4 Phases

- **P0 — Selection table** (file / quarantine reason / current verdict /
  action). Gate: foreman authorization for the tranche list.
- **P1..Pn — Per-file drain:** repair → two consecutive green focused runs →
  drop exclusion → manifest updated with restoration condition met.
- **P-final — Ledger:** `DEBT-TEST-QUARANTINE-2026-09-12` updated with the
  new count and the next tranche's eligibility.

### C.12.5 Focused verify

`bash scripts/run_test.sh <each reinstated file>` (twice); csproj diff shows
only the intended `Compile Remove` drops; manifest check.

### C.12.6 File impact map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `Ashfall.Core.Tests` csproj | MODIFY | drop exclusions per file | low |
| reinstated test files | MODIFY | retarget stale contracts | medium each |
| quarantine manifest | MODIFY | record restorations | low |

**Out of scope:** runtime quarantines (manifest is empty); full-suite runs.

**Handoff — MUST PRESERVE:** per-file rule; two-green-run rule. **MUST ADD:**
evidence-backed restorations. **MUST NOT DO:** weaken assertions to pass.
**VERIFY WITH:** C.12.5. **FIRST SAFE STEP:** P0 table (read-only).

---

## Plan 13 — XP-01 Difficulty Full Binding (`CF-XP01-DIFFICULTY-FULL-BINDING`)

**Anchor:** XP-01 / active batch `XP-WAVE1-DIFFICULTY-AUTHORITY`;
`docs/plans/xp/w1/W1_PREMISE_EVIDENCE.md`; decision packet D1 (signed,
narrowed).
**Status:** Core complete and verified — catalog (3 presets, 8 scalars,
strict reference key), director, scalar provider, validator integration,
tests 8/8. Zero `src/` consumers by design. This plan is the **completion of
the active batch**: campaign binding, persistence, premise-checked consumers,
host session, panel strip.

### C.13.1 Current reality (verified)

- `DifficultyDirector` resolves an immutable preset id into typed scalars
  (hunger/thirst/radiation/disease/hostile/market-price/decay/crisis-deadline
  multipliers, validated 0.25–2.5).
- The campaign header currently contains no difficulty field; the packet
  names campaign binding, persistence, scalar consumers, and the chronicle
  projection as separate follow-on slices.
- The director's invariant is recorded: read-only scalar provider; owns no
  endings, completion history, UI state, or RNG.

### C.13.2 Required delta

1. **Campaign binding:** preset selected once at campaign creation; the id is
  written into the checksummed campaign header (additive field
  `difficulty_preset_id`); immutable thereafter (any mutation attempt is a
   typed failure).
2. **Persistence:** save envelope V+1 with a frozen prior shape and migration
   (old saves resolve to `difficulty_standard`; never throw on legacy).
3. **Consumers — one at a time, premise-checked:** for each of the proposed
   scalar seams, verify the live calculation site, then bind the provider as
   a pure multiplier input. Candidate seams (each gated on its premise check
   passing): needs decay rates, radiation accumulation, disease exposure
   modifier, hostile encounter pressure, market price composition (through
   the existing `ExplainPrice` factor rows — presentation must show the
   difficulty factor, not hide it), item decay, crisis deadline scaling.
4. **Host + panel:** `DifficultyHostSession` (catalog load, director
   construction, restore); campaign panel gains a CAMPAIGN LEDGER strip line
   showing the preset name (truthful state only; no mid-run selection UI by
   design).
5. **Selftest:** `--difficulty-selftest` covering catalog load, restore
   migration, and one bound consumer end-to-end.

### C.13.3 Ownership & seams

| Concern | Owner |
|---|---|
| Preset data | `difficulty_presets.json` (sole authority) |
| Scalar resolution | `DifficultyDirector` (existing) |
| Campaign fact | campaign header (additive field) |
| Each calculation | its existing owner, now reading the provider |
| Presentation | campaign panel strip |

### C.13.4 State/save/determinism

One new persisted fact (`difficulty_preset_id`), immutable, checksummed with
the header. Consumers multiply deterministically; no RNG involvement. Legacy
migration: absent field ⇒ standard preset ⇒ legacy byte-identical behavior
**until** each consumer binds — the parity guard per consumer is "bound
scalar at 1.0 ⇒ byte-identical to unbound" (standard preset's scalars are
1.0-authored; verify and pin).

### C.13.5 Failure modes

Unknown preset id in a save (fail-closed to an explicit error at load with a
typed recovery path — the catalog tests already pin fail-closed loading);
mid-campaign difficulty change attempts (typed refusal); consumers silently
double-applying (each site premise-checked exactly once; a parity test per
consumer); catalog drift between host and tests (loader is shared).

### C.13.6 Phases

- **P0 — Commit the W1 Core:** land the untracked difficulty files + evidence
  as the batch's first reviewable unit. Gate: tests 8/8 + integrity green on
  a clean tree.
- **P1 — Header + migration:** additive field, frozen prior shape, V+1,
  migration tests (absent ⇒ standard; corrupt ⇒ typed error).
- **P2 — Host session + restore:** `DifficultyHostSession`; binding at
  campaign creation; `--difficulty-selftest` skeleton.
- **P3 — Consumers in premise-checked pairs:** for each seam: evidence note →
  bind → parity test at scalar 1.0 → focused suite. No more than two seams
  per review unit.
- **P4 — Panel strip:** preset line in CAMPAIGN LEDGER; a11y checks.
- **P5 — Batch closeout:** W1 evidence/acceptance docs completed; `KNOWN_DEBT
  .md` difficulty-chronicle row re-pointed at Plan 08.

### C.13.7 Focused verify

`bash scripts/run_test.sh Ashfall.Core.Tests/Difficulty/`; per-consumer owning
suites; `--difficulty-selftest`; `--data-integrity-selftest`; save round-trip
+ migration tests; `--endings-selftest` untouched (director owns no endings).

### C.13.8 File impact map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| campaign header/save envelope | MODIFY (additive V+1) | persist preset id | medium |
| `src/Host/DifficultyHostSession.cs` | CREATE | host owner | low |
| 7 consumer sites (each premise-checked) | MODIFY | scalar input | medium each |
| campaign panel | MODIFY | LEDGER strip | low |
| consumer owning test files | MODIFY | parity pins | low |

**Out of scope:** chronicle projection (Plan 08); XP-03 shock magnitude
(follow-on); any travel binding (deliberately excluded by the packet).

**Handoff — MUST PRESERVE:** director invariants; JSON authority. **MUST
ADD:** one persisted fact + premise-checked multipliers. **MUST NOT DO:**
runtime difficulty switching; hidden difficulty effects (panel + price
explanation must surface them). **VERIFY WITH:** C.13.7. **FIRST SAFE STEP:**
P0 commit of the already-green Core.

---

## Plan 14 — XP-04 Economy Legs: Funds, Black-Market Legs, Heat, Counterfeit (`CF-XP04-ECONOMY-LEGS`)

**Anchor:** XP-04 (W2 vehicle); sealed black-market settlement surface;
DEC-05 restock design (Plan 03 reconciles the ledger truth this builds on).
**Status:** black-market trade actions exist with immediate canonical-
inventory settlement, but the **funds/goods legs are undefined** — no scrip
authority exists; heat/attention and counterfeit purity are unsigned designs;
restock priority already has a signed deterministic design that this plan
must **reference, not re-specify**.

### C.14.1 Required delta

1. **`FundsLedger` (Core):** integer-scrip authority with a bounded 128-entry
   movement log (MedicalRecordLog rules: append-only, typed reasons,
   oldest-evicted with a running balance invariant). One owner; panels and
   hosts never track balance separately.
2. **Four black-market legs as transactional two-leg operations:**
   `bm_buy` (goods in / scrip out), `bm_sell` (scrip in / goods out),
   `bm_fence` (faction-recognized loot in / scrip in at a purity discount —
   the only fence path), `bm_contract` (escrowed deliverable ↔ scrip). Each
   leg is atomic: either both halves commit or neither does.
3. **Heat/attention:** per-faction-market integer heat, raised by legs,
   decayed daily; at heat ≥ 10 the market relocates (deterministic new
   contact, discovered anew). Persistence in the existing black-market save
   section (additive).
4. **Counterfeit purity tiers:** `sealed/clean/suspect/cut` item states for
   scrip-adjacent goods; `bm_fence` prices by purity; detection at
   use/inspection sites is a follow-on consumer (named, not built here).
5. **Restock priority:** bind the DEC-05 design verbatim (Plan 03's evidence
   note is the input); no second ordering algorithm.
6. **Port + UI:** `IFundsSurface.Debit/Credit` port contract; TradePanel
   funds readout + NEXT SUPPLY strip (day-gated restock visibility).

### C.14.2 Ownership & seams

| Concern | Owner |
|---|---|
| Scrip balance + movement log | new `Economy/FundsLedger` (Core) |
| Leg execution | `BlackMarketSystem` extension (two-leg transactions) |
| Heat + relocation | black-market owner (additive state) |
| Purity | item-state extension through the inventory authority (additive field, never a parallel item id family) |
| Restock ordering | DEC-05 design (existing Core scoring) |
| Prices | canonical `MarketSystem` (unchanged composition) |

### C.14.3 State/save/determinism

Two save touches: `funds_ledger` section (new) + additive fields in the
existing `black_market` section (heat, relocations, purity map) — envelope
V+2 with frozen prior shapes and neutral migrations. Relocation target
selection uses the existing black-market stock RNG stream pattern
(fork-per-day, snake_case-gated). Purity assignment at mint time is
deterministic from authored ratios + campaign stream with persisted
anti-reroll.

### C.14.4 Failure modes

Negative balance (typed refusal; the ledger debits preflight); partial leg
failure mid-transaction (two-leg atomicity test with injected failure);
purity field on old saves (default `clean`, never fabricated `sealed`);
relocation while a contract is escrowed (contract survives relocation —
pinned); scrip inflation (bounded log + authored prices only; no UI-invented
values).

### C.14.5 Phases

- **P0 — Design sign-off:** the D3/D4-class decisions (leg grammar, heat
  thresholds, purity ratios, relocation policy) signed. Gate: foreman lines.
- **P1 — `FundsLedger` Core + tests** (round-trip, eviction invariant,
  typed refusals).
- **P2 — Legs on `BlackMarketSystem`** + atomicity tests + economy regression.
- **P3 — Heat + relocation** + persistence + reload replay.
- **P4 — Purity** + fence pricing + old-save migration.
- **P5 — Port + panels + restock bind** + `EconomyProbeTests` extension +
  `--black-market`/economy selftests.

### C.14.6 Focused verify

`bash scripts/run_test.sh Ashfall.Core.Tests/Economy/` (118+ baseline);
black-market snapshot tests; save round-trip + migration; panel lifecycle +
a11y; `--content-utilization-selftest` if catalogs grow.

### C.14.7 File impact map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/Economy/FundsLedger.cs` + DTOs | CREATE | scrip authority | medium |
| `BlackMarketSystem` | MODIFY | legs + heat | medium |
| inventory item-state (purity) | MODIFY (additive) | purity tiers | medium (schema) |
| `black_market_actions.json` / catalogs | CREATE/MODIFY | authored legs | low |
| TradePanel + port surface | MODIFY | funds + next supply | low |
| save sections | MODIFY (V+2) | persistence | medium |

**Out of scope:** XP-08 tariffs/route contracts (follow-on); counterfeit
detection consumers; tax/politics.

**Handoff — MUST PRESERVE:** canonical price composition; DEC-05 ordering.
**MUST ADD:** one funds authority + four atomic legs. **MUST NOT DO:** panel
-side balance tracking or a second pricing path. **VERIFY WITH:** C.14.6.
**FIRST SAFE STEP:** P1 `FundsLedger` (pure Core, unbound).

---

## Plan 15 — XP-06 Body Integrity: Limb Requirements & Prosthetics (`CF-XP06-BODY-INTEGRITY`)

**Anchor:** XP-06 (W5 vehicle); the equipment half of
`DEBT-AMPUTATION-EQUIPMENT-RESTRICTION` (DEC-03's split schema package).
**Status:** `AmputationSystem` is the sealed limb authority (avatar +
expedition speed multiplier live; `C2AmputationTravelTests` 7/7);
`EquipmentConditionSystem`, `TickSharedSkillProgression`, and the crafting
chains (foundry/ceramics/cordage/apiculture) are live owners. The equipment
half is blocked **only on schema**: `ItemDefinition`/`EquipSlot` has no
handedness/limb-requirement field. XP-06 is the designed vehicle:
additive `limb_requirements` + `strength_requirement`, a survivor `body_state`
limb map, equip-time gating with amputation auto-unequip, a prosthetics
catalog with a fitting→adaptation→mastery rehabilitation arc, and phantom-pain
events extending the sealed sleep-narrative classification.

### C.15.1 Required delta

1. **Schema package (the DEC-03 successor signature):** additive
   `limb_requirements` (e.g. two-handed items require two effective hands)
   and `strength_requirement` on `ItemDefinition`; old items default to no
   requirement (byte-identical equip behavior — pinned).
2. **`body_state` limb map:** per-survivor effective-limb state derived from
   `AmputationSystem` (the sole surgical authority — body state **reads**
   amputations, never records its own). Persisted additively in the survivor
   save family.
3. **Equip-time gating:** preflight at the existing equip owner; failure is a
   typed "limb_requirements_unmet" with the missing limb named. Amputation
   auto-unequips now-violating items through the existing unequip path
   (exactly-once, journal line).
4. **Prosthetics catalog:** 6 items crafted through the live chains, with
   condition decay via `EquipmentConditionSystem`; fitting is a medical
   procedure through the ward owner (Plan 01's staffing gate applies).
5. **Rehabilitation arc:** fitting → adaptation → mastery as skill-progression
   entries (`TickSharedSkillProgression`); adaptation period affects the
   effective-limb contribution deterministically (no RNG).
6. **Phantom pain:** event co-trigger classification extending the sealed
   sleep-narrative projection (additive tag; no new sleep authority).
7. **Host + UI:** `BodyIntegrityHostSession`; `SurvivorDetailPanel` limb-slot
   display; `--body-integrity-selftest`.

### C.15.2 Ownership & seams

| Concern | Owner |
|---|---|
| Surgical limb facts | `AmputationSystem` (unchanged) |
| Effective-limb projection | new `Survivors/BodyIntegrity` (read model + gates) |
| Item requirements | `ItemDefinition` additive fields + items.json |
| Equip/unequip execution | existing equipment owner |
| Prosthetic crafting | existing crafting owners |
| Condition decay | `EquipmentConditionSystem` |
| Rehab progression | `TickSharedSkillProgression` |
| Phantom-pain tag | sealed sleep-narrative classification |

### C.15.3 State/save/determinism

`body_state` persists additively (old saves derive from existing amputation
records — deterministic replay of the derivation, never fabricated). Prosthetic
condition rides equipment condition persistence. Adaptation timelines are
authored constants × progression state. No new RNG streams; any discomfort/
phantom cadence uses the existing medical/needs day-keyed streams.

### C.15.4 Failure modes

Amputation mid-equip (gate order: surgical event → auto-unequip → next equip
refused); prosthetic destroyed while fitted (falls back to unsatisfied
requirement + medical event); old saves (defaults pinned byte-identical);
ward unstaffed (fitting refused by Plan 01's preflight — correct, surfaced);
two-handed + one prosthetic (effective-hand rules authored, tested at
boundaries).

### C.15.5 Phases

- **P0 — Schema signature:** DEC-03-successor line approving the additive
  fields + effective-limb semantics. Gate: foreman decision.
- **P1 — Schema + gating Core:** fields, loader validation, gate logic,
  legacy parity pins; focused tests.
- **P2 — `body_state` + auto-unequip:** derivation, persistence, exactly-once
  journal; reload replay.
- **P3 — Prosthetics catalog + crafting + condition.**
- **P4 — Rehab arc + phantom-pain tag** (sleep-narrative extension tests).
- **P5 — Host + panel + selftest**; a11y + lifecycle checks.

### C.15.6 Focused verify

New `BodyIntegrity` test file alone first; `Ashfall.Core.Tests/Medical/`
regression; equipment + crafting owning suites; `--body-integrity-selftest`;
`--data-integrity-selftest` for the catalog.

### C.15.7 File impact map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `ItemDefinition` + items.json | MODIFY (additive) | requirements | medium (schema) |
| `Assets/Ashfall.Core/Survivors/BodyIntegrity/` | CREATE | projection + gates | medium |
| equip owner + survivor save | MODIFY | gating + persistence | medium |
| `prosthetics.json` + crafting chains | CREATE/MODIFY | content | low |
| `SurvivorDetailPanel` + host session | CREATE/MODIFY | presentation | low |

**Out of scope:** bionics rework (Plan 177 sealed owner unchanged); XP-10
phobia interplay (follow-on); new combat math.

**Handoff — MUST PRESERVE:** `AmputationSystem` as surgical authority; sealed
sleep classification. **MUST ADD:** additive requirements + read-model gates.
**MUST NOT DO:** body-state surgery records or a second limb ledger.
**VERIFY WITH:** C.15.6. **FIRST SAFE STEP:** P1 schema fields with legacy
parity pins (unbound until the gate owner consumes them).

---

# Part D — Purpose-Enhanced Expansion Plans (EN-01 … EN-08)

These eight proposals are the "build on top" layer: each becomes buildable
**only after** its named roster plans complete, extends the seams they finish,
and is framed to the same contract (premise first, one owner, bounded,
testable). They are proposals for foreman authorization — nothing here is
authorized for implementation by this document alone.

## EN-01 — Difficulty-Consequence Weave

**Builds on:** Plan 13 (scalars), Plan 06 (war consumers + clock), Plan 08
(chronicle).
**Premise:** difficulty currently multiplies survival pressure but not world
drama. With the war chain routable (Plan 06) and scalars bound (Plan 13),
shock magnitude, crisis deadlines, and rumor severity can read the same
director — one knob, coherent world response.
**Design sketch:** `WorldClockHorizon` stage severity × difficulty scalars
(computed in Core at the event site, never in panels); the crisis predictor's
deadline input reads `crisis_deadline` scalar; chronicle rows carry the
preset tag (Plan 08 projection). A balance-soak harness (seeded, 30/60/90-day)
asserts difficulty monotonicity: dirge never easier than sparing on any
tracked axis.
**Gate:** all three roster plans sealed; one foreman authorization.
**Verify:** soak harness + per-consumer parity at 1.0.

## EN-02 — The Living Map

**Builds on:** Plan 07 (planner, survey bands, closures), Plan 06 (war fronts),
follow-on XP-08 (seasonal migration).
**Premise:** route information currently lives in dispatch advisories; once
edges carry conditions, survey bands, and war closures, one map presentation
can plan routes honestly.
**Design sketch:** a route-planning surface over the `GraphTravelPlanner`
read model: select destination → show candidate route, per-edge condition
class, uncertainty band (text, never fake precision), known closures, and
caravan/expedition cost estimates from the existing owners. War-front edges
flag from Plan 06's chain state. Strictly presentation: the panel issues the
existing dispatch commands and renders Core estimates.
**Gate:** Plan 07 P4+ sealed.
**Verify:** panel route battery + a11y + lifecycle selftests; estimator
parity against dispatch advisories.

## EN-03 — Underground Economy Pressure

**Builds on:** Plan 14 (funds, heat, purity), Plan 03 (restock truth),
follow-on XP-07 (provenance).
**Premise:** heat, purity, and faction recognition become a single pressure
system: fencing recognized loot raises heat; cut goods surface at inspection;
relocation (heat ≥ 10) rediscovers markets.
**Design sketch:** a Core `EconomyPressureRules` pure module joining the
three existing states (funds ledger, black-market heat, purity map) into
derived pressure tiers consumed by encounter/inspection content and the
TradePanel risk line. Provenance (XP-07) later feeds faction recognition of
`looted` goods into the same tier function — one consumer surface, no new
ledger.
**Gate:** Plan 14 sealed + XP-07 premise check (if recognition is in scope).
**Verify:** economy suite + a 20-day pressure soak (continuous == mid-reload).

## EN-04 — Rehabilitation Medicine

**Builds on:** Plan 15 (prosthetics + rehab), Plan 01 (recovery ramp + ward
staffing), follow-on XP-10 (therapeutic protocol).
**Premise:** recovery is currently per-system (affliction ramp, prosthetic
adaptation, future phobia treatment). One medical-pipeline arc presents and
schedules them together through the ward the staffing gate already guards.
**Design sketch:** a read-model "recovery slate" on the medical owner:
active affliction ramps (Plan 01 D2 outcome), prosthetic adaptation
timelines (Plan 15), and — when XP-10 lands — phobia therapeutic protocols
as a treatment kind. The ward panel renders the slate; treatment commands
flow through existing medical procedures. No new medical authority.
**Gate:** Plans 01 + 15 sealed.
**Verify:** medical suite + ward staffing preflight interaction tests.

## EN-05 — Signal Continuity & Voice

**Builds on:** Plan 02 (sealed follow-up/audio content), follow-on XP-09
(presenter skills — premise-check-first).
**Premise:** rescued senders, follow-up chains, and (if XP-09 survives its
premise check) presenter quality can project into one continuity surface:
the rescued-sender arc as journal/chronicle entries with audio cues.
**Design sketch:** a `RescuedArcProjection` read model over the existing
mission + follow-up ledgers: each resolved rescue yields a dated journal arc
(employer/medical/revenge outcomes already authored as follow-up classes).
XP-09's broadcast quality, if authorized, tags arc visibility (a good
broadcaster's rescue stories travel). Presentation-only; no mission-state
changes.
**Gate:** Plan 02 sealed; XP-09 premise decision.
**Verify:** radio replay extension + projection unit tests.

## EN-06 — One Bootstrap Path (Host Integrity)

**Builds on:** Plan 10 (bootstrap unification), Plan 11 (path sweep), Plan 09
(seam sweep).
**Premise:** the three host-hygiene completions compose into a single
verifiable claim: every subsystem sets up through the manifest on every
lifecycle path, every catalog read resolves through `CatalogPath`, every
port seam is live or retired.
**Design sketch:** one gate extension: a lifecycle selftest asserting the
executed setup set is identical across fresh-boot and restore paths;
forbidden-path gate at its documented minimum; port policy DEFERRED count
zero. This is the "host wiring completeness" invariant future waves inherit.
**Gate:** Plans 09–11 sealed.
**Verify:** the three contributing gates + the new lifecycle assertion.

## EN-07 — Chronicle & Aspiration Readiness

**Builds on:** Plan 08 (chronicle), Plan 13 (difficulty), E1/Plan 53
(READY-UNCLAIMED governance programme).
**Premise:** with a truthful difficulty-tagged chronicle, the ambition
governance programme (E1A–E1P) has its read model; campaign aspiration can
be governed by evidence rather than aspiration docs.
**Design sketch:** the E1 programme's rails-readiness slice consumes the
chronicle projection as its single source of run history; the campaign
panel's LEDGER strip becomes the player-facing aspiration surface (runs by
difficulty, completed arcs). Claim E1 through the census before starting.
**Gate:** Plan 08 sealed + E1 claimed per census protocol.
**Verify:** E1's own acceptance gates + Endgame suite.

## EN-08 — Ledger Truth Program

**Builds on:** Plan 03 (restock reconcile), Plan 01 (Plan 24 rows), Plan 12
(quarantine drain), census rerank protocol.
**Premise:** several ledgers currently disagree with source truth (stale
deferred rows, stale PARTIALLY-SEALED census rows, remaining quarantine
exclusions). Wave 12 head certification requires a measured, truthful queue.
**Design sketch:** one bounded pass, after the roster's reconcile items
land: rerun the census refresh (counts, dependency graph, claims check, head
premise-check) per the Wave 11 Part 2 C2 protocol; verify every flipped row
cites its sealing evidence; certify the next wave head. This is the program's
own exit criterion made explicit.
**Gate:** roster reconcile items sealed.
**Verify:** census refresh outputs + signature queue drained or explicitly
re-queued.

---

# Part E — Sequencing & Dependency Order

## E.1 DAG (roster)

```
P01 Plan24 closure ─────────────┐
P02 P1 content seal ──┐         │
P03 P5 restock ───────┼─ (no interdependency; verify-and-seal lane)
P09 P36C seam sweep ──┤         │
P10 P28 bootstrap ────┤         │
P11 P26A path sweep ──┘         │
P12 quarantine drain (per-file; co-scheduled with touched systems)
P13 XP-01 binding ──→ P08 P34 chronicle ──→ EN-01/EN-07
P06 P30 consumers (D5 cluster) ──→ EN-01, feeds war-front edges to P07
P07 P32 graph travel (D8) ──→ EN-02; benefits from P06 producers
P14 XP-04 economy legs ──→ EN-03; consumes P03's reconciled design
P15 XP-06 body integrity (schema sign-off) ──→ EN-04
P04 P6 armor grades ── (independent; owner-adjacent to foundry/garage)
P05 P3 semantic kinds (D11) ── (independent; briefing/audio lanes)
```

Hard edges: **P13 → P08**; **P03 → P14** (design reference); **P06 → P07**
(war-front closure producer before its consumer is exercised at scale);
**P01+P15 → EN-04**; **P09+P10+P11 → EN-06**.

## E.2 Recommended wave order (completion-first)

1. **Wave C1 — Truth & landings (no signatures needed):** P02, P09, P10,
   P11 (+ P01's fixture repair). Everything here is verify-and-seal or
   bounded shrink-only; it makes every later ledger claim trustworthy.
2. **Wave C2 — Signature cluster A:** P01 (D2), P03 (reconcile line), P12
   tranche authorization, P13 P0 commit + P1 header. One foreman sitting
   clears the whole wave.
3. **Wave C3 — Active-batch completion:** P13 consumers through panel +
   selftest; P08 chronicle after it.
4. **Wave C4 — Signature cluster B:** P06 (D5/D6/D7/D14 together, per the
   packet) then P07 (D8, D10).
5. **Wave C5 — Mechanics:** P14 (design sign-off then legs), P15 (schema
   sign-off then body integrity), P04, P05 (D11).
6. **Wave C6 — Enhancements:** EN-01…EN-08 in dependency order (EN-06 early
   is cheap; EN-01 after C4; EN-04/EN-07 last).

## E.3 Follow-on horizon (not roster, sequenced after)

- **XP-07 provenance** (W6 vehicle): consumes Plan 07 survey state + Plan 14
  `bm_fence`; bounded per-instance provenance + named items + reveal hooks.
- **XP-08 trade routes & seasonal migration** (W6): hard-depends on Plan 14
  funds + Plan 07 planner; closes Plans 192/199 intents without NPC agents.
- **XP-09 presenter skills** (W5): **premise-check-first** — the Wave 10 A3
  sweep recorded presenter skills as STALE/RETIRED; re-verify against the
  live `RadioProgramProductionSystem` before any bind.
- **XP-10 phobia growth** (W5): same premise-check-first rule; co-trigger
  design depends on Plan 15's phantom-pain tag.

---

# Part F — Consolidated Foreman Decision Checklist

| # | Decision | Needed by | Recommended shape |
|---|---|---|---|
| F1 | D2 Plan 24 recovery ramp | Plan 01 | option ii (zero-code close) |
| F2 | P5 ledger reconciliation line | Plan 03 | ratify DEC-05 wording across the three docs |
| F3 | D5 war runtime clock policy | Plan 06 | `linear_compress` + 2-day floor |
| F4 | D6 30B scope | Plan 06 | sign with F3 per packet |
| F5 | D7 30C autonomy boundary | Plan 06 | sign with F3 per packet |
| F6 | D14 Plan 123 §4.1 emitter | Plan 06 | sign with F3 per packet |
| F7 | D8 graph travel binding | Plan 07 | expedition+caravan first; aviation/naval debt |
| F8 | D10 32C knowledge gating | Plan 07 P5 | after D8 |
| F9 | 34C chronicle shape + claim transfer | Plan 08 | completed-runs-only first; `runs_started` only if demanded |
| F10 | D11 semantic-kind title rule | Plan 05 | fail-closed title rule (C.5.1) |
| F11 | D21 quarantine tranche list | Plan 12 | the P0 selection table |
| F12 | XP-01 consumer list per seam | Plan 13 | one evidence note per consumer, pairs bound |
| F13 | XP-04 leg/heat/purity design | Plan 14 | the D3/D4-class sign-off |
| F14 | XP-06 schema package | Plan 15 | DEC-03-successor additive fields |
| F15 | P6 claim note (approved deferral reversal) | Plan 04 | new claim + premise note |
| F16 | Snapshot rebaseline window | Plan 01 P3 | renderer-capable session |

Premise-check obligations standing across the program: every consumer bind
(Plan 13's seven seams, Plan 07 dispatch, Plan 14 legs) re-verifies the live
calculation site in source before editing — the W1 XP-05 correction is the
precedent for why.

---

# Part G — Program Verification, Rollback, Out of Scope

## G.1 Program-level verification pattern

Every roster plan closes with: focused suite green (its own files), owning
regional suite green, `dotnet build Ashfall.csproj` 0 errors, and where
runtime paths changed, the matching headless selftest(s). Save-touching
plans add round-trip + migration + continuous-vs-mid-reload equality tests.
Full-suite runs remain exception-gated per TEST_POLICY.md (a dedicated window
with a stated reason), not a default.

## G.2 Rollback strategy

Each roster plan lands as small reversible commits (Core-first, host second,
panel third — the repo's proven pattern). Data changes are additive with
neutral legacy defaults, so revert = code revert; no save is ever migrated
irreversibly (frozen prior shapes everywhere). Plans touching shared seams
(Plan 07 dispatch, Plan 13 consumers, Plan 15 schema) carry a named parity
guard whose removal blocks the revert review.

## G.3 Out of scope for the whole program

- Reversing DEC-06 (availability consumer stays retired).
- Promoting Plans 42/46/49 ahead of their audits.
- Census tranche-2 audits as stealth integration (EN-08 keeps them honest).
- XP-09/XP-10 binds without fresh premise evidence.
- Any new parallel authority — funds, maps, body state, chronicle, bootstrap,
  or otherwise — regardless of convenience.

## G.4 Final implementation handoff

**MUST PRESERVE:** every existing owner named in Part C; signed decisions
DEC-05/DEC-06/DEC-20 and the W1 packet D1/D2; deterministic RNG contracts;
frozen save-shape discipline; the fail-closed loader pattern.
**MUST ADD:** only what each plan's delta names, behind its gate, in phase
order, with the parity guards listed.
**MUST NOT DO:** bind consumers without premise notes; run broad suites by
default; edit generated files by hand; race claimed paths (check
`WORKTREE_OWNERSHIP.md` immediately before each claim).
**VERIFY WITH:** each plan's Focused-verify block; EN-06's composite gate at
program exit; EN-08's census rerank as the closing artifact.
**FIRST SAFE STEP:** Wave C1's P02 P0 census or P09 P0 table — both are
read-only evidence passes a single cheap agent can run today.

---

*End of program document. Register in `docs/INDEX.md` when the integrator
next regenerates the index (this document intentionally does not edit it —
the index has unrelated in-flight edits as of 2026-09-18).*
