# Plan 44 — Relations That Change Outcomes: Affinity With Consequences

> **Wave:** Continuity Wave 6 — *The People In It* (closing plan)
> **Depends on:** 40A (who they are), 41A (what they lost), 43A/B (what was decided about them),
> 24A/24B (the two channels relations should act through), 20A (where they were sent).
> **Coordination:** parallel plans 132 (agendas), 147 (per-NPC memory), 150 (romance/family),
> 144 (autonomy) author relationship *content*. This plan builds the one consumer path they all
> otherwise invent separately.
>
> **Theme:** affinity is computed in three places, displayed in one, and read by **nothing**.
> `IdeologicalFrictionSystem` drains or gains affinity per day, `TraumaBondSystem` grants a
> +15 bond bonus, `SurvivorRelationsSystem` stores pair state, the coordinator pushes a read model
> to `SurvivorRelationsPanel` — and no duty assignment, expedition party, ration line, care plan,
> or production result anywhere consults it. Two people who hate each other split a shift exactly
> as well as two people who would die for each other.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | Relations write path is real | `SurvivorSocialCoordinator.cs:66–70` holds `NeedsSystem`, `SurvivorRelationsSystem`, `DutyRosterSystem`, `ISeededRng`; `:121–140` wires TraumaBond→`AdjustAffinity` + `AreOnSameShift` (roster → social), IdeologicalFriction `OnAffinityChanged`→Relations, RationConflict `OnMoraleDelta`→needs |
| 2 | Friction drifts affinity daily with authored constants | `IdeologicalFrictionSystem.cs:30–31` `ConflictAffinityDrainPerDay = 2f`, `SynergyAffinityGainPerDay = 1f`; `:53 OnAffinityChanged`; `:70 GetAffinity(a,b)`; `:128–136` applies per-hours drift |
| 3 | Trauma bonds grant a fixed bonus | `TraumaBondSystem.cs:46 BondAffinityBonus = 15f`, and the coordinator supplies same-shift truth from the roster |
| 4 | **No mechanical reader exists outside the social cluster** | `grep -rn "Affinity" Assets/Ashfall.Core src/` → every hit is inside `IdeologicalFrictionSystem.cs` / `TraumaBondSystem.cs` / relations internals or the `AdjustAffinity` wiring. Nothing in `DutyRoster/*`, `Expeditions/*`, `Medical/*`, `Greenhouse/*`, `Crafting/*`, or `src/UI` (beyond the read model) reads a pair value |
| 5 | The display half works | `src/Main.SurvivorSocial.cs:57,122` → `SurvivorRelationsPanel.SetSocialReadModel(BuildReadModel())` — so relations are *shown* while doing nothing |
| 6 | One relations-adjacent surface is decorative | `TraumaBondingCohortPanel` is in Wave 1's 30-console set (`docs/debug/10LOOP_player_ui_ux_BUG_AUDIT.md` BUG-UI-002: bound to no authority) — the one panel that should explain bonds is the one with no system |
| 7 | The consumer channels already exist | 24A `FitnessVerdict` (can this pair work?), 24B `NeedsModifierStack` (does working together help?), 35C production-with-labour (does crew composition change yield?), 20A exposure (who covers for whom?), 22B caregiving/convalescent diets |
| 8 | Cohabitation already matters structurally | bunks/`ShelterDecor`, `caregiving` route, `apprenticeship` (mentor/apprentice), `Generational.BookChild(childId, parentIds, guessBand, …)` (41C) — relationships of four kinds exist as data with no shared consumer |
| 9 | Roster→social already crosses the boundary in one direction | `SurvivorSocialCoordinator.cs:126` `TraumaBond.AreOnSameShift = (a,b) => …` reads the roster — proving the wiring style; the reverse (social→roster) is simply absent |
| 10 | Registry recommends it, atlas demands it | `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` §27: "Trauma Bonding & Co-Shift Buffs → Create bonded-pair narrative dilemmas"; atlas §11 lists guilt/relations as High-Leverage state that should drive behaviour |

**Reading:** the model is built, the plumbing runs one way, and the payoff is missing. Every step
below is a consumer added to an existing channel — no new relationship system, no affinity rework.

---

## Task 44A — Read affinity where work is decided

**Goal:** make pair state change the two decisions the player makes constantly — who works with whom,
and who goes where.

**Files:** `DutyRosterAssignmentEngine.cs` (+ `DutyRosterSystem`, `DutyRosterHoldfastBridge`),
`src/Host/ExpeditionHostSession.cs` (party validation), `ExpeditionSystem` (risk/estimate),
`SurvivorRelationsSystem.cs` (query surface), `src/UI/DutyRosterPanel.cs`,
`src/UI/ExpeditionPanel.cs` / `ExpeditionRadarPanel.cs`, new `docs/systems/RELATION_EFFECTS.md`.

### Substeps

1. **Publish a single query API** — `Relations.EffectOf(a, b) → {band, workingModifier,
   riskModifier, noteKey}` — and forbid direct `GetAffinity` calls from consumers, so pair maths
   lives in one place (the same single-authority rule used throughout Wave 2).
2. **Bands, not numbers**: define authored bands (Hostile / Strained / Cordial / Close / Bonded) with
   thresholds in data, and present them in words — the player should never see a bare affinity float.
3. **Duty assignment consults it**: co-assigned pairs get a productivity/morale/error effect through
   24B's stack; a hostile pair on the same shift raises mistake probability — and the preview shows
   it before confirming (24A's warn-don't-block rule).
4. **Expedition party composition consults it**: bonded pairs reduce each other's panic/failure
   risk; hostile pairs raise encounter-handling risk; a bereaved survivor paired with who they lost
   behaves differently (41A grief state + this band).
5. **Wire it into `ExpeditionSystem.Estimate`** as a displayed term alongside fuel/dose/risk
   (32B), so the dispatch screen shows *who should go together* as a number the player can argue with.
6. **Separation has a cost**: a bonded pair split across shifts or by a long expedition accrues
   fatigue/morale pressure — authored drift, not a hidden timer, and reported (31).
7. **Caregiving consults it**: a patient recovers differently with a close visitor/nurse
   (`caregiving` route + `MedicalWardSystem` staff assignments), giving the ward a reason to schedule
   people the player knows.
8. **Apprenticeship consults it**: mentor/apprentice pairing quality reads band + belief (40A) —
   the skill-transfer channel Wave 5's 35C step 3 needs anyway.
9. **Make `TraumaBondingCohortPanel` real or retire it** (Wave 1's 16A): it is the natural home for
   the bond view; give it the coordinator's read model and one mutating action, or keep it shelved —
   the choice must be recorded, not left decorative.
10. **No double-counting**: relations effects must be distinguishable from fatigue/fitness effects in
    the attribution (31) — a mistake blamed on exhaustion must not also be blamed on a grudge.
11. **Determinism**: all pair-driven rolls on `ISeededRng` streams; a 100-day identical-assignment
    replay must be digest-identical.
12. **Tests**: band derivation, duty preview accuracy, expedition estimate term, split-pair drift,
    caregiving/apprenticeship effects, one-negative-test-per-channel (a hostile pair must *not*
    change unrelated systems), determinism, and a query-only test asserting no consumer reads raw
    affinity.
13. **Docs**: `docs/systems/RELATION_EFFECTS.md` — the channel table (band → effect → where shown).
14. **Run the checklist** + `--expedition-selftest` + `triad-drift-gate.sh`.

**DoD:** pair state changes shift tables and party lists, visibly and before the fact.

---

## Task 44B — Relations as memory and story: what pairs accumulate

**Goal:** relationships should have a history the player can discover and a story hook that content
waves can hang on (parallel 132/147/150), using the record layer from 41A/41B rather than a new one.

**Files:** `SurvivorRelationsSystem.cs` (event log per pair), `JournalSystem`,
`MemorialSystem`/`DeathQuality`, `GuiltInsomniaSystem`, `TraumaBondSystem`, `wall_carving_templates`
+ `confession_secrets` (dead catalogs — Wave 6's 41A), 42A voice line bank, standing record,
`docs/narrative/`.

### Substeps

1. **Give each pair a small, bounded history**: what happened between them (shared shift, saved a
   life, a refused ration, a funeral attended) with day, cause, and delta — so affinity becomes
   *explainable*, not just a decaying float.
2. **Bound the record** (Wave 5's 39B retention): N entries per pair, rolled into the standing
   record past that — a 400-year campaign must not grow pair logs without limit.
3. **Surface the "why"**: the relations panel shows the top contributing events per pair (click
   through with 31B's routing), so the player can act on knowledge rather than guess at a number.
4. **Confessions and grudges get a channel**: `confession_secrets.json` (8 defs with
   forgiveness/grudge outcomes) is currently dead content — wire it as a pair-event source (a
   disclosed secret writes history and shifts band), which is exactly the "narrative dilemma" the
   registry recommends.
5. **Carvings and memorials as pair artefacts**: `wall_carving_templates.json` (morale-band gated)
   becomes pair/mourning expression through 41A's memorial pipeline.
6. **Voice carries it**: 42C's "about other people" lines read pair history — the player hears a
   relationship, not just sees it.
7. **Bereavement is a state, not a deletion**: on a partner's death, the survivor keeps an explicit
   bereaved state (fitness, grief, and band transition to memory) rather than an affinity entry
   pointing at a corpse — and the memorial keeps the pair.
8. **Family**: children with `BookChild(parentIds, guessBand, …)` (41C) read as parent/child bands
   with mechanical consequences (dose priority, ration fairness, mourning weight) — the *mechanics*
   only; romance/family content design belongs to parallel 150.
9. **No stat-sheet romance**: no affection meters, no dating loop; relationships are obligations,
   dependencies, and things that go wrong under scarcity.
10. **Test the explainability**: assert any band change has at least one traceable history entry —
    that single property is what stops relations drifting back into invisible arithmetic.
11. **Tests**: history write/read, bounded retention, confession → band change, bereavement
    transition, carving/memorial artefacts, determinism of an event sequence, save round-trip.
12. **Run the checklist** + narrative gates (`ashfall-narrative-continuity`, `ashfall-dialog-graph-lint`).

**DoD:** every relationship has a reason, a record, and a voice.

---

## Task 44C — Close Wave 6: the identity → memory → voice → consent → outcome loop, proven

**Goal:** a single journey test and a short status report proving the inner-life layer is now one
connected system instead of five islands with a panel each.

**Files:** new `src/Host/InnerLifeJourneySelfTest.cs` (verb), `Ashfall.Core.Tests/
SurvivorInnerLifeIntegrationTests.cs`, `docs/systems/INNER_LIFE_STATUS.md`,
`artifacts/inner-life-report.json` (gitignored), Wave 3's 29A/29B doc updates,
`docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` §26/§27 rows, `AGENTS.md` known-issues rows.

### Substeps

1. **Declare the loop as an acceptance test**: authored belief (40A) → friction drift (44A) →
   policy adopted (43B) → grievance voiced (42C) → grief applied on a death (41A) → band change with
   history (44B) → duty/expedition outcome changed (44A) → ending input recorded (19A) → legacy
   ledger line (34C). One seeded run, one assertion per arrow.
2. **Produce a report** (`artifacts/inner-life-report.json`): for each arrow — implemented? bound?
   observed in a 100-day soak? surfaced to the player? — the same "presence ≠ liveness" question,
   answered per arrow, and pasted into the status doc.
3. **Reuse the port contract** (36A): each arrow is a declared seam; the gate must know about all of
   them, so a future unplugging fails CI.
4. **Coverage per arrow** (Wave 3's 27B): no arrow may rely on a test-only seam — the `ApplyGrief`
   lesson generalised.
5. **Fix the docs this wave invalidated**: registry rows for "Trauma Bonding & Co-Shift Buffs",
   "Ideological Friction", "Leadership Stress & Burnout", "Caregiving", and atlas §11's
   *"Orphan State (Underconnected)"* / *"Hidden State (Weak Feedback)"* classifications — each
   updated with file:line evidence (29B's format).
6. **`AGENTS.md` known-issue rows**: H2 (`WornGear` duplicate — Wave 2's 21B may have retired the
   bridge), H5/H11 (already disproved), H7 (Wave 3's 28B), and add the identity/tags rule from
   40A/40B ("authored, not inferred; tags, not id lists") as a *rule*, since agents keep writing the
   opposite.
7. **Retire the parallel-plan landmines**: add a coordination note to `docs/roadmap/README.md`
   (Wave 3's 29C) listing which seams 132/144/147/148/150/154/159 must now use instead of inventing
   their own — that note is what prevents this wave being undone by the next content wave.
8. **Balance sanity**: a 200-day soak with relations fully active must not make any assignment
   strictly dominant — if "always pair bonded survivors" is optimal, tune bands rather than adding
   mechanics.
9. **Accessibility**: all relation information has a text path (no colour-only bands) and is
   keyboard-reachable (37B).
10. **Snapshots**: relations, duty roster, expedition dispatch, memorial, and caregiving surfaces at
    representative states.
11. **Publish wave-close metrics** in the Wave 6 index (below) and mark plans 40–44 statuses in the
    wave ledger (29C).
12. **Run the full checklist** + `verify-fast.sh` + the release gate (Wave 5's 39A).

**DoD:** one seeded run proves the inner-life loop closes end to end, and the docs say so with
evidence.

---

## Cross-Task Dependencies (and the wave's shape)

```
40A identity ──► 41A memory ──► 42A/42B/42C voice ──► 43A/43B/43C governance
      └────────────────┬────────────────────────────────┘
                       ▼
                 44A outcomes (duty, expedition, care, training)
                       ▼
                 44B history/records ──► 44C proof + docs
   every arrow is a declared seam under 36A's port contract
```

**Wave 6 order:** 36A → 40A → 40B → 41A → 42A → 42B → 43A → 44A → 41B → 42C → 43B → 44B → 41C →
43C → 44C. **If only three tasks run: 40A, 41A, 44A** — real identity data, a death that changes the
living, and relationships that alter the shift table.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. python3 scripts/ci/generate-port-contract.py --check          # 36A
7. godot --headless --path . -- --survivors-selftest             # social/needs effects
8. godot --headless --path . -- --expedition-selftest            # party composition term
9. inner-life journey verb + 100/200-day soak report             # 44C
10. ashfall-narrative-check / -continuity / dialog-graph-lint
11. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | UI | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 44A | 1 query API | 3–4 | 1 (bands) | 3 | 12–16 | Medium | MEDIUM (assignment value shifts) |
| 44B | 1–2 | 2 | 2 revived catalogs | 2 | 10–14 | Medium | LOW |
| 44C | 0 | 1 verb | 0 | 0 | 6–10 + report | Low–Med | LOW |

**Guardrails:** no affection meters, no romance loop design, no new relationship system, no
unbounded pair logs, no affinity read directly by consumers (query API only), no colour-only bands,
and no arrow in the loop that isn't asserted by 44C's journey test — an unevidenced connection is
exactly how Waves 1–5 found "systems that exist and never meet".
