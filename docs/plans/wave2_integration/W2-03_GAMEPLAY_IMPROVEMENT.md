# ASHFALL — WAVE 2 INTEGRATION PROGRAM · PLAN 3 OF 6

# GAMEPLAY IMPROVEMENT & SURVIVAL-LOOP INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W2 (six-plan integration wave)
**Document:** W2-03 · part A of D
**Target size:** ~150,000 characters (this plan)
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W2-01 (maintenance), W2-02 (bugs), W2-04 (environments), W2-05 (locations), W2-06 (enrichment)
**Plan-unblocking annex:** Annex U at the end — deliberately separated per the Wave 2 rule.

---

## 0. How to read this plan

This plan improves how the game **plays**: pressure curves, feedback, choices,
crises, progression, and the campaign arc. It never invents a new authority:
every lever is a value, a policy, or a consumer bind on an existing owner
(`NeedsSystem`, difficulty scalars, economy, radiation, medical, power, water,
weather). Every change is measurable and reversible.

### 0.1 Two selection levels

| Plan Path | Name | Meaning |
|---|---|---|
| **A** | Tune & Telegraph | data-value tuning plus clearer feedback; no new consumers |
| **B** | Deepen the Loop | tuning + new consumer binds on existing owners + foresight surfaces |
| **C** | Campaign Arc Redesign | pacing, structure, and long-arc work across the campaign; largest |

**Level 2:** ten points, each A/B/C, selectable individually (Appendix A).

### 0.2 Default mapping

| Plan Path | A points | B points | C points |
|---|---|---|---|
| A Tune & Telegraph | 1–10 | — | — |
| B Deepen the Loop | 1,5 | 2,3,4,6,7,9 | 8,10 |
| C Campaign Arc | — | 3,6 | 1,2,4,5,7,8,9,10 |

### 0.3 The Wave 2 rule for this plan

> **Every gameplay lever must name its owner and its measurement.** No tuning
> without a before/after number; no new UI without a Core value behind it; no
> new system where a scalar or consumer will do. Content (environments,
> locations, prose) belongs to W2-04/05/06.

### 0.4 Measurement vocabulary

| Term | Meaning |
|---|---|
| pressure curve | how scarcity intensifies over campaign days |
| time-to-threat | in-game hours/days before a shortfall becomes harm |
| telegraph | the game's warning before an irreversible outcome |
| lever | one authored value or policy function |
| soak | a seeded multi-day run producing metrics |
| band | an intended range (e.g., first crisis within days 8–14) |

---

## 1. Executive summary

ASHFALL's survival core is complete: `NeedsSystem` owns nine needs
(`Hunger, Thirst, Fatigue, Warmth, Morale, Health, Hygiene, Numbness,
RadiationAnxiety`), difficulty presets already scale nine economy/survival
multipliers, a hardcore economy tuning catalog exists, nutrition profiles,
diseases, radiation phases, power/water chains, weather multipliers, and a
death path all exist. The improvements that matter are therefore **pacing,
clarity, and choice** — not missing systems.

Verified levers and evidence:

- `difficulty_presets.json` (schema_version 1) defines `difficulty_sparing`
  and `difficulty_standard` with scalars: `hunger_rate_mult`, `thirst_rate_mult`,
  `radiation_gain_mult`, `disease_onset_mult`, `hostile_encounter_mult`,
  `market_price_mult`, `equipment_decay_mult`, `crisis_deadline_mult`.
- `NeedsSystem` exposes a single canonical mutation surface: `Modify`,
  `SetExternalModifier`/`RemoveExternalModifier`/`ClearExternalModifiers`,
  `ApplyAttributedDelta`, plus `SetHealth`, `AdjustHealth`, `ForceDeath`,
  `NotifyNeedsRestored` — a rich, disciplined API for tuning consequences.
- `hardcore_economy_tuning.json` exists as a tuning authority.
- Weather already multiplies outdoor radiation (`FalloutStormOutdoorRadModifier
  = 150f`, `BlackRainOutdoorRadModifier = 250f` with
  `BlackRainHazmatMeltMultiplier = 5f` in `WeatherSystem`).
- The wave-1 XP work proved the difficulty **catalog and binding** are live
  (XP-01 done), so this plan builds on a working difficulty spine rather than
  inventing one.

The plan's ten points are ordered by player impact: pacing first, failure
telegraphing second, difficulty consequence weave third, economy pressure
fourth, progression fifth, feedback sixth, onboarding seventh, crisis/failure
eighth, late game ninth, agency density tenth.

---

## 2. Verified current state (gameplay evidence)

### 2.1 Needs and health

| Fact | Evidence |
|---|---|
| nine need kinds | `Assets/Ashfall.Core/Survivors/NeedsSystem.cs:10–21` |
| canonical mutation API | `Modify` (:198/:299), `SetExternalModifier` (:223), `ApplyAttributedDelta` (:237), `SetHealth` (:367), `AdjustHealth` (:375), `ForceDeath` (:358) |
| restful-sleep path exists | `NotifyNeedsRestored` (:381) |
| modifier stack | `NeedsModifierStack` (:87) used by events/policies |

**Observation:** the API is event-friendly (`sourceId` on external modifiers)
but the plan must verify which **presentation surfaces** explain changes to the
player (Point 6).

### 2.2 Difficulty spine

| Fact | Evidence |
|---|---|
| preset catalog | `Assets/StreamingAssets/Data/difficulty_presets.json` |
| sparing/standard scalars | nine multipliers listed in §1 |
| starting bonus items | `starting_bonus_item_ids: ["canned_food", "iodine_pills"]` |
| difficulty authority | XP-W1 claim previously DONE per the wave-1 premise correction |

**Observation:** the scalars exist; what is unverified is **which consumers**
read each scalar end-to-end (the EN-01 difficulty-consequence weave names four
consumer sites). Point 3 verifies and completes that weave without changing the
authority.

### 2.3 Economy and scarcity

| Fact | Evidence |
|---|---|
| hardcore tuning catalog | `Assets/StreamingAssets/Data/hardcore_economy_tuning.json` |
| economy goods catalog | `economy_goods.json` |
| regional prices | `regional_prices.json` |
| market/black-market/pressure systems | `Assets/Ashfall.Core/Economy/*`, `BlackMarketSettlementService`, `RegionalPriceAtlas`, `RegionalSupplyRouter` |
| no FundsLedger | verified in wave 1 (UNBLOCK-02 F13 proposal); the economy's cash model is bounded |

**Observation:** pressure tiers (XP-08/EN-03) are the natural bounded lever; the
plan uses them as a proposal, not a new economy.

### 2.4 Weather/radiation multipliers

| Fact | Evidence |
|---|---|
| fallout storm outdoor rads | `WeatherSystem.FalloutStormOutdoorRadModifier = 150f` |
| black rain outdoor rads | `BlackRainOutdoorRadModifier = 250f` |
| black rain hazmat melt | `BlackRainHazmatMeltMultiplier = 5f` |
| season windows | `weather_seasons.json` (First Thaw day 0, Ash Settling day 30, …) |

**Observation:** the survival pressure is already weather-coupled; the plan
teaches the player these couplings (Point 6) rather than adding new ones.

### 2.5 Progression and skills

| Fact | Evidence |
|---|---|
| skill catalog | `skills.json`; `SkillCatalogLoader` |
| dormancy tick | `SkillProgressionSystem.TickDaily` wired (DEBT-185 RETIRED: host tick added) |
| apprenticeship/catalog | `ApprenticeshipSystem` exists (content-thin per W2-01/W2-06) |
| development traits | `development_traits.json` |
| XP-04/06/07/08 status | wave-1 program: XP-04 blocked on F13; XP-06 blocked on F14; XP-07 unsigned; XP-08 blocked on F13 |

**Observation:** progression exists but its **pacing and feedback** are the open
gameplay questions (Point 5), not its existence.

### 2.6 Death and failure

| Fact | Evidence |
|---|---|
| `NeedsSystem.ForceDeath` | explicit death path |
| `SurvivorFateSystem` | fate/death events (Plan 194 wiring evidence) |
| game-over UI | `_gameOver` handlers in `Main.UiPanels.cs` (:1612) |
| crisis presentation | `CrisisPresentationCoordinator` (Plan 194 evidence) |

**Observation:** the failure machinery exists; the design question is how
foreseeable and recoverable failure is (Points 2 and 8).

---

## 3. Scope, non-goals, rules

### 3.1 In scope

- Pacing and pressure values, in data where a catalog owns them.
- Consumer binds that make existing scalars visibly matter (difficulty weave).
- Feedback/telegraphing surfaces backed by real Core values.
- Onboarding first-hour flow improvements using existing systems.
- Crisis, failure, and late-game pacing.
- Choice density in existing event systems.

### 3.2 Non-goals

- New gameplay systems (Rule 5).
- Environmental mechanics/content → W2-04.
- Location authoring/tiers → W2-05.
- Prose/voice additions → W2-06 (this plan may name the surface, W2-06 writes it).
- Bug repairs → W2-02.
- Save schema → UNBLOCK-01/02.
- Difficulty authority changes → it is live; this plan binds consumers only.

### 3.3 Rules

1. **Owner-named lever.** Every change states: owner, current value, proposed
   value, measurement.
2. **Data-first.** If a value lives in JSON, tune the JSON; do not hardcode.
3. **Determinism.** Soaks use the seeded RNG; no wall-clock.
4. **Reversibility.** One commit per tuning tranche; revert is a value restore.
5. **Focused measurement.** Seeded soak runs (bounded days) + focused suites;
   no full-suite safety runs.
6. **No balance-by-vibes.** Each tranche records before/after metrics from the
   same seed set.

---

## 4. Plan Path and decision index

### 4.1 The ten points

| # | Point | Default |
|---|---|---|
| 1 | Survival pressure curve and pacing bands | B |
| 2 | Failure telegraphing: time-to-threat and promises | B |
| 3 | Difficulty consequence weave (consumer completion) | B |
| 4 | Economy pressure and scarcity tiers | B |
| 5 | Progression pacing and skill feedback | A |
| 6 | Feedback/information surfaces (causal clarity) | B |
| 7 | Onboarding first-hour flow | B |
| 8 | Crisis, death, and recovery design | C |
| 9 | Late-game and campaign arc | C |
| 10 | Agency and choice density | C |

### 4.2 Selection sheet

```text
PLAN 3 — GAMEPLAY IMPROVEMENT
Plan Path: [ ] A Tune & Telegraph  [ ] B Deepen the Loop (default)  [ ] C Campaign Arc

01 pressure curve ......... [A] [B] [C]   default B
02 telegraphing ........... [A] [B] [C]   default B
03 difficulty weave ....... [A] [B] [C]   default B
04 economy pressure ....... [A] [B] [C]   default B
05 progression pacing ..... [A] [B] [C]   default A
06 feedback surfaces ...... [A] [B] [C]   default B
07 onboarding ............. [A] [B] [C]   default B
08 crisis/death ........... [A] [B] [C]   default C
09 late game .............. [A] [B] [C]   default C
10 agency density ......... [A] [B] [C]   default C
```

---

## 5. Decision Point 1 — Survival pressure curve and pacing bands (default B)

### 5.1 The design question

The campaign's first shortfall should be a **near miss the player can learn
from**, not a surprise wipe and not a triviality. After the first month, the
curve should tighten in visible steps tied to events (weather, season, war
  phase), not continuously.

### 5.2 Path A — Tune existing values within bands

- Define bands from a seeded soak baseline: first hunger threat (days 6–10),
  first thirst threat (days 3–6), first warmth threat (weather-dependent),
  first radiation scare (days 10–20).
- If the baseline falls outside the band, adjust the owning value by the
  smallest step that re-enters it (`difficulty` scalars for a global shift; the
  need decay profile for a specific need).
- Record before/after curves from the same seeds.

### 5.3 Path B — Pressure curve as data + soak harness

- Add (or reuse) a **pressure-curve catalog** owned by the difficulty/economy
  authority: per campaign-day window, a small set of multipliers already
  consumed by the tick (needs decay, event weight, price pressure). No new
  consumer — the plan binds existing ones (Point 3 completes them).
- Build the **seeded soak harness**: run N seeds × M days headless, output
  per-day shortfall time-to-threat, and compare against bands. The harness is
  the measurement authority for the whole plan.
- Tune the curve so the bands hold for a chosen difficulty set (Standard
  primary; Sparing offset; any future preset derived).

### 5.4 Path C — Dynamic difficulty pressure

Path B, plus a bounded dynamic element: if the player is far ahead of the
curve (no pressure event for X days), the next event window weights scarcity
slightly higher; if the player is in a death spiral, one mercy window
(bounded, once per campaign) delays the next crisis deadline. The scalar is
explicit, capped, and recorded in the save as part of the existing difficulty
state — **no new authority**, no hidden rubber-banding beyond the cap.

This is the largest behavioural change in the plan and should be signed
separately; it also needs a player-facing honesty policy (either disclosed or
subtle, the signed decision records which).

### 5.5 Measurement plan

```bash
# seeded soak (design the harness under B; example shape)
dotnet run --project src -- --simulate --seeds 1..20 --days 60 \
  --report needs-threats,economy,radiation
```

Metrics: time-to-first-threat per need; number of deaths per 60 days; days
without any pressure event; resource troughs.

Bands are recorded in the catalog header or a design doc so future tuning has
targets.

### 5.6 Anti-patterns

- Tuning to make the game "harder" without a band.
- Changing multiple owners in one tranche (unattributable).
- Using difficulty presets to hide a pacing bug.

---

## 6. Decision Point 2 — Failure telegraphing (default B)

### 6.1 The design question

Every irreversible harm (death, permanent injury, destroyed gear, lost
survivor) should be foreshadowed by at least one visible signal with enough
time to respond. The telegraph must come from real state, never a scripted
lie.

### 6.2 Path A — Audit and fill the worst gaps

- For each irreversible outcome, list its existing signals (UI, log, radio,
  briefing, panel state).
- Identify outcomes with no signal and add the smallest existing-surface
  signal (a status label, a briefing line via the existing crisis route, a
  panel state change).

### 6.3 Path B — Time-to-threat surfacing

- Extend the existing crisis/briefing path (`CrisisPredictor`,
  `DailyBriefingReportBuilder.AppendCrisisWarnings`, `FeedbackMessages`) to
  report **time-to-threat** for the top N concerns: "Thirst: 2 days at current
  consumption".
- Values come from the owners (`NeedsSystem` state + consumption rates), never
  invented by the UI.
- Rule: the UI never fabricates a fallback (the production-UI purity gates).
  If a value is unknown, the message says "unknown, last reading day N".

### 6.4 Path C — Promise/consistency model

Path B, plus a **promise ledger**: every warning the game shows is recorded
with the range it promised (e.g., "crisis within 3 days"); when the outcome
occurs, the game checks the promise was within tolerance and logs mismatches
for tuning. This turns telegraph honesty into a measurable contract and feeds
Point 3 of W2-02 (typed/observable paths).

### 6.5 Acceptance

- No irreversible outcome without at least one signal (audit table complete).
- Signals quote owner values with units and a time basis.
- The purity gate stays green (no fabricated values).
- A soak confirms warnings precede outcomes within tolerance (Path C).

---

## 7. Decision Point 3 — Difficulty consequence weave (default B)

### 7.1 The design question

The nine difficulty scalars exist; do they **visibly** change the world, or do
some merely sit in data? The EN-01 program names four consumer sites where
difficulty should route into consequences (war stage severity, crisis
deadlines, shock/rumor weight, and the chronicle stamp). XP-01 already bound
the authority and some consumers.

### 7.2 Path A — Consumer audit

- For each scalar, find every consumer (grep + read).
- Classify: consumed visibly / consumed invisibly / unconsumed.
- Publish the table; fix only typo-level misbindings.

### 7.3 Path B — Complete the visible weave for the named sites

- Bind the named sites through their canonical owners (no new system):
  - crisis deadline scale already exists (`crisis_deadline_mult`) — verify it
    reaches the crisis scheduler;
  - war stage severity through `FactionWarChainRunner`/stage selection;
  - shock/rumor weight through the radio/journal event weight;
  - chronicle stamp through `CampaignCompletionHistory` v2 (already stamped).
- For each bind, a focused test proving the scalar changes the outcome on
  Sparing vs Standard.
- A visible-consequence smoke: run one seeded week on each preset and assert
  the consequences differ in the expected direction (monotonicity).

### 7.4 Path C — Monotonicity soak + difficulty honesty surface

Path B, plus a monotonicity soak across all presets (more difficulty ⇒
weak-or-equal outcomes on every measured axis) and a player-facing difficulty
description generated from the actual bound scalars, so the menu never claims
a difference the game does not make.

### 7.5 Acceptance

- Every scalar has at least one visible consumer, or is retired/documented.
- Monotonicity holds across the measured axes (Path C).
- No new difficulty authority; no preset rename; no new save field.

---

## 8. Decision Point 4 — Economy pressure and scarcity tiers (default B)

### 8.1 The design question

Scarcity should rise in tiers the player can read (plenty → tightening → lean →
crisis → collapse) and should be recoverable through play. The economy already
has markets, regional prices, black market, caravans, and ration systems.

### 8.2 Path A — Tune existing price/ration values to bands

- Soak: measure food/med/parts availability through 60 days.
- If one good dominates or vanishes, tune its catalog value or regional factor.
- No new tiers.

### 8.3 Path B — Pressure tiers over existing owners (pure read model)

- Implement the XP-08/EN-03 style tiers as a **read model** over heat/trust/
  settlement state: `EconomyPressureTier` derived each day, consumed by prices,
  event weights, and barter. One catalog file (`economy_pressure_tiers.json`)
  as the authority for thresholds and effects.
- No FundsLedger requirement: the tier reads existing state and multiplies
  existing values (bounded).
- Ration conflict/desperation owners consume the tier rather than duplicating
  scarcity logic.
- Test: tier transitions at authored thresholds; save/load parity of tier
  state (derived, so no persistence beyond a day stamp).

### 8.4 Path C — Full pressure economy with recovery arcs

Path B, plus authored recovery arcs (a bad tier opens specific opportunities:
salvage rush, caravan glut, ration reform) through existing event systems, with
tier-aware weight tables. This is content-shaped; the **mechanics** are the
tiers; the arcs may be authored by W2-06 on request.

### 8.5 Acceptance

- Tier thresholds authored and consumed.
- No parallel currency or ledger (UNBLOCK-02's boundary respected).
- Player can read the tier (Point 6 surface).
- Recovery routes exist for every crisis tier (C).

---

## 9. Decision Point 5 — Progression pacing and skill feedback (default A)

### 9.1 The design question

Skills exist (`skills.json`, `SkillProgressionSystem`, dormancy tick fixed).
Does progression feel earned and visible? XP-06/07/08 are blocked elsewhere;
this point tunes only what is live.

### 9.2 Path A — Measure and tune

- Soak: skill gain rates per activity; time to first promotion; dormancy
  penalty visibility.
- Tune only if a band is violated (e.g., first skill level in 2–5 days).
- No new progression mechanics.

### 9.3 Path B — Feedback for existing progression

- Surface existing progression through existing panels (skill value, recent
  gain cause, dormancy notice) — values from the owner.
- Ensure apprenticeship/study actions explain their contribution.

### 9.4 Path C — Progression identity (only if XP-06/07/08 unblock)

- If F14/F13 signatures land, integrate the richer progression surfaces
  (body-integrity rehab XP-06; provenance-driven appraisal XP-07) as consumers
  of the existing progression owner.
- Not authorized by this plan; listed so the option is visible.

### 9.5 Acceptance

- Measured rates within bands or bands revised with justification.
- Any feedback surface reads owner values only.

---

## 10. Decision Point 6 — Feedback and information surfaces (default B)

### 10.1 The design question

The player should be able to answer "why did that happen?" and "what happens if
I do nothing?" from the UI. This is the causal-clarity point.

### 10.2 Path A — Audit the top ten confusion sources

- List the outcomes players cannot explain (needs drops, radiation rise,
  sickness onset, gear decay, morale crash).
- For each, name the owning value and the existing surface that could show it.
- Fix the worst two with existing labels/tooltips.

### 10.3 Path B — Cause and forecast model (read-only)

- A Core read model that, for a survivor/need, returns: current value, daily
  delta, top contributing modifiers (from `NeedsModifierStack`/attributed
  deltas — the API already names sources), and a bounded forecast at current
  rates.
- One panel section presents it (survivor detail), plus briefing integration
  for the top concerns (Point 2).
- No new gameplay authority; no fabricated values; purity gates enforced.

### 10.4 Path C — Contradiction detector

Path B, plus a consistency check between displayed forecasts and actual
outcomes (the promise ledger from Point 2), surfacing a diagnostics entry when
a forecast materially missed. This keeps the UI honest over time.

### 10.5 Acceptance

- Every audited outcome names its owner and contributing modifiers.
- Forecasts derive from owner deltas, with a stated basis and horizon.
- No UI-computed gameplay values.

---

## 11. Decision Point 7 — Onboarding first-hour flow (default B)

### 11.1 The design question

The first hour must teach: needs, water/food, warmth, radiation, power, and the
survivor social layer — without a wall of text or a tutorial that lies about
the simulation.

### 11.2 Path A — Audit and sequence

- List the teachable systems in the order the player meets them.
- Check existing tutorial/onboarding surfaces; identify where the game demands
  a system it has not taught.

### 11.3 Path B — Teach-to-demand gating

- For each early demand, ensure a teach moment precedes it through existing
  surfaces (onboarding journey exists: `Main.Onboarding.cs`,
  `ResetOnboardingJourney`).
- Add bounded teach cards/step assertions only where the gap is proven.
- Use real current values in the teach text (a displayed number is a value
  read, not a scripted lie).

### 11.4 Path C — Scenario-based opening

Path B, plus an optional authored opening scenario (a first crisis with a
scripted but truthful state) teaching the loop through one small story, using
existing event/quest owners. This is content-heavy and should involve W2-06.

### 11.5 Acceptance

- No early demand without a preceding teach moment.
- Teach text values read from owners.
- Onboarding skip/return behavior preserved.

---

## 12. Decision Point 8 — Crisis, death, and recovery design (default C)

### 12.1 The design question

Failure must be legible, bounded where appropriate, and recoverable enough that
the player keeps playing. The game already has crisis presentation, fate
events, and game-over handling.

### 12.2 Path A — Audit failure outcomes

- List every failure path (death, permanent injury, loss, game over) and its
  pre/post signals and recovery routes.
- Identify unrecoverable-without-warning outcomes.

### 12.3 Path B — Recovery routes for every crisis type

- For each crisis family (fire, flood, radiation, disease, power, war), ensure
  the authored design includes a recovery action through existing owners.
- Add tests for the recovery route at the crisis boundary (e.g., after a flood
  event, the sump can be cleared with existing actions).

### 12.4 Path C — Death posture and campaign continuity

Path B, plus signed decisions on:
- **death posture**: hardcore (no reload), soft (reload allowed, campaign
  continues), or memorial (dead survivors leave legacy tokens) — implemented
  through existing save/slot behavior and the memorial system, no new
  authority;
- **campaign continuation**: what persists after game over (completion history
  already records it; a continue-loop needs its own signed decision);
- **recovery arcs** for the worst crises (a bad month leaves scars and
  opportunities, authored through existing event systems).

These are design decisions with save semantics implications; the plan lists
them so they can be signed deliberately rather than discovered.

### 12.5 Acceptance

- Every crisis family has a recovery route or an explicit "terminal by design"
  record.
- Death posture is a signed line, implemented through existing mechanisms.
- No hidden punishment loops (e.g., permanent unwarned debuffs).

---

## 13. Decision Point 9 — Late-game and campaign arc (default C)

### 13.1 The design question

What happens after survival stabilizes? The campaign has a 180-day Year of Ash
timeline, a war clock, endings, and completion history. Late-game improvement
means escalation, identity, and closure — not infinite treadmill.

### 13.2 Path A — Audit the arc

- Map the current escalation events by day window; identify dead zones (no
  meaningful escalation for >20 days).

### 13.3 Path B — Fill dead zones with existing owners

- Add escalation weight windows using existing war/weather/economy systems.
- Ensure each 30-day chapter has at least one authored pressure and one
  authored opportunity.

### 13.4 Path C — Arc structure and endings

Path B, plus:
- explicit chapter structure (phases with a named theme) recorded as design
  data, not code;
- ending reachability audit (every ending path is reachable and its triggers
  consumed);
- a post-campaign summary reading completion history (EN-07 territory; this
  plan only requests the gameplay-side checks).

### 13.5 Acceptance

- No 20-day dead zone in escalation.
- Every chapter has pressure + opportunity.
- Endings reachable (audit) with triggers consumed.

---

## 14. Decision Point 10 — Agency and choice density (default C)

### 14.1 The design question

How often does the player make a **meaningful** choice, and are dead choices
removed? The game has narrative encounters, travel encounters, micro-locations,
quests, moral choice, and politics.

### 14.2 Path A — Audit choice inventory

- Count meaningful choices per 10 days; identify stretches with none.
- Identify choices with a strictly dominant option (dead choices).

### 14.3 Path B — Choice quality pass

- For each dominant choice, adjust costs/outcomes within existing owners so
  options trade off.
- For dead zones, schedule existing event families with weight windows (not
  new content).

### 14.4 Path C — Choice consequence model

Path B, plus a bounded **consequence ledger** (existing systems already route
consequences: espionage, faction, journal) ensuring a choice's consequence is
recorded and surfaced in the campaign's story. This overlaps W2-06's narrative
surfaces; mechanics stay here, prose there.

### 14.5 Acceptance

- Choice density band (e.g., ≥1 meaningful choice per 2–4 days).
- No strictly dominant options in audited sets.
- Consequence recorded for audited high-impact choices.

---

*(Part A ends. Part B continues with execution phases, verification/soak
design, risks, ownership, then Part C with worked scenarios and Annex U.)*---

# PART B — EXECUTION, MEASUREMENT, AND VERIFICATION

---

## 15. The measurement harness (shared by all points)

Every gameplay change in this plan is measured by a **seeded soak**. The
harness is the plan's central artifact; without it, tuning is opinion.

### 15.1 Soak design

| Parameter | Value | Rationale |
|---|---|---|
| seeds | 1..20 (fixed set) | same set for before/after comparison |
| days | 60 (standard), 180 (arc runs) | first two months cover the pressure curve; 180 covers the campaign |
| host | `godot --headless` or the host CLI simulate path | no window, deterministic |
| sampling | daily snapshot of needs, resources, deaths, tier, events | metric source |
| output | machine-readable table (CSV/JSON) | diffable before/after |
| runtime cap | a few minutes per seed set | honours the focused-verification policy |

If the host lacks a simulate verb rich enough for the metrics, the harness adds
a **test-only** headless runner that drives the Core tick directly (no new
production path, no selftest-verb sprawl unless W-4 signs one).

### 15.2 Metrics dictionary

| Metric | Definition | Band/use |
|---|---|---|
| `tt_first_hunger` | days until any survivor's hunger crosses the warning threshold | 6–10 |
| `tt_first_thirst` | same for thirst | 3–6 |
| `tt_first_warmth` | days until warmth warning (weather-dependent) | data-driven |
| `tt_first_rad_scare` | days until radiation anxiety/acute warning | 10–20 |
| `deaths_60` | survivor deaths in 60 days | monotone with difficulty |
| `pressure_events` | authored pressure events per 10 days | ≥1 per chapter |
| `choice_density` | meaningful choices per 10 days | ≥2–5 |
| `tier_days[t]` | days spent in each economy tier | shape check |
| `skill_l1_day` | day of first skill level for a typical survivor | 2–5 |
| `crisis_recovery_rate` | fraction of crises with a used recovery action | ≥ 0.8 |
| `dead_zone_max` | longest stretch with no escalation | ≤ 20 days |
| `monotonicity` | outcomes ordered by difficulty | weak-monotone |

### 15.3 Band governance

Bands are **design targets**, recorded in a single doc
(`docs/gameplay/PACING_BANDS.md`). A tranche that moves a metric records the
band change explicitly; moving a band is a design decision, not a tuning
accident.

### 15.4 The before/after protocol

```text
1. Run seed set S on HEAD → baseline table B0
2. Apply one lever tranche
3. Run seed set S → table B1
4. Diff B0/B1 → record deltas per metric
5. If a metric left its band without a signed band change, revert or adjust
6. Commit with the diff attached
```

One lever per tranche. Multiple independent levers in one commit make the diff
unattributable and the revert coarse.

---

## 16. Phase plan

### Phase G0 — Baseline and band proposal (1–2 days, all paths)

- Build/verify the soak harness (or adapt the existing simulate path).
- Run the baseline on Standard and Sparing.
- Propose bands from the baseline + design intent.
- Deliverable: `P0_GAMEPLAY_PREMISE.md` + baseline tables + band proposal.

This phase alone is valuable under Plan Path A: it turns "the pacing feels
off" into numbers.

### Phase G1 — Pacing and telegraphing (Points 1 + 2)

- Path A: tune values into bands; fill the worst telegraph gaps.
- Path B: pressure-curve catalog + time-to-threat surfacing + promise audit.
- Evidence: before/after soak; telegraph audit table; focused UI tests for new
  labels (purity gate).

### Phase G2 — Difficulty weave completion (Point 3)

- Consumer audit table.
- Bind the named sites (Path B) through owners.
- Monotonicity smoke: Sparing vs Standard on the measured axes.
- Evidence: bind tests + monotonicity table.

### Phase G3 — Economy pressure (Point 4)

- Path A: price/ration tuning to bands.
- Path B: `economy_pressure_tiers.json` + read model + consumers + transitions
  test. Coordinate with UNBLOCK-02/EN-03 boundaries (read-only; no FundsLedger).
- Evidence: tier transition tests; tier-day distribution; no new ledger grep.

### Phase G4 — Progression, feedback, onboarding (Points 5–7)

- Point 5 Path A: rate measurement + minimal tuning.
- Point 6 Path B: cause/forecast read model + one surface + purity checks.
- Point 7 Path B: teach-to-demand audit + gating gaps.
- Evidence: soak metrics, forecast unit tests, onboarding sequence test.

### Phase G5 — Crisis, arc, agency (Points 8–10, default C)

- Point 8: crisis recovery audit; signed death posture line.
- Point 9: escalation windows; chapter audit.
- Point 10: choice density + dominance pass; consequence recording for
  high-impact choices.
- Evidence: recovery tests, escalation coverage table, choice audit.

### Phase G6 — Closeout

- Final soak on the ready tree; band table current; evidence pack; handoff.

### 16.1 Ordering constraints

- G0 first, always.
- G1 before G3 (pressure before prices).
- G2 independent of G1; can run in parallel with disjoint claims.
- G5 C-items last (they may need signed decisions).

---

## 17. Verification plan

### 17.1 Per-point evidence

| Point | Measurement | Test |
|---|---|---|
| 1 | soak bands before/after | pressure-curve unit tests (thresholds) |
| 2 | telegraph audit completeness | forecast/briefing tests; purity gate |
| 3 | monotonicity table | per-bind behavioural tests |
| 4 | tier-day distribution | transition tests; save parity |
| 5 | skill-rate metrics | progression suite |
| 6 | forecast accuracy vs outcome | read-model unit tests |
| 7 | teach-to-demand sequence | onboarding flow test |
| 8 | recovery rate | crisis recovery tests |
| 9 | dead-zone max | escalation coverage table |
| 10 | choice density | choice audit + consequence recording tests |

### 17.2 Commands

```bash
# focused suites (existing)
bash scripts/run_test.sh Ashfall.Core.Tests/Survivors
bash scripts/run_test.sh Ashfall.Core.Tests/Economy
bash scripts/run_test.sh Ashfall.Core.Tests/Difficulty
# selftests
godot --headless --path . -- --7day-smoke-selftest
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
# harness (new; test-only or CLI)
<soak command designed in G0>
```

### 17.3 Guardrails

- No tuning tranche without a baseline diff.
- No new UI value not read from an owner.
- No difficulty change that breaks monotonicity.
- No save-schema change.
- No new currency/ledger.
- No full-suite safety runs.

---

## 18. Risks

| # | Risk | L | I | Mitigation |
|---|---|---|---|---|
| 1 | tuning churn without bands | M | M | band doc + one-lever tranches |
| 2 | telemetry harness becomes a second game loop | L | M | bounded seeds/days; test-only |
| 3 | difficulty weave breaks existing balance | M | H | monotonicity smoke + revertible binds |
| 4 | economy tiers duplicate UNBLOCK-02 work | M | H | read-model only; coordinate claims |
| 5 | feedback surfaces fabricate values | M | H | purity gates; owner reads only |
| 6 | onboarding teaches a value the sim disagrees with | M | M | teach text reads current values |
| 7 | death posture decision blocks shipping | M | M | default posture documented; C optional |
| 8 | late-game content authored here by accident | M | H | content routes to W2-06 |
| 9 | choice audit pulls narrative rework | M | M | mechanics only; prose to W2-06 |
| 10 | soak runtime competes with builders | M | M | bounded minutes; scheduled |
| 11 | band drift over patches | M | L | bands versioned with the doc |
| 12 | dynamic pressure (C) feels unfair | M | H | cap + honesty policy + separate signature |

---

## 19. Ownership and claims

| Phase | Claim | Paths |
|---|---|---|
| G0 | `W2-03-G0-SOAK-BANDS` | harness (test/tooling), `docs/gameplay/PACING_BANDS.md` |
| G1 | `W2-03-G1-PRESSURE` | difficulty/need tuning data; pressure catalog; briefing/forecast surfaces |
| G2 | `W2-03-G2-DIFFICULTY-WEAVE` | consumer bind sites in owners (bounded), tests |
| G3 | `W2-03-G3-PRESSURE-TIERS` | new tier catalog + read model + consumers, tests |
| G4 | `W2-03-G4-PROGRESSION-FEEDBACK` | progression tuning, cause/forecast model + panel, onboarding |
| G5 | `W2-03-G5-CRISIS-ARC-AGENCY` | crisis recovery wiring, escalation windows, choice balance |
| G6 | `W2-03-G6-CLOSEOUT` | evidence + ledger proposals |

Coordination: W2-02 owns defect repairs; W2-04 owns environment mechanics; W2-05
locations; W2-06 prose. This plan's claims are values, read models, and
existing-owner consumer binds.

---

## 20. Rollback and decline

| Point | Rollback | Decline consequence |
|---|---|---|
| 1 | restore values from the tranche diff | pacing stays unmeasured |
| 2 | remove added signals | gaps remain (audited) |
| 3 | unbind consumer | scalar stays invisible (recorded) |
| 4 | remove tier catalog/consumers | pressure stays flat |
| 5 | restore tuning | rates unbounded |
| 6 | remove surface | causal clarity unchanged |
| 7 | keep audit, drop gating | teach gaps recorded |
| 8 | keep audit, drop posture | failure design undeclared |
| 9 | keep audit | dead zones recorded |
| 10 | keep audit | dominance recorded |

Every decline leaves a dated record so the next pass starts from evidence.

---

## 21. DoD and handoff

**Path A:** bands proposed, values tuned into them, worst telegraph gaps
filled, baseline/final soaks attached.

**Path B:** all of A, plus pressure catalog, difficulty weave binds with
monotonicity, tier read model, forecast surface, onboarding gates.

**Path C:** all of B, plus signed death posture, arc structure, and choice
consequence model (or explicit deferrals).

**Handoff fields:** outcome, files, contract (levers + bands), commands and
soak tables, limitations, untouched shared paths, ledger proposals, Annex U
status.

### 21.1 First safe step

> Phase G0 only: build the soak, run the baseline, propose bands. No value
> changes until the baseline exists.

---

*(Part B ends. Part C continues with worked tuning examples, scenarios, Q&A,
Annex U, and appendices.)*---

# PART C — WORKED TUNING AND DESIGN WALKTHROUGHS

---

## C.1 Walkthrough 1 — Pressure-curve tranche (Point 1, Path B)

### C.1.1 Baseline (illustrative real-shaped numbers)

A 20-seed × 60-day soak on Standard might produce:

| Metric | Baseline | Band | Verdict |
|---|---|---|---|
| `tt_first_thirst` | 2.1 days | 3–6 | too fast |
| `tt_first_hunger` | 4.8 days | 6–10 | too fast |
| `tt_first_warmth` | 11 days (First Thaw) | season-driven | plausible |
| `tt_first_rad_scare` | 14 days | 10–20 | in band |
| `deaths_60` | 5.4 | ≤ 3 on Standard | too lethal |
| `pressure_events` | 0.8 / 10 days | ≥ 1 | thin |

### C.1.2 The tranche

Smallest attributable change: one lever — **the first-window pressure
multiplier** (day 0–14) for hunger/thirst decay. If the owning value is the
difficulty scalar (`hunger_rate_mult`/`thirst_rate_mult`), changing it shifts
all days, which may over-correct later windows. That is exactly why Path B
prefers the **windowed pressure curve**: the early window loosens while later
windows keep their pressure.

```json
// economy/needs pressure curve (shape proposal)
{
  "schema_version": 1,
  "windows": [
    { "id": "opening",  "start_day": 0,  "end_day": 14, "hunger_mult": 0.8, "thirst_mult": 0.8, "warmth_mult": 1.0 },
    { "id": "settling", "start_day": 15, "end_day": 45, "hunger_mult": 1.0, "thirst_mult": 1.0, "warmth_mult": 1.0 },
    { "id": "tightening","start_day": 46, "end_day": 90, "hunger_mult": 1.15, "thirst_mult": 1.1, "warmth_mult": 1.1 },
    { "id": "ash",      "start_day": 91, "end_day": 180,"hunger_mult": 1.3, "thirst_mult": 1.2, "warmth_mult": 1.2 }
  ]
}
```

Consumption is **read-only** by the needs tick (the multiplier enters as an
existing external-modifier or decay-rate parameter). No new authority: the
curve is a data table consumed by the owner.

### C.1.3 After

| Metric | After | Band |
|---|---|---|
| `tt_first_thirst` | 3.8 | in |
| `tt_first_hunger` | 7.2 | in |
| `deaths_60` | 2.1 | in |
| `pressure_events` | 1.3/10 days | in |

### C.1.4 The diff discipline

The commit contains: the curve file, the consumer bind (if not already),
the before/after tables, and the band entry. One lever, one diff, one
revertible change.

---

## C.2 Walkthrough 2 — Telegraph fill (Point 2, Path B)

### C.2.1 Audit row (example)

| Outcome | Existing signal | Gap | Fix |
|---|---|---|---|
| Survivor dies of thirst | none until the death event | no countdown | briefing line via crisis route: "Thirst: ~2 days" |
| Gear destroyed by wear | condition percentage in item detail | no warning at low condition | condition state label already exists; add a threshold note |
| Radiation acute onset | dosimeter reading | no time-to-threat estimate | dose-rate forecast from the dose ledger owner |

### C.2.2 The time-to-threat computation

```csharp
// Core read-only; owner values only
public readonly struct TimeToThreat
{
    public NeedKind Need { get; init; }
    public float CurrentValue { get; init; }
    public float DailyDelta { get; init; }
    public float Threshold { get; init; }
    public int Days => DailyDelta >= -0.0001f ? int.MaxValue
                      : (int)MathF.Ceiling((CurrentValue - Threshold) / -DailyDelta);
}
```

`DailyDelta` comes from the owner's tracked rate (or a bounded sampled delta);
`Threshold` from the authored warning level. The UI renders
`Days` with the word "about" and a floor ("at least 1 day"), never false
precision.

### C.2.3 Briefing integration

`DailyBriefingReportBuilder.AppendCrisisWarnings` already assembles warnings
(Plan 24 consumer). The tranche adds at most the top-2 nearest threats with
their `TimeToThreat` days, sourced from the same read model. The briefing
remains read-only; it never mutates.

### C.2.4 Acceptance

- Telegraph table complete for irreversible outcomes.
- Every displayed day count derives from `TimeToThreat`.
- Purity gate green: no fallback numbers (unknown shows "unknown").

---

## C.3 Walkthrough 3 — Difficulty weave bind (Point 3, Path B)

### C.3.1 Consumer audit table (target shape)

| Scalar | Consumed by (verified at P0) | Visible? | Action |
|---|---|---|---|
| `hunger_rate_mult` | needs tick | yes | keep |
| `thirst_rate_mult` | needs tick | yes | keep |
| `radiation_gain_mult` | exposure pipeline | yes | keep |
| `disease_onset_mult` | disease onset | yes | keep |
| `hostile_encounter_mult` | travel encounter weight | yes | keep |
| `market_price_mult` | market pricing | yes | keep |
| `equipment_decay_mult` | wear tick | yes | keep |
| `crisis_deadline_mult` | crisis scheduler | **verify** | bind if unconsumed |
| (war stage severity) | — | no | bind through stage selection |
| (shock/rumor weight) | — | no | bind through radio/journal weight |
| (chronicle stamp) | completion history | yes (record) | keep |

### C.3.2 Bind test pattern

```csharp
[Fact]
public void CrisisDeadline_ScalesWithDifficulty()
{
    var sparing = new Harness(Difficulty.Sparing).ScheduleCrisis();
    var standard = new Harness(Difficulty.Standard).ScheduleCrisis();
    Assert.True(sparing.DeadlineDays >= standard.DeadlineDays);
    Assert.Equal(1.25f, sparing.Multiplier, precision: 3);
}
```

### C.3.3 Monotonicity smoke

| Axis | Expectation |
|---|---|
| deaths_60 | non-decreasing with difficulty |
| first-threat days | non-increasing with difficulty |
| price level | non-decreasing |
| crisis deadline | non-increasing |
| encounter count | non-decreasing |

If an axis violates monotonicity, the bind is wrong (or the design is
intentionally non-monotone — then it is recorded as a design line).

---

## C.4 Walkthrough 4 — Economy pressure tiers (Point 4, Path B)

### C.4.1 Catalog shape

```json
{
  "schema_version": 1,
  "tiers": [
    { "id": "plenty",     "min_pressure": 0.0, "price_mult": 0.95, "ration_conflict": 0.0,  "event_weight_mult": 1.0 },
    { "id": "tightening", "min_pressure": 0.3, "price_mult": 1.10, "ration_conflict": 0.1,  "event_weight_mult": 1.1 },
    { "id": "lean",       "min_pressure": 0.55,"price_mult": 1.35, "ration_conflict": 0.25, "event_weight_mult": 1.25 },
    { "id": "crisis",     "min_pressure": 0.75,"price_mult": 1.7,  "ration_conflict": 0.5,  "event_weight_mult": 1.5 },
    { "id": "collapse",   "min_pressure": 0.9, "price_mult": 2.1,  "ration_conflict": 0.8,  "event_weight_mult": 1.8 }
  ]
}
```

`pressure` is a derived 0..1 from existing state (regional scarcity + shelter
reserves + war disruption + weather). The read model computes it; consumers
read the tier. No persistence beyond the current day's derived value.

### C.4.2 Consumer rules

| Consumer | Reads | Bound |
|---|---|---|
| market pricing | `price_mult` | existing price calc multiplier |
| ration conflict | `ration_conflict` | existing conflict probability param |
| event scheduling | `event_weight_mult` | existing weight tables |
| briefing | tier name | new label from the read model (Point 6) |

### C.4.3 Boundary check

- No currency created or stored.
- No second ledger (UNBLOCK-02's F13 contract untouched).
- No player-facing "pressure stat" beyond the tier label (the tier is a read).

### C.4.4 Transition test

```csharp
[Theory]
[InlineData(0.0, "plenty")]
[InlineData(0.32, "tightening")]
[InlineData(0.6, "lean")]
[InlineData(0.8, "crisis")]
[InlineData(0.95, "collapse")]
public void PressureTier_MapsAtThresholds(float pressure, string tier)
    => Assert.Equal(tier, EconomyPressureModel.TierFor(pressure));
```

---

## C.5 Walkthrough 5 — Cause and forecast surface (Point 6, Path B)

### C.5.1 Read-model shape

```csharp
public sealed record NeedCause(
    NeedKind Need, float Current, float DailyDelta,
    IReadOnlyList<ModifierContribution> TopContributors, TimeToThreat Threat);

public sealed record ModifierContribution(string SourceId, float PerDay, int? ExpiresDay);
```

`TopContributors` comes from the existing modifier stack/attributed deltas — the
API already names sources (`SetExternalModifier(sourceId, …)`,
`ApplyAttributedDelta(…)`), so the panel shows "cold snap −4.2/day (until day
34)" truthfully.

### C.5.2 Surface

One survivor-detail section: current value, delta, top three contributors,
time-to-threat. No chart invented; no value computed by the UI.

### C.5.3 Purity checks

The existing `ProductionUiNoFabricatedFallback`/`PlayerSurfaceBindingPurity`
gates apply. If a value is unavailable, the surface shows the reason
("last reading day 12"), not a placeholder that looks real.

---

## C.6 Walkthrough 6 — Onboarding teach-to-demand (Point 7, Path B)

### C.6.1 Demand map (example rows)

| Demand | Day encountered | Taught before? | Fix |
|---|---|---|---|
| water production | day 2–4 | partially | teach card at first thirst warning |
| fire/heat | first cold snap | no | teach when temperature drops below threshold |
| radiation zone | first travel to a rad zone | yes (dosimeter note) | verify order |
| power allocation | first power crunch | no | teach at first brownout event |

### C.6.2 Rule

A teach moment triggers on the **first real occurrence** of the mechanic (real
values), not on a fixed script. This keeps the tutorial truthful if the
simulation diverges (e.g., a lucky player reaches day 10 without thirst
pressure — the teach fires at the real first warning, not day 2).

### C.6.3 Test

An onboarding flow test drives the seeded early game and asserts each teach
event fires exactly once and before its demand threshold.

---

## C.7 Worked example — a full tranche package

**Tranche G1-A1: opening-window loosening**

| Field | Value |
|---|---|
| Owner | needs tick via pressure curve (new table) |
| Before | thirst 2.1d / hunger 4.8d / deaths 5.4 |
| After | thirst 3.8d / hunger 7.2d / deaths 2.1 |
| Files | `Data/pressure_curve.json` (new), consumer bind, tests |
| Evidence | baseline/after soak tables, band entries |
| Revert | delete table + bind (2 files) |
| Risk | none observed on 20 seeds; monitor later windows |

This is the plan's atom: one lever, one table, one diff, one revert.

---

## C.8 Design-decision register (for C-path items)

```markdown
### DR-W2-03-8 — Death posture
- Options: hardcore / soft / memorial
- Recommendation: soft + memorial tokens via existing memorial system
- Save implications: none (uses existing slots/game-over path)
- Player-facing text: W2-06 authors with the decided posture
- Decided: ____
```

```markdown
### DR-W2-03-9 — Campaign continuation after game over
- Options: none / NG+ read-only / NG+ with disposition
- Recommendation: read-only continuation (completion history + new campaign)
- Boundary: no rewards/unlocks (DEC-20)
- Decided: ____
```

```markdown
### DR-W2-03-4 — Dynamic pressure honesty
- Options: disclosed / subtle / none
- Recommendation: subtle with a cap and a documented mercy window
- Decided: ____
```

---

## C.9 Metrics appendix — what each band means

| Band | Player experience | Failure it prevents |
|---|---|---|
| first-threat windows | a near miss that teaches | instant wipe or trivial opening |
| deaths per 60 (Standard) | rare losses with causes | attrition blur |
| pressure events ≥ 1/10d | something always developing | dead zones |
| choice density ≥ 2/10d | agency | long passive stretches |
| recovery rate ≥ 0.8 | hope | unrecoverable spiral |
| monotonicity | difficulty means what it says | dishonest presets |

---

*(Part C ends. Part D continues with scenarios, Q&A, Annex U, closeout, and
appendices.)*---

# PART D — OPTION ANALYSIS, SCENARIOS, Q&A, ANNEX U, APPENDICES

---

## D.1 Per-point option analysis (advantages / costs / what breaks)

### D.1.1 Point 1 — Pressure curve

| Path | Advantages | Costs | Failure mode avoided |
|---|---|---|---|
| A | fastest; data-only; trivially revertible | cannot fix a wrong curve *shape* | unmeasured pacing |
| B | windowed pressure; attributable; band-governed | one catalog + consumer + soak maintenance | one-global-multiplier over-correction |
| C | self-correcting pacing | perceived unfairness; needs honesty policy | stale curve after player skill shifts |

### D.1.2 Point 2 — Telegraphing

| Path | Advantages | Costs | Failure mode avoided |
|---|---|---|---|
| A | closes worst gaps fast | ad-hoc signals | obvious surprise deaths |
| B | one time-to-threat model for all warnings | one read model + surface work | inconsistent, invented numbers |
| C | promises verified against outcomes | promise ledger + tuning loop | warnings that quietly lie |

### D.1.3 Point 3 — Difficulty weave

| Path | Advantages | Costs | Failure mode avoided |
|---|---|---|---|
| A | finds invisible scalars | no bind work | data-only difficulty |
| B | every scalar visibly matters; monotonicity | bind tests per site | presets that differ on paper only |
| C | player-facing honesty text + soak | more measurement | contradicted menu claims |

### D.1.4 Point 4 — Economy pressure

| Path | Advantages | Costs | Failure mode avoided |
|---|---|---|---|
| A | tunes existing prices | no readable pressure shape | flat or erratic scarcity |
| B | one tier table, read-only; composes with EN-03 | thresholds need tuning | parallel economy systems |
| C | recovery arcs | content burden (W2-06) | unrecoverable collapse |

### D.1.5 Point 5 — Progression pacing

| Path | Advantages | Costs | Failure mode avoided |
|---|---|---|---|
| A | measures and tunes rates | no new feedback | unbounded grind |
| B | explains gains/dormancy | panel work | invisible progression |
| C | integrates XP-06/07 when unblocked | depends on other signatures | orphaned XP features |

### D.1.6 Point 6 — Feedback

| Path | Advantages | Costs | Failure mode avoided |
|---|---|---|---|
| A | closes top confusions | ad-hoc | "why did that happen?" |
| B | causal read model for everything | one model + surface | invented UI numbers |
| C | honesty auditing | ledger maintenance | forecasts that drift |

### D.1.7 Point 7 — Onboarding

| Path | Advantages | Costs | Failure mode avoided |
|---|---|---|---|
| A | audit reveals gaps | no fixes | demanded-but-untaught systems |
| B | teach-at-first-occurrence | event ordering tests | scripted tutorial vs. real sim |
| C | authored opening scenario | content cost | dry first hour |

### D.1.8 Point 8 — Crisis/death

| Path | Advantages | Costs | Failure mode avoided |
|---|---|---|---|
| A | maps failure | no recovery | surprise-terminals |
| B | recovery per family | tests at boundaries | dead-end crises |
| C | declared death posture + continuation | save-semantics decisions | accidental hardcore |

### D.1.9 Point 9 — Late game

| Path | Advantages | Costs | Failure mode avoided |
|---|---|---|---|
| A | finds dead zones | no fixes | mid-campaign lull |
| B | fills zones with existing systems | weighting work | content treadmill |
| C | chapter structure + ending audit | design data | unreachable endings |

### D.1.10 Point 10 — Agency

| Path | Advantages | Costs | Failure mode avoided |
|---|---|---|---|
| A | measures choice density + dominance | no fixes | passive stretches |
| B | quality pass on dominant choices | balance work | fake choices |
| C | consequence recording | overlaps W2-06 | choices that don't matter |

---

## D.2 Cross-point interactions

| Interaction | Consequence |
|---|---|
| 1 ↔ 4 | pressure curve and economy tiers both modulate scarcity; they must not double-count (curve = needs decay; tiers = prices/conflict) |
| 2 ↔ 6 | telegraphs are the point-6 read model's first consumer |
| 3 ↔ 1 | difficulty scales the curve window multipliers; one authority per multiplier chain |
| 4 ↔ 10 | scarcity tiers change choice stakes; dominance audit must re-run after tiers |
| 7 ↔ 2 | onboarding teach moments reuse telegraphs |
| 8 ↔ 9 | crisis recovery shapes late-game resilience |
| 5 ↔ 3 | progression rate scaling by difficulty must stay monotone |

The plan executes in the order G1 → G3 → G5 precisely to respect these.

---

## D.3 Scenarios

### D.3.1 Scenario — Plan Path A, one week

1. G0: soak + bands (2 days).
2. G1-A: tune thirst/hunger values into band; fill two telegraph gaps (1 day).
3. G2-A: consumer audit table (0.5 day).
4. G4-A: progression rate measure (0.5 day).
5. Closeout (0.5 day).
Outcome: measured pacing, worst surprises removed, invisible scalars listed.

### D.3.2 Scenario — Plan Path B, one month

1. G0 (2 days).
2. G1-B: pressure catalog + time-to-threat + briefing (5 days).
3. G2-B: weave binds + monotonicity (3 days).
4. G3-B: tiers + consumers + transitions (4 days).
5. G4-B: forecast surface + onboarding gates + progression feedback (5 days).
6. G5-B: recovery routes for crisis families (3 days).
7. Closeout (1 day).
Outcome: pacing governed by bands; every warning real; every scalar visible;
scarcity readable; UI explains causes.

### D.3.3 Scenario — Plan Path C, a quarter

Path B plus the three signed C decisions (dynamic pressure, death posture,
arc structure) and the choice consequence model, each its own package with
soaks and player-facing text authored by W2-06.

### D.3.4 Scenario — a band cannot be met

If `deaths_60` stays above band even at the loosest sensible curve, the plan
does not force it: it revises the band with a written rationale (maybe the
design intends harsher Standard) or escalates the specific mechanism (e.g.,
water access) to W2-04/05 for a structural fix. Bands serve the design; the
design does not serve the bands.

### D.3.5 Scenario — tuning conflicts with a bug

If a soak reveals a defect (e.g., thirst decay double-applied), stop and hand
to W2-02 with the repro. Never tune around a bug; that encodes it.

---

## D.4 Foreman Q&A

**Q1. Is this plan allowed to change the game's difficulty?**
Yes, within bands and with monotonicity; it cannot change the authority or
invent presets. The difficulty spine is live and this plan binds/surfaces it.

**Q2. What if the baseline shows the game is already well-paced?**
Then G0's tables become the reference and the plan's value shifts to
telegraphing, feedback, and arc coverage. That is a good outcome.

**Q3. Does the soak harness become a permanent burden?**
It is bounded (20 seeds × 60 days, minutes) and only run for gameplay
tranches. It is also the evidence source for every future balance question —
the best ratio of effort to value in this plan.

**Q4. Will the economy tiers conflict with UNBLOCK-02's XP-08 routes?**
The tiers are a read model over existing state; XP-08's routes and migration
are separate. The plan cites the boundary and defers any route/ledger work.

**Q5. Is dynamic pressure (Point 1 C) ethical?**
Only with a cap, a signed honesty policy, and no hidden punishment. The plan
recommends "subtle with cap" or declining C; it never silently rubber-bands.

**Q6. Does death posture belong here or in design docs?**
It has save/session implications, so it is presented as a signed decision line
here, implemented through existing mechanisms, with prose authored by W2-06.

**Q7. What about XP-06/07/08 blockers?**
They stay blocked; Point 5 C lists them as consumers only, enabling integration
when UNBLOCK-01/02 and the XP authorizations land.

**Q8. Where do environment mechanics (cold snaps, storms) get tuned?**
W2-04 owns environment mechanics; this plan tunes how needs respond to them
(the pressure curve reads weather), not the weather itself.

**Q9. Can a builder tune without the harness?**
No. Every tranche cites baseline/after tables. "It feels better" is not
evidence.

**Q10. What is the smallest useful approval?**
G0: the baseline and bands, no changes. It immediately makes pacing a fact.

**Q11. What is the largest?**
Plan Path C's signed decisions plus arc work — realistically several packages.

**Q12. How do we know the plan worked?**
Six metrics in band, no dead zones, every warning derived from an owner, and
the monotonicity table holding across presets.

---

## D.5 Annex U — Plan-unblocking (deliberately separate)

> **Wave 2 rule:** this annex is W2-03's unblocking component, kept separate
> from the tuning body. Nothing here authorizes another plan without U.2.

### U.1 What W2-03 releases

| Blocked item | Release mechanism | Gate |
|---|---|---|
| EN-01 Difficulty-Consequence Weave | Point 3 completes the consumer binds and monotonicity | G2 |
| EN-03 Underground economy pressure | Point 4's read-model tiers implement the pressure half without the ledger | G3 |
| XP-04 economy legs (partial) | Tier consumers make existing economy scalars visible; the ledger half stays UNBLOCK-02 | G3 |
| XP-06 rehabilitation (consumer) | Point 5 C lists the consumer bind, enabled post-F14 | G4/C |
| XP-08 trade routes (consumer) | Tiers read trade disruption; route contracts stay UNBLOCK-02 | G3 |
| W2-05 location importance | Travel/choice metrics from the soak feed location value decisions | G0 |
| W2-06 enrichment | Telegraph/onboarding/briefing surfaces name where prose goes | G1/G4 |
| E1/Plan 53 census governance | The band doc + soak tables are new evidence the census can cite | G0 |

### U.2 Signatures needed

```text
[ ] I authorize G0 soak+bassline and the band document.
[ ] I authorize Point 1 pressure-curve tuning (Path A/B/C chosen).
[ ] I authorize Point 2 time-to-threat surfacing via the crisis/briefing path.
[ ] I authorize Point 3 difficulty consumer binds (no authority change).
[ ] I authorize Point 4 economy pressure tiers as a read model (no ledger).
[ ] I authorize Point 6 cause/forecast read model + surface.
[ ] I authorize Point 7 onboarding teach gates.
[ ] I authorize Point 8 recovery routes; death posture: [ ] soft [ ] memorial [ ] hardcore.
[ ] I authorize Point 9 escalation windows; chapter structure: [ ] yes [ ] no.
[ ] I authorize Point 10 choice quality + consequence recording.
[ ] I authorize C-path dynamic pressure: [ ] no [ ] subtle+cap [ ] disclosed.
```

### U.3 What W2-03 never touches for unblocking

- Difficulty authority/schema (live; binds only).
- FundsLedger/trade migration (UNBLOCK-02).
- Body-integrity schema (UNBLOCK-01).
- Strings/semantic kind (UNBLOCK-03).
- Ledger truth (UNBLOCK-04).
- Expansion admission (UNBLOCK-05).

### U.4 The measurement-release rule

Releasing EN-01/EN-03 is only claimed when the corresponding soak/bind evidence
exists and the U.2 line is signed. A read model that exists but is not consumed
does not release an EN — reachability is the standard (the repo's own principle:
presence in data is not gameplay reachability).

---

## D.6 Appendices

### D.6.1 Selection sheet

```text
ASHFALL WAVE 2 · PLAN 3 (GAMEPLAY) · SELECTION
Date: ______  Foreman: ______  HEAD: ______

PLAN PATH: [ ] A Tune & Telegraph  [ ] B Deepen the Loop (default)  [ ] C Campaign Arc

01 pressure curve ..... [A] [B] [C]   default B
02 telegraphing ....... [A] [B] [C]   default B
03 difficulty weave ... [A] [B] [C]   default B
04 economy pressure ... [A] [B] [C]   default B
05 progression ........ [A] [B] [C]   default A
06 feedback ........... [A] [B] [C]   default B
07 onboarding ......... [A] [B] [C]   default B
08 crisis/death ....... [A] [B] [C]   default C
09 late game .......... [A] [B] [C]   default C
10 agency ............. [A] [B] [C]   default C

Signature: ________________
```

### D.6.2 Band doc template

```markdown
# ASHFALL Pacing Bands (<date>, HEAD <sha>)
| Metric | Band | Basis | Last changed |
|---|---|---|---|
| tt_first_thirst | 3–6 days | soak 20×60 Standard | <date> |
...
```

### D.6.3 Tranche record template

```markdown
### Tranche <id>
- Lever: <owner, value, before → after>
- Seeds: 1..20; days: 60
- Metrics: <table with before/after/band verdict>
- Files: <list>
- Revert: <command>
- Notes: <interactions; conflicts>
```

### D.6.4 Glossary

| Term | Meaning |
|---|---|
| band | intended metric range |
| lever | one authored value/policy |
| soak | seeded multi-day measured run |
| time-to-threat | estimated days until a threshold with no action |
| pressure tier | derived scarcity band from existing state |
| teach-to-demand | teaching a system at its first real occurrence |
| promise ledger | record of warning claims checked against outcomes |
| dead zone | stretch with no escalation/choice |

### D.6.5 What "done" looks like for each default path

| Default | Done |
|---|---|
| Point 1 B | curve catalog + bands in band across 20 seeds |
| Point 2 B | telegraph table complete; time-to-threat surfaced |
| Point 3 B | scalar table all visible or retired; monotone |
| Point 4 B | tiers authored, consumed, transition-tested |
| Point 5 A | rate metrics within bands or bands justified |
| Point 6 B | cause/forecast model + one surface, purity clean |
| Point 7 B | teach-before-demand verified by flow test |
| Point 8 C | recovery routes + signed death posture |
| Point 9 C | no dead zones; endings reachable audit |
| Point 10 C | density band met; dominance pass; consequences recorded |

---

## D.7 Final statement for W2-03

ASHFALL does not need more survival systems; it needs its existing ones to
**speak clearly and pace honestly**. This plan makes the pressure curve
authored and measured, makes every warning derive from an owner, completes the
difficulty weave so presets visibly matter, gives scarcity a readable shape,
and audits the campaign's long arc for dead zones and fake choices.

Recommended: **Plan Path B** with Point 5 at Path A. Start with G0 — the
baseline and bands — because it converts every later argument from taste to
table.

---

**End of W2-03.** Proposal only; executes nothing; releases nothing without
U.2 signatures.

*Document control: W2-03 · Wave 2 · HEAD 5be1a30a · companion to W2-01/02/04/05/06.*---

# PART E — EXECUTION PLAYBOOK AND PER-POINT CHECKLISTS

---

## E.1 The gameplay tranche lifecycle

```text
1. LEVER   — name the owner and the exact value/policy.
2. BASELINE— run the seed set; record the metric table.
3. CHANGE  — one lever, one diff.
4. MEASURE — rerun the same seed set; diff tables.
5. VERDICT — in band / band revised with rationale / revert.
6. COMMIT  — attach the diff and the band note.
```

A tranche without a baseline table is not reviewed.

---

## E.2 Per-point checklists

### E.2.1 Point 1 — pressure curve

```text
[ ] Bands proposed and signed (doc)
[ ] Soak runs before/after identical seeds
[ ] One lever per tranche
[ ] Window multipliers consume needs decay (owner bind)
[ ] Later windows not overcorrected (per-window table)
[ ] No player-facing lie (the curve is not displayed as a number unless true)
[ ] (C) dynamic pressure: cap + honesty policy signed
```

### E.2.2 Point 2 — telegraphing

```text
[ ] Irreversible-outcome list complete
[ ] Each outcome has ≥1 signal
[ ] TimeToThreat computed from owner deltas
[ ] Briefing shows top threats with basis
[ ] Unknown shows "unknown", never a fabricated number
[ ] Purity gates green
[ ] (C) promise ledger records ranges and checks outcomes
```

### E.2.3 Point 3 — difficulty weave

```text
[ ] Consumer audit table complete
[ ] Every scalar has a visible consumer or retirement note
[ ] Named sites bound: crisis deadline, war severity, shock/rumor, chronicle
[ ] Per-bind tests (Sparing vs Standard)
[ ] Monotonicity smoke across axes
[ ] (C) player-facing difficulty description generated from binds
```

### E.2.4 Point 4 — economy pressure

```text
[ ] Tier catalog authored (thresholds + effects)
[ ] Pressure read model computes from existing state
[ ] Consumers bound: prices, ration conflict, event weights
[ ] Transition tests at thresholds
[ ] No ledger/currency created (boundary grep)
[ ] Save parity (derived tier, no persisted authority)
[ ] (C) recovery arcs authored through existing event owners
```

### E.2.5 Point 5 — progression

```text
[ ] Rate metrics measured (skill L1 day, dormancy visibility)
[ ] Bands checked; tuning only if violated
[ ] Feedback surface reads owner values
[ ] (C) XP-06/07 consumers listed, not implemented pre-signature
```

### E.2.6 Point 6 — feedback

```text
[ ] Top confusion sources audited
[ ] Cause read model (value, delta, contributors, time-to-threat)
[ ] Contributor sources from the modifier stack/attributed deltas
[ ] One surface live; purity green
[ ] (C) forecast-vs-outcome consistency check
```

### E.2.7 Point 7 — onboarding

```text
[ ] Demand map (system, first encounter, taught before?)
[ ] Teach moments trigger on first real occurrence
[ ] Teach text values read from owners
[ ] Flow test: each teach fires once, before demand
[ ] Skip/return behavior preserved
```

### E.2.8 Point 8 — crisis/death

```text
[ ] Failure-outcome inventory
[ ] Recovery route per crisis family (or terminal-by-design record)
[ ] Boundary tests at crisis resolution
[ ] Death posture signed (soft/memorial/hardcore)
[ ] (C) continuation decision signed (no rewards per DEC-20)
```

### E.2.9 Point 9 — late game

```text
[ ] Escalation map by day window
[ ] Dead zones ≤ 20 days
[ ] Chapter table: pressure + opportunity per 30 days
[ ] Ending reachability audit
[ ] No infinite treadmill treadmill; arcs end
```

### E.2.10 Point 10 — agency

```text
[ ] Choice inventory (density per 10 days)
[ ] Dominance audit (strictly better options)
[ ] Quality pass within existing owners
[ ] Dead-zone weighting through existing event families
[ ] (C) consequence recorded for high-impact choices
```

---

## E.3 Soak harness detail

### E.3.1 Metric collection

```csharp
public sealed record DaySnapshot(
    int Day, IReadOnlyDictionary<string, float> Needs,
    IReadOnlyDictionary<string, float> Resources,
    int AliveCount, int DeathsToday, string? EconomyTier,
    IReadOnlyList<string> EventsToday, IReadOnlyList<string> ChoicesToday);
```

### E.3.2 Aggregate report

```
tt_first_X = first day where any need crosses its warning threshold
deaths_60 = deaths through day 60
pressure_events = count of event kinds flagged pressure
choice_density = choices / 10 days
tier_days = histogram
dead_zone_max = longest run with no pressure/choice
```

### E.3.3 Determinism rules

- One seed set, fixed order.
- No wall-clock in the harness.
- Traces stored per run for diffing.

---

## E.4 Worked tranche records

### E.4.1 Record — opening looseness

```markdown
### Tranche G1-A1
- Lever: pressure window "opening" hunger/thirst 1.0 → 0.8
- Seeds 1..20; days 60
- tt_first_thirst 2.1 → 3.8 (band 3–6) ✔
- tt_first_hunger 4.8 → 7.2 (band 6–10) ✔
- deaths_60 5.4 → 2.1 (band ≤3) ✔
- pressure_events 0.8 → 1.1 (band ≥1) ✔
- Files: pressure_curve.json; needs bind
- Revert: delete curve + bind
```

### E.4.2 Record — telegraph

```markdown
### Tranche G1-B2
- Lever: briefing time-to-threat for top 2 needs
- Source: NeedsSystem deltas + authored thresholds
- Test: thirst warning appears ≥1 day before threshold crossing on 20 seeds
- Files: briefing renderer + read model
```

### E.4.3 Record — difficulty bind

```markdown
### Tranche G2-B1
- Lever: crisis_deadline_mult bind to crisis scheduler
- Test: Sparing deadline = 1.25× Standard
- Monotonicity: deadlines non-increasing across difficulty ✔
```

---

## E.5 Extended scenario walks

### E.5.1 Scenario — a metric moves the wrong way

If `deaths_60` rises after an intended loosening, check for a *second*
application of the lever (double-count) or a coupled system responding
inversely. Fix the coupling, not the number.

### E.5.2 Scenario — bands cannot all hold simultaneously

Prioritize: survival clarity > difficulty honesty > choice density > late-game
escalation. Record which band yields and why.

### E.5.3 Scenario — a soak reveals a bug

Stop tuning; hand the repro to W2-02. Tuning around a bug encodes it.

### E.5.4 Scenario — the chooser picks Path C for Point 1 only

Dynamic pressure with everything else B: the dynamic element gets its own
honesty policy, cap, and soak; the rest proceeds unchanged.

---

## E.6 The gameplay knowledge base

### E.6.1 Owners the plan must respect

| Concern | Owner |
|---|---|
| needs | `NeedsSystem` (nine kinds, modifier stack, attributed deltas) |
| difficulty | preset catalog + live binding (XP-01) |
| economy pressure | market/prices/ration owners (read model only added) |
| radiation | `RadiationSystem`, dose ledger |
| food/nutrition | `KitchenNutritionSystem`, `NutritionDiversitySystem`, preservation/rationing |
| power/water | grid + water owners |
| skills | `SkillProgressionSystem` |
| crisis | crisis coordinator/predictor |
| chronicle | `CampaignCompletionHistory` v2 |
| death/fate | `SurvivorFateSystem`, `NeedsSystem.ForceDeath` |

### E.6.2 Anti-patterns

| Anti-pattern | Why |
|---|---|
| tuning multiple owners in one tranche | unattributable |
| UI-computed forecasts | fabricated |
| new progression store | Rule 5 |
| difficulty presets renamed | save/chronicle drift |
| hardcore hidden (no telegraph) | surprise wipe |
| rubber-banding without a cap | player distrust |

### E.6.3 One-page summary

- **What:** ten pacing/clarity/choice points with A/B/C each.
- **First:** G0 baseline + bands.
- **Smallest:** G0 alone (pure measurement).
- **Largest:** Path C arc work (signed separately).
- **Never:** new systems, fabricated UI values, unmeasured tuning.
- **Unblocking:** Annex U, signed, separate.

---

## E.7 Final checklist

```text
[ ] G0 baseline + bands
[ ] Pressure curve in band (or band revised)
[ ] Telegraph coverage complete
[ ] Difficulty scalars visible; monotone
[ ] Economy tiers live; boundary clean
[ ] Progression measured
[ ] Cause/forecast surface live
[ ] Onboarding teach-before-demand
[ ] Crisis recovery routes
[ ] No dead zones; endings reachable
[ ] Choice density band met
[ ] Tranche records attached
[ ] Annex U signatures recorded
```

**End of W2-03 execution playbook.**

*Document control: W2-03 · Wave 2 · HEAD 5be1a30a · proposal only.*