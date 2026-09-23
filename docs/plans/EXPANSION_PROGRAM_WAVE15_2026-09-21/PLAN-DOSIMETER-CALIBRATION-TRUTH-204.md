# PLAN-DOSIMETER-CALIBRATION-TRUTH-204 — Instrument Accuracy, Drift & Trust

**Wave 15 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-RADIATION-BACKGROUND-TRUTH-189, PLAN-METROLOGY-TRUTH-172, PLAN-MAINTENANCE-DECAY-TRUTH-119.
**Non-goals:** no dose ledger change (existing owner), no shielding model
(Plan 189), no standards set (Plan 172).

## 1. Outcome
`Radiation/DosimeterCalibrationSystem.cs` (**373 lines**) is reachable and
unaddressed. Plan 189 covers measurement error floors for counters; **personal
dosimeters** are a separate instrument class whose accuracy drifts and whose
readings people trust with their health. Unstated, a dosimeter is either
perfect or flavor.

| Deliverable | Detail |
|---|---|
| Calibration state | dosimeters carry calibration date, drift rate, and a documented accuracy band in game days |
| Reading truth | a displayed dose reading is the true value plus band error; the band widens with drift (seeded where specified) |
| Calibration path | recalibration consumes a standard (Plan 172) and time; without it, drift continues |
| Warning honesty | threshold alarms fire against the **measured** value with the band disclosed, never a hidden correction |
| Save truth | calibration state and readings restore; no re-roll or silent recalibration on load |

## 2. Evidence
- `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` (373 lines; unaddressed — Wave 13/15 audit).
- Plan 189 owns the measurement error-floor family; this plan is the personal-instrument sibling.
- Plan 172 supplies standards for recalibration; Plan 119 the drift contract pattern.
- The dose ledger remains the single dose authority; this plan never writes dose.

## 3. Packages
- **DCT-204A** calibration state + drift table.
- **DCT-204B** reading-band tests (paired sample distributions).
- **DCT-204C** recalibration path consuming a standard + time.
- **DCT-204D** alarm-against-measured test with band disclosure.
- **DCT-204E** save round-trip; no silent recalibration.

## 4. Acceptance & verification
- Accuracy bands widen with drift; recalibration narrows them and consumes the standard.
- Alarms fire against measured values with the band shown; no hidden correction.
- Save/load preserves calibration state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Radiation/`.

## 5. Risks
False security → the band is always disclosed with the reading.
Dose duplication → the ledger remains sole writer; a test asserts no foreign writes.

---

## 6. Expanded census (2 files · 393 lines)

Scope: `Assets/Ashfall.Core/Radiation/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `Dosimeter.cs` | 20 | Support | — | 0 | 0 | 0 |
| `DosimeterCalibrationSystem.cs` | 373 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `DosimeterCalibrationSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Radiation/` |
| Test references | 17 name references across the test tree |
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

Domain files: 2. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `EVIDENCE` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `DCT-204A` | `DosimeterCalibrationSystem.cs` |
| `DCT-204B` | no name match — resolve at claim time |
| `DCT-204C` | no name match — resolve at claim time |
| `DCT-204D` | no name match — resolve at claim time |
| `DCT-204E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **9** · Test files: **8** · Data files: **28**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 9 | `src/Dose/DoseRegisterSurface.cs`, `src/Host/DoseLedgerHostSession.cs`, `src/Host/InventoryHostSession.cs`, `src/UI/GeigerCalibrationPanel.cs`, `src/UI/LowBackgroundLeadPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 8 | `Ashfall.Core.Tests/DoseItemExpansionTests.cs`, `Ashfall.Core.Tests/DosimeterCalibrationSystemTests.cs`, `Ashfall.Core.Tests/HeirloomSystemTests.cs`, `Ashfall.Core.Tests/Inventory/ItemDescriptionCatalogTests.cs`, `Ashfall.Core.Tests/JournalProducerIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 28 | `Assets/StreamingAssets/Data/dose_items.json`, `Assets/StreamingAssets/Data/dose_quests.json`, `Assets/StreamingAssets/Data/duty_roster_quests.json`, `Assets/StreamingAssets/Data/economy_goods.json`, `Assets/StreamingAssets/Data/holdfast_items.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **12** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `contractor_roster` |
| `dose_ledger` |
| `duty_roster` |
| `dynamic_quests` |
| `economy` |
| `holdfast` |
| `holdfast_trade` |
| `inventory` |
| `journal` |
| `low_background_metrology` |
| `personal_quests` |
| `quests` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **29** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--dose-ledger-selftest` |
| `--dose-uitest` |
| `--duty-roster-loop-selftest` |
| `--duty-roster-save-selftest` |
| `--duty-roster-selftest` |
| `--duty-roster-uitest` |
| `--economy-selftest` |
| `--economy-uitest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--holdfast-briefing` |
| `--holdfast-runtime-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **20**.

| Event | First declaration |
|---|---|
| `OnAntennaCalibrationChanged` | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` |
| `OnCalibrationCompleted` | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` |
| `OnCalibrationFailed` | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` |
| `OnCalibrationOverdue` | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` |
| `OnCalibrationStarted` | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` |
| `OnDoseChanged` | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` |
| `OnDoseCorrected` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnDutyVacated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnEconomyChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnInventoryChanged` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnItemAdded` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnItemDegraded` | `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` |

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

Host files (`src/`) whose names share a domain token: **1**
(1 of them panels/HUD).

| Host file |
|---|
| `src/UI/GeigerCalibrationPanel.cs` |

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

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `metrology_calibration_drift` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 0 (laddered 0) · RNG streams 1 · host files 2 · catalogs 0 · test regions 0 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DOSIMETER-CALIBRATION-TRUTH-204
wave: 15
status: PROPOSED — foreman claim required
packages: DCT-204A, DCT-204B, DCT-204C, DCT-204D, DCT-204E
claim paths:
  - src/UI/GeigerCalibrationPanel.cs  # §19 candidate host surface
  - metrology_calibration_drift  # §19 candidate host surface
verification:
  - godot --headless --path . -- --dose-ledger-selftest
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
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
