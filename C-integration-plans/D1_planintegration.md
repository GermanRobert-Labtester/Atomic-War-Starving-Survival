
# D1 — Flagship Integration Plan: The Seven-Day Slice as ASHFALL's Standing Product Proof

> **Canonical filename:** `D1_planintegration.md`<br>
> **Next file:** `D1_planintegration[2].md`<br>
> **Then:** `D1_planintegration[3].md`<br>
> **Naming rule:** keep the sequence number immediately before `.md`, inside square brackets, so the filename remains easy to copy/paste and sort.
>
> **Plan class:** Flagship integration / execution / verification plan<br>
> **Primary source:** Plan 54 — *The Seven-Day Slice: A Build Real Humans Test the Waves Against*<br>
> **Source wave:** Continuity Wave 8 — *The Presented Game*<br>
> **Integration intent:** convert the source's 54A → 54B → 54C closing sequence into an implementation-ready program with explicit contracts, sequencing, artifacts, tests, failure policy, human-study operations, release gates, ownership boundaries, and the next tasks that naturally follow once the slice becomes operational.
>
> **Source-preserving rule:** statements about the current repository state below are inherited from the supplied Plan 54 unless marked **Integration-derived**. The new tasks after 54C are **Integration-derived** follow-ons; they extend the source rather than pretending to be pre-existing repository findings.

---

## 0. Executive Directive

ASHFALL already has a large automated assurance surface: the source records 5,303 passing tests, 138 validating catalogs, 46 gates, and a 13-step "manual" checklist whose rows are machine-verified. That is an engineering achievement, but it leaves a product-proof gap: the repository can prove that systems execute while still failing to prove that a new person understands, survives, interprets, and emotionally reads the first week.

D1 closes that gap by making one seven-day scenario the common object shared by deterministic tests, exported builds, accessibility checks, telemetry, human playtests, synthetic-player sweeps, performance budgets, patch notes, and release decisions. The same scenario must be reproducible enough for CI, legible enough for a first-time player, and stable enough for scorecards to remain comparable release over release.

This plan therefore has four outcomes:

1. **Cut a canonical seven-day slice.** It is authored as data, versioned, hashed, deterministic where required, bootable as an exported demo, and pinned to an explicit difficulty/presentation contract.
2. **Measure human understanding without substituting automation for people.** Real sessions use a written protocol, passive/consented instrumentation, de-identified records, and findings that terminate in decisions.
3. **Promote the slice into a release instrument.** Fast and nightly gates produce generated scorecards, enforce thresholds, expose regressions, and bind release artifacts to a concrete playable experience.
4. **Create the next integration rail.** Once the instrument is live, derived tasks harden scenario contracts, failure-path coverage, telemetry integrity, accessibility evidence, build provenance, playtest operations, balance calibration, cross-platform parity, and release governance.

The governing principle is simple: **a green deterministic run proves execution; a human session proves comprehension; a release decision requires both.**

---

## 1. Source Baseline and Problem Statement

### 1.1 Source facts that D1 treats as fixed inputs

The supplied Plan 54 establishes the following current-state evidence:

- `src/Host/SevenDayDeterministicSmokeTest.cs` already exists, but the seven-day smoke run is not registered as one of the CI gates.
- `docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md` presents 13 rows as manual QA while every row is backed by selftests rather than recorded human evidence.
- `docs/HoldfastManualPlaytest.md` and `docs/HoldfastPlaytestHandoff.md` exist as procedures/handoffs, not as a corpus of real session records.
- `export_presets.cfg` has Linux/X11 and Windows Desktop presets but no dedicated slice/demo preset or scenario-start asset.
- Earlier waves changed the actual first-week experience: guidance, eating/ration behavior, place-dependent dose, equipment failure, attribution, policy decisions, ambience, and other presentation/loop work.
- Player telemetry is designed but, per the source, the seven-day slice is intended to become its first real product use.
- Content supply is not the limiting factor; the source enumerates hundreds of authored items/quests/lines/broadcasts and frames wiring/legibility as the actual risk.
- Difficulty presets are not yet an established runtime contract in the cited state, while the slice requires an explicit default difficulty.
- Exported-build boot verification is still a dependency.
- Accessibility expectations exist as prose/skills/design rules but do not yet prove first-timer completion of this slice.

D1 must not erase those findings by introducing parallel mechanisms. The implementation should preferentially wire into the source-named systems, manifests, release scripts, telemetry work, design pillars, and roadmap rails.

## 2. Integration Architecture

### 2.1 Canonical scenario flow

```text
data/slice_seven_days.json
        │
        ▼
SliceScenario loader + validator
        │
        ├──► deterministic host runner
        │        └──► beat evidence + digest + synthetic metrics
        │
        ├──► game scenario-start adapter
        │        └──► exported demo build
        │                └──► human playtest + accessibility completion
        │
        └──► scorecard metadata
                 ├── scenario id/version/hash
                 ├── git revision
                 ├── build artifact identity
                 ├── automated metrics
                 ├── human session summary
                 └── release decision
```

The scenario file is the contract. No test-only hardcoded parallel story should be allowed to become authoritative, because that creates a false green path that humans never play.

### 2.2 Core integration boundaries

| Boundary | Producer | Consumer | Required contract |
|---|---|---|---|
| Scenario definition | `slice_seven_days.json` | host + game runtime | schema, version, seed, roster, stock, events, beat ids |
| Scenario initialization | `SliceScenario` / loader | `Main.GameFlow` / scenario-start | identical initial state |
| Beat evidence | game systems | deterministic harness + telemetry | stable ids and occurrence semantics |
| Demo export | export pipeline | playtesters + boot smoke | exact scenario/version embedded |
| Telemetry | local recorder | scorecard generator | opt-in, minimal, deterministic event names |
| Session record | playtest protocol | decisions/roadmap | de-identified, beat-linked findings |
| Scorecard | generator | release gate | generated only; thresholds + provenance |
| Waiver | design/release authority | release gate | explicit reason, owner, expiry, affected metric |

### 2.3 Data ownership rules

1. The slice JSON owns scenario-specific initial conditions and scripted events.
2. General gameplay tuning remains owned by the game's ordinary tuning/configuration systems; the slice must not fork them just to pass.
3. The deterministic harness owns orchestration and assertions, not gameplay semantics.
4. Telemetry owns observations, never state mutation.
5. The scorecard generator owns derived release metrics; humans do not hand-edit metric values.
6. Human session notes own qualitative observations; automated tests never fabricate them.
7. `DECISIONS.md` owns accepted/rejected design responses to findings.
8. `PILLARS.md` owns product-level exceptions where the team intentionally accepts a metric regression for a design reason.
9. The release gate owns enforcement, not threshold invention; thresholds are versioned policy inputs.
10. A scenario-version change must invalidate comparison claims that cross incompatible versions unless an explicit migration note explains why the metrics remain comparable.

## 3. Program Sequencing and Critical Path

### 3.1 Mandatory order

The source gives the closing execution order as `54A → 54B → 54C`, with 54A and 54B near the end of Wave 8 and 54C closing the wave. D1 keeps that order and expands it into six phases:

- **Phase 0 — Preflight:** dependency reconciliation, authority map, branch/build baseline.
- **Phase 1 — Scenario contract:** 54A data model, fixed beats, freeze/version policy.
- **Phase 2 — Executable slice:** deterministic runner, demo export, boot check, accessibility preflight.
- **Phase 3 — Human evidence:** 54B protocol, session records, finding-to-decision pipeline.
- **Phase 4 — Standing proof:** 54C scorecard, two-tier gates, release enforcement.
- **Phase 5 — Derived hardening:** 54D onward, closing the new classes of risk exposed by operating the slice.

### 3.2 Critical-path dependencies

The integration must reconcile source dependencies before claiming completion:

- 34B/default difficulty selection is needed before the slice is a stable product statement.
- 26B/export boot work is needed before human distribution is safe.
- 46A/46B metrics and recorder work is needed before human/synthetic comparisons.
- 17B guidance and 31B attribution are directly evaluated by the session questions.
- 37B/37C accessibility behavior is part of slice completion, not a later polish pass.
- 50A/52A presentation and ambience coverage are included in the measured slice.
- 48B release artifacts/changelog supply provenance and publication.
- 53B/53C pillars/intake turn slice findings into roadmap decisions rather than uncontrolled ticket growth.

If one dependency is incomplete, do not create a private substitute inside D1. Either wire to the real dependency, implement its minimum required contract as part of the task that owns it, or mark the task blocked.

### 3.3 Exit rule for each phase

No phase closes on "code written." A phase closes only when:
- the artifact exists;
- the artifact is wired to its consumer;
- at least one positive test and one failure test prove the contract;
- documentation names the authority;
- verification commands pass;
- no duplicate competing path remains undocumented.

---

## 4. Phase 0 — Repository Preflight and Authority Lock

Before changing behavior, create a single integration worksheet in `docs/qa/SLICE_INTEGRATION_STATUS.md`. It is not a scorecard; it is a temporary execution ledger.

### Substeps

1. Record the exact git revision from which D1 begins.
2. Re-read the source-named files and confirm paths still exist.
3. Enumerate all CLI verbs related to seven-day smoke, playable shell, Day 1, radio, journal, expedition demos, data integrity, bridge tests, and release gates.
4. Inspect `docs/ci/CI_GATE_MANIFEST.json` and list every existing gate, tier, owner, timeout, and invocation.
5. Inspect `export_presets.cfg` and document current export preset names verbatim before adding a demo preset.
6. Locate the scenario/game-start composition entry point and determine where a scenario adapter can be inserted without bypassing the normal composition root.
7. Locate the telemetry recorder implementation produced by 46B, if present; verify event schema, storage location, opt-in behavior, and redaction semantics.
8. Locate difficulty/tuning configuration produced by 34B, if present; identify its canonical default rather than inventing one.
9. Locate accessibility runtime toggles and input maps used by 37B/37C.
10. Locate presentation manifest/ambience state validators from 50A/52A.
11. Confirm the release-gate script and CI workflow invoke the same underlying verbs locally and in CI.
12. Search for any second seven-day scenario, demo start path, hardcoded seed, or stale playtest runner that could become a parallel authority.
13. Baseline all existing test counts, gate counts, catalog counts, and known warnings. Do not copy source numbers forward if the current branch differs.
14. Run the existing seven-day smoke verb once and capture its output/digest, even though it is ungated.
15. Run the current exported build flow once, if available, and record whether the artifact boots.
16. Create a "do not regress" baseline section listing any already-known failures; do not attribute pre-existing failures to D1.
17. Decide the authoritative source for slice version: recommended `scenario_id` + semantic `scenario_version` + SHA-256 of canonicalized scenario content.
18. Decide the authoritative source for build identity: git commit plus build manifest hash.
19. Decide the authoritative location for generated scorecard outputs; the source expects `docs/qa/SLICE_SCORECARD.md`.
20. Commit the status ledger separately if practical so later diffs distinguish preflight evidence from implementation.

### Phase 0 DoD

The team can answer, without searching ad hoc: "What file defines the slice?", "What code loads it?", "What executable runs it?", "What build contains it?", "What event IDs measure it?", "What document records human findings?", and "What gate rejects a regression?"

---


## 5. Task 54A — Define, Cut, Freeze, and Export the Seven-Day Slice

### 5.1 Goal

Produce one curated first week that is simultaneously a product scenario, deterministic test fixture, demo entry point, telemetry surface, accessibility target, and release reference. It must not be tuned into an artificial test-only success path.

### 5.2 Required files and integration points

Primary source-named artifacts:

- new `Assets/Ashfall.Core/Campaign/SliceScenario.cs`;
- new `data/slice_seven_days.json`;
- `src/Host/SevenDayDeterministicSmokeTest.cs`;
- `src/Main.GameFlow.cs`;
- `src/Main.Onboarding.cs`;
- `export_presets.cfg`;
- `scripts/ci/export-smoke-boot.sh`;
- `Ashfall.Core.Tests/SliceScenarioTests.cs`;
- `docs/qa/SLICE.md`;
- `docs/design/PILLARS.md`;
- `docs/roadmap/RAILS.md`.

Add helper files only when they prevent duplication. Do not split a small contract into many abstractions merely to create architecture.

### 5.3 Scenario schema contract

The JSON should be explicit enough that the game and test runner cannot disagree about initialization. Recommended shape:

```json
{
  "scenarioId": "slice.seven_day.v1",
  "scenarioVersion": 1,
  "seed": 730541,
  "difficulty": "default",
  "startDay": 1,
  "endDay": 7,
  "roster": [],
  "startingStock": [],
  "startingWorldFlags": [],
  "scriptedEvents": [],
  "beats": [],
  "idealPolicy": [],
  "presentationRequirements": [],
  "telemetryFunnel": []
}
```

The exact serialization naming should follow repository conventions. The contract matters more than this illustrative casing.

### 5.4 Detailed implementation substeps

1. Write the intended week in `docs/qa/SLICE.md` before encoding JSON. For each day, record player-facing intent, required system, trigger, expected readable consequence, and completion evidence.
2. Define day 1 as orientation + feeding + first meaningful ration decision. Do not allow orientation to terminate in a modal tutorial chain that bypasses normal guidance.
3. Define day 2 as crafting + first dispatch. Ensure the dispatch uses the same expedition contract as a full campaign.
4. Define day 3 as weather threat + route choice. The selected route must affect a downstream observable state so attribution can be tested.
5. Define day 4 as injury or dose pressure. Pin the triggering rule so the slice is repeatable while letting the ordinary health/exposure system own consequences.
6. Define day 5 as one death and funeral/aftermath. The death can be scenario-forced only at the world-event boundary; grief, morale, roster, funeral, journal, or related system effects must remain ordinary runtime behavior.
7. Define day 6 as policy choice + grievance. The policy action must exercise the real policy/standing/grievance pipeline.
8. Define day 7 as deadline + resolution. A player should be able to explain which prior decisions contributed to the result.
9. Introduce a `SliceScenario` immutable/read-only runtime representation after validation. Avoid passing raw JSON structures through gameplay code.
10. Implement schema validation for required fields, valid day range, non-empty beat ids, unique beat ids, valid roster/item/event references, valid funnel ids, and valid difficulty id.
11. Validate that every `scriptedEvent` references a real event definition or supported event adapter.
12. Validate that every starting inventory item maps to a real authored item and that quantities satisfy ordinary inventory constraints.
13. Validate roster members through the canonical survivor/character definitions; do not embed full duplicate survivor objects in the slice.
14. Canonicalize scenario data before hashing. Key order, whitespace, and comments must not change the content hash.
15. Store/emit `scenarioId`, `scenarioVersion`, and `scenarioContentHash` in deterministic-run output.
16. Add a scenario-start adapter at the game flow boundary that applies initial state through ordinary domain APIs wherever feasible.
17. Reject direct field mutation when a public gameplay API exists; direct initialization shortcuts easily hide broken integration.
18. Implement an explicit demo/slice startup argument or build flag that chooses the scenario without making the full campaign accidentally enter slice mode.
19. Scope demo saves to a separate root or namespace. A playtester must not overwrite full-campaign saves, and a developer's existing saves must not contaminate results.
20. Remove or disable navigation paths from the demo into a full campaign if the source's demo contract requires a scenario-only build.
21. Add the dedicated demo export preset without altering the semantics of Linux/X11 or Windows Desktop production presets.
22. Ensure the staging/export pipeline copies the same validated data/catalog set used by ordinary exported builds.
23. Add a build manifest beside the demo artifact containing git revision, scenario id/version/hash, platform, timestamp, and gate version.
24. Extend `SevenDayDeterministicSmokeTest` to load the JSON scenario rather than maintain a duplicate hardcoded week.
25. Assert required beats occur in declared order while permitting incidental events that do not invalidate the slice contract.
26. For each required beat, capture a compact evidence record: beat id, day, triggering system, relevant entity ids, and deterministic state digest.
27. Define the digest from stable state only. Exclude wall-clock timestamps, unordered dictionary iteration, transient object ids, log sequence noise, and presentation-only values unless intentionally under test.
28. Run the deterministic scenario repeatedly under the same seed in-process and out-of-process to detect hidden singleton/static contamination.
29. Run at least one alternate seed policy where the scenario supports it; determine which outcomes are invariant and which are allowed to vary.
30. Register the deterministic slice as a fast CI gate using the existing gate-manifest conventions.
31. Add failure fixtures: missing item reference, duplicate beat id, invalid day, unknown difficulty, unknown event, changed content hash, and unreachable required beat.
32. Add a test proving a valid scenario cannot silently skip a required beat and still return success.
33. Add a test proving the test runner and game runtime deserialize the same scenario representation.
34. Add a headless export smoke that launches the actual demo artifact, reaches the scenario-start state, verifies catalogs load, and exits with a machine-readable success result.
35. Make boot smoke fail on missing data, invalid manifest, wrong scenario version, uncaught startup exception, or inability to reach day 1.
36. Decide the default difficulty only through the real 34B contract. Record the chosen preset in `PILLARS.md` as a product expectation.
37. Add a test that the slice cannot silently fall back to another difficulty if the configured default is missing.
38. Wire funnel events for guidance opened, first ration decision, first dispatch, first storm survived, and first death witnessed.
39. Add semantic event IDs at domain/UI boundaries rather than deriving critical telemetry from fragile log text.
40. Assert each funnel event emits at most once per relevant semantic occurrence unless repeated events are intentionally modeled.
41. Add keyboard-only completion automation or the strongest headless/UI harness supported by the repo. It must traverse every required interaction, not merely open panels.
42. Run the same path with reduce-motion enabled and captions enabled. A setting that exists but blocks progression is a failure.
43. Verify focus order, modal dismissal, choice navigation, dispatch confirmation, policy selection, and funeral/death acknowledgement.
44. Add presentation manifest assertions for every slice-required portrait/icon/other source-defined face asset.
45. Add ambience-state assertions for each day/beat where 52A expects an ambience state.
46. Verify the slice has no silent fallback to placeholder presentation assets unless that fallback is explicitly allowed and counted.
47. Add an asset expectation section to `SLICE.md` listing required presentation states by beat, not every asset in the game.
48. Define a freeze policy: changing seed, roster, stock, scripted beats, required presentation, or ideal policy increments scenario version unless explicitly classified as metadata-only.
49. Require changelog entry and scorecard re-baseline for scenario-version changes.
50. Require old scorecards to retain their original scenario id/version/hash so history remains interpretable.
51. Add a `--print-slice-contract` or equivalent debug verb that emits the scenario metadata and required beats without running the week, useful for CI diagnostics.
52. Add structured error messages that identify the failing JSON path/reference rather than returning a generic "scenario invalid."
53. Run the ordinary full campaign startup after adding scenario-start behavior to prove no slice-only condition leaks into production starts.
54. Run save/load during the slice, ideally across at least two day boundaries, and verify restored state preserves pending scripted events and beat progression.
55. Verify replay/reset starts from pristine scenario state and does not carry telemetry/session residue forward.
56. Add docs describing how to intentionally re-baseline the slice, including when **not** to re-baseline (e.g., a regression must be fixed rather than normalized).
57. Run the release checklist against the demo preset and capture final verification commands in the task close-out.

### 5.5 Day-by-day acceptance matrix

| Day | Player-facing obligation | Systems exercised | Minimum machine evidence | Human question |
|---|---|---|---|---|
| 1 | Orient, feed, choose ration posture | guidance, inventory/food, policy/choice UI | guidance event + ration event + state delta | "What did the ration choice change?" |
| 2 | Craft and dispatch | crafting, inventory, expedition | crafted item + dispatch accepted | "How did you know the party was ready?" |
| 3 | Respond to weather and choose route | weather, map/travel, attribution | threat + route + downstream effect | "Why did the consequence happen?" |
| 4 | Handle injury/dose | health/exposure, treatment, UI | exposure/injury state + response | "What would you do to reduce this risk next time?" |
| 5 | Experience death/funeral | mortality, roster, grief/morale, presentation | death + aftermath/funeral beat | "Who died and did it matter to you?" |
| 6 | Make policy choice and see grievance | policy, standing/grievance, feedback | policy id + grievance delta | "Who disliked the decision and why?" |
| 7 | Meet/miss deadline and resolve | objective/deadline, summary/briefing | resolution id + causal summary | "What earlier decision most affected the ending?" |

### 5.6 Failure policy

A slice failure is not automatically a test failure of the same class. Classify it:

- **Contract failure:** malformed scenario, missing reference, version/hash mismatch.
- **Determinism failure:** identical inputs produce incompatible required outcomes/digest.
- **Integration failure:** required system never receives the scenario action.
- **Presentation failure:** system state changes but required cue is absent/incorrect.
- **Accessibility failure:** completion path fails under declared accessibility/input mode.
- **Product-balance finding:** ordinary player fails because the default week is too punishing or too opaque.
- **Protocol finding:** the measurement design itself is insufficient.

Only the first five should normally fail CI immediately. Product-balance findings should fail release only when the scorecard policy says the release bar has been crossed.

### 5.7 54A Definition of Done

54A closes when one stranger-capable demo artifact boots directly into the frozen scenario; the host runner executes the same data; required beats occur in order; the fast gate is registered; version/hash provenance is emitted; default difficulty is explicit; keyboard/reduce-motion/caption paths complete; required presentation assets/states resolve; and the ordinary full-campaign start remains unaffected.

---


## 6. Task 54B — Human Playtest Protocol That Produces Decisions

### 6.1 Goal

Establish a small, repeatable human research process that measures comprehension, friction, and emotional read without turning anecdote into fake statistics. The source's target is six to ten sessions per slice build, with at least six sessions per release for DoD.

### 6.2 Required artifacts

- new `docs/qa/PLAYTEST_PROTOCOL.md`;
- new `docs/qa/sessions/YYYY-MM-DD-NN.md` session records;
- `docs/balance/DECISIONS.md`;
- `docs/telemetry/PRIVACY.md`;
- integration with the 46B local opt-in recorder;
- links from `MANUAL_PLAYTHROUGH_CHECKLIST.md` to actual session evidence;
- optional helper templates under `docs/qa/templates/` if the repository already uses templates.

### 6.3 Protocol design substeps

1. Write protocol version `PT-SLICE-1` before running the first measured session.
2. Define the research question: "Can a person new or relatively new to ASHFALL understand and act through the canonical seven-day slice without developer tutoring?"
3. Define secondary questions: causal attribution, preparedness legibility, emotional engagement, navigation friction, accessibility friction, and perceived fairness.
4. Define participant conditions from the source: new player, returning player, and hostile-to-the-genre/low-affinity player. Do not pretend these are statistically representative strata.
5. Record only the minimum participant descriptors needed to interpret a session; avoid names, precise age, contact details, or other personal data in committed notes.
6. Define session length and stopping rule. If a participant cannot reasonably reach day 7 within the planned session, capture the stop point and reason rather than forcing completion.
7. Define the intervention rule exactly: help only after the agreed stuck threshold (source proposes >3 minutes), except for technical/accessibility emergencies.
8. Every intervention must record timestamp/beat, reason, and what was said at a high level.
9. Define "stuck" operationally: repeated failed action, no progress, explicit request for help, or inactivity while searching for a required affordance.
10. Forbid explanatory priming before the slice. The facilitator may explain controls required to launch the build, privacy/consent, and how to stop the session; they should not explain the game's intended strategy.
11. Use the same build artifact that passed demo boot verification. Never rebuild between CI approval and session without generating a new build identity.
12. Display scenario/build identity in the session record so a finding can be tied to exact code/content.
13. Use passive local telemetry only after informed opt-in. A participant can decline telemetry and still participate with observer notes.
14. Screen capture is separate consent from telemetry. Do not conflate the two.
15. If screen capture is used, store it outside the repository and record only a non-identifying external reference if policy permits; do not commit raw personal recordings.
16. Define note-taking categories: observation, direct participant statement, facilitator inference, severity, beat, system, decision candidate.
17. Never write an inference as an observation. Example: "hovered over three buttons for 40s" is observation; "did not understand crafting" is hypothesis until corroborated.
18. Define severity rubric:
   - S0 observation/no action;
   - S1 minor friction;
   - S2 repeated confusion or non-blocking misread;
   - S3 blocks/derails a required beat without help;
   - S4 critical technical/accessibility/data-loss/privacy issue.
19. Define evidence confidence separately from severity: single-session signal, repeated signal, telemetry-correlated signal, or deterministic reproduction.
20. Record the source's three legibility questions after each session: what did you think was happening on day 3; what did you think would happen if you did X; what did you want and couldn't find.
21. Add one causality question for day 7: "What earlier decision most affected the outcome?"
22. Add one preparedness question for day 2: "What told you your expedition was or was not ready?"
23. Add one health/exposure question for day 4: "What do you think caused the injury/dose problem?"
24. Add emotional read questions after day 5 without demanding a particular emotion: who did you remember/name; what did the death/funeral change for you; did anything feel manipulative or empty.
25. Add policy/grievance comprehension question for day 6: "Who reacted to your policy and why?"
26. Ask the participant to identify one UI element they repeatedly searched for or misread.
27. Ask what they expected the game to let them do but could not do. This is a high-yield source of missing affordances versus intentional scope.
28. Do not interrupt the active play loop with rating prompts unless a specific research question requires it.
29. Create a standardized session record with sections for build metadata, consent flags, start/end, completion, interventions, per-beat observations, funnel summary, interview answers, findings, and redaction check.
30. Add a mandatory de-identification checkbox before a session record may be committed.
31. Create a lint/check script that validates required metadata fields without validating the human content itself.
32. The session validator must reject personally identifying fields if they match obvious forbidden keys such as fullName/email/phone, but documentation must state that automation is not a complete privacy guarantee.
33. Link each finding to one or more stable beat ids.
34. Findings that cannot be tied to a beat may still exist (e.g., global UI issue), but they must name a global system surface and explain why no beat applies.
35. Use a unique finding id such as `PT-2026-09-07-03-F02`.
36. For every finding, record `(observed behavior, beat/system, cause hypothesis, severity, evidence confidence, proposed decision)`.
37. Aggregate repeated findings by semantic issue, not wording. Three participants failing to find the same action should become one issue with three evidence references.
38. Do not average categorical confusion into a meaningless score. Count occurrences and preserve examples.
39. Compare human funnel completion to synthetic baselines only at the same scenario version and build family.
40. Treat synthetic success as a control for system reachability, not proof of human legibility.
41. After each batch, triage every actionable finding into the source-defined buckets: `fix now`, `rails missing`, `design intent disagreement`, or `wont fix, logged`.
42. Write accepted/rejected rationale in `docs/balance/DECISIONS.md`; never let the real decision live only in a chat or issue tracker.
43. Include session ids, affected beat ids, decision owner, target release, and verification method in the decision entry.
44. A rejected finding requires a reason. "Works as designed" is insufficient unless the design intent is linked.
45. A "design intent disagreement" must route to `PILLARS.md` or an equivalent design-authority record if it changes the intended first-week experience.
46. A "rails missing" finding must route to the roadmap intake mechanism established by 53C rather than bypassing it.
47. A `fix now` finding that changes slice data triggers the freeze/version policy; a UI-only correction may or may not require scenario version bump depending on comparability impact.
48. Re-run the deterministic gate after every accepted change even if the finding was purely human-facing.
49. Re-run the relevant session subset when a change targets a repeated S2/S3 comprehension problem.
50. For S4 privacy, data-loss, or technical blockers, suspend affected playtesting until remediated.
51. Replace overclaiming language in `MANUAL_PLAYTHROUGH_CHECKLIST.md`. Each row should distinguish machine verification from last human evidence date/session batch.
52. Add an explicit statement: "No human session recorded" is a valid state; it is better than presenting selftests as human QA.
53. For each release candidate, require the minimum session count only if the candidate is intended for the human-evidence bar. Development/nightly builds can remain automation-only.
54. Preserve raw qualitative notes long enough to make the decision, then follow the privacy policy for retention; do not retain personal recordings indefinitely just because storage is cheap.
55. Create a batch summary that reports participant count, completion count, intervention count, repeated findings, highest severity, and decisions—not vanity satisfaction scores.
56. Perform a facilitator-bias review after the first two sessions. If the facilitator repeatedly explains the same concept, that behavior is itself evidence and the protocol is being contaminated.
57. Add a "protocol deviation" section to every session. Unrecorded deviations invalidate comparability.
58. Version the protocol. A major change in intervention threshold, questions, instrumentation, or participant condition increments protocol version.
59. Record protocol version in scorecard metadata so a later comparison does not silently cross research-method changes.
60. Close the batch only after every S2+ finding has a disposition.

### 6.4 Session record template

```md
# Slice Playtest Session <ID>

- Build revision:
- Build artifact id/hash:
- Scenario id/version/hash:
- Protocol version:
- Participant condition:
- Telemetry consent: yes/no
- Screen capture consent: yes/no/not used
- Started:
- Ended:
- Reached day:
- Completed day 7: yes/no
- Interventions:

## Beat observations
### d1.orientation
Observation:
Participant statement:
Inference:
Intervention:
Finding ids:

...

## Exit interview
1. Day-3 interpretation:
2. Expected consequence of X:
3. Wanted but could not find:
4. Day-7 causality:
5. Emotional read:
6. UI/search friction:

## Findings
- <finding id> ...

## Redaction check
- [ ] No directly identifying data
- [ ] No raw contact information
- [ ] External media reference, if any, follows privacy policy
```

### 6.5 Human evidence acceptance criteria

A playtest batch is valid when:
- all sessions identify the same build/scenario/protocol versions;
- deviations are recorded;
- telemetry consent is explicit;
- session notes are de-identified;
- interventions are visible rather than silently hidden;
- every S2+ finding has a disposition;
- accepted changes point to verification;
- rejected changes have rationale;
- human claims in the checklist cite the batch/session evidence;
- the batch can be compared against machine/synthetic results without merging the evidence types.

### 6.6 54B Definition of Done

At least six valid sessions exist for the release under a written protocol; the records are de-identified; the source's required legibility/emotional questions are answered; repeated problems are consolidated; decisions are committed; the manual checklist distinguishes machines from humans; and the release can state what people actually failed to understand, not merely what automated systems executed.

---


## 7. Task 54C — Make the Slice the Project's Standing Proof

### 7.1 Goal

Promote the slice from a one-time demo into a continuously enforced project instrument. Fast CI proves structural/deterministic integrity; nightly CI measures deeper synthetic/performance behavior; release candidates add human evidence and a generated scorecard.

### 7.2 Gate architecture

Create two source-specified gates:

1. `slice_determinism` — fast tier.
2. `slice_play_metrics` — nightly tier.

Recommended responsibilities:

| Gate | Frequency | Inputs | Fails on |
|---|---|---|---|
| `slice_determinism` | every relevant push/PR | frozen scenario + host runtime | schema/reference failure, missing beat, ordering/digest regression, load mismatch |
| `slice_play_metrics` | nightly / release candidate | scenario + synthetic policies + perf harness | unreachable beat, threshold regression, perf floor breach, metric/schema failure |

Human-session presence should normally be a release-candidate/release gate concern, not a fast developer gate.

### 7.3 Scorecard contract

`docs/qa/SLICE_SCORECARD.md` must be generated. Never manually edit metric values.

Required metadata:
- generated-at timestamp;
- git revision;
- build artifact identity;
- scenario id/version/hash;
- protocol version if human evidence is included;
- threshold policy version;
- deterministic digest;
- platform/runtime;
- generator version.

Required metric families:
- scenario completion/reachability;
- beat ordering and missing beats;
- time/turns to first meaningful decision;
- first-death day under named synthetic policies;
- funnel discovery/drop points;
- unbound-port/dead-definition counts if supplied by the relevant sweep harness;
- snapshot/presentation diffs if the existing infrastructure exposes them;
- performance floor from the canonical slice run;
- human sessions count;
- human completion count;
- intervention count;
- repeated S2/S3 findings;
- open release-blocking findings;
- accessibility completion status.

### 7.4 Detailed implementation substeps

1. Add both gates to `docs/ci/CI_GATE_MANIFEST.json` using the existing manifest schema; do not create a second manifest.
2. Set clear ownership and expected runtime.
3. Make fast gate output concise on success and diagnostic on failure.
4. Fast gate must print scenario metadata and the first failed/missing beat.
5. Fast gate must fail if scenario data changed without an expected hash/version update according to freeze policy.
6. Nightly gate must run named synthetic policies rather than one opaque "bot."
7. At minimum define `ideal`, `naive`, and `adverse`/mistake-tolerant policies if compatible with 46A/46B design.
8. Record policy version because a stronger synthetic policy can make a build look better without game changes.
9. Use a small fixed seed matrix for trend comparability and optionally a rotating exploratory seed set for discovery.
10. Keep fixed-seed metrics separate from exploratory-seed findings.
11. Assert every required beat is reachable under at least the intended/ideal policy.
12. Treat unreachable required beats as structural release blockers, not balance noise.
13. Define the "shippable first week" bar in a versioned machine-readable threshold file plus human-readable rationale.
14. Do not choose thresholds simply to make the current build green. Baseline first, then make an explicit product decision.
15. Example threshold categories: naive day-7 completion, earliest acceptable first-death distribution, max required-beat miss rate, minimum funnel discovery, max intervention rate for human release batch, performance budget.
16. Keep thresholds scenario-version scoped.
17. If a scenario version intentionally becomes harder or more opaque, revise thresholds through a design decision rather than silently re-baselining.
18. Implement `scripts/ci/generate-slice-scorecard.py` or repository-language equivalent.
19. Generator reads machine outputs, does not rerun arbitrary tests implicitly unless explicitly designed as orchestrator.
20. Make machine outputs structured (JSON recommended) and keep human session aggregation deterministic.
21. Scorecard generation must fail on missing required metric, duplicate session id, incompatible scenario version, or malformed source output.
22. Scorecard generation must not treat missing human sessions as zero findings. Render "no human evidence for this build" explicitly.
23. Add a `--check` mode that regenerates to a temp path and fails if the committed/generated artifact is stale, if the repository intentionally commits it.
24. Avoid embedding volatile wall-clock data in comparisons if that makes every run dirty; separate display metadata from comparison payload where useful.
25. Add trend storage or release snapshots so scorecards across releases can be compared.
26. Generate a small "delta from previous comparable release" section using only matching scenario/protocol/threshold policy unless an explicit compatibility note permits otherwise.
27. Integrate performance data from the source's 26C dependency using the slice as canonical perf scenario.
28. Capture wall-clock time, allocations/GC or relevant engine metrics only if the existing perf harness supports them reliably.
29. Define performance failures as budgets with warmup/variance policy; do not gate on single noisy samples.
30. Integrate unbound-port/dead-definition/snapshot metrics only from existing authoritative scanners. Do not reimplement them in the scorecard generator.
31. Add accessibility result status: keyboard completion, reduce-motion completion, caption path.
32. Add build-boot result status from the exported demo smoke.
33. Add a release-gate stage that checks scorecard thresholds.
34. Make any threshold failure produce a non-zero exit with metric name, actual, threshold, and comparable baseline.
35. Implement waiver support only if release policy requires it. Waivers need id, metric, reason, owner, pillar/decision link, creation date, expiry, and maximum releases.
36. A waiver must never turn a missing scenario/manifest/privacy contract into success.
37. Keep privacy/S4 findings non-waivable unless the project's policy explicitly allows a reviewed exception.
38. Update `.github/workflows/ci.yml` to schedule nightly metrics and upload structured artifacts/logs according to repository convention.
39. Ensure local commands reproduce CI behavior so developers can diagnose without pushing.
40. Add a CI time budget. The source expects the seven-day deterministic harness to remain cheap.
41. If nightly runtime grows beyond budget, optimize orchestration or seed count before removing core coverage.
42. Add tests for scorecard completeness.
43. Add tests for threshold enforcement with fixtures just above, at, and below each boundary.
44. Add tests for missing/incompatible human evidence.
45. Add tests for scenario-version drift.
46. Add tests for policy-version drift.
47. Add tests proving stale scorecard data cannot satisfy the release gate.
48. Add tests proving a manually edited metric is overwritten or rejected by regeneration.
49. Update `MANUAL_PLAYTHROUGH_CHECKLIST.md` to cite both machine gates and the most recent human batch where applicable.
50. Add source-required "worst moments" release note generation or a curated section fed by accepted findings; clearly distinguish generated metrics from editorial text.
51. Add the slice score to 53A's plan register / wave ledger so future work sees the current first-week health.
52. Add an intake rule: a feature proposal that claims first-week value must identify which slice beat or metric it improves.
53. Do not reject features solely because they do not affect the slice; instead use the slice as a relevance/priority filter for first-week claims.
54. Add release close-out commands to the wave ledger.
55. Capture the final scorecard in the release artifact set from 48B.
56. Preserve old release scorecards as immutable historical records.
57. Add a changelog line for every scenario version.
58. Add a deprecation policy for old scenario versions; deprecation means no new gate runs, not deletion of historical interpretation.
59. Define how long the canonical scenario remains frozen before deliberate redesign—e.g., across a release train, not forever.
60. Conduct one deliberate red-team exercise: intentionally break a required beat, funnel emission, scorecard input, threshold, and demo data deployment; prove the appropriate gate fails each time.

### 7.5 Release decision matrix

| Condition | Fast gate | Nightly | Human evidence | Release result |
|---|---:|---:|---:|---|
| missing required beat | fail | fail | irrelevant | block |
| demo does not boot | may pass host | fail/release smoke fail | cannot validly run | block |
| synthetic reachability healthy, humans confused repeatedly | pass | pass | S2/S3 repeated | decision required; often block until disposition |
| humans succeed, deterministic digest drifts unexpectedly | fail | fail | not enough | block |
| threshold metric slightly regresses with approved pillar change | pass | fail threshold | evidence present | waiver/design decision only |
| no human sessions for ordinary dev build | pass | pass | none | dev build allowed |
| no human sessions for release requiring human bar | pass | pass | missing | block |
| privacy/S4 issue open | pass | pass | critical | block |

### 7.6 54C Definition of Done

The canonical first week has a generated scorecard, fast and nightly gates, release thresholds, provenance, historical comparability rules, human-evidence integration, and a release gate that demonstrably rejects a broken slice. Every release can state what improved, what regressed, what humans struggled with, and what exception—if any—was intentionally accepted.

---


## 8. Integration-Derived Next Tasks After 54C

The source stops at 54C. The following tasks are deliberate D1 extensions: they are not claims about pre-existing Plan 54 numbering. They exist because operating the slice as a standing proof creates new integration surfaces that should be hardened rather than left implicit.

These tasks should begin only after the basic 54A–54C path is working. They are ordered to reduce the risk of building analytics/process sophistication on top of an unstable scenario contract.

---

## 9. Task 54D — Scenario Contract Hardening and Migration Safety
**Classification:** Integration-derived follow-on.

**Outcome:** The scenario behaves like a versioned compatibility contract, not a mutable test fixture.

### Substeps
1. Extract a formal schema/version contract for `slice_seven_days.json` using the repository's existing validation approach; avoid introducing a second schema technology unless justified.
2. Define which fields are identity-bearing, behavior-bearing, presentation-bearing, and metadata-only. Document which classes force a scenario version increment.
3. Add forward-compatible parsing only where repository policy supports it. Unknown behavior-bearing fields should fail closed rather than be silently ignored.
4. Add a canonicalization routine and golden fixtures proving whitespace/key order do not alter content hash.
5. Add migration tests from every retained historical slice version to the current loader when migrations are intentionally supported.
6. Do not mutate historical scorecards during migrations. They describe what was tested at the time.
7. Add reference graph validation for roster ids, item ids, event ids, policy ids, locations, broadcasts, ambience states, and any authored catalog ids consumed by the slice.
8. Detect orphaned scenario references during normal catalog validation so content refactors break loudly.
9. Add a command that prints a dependency graph for the slice: each beat → systems → referenced authored definitions.
10. Use that graph in code review to detect unexpectedly large blast radius from a first-week edit.
11. Add a mutation-test suite that deletes or renames one referenced definition at a time and proves validation catches it.
12. Add a scenario-state capture immediately after initialization and compare host/runtime captures for structural parity.
13. Add save/load parity at day 2, day 4, and day 6; verify pending scripted events are neither duplicated nor lost.
14. Add reset/restart parity proving session-scoped flags, random streams, telemetry counters, and temporary UI state do not contaminate a new run.
15. Add an explicit end-of-slice terminal state so day 8 cannot accidentally continue into undefined demo content.
16. Define behavior if the player reaches a normally valid full-campaign action that the demo intentionally disables; surface a product-consistent message rather than a dead control.
17. Add platform-neutral line endings/encoding rules for scenario data and hashes.
18. Add schema diagnostics that identify JSON pointer/path, invalid value, and expected reference domain.
19. Add a codeowner/authority note so future plans know where the slice contract may be changed.
20. Close 54D only after a deliberately corrupted scenario produces precise failures in validation, host runner, and exported boot smoke without ambiguous downstream exceptions.

### 54D Verification focus

- prove the new contract through at least one positive test and one deliberate-failure test;
- integrate with existing manifests/scripts rather than creating a parallel authority;
- update `SLICE.md`, the scorecard contract, or release documentation only where the task changes their semantics;
- rerun `slice_determinism`, relevant nightly metrics, exported demo boot, and full-campaign regression checks after completion;
- record any scenario-version or threshold-policy change explicitly.

**DoD:** The scenario behaves like a versioned compatibility contract, not a mutable test fixture.

---

## 10. Task 54E — Failure-Path and Player-Mistake Coverage
**Classification:** Integration-derived follow-on.

**Outcome:** The slice proves resilience and feedback quality under mistakes, not merely success under an ideal script.

### Substeps
1. Create a matrix of plausible first-week mistakes rather than scripting one ideal route only.
2. Include the source-named failures: ignore guidance, dispatch an unready party, refuse ration cuts, and leave someone untreated.
3. Add at least one inventory-starvation mistake, one route/weather misread, one policy/grievance misread, and one UI cancellation/reversal path.
4. Separate recoverable mistakes from terminal mistakes. A survival game may punish errors, but the player must receive legible causal feedback.
5. Create synthetic policies that intentionally choose these mistakes through public gameplay actions.
6. Prove every recoverable mistake has at least one discoverable recovery action unless the design explicitly intends otherwise.
7. Prove terminal failure resolves to a coherent game state rather than soft lock, endless modal, null expedition, or unfinishable deadline.
8. Instrument cause-of-failure codes using stable semantic identifiers.
9. Ensure cause-of-failure presentation does not reveal hidden simulation details that undermine game design; telemetry may be more precise than player-facing copy.
10. Add tests for dispatch validation and the presentation of why a party is unready.
11. Add tests for ration refusal consequences across the day boundary.
12. Add tests for untreated injury/dose progression and eventual consequence attribution.
13. Add tests for attempting a policy action with insufficient prerequisites or conflicting state.
14. Add tests for saving/loading during a failure path.
15. Add keyboard-only recovery tests for every recoverable path selected for the matrix.
16. Run at least two human sessions with a facilitator instruction not to optimize, to observe whether organic mistakes match the synthetic matrix.
17. Promote newly observed high-frequency mistakes into named policies only after they repeat; do not overfit to one participant.
18. Add scorecard fields for terminal-state reason distribution under synthetic adverse policies.
19. Set a release rule: a newly introduced soft lock or opaque terminal state is a blocker even if the ideal policy still completes.
20. Close 54E when the first week is robust not only to the 'correct' playthrough but also to predictable human errors.

### 54E Verification focus

- prove the new contract through at least one positive test and one deliberate-failure test;
- integrate with existing manifests/scripts rather than creating a parallel authority;
- update `SLICE.md`, the scorecard contract, or release documentation only where the task changes their semantics;
- rerun `slice_determinism`, relevant nightly metrics, exported demo boot, and full-campaign regression checks after completion;
- record any scenario-version or threshold-policy change explicitly.

**DoD:** The slice proves resilience and feedback quality under mistakes, not merely success under an ideal script.

---

## 11. Task 54F — Telemetry Integrity, Privacy, and Replay Correlation
**Classification:** Integration-derived follow-on.

**Outcome:** Slice telemetry is trustworthy, minimal, opt-in, and correlatable with qualitative evidence.

### Substeps
1. Inventory every telemetry event consumed by the slice scorecard and mark its producer, payload, cardinality, consent requirement, and retention.
2. Enforce a minimal payload. Prefer stable ids, day/beat, coarse outcome, and build/scenario metadata over free-text or personal data.
3. Add event-schema validation at build/test time.
4. Add a telemetry contract version independent from scenario version.
5. Prevent duplicate semantic events caused by UI reopen, scene reload, save/load, or signal re-subscription.
6. Use session-local event sequence ids if needed for debugging, but do not treat sequence ids as cross-session identity.
7. Add a deterministic replay correlation id generated from non-personal session/build data or random local token; document its privacy properties.
8. Prove declining telemetry produces no telemetry file and does not alter gameplay.
9. Prove opting in mid-flow, if supported, does not backfill prior actions without explicit design.
10. Prove opting out/ending session closes writers cleanly and avoids corrupt partial files.
11. Validate that scorecard aggregation ignores unrelated full-campaign telemetry.
12. Validate that malformed or partial telemetry is reported as incomplete evidence, not counted as zero behavior.
13. Add redaction tests for any free-text fields that cannot be removed.
14. Keep participant session record ids separate from device/user identity.
15. Document retention and deletion behavior in `docs/telemetry/PRIVACY.md`.
16. Add a human-readable export/viewer so researchers can inspect events without opening implementation internals.
17. Add a replay timeline that aligns telemetry events to slice beat ids and session-note timestamps at coarse resolution.
18. Use correlation to diagnose disagreement: e.g., participant says they never saw guidance while event stream says it opened; investigate presentation/attention rather than declaring the participant wrong.
19. Add privacy gate checks that forbidden keys do not appear in committed sample telemetry.
20. Close 54F when telemetry is useful for product evidence without being necessary for participation and without collecting identity.

### 54F Verification focus

- prove the new contract through at least one positive test and one deliberate-failure test;
- integrate with existing manifests/scripts rather than creating a parallel authority;
- update `SLICE.md`, the scorecard contract, or release documentation only where the task changes their semantics;
- rerun `slice_determinism`, relevant nightly metrics, exported demo boot, and full-campaign regression checks after completion;
- record any scenario-version or threshold-policy change explicitly.

**DoD:** Slice telemetry is trustworthy, minimal, opt-in, and correlatable with qualitative evidence.

---

## 12. Task 54G — Accessibility Evidence Pack for the Canonical Week
**Classification:** Integration-derived follow-on.

**Outcome:** Accessibility claims become concrete evidence tied to the playable slice.

### Substeps
1. Convert keyboard-only, reduce-motion, and captions from checkboxes into named slice completion evidence.
2. Create an interaction inventory for every required beat: focusable controls, shortcuts, modal ownership, scrolling, confirmation, and cancellation.
3. Add a deterministic focus-order snapshot or structural test where the UI framework permits it.
4. Verify no required action depends solely on hover, pointer precision, color, animation timing, or audio.
5. Verify captions cover all audio information required to make a gameplay decision during the slice.
6. Verify reduce-motion does not hide state transitions or delay/skip required callbacks.
7. Test UI at the repository's supported scaling/text-size options if such options exist.
8. Test choice lists at the largest supported content strings relevant to the slice to catch clipping/wrapping.
9. Test the death/funeral sequence specifically for focus traps and forced timing.
10. Test route and policy choices for keyboard discoverability and clear selected state.
11. Test dispatch preparation for non-pointer inspection of readiness information.
12. Add an accessibility session condition for at least one release batch when feasible; do not claim broad accessibility from automation alone.
13. Record accessibility blockers as S4/S3 depending on whether they prevent the declared supported mode.
14. Add accessible-name/label checks where the UI technology exposes them.
15. Add screenshot or structural artifacts only when stable enough to be useful; avoid brittle pixel-only gates for semantic accessibility.
16. Document the supported accessibility contract in `SLICE.md` and link to the broader design rules.
17. Ensure the demo starts with saved accessibility preferences if a player configured them previously, while the test harness can explicitly force modes.
18. Add a release scorecard section that clearly says `keyboard`, `reduce motion`, `captions`: pass/fail/not tested.
19. Never render 'not tested' as pass.
20. Close 54G when the canonical week can be completed under each declared mode and failures are visible in the standing scorecard.

### 54G Verification focus

- prove the new contract through at least one positive test and one deliberate-failure test;
- integrate with existing manifests/scripts rather than creating a parallel authority;
- update `SLICE.md`, the scorecard contract, or release documentation only where the task changes their semantics;
- rerun `slice_determinism`, relevant nightly metrics, exported demo boot, and full-campaign regression checks after completion;
- record any scenario-version or threshold-policy change explicitly.

**DoD:** Accessibility claims become concrete evidence tied to the playable slice.

---

## 13. Task 54H — Demo Artifact Provenance and Cross-Platform Boot Parity
**Classification:** Integration-derived follow-on.

**Outcome:** There is no ambiguity about what code/content a human actually played.

### Substeps
1. Define a build manifest format shared by Linux and Windows demo artifacts.
2. Embed git revision, scenario id/version/hash, catalog/data manifest hash, build configuration, and export preset name.
3. Name artifacts predictably without relying on mutable 'latest' directories.
4. Run headless or scripted boot verification on every supported demo platform available in CI.
5. Verify the demo reads data from the packaged artifact, not from a developer checkout path.
6. Test missing/corrupted data behavior and require clear startup failure.
7. Test read-only installation directory behavior so save/log paths correctly use writable user storage.
8. Test paths with spaces and non-ASCII characters where supported by the release environment.
9. Verify locale does not change deterministic parsing of numeric/date-like data.
10. Verify scenario content hash inside the running build matches the build manifest.
11. Add a startup screen/debug overlay accessible to testers that displays build/scenario ids without exposing developer-only controls.
12. Make session records copy the build identity from the running artifact rather than manual typing where possible.
13. Archive boot logs as CI artifacts for failed builds.
14. Add a minimal startup timeout and a distinct exit code for scenario-load failure versus engine boot failure.
15. Verify quitting/relaunching the demo does not start from an unintended prior save unless resume behavior is explicitly selected.
16. Verify uninstall/reinstall/reset instructions for playtesters are documented.
17. Run one artifact on a clean machine/container/VM that does not contain the repository.
18. Prevent release distribution of an artifact whose build manifest does not match the scorecard revision.
19. Add release-gate verification of artifact checksum.
20. Close 54H when the exact build humans receive is cryptographically/procedurally tied to the revision and scenario that CI scored.

### 54H Verification focus

- prove the new contract through at least one positive test and one deliberate-failure test;
- integrate with existing manifests/scripts rather than creating a parallel authority;
- update `SLICE.md`, the scorecard contract, or release documentation only where the task changes their semantics;
- rerun `slice_determinism`, relevant nightly metrics, exported demo boot, and full-campaign regression checks after completion;
- record any scenario-version or threshold-policy change explicitly.

**DoD:** There is no ambiguity about what code/content a human actually played.

---

## 14. Task 54I — Balance Calibration Without Slice Overfitting
**Classification:** Integration-derived follow-on.

**Outcome:** The slice influences balance while remaining a truthful sample of the actual game.

### Substeps
1. Establish baseline metrics before tuning: completion, death day distribution, resource trajectories, dispatch outcomes, treatment/dose state, and deadline outcomes.
2. Separate scenario-fixed inputs from global balance parameters.
3. Prohibit hidden slice-only buffs/debuffs unless they are explicitly part of the scenario contract and product intent.
4. Use 46A-style sweeps across named policies and a fixed seed matrix.
5. Track resource minima and failure causes, not only final completion.
6. Identify knife-edge parameters where tiny changes flip most runs.
7. Flag parameters whose safe slice values create implausible full-campaign behavior; the slice is a product filter, not the entire balance model.
8. Use human confusion findings to change presentation before changing difficulty when the underlying choice is fair but unreadable.
9. Use human frustration/fairness evidence alongside synthetic survival data when deciding difficulty.
10. Keep the default preset the calibration target; do not tune an easier hidden demo difficulty.
11. Define acceptable outcome diversity: the slice should permit meaningful variation without making required beats unreachable.
12. Ensure day-5 death/funeral remains narratively intentional; do not accidentally eliminate the beat because optimization avoids all mortality.
13. Ensure day-7 resolution can reflect multiple prior decision paths if the design intends replayability.
14. Add balance decisions to `DECISIONS.md` with metric evidence and scenario version.
15. Run post-change regression across full-campaign representative tests to detect local overfitting.
16. Keep thresholds stable across small tuning changes; revise them only when product goals change.
17. Record confidence and sample size for human-derived balance conclusions.
18. Do not infer population-level percentages from six to ten sessions; treat them as discovery evidence.
19. Add a 'tuning debt' list for parameters intentionally deferred.
20. Close 54I when the default first week is neither engineered solely to pass nor allowed to fail for reasons the player cannot read.

### 54I Verification focus

- prove the new contract through at least one positive test and one deliberate-failure test;
- integrate with existing manifests/scripts rather than creating a parallel authority;
- update `SLICE.md`, the scorecard contract, or release documentation only where the task changes their semantics;
- rerun `slice_determinism`, relevant nightly metrics, exported demo boot, and full-campaign regression checks after completion;
- record any scenario-version or threshold-policy change explicitly.

**DoD:** The slice influences balance while remaining a truthful sample of the actual game.

---

## 15. Task 54J — Presentation and Attribution Regression Harness
**Classification:** Integration-derived follow-on.

**Outcome:** Attribution becomes a tested cross-layer contract linking simulation state to human understanding.

### Substeps
1. Enumerate every state transition in the seven-day slice that requires player-facing attribution.
2. For each transition, identify its presentation surface: briefing, tooltip, log, journal, panel delta, icon, ambience, audio cue, or combination.
3. Create a machine-readable attribution expectation map keyed by beat/state effect.
4. Validate required presentation surfaces exist and are bound to live data.
5. Detect stale copy that names an old mechanic, cost, location, or consequence.
6. Add tests that effect attribution references the actual cause id rather than a generic fallback when a specific cause exists.
7. Exercise day-3 route/weather attribution and day-6 policy/grievance attribution as priority cases from the source.
8. Exercise day-4 injury/dose attribution across save/load.
9. Exercise day-7 resolution summary and confirm it does not cite choices the player did not make.
10. Add snapshot/structural checks for critical panels using the project's existing UI testing approach.
11. Count placeholder/missing assets in the slice and surface them in the scorecard.
12. Verify ambience state transitions do not lag behind day/beat state.
13. Verify captions/transcripts match required gameplay-relevant audio cues.
14. Run a stale-binding fault injection: disconnect one expected UI binding and prove the harness detects it.
15. Run a stale-data fault injection: mutate a consequence after the UI string is generated and ensure the chosen architecture does not present contradictory values.
16. Use human interview answers to validate the effectiveness of attribution—not only presence.
17. Tag repeated misattribution findings to the exact expectation map entry.
18. Prioritize fewer clear causal cues over duplicative popups across many surfaces.
19. Document intentional ambiguity where the game design wants uncertainty; exclude those cases from strict causal explanation gates but keep consistency checks.
20. Close 54J when the game can explain the first week's important consequences without relying on the developer to narrate them.

### 54J Verification focus

- prove the new contract through at least one positive test and one deliberate-failure test;
- integrate with existing manifests/scripts rather than creating a parallel authority;
- update `SLICE.md`, the scorecard contract, or release documentation only where the task changes their semantics;
- rerun `slice_determinism`, relevant nightly metrics, exported demo boot, and full-campaign regression checks after completion;
- record any scenario-version or threshold-policy change explicitly.

**DoD:** Attribution becomes a tested cross-layer contract linking simulation state to human understanding.

---

## 16. Task 54K — Playtest Operations and Decision Throughput
**Classification:** Integration-derived follow-on.

**Outcome:** Human evidence moves through a controlled funnel from session to decision to verified change.

### Substeps
1. Create a release-batch checklist from recruitment through decision close-out.
2. Automate generation of blank session-record filenames/ids to avoid duplicates.
3. Automate insertion of build/scenario metadata where possible.
4. Create a small local report that aggregates session records without uploading them to a third-party service.
5. Track sessions planned, valid, excluded, and reason for exclusion.
6. Define exclusion rules before looking at results: technical invalidation, protocol deviation, consent withdrawal, wrong build, corrupted session.
7. Never discard a valid negative session because the player 'played wrong.'
8. Create a repeated-finding index keyed by semantic issue and beat.
9. Require every repeated S2+ finding to have an owner and target decision date/release.
10. Add decision aging: unresolved release-impacting findings become visible debt.
11. Distinguish product decisions from implementation tickets; one decision may generate several code tasks.
12. Link implementation PR/commit ids back to the decision entry where workflow supports it.
13. After fixes, mark findings as `candidate resolved` until verified in a later machine/human run.
14. Create a regression tag when a previously resolved human finding reappears.
15. Generate a short playtest batch summary suitable for release close-out.
16. Keep facilitator training notes in the protocol so a second facilitator can run comparable sessions.
17. Perform periodic protocol calibration by having two observers independently code one session and compare finding classification.
18. Keep emotional/narrative findings visible even when they do not map to a simple metric.
19. Define a cap on open 'wont fix, logged' findings that require periodic review if they recur.
20. Close 54K when playtesting reliably produces closed-loop decisions instead of an ever-growing anecdote backlog.

### 54K Verification focus

- prove the new contract through at least one positive test and one deliberate-failure test;
- integrate with existing manifests/scripts rather than creating a parallel authority;
- update `SLICE.md`, the scorecard contract, or release documentation only where the task changes their semantics;
- rerun `slice_determinism`, relevant nightly metrics, exported demo boot, and full-campaign regression checks after completion;
- record any scenario-version or threshold-policy change explicitly.

**DoD:** Human evidence moves through a controlled funnel from session to decision to verified change.

---

## 17. Task 54L — Release Governance, Waivers, and Historical Comparability
**Classification:** Integration-derived follow-on.

**Outcome:** The slice becomes durable release governance rather than a one-release novelty.

### Substeps
1. Define the authority that may approve a slice threshold waiver.
2. Create a versioned waiver schema with metric, actual, threshold, reason, owner, design-decision link, issue date, expiry, and max release count.
3. Require the release gate to print active waivers.
4. Reject expired waivers automatically.
5. Reject waivers for missing scenario identity, corrupt artifact, invalid scorecard, privacy blocker, or incompatible evidence.
6. Store threshold policy version with every scorecard.
7. Store protocol version with human aggregates.
8. Define comparability classes: directly comparable, comparable with note, incompatible.
9. Make cross-release trend generation refuse incompatible comparisons by default.
10. Document intentional scenario redesigns as new baselines rather than pretending the trend is continuous.
11. Preserve old scorecards and build manifests as immutable release evidence.
12. Add a release close-out section: what changed, scorecard delta, human worst moments, accepted regressions, active waivers, next action.
13. Add a roadmap input that references repeated slice regressions across releases.
14. Add an audit test that each released scenario version has a changelog entry and scorecard.
15. Add an audit test that each scorecard references an existing threshold policy version.
16. Ensure generated docs distinguish data generated by tools from editorial interpretation.
17. Periodically retire metrics that no longer influence decisions; unused metrics create false process weight.
18. Add new metrics only with an owner and decision question.
19. Run a dry-run release where one metric is intentionally below threshold and verify governance, not just code, handles it correctly.
20. Close 54L when release exceptions are explicit, temporary, attributable, and historically understandable.

### 54L Verification focus

- prove the new contract through at least one positive test and one deliberate-failure test;
- integrate with existing manifests/scripts rather than creating a parallel authority;
- update `SLICE.md`, the scorecard contract, or release documentation only where the task changes their semantics;
- rerun `slice_determinism`, relevant nightly metrics, exported demo boot, and full-campaign regression checks after completion;
- record any scenario-version or threshold-policy change explicitly.

**DoD:** The slice becomes durable release governance rather than a one-release novelty.

---

## 18. Task 54M — First-Week Product Feedback Loop Into Waves 9+
**Classification:** Integration-derived follow-on.

**Outcome:** Future waves use the slice as evidence without mistaking seven days for the whole game.

### Substeps
1. Add the latest slice scorecard summary to the plan register/current-authority surface.
2. Require new first-week feature proposals to name affected beat(s), metric(s), or repeated human finding(s).
3. Classify roadmap proposals as `slice critical`, `slice relevant`, `post-slice`, or `non-slice systemic`; do not force all work into the first week.
4. Prioritize repeated S3 blockers ahead of new first-week content unless a pillar decision says otherwise.
5. Use the source's observation that content supply is already large as a guardrail against solving wiring/legibility problems with more content.
6. Track whether a proposed feature increases the slice's cognitive load, UI surface count, or required decisions.
7. Require an explicit removal/simplification consideration for each new first-week interaction.
8. After every major wave, rerun 54A/54C gates and schedule a human batch when the change materially affects comprehension or emotional read.
9. Keep scenario version stable for implementation-only changes where comparability remains valid.
10. Create a quarterly or milestone review of the slice itself: does it still represent the intended game?
11. If the first week fundamentally changes, design a new slice version deliberately and preserve the old one for historical comparison.
12. Use failures to discover missing rails/system integration, not only local bugs.
13. Promote systemic findings out of the slice into broader repository remediation plans.
14. Track the percentage of first-week regressions caught by fast, nightly, human, or release review to understand which instrument is providing value.
15. Retire redundant checks only when another instrument proves it covers the same failure class.
16. Ensure human emotional read does not become a gate with fabricated numeric certainty; preserve qualitative decision records.
17. Use the slice as a demonstration artifact for external testers only after privacy/build provenance rules are satisfied.
18. Document what the slice does **not** prove: long-horizon economy, late-game content, campaign-scale save growth, rare-event distribution, and other post-day-7 behaviors.
19. Create separate longer-horizon instruments rather than stretching the seven-day slice until it becomes slow and unfocused.
20. Close 54M when the seven-day proof actively shapes prioritization while remaining bounded to what it can validly measure.

### 54M Verification focus

- prove the new contract through at least one positive test and one deliberate-failure test;
- integrate with existing manifests/scripts rather than creating a parallel authority;
- update `SLICE.md`, the scorecard contract, or release documentation only where the task changes their semantics;
- rerun `slice_determinism`, relevant nightly metrics, exported demo boot, and full-campaign regression checks after completion;
- record any scenario-version or threshold-policy change explicitly.

**DoD:** Future waves use the slice as evidence without mistaking seven days for the whole game.

---


## 19. Cross-Task Dependency Graph

```text
17B guidance ───────────────┐
31B attribution ────────────┼────► 54A canonical slice
34B default difficulty ─────┤
37B/37C accessibility ──────┤
50A/52A presentation ───────┘
26B export boot ─────────────────► 54A demo artifact
46A/46B metrics ─► 54A telemetry ─► 54B human evidence ─► 54C scorecard
48B release artifacts ────────────────────────────────► 54C provenance
53B/53C pillars/intake ───────────────────────────────► 54B/54C decisions

54A ─► 54D contract hardening ─┐
    ├► 54E mistake coverage    │
    ├► 54G accessibility      ├► 54C/54L release proof ─► 54M Waves 9+
    ├► 54H artifact identity  │
    └► 54J attribution ───────┘
54B ─► 54F telemetry integrity ─► 54K playtest operations ─► 54I balance
```

The operational critical path is:

`Preflight → 54A scenario/demo → 54B protocol + ≥6 valid sessions → 54C scorecard/gates`

54D–54M harden and scale the instrument after the end-to-end path exists. Prioritize 54D, 54F, 54G, and 54H first because scenario identity, evidence integrity, accessibility, and artifact provenance protect every later conclusion.

---

## 20. Verification Strategy

Preserve the source's verification intents, using the repository's actual final verb names:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- <7day-slice-verb>
bash scripts/ci/export-smoke-boot.sh
python scripts/ci/generate-slice-scorecard.py --check
bash scripts/ci/release-gate.sh
bash scripts/ci/verify-fast.sh
```

Test layers:

1. **Contract:** scenario/schema/reference/hash/version/threshold/telemetry/session validation.
2. **Domain integration:** initialization through ordinary APIs, beat triggers, day transitions, failure paths, save/load.
3. **Host deterministic:** seven-day run, ordering, digest, fixed seeds, synthetic policies.
4. **Game runtime:** scenario start, input/focus, presentation, ambience, captions/reduce-motion, full-campaign regression.
5. **Exported artifact:** clean boot, packaged data, writable saves, manifest identity.
6. **Human evidence:** protocol, consent, de-identification, interventions, comprehension, emotional read, decisions.
7. **Release governance:** scorecard, thresholds, stale-evidence rejection, waiver expiry, historical comparability.

### Deliberate-break suite

Before closing D1, intentionally and separately:
- delete one required scenario reference;
- duplicate one beat id;
- change scenario content without appropriate version handling;
- suppress one required day-3 consequence;
- suppress one funnel event;
- disconnect one attribution binding;
- remove one packaged catalog;
- mismatch the artifact's scenario hash;
- provide malformed telemetry;
- add a session record from an incompatible scenario version;
- break keyboard reachability of one required action;
- push one nightly metric below its threshold;
- activate an expired waiver;
- stale the generated scorecard;
- break ordinary campaign startup while leaving slice startup green.

Record which test/gate rejects each fault. A deliberately broken case that passes is itself a release-blocking coverage gap.

---

## 21. Canonical Scorecard Shape

```md
# Seven-Day Slice Scorecard — <release>

## Provenance
Revision:
Build artifact:
Scenario id/version/hash:
Threshold policy:
Telemetry schema:
Playtest protocol:
Comparable previous release:

## Structural
Scenario valid:
Required beats:
Deterministic digest:
Export boot:

## Synthetic
Ideal completion:
Naive completion:
Adverse-policy terminal causes:
First-death distribution:
Funnel discovery:
Unreachable beats:

## Performance
Canonical slice budget status:

## Accessibility
Keyboard-only:
Reduce motion:
Captions:

## Human evidence
Valid sessions:
Day-7 completions:
Interventions:
Repeated S2:
Repeated S3:
Open S4:
Top repeated findings:

## Integration
Missing required presentation:
Attribution failures:
Authoritative dead-def/unbound-port metrics if available:

## Decision
Failed thresholds:
Active waivers:
Human blockers:
Result: PASS / BLOCK / PASS WITH APPROVED WAIVER
```

Every populated value needs an authoritative producer. Missing human evidence must render as `NO HUMAN EVIDENCE`, never as zero findings.

---

## 22. Documentation Pack

D1 should leave these authorities:

- `docs/qa/SLICE.md` — canonical week, beat intent, version/freeze/re-baseline rules.
- `docs/qa/PLAYTEST_PROTOCOL.md` — protocol and facilitator rules.
- `docs/qa/sessions/` — de-identified session records.
- `docs/qa/SLICE_SCORECARD.md` — generated current scorecard.
- `docs/telemetry/PRIVACY.md` — consent, minimization, retention.
- `docs/balance/DECISIONS.md` — finding dispositions and balance/design changes.
- `docs/design/PILLARS.md` — default difficulty and pillar-level exceptions.
- `docs/roadmap/RAILS.md` — systemic gaps exposed by the slice.
- `docs/ci/CI_GATE_MANIFEST.json` — slice gates.
- `docs/CURRENT_AUTHORITY.md` — pointers to scenario, protocol, scorecard, and release gate.

`SLICE_INTEGRATION_STATUS.md` may be used during implementation, then archived/retired once the generated scorecard and authority docs replace it.

---

## 23. Recommended Implementation Slices

Use small, reviewable changes:

1. `D1-01` preflight and authority inventory.
2. `D1-02` scenario model/data/validation.
3. `D1-03` data-driven deterministic runner + beat evidence.
4. `D1-04` runtime scenario-start adapter + demo save namespace.
5. `D1-05` demo export + clean boot + build manifest.
6. `D1-06` default difficulty + presentation/accessibility requirements.
7. `D1-07` semantic telemetry funnel.
8. `D1-08` fast `slice_determinism` gate.
9. `D1-09` playtest protocol/privacy/session template.
10. `D1-10` first valid human batch + decision close-out.
11. `D1-11` scorecard generator + threshold policy.
12. `D1-12` nightly metrics + release enforcement.
13. `D1-13` correct manual-checklist claims.
14. `D1-14` deliberate-fault close-out.
15. `D1-15+` derived 54D–54M hardening.

A behavioral change should leave the repo buildable and should not introduce a permanent slice-only gameplay bypass.

---

## 24. Key Risk Register

| Risk | Detection | Mitigation |
|---|---|---|
| host runner diverges from playable runtime | initialization-state parity | one scenario representation + ordinary APIs |
| slice-only tuning makes demo misleading | tuning diff + full-campaign regression | real default preset |
| hash/digest unstable | repeated/cross-platform tests | canonicalization + stable-state digest |
| facilitator teaches required concepts | intervention log | strict protocol/deviation record |
| telemetry collects unnecessary identity | schema/privacy checks | minimal opt-in payload |
| synthetic policy changes fake improvement | policy version in scorecard | compare like-for-like policies |
| missing human evidence appears healthy | scorecard fixture | explicit `NO HUMAN EVIDENCE` |
| exported demo relies on checkout paths | clean-machine boot | packaged-data smoke |
| accessibility exists but progression blocks | full beat completion | end-to-end accessible path |
| thresholds are tuned to current result | policy review | baseline first, decide bar explicitly |
| waivers become permanent | expiry audit | hard expiry/max releases |
| content refactor breaks slice refs | reference graph | catalog/reference validation |
| six sessions are overinterpreted | review language | discovery evidence, not population statistics |
| day-7 proof is mistaken for whole game | scope statement | separate long-horizon instruments |

---

## 25. Guardrails / Non-Goals

D1 does not prove the full campaign, late-game economy, rare-event distribution, or long-horizon save growth. It does not replace existing integrity/selftests or authorize personal analytics. It must not create a hidden easier demo difficulty, bespoke tutorials solely to force completion, or test-only gameplay APIs that bypass the product.

Preserve the source's core constraints: no unconsented recording; no personal data in committed notes; no hand-authored scorecard metrics; no release exception without an explicit decision; no interpreting a green deterministic run as evidence that a player understood the game.

---

## 26. Master Definition of Done

### Scenario / machine proof
- [ ] canonical JSON scenario exists, validates, versions, and hashes;
- [ ] host and game runtime consume the same representation;
- [ ] seven-day required beats occur in order;
- [ ] deterministic evidence/digest is stable under declared conditions;
- [ ] save/load/restart preserve scenario semantics;
- [ ] `slice_determinism` is registered and locally reproducible.

### Export
- [ ] dedicated demo preset exists;
- [ ] clean exported boot succeeds;
- [ ] packaged catalogs/data are verified;
- [ ] build manifest binds revision and scenario hash;
- [ ] demo saves are isolated;
- [ ] full-campaign startup remains healthy.

### Presentation / accessibility
- [ ] real default difficulty is pinned;
- [ ] required presentation/ambience resolves;
- [ ] no bespoke tutorial debt is required;
- [ ] keyboard-only completion passes;
- [ ] reduce-motion completion passes;
- [ ] captions cover required gameplay-relevant audio;
- [ ] critical causal transitions have live attribution surfaces.

### Human proof
- [ ] playtest protocol is versioned;
- [ ] telemetry is optional;
- [ ] at least six valid release sessions exist;
- [ ] records are de-identified;
- [ ] interventions and deviations are recorded;
- [ ] legibility/emotional questions are captured;
- [ ] repeated S2/S3 findings have dispositions;
- [ ] S4 blockers are closed;
- [ ] `DECISIONS.md` contains accepted/rejected decisions;
- [ ] manual QA separates human and machine evidence.

### Standing release proof
- [ ] nightly `slice_play_metrics` runs;
- [ ] scorecard is generated, not manually populated;
- [ ] thresholds are versioned and enforced;
- [ ] stale/incompatible evidence fails;
- [ ] scorecard includes provenance, synthetic, performance, accessibility, human, and integration status;
- [ ] waivers, if supported, are explicit and expiring;
- [ ] historical scorecards remain interpretable;
- [ ] fault injection proves gate coverage.

### Continuation
- [ ] 54D–54M are triaged;
- [ ] 54D/54F/54G/54H are prioritized where uncovered;
- [ ] Waves 9+ can consume slice evidence without duplicating the system;
- [ ] the repository explicitly states what seven-day evidence cannot prove.

---

## 27. Final Execution Order

```text
0  Preflight / authority
1  54A scenario intent + schema
2  54A runtime initialization
3  54A deterministic runner + fast gate
4  54A demo export + clean boot
5  54A difficulty + telemetry + accessibility + presentation
6  54B protocol + privacy
7  54B first six-session batch
8  54B decisions + targeted fixes
9  54C scorecard generator
10 54C nightly metrics + threshold policy
11 54C release enforcement + historical snapshot
12 54D scenario contract hardening
13 54F telemetry integrity
14 54G accessibility evidence
15 54H artifact provenance
16 54E failure paths
17 54J attribution regression
18 54I balance calibration
19 54K playtest operations
20 54L governance
21 54M Waves 9+ feedback loop
```

---

## 28. Implementation-Agent Handoff Rules

Any implementation agent receiving this plan should:

1. re-verify current repository state before editing;
2. preserve the supplied Plan 54 as the semantic source;
3. use existing composition roots, validators, CI manifests, export scripts, and telemetry systems;
4. avoid inventing missing dependencies silently;
5. keep scenario data authoritative;
6. never fabricate human sessions or scorecard metrics;
7. mark absence of real playtests as `NO HUMAN EVIDENCE`;
8. make every new gate locally reproducible;
9. evaluate every scenario edit for version/hash/comparability impact;
10. keep ordinary campaign behavior intact;
11. run narrow tests after each work package and the full relevant suite before close-out;
12. finish with files changed, tests added, commands/results, scenario version/hash, scorecard status, human-evidence status, blockers, and the next task.

---

## 29. Filename Continuation Rule

```text
D1_planintegration.md
D1_planintegration[2].md
D1_planintegration[3].md
D1_planintegration[4].md
D1_planintegration[5].md
...
```

The bracketed number belongs immediately before `.md`.

---

## 30. Close-Out Principle

D1 preserves Plan 54's central correction: machine-confirmed execution and human-confirmed understanding are different instruments. The canonical seven-day slice matters because it forces simulation, content, presentation, accessibility, build engineering, telemetry, human research, and release governance to converge on one playable object.

A completed D1 lets the project answer with evidence:

1. Does the first week execute correctly?
2. Does the exact exported artifact boot and carry the intended scenario?
3. Can a person understand and emotionally read the week without developer tutoring?
4. Did this release make that week better or worse, and is the project explicitly willing to ship the result?

That is the standing-proof standard.
