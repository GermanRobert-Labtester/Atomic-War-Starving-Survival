# ASHFALL — Wave 2, Program B: Successor Corpus Tranche-2 & Ship-Readiness Program
## Census drain protocol, the ambition queue, and the road from passing selftests to a shippable release

| Field | Value |
|---|---|
| Program ID | `W2B-SUCCESSOR-CORPUS-AND-SHIP-READINESS` |
| Authored | 2026-09-19 (Wave 2, follow-on to Wave 2 Program A) |
| Status | **planning authority — nothing executed, nothing claimed** |
| Package count | 6 drain waves (T2-W1…T2-W6) + 5 ship-readiness tracks (R1…R5) + 2 governance packages |
| Signature requirement | drain-wave heads need no new signature (they execute under existing authority once their premise audit passes); ship-readiness tracks are sequenced by the D22 declaration |
| Production changes made by this document | none |

This is Wave 2 Program B, the final forward document in the 2026-09-19
chain. Where its predecessors answered *what is true*, *what is
executable now*, and *what one signature each releases*, this document
answers the two questions that remain after those are settled:

1. **What happens to the other hundred-plus plan corpus rows?** The
   census holds 131 files: 33 SEALED, 113 AUDIT-PENDING, 2
   READY-UNCLAIMED (as of the 2026-09-19 read; five anchors flip on the
   integrator's pending ledger pass). The audit proved the queue's head;
   this program drains its body.
2. **What turns a green selftest tree into a shippable release?** The
   repo can already export, smoke-boot, and gate itself. Ship-readiness
   is the remaining distance: string discipline, localization rails,
   VO loudness, release craft, and store-facing hardening — each
   sequenced, none invented from scratch (the skills and scripts exist;
   the program orders them).

## 0 · Program summary card

| Field | Value |
|---|---|
| Corpus state (verified 2026-09-19) | 131 census files · 33 SEALED · 113 AUDIT-PENDING · 2 READY-UNCLAIMED · 117 nonterminal rows → 112 after the five anchor flips |
| Drain mechanism | tranche-2: six waves of premise-audit-first packages, heads certified by an EN-08-style mini-rerank |
| Ship-readiness tracks | R1 string freeze & extraction · R2 localization bootstrap · R3 VO loudness & generation gate · R4 release craft hardening · R5 store-facing hardening |
| Governance packages | T2-GOV ledger truth consolidation (with the EN-08 rerank) · T2-GOV2 chain archival & INDEX registration |
| Save sections added | zero (drain waves execute existing plans; ship tracks are presentation/pipeline only) |
| New gameplay authorities | zero by design — every drain wave executes an already-authored plan's existing owner map |
| Prerequisite | nothing; the protocol is executable today, wave heads selected by the ranking in §A.3 |

Reading paths by role: **foreman** — §0, Part B (what the drain never
does), the D22 line (in Program A's Part F; it sequences R1–R3).
**builder** — Part C (your wave's package), Part E (sequencing).
**cheap sweep agent** — premise-audit protocol, all read-only.
**integrator** — Part E.3 ledger routing, Part G exit criteria.

---

## How to read this document

- **Part A** establishes the corpus reality: the census math, the
  category structure of the 113 AUDIT-PENDING rows, the ranking rules,
  and the ship-readiness baseline (what the repo can already prove about
  itself).
- **Part B** defines the drain contract: what a tranche-2 wave is, what
  it never does (no premise skipping, no silent seals, no parallel
  queues), and how wave heads are certified.
- **Part C** specifies the six drain waves: each wave's shape, its
  premise-audit entry gate, its execution protocol, its closeout, and
  its acceptance question.
- **Part D** specifies the five ship-readiness tracks and the two
  governance packages.
- **Part E** sequences everything: the drain/release interleaving, ledger
  routing, concurrency, and the timeline sketch.
- **Part F** holds the one signature this program actually needs (none
  for the drain; the D22 declaration for R1–R3) and the certification
  lines the integrator records per wave.
- **Part G** covers verification, rollback, out-of-scope, handoff, exit
  criteria, and the empty-corpus end state.
- **Part H** is the evidence index with reproducible probes.
- **Part I** holds appendices: the audit-grade premise template, the
  per-wave log format, worked classifications, test matrices, glossary,
  and self-check.

A note on honesty: this document deliberately does **not** pre-name all
113 AUDIT-PENDING rows' dispositions. Naming them without auditing them
would be the exact anti-pattern the census exists to prevent. What it
pre-builds is the *protocol* — the ranking, the entry gates, the wave
shapes — so that each row's audit-to-package interval is short. The wave
heads named in Part C are the ones whose evidence is already on record
(the census anchors and the audit's verified frontier); the rest enter
through the same gates as their waves reach them.

---

# Part A — Current Reality: the Corpus and the Ship Baseline (verified 2026-09-19)

## A.1 The census math

- 131 tracked files: 33 SEALED · 113 AUDIT-PENDING · 2 READY-UNCLAIMED.
- The 2026-09-19 audit verified five anchors (C2[9]–C2[13]) as executed
  and awaiting their SEALED flips — the integrator's ledger pass moves
  the nonterminal count from 117 to 112 (111 AUDIT-PENDING + 1
  READY-UNCLAIMED after the audit's two dependency-unblocked rows are
  audited).
- The audit's headline frontier — 8 available plans — is executing under
  the companion program. The EN program adds one-signature proposals;
  Wave 2 Program A converts the gated tier. None of those touch the
  AUDIT-PENDING body. This program does.

## A.2 What the AUDIT-PENDING rows actually are

The census's own category structure (read 2026-09-19) sorts the body
into recognizable strata. The exact per-row truth requires each row's
audit — which is the point — but the strata determine wave design:

| Stratum | Approximate share | Character | Entry gate |
|---|---|---|---|
| Sealed-elsewhere residuals | large | rows whose capability shipped through a canonical owner; the row is stale, the work is done | verification-only audit → SEALED flip |
| Executed-unrecorded | medium | rows whose work landed in a prior wave's commit but no closeout row exists | evidence citation → SEALED flip |
| Partial-by-design | medium | rows intentionally split (e.g. equipment halves, follow-on legs) | premise audit → either SEALED-partial or a scoped package |
| Genuinely open | small | rows whose plan never executed and whose premise still holds | full premise audit → standard package via the companion genre |
| Premise-consumed | small | rows whose premise the last 18 months of sealing invalidated (XP-02/03 pattern) | premise audit → RETIRED with citation |
| Terminal candidates | small | rows superseded by a later authority | audit → TERMINAL/RETIRED flip |

The strata's lesson from the audit's own counting: the modal
AUDIT-PENDING row is *already done or already dead* — the audit work is
forensic, and the build work concentrates in the genuinely-open stratum,
which the census's READY-UNCLAIMED and dependency-unblocked rows front.

## A.3 The ranking rules (wave-head selection)

A row becomes a wave head when it scores highest on, in order:

1. **Dependency-unblocked** — its named blockers are sealed (verified,
   not assumed).
2. **Player-visible payoff** — executing it adds observable gameplay,
   not paperwork.
3. **Rail completeness** — owner, save, tests already exist.
4. **Blast radius smallness** — the package stays inside one claim.
5. **Tie-break: census order** — deterministic, no discretion.

The same ranking governs wave *composition*: each wave packages 8–15
rows that share a claim footprint (one region of the tree), so a single
builder can own the wave disjointly from the concurrent programs.

## A.4 The ship-readiness baseline (what the repo can already prove)

Verified by the 2026-09-19 audit and the repo's own gates:

- **Export:** headless Godot exports for configured Linux/Windows
  presets; PCK includes the JSON data authority; smoke-boot of the
  binary (`ashfall-export-build` skill's rails).
- **Data integrity:** `CatalogIntegrityValidator` gates the ~280-file
  data authority; `--data-integrity-selftest`.
- **Test discipline:** focused xUnit through `scripts/run_test.sh`
  (180-second cap); selftest battery; determinism guards (seeded replay,
  checksum culture invariance).
- **UI QA rails:** 69 golden snapshot panels + diff tooling; a11y audit
  rails; player-panels battery.
- **Asset hygiene:** LFS policy checks; the asset registry; the
  forbidden-path gate (with its two known lowercase stragglers —
  Program A's G-package territory, and the audit's flagged follow-up).
- **Rulebook hygiene:** 13 client rulebooks synced from canonical
  AGENTS.md via `scripts/ci/sync-agent-rulebooks.py` with `--check`.

What does **not** exist yet (the R-tracks' reason): a UI string
extraction pass (localization is DEC-13-deferred behind it), a dialogue
text freeze (DEC-11's VO gate), audio-bus loudness calibration, a tagged
release ritual with changelog provenance, and store-page-facing
metadata. Each track in Part D assembles existing rails rather than
inventing new ones.

## A.5 Standing premise corrections (banked by the chain)

Carried verbatim from the 2026-09-19 chain: the bootstrap call-site
truth; the XP-01 partial binding; XP-02/03 premise-consumed; the census
counts (this document's §A.1); the forbidden-path stragglers; the
quarantine truth (50 active entries, candidate retired, archive absent);
the DEC-20 v2 frozen shape; and the two-ledger residual set the
integrator owns (anchor flips, DEC-01/16 updates, the stale
merchant-restock row). None are re-litigated here.

## A.6 Why drain and ship-readiness belong in one program

Both are *completion-of-the-queue* work: the drain closes the corpus the
way the companion program closed its frontier, and the ship tracks close
the distance between "all gates green" and "a player outside the team
can install and keep it." Sequencing them together prevents the classic
failure mode — an endless feature queue that never ships — by making
the drain's exit condition (empty nonterminal census) *simultaneous*
with the release ritual's readiness. Part E interleaves them so neither
starves the other.

## A.7 Drain-rate evidence (what the sealing waves already proved)

The 2026-09-17/19 history is the drain's calibration data:

| Event | Rows affected | Elapsed | Mechanism |
|---|---|---|---|
| 2026-09-17 wave (six debts + D1 handoffs) | ~10 ledger rows | 1 session | evidence citations |
| 2026-09-18 packet (23 items adjudicated) | 23 decision rows | 1 session | packet + register |
| 2026-09-18/19 sealing wave (ten seals) | ~15 census anchors + dependents | 2 sessions | companion-genre packages |
| 2026-09-19 audit (this chain) | 8-plan frontier verified | 1 session | read-only audit |

Projected forward (I.29): the body's modal rows are cheaper than the
sealing wave's packages (flips vs. builds), so a wave-per-cycle cadence
is realistic without heroics. The history also warns: every era's
cheapest rows drained fastest, leaving the open stratum concentrated —
which is exactly why T2-W5 is uncapped in count but capped in
concurrency, and why the convergence guard exists.

## A.8 Two-track independence (why drain and ship cannot block each other)

The drain's outputs are ledger truth and (in W5) gameplay; the ship
tracks' outputs are pipeline artifacts. Their only shared resource is
builder attention, which E.2's interleave arbitrates by rule. Their
shared *files* are nearly disjoint (docs/ledgers vs. tooling/config),
and the one real interaction — a string freeze pausing string-bearing
packages — has an explicit protocol (queue behind the freeze or land
strings via the catalog). Independence is what lets the program exit
even if the declaration never fires (G.5's either/or): the drain
completes regardless, and the ship tracks complete whenever the
declaration does.

---

# Part B — The Drain Contract

## B.1 What a tranche-2 wave is

A bounded package of 8–15 census rows sharing one claim footprint,
executed in a fixed loop:

```
rank (A.3) → premise audit (per row) → classify → act → closeout
```

- **classify:** SEALED-RESIDUAL (done elsewhere; flip with citation) ·
  EXECUTED-UNRECORDED (done, uncited; flip) · PARTIAL-BY-DESIGN (flip
  partial or spawn a scoped package) · OPEN (execute the companion-genre
  plan) · PREMISE-CONSUMED (RETIRED with citation) · TERMINAL candidate
  (flip per census rules).
- **act:** flips are owner-routed ledger edits with citations; OPEN rows
  become standard packages with claims; nothing is bulk-edited.
- **closeout:** the wave log records every row's verdict + evidence +
  command results; the integrator's mini-rerank certifies the next
  wave head.

## B.2 What the drain never does

- **Never skip the premise audit.** A plan's existence is not evidence
  its premise survived the last year of sealing (Rule 7; the XP-02/03
  lesson is the canonical example).
- **Never silently seal.** Every SEALED flip carries the citation and
  the focused evidence that proves it (the anchor-flip discipline the
  integrator is already applying).
- **Never spawn a parallel queue.** The census stays the single corpus
  authority; wave logs are working documents, not ledgers.
- **Never race the active programs.** Rows whose footprints overlap the
  companion/EN/W2A claims are deferred to later waves, not audited
  around.
- **Never aggregate away independence.** Save/load, determinism,
  lifecycle, mutation, fuzzing, state-transition, and cross-system rows
  keep their independent tests (TEST_POLICY's rule); the drain batches
  *ledger work*, never test identity.

## B.3 The premise-audit entry gate (every row, every wave)

Each row's audit answers six questions, recorded in the wave log:

1. **Does the row's named blocker still exist?** (verify in source, not
   the ledger)
2. **Does its named owner still own the concern?** (grep the seam)
3. **Did a canonical owner ship the capability?** (the sealed-elsewhere
   test)
4. **Does its save path still match the current section family?**
5. **Does its test file still exist and pass focused?**
6. **What does the player see if this row executes?** (the payoff
   question — if "nothing, ever," the row is a terminal candidate)

A row that cannot be audited cheaply is not audited cheaply: it becomes
its own package rather than being swept into a verdict.

## B.4 Wave-head certification (the mini-rerank)

Before each wave starts, the integrator (or the foreman acting as one)
runs the EN-08-style rerank on the remaining AUDIT-PENDING rows and
certifies the head in the wave log:

```
T2-W<nn> head certified: <row id> · rank basis <1-5 from A.3> ·
premise probes green · claim footprint <files> · no overlap with
active claims
```

Certification is the only gate between waves — no new foreman signature
is needed for the drain itself, because every row's authority already
exists in the corpus; the drain executes recorded decisions, it does not
make new ones.

## B.5 Exclusions

- The 8 companion plans, EN proposals, and W2A packages (owned by their
  programs; their census rows flip on their closeouts).
- New green-field design (the drain executes or retires; it never
  authors new systems).
- VO/localization *execution* details beyond the track gates (R1–R3 own
  their scopes).

---

# Part C — The Six Drain Waves

Wave shapes are fixed; wave *contents* fill by the A.3 ranking at
certification time. The named heads below are the ones already on
record as next-in-line; later waves' heads are certified fresh.

## T2-W1 — Anchor Residual Wave

**Shape:** the cheapest, highest-certainty rows first — the
sealed-elsewhere and executed-unrecorded strata, deliberately front-
loaded to shrink the nonterminal count fast and prove the protocol.

**Known contents on record (verified 2026-09-19):**

- The five anchor flips (C2[9]–C2[13]) — already verified by the audit,
  awaiting the integrator's ledger pass (this wave either executes them
  or, if the integrator has already flipped them, records them as done).
- The ledger residuals the audit flagged: DEC-01/DEC-16 register rows
  (D1/D2 resolved), DEC-05 evidence drift, the stale merchant-restock
  row in INTEGRATION_PLANS.md — reconciled with citations.
- The census's own stale-count rows wherever the audit's probes showed
  sealed-elsewhere truth.

**Protocol:** audit per row (B.3) → flip with citation → closeout. No
code. **Acceptance question:** "Show me the nonterminal count lower than
the wave's start, with every flip carrying a citation." **Estimate:**
one session.

## T2-W2 — Dependency-Unblocked Open Wave

**Shape:** genuinely-open rows whose blockers the 2026-09-19 seals
removed — the same class as the audit's Plans 37/48 (census C2[15],
C2[21]), whose standard premise audits are already mandated.

**Known contents on record:** C2[15]/Plan 37 (input reality, focus,
controller parity) and C2[21]/Plan 48 (release craft) — both specified
at execution grade in the companion program as Plans 07/08; their census
rows flip on those closeouts, and their *dependent* rows (the corpus
edges the audit traced: Plan 37's 17B/25A/25B/16A/31B/28A set; Plan 48's
29A/39A/46A/46B/47A/47C set) become this wave's auditees.

**Protocol:** premise audit per dependent row (do the deps really
clear?) → classify → execute-or-flip. Execution follows the companion
genre (its plans carry phases, gates, focused verify).

**Acceptance question:** "Show me one dependent row's audit proving its
premise, and its verdict with evidence." **Estimate:** 1–2 sessions per
8–15 rows.

## T2-W3 — Partial-by-Design Wave

**Shape:** rows intentionally split across waves (equipment halves,
follow-on legs, expansion phases) — the stratum the W2A program's G-02
and the fifteen program's split-seals exemplify. Each row's audit
determines: is the remaining half owned by a gated package (flip
partial, cite the gate) or orphaned (spawn a scoped package)?

**Protocol:** audit → route each partial to its owner (gated program,
active claim, or new scoped package) → flip or claim. **Acceptance
question:** "Show me every partial row naming its remaining-half owner —
none orphaned." **Estimate:** 1–2 sessions.

## T2-W4 — Premise-Consumed Retirement Wave

**Shape:** rows whose premise the sealing waves invalidated — the
XP-02/XP-03 pattern (capability landed through canonical owners like
`WastelandMapSystem.PlanRoute`). This wave exists because retiring a
row is *also* evidence work: the retirement citation must name the
canonical owner and the seam that consumed the premise.

**Protocol:** audit → retirement citation (owner + seam + probe) →
RETIRED flip. **Acceptance question:** "Show me one retirement whose
citation I can re-verify with one grep." **Estimate:** one session.

## T2-W5 — Genuinely-Open Execution Wave

**Shape:** the stratum that needs real building — rows whose premises
hold and whose plans never executed. These become standard
companion-genre packages, one claim each, sequenced by the A.3 ranking.
This is the only wave expected to span multiple builder sessions, and
its size is deliberately uncapped in *count* but capped in *concurrency*
(two packages at a time, E.6).

**Protocol:** full premise audit → scoped package (companion genre:
phases, gates, focused verify, file impact, rollback, handoff) → claim →
execute → closeout. **Acceptance question:** "Show me the package's
phase-gate log with every gate's focused result recorded."

## T2-W6 — Terminal Consolidation Wave

**Shape:** the stratum of terminal candidates — superseded rows,
retired-adjacent concepts, and rows whose payoff question answered
"nothing, ever." The census's TERMINAL/RETIRED rules apply; nothing is
deleted silently (the repo-hygiene discipline: quarantine/annotate, not
destroy).

**Protocol:** audit → TERMINAL/RETIRED flips with annotations → the
census's own terminal section reconciliation. **Acceptance question:**
"Show me the census's terminal section matching the archive, with no
row deleted without an annotation."

## C.7 Wave-log format (every wave, one log)

```
# T2-W<nn> log — <date>
head certified: <row> · <rank basis> · probes green
rows:
  - <row id>: <classification> · evidence <file:line / command> ·
    action <flip / package / retirement> · citation <where recorded>
closeout: nonterminal <before> → <after> · focused results <list>
next head: <row> certified per B.4
```

---

# Part D — Ship-Readiness Tracks & Governance Packages

## R1 — String Freeze & Extraction Track

**Gate:** the D22 declaration (Program A's W2A-22 line — scope + date).
**Scope:** declare the freeze; run the UI string extraction pass
(inventory user-facing strings across UI, data JSON, and diegetic text —
the `ashfall-localize` rails); produce the extraction catalog as the
localization source of truth. **Never:** machine-translation before
extraction is complete; hardcoded-string additions after the freeze
(the gate turns new hardcoded strings into review blockers). **Acceptance
question:** "Show me the extraction catalog's coverage number and the
freeze date on record."

## R2 — Localization Bootstrap Track

**Gate:** R1 complete (extraction catalog exists). **Scope:** scaffold
the Godot translation layer over the catalog; wire the locale selection;
gate new strings (the localize skill's discipline). No multi-language
*content* commitment is made here — the rails stand ready; translation
is a content decision after the rails prove out. **Acceptance:**
"Switch locale in a headless run and show me the UI reading from the
translation layer."

## R3 — VO Loudness & Generation Gate Track

**Gate:** dialogue text freeze (R1's scope covers it) + the audio-bus
loudness calibration (DEC-11's recheck trigger). **Scope:** calibrate
the audio bus loudness (the audio-qa rails: cue catalog audit, orphan
detection, loudness normalization); only then does full VO generation
un-defer. **Acceptance:** "Show me the loudness report and the cue
catalog with zero orphans."

## R4 — Release Craft Hardening Track

**Gate:** none (builds on the companion program's Plan 08, which lands
the release gates). **Scope:** harden the tagged-release ritual —
version bump discipline, changelog from git history with provenance,
lane/snap discipline (Bit-era rule: never push the main lane), full
pre-release gate (focused tests, data integrity, asset gate, export
smoke), and the release checklist. **Acceptance:** "Show me one tagged
release produced by the ritual, with its checklist fully checked."

## R5 — Store-Facing Hardening Track

**Gate:** R4's first tagged release. **Scope:** store-page metadata,
rating-questionnaire answers grounded in actual content, platform smoke
matrices, and the first-access experience polish (tutorial-review rails
for the first-hour audit). **Acceptance:** "Show me the store-facing
checklist grounded in evidence, not aspiration."

## T2-GOV — Ledger Truth Consolidation

The EN-08 rerank, executed here as the wave-head certification loop
(B.4), plus the census's own reconciliation: the 131-file census's
status column becomes byte-consistent with the ledgers' rows. This
package closes the "two truths" drift the 2026-09-19 audit flagged.

## T2-GOV2 — Chain Archival & INDEX Registration

The 2026-09-19 chain (audit, companion, EN, W2A, W2B) gets registered in
`docs/INDEX.md` on the integrator's next regeneration, and superseded
planning docs are archived per the docs-atlas discipline (supersede
links, never delete). This document schedules that work; it does not
execute it (the index is a claimed path with in-flight edits).


---

# Part E — Sequencing: Drain and Release, Interleaved

## E.1 The full-program DAG

```
                      ┌─ T2-W1 anchors/residuals (today)
census body ──────────┼─ T2-W2 dependency-unblocked (after companion waves)
                      ├─ T2-W3 partials (after W2A heads sign, or routes to gates)
                      ├─ T2-W4 retirements (anytime; evidence-only)
                      ├─ T2-W5 open execution (1-2 concurrent packages)
                      └─ T2-W6 terminal consolidation (last)

D22 declaration (W2A-22) ──→ R1 strings ──→ R2 localization rails
                        └──→ R3 VO loudness (needs R1 freeze + calibration)
companion Plan 08 ──→ R4 release craft ──→ R5 store-facing
T2-GOV rerank loop runs between every wave
T2-GOV2 INDEX registration runs at chain completion
```

## E.2 Interleaving rule (the anti-starvation contract)

The drain and the ship tracks are deliberately interleaved so neither
starves the other:

- Every drain wave T2-W1…W4 is ledger/forensic work — it never blocks a
  release.
- T2-W5 (real building) is capped at two concurrent packages (E.6); when
  a release track (R3/R4) needs the same builders, the release wins —
  shipping is the program's terminal value.
- R1's freeze declaration can pause T2-W5's *string-bearing* packages:
  a package that would add user-facing strings mid-freeze queues behind
  the freeze or lands its strings via the extraction catalog. (Ledger
  work and retirement waves are never paused by a freeze.)

## E.3 Ledger routing

| Flip/action | Route |
|---|---|
| Census status flips | wave closeout, owner-routed with citations |
| RETIRED/TERMINAL annotations | wave log + census terminal section |
| DEC-13 (localization) un-defer | R2 closeout cites the extraction catalog |
| DEC-11 (VO) un-defer | R3 closeout cites the loudness report |
| INDEX registration | T2-GOV2, integrator-owned |
| Register rows for new gates | only if a track creates one (none planned) |

## E.4 Effort inventory

| Unit | Sessions (est.) | Code | Signature |
|---|---|---|---|
| T2-W1 | 1 | none | none (certification only) |
| T2-W2 | 1–2 | per dependent row | none |
| T2-W3 | 1–2 | routed | none (routing) |
| T2-W4 | 1 | none | none |
| T2-W5 | open-ended, 2 packages concurrent | real | per-package claims |
| T2-W6 | 1 | none | none |
| R1 | 1–2 | extraction tooling runs | D22 line |
| R2 | 1–2 | translation layer scaffold | none |
| R3 | 1–2 | calibration | none |
| R4 | 1 + per release | ritual scripts | none |
| R5 | 1 | metadata | none |
| T2-GOV | between waves | none | certification lines |
| T2-GOV2 | 1 | none | none |

## E.5 Claim-row template (drain waves)

```
claim-w2b-t2w<nn>-<date>
owner: <builder>
wave: T2-W<nn> per Seal-steps/ashfall-wave2-successor-corpus-
  tranche-two-and-ship-readiness-program-2026-09-19.md Part C
head certification: <log citation>
rows: <list with classifications>
verify: <per-row citations + any focused runs>
status: ACTIVE
```

Ship tracks use the same shape with the track ID.

## E.6 Concurrency

- One drain wave active at a time (waves are serial by certification).
- Inside T2-W5: max two packages, disjoint claims.
- Ship tracks: R1/R2/R3 are serial among themselves (R2 needs R1; R3
  needs R1); R4/R5 can overlap the drain freely.
- The active programs (companion, EN, W2A) keep their own concurrency
  rules; the drain never claims a footprint they own.

## E.7 Timeline sketch (gate-ordered)

1. T2-W1 immediately (ledger work only).
2. T2-W4/W2 as the companion waves land their inputs.
3. T2-W3 as W2A heads sign.
4. R1 whenever the foreman declares the freeze; R2/R3 follow.
5. T2-W5 fills builder capacity between release events.
6. R4's first tagged release → R5.
7. T2-W6 + T2-GOV2 close the program (G.5).

---

# Part F — The Signature Situation

## F.1 The drain needs no new signature

Every AUDIT-PENDING row's authority already exists — its plan (if open),
its owner (if sealed-elsewhere), its retirement rule (if consumed), its
terminal rule (if terminal). The drain executes recorded decisions under
the integrator's wave-head certifications (B.4). This is deliberate: the
chain's earlier programs already asked for every *new* decision the
corpus needs; Program B is the queue's tail, not a new decision surface.

The two exceptions route to their owners: a genuinely-open row whose
premise audit reveals a *new* architectural decision (Rule 10 stop →
foreman), and any row whose execution would need a W2A-gated input
(defer to that gate).

## F.2 The one declaration that sequences the ship tracks

`D22 string freeze: [declared, scope+date / deferred]` — Program A's
W2A-22 line. Declared: R1 starts immediately and R2/R3 un-park. Deferred
again: the ship tracks hold and the drain proceeds alone (the program
degrades gracefully; it never pressures the declaration).

## F.3 Certification lines (integrator-recorded, per wave)

```
T2-W<nn> head certified: <row> — probes green, footprint clear.
T2-W<nn> closed: nonterminal <n> → <m>, all flips cited.
```

These are records, not requests — the certification *is* the gate
(B.4).

---

# Part G — Verification, Rollback, Out of Scope, Handoff

## G.1 Verification pattern

**Drain waves:** per row, the B.3 audit's probes (read-only) + the
focused run for any row whose classification claims a passing suite +
the census diff showing exactly the wave's flips. **Ship tracks:** the
track's acceptance question (D) + the underlying skill rails' own gates
(snapshot diffs, loudness reports, export smoke). All runs respect
TEST_POLICY: focused-first, 180-second cap, no full-suite defaults, no
broad diagnostic runs without a named hypothesis.

## G.2 Rollback

Ledger work: flips are reversible by citation (the wave log records
before/after). Packages spawned from T2-W5 follow their own plans'
rollback sections. Ship tracks: extraction catalogs and translation
layers are additive artifacts; a wrong freeze date is a record edit; the
loudness calibration is a config revert. Nothing in Program B writes a
save migration, a data migration, or an irreversible deletion (terminal
rows are annotated, never destroyed).

## G.3 Out of scope

- Any new gameplay system (the drain executes or retires only).
- Translation *content* (R2 builds rails; content is post-program).
- VO *generation* (R3 un-defers the gate; generation is a content wave
  after it).
- The 8 companion plans / EN proposals / W2A packages (their programs).
- Census rows whose footprints overlap active claims (deferred, B.2).

## G.4 Final handoff

At exit: the census's nonterminal set is empty or explicitly carries
rows with named gates; every flip since 2026-09-19 is citable; the
release ritual has produced at least one tagged release (if the D22
chain fired); the INDEX registers the whole chain; and the handoff
names every shared path touched and every file intentionally untouched.

## G.5 Exit criteria

The program exits when all of: (1) census terminal/SEALED/RETIRED
columns sum to 131 with zero AUDIT-PENDING; (2) the ship tracks are
either done or explicitly blocked on a recorded declaration; (3) one
tagged release exists or the release path is documented as awaiting the
freeze; (4) T2-GOV2 registered the chain. At that point the repository
has no unaudited plan corpus, no unexercised release rail, and no
silent decision debt — the three things this chain set out to end.

## G.6 The empty-corpus end state

When the drain finishes, the census becomes what it was always meant to
be: a historical index of everything the repository ever planned, each
row ending in SEALED (shipped), RETIRED (evidence-closed), or TERMINAL
(annotated). New work stops entering the census and starts entering the
*package* genre directly — premise-audited, claimed, gated, closed.
The 2026-09-19 chain then reads, end to end, as the documented
transition of ASHFALL's planning culture from *accumulation* to
*drainage*: the audit found the queue's truth, the companion program
executed its frontier, the EN/W2A programs pre-built its decisions,
and this program emptied its body while building the road out the
door.

## G.7 Long-horizon view (after the empty corpus)

Post-exit, the repository's planning culture changes shape permanently:
new work enters as packages (premise-audited, claimed, gated, closed)
rather than corpus rows; the census becomes a read-only historical
index; the release ritual runs on cadence; and the localization/VO
rails stand ready for content decisions. The successor questions are
content-shaped (what to translate, what VO to generate, what the next
expansion is) rather than debt-shaped — which is the structural goal
of the entire 2026-09-19 chain.

## G.8 Metrics review (the foreman's cadence)

The I.38 dashboard reviewed per wave closeout; any red cell escalates
per its mitigation. The two cells that matter most: AUDIT-PENDING
monotonicity (the program's visible heartbeat) and decision-debt-zero
(the program's integrity guarantee). Everything else is efficiency,
not correctness.

---

# Part H — Evidence Index

| Claim | Evidence |
|---|---|
| Census counts | 131 files · 33 SEALED · 113 AUDIT-PENDING · 2 READY-UNCLAIMED (2026-09-19 read; anchor flips pending) |
| Anchor verification | 2026-09-19 audit §2 (C2[9]–C2[13] executed; flip pending) |
| Plans 37/48 dependency sets | corpus headers: Plan 37 → 17B/25A/25B/16A/31B/28A; Plan 48 → 29A/39A/46A/46B/47A/47C |
| Export smoke rails | headless export presets; PCK data authority; smoke-boot (ashfall-export-build skill) |
| Data integrity gate | `CatalogIntegrityValidator`; `--data-integrity-selftest`; ~280-file authority |
| Snapshot rails | 69 golden panels + diff tooling (ashfall-snapshot-diff) |
| Localization deferral | DEC-13 (register): "deferred until UI string extraction and string freeze" |
| VO deferral | DEC-11 (register): "deferred until dialogue string freeze and audio bus loudness calibration" |
| Rulebook sync | `scripts/ci/sync-agent-rulebooks.py` + `--check`; 13 clients |
| Forbidden-path stragglers | `src/Audio/AudioCueCatalog.cs:407`; `src/Main.FlagshipInstitutions.cs:45` |
| Ledger residuals | audit's flagged set: anchor flips, DEC-01/16, DEC-05 drift, merchant-restock row |

## H.1 Reproducible probes (read-only, run at any certification)

```
grep -c "AUDIT-PENDING\|READY-UNCLAIMED\|SEALED" docs/plans/UNCLAIMED_CORPUS_CENSUS.md
python3 scripts/ci/sync-agent-rulebooks.py --check
python3 scripts/ci/generate-port-contract.py --check
bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/CatalogPathForbiddenGateTests.cs
godot --headless --path . -- --data-integrity-selftest
grep -n "res://assets/StreamingAssets" src/Audio/AudioCueCatalog.cs src/Main.FlagshipInstitutions.cs
```

Baselines at publication (2026-09-19): the §A.1 counts; sync `--check`
clean; port contract 262/180/0; forbidden-path gate 2/2; two lowercase
straggler hits. Any drift is a premise change — record before
certifying a wave against it.


---

# Part I — Appendices

## I.1 The audit-grade premise template (every T2-W5 row, worked)

A genuinely-open row's premise audit produces this record before any
package forms. Template with a filled example (shape only — the actual
row is certified at wave time):

```
ROW: <census id> — "<plan title>"
1. Blocker check:   named blocker "<X>" — EXISTS / GONE (evidence: seam grep)
2. Owner check:     named owner "<O>" — still owns / moved to <O'> (evidence)
3. Sealed-elsewhere: capability shipped by <canonical owner>? NO (else: retire)
4. Save path:       section family "<S>" — current shape matches? (round-trip probe)
5. Test file:       <file> — exists? focused run result
6. Player payoff:   "<one sentence of observable gameplay>"
VERDICT: OPEN → package (companion genre) / flip / retire — with citation
```

The six answers are the package's Part A. A row whose answers disagree
with its plan's own premises (e.g. the plan assumed an owner that has
since moved) is *redrafted*, not executed — the redraft is the
premise-audit's output, and it follows Rule 10 to the foreman if it
needs a new decision.

## I.2 Worked classifications (one per stratum, from the chain's own record)

**Sealed-elsewhere residual** — the XP-02 row: the plan's premise
(neighborhood pressure system) shipped through canonical owners; the
audit's evidence (`WastelandMapSystem.PlanRoute` and the needs owners)
retires the premise. Verdict: SEALED-RESIDUAL flip citing the canonical
owner + seam. This is the modal row — expect the majority of
AUDIT-PENDING to close this way.

**Executed-unrecorded** — the anchor pattern (C2[9]–C2[13]): work landed
in a prior wave's commits; the audit verified the outcomes; the flip
cites the verification. Wave T2-W1's core work.

**Partial-by-design** — the amputation row: the expedition half is
sealed with a real signature; the equipment half is W2A-G-02, gated on
S8/D12. Verdict: flip PARTIAL citing the gate; the row fully seals when
G-02 closes.

**Genuinely-open** — the Plan 37/48 class: dependency-unblocked,
premise-auditable, execution-grade plans already written (companion
Plans 07/08). Verdict: execute; flip on closeout.

**Premise-consumed** — the XP-03 row: premise consumed by the C1
completion. Verdict: RETIRED with the canonical-owner citation.

**Terminal candidate** — a superseded audit doc's row whose payoff
question answers "nothing, ever" because a later authority renders the
plan moot. Verdict: TERMINAL with annotation (never silent deletion).

## I.3 R1 worked specification — the string extraction inventory

The extraction pass (D22-declared) inventories user-facing strings in
strata, each with its own probe:

| Stratum | Probe | Output |
|---|---|---|
| Panel/UI labels | grep UI tree for literal strings; snapshot battery as visual ground truth | catalog section `ui` |
| Data-authored text (items, quests, encounters, radio) | JSON sweep of the ~280-file authority; schema-known text fields | catalog section `data` |
| Diegetic prose (notes, journals, logs) | authored-content sweep (the write skill's corpus map) | catalog section `prose` |
| Toasts/feedback | event-bridge sweep for host-emitted strings | catalog section `feedback` |

Rules: the catalog is a *generated artifact* (never hand-edited; the
owning script's `--check` mode gates drift, mirroring the rulebook
sync); every entry carries its source file:line; the freeze date makes
*new* hardcoded strings review blockers (the localize skill's gate);
the catalog's coverage number (entries / estimated total) is R1's
acceptance metric. The pass never translates anything — extraction
only, so the freeze is cheap to declare and cheap to maintain.

## I.4 R2 worked specification — the translation layer scaffold

- **Layer:** Godot's translation API over the extraction catalog
  (generated `.translation` resources from the catalog's source
  strings; the catalog is the single authority — the layer never gains
  its own string table).
- **Locale selection:** a settings seam (the existing settings owner),
  persisted in the existing preferences path — no new save family.
- **Fallback:** missing translation → source string (never a crash,
  never a placeholder token in the UI).
- **Gate:** post-freeze hardcoded strings fail a review check (the
  localize skill's discipline; a small CI-side grep over the UI tree
  for catalog-absent literals).
- **Acceptance:** headless locale switch shows the layer serving the
  catalog; snapshot diff under the source locale is byte-stable
  (localization must not regress the source experience).

## I.5 R3 worked specification — the loudness calibration

- **Inventory:** the audio-qa rails' cue catalog audit (event bridge
  wiring, orphan detection, format policy).
- **Calibration:** bus-level loudness normalization to a single
  authored target (the cue catalog's existing normalization rails); the
  report records pre/post measurements per cue family.
- **Un-defer condition:** DEC-11's recheck trigger — "dialogue string
  freeze and audio bus loudness calibration" — both satisfied (R1 + this
  track), recorded in the register by the R3 closeout.
- **Generation itself** stays out: the gate opens; content waves decide
  what walks through it.

## I.6 R4 worked specification — the tagged-release ritual

```
pre-release gate (every release):
  1. focused suites for everything changed since the last tag
     (the closeout rows name them — no full-suite default)
  2. --data-integrity-selftest
  3. asset gate (registry + LFS policy)
  4. export presets Linux/Windows; PCK data-authority check; smoke-boot
  5. snapshot battery for touched panels
ritual:
  6. version bump per the documented scheme
  7. changelog from git history (categorized, provenance-kept)
  8. lane discipline: snap + export on a release lane, never main
  9. tag; publish artifacts; record the checklist in the release log
```

The ritual is the release-captain rails assembled into a runnable order;
its first execution (the R4 acceptance) proves the order, not just the
parts. Every subsequent release reuses it unchanged.

## I.7 R5 worked specification — the store-facing checklist

Grounded-in-evidence only: content descriptors answered from actual
data sweeps (violence/horror/gambling themes verified against the
narrative JSON, not assumed); platform smoke matrix from the export
presets actually built; first-access experience from the
tutorial-review rails' first-hour audit; store copy from the game's own
systems (economy, needs, chronicle) as verified by the chain. Any
checklist line that cannot cite evidence is marked *aspirational* and
excluded — the store page ships only grounded claims.

## I.8 Consolidated test matrix

| Unit | Focused verification | Pinned | Round-trip | Determinism |
|---|---|---|---|---|
| T2-W1/W4/W6 | census diff + citations | ledger rows | — | — |
| T2-W2 rows | per-row probes + suites | regional suites | per plan | per plan |
| T2-W5 packages | companion-genre per plan | per plan | per plan | paired-seed where stateful |
| R1 | extraction script `--check` | catalog coverage | catalog regen idempotent | — |
| R2 | locale-switch headless | source snapshots byte-stable | preferences path | — |
| R3 | loudness report | cue catalog, orphan count 0 | — | — |
| R4 | the 5-step gate | export presets, smoke-boot | — | — |
| R5 | smoke matrix | checklist citations | — | — |

## I.9 Risk register

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Drain becomes silent bulk-sealing | medium | high | B.2's never-list; per-row citations; audit spot-checks |
| Wave footprint overlaps an active claim | medium | medium | certification checks overlap before start (B.4) |
| R1 freeze stalls T2-W5 string-bearing packages | high | low | E.2 interleaving: queue or route via catalog |
| Extraction catalog drifts from source | medium | medium | generated artifact + `--check` gate |
| Translation layer becomes a second string authority | low | high | catalog is sole authority (I.4) |
| Release ritual drifts per-release | medium | medium | the ritual is versioned; changes are review-visible |
| Census math disputes (counts drift mid-drain) | high (observed) | low | every wave log records before/after; GOV reconciles |
| T2-W5 packages reintroduce decision debt | low | high | Rule 10 stop; premise audits surface new decisions to the foreman, never improvised |

## I.10 Failure playbook (Program B edition)

| Failure | Early signal | Containment |
|---|---|---|
| Premise skipped (row classified without probes) | audit spot-check finds no probe record | reclassify; the wave log row is void |
| Flip without citation | census diff shows bare status change | revert; re-flip with citation |
| Parallel queue (a wave log acting as a ledger) | ledger/wave disagreement | the census wins; wave log corrected |
| Catalog hand-edit | extraction `--check` fails | regenerate; hand-edits are void |
| Locale fallback crash | headless locale-switch run fails | fallback is source string; fix the layer |
| Loudness regression post-calibration | cue report delta | config revert; re-run report |
| Ritual shortcut (a release skipping a gate step) | release log missing a step | the release is not shippable; re-run the gate |
| Starvation (drain or ship track stalls for cycles) | metrics I.11 | E.2 interleaving review; foreman arbitrates |

## I.11 Program metrics

| Metric | Healthy | Drift trigger |
|---|---|---|
| AUDIT-PENDING count | monotone non-increasing | any increase |
| Flips-per-citation | 1:1 | any bare flip |
| Wave cadence | one wave per certification cycle | two idle cycles |
| Catalog coverage | rising to plateau | drop without regeneration |
| Release gate completeness | 5/5 steps | any skip |
| Decision debt created by the drain | 0 | any Rule-10 improvisation |

## I.12 Skill cross-reference

| Phase | Skill |
|---|---|
| Wave premise audits | `ashfall-analyze` |
| T2-W5 packages | `ashfall-implement` |
| Corpus classification sweeps | `ashfall-scan` / `ashfall-docs-atlas` |
| R1/R2 | `ashfall-localize` |
| R3 | `ashfall-audio-qa` |
| R4 | `ashfall-release-captain` / `ashfall-export-build` |
| R5 first-access | `ashfall-tutorial-review` |
| Snapshot verification | `ashfall-snapshot-diff` |
| Repo hygiene for T2-W6 | `ashfall-repo-hygiene` (annotate, never destroy) |

## I.13 Anti-pattern table (Program B edition)

| Anti-pattern | Correct reading |
|---|---|
| "113 pending rows = 113 packages" | the modal row is done or dead; the drain's mass is forensic, not build |
| "Seal everything the sweep matches" | every flip needs its own citation; pattern-matching is not evidence |
| "The wave log is the new census" | the census is the authority; wave logs are working documents |
| "Localization can start before extraction" | R2's gate is R1's catalog; strings before extraction create a second authority |
| "The release ritual is overhead; ship the binary" | the ritual's 5 gates are the difference between a green tree and a shippable artifact |
| "Terminal rows can be deleted" | annotate; never destroy (the repo-hygiene rule) |
| "The drain can wait for all programs to finish" | T2-W1/W4 are safe today; waiting breeds the next 117-row backlog |
| "A wave can audit around an active claim" | defer the row; footprints never overlap claims (B.2) |

## I.14 Glossary

- **AUDIT-PENDING** — a census row awaiting premise audit; not
  necessarily un-built.
- **Certification** — the integrator's recorded wave-head gate (B.4).
- **Drain** — the tranche-2 program: rank → audit → classify → act →
  closeout, per row.
- **Extraction catalog** — the generated inventory of user-facing
  strings; localization's single authority.
- **Freeze** — the declared date after which new hardcoded strings are
  review blockers (D22).
- **Mini-rerank** — the EN-08 ranking protocol, run before each wave.
- **READY-UNCLAIMED** — audited, unblocked, unclaimed row.
- **Ship-readiness track** — one of R1–R5; pipeline/presentation work,
  never gameplay authority.
- **Terminal row** — a census row annotated out of execution (superseded
  or payoff-free); never deleted.
- **Tranche-2** — the census body after tranche-1 (the sealed anchors
  and the audit's frontier).

## I.15 Governance interaction (how this program reads the rules)

- **Rule 3 (data authority):** the extraction catalog is generated *from*
  the data authority and the UI tree — it never becomes a second place
  strings are authored.
- **Rule 5 (one authority):** the census stays the corpus authority;
  the translation layer never owns strings; the release ritual never
  owns test truth (it *runs* the owning suites).
- **Rule 6 (no racing):** certification checks claim footprints; rows
  overlapping active claims defer.
- **Rule 7 (current evidence):** B.3's six questions; a row is never
  classified on its plan's say-so.
- **Rule 8 (focused verification):** every wave's runs are named and
  bounded; the release gate aggregates *named* suites, never
  "everything."
- **Rule 10 (stop when authority is missing):** a premise audit that
  surfaces a new decision stops at the foreman — the drain creates
  zero decision debt by construction.

## I.16 Worked example: one T2-W1 row, audit to flip

The reference shape for a residual flip (the wave's modal work):

1. **Pick:** the certification names the row (say, an auditee whose
   plan references a subsystem the 2026-09-18/19 wave sealed).
2. **Probe:** grep the row's named owner/seam; run the owning suite
   focused; confirm the outcome the row's plan promised is observable
   (selftest or panel battery).
3. **Classify:** the six questions — blocker GONE (the seal), owner
   current, sealed-elsewhere NO (this row's own work landed),
   save path current, test file passing, payoff present.
   Verdict: EXECUTED-UNRECORDED.
4. **Flip:** the census row's status changes with a citation line —
   `<date> SEALED — executed in <wave>; evidence <probes + suite
   result>; see <closeout doc>`.
5. **Closeout:** the wave log's row entry; the nonterminal count drops
   by one; the next head is certified.
6. **Rollback:** if a spot-check later disputes the flip, the citation
   is withdrawn, the row reopens, and the dispute is audited properly —
   a wrong flip is reversible *because* it was cited.

## I.17 Worked example: the first tagged release (R4's acceptance, narrated)

1. The pre-release gate assembles the changed-since-last-tag closeout
   rows (the chain's packages record them); each row's focused suite
   runs green; results land in the release log.
2. `--data-integrity-selftest` passes over the ~280-file authority.
3. The asset gate (registry + LFS policy) passes.
4. Exports build for both presets; the PCK check confirms the JSON
   authority inside; both binaries smoke-boot headless.
5. The snapshot battery re-renders touched panels; diffs are clean.
6. Version bumps; the changelog generator produces categorized history
   with commit provenance; the release lane snaps and exports (never
   main).
7. The tag lands with the checklist recorded — and the ritual is now
   *proven end-to-end*, not just assembled.

## I.18 Determinism and save interaction (what Program B never touches)

No drain wave, ship track, or governance package writes a save section,
a migration, or a seeded-stream change. The one persistence-adjacent
artifact is R2's locale preference, riding the existing preferences
path. The determinism guards (paired replays, checksum culture) are
*used* as verification rails by waves and tracks, never modified. If a
T2-W5 package needs stateful work, it carries its own determinism
analysis per the companion genre — this program's rails add nothing on
top.

## I.19 Chain integrity (the six-document rule, restated)

The chain's documents must stay mutually consistent on: the census
counts (this §A.1), the 8-plan frontier (audit/companion), the
signature queues (EN Part F, W2A Part F), and the drain/ship state
(this program's metrics). Any edit to one reconciles the others in the
same change; the live ledgers win all disagreements. This document's
reconciliable surfaces are §A.1, the wave shapes (Part C), and the
metrics (I.11).

## I.20 Per-wave definition-of-done tables

**T2-W1 — anchors/residuals.**

| DoD | Evidence |
|---|---|
| Nonterminal count reduced by the wave's row count | census diff, before/after in log |
| Every flip carries a citation | log row cites probe + suite + closeout |
| Ledger residuals reconciled (audit's flagged set) | register/INTEGRATION_PLANS rows updated owner-routed |
| No code changed | git diff shows docs/ledger only |

**T2-W2 — dependency-unblocked.**

| DoD | Evidence |
|---|---|
| Each dependent row's premise audit recorded | six-question template filled |
| Companion Plans 07/08 rows flip on closeout | their closeout docs cited |
| No row executed whose blocker still exists | blocker probes in every log row |

**T2-W3 — partials.**

| DoD | Evidence |
|---|---|
| Every partial names its remaining-half owner | routing table in log |
| Zero orphaned partials | routing completeness check |
| Gated partials cite their gates | W2A Part F line IDs |

**T2-W4 — retirements.**

| DoD | Evidence |
|---|---|
| Every retirement cites owner + seam + probe | one-grep re-verifiable |
| No retirement of a live-capability row | sealed-elsewhere test negative first |

**T2-W5 — open execution.**

| DoD | Evidence |
|---|---|
| Packages follow the companion genre | phases/gates/verify per plan |
| Max two concurrent, disjoint | claim review |
| Zero new decision debt | any Rule-10 stop is recorded, not improvised |

**T2-W6 — terminal consolidation.**

| DoD | Evidence |
|---|---|
| Terminal section matches archive | census ↔ archive diff |
| No row deleted | annotations only |
| 131-row total preserved | count invariant |

## I.21 Per-track definition-of-done tables

**R1.** Freeze date recorded; catalog generated with coverage number;
`--check` clean; new-string gate active.

**R2.** Locale switch works headless; source-locale snapshots
byte-stable; fallback is source string; catalog is the only string
table.

**R3.** Cue catalog audited (zero orphans); loudness report pre/post;
DEC-11 un-defer recorded with both triggers satisfied.

**R4.** One tagged release with the 5-step gate fully checked; ritual
doc versioned; lane discipline observed (snap/export off main).

**R5.** Store checklist every line evidence-cited or marked
aspirational-and-excluded; smoke matrix from real presets; first-access
audit integrated.

**T2-GOV.** Census ↔ ledger byte-consistency pass; every wave
certification recorded; metrics current.

**T2-GOV2.** INDEX registers the five-document chain; superseded docs
archived with supersede links.

## I.22 Corpus economics (why the drain converges)

The strata shares (A.2) make the drain convergent rather than
open-ended: if the modal row is sealed-elsewhere or executed-unrecorded
(the audit's own counting suggests most are), then each wave's *audit*
work is O(rows) but its *build* work is O(genuinely-open only). The
open stratum is fronted by the READY-UNCLAIMED and
dependency-unblocked rows (small by the census's own accounting), and
the terminal/retirement strata are evidence-only. The worst case —
113 genuinely-open rows — is excluded by the census's own history: the
2026-09-18/19 waves sealed a large fraction of the corpus's
dependencies, and the audit verified the pattern (two of the last
dependency-blocked rows cleared purely by sealing elsewhere).

Convergence guard: if two consecutive waves find their
genuinely-open share rising instead of falling, the integrator halts
the drain and audits the ranking — a rising open-share means premise
audits are being skipped (the I.10 first row), not that the corpus
grew.

## I.23 What Program B is not

- Not a content program: no new quests, factions, items, or systems —
  the drain executes or retires; the ship tracks build pipeline.
- Not a re-audit of the 2026-09-19 chain: its verdicts stand; this
  program builds on them.
- Not a translation or VO commitment: R2/R3 build rails and open
  gates; content decisions follow.
- Not a census replacement: the census is the authority; every wave
  log defers to it.
- Not a release promise: R4/R5 execute when the D22 chain fires; if it
  never does, the drain still completes and the program exits on its
  drain-side criteria alone (G.5's either/or).

## I.24 Reader's map

**The foreman** reads §0, B.4 (certification — the integrator's gate,
possibly the foreman acting as one), F.2 (the one declaration), and
G.6 (the end state). **The builder** reads one wave's Part C shape +
the audit template (I.1) + E.5 (claim row). **The integrator** reads
B.4, E.3, I.11, G.5 — and runs the H.1 probes before every
certification. **A sweep agent** reads B.3 and spot-checks wave logs
against the census (read-only). **A future auditor** reads I.19 (chain
integrity), Part H, and I.25 (self-check): every claim re-verifies
with the probes in minutes.

## I.25 Document self-check (performed)

- [x] Wave set covers every stratum in A.2 — none unowned.
- [x] No wave or track adds gameplay authority, save sections, or
      migrations (I.18).
- [x] Every named head cites 2026-09-19-verified evidence; unverified
      rows enter only through their wave's audit (the honesty note).
- [x] The drain needs no new signature; the ship tracks need exactly
      the D22 declaration (F).
- [x] The 131-row total is preserved throughout; terminal rows are
      annotated, never deleted.
- [x] Zero production changes; zero claims; zero ledger edits by this
      document.

## I.26 Wave deep specifications (execution detail behind Part C)

### T2-W1 deep spec

**Phases.** P0 certification (H.1 probes + residual list assembled from
the audit's flagged set and the census's stale-count candidates) →
P1 per-row audit (I.16's loop; batched by ledger file) → P2 flips
(owner-routed, one ledger file per claim footprint) → P3 closeout
(census diff, count delta, next head certification).

**File impact.** `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` (status
column), the flagged ledger rows (INTEGRATION_PLANS/KNOWN_DEBT/register
— integrator-owned), the wave log (new, `docs/plans/w2b/`). No source,
no data, no tests.

**Risk table.**

| Risk | Mitigation |
|---|---|
| Flip dispute (spot-check disagrees) | citation withdrawal protocol (I.16 step 6) |
| Residual list incomplete | the GOV reconciliation catches strays at wave end |
| Ledger edit conflicts with in-flight integrator edits | P2 is owner-routed; the builder never edits shared ledgers directly |

### T2-W2 deep spec

**Phases.** P0 certification → P1 dependency probes per row (blocker
existence in source) → P2 premise audits (six-question) → P3
classification routing (execute/flip/defer) → P4 execution of routed
packages (companion genre, one claim each) → P5 closeout.

**File impact.** The wave log; census status; per-executed-row: the
row's own plan's file impact (already authored in the corpus).

**Risk table.**

| Risk | Mitigation |
|---|---|
| Dependency edge stale (corpus header wrong) | probe in source; the Plan 37/48 lesson (headers declare wider sets than census edges) |
| Executed row's plan outdated mid-wave | redraft per I.1; Rule 10 stop if a new decision surfaces |
| Footprint overlap with companion waves | certification overlap check; defer |

### T2-W3 deep spec

**Phases.** P0 certification → P1 partial inventory (remaining-half
enumeration per row) → P2 routing table (owner per half: gated line /
active claim / new scoped package) → P3 orphan resolution (any orphaned
half becomes a scoped package or a Rule-10 stop) → P4 closeout.

**File impact.** Wave log; census partial-status annotations; routed
package claims where spawned.

**Risk table.**

| Risk | Mitigation |
|---|---|
| Orphaned half silently dropped | routing completeness check is the DoD |
| Gate cited but gate later declined | the row reopens via the chain-integrity rule |
| Double-owning a half (two programs claim it) | certification overlap check; W2A/EN queues are the tiebreaker record |

### T2-W4 deep spec

**Phases.** P0 certification → P1 sealed-elsewhere tests (canonical
owner + seam grep per row) → P2 retirement citations → P3 flips →
P4 closeout. Evidence-only; no code ever.

**Risk table.**

| Risk | Mitigation |
|---|---|
| Retiring a live capability | the sealed-elsewhere test must be positive-first (I.20-T2-W4) |
| Citation rot (seam moves later) | one-grep re-verifiability requirement; GOV spot-checks |

### T2-W5 deep spec

**Phases.** Per package, the companion genre verbatim: P0 premise →
P1..Pn phased gates → closeout. The wave adds only the ranking entry
gate and the two-package concurrency cap.

**Risk table.**

| Risk | Mitigation |
|---|---|
| Decision debt creation | Rule 10 stop recorded, never improvised |
| Package sprawl (row too big for one claim) | split per the companion program's own split discipline (e.g. the amputation halves) |
| Starvation by release tracks | E.2: release wins, package queues |

### T2-W6 deep spec

**Phases.** P0 certification → P1 terminal-candidate audit (payoff
question + supersession evidence) → P2 annotations (never deletions)
→ P3 terminal-section reconciliation → P4 program closeout (G.5).

**Risk table.**

| Risk | Mitigation |
|---|---|
| Deleting instead of annotating | repo-hygiene rule; the count invariant (131) fails loudly |
| Terminalizing a payoff-bearing row | the payoff question must answer "nothing, ever" with evidence |

## I.27 Track deep specifications (execution detail behind Part D)

**R1 phases.** P0 declaration recorded → P1 stratum probes (I.3's four
strata) → P2 catalog generation + coverage report → P3 `--check` gate
wired → P4 freeze enforcement active (new-string review blocker) →
closeout.

**R2 phases.** P0 (gate: R1 catalog) → P1 layer scaffold over the
catalog → P2 locale seam (settings owner, existing preferences path)
→ P3 fallback proof + source-snapshot byte-stability → closeout.

**R3 phases.** P0 cue-catalog audit (orphans, wiring, format policy) →
P1 loudness calibration to the authored target → P2 report (pre/post
per family) → P3 DEC-11 un-defer recording (both triggers) →
closeout.

**R4 phases.** P0 ritual doc assembled (I.6's order) → P1 first
execution end-to-end → P2 post-mortem: any step that needed
improvisation becomes a ritual fix, not a one-off → closeout (the
ritual is now proven).

**R5 phases.** P0 checklist drafting from evidence sources only →
P1 grounded-answer pass (each line cited) → P2 aspirational lines
excluded → P3 smoke matrix from real presets → closeout.

**Track risk table (consolidated).**

| Risk | Track | Mitigation |
|---|---|---|
| Freeze violated silently | R1/R2 | the new-string gate is CI-side, not convention |
| Catalog drift | R1/R2 | generated artifact + `--check` (the rulebook-sync pattern) |
| Second string authority | R2 | catalog is sole source; layer reads only |
| Loudness target disputes | R3 | target is authored data; report shows measurements |
| Ritual improvisation | R4 | post-mortem folds every improvisation into the ritual |
| Aspirational store claims | R5 | grounded-or-excluded rule |

## I.28 Certification failure modes (B.4's dark side)

| Failure | Detection | Recovery |
|---|---|---|
| Rubber-stamp certification (no probes run) | H.1 baselines absent from the log | certification void; wave re-certified |
| Head chosen by preference, not ranking | rank basis missing/unjustified | re-rank; log corrected |
| Overlap missed | claim conflict surfaces mid-wave | row deferred; wave continues on clean rows |
| Certification while ledgers in-flight | GOV finds disagreement | wave's flips reconciled by the integrator |

The certification is cheap, which makes it easy to fake. The H.1
baselines in the log are the anti-fake: they take five minutes to run
and are verifiable by any sweep agent.

## I.29 Drain projection table (planning arithmetic, not promises)

Given A.2's strata and the 2026-09-19 counting pattern, a *projection*
(not a commitment — the whole point is that audits decide):

| Wave | Rows (projected) | Sessions | Nonterminal after |
|---|---|---|---|
| T2-W1 | 8–15 (residuals) | 1 | ~100–105 |
| T2-W2 | 8–12 (dependent) | 1–2 | ~90–97 |
| T2-W3 | 6–10 (partials) | 1–2 | ~82–90 |
| T2-W4 | 10–20 (retirements) | 1 | ~65–80 |
| T2-W5 | open (per package) | multi-session | → 0 over packages |
| T2-W6 | remainder (terminal) | 1 | 0 |

The projection exists to make *stalling visible*: if two waves pass and
the count does not move by the projected band, the integrator's halt
rule (I.22's convergence guard) fires.

## I.30 Corpus history table (why the body is what it is)

The AUDIT-PENDING body is not laziness — it is the residue of a
deliberate strategy the repo has followed since the Godot migration:

| Era | What accumulated | What this chain does about it |
|---|---|---|
| Unity era | plans authored against a retired engine | premise audits retire them (T2-W4) with migration citations |
| Early Godot migration | plans for systems that later sealed through canonical owners | sealed-elsewhere residuals (T2-W1/W4) |
| Completion-first waves (2026-09-17/18) | partials intentionally split for review tractability | partial routing (T2-W3) |
| The 2026-09-18/19 sealing wave | dependencies cleared faster than ledgers flipped | executed-unrecorded flips (T2-W1) |
| The audit era (2026-09-19→) | the frontier executes; the body waits its turn | this program |

The history predicts the strata mix (A.2) and justifies the wave order:
the cheapest, most-certain strata drain first (W1), the evidence-heavy
ones follow (W4/W3), and real building (W5) lands last with the
smallest, best-audited set.

## I.31 Ship-readiness gap analysis (A.4's inverse, itemized)

| Gap | Current state | Closing track | Effort class |
|---|---|---|---|
| UI string inventory | strings exist inline; no catalog | R1 | tooling run |
| Translation layer | none (DEC-13 deferred) | R2 | scaffold |
| Locale persistence | settings owner exists; no locale field | R2 | one seam |
| Dialogue freeze | not declared (DEC-11 trigger) | R1/D22 | declaration |
| Loudness calibration | cues normalized per-family; no bus target | R3 | calibration + report |
| VO generation | gated (DEC-11) | R3 un-defers; content follows | gate only here |
| Release ritual | gates exist as skills/scripts; not ordered | R4 | ordering + first run |
| Changelog provenance | git history rich; no generator ritual | R4 | script + ritual |
| Store metadata | none | R5 | checklist |
| First-access audit | tutorial-review rails exist; not integrated | R5 | integration |

Every row closes with existing rails — the gap list contains no
invention, which is the whole reason ship-readiness is a *program* here
and not a research project.

## I.32 Audit quality rubric (what makes a B.3 audit "good")

| Dimension | Pass | Fail |
|---|---|---|
| Probe count | ≥ the six questions, each with a command or file:line | "the plan says so" |
| Recency | probes run this wave, results in the log | copied from a prior audit |
| Verifiability | a sweep agent can re-run each probe | citations to absent files |
| Classification fit | verdict follows from the answers mechanically | verdict contradicts an answer |
| Citation | flip/retire/execute each carry one | bare status changes |
| Payoff honesty | "player sees X" is specific or the row is terminal-bound | vague payoff language used to justify execution |

An audit that fails the rubric is not an audit — the row re-enters the
wave's queue (and repeated rubric failures on the same row escalate to
the integrator as a premise-audit-quality stop, not a forced verdict).

## I.33 What a builder may NOT infer from this document

- No authorization: waves execute under certification, packages under
  claims; this document confers neither.
- No row dispositions: only the named heads are evidenced; the other
  100+ rows await their audits (the honesty note, restated as a rule).
- No premise proof: §A is a 2026-09-19 snapshot; H.1 re-verifies.
- No precedence: the census and ledgers win disagreements.
- No scope elasticity: wave shapes are fixed; changing one is an
  integrator-routed amendment to this document, reconciled across the
  chain (I.19).

## I.34 Worked example: a T2-W5 package, from row to closeout

The reference shape for the one stratum that builds (shape only; the
actual row is certified at wave time):

1. **Certification:** the row ranks highest — dependency-unblocked
   (its blocker sealed 2026-09-19), payoff specific, rail complete,
   radius inside one claim.
2. **Premise audit (I.1):** six answers recorded; verdict OPEN; the
   row's authored plan survives with one premise correction (an owner
   moved) — the redraft is logged.
3. **Package:** the redrafted plan becomes a companion-genre package —
   phases, gates, focused verify, file impact, rollback, handoff —
   filed under a `claim-w2b-t2w5-*` row.
4. **Execution:** phases run with recorded gate results; the owning
   suites stay pinned; any new decision surfaces as a Rule-10 stop
   (recorded, escalated — never improvised).
5. **Closeout:** the package's evidence lands; the census row flips
   SEALED citing the closeout; the wave log's count drops; the next
   package (or wave) is certified.
6. **The full-loop proof:** a sweep agent can trace row → audit →
   claim → gates → closeout → flip with only the log and the census —
   no tribal knowledge anywhere in the chain.

## I.35 Per-unit verification recipe (the one-loop rule, W2B edition)

```
Drain row:   probes (B.3) + focused run if a suite is claimed + census diff
Drain wave:  H.1 baselines at certification + count delta at closeout
T2-W5 pkg:   companion genre's recipe verbatim
R1:          catalog --check + coverage number
R2:          locale-switch headless + source snapshots byte-stable
R3:          loudness report + orphan count 0
R4:          the 5-step gate, all results in the release log
R5:          checklist citations + smoke matrix
```

Closed under the program: no unit invents a broader run than
TEST_POLICY allows; every result is recorded before proceeding; a
compile-green (or export-green) result never substitutes for the unit's
named acceptance.

## I.36 Review cadence

| Cadence | Review |
|---|---|
| per row | audit record + citation |
| per wave | certification → closeout → next-head certification |
| per package (T2-W5) | phase gates + claim review |
| per track phase | acceptance question answered with evidence |
| per release | the 5-step gate + ritual post-mortem |
| program exit | G.5 criteria + chain reconciliation (I.19) |

## I.37 The declaration's scope options (D22, decision aid)

| Scope option | What it freezes | Cost | Un-parks |
|---|---|---|---|
| Full (UI + dialogue + data text) | everything player-facing | highest discipline; R1 runs all four strata | R2 + R3 |
| UI-first (panels + feedback) | interface strings only | moderate; dialogue still live | R2 only |
| Dialogue-first | narrative text only | moderate; VO can proceed, UI still drifts | R3 only |
| Deferred again | nothing | zero; ship tracks hold | nothing |

The program recommends **Full** at the first natural pause after the
companion program's D-waves land (the tree's string churn is then at a
local minimum), but every option is valid — the tracks degrade
gracefully (F.2). What is *not* valid is an undeclared freeze: a
half-maintained convention that blocks nothing and gates nothing.

---

# Part J — Foreman's One-Page Summary

**What this program is:** the queue's tail. The audit found the truth;
the companion program executes the 8-plan frontier; the EN and W2A
programs pre-build every remaining decision; **Program B drains the
other ~110 census rows and builds the road to release.**

**What you are asked to do:** almost nothing new.

- The drain needs **zero signatures** — waves run under the
  integrator's certifications (B.4), which are records, not requests.
- The ship tracks need **one declaration** you were already asked for
  (D22, the string freeze — scope options in I.37).
- Everything else is already authorized by the corpus's own recorded
  decisions.

**What you get, in order:** a shrinking census (visible every wave
closeout), zero silent seals (every flip cited), no starvation (drain
and release interleave by rule), and — if the declaration fires — a
tagged, checklist-proven release and store-ready metadata.

**What it costs you:** review of wave closeouts at your chosen cadence
(I.36), arbitration if a Rule-10 stop surfaces a genuinely new
decision (rare by construction), and the declaration itself.

**What it never does:** invents systems, races active claims, edits a
ledger without routing, deletes a row, or ships an uncited claim.

**How to start:** hand this document to the integrator and say
"certify T2-W1." Everything downstream is already written.

---

## I.38 Metrics dashboard (what the foreman watches)

| Metric | Source | Healthy shape |
|---|---|---|
| AUDIT-PENDING count | census | monotone ↓ to 0 |
| Flips with citations | wave logs | 100% |
| Wave cadence | log dates | one per cycle |
| Open-share trend (I.22) | wave classifications | ↓ or flat |
| Release gate completeness | release logs | 5/5 |
| Catalog coverage | R1 report | ↑ to plateau |
| Decision debt created | Rule-10 stops | 0 |

## I.39 Second anti-pattern table (process-level)

| Anti-pattern | Correct reading |
|---|---|
| "We can ship without the ritual; it's bureaucracy" | the ritual's gates are the only thing standing between a green tree and a player-visible regression |
| "The census is paperwork; just build" | the census *is* the queue's memory; abandoning it rebuilds the 117-row problem |
| "Certifications can be batch-signed monthly" | certification is the anti-fake gate (I.28); batching defeats it |
| "The strata estimates are commitments" | they are projections (I.29); audits decide |
| "T2-W5 can absorb scope creep from other programs" | the drain executes recorded decisions; absorbing scope recreates decision debt |
| "R5 can draft aspirational copy now, fix later" | grounded-or-excluded (I.7) — aspirational copy never ships |

## I.40 Glossary, second set (process terms)

- **Certification void** — a certification reversed for missing
  baselines (I.28); the wave re-certifies with probes.
- **Convergence guard** — the halt rule when the genuinely-open share
  rises two waves running (I.22).
- **Drain projection** — the I.29 arithmetic; a visibility tool, not a
  schedule.
- **Grounded-or-excluded** — R5's rule: every store-facing claim cites
  evidence or is excluded.
- **Halt rule** — the integrator's stop on suspicious drain patterns.
- **Interleaving contract** — E.2's anti-starvation rules.
- **Rubric stop** — an escalation for repeated audit-quality failures
  on one row (I.32).

## I.41 Provenance & chain closure

Authored 2026-09-19 as Wave 2 Program B, completing the
five-document chain:

1. `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` — the truth.
2. `Seal-steps/ashfall-eight-unblocked-plans-completion-first-execution-program-2026-09-19.md` — the frontier.
3. `Seal-steps/ashfall-enhanced-expansion-program-en-01-en-08-and-xp-pillar-rescoping-2026-09-19.md` — the proposals.
4. `Seal-steps/ashfall-wave2-decision-gated-chains-execution-program-2026-09-19.md` — the gated tier.
5. **This document** — the tail and the road out.

All citations verified against the worktree at HEAD `fc73a306` plus the
uncommitted 2026-09-18/19 session; no claim, ledger, code, data, or
test file was modified to produce any document in the chain. The chain
reads, end to end, as: *what is true → what executes now → what one
line each releases → what the tail drains → what ships.*

*End of Wave 2 Program B. Planning authority only: register in
`docs/INDEX.md` when the integrator next regenerates the index (the
index and live ledgers have in-flight edits as of 2026-09-19).*

## I.42 Worked example: one complete certification record

```
T2-W2 head certified: census row C2[16] — Plan 40 survey follow-on
  rank basis: dependency-unblocked (blocker Plan 39 sealed 2026-09-18;
    verified: <suite> 11/11) · payoff: survey results surface in the
    map panel · rail: owner + save + tests live · radius: one claim
  H.1 baselines (2026-09-19): counts 33/113/2 · sync --check clean ·
    port contract 262/180/0 · forbidden-path 2/2 · 2 straggler hits
  footprint: src/Exploration/ + docs/plans/w2b/T2-W2_LOG.md
  overlap: none with active claims (checked against
    WORKTREE_OWNERSHIP.md at certification)
```

Five minutes to produce, verifiable by any sweep agent, and the wave's
entire legitimacy rests on it — which is why it is the anti-fake gate.

## I.43 End-state appendix (the program's final page, pre-written)

When Wave 2 Program B exits, the closing handoff reads:

- Census: 131 rows — SEALED + RETIRED + TERMINAL = 131; AUDIT-PENDING =
  0; every flip since 2026-09-19 citable end-to-end.
- Chain: five documents registered in the INDEX; superseded planning
  docs archived with links; the live ledgers and the census
  byte-consistent (T2-GOV's final pass).
- Ship: the release ritual proven by at least one tagged release (or
  documented as awaiting the D22 declaration); store checklist grounded;
  localization and VO rails standing.
- Shared paths: every ledger touch owner-routed and listed; no file
  left intentionally unexplained.
- Successor state: new work enters as packages; the corpus era closes.

That page is the chain's destination. Everything between here and there
is already written down.

## I.44 Per-stratum probe cookbook (the B.3 questions, mechanized)

**Sealed-elsewhere residual:**
```
1. name the row's promised capability
2. grep the canonical owner tree for the capability's seam
3. run the owning suite focused
4. if seam + suite + observable outcome all present → flip with all three cited
```

**Executed-unrecorded:**
```
1. find the sealing wave's closeout (git log + closeout docs)
2. verify the row's named outcomes against current behavior (selftest/panel)
3. flip citing the closeout + the verification
```

**Partial-by-design:**
```
1. read the split record (the plan's or the debt row's split note)
2. verify each half's status independently (done half: evidence; open
   half: gate/claim/package)
3. flip-or-route; never mark a split row SEALED while a half is orphaned
```

**Genuinely-open:**
```
1. blocker probe (does the named blocker still exist in source?)
2. owner probe (grep the seam; confirm ownership unchanged)
3. save probe (round-trip the row's section family)
4. test probe (focused run of the row's suite if it exists)
5. payoff statement (one specific sentence)
6. → package (companion genre) or Rule-10 stop
```

**Premise-consumed:**
```
1. identify the canonical owner that shipped the premise's capability
2. cite the seam (one grep)
3. retire with both citations; never execute a consumed premise
```

**Terminal candidate:**
```
1. payoff question: "what does the player see if this executes?"
2. supersession evidence: the later authority that moots it
3. annotate TERMINAL with both; never delete
```

## I.45 Drain-wave claim review checklist

The integrator checks, per wave claim:

- [ ] certification record present with H.1 baselines (I.42 shape)
- [ ] every row's audit record answers all six B.3 questions
- [ ] classifications follow the rubric (I.32), not preference
- [ ] every flip/retire/annotation carries its citation
- [ ] census diff shows exactly the wave's rows, nothing else
- [ ] no source/data/test file touched unless a spawned package's claim
      covers it
- [ ] count delta recorded; next head certified
- [ ] no overlap violations (checked against WORKTREE_OWNERSHIP.md)

A claim failing any box returns to the builder with the box named —
never silently patched by the reviewer.

## I.46 Ship-track handoff templates

**R1 closeout:**
```
freeze: declared <scope> <date> (W2A-22 line cited)
catalog: generated <date> · coverage <n>/<est> · --check clean
gate: new hardcoded strings are review blockers (check wired <date>)
ledger: DEC-13 recheck trigger satisfied; un-defer routed
```

**R2 closeout:**
```
layer: scaffolded over the catalog (sole string authority)
locale: seam wired via settings owner; preferences path unchanged
proof: headless locale-switch run + source snapshots byte-stable
fallback: missing translation → source string (test cited)
```

**R3 closeout:**
```
cues: catalog audited · orphans 0 · format policy pass
loudness: calibrated to <authored target> · report pre/post per family
ledger: DEC-11 un-defer recorded (freeze + calibration both cited)
```

**R4 closeout:**
```
release: <tag> · 5-step gate results in release log
ritual: versioned at <doc> · post-mortem folded <n> improvisations
lane: snap/export off main · artifacts published
```

**R5 closeout:**
```
checklist: <n> lines · all evidence-cited · <m> aspirational excluded
smoke: matrix from real presets <list>
first-access: tutorial-review audit integrated
```

## I.47 Objections & answers (the FAQ the foreman will ask)

**"Isn't this just telling people to do the work they were already
going to do?"** — No: the drain's value is the *protocol*, not the
labor. Without it, 113 pending rows decay into the next 117-row
backlog; with it, each row has a gate, a rubric, and a citation
requirement that make silent accumulation impossible.

**"Why not just audit all 113 rows in one sweep?"** — Because a bulk
audit is a bulk seal (B.2's first never). The waves exist to keep every
flip reviewable and every footprint claimed. One session per wave is
the price of zero silent seals.

**"Why do ship tracks live here instead of their own program?"** —
Because drain and release share the anti-starvation contract (E.2) and
the exit criteria (G.5). Splitting them re-creates the classic failure:
an endless feature queue that never ships.

**"What if the census is wrong about a stratum?"** — Then a wave's
audits find out, row by row, with evidence. The strata are priors for
wave *design*, never verdicts for rows.

**"What if we want to add a new feature mid-drain?"** — It enters as a
package (the post-exit culture, early), not a census row. The corpus is
for the inherited backlog, not new ambition.

**"Who is the integrator?"** — Whoever holds the governance claim
(currently `claim-wave11-part2-execution-2026-09-18`'s owner, or the
foreman acting directly). The certification role is claim-based, not
person-based.

## I.48 Closing note

The 2026-09-19 chain began with a question — *what is actually
unblocked?* — and answered it with evidence. This document ends the
chain with the question's mirror: *what is actually left?* The answer
is a body of mostly-finished or mostly-dead rows, a handful of signed
decisions away from closure, and a short road of pipeline work between
the repo's green gates and a player's install. The program's job is to
hold that answer true until the last row flips: no skipping, no
silence, no starvation, no uncited claim — drain by evidence, ship by
ritual, exit by criteria.

## I.49 Worked wave log (T2-W1, fully simulated)

```
# T2-W1 log — 2026-09-2x
head certified: anchor residuals (5) + flagged ledger set
  baselines: 33/113/2 · sync --check clean · port 262/180/0
  · forbidden-path 2/2 · 2 stragglers · probes run this wave

rows:
  - C2[9]:  EXECUTED-UNRECORDED · <closeout doc> cites suite 11/11 ·
    action flip SEALED · citation <doc>:<line>
  - C2[10]: EXECUTED-UNRECORDED · same wave closeout · flip SEALED
  - C2[11]: EXECUTED-UNRECORDED · same · flip SEALED
  - C2[12]: EXECUTED-UNRECORDED · same · flip SEALED
  - C2[13]: EXECUTED-UNRECORDED · same · flip SEALED
  - register DEC-01: resolved-by-execution (D1) · row updated
    owner-routed · citation audit §2
  - register DEC-16: resolved-by-execution (D2) · row updated
  - register DEC-05: evidence drift 14→6 · row corrected ·
    citation: current ledger read
  - INTEGRATION_PLANS merchant-restock row: stale (work sealed
    2026-09-18) · row updated owner-routed · citation restock 6/6

closeout: nonterminal 117 → 112 (5 flips; ledger rows are not census
  rows) · all flips cited · no code/data/test changes (git diff
  docs-only) · next head: T2-W2 row C2[15]-dependent set
```

Every subsequent wave log is this shape with its own rows — the format
is the contract, and a log that cannot fill it is a wave that did not
happen.

## I.50 Document map (index to this document)

| If you want… | Read |
|---|---|
| the state of the corpus | §A.1–A.2 |
| why the waves are ordered this way | A.3, I.22, I.29 |
| the rules of the drain | Part B (all) |
| your wave's execution shape | Part C + I.26 |
| the ship tracks | Part D + I.27 |
| who flips what | E.3 |
| whether a signature is needed | Part F |
| the verification loop | G.1, I.35 |
| the end state | G.5–G.6, I.43 |
| the evidence | Part H + H.1 |
| the templates | I.1, C.7, I.42, I.46, I.49 |
| the objections answered | I.47 |
| the one-page version | Part J |

## I.51 Final self-check (published state)

| Check | Result |
|---|---|
| Every stratum has a wave; every wave has a protocol | A.2 ↔ C |
| Zero new signatures required for the drain | F.1 |
| One declaration (already asked) sequences the ship tracks | F.2 |
| No gameplay authority, save section, or migration anywhere | I.18 |
| Terminal rows annotated, never deleted | B.2, I.20-T2-W6 |
| Named heads evidenced; unnamed rows honestly deferred | the honesty note, I.33 |
| Chain-integrity surfaces named | I.19 |
| Production changes by this document | none |

## I.52 Audit quality, by example (good vs. void)

**A good audit record (row C2[17], shape only):**
```
1. blocker "Plan 42 audit" — GONE: Plan 42 sealed 2026-09-18
   (closeout cited; suite 9/9 re-run this wave)
2. owner "ExpeditionHostSession" — current: seam grep hits
   <file>:<line> today
3. sealed-elsewhere — NO: no canonical owner ships this capability
   (grep across Core for the feature's vocabulary: zero hits)
4. save path — round-trip probe passed on the expedition section
5. test file — exists, focused run green (n/n)
6. payoff — "expedition results appear on the map panel with per-leg
   provenance"
VERDICT: OPEN → package (companion genre); premise correction: plan's
  step 3 references an owner that moved; redraft logged.
```

**A void audit record (same row, done wrong):**
```
"Plan 42 is sealed so this row is probably fine — flip SEALED."
```

The second record is void on five rubric dimensions at once: no probes,
no recency, no verifiability, no classification fit (an OPEN row
flipped), no citation. The first is five minutes more work and is the
entire difference between a drain and a bulk seal.

## I.53 The drain's relationship to the census authority (restated precisely)

The census is the corpus's single source of truth. The drain *writes*
to it exactly one way: cited status flips, owner-routed, at wave
closeouts. It never restructures the census, never edits row prose,
never renumbers, and never creates rows (new work enters as packages).
If the census and a wave log disagree, the census wins and the log is
corrected — the reverse never happens. This one-way write discipline is
what lets 113 rows drain through many hands without the corpus
fragmenting into per-builder truths, which is precisely the failure
mode the pre-foreman era's 40+ planning documents demonstrated (and
the docs-atlas skill exists to clean up).

## I.54 Closing metrics tables (the last three numbers)

| When | The number that matters | Target |
|---|---|---|
| every wave closeout | AUDIT-PENDING | ↓ by the wave's count |
| every release | gate steps complete | 5/5 |
| program exit | rows with unresolved dispositions | 0 |

Everything else in this document exists to make those three numbers
honest.

## I.55 Audit speed table (why a wave fits in a session)

| Row class | Probes needed | Typical time | Bottleneck |
|---|---|---|---|
| sealed-elsewhere residual | 1 grep + 1 focused run | ~5 min | reading the closeout |
| executed-unrecorded | 1 closeout read + 1 selftest | ~5 min | finding the closeout |
| partial-by-design | 2 status checks | ~8 min | the split record's location |
| genuinely-open | 5 probes + redraft | ~30 min | the redraft |
| premise-consumed | 1 grep + citation | ~5 min | the seam citation |
| terminal candidate | payoff + supersession | ~10 min | supersession evidence |

A 12-row wave of mostly-cheap classes is ~1.5 hours of audit plus
closeout writing — one session with review, exactly as I.29 projects.
The genuinely-open class is the only expensive one, and it is
deliberately rare, fronted, and capped.

## I.56 The one-sentence versions (for anyone who reads nothing else)

- **The drain:** rank → audit → classify → act → closeout, per row,
  cited every time, no exceptions.
- **The certification:** five minutes of probes recorded in the log, or
  the wave does not start.
- **The ship tracks:** assemble existing rails in a fixed order; freeze
  strings before extracting; calibrate before generating; gate before
  tagging; ground every store claim.
- **The program's promise:** the census reaches zero honestly, and the
  release path is proven — or documented as awaiting one declaration.

*End of appendices.*

## I.57 Governance check matrix (rule by rule, how this program complies)

| Rule | Program's compliance |
|---|---|
| 1 — Godot authoritative | every track targets Godot rails; no engine ambiguity introduced |
| 2 — Core engine-free | the drain spawns Core-only packages; ship tracks touch tooling/host presentation only |
| 3 — JSON data authoritative | extraction catalog generated *from* the authority; authored targets stay in data |
| 4 — determinism/persistence preserved | I.18: no stream, migration, or section changes; guards used, never modified |
| 5 — one authority per concern | census sole corpus authority; catalog sole string authority; ritual runs suites, owns none |
| 6 — no racing | certification overlap checks; deferral over conflict |
| 7 — current evidence | B.3 probes, H.1 baselines, rubric I.32 |
| 8 — focused verification | every unit's recipe is named and bounded |
| 9 — no secrets | store metadata from repo evidence only; no external accounts invented |
| 10 — stop when authority is missing | the drain's zero-decision-debt construction; Rule-10 stops recorded, never improvised |

Compliance is not asserted — it is designed in: each rule's mechanism
above names the section that enforces it.

## I.58 Publication state (final)

Published 2026-09-19 alongside Wave 2 Program A as the closing document
of the five-part chain. Files created by the Wave-2 pair: this document
and `ashfall-wave2-decision-gated-chains-execution-program-2026-09-19.md`
— two new planning documents, nothing else. The chain's total
footprint across all five documents: one audit report, four program
documents, one AGENTS.md section update, thirteen synced client
rulebooks, one sync report. Zero production code, data, tests, claims,
or ledger rows modified by any of them.

Chain integrity note (the closing rule, I.19 restated one final time):
the audit's frontier, the companion's roster, the EN readiness table,
W2A's Part F queue, and this document's §A.1 counts name the same
repository state from five angles — any future edit to one reconciles
the others in the same change, or the chain loses its evidence basis
and the queue reverts to unaudited memory.
