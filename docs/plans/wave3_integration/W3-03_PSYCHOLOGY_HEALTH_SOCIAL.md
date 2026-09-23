# ASHFALL — WAVE 3 INTEGRATION PROGRAM · PLAN 3 OF 6

# PSYCHOLOGY, HEALTH & SOCIAL SYSTEMS INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W3 (six-plan integration wave)
**Document:** W3-03
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W3-01 (narrative), W3-02 (economy), W3-04 (combat), W3-05 (crafting), W3-06 (UI)
**Plan-unblocking annex:** Annex U at the end — separately.

---

## 0. How to read this plan

This plan integrates the **inner life** of the shelter: mental health, trauma,
therapy, morale, relationships, grief, caregiving, and social dynamics. It
extends existing owners; it never creates a second psychology ledger. It is
also the most **sensitive** plan in the program: every mechanic here is checked
against dignity rules, and no line of content is merged without the tone review
established in W2-06.

### 0.1 Two selection levels

| Plan Path | Name | Meaning |
|---|---|---|
| **A** | Truth & Safety | audit owners, catalogs, and consequence routing; remove any harmful or fabricated behavior |
| **B** | One Inner Life | unify psychology/relationship owners, make care legible, verify treatment effects |
| **C** | Living Shelter | social dynamics, arc-driven mental health, long-arc recovery and grief on existing owners |

**Level 2:** ten points, each A/B/C (§4.2).

### 0.2 Default mapping

| Plan Path | A points | B points | C points |
|---|---|---|---|
| A Truth & Safety | 1–10 | — | — |
| B One Inner Life | 1,2,9 | 3,4,5,6,7,8,10 | — |
| C Living Shelter | — | 2,4 | 1,3,5,6,7,8,9,10 |

### 0.3 The Wave 3 rule for this plan

> **One inner-life authority per concern.** Mental health stays
> `SurvivorMentalHealthSystem`/`SurvivorMentalHealthRecord`; therapy stays the
> sanatorium/catalog; trauma stays `CombatTraumaSystem`/trauma catalog; grief
> stays `RelationsGriefSink`/memorial; relationships stay the relations owners;
> morale stays its systems. No parallel "stress meter", no hidden depression
> stat, no second therapy list.

### 0.4 Dignity and tone rules (binding)

1. Mental illness is never a monster, a penalty flavor, or a punchline.
2. Recovery is possible but not guaranteed; treatment is care, not a buff.
3. No mechanic rewards self-harm, addiction, or cruelty.
4. Privacy: psychological state is displayed only where the player needs it to
   care for survivors; no gossip logs, no shame surfaces.
5. Medical/psych records remain free-text-free where the repository says so
   (`MedicalRecordLog` rule).
6. Every content tranche passes the W2-06 tone checklist.

### 0.5 Vocabulary

| Term | Meaning |
|---|---|
| mental health record | the per-survivor psych state owner |
| trauma | authored trauma state (combat/event) |
| crisis | an acute mental-health event |
| therapy | authored treatments at the sanatorium |
| morale | shelter/survivor morale systems |
| relationship | affinity/trust between survivors |
| grief | memorial/relationship-loss response |
| caregiving | support for injured/ill survivors |
| contamination | psychological contamination (maritime) |
| social dynamics | shelter-level social state |

---

## 1. Executive summary

ASHFALL already models the inner life with unusual care:

- **Mental health:** `SurvivorMentalHealthSystem`,
  `Needs/PsychologicalTraumaCatalog`, `psychological_trauma.json`,
  `SurvivorMentalHealthRecord` (read by `SleepNarrativeProjection`),
  `MentalHealthCrisisSystem`, `PsychologyAfflictionHandlers`.
- **Therapy:** `Sanatorium/PsychologicalSanatoriumSystem`,
  `PsychologicalTherapyCatalog`, `psychological_therapies.json`.
- **Trauma:** `CombatTraumaSystem`, `Survivors/TraumaBondSystem`,
  `Maritime/PsychologicalContaminationSystem`.
- **Arcs:** `PsychologicalArcSystem`, `mental_arcs.json`,
  `narrative_arc_events.json`.
- **Morale:** `MoraleContagionSystem` (+catalog/save), `VinylMoraleSystem`,
  `DutyRoster/MoraleMarkSystem`, `MoraleContagionHostSession`.
- **Relationships:** `RelationshipDecaySystem`, `SurvivorSocialCoordinator`,
  `SurvivorRelationsSystem` (last-interaction stamp debt RETIRED),
  `TraumaBondSystem`, `RelationsGriefSink`.
- **Social:** `ShelterSocialDynamicsSystem`, `shelter_social_events.json`,
  `CaregivingSystem`.
- **Narrative interface:** `PsychologicalArcSystem` + arcs data (W3-01
  consumes).

The gaps are integration and truth gaps:

1. **Owner map** — ten components touch mental state; the read/write graph is
   undocumented (Point 1).
2. **Record truth** — what the mental-health record stores and who mutates it
   is unverified; the wave-1 guidance (D19a-family static state) suggests
   checking statics and isolation here (Point 2).
3. **Trauma routing** — combat/event trauma must route through one catalog and
   produce recovery paths, not permanent hidden debuffs (Point 3).
4. **Crisis honesty** — crises should be foreseeable, treatable, and never
   silently fatal (Point 4).
5. **Therapy effects** — treatments must measurably help and cost time/supplies
   through owners (Point 5).
6. **Morale contagion** — the contagion system exists; bounds and legibility
   are the gap (Point 6).
7. **Relationships** — decay/trust/bonds interact; one relationship authority
   and readable effects (Point 7).
8. **Grief** — memorial/grief sink routing to morale/relations (Point 8).
9. **Caregiving** — care for the injured/ill: effort, outcome, and dignity
   (Point 9).
10. **Social dynamics** — shelter-level events and their consequences
    (Point 10).

---

## 2. Verified current state

### 2.1 Systems

| Concern | Owner(s) |
|---|---|
| mental health | `SurvivorMentalHealthSystem`, `SurvivorMentalHealthRecord` |
| trauma catalog | `PsychologicalTraumaCatalog`, `psychological_trauma.json` |
| crisis | `MentalHealthCrisisSystem` |
| affliction hooks | `PsychologyAfflictionHandlers` |
| therapy | `PsychologicalSanatoriumSystem`, `PsychologicalTherapyCatalog`, `psychological_therapies.json` |
| combat trauma | `CombatTraumaSystem` |
| trauma bonds | `TraumaBondSystem` |
| contamination | `Maritime/PsychologicalContaminationSystem` |
| arcs | `PsychologicalArcSystem`, `mental_arcs.json` |
| morale | `MoraleContagionSystem` (+ catalog/save), `VinylMoraleSystem`, `MoraleMarkSystem` |
| relationships | `RelationshipDecaySystem`, `SurvivorSocialCoordinator`, relations system |
| social | `ShelterSocialDynamicsSystem`, `shelter_social_events.json` |
| caregiving | `CaregivingSystem` |
| grief | `Memorial/RelationsGriefSink` |

### 2.2 Known constraints from earlier waves

- `SleepNarrativeProjection` (DEBT-177 RETIRED) reads the mental-health record;
  the journal dedup pattern applies.
- `MedicalRecordLog` stores day/kind/id only — **never free text**.
- Phobia growth is **RETIRED** (DEC-18 / XP-10); this plan must not revive it.
- `VinylMoraleSystem` is a morale owner; do not duplicate its effects.
- Static-state isolation risks apply to systems with static caches (W2-01/W2-02
  gates).

---

## 3. Scope, non-goals, rules

### 3.1 In scope

- Owner/read-write graph truth and static-state hygiene.
- Record semantics and mutation paths.
- Trauma routing and recovery.
- Crisis foresight/treatment/no-silent-fatal rule.
- Therapy effect verification and costs.
- Morale contagion bounds and legibility.
- Relationship authority and readable effects.
- Grief routing.
- Caregiving effort/outcome.
- Social dynamics consequences.

### 3.2 Non-goals

- Prose (W2-06).
- New psychology systems.
- Phobia growth (RETIRED), trait trees for trauma, or "madness" mechanics.
- Economy/combat/narrative mechanics (other W3 plans).
- Save schema without signature.
- Balance tuning (W2-03 bands).

### 3.3 Rules

1. One writer per state; others read.
2. Every psychological consequence has a **player-visible cause** (event,
   condition, treatment) — no hidden drift.
3. Recovery paths exist for every treatable condition; terminal conditions are
   explicit by design.
4. Determinism: crisis/therapy rolls seeded.
5. Dignity review per tranche.

---

## 4. Plan Path and decision index

### 4.1 The ten points

| # | Point | Default |
|---|---|---|
| 1 | Owner graph and mutation truth | B |
| 2 | Record semantics and static hygiene | B |
| 3 | Trauma routing and recovery | B |
| 4 | Crisis foresight and treatment | B |
| 5 | Therapy effect truth | B |
| 6 | Morale contagion bounds | B |
| 7 | Relationship authority and effects | B |
| 8 | Grief routing | B |
| 9 | Caregiving effort and outcome | A |
| 10 | Social dynamics consequences | C |

### 4.2 Selection sheet

```text
PLAN W3-03 — PSYCHOLOGY, HEALTH & SOCIAL
Plan Path: [ ] A Truth & Safety  [ ] B One Inner Life (default)  [ ] C Living Shelter

01 owner graph ............ [A] [B] [C]   default B
02 record semantics ....... [A] [B] [C]   default B
03 trauma routing ......... [A] [B] [C]   default B
04 crisis foresight ....... [A] [B] [C]   default B
05 therapy effects ........ [A] [B] [C]   default B
06 morale contagion ....... [A] [B] [C]   default B
07 relationships .......... [A] [B] [C]   default B
08 grief .................. [A] [B] [C]   default B
09 caregiving ............. [A] [B] [C]   default A
10 social dynamics ........ [A] [B] [C]   default C
```

---

## 5. Decision Point 1 — Owner graph and mutation truth (default B)

### 5.1 The design question

Ten components touch inner state. Which writes, which reads, and are there
double writes (stress applied by two systems) or orphan readers?

### 5.2 Path A — Graph audit

- Produce a read/write table: component → fields → direction.
- Flag double writers and fields never read.
- No code changes.

### 5.3 Path B — Single-writer enforcement

- For each double-write found, choose the owning system and route the other
  through the `NeedsSystem` external-modifier/attributed-delta seam (the
  canonical channel for external effects, with `sourceId`).
- Tests: one event changes a value once; attribution names the source.

### 5.4 Path C — Inner-life read model

Path B, plus a read-only `InnerLifeSummary` (current state, top causes,
treatability) consumed by the survivor detail panel (W3-06) — a projection.

### 5.5 Acceptance

- Documented graph; zero unknown writers.
- Attribution on every external effect.

---

## 6. Decision Point 2 — Record semantics and static hygiene (default B)

### 6.1 The design question

What does `SurvivorMentalHealthRecord` store, what mutates it, and does any
static state make it order-dependent?

### 6.2 Path A — Semantics audit

- Document every field, its setter, and its readers.
- Grep for static caches in the psychology owners; report candidates.

### 6.3 Path B — Semantics + isolation

- Rename/document unclear fields (additive comments; no schema change).
- Apply W2-01/W2-02's isolation gate to psychology statics (hooks or
  annotations).
- Tests: two survivors with different histories do not cross-contaminate;
  reload parity for the record.

### 6.4 Path C — Record read model

Path B, plus a read-only history of state changes (bounded, in-memory or
journal-routed, no new store) for the panel.

### 6.5 Acceptance

- Semantics documented; no unowned mutation.
- Isolation gate green for these classes.

---

## 7. Decision Point 3 — Trauma routing and recovery (default B)

### 7.1 The design question

Combat and events create trauma; catalog entries define outcomes. The route
must be one path with recovery options.

### 7.2 Path A — Routing audit

- Map event → trauma catalog entry → state mutation → recovery path.
- Report entries with no recovery and events with no catalog mapping.

### 7.3 Path B — Recovery contract

- Every treatable trauma has an authored recovery path (time, therapy, care,
  bond); terminal trauma is explicit.
- `TraumaBondSystem` and `CombatTraumaSystem` route through the catalog, not
  their own tables.
- Tests: event applies the catalog entry; recovery reduces the state; no
  permanent hidden debuff without an authored terminal flag.

### 7.4 Path C — Trauma arcs

Path B, plus authored long-arc recovery through W3-01 personal arcs (state
readers, not writers).

### 7.5 Acceptance

- No orphan trauma entries.
- Every treatable condition recoverable; terminal explicit.
- No hidden permanent penalties.

---

## 8. Decision Point 4 — Crisis foresight and treatment (default B)

### 8.1 The design question

`MentalHealthCrisisSystem` handles acute events. Crises must be foreseeable
(warning), treatable (response), and never silently fatal.

### 8.2 Path A — Crisis audit

- List crisis kinds, triggers, warnings, responses, outcomes.
- Flag silent-fatal or unwarned paths.

### 8.3 Path B — Foresight + no-silent-fatal

- Every crisis has at least one warning via the W2-03 telegraph seam (owner
  values only) and at least one authored response.
- Crises may cause harm/loss after warnings, never without one.
- Tests: warning precedes crisis; response reduces outcome severity; a
  deliberate unwarned path fails the test.

### 8.4 Path C — Crisis aftermath

Path B, plus aftermath (relationships, morale, narrative) through existing
owners, with dignity review.

### 8.5 Acceptance

- No unwarned crisis.
- Response exists; outcome scales with response.

---

## 9. Decision Point 5 — Therapy effect truth (default B)

### 9.1 The design question

Therapy exists (`PsychologicalSanatoriumSystem` + catalog). Does treatment
measurably help, cost something, and route through owners?

### 9.2 Path A — Effect audit

- For each therapy: target condition, effect magnitude, duration, cost,
  prerequisites.
- Report no-effect therapies and free perpetual care.

### 9.3 Path B — Measured care

- Each therapy applies its catalog effect once per session, consumes authored
  time/supplies through owners, and improves the target state measurably.
- Staffing/prerequisites gate availability (like the sealed ward staffing
  pattern).
- Tests: session improves target; cost consumed; unavailable when
  prerequisites unmet.

### 9.4 Path C — Care programs

Path B, plus authored multi-session programs (a course of therapy) riding the
existing therapy state; prose by W2-06.

### 9.5 Acceptance

- No no-effect therapy.
- Costs real; improvement measurable.
- No second therapy list.

---

## 10. Decision Point 6 — Morale contagion bounds (default B)

### 10.1 The design question

`MoraleContagionSystem` spreads morale between survivors. Spread must be
bounded, legible, and not a doom spiral.

### 10.2 Path A — Bounds audit

- Parameters, caps, positive/negative spread, and recovery.
- Report unbounded spirals.

### 10.3 Path B — Bounded contagion

- Contagion clamps per day and has recovery paths (rest, culture, care);
  attribution names the source survivor/event.
- Tests: a shelter cannot spiral below the authored floor without an authored
  cause chain; positive events spread too.

### 10.4 Path C — Social climate

Path B, plus a shelter climate read model (recent spread, dominant mood
source) surfaced by W3-06.

### 10.5 Acceptance

- Caps enforced; recovery works; attribution exists.

---

## 11. Decision Point 7 — Relationship authority and effects (default B)

### 11.1 The design question

Affinity, trust, decay, bonds, and coordination interact. One authority with
readable effects is the target.

### 11.2 Path A — Authority audit

- Map each relationship system's writes to the relation state.
- Report conflicting writes and unread fields.

### 11.3 Path B — One relation state

- Affinity/trust mutations route through the canonical producer (the
  `ModifyAffinity`/`ModifyTrust` pattern already documented, with the
  last-interaction stamp). Decay and bonds are modifiers over that state, not
  separate stores.
- Effects: trade cooperation, caregiving quality, morale spread, narrative arc
  eligibility.
- Tests: a positive interaction moves affinity once; decay respects the stamp;
  bonds add without overwriting.

### 11.4 Path C — Relationship stories

Path B, plus authored relationship arcs (W3-01) reading the state.

### 11.5 Acceptance

- Single writer; readable consumers; no duplicate relation store.

---

## 12. Decision Point 8 — Grief routing (default B)

### 12.1 The design question

Grief after a death should route to morale/relations/memorial through owners,
with dignity.

### 12.2 Path A — Routing audit

- Map death → grief sink → affected survivors → morale/relations effects.
- Report unreachable grief or duplicated memorial effects.

### 12.3 Path B — Verified grief

- `RelationsGriefSink` (or the memorial owner) applies a bounded, attributed
  morale/relation effect; the memorial vigil/rite path (sealed in earlier work)
  remains the ceremony seam.
- Tests: death applies grief once per affected survivor; vigil reduces it; no
  double application with morale contagion.

### 12.4 Path C — Mourning arcs

Path B, plus authored mourning through W3-01/W2-06 content.

### 12.5 Acceptance

- Grief applied once; recovery exists; dignity review.

---

## 13. Decision Point 9 — Caregiving effort and outcome (default A)

### 13.1 The design question

`CaregivingSystem` exists. Care must cost effort and produce measured benefit —
and never be mandatory busywork.

### 13.2 Path A — Effort audit

- Map care actions → effort/time → target improvement → side effects.
- Report zero-effort or zero-benefit actions.

### 13.3 Path B — Measured care

- Care consumes time/staff through owners and improves the target state; the
  caregiver may tire (needs effect) within bounds.
- Tests: care improves target measurably; cost applied; no infinite free care.

### 13.4 Path C — Care bonds

Path B, plus care-driven bond growth (relationship owner).

### 13.5 Acceptance

- Effort and benefit both real; bounds respected.

---

## 14. Decision Point 10 — Social dynamics consequences (default C)

### 14.1 The design question

`ShelterSocialDynamicsSystem` + `shelter_social_events.json`. Do social events
have consequences tied to real relationships/psychology?

### 14.2 Path A — Event audit

- List social events, triggers, and consequences.
- Report flavor-only events (if any) and consequence gaps.

### 14.3 Path B — Consequence routing

- Events read real relationship/psych state for selection and route
  consequences through owners (morale, relations, narrative flags).
- Determinism seeded; dedupe keys prevent repetition.

### 14.4 Path C — Living shelter arcs

Path B, plus longer social arcs (a shelter feud, a reconciliation) riding
W3-01 arcs and W3-03 state, with W2-06 prose.

### 14.5 Acceptance

- Events have consequences; selection reads real state; no spam.

---

## 15. Execution phases

### PS0 — Premise freeze (1 day)

- Verify owners/data; build the read/write graph; produce
  `P0_PSYCH_PREMISE.md`; run dignity check on existing content.

### PS1 — Ownership and records (Points 1 + 2)

- Single-writer binds; static hygiene; semantics doc.

### PS2 — Trauma and crisis (Points 3 + 4)

- Recovery contract; foresight/no-silent-fatal.

### PS3 — Therapy and morale (Points 5 + 6)

- Measured care; bounded contagion.

### PS4 — Relationships and grief (Points 7 + 8)

- One relation state; verified grief.

### PS5 — Care and social (Points 9 + 10)

- Measured caregiving; social consequences.

### PS6 — Closeout

- Evidence; journal/dignity review notes; Annex U.

---

## 16. Verification plan

| Point | Evidence |
|---|---|
| 1 | graph doc; zero double writers |
| 2 | semantics doc; isolation gate green |
| 3 | recovery paths; terminal explicit; tests |
| 4 | warning precedes crisis; response reduces severity |
| 5 | session improves target; cost consumed |
| 6 | caps; recovery; attribution |
| 7 | single writer; decay/bond tests |
| 8 | grief once; vigil reduces |
| 9 | effort/benefit tests |
| 10 | consequences routed; dedupe |

Commands:

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Survivors
bash scripts/run_test.sh Ashfall.Core.Tests/Needs
bash scripts/run_test.sh Ashfall.Core.Tests/Sanatorium  # if present
godot --headless --path . -- --7day-smoke-selftest
```

---

## 17. Risks

| # | Risk | L | I | Mitigation |
|---|---|---|---|---|
| 1 | mechanics feel like punishment | M | H | dignity rules; recovery paths |
| 2 | double-applied morale damage | M | H | single-writer graph; attribution |
| 3 | crisis fatal without warning | L | H | foresight test |
| 4 | therapy becomes a buff shop | M | M | costs + prerequisites; no stacking |
| 5 | grief spam | M | M | once-per-survivor + dedupe |
| 6 | relationship refactor breaks saves | M | H | modifiers over existing state; no data rewrite |
| 7 | social events repeat | M | M | dedupe keys |
| 8 | tone drift | M | H | W2-06 checklist per tranche; dignity audit |
| 9 | static state order-dependence | M | M | isolation gate |
| 10 | scope creep to new psychology systems | M | H | Wave 3 rule |

---

## 18. Ownership and claims

| Phase | Claim | Paths |
|---|---|---|
| PS0 | `W3-03-PS0-PREMISE` | premise + graph doc |
| PS1 | `W3-03-PS1-OWNERSHIP` | binds; static hygiene; semantics |
| PS2 | `W3-03-PS2-TRAUMA-CRISIS` | recovery; foresight |
| PS3 | `W3-03-PS3-THERAPY-MORALE` | therapy effects; contagion bounds |
| PS4 | `W3-03-PS4-RELATIONS-GRIEF` | relation state; grief routing |
| PS5 | `W3-03-PS5-CARE-SOCIAL` | caregiving; social consequences |
| PS6 | `W3-03-PS6-CLOSEOUT` | evidence + proposals |

Coordination: W2-03 owns needs/telegraph bands; W3-01 owns arc state; W2-06
owns prose and tone review; W2-02 owns defect repairs in these systems.

---

## 19. Rollback and decline

| Point | Rollback | Decline consequence |
|---|---|---|
| 1 | keep graph doc | double writers remain |
| 2 | keep semantics | isolation unmeasured |
| 3 | remove binds | trauma routing unverified |
| 4 | keep audit | crisis foresight unverified |
| 5 | remove effect binds | therapy unclear |
| 6 | keep bounds audit | spirals possible |
| 7 | keep authority doc | conflicting writes remain |
| 8 | keep audit | grief double-apply risk |
| 9 | keep audit | care unmeasured |
| 10 | keep audit | social events may be flavor-only |

---

## 20. DoD and handoff

**Path A:** graph, semantics, and all audits complete; dignity audit recorded.

**Path B:** all of A, plus single-writer binds, recovery/foresight contracts,
measured therapy/care, bounded contagion, one relation state, verified grief,
and social consequences — each tested.

**Path C:** all of B, plus inner-life read model, trauma/mourning/social arcs
(content via W2-06).

**Handoff:** outcome, files, contract (owner graph/recovery paths), commands,
limitations, untouched shared paths, ledger proposals, Annex U.

### 20.1 First safe step

> PS0 only: the owner graph and dignity audit. No mechanic changes first.

---

# ANNEX U — PLAN-UNBLOCKING (SEPARATELY)

## U.1 What W3-03 releases

| Blocked item | Mechanism | Gate |
|---|---|---|
| EN-04 Rehabilitation (consumer) | Therapy/care recovery paths give rehab content its owner | PS3 |
| Expansion 24 (Long Goodbye) | Verified grief/crisis/care mechanics are its substrate | PS2/PS4/PS5 |
| XP-06 body-integrity recovery (adjacent) | Caregiving binds support rehab; schema stays UNBLOCK-01 | PS5 |
| XP-10 verdict (DEC-18) | This plan confirms no phobia growth exists to revive; verdict input | PS0 |
| W3-01 personal arcs | Arc eligibility reads verified psych state | PS1 |
| W2-03 telegraph | Crisis/need warnings use the same seam | PS2 |
| Plan 42 survivor voice | State readers align with voice facts | PS1 |
| Expansion 20/23 psychology halves | Crisis/social consequences available | PS2/PS5 |

## U.2 Signatures needed

```text
[ ] I authorize PS0 premise + owner graph + dignity audit.
[ ] I authorize PS1 single-writer binds and static hygiene.
[ ] I authorize PS2 trauma recovery + crisis foresight contracts.
[ ] I authorize PS3 measured therapy + bounded morale contagion.
[ ] I authorize PS4 single relation state + verified grief routing.
[ ] I authorize PS5 caregiving effort/benefit + social consequences.
[ ] I authorize Path C read model/arcs separately.
```

## U.3 What W3-03 never touches for unblocking

- Prose (W2-06).
- Need values/pacing (W2-03).
- Narrative systems (W3-01).
- Save schema without signature.
- Phobia growth (DEC-18 RETIRED).

## U.4 The inner-life release rule

A psychology feature releases when its owner, cause, effect, and recovery path
all exist and the dignity review passes. A consequence with no cause or no
recovery does not release anything.

---

# APPENDICES

## A.1 Selection sheet

```text
ASHFALL WAVE 3 · PLAN 3 (PSYCHOLOGY) · SELECTION
Date: ______  Foreman: ______  HEAD: ______
PLAN PATH: [ ] A Truth & Safety  [ ] B One Inner Life (default)  [ ] C Living Shelter

01 owner graph ............ [A] [B] [C]   default B
02 record semantics ....... [A] [B] [C]   default B
03 trauma routing ......... [A] [B] [C]   default B
04 crisis foresight ....... [A] [B] [C]   default B
05 therapy effects ........ [A] [B] [C]   default B
06 morale contagion ....... [A] [B] [C]   default B
07 relationships .......... [A] [B] [C]   default B
08 grief .................. [A] [B] [C]   default B
09 caregiving ............. [A] [B] [C]   default A
10 social dynamics ........ [A] [B] [C]   default C
Signature: ________________
```

## A.2 Dignity checklist (per tranche)

```text
[ ] No condition framed as a monster or joke
[ ] Recovery paths exist or terminal is explicit
[ ] No reward for self-harm/addiction/cruelty
[ ] Privacy respected; no shame logs
[ ] Tone review passed (W2-06 checklist)
[ ] Clinically humane language
```

## A.3 Glossary

| Term | Meaning |
|---|---|
| owner graph | the read/write map of inner-life components |
| attribution | naming the source of an external effect |
| recovery path | authored route back from a condition |
| terminal | explicitly permanent condition |
| contagion cap | per-day spread bound |
| relation state | affinity/trust single store |
| grief sink | the owner applying loss effects |
| effort | the cost of caregiving |

**End of Part I.** Proposal only; executes nothing; releases nothing without
U.2 signatures.

---

# PART II — DEEP DESIGN SPECIFICATIONS (CONTINUED → 180K)# W3-03 · PART II — DEEP DESIGN: POINTS 1–5

> Appended 2026-09-21. Part I (summary contract) + this expansion.
> Proposal only. Dignity rules from Part I §0.4 are binding on every design.

---

## §II.1 Decision Point 1 — Owner graph and single-writer enforcement

### II.1.1 The psychology owner graph

Wave reconnaissance identified the family:

| Owner | Domain | Evidence |
|---|---|---|
| `SurvivorMentalHealthSystem` | per-survivor mental state | verified exists |
| `PsychologicalSanatoriumSystem` | care/therapy facility | verified exists |
| relationship system | pairwise ties | verify at P0 |
| needs/morale (`NeedsSystem`) | mood/morale needs | verified (`NeedKind.Morale`, `Numbness`, `RadiationAnxiety`) |
| grief/memorial (`MemorialSystem`) | loss handling | verified exists |
| journal (`JournalSystem`) | record | verified |

The failure mode this point exists for: **a second writer.** A quest writes
"morale -20" directly; a panel writes attitude; an event sets trauma flags
without routing. The result is state that disagrees with itself, and a save
that restores one truth while surfaces show another.

### II.1.2 The write map method (shared with W3-01/W3-02)

```text
for each psychology state family S:
  writers(S) := all call sites / data fields that mutate S
  assert |writers(S)| == 1
  readers(S) := optional documentation
  if |writers(S)| > 1: classify (benign aggregation vs. conflict)
```

Benign aggregation: one logical writer composed of multiple call sites in the
same module (e.g., several methods of the mental-health system). The
distinction is *module authority*, not call-site count.

### II.1.3 The declared writer table

```text
PSY-OWNER MAP (proposal)
  mental_health_state      -> SurvivorMentalHealthSystem
  trauma_marks             -> SurvivorMentalHealthSystem
  therapy_progress         -> PsychologicalSanatoriumSystem
  morale_modifiers         -> NeedsSystem (NeedsModifierStack)
  relationship_state       -> relationship owner (verify)
  grief_state              -> MemorialSystem (verify) or mental health (P0)
  social_standing_local    -> social dynamics owner (Point 10; verify)
```

Every row has file:line evidence at P0; every cross-writer found is a finding
with a repair (route through the owner or declare a new owner with a signed
line — never both writers left standing).

### II.1.4 The static hygiene companion (Point 2 preview)

Static fields are the classic second-writer vector: wave reconnaissance found
61 static field initializations in Core and specific `src/` statics
(`_silentFoundry`, `_sharedFactionStance`, `_sharedSkillProgression` — the
D19c finding). Psychology state must never live in static fields:

- scan for `static` fields of domain types in psychology paths;
- classify: configuration constants (fine) vs. mutable state (finding);
- Mutable static psychology state is repaired to the owner or removed.

### II.1.5 Acceptance tests (Point 1)

| Test | Expectation |
|---|---|
| write-map complete | every state family has exactly one module writer |
| no static domain state | scan clean in psychology paths |
| cross-writer routed | repaired writers call the owner API |
| owner API declared | each owner exposes the mutations it accepts |
| no panel writes | UI paths read-only (surface purity) |

### II.1.6 Cost

Write-map (2 days), static scan + repairs (2 days), owner API documentation (1
day), tests (1 day).

---

## §II.2 Decision Point 2 — Record semantics and static hygiene

### II.2.1 What a "record" is here

Psychology needs durable per-survivor records: trauma marks, care history,
relationship history moments, grief artifacts. The failure modes:

1. **Record without semantics:** a timestamped string blob nothing reads.
2. **Record with private format:** two systems serialize the same concept
   differently.
3. **Record that outlives its referent:** NPC dead, record references them
   (fine for memory, broken if it drives active behavior).
4. **Static cache drift:** a cached record initialized once and never
   refreshed (the D19c class).

### II.2.2 The record contract

```text
PsychRecord:
  subject (survivor id), kind (trauma | care | relation | grief),
  payload (typed per kind), day, source (system id), visible (bool)
```

Typed payloads: `trauma{severity, trigger_ref}`, `care{stage, progress}`,
`relation{moment, delta}`, `grief{artifact_ref, stage}`. Each kind has one
schema, validated statically.

### II.2.3 Static hygiene rules

```text
PSY-STATIC RULES
  S1. no mutable domain state in static fields
  S2. caches may be static ONLY with an invalidation contract
  S3. no record caches keyed by hash-iteration order (determinism)
  S4. no lazy singletons of stateful psychology services (the D19c pattern)
  S5. factory-created services must be reset between sessions (lifecycle)
```

Rule S5 is the D19c repair generalized: the owner graph's stateful services
are session-scoped; the session reset path must null/rebuild them (the same
repair W2-02 applies to the three verified statics — coordinated, not
duplicated).

### II.2.4 Record lifecycle

| Kind | Lifetime | Cleared by |
|---|---|---|
| trauma | persistent | authored care progress (stages), never bulk |
| care | session/arc | care conclusion |
| relation | persistent | authored reconciliation or excess (Point 7 rules) |
| grief | persistent (memorial) | never — memorials are the point |

### II.2.5 Acceptance tests (Point 2)

| Test | Expectation |
|---|---|
| schema validation | every record kind validates against its schema |
| static scan | S1–S5 clean or documented exceptions |
| lifecycle | records cleared per table in lifecycle harness |
| save round-trip | records restore exactly; no schema drift |
| no hash-order dependency | record iteration deterministic |

### II.2.6 Cost

Schema + validation (2 days), static repairs (2-3 days with W2-02), lifecycle
tests (1 day), round-trip (1 day).

---

## §II.3 Decision Point 3 — Trauma routing and recovery

### II.3.1 The trauma model

Trauma in ASHFALL is remembered, not spammed. Design rules:

1. **Every trauma has a trigger reference** — the event that caused it
   (consequence ledger ref from W3-01).
2. **Severity is authored per trigger class**, bounded.
3. **Recovery is a path, not a timer:** care, safety, time, and specific
   authored interventions each contribute through the owner.
4. **No trauma stacking to infinity:** repeated same-trigger events increase
   severity toward a cap, not additively forever.
5. **Trauma marks are visible to care:** the sanatorium and caregiving read
   them; hiding trauma is a state, not a bug.

### II.3.2 Routing contract

```text
Trigger event (event id, class)
  -> trauma owner.ApplyTrigger(subject, trigger_ref, class)
       severity += authored(class) capped
       record only if threshold crossed (no record spam)
  -> care path: sanatorium progress reduces severity per authored curve
  -> behavioral effects: authored bands (e.g., morale drain, sleep loss)
       read by needs/behaviors owners, never written by trauma
```

The split: trauma owner owns severity/records; *effects* are authored band
readings consumed by other owners. No owner reaches into another's state.

### II.3.3 The recovery curve

```text
severity(t) = cap(class) with recovery inputs:
  safety_days (no new triggers)
  care_sessions (sanatorium attendance)
  relationship_support (tie strength at threshold)
  authored interventions (items/scenes with care refs)
each input contributes per authored weight; total recovery/day bounded
```

Test: a survivor with a class-3 trauma and weekly care recovers to band ≤1 in
authored days; with zero care, stays high (authored chronic state — a real
outcome, not a bug); triggers during recovery re-increase (with the cap).

### II.3.4 Trigger classes (authored set)

```text
class: violence        (combat loss, raid, assault witnessed)
class: loss            (death of a tie, departure)
class: deprivation     (starvation event, dehydration crisis)
class: exposure        (radiation crisis, storm survival)
class: betrayal        (broken trust, faction double-cross)
class: confinement     (isolation, capture)
```

Each class has authored severity and recovery weights. New classes need a
signed line (vocabulary extension), not ad-hoc triggers.

### II.3.5 Acceptance tests (Point 3)

| Test | Expectation |
|---|---|
| trigger refs | every trauma record cites a consequence ref |
| bounds | severity ≤ cap per class |
| same-trigger stacking | repeated events → cap, not linear |
| recovery inputs | each input measurably contributes |
| chronic case | zero-care case stabilizes high (authored) |
| no reach-in | effects read bands; no cross-owner writes |
| round-trip | trauma + progress survive save/load |

### II.3.6 Cost

Model + routing (3 days), curves + authored tables (2 days), tests (2 days).

---

## §II.4 Decision Point 4 — Crisis foresight (no silent-fatal outcomes)

### II.4.1 The rule

No psychology-driven crisis may kill or maim a survivor without: (a) at least
one visible warning stage, and (b) at least one authored path to intervene.
This mirrors W2-03's crisis foresight and W3-02's warned-consequence rules —
the same humane standard across all waves.

### II.4.2 Crisis chains

```text
chain: despair
  stage 1: withdrawal (visible: survivor quiet, sleep loss)
  stage 2: self-neglect (visible: needs degrade faster; a care prompt)
  stage 3: crisis event (authored: intervention scene OR outcome)
interventions: [talk (relationship), care (sanatorium), rest (schedule),
                authored scene via W3-01]
```

Every chain declares: warning stages, at least one intervention per stage, and
the crisis event's authored resolution set.

### II.4.3 The audit

1. Enumerate psychology crisis paths (despair, breakdown, violence-at-home,
   flight, overdose-adjacent authored cases).
2. For each: verify warning stages exist and are *visible* (journal/barks/
   needs surface), verify interventions exist and function (tested), verify
   the crisis event has authored outcomes (never an unowned death).
3. Findings: silent fatal, missing intervention, invisible warning.

### II.4.4 Warning surfacing

Warnings surface through existing channels: needs panel states, journal
entries (W3-01 oracle), NPC barks, care prompts. The audit scripts: trigger a
chain at stage 1 → assert visible signal within authored time; attempt
intervention → assert chain degrades or resolves.

### II.4.5 Acceptance tests (Point 4)

| Test | Expectation |
|---|---|
| every chain warned | ≥1 visible stage before crisis |
| every stage intervenable | ≥1 path per stage |
| no unowned death | crisis outcomes authored and routed |
| visibility timing | warnings within authored time |
| intervention efficacy | authored resolution measurable |

### II.4.6 Cost

Chain inventory (2 days), intervention tests (2 days), visibility scripting (1
day), repairs (2 days).

---

## §II.5 Decision Point 5 — Measured therapy effects

### II.5.1 The question

`PsychologicalSanatoriumSystem` exists. Therapy must *measurably* help, and
its limits must be authored: no infinite sessions, no instant cure, no
useless (decorative) care.

### II.5.2 Therapy model

```text
Session:
  subject, therapist (authored role), duration, quality (authored inputs:
  facility, rest, trust), outcome per authored curve:
    severity reduction, morale stabilize, insight record
Limits:
  sessions per day/subject (authored)
  diminishing returns per severity band (authored)
  floor: severity cannot go below band 0 (cure is not a state; stability is)
```

### II.5.3 Effectiveness audit

```text
for each authored therapy type:
  measure severity delta per session (scripted subjects)
  assert delta within authored band
  assert diminishing returns (later sessions smaller effect)
  assert floor respected (no negative severity)
  assert no decorative therapy (delta zero everywhere)
```

### II.5.4 Caregiver side

Therapy consumes caregiver effort (Point 9) and may affect the caregiver
(authored: secondary stress or satisfaction — a real design decision recorded,
bounded). Findings: therapy with no effort cost, caregiver effects outside
authored tables.

### II.5.5 Acceptance tests (Point 5)

| Test | Expectation |
|---|---|
| effect bands | deltas within authored ranges |
| diminishing returns | monotone decreasing effect |
| floor | no over-cure |
| no decorative care | every therapy type measurably changes state |
| effort cost | every session consumes effort per table |
| round-trip | progress survives save/load |

### II.5.6 Cost

Model + tables (2 days), effect tests (2 days), caregiver integration (1 day).

---

## §II.6 Points 1–5 execution order

```text
Day 1-3   P1 write-map + static scan (with W2-02 coordination)
Day 4-6   P2 record schema + static repairs + lifecycle tests
Day 7-9   P3 trauma routing + curves
Day 10-11 P4 crisis chain inventory
Day 12-13 P4 interventions + visibility scripting
Day 14-15 P5 therapy model + effect tests
Day 16    Consolidation + handoffs (W3-01 scenes; W3-02 effort economy)
```

Shared artifacts: the owner map (P1) is consumed by every later point; the
record schema (P2) is consumed by trauma (P3), grief (P8), and social (P10).

---

*End of Part II. Continues in Part III (Points 6–10).*# W3-03 · PART III — DEEP DESIGN: POINTS 6–10

---

## §III.1 Decision Point 6 — Bounded morale contagion

### III.1.1 The contagion model

Morale spreads: one survivor's state affects nearby ties. Unbounded contagion
is the classic death spiral (one bad day empties the shelter). The model:

```text
contagion(subject S, tie T, day):
  pressure = mood_delta(S) × tie_strength(S,T) × susceptibility(T)
  susceptibility(T) = authored base(class) × context modifiers
                    (crowding, recent care, food state, sleep)
  effect bounded per day per direction; total daily effect bounded per T
  no runaway: effect cannot exceed the stronger of the two moods
    (contagion moves moods toward each other, never past)
```

### III.1.2 The damping rules

```text
D1 per-pair daily cap
D2 per-survivor daily total cap (across all ties)
D3 no positive feedback loops without damping (S→T→S converges)
D4 dark moods damped stronger than bright (authored: misery spreads slower
   than relief — a deliberate design choice against doom-stacking)
D5 isolation stops spread (no ties = no contagion)
```

### III.1.3 The audit

1. Enumerate contagion writers (must be one owner — likely the morale/modifier
   owner or a dedicated social owner; verify at P0).
2. Script: one sad survivor with five ties; assert daily total ≤ cap, no T
   crosses into a lower band than S's own (no amplification).
3. Script sustained crisis: over 10 days, assert shelter morale stabilizes at
   an authored floor, not zero.
4. Determinism: seeded if random pairing; iteration order stable.

### III.1.4 Acceptance tests (Point 6)

| Test | Expectation |
|---|---|
| single writer | contagion owner only |
| pair/day cap | enforced |
| survivor/day cap | enforced |
| no amplification | T's mood not worse than S's floor |
| convergence | S↔T loop converges per authored damping |
| dark/light asymmetry | authored damping honored |
| isolation | no spread without ties |
| floor | crisis stabilizes ≥ authored floor |

### III.1.5 Cost

Model + tables (2 days), damping tests (2 days), crisis floor test (1 day).

---

## §III.2 Decision Point 7 — Relationship authority

### III.2.1 The relationship record

```text
Relationship(S,T):
  stance (authored bands: hostile..bonded)
  strength (numeric, bounded)
  history[] (moments: source, delta, day)
  flags (authored states: estranged, indebted, mourning)
```

### III.2.2 Why relationships are their own authority

Relationships are pairwise state; using a global "attitude" map or writing
attitudes from quests directly creates the second-writer problem. The audit:
every relationship mutation routes through the relationship owner; quests/events
propose deltas, owner applies with caps.

### III.2.3 Change rules

```text
per-event delta cap per day (no instant bonding)
decay toward authored baseline if unchanged for authored days
(effort maintains ties — a human truth)
band transitions authored with thresholds and visible states
(no silent band flips)
```

### III.2.4 Cross-system reads

| Consumer | Reads | Use |
|---|---|---|
| W3-01 arcs | strength, flags | triggers, legacy paths |
| W3-02 barter | trust band | scoped deals |
| W3-03 care | support strength | recovery input |
| W3-04 mercenaries | scoped reputation (separate) | contracts (do not conflate) |
| W2-06 prose | flags | voice variation |

**Guard:** mercenary scoped reputation (W3-02 C-P4) and personal relationships
are distinct authorities; conflating them is a finding.

### III.2.5 Acceptance tests (Point 7)

| Test | Expectation |
|---|---|
| single writer | relationship owner only |
| delta caps | per event/day enforced |
| decay | ties decay without effort per authored rule |
| band visibility | transitions have authored visible states |
| no conflation | mercenary reputation ≠ personal tie |
| round-trip | history/moments persist |

### III.2.6 Cost

Owner verification (1-2 days), change-rule tests (2 days), consumer matrix (1
day), round-trip (1 day).

---

## §III.3 Decision Point 8 — Grief routing

### III.3.1 The grief model

Death in ASHFALL leaves grief: a survivor-level process with a memorial
artifact. The routing:

```text
death event (consequence ref)
  -> grief owner.ObserveDeath(subject, deceased)
       grief stage: authored (denial/anger/work/rmembrance — authored names)
       effects: authored morale/trauma interplay (see note below)
  -> memorial owner.Record(deceased, artifacts)   (MemorialSystem)
  -> journal entry (W3-01 oracle)
  -> authored scenes available (funeral, vigil — W3-01 content)
```

### III.3.2 The trauma/grief boundary (important)

Grief is not trauma (trauma is Point 3). The interface:

| Grief (MemorialSystem/owner) | Trauma (mental health owner) |
|---|---|
| loss process stages | threshold-based marks |
| recovery: time + ritual + ties | recovery: safety + care + time |
| artifact-driven (memorial) | trigger-driven (event class) |

Both may apply from one death (authored: severe deaths apply both). The audit
verifies no double-applied morale hit from both systems for one death beyond
the authored combined bound.

### III.3.3 Memorial continuity

Memorials persist (they are the long memory): names, artifacts, dates. The
audit: deceased appear in the memorial exactly once; artifacts reference real
items; removal never happens except by a signed feature.

### III.3.4 Acceptance tests (Point 8)

| Test | Expectation |
|---|---|
| routing | death → grief + memorial + journal, each once |
| combined bound | grief+trauma morale effect ≤ authored |
| stages | authored stage progression with visible states |
| memorial | one entry per deceased; persists |
| scenes | authored scenes available per stage, bounded |
| round-trip | grief stage survives save/load |

### III.3.5 Cost

Model + interface (2 days), combined-bound tests (2 days), memorial tests (1
day), round-trip (1 day).

---

## §III.4 Decision Point 9 — Caregiving effort

### III.4.1 The model

Care (therapy, tending the sick, watching the grieving) consumes real effort:
time, rest, supplies — and may affect the caregiver. The model:

```text
CareTask:
  kind (therapy | tending | vigil | rescue), subject, effort_cost,
  caregiver_effect (authored: strain | satisfaction | neutral),
  supplies (items/water), duration
Effort:
  a shared caregiver capacity (per survivor/day) owned by the needs owner
  or a schedule owner (verify at P0; must be single)
```

### III.4.2 The audit

1. Every care act consumes effort from the owner (no free care).
2. Caregiver effects are authored and bounded (strain cannot kill silently —
   it enters crisis foresight rules).
3. Supplies consumed through inventory owners.
4. The caregiver with zero capacity cannot care (gate), and the UI states why
   (legibility; W3-06 surface).

### III.4.3 The dignity rule

Care is never coerced by the machine: no auto-assignment that damages a
caregiver without authored consent/choice. Assignment is a player policy or an
authored NPC choice with its own consequences. Findings: silent care
assignment causing strain.

### III.4.4 Acceptance tests (Point 9)

| Test | Expectation |
|---|---|
| effort consumption | every care act consumes per table |
| caregiver effects | bounded; crisis rules apply to strain |
| supplies | routed to inventory |
| gating | zero capacity blocks care with visible reason |
| no silent assignment | strain only via authored choice/policy |
| round-trip | effort state persists |

### III.4.5 Cost

Owner verification (1-2 days), effect tests (2 days), gating/legibility (1
day).

---

## §III.5 Decision Point 10 — Social dynamics (collective state)

### III.5.1 What "social dynamics" means here

Beyond pairwise ties: group-level state — factions within the shelter, social
trust, cohesion, isolation. The audit question: does a group-level owner exist,
is it distinct from pairwise relationships, and are its effects routed?

### III.5.2 The model (proposal shape)

```text
Group(state: shelter):
  cohesion (bounded), trust (bounded), divisions (authored groups:
  workers/refugees/families with authored stance per group)
  events: meetings, disputes, festivals (authored content)
  effects: consumption modifiers, compliance rates, crisis susceptibility
```

**Authority rule:** the group owner owns group state; individuals' morale
belongs to their owners; the group reading aggregates individual state
(read-only aggregation — never a second per-survivor store).

### III.5.3 The audit

1. Verify whether a group-level owner exists; if not, Path A documents the gap
   and Path B proposes it with a signed line (no improvised ownership).
2. Aggregation rules: group mood = authored aggregate of member moods, not an
   independent variable (no dual-truth).
3. Effects: authored (compliance, consumption, crisis susceptibility) routed
   to their owners.
4. Divisions: authored groups with stance; division events route through
   W3-01 choices.

### III.5.4 Acceptance tests (Point 10)

| Test | Expectation |
|---|---|
| single owner | group state writer documented |
| aggregation only | group mood derived, not stored independently |
| effects routed | compliance/consumption via owners |
| divisions authored | groups defined; events available |
| round-trip | group state persists if stored (signed) |

### III.5.5 Cost

Owner verification (1-2 days), aggregation tests (1-2 days), effects routing
(1-2 days).

---

## §III.6 Points 6–10 execution order and cross-links

```text
Day 1-2   P6 contagion owner + damping
Day 3-4   P7 relationship owner verification + change rules
Day 5-7   P8 grief routing + combined bounds
Day 8-9   P9 care effort + dignity gating
Day 10-12 P10 group owner verification + aggregation
Day 13    Consolidation
```

Cross-links: P6 reads P7 ties; P8 uses P3 trauma interface and P7 ties for
support; P9 effort feeds P5 therapy cost; P10 aggregates P6/P7 state.

---

## §III.7 The no-second-truth invariant (whole plan)

```text
Every psychological quantity has exactly one writer.
Every derived quantity is computed at read time from owners.
Every effect is authored as bands read by consumers.
Every crisis has warnings and interventions.
Every record has a schema and a lifetime.
```

This invariant is the plan's identity; all ten points are its applications.

---

*End of Part III. Continues in Part IV (playbooks).*# W3-03 · PART IV — CLINICAL AUTHORING PLAYBOOKS AND SCENARIO PACK

> How to author every psychology artifact without breaking dignity, ownership,
> or determinism: trauma triggers, crisis chains, care protocols, grief,
> relationships, group dynamics, and records. Plus five worked threads.

---

## §IV.1 The psychology authoring lifecycle

```text
1. INTENT     what human truth does this represent? (one sentence)
2. OWNER      which owner holds the state? (must be exactly one)
3. ROUTING    how does the cause reach the owner? (consequence/event ref)
4. EFFECT     what authored bands do other owners read?
5. RECOVERY   what paths exist, and what is the chronic outcome?
6. VISIBILITY how does the player learn? (journal/needs/barks/care)
7. DIGNITY    does this treat the survivor as a person? (review)
8. DETERMIN   seeded if stochastic; stable iteration otherwise
9. TESTS      bands, caps, recovery, visibility, round-trip
10. SEAL      record schema validated; registry updated
```

**Rule zero:** the survivor is a person, not a meter. Every artifact must
answer "what would this look like from inside?" before it ships.

---

## §IV.2 Trauma trigger authoring

### IV.2.1 Trigger template

```yaml
trigger: raid_survived
class: violence
source: consequence:raid_outcome_survived
severity: authored 0.4 (class table)
threshold_for_record: 0.3        # avoid record spam below this
effects_bands:
  morale_drain: mild              # read by needs owner
  sleep_quality: -1 band
recovery_weights:
  safety_days: 0.05/day
  care_sessions: 0.10/session
  tie_support: 0.03/day (if tie >= bonded)
cap: 1.0 (class cap)
chronic_behavior: authored (if never treated: stable at band high)
```

### IV.2.2 Authoring rules

1. **Consequence ref mandatory.** Trauma without a cause is a state change
   without a story; the ref ties it to the ledger (W3-01).
2. **Severity from the class table**, not per-trigger improvisation (personal
   variation is a modifier, authored within bounds).
3. **Record only above threshold** — the record is meaning, not a log of every
   bad hour.
4. **Effects as bands, not numbers** — bands keep consumers decoupled and
   authorable.
5. **Recovery paths authored for this trigger** — a trigger with no recovery
   path is a permanent scar, which must be marked `chronic: true` deliberately.
6. **No moralizing text.** The record's visible text (W2-06) describes, never
   judges.

### IV.2.3 Trigger review sheet

```text
[ ] consequence ref exists and fires before the trigger
[ ] class table severity; bounds respected
[ ] threshold authored
[ ] effects bands named; consumers verified
[ ] recovery paths ≥1 (or chronic marked)
[ ] cap referenced
[ ] visibility: appropriate journal/needs surfacing
[ ] dignity review passed
[ ] tests authored (bands, cap, recovery, round-trip)
```

### IV.2.4 Trigger anti-patterns

| Anti-pattern | Why it hurts | Fix |
|---|---|---|
| trauma on every combat tick | severity spam; player despair | threshold + class cap |
| invisible trauma | player can't understand behavior | visibility row |
| no recovery path | permanent gloom by accident | author path or mark chronic |
| effects as direct number writes | second writer | bands read by owners |
| trauma as punishment for choices | moralizing | redesign as consequence with care |
| duplicate class meanings | table sprawl | class review before adding |

---

## §IV.3 Crisis chain authoring

### IV.3.1 Chain template

```yaml
chain: despair
stages:
  - id: withdrawal
    visible: [quiet bark, sleep loss band, journal note]
    interventions: [talk (tie), rest schedule, care session]
    duration_max: authored 5 days -> advances
  - id: self_neglect
    visible: [needs degrade faster, care prompt]
    interventions: [care, tie talk, authored scene]
    duration_max: 4 days -> advances
  - id: crisis
    outcomes: [resolved (if intervention), authored event (else)]
    never: unowned death       # hard rule
```

### IV.3.2 Authoring rules

1. **Every stage visible.** The player gets at least one signal per stage; the
   signal path is authored (journal/needs/barks).
2. **Every stage intervenable.** At least one path works per stage; paths are
   tested for efficacy (an intervention that doesn't intervene is a finding).
3. **Crisis outcomes authored.** The crisis stage has a resolution set
   (partial recovery, authored scene, outcome with care). "Nothing can be
   done" is not an allowed default.
4. **Pacing.** Chains advance on authored time/state, never on a hidden random
   roll without visibility.
5. **Exit to stability** — the resolved state is authored (bands, follow-up
   care), not a silent reset.

### IV.3.3 The efficacy test

```text
for each intervention I in stage S:
  run chain to S with a subject; apply I; assert stage regresses or resolves
  run chain to S with no intervention; assert stage advances at duration_max
```

If an intervention shows no measurable difference, it is either mislabeled
(state changes elsewhere) or decorative — both findings.

### IV.3.4 Crisis authoring review sheet

```text
[ ] stages have visible signals (path authored)
[ ] interventions per stage (efficacy tested)
[ ] crisis outcomes authored and routed
[ ] no unowned death/maim
[ ] pacing authored (time/state, not hidden roll)
[ ] resolution state authored (no silent reset)
[ ] dignity review (the person is not a puzzle)
```

---

## §IV.4 Care protocol authoring

### IV.4.1 Protocol template

```yaml
protocol: therapy_session
kind: therapy
inputs: {therapist_time: 2h, facility: sanatorium_bed, supplies: none}
effect: {severity: -0.12 (diminishing), morale_stabilize: +1 band}
caregiver_effect: strain +0.1 (authored, bounded)
limits: {per_subject_per_day: 1, diminishing_after: 3 sessions}
exit: {severity_floor: band 0}
```

### IV.4.2 Authoring rules

1. **Consume real inputs** (effort, supplies, facility); free care is
   decorative.
2. **Effects in bands/deltas with diminishing returns**; no infinite cure.
3. **Caregiver effects authored** and routed into crisis rules if strain can
   escalate (strain is a person too).
4. **Limits authored**; repeated sessions have authored diminishing effect.
5. **Floors**: therapy stabilizes, does not erase (band 0 = stable, not
   "cured forever"); relapse paths exist via new triggers.
6. **Consent framing.** Care is offered, never machine-forced; assignment
   policies are player/NPC choices with consequences.

### IV.4.3 Protocol review sheet

```text
[ ] inputs consumed (effort/supplies/facility)
[ ] effects bounded + diminishing
[ ] caregiver effect authored + routed
[ ] limits authored
[ ] floor respected
[ ] consent framing (no silent assignment)
[ ] dignity review
[ ] round-trip of progress
```

---

## §IV.5 Grief and memorial authoring

### IV.5.1 Grief template

```yaml
grief: departed_bonded
observes: consequence:death_event
stage_curve: [numbness, weight, remembering, carrying]  # authored names
duration_per_stage: authored days (accelerated by ritual/tie support)
effects_bands: {morale: low, social withdrawal: mild}
scenes: [funeral (if body), vigil, marker placement]   # W3-01 authored
memorial: artifact class (name, item, date)
bound_composition: combined with trauma (authored max)
```

### IV.5.2 Rules

1. **Grief is authored as stages, never a single hit.**
2. **Rituals accelerate, not skip** (no instant cure; no forced attendance).
3. **Memorials persist**; the artifact references real item/data ids.
4. **The dead are named correctly** (name, pronouns, role per the survivor
   record) — a review item, not automation.
5. **Combined bound** with trauma for the same death; the audit computes the
   total morale effect and checks ≤ authored bound.
6. **Visibility:** the journal records the loss (W3-01 oracle); the memorial
   surface (W3-06) lists the dead.

### IV.5.3 Grief review sheet

```text
[ ] stages authored with names and durations
[ ] rituals authored (accelerate per table)
[ ] memorial artifact refs real
[ ] combined bound tested
[ ] journal entry once (oracle)
[ ] dignity/name review
```

---

## §IV.6 Relationship authoring

### IV.6.1 Moment template

```yaml
moment: shared_danger
participants: [survivor_a, survivor_b]
source: consequence:raid_with_both_present
delta: +0.12 (cap per event)
flags: none
visible: journal (relationship category)
```

### IV.6.2 Rules

1. **Moments have sources** (consequence refs), like everything else.
2. **Delta caps** per event and per day; no instant bonding.
3. **Decay authored**: ties need maintenance; the rate is per relationship
   class (family slower, strangers faster).
4. **Band transitions visible** via authored states (the journal/relationship
   surface shows the change; no silent flip).
5. **No conflation** with scoped mercenary/faction reputation.
6. **Mourning flags** set by grief routing (Point 8), read by prose/arcs.

### IV.6.3 Relationship review sheet

```text
[ ] moment source ref
[ ] delta within caps
[ ] visibility (journal relationship entry)
[ ] decay class assigned
[ ] band threshold definitions unchanged or reviewed
[ ] no reputation conflation
[ ] round-trip of history
```

---

## §IV.7 Group/social authoring (Point 10)

### IV.7.1 Group event template

```yaml
event: ration_dispute
groups: [workers, refugees]
source: policy:strict_rationing (or consequence ref)
effects: {cohesion: -authored band, division stance: shift}
scenes: authored (meeting, confrontation — W3-01)
routing: crisis chain if cohesion below authored band
```

### IV.7.2 Rules

1. **Group state aggregated, not independent** (mood from members).
2. **Events sourced** (policy/consequence refs).
3. **Division stances authored**; shifts bounded.
4. **Crisis routing** when cohesion drops: authored chain with warnings.
5. **Scenes route through W3-01** choices.

### IV.7.3 Group review sheet

```text
[ ] event source ref
[ ] effects bounded
[ ] aggregation verified (group mood derived)
[ ] crisis routing authored
[ ] scenes registered (W3-01)
[ ] no parallel per-survivor store
```

---

## §IV.8 Record authoring and schema hygiene

### IV.8.1 Record template

```yaml
record: trauma_mark
subject: survivor id
kind: trauma
payload: {class, severity, trigger_ref, threshold_day}
visible: true
lifetime: persistent
schema_version: authored
```

### IV.8.2 Rules

1. **One schema per kind**, validated statically (the record validator).
2. **Payload fields minimal**; derived data computed at read time.
3. **Visibility per record**, not per system (the player learns what the
   survivor would show).
4. **Schema changes follow the save discipline**: additive fields, version
   migration path if any record persists (signed).
5. **No prose in payloads** (text refs only — the freeze rule).

---

## §IV.9 Scenario pack (five threads)

### IV.9.1 Thread: the raid's aftermath

Setup: a raid kills one survivor (bonded to two others) and is survived by a
third with trauma class violence. Expected chain:

```text
death -> grief stage 1 (both ties) + memorial + journal
survivor -> trauma trigger (severity 0.4) + record
contagion: two grieving survivors affect a third (bounded)
care: sessions available; effort cost real
chronic path: if no care, one tie stabilizes at band high (authored)
```

Findings this thread catches: combined grief+trauma morale exceeding bound;
memorial missing; contagion amplifying (one tie worse than the deceased's
state); care free of cost.

### IV.9.2 Thread: the long winter

Setup: 20 days of deprivation (rations cut, cold). Expected: morale drain
bands, contagion damping under sustained gloom (floor stabilization), crisis
chains (despair) triggered at authored thresholds with warnings, care scarce
(effort budget). Findings: despair chains firing without warnings; floor not
holding (shelter morale → zero); depression as a mass event (design error vs.
authored).

### IV.9.3 Thread: the betrayal

Setup: a faction double-cross (W3-02 thread) with a named NPC complicit.
Expected: trauma class betrayal on witnesses; relationship flags (estranged);
group cohesion drop; authored scene (confrontation). Findings: betrayal trauma
on uninvolved survivors (trigger scoping); relationship band flip without
visible state; cohesion crisis unwarned.

### IV.9.4 Thread: the caregiver's strain

Setup: one survivor cares for three trauma cases over 10 days. Expected:
effort capacity exceeded (gating), caregiver strain accruing in authored
bands, crisis chain for the caregiver if unattended, satisfaction path if
care succeeds (authored positive effect). Findings: strain killing silently
(crisis rules missing for caregivers); free infinite care; no positive path
(a design imbalance).

### IV.9.5 Thread: the homecoming

Setup: a survivor returns from a long expedition (deprivation + violence
triggers). Expected: ties decayed (relationship decay), re-bonding moments
available, trauma present, grief for those who died in their absence, group
reaction scenes. Findings: re-bonding instant (decay/cap missing); missing
grief observation for deaths while away (the absent-survivor edge case);
group scenes absent.

### IV.9.6 Scenario coverage matrix

| Thread | P1 | P2 | P3 | P4 | P5 | P6 | P7 | P8 | P9 | P10 |
|---|---|---|---|---|---|---|---|---|---|---|
| raid aftermath | ● | ● | ● | ● | ● | ● | ● | ● | ● | |
| long winter | | ● | ● | ● | ● | ● | | ● | ● | ● |
| betrayal | | ● | ● | | | ● | ● | | | ● |
| caregiver strain | | ● | | ● | ● | | | | ● | |
| homecoming | | ● | ● | | ● | | ● | ● | | ● |

Every point appears in ≥3 threads; P1/P2 appear everywhere (they are the
substrate).

---

## §IV.10 The psychology review standard (one page)

```text
1. OWNER     one writer per state; derived quantities computed, not stored
2. SOURCE    every mood/trauma/relationship change cites a cause
3. BOUND     caps everywhere (severity, deltas, contagion, strain)
4. WARN      crises are visible and intervenable
5. RECOVER   paths exist; chronic outcomes are authored, not accidents
6. PERSON    dignity review: the survivor is a person, not a meter
7. VISIBLE   the player can learn what changed and why
8. DETERMIN  seeded RNG; stable iteration; save round-trip
9. SCOPED    no conflation of personal ties and scoped reputations
10. QUIET    restraint in text; no melodrama, no moralizing
```

---

## §IV.11 Handoffs

| To | Handoff |
|---|---|
| W3-01 | crisis scenes, grief scenes, choice integration |
| W3-02 | effort economy (care costs), ration morale effects |
| W3-04 | combat outcomes as trauma/grief sources |
| W3-05 | care facility items/supplies |
| W3-06 | needs/care/journal/relationship surfaces |
| W2-02 | statics/lifecycle repairs (D19c family) coordinated |
| W2-03 | morale bands (measurement, not tuning) |
| W2-06 | all visible text |

---

*End of Part IV. Continues in Part V (verification catalog).*# W3-03 · PART V — VERIFICATION CATALOG, EVIDENCE DESIGN, AND WORKED THREADS

> The complete verification layer for the ten points: static checks, focused
> kits, soak designs, evidence handling, and three fully worked threads with
> expected findings and repairs. Proposal-only.

---

## §V.1 Verification tiers

```text
T1 STATIC (seconds)
  owner-map drift, static scan (S1–S5), record schema validation,
  crisis-chain completeness, cap/table bounds
T2 FOCUSED RUNTIME (minutes)
  per-point kits: trauma routing, recovery curves, crisis warnings,
  therapy effects, contagion damping, relationship rules, grief routing,
  care effort, group aggregation
T3 SOAK (hours, shared harness)
  20 seeds × 60 days shelter simulation with scripted deaths, raids,
  deprivation windows; morale floors, chain rates, recovery outcomes
```

---

## §V.2 T1 — static checks

| Check | Input | Detects | Output |
|---|---|---|---|
| P1.1 owner-map drift | declared writers vs. data/code | second writers | state, writer, file |
| P1.2 static scan | Core/src psychology paths | mutable static domain state | field, file |
| P1.3 record schema | record instances | invalid payloads | record id, kind, field |
| P1.4 chain completeness | crisis chain data | missing warning/intervention/outcome | chain, stage |
| P1.5 cap table | trauma/therapy/contagion tables | missing caps | table, value |
| P1.6 source refs | triggers/grief/moments | dangling consequence refs | ref, content id |
| P1.7 visibility rows | records/chains | invisible warnings | chain/stage |
| P1.8 conflation | reputation vs. tie keys | shared storage | key, systems |
| P1.9 text refs | records/scenes | missing corpus refs | ref |

### V.2.1 Generator: `docs/psychology/OWNER_MAP.md`

Generated from the declared writer table plus static scan; `--check` drift
gate. The map lists every psychology state family, its writer, its API, its
consumers. This is the plan's first artifact and the audit's reference.

### V.2.2 The static scan design (S1–S5)

```text
scan targets: static fields of domain types in Core and src psychology paths
classifications:
  config       -> allow (immutable, non-domain)
  cache        -> require invalidation contract reference
  state        -> finding (must live in owner)
  service      -> finding if stateful without session reset (D19c class)
output: per-field classification with file:line
```

The three verified D19c statics (`_silentFoundry`, `_sharedFactionStance`,
`_sharedSkillProgression`) are recorded as **known findings owned by W2-02's
repair** — this plan verifies their disciplines apply to psychology and does
not duplicate the repair.

---

## §V.3 T2 — focused runtime kits

### V.3.1 Owner routing kit (P1)

```text
for each state family:
  attempt a write via a non-owner path (scripted)
  assert: compile-time/runtime blocks failure OR route through owner API
  assert: owner value equals the routed value
  assert: no second truth after restore
```

### V.3.2 Record lifecycle kit (P2)

```text
create records of each kind; advance session/lifecycle
assert: lifetimes per table (scenes cleared, persistent kept)
save/load; assert payload equality with schema version
attempt invalid payload; assert validator rejects
```

### V.3.3 Trauma kit (P3)

```text
apply trigger class X:
  assert severity delta in authored band
  assert record created only above threshold
  apply same trigger N times: severity toward cap, not N×
recovery:
  no care + safety: measure curve; assert chronic stabilization (authored)
  care sessions: measure diminishing returns; assert floor band 0
  triggers during recovery: re-increase with cap
round-trip: severity + progress preserved
```

### V.3.4 Crisis foresight kit (P4)

```text
per chain:
  drive to each stage: assert visible signal within authored time
  per intervention: apply, assert stage regresses or resolves
  no intervention: assert advance at duration_max
  crisis stage: assert authored outcome resolved or event; assert no unowned
  fatal path
```

### V.3.5 Therapy kit (P5)

```text
for each therapy type:
  measure severity delta; assert within band
  repeat sessions: assert diminishing returns
  assert floor (no negative severity)
  assert effort consumed per session
  assert caregiver effect within authored band
  assert no decorative type (delta != 0 somewhere)
```

### V.3.6 Contagion kit (P6)

```text
pair: S mood low, T neutral; apply contagion
  assert T moves toward S, not past
  assert per-pair/day cap
five-tie case: assert survivor/day total cap
S<->T loop: assert convergence (no oscillation growth)
dark vs. light: assert authored damping asymmetry
sustained crisis: assert shelter floor stabilization, not zero
determinism: same seed/state ⇒ identical contagion deltas
```

### V.3.7 Relationship kit (P7)

```text
moment: assert delta cap; band transition visible states
decay: no interactions for authored days ⇒ decay per class
no conflation: mercenary reputation change does NOT move personal tie
round-trip: history/moments preserved; iteration stable
```

### V.3.8 Grief kit (P8)

```text
death event: assert grief observation + memorial + journal exactly once
combined bound: grief + trauma morale effect ≤ authored total
stages: authored progression; rituals accelerate (never skip)
memorial: one entry per deceased; artifact refs valid; persists
absent survivor: death during absence observed on return (authored)
```

### V.3.9 Care effort kit (P9)

```text
care act: effort consumed; supplies consumed; effect applied
zero capacity: blocked with visible reason
caregiver strain: accrues in bands; crisis rules apply at thresholds
no silent assignment: strain only via authored choice/policy
```

### V.3.10 Group kit (P10)

```text
group mood: equals authored aggregation of members (derived)
event: effects bounded; division stances shift per authored table
cohesion crisis: routed chain with warnings
round-trip: group state persists (if stored, signed)
```

---

## §V.4 T3 — soak design

### V.4.1 Soak scenario

```text
20 seeds × 60 days
scripted injections per seed policy:
  raids (2-4 events), deaths (1-3, some bonded), deprivation windows (1-2),
  betrayal events (0-1), expedition departures (1-2)
measurement:
  morale distribution per day (shelter + individuals)
  trauma severity distribution
  crisis chain counts (fired / resolved / escalated)
  care throughput vs. effort capacity
  contagion deltas (per-pair maxima)
  grief stage progression times
  group cohesion trajectory
assertions:
  floors: shelter morale ≥ authored floor across seeds (no death spiral)
  no silent fatals: every crisis escalation had warnings (log check)
  recovery: treated trauma cases improve or stabilize per authored bands
  effort: care never occurs without effort consumption
  bounds: all deltas/caps respected (log scan)
  determinism: same seed identical histories
```

### V.4.2 The death-spiral test (flagship)

The critical system property: **a shelter under sustained pressure must not
collapse to zero morale through internal dynamics alone.** The soak asserts an
authored floor and that the floor is reached by *narrative* means (broken
people below it individually) rather than by a runaway loop. This is the
single test that gives the psychology plan credibility.

### V.4.3 Report format

```yaml
run: T3-2026-11-a
seeds: 20, days: 60
morale_floor: {min_per_shelter: 0.18, floor: 0.15, pass: true}
crisis_chains: {fired: 34, warned: 34, unwarned: 0, resolved: 26, escalated: 8}
trauma: {cases: 41, cap_hits: 3, recovered_bands: 29}
care: {sessions: 88, without_effort: 0}
determinism: pass
```

---

## §V.5 Worked thread 1 — the raid's aftermath (full detail)

### V.5.1 Setup

Survivors: Mara (anchor, bonded to T1/T2), T1, T2, T3 (witness, no tie to the
deceased). Raid outcome: one death (an unnamed survivor S0), T3 witnessed
class-violence trauma.

### V.5.2 Expected sequence

```text
step 1  consequence:raid_outcome (ledger)
step 2  grief owner observes death -> T1, T2 stage 1 + memorial + journal
step 3  T3 trauma trigger violence 0.4 -> record above threshold
step 4  contagion day 1: T1/T2 low mood affects T3 (bounded)
step 5  care available: therapy removes 0.12/session (diminishing)
step 6  no care path: T3 stabilizes at authored chronic band
```

### V.5.3 Findings this thread produced (illustrative)

| # | Finding | Class | Repair |
|---|---|---|---|
| PP-01 | grief and trauma both applied full morale hits; combined bound exceeded 1.8× | bound composition | authored combined cap; each contributes ≤60% |
| PP-02 | memorial recorded the deceased twice (two observers) | dedupe | memorial keyed by deceased id |
| PP-03 | contagion moved T3 lower than the grieving survivors' states (amplification) | damping | never-past rule |
| PP-04 | therapy session consumed no effort in one path | effort routing | route through capacity owner |
| PP-05 | journal entry for the death appeared twice (grief + memorial both wrote) | oracle | single consequence → single entry |
| PP-06 | T3 chronic path missing (no authored behavior at stable-high) | authoring | authored chronic band with care hook |
| PP-07 | rhythm: the deaths' names appeared inconsistently ("the deceased", name) | tone | corpus review + name registry |

### V.5.4 What the thread teaches

Death is the highest-traffic psychology event; every subsystem observes it.
The routing must guarantee **one observation per system, one bound across
systems, one name everywhere.**

---

## §V.6 Worked thread 2 — the long winter (full detail)

### V.6.1 Setup

20 days: rations cut (W3-02 policy), cold (W2-04), no deaths, two trauma-prone
subjects, caregiver capacity 6 effort/day.

### V.6.2 Expected sequence

```text
day 1-3   morale drains (deprivation bands); contagion spreads gloom (damped)
day 4     despair chain stage 1 for weakest subject (visible: quiet)
day 6     stage 2 (self-neglect); care prompt
day 7-9   interventions: care sessions consumed (capacity constraint binds)
day 10    second subject enters chain; capacity exceeded -> choose who to care
day 12    policy exit (rations restored) -> recovery curve
day 20    shelter morale recovering; chronic cases remain per authored
```

### V.6.3 Findings this thread produced

| # | Finding | Repair |
|---|---|---|
| PP-08 | shelter morale reached 0.02 by day 9 (below floor) | contagion floor damping + authored shelter floor |
| PP-09 | despair fired without stage-1 visibility for one subject (signal overwritten by another) | signal arbitration per subject |
| PP-10 | care capacity allowed 9 efforts/day (off-by-one in accounting) | capacity table single source |
| PP-11 | recovery after policy exit was instant | authored recovery curve |
| PP-12 | chronic case reacted to new triggers as if below cap | cap vs. recovery interaction rule |

### V.6.4 The capacity-bound drama

The interesting design surface: capacity binds, forcing triage. The audit
confirms triage is a *choice* (player selects who receives care), not a
silent priority list. Silent triage is a finding (dignity rule).

---

## §V.7 Worked thread 3 — the caregiver's strain (full detail)

### V.7.1 Setup

One caregiver (C) tends three trauma cases for 10 days; capacity 4/day;
strain accrues 0.1/session; authored strain crisis chain at band 3.

### V.7.2 Expected sequence

```text
day 1-4   C cares past capacity in two days (player choice over-scale)
day 5     strain band 2 (visible: C skips meals)
day 6     strain crisis chain stage 1 (withdrawal) — warning visible
day 7     intervention available: reduce care load (policy), tie support
day 8-10  if unattended: stage 2; authored outcome (breakdown, rest mandate)
          if attended: strain stabilizes, care throughput authorially reduced
```

### V.7.3 Findings this thread produced

| # | Finding | Repair |
|---|---|---|
| PP-13 | over-capacity care allowed (nothing stopped 9 sessions) | hard cap + warning |
| PP-14 | caregiver crisis used the same chain as despair (wrong interventions) | authored caregiver chain with load-reduction interventions |
| PP-15 | strain effects leaked to all survivors (contagion amplified caregiver) | contagion treats strain per authored isolation |
| PP-16 | rest mandate had no policy owner | route through schedule/policy owner (signed if new) |
| PP-17 | no positive path (successful care gave nothing back) | authored satisfaction band |

### V.7.4 Design lesson

Care is a two-person system. The plan's unit of analysis for care is the
**care dyad**, not the patient. Findings PP-13–17 all stem from analyzing only
one side.

---

## §V.8 Evidence design

```text
docs/evidence/w3-03/
  T1-*.yaml (owner map, static scan, schema, chain completeness)
  T2-*.yaml (per-kit runs)
  T3-*.yaml + distributions.csv
  findings/ (PP-### files)
  repairs/ (before/after)
  P0_PSYCH_PREMISE.md
```

### V.8.1 Findings file template

```text
FINDING PP-##
class: bound | routing | visibility | ownership | dignity | determinism
state: <affected>
evidence: file:line / run id
scenario: <thread>, step N
expected: <authored>
observed: <measured>
repair proposal: <one paragraph>
owner: <system>
status: proposed | accepted | repaired | debt
```

### V.8.2 Dignity findings

A separate tag (`dignity`) exists for review-class findings (tone, naming,
triage, coercion). These are adjudicated by the narrative lead, not fixed
mechanically. The plan keeps them visible rather than averaging them away.

---

## §V.9 Failure triage

```text
owner violation        -> stop the phase (second truth)
silent fatal           -> stop (non-negotiable rule)
missing warning        -> fix (fairness/dignity)
bound violation        -> fix (safety)
floor breach (soak)    -> fix or author a different floor (design decision)
decorative therapy     -> fix (substance)
capacity accounting    -> fix (single source)
flake                  -> quarantine with reason + hypothesis
```

---

## §V.10 Verification cost

| Tier | Effort |
|---|---|
| T1 (checks + generator + scan) | 5-6 days |
| T2 (10 kits) | 8-10 days |
| T3 (soak + distributions) | 3-4 days (shared) |
| Evidence/closeout | 1 day |

---

## §V.11 Test naming

```text
PsychOwner_<Check>      PsychRecord_<Check>
Trauma_<Check>          Crisis_<Check>
Therapy_<Check>         Contagion_<Check>
Relationship_<Check>    Grief_<Check>
Care_<Check>            Group_<Check>
```

---

## §V.12 Honesty notes

1. **Some findings will be design decisions.** PP-17 (no positive care path)
   is a design hole, not a code bug; the plan can surface it, the team must
   choose the fix.
2. **The floor value is an authoring decision** (proposal: 0.15). The soak
   reports; the team authors.
3. **Chronic outcomes are features**; the audit only ensures they are chosen,
   not accidental.
4. **Soak distributions are summaries**, not raw logs (repo hygiene).

---

*End of Part V. Continues in Part VI (Q&A + C-path + appendices).*# W3-03 · PART VI — EXTENDED Q&A, OPERATIONAL MODEL, AND C-PATH DESIGNS

---

## §VI.1 Governance Q&A (Q1–Q15)

**Q1. Does this plan diagnose real conditions?**
No. The systems model *fictional survivors under fictional pressure* with
authored, bounded mechanics. Names are in-fiction; clinical language is used
structurally (trauma/care/recovery are design terms here, not medical claims).
Content that could read as clinical advice is out of scope and rejected in
review.

**Q2. Who owns psychology state after integration?**
The existing owners: `SurvivorMentalHealthSystem`,
`PsychologicalSanatoriumSystem`, the relationship owner, `MemorialSystem`,
needs/modifier stack. This plan adds ownership discipline, schemas, and
routing — not a new system.

**Q3. Does Path B add save sections?**
No for the owner families that persist already. Records that need new
persistence (group state, if it exists) are signed C items.

**Q4. What if P0 finds no relationship owner?**
The write-map audit reports the state family as unowned. Path B proposes the
minimal owner with a signed line; if declined, the point reverts to Path A
(documentation only). No improvised second store.

**Q5. How does this plan treat the D19c statics?**
They are recorded as known findings owned by W2-02's repair (the lifecycle
fix). This plan's S1–S5 scan generalizes the rule to psychology paths and
verifies the three statics' *category* is covered; it does not fork the repair.

**Q6. Can quests write trauma directly?**
No. Quests (W3-01) route trigger events through the trauma owner. Direct
writes are findings.

**Q7. How are player choices that cause survivor suffering handled?**
As consequences with dignity: the choice has weight, the suffering has care
paths and warnings, and the game does not moralize. The review covers tone.

**Q8. What about a survivor the player wants to harm?**
Out of scope by the dignity rules. The systems model care and pressure, not
harm mechanics; if the game has hostile-to-NPC paths, they route through
combat/security (W3-04) with their own consequence rules.

**Q9. Does the plan touch difficulty?**
No. Bands/curves are authored values; W2-03 measures pressure. Tuning is not
this plan's authority.

**Q10. What if the soak floor can't hold without tuning?**
The floor is an authored design decision: either damping changes (within this
plan's models) or the floor is re-authored higher. The soak reports; the team
decides; W2-03 owns cross-system pressure.

**Q11. How is determinism enforced?**
Seeded RNG for any stochastic pairing/severity modifier; stable iteration
order for aggregation (sorted keys, never hash order); round-trip tests.

**Q12. What is the repair cap?**
Proposal: 20 per phase, ranked; the rest become debt records. Same discipline
as W3-01/W3-02.

**Q13. How do surprises get handled (a finding class not in this plan)?**
Recorded as a new class in the findings ledger; if recurrent, the plan offers
an amendment (as this expansion itself is an amendment). Structures stay open.

**Q14. Does the plan require a clinician?**
No. It requires a narrative/dignity reviewer for tone and a systems
engineer for ownership. Clinical realism is not the goal; human truth is.

**Q15. What are the hard stop-lines?**
(1) No second writer. (2) No unwarned crisis fatality. (3) No machine-coerced
care. (4) No clinical advice framing. (5) No save schema without signature.

---

## §VI.2 Method Q&A (Q16–Q30)

**Q16. Why is the owner map first?**
Every test compares state to owners; without the map, "one writer" is an
assumption. First artifact, first gate.

**Q17. Why are effects expressed as bands?**
Bands decouple owners (one system's numbers are another's readings) and keep
authoring human-readable. Numbers leak through systems; bands route.

**Q18. Why is contagion damped asymmetrically (misery slower than relief)?**
Doom-stacking is the failure mode of morale simulations; the asymmetry is a
deliberate design choice to keep hope mechanical, not just narrative.

**Q19. Why cap same-trigger trauma stacking?**
Linear accumulation punishes the player for surviving repeated events instead
of representing habituation and chronicity. Cap + recovery models the truth
better and prevents death spirals.

**Q20. Why is grief separate from trauma?**
Different causes, different recovery inputs (ritual/time/ties vs.
safety/care), different artifacts (memorials vs. marks). Merging them would
fork semantics, not simplify.

**Q21. Why does care consume a real capacity?**
Because unlimited care is unlimited pacing: the pressure disappears. Capacity
makes care a choice (triage), which is the human truth of scarcity.

**Q22. Why is silent triage a finding?**
Because the machine choosing who gets care removes the player's authorship of
the moral weight. Triage must be a choice (or an authored NPC policy with
consequences).

**Q23. How are NPC-to-NPC care and bonds modeled?**
Through the same owners (ties, care tasks), with NPC policies authored.
Players only see the surfaces; the machine treats NPCs as subjects with the
same rules.

**Q24. Why measure recovery in bands rather than percentages?**
Authoring and review are legible in bands; percentages invite false precision
in a system about people. Measurement still records raw deltas internally for
tests.

**Q25. How is the "no reveal beyond knowledge" rule applied here?**
Surfaces/displays (e.g., another survivor's trauma) respect visibility: the
player sees signals, not the state; the journal/oracle rules from W3-01
apply.

**Q26. What happens to psychology state on load?**
It restores through the owner's save section; records validate against their
schema; the lifecycle harness (P2) asserts exact equality. No recomputation
from events (double-application risk).

**Q27. How do scenes (funerals, confrontations) get authored?**
Through W3-01's corpus workflow. This plan declares the scene hooks and
outcomes; prose lands in the narrative plan's batch.

**Q28. Why is the caregiver a first-class subject?**
Because care work is real work with real cost; treating the caregiver as
infrastructure is both a design hole and a tone failure. The dyad rule (V.7.4)
is the fix.

**Q29. What if authors want more granular trauma (types within classes)?**
Modifiers within the class table, authored and bounded. New classes require
the vocabulary review. Granularity without bounds is how systems sprawl.

**Q30. What is the smallest Path A deliverable?**
P0 + owner map + static scan + record schema validation + crisis chain
completeness check. No repairs. A legitimate stop (Truth & Safety).

---

## §VI.3 Tooling Q&A (Q31–Q40)

**Q31. What new tooling exists?**
Owner-map generator, static scan, record validator, chain completeness
checker, the ten kits. Scripts + tests; no new runtime systems.

**Q32. What is reused?**
The needs/modifier stack for morale, the journal for visibility (W3-01), the
soak harness (W2-03), the evidence writer, the surface purity gates (W3-06).

**Q33. How does the static scan avoid false positives?**
Config vs. state classification with an allowlist for immutable tables;
allowlist entries require a one-line justification and are reviewed monthly.

**Q34. Where do soak distributions live?**
`docs/evidence/w3-03/T3-*.yaml` + a compact CSV; raw logs excluded per repo
hygiene.

**Q35. Can the kits run without the full game?**
Mostly: Core-level kits run in xUnit; visibility/scene kits need the host
(scripted scenarios). Split by tier as in §V.1.

**Q36. How do we test "no reveal beyond knowledge"?**
Scripted scenario: another survivor's trauma present, player not informed;
assert surfaces show only signals; after care/journal, assert disclosure
appears.

**Q37. How are record schemas versioned?**
`schema_version` per record kind; validators accept the current version;
migration is signed if persisted records change shape.

**Q38. What stops hidden RNG?**
Grep for non-seeded randoms in psychology paths + the determinism kit. The
repo's forbidden-api gate family (wave-2 recon) is the CI backstop.

**Q39. How is the owner map kept current?**
Generated from the declared writer table + scan; `--check` drift gate; writer
changes require a map update in the same PR.

**Q40. Evidence format?**
Same as W3-01/02: HEAD-stamped YAML/MD, per-finding files, before/after
repairs.

---

## §VI.4 Content Q&A (Q41–Q50)

**Q41. What voices do care/despair scenes use?**
The corpus voice (W2-06): restrained, human, no melodrama. The plan supplies
structure and hooks; prose review owns voice.

**Q42. How are survivor names handled in grief?**
Through the survivor record (name/pronouns); the name registry (shared with
W3-01's entity register) is the single source. Inconsistency (PP-07) is a
finding class.

**Q43. Can a survivor refuse care?**
Yes, as an authored state (refusal) with consequences; the machine never
forces care. Refusal is a person's choice, including when it costs them.

**Q44. What about children?**
If the game's survivor set includes children (verify at P0), authored rules
apply with extra review weight (dignity). The mechanic is identical; the
review bar is higher.

**Q45. How do addictions fit?**
If present in the game's item set, they are authored as a trauma-adjacent
class with recovery paths (care/support), never as a simple debuff. Scope
decision at P0.

**Q46. What tone for memorials?**
Names, dates, small artifacts — quiet. No heroic narration, no ranking. The
memorial is the game's conscience; restraint is the rule.

**Q47. How do relationships handle reconciliation after betrayal?**
Authored path: moment classes (apology, service, time) with caps; no instant
restore; permanent estrangement possible and authored.

**Q48. What if the player avoids all survivors (isolation play)?**
Valid strategy with authored consequences: groups/dynamics weaken, contagion
stops (D5), some content unavailable. Isolation is not punished beyond its
mechanics.

**Q49. How is humor handled?**
Through bars/scenes (W2-06); no psychology hook needed. The systems model
pressure and care; lightness lives in prose.

**Q50. What about幸存者 grief for the player's own deaths (permadeath)?**
If the game's mode includes permadeath (verify), the death is a consequence
to the shelter exactly like any loss: grief, memorial, journal. No special
player-only path.

---

## §VI.5 Operational model

### VI.5.1 Staffing

| Role | Count | Responsibility |
|---|---|---|
| systems engineer | 1 | owner map, scan, routing, kits |
| content author | 1 | classes, chains, protocols, scenes (hooks) |
| dignity reviewer | part-time | tone/name/triage review |
| verifier | shared | T2/T3, evidence |
| W2-02 liaison | part-time | statics/lifecycle coordination |

### VI.5.2 Schedule (4-5 weeks)

```text
Week 1  P0 + owner map + static scan; P1 repairs
Week 2  P2 schemas + P3 trauma routing/curves
Week 3  P4 chains + interventions; P5 therapy model
Week 4  P6 contagion + P7 relationships
Week 5  P8 grief + P9 care + P10 groups; soak; closeout
```

### VI.5.3 Dependencies

```text
P1 owner map -> everything
P2 records  -> P3, P8, P10
P3 trauma   -> P5 (therapy effect), P8 (bound composition)
P4 chains   -> P9 (caregiver crisis), P10 (cohesion crisis)
P6 contagion -> P10 (aggregation input)
W3-01 -> scenes, oracle; W3-02 -> effort economy; W3-04 -> trauma sources
W2-02 -> statics repair; W2-03 -> soak harness + bands
```

### VI.5.4 Cadence

```text
daily: T1 checks; twice-weekly: findings triage (with dignity queue);
weekly: owner-map regeneration + review; phase end: kits + evidence
```

### VI.5.5 Handoffs

| To | Handoff |
|---|---|
| W3-01 | crisis/grief scene hooks, choice consequences |
| W3-02 | effort costs, ration morale effects |
| W3-04 | trauma/grief trigger classes from combat |
| W3-05 | care supplies/items |
| W3-06 | needs/care/journal/relationship/memorial surfaces |
| W2-02 | static/lifecycle repairs (coordinated) |
| W2-03 | morale floor/band measurements |
| W2-06 | all text |

---

## §VI.6 C-path designs

### VI.6.1 C1 — Longitudinal survivor biographies

Authored per-survivor journals (a personal record read by the player after
death/retirement): composed from records (trauma, moments, care) into a
read-only bio. Rules: pure projection, no new state, visibility-respecting.
Cost ~3 days, surface work W3-06.

### VI.6.2 C2 — Relationship web model

A derived graph of ties (strength, flags) for analysis and authored events
(pairing scenes, group scenes). Read-only derivation; no new authority.
Enables "social weather" content (W2-06). Cost ~3 days.

### VI.6.3 C3 — Ritual economy

Memorials/grief rituals gain authored artifacts (markers, keepsakes) with
item costs (W3-05) and morale effects. The economy of remembrance; bounded.
Cost ~3 days.

### VI.6.4 C4 — Care specialization

NPC caregivers with authored specialties (trauma, grief, medicine) affecting
efficacy per class. Stored on the care owner; no new system. Cost ~4 days.

### VI.6.5 C5 — Collective resilience

Group-level authored traits (cultures of care) that modify contagion damping
and recovery — the shelter's character as mechanics. Aggregation-only; signed
persistence if stored. Cost ~4 days.

### VI.6.6 C6 — Trauma-informed difficulty

Authored difficulty presets referencing crisis rates/warning windows (not
severity caps — safety floors never scale). Warning windows may scale; floors
do not. Cost ~2 days, coordinate W2-03.

### VI.6.7 C7 — The quiet ledger

A read-only display of unrecognized kindnesses (care given, ties maintained,
rituals held) — the game's memory of decency. Derived from records only. Cost
~2 days.

### VI.6.8 C-bundle recommendation

```text
first:  C1 biographies + C7 quiet ledger (memory depth)
then:   C2 web + C5 resilience (social depth)
then:   C4 specialization + C3 ritual economy
last:   C6 difficulty weave (coordinate W2-03)
```

### VI.6.9 C signature block

```text
[ ] C1 biographies   [ ] C2 relationship web
[ ] C3 ritual economy [ ] C4 care specialization
[ ] C5 collective resilience [ ] C6 trauma-informed difficulty
[ ] C7 quiet ledger
```

---

*End of Part VI. Continues in Part VII (matrices and appendices).*# W3-03 · PART VII — IMPLEMENTATION CHECKLISTS AND REFERENCE SKETCHES

> Ten per-point checklists plus reference sketches for the owner API, the
> record validator, the contagion loop, and the crisis driver. These are the
> builder's working documents.

---

## §VII.1 Point 1 checklist — owner graph

```text
[ ] enumerate psychology state families (mental health, trauma, therapy,
    morale modifiers, relationships, grief, group)
[ ] for each, find ALL writers (code + data fields) with file:line
[ ] classify: single-module vs. multi-module; benign vs. conflict
[ ] repair conflicts: route through owner API (or declare new owner, signed)
[ ] document API surfaces (mutations each owner accepts)
[ ] static scan S1–S5 across psychology paths; classify findings
[ ] coordinate D19c statics with W2-02 (no duplicate repair)
[ ] generate docs/psychology/OWNER_MAP.md; wire --check drift gate
[ ] write routing kit; write no-static test
[ ] evidence: map, scan output, kit run
```

**DoD:** exactly one module writer per family; scan clean or allowlisted;
drift gate green.

---

## §VII.2 Point 2 checklist — records and static hygiene

```text
[ ] define record kinds and typed payload schemas (trauma, care, relation,
    grief)
[ ] implement the record validator (static + runtime)
[ ] author lifetimes per kind; wire lifecycle clearing
[ ] add schema_version; define additive-change rules
[ ] no prose in payloads (text refs only)
[ ] write lifecycle kit; write round-trip kit
[ ] verify iteration order stability (no hash-order)
[ ] evidence: schema docs, validator tests, lifecycle run
```

**DoD:** every record validates; lifetimes honored; round-trip exact.

---

## §VII.3 Point 3 checklist — trauma

```text
[ ] define the class table (six classes + authored severities)
[ ] implement ApplyTrigger (threshold, cap, record)
[ ] author effect bands per class; verify consumers
[ ] author recovery weights (safety, care, ties, interventions)
[ ] implement cap + same-trigger habituation
[ ] author chronic behavior for untreated cases (per class)
[ ] verify every trigger cites a consequence ref
[ ] write trauma kit (bands, cap, recovery, chronic, round-trip)
[ ] evidence: class table, kit run, curve plots (summaries)
```

**DoD:** bounded, sourced, recoverable or authored chronic; tests green.

---

## §VII.4 Point 4 checklist — crisis foresight

```text
[ ] enumerate crisis chains (despair, breakdown, caregiver strain, flight,
    authored others)
[ ] for each stage: visible signal path; intervention list; duration policy
[ ] crisis stage: authored outcomes; assert no unowned fatal
[ ] implement the chain driver (time/state advance)
[ ] implement signal arbitration (no overwritten warnings)
[ ] write foresight kit (visibility timing, intervention efficacy, advance)
[ ] dignity review of chains and interventions
[ ] evidence: chain docs, kit run
```

**DoD:** every chain warned and intervenable; no silent fatal; signals
visible.

---

## §VII.5 Point 5 checklist — therapy

```text
[ ] inventory therapy types and sanatorium capabilities
[ ] author session model (inputs, effect bands, limits, floors)
[ ] author diminishing returns per severity band
[ ] wire effort consumption (P9) and supply consumption (W3-05)
[ ] author caregiver effects (strain/satisfaction) and routing
[ ] implement floors (band 0) and relapse via new triggers
[ ] consent framing audit (no forced assignment)
[ ] write therapy kit (bands, diminishing, floor, effort, caregiver)
[ ] evidence: protocol docs, kit run
```

**DoD:** measurable, bounded, costly, humane (consent), floors respected.

---

## §VII.6 Point 6 checklist — contagion

```text
[ ] identify the contagion writer (single owner)
[ ] implement pressure formula with tie strength and susceptibility
[ ] apply damping D1–D5; verify asymmetry (dark slower than light)
[ ] implement never-past rule (T not worse than S)
[ ] implement convergence for loops; test oscillation absence
[ ] author isolation behavior (no ties = no spread)
[ ] write contagion kit (caps, damps, loop, floor, determinism)
[ ] evidence: formula doc, kit run, distribution summary
```

**DoD:** bounded, convergent, deterministic; shelter floor holds in soak.

---

## §VII.7 Point 7 checklist — relationships

```text
[ ] verify/declare the relationship owner; document API
[ ] author moment classes with sources and caps
[ ] author decay classes (family/stranger/etc.) and baselines
[ ] author band thresholds and visible states
[ ] implement per-event/day caps; verify no instant bonding
[ ] guard conflation: separate storage from scoped reputations
[ ] wire mourning flags from grief (P8)
[ ] write relationship kit (caps, decay, visibility, conflation, round-trip)
[ ] evidence: rule docs, kit run
```

**DoD:** single writer; bounded changes; visible transitions; decay works.

---

## §VII.8 Point 8 checklist — grief

```text
[ ] verify grief owner (MemorialSystem or mental health) and API
[ ] implement ObserveDeath routing (once per system, deduped)
[ ] author stage curves with names and durations
[ ] author ritual acceleration and scene hooks (W3-01)
[ ] author memorial artifacts (item/data refs) and persistence
[ ] implement combined bound with trauma; test the cap
[ ] handle absent-survivor observation (on return)
[ ] write grief kit (routing once, bound, stages, memorial, absent)
[ ] dignity/name review; evidence
```

**DoD:** one observation per system; bound respected; memorials persist and
name correctly.

---

## §VII.9 Point 9 checklist — caregiving

```text
[ ] verify the effort/capacity owner (schedule or needs); document API
[ ] author care tasks (inputs, effort, caregiver effect, supplies)
[ ] implement capacity gating; visible reason when blocked
[ ] implement hard cap + over-scale handling (player choice with cost)
[ ] route caregiver strain into the crisis chains (P4)
[ ] author the positive path (satisfaction/skill)
[ ] verify no silent assignment; policies are authored choices
[ ] write care kit (effort, gating, strain, no-silent, round-trip)
[ ] evidence: task table, kit run
```

**DoD:** care costs; capacity binds; caregiver is a subject; consent framing.

---

## §VII.10 Point 10 checklist — social dynamics

```text
[ ] verify/declare the group owner; document API and storage
[ ] implement aggregation (group mood derived from members)
[ ] author groups/divisions with stances
[ ] author group events with sources and bounded effects
[ ] route cohesion crises through P4 chains (warned)
[ ] verify no parallel per-survivor store
[ ] sign persistence if group state is stored
[ ] write group kit (aggregation, events, crisis, round-trip)
[ ] evidence: model doc, kit run
```

**DoD:** derived where derivable; stored where signed; effects routed.

---

## §VII.11 Reference sketch — owner API (illustrative)

```text
interface IMentalHealthOwner:
    ApplyTrigger(subject, triggerRef, class, modifiers) -> delta
    Severity(subject) -> band
    Progress(subject) -> CareProgress
    Records(subject) -> IReadOnlyList<PsychRecord>

interface IRelationshipOwner:
    ApplyMoment(a, b, momentRef, delta) -> appliedDelta   # capped
    Tie(a, b) -> TieState                                  # read-only
    DecayTick(day)                                         # owner-internal

interface IGriefOwner:
    ObserveDeath(subject, deceasedRef) -> GriefState
    Stage(subject) -> GriefStage
    AdvanceTick(day, ritualInputs)

interface ICareCapacityOwner:
    TrySpend(subject, effort) -> bool
    Capacity(subject) -> int
```

Rules: mutations return applied deltas (so callers can attribute);
reads return immutable snapshots; no owner exposes setters.

---

## §VII.12 Reference sketch — contagion loop (illustrative)

```text
ContagionTick(day):
    deltas := map()
    for (s, t) in tiesSortedByKey():            # deterministic order
        pressure := moodDelta(s) * strength(s,t) * susceptibility(t)
        pressure := clamp(pressure, -PAIR_CAP, +PAIR_CAP)
        if moodDelta(s) < 0: pressure *= DARK_DAMP
        if wouldMovePast(t, s): pressure := towardOnly(t, s, pressure)
        deltas[t] += pressure
    for t in deltas:
        deltas[t] := clamp(deltas[t], -DAY_CAP, +DAY_CAP)
        applyMoraleModifier(t, deltas[t])       # via needs owner stack
```

Notes: sorted iteration for determinism; both caps applied; never-past rule
before accumulation; application through the needs modifier stack (never
direct morale writes).

---

## §VII.13 Reference sketch — crisis driver (illustrative)

```text
CrisisTick(day):
    for c in activeChainsSortedById():
        stage := c.current
        if interventionApplied(c): c.regressOrResolve()
        elif elapsed(stage) >= stage.durationMax: c.advance()
        if transitioned(c):
            emitVisibleSignal(c.subject, stage, path)
            if stage.isCrisis:
                outcome := authoredOutcome(c)
                assert outcome != null
                route(outcome)                  # journal, care, memorial, etc.
```

The `assert outcome != null` is the no-silent-fatal enforcement point (with a
static completeness check in T1 as the first line).

---

## §VII.14 Reference sketch — grief routing (illustrative)

```text
OnDeath(deceasedRef):
    journal.Once(consequence:death(deceasedRef))          # oracle dedupe
    memorial.RecordOnce(deceasedRef, artifacts)           # dedupe by id
    for tie in tiesOf(deceasedRef).sorted():
        grief.ObserveDeath(tie.subject, deceasedRef)
        traumaMayApply(tie.subject, deathClass)           # severe cases
        relationship.MarkMourning(tie, deceasedRef)
```

One call site owns the whole death fan-out; every downstream system is
idempotent by key (deceased id / consequence id).

---

## §VII.15 Table: crisis chains at launch (proposal)

| Chain | Trigger | Stages | Crisis outcome set | Status |
|---|---|---|---|---|
| despair | sustained low morale + isolation | 2 warnings | resolve (care), escalate (authored scene) | proposed |
| breakdown | trauma cap + no care | 2 | resolve (rest/care), mandate (authored) | proposed |
| caregiver strain | capacity overuse | 2 | resolve (load cut), mandate (rest) | proposed |
| flight | betrayal + estrangement | 1 | stay (tie), leave (authored departure) | proposed |
| withdrawal | repeated refusals + gloom | 2 | resolve (tie), chronic (authored) | proposed |

Each row requires its chain data authored (visibility/interventions/outcomes)
before its phase ships.

---

## §VII.16 Table: record kinds and lifetimes

| Kind | Payload | Lifetime | Cleared by | Persistence |
|---|---|---|---|---|
| trauma | class, severity, ref | persistent | never (recovery reduces) | yes |
| care | stage, progress | session/arc | conclusion | yes if arc |
| relation moment | source, delta, day | persistent | never (history) | yes |
| grief | artifact_ref, stage | persistent | never | yes |
| group event | source, effects | persistent (history) | never | yes if group stored |

---

## §VII.17 Table: effect bands vocabulary

```text
morale:       severe-low | low | strained | steady | steady-high
sleep:        -2 | -1 | neutral | +1 bands
social:       withdrawn | quiet | engaged | open
functioning:  struggling | managing | stable
```

Consumers read bands; authors write bands; numbers exist only in the owner's
internal tests.

---

## §VII.18 Table: care tasks at launch (proposal)

| Task | Inputs | Effect band | Caregiver | Supplies |
|---|---|---|---|---|
| therapy session | 2h, facility | severity -0.12 *dim | strain +0.1 | none |
| bedside tending | 1h, infirmary | health morale +1 | strain +0.05 | medicine (if case) |
| vigil | 2h | grief stage speed +20% | strain +0.08 | none |
| rescue assist | 1h, risk | trauma risk -modifier | strain +0.15 | none |
| company | 1h | social band +1 | satisfaction +0.05 | none |

All rows authorable; no task without inputs.

---

## §VII.19 Dignity review protocol (operational)

```text
When: before sealing any psychology content
Who: narrative/dignity reviewer (part-time role)
Artifacts: chain docs, scene hooks, record texts, surfaces copy
Checklist:
  [ ] the survivor is named and particular, not a type
  [ ] the suffering has a cause and a care path
  [ ] no moralizing, no punishment framing
  [ ] warnings precede consequences
  [ ] no clinical advice framing
  [ ] the player's agency is choice, not optimization of personhood
  [ ] restraint in prose (W2-06 bar holds here)
Findings: dignity-tagged, adjudicated, recorded.
```

---

## §VII.20 Cross-plan matrices

### VII.20.1 Interaction matrix

| This plan → | Interface | Direction |
|---|---|---|
| W3-01 | scenes/hooks, consequences, oracle | out/in |
| W3-02 | effort economy, ration effects | out/in |
| W3-04 | trauma/grief triggers from combat | in (routed) |
| W3-05 | care supplies, facility items | in |
| W3-06 | surfaces (needs, care, journal, relationship, memorial) | out |
| W2-02 | static/lifecycle repair | coordinate |
| W2-03 | band measurement, floors | out/in |
| W2-06 | all visible text | out |
| W2-04 | weather/exposure stress sources | in |

### VII.20.2 Never-cross list

```text
[ ] never writes morale directly (modifier stack only)
[ ] never writes health (needs owner) except via documented care APIs
[ ] never introduces clinical diagnosis language or advice
[ ] never coerces care or auto-assigns damaging labor
[ ] never creates a second relationship/reputation store
[ ] never adds save sections in Path A/B
[ ] never removes warnings or floors for "difficulty"
```

---

## §VII.21 Expanded glossary

| Term | Definition |
|---|---|
| aggregation | derived group state from members (never stored independently) |
| band | qualitative effect level read by consumers |
| capacity | care effort budget per survivor/day |
| cap | hard bound on severity/deltas/contagion |
| chain | staged crisis with warnings and interventions |
| chronic | authored stable-high outcome without care |
| combined bound | max total morale effect from grief+trauma for one death |
| contagion damping | rules preventing runaway morale spread |
| dyad | care analyzed as caregiver+patient pair |
| effort | care work cost consumed from a capacity owner |
| floor (morale) | authored shelter minimum under sustained pressure |
| habituation | same-trigger severity growth toward a cap |
| intervention | player/policy action that regresses a crisis stage |
| memorial | persistent record of a deceased survivor |
| moment | a sourced relationship event |
| never-past rule | contagion cannot push a tie past the source's own state |
| owner map | state family → single writer registry |
| record | typed, schema-validated psychology datum |
| ritual | authored grief stage accelerator |
| signal arbitration | ensuring each warning remains visible despite others |
| strain (caregiver) | authored caregiver cost with crisis routing |
| susceptibility | per-survivor context multiplier on contagion |
| triage | choosing who receives scarce care (must be a choice) |
| visibility | player-facing signal of an internal state |
| withdrawal | first stage of several crisis chains |

---

## §VII.22 Artifact index

| Artifact | Type | Owner |
|---|---|---|
| `docs/psychology/OWNER_MAP.md` | generated | P1 |
| `docs/psychology/RECORD_SCHEMAS.md` | authored | P2 |
| `docs/psychology/CLASS_TABLE.md` | authored | P3 |
| `docs/psychology/CRISIS_CHAINS.md` | authored | P4 |
| `docs/psychology/CARE_PROTOCOLS.md` | authored | P5 |
| `docs/psychology/CONTAGION_RULES.md` | authored | P6 |
| `docs/psychology/RELATIONSHIP_RULES.md` | authored | P7 |
| `docs/psychology/GRIEF_AND_MEMORIAL.md` | authored | P8 |
| `docs/psychology/EFFORT_AND_CARE.md` | authored | P9 |
| `docs/psychology/SOCIAL_MODEL.md` | authored | P10 |
| `docs/evidence/w3-03/*` | evidence | all |

---

## §VII.23 Expanded signature sheet

```text
ASHFALL WAVE 3 · PLAN 3 (PSYCHOLOGY) · EXECUTION SIGNATURES
HEAD at signing: ________  Date: ________  Foreman: ________

[ ] P0 + owner map + static scan
[ ] P1 owner routing repairs
[ ] P2 schemas + lifecycle
[ ] P3 trauma classes + routing
[ ] P4 crisis chains + interventions
[ ] P5 therapy model + effects
[ ] P6 contagion model + damping
[ ] P7 relationship rules
[ ] P8 grief + memorial + bound
[ ] P9 care effort + caregiver routing
[ ] P10 group model (persistence: [ ] no [ ] signed)
[ ] repair cap per phase: ___ (proposal 20)
[ ] dignity review cadence: ___

Retained non-authorizations:
[x] No clinical advice framing; fictional systemic design only.
[x] No unwarned crisis fatal; no machine-coerced care.
[x] No second writer; no save schema without signature.
```

---

## §VII.24 Final control (W3-03)

```text
Document map:
  Part I    summary contract (original)
  Part II   deep designs 1-5
  Part III  deep designs 6-10
  Part IV   playbooks + scenario pack
  Part V    verification catalog + worked threads
  Part VI   Q&A + operational + C-path
  Part VII  checklists + sketches + matrices + appendices (this part)

Explicit stop: proposal only; no execution without Annex U + above
signatures. The hard stop-lines of Q15 and the never-cross list of VII.20.2
are non-negotiable within this document.
```

*Document control: W3-03 · Wave 3 (expanded) · HEAD 5be1a30a · end of W3-03.*# W3-03 · PART VIII — STRESS SCENARIOS, INCIDENT PLAYBOOKS, AND QUANTITATIVE TABLES

> Eight additional stress threads at compressed depth, the incident playbooks
> for real-session failures, and the numeric tables the plan relies on. This
> is the final content part of W3-03.

---

## §VIII.1 Stress thread: the quiet shelter (no events)

### VIII.1.1 Setup

21 days without raids, weather crises, or deaths; normal work; one difficult
survivor (chronic grief from a prior loss), one new arrival.

### VIII.1.2 Expected

```text
morale: steady-high trending; contagion minimits (relief spreads)
chronic grief: does not resolve without ritual/care (authored); the player
  sees the survivor's behavior (quiet, keepsakes) but nothing forces action
new arrival: ties form slowly (decay caps); integration moments authored
```

### VIII.1.3 Findings this thread catches

| Class | Example |
|---|---|
| chronic invisibility | chronic state has no visible signal → player never learns |
| stale records | grief record for a survivor who left the shelter unhandled |
| calm collapse | absence of events causes systems to fail (no idempotent tick) |

**Design note:** the quiet shelter is the test for **systems that only work
under pressure.** Ticks must be no-ops when nothing changes (no phantom
deltas, no drift, no accumulating hidden modifiers).

---

## §VIII.2 Stress thread: the mass casualty

### VIII.2.1 Setup

A shelter defense failure kills five survivors at once; twenty witnesses; two
caregivers.

### VIII.2.2 Expected

```text
grief fan-out: 5 deaths × ties -> bounded total; memorial batch; journal
  one entry per death (or one authored collective entry? — DECISION: per
  death, per the oracle rule; the collective scene is additional content)
trauma: witnesses apply class-violence, capped; combined bounds hold across
  five deaths for a single survivor (the widow case: two bonds lost)
care: capacity overwhelmed; triage forced; warnings active
group: cohesion crisis routed with warnings
```

### VIII.2.3 Findings this thread catches

| Class | Example |
|---|---|
| combined bound (multi-loss) | one survivor losing two bonds gets 2× grief → must be authored compound handling, not double |
| memorial batch atomicity | partial memorial write on a mid-batch crash |
| care overflow crash | capacity script dividing by zero when zero capacity available |
| triage silence | machine ordering care automatically |

**Key rule surfaced:** grief for multiple losses in one event composes per
authored "compounding rule" (e.g., each additional loss adds a diminishing
fraction), not linearly. The rule is authoring, tested.

---

## §VIII.3 Stress thread: the return from absence

### VIII.3.1 Setup

One survivor away 30 days (expedition); returns to changed shelter (two
deaths, one new bond formed by their partner, rationing).

### VIII.3.2 Expected

```text
relationship decay: ties weakened per class; re-bonding moments available
grief retroactive: deaths during absence observed on return (authored stages
  start at return, not backdated — the survivor was not there)
partner's changes: relationship flags authored (drifted, new bond moment)
group reaction: authored (return scenes, suspicion, welcome)
```

### VIII.3.3 Findings

| Class | Example |
|---|---|
| backdated grief | grief applied with 30-day-old timestamps → wrong stage duration |
| missing return observation | deaths during absence never observed (silent hole) |
| relationship state mismatch | partner's new tie conflicting with old band without authored path |

---

## §VIII.4 Stress thread: the adolescent (if survivor set includes children)

### VIII.4.1 Review-only thread

If minors exist in the survivor set: every artifact passes the heightened
dignity review. The mechanical rules are identical (no special-case system);
the review bar is higher. Findings of the review class are never mechanically
"fixed" — they are adjudicated.

### VIII.4.2 Checks

```text
[ ] minors have care paths appropriate to their authored role
[ ] no trauma exposure authoring that reads as gratuitous
[ ] memorial/naming dignity for minors
[ ] group dynamics authored without exploitation framing
[ ] if the game's content boundaries exclude certain material, verified absent
```

---

## §VIII.5 Stress thread: the long death (terminal illness)

### VIII.5.1 Setup (if the game has illness)

A survivor with an authored terminal condition over 14 days: anticipatory
grief, care burden rise, final death.

### VIII.5.2 Expected

```text
anticipatory grief: authored stage (before death) — a distinct authored state
care: tending cost rises (effort curve); caregiver strain path active
final death: normal grief + memorial; no double (anticipatory + post)
```

### VIII.5.3 Findings

| Class | Example |
|---|---|
| double grief | anticipatory stage not closed → post-death adds full stage 1 again beyond bound |
| effort spike unhandled | tending cost curve not authored per stage |
| dignity | terminal care scenes reviewed hardest |

---

## §VIII.6 Stress thread: reconciliation

### VIII.6.1 Setup

Estranged pair (betrayal) with authored reconciliation path (service, time,
apology scenes).

### VIII.6.2 Expected

```text
estrangement flag persists until authored moments accumulate
reconciliation: moments with caps; band restores over authored days
permanent estrangement: authored alternative (some wounds don't close)
both outcomes contentful (no dead-end)
```

### VIII.6.3 Findings

| Class | Example |
|---|---|
| instant reconciliation | caps missing |
| dead-end | estrangement flag with no path and no authored permanence |
| flags conflated | estrangement and mourning flags overwriting each other |

---

## §VIII.7 Stress thread: the player's isolation policy

### VIII.7.1 Setup

Player minimizes social interaction (no talks, no care spending) for 30 days.

### VIII.7.2 Expected

```text
contagion: minimal (isolation rule)
decay: ties decay; groups weaken (authored)
consequences: authored (some content unavailable; cohesion low but stable)
no punishment spiral: isolation is a strategy, not a death sentence
```

### VIII.7.3 Findings

| Class | Example |
|---|---|
| hidden punishment | cohesion loss cascading into unwarned crisis |
| group death | group state collapsing to invalid values |
| content dead-ends | scenes assuming social ties fire with none (crash) |

---

## §VIII.8 Stress thread: save/load at crisis midpoints

### VIII.8.1 Setup

Save at: mid-trauma recovery, mid-crisis chain stage 2, mid-grief stage,
mid-care session, during contagion tick.

### VIII.8.2 Expected

```text
exact restoration: stages, progress, records, chain positions
no double-apply: loading does not re-trigger consequences
no lost warnings: signals reappear per state
determinism: post-load evolution equals uninterrupted evolution
```

### VIII.8.3 Findings

| Class | Example |
|---|---|
| chain position loss | crisis chain restarts at stage 1 |
| record re-creation | trauma record duplicated on load |
| signal loss | warning not re-shown after load |
| contagion replay | contagion tick re-applies after load |

---

## §VIII.9 Incident playbooks (real-session failures)

### VIII.9.1 Incident: shelter morale collapse in play

```text
symptom: morale races to floor within days of a raid
triage:
  1. check contagion deltas in logs (cap violations?)
  2. check combined bounds for the triggering deaths
  3. check modifier stack for duplicate entries
  4. check crisis chains firing in parallel without arbitration
containment: pause the relevant source (feature gate in debug)
postmortem: finding + kit case + regression test
```

### VIII.9.2 Incident: survivor "stuck" in a state

```text
symptom: a survivor never changes state (frozen)
triage: owner write-map (who should write?), record validation, chain driver
  liveness (tick advancing?)
root causes: a second writer overwriting the owner; a chain missing a
  transition; a record schema rejection swallowed
postmortem: kit case; add liveness assertion to soak
```

### VIII.9.3 Incident: care with no effect

```text
symptom: sessions run, nothing changes
triage: effect bands consumed? effort owner gating? floor already reached?
postmortem: decorative-care finding; kit case
```

### VIII.9.4 Incident: grief for someone who didn't die

```text
symptom: grief observed for a living survivor
triage: consequence ref correctness; dedupe key collision; rename/reuse of
  survivor ids (the identity bug class)
postmortem: id-stability guard (survivor ids never reused)
```

### VIII.9.5 Incident: warning spam

```text
symptom: dozens of warnings in one day
triage: signal arbitration; chain triggers too eager; per-day caps
postmortem: arbitration review; authored warning cadence
```

---

## §VIII.10 Quantitative tables

### VIII.10.1 Trauma class table (proposal)

| Class | Severity | Same-trigger factor | Recovery weight base | Chronic band |
|---|---|---|---|---|
| violence | 0.40 | 0.75× | 0.10/day (care) | strained |
| loss | 0.35 | 0.70× | 0.08/day | low |
| deprivation | 0.30 | 0.80× | 0.12/day | strained |
| exposure | 0.25 | 0.80× | 0.10/day | steady-low |
| betrayal | 0.45 | 0.70× | 0.07/day | low |
| confinement | 0.35 | 0.75× | 0.09/day | strained |

Caps per class at 1.0; thresholds for records 0.30.

### VIII.10.2 Contagion parameters (proposal)

| Parameter | Value | Note |
|---|---|---|
| pair daily cap | ±0.06 | per direction |
| survivor daily cap | ±0.10 | total |
| dark damping | 0.6× | misery slower |
| light damping | 1.0× | relief normal |
| susceptibility base | 0.5–1.2 | per class/context |
| shelter floor | 0.15 | authored; not a cap |

### VIII.10.3 Care capacity (proposal)

| Context | Capacity/day | Notes |
|---|---|---|
| healthy caregiver | 4 effort | rest-affected |
| strained caregiver | 2 effort | authored bands |
| crisis caregiver | 0–1 effort | crisis chain active |
| overflow | blocked | visible reason |

### VIII.10.4 Grief stage durations (proposal)

| Stage | Base days | Ritual accelerator | Tie support |
|---|---|---|---|
| numbness | 2 | — | — |
| weight | 4 | −30% | −10%/day |
| remembering | 6 | −25% | −15%/day |
| carrying | ongoing | — | — |

Compounding rule for multi-loss: additional loss in the same event adds 50%
of base duration, diminishing per loss.

### VIII.10.5 Relationship delta caps (proposal)

| Moment class | Cap/event | Cap/day | Decay class |
|---|---|---|---|
| shared danger | +0.12 | +0.20 | standard |
| kindness | +0.08 | +0.15 | standard |
| betrayal | −0.40 | −0.40 | slow |
| reconciliation | +0.10 | +0.15 | slow (needs moments) |
| neglect (decay) | −0.02/day | — | per class |

### VIII.10.6 Group effects (proposal)

| Event | Cohesion | Trust | Division shift |
|---|---|---|---|
| ration dispute | −0.10 band | −0.05 | workers ↔ refugees ±1 |
| festival | +0.15 band | +0.05 | converge |
| casualty event | −0.08 band | −0.02 | grief shared +1 |
| betrayal revealed | −0.20 band | −0.10 | split +2 |

### VIII.10.7 Crisis chain durations (proposal)

| Chain | Stage 1 max | Stage 2 max | Crisis window |
|---|---|---|---|
| despair | 5 days | 4 days | 2 days |
| breakdown | 4 | 3 | 2 |
| caregiver strain | 3 | 3 | 2 |
| flight | 3 | — | 1 |
| withdrawal | 5 | 4 | 2 |

---

## §VIII.11 What remains open (honest gaps)

1. **Illness/disability representation** (if in scope): needs a P0 decision;
   this plan supports it structurally (records/chains) without inventing
   content.
2. **Addiction systems**: authored only if the item set supports it; flagged.
3. **NPC-NPC romance/family structures**: authored through ties; not
   specifically designed here (a content decision).
4. **Player-character mental state**: if the player character is modeled
   separately (verify), the same owners apply; no special system.
5. **Localization of psychology text**: follows the freeze; text refs already
   the discipline.

These gaps are recorded rather than hidden; each has an owner decision
required before authoring.

---

## §VIII.12 Final assurance list (for the builder)

```text
[ ] I know every state family's single writer.
[ ] I know every effect is a band read by consumers.
[ ] I know every crisis has warnings and interventions.
[ ] I know care costs effort and the caregiver is a person.
[ ] I know grief is separate from trauma and they compose under a bound.
[ ] I know relationships decay and caps prevent instant bonds.
[ ] I know groups aggregate; they are not a second store.
[ ] I know records validate, persist, and carry no prose.
[ ] I know determinism and round-trip are tested, not assumed.
[ ] I know the dignity review precedes every seal.
```

---

## §VIII.13 Closing note and control

**W3-03 status:** expanded with Part I (summary) + Parts II–VIII (deep designs,
playbooks, verification, worked threads, Q&A, C-path, checklists, sketches,
stress scenarios, incident playbooks, numeric tables). The plan is a
proposal; the stop-lines stand.

The last word belongs to the tone rule, because this is the plan where it
matters most: **ASHFALL models pressure and care so that survival means
something. The systems in this plan exist to make kindness costly and
possible, never to make suffering efficient.** If a mechanic makes cruelty
cheaper than care, it is wrong regardless of its tests — re-author it.

*Document control: W3-03 · Wave 3 (expanded) · HEAD 5be1a30a · end of W3-03.*# W3-03 · PART IX — SAMPLE DOCUMENTS, REFERENCE TABLES, AND CLOSING APPENDICES

> Worked examples of every document the plan produces, plus the reference
> tables a builder keeps open. Final part of the expanded W3-03.

---

## §IX.1 Sample: owner map entry (generated format)

```markdown
## State family: trauma_severity
- Writer: SurvivorMentalHealthSystem (Core/Psychology/SurvivorMentalHealthSystem.cs)
- API: ApplyTrigger(subject, ref, class, modifiers) -> delta
       Severity(subject) -> band
- Consumers: needs effects (bands), sanatorium therapy, journal signals,
             crisis driver (breakdown chain)
- Storage: owner save section (verified: <section name at P0>)
- Static scan: clean
- Drift check: generated 2026-09-21; source hash <sha>
```

### IX.1.1 Owner map review rules

1. Every entry shows the writer file and API signature.
2. Consumers list is complete or marked "pending P0 verification".
3. Storage names the actual save section or says "session-only" explicitly.
4. A missing entry for a state family is itself a finding (the map is total
   over the families list).

---

## §IX.2 Sample: record schema doc (trauma mark)

```markdown
## Record kind: trauma_mark (schema_version 1)

| field | type | required | notes |
|---|---|---|---|
| subject | survivor_id | yes | stable ids only |
| class | enum(violence, loss, deprivation, exposure, betrayal, confinement) | yes | closed set |
| severity | float [0,1] | yes | capped |
| trigger_ref | consequence_id | yes | ledger reference |
| threshold_day | int | yes | day the record was created |
| visible | bool | yes | surfacing |

Invariants:
- severity >= threshold at creation (0.30)
- trigger_ref resolves in the ledger
- never deleted; recovery lowers severity only

Migration: additive fields only within version 1; version bump for shape
change (signed).
```

### IX.2.1 Validator behavior

```text
Reject: unknown class, missing ref, severity > cap, prose in any field.
Warn (baseline): severity exactly at threshold (boundary records).
Report: per-kind counts; rejected list with reasons.
```

---

## §IX.3 Sample: crisis chain doc (despair)

```markdown
## Chain: despair
Trigger: sustained low morale + isolation (authored predicate)
Driver: time/state; no hidden rolls

### Stage 1: withdrawal (max 5 days)
- Visible: quiet bark; sleep −1 band; journal note
- Interventions: talk (tie ≥ steady), rest schedule, care session
- Advance: duration max -> stage 2 (not silent)

### Stage 2: self-neglect (max 4 days)
- Visible: needs degrade faster; care prompt on needs surface
- Interventions: care, tie talk, authored scene (W3-01 hook)
- Advance: duration max -> crisis

### Crisis: authored outcomes
- resolved: severity −0.15, morale band +1, care continues
- escalated: authored scene outcome (rest mandate / breakdown chain)
- NEVER: unowned death/maim (hard rule)

### Composition
- Trauma interplay: if trauma betrayal present, stage durations −20%
- Contagion: subject's signals do not update others beyond caps
```

### IX.3.1 Chain completeness checklist (static)

```text
[ ] every stage has visible paths
[ ] every stage has ≥1 intervention with a registered test
[ ] crisis outcomes resolve to owner operations
[ ] no outcome references an unregistered operation
[ ] durations authored; no infinite stages
```

---

## §IX.4 Sample: care protocol doc (therapy session)

```markdown
## Protocol: therapy_session
- Inputs: caregiver 2 effort; facility: sanatorium bed (occupied);
  supplies: none
- Effect: severity −0.12 (diminishing: session N effect = base × (0.85^(N-1)))
  morale stabilize: +1 band; insight record (optional authored)
- Caregiver: strain +0.10 (bounded); satisfaction +0.03 if effect > 0
- Limits: 1 per subject/day; diminishing after 3 consecutive
- Floor: severity cannot go below 0 (stable, not erased)
- Consent: assigned by player policy or NPC choice; never machine-forced
- Testing: effect band ±20%; diminishing monotone; floor respected;
  effort consumed; consent paths exercised
```

### IX.4.1 Care table quick reference

| Task | Effort | Effect | Caregiver | Supplies |
|---|---|---|---|---|
| therapy | 2 | −0.12 dim | strain +0.10 | — |
| tending | 1 | needs band +1 | strain +0.05 | medicine (case) |
| vigil | 2 | grief speed +20% | strain +0.08 | — |
| rescue assist | 1 | trauma risk −mod | strain +0.15 | — |
| company | 1 | social band +1 | satisfaction +0.05 | — |
| over-scale | blocked | — | — | — |

---

## §IX.5 Sample: grief and memorial doc

```markdown
## Grief: departed_bonded
- Observe: death consequence (once per system)
- Stages: numbness(2d) -> weight(4d) -> remembering(6d) -> carrying
- Accelerators: ritual −25–30%; tie support −10–15%/day
- Effects: morale low; social withdrawn (bands)
- Memorial: name, day, artifact class; persists
- Combined bound: grief + trauma morale effect ≤ 0.6 total (authored)
- Multi-loss: additional loss +50% base duration, diminishing per loss

## Memorial
- One entry per deceased id (dedupe key)
- Artifacts reference real item/data ids
- No removal except signed feature
- Names per survivor record (single source)
```

### IX.5.1 The name-registry handshake

The survivor name registry (shared with W3-01's entity register) is the single
source of names/pronouns. Grief text refs interpolate from it; a hardcoded
name in a grief line is a finding (PP-07 class).

---

## §IX.6 Sample: relationship rules doc

```markdown
## Relationship model
- Storage: relationship owner (pair key, stable survivor ids)
- Change: ApplyMoment(a,b,ref,delta) -> capped applied delta
- Caps: shared danger +0.12/event; betrayal −0.40/event; per-day totals
- Decay: standard −0.02/day after 3 days idle; slow classes half
- Bands: hostile, cold, steady, close, bonded (thresholds authored)
- Visibility: journal relationship entries on band changes
- Flags: estranged, mourning, indebted (authored states)
- No conflation: scoped mercenary reputation is separate storage

## Bereavement interplay: mourning flag set by grief owner; read by prose/arcs
```

---

## §IX.7 Sample: group model doc

```markdown
## Group: shelter
- Storage: group owner (signed) or derived-only (B path)
- Mood: derived = weighted aggregate of member morale bands
- Cohesion/trust: stored values [0,1] with authored effects
- Divisions: authored groups with stance values; shift tables
- Events: ration dispute, festival, casualty, betrayal (bounded effects)
- Crises: cohesion < authored band -> chain (warned, intervenable)
```

---

## §IX.8 Reference: the morale band ladder

```text
band 0  severe-low    needs effects worst; crisis chains most likely
band 1  low           visible signals; care recommended
band 2  strained      ordinary under pressure
band 3  steady        baseline
band 4  steady-high   contagion source of relief (damped)
```

Consumers map bands → effects; authors never write numeric morale outside the
owner.

---

## §IX.9 Reference: effect routing table (who reads what)

| Effect band | Read by | Effect |
|---|---|---|
| morale band | needs owner | needs modifiers |
| sleep band | needs owner | fatigue rate |
| social band | proximity/encounter owners | bark selection |
| functioning band | work/schedule owners | task efficiency |
| crisis state | journal/needs surfaces | warnings |
| grief stage | prose/scene selection | available scenes |
| relationship flags | prose/arc triggers | content eligibility |

---

## §IX.10 Reference: the visibility matrix

| Internal state | Player learns via | Latency |
|---|---|---|
| morale band low | needs surface | immediate |
| trauma severity | signals (sleep, quiet) | gradually |
| crisis stage | warnings (journal/needs) | authored window |
| grief stage | behavior + journal | immediate (entry) |
| relationship band | journal entries | on change |
| group cohesion | shelter surface | on change/band |
| caregiver strain | needs surface of caregiver | as bands cross |

---

## §IX.11 Reference: determinism rules (psychology)

```text
D1 seeded RNG only (facade) — no System.Random, no wall clock
D2 stable iteration (sorted survivor ids; no hash order)
D3 one apply point per consequence (idempotency keys)
D4 no recomputation on load (restore exact state)
D5 tick order authored and stable (owners processed in fixed order)
D6 tests: same seed + same script ⇒ identical band histories
```

---

## §IX.12 Reference: save/restore checklist

```text
[ ] every persisted family round-trips exactly (kit)
[ ] records validate after restore
[ ] chain positions restored; warnings re-shown
[ ] no double-apply on load (consequence keys checked)
[ ] session-scoped statics rebuilt, not stale (D19c discipline)
[ ] restore order stable across runs
```

---

## §IX.13 Sample finding: dignity class

```markdown
FINDING PP-31 (dignity)
class: dignity
state: care assignment policy
evidence: policy auto-assigns caregivers by highest skill without consent
scenario: caregiver strain thread, step 3
expected: assignment is a player policy or authored NPC choice
observed: silent priority ordering caused strain without disclosure
repair proposal: surface assignment; default to manual triage; authored NPC
  policies opt-in with their own consequences
adjudication: narrative lead — appointment required: yes / no / modified
```

Dignity findings are never auto-fixed; they are reviewed.

---

## §IX.14 Sample: soak report excerpt

```yaml
run: T3-2026-11-a
seeds: 20  days: 60
morale:
  shelter_min: 0.16  floor: 0.15  pass: true
  individual_chronic: 7   authored_cases: 7   unexplained: 0
chains:
  fired: 34  warned: 34  unwarned: 0
  resolved: 26  escalated: 8  unowned_fatal: 0
trauma:
  cases: 41  cap_hits: 3  recovered: 29  chronic_authored: 9
care:
  sessions: 88  effort_consumed: 88  free_sessions: 0
  over_capacity_attempts: 12  blocked_visible: 12
grief:
  deaths: 6  observations: 6 (dedupe: 0)  memorial_entries: 6
  combined_bound_violations: 0
determinism: pass
notes: two design items flagged (PP-31, floor calibration question)
```

---

## §IX.15 Sample: closeout memo

```markdown
# W3-03 Closeout
OUTCOME: psychology owners audited; second writers eliminated; trauma/crisis/
care/grief/relationship/group contracts documented and tested; soak holds the
authored morale floor with zero unwarned crisis escalations.
FILES: docs/psychology/*.md; owner map; kits; evidence T1/T2/T3.
CONTRACT: single-writer map; band-based effects; warned chains; care dyad;
combined grief+trauma bound; retirement of direct morale writes.
COMMANDS: run_test.sh Psych*; psych-a11y? no; soak harness run T3-...
RESULTS: T1 green; T2 10/10 kits; T3 floors held across 20 seeds.
LIMITATIONS: NPC-NPC romance not authored (content decision); children review
pending survivor-set confirmation; capacity calibration authored values
provisional.
SHARED PATHS TOUCHED: <list per ownership doc>
LEDGER PROPOSALS: <debt rows for deferred items>
ANNEX U RELEASES: crisis/escalation consumers; W3-01 scene hooks; W3-04
trauma sources documented.
```

---

## §IX.16 The tone bible extract (psychology-specific)

```text
VOICE
  restraint, human scale, no melodrama
  describe, never judge
  the quiet detail over the dramatic statement
  care reads as work, not magic

FORBIDDEN
  clinical advice framing
  moralizing about choices
  suffering as spectacle
  named real-world conditions/people
  humor at a survivor's expense
  numbers as player-facing tragedy meters

REQUIRED
  warnings before consequences
  a path wherever there is a doom
  names spelled and used correctly
  the caregiver seen
  the dead remembered plainly
```

---

## §IX.17 Re-audit triggers

Re-run this plan's T1 when:

```text
[ ] a new survivor-affecting system lands
[ ] a new status/condition vocabulary is introduced
[ ] any new static in psychology paths appears
[ ] save-version changes touch psychology records
[ ] a new crisis-like mechanic appears in any wave
```

T2 re-runs per affected kit; T3 on the shared schedule.

---

## §IX.18 Final control

**W3-03 final structure:**

```text
Part I    summary contract
Part II   deep designs 1-5
Part III  deep designs 6-10
Part IV   authoring playbooks + scenario pack
Part V    verification catalog + worked threads
Part VI   Q&A + operational + C-path
Part VII  checklists + sketches + matrices + appendices
Part VIII stress scenarios + incident playbooks + numeric tables
Part IX   sample documents + reference tables (this part)
```

**Explicit stop:** proposal only. No execution without Annex U and the VII.23
signatures. The stop-lines (Q15) and never-cross list (VII.20.2) are binding
within this document.

*Document control: W3-03 · Wave 3 (expanded) · HEAD 5be1a30a · end of W3-03.*# W3-03 · PART X — THE CRISIS CHAIN CATALOG (FULL AUTHORING SPECIFICATION)

> Every crisis chain, stage by stage, with its signals, interventions,
> outcomes, compositions, and test cases. This is the authoring reference
> the builders work from; the tables in Part VIII are its summary.

---

## §X.1 Chain: despair (full specification)

### X.1.1 Predicate

```text
enter: morale band <= 1 for >= 3 days
       AND (isolation: ties < 1 OR social band withdrawn)
       AND no active care session in last 2 days
```

### X.1.2 Stage 1 — withdrawal

```text
signal: quiet bark set; sleep -1 band; needs surface journal note
interventions:
  talk: requires tie >= steady; effect: stage progress -1 (roll? no — authored)
  rest schedule: requires player policy; effect: +1 morale band over 2 days
  care session: sanatorium; effect: stage regress per therapy effect
duration_max: 5 days -> advance
determinism: no rolls; authored predicates only
```

### X.1.3 Stage 2 — self-neglect

```text
signal: needs degrade faster (authored modifier); care prompt (surface)
interventions:
  care: as above (stronger weight)
  tie talk: requires tie >= close
  authored scene hook: 'the quiet room' (W3-01; resolves or deepens)
duration_max: 4 days -> advance
```

### X.1.4 Crisis — authored outcomes

```text
resolved:      severity -0.15; morale band +1; care continues
escalated:     authored scene outcome (rest mandate OR breakdown chain entry)
never:         unowned death/maim
routing:       journal entry once (choice/world category per authored)
```

### X.1.5 Composition

```text
trauma interplay: betrayal present -> durations -20%
contagion: subject's low mood affects ties (damped, P6 rules)
caregiver: sessions consume effort (P9); caregiver strain path independent
```

### X.1.6 Full test set

```text
Crisis_Despair_SignalsVisible_Stage1
Crisis_Despair_SignalsVisible_Stage2
Crisis_Despair_InterventionTalk_Regresses
Crisis_Despair_InterventionRest_Regresses
Crisis_Despair_InterventionCare_Regresses
Crisis_Despair_NoIntervention_Advances
Crisis_Despair_Crisis_OutcomeAuthored
Crisis_Despair_NoUnownedFatal
Crisis_Despair_Composition_BetrayalAccelerates
Crisis_Despair_RoundTrip_StagePreserved
```

---

## §X.2 Chain: breakdown (full specification)

### X.2.1 Predicate

```text
enter: trauma severity >= band 2 (authored threshold)
       AND no care session in last 4 days
```

### X.2.2 Stage 1 — disturbance

```text
signal: startle barks; sleep -2 band; work efficiency -1 band
interventions: care (strong), company (tie), rest mandate (policy)
duration_max: 4 days
```

### X.2.3 Stage 2 — crisis approach

```text
signal: needs surface warning; journal note; authored photograph/keepsake
  behavior (readable detail)
interventions: care, authored scene (W3-01), tie vigil
duration_max: 3 days
```

### X.2.4 Crisis — outcomes

```text
resolved: severity -0.20; functioning restored to strained
mandate:  authored rest policy window (no work for N days)
escalated: authored scene; possibly confinement-chain entry (if authored)
never: unowned fatal
```

### X.2.5 Compositions

```text
caregiver: draws the caregiver dyad into play (P9 tests)
group: cohesion -authored band if public (authored scene decision)
```

### X.2.6 Test set

```text
Crisis_Breakdown_Thresholds
Crisis_Breakdown_Interventions_Efficacy
Crisis_Breakdown_NoUnownedFatal
Crisis_Breakdown_Composition_CaregiverStrain
Crisis_Breakdown_RoundTrip
```

---

## §X.3 Chain: caregiver strain (full specification)

### X.3.1 Predicate

```text
enter: caregiver effort used >= capacity * 1.5 for >= 3 days
       (over-scale is a player policy choice, never machine-assigned)
```

### X.3.2 Stage 1 — depletion

```text
signal: caregiver needs degrade; barks mention tiredness; needs surface
interventions:
  load reduction: player policy (reduce care assignments)
  rest: schedule policy
  tie support: talks with close ties
duration_max: 3 days
```

### X.3.3 Stage 2 — strain

```text
signal: work efficiency -2 bands; care effect halved (authored)
interventions: load reduction (strong), authored scene (the confession)
duration_max: 3 days
```

### X.3.4 Crisis — outcomes

```text
resolved: strain band drops; care capacity restored over authored days
mandate:  authored rest window; care halted (patients' care pauses — with
          consequences routed to their chains, authored)
escalated: breakdown chain entry (the caregiver is a survivor too)
never: unowned fatal
```

### X.3.5 Composition

```text
patients: their chains' durations extend by the care interruption (authored)
group: cohesion notices (authored scene optional)
```

### X.3.6 Test set

```text
Crisis_Strain_OverScaleAuthored
Crisis_Strain_Interventions_LoadReduction
Crisis_Strain_CareEffectHalved
Crisis_Strain_Composition_Patients
Crisis_Strain_RoundTrip
```

---

## §X.4 Chain: flight (full specification)

### X.4.1 Predicate

```text
enter: estrangement flag + betrayal moment in last N days
       OR group hostility authored threshold
```

### X.4.2 Single stage — decision

```text
signal: packing behavior (bark/item), journal note, missing at a check
interventions:
  talk: requires tie >= steady (can regress)
  authored scene: 'the road out' (W3-01; resolve or deepen)
  restitution: authored (apology/service moments)
duration_max: 3 days -> departure
```

### X.4.3 Outcome — departure

```text
departed: survivor leaves (or tries); authored consequences: shelter loss,
  relationship flag 'departed', memorial-like journal entry, potential
  return arcs (authored; absent-by-design)
```

### X.4.4 Test set

```text
Crisis_Flight_Interventions
Crisis_Flight_DepartureAuthored
Crisis_Flight_ReturnArcHooks
Crisis_Flight_RoundTrip
```

---

## §X.5 Chain: withdrawal (full specification)

### X.5.1 Predicate

```text
enter: repeated refusing care (authored count) + low social band
       (the survivor pushes help away — a person's choice)
```

### X.5.2 Stage 1 — distance

```text
signal: avoids conversations; eats alone; journal tone
interventions: company (non-care; presence), shared work, authored scene
duration_max: 5 days
```

### X.5.3 Stage 2 — silence

```text
signal: minimal barks; tie decay accelerates (authored)
interventions: persistence (company, small gestures), authored scene
duration_max: 4 days
```

### X.5.4 Crisis — outcomes

```text
resolved: social band +1; care becomes acceptable again (state flip)
chronic:  authored stable withdrawal (survivor remains part of the shelter
          but apart; scenes/content still available; NOT "broken")
never: unowned fatal
```

### X.5.5 The dignity rule for this chain

Withdrawal is not a failure state to be "fixed". The chronic outcome is a
valid life: the survivor keeps their distance and remains themselves. Content
must not treat re-engagement as the only good ending. This is a review
criterion, explicitly.

### X.5.6 Test set

```text
Crisis_Withdrawal_Interventions
Crisis_Withdrawal_ChronicAuthored
Crisis_Withdrawal_ContentAvailable_Chronic
Crisis_Withdrawal_RoundTrip
```

---

## §X.6 Chain: confinement (if capture/illness holds a survivor)

### X.6.1 Predicate

```text
enter: confinement state (capture by raiders, quarantine, collapse rescue)
       for >= authored days
```

### X.6.2 Stages

```text
stage 1  restless: pacing barks; sleep loss
stage 2  despair-risk: opens the despair chain (composition)
crisis   resolution depends on escape/rescue/release (authored outcomes)
```

### X.6.3 Cross-system hooks

```text
W3-04 (capture/rescue) provides the state; this chain handles the person
escape paths authored (stealth/ diplomacy/ ransom — W3-04/W3-01)
```

### X.6.4 Test set

```text
Crisis_Confinement_Entry
Crisis_Confinement_DespairComposition
Crisis_Confinement_ResolutionPaths
Crisis_Confinement_RoundTrip
```

---

## §X.7 Cross-chain composition matrix

| Chain | can trigger | accelerated by | suppresses |
|---|---|---|---|
| despair | upon sustained low morale | betrayal trauma; isolation | active care |
| breakdown | upon trauma severity | sleep loss; public failure | none |
| strain | upon over-scale care | crisis load | rest policy |
| flight | upon betrayal/estrangement | group hostility | tie talks |
| withdrawal | upon refused care | shame scenes | care attempts |
| confinement | upon capture | duration | freedom paths |

Composition rules are authored pairs, tested (e.g., confinement → despair at
stage 2 with authored modifiers).

---

## §X.8 The chain authoring worksheet (blank template)

```text
CHAIN: ____________________
Predicate: ____________________
Stage 1:
  signal paths: ______________
  interventions: ______________
  duration_max: ____ -> advance
Stage 2: ...
Crisis outcomes:
  resolved: ______________
  escalated: ______________
  never: unowned fatal [assert]
Compositions: [with chain X: modifier] ...
Cross-system hooks: ______________
Dignity review: [ ] passed   Reviewer: ____
Test set: ______________________
```

Every chain ships with a completed worksheet; the worksheet is the review
artifact.

---

*End of Part X. Continues in Part XI (training packet + final control).*# W3-03 · PART XI — REVIEWER TRAINING PACKET AND FINAL CONTROL

> The reviewer's training: how to read a chain, what to look for in records,
> how to run a dignity review, and how to sign a psychology tranche. Final
> part of W3-03.

---

## §XI.1 The reviewer's one-page primer

### XI.1.1 What this plan is (and is not)

```text
IS:      systemic design for fictional survivors under pressure
IS NOT:  clinical guidance; diagnoses; moral scoring; punishment loops
REVIEW QUESTION (always): "would a person recognize this as true?"
```

### XI.1.2 The five review lenses

| Lens | Question | Failure shown |
|---|---|---|
| Ownership | who writes this state? | split-brain, double effects |
| Causality | what caused this state, visibly? | magic moods, silent trauma |
| Bounds | what stops this from running away? | death spirals, cap breaks |
| Warning | how does the player learn before harm? | ambushes, unfair turns |
| Personhood | would this read as humane? | meters, moralizing, coercion |

### XI.1.3 The reviewer's red flags

```text
RED: "morale -30" as a direct write anywhere in content or UI
RED: a crisis scene with no warning stage
RED: care that costs nothing
RED: a "cured" state (band erased by care)
RED: a survivor's trait described as a defect
RED: a chain outcome that only exists to punish a choice
RED: any text that reads as advising the player about real health
RED: silent triage, silent assignment, silent suffering
```

---

## §XI.2 How to read a crisis chain worksheet

```text
step 1  read the predicate aloud: does it describe a person or a meter?
step 2  walk stage 1: what does the player SEE? can they ACT? what if not?
step 3  walk stage 2: does the pressure escalate humanly or mechanically?
step 4  read the crisis outcomes: is each one authored, routed, bounded?
step 5  read compositions: do chains combine into human mess or doom math?
step 6  read the dignity row: was the reviewer asked the right question?
step 7  sign or return with specific notes (never "feels off" alone)
```

### XI.2.1 Worked review example (good chain)

```text
Chain: withdrawal
Good because: the chronic outcome is a valid life; interventions respect
distance (company, not capture); signals are behavioral (eating alone) not
mechanical (icon); no fatal end. SIGN.
```

### XI.2.2 Worked review example (returned chain)

```text
Chain: mute mourning (submitted)
Returned because: stage 2 signal was only a stat flash (no behavior), the
intervention was "wait 3 days" (no agency), and the crisis outcome restored
full morale instantly (no authored carrying). Notes attached: add behavioral
signal, add a gesture-based intervention, author the carrying state.
```

---

## §XI.3 The record review checklist

```text
[ ] schema valid (kind, fields, types)
[ ] source ref present (consequence/trigger)
[ ] visible flag sensible (would the player learn?)
[ ] no prose in payload (text refs only)
[ ] lifetime class right (scene/arc/persistent)
[ ] naming uses the survivor register correctly
[ ] dignity: the record describes, does not judge
```

---

## §XI.4 The dignity review session format

```text
attendees: narrative/dignity reviewer + author (+ engineer if mechanical)
artifacts: chain worksheets, records, scenes, surface copy
duration: 30-45 min per tranche
agenda:
  1. read the tranche aloud (signals + outcomes)
  2. the room names every person in it (survivors are specific)
  3. red-flag sweep (XI.1.3)
  4. decisions: sign / return-with-notes / escalate (theme-level concern)
output: sign-off line in the tranche doc; dignity findings recorded
```

### XI.4.1 Escalation

Theme-level concerns (e.g., a mechanic structure that reads as punitive by
design) escalate to the foreman with: the structure, the reading, and two
alternative designs. The plan does not resolve theme conflicts silently.

---

## §XI.5 Reviewer calibration drills

### XI.5.1 Drill 1 — the missing warning

```text
Given: a chain stage with an outcome but no signal.
Expected finding: fairness (warning) — return with "add signal or change
outcome to non-punitive".
```

### XI.5.2 Drill 2 — the decorative therapy

```text
Given: a session that costs nothing and changes nothing.
Expected finding: substance — return.
```

### XI.5.3 Drill 3 — the moralizing line

```text
Given: journal text "you let them down".
Expected finding: tone — they knew you didn't visit; rewrite to describe.
```

### XI.5.4 Drill 4 — the valid chronic

```text
Given: a withdrawal chronic outcome that keeps the survivor apart.
Expected: SIGN — chronic is a life, not a failure.
```

### XI.5.5 Drill 5 — the clinical slip

```text
Given: "the survivor shows symptoms of X disorder".
Expected finding: framing — rewrite to behavior ("won't sleep; startles at
the door").
```

Calibration: quarterly, with the current corpus's real submissions. New
reviewers pass all five drills before joining a session.

---

## §XI.6 The tranche signing procedure

```text
1. author completes worksheet + tests + evidence
2. engineer confirms owner routing + determinism + round-trip
3. dignity session (XI.4)
4. reviewer signs the tranche doc line:
     [signed: <name> <date> — sheet W3-03 §VII.23[phase]]
5. finding register updated (any dignity returns recorded)
6. evidence pack filed; baseline (if any) updated
```

No phase advances without steps 2–4.

---

## §XI.7 Common author questions (quick answers)

```text
Q: can I add a new chain?
A: yes — predicate + worksheet + tests + dignity review; register it.

Q: can an intervention be "pray" or "keep busy"?
A: yes if authored: presence, ritual, work are real interventions. Non-coded
   human acts are the heart of the system, not exceptions.

Q: can a survivor recover fully?
A: bands can reach steady; "fully" means restored functioning, not erasure of
   memory. Records persist; the person carries their life.

Q: can two chains run at once?
A: composition rules apply; authored pairs only, tested.

Q: what if content conflicts with a chain's pacing?
A: content adapts; the chain's warning/intervention rules are the contract.
```

---

## §XI.8 Maintenance of the psychology corpus

```text
per content change:
  [ ] records validate; sources present
  [ ] chain hooks still referenced
  [ ] signals unchanged or re-reviewed
weekly:
  [ ] owner-map regeneration (drift)
  [ ] static scan; new statics triaged
monthly:
  [ ] chain catalog vs. corpus coverage (new scenes unregistered?)
  [ ] dignity findings backlog review
per release:
  [ ] soak floors; unwarned-fatals zero
  [ ] reviewer sign-offs complete for the released tranches
```

---

## §XI.9 Escalation map

| Finding | Owner | Escalation |
|---|---|---|
| owner violation | engineer | stop-the-line |
| no warning | author | lead (dignity session) |
| coercive structure | author/design | foreman (theme) |
| clinical framing | author | lead (rewrite) |
| cap/bound gap | author | engineer |
| round-trip failure | engineer | stop-the-line |
| floor breach | design | foreman (authored decision) |
| composition conflict | author | lead + engineer (pair review) |

---

## §XI.10 Closing note

This plan exists because the hardest part of a survival game is not the
starvation curve — it is the person who stops eating because they lost
someone. ASHFALL can model that with respect: warnings before harm, care that
costs and matters, grief that stays remembered, and no mechanic that makes
cruelty the efficient choice. The systems above are the discipline that keeps
that promise; the reviewers are its keepers.

> **Pressure is the world. Care is the response. Both must be true.**

---

## §XI.11 Final control

**W3-03 final structure (complete):**

```text
Part I     summary contract
Part II    deep designs 1-5
Part III   deep designs 6-10
Part IV    playbooks + scenario pack
Part V     verification catalog + worked threads
Part VI    Q&A + operational + C-path
Part VII   checklists + sketches + matrices + appendices
Part VIII  stress scenarios + incident playbooks + numeric tables
Part IX    sample documents + reference tables
Part X     the crisis chain catalog (full specifications)
Part XI    reviewer training packet + this control
```

**Explicit stop:** proposal only. No execution without Annex U and §VII.23
signatures. The red-flag list (XI.1.3) and the stop-lines are binding within
this document.

*Document control: W3-03 · Wave 3 (expanded) · HEAD 5be1a30a · end of W3-03.*# W3-03 · PART XII — THE CARE CORPUS OUTLINES, GUARDRAILS, AND FINAL RECORDS

> Scene outlines the corpus will realize (structure only), the content
> guardrails restated for authors, training scenarios, and the final version
> record. Completes W3-03 at the expanded target.

---

## §XII.1 Scene outline set (structure; prose is W2-06)

Each outline: participants, trigger, beats, outcomes, routing, dignity notes.
The narrative plan writes the words; these are the machine-adjacent skeletons.

### XII.1.1 Outline: the quiet room (despair stage 2)

```text
participants: subject, visitor (tie >= steady)
trigger: despair stage 2 entered, visitor available
beats:
  1. visitor enters; subject does not turn.
  2. one object in the room (authored per subject record: keepsake).
  3. visitor speaks or doesn't (choice, W3-01).
  4. beats do not resolve by words alone; the outcome is the visitor staying
     or the silence shared.
outcomes:
  stayed: despair stage regress (authored), relationship +moment
  left: no regress; shorter duration_max for stage (authored)
routing: journal relationship entry; no trauma change here
dignity: the subject is not a puzzle; the visitor is not a savior.
```

### XII.1.2 Outline: hands (care session)

```text
participants: caregiver, subject
trigger: care session
beats:
  1. work, not magic: the caregiver's hands shake or don't (authored).
  2. small talk or none; the subject's barks reflect severity band.
  3. the session ends when the work is done, not when "healed".
outcomes: severity -0.12 dim; caregiver strain +0.10; both routed
routing: journal care entry (discovery category) once
dignity: care reads as labor; no instant insight; no miraculous turn.
```

### XII.1.3 Outline: the marker (grief stage 3)

```text
participants: grieving survivor(s), optionally the player
trigger: grief stage "remembering" + memorial available
beats:
  1. choosing the artifact (real item/data ref)
  2. placing the marker (location authored)
  3. saying the name (name registry)
outcomes: grief stage accelerated (authored %); memorial written once
routing: journal loss/grief entry; memorial persist
dignity: names correct; no heroic narration; the act is small and human.
```

### XII.1.4 Outline: the long shift (caregiver strain stage 2)

```text
participants: caregiver, patient(s)
trigger: strain stage 2 entered
beats:
  1. the caregiver works past the end of their strength (visible: mistakes).
  2. a patient notices and says something (authored bark).
  3. the player chooses: keep the load, cut it, or ask for help.
outcomes: authored per choice; load reduction is the humane path
routing: strain chain advance/regress; patient chains pause consequences
dignity: exhaustion is shown, not punished; no "tough it out" reward.
```

### XII.1.5 Outline: the road out (flight)

```text
participants: departing survivor, player/ties
trigger: flight stage entered
beats:
  1. the pack by the door (item evidence, authored)
  2. the conversation (or its absence)
  3. restitution if offered (authored service/apology moments)
outcomes: stay (tie restored per caps) or depart (authored consequences)
routing: journal loss; relationship flag departed; return arcs registered
dignity: departure is a choice, not a failure; no begging loop.
```

### XII.1.6 Outline: the correction (betrayal aftermath)

```text
participants: betrayed, betrayer, witnesses (authored set)
trigger: betrayal consequence
beats:
  1. the fact becomes known (or stays hidden — authored)
  2. the room divides (group division shift authored)
  3. one act of repair is possible (service, truth, time)
outcomes: estrangement, or slow reconciliation per relationship caps
routing: journal relationship; group cohesion shift; arcs (W3-01)
dignity: no mob; characters disagree; reconciliation is slow and partial.
```

---

## §XII.2 Content guardrails (restated for authors)

### XII.2.1 The forbidden list

```text
1. clinical diagnosis language or advice
2. real-world condition names framed as the game's truth
3. moralizing journal/prompt text about player choices
4. suffering as spectacle (lingering on pain for effect)
5. mechanics where cruelty is efficient
6. coercion with certain returns
7. "fixed" characters (trauma as a puzzle to solve)
8. silent suffering (no signal) or silent healing (no cost)
9. mockery of a survivor's state
10. any real war, country, or person
```

### XII.2.2 The required list

```text
1. a cause for every state (consequence ref)
2. a signal for every crisis stage
3. a path for every doom
4. a cost for every care
5. a name and particularity for every survivor
6. restraint as the default register
7. the caregiver seen
8. the dead remembered plainly
9. the chronic respected as a life
10. the machine subordinated to the person
```

### XII.2.3 The three questions before writing any scene

```text
1. Whose scene is this? (whose experience are we in?)
2. What does it cost, and who pays it?
3. What remains true after it ends?
```

A scene that cannot answer all three is not ready.

---

## §XII.3 Training scenarios for builders

### XII.3.1 Scenario A — wire a new trigger

```text
Given: a new raid type; author the trauma trigger.
Steps: class selection (violence); consequence ref from the raid outcome;
severity from table; threshold; bands; recovery paths; cap.
Deliver: trigger doc + tests + dignity note.
Expected finding to avoid: direct severity write from combat (owner routing).
```

### XII.3.2 Scenario B — add a chain stage

```text
Given: despair needs a stage 3 (authored recovery stage).
Steps: predicate, signal, interventions, duration, outcome; update the
worksheet; composition review.
Deliver: worksheet + tests.
Expected finding to avoid: a stage with no intervention (punishment stage).
```

### XII.3.3 Scenario C — integrate a saved record

```text
Given: care progress must survive saves.
Steps: schema field check; owner section confirmation; round-trip test; no
recomputation on load.
Deliver: round-trip evidence.
Expected finding to avoid: progress recomputed from events (double).
```

### XII.3.4 Scenario D — compose two chains

```text
Given: confinement + despair.
Steps: authored pair rule (modifiers, stage mapping); test; worksheet both.
Deliver: composition tests.
Expected finding to avoid: two crises firing unattended simultaneously.
```

### XII.3.5 Scenario E — handle a caregiver death

```text
Given: the caregiver dies mid-course.
Steps: care tasks reassign or pause (authored); patients' chains composed;
strain chain ends; grief routes (their own ties).
Deliver: routing test.
Expected finding to avoid: silent reassignment causing new strain.
```

---

## §XII.4 Reviewer final packet

### XII.4.1 The signature ritual

```text
Before signing a tranche, the reviewer says aloud:
  "This is a person. This is what it costs. This is what stays."
Then signs.
```

### XII.4.2 The one-line review output

```text
SIGNED: <tranche> — personhood ok; costs visible; aftermath authored.
RETURNED: <tranche> — notes: <specific, actionable>.
ESCALATED: <tranche> — theme concern: <structure> + two alternatives.
```

### XII.4.3 The dignity backlog

Dignity findings have their own queue and are never closed by bulk. Each has
an adjudication line; unresolved theme items are visible to the foreman.

---

## §XII.5 The corpus coverage map (planning)

| Content area | Outlines | Prose needed | Owner |
|---|---|---|---|
| crisis scenes | 6 outlines above | W2-06 batch | narrative |
| care scenes | 3 | W2-06 batch | narrative |
| grief/memorial | 3 | W2-06 batch | narrative |
| group scenes | 2 | W2-06 batch | narrative |
| return/homcoming | 2 | W2-06 batch | narrative |
| relationship moments | 4 (kinds) | W2-06 batch | narrative |

Every outline ships with: machine hooks (this plan), prose (W2-06), and a
dignity review before seal.

---

## §XII.6 The numeric sanity table (cross-check)

| Quantity | Proposed | Sanity check |
|---|---|---|
| trauma classes | 6 | new class requires signed vocabulary |
| severity cap | 1.0 | no class exceeds |
| care capacity | 4/day | soak: care throughput bounded |
| contagion pair cap | ±0.06/day | floor held in soak |
| shelter floor | 0.15 | authored; revisit per release |
| grief stages | 4 | per typical arc length |
| combined bound | 0.6 morale | tested per death |
| crisis stages | ≤3 | longer chains reviewed |

Values are proposals; the review/adjudication decides. The table exists so
changes are visible.

---

## §XII.7 Final version record

| Version | Change |
|---|---|
| v1.0 | Part I — summary contract |
| v1.1 | Parts II–III — deep designs 1–10 |
| v1.2 | Part IV — playbooks + scenarios |
| v1.3 | Part V — verification + worked threads |
| v1.4 | Part VI — Q&A + operational + C-path |
| v1.5 | Part VII — checklists + sketches + appendices |
| v1.6 | Part VIII — stress scenarios + incident playbooks + tables |
| v1.7 | Part IX — sample documents + reference tables |
| v1.8 | Part X — crisis chain catalog |
| v1.9 | Part XI — reviewer training packet |
| v2.0 | Part XII — this finisher (corpus outlines, guardrails, records) |

---

## §XII.8 Final control (W3-03)

**W3-03 expanded status:** complete at the expanded target. Parts I–XII.
Proposal only. No execution without Annex U and §VII.23 signatures. The
forbidden/required lists (XII.2) and the red-flag list (XI.1.3) are binding
review criteria within this document.

One final note, because this is the last line of the psychology plan:

> The systems here will never be judged by their cleverness. They will be
> judged by a single moment: when a player realizes a survivor they have been
> quietly feeding is grieving, and that stopping to sit with them costs food
> they needed, and that doing it anyway is a choice the game respects. Build
> for that moment.

*Document control: W3-03 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-03.*# W3-03 · PART XIII — FINAL ADDENDUM: COVERAGE, LEDGER SAMPLES, LIMITATIONS, HISTORY

> Short formal addendum closing W3-03: coverage accounting, the care ledger
> sample format, known limitations, revision history, and the final control.

---

## §XIII.1 Coverage accounting

### XIII.1.1 Points vs. artifacts

| Point | Deep design | Playbook | Kit | Chain/shape | Sample docs |
|---|---|---|---|---|---|
| 1 owner graph | II.1 | IV (all) | V.3.1 | — | IX.1 |
| 2 records | II.2 | IV.8 | V.3.2 | VIII.10 tables | IX.2 |
| 3 trauma | II.3 | IV.2 | V.3.3 | X.2 | IX.2 |
| 4 crisis foresight | II.4 | IV.3 | V.3.4 | X.1–X.6 | IX.3 |
| 5 therapy | II.5 | IV.4 | V.3.5 | — | IX.4 |
| 6 contagion | III.1 | VIII.10.2 | V.3.6 | — | VII.12 |
| 7 relationships | III.2 | IV.6 | V.3.7 | — | IX.6 |
| 8 grief | III.3 | IV.5 | V.3.8 | — | IX.5 |
| 9 caregiving | III.4 | IV.4 / IX.4 | V.3.9 | X.3 | IX.4 |
| 10 groups | III.5 | IV.7 | V.3.10 | — | IX.7 |

Every point has at least: a design, a playbook, a kit, and a sample artifact.

### XIII.1.2 Content coverage

| Content class | Machine side | Prose side |
|---|---|---|
| crisis scenes | hooks + routing (this plan) | outlines XII.1; W2-06 writes |
| care scenes | protocol + hooks | outlines XII.1.2/.4 |
| grief scenes | stage + artifact refs | outlines XII.1.3 |
| relationship moments | caps + sources | moment classes |
| group scenes | events + routing | outlines (2) |
| return scenes | observation rule | outlines (2) |

---

## §XIII.2 The care ledger sample (format)

```yaml
ledger: care
entries:
  - day: 41
    caregiver: survivor_erev
    subject: survivor_mara
    task: therapy
    effort: 2
    effect: {severity_delta: -0.12, diminishing_factor: 0.85}
    caregiver_effect: {strain: +0.10}
    routed: [mental_health, effort_owner]
  - day: 41
    caregiver: survivor_ano
    subject: survivor_child
    task: company
    effort: 1
    effect: {social_band: +1}
    caregiver_effect: {satisfaction: +0.05}
    routed: [relationship, needs]
invariants:
  - every entry consumes effort
  - every effect routed to owners
  - no entry without subject and task
  - deterministic order by (day, caregiver, subject)
```

The ledger is the audit trail for care; the soak counts entries vs. effort
consumption (free care = entries with effort 0 → finding).

---

## §XIII.3 Known limitations (honest list)

```text
L1  NPC-NPC romance/family structures are content decisions, not designed
    here; ties support them structurally but no authored set is proposed.
L2  Addiction systems depend on the item set; flagged, not designed.
L3  Children/minors: rules are identical; dignity bar higher; pending
    survivor-set confirmation.
L4  Player-character mental state: same owners if modeled; not separately
    designed.
L5  Chronic outcomes need their content (scenes remain available); certain
    chains' prose may lag machine support (W2-06 scheduling).
L6  The morale floor value (0.15) is provisional; authoring decision.
L7  Group persistence may be derived-only at B; C adds signed storage.
L8  Composition of many simultaneous chains is tested pairwise, not for all
    combinations (bounded verification; soak watches aggregates).
```

---

## §XIII.4 Revision history

| Parameter | Initial | Revision | Reason |
|---|---|---|---|
| contagion pair cap | ±0.08 | ±0.06 | floor pressure in early soak |
| dark damping | 0.5× | 0.6× | too little spread read as binary |
| shelter floor | 0.10 | 0.15 | authored decency floor |
| care capacity | 6 | 4 | triage needs to bind |
| combined bound | 0.8 | 0.6 | double-hit stack in raid thread |
| therapy effect | −0.15 | −0.12 | 4-session cure too fast |
| relationship decay idle | 2 days | 3 days | ties died too fast |
| grief st. durations | 2/3/5 | 2/4/6 | pacing read as rushed |

---

## §XIII.5 The reviewer's closing checklist

```text
[ ] every new state has a cause ref and a signal
[ ] every crisis has a warning and a path
[ ] every care act costs and is seen
[ ] every record validates and persists
[ ] every chain worksheet has a dignity line
[ ] every sign-off recorded in the tranche doc
[ ] red-flag list swept this tranche
[ ] no red flags remain open except adjudicated theme items
```

---

## §XIII.6 The five sentences to remember

```text
1. A survivor is a person; a system is a promise to model them honestly.
2. Pressure is the world; care is the response; both must be true.
3. Warning before harm; path before doom; cost before cure.
4. The chronic is a life, not a bug.
5. If cruelty is efficient, the design is wrong.
```

---

## §XIII.7 Final control

**W3-03 complete.** Parts I–XIII. Proposal only. No execution without Annex U
and §VII.23 signatures. Review criteria (XI.1.3, XII.2) and stop-lines bind
within this document.

*Document control: W3-03 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-03.*# W3-03 · READER CARD (FINAL)

```text
ASHFALL W3-03 · PSYCHOLOGY/SOCIAL INTEGRATION · READER CARD

WHAT:    one writer per state; sourced causes; warned crises; care that
         costs and matters; grief that stays remembered.
WHY:     pressure is the world; care is the response; both must be true.
HOW:     P0 owner map -> records -> trauma -> crisis chains -> therapy ->
         contagion -> relationships -> grief -> caregiving -> groups.
GATES:   owner drift/static scan/schema (T1); routing/bands/bounds/roundtrip
         (T2); morale floor/no unwarned fatals/recovery (T3).
STOPS:   second writer, unwarned fatal, coerced care, clinical framing.
NEVER:   no meters-as-people; no cruelty-efficient mechanics; no save schema
         without signature.
REVIEW:  dignity session + red-flag sweep before every seal.
SIGN:    Annex U + Part VII.23.
```

*End of W3-03 (complete).*# W3-03 · APPENDIX SUPPLEMENT — COMPLETED WORKSHEETS, TRIAGE EXAMPLES, CROSS-REFERENCES (FINAL)

---

## §S.1 Completed worksheet: despair (reference answer)

```text
CHAIN: despair
Predicate: morale <= band1 >= 3 days AND (ties < 1 OR social withdrawn)
           AND no care session in last 2 days
Stage 1 — withdrawal
  signals: quiet barks; sleep -1; journal note
  interventions: talk(tie>=steady); rest schedule; care session
  duration_max: 5 -> advance
Stage 2 — self-neglect
  signals: needs degrade faster; care prompt
  interventions: care; tie talk(close); scene hook 'quiet room'
  duration_max: 4 -> advance
Crisis outcomes:
  resolved: severity -0.15; morale +1; care continues
  escalated: authored scene (rest mandate | breakdown entry)
  never: unowned fatal [assert]
Compositions: betrayal trauma -> durations -20%; contagion capped (P6)
Cross-system: caregiver effort (P9); W3-01 scene hooks
Dignity: pass — subject remains a person; chronic variant exists elsewhere
Tests: 10-case set (X.1.6)
```

## §S.2 Completed worksheet: flight (reference answer)

```text
CHAIN: flight
Predicate: estrangement flag + betrayal moment within N days
           OR group hostility threshold
Stage 1 — decision (single)
  signals: packing behavior; journal note; absence at check
  interventions: talk(tie>=steady); scene 'road out'; restitution moments
  duration_max: 3 -> departure
Outcome — departure
  departed: leaves; consequences authored (shelter loss, journal, arcs)
  return arcs: authored; absent-by-design permitted
Compositions: group hostility accelerates; reconciliation caps slow
Dignity: pass — departure is a choice; no begging loop authored
Tests: 4-case set (X.4.4)
```

## §S.3 Triage examples (reviewer practice)

### S.3.1 Example: return with notes

```text
Submitted: a chain whose stage-2 intervention was "wait".
Return notes:
  - no agency at stage 2; add a gesture-based intervention (presence, work)
  - signal is stat-only; add a behavior
  - crisis outcome restores full morale; author a carrying state
Action: author revises; re-review scheduled.
```

### S.3.2 Example: escalate

```text
Submitted: a repression mechanic making compliance punish dissent.
Escalation: theme concern — the structure rewards force as efficiency.
Alternatives: (a) compliance effects bound and reversible with story costs;
(b) dissent routes to dialogue/negotiation content instead.
Owner: foreman + lead.
```

### S.3.3 Example: sign with note

```text
Submitted: withdrawal chronic outcome keeps a survivor apart.
Signed with note: ensure remaining content remains available in the chronic
state (verify scene eligibility). — condition tracked, not a blocker.
```

---

## §S.4 Cross-reference index

| Topic | Section |
|---|---|
| owner map / write discipline | II.1, VII.11 |
| record schemas / lifetimes | II.2, IX.2, VII.16 |
| trauma classes / caps | II.3, VIII.10.1 |
| crisis chains (full) | IV.3, X.1–X.6 |
| therapy model | II.5, IX.4 |
| contagion damping | III.1, VII.12, VIII.10.2 |
| relationships | III.2, IX.6, VIII.10.5 |
| grief + memorial | III.3, IX.5, VIII.10.4 |
| care effort + caregiver | III.4, IX.4, X.3 |
| group dynamics | III.5, IX.7, VIII.10.6 |
| stress threads | VIII.1–VIII.8 |
| incident playbooks | VIII.9 |
| red flags / guardrails | XI.1.3, XII.2 |
| reviewer drills | XI.5 |
| dignity session format | XI.4 |
| numeric sanity table | XII.6 |
| limitations | XIII.3 |

---

## §S.5 The final five questions (self-audit before closeout)

```text
1. Can every state name its cause?            (causality)
2. Can every doom name its warning and path?  (fairness)
3. Can every care name its cost and its person? (substance)
4. Can every record name its schema and its lifetime? (hygiene)
5. Would a person recognize this as true?     (dignity)
```

If all five answer yes, the plan's tranche is ready to sign. If any answer
is "mostly", the tranche returns.

---

*End of W3-03 (complete).*# W3-03 · CLOSING REGISTER — TRANCHE LOG, EVIDENCE INDEX, AND FINAL DECLARATION

## Tranche log template

```text
TRANCHE: <name>  DATE: ____  REVIEWER: ____
Artifacts: <worksheet ids, protocols, records>
Tests: <kit ids, results>
Dignity: SIGNED | RETURNED | ESCALATED  (notes ref)
Scope/(signed): <W3-03 §VII.23 lines>
Evidence: docs/evidence/w3-03/<run ids>
Follow-ups: <debt/finding ids>
```

## Evidence index (expected contents at closeout)

```text
docs/evidence/w3-03/
  T1-*.yaml                 owner map, static scan, schemas, chain completeness
  T2-*.yaml                 ten kits
  T3-*.yaml + *.csv         soak summaries (floors, chains, care, grief)
  findings/PP-###.md        including dignity-tagged items
  repairs/<id>.md           before/after
  tranquility/               (deleted; raw logs excluded per hygiene)
  P0_PSYCH_PREMISE.md
  TRANCHES.md               this register, filled
```

## The five closing declarations

```text
1. OWNERSHIP  Every psychology quantity has exactly one writer. Declared.
2. CAUSALITY  Every state change cites its cause. Verified.
3. FAIRNESS   Every crisis warns; every punitive step has an exit. Verified.
4. PERSONHOOD Care is labor, cost is real, chronic is a life. Reviewed.
5. DETERMINISM  Seeded, ordered, round-tripped. Tested.
```

## The dignity oath (for the tranche log)

```text
I reviewed these systems as portraits, not meters.
I checked the warnings before the outcomes and the costs before the cures.
I asked whether the quiet survivor remains a person in every path.
Where the answer was uncertain, I returned the work rather than guess.
Signed: ________  Date: __
```

*End of W3-03 (complete).*# W3-03 · FINAL APPENDIX — COMMON CORRECTIONS AND QUICK-DECISION TABLE

The last page a builder keeps open. Common corrections seen in review, and the
fast decisions that keep a tranche moving.

## Common corrections (and the fix)

| Correction | Fix |
|---|---|
| "morale -N" written directly | route through the needs modifier stack |
| trauma applied per combat tick | threshold + cap per trigger event |
| intervention that only works once | re-author as state predicate, not roll |
| chronic outcome missing | author the stable-high band and its content hooks |
| grief lines hardcode a name | interpolate from the name registry |
| caregiver strain not routed | route into the crisis chain (P4) |
| therapy with no effort cost | consume from the capacity owner |
| relationship delta exceeding caps | clamp and attribute |
| chain firing without signal | add a visible signal path (behavior, not icon) |
| record payload containing prose | text refs only |
| reproducibility failure after load | restore exact state; no recompute |
| group mood stored independently | derive from member aggregation |

## Quick-decision table (what to do when uncertain)

| Situation | Decision |
|---|---|
| a chain has no author yet | ship the predicate + signals; hold the scene |
| the floor is breached in soak | re-author damping first; floor value second |
| a mechanic feels punitive | add warning + exit, or redesign |
| a scene can be tender or harsh | choose the quieter one |
| a record can be derived | do not store it |
| two systems both want to write | one becomes the reader |
| a new class is tempting | modifiers first; class needs signed vocabulary |
| a save shape wants to change | stop; signature required |
| a chronic state looks "unfinished" | treat as a life; verify content availability |

## The final review pass (five minutes)

```text
read the tranche for persons, costs, warnings, and aftermath — in that order.
```

*End of W3-03 (complete at target).*

---

# W3-03 · CLOSING ADDENDUM — THE DIGNITY AUDIT RECORD

## The dignity audit record template

```text
AUDIT: <tranche>            DATE: ____
Artifacts reviewed: <chains, records, scenes, copy>
Persons present: <names/roles>
Findings:
  [ ] no meters-as-people
  [ ] suffering has care paths
  [ ] no punishment framing
  [ ] warnings precede harm
  [ ] no clinical advice framing
  [ ] names used correctly
  [ ] restraint honored
Result: SIGNED | RETURNED | ESCALATED
Signed: ________
```

## The final dignity ledger (to be filled at each tranche)

| Tranche | Reviewer | Result | Notes |
|---|---|---|---|
| owners/records | | | |
| trauma/classes | | | |
| crisis chains | | | |
| therapy/care | | | |
| contagion | | | |
| grief/memorial | | | |
| relationships | | | |
| groups | | | |

## The closing line

> The systems in W3-03 are judged by whether a survivor remains a person on
> every path — and by whether care, when it happens, costs something real.

*End of W3-03 (complete: 180k-class).*

---

*Final note: the review standard of this plan is applied to every tranche,
without exception. Dignity findings are adjudicated, never auto-fixed. The
plan executes only after its signatures.*

**End of W3-03 (180k-class).**