# Plan 43 — Governing Together: The Shelter Decides

> **Wave:** Continuity Wave 6 — *The People In It*
> **Depends on:** 40A (who believes what), 24A/24B (fitness + needs stack as the effect channel),
> 31 (decisions and their consequences must be reportable), 38C (deadlines give policy a spine).
> **Coordination:** parallel `Plan_159_Shelter_Governance_Political_System` designs a governance
> *system*. This plan deliberately adds **no new system**: it wires the leadership, schedule, ration,
> register, and arbitration machinery that already exists, and hands 159 a working substrate.
>
> **Theme:** `LeadershipSystem` models a designated leader, leader stress, break risk, deaths
> witnessed, and crisis pressure — and **no non-test code calls `DesignateLeader` or
> `OnCrisisEvent`.** The player cannot appoint a leader, cannot see one, and cannot be deposed.
> Meanwhile the canon registry recommends "Add election and mutiny mechanics", the trade screen
> prints a fabricated `"Leader: Varek (gen 1)"`, and the only real governance verbs in the game are
> two panel toggles: curfew and emergency override.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | Leadership is fully modelled | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` — `IsDesignatedLeader` (`:85`), `GetLeaderStress` (`:90`), `GetDeathsWitnessed` (`:96`), `DesignateLeader` (`:103`), `StepDown` (`:129`), `OnSurvivorDied` (`:145`), `OnSurvivorInjured` (`:182`), `OnCrisisEvent` (`:202`), `Tick` (`:213`), events `OnLeaderDesignated`/`OnLeaderSteppedDown`/`OnLeaderStressIncreased`/`OnLeaderBreakRisk` (`:45–48`) |
| 2 | **Nothing in the game designates or informs a leader** | `grep -rn "DesignateLeader\|OnCrisisEvent" src/` excluding selftests/panel-tests/UI-tests → **0**; `grep -rniE "leader" src/` returns faction-leader UI labels only, and `src/Economy/TradeScreenGodotPanel.cs:164` — `Text = "Leader: Varek (gen 1)"`, a hardcoded string |
| 3 | Leadership's only live wiring is morale | `SurvivorSocialCoordinator.cs:113–119` binds `Leadership.ApplyMoraleDelta`/`ApplyShelterMoraleDelta` to `_needs.Modify(…, Morale, …)` — so stress can move morale, but nothing ever raises it |
| 4 | Two governance verbs do reach the player | `src/UI/ShelterSchedulePanel.cs:72,83` toggle `curfewActive` and `emergencyOverride` on `ShelterScheduleSystem` (`:10–19`: `fatigueRecoveryModifier`, `lightingDemand`, `SleepAssignment`) — real decisions with real state, but `fatigueRecoveryModifier` is display-only (Wave 2's gap #8, fixed by 24B) |
| 5 | Ration policy is a decision that exists | `src/Host/StartingLevelHostSession.cs:37 ResolveMorningRationTriage(RationPolicy policy)`; the policy feeds `SurvivorSocialCoordinator.RationPolicy` (`Main.SurvivorSocial.cs:34`, `TickSurvivorSocial:111–113`) → `RationConflictSystem` → morale — the **shape** every policy should copy |
| 6 | No voting, council, election, or mutiny anywhere | `grep -rniE "mutiny\|election\|vote\|ballot\|council" Assets/Ashfall.Core src/` → only unrelated matches (`TradeTellEngine` "tell selection", `TradeSelectionSnapshot`, encounter "selection") — the registry's recommendation is unimplemented, and correctly so: see guardrails |
| 7 | The consent/bureaucracy layer exists and is unwired to each other | `VoluntaryRegisterSystem.cs`, `CensusClaimSystem.cs`, `CensusHeadlessDemo.cs`, `CrossingArbitrationSystem.cs` (+ `CrossingArbitrationHeadlessDemo`), `RegionalTreatySystem.cs` — registers, claims, and arbitration exist as Core capabilities and as separate host slices, never as one governance surface |
| 8 | Ideological friction has data and no voice | `IdeologicalFrictionSystem.cs:30–31` (`ConflictAffinityDrainPerDay = 2f`, `SynergyAffinityGainPerDay = 1f`), `:59 RegisterBelief` (fed from invented data pre-40A), and no consumer of affinity outside the social systems (Plan 44's finding) |
| 9 | The grievance channel already exists | `RationConflictSystem` → `OnMoraleDelta` (coordinator `:41–46`); `DutyRoster/MoraleMarkSystem.cs` + 43 authored marks with inspect/bark sentences — a vocabulary for "the crew is unhappy about X" is present and only needs a decision source |
| 10 | Fake surfaces occupy the slots | `TraumaBondingCohortPanel`, `caregiving`, `apprenticeship`, `shelter_schedule`, `standing_record` are routed panels; `TraumaBondingCohortPanel` is one of Wave 1's 30 unbacked consoles (`16A`) — governance work must not add a 31st |
| 11 | Ending machinery wants policy facts | `EpilogueMatrixRuntime` branches on `grandTreatySigned`, `debtLedgersBurned` (Wave 1's 19A) — how the shelter decided those things is currently unrecorded |

---

## Task 43A — A leader, appointed and visible: wire the leadership system

**Goal:** designation, stress, crisis, and step-down run in the campaign, and the player can see and
cause them — using the coordinator's already-bound morale channel for every effect.

**Files:** `LeadershipSystem.cs`, `SurvivorSocialCoordinator.cs:113–124`,
`src/Main.SurvivorSocial.cs`, `src/Main.CampaignOwners.cs` (`survivor_social` owner),
`src/UI/SurvivorDetailPanel.cs` / `SurvivorRelationsPanel.cs` / `GameDashboardPanel` overview,
`StatusPanel.cs`, `EpilogueContextFactory` (Wave 1's 19A), `docs/systems/LEADERSHIP.md`,
`Ashfall.Core.Tests/LeadershipWiringTests.cs`.

### Substeps

1. **Expose designation as a player action** on the existing roster/survivor surface — an appoint
   action that calls `DesignateLeader` (validation + reason strings, `ActionResult` style), plus
   `StepDown`. No new panel: `survivor_detail` and `survivors` routes already exist.
2. **Feed the crisis channel**: wherever the game already signals a crisis (`survivor_perished`,
   `hazard_warning`, `duty_vacated`, brownout/breach events from 23B/30B), notify
   `OnCrisisEvent`/`OnSurvivorDied`/`OnSurvivorInjured` **once** per event — 16C's delegate-identity
   discipline applies or the leader accrues stress per panel reopen.
3. **Tick it**: `Leadership.Tick(gameHours)` is driven through `SurvivorSocialCoordinator.TickDay`
   (already registered as the `survivor_social` day owner at `Main.CampaignOwners.cs:34`) — no new
   owner needed.
4. **Make stress legible before it is consequential**: `GetLeaderStress`,
   `OnLeaderBreakRisk`, and `GetDeathsWitnessed` surface in the overview + leader's detail panel,
   with keyed text (25A) and words as well as a bar.
5. **Make it consequential through the one existing channel**: stress → shelter morale delta
   (already bound at `:113–119`), break risk → a decision point for the player (confirm leadership,
   step someone down, or lose the shift), and no second morale mechanism.
6. **Belief matters**: post-40A, the leader's `belief_profile_id` should modulate friction pairings
   (a leader whose profile clashes gains no synergy) — read from `IdeologicalFrictionSystem`, not
   hardcoded pairings.
7. **Succession when they die**: on leader death, designate-by-situation (highest standing/
   leadership-skill survivor) with an explicit player confirmation — and record the gap in the
   standing record (41B).
8. **Voice**: the leader speaks for the shelter in policy announcements (42C step 6) — this is the
   cheapest way for authority to be felt rather than read.
9. **Fix the fabricated faction leader**: `TradeScreenGodotPanel.cs:164` must bind the faction's
   authored leader (via `characters.json`/faction catalog, which `CrossingCatalog.cs:350` already
   validates against) — a hardcoded "Varek (gen 1)" is Wave 1's BUG-UI-002 pattern in a live panel.
10. **Record the decision for the ending**: leader identity + how leadership ended becomes an
    `EpilogueEvaluationContext` input alongside 19A's five facts, so "who steered this" is part of
    what the ending reads.
11. **Persistence**: leadership state lives in the survivor-social section (already saved); assert
    the round-trip restores designation, stress, and witnessed-death counts.
12. **Tests**: appoint/step-down validation, crisis→stress accrual exactly once per event,
    stress→morale, break-risk decision, succession, faction leader from data, save round-trip,
    determinism of a 100-day crisis cadence.
13. **Run the checklist** + `--survivors-selftest` + `triad-drift-gate.sh`.

**DoD:** the shelter has a leader the player chose, who can be worn down by what the player does.

---

## Task 43B — Policy as a decision with a consequence, once

**Goal:** one policy mechanism (propose → decide → apply → register grievance → report), and ration
policy + curfew + emergency override expressed through it — so a third policy, and a fourth, and a
fifth cost nothing to add.

**Files:** new `Assets/Ashfall.Core/Governance/PolicySystem.cs` (thin: catalogue + current +
proposer + grievance hooks), `ShelterScheduleSystem.cs` (curfew/override),
`StartingLevelSystem` ration policy, `RationConflictSystem.cs`, `ShelterSchedulePanel.cs:72,83`,
`src/Host/StartingLevelHostSession.cs:37`, policies data JSON, 31 (events), 42C (voice), 24B (needs
modifiers).

### Substeps

1. **Model the pattern that already works**: morning ration triage — a choice, a policy value, a
   downstream grievance system, a morale channel. Generalise *that*, don't invent a new
   political-simulation layer.
2. **Author the policy catalogue in data**: id, scope (rations/curfew/work rhythm/quarantine/
   mourning/funerals), options, per-option effects expressed as *declarations* (needs modifier,
   morale band, fatigue recovery, friction multiplier), who can propose, and how it's decided.
   No effect math in C#.
3. **Effects flow through the existing stacks** — `NeedsModifierStack` (24B), ration conflict
   (morale), schedule (`fatigueRecoveryModifier`, now made real by 24B) — so policy changes the same
   numbers the rest of the game reads, and 31 can attribute them (`policy_adopted`, `grievance_raised`).
4. **Decisions must cost attention**: adopting a policy consumes a duty slot/day or a council moment
   (43C), so policies are chosen, not toggled.
5. **Grievances are the feedback**: each policy option declares who is likely to object
   (belief/profession/age class), feeding `RationConflictSystem`-style grievance records and 42C
   lines — the crew's reaction *is* the policy's difficulty.
6. **Reversibility with a price**: changing policy back should be possible and should cost morale or
   trust (flip-flopping leadership is a genre truth), authored as data, not as a hard block.
7. **Migrate the existing toggles** to the mechanism (curfew and emergency override currently write
   state directly from panel lambdas) — behaviour preserved, one equivalence test each.
8. **Register every policy decision** in the standing record + journal so the shelter's history is
   queryable (41B) and the ending can cite it (19A: how a treaty or ledger decision was *made*, not
   merely that it was).
9. **No tyranny of the modal**: a policy prompt must never interrupt a crisis (42B's attention
   budget), and must be resolvable from the briefing/journal later.
10. **Accessibility + keys**: policy text through 25A; options must be readable without colour, and
    keyboard-operable (37B).
11. **Tests**: catalogue resolution, per-option effect equivalence to today's behaviour for the two
    migrated toggles, grievance generation, cost enforcement, reversal price, save round-trip,
    determinism.
12. **Docs**: `docs/systems/POLICIES.md` with the table of authored policies and their channels.
13. **Run the checklist** + `--data-integrity-selftest`.

**DoD:** policy is one mechanism with authored options, real effects, and audible objections.

---

## Task 43C — Consent, coercion, and the crew's limit: standing, registers, and the mutiny question

**Goal:** give accumulated grievance a terminal state — depose, refuse, desert, or mutiny — using the
arbitration, register, and census systems that already exist, and *only* if the substrate from 43A/43B
supports it. This task may legitimately end in "not yet, here's why".

**Files:** `CrossingArbitrationSystem.cs`, `VoluntaryRegisterSystem.cs`, `CensusClaimSystem.cs`,
`RegionalTreatySystem.cs`, `IdeologicalFrictionSystem.cs`, `RationConflictSystem.cs`,
`SurvivorFateSystem.cs` (desertion/fate), `LeadershipSystem.cs`, `UtilityAiSystem`
(`src/UtilityAI/` is a panel only; Core owns the system), new `docs/systems/CREW_CONSENT.md`.

### Substeps

1. **Gate this task on a decision record**: if grievance, policy, and leadership state are not yet
   persisted and readable, **stop here** and hand the gap list to Wave 7. A mutiny mechanic without a
   grievance substrate is theatre.
2. **Define the escalation ladder as data**, not code: quiet discontent → refusal (a shift not
   worked) → organised grievance → open challenge → departure/defection. Each rung: trigger, effect,
   player-readable signal (42C), and an off-ramp — the same "no path without an exit" rule as 23C.
3. **Refusal first, violence last**: a crew that has had enough should *stop working* before anyone
   gets hurt. `DutyRosterSystem` and the fitness/labour channels (24A/35C) are where that lands.
4. **Use the arbitration machinery for disputes**: `CrossingArbitrationSystem` exists with a headless
   demo and a `faction_`/`npc_` id surface validated by `CrossingCatalog.cs:350` — internal disputes
   should be resolved through a documented arbitration path (with stakes), not a bespoke dialog.
5. **Registers and census as consent instruments**: `VoluntaryRegisterSystem` +
   `CensusClaimSystem` + `CensusHeadlessDemo` are already live inside the dose session — make them the
   mechanism for who is counted, who is bound to what duty, and who may leave. That is governance
   expressed as paperwork, which is exactly this game's texture.
6. **Desertion is a fate event, not a leak**: a departing survivor goes through
   `SurvivorFateSystem` (fate record, memorial-not-held, keepsake left behind, rations recalculated,
   grievance ripple) so departure has the same weight as death.
7. **Leadership challenge uses 43A**: `StepDown`/`DesignateLeader` are the verbs; the challenge is a
   decision point with outcomes, never a QTE.
8. **Belief and ideology are the fuel**: friction pairings (40A beliefs) determine who joins whom —
   never hardcode factions inside the shelter.
9. **Every rung must be traceable**: briefing lines, journal entries, and standing-record updates —
   so after the fact the player can read the sequence that ended their leadership.
10. **Tone is the design constraint**: no coup montage, no villain monologue. Cold, tired, human —
    someone says they're not going, and the player decides what that costs.
11. **Tests**: ladder transitions with off-ramps, refusal removing labour from production (35C),
    arbitration outcomes, register-driven departure, fate records for desertion, save round-trip
    mid-escalation, determinism, and a negative test: no rung fires without its authored trigger.
12. **Run the checklist** + `ashfall-expansion-qa-playthrough` (ladder reachability in a seeded run).

**DoD:** the crew can say no — legibly, in stages, with consequences the player can trace — or this
task ships the gate document explaining exactly why it waits.

---

## Cross-Task Dependencies

```
40A (beliefs) ──► 43A step 6, 43C step 8      31A (kinds) ──► 43A step 2, 43B step 3
24A (fitness) ──► 43B step 3, 43C step 3      24B (needs stack) ──► 43A step 5, 43B step 3
42C (voice/grievance) ──► 43B step 5, 43C step 2,9
41B (records) ──► 43B step 8, 43C step 9      38C (commitments) ──► 43B policies meet deadlines
19A (derived ending) ◄── 43A step 10, 43B step 8
   43A (leader) ──► 43B (policy) ──► 43C (consent/coercion) — deliberately sequential
```

**Execution order:** 43A → 43B → 43C. 43C's step 1 is a real stop condition, and honouring it is
part of the plan's success criteria, not a failure.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. python3 scripts/ci/generate-port-contract.py --check          # leadership/policy sinks bound
7. bash scripts/ci/triad-drift-gate.sh                          # social section + policy state
8. ashfall-expansion-qa-playthrough / seeded soak                # ladder + policy reachability
9. ashfall-ui-access + ashfall-snapshot-diff (schedule/roster/detail panels)
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | UI | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 43A | 0 (wire) +1 test set | 4 | 0 | 3 | 10–13 | Low–Med | LOW (the system already exists) |
| 43B | 1 thin | 3 | 1 new | 2 | 10–14 | Medium | MEDIUM (morale economy is sensitive) |
| 43C | 1–2 | 3 | 1 | 2 | 12–16 | **High** | MEDIUM — and it may legitimately defer |

**Guardrails:** no new political-simulation system, no approval-rating meter, no faction-simulation
inside the shelter, no QTE or cinematic coup, no voting UI before grievances are legible, no 31st
fake console, and no mechanic that punishes the player for information they were never given —
the crew's consent must be *readable* before it can be lost.
