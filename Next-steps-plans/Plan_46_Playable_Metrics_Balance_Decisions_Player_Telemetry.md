# Plan 46 — Playable Metrics: Measure the Player, Decide the Difficulty

> **Wave:** Continuity Wave 7 — *Content on Rails & the Measurement Layer*
> **Depends on:** 31C (the day record this builds on), 34B (difficulty presets), 26C (perf budgets),
> 17B (the onboarding whose funnel is being measured).
>
> **Theme:** the repo contains **27 seeded balance CSVs** with per-day need/dose columns — real
> simulation output — and **no file in the repository records what produced them, under what
> parameters, or what was decided.** Meanwhile there is no player-side measurement at all: every
> "telemetry" string in the codebase is diegetic fiction (orbital harrow, pump nodes, cohort supply).
> So Waves 1–6 changed consumption rates, dose curves, ration maths, season severity and difficulty
> — each balanced against a spreadsheet nobody can regenerate, and none of it validated against what
> an actual player does in the first hour.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | Simulation output exists, undocumented | `artifacts/balance/` → **27 CSVs**; header: `seed,day,hunger,thirst,fatigue,warmth,morale,health,radiationDose,lifetimeExposure,hungerCritical,thirstCritical,healthLossPerDay`; scenario-named files (`fed_dailyration_seed_42_30d`, `fed_severescarcity_seed_999_30d`, `power_econ_combinedstress_seed_42_7d`) |
| 2 | **Nothing references them** | `grep -rl "artifacts/balance" docs/ *.md scripts/ tools/` → **0 files**; no generator script found in `scripts/pipeline` or `scripts/maintenance` — the producer is outside the repo (a skill run), so the corpus is unreproducible |
| 3 | No design-record home | `ls docs \| grep -iE "balance\|design"` → **0 directories/files**; no difficulty target, no "time to first death", no survival-rate curve is written down anywhere |
| 4 | No player telemetry of any kind | `grep -rniE "telemetry\|analytics\|funnel\|playtest" src/ Assets/Ashfall.Core` → hits are diegetic only: `WorldHostSession.cs:192 ActivateOrbitalTelemetryDemo`, `StatusPanel.cs:330 "COHORT & SUPPLY TELEMETRY"`, `SumpFloodingPanel.cs:120`, `CombatHistoryPanel.cs:46`, `MapPanel.cs:108 intel.telemetryActive` |
| 5 | The substrate for real metrics exists | `DayStateChangeEvent` with `Kind/SourceOwnerId/PrimaryId/SecondaryId/Numeric` from all 19 owners (Wave 4's 31 measured this), plus `PlayerSurfaceContract`/`ObserveSigil("inventory.used")` (`src/Main.Inventory.cs:90`) — sigils are a *beginning* of action instrumentation with no aggregate |
| 6 | Perf is measured but advisory | `artifacts/runtime-scale-results.json` n=5, `"advisory"` (`PerformanceSelfTest.cs:47`) — Wave 5's 39A step 2 fixes the sampling |
| 7 | Onboarding is unmeasurable and (pre-17B) unreachable | `docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md` Day-1 rows are all "None (PASS)" via selftests — machine checks, not player behaviour |
| 8 | Difficulty doesn't exist yet | Wave 4's 34B (no `difficulty` string anywhere in `src/`; `hardcore_economy_tuning.json` applied as three empty arrays) — metrics must arrive with the presets, not after |
| 9 | Content utilisation has a metric; play has none | `--content-utilization-selftest` publishes stage counts; there is no equivalent for "did the player experience it" |

**Reading:** the project already measures its *simulation* and its *content*; it measures neither
the *player* nor the *decisions made about balance*. That is the difference between tuning and
guessing — and after Waves 1–6 rewired the loops, guessing is what's on tap.

---

## Task 46A — Make the balance corpus reproducible and its decisions readable

**Goal:** one in-repo harness that regenerates the CSVs from a named scenario + seed set, and one
documented place where the resulting decisions live.

**Files:** `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs` (reuse),
new `scripts/balance/run_sweep.py` + `scripts/balance/scenarios/*.json`,
`artifacts/balance/` (regenerated), new `docs/balance/README.md`,
`docs/balance/DECISIONS.md` (ADR log), `.gitignore`/`.gdignore` for artifacts,
`docs/ci/CI_GATE_MANIFEST.json`, `Ashfall.Core.Tests/BalanceCorpusTests.cs`.

### Substeps

1. **Reconstruct the generator before deleting anything** — the CSV columns name the harness outputs
   exactly; find the code path that can produce them (the perf harness already drives multi-day
   campaigns) and make it a scripted, checked-in sweep rather than an improvised skill run.
2. **Scenarios become data**: `fed_dailyration`, `fed_scarcity`, `fed_severescarcity`,
   `power_econ_combinedstress` etc. move into `scripts/balance/scenarios/*.json` with the seed list,
   day count, roster tier and policy script — so a sweep is reviewable in a diff.
3. **Record the run header** in every CSV (git sha, scenario, seed, build config, Godot/.NET
   version) — the current files can't be attributed to any build, which makes them unscientific.
4. **Publish a standard panel** per sweep: survival rate by day 7/30/90, time to first critical,
   time to first death, morale floor, dose curve, resource exhaustion day — as a generated
   `docs/balance/README.md` table, not a hand-typed summary.
5. **Define the targets** for the first time, in `docs/balance/TARGETS.md`: what a "normal" preset
   *should* feel like (e.g. day-30 survival ≥ X for a competent script policy, first crisis window,
   first death not before day N), stated as numbers with a rationale and a review date.
6. **Write ADRs**: `docs/balance/DECISIONS.md` — one entry per tuning change: what, why, sweep
   evidence, affected preset, rollback condition. This is the missing half of "we tuned it".
7. **Baseline assertions in CI**: a nightly gate runs the reference scenario set and fails on a
   defined drift (e.g. day-30 survival moves >N% on an unrelated PR) — the balance twin of Wave 1's
   `SaveChecksum` sweeps and Wave 3's coverage ratchet.
8. **Re-baseline after every wave**: Waves 1–6 each shift consumption/dose/morale; the corpus must be
   regenerated once, explicitly, with the before/after table in the ADR — otherwise the old CSVs will
   be quietly compared against new numbers.
9. **Scripted policies, not random play**: encode 3–4 player archetypes (cautious, greedy,
   neglectful, expert) as deterministic action policies so sweeps measure design intent rather than
   dice noise.
10. **Cover the new systems**: add scenarios for season severity (38B), ration policy (43B),
    gear lifespan (21A), power shedding (23B), and the identity/relation effects (44A) — the rails
    Waves 2/5/6 built have no balance evidence yet.
11. **Kill the orphan files**: the 27 unattributed CSVs get regenerated or moved to
    `docs/archive/balance/` with a note; no repo artifacts whose producer nobody knows.
12. **Tests**: sweep determinism (same seed + scenario ⇒ same CSV), scenario schema validation, and
    the drift assertion itself (prove it fails on an intentional 5% regression).
13. **Run the checklist** + the nightly sweep once locally and paste the table into the ADR.

**DoD:** every balance number in the repo can be regenerated, attributed, and traced to a decision.

---

## Task 46B — Player action metrics: local, private, opt-in, and readable

**Goal:** a player-behaviour record built on the day-event stream and action sigils — stored locally,
never uploaded, off by default in release, on by default in dev — that answers the first-hour
questions no selftest can.

**Files:** new `Assets/Ashfall.Core/Telemetry/PlaySessionRecorder.cs`,
`src/Main.Campaign.cs` (day hook), `src/Main.GameFlow.cs`, `src/Main.PlayerSurfaces.cs`
(sigil sites), `src/Host/AshfallInputActions.cs` (used-keys), 31C's day record,
new `src/Host/TelemetrySelfTest.cs` (verb), `docs/telemetry/PLAY_METRICS.md`,
`docs/telemetry/PRIVACY.md`, `.gitignore`, `UserSettings` (toggle).

### Substeps

1. **Privacy stance first, in writing**: local-only files under `user://`, off by default in release
   builds, on in dev, **no network path at all**, no absolute paths/usernames/content strings
   (redaction rules from 31C step 7), and a documented deletion route. This constraint gates the whole
   task; if it can't be met, stop.
2. **Define the event schema**: session id (random, non-identifying), build/version, preset, seed,
   day, action (`panel_opened`, `ration_policy_set`, `dispatch`, `choice_resolved`, `consume`,
   `save`, `quit`), target id, and outcome — key ids, not prose.
3. **Build on what already exists**: `ObserveSigil("inventory.used")`
   (`src/Main.Inventory.cs:90`) is a real, if isolated, action-instrumentation point — generalise it
   into `PlaySessionRecorder.Record(action, target, outcome)` rather than inventing a second system,
   and grep for other sigil sites to migrate.
4. **Attach the day stream**: 31's `DayStateChangeEvent`s are the *consequence* half; joining actions
   to consequences is what makes "the player cut rations on day 4 and two left on day 9" answerable.
5. **First-hour funnel**: define the steps (guidance opened, first craft, first dispatch, first ration
   decision, first storm survived, first day advanced past tutorial, first death witnessed) and record
   completion + time-to-step per session.
6. **Dead-end detection**: flag sequences where the player repeats an action N times or opens/closes
   the same panel — the mechanical signature of confusion, which no selftest produces.
7. **Stuck detection**: days advanced with zero player actions (a player who stopped understanding the
   game keeps clicking "next day") — the most valuable single metric Waves 1–6 could use.
8. **Local report generator**: a CLI verb that turns recorded sessions into `docs/telemetry`-friendly
   aggregate tables (funnel %, median time-to-step, top dead-end panels) so the data is usable without
   any external analytics service.
9. **Headless synthetic players**: run the scripted policies from 46A step 9 through the same
   recorder, so the funnel pipeline is testable in CI without a human — and so a real report can be
   produced on every release candidate.
10. **Accessibility parity signals**: record (locally, aggregated) whether input mode was
    keyboard/controller/mouse (37C) to check that the work isn't speculative; no per-player identity.
11. **Size + rotation**: bounded files, rotation on session end, and a hard cap — a telemetry file
    that grows forever is the bug Wave 5's 39B exists to prevent.
12. **Tests**: schema stability, opt-out produces no file, rotation, aggregation correctness, funnel
    step detection on a scripted session, and a redaction test asserting no path/name/prose leaks.
13. **Docs**: `docs/telemetry/PLAY_METRICS.md` (schema + how to read a report) and
    `docs/telemetry/PRIVACY.md` (the stance, in plain language, citable in a store listing).
14. **Run the checklist** + the release gate.

**DoD:** a release candidate ships with a first-hour funnel report, generated locally, from
synthetic players, with no network and no identity.

---

## Task 46C — Close the loop: metrics that change the game

**Goal:** wire measurement back into design decisions — dynamic tuning guardrails, a difficulty
review ritual, and the first "the data said X so we changed Y" record.

**Files:** `docs/balance/TARGETS.md` + `DECISIONS.md`, difficulty presets (34B), season severity
(38B), onboarding steps (17B), guidance nudges (42B's attention budget),
`Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` (utilisation vs reachability),
new `docs/balance/REVIEW_RITUAL.md`, `Next-steps-plans/` wave ledger (Wave 3's 29C).

### Substeps

1. **Set acceptance thresholds per release** from 46A's targets, and require a signed-off funnel
   report before a release candidate is cut (Wave 5's 39A gate list gets one more line).
2. **Instrument the fixes**: for each Wave 1–6 plan that changed difficulty (22A food, 23B load
   shedding, 34B presets, 38B seasons), state the expected metric movement, then check it against
   the sweep — the discipline that turns "we shipped a fix" into "we shipped a known change".
3. **No adaptive difficulty without an explicit rule**: if a "director" is ever proposed (and it
   will be), it must be authored, visible in `DECISIONS.md`, and never silent — the game's contract is
   that outcomes are foreseeable (Wave 4's 34B guardrail).
4. **Guide from data, not opinion**: where ≥X% of sessions stall at a step, the guidance overlay
   (17B) and the briefing's attribution (31B) are the fix surfaces — voice/journal/panels, not new
   mechanics.
5. **Retire unreachable content by evidence**: a family wired in 45B but with 0% reachability in the
   synthetic sweep is either unfindable (fix discovery) or unnecessary (archive it) — the honest
   continuation of Wave 1's 18B.
6. **Publish a per-release balance delta** in the changelog (Wave 7's 48A) — generated from
   `DECISIONS.md` entries, so patch notes are honest about what changed numerically.
7. **Watch the long tail**: multi-session metrics (38C deadlines met, 41C generations reached, 34C
   legacy records written) are where a 200-hour game breaks; define them before they matter.
8. **Keep CI fast**: sweeps nightly, funnel checks per release, schema tests per push.
9. **Calibrate expectations**: state the noise floor (seeds needed before a delta is meaningful) in
   `TARGETS.md`, so nobody "fixes" a 2% wobble — the statistical hygiene Wave 5's 39A step 2 started.
10. **Ritual**: a documented monthly review — read the report, write ≤3 decisions, each with an owner
    and a plan number — so measurement doesn't become decoration.
11. **Tests**: a meta-test that the report generator produces every declared section, and that
    `DECISIONS.md` entries cite a sweep artifact (an ADR with no evidence fails the docs gate).
12. **Run the checklist** + a full synthetic-session run.

**DoD:** at least one design change per release traceable to a measurement, in writing.

---

## Cross-Task Dependencies

```
31C (day record) ──► 46B steps 4–5      34B (presets) ──► 46A steps 5,10 & 46B step 2
26C (perf budgets) ─► 46A step 7         17B (guidance) ─► 46B step 5 funnel steps
39A (release gate) ─► 46C step 1         45A/45B (rails) ─► 46C step 5 reachability
                     46A (reproducible sweeps) ──► 46C (targets + ritual)
                     46B (player metrics) ─────► 46C step 4 (fix surfaces)
```

**Execution order:** 46A → 46B → 46C, and inside Wave 7: **45A → 46A → 47A → 46B → 48A → 49A →
46C → 48B → 49B → 49C/D → 48C → 49E** — a difficulty target is meaningless before the rails prove
what is reachable.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. python3 scripts/balance/run_sweep.py --scenario reference --seeds …   # reproducible
7. godot --headless --path . -- --play-metrics-selftest          # (46B synthetic session)
8. privacy assertions: opt-out writes nothing; no identity/path/prose in records
9. drift gate: intentional 5% regression fails the nightly sweep
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Scripts/Docs | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 46A | 1 harness hook | 1 | 2 scripts + 3 docs | 6–10 | Medium | LOW (artifacts only) |
| 46B | 1 new | 4–6 | 2 docs | 10–14 | Medium | LOW (local, opt-in) — **privacy is the risk** |
| 46C | 0 | 0 | ritual + docs | 3–5 | Low | LOW |

**Guardrails:** no network telemetry, no per-player identity, no remote config, no silent adaptive
difficulty, no gate that depends on manual skill runs (everything reproducible in-repo), and no
balance claim without a named scenario, seed set, and build sha.
