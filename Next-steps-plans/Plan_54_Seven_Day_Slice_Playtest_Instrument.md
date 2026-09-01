# Plan 54 — The Seven-Day Slice: A Build Real Humans Test the Waves Against

> **Wave:** Continuity Wave 8 — *The Presented Game* (closing plan)
> **Depends on:** 17B (guidance), 22A/24A/20A/21A (the loops the slice exercises), 31A+31B
> (attribution the slice must demonstrate), 46B (metrics), 48B (release artifacts), 50C/52 (the
> presented/audio layer), 53C (this is a `NOW` item under the intake policy it just created).
>
> **Theme:** every automated signal in this project is green. 5,303 tests pass, 138 catalogs validate
> with 0 errors, 46 gates, and the manual playthrough checklist records each of its 13 steps as
> **"None (PASS)" verified by a selftest** — meaning the human-facing QA document is a list of
> machine checks with a clipboard aesthetic. There is **one deterministic seven-day smoke test in
> the repo and it isn't even a gate**, no demo build, no playtest protocol, and no recorded instance
> of a new player failing to understand the ration decision. Waves 1–7 changed what the first week
> *is*; this plan finds out whether it works.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | A real 7-day deterministic harness exists, ungated | `src/Host/SevenDayDeterministicSmokeTest.cs` (verb: `HostCli.cs:122,375` → `7day_smoke_selftest`); absent from `docs/ci/CI_GATE_MANIFEST.json`'s 46 gates |
| 2 | "Manual" QA is machine-substituted | `docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md` rows 1–13: every "Blocker" column reads `None (PASS)`, every verification column cites a selftest (`PlayableShellSelfTest`, `Day1PlayableSelfTest`, `Day1ToDay2MilestoneSelfTest`, `PlayerPanelsUiTest`, `RadioSelfTest`, `JournalSelfTest`, `ExpeditionHeadlessDemo`) |
| 3 | Two playtest docs exist with no data | `docs/HoldfastManualPlaytest.md`, `docs/HoldfastPlaytestHandoff.md` — handoffs and checklists; no session records, no findings from real players |
| 4 | No demo/first-hour build target | `export_presets.cfg` has exactly two presets (`Linux/X11`, `Windows Desktop`); no slice or demo preset, no scenario-start asset |
| 5 | The first week is now a different game than the checklist assumes | Waves 1–7 rewrite the first days: guidance (17B), eating that works (22A), dose that depends on place (20A), gear that fails (21A), attribution in the briefing (31), policy decisions (43B), ambience (52) — the Day-1 rows were written before any of it |
| 6 | Player telemetry exists in design only | Wave 7's 46B (local, private, opt-in recorder + synthetic players) — this plan is its first real use |
| 7 | The slice's content is already authored | 225 authored items, 43 duty-roster marks, 65+50+100 moral-choice quests, 152 atmosphere lines, 30 heirlooms, 118 radio broadcasts, 23 echoes — the failure is never supply |
| 8 | Difficulty presets don't exist yet | Wave 4's 34B (no `difficulty` string in `src/`; tuning applied as three empty arrays) — a slice needs one preset chosen, and its choice is a product decision |
| 9 | Exported builds are still unbooted in CI | Wave 3's 26B — a playtest build handed to a human is exactly the artifact that has never been verified to boot |
| 10 | Accessibility gates are prose | `ashfall-ui-access`/`ashfall-input-map-audit` skills, `docs/ui/DESIGN_SYSTEM_RULES.md` — nothing measures whether a first-timer can operate the slice with keyboard only (37B) |

---

## Task 54A — Define and cut the seven-day slice

**Goal:** one curated, buildable, repeatable first week — authored beats, a fixed seed set, and a
freeze policy — that every later change must keep working.

**Files:** new `Assets/Ashfall.Core/Campaign/SliceScenario.cs` (+
`data/slice_seven_days.json`), new `export_presets.cfg` demo preset,
`src/Host/SevenDayDeterministicSmokeTest.cs`, `scripts/ci/export-smoke-boot.sh` (26B),
`docs/design/PILLARS.md` (53B), `docs/qa/SLICE.md`, `src/Main.GameFlow.cs` /
`Main.Onboarding.cs`, `docs/roadmap/RAILS.md`, `Ashfall.Core.Tests/SliceScenarioTests.cs`.

### Substeps

1. **Write the intended week first, on one page**: day 1 (orient, feed, first decision), day 2
   (craft + first dispatch), day 3 (weather threat + route choice), day 4 (injury or dose event),
   day 5 (a death and a funeral), day 6 (a policy choice with a grievance), day 7 (a deadline and a
   resolution). Each beat names the *system* that must fire, so a failure downstream is attributable.
2. **Author it as data, not as a test script** (`slice_seven_days.json`): seed, roster, starting
   stock, scripted world events, and the policy script of the "ideal player" — so the slice is
   reproducible in the harness, in a demo build, and in a human session.
3. **Freeze the slice**: a scenario id + content hash; changing it requires a note in the changelog
   (48B) and a re-run of every gate that consumes it. An unfrozen slice can't measure anything.
4. **Prove it completes deterministically**: extend `SevenDayDeterministicSmokeTest` to run the slice
   and assert the seven beats occur in order, with a digest; then **register it as a gate**
   (closing Wave 5's finding that it isn't one).
5. **Cut the demo preset** (`export_presets.cfg`): scenario-start build that boots straight into day
   1 with the slice seed, save slots scoped to a demo root, and no path into a full campaign —
   built by the staging script so 26B's data-deploy guarantees apply.
6. **Boot-verify the demo artifact** headlessly (26B step 6) before any human touches it: nothing
   destroys a playtest like a build that can't load its own catalogs.
7. **Difficulty choice is a product decision**: pin the slice to the *default* preset (34B) and record
   it in `docs/design/PILLARS.md` — the first week sets expectations for the whole game.
8. **Instrument the slice** with 46B's recorder and its funnel steps (guidance opened, first ration
   decision, first dispatch, first storm survived, first death witnessed) so a human session produces
   the same shape of data as the synthetic run — comparable baselines, two sources.
9. **Accessibility preflight**: the slice must be completable keyboard-only (37B) and with
   reduce-motion + captions (37C); assert it in the harness before recruiting a human.
10. **Guide, don't tutorial**: 17B's overlay is the only permitted teaching surface; if a beat needs
    a bespoke tutorial pop-up, the beat is wrong (or the system is unintelligible) — record which.
11. **Freeze the art/audio expectations**: the slice requires 50A's manifest coverage for its
    rendered faces (portraits, icons) and 52A's ambience states — the point where the presentation
    waves become measurable.
12. **Tests**: slice determinism and beat ordering, digest stability across platforms, demo-boot
    smoke, keyboard-only completion, funnel-step emission per beat.
13. **Docs**: `docs/qa/SLICE.md` (the beats, the intent, the freeze policy, how to re-baseline).
14. **Run the checklist** + the release gate (39A) against the demo preset.

**DoD:** one reproducible week, bootable by a stranger, gated on every push.

---

## Task 54B — Real humans: the playtest protocol that produces decisions, not vibes

**Goal:** a small, rigorous, consented playtest program — scripted conditions, silent observation,
structured capture — whose output is a `DECISIONS.md` entry per finding, feeding 53's roadmap.

**Files:** new `docs/qa/PLAYTEST_PROTOCOL.md`, new `docs/qa/sessions/YYYY-MM-DD-NN.md`
(records), `docs/balance/DECISIONS.md` (46A step 6), 46B's telemetry recorder,
`docs/telemetry/PRIVACY.md` (consent + redaction), `docs/design/PILLARS.md`,
`ashfall-telemetry-playtest`, `ashfall-tutorial-review`.

### Substeps

1. **Write the protocol before recruiting anyone**: session length, who participates
   (never the developer; rotate across tools), the three scripted conditions (new player, returning
   player, hostile-to-the-genre player), the intervention rule (help only when the player is stuck
   >3 minutes, and log it), and the exit interview.
2. **Define the unit of evidence**: a finding is `(observed behaviour, beat, cause hypothesis,
   severity, decision)` — no finding without a beat reference and a decision, mirroring 46A's ADR
   discipline.
3. **Instrument passively**: 46B's local recorder + session notes + screen capture where consented;
   never an interruption-heavy questionnaire mid-play, which destroys exactly what's being measured.
4. **Set the recruiting target modestly and honestly**: 6–10 sessions per slice build is enough to
   find the sharp edges and matches the repo's existing single-player scope; more is
   statistics-theatre at this size.
5. **Test the failure paths humans take**: ignoring guidance, dispatching an unready party, refusing
   the ration cut, letting someone die untended — the QA checklist only ever walks the happy path, and
   the slice's job is what happens when the player is clever or careless.
6. **Ask the three legibility questions after every session**: what did you think was happening on
   day 3? what did you think would happen if you did X? what did you want and couldn't find? — the
   attribution work (31B) is judged by those answers, not by the presence of a briefing.
7. **Record the emotional read too**: who did they name? who did they mourn? did the funeral mean
   anything? — Waves 6 and 8 spend the most effort here and nothing in CI can measure it.
8. **Triage findings into the roadmap**, not into a bug list: `fix now` / `rails missing` (53C) /
   `design intent disagreement` (pillar decision) / `wont fix, logged`.
9. **Close the loop publicly in-repo**: every accepted finding gets a `DECISIONS.md` entry with the
   session id and the change; every rejected one gets a reason, or playtesting becomes theatre.
10. **Repeat per release candidate** (48B step 6) with the frozen slice, so deltas across builds are
    comparable and regressions are felt rather than argued.
11. **Guard the humans**: consent, right to withdraw, notes de-identified, no personal data in
    committed records — the same privacy posture as 46B step 1.
12. **Tests**: the harness asserts the protocol artifacts exist per release (a session record set
    without findings is flagged), and the funnel report shape matches synthetic baselines.
13. **Docs**: `docs/qa/PLAYTEST_PROTOCOL.md` + `docs/qa/sessions/` (records), replacing the
    selftest-backed claim rows in `MANUAL_PLAYTHROUGH_CHECKLIST.md` with real findings — the
    checklist should cite both, since a machine check and a human are different instruments.

**DoD:** at least six sessions per release, each producing findings that land as decisions.

---

## Task 54C — Make the slice the project's standing proof

**Goal:** promote the slice from an event to an instrument: a nightly gate, a per-release scorecard,
and the number every wave index reports.

**Files:** `docs/ci/CI_GATE_MANIFEST.json` (new slice gates), `.github/workflows/ci.yml`,
`scripts/ci/release-gate.sh` (39A), `docs/qa/SLICE_SCORECARD.md` (generated), 46A's sweep harness,
53A's plan register, 48B's changelog, `Next-steps-plans/` wave ledger (29C),
`docs/CURRENT_AUTHORITY.md`.

### Substeps

1. **Two gates, two tiers**: `slice_determinism` (fast tier — beats occur in order, digest stable) and
   `slice_play_metrics` (nightly — synthetic-player funnel + reachability + perf).
2. **Publish a scorecard per release** (generated, never typed): completion rate under the scripted
   policy, first-death day, time-to-first-decision, funnel drop points, unbound-port count, dead-def
   count, snapshot diffs, and human-session findings count — one table in `docs/qa/SLICE_SCORECARD.md`.
3. **Define the bar for "shippable first week"**: derived from 46A's targets, not from mood — e.g.
   day-7 completion under the naive policy ≥ X%, first death not before day N, no beat unreachable,
   no funnel step below Y% discovery.
4. **Refuse releases that regress the slice** — the release gate (39A step 10) fails when the
   scorecard falls below the bar, and any exception requires a written pillar decision.
5. **Make it the roadmap's measuring stick** (53B step 7): a proposed feature's day-2 relevance is
   judged by whether the slice gets better, which converts the pillars from slogans into a filter.
6. **Track the funnel across releases**, not just within one, so Waves 9+ can see whether guidance and
   attribution work compounded or decayed.
7. **Version the slice**: scenario id + data hash in the changelog, with a deprecation policy for an
   old slice so old scorecards remain interpretable (48A's compatibility thinking applied to QA).
8. **Keep it cheap**: the nightly must fit in the CI time budget — the 7-day deterministic harness
   already runs in seconds; the synthetic funnel is a few policies × a few seeds.
9. **Fold in the perf floor** (26C): the slice run is the canonical perf scenario, so the budget is
   measured on the same thing humans play.
10. **Retire the "PASS-by-selftest" claims**: rewrite `MANUAL_PLAYTHROUGH_CHECKLIST.md` rows to cite
    both the machine check and the most recent human session date, so the doc stops overstating.
11. **Publish what it found**: a short "the slice's worst moments" list per release in the patch notes
    (48B) — honesty about first-week problems is a design signature, and it also deters
    nice-to-have features (53C's intake rubric gains a real cost signal).
12. **Tests**: scorecard completeness (every declared metric present), bar enforcement (a fixture
    below the bar fails the gate), slice-version drift detection.
13. **Run the checklist** + `bash scripts/ci/release-gate.sh` and paste the scorecard into this wave's
    close-out.

**DoD:** the first week has a number, the number is gated, and every release says what it moved.

---

## Cross-Task Dependencies

```
34B (presets) ──► 54A step 7       46A/46B (targets, recorder) ──► 54A step 8, 54B step 3, 54C step 2
26B (export boot) ──► 54A step 6   17B/31B (guidance, attribution) ──► 54B step 6
37B/37C (keyboard-only, reduce-motion) ──► 54A step 9   50A/52A (assets, ambience) ──► 54A step 11
48B (release artifacts) ◄── 54C steps 2–4               53B/53C (pillars, intake) ◄── 54C step 5
   54A (cut the slice) ──► 54B (humans) ──► 54C (standing instrument)
```

**Execution order:** 54A → 54B → 54C, and in Wave 8: 50A → 51A → 52A → 51B → 50C → 52B → 51C →
52C → 53A → 53B → 53C → **54A → 54B** → 54C. The slice is last on purpose: it is the instrument that
measures everything before it, and it is the only thing in the roadmap that a stranger can play.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- <7day slice verb>                # beats in order, digest stable
7. bash scripts/ci/export-smoke-boot.sh                          # demo preset boots
8. bash scripts/ci/generate-slice-scorecard.py --check           # (54C)
9. keyboard-only + reduce-motion completion of the slice in CI
10. human session records present for the release (54B), consented and de-identified
11. bash scripts/ci/release-gate.sh && bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Code | Data | Docs/Process | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 54A | 1–2 | 1 scenario | 1 doc + 1 preset | 8–12 | Medium | LOW |
| 54B | 0 | 0 | protocol + records | 3–5 + real sessions | **High (it is a research discipline, not a ticket)** | LOW |
| 54C | 0 | 0 | scorecard generator + 2 gates | 5–8 | Medium | LOW |

**Guardrails:** no tutorial bespoke pop-ups (guidance or nothing); no slice tuned to pass — if the
ideal player has to be superhuman, the first week is wrong and that is the finding; no unconsented
recording, no personal data in committed notes; no scorecard metric that isn't generated; no release
exception without a written pillar decision; and no treating a green deterministic run as evidence
that a person understood anything — that substitution is the exact habit this plan exists to break.
