# ASHFALL — UNBLOCK PROGRAM · PLAN 4
## Ledger, Register, Quarantine and Census Truth: D3 / D4 / D13 / D16 / D19a / D19c / D21, E1 Continuation, Residuals

**Status:** planning deliverable only. Read-only pass. No production, data, test,
save, or governance-ledger file is modified by this document. No path is claimed.
**Date:** 2026-09-21
**Baseline verified at:** `Zcode_Branch`, HEAD `5be1a30a63cd86cf23e4034473b739ac514f0f2a`
(2026-09-20 02:03 +0300) plus the current uncommitted worktree (untracked
`docs/expansions/wave1..wave4/` only).
**Role:** unblock-plan author. This plan repairs the *truth* of the planning
system so that the other four unblock plans (and every future package) are
read against current facts. It implements nothing.
**Authority chain:** `AGENTS.md`, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`,
`TEST_POLICY.md`, `KNOWN_DEBT.md`, `docs/governance/DECISION_REGISTER.md`,
`docs/governance/DECISION_PACKET_2026-09-18.md`,
`docs/plans/UNCLAIMED_CORPUS_CENSUS.md`, `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md`,
`docs/plans/PLAN_24_CLOSEOUT.md`, `docs/roadmap/e1/`, and current source.

---

## 0. How to read this plan

| Sibling | Region | Primary releases |
|---|---|---|
| Plan 1 | Equipment/body schema | XP-06, EN-04, Expansion 16 |
| Plan 2 | Funds and trade | XP-04, XP-08, EN-03, C3 192/199 |
| Plan 3 | Semantic/voice/strings | D11/CF-P3, Plan 42/46/49, EN-05, DEC-11/13 |
| **Plan 4 (this)** | Ledger, register, quarantine, census | D3, D4, D13, D16, D19a, D19c, D21, E1C+, census audits, Plan 24 residual, EN-08 |
| Plan 5 | Newest expansions, C3/EN gate | Expansions 12–31, EN-01/02/06/07, XP-07/09/10 |

This plan is the **truth-repair** plan. Unlike Plans 1–3, its output is mostly
not production code: it is a set of signed decisions, exact ledger patches,
audit protocols, and one bounded defect repair. Its value is that every other
agent reads these ledgers before editing; stale ledgers cause repeated work,
phantom blockers, and re-litigated decisions.

Vocabulary: **truth row** = a ledger statement that is false or unverifiable at
current HEAD; **patch** = the exact edit that makes the row true; **premise
audit** = the Rule 7 re-check before the first edit; **reconciliation** = the
zero-code action of aligning evidence with the live verdict.

---

## 1. Executive summary

### 1.1 The problem in one paragraph

The queue's *decision* layer is largely resolved, but its *record* layer has
drifted. Verified at HEAD `5be1a30a`:

- the test quarantine is effectively **empty** (2 `Compile Remove` mentions, both
  inside comments) while `AGENTS.md` and the 2026-09-19 audit still describe a
  48-entry drain and a decision item (`D21`) — the premise for that blocker is
  gone, not resolved by a sweep;
- the decision register's own invariant ("zero items unsigned without a
  condition") is still formally false: `DEC-01` and `DEC-16` read
  `DEFERRED-WITH-CONDITION` although their named work was executed and sealed
  2026-09-18 (Plan 24 closeout, both signatures resolved);
- `INTEGRATION_PLANS.md` still shows `XP Expansion W1 — ACTIVE` while the
  `XP-WAVE1-DIFFICULTY-AUTHORITY` claim row says DONE and the source
  (`difficulty_presets.json`, `Main.Difficulty.cs`, 8 consumers,
  `--difficulty-selftest` 14/14) confirms full binding;
- `AGENTS.md`'s "ACTIVE QUEUE — UNBLOCKED PLANS" still lists `C2[15]/Plan 37`
  and `C2[21]/Plan 48` as available although both are SEALED in
  `WORKTREE_OWNERSHIP.md` and in the census;
- the corpus census's classification summary (38 sealed / 89 audit-pending /
  1 ready) does not match its own current rows or the 2026-09-19 audit's
  112-nonterminal measurement;
- one genuinely open defect candidate exists where the packet suspected one:
  `_sharedSkillProgression`, `_sharedFactionStance`, and `_silentFoundry` are
  cached across in-memory resets (`ResetPlansExpansionSessions` /
  `ResetEnrolledFlagshipSessions` never null them; `SetupSilentFoundry`
  early-returns on a non-null instance), which is a stale-campaign-state hazard
  (D19c);
- several micro-decisions remain unsigned: D3 (water sample equipability), D13
  (Plans 126–129 header), D16 (flooded-route tags — premise verified: zero
  tags), D19a (pre-campaign barter RNG fallback `new SeededRng(147)`);
- the Plan 24 snapshot rebaseline remains environment-blocked with no written
  procedure for the first renderer-capable session;
- Plan 32's aviation/naval residual is recorded as "documented" but has no
  ledger row of its own;
- E1/Plan 53's governance programme is live through E1A/E1B but E1C (corpus
  metadata migration) is intentionally unstarted.

None of these are architectural crises. All of them cost agents time and
occasionally cause a builder to start work that is already done or to avoid work
that is actually available. This plan supplies the signatures and the exact
patches.

### 1.2 Signature bundle in one glance

| # | Sign-off line | Releases | Blast radius |
|---|---|---|---|
| D3 | `water_sample_contaminated: [Option A strip equipability / Option B retain as lore].` | catalog truth; one item row | `items.json`, Plan21ProtectiveWearTests count if A, migration note |
| D4 | `I ratify DEC-15 (WeatherStationSystem sole forecast authority; radio weather diegetic).` | register invariant | `DECISION_REGISTER.md` only (already SIGNED; confirm) |
| D13 | `Plans 126-129: [(a) numbering drift — fix the comment / (b) genuine gap — open a plan / (c) leave as-is].` | one header line + one census note | `src/Main.Plans126_129.cs` comment, census |
| D16 | `Flooded-route tags: [declined — zero tags authored / author them in a future map tranche].` | map topology clarity | `wasteland_map_v1.json` only if authored; otherwise docs |
| D19a | `Pre-campaign barter RNG: [allowed with seeded fallback / must not be constructible].` | barter determinism policy | `src/Main.Plans147.cs` one line |
| D19c | *(not a decision — audit action)* verified candidate defect; routes to a bounded repair package | stale-state repair | `Main.CampaignServices.cs`, `Main.Lifecycle.cs`, `Main.Economy.cs` |
| D21 | `Quarantine drain: [record the reconciled empty truth / not worth it / restore a named historical source as a deliberate package].` | EN-08 gate; honest test policy | `AGENTS.md`, `TEST_POLICY.md` note, `KNOWN_DEBT.md` |
| L-LEDGER | `I authorize the ledger-truth patch matrix in §4.9.` | all stale rows | `AGENTS.md`, `INTEGRATION_PLANS.md`, `DECISION_REGISTER.md`, `KNOWN_DEBT.md`, census |
| L-E1C | `E1C corpus metadata migration is authorized as the next bounded E1 phase.` | E1 programme continuation, census freshness | `scripts/ci/plan_governance_config.json`, `PLAN_REGISTER.*`, E1 logs |
| L-CENSUS2 | `Census tranche-2 audits are authorized under the §5.9 protocol.` | 85 AUDIT-PENDING rows | `UNCLAIMED_CORPUS_CENSUS.md` evidence, audit logs |
| L-P24R | `Plan 24 snapshot rebaseline procedure accepted; execution deferred to the first renderer-capable session.` | Plan 24 residual closure path | `docs/plans/PLAN_24_CLOSEOUT.md` addendum, snapshot inventory |
| L-P32R | `Plan 32 aviation/naval residual recorded as an owned follow-up row.` | residual ownership | `KNOWN_DEBT.md` or census note |

### 1.3 What is NOT in this plan

- No re-litigation of sealed decisions (the 2026-09-18 treats D1/D2/D5–D10/D14/
  D15/D23-item-1 as closed; this plan verifies rather than reopens).
- No sweeping test re-enablement (D21 is a truth declaration first; restoration
  is a separate named package).
- No production change beyond the D19c bounded repair and the one-line D19a/D13
  options if chosen.
- No census row sealing by assertion; the tranche-2 protocol requires evidence.
- No renderer execution (the Plan 24 rebaseline procedure is written now,
  executed when a renderer session exists).

---

## 2. Verified current reality

### 2.1 Quarantine: the blocker's premise is gone

Command and result:

```text
$ grep -c 'Compile Remove' Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2
$ grep -n 'Compile Remove' Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
33:       Compile Remove entries. SDK-style projects already ignore absent source files;
34:       any future explicit Compile Remove quarantine must target a real file and is
```

Both matches are inside a comment block that names the *policy*. There are
**zero active `Compile Remove` exclusions**.

`KNOWN_DEBT.md` `DEBT-TEST-QUARANTINE-2026-09-12` is `RECONCILED` (2026-09-18):
"the 51 active `Compile Remove` entries were ghost metadata; their referenced
source files are absent from the current test tree … The ghost entries were
removed and `QuarantineManifestGateTests` now requires every future explicit
`Compile Remove` quarantine to reference a real source file."

The 2026-09-19 audit still says "48 active `Compile Remove` exclusions remain
(count verified)" — that count is stale by one day's work. `AGENTS.md` still
lists "quarantine drain (D21)" as decision-blocked.

**Truth:** the drain is not blocked; it is empty. What remains is (a) a decision
about whether any historical quarantined source should be restored as a
deliberate package, and (b) an honest ledger statement. Both are cheap.

### 2.2 Register: the invariant is still formally false

`docs/governance/DECISION_REGISTER.md` states:

> **Invariant:** Every decision item must have a terminal verdict … Zero items
> may remain unsigned without a condition.

Current rows:

| Row | Current verdict | Reality |
|---|---|---|
| `DEC-01` Medical Ward Staffing | `DEFERRED-WITH-CONDITION` | executed and SEALED 2026-09-18; `DEBT-PLAN24-MEDICAL-WARD-STAFFING` RETIRED; Plan 24 closeout says "both signature items resolved" |
| `DEC-16` Affliction Recovery Ramps | `DEFERRED-WITH-CONDITION` | closed per option (ii) 2026-09-18 (no ramp authority fabricated); Plan 24 closeout says resolved |
| `DEC-05` Restock | `SIGNED` | evidence corrected 6/6 on 2026-09-19; verdict unchanged |
| `DEC-03` Amputation equipment | `DEFERRED-WITH-CONDITION` | still genuinely deferred until Plan 1's F14 signs (this is truthful) |
| `DEC-17`/`DEC-18` | `RETIRED` | truthful |

The register's own invariant requires `DEC-01` and `DEC-16` to move to a
terminal verdict with their resolution evidence. The environment-blocked
snapshot rebaseline does not keep DEC-01 "unsigned": the decision (ward
staffing) was resolved; the residual is a rendering task, not a decision.

### 2.3 Integration ledger: XP W1 status drift

- `INTEGRATION_PLANS.md` line 8: "**XP Expansion W1 — ACTIVE (2026-09-18)**: …
  The first owned package is `XP-WAVE1-DIFFICULTY-AUTHORITY` …"
- `WORKTREE_OWNERSHIP.md` `claim-xp-wave1-difficulty-2026-09-18`: "**DONE
  2026-09-19:** XP-01 full binding complete. 4 catalog presets, selection at
  new-game, manifest persistence, 8 scalar consumers …, `--difficulty-selftest`
  14/14 PASS."
- Source confirms: `Assets/StreamingAssets/Data/difficulty_presets.json` exists;
  `src/Main.Difficulty.cs` has `ResolveDifficultyPresetId`,
  `SelectDifficultyForNewCampaign`, starter-bonus grant; git commit `d13e9322`
  "docs(plans): close out CF-XP01 …".

**Truth:** the current batch should present XP-01 as COMPLETE and either name
the next XP package or mark the batch pending a new head. Leaving "ACTIVE"
risks a builder re-claiming finished work.

### 2.4 AGENTS.md active queue: four of eight listed items are done

`AGENTS.md` §"ACTIVE QUEUE — UNBLOCKED PLANS (audited 2026-09-19)" lists eight
items. Verified status at HEAD:

| Listed item | Reality |
|---|---|
| `CF-P1-DISTRESS-CONTENT-SEAL` | SEALED (claim row DONE 2026-09-19) |
| `CF-P5-RESTOCK-RECONCILE` | available → closed by Plan 2's F13-B (or a standalone reconcile) |
| `CF-P6-VEHICLE-ARMOR-GRADES` | SEALED (claim row DONE) |
| `CF-P28-ONE-BOOTSTRAP-PATH` | SEALED (claim row DONE; composition-root gate resolved) |
| `CF-XP01-DIFFICULTY-FULL-BINDING` | SEALED (claim row DONE; commit `d13e9322`) |
| `E1/Plan 53` | ACTIVE through E1B; E1C next |
| `C2[15]/Plan 37` | SEALED (claim row + census) |
| `C2[21]/Plan 48` | SEALED (claim row + census) |

Six of the eight are done. The queue handoff is stale in the most expensive
possible way: it tells agents to consider work that is finished.

### 2.5 Census: summary counts do not match the table

The census's §3 classification summary still reads:

| Classification | Count |
|---|---:|
| SEALED | 38 |
| SEALED-ELSEWHERE | 2 |
| RECONCILED-DUPLICATE | 1 |
| READY-UNCLAIMED | 1 |
| AUDIT-PENDING | 89 |
| Total | 131 |

The live table has since gained many `SEALED` rows (Plan 132, Plan 220/205,
Plan 138, Plan 207, Plan 168, Plan 203, Plan 200/212/206/182, crop roster,
Plan 133, Plan 37, Plan 48, and the C2 anchors C2[9]–C2[13]) and loses
`AUDIT-PENDING` rows accordingly. Simple greps count 85 `AUDIT-PENDING` and 59
`SEALED` occurrences; the 2026-09-19 audit measured 112 nonterminal rows after
sealing the anchors. The numbers must be re-derived from the table in a single
generated pass rather than maintained by hand.

### 2.6 D3 — `water_sample_contaminated`

`Assets/StreamingAssets/Data/items.json` (row starts at line 3411):

```json
{
  "id": "water_sample_contaminated",
  ...
  "isEquipable": true,
  "equipSlot": "Body",
  "tradeValue": 6,
  ...
}
```

`DEC-09` says retained as intentional wasteland lore quirk
(`isEquipable: true`, body slot, rad protection 20) — but the row's
`radProtection` is **0** in current data (the row shows `radProtection: 0`
among the fields above; DEC-09's "rad protection 20" is stale). The decision
packet's D3 asks: strip equipability (Option A) or retain as lore (Option B).
The memo contradicts itself across three documents
(`WATER_SAMPLE_CONTAMINATED_DECISION_MEMO.md`,
`WAVE10_MICRO_DEFERRAL_SWEEP.md`, `DECISION_REGISTER.md` DEC-09): DEC-09 says
retain, the sweep recommended strip. The data says `isEquipable: true` today.

**Truth:** the row exists and is equipable; the register's lore rationale cites
a rad protection value the row does not have. The decision must be taken with
the current row in front of the foreman, and whichever way it lands, DEC-09's
condition text must be corrected.

### 2.7 D4 — DEC-15 ratification

`DEC-15` is already `SIGNED` in the register ("WeatherStationSystem remains the
sole forecast authority. Radio retains diegetic static/atmospheric emergency
lines"), with evidence `DailyBriefingReportBuilder.cs` /
`WeatherStationSystem.cs`. The 2026-09-18 packet's D4 asked the foreman to
ratify it. **Truth:** no code change is required; the only action is a dated
ratification note if the foreman wants the register's signature explicitly
re-affirmed. Otherwise D4 is closed by the existing SIGNED row.

### 2.8 D13 — Plans 126–129 header

`src/Main.Plans126_129.cs` lines 1–7:

```text
// Main Partial : Plans 126-129 Host Wire & Orchestration
// Subsystems   : Plan 126 — Subterranean Biological Fermentation (this wave)
//                Plans 127-129 land in follow-up waves of this flagship
//                (tethered recon drone, continuous steel casting, lidar).
```

The header says 127–129 "land in follow-up waves"; the 2026-09-19 audit says
"no drone/caster/lidar implementation exists; the claim is unproven numbering
drift". **Truth:** either (a) the comment is drift and should be corrected to
state 127–129 are not tracked, or (b) the gap is real and should open a plan.
Option (a) is a one-line comment fix; option (b) is a new package.

### 2.9 D16 — flooded-route tags

```text
$ grep -c "flooded\|amphibious" Assets/StreamingAssets/Data/wasteland_map_v1.json
0
```

**Truth:** zero flooded/amphibious tags exist. The decision is not "do the tags
work?" — it is whether to author them at all, and if so, in which map tranche.
The 2026-09-19 audit already verified this premise; the decision line is now
low-cost.

### 2.10 D19a — pre-campaign barter RNG fallback

`src/Main.Plans147.cs:240`:

```csharp
var rng = _campaignDay != null
    ? _campaignDay.Rng.Fork("shelter_barter")
    : new SeededRng(147);
```

When no campaign day exists (pre-campaign barter), the code seeds a fixed RNG
(147). The decision packet asks whether pre-campaign barter is allowed (with
this seeded fallback) or must not be constructible (disable the surface until a
campaign exists). **Truth:** the fallback is deterministic and documented in
code; the decision is a gameplay policy, not a determinism concern.

### 2.11 D19c — stale cached sessions (candidate defect, verified statically)

The 2026-09-18 packet item D19c asked to verify whether
`_silentFoundry`/`_sharedFactionStance`/`_sharedSkillProgression` are correctly
reset on new-game vs load. Static verification at HEAD:

| Object | Declaration | Lifecycle behavior |
|---|---|---|
| `_silentFoundry` | `src/Main.ExpansionHub.cs:35` (`= null!`) | created in `SetupSilentFoundry` (`src/Main.Economy.cs:218`); `SetupSilentFoundry` early-returns when non-null (`:212`); **never nulled** in `ResetPlansExpansionSessions` or `ResetEnrolledFlagshipSessions` |
| `_sharedSkillProgression` | `src/Main.CampaignServices.cs:149` | lazily created in `EnsureSharedSkillProgression` (`:184`); **never nulled** in either reset |
| `_sharedFactionStance` | `src/Main.CampaignServices.cs:195` | cached from `_silentFoundry.GuildStanceEngine` (`:203`); **never nulled** in either reset |

`ResetAllSessionsInMemory` (Main.Lifecycle.cs:521) calls
`_lifecycleRegistry.ResetAll()`, which invokes the two reset callbacks
(registered at Main.Lifecycle.cs:366 and :375). Because none of the three
fields is nulled by those callbacks, and because `SetupSilentFoundry` refuses
to rebuild while the field is non-null, a new campaign that reuses the same
Main instance can observe the previous campaign's foundry session, faction
stance, or skill-progression instance until something else replaces it.

This is a **candidate defect**, not a proven runtime bug: the fresh-game path
may construct a new Main/host object or may reset through a different route.
The audit action is to (1) trace the fresh-game path and (2) run a focused
runtime probe. If confirmed, it routes to a bounded repair package (add the
three nulls to the correct reset callback, with a reload-parity test) — not to
a foreman decision, per the packet's own wording.

### 2.12 D21 — quarantine truth and the historical sources

The reconciled debt says the 51 ghost entries referenced *absent* source files.
That means there is no current corpus to re-enable; restoration would mean
pulling named historical sources out of git/Twin evidence, rematching them to
current APIs, and proving a focused suite — a deliberate implementation
package, as the debt row says. The honest ledger state is:

1. active exclusions: 0;
2. `QuarantineManifestGateTests` enforces real-file references for any future
   exclusion;
3. no restoration package is currently named or authorized;
4. `AGENTS.md`/audit's "48 active" and "quarantine drain blocked" statements
   are false and must be corrected.

### 2.13 Plan 24 residual — snapshot rebaseline

`docs/plans/PLAN_24_CLOSEOUT.md` records:

- item 12 "Snapshot review — ENVIRONMENT-BLOCKED (documented): headless
  renderer unavailable (SubViewport needs a real display); no snapshot files
  touched by this wave; the intended render changes are enumerated below for
  the first renderer-capable rebaseline";
- "the environment-blocked snapshot rebaseline (item 12) is the only recorded
  residual and requires no further decision."

The residual has an inventory but no *procedure* (which snapshots, which
commands, which review criteria, how to record MATCH/DIFF). This plan supplies
the procedure; execution stays with the first renderer-capable session.

### 2.14 Plan 32 residual — aviation/naval travel migration

`docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` records: "Aviation/naval
migration remains a documented residual, not a blocker" and the census C2[11]
row says "aviation/naval residual recorded as debt-free docs". **Truth:** there
is no owned ledger row for the residual, so a future agent cannot find a
promotion condition. This plan records an owned follow-up (a debt row or a
census note with a recheck trigger).

### 2.15 E1/Plan 53 — E1C unstarted

`docs/roadmap/e1/E1B_IMPLEMENTATION_LOG.md`: "E1C metadata migration remains
intentionally unstarted." The tooling exists:
`scripts/ci/generate-plan-register.py`, `scripts/ci/plan_corpus_lib.py`,
`scripts/ci/plan_governance_config.json`, with a baseline
(`docs/roadmap/e1/E1_BASELINE.md`: 609 plans, 1769 markdown docs, namespaces
integration 245 / next_steps 230 / piagents 134). E1C is the corpus metadata
migration that would let the census and register be generated rather than
hand-maintained — directly relevant to §2.5.

### 2.16 The census tranche-2 work

85 `AUDIT-PENDING` rows remain (grep count at HEAD; the summary says 89). The
2026-09-19 audit says they "are audit work, not certified integration plans".
No tranche-2 protocol has been executed. This is the largest single
truth-repair opportunity: a bounded, sweep-role audit protocol that converts
rows to SEALED / SPLIT / STALE / IMPLEMENT with evidence.

---

## 3. Blocked-plan inventory released by this plan

### 3.1 Primary releases

#### 3.1.1 `CF-QD-QUARANTINE-DRAIN-TRANCHE` (roster 12, D21/F11)

- **Block:** D21 unsigned, ledger says 48 active exclusions.
- **Release:** D21 truth declaration. The drain package becomes either "closed
  empty" (recommended) or "restore named historical source X" (a deliberate
  package). EN-08's quarantine leg is cleared either way.

#### 3.1.2 `EN-08 Ledger Truth Program`

- **Gate:** per the 2026-09-19 audit, EN-08 builds on `CF-P5-RESTOCK-RECONCILE`
  (Plan 2's F13-B) + the quarantine-drain leg (this plan's D21). With both
  cleared, EN-08's authorization line can be signed in Plan 5.
- **Release:** the audit/ledger-truth machinery (census tranche-2 + register
  invariant + count reconciliation) becomes EN-08's execution substrate.

#### 3.1.3 E1/Plan 53 continuation (E1C+)

- **Block:** E1C "intentionally unstarted"; no authorization recorded.
- **Release:** L-E1C. E1C corpus metadata migration → E1D freshness verifier →
  E1E overlap clusters → E1F pillars/rubric → E1G ambition audit → E1H rails →
  E1I intake checker → E1J UI authority gate → E1K numbering/archive/co-author
  rules → E1L CI rollout → E1M roadmap publication → E1N metrics/review →
  E1O independent audit → E1P closure.
- **Value:** generated registers/census freshness, which prevents exactly the
  drift documented in §2.

#### 3.1.4 Plan 24 residual (snapshot rebaseline)

- **Block:** renderer-capable session; no procedure.
- **Release:** L-P24R supplies the procedure; the first renderer session can
  execute without a new decision.

#### 3.1.5 Census tranche-2

- **Block:** no protocol/authorization.
- **Release:** L-CENSUS2. 85 rows enter a bounded audit; nonterminal count
  finally reflects reality.

### 3.2 Micro-decision releases

| Item | Release |
|---|---|
| D3 | catalog truth; DEC-09 condition corrected |
| D4 | register ratification (or closure as already-signed) |
| D13 | header truth (one line) or a new named plan |
| D16 | map topology decision recorded; zero-tag premise pinned |
| D19a | barter policy recorded; one-line code option |
| D19c | bounded repair package (if confirmed) |

### 3.3 Secondary releases

| Item | Gain |
|---|---|
| `AGENTS.md` queue | stops pointing agents at finished work |
| `INTEGRATION_PLANS.md` batch | XP-01 closed; next head NAMED |
| `UNBLOCKED_PLANS_AUDIT_2026-09-19.md` | superseding counts for CF items |
| `KNOWN_DEBT.md` | Plan 32 residual row; register rows consistent |
| Plan 2's F13-B | closes `CF-P5-RESTOCK-RECONCILE` (shared with Plan 2) |
| Plan 3's audits | the census counts feed its L42/L46/L49 notes |

### 3.4 Non-releases

- No test re-enablement without a named historical source.
- No census row sealing without evidence.
- No production change beyond D19c (if confirmed) and the one-line options.
- No reopening of sealed decisions.

---

## 4. Decision packet

### 4.1 D3 — water sample equipability

**Sign-off line:**

> `water_sample_contaminated: [Option A strip equipability / Option B retain as lore].`

**Options with current-data truth:**

| Option | Effect | Blast radius |
|---|---|---|
| A. strip | set `isEquipable: false`, likely `equipSlot` removal; the item stays a document/sample | `items.json`; `Plan21ProtectiveWearTests` count if it enumerates equipables; a save-migration note if any save could hold it equipped |
| B. retain | keep equipable; correct DEC-09's rad-protection claim (row has 0; DEC-09 says 20); document the quirk | `DECISION_REGISTER.md`, `WAVE10_MICRO_DEFERRAL_SWEEP.md` note; `items.json` unchanged |

**Recommendation:** B is the lower-risk truth-preserving choice if the item was
authored as a quirk intentionally; A is cleaner if the foreman considers an
equipable water sample a defect. Either way, DEC-09's stale "rad protection 20"
must be corrected to the row's actual value. The execution ordering rule from
Plan 1 applies: this decision lands before Plan 1's Phase 3 authors
prosthetics into `items.json`, so the catalog is edited once.

### 4.2 D4 — DEC-15 ratification

**Sign-off line:**

> `I ratify DEC-15 (WeatherStationSystem sole forecast authority; radio weather diegetic).`

**Effect:** a dated confirmation line in the register; no code. If the foreman
prefers, the existing SIGNED row already satisfies the packet item and D4 can
be recorded as "already signed; ratification noted".

### 4.3 D13 — Plans 126–129

**Sign-off line:**

> `Plans 126-129: [(a) numbering drift — fix the comment / (b) genuine gap — open a plan / (c) leave as-is].`

**Options:**

| Option | Action |
|---|---|
| (a) | fix the header comment to state 127–129 are not tracked under this file; one line |
| (b) | open a named plan for the drone/caster/lidar gaps (which are real candidate features: tethered recon, continuous casting, lidar) |
| (c) | leave the ambiguity; costs nothing and continues to mislead |

**Recommendation:** (a) unless the foreman wants the features. The 2026-09-19
audit found no implementation and no proof of numbering drift either way; (a)
tells the truth in one line.

### 4.4 D16 — flooded-route tags

**Sign-off line:**

> `Flooded-route tags: [declined — zero tags authored / author them in a future map tranche].`

**Premise pinned:** zero `flooded`/`amphibious` occurrences in
`wasteland_map_v1.json`. If authored later, the map file's tranche owner must
add them with route-validation tests; this plan does not author them.

### 4.5 D19a — pre-campaign barter

**Sign-off line:**

> `Pre-campaign barter RNG: [allowed with seeded fallback / must not be constructible].`

**Options:**

| Option | Action |
|---|---|
| allowed | keep `new SeededRng(147)`; document it as the deterministic pre-campaign seed |
| not constructible | the barter surface refuses until a campaign day exists; one guard line |

**Recommendation:** allowed is fine because the fallback is deterministic and
pre-campaign states cannot be saved into a campaign slot without a campaign
envelope; the guard option is only needed if pre-campaign bartering is a
design smell.

### 4.6 D19c — stale cached sessions audit

**Sign-off line:** *(audit action, not a decision)*

> `D19c audit: [confirmed defect — open bounded repair / no defect — record the trace].`

**Repair design if confirmed** (bounded):

1. Add the three fields to the correct reset callback
   (`ResetEnrolledFlagshipSessions` for `_silentFoundry` + `_sharedFactionStance`;
   `ResetPlansExpansionSessions` for `_sharedSkillProgression` — or a single new
   `ResetSharedCampaignServices` callback, which is cleaner).
2. Guard `SetupSilentFoundry`'s early return remains valid.
3. Test: fresh-campaign-after-slot-switch parity — construct campaign A, reset,
   construct campaign B, assert B does not observe A's foundry unlock/stance/
   skill state; and a save/load parity test.
4. No new save section; this is instance hygiene.

**Blast radius:** `src/Main.CampaignServices.cs`, `src/Main.Lifecycle.cs`,
possibly `src/Main.Economy.cs` (null assignment), one focused test.

### 4.7 D21 — quarantine truth

**Sign-off line:**

> `Quarantine drain: [record the reconciled empty truth / restore historical source <name> as a deliberate package / defer].`

**Recommended action:** record the empty truth:

1. `AGENTS.md` decision-blocked list: remove "quarantine drain (D21)".
2. `TEST_POLICY.md` quarantine note: state the current count (0 active) and the
   gate that keeps future exclusions honest.
3. `KNOWN_DEBT.md`: `DEBT-TEST-QUARANTINE-2026-09-12` stays RECONCILED with the
   existing evidence (no change needed beyond the reviewed date).
4. `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md`: superseding note ("48" was
   stale; the entries were ghost metadata removed in the 2026-09-18
   reconciliation).

**Restoration option:** if a specific historical test family is wanted, name
the source file(s) and open a package; the debt row's promotion condition
already requires rematching + focused proof.

### 4.8 L-E1C — E1 continuation

**Sign-off line:**

> `E1C corpus metadata migration is authorized as the next bounded E1 phase.`

**Scope:** metadata migration for the 609-plan corpus per the E1B register
schema; no plan body edits; generated register/census outputs; E1D freshness
verifier locked to the migration. **Blast radius:** `scripts/ci/plan_*`,
`docs/roadmap/PLAN_REGISTER.*`, E1 logs, one tooling test family.

### 4.9 L-LEDGER — ledger-truth patch matrix

**Sign-off line:**

> `I authorize the ledger-truth patch matrix in §4.9.`

The matrix is the exact list of edits; the integrator applies it. Each row
names the file, the statement, the correction, and the evidence.

| File | Statement today | Corrected statement | Evidence |
|---|---|---|---|
| `AGENTS.md` | "quarantine drain (D21)" is decision-blocked | remove; quarantine active count is 0 | §2.1 |
| `AGENTS.md` | lists Plan 37 / Plan 48 / CF items as available | point at the sealed rows; keep only genuinely open items | §2.4, census |
| `AGENTS.md` | "semantic-kind re-grouping (D11)" blocked | updated by Plan 3's D11 signature | Plan 3 |
| `INTEGRATION_PLANS.md` | "XP Expansion W1 — ACTIVE" | "XP Expansion W1 — XP-01 COMPLETE; next head pending" (or name the next package) | §2.3, claim row |
| `INTEGRATION_PLANS.md` | stale restock sentence | reconciled per Plan 2 F13-B | Plan 2 §2.11 |
| `DECISION_REGISTER.md` | `DEC-01` DEFERRED-WITH-CONDITION | terminal verdict + resolution evidence | §2.2, `PLAN_24_CLOSEOUT.md` |
| `DECISION_REGISTER.md` | `DEC-16` DEFERRED-WITH-CONDITION | terminal verdict + resolution evidence | §2.2 |
| `DECISION_REGISTER.md` | `DEC-09` cites rad protection 20 | cite the row's actual `radProtection` value or the D3 outcome | §2.6 |
| `UNCLAIMED_CORPUS_CENSUS.md` | summary counts 38/89 | re-derived counts from the table (generated) | §2.5 |
| `UNBLOCKED_PLANS_AUDIT_2026-09-19.md` | "48 active" | superseding note | §2.1 |
| `KNOWN_DEBT.md` | no Plan 32 residual row | add the aviation/naval residual row with a recheck trigger | §2.14 |
| `KNOWN_DEBT.md` / register | Plan 24 residual phrasing | snapshot residual is procedural, not decision | §2.13 |

### 4.10 L-CENSUS2 — tranche-2 audits

**Sign-off line:**

> `Census tranche-2 audits are authorized under the §5.9 protocol.`

**Effect:** sweep-role audits over the 85 `AUDIT-PENDING` rows, in bounded
batches, converting each to SEALED / SEALED-ELSEWHERE / RECONCILED-DUPLICATE /
READY-UNCLAIMED / STALE / IMPLEMENT with evidence. The protocol is §5.9.

### 4.11 L-P24R and L-P32R

**Sign-off lines:**

> `Plan 24 snapshot rebaseline procedure accepted; execution deferred to the first renderer-capable session.`
> `Plan 32 aviation/naval residual recorded as an owned follow-up row.`

Both are procedural; §5.11 and §5.12 define them.

### 4.12 Signing order and first safe step

1. Sign D21 truth, D4/D13/D16/D19a (all cheap).
2. Sign L-LEDGER (the patch matrix).
3. Sign D3 (catalog truth) before Plan 1's Phase 3.
4. Run the D19c audit; if confirmed, open the bounded repair.
5. Sign L-E1C and L-CENSUS2; sign L-P24R/L-P32R.

**First safe step:** run the §2 verification commands and the D19c trace; they
are read-only and may run before any signature.---

## 5. Technical design (execution-ready after signature)

### 5.1 D3 — item row design and migration impact

**Data edit (Option A, strip):**

```jsonc
{
  "id": "water_sample_contaminated",
  // Option A removes or sets false:
  "isEquipable": false,
  // equipSlot removed (or left; validator should reject a slot on a
  // non-equipable item if such a rule exists; check before editing)
}
```

**Data edit (Option B, retain):** no data change; the register condition text
is corrected to the row's actual values.

**Migration impact analysis (required before the edit):**

1. Can a save hold the item equipped in a body slot? If yes, and Option A is
   chosen, the restore path must unequip it deterministically with one journal
   line (the Plan 1 auto-unequip pattern) — or the save-store restore must
   tolerate an equipped item whose definition is no longer equipable. The
   safest pattern is restore-tolerance: the item stays equipped until the next
   equip action; no forced mutation on load. A test pins this.
2. Does `Plan21ProtectiveWearTests` enumerate equipable items? If yes, its
   count changes under Option A; update the count in the same package with a
   drift note (the D1 truth-repair precedent).
3. Does any loot/quest grant the item? If a quest grants it as equipment,
   Option A changes the quest's reachable outcome; grep before editing.
4. Does the trade/valuation path treat it as gear? Check `tradeValue` context.

**Verification:** `--data-integrity-selftest` PASS; the Plan 21 suite green;
one save round-trip with the item equipped (legacy save fixture) proving
restore tolerance.

### 5.2 Register invariant mechanics

**Goal:** the invariant statement becomes true: every row has a terminal
verdict or a named condition with a recheck trigger.

**Terminal verdict definitions (already in the register):** `SIGNED`,
`DECLINED`, `DEFERRED-WITH-CONDITION`, `RETIRED`. The invariant allows
`DEFERRED-WITH-CONDITION` but requires the condition to be *unmet* and
*verifiable*. The failure mode is a condition that has been met while the row
still reads deferred.

**Audit procedure (row-by-row):**

1. For each non-terminal row (`DEC-01`, `DEC-03`, `DEC-09`, `DEC-16`), read its
   condition and its execution package.
2. Verify the package's status at HEAD (claim row, debt row, source).
3. If the condition is met and the package executed: set `SIGNED` (or
   `RETIRED` if the concept was superseded), record evidence, keep the
   recheck trigger as "closed; reopen only on new drift".
4. If the condition is unmet: confirm the trigger is specific and the package
   named; leave deferred.
5. If the condition is incoherent (cites values that do not exist, like
   DEC-09's rad protection 20): correct the condition text or convert the row.

**Expected outcomes at HEAD:**

| Row | Expected |
|---|---|
| `DEC-01` | `SIGNED` (ward staffing implemented; option b; debt RETIRED) |
| `DEC-16` | `SIGNED` or `DECLINED` (resolved per option ii; no ramp authority) — the foreman chooses the label; both are terminal and truthful |
| `DEC-03` | stays deferred until Plan 1's F14 signs |
| `DEC-09` | condition text corrected; verdict follows D3 |
| `DEC-05` | unchanged (`SIGNED`; 6/6) |

**Register maintenance rule to prevent recurrence:** a row's condition must
name a *package id*, and the wave-closeout routine must re-verify every
condition against the claim/debt ledger. This is E1N/E1O territory; until then,
Plan 4 records the checklist in the register's governance-cadence section.

### 5.3 Census regeneration design

**Problem:** the summary block is hand-maintained and stale.

**Solution:** generate the classification summary from the table or from the
E1 register.

Option A (minimal): extend the existing docs generator (or add a small script)
that counts `| \`C…\` | … | STATUS |` rows and rewrites the summary block
between markers. Add a `--check` mode to the CI gate manifest so drift fails.

Option B (E1-native): E1C's metadata migration produces a machine-readable
register (`docs/roadmap/PLAN_REGISTER.json`) that already contains plan
metadata; the census summary derives from it. This is the strategic option and
the reason E1C matters here.

Recommendation: Option A now (small, immediate truth), Option B as E1C's
deliverable (sustained truth). The Plan 4 patch matrix lists the immediate
correction; E1C replaces the mechanism.

### 5.4 D13 design

**Option (a) comment fix:**

```csharp
// Subsystems   : Plan 126 — Subterranean Biological Fermentation (this wave)
//                Plans 127-129 are not tracked in this file; see the census
//                (no drone/caster/lidar implementation exists at this HEAD).
```

**Option (b) new plan:** open a conventional integration plan for the three
candidate features. This is a project-level choice; if chosen, the plan must go
through the normal scaffold (premise evidence, authority map, phases).

**Verification:** for (a), a comment change needs no test; the census note
records the resolution.

### 5.5 D16 design (if tags are ever authored)

If the foreman chooses "author them in a future map tranche", the authoring
rules are:

1. Tags are additive fields on map edges/nodes (`flooded`, `amphibious`),
   `schema_version` bump per the map catalog convention.
2. Route validation: a flooded edge may delay/block ground travel; an
   amphibious edge requires an amphibious capability (the draisine/boat owner).
3. Determinism: no RNG; the conditions are authored.
4. Consumers: expedition/caravan routing reads the tags; wildlife/trade
   overlays do not.
5. Tests: route validation + one expedition refusal/delay case.

This plan does not authorize the tranche; it records the rules so a future
package is bounded.

### 5.6 D19a design

**Option allowed (documented):**

```csharp
// Pre-campaign barter uses a fixed seed so previews are deterministic before a
// campaign clock exists. Decided in PLAN 4 D19a; do not change without a new
// decision.
private const int PreCampaignBarterSeed = 147;
var rng = _campaignDay != null
    ? _campaignDay.Rng.Fork("shelter_barter")
    : new SeededRng(PreCampaignBarterSeed);
```

**Option not constructible:**

```csharp
if (_campaignDay == null) return null; // or a typed refusal at the call site
```

plus a panel guard so the surface is not offered pre-campaign.

**Verification:** with "allowed", one determinism test repeated twice gives the
same preview; with "not constructible", one test asserts refusal.

### 5.7 D19c — bounded stale-state repair design

**Audit first (read-only, 0.5 day):**

1. Trace the fresh-game path: what happens on "New Campaign" after a loaded
   campaign? Find the exact call sequence (`ResetAllSessionsInMemory`? a new
   Main? a scene reload?).
2. If the path constructs a new Main instance, the cached fields die with the
   old instance and there is **no defect**; record the trace and close D19c.
3. If the path reuses the instance and relies on the reset callbacks, the
   defect is confirmed for any field not nulled.

**Repair design (if confirmed):**

Add one reset callback, e.g.:

```csharp
private void ResetSharedCampaignServices()
{
    _silentFoundry = null!;
    _silentFoundryPanel = null!;
    _sharedFactionStance = null;
    _sharedSkillProgression = null;
    _silentFoundryDirty = false; // if such a flag exists
}
```

Register it with the other reset callbacks in `Main.Lifecycle.cs` (the
`_lifecycleRegistry` registration block at lines ~366/375) so it runs inside
`ResetAllSessionsInMemory`. Placement matters: `_sharedFactionStance` depends on
`_silentFoundry`, so null both together.

**Test design:**

1. Build campaign A with the foundry unlocked and a skill progressed.
2. Reset (the same path a new campaign uses).
3. Build campaign B.
4. Assert B's foundry is sealed-by-default and B's stance/skills do not carry
   A's state (or, if the fresh path is a new instance, assert the trace).
5. A save/load parity test ensures no regression.

**Risk:** nulling `_silentFoundry` while a panel still references it. The
existing resets already unbind panels (`_blackMarketPanel?.Unbind()`,
`RemovePanel(...)`), so the repair must follow the same pattern for the foundry
panel.

### 5.8 D21 — quarantine truth and restoration policy

**Truth declaration:**

1. Active `Compile Remove` exclusions: **0**.
2. `QuarantineManifestGateTests` enforces real source files for any future
   exclusion.
3. Historical drafts are recoverable from git/Twin; restoration is a deliberate
   package requiring rematch + focused proof.

**If restoration is wanted (separate package):**

1. Name the source file(s) and the historical location.
2. Rematch to current APIs/content (the debt row's condition).
3. Restore the file and drop any exclusion.
4. Run the focused target alone first, then the directory.
5. Record in `KNOWN_DEBT.md`/`TEST_POLICY.md`.

**Ledger corrections:** per §4.7/§4.9.

### 5.9 Census tranche-2 audit protocol

This is the largest piece of work this plan authorizes. It is read-only.

**Batch rule:** audit rows in batches of 8–12 (the corpus is 131 rows; 85
pending). Each batch produces one audit log; no batch edits production.

**Per-row evidence standard:**

| Evidence | Required |
|---|---|
| Baseline/source plan read | yes |
| Current owner identified | file:line or "no owner exists" |
| Data/catalog reachability | grep + loader/consumer paths |
| Declared dependencies checked | yes (DAG edges) |
| Classification | one of the six |
| Promotion/recheck condition | required for non-terminal classifications |

**Classification rules:**

| Classification | When |
|---|---|
| SEALED | the plan's subject is executed and verified in current source + tests |
| SEALED-ELSEWHERE | another plan's closeout demonstrably covers the same subject |
| RECONCILED-DUPLICATE | the plan is a duplicate of a sealed plan (banner + provenance) |
| READY-UNCLAIMED | prerequisites sealed, plan current, no signature needed |
| STALE | premise no longer exists (a plan is not proof) — record why |
| IMPLEMENT | a real, current gap with a bounded package shape |

**Output format (per row):** a table row with the evidence and a one-line
reason. The protocol must never seal from a filename collision (the census's
own key finding: 39 of 42 number-matches were collisions).

**Batch sequencing:** dependency-topological where the DAG has edges;
otherwise alphabetical by corpus id. The first batch should include the
Cycle-1 rows around the plan 42/46/49 chain to feed Plan 3's certifications.

**Acceptance:** after all batches, the census summary is regenerated (per §5.3)
and the nonterminal count is truthful.

### 5.10 E1C — corpus metadata migration design

**Input:** E1A baseline (609 plans, 1769 docs) + E1B register schema/tooling.

**Output:** every corpus plan carries machine-readable metadata (id,
namespace, title, status, dependencies, evidence links) in
`docs/roadmap/PLAN_REGISTER.json`/`.md`, generated from the corpus by
`generate-plan-register.py`, not hand-maintained.

**Boundaries:**

1. Metadata only; no plan body edits; no plan deletion.
2. Migration is idempotent and `--check`-able.
3. IDs derive from the E1B schema (the census numbering vs filename collision
   rule is encoded).
4. The census can consume the register (Option B in §5.3).

**Verification:** generator `--check` stable across two runs; one contract test
for the schema; a sample of migrated plans compared to their headers.

### 5.11 Plan 24 snapshot rebaseline procedure

**Purpose:** make the residual executable without a new decision.

**Procedure (for the first renderer-capable session):**

1. Read `docs/plans/PLAN_24_CLOSEOUT.md` item 12's "intended render changes"
   inventory.
2. Confirm the repo's snapshot mechanism and command (the snapshot harness is
   the authority; do not invent one).
3. Render each affected panel at the canonical resolution; compare against the
   stored snapshot; record MATCH/DIFF per panel.
4. For DIFF: verify the diff is the intended change (the inventory enumerates
   intended changes); if intended, rebaseline the snapshot; if not, file a
   regression.
5. Record the run: date, renderer/host, resolution, list of MATCH/DIFF, and
   the decision per diff.
6. Update `PLAN_24_CLOSEOUT.md`'s residual line to CLOSED with the evidence.

**Environment note:** the session must have a real display/GPU per the closeout
note; do not attempt on headless and do not claim a match without rendering.

### 5.12 Plan 32 residual row

Add to `KNOWN_DEBT.md` (proposed id `DEBT-PLAN32-AVIATION-NAVAL-TRAVEL`):

| Field | Value |
|---|---|
| Status | `ACCEPTED` (residual, not blocker) |
| Area | graph travel — aviation/naval legs |
| Evidence | `UNBLOCKED_PLANS_AUDIT_2026-09-19.md` §2 item 6; census C2[11] |
| Owner role | Integrator |
| Promotion condition | an aviation/naval travel package is proposed with a named consumer; aviation/naval systems already exist but do not consume the map graph |
| Reviewed | 2026-09-21 |

This gives the residual an owner and a trigger instead of a prose note.

### 5.13 EN-08 gate clearing

After D21 + Plan 2's F13-B:

| EN-08 gate leg | State |
|---|---|
| restock reconcile (`CF-P5`) | closed by Plan 2 F13-B |
| quarantine drain (`D21`) | truth-recorded empty |
| register invariant | restored by §5.2 |
| census freshness | protocol running (§5.9) + E1C mechanism (§5.10) |

Plan 5 may then sign EN-08's authorization with these as its execution
substrate.

### 5.14 Ledger-truth verification design

Every patch in §4.9 is verified by a command, not an assertion:

| Patch | Verification |
|---|---|
| AGENTS quarantine removal | `grep -c 'Compile Remove'` still 2 comments + manual read |
| AGENTS queue refresh | every listed item's claim/debt status checked |
| INTEGRATION_PLANS XP status | claim row + source + selftest |
| DEC-01/16 verdicts | Plan 24 closeout + debt rows + tests |
| DEC-09 text | item row read |
| census counts | regenerated summary `--check` |
| residual rows | row exists with a trigger |

A truth patch without its command evidence is not done.

### 5.15 Open questions (execution-time)

1. Does the restore path tolerate an equipped item whose definition loses
   `isEquipable` (D3 Option A)? Inspect before editing.
2. Is there a `--check`-able docs generator that can own the census summary,
   or does one need extending?
3. Does the fresh-game path construct a new Main instance (D19c)? Trace.
4. Which claim/debt rows does the AGENTS.md queue need to point at for the six
   completed items?
5. Does E1C's register schema already encode the census classification, or does
   the census remain a separate generated artifact?
6. What is the exact snapshot harness command for Plan 24's procedure?

### 5.16 Enrichment texts (paste-ready)

**`AGENTS.md` queue block** — rewrite the available list to: E1 continuation
(E1C+), Plan 42/46/49 chain (Plan 3), D11/D22 (Plan 3), D-series micro (this
plan), expansion waves (Plan 5), and name the XP-01 complete status.

**`INTEGRATION_PLANS.md` current batch** — replace the ACTIVE heading with the
completed status plus the next-head placeholder:

> **XP Expansion W1 — XP-01 COMPLETE (2026-09-19).** `XP-WAVE1-DIFFICULTY-AUTHORITY`
> delivered the canonical catalog, new-game selection, manifest persistence,
> and 8 scalar consumers; `--difficulty-selftest` 14/14. The next XP package is
> not yet claimed; see the unblock plans for XP-04 (Plan 2) and XP-06 (Plan 1).

**`DECISION_REGISTER.md`** — per §5.2; add a "recurrence guard" sentence to the
governance cadence: every wave closeout re-verifies each condition against the
claim/debt ledger.

**`UNCLAIMED_CORPUS_CENSUS.md`** — the regenerated summary plus a note that
tranche-2 is running under Plan 4's protocol.

**`docs/plans/PLAN_24_CLOSEOUT.md`** — append the procedure reference and keep
the residual OPEN until the renderer session runs.

**Plan 2's F13-B** — cross-reference: the restock reconciliation is Plan 2's
package; this plan's patch matrix records the ledger edit as shared.

**`KNOWN_DEBT.md`** — add the Plan 32 residual row; update reviewed dates for
the touched rows.---

## 6. Phased execution program

### Phase group A — Micro-decisions (0.5–1 day)

**A0 — premise (0.25 day).** Re-run the Appendix A commands; confirm each
micro-decision's current state (D3 row, D13 header, D16 zero tags, D19a
fallback, D4 register row).

**A1 — signatures recorded (0.25 day).** D3, D4, D13, D16, D19a lines into the
register/packet.

**A2 — minimal edits (0.25–0.5 day).**
- D3: one catalog edit (Option A) or one register correction (Option B), plus
  the failure-proof test if A.
- D13: one comment line (Option a).
- D16: a register/census note (declined) — no data.
- D19a: a documented constant (allowed) or one guard line (not constructible).

**A3 — verification (0.25 day).** Data-integrity; Plan 21 suite if touched; the
one determinism/refusal test.

### Phase group B — Ledger truth patches (0.5–1 day)

**B0 — re-derive current status per item (0.25 day).** For each row in §4.9,
gather the evidence command and output.

**B1 — apply patches (0.5 day).** AGENTS queue, INTEGRATION_PLANS batch,
register rows, census summary correction, audit superseding notes, debt rows.

**B2 — verification (0.25 day).** Re-run the evidence commands and confirm each
patched statement is now true; run any affected generated `--check`.

### Phase group C — D19c audit + repair (1–1.5 days)

**C0 — trace (0.5 day).** Determine the fresh-game path's instance lifecycle;
record the trace with file:line. Outcome: confirmed / no defect.

**C1 — repair (0.5 day, only if confirmed).** Add the reset callback; null the
three fields with correct ordering; follow the panel-unbind pattern.

**C2 — tests (0.5 day).** Fresh-after-slot-switch parity + save/load parity;
run adjacent suites.

### Phase group D — Census tranche-2 (ongoing; 0.5–1 day per batch)

**D0 — batch selection (0.25 day).** Choose 8–12 rows; include the Plan 42/46/49
chain rows first.

**D1 — audit (0.5 day).** Produce the batch log with the §5.9 evidence standard.

**D2 — classification and census update (0.25 day).** Apply rows; never seal
from a filename collision.

**D3 — summary regeneration (0.25 day).** Run the generator/check.

Repeat until the pending set is drained or the authorized tranches end.

### Phase group E — E1C metadata migration (per the E1 programme; 2–4 days)

**E0 — schema freeze (0.5 day).** Read the E1B register schema; confirm the
metadata fields and collision rules.

**E1 — migration tooling (1–2 days).** Extend `generate-plan-register.py` to
emit the metadata for all corpus plans; idempotent; `--check`.

**E2 — generation + review (1 day).** Generate; sample-compare vs headers;
publish `PLAN_REGISTER.*`.

**E3 — census consumption decision (0.5 day).** Wire or document the census's
use of the register (Option B).

### Phase group F — Residual procedures (0.5 day)

**F0 — Plan 24 procedure** written into the closeout; execution deferred.

**F1 — Plan 32 residual row** added with a trigger.

**F2 — EN-08 gate note** recorded for Plan 5.

### Phase group G — Closeout (0.5 day)

Register/census/ledger rows; enrichment texts; handoff.

**Total estimated effort:** 4–7 builder-days for the bounded work (A–C, F, G)
plus the tranche-2 batches and the E1C programme (separately authorized
duration).

---

## 7. Verification plan

### 7.1 Focused verification matrix

| Phase | Command | Expected |
|---|---|---|
| A | `grep -n "water_sample_contaminated" -A 12 items.json` | row matches the signed decision |
| A | `grep -c "flooded\|amphibious" wasteland_map_v1.json` | 0 (or authored per decision) |
| A | `grep -n "SeededRng(147)" src/Main.Plans147.cs` | documented constant or guard |
| A | `bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/Plan21ProtectiveWearTests.cs` | green (if touched) |
| A | `godot --headless --path . -- --data-integrity-selftest` | PASS |
| B | re-run each §4.9 evidence command | patched statement true |
| B | generated docs `--check` | PASS |
| C | trace commands + new parity test | no-defect trace or green repair |
| C | `bash scripts/run_test.sh Ashfall.Core.Tests/…/CampaignReloadParity*.cs` | green |
| D | per-batch audit log | every row classified with evidence |
| D | census summary `--check` | counts match the table |
| E | `python3 scripts/ci/generate-plan-register.py --check` | stable across runs |
| E | E1 contract test | green |
| F | closeout/debt row present | trigger recorded |
| G | all ledger evidence commands | true |

### 7.2 Commands

```bash
# A — premises
grep -n "water_sample_contaminated" -A 12 Assets/StreamingAssets/Data/items.json
sed -n '1,10p' src/Main.Plans126_129.cs
grep -c "flooded\|amphibious" Assets/StreamingAssets/Data/wasteland_map_v1.json
sed -n '236,246p' src/Main.Plans147.cs
grep -n "DEC-01\|DEC-16\|DEC-09\|DEC-15" docs/governance/DECISION_REGISTER.md

# A — quarantine truth
grep -c 'Compile Remove' Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
grep -n 'Compile Remove' Ashfall.Core.Tests/Ashfall.Core.Tests.csproj

# C — D19c trace
grep -n "ResetAllSessionsInMemory\|ResetPlansExpansionSessions\|ResetEnrolledFlagshipSessions" \
  src/Main.Lifecycle.cs
grep -n "_sharedSkillProgression\|_sharedFactionStance" src/Main.CampaignServices.cs
grep -n "SetupSilentFoundry" -A 6 src/Main.Economy.cs

# D — census
grep -c "AUDIT-PENDING" docs/plans/UNCLAIMED_CORPUS_CENSUS.md
sed -n '152,165p' docs/plans/UNCLAIMED_CORPUS_CENSUS.md

# E — E1
python3 scripts/ci/generate-plan-register.py --check
head -40 docs/roadmap/e1/E1_BASELINE.md

# shared gates
dotnet build Ashfall.csproj --no-restore
godot --headless --path . -- --data-integrity-selftest
python3 scripts/ci/generate-docs-index.py --check
```

### 7.3 Failure-proof obligations

- D3 Option A: a legacy-save-with-item-equipped test must fail (or be proven
  tolerant) before the data edit ships.
- Register patches: each terminal verdict must cite the evidence command; a
  verdict without evidence is rejected.
- D19c: if confirmed, the parity test must fail before the reset callback
  exists.
- Census: a batch that seals a row from a filename collision is rejected by the
  protocol's own rule.
- E1C: the generator must be stable across two runs; an unstable generator is
  not done.

### 7.4 What is not accepted as evidence

- "The audit said so" — re-verify at HEAD.
- A count copied from an older document without a fresh command.
- A census seal without owner/reachability evidence.
- A register verdict without its execution evidence.

---

## 8. Risks and mitigations

| # | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| 1 | Truth patches create a second source of truth | low | medium | patches cite the owning authority; no new ledgers |
| 2 | D3 Option A breaks a legacy save with the item equipped | low | medium | restore-tolerance test; unequip only if required |
| 3 | D19c repair nulls a field still referenced by a panel | medium | medium | follow the existing unbind/RemovePanel pattern |
| 4 | D19c trace finds a new-instance path (no defect) | medium | low | record and close; no code |
| 5 | Census tranche-2 expands unboundedly | medium | high | batch cap; authorized tranches; stop rule |
| 6 | Census sealing from filename collision | medium | high | protocol rule + a collision check per row |
| 7 | E1C migration rewrites plan bodies | low | high | metadata-only boundary; generator diff review |
| 8 | Register condition text is corrected but recurrence continues | medium | medium | recurrence guard in the cadence section |
| 9 | Plan 24 procedure executed headless by mistake | low | medium | environment note + explicit display requirement |
| 10 | Patch matrix collides with another plan's edits | low | medium | check `WORKTREE_OWNERSHIP.md` immediately before edit |
| 11 | AGENTS queue rewrite loses a genuinely open item | medium | high | compare against census + claims before/after |
| 12 | Known-debt/dates churn without evidence | low | low | evidence command per row |

---

## 9. Ownership, sequencing, coordination

### 9.1 Proposed claims

| Phase group | Claim | Owner | Disjoint paths |
|---|---|---|---|
| A | `UNBLOCK-04-A-MICRO-DECISIONS` | integrator/builder | items.json (D3), Main.Plans126_129.cs, Main.Plans147.cs, register |
| B | `UNBLOCK-04-B-LEDGER-TRUTH` | integrator | AGENTS.md, INTEGRATION_PLANS.md, register, census, audit notes, debt |
| C | `UNBLOCK-04-C-CACHED-SESSIONS` | builder | Main.CampaignServices.cs, Main.Lifecycle.cs, Main.Economy.cs, tests |
| D | `UNBLOCK-04-D-CENSUS-TRANCHE2` | sweep | census + audit logs (read-only production) |
| E | `UNBLOCK-04-E-E1C` | integrator | scripts/ci/plan_*, PLAN_REGISTER.*, E1 logs |
| F | `UNBLOCK-04-F-RESIDUALS` | integrator | PLAN_24_CLOSEOUT.md, KNOWN_DEBT.md |
| G | `UNBLOCK-04-G-CLOSEOUT` | integrator | ledgers/register |

### 9.2 Race rules

- `DECISION_REGISTER.md`, `AGENTS.md`, `INTEGRATION_PLANS.md`, `KNOWN_DEBT.md`,
  census: integrator-only writers; other plans propose text.
- D3's `items.json` edit must sequence with Plan 1 (F14 catalog tranche) and
  Plan 2 (economy catalogs) — check ownership before editing.
- Census tranche-2 is read-only on production; it may run in parallel with
  builders.
- E1C touches only governance tooling/docs.

### 9.3 Cross-plan coordination

| Sibling | Interface | Rule |
|---|---|---|
| Plan 1 | D3 ordering; register rows | D3 lands before Plan 1 Phase 3; Plan 4 records F14 rows after signing |
| Plan 2 | F13-B restock reconcile | Plan 2 owns the package; Plan 4 records the ledger edit |
| Plan 3 | census rows for 42/46/49; DEC numbering | coordinate DEC ids; Plan 3's audits feed tranche-2 |
| Plan 5 | EN-08 gate; residual rows | Plan 4 clears the gate; Plan 5 signs EN-08 |

---

## 10. Rollback and decline paths

| Deliverable | Rollback |
|---|---|
| Micro-decisions | each is independently revertible; Option A catalog edit reverts to the prior row |
| Ledger patches | text patches revert by restoring the prior wording; evidence commands re-run |
| D19c repair | remove the nulls/callback; no save impact |
| Census tranche-2 | classification rows revert; no production impact |
| E1C | generated metadata is additive; revert the generator + regenerate |
| Residual procedures | docs only |

Declines:

- Decline D3: catalog stays; DEC-09 text still needs correction or the
  contradiction persists.
- Decline D13: ambiguity persists.
- Decline D21 truth: the ledger stays false; EN-08 stays gated.
- Decline L-E1C: the register stays hand-maintained and drift recurs.
- Decline L-CENSUS2: the 85 rows stay unaudited; the queue's largest unknown
  remains.

---

## 11. Definition of done and handoff

### 11.1 DoD per deliverable

| Deliverable | Done when |
|---|---|
| D3 | one signed option applied; DEC-09 text corrected; tests/round-trip green |
| D4 | ratification line or "already signed" note recorded |
| D13 | header states the truth; census note recorded |
| D16 | decision recorded; zero-tag premise pinned |
| D19a | policy recorded; code/doc option applied |
| D19c | trace recorded; repair green or no-defect closed |
| D21 | quarantine truth recorded in AGENTS/TEST_POLICY/audit note |
| L-LEDGER | every §4.9 patch applied and verified |
| L-E1C | generator stable; register published; census consumption decided |
| L-CENSUS2 | all authorized batches classified with evidence; summary regenerated |
| L-P24R | procedure written; residual remains OPEN until rendered |
| L-P32R | residual row exists with a trigger |
| EN-08 gate | all legs cleared; note recorded for Plan 5 |

### 11.2 Handoff fields

Outcome, files, contract, evidence, shared paths untouched, proposed ledger
edits (already applied), next safe step.

### 11.3 First safe step

> Run the §2/Appendix A verification commands and the D19c trace. All are
> read-only and may run before any signature.

---

## 12. Worked scenarios

### 12.1 A builder reads AGENTS.md after Plan 4

1. The queue no longer lists Plan 37/48/CF items as available.
2. The quarantine drain is gone from the blocked list; the truth note says 0
   active exclusions.
3. XP-01 is COMPLETE; the next head is named (XP-04/XP-06 via the unblock
   plans).
4. The builder picks a genuinely open package instead of re-doing finished
   work.

### 12.2 The register invariant verified

1. An integrator runs the row-by-row audit.
2. DEC-01 moves to SIGNED with the ward-staffing evidence.
3. DEC-16 moves to its terminal verdict with the option (ii) evidence.
4. DEC-03 stays deferred with its unmet condition (Plan 1 pending).
5. DEC-09's condition text is corrected to the row's actual values.
6. The register's invariant now holds; the cadence section records the
   recurrence guard.

### 12.3 D19c confirmed and repaired

1. The trace shows the fresh-game path reuses the instance and calls
   `ResetAllSessionsInMemory`.
2. A parity test fails: campaign B observes campaign A's foundry unlock.
3. The repair adds the reset callback with correct ordering and panel unbind.
4. The test passes; a save/load parity test also passes.
5. The fix is reported as a bounded repair, not a decision.

### 12.4 A census batch runs

1. Batch 1: rows around the Plan 42/46/49 chain (8 rows).
2. Each row gets owner/reachability/classification evidence.
3. Two rows are IMPLEMENT with bounded packages; one is STALE (premise gone);
   five remain AUDIT-PENDING for a later batch with named unknowns.
4. The summary regenerates; counts match.

### 12.5 E1C makes the census generated

1. The migration emits metadata for 609 plans.
2. The generator runs twice with identical output.
3. The census summary is now derived, not hand-maintained.
4. The next wave's classification counts cannot drift.

---

## 13. Foreman briefing — anticipated questions

**Q1. Is this plan real work or bookkeeping?**
Both, but the bookkeeping is the point: every agent reads the ledgers first.
The bounded code work is the D19c repair and the D3 catalog decision; the rest
is truth that prevents repeated work and phantom blockers.

**Q2. Why not just fix the files without a decision?**
Because the ledgers are foreman/integrator-owned and the register's invariant is
a governance claim. Patches without an authorizing line repeat the drift.

**Q3. Is D21 really closed if no tests were restored?**
Yes, in the sense that the drain's premise (active exclusions) no longer
exists. Restoration of a historical source is a separate, deliberate package,
not a drain.

**Q4. Why is there a defect (D19c) in a plan about ledgers?**
Because the packet named it as an audit action and the static evidence points
to a real stale-state hazard. It is included because it is cheap to verify and
bounded to repair — and leaving a suspected state-leak undocumented is exactly
the kind of silent gap this plan exists to remove.

**Q5. Does D3 matter?**
It is one item row, but it is also the register-vs-data contradiction that
makes agents distrust the ledgers. And it sits in the same catalog region as
Plan 1's schema rows, so resolving it first prevents a double edit.

**Q6. What does E1C actually change for the player?**
Nothing directly. It makes plan governance machine-readable so the queue stops
drifting, which is what makes every future wave cheaper.

**Q7. How many census batches are authorized?**
The signature authorizes the protocol; each batch is bounded and reported. The
foreman may stop after any batch.

**Q8. What if a batch finds a real gap?**
It is classified IMPLEMENT with a bounded package shape; it does not get
implemented inside the audit.

**Q9. Why not seal the 85 rows by counting filename matches?**
Because the census's own key finding is that 39 of 42 filename matches were
numbering collisions. Sealing from a filename would create false seals.

**Q10. Does the Plan 24 procedure need the same renderer as the snapshot
gate?**
Yes; the procedure is explicit that headless runs cannot certify a match.

**Q11. Why record Plan 32's residual instead of fixing it?**
Because it is not a blocker and no consumer is named; the row gives it an owner
and a trigger instead of prose.

**Q12. What is the smallest useful action here?**
The D21 truth + register rows + AGENTS queue. That alone stops agents from
starting finished or phantom work. Roughly half a day.

**Q13. What is the largest action?**
The census tranche-2 + E1C, which together make the corpus governance
self-maintaining.

**Q14. Are we reopening sealed decisions?**
No. The plan verifies seals and corrects records; it does not re-litigate.

**Q15. How does EN-08 benefit?**
Its gate legs (restock reconcile, quarantine drain, register invariant, census
freshness) are exactly this plan's outputs.

**Q16. What if DEC-16's foreman prefers a different label?**
Any terminal verdict is acceptable as long as it is truthful; the register
defines SIGNED/DECLINED/DEFERRED/RETIRED. Option (ii) was executed; the label
should reflect whether any ramp work remains (none) and why.

**Q17. What stops the queue from going stale again?**
The recurrence guard: every wave closeout re-verifies each condition against
the claim/debt ledger, and E1C makes the counts generated.

**Q18. Does this plan touch save files?**
Only D3's restore-tolerance question, and only if Option A ships. No schema
change.

**Q19. Does this plan touch tests?**
Only to add the D3/ D19a proof tests if those options are chosen, and the D19c
parity test if the defect is confirmed.

**Q20. What is the single most important outcome?**
A single, current truth: no plan is listed as blocked when its blocker is
gone, and no plan is listed as available when it is sealed.

---

## 14. Patch matrix — concrete replacement text

The following are the exact minimal texts the integrator can paste. They are
kept terse so review is fast.

### 14.1 AGENTS.md — blocked list

Replace the current blocked sentence with:

> Still **decision-blocked** — never start without the named signature:
> semantic-kind re-grouping (D11 — see UNBLOCK-03), XP-04 economy legs (F13 —
> see UNBLOCK-02), XP-06 body-integrity schema (F14 — see UNBLOCK-01), EN-01…EN-08
> proposals (EN-03/04/05/08 have named gates in the unblock plans; EN-01/02/06/07
> await Plan 5 authorization), Plan 49 (needs the Plan 42/46 certifications in
> UNBLOCK-03), C3 HOLDs 174/175/192/199 (192/199 release via UNBLOCK-02), string
> freeze (D22 — UNBLOCK-03). The quarantine drain (D21) is not blocked: active
> `Compile Remove` exclusions are 0 (UNBLOCK-04 records the truth).

### 14.2 AGENTS.md — available queue

Replace the eight-item list's statuses with the verified ones and point the
five live ones at UNBLOCK-01..05; delete the four sealed items from the
"available" list and mention them under "Completed".

### 14.3 INTEGRATION_PLANS.md — current batch header

> **XP Expansion W1 — XP-01 COMPLETE (2026-09-19).** The
> `XP-WAVE1-DIFFICULTY-AUTHORITY` package delivered the canonical catalog,
> new-game selection, manifest persistence, 8 scalar consumers, and
> `--difficulty-selftest` 14/14. The next XP package is not yet claimed; XP-04
> and XP-06 are released by UNBLOCK-02 and UNBLOCK-01 respectively.

### 14.4 DECISION_REGISTER.md — DEC-01 / DEC-16

`DEC-01` → `SIGNED` with evidence: `duty_roles.json` ward role,
`MedicalWardSystem.StaffingPreflight`, `DEBT-PLAN24-MEDICAL-WARD-STAFFING`
RETIRED, `PLAN_24_CLOSEOUT.md`.

`DEC-16` → terminal verdict per §5.2 with evidence: option (ii) executed; no
ramp authority; `PLAN_24_CLOSEOUT.md`.

### 14.5 DECISION_REGISTER.md — DEC-09

Correct the condition to cite the item row's actual values (or the D3 outcome),
and link the D3 sign line.

### 14.6 UNCLAIMED_CORPUS_CENSUS.md — summary

Regenerated counts (mechanism per §5.3) plus:

> Summary generated from the table on <date>; tranche-2 audits run under
> UNBLOCK-04's protocol. Historical counts above are provenance.

### 14.7 UNBLOCKED_PLANS_AUDIT_2026-09-19.md — superseding notes

- quarantine: "the 48 count was stale; 0 active after the 2026-09-18
  reconciliation (verified 2026-09-21)".
- completed CF items: point at claim rows.

### 14.8 KNOWN_DEBT.md — Plan 32 residual row

Per §5.12; plus reviewed-date updates for touched rows.

### 14.9 TEST_POLICY.md — quarantine note

Add: "Active explicit quarantines: 0 (verified <date>). Any future explicit
`Compile Remove` must reference a real source file per
`QuarantineManifestGateTests`; restoration is a deliberate package."

---

## Appendix A — Premise commands (full)

```bash
# quarantine
grep -c 'Compile Remove' Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
grep -n 'Compile Remove' Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
grep -rn "QuarantineManifestGateTests" Ashfall.Core.Tests --include=*.cs | head

# register rows
grep -n "DEC-01\|DEC-03\|DEC-05\|DEC-09\|DEC-16\|DEC-17\|DEC-18" \
  docs/governance/DECISION_REGISTER.md

# XP status
grep -n "XP Expansion W1" -A 10 INTEGRATION_PLANS.md
grep -n "claim-xp-wave1" -A 2 WORKTREE_OWNERSHIP.md | head
ls Assets/StreamingAssets/Data/difficulty_presets.json

# AGENTS queue
sed -n '/ACTIVE QUEUE/,/decision-blocked/p' AGENTS.md

# census
grep -c "AUDIT-PENDING" docs/plans/UNCLAIMED_CORPUS_CENSUS.md
sed -n '152,165p' docs/plans/UNCLAIMED_CORPUS_CENSUS.md

# D3
grep -n "water_sample_contaminated" -A 12 Assets/StreamingAssets/Data/items.json

# D13
sed -n '1,8p' src/Main.Plans126_129.cs

# D16
grep -c "flooded\|amphibious" Assets/StreamingAssets/Data/wasteland_map_v1.json

# D19a
sed -n '236,246p' src/Main.Plans147.cs

# D19c
grep -n "_sharedSkillProgression\|_sharedFactionStance" src/Main.CampaignServices.cs
grep -n "ResetPlansExpansionSessions\|ResetEnrolledFlagshipSessions\|ResetAllSessionsInMemory" \
  src/Main.Lifecycle.cs
grep -n "SetupSilentFoundry" -A 4 src/Main.Economy.cs
grep -n "_silentFoundry" src/Main.ExpansionHub.cs | head

# Plan 24 residual
grep -n -i "snapshot\|residual" docs/plans/PLAN_24_CLOSEOUT.md | head

# Plan 32 residual
grep -n "aviation" docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md
grep -n "C2\[11\]" docs/plans/UNCLAIMED_CORPUS_CENSUS.md

# E1
head -30 docs/roadmap/e1/E1_BASELINE.md
tail -5 docs/roadmap/e1/E1B_IMPLEMENTATION_LOG.md
python3 scripts/ci/generate-plan-register.py --check
```

---

## Appendix B — File inventory proposal

**Micro-decisions:** `Assets/StreamingAssets/Data/items.json` (D3),
`src/Main.Plans126_129.cs` (D13), `src/Main.Plans147.cs` (D19a),
`DECISION_REGISTER.md` (D3/D4/D9/D13/D16/D19a).
**Ledger patches:** `AGENTS.md`, `INTEGRATION_PLANS.md`,
`UNCLAIMED_CORPUS_CENSUS.md`, `KNOWN_DEBT.md`, `TEST_POLICY.md`,
`docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md`,
`docs/plans/PLAN_24_CLOSEOUT.md`.
**D19c:** `src/Main.CampaignServices.cs`, `src/Main.Lifecycle.cs`,
`src/Main.Economy.cs` (if null assignment needed), one focused test file.
**Census/E1:** `scripts/ci/plan_*`, `docs/roadmap/PLAN_REGISTER.*`,
`docs/roadmap/e1/*` logs, census/audit logs.
**Generated:** docs index, architecture map if a selftest row changes (not
expected).

---

## Appendix C — Glossary

| Term | Meaning |
|---|---|
| truth row | a ledger statement false or unverifiable at HEAD |
| patch | the exact edit that makes a truth row true |
| reconciliation | zero-code alignment of evidence with a verdict |
| terminal verdict | SIGNED / DECLINED / DEFERRED-WITH-CONDITION / RETIRED |
| collision seal | incorrectly sealing a corpus plan from a filename match |
| tranche-2 | the per-clause corpus audit protocol |

---

## Appendix D — Cross-plan interface table

| Interface | This plan provides | Consumed by |
|---|---|---|
| quarantine truth | D21 | EN-08, TEST_POLICY, agents |
| register invariant | §5.2 | all decision-gated work |
| census freshness | tranche-2 + E1C | Plan 3's certifications, future heads |
| AGENTS/INTEGRATION truth | patch matrix | all agents |
| D3 catalog truth | micro-decision | Plan 1's catalog tranche |
| Plan 24 procedure | residual path | first renderer session |
| Plan 32 residual row | ownership | future travel packages |

---

## Appendix E — Re-verification checklist before signature

- [ ] `Compile Remove` count is still 2 comment-only matches.
- [ ] DEC-01/DEC-16 still read DEFERRED-WITH-CONDITION.
- [ ] INTEGRATION_PLANS still says XP W1 ACTIVE.
- [ ] AGENTS queue still lists the sealed items.
- [ ] Census summary still mismatches the table.
- [ ] `water_sample_contaminated` still `isEquipable: true`.
- [ ] `SeededRng(147)` still present.
- [ ] The three cached fields still lack reset handling.
- [ ] Plan 24 residual still open; Plan 32 residual still unowned.
- [ ] E1C still unstarted.

---

## Closing statement

The queue's biggest hidden cost is not an unimplemented feature; it is a
ledger that tells agents the wrong thing. This plan makes six micro-decisions,
repairs the truth of five governance documents, records one bounded defect
audit, declares the quarantine truth, authorizes the first sustained
generated-register programme, and gives two residuals owners. None of it is
large; all of it compounds.

Recommended first action: sign the cheap lines (D4/D13/D16/D19a/D21 truth),
apply the patch matrix, then decide D3 before Plan 1's catalog tranche and run
the D19c trace.

**End of UNBLOCK-04.** This document is a proposal to repair truth and release
blocked plans; it does not execute, claim, or certify any of them.---

## 15. The complete truth ledger — verified facts at HEAD `5be1a30a`

This table is the reference the patch matrix relies on. Each row names the
claim, the verification, and the truth. It is also a useful onboarding summary
for any agent returning to the workspace after a pause.

### 15.1 Blocked-and-released claims

| Claim | Verification | Truth |
|---|---|---|
| "Quarantine drain is blocked with 48 active exclusions" | `grep -c 'Compile Remove'` → 2 comment-only | FALSE; 0 active |
| "D21 needs a signature before EN-08" | debt reconciled; gate test present | the *truth declaration* is small; the drain itself is empty |
| "XP-04/XP-06 blocked on F13/F14" | no FundsLedger; no limb fields | TRUE; released by Plans 2 and 1 |
| "Plan 49 blocked on Plan 42/46" | both `AUDIT-PENDING` | TRUE; released by Plan 3's certifications |
| "D22 string freeze unsigned" | no freeze declaration | TRUE; released by Plan 3 |
| "C3 192/199 held" | C3 decision rows | TRUE; released by Plan 2's XP-08 signatures |
| "EN-01…EN-08 need authorization" | EN program doc | TRUE; Plan 5 signs the unblocked four |
| "E1C unstarted" | E1B log | TRUE; released by this plan |
| "Census tranche-2 unrun" | 85 AUDIT-PENDING | TRUE; released by this plan |
| "Plan 24 snapshot rebaseline environment-blocked" | closeout item 12 | TRUE; procedure released by this plan |

### 15.2 Completed-and-stale claims

| Claim | Verification | Truth |
|---|---|---|
| AGENTS lists CF-P1 seal as available | claim row DONE 2026-09-19 | STALE |
| AGENTS lists CF-P6 as available | claim row DONE | STALE |
| AGENTS lists CF-P28 as available | claim row DONE | STALE |
| AGENTS lists CF-XP01 as available | claim row DONE; commit `d13e9322` | STALE |
| AGENTS lists Plan 37 as available | claim row DONE; census SEALED | STALE |
| AGENTS lists Plan 48 as available | claim row DONE; census SEALED | STALE |
| INTEGRATION says XP W1 ACTIVE | claim DONE; source complete | STALE |
| Census summary 38/89 counts | table has many more sealed | STALE |
| Audit says 48 quarantine active | actual 0 | STALE |
| DEC-05 evidence 14/14 | corrected 6/6 | FIXED 2026-09-19 |
| DEC-01/DEC-16 deferred | resolved per Plan 24 closeout | STALE |
| DEC-09 cites rad protection 20 | row has 0 | STALE |

### 15.3 Open items (truthful)

| Item | State |
|---|---|
| D3 water sample | equipable true; lore rationale stale |
| D4 DEC-15 | already SIGNED; ratification optional |
| D13 header | still says 127–129 follow-up |
| D16 tags | 0 authored |
| D19a fallback | `SeededRng(147)` present |
| D19c cached fields | not reset; candidate defect |
| DEC-03 | deferred until F14 (Plan 1) |
| Plan 32 residual | unowned |
| 85 census rows | unaudited |
| E1C | unstarted |

### 15.4 Why this matters more than it looks

Every stale row above is a *plausible instruction*. An agent reading
"CF-XP01 available" may re-claim a finished package; an agent reading "48
active exclusions" may start a drain project; an agent reading "XP W1 ACTIVE"
may treat the batch as open when its head is complete. The cost of a stale
ledger is measured in wasted sessions, not lines of code. The patch matrix is
therefore not cosmetic work; it is queue throughput maintenance.

---

## Appendix F — Census tranche-2 batch worksheets

### F.1 Batch 1 — the Plan 42/46/49 chain (priority)

Rows to audit first (all named in the A1 audit and the census):

| Row | Subject | Known from A1 / current state | Expected classification |
|---|---|---|---|
| `C2[18]` | Plan 42 survivor voice | complete plan; prerequisites audit by Plan 3's L42-A | CERTIFIED → claimable / AMENDED |
| `C2[20]` | Plan 46 playable metrics | complete plan; decisions pending | CERTIFIED → claimable / AMENDED |
| `C2[17]` | identity | read model live; write path open | CERTIFIED-WITH-BOUNDARY (voice read) |
| `C2[16]` | Plan 39 session durability | numbering collision with orbital closeout | STALE or routed |
| `C1[16]` | Plan 49 | prerequisite chain | AUDIT-PENDING → promotion after 42/46 |
| `C2[19]` | Plan 44 relations | collision closeout | audit |
| `C2[22]` | Plan 50 asset truth | audit | audit |
| `C2[23]` | Plan 52 sound of scarcity | audit | audit |

### F.2 Batch 2 — the completed recent wave rows (truth confirmation)

The recent integrations (Plan 132, 138, 168, 200, 203, 205/220, 206/212/182,
207, crop roster, 133) already have claim/log evidence in
`WORKTREE_OWNERSHIP.md`. Tranche-2 confirms their census rows reflect the
seals and upgrades any remaining `AUDIT-PENDING` rows with claim-row evidence.

| Evidence | Use |
|---|---|
| claim rows in `WORKTREE_OWNERSHIP.md` (DONE statuses) | seal or reconcile |
| integration logs (`docs/plans/PLAN_*_INTEGRATION_LOG.md`) | evidence links |
| selftest counts recorded in claims | verification |

### F.3 Batch 3 — the unclaimed corpus remainder

Rows with no recent claim/log: audit against owners/reachability; classify
STALE / READY-UNCLAIMED / IMPLEMENT per §5.9.

### F.4 Batch log template

```markdown
# Census tranche-2 — batch N (<date>, HEAD <sha>)
Rows audited: <n>
| Row | Subject | Owner evidence | Reachability | Classification | Condition |
|---|---|---|---|---|---|
...
Notes / drift / collisions checked: ...
```

### F.5 Stop rule

Stop a batch immediately if: a row's evidence contradicts the census's own
dependency DAG, a collision is discovered, or an owner appears to be missing
without a named repair. Report; do not improvise.

---

## Appendix G — D19c trace worksheet

### G.1 Questions to answer with file:line evidence

1. What does the "New Campaign" button call? (Find the handler.)
2. Does it call `ResetAllSessionsInMemory`, reload the scene, or construct a
   new Main?
3. Which callbacks are registered in the `_lifecycleRegistry`, and in what
   order do they run?
4. After a reset, is `_silentFoundry` still non-null?
5. Does `EnsureSharedFactionStance` return the stale foundry's stance after a
   reset?
6. Does `EnsureSharedSkillProgression` return a stale skill set after a reset?

### G.2 Outcomes

| Outcome | Action |
|---|---|
| new instance per campaign | close D19c as no-defect; record the trace |
| reset callbacks are the only path, fields not nulled | repair |
| fields nulled elsewhere (not the two callbacks) | record where; no repair |

### G.3 Evidence template

```text
D19c trace — <date>, HEAD <sha>
New campaign path: <file:line> → ...
Reset callbacks: <list>
_silentFoundry after reset: <state>
_sharedFactionStance after reset: <state>
_sharedSkillProgression after reset: <state>
Outcome: <no-defect | confirmed>
Repair package (if any): <id>
```

---

## Appendix H — Proposed replacement text for the AGENTS.md queue

This is the full proposed block so the integrator can paste it after
verification. It is written to be short, current, and pointer-based (the
rulebook must not duplicate plan prose).

```markdown
## ACTIVE QUEUE — UNBLOCK PROGRAM (verified 2026-09-21)

Five unblock plans release the remaining blocked/partial work. Each is a
proposal until its signatures are recorded:

1. `UNBLOCK-01_BODY-INTEGRITY_SCHEMA_F14_XP06.md` — F14 schema; releases
   XP-06, EN-04, Expansion 16's prosthetic half.
2. `UNBLOCK-02_FUNDS_TRADE_F13_XP04_XP08.md` — FundsLedger + trade legs +
   route/migration contracts; releases XP-04, XP-08, EN-03, C3 192/199.
3. `UNBLOCK-03_SEMANTIC_VOICE_STRING_FREEZE_D11_D22_PLAN424649.md` — D11
   routing, D22 freeze, Plan 42/46 certifications, Plan 49 packet, EN-05.
4. `UNBLOCK-04_LEDGER_REGISTER_CENSUS_QUARANTINE_TRUTH.md` — micro-decisions,
   register invariant, quarantine truth, census tranche-2, E1C, residuals.
5. `UNBLOCK-05_EXPANSION_WAVES_C3_EN_GATE.md` — newest expansion waves intake,
   C3/EN authorization gate, XP-07/09/10 premise checks.

Executed and closed (do not re-claim): CF-P1 distress content seal, CF-P5
restock reconcile (Plan 2 F13-B), CF-P6 vehicle armor, CF-P28 bootstrap,
CF-XP01 difficulty binding, E1A/E1B, Plan 37 input, Plan 48 release craft,
Plan 168/203/138/207/132/220/205/200/212/206/182 integrations, crop roster
phases, Plan 133 communique, partial waves 5/6, and the C2[9]–C2[13] anchors.

Quarantine: active explicit `Compile Remove` exclusions = 0 (verified
2026-09-21); restoration of any historical source is a deliberate package.
```

The integrator must re-verify each "executed and closed" claim against the
claim/debt ledger at patch time; the list above is accurate at HEAD
`5be1a30a`, not eternally.

---

## Appendix I — Residual inventory (what remains open after Plan 4)

If every signature in this plan lands, the following residual realities remain
— none of them decision-blocked, some environment-gated:

| Residual | Nature | Owner trigger |
|---|---|---|
| Plan 24 snapshot rebaseline | environment (renderer) | first renderer session; procedure ready |
| Plan 32 aviation/naval travel | product/consumer gap | a proposed aviation/naval travel package |
| Unrestored historical quarantined tests | deliberate package if wanted | a named source + focus proof |
| 85 census rows (partially drained) | audit work | tranche-2 batches continue |
| E1D–E1P | governance programme | E1C completion |
| `water_sample` migration tolerance (if Option A) | test coverage | Plan 4 A2 |
| `_silentFoundry` etc. repair (if confirmed) | bounded code | Plan 4 C |
| XP-09/XP-10 premise vs DEC-17/18 | premise checks | Plan 5 |
| Play metrics settings default | product call | Plan 3 L46-A |
| VO recording decision | product call | Plan 3 D22-B |

This inventory is itself a truth artifact: after Plan 4, "what is left?" has a
short, honest answer.

---

## Appendix J — Verification log template for the closeout

```markdown
# UNBLOCK-04 verification log — <date>, HEAD <sha>

## Patches applied
| File | Statement | Corrected | Evidence command | Result |
|---|---|---|---|---|

## Micro-decisions
| Item | Sign-off | Applied? | Verification |
|---|---|---|---|

## D19c
Outcome: <no-defect|confirmed|repaired>. Evidence: <commands>.

## Census
Batches run: <n>. Rows classified: <n>. Summary regenerated: <yes/no>.

## E1C
Generator stable across runs: <yes/no>. Register published: <path>.

## Residuals
| Residual | State |
|---|---|

## Limitations
<what was not verified>
```

---

## Appendix K — Why five plans and not one

A reader may ask why this truth-repair work is a separate plan rather than a
section of the other four. The answer is the file-ownership rule and the
reviewing burden:

1. The other four plans need these truths to be signed *first*; bundling them
   would block the queue behind bookkeeping.
2. This plan's writers are integrator-only paths (`AGENTS.md`,
   `INTEGRATION_PLANS.md`, the register); the other plans are builder paths.
   Mixing them in one claim encourages races.
3. The census/E1C work has a different acceptance rhythm (batches, generated
   checks) than a schema or economy package.
4. The foreman may sign truth repairs without signing any product decision —
   keeping them separable preserves that freedom.

**End of UNBLOCK-04 appendices.**---

## 16. Truth-maintenance runbook

This runbook is the durable part of Plan 4: even if only the cheap signatures
land, this procedure keeps the ledgers from drifting again. It is written for
the integrator and for any wave-closeout session.

### 16.1 The closeout re-verification loop

Run at the end of every execution wave, before the batch is marked ACCEPTED:

1. **Claim ledger sweep.** For every claim row marked DONE/HANDED_OFF in that
   wave, verify: the claim's evidence lines still hold; the paths named are
   still the owners; no follow-up row was silently orphaned.
2. **Debt sweep.** For every debt row touched or referenced by the wave,
   verify its status matches reality (RETIRED means the evidence exists;
   QUARANTINED means the manifest gate passes).
3. **Register sweep.** For every decision row whose condition names a package
   executed in the wave, verify the condition is met and move the verdict to
   terminal.
4. **Queue handoff sweep.** For every item removed from the available queue,
   verify the sealing evidence; for every item added, verify its premise.
5. **Count sweep.** Verify every count that appears in a ledger (quarantine
   entries, section counts, catalog counts, test counts) with a fresh command.
   Counts copied from previous documents are the most common drift source.

The loop is cheap (an hour) and prevents the exact staleness Plan 4 documents.

### 16.2 Ledger statement hygiene rules

1. **No statement without a command.** Every quantitative claim in a ledger
   must be reproducible by a command recorded beside it or in the row's
   evidence field.
2. **No "still deferred" without a live condition.** A deferred row must name a
   package and a trigger that is still unmet; if the package is done, the row
   moves.
3. **No "available" without a premise check.** An item enters the available
   queue only with a premise note; otherwise it is "candidate, premise
   unverified".
4. **No prose duplication across ledgers.** Reference, don't copy. Copies
   drift (the audit's own §7 findings are all copy-drift).
5. **Superseding notes, not rewrites.** When history becomes false, append a
   dated superseding note; do not silently rewrite the historical statement.
   This preserves the audit trail and prevents "the document always said so"
   confusion.

### 16.3 The staleness taxonomy (what to look for)

| Type | Example from this plan | Detection |
|---|---|---|
| Count drift | "48 quarantine entries" vs. actual 0 | grep + count |
| Status drift | "XP W1 ACTIVE" vs. claim DONE | claim row read |
| Evidence drift | DEC-05 14/14 vs. 6/6 | test run |
| Condition drift | DEC-01 deferred though resolved | closeout read |
| Fact drift | DEC-09 rad protection 20 vs. row 0 | data read |
| Queue drift | Plan 37/48 listed available though sealed | census/claim read |
| Summary drift | census summary 38/89 vs. table | generated count |

### 16.4 The command log pattern

Each ledger patch should leave a one-line command log in the implementation
log, e.g.:

```text
[date] quarantine truth: grep -c 'Compile Remove' → 2 (comment-only); AGENTS line removed.
[date] register: DEC-01 → SIGNED; evidence: PLAN_24_CLOSEOUT.md + debt row RETIRED.
[date] census: summary regenerated; AUDIT-PENDING=85 (was 89 in summary).
```

This log is what makes the next closeout cheap.

### 16.5 Escalation

If a sweep finds a contradiction it cannot resolve with current evidence, the
correct action is a bounded audit (like D19c) or a foreman question — never a
guess. Rule 10 applies to ledgers as much as to code.

### 16.6 Success criteria for the runbook

- After the next wave, the same five sweeps find zero new staleness.
- A new agent can read the queue and know, without cross-checking, what is done,
  what is blocked, and what is next.
- Every "blocked" item names a signature; every "available" item names a
  premise check.

---

## 17. D3 and D19c test-guidance detail

### 17.1 D3 Option A — restore-tolerance test design

If the item loses equipability and a legacy save may hold it equipped:

**Fixture:** a hand-authored save (or fixture object) with
`water_sample_contaminated` equipped in the body slot.

**Test 1 — load tolerance:** restore the save; assert the load does not throw,
does not mutate the equipped set, and the item remains visible in the
inspection/slot read. This proves the save codec tolerates a definition change.

**Test 2 — first-interaction behavior:** perform an unequip or equip action;
assert the unequip succeeds and the item returns to inventory. This proves the
game moves on rather than wedging.

**Test 3 — data-integrity:** with Option A applied, the validator reports no
error for the row; if a "slot on non-equipable item" rule exists, the edited
row must not trigger it.

**Test 4 — adjacent suite:** the Plan 21 protective-wear suite stays green;
if it enumerates equipables, its count pin changes in the same package with a
drift note.

### 17.2 D19c repair test design

**Test A — reset hygiene:** construct a host with a non-default foundry/stance/
skill state; invoke the same reset path a new campaign uses; assert the three
fields are null/pending when the next setup runs. If the trace shows a new
instance per campaign, this test becomes a trace assertion instead.

**Test B — reload parity:** after the repair, a save/load cycle produces
identical foundry/stance/skill state to a continuous run (the standard paired
replay shape).

**Test C — panel safety:** open the foundry panel, reset, re-open; assert no
stale binding or double-subscription (the panel-bind-lifecycle pattern; a ×100
reopen assertion if the harness supports it).

### 17.3 D19a test design

**Allowed:** two consecutive previews with no campaign day produce identical
results (seed 147). **Not constructible:** the barter surface returns a typed
refusal (or is unavailable) pre-campaign; a test asserts the refusal.

### 17.4 D13/D16 verification

D13 is a comment change: no test beyond the census note. D16 is a decision: if
tags are declined, no data changes and the zero-tag command is the evidence;
if authored later, the map tranche provides its own route-validation tests.

---

## Appendix L — Five-signature minimum for Plan 4

If the foreman wants the smallest possible action to restore truth:

1. `Quarantine drain: record the reconciled empty truth.`
2. `I authorize the ledger-truth patch matrix in §4.9.`
3. `Plans 126-129: (a) numbering drift — fix the comment.`
4. `water_sample_contaminated: Option B retain as lore` (with the DEC-09
   correction) **or** Option A.
5. `E1C corpus metadata migration is authorized as the next bounded E1 phase.`

These five close the largest drifts and start the mechanism that prevents
recurrence. The rest (D16/D19a/D19c/D21-restoration/L-CENSUS2/L-P24R/L-P32R)
can follow.

**End of UNBLOCK-04 top-up appendices.**