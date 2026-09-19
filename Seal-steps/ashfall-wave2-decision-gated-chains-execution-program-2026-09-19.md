# ASHFALL — Wave 2, Program A: Decision-Gated Chains Execution Program
## The F13/F14/D20 tier, the EN-03/04/05 composites, and the carried one-liners — execution-grade plans, one signature each

| Field | Value |
|---|---|
| Program ID | `W2A-DECISION-GATED-CHAINS` |
| Authored | 2026-09-19 (Wave 2, follow-on to the eight-plan execution program) |
| Status | **planning authority — nothing executed, nothing claimed** |
| Package count | 8 gated execution packages (G-01…G-08) + 7 one-liner micro-packages (M-01…M-07) |
| Signature requirement | every package; this document authorizes nothing |
| Production changes made by this document | none |

This is Wave 2 Program A in the four-document chain begun on 2026-09-19:

1. `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` — what is true (8 plans available).
2. `Seal-steps/ashfall-eight-unblocked-plans-completion-first-execution-program-2026-09-19.md` — what is executable now, signature-free (the 8 plans).
3. `Seal-steps/ashfall-enhanced-expansion-program-en-01-en-08-and-xp-pillar-rescoping-2026-09-19.md` — what is proposed next, one line each (EN-01…EN-08 + XP re-scoping).
4. **This document** — what becomes executable the moment a signature lands: the gated tier, written as full execution plans so the interval between decision and build is minutes, not weeks.

The design premise of Wave 2 Program A: **the decision queue is the bottleneck, not the engineering.** Every gated item in this program has finished rails beneath it — sealed owners, proven save paths, live panels, green test suites. What separates each from execution is a single foreman line recorded in the DECISION_REGISTER format. This document pre-builds the packages so that a signature is the *only* remaining input.

## 0 · Program summary card

| Field | Value |
|---|---|
| Gated packages (full plans) | G-01 XP-04 economy legs · G-02 XP-06 body-integrity schema · G-03 EN-03 pressure composite · G-04 EN-04 rehabilitation slate · G-05 EN-05 signal continuity · G-06 XP-08 + D20 endgame lifts · G-07 D21 quarantine drain · G-08 D11 semantic-kind briefing |
| One-liners (micro-packages) | M-01 D3 water sample · M-02 D4 DEC-15 ratification · M-03 D13 Plans 126–129 · M-04 D15 SOFC fuel owner · M-05 D19a pre-campaign barter · M-06 D19b station identity · M-07 D22 string freeze |
| Evidence baseline | HEAD `fc73a306` + the uncommitted 2026-09-18/19 session (ten seals verified by the 2026-09-19 audit) |
| Save sections added | one, G-01's `funds_ledger` (already specified by the fifteen program's Plan 14 and routed through the save-owner protocol); all other packages route through existing owners and existing sections |
| Retired decisions touched | **none** — DEC-06/17/18/20 remain untouched; D20 lift signatures are the register's own protocol, not a reversal |
| Companion dependencies | G-03 waits on G-01; G-04 waits on G-02; G-05 waits on companion Plan 01; G-06 waits on D20; the rest are standalone |

Package readiness at a glance (rails sealed? missing input?):

| Package | Rails sealed today | Missing input | Signature line |
|---|---|---|---|
| G-01 | settlement, heat/trust, restock, DEC-05 precedent | design shape only | S7 (F13) |
| G-02 | amputation, equipment condition, crafting chains | schema shape only | S8 (F14 / DEC-03 successor) |
| G-03 | everything G-01 adds + tier design written | G-01 + S7 | follows S7 |
| G-04 | everything G-02 adds + slate design written | G-02 + S8 | follows S8 |
| G-05 | radio V6, ledgers, replay harness, content | companion Plan 01 + S6 | S6 |
| G-06 | hardcore rail owners live | four product calls | S9 (D20 ×4) |
| G-07 | policy + per-file protocol written | drain authorization | D21 line |
| G-08 | semantic authority sealed; pin amendment drafted | option (i)/(ii) choice | D11 line |
| M-01…M-07 | each is one file or one row | one line each | per package |

Reading paths by role: **foreman** — §0, Part B (what each signature buys), Part F
(the exact lines). **builder** — Part C (your one package, after its
signature), Part E (sequencing). **cheap sweep agent** — any "current
reality" block; all read-only. **integrator** — Part E.3 ledger routing,
Part G.5 exit criteria.

---

## How to read this document

- **Part A** establishes the gated tier's current reality: which owners,
  save paths, and test suites are already live under each gate, and the
  carried decision-packet inventory (D3…D22) with its verified evidence.
- **Part B** defines the signature-to-execution contract: what a line
  authorizes, what it never authorizes, and the amendment protocol.
- **Part C** expands each of G-01…G-08 into a full execution plan: current
  reality (with file:line evidence), required delta, ownership and seams,
  state/save/determinism analysis, failure modes, phased gates, focused
  verify, file impact map, rollback, and handoff.
- **Part D** handles the one-liners M-01…M-07 as bounded micro-packages.
- **Part E** sequences the tier: dependency DAG, recommended signature
  order, ledger routing, concurrency rules, timeline sketch.
- **Part F** consolidates every signature line in one queue.
- **Part G** covers verification, rollback, out-of-scope, handoff, and
  exit criteria.
- **Part H** is the evidence index.
- **Part I** holds appendices: worked specifications, test matrices, the
  failure playbook, and document self-check.

A note on tone: this document is written to be *signed against*. Each Part
F line is the exact text to record; each Part C plan is what that line
unlocks. Nothing here is vague on purpose — where a design choice exists
(options A/B, option i/ii), the options are stated with their recorded
costs and blast radii, and the package executes whichever one the line
names.

---

# Part A — Current Reality: the Gated Tier (verified 2026-09-19)

## A.1 What "gated" means here

The 2026-09-19 audit classified the queue into three tiers:

1. **Available now** (8 plans) — blockers verifiably gone; the companion
   program executes them signature-free.
2. **Decision-gated** (this document) — engineering rails exist or will
   exist through the companion program / EN layer; the only missing input
   is a recorded product or architecture decision.
3. **Premise-consumed or retired** — closed by evidence or formal
   retirement; not listed here except where a carried decision asks about
   them (D13, D20 rows).

Gated is not blocked. Every package below names: the live owner it
extends, the green test suite that guards it, the save section it routes
through, and the single decision that releases it.

## A.2 The rails under each gate (verified)

| Gate | Live owner(s) | Guard suite (green) | Save routing | The one missing input |
|---|---|---|---|---|
| F13 / XP-04 | `BlackMarketSystem` (heat/trust/loans), `BlackMarketSettlementService`, `ShelterBarterSystem` restock (`:283`, `:307-316`) | Black-market settlement 18/18; restock priority 6/6 | campaign state through existing sections; no new section needed | funds-authority + purity/counterfeit design shape |
| F14 / XP-06 | `AmputationSystem` (limb state, `GetMovementSpeedMultiplier`), `EquipmentConditionSystem`, `MedicalWardSystem` (14/14) | amputation travel + protective-wear suites | amputation already persists; equipment rows need a *default*, not a section | ItemDefinition handedness/limb-requirement schema shape (DEC-03 successor / D12 equipment half) |
| S6 / EN-05 | rescue manager, follow-up scheduler, trust ledger, cue resolver (`RadioSave` V5→V6) | Wave-5 replay harness; radio suite | V6 codec frozen shape | companion Plan 01 (P1 seal) + the S6 line |
| S9 / D20 | four named owner surfaces (174: enrichment/trade specialty; 175: profile store; 192: route DTO; 199: population owner) | per-row, named in C3_HANDOFF | per-row, mostly "versioned/checksummed outside campaign slots" (175) | four product calls, each one line |
| D21 | csproj `Compile Remove` list (50 active) | per-file focused targets | none (test sources) | drain authorization + batch size |
| D11 | semantic authority (`DayEventVocabulary`), briefing builder | `DayEventVocabularyTests` (the pin, 8 cases); builder tests (13) | day records already persisted | option (i) keep flat / (ii) re-group + amend pin |

## A.3 The carried decision inventory (D3…D22, verified against the packet)

The 2026-09-18 decision packet carries 23 items. As of the 2026-09-19
audit: D1, D2 resolved by execution (ledger rows not yet flipped);
several are sequencing gates rather than questions; and exactly the
subset below remains genuinely open. Each row's evidence was verified in
source — the packet's own citations were re-checked, and two ledger
drifts were found and recorded in the audit (the 51→50 quarantine count
and the AutopsyProcedures "next candidate" being already RETIRED).

| ID | One-line substance | Type | Recorded recommendation | Package here |
|---|---|---|---|---|
| D3 | `water_sample_contaminated` equipability — three documents disagree | tie-break | memo: Option B; sweep: Option A | M-01 |
| D4 | ratify DEC-15 (weather forecast authority) | policy ratification | retain (already matched) | M-02 |
| D11 | semantic-kind briefing re-grouping | contract decision | **none recorded** (the only one) | G-08 |
| D12 | amputation equipment restriction schema | schema sign-off | promotion condition written | **G-02** (same substance as F14) |
| D13 | Plans 126–129 header/numbering authority | adjudication | two readings on record (a/b/c) | M-03 |
| D15 | SOFC inventory-fuel binding | owner naming | "small wiring package"; seam exists | M-04 |
| D19a | barter constructible pre-campaign? | policy | none | M-05 |
| D19b | radio triangulation station identity | schema | station-selection seam vs synthetic id | M-06 |
| D20 | four C3 endgame HOLDs (174/175/192/199) | product calls ×4 | preconditions written verbatim | G-06 (+XP-08 interplay) |
| D21 | test-quarantine drain | authorization | ledger row itself is stale | G-07 |
| D22 | string freeze (gates VO + localization) | sequencing declaration | recheck triggers written | M-07 |

D12 and F14 name the same work from two directions (the debt ledger's
`DEBT-AMPUTATION-EQUIPMENT-RESTRICTION` and the XP-06 pillar). Wave 2
Program A treats them as one package, G-02, and the signature line
satisfies both records.

## A.4 Standing premise corrections (banked by the prior documents)

These were verified 2026-09-19 and are not re-litigated here:

- `ExecuteSubsystemManifestBootstrap` has exactly one call site
  (`src/Main.SaveOrchestrator.cs:163`, restore path) — the fresh path
  composes directly; companion Plan 04 closes the gap.
- XP-01 difficulty binding is partial: default preset only via
  `ResolveProvider(null)`; one consumer (`HostileEncounterMult`,
  `src/Main.EvolvingWorld.cs:193`); no save section, no panel. Companion
  Plan 05 completes it; EN-01 builds on top.
- XP-02 and XP-03 are premise-consumed (route planning landed through
  `WastelandMapSystem.PlanRoute`; neighborhood pressure landed through
  canonical owners). XP-05 is verify-and-record (the per-scalar parity
  protocol).
- The census holds 131 files: 33 SEALED, 113 AUDIT-PENDING, 2
  READY-UNCLAIMED (anchor count as of the 2026-09-19 read; the five
  2026-09-19 anchor seals flip on the integrator's ledger pass).
- The forbidden-path gate has a known blind spot: two lowercase
  `res://assets/StreamingAssets/Data` references
  (`src/Audio/AudioCueCatalog.cs:407`,
  `src/Main.FlagshipInstitutions.cs:45`) escape the ordinal pattern.

## A.5 Why this tier is worth a dedicated program

The audit's counting rule excluded these items from "available" because
each needs a *new* foreman decision. But the engineering underneath them
is disproportionately ready:

- G-01's market surface is sealed to 18/18 with deterministic restock
  already live — the funds authority is additive to a finished host.
- G-02's runtime is sealed: amputation owns limb state and the movement
  multiplier; the *only* open surface is a data field and an equip
  preflight branch.
- G-05's radio subsystem is the most-tested system in the repo (frozen
  V6 codec, exactly-once ledgers, deterministic replay harness).
- G-07 and G-08 are pure policy executions with drafted protocols.

The interval between "foreman signs" and "builder starts" should be a
file-claim, not a design phase. That is what the rest of this document
buys.

## A.6 Value-per-signature map (what each line actually buys)

The tier's asymmetry is the point: the engineering is ready, so a line's
value is the gameplay it releases, not the design it completes.

| Line | Player-visible result | Engineering released | Lines of code, est. |
|---|---|---|---|
| W2A-1 (D4) | none (governance) | register invariant restored | 0 |
| W2A-2 (D3) | item behaves consistently either way | one row + pin or description | ~5 |
| W2A-3 (F13) | scrip, fences, purity pricing, relocating markets | funds authority + 4 atomic legs + heat | ~900 |
| W2A-4 (F14/D12) | limbs matter for gear; prosthetics exist | schema + projection + catalog + arc | ~800 |
| W2A-5 (D15) | SOFC consumes real fuel | one binding | ~40 |
| W2A-6/7 (EN composites) | market temperature, rehab slates | 2 projections + panel strips | ~250 |
| W2A-8 (EN-05) | rescued contacts keep living | arc projection + replay | ~300 |
| W2A-9 (D21) | test debt visibly shrinks | drain protocol | ~0 (recovered) |
| W2A-10 (D11) | briefings grouped by meaning (or closed) | re-group or close | ~120 (opt. ii) |
| W2A-11 (D20 ×4) | endgame direction begins | 1–3 owners per lift | 150–600/lift |
| one-liners | consistency residue | bounded fixes | ≤50 each |

Read alongside E.2: the two ~900/~800-line packages are the tier's real
mass; everything else is surgical. That is why the program pre-builds
G-01/G-02 to execution grade and treats the rest as protocol.

---

# Part B — The Signature-to-Execution Contract

## B.1 What a signature authorizes

A Part F line, once recorded, authorizes exactly one Part C or Part D
package at the scope written in its plan — nothing wider. Concretely it
authorizes:

- executing the listed phases in order, at their phase gates;
- the file impact map's writes (and no others outside a shared-root
  protocol);
- the focused verification commands as the package's acceptance;
- the ledger flips named in the package's handoff, routed through the
  owning claim.

## B.2 What a signature never authorizes

- save sections without an owner protocol: G-01's `funds_ledger` is the
  one planned new section in this program (the fifteen program's Plan 14
  already specifies its envelope, frozen prior shapes, and neutral
  migration); every other package routes through an existing owner's
  section, and no package may invent a second section for state an owner
  already persists;
- reversals of retired decisions (DEC-06 integer-permille pool,
  DEC-17/18 retires, DEC-20 v2 frozen shape) — D20 lift lines are the
  register's own precondition protocol and explicitly not reversals;
- gate widening — any package that needs an allowlist or ratchet edit
  stops and reports;
- stealth scope: EN- or G- work riding a companion package's commit;
- new registries, ledgers, or modality managers where an owner exists.

## B.3 The line format

Every line follows the DECISION_REGISTER convention (already used by
S1–S10 in the EN program):

```
W2A-<nn> <package>: <choice among the named options>, scope per Part C.<k>
```

Choices with options name the option letter. Owner-naming lines name the
owner. Multi-part lines (D20) name each sub-row. A line that adds scope
not written in Part C is invalid — it becomes an amendment (B.5).

## B.4 What the builder does with a line

1. Record it verbatim in the package log (`docs/plans/w2a/G-<nn>_LOG.md`).
2. File the claim in `WORKTREE_OWNERSHIP.md` naming Part C's file impact
   map.
3. Run the P0 premise probes (each plan's §"Current reality" commands).
4. Execute phases in order; each gate records its focused result.
5. Close out per the handoff: ledger flips (owner-routed), closeout row,
   playbook statement (C.9 reference).

## B.5 Scope amendment protocol

If field evidence contradicts a plan's premise mid-execution, the
protocol is the one proven on 2026-09-19 (the XP-05 parity stop): stop at
the phase gate → record the correction in the package log → propose the
amended scope as a new line → never bend the signed design silently.

## B.6 Exclusions

- Census tranche-2 rows — Wave 2 Program B owns the drain protocol.
- The companion program's eight plans — already fully specified there.
- EN-01/02/06/07/08 — the EN program owns them; only the composites
  EN-03/04/05 (which are gated-tier chains) are re-specified here as
  execution plans, and only their chain heads (G-01, G-02) add content
  beyond the EN program's designs.

## B.7 What a builder may NOT infer from this document

The same five prohibitions as the EN program apply verbatim: no implicit
authorization, no premise proof (evidence is a 2026-09-19 snapshot), no
claim reservation, no precedence over live ledgers, no scope elasticity.

---

# Part C — The Eight Gated Execution Plans

Each plan follows the house genre: current reality (verified, file:line),
required delta, ownership and seams, state/save/determinism, failure
modes, phases with gates, focused verify, file impact map, rollback, and
handoff. Plans reference the C.9 failure playbook of the EN program (the
same eight rows: premise drift, parallel-authority creep, presentation
recomputation, determinism break, save-shape drift, exactly-once
violation, green-widening, stealth integration).

## G-01 — XP-04 Economy Legs: Funds, Fence Purity, Heat, Contracts (`W2A-G01-XP04-ECONOMY-LEGS`)

**Anchor:** XP-04 (the fifteen program's Plan 14) · **Gate:** F13/S7 ·
**Chain head for:** G-03 (EN-03 composite).

### C.G1.1 Current reality (verified)

- Black-market trade actions live with immediate canonical-inventory
  settlement (`BlackMarketSettlementService`, 18/18 tests, DEC-02 owner
  routing); the panel shows explicit due dates and no restock button
  (reopen never rerolls — the R4 discipline).
- Heat/trust/loans live: heat decays daily, trust bounded [0,100], one
  active loan per syndicate, idempotent overdue events with a fired-key
  ledger; bounty escalation delegates to `FactionBountySystem` (no second
  bounty ledger).
- Deterministic restock priority is **sealed**: `ComputeItemPriorityScore`
  (`Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs:283`) with the
  `PriorityScorer` seam (`:136`) consumed by the arrival sort
  (`:307-316`); 6/6 tests. The fifteen program's Plan 03 (companion
  Plan 02 here) added the cross-reference note: XP-04 **references** the
  DEC-05 design, never re-specifies it.
- Missing, and only missing: the scrip authority (`FundsLedger`), the
  four two-leg transactions, purity tiers, escrowed contracts, and the
  heat-relocation trigger — all specified in the fifteen program's
  C.14.1 and awaiting the design sign-off.

### C.G1.2 Required delta (what the S7 line releases)

Exactly the fifteen program's C.14.1, restated as executable items:

1. **`FundsLedger` (Core):** integer-scrip authority, bounded 128-entry
   append-only movement log (MedicalRecordLog rules: typed reasons,
   oldest-evicted, running-balance invariant), typed refusals on
   over-debit.
2. **Four atomic legs on `BlackMarketSystem`:** `bm_buy`, `bm_sell`,
   `bm_fence` (faction-recognized loot in at purity discount — the only
   fence path), `bm_contract` (escrowed deliverable ↔ scrip). Both halves
   commit or neither does.
3. **Heat/attention + relocation:** per-faction-market integer heat,
   raised by legs, decayed daily; heat ≥ authored threshold relocates the
   market (deterministic new contact, discovered anew through the
   existing idempotent contact-discovery gate). Additive fields in the
   existing black-market save section.
4. **Purity tiers:** `sealed/clean/suspect/cut` scrip-adjacent item
   states; `bm_fence` prices by purity; detection consumers are named
   follow-ons, not built here.
5. **Restock bind:** DEC-05 design verbatim (the sealed scorer), plus the
   day-gated NEXT SUPPLY visibility strip.
6. **Port + UI:** `IFundsSurface.Debit/Credit`; TradePanel funds readout;
   no panel-side balance math.

### C.G1.3 The S7 signature's four design choices

The F13 line resolves four shape decisions; the recommended shapes are
the fifteen program's (each is reversible only by amendment):

| Choice | Recommended shape | Alternative |
|---|---|---|
| Leg grammar | the four named legs, atomic pairs | free-form barter extension (rejected: unbounded surface) |
| Heat scale | integer 0–12, relocate at ≥ 10, decay 1/day | float heat with multiplicative decay (rejected: determinism friction) |
| Purity ratio | authored per-catalog defaults: sealed 1.00 / clean 0.85 / suspect 0.55 / cut 0.30 | per-instance RNG at fence time (rejected: anti-reroll cost) |
| Escrow | contract survives relocation (pinned) | contracts void on relocation (rejected: punishes the player for system state) |

### C.G1.4 Ownership & seams

| Concern | Owner |
|---|---|
| Scrip balance + movement log | new `Economy/FundsLedger` (Core, sole authority) |
| Leg execution | `BlackMarketSystem` (extension) |
| Heat + relocation | black-market owner (additive state) |
| Purity | item-state extension through the inventory authority (additive field; never a parallel item-id family) |
| Restock ordering | sealed DEC-05 scorer (referenced) |
| Prices | canonical `MarketSystem` composition (unchanged) |
| Save | `funds_ledger` section (new, envelope V+2 frozen shape) + additive black-market fields |

### C.G1.5 State, save, determinism

- `funds_ledger` save section: integer balance + bounded log + typed
  reason vocabulary; old saves migrate to balance 0 / empty log (neutral
  default; no fabricated history).
- Heat, relocation history, purity map: additive fields in the existing
  `black_market` section, neutral defaults (`heat 0`, no relocations,
  purity absent = `clean`).
- Relocation target selection forks the existing black-market stock RNG
  pattern (campaign stream, fork-per-day, snake_case-gated). Purity at
  mint time: deterministic from authored ratios + campaign stream with
  persisted anti-reroll (same-day reopen never re-rolls — the R4 rule).
- Determinism guard: paired-seed test over a leg-heavy day; fingerprint
  equality across continuous vs. mid-reload play.

### C.G1.6 Failure modes (playbook rows 1,2,3,5,6)

Premise drift (scorer seam moved — stop at gate); parallel authority
(no second balance anywhere — panels render); presentation recomputation
(TradePanel must not compute prices); save-shape drift (neutral
defaults only); exactly-once (escrow + relocation events keyed).
Negative balance is a typed refusal via preflight debit, never a
post-hoc clamp.

### C.G1.7 Phases

- **P0 — premise probes:** run C.G1.1's greps; confirm 18/18, 6/6, seam
  lines. Gate: all green, else stop and amend.
- **P1 — `FundsLedger` Core:** round-trip, eviction invariant, typed
  refusal tests. Gate: new file alone passes.
- **P2 — legs:** atomicity tests with injected mid-transaction failure;
  economy regional regression (118+ baseline).
- **P3 — heat + relocation:** persistence, reload replay, contract
  survival pin.
- **P4 — purity:** fence pricing, old-save migration, anti-reroll test.
- **P5 — port + panels + restock bind:** `EconomyProbeTests` extension,
  panel lifecycle + a11y, `--black-market` selftest.

### C.G1.8 Focused verify

```
bash scripts/run_test.sh <new FundsLedger tests>            # alone first
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/        # regional
bash scripts/run_test.sh <black-market settlement suite>     # 18/18 pin
bash scripts/run_test.sh <restock priority suite>            # 6/6 pin
dotnet build Ashfall.csproj
godot --headless --path . -- --black-market-selftest
```

### C.G1.9 File impact & rollback

| File/area | Action | Risk |
|---|---|---|
| `Assets/Ashfall.Core/Economy/FundsLedger.cs` + DTOs | CREATE | medium |
| `BlackMarketSystem` | MODIFY | medium |
| inventory item-state (purity) | MODIFY (additive) | medium (schema) |
| `black_market_actions.json` / catalogs | CREATE/MODIFY | low |
| TradePanel + port surface | MODIFY | low |
| save sections (`funds_ledger`, black-market additive) | MODIFY | medium |

Rollback: revert in phase order (P5→P1); the `funds_ledger` section
orphaning is harmless (unknown sections skip); purity field absent
defaults to `clean`.

**Handoff — MUST PRESERVE:** canonical price composition; DEC-05
ordering; 18/18 and 6/6 pins. **MUST ADD:** one funds authority + four
atomic legs + relocation. **MUST NOT DO:** panel-side balance tracking,
a second pricing path, a second bounty/heat ledger. **FIRST SAFE STEP:**
P1 `FundsLedger` (pure Core, unbound).

## G-02 — XP-06 Body Integrity: Limb Requirements & Prosthetics (`W2A-G02-XP06-BODY-INTEGRITY`)

**Anchor:** XP-06 (the fifteen program's Plan 15) · **Gates:** F14/S8 and
D12 (the debt ledger's `DEBT-AMPUTATION-EQUIPMENT-RESTRICTION` equipment
half — same signature) · **Chain head for:** G-04 (EN-04 composite).

### C.G2.1 Current reality (verified)

- `AmputationSystem` is the sealed surgical limb authority: avatar +
  expedition halves sealed, the movement multiplier seam
  (`GetMovementSpeedMultiplier`) consumed by dispatch since C2-D3;
  `C2AmputationTravelTests` green.
- The interim truth is stated plainly in the record
  (`C2_DECISION.md:27`): *"amputation still does not restrict gear. No
  presentation-only limb illusion is used as a substitute."* The
  expedition half has a real signature (`"I approve C2-D3"`); only the
  equipment half is open.
- `ItemDefinition`/`EquipSlot` has no handedness/limb-requirement field
  (the D12 evidence row); `EquipmentConditionSystem`,
  `TickSharedSkillProgression`, and the crafting chains (foundry,
  ceramics, cordage, apiculture) are live owners ready to carry
  prosthetics.
- `MedicalWardSystem` runs procedures behind `StaffingPreflight`
  (`Fail("ward_unstaffed")`), 14/14 — the fitting gate already exists.

### C.G2.2 Required delta (what the S8/D12 line releases)

The fifteen program's C.15.1, as executable items:

1. **Schema package:** additive `limb_requirements` +
   `strength_requirement` on `ItemDefinition`; old items default to no
   requirement — byte-identical equip behavior, pinned.
2. **`body_state` limb map:** per-survivor effective-limb state **derived
   from** `AmputationSystem` (reads amputations; never records its own);
   persisted additively in the survivor save family.
3. **Equip-time gating:** typed `limb_requirements_unmet` refusal naming
   the missing limb; amputation auto-unequips now-violating items through
   the existing unequip path (exactly-once, journal line).
4. **Prosthetics catalog:** 6 items through the live crafting chains,
   condition decay via `EquipmentConditionSystem`; fitting is a ward
   procedure behind the staffing preflight.
5. **Rehabilitation arc:** fitting → adaptation → mastery as
   skill-progression entries; adaptation affects effective-limb
   contribution deterministically (authored constants; no RNG).
6. **Phantom pain:** event co-trigger tag extending the sealed
   sleep-narrative classification (additive; no new sleep authority).
7. **Host + UI:** `BodyIntegrityHostSession`; `SurvivorDetailPanel`
   limb-slot display; `--body-integrity-selftest`.

### C.G2.3 The S8 signature's schema choices

| Choice | Recommended shape | Alternative |
|---|---|---|
| Requirement encoding | `limb_requirements: ["hand","hand"]` list semantics (two entries = two effective hands) | boolean `is_two_handed` (rejected: can't express boots/later limbs) |
| Default for old rows | field absent → no requirement, byte-identical equip | backfill all rows (rejected: 280-file churn for zero gameplay) |
| Effective-hand rule | one prosthetic hand satisfies one `hand` entry at adaptation ≥ threshold; pre-adaptation counts 0.5 (authored) | prosthetic never counts (rejected: dead content) |
| Auto-unequip timing | surgical event → auto-unequip → next equip refused (gate order pinned) | lazy check at next tick (rejected: phantom-requirement window) |

### C.G2.4 Ownership & seams

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
| Fitting procedure | `MedicalWardSystem` (preflight gate) |

### C.G2.5 State, save, determinism

`body_state` persists additively; old saves derive from existing
amputation records by deterministic replay of the derivation (never
fabricated). Prosthetic condition rides equipment-condition persistence.
Adaptation timelines are authored constants × progression state. No new
RNG streams; phantom cadence uses the existing day-keyed medical/needs
streams. The 2-handed-weapon and boot rows in `items.json` gain the field
**or** rely on the documented default — the S8 line picks (recommended:
explicit field on the 2-handed family only; default elsewhere).

### C.G2.6 Failure modes (playbook rows 1,3,5,6)

Amputation mid-equip (gate order pinned); prosthetic destroyed while
fitted (falls back to unsatisfied requirement + medical event, never a
phantom slot); old saves (defaults pinned byte-identical — the
Plan21ProtectiveWear count pin stays 6 unless the line opts into
2-handed explicit rows); ward unstaffed (fitting refused — surfaced,
correct); two-handed + one prosthetic (boundary tests at the 0.5
pre-adaptation contribution).

### C.G2.7 Phases

- **P0 — premise probes:** amputation suite, protective-wear count pin,
  ward 14/14, crafting chain suites. Gate: green or amend.
- **P1 — schema + gating Core:** fields, loader validation
  (`CatalogIntegrityValidator` extension), gate logic, legacy parity
  pins. Gate: new file alone.
- **P2 — `body_state` + auto-unequip:** derivation, persistence,
  exactly-once journal, reload replay.
- **P3 — prosthetics catalog + crafting + condition.**
- **P4 — rehab arc + phantom-pain tag** (sleep-narrative extension tests).
- **P5 — host + panel + selftest**; a11y + lifecycle.

### C.G2.8 Focused verify

```
bash scripts/run_test.sh <new BodyIntegrity tests>            # alone first
bash scripts/run_test.sh Ashfall.Core.Tests/Medical/           # regional
bash scripts/run_test.sh <equipment + crafting owning suites>
dotnet build Ashfall.csproj
godot --headless --path . -- --body-integrity-selftest
godot --headless --path . -- --data-integrity-selftest
```

### C.G2.9 File impact & rollback

| File/area | Action | Risk |
|---|---|---|
| `ItemDefinition` + items.json | MODIFY (additive) | medium (schema) |
| `Assets/Ashfall.Core/Survivors/BodyIntegrity/` | CREATE | medium |
| equip owner + survivor save | MODIFY | medium |
| `prosthetics.json` + crafting chains | CREATE/MODIFY | low |
| `SurvivorDetailPanel` + host session | CREATE/MODIFY | low |

Rollback: revert P5→P1; schema field absent = default no-requirement
(neutral); `body_state` section orphan-skips. The C2 invariant returns
verbatim if fully rolled back.

**Handoff — MUST PRESERVE:** `AmputationSystem` as sole surgical
authority; the "no presentation-only limb illusion" interim truth until
P1 lands; DEC-20 v2 store semantics. **MUST ADD:** the schema, the
projection, the catalog, the arc. **MUST NOT DO:** a second limb-state
authority, bionics rework, new combat math. **FIRST SAFE STEP:** P1
schema (pure Core + loader, unbound).

## G-03 — EN-03 Chain: Underground Economy Pressure Composite (`W2A-G03-EN03-PRESSURE-COMPOSITE`)

**Anchor:** EN-03 (the EN program's full proposal) · **Gate:** G-01
sealed + the EN S-line (S6-equivalent for EN-03 follows S7).

### C.G3.1 Current reality

Everything EN-03's design requires exists **after G-01**: funds
authority, legs, heat, relocation, purity. The EN program's C.3 worked
specification (tier function, pressure soak, acceptance table) is the
design of record; this package adds only the execution detail the EN
program deferred: the composite's host binding and the pressure
**projection**.

### C.G3.2 Required delta (beyond the EN proposal)

1. **Pressure read model:** a projection over funds + heat + trust +
    purity that renders a four-band market-temperature rating
    (calm/raised/hot/relocated) for the panel — one query per bind, no
    caching, no recomputation in the panel.
2. **Composite gate test:** a soak that drives a seeded campaign through
   leg-heavy days under two difficulty bands and asserts: monotone heat
   response, relocation determinism, funds fingerprint stability,
   contract-survival pin. Runs twice; fingerprints identical.
3. **Panel strip:** the existing black-market panel gains the
   temperature band + next-relocation hint (rendered from the
   projection; never computed locally).

### C.G3.3 Phases (all after G-01 P5)

- **P0:** G-01 closeout row + evidence read; EN-03 S-line recorded.
- **P1:** projection Core + tests (band boundaries, neutrality on old
  state).
- **P2:** composite soak (seeded, twice, fingerprint equality).
- **P3:** panel strip + lifecycle/a11y + selftest extension.

**Focused verify:** projection tests alone; economy regional; the soak
file; panel selftest. **Rollback:** strip is presentation-only (revert);
projection is additive (revert). **MUST NOT DO:** a second pressure
ledger — pressure is a *view* over G-01 state, never persisted.

## G-04 — EN-04 Chain: Rehabilitation Medicine Slate (`W2A-G04-EN04-REHAB-SLATE`)

**Anchor:** EN-04 · **Gate:** G-02 sealed + the EN S-line for EN-04
(follows S8).

### C.G4.1 Current reality

After G-02: limb requirements, body-state projection, prosthetics, the
fitting→adaptation→mastery arc, phantom-pain tag. The EN program's
slate read-model specification is the design of record. The uniform
discharge window (`discharge_recovery_days`) remains the ramp (option
(ii) closed; never reopened).

### C.G4.2 Required delta (beyond the EN proposal)

1. **Slate read model:** per-survivor rehabilitation slate — fitted
   prosthetics, adaptation progress, next milestone, phantom-pain
   cadence — rendered for `SurvivorDetailPanel` (and the ward's patient
   list where the ward owner already renders rows).
2. **Milestone journal entries:** fitting, adaptation complete, mastery —
   exactly-once keys through `JournalSystem.TryAddRawEntry` (the proven
   war-routing pattern).
3. **Composite acceptance:** a seeded campaign drives one survivor
   through amputation → auto-unequip → prosthetic craft → fitting →
   adaptation → mastery with reloads mid-arc; asserts movement multiplier
   trajectory, journal exactly-once, and save round-trip.

### C.G4.3 Phases (all after G-02 P5)

- **P0:** G-02 closeout read; EN-04 S-line recorded.
- **P1:** slate projection + tests.
- **P2:** journal integration + exactly-once tests.
- **P3:** panel rendering + lifecycle/a11y + selftest extension.

**Focused verify:** slate tests alone; medical regional; journal suite;
panel selftest. **Rollback:** presentation + additive projection (both
revertible); no save shape touched. **MUST NOT DO:** reopen the
uniform-ramp decision (DEC-closed), a second rehab ledger.

## G-05 — EN-05 Chain: Signal Continuity & Voice (`W2A-G05-EN05-SIGNAL-CONTINUITY`)

**Anchor:** EN-05 · **Gate:** companion Plan 01 (the P1 distress content
seal) + the EN S6 line.

### C.G5.1 Current reality (verified)

- The radio subsystem is the most-tested system in the repo: rescue
  mission manager, follow-up scheduler, trust ledger, audio cue resolver
  — all sealed with exactly-once ledgers and the V6 codec (`RadioSave`
  V5→V6 frozen shape, migration to empty default).
- The Wave-5 deterministic replay harness drives the complete lifecycle
  (detect → stages → answer → rescue → follow-ups → trust → cue) through
  the real codec with stable fingerprints.
- Content is shipped: 24 authored follow-up entries (17 primary + 7
  expansion-effective) and 102 `audio_cue` occurrences; the
  primary-wins-overridden expansion rows remain dead data by design.
- Companion Plan 01's seal tail adds the validator rules, population
  replay, and closeout that this package builds on.
- Journal integration is a proven pattern (`TryAddRawEntry` consumers in
  the war routing); audio cues fire on the Intercept edge.

### C.G5.2 Required delta (beyond the EN proposal)

1. **Rescued-arc projection:** a read model over the follow-up scheduler
   + trust ledger that renders each rescued contact's continuing story
   (last event, trust band, next scheduled beat) for the radio panel.
2. **Signal-trust arc mapping:** the EN program's arc projection
   (employer/medical/revenge classes → trust cadence) implemented as a
   pure function over authored data; bounded [0,100]; integer math.
3. **Replay extension:** the harness gains rescued-arc assertions —
   fingerprint includes the projection state; paired-seed equality.
4. **Panel strip:** contact card with arc state; a11y and lifecycle per
   the house rules.

### C.G5.3 Phases (after companion Plan 01 seals)

- **P0:** P1 seal closeout read; EN S6 line recorded (with/without the
  XP-09 hook named — recommended: without; XP-09 is retired-adjacent).
- **P1:** projection + pure arc function + tests (band boundaries,
  empty-state neutrality).
- **P2:** replay harness extension; paired-seed fingerprint equality.
- **P3:** panel strip + cue-parity assertions (Intercept edge only).

**Focused verify:** projection tests alone; radio suite; replay harness;
panel selftest. **Rollback:** all additive/presentation; V6 shape
untouched. **MUST NOT DO:** a second trust ledger; cues off non-Intercept
edges; reopening the integer-permille availability retirement (DEC-06 —
it stays a tested pure math pin).

## G-06 — XP-08 + the D20 Endgame Lifts (`W2A-G06-XP08-ENDGAME-LIFTS`)

**Anchor:** XP-08 (trade routes & seasonal migration, W6 vehicle,
hard-depends on the funds legs) + the four C3 HOLD rows · **Gate:** S9
(the four D20 sub-lines) + G-01 sealed.

### C.G6.1 Current reality (verified)

- The C3 disposition itself is SIGNED (`0 PROMOTE · 1 RETIRE · 4 HOLD`);
  191 is retired and closed; 181/193/194 are explicitly out of scope.
- Each HOLD names its precondition verbatim (C3_HANDOFF.md:16-24):

| Plan | Condition to lift the HOLD | Type |
|---|---|---|
| 174 | a signed extension point on `SurvivorEnrichmentService`/`TradeSpecialtySystem` (not a new BackstorySystem) **with a consumed gameplay surface** | architecture-owner naming |
| 175 | a signed cross-run profile-store owner (versioned/checksummed, **outside campaign slots**) + Plan 34/149 completion-fact producers verified | product call (Meta Profile / NG+) |
| 192 | a player-route DTO with standing/raid/save seams signed **in a map amendment**; must not touch NPC owners (`CaravanTradeRouteCatalog`/`CaravanTradeNetworkSystem`/`TravelingCaravanSystem`) | schema |
| 199 | product names a human population owner **distinct from fauna** | product call (seasonal human/faction migration) |

- 175 has a live producer rail: `CampaignCompletionHistory` v2 store
  (frozen shape per DEC-20), 11/11 tests, writes completion facts every
  campaign end — exactly the producers the 175 precondition asks to
  verify.

### C.G6.2 Package shape

This package is **four independent sub-packages** sharing one phase
protocol. The foreman may sign any subset; each lift is its own line.

**Lift 174 — Enrichment Extension Point.** The surface: a typed
extension seam on `SurvivorEnrichmentService` (or
`TradeSpecialtySystem`, named in the line) that XP-08's route contracts
consume — e.g. a survivor's trade specialty feeding route standing.
Never a new BackstorySystem (the HOLD's own words). Phases: P0 premise
(services' current seams) → P1 seam + consumer + parity tests → P2
ledger flip (HOLD → executing). Files: the named service, one consumer,
tests. Rollback: seam removal; no save shape.

**Lift 175 — Cross-Run Profile Store.** The product call: does ASHFALL
grow a meta-profile (NG+/cross-run continuity) or stay per-campaign?
If lift: the store is a **new checksummed, versioned file outside the
campaign save family** (its own owner; not a campaign section), fed by
the already-verified `CampaignCompletionHistory` producers. Phases: P0
producers verification (11/11 + live write) → P1 store + checksum +
round-trip → P2 consumers (named by the line) → P3 flip. This is the
one sub-package allowed a new persistence file — the precondition
itself specifies "outside campaign slots", and the program-level
save-section rule covers campaign sections only. Blast radius is
nonetheless the largest here; the line should name the first consumer
explicitly (recommended: none in this package — store + producers only).

**Lift 192 — Player Route DTO.** A map amendment: player-route DTO with
standing/raid/save seams, signed in a map amendment (the map authority
docs), explicitly **not** touching the three NPC route owners. Builds on
EN-02's route-planning read model (the companion program's Plan 07 and
the EN program's EN-02). Phases: P0 map-authority read → P1 DTO + seams
in the map amendment → P2 save routing (additive to the campaign map
state) → P3 consumer (route standing display) → P4 flip.

**Lift 199 — Human Population Owner.** Product names an owner distinct
from fauna (seasonal human/faction migration). If lift: a
`HumanPopulationSystem` authority for seasonal movement of survivor
groups/faction presence, distinct from the wildlife overlay owner
(`WildlifeMapOverlay`, 1/1). Phases: P0 owner naming → P1 Core authority
+ day-keyed streams → P2 map/panel consumers → P3 flip.

**XP-08 proper (tariffs/route contracts/seasonal migration)** executes
only on the lifts its features need: route contracts want 174 + 192;
seasonal migration wants 199 + G-01. The line set determines the build
order; no XP-08 feature builds on an unlifted row.

### C.G6.3 Focused verify

Per sub-package: the named service's suite; `CampaignCompletionHistoryTests`
(11/11) for 175; map + wildlife suites for 192/199; per-flip ledger row
verification. 175 additionally: checksum round-trip, cross-run
read-back, and a determinism guard (store writes are fact-recorded, not
simulated).

**MUST NOT DO (all four):** touching NPC route owners (192), creating a
BackstorySystem (174), putting the profile store inside campaign slots
(175), folding human migration into the fauna owner (199).

## G-07 — D21 Quarantine Drain Program (`W2A-G07-QUARANTINE-DRAIN`)

**Anchor:** D21 · **Gate:** the D21 line (batch authorization).

### C.G7.1 Current reality (verified)

- The quarantine row (`DEBT-TEST-QUARANTINE-2026-09-12`, owner Foreman)
  is stale on two counts: it says 51 remaining `Compile Remove` entries
  and names AutopsyProcedures as the next candidate — the real count is
  **50 active** (56 matches minus 6 commented at csproj lines 111, 113,
  143, 151, 154, 156), and AutopsyProcedures is already RETIRED/sealed
  (2026-09-17).
- The `Twin_ASHFall/` archive the row points at **does not exist** —
  recovery is git-only, from `1d216b98^`.
- Policy is written: per-file, current API/content contract, source
  returns to project, focused target passes; "a green compile alone is
  not re-enable evidence" (TEST_POLICY.md:39-44).

### C.G7.2 The drain protocol (what the D21 line releases)

1. **Inventory pass (P1):** enumerate the 50 active entries; classify
   each as `RE-ENABLE CANDIDATE` (source recovers, contract still
   meaningful) / `RETIRED` (capability sealed elsewhere or abandoned)
   / `UNDECIDABLE` (needs a per-file decision). Output: a table in the
   package log with evidence per row.
2. **Batch execution (P2):** for each candidate in the authorized batch
   size (recommended: 10/file per batch): recover the source from git,
   re-point the contract to current APIs (never the reverse — the code
   bends to the API, or the file retires), remove the `Compile Remove`
   entry, run the file's focused target alone, then regional.
3. **Ledger reconciliation (P3):** flip the row to the true remaining
   count; record AutopsyProcedures' already-sealed status; the
   quarantine manifest (`Twin_ASHFall/quarantine/manifests/…`) rows are
   annotated, not silently deleted.
4. **Never silently re-enable** (TEST_POLICY): every re-enable carries
   the written reason + passing focused target in the package log.

### C.G7.3 Phases & verify

P1 inventory (read-only) → P2 batches (per-file) → P3 ledger. Verify per
file: `bash scripts/run_test.sh <recovered file>` alone first, then its
directory; the csproj diff shows exactly the batch's entries removed.

**MUST NOT DO:** bulk-uncommenting as a "fix"; re-enabling against
retired APIs; deleting manifest rows without annotation.

## G-08 — D11 Semantic-Kind Briefing Re-Grouping (`W2A-G08-SEMANTIC-BRIEFING`)

**Anchor:** D11 — the only carried item with **no recorded
recommendation** · **Gate:** the D11 line (option i or ii).

### C.G8.1 Current reality (verified)

- The semantic authority is sealed: 31A.1/31A.2 `SEALED`, claim
  `claim-wave10-part1-c1-plan31-2026-09-17` DONE for 31B routes + 31C
  day records. **Only the section-grouping rewrite remains.**
- The blocker is a pinned contract: the C2/Plan-17 no-silent-drop
  contract pins `GenericSectionTitle` ("System Activity") as the section
  for every unhandled kind (`DayEventVocabularyTests`, 8 cases), and
  B1 §6.12 forbids an unrelated briefing rewrite.

### C.G8.2 The two options, execution-shaped

**Option (i) — keep the flat pinned contract (status quo, zero cost).**
The briefing stays flat; every unhandled semantic kind lands in "System
Activity". Nothing executes; the D11 line closes the item as decided.
This program records it and moves on.

**Option (ii) — authorize re-grouping (the package):** map the 10
`SemanticKind` categories onto named briefing sections by amending the
pin. Blast radius (verified): `DailyBriefingReportBuilder.cs`,
`DayEventVocabulary.cs`, `DayEventVocabularyTests.cs` (8 cases — the
pin), `DailyBriefingReportBuilderTests.cs` (13 cases),
`docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md`, and the parity gate
`DayEventParitySourceGateTests`.

**Option (ii) phases:** P0 vocabulary read + section naming table →
P1 builder re-grouping + amended pin (the no-silent-drop contract
**moves**, never weakens: every kind still lands somewhere, unhandled →
a named fallback section) → P2 parity gate + matrix doc update → P3
briefing snapshot refresh. **Focused verify:** vocabulary tests (amended
pin), builder tests (13), parity gate, briefing selftest.

**The design rule either way:** the no-silent-drop invariant survives —
option (ii) re-groups, it never allows a kind to vanish from the
briefing. If a proposed mapping cannot place every kind, the mapping is
wrong, not the pin.

---

# Part D — The One-Liners: Seven Micro-Packages (M-01…M-07)

Each micro-package is bounded to hours, has a verified premise, and
releases on a single line. They are collected here because individually
they are too small for Part C and collectively they clear the decision
packet's residue.

## M-01 — D3 `water_sample_contaminated` equipability (`W2A-M01`)

**The three-way contradiction (verified):** the memo
(`WATER_SAMPLE_CONTAMINATED_DECISION_MEMO.md`) recommends retain-as-lore
(Option B); the micro-deferral sweep recommends strip (Option A);
DEC-09 sides with the memo. The sweep's premise is imprecise — the memo
records a live consumer: `Inventory.DegradeEquippedGear` degrades the
item **if worn in the Body slot during radiation exposure** (the
equipability is consumed by the degrade path).

- **Option A (strip):** 5 fields on one row (`items.json:3411+`:
  `isEquipable:false`, `equipSlot:"None"`, `radProtection:0`,
  `durability:0`, `degradeRate:0`), the `Plan21ProtectiveWearTests`
  count pin 6→5, and a save-load unequip/migration note for saves with
  the item equipped. Execution: P1 data edit + validator + count-pin
  amendment → P2 migration test. Verify: protective-wear suite, catalog
  integrity selftest.
- **Option B (retain):** zero code; author the diegetic reinterpretation
  ("radiation dosimeter / chemical indicator flask") into the item
  description for consistency. Execution: one JSON description edit +
  validator run.

**The line must name the option and the overridden document.**

## M-02 — D4 DEC-15 Ratification (`W2A-M02`)

Ratify that `WeatherStationSystem` is the sole forecast authority and
radio weather stays diegetic. Substance already settled (DEC-15 = SIGNED,
matching the sweep's own recommendation); what is missing is the **user**
signature — the register's "zero unsigned items" invariant is false
until then. Execution: the line + the register row's signature field.
No code, no data, no tests. One ledger edit routed through the
register's owner.

## M-03 — D13 Plans 126–129 Header Authority (`W2A-M03`)

**The contradiction (verified):** the live header
(`src/Main.Plans126_129.cs:3-6`) says 127–129 are "tethered recon drone,
continuous steel casting, lidar" follow-ups; a grep for
`ReconDrone|SteelCasting|Lidar|TetheredDrone` across
`Assets/Ashfall.Core/**/*.cs` returns zero relevant hits. D1 declined to
act; the superseded S2 audit said "fix the comment, do not rebuild."

- **(a) numbering drift:** retarget the header comment to the real
  127/128/129 subjects (read the plan corpus headers — the audit's
  census rows carry them), close. One comment edit + one test-adjacent
  grep recorded.
- **(b) genuinely un-built:** open a scoped plan for the drone/caster/
  lidar trio — which becomes a census row, not this program's content.
- **(c) leave as-is permanently:** record the decision; the header stays
  a known ambiguity forever.

## M-04 — D15 SOFC Fuel Owner (`W2A-M04`)

**Verified:** the placeholder is live — `FuelConsumer = units => true`
(`src/Main.Plans122to125.cs:51`, comment: "inventory owner binds the
real check in Phase 9"); the seam exists at
`src/Host/SofcPowerHostSession.cs:22`, consumed at `:91` and `:111-112`;
CLI stubs at `src/Host/HostCli.Plans122to125.cs`.

The line names the owner (inventory or grid units). Execution
(inventory case): bind the delegate to a preflight against the
inventory authority's fuel rows (typed refusal when insufficient), a
consumption ledger entry per burn tick (bounded, additive to the
existing power section), focused tests for refusal + round-trip, and the
selftest extension. Blast radius: one binding + the host session + the
named owner. **The SOFC stops running on free fuel the day this lands.**

## M-05 — D19a Pre-Campaign Barter Constructibility (`W2A-M05`)

**Verified:** `new SeededRng(147)` when `_campaignDay` is null
(`src/Main.Plans147.cs`) — the fallback fires only pre-campaign; in-game
the campaign stream is always used. Pure policy on one file.

- **Allowed:** the fallback stays; document the pre-campaign seed in the
  file and the determinism docs. Zero code.
- **Must not be constructible:** pre-campaign construction returns a
  typed refusal; one guard + one test.

## M-06 — D19b Radio Station Identity (`W2A-M06`)

**Verified:** `stationId` defaults to `"station_alpha"` on
`RadioHostSession.RecordObservation` — every player-recorded
triangulation observation carries the synthetic identity because the
panel has no station selection.

- **Keep synthetic:** document the choice; zero code.
- **Station-selection seam:** an authored station list (data), a panel
  selector, and the observation records carry the chosen id — persisted
  records are affected, so the field needs a neutral default
  (`station_alpha`) and a round-trip test. Blast radius:
  `RadioHostSession`, the triangulation panel, observation records.

## M-07 — D22 String Freeze Declaration (`W2A-M07`)

Not a question — a sequencing declaration only the foreman can make. The
line names scope + date. What it releases (in Wave 2 Program B, not
here): the localization bootstrap (UI string extraction → translation
catalog) and the VO generation gate (dialogue freeze + loudness
calibration). If deferred again, both recheck triggers stay parked.
This program records the declaration and hands the scope to Program B's
ship-readiness tracks.


---

# Part E — Sequencing & Dependency Order

## E.1 DAG (packages + external inputs)

```
companion Plan 01 (P1 seal) ─────────────┐
S6 (EN-05 line) ─────────────────────────┼─→ G-05
                                         │
S7 (F13 line) ──→ G-01 ──┬─→ G-03 (needs its EN S-line)
                         │
S8/D12 line ────→ G-02 ──┴─→ G-04 (needs its EN S-line)

S9 (D20 ×4, any subset) ──→ G-06 sub-packages (each independent)
G-01 ──→ (route contracts & seasonal migration features inside G-06)

D21 line ──→ G-07            D11 line ──→ G-08
one-liners M-01…M-07: each standalone on its own line
```

No package depends on the companion program's D-wave completing except
G-05 (companion Plan 01). G-06's sub-packages are independent of
everything except their own lift line and (for route contracts) G-01.

## E.2 Recommended signature order

1. **M-02 (D4 ratification)** — one line, zero code, restores the
   register's "zero unsigned items" invariant today.
2. **M-01 (D3 water sample)** — one row + one test pin; clears the only
   three-way contradiction in the packet.
3. **S7 → G-01** — the single highest-value engineering release in the
   tier (the whole underground economy becomes real).
4. **S8/D12 → G-02** — the second-highest (body integrity schema);
   independent of 3, can run concurrently with a second builder.
5. **M-04 (D15 SOFC)** — tiny; the "free fuel" live placeholder closes.
6. **G-03 / G-04** — the composites, once their heads seal.
7. **D21 → G-07** and **D11 → G-08** — anytime; good filler packages
   between waves.
8. **G-05** — after companion Plan 01 seals (it will, in the companion
   program's D-wave).
9. **S9 sub-set → G-06** — product calls; whenever the endgame direction
   is decided. 175 (profile store) is the only one with large blast
   radius; the others are small.
10. **M-03/M-05/M-06/M-07** — residue, anytime.

## E.3 Ledger routing (who flips what)

| Flip | Route |
|---|---|
| D-row status in the decision packet/register | integrator owns the register; the package's closeout cites the line verbatim |
| `DEBT-AMPUTATION-EQUIPMENT-RESTRICTION` → SEALED | G-02's closeout, owner-routed |
| `DEBT-TEST-QUARANTINE` count + candidate correction | G-07 P3, owner-routed |
| F13/F14 removal from the gate queue | the S7/S8 lines themselves, recorded by the integrator |
| KNOWN_DEBT XP-04/XP-06 rows | G-01/G-02 closeouts, owner-routed |
| census rows touched by G-06 lifts | the lift's P-final, owner-routed |

No builder edits a shared ledger directly; every flip is an owner-routed
citation (the 2026-09-19 audit's discipline).

## E.4 Effort & gate inventory

| Package | Phases | New files | Save shape | Signature |
|---|---|---|---|---|
| G-01 | 6 | ~4 | one new section + additive fields | S7 |
| G-02 | 6 | ~5 | additive (survivor family) | S8/D12 |
| G-03 | 4 | ~2 | none (view only) | follows S7 |
| G-04 | 4 | ~2 | none (view only) | follows S8 |
| G-05 | 4 | ~2 | none (V6 untouched) | S6 + companion P01 |
| G-06 | 3-4 per lift | 1-3 per lift | 175 only: new non-campaign file | S9 ×(subset) |
| G-07 | 3 | 0 (recovered) | none | D21 |
| G-08 | 4 (option ii) / 0 (option i) | 0 | none | D11 |
| M-01…M-07 | ≤2 each | 0-1 | M-01 A-case: migration note; M-06 seam: neutral default | one line each |

## E.5 Claim-row template

```
claim-w2a-<package>-<date>
owner: <builder>
package: W2A-<nn> per Seal-steps/ashfall-wave2-decision-gated-chains-
  execution-program-2026-09-19.md Part C/D
authorization line: <verbatim from Part F>
files: <Part C file impact map>
verify: <Part C focused verify block>
status: ACTIVE
```

## E.6 Concurrency rules

- At most two concurrent G-packages with disjoint claims (G-01 ∥ G-02 is
  the designed pair: economy vs. survivors, no shared files).
- G-03/G-04 never run while their heads are mid-flight.
- G-06 sub-packages may interleave with anything except their own named
  owners' other claims.
- Micro-packages are always safe alongside anything (single-file scope).
- Shared roots (`KNOWN_DEBT.md`, register, census) are integrator-only.

## E.7 Timeline sketch (gate-ordered, no dates promised)

1. The companion program's D-waves land (8 plans) — G-05's input seals.
2. S7/S8 signed → G-01/G-02 pair executes (the tier's main build).
3. Composites G-03/G-04 follow their heads.
4. One-liners and G-07/G-08 interleave as filler.
5. G-05 after companion Plan 01.
6. G-06 whenever the endgame product calls land.

---

# Part F — Consolidated Signature Queue

| # | Line to sign | Unlocks | Blast radius if signed |
|---|---|---|---|
| W2A-1 | `M-02: I ratify DEC-15 (WeatherStationSystem sole forecast authority; radio weather diegetic).` | register invariant restored | none (ledger only) |
| W2A-2 | `M-01 water_sample_contaminated: [Option A strip / Option B retain as lore], overriding [document].` | one data row + pin | A: 5 fields + migration note; B: description edit |
| W2A-3 | `S7 / F13 XP-04 economy legs: [leg grammar / heat scale / purity ratios / escrow shapes per C.G1.3]` | G-01 (funds, legs, relocation) | ~6 files + one save section |
| W2A-4 | `S8 / F14 / D12 body-integrity schema: [requirement encoding / default / effective-hand rule / auto-unequip order per C.G2.3]` | G-02 (schema, prosthetics, arc) | ~5 files + additive survivor fields |
| W2A-5 | `D15 SOFC fuel owner: [inventory / grid units].` | M-04 binding | one binding + host session |
| W2A-6 | `EN-03 composite: authorized (scope per EN program C.3 + W2A G-03).` | G-03 after G-01 | projection + panel strip |
| W2A-7 | `EN-04 composite: authorized (scope per EN program C.4 + W2A G-04).` | G-04 after G-02 | projection + panel strip |
| W2A-8 | `EN-05 / S6: authorized [without XP-09 hook].` | G-05 after companion P01 | projection + replay extension |
| W2A-9 | `D21 quarantine drain: [authorize batch of N / not worth it / correct the ledger row only].` | G-07 | per-file batches |
| W2A-10 | `D11 semantic-kind briefings: [(i) keep flat / (ii) re-group + amend the pin].` | G-08 (if ii) | briefing chain (6 files) |
| W2A-11 | `D20: 174 [lift/hold] · 175 [lift/hold] · 192 [lift/hold] · 199 [lift/hold].` | G-06 sub-packages | per lift (175 largest) |
| W2A-12 | `D13 Plans 126-129: [(a) fix comment / (b) open plan / (c) leave].` | M-03 | one comment or one census row |
| W2A-19a | `D19a pre-campaign barter: [allowed / not constructible].` | M-05 | one file |
| W2A-19b | `D19b station identity: [keep synthetic / author seam].` | M-06 | panel + records (if seam) |
| W2A-22 | `D22 string freeze: [declared, scope+date / deferred].` | Program B tracks | none here |

Every line is optional and independent except the two chain dependencies
(W2A-6 needs W2A-3's package sealed; W2A-7 needs W2A-4's). Nothing in
the companion program or the EN program waits on any line here.

---

# Part G — Verification, Rollback, Out of Scope, Handoff

## G.1 Verification pattern (per package)

```
1. bash scripts/run_test.sh <the package's new files>       # alone first
2. bash scripts/run_test.sh <the owning regional suite>
3. dotnet build Ashfall.csproj                              # 0 errors
4. godot --headless --path . -- <named selftests>           # touched paths only
5. record all four in the package log before proceeding
```

Additions by kind: G-01/G-02 append round-trip + legacy-parity +
continuous-vs-mid-reload tests; G-03's soak runs twice with fingerprint
equality; G-05 extends the replay harness; G-08 amends (never weakens)
the pin. The 180-second cap and the TEST_POLICY exclusions bind every
run; a compile-green result never substitutes for steps 2–4.

## G.2 Rollback

Every package is revertible in phase order; no package writes a
migration that a revert corrupts: the funds section orphan-skips,
purity defaults to `clean`, limb requirements default to none,
`body_state` derives from existing amputation records, the profile
store (175) is a new file — deleting it loses only meta facts, never
campaign state. The one irreversibility is M-01 Option A's count pin
(and its migration note) — which is why it is a signed choice, not a
default.

## G.3 Out of scope for the whole program

- Census tranche-2 rows (Wave 2 Program B).
- EN-01/02/06/07/08 (the EN program).
- The companion program's eight plans.
- VO generation and localization execution (Program B; D22 gates them).
- Bionics rework, new combat math, tax/politics, XP-09/10 revivals.

## G.4 Final handoff

When the tier closes: every signed package carries a closeout row
(evidence, commands, results), every D-row is flipped or explicitly
re-queued with its condition, the register holds zero unsigned items
(M-02's ratification completes that invariant), and the census/queue
state feeds Wave 2 Program B's drain protocol. The handoff names every
shared path touched (integrator-routed) and every file left
intentionally untouched.

## G.5 Exit criteria

The program exits when: every Part F line has a verdict; every released
package is sealed with evidence or re-queued with a named gate; G-07's
ledger row states the true remaining count; and the decision packet's
open set is empty or explicitly carried with conditions. The repo then
has no engineering work waiting on an unmade decision — the entire
remaining queue is either executing (companion/EN programs) or draining
(Program B).

---

## G.6 The empty-queue end state (what the repo looks like after this program)

When Wave 2 Program A exits, the decision debt is zero: every carried
D-row has a verdict, the register holds no unsigned items, and the
quarantine row states a true count. At that point the entire remaining
work-inventory of the repository is:

1. executing packages (companion program, EN program, this tier's
   released chains) — owned, claimed, gated;
2. census tranche-2 rows — draining under Wave 2 Program B's protocol;
3. ship-readiness tracks (localization, VO, release craft) — sequenced
   by the D22 declaration in Program B.

That is the structural payoff: nothing anywhere waits silently on an
unnamed decision. Every future gap is either a package with an owner or
a proposal with a named gate. The 2026-09-19 audit chain (audit →
companion → EN → W2A → W2B) then documents *how the queue was emptied*,
which is itself the template every future wave inherits.

---

# Part H — Evidence Index

| Claim | Evidence |
|---|---|
| Black-market settlement 18/18, DEC-02 | `BlackMarketSettlementService` suite; 2026-09-19 audit §2 |
| Restock scorer sealed | `ShelterBarterSystem.cs:283` (`ComputeItemPriorityScore`), `:136` (seam), `:307-316` (sort); 6/6 |
| XP-04 design of record | fifteen program Plan 14 (C.14.1–C.14.7) |
| Amputation sealed; equipment half open | `C2_DECISION.md:19-32` (incl. the "I approve C2-D3" signature and the `:27` interim truth) |
| XP-06 design of record | fifteen program Plan 15 (C.15.1–C.15.7) |
| Ward preflight | `StaffingPreflight` / `Fail("ward_unstaffed")`; 14/14 |
| Radio V6 + replay harness | `RadioSave` V5→V6 frozen shape; Wave-5 harness; 24 follow-ups; 102 cue occurrences |
| D20 preconditions verbatim | `C3_HANDOFF.md:16-24`; disposition `C3_DECISION.md` (0/1/4) |
| 175 producers live | `CampaignCompletionHistoryTests` 11/11; DEC-20 v2 frozen shape |
| Quarantine truth | 50 active `Compile Remove` (csproj, 6 commented out); AutopsyProcedures RETIRED 2026-09-17; `Twin_ASHFall/` absent; recovery `1d216b98^` |
| D11 scope | `WAVE9_PART1_CLOSEOUT.md:18`; 31A.1/31A.2 SEALED (`B3_PLAN31_RECONCILIATION.md:31-32`); pin = `DayEventVocabularyTests` |
| D3 consumer | memo `:40` — `DegradeEquippedGear` consumes Body-slot equipability; fields at `items.json:3411-3427` |
| D15 seam | `src/Main.Plans122to125.cs:51`; `src/Host/SofcPowerHostSession.cs:22,91,111-112` |
| D19a/b | `src/Main.Plans147.cs` (SeededRng 147 fallback); `PANEL_AUTHORITY_OWNERSHIP.md:62-70` |
| Census counts | 131 files: 33 SEALED / 113 AUDIT-PENDING / 2 READY-UNCLAIMED (2026-09-19 read) |
| Bootstrap call site | `src/Main.SaveOrchestrator.cs:163` (sole) |
| Forbidden-path stragglers | `src/Audio/AudioCueCatalog.cs:407`; `src/Main.FlagshipInstitutions.cs:45` |

## H.1 Reproducible probes (run at any claim time, read-only)

```
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/         # 118+ regional baseline
bash scripts/run_test.sh <black-market settlement suite>      # 18/18
bash scripts/run_test.sh <restock priority suite>             # 6/6
bash scripts/run_test.sh Ashfall.Core.Tests/Medical/          # ward 14/14
bash scripts/run_test.sh Ashfall.Core.Tests/Endgame/CampaignCompletionHistoryTests.cs  # 11/11
grep -n "ComputeItemPriorityScore" Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs
grep -n "FuelConsumer" src/Main.Plans122to125.cs src/Host/SofcPowerHostSession.cs
grep -n "station_alpha" src/Host/RadioHostSession.cs
grep -c "Compile Remove" Ashfall.Core.Tests.csproj
```

Any drift from these baselines is a premise change — record it before
building on the seam (B.5).

Baseline results at publication (2026-09-19): economy regional green at
its recorded baseline; black-market settlement 18/18; restock priority
6/6; ward 14/14; completion history 11/11; one scorer seam with
consumers at `:283`/`:307-316`; one SOFC placeholder delegate at
`:51`; one synthetic station default; fifty active quarantine entries.
These are the numbers every P0 probe re-asserts.


---

# Part I — Appendices

## I.1 Worked specification: the `FundsLedger` movement vocabulary

The ledger's typed reasons are the contract every leg, panel, and test
shares. Recommended initial vocabulary (each reason is a pinned enum
value; adding one is an additive change, renaming one is a break):

| Reason | Raised by | Debits | Credits |
|---|---|---|---|
| `bm_buy` | goods leg | ✓ | — |
| `bm_sell` | goods leg | — | ✓ |
| `bm_fence` | fence leg (purity-priced) | — | ✓ |
| `bm_contract_escrow` | contract open | ✓ | — |
| `bm_contract_payout` | contract complete | — | ✓ |
| `bm_contract_refund` | contract void (system-side) | — | ✓ |
| `loan_disburse` | syndicate loan | — | ✓ |
| `loan_repay` | loan settlement | ✓ | — |
| `loan_overdue_penalty` | idempotent overdue event | ✓ | — |
| `adjustment_signed` | foreman-noted corrections only | ✓/✓ | ✓/✓ |

Rules: the running-balance invariant (`balance == Σ signed reasons`) is
checked on every append and every round-trip; the log evicts oldest at
128 with the invariant re-checked; `adjustment_signed` requires a
package-log citation (it exists so corrections are auditable, not so
panels can fudge). A reason outside the vocabulary is a load-time
refusal, never a silent skip.

## I.2 Worked specification: the purity state machine

```
            (mint: authored ratio + campaign stream, anti-reroll)
   sealed ──(inspection consumer, follow-on)──→ suspect*   *not in this package
   clean   ──(bm_fence price 0.85)──→ sold
   suspect ──(bm_fence price 0.55)──→ sold
   cut     ──(bm_fence price 0.30)──→ sold
   absent  ─ default clean (old saves; never fabricated sealed)
```

Purity is item-state on scrip-adjacent goods only — never a parallel
item-id family, never a global flag. The field rides the inventory
authority's persistence; the fence leg prices from it; detection at
use/inspection sites is a named follow-on consumer (G-01 explicitly does
not build it).

## I.3 Worked specification: heat bands and relocation

| Heat | Band | Panel hint | System effect |
|---|---|---|---|
| 0–3 | calm | "quiet trade" | none |
| 4–6 | raised | "watchful eyes" | restock visible one day later |
| 7–9 | hot | "doors closing" | contract legs refused |
| ≥10 | — | relocation event | deterministic new contact; contracts survive (pinned) |

Decay 1/day (floor 0). Legs raise heat by authored amounts (recommended:
buy 1, sell 1, fence 3, contract 2). The G-03 pressure projection reads
these bands directly — the composite never re-derives them.

## I.4 Worked specification: effective-limb contribution

```
effective(hand) = natural_hand ? 1.0
                : (fitted_prosthetic && adaptation ≥ threshold) ? 1.0
                : (fitted_prosthetic && adaptation < threshold)  ? 0.5
                : 0.0
two_handed item requires effective(hand) + effective(hand) ≥ 1.5
boots: both foot slots — natural or fitted (no 0.5 window; authored)
strength_requirement: survivor strength ≥ authored value (pure compare)
```

All constants authored; boundaries tested at exactly 1.0/1.5/0.5 and at
the adaptation threshold. No RNG anywhere in the projection.

## I.5 Consolidated test matrix (per package, focused-first)

| Package | New test files | Pinned existing | Round-trip | Determinism | Panel |
|---|---|---|---|---|---|
| G-01 | FundsLedger, legs-atomicity, heat-relocation, purity | 18/18, 6/6, economy 118+ | ✓ (new section + additive) | paired-seed, anti-reroll | lifecycle + a11y |
| G-02 | BodyIntegrity (gates, derivation, boundaries) | amputation, protective-wear pin, ward 14/14, crafting | ✓ (additive) | derivation replay | limb slots |
| G-03 | projection bands, soak ×2 | G-01 suite | view-only | fingerprint equality | strip |
| G-04 | slate, journal exactly-once, composite arc | G-02 + medical | view-only | arc determinism | slate |
| G-05 | projection, replay extension | radio suite, harness | none | paired-seed | contact card |
| G-06 | per lift: seam/consumer, store checksum, DTO, population | named owners' suites | 175: new file | 175 fact-record only | per lift |
| G-07 | recovered files, alone first | regional per file | none | — | — |
| G-08 | amended pin (8), builder (13) | parity gate | none | — | snapshot refresh |
| M-01 A | pin amendment 6→5 | protective-wear | migration note | — | — |
| M-04 | refusal + round-trip | power suite | additive entry | day-keyed | readout |

## I.6 Governance interaction (how this program reads the rules)

- **Rule 5 (one authority per concern):** every G-package's first table
  is an ownership map; the two new authorities (FundsLedger,
  BodyIntegrity projection) exist because no owner covers them, and each
  is born with its save routing and test file in the same phase.
- **Rule 3 (JSON data is authoritative):** heat thresholds, purity
  ratios, leg grammar constants, prosthetic stats, adaptation constants,
  station lists — all authored in `Assets/StreamingAssets/Data/` with
  validator rules added in the same phase that consumes them.
- **Rule 4 (determinism):** no new RNG streams except where the design
  names them (relocation, purity mint) — both fork the existing
  campaign pattern with persisted anti-reroll.
- **Rule 7 (current evidence):** every premise block carries its probe
  commands; drift stops the package (B.5).
- **Rule 10 (stop when authority is missing):** the entire program is
  structured around this rule — its packages exist to make the missing
  authority a one-line decision instead of an improvisation.

## I.7 Risk register (program-level)

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Signature scope creep (a line quietly widens a package) | medium | high | line format + B.2 exclusions + amendment protocol |
| G-01/G-02 concurrent file overlap (economy vs. survivors assumed disjoint, isn't) | low | medium | P0 includes a cross-check of both impact maps; claim review enforces |
| Purity schema churn ripples into unrelated item rows | medium | medium | additive field, default `clean`, validator rule scoped to scrip-adjacent goods |
| 175 store creep (consumers added without product direction) | medium | high | the line names the first consumer; store+producers only by default |
| Amended pin weakens no-silent-drop | low | high | I.8 invariant: every kind still lands somewhere; parity gate proves it |
| Quarantine re-enables against retired APIs | medium | medium | code bends to API or retires; never the reverse |
| Ledger drift during execution (rows flipped late) | high (observed) | low | closeout cites verbatim lines; integrator's rerank catches residuals |

## I.8 The pin-amendment invariant (G-08, stated once)

Amending a pinned contract is legal only when the amendment **moves the
invariant to a stricter or equal surface**: the no-silent-drop pin
guarantees every semantic kind lands in a named briefing section;
option (ii) re-groups the destinations but every kind still lands
somewhere, and the parity gate (`DayEventParitySourceGateTests`) plus
the amended 8-case pin prove it. A mapping that drops a kind is not an
amendment — it is a regression wearing one.

## I.9 Glossary

- **Gate** — a named, verifiable precondition (a signature, a sealed
  package, a passing suite) that releases a package.
- **Leg** — one half of a two-leg transaction (`bm_buy` etc.); atomic
  pairs commit or roll back together.
- **Lift** — recording the D20 decision to release a specific HOLD row.
- **Line** — a Part F signature text, recorded verbatim.
- **Pin** — a test that freezes a contract (e.g. the no-silent-drop
  section mapping) so changes must amend it visibly.
- **Projection** — a read model over owned state; never persisted, never
  authoritative, always recomputed at bind.
- **Rail** — a finished engineering surface (owner + save + tests) that
  a gated package builds on.

## I.10 Document self-check

| Check | Result at publication |
|---|---|
| Every package names its gate and line | Part C ×8 + Part D ×7 + Part F ×15 |
| Every design extends a named owner | ownership tables in every plan |
| Save additions are named and owned | one (G-01 funds) + one optional non-campaign file (G-06/175); all others additive or view-only |
| No retired decision reversed | D20 lifts are the register's protocol; DEC-06/17/18/20 untouched |
| Every phase has a gate | phase lists + G.1 pattern |
| Evidence is file:line or command-result | Part H + inline |
| Production changes by this document | none |

---

*End of Wave 2 Program A. Planning authority only: register in
`docs/INDEX.md` when the integrator next regenerates the index.*

**Document provenance:** authored 2026-09-19 as Wave 2 Program A of the
four-document chain (audit → eight-plan companion → EN program → this);
all citations verified against the worktree at HEAD `fc73a306` plus the
uncommitted 2026-09-18/19 session; no claim, ledger, code, data, or test
file was modified to produce it. Wave 2 Program B (successor corpus
tranche-2 and ship readiness) completes the chain's forward edge.

## I.11 Definition-of-done tables (per package)

**G-01 — economy legs.**

| DoD | Evidence |
|---|---|
| Funds authority is sole (grep: no other balance field) | ownership audit row in closeout |
| Four legs atomic under injected failure | atomicity test file, red-then-green record |
| Relocation deterministic + contracts survive | relocation test + pin |
| Purity defaults neutral on old saves | migration + round-trip test |
| Panel renders, never computes | estimator/parity test |
| 18/18, 6/6, economy 118+ unchanged | focused runs recorded |
| Ledger rows flipped (XP-04, F13) | owner-routed citations |

**G-02 — body integrity.**

| DoD | Evidence |
|---|---|
| Old items equip byte-identically | legacy parity pin |
| Amputation auto-unequips exactly once | journal key test |
| Prosthetics craftable through live chains | crafting suite + catalog validator |
| Adaptation deterministic (no RNG) | boundary tests |
| Ward gate refuses unstaffed fitting | preflight test |
| D12 debt row SEALED | owner-routed citation |

**G-03 — pressure composite.** bands render from projection (parity
test); soak ×2 fingerprints equal; no persisted pressure state (grep);
strip a11y + lifecycle green.

**G-04 — rehab slate.** slate renders one survivor full-arc with
reloads (composite test); journal exactly-once; movement trajectory
pinned; uniform ramp untouched (ward suite 14/14 unchanged).

**G-05 — signal continuity.** replay harness extension green with
fingerprint equality; contact cards render arc state; V6 shape
byte-stable (codec suite unchanged); cues fire on Intercept edge only
(cue-parity assertion).

**G-06 — endgame lifts.** per lift: the named precondition verbatim in
the closeout; the named owners untouched (grep evidence for 192's three
NPC owners); 175's store checksummed + round-tripped + fact-only writes;
census rows flipped owner-routed.

**G-07 — quarantine drain.** every re-enable has written reason +
passing focused target; csproj diff shows exactly the batch; ledger row
states the true count; manifest rows annotated, not deleted.

**G-08 — semantic briefing.** every semantic kind lands in a named
section (parity gate); amended pin passes 8/8; builder 13/13; matrix doc
updated; briefing snapshot refreshed.

## I.12 Worked example: one `bm_fence` transaction, end to end

The reference shape for a two-leg transaction landing (the same loop
`bm_buy`/`bm_sell`/`bm_contract` repeat):

1. **Preflight (Core, synchronous):** the fence leg computes price =
   `MarketSystem` composition × purity ratio (0.55 for `suspect`); the
   ledger preflight-debits nothing yet but validates the goods half
   (faction-recognized loot present, not escrowed elsewhere) and the
   scrip half (ledger will accept a credit).
2. **Commit pair:** goods out (inventory authority) + scrip in
   (`FundsLedger` credit, reason `bm_fence`) — either both commit or
   the transaction refuses with a typed error naming the failed half.
   No partial state is observable: the inventory write and the ledger
   append happen inside one Core call; the injected-failure test proves
   rollback by failing the second write and asserting the first is
   absent.
3. **Heat:** +3 on the faction market's heat (authored), band effects
   evaluated at the next day tick, never inline.
4. **Persistence:** the ledger's bounded log and the inventory state
   ride their owning sections; a mid-transaction crash leaves either
   both halves (round-trip test) or neither (rollback test).
5. **Presentation:** the panel shows the priced fence result from the
   returned record; it never recomputes the price, the purity, or the
   heat (estimator test asserts the rendered values equal the record's).
6. **Rollback of the whole feature:** revert the commit; the ledger
   section orphan-skips; purity defaults `clean`; the market returns to
   its pre-G-01 truth with zero migration debt.

## I.13 W2A-specific failure playbook rows (extending the C.9 eight)

| Failure | Early signal | Containment |
|---|---|---|
| Leg half-commit (inventory moved, scrip not) | atomicity test red | typed refusal + rollback; never clamp |
| Phantom requirement (item gated on stale limb state) | parity pin red | re-derive from AmputationSystem; never cache |
| Escrow leak on relocation | contract-survival pin red | contract survives (pinned); relocation is cosmetic-geographic |
| Purity fabricated on load (old save gains `sealed`) | migration test red | absent = `clean`; never invent rarity |
| Profile store creeping into campaign slots | save-tree diff | the precondition's own words forbid it; move back out |
| Re-enable against a retired API | focused target fails | the file retires; the API never bends |
| Pin "amended" into deletion | parity gate red | I.8 invariant; the mapping is wrong, not the pin |
| Synthetic `station_alpha` spreading to new records | observation diff | M-06's chosen shape enforced at the seam |

## I.14 Program metrics (review cadence)

| Metric | Healthy value | Drift trigger |
|---|---|---|
| Part F lines with verdicts | monotonically growing | a signed package stalls two review cycles |
| Register unsigned items | 0 after W2A-1 | any new unsigned item |
| Packages executing vs. signed | 1:1 | a signature without a claim within one cycle |
| Save sections added (campaign family) | exactly 1 (G-01) | any other is a breach (B.2) |
| Quarantine active count | monotone non-increasing | any increase without a recovery note |
| Ledger residuals at closeout | 0 | any disagreement survives the integrator's rerank |

## I.15 Skill cross-reference

| Phase | Skill |
|---|---|
| P0 premise probes | `ashfall-analyze` |
| Package execution | `ashfall-implement` |
| Mid-package bugs | `ashfall-repair` / `ashfall-problem-identifier` |
| G-01/G-03 balance claims | `ashfall-balance-sim` (seeded sweeps) |
| Save safety (G-01, G-02, M-01A, 175) | `ashfall-save-fuzz` / `ashfall-determinism-guard` |
| Panel phases | `ashfall-ui-access` / `ashfall-snapshot-diff` |
| G-07 | `ashfall-test-gap` (coverage framing) + repo scripts |
| G-08 snapshot refresh | `ashfall-snapshot-diff` |

## I.16 Anti-pattern table (frequently misread rules, W2A edition)

| Anti-pattern | Correct reading |
|---|---|
| "The gate queue is empty, so everything is decided" | the Part F queue *is* the decision debt; unsigned = undecided |
| "D20's disposition is SIGNED, so 174-199 are done" | the disposition is signed; each HOLD still needs its own lift line |
| "The quarantine row says 51" | the row is stale; the truth is 50 active and the named candidate is already retired |
| "M-02 is trivial, skip the line" | the register's zero-unsigned invariant is false until the ratification line exists |
| "G-03 can compute heat itself" | pressure is a view over G-01 state; re-deriving it creates a parallel authority |
| "Amending the pin is routine maintenance" | pin amendments move invariants; they are signature-visible (I.8) |
| "175's store can live in the campaign save" | the precondition's own words: outside campaign slots, versioned, checksummed |
| "Option A on M-01 is the sweep's recommendation, just do it" | two documents disagree and a consumer exists; the tie-break is the foreman's, with the overridden document named |

## I.17 Chain-integrity rule

The six-document chain (audit → eight-plan companion → EN program →
this program → Program B → the live ledgers) must stay consistent: any
edit to one document's queue, gate, or package set must reconcile the
others in the same change. The ledgers always win disagreements; this
document's §A counts and Part F queue are its reconciliable surfaces.

## I.18 Worked example: the amputation auto-unequip sequence (G-02)

The reference shape for a cross-system event landing (the same loop the
fitting/milestone events in G-04 repeat):

1. **Surgical event (existing authority):** `AmputationSystem` records
   the amputation in its own save family — unchanged by this package.
2. **Derivation (new projection):** on the next bind (and at every
   save-load), `BodyIntegrity` re-derives the survivor's effective-limb
   map from the amputation records — a pure function; the persisted
   `body_state` is a cache-of-record with a derivation-replay test
   proving the cache equals the function output.
3. **Violation sweep:** the projection lists currently-equipped items
   whose requirements the new map violates (e.g. a two-handed rifle with
   one effective hand at 0.0).
4. **Auto-unequip (existing path):** each violating item is unequipped
   through the equipment owner's existing unequip routine, in
   deterministic order (slot order, then item id — pinned), each with
   exactly one journal line keyed
   `body_integrity_unequip:<survivor>:<slot>:<item>:<day>`.
5. **Next equip refused:** a subsequent equip attempt of the same item
   returns the typed `limb_requirements_unmet` refusal naming the
   missing limb — surfaced in the panel as a row-level hint, never a
   modal.
6. **Reload mid-sequence:** the fired-key ledger makes the sweep
   idempotent; continuous vs. mid-reload play produce identical
   equipment state (fingerprint test).
7. **Rollback:** revert the projection and gate; the equip path returns
   to its pre-G-02 truth; the journal lines remain as history (harmless
   orphans — the reader tolerates unknown reason kinds by design).

## I.19 Per-package rollback detail (beyond G.2's summary)

| Package | Revert order | Residual after full rollback | Residual acceptable? |
|---|---|---|---|
| G-01 | P5→P1 | orphaned `funds_ledger` sections in player saves | yes — unknown sections skip |
| G-02 | P5→P1 | authored `limb_requirements` fields ignored; journal lines | yes — fields default off |
| G-03 | P3→P1 | none (view only) | yes |
| G-04 | P3→P1 | journal history lines | yes |
| G-05 | P3→P1 | none (V6 untouched) | yes |
| G-06/174 | seam removal | consumer code removed with seam | yes |
| G-06/175 | delete store file | meta facts lost, campaign intact | yes — by design |
| G-06/192 | DTO removal | map amendment row flagged reverted | yes, amend the amendment |
| G-06/199 | authority removal | none | yes |
| G-07 | re-add `Compile Remove` entries | none | yes |
| G-08 | re-pin flat contract | snapshot pair mismatch | refresh snapshots on revert |
| M-01 A | restore 5 fields | count pin back to 6 | yes, amend pin back |
| M-04 | restore placeholder delegate | SOFC runs free again (known debt returns) | yes — debt row returns |
| M-06 | remove selector | observations keep `station_alpha` | yes — neutral default |

## I.20 Acceptance question per package (what the foreman asks at review)

| Package | The one question that proves the package |
|---|---|
| G-01 | "Show me a failed second-half transaction with the first half absent." |
| G-02 | "Show me an old save where a two-handed item equips identically before and after." |
| G-03 | "Show me two soak fingerprints that are equal." |
| G-04 | "Show me one survivor's full arc with a reload in the middle." |
| G-05 | "Show me the replay fingerprint with and without the projection — campaign state identical." |
| G-06/175 | "Show me the store's checksum round-trip and a campaign save that doesn't reference it." |
| G-07 | "Show me one re-enable with its written reason and its alone-first run." |
| G-08 | "Show me the parity gate proving every kind lands somewhere." |
| M-04 | "Show me the SOFC refusing to run without fuel." |

If a package cannot answer its one question, it is not done — regardless
of what else is green.

## I.21 Interaction with the EN program's S-queue

The EN program's Part F holds S1–S10; this program holds W2A-1…W2A-22.
The queues are disjoint but two lines interact:

- **S7 (EN) = W2A-3 (here)** — the same F13 signature; sign once, cite
  in both programs' logs. G-01 executes under this program's plan; EN-03
  executes under the EN program's proposal once G-01 seals.
- **S8 (EN) = W2A-4 (here)** — same F14/D12 signature; same protocol.

No other overlaps exist. A foreman signing S7/S8 in the EN program's
queue automatically releases this program's G-01/G-02 — the citation
cross-reference in the claim row is the mechanism.

## I.22 What Wave 2 Program A is not

- It is not a bid to re-open settled work: every "option" presented here
  (A/B, i/ii, lift/hold) is a decision the record shows is genuinely
  open, with its evidence cited.
- It is not a second queue competing with the live ledgers: Part F is a
  *view* of the decision packet's open set, formatted for signing; the
  packet and register remain the authorities.
- It is not a schedule: dates appear nowhere; ordering is gate-order
  only.
- It is not a design override: where the fifteen program or EN program
  already designed a package, this document references and executes;
  new design content is confined to the signature-choice tables
  (C.G1.3, C.G2.3) and the worked specifications, each marked
  recommended-with-alternative.

## I.23 Publication checklist (performed)

- [x] Package set matches the audit's decision-blocked list (F13, F14,
      D20 rows, D11, D21, D3/D4/D13/D15/D19a/D19b/D22) — nothing added,
      nothing dropped.
- [x] Every package's premise block cites verified 2026-09-19 evidence.
- [x] Save additions enumerated and owned (G-01 section; G-06/175 file).
- [x] No retired decision reversed; D20 lifts follow the register's own
      precondition protocol.
- [x] Chain-integrity surfaces named (§A counts, Part F queue).
- [x] Zero production changes; zero claims filed; zero ledgers edited.

## I.24 Reader's map (one paragraph per audience)

**The foreman** reads §0, Part B, and Part F — fifteen lines, each
buying a named package with a named blast radius. The recommended order
(E.2) front-loads the zero-code ratifications and the two big engineering
releases; everything else is optional and independent. **The builder**
reads one Part C or Part D package plus Part E's claim template; the
package is complete: premises, probes, phases, gates, files, rollback,
handoff. **The integrator** reads E.3 (ledger routing), I.14 (metrics),
and G.4 (final handoff); the closeout discipline is the same
owner-routed citation pattern proven on 2026-09-19. **A sweep agent**
reads any premise block and re-runs its probes; everything is
read-only. **A future auditor** reads I.23's checklist and Part H —
the document's claims are all re-verifiable with the H.1 probes in
under ten minutes.

*End of appendices.*

## I.25 Closing note on the tier's asymmetry

Wave 2 Program A exists because the repo's hardest remaining problems
are not engineering problems. The evidence tables in Part A show sealed
owners under every gate: markets that settle 18/18, limbs that persist
and multiply speed, radios that replay deterministically, a completion
history already writing cross-run facts. What stands between that
finished rail and the player's next real system is, in every case, a
recorded choice — a funds shape, a schema field, a lift, a drain, a
freeze. This document's contribution is to make each choice cheap to
make and each consequence impossible to mistake: the line names the
package, the package names the files, the files name the owners, and
the probes re-prove the premise before anything moves. Sign the lines
you want; the rest of the chain is already waiting downstream.
