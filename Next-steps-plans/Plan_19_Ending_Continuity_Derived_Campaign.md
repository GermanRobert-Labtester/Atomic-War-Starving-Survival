# Plan 19 — Ending Continuity: The Campaign Must Compute Its Own Ending

> **Wave:** Continuity Wave 1 (closing plan)
> **Depends on:** 15A/15B (choices must exist to be counted), 18B (state must be real to be read).
> **Task 19A is the single highest-severity finding in this wave.**
>
> **Theme:** everything the player does for 200 days is discarded at the epilogue. The ending's
> five input booleans are hardcoded `true`, deaths hardcoded `0`, and the whole 32-permutation
> matrix therefore always prints the same result. On top of that, the game's only *multi-year*
> continuity (children, memorials, generational succession) feeds nothing during play, and the
> documents that tell the next agent what is true have themselves drifted.

---

## Evidence Inventory (re-verified @ `ccac926e`)

### 🔴 19A — The ending is not derived from the game

| Fact | Evidence |
|---|---|
| At game over, the epilogue is bound with literal constants | `src/Main.GameFlow.cs:444` — `_epiloguePanel.Bind(_simDay, _survivors?.RosterState?.Count ?? 4, **0**, **true**, **true**, **true**, **true**, **true**)` (deaths, treaty, tempest, ledgers, children, vel) |
| The player-facing route does the same | `src/Main.PlayerSurfaces.cs:246` — identical `0, true, true, true, true, true` |
| The panel only forwards those numbers | `src/UI/EpiloguePanel.cs:43–65` — `Bind(...)` writes them straight into `EpilogueEvaluationContext`, then `RefreshView()` → `_runtime.GenerateEpilogueNarrative(_context)` |
| **No gameplay path anywhere derives these inputs** | `grep -rn "EpilogueEvaluationContext" src/ Assets/Ashfall.Core` → only `ExpansionHostSession.cs:416` (`EvaluateEpilogueDemo(int,int,int,bool,bool,bool,bool)` — a **demo** taking booleans as arguments), `HostCli.PanelTests.cs` (5 synthetic fixtures), and the panel itself |
| The inputs are *decidable* — the authorities exist | `grandTreatySigned` → `Assets/Ashfall.Core/RegionalTreatySystem.cs`, `regional_treaty` + `aquifer_treaty_concession` routes; `debtLedgersBurned` → `Assets/Ashfall.Core/LedgerDebtSystem.cs`; `childrenSurvived` → `Assets/Ashfall.Core/CohortSystem.cs` (host: `src/Host/DoseLedgerHostSession.cs:45`); `tempestDecommissioned` → **already derived** for the Verdict expansion: `Assets/Ashfall.Core/Verdict/VerdictEndingEvaluator.cs:64 IsTempestDecommissioned(ReckoningState)`; `velSecretExposed` → `Verdict/EvidenceLedger.cs`, `ReckoningSystem.cs`; deaths → `Assets/Ashfall.Core/Memorial/` + `Survivors/NeedsSystem.cs:79 OnDied` (subscribed at `src/Host/SurvivorsHostSession.cs:111`) |
| The matrix genuinely branches on them | `EpilogueMatrixRuntime.cs:35,38,41,44,55,68,71` — e.g. `if (!ctx.tempestDecommissioned && ctx.totalDeathsRecorded > 50)`, `if (ctx.livingDwellerCount >= 8 && ctx.childrenSurvived)`, `if (ctx.debtLedgersBurned && ctx.childrenSurvived)`, `if (!ctx.debtLedgersBurned)` |
| Consequence | `totalDeathsRecorded` is **always 0**, so the `> 50 deaths` branch is unreachable; `debtLedgersBurned` is always true, so the `!burned` branches are unreachable; the game prints one epilogue forever |
| An orphaned sidecar proves a factory was lost | `Assets/Ashfall.Core/Endgame/EpilogueContextFactory.cs.uid` exists with **no** `EpilogueContextFactory.cs` and 0 references — the derivation layer was intended and is gone |

**Reading:** this is not polish. A survival-management game is judged by whether the ending
remembers you. Right now it cannot. The fix is one builder + eight reads, not a new system.

### 19B — Multi-year continuity state that changes nothing

| Fact | Evidence |
|---|---|
| Children are simulation-visible only in a demo | `CohortSystem` is constructed only as a child of the dose ledger session (`src/Host/DoseLedgerHostSession.cs:45`); its only UI read is `src/Dose/DoseRegisterSurface.cs:240–319`, and the mutation call uses the literal id `"sv_cohort_demo"` |
| The atlas already classifies it | `docs/ASHFALL_EXPANSION_CONTEXT_ATLAS.md` §11: *Child Maturation Baseline — Written by `CohortSystem`, Read by `CohortSystem`, Player Visibility **Low**, classification **"Orphan State (Underconnected)"*** |
| Location memory is the same pattern | same table: *Location Strata Inscriptions — Visibility Low (Inspect popups), **"Hidden State (Weak Feedback)"*** |
| The ending already wants this state | `EpilogueMatrixRuntime.cs:55,68` branch on `childrenSurvived` |

### 19C — Truth drift in the artefacts agents read

| Fact | Evidence |
|---|---|
| 15 dangling Godot sidecars | `find src Assets -name "*.cs.uid" \| [no sibling .cs]` → **15**, incl. `Endgame/EpilogueContextFactory.cs`, `Flags/FlagId.cs`, `Shelter/OrbitalHarrowSystem.cs`, `Survivors/VigilStateMachine.cs`, `Host/CampaignServices.cs`, `AtomicFileWriter.cs`, `SaveEnvelopeDetection.cs`, `Clock/DayCycle.cs` (several with 0 references → the class is gone) |
| Data audit is stale enough to mislead | `docs/data/DATA_GAP_AUDIT.md:17` still lists `questline_master.json` as "**ORPHAN** — no C# loader"; `src/Main.Application.cs:392` + `src/Main.cs:42` prove it loaded (362 defs, `QuestlineSystem` consumer) |
| Audio audit is stale on a Core gap | `SILENCE_AUDIT.md` §4.7 "no death event… this is a Core gap"; `NeedsSystem.cs:79` has `public event Action<SurvivorNeedsState>? OnDied` and the host subscribes at `src/Host/SurvivorsHostSession.cs:111` |
| Canon registry carries a false defect | §26.3 asserts `AirlockSecuritySystem.cs:80` GetHashCode nondeterminism; `grep GetHashCode Assets/Ashfall.Core/AirlockSecuritySystem.cs` → **no match** |
| Plan-14 premiss is stale | `Next-steps-plans/Plan_14_…md` claims the weather forecast "never surfaced"; `src/UI/WeatherForecastPanel.cs` exists and is routed (`weather_forecast`) |
| Working tree is large and uncommitted | `git status --porcelain \| wc -l` → **95** at `ccac926e` (branch `main`) |
| A 143 MB duplicate tree sits in the worktree | `.claude/worktrees/plan06-narrative` (143M) is excluded only via `.git/info/exclude`, **not** `.gitignore` — invisible to other agents/CI clones, and it poisons every repo-wide grep with duplicate "source" hits (observed repeatedly during this audit) |

---

## Task 19A — Derive the ending from the campaign

**Goal:** `EpilogueEvaluationContext` becomes a *projection of saved state*, computed once, in
Core, at game over. Until this lands, no ending-related content work is worth doing.

**Files:** new `Assets/Ashfall.Core/Endgame/EpilogueContextFactory.cs` (recreate the lost file),
`src/Main.GameFlow.cs:444`, `src/Main.PlayerSurfaces.cs:246`, `src/UI/EpiloguePanel.cs`,
`src/Host/ExpansionHostSession.cs`, plus read-only touches into `RegionalTreatySystem`,
`LedgerDebtSystem`, `CohortSystem`, `Memorial/`, `Verdict/*`, `IFlagLedger`.

### Substeps

1. **Write the failing test first** (it is a one-line proof of the whole bug): assert that two
   campaigns with different recorded states produce different `EpilogueEvaluationContext` values.
   It must fail today, identically, for both the panel route and the game-over route.
2. **Author `EpilogueContextInputs`** in Core — a plain DTO carrying only what is needed:
   `days, livingDwellers, deathsRecorded, grandTreatySigned, tempestDecommissioned,
   debtLedgersBurned, childrenSurvived, velSecretExposed`, plus `sourceIds[]` for traceability.
   No engine refs, no Godot types (Invariant 1).
3. **Author `EpilogueContextFactory.Build(inputs)`** → `EpilogueEvaluationContext`. This is the
   seam that keeps the *decision about what counts* in Core, where it can be tested.
4. **Resolve each fact from its authority**, one substep each, in this order of confidence:
   - `deathsRecorded` — count from the memorial/`OnDied` ledger, never from a literal `0`.
   - `tempestDecommissioned` — reuse the existing `VerdictEndingEvaluator.IsTempestDecommissioned(ReckoningState)`; do **not** write a second rule (that would fork the definition of the ending between the base game and the expansion).
   - `grandTreatySigned` — read `RegionalTreatySystem` state / the `regional_treaty` +
     `aquifer_treaty_concession` facts.
   - `debtLedgersBurned` — read `LedgerDebtSystem` (foreclosure/settlement state).
   - `childrenSurvived` — read `CohortSystem` (19B deepens this; here, just read it truthfully).
   - `velSecretExposed` — read `EvidenceLedger` / `ReckoningSystem`.
5. **Where a fact has no authority, add a flag read** from `IFlagLedger` using a sanctioned
   `flag_` id from the data authority rather than inventing a bool. Add the id to the catalog so
   `CatalogIntegrityValidator` gates it (Invariant 6, snake_case ids).
6. **Replace the hardcoded binds**: `src/Main.GameFlow.cs:444` and
   `src/Main.PlayerSurfaces.cs:246` must both call one host accessor that returns the built
   context. Two call sites, one derivation — otherwise the panel route and the game-over route
   drift again (that duplication is how this bug survived).
7. **Narrow the panel signature**: change `EpiloguePanel.Bind(8 positional args)` to
   `Bind(EpilogueEvaluationContext)` — positional booleans are exactly how `true,true,true,true,true`
   went unnoticed. Add a `Bind` overload only if other call sites make that impractical.
8. **Make the inputs visible *during* play, not only at the end**: each of the five facts gets one
   line in the existing briefing/journal feed when it flips (17A's event vocabulary:
   `treaty_signed`, `ledger_burned`, `tempest_decommissioned`, `child_born`, `child_lost`,
   `secret_exposed`). An ending condition the player never learns about is not a choice.
9. **Reachability proof**: a test per branch in `EpilogueMatrixRuntime` showing the branch is
   reachable from some campaign state — the `!debtLedgersBurned` and `totalDeathsRecorded > 50`
   paths that are currently dead must now be attainable.
10. **Retire the demo path**: `ExpansionHostSession.EvaluateEpilogueDemo(...)` takes booleans;
    keep it only under an explicit `*Demo` verb used by CLI selftests, or delete it. A boolean-
    taking API adjacent to the real one is how the mistake gets re-made.
11. **Save continuity**: assert the derived context survives save → load → game over identically
    (the inputs must come from persisted sections, not in-memory-only state). If any fact is not
    persisted, that is a *section* gap — fix the section, not the projection.
12. **Determinism**: same seed + same choices ⇒ identical ending text (paired replay test).
13. **Delete `EpilogueContextFactory.cs.uid`'s orphan status** by landing the real file; sweep the
    other 14 dangling sidecars in 19C.
14. **Verify** against a long run: use `ashfall-telemetry-playtest` / a scripted 200-day seeded
    campaign and print the ending for three different choice policies. They must differ. Paste the
    three endings into the task log as the acceptance evidence.
15. **Run the five-step verification checklist.**

**DoD:** three play styles produce three different endings; `totalDeathsRecorded` matches the
memorial; no ending input is a literal anywhere in `src/`.

---

## Task 19B — Make the years mean something: cohort, memorials, and legacy

**Goal:** close the two "underconnected / weak feedback" state entries in the atlas, so a
multi-year campaign feels like the same holdfast ageing rather than a fresh bunker with a bigger
day counter.

**Files:** `Assets/Ashfall.Core/CohortSystem.cs`, `Regional/Generational*`,
`Assets/Ashfall.Core/Memorial/`, `src/Host/DoseLedgerHostSession.cs`,
`src/Dose/DoseRegisterSurface.cs`, `src/UI/` surfaces (greenhouse, kitchen_nutrition,
apprenticeship, library_study, school-adjacent panels that already exist), day-advance owners.

### Substeps

1. **Enumerate what `CohortSystem` actually offers today** (children list, ageing, maturation
   baseline, `CorrectBaseline`) and write the list in the plan log — the design must fit the
   system, not the summary.
2. **Pick exactly three links, no more** (this is the whole task's discipline):
   (a) mouths to feed, (b) a schoolable/apprenticeable population, (c) the ending's
   `childrenSurvived`.
3. **Rations:** include cohort children in the shelter ration allocation consumed by
   `starting_level_rations` / needs, at a child fraction defined in data (not code), so the
   hunger curve of a growing holdfast is real.
4. **Apprenticeship/schooling:** feed cohort size into the existing
   `ApprenticeshipSystem`/`library_study`/knowledge progression as an eligibility and capacity
   input — these systems already exist; children become their population.
5. **Work shifts:** cohort maturation must gate *when* a child can take a duty-roster shift;
   reuse `DutyRosterSystem` rather than adding a labour rule.
6. **Remove the demo literal**: `src/Dose/DoseRegisterSurface.cs:319` calls
   `CorrectBaseline("sv_cohort_demo", "high")`; drive the survivor id from the live selection, and
   move any genuinely demo-only flow into a `--*selftest` verb.
7. **Memorials:** ensure every recorded death produces a memorial entry whose text varies by the
   circumstances that already exist (`DeathQuality`, `MemorialOutcome`, epitaph and wall-carving
   catalogs from Plan 09/Plan 65–69 work) — the death must be remembered *differently*, and its
   count is the ending input from 19A step 4.
8. **Surface the passage of time**: one visible "this year" marker in the existing status/shelter
   surface (children ages, generation count, memorial wall growth) so ageing is perceived, not
   just stored. Reuse existing panels; no new console.
9. **Emit day events** for `child_born`, `child_aged`, `child_lost`, `generation_advanced`
   (17A vocabulary) and link them to the briefing.
10. **Persistence**: cohort + memorial changes must land in their existing sections
    (`SaveSectionRegistry`) and pass round-trip tests; if cohort state rides inside the dose
    section, prove the coupling is deliberate and documented.
11. **Tests**: ration arithmetic with N children, apprenticeship eligibility by age, duty-roster
    gating, memorial text variance, ending-flag derivation from cohort state, and save round-trip
    for all of it.
12. **Balance check**: `ashfall-balance-sim` over a 3-year seeded campaign — children must be a
    real cost and a real future, not free population. Record the food-per-capita curve.
13. **Update the atlas §11 table**: change *Child Maturation Baseline* from
    "Orphan State (Underconnected)" to the new real read-set, and the same for
    *Location Strata Inscriptions* if 19B step 8 touches it. The atlas is only useful if it is
    re-checked.
14. **Run the checklist.**

**DoD:** children eat, learn, work at the right age, are mourned specifically, and decide an
ending branch.

---

## Task 19C — Keep the record honest: session continuity, sidecars, and document truth

**Goal:** make the repo's own statements about itself trustworthy, so the next agent (or the next
plan) cannot repeat this wave's findings by accident. Continuity of *knowledge* is continuity of
the game.

**Files:** `.gitignore`, `project.godot` (only if a reimport is needed),
`docs/data/DATA_GAP_AUDIT.md`, `docs/audio/SILENCE_AUDIT.md`,
`docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` §26, `docs/CURRENT_AUTHORITY.md`,
`docs/GODOT_MIGRATION_STATUS.md`, new `scripts/ci/uid-sidecar-gate.sh`,
`Ashfall.Core.Tests/SidecarHygieneTests.cs` (or a shell gate), the 15 `.uid` files.

### Substeps

1. **Sweep the 15 dangling `.cs.uid` sidecars**: for each, confirm the source really is gone
   (`EpilogueContextFactory`, `FlagId`, `OrbitalHarrowSystem`, `DayCycle`, `AtomicFileWriter`,
   `SaveEnvelopeDetection`, `Host/HostCli.PanelTests.{Campaign,Diagnostics,Expansion,Persistence,UI}`,
   `Radio/CensusBroadcastScheduler`, `Shelter/OrbitalHarrowHeadlessDemo`,
   `Survivors/VigilStateMachine`, `Host/CampaignServices`). Delete the sidecar, never resurrect a
   class to match a stray `.uid`.
2. **Verify before deleting**: `VigilStateMachine` (6 refs), `CampaignServices` (1),
   `CensusBroadcastScheduler` (1), `OrbitalHarrowTelemetrySystem` (4) may exist under another
   path — resolve each to its real file first; only orphans go.
3. **Add a gate**: fail if any `*.cs.uid` has no sibling `*.cs` (mirror how
   `scripts/ci/asset-orphan-sweep.sh` handles asset sidecars — reuse its style, do not invent a
   new concept). Wire it into `scripts/ci/verify-fast.sh`.
4. **Move the duplicate-tree exclusion into `.gitignore`**: `.claude/worktrees/` (143 MB) is
   currently only in `.git/info/exclude`, which is local and unshared. Every other contributor and
   CI clone lacks it, and repo-wide greps silently return duplicate "authorities" (this audit hit
   them repeatedly). Add the ignore rule; leave the directory on disk.
5. **Reconcile `docs/data/DATA_GAP_AUDIT.md`**: mark each orphan row `WIRED / DEAD /
   MOVED` against the current scanner output (e.g. `questline_master.json` is loaded at
   `src/Main.Application.cs:392`); regenerate from `artifacts/content-utilization.json` where
   possible so the doc stops drifting by hand.
6. **Reconcile `docs/audio/SILENCE_AUDIT.md`**: correct §4.7 (`OnDied` exists at
   `NeedsSystem.cs:79`, host-subscribed at `SurvivorsHostSession.cs:111`) and record that
   `rad_geiger_loop`'s exposure-end blocker is now owned by Plan 17C step 7.
7. **Reconcile `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` §26**: remove the
   `AirlockSecuritySystem.cs:80` GetHashCode claim (0 occurrences today) and re-state the
   `Main.cs` line/method counts against source rather than memory.
8. **Refresh `docs/CURRENT_AUTHORITY.md`** with this wave's artefacts: the 10-gap list, Plans
   15–19, and the three new metrics that now matter — `EFFECT_PRODUCED` count,
   `exempt_no_source_evidence` count, live-panel count.
9. **Land the working tree honestly**: 95 modified files at `ccac926e` on `main`. Group them into
   reviewable commits *before* starting 19A, and follow the project's lane discipline —
   `bit`-style "always create a lane" guidance does not apply here; this is a Git/Godot repo, so:
   small commits, one system per commit, never a 95-file commit.
10. **Add a session-continuity journey test** (the audit's named test gap): new game → act →
    save → quit → load → same panel values, same briefing history, same ending inputs. This is
    the test that would have caught 19A's `0` deaths and 16B's throwaway authorities together.
11. **Prove the ending is derived, in CI**: a scripted campaign whose save file is loaded and run
    to game over, asserting the epilogue text differs from the all-`true` default — pin the exact
    expected string so a regression to hardcoding fails loudly.
12. **Verify `--data-integrity-selftest` stays at 0 errors** and
    `--content-utilization-selftest` reflects the newly derived facts as runtime evidence (18B
    step 9), so `RUNTIME` climbs above 9.
13. **Run the checklist + `bash scripts/ci/verify-fast.sh`** as the wave-close gate, and record
    the wave's before/after metric table in the commit message.

**DoD:** zero dangling sidecars under a gate, zero stale "known issue" claims in the four audit
docs, one CI test that fails if the ending is ever hardcoded again.

---

## Cross-Task Dependencies

```
19A (derive ending) ◄── 15A/15B (choices must exist to be counted)
        │              ◄── 18B (facts must be real state, not exempted files)
        ├──► 19B (childrenSurvived is read by the ending; deepened after 19A reads it truthfully)
        └──► 19C (the CI test that pins 19A forever)
```

**Execution order:** **19A first, alone, before anything else in this plan.** Then 19C steps 1–4
(sidecars + ignore rule — 30 minutes of hygiene that de-risks every later grep), then 19B, then
the rest of 19C.

**Wave-level order (all five plans):** 15A → 16A → 15C → 16B → 19A → 16C → 17A → 17C → 17B →
18A → 18B → 19B → 18C → 19C. If only three tasks can be done, do **15A, 19A, 16A** — a playable
choice, an ending that remembers, and a menu that stops lying.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --content-utilization-selftest   # runtime evidence up, exemptions down
7. bash scripts/ci/triad-drift-gate.sh
8. bash scripts/ci/verify-fast.sh
9. three seeded 200-day campaigns, three policies → three distinct endings (manual proof, logged)
```

---

## Estimated Effort & Impact

| Task | New Core | Host | Docs/Hygiene | Tests | Player payoff | Difficulty |
|---|---|---|---|---|---|---|
| 19A | 1 file (+1 recreated) | 3 lines of bind | 0 | 6–9 | **the ending finally reflects the game** | Low–Med |
| 19B | 1–2 | 3 | 1 (atlas) | 12–16 | years feel like years | Medium |
| 19C | 0 | 1 (gate) | 6 docs + `.gitignore` | 3–4 + gate | future agents stop re-breaking this | Low |

**Guardrails:** no new ending branch, no new epilogue prose, no new panel, no new matrix — the
32 permutations and their authored text already exist and are currently unreachable. This task
connects what is already written to the state that already exists. That is the whole point of the
wave.
