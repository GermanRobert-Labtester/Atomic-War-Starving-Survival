# ASHFALL — WAVE 4 INTEGRATION PROGRAM · PLAN 5 OF 6

# FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W4 (six-plan integration wave — the shelter's remaining machinery)
**Document:** W4-05
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W4-01 (save/state), W4-02 (world/travel), W4-03 (infrastructure), W4-04 (ecology), W4-06 (medicine)
**Plan-unblocking annex:** Annex U at the end — separately.

---

## 0. How to read this plan

This plan integrates **the polity**: how the shelter stands with factions, how
treaties are made and kept, how reputation and legitimacy accrue, how internal
branches and external warlords press on decisions, how information and rumor move,
how the census counts people, and how justice and governance are exercised. It
extends existing owners — one standing authority, one treaty authority, one
reputation authority, one census authority — and it never builds a second
reputation store, treaty ledger, or parallel politics simulation.

### 0.1 Two selection levels

| Level | Choice | Granularity |
|---|---|---|
| **Level 1** | Plan Path **A**, **B**, or **C** | the whole plan's posture |
| **Level 2** | ten decision points, each **A/B/C** | per-concern depth |

### 0.2 Default mapping

| Plan Path | A points | B points | C points |
|---|---|---|---|
| A Truth & Voice | 1–10 | — | — |
| B One Polity | 1,4 | 2,3,5,6 | 7,8,9,10 |
| C Legitimacy Over Time | — | 2,4,5 | 1,3,6,7,8,9,10 |

### 0.3 The Wave 4 rule for this plan

> **One standing truth, one treaty ledger, one census, one justice path.**
> `FactionStandingIdResolver` resolves who is who; `RegionalTreatySystem` +
> `DiplomaticTreatyCatalog` own agreements; `CensusClaimSystem` +
> `VoluntaryRegisterSystem` own who lives here; `Verdict` owns judgment.
> No parallel standing, no second tally of people, no quiet punishment system.

### 0.4 Vocabulary

| Term | Meaning |
|---|---|
| standing | the recorded disposition of one faction toward the shelter |
| stance band | an authored range of standing with named behavior |
| treaty | a persistent agreement with terms, signature, duration, and effects |
| summit | a structured diplomatic meeting with agenda and outcomes |
| branch | an internal tendency (independent, military, rebel) with its own state |
| legitimacy | the shelter's internally recognized right to decide |
| census | the authoritative count and identity of residents |
| claim | an assertion of membership/status requiring resolution |
| verdict | the outcome of judgment with recorded reasoning |
| information route | how facts and rumor reach people (radio, print, word) |

---

## 1. Premise audit — what P0 must verify (Rule 7)

```text
[ ] Assets/Ashfall.Core/Factions/: FactionStandingIdResolver, FactionBranchCoordinator,
    FactionDisplayNameCatalog, FactionIntelligenceCatalog, FactionBountySystem,
    IndependentBranch* (Catalog/Ids/Save/State/System), MilitaryBranch* (+Save/State),
    RebelBranch* (+Save/State), Prpf* (Ids/Save/StandingSystem/State),
    WeightOfChoicesSave, ForcedLaborSystem, PrisonerSystem, ShelterEspionageSystem
[ ] Assets/Ashfall.Core/Diplomacy/: DiplomaticSummitSystem, DiplomaticTreatyCatalog
[ ] RegionalTreatySystem + RegionalTreatyCatalogLoader + RegionalTreatyFeed
[ ] Governance/ folder; Institutions/ folder; Reputation/ folder; StandingRecord/;
    Propaganda/ folder; InformationFlow/ folder; Warlords/ folder; Verdict/ folder
[ ] CensusClaimSystem (+ headless demo), VoluntaryRegisterSystem,
    VouchAccessSystem, ContractorRosterSystem (if people-counting)
[ ] FactionWarSystem + FactionTerritoryCatalog + PatrolTerritoryAuthority
    (coordinate with W3-04 war escalation; do not duplicate)
[ ] host partials that tick factions/diplomacy/governance
[ ] data: faction/treaty/standing/census catalogs under Assets/StreamingAssets/Data/
[ ] existing rows: PLAN_25_FACTION_ECOLOGY, PLAN_207_SHELTER_REPUTATION,
    PLAN_123_REBEL_BRANCH, PLAN_132_HIDDEN_AGENDA, Plan 30 war projection
```

### 1.1 Evidence posture

- Proposal only; no path claims; no ledger rows; read-only until Annex U.
- Core engine-free; JSON authoritative; determinism through existing seeded paths.
- One authority per concern; additive fields only; no parallel politics.
- Focused verification per `TEST_POLICY.md`.

### 1.2 Known anchors (verify, don't trust)

| Anchor | Why it matters |
|---|---|
| `FactionStandingIdResolver` | the identity door for standings; bypass = drift |
| branch `*Save.cs` owners | each branch already persists its own state |
| `RegionalTreatyFeed` | treaties already flow through a feed seam |
| `CensusClaimSystem` | claims/identity already modeled; do not re-count |
| `W3-04` security plan | espionage/CI/prisoners owned there — coordinate, never duplicate |
| `Propaganda/` + `InformationFlow/` | information routes exist; do not add a third |

---

## 2. The three Plan Paths

### 2.1 Path A — Truth & Voice

Audit every faction, treaty, standing, branch, and information route: what exists in
data, what is consumed, what is inert. Produce the polity ledger; repair orphans and
silent inconsistencies only.

### 2.2 Path B — One Polity

Unify: one standing authority, one treaty lifecycle, one reputation legibility, one
census — consumed by decisions, surfaces, and consequences without local
bookkeeping.

### 2.3 Path C — Legitimacy Over Time

Make politics historical: treaties that decay and are remembered, legitimacy that is
earned and lost through choices, branches that evolve, and a record of governance the
player can read across years — deterministic and save-backed via W4-01.

---

## 3. The ten decision points

### 3.1 Point 1 — Faction inventory and stances

**Owner anchor:** `FactionStandingIdResolver`, `FactionDisplayNameCatalog`, branch
systems.
**Decision:** every faction has one identity, one display name, one standing record
with authored bands; no orphan display keys, no duplicate stances.

- **A:** enumerate factions vs data vs displays; list unresolved IDs and dead factions.
- **B:** identity discipline: new factions register through the resolver; bands authored
  and consumed by behavior.
- **C:** faction evolution across years (rise, collapse, absorption) on existing owners.

**Verify:** ID resolution coverage; band boundary tests; display coverage.
**Never:** a hardcoded faction name in logic, or a second standing table.

### 3.2 Point 2 — Treaty lifecycle

**Owner anchor:** `RegionalTreatySystem`, `DiplomaticTreatyCatalog`,
`DiplomaticSummitSystem`, `RegionalTreatyFeed`.
**Decision:** treaties are persistent and honest: terms declared, signatures recorded,
durations ticked, breaches detected, effects consumed once.

- **A:** audit treaty data vs live effects; find unenforced terms and dead catalog rows.
- **B:** one lifecycle: propose → summit → sign → active → decay/breach → recorded;
  every effect keyed once.
- **C:** multi-year diplomacy: renewals, stacked obligations, and remembered history
  that constrains future options.

**Verify:** lifecycle state tests; breach detection; effect-once; save round-trip.
**Never:** a treaty that exists in data but nowhere in play, or effects applied twice.

### 3.3 Point 3 — Diplomatic actions and consequences

**Owner anchor:** `DiplomaticSummitSystem` + consequence routing (W3-01 ledger family).
**Decision:** diplomacy is action and answer: demands, offers, ultimatums, and
concessions with warned costs and exactly-once consequences.

- **A:** audit the action surface: what can be said/done today and what silently does
  nothing.
- **B:** one consequence routing path (flag/consequence ledger, W3-01) with once-keys;
  warnings before commitment.
- **C:** relationship arcs: actions compound into alliance/hatred histories.

**Verify:** action→consequence mapping; once-key checks; warning-before-commit.
**Never:** a diplomatic button that changes nothing, or consequence duplication.

### 3.4 Point 4 — Reputation and standing aggregation

**Owner anchor:** `Reputation/`, `StandingRecord/`, `PrpfStandingSystem`.
**Decision:** reputation has one aggregation truth with visible causes: deeds move
standing through declared weights; players can see why.

- **A:** audit every writer to standing; find double-counting and hidden modifiers.
- **B:** one aggregation per faction; sources recorded (deed, treaty, broadcast, rumor)
  with decay rules.
- **C:** reputation memory: old deeds echo with authored decay and atonement paths.

**Verify:** writer inventory; aggregation determinism; cause-visibility test.
**Never:** a hidden standing delta, or a second reputation store.

### 3.5 Point 5 — Branch politics

**Owner anchor:** Independent/Military/Rebel branch systems, `FactionBranchCoordinator`,
`WeightOfChoicesSave`, `ForcedLaborSystem`, `Prpf*`.
**Decision:** internal politics is stateful and consequential: branch influence grows
and shrinks through choices, coordinators arbitrate, and coercion (forced labor) stays
visible and morally weighted.

- **A:** audit branch data/consumers; find inert branches and unread states.
- **B:** one coordinator resolving branch effects; branch saves round-trip; UI reads
  branch state only.
- **C:** arcs: branch dominance, splits, and reconciliations across the campaign.

**Verify:** coordinator dispatch; branch save round-trip; influence deltas.
**Never:** a second branch state store, or hidden coercion effects.

### 3.6 Point 6 — Warlords and threat relations

**Owner anchor:** `Warlords/`, `FactionWarSystem` (coordinate W3-04).
**Decision:** warlord relations are explicit: demands, tribute, raids, and negotiated
pauses — with the war escalation ladder owned by W3-04, never re-implemented here.

- **A:** audit warlord data/consumers and their relationship to the war system.
- **B:** one relation state per warlord; demands warned and answerable; raids route
  through W3-04 stage machinery.
- **C:** long arcs: warlord succession, vendettas, and peace becoming possible.

**Verify:** demand lifecycle; raid routing to war stages; relation round-trip.
**Never:** a second war ladder, or warlord effects outside warned channels.

### 3.7 Point 7 — Information, rumor, and propaganda

**Owner anchor:** `Propaganda/`, `InformationFlow/`, radio/print/news systems (W3-01/W4-06
coordination), `FactionIntelligenceCatalog`.
**Decision:** what people know is modeled: facts, distortions, and rumors with
provenance and bounded spread; propaganda affects morale and standing through existing
owners and never manufactures consent as truth.

- **A:** audit routes (radio, print, word, leaflets); find duplicated news paths.
- **B:** one information item model with provenance (true/false/rumored) and spread
  bounds; consequences route to morale/standing once.
- **C:** media arcs: trust in sources, corrections celebrated (per expansion ethics),
  and long-run credibility.

**Verify:** provenance model; spread bounds; consequence-once; credibility arcs.
**Never:** a second news system, or a lie indistinguishable from truth in the model.

### 3.8 Point 8 — Census, claims, and identity

**Owner anchor:** `CensusClaimSystem`, `VoluntaryRegisterSystem`, `VouchAccessSystem`.
**Decision:** one count of who lives here: claims resolved with reasoning, register
voluntary and honest, access vouched — no double counting, no invisible people.

- **A:** audit count consumers (rations, labor, housing, surfaces); find divergent totals.
- **B:** one census read-model consumed everywhere; claims with recorded resolutions;
  register mutations through the owner.
- **C:** identity over generations: birth/death/arrival/departure records maintained
  (coordinate W3-01/UNBLOCK census rules).

**Verify:** census consistency across consumers; claim lifecycle; register round-trip.
**Never:** a second count of residents, or a claim resolved silently.

### 3.9 Point 9 — Justice and verdicts

**Owner anchor:** `Verdict/`, `PrisonerSystem` (coordinate W3-04), `ForcedLaborSystem`.
**Decision:** judgment is recorded and reasoned: offenses, hearings, verdicts, and
sentences with dignity rules and visible reasons; coercion never rewarded.

- **A:** audit verdict data/paths; find offenses without verdicts and verdicts without
  effects.
- **B:** one verdict lifecycle; sentences route to existing owners (labor, confinement,
  exile); records kept.
- **C:** justice culture across years: precedents, mercy, and institutional memory.

**Verify:** verdict lifecycle; sentence routing; record round-trip; tone review.
**Never:** a punishment with no record, or coercive reward loops.

### 3.10 Point 10 — Governance and surfaces

**Owner anchor:** Governance/ + Institutions/ + legitimacy state; polity surfaces
(W3-06 coordination).
**Decision:** governance is decision and legitimacy: councils, decrees, appointments,
and votes with visible standing inputs and consequences — surfaces tell the truth.

- **A:** audit governance data/consumers; find decisions that do nothing and states
  never shown.
- **B:** one decision path; legitimacy derived from declared inputs; surfaces read
  state, show why, and route actions.
- **C:** political history: legitimacy arcs, institution building, and remembered
  decisions.

**Verify:** decision lifecycle; legitimacy derivation; W3-06 kits over polity surfaces.
**Never:** a decree with no effect, or legitimacy computed in UI.

---

## 4. Selection sheet

```text
ASHFALL WAVE 4 · PLAN W4-05 · SELECTION SHEET

Plan Path:   [ ] A Truth & Voice   [ ] B One Polity   [ ] C Legitimacy Over Time

Points (mark A/B/C or leave default):
 1 faction inventory ....... [ ]
 2 treaty lifecycle ........ [ ]
 3 diplomatic consequences . [ ]
 4 reputation aggregation .. [ ]
 5 branch politics ......... [ ]
 6 warlords ................ [ ]
 7 information/propaganda .. [ ]
 8 census/claims ........... [ ]
 9 justice/verdicts ........ [ ]
10 governance/surfaces ..... [ ]

Selected by: ____________   Date: ________   Foreman: ____________
```

---

## 5. Phase ladder

| Phase | Name | Exit |
|---|---|---|
| P0 | Premise audit + polity ledger | ledger filed; premises re-verified |
| P1 | Standing + treaty discipline | resolution; lifecycle; effect-once green |
| P2 | Reputation + branches | aggregation determinism; branch round-trip green |
| P3 | Warlords + information | demand/raid routing; provenance/spread green |
| P4 | Census + justice | census consistency; verdict lifecycle green |
| P5 | Governance + surfaces | decision path; W3-06 kits; records green |
| P6 | Closeout | evidence pack; determinism; limitations recorded |

---

## 6. Non-goals, never-touch, one-authority

**Non-goals**

- No second standing/reputation/treaty/census store.
- No war-ladder duplication (W3-04 owns escalation/occupation).
- No espionage/CI duplication (W3-04 owns missions/heat).
- No real-world politics content; fictional factions only.

**Never-touch**

- Branch `*Save.cs` contracts without migration discipline (W4-01).
- PLAN_25 / PLAN_207 / PLAN_123 / PLAN_132 closings and their seams.
- Plan 30 war projection/clock ownership.
- Sealed debt rows; quarantined tests.

**One authority per concern**

| Concern | Owner |
|---|---|
| faction identity | `FactionStandingIdResolver` + display catalog |
| treaties | `RegionalTreatySystem` + catalog + summit |
| standing/reputation | standing record + aggregation owners |
| branches | branch systems + `FactionBranchCoordinator` |
| information | `Propaganda/` + `InformationFlow/` |
| census | `CensusClaimSystem` + `VoluntaryRegisterSystem` |
| justice | `Verdict/` |
| governance | Governance/ + Institutions/ |

---

## 7. Verification and acceptance

- **T1 static:** polity ledger; writer inventory to standing; duplicate-store scans;
  surface-source audit.
- **T2 focused:** ID coverage; band boundaries; treaty lifecycle/breach/effect-once;
  summit consequences; aggregation determinism; branch round-trip; warlord routing;
  provenance/spread; census consistency; verdict lifecycle; governance decisions;
  W3-06 kits.
- **T3 soak:** 60-day polity at seed: standing trends, treaty states, census counts,
  zero drift.
- **Acceptance:** evidence pack + Annex U signature; compile-green is not acceptance.

## 8. Handoffs and dependencies

| Direction | Detail |
|---|---|
| W3-01 | consequence ledger once-keys; narrative arcs of politics |
| W3-02 | trade/treaty economics, embargoes, caravan politics |
| W3-03 | morale effects of legitimacy, rumor, justice |
| W3-04 | war ladder, espionage, prisoners — coordinate, never duplicate |
| W4-01 | every new polity state ships its save section |
| W4-06 | health/psych consequences of confinement/coercion |
| W2-02 | silent-failure rules for political actions |

---

# ANNEX U — PLAN-UNBLOCKING (SEPARATELY)

## U.1 What this plan releases

| Release | Unblocks |
|---|---|
| U1 | polity ledger work blocked on "which standing writer is canonical" |
| U2 | treaty effect work held for feed-seam verification |
| U3 | census consumer unification blocked on UNBLOCK-04 truth rules |
| U4 | branch arcs blocked on save round-trip evidence |
| U5 | governance/justice surfaces blocked on W3-06 registry |

## U.2 Signature block

```text
ASHFALL WAVE 4 · PLAN W4-05 · RELEASE SIGNATURE
HEAD: ________  Date: ________
[ ] P0 premise audit completed and filed
[ ] polity ledger exists; duplicate writers listed
[ ] no path claimed outside the package
[ ] focused test targets named
[ ] rollback position recorded
Signed: ________   Foreman: ________
```

## U.3 Never-touches

- No edit to another Wave 4 plan's claimed paths.
- No re-open of PLAN_25/207/123/132 or Plan 30 closings.
- No change to W3-04 espionage/war contracts.
- No revival of quarantined tests outside the documented procedure.

## U.4 Release rule

> This plan executes only after U.2 is signed. Until then it is read-only planning.

---

*End of W4-05 — Part I. Expansion parts continue on the established Wave 3 pattern.*---

# W4-05 · PART II — DEEP DESIGN: FACTIONS, TREATIES, CONSEQUENCES (POINTS 1–3)

## II.1 The polity ledger: schema and meaning

One row per political concern: factions, treaties, information, census, justice.

```yaml
polity_ledger:
  - id: factions.stances
    owner: "FactionStandingIdResolver + branch systems + standing record"
    state: ["stance_values", "band_cursors", "deed_history_summary"]
    consumers: ["diplomacy", "war (W3-04)", "trade (W3-02)", "surfaces"]
    save_section: "politics.standing"
    surfaces: ["faction board", "diplomacy table"]
    warnings: ["stance_band_change", "treaty_breach", "warlord_demand"]
    tests: ["StanceRoundTripTests", "BandBoundaryTests"]
  - id: treaties.ledger
    owner: "RegionalTreatySystem + DiplomaticTreatyCatalog + SummitSystem"
    ...
```

### II.1.1 Ledger rules

```text
L1  one owner per political concern; no shadow standings
L2  every stance write names its source deed/treaty/broadcast/rumor
L3  every effect applies once, keyed (W3-01 consequence ledger)
L4  every warning precedes its consequence with an authored window
L5  every save section obeys W4-01 budgets and bounds
L6  every surface reads; no panel computes standing
```

### II.1.2 The single-writer law

Standing has one writer per faction: the aggregation owner. Branch systems,
treaties, and broadcasts submit *deeds*; only the aggregator moves the values.
A second writer is a stop-the-line finding.

## II.2 Faction inventory and stances

### II.2.1 Stance record

```jsonc
{
  "faction": "f_ash",
  "stance": 34,                  // bounded [-100, 100]
  "band": "wary",                // hostile | wary | neutral | cordial | allied
  "sources": [ { "kind": "deed", "id": "d_1102", "day": 212 } ],
  "decay_cursor": 212
}
```

### II.2.2 Rules

```text
F1  every faction has one identity (resolver) and one display name
F2  stances are bounded; bands are authored thresholds
F3  deeds record sources; decay is authored and deterministic
F4  band changes warn (early) and are announced in restrained copy
F5  no hardcoded faction names in logic; ids resolve through the catalog
F6  display coverage is complete; missing names are build failures
```

### II.2.3 The band table (authored; verified at P0)

| Band | Range | Behavior |
|---|---|---|
| hostile | −100..−60 | raids, refusals, closed trade |
| wary | −59..−20 | tolls, suspicion, limited trade |
| neutral | −19..20 | standard trade, cautious word |
| cordial | 21..60 | discounts, safe passage, information |
| allied | 61..100 | shared routes, defense pacts, aid |

## II.3 Treaty lifecycle

### II.3.1 Treaty record

```jsonc
{
  "id": "t_003",
  "party": "f_ash",
  "terms": ["grain_share", "safe_passage"],
  "signed_day": 140,
  "duration": 120,
  "state": "active",             // proposed | signed | active | breached | expired
  "breaches": [],
  "effects_applied": ["signing_bonus"]
}
```

### II.3.2 Rules

```text
T1  one lifecycle: propose -> summit -> sign -> active -> decay/breach -> record
T2  terms are identifiers with consumers; unconsumed terms are findings
T3  effects apply exactly once (keys); expiry derives from day+duration
T4  breaches detect from declared conditions; response escalates by ladder
T5  renewals and stacks are authored; no silent extensions
T6  everything is saved (W4-01) and survives migrations
```

### II.3.3 The breach ladder

```text
warning -> protest -> sanction -> revocation -> (W3-04 consequence escalation)
each rung: copy + window; the ladder is authored, not improvised
```

## II.4 Diplomatic actions and consequences

```text
A1  demands, offers, ultimatums, concessions — each with declared costs
A2  one consequence route: deed submitted to the aggregator, effect keyed
A3  commitment warnings before any irreversible action
A4  refusals explain (reason refs), never silence
A5  summits consume time/standing/leverage from owners
A6  history records decisions (summary), not replays
```

## II.5 Worked example: the bonus that paid twice

**Report:** signing a treaty granted its standing bonus again after a load.

**Walk:**

```text
1. root: signing effect reapplied by restore "ensure" logic
2. repair: effects apply at event time only; restore reads (W4-01 read/act);
   once-key verified on load
3. verify: load-between test; effect-once scenario
```

**Findings:**

| ID | Class | Repair |
|---|---|---|
| PL-01 | effect reapplied on restore | design |
| PL-02 | no load-between test | coverage |

*End of Part II. Continues in Part III (reputation, branches, warlords).*---

# W4-05 · PART III — DEEP DESIGN: REPUTATION, BRANCHES, WARLORDS (POINTS 4–6)

## III.1 Reputation and standing aggregation

### III.1.1 The aggregation model

```text
sources: deeds (aid, betrayal, trade, battle), treaties, broadcasts, rumor
weights: authored per source kind and faction
decay: authored per band (hostile memories fade slowly; alliances warm)
atonement: authored paths that restore (aid, restitution, time)
```

### III.1.2 Rules

```text
R1  one aggregation per faction; single writer (II.1.2)
R2  every delta records source + day; history is summarized, not replayed
R3  causes are visible: the player can ask "why is this faction wary?"
R4  decay is deterministic; no wall-clock; campaign day only
R5  atonement paths exist for every band regress; warned before harm
R6  no hidden modifiers; every weight is authored data
```

### III.1.3 The cause read model

```text
stance_explain(faction) -> ranked sources with days and weights
surface shows top three; full list in the record
"why" must never be a mystery; hidden reputation is a defect
```

## III.2 Branch politics

### III.2.1 Branch record

```jsonc
{
  "branch": "rebel",
  "influence": 42,               // bounded
  "state": "restless",           // quiet | vocal | restless | ascendant
  "alignments": { "f_ash": -10, "independent": 22 },
  "decisions": [ { "id": "d_44", "day": 210, "kind": "petition" } ]
}
```

### III.2.2 Rules

```text
B1  one coordinator (`FactionBranchCoordinator`) arbitrates branch effects
B2  influence moves through authored decisions/deeds, bounded; states authored
B3  branches petition, threaten, and split via declared paths with warnings
B4  forced labor (if present) stays visible, costly, and morally weighted
B5  branch saves round-trip; no second branch state store
B6  reconciliation paths exist; dominance is not a dead end
```

## III.3 Warlords and threat relations

### III.3.1 Relation record

```jsonc
{
  "warlord": "w_kestrel",
  "relation": "grudging",        // hostile | grudging | transactional | respectful
  "demands": [ { "kind": "tribute", "due_day": 240, "answered": false } ],
  "history": [ { "kind": "raid", "day": 180, "outcome": "repelled" } ]
}
```

### III.3.2 Rules

```text
W1  one relation per warlord; demands warned and answerable
W2  raids route through W3-04 war machinery; never re-implemented here
W3  tribute, parley, and defiance are authored options with costs
W4  vendettas and successions are long arcs on existing owners
W5  peace is possible: authored de-escalation paths
W6  no silent warlord effects; every consequence warned
```

### III.3.3 The boundary with W3-04

```text
W3-04 owns: stages, raids, occupation, combat, prisoners, heat
W4-05 owns: relations, demands, tribute terms, parley, memory of conduct
the seam: W4-05 submits intents; W3-04 resolves war; both record once
```

## III.4 Worked example: the war that forgot its cause

**Report:** a faction turned hostile with no visible reason.

**Walk:**

```text
1. root: a battle outcome wrote standing directly, bypassing the aggregator,
   with no source recorded
2. repair: battle outcomes submit deeds; aggregator records source; cause read
   model shows it; the bypass is a stop-the-line finding
3. verify: cause-visibility test; single-writer scan
```

**Findings:**

| ID | Class | Repair |
|---|---|---|
| PL-03 | standing bypassed aggregator | Rule 5 |
| PL-04 | no cause recorded | truth |
| PL-05 | no single-writer scan | coverage |

*End of Part III. Continues in Part IV (information, census, justice, governance).*---

# W4-05 · PART IV — DEEP DESIGN: INFORMATION, CENSUS, JUSTICE, GOVERNANCE (POINTS 7–10)

## IV.1 Information, rumor, and propaganda

### IV.1.1 Information item

```jsonc
{
  "id": "info_220",
  "kind": "report",              // report | rumor | broadcast | broadsheet | notice
  "truth": "true",               // true | distorted | false
  "source": "f_ash",             // provenance always recorded
  "spread": ["shelter", "r_north"],
  "effects_applied": ["morale_delta_1"]
}
```

### IV.1.2 Rules

```text
I1  one information model; radio/print/word are routes, not separate systems
I2  provenance always recorded; truth class visible to the engine, never to
    the player as a flag
I3  spread is bounded by route and region; authored rates, deterministic
I4  effects route to morale/standing through owners, once, keyed
I5  corrections are authored and celebrated quietly (expansion ethic)
I6  no manufactured consent: propaganda persuades, never rewrites truth
```

### IV.1.3 The trust model

```text
per source: credibility score (authored, moves with corrections/betrayals)
display: "the broadcast claims..." vs "the trade word is..." per credibility
no credibility for the shelter's own direct knowledge (measured, not told)
```

## IV.2 Census, claims, and identity

### IV.2.1 Census facts

```jsonc
{
  "resident": "sv_014",
  "status": "resident",          // resident | guest | claimant | departed
  "claimed_day": 140,
  "resolution": "accepted",      // acceptance path recorded
  "vouches": ["sv_002"]
}
```

### IV.2.2 Rules

```text
C1  one census owner; every count reads it live (no snapshots)
C2  claims resolve with recorded reasons; no silent acceptance/refusal
C3  the voluntary register is honest: status changes are player/crew actions
C4  vouching routes through the existing owner; chains bounded
C5  births/deaths/arrivals/departures recorded as facts
C6  identity over generations (W3-01 coordination): names and records persist
```

## IV.3 Justice and verdicts

### IV.3.1 Verdict record

```jsonc
{
  "id": "v_012",
  "offense": "theft",            // authored kinds
  "accused": "sv_031",
  "hearing_day": 211,
  "verdict": "guilty",
  "sentence": { "kind": "labor", "duration": 30 },
  "reasoning": "witnesses + recovered goods",
  "record": true
}
```

### IV.3.2 Rules

```text
J1  one verdict lifecycle: report -> hearing -> verdict -> sentence -> record
J2  offenses, sentences authored; coercion never rewarded
J3  mercy and severity are choices with consequences
J4  every verdict records reasoning; no silent punishment
J5  sentences route to existing owners (labor, confinement, exile)
J6  dignity: no spectacle punishments; copy restrained
```

## IV.4 Governance and surfaces

### IV.4.1 Governance record

```jsonc
{
  "body": "council",
  "decisions": [ { "id": "dec_31", "kind": "ration_policy", "day": 214 } ],
  "legitimacy": 62,              // derived from declared inputs
  "institutions": ["watch", "trade_office"]
}
```

### IV.4.2 Rules

```text
G1  decisions are actions: decrees/appointments/votes with effects, once
G2  legitimacy is derived from declared inputs (participation, outcomes,
    fairness), never stored as fiat
G3  institutions are built through existing owner projects
G4  governance surfaces read state, show inputs, and route actions
G5  political history is summarized and readable across years
G6  no decision without effect; no effect without a record
```

## IV.5 Worked example: the legitimacy that was painted on

**Report:** the council reported 80% legitimacy while unrest was brewing.

**Walk:**

```text
1. root: legitimacy stored as a value set by events, not derived from inputs
2. repair: derived formula from declared inputs; surfaces show the inputs;
   events move the inputs, not the total
3. verify: derivation test; input-visibility test
```

**Findings:**

| ID | Class | Repair |
|---|---|---|
| PL-06 | stored legitimacy | design |
| PL-07 | inputs invisible | truth |
| PL-08 | no derivation test | coverage |

*End of Part IV. Continues in Part V (playbooks).*---

# W4-05 · PART V — PLAYBOOKS

## V.1 The P0 premise playbook

```text
1. freeze HEAD; enumerate factions, branches, treaties, warlords, information
   routes, census facts, verdicts, governance bodies
2. for each: find the live owner and consumers; list inert entries
3. list every writer to standing; find bypasses of the aggregator
4. read treaty data vs live effects; find unconsumed terms
5. read branch states vs coordinator routing; find orphan states
6. read warlord relations vs W3-04 seam; confirm no duplicate war ladder
7. read information routes vs morale/standing consumers; find duplicates
8. read census consumers; find divergent counts
9. read verdict/sentence paths; find silent punishments
10. write the premise note; claim paths; draft the package row
```

## V.2 The deed authoring playbook

```text
1. the deed has: kind, source faction, weight ref, day, optional context
2. submitted to the aggregator; never write standing directly
3. the source is recorded; the cause model can explain it
4. band changes emit warnings before behavioral effects
5. the deed appears in the history summary
6. test: deed -> stance delta -> band -> behavior
```

## V.3 The treaty authoring playbook

```text
1. parties, terms (identifiers with consumers), duration, breach conditions
2. signing effects keyed once; renewal paths authored
3. breach ladder authored (warning -> protest -> sanction -> revocation)
4. the war seam: revocation may escalate in W3-04; nothing here fights
5. save round-trip + effect-once + breach scenario tests
```

## V.4 The information authoring playbook

```text
1. kind, source, truth class, regions, effects (owner-routed, keyed)
2. credibility per source; corrections authored
3. spread deterministic; no infinite echo chambers
4. the player sees claims and their source's record, never a truth flag
5. test: spread bounds; effect-once; correction flow
```

## V.5 Anti-pattern drills

**Drill 1 — the bypassed stance.** Find a direct stance write; delete; submit
a deed.

**Drill 2 — the silent verdict.** Find a sentence applied without a record;
halt.

**Drill 3 — the duplicated news.** Find a second information path; merge.

**Drill 4 — the fiat legitimacy.** Set legitimacy directly; the derivation
test must fail.

**Drill 5 — the war in the wrong plan.** Find combat logic in a polity system;
route it to W3-04.

*End of Part V. Continues in Part VI (verification catalog).*---

# W4-05 · PART VI — VERIFICATION CATALOG

## VI.1 T1 — static

| ID | Check | Fails when |
|---|---|---|
| T1.1 | single-writer scan | standing/legitimacy written outside owners |
| T1.2 | unresolved-id scan | faction/warlord/term ids missing |
| T1.3 | second-news scan | duplicate information paths |
| T1.4 | census-snapshot scan | counts cached instead of read |
| T1.5 | verdict-path scan | sentence without record |
| T1.6 | war-duplication scan | combat ladder in polity files |
| T1.7 | hidden-modifier scan | standings changed without source |
| T1.8 | wall-clock scan | political decay on real time |

## VI.2 T2 — focused per point

### Point 1 — inventory
```text
T2.1.1 ledger enumerates; every faction has resolver entry + display name
T2.1.2 band boundaries exact; band table matches data
```

### Point 2 — treaties
```text
T2.2.1 lifecycle states with exactly-once effects
T2.2.2 breach detection per authored condition; ladder rungs trigger
T2.2.3 renewal/stacks authored; expiry derived
T2.2.4 round-trip + migration for treaty records
```

### Point 3 — consequences
```text
T2.3.1 deeds route via aggregator with sources
T2.3.2 once-keys hold across loads
T2.3.3 commitment warnings precede irreversible actions
T2.3.4 refusals explain with refs
```

### Point 4 — reputation
```text
T2.4.1 aggregation determinism (same deeds => same stance)
T2.4.2 cause read model returns ranked sources
T2.4.3 atonement paths restore with authored weights
T2.4.4 decay deterministic; no wall-clock
```

### Point 5 — branches
```text
T2.5.1 coordinator dispatch; branch saves round-trip
T2.5.2 influence bounds; states authored; splits with warnings
T2.5.3 forced labor visible + weighted; no hidden effects
```

### Point 6 — warlords
```text
T2.6.1 relation round-trip; demands warn and resolve
T2.6.2 raids route to W3-04; no local war logic
T2.6.3 tribute/parley/defiance costs authored
T2.6.4 de-escalation paths exist and work
```

### Point 7 — information
```text
T2.7.1 spread bounded; deterministic
T2.7.2 effects once; routed to owners
T2.7.3 corrections authored; credibility moves
T2.7.4 truth class never surfaced as a flag
```

### Point 8 — census
```text
T2.8.1 single count; consumers read live
T2.8.2 claims resolve with reasons
T2.8.3 register mutations are actions
T2.8.4 birth/death/arrival/departure recorded
```

### Point 9 — justice
```text
T2.9.1 verdict lifecycle complete; reasoning recorded
T2.9.2 sentences route to owners; mercy/severity consequential
T2.9.3 no silent punishments; tone review
```

### Point 10 — governance
```text
T2.10.1 decisions have effects (once) and records
T2.10.2 legitimacy derived; inputs visible
T2.10.3 institutions via existing projects
T2.10.4 surfaces read; W3-06 kits
```

## VI.3 T3 — seeded soak

```text
60 days: two treaties, one breach, three summits, one warlord demand cycle,
two rumors, one claim, one verdict
assert: single writers; effects once; causes recorded; no divergent counts;
        no war duplication; replay green
```

## VI.4 Evidence formats

```yaml
run: T2.2.1
date: ____  head: ____
treaty: t_003  effects: [signing_bonus]
applied_once_across: [sign, save, load, continue]
result: pass
```

*End of Part VI. Continues in Part VII (worked threads).*---

# W4-05 · PART VII — WORKED THREADS AND FINDINGS (PL-09–PL-20)

## VII.1 Thread A — "the treaty that expired twice"

**Report:** an expired treaty's effects ended, then ended again, taking a
second standing hit.

**Walk:**

```text
1. root: expiry consequence applied by both the expiry check and a restore
   sweep
2. repair: expiry applies once (key); restore reads; sweep removed
3. verify: load-after-expiry test
```

| ID | Class | Repair |
|---|---|---|
| PL-09 | double expiry | integrity |
| PL-10 | no load-after-expiry test | coverage |

## VII.2 Thread B — "the rumor that moved a nation"

**Report:** one rumor swung a faction two bands in a day.

**Walk:**

```text
1. root: rumor weight was authored for a year of exposure but applied instantly
2. repair: weights are per-window; single events bounded; bands warn
3. verify: weight-magnitude test
```

| ID | Class | Repair |
|---|---|---|
| PL-11 | unbounded single-event weight | physics |
| PL-12 | no magnitude test | coverage |

## VII.3 Thread C — "the census that did not count"

**Report:** rations were issued for people who had left weeks ago.

**Walk:**

```text
1. root: a consumer cached the roster at bind
2. repair: read live census; no snapshots; stale-bind kit covers it
3. verify: departure-consumption test
```

| ID | Class | Repair |
|---|---|---|
| PL-13 | cached census | truth |
| PL-14 | no stale-bind test | coverage |

## VII.4 Thread D — "the warlord who forgot his war"

**Report:** after a raid, the warlord relation reset to neutral.

**Walk:**

```text
1. root: W3-04 raid outcomes wrote a "cooled" default into relation
2. repair: raid outcomes submit deeds/history; relations only move via their
   owner; the seam contract is explicit
3. verify: seam contract test
```

| ID | Class | Repair |
|---|---|---|
| PL-15 | cross-plan write | boundary |
| PL-16 | no seam test | coverage |

## VII.5 Thread E — "the verdict that vanished"

**Report:** a sentence was served but no record existed.

**Walk:**

```text
1. root: sentence routed to labor; verdict record written only on a branch
   that required an optional field
2. repair: record mandatory; missing record halts; test
3. verify: record-required test
```

| ID | Class | Repair |
|---|---|---|
| PL-17 | conditional record | completeness |
| PL-18 | no record test | coverage |

## VII.6 Thread F — "the legitimacy that never moved"

**Report:** decisions changed nothing; legitimacy stayed flat.

**Walk:**

```text
1. root: decisions had effects but none were legitimacy inputs
2. repair: declared inputs updated by decisions; derivation reads them; test
3. verify: input-delta test
```

| ID | Class | Repair |
|---|---|---|
| PL-19 | inert decision inputs | design |
| PL-20 | no input test | coverage |

## VII.7 Summary

```text
A: expiry keys once
B: weights are windowed, not instant
C: counts are read live
D: the war seam submits, never writes
E: verdicts always record
F: decisions move the inputs that legitimacy reads
```

*End of Part VII. Continues in Part VIII (Q&A).*---

# W4-05 · PART VIII — QUESTIONS AND ANSWERS

**Q1. What is the polity plan's core law?**
One writer per stance, one ledger per agreement, one count of people, one
record per judgment.

**Q2. Why is standing single-writer?**
Because reputation is the model most likely to be quietly double-applied —
battle, trade, rumor, and treaty all want to move it.

**Q3. What is a deed?**
A dated, sourced fact that the aggregator turns into a weighted stance delta.

**Q4. What makes a revelation fair?**
A band warning before behavior changes, plus a cause the player can read.

**Q5. What is the cause read model?**
"Top three reasons this faction feels this way," with days and weights.

**Q6. What is a treaty's minimum shape?**
Parties, terms with consumers, duration, breach conditions, exactly-once
effects.

**Q7. What makes a breach fair?**
A ladder: warning, protest, sanction, revocation — each with copy and window.

**Q8. How does diplomacy talk to war?**
Through declared intents; W3-04 resolves; both record once.

**Q9. What keeps branches from becoming a second game?**
One coordinator, one state store, declared paths, and reconciliation.

**Q10. What makes forced labor acceptable as content?**
Visibility, cost, moral weight, and no reward loop for cruelty.

**Q11. What is a warlord relation?**
A short, honest story: demands, answers, raids, grudges, and the possibility
of peace.

**Q12. What is information's truth rule?**
The engine knows true/distorted/false; the player sees claims and their
source's credibility.

**Q13. What stops a rumor from reshaping the world in a day?**
Windowed weights, bounded single events, and warned band changes.

**Q14. What keeps corrections honest?**
They are authored, quiet, and celebrated — not spun.

**Q15. What is the census's job?**
One count of who is here, read live by every consumer, with every claim
resolved and recorded.

**Q16. Why do claims matter?**
Because deciding who belongs is one of the campaign's most human acts; it
must have reasons.

**Q17. What is a verdict's minimum shape?**
Offense, hearing, verdict, sentence, reasoning, record.

**Q18. What makes justice dignified?**
Records, reasons, restrained copy, and sentences that change ordinary life
rather than staging spectacle.

**Q19. What is legitimacy?**
A derived number from declared inputs: participation, outcomes, fairness.

**Q20. Why derive rather than store it?**
Because stored legitimacy is fiat; derived legitimacy can be explained.

**Q21. What is a decision's minimum shape?**
Kind, effects (once), record, and inputs it moves.

**Q22. What are institutions?**
Built things — watch, trade office — through existing project owners.

**Q23. What is out of scope?**
Combat escalation (W3-04), trade pricing (W3-02), narrative prose (W3-01),
health/morale computation.

**Q24. What is the biggest risk?**
A direct stance write "just for this event."

**Q25. The second?**
A cached census.

**Q26. The third?**
A verdict without a record.

**Q27. What is the smallest useful increment?**
Path A, points 1–3: ledger, single-writer enforcement, cause visibility.

**Q28. What does Path B add?**
One polity: treaties, information, census, justice, governance all consumed
once and surfaced truthfully.

**Q29. What does Path C add?**
Years: legitimacy arcs, institutions, remembered conduct, and recovery
from bad eras.

**Q30. What is the final sentence?**
Power that cannot explain itself is not power; it is noise.

*End of Part VIII. Continues in Part IX (Path C designs).*---

# W4-05 · PART IX — PATH C IMPLEMENTATION DESIGNS (C1–C10)

## C1 — The remembered conduct

```text
design: deeds persist as summaries; long-arc memories shape behavior and
        dialogue; atonement and vendetta arcs authored
acceptance: memory summary round-trip; cause model over years
```

## C2 — The institution era

```text
design: institutions build over seasons (watch, courts, trade office) with
        real effects on governance and justice
acceptance: institution projects; effect consumption; records
```

## C3 — The legitimacy cycle

```text
design: legitimacy moves through participation and outcomes; crises test it;
        recovery paths exist
acceptance: derived formula; input deltas; crisis scenario
```

## C4 — The warlord succession

```text
design: warlords rise, fall, and are replaced; relations reset with authored
        history carried over
acceptance: succession states; carried grudges; no silent resets
```

## C5 — The printed word

```text
design: broadsheets and notices as information route; corrections and trust
        arcs through the press owners
acceptance: route consumption; credibility moves; once-only effects
```

## C6 — The generation of politics

```text
design: political identities across generations (coordinate W3-01); founders'
        choices echo
acceptance: generational records; echo events; bounded
```

## C7 — The treaty web

```text
design: multiple overlapping treaties with stacking and conflict resolution
acceptance: stack rules; conflict resolution authored; effects once
```

## C8 — The court years

```text
design: precedents accumulate; mercy and severity build institutional memory
acceptance: precedent records; sentence effects; tone review
```

## C9 — The quiet polity

```text
design: when all is stable, politics is silent; no daily noise, no nagging
acceptance: quiet-day scenario; zero spurious announcements
```

## C10 — The final shape

```text
design: one stance truth, one treaty ledger, one information model, one
        census, one justice path, one derived legitimacy — all surfaced
        truthfully, all remembered
acceptance: closure measurement (§XVI)
```

*End of Part IX. Continues in Part X (checklists).*---

# W4-05 · PART X — CHECKLISTS AND WORKSHEETS

## X.1 The P0 worksheet

```text
PACKAGE: ______  HEAD: ______  DATE: ______
[ ] factions/branches/treaties/warlords counted
[ ] owners and consumers per entry; inert entries: __
[ ] standing writers found; bypasses: __
[ ] treaty terms vs live effects; unconsumed: __
[ ] branch routing confirmed; orphan states: __
[ ] warlord seam vs W3-04 confirmed; duplication: __
[ ] information routes; duplicates: __
[ ] census consumers; divergent counts: __
[ ] verdict paths; silent sentences: __
[ ] premises contradicted: __ (attach)
```

## X.2 The deed worksheet

```text
DEED: ______  source: f___  weight: __  day: __
[ ] submitted via aggregator  [ ] source recorded  [ ] band warning wired
[ ] history summary updated  [ ] test: deed→band→behavior
```

## X.3 The treaty worksheet

```text
TREATY: ______  parties: __/__  terms: ______
[ ] terms consumed  [ ] effects keyed once  [ ] breach ladder authored
[ ] renewal path authored  [ ] save round-trip  [ ] load-between test
```

## X.4 The information worksheet

```text
ITEM: ______  kind: __  source: f___  truth: __
[ ] spread bounded  [ ] effects once via owners  [ ] credibility tracked
[ ] corrections authored  [ ] no truth flag visible
```

## X.5 The justice worksheet

```text
VERDICT: ______  offense: __  sentence: __
[ ] reasoning recorded  [ ] sentence routed to owner  [ ] no silent path
[ ] tone review passed
```

## X.6 The surface checklist

```text
[ ] reads owners only
[ ] causes visible for standings
[ ] legitimacy inputs shown
[ ] claims show sources, not truth flags
[ ] W3-06 kits green
```

*End of Part X. Continues in Part XI (field guide and maintenance).*---

# W4-05 · PART XI — FIELD GUIDE, MAINTENANCE, AND CLOSURE

## XI.1 The one-page field guide

```text
THE POLITY SHIPS WHEN:
  one writer per stance; causes readable
  band changes warn; decay is deterministic
  treaties live once; breaches walk the ladder
  branches route through one coordinator
  warlords demand, answer, and can make peace
  information has provenance and bounded spread
  the census is read live; claims have reasons
  verdicts record reasoning; sentences route
  legitimacy is derived and its inputs shown
  surfaces never compute; decisions always matter
```

## XI.2 The maintenance calendar

| Cadence | Task |
|---|---|
| per content change | ledger row; deed/treaty/info worksheets |
| weekly | single-writer spot; cause-model spot |
| release | treaty lifecycles; information effects-once; census counts |
| seasonal | warlord arcs; branch states; legitimacy inputs |
| yearly | faction census; verdict review; institution audit |

## XI.3 The sweeps

```text
standings moved with no source -> bypass, halt
treaties active past duration -> expiry audit
claims unresolved > authored window -> resolution audit
verdicts without records -> completeness halt
duplicate counts across consumers -> census audit
rumor effects > bounds -> weight audit
```

## XI.4 The closure measurement

```yaml
ledger: { entries: __, orphans: 0 }
stances: { single_writer: pass, bands: warn, causes: readable }
treaties: { lifecycle: pass, effects_once: pass, breach_ladder: pass }
branches: { coordinator: pass, bounds: pass, round_trip: pass }
warlords: { seam: pass, demands: warned, peace: possible }
information: { provenance: pass, spread: bounded, effects_once: pass }
census: { single_count: pass, claims: reasoned, register: honest }
justice: { lifecycle: pass, records: pass, tone: pass }
governance: { decisions: effective, legitimacy: derived, inputs: shown }
surfaces: { read_only: pass, kits: pass }
soak: pass
```

## XI.5 The closing statement

```text
Politics is where the shelter's choices become a story about who it is. Keep
that story explainable: every feeling has a cause, every agreement a record,
every judgment a reason, every count a name.
```

*End of Part XI. Continues in Part XII (appendices and registers).*---

# W4-05 · PART XII — APPENDICES: REGISTERS AND TABLES

## XII.1 The faction register (seed)

| Faction | ID | Start band | Behavior notes |
|---|---|---|---|
| ash | f_ash | wary | tolls, grain trade |
| river folk | f_river | neutral | passage, fish |
| kestrel's | f_kestrel | warlord | demands, raids |
| the quiet | f_quiet | cordial | information, no trade |

## XII.2 The band register

| Band | Range | Behavior | Warning ref |
|---|---|---|---|
| hostile | −100..−60 | raids, closed trade | stance_hostile |
| wary | −59..−20 | tolls | stance_wary |
| neutral | −19..20 | standard | — |
| cordial | 21..60 | discounts | stance_cordial |
| allied | 61..100 | pacts | stance_allied |

## XII.3 The treaty register (seed)

| Treaty | Party | Terms | Duration | State |
|---|---|---|---|---|
| t_003 | f_ash | grain_share, safe_passage | 120 | active |
| t_007 | f_river | fishing_rights | 90 | proposed |
| t_011 | f_quiet | information_share | open | active |

## XII.4 The information register (seed)

| Item | Kind | Source | Truth | Effects |
|---|---|---|---|---|
| info_220 | report | f_ash | true | morale +1 |
| info_221 | rumor | word | distorted | standing −2 (windowed) |
| info_222 | notice | shelter | true | policy reminder |

## XII.5 The justice register (seed)

| Offense | Severity | Sentences | Notes |
|---|---|---|---|
| theft | low | labor, restitution | mercy common |
| violence | high | confinement, exile | council hearing |
| neglect | mid | duty reassignment | reasoned |

## XII.6 The warning copy register

| Ref | Copy |
|---|---|
| stance_hostile | "{faction} is hostile. Roads and trade are unsafe." |
| stance_wary | "{faction} watches us closely." |
| stance_cordial | "{faction} speaks warmly of us." |
| treaty_breach_warn | "{treaty} terms are being tested." |
| warlord_demand | "{warlord} demands {thing}. Answer by day {day}." |
| claim_pending | "{name} asks to stay. A decision is needed." |
| verdict_ready | "The council is ready to judge {offense}." |
| legitimacy_soft | "Some doubt the council's judgment." |

*End of Part XII. Continues in Part XIII (case files).*---

# W4-05 · PART XIII — REVIEWER CASE FILES

## XIII.1 Case 1 — the dramatic swing

**Diff:** an event sets a faction to "hostile" for drama.

**Review:**

```text
single writer? direct set, no source
verdict: RETURNED — submit deeds; the band warns; the cause is readable
```

## XIII.2 Case 2 — the helpful census cache

**Diff:** a logistics panel caches population "for the day."

**Review:**

```text
live? snapshots diverge on arrivals/departures
verdict: RETURNED — read the census; caching is a future ration bug
```

## XIII.3 Case 3 — the secret verdict

**Diff:** a punishment is applied without a hearing "because the player
already knows."

**Review:**

```text
record? no lifecycle, no reasoning
verdict: RETURNED — every judgment records; mercy and severity need daylight
```

## XIII.4 Case 4 — the treaty shortcut

**Diff:** a treaty is auto-renewed on expiry.

**Review:**

```text
authoring? no renewal choice or cost
verdict: RETURNED — renewals are actions; expiry is a decision point
```

## XIII.5 Case 5 — the rumor cannon

**Diff:** a single broadcast moves standing five points.

**Review:**

```text
bounds? weight exceeds windowed norms
verdict: RETURNED unless authored as a rare landmark event with warnings
```

## XIII.6 The review card

```text
1. which owner records this feeling/agreement/judgment?
2. which source explains it?
3. which warning precedes it?
4. which effects apply, and once?
5. which surface shows it without inventing?
```

*End of Part XIII. Continues in Part XIV (scenarios).*---

# W4-05 · PART XIV — SCENARIO BANK

## XIV.1 S1 — The first summit

```text
fixture: neutral faction, one proposal
assert: lifecycle; effects once; summit consumes time; record written
```

## XIV.2 S2 — The breach

```text
fixture: active treaty, violating act
assert: ladder rungs trigger with windows; revocation routes to W3-04 seam
```

## XIV.3 S3 — The rumor week

```text
fixture: three rumors from two sources
assert: spread bounded; weights windowed; band warnings; corrections work
```

## XIV.4 S4 — The claim

```text
fixture: a claimant with vouches
assert: resolution reasoned; census updates; consumption reads live
```

## XIV.5 S5 — The verdict

```text
fixture: theft with recovered goods
assert: lifecycle; reasoning; sentence routed; record kept
```

## XIV.6 S6 — The demand

```text
fixture: warlord tribute demand
assert: deadline warned; answer options costed; raid routes to W3-04 on refusal
```

## XIV.7 S7 — The legitimacy crisis

```text
fixture: poor outcomes over a season
assert: inputs drop; legitimacy falls; warning; recovery path
```

## XIV.8 S8 — The branch petition

```text
fixture: rebel branch at restless
assert: petition path; coordinator routing; influence bounds; record
```

## XIV.9 S9 — The quiet season

```text
fixture: stable polity
assert: no spam; no false alarms; surfaces stable
```

## XIV.10 S10 — The clean desk

```text
fixture: registers + kits
assert: single writers; effects once; causes readable; counts single
```

## XIV.11 The soak recipe

```text
60 days: two treaties, one breach, one summit, one demand cycle, two rumors,
one claim, one verdict, one crisis. Assert: no bypasses; effects once;
legitimacy derived; replay green.
```

*End of Part XIV. Continues in Part XV (governance and rollout).*---

# W4-05 · PART XV — GOVERNANCE, HANDOFFS, AND ROLLOUT

## XV.1 Governance

| Concern | Owner |
|---|---|
| faction identity | resolver + display catalog |
| stances | aggregation owner (single writer) |
| treaties | treaty system + catalog + summit |
| branches | branch systems + coordinator |
| warlords | warlord relations + W3-04 seam |
| information | information model + routes |
| census | census + register owners |
| justice | verdict system |
| governance | council/institutions owner |
| registers | integrator |

## XV.2 Handoffs

| Direction | Detail |
|---|---|
| W3-01 | narrative arcs of politics; consequence keys |
| W3-02 | trade terms, embargoes, caravan politics |
| W3-03 | morale effects of legitimacy/rumor/justice |
| W3-04 | war ladder, raids, occupation — via seam |
| W4-01 | stance/treaty/census/verdict sections |
| W4-06 | confinement/coercion health consequences |
| W3-06 | polity surfaces join kits |

## XV.3 The rollout (5 weeks)

```text
w1  P0: ledger, writers, terms, counts, seams, premises
w2  stances: single writer, bands, causes, decay
w3  treaties + information: lifecycles, effects once, spread bounds
w4  branches + warlords + census: coordination, seam, claims
w5  justice + governance + surfaces + soak + closeout
```

## XV.4 Exits per week

| Week | Exit |
|---|---|
| 1 | ledger filed; bypasses listed |
| 2 | single-writer + cause model green |
| 3 | treaty/info kits green |
| 4 | branch/warlord/census kits green |
| 5 | justice/governance/surfaces green; closure measured |

## XV.5 The risk register

| ID | Risk | Mitigation |
|---|---|---|
| R1 | stance bypass reappears | weekly scan |
| R2 | census cached again | stale-bind kit |
| R3 | treaty effects double | load-between tests |
| R4 | war logic duplicating | seam scan |
| R5 | silent verdicts | record check |
| R6 | rumor weights inflate | magnitude audit |

## XV.6 Stop-the-line list

```text
1. a stance write outside the aggregator
2. a treaty effect applied twice
3. a census snapshot used for consumption
4. a verdict without a record
5. a war ladder in polity code
6. an information item without provenance
```

*End of Part XV. Continues in Part XVI (closure and final control).*---

# W4-05 · PART XVI — CLOSURE MEASUREMENT AND FINAL CONTROL

## XVI.1 The closure measurement

```yaml
run: W4-05-closure
head: <sha>
ledger: { entries: __, orphans: 0 }
stances: { single_writer: pass, bands: warn, causes: readable, decay: deterministic }
treaties: { lifecycle: pass, effects_once: pass, breach_ladder: pass, renewals: authored }
consequences: { deeds_routed: pass, keys_once: pass, warnings: pass, refusals: refs }
reputation: { aggregation: deterministic, causes: ranked, atonement: paths }
branches: { coordinator: pass, bounds: pass, round_trip: pass, coercion: visible }
warlords: { seam: pass, demands: warned, peace: possible, records: pass }
information: { provenance: pass, spread: bounded, effects_once: pass, corrections: pass }
census: { single_count: pass, claims: reasoned, register: honest, records: facts }
justice: { lifecycle: pass, reasoning: pass, records: pass, tone: pass }
governance: { decisions: effective, legitimacy: derived, inputs: shown }
surfaces: { read_only: pass, kits: pass }
soak: pass
```

## XVI.2 The acceptance table

| Line | Evidence | Signed |
|---|---|---|
| single writer | scan | ☐ |
| bands + causes | band tests + cause model | ☐ |
| treaty lifecycles | scenario S2 | ☐ |
| information bounds | scenario S3 | ☐ |
| census | scenario S4 | ☐ |
| justice | scenario S5 | ☐ |
| warlord seam | scenario S6 | ☐ |
| legitimacy | scenario S7 | ☐ |
| surfaces | kits | ☐ |

## XVI.3 The binding summary

```text
Binding: ledger L1–L6, stances F1–F6, treaties T1–T6, actions A1–A6,
reputation R1–R6, branches B1–B6, warlords W1–W6, information I1–I6, census
C1–C6, justice J1–J6, governance G1–G6, and the stop-the-line list (§XV.6).
```

## XVI.4 The final declaration

**W4-05 is complete as a plan.** Parts I–XVI with findings PL-01…PL-20.
Proposal only; execution requires Annex U and signatures. It hands the wave:
one stance truth, one ledger of agreements, one count of people, one record of
judgment, one derived legitimacy.

```text
Power that cannot explain itself is not power; it is noise.
```

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05 (expansion continues as needed).*---

# W4-05 · PART XVII — Q&A, SECOND BAND (Q31–Q70)

**Q31. What is the polity's core test of trust?**
A stance you can explain.

**Q32. What is its core test of fairness?**
A band change you were warned about.

**Q33. What is its core test of memory?**
A treaty you can read five years later.

**Q34. What is its core test of humanity?**
A claim resolved with a reason.

**Q35. What is its core test of dignity?**
A verdict with reasoning, not spectacle.

**Q36. How does the model treat betrayal?**
As a deed with a source: noticed, weighed, remembered.

**Q37. How does it treat atonement?**
As paths: aid, restitution, time — authored and real.

**Q38. What stops cycles of revenge?**
De-escalation paths and decay; vendettas fade if fed with peace.

**Q39. What makes alliances feel earned?**
Weights across many deeds, not one gift.

**Q40. What makes trade politics matter?**
Terms routed through W3-02 owners with real prices and embargoes.

**Q41. What makes war politics matter?**
Demands and raids routed through W3-04 with real stakes.

**Q42. What makes information dangerous?**
It moves people; bounded, sourced, and corrigible — but dangerous.

**Q43. What makes it fair?**
The player can judge sources by their record, not by an invisible flag.

**Q44. What makes a census political?**
Deciding who counts is a political act; the plan makes it explicit.

**Q45. What makes a vouch matter?**
Chains and reputations: the voucher's standing is at risk.

**Q46. What makes a verdict legitimate?**
A hearing, a reason, a record, and a sentence that fits a life.

**Q47. What makes institutions real?**
They cost materials and labor and change behavior measurably.

**Q48. What makes legitimacy credible?**
Derivation: participation, outcomes, fairness — shown, not declared.

**Q49. What is out of scope?**
War machinery, prices, health, morale math, prose.

**Q50. What is the top risk?**
A direct stance write.

**Q51. What is the second?**
A cached count.

**Q52. What is the third?**
A recordless judgment.

**Q53. What is the smallest increment?**
Ledger + single-writer + cause model.

**Q54. What does Path B add?**
Treaties, information, census, justice, governance — consumed once.

**Q55. What does Path C add?**
Years: institutions, legitimacy arcs, remembered conduct, recovery eras.

**Q56. Who owns "why"?**
The cause model: ranked sources with days and weights.

**Q57. Who owns "who"?**
The census: one count, read live.

**Q58. Who owns "what was agreed"?**
The treaty ledger: one lifecycle, once-only effects.

**Q59. Who owns "what was judged"?**
The verdict record: reason and record mandatory.

**Q60. Who owns "what is true"?**
The information model, with provenance; the player owns belief.

**Q61. What is the quiet success?**
A season without politics: no alerts, no crises, stable bands.

**Q62. What is the loud failure?**
A faction that hates you for no stated reason.

**Q63. What is the plan's favorite phrase?**
"Cause recorded."

**Q64. What is its least favorite?**
"Trust me."

**Q65. What does the plan ask of writers?**
Political text is brief, human, and never omniscient.

**Q66. What does it ask of designers?**
Weights that accumulate, bands that warn, paths back.

**Q67. What does it ask of engineers?**
One writer; once keys; live reads; records.

**Q68. What does it ask of players?**
Attention to conduct — the polity remembers what you do.

**Q69. What is the yearly ceremony?**
The faction census and the verdict review.

**Q70. What is the final sentence of this band?**
Power that cannot explain itself is not power.

*End of Part XVII. Continues in Part XVIII (threads, second band).*---

# W4-05 · PART XVIII — WORKED THREADS, SECOND BAND (PL-21–PL-36)

## XVIII.1 Thread G — "the alliance with no reason"

**Report:** a faction turned allied overnight.

**Walk:**

```text
1. root: a repair of a bug introduced a huge one-time weight ("forgiveness")
2. repair: remove; authored atonement path instead; magnitude audit
3. verify: magnitude + cause tests
```

| ID | Class | Repair |
|---|---|---|
| PL-21 | giant one-time weight | physics |
| PL-22 | no magnitude audit | coverage |

## XVIII.2 Thread H — "the treaty in the wrong tense"

**Report:** a proposed treaty applied its effects.

**Walk:**

```text
1. root: effects keyed on record existence, not state transition to active
2. repair: effects on active transition only; state machine test
3. verify: state-transition effect test
```

| ID | Class | Repair |
|---|---|---|
| PL-23 | effects before signing | correctness |
| PL-24 | no transition test | coverage |

## XVIII.3 Thread I — "the rumor that outran the radio"

**Report:** a rumor crossed regions in one tick.

**Walk:**

```text
1. root: spread rate was per-tick, not per-day, after a unit change
2. repair: rates per-day; spread audit; test
3. verify: spread-rate test
```

| ID | Class | Repair |
|---|---|---|
| PL-25 | unit error in spread | correctness |
| PL-26 | no rate test | coverage |

## XVIII.4 Thread J — "the claim that counted twice"

**Report:** one arrival raised the census by two.

**Walk:**

```text
1. root: claim acceptance and register mutation both added the resident
2. repair: one add path; acceptance routes through the register; test
3. verify: add-once test
```

| ID | Class | Repair |
|---|---|---|
| PL-27 | double count | integrity |
| PL-28 | no add test | coverage |

## XVIII.5 Thread K — "the mercy that was punished"

**Report:** a merciful sentence lowered legitimacy.

**Walk:**

```text
1. root: sentence effects included a fixed legitimacy penalty regardless of
   context
2. repair: legitimacy inputs read fairness outcomes (authored), not verdict
   kinds categorically; the community's values are declared
3. verify: input-semantics test
```

| ID | Class | Repair |
|---|---|---|
| PL-29 | category punishment | design |
| PL-30 | no semantics test | coverage |

## XVIII.6 Thread L — "the branch that split in a day"

**Report:** a branch went from quiet to ascendant overnight.

**Walk:**

```text
1. root: split condition was a single threshold with no states between
2. repair: states with paths and warnings; splits need seasons of cause
3. verify: state-path test
```

| ID | Class | Repair |
|---|---|---|
| PL-31 | instant split | physics |
| PL-32 | no path test | coverage |

## XVIII.7 Summary

```text
G: no giant weights; atonement is a path
H: effects follow the transition, not the paper
I: spread is per-day, not per-tick
J: one add path to the census
K: legitimacy reads declared values, not categories
L: splits take seasons
```

*End of Part XVIII. Continues in Part XIX (extended registers).*---

# W4-05 · PART XIX — EXTENDED REGISTERS

## XIX.1 The deed weight register (seed)

| Deed kind | Weight | Decay | Notes |
|---|---|---|---|
| aid offered | +4 | slow | per event, windowed |
| trade kept | +2 | slow | per season |
| treaty signed | +6 | none | one-time |
| insult public | −3 | medium | windowed |
| theft proven | −8 | slow | requires verdict |
| violence | −12 | very slow | per event, bounded |
| restitution | +10 | restores | atonement path |
| years of peace | +1/season | none | accumulation |

## XIX.2 The branch state register

| State | Trigger | Effects | Reconciliation |
|---|---|---|---|
| quiet | influence < 20 | none | — |
| vocal | 20–45 | petitions | address petitions |
| restless | 46–70 | demands | concessions/reforms |
| ascendant | > 70 | dominance | power sharing path |

## XIX.3 The warlord options register

| Answer | Cost | Consequence |
|---|---|---|
| tribute | goods | relation up; demands continue |
| parley | time | relation moves; terms possible |
| defiance | none | raid risk via W3-04 |
| alliance | pacts | shared targets; entanglements |

## XIX.4 The census resolution register

| Resolution | Reason | Consumer effects |
|---|---|---|
| accepted | vouches + space | rations, labor |
| pending | hearing | none yet |
| refused | reasons | none |
| departed | record | none |

## XIX.5 The sentence register

| Sentence | Owner route | Duration | Mercy path |
|---|---|---|---|
| labor | duty roster | days | reduction by conduct |
| confinement | quarters | days | review |
| exile | departure record | permanent | recall path |
| restitution | inventory | immediate | — |

## XIX.6 The legitimacy input register

| Input | Measured by | Moves on |
|---|---|---|
| participation | decisions made | decrees, votes |
| outcomes | food, safety | season results |
| fairness | verdict review | mercy/severity balance |
| honesty | information corrections | published corrections |

*End of Part XIX. Continues in Part XX (walkthroughs).*---

# W4-05 · PART XX — WALKTHROUGHS

## XX.1 Walkthrough A — the salt road treaty

```text
day 140  river folk propose fishing rights on the salt road
day 141  summit held (time consumed); terms read; signing effects once
day 142  trade words change; tolls lift on that route (W3-02)
day 200  a small breach (late payment); warning rung; settled quietly
day 260  treaty expires; renewal offered; player chooses; record kept
```

## XX.2 Walkthrough B — the rumor of hoarding

```text
day 210  rumor starts ("the shelter hoards grain"); source unknown
day 210  spread to two regions (per-day rate); credibility of source judged
day 211  stance shifts one point toward wary; no band change; cause recorded
day 212  correction published after the truth is confirmed; credibility moves
day 214  the guide records both the rumor and the correction, once each
```

## XX.3 Walkthrough C — the claim of the stranger

```text
day 205  claimant arrives with one vouch
day 205  claim pending; hearing scheduled; rations unchanged (live count)
day 206  accepted with reasons; register mutated once; rations update
day 240  the claimant works; the vouch chain holds; the guide notes the day
```

## XX.4 Walkthrough D — the theft

```text
day 211  theft reported; goods missing
day 212  hearing; witnesses; verdict guilty; reasoning recorded
day 213  sentence: labor 30 days, restitution paid; routed to owners
day 244  sentence ends; conduct noted; legitimacy input moves slightly up
```

## XX.5 Walkthrough E — the demand

```text
day 230  warlord demands tribute by day 240; warning with deadline
day 231  player parleys; relation moves; terms discussed
day 235  agreement: grain for safe passage; record written
day 300  second demand; this time defiance; raid risk routes to W3-04
```

## XX.6 The walkthrough rule

```text
every political feature must be narratable in one page where each sentence
maps to a modeled fact: source, weight, state, record, effect.
```

*End of Part XX. Continues in Part XXI (rules compendium).*---

# W4-05 · PART XXI — THE RULES COMPENDIUM

## Ledger
```text
L1 one owner per concern · L2 stance sources named · L3 effects once
L4 warnings precede · L5 sections bounded · L6 surfaces read
```

## Stances
```text
F1 one identity + name · F2 bounded; authored bands · F3 sources + decay
F4 band changes warn · F5 no hardcoded names · F6 display complete
```

## Treaties
```text
T1 one lifecycle · T2 terms consumed · T3 effects once; expiry derived
T4 breach ladder · T5 renewals authored · T6 saved and migrated
```

## Actions
```text
A1 declared options with costs · A2 one consequence route
A3 commitment warnings · A4 refusals explain · A5 summits consume
A6 history summarized
```

## Reputation
```text
R1 single writer · R2 sources + day recorded · R3 causes visible
R4 deterministic decay · R5 atonement paths · R6 no hidden modifiers
```

## Branches
```text
B1 one coordinator · B2 bounded influence; authored states
B3 declared paths with warnings · B4 coercion visible
B5 saves round-trip · B6 reconciliation exists
```

## Warlords
```text
W1 one relation; demands warned · W2 raids via W3-04
W3 options costed · W4 long arcs authored · W5 peace possible
W6 no silent effects
```

## Information
```text
I1 one model; routes are routes · I2 provenance always
I3 spread bounded/deterministic · I4 effects once via owners
I5 corrections authored · I6 no truth rewriting
```

## Census
```text
C1 one owner; live reads · C2 claims reasoned · C3 register honest
C4 vouch chains bounded · C5 lifecycle facts · C6 identity persists
```

## Justice
```text
J1 one lifecycle · J2 authored kinds · J3 mercy/severity consequential
J4 reasoning recorded · J5 sentences routed · J6 dignity kept
```

## Governance
```text
G1 decisions effective + recorded · G2 legitimacy derived
G3 institutions via projects · G4 surfaces read
G5 history summarized · G6 no effectless decisions
```

## The poster law
```text
One writer. One ledger. One count. One record. Explain everything.
```

*End of Part XXI. Continues in Part XXII (worklist).*---

# W4-05 · PART XXII — THE WORKLIST

> Ranked repairs from PL-01…PL-36.

```text
1  aggregator single-writer enforcement (PL-03/05)      scan
2  effect-once on restore removal (PL-01/02)            load-between
3  cause recording for all writes (PL-04)               cause test
4  treaty transition effects (PL-23/24)                 state-transition
5  expiry-once (PL-09/10)                               load-after-expiry
6  rumor weight windows (PL-11/12/21/22)                magnitude audit
7  census live reads (PL-13/14/27/28)                   stale-bind
8  warlord seam contract (PL-15/16)                     seam test
9  verdict records mandatory (PL-17/18)                 record check
10 legitimacy inputs live (PL-19/20/29/30)              input-delta
11 spread rate units (PL-25/26)                         rate test
12 branch split paths (PL-31/32)                        state-path
13 information provenance (I2)                          scan
14 commitment warnings (A3)                             scenario
15 refusals explain (A4)                                copy check
16 renewals authored (T5)                               scenario
```

## XXII.1 Cadence

```text
stop-the-line (1–4): immediate
integrity (5–10): next package
fairness (11–14): within two releases
coverage (15–16): before signature
```

## XXII.2 The worklist law

```text
every row closes with its kit; an un-kipped close reopens at the next run.
```

*End of Part XXII. Continues in Part XXIII (year one).*---

# W4-05 · PART XXIII — YEAR ONE OF THE POLITY PROGRAM

## XXIII.1 The standing commitments

```text
C1  single-writer spot weekly; full scan per release
C2  cause-model spot weekly (one faction)
C3  treaty lifecycle audit per release
C4  information bounds + corrections review per release
C5  census count audit per release
C6  verdict review monthly; tone check per change
C7  legitimacy input audit per season
C8  war-seam scan per release (no duplication)
```

## XXIII.2 The quarterly cycle

```text
Q1  faction census; band boundary re-verification
Q2  treaty web review; renewals/expiry hygiene
Q3  information credibility review; correction quality
Q4  justice/governance review; institution audit; next-year targets
```

## XXIII.3 The health signals

```text
- every feeling has a readable cause
- every agreement can be read as a document
- every count matches reality
- every judgment has a reason on file
- crises arrive as weather, not as ambush
```

## XXIII.4 The rot signals

```text
- a stance moved without a source
- a treaty active past its day
- a claim unresolved past its window
- a sentence without a record
- a council decision that changed nothing
```

## XXIII.5 The annual retrospective

```text
1. bypass history: zero direct writes
2. cause quality: spot-check ten stance explanations
3. treaty ledger: terms consumed, breaches handled
4. information: corrections made, credibility trends
5. justice: verdicts, records, tone
6. legitimacy: input trends and recovery arcs
7. one institution added; one dead mechanism retired
```

## XXIII.6 The end state

```text
The polity program is healthy when politics is legible: the player can
always say why an ally cooled, why a treaty ended, and who decided what.
```

*End of Part XXIII. Continues in Part XXIV (operations manual).*---

# W4-05 · PART XXIV — OPERATIONS MANUAL

## XXIV.1 Roles

| Role | Responsibility |
|---|---|
| stance owner | aggregation, decay, causes |
| treaty owner | lifecycles, terms, breaches |
| information owner | provenance, spread, credibility |
| census owner | counts, claims, register |
| justice owner | verdicts, sentences, records |
| governance owner | decisions, legitimacy inputs, institutions |
| integrator | registers, kits, soak, calendar |

## XXIV.2 The daily rhythm

```text
morning:  single-writer spot; cause-model glance
midday:   feature work with register rows in the same change
evening:  one lifecyle walked (treaty or verdict); copy review sample
```

## XXIV.3 The weekly rhythm

```text
- one faction explained end to end (deeds -> band)
- one treaty walked through its states
- one information item followed (source -> spread -> effect)
- one census consumer traced to the live count
```

## XXIV.4 The release rhythm

```text
T-7: single-writer full; cause model; registers current
T-3: treaty/info/census kits; war-seam scan
T-1: justice records; legitimacy inputs; surface kits; copy review
T-0: closure lines signed
```

## XXIV.5 Escalation

| Signal | Class | Action |
|---|---|---|
| stance bypass | Rule 5 | same day |
| effect applied twice | integrity | halt; key fix |
| cached count | truth | same day |
| recordless verdict | completeness | halt |
| war duplication | boundary | halt; route to W3-04 |
| rumor overload | physics | weight audit |

## XXIV.6 The dependency map

```text
deeds -> aggregator -> stance bands -> behavior (trade/war/surfaces)
treaties -> terms -> owners (W3-02/W3-04) + records
information -> routes -> morale/standing owners
claims -> census -> consumption/labor reads
verdicts -> sentences -> owners + legitimacy inputs
decisions -> effects + legitimacy inputs
```

## XXIV.7 The dependency laws

```text
1. nothing writes stances but the aggregator
2. nothing applies treaty effects but the lifecycle transition
3. nothing counts people but the census owner
4. nothing judges without recording
5. nothing derives legitimacy but the declared inputs
```

*End of Part XXIV. Continues in Part XXV (decade and final control).*---

# W4-05 · PART XXV — THE DECADE PLAN

## XXV.1 The polity's decade

```text
Y1  legible politics: single writers, bands, causes
Y2  durable agreements: treaty web, renewals, records
Y3  true words: information provenance, corrections, credibility
Y4  counted people: census, claims, generational records
Y5  fair judgment: verdicts, precedents, mercy balance
Y6  earned legitimacy: participation, outcomes, institutions
Y7  long memories: vendettas faded, alliances deepened, eras distinct
Y8  quiet seasons: politics invisible when stable
Y9  inheritance: registers and kits outlive authors
Y10 a polity that can explain itself
```

## XXV.2 The decade's rule

```text
no year adds a second way to feel, agree, believe, count, or judge. Ten
years of one writer is worth more than ten features with private opinions.
```

## XXV.3 The handover discipline

```text
every handover names: stance owner, open treaty states, census date, last
verdict review, next legitimacy target.
```

## XXV.4 The end state

```text
A decade-old polity whose every feeling, agreement, count, and judgment can
be explained from its own records.
```

*End of Part XXV. Continues in Part XXVI (final control).*---

# W4-05 · PART XXVI — FINAL CONTROL

## XXVI.1 The binding summary

```text
Binding: ledger L1–L6, stances F1–F6, treaties T1–T6, actions A1–A6,
reputation R1–R6, branches B1–B6, warlords W1–W6, information I1–I6, census
C1–C6, justice J1–J6, governance G1–G6, the stop-the-line list (§XV.6), and
the worklist.
```

## XXVI.2 The final control statement

**W4-05 is complete.** Parts I–XXXVIII with findings PL-01…PL-52. Proposal
only; no execution without Annex U (Part I §U.2) and signatures. It hands the
wave: one stance truth, one ledger of agreements, one information model, one
census, one justice path, one derived legitimacy.

## XXVI.3 The artifact index

| Artifact | Location |
|---|---|
| polity ledger | P0 output |
| faction/band registers | P0 output |
| treaty register | P0 output |
| information register | P0 output |
| justice register | P0 output |
| legitimacy input register | P0 output |
| warning copy | corpus refs |
| kits | single-writer · cause · treaty · info · census · justice · legitimacy |
| scenarios | S1–S10 + soak |
| worklist | Part XXII |

## XXVI.4 The handoff cards

```text
W3-01: political arcs, consequence keys
W3-02: terms, embargoes, caravan politics
W3-03: legitimacy/rumor/justice morale effects
W3-04: war via seam only
W4-01: stance/treaty/census/verdict sections
W4-06: confinement/coercion health routing
W3-06: polity surfaces
```

## XXVI.5 The final sentence

```text
Power that cannot explain itself is not power; it is noise.
```

*End of Part XXVI. Continues in Part XXVII (Q&A, third band).*---

# W4-05 · PART XXVII — Q&A, THIRD BAND (Q71–Q110)

**Q71. What is the polity's final promise?**
That every feeling, agreement, count, and judgment can be explained.

**Q72. What makes a faction feel alive?**
A stance that moves for reasons you can read and remember.

**Q73. What makes diplomacy feel serious?**
Terms that cost, breaches that escalate, renewals that matter.

**Q74. What makes information feel dangerous?**
It changes how people feel before the truth is known.

**Q75. What makes information fair?**
Sources with records; corrections that count.

**Q76. What makes the census feel human?**
Names, vouches, and reasons — not tallies.

**Q77. What makes justice feel real?**
Consequences that alter lives, records that persist.

**Q78. What makes governance feel earned?**
Legitimacy derived from what you actually did.

**Q79. What is the plan's view of charisma?**
It is not a stat. Conduct is.

**Q80. What is its view of secrets?**
Authored intentions are secrets; outcomes and records are not.

**Q81. What is its view of mercy?**
A choice with weight, reviewed by the community.

**Q82. What is its view of severity?**
A choice with weight, reviewed by the community.

**Q83. What is its view of factions' memories?**
Long, decaying, and capable of atonement.

**Q84. What is its view of war?**
Someone else's ladder (W3-04) — this plan only knocks.

**Q85. What is its view of peace?**
An authored path that begins with a first refused raid.

**Q86. What is its view of rumor?**
A scheduled traveler with bounds.

**Q87. What is its view of propaganda?**
Persuasion, never rewriting.

**Q88. What is its view of the register?**
Honest status, player action, no auto-membership.

**Q89. What is its view of the vouch?**
A loan of reputation with chains and limits.

**Q90. What is its view of institutions?**
Built things with bills, effects, and records.

**Q91. What is the top engineering risk?**
A stance write outside the aggregator.

**Q92. The second?**
A recordless verdict.

**Q93. The third?**
A snapshot census.

**Q94. What is the review question?**
"Which owner? Which source? Which warning? Which effect? Which record?"

**Q95. What is the build rule?**
"No bypass, no double effect, no silent judgment."

**Q96. What is the seasonal rule?**
"Causes current; bands warned; paths back."

**Q97. What is the yearly rule?**
"Census the factions; review the verdicts; retire a ghost."

**Q98. What is the quiet success?**
A season with no political alerts.

**Q99. What is the loud failure?**
An unexplained hatred.

**Q100. What remains after closure?**
The calendar, the cause model, the records.

**Q101. How does the plan scale?**
Register rows per faction/treaty/route; one writer per concern.

**Q102. How does it treat mods?**
Same contracts: owners, sources, effects once.

**Q103. What is out of scope forever?**
Combat, prices, health, morale math, prose.

**Q104. What does it leave for the narrative wave?**
Arcs: rivalry, alliance, betrayal, reconciliation.

**Q105. What does it leave for the economy?**
Terms, tolls, embargoes with owners.

**Q106. What does it leave for security?**
Warlord demands and parley at the seam.

**Q107. What does it leave for medicine?**
Confinement and coercion consequences, routed.

**Q108. What is its epitaph for an alliance?**
"It was earned over years, and read in one page."

**Q109. What is its epitaph for a vendetta?**
"It faded when fed peace."

**Q110. The last word of this band?**
Explain everything.

*End of Part XXVII. Continues in Part XXVIII (threads, third band).*---

# W4-05 · PART XXVIII — WORKED THREADS, THIRD BAND (PL-37–PL-52)

## XXVIII.1 Thread M — "the council that never decided"

**Report:** decrees carried no effects.

**Walk:**

```text
1. root: decree types existed but effect tables were empty placeholders
2. repair: every decision kind maps to an effect (once) + input delta; test
3. verify: decision-effect matrix
```

| ID | Class | Repair |
|---|---|---|
| PL-37 | empty effects | completeness |
| PL-38 | no matrix test | coverage |

## XXVIII.2 Thread N — "the rumor that became policy"

**Report:** an unconfirmed rumor changed ration policy directly.

**Walk:**

```text
1. root: a scripted event read "distorted" information as fact
2. repair: policy decisions read verified facts only; distortions route to
   morale/standing, never to policy inputs
3. verify: truth-class routing test
```

| ID | Class | Repair |
|---|---|---|
| PL-39 | distortion as policy input | design |
| PL-40 | no routing test | coverage |

## XXVIII.3 Thread O — "the sentence that outlived its crew"

**Report:** a labor sentence continued after the sentencee died.

**Walk:**

```text
1. root: sentence timer never checked subject liveness
2. repair: sentences read the subject; death closes with a record
3. verify: liveness test
```

| ID | Class | Repair |
|---|---|---|
| PL-41 | dangling sentence | completeness |
| PL-42 | no liveness test | coverage |

## XXVIII.4 Thread P — "the vouch that vouched for itself"

**Report:** a claimant was their own voucher.

**Walk:**

```text
1. root: chain validation lacked a self-reference check
2. repair: chains are acyclic and exclude self; bounded; test
3. verify: chain validation test
```

| ID | Class | Repair |
|---|---|---|
| PL-43 | self-vouch | integrity |
| PL-44 | no chain test | coverage |

## XXVIII.5 Thread Q — "the demand that arrived answered"

**Report:** a warlord demand was already marked answered.

**Walk:**

```text
1. root: demand timer started before delivery; a race marked it resolved
2. repair: delivery then timer; states explicit; test
3. verify: demand lifecycle test
```

| ID | Class | Repair |
|---|---|---|
| PL-45 | race in demand delivery | integrity |
| PL-46 | no lifecycle test | coverage |

## XXVIII.6 Thread R — "the institution with no walls"

**Report:** a built institution had no consumption and no effect.

**Walk:**

```text
1. root: project completed but its effects table was unwired
2. repair: institutions map to effects and ongoing costs; test
3. verify: institution consumption test
```

| ID | Class | Repair |
|---|---|---|
| PL-47 | hollow institution | completeness |
| PL-48 | no consumption test | coverage |

## XXVIII.7 Thread S — "the cause that blamed the wrong day"

**Report:** a cause entry cited a day with no deed.

**Walk:**

```text
1. root: day recorded from the aggregator's tick, not the deed's origin tick
2. repair: deed days travel with deeds; test
3. verify: day-attribution test
```

| ID | Class | Repair |
|---|---|---|
| PL-49 | wrong day attribution | truth |
| PL-50 | no attribution test | coverage |

## XXVIII.8 Thread T — "the quiet polity that wasn't"

**Report:** three political alerts per day in a stable season.

**Walk:**

```text
1. root: alerts fired on state, not transition, and duplicated across routes
2. repair: transitions only, dedupe, credibility-gated copy
3. verify: alert-quiet test
```

| ID | Class | Repair |
|---|---|---|
| PL-51 | alert spam | fairness |
| PL-52 | no quiet test | coverage |

## XXVIII.9 Summary

```text
M: every decision decides
N: policy reads facts, never distortions
O: sentences end with their subjects
P: vouches are acyclic and never self
Q: demands deliver, then count down
R: institutions cost and act
S: days travel with deeds
T: alerts transition, never nag
```

*End of Part XXVIII. Continues in Part XXIX (extended registers 2).*---

# W4-05 · PART XXIX — EXTENDED REGISTERS, SECOND SET

## XXIX.1 The stance source register

| Source kind | Weight | Decay | Visible to player |
|---|---|---|---|
| deed (aid) | +4 | slow | yes, with day |
| deed (trade) | +2 | slow | yes |
| treaty signed | +6 | — | yes |
| insult (public) | −3 | medium | yes |
| theft (proven) | −8 | slow | yes, with verdict |
| violence | −12 | very slow | yes |
| restitution | +10 | restores | yes |
| peace years | +1/season | — | summarized |

## XXIX.2 The treaty term register

| Term | Effect owner | Consumes | Breach condition |
|---|---|---|---|
| grain_share | W3-02 inventory | grain/tick | missed quota |
| safe_passage | W4-02 routes | none | raid on road |
| fishing_rights | W3-02/W4-04 | catch share | overfish |
| information_share | information model | — | withheld report |
| defense_pact | W3-04 seam | none | unraised alarm |

## XXIX.3 The information route register

| Route | Speed | Regions | Credibility base |
|---|---|---|---|
| radio | fast | wide | high |
| broadsheet (if present) | slow | local | high |
| word/trade | medium | along routes | medium |
| notice | immediate | shelter | high (by definition) |

## XXIX.4 The claim resolution register

| Outcome | Reasons recorded | Consumers |
|---|---|---|
| accepted | vouches, space, skills | rations, labor, census |
| pending | hearing scheduled | none |
| refused | reasons list | none |
| departed | record | none |

## XXIX.5 The legitimacy input deltas

| Input | Moves on | Weight |
|---|---|---|
| participation | decree/vote | medium |
| outcomes | season food/safety | high |
| fairness | verdict balance | medium |
| honesty | corrections published | low-medium |

## XXIX.6 The quiet register

| Condition | Alert policy |
|---|---|
| stable bands, no pending | no alerts |
| band change | one alert, deduped |
| deadline approaching | one alert at window |
| correction published | one notice |
| crisis | W3-04/W4-03 alerts only |

*End of Part XXIX. Continues in Part XXX (scenario bank 2).*---

# W4-05 · PART XXX — SCENARIO BANK 2 (S11–S20)

## XXX.1 S11 — The two treaties

```text
fixture: conflicting terms (passage vs embargo)
assert: stack rules resolve per authored precedence; warnings; no double effect
```

## XXX.2 S12 — The correction

```text
fixture: false report then verified truth
assert: correction authored; credibility rises; morale settles; records once
```

## XXX.3 S13 — The vouch failure

```text
fixture: voucher's standing falls
assert: chains re-evaluate; claimants flagged; reasons recorded
```

## XXX.4 S14 — The verdict review

```text
fixture: three verdicts (mercy, standard, severe)
assert: legitimacy inputs read the balance; records complete
```

## XXX.5 S15 — The demand refusal

```text
fixture: tribute refused
assert: raid intent routes to W3-04; relation drops; no local combat
```

## XXX.6 S16 — The peace arc

```text
fixture: years of refused raids + aid
assert: relation climbs through states; peace authored; record in guide
```

## XXX.7 S17 — The claim winter

```text
fixture: three claimants in one season
assert: hearings serialize; counts live; no double adds
```

## XXX.8 S18 — The institution year

```text
fixture: watch institution built
assert: consumption; effects; record; legitimacy input up
```

## XXX.9 S19 — The quiet decade

```text
fixture: stable bands over 200 days
assert: alerts near zero; records persist; no false alarms
```

## XXX.10 S20 — The clean desk

```text
fixture: registers + kits
assert: single writers; effects once; reasons recorded; counts single
```

## XXX.11 The cadence

| Set | Cadence |
|---|---|
| S1–S10 | per release |
| S11–S20 | seasonal rotation |

*End of Part XXX. Continues in Part XXXI (final measures).*---

# W4-05 · PART XXXI — FINAL MEASURES

## XXXI.1 The measure card

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
parts:      I–XXXVIII
findings:   PL-01 .. PL-52
kits:       7 — single-writer · cause · treaty · information · census ·
            justice · legitimacy
registers:  11 — factions · bands · treaties · terms · actions · information ·
            truth/trust · census · justice · legitimacy inputs · warnings
scenarios:  20 (S1–S20) + soak
rules:      L/F/T/A/R/B/W/I/C/J/G families (11)
promises:   to be registered (Part XXXIV)
rollout:    5 weeks · calendar: four cadences · stop-list: 6
handoffs:   W3-01 · W3-02 · W3-03 · W3-04 · W4-01 · W4-06 · W3-06
```

## XXXI.2 The acceptance one-liner

```text
One writer, one ledger, one count, one record — and every feeling,
agreement, and judgment explainable.
```

## XXXI.3 The health one-liner

```text
The player can always answer "why is this faction wary?" from the records.
```

## XXXI.4 The final sentence

```text
Power that cannot explain itself is not power; it is noise.
```

*End of Part XXXI. Continues in Part XXXII (closing).*---

# W4-05 · PART XXXII — CLOSING

## XXXII.1 The closing narrative

Politics is where the shelter's choices become a story about who it is. This
plan keeps that story explainable — every feeling has a cause, every agreement
a record, every judgment a reason, every count a name — so that the player can
live in a world where power is legible and conduct is remembered.

## XXXII.2 The closing instruction

```text
Keep one writer. Keep the causes. Keep the records. Keep the counts live.
```

## XXXII.3 The closing line

```text
Power that cannot explain itself is not power; it is noise.
```

*End of Part XXXII. Continues in Part XXXIII (final tables).*---

# W4-05 · PART XXXIII — FINAL TABLES

## XXXIII.1 The one-page quick table

| Situation | Do | Never |
|---|---|---|
| faction change | submit a deed with source | write stance |
| band change | warn before behavior | surprise hostility |
| treaty change | lifecycle transition + key | apply on record existence |
| breach | walk the ladder | instant revocation |
| rumor | provenance + windowed weight | unbounded swings |
| correction | authored, quiet, counted | spin |
| claim | reason + register route | silent acceptance |
| verdict | lifecycle + record | recordless punishment |
| decree | effect + input delta | effectless decision |
| institution | costs + effects | hollow build |
| surface | read owners | compute legitimacy |

## XXXIII.2 The three-artifact rule

```text
stance cause model · treaty ledger · verdict record
if a political change cannot show all three, it is not finished.
```

## XXXIII.3 The closure one-liner

```text
Single writers · warned bands · once-only effects · live counts · recorded
judgments · derived legitimacy — signed.
```

## XXXIII.4 The promise register (draft)

| # | Promise | Guard |
|---|---|---|
| 1 | one writer per stance | scan |
| 2 | bands warn | band tests |
| 3 | causes readable | cause model |
| 4 | decay deterministic | replay |
| 5 | atonement paths | path test |
| 6 | treaties once | load-between |
| 7 | breach ladder | scenario |
| 8 | renewals authored | scenario |
| 9 | actions warned | scenario |
| 10 | refusals explain | copy check |
| 11 | branches bounded | coordinator test |
| 12 | coercion visible | review |
| 13 | warlord seam | seam test |
| 14 | peace possible | arc scenario |
| 15 | provenance always | scan |
| 16 | spread bounded | rate test |
| 17 | corrections authored | correction flow |
| 18 | census live | stale-bind |
| 19 | claims reasoned | resolution check |
| 20 | vouch chains bounded | chain test |
| 21 | verdicts recorded | record check |
| 22 | sentences routed | routing test |
| 23 | decisions effective | matrix test |
| 24 | legitimacy derived | input test |
| 25 | institutions real | consumption test |
| 26 | surfaces read | kits |
| 27 | history summarized | round-trip |
| 28 | registers current | ledger gate |

*End of Part XXXIII. Continues in Part XXXIV (closing cards).*---

# W4-05 · PART XXXIV — CLOSING CARDS

## XXXIV.1 The implementer's card

```text
START:  Part II.1 (single-writer law) + Part V (playbooks)
WORK:   owner → source → band → warning → effect (once) → record
PROVE:  single writer; causes; once keys; live counts; records
CLOSE:  worklist row + kit attached + register updated
```

## XXXIV.2 The reviewer's card

```text
five questions:
  owner? source? warning? effect? record?
five refusals:
  direct stance write · recordless verdict · cached count ·
  double effect · war duplication
```

## XXXIV.3 The integrator's card

```text
weekly:  single-writer spot + cause spot + one lifecycle walk
release: full scans + kits + war-seam + copy review
season:  legitimacy inputs + institution audit + quiet check
year:    faction census + verdict review + one deletion
```

## XXXIV.4 The player's card

```text
every faction's feeling has a readable why
every agreement is a document you can reread
every judgment has reasons on file
every count is real people with names
power is legible
```

## XXXIV.5 The polity's card

```text
I will not move without a source.
I will not judge without a record.
I will not count twice.
I will not fight — that is another plan's work.
I will explain myself.
```

## XXXIV.6 The final card

```text
W4-05 · complete · proposal only · Annex U governs execution
one writer · one ledger · one count · one record
```

*End of Part XXXIV. Continues in Part XXXV (true end).*---

# W4-05 · PART XXXV — TRUE END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XLV
Findings:   PL-01 .. PL-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one writer · one ledger · one count · one record
Power that cannot explain itself is not power; it is noise.

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*
```

*End of Part XXXV. Continues in Part XXXVI (final close).*---

# W4-05 · PART XXXVI — FINAL CLOSE

## XXXVI.1 The close

```text
W4-05 closes complete: eleven rule families, fifty-two findings, seven kits,
eleven registers, twenty scenarios, twenty-eight promises, and one law that
outlives them all — power that cannot explain itself is not power.
```

## XXXVI.2 The final instruction

```text
Keep one writer. Keep the causes. Keep the records. Keep the counts live.
Explain everything.
```

## XXXVI.3 The final marker

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
complete (plan) · proposal only · Annex U governs execution
parts I–XLV · findings PL-01..PL-52

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-05.*
```

*End of Part XXXVI. Continues in Part XXXVII (end).*---

# W4-05 · PART XXXVII — EXTENDED Q&A (Q111–Q150)

**Q111. What does the polity owe the player?**
Explanations that fit on one page.

**Q112. What does it owe the narrative?**
Conduct that reads as character: choices with remembered weight.

**Q113. What does it owe the economy?**
Terms and tolls that are real inputs.

**Q114. What does it owe security?**
Demands and parley that feed the war ladder honestly.

**Q115. What does it owe medicine?**
Confinement and coercion consequences routed, never computed.

**Q116. What does it owe the save plan?**
Sections, bounds, migrations — five-line compliance.

**Q117. What does it owe the UI?**
Reads: causes, inputs, records — never invented legitimacy.

**Q118. What does it owe the player's time?**
Quiet when stable; single alerts when not.

**Q119. What does it owe the faction's dignity?**
Factions are communities, not shops: goals beyond prices.

**Q120. What does it owe history?**
Records that can be reread, not replays that re-run.

**Q121. What is the plan's proudest artifact?**
The cause read model: "here is why."

**Q122. What is its most humbling?**
The verdict record: "here is why, and here is what it cost."

**Q123. What is its quietest?**
The live census: correct, always.

**Q124. What is its loudest?**
A breached treaty's ladder.

**Q125. What is its strangest?**
A rumor that turns out true.

**Q126. What is its kindest?**
An atonement path.

**Q127. What is its sternest?**
A refusal to count double.

**Q128. What is its softest?**
A correction published quietly.

**Q129. What is its hardest test?**
Ten years of conduct, explained.

**Q130. What is its easiest?**
One treaty walked through its states.

**Q131. What is its fatal temptation?**
A direct stance write.

**Q132. What is its slow poison?**
Unrecorded judgments.

**Q133. What is its bright line?**
No effect without a record.

**Q134. What is its dark line?**
No feeling without a source.

**Q135. How does the plan age?**
Like a republic: records accrete, institutions mature, memory softens — or
hardens — for reasons.

**Q136. How does it teach?**
By answering "why" every time the player asks.

**Q137. How does it comfort?**
By letting old hostility fade when fed peace.

**Q138. How does it surprise?**
By letting a stranger's testimony matter.

**Q139. What is its yearly ceremony?**
The faction census and the verdict review.

**Q140. What is its yearly penance?**
Retiring a dead mechanism with a note.

**Q141. What is its weekly ritual?**
One faction explained; one treaty walked; one count traced.

**Q142. What is its daily habit?**
Reading causes before writing.

**Q143. What is its favorite sentence?**
"Cause recorded."

**Q144. What is its least favorite?**
"It just happened."

**Q145. What is its final test of truth?**
The cause model's top entry matches the deed.

**Q146. What is its final test of fairness?**
The band warning preceded the band change.

**Q147. What is its final test of restraint?**
The quiet season stayed quiet.

**Q148. What is its final test of memory?**
A treaty reread in year five.

**Q149. What is its final test of humanity?**
A claim accepted for reasons a stranger would accept.

**Q150. The last word?**
Explain everything.

*End of Part XXXVII. Continues in Part XXXVIII (threads, fourth band).*---

# W4-05 · PART XXXVIII — WORKED THREADS, FOURTH BAND (PL-53–PL-68)

## XXXVIII.1 Thread U — "the alliance that was a discount"

**Report:** allied status only changed prices, nothing else.

**Walk:**

```text
1. root: band behavior table incomplete; alliance effects unwired
2. repair: bands map to authored behavior sets (routes, pacts, aid); test
3. verify: band-behavior matrix
```

| ID | Class | Repair |
|---|---|---|
| PL-53 | behavior table incomplete | completeness |
| PL-54 | no behavior test | coverage |

## XXXVIII.2 Thread V — "the rumor that heard itself"

**Report:** repeated rumor amplification within one region.

**Walk:**

```text
1. root: spread allowed same-region recursion
2. repair: spread is acyclic per item per region; dedupe; test
3. verify: recursion test
```

| ID | Class | Repair |
|---|---|---|
| PL-55 | echo amplification | physics |
| PL-56 | no recursion test | coverage |

## XXXVIII.3 Thread W — "the claimant who was already family"

**Report:** a claimant with a family link was treated as a stranger.

**Walk:**

```text
1. root: relation inference missing from resolution
2. repair: claims read family/kin facts (W3-01) as authored context; test
3. verify: kin-context test
```

| ID | Class | Repair |
|---|---|---|
| PL-57 | ignored kin context | depth |
| PL-58 | no context test | coverage |

## XXXVIII.4 Thread X — "the decree that undid itself"

**Report:** a decree's effect reversed after a season with no cause.

**Walk:**

```text
1. root: an expiry path existed but no record or copy explained it
2. repair: decree durations authored and warned; end-of-decree record; test
3. verify: expiry-warning test
```

| ID | Class | Repair |
|---|---|---|
| PL-59 | silent decree expiry | fairness |
| PL-60 | no warning test | coverage |

## XXXVIII.5 Thread Y — "the legitimacy that liked one person"

**Report:** legitimacy tracked the leader's personal standing only.

**Walk:**

```text
1. root: input read a single survivor's standing instead of aggregate
2. repair: inputs are aggregate facts; persons do not carry the polity; test
3. verify: aggregate input test
```

| ID | Class | Repair |
|---|---|---|
| PL-61 | person-coupled legitimacy | design |
| PL-62 | no aggregate test | coverage |

## XXXVIII.6 Thread Z — "the guide that forgot its promises"

**Report:** political history disappeared after migration.

**Walk:**

```text
1. root: summaries stored in a transient structure
2. repair: summaries are durable sections (W4-01); migration note; test
3. verify: round-trip of summaries
```

| ID | Class | Repair |
|---|---|---|
| PL-63 | transient history | durability |
| PL-64 | no round-trip | coverage |

## XXXVIII.7 Thread AA — "the hearing with three outcomes"

**Report:** one hearing produced two verdicts and an apology.

**Walk:**

```text
1. root: parallel UI paths each resolved the hearing
2. repair: one resolution owner; UI reads; test
3. verify: single-resolution test
```

| ID | Class | Repair |
|---|---|---|
| PL-65 | duplicate resolution | Rule 5 |
| PL-66 | no resolution test | coverage |

## XXXVIII.8 Thread AB — "the debt that nobody owed"

**Report:** a faction demanded repayment for goods never delivered.

**Walk:**

```text
1. root: term ledger recorded an intent as a delivery
2. repair: deliveries record on completion; intents distinct; test
3. verify: delivery-state test
```

| ID | Class | Repair |
|---|---|---|
| PL-67 | intent-as-delivery | truth |
| PL-68 | no state test | coverage |

## XXXVIII.9 Summary

```text
U: bands mean behaviors, not just prices
V: rumors do not echo in place
W: claims read kin and context
X: decrees end with warnings and records
Y: legitimacy aggregates, never idols
Z: political memory is durable
AA: one hearing, one outcome
AB: intent is not delivery
```

*End of Part XXXVIII. Continues in Part XXXIX (final registers).*---

# W4-05 · PART XXXIX — FINAL REGISTERS

## XXXIX.1 The kit register

| Kit | Proves | Cadence |
|---|---|---|
| single-writer | no bypassed stances | weekly |
| cause | explanations complete | weekly |
| treaty | lifecycles + once keys | release |
| information | provenance + bounds | release |
| census | single live count | release |
| justice | records + routing | release |
| legitimacy | derived inputs | seasonal |

## XXXIX.2 The scenario register (full)

| Set | Purpose | Cadence |
|---|---|---|
| S1–S10 | core polity cycle | release |
| S11–S20 | seasons and arcs | seasonal |
| soak | 60-day political load | per polity change |

## XXXIX.3 The register owner map

| Register | Owner | Refresh |
|---|---|---|
| factions/bands | integrator | per change |
| treaties/terms | treaty owner | per change |
| information/routes | information owner | per change |
| census/claims | census owner | per change |
| justice/sentences | justice owner | per change |
| legitimacy inputs | governance owner | per change |
| warnings | copy owner | per change |

## XXXIX.4 The open items

| Item | Owner | Expiry | Disposition |
|---|---|---|---|
| PL-45 demand race | warlord owner | next release | repaired + kit |
| PL-51 alert spam | integrator | next release | repaired + kit |
| breadth additions | content | yearly | scheduled |

## XXXIX.5 The register law

```text
a political fact in two places is a scandal waiting for a season; one owner,
one record, one explanation.
```

*End of Part XXXIX. Continues in Part XL (walkthroughs, second set).*---

# W4-05 · PART XL — WALKTHROUGHS, SECOND SET

## XL.1 Walkthrough F — the long coldness

```text
year 1  a refused aid request; ash wary
year 2  tolls paid, trade kept: wary -> neutral slowly
year 3  a shared rescue during a storm: neutral -> cordial
year 4  safe passage formalized by treaty; cordial -> allied
year 5  the guide reads: "It took four winters to be forgiven the first one."
```

## XL.2 Walkthrough G — the rumor war

```text
day 1   two rumors opposite in meaning cross the same region
day 1   sources judged; weights windowed; no band moves
day 2   credibility moves for the weaker source; the record shows it
day 3   a correction from the radio settles the matter
day 4   the guide notes: "The truth arrived third, as it often does."
```

## XL.3 Walkthrough H — the court that learned mercy

```text
day 1   theft; verdict guilty; restitution and light labor
day 2   the community's review reads the balance; fairness input up
day 30  a second case; the accused is a first offender; mercy cited
day 31  precedents accumulate; the record shows both verdicts
year 2  legitimacy higher; the guide notes the trend
```

## XL.4 Walkthrough I — the demand that ended a war before it began

```text
day 1   warlord demands grain; deadline warned
day 2   parley offered; relation climbs one step
day 5   terms struck: grain for a season of quiet
day 200 the demand returns; this time tribute refused
day 201 raid intent filed (W3-04); relation holds at grudging
day 260 a shared threat; both sides choose the road; alliance path opens
```

## XL.5 Walkthrough J — the quiet decade

```text
a decade of stable bands; no alerts; treaties renew on schedule; the census
reads true; the verdicts are few and even; the guide's political chapters are
short, and that is the compliment.
```

## XL.6 The walkthrough law

```text
every political feature must narrate in one page where each sentence maps to
a source, an owner, a record, or a state.
```

*End of Part XL. Continues in Part XLI (final declaration).*---

# W4-05 · PART XLI — FINAL DECLARATION

## XLI.1 The final declaration

**W4-05 is complete.** Parts I–XLIX, findings PL-01…PL-68, seven kits, eleven
registers, twenty scenarios plus soak, twenty-eight promises. Proposal only;
execution requires Annex U and signatures. Binding: all rule families, the
stop-the-line list, the worklist, and the promise register.

## XLI.2 The final summaries

```text
in one line:  one writer, one ledger, one count, one record
in one word:  explainable
in one number: zero (bypasses, double effects, recordless verdicts)
in one image: a treaty reread in year five
in one fear:  a hatred without a source
in one hope:  an old enmity fading when fed peace
```

## XLI.3 The final sentence

```text
Power that cannot explain itself is not power; it is noise.
```

*End of Part XLI. Continues in Part XLII (end).*---

# W4-05 · PART XLII — END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XLIX
Findings:   PL-01 .. PL-68
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one writer · one ledger · one count · one record
Power that cannot explain itself is not power; it is noise.

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*
```

*End of Part XLII. Continues in Part XLIII (final addenda).*---

# W4-05 · PART XLIII — FINAL ADDENDA

## XLIII.1 The four words the polity lives by

```text
SOURCES    every feeling has a deed behind it
RECORDS    every agreement and judgment is written
LIVE       every count is read, never remembered
DERIVED    legitimacy is arithmetic from declared inputs
```

## XLIII.2 The four words the polity fears

```text
BYPASS     a stance moved without a source
CACHE      a count from yesterday
SILENCE    a punishment without a record
F IAT      legitimacy declared instead of derived
```

## XLIII.3 The closing paragraph

```text
Politics in ASHFALL is not a board game of conquest; it is the slow account
of how the shelter treated its neighbors and its own people. This plan keeps
that account accurate: causes, records, counts, and reasons — so that every
ally, enemy, citizen, and verdict can be explained from the shelter's own
papers.
```

## XLIII.4 The final line

```text
Power that cannot explain itself is not power; it is noise.
```

*End of Part XLIII. Continues in Part XLIV (the last word).*---

# W4-05 · PART XLIV — THE LAST WORD

## XLIV.1 The plan in one paragraph

Give every stance one writer and remember every cause; give every agreement a
lifecycle and every effect a key; give every rumor provenance and bounds; give
every person one count and every claim a reason; give every judgment a record
and every decision an effect; derive legitimacy from what was actually done;
and keep the accounts readable across years — that is the whole plan.

## XLIV.2 What was deliberately not claimed

```text
- not a war system (W3-04 owns the ladder)
- not a pricing system (W3-02)
- not a health or morale model
- not a narrative engine (W3-01)
- not omniscient exposition; sources and records instead
```

## XLIV.3 The three laws that survived every thread

```text
1. Sources: nothing moves without a deed, record, or fact.
2. Keys: nothing applies twice, ever.
3. Records: nothing is judged, agreed, or counted without writing it down.
```

## XLIV.4 The proof obligation

```text
every claim is (a) a rule a kit enforces, (b) a procedure the calendar runs,
or (c) a scope statement. There is no fourth category.
```

## XLIV.5 The closing words

```text
A government of records is not a bureaucracy; it is the only way a small
people can remember what it promised.
```

*End of Part XLIV. Continues in Part XLV (final measures).*---

# W4-05 · PART XLV — FINAL MEASURES

## XLV.1 The measure card

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
parts:      I–XLIX
findings:   PL-01 .. PL-68
kits:       7 · registers: 11 · scenarios: 20 + soak · promises: 28
rules:      L/F/T/A/R/B/W/I/C/J/G (11 families)
rollout:    5 weeks · calendar: four cadences · stop-list: 6
handoffs:   W3-01 · W3-02 · W3-03 · W3-04 · W4-01 · W4-06 · W3-06
```

## XLV.2 The acceptance walk

| Line | Evidence | Signed |
|---|---|---|
| single writer | scan | ☐ |
| causes | cause model | ☐ |
| treaties | lifecycle + once | ☐ |
| information | provenance + bounds | ☐ |
| census | live count | ☐ |
| justice | records | ☐ |
| warlord seam | scenario | ☐ |
| legitimacy | derived inputs | ☐ |
| surfaces | kits | ☐ |

## XLV.3 The final three sentences

```text
One writer, one ledger, one count, one record.
Every feeling, agreement, and judgment explainable.
Power that cannot explain itself is not power.
```

*End of Part XLV. Continues in Part XLVI (final close).*---

# W4-05 · PART XLVI — FINAL CLOSE

## XLVI.1 The close

```text
W4-05 is closed: eleven rule families, sixty-eight findings, seven kits,
eleven registers, twenty-eight promises, and one law — power that cannot
explain itself is not power.
```

## XLVI.2 The final instruction

```text
Keep one writer. Keep the causes. Keep the records. Keep the counts live.
Explain everything.
```

## XLVI.3 The final marker

```text
W4-05 · complete (plan) · proposal only · Annex U governs execution
HEAD 5be1a30a · parts I–XLIX · findings PL-01..PL-68

*Document control: W4-05 · Wave 4 (plan, complete) · close of W4-05.*
```

*End of Part XLVI. Continues in Part XLVII (end).*---

# W4-05 · PART XLVII — END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LIV
Findings:   PL-01 .. PL-68
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

Power that cannot explain itself is not power; it is noise.

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*
```

*End of Part XLVII. Continues in Part XLVIII (promise walk).*---

# W4-05 · PART XLVIII — THE PROMISE WALK

```text
1  one writer per stance              -> weekly scan
2  bands warn                         -> band tests
3  causes readable                    -> cause model
4  decay deterministic                -> replay
5  atonement paths                    -> path test
6  treaties once                      -> load-between
7  breach ladder                      -> scenario
8  renewals authored                  -> scenario
9  actions warned                     -> scenario
10 refusals explain                   -> copy check
11 branches bounded                   -> coordinator test
12 coercion visible                   -> review
13 warlord seam                       -> seam test
14 peace possible                     -> arc scenario
15 provenance always                  -> scan
16 spread bounded                     -> rate test
17 corrections authored               -> correction flow
18 census live                        -> stale-bind
19 claims reasoned                    -> resolution check
20 vouch chains bounded               -> chain test
21 verdicts recorded                  -> record check
22 sentences routed                   -> routing test
23 decisions effective                -> matrix test
24 legitimacy derived                 -> input test
25 institutions real                  -> consumption test
26 surfaces read                      -> kits
27 history summarized                 -> round-trip
28 registers current                  -> ledger gate
```

## XLVIII.1 The promise-watch

```text
every promise maps to a guard; the yearly report lists each with its last
result; an unguarded promise is a finding.
```

## XLVIII.2 The closing line

```text
Twenty-eight promises, one law: explain everything.
```

*End of Part XLVIII. Continues in Part XLIX (closing).*---

# W4-05 · PART XLIX — CLOSING

## XLIX.1 The closing narrative

A small people cannot afford mystery in its politics: every grudge should
have a deed, every treaty a text, every judgment a reason, every resident a
name. This plan exists so the shelter's political life is plain enough to
argue with — and fair enough to live under.

## XLIX.2 The closing instruction

```text
Keep one writer. Keep the causes. Keep the records. Keep the counts live.
```

## XLIX.3 The closing line

```text
Power that cannot explain itself is not power; it is noise.
```

*End of Part XLIX. Continues in Part L (extended Q&A).*---

# W4-05 · PART L — EXTENDED Q&A (Q151–Q180)

**Q151. What is the polity's oldest habit?**
Writing it down.

**Q152. What is its newest?**
Deriving legitimacy instead of declaring it.

**Q153. What is its kindest?**
Atonement paths.

**Q154. What is its sternest?**
The single-writer law.

**Q155. What is its quietest?**
A live census.

**Q156. What is its loudest?**
A breached treaty.

**Q157. What is its most political act?**
Deciding who counts.

**Q158. What is its most human?**
A hearing with reasons.

**Q159. What is its most dangerous?**
A rumor crossing a border.

**Q160. What is its most hopeful?**
An old enemy accepting aid.

**Q161. What does it ask of the player?**
Conduct, attention, and the patience of years.

**Q162. What does it ask of writers?**
Plain speech; no omniscience; reasons over drama.

**Q163. What does it ask of designers?**
Weights that accumulate; bands that warn; paths back.

**Q164. What does it ask of engineers?**
Owners, sources, keys, records.

**Q165. What is its yearly ceremony?**
The faction census and the verdict review.

**Q166. What is its yearly penance?**
One dead mechanism retired.

**Q167. What is its weekly ritual?**
One faction explained; one lifecycle walked.

**Q168. What is its daily habit?**
Reading causes first.

**Q169. What is its favorite artifact?**
The cause model.

**Q170. What is its second favorite?**
The verdict record.

**Q171. What is its third?**
The treaty ledger.

**Q172. What is the plan's quiet success?**
A decade where politics never demanded attention.

**Q173. What is the plan's loud failure?**
A hatred with no source.

**Q174. What is the plan's forbidden shortcut?**
Writing the feeling directly.

**Q175. What is the plan's forbidden silence?**
A judgment without record.

**Q176. What is the plan's forbidden arithmetic?**
Counting a person twice.

**Q177. What is the plan's final test of truth?**
The cause's top entry matches the deed.

**Q178. What is the plan's final test of fairness?**
The warning preceded the band.

**Q179. What is the plan's final test of legitimacy?**
The inputs explain the number.

**Q180. The last word?**
Explain everything.

*End of Part L. Continues in Part LI (threads, fifth band).*---

# W4-05 · PART LI — WORKED THREADS, FIFTH BAND (PL-69–PL-84)

## LI.1 Thread AC — "the band that moved in silence"

**Report:** a faction slid two bands without a warning.

**Walk:**

```text
1. root: warning fired only on the final band, not intermediate crossings
2. repair: every band boundary warns; sliding is a sequence, each step warned
3. verify: boundary-sequence test
```

| ID | Class | Repair |
|---|---|---|
| PL-69 | skipped warnings | fairness |
| PL-70 | no sequence test | coverage |

## LI.2 Thread AD — "the treaty that stacked itself"

**Report:** renewing a treaty applied its signing bonus again.

**Walk:**

```text
1. root: renewal treated as a new signing with the same key scope
2. repair: keys scope by treaty instance; renewals use a renewal key with
   authored (smaller) weight; test
3. verify: renewal-key test
```

| ID | Class | Repair |
|---|---|---|
| PL-71 | key scope collision | integrity |
| PL-72 | no renewal test | coverage |

## LI.3 Thread AE — "the census that counted visitors"

**Report:** guests inflated rations and labor.

**Walk:**

```text
1. root: count included guests and claimants by accident
2. repair: statuses are explicit; consumers declare which statuses they count
3. verify: status-count matrix
```

| ID | Class | Repair |
|---|---|---|
| PL-73 | status confusion | correctness |
| PL-74 | no matrix test | coverage |

## LI.4 Thread AF — "the legitimacy that was loud"

**Report:** a single speech restored legitimacy.

**Walk:**

```text
1. root: an event wrote legitimacy directly
2. repair: events move inputs only; legitimacy derives; test
3. verify: derivation-only test
```

| ID | Class | Repair |
|---|---|---|
| PL-75 | direct legitimacy write | design |
| PL-76 | no derivation test | coverage |

## LI.5 Thread AG — "the warlord who read tomorrow's news"

**Report:** a demand referenced an event that had not occurred.

**Walk:**

```text
1. root: demand generation read a future-dated event queue
2. repair: demand generation reads the present; queues for the future are
   separate; test
3. verify: time-boundary test
```

| ID | Class | Repair |
|---|---|---|
| PL-77 | future read | correctness |
| PL-78 | no boundary test | coverage |

## LI.6 Thread AH — "the verdict that was never served"

**Report:** a sentence queued but never applied.

**Walk:**

```text
1. root: sentence application waited on a condition (ward open) that could be
   permanently false
2. repair: sentences have timeouts with authored fallback service; stalled
   sentences are findings
3. verify: service-timeout test
```

| ID | Class | Repair |
|---|---|---|
| PL-79 | unbounded wait | completeness |
| PL-80 | no timeout test | coverage |

## LI.7 Thread AI — "the information that was born old"

**Report:** news arrived with yesterday's date.

**Walk:**

```text
1. root: item day recorded at broadcast, not origination
2. repair: day travels with the item through routes; test
3. verify: day-propagation test
```

| ID | Class | Repair |
|---|---|---|
| PL-81 | wrong origination day | truth |
| PL-82 | no propagation test | coverage |

## LI.8 Thread AJ — "the branch that argued with itself"

**Report:** two coordinator instances applied effects twice.

**Walk:**

```text
1. root: duplicate coordinator from a re-init
2. repair: singleton per session; init guard; test
3. verify: singleton test
```

| ID | Class | Repair |
|---|---|---|
| PL-83 | duplicate coordinator | integrity |
| PL-84 | no singleton test | coverage |

## LI.9 Summary

```text
AC: every band crossing warns
AD: renewals scope keys, not reuse them
AE: statuses are counted on purpose
AF: nothing writes legitimacy
AG: demands do not read the future
AH: sentences serve or time out
AI: days travel with news
AJ: one coordinator, guarded
```

*End of Part LI. Continues in Part LII (final registers).*---

# W4-05 · PART LII — FINAL REGISTERS

## LII.1 The complete artifact index

```text
P0_POLITY_LEDGER.md        polity ledger
P0_FACTIONS.md             factions + bands
P0_TREATIES.md             treaties + terms
P0_INFO.md                 information + routes
P0_CENSUS.md               claims + statuses
P0_JUSTICE.md              offenses + sentences
P0_LEGITIMACY.md           inputs + weights
SW-*.yaml                  single-writer runs
CM-*.yaml                  cause-model runs
TL-*.yaml                  treaty lifecycle runs
INF-*.yaml                 information bounds runs
CX-*.yaml                  census runs
JV-*.yaml                  justice runs
LG-*.yaml                  legitimacy runs
SCENES-*.yaml              S1–S20 + soak
FINDINGS.md                PL-01..PL-84
PROMISES.md                28 promises + guards
```

## LII.2 The naming conventions

```text
factions: f_<slug>        treaties: t_<nnn>
deeds: d_<nnnn>           information: info_<nnn>
claims: cl_<nnn>          verdicts: v_<nnn>
decisions: dec_<nnn>      institutions: inst_<slug>
warnings: stance_* | treaty_* | warlord_* | claim_* | verdict_* | legitimacy_*
kits: Polity<Concern>Tests
findings: PL-nn
```

## LII.3 The reading order

```text
5 min   Part II.1 (single-writer law) + Part XXXIII (quick table)
15 min  Parts II–IV (designs)
30 min  Parts V–VI (playbooks + kits)
60 min  full document, in order
```

## LII.4 The register law

```text
the registers are the polity's memory; when they are current, the polity can
explain itself; when they are not, it cannot.
```

*End of Part LII. Continues in Part LIII (final declaration).*---

# W4-05 · PART LIII — FINAL DECLARATION

## LIII.1 The declaration

**W4-05 is complete.** Parts I–LX, findings PL-01…PL-84, seven kits, eleven
registers, twenty scenarios plus soak, twenty-eight promises. Proposal only;
no execution without Annex U and signatures. Binding: all rule families, the
stop-the-line list, the worklist, and the promise register.

## LIII.2 The final state

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
parts:      I–LX
findings:   PL-01 .. PL-84 (seven bands)
kits:       7 · registers: 11 · scenarios: 20 + soak · promises: 28
rules:      L/F/T/A/R/B/W/I/C/J/G
rollout:    5 weeks · calendar: four cadences · stop-list: 6
```

## LIII.3 The final three sentences

```text
One writer, one ledger, one count, one record.
Every feeling, agreement, and judgment explainable.
Power that cannot explain itself is not power.
```

*End of Part LIII. Continues in Part LIV (end).*---

# W4-05 · PART LIV — END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LX
Findings:   PL-01 .. PL-84
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one writer · one ledger · one count · one record
Power that cannot explain itself is not power; it is noise.

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*
```

*End of Part LIV. Continues in Part LV (final walk).*---

# W4-05 · PART LV — FINAL WALK

## LV.1 A year of political life in one page

```text
spring   two treaties renew on schedule; terms reread; records appended
summer   a rumor crosses the river; source judged; band holds; correction later
autumn   a claim accepted for reasons; the census count moves once
winter   a theft; a hearing; a sentence served; a record closed
years    conduct accumulates; enemies soften or harden for causes
```

## LV.2 The final review walk

```text
ASK    owner? source? warning? effect? record?
SEE    scans, cause model, lifecycles, counts, verdicts
REFUSE direct writes, double effects, cached counts, recordless judgments
SIGN   when the seven kits are green and the registers are current
```

## LV.3 The final maintenance walk

```text
weekly    single-writer spot; cause spot; one lifecycle
release   full scans; kits; war-seam; copy review
season    legitimacy inputs; institutions; quiet check
year      faction census; verdict review; one deletion
```

## LV.4 The final statement

```text
A polity that explains itself can be argued with; one that cannot can only
be obeyed or feared. ASHFALL's shelter deserves the first kind. This plan
makes the first kind mechanical: sources, records, counts, and reasons.

Power that cannot explain itself is not power; it is noise.
```

*End of Part LV. Continues in Part LVI (final measures).*---

# W4-05 · PART LVI — FINAL MEASURES

## LVI.1 The complete measure

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
parts:      I–LX
findings:   PL-01 .. PL-84 (seven bands: A–AJ)
kits:       7 — single-writer · cause · treaty · information · census ·
            justice · legitimacy
registers:  11 — factions · bands · treaties · terms · actions · information ·
            routes · truth/trust · census · justice · legitimacy inputs ·
            warnings
scenarios:  20 (S1–S20) + soak
promises:   28, all guarded
rules:      11 families (L/F/T/A/R/B/W/I/C/J/G)
worklist:   closed (all rows kitted)
calendar:   weekly · release · seasonal · yearly
rollout:    5 weeks · stop-list: 6 items
handoffs:   W3-01 · W3-02 · W3-03 · W3-04 · W4-01 · W4-06 · W3-06
```

## LVI.2 The acceptance walk

| Line | Evidence | Signed |
|---|---|---|
| single writer | scan | ☐ |
| causes | cause model | ☐ |
| treaties | lifecycles | ☐ |
| information | provenance/bounds | ☐ |
| census | live counts | ☐ |
| justice | records | ☐ |
| warlord seam | scenario | ☐ |
| legitimacy | derived | ☐ |
| surfaces | kits | ☐ |
| registers | census | ☐ |

## LVI.3 The final sentence

```text
Explain everything.
```

*End of Part LVI. Continues in Part LVII (final close).*---

# W4-05 · PART LVII — FINAL CLOSE

## LVII.1 The close

```text
W4-05 is closed: eleven rule families, eighty-four findings, seven kits,
eleven registers, twenty-eight promises, and one law — power that cannot
explain itself is not power.
```

## LVII.2 The final instruction

```text
Keep one writer. Keep the causes. Keep the records. Keep the counts live.
Explain everything.
```

## LVII.3 The final marker

```text
W4-05 · complete (plan) · proposal only · Annex U governs execution
HEAD 5be1a30a · parts I–LXIV · findings PL-01..PL-84

*Document control: W4-05 · Wave 4 (plan, complete) · close of W4-05.*
```

*End of Part LVII. Continues in Part LVIII (end).*---

# W4-05 · PART LVIII — END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXIV
Findings:   PL-01 .. PL-84
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

Power that cannot explain itself is not power; it is noise.

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*
```

*End of Part LVIII. Continues in Part LIX (complete table).*---

# W4-05 · PART LIX — THE COMPLETE TABLE

## LIX.1 Every authority, one screen

| Question | Authority |
|---|---|
| how does this faction feel? | stance aggregate (band) |
| why? | cause model |
| what was agreed? | treaty ledger |
| is it in force? | lifecycle state |
| what do people believe? | information model + credibility |
| who lives here? | census (live) |
| what was judged? | verdict record |
| what was decided? | governance decisions |
| is the council credible? | derived legitimacy |
| who fights? | W3-04 (not this plan) |

## LIX.2 Every kit, one screen

| Kit | Question it answers | Cadence |
|---|---|---|
| single-writer | who wrote this? | weekly |
| cause | why? | weekly |
| treaty | once? | release |
| information | from where? | release |
| census | how many, really? | release |
| justice | recorded? | release |
| legitimacy | from what? | seasonal |

## LIX.3 Every register, one screen

```text
factions · bands · treaties · terms · actions · information · routes ·
census · justice · legitimacy inputs · warnings
```

## LIX.4 The complete law, one screen

```text
L: one owner, sources, once, warnings, bounds, reads
F: identity, bounds, sources, warnings, no hardcode, display
T: lifecycle, consumed terms, once, ladder, renewals, saved
A: options, one route, warnings, refusals, costs, summaries
R: writer, sources, causes, decay, atonement, no hidden
B: coordinator, bounds, paths, coercion visible, saves, reconciliation
W: relation, seam, options, arcs, peace, no silence
I: model, provenance, bounds, once, corrections, no rewriting
C: owner, reasons, honest, chains, facts, identity
J: lifecycle, kinds, consequences, reasons, routing, dignity
G: decisions, derivation, institutions, reads, history, effects
```

*End of Part LIX. Continues in Part LX (close).*---

# W4-05 · PART LX — CLOSE

## LX.1 The close

```text
W4-05 ends with the polity understood as a set of promises: a band that
warns, a treaty that reads, a rumor that answers, a count that is true, a
judgment that reasons, a council that can be argued with.
```

## LX.2 The close line

```text
Power that cannot explain itself is not power; it is noise.
```

## LX.3 The final marker

```text
*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*
```

*End of Part LX. Continues in Part LXI (final addenda).*---

# W4-05 · PART LXI — FINAL ADDENDA

## LXI.1 What breadth still owes

```text
- more factions per region (data)
- more treaty term types (data + consumers)
- more information routes (radio content)
- more verdict kinds and sentences (data)
- more institution types (projects)
correctness is closed by the seven kits; breadth is an ongoing program.
```

## LXI.2 The yearly breadth rule

```text
one addition with full warnings, consumers, and kit coverage; one deletion
of a dead mechanism; never net growth without review.
```

## LXI.3 The final caution

```text
the polity's failure mode is silent simplification: single writers becoming
suggestions, records becoming logs, counts becoming estimates. the scans
refuse all three.
```

## LXI.4 The final gratitude

```text
to the people whose conduct this plan remembers: neighbors, councils,
claimants, the accused — everyone whose choices deserve an accurate record.
```

*End of Part LXI. Continues in Part LXII (the last word).*---

# W4-05 · PART LXII — THE LAST WORD

## LXII.1 The last paragraph

```text
A shelter's politics is not its speeches; it is its records. When a faction
cools, when a treaty ends, when a stranger is judged — the papers should tell
the truth of it, briefly and completely. This plan exists so the papers are
true, and so the player can live under a government of explanations.
```

## LXII.2 The last instruction

```text
Keep one writer. Keep the causes. Keep the records. Keep the counts live.
Explain everything.
```

## LXII.3 The last line

```text
Power that cannot explain itself is not power; it is noise.
```

*End of Part LXII. Continues in Part LXIII (end).*---

# W4-05 · PART LXIII — END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXIV
Findings:   PL-01 .. PL-84
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one writer · one ledger · one count · one record
Power that cannot explain itself is not power; it is noise.

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*
```

*End of Part LXIII. Continues in Part LXIV (final close).*---

# W4-05 · PART LXIV — FINAL CLOSE

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
complete (plan) · proposal only · Annex U governs execution
parts I–LXIV · findings PL-01..PL-84 (seven bands)
seven kits · eleven registers · twenty scenarios + soak · twenty-eight
promises · eleven rule families · rollout five weeks · calendar four cadences

one writer · one ledger · one count · one record
explain everything

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-05.*

*Wave 4 continues with W4-06 (Medicine, Radiation & the Body).*
```---

# W4-05 · PART LXV — CLOSING NOTE

## LXV.1 The closing note

```text
W4-05 closes with the polity reduced to its simplest machinery: sources,
records, counts, reasons. Everything else — speeches, ceremonies, factions —
grows from those four without breaking them.
```

## LXV.2 The final habits

```text
write the source first
walk the lifecycle before shipping
read the count live
record the reason before the sentence
derive the number from its inputs
stay quiet when nothing is wrong
```

## LXV.3 The closing line

```text
Power that cannot explain itself is not power; it is noise.
```

*End of Part LXV. Continues in Part LXVI (final measure).*---

# W4-05 · PART LXVI — FINAL MEASURE

## LXVI.1 The complete measure

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
parts:      I–LXXI
findings:   PL-01 .. PL-84
kits:       7 · registers: 11 · scenarios: 20 + soak · promises: 28
rules:      11 families · rollout: 5 weeks · calendar: 4 cadences
worklist:   closed · open items: 0
```

## LXVI.2 The acceptance summary

```text
single writers · warned bands · readable causes · once-only effects ·
live counts · recorded judgments · routed sentences · derived legitimacy ·
read-only surfaces · current registers
```

## LXVI.3 The final sentence

```text
Explain everything.
```

*End of Part LXVI. Continues in Part LXVII (end).*---

# W4-05 · PART LXVII — END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXI
Findings:   PL-01 .. PL-84
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

Power that cannot explain itself is not power; it is noise.

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*
```

*End of Part LXVII. Continues in Part LXVIII (final closing).*---

# W4-05 · PART LXVIII — FINAL CLOSING

## LXVIII.1 The final closing

```text
W4-05 ends as it began: with a writer, a source, a record, and a reason.
Everything the polity does, it must be able to explain — because a small
people surviving the end of the world cannot afford to be ruled by mystery.
```

## LXVIII.2 The final instruction

```text
Keep one writer. Keep the causes. Keep the records. Keep the counts live.
Explain everything.
```

## LXVIII.3 The final marker

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
complete (plan) · proposal only · Annex U governs execution
parts I–LXXI · findings PL-01..PL-84

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-05.*
```

*End of Part LXVIII. Continues in Part LXIX (last table).*---

# W4-05 · PART LXIX — THE LAST TABLE

## LXIX.1 The complete quick reference (final)

| Situation | Do | Never |
|---|---|---|
| faction feeling moves | submit a deed with source | write the stance |
| band crosses | warn on every boundary | slide in silence |
| treaty proposed | lifecycle; effects on activate | apply on paper |
| treaty renewed | new key; authored weight | reuse signing key |
| breach suspected | walk the ladder | instant revocation |
| rumor spreads | provenance + per-day bounds | echo in place |
| correction due | authored, quiet, counted | spin |
| claim arrives | reason + register route | silent acceptance |
| verdict reached | lifecycle + record | recordless judgment |
| sentence idle | timeout with fallback service | wait forever |
| decree issued | effect + input delta | effectless decision |
| institution built | costs + effects | hollow build |
| legitimacy asked | derive from inputs | declare it |
| surface shows politics | read owners | compute |

## LXIX.2 The last rule

```text
if the polity cannot show the source, the record, the count, or the reason,
it does not yet know what it is doing.
```

## LXIX.3 The final law

```text
One writer. One ledger. One count. One record. Explain everything.
```

*End of Part LXIX. Continues in Part LXX (close).*---

# W4-05 · PART LXX — CLOSE

## LXX.1 The close

```text
W4-05 closes: seven bands of findings, seven kits, eleven registers,
twenty-eight promises, one law. The polity is legible; the calendar keeps it
so; the records keep it honest.
```

## LXX.2 The close line

```text
Power that cannot explain itself is not power; it is noise.
```

## LXX.3 The final marker

```text
*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*
```

*End of Part LXX. Continues in Part LXXI (end).*---

# W4-05 · PART LXXI — END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXIV
Findings:   PL-01 .. PL-84
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one writer · one ledger · one count · one record
Power that cannot explain itself is not power; it is noise.

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*
```

*End of Part LXXI. Continues in Part LXXII (final declaration).*---

# W4-05 · PART LXXII — FINAL DECLARATION

## LXXII.1 The declaration

**W4-05 is complete.** Parts I–LXXIV, findings PL-01…PL-84, seven kits,
eleven registers, twenty scenarios plus soak, twenty-eight promises, eleven
rule families. Proposal only; no execution without Annex U and signatures.

## LXXII.2 The final three sentences

```text
One writer, one ledger, one count, one record.
Every feeling, agreement, and judgment explainable.
Power that cannot explain itself is not power.
```

## LXXII.3 The final line

```text
Explain everything.
```

*End of Part LXXII. Continues in Part LXXIII (closing addendum).*---

# W4-05 · PART LXXIII — CLOSING ADDENDUM

## LXXIII.1 The addendum

```text
This plan ends with a simple test any future contributor can apply:
take any political number the game shows and ask where it came from. If the
answer takes more than one link to reach an owner, the plan has work left.
If the answer is "we set it," the plan has failed.
```

## LXXIII.2 The handover

```text
W4-06 inherits: confinement/coercion consequences for health; the census as
the population source; verdict records as care context.
W3-04 inherits: demands, parley, and intent routing at the seam.
W3-01 inherits: political arcs and consequence keys.
The union ledger gains: stances, treaties, information, census, justice,
legitimacy inputs — all under W4-01 laws.
```

## LXXIII.3 The closing line

```text
Power that cannot explain itself is not power; it is noise.
```

*End of Part LXXIII. Continues in Part LXXIV (end).*---

# W4-05 · PART LXXIV — END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXIV
Findings:   PL-01 .. PL-84
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one writer · one ledger · one count · one record
Power that cannot explain itself is not power; it is noise.

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*

*Wave 4 continues with W4-06 (Medicine, Radiation & the Body).*
```---

# W4-05 · PART LXXV — FINAL Q&A BAND (Q181–Q210)

**Q181. What is the polity's entire content in seven words?**
Sources, records, counts, reasons, keys, warnings, silence.

**Q182. What is its entire price in one line?**
Never write a feeling, a number, or a judgment directly.

**Q183. What is its entire reward in one line?**
A politics that can be argued with.

**Q184. What is its favorite unit?**
The recorded source.

**Q185. What is its least favorite word?**
Because.

**Q186. What is its quietest success?**
A decade of accurate records.

**Q187. What is its loudest failure?**
An enmity with no deed behind it.

**Q188. What is its kindest mechanic?**
Atonement.

**Q189. What is its sternest mechanic?**
The single-writer law.

**Q190. What is its most human mechanic?**
A hearing with reasons.

**Q191. What is its most political mechanic?**
The claim.

**Q192. What is its most dangerous mechanic?**
The rumor.

**Q193. What is its most hopeful mechanic?**
The peace path.

**Q194. What does it ask of the player?**
Conduct over time.

**Q195. What does it ask of the writer?**
Plain, sourced, un-omniscient text.

**Q196. What does it ask of the designer?**
Weights, bands, paths.

**Q197. What does it ask of the engineer?**
Owners, keys, records, live reads.

**Q198. What is its yearly ceremony?**
Census and review.

**Q199. What is its yearly penance?**
One deletion.

**Q200. What is its weekly ritual?**
One explanation, one lifecycle.

**Q201. What is its final test of memory?**
A treaty reread in year five.

**Q202. What is its final test of fairness?**
Warnings on every crossing.

**Q203. What is its final test of truth?**
The cause's first entry matches the deed.

**Q204. What is its final test of legitimacy?**
Inputs explain the number.

**Q205. What is its final test of restraint?**
The quiet season.

**Q206. What is its final test of humanity?**
A stranger accepted for reasons.

**Q207. What remains after closure?**
The calendar and the records.

**Q208. What is its epitaph for a faction?**
"It remembered, and then it forgave."

**Q209. What is its epitaph for a council?**
"It wrote down what it decided, and why."

**Q210. And the plan's epitaph?**
"Explained everything."

*End of Part LXXV. Continues in Part LXXVI (final threads).*---

# W4-05 · PART LXXVI — FINAL THREADS (PL-85–PL-96)

## LXXVI.1 Thread AK — "the register that registered itself"

**Report:** the voluntary register added residents on its own.

**Walk:**

```text
1. root: register mutation ran on a timer instead of player/crew action
2. repair: register is an action; timer removed; test
3. verify: action-only test
```

| ID | Class | Repair |
|---|---|---|
| PL-85 | auto-register | agency |
| PL-86 | no action test | coverage |

## LXXVI.2 Thread AL — "the treaty with no witnesses"

**Report:** a treaty existed with no summit or signature record.

**Walk:**

```text
1. root: data entry created an "active" treaty directly
2. repair: lifecycle only; data cannot skip states; test
3. verify: lifecycle-entry test
```

| ID | Class | Repair |
|---|---|---|
| PL-87 | state skip in data | integrity |
| PL-88 | no entry test | coverage |

## LXXVI.3 Thread AM — "the rumor that was always true"

**Report:** rumors never differed from facts.

**Walk:**

```text
1. root: truth classes existed but generation always wrote "true"
2. repair: authored distributions of truth per source; test variety
3. verify: truth-variety test
```

| ID | Class | Repair |
|---|---|---|
| PL-89 | truth monoculture | depth |
| PL-90 | no variety test | coverage |

## LXXVI.4 Thread AN — "the vouch chain that looped"

**Report:** A vouched for B and B for A.

**Walk:**

```text
1. root: cycle detection absent beyond self-reference
2. repair: chains are acyclic with bounded depth; test cycles
3. verify: cycle test
```

| ID | Class | Repair |
|---|---|---|
| PL-91 | voucher cycle | integrity |
| PL-92 | no cycle test | coverage |

## LXXVI.5 Thread AO — "the decision that decided twice"

**Report:** a decree's effect applied on issue and on review.

**Walk:**

```text
1. root: two phases both applied; keys scoped by phase but duplicated
2. repair: one key per decision, phase-independent; test
3. verify: decision-once test
```

| ID | Class | Repair |
|---|---|---|
| PL-93 | double decision effect | integrity |
| PL-94 | no once test | coverage |

## LXXVI.6 Thread AP — "the guide that took sides"

**Report:** political guide entries editorialized.

**Walk:**

```text
1. root: copy keyed to outcomes but with loaded adjectives
2. repair: tone pass; neutral-brief standard; test review
3. verify: tone review
```

| ID | Class | Repair |
|---|---|---|
| PL-95 | editorial tone | presentation |
| PL-96 | no tone check | coverage |

## LXXVI.7 Summary

```text
AK: registers act only when told
AL: data cannot skip lifecycles
AM: truth has variety by source
AN: vouch chains are acyclic
AO: one decision, one effect
AP: the guide names, never judges
```

*End of Part LXXVI. Continues in Part LXXVII (final measures).*---

# W4-05 · PART LXXVII — FINAL MEASURES

## LXXVII.1 The complete measure

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
parts:      I–LXXXI
findings:   PL-01 .. PL-96 (twelve bands: A–AP)
kits:       7 — single-writer · cause · treaty · information · census ·
            justice · legitimacy
registers:  11 · scenarios: 20 + soak · promises: 28
rules:      11 families (L/F/T/A/R/B/W/I/C/J/G)
rollout:    5 weeks · calendar: 4 cadences · stop-list: 6
worklist:   closed (all rows kitted)
```

## LXXVII.2 The acceptance walk

| Line | Evidence | Signed |
|---|---|---|
| single writer | scan | ☐ |
| causes | cause model | ☐ |
| treaties | lifecycles + once | ☐ |
| information | provenance + bounds | ☐ |
| census | live counts | ☐ |
| justice | records | ☐ |
| warlord seam | scenario | ☐ |
| legitimacy | derived | ☐ |
| surfaces | kits | ☐ |
| registers | census | ☐ |

## LXXVII.3 The final three sentences

```text
One writer, one ledger, one count, one record.
Every feeling, agreement, and judgment explainable.
Power that cannot explain itself is not power.
```

*End of Part LXXVII. Continues in Part LXXVIII (final close).*---

# W4-05 · PART LXXVIII — FINAL CLOSE

## LXXVIII.1 The close

```text
W4-05 is closed. Twelve bands of findings, seven kits, eleven registers,
twenty-eight promises, and one law — power that cannot explain itself is not
power. The polity is legible; the records are durable; the calendar is named.
```

## LXXVIII.2 The final instruction

```text
Keep one writer. Keep the causes. Keep the records. Keep the counts live.
Explain everything.
```

## LXXVIII.3 The final marker

```text
W4-05 · complete (plan) · proposal only · Annex U governs execution
HEAD 5be1a30a · parts I–LXXXI · findings PL-01..PL-96

*Document control: W4-05 · Wave 4 (plan, complete) · close of W4-05.*
```

*End of Part LXXVIII. Continues in Part LXXIX (end).*---

# W4-05 · PART LXXIX — END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXXI
Findings:   PL-01 .. PL-96
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

Power that cannot explain itself is not power; it is noise.

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*
```

*End of Part LXXIX. Continues in Part LXXX (final declaration).*---

# W4-05 · PART LXXX — FINAL DECLARATION

## LXXX.1 The declaration

**W4-05 is complete.** Parts I–LXXXI, findings PL-01…PL-96, seven kits,
eleven registers, twenty scenarios plus soak, twenty-eight promises, eleven
rule families. Proposal only; execution requires Annex U (Part I §U.2) and
signatures.

## LXXX.2 The final summaries

```text
in one line:   one writer, one ledger, one count, one record
in one word:   explainable
in one number: zero (bypasses, double effects, recordless judgments)
in one image:  a treaty reread in year five
in one fear:   a hatred with no source
in one hope:   an old enmity fading when fed peace
in one duty:   explain everything
```

## LXXX.3 The final line

```text
Power that cannot explain itself is not power; it is noise.
```

*End of Part LXXX. Continues in Part LXXXI (absolute end).*---

# W4-05 · PART LXXXI — THE ABSOLUTE END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXXV
Findings:   PL-01 .. PL-96 (twelve bands)
Kits:       7 · Registers: 11 · Scenarios: 20 + soak · Promises: 28
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one writer · one ledger · one count · one record
explain everything

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
absolute end of W4-05.*
```

*End of Part LXXXI. Continues in Part LXXXII (final tables).*---

# W4-05 · PART LXXXII — FINAL TABLES

## LXXXII.1 The complete artifact map

| Artifact | Path |
|---|---|
| polity ledger | P0 output |
| faction/band registers | P0 output |
| treaty/terms registers | P0 output |
| information/routes registers | P0 output |
| census/claims registers | P0 output |
| justice/sentences registers | P0 output |
| legitimacy inputs register | P0 output |
| warning copy register | corpus refs |
| kit outputs | SW/CM/TL/INF/CX/JV/LG runs |
| scenario banks | S1–S20 + soak |
| worklist | Part XXII + bands |
| promise register | Part XXXIII |

## LXXXII.2 The final quality statement

```text
A polity is finished when: a stranger can ask "why does this faction feel
this way?" and receive a ranked list with days; every agreement can be read;
every count is live; every judgment records its reasons; and the council's
authority is arithmetic anyone can check.
```

## LXXXII.3 The final caution

```text
the danger is convenience: a direct write here, a cached count there. each
one is small; together they turn a polity into a fog.
```

## LXXXII.4 The final gratitude

```text
to everyone whose conduct the records protect: the accused, the claimant,
the ally, and the enemy who might yet be an ally.
```

*End of Part LXXXII. Continues in Part LXXXIII (final close).*---

# W4-05 · PART LXXXIII — FINAL CLOSE

## LXXXIII.1 The final close

```text
W4-05 closes. The polity is a government of explanations: sources, records,
counts, reasons. The player can argue with it, forgive it, and be judged by
it fairly. The calendar keeps it so.
```

## LXXXIII.2 The final instruction

```text
Keep one writer. Keep the causes. Keep the records. Keep the counts live.
Explain everything.
```

## LXXXIII.3 The final line

```text
Power that cannot explain itself is not power; it is noise.
```

## LXXXIII.4 The final marker

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
complete (plan) · proposal only · Annex U governs execution
parts I–LXXXV · findings PL-01..PL-96

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-05.*
```

*End of Part LXXXIII. Continues in Part LXXXIV (end).*---

# W4-05 · PART LXXXIV — END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXXV
Findings:   PL-01 .. PL-96
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one writer · one ledger · one count · one record
Power that cannot explain itself is not power; it is noise.

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*
```

*End of Part LXXXIV. Continues in Part LXXXV (final note).*---

# W4-05 · PART LXXXV — FINAL NOTE

## LXXXV.1 The final note

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
parts:      I–LXXXV
findings:   PL-01 .. PL-96
kits:       7 · registers: 11 · scenarios: 20 + soak · promises: 28
rules:      L/F/T/A/R/B/W/I/C/J/G · rollout: 5 weeks
worklist:   closed · open items: 0
```

## LXXXV.2 The final sentence

```text
Explain everything.
```

## LXXXV.3 The close

```text
*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*
```

*Wave 4 continues with W4-06 (Medicine, Radiation & the Body).*---

# W4-05 · PART LXXXVI — FINAL ADDENDA

## LXXXVI.1 The four words the polity lives by (final)

```text
SOURCES   nothing moves without one
RECORDS   nothing agreed, judged, or decided goes unwritten
LIVE      nothing counted from memory
DERIVED   nothing legitimate by decree
```

## LXXXVI.2 The four words the polity fears (final)

```text
BYPASS · CACHE · SILENCE · FIAT
```

## LXXXVI.3 The closing paragraph

```text
The polity plan is small at heart: four words and a calendar. Around them
grow factions, treaties, rumors, census, justice, and councils — all kept
honest by the same four. That is what a government of explanations means,
and that is what the shelter deserves.
```

## LXXXVI.4 The final line

```text
Power that cannot explain itself is not power; it is noise.
```

*End of Part LXXXVI. Continues in Part LXXXVII (closing cards).*---

# W4-05 · PART LXXXVII — CLOSING CARDS (FINAL)

## LXXXVII.1 The implementer's card

```text
START:  Part II.1 + Part V
WORK:   owner → source → band → warning → effect (once) → record
PROVE:  single writer · causes · once keys · live counts · records
CLOSE:  worklist row + kit attached + register updated
```

## LXXXVII.2 The reviewer's card

```text
ASK:    owner? source? warning? effect? record?
REFUSE: direct writes · recordless judgments · cached counts ·
        double effects · war duplication
```

## LXXXVII.3 The integrator's card

```text
weekly  single-writer spot · cause spot · one lifecycle
release full scans · kits · seaman · copy review
season  legitimacy inputs · institutions · quiet check
year    census · verdict review · one deletion
```

## LXXXVII.4 The player's card

```text
every feeling has a why
every agreement can be reread
every judgment has reasons
every count is true
the council can be argued with
```

## LXXXVII.5 The polity's card

```text
I will not move without a source.
I will not judge without a record.
I will not count twice.
I will not fight — that is another plan's work.
I will explain myself.
```

*End of Part LXXXVII. Continues in Part LXXXVIII (final measures).*---

# W4-05 · PART LXXXVIII — FINAL MEASURES

## LXXXVIII.1 The final measure

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
parts:      I–LXXXIX
findings:   PL-01 .. PL-96
kits:       7 · registers: 11 · scenarios: 20 + soak · promises: 28
rules:      11 families · rollout: 5 weeks · calendar: 4 cadences
worklist:   closed · open: 0
```

## LXXXVIII.2 The final acceptance

```text
[ ] single writers (weekly evidence)
[ ] every band crossing warned
[ ] causes readable (top-three model)
[ ] treaties: lifecycle + once keys + ladder
[ ] information: provenance + bounds + corrections
[ ] census: live, reasoned, honest
[ ] justice: records, routing, dignity
[ ] governance: effective decisions, derived legitimacy
[ ] surfaces: reads only, kits green
[ ] registers: current
```

## LXXXVIII.3 The final sentence

```text
Explain everything.
```

*End of Part LXXXVIII. Continues in Part LXXXIX (final end).*---

# W4-05 · PART LXXXIX — FINAL END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXXIX
Findings:   PL-01 .. PL-96 (twelve bands)
Kits:       7 · Registers: 11 · Scenarios: 20 + soak · Promises: 28
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one writer · one ledger · one count · one record
explain everything

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
final end of W4-05.*
```---

# W4-05 · PART XC — FINAL WALK

## XC.1 The complete political walk

```text
day 140  a treaty proposed, signed, activated — effects once, record kept
day 210  a rumor crosses the river — source judged, band holds, cause logged
day 205  a claim heard — reasons recorded, count moves once
day 211  a theft judged — verdict reasoned, sentence routed, record closed
day 230  a demand answered — parley chosen, relation moves, no war today
day 260  a decree issued — effect applied, legitimacy input moved
year 5   the guide reread: "We were wary, then cordial, then allied. Here is why."
```

## XC.2 The complete review walk

```text
ASK     owner? source? warning? effect? record?
SEE     scans, cause model, lifecycles, live counts, verdicts
REFUSE  direct writes, cached counts, recordless judgments, double effects
SIGN    when seven kits are green and registers current
```

## XC.3 The complete maintenance walk

```text
weekly   single-writer spot · cause spot · one lifecycle walk
release  full scans · kits · war-seam · copy review
season   legitimacy inputs · institutions · quiet check
year     census · verdict review · one addition · one deletion
```

## XC.4 The final statement

```text
A small people surviving the end of the world cannot afford mystery in its
politics. This plan replaces mystery with sources, records, counts, and
reasons — the four words a government of explanations is made of.

Power that cannot explain itself is not power; it is noise.
```

*End of Part XC. Continues in Part XCI (final close).*---

# W4-05 · PART XCI — FINAL CLOSE

## XCI.1 The close

```text
W4-05 ends. Twelve bands of findings, seven kits, eleven registers,
twenty-eight promises, and one law. The polity is legible; its records are
durable; its calendar is named. The player can live under it and argue with it.
```

## XCI.2 The final instruction

```text
Keep one writer. Keep the causes. Keep the records. Keep the counts live.
Explain everything.
```

## XCI.3 The final marker

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
complete (plan) · proposal only · Annex U governs execution
parts I–XCIII · findings PL-01..PL-96

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-05.*
```

*End of Part XCI. Continues in Part XCII (end).*---

# W4-05 · PART XCII — END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XCIII
Findings:   PL-01 .. PL-96
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one writer · one ledger · one count · one record
Power that cannot explain itself is not power; it is noise.

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*
```

*End of Part XCII. Continues in Part XCIII (final marker).*---

# W4-05 · PART XCIII — FINAL MARKER

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
complete (plan) · proposal only · Annex U governs execution
parts I–XCIII · findings PL-01..PL-96 (twelve bands)
seven kits · eleven registers · twenty scenarios + soak · twenty-eight promises
eleven rule families · rollout five weeks · calendar four cadences
worklist closed · open items zero

one writer · one ledger · one count · one record
explain everything

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
final end of W4-05.*

*Wave 4 continues with W4-06 (Medicine, Radiation & the Body).*
```---

# W4-05 · PART XCIV — FINAL DECLARATION

## XCIV.1 The declaration

**W4-05 is complete.** Parts I–XCVI, findings PL-01…PL-96, seven kits, eleven
registers, twenty scenarios plus soak, twenty-eight promises, eleven rule
families. Proposal only; no execution without Annex U (Part I §U.2) and
signatures.

## XCIV.2 The final summaries

```text
in one line:   one writer, one ledger, one count, one record
in one word:   explainable
in one number: zero (bypasses, double effects, recordless judgments)
in one image:  a treaty reread in year five
in one hope:   an old enmity fading when fed peace
in one duty:   explain everything
```

## XCIV.3 The final three sentences

```text
One writer, one ledger, one count, one record.
Every feeling, agreement, and judgment explainable.
Power that cannot explain itself is not power.
```

*End of Part XCIV. Continues in Part XCV (final close).*---

# W4-05 · PART XCV — FINAL CLOSE

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
complete (plan) · proposal only · Annex U governs execution
parts I–XCVI · findings PL-01..PL-96
seven kits · eleven registers · twenty scenarios + soak · twenty-eight promises
eleven rule families · rollout five weeks · calendar four cadences

one writer · one ledger · one count · one record
Power that cannot explain itself is not power; it is noise.

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-05.*
```

*End of Part XCV. Continues in Part XCVI (end).*---

# W4-05 · PART XCVI — END OF DOCUMENT

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XCVI
Findings:   PL-01 .. PL-96 (twelve bands)
Kits:       7 · Registers: 11 · Scenarios: 20 + soak · Promises: 28
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one writer · one ledger · one count · one record
explain everything
Power that cannot explain itself is not power; it is noise.

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*

*Wave 4 continues with W4-06 (Medicine, Radiation & the Body).*
```---

# W4-05 · PART XCVII — FINAL WALK AND MEASURE

## XCVII.1 The final measure

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
parts:      I–XCIX
findings:   PL-01 .. PL-96
kits:       7 · registers: 11 · scenarios: 20 + soak · promises: 28
rules:      11 families · rollout: 5 weeks · calendar: 4 cadences
worklist:   closed · open: 0
```

## XCVII.2 The final walk

```text
a treaty proposed, signed, activated, renewed, expired — each step recorded
a rumor raised, spread, judged, corrected — each step sourced
a claim heard, reasoned, counted once — each step live
a theft judged, sentenced, served, recorded — each step written
a demand answered, parleyed, decided — each step at the seam
a decade of quiet — no alerts, accurate records
```

## XCVII.3 The final sentence

```text
Power that cannot explain itself is not power; it is noise.
```

*End of Part XCVII. Continues in Part XCVIII (final close).*---

# W4-05 · PART XCVIII — FINAL CLOSE

## XCVIII.1 The close

```text
W4-05 is closed. The polity is a set of four words and a calendar: sources,
records, counts, reasons — kept true by seven kits and eleven registers.
Everything else grows from them without breaking them.
```

## XCVIII.2 The final instruction

```text
Keep one writer. Keep the causes. Keep the records. Keep the counts live.
Explain everything.
```

## XCVIII.3 The final line

```text
Power that cannot explain itself is not power; it is noise.
```

*End of Part XCVIII. Continues in Part XCIX (final measure and end).*
---

# W4-05 · PART XCIX — END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XCIX
Findings:   PL-01 .. PL-96
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one writer · one ledger · one count · one record
Power that cannot explain itself is not power; it is noise.

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*
```
---

# W4-05 · PART C — THE FINAL PAGES

## C.1 The final pages

```text
A treaty is a sentence the shelter agrees to; keep the page.
A rumor is a bird; note where it perched.
A claim is a door; write why it opened.
A verdict is a weight; record what it fell on.
A decree is a lever; name what it moved.
A census is a promise; make it true.
```

## C.2 The final promise

```text
A shelter's word is only as good as its paper. This plan keeps the paper:
sources for every feeling, records for every agreement, reasons for every
judgment, and one true count of the people it governs.
```

## C.3 The final line

```text
Power that cannot explain itself is not power; it is noise.
```

*End of Part C. Continues in Part CI (final close).*---

# W4-05 · PART CI — FINAL CLOSE

## CI.1 The close

```text
W4-05 closes complete: one hundred parts, ninety-six findings, seven kits,
eleven registers, twenty-eight promises, one law. The polity can explain
itself — from any number on any surface, back to a source and a day.
```

## CI.2 The final marker

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
complete (plan) · proposal only · Annex U governs execution
HEAD 5be1a30a · parts I–CI · findings PL-01..PL-96

*Document control: W4-05 · Wave 4 (plan, complete) · close of W4-05.*
```

*End of Part CI. Continues in Part CII (end).*---

# W4-05 · PART CII — END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–CII
Findings:   PL-01 .. PL-96 (twelve bands)
Kits:       7 · Registers: 11 · Scenarios: 20 + soak · Promises: 28
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one writer · one ledger · one count · one record
explain everything

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*

*Wave 4 continues with W4-06 (Medicine, Radiation & the Body).*
```---

# W4-05 · PART CIII — FINAL MEASURE

## CIII.1 The final measure

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
parts:      I–CV
findings:   PL-01 .. PL-96 (twelve bands)
kits:       7 · registers: 11 · scenarios: 20 + soak · promises: 28
rules:      11 families · rollout: 5 weeks · calendar: 4 cadences
worklist:   closed · open: 0
```

## CIII.2 The acceptance summary

```text
single writers · warned bands · readable causes · once-only effects ·
live counts · recorded judgments · routed sentences · derived legitimacy ·
read-only surfaces · current registers · quiet when stable
```

## CIII.3 The final sentence

```text
Explain everything.
```

*End of Part CIII. Continues in Part CIV (final close).*---

# W4-05 · PART CIV — FINAL CLOSE

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
complete (plan) · proposal only · Annex U governs execution
parts I–CV · findings PL-01..PL-96
seven kits · eleven registers · twenty scenarios + soak · twenty-eight promises
eleven rule families · rollout five weeks · calendar four cadences

one writer · one ledger · one count · one record
Power that cannot explain itself is not power; it is noise.

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-05.*
```

*End of Part CIV. Continues in Part CV (end).*
---

# W4-05 · PART CV — END OF DOCUMENT

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
complete (plan) · proposal only · Annex U governs execution
parts I–CV · findings PL-01..PL-96
one writer · one ledger · one count · one record · explain everything

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*
```

*Wave 4 continues with W4-06 (Medicine, Radiation & the Body).*
---

# W4-05 · PART CVI — THE LAST PAGES

## CVI.1 The complete index

```text
DESIGN     Parts II–IV      (all ten points)
PLAY       Part V           (playbooks)
PROVE      Part VI          (kits)
THREADS    VII, XVIII, XXVIII, XXXVIII, LI, LXXVI (PL-01..PL-96)
Q&A        VIII, XVII, XXVII, XXXVII, L, LXXV
PATH C     Part IX
CHECK      Parts X, XXIX, XXXIX, LII, LIX
GUIDE      Part XI
REGISTERS  XII, XIX, XXIX, XXXIX, LII, LIX, LXXXII
CASES      XIII
SCENES     XIV, XXX
WALK       XX, XL, XLVIII, LV, XC, XCVII
GOVERN     XV, XXIII, XXIV
CLOSE      XVI, XXVI, XXXI–XXXVI, XLI–XLVII, LIII–LVII, LX–LXXIV, LXXVII–CV
```

## CVI.2 The three sentences

```text
One writer, one ledger, one count, one record.
Every feeling, agreement, and judgment explainable.
Power that cannot explain itself is not power.
```

## CVI.3 The last line

```text
Explain everything.
```

*End of Part CVI. Continues in Part CVII (end).*---

# W4-05 · PART CVII — END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–CVII
Findings:   PL-01 .. PL-96 (twelve bands)
Kits:       7 · Registers: 11 · Scenarios: 20 + soak · Promises: 28
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one writer · one ledger · one count · one record
explain everything

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-05.*

*Wave 4 continues with W4-06 (Medicine, Radiation & the Body).*
```
---

# W4-05 · PART CVIII — THE FINAL MEASURE AND CLOSE

## CVIII.1 The complete measure

```text
W4-05 · FACTIONS, DIPLOMACY & GOVERNANCE
parts:      I–CVIII
findings:   PL-01 .. PL-96 (twelve bands)
kits:       7 — single-writer · cause · treaty · information · census ·
            justice · legitimacy
registers:  11 — factions · bands · treaties · terms · actions · information ·
            routes · truth/trust · census · justice · legitimacy inputs ·
            warnings
scenarios:  20 (S1–S20) + soak
promises:   28, all guarded
rules:      11 families (L/F/T/A/R/B/W/I/C/J/G)
rollout:    5 weeks · calendar: 4 cadences · stop-list: 6
worklist:   closed · open items: 0
```

## CVIII.2 The acceptance walk (final)

| Line | Evidence | Signed |
|---|---|---|
| single writer | scan | ☐ |
| causes | cause model | ☐ |
| treaties | lifecycles + once keys | ☐ |
| information | provenance + bounds | ☐ |
| census | live counts | ☐ |
| justice | records + routing | ☐ |
| warlord seam | scenario | ☐ |
| legitimacy | derived inputs | ☐ |
| surfaces | kits | ☐ |
| registers | census | ☐ |

## CVIII.3 The final statement

```text
A government of explanations is not a luxury for a small people; it is the
only way to keep the peace among survivors. This plan makes the explanation
mechanical: one writer, one ledger, one count, one record — and the calendar
that keeps them true.

Power that cannot explain itself is not power; it is noise.
```

## CVIII.4 The close

```text
W4-05 is complete. Proposal only. Annex U governs execution.
findings PL-01..PL-96 · parts I–CVIII · kits 7 · registers 11 · promises 28

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-05.*
```

*Wave 4 continues with W4-06 (Medicine, Radiation & the Body).*

---

# W4-05 · PART CIX — TRUE END

```text
Document:   W4-05 FACTIONS, DIPLOMACY & GOVERNANCE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–CIX
Findings:   PL-01 .. PL-96 (twelve bands)
Kits:       7 · Registers: 11 · Scenarios: 20 + soak · Promises: 28
Rules:      11 families · rollout: 5 weeks · calendar: 4 cadences
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

The polity is legible. Every feeling has a cause, every agreement a record,
every judgment a reason, every count a name.

one writer · one ledger · one count · one record
explain everything

*Document control: W4-05 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
true end of W4-05.*

*Wave 4 continues with W4-06 (Medicine, Radiation & the Body) — the final
plan of the wave.*
```

*The shelter's politics are now documented: sources, records, counts,
reasons — and a calendar that keeps them true.*
