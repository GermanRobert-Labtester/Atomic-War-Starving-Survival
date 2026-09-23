# ASHFALL — WAVE 3 INTEGRATION PROGRAM · PLAN 4 OF 6

# COMBAT, DEFENSE & SECURITY INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W3 (six-plan integration wave)
**Document:** W3-04
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W3-01 (narrative), W3-02 (economy), W3-03 (psychology), W3-05 (crafting), W3-06 (UI)
**Plan-unblocking annex:** Annex U at the end — separately.

---

## 0. How to read this plan

This plan integrates the **violent and covert** side of the game: tactical
combat, weapon condition, defense (perimeter/sky), espionage and
counter-intelligence, prisoners, bounties, mercenaries, sound ranging, and
security consequences. It extends existing owners and keeps one authority per
concern. It does not write prose (W2-06) or tune balance bands (W2-03); it
makes combat/security truthful, reachable, and consequence-complete.

### 0.1 Two selection levels

| Plan Path | Name | Meaning |
|---|---|---|
| **A** | Truth & Safety | audit hosts, catalogs, persistence, and consequence routing; fix gaps |
| **B** | One Battlefield | unify combat/defense/security seams, make state legible, verify consequences |
| **C** | Long War | campaign-level escalation, occupation/insurgency, and strategy on existing owners |

**Level 2:** ten points, each A/B/C (§4.2).

### 0.2 Default mapping

| Plan Path | A points | B points | C points |
|---|---|---|---|
| A Truth & Safety | 1–10 | — | — |
| B One Battlefield | 1,4,7 | 2,3,5,6,8,9 | 10 |
| C Long War | — | 2,3 | 1,4,5,6,7,8,9,10 |

### 0.3 The Wave 3 rule for this plan

> **One combat authority, one defense authority, one intelligence authority.**
> `TacticalCombatSystem` owns resolution; ballistics/weapon condition are its
> subsystems; `PerimeterDefenseSystem` and `SkyDefenseBatterySystem` own their
> layers; `EspionageSystem`/`CounterIntelligenceSystem` own covert play. No
> parallel combat calculator, no hidden damage system, no second spy network.

### 0.4 Dignity and tone rules (binding)

1. Combat is dangerous and costly; it is never a power fantasy without
   consequence.
2. Prisoners and interrogation: consequences first; no torture mechanic that
   reads as reward.
3. No graphic cruelty in text; restrained register (W2-06 tone bible).
4. The dead on all sides are people; no dehumanizing language.
5. Espionage is tradecraft and risk, not glamour.

### 0.5 Vocabulary

| Term | Meaning |
|---|---|
| tactical layer | the combat resolution system |
| exposure | being visible/targetable in combat |
| condition | weapon/armor durability |
| perimeter | shelter defense layer |
| sky layer | air defense layer |
| heat | counter-intelligence attention state |
| cell | spy network node |
| bounty | faction reward for a target |
| ranging | sound-ranging threat observation |

---

## 1. Executive summary

ASHFALL's combat/security stack is broad and modular:

- **Tactical combat:** `TacticalCombatSystem` (+ `.Actions`, `.Breaching`,
  `.Damage`, `.Persistence`, `.Targeting` partials), `CombatCatalog`,
  `CombatTypes`, `CombatPerks`, `CombatDoctrineCapability`,
  `EnemyCompositionSelector`, `CombatAiMove`, `CombatFactionStandingBridge`,
  `CombatHeadlessDemo`.
- **Weapons/ballistics:** `BallisticsSystem`, `BallisticsWorkbenchSystem`,
  `BallisticShieldEngine`, `WeaponConditionSystem`, `WeaponEquipmentBridge`.
- **Stealth/breaching:** `Combat/StealthSystem`, `CombatBreachingEngine`.
- **Defense:** `Defense/DefenseSystem`, `PerimeterDefenseSystem` (+catalog),
  `SkyDefense/SkyDefenseBatterySystem` (+ordnance catalog),
  `AirlockSecuritySystem`, `Shelter/SkyLayerArmorSystem`.
- **Intelligence:** `Factions/EspionageSystem`, `CounterIntelligenceSystem`
  (+state), `ShelterEspionageSystem`, `EspionageConsequenceRouter`,
  `FactionIntelligenceCatalog`, `Factions/FactionBountySystem`,
  `Factions/PrisonerSystem`.
- **Pressure:** `DutyRoster/ShelterEncounterSystem`, `Muster/IronRaidersSystem`,
  `Combat/SoundRangingThreatEngine`, `Economy/MercenarySystem`,
  `YearOfAsh` storm/defense ties, `FactionWarSystem` (war chain).

The gaps:

1. **Combat host truth** — the partial system is complete, but its host wiring,
   save round-trip, and consequence routing need end-to-end verification
   (Point 1).
2. **Weapon/armor condition** — condition systems exist; their effect on
   combat resolution and repair loop needs a single truth (Point 2).
3. **Exposure/stealth** — stealth and targeting exist; legibility and
   consequence need verification (Point 3).
4. **Defense layers** — perimeter and sky defense exist; distinct roles, one
   alert story, and honest outcomes are the gap (Point 4).
5. **Espionage truth** — mission outcomes must route consequences through the
   existing router exactly once (Point 5).
6. **Counter-intelligence heat** — heat/attention state needs consequences
   and bounds (Point 6).
7. **Prisoners and bounties** — systems exist; ethics, terms, and consequence
   pathways need verification (Point 7).
8. **Sound ranging / threat observation** — observations feed from war events
   (debt row); consumption and response need audit (Point 8).
9. **Mercenary and faction standing** — combat outcomes should move faction
   standing through the bridge; verify (Point 9).
10. **War escalation and occupation** — the campaign war chain exists;
    occupation/insurgency depth is the C-path gap (Point 10).

---

## 2. Verified current state

### 2.1 Combat

| Component | Role |
|---|---|
| `TacticalCombatSystem` | resolution (partials for actions/breaching/damage/targeting/persistence) |
| `CombatCatalog` + `CombatTypes` + `CombatPerks` | data/definitions |
| `EnemyCompositionSelector` | enemy generation |
| `CombatAiMove` | AI movement |
| `CombatFactionStandingBridge` | outcome → standing |
| `BallisticsSystem`, `BallisticShieldEngine` | projectile/shield math |
| `WeaponConditionSystem` | durability |
| `WeaponEquipmentBridge` | equipment effects |
| `StealthSystem`, `CombatBreachingEngine` | stealth/breach |
| `CombatDoctrineCapability` | doctrine capabilities |

### 2.2 Defense

| Component | Role |
|---|---|
| `PerimeterDefenseSystem` + catalog | ground defense |
| `SkyDefenseBatterySystem` + ordnance catalog | air defense |
| `DefenseSystem` | generic defense |
| `AirlockSecuritySystem` | entry security |
| `SkyLayerArmorSystem` | sky armor (weather/hazard interaction, W2-04) |

### 2.3 Security/intelligence

| Component | Role |
|---|---|
| `EspionageSystem` | missions |
| `CounterIntelligenceSystem` + state | defense against spies |
| `ShelterEspionageSystem` | shelter-specific |
| `EspionageConsequenceRouter` | consequence routing |
| `FactionIntelligenceCatalog` | intel data |
| `PrisonerSystem` | captives |
| `FactionBountySystem` | bounties |
| `MercenarySystem` | hired force |
| `IronRaidersSystem` | raider threat |
| `ShelterEncounterSystem` | shelter attacks |
| `SoundRangingThreatEngine` + catalog | threat observations |

### 2.4 Data

`combat` data (verify at P0), ordnance catalog, perimeter catalog, intel
catalog, sound-ranging catalog; see `ls Assets/StreamingAssets/Data` families
at execution.

### 2.5 Known integration facts from earlier waves

- `SkyDefenseBatterySystem` has a live panel + selftest (17/17) and an
  additive read-only `OrdnanceCatalog` (wave-8 B2 evidence).
- `SoundRangingThreatEngine` is fed from `FactionWarSystem.OnTerritorialClashOccurred`
  (DEBT-PLAN123 RETIRED evidence).
- `CombatFactionStandingBridge` exists — verify consumption.
- `WeaponConditionSystem` pairs with crafting repair (W3-05).

---

## 3. Scope, non-goals, rules

### 3.1 In scope

- Combat host wiring, save round-trip, and consequence truth.
- Weapon/armor condition effects and repair loop.
- Stealth/exposure legibility and outcomes.
- Defense layer roles and one alert path.
- Espionage mission truth and consequence-once.
- Counter-intel heat consequences and bounds.
- Prisoner/bounty systems ethics and completion.
- Sound-ranging observations and response.
- Faction standing movement from outcomes.
- War escalation/occupation depth (C).

### 3.2 Non-goals

- New combat engine.
- Balance tuning (W2-03 bands; combat numbers may be measured but not tuned
  here).
- Prose (W2-06).
- Environment/sky mechanics (W2-04).
- Save schema without signature.

### 3.3 Rules

1. Resolution stays in `TacticalCombatSystem`; no second calculator.
2. Outcomes route consequences exactly once (idempotency keys).
3. Every defense layer has a distinct authored role; alerts are one path.
4. Heat/standing changes are attributed and bounded.
5. Prisoner/interrogation mechanics follow the dignity rules; coercion yields
   unreliable information (never a torture buff).
6. Determinism: combat rolls seeded; replays equal.

---

## 4. Plan Path and decision index

### 4.1 The ten points

| # | Point | Default |
|---|---|---|
| 1 | Combat host truth and persistence | B |
| 2 | Weapon/armor condition and repair loop | B |
| 3 | Stealth, exposure, and targeting legibility | A |
| 4 | Defense layer roles and alert path | B |
| 5 | Espionage mission truth and consequence-once | B |
| 6 | Counter-intelligence heat and bounds | B |
| 7 | Prisoner and bounty ethics/completion | B |
| 8 | Sound ranging observation and response | B |
| 9 | Faction standing from outcomes | B |
| 10 | War escalation and occupation depth | C |

### 4.2 Selection sheet

```text
PLAN W3-04 — COMBAT, DEFENSE & SECURITY
Plan Path: [ ] A Truth & Safety  [ ] B One Battlefield (default)  [ ] C Long War

01 combat host truth ...... [A] [B] [C]   default B
02 condition/repair ....... [A] [B] [C]   default B
03 stealth/exposure ....... [A] [B] [C]   default A
04 defense layers/alerts .. [A] [B] [C]   default B
05 espionage truth ........ [A] [B] [C]   default B
06 counter-intel heat ..... [A] [B] [C]   default B
07 prisoners/bounties ..... [A] [B] [C]   default B
08 sound ranging .......... [A] [B] [C]   default B
09 faction standing ....... [A] [B] [C]   default B
10 war escalation ......... [A] [B] [C]   default C
```

---

## 5. Decision Point 1 — Combat host truth and persistence (default B)

### 5.1 The design question

Tactical combat has six partials and a persistence layer. The audit: does a
combat started by each entry path (expedition binder, shelter encounter, war
event) resolve, persist mid-fight, and restore correctly?

### 5.2 Path A — Entry/persistence audit

- Map entry paths → `TacticalCombatSystem` → save/restore → outcome routing.
- Report entry paths with no persistence or no outcome route.

### 5.3 Path B — Truth + mid-fight round-trip

- Verify/repair mid-fight capture/restore (the `.Persistence` partial exists).
- Outcome routing through `CombatFactionStandingBridge` and the consequence
  ledger exactly once.
- Tests: start → save → load → resume equals continuous; outcome recorded once.

### 5.4 Path C — Combat replay

Path B, plus a deterministic replay harness for a combat (seed + orders →
identical result), used by W3-01 for narrative consequences.

### 5.5 Acceptance

- Every entry path persists and resolves.
- Outcome exactly once; replay equality (C).

---

## 6. Decision Point 2 — Weapon/armor condition and repair loop (default B)

### 6.1 The design question

`WeaponConditionSystem` + equipment bridge + crafting repair (W3-05). Condition
must affect combat measurably and be repairable through an owned path.

### 6.2 Path A — Effect audit

- Map condition → combat effect → repair action → cost.
- Report condition with no effect or repair with no cost.

### 6.3 Path B — Verified wear

- Condition applies its authored effect at resolution; repair consumes
  materials/time through crafting owners; broken gear is unusable per authored
  rule.
- Tests: worn weapon resolves worse (or fails per rule); repair restores;
  materials consumed.

### 6.4 Path C — Field maintenance

Path B, plus in-field maintenance kits (items) through crafting; content via
W2-06.

### 6.5 Acceptance

- Wear matters; repair costs; no free repair.

---

## 7. Decision Point 3 — Stealth, exposure, and targeting legibility (default A)

### 7.1 The design question

Stealth/exposure mechanics exist; the player should understand why they were
seen (or not).

### 7.2 Path A — Legibility audit

- Map exposure factors (light, noise, stance, gear) → detection → consequence.
- Report unexplained detections.

### 7.3 Path B — Explainable detection

- Exposure read model shows the dominant factors; stealth outcomes attributed.
- Tests: factor changes move detection monotonically.

### 7.4 Path C — Stealth arcs

Path B, plus authored infiltration routes (W3-01).

### 7.5 Acceptance

- Detection factors named; monotone tests.

---

## 8. Decision Point 4 — Defense layer roles and alert path (default B)

### 8.1 The design question

Perimeter, sky, airlock, and sky armor layers exist. Each must have a distinct
role and feed **one** alert path (Plan 194's crisis producers are the pattern).

### 8.2 Path A — Role audit

- Table: layer → threat type → response → alert.
- Report overlapping roles and alert paths that bypass the canonical crisis
  route.

### 8.3 Path B — One alert story

- Alerts route through the existing crisis/emergency owners (fire/flood/rad
  producers precedent); layers report state, crisis presents.
- Tests: each layer raises the alert once; response reduces damage measurably.

### 8.4 Path C — Layered defense doctrine

Path B, plus authored doctrine choices (guns vs. armor vs. concealment) with
tradeoffs through existing owners.

### 8.5 Acceptance

- Distinct roles; single alert path; response measurable.

---

## 9. Decision Point 5 — Espionage mission truth and consequence-once (default B)

### 9.1 The design question

`EspionageSystem` missions + `EspionageConsequenceRouter`. Every mission must
have a real outcome and route consequences exactly once.

### 9.2 Path A — Mission audit

- Map mission types → success/failure → consequences → journal/flags.
- Report no-consequence missions.

### 9.3 Path B — Consequence truth

- Router applies outcomes once (idempotency key per mission); failures have
  consequences and cost.
- Tests: success changes the target state; failure raises detection;
  double-routing prevented.

### 9.4 Path C — Spy networks

Path B, plus cell/network state on the espionage owner (additive, signed),
with W3-01 narrative integration.

### 9.5 Acceptance

- Every mission has consequences; exactly once; failure cost.

---

## 10. Decision Point 6 — Counter-intelligence heat and bounds (default B)

### 10.1 The design question

`CounterIntelligenceSystem` + heat state. Heat must accrue, decay, and have
consequences (and never a silent execution).

### 10.2 Path A — Heat audit

- Map heat sources → thresholds → consequences → decay.
- Report unconsumed thresholds and undecaying heat.

### 10.3 Path B — Bounded heat

- Heat consequences authored and warned (telegraph seam); decay exists; caps
  enforced.
- Tests: threshold event fires once (or per authored cycle); decay returns to
  baseline; no unwarned punitive outcome.

### 10.4 Path C — Counter-intel operations

Path B, plus active operations (double agents) through existing owners and
W3-01 flags.

### 10.5 Acceptance

- Heat bounded; consequence warned; decay tested.

---

## 11. Decision Point 7 — Prisoner and bounty ethics/completion (default B)

### 11.1 The design question

`PrisonerSystem` and `FactionBountySystem` exist. Their loops must complete
(capture → terms → outcome), respect the ethics rules, and route consequences.

### 11.2 Path A — Loop audit

- Map capture → holding → terms/ransom/labor → release/exchange → consequences.
- Report loops with no exit.

### 11.3 Path B — Completed terms

- Every prisoner has authored outcomes with consequences through owners;
  coercion yields unreliable information (authored probability), never a
  guaranteed reward.
- Bounties complete with faction standing effects (Point 9).
- Tests: each outcome path reachable; coercion unreliability sampled; no
  endless holding without cost.

### 11.4 Path C — Prisoner narrative

Path B, plus narrative ties (W3-01) with dignity review.

### 11.5 Acceptance

- No dead-end loops; ethics respected; consequences routed.

---

## 12. Decision Point 8 — Sound ranging observation and response (default B)

### 12.1 The design question

`SoundRangingThreatEngine` observations come from war events (verified debt).
The response loop (observation → warning → preparation) needs audit.

### 12.2 Path A — Response audit

- Map observation → consumer → player-visible warning/preparation.
- Report observations with no consumer.

### 12.3 Path B — Warning loop

- Observations feed the briefing/crisis warn path (W2-03/W2-04 seams); the
  player can prepare.
- Tests: a war event produces an observation; the warning appears; preparation
  changes the outcome.

### 12.4 Path C — Threat forensics

Path B, plus an authored ranging read model (direction/type/confidence) with
W3-06 surface.

### 12.5 Acceptance

- Observation has a consumer; warning exists; preparation matters.

---

## 13. Decision Point 9 — Faction standing from outcomes (default B)

### 13.1 The design question

`CombatFactionStandingBridge` exists (also espionage/war outcomes). Standing
changes must be attributed, bounded, and consumed by economy (W3-02 stance).

### 13.2 Path A — Attribution audit

- Map outcome types → standing deltas → consumers.
- Report unattributed or double-applied deltas.

### 13.3 Path B — Bounded standing

- Deltas attributed per event and clamped; consumed by stance/trade/faction
  systems.
- Tests: an outcome moves standing once; clamp respected; stance effect
  visible (W3-02).

### 13.4 Path C — Reputation arcs

Path B, plus authored reputation arcs (W3-01).

### 13.5 Acceptance

- Attributed, bounded, consumed.

---

## 14. Decision Point 10 — War escalation and occupation depth (default C)

### 14.1 The design question

The campaign war chain exists (stages, clashes, decrees, warnings). The C-path
depth: occupation/insurgency, front movement, and strategy — all on existing
owners.

### 14.2 Path A — Escalation audit

- Map war stages → events → consequences → player options.
- Report stages with no player-facing effect.

### 14.3 Path B — Consequence completeness

- Every stage has at least one consequence and one player option through
  existing systems (defense, trade, narrative).

### 14.4 Path C — Occupation model

Path B, plus occupation state (who controls what) on the war/faction owner
(additive, signed), with insurgency options through existing systems and
W3-01 narrative.

### 14.5 Acceptance

- No stage without consequence/option.
- (C) occupation signed and tested.

---

## 15. Execution phases

### CS0 — Premise freeze (1 day)

- Verify systems/data; run combat/defense selftests; produce
  `P0_COMBAT_PREMISE.md`.

### CS1 — Combat truth (Points 1 + 2)

- Entry/persistence; condition/repair.

### CS2 — Stealth/defense (Points 3 + 4)

- Legibility; layer roles/alerts.

### CS3 — Intelligence (Points 5 + 6)

- Mission consequences; heat bounds.

### CS4 — Prisoners/ranging/standing (Points 7 + 8 + 9)

- Loop completion; warning loop; standing bounds.

### CS5 — Escalation (Point 10, C)

- Stage completeness; occupation.

### CS6 — Closeout

- Evidence; ledger proposals; Annex U.

---

## 16. Verification plan

| Point | Evidence |
|---|---|
| 1 | entry map; mid-fight round-trip; outcome once |
| 2 | wear effect; repair cost |
| 3 | detection factors; monotone |
| 4 | layer table; alert once; response measurable |
| 5 | mission consequence once |
| 6 | heat bounds/decay/warned |
| 7 | loop exits; coercion unreliable sampled |
| 8 | observation → warning → preparation |
| 9 | standing attributed/bounded |
| 10 | stage consequence/option; occupation (C) |

Commands:

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Combat
bash scripts/run_test.sh Ashfall.Core.Tests/Defense    # if present
bash scripts/run_test.sh Ashfall.Core.Tests/Factions
godot --headless --path . -- --7day-smoke-selftest
```

---

## 17. Risks

| # | Risk | L | I | Mitigation |
|---|---|---|---|---|
| 1 | combat refactor changes balance | M | H | no tuning; truth only |
| 2 | double consequence routing | M | M | idempotency keys |
| 3 | heat becomes unwarned punishment | M | M | telegraph seam |
| 4 | prisoner loop rewards cruelty | L | H | ethics rule; unreliable coercion |
| 5 | defense layers overlap | M | M | role table |
| 6 | standing runaway | M | M | clamps |
| 7 | occupation persistence unowned | M | H | signed line only |
| 8 | concurrent claims on combat files | M | H | single-writer |
| 9 | tone drift in security prose | M | M | W2-06 review |
| 10 | scope creep to strategy game | M | H | Wave 3 rule |

---

## 18. Ownership and claims

| Phase | Claim | Paths |
|---|---|---|
| CS0 | `W3-04-CS0-PREMISE` | premise doc |
| CS1 | `W3-04-CS1-COMBAT-TRUTH` | combat host + condition |
| CS2 | `W3-04-CS2-STEALTH-DEFENSE` | stealth read + layer alerts |
| CS3 | `W3-04-CS3-INTEL` | espionage router + heat |
| CS4 | `W3-04-CS4-PRISONERS-RANGING-STANDING` | loops + warnings + standing |
| CS5 | `W3-04-CS5-ESCALATION` | stage completeness (+ occupation) |
| CS6 | `W3-04-CS6-CLOSEOUT` | evidence + proposals |

Coordination: W3-02 owns economy consequences; W3-03 owns trauma outcomes;
W3-05 owns repair materials; W3-01 owns narrative ties; W2-04 owns sky/weather
inputs.

---

## 19. Rollback and decline

| Point | Rollback | Decline consequence |
|---|---|---|
| 1 | keep audit | persistence gaps remain |
| 2 | keep audit | wear unverified |
| 3 | keep audit | detection unexplained |
| 4 | keep audit | overlapping layers remain |
| 5 | keep audit | missions may be consequence-free |
| 6 | keep bounds audit | heat unbounded |
| 7 | keep audit | dead-end loops remain |
| 8 | keep audit | observations unconsumed |
| 9 | keep audit | standing unverified |
| 10 | keep audit | stages may lack options |

---

## 20. DoD and handoff

**Path A:** premise + all audits complete; selftests recorded.

**Path B:** all of A, plus combat truth, wear/repair, legibility, alert unity,
consequence-once, bounded heat, completed prisoner loops, warning loop, bounded
standing — each tested.

**Path C:** all of B, plus occupation/insurgency state (signed) and stage
depth.

**Handoff:** outcome, files, contract (authorities/consequences), commands,
limitations, untouched shared paths, ledger proposals, Annex U.

### 20.1 First safe step

> CS0 only: premise and selftests. No mechanic changes first.

---

# ANNEX U — PLAN-UNBLOCKING (SEPARATELY)

## U.1 What W3-04 releases

| Blocked item | Mechanism | Gate |
|---|---|---|
| Expansion 20 (Quiet Hand) | Espionage/heat/prisoner truth is its mechanical substrate | CS3/CS4 |
| Expansion 23 (Alarm) | Defense layers + alert path complete the shelter-alarm chain | CS2 |
| Expansion 21 (Grid) defense interplay | Sky/defense power demand verified via owners | CS2 |
| Expansion 14 (Above the Ash) | Sky defense/ordnance read path verified | CS1/CS2 |
| W3-01 narrative consequences | Combat/intel outcomes route through flags/ledger | CS3/CS4 |
| W3-03 trauma | Combat outcomes feed trauma catalog | CS1 |
| EN-01 war severity (consumer) | Stage consequences verified | CS5 |
| Plan 30 war clock (already sealed) | Escalation stage completeness extends its contract | CS5 |

## U.2 Signatures needed

```text
[ ] I authorize CS0 premise + selftests.
[ ] I authorize CS1 combat host persistence + condition/repair truth.
[ ] I authorize CS2 stealth legibility + defense layer alert unification.
[ ] I authorize CS3 espionage consequence-once + counter-intel heat bounds.
[ ] I authorize CS4 prisoner/bounty loop completion + ranging warning loop +
    standing bounds.
[ ] I authorize CS5 stage completeness (+ occupation state: [ ] no [ ] signed).
```

## U.3 What W3-04 never touches for unblocking

- Balance bands (W2-03).
- Prose (W2-06).
- Environment/sky mechanics (W2-04).
- Economy/psychology/narrative owners (W3-02/03/01).
- Save schema without signature.

## U.4 The security-release rule

A combat/security feature releases when its resolution, consequence, and
recovery/aftermath paths exist. An event that changes state without a routed
consequence releases nothing.

---

# APPENDICES

## A.1 Selection sheet

```text
ASHFALL WAVE 3 · PLAN 4 (COMBAT/SECURITY) · SELECTION
Date: ______  Foreman: ______  HEAD: ______
PLAN PATH: [ ] A Truth & Safety  [ ] B One Battlefield (default)  [ ] C Long War

01 combat host truth ...... [A] [B] [C]   default B
02 condition/repair ....... [A] [B] [C]   default B
03 stealth/exposure ....... [A] [B] [C]   default A
04 defense layers/alerts .. [A] [B] [C]   default B
05 espionage truth ........ [A] [B] [C]   default B
06 counter-intel heat ..... [A] [B] [C]   default B
07 prisoners/bounties ..... [A] [B] [C]   default B
08 sound ranging .......... [A] [B] [C]   default B
09 faction standing ....... [A] [B] [C]   default B
10 war escalation ......... [A] [B] [C]   default C
Signature: ________________
```

## A.2 Ethics checklist (per tranche)

```text
[ ] Combat has cost; no gratuitous detail
[ ] Coercion unreliable; no torture buff
[ ] Prisoners have terms and exits
[ ] No dehumanizing language
[ ] Security consequences warned
[ ] Tone review passed
```

## A.3 Glossary

| Term | Meaning |
|---|---|
| entry path | how combat starts (binder/encounter/war) |
| condition | durability state of weapons/armor |
| exposure | detectability factors |
| alert path | the canonical crisis/emergency route |
| heat | counter-intelligence attention |
| cell | espionage network node |
| ranging | sound-based threat observation |
| standing | faction disposition delta |

**End of Part I.** Proposal only; executes nothing; releases nothing without
U.2 signatures.

---

# PART II — DEEP DESIGN SPECIFICATIONS (CONTINUED → 180K)# W3-04 · PART II — DEEP DESIGN: ALL TEN POINTS

> Appended 2026-09-21. One part, ten designs. Each point gets: problem, owner
> map, contract, algorithms, failure classes, tests. Proposal only.

---

## §II.1 Point 1 — Combat host truth and persistence (default B)

### Problem

`TacticalCombatSystem` spans six partials (Actions, Breaching, Damage,
Persistence, Targeting, core). Combat starts from multiple entry paths
(expedition binder, shelter encounter, war event, scripted ambush). The audit:
does every path initiate, resolve, persist mid-fight, restore, and route its
outcome exactly once?

### Owner map (P0 targets)

| Concern | Owner |
|---|---|
| resolution | `TacticalCombatSystem` |
| targeting | `.Targeting` partial |
| damage | `.Damage` partial |
| breaching | `CombatBreachingEngine` |
| persistence | `.Persistence` partial |
| standing consequences | `CombatFactionStandingBridge` |
| enemy selection | `EnemyCompositionSelector` |

### Contract

```text
CombatSession:
  id, entry (binder|shelter|war|scripted), participants, seed,
  state (active|resolved|aborted), outcome (win|loss|retreat|partial)
Outcome routing:
  exactly once via idempotency key combat:<session-id>
  consequences: casualties, wounds, standing, loot, narrative refs
```

### The entry-path audit procedure

```text
for each entry E:
  1. initiate a combat with scripted state
  2. assert session created with seed + participants
  3. resolve to each outcome type (scripted order/inputs)
  4. assert outcome recorded once; consequences applied once
  5. mid-fight: save; load; assert state (participants, HP, positions)
  6. resume; assert completion equals uninterrupted run (same seed)
```

### Failure classes

| Class | Example | Repair |
|---|---|---|
| no persistence | mid-fight save restarts combat | wire `.Persistence` for the path |
| double outcome | standing applied twice (bridge + direct) | idempotency key |
| aborted session leak | combat cancelled, session state lingers | abort path clears |
| seed loss | load restarts RNG sequence | seed restored with session |
| entry divergence | one entry applies modifiers others don't | single init path |

### Tests

```text
CombatEntry_<E>_InitiatesAndPersists
CombatOutcome_Once_<type>
CombatRoundTrip_<entry>
CombatSeed_ReplayEquals
```

### Acceptance

All entries persist and resolve; outcome once; replay equal; no leaked
sessions after aborts.

---

## §II.2 Point 2 — Weapon/armor condition and repair loop (default B)

### Problem

`WeaponConditionSystem` + `WeaponEquipmentBridge` exist; crafting provides
repair (W3-05). Condition must measurably affect resolution, break per
authored rules, and repair through a real cost loop.

### Condition model

```text
Condition(item): [0..1]
bands: pristine(.8+), worn(.5-.8), damaged(.2-.5), broken(<.2)
effects (authored): accuracy/jam/armor-absorb modifiers per band
decay: per use (combat rounds), per event (breach), per time (neglect)
repair: materials + time + station (W3-05 recipes)
```

### Contract

- Condition lives on the item/equipment owner (single writer: condition
  system).
- Combat reads bands and applies modifiers at resolution (no second formula).
- Repair consumes materials through the crafting owner and restores per
  authored amounts.
- Broken = authored state (unusable / improvised), never silent no-op.

### Failure classes

| Class | Example | Repair |
|---|---|---|
| decorative condition | worn weapon resolves identically | apply band modifiers |
| double decay | condition drops twice per round (two callers) | single write point per event |
| free repair | repair action without materials | route consumption |
| broken no-op | broken item still usable | authored broken behavior |
| unbounded repair | repair above 1.0 | clamp |

### Tests

```text
Weapon_WornResolvesWorse
Weapon_BrokenAuthoredBehavior
Weapon_DecaySinglePerEvent
Repair_ConsumesMaterials
Repair_RestoresPerAuthored
Condition_Bounds
```

### Acceptance

Wear matters; repair costs; bands apply once; broken state authored.

---

## §II.3 Point 3 — Stealth, exposure, and targeting legibility (default A)

### Problem

`StealthSystem` + `.Targeting` exist. The audit: can the player understand
detection (why seen/not), and does each exposure factor move detection
monotonically?

### Exposure model

```text
Exposure = f(light, noise, stance, gear, distance, cover)
  each factor authored [0..1] with monotone direction
  detection chance authored curve over exposure
  attribution: the dominant factors list (for legibility)
```

### Contract

- Detection randomness is seeded.
- The surface (W3-06) can show dominant factors post-event (learning).
- Factors sourced from owners (light: time/weather; noise: authored action
  class; gear: equipment owner).

### Failure classes

| Class | Example | Repair |
|---|---|---|
| unexplained detection | no factor attribution | attribution list |
| non-monotone factor | louder = less likely seen | formula fix |
| hidden factor | detection uses undeclared state | declare + source |
| unseeded | stealth rolls unseeded | seeded facade |

### Tests

```text
Stealth_FactorMonotonicity_<factor>
Stealth_AttributionPresent
Stealth_ReplayEqual
Stealth_NoHiddenFactors
```

### Acceptance

Factors named, monotone, sourced; detection explainable.

---

## §II.4 Point 4 — Defense layer roles and alert path (default B)

### Problem

Perimeter, sky, airlock, and sky-armor layers exist. Each must have a
distinct role and feed one alert path (existing crisis producers are the
pattern: fire/flood/rad).

### Layer table (P0 fill)

| Layer | Threat | Response | Alert |
|---|---|---|---|
| airlock security | breach/entry | lock/deny | intrusion crisis |
| perimeter | ground raid | posts/engines | raid warning |
| sky battery | air/missile | intercept | sky warning |
| sky armor | fallout/ash | mitigation | (environment warning, W2-04) |

### Contract

- Each layer: state (operational/degraded/down), inputs (power/ammo/crew),
  response resolution, alert emission.
- Alerts route through the single crisis path with attribution (which layer).
- Responses measurable: degradation reduces interception success.

### Failure classes

| Class | Example | Repair |
|---|---|---|
| overlapping roles | two layers both "stop raids" | role table split |
| silent alert | layer fires without alert | route to crisis |
| decorative layer | interception success constant | apply degradation |
| unpowered still works | power input ignored | input consumption |

### Tests

```text
Defense_LayerRole_<layer>
Defense_DegradeReducesResponse
Defense_AlertOnce_PerEvent
Defense_PowerConsumed
Airlock_DenyAndAlert
```

### Acceptance

Distinct roles; degradation measured; alerts once; inputs consumed.

---

## §II.5 Point 5 — Espionage mission truth and consequence-once (default B)

### Problem

`EspionageSystem` + `EspionageConsequenceRouter` + `FactionIntelligenceCatalog`.
Missions must have real outcomes; consequences routed once; failures costly.

### Mission model

```text
Mission: id, type (recon|sabotage|theft|recruit|disinfo), target,
  difficulty (authored), team, cost, duration
Outcome: success|partial|failure|blown (each with consequences)
Consequences: intel gained, target state change, detection heat (P6),
  standing, narrative refs
```

### Contract

- Outcomes seeded (difficulty + skill + situation).
- Router applies exactly once (key `esp:<mission-id>`).
- Failure consequences: cost + detection (bounded).
- Success changes target state *through the target owner* (not a copy).

### Failure classes

| Class | Example | Repair |
|---|---|---|
| no-consequence mission | success does nothing | route effects |
| double route | heat + standing applied twice | idempotency |
| copy state | intel stored separately, stale | read target owner |
| free failure | blown mission costs nothing | author costs |
| unseeded | outcomes random unseeded | seeded facade |

### Tests

```text
Espionage_MissionOutcome_<type>
Espionage_ConsequenceOnce
Espionage_TargetStateOwnerRead
Espionage_FailureCosts
Espionage_ReplayEqual
```

### Acceptance

Every mission matters; once; failures cost; owner-sourced state.

---

## §II.6 Point 6 — Counter-intelligence heat and bounds (default B)

### Problem

`CounterIntelligenceSystem` + state. Heat must accrue, decay, and have
warned, bounded consequences.

### Model

```text
Heat sources: mission volume, blown ops, snitching, faction pressure
Thresholds: T1 watch, T2 watchlist, T3 warrant
Consequences: surveillance (authored events), interrogations (scenes),
  raid (combat path)
Decay: authored rates; hiding reduces
Cap: authored; reachable only deliberately
```

### Contract

- Single writer (counter-intel owner).
- Every threshold consequence is warned (at least one visible signal) and has
  an avoidance path.
- Raid routes to W3-04 P1 combat resolution (no side combat engine).
- Decay verified; cap enforced.

### Failure classes

| Class | Example | Repair |
|---|---|---|
| silent warrant | raid with no warning | add warning + avoidance |
| unbounded heat | heat grows without cap | cap |
| double writer | two systems raise heat | single writer |
| undecaying | heat never falls | decay rates |
| instant punitive raid | T3 on same tick as T2 | threshold pacing |

### Tests

```text
CounterIntel_Bounds
CounterIntel_DecayToBaseline
CounterIntel_WarnedThresholds_<T>
CounterIntel_SingleWriter
CounterIntel_RaidRoutesToCombat
```

### Acceptance

Bounded, warned, decaying, routed.

---

## §II.7 Point 7 — Prisoner and bounty ethics/completion (default B)

### Problem

`PrisonerSystem` + `FactionBountySystem` exist. Loops must complete
(capture → terms → outcome), respect ethics, route consequences.

### Prisoner loop

```text
Capture -> holding (costs: food/space/guard) -> terms:
  [ransom | exchange | labor | release | tribunal-authored]
-> outcome effects (standing, items, narrative, character arc)
Coercion: authored unreliable-info rule (probability of false intel)
```

### Ethics rules (binding)

1. No torture-reward mechanic; coercion yields unreliable information
   (authored chance of false/incomplete).
2. Prisoners have needs (food/water) and dignity; endless holding costs and
   triggers authored scenes (not a silent optimal strategy).
3. Every holding has at least one exit path (release at minimum).

### Bounty loop

```text
Bounty: target ref, faction, reward, duration
Completion: target state (dead/captured) -> reward + standing
Failure/expiry: authored consequences (none punitive-beyond-auth)
```

### Failure classes

| Class | Example | Repair |
|---|---|---|
| dead-end | prisoner held forever, no exit | exits authored |
| free reward | coercion always gives true intel | unreliability rule |
| double consequence | standing applied on capture and delivery | key |
| cost-free holding | prisoners consume nothing | needs routing |
| exploitable loop | labor infinitely profitable | cap/diminish |

### Tests

```text
Prisoner_ExitsExist
Prisoner_HoldingCosts
Prisoner_CoercionUnreliable_Sampled
Bounty_CompletionOnce
Bounty_ExpiryAuthored
```

### Acceptance

No dead ends; ethics respected; consequences once.

---

## §II.8 Point 8 — Sound ranging observation and response (default B)

### Problem

`SoundRangingThreatEngine` observations come from war events (verified debt
row). The response loop (observation → warning → preparation) needs audit.

### Model

```text
Observation: direction, type (armor|foot|arty|unknown), confidence, day
Consumer: briefing/warning surface (W3-06), defense readiness (P4)
Preparation: authored actions (posts, curfew, evacuation) affecting outcome
```

### Contract

- Observations originate from events (war clashes) — no free-floating
  ranging.
- Every observation has a visible consumer (warning) within authored time.
- Preparation measurably affects defense outcome (P4 link).

### Failure classes

| Class | Example | Repair |
|---|---|---|
| orphan observations | generated, never shown | wire consumer |
| useless warning | shown, preparation does nothing | outcome link |
| confidence ignored | unknown shown as certain | authored presentation |
| bundled spam | 20 observations per tick | aggregation/cooldown |

### Tests

```text
Ranging_ObservationHasConsumer
Ranging_PreparationAffectsOutcome
Ranging_ConfidencePresentation
Ranging_NoSpam
```

### Acceptance

Observations consumed; warnings useful; preparation matters.

---

## §II.9 Point 9 — Faction standing from outcomes (default B)

### Problem

`CombatFactionStandingBridge` exists (also espionage/war outcomes). Standing
changes must be attributed, bounded, consumed by economy (W3-02 stance).

### Model

```text
Outcome types -> authored deltas (bounded per event, clamped daily)
Attribution: event ref per delta
Consumers: stance bands (economy), narrative gates, bounty availability
```

### Contract

- Single bridge for combat/espionage/war outcomes (no scattered writes).
- Clamps: per event, per day; band transitions warned (W3-02).
- Deltas attributed in the ledger.

### Failure classes

| Class | Example | Repair |
|---|---|---|
| scattered writes | standing moved in three places | one bridge |
| double application | combat + loot both move standing | attribution keys |
| runaway | 10 events -> extreme stance in a day | clamps |
| invisible | band change with no signal | warning |

### Tests

```text
Standing_AttributedOnce
Standing_Clamped_Daily
Standing_BandVisible
Standing_ConsumedByEconomy
```

### Acceptance

One bridge; bounded; attributed; consumed.

---

## §II.10 Point 10 — War escalation and occupation depth (default C)

### Problem

The war chain exists (stages, clashes, decrees, warnings). The depth
question: do stages have player-facing effects and options; and (C) can
occupation state be modeled?

### Stage completeness model

```text
Stage: id, entry event, effects (territory, routes, stance, prices),
  player options (defense prep, diplomacy, trade routes, narrative choices)
Audit: every stage has >=1 effect and >=1 option, routed to owners.
```

### Occupation model (C, signed)

```text
Occupation: territory ref, controller, tax/levy, resistance level,
  player leverage (insurgency options through existing systems)
Persisted via the war/faction owner's existing section (or signed extension).
```

### Failure classes

| Class | Example | Repair |
|---|---|---|
| cosmetic stage | stage 3 changes nothing the player feels | route effects |
| option-less stage | player can only wait | author options |
| occupation freeze | territory state stuck | transition rules |
| insurgency parallel | a second resistance system | route through existing |

### Tests

```text
WarStage_EffectPresent_<stage>
WarStage_OptionPresent_<stage>
Occupation_Transitions (C)
Occupation_RoutedThroughOwners (C)
```

### Acceptance

Stage effects/options total; (C) occupation signed and tested.

---

## §II.11 The combat/security invariants (whole plan)

```text
1. One resolution authority (TacticalCombatSystem).
2. One outcome route (bridge/router) with idempotency.
3. Every defense layer distinct and alerting once.
4. Every punitive path warned with an exit.
5. Coercion never rewarded with certainty.
6. All stochastic paths seeded; replays equal.
7. Mid-fight persistence on every entry path.
```

---

## §II.12 Cross-point dependencies

```text
P1 resolution ──► P2 condition effects, P5 missions outcomes (raid), P7 capture
P3 stealth ──► P5 mission infiltration, P1 ambush
P4 defense ──► P6 raids, P8 preparation outcomes
P5 espionage ──► P6 heat, P9 standing
P6 counter-intel ──► P5 blown ops, P4 raid routing
P7 prisoners ──► P1 capture, P9 standing
P8 ranging ──► P4 readiness
P9 standing ──► W3-02 stance
P10 war ──► all of the above as event source
```

---

## §II.13 Execution order (Points 1–10)

```text
Week 1  P1 entry/persistence audit + P2 condition audit (shared scenario)
Week 2  P1 repairs + P3 attribution; P4 layer table
Week 3  P5 mission audit + P6 heat audit
Week 4  P7 prisoners/bounties + P8 ranging + P9 standing
Week 5  P10 stage completeness (+ occupation if signed)
Week 6  consolidation + soak + closeout
```

---

*End of Part II. Continues in Part III (playbooks).*# W3-04 · PART III — AUTHORING PLAYBOOKS, VERIFICATION, AND WORKED THREADS

---

## §III.1 Encounter and battle authoring playbook

### III.1.1 Encounter template

```yaml
encounter: ridge_ambush
entry: binder | shelter | war | scripted
composition: authored enemy set (selector ref)
terrain: [cover_class, exposure_class]
objective: [survive | repel | breach | capture]
outcomes:
  win: [consequences]
  loss: [consequences]
  retreat: [consequences]
  partial: [consequences]
persist: yes/no (must be yes for >1 round encounters)
seed: session seed
narrative: hooks (W3-01)
```

### III.1.2 Authoring rules

1. **Every encounter persists if resolvable over time** — mid-fight save is
   the norm, not the exception.
2. **Every outcome authored** — no "nothing happened" unless authored
   (retreat without consequence is a finding).
3. **Composition through the selector** — no hand-built enemy lists duplicating
   the catalog.
4. **Terrain modifiers authored** and sourced from the map/location owners.
5. **Consequences routed** through the standing bridge and outcome vocabulary
   (W3-01 contract where narrative).
6. **Tone:** violence is costly; the aftermath (wounds, losses, salvage) is
   part of the encounter (W2-06 prose).

### III.1.3 Review sheet

```text
[ ] entry path declared; persistence wired
[ ] composition via selector
[ ] all outcomes authored + routed
[ ] terrain factors sourced
[ ] seed in session
[ ] narrative hooks registered
[ ] aftermath authored (losses, wounds, loot)
[ ] tone review
```

---

## §III.2 Condition/repair authoring playbook

### III.2.1 Condition table template

```yaml
item_class: rifles
decay: {per_round: 0.004, per_breach: 0.02, per_week_neglect: 0.01}
bands: {pristine: 0.8, worn: 0.5, damaged: 0.2}
effects:
  worn: {accuracy: -0.08, jam_chance: +0.02}
  damaged: {accuracy: -0.18, jam_chance: +0.06}
  broken: {usable: false, improvised: melee-strike}
repair: {recipe: rifle_repair, materials: [steel, tools], station: workbench}
```

### III.2.2 Authoring rules

1. **Decay per event, not per tick** — tick decay creates hidden wear.
2. **Effects as deltas on resolution inputs** (accuracy, jam), authored per
   band; no hidden multiplier.
3. **Broken behavior authored** per class (unusable, improvised, salvage).
4. **Repair routes through crafting** (W3-05 recipe refs); no repair action
   outside the craft owner.
5. **No condition on items that should never decay** (authored exclusion
   list).

### III.2.3 Review sheet

```text
[ ] decay events declared (not tick)
[ ] bands + effects authored
[ ] broken behavior authored
[ ] repair recipe refs exist (W3-05)
[ ] exclusion list reviewed
[ ] tests: worn-resolves-worse; repair-costs
```

---

## §III.3 Defense authoring playbook

### III.3.1 Layer template

```yaml
layer: perimeter
threat: ground_raid
inputs: {crew: 2, ammo: class, power: low}
state: operational|degraded|down (authored transitions)
response: {intercept_chance: 0.7 band, casualties_mod: -0.3}
alerts: raid_warning (crisis path)
degradation: [inputs missing -> chance drop per authored table]
```

### III.3.2 Authoring rules

1. **One role per layer** (role table, no overlaps).
2. **Inputs consumed** (crew time, ammo, power) — no free defense.
3. **Degradation table authored** (what happens with no crew/ammo/power).
4. **Alerts through the crisis path** once per event with attribution.
5. **Outcomes connect to combat resolution** (P1) — a successful interception
   reduces the raid, never negates it silently.

### III.3.3 Review sheet

```text
[ ] distinct role row
[ ] inputs + consumption
[ ] degradation table
[ ] alert single-path
[ ] combat/encounter linkage
[ ] tests: degrade reduces response; alert once
```

---

## §III.4 Espionage/heat authoring playbook

### III.4.1 Mission template

```yaml
mission: intel_holdfast_forge
type: recon
target: holdfast_forge (owner ref)
difficulty: authored 0.55
team: [agent ids], cost: {supplies, days}
outcomes:
  success: {intel: forge_capacity, standing: 0, heat: +4}
  partial: {intel: partial, heat: +8}
  failure: {heat: +14, supplies_lost}
  blown: {heat: +22, agent_risk, standing: -0.1}
consequences_router: esp:<id>
```

### III.4.2 Authoring rules

1. **Outcomes authored per type with bounded deltas.**
2. **Heat contributions per outcome** (feeds P6, bounded per event).
3. **Blown adds agent risk** (authored arc hooks, W3-01).
4. **Intel references target state** (read, not copy).
5. **No free success** (cost always paid).

### III.4.3 Heat authoring rules

```text
sources: mission outcomes, blown ops, captured agents, faction pressure
per-event caps: authored (e.g., +22 max)
thresholds: T1/T2/T3 with warned consequences + avoidance
decay: rates per class; hiding accelerates
cap: authored; deliberate-only
```

### III.4.4 Review sheet

```text
[ ] outcomes bounded + routed once
[ ] heat per outcome within caps
[ ] thresholds warned; avoidance authored
[ ] agent risk hooks
[ ] intel reads target owner
[ ] tests: consequence-once; warned thresholds; decay
```

---

## §III.5 Prisoner/bounty authoring playbook

### III.5.1 Prisoner template

```yaml
prisoner: faction_scout
capture: combat outcome ref
holding: {needs: food/water, guard_effort, space}
terms:
  ransom: {amount, faction ref, effects}
  exchange: {target, effects}
  labor: {work_class, cap, release_path, effects}
  release: {effects}
  tribunal: {authored}
coercion: {unreliable: 0.5 false/incomplete, never certain reward}
scenes: [interrogation (authored, dignity review), release, inbox]
```

### III.5.2 Authoring rules

1. **Every holding costs** (needs routed, guard effort).
2. **Every term authored with effects**; minimum exit = release.
3. **Coercion unreliable** by authored probability; false info must be
   identifiable later (the discovery is part of the story).
4. **Dignity review** on interrogation scenes (no gratuitous content).
5. **Labor capped** (diminishing/limits) so it is not infinite profit.

### III.5.3 Bounty template

```yaml
bounty: target_ref, faction, reward, duration_days
states: offered|accepted|completed|expired
completion: target_state (dead|captured) -> reward + standing delta
expiry: authored (no punitive default)
```

### III.5.4 Review sheet

```text
[ ] holding costs routed
[ ] exits complete (min release)
[ ] coercion unreliability authored + sampled in test
[ ] scenes dignity-reviewed
[ ] labor capped
[ ] bounty states + completion once
[ ] expiry authored
```

---

## §III.6 Standing authoring playbook

```yaml
event: defended_holdfast
delta: +0.15
attribution: combat:<session>
daily_cap: +0.25
band_warnings: authored (W3-02 consumption)
```

Rules: one bridge; per-event and per-day caps; attribution; band transitions
warned; consumed by stance bands (W3-02) and narrative gates (W3-01).

---

## §III.7 War stage authoring playbook

```yaml
stage: 03_escalation
entry: consequence:war_stage2_resolved
effects:
  territory: [lost, contested]
  routes: [gate refs closed]
  stance: [faction deltas]
  prices: [shock refs]
options:
  defense: [prep actions]
  diplomacy: [treaty paths]
  trade: [corridor paths]
  narrative: [W3-01 choices]
```

Rules: every stage has ≥1 effect and ≥1 option, all routed; warnings precede
stage entry (authored); effects observable in owners.

---

## §III.8 Verification catalog (all points)

### III.8.1 T1 static checks

| Check | Detects |
|---|---|
| S4.1 entry-path table | entries without persistence wiring |
| S4.2 outcome completeness | encounter outcomes missing routes |
| S4.3 condition tables | classes without bands/broken behavior |
| S4.4 layer roles | overlapping roles; missing inputs |
| S4.5 mission tables | outcomes without heat/routing refs |
| S4.6 threshold tables | thresholds without warnings/avoidance |
| S4.7 prisoner terms | terms without exits |
| S4.8 ranging consumers | observations without consumers |
| S4.9 standing caps | missing clamps |
| S4.10 stage completeness | stages without effects/options |

### III.8.2 T2 kits

```text
CombatEntry_Kit          Condition_Kit         Stealth_Kit
DefenseLayer_Kit         Espionage_Kit         CounterIntel_Kit
Prisoner_Kit             Ranging_Kit           Standing_Kit
WarStage_Kit             (Occupation_Kit C)
```

Each kit: scripted state, authored expectations, single-assert failures with
evidence.

### III.8.3 T3 soak

```text
20 seeds × 60 days: injection of raids, missions, war stages
assertions:
  no double outcomes (ledger scan)
  heat never exceeds cap; thresholds warned (log)
  standing clamps honored
  defense degradation measured
  every stage effects/options felt (spot checks)
  determinism (seed replay)
```

### III.8.4 Evidence

```text
docs/evidence/w3-04/
  T1-*.yaml T2-*.yaml T3-*.yaml
  findings/ (CS-### files)
  repairs/
  P0_SECURITY_PREMISE.md
```

---

## §III.9 Worked thread — the night raid

### III.9.1 Sequence

```text
day 0   ranging observation (armor, north, 0.6 confidence)
day 0   alert shown; player preps (posts, ammo)
day 1   gate: perimeter degraded (crew split) -> intercept chance down
day 1   raid combat (P1) resolves; losses taken; outcome routed
day 1   casualties: 2 wounded (health owner), 1 captured by raiders (NPC arc)
day 2   standing delta (defended); economy shock (food stores lost)
day 3   counter-intel: raiders' scout mission heat (if espionage present)
day 4   aftermath scene (W3-01); memorial if death
```

### III.9.2 Findings produced

| # | Finding | Class | Repair |
|---|---|---|---|
| CS-01 | ranging observation shown but prep actions had no effect on outcome | useless warning | connect prep to defense inputs |
| CS-02 | perimeter degraded silently (no alert about missing crew) | silent state | degradation surfaces |
| CS-03 | combat outcome applied wounded twice (combat + aftermath) | double apply | idempotency |
| CS-04 | standing delta unbounded on a big win (+0.4) | clamp | daily cap |
| CS-05 | captured survivor's arc not triggered (dead-end capture) | missing routing | W3-01 arc hook |
| CS-06 | food shock applied but not attributed | visibility | attribution string |
| CS-07 | memorial double entry (combat + grief both wrote) | dedupe | keyed once |

### III.9.3 What the thread teaches

A single night draws in seven systems; the audit's discipline is **one
consequence per system per event** across the whole chain. The thread is the
plan's canonical regression scenario.

---

## §III.10 Faction security thread — the quiet hand

### III.10.1 Sequence

```text
day 0   mission recon (success, +intel, +4 heat)
day 3   mission sabotage (partial, +8 heat, target degraded)
day 5   T1 warning (watching) — visible
day 8   blown mission (+22 heat, agent at risk)
day 9   T2 warning + shakedown scene (W3-01)
day 11  raid attempt (P1) routed; avoided by laying low (decay −8/day)
```

### III.10.2 Findings

| # | Finding | Repair |
|---|---|---|
| CS-08 | blown mission heat crossed T1→T3 in one event | per-event heat cap |
| CS-09 | agent risk had no arc hook | W3-01 hook authored |
| CS-10 | raid triggered despite T2 avoidance path taken | avoidance state not read |
| CS-11 | interrogation scene (if captured) unreviewed | dignity review |

---

## §III.11 Cost model

| Phase | Effort |
|---|---|
| P0 premise + tables | 4-5 days |
| P1-P2 combat/condition | 8-10 days |
| P3-P4 stealth/defense | 6-8 days |
| P5-P6 espionage/heat | 8-10 days |
| P7-P9 prisoners/ranging/standing | 8-10 days |
| P10 stages (+C occupation) | 4-6 days |
| Verification T1/T2/T3 | 8-10 days |
| Closeout | 2 days |

≈ 6-8 weeks integrated (matches Part I §15 phases).

---

*End of Part III. Continues in Part IV (Q&A, C-path, appendices).*# W3-04 · PART IV — Q&A, C-PATH DESIGNS, MATRICES, AND APPENDICES

---

## §IV.1 Governance Q&A (Q1–Q10)

**Q1. Does this plan add a combat engine?**
No. One resolution authority (`TacticalCombatSystem`); the plan adds contracts,
entry-path truth, persistence verification, and consequence routing.

**Q2. Who owns outcomes?**
Each outcome type its existing owner: wounds→health, standing→bridge,
captures→prisoner/arc, loot→inventory, narrative→W3-01 ledger.

**Q3. Does Path B add save sections?**
No for combat (the `.Persistence` partial exists). Occupation (C) rides the
war/faction owner or a signed extension.

**Q4. How are prisoners kept ethical?**
Dignity rules + unreliable coercion + exits + holding costs + scene review.
No torture-reward mechanics.

**Q5. What stops unbounded heat?**
Per-event caps, decay, hard cap, warned thresholds, avoidance paths. The
flagship test crosses T1→T2 over multiple events, never one.

**Q6. How does combat feed the economy?**
W3-02's consequence refs (shocks from war/raids) and standing→stance. This
plan routes; W3-02 composes.

**Q7. What about the player's own wounds?**
Health owner handles; combat applies through it. Death/critical rules follow
the existing crisis-warning standards (W2-03/W3-03).

**Q8. Can defense fully negate a raid?**
No silent negation: interception reduces the raid per authored tables; the
encounter still resolves (reduced). Total prevention is an authored outcome
dedicated (rare, justified).

**Q9. What counts as "the alert path"?**
The existing crisis/emergency producers (fire/flood/rad precedent). Defense
alerts use it; no second notification system.

**Q10. What is the repair cap?**
20 per phase, ranked; rest are debt records. Same across Wave 3.

---

## §IV.2 Method Q&A (Q11–Q20)

**Q11. Why audit entry paths first?**
Persistence and outcome truth are per-entry; a missing path is invisible until
mid-fight save. First audit, highest yield.

**Q12. Why band-based condition?**
Bands keep effects authorable and auditable; continuous multipliers hide
double-application.

**Q13. Why is "useless warning" a finding class?**
A warning with no actionable preparation is theater; it trains players to
ignore warnings (the worst failure of a warning system).

**Q14. Why cap standing per day?**
Cliffs: one heroic day flipping a faction to allied breaks the economy's
stance bands (W3-02) and the narrative's pacing.

**Q15. Why is coercion unreliable?**
Because certainty would make cruelty optimal — the design rejects that
incentive structure on principle and mechanic.

**Q16. Why must occupation (C) route through existing owners?**
Rule 5: no parallel systems. Occupation is state + effects, not a new
simulation.

**Q17. How is determinism proven?**
Session seeds restored on load; replay equality tests per encounter; standing
and outcomes keyed.

**Q18. What about stealth in combat (P3 within P1)?**
Exposure feeds targeting at resolution; the playbook treats them as one
scenario with two contracts.

**Q19. How do ranging observations aggregate?**
Bounded per tick with authored cooldown; confidence presented honestly;
consumers verified (surface + prep).

**Q20. Smallest Path A?**
P0 + entry/persistence table + outcome completeness + defense role table +
threshold warning check. No repairs.

---

## §IV.3 C-path designs

### IV.3.1 C1 — Combat doctrines

Authored doctrine choices (fortify/patrol/intercept) affecting defense tables
and encounter compositions. Stored on the defense owner; signed persistence.
Adds strategic choice without new systems.

### IV.3.2 C2 — Persistent injuries

Wounds with lasting effects (scars, reduced capacity) through the health
owner's existing records; combat applies, health persists; arcs read (W3-01).
Requires health-owner API coordination; no parallel injury store.

### IV.3.3 C3 — Agent networks

Espionage cells with relationships (agents as people) routed through the
existing relationship owner; missions assign agents; blown ops affect arcs.
No second roster authority.

### IV.3.4 C4 — Occupation and insurgency

Territory control state + resistance level + authored insurgency options
(raids, sabotage, diplomacy) through existing systems. This is the plan's
deepest C item; signed persistence and W3-01 narrative integration required.

### IV.3.5 C5 — War economics integration

Stages author economic consequences as first-class effects (shocks, routes,
stance) — the handshake with W3-02's lexicon. Mostly coordination, little new
state.

### IV.3.6 C6 — Faction war goals

Authored war aims per faction (territory, concessions) that frame stages and
endings; read by endings (W3-01) and decrees. Content-heavy, state-light.

### IV.3.7 C-bundle recommendation

```text
first: C5 (integration, cheap), C2 (injuries, high fiction value)
then:  C1 (doctrine), C3 (agents)
last:  C4 (occupation, biggest), C6 (war goals, content)
```

### IV.3.8 C signature block

```text
[ ] C1 doctrines   [ ] C2 persistent injuries  [ ] C3 agent networks
[ ] C4 occupation  [ ] C5 war economics        [ ] C6 war goals
```

---

## §IV.4 Interaction matrix

| This plan → | Interface | Direction |
|---|---|---|
| W3-01 | narrative hooks, capture arcs, aftermath scenes | out/in |
| W3-02 | standing→stance; war shocks | out |
| W3-03 | wounds/trauma/grief from combat | out |
| W3-05 | repair recipes; condition materials | in |
| W3-06 | alerts, ranging, defense surfaces | out |
| W2-03 | threat/pressure measurement | out |
| W2-04 | weather gates on defense/routes | in |
| W2-02 | session lifecycle for combat state | coordinate |

### IV.4.1 Never-cross list

```text
[ ] never a second combat calculator
[ ] never an unlimited torture/reward mechanic
[ ] never unwarned punitive outcomes
[ ] never silent state (layers, heat, standing)
[ ] never a second alert/notification system
[ ] never save sections without signature (P/C excepted)
```

---

## §IV.5 Expanded glossary

| Term | Meaning |
|---|---|
| alert path | canonical crisis/emergency notification route |
| blown (mission) | worst espionage outcome with agent risk |
| band (condition) | qualitative equipment state |
| coercion reliability | authored probability of false/incomplete intel |
| daily cap | bound on standing/heat movement per day |
| degradation table | defense response reduction from missing inputs |
| doctrine | authored defense/combat posture choice (C) |
| entry | the path that starts combat |
| exposure | detection factor composite |
| idempotency key | once-only outcome application key |
| interception | defense response reducing raid intensity |
| occupation | territory control state (C) |
| ranging | sound-based threat observation |
| session (combat) | the persisted resolution unit |
| standing | faction disposition delta |
| term (prisoner) | authored resolution path for captives |
| threshold (heat) | T1/T2/T3 attention levels |
| useless warning | warning without actionable preparation (finding) |

---

## §IV.6 Artifact index

| Artifact | Type |
|---|---|
| `docs/security/COMBAT_ENTRY_MAP.md` | owned audit doc |
| `docs/security/CONDITION_TABLES.md` | authored |
| `docs/security/DEFENSE_ROLES.md` | authored |
| `docs/security/ESPIONAGE_MISSIONS.md` | authored |
| `docs/security/HEAT_THRESHOLDS.md` | authored |
| `docs/security/PRISONER_TERMS.md` | authored |
| `docs/security/RANGING.md` | authored |
| `docs/security/STANDING_CAPS.md` | authored |
| `docs/security/WAR_STAGES.md` | authored |
| `docs/evidence/w3-04/*` | evidence |

---

## §IV.7 Expanded signature sheet

```text
ASHFALL WAVE 3 · PLAN 4 (COMBAT/SECURITY) · EXECUTION SIGNATURES
HEAD: ________  Date: ________  Foreman: ________

[ ] P0 premise + entry map + role table
[ ] P1 combat entry/persistence truth
[ ] P2 condition + repair loop
[ ] P3 stealth attribution
[ ] P4 defense layers + alerts
[ ] P5 espionage consequence-once
[ ] P6 counter-intel heat bounds
[ ] P7 prisoners/bounties completion + ethics
[ ] P8 ranging warning loop
[ ] P9 standing bridge bounds
[ ] P10 stage completeness (occupation: [ ] no [ ] signed)
[ ] C2 persistent injuries (health API): [ ] no [ ] signed
[ ] repair cap: ___

Retained: no torture reward; no unwarned fatals; no second engine.
```

---

## §IV.8 Final control (W3-04)

```text
Document map:
  Part I   summary contract (original)
  Part II  deep designs 1-10
  Part III playbooks + verification + worked threads
  Part IV  Q&A + C-path + matrices + appendices (this part)

Explicit stop: proposal only; no execution without Annex U + above
signatures.
```

*Document control: W3-04 · Wave 3 (expanded) · HEAD 5be1a30a · end of W3-04.*# W3-04 · PART V — DEFENSE DEEP PLAYBOOK: LAYERS, DOCTRINES, AND THREAT INTEGRATION

> Every defense layer specified in authoring depth: state machines, input
> consumption, degradation tables, alert emissions, doctrine interactions, and
> threat-specific scenario walkthroughs. Proposal only.

---

## §V.1 The defense stack model

### V.1.1 Layer composition

```text
ThreatEvent (raid | intrusion | air | fallout | storm)
  -> LayerEvaluation (ordered by threat class)
       airlock -> perimeter -> sky battery -> sky armor
  -> ResponseSet (each layer contributes authored reduction)
  -> ResidualThreat (what gets through)
  -> Combat/Encounter (residual resolves through P1)
  -> Alert (single crisis path, attributed)
```

The critical rule: **layers reduce, they do not erase.** Residual threat
always resolves through the authoritative combat path; total prevention is an
authored special case, not the default.

### V.1.2 Why layers, not one number

A single "shelter defense" number cannot express: airlock discipline vs.
perimeter firepower vs. sky interception. Layers let content target specific
weaknesses (the raiders cut the fence; the storm taxes the roof), and let the
player prepare selectively (crew, ammo, power). The audit's job is to keep
each layer's role distinct and its inputs real.

---

## §V.2 Airlock security — full specification

### V.2.1 State machine

```text
states: sealed | cycling | overridden | breached
transitions:
  sealed -> cycling: authorized entry request (resident/trade/diplomat)
  cycling -> sealed: cycle completes (authored duration)
  cycling -> overridden: emergency override (player/faction)
  any -> breached: hostile intrusion success (combat outcome)
  breached -> sealed: repair action (materials + time, W3-05)
```

### V.2.2 Inputs

| Input | Source | Effect |
|---|---|---|
| guard crew | roster/schedule | override speed; breach resistance |
| power | grid owner | cycling speed; alarm function |
| maintenance | W3-05 materials | repair from breached |
| identification | faction/relationship | who may enter |

### V.2.3 Degradation table

| Condition | Cycling time | Breach resistance | Alarm |
|---|---|---|---|
| all inputs | authored fast | full | works |
| no power | manual (slow) | reduced | silent (finding if unwarned) |
| no crew | slow | low | works |
| neither | blocked | minimal | silent |

The silent-alarm row is the interesting one: **an unpowered airlock that
cannot alarm is a warning system failure.** The audit requires either authored
warning content (the player notices the dark panel) or a loud failure. Silent
security failure is a finding class (`silent-state`).

### V.2.4 Alert emissions

```text
entry denied -> no alert (normal)
override used -> journal note (authored)
breach attempt -> intrusion crisis (single path, attribution: airlock)
breach success -> combat/encounter trigger + crisis
```

### V.2.5 Tests

```text
Airlock_EntryAuthorized_NoAlert
Airlock_CycleTime_PerInputs
Airlock_BreachResistance_PerCrew
Airlock_AlarmSilent_Warned
Airlock_Breached_RepairPath
Airlock_BreachRoutesToCombat
```

---

## §V.3 Perimeter defense — full specification

### V.3.1 State machine

```text
states: intact | exposed | overrun
segment model (optional): authored segments (north fence, gate, yard)
  each segment: condition (maintained/damaged/down), coverage (covered/empty)
transitions:
  intact -> exposed: segment down / coverage gap (crew reassigned)
  exposed -> overrun: hostile success at the gap
  any -> intact: repair/recall crews
```

### V.3.2 Inputs

| Input | Source | Effect |
|---|---|---|
| crew posts | schedule | coverage per segment |
| ammo class | inventory | response firepower |
| fortifications | W3-05 materials | segment condition |
| warning lead | P8 ranging | prep time (posts before contact) |

### V.3.3 Response resolution (authored table)

```text
intercept_chance = base(coverage) × firepower(ammo) × prep(lead_time)
casualty_modifier = authored(intercept band)
loot_loss_modifier = authored(overrun band)
```

All four quantities come from owners; the table is data; the audit checks each
input actually moves the outcome (the "decorative input" class).

### V.3.4 Degradation narrative

Each degradation state gets authored player-facing signals:

| State | Signal |
|---|---|
| segment damaged | visible damage note (surface); repair prompt |
| coverage gap | crew assignment warning (schedule surface) |
| crew exhausted | fatigue bands; post effectiveness drop |
| ammo low | inventory band; quartermaster note |

### V.3.5 Tests

```text
Perimeter_SegmentStates
Perimeter_CoverageGapWarned
Perimeter_InterceptPerInput_<input>
Perimeter_CasualtyModifierBands
Perimeter_LootLossBands
Perimeter_RepairPath_W3_05
Perimeter_PrepTime_FromRanging
```

---

## §V.4 Sky battery — full specification

### V.4.1 State machine

```text
states: ready | reloading | damaged | offline
transitions:
  ready -> reloading: fired (ammo consumed)
  reloading -> ready: reload time (crew)
  any -> damaged: counter-battery/storm (authored events)
  damaged -> offline: repair failed / abandoned
offline -> ready: repaired (W3-05 + crew)
```

### V.4.2 Ordnance integration

The verified `OrdnanceCatalog` + `SkyDefenseBatterySystem` already exist. The
audit confirms:

1. ordnance items consumed from inventory per shot (no free fire);
2. intercept resolution uses the authored ordnance profiles (range, altitude
   band, reliability);
3. reload times per ordnance class;
4. counter-battery events authored (the enemy shoots back).

### V.4.3 Tests

```text
SkyBattery_OrdnanceConsumed
SkyBattery_InterceptPerProfile
SkyBattery_ReloadPerClass
SkyBattery_DamageStates
SkyBattery_CounterBatteryAuthored
SkyBattery_OfflineWarned
```

---

## §V.5 Sky armor — full specification

### V.5.1 Role separation

Sky armor (`SkyLayerArmorSystem`) is **environmental mitigation** (ash, fallout
particles, temperature), not interception. Its inputs are materials and
maintenance; its states are coverage bands. The audit verifies the role does
not overlap the battery's (interception) or airlock's (entry).

### V.5.2 Tests

```text
SkyArmor_CoverageBands
SkyArmor_MaintenanceConsumed
SkyArmor_EnvironmentMitigation_Measured (W2-04 interface)
SkyArmor_NoInterceptionOverlap
```

---

## §V.6 Defense doctrines (C-path design, full)

### V.6.1 Doctrine model

```text
Doctrine: authored posture emphasizing a layer
  fortify    -> perimeter condition decay reduced; mobility down
  patrol     -> warning lead improved; crew fatigue up
  intercept  -> sky readiness up; ammo burn up; ground coverage down
  conceal    -> breach chance down; trade/diplomacy options down
```

Each doctrine is a set of **authorable modifiers** on the inputs above, stored
on the defense owner (signed persistence). Doctrines trade off manifestly; the
audit checks every doctrine has at least one cost.

### V.6.2 Doctrine change ceremony

Changing doctrine has an authored cost (time, re-posting crews) and can be
done only at authored intervals (no per-tick flipping).

### V.6.3 Tests

```text
Doctrine_MinusCostRejected
Doctrine_ModifiersPerLayer
Doctrine_ChangeCost
Doctrine_Persistence (signed)
Doctrine_NoFreeDominance
```

---

## §V.7 Threat integration walkthroughs

### V.7.1 Ground raid, full sequence

```text
T-12h  ranging observation (P8) -> alert -> prep window
T-8h   player: posts crews (perimeter coverage up), doctrine patrol
T-4h   weather gate: storm delays raiders (W2-04 route gate) — authored
T-0    raid event:
         airlock: denied (sealed; no power -> manual cycle, slow)
         perimeter: intercept per coverage/ammo/prep
         residual: combat (P1) — losses per bands
T+1h   aftermath: wounds, loot loss, standing, journal, grief if deaths
T+2d   repair tasks scheduled (W3-05 materials); crews fatigued
```

### V.7.2 Intrusion (quiet), full sequence

```text
T-0    intrusion attempt (stealth vs. airlock/patrol)
         detection factors: guard alertness (fatigue), lighting (power),
         airlock sensing (power)
T+1h   if detected: conflict resolves (combat or expulsion)
       if not: theft/sabotage outcome (inventory/production effects)
T+1d   investigation content authored (who was it? clues) -> W3-01
```

The quiet intrusion is the **counter-intelligence bridge**: heat from P6 can
invite it; defense detects it; narrative investigates it.

### V.7.3 Air attack, full sequence

```text
T-0    air threat (war stage event)
       sky battery: intercept per ordnance/readiness
       sky armor: mitigates residual environmental hazard
       residual: structure damage (authored), casualties, fire (W2-04 owner)
T+1d   counter-battery risk (authored probability) -> battery damage
```

### V.7.4 Combined siege (long form)

```text
Weeks of pressure: raids + intrusions + air + supply closure
layer degradation accumulates; crews fatigue; ammo dwindles
authored crisis producers (fire/flood/rad) interact (W2-04)
the audit's soaked scenario: no single layer can be ignored; preparation
compounds; the shelter's choices author the outcome
```

---

## §V.8 The defense audit checklist

```text
[ ] role table: no overlapping layer responsibilities
[ ] each layer: state machine, inputs, degradation table
[ ] each input: sourced from an owner; verified to move outcomes
[ ] each degradation: player-visible signal (no silent failure)
[ ] alerts: single crisis path; attributed; once per event
[ ] residual: always resolves through combat; no silent negation
[ ] repairs: routed through W3-05 (materials) and schedule (crew)
[ ] doctrines (C): costs, intervals, persistence signed
[ ] tests: per-layer kit green; soak siege scenario holds
```

---

## §V.9 Common defense findings (seeded catalogue)

| # | Finding | Class | Repair |
|---|---|---|---|
| D-01 | layer input decorative (ammo doesn't change outcome) | substance | wire input to table |
| D-02 | silent degradation (no signal) | visibility | authored signal |
| D-03 | alert bypasses crisis path | routing | single path |
| D-04 | interception negates raid silently | residual rule | reduce, never erase |
| D-05 | repair without materials | routing | consume from W3-05 |
| D-06 | crew fatigue ignored | fidelity | effectiveness bands |
| D-07 | overlapping roles (battery stops ground) | role | split table |
| D-08 | total prevention possible by default | design | authored rarity |
| D-09 | unpowered still fully operational | input | degradation row |
| D-10 | counter-battery never authored | completeness | event + rate |

---

*End of Part V. Continues in Part VI (intelligence deep playbook).*# W3-04 · PART VI — INTELLIGENCE DEEP PLAYBOOK: MISSIONS, AGENTS, HEAT, AND COUNTER-OPERATIONS

> The spy game in authoring depth: mission catalog, agent handling, outcome
> tables, heat economy, counter-intelligence operations, and worked threads.
> Dignity and consent rules apply (agents are people, not tools).

---

## §VI.1 Mission catalogue (authoring templates)

### VI.1.1 Mission: recon

```yaml
mission: recon
purpose: reveal target state (capacity, stock, garrison, routes)
difficulty inputs: [target security band, agent skill, cover quality]
duration: 1-3 days
outcomes:
  success:  {intel: full target read, heat: +3, cost: supplies}
  partial:  {intel: partial (authored subset), heat: +5}
  failure:  {intel: none, heat: +9, agent stress}
  blown:    {heat: +16, agent compromised}
```

Authoring rule: intel is a **read** of the target's owner (capacity, stock,
garrison), never a copied snapshot; stale intel is honest (dated).

### VI.1.2 Mission: sabotage

```yaml
mission: sabotage
purpose: degrade target capability (forge output, route, battery)
outcome effects: target state delta (authored band) through target owner
blowback: authored (target investigates -> heat floor + counter-op)
```

### VI.1.3 Mission: theft

```yaml
mission: theft
purpose: move goods (rare items, ordnance, documents)
cost: risk of open conflict if caught (authored escalation)
effective limits: capacity (what can be carried), fencing (buyer)
```

### VI.1.4 Mission: recruit

```yaml
mission: recruit
purpose: turn a person (informant, insider)
terms: authored deal (money/favors/safety) — through contract machinery
outcome: relationship/turncoat state owned by the relationship/espionage
  boundary (one owner; the plan documents which)
consent: the recruit is a person; coercion is unstable (unreliability rule)
```

### VI.1.5 Mission: disinformation

```yaml
mission: disinformation
purpose: influence target stance/decision (move patrols, sour a deal)
effects: stance/behavior deltas (bounded), blown -> reputation hit
```

---

## §VI.2 Agent handling model

### VI.2.1 Agent record

```text
Agent:
  id (survivor or contact), skills (authored bands), stress (authored),
  cover (authored state), loyalty (authored bands), history[] (missions)
```

### VI.2.2 Consent and care

Agents are people: high stress has authored consequences (P7 arcs, W3-03
trauma routing); missions consume rest; a loyal agent is an authored
relationship, not a stat. The audit: agents' stress routes to psychology
owners; no infinite missions without cost.

### VI.2.3 Agent loss

```text
captured: ransom/exchange (P7) or rescue mission (authored)
killed: grief routing (W3-03), memorial, narrative arc
disappeared: authored uncertainty (no silent removal)
```

No agent vanishes silently; every loss has a consequence path.

### VI.2.4 Tests

```text
Agent_StressRoutes_Psychology
Agent_MissionConsumesRest
Agent_LossConsequencePaths
Agent_NoInfiniteMissions
Agent_CaptureRoutes_Prisoner
```

---

## §VI.3 Heat economy (full tables)

### VI.3.1 Accrual sources

| Source | Heat/event | Cap | Notes |
|---|---|---|---|
| recon success | +3 | — | low profile |
| recon failure | +9 | — | noticed |
| sabotage success | +8 | — | damage noticed eventually |
| sabotage failure | +14 | — | immediate |
| theft success | +6 | — | inventory noticed |
| theft failure | +12 | — | alarm |
| blown mission | +16–22 | per-event cap | worst case |
| captured agent | +18 | — | investigation |
| faction pressure | +5–15 | authored | political |
| snitch event | +10 | — | authored turncoat |

**Hard rule:** any single event ≤ +22 (per-event cap); thresholds are crossed
over multiple events, never one.

### VI.3.2 Thresholds and consequences

| Threshold | State | Consequence | Warning | Avoidance |
|---|---|---|---|---|
| T1 (20) | watch | patrol questions; prices for favors up | rumor/chatter | normal behavior |
| T2 (45) | watchlist | shakedown scene; informants active | direct warning scene | pay/refuse/stall |
| T3 (80) | warrant | raid attempt (combat path) | warrant notice + route out | lay low (decay ×2), leave, deal |

### VI.3.3 Decay

| Heat class | Idle | Hiding | Notes |
|---|---|---|---|
| transaction | −4/day | −8/day | hiding has its own cost (missed trade) |
| warrant | −2/day | −4/day | slower; de-escalates through scenes |
| political | −3/day | −3/day | tied to faction events |

### VI.3.4 Tests

```text
Heat_PerEventCap_22
Heat_Thresholds_Pacing
Heat_Warnings_<T2,T3>
Heat_Avoidance_Paths
Heat_Decay_Rates
Heat_Warrant_RoutesToCombat
Heat_SingleWriter
```

---

## §VI.4 Counter-intelligence operations (when the enemy spies on you)

### VI.4.1 Threat model

Counter-intelligence (`CounterIntelligenceSystem`) is the mirror: hostile
agents probe the shelter. Sources: war stages, faction stance, heat blowback,
authored infiltrators.

### VI.4.2 Detection model

```text
signals: suspicious behavior (authored), missing items, opened containers,
  intercepted transmissions (radio owner), agent confession (turncoat)
counter-measures: vetting (crew screening), watch rotations, decoys,
  counter-ops (double agent play)
```

### VI.4.3 The double-agent play (C-path depth)

Authored: identify, turn, feed false intel; effects on enemy decisions
(authored deltas to their next events). State on the espionage owner; no
parallel system.

### VI.4.4 Tests

```text
CounterIntel_DetectionSignals
CounterIntel_VettingCosts
CounterIntel_DoubleAgent_Delta
CounterIntel_NoUnwarnedPurge
CounterIntel_ConfessionRouting
```

---

## §VI.5 The intelligence audit checklist

```text
[ ] every mission: outcomes bounded, routed once, heat attributed
[ ] intel reads owners, dated and stale-honest
[ ] agents: stress/rest/loss routed; consent framing
[ ] heat: caps, thresholds, warnings, avoidance, decay
[ ] counter-intel: detection signals; measures cost; no unwarned purge
[ ] raids route to combat (P1); captures route to prisoners (P7)
[ ] narrative: blown agents -> arcs (W3-01)
[ ] determinism: seeded outcomes; replay equal
```

---

## §VI.6 Worked thread — the informant's ledger

### VI.6.1 Sequence

```text
day 0   recon mission (success, +3 heat) reveals forge stock low
day 2   recruit attempt on a forge worker: terms (safety for family)
        outcomes: success -> informant; failure -> suspicion +6 heat
day 5   informant intel (dated): patrol schedule -> window for sabotage
day 6   sabotage (partial): forge output -band for N days; +8 heat
day 9   forge notices -> investigation (+counter-op); informant at risk
day 12  counter-intel detects the informant? authored check:
          safe: informant continues
          exposed: informant captured -> rescue or extraction mission (authored)
day 14  T2 warning (watchlist): shakedown scene
day 16  player lays low: heat decays; operations pause
```

### VI.6.2 Findings produced

| # | Finding | Class | Repair |
|---|---|---|---|
| IS-01 | informant's family promise not tracked as an obligation | contract gap | scoped obligation record |
| IS-02 | sabotage output penalty unbounded (forge never recovered) | recovery | authored restoration curve |
| IS-03 | investigation event had no player visibility | warning | journal signal |
| IS-04 | captured informant had no extraction path | completeness | rescue mission authored |
| IS-05 | heat from investigation pushed T1→T3 in two events | pacing | per-event caps enforced |
| IS-06 | intel shown as current though three days old | honesty | dated/stale presentation |
| IS-07 | shakedown scene resolved by a silent skill check | agency | explicit choice surfaces |

### VI.6.3 Design lesson

Intelligence is a **relationship economy**: agents have families, promises
become obligations, losses become arcs. The audit's cross-plan hooks (W3-01
narrative, W3-03 stress, P7 captures) are what keep it human.

---

## §VI.7 Worked thread — the mirror game

### VI.7.1 Sequence

```text
Hostile infiltration suspected after a theft (inventory loss).
Evidence chain: opened container -> watch rotation review -> transmission
intercept (radio owner) -> identify a mole (authored candidate set).
Player options: confront (scene), turn (double agent), expel, monitor.
Consequences per option: cohesion effects, enemy reaction, heat changes.
```

### VI.7.2 Findings

| # | Finding | Repair |
|---|---|---|
| IS-08 | mole candidate set had no authored evidence linkage | evidence chain authored |
| IS-09 | expulsion consequence leaked cohesion into a divergent store | route to group owner (W3-03) |
| IS-10 | double-agent feeding false intel had no effect on enemy events | authored decision deltas |
| IS-11 | monitoring produced no visible progress (uncertainty monotony) | authored beats over time |

---

## §VI.8 Intelligence doctrine tables (C-path)

| Doctrine | Missions focus | Heat profile | Agent strain | Political cost |
|---|---|---|---|---|
| quiet hand | recon/recruit | low per op, slow accrual | low | none |
| hammer | sabotage/theft | high per op | high | stance risk |
| ghost | disinfo/recruit | medium, political | medium | faction suspicion |
| mirror | counter-intel focus | low external, internal | medium | none |

Each doctrine is authored modifier sets with manifest costs; no free
dominance; signed persistence.

---

## §VI.9 Quick reference: the intelligence rules

```text
1. intelligence reads reality; it never invents it
2. agents are people: stress, rest, families, losses
3. heat is paced: no single event crosses a threshold
4. every threshold warns and every warning has a path
5. blown operations have arcs, not deletions
6. counter-intelligence mirrors the same rules
7. outcomes seeded; consequences once; owners authoritative
```

---

*End of Part VI. Continues in Part VII (prisoners, bounties, ranging, standing).*# W3-04 · PART VII — PRISONERS, BOUNTIES, RANGING, AND STANDING: DEEP PLAYBOOK

> The human edges of security: captives, contracts on heads, the sound of
> something coming, and what the factions think of you afterwards. Dignity
> rules bind every page.

---

## §VII.1 Prisoner system — full specification

### VII.1.1 Capture paths

```text
capture sources:
  combat outcome (P1): wounded/overwhelmed enemies taken alive (authored rate)
  raid defense (captured raiders)
  infiltration (captured intruders/informants)
  authored events (deserters, refugees, stranded)
capture record:
  person ref (id or authored identity class), condition (wounded/healthy),
  origin (faction/unknown), day, location
```

### VII.1.2 Holding model (full)

| Factor | Effect | Routing |
|---|---|---|
| food/water | daily consumption per person | inventory owner |
| space | shelter capacity band | shelter owner |
| guard effort | schedule effort per day | roster owner |
| conditions | morale/health drift per authored bands | needs/health owners |
| scenes | authored interactions at thresholds | W3-01 hooks |

**No silent optimal strategy:** holding forever costs accruing resources and
produces authored pressure scenes (the guard who talks too much; the captive
who stops eating). The player should feel the weight, not just the math.

### VII.1.3 The term matrix (full)

| Term | Requirements | Outcome effects | Risks |
|---|---|---|---|
| ransom | contact faction; amount authored | goods/standing; release | faction may refuse; trap ambush authored |
| exchange | a captive of theirs (or asset) | swap; standing neutral | valuation mismatch authored |
| labor | time; conditions authored | work contribution (capped/diminishing) | morale of both sides; escape risk |
| release | — | standing +/-, information | they may return in a raid (authored) |
| tribunal | evidence; authority (authored) | standing; faction reaction | faction perceives judgment |
| recruit | terms; consent (person) | turncoat/ally | unreliable; betrayal risk |

Every row: effects bounded, routed, and authored. Release is always available
(the minimum exit). No term disabled by authoring omission.

### VII.1.4 Coercion rules (binding)

```text
coercion exists as authored interaction (interrogation scenes)
outcomes: information is UNRELIABLE by authored probability
          (false leads, partial truths, delays)
never: certain reward, repeatable farming, "torture buffs"
scenes: dignity-reviewed; no gratuitous content; the person is present
recovery: captives' state matters (a broken captive yields less, and the
          shelter's people notice — authored morale effects)
```

### VII.1.5 Tests

```text
Prisoner_HoldingCosts_All
Prisoner_TermsAllAvailable
Prisoner_ReleaseMinimumExit
Prisoner_CoercionUnreliable_Sampled
Prisoner_CoercionNeverCertain
Prisoner_EscapeRiskAuthored
Prisoner_SocialPressureScenes
Prisoner_ConditionAffectsOutcomes
```

---

## §VII.2 Bounty system — full specification

### VII.2.1 Bounty model

```text
Bounty:
  target (person ref/faction), issuer, reward (goods/standing/favor),
  duration, conditions (dead | captured | driven off)
states: offered | accepted | completed | expired | revoked
completion:
  condition met -> reward + standing via bridge (P9) + narrative beat
expiry:
  authored (no punitive default; issuer may re-offer)
revocation:
  authored (politics/scene consequences)
```

### VII.2.2 Targets as people

Bounties name people. Rules:

1. a bounty target is an authored character with a place (not a spawning
   mob);
2. completion routes consequences: their faction reacts, their ties react
   (W3-03 relationships, W3-01 arcs);
3. the "driven off" condition exists where killing/capture is not authored
   (a third path for mercy);
4. no bounty farms: targets do not respawn; expiry/rotation is authored.

### VII.2.3 Tests

```text
Bounty_States_Transitions
Bounty_CompletionOnce
Bounty_RewardRouted_Standing
Bounty_TargetConsequences_<faction,tie>
Bounty_DrivenOffAvailable
Bounty_NoRespawnFarming
Bounty_ExpiryAuthored
```

---

## §VII.3 Sound ranging — full specification

### VII.3.1 Observation model

```text
Observation:
  type (armor | foot | arty | engines | unknown),
  direction (authored compass/landmark), confidence [0..1],
  source (war clash | route event | authored), day
```

Provenance rule: observations originate from real events (war clashes,
movements), never free-floating. The verified debt row (engine fed from
`FactionWarSystem.OnTerritorialClashOccurred`) is the pattern.

### VII.3.2 Confidence presentation

```text
confidence bands: certain (>0.8) | probable (0.5-0.8) | faint (0.2-0.5)
presentation: type + direction + band; never precise numbers to the player
uncertainty: faint observations may be wrong (authored); certain are true
```

### VII.3.3 Response loop

```text
observation -> alert (crisis path) -> preparation choices:
  posts (defense coverage up)
  curfew (labour down, safety up)
  convoy escort (routes safer, slower)
  evacuation (content loss, lives protected)
preparation -> measurable modifier in the defense tables (P4/V.3.3)
```

### VII.3.4 Spam control

```text
aggregation: same-type observations within authored window merge
cooldown per source
cap per day (authored)
priority: higher confidence/larger threat displaces faint duplicates
```

### VII.3.5 Tests

```text
Ranging_ObservationHasProvenance
Ranging_ConfidenceBands
Ranging_ResponseModifiers_Measured
Ranging_AggregationWindow
Ranging_DailyCap
Ranging_FaintMayBeWrong_Authored
```

---

## §VII.4 Standing system — full tables

### VII.4.1 Event deltas (authored table)

| Event | Δ standing | Notes |
|---|---|---|
| defended shelter (raid repelled) | +0.10..+0.15 | faction depends on who attacked |
| civilian casualties caused | −0.10..−0.20 | witnessed/authored |
| bounty completed | +0.15 issuer / −0.20 target faction | mirrored |
| prisoner well-treated (authored observation) | +0.05 | word travels |
| prisoner mistreated (authored) | −0.10 | the shelter hears first |
| trade honored | +0.05 | contract machinery |
| betrayal revealed | −0.25 | narrative event |
| intelligence success | 0 | factions unaware by default |
| intelligence blown | −0.10..−0.15 | target learns |

### VII.4.2 Clamps

```text
per event: authored (max ±0.25)
per day: authored (max ±0.40)
band transitions: warned (W3-02 consumption)
sources: all through the bridge (P9) — one writer
```

### VII.4.3 Consumers

| Consumer | Reads | Effect |
|---|---|---|
| economy stance bands (W3-02) | band | prices/access |
| narrative gates (W3-01) | value/band | content eligibility |
| bounty availability | band | offers/revocations |
| defense intelligence sharing | band | authored aid events |
| endings (W3-01 P7) | flags | terminal states |

### VII.4.4 Tests

```text
Standing_TableComplete
Standing_ClampPerEvent
Standing_ClampPerDay
Standing_WarnedTransitions
Standing_BridgeSingleWriter
Standing_ConsumedByEconomy
Standing_ConsumedByNarrative
```

---

## §VII.5 Worked thread — the prisoner at the gate

### VII.5.1 Sequence

```text
day 0   raid repelled; one raider captured (wounded)
day 1   holding begins: food/water consumed; guard effort assigned;
        shelter morale note (people are uneasy about a prisoner)
day 2   interrogation scene: coercion offered
          reliable path: patient questioning with the medic's help
          unreliable path: coercion -> false lead authored (wastes a patrol)
day 3   faction contact possible: ransom offer (authored)
day 4   player choices:
          ransom -> released; standing trade; ambush risk authored
          labor -> works the wall (capped); escape risk rises
          tribunal -> evidence presented; faction reaction authored
          release -> walked out; may return (authored)
day 5   aftermath: the guard who talked; the child who asked questions
        (authored scenes, dignity)
```

### VII.5.2 Findings

| # | Finding | Class | Repair |
|---|---|---|---|
| PR-01 | holding consumed no food (captive ignored) | routing | daily consumption |
| PR-02 | coercion yielded a certain lead | ethics | unreliability rule |
| PR-03 | ransom ambush risk absent (free money) | authoring | ambush authored |
| PR-04 | release had no return possibility (safe forever) | consequence | return arc |
| PR-05 | guard effort not counted against schedule | routing | effort consumption |
| PR-06 | interrogation scene unreviewed | dignity | review gate before content seal |
| PR-07 | tribunal had no evidence requirements (button mash) | substance | evidence authored |

---

## §VII.6 Worked thread — the bounty on a name

### VII.6.1 Sequence

```text
A faction offers a bounty on a raider chief who has been hitting caravans.
Player accepts. The chief's camp is a location; approach options: open fight,
infiltration (P5), negotiation (authored). Completion modes: dead, captured,
driven off. Consequences: the chief's faction (if any), the chief's people
(authored, they remember), caravan safety (route risk down).
```

### VII.6.2 Findings

| # | Finding | Repair |
|---|---|---|
| PR-08 | "driven off" condition absent (only kill/capture) | third path authored |
| PR-09 | chief's camp ties not authored (a person with no people) | authored ties/arcs |
| PR-10 | reward paid twice (accept + complete) | once-key |
| PR-11 | route risk unchanged after completion | effect routed to route system |

---

## §VII.7 The security-humanity standard (one page)

```text
1. Captives are people: they eat, they speak, they are seen.
2. Coercion is unreliable; cruelty is never the efficient path.
3. Every holding has an exit; every term is authored.
4. Bounties name people; completion has consequences beyond loot.
5. Observations come from events; warnings lead to preparation.
6. Standing is a mirror of how you treated people, bounded and attributed.
7. Scenes pass dignity review before sealing.
```

---

## §VII.8 Quick reference tables

### VII.8.1 The guard-effort costs

| Task | Effort/day | Source |
|---|---|---|
| hold one prisoner | 1 | roster |
| escort transfer | 2 (for the day) | roster |
| interrogation (authored scene) | 1 | roster + scene |
| tribunal (evidence prep) | 2 (one-time) | roster + journal |

### VII.8.2 The warning ladder for security events

```text
observation (ranging) -> alert -> prep window -> contact
prisoner policy change -> morale/word -> faction contact
heat threshold -> warning scene -> avoidance -> consequence
standing drop -> visible signal (messenger, bark) -> band change
```

---

*End of Part VII. Continues in Part VIII (war escalation and occupation).*# W3-04 · PART VIII — WAR ESCALATION AND OCCUPATION: FULL DESIGNS

> The strategic layer: stage catalogs, faction goals, occupation state,
> insurgency options, and how war reaches every other system. Content-heavy
> where needed, state-light where possible; C items signed.

---

## §VIII.1 The war chain model (full)

### VIII.1.1 Stage definition (complete)

```yaml
stage: 04_siege_lines
entry: consequence ref (previous stage resolved + authored trigger)
duration: authored bands (min/max) or event-driven
effects:
  territory: [reference to map/region state]
  routes: [gate refs -> closed/delayed]
  stance: [faction deltas]
  economy: [shock refs (W3-02 lexicon)]
  defense: [threat profile per week, authored]
  population: [refugee flows, labor losses (authored)]
options:
  military: [prep, sortie, fortify]
  diplomatic: [treaty, envoy, concession]
  economic: [smuggle, ration, relocate]
  narrative: [W3-01 choice hooks]
  intelligence: [recon, sabotage (P5)]
warnings: authored (the stage announces itself through events, decrees,
  ranging, radio)
exit: authored conditions (resolution, escalation, collapse)
```

### VIII.1.2 The completeness rule

Every stage must have:

1. **≥1 effect** the player can feel (route, price, threat, population);
2. **≥1 option** the player can take (military/diplomatic/economic);
3. **warnings** before the effects land;
4. **an exit** (authored conditions, including at least one non-military
   path where the story supports it).

The audit enumerates stages × effects × options; empty cells are findings.

### VIII.1.3 The stage catalogue (proposal set)

| Stage | Character | Effects | Options |
|---|---|---|---|
| 01 tension | decrees, rumors | stance drift; prices jitter | prepare, trade, ignore |
| 02 border | clashes on routes | route delays; ranging | escort, avoid, broker |
| 03 escalation | mobilization | labor loss; demand spikes | fortify, diplomacy, evacuate |
| 04 siege lines | sustained pressure | supply cuts; raids; refugees | sortie, treaty, smuggle |
| 05 climax | decisive operations | attacks on shelter/allies | defense, intervention, surrender terms |
| 06 aftermath | rebuilding or occupation | routes restore or occupier rules | rebuild, resist, accommodate |

The catalogue is authored content; stages may be skipped/merged per the war's
authored shape, but every used stage meets the completeness rule.

### VIII.1.4 Faction war goals (C)

```text
Goal: faction, aim (territory | concession | elimination | control),
  settlement terms (authored), red lines (authored)
```

Goals frame stages and endings: an ending (W3-01 P7) can resolve a war aim;
decrees (authored events) express red lines. Content-heavy, state-light.

---

## §VIII.2 Occupation model (C-path, full)

### VIII.2.1 State

```text
Occupation:
  territory (region/location ref), controller (faction),
  regime (authored: garrison | martial | administrative),
  levy (goods/effort per period), resistance (authored level),
  legitimacy (authored, affected by player actions)
```

Stored on the war/faction owner's section (signed); derived effects computed
at read time.

### VIII.2.2 Transitions

```text
entered: by war outcome or authored event
changes: regime shifts (authored events), levy changes (resistance/politics)
exited: liberation (player/military), negotiated withdrawal (diplomacy),
  collapse (resistance + external pressure)
```

Every transition has authored triggers and warnings; no silent occupation.

### VIII.2.3 Effects (routed)

| Effect | Owner | Notes |
|---|---|---|
| levy consumption | inventory/economy owner | periodic, visible |
| movement restrictions | map/route owner | checkpoints |
| population pressure | shelter/population owner | authored events |
| resistance opportunities | W3-01 choices + W3-04 P5 | sabotage/underground |
| trade terms | W3-02 stance bands | occupier prices |

### VIII.2.4 Insurgency options (authored)

```text
sabotage (P5 missions), smuggling (W3-02 routes), recruiting (P5),
intelligence (P5), open revolt (P1+P4, severe consequences),
diplomatic pressure (W3-01/02), waiting it out (authored attrition)
```

No new resistance system: options route through existing owners.

### VIII.2.5 Liberation/withdrawal ceremonies

Authored scenes + journal (W3-01) + memorial for the cost; standing shifts;
occupation state removed (with record for the chronicle).

### VIII.2.6 Tests

```text
Occupation_EnterWarned
Occupation_EffectsRouted_<effect>
Occupation_Transitions_<direction>
Occupation_Exit_AuthoredScenes
Occupation_Persistence (signed)
Occupation_NoParallelResistance
Insurgency_OptionsRouted
```

---

## §VIII.3 War-economy handshake (C5 detail)

### VIII.3.1 The interface

```text
Stage effects -> W3-02 shock lexicon entries:
  supply cuts -> supply-cut shocks (routes closed)
  demand -> demand-spike shocks (refugees, casualties)
  cost floors -> cost-floor shocks (war premium)
Faction goals -> stance bands shifts (W3-02)
Occupations -> levies (authored flows) + price regimes
```

### VIII.3.2 The discipline

The war plane authors **events**; the economy plane authors **shocks** from
those events (via consequence refs). Neither duplicates the other's tables.
The handshake is a documented mapping: stage event → shock archetype → market
effect.

### VIII.3.3 Tests

```text
WarEconomy_StageShocksExist
WarEconomy_ShockCauseRefs
WarEconomy_NoDuplicatedState
WarEconomy_OccupierPrices_StanceBand
```

---

## §VIII.4 War-narrative handshake

```text
stages -> decrees (authored texts), radio segments (W3-01 channels),
  choices (stay/flee/broker), journal entries
goals -> ending conditions (W3-01 P7 reads)
occupation -> resistance scenes, liberation arcs
costs -> memorial integration (W3-03)
```

Rule: every stage has at least one authored narrative beat (a player-facing
moment), registered with W3-01's reactor/journal contracts.

---

## §VIII.5 The war audit checklist

```text
[ ] stage table: effects/options/warnings/exits complete
[ ] stage transitions authored and tested
[ ] faction goals authored; settlements defined
[ ] war-economy mapping documented; shocks referenced
[ ] war-narrative beats registered
[ ] occupation (C): state, transitions, effects, exits, persistence
[ ] insurgency routes through existing systems only
[ ] determinism: stage progression driven by authored conditions
[ ] soak: war arc runs end-to-end without dead stages
```

---

## §VIII.6 Worked thread — the siege that ended in terms

### VIII.6.1 Sequence

```text
week 0  stage 02 (border): route delays; ranging observations
week 1  player brokers a convoy treaty (corridor) — partial relief
week 2  stage 03 (escalation): labor losses; prices spike (shock refs)
week 3  stage 04 (siege lines): supply cuts; refugees arrive; raids
week 4  defense holds; diplomacy opens (goals: concession vs. territory)
week 5  settlement: terms authored (a treaty with costs); war exits
week 6  aftermath: rebuilding; memorial; stance shifts; routes restore
```

### VIII.6.2 Findings

| # | Finding | Class | Repair |
|---|---|---|---|
| WR-01 | stage 03 had effects but no player option (waiting only) | completeness | authored options |
| WR-02 | settlement terms applied to stance but not to routes | routing | route restoration |
| WR-03 | refugee population pressure not routed (no consumption change) | handshake | W3-02 population flow |
| WR-04 | stage warnings absent for week 3 (surprise siege) | fairness | authored warning beat |
| WR-05 | war exit left occupation flags unset (state leak) | cleanup | transition clears |
| WR-06 | memorial for war dead missing | cross-plan | W3-03 routing |
| WR-07 | treaty corridor double-restocked markets | economy | single writer (W3-02) |

### VIII.6.3 Design lesson

War is the largest **producer of consequences** in the game; its discipline is
to delegate: every effect belongs to an owner, every stage warns, every exit
cleans state. The audit's stage table is the backbone.

---

## §VIII.7 The war stage authoring worksheet (blank)

```text
STAGE: ________  ENTRY: ________  DURATION: ________
Effects (>=1): ______________________________________
Options (>=1): military ____ diplomatic ____ economic ____ narrative ____
Warnings: ____________________________________________
Exits (>=1): _________________________________________
Shocks referenced: ___________________________________ (W3-02)
Narrative beats: _____________________________________ (W3-01)
Defense threat profile: ______________________________ (P4)
Tests: _______________________________________________
Reviewer: [ ] completeness [ ] fairness [ ] tone
```

---

## §VIII.8 Quick reference: occupation numbers (proposal)

| Parameter | Value | Note |
|---|---|---|
| levy rate | authored per regime | visible, periodic |
| resistance growth | +authored/day under repression | bounded |
| liberation threshold | authored (resistance + external) | warned |
| regime shifts | authored events | no silent change |
| exit warning | written announcement | recorded for chronicle |

---

*End of Part VIII. Continues in Part IX (verification expansion).*# W3-04 · PART IX — VERIFICATION EXPANSION: KITS, SOAK, EVIDENCE, AND INCIDENTS

> Full verification design: every kit specified at case level, the soak
> scenario in detail, evidence formats, incident playbooks, and the
> closeout acceptance. Proposal only.

---

## §IX.1 The kit catalog (case-level)

### IX.1.1 Combat entry kit (P1)

```text
cases:
  K1.1 binder entry -> session created; seed recorded
  K1.2 shelter entry -> session created
  K1.3 war event entry -> session created
  K1.4 mid-fight save/load -> state exact
  K1.5 resume equals continuous (same seed)
  K1.6 outcome once (ledger scan)
  K1.7 abort clears session
  K1.8 retreat outcome routed (consequences present)
assertions: single failure lines with session id + diff
```

### IX.1.2 Condition kit (P2)

```text
K2.1 worn band resolves worse (authored delta applied)
K2.2 damaged band worse than worn
K2.3 broken behavior authored per class
K2.4 decay once per event (not tick)
K2.5 repair consumes materials (W3-05 refs)
K2.6 repair restores per authored amount; clamp at 1.0
K2.7 exclusion list items never decay
```

### IX.1.3 Stealth kit (P3)

```text
K3.1 factor monotonicity: light/noise/stance/gear each move detection
K3.2 attribution present post-event (dominant factors)
K3.3 no hidden factors (detection inputs enumerated)
K3.4 replay equal
K3.5 cover terrain consumed from map/location owner
```

### IX.1.4 Defense kit (P4)

```text
K4.1 layer role distinct (no overlap table violations)
K4.2 each input moves the outcome (ammo/crew/power/prep)
K4.3 degradation states have signals
K4.4 alert once per event via crisis path
K4.5 residual resolves via combat (never silent erase)
K4.6 repair routes to W3-05; crew effort consumed
K4.7 battery ordnance consumed per shot; reload per class
K4.8 sky armor mitigates environment only (no interception)
K4.9 counter-battery authored
```

### IX.1.5 Espionage kit (P5)

```text
K5.1 every mission type: outcomes authored (success/partial/fail/blown)
K5.2 consequence-once per mission key
K5.3 intel reads target owner; dated presentation
K5.4 agent stress routes to psychology
K5.5 capture routes to prisoners; loss routes to grief/memorial
K5.6 no infinite missions (rest cost)
K5.7 determinism: seeded outcomes
```

### IX.1.6 Counter-intel kit (P6)

```text
K6.1 per-event cap (+22 max)
K6.2 thresholds paced (no single-event crossing)
K6.3 T2/T3 warnings visible; avoidance paths work
K6.4 decay rates per class; baseline reachable
K6.5 warrant routes to combat once
K6.6 single writer
K6.7 double-agent deltas affect enemy events (C)
```

### IX.1.7 Prisoner kit (P7)

```text
K7.1 holding costs (food/water/space/guard) all consumed
K7.2 every term available; release minimum
K7.3 coercion unreliable (sampled over N=20; false leads occur)
K7.4 escape risk authored per conditions
K7.5 social pressure scenes appear at thresholds
K7.6 condition affects outcomes (wounded/healthy)
K7.7 tribunal evidence requirements
```

### IX.1.8 Bounty kit (P7)

```text
K8.1 states total; completion once
K8.2 reward routed (goods + standing)
K8.3 target consequences authored (faction/tie)
K8.4 driven-off path available where authored
K8.5 no respawn farming
K8.6 expiry authored (no punitive default)
```

### IX.1.9 Ranging kit (P8)

```text
K9.1 provenance: observations from events only
K9.2 confidence bands; faint may be wrong
K9.3 responses measurably modify defense outcomes
K9.4 aggregation window + daily cap
K9.5 queue priority (confidence/threat)
```

### IX.1.10 Standing kit (P9)

```text
K10.1 table complete (event → delta)
K10.2 per-event clamp; per-day clamp
K10.3 band transitions warned
K10.4 single bridge writer
K10.5 economy consumption (stance)
K10.6 narrative consumption (gates)
```

### IX.1.11 War stage kit (P10)

```text
K11.1 stage completeness (effect + option + warning + exit)
K11.2 transitions authored; no dead stages
K11.3 war-economy mapping references resolve
K11.4 war-narrative beats registered
K11.5 occupation (C): enter/exit warned; effects routed; persistence
```

---

## §IX.2 Soak scenario (combat/security)

### IX.2.1 Parameters

```text
20 seeds × 60 days
injections:
  raids: 2-4 (varying composition, prep states)
  intrusions: 1-2 (quiet)
  air events: 0-2
  war stages: authored arc over the 60 days
  missions: 2-5 (kits of types)
measurements:
  outcomes per ledger (duplicates?)
  heat series per seed (caps, thresholds, warnings)
  defense input usage (null runs?)
  prisoner/bounty flows
  standing series (clamps)
  determinism replay
```

### IX.2.2 Assertions

```text
no duplicate outcomes (any key twice -> fail)
heat ≤ cap; threshold crossings preceded by warnings (log check)
every defense input used in ≥1 seed (decorative input -> finding)
ranging observations consumed (surface events)
standing within clamps; transitions warned
war stages: no dead stage (each fires effects/options)
occupation: transitions warned; exits authored (if enabled)
determinism: identical histories per seed
```

### IX.2.3 Report format

```yaml
run: T3-2026-12-a
seeds: 20 days: 60
outcomes: {sessions: 140, duplicates: 0}
heat: {max: 77, cap: 80, threshold_warnings: {T2: 6, T3: 2}, unwarned: 0}
defense: {inputs_used: {ammo: 20, crew: 20, power: 17, prep: 9}}
prisoners: {held: 7, terms_used: [ransom, release, labor], coercion_false: 3}
standing: {violations: 0, warned_transitions: 5}
war: {stages: 6, dead_stages: 0}
determinism: pass
```

---

## §IX.3 Evidence and closeout

```text
docs/evidence/w3-04/
  T1-*.yaml (entry map, role table, tables completeness)
  T2-*.yaml (11 kits)
  T3-*.yaml + .csv
  findings/CS-###.md (incl. dignity-tagged)
  repairs/<id>.md
  P0_SECURITY_PREMISE.md
```

### IX.3.1 Closeout acceptance

```text
[ ] entry map complete; persistence verified per path
[ ] condition/repair loop verified
[ ] stealth attribution present; monotone
[ ] layers: roles distinct; inputs live; alerts once
[ ] espionage: consequence-once; failures costly
[ ] heat: bounded, warned, decaying
[ ] prisoners: costs, exits, unreliable coercion
[ ] bounties: once, routed, no farming
[ ] ranging: warned, useful, bounded
[ ] standing: one bridge, clamps, consumed
[ ] war: stage completeness; (C) occupation signed
[ ] soak assertions hold; determinism pass
```

---

## §IX.4 Incident playbooks

### IX.4.1 Incident: combat state lost on load

```text
symptom: mid-fight load restarts or corrupts
triage: which entry? persistence partial wired? seed restored?
containment: disable mid-fight saving for the path (authored) until fixed
postmortem: kit addition (entry × save matrix)
```

### IX.4.2 Incident: raid outcome applied twice

```text
symptom: doubled wounds/standing after a raid
triage: ledger scan for the session key; find the second writer
repair: route through the bridge; single key
postmortem: outcome-once assertion promoted to every event class
```

### IX.4.3 Incident: heat spikes to warrant

```text
symptom: T3 without warnings; player blindsided
triage: per-event cap enforcement; threshold jump logging
repair: cap + warning gate (T3 unreachable same tick as T2)
postmortem: kit case K6.2 strengthened
```

### IX.4.4 Incident: prisoner loop without exit

```text
symptom: captive held indefinitely; scene pressure absent
triage: term availability; holding cost routing
repair: release minimum + pressure scenes authored
postmortem: K7.2/K7.5 mandatory
```

### IX.4.5 Incident: delegated alert lost

```text
symptom: layer fired silently; no crisis
triage: alert emission path vs. crisis producer
repair: single path + attribution; test the emission directly
postmortem: emission assertion per layer
```

---

## §IX.5 Test naming and focus

```text
Combat_<Check>          Condition_<Check>
Stealth_<Check>         Defense_<Check>
Espionage_<Check>       CounterIntel_<Check>
Prisoner_<Check>        Bounty_<Check>
Ranging_<Check>         Standing_<Check>
WarStage_<Check>        Occupation_<Check> (C)
```

Focused: `run_test.sh Ashfall.Core.Tests/Combat*` etc. New kits run alone
first per repo policy.

---

## §IX.6 Cost model

| Tier | Effort |
|---|---|
| T1 tables + checks | 4-5 days |
| T2 kits (11) | 10-12 days |
| T3 soak | 3-4 days (shared) |
| Closeout | 1-2 days |

---

*End of Part IX. Continues in Part X (Q&A second set, checks, samples, control).*# W3-04 · PART X — Q&A (SECOND SET), CHECKLISTS, SAMPLES, AND FINAL CONTROL

---

## §X.1 Field Q&A (Q21–Q40)

**Q21. A raid resolves but nothing changes afterward. First check?**
Outcome routing: the session resolved but consequences weren't applied (or
applied twice). Ledger scan by key, then the bridge.

**Q22. Players say combat "feels random" beyond the dice.**
Hidden inputs: detection factors or enemy composition unstated. Legend the
inputs (post-event attribution), verify composition via selector.

**Q23. Condition drops while the item sits in storage.**
Tick-based decay (a finding). Decay is per account event; storage decay is an
authored neglect rate, not per-tick.

**Q24. The airlock alarm never sounds.**
Silent-state class: unpowered/undegraded alert paths. Either author the
warning (dark panel) or make the failure loud.

**Q25. Defense works even with no crew.**
Input not consumed. Check schedule consumption; a layer without crew must
degrade per its table.

**Q26. An espionage mission succeeds but the faction notices instantly.**
Heat attribution too aggressive per event (cap) or a direct stance write. Check
the mission's consequence table and the per-event heat cap.

**Q27. Agents never get tired.**
Stress/rest not routed (K5.4/K5.6). Mission assignments must consume rest and
feed psychology bands.

**Q28. A blown agent vanishes from the story.**
Agent-loss consequence path missing. Capture → prisoners; death → grief;
disappearance → authored uncertainty. No silent removal.

**Q29. Heat decays even while actively trading.**
Decay rules are per class; active sources should re-raise within caps, never
below the minimum. Check source re-firing during the window.

**Q30. Prisoner scenes feel like a punishment meter.**
Dignity review: holding pressure should be authored human interactions, not
stat pings. Return to the scene authoring guide (VII.1.4).

**Q31. Ransom ambush chance is invisible.**
Traps are authored but must be prepared for: the faction's behavior telegraphs
(authored warning), and the player choice is explicit.

**Q32. Bounty targets respawn.**
Respawn farming (K8.5). Targets are authored people with authored durations;
if the faction re-issues, it is a new authored target.

**Q33. Ranging shows exact enemy numbers always.**
Confidence bands violated. Faint observations may be wrong; certainty only
above the band threshold.

**Q34. Preparation choices don't matter.**
Useless-warning class. Check K9.3: preparation must modify defense outcomes in
the tables, or the warning system is theater.

**Q35. Standing keeps drifting one way.**
Unclamped or unbalanced table. Check per-event/per-day clamps and the
mirroring rules (bounty/hostility).

**Q36. War stages pass with nothing for the player to do.**
Completeness violation. Each stage authors ≥1 option; a "wait" stage must have
authored beats or is returned.

**Q37. Occupation levies surprise the shelter.**
Enter-warned rule: occupation entry is announced; levies are periodic and
visible. Check the warning beat registration (VIII.2.6).

**Q38. Liberation left old occupation flags.**
Transition cleanup missed. Exit ceremonies clear state; the audit's state-leak
finding covers it.

**Q39. Two missions double-count the same intel.**
Intel reads are dated and de-duplicated per target/state; repeated reads
should return freshness, not stack.

**Q40. The whole security layer "feels like a spreadsheet".**
Tone/playbook: every table needs at least one authored human beat per stage
(scene, bark, journal note). Mechanics without moments read as spreadsheets;
that is a content finding, not a mechanical one.

---

## §X.2 Governance Q&A (Q41–Q60)

**Q41. Who authorizes occupation?**
The C-path signature (§IV.7); nothing in B touches territory state.

**Q42. Can combat add new enemy types freely?**
Via the selector catalog with authored compositions; the plan doesn't gate
content, only routes.

**Q43. Do we need new save sections?**
Combat no; occupation yes (signed); agents/prisoners ride existing owners
(verify at P0; any gap becomes a signed item).

**Q44. How does the plan handle player-character capture?**
If authored, it is a combat outcome with a route to the prisoner/arc systems;
no special player-only system.

**Q45. Is torture ever allowed in the game?**
Only as an authored coercive action with unreliable outcomes and dignity
review. Never as a mechanic that rewards cruelty with certainty.

**Q46. Do factions remember specific combat acts?**
Through standing (aggregate) and authored narrative beats (specific). Both
attributed.

**Q47. Can defense be fully AI-managed?**
The player sets policies (crew, doctrine) and may delegate posts; outcomes
still route through owners. Delegation is an authored policy, not an autopilot
system.

**Q48. What stops an exploit of bounty + release loops?**
Targets authored once; release return arcs; no respawns. The exploit is
authored away rather than patched.

**Q49. How is classified intel kept from leaking into UI?**
Intel is presented per source/freshness; the surface (W3-06) renders reads,
never owner state raw.

**Q50. What about allied AI factions fighting each other?**
Through the war chain (clashes, stages) with authored outcomes; no side
simulator.

**Q51. Where do warcrimes fit?**
The game's tone excludes explicit warcrime mechanics; mistreatment surfaces
via standing/morale consequences only, restrainedly authored, dignity-reviewed.
No graphic content.

**Q52. How deterministic are combats?**
Fully seeded per session; replay equality is a kit case (K1.5).

**Q53. Do morale effects from combat route to W3-03?**
Yes via consequence refs (wounds, deaths, witnessed violence). The routing is
documented in the war-narrative handshake (VIII.4).

**Q54. Can players negotiate during combat?**
Authored encounters may include parley outcomes (retreat/terms); those are
encounter outcomes (P1) routed like others.

**Q55. What is the maximum scripted complexity for a battle?**
Authored cap (e.g., ≤8 participants, ≤3 phases) to keep resolution readable
and testable; larger set-pieces are multiple encounters.

**Q56. Are defense doctrines available at B?**
No — C only; B verifies the layers and inputs doctrines will modify.

**Q57. What if the ranging engine is already wired differently?**
P0 verifies; the plan adapts to the actual wiring rather than forcing the
model (Rule 7).

**Q58. How are night/day and weather consumed?**
Through exposure factors (P3), route gates, and defense tables; owners are
W2-04/clock.

**Q59. What is the single most important security test?**
The soak's "no duplicate outcomes + all warnings fired" pair. It catches the
two failure classes that ruin campaigns: doubled consequences and surprise
attacks.

**Q60. What does success look like to a player?**
Raids feel prepared-for, defense choices matter, spies are people, prisoners
are heavy, heat teaches, and the war is a story the player helped steer. That
is the plan.

---

## §X.3 Implementation checklists (per point)

### X.3.1 P1 combat host

```text
[ ] entry table complete; persistence per entry
[ ] session record (seed, participants, state)
[ ] outcome vocabulary + routing table
[ ] idempotency keys per outcome
[ ] abort path clears
[ ] mid-fight round-trip matrix (entry × outcome × save point)
```

### X.3.2 P2 condition

```text
[ ] condition tables per class; bands + effects
[ ] event-based decay (no tick decay)
[ ] broken behavior authored
[ ] repair recipes (W3-05) + material consumption
[ ] exclusion list
```

### X.3.3 P3 stealth

```text
[ ] factor enumeration + monotonicity tests
[ ] attribution post-event
[ ] seeded rolls
[ ] terrain/cover from owners
```

### X.3.4 P4 defense

```text
[ ] role table (no overlap)
[ ] layer state machines + inputs + degradation tables
[ ] input→outcome wiring verified
[ ] alerts through crisis once, attributed
[ ] residual → combat
[ ] repairs via W3-05 + schedule
[ ] battery ordnance/reload; armor role separation
```

### X.3.5 P5 espionage

```text
[ ] outcome tables per mission type
[ ] consequence-once keys
[ ] intel owner reads + dating
[ ] agent stress/rest/loss routing
[ ] capture → prisoners; death → grief
```

### X.3.6 P6 counter-intel

```text
[ ] per-event cap; pacing
[ ] thresholds warned; avoidance paths
[ ] decay rates; cap
[ ] warrant → combat once
[ ] single writer
```

### X.3.7 P7 prisoners/bounties

```text
[ ] holding costs routed
[ ] term matrix complete; release minimum
[ ] coercion unreliable sampled
[ ] pressure scenes authored + reviewed
[ ] bounty states/once/driven-off/expiry
```

### X.3.8 P8 ranging

```text
[ ] provenance from events
[ ] confidence presentation
[ ] response modifiers measured
[ ] aggregation + caps
```

### X.3.9 P9 standing

```text
[ ] event table; clamps; warnings
[ ] bridge single writer
[ ] consumption (economy/narrative)
```

### X.3.10 P10 war

```text
[ ] stage table complete
[ ] transitions; effects/options routing
[ ] economy mapping; narrative beats
[ ] occupation (C) signed; transitions; persistence
```

---

## §X.4 Sample documents

### X.4.1 Sample: entry map row

```markdown
## Entry: shelter_encounter
- Initiator: ShelterEncounterSystem.TriggerRaid()
- Session: TacticalCombatSystem.Start(session: raid_...) seed from campaign RNG
- Persistence: .Persistence partial (verified: SaveCombatState/ RestoreCombatState)
- Outcomes: repel (standing+, loot), loss (wounds, deaths, loot loss),
  retreat (partial), captured (prisoner/arc)
- Bridge: CombatFactionStandingBridge.Apply(session key raid:<id>)
- Tests: K1.2, K1.4, K1.5, K1.6
```

### X.4.2 Sample: heat threshold doc

```markdown
## Threshold T2 — watchlist (45)
- Trigger: heat >= 45 (accrual through events, capped +10..+22 each)
- Consequence: shakedown scene (pay/refuse/stall), informants active
- Warning: direct scene begins with the knock (in-fiction warning)
- Avoidance: pay (heat -8), refuse (heat +4 but no scene escalation),
  stall (heat unchanged; 3-day window)
- Tests: K6.3, K6.4
```

### X.4.3 Sample: ranging observation

```yaml
observation:
  type: armor
  direction: north ridge
  confidence: 0.62          # probable band
  source: war clash #114 (consequence ref)
  day: 31
consumers: [alert surface, defense prep]
prep_effects: {posts: +coverage, curfew: +safety, escort: +route_safety}
```

---

## §X.5 Reference tables

### X.5.1 Outcome vocabulary (one page)

| Outcome | Owner route |
|---|---|
| wounds/casualties | health owner |
| deaths | grief/memorial (W3-03) + journal |
| standing | bridge (P9) |
| loot/inventory | inventory owner |
| captures | prisoner system (P7) |
| route/territory | map/war owner |
| intel | espionage record |
| narrative hooks | W3-01 ledger |

### X.5.2 The warning forms

```text
ranging observation    -> alert + prep window
heat threshold         -> scene warning before consequence
standing band change   -> messenger/ bark signal
defense degradation    -> surface note + repair prompt
war stage              -> decrees, radio, events
occupation entry       -> announcement + scene
```

### X.5.3 The three stop-the-line failures

```text
duplicate outcome application
silent fatal (unwarned punitive death)
silent state leak (occupation flags surviving exit)
```

---

## §X.6 Final version record

| Version | Change |
|---|---|
| v1.0 | Part I summary contract |
| v1.1 | Part II deep designs 1–10 |
| v1.2 | Part III playbooks + verification + threads |
| v1.3 | Part IV Q&A + C-path + appendices |
| v1.4 | Part V defense deep playbook |
| v1.5 | Part VI intelligence deep playbook |
| v1.6 | Part VII prisoners/bounties/ranging/standing |
| v1.7 | Part VIII war escalation + occupation |
| v1.8 | Part IX verification expansion |
| v1.9 | Part X this part (Q&A 21–60, checklists, samples, control) |

---

## §X.7 Final control (W3-04)

**W3-04 expanded status:** complete at the expanded target. Parts I–X.
Proposal only. Execution requires Annex U signatures (Part I §U.2) and
§IV.7. The stop-the-line failures (X.5.3) and the humanity rules
(VII.7) are binding within this document.

One closing word for the security plan:

> The measure of this system is not how well the shelter fights. It is
> whether the player understands why the fight came, which observation warned
> them, what preparation changed, and who was hurt. Security that cannot
> answer those questions is noise; this plan exists to keep the answers
> visible.

*Document control: W3-04 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-04.*# W3-04 · PART XI — ENEMY COMPOSITION, AI MOVEMENT, AND ENCOUNTER DESIGN

> The adversary side in authoring depth: how enemies are selected,
> composed, moved, and framed; how encounters are paced and varied; and the
> narrative meaning of different foes. Proposal only.

---

## §XI.1 Enemy composition system (full)

### XI.1.1 The selector contract

```text
EnemyCompositionSelector.Select(context) -> Composition
context inputs (all from owners):
  faction/who            (war/faction owner)
  era/threat tier        (campaign clock/war stage)
  location danger        (map/location owner)
  narrative conditions   (W3-01 flags)
  difficulty             (difficulty owner — W2-03 bands)
composition output:
  participants[] (archetypes, counts, gear bands), tactics profile,
  morale/loot bands, seed
```

Authoring rule: compositions are **data profiles** selected by rules, never
ad-hoc spawned lists. The audit: every encounter's composition traceable to a
profile + context.

### XI.1.2 Profile authoring template

```yaml
profile: raider_siege_veteran
for: [faction_raiders, war_stage>=3]
size_band: 4-7
archetypes: [breacher x1-2, rifleman x2-4, scout x1]
gear_bands: [worn, mixed]
tactics: aggressive | cautious | patient | desperate
morale_band: steady (war-hardened)
loot_band: mixed (salvage, some ammunition)
narrative: [war_stage refs]
seed policy: campaign RNG (deterministic)
```

### XI.1.3 Composition audits

1. **Legal archetypes:** every participant archetype exists in the combat
   catalog; unknown refs fail static checks.
2. **Scale bound:** sizes within authored caps per context (readability and
   resolution limits).
3. **Gear coherence:** gear bands match the era/faction authored economy; a
   stage-1 militia with pristine rifles is a finding.
4. **Loot coherence:** loot bands referenced from item catalogs; no unique
   quest items in random loot.
5. **Determinism:** selection seeded by the session seed (campaign RNG
   contract), never wall clock.

### XI.1.4 Tests

```text
Composition_ArchetypesExist
Composition_SizeBands
Composition_GearCoherence
Composition_LootCatalogs
Composition_Deterministic
Composition_ContextTrace
```

---

## §XI.2 AI movement and tactical behavior

### XI.2.1 The movement contract

`CombatAiMove` partial exists. The audit: movement decisions derive from owned
state (positions, cover, objectives), use the seeded RNG where stochastic,
and never read hidden player state (no omniscient AI).

### XI.2.2 Behavior profiles (authoring)

| Profile | Behavior | Counterplay |
|---|---|---|
| aggressive | close and press | fallback, choke points |
| cautious | hold cover, suppress | flank, patience |
| patient | wait for attrition | supply (they run down), rotate |
| desperate | abandon cover risk | hold nerve; they break or win |
| disciplined | focus fire, cover each other | area denial, morale play |

Profiles come from composition; the audit checks the profile affects actual
move/decision tables (no decorative profile field).

### XI.2.3 Information rules

```text
AI reads: visible units, known objectives, last known positions, authored
  garrison knowledge
AI does not read: player OOC state, hidden inventory, unseen movement
drift: last-known positions age (authored memory window)
```

### XI.2.4 Tests

```text
Ai_ProfileAffectsBehavior_<profile>
Ai_NoOmniscience
Ai_MemoryWindow
Ai_Deterministic_MoveOrder
Ai_ObjectivesFromOwner
```

---

## §XI.3 Encounter design playbook (expanded)

### XI.3.1 The encounter shape model

```text
Shape:
  premise (why are we fighting: raid | ambush | defense | escape | standoff)
  phases (authored 1-3): contact -> escalation -> resolution
  beats per phase: authored triggers (losses, reinforcements, objective)
  exits: outcomes (win/loss/retreat/parley) with consequences
```

### XI.3.2 Phase authoring

Each phase declares: entry trigger, tension function (what escalates), and
beat list. Reinforcement beats come from composition profiles (legal adds);
objective beats change the win condition (defend the door, hold the line,
extract). Phases without beats are empty; the review returns them.

### XI.3.3 Variety discipline

The audit samples encounters across a soak and measures:

| Metric | Target |
|---|---|
| composition spread | profiles used ≥ authored count |
| shape spread | all shapes represented |
| phase-length variety | not all single-phase |
| outcome spread | all outcomes occur |

A campaign of identical fights is a content finding; the selector rules are
the fix.

### XI.3.4 Difficulty coherence

Difficulty (W2-03 bands) modifies compositions and margins, not enemy
intelligence law. The audit: difficulty inputs exist in the selector context
and change compositions authored-ly; no hidden cheat scaling.

### XI.3.5 Tests

```text
Encounter_PhasesHaveBeats
Encounter_ReinforcementsFromProfiles
Encounter_ObjectiveBeatsLegal
Encounter_VarietySoak
Difficulty_AuthoredInputs
Difficulty_NoHiddenScaling
```

---

## §XI.4 The adversaries' humanity (tone)

### XI.4.1 Named vs. unnamed

Composition participants may be unnamed (faceless rank-and-file) but the
game's tone asks:

1. **Aftermath acknowledges them:** the dead are recorded (numbered, not
   named, unless authored), stripped of dehumanizing language.
2. **Motives are contextual:** raiders are starving too; authored context
   (war, famine) explains pressure without excusing.
3. **No caricature:** faction behavior authored; no cartoon evil.
4. **Prisoners extend the humanity:** captures (P7) are people with terms.

### XI.4.2 The tone review items

```text
[ ] no gratuitous detail in text
[ ] aftermath respectful (burial/records authored)
[ ] faction context authored (why they fight)
[ ] no real-world references
[ ] restraint in language
```

---

## §XI.5 Scenario pack (five mini-threads)

### XI.5.1 Mini-thread: the fence line

```text
A perimeter segment fails at night (storm). Player choices: repair with
materials now (cost, exposed during work) or wait (coverage gap). Raiders
scout the gap (authored observation). Outcome depends on prep.
Findings: coverage gap signal, repair under risk, scout observation routing.
```

### XI.5.2 Mini-thread: the extract

```text
An expedition is pinned (event). Send relief (crew, combat), negotiate
(authored), or abandon (narrative cost). Combat resolution of the relief;
losses; standing; arcs (who was left).
Findings: relief timing, negotiation path, abandonment consequences.
```

### XI.5.3 Mini-thread: the quiet breach

```text
An intrusion succeeds (thief). Investigation (W3-01), counter-intel, heat.
Theft effect on inventory; the thief may be authored (a person).
Findings: investigation content, inventory deltas, heat linkage.
```

### XI.5.4 Mini-thread: the counter-battery

```text
The battery fires (intercept success). Enemy counter-battery event damages it
(authored rate). Repair or hide the battery. Ammo depletion.
Findings: counter-battery authored, damage states, repair costs.
```

### XI.5.5 Mini-thread: the line held

```text
A major raid: all layers engaged; the player's prep (posts, ammo, prep time
from ranging, doctrine C) compounds. Losses authored; the aftermath scene
authored; the memorial for the fallen.
Findings: compounding verification, aftermath content, memorial routing.
```

---

## §XI.6 The enemy audit checklist

```text
[ ] selector profile coverage per context
[ ] archetypes/gear/loot coherence
[ ] AI profiles effective; no omniscience; memory window
[ ] encounter shapes/phases/beats authored
[ ] variety measured in soak
[ ] difficulty inputs authored, no hidden scaling
[ ] tone items reviewed
[ ] determinism across compositions and AI
```

---

## §XI.7 Quick tables

### XI.7.1 Composition contexts (proposal)

| Context | Size band | Gear | Tactics pool |
|---|---|---|---|
| stage 1 militia | 3-5 | improvised | cautious, desperate |
| stage 2 raiders | 4-6 | worn/mixed | aggressive, patient |
| stage 3 veterans | 4-7 | mixed | disciplined, aggressive |
| stage 4 elite | 5-8 | good | disciplined, patient |
| intruders | 1-3 | light | patient, desperate |

### XI.7.2 AI memory windows (proposal)

| Profile | Last-known memory | Note |
|---|---|---|
| aggressive | 1 phase | presses |
| cautious | 2 phases | repositions |
| patient | 3 phases | waits |

---

*End of Part XI. Continues in Part XII (threat catalogue and scenario pack).*# W3-04 · PART XII — THE THREAT CATALOGUE: FACTIONS, TELLS, COUNTERPLAY, AND ESCALATION

> Per-adversary security profiles: what they do, how they are detected, what
> preparation counters them, and how they escalate. A content catalogue built
> to the plan's contracts.

---

## §XII.1 Catalogue entry template

```yaml
threat: <name>
class: [ground | air | covert | environmental]
origin: <faction/actor + context>
signature: tells (ranging, radio, bark, evidence) with confidence bands
composition_profiles: [refs]
tactics: [profiles]
timing: seasons/stages of activity
targets: [shelter | routes | allies | infrastructure]
counterplay:
  preparation: [posts | doctrine | route choice | diplomacy]
  during: [engagement options | retreat | parley]
  after: [repair | pursuit | investigation]
escalation: <what happens if unchecked (authored stage table)>
de-escalation: <what settles them (authored options)>
narrative hooks: [W3-01 refs]
```

Every threat in the catalogue is a **parameterized adversary**, not a spawn
table: it has a reason, a tell, a counter, and a way to stop.

---

## §XII.2 Threat: the raiders (ground pressure)

### XII.2.1 Profile

```text
class: ground
origin: war-stage driven; starving armed groups
signature: ranging (foot/engines), scout evidence, burned markers (authored)
composition: raider profiles (stage-scaled)
tactics: aggressive/patient depending on stage
targets: stores, routes, isolated buildings
```

### XII.2.2 Tells

| Tell | Confidence | Player action window |
|---|---|---|
| distant engines (ranging) | probable | hours |
| scout striding the ridge | faint→certain | a day |
| missing patrol contact | certain | immediate |
| burned way-station | certain | warns of route danger |

### XII.2.3 Counterplay

```text
prep:  posts at likely approaches; ammo readied; route closure
during: defense layers (P4); parley rarely authored (they want to take)
after:  repair, pursuit (authored), standing with affected factions
settle: mission against their base (P5) or a treaty (authored, costly)
```

### XII.2.4 Escalation table

| Unchecked offenses | Stage | Effect |
|---|---|---|
| 1-2 raids survived | probing | rare raids |
| 3-4 | bold | heavier compositions; route ambushes |
| 5+ | siege | stage-4 profile; allies pressured |

### XII.2.5 Findings to watch

```text
tells without ranging provenance
composition gear not stage-coherent
settlement path missing (unbeatable stall)
escalation unbounded (no authored cap/recovery)
```

---

## §XII.3 Threat: the state force (disciplined pressure)

### XII.3.1 Profile

```text
class: ground+air
origin: a faction's regulars (war stage or stance-driven)
signature: patrol schedules (intel), transmissions (radio), flags
composition: veteran/elite profiles; disciplined tactics
targets: control points, contraband, alignment
```

### XII.3.2 Tells and counterplay

Tells are *institutional*: patrol rhythms, checkpoints, wanted notices. The
counterplay is political as much as military: keep standing above the band
that authorizes action, use diplomacy, avoid contraband exposure (P5 heat).

### XII.3.3 The standing lockout

```text
below hostile band -> normal
hostile -> inspections, seizures (authored), curfews
banned -> military action (combat path; warnings)
recovery: standing repair through authored acts (trade, aid, returns)
```

The lockout must be recoverable: the audit verifies every punitive band has a
repair path with authored acts.

---

## §XII.4 Threat: the quiet hand (covert pressure)

### XII.4.1 Profile

```text
class: covert
origin: an intelligence service, competitor, or vendetta
signature: evidence trail (items, doors, missing reports), not ranged
composition: individual agents; contact rarely direct combat
targets: information, sabotage, recruitment
```

### XII.4.2 Counterplay

```text
prep:  vetting, watch rotation, decoys, counter-intel ops
during: detection signals (P6), confrontation choice
after:  investigation (W3-01), double-agent play, heat management
settle: turn an agent, expose a network, or outlast the attention
```

### XII.4.3 The unattended covert threat

If never addressed, the quiet hand steals, misinforms, and eventually
targets a decisive moment (authored). The escalation is *information loss*,
not firepower — the plan's counter-intel design must reflect that.

---

## §XII.5 Threat: the elements (environmental security)

### XII.5.1 Profile

```text
class: environmental (W2-04 handshake)
origin: storms, ash falls, floods, cold
signature: weather forecasts (warned), sky-layer changes
impact: layers degraded, routes closed, power faults
```

### XII.5.2 Counterplay

Maintenance (materials), stocking, sealing routes, sky armor upkeep, and
authored building choices. Security and environment share the damage surface:
the audit verifies environment damage routes to the same repair owners, and
that warnings come from weather owners (no duplicated warning systems).

---

## §XII.6 Cross-threat composition (pitched pressure)

The catalogue's design value: threats stack with authored interactions:

```text
raiders + storm  -> approach slowed but shelter more exposed (allocations)
state force + quiet hand -> inspections cover agents; heat and standing both
raiders + state force -> routes contested both ways; diplomacy leverage
covert + environmental -> sabotaged seals during a fall
```

Each pair gets an authored interaction note; the plan tests the two highest
frequency pairs in soak.

---

## §XII.7 The threat catalogue worksheet (blank)

```text
THREAT: ________  CLASS: ________  ORIGIN: ________
Tells: ______________________________________ (confidence bands)
Compositions: _______________________________ (profiles)
Counterplay prep/during/after: ______________
Settle paths: _______________________________
Escalation table: ___________________________
De-escalation: ______________________________
Narrative hooks: ____________________________
Tone review: [ ] (no caricature; reasons authored)
```

---

## §XII.8 Faction security relations table (proposal)

| Faction state | Border | Raids | Covert | Trade |
|---|---|---|---|---|
| allied | open | none | none | full |
| friendly | open | none | rare | full |
| neutral | open | rare (roused) | possible | partial |
| cold | delayed | occasional | likely | restricted |
| hostile | closed | frequent | active | black market |
| war | contested | war stages | intense | none |

The table is a mapping of existing owners (stance, war, route systems) — it
does not create new state; it documents how security reads politics.

---

*End of Part XII. Continues in Part XIII (operations dossier and readiness).*# W3-04 · PART XIII — READINESS OPERATIONS: ROSTERS, DRILLS, MAINTENANCE, AND THE DEFENSE CULTURE

> The day-to-day of defense: who stands where, what drills change, how
> maintenance cycles run, and how the shelter's defense culture emerges from
> authored policy rather than hidden systems.

---

## §XIII.1 Roster and post system

### XIII.1.1 Post model

```text
Post:
  location (segment/tower/gate), requirement (crew count/skill),
  schedule (shifts), doctrine modifier
Assignment:
  survivor -> post -> shift; effort consumed; fatigue accrues
Coverage:
  derived = posts staffed / posts required (per segment)
```

### XIII.1.2 Authoring rules

1. Coverage is **derived** from assignments, never stored separately (no
   second truth — same discipline as W3-03 aggregation).
2. Shift work accrues fatigue per authored bands; fatigue reduces
   effectiveness (P4 input).
3. Empty posts produce the coverage-gap signal (V.3.4) — a warning, not a
   silent hole.
4. Assignments respect survivor condition (wounded cannot stand posts) with
   authored reasons surfaced.

### XIII.1.3 Tests

```text
Roster_CoverageDerived
Roster_FatigueAccrues
Roster_EmptyPostWarned
Roster_ConditionGating
Roster_AssignmentPersist
```

---

## §XIII.2 Drills and readiness

### XIII.2.1 Drill model

```text
Drill: authored training event (fire drill, breach drill, evacuation)
effects (authored bands):
  response time improvement (temporary/permanent per type)
  crew skill band up (bounded)
  fatigue cost (real)
  morale effect (authored: confidence or resentment)
```

### XIII.2.2 Rules

1. Drills cost time and energy — no free readiness.
2. Effects are bounded and documented; repeated drills diminish (habituation),
   never stack to a cheat.
3. Drills are authored events with scenes (W3-01 hooks), not menu toggles.
4. Readiness state (skill bands) lives on the roster/defense owner.

### XIII.2.3 Tests

```text
Drill_EffectBands
Drill_DiminishingRepeats
Drill_FatigueCost
Drill_MoraleAuthored
Drill_ReadinessPersist
```

---

## §XIII.3 Maintenance cycles

### XIII.3.1 Preventive maintenance

```text
Task: inspect/repair a layer segment/equipment class
inputs: materials (W3-05), crew time (schedule)
effects: condition band maintained; failure probability reduced
neglect: authored decay rate (condition down over time without inspection)
```

### XIII.3.2 The neglect curve

Allowing condition to run down is an authored state with tells (visible wear,
authored notes) and consequences (reduced response, then failure events).
Inspection prevents; the player chooses the cadence.

### XIII.3.3 Tests

```text
Maintenance_InputsConsumed
Maintenance_ConditionMaintained
Neglect_DecayObservable
Neglect_FailureEventAuthored
Maintenance_CadenceAuthored
```

---

## §XIII.4 Defense culture (the soft layer)

### XIII.4.1 What culture means here

The shelter's authored attitudes toward security: confidence, watchfulness,
resentment of curfews, pride in drills. Mechanically it is **morale and group
effects routed through W3-03 owners** — no separate culture stat.

### XIII.4.2 Authored expressions

```text
events: drill resentment scene; watch rota volunteers; a fatal raid changes
  mood (authored scenes); a successful defense brings pride (authored)
consumers: dialogue selection, scene eligibility, assigned W3-03 bands
```

### XIII.4.3 The tone rule

Security content must show both sides: the value of readiness and its human
cost (fear, exhaustion, resentment). A defense culture that is all pride reads
as propaganda; one that is all dread reads as cruelty. The review balances.

### XIII.4.4 Tests

```text
Culture_RoutesMoraleOwner
Culture_ScenesRegistered
Culture_NoSeparateStat
Culture_ToneReview
```

---

## §XIII.5 The readiness cycle (operational loop)

```text
threat warning -> prep window -> assignments/doctrine -> contact ->
aftermath -> maintenance/drills -> culture feedback -> next warning
```

Each arrow is an owner seam: warning (P8/weather/war), prep (roster/P4),
contact (P1), aftermath (routing), maintenance (W3-05/schedule), culture
(W3-03), feedback (narrative). The plan's job: no arrow without a seam.

---

## §XIII.6 Readiness failure modes (catalogue)

| # | Failure | Symptom | Fix |
|---|---|---|---|
| R-01 | coverage stored (second truth) | assignments and coverage diverge | derive coverage |
| R-02 | drills free | readiness without cost | inputs consumed |
| R-03 | drills stacked | cheat scaling | habituation caps |
| R-04 | empty post silent | surprise reduction | warning signal |
| R-05 | neglect invisible | mystery failures | tells + notes |
| R-06 | culture as a stat | "security morale" bar | route to W3-03 |
| R-07 | maintenance materials skipped | free repair | W3-05 routing |
| R-08 | assignment ignores wounds | wounded on posts | condition gating |

---

## §XIII.7 Quick tables

### XIII.7.1 Shift model (proposal)

| Shift | Length | Fatigue/unit | Coverage |
|---|---|---|---|
| day watch | 8h | +0.05 | standard |
| night watch | 8h | +0.08 | reduced vision (authored) |
| double | 16h | +0.20 | degraded |
| rest day | — | −0.30 | — |

### XIII.7.2 Maintenance cadences (proposal)

| Layer | Inspection interval | Neglect onset |
|---|---|---|
| airlock | 14 days | 21 days |
| perimeter | 10 days | 14 days |
| battery | 7 days | 10 days |
| sky armor | 21 days | 30 days |

---

*End of Part XIII. Continues in Part XIV (final appendices and control).*# W3-04 · PART XIV — FINAL APPENDICES: SAMPLE DOCUMENTS, GLOSSARY SUPPLEMENT, AND CONTROL

> Closing appendices: sample records, the extended glossary, the handoff index,
> and the final control. Completes W3-04 at the expanded target.

---

## §XIV.1 Sample: combat session record

```yaml
session:
  id: raid:174
  entry: shelter_encounter
  seed: campaign#174 (deterministic)
  participants:
    shelter: [guard_a, guard_b, survivor_erev]
    raiders: [breacher_1, rifleman_1, rifleman_2, scout_1]
  phases:
    - contact: initial exchange; breach attempt on airlock
    - escalation: perimeter posts engage; one raider down
    - resolution: raiders withdraw (loss band)
  outcome: repel
  consequences:
    wounds: {guard_b: light}
    standing: {faction_raiders: -0.05, faction_salt: +0.10}
    loot: {ammo_rifle: +12, salvage_scrap: +4}
    narrative: [scene_after_raid_hold (authored)]
  routed_once: true
  evidence: {duplicate_scan: clean}
```

---

## §XIV.2 Sample: defense readiness report

```markdown
## Readiness — day 34 (generated view, read-only)
| Layer | State | Inputs | Coverage | Note |
|---|---|---|---|---|
| airlock | sealed | crew 2/2, power ok | full | manual cycle drill run day 30 |
| perimeter W | intact | crew 2/3 | gap (west) | gap warning active |
| perimeter E | exposed | crew 1/3, ammo low | degraded | repair task queued |
| battery | ready | 4 rds, crew 1 | — | reload 2 days |
| sky armor | maintained | — | full | inspect day 40 |
Warnings: coverage gap (west) — assign crew or accept risk.
```

The report is derived; no stored state beyond the owners. The surface work is
W3-06.

---

## §XIV.3 Sample: heat operation log

```markdown
## Counter-intelligence log (day 28-40)
- day 28: recon vs. forge -> +3 heat (12)
- day 30: sabotage partial -> +8 (20) -> T1 crossing; rumor warning fired
- day 33: blown runner -> +16 (36) -> warning: patrol questions
- day 35: shakedown scene -> pay (-8) -> 28
- day 38: informant tipped -> +5 (33) -> surveillance beat
- day 40: lay low -> -8 -> 25 (decaying)
Warnings fired before each threshold consequence: T1 (rumor), T2 (scene).
```

---

## §XIV.4 Extended glossary

| Term | Definition |
|---|---|
| abort path | cleanup for a cancelled combat session |
| archetype | combatant data class in the composition catalog |
| composition profile | authored enemy package selected by context |
| coverage | derived staffed/required ratio per defense segment |
| counter-battery | enemy fire at the sky battery (authored event) |
| doctrine | authored posture modifying layer inputs (C) |
| driven off | bounty completion mode short of death/capture |
| entry path | route that begins a combat session |
| escape risk | prisoner attempt chance per authored conditions |
| evidence chain | authored clue sequence for investigations |
| fatigue bands | roster effectiveness reductions from shift work |
| gear band | authored equipment quality class for compositions |
| heat class | transaction/warrant/political decay grouping |
| holding cost | daily prisoner resource consumption |
| idempotency key | once-only outcome application key |
| informant | recruited person feeding intel (consent-framed) |
| intrusion | quiet hostile entry (not a raid) |
| levy | occupation resource draw (C) |
| lockout | stance-driven access restrictions with repair paths |
| memory window | AI last-known-position aging |
| neglect curve | authored condition decay without maintenance |
| observation | ranging contact record with confidence |
| parley | authored negotiation outcome in encounters |
| post | roster position with requirements |
| pressure scene | authored prisoner-related human beat |
| profile | AI tactical behavior class |
| provenance | event source of an observation |
| readiness | derived state of layers + roster + maintenance |
| residual | threat remaining after layer responses |
| settled path | authored way a threat stops (treaty, mission, time) |
| shakedown | T2 heat consequence scene |
| signature | a threat's detectable tells |
| state leak | occupation/war flags surviving an exit (finding) |
| tell | authored warning sign of a threat |
| tribunal | authored prisoner adjudication term |
| warrant | T3 heat consequence leading to a raid |
| warden effort | roster cost of holding/escorting prisoners |

---

## §XIV.5 Handoff index

| Receiver | Deliverable |
|---|---|
| W3-01 | encounter narrative hooks; aftermath scenes; war beats |
| W3-02 | standing→stance; war shock mapping; occupation levies |
| W3-03 | trauma/grief triggers; group cohesion effects |
| W3-05 | repair recipes; ordnance/materials; maintenance parts |
| W3-06 | readiness/heat/ranging surfaces; alert presentation |
| W2-02 | session lifecycle repair coordination |
| W2-03 | threat/difficulty measurement handoff |
| W2-04 | environment damage/warning handshake |
| W2-06 | scene prose; tone review |
| UNBLOCK-03 | security strings through the freeze when declared |

---

## §XIV.6 Known limitations

```text
L1  Persistent injuries (C2) need health-owner API confirmation.
L2  Agent networks (C3) depend on relationship-owner capacity.
L3  Occupation (C4) is the largest signed item; its persistence shape is
    pending the war owner's current section review at P0.
L4  Counter-battery rates are authored proposals; W2-03 measures pressure.
L5  Ranging/war feed specifics depend on the verified debt row; P0 confirms.
L6  Batteries' ordnance catalog depth is what exists; new ordnance is content.
L7  Attack compositions assume the current archetype catalog; expansions
    extend profiles, not the system.
```

---

## §XIV.7 Version record

| Version | Change |
|---|---|
| v1.0–v1.3 | Parts I–IV (summary, deep designs, playbooks, Q&A/C/appendices) |
| v1.4 | Part V defense deep playbook |
| v1.5 | Part VI intelligence deep playbook |
| v1.6 | Part VII prisoners/bounties/ranging/standing |
| v1.7 | Part VIII war escalation + occupation |
| v1.8 | Part IX verification expansion |
| v1.9 | Part X Q&A 21–60 + checklists + samples |
| v2.0 | Part XI enemy composition + AI + encounters |
| v2.1 | Part XII threat catalogue |
| v2.2 | Part XIII readiness operations |
| v2.3 | Part XIV this close |

---

## §XIV.8 Final control (W3-04)

**W3-04 expanded status:** complete at the expanded target. Proposal only. No
execution without Annex U (Part I §U.2) and §IV.7 signatures. Binding within
this document: the three stop-the-line failures (§X.5.3), the humanity rules
(§VII.7), the no-silent-state rule (§V.2.3), and the no-second-truth rule.

> The final measure of the security plan is trust: when the sirens come, the
> player should know why, know what they prepared, and know what it will cost
> — and when it is over, know who paid. That is what every table in these
> fourteen parts exists to make true.

*Document control: W3-04 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-04.*# W3-04 · PART XV — THE EXECUTION RUNBOOK: FROM P0 TO CLOSEOUT, STEP BY STEP

> The operational book a builder follows: prerequisite checks, phase-by-phase
> steps, commands, expected outputs, and the decision points inside execution.
> Everything here assumes signatures from Annex U and §IV.7 are in hand.

---

## §XV.1 Preconditions (before any work)

```text
[ ] Annex U.2 signatures present for the phases being run
[ ] §IV.7 C items signed or explicitly excluded
[ ] P0 premise document scheduled (first task)
[ ] claim registered per WORKTREE_OWNERSHIP discipline
[ ] test policy reviewed (focused runs; no full-suite)
[ ] the D19c/static coordination with W2-02 noted if P1/P5/P6 touch statics
```

If any precondition fails, stop and escalate to the foreman (Rule 10).

---

## §XV.2 P0 — premise execution (day 1–3)

### XV.2.1 Steps

```text
1. verify each owner referenced in Part II exists at current HEAD
   (TacticalCombatSystem parts, ConditionSystem, StealthSystem, Defense layers,
   Espionage/CounterIntel, Prisoner/Bounty, Ranging, Standing bridge, War chain)
2. capture file:line evidence for each into docs/security/P0_SECURITY_PREMISE.md
3. run the existing selftests for the family:
     godot --headless --path . -- --7day-smoke-selftest
     (and any combat/defense-specific selftests verified present)
4. record current known-state gaps (the debt row re: ranging feed)
5. confirm data families: combat catalogs, ordnance, perimeter, intel,
   sound-ranging, war stages
6. list any premise that has changed since the plan's writing (Rule 7)
```

### XV.2.2 Deliverable

`docs/security/P0_SECURITY_PREMISE.md` — owners, evidence, selftest results,
changed premises, and the adjusted phase order if needed.

### XV.2.3 Exit criteria

Every Part II owner line has current evidence; no phase starts on a stale
premise.

---

## §XV.3 P1 — combat entry/persistence execution

```text
1. build the entry table (P0 caveat list -> entries)
2. for each entry: script an initiation; verify session record + seed
3. run the round-trip matrix (entry × save point × outcome)
4. verify outcome-once per key across the full scenario set
5. verify abort cleanup
6. repair findings (ranked, cap 20); evidence per repair
7. kit green; proceed
```

Commands (illustrative at P0):

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Combat
# host-level scenario runner as verified at P0
```

Expected outputs: kit report with per-case lines; zero duplicate outcomes.

---

## §XV.4 P2 — condition execution

```text
1. extract condition tables; verify bands/effects authored
2. run decay-once and repair tests
3. verify broken behavior per class
4. repair findings; kit green
```

---

## §XV.5 P3 — stealth execution

```text
1. enumerate exposure factors; source each to an owner
2. monotonicity tests per factor
3. attribution presence test
4. replay equality
```

---

## §XV.6 P4 — defense execution

```text
1. role table verification (no overlaps)
2. per-layer state machines + degradation tables authored
3. input→outcome tests (each input moves the result)
4. alert-once through the crisis path
5. residual→combat test
6. repair routing (W3-05 refs)
7. battery ordnance/reload; armor separation
8. kit green
```

---

## §XV.7 P5 — espionage execution

```text
1. mission outcome tables verified per type
2. consequence-once across the scenario set
3. intel owner reads; dating/staleness presentation
4. agent routing (stress/rest/capture/loss)
5. determinism
6. kit green
```

---

## §XV.8 P6 — counter-intel execution

```text
1. per-event cap and pacing tests
2. threshold warnings + avoidance paths
3. decay rates; cap; single writer
4. warrant→combat once
5. kit green
```

---

## §XV.9 P7 — prisoners and bounties execution

```text
1. holding costs routed (all four)
2. term matrix complete; release minimum
3. coercion unreliability sampled (N=20)
4. pressure scenes authored + dignity-reviewed
5. bounty states/once/driven-off/expiry
6. kit green
```

---

## §XV.10 P8 — ranging execution

```text
1. provenance verification (events only)
2. confidence banding; faint-may-be-wrong authored
3. response modifiers measured (preparation matters)
4. aggregation + caps
5. kit green
```

---

## §XV.11 P9 — standing execution

```text
1. event table complete
2. clamps per event/day
3. bridge single writer
4. consumption by economy + narrative verified
5. kit green
```

---

## §XV.12 P10 — war stages execution

```text
1. stage completeness table (effects/options/warnings/exits)
2. transitions authored + tested
3. war-economy mapping refs resolve
4. war-narrative beats registered
5. (C) occupation per signature: transitions, effects, exits, persistence
6. kit green
```

---

## §XV.13 Soak execution

```text
1. configure the shared harness (seeds, injections)
2. run; collect measurements
3. assertions per §IX.2.2
4. findings triage; repairs ranked
5. evidence pack
```

Expected: zero duplicates; all warnings fired; no decorative inputs; war runs
end-to-end.

---

## §XV.14 Closeout

```text
1. final kit + soak runs on the frozen HEAD
2. evidence pack assembled
3. closeout memo (below)
4. Annex U releases recorded
5. ledger proposals (debt rows for deferrals)
```

### XV.14.1 Closeout memo template

```text
OUTCOME:
FILES:
CONTRACT: entry persistence; outcome-once; layer roles; heat bounds;
          prisoner exits; ranging utility; standing bridge; stage completeness
COMMANDS: T1/T2/T3 + results
LIMITATIONS:
SHARED PATHS TOUCHED:
LEDGER PROPOSALS:
ANNEX U RELEASES EARNED:
```

---

## §XV.15 In-execution decision points (quick guidance)

| Situation | Decision |
|---|---|
| an entry path has no persistence and adding it touches saves | stop; agent-signed item or exclude the entry from B |
| a layer input is decorative at P0 | wire or remove; no third option |
| coercion sampling shows certainty | fix table before content seals |
| occupation shape at P0 differs from the C sketch | adapt the C design to the owner; never fork |
| W2-03 bands conflict with a repair's pressure | record; repairs fix truth; bands measure |
| a finding implies a new system | escalate (Rule 5) |
| test flake | quarantine with reason + hypothesis; never silent retry |

---

## §XV.16 Post-execution maintenance

```text
per content change: entry/composition/layer-completeness checks
weekly: T1; soak spot-run; findings triage
per release: T3; threat catalogue review; warning-ladder audit
```

---

*End of Part XV. Continues in Part XVI (final control and reading card).*# W3-04 · PART XVI — FINAL CONTROL, READING CARD, AND APPENDICES CLOSE

---

## §XVI.1 Reading card

```text
ASHFALL W3-04 · COMBAT/DEFENSE/SECURITY · READING CARD

WHAT:    one resolution authority; every entry persists; every outcome
         routes once; layers defend with real inputs; spies are people;
         heat warns; prisoners exit; warnings mean preparation.
WHY:     security is a story of preparation, not ambush.
HOW:     P0 premise -> P1 entry/persistence -> P2 condition -> P3 stealth ->
         P4 layers -> P5 espionage -> P6 counter-intel -> P7 prisoners/
         bounties -> P8 ranging -> P9 standing -> P10 stages/occupation.
GATES:   T1 tables/returns; T2 eleven kits; T3 soak (duplicates=0,
         warnings=100%, inputs live, stages complete).
STOPS:   duplicate outcomes; unwarned fatal; silent state leaks.
NEVER:   second engine; torture reward; silent degradation; parallel alert
         systems; unbounded heat/standing.
SIGN:    Annex U + §IV.7.
```

---

## §XVI.2 The plan's ten sentences

```text
1. Combat starts from many doors; all of them must persist and resolve.
2. Condition is consequence; wear must be felt and repair must cost.
3. Detection must be explainable, or stealth is a coin toss.
4. Layers defend together but answer separately; none erases the threat.
5. Missions matter; failures cost; consequences apply once.
6. Heat is attention: it accrues, it warns, it fades, it never surprises fatally.
7. Captives are people: they eat, they speak, they leave somehow.
8. A warning is a promise of preparation; never make one you cannot keep.
9. Standing is the mirror of conduct; keep it truthful and bounded.
10. War is the largest story in the game; every stage must be sayable in a
    player's own words when it ends.
```

---

## §XVI.3 Consolidated open items (before execution)

```text
[ ] P0: verify the ranging feed (debt row) as currently wired
[ ] P0: locate the prisoner needs/space owners (capacity routing)
[ ] P0: confirm agent storage owner (survivor vs. contact record)
[ ] P0: confirm war chain owner's section for occupation (C)
[ ] P0: check combat selftest availability and command names
[ ] coordination: W2-02 for any static/session issues found
[ ] coordination: W3-02 shock refs for war stages
[ ] coordination: W3-01 hooks for aftermath/war beats
[ ] signature check: C2/C4 signed before their phases
```

---

## §XVI.4 The eleven kits index

```text
P1 CombatEntry_Kit        P2 Condition_Kit       P3 Stealth_Kit
P4 Defense_Kit            P5 Espionage_Kit       P6 CounterIntel_Kit
P7 Prisoner_Kit + Bounty_Kit
P8 Ranging_Kit            P9 Standing_Kit        P10 WarStage_Kit
(+ Occupation_Kit C)
```

---

## §XVI.5 The final acceptance statement

```text
The plan is closed (Path B) when:
  every entry path persists and resolves,
  every outcome applies exactly once,
  every layer role is distinct and every input is live,
  every punitive path warned with an exit,
  coercion is unreliable, prisoners have exits,
  warnings prepare and preparation matters,
  standing is bounded and consumed,
  every war stage is complete,
  and the soak runs clean with determinism.

The plan is closed (Path C adds):
  occupation transitions authored and persisted,
  doctrines costed and signed,
  agent networks and persistent injuries signed and routed.
```

---

## §XVI.6 Version record (complete)

| Version | Change |
|---|---|
| v2.4 | Part XV execution runbook |
| v2.5 | Part XVI this final control (reading card, open items, acceptance) |

---

## §XVI.7 Final control

**W3-04 complete at the expanded target.** Parts I–XVI. Proposal only. No
execution without Annex U and §IV.7 signatures. Binding within this document:
stop-the-line failures (§X.5.3), humanity rules (§VII.7), no-silent-state
(§V.2.3), no-second-truth, and the runbook's preconditions (§XV.1).

*Document control: W3-04 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-04.*# W3-04 · PART XVII — ACCEPTANCE MATRIX AND SEEDED FINDINGS CATALOGUE (FINAL ADDENDUM)

---

## §XVII.1 The acceptance matrix (10 points × 6 dimensions)

| # | Point | Truth | Bounds | Routing | Warnings | Persistence | Tests |
|---|---|---|---|---|---|---|---|
| 1 | combat host | entries entirely persist | session caps authored | outcomes → bridge once | abort/retreat authored | round-trip per entry | CombatEntry_Kit |
| 2 | condition | bands affect resolution | clamp [0,1]; decay events | repair → W3-05 | broken state visible | condition owner | Condition_Kit |
| 3 | stealth | factors explain detection | monotone factors | outcomes → resolver | attribution post-event | n/a | Stealth_Kit |
| 4 | layers | distinct roles | input consumption | alerts → crisis | degradation signals | layer state owner | Defense_Kit |
| 5 | espionage | mission outcomes real | per-event heat caps | consequences once | blown → arcs | agent state owner | Espionage_Kit |
| 6 | counter-intel | heat paced | cap + decay | warrant → combat | T2/T3 warned | heat owner | CounterIntel_Kit |
| 7 | prisoners | terms exist | holding costs | outcomes → owners | pressure scenes | captive records | Prisoner_Kit |
| 7b | bounties | targets are people | rewards bounded | completion once | expiry authored | bounty state | Bounty_Kit |
| 8 | ranging | provenance events | daily cap | prep → defense | confidence bands | n/a (derived) | Ranging_Kit |
| 9 | standing | table complete | per-event/day clamps | bridge single writer | band transitions | standing owner | Standing_Kit |
| 10 | war stages | effects/options total | stage durations | mapping refs valid | stage warnings | (C) occupation | WarStage_Kit |

Every cell has an owner and a test; empty cells are the audit's first output.

---

## §XVII.2 The seeded findings catalogue (50)

### XVII.2.1 Combat and condition (F01–F10)

| # | Finding | Class | Repair |
|---|---|---|---|
| F01 | mid-fight save restarts on one entry | persistence | wire partial |
| F02 | outcome applied by two paths | once | single key |
| F03 | aborted session lingers | hygiene | cleanup |
| F04 | seed lost on load | determinism | restore seed |
| F05 | entry divergence (modifier sets differ) | truth | single init |
| F06 | worn weapon same accuracy | substance | apply bands |
| F07 | broken gun still fires | authored state | broken behavior |
| F08 | repair without materials | routing | W3-05 consume |
| F09 | double decay per round | single-write | event write |
| F10 | condition above 1.0 after repair | clamp | clamp |

### XVII.2.2 Stealth (F11–F15)

| F11 | non-monotone noise factor | truth | formula |
| F12 | no attribution after detection | learning | factor list |
| F13 | hidden factor (undeclared state) | truth | declare/source |
| F14 | unseeded detection | determinism | seeded |
| F15 | cover ignored from map owner | wiring | read owner |

### XVII.2.3 Defense (F16–F25)

| F16 | ammo does not change interception | substance | wire input |
| F17 | unpowered alarm silent | silent state | warning authored |
| F18 | battery stops ground raids (role overlap) | role | split |
| F19 | total prevention default | design | residual rule |
| F20 | repair free | routing | materials |
| F21 | crew fatigue ignored | fidelity | bands |
| F22 | alert bypasses crisis | routing | single path |
| F23 | counter-battery absent | completeness | event authored |
| F24 | sky armor intercepts (overlap) | role | separate |
| F25 | ordnance not consumed | routing | inventory |

### XVII.2.4 Intelligence (F26–F35)

| F26 | mission success no consequence | routing | route effects |
| F27 | heat applied twice | once | idempotency |
| F28 | intel stored as copy (stale) | truth | owner read |
| F29 | blown mission free | costs | author costs |
| F30 | agent stress unrouted | routing | psychology |
| F31 | agent loss silent | completeness | grief/capture paths |
| F32 | heat T3 in one event | pacing | per-event cap |
| F33 | threshold unwarned | fairness | warning + avoid |
| F34 | warrant bypassed avoidance path | wiring | read avoidance |
| F35 | informant promise untracked | contract | obligation record |

### XVII.2.5 Prisoners and bounties (F36–F42)

| F36 | holding free | routing | consumption |
| F37 | no release path | completeness | minimum exit |
| F38 | coercion always true | ethics | unreliability |
| F39 | ransom ambush absent | authoring | trap event |
| F40 | release safe forever | consequence | return arc |
| F41 | bounty paid twice | once | key |
| F42 | target respawn farming | design | no respawns |

### XVII.2.6 Ranging and standing (F43–F47)

| F43 | observation orphan | consumer | wire surface |
| F44 | preparation useless | promise | outcome link |
| F45 | confidence shown as certainty | honesty | banding |
| F46 | observation spam | hygiene | aggregation/cap |
| F47 | standing clamp missing | bounds | clamp |

### XVII.2.7 War and occupation (F48–F50)

| F48 | stage with no option | completeness | author options |
| F49 | exit leaves occupation flags | leak | transition cleanup |
| F50 | stage effects not mapped to economy | handshake | shock refs |

---

## §XVII.3 The repair ranking rule

```text
Rank by: (1) stop-the-line classes, (2) duplicates/leaks, (3) ethics/warning,
(4) bounds/decay, (5) completeness, (6) cosmetic.
Cap 20 per phase; overflow -> debt rows with owners.
Never fix (6) while (1) is open.
```

---

## §XVII.4 Cross-check: findings vs. kits

| Kit | Catches |
|---|---|
| CombatEntry_Kit | F01–F05 |
| Condition_Kit | F06–F10 |
| Stealth_Kit | F11–F15 |
| Defense_Kit | F16–F25 |
| Espionage_Kit | F26–F31 |
| CounterIntel_Kit | F32–F35 |
| Prisoner/Bounty_Kit | F36–F42 |
| Ranging_Kit | F43–F46 |
| Standing_Kit | F47 |
| WarStage_Kit | F48–F50 |

Completeness: every seeded finding has a catching kit; a finding without a kit
means the kit design has a hole (finding itself).

---

## §XVII.5 The final three tables

### XVII.5.1 Severity classes of findings

| Class | Response |
|---|---|
| stop-the-line | fix before any other work |
| ethics | fix or redesign; review required |
| fairness | fix (warning/exit) |
| bounds | fix (safety) |
| truth | fix (single source) |
| hygiene | schedule |
| cosmetic | backlog only |

### XVII.5.2 The evidence minimum per repair

```text
before output, after output, command, HEAD, owner, reviewer
```

### XVII.5.3 The completion meter

```text
[ ] kits green (11)
[ ] soak assertions (7 groups)
[ ] stop-the-line zero
[ ] ethics zero
[ ] warnings 100% fired in soak
[ ] duplicates zero
[ ] determinism pass
```

---

*End of Part XVII. W3-04 closes here.*

---

# W3-04 · CLOSING ADDENDUM — THE THREAT LADDER AND SECURITY CONTRIBUTOR CARD

## The threat ladder (one page)

```text
T0  quiet      no active threat; tells only
T1  probing    first contacts; warnings issuing
T2  pressure   sustained operations; defenses tested
T3  siege      combined pressure; all layers engaged
T4  decisive   the defining battle(s); consequences land
T5  aftermath  repairs, memorial, politics, recovery

Every rung is authored: tells, warnings, options, consequences.
No rung is silent; no rung lacks a path.
```

## The security contributor card

```text
BEFORE A SECURITY FEATURE SHIPS, ANSWER:
  what warns the player?            (tell + timing)
  what can they prepare?            (input that matters)
  what does it cost?                (effort/ammo/standing)
  what happens if ignored?          (escalation, bounded)
  what settles it?                  (authored resolution)
  who gets hurt?                    (routing: wounds, grief, standing)
IF ANY ANSWER IS BLANK: RETURN.
```

## The final drill set

```text
D1  unwarned raid           -> fairness finding
D2  double outcome          -> stop-the-line
D3  free defense            -> input ownership finding
D4  certain interrogation   -> ethics finding
D5  endless prisoner        -> exit completeness finding
D6  heat one-event jump     -> pacing finding
D7  occupation state leak   -> cleanup finding
D8  silent layer failure    -> visibility finding
```

## Closing line

> Security in ASHFALL is preparation made meaningful: the warning that gave
> time, the choice that mattered, and the cost paid honestly.

**W3-04 complete (180k-class).** Proposal only; execution requires Annex U
and §IV.7 signatures.

*Document control: W3-04 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-04.*

---

# W3-04 · PART XVIII — THE SECURITY CORPUS REGISTER

> Strings, scene hooks, and content refs the security systems need, listed
> for the corpus handoff (W2-06) and the freeze route (UNBLOCK-03).

## XVIII.1 Warning and alert copy

```text
alert_ranging_armor        "engines, north ridge"           (confidence-banded)
alert_ranging_foot         "movement, the low fields"
alert_gate_delay           "the north road is closed"
alert_perimeter_gap        "nobody on the west wall"
alert_battery_reload       "the battery is dry"
alert_heat_watch           "someone has been asking questions"
alert_heat_watchlist       "they are watching you now"
alert_heat_warrant         "there is a notice with your name on it"
alert_occupation_entry     "the garrison has taken the crossing"
```

## XVIII.2 Threshold scenes (hooks)

```text
scene_shakedown             T2 heat: pay/refuse/stall
scene_warrant_warning       T3: the knock and the options
scene_interrogation         prisoner/person under questioning (dignity)
scene_ransom_offer          faction contact
scene_tribunal              evidence presented
scene_after_raid_hold       defense aftermath
scene_liberation            occupation exit
scene_agent_extraction      captured informant rescue
```

## XVIII.3 Outcome copy refs

```text
outcome_repelled            "you held"
outcome_retreat             "you pulled back"
outcome_captured            "they took one of ours"
outcome_bounty_done         "it's finished"
outcome_offer_driven_off    "they won't come back"
outcome_treaty_signed       "the corridor is open"
outcome_treaty_collapsed    "the corridor is closed"
```

## XVIII.4 Presentation rules

```text
[ ] warnings precede effects (never the reverse)
[ ] confidence bands on observations (never false precision)
[ ] no gratuitous detail in aftermath copy
[ ] deceased/faction language respectful and plain
[ ] all refs registered before use; freeze routing when declared
```

## XVIII.5 The register close

> Every warning is a promise; every scene is a person's moment; every
> outcome line is written like someone will read it after losing something.

*End of Part XVIII. W3-04 closes (180k-class).*

---

# W3-04 · PART XIX — FINAL APPENDICES AND METRICS CLOSE

## XIX.1 Final metric run (expected shape)

```yaml
run: T3-final
seeds: 20 days: 60
sessions: 140 duplicates: 0
entry_paths_verified: all
condition_effects: verified
detection_attribution: present
layers: {roles_distinct: true, inputs_live: true, alerts_once: true}
missions: {consequence_once: true, failures_cost: true}
heat: {cap: enforced, warned: true, decay: true}
prisoners: {costs: routed, exits: complete, coercion_unreliable: sampled}
bounty: {once: true, farming: none, expiry: authored}
ranging: {provenance: true, useful: true, bounded: true}
standing: {single_writer: true, clamps: true, consumed: true}
war: {stages_complete: true, dead_stages: 0}
determinism: pass
```

## XIX.2 The final review card

```text
SECURITY SHIPS WHEN:
  every fight can be saved, resumed, and resolved once
  condition is felt and repair costs
  detection is explainable
  layers reduce but never erase
  missions matter and failures cost
  heat warns and fades
  captives eat, speak, and leave somehow
  warnings prepare and preparation matters
  standing mirrors conduct within bounds
  every war stage is sayable in the player's own words
```

## XIX.3 The closing statement

```text
The security systems exist so that danger is legible: the player should
know what is coming, what they chose to prepare, and what it cost — and who
paid. Nothing else about them matters as much.
```

**W3-04 complete.** Proposal only; execution requires Annex U and §IV.7.

*Document control: W3-04 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-04.*

---

# W3-04 · PART XX — THE CLOSING CHECKLIST AND HANDOFF CLOSE

## XX.1 The one-page close

```text
[ ] P0 premise filed; owners verified; selftests recorded
[ ] P1 entry/persistence truth; outcome-once
[ ] P2 condition tables; repair routing
[ ] P3 detection attribution; monotone factors
[ ] P4 layer roles; inputs live; alerts once; residual via combat
[ ] P5 mission outcomes; consequence-once; agent routing
[ ] P6 heat bounds; warned thresholds; decay
[ ] P7 prisoner costs/exits; coercion unreliable; bounties once
[ ] P8 ranging provenance/usefulness/bounds
[ ] P9 standing single-writer/clamps/consumption
[ ] P10 stage completeness; (C) occupation signed
[ ] soak assertions; determinism; evidence pack
```

## XX.2 The handoff close

```text
to W2-01: generated security checks join the focused pipeline
to W3-01: aftermath/war scene hooks and outcome refs filed
to W3-02: war shock mapping and occupation levies filed
to W3-03: trauma/grief trigger refs filed
to W3-05: repair/ordnance recipe refs filed
to W3-06: readiness/heat/ranging surface rows filed
```

## XX.3 The security register (strings)

```text
All warnings, threshold scenes, outcome lines, and alert copy are filed in
Part XVIII; corpus authorship per W2-06; freeze routing when declared.
```

## XX.4 The final statement

```text
Security in ASHFALL is not a wall — it is a conversation with danger that
the player can hear, answer, and survive. Keep the conversation audible.
```

**W3-04 complete (target-class).** Proposal only.

*Document control: W3-04 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-04.*

---

# W3-04 · PART XXI — THE ANSWER KEY

## XXI.1 Drill answers (the reviewer's key)

```text
D1  unwarned raid -> finding: fairness; return for warning + prep window
D2  double outcome -> finding: stop-class; single key, bridge route
D3  free defense -> finding: ownership; consume inputs
D4  certain interrogation -> finding: ethics; unreliable rule
D5  endless prisoner -> finding: completeness; release minimum + pressure
D6  one-event heat jump -> finding: pacing; per-event cap
D7  occupation state leak -> finding: cleanup; transition clears
D8  silent layer failure -> finding: visibility; authored signal
```

## XXI.2 The five review outputs

```text
SIGNED:    all rules hold; evidence present
RETURNED:  specific notes (mechanism + missing test)
ESCALATED: theme/struct concern (two alternatives named)
DEBT:      accepted with owner + expiry (never silent)
REJECTED:  scope violation (new system proposed; Rule 5)
```

## XXI.3 The final compliance map

| Rule | Where verified |
|---|---|
| one resolution authority | P1 audit; soak duplicates=0 |
| warn-before-harm | threshold kits; soak log |
| coercion unreliable | P7 sampling |
| bounded standing/heat | P6/P9 clamps |
| stage completeness | P10 table |
| determinism | every kit replay |

## XXI.4 The close

```text
If a finding cannot name its owner, its warning, and its exit, it is not
ready to be called fixed. The answer key above exists so nobody has to
guess what "fixed" means.
```

**W3-04 complete.** Proposal only.

*Document control: W3-04 · final addendum · HEAD 5be1a30a.*

---

# W3-04 · FINAL SIGNATURE CARD

```text
ASHFALL WAVE 3 · PLAN 4 · FINAL SIGNATURE
HEAD: ________  Date: ________
[ ] P1 entry/persistence truth        [ ] P2 condition/repair
[ ] P3 stealth attribution            [ ] P4 layers/alerts
[ ] P5 espionage consequence-once     [ ] P6 heat bounds
[ ] P7 prisoners/bounties             [ ] P8 ranging utility
[ ] P9 standing bridge                [ ] P10 war stages
[ ] soak assertions                   [ ] determinism
Signed: ________  Foreman: ________
```

*End of W3-04 · final.*

---

# W3-04 · CLOSING MEASUREMENT DECLARATION

```text
The security plan is closed when every fight can be resumed, every outcome
applied once, every warning fired before its harm, every captive able to
leave, and every stage of the war sayable in the player's own words.
Measurement is the closeout; hope is not.
```

*End of W3-04 (final).*

---

# W3-04 · THE LAST LINE

```text
Hold the line so it can be told: what came, what warned, what you prepared,
what it cost, who paid. Everything else in these twenty parts serves those
five questions.
```

**End of W3-04 (180k-class).** Proposal only; execution requires Annex U and
§IV.7 signatures.