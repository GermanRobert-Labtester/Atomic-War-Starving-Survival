# PLAN-ACHIEVEMENTS-COMPLETION-TRUTH-76 — Achievements, History & Profile Surfaces

**Wave 7 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-UNBLOCK-03 U1c, PLAN-DEBT-DRAIN-24.
**Expanded appendix:** [`PLAN-ACHIEVEMENTS-COMPLETION-TRUTH-76_APPENDIX-A_ACHIEVEMENT_CATALOG.md`](PLAN-ACHIEVEMENTS-COMPLETION-TRUTH-76_APPENDIX-A_ACHIEVEMENT_CATALOG.md)
— the achievement catalog: **16 achievements** with categories and conditions (AC-76A surface input).

**Non-goals:** no platform achievement service, no cloud profile, no second
history store.

## Outcome
Achievements and completion tracking exist (Plan 149 `AchievementSystem`:
16 achievements, data-backed conditions, once-only emission, read-only export;
`CampaignCompletionHistory` schema v2 with `difficultyPresetId`;
`CrossRunProfileStore` user-level; `CompletionHistorySummary`; `EpilogueMatrix`).
What is missing is the **player-facing truth surface** and the profile/achievement
integration being wired end-to-end.

| Deliverable | Detail |
|---|---|
| Achievement surface | panel lists achieved/locked with plain-language conditions from the catalog |
| Session vs profile | in-campaign achievements reflect run facts; cross-run profile rows append-only (DEC-20/33 boundaries) |
| Completion history | campaign list with difficulty stamp, outcome, dates; read-only summary |
| Profile page | user-level stats and legacy unlocks; never campaign-mutating |
| Toast/feedback | unlocked achievements produce one journal line + optional toast; no spam |
| Integrity | no achievement grants on load/reload; no duplicate unlocks; deterministic conditions |

## Evidence
- Plan 149: `AchievementSystem` + `achievements.json` (16 across 6 categories), `AchievementsPanel` data-backed, `GetCompletedAchievementIds()`.
- `CampaignCompletionHistory` v2 (`difficultyPresetId` NonSerialized for checksum, folded into v2 hash), `CampaignCompletionHistoryTests` 11/11.
- `CrossRunProfileStore` (`user://profile.json`), DEC-20/33 boundaries; `CompletionHistorySummary` (EN-07).
- `UnifiedEndingResolver`, `EpilogueMatrix` 25 outcomes.

## Packages
- **AC-76A** achievements surface: list with conditions + progress; reads the Core authority only.
- **AC-76B** feedback: once-only unlock event → journal + optional toast; reload never re-fires.
- **AC-76C** completion history surface: list/summary; difficulty stamp visible; no mutation.
- **AC-76D** profile surface: user-level read model; separate from campaign slots; deletion supported.
- **AC-76E** integrity probes: no grant-on-load, no duplicates, deterministic; profile isolation tested.

## Acceptance & verification
- Unlock fires once across save/reload; profile survives campaign deletion; no campaign state leaks into profile.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Achievements/`; endgame suites; `Plan149AchievementIntegrationTests`.

## Risks
Privacy of local history → user-level file documented in PLAN-TELEMETRY-PRIVACY-58 with delete control.

---

## 6. Expanded census (1 files · 404 lines)

Scope: `Assets/Ashfall.Core/Achievements/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `AchievementSystem.cs` | 404 | System | — | 1 | 0 | 2 |

**Totals:** 1 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `achievements.json` | object[2 keys] |

**State surfaces:** `AchievementSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Achievements/` |
| Test references | 1 name references across the test tree |
| Determinism | 1 banned refs to fix or justify |
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

Domain files: 1. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `AC-76A` | `AchievementSystem.cs` |
| `AC-76B` | no name match — resolve at claim time |
| `AC-76C` | no name match — resolve at claim time |
| `AC-76D` | no name match — resolve at claim time |
| `AC-76E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **4** · Test files: **1** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Main.GameFlow.cs`, `src/Main.PlayerSurfaces.cs`, `src/UI/AchievementsPanel.cs`, `src/UI/GameDashboardPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/achievements.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **0** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| — | no section key shares a token with this domain |

**Verdict:** no section key shares a token with this domain — either the authority is derived/stateless, or its persistence key is named after a different owner. Not a conclusion; verify in the owning system.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **8** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--dashboard-uitest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--real-main-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this domain |

**Verdict:** no event name shares a token with this domain — the domain is command-polled, data-driven, or silently unreachable. Confirm against the event catalog before treating it as a gap.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **1**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/achievements.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **8** (155 files, 1228 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Achievements` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Economy` | 41 | 329 |
| `Holdfast` | 1 | 13 |
| `InformationFlow` | 3 | 19 |
| `Integration` | 16 | 74 |
| `Lifecycle` | 1 | 5 |
| `Shelter` | 87 | 754 |

**Verdict:** 1228 cases sit under matching regions — run those first (`Achievements`, `Audio`, `Economy`, `Holdfast`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **378**
(232 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/CompletionHistorySelfTest.cs` |
| `src/Host/CompletionHistoryStore.cs` |
| `src/Host/HoldfastTerminalPanel.cs` |
| `src/Host/HostCli.PanelTests.cs` |
| `src/Host/PanelBindLifecycleSelfTest.cs` |
| `src/Main.AdvancedShelterSystems.cs` |
| `src/Main.Anomaly.cs` |
| `src/Main.Application.cs` |
| `src/Main.Audio.cs` |
| `src/Main.Bionics.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **23**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `anomaly_hazard` | no |
| `bionics` | no |
| `black_market` | no |
| `caravan_trade_network` | no |
| `economy` | no |
| `expanded_shelter` | no |
| `holdfast` | yes |
| `holdfast_trade` | no |
| `nuclear_core_lifecycle` | no |
| `shelter` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **8**.

| Stream |
|---|
| `advanced_mfg_ebpvd_coating` |
| `anomaly_hazard` |
| `bionics` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `economy` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **27**
(CODEX_ONLY 6, GAMEPLAY_CONSUMED 13, OPTIONAL 3, UNRESOLVED 5).

| Catalog | Classification |
|---|---|
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `hardcore_economy_tuning.json` | GAMEPLAY_CONSUMED |
| `holdfast_factions.json` | GAMEPLAY_CONSUMED |
| `holdfast_flavor.json` | GAMEPLAY_CONSUMED |
| `holdfast_items.json` | GAMEPLAY_CONSUMED |
| `holdfast_locations.json` | GAMEPLAY_CONSUMED |
| `holdfast_npcs.json` | UNRESOLVED |

**Verdict:** 5 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 23 (laddered 1) · RNG streams 8 · host files 20 · catalogs 11 · test regions 8 · flags 8

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ACHIEVEMENTS-COMPLETION-TRUTH-76
wave: 7
status: PROPOSED — foreman claim required
packages: AC-76A, AC-76B, AC-76C, AC-76D, AC-76E
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Host/CompletionHistorySelfTest.cs  # §19 candidate host surface
  - src/Host/CompletionHistoryStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/achievements.json  # §17 catalog (verify schema + consumer)
  - audio_cues.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Achievements/
  - godot --headless --path . -- --dashboard-uitest
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
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
