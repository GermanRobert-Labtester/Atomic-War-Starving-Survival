# PLAN-PLATFORM-PARITY-53 — Windows/Linux Export Truth, Paths, Fonts & Timing

**Wave 6 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-LAUNCH-FACE-06 F6-2 (release craft), PLAN-RELEASE-OPS-20.
**Non-goals:** no macOS target, no console ports, no engine upgrade.

## Outcome
Two export presets exist (`export_presets.cfg`) and a parity CLI partial exists
(`HostCli.ExportParity.cs`), but parity is not a release gate. This plan makes
both platforms provably equivalent on the things that actually break:
case-sensitive paths, font rendering, save locations, input devices, and
timing.

| Area | Risk | Deliverable |
|---|---|---|
| Paths | Linux is case-sensitive; Windows is not | case-collision gate runs on both exports; all catalog/asset loads proven case-exact |
| Fonts | renderer differences / fallback | snapshot set rendered on both exports; glyph/line-height diff budget |
| Save | `user://` mapping differs | save/load/reload round-trip on both; backup restore proven |
| Input | controller/IME/keyboard layouts | F6-1 binding set exercised on both; glyph sets per platform |
| Timing | frame pacing outside headless | one 15-FPS session per platform with frame-time budget |
| Data | PCK includes JSON authority | export parity verifies every catalog hash against the source tree |

## Evidence
- `export_presets.cfg` with Linux/X11 + Windows Desktop presets.
- `src/Host/HostCli.ExportParity.cs` and the "Packaged-data parity of the exported Linux build" gate.
- `.github/workflows/build.yml` exports; Plan 48 audit found verification steps unable to fail.
- `global.json`, CPM, LFS (3,858 objects) affect checkout parity.
- Fixed 1920×1080 viewport, `canvas_items`, `keep_height`.

## Packages
- **PP-53A** export matrix: both presets built; PCK catalog hash comparison vs `Assets/StreamingAssets/Data`.
- **PP-53B** path truth: run the case-collision and forbidden-path gates against the exported trees; fail on any case-only mismatch.
- **PP-53C** font/snapshot parity: render the snapshot corpus from each export; per-panel diff budget and triage.
- **PP-53D** save parity: create/advance/save/reload/backup-restore on each platform; checksum equality.
- **PP-53E** frame budget: bounded 15-FPS session per platform; report frame-time p95.
- **PP-53F** release evidence: parity report artifact attached to the release checklist.

## Acceptance & verification
- Both exports boot, load data, save/load, and pass the snapshot diff within budget.
- `bash scripts/ci/export-build.sh`; `godot --headless --path . -- --export-parity-selftest`; `bash scripts/ci/case-collision-gate.sh`.

## Risks
Snapshot diffs from renderer AA → tolerance budget and per-panel triage; not a blanket suppression.

---

## 6. Expanded census (2 files · 377 lines)

Scope: `Assets/Ashfall.Core/Settings/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
DTO/Type 1 · Support 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `UserSettingsCodec.cs` | 231 | Support | — | 0 | 0 | 0 |
| `UserSettingsData.cs` | 146 | DTO/Type | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Settings/` |
| Test references | 6 name references across the test tree |
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
Governed artifacts: 5. Other plans referencing them: **5**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-SETTINGS-INTEGRITY-54` | 2 |
| `PLAN-INPUT-REBINDING-106` | 2 |
| `PLAN-RELEASE-OPS-20` | 1 |
| `PLAN-ARCHITECTURE-BOUNDARY-31` | 1 |
| `PLAN-BUILD-ERGONOMICS-56` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `HostCli.ExportParity.cs` |
| `UserSettingsCodec.cs` |
| `UserSettingsData.cs` |
| `global.json` |
| `src/Host/HostCli.ExportParity.cs` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `PP-53A` | `HostCli.ExportParity.cs`, `src/Host/HostCli.ExportParity.cs` |
| `PP-53B` | no name match — resolve at claim time |
| `PP-53C` | `HostCli.ExportParity.cs`, `src/Host/HostCli.ExportParity.cs` |
| `PP-53D` | `HostCli.ExportParity.cs`, `src/Host/HostCli.ExportParity.cs` |
| `PP-53E` | no name match — resolve at claim time |
| `PP-53F` | `HostCli.ExportParity.cs`, `src/Host/HostCli.ExportParity.cs` |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 4. Host files: **30** · Test files: **24** · Data files: **5**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 30 | `src/Audio/AudioSelfTest.cs`, `src/Audio/AudioSettings.cs`, `src/Economy/TradeScreenGodotPanel.cs`, `src/Host/CvdDiamondHostSession.cs`, `src/Host/HostCli.AdvancedIndustrialRecon.cs` |
| Tests (`Ashfall.Core.Tests/`) | 24 | `Ashfall.Core.Tests/ActionResultTests.cs`, `Ashfall.Core.Tests/CrossingItemsPlan126Tests.cs`, `Ashfall.Core.Tests/DoseItemExpansionTests.cs`, `Ashfall.Core.Tests/EcologyBalanceSimulationTests.cs`, `Ashfall.Core.Tests/Economy/TradeScreenScenarioCatalogTests.cs` |
| Data (`StreamingAssets/Data/`) | 5 | `Assets/StreamingAssets/Data/door_encounters.json`, `Assets/StreamingAssets/Data/nuclear_winter_phases.json`, `Assets/StreamingAssets/Data/quests_faction_branching.json`, `Assets/StreamingAssets/Data/trade_texts.json`, `Assets/StreamingAssets/Data/world_history.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **14** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caravan_trade_network` |
| `chemical_recon` |
| `crossing` |
| `cvd_diamond` |
| `dose_ledger` |
| `dynamic_quests` |
| `encounters` |
| `faction_espionage` |
| `holdfast_trade` |
| `nuclear_core_lifecycle` |
| `personal_quests` |
| `quests` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **19** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--audio-selftest` |
| `--audio-test` |
| `--crossing-selftest` |
| `--cvd-diamond-selftest` |
| `--dose-ledger-selftest` |
| `--dose-uitest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--holdfast-trade-save-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **18**.

| Event | First declaration |
|---|---|
| `OnActionCompleted` | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` |
| `OnActionExecuted` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnActionResolved` | `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` |
| `OnActionSelected` | `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` |
| `OnChapterAdvanced` | `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` |
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnDayAdvanced` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnDoseChanged` | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` |
| `OnDoseCorrected` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |

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

Host files (`src/`) whose names share a domain token: **2**
(0 of them panels/HUD).

| Host file |
|---|
| `src/Host/HostCli.ExportParity.cs` |
| `src/Settings/UserSettings.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **0**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| — | no section key shares a token with this domain |

**Verdict:** no save section matches — persistence is owned under a differently-named section, or absent.

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 5
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 2 · catalogs 0 · test regions 0 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PLATFORM-PARITY-53
wave: 6
status: PROPOSED — foreman claim required
packages: PP-53A, PP-53B, PP-53C, PP-53D, PP-53E, PP-53F
claim paths:
  - src/Host/HostCli.ExportParity.cs  # §19 candidate host surface
  - src/Settings/UserSettings.cs  # §19 candidate host surface
verification:
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 5 other plan(s) name these artifacts (§12)
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
