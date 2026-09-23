# ASHFALL — UNBLOCK PROGRAM · PLAN 5
## Newest Expansion Waves Intake and the C3/EN Authorization Gate: Expansions 12–31, HOLDs 174/175, EN-01…EN-08, XP-07/09/10

**Status:** planning deliverable only. Read-only pass. No production, data, test,
save, or governance-ledger file is modified by this document. No path is claimed.
**Date:** 2026-09-21
**Baseline verified at:** `Zcode_Branch`, HEAD `5be1a30a63cd86cf23e4034473b739ac514f0f2a`
(2026-09-20 02:03 +0300) plus the current uncommitted worktree, which contains
the untracked expansion waves `docs/expansions/wave1..wave4/`.
**Role:** unblock-plan author. This plan turns the newest design corpus and the
remaining hold/authorization boundary into an execution-ready intake.
**Authority chain:** `AGENTS.md`, `INTEGRATION_PLANS.md`,
`WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`,
`docs/governance/DECISION_REGISTER.md` (DEC-17, DEC-18),
`docs/plans/wave8_part2/C3_DECISION.md`,
`Seal-steps/ashfall-enhanced-expansion-program-en-01-en-08-and-xp-pillar-rescoping-2026-09-19.md`,
`Seal-steps/ashfall-feature-expansion-proposal-part-1-pillars-xp-01-…-xp-10.md`,
and the expansion indexes (WAVE1..WAVE4).

---

## 0. How to read this plan

| Sibling | Region | Primary releases |
|---|---|---|
| Plan 1 | Equipment/body schema | XP-06, EN-04, Expansion 16 prosthetic half |
| Plan 2 | Funds and trade | XP-04, XP-08, EN-03, C3 192/199 |
| Plan 3 | Semantic/voice/strings | D11, D22, Plan 42/46/49, EN-05 |
| Plan 4 | Ledger/register/census truth | D-series micro, D21 truth, E1C, census, EN-08 gate |
| **Plan 5 (this)** | Newest expansions, C3 holds, EN gate | Expansions 12–31 intake, HOLDs 174/175, EN-01/02/06/07, XP-07/09/10 |

This is the **intake and authorization** plan. It does not author expansion
content. It converts:

1. twenty untracked design bibles (expansions 12–31, waves 1–4) into a
   gated intake pipeline with shared decisions and per-wave admission;
2. the two remaining C3 HOLDs (174, 175) into lift or decline decisions with
   named owners;
3. the eight EN proposals into a signed authorization gate that respects each
   proposal's blocking companion (Plans 1–4 supply those);
4. the three XP pillars whose premise status is unresolved (XP-07 Plan 190,
   XP-09 vs DEC-17, XP-10 vs DEC-18) into signed scope or explicit closure.

Vocabulary: **intake** = the act of admitting a wave into the live queue with
claims and premises; **authorization** = the foreman line that converts a
proposal into a claimable package; **premise check** = the Rule 7 verification,
including checks against RETIRED register rows; **enrichment** = the cross-plan
notes that keep plan documents truthful after release.

---

## 1. Executive summary

### 1.1 The state in one paragraph

On 2026-09-20, a twenty-plan expansion corpus was authored in four waves
(12–31) covering generations, belief, aviation, agriculture, bodies, leisure,
the underground, hazards, espionage, power, water, fire, dying, rail, food,
clothing, schooling, glass, print, and masonry. Every plan is explicitly a
"design bible: not a claim, not an authorization", and the indexes name the
same open decisions: content volume budgets, save-section placement, selftest
verbs, and priority order. The corpus is untracked in git and absent from the
live ledgers, which means it is simultaneously the newest work in the repo and
invisible to agents. In parallel, two C3 HOLDs remain (174 mechanical-origin
seam; 175 cross-run profile store), the eight EN proposals remain unauthorized
(four now unblocked by Plans 1–4, four gated on them), and three XP pillars sit
at the premise boundary: XP-07 (Plan 190 item provenance, mapped but unsigned),
XP-09 (presenter skill tree — **DEC-17 RETIRED**), and XP-10 (phobia growth —
**DEC-18 RETIRED**). This plan supplies: the wave-intake protocol and shared
decisions, the two C3 lift designs, the EN authorization lines, and the
retired-adjacent premise verdicts.

### 1.2 Signature bundle in one glance

| # | Sign-off line | Releases | Blast radius |
|---|---|---|---|
| W-1 | `I approve the expansion-wave intake protocol (§5.1) and the wave admission order (§5.5).` | all four waves | ledgers/claims; no production |
| W-2 | `Content budgets: approved as authored per plan / capped at <n> words per wave; implementation ships content tranches incrementally.` | authoring scope | content only |
| W-3 | `Save placement: additive sub-objects inside existing owners (proposal), per plan; no new sections without a signed schema row.` | all waves | each consuming owner's save shape as plans execute |
| W-4 | `Selftest verbs: extend existing verbs per plan; no new verbs without a named owner (proposal).` | verification surface | CLI registry as plans execute |
| W-5 | `Wave priority: [Wave 5 admission order as proposed / amend].` | scheduling | queue order |
| C3-174 | `174: [lift — signed extension point on <owner> with a consumed surface / retire on this pass].` | mechanical origin effects | `SurvivorEnrichmentService` or `TradeSpecialtySystem` |
| C3-175 | `175: [lift — signed cross-run profile-store owner <name> / hold / retire].` | Meta Profile / NG+ | new versioned store outside campaign slots |
| EN-02 | `EN-02 The Living Map: authorized [amendments].` | EN-02 package | map panel/planning read-model |
| EN-06 | `EN-06 One Bootstrap Path: authorized [amendments].` (after Plan 2's F13-B + the bootstrap companion) | EN-06 package | lifecycle/bootstrap gates |
| EN-01 | `EN-01 Difficulty-Consequence Weave: authorized [amendments].` | EN-01 package | difficulty consumers + war routes |
| EN-07 | `EN-07 Chronicle & Aspiration Readiness: authorized [amendments].` | EN-07 package | completion-history read model |
| EN-03/04/05/08 | `authorized once <Plan 2/1/3/4> lands` | four gated ENs | per their proposals |
| XP-07 | `XP-07 Plan 190 scope: [signed as provenance subset / declined].` | item provenance | item instance state, chronicle |
| XP-09 | `XP-09: [declined — DEC-17 stands / reversal decision: presenter skills revived as <scope>].` | radio progression clarity | radio production only |
| XP-10 | `XP-10: [declined — DEC-18 stands / reversal decision: phobia growth revived as <scope>].` | psychology clarity | mental-health domain only |

### 1.3 What is NOT in this plan

- No expansion content is authored, claimed, or implemented.
- No wave is admitted before its premise check and claim (the protocol forbids
  stealth integration).
- No C3 hold is lifted from evidence alone; the lift sign-offs name owners and
  consumed surfaces.
- No EN is authorized ahead of its blocking companion (the audit's gating rule).
- No DEC-17/DEC-18 reversal is proposed by default; the plans present the
  retired-adjacent pillars as declines unless the foreman chooses a reversal.
- No new save sections are authorized by this plan; save placement is a
  per-plan rule that must be satisfied at execution time.

---

## 2. Verified current reality

### 2.1 The newest expansion corpus

`docs/expansions/wave1..wave4/` (untracked; 2.7 MB total; 20 plan documents at
50–72 KB each, plus four indexes):

| Wave | Expansions | Themes | Index |
|---|---|---|---|
| 1 | 12–16 | Second Generation, Faithful & Fractured, Above the Ash, Deep Root, Rebuilt Body | `wave1/WAVE1_INDEX.md` |
| 2 | 17–21 | Long Evening, Underneath, Bitter Air, Quiet Hand, Grid | `wave2/WAVE2_INDEX.md` |
| 3 | 22–26 | Clean Flow, Alarm, Long Goodbye, Iron Road, Common Table | `wave3/WAVE3_INDEX.md` |
| 4 | 27–31 | Thread, Lesson, Glass, Press, Kiln | `wave4/WAVE4_INDEX.md` |

Shared constraints stated by every wave: Godot-authoritative, Core engine-free,
JSON-authoritative with `schema_version`, one authority per concern,
deterministic seeded RNG only, additive persistence inside existing save
owners, restrained fictional tone, focused verification with data-integrity and
content-utilization selftests.

Open decisions named by every index: content volume budgets, save-section
placement, selftest verbs, priority order. The indexes also state the promotion
steps: pick one plan/phase, re-audit the premise, claim paths, add an
`INTEGRATION_PLANS.md` row, implement data → Core → persistence → host/UI →
content, verify, and let only the foreman/integrator update ledgers.

### 2.2 Cross-wave hooks (the corpus is designed to compose)

The indexes' hook diagrams show the waves interlock additively (children and
rites; water and fire; rail and freight; thread and press; kiln and glass).
Each plan is self-contained and can ship alone; none requires another. That
property is what makes a wave-admission order possible without a monolithic
big-bang.

### 2.3 Prior expansions 01–11 are live

`docs/expansions/EXPANSIONS_MASTER_CATALOG.md` documents expansions 01–11
(Holdfast/Ice Road, Duty Roster, Standing Record, Nobody's Charter, Year of
Ash, Muster, Dose, Verdict, Black Flotilla, Silent Foundry, Long Line) with
namespaces, host sessions, save keys, and selftest verbs. The new corpus
extends the same pattern; numbering 12–31 is collision-free per the wave
indexes' evidence.

### 2.4 C3 HOLD 174 — mechanical origin effects

`docs/plans/wave8_part2/C3_DECISION.md`: HOLD with the recheck condition "a
signed extension point on an existing owner (`SurvivorEnrichmentService` or
`TradeSpecialtySystem`) with a consumed gameplay surface; otherwise retire on
the next pass." Current evidence:

- `SurvivorEnrichmentService` (line 35) is live and consumed by
  `SurvivorDetailPanel` (per Plan 42 §2.12).
- `TradeSpecialtySystem` (line 67) exists.
- No `BackstorySystem` or `backstory_templates.json` was created (the hold
  forbids it).

The lift therefore requires the foreman to name the seam and the consumed
surface; nothing in current source decides it.

### 2.5 C3 HOLD 175 — cross-run profile store

`C3_DECISION.md`: HOLD with the condition "a signed cross-run profile-store
owner (versioned, checksummed, outside campaign slots) plus verified Plan
34/149 completion-fact producers." Current evidence:

- `CampaignCompletionHistory` (schema v2; `difficultyPresetId` stamp;
  `CampaignCompletionHistoryTests` 11/11) is the completion-fact producer, and
  DEC-20 records its boundary: user-level, append-only, no rewards/unlocks/
  profile/NG+.
- No cross-run profile store exists; the boundary is explicit that the history
  store is **not** a profile.
- Plan 34's completion-fact producers are sealed (the audit's
  `DEBT-PLAN34-DIFFICULTY-CHRONICLE-AUTHORITY` RETIRED).

The lift therefore requires a product decision naming a new store (or a
decline). The boundary work makes it cheaper than before: the completion facts
exist; only the profile consumer is missing.

### 2.6 EN gate matrix at HEAD

From the EN program §A.2, updated by Plans 1–4:

| EN | Gate | State now | Released by |
|---|---|---|---|
| EN-01 Difficulty-Consequence Weave | Plans 13/06/08 sealed | 06+08 sealed; difficulty binding done (XP-01) | this plan (authorize) |
| EN-02 The Living Map | Plan 07 P4+ sealed | SEALED prerequisites | this plan (authorize) |
| EN-03 Underground Economy Pressure | Plan 14 + XP-07 premise | blocked on F13 | Plan 2 (F13-A/D), then this plan |
| EN-04 Rehabilitation Medicine | Plans 01 + 15 sealed | blocked on F14 | Plan 1 (F14-E), then this plan |
| EN-05 Signal Continuity & Voice | Plan 02 + XP-09 premise | distress seal done; XP-09 retired-adjacent | Plan 3 (D22 freeze) + this plan's XP-09 verdict |
| EN-06 One Bootstrap Path | Plans 09–11 sealed | 09+11 sealed; 10 = CF-P28 sealed | this plan (authorize) |
| EN-07 Chronicle & Aspiration | Plan 08 + E1 | 08 sealed; E1E1C authorized by Plan 4 | this plan (authorize) |
| EN-08 Ledger Truth Program | roster reconcile items | cleared by Plan 2 F13-B + Plan 4 D21 | this plan (authorize once both land) |

The program's own recommended order (E.2): EN-02 → EN-06 → EN-01 → EN-07 →
EN-08 → EN-05 → (F13 → EN-03 → XP-07) and (F14 → EN-04).

EN invariants from the program §E.4: **no EN adds a save section or migrates a
save shape**; every behavior rides existing state through pure Core composition.
That invariant is part of what the authorizations approve.

### 2.7 XP-07 — item provenance (Plan 190)

The XP proposal describes provenance records (`origin_kind`/`day`/`actor`/
`place`), a bounded named-item set (24 proposed), reveal conditions, provenance-
aware interactions, a HISTORY read surface, and chronicle export on death.
Current evidence: the chronicle vault, archive, and item instance condition
state exist; no provenance field exists; Plan 178's closeout says "Plan 190
instance-lore stays mapped until separately signed".

The pillar's foreman decisions: sign the scope (provenance + named subset vs.
full instance-lore), the launch set size and trait envelope, and whether
`looted` flagging applies retroactively.

### 2.8 XP-09 — presenter skill tree (RETIRED-adjacent)

`DEC-17` is `RETIRED`: "Radio station production operates through equipment and
program production, not individual character RPG skill trees." The XP proposal
includes XP-09 (5-skill domain, 4 archetypes, fatigue coupling). Under the EN
program §A.4, XP-09 is "retired-adjacent — any revival is a reversal decision,
foreman-only." The premise check therefore must not treat the pillar as merely
blocked; it must present the reversal explicitly.

### 2.9 XP-10 — phobia growth (RETIRED-adjacent)

`DEC-18` is `RETIRED`: "Superseded by psychological trauma, guilt insomnia, and
morale contagion systems in Core." The XP proposal includes XP-10 (8-entry
closed phobia vocabulary, 4 earned traits, therapeutic protocol). Same rule:
revival is a reversal decision.

### 2.10 The C3 HOLDs' relationship to the expansions

The wave corpus interacts with the holds:

- Expansion 12 (Second Generation, succession dossiers) may eventually want a
  cross-run record (175 territory) for lineage legacies — but the index states
  the plan can ship alone.
- Expansion 20 (Quiet Hand, informants) may want relationship-driven origins
  (174 territory) — again optional.
- Neither expansion requires a hold lift; the intake notes must state that so
  builders do not assume 175/199-style owners exist.

### 2.11 The blocking rule the program states and this plan enforces

EN program §B.1: "no builder claims an EN package; no EN work rides inside
another package (no stealth integration)." §B.4: scope amendments narrow, never
widen; a false premise stops at the gate. This plan's authorization lines and
claim templates enforce both.

---

## 3. Blocked-plan inventory released by this plan

### 3.1 The twenty expansion plans (the newest blocked corpus)

- **Block:** not admitted to any live ledger; no claims; no authorization.
- **Released by:** W-1..W-5 (protocol, budgets, save placement, selftest
  verbs, admission order). Each wave then enters the standard promotion path
  one plan/phase at a time.

### 3.2 C3 HOLD 174

- **Block:** unsigned extension point + consumed surface.
- **Released by:** C3-174 lift (or retirement on this pass, which is also a
  release from limbo).

### 3.3 C3 HOLD 175

- **Block:** unsigned cross-run profile-store owner.
- **Released by:** C3-175 lift (or hold/retire with a condition).

### 3.4 EN-01, EN-02, EN-06, EN-07

- **Block:** no authorization line.
- **Released by:** the four authorization lines; all prerequisites are already
  sealed or land via Plans 2/4.

### 3.5 EN-03, EN-04, EN-05, EN-08

- **Block:** authorization must follow their companions.
- **Released by:** Plan 2 (F13) → EN-03; Plan 1 (F14-E) → EN-04; Plan 3 (D22)
  → EN-05; Plan 2 F13-B + Plan 4 D21 → EN-08. This plan signs the lines
  conditionally ("authorized once <companion> lands") so the queue can schedule
  them.

### 3.6 XP-07

- **Block:** Plan 190 unsigned.
- **Released by:** XP-07 scope signature or decline.

### 3.7 XP-09 / XP-10

- **Block:** premise conflict with RETIRED rows; "blocked" is the wrong frame.
- **Released by:** explicit decline (DEC-17/DEC-18 stand) or a reversing scope
  decision. Either outcome removes the limbo.

### 3.8 Secondary releases

| Item | Gain |
|---|---|
| Plan 1 | Expansion 16's prosthetic half gets its prerequisite name |
| Plan 2 | Expansions 17/25/26 get boundary notes (no currency leak) |
| Plan 3 | Expansions 28/30 get freeze-key notes |
| Plan 4 | The wave corpus gains census/register visibility |
| `EXPANSIONS_MASTER_CATALOG.md` | gains a wave 12–31 section or a pointer |
| `docs/expansions/` git status | waves become tracked deliverables |
| EN program | authorization record completed per §B.3 format |

### 3.9 Non-releases

- No content budget is spent by this plan.
- No save section is created.
- No hold is lifted without a named owner/surface.
- No old DEC-17/DEC-18 concept is revived by default.

---

## 4. Decision packet

### 4.1 W-1 — intake protocol

**Sign-off line:**

> `I approve the expansion-wave intake protocol (§5.1) and the wave admission order (§5.5).`

**Meaning:** the protocol governs how any expansion from 12–31 enters the live
queue. It forbids stealth integration, requires a premise check, requires a
claim, and requires the standard promotion path. The admission order sequences
waves and plans.

### 4.2 W-2 — content budgets

**Sign-off line:**

```text
Content budgets: approved as authored per plan / capped at <n> words per wave; implementation ships content tranches incrementally.
```

**Context:** the indexes estimate roughly 250k–380k words per wave if fully
authored. The decision is whether to approve those envelopes or cap them. The
recommended shape is "cap per wave, tranche incrementally": each plan's Phase 1
(schema/validators) is safe and additive; content follows in reviewed tranches.

### 4.3 W-3 — save placement

**Sign-off line:**

> `Save placement: additive sub-objects inside existing owners (proposal), per plan; no new sections without a signed schema row.`

**Rationale:** every wave index says persistence is additive inside existing
owners. This line makes that binding, so a builder cannot invent a sibling
section mid-execution. If a plan genuinely needs a new section, it returns for
a schema row (the same discipline as Plans 1–2).

### 4.4 W-4 — selftest verbs

**Sign-off line:**

> `Selftest verbs: extend existing verbs per plan; no new verbs without a named owner (proposal).`

**Rationale:** the CLI verb surface is a shared resource; each plan recommends
extending the owning domain's verb. This keeps the registry stable and the
help contract testable.

### 4.5 W-5 — admission order

**Sign-off line:**

```text
Wave priority: [Wave 5 admission order as proposed / amend].
```

**Proposed order (§5.5 rationale):**

1. Expansion 12 (Second Generation) — content-light, socially central,
   extends live cohort/generational systems.
2. Expansion 13 (Faithful & Fractured) — content-light, ritual/social, extends
   live spiritual/zealotry systems.
3. Expansion 22 (Clean Flow) and 26 (Common Table) — water and food are the
   tightest survival loops.
4. Expansion 23 (Alarm) — hardens everything.
5. Expansion 15 (Deep Root) — second largest content build.
6. Expansions 27 (Thread) and 31 (Kiln) — winter clothing and permanence.
7. Expansion 29 (Glass) and 30 (Press) — measurement and print compound school
   and archive.
8. Expansion 14 (Above the Ash), 16 (Rebuilt Body), 17 (Long Evening),
   18 (Underneath), 19 (Bitter Air), 20 (Quiet Hand), 21 (Grid), 24 (Long
   Goodbye), 25 (Iron Road), 28 (Lesson) — later waves, largest builds or
   most sensitive tone.

The order is a scheduling preference, not a dependency chain: each plan ships
alone.

### 4.6 C3-174 — mechanical origin effects

**Sign-off line:**

```text
174: [lift — signed extension point on <owner> with a consumed surface / retire on this pass].
```

**Options:**

| Option | Owner | Consumed surface | Notes |
|---|---|---|---|
| A. Lift on `SurvivorEnrichmentService` | the live read model | a mechanical modifier consumed by a gameplay system (e.g., a starting skill or a trade-specialty bonus) | lowest risk; the service already resolves identity |
| B. Lift on `TradeSpecialtySystem` | trade specialty | specialty effects consumed in trade | narrower; fits economic backstories |
| C. Retire | — | — | honest closure; the flavor already exists via enrichment/echoes |

**Recommendation:** A if the foreman wants mechanical origins; otherwise C.
The decision must name the consumed gameplay surface, or the lift repeats the
duplication the hold was created to prevent.

### 4.7 C3-175 — cross-run profile store

**Sign-off line:**

```text
175: [lift — signed cross-run profile-store owner <name> / hold / retire].
```

**If lifted, the normative constraints (from the C3 condition + DEC-20):**

1. Versioned, checksummed store outside campaign slots (a user-level file, not
   a campaign envelope section).
2. It may read completion-history facts; it must not alter them.
3. It owns no ending calculation, campaign state, difficulty semantics,
   rewards, unlocks, prestige, or NG+ gameplay effects unless separately
   signed.
4. Deterministic; migration neutral for old installs.

**Recommendation:** lift only if a concrete player-facing use is named
(e.g., a run-history ledger screen). Otherwise keep HOLD with the condition
now cheaper (facts exist), or retire.

### 4.8 EN authorizations

**Format (program §B.3):** `EN-<nn> <name>: authorized [scope amendments, or "none"].`
The line is pasted verbatim into the authorization record.

**Ready to authorize now (prerequisites sealed):**

| Line | Notes |
|---|---|
| `EN-02 The Living Map: authorized.` | prerequisites sealed; recommended first |
| `EN-06 One Bootstrap Path: authorized.` | bootstrap companion sealed; recommend after Plan 2 F13-B ledger truth |
| `EN-01 Difficulty-Consequence Weave: authorized.` | XP-01 binding done; war routes sealed |
| `EN-07 Chronicle & Aspiration Readiness: authorized.` | completion-history v2 live; E1C authorized by Plan 4 |

**Conditionally authorized (companion-gated):**

| Line | Gate |
|---|---|
| `EN-03 Underground Economy Pressure: authorized once F13 lands.` | Plan 2 |
| `EN-04 Rehabilitation Medicine: authorized once F14-E lands.` | Plan 1 |
| `EN-05 Signal Continuity & Voice: authorized once D22 freeze and XP-09 verdict land.` | Plan 3 + §4.9 |
| `EN-08 Ledger Truth Program: authorized once F13-B and D21 truth land.` | Plan 2 + Plan 4 |

**EN invariants the signature approves:** no new save section; no save-shape
migration; pure Core composition over existing persisted state; per-proposal
P0 premise re-verification.

### 4.9 XP premise verdicts

**XP-07 sign-off:**

```text
XP-07 Plan 190 scope: [signed as provenance subset / declined].
```

**XP-09 sign-off:**

```text
XP-09: [declined — DEC-17 stands / reversal decision: presenter skills revived as <scope>].
```

**XP-10 sign-off:**

```text
XP-10: [declined — DEC-18 stands / reversal decision: phobia growth revived as <scope>].
```

**Recommendation:** decline XP-09/XP-10 unless the foreman actively wants the
reversals; the retired systems (program production, trauma/guilt/morale) already
serve the design goals. Declines remove the pillars from limbo and close the
premise-check work.

### 4.10 Signing order and first safe step

1. Sign W-1..W-5 (intake and shared decisions).
2. Sign C3-174/175 (owners or closures).
3. Sign EN-02/EN-06/EN-01/EN-07; record the conditional EN lines.
4. Sign XP-07 scope and the XP-09/XP-10 verdicts.
5. Plan 5's closeout updates the ledgers; wave intake proceeds plan-by-plan.

**First safe step:** the wave premise checks and the C3/EN/XP audits are
read-only and may run before any signature.---

## 5. Technical design (execution-ready after signature)

### 5.1 Expansion-wave intake protocol (binding once W-1 signs)

**Step 1 — Wave bookkeeping (integrator).** The wave directories are untracked;
before any claim, the integrator adds the wave indexes and admitted plans to
git as docs (tracked deliverables) so the corpus has provenance. No plan body
is edited during bookkeeping.

**Step 2 — Plan selection (foreman).** Pick exactly one plan and one phase.
Never start two plans at once (every index says this; the foreman rule repeats
it).

**Step 3 — Premise check (builder, read-only).** Before the first edit:

1. Re-verify the plan's "System (live)" claims by file:line — the system still
   exists and still owns the described concern.
2. Re-verify the "Current content" claims by catalog counts.
3. Confirm no newer owner superseded the seam (Rule 7: a plan is not proof).
4. Confirm the plan's non-duplication statement against current authorities.
5. Confirm the plan's save-placement claim satisfies W-3.
6. Confirm no active claim holds the paths.
7. Record all of it as a P0 premise note (the standard pattern:
   `*_PREMISE_EVIDENCE.md` or the package log).

**Step 4 — Claim (integrator).** Add the claim row with exact paths per
`WORKTREE_OWNERSHIP.md`; confirm disjointness.

**Step 5 — Ledger row (integrator).** Add the package row to
`INTEGRATION_PLANS.md` with owner, acceptance, and focused verification. The
current batch's rules allow a new package only when the batch permits; the
intake uses the standard mechanism.

**Step 6 — Implement in the fixed order:** data first, then pure Core, then
persistence, then host/UI, then content. Every index states this order; it
matches the repo's proven phased pattern.

**Step 7 — Verify** with focused tests + `--data-integrity-selftest` +
`--content-utilization-selftest` (+ `--panel-bind-lifecycle-selftest` and a11y
for UI phases).

**Step 8 — Handoff** per `AI_AGENT_WORKFLOW.md`; only the foreman/integrator
updates the live ledgers.

**Forbidden:** stealth integration (EN program §B.1 applied to expansions); a
phase without its premise note; a new save section without a signed row; a new
selftest verb without W-4; widening a phase's scope without a foreman
amendment.

### 5.2 Shared decisions detail

#### 5.2.1 Content budgets (W-2)

Each index lists an authoring estimate. If fully authored, the four waves
total roughly 1.1–1.4 million words of prose. The recommended shape:

| Setting | Value |
|---|---|
| Phase 1 (schemas/validators) | unrestricted (safe, additive) |
| Phase 2 (Core) | unrestricted (mechanical) |
| Content phases | capped per wave (e.g., 40k words per wave per quarter), reviewed per tranche |
| Tone review | `ashfall-write`/narrative-continuity review on every prose tranche |

**Why cap:** the repo's testing budget cannot absorb four waves of content at
once, and content is the easiest thing to overproduce and under-integrate
(the "dead data" failure mode the content-utilization gate exists to catch).

#### 5.2.2 Save placement (W-3)

Normative rule:

1. State rides an existing owner as an additive sub-object (nullable), exactly
   as the earlier flagships did (`BlackMarketState.factionBounties`,
   `MedicalPipelineSaveState.record`).
2. If a plan's data is genuinely a new domain with its own lifecycle, it must
   propose a new section with: owner key, filename, capture/restore, migration
   neutral, and count updates — and get a schema row signed. The wave indexes
   recommend additive; the intake enforces "additive unless a row is signed".
3. No plan may read/write another owner's section directly; it routes through
   the owner's API or read model (the EN program's pure-composition invariant).

#### 5.2.3 Selftest verbs (W-4)

Normative rule:

1. Extend the owning domain's existing verb (e.g., a new plan in the shelter
   domain extends the shelter/facility selftest).
2. A new verb requires: a name, an owner, a manifest row, a help-contract
   update, and a reason a new verb is better than extending.
3. The `SELFTEST_MANIFEST.json` and CLI catalog generators run `--check` in the
   same package.

### 5.3 Per-wave plan analysis (all twenty plans)

The table below is the intake dossier. "Live seam" and "gap" come from the wave
indexes; "unblock cross-ref" maps each plan to the sibling unblock plans whose
decisions touch it. All are re-verified at their own P0.

#### Wave 1 — expansions 12–16

| Plan | Live seam | Gap | Unblock cross-ref |
|---|---|---|---|
| 12 Second Generation | `CohortSystem`, `ChildDevelopmentSystem`, `GenerationalSystem`, `ApprenticeshipSystem` | no curriculum, milestones, kinship, rites, succession content | none required; optional 175 for lineage legacy |
| 13 Faithful & Fractured | `SpiritualMeaningCoordinator`, `ZealotrySystem`, `IdeologicalFrictionSystem` | no pilgrimage, relics, shrines, schism, holy calendar | none; symmetric-belief contract |
| 14 Above the Ash | `AviationSystem`, `SkyDefenseBatterySystem`, `SkyLayerArmorSystem`, `OrbitalHarrowTelemetrySystem` | no sky trade, airdrop contest, survey, seasons, warning network | optional Plan 2 route model if sky trade settles |
| 15 Deep Root | `AgricultureSystem`, `GreenhouseSystem`, `ApicultureSystem`, `CompanionAnimalSystem` | no open ground, orchard, livestock, vet, aquaculture, seed bank | none; photosynthesis/growth stays with Greenhouse |
| 16 Rebuilt Body | `AmputationSystem`, `BionicsSystem`, `RoboticsSystem` | no simple prosthetics, rehab content, complications, automation, drones, rogue arcs | **Plan 1 required for prosthetic/rehab half**; robotics half separate |

#### Wave 2 — expansions 17–21

| Plan | Live seam | Gap | Unblock cross-ref |
|---|---|---|---|
| 17 Long Evening | `SurvivorDowntimeSystem`, `VinylMoraleSystem`, `CultureCreationSystem`, `CulturalArchiveVaultSystem` | no performance, festival, sport, game, gallery, broadcast culture | Plan 2 boundary note (no currency leak) |
| 18 Underneath | `SubterraneanSystem`, `TunnelNetworkSystem`, `ExcavationSystem`, `SeismicDynamicsSystem`, `HydroGeology` | no ore/mining, subsidence, aquifers, gas fields, cave ecology, deep heritage | none |
| 19 Bitter Air | `ChemWarfareSystem`, `DecontaminationSystem`, `PathogenStrainSystem`, `DiseaseQuarantineCoordinator` | no plumes, air quality, mask logistics, quarantine zones, toxic legacy, vault | none |
| 20 Quiet Hand | `EspionageSystem`, `CounterIntelligenceSystem`, `ShelterEspionageSystem`, prisoner/bounty systems | no informants, dead-drop tradecraft, interrogation ethics, prisoner terms, defectors, hunters | optional 174 for origin-driven informants |
| 21 Grid | `PowerGridSystem`, `PowerDistributionSubgridSystem`, SOFC/solar/turbine/flywheel | no load politics, cascades, fuel chains, storage doctrine, EMP hardening, microgrids | none; generation contributions stay runtime-only |

#### Wave 3 — expansions 22–26

| Plan | Live seam | Gap | Unblock cross-ref |
|---|---|---|---|
| 22 Clean Flow | `WaterTreatmentSystem`, `BrineWaterSystem`, `SanitationSystem`, `WaterborneExposureRules`, `SumpFloodingSystem`, `ChlorAlkaliSynthesisEngine` | no source world, quality profiles, hygiene practices, waste routing, drainage, public health, water commons | none; mass balance preserved |
| 23 Alarm | `ShelterFireHazardSystem`, `CascadeCoordinator`, `CascadeRuleCatalog`, `DiseaseTriage`, rescue dispatch | no evacuation, rescue, command, drills, suppression logistics, refuges, mutual aid | none |
| 24 Long Goodbye | `FinalWishSystem`, `CaregivingSystem`, `PsychologicalSanatoriumSystem`, `MentalHealthCrisisSystem`, `PsychologicalArcSystem`, `MemorialSystem` | no aging, palliative care, therapy modalities, crisis protocols, grief stage, legacy tokens | Plan 1's phantom-pain work is adjacent but independent; XP-10 verdict relevant to psychology boundaries |
| 25 Iron Road | `RailwaySystem`, `RailwayInterlockEngine`, `RailLogisticsCatalog`, `RailGrindingEngine` | no bridges, locomotives, track maintenance, schedules, rail towns, interdiction, commons | Plan 2 boundary note (freight ≠ player routes) |
| 26 Common Table | `KitchenNutritionSystem`, `NutritionDiversitySystem`, `FoodPreservationSystem`, `ResourceRationingSystem`, `RationConflictSystem`, `DesperationSystem` | no menus, deficiencies, preservation methods, ration policies, hunger phases, food culture | Plan 2 boundary note (food ≠ currency) |

#### Wave 4 — expansions 27–31

| Plan | Live seam | Gap | Unblock cross-ref |
|---|---|---|---|
| 27 Thread | `Inventory.WornGear`, `NeedsSystem` warmth/hygiene, `ShelterAtmosphereSystem.ThermalComfort`, `craft_textile_repair` | no garment model, no laundry | Plan 1 schema stays optional (no slot multiplication) |
| 28 Lesson | `ApprenticeshipSystem`, `LibraryStudySystem`, `ArchiveDeskSystem` | 1.7 KB apprenticeship catalog | Plan 3 freeze keys (text-heavy) |
| 29 Glass | `PrecisionOpticsEngine`, `OpticsGlassworksCatalog`, `GeodeticSurveyEngine` | 1.4 KB glass recipes | Plan 3 keys if panel text |
| 30 Press | `PaperPrintingCatalog`, `ArchiveDeskSystem`, `ArchiveInkCatalogLoader`, `UndergroundPrintingPressPanel` | no print data at all | Plan 3 freeze keys (text-heavy) |
| 31 Kiln | `CeramicsKilnCatalog`, `MasonryBrickworksCatalog`, narrative kiln/lime/mudbrick/refractory datasets, `weather_hardening_upgrades.json` | narrative only, no gameplay | Plan 2 funds if any build market emerges (not required) |

### 5.4 Expansion-to-unblock map

| Unblock plan | Expansions it affects | Relationship |
|---|---|---|
| Plan 1 (F14) | 16 (required), 24 (adjacent), 27 (constraint) | 16's prosthetic/rehab half cannot start before F14-A..E |
| Plan 2 (F13) | 17, 25, 26 (boundaries), 14/21 (optional hooks) | no hard dependency; boundaries prevent duplicate economy |
| Plan 3 (D11/D22) | 28, 30 (keys), all (briefing strings) | text-heavy plans use freeze keys |
| Plan 4 (truth) | all | census/register visibility; E1C metadata |
| Plan 5 (this) | all | intake, budgets, placement, verbs, order |

### 5.5 Admission order rationale

The proposed order (§4.5) optimizes for: survival-critical loops first (water,
food), social centrality second (generations, belief), hardening third (alarm),
and largest/most sensitive builds last. It is compatible with the wave indexes'
own recommendations (Wave 1: 12/13 then 15; Wave 2: 21/19 then 18; Wave 3:
22/26 then 23; Wave 4: 27 then 31 then 29 then 30 then 28) while interleaving
across waves by player impact rather than by wave number. If the foreman
prefers wave-strict order, that is an amendment, not a redesign.

### 5.6 C3-174 design — mechanical origin effects

**Option A — extend `SurvivorEnrichmentService`:**

Current role: read-only identity view (belief, profession, keepsake, background,
stance, manifesto law). A mechanical surface could be:

1. a starting **skill** seeded from the authored profession (consumed by
   `SkillProgressionSystem`), or
2. a **trade specialty** read from the enriched identity (consumed by the
   trade/barter path), or
3. a **starting item** grant from the authored keepsake (consumed by
   inventory).

Each is a consumed gameplay surface; the lift must name exactly one (or a
short list). The mechanics must be additive and bounded (a small bonus, never
a new progression system). Tests: enrichment → mechanical effect → save/load
parity; unenriched survivors get no effect (no fabricated identity).

**Option B — extend `TradeSpecialtySystem`:**

Narrower: the mechanical origin is a specialty/affinity that changes trade
outcomes. This fits the C3 forensic note ("would duplicate
`TradeSpecialtySystem`") because it extends the existing specialty owner
instead of adding a second one.

**Option C — retire:**

The Plan 174 flavor is already covered; mechanical origins may simply not be
wanted. Retiring closes the row honestly and removes it from the next pass's
audit budget.

**Lift conditions (must all hold):** owner named; consumed surface named;
bounded effect; deterministic; no second progression store; no save section
(rides existing identity state).

### 5.7 C3-175 design — cross-run profile store

**If lifted, the minimal honest shape:**

```csharp
// Core, engine-free. A user-level store, NOT a campaign section.
public sealed class CrossRunProfileStore
{
    public int schemaVersion { get; } = 1;
    // Append-only run records derived from CampaignCompletionHistory facts:
    // runIndex, endingId, difficultyPresetId, day count, chronicle flags.
    // No rewards, unlocks, prestige, or NG+ gameplay effects unless separately signed.
    public ProfileResult Record(CampaignCompletionHistory history);
    public IReadOnlyList<ProfileRunRow> Runs { get; }
    // Versioned + checksummed; migration neutral for old installs.
}
```

**Boundaries (from DEC-20 + the C3 condition):**

1. Outside campaign slots (a `user://` file, not a campaign envelope section).
2. Reads completion-history facts; never alters them; no ending calculation.
3. No gameplay effects in the base lift. A future "NG+" consumes the profile
   only with a separate signed decision naming the effects.
4. Deterministic; no RNG; checksummed; corruption-tolerant (append-only rows).
5. The player-facing surface, if any, is a run-history screen; no rewards UI.

**If held:** the condition is now cheaper (facts exist); add a recheck trigger
("when a player-facing run-history/aspiration screen is proposed").
**If retired:** close the Meta Profile/NG+ concept explicitly.

### 5.8 EN authorization packages

Each EN is already fully specified in its program document. This plan's
authorization lines convert them to claims; the design summaries below are for
the record and for scope discipline.

#### EN-02 — The Living Map

- Gate: sealed (Plan 32 graph travel + fog + caravan hops).
- Deliverable: a map screen that plans a route honestly — candidate path,
  per-edge conditions, uncertainty bands, closures, war-front warnings, cost
  estimates.
- Note: XP-02's `GraphTravelPlanner` must **not** be rebuilt; the canonical
  `WastelandMapSystem.PlanRoute` is the owner (the program's standing premise
  correction).
- Phases: P0 premise → P1 read model → P2 panel/route battery → P3 estimator
  parity gate → P4 replay.
- Tests: `WastelandMapTests` baseline; `--player-panels-uitest`; estimator
  parity.

#### EN-06 — One Bootstrap Path

- Gate: sealed (CF-P28 bootstrap; port contract; forbidden-path gate).
- Deliverable: the standing invariant that every lifecycle path runs the same
  bootstrap with zero deferred seams.
- Phases: P0 premise → P1 composite lifecycle gate → P2 ratchet gate → P3
  failure proof → P4 docs.
- Tests: port-contract `--check`; forbidden-path gate; bootstrap call-site
  proof.

#### EN-01 — Difficulty-Consequence Weave

- Gate: XP-01 difficulty binding done; war clock + consequence routing sealed.
- Deliverable: the world reacts to difficulty (war stage severity, crisis
  deadlines, shock/rumor weight) through the canonical owners.
- Consumers: the four consumer sites the program names; no new difficulty
  authority.
- Phases: P0 premise (four consumer greps) → P1..P4 consumer binds → P5
  monotonicity soak.
- Tests: monotonicity soak (twice, identical fingerprints).

#### EN-07 — Chronicle & Aspiration Readiness

- Gate: completion-history v2 + E1C (authorized by Plan 4).
- Deliverable: the campaign LEDGER strip — prior runs by difficulty and
  ending, completed arcs; evidence-governed aspiration.
- Boundaries: reads the v2 store; no rewards/unlocks; DEC-20 respected.
- Tests: `CampaignCompletionHistoryTests`; metrics drift trigger.

#### EN-03 / EN-04 / EN-05 / EN-08 (conditional)

- EN-03: after Plan 2's F13-A/D; pressure tiers over heat/trust/settlement;
  one `economy_pressure_tiers.json`; pure read model.
- EN-04: after Plan 1's F14-E; one ward screen scheduling every recovery
  track; no second recovery ledger.
- EN-05: after Plan 3's D22 freeze and the XP-09 verdict; `RescuedArcProjection`
  read model; journal exactly-once; replay extension.
- EN-08: after Plan 2 F13-B + Plan 4 D21; rerank protocol gates; census
  freshness.

**Concurrency rule (program §E.7):** at most three concurrent EN packages with
disjoint ownership; EN packages never edit shared roots in parallel with their
companion program's builders.

### 5.9 XP-07 design — item provenance scope

**Proposed signed scope (if not declined):**

1. `ProvenanceRecord` as an additive optional on item instances:
   `origin_kind` (closed vocabulary), `origin_day`, `origin_actor_id`,
   `origin_place_id`, `named_item_id?`.
2. A bounded named-item set (24 proposed) with lore fragments using the
   existing chronicle/archive patterns, each mechanically distinct but never
   strictly superior.
3. Reveal conditions consumed by existing owners (appraisal skill, condition
   threshold, place visit via the sealed map, chronicle milestone, pairing).
4. A HISTORY read surface in the item detail panel via the existing
   inspection model.
5. Provenance export on death/inheritance through the canonical legacy flow
   (Plan 206).
6. No new inventory-instance scope beyond the existing instance fields; no
   second chronicle.

**Decline consequence:** Plan 190 stays mapped/unsigned; the provenance debt
remains but the pillar leaves the active queue.

### 5.10 XP-09 / XP-10 premise analysis

#### XP-09 — presenter skill tree

- DEC-17 RETIRED the concept: radio production is equipment/program-based, not
  character RPG skills.
- The XP proposal's skill domain, archetypes, and fatigue coupling would
  reintroduce progression.
- Current live systems that already serve the intent: `RadioProgramProductionSystem`,
  `FactionRadioTypes`, presenter equipment gates, program slots.
- **Verdict options:** decline (recommended) or a reversal decision with a
  narrow scope (e.g., presenter "voice/quality" as equipment/program tags, not
  a skill tree). A reversal must explicitly cite DEC-17 and its rationale.

#### XP-10 — phobia growth

- DEC-18 RETIRED the concept, superseded by trauma, guilt insomnia, and morale
  contagion.
- The XP proposal's closed phobia vocabulary and earned traits would add a
  parallel psychology layer.
- Current live systems: `SleepNarrativeProjection`, `MoraleContagionSystem`,
  `PsychologicalArcSystem`, `MentalHealthCrisisSystem`.
- **Verdict options:** decline (recommended) or a narrowly scoped reversal
  (e.g., a closed vocabulary of *fears* feeding the existing crisis pipeline,
  with no separate growth or treatment system). A reversal must cite DEC-18.

Both verdicts are recorded in the register; declines retire the pillars from
the active queue and remove their open questions from future audits.

### 5.11 Wave tracking and ledger design

1. The four wave indexes are committed as tracked docs with their status lines
   ("design plans, pre-integration").
2. A single intake ledger section (in `INTEGRATION_PLANS.md` or a companion
   doc) lists every plan with: number, title, status (design/admitted/claimed/
   executed), first phase admitted, and the unblock cross-refs.
3. `EXPANSIONS_MASTER_CATALOG.md` either gains a wave 12–31 section or a
   pointer to the intake ledger (recommended: pointer, to avoid duplicate
   prose).
4. The census does not need to absorb the expansion plans (they are not corpus
   plans), but the intake ledger is where their status lives.

### 5.12 Open questions (execution-time, per plan at its P0)

1. Does each plan's live seam still exist with the same name/API?
2. Does the plan's "current content" count still hold?
3. Does the plan's save-placement claim satisfy W-3?
4. Is there any newer owner that superseded the seam (Rule 7)?
5. Does the plan's non-duplication statement survive the recent integrations
   (Plans 132/138/168/200/203/205/206/207/212/220 et al.)?
6. Does the plan's proposed selftest extension fit the owning verb?
7. Does the plan's content budget fit W-2's cap?
8. Does the plan interact with any C3/EN decision in this bundle?

### 5.13 Enrichment texts (paste-ready)

**`AGENTS.md`** — add a short pointer: the five unblock plans, the wave intake
status, and the rule that expansions enter only through the protocol.

**Wave indexes** — add one line to each: "Intake governed by UNBLOCK-05; wave
admitted <date> under order §5.5."

**Expansion 16** — prerequisite block referencing Plan 1's F14 (already
specified in Plan 1 §14.3).

**Expansions 17/25/26** — boundary notes referencing Plan 2 §14.4.

**Expansions 28/30** — freeze-key notes referencing Plan 3's D22.

**`EXPANSIONS_MASTER_CATALOG.md`** — a wave 12–31 pointer.

**EN program** — an authorization-record section listing the signed lines
verbatim (program §B.3 format).

**`C3_DECISION.md`** — a dated addendum for 174/175 outcomes (lift/retire/hold
with owners); the historical table remains.

**Register** — new rows for EN authorizations (or one bundle row), the C3
outcomes, and the XP-07/09/10 verdicts.

### 5.14 What a builder may NOT infer from this document

- An admitted wave is not an authorized plan: each plan still needs its own
  claim and P0 note.
- A shared decision (budgets/placement/verbs) is a constraint, not a scope
  authorization.
- The admission order is not a promise of the next claim; it is a preference.
- The C3/EN/XP lines authorize exactly their named packages, nothing broader.---

## 6. Phased execution program

### Phase group A — Wave bookkeeping and intake (1–2 days)

**A0 — track the corpus.** Commit the four wave directories and indexes as
tracked docs; no content edits. Confirm no .gitignore rule blocks them and no
large binaries ride along.

**A1 — intake ledger.** Create the wave-status section/companion doc with the
20-plan table (number, title, status, first-phase, cross-refs).

**A2 — shared decisions recorded.** W-2/W-3/W-4/W-5 lines into the register and
the indexes' status lines.

**A3 — admission of the first plan.** Per the order, Expansion 12 (or the
foreman's amendment) gets its P0 premise note, claim, and ledger row. Only one
plan starts.

### Phase group B — C3 decisions (0.5–1 day)

**B0 — 174 owner audit.** Confirm the chosen owner's API and the consumed
surface; if retiring, prepare the register text.

**B1 — 175 store audit.** Confirm the completion-history boundary and design
the store only if lifting.

**B2 — register + C3 addendum.** Record outcomes; keep the historical table.

### Phase group C — EN authorizations (0.5–1 day)

**C0 — authorization lines.** Sign EN-02/EN-06/EN-01/EN-07; record the four
conditional lines. Paste verbatim per §B.3.

**C1 — claim templates.** For each authorized EN, prepare the claim row from
the program's template (§E.5) but do not claim until the builder starts.

**C2 — concurrency check.** Confirm at most three concurrent EN packages with
disjoint ownership before any start.

### Phase group D — XP verdicts (0.5 day)

**D0 — XP-07 scope.** Sign the provenance subset or decline.

**D1 — XP-09/XP-10 verdicts.** Decline (recommended) or narrow reversals with
DEC-17/DEC-18 citations.

**D2 — register + XP doc annotations.**

### Phase group E — First-wave execution support (as admitted)

This group is not executed by this plan; it names the support this plan
provides: P0 premise templates, the shared-decision constraints, the
cross-refs, and the intake ledger. The first admitted plan's builder follows
the standard phased pattern.

### Phase group F — Closeout (0.5 day)

Ledger/register/anchor updates; enrichment texts; handoff.

**Total estimated effort:** 2.5–4 days for the decisions and intake; the wave
execution itself is a multi-wave programme.

---

## 7. Verification plan

### 7.1 Verification matrix

| Deliverable | Command/evidence | Expected |
|---|---|---|
| Intake protocol | wave indexes tracked; intake ledger present | yes |
| Shared decisions | register lines + index status lines | present |
| First plan P0 | premise note with file:line | present |
| C3 174 | owner API read + consumed-surface grep | evidence |
| C3 175 | completion-history boundary read + store design | evidence |
| EN lines | authorization record verbatim | present |
| EN claim templates | program §E.5 shape | present |
| XP-07 | scope/decline recorded | present |
| XP-09/10 | verdicts recorded with DEC citations | present |
| Cross-refs | expansion notes applied per §5.13 | present |
| Tone | new prose passes narrative review when authored | phase-gated |

### 7.2 Commands

```bash
# wave corpus presence/size
find docs/expansions/wave1 docs/expansions/wave2 docs/expansions/wave3 \
     docs/expansions/wave4 -name "*.md" | wc -l
du -sh docs/expansions/wave*

# live seam premise samples (repeat per plan at its P0)
grep -rn "class CohortSystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class ChildDevelopmentSystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class ZealotrySystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class AviationSystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class AgricultureSystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class AmputationSystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class SurvivorDowntimeSystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class SubterraneanSystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class ChemWarfareSystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class EspionageSystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class PowerGridSystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class WaterTreatmentSystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class ShelterFireHazardSystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class FinalWishSystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class RailwaySystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class KitchenNutritionSystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class ApprenticeshipSystem" Assets/Ashfall.Core --include=*.cs
grep -rn "class PrecisionOpticsEngine" Assets/Ashfall.Core --include=*.cs
grep -rn "class PaperPrintingCatalog" Assets/Ashfall.Core --include=*.cs
grep -rn "class CeramicsKilnCatalog" Assets/Ashfall.Core --include=*.cs
grep -rn "class WornGear " Assets/Ashfall.Core --include=*.cs

# C3
grep -n "HOLD" -A 3 docs/plans/wave8_part2/C3_DECISION.md
grep -rn "class SurvivorEnrichmentService\|class TradeSpecialtySystem" \
  Assets/Ashfall.Core --include=*.cs

# EN gates
python3 scripts/ci/generate-port-contract.py --check
bash scripts/run_test.sh Ashfall.Core.Tests/Endgame/CampaignCompletionHistoryTests.cs

# register/retired rows
grep -n "DEC-17\|DEC-18\|DEC-20" docs/governance/DECISION_REGISTER.md
```

### 7.3 Failure-proof obligations

- A wave admitted without a P0 premise note is rejected by the protocol.
- A C3 lift without a consumed-surface grep is rejected.
- An EN authorized ahead of its companion is rejected.
- An XP reversal without a DEC citation is rejected.
- An expansion note that claims a prerequisite boom/benefit (e.g., "Plan 2
  required") without the cross-ref table is corrected.

### 7.4 What is not accepted as evidence

- The wave index's own status string as proof of a live seam.
- A plan's non-duplication statement without a current-authority check.
- A content budget "approved" without a tranche cap.
- A save-placement claim that contradicts W-3.

---

## 8. Risks and mitigations

| # | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| 1 | Four waves flood the queue and starve verification | high | high | W-5 order + one-plan-at-a-time + content caps |
| 2 | A wave plan duplicates a recently integrated system | medium | high | P0 non-duplication check against the 2026-09/20 claims |
| 3 | Stealth integration (an expansion phase inside another package) | medium | high | protocol forbids; claims are path-disjoint |
| 4 | New save sections proliferate | medium | high | W-3 additive rule + schema-row gate |
| 5 | Selftest-verb sprawl | medium | medium | W-4 extend rule + manifest `--check` |
| 6 | Content overproduced and unreachable | high | medium | content-utilization gate per phase; caps |
| 7 | Tone drift in sensitive expansions (24, 28, 19, 20) | medium | high | narrative review per prose tranche |
| 8 | C3 174 lift duplicates a progression store | medium | high | owner + consumed-surface requirement |
| 9 | C3 175 becomes a rewards/NG+ backdoor | medium | high | DEC-20 boundary in the lift design |
| 10 | EN authorized ahead of its companion | medium | high | conditional lines + concurrency check |
| 11 | EN packages compete with companion builders on shared roots | medium | medium | program §E.7 rule |
| 12 | XP-09/XP-10 revival silently reactivates retired concepts | low | high | explicit reversal decision required |
| 13 | Untracked wave docs are lost/ignored | medium | medium | Phase A0 tracking |
| 14 | The intake ledger duplicates prose and drifts | medium | low | pointer discipline (no prose copies) |
| 15 | Premise checks burn sweep capacity | medium | medium | repeatable grep templates; one note per P0 |

---

## 9. Ownership, sequencing, coordination

### 9.1 Proposed claims

| Phase group | Claim | Owner | Paths |
|---|---|---|---|
| A | `UNBLOCK-05-A-WAVE-INTAKE` | integrator | wave docs tracking, intake ledger, register/index status |
| B | `UNBLOCK-05-B-C3-DECISIONS` | integrator | C3 addendum, register |
| C | `UNBLOCK-05-C-EN-AUTH` | integrator | EN authorization record, register, claim templates |
| D | `UNBLOCK-05-D-XP-VERDICTS` | integrator | register, XP doc annotations |
| E | per-plan claims | builders | per plan after admission |
| F | `UNBLOCK-05-F-CLOSEOUT` | integrator | ledgers, enrichment |

### 9.2 Race rules

- Wave docs: integrator adds them once; builders do not edit plan bodies.
- The first admitted plan's claim must be path-disjoint from active claims;
  check immediately before claiming.
- EN packages respect the three-concurrent rule and never share roots with
  their companion program's builders.
- C3/XP/EN register rows are integrator-only.

### 9.3 Cross-plan coordination

| Sibling | Interface | Rule |
|---|---|---|
| Plan 1 | Expansion 16 prerequisite | Plan 5's intake note references F14-A..E |
| Plan 2 | 17/25/26 boundaries | intake notes reference F13-A/D |
| Plan 3 | 28/30 keys | intake notes reference D22 |
| Plan 4 | wave visibility | intake ledger + E1C metadata |
| Active batch | package rows | expansions enter only with integrator ledger rows |

---

## 10. Rollback and decline paths

| Deliverable | Rollback |
|---|---|
| Wave tracking | docs can be untracked/reverted; no production impact |
| Intake ledger | revert the section |
| C3 lifts | register addendum revert; no code until a package starts |
| EN authorizations | an authorization without a started claim can be withdrawn by a new line; started claims follow their own rollback |
| XP verdicts | a decline can be reversed only by a new explicit reversal decision |

Declines:

- Decline W-5 order: the foreman's amendment replaces it.
- Decline a wave: the corpus stays design-only; no claims.
- Decline 174/175: rows retire or hold with conditions; no limbo by default.
- Decline an EN: the proposal stays queued with its gate; no stealth work.

---

## 11. Definition of done and handoff

### 11.1 DoD per deliverable

| Deliverable | Done when |
|---|---|
| Intake protocol | approved; indexes tracked; intake ledger present |
| Shared decisions | recorded; enforced in the intake ledger |
| First plan admission | P0 note + claim + ledger row |
| C3 174/175 | outcome recorded with owner/condition |
| EN lines | authorization record with verbatim lines + claim templates |
| XP-07/09/10 | verdicts recorded |
| Cross-refs | notes applied |
| Closeout | ledgers/register updated; handoff per workflow |

### 11.2 Handoff fields

Outcome, files, contract, evidence, shared paths untouched, proposed ledger
edits, next safe step.

### 11.3 First safe step

> Phase A0 (tracking) and the read-only premise greps may start immediately;
> no plan is admitted or claimed before W-1 signs.

---

## 12. Worked scenarios

### 12.1 The first expansion is admitted

1. Foreman signs W-1..W-5 and admits Expansion 12 per the order.
2. Builder runs the P0 premise check: cohort/generational/apprenticeship
   systems exist; content counts match; save placement is additive; no claim
   collision.
3. Integrator lands the claim and ledger row.
4. Builder implements data → Core → persistence → host/UI → content, one phase
   at a time, with focused verification.
5. Ledger updates only at closeout.

### 12.2 C3 174 is lifted on the enrichment service

1. Foreman names the mechanical surface: a starting skill seeded from authored
   profession.
2. Builder verifies the service API and the skill owner; implements the bounded
   seed; tests enriched vs. unenriched parity.
3. Register addendum records the lift with the owner/surface.
4. No BackstorySystem is created.

### 12.3 EN-02 ships

1. Authorization line signed; claim created.
2. P0 re-verifies `PlanRoute`/fog semantics and the panel route battery.
3. Read model + panel + estimator parity gate + replay.
4. No new save section; no GraphTravelPlanner beside the canonical owner.

### 12.4 XP-09 is declined

1. Premise analysis confirms DEC-17 retired the skill tree.
2. Foreman declines; register records "DEC-17 stands; XP-09 closed".
3. The radio progression question leaves the active queue; EN-05's optional
   XP-09 hook is dropped from its scope.

### 12.5 A wave premise check finds a duplicate

1. A builder checks Expansion 20's informant tradecraft against
   `EspionageSystem`/`CounterIntelligenceSystem` and finds an overlap with a
   recent integration.
2. The P0 note records the overlap; the plan returns for a scope amendment
   (narrow) before any claim.
3. No stealth merge occurs.

---

## 13. Foreman briefing — anticipated questions

**Q1. Why admit all four waves at once?**
The plan admits *no* plan to execution; it admits the corpus to governance
(tracking + intake protocol + shared decisions + order) so the queue sees it.
Each plan still gets its own claim and premise check.

**Q2. Why not keep the waves untracked until wanted?**
Untracked plans are invisible to agents and at risk of loss; tracking them
costs nothing and prevents a future agent from re-authoring the same designs.

**Q3. Is the admission order a dependency chain?**
No; each plan ships alone. The order is scheduling preference for value and
test budget.

**Q4. What if another wave appears (32+)?**
The protocol applies to any expansion; the intake ledger gains rows; the
shared decisions already govern.

**Q5. Can a builder start Expansion 12 before W-1 signs?**
No; the corpus is design-only until the intake is approved. Once approved, the
P0 note + claim gate still applies.

**Q6. Why cap content if the plans are already written?**
The plans are design prose; the in-game content is authored separately. The cap
governs production, not the design corpus.

**Q7. How do the expansions interact with the four unblock plans?**
Mostly by boundary notes: 16 needs F14; 17/25/26 need economy boundaries; 28/30
need freeze keys. None hard-blocks on a hold except 16's prosthetic half.

**Q8. Is C3 174 worth lifting?**
Only if a mechanical origin surface is wanted. The flavor already exists; the
lift must name a consumed surface or it repeats the duplication the hold
prevented.

**Q9. Is C3 175 worth lifting?**
Only with a concrete player-facing use. The completion facts exist; the store
itself is easy; the boundary (no rewards/NG+ by default) is what protects the
design.

**Q10. Why recommend declining XP-09/XP-10?**
Because DEC-17/DEC-18 retired them and the live systems already serve the
intent. Declining is not "losing" the pillars; it removes limbo and cites the
existing replacements.

**Q11. What if the foreman wants a narrow reversal?**
The decision line supports it: name the scope, cite the retired row, and keep
it bounded (e.g., equipment-tag presenter quality; fear vocabulary feeding the
existing crisis pipeline without a growth system).

**Q12. Why EN-02 first?**
Its gate is fully sealed and it adds player-visible value (honest route
planning) without waiting on anything else.

**Q13. Do EN packages add save sections?**
No; the program's defining constraint is pure composition over existing state.
The authorization approves that invariant.

**Q14. What stops three EN packages from colliding?**
The program's concurrency rule (≤3, disjoint ownership) plus the claim table.

**Q15. Does this plan touch the census?**
Only indirectly; expansions are not corpus plans. Plan 4 owns census truth.

**Q16. What happens to the expansion docs' open decisions after this plan?**
Budgets/placement/verbs/order are answered by W-2..W-5; per-plan specifics are
answered at each P0.

**Q17. Can multiple waves run concurrently?**
The protocol says one plan and one phase; different builders could run
different plans if claims are disjoint, but the recommended shape is
sequential admission to protect the test budget.

**Q18. What is the smallest useful outcome here?**
Tracking + protocol + shared decisions: the corpus becomes governable, and one
plan can start. Roughly 1–2 days.

**Q19. What is the largest outcome?**
All four waves admitted and executed over the coming months, with EN/C3/XP
boundaries resolved and the expansions composed additively.

**Q20. What is the single most important rule?**
No stealth integration: every expansion phase enters through its own claim with
its own premise note.

---

## 14. Per-plan risk notes (tone, authority, verification)

A compact risk note per expansion so the P0 check has a starting hypothesis.
These are not verdicts; they are the questions the premise check should answer.

| Plan | Authority risk | Tone risk | Verification focus |
|---|---|---|---|
| 12 Second Generation | avoid a second progression system; children route through GenerationalSystem | no child combat, no saccharine milestones | cohort/generational suites; schools/rites reachability |
| 13 Faithful | no piety meter; symmetrical fictional beliefs only (ZealotrySystem contract) | belief violence never glorified | zealotry symmetry tests; ritual inventory routes |
| 14 Above the Ash | no second flight model; sky defense stays with its battery | no air-war spectacle | aviation/sky suites; ordnance catalogs |
| 15 Deep Root | growth stays with GreenhouseSystem; no second agriculture engine | no pastoral fantasy; real scarcity | agriculture/greenhouse/acquisition suites |
| 16 Rebuilt Body | F14 schema required; RoboticsSystem owns automation half | disability dignity; prosthetics are tools | Plan 1 tests; bionics parity |
| 17 Long Evening | no new currency; gambling harm through existing systems | no real songs/sports | recreation/culture suites |
| 18 Underneath | SubterraneanSystem owns nodes; no infinite ore; permanent consequences | no magic materials | subterranean/seismic suites |
| 19 Bitter Air | hazard/decon/strain owners only; no weapon-use reward | clinical, non-exploitative | chem/disease/quarantine suites |
| 20 Quiet Hand | prisoner/bounty owners; no torture mechanic; coercion yields unreliable intelligence | no glamorized spycraft | espionage/counter-intel suites |
| 21 Grid | PowerGridSystem sole authority; generation never persisted | no blackout death directly; explicit priority ladder | power suites; runtime-only contribution check |
| 22 Clean Flow | water never created; exposure via DiseaseSystem.TryExpose; hygiene derived | dignified sanitation prose | water/treatment/sump suites |
| 23 Alarm | CascadeCoordinator owns rules; every cascade has warning + off-ramp | no scripted death without route | fire/cascade/triage suites |
| 24 Long Goodbye | care/therapy/crisis owners only; no death rewards | restraint, dignity, no cure-for-all | mental-health/memorial suites |
| 25 Iron Road | RailwaySystem + interlock sole train movers; bridges warn before failure | towns are people, not resources | rail suites; bridge warning states |
| 26 Common Table | nutrition consequences via NeedsSystem.Modify; preservation has cost | deprivation with dignity | kitchen/nutrition/preservation suites |
| 27 Thread | warmth/hygiene via NeedsSystem; garments are items; no fashion stat system | the dead's clothes are a dignified choice | inventory/warmth/atmosphere suites |
| 28 Lesson | literacy writes through SkillProgressionSystem/study owner | adults never humiliated; exams not punitive | apprenticeship/study suites |
| 29 Glass | optics owner; vision via medical pipeline; no glass superweapons | sight is capability, not humanity | optics/survey suites |
| 30 Press | archive owner; print never manufactures consent | corrections celebrated; censorship debated | archive/printing suites |
| 31 Kiln | crafting/shelter upgrade owners; no parallel construction store | building is care, not conquest; supervised apprentices | crafting/upgrade suites |

---

## Appendix A — Premise command template per plan

Reusable P0 template (fill the plan-specific values):

```bash
# 1. Live seam
grep -rn "class <OwnerSystem>" Assets/Ashfall.Core --include=*.cs
# 2. Current content count
ls Assets/StreamingAssets/Data/ | grep -i "<domain_catalog>"
grep -c '"id"' Assets/StreamingAssets/Data/<domain_catalog>
# 3. Non-duplication check
grep -rn "<proposed_new_owner_name>" Assets/Ashfall.Core src --include=*.cs
# 4. Save placement
grep -n "<existing_owner_section>" Assets/Ashfall.Core/Save/SaveSectionRegistry.cs
# 5. Selftest
grep -n "<domain>" docs/ci/SELFTEST_MANIFEST.json
# 6. Claim collision
grep -n "<path_glob>" WORKTREE_OWNERSHIP.md
```

## Appendix B — Intake ledger template

```markdown
## Expansion Waves 12–31 — Intake Ledger (<date>)

| # | Title | Wave | Status | First phase admitted | Claim | Unblock cross-refs |
|---|---|---|---|---|---|---|
| 12 | The Second Generation | 1 | DESIGN | — | — | — |
...
```

## Appendix C — EN authorization record template (program §B.3)

```markdown
## EN Authorization Record (<date>)

- `EN-02 The Living Map: authorized.` — claim: <path>; P0: <notes>
- `EN-06 One Bootstrap Path: authorized.` — ...
- `EN-01 Difficulty-Consequence Weave: authorized.` — ...
- `EN-07 Chronicle & Aspiration Readiness: authorized.` — ...
- `EN-03 Underground Economy Pressure: authorized once F13 lands.`
- `EN-04 Rehabilitation Medicine: authorized once F14-E lands.`
- `EN-05 Signal Continuity & Voice: authorized once D22 freeze and XP-09 verdict land.`
- `EN-08 Ledger Truth Program: authorized once F13-B and D21 truth land.`
```

## Appendix D — C3 addendum template

```markdown
## C3 Addendum (<date>) — HOLD outcomes

| Plan | Outcome | Owner/surface or reason | Recheck |
|---|---|---|---|
| 174 | lift/retire/hold | <owner + consumed surface> | <trigger> |
| 175 | lift/hold/retire | <store name> | <trigger> |
```

## Appendix E — Glossary

| Term | Meaning |
|---|---|
| intake | admitting a design corpus into governance with tracking and protocol |
| admission | admitting one plan to execution with a claim |
| stealth integration | an expansion phase riding inside another package (forbidden) |
| companion-gated | an EN whose authorization follows a Plan 1–4 outcome |
| reversal decision | a foreman decision reviving a RETIRED concept, citing its row |
| consumed surface | a gameplay path that actually reads and uses a new effect |

## Appendix F — Cross-plan interface table

| Interface | This plan provides | Consumed by |
|---|---|---|
| intake protocol | W-1 | all expansion executions |
| shared decisions | W-2..W-5 | all expansion phases |
| admission order | W-5 | scheduling |
| C3 outcomes | C3-174/175 | 174/175/expansion notes |
| EN authorizations | EN lines | EN packages, Plan 1–4 gate tracking |
| XP verdicts | XP-07/09/10 | radio/psychology/Plan 190 boundaries |

## Appendix G — Re-verification checklist before signature

- [ ] The four wave directories still contain 20 plan docs + 4 indexes.
- [ ] The wave indexes still state "design plans, pre-integration".
- [ ] C3 174/175 still HOLD with the same conditions.
- [ ] The EN gate matrix still matches §2.6.
- [ ] XP-07 is still unsigned; DEC-17/DEC-18 still RETIRED.
- [ ] The companion plans (1–4) are still the named gates.
- [ ] No active claim holds wave/intake paths.
- [ ] `AGENTS.md` still lacks a wave pointer (the enrichment adds it).

## Appendix H — Handoff checklist

- [ ] Waves tracked; intake ledger present.
- [ ] Shared decisions recorded and enforced.
- [ ] First plan admitted with P0 + claim (or scheduled).
- [ ] C3 outcomes recorded with owners.
- [ ] EN lines recorded verbatim; conditional lines clear.
- [ ] XP verdicts recorded.
- [ ] Cross-ref notes applied to affected expansions.
- [ ] Register rows proposed/applied.
- [ ] Handoff per `AI_AGENT_WORKFLOW.md`.

---

## Closing statement

The newest plans in the repository are also the least visible: twenty design
bibles, authored in a single day, untracked and unadmitted. This plan gives
them governance without giving away scope: an intake protocol, four shared
decisions, an admission order, two C3 outcomes, eight EN authorization lines,
and three XP verdicts. None of it authorizes content; all of it makes the
content authorizable one bounded phase at a time.

Recommended first action: approve W-1..W-5, track the corpus, and admit exactly
one plan — Expansion 12 per the proposed order — with its P0 premise note and
claim. Everything else follows the same loop.

**End of UNBLOCK-05.** This document is a proposal to govern and release the
newest plans; it does not execute, claim, or authorize any of them. The next
action belongs to the foreman.---

## 15. Cross-wave composition and shared-contract analysis

The indexes promise that each plan ships alone while composing additively.
This section turns those promises into concrete shared-contract rules so that
when two plans both want the same resource, neither invents a second owner.

### 15.1 Shared resource matrix

| Shared resource | Plans that touch it | Single owner | Rule |
|---|---|---|---|
| Water (supply, quality, drainage) | 15, 22, 23, 26 | `WaterTreatmentSystem`, `SanitationSystem`, `SumpFloodingSystem` | 22 owns source/quality; 23 uses water for firefighting through the same owner; 15 irrigates through greenhouse/agriculture; 26 never creates water |
| Food (production, preservation, rationing) | 15, 26, 12 | `KitchenNutritionSystem`, `FoodPreservationSystem`, `ResourceRationingSystem`, `NeedsSystem` | 26 owns menu/culture; 15 owns produce; 12's child rations route through the data fraction already wired |
| Power/fuel | 21, 23, 29, 31 | `PowerGridSystem` + distribution/generation/storage owners | generation contributions stay runtime-only; no plan persists a second grid |
| Fire/cascade | 23, 31 | `ShelterFireHazardSystem`, `CascadeCoordinator` | every cascade rule has an off-ramp; kiln heat routes through the fire owner |
| Rail/freight | 25, 26 | `RailwaySystem` + interlock | trains move only through the interlock; freight, not player routes |
| Clothing/fiber | 27, 30 | `Inventory.WornGear` + `craft_textile_repair` | garments are items; print supplies patterns, never a fashion stat |
| Text/print | 28, 30 | `ArchiveDeskSystem`, `LibraryStudySystem`, printing catalog | print verifies and records; it never broadcasts or censuses |
| Glass/optics | 29, 27, 31 | `PrecisionOpticsEngine`, `OpticsGlassworksCatalog` | vision consequences route through the medical pipeline; kiln supplies furnaces/pots |
| Masonry/refractory | 31, 21 | crafting + shelter upgrades, `CupolaFoundryEngine` | build results land in the live upgrade path; refractory consumption stays with the cupola |
| Belief/ritual | 13, 24 | `SpiritualMeaningCoordinator`, `MemorialSystem` | faith and mourning stay separate from a piety meter |
| Care/psychology | 24, 19, 20 | `CaregivingSystem`, therapy/crisis owners | no second trauma/phobia ledger |
| Subterranean | 18, 21, 22, 23 | `SubterraneanSystem`, `ExcavationSystem`, water/sump owners | node state stays with the subterranean owner; aquifers route to water |
| Espionage/social friction | 20, 13 | `EspionageSystem`, `IdeologicalFrictionSystem` | informants read relationships; no new knowledge simulation |
| Generations | 12, 24 | `GenerationalSystem`, `CohortSystem` | aging changes roles, not worth; legacy tokens route through Plan 206 |
| Aviation/sky | 14, 21, 29 | aviation/sky-defense owners | no second flight model; optics may supply instruments only |

### 15.2 Composition rules (binding at P0)

1. **Read-through, not copy:** a consuming plan reads the owner's API/read
   model; it never copies the owner's state into its own store.
2. **One writer per state:** the plan that owns a domain is the only writer;
   other plans request/command through it.
3. **Pure composition:** whenever possible, new behavior is a read model or a
   policy function over existing state (the EN program's invariant; the
   expansions should mirror it).
4. **Additive persistence only:** if a plan must persist, it rides the owner's
   existing section as a nullable sub-object (W-3).
5. **Determinism:** seeded RNG through `CampaignRngManager` only; no
   `System.Random`, no wall-clock, no hash-order.
6. **Reachability:** every new catalog row must be reachable by a real
   consumer or the content-utilization gate fails the package.

### 15.3 Cross-wave example — water and fire share a surface

Expansion 22 (Clean Flow) owns water quality/drainage; Expansion 23 (Alarm)
needs firefighting water. The composition:

1. 23 requests water availability from the water owner's read model.
2. 23 does not create a water reserve or a second pressure model.
3. If suppression drains a tank, the tank is decremented through the water
   owner, not by 23 directly.
4. The alarm's drill consumes time and small supplies, not the water ledger
   unless a real fire occurs.
5. Tests: a fire suppression reduces the canonical tank; the water owner
   remains the only writer.

This pattern repeats for every shared row in §15.1: identify the owner, name
the read/command path, and forbid the copy.

### 15.4 Cross-wave example — rail and the table share freight

Expansion 25 (Iron Road) moves freight; Expansion 26 (Common Table) needs
grain. The composition: 26 authors demand; 25 schedules freight through the
interlock; the delivered grain enters the canonical preservation/storage
owner. 26 never moves goods itself, and 25 never authors menus. If a player
trade route is ever wanted, it is XP-08's contract model (Plan 2), not a
second rail ledger.

### 15.5 Cross-wave example — print and school share text

Expansion 28 (Lesson) teaches literacy; Expansion 30 (Press) prints manuals.
The composition: 30 produces a manual item through the archive/printing owner;
28's study system consumes the manual as an existing `ManualDefinition`; the
freeze keys (Plan 3) cover any new UI text. Neither plan creates a second
library or archive.

### 15.6 Cross-wave example — kiln and glass share heat

Expansion 31 (Kiln) owns furnaces/lime/brick; Expansion 29 (Glass) needs
furnace heat. The composition: 29 requests furnace availability from the kiln
owner's read model; the glass batch consumes fuel/heat through the kiln's
command; neither plan creates a second thermal model. Radiation browning of
glass stays with `RadiationSystem`; vision consequences stay medical.

### 15.7 Why this matrix matters for intake

Without it, the first two admitted plans could each build "a small water
helper" and create the exact parallelism the repo forbids. The matrix is the
intake checklist's companion: every P0 note references the relevant rows.

---

## 16. Phase anatomy — what Phase 1 looks like for an expansion

Every wave plan recommends the same implementation order (data → Core →
persistence → host/UI → content). This section records the generalized phase
anatomy so a builder can size a plan quickly and so the intake ledger can
track "first phase admitted".

### 16.1 The standard five-phase shape

| Phase | Work | Exit evidence |
|---|---|---|
| P0 | premise re-verification + claim | premise note with file:line; claim row |
| P1 | data schemas/catalogs + validators | catalog loads; `--data-integrity-selftest` PASS; no consumers yet (scanner may warn — expected until P2) |
| P2 | pure Core systems/read models | focused Core tests; determinism; no save |
| P3 | persistence (additive sub-object) | round-trip; legacy neutral; Triad gate |
| P4 | host/UI integration + content | host wiring test; panel lifecycle; content-utilization PASS; a11y for UI |
| P5 | soak/replay + closeout | paired replay; ledger row; handoff |

Some plans split P4 into host and UI, or P1 into schema and content. The
intake ledger tracks the admitted phase, not the calendar.

### 16.2 Sizing heuristics

| Plan profile | Example | Expected effort |
|---|---|---|
| content-light social extension | 12, 13, 17, 28 | 6–10 builder-days to a playable slice |
| survival-loop extension | 22, 26, 15 | 8–12 |
| hazard/complex systems | 19, 21, 23 | 8–14 |
| largest builds | 18, 24, 25 | 12–18 |
| craft/industry | 27, 29, 30, 31 | 8–12 |
| schema-coupled | 16 | depends on Plan 1 completion |

These are order-of-magnitude, not commitments; the P0 note refines them.

### 16.3 The slice rule

Each plan should reach a **playable slice** before breadth: one complete loop
(a crop that grows and feeds; a garment that warms; a route that runs) rather
than many half-loops. The wave indexes' "one plan and one phase" rule is the
same discipline applied at the package level. The intake ledger should record
the slice definition when a plan is admitted.

### 16.4 Verification recipe per phase

Adapt the EN program's one-loop recipe:

```text
1. bash scripts/run_test.sh <plan's focused files>       # alone first
2. bash scripts/run_test.sh <owning regional suite>
3. dotnet build Ashfall.csproj --no-restore
4. godot --headless --path . -- <named selftests>
5. record all four results before proceeding
```

Stateful phases add round-trip + legacy parity + continuous-vs-reload tests;
soak phases run twice with fingerprint equality; panel phases add lifecycle
×100 and a11y; data phases add integrity + utilization.

### 16.5 What "done" means for a phase

A phase is done when its exit evidence is recorded, not when the code
compiles. A compile-green result is explicitly not acceptance (the rulebook
and every wave index say this).

---

## 17. Programme scheduling model

The corpus is too large to execute at once; the scheduling model keeps the
test budget honest.

### 17.1 Concurrency budget

| Resource | Limit | Rationale |
|---|---|---|
| Concurrent expansion packages | 1–2 builders | the protocol's one-plan rule for safety; two only with disjoint paths |
| Concurrent EN packages | ≤3 (program §E.7) | program rule |
| Sweep/audit work (census, premise checks) | parallel, read-only | no production contention |
| Content review | one tranche at a time | narrative/tone review capacity |

### 17.2 A quarter-1 sketch (illustrative)

| Slot | Work |
|---|---|
| 1 | Expansion 12 slice (generations) |
| 2 | Expansion 22 slice (water) after 12's P3 |
| 3 | EN-02 (living map) in parallel (disjoint) |
| 4 | C3/XP decisions and intake bookkeeping (Plan 5 groups A–D) |
| review | content tranche from 12/22 |

The sketch is illustrative; the point is that the scheduling model reserves
verification capacity instead of assuming it is free.

### 17.3 Stop conditions

Pause admission when: the standing test baseline drifts, a companion unblock
plan is mid-execution on a shared root, the content-utilization gate reports
orphans, or more than two packages share a domain. These are the same signals
the repo already uses for wave stops.

### 17.4 How expansions interact with the active batch

`INTEGRATION_PLANS.md` rules: a new batch is created only after the current
batch is ACCEPTED or BLOCKED; a batch may cite many plans but may expose no
more than three concurrent packages with disjoint ownership. Expansion intake
therefore waits for the batch rule's permission or enters through the existing
concurrent-package allowance with integrator rows. The intake ledger records
which mechanism admitted each plan.

---

## Appendix I — Content authoring and review workflow

1. **Data-first authoring:** content lives in snake_case catalogs with
   `schema_version`; no prose in C#.
2. **Key discipline:** player-facing UI/tutorial/warning/item strings use
   localisation keys (and, after Plan 3's D22, the freeze gate).
3. **Voice/briefing/text surfaces:** narrative prose goes through the
   narrative-continuity/write review; UI strings through the key discipline.
4. **Reachability:** every authored row gets a consumer before the phase
   closes.
5. **Tone:** each wave's hard contract (the indexes' ethical matrix) is
   checked per tranche; sensitive topics (children, belief, illness, death,
   interrogation, scarcity) get a second read.
6. **Volume:** tranches are bounded (W-2); unreachable or unused prose is
   rolled back rather than shipped as dead data.

## Appendix J — P0 verdict table template

```markdown
# P0 premise — Expansion <n> (<date>, HEAD <sha>)

| Claim from plan | Verified? | Evidence |
|---|---|---|
| live seam exists | yes/no | file:line |
| content count | yes/no | catalog count |
| no newer owner | yes/no | grep |
| save placement additive | yes/no | registry read |
| selftest extension fits | yes/no | manifest read |
| no claim collision | yes/no | WORKTREE_OWNERSHIP read |
| cross-refs applied | yes/no | intake ledger |

Verdict: ADMIT / AMEND (list) / DECLINE (reason)
Slice definition: <one loop>
```

## Appendix K — Recheck triggers for this plan

| Trigger | Action |
|---|---|
| a wave plan is amended | intake ledger + P0 re-run |
| a companion unblock plan changes scope | cross-ref notes re-verified |
| an EN line is withdrawn/amended | authorization record updated |
| DEC-17/DEC-18 status changes | XP-09/XP-10 verdicts revisited |
| C3 conditions change | 174/175 outcomes revisited |
| a new wave (32+) appears | protocol applies; ledger rows added |
| the batch rule changes | admission mechanism re-checked |

## Appendix L — Five-plan programme summary

| Plan | Blocked region | Key signatures | Primary releases |
|---|---|---|---|
| 1 | equipment/body schema | F14-A..G | XP-06, EN-04, Expansion 16 half |
| 2 | funds/trade | F13-A..G | XP-04, XP-08, EN-03, C3 192/199 |
| 3 | semantic/voice/strings | D11, D22, L42/L46/L49 | CF-P3, Plan 42/46/49, EN-05, DEC-11/13 |
| 4 | ledger/register/census | D3/D4/D13/D16/D19a/c/D21, L-E1C, L-CENSUS2 | EN-08, E1 continuation, residuals |
| 5 | newest expansions/holds/EN | W-1..5, C3-174/175, EN lines, XP verdicts | Expansions 12–31, 174/175, EN-01/02/06/07, XP-07/09/10 |

Together the five plans touch every nonterminal item in the queue that is not
already sealed, and they do so without racing: each owns a different region,
and their interfaces are the cross-plan tables in §5.4/§15 and the sibling
tables in each plan.

---

**End of UNBLOCK-05 appendices.**---

## 18. Kickoff briefs — the first five admissions

These briefs give the foreman and the first builders a head start: each names
the slice, the owner seams to re-verify, the cross-refs, and the first tests.
They are hypotheses for the P0 check, not authorizations.

### 18.1 Expansion 12 — The Second Generation (first admission)

**Slice:** one child grows through one development phase with one rite and one
curriculum task, observed in the cohort/system surfaces.

**Owner seams to re-verify:** `CohortSystem`, `ChildDevelopmentSystem`,
`GenerationalSystem`, `ApprenticeshipSystem`; the canonical age class
(`GenerationalSystem.GetCanonicalChildProfile` — the source plan's `isChild`
flag is the wrong authority, per Plan 42 §2.12's premise correction).

**Cross-refs:** none required; optional 175 lineage note if a dossiers feature
wants a cross-run record (it does not need one to ship).

**First tests:** developer trait progression determinism; rite reachability;
no child combat paths; cohort/generational regression.

**Tone guard:** no saccharine milestones; children are people in a hard place,
not a resource upgrade.

### 18.2 Expansion 13 — The Faithful & The Fractured (second)

**Slice:** one pilgrimage or rite chain that consumes authored supplies,
changes one social state, and records one journal chronicle entry.

**Owner seams:** `SpiritualMeaningCoordinator`, `ZealotrySystem`,
`IdeologicalFrictionSystem`; the symmetry contract (fictional, mechanically
symmetrical belief; no piety meter).

**Cross-refs:** none required; the expansion may use 24's memorial surfaces for
rites of the dead, but independently.

**First tests:** zealotry symmetry; ritual inventory consumption through the
canonical inventory; no new belief ledger; journal exactly-once.

**Tone guard:** belief violence never glorified; schism is human, not a damage
system.

### 18.3 Expansion 22 — The Clean Flow (third)

**Slice:** one water source → treatment → storage → hygiene loop with one
quality profile and one failure mode, all through existing owners.

**Owner seams:** `WaterTreatmentSystem`, `BrineWaterSystem`, `SanitationSystem`,
`WaterborneExposureRules`, `SumpFloodingSystem`.

**Cross-refs:** expansion 23 will share the firefighting water surface; 26's
food water demand is a read; 15's irrigation is a read.

**First tests:** mass balance (water is never created); exposure only via
`DiseaseSystem.TryExpose`; hygiene derived, never stored; deterministic
quality outcomes; sump/overflow regression.

**Tone guard:** dignified sanitation prose; no gross-out descriptions.

### 18.4 Expansion 26 — The Common Table (fourth)

**Slice:** one menu/ration policy that changes one need outcome and one
preservation method with a real cost.

**Owner seams:** `KitchenNutritionSystem`, `NutritionDiversitySystem`,
`FoodPreservationSystem`, `ResourceRationingSystem`, `RationConflictSystem`,
`DesperationSystem`.

**Cross-refs:** Plan 2's boundary note (food is never currency); expansion 25
freight is a supply read, not a second logistics layer; Plan 1's needs
authority stays the only need writer.

**First tests:** all consequences via `NeedsSystem.Modify`; preservation cost;
ration policy transitions; no hidden nutrition bar; deprivation dignity review.

**Tone guard:** hunger is depicted with restraint; scarcity is systemic, not
punitive spectacle.

### 18.5 Expansion 23 — The Alarm (fifth)

**Slice:** one hazard → warning → evacuation/response → off-ramp chain with
one drill and one mutual-aid request.

**Owner seams:** `ShelterFireHazardSystem`, `CascadeCoordinator`,
`CascadeRuleCatalog`, `DiseaseTriage`, rescue dispatch.

**Cross-refs:** water for suppression reads expansion 22's owner; power/fuel
gating reads the grid owner; kiln heat reads 31's owner if both are live.

**First tests:** every cascade rule has a warning and an off-ramp; no scripted
death without a route; drill consumes small supplies; rescue risk/pull-back
deterministic; triage regression.

**Tone guard:** disaster is not spectacle; rescue has real cost.

### 18.6 The next five (brief)

| Plan | Slice | Owner seam | Notable guard |
|---|---|---|---|
| 15 Deep Root | one crop → food loop + one livestock/vet case | Agriculture/Greenhouse/Apiculture/Companion | growth stays with Greenhouse |
| 27 Thread | one garment layer + laundry loop | WornGear/Needs warmth/Atmosphere | warmth via NeedsSystem only |
| 31 Kiln | one lime → brick → shelter upgrade loop | Crafting + upgrade path + cupola refractory | permanence is care, not conquest |
| 29 Glass | one batch → lens → instrument/spectacles loop | PrecisionOptics + survey | vision consequences medical |
| 30 Press | one paper → type → printed notice loop | Archive/printing | print never broadcasts |

---

## 19. Programme closure criteria for the wave corpus

The intake programme is complete when:

1. Every plan in waves 1–4 has a status in the intake ledger (design /
   admitted / claimed / executed / declined).
2. No plan is executing without a P0 note and a claim.
3. The shared decisions (W-2..W-5) are enforced in each executed plan's
   evidence.
4. The cross-ref notes exist on every affected expansion.
5. The first-wave slices are playable and content-utilization clean.
6. The five-plan programme's other four plans have resolved their boundaries
   (Plan 1's F14, Plan 2's F13, Plan 3's freeze, Plan 4's truth) so the wave
   programme can run on a truthful queue.

The programme is **not** complete when all content is authored; it is complete
when the corpus is governable, the first slices are live, and no plan sits in
limbo.

---

## 20. Final summary — what the five plans release, together

| Original blocked item | Released by | Mechanism |
|---|---|---|
| XP-06 body integrity | Plan 1 | F14 schema signature |
| EN-04 rehabilitation | Plan 1 → Plan 5 | F14-E then authorization |
| Expansion 16 prosthetic half | Plan 1 → Plan 5 | prerequisite + admission |
| XP-04 economy legs | Plan 2 | F13-A/D signatures |
| XP-08 trade routes + migration | Plan 2 | F13-E/F signatures |
| EN-03 underground economy | Plan 2 → Plan 5 | F13 then authorization |
| C3 192 player routes | Plan 2 | route DTO signed by F13-E |
| C3 199 human migration | Plan 2 | population owner signed by F13-F |
| CF-P3 semantic kind | Plan 3 | D11 routing + test migration |
| Plan 42 survivor voice | Plan 3 | L42-A certification |
| Plan 46 playable metrics | Plan 3 | L46-A certification |
| Plan 49 depth passes | Plan 3 | L49-A activation packet |
| EN-05 signal continuity | Plan 3 → Plan 5 | freeze then authorization |
| DEC-11 VO / DEC-13 localisation | Plan 3 | freeze declaration |
| D3/D4/D13/D16/D19a/c/D21 | Plan 4 | micro-decisions + truth patches |
| Quarantine drain | Plan 4 | empty-truth declaration |
| E1C+ governance | Plan 4 | E1C authorization |
| Census 85 rows | Plan 4 | tranche-2 protocol |
| Plan 24 snapshot residual | Plan 4 | procedure (execution deferred) |
| Plan 32 aviation/naval residual | Plan 4 | owned debt row |
| EN-08 ledger truth | Plan 4 → Plan 5 | gate clearance then authorization |
| EN-01/02/06/07 | Plan 5 | authorization lines |
| C3 174 mechanical origins | Plan 5 | lift/retire decision |
| C3 175 profile store | Plan 5 | lift/hold/retire decision |
| XP-07 item provenance | Plan 5 | scope signature |
| XP-09 presenter skills | Plan 5 | decline or reversal vs DEC-17 |
| XP-10 phobia growth | Plan 5 | decline or reversal vs DEC-18 |
| Expansions 12–31 | Plan 5 | intake protocol + admission |

This table is the answer to "what does unblocking actually buy?" — every
nonterminal decision in the queue has a named release path, and none of the
paths requires a new architecture.

---

**End of UNBLOCK-05 top-up appendices.**