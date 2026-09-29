# Feature / Task Plan: The Long Siege — multi-day sieges over the existing raid, watch and perimeter owners

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit)

> Prose companion: `docs/expansions/expansion_long_siege_plan.md`. Family index: `docs/expansions/expansion_shelter_under_pressure_index.md`.
> Not a claim. **Does not touch Expansion 36 (*The Watch*)** — `NightWatchHostSession`, `NightWatchOperationsState`, patrol readiness and sound ranging are read-only inputs. The defense owner (`DefenseSystem`) gains one additive nested DTO field and **no logic change**.

> **Editorial polish (prose pass):** sections **0**, **1b** and **12** are narrative texture only. No
> authority, claimed path, decision, acceptance criterion or verification step changes. Sample lines
> are content candidates for `siege_lines.json` / `siege_terms.json` rows; they belong in data, never
> in code.

---

## 0. Prologue — Two Clocks

> *"A raid wants something and leaves. A siege wants you to have been here, and stays."*

Every other combat in this game resolves in an afternoon. A siege is what happens when the enemy
discovers patience. There are two clocks and neither is yours: **supply** and **resolve** — theirs —
and behind them a third thing the plan calls *patience*, which is simply how long a doctrine can
stand being bored outside a wall.

Five pressures, one per day, seeded: Probe, Starve, Noise, Parley, Sap. Each one touches an
existing surface and nothing else. There is no battle here. There is a schedule being kept by
someone who is in no hurry, and a shelter that has to decide, every morning, which clock to spend.

**Tone & register.** Bureaucratic dread. The vocabulary is the staff table: *doctrine, patience,
supply, resolve, rung, terms, runner, sally*. Prose should read like a log kept by someone who
understands that the interesting question is not *will we hold* but *what will we have agreed to by
the time this ends*. Never write a battle scene. Write a week.

**Mystery & texture.** The three starter doctrines — **Toll, Quiet Ring, Diggers** (DEC-LS-08) —
are the plan's three ways of being feared. Each has its own patience and its own temper. Which one
arrives is not a difficulty setting; it is a character walking up to your gate. §12 keeps the
besiegers' reasons permanently out of reach.

## 1. Goal & Outcome

> *Design intent: the player should be able to feel a siege in their admin. The dread is not the
> fighting — it is the eleventh consecutive morning of reading a board.*

- **Goal:** Represent a *siege* as a multi-day state — identity, doctrine, camp, and two clocks — that calls the **existing** raid resolver once per day at seeded strength, reads defenders' stores, water, morale and wire state from their real owners, and ends in one of five authored ways. Only the besiegers' supply, resolve and patience are new state.
- **Outcome (observable):** on a fixed seed a siege begins after a repelled raid when a doctrine row and aggression allow; each day applies exactly one seeded pressure (Probe/Starve/Noise/Parley/Sap) through an existing surface; besieger resolve falls with supply and failed pressure; a runner, a sally and a parley each change the correct clock; the siege ends Lifted/Relieved/Negotiated/Ground down/Fallen; the raid path with no doctrine rows behaves identically to today; save/load mid-siege round-trips.
- **Non-Goals:** no change to `ResolvePreCombatRaid`, raid strength, trap arithmetic, or any Watch/perimeter state; no new combat engine; no new faction system; no new save section; no new routed panel; no outposts (Year Two owns `OutpostPressureModel`); no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; raid parity holds; handoff lists untouched shared paths.

## 1b. Texture, Mystery & Voice

**One pressure per day (DEC-LS-03) is the plan's literary rhythm.**

Probe, Starve, Noise, Parley, Sap — exactly one a day, at most one resolver call. That constraint
is what makes a siege readable as a *narrative* rather than a damage table. Each morning the board
says what kind of day it is going to be, and the shelter answers with one decision. Five verbs,
seven days, and the shape of a week becomes the shape of a story.

**The five endings are five different kinds of tired.**

Lifted, Relieved, Negotiated, Ground down, Fallen. Note that only one of them is defeat and only
one is victory. The other three are settlements — arrangements made by exhausted people. LS-P6
routes each through existing owners, which means each ending is a *transaction*, not a cutscene.

**What the player is never told.**

- Who the besiegers are. DEC-LS-01 says a siege starts from a repelled raid **by authored doctrine
  only**. Identity is a row; motive is not authored and must not be improvised.
- What the Diggers are digging for. The name is doctrine (DEC-LS-08). Sap is a signal to *The Deep
  Works* and nothing more.
- Why a runner came back. `siege_runner_outcomes.json` supplies seeded outcomes. Whether the runner
  talked is a question for *The Quiet War*, and this plan hands it over without comment.
- Whether *Fallen* was inevitable. DEC-LS-06 requires a **visible run of days** before it can happen.
  That visibility is the only mercy in the design.

**Voice — sample fragments (content candidates for `siege_lines.json` / `siege_terms.json`).**

> "Day four. Noise again. They have learned that we do not sleep, which cost them nothing to learn
> and costs us everything to know."

> "Parley at the gate. The terms are reasonable. That is the part I cannot get past."

> "Sap. The Watch heard it before we did. The Watch hears everything and is not authorised to
> decide anything."

> "Supply is a number. Resolve is not a number. We print them on the same board and I have started
> to resent the board."

**Design texture beats.**

- **Announce before acting, always (LS-P3/DEC-LS-05).** Defender clocks are read-only projections.
  The player must never lose to something they were not shown.
- **At most one resolver call per siege day (§6.3).** This is the plan's respect for the existing raid
  arithmetic — and its narrative spine. Do not add a second roll.
- **Difficulty scales patience and probe strength, never the resolver (DEC-LS-10).** Harder sieges
  are longer sieges. That is far worse and far fairer.
- **A siege is not a raid.** The moment this plan starts to look like combat, it has failed its
  own Non-Goals.

---

## 2. Evidence table (verified 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | `ResolvePreCombatRaid(day, raiderStrength, isNight, perimeter, poweredProvider, targetingRng, captureRng, guardFraction)` → `DefenseEngagementResult`. | `Defense/DefenseSystem.cs` L290; result class L90–101 | LIVE |
| E2 | Defense state `settlement_defenses`: installations (HP, armed/sprung/broken, manned_by) and `raid_log` (cap 50); `CaptureState`/`RestoreState`. | `DefenseSystem.cs` L69–76, L106, L423–460 | LIVE |
| E3 | The resolver is called only from `Main.Muster.cs` L174 (Iron Raiders raid) and one debug command `Main.Defense.Integration.cs` L203. | grep | LIVE |
| E4 | Iron Raiders: aggression 0..1, visibility, `EvaluateRaidChance`, `ExecuteRaid`, `OnRaidExecuted`; `crewDanger` is 6 if aggression ≥ 0.6 else 3. | `Muster/IronRaidersSystem.cs` L27–90; `Main.Muster.cs` L158–170 | LIVE |
| E5 | Seeded targeting/capture streams via `CampaignStreamIds.DefenseTargeting` / `DefenseCapture` (fork keyed `(day, 0)`). | `src/Main.Defense.Integration.cs` L83–90 | LIVE |
| E6 | The Watch: `NightWatchHostSession` (duty roster, perimeter, gate, acoustics), readiness engine, operations state. | `src/Host/NightWatchHostSession.cs`; `World/NightWatch*.cs`; `night_watch_operations.json` | LIVE (read-only) |
| E7 | Treaty raid-pressure modifier composes at the read site. | `Main.Muster.cs` L197–215 | LIVE |
| E8 | No expedition start gate in `ExpeditionSystem`. | grep `CanStart/StartBlocked` — none | **VERIFY (P0)** |
| E9 | Guard animals normalise to a detection fraction passed to the resolver. | `Main.Defense.Integration.cs` L110–115 | LIVE |
| E10 | Deep well (yield ledger), sump pump: water owners. | `DeepWellSystem.cs`; save section `deep_well` | LIVE |
| E11 | Real morale/sleep owner and its read API. | needs system; `Survivors/` | **VERIFY (P0)** |
| E12 | Where faction aggression is fed from and how factions get *identity* rows for a besieger. | `FactionWarSystem` / muster | **VERIFY (P0)** |
| E13 | Difficulty binding hook for raid strength (XP-WAVE1). | `INTEGRATION_PLANS.md` | **VERIFY (P0)** |
| E14 | Panels: `DefenseGridPanel`, `NightWatchPanel`. | `src/UI/` | LIVE |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Raid resolution, traps, perimeter | `DefenseSystem`, `PerimeterDefenseSystem` | additive nested `siege` DTO in `settlement_defenses`; no logic change |
| Raid opportunity | `IronRaidersSystem` | none; a siege *starts* from a repelled raid, by authored doctrine only |
| Watch, acoustics, gate | Expansion 36 owners | read-only |
| Stores/water/morale/sleep | real owners | read-only |
| Siege state (identity, doctrine, supply, resolve, patience, day count, phase) | — | `SiegeSystem` (pure Core) — **DEC-LS-01/02** |
| Road cutting | expedition/travel host | one dispatch refusal via an additive predicate — **DEC-LS-04** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Defense/SiegeSystem.cs` (new, pure), `Defense/SiegeDoctrine.cs` (new), `Defense/SiegeEndings.cs` (new), `Defense/DefenseSystem.cs` (additive nested DTO field only), `CatalogIntegrityValidator.cs` (`INT`), `Random/` stream id (`INT`)
**Data:** `siege_doctrines.json`, `siege_pressures.json`, `siege_terms.json`, `siege_runner_outcomes.json`, `siege_lines.json`
**Host:** `src/Main.Defense.Integration.cs` (`INT`, daily siege tick), `src/Main.Muster.cs` (`INT`, siege-start hook after a repelled raid), a dispatch refusal in the expedition host (`src/Host/ExpeditionHostSession.cs`, `INT`), day-owner registration (`src/Main.CampaignOwners.cs`, `INT`)
**Presentation:** extend `DefenseGridPanel` and `NightWatchPanel` with a Siege Board strip — **DEC-LS-07**; no new routed panel
**Tests:** `Ashfall.Core.Tests/Defense/SiegeSystemTests.cs`, `SiegeDoctrineTests.cs`, `SiegeEndingsTests.cs`, `Ashfall.Core.Tests/Save/SiegeSaveTests.cs`; extend defense-raid tests

## 5. Packages

### LS-P0 — Premise audit (Auditor; read-only)
- Close E8, E11, E12, E13: expedition start gate options; real morale/sleep read API; how a besieger identity is authored and where aggression is fed; difficulty hook; list every reader of `DefenseSystemState`; confirm the raid resolver is not called from any other path; foreman signs DEC-LS-01…10.
- **Accept:** each VERIFY answered with `path:line` or a test; dispatch-gate option chosen.

### LS-P1 — Siege model (Core, pure + nested DTO)
- `SiegeState { siegeId, doctrineId, besiegerId, startDay, day, supply, resolve, patience, phase, campNodeId?, runnerIds[] }`; validation; nested additive in `settlement_defenses`, default null.
- **Accept:** round-trip; old saves load; no doctrine → no siege; state holds no defender values.

### LS-P2 — Doctrines & pressures (data + Core)
- `siege_doctrines.json` (weights over five pressures, patience, temper), `siege_pressures.json` (cost/effect tables).
- **Accept:** validator registration; every pressure names the existing surface it touches; weights sum-checked.

### LS-P3 — Daily tick (Core + host)
- One seeded pressure per day (`CampaignStreamIds` fork keyed `(day, siegeId)`): Probe → the existing resolver at seeded low strength; Starve → the E8 dispatch gate flag; Noise → a read of Watch acoustics + a bounded morale/sleep pressure through the real owner; Parley → offer at the gate; Sap → a *Deep Works* signal (soft, dark if absent).
- **Accept:** exactly one resolver call per day at most; same seed → same sequence; resolver output unchanged.

### LS-P4 — Defender verbs (Core + host)
- Repair/reset (existing commands, priced), **Siege Table** preset (with *Ration Wars* if present), **runner** (seeded outcomes), **sally** (party if coordinator exists, else one volunteer), **hold**.
- **Accept:** each verb changes only the clock it names; runner/volunteer outcomes are seeded; survivor conservation across outcomes.

### LS-P5 — Nerve & stores reads (Core, read-only)
- A `SiegeReadout` projects stores, water (deep well + sump), wire, morale/sleep — never stored.
- **Accept:** readout equals owners' values in a scripted week; no writes.

### LS-P6 — Endings (Core + host)
- Lifted, Relieved, Negotiated, Ground down, Fallen: each routes through existing owners (treaty/debt for Negotiated; prisoner/loot for Lifted; existing breach consequences for Fallen).
- **Accept:** each ending reachable in a test; Fallen requires a long visible run (threshold from data); no ending writes to an owner it does not name.

### LS-P7 — Presentation
- Siege Board strip in `DefenseGridPanel` and `NightWatchPanel`; focus/back preserved.
- **Accept:** presenter tests; panels hold no authority.

### LS-P8 — Cross-plan hooks
- Siege Table (RW), sap/countermine (DW), captured runner (QW), sally (CC), sealed gate (PY), road flag (LR) — all ship dark until both ends exist.
- **Accept:** each hook a no-op when the other plan is absent; one test per hook shape.

### LS-P9 — Content waves W1–W4 & governance close.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Raid parity: with no doctrine rows, Iron Raiders raids resolve identically on a saved corpus.
3. At most one resolver call per siege day.
4. Determinism: same seed → same pressures, runner outcomes and endings (`CampaignStreamIds` fork; never `System.Random`).
5. Save round-trip mid-siege with a runner out and a pending parley.
6. Defender values are read, never stored, never written except through the owner's public commands.
7. Zero writes to Watch/perimeter state.

## 7. Cross-plan boundaries
- **The Watch (Exp. 36):** read-only.
- **The Ration Wars:** Siege Table preset.
- **The Deep Works:** sap/countermine — the Siege owns the action; the Works owns the drift and the collapse.
- **The Quiet War:** captured runners → QW gate/informant systems.
- **The Plague Year:** a sealed gate is a Gate Protocol; the Siege never forces it.
- **The Living Region:** road cuts read as a flag.
- **Crews and Companions:** sallies are parties.
- **Year Two:** outposts belong to Year Two; *Relieved* consumes its supply flag.
- **Shelter Governance:** terms are Assembly business if present.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-LS-01 | A siege starts from a repelled raid by authored doctrine only; the raid source is unchanged. | rule | Yes |
| DEC-LS-02 | Siege state nests in the `settlement_defenses` DTO; no new section. | architecture | Yes; confirm in P0 |
| DEC-LS-03 | At most one resolver call and one pressure per day. | rule | Yes |
| DEC-LS-04 | Road cutting via an additive dispatch predicate in the expedition host. | architecture | Choose in P0 |
| DEC-LS-05 | Defender clocks are read-only projections. | architecture | Yes |
| DEC-LS-06 | Five endings; Fallen must be earned by a visible run of days. | design | Yes |
| DEC-LS-07 | No new routed panel; extend `DefenseGridPanel` and `NightWatchPanel`. | UI | Yes |
| DEC-LS-08 | Three starter doctrines (Toll, Quiet Ring, Diggers). | scope | Yes |
| DEC-LS-09 | Runner outcomes may leak information to QW only through its public seam. | boundary | Yes |
| DEC-LS-10 | Difficulty scales doctrine patience and probe strength, never the resolver. | rule | Yes; depends on E13 |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Siege`, `Besieg`, `Doctrine`, `Patience`)
- [ ] Premise re-verified (Rule 7); ledger files re-read; no overlapping live claim on defense/muster/watch paths
- [ ] Signed decisions in hand

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing defense, perimeter, iron-raiders and night-watch tests (list from P0 selector)
- [ ] `--defense-selftest` and night-watch selftests (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: a siege would need to change resolver arithmetic or Watch state; no dispatch-gate option exists that avoids editing `ExpeditionSystem` logic; morale/sleep have no public read; the besieger cannot be authored without a new faction system; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered** — not gaps, not TODOs, not deferred work. They
keep the siege larger than the board that meters it. Any future plan that answers one must name the
signed decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| LS-OM-1 | Why do sieges begin only after a *repelled* raid? | DEC-LS-01 is a rule and a piece of characterisation: the besieger has already learned something about you. The rule is not explained and should not be. | Never — locked by decision. |
| LS-OM-2 | What are the Diggers digging for? | DEC-LS-08 names a doctrine; sap is a *Deep Works* signal and nothing more. Intent is deliberately unauthored. | *The Deep Works*, jointly, if countermine is ever authored. |
| LS-OM-3 | Why do the terms sound reasonable? | `siege_terms.json` supplies terms; LS-P6 routes Negotiated through treaty/debt owners. Whether the offer is sincere is not modelled. | Never — a rule, not a gap. |
| LS-OM-4 | Did the runner talk? | `siege_runner_outcomes.json` is seeded; DEC-LS-09 hands any leak to *The Quiet War* through its public seam only. | The Quiet War, at its discretion. |
| LS-OM-5 | What is "patience", measured in? | LS-P1 stores it as state alongside supply and resolve. Its unit is never defined and must not be. | Never — texture by omission. |
| LS-OM-6 | Who keeps the doctrine rows? | Three starter doctrines are authored (DEC-LS-08). Their authorship, and why they are exactly three, is unrecorded. | Never — the artefact reads as canon. |
