# ASHFALL — Enhanced Expansion Program: EN-01 … EN-08 Full Proposals & XP Pillar Re-Scoping

**Date:** 2026-09-19
**Series:** Seal-steps integration programs. Companion to
`Seal-steps/ashfall-eight-unblocked-plans-completion-first-execution-program-2026-09-19.md`
(the signature-free execution program for the 8 unblocked plans) and
successor in genre to Part D of
`ashfall-fifteen-unblocked-partial-integrations-completion-first-full-integration-and-enhanced-expansion-program.md`,
whose eight enhancement sketches (EN-01…EN-08) this document expands into
full, implementation-ready proposals.
**Repo state inspected:** branch `Zcode_Branch`, HEAD `fc73a306`
(2026-09-19 02:27) plus the current uncommitted completion-first session.
**Queue baseline:** `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md`.
**Document role:** planning authority only — these are **proposals for
foreman authorization**. Nothing here edits code, data, ledgers, or claims;
nothing here is authorized for implementation by this document alone. Every
plan re-verifies its premises at claim time per AGENTS.md Rule 7.

---

## 0 · Program summary card

| Field | Value |
|---|---|
| Proposal count | 8 enhanced expansions (EN-01…EN-08) + 5 XP pillar re-scopes |
| Authorization status | **none authorized** — one foreman line per expansion (Part F) |
| Gates already satisfied by the 2026-09-18/19 seals | EN-02, EN-06, EN-07, EN-08 (near-ready); EN-01 awaits the companion program's Plan 05; EN-03/EN-04/EN-05 await decision-gated roster items |
| Companion execution program | the 8 unblocked plans (signature-free) |
| Production changes made by this document | none |

Proposal readiness at a glance (inputs sealed? gate named?):

| Proposal | Inputs sealed today | Missing input | Signature |
|---|---|---|---|
| EN-02 | all | none | S1 only |
| EN-06 | 2 of 3 | companion Plan 04 (bootstrap parity) | S2 |
| EN-01 | 2 of 3 | companion Plan 05 (XP-01 full binding) | S3 |
| EN-07 | 2 of 3 | companion Plan 06 (E1 claim) | S4 |
| EN-08 | in flight | companion Plans 02/06 reconcile | S5 |
| EN-05 | 1 of 2 | companion Plan 01 (P1 seal) | S6 |
| EN-03 | partial | F13 (XP-04 design) | S7 chain |
| EN-04 | partial | F14 (XP-06 schema) | S8 chain |

Reading paths by role: **foreman** — §0, Part B (authorization boundary),
Part F (one line per proposal). **builder** — Part C/D (your one proposal,
after its authorization), Part E (sequencing). **cheap sweep agent** — any
"premise check" block; all read-only. **integrator** — Part E.3 ledger
routing, Part G.5 exit criteria.

---

## How to read this document

- **Part A** establishes current reality: the seam inventory the 2026-09-18/19
  execution wave created or finished, and a gate matrix showing which EN
  prerequisites are now satisfied.
- **Part B** defines the authorization boundary and the selection rules.
- **Part C** expands each of EN-01…EN-08 into a full proposal: premise
  (with current source evidence), gate status, design, ownership and seams,
  state/save/determinism, host wiring, failure modes, phases, tests, file
  impact, rollback, handoff.
- **Part D** re-scopes the XP-02…XP-10 pillars against what actually landed:
  two pillars are premise-consumed (their capability shipped through
  canonical owners), one is verify-and-record, two are sequenced, two are
  premise-check-first against retired decisions.
- **Part E** sequences everything against the companion program.
- **Part F** is the consolidated signature queue.
- **Part G** closes with verification, rollback, out-of-scope, and handoff.
- **Part H** is the evidence index.

**Authority compliance baked into every proposal below** (identical to the
companion program's eight rules): Godot-only host, engine-free Core, JSON
data authority, one owner per concern, seeded determinism, signed/retired
decisions respected (DEC-06, DEC-17, DEC-18, DEC-20, C3 HOLDs), panels never
recompute, focused verification only.

---

# Part A — Current Reality (verified 2026-09-19)

## A.1 The seam inventory the execution wave finished

These are the load-bearing seams the EN layer builds on. Each is verified
live at current HEAD (upstream audit, 2026-09-19):

| Seam | Owner | Evidence | Consumed by |
|---|---|---|---|
| Difficulty scalar provider | `DifficultyDirector` / `DifficultyScalarsProvider` (4 presets × 8 scalars) | `Assets/Ashfall.Core/Difficulty/*`; `src/Main.Difficulty.cs`; one consumer at `src/Main.EvolvingWorld.cs:193` | EN-01, EN-07 |
| War chain authored-day clock | `FactionWarChainRunner.ToAuthoredDay` (180→480) | `FactionWarClockTests` 1/1 | EN-01 |
| War consequence routing template | clash/decree/stage/chain → radio + journal + sound-ranging | `WireFactionWarConsequenceRouting()` (`src/Main.YearOfAsh.cs`) | EN-01 (economy/chronicle routes) |
| Graph travel + fog | `WastelandMapSystem.PlanRoute` (`:581`), `DiscoverSurvey` (`:258`), Unknown-fog dispatch refusal | `WildlifeMapOverlayTests` 1/1; caravan hops + trade-network overlay | EN-02 |
| Completion history v2 | `CampaignCompletionHistory` stamps `difficultyPresetId` | `CampaignCompletionHistory.cs:44`; tests 11/11 | EN-07 |
| Port contract zero-deferred | 262 seams, 180 HOST_REQUIRED, 0 DEFERRED | `generate-port-contract.py --check` | EN-06 |
| Single catalog path | allowlist = the authority only | `CatalogPathForbiddenGateTests` 2/2 | EN-06 |
| Manifest bootstrap (restore path) | `ExecuteSubsystemManifestBootstrap` | `src/Main.Lifecycle.cs:542`; sole call `src/Main.SaveOrchestrator.cs:163` | EN-06 (needs the companion Plan 04 first) |
| Black-market heat / trust / settlement | `BlackMarketSystem` + `BlackMarketSettlementService` | settlement 18/18; panel contracts 3/3 | EN-03 |
| Ward staffing gate | `MedicalWardSystem.StaffingPreflight` + `ward` duty role | `MedicalWardSystemTests` 14/14 | EN-04 |
| Distress follow-up/audio ledgers | `DistressFollowUpScheduler`, `DistressAudioCueResolver`, V6 save | 19+10 focused tests; content 17+7 / 66+36 | EN-05 |
| Restock priority | `ShelterBarterSystem.ComputeItemPriorityScore` | `:283`, consumed `:307-316`; 6/6 | EN-03 |

## A.2 EN gate matrix (before → after the 2026-09-18/19 wave)

| EN | Declared gate (fifteen program Part D) | Status at 2026-09-19 |
|---|---|---|
| EN-01 Difficulty-Consequence Weave | Plans 13 + 06 + 08 sealed | **06 sealed · 08 sealed · 13 partial** (full binding = companion Plan 05, available now) |
| EN-02 The Living Map | Plan 07 P4+ sealed | **SEALED** — graph travel, fog gating, caravan hops all landed |
| EN-03 Underground Economy Pressure | Plan 14 sealed + XP-07 premise | **BLOCKED** (Plan 14 = F13 design sign-off) |
| EN-04 Rehabilitation Medicine | Plans 01 + 15 sealed | **01 sealed · 15 BLOCKED** (F14 schema sign-off) |
| EN-05 Signal Continuity & Voice | Plan 02 sealed + XP-09 premise | **02 available** (companion Plan 01) · XP-09 premise-check-first |
| EN-06 One Bootstrap Path | Plans 09–11 sealed | **09 sealed · 11 sealed · 10 available** (companion Plan 04) |
| EN-07 Chronicle & Aspiration | Plan 08 sealed + E1 claimed | **08 sealed · E1 available** (companion Plan 06) |
| EN-08 Ledger Truth Program | roster reconcile items sealed | **in flight** — companion Plans 02/06 + census flips |

## A.3 Premise-check quick reference (per proposal, read-only, today)

| EN | First thing to verify at P0 | Probe |
|---|---|---|
| EN-01 | the four consumer sites still read the owner chain | grep `HostileEncounterMult` consumers; read `CrisisPredictionModel` deadline input; read `ApplyShock` call sites; read `EconomyMarketRumorRules` inputs |
| EN-02 | `PlanRoute` + fog semantics unchanged; panel route battery intact | `WastelandMapTests` baseline; `--player-panels-uitest` |
| EN-03 | heat/trust owners unchanged; no parallel pressure state appeared | grep `pressure` in `Assets/Ashfall.Core/Economy/` |
| EN-04 | ward gate + uniform ramp decision intact | `MedicalWardSystemTests` 14/14; option-ii note in `PLAN_24_CLOSEOUT.md` |
| EN-05 | ledgers + content still green | Radio suite; content counts (17+7 / 66+36) |
| EN-06 | three contributing gates green | port-contract `--check`; forbidden-path gate; bootstrap call site |
| EN-07 | v2 store semantics unchanged | `CampaignCompletionHistoryTests` 11/11; DEC-20 row |
| EN-08 | truth-gap table still accurate | re-run the §C.8.1 probes |

## A.4 Standing premise corrections

- **XP-02's premise is consumed**: "expeditions/caravans never query
  `WastelandMapSystem`" was true at proposal time and is false now — the
  2026-09-19 session wired exactly that. EN-02 and the Part D re-scope
  inherit the finished seam; nobody rebuilds a `GraphTravelPlanner` beside
  the canonical owner.
- **XP-03's premise is consumed** for the clock and the radio/journal/
  sound-ranging routes; the economy and chronicle routes remain genuinely
  unbuilt and are folded into EN-01's design rather than duplicated.
- **XP-05 is already live** (SOFC inventory fuel with grid fallback) —
  verify-and-record only.
- **DEC-17 (presenter skill tree) and DEC-18 (phobia growth) are RETIRED**:
  XP-09/XP-10 are not blocked, they are *retired-adjacent* — any revival is
  a reversal decision, foreman-only (Part D.5/D.6).
- **D20 HOLDs stand**: EN/XP work that would touch Plans 192/199 territory
  (player-route DTOs, human population owner) routes through the D20
  signatures, not around them.

## A.4 Player-visible outcome map (what each EN adds to the game)

| EN | Player-visible outcome | Why it matters (design intent) |
|---|---|---|
| EN-01 | the world reacts to the chosen difficulty: harsher war stages, tighter crisis deadlines, heavier shocks and rumors under DIRGE; gentler under SPARING | one knob, coherent world response — difficulty stops being a survival-only multiplier |
| EN-02 | a map screen that plans a route honestly: candidate path, per-edge conditions, uncertainty bands, closures, war-front warnings, cost estimates | route information leaves dispatch advisories and becomes legible before commitment |
| EN-03 | fencing pressure becomes visible: heat tiers, cut-goods risk at inspection, relocation when heat overflows | the underground economy gains a single, explainable pressure system |
| EN-04 | one ward screen schedules every recovery track (affliction discharge, prosthetic adaptation) behind the staffing gate | recovery stops being scattered across panels |
| EN-05 | rescued senders live on: dated journal arcs with authored aftermath and audio cues | rescue decisions acquire long-term narrative weight |
| EN-06 | an invisible but standing guarantee: identical setup on every lifecycle path, zero deferred seams, one data path | future waves inherit a provably wired host |
| EN-07 | the campaign LEDGER strip: prior runs by difficulty and ending, completed arcs | aspiration becomes evidence-governed and player-visible |
| EN-08 | a truthful queue: ledgers that agree, a certified next wave head | the planning process itself becomes trustworthy |

## A.5 Cross-EN composition matrix

| | EN-01 | EN-02 | EN-03 | EN-04 | EN-05 | EN-06 | EN-07 | EN-08 |
|---|---|---|---|---|---|---|---|---|
| EN-01 | — | war-front flags | shock band feeds pressure context | — | — | — | severity feeds chronicle rows | routes cited as evidence |
| EN-02 | — | — | inspection sites on route | — | — | — | — | — |
| EN-03 | — | — | — | — | — | — | — | restock truth input |
| EN-04 | — | — | — | — | — | — | — | — |
| EN-05 | — | — | — | — | — | — | arcs feed aspiration metrics | — |
| EN-06 | — | — | — | — | — | — | — | gates cited in rerank |
| EN-07 | — | — | — | — | — | — | — | metrics feed review |
| EN-08 | — | — | — | — | — | — | — | — |

The matrix shows why the recommended order (E.2) is value-ordered: EN-02
and EN-06 unlock no downstream waits, while EN-01/EN-07 feed EN-08's
head certification with their evidence.

## A.6 Lineage: from the fifteen program's sketches to these full proposals

The fifteen program's Part D carried eight ~15-line sketches. This document
develops each into a full proposal. The mapping (so nothing is lost and
nothing is silently added):

| EN | Fifteen-program sketch (verbatim scope) | This document adds |
|---|---|---|
| EN-01 | stage severity × difficulty; crisis deadline scalar; chronicle preset tag; monotonicity soak | worked band-projection spec; economy/chronicle route designs (the XP-03 remainder); acceptance, risk, and test tables; phase gates |
| EN-02 | route-planning surface over the planner read model; per-edge conditions; uncertainty bands; closures; cost estimates; war-front flags | panel DTO spec; ownership/seam map; estimator-parity gate design; acceptance and risk tables |
| EN-03 | `EconomyPressureRules` joining funds/heat/purity into tiers; 20-day soak | tier-function contract; neutral-input interim design; relocation routing; phases and DoD |
| EN-04 | recovery slate read model; ward panel rendering; existing treatment commands | slate contract; staffing interaction tests; absence-composition design |
| EN-05 | `RescuedArcProjection`; journal arcs with cues; optional presenter-quality tag | arc projection contract; exactly-once guarantees; replay extension design |
| EN-06 | composite gate: lifecycle parity + forbidden-path minimum + zero DEFERRED | worked gate spec (selftest + ratchet + stragglers); false-parity guard |
| EN-07 | E1 consumes the chronicle; LEDGER strip; claim E1 first | strip DTO; DEC-20 boundary risk table; metrics wiring |
| EN-08 | census rerank; flipped rows cite evidence; head certification | 7-step rerank protocol table; truth-gap inventory (the P0 artifact) |

Nothing outside the sketches' scope was invented; every addition is a
specification of *how* the sketch lands, not *what* it lands.

---

# Part B — Authorization Boundary & Selection

## B.1 What "authorization" means here

Each EN proposal is a design + seam map + phase plan. One foreman line
("EN-0n: authorized" optionally with scope amendments) converts it into a
claimable package in `INTEGRATION_PLANS.md`. Until that line exists:

- no builder claims an EN package;
- no EN work rides inside another package (no stealth integration);
- the companion program's signature-free plans proceed independently — EN
  authorization is never a blocker for them.

## B.2 Selection rules (what earned a full expansion here)

1. The proposal extends seams that **exist and are verified live** (§A.1)
   — no speculative rails.
2. It composes at least two finished systems into one player-visible
   surface or one standing invariant — the "build on top" test.
3. It is bounded: named files, named owners, named tests, named rollback.
4. It reverses nothing signed or retired.

## B.3 Authorization line format

Each signature in Part F follows the repo's recorded-verbatim convention
(`docs/plans/wave8_part2/C2_DECISION.md:32` is the precedent):

```
EN-<nn> <name>: authorized [scope amendments, or "none"].
```

Rules: (1) the line is pasted back verbatim into the authorization record
(the package log cites it); (2) scope amendments narrow — they never widen
into green-field; (3) a deferred line ("EN-<nn>: deferred") keeps the
proposal queued with its gate named; (4) a declined line closes it and the
Part F row records the verdict.

## B.4 Scope amendment protocol

If a builder discovers, mid-execution, that an authorized design premise is
false (the XP-05 precedent), the protocol is: stop at the phase gate →
record the correction in the package log → propose the amended scope as a
new authorization line → never bend the signed design silently. This is
AGENTS.md Rule 10 applied to expansions.

## B.6 What a builder may NOT infer from this document

- No authorization: the designs here are proposals until a Part F line
  exists. Absence of a signature is not implicit approval.
- No premise proof: §A.1 evidence was verified 2026-09-19 and *will* drift;
  every package re-verifies at P0.
- No claim: nothing here reserves paths; claims live only in
  `WORKTREE_OWNERSHIP.md`.
- No precedence: where this document and the live ledgers disagree, the
  ledgers win; the discrepancy is reported, not resolved unilaterally
  (Rule 10).
- No scope elasticity: "the design sketch says" is never a basis to widen
  an authorized scope; amendments go back for a line (B.4).

## B.3 Exclusions

- Green-field pillars with no finished rails (XP-09/XP-10 revivals, mass
  migration, settlement networks) — out of scope until a reversal decision.
- Census tranche-2 audits — folded into EN-08's protocol, not expanded here.
- The companion program's eight plans — already fully specified there.

---

# Part C — The Eight Enhanced Expansion Proposals

---

## EN-01 — Difficulty-Consequence Weave

**Builds on:** Plan 13/XP-01 full binding (companion Plan 05, available),
Plan 06 (war consumers + clock, sealed), Plan 08 (chronicle, sealed).
**Gate:** companion Plan 05 sealed + one foreman authorization.
**Premise:** difficulty currently multiplies survival pressure (one
consumer live: `HostileEncounterMult`) but not world drama. With the war
chain routable and the scalar provider bound, shock magnitude, crisis
deadlines, and rumor severity can read the same director — one knob,
coherent world response.

### C.1.1 Current evidence

- The 8 authored scalars include exactly the weave surface:
  `crisis_deadline_mult` (no consumer yet — companion Plan 05 row 8 of the
  consumer matrix), `market_price_mult`, `equipment_decay_mult`,
  `hostile_encounter_mult` (live), and the four needs/radiation/disease
  survival multipliers.
- The war chain now fires inside the campaign window (`ToAuthoredDay`), and
  its projection events are routed (radio + journal + sound-ranging) — the
  routing template EN-01 reuses for the economy and chronicle routes.
- `CrisisPredictor.Evaluate` assembles from canonical host read models
  (`DailyBriefingReportBuilder.AppendCrisisWarnings` integration, 8/8
  tests); the deadline input is a named seam awaiting the scalar.
- Market shocks are applied idempotently through `MarketSystem.ApplyShock`
  (severity bp clamped [500,3000]); rumors project through
  `EconomyMarketRumorRules` (deterministic, restore never replays).

### C.1.2 Design

1. **War-chain severity × difficulty** (Core, at the event site, never in
   panels): when a chain stage surfaces, its severity tier (authored) is
   scaled by a bounded integer projection of the difficulty band
   (sparing softens, dirge hardens). The scaling is a pure function in
   Core, unit-pinned at 1.0 parity for `difficulty_standard`.
2. **Crisis deadline consumption**: `CrisisPredictor`'s deadline runway
   multiplies by `crisis_deadline_mult` (sparing = more time, dirge = less)
   — the authored scalar finally has its consumer, and the briefing shows
   the adjusted date.
3. **Shock magnitude band**: when a war-stage event routes an economy
   shock (the new EN-01 route, below), the severity bp is chosen from the
   authored band and biased by the band projection — still clamped by the
   existing [500,3000] envelope.
4. **Rumor severity line**: `EconomyMarketRumorRules` projections carry a
   severity word derived from the same band (text, never a fake number).
5. **War-chain economy route (the XP-03 remainder)**: territorial clash →
   one bounded market shock on the affected category, idempotent per clash
   (the `ApplyShock` idempotency key pattern), routed through the existing
   `Main.Economy` relay, never a second ledger.
6. **War-chain chronicle route (the XP-03 remainder)**: chain resolution →
   one completion-history-tagged journal entry through `JournalSystem`
   (exactly-once via the fired-key ledger pattern), no new save section.
7. **Balance-soak harness** (seeded, 30/60/90-day): asserts difficulty
   monotonicity — dirge never easier than sparing on any tracked axis
   (needs pressure, crisis count, shock count, war severity exposure), and
   standard ≡ pre-weave fingerprints (parity guard).

### C.1.3 Ownership & seams

| Concern | Owner (extended, not forked) |
|---|---|
| Scalar resolution | `DifficultyDirector` (companion Plan 05 completes the binding) |
| War stage severity | `FactionWarChainRunner` (authored tiers) + new pure band projection in Core |
| Crisis deadline | `CrisisPredictionModel` deadline input |
| Economy shock | `MarketSystem.ApplyShock` (idempotent, clamped) |
| Rumor text | `EconomyMarketRumorRules` |
| Chronicle row | `JournalSystem` + completion-history tag (read-only) |

### C.1.4 State, save, determinism

No new save sections: shocks ride the market save, journal rows the journal
save, and the band projection is pure. The fired-key ledgers make the new
routes exactly-once; the soak harness pins determinism (same seed ⇒ same
war exposure per preset).

### C.1.5 Failure modes

Difficulty leaking into presentation arithmetic (all scaling in Core at
the event site); double shocks (idempotency keys); parity drift (the
standard-preset fingerprint guard fails CI).

### C.1.6 Phases

- **P0 — Premise notes** for the four consumer sites (war severity, crisis
  deadline, shock band, rumor line). Gate: notes recorded.
- **P1 — Band projection + crisis deadline.** Gate: unit + parity tests.
- **P2 — Economy + chronicle routes.** Gate: idempotency + exactly-once
  tests; adjacent suites green.
- **P3 — Soak harness + monotonicity gate.** Gate: 30/60/90-day runs
  recorded; dirge ≥ standard ≥ sparing on every axis.
- **P4 — Closeout.** Gate: ledger row + evidence log.

### C.1.7 Tests & focused verify

New `DifficultyConsequenceWeaveTests` (band projection, parity, idempotency);
`CrisisPredictionTests` (16/16 baseline); `Economy` suite for the shock
route; `Journal` parity; the soak harness as a long-running focused target.

### C.1.8 File impact & rollback

`Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs` (severity seam),
`Assets/Ashfall.Core/Campaign/CrisisPredictionModel.cs` (deadline input),
`Assets/Ashfall.Core/Economy/MarketSystem.cs` (no change — consumer only),
`src/Main.YearOfAsh.cs` / `src/Main.Economy.cs` (route wiring), Core band
projection (new, engine-free). Rollback: revert code; data unchanged; no
save migration.

### C.1.9 Band projection — worked specification

The projection is a pure Core function with this contract:

```
int DifficultyBandProjection::Apply(
    int authoredSeverityPermille,      // authored tier, e.g. 1000 = nominal
    DifficultyScalarsProvider scalars) // resolved, immutable
    -> int boundedSeverityPermille     // clamped [authoredMin, authoredMax]
```

Rules: (1) nominal parity — `scalars == Legacy` or `difficulty_standard`
must return `authoredSeverityPermille` exactly (byte-parity pin); (2) the
multiplier is derived from the scalar *family* the consuming site names
(e.g. war severity reads `hostile_encounter_mult`; crisis deadline reads
`crisis_deadline_mult`) — never an ad-hoc constant; (3) integer permille
arithmetic only, clamped to the site's authored envelope; (4) zero RNG;
(5) unit tests pin each preset × each site. Presentation never calls it —
the event site calls it once and stores the result in the event payload.

### C.1.10 Phase-gate checklist

| Phase | Gate | Must also hold |
|---|---|---|
| P0 | premise notes recorded (4 sites) | notes cite file:line |
| P1 | band projection + deadline tests green | `CrisisPredictionTests` 16/16 baseline unchanged at standard |
| P2 | route idempotency + exactly-once tests green | radio suite + journal parity green |
| P3 | soak: 30/60/90-day monotonicity | standard ≡ pre-weave fingerprint |
| P4 | closeout row + log | docs-index regen (owner) |

### C.1.11 Acceptance / definition of done

| # | Outcome | Proof |
|---|---|---|
| 1 | DIRGE war stages are observably harsher than SPARING at the same seed | soak axis: war severity exposure |
| 2 | Crisis deadlines scale with `crisis_deadline_mult` | predictor tests per preset |
| 3 | Clash → one bounded shock, idempotent per clash | economy route tests |
| 4 | Chain resolution → exactly one chronicle row | fired-key ledger test |
| 5 | Standard ≡ pre-EN-01 behavior | fingerprint parity |
| 6 | No panel performs scaling | source gate (grep the panels for scalar reads = 0) |

### C.1.12 Risk table

| Risk | Class | Mitigation |
|---|---|---|
| Difficulty leaking into presentation | HIGH | scaling only at Core event sites; source gate |
| Double shocks on replayed events | HIGH | `ApplyShock` idempotency keys (existing pattern) |
| Parity drift after scalar edits | MEDIUM | standard-preset fingerprint in CI |
| Soak flakiness | LOW | seeded harness; two-run stability assertion |

**Handoff — MUST PRESERVE:** shock clamps, fired-key ledgers, director
fail-closed semantics. **MUST ADD:** one band projection, two routes, one
deadline consumer, one soak gate. **MUST NOT DO:** scale anything in a
panel; create a second war ledger. **VERIFY WITH:** §C.1.7.
**FIRST SAFE STEP:** P0 premise notes (read-only).

---

## EN-02 — The Living Map

**Builds on:** Plan 07 (sealed — planner, survey bands, closures, caravan
hops), Plan 06 (war fronts, sealed), XP-08's follow-on horizon.
**Gate:** **satisfied** — awaiting one foreman authorization only.
**Premise:** route information currently lives in dispatch advisories; once
edges carry conditions, survey bands, and war closures, one map
presentation can plan routes honestly.

### C.2.1 Current evidence

- `WastelandMapSystem` owns the graph: `PlanRoute(fromId, toId)` (`:581`),
  per-node fog state + provenance + survey traits (`:715` knowledge-state
  comment), `DiscoverSurvey(nodeId, surveySourceId, day, traits)` (`:258`),
  and map trap-site location registration (port-contract LIVE_VIA_CORE).
- Expeditions consume map distance with Unknown-fog dispatch refusal;
  caravans expand authored hops through `PlanRoute` and wait on Unknown
  nodes; the trade network overlays graph ticks; wildlife merges
  map-connected sectors (`WildlifeMapOverlayTests` 1/1).
- `WastelandMapView.cs` consults fog state for rendering; the ten former
  orphan nodes now have canonical `locations.json` records with a loader
  gate preventing future orphans.
- War fronts: `FactionWarMapWidget` refreshes on `OnFactionStandingChanged`;
  clash/decree/stage/chain events are routed (radio/journal/sound-ranging)
  — a war-front edge flag can be derived from the chain state read model.

### C.2.2 Design

1. **Route-planning surface** (new panel, registered through the full route
   battery): select destination → candidate route via `PlanRoute` →
   per-edge condition class (authored), survey uncertainty band (text,
   never fake precision), known closures, and cost estimates from the
   existing owners (expedition estimate; caravan transit days).
2. **Read model only**: a Core `RoutePlanReadModel` that composes
   `WastelandMapSystem` queries into the presentation DTO — it owns
   nothing, saves nothing, and issues no commands.
3. **War-front edge flags**: edges whose terminal nodes sit in an active
   war-front sector (chain state read model) render a warning class; the
   estimate already consumes the expedition danger composer, so the flag is
   presentational truth from Core state.
4. **Honest uncertainty**: fog/Unknown nodes show "unverified" with the
   survey band text; the panel never invents distances or inventories.
5. **Strictly presentation**: the panel issues the existing dispatch
   commands (expedition dispatch, caravan scheduling) and renders Core
   estimates — zero recomputation (rule 6 of the integration doctrine).

### C.2.3 Ownership & seams

| Concern | Owner |
|---|---|
| Graph, fog, surveys, routes | `WastelandMapSystem` (read-only consumption) |
| Cost estimates | `ExpeditionSystem` estimate + caravan transit-day owners |
| War-front flags | chain-state read model (EN-01-adjacent, but independently derivable) |
| Panel | new route-planning panel + registry row + PlayerSurfaces entry |

### C.2.4 State, save, determinism

None: read-model over persisted state. Determinism inherited (route
planning is deterministic; no RNG).

### C.2.5 Failure modes

Estimator drift vs the panel (estimator parity test against dispatch
advisories); dead routes (all nodes resolve through the loader gate);
scope creep into pathfinding rewrites (forbidden — `PlanRoute` is the only
planner).

### C.2.6 Phases

- **P0 — Premise check + panel claim.** Gate: route battery claim row.
- **P1 — Read model + tests.** Gate: read-model unit tests green.
- **P2 — Panel + registration.** Gate: PanelRouteGate 20/20-style parity,
  lifecycle, a11y.
- **P3 — Estimator parity + war-front flags.** Gate: parity test green.

### C.2.7 Tests & focused verify

New `RoutePlanReadModelTests`; `PanelRouteGateTests`; `WastelandMapTests`
(68/68 baseline); `--panel-bind-lifecycle-selftest`; `--ui-a11y` selftest;
`--player-panels-uitest`.

### C.2.8 File impact & rollback

New `Assets/Ashfall.Core/World/RoutePlanReadModel.cs`, new panel under
`src/UI/`, registry + PlayerSurfaces + GameFlow rows. Rollback: delete the
panel and read model; no data or save touched.

### C.2.9 Panel DTO sketch

```
RoutePlanView {
  destinationId, destinationName;
  route: [ { nodeId, edgeConditionClass, surveyBandText,
             isClosureKnown, isWarFront, legDistanceKm } ];
  totals { distanceKm, estimatedDays, unverifiedLegs };
  expeditionCost { consumablesSummary, dangerBand }   // from the estimate owner
  caravanTransitDays?                                  // when region IDs resolve
  dispatchCommand { kind: expedition|caravan, targetId }
}
```

The DTO is composed by the read model from Core queries only; every field
names its owning query in the read model's doc comment so a reviewer can
trace truth per field.

### C.2.10 Phase-gate checklist

| Phase | Gate | Must also hold |
|---|---|---|
| P0 | premise check + claim row | route battery claim |
| P1 | read-model unit tests | no mutation APIs exposed |
| P2 | panel registered; route/lifecycle/a11y gates | `PanelRouteGateTests` parity pattern |
| P3 | estimator parity test vs dispatch advisories | war-front flag truth test |

### C.2.11 Acceptance / definition of done

| # | Outcome | Proof |
|---|---|---|
| 1 | Select any discovered destination → honest candidate route | read-model tests over the real map catalog |
| 2 | Unknown legs show "unverified" with the survey band | a11y: words, never fake precision |
| 3 | Known closures and war fronts flagged with text | flag truth test |
| 4 | Dispatch issues the existing commands | panel issues zero new commands |
| 5 | Estimates identical to dispatch advisories | estimator parity test |
| 6 | Keyboard/controller operable | lifecycle + a11y selftests |

### C.2.12 Risk table

| Risk | Class | Mitigation |
|---|---|---|
| Panel recomputes estimates | HIGH | read model composes Core queries only; source gate on arithmetic |
| Dead routes after map edits | MEDIUM | loader gate prevents orphan nodes |
| Fog-state race (restore mid-session) | MEDIUM | read model re-queries on bind; lifecycle ×100 gate |
| Scope creep into pathfinding | HIGH | `PlanRoute` is the sole planner (hard rule) |

**Handoff — MUST PRESERVE:** `PlanRoute` as sole planner; fog semantics.
**MUST ADD:** one read model + one panel. **MUST NOT DO:** recompute
estimates; mutate map state from the panel. **VERIFY WITH:** §C.2.7.
**FIRST SAFE STEP:** P0 (read-only premise check).

---

## EN-03 — Underground Economy Pressure

**Builds on:** Plan 14/XP-04 (BLOCKED — F13 design sign-off), Plan 03
restock truth (available), XP-07 provenance (sequenced).
**Gate:** Plan 14 sealed + XP-07 premise check (if recognition is in scope).
**Premise:** heat, purity, and faction recognition become a single pressure
system: fencing recognized loot raises heat; cut goods surface at
inspection; relocation (heat ≥ 10) rediscovers markets.

### C.3.1 Current evidence (expanded)

- Live: `BlackMarketSystem` heat/trust/loans (heat decays daily; trust
  bounded [0,100]; loans one active per syndicate; idempotent overdue
  events with a fired-key ledger; bounty escalation delegated to the
  canonical `FactionBountySystem` — no second bounty ledger), canonical
  settlement (`BlackMarketSettlementService`, 18/18 tests, immediate
  canonical-inventory semantics per DEC-02), panel surface with explicit
  due dates and no restock button (reopen never rerolls, R4 honored).
- Restock priority is live and deterministic: `ComputeItemPriorityScore`
  (`ShelterBarterSystem.cs:283`) with the `PriorityScorer` seam (`:136`),
  consumed by the arrival sort (`:307-316`); tests 6/6.
- Black-market stock generation is campaign-gated, day-keyed
  (`CampaignRngManager.Fork(BlackMarketStock, day)`), same-day reopen
  never rerolls — the exact determinism discipline EN-03's soak inherits.
- Missing (and decision-gated): the funds authority (XP-04 `FundsLedger`),
  the purity map (XP-04 `FencePurityRolls`), and per-instance provenance
  (XP-07). All three sit behind F13/sequencing — EN-03's composite cannot
  be built until its inputs exist.
- Contact discovery is idempotent and campaign-gated — the relocation
  trigger (heat ≥ authored threshold) can route through it without a new
  discovery authority.

### C.3.2 Design (ready to execute once Plan 14 lands)

1. **Core `EconomyPressureRules`** — pure module joining the three states
   (funds ledger, black-market heat, purity map) into derived pressure
   tiers; consumed by encounter/inspection content and the TradePanel risk
   line. One consumer surface, no new ledger.
2. **Pressure tiers** as data (bounded, monotonic in each input); tier
   words + numbers in the panel, never color-only.
3. **Relocation trigger** (heat ≥ authored threshold) routes through the
   existing black-market contact discovery (idempotent, campaign-gated) —
   no new discovery authority.
4. **Provenance feed (XP-07, later)**: faction recognition of `looted`
   goods enters the same tier function — the tier function's signature is
   designed now so the later input is additive.

### C.3.3 Ownership, phases, tests (executed only after the gate)

Pressure rules in Core (pure, engine-free); consumers at the existing
encounter/inspection call sites; panel line on `BlackMarketPanel` /
`TradePanel`. Phases: P0 premise notes → P1 rules + tier data → P2
consumers → P3 relocation → P4 20-day pressure soak (continuous ==
mid-reload). Tests: economy suite + new rules tests + the soak.

### C.3.3 Tier function — worked specification

```
EconomyPressureRules::Evaluate(
    FundsLedgerState funds,          // XP-04 (gate-blocked input; neutral when absent)
    int blackMarketHeat,             // live owner field
    int averagePurityPermille,       // XP-04 purity map (neutral when absent)
    ProvenanceSignal? lootedGoods)   // XP-07 (additive, optional by design)
    -> EconomyPressureTier { tier, bandWord, numericLine }
```

Rules: pure function, no state, no RNG; tiers are data-authored
(`economy_pressure_tiers.json`, snake_case, strict loader, bounded
inputs/outputs, validator hook); each input has a neutral value so the
function composes correctly *before* XP-04/XP-07 exist (heat-only tiering
is a valid interim); monotonic in each input; the panel line renders
`bandWord + numericLine` (words + numbers, never color-only).

### C.3.4 Phases (after F13)

| Phase | Gate |
|---|---|
| P0 — premise notes for the three consumer sites | notes cite file:line |
| P1 — Core rules + tier data + validator | catalog + rules tests green |
| P2 — consumers (encounter/inspection + panel risk line) | economy suite green |
| P3 — relocation trigger through contact discovery | idempotency test |
| P4 — 20-day pressure soak (continuous == mid-reload) | soak green, bounded |

### C.3.5 Acceptance / definition of done

| # | Outcome | Proof |
|---|---|---|
| 1 | One explainable pressure line on the trade surface | panel contract test |
| 2 | Heat-only tiering works before XP-04 lands | neutral-input tests |
| 3 | Relocation fires exactly once per overflow | idempotency test |
| 4 | No second heat/purity ledger | source gate (one owner per input) |
| 5 | 20-day soak parity | continuous == mid-reload fingerprint |

### C.3.6 Risk table

| Risk | Class | Mitigation |
|---|---|---|
| Parallel pressure authority | HIGH | rules module is pure; inputs stay with their owners |
| Premature build before F13 | HIGH | phases gated; P0 refuses to start |
| Tier data drift | MEDIUM | validator bounds + monotonicity tests |

**Handoff — MUST PRESERVE:** canonical settlement; heat/trust bounds.
**MUST ADD:** one pure rules module + tier data. **MUST NOT DO:** a second
heat ledger; presentation-side pressure math. **VERIFY WITH:** economy
suite + soak. **FIRST SAFE STEP:** none until F13 — this proposal is
gate-blocked by design.

---

## EN-04 — Rehabilitation Medicine

**Builds on:** Plan 15/XP-06 (BLOCKED — F14 schema sign-off), Plan 01
recovery-ramp outcome (sealed, option ii), XP-10 therapeutic protocol
(retired-adjacent).
**Gate:** Plans 01 + 15 sealed (01 done; 15 awaits the schema package).
**Premise:** recovery is currently per-system (uniform affliction ramp,
prosthetic adaptation future, therapeutic protocols retired) — one
medical-pipeline arc presents and schedules them together through the ward
the staffing gate already guards.

### C.4.1 Current evidence (expanded)

- `MedicalWardSystem` runs procedures behind `StaffingPreflight` (ward
  duty role authored in `duty_roles.json` with `skill_paramedic` and
  hazard class `medical`; `DutyRosterIds.RoleWard`; the preflight fails
  with `Fail("ward_unstaffed")`), 14/14 tests; host binding in
  `Main.Medical.cs` reads the roster's ward assignment; the preflight
  falls open by design when the roster is null (old-save semantics).
- Admissions carry no cause field, and the affliction-specific recovery
  ramp was closed per option (ii): the uniform data-authored discharge
  window (`discharge_recovery_days`) **is** the ramp. EN-04 renders that
  truth; it never re-opens the closed decision.
- Amputation owns limb state and the movement multiplier seam
  (`GetMovementSpeedMultiplier`, consumed by dispatch since C2-D3);
  bionics own surgery/rehab/maintenance with bounded capability and
  typed malfunctions. Prosthetics as an XP-06 pillar (limb-requirement
  gate, adaptation arcs) do not exist and await the F14 schema package.
- The medical record log (`MedicalRecordLog`, bounded 64, append-only,
  day+kind+ids only) and the afflictions panel rows are the presentation
  precedents EN-04's slate follows.

### C.4.2 Design (ready once Plan 15 lands)

1. **Recovery slate read model** on the medical owner: active affliction
   ramps (uniform discharge windows), prosthetic adaptation timelines
   (Plan 15), and — only if a reversal decision revives therapeutic
   protocols — those as a treatment kind. Read-only composition.
2. **Ward panel renders the slate**; treatment commands flow through the
   existing `RunProcedure` + staffing gate. No new medical authority.
3. **Scheduling** is derived (discharge day projections), never a second
   timer ledger.

### C.4.3 Ownership, phases, tests

Read model in Core (pure); rendering on the ward/afflictions panels;
phases gated on Plan 15's schema package. Tests: medical suite + read-model
units + staffing-preflight interaction tests (slate commands refuse when
unstaffed).

### C.4.4 Slate read model — worked specification

```
RecoverySlate::Build(
    MedicalWardState ward,           // admissions + discharge windows (uniform ramp)
    ProstheticAdaptationState? pros, // Plan 15 output (absent until F14)
    TherapeuticProtocolState? proto) // only if a reversal revives XP-10
    -> [ RecoveryEntry { survivorId, kind, startedDay, projectedDay, staffingNote } ]
```

Rules: read-only composition over owner state; `projectedDay` is derived
from each owner's persisted window (never a second timer); entries carry
the staffing note ("ward unstaffed" when the preflight would refuse); the
panel renders the slate and issues only existing treatment commands.

### C.4.5 Phases (after F14)

| Phase | Gate |
|---|---|
| P0 — premise notes (ward + amputation + bionics seams) | notes cite file:line |
| P1 — slate read model | unit tests incl. staffing-refusal composition |
| P2 — ward panel rendering | medical suite + panel battery green |
| P3 — interaction tests (slate commands respect preflight) | staffing interaction tests |

### C.4.6 Acceptance / definition of done

| # | Outcome | Proof |
|---|---|---|
| 1 | Every active recovery track appears in one slate | read-model coverage test |
| 2 | No fabricated cause fields or new timers | source gate |
| 3 | Commands refuse when ward unstaffed | interaction test |
| 4 | Affliction-only slate valid before Plan 15 output exists | absence-composition test |

### C.4.7 Risk table

| Risk | Class | Mitigation |
|---|---|---|
| Second recovery authority | HIGH | read model owns nothing; owners keep windows |
| Scope creep into rehabilitation mechanics | MEDIUM | F14's schema package is the only mechanics entry |

**Handoff — MUST PRESERVE:** staffing gate; uniform ramp decision (option
ii). **MUST ADD:** one read model + one rendering surface.
**MUST NOT DO:** fabricate cause fields; add a second recovery timer.
**VERIFY WITH:** medical suite. **FIRST SAFE STEP:** none until F14 —
gate-blocked by design.

---

## EN-05 — Signal Continuity & Voice

**Builds on:** Plan 02 (distress content seal — available as companion Plan
01), XP-09 (presenter skills — premise-check-first, retired-adjacent).
**Gate:** Plan 02 sealed + XP-09 premise decision.
**Premise:** rescued senders, follow-up chains, and (if XP-09 survives its
premise check) presenter quality can project into one continuity surface:
the rescued-sender arc as journal/chronicle entries with audio cues.

### C.5.1 Current evidence (expanded)

- The rescue mission manager, follow-up scheduler, trust ledger, and audio
  cue resolver are sealed with exactly-once ledgers and the V6 codec
  (`RadioSave` V5→V6 frozen shape; migration to empty default); the
  unified deterministic replay harness (Wave 5) drives the complete
  lifecycle detect → stages → answer → rescue → follow-ups → trust → cue
  through the real codec with stable fingerprints.
- Content is shipped: 24 authored follow-up entries (17 primary + 7
  expansion-effective) and 102 `audio_cue` occurrences (66 + 36); the
  primary-wins-overridden expansion rows remain untouched dead data by
  design. The PR3 seal tail (companion Plan 01) adds the validator rules,
  population replay, and closeout that EN-05 then builds on.
- Follow-up classes already author aftermath outcomes (employer/medical/
  revenge) — the arc vocabulary exists in data; signal trust is bounded
  [0,100] with the integer-permille availability specification retained as
  a tested pure math pin (DEC-06: no runtime selection pool).
- Journal integration is a proven pattern: the war routing writes
  permanent entries through `JournalSystem.TryAddRawEntry` with exactly-once
  keys; radio intercepts flow through `RadioHostSession.InterceptWarlordWarning`.

### C.5.2 Design

1. **`RescuedArcProjection`** — read model over the mission + follow-up
   ledgers: each resolved rescue yields a dated journal arc composed from
   the authored aftermath classes; presentation-only, no mission-state
   changes.
2. **Audio cue continuity** — the arc's terminal entry resolves its
   authored cue through the existing resolver (dedupe keys already
   persisted).
3. **XP-09 interaction (only if the premise check survives)**: broadcast
   quality tags arc visibility — a good broadcaster's rescue stories
   travel. Without the premise decision, EN-05 ships without this hook.

### C.5.3 Ownership, phases, tests

Read model in Core (radio domain, read-only); journal writes through the
existing journal owner; phases: P0 after Plan 02 seals → P1 read model →
P2 journal integration (exactly-once) → P3 replay extension (the Wave 5
harness gains arc assertions). Tests: radio suite + projection units +
replay extension.

### C.5.4 Arc projection — worked specification

```
RescuedArcProjection::Project(
    DistressRescueMissionLedger missions,   // resolved rescues (terminal states)
    DistressFollowUpLedger followUps,       // fired aftermath entries
    SignalTrustLedger trust)                // sender trust history
    -> [ ArcEntry { signalId, survivorOrSenderRef, resolvedDay,
                    aftermathClass, journalTextKey, audioCueId?, nextArcDay? } ]
```

Rules: read-only projection; each resolved rescue with at least one fired
follow-up yields one arc; the arc's terminal entry resolves the authored
`audio_cue` through the existing resolver (persisted dedupe keys apply — no
replay after reload); journal writes go through the journal owner's
deduped API (knowledge-key pattern) so restore never replays; mission
state is never mutated by the projection.

### C.5.5 Phases (after companion Plan 01 seals)

| Phase | Gate |
|---|---|
| P0 — premise notes (mission/follow-up/trust ledgers) | notes cite file:line |
| P1 — projection read model | unit tests over authored chains |
| P2 — journal integration (exactly-once) | journal parity + radio suite green |
| P3 — replay extension (arc assertions in the Wave 5 harness) | replay green, fingerprints stable |
| P4 — (optional) XP-09 visibility hook | only with the premise decision |

### C.5.6 Acceptance / definition of done

| # | Outcome | Proof |
|---|---|---|
| 1 | Every authored aftermath class projects into a dated arc | projection coverage test (24 chains) |
| 2 | Arc terminal cues resolve without replay | dedupe key test |
| 3 | Restore never re-journals an arc | journal parity test |
| 4 | Zero mission-state mutation | source gate |

### C.5.7 Risk table

| Risk | Class | Mitigation |
|---|---|---|
| Projection drift from ledger semantics | MEDIUM | projection consumes the same public ledgers the replay harness drives |
| Journal flooding | MEDIUM | one entry per arc; journal dedup keys |
| Presenter-skill scope creep | HIGH | hook is optional and unsigned |

**Handoff — MUST PRESERVE:** exactly-once ledgers; Intercept-gated
playback. **MUST ADD:** one read model + journal projection.
**MUST NOT DO:** mutate mission state from the projection; revive
presenter skills without a reversal decision. **VERIFY WITH:** radio
suite + replay. **FIRST SAFE STEP:** none until Plan 02 (companion Plan 01)
seals.

---

## EN-06 — One Bootstrap Path (Host Integrity)

**Builds on:** Plan 10 (available as companion Plan 04), Plan 11 (sealed),
Plan 09 (sealed).
**Gate:** **nearly satisfied** — companion Plan 04 is the last input.
**Premise:** the three host-hygiene completions compose into a single
verifiable claim: every subsystem sets up through the manifest on every
lifecycle path, every catalog read resolves through `CatalogPath`, every
port seam is live or retired.

### C.6.1 Current evidence (expanded)

- Port contract: 262 seams, 180 HOST_REQUIRED, **0 DEFERRED** (verified
  2026-09-19 with `generate-port-contract.py --check`). The ratchet history
  is instructive: 17 DEFERRED at Wave 11 Part 2 → 14 after the first
  2026-09-19 pass → 2 after the Phase-9 sweep (LIVE_VIA_CORE and
  OPTIONAL_HOST reclassifications with per-seam evidence) → 0 after the
  craft-gate and spiritual-coordinator bindings (`CraftingHostSession.Create`
  binds `BindCraftResultGate` to `ItemCatalog.Contains`; `SurvivorFate.
  OnSurvivorFate` feeds `RegisterDeath`; DX-02 closed).
- CatalogPath: allowlist = the authority only (`src/Host/CatalogPath.cs`);
  2/2 gate green. Two lowercase `res://assets/StreamingAssets/Data`
  references escape the ordinal pattern and are the P2 verification
  targets: `src/Audio/AudioCueCatalog.cs:407` and
  `src/Main.FlagshipInstitutions.cs:45`.
- Manifest bootstrap: `ExecuteSubsystemManifestBootstrap(LifecyclePhase?)`
  exists (`src/Main.Lifecycle.cs:542`) with 18 idempotent setup delegates
  registered; the sole call site is the restore path
  (`src/Main.SaveOrchestrator.cs:163`). The fresh path (`ComposeCampaign`,
  `src/Main.CampaignServices.cs:25+`) composes sessions directly — the
  exact gap the companion program's Plan 04 closes, after which EN-06's
  composite gate can assert identity.

### C.6.2 Design

One composite gate extension: a lifecycle selftest asserting the executed
setup set is identical across fresh-boot and restore paths; the
forbidden-path gate held at its documented minimum (plus the two
lowercase-path stragglers resolved); port policy DEFERRED count pinned at
zero (a ratchet test that fails if a DEFERRED row reappears). This is the
"host wiring completeness" invariant future waves inherit.

### C.6.3 Phases & tests

P0 wait on companion Plan 04 → P1 composite selftest (register in the
manifest + CLI help contract) → P2 straggler verification → P3 standing
gate documentation. Tests: the three contributing gates + the new lifecycle
assertion + the DEFERRED-ratchet test.

### C.6.4 Composite gate — worked specification

```
lifecycle-parity selftest:
  boot fresh game → record executed manifest action set (sorted ids)
  load campaign  → record executed manifest action set (sorted ids)
  assert sets identical AND both non-empty
port-ratchet test:
  parse docs/ci/port_contract_policy.json → assert DEFERRED count == 0
  (a new DEFERRED row fails CI until explicitly re-promoted with evidence)
forbidden-path gate:
  existing CatalogPathForbiddenGateTests, held at allowlist == authority
  + resolve or route the two lowercase res://assets/ stragglers
```

### C.6.5 Acceptance / definition of done

| # | Outcome | Proof |
|---|---|---|
| 1 | Fresh and restore execute identical setup sets | composite selftest |
| 2 | DEFERRED count pinned at zero | ratchet test |
| 3 | Catalog path allowlist == authority | existing gate + straggler resolution |
| 4 | Composite selftest registered (manifest + CLI help) | SelfTestManifest + help contract tests |

### C.6.6 Risk table

| Risk | Class | Mitigation |
|---|---|---|
| Green-by-widening (allowlist/ratchet relaxations) | HIGH | hard rule: never widen to go green |
| False parity (empty sets match) | MEDIUM | non-empty assertion |
| Shared-root race during companion P04 | MEDIUM | sequence after P04 lands |

**Handoff — MUST PRESERVE:** all three contributing gates' semantics.
**MUST ADD:** one composite assertion + one ratchet. **MUST NOT DO:** widen
allowlists to go green. **VERIFY WITH:** composite selftest.
**FIRST SAFE STEP:** P2 straggler verification (read-only, today).

---

## EN-07 — Chronicle & Aspiration Readiness

**Builds on:** Plan 08 (sealed), Plan 13/XP-01 (available as companion
Plan 05), E1/Plan 53 (available as companion Plan 06).
**Gate:** companion Plans 05 + 06 sealed.
**Premise:** with a truthful difficulty-tagged chronicle, the ambition
governance programme (E1A–E1P) has its read model; campaign aspiration can
be governed by evidence rather than aspiration docs.

### C.7.1 Current evidence

- Completion history v2: append-only, checksum-validated, difficulty-tagged
  (`difficultyPresetId`), 11/11 tests, user-store journey selftest green.
- The census E1 row is `READY-UNCLAIMED` with the full E1A–E1P programme
  (companion Plan 06).
- DEC-20 pins the boundary: the store owns no rewards, unlocks, profile,
  prestige, or New Game+.

### C.7.2 Design

1. **E1's rails-readiness slice consumes the chronicle projection as its
   single source of run history** (runs by difficulty, completed arcs).
2. **Campaign panel LEDGER strip** — player-facing aspiration surface:
   prior runs with preset, ending id, and completed arc counts (words +
   numbers; no fabricated prestige mechanics).
3. **Aspiration metrics** feed E1N (metrics/review) — drift triggers when
   the register and the chronicle disagree.

### C.7.3 Phases & tests

P0 wait on companion Plans 05/06 → P1 E1 readiness slice → P2 LEDGER strip
(panel battery: route, lifecycle, a11y) → P3 metrics wiring. Tests:
endgame suite + E1's own gates + panel battery.

### C.7.4 LEDGER strip — worked specification

```
CampaignLedgerStrip (read model over CampaignCompletionHistory):
  [ RunRow { presetId, presetDisplay, endingId, endedDay, arcsCompleted } ]
  summary { runsByPreset: map, completedArcs: int }
```

Rules: reads only the checksum-validated user-level store; renders words +
numbers; no rewards, unlocks, prestige, or NG+ mechanics (DEC-20); panel
registration through the standard route battery; localization-ready string
keys (no free-text composition).

### C.7.5 Acceptance / definition of done

| # | Outcome | Proof |
|---|---|---|
| 1 | Prior runs render with preset, ending, arcs | panel contract test over seeded history |
| 2 | Empty history renders an honest empty state | absence test |
| 3 | No prestige/reward surface | source gate + DEC-20 note |
| 4 | E1N metrics consume the same projection | metrics wiring test |

### C.7.6 Risk table

| Risk | Class | Mitigation |
|---|---|---|
| DEC-20 boundary creep | CRITICAL | no reward/unlock fields in the DTO; review gate |
| History schema drift | MEDIUM | v2 checksums; projection pinned by 11/11 tests |

**Handoff — MUST PRESERVE:** DEC-20 boundary. **MUST ADD:** one projection
consumer + one panel strip. **MUST NOT DO:** rewards/prestige/NG+.
**VERIFY WITH:** endgame suite + E1 gates. **FIRST SAFE STEP:** none until
the companion plans seal.

---

## EN-08 — Ledger Truth Program

**Builds on:** Plan 03 restock reconcile (available), Plan 01 Plan 24 rows
(sealed), Plan 12 quarantine drain (decision-gated), the census rerank
protocol.
**Gate:** roster reconcile items sealed (in flight).
**Premise:** several ledgers disagree with source truth (stale deferred
rows, unflipped census anchors, register count drift, 48 quarantine
exclusions). Wave-12 head certification requires a measured, truthful
queue.

### C.8.1 Current evidence (the truth gap, verified 2026-09-19)

| Ledger | Defect |
|---|---|
| `DECISION_REGISTER.md` | DEC-01/DEC-16 still DEFERRED though D1/D2 resolved+implemented; DEC-05 evidence count drift (14/6); no rows for the resolved D5–D10 packet items |
| `INTEGRATION_PLANS.md` | restock row still "deferred with authority question" |
| `UNCLAIMED_CORPUS_CENSUS.md` | anchors C2[9]–C2[13] not yet flipped (sealing evidence exists — companion program §A.8) |
| `Ashfall.Core.Tests.csproj` | 48 active `Compile Remove` exclusions |
| `AGENTS.md` + clients | fixed 2026-09-19 (queue handoff regenerated; sync green; integrity tests 2/2) |

### C.8.2 Design

One bounded pass after the companion program's reconcile items land: rerun
the census refresh (counts, dependency graph, claims check, head
premise-check) per the Wave 11 Part 2 C2 protocol; verify every flipped row
cites its sealing evidence; reconcile the register with the resolved packet
items; certify the next wave head. This is the companion program's own exit
criterion made explicit (its G.5) — EN-08 owns the protocol, the companion
plans supply the truth.

### C.8.3 Phases & tests

P0 wait on companion Plans 02/06 → P1 register reconciliation (owner-routed)
→ P2 census rerank with evidence citations → P3 head certification + queue
publication → P4 quarantine drain tranche (only if D21 signs). Tests:
docs-index `--check`; claims verifier; census counts agree across ledgers.

### C.8.4 Rerank protocol — step table (per Wave 11 Part 2 C2)

| Step | Action | Output |
|---|---|---|
| 1 | Re-count corpus rows at then-current HEAD | counts table (SEALED / SEALED-ELSEWHERE / RECONCILED-DUPLICATE / AUDIT-PENDING / READY) |
| 2 | Rebuild the dependency DAG from corpus headers | edge list + unresolved-prereq list |
| 3 | Claims check: every ACTIVE claim vs its paths | overlap report (must be empty) |
| 4 | Head premise-check for the top candidate | written verdict (READY / NEEDS-RESCOPE / STILL-BLOCKED) |
| 5 | Verify every flipped row cites sealing evidence | row-by-row citation audit |
| 6 | Register reconciliation: DEC-01/16/05 + resolved D-items | register diff |
| 7 | Certify the next wave head and publish the queue | queue publication + ledger rows |

### C.8.5 Acceptance / definition of done

| # | Outcome | Proof |
|---|---|---|
| 1 | All four ledgers agree on the sealed state | cross-ledger diff empty |
| 2 | Census counts reproducible | two independent counts equal |
| 3 | Next wave head certified with a premise verdict | verdict document |
| 4 | No row sealed without evidence | citation audit |

**Handoff — MUST PRESERVE:** census protocol; owner routing.
**MUST ADD:** one rerank + one reconciliation pass. **MUST NOT DO:** seal
rows without evidence; flip ledgers outside the owning claim.
**VERIFY WITH:** §C.8.3. **FIRST SAFE STEP:** the §C.8.1 table is already
the P0 artifact (produced by the 2026-09-19 audit).

---

## C.9 Common failure playbook (applies to every proposal)

Every EN proposal is exposed to the same small set of failure modes; the
playbook is written once here and referenced by each plan's risk table:

| Failure | Early signal | Containment | Recovery |
|---|---|---|---|
| Premise drift (the design assumed a seam that moved) | P0 probe disagrees with §A.1 | stop at the phase gate; record the correction | amended scope line (B.4) or proposal retirement |
| Parallel-authority creep (a second ledger/registry/store appears) | review sees new state where the design said "none" | delete the state; re-route through the owner | revert the commit; re-run the owner's suite |
| Presentation recomputation (panel doing Core math) | arithmetic in a `.cs` under `src/UI/` | move the math to the read model; render only | parity/estimator test catches it in CI thereafter |
| Determinism break | same-seed runs diverge | seed audit; ordered iteration; invariant culture | fix at the source; add the paired-seed test |
| Save-shape drift | new field without legacy default | additive + neutral default + round-trip test | revert or ship a frozen-shape migration |
| Exactly-once violation | duplicate journal/shock/arc after reload | fired-key ledger; dedupe keys | idempotency test; fingerprint equality |
| Green-widening (relaxing a gate to pass) | allowlist/ratchet edits in the same PR as a feature | never widen; fix the offender | gate's failure fixture proves it still fails |
| Stealth integration (EN work riding another package) | unclaimed EN-named files in a companion commit | split the commit; queue the proposal | Part F line first, always |

The playbook is not advisory: each proposal's phases reference it by row
name where relevant, and package closeouts must state that none of the
eight rows fired (or record the ones that did and their containment).

---

# Part D — XP Pillar Re-Scoping (XP-02 … XP-10 against current truth)

## D.0 XP wave-log template (records every re-scope verdict)

```
<date> XP-<nn> re-scope verdict: <PREMISE-CONSUMED | VERIFY-AND-RECORD |
  SEQUENCED | RETIRED-ADJACENT>
  authored files: <file map status>
  current truth: <file:line evidence>
  disposition: <folded into EN-<nn> item <k> | closed by premise
  correction | blocked on <gate> | foreman reversal required>
  verify: <command + result>
```

Every verdict row is added to `docs/plans/xp/w1/` (or the owning wave's
log) by the claim owner — the pillar map in this document cites them.

## D.1 XP-02 → re-scope: "Survey & Route Knowledge Read Model"

The pillar's files (`GraphTravelPlanner`, `RouteTopologyModel`,
`SurveyKnowledgeStore`) were never built — and must never be built as
authored, because the canonical owner now provides planning
(`WastelandMapSystem.PlanRoute`) and survey knowledge (`DiscoverSurvey`
with traits, per-node knowledge state). The legitimate remainder is
**presentation and read-model**, not a second planner:

| Pillar intent | Current truth | Re-scoped work |
|---|---|---|
| plan routes on the graph | `PlanRoute` live; expeditions/caravans/trade network consume it | none (EN-02 renders it) |
| route topology model | graph + fog + traits owned by `WastelandMapSystem` | none |
| survey knowledge store | per-node survey traits + provenance owned by the map | a read model only if a consumer appears (EN-02's uncertainty band is that consumer) |

**Seam map for the re-scope:**

| Seam | Owner | Re-scope consumer |
|---|---|---|
| `PlanRoute(fromId, toId)` | `WastelandMapSystem.cs:581` | EN-02 read model |
| `DiscoverSurvey(node, source, day, traits)` | `:258` | survey band text (authored traits) |
| fog state / knowledge state | per-node (`:715` domain comment) | "unverified" classification |
| caravan hop expansion | sealed 2026-09-19 | transit-day estimates in the route view |

**Phases:** P0 fold-into-EN-02 note recorded in the XP wave log → P1..P3
become EN-02's phases (no separate package). **Tests:** EN-02's battery.
**Verdict:** premise-consumed; fold into EN-02. Any further XP-02 scope
requires a fresh premise check proving a gap, per the standing rule.

## D.2 XP-03 → re-scope: "War Economy & Chronicle Routes"

Clock: shipped (`ToAuthoredDay`, `FactionWarClockTests` 1/1).
Radio/journal/sound-ranging routes: shipped through
`WireFactionWarConsequenceRouting()`. The distinct remainders — economy
shock routing and chronicle rows — are folded into **EN-01** (design
§C.1.2 items 5–6). The `WorldClockHorizon` severity concept is EN-01's
band projection (§C.1.9).

| XP-03 authored file | Current truth | Disposition |
|---|---|---|
| `WorldClockHorizon.cs` | severity horizon concept | EN-01 band projection (pure Core function) |
| `WarChainEconomyRoute.cs` | not built | EN-01 route item 5 (clash → bounded shock) |
| `WarChainExpeditionRoute.cs` | partially covered | expedition danger already consumes `HostileEncounterMult`; verify-and-record |
| `WarChainRadioRoute.cs` | shipped | none |
| `WarChainChronicleRoute.cs` | not built | EN-01 route item 6 (chain resolution → chronicle row) |

**Verdict:** premise-consumed; remainder owned by EN-01. Phases and tests
are EN-01's; no separate claim is opened.

## D.3 XP-05 → verify-and-record

SOFC fuel is live with the quality-tier resolver and grid fallback
(`ConsumeSofcFuel`, `Main.Plans122to125.cs`); W1 already recorded the
premise correction. Remaining work: none. Any fuel rationing queue
(`FuelRationingQueue`) or fuel-grade catalog is **new scope** needing its
own authorization — it is not XP-05 debt. A future proposal must premise
check the existing `sofc_power_catalog.json` fuel-quality profiles first
and extend that owner rather than adding a parallel grades catalog.

**Verdict:** closed by premise correction; record-only.

## D.4 XP-07 → sequenced follow-on: per-instance provenance

`ItemProvenanceModel` / `NamedItemsCatalog` / `LoreFragmentRevealRules` /
`ProvenanceInteractions` remain unbuilt. Dependencies: EN-03's pressure
tiers (which need Plan 14) and EN-02's survey state (available). The
pillar stays sequenced after F13; when built it feeds the EN-03 tier
function by design (additive input).

| Seam the pillar must extend | Owner today | Provenance hook |
|---|---|---|
| instance identity | `WornGear`/inventory instance records | per-instance `provenanceId` additive field |
| named items | `items.json` catalog | `named_items.json` overlay with strict reference keys |
| reveal | journal/lore owners | reveal rules consume existing knowledge keys |
| faction recognition | EN-03 tier function | `looted` signal as additive input |

**Phases (after F13 + EN-03 P1):** P0 premise check on instance identity
→ P1 provenance model + named-items catalog + validator → P2 reveal rules
through the journal owner → P3 EN-03 recognition feed → P4 round-trip +
parity. **Tests:** inventory suite + new provenance tests + EN-03
composition test. **Verdict:** blocked on sequencing, design pre-aligned
with EN-03.

## D.5 XP-08 → sequenced follow-on: trade routes & seasonal migration

`TradeRouteContractStore`, `RouteReliabilityLedger`,
`SeasonalMigrationScheduler`, `MigrationConsequenceApplier` remain
unbuilt. Hard dependencies: XP-04 funds (F13) and the sealed Plan 07
planner. **D20 interaction (mandatory):** the pillar's caravan-route DTOs
sit in Plan 192 HOLD territory and its human-migration scope sits in Plan
199 HOLD territory — the fifteen program's claim that XP-08 "closes Plans
192/199 intents without NPC agents" is a design hope, not a signature.
The D20 lift/hold lines are the entry gate for any XP-08 claim.

| Component | Extends | D20 territory |
|---|---|---|
| `TradeRouteContractStore` | caravan trade-route catalog owners | 192 (player-route DTO) — needs the lift or a non-player-route scope |
| `RouteReliabilityLedger` | caravan transit-day overlay (sealed) | none |
| `SeasonalMigrationScheduler` | calendar/commitment owners | 199 (human population owner) — needs the lift or fauna-only scope |
| `MigrationConsequenceApplier` | consequence routing template | none |

**Verdict:** blocked on F13 + D20; premise check must precede any bind.

## D.6 XP-09 / XP-10 → premise-check-first, retired-adjacent

- XP-09 (presenter skills): DEC-17 RETIRED — "radio station production
  operates through equipment and program production, not individual
  character RPG skill trees." A revival is a reversal decision. The
  premise check must produce fresh evidence that a *new* design (not the
  retired skill tree) is wanted — e.g. an equipment/program-quality
  projection that does not fork progression. Foreman-only.
- XP-10 (phobia growth): DEC-18 RETIRED — superseded by trauma, guilt
  insomnia, and morale contagion. Same rule: reversal is foreman-only; the
  existing psychological systems are the authorities any new design must
  extend.
- EN-05's XP-09 hook is optional by design and ships without it.

**Premise-check protocol for both (read-only, cheap, today):**
1. Re-read the retired register rows and their evidence pointers.
2. Grep the live systems for the retired concept's vocabulary (skill
trees / phobia ledgers) — zero live consumers expected.
3. If a *new* design is desired, write it as a fresh proposal that extends
   the live owners (`RadioProgramProductionSystem` equipment/program
   quality; psychological trauma owners) and present it for signature —
   never as a revival of the retired rows.
4. Record the verdict in the XP wave log either way.

---

# Part E — Sequencing

## E.1 DAG (proposals + companion program)

```
companion P05 (XP-01 binding) ──→ EN-01 ──┐
companion P06 (E1) ──────────────→ EN-07 ──┤
companion P04 (bootstrap) ──────→ EN-06 ──┼──→ EN-08 (rerank + head cert)
companion P01 (P1 seal) ────────→ EN-05 ──┤
(sealed Plan 07) ───────────────→ EN-02 ──┘
F13 (Plan 14 design) ──→ XP-04 legs ──→ EN-03 ──→ XP-07
F14 (Plan 15 schema) ──→ XP-06 body ──→ EN-04
F13 + D20 ──→ XP-08
D11/D21/… (Part F) ──→ Part D decision-gated items
```

## E.2 Recommended authorization order (maximum value per signature)

1. **EN-02** — gate already satisfied; one line makes the living map real.
2. **EN-06** — after companion Plan 04 lands; cheap standing invariant.
3. **EN-01** — after companion Plan 05; completes the active XP batch's
   world-drama half.
4. **EN-07** — after companion Plan 06; player-facing chronicle.
5. **EN-08** — closes the program; certifies the next wave head.
6. **EN-05** — after companion Plan 01; radio continuity.
7. **F13 → EN-03 → XP-07** and **F14 → EN-04** when the foreman chooses.

## E.4 Effort & gate inventory

| Proposal | Phase count | New save state | New catalogs | Standing gates added |
|---|---:|---|---|---|
| EN-01 | 5 | none (idempotent rides) | none | monotonicity soak gate |
| EN-02 | 4 | none | none | estimator parity gate |
| EN-03 | 5 (post-F13) | none (pure) | `economy_pressure_tiers.json` | pressure soak gate |
| EN-04 | 4 (post-F14) | none | none | slate coverage gate |
| EN-05 | 4–5 | none | none | replay arc assertions |
| EN-06 | 4 | none | none | composite lifecycle + ratchet gates |
| EN-07 | 4 | none (reads v2 store) | none | metrics drift trigger |
| EN-08 | 5 | none | none | rerank protocol gates |

Notable invariant: **no EN proposal adds a save section or migrates a save
shape.** Every new behavior rides existing persisted state through pure
Core composition — the program's defining architectural constraint.

## E.5 Claim-row template (per authorized proposal)

```
| claim-en-<nn>-<short-name>-<date> | EN-<nn>-<NAME> | <builder role> |
  **Core:** <exact Core paths> · **Host:** <exact src paths> ·
  **Data:** <exact JSON paths> · **Tests:** <exact test paths> ·
  **Docs:** <log path> · **Governance:** WORKTREE_OWNERSHIP.md |
  authorization: <foreman line verbatim> · P0 evidence: <notes path> ·
  Phase gates: <per-phase results> |
```

## E.8 Per-proposal verification recipe (the one-loop rule)

For every authorized proposal, every phase, in order:

```
1. bash scripts/run_test.sh <the proposal's focused files>     # 180s cap
2. bash scripts/run_test.sh <the owning regional suite>         # only the named region
3. dotnet build Ashfall.csproj                                  # 0 errors
4. godot --headless --path . -- <named selftests>               # only the touched runtime paths
5. record all four results in the package log                    # before proceeding
```

Additions by kind: stateful phases append round-trip + legacy-parity +
continuous-vs-mid-reload tests; soak phases (EN-01, EN-03) run twice and
assert identical fingerprints; panel phases append lifecycle ×100, a11y,
and player-panels; gate phases (EN-06) prove the failure fixture fails.

The recipe is closed under the program: no phase may invent a broader run
than TEST_POLICY.md allows, and a compile-green result never substitutes
for steps 2–4.

## E.7 Concurrency & review rules

- At most three concurrent authorized EN packages with disjoint ownership.
- EN packages never edit shared roots in parallel with companion-program
  packages that touch the same files (`Main.YearOfAsh.cs`, panel base,
  `ExpeditionHostSession.cs` are the known overlaps).
- Each package review includes: the authorization line (verbatim), the P0
  verdict table, per-phase gate results, and the C.9 playbook statement.
- A proposal whose gate regresses mid-execution (e.g. EN-06's ratchet
  breaks because another package deferred a seam) pauses and reconciles
  with that package's owner — gates are never traded against each other.

## E.6 Timeline sketch (no dates promised — gate-ordered)

1. Companion program waves D1–D4 land (the 8 unblocked plans).
2. S1 (EN-02) can be signed at any time — its inputs are sealed today.
3. S2 (EN-06) the moment companion Plan 04 closes.
4. S3 (EN-01) after companion Plan 05 closes the XP W1 batch.
5. S4/S5 (EN-07/EN-08) close the governance loop and certify the next head.
6. S7/S8/S9 unlock the decision-gated chains when the foreman chooses.

## E.3 Ledger routing

Every authorized EN proposal becomes a package row in `INTEGRATION_PLANS.md`
(claimed in `WORKTREE_OWNERSHIP.md`) — the ledger edits are owner-routed
through `claim-wave11-part2-execution-2026-09-18` or its successor claim,
exactly as in the companion program.

---

# Part F — Consolidated Signature Queue

| # | Line to sign | Unlocks | Blast radius if authorized |
|---|---|---|---|
| S1 | `EN-02 The Living Map: authorized [scope amendments or none]` | route-planning panel package | 1 read model + 1 panel + registry rows; no data/save |
| S2 | `EN-06 One Bootstrap Path composite gate: authorized` | composite invariant (after companion P04) | 1 selftest + 1 ratchet test + manifest rows |
| S3 | `EN-01 Difficulty-Consequence Weave: authorized [band projection shape]` | war-economy/chronicle routes + crisis deadline consumer (after companion P05) | band projection (Core) + 2 routes + deadline seam + soak gate |
| S4 | `EN-07 Chronicle & Aspiration: authorized` | LEDGER strip + E1 readiness slice (after companion P06) | 1 read model + 1 panel strip + metrics wiring |
| S5 | `EN-08 Ledger Truth Program: authorized` | census rerank + head certification | ledgers only (owner-routed) |
| S6 | `EN-05 Signal Continuity & Voice: authorized [with/without XP-09 hook]` | rescued-arc projection (after companion P01) | 1 projection + journal integration + replay extension |
| S7 | `F13 XP-04 economy legs design: [signed shape]` | EN-03 + XP-07 chain | per the XP Part-2 file map (largest in this queue) |
| S8 | `F14 XP-06 body-integrity schema: [signed shape]` | EN-04 chain | schema + every 2-handed/boot row default (second-largest) |
| S9 | `D20 174/175/192/199: [lift/hold ×4]` | XP-08 and endgame HOLD rows | per C3 handoff conditions |
| S10 | carried: D11, D21, D3, D4, D13, D16, D19a–c, D22, F16 | per the 2026-09-18 packet | per packet |

Every signature stays optional and independent; nothing in the companion
program waits on any of them.

---

# Part G — Verification, Rollback, Out of Scope, Handoff

## G.1 Verification pattern (per proposal)

Identical to the companion program G.1: focused suite green, owning
regional suite green, build 0/0, matching headless selftests, round-trip +
parity + continuous-vs-mid-reload where stateful (EN-01's soak, EN-03's
20-day pressure soak), and soak harnesses for the composite proposals.

## G.2 Rollback

Every proposal lands as small reversible commits (read model / pure module
first, host second, panel third). No proposal migrates a save shape; revert
= code revert. EN-01's routes carry idempotency keys whose removal blocks
the revert review.

## G.3 Out of scope for the whole program

- Reversing DEC-06, DEC-17, DEC-18, DEC-20, or the C3 HOLDs without the
  named signature.
- New parallel authorities (planners, ledgers, recovery timers, pressure
  stores, prestige systems) regardless of convenience.
- Census tranche-2 audits as stealth integration.
- Full-suite runs by default.
- Any production edit by this document itself.

## G.4 Final handoff

**MUST PRESERVE:** every owner named in Part C/D; the sealed seams in §A.1;
signed and retired decisions; deterministic contracts; frozen save shapes.
**MUST ADD:** only what an authorized proposal's design names, behind its
gate, in phase order.
**MUST NOT DO:** build an EN proposal before its authorization line; fork
a canonical owner; recompute Core outcomes in panels.
**VERIFY WITH:** each proposal's test block; EN-08's rerank as the closing
artifact.
**FIRST SAFE STEP:** EN-06 P2 (the two lowercase-path stragglers —
read-only, today) or any Part C premise-check block.

## G.6 Long-horizon view (what this program sets up)

Once the EN layer and the companion program land, the repo reaches a
specific, verifiable state: every host seam bound (port contract ratchet at
zero), one data path, one lifecycle path, a difficulty-tagged chronicle, an
evidence-governed ambition queue, and a truthful decision register. From
that state, the natural long-horizon successors are:

1. **Census tranche-2 execution** — the 111 AUDIT-PENDING rows become
   ranked, auditable work through E1's queue, each entering through the
   premise-audit protocol (companion Part B.3).
2. **The decision-gated chains** (XP-04→EN-03→XP-07, XP-06→EN-04,
   XP-08 after D20) — each becomes a normal package once its signature
   lands; the designs here pre-align their seams.
3. **Release practice** (companion Plan 08) — the ship gate hardens into
   the tagged-release path with artifact provenance.
4. **Successor waves** inherit EN-06's composite invariant as a standing
   gate, so host wiring completeness stops being an audit topic.

The program deliberately does not schedule these — it guarantees the rails
they need.

## G.7 Review cadence

| Cadence | Review |
|---|---|
| per phase gate | package log updated; focused results recorded |
| per package close | closeout row + evidence; ledger flip (owner-routed) |
| per authorization | Part F row verdict recorded verbatim |
| per wave | EN-08-style mini-rerank before the next head is certified |
| program exit | G.5 criteria; successor handoff documented |

## G.5 Exit criteria

The program exits when every authorized proposal is sealed with evidence,
the unauthorized ones remain explicitly queued with their gates named
(Part F), and EN-08's rerank certifies the next wave head from a truthful
queue. The 2026-09-19 audit + the companion program + this document then
form one continuous, evidence-cited chain from "what was blocked" to "what
shipped" to "what is proposed next".

---

# Part H — Evidence Index

| Claim | Evidence |
|---|---|
| Difficulty catalog 4×8 scalars; one live consumer; default-only resolve | `Assets/StreamingAssets/Data/difficulty_presets.json`; `src/Main.EvolvingWorld.cs:193`; `src/Main.Difficulty.cs:28` |
| War clock + routing template | `FactionWarClockTests` 1/1; `WireFactionWarConsequenceRouting()` (`src/Main.YearOfAsh.cs`) |
| Graph travel / fog / surveys | `Assets/Ashfall.Core/World/WastelandMapSystem.cs:258,581,715`; `WildlifeMapOverlayTests` 1/1 |
| Completion history v2 difficulty tag | `Assets/Ashfall.Core/Endgame/CampaignCompletionHistory.cs:44`; tests 11/11 |
| Port contract 0 deferred; single catalog path | `generate-port-contract.py --check`; `CatalogPathForbiddenGateTests` 2/2 |
| Bootstrap restore-path-only | `src/Main.Lifecycle.cs:542`; `src/Main.SaveOrchestrator.cs:163` |
| Black-market settlement/heat; restock priority | settlement 18/18; `ShelterBarterSystem.cs:283,307-316`; restock tests 6/6 |
| Ward staffing gate | `MedicalWardSystem.StaffingPreflight`; `MedicalWardSystemTests` 14/14 |
| Distress ledgers + content | scheduler/resolver tests 19+10; catalogs 17+7 follow-ups, 66+36 cues |
| Crisis predictor + briefing integration | `DailyBriefingCrisisTests` 8/8; `CrisisPredictionTests` 16/16 |
| XP pillar files absent | file-map grep (all 27 pillar files MISSING; XP-01's exist) |
| Retired rows DEC-17/DEC-18; DEC-20 boundary; C3 HOLDs | `docs/governance/DECISION_REGISTER.md` |
| Queue baseline (8 available; 117→112; ten seals) | `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` |
| Companion program | `Seal-steps/ashfall-eight-unblocked-plans-completion-first-execution-program-2026-09-19.md` |

Reproducible verification commands (the probes behind §A.1 — re-run at any
claim time, all read-only):

```
bash scripts/run_test.sh Ashfall.Core.Tests/FactionWarClockTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Endgame/CampaignCompletionHistoryTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/World/WildlifeMapOverlayTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/CatalogPathForbiddenGateTests.cs
python3 scripts/ci/generate-port-contract.py --check
grep -n "ExecuteSubsystemManifestBootstrap" src/Main.SaveOrchestrator.cs
grep -n "HostileEncounterMult" src/Main.EvolvingWorld.cs
grep -n "difficultyPresetId" Assets/Ashfall.Core/Endgame/CampaignCompletionHistory.cs
```

Baseline results (2026-09-19): 1/1, 11/11, 1/1, 2/2, PASS (262 seams /
180 HOST_REQUIRED / 0 deferred), one call site (`:163`), one consumer
(`:193`), field at `:44`. Any drift from these baselines is a premise
change — record it before building on the seam.

---

# Part I — Appendices

## I.1 Worked example: EN-02 end-to-end (the reference implementation shape)

Every authorized proposal follows this same shape; EN-02 is worked here in
full because its inputs are sealed today.

**User story.** A player opens the map surface, selects a discovered
destination, and sees an honest candidate route: legs with condition
classes, survey uncertainty bands, known closures, war-front warnings, and
cost estimates — then dispatches through the existing commands.

**Seam walk (what the builder verifies at P0):**

1. `WastelandMapSystem.PlanRoute(fromId, toId)` returns the node path —
   the only planner (verified `Assets/Ashfall.Core/World/WastelandMapSystem.cs:581`).
2. Per-node fog/knowledge state classifies legs as verified / surveyed /
   unverified (domain comment `:715`; `DiscoverSurvey` `:258` authors
   traits).
3. Edge conditions and closures come from the map catalog and the sealed
   orphan-gate discipline (`WastelandMapCatalogLoaderTests`, 5/5).
4. War-front flags derive from the chain-state read model
   (`FactionWarMapWidget` precedent refreshes on standing change).
5. Cost estimates come from the existing owners: the expedition estimate
   (already consuming map distance and danger bands) and caravan transit
   days (sealed overlay).

**Build order.** Core read model (pure) → host bind → panel + registration
→ estimator parity gate. Every phase lands as one reviewable commit with
its focused tests; the panel never issues a new command.

**Definition of done (observable).** With a real campaign and a discovered
destination, the route view renders truthfully, dispatch works through it,
and the parity gate proves the estimates equal the dispatch advisories —
verified headlessly through the panel lifecycle battery.

**Rollback.** Delete the panel + read model; revert the registration rows.
No data, save, or catalog was touched.

## I.2 Consolidated test matrix (program-level)

| Proposal | New focused files | Existing suites gated | Headless selftests | Soak/parity |
|---|---|---|---|---|
| EN-01 | band projection, routes, monotonicity | Crisis 16/16, Economy, Journal | — (Core-only) | 30/60/90-day monotonicity + standard fingerprint |
| EN-02 | read model, estimator parity | WastelandMap 68/68, PanelRoute | panel lifecycle, a11y, player-panels | estimator parity |
| EN-03 | rules, tier catalog, pressure soak | Economy 184+ | — | 20-day continuous==mid-reload |
| EN-04 | slate, staffing interaction | Medical 371+ | — | absence composition |
| EN-05 | projection, arc replay | Radio 323+ | content-utilization, audio | replay fingerprint stability |
| EN-06 | composite lifecycle, ratchet | Tooling | composite selftest (new) | fresh==restore setup set |
| EN-07 | strip contract, metrics | Endgame 84+ | endings selftest | empty-history honesty |
| EN-08 | rerank checks | Tooling | docs-index, claims verifier | two independent counts equal |

Baseline counts are the sealed values from the 2026-09-19 audit; each
package re-measures its own baselines at claim time.

## I.3 Governance interaction (how this program touches the ledgers)

| Touchpoint | Rule |
|---|---|
| `INTEGRATION_PLANS.md` | an authorized EN becomes a package row; owner-routed edits only |
| `WORKTREE_OWNERSHIP.md` | claim row before the first edit; shared roots are sole-active-builder |
| `KNOWN_DEBT.md` | no new debt rows from proposals themselves; execution may retire rows with evidence |
| `DECISION_REGISTER.md` | EN authorizations are recorded as rows; retired rows are never overwritten |
| Census | EN-08's rerank is the only sanctioned census mutation path |
| `docs/INDEX.md` | regenerated by the integrator at wave close; never hand-edited |

## I.4 Glossary

- **Band projection** — pure Core function mapping an authored severity to
  a difficulty-scaled severity within the authored envelope (EN-01).
- **Companion program** — the signature-free execution program for the 8
  unblocked plans (same date, same directory).
- **Fired-key ledger** — exactly-once event record keyed by a stable event
  identity (idempotency across save/restore).
- **Gate** — a named condition that must hold before the next phase; every
  phase in this document has one.
- **Owner-routed** — an edit to a ledger/file claimed by an active claim,
  executed by that claim's owner or under an explicit user-authorized
  transfer.
- **Premise check** — read-only re-verification of a design assumption
  against current source before any edit (Rule 7).
- **Premise-consumed** — a proposal whose stated gap no longer exists
  because the capability shipped through canonical owners.
- **Ratchet** — a gate that only allows a metric to improve (e.g. DEFERRED
  count stays zero).
- **Read model** — pure composition over persisted owner state exposing a
  presentation DTO; owns nothing, saves nothing.
- **Retired-adjacent** — a proposal whose concept was formally retired
  (DEC-17/DEC-18); any revival is a foreman reversal decision.

## I.5 Frequently misread rules (anti-pattern table)

| Anti-pattern | Correct reading | Where it bites |
|---|---|---|
| "The gate passed, so it's integrated" | A gate proves its named invariant only; integration = owner + route + save + observable outcome agree | EN-06 composite gate; all soak gates |
| "Read model can cache" | Read models re-query on bind; caching creates a parallel state | EN-02, EN-04, EN-07 |
| "One more scalar consumer is trivial" | Every consumer needs its premise note + parity guard (XP-05 precedent) | EN-01, companion Plan 05 |
| "The sketch said it, so it's authorized" | Sketches and this document are proposals; only a Part F line authorizes | every EN |
| "Fold XP-03 into anything convenient" | The remainder has one owner: EN-01 | EN-01 route items 5–6 |
| "Rerank the census while executing" | EN-08 owns the rerank protocol; builders cite, owners flip | all ledger touches |
| "Difficulty belongs in the panel" | Scaling happens at Core event sites; panels render results | EN-01 band projection |
| "A soak run can replace a unit test" | Soaks prove bounds and monotonicity; units prove causes | EN-01, EN-03 |

## I.6 Skill cross-reference (which repo skill supports which phase)

| Phase | Skill | Use |
|---|---|---|
| P0 premise notes / audits | `ashfall-analyze` | read-only forensic pass producing the verdict table |
| Proposal development | `ashfall-plan` | dependency-ordered, evidence-grounded planning (this document's genre) |
| Execution | `ashfall-implement` | conservative phase-by-phase integration with mandatory tests |
| Bug discovery mid-phase | `ashfall-repair`, `ashfall-problem-identifier` | validate → root cause → minimal repair plan |
| Balance claims | `ashfall-balance-sim` | seeded headless sweeps (EN-01 monotonicity, EN-03 pressure soak) |
| Save safety | `ashfall-save-fuzz`, `ashfall-determinism-guard` | round-trip/checksum/seed-parity evidence (all stateful phases) |
| UI phases | `ashfall-ui-access`, `ashfall-snapshot-diff` | contrast/keyboard/overflow audit + golden-panel regression |
| Release craft | `ashfall-release-captain`, `ashfall-export-build` | companion Plan 08's gates |
| Queue truth | this document + the 2026-09-19 audit | EN-08's rerank inputs |

## I.7 Program metrics (review cadence)

| Metric | Healthy value | Drift trigger |
|---|---|---|
| Authorized proposals sealed with evidence | all signed | an authorized proposal stalls two review cycles |
| Decision queue (Part F) | shrinking or explicitly re-queued | a signature lapses without a verdict |
| Census nonterminal count | monotone non-increasing between reranks | any increase without a new proposal |
| Save sections added by this program | 0 | any > 0 is an architecture breach (E.4 invariant) |
| DEFERRED port seams | 0 (ratchet) | any > 0 fails EN-06's gate |
| Ledger disagreements (EN-08 §C.8.1 table) | 0 after the rerank | any new disagreement |

## I.8 Worked example: EN-01's crisis-deadline consumer (the smallest full loop)

This is the reference shape for a scalar consumer landing — the same loop
the companion program's Plan 05 repeats for six scalars and EN-01
repeats for the deadline:

1. **Premise note.** `CrisisPredictionModel`'s deadline input is read; the
   calculation site is named with file:line; the authored scalar is
   `crisis_deadline_mult` (sparing 1.25 = more runway, dirge 0.65 = less).
2. **Seam design.** The multiplier is applied where the runway is
   *computed* (Core, event site), never where it is *rendered* (briefing
   builder). The provider is passed in, never singleton-fetched, so tests
   can pin Legacy parity.
3. **Parity guard.** A test computes the deadline with
   `DifficultyScalarsProvider.Legacy` and asserts the exact pre-EN-01
   value; a second test pins each preset's expected runway (1.25×, 1.0×,
   0.8×, 0.65× of the authored base, integer-day rounding rules stated).
4. **Observable outcome.** The daily briefing's crisis warning shows the
   adjusted date; a campaign-day journey test drives a seeded crisis under
   two presets and asserts the dirge warning surfaces earlier.
5. **Failure mode.** If the briefing ever recomputes the date, the parity
   test catches the divergence (the briefing asserts it renders the
   provided value, not its own arithmetic).
6. **Rollback.** Revert the consumer commit; the scalar stays authored and
   dormant; no data migration anywhere.

The loop is deliberately boring — that is the point: every consumer in
this program, EN or companion, is the same six steps with the same guards.

## I.9 Document self-check (how this document stays honest)

| Check | Result at publication |
|---|---|
| Every proposal names its gate and authorization line | Part C ×8 + Part F ×10 |
| Every design extends a named owner (no parallel authorities) | §A.1 seam inventory + per-proposal ownership tables |
| No save migration promised anywhere | E.4 invariant row; G.2 rollback |
| No retired decision reversed | A.4 corrections; D.6 protocol; G.3 |
| Every phase has a gate | per-proposal phase tables + C.9 playbook |
| Evidence citations are file:line or command-result | Part H + inline |
| Production changes by this document | none (docs-only, two new files total across both programs) |

---

*End of program document. Planning authority only: register in
`docs/INDEX.md` when the integrator next regenerates the index (the index
and the live ledgers have in-flight edits as of 2026-09-19).*

**Document provenance:** authored 2026-09-19 by the same read-only audit
session that produced `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` and
the companion execution program; all citations were verified against the
worktree at HEAD `fc73a306` plus the uncommitted 2026-09-18/19 session; no
claim, ledger, code, data, or test file was modified to produce it. The
three documents form one chain: audit (what is true) → companion program
(what is executable now, signature-free) → this program (what is proposed
next, one signature each). Chain-integrity rule: the audit's §3 frontier,
the companion program's Part B roster, and this document's §0 readiness
table name the same eight packages with the same gates — any future edit
to one of the three documents must reconcile the other two in the same
change, or the chain is broken and the queue loses its evidence basis.
