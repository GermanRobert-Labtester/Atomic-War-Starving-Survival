# PLAN-RADIATION-BACKGROUND-TRUTH-189 — Low-Background Shelter, Counting & Contamination

**Wave 14 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ENERGY-NUCLEAR-48, PLAN-MEDICAL (Plan 47/124 dose seams), PLAN-MAINTENANCE-DECAY-TRUTH-119.
**Implementation scaffold:** [`PLAN-RADIATION-BACKGROUND-TRUTH-189_APPENDIX-A_SCAFFOLD.md`](PLAN-RADIATION-BACKGROUND-TRUTH-189_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-DOSIMETER-CALIBRATION-TRUTH-204` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no dose model rewrite (existing ledger/dose owners remain), no
reactor model (Plan 48), no medical care (Plan 124).

## 1. Outcome
`Radiation/LowBackgroundLeadEngine.cs` (**531 lines**) is reachable and
unaddressed: shielding and low-background measurement (lead castles, counting
rooms). It sits beside the existing dose ledger — the contract must state that
shielding reduces **measured/accumulated** dose through the dose owner, and
that measurement has an error floor rather than perfect truth.

| Deliverable | Detail |
|---|---|
| Shielding model | shielding mass/geometry reduces background per a documented curve; material sourcing ties to industry (Plan 45) |
| Measurement | a counting result has an error floor and drift; a "clean" reading is a probabilistic statement, not absolute |
| Dose coupling | dose increments route through the existing dose owner; this system never writes dose anywhere else |
| Contamination | contaminated shielding material is detected and handled (decontamination or disposal) with a visible state |
| Save truth | shielding state and counters restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Radiation/LowBackgroundLeadEngine.cs` (531 lines; unaddressed — Wave 13 audit).
- The dose ledger section exists (`dose_ledger`, ladder 2) — the coupling target.
- Plan 119 supplies shielding wear; Plan 48 owns power/reactor context.
- Plan 93 verifies material transfers.

## 3. Packages
- **RBT-189A** shielding curve + material table.
- **RBT-189B** measurement error-floor tests (paired samples).
- **RBT-189C** dose-coupling audit (no foreign writes).
- **RBT-189D** contaminated-shielding detection + handling fixtures.
- **RBT-189E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Background falls per the curve as shielding increases; measurement never claims absolute zero.
- Dose changes appear only in the dose owner.
- Contaminated material is detected and has a documented handling path.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Radiation/`.

## 5. Risks
Absolute truth claims → error floor is tested and displayed.
Dose duplication → the coupling audit forbids a second writer.

---

## 6. Expanded census (5 files · 2,257 lines)

Scope: `Assets/Ashfall.Core/Radiation/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 3 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `LowBackgroundLeadEngine.cs` | 531 | System | **yes** | 0 | 0 | 2 |
| `RadiationEconomyBridge.cs` | 357 | Support | — | 0 | 0 | 2 |
| `RadiationPhaseProgression.cs` | 533 | Support | — | 0 | 0 | 2 |
| `RadiationSocialBridge.cs` | 350 | Support | — | 0 | 0 | 2 |
| `RadiationSystem.cs` | 486 | System | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 4 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `low_background_lead_catalog.json` | object[2 keys] |
| `shelter_shielding.json` | object[15 keys] |
| `radiation_economy_social.json` | object[4 keys] |
| `leadership_policies.json` | object[2 keys] |
| `lead_crystal_scintillator_aging_logs.json` | array[7] |
| `lead_wall_degradation_logs.json` | array[7] |

**State surfaces:** `LowBackgroundLeadEngine.cs`, `RadiationEconomyBridge.cs`, `RadiationPhaseProgression.cs`, `RadiationSocialBridge.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Radiation/` |
| Test references | 49 name references across the test tree |
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

Domain files: 1. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `RBT-189A` | no name match — resolve at claim time |
| `RBT-189B` | no name match — resolve at claim time |
| `RBT-189C` | no name match — resolve at claim time |
| `RBT-189D` | no name match — resolve at claim time |
| `RBT-189E` | `LowBackgroundLeadEngine.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **16** · Test files: **48** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 16 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Host/AutopsyHostSession.cs`, `src/Host/DecontaminationHostSession.cs`, `src/Host/HoldfastRuntimeSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 48 | `Ashfall.Core.Tests/AudioEventIntegrationTests.cs`, `Ashfall.Core.Tests/AutopsyBridgeTests.cs`, `Ashfall.Core.Tests/AutopsyIntegrationTests.cs`, `Ashfall.Core.Tests/AutopsySystemTests.cs`, `Ashfall.Core.Tests/BalanceFedLoopTelemetryTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/slice_seven_days.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **1**; isolated: **3**.

| From | → To |
|---|---|
| `RadiationPhaseProgression` | `RadiationSystem` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **6** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `economy` |
| `low_background_metrology` |
| `radiation` |
| `shelter_social_dynamics` |
| `social` |
| `survivor_social` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **3** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--economy-selftest` |
| `--economy-uitest` |
| `--social-drift-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnEconomyChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnPhaseChanged` | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` |
| `OnPhaseTransitioned` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnRadiationDoseResetRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnRadiationExposure` | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **11**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/economy_goods.json` |
| `Assets/StreamingAssets/Data/hardcore_economy_tuning.json` |
| `Assets/StreamingAssets/Data/low_background_lead_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/lead_crystal_scintillator_aging_logs.json` |
| `Assets/StreamingAssets/Data/narrative/lead_wall_degradation_logs.json` |
| `Assets/StreamingAssets/Data/narrative/radiation_survey_readings_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/surface_radiation_topo_sheets.json` |
| `Assets/StreamingAssets/Data/narrative/typographic_lead_wear_logs.json` |
| `Assets/StreamingAssets/Data/narrative_progression.json` |
| `Assets/StreamingAssets/Data/radiation_economy_social.json` |
| `Assets/StreamingAssets/Data/shelter_social_events.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **3** (62 files, 490 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Economy` | 41 | 329 |
| `Progression` | 11 | 83 |
| `Radiation` | 10 | 78 |

**Verdict:** 490 cases sit under matching regions — run those first (`Economy`, `Progression`, `Radiation`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **24**
(7 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Host/EconomyHostSession.cs` |
| `src/Host/EconomySaveStore.cs` |
| `src/Host/LowBackgroundMetrologyHostSession.cs` |
| `src/Host/LowBackgroundMetrologySaveStore.cs` |
| `src/Host/Phase0HostSession.cs` |
| `src/Host/Phase0SaveStore.cs` |
| `src/Host/ShelterSocialSaveStore.cs` |
| `src/Host/SurvivorSocialSaveStore.cs` |
| `src/Main.Economy.cs` |
| `src/Main.LowBackgroundMetrology.cs` |
| `src/Main.Phase0.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **7**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `economy` | no |
| `low_background_metrology` | no |
| `phase0` | no |
| `radiation` | no |
| `shelter_social_dynamics` | no |
| `social` | no |
| `survivor_social` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `economy` |
| `low_background_metrology` |
| `social` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **9**
(CODEX_ONLY 5, GAMEPLAY_CONSUMED 4).

| Catalog | Classification |
|---|---|
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `hardcore_economy_tuning.json` | GAMEPLAY_CONSUMED |
| `narrative/lead_crystal_scintillator_aging_logs.json` | CODEX_ONLY |
| `narrative/lead_wall_degradation_logs.json` | CODEX_ONLY |
| `narrative/radiation_survey_readings_batch_2.json` | CODEX_ONLY |
| `narrative/surface_radiation_topo_sheets.json` | CODEX_ONLY |
| `narrative/typographic_lead_wear_logs.json` | CODEX_ONLY |
| `narrative_progression.json` | GAMEPLAY_CONSUMED |
| `shelter_social_events.json` | GAMEPLAY_CONSUMED |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

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
**Surface:** save sections 7 (laddered 0) · RNG streams 3 · host files 15 · catalogs 20 · test regions 3 · flags 3

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-RADIATION-BACKGROUND-TRUTH-189
wave: 14
status: PROPOSED — foreman claim required
packages: RBT-189A, RBT-189B, RBT-189C, RBT-189D, RBT-189E
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Host/EconomyHostSession.cs  # §19 candidate host surface
  - src/Host/EconomySaveStore.cs  # §19 candidate host surface
  - src/Host/LowBackgroundMetrologyHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/economy_goods.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/hardcore_economy_tuning.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Economy/
  - godot --headless --path . -- --economy-selftest
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
