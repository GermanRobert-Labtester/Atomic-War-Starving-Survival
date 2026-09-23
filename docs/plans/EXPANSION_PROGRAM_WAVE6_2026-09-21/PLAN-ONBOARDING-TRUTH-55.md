# PLAN-ONBOARDING-TRUTH-55 — First Hour, Guidance Routes & Teach-vs-Demand

**Wave 6 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-UI-SURFACE-15, PLAN-LAUNCH-FACE-06 F6-1.
**Non-goals:** no forced tutorial, no unskippable modal chain, no fake scripting.

## Outcome
The onboarding assets exist (`TutorialPanel`, `OnboardingHintPanel`,
`Main.Onboarding.cs`, 7-step `FirstHourFunnel` from `PlaySessionRecorder`,
`--day1-selftest`, `--day1-playable-selftest`, `--campaign-journey-selftest`),
but the survival systems demand more than the first hour teaches. This plan
measures and seals the gap.

| Deliverable | Detail |
|---|---|
| Teach audit | every demand of days 1–3 (water, food, warmth, power, health, defence) mapped to what teaches it and when |
| Guidance routes | every hint opens the right panel (the retired `OnboardingHintPanel` had no route — verify current routes) |
| Funnel truth | the 7-step funnel is generated from real actions, not parallel bookkeeping |
| Skippability | every step is skipped and re-openable; a veteran path exists |
| Failure recovery | a player who misses a step can still recover on day 3 (no soft lock) |
| Day-1 replay | a scripted day-1 path passes headless and leaves the shelter viable |

## Evidence
- `src/UI/TutorialPanel.cs`, `OnboardingHintPanel.cs`, `src/Main.Onboarding.cs`.
- Telemetry: `PlaySessionRecorder` 7-step `FirstHourFunnel`, JSONL under `user://`, zero network/PII.
- Gates: `--day-1-selftest`, `--day1-playable-selftest`, `--day1-to-day2-milestone-selftest`, `--real-campaign-journey-selftest`.
- Plan 37 evidence: `ashfall_help` predicate exists; guidance panel routing was previously broken.

## Packages
- **ON-55A** demand↔teach matrix: generated from needs/medical/power/water owners vs tutorial steps; gaps listed as findings.
- **ON-55B** route seal: every hint has a resolvable route; probe opens and closes each.
- **ON-55C** funnel derivation: funnel steps derive from real actions; duplicate counters removed.
- **ON-55D** skip/reopen: scripted skip path and late recovery path.
- **ON-55E** day-1 viability: scripted first day ends with alive roster, water, food, heat, and no critical deficit.
- **ON-55F** hint pacing: hints are event-driven with cooldowns; no modal spam.

## Acceptance & verification
- Zero unreachable hints; zero untaught day-1 demands; day-1 viability green.
- `godot --headless --path . -- --day1-playable-selftest`; `--campaign-journey-selftest`; `bash scripts/run_test.sh Ashfall.Core.Tests/Onboarding/`.

## Risks
Hints become nagging → cooldown + dismissal memory; event-driven only.

---

## 6. Expanded census (2 files · 768 lines)

Scope: `Assets/Ashfall.Core/Onboarding/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
DTO/Type 1 · Support 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `OnboardingJourney.cs` | 630 | Support | — | 0 | 0 | 2 |
| `OnboardingSaveState.cs` | 138 | DTO/Type | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `OnboardingJourney.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Onboarding/` (create if absent) |
| Test references | 3 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 6. Other plans referencing them: **0**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| — | no other plan references these names |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `Main.Onboarding.cs` |
| `OnboardingHintPanel.cs` |
| `OnboardingJourney.cs` |
| `OnboardingSaveState.cs` |
| `src/Main.Onboarding.cs` |
| `src/UI/TutorialPanel.cs` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `ON-55A` | `src/UI/TutorialPanel.cs` |
| `ON-55B` | no name match — resolve at claim time |
| `ON-55C` | no name match — resolve at claim time |
| `ON-55D` | no name match — resolve at claim time |
| `ON-55E` | no name match — resolve at claim time |
| `ON-55F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 5. Host files: **6** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 6 | `src/Host/HostCli.Onboarding.cs`, `src/Host/OnboardingSaveStore.cs`, `src/Main.Onboarding.cs`, `src/Main.UiPanels.cs`, `src/UI/OnboardingHintPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/OnboardingJourneyTests.cs`, `Ashfall.Core.Tests/WildlifeTrapping/WildlifeTrappingFlagshipTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `onboarding` |
| `wildlife_ecosystem` |
| `wildlife_trapping` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **16** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--onboarding-journey-selftest` |
| `--onboarding-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--player-panels-ui-test` |
| `--player-panels-uitest` |
| `--real-campaign-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnTrappingChanged` | `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **0**.

| Catalog |
|---|
| — | no catalog filename shares a token with this domain |

**Verdict:** no catalog filename shares a token with this domain — the authority is likely code-defined or its data lives in a broader catalog. Not a conclusion; check the owning loader.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **5**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/HostCli.Onboarding.cs` |
| `src/Host/OnboardingSaveStore.cs` |
| `src/Main.Onboarding.cs` |
| `src/Main.UiTests.RealCampaignJourney.cs` |
| `src/UI/OnboardingHintPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `onboarding` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **0**
(none).

| Catalog | Classification |
|---|---|
| — | no catalog filename shares a token with this domain |

**Verdict:** no catalog matches — the domain is code-authoritative or its data lives in a broader catalog.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 1 (laddered 0) · RNG streams 0 · host files 5 · catalogs 0 · test regions 0 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ONBOARDING-TRUTH-55
wave: 6
status: PROPOSED — foreman claim required
packages: ON-55A, ON-55B, ON-55C, ON-55D, ON-55E, ON-55F
claim paths:
  - src/Host/HostCli.Onboarding.cs  # §19 candidate host surface
  - src/Host/OnboardingSaveStore.cs  # §19 candidate host surface
  - src/Main.Onboarding.cs  # §19 candidate host surface
  - src/Main.UiTests.RealCampaignJourney.cs  # §19 candidate host surface
verification:
  - godot --headless --path . -- --campaign-journey-selftest
dependencies:
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
