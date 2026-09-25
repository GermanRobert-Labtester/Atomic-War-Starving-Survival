# Forward Leap Wave — 2026-09-26 (run report)

Status legend: **DONE** = implemented and verified this wave; **VERIFIED** = audit
run green with no fix required; **BLOCKED** = cannot land without a named owner
or external resource.

## Part A — 8 forward-leap tasks

| # | Task | Status | Evidence |
|---|---|---|---|
| A1 | Alpha content-complete campaign sweep | PARTIAL | `--7-day-smoke-selftest` PASS, `--campaign-fuzz-selftest` PASS, `--real-campaign-journey-selftest` PASS (101 s), `--onboarding-journey-selftest` PASS. The **human** 3-day through-every-panel pass remains human-gated (no input automation). |
| A2 | Save/load hardening matrix | VERIFIED | `--session-durability-selftest` **21/21**, `--save-store-checksum-selftest` PASS, `--save-load-ui-failure-selftest` PASS, `--save-load-selftest` PASS. Corruption-injection beyond these is the remaining scope. |
| A3 | Architecture de-bloating | PARTIAL | Measured: `Main.cs` is 83 lines but the class spans **213 `Main.*.cs` partials**; `src/UI` is 265 files; `WornGear` is **not** duplicated (1 definition). A dedup target list is required before surgical extraction; no safe one-file fix existed this wave. |
| A4 | Determinism audit at scale | VERIFIED | `--deterministic-smoke-selftest` → 7-day smoke PASS same-seed; plus the existing save/checksum gates. A paired-replay map over every consumer is still the larger scope. |
| A5 | Localization readiness | PARTIAL + ratchet | `assets/l10n/strings.csv` = 360 lines total, while UI code holds **603** hardcoded `Text/Title/Label` literals (test-counted) — coverage is a real gap. Ratchet gate added (see below) so the count cannot grow. |
| A6 | Three-platform distribution pipeline | DONE (unsigned) | macOS `.app` export on Linux (runbook in `docs/builds/MACOS_EXPORT.md`), **macOS step added to `scripts/tools/ashfall-package-verify.sh`**; Linux/Windows zips remain in `dist/`. Signing/notarization = Mac hardware. |
| A7 | Accessibility & input parity | VERIFIED | `--ui-accessibility-selftest` PASS, `--accessibility-selftest` PASS, `--ui-layout-selftest` PASS, plus the standing 169-panel/559-button clickability gates. Full controller traversal remains the large scope. |
| A8 | Telemetry → balance loop | PARTIAL | Funnel fixture 7/7 recorded; live `play_metrics.jsonl` is multi-session and explicitly not a playthrough metric (documented in `docs/telemetry/FIRST_HOUR_TRIAGE_2026-09-25.md`). Balance tuning requires real playtest data. |

### Ratchet gate added this wave

`Ashfall.Core.Tests/Tooling/LocalizationRatchetTests.cs` — scans `src/UI/*.cs`
for hardcoded UI literals and fails when the count **exceeds** the recorded
baseline, so localization debt can only shrink. (Baseline captured at 603 literals as counted by the test itself; the shell-grep figure of 593 differed slightly because of quoting semantics — the test count is the gate authority.)

## Part B — 7 larger game features

| # | Feature | Status | Detail |
|---|---|---|---|
| B1 | Shelter breach drills | **DONE** | `PowerGridHostSession.RunLoadShedDrill()` routes through the canonical `PowerGridSystem.ApplyBrownoutShedPreset()`; `PowerGridPanel` gained **RUN LOAD-SHED DRILL** with a truthful result line. No new save state. |
| B2 | Heat zoning contest | **DONE** | `ShelterThermalPanel` now exposes a **per-room radiator valve control** (100→75→50→25→0 %) using the existing `SetRadiatorValve` allocation seam; room lines show live valve %; labels refresh on state change. |
| B3 | Water network pressure | BLOCKED | `WaterTreatmentPanel` sits under the (COMPLETE) water-sources claim and no pressure/valve API was located in the treatment system; needs the owner's premise audit first. |
| B4 | Radio traffic decoding | BLOCKED | The radio decode/intercept API could not be located in Core under the expected names; needs a premise audit of `RadioIntelligencePanel` (unclaimed) + the radio owner. |
| B5 | Scarcity substitutions | BLOCKED | Recipe access is owned by the plan24 A2-claimed `CraftingSystem.cs`; the substitution advisor needs that catalog's public surface confirmed first. |
| B6 | Item provenance chain | BLOCKED | Requires per-instance identity on inventory stacks; the inventory owner (`Inventory.cs`, plan22 claim) must expose it before a provenance display is truthful. |
| B7 | Shift fatigue marketplace | BLOCKED | Duty roster is under the **ACTIVE** `claim-c1-plan24-survivor-ledger-2026-09-16` package; roster UI changes belong to that owner. |

## Part C — 4 UI/UX tasks

| # | Task | Status | Detail |
|---|---|---|---|
| C1 | In-panel smooth transitions (no nested menus) | **DONE (wave 1)** | New `src/UI/UiPanelFlow.cs` `TransitionSwap` (60 ms fade-out + 100 ms fade/rise/settle, reduced-motion aware). Wired into `InventoryPanel` filter swaps and `DoseGeographyPanel` sector swaps. Wave 2 = convert the remaining nested flows. |
| C2 | Movable UI screens | **DONE (wave 1)** | `UiPanelFlow.AttachDrag` + `UiLayoutStore` (`user://ui_layout.json`, atomic, exception-safe). Applied to `ConfirmationModal` and `NarrativeArcModal` (both draggable, positions persist). `AshfallDashboardShell.EnableWindowDrag(key)` exposes it to windowed panels. |
| C3 | Animation coverage wave 2 | **DONE (core coverage)** | `AshfallMetricCard.SetValue` now pulses on real value change — inherited by every status rail in the game (169 panels). |
| C4 | Out-of-menu HUD beauty | **DONE** | `AshfallDashboardShell` gained a 2 px accent rule under the header (inherited chrome polish); snapshot corpus rebaselined **32/32**. |

## Verification (this wave)

- Build: 0 errors.
- `--player-panels-uitest` PASS, `--ui-layout-selftest` PASS,
  `--ui-accessibility-selftest` PASS, `--data-integrity-selftest` 427/427.
- Snapshot corpus: regenerated, **32/32 match**.
- Selftests: 7-day smoke PASS, campaign fuzz PASS, session durability 21/21,
  save-store checksum PASS, save-load + save-load-UI-failure PASS,
  deterministic smoke PASS, asset registry PASS.
