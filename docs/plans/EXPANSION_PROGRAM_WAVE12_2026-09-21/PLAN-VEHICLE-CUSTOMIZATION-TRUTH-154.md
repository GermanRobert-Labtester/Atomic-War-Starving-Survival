# PLAN-VEHICLE-CUSTOMIZATION-TRUTH-154 — Loadout Slots, Compatibility & Trade-Offs

**Wave 12 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-TRANSPORT-EXPEDITION-30, PLAN-MAINTENANCE-DECAY-TRUTH-119, PLAN-INVENTORY-CONSERVATION-93.
**Implementation scaffold:** [`PLAN-VEHICLE-CUSTOMIZATION-TRUTH-154_APPENDIX-A_SCAFFOLD.md`](PLAN-VEHICLE-CUSTOMIZATION-TRUTH-154_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-EXPEDITION-VEHICLE-TRUTH-219` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no armor grades (CF-P6 owns that seam — do not touch it), no new
vehicle catalog, no travel resolution (Plan 30).

## 1. Outcome
`Vehicles/VehicleCustomizationSystem.cs` is a single-file authority for fitting
vehicles. Today a fitting has no stated slot model, no compatibility rules, and
no trade-off (weight vs speed vs consumption), so customization is either
cosmetic or free power — and it risks colliding with the active
`CF-P6-VEHICLE-ARMOR-GRADES` seam if built carelessly.

| Deliverable | Detail |
|---|---|
| Slot model | documented slots per vehicle class with capacity; a fitted part occupies exactly one slot |
| Compatibility | part↔vehicle and part↔part rules (catalog-tagged); an illegal fit is refused typed |
| Trade-offs | each fitting publishes its effect rows (weight, consumption, speed, capacity) to the travel owner (Plan 30) — no local modifiers |
| Recall | fitted parts remain inventory items and return on removal; conservation wrapper balances |
| Persistence | loadout restores with the vehicle state; a load does not re-fit or drop parts |

## 2. Evidence
- `Assets/Ashfall.Core/Vehicles/VehicleCustomizationSystem.cs` (whole directory; verified).
- AGENTS.md: `CF-P6-VEHICLE-ARMOR-GRADES` is an available claim on the Plan 50 vehicle seam — armor grades are explicitly out of scope here.
- Plan 30 owns travel arithmetic the effects feed.
- Plan 93's wrapper covers part transfers.

## 3. Packages
- **VCT-154A** slot model + capacity table per class.
- **VCT-154B** compatibility rules + illegal-fit refusal tests.
- **VCT-154C** effect rows handed to Plan 30 (no local modifiers).
- **VCT-154D** removal/recall conservation test.
- **VCT-154E** persistence round-trip + no re-fit on load.

## 4. Acceptance & verification
- A slot holds at most one part; illegal fits are refused with a reason.
- Effects appear in travel arithmetic exactly once.
- Removing a part returns it to inventory; conservation balances.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Vehicles/` (create if absent).

## 5. Risks
Colliding with CF-P6 → armor grades are excluded; the claim must state that boundary.
Free power creep → trade-off rows are mandatory per fitting and visible in Plan 73's registry inputs.

---

## 6. Expanded census (1 files · 435 lines)

Scope: `Assets/Ashfall.Core/Vehicles/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `VehicleCustomizationSystem.cs` | 435 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `vehicle_modifications.json` | object[2 keys] |
| `vehicles.json` | object[3 keys] |
| `vehicle_armor_grades.json` | object[3 keys] |
| `vehicle_modules.json` | object[2 keys] |

**State surfaces:** `VehicleCustomizationSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Vehicles/` |
| Test references | 1 name references across the test tree |
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
Governed artifacts: 8. Other plans referencing them: **6**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-EXPEDITION-VEHICLE-TRUTH-219` | 5 |
| `PLAN-TRANSPORT-EXPEDITION-30` | 3 |
| `PLAN-MOD-CONTENT-BOUNDARY-92` | 2 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 1 |
| `PLAN-COMBAT-DEPTH-62` | 1 |
| `PLAN-SKY-ARMOR-TRUTH-256` | 1 |

**Governed artifacts (first 12):**

| Path |
|---|
| `Assets/Ashfall.Core/Vehicles/VehicleCustomizationSystem.cs` |
| `PLAN-VEHICLE-CUSTOMIZATION-TRUTH-154_APPENDIX-A_SCAFFOLD.md` |
| `VehicleCustomizationSystem.cs` |
| `Vehicles/VehicleCustomizationSystem.cs` |
| `vehicle_armor_grades.json` |
| `vehicle_modifications.json` |
| `vehicle_modules.json` |
| `vehicles.json` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `VCT-154A` | no name match — resolve at claim time |
| `VCT-154B` | no name match — resolve at claim time |
| `VCT-154C` | no name match — resolve at claim time |
| `VCT-154D` | no name match — resolve at claim time |
| `VCT-154E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 6. Host files: **8** · Test files: **10** · Data files: **23**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 8 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/ExpeditionHostSession.cs`, `src/Host/HostCli.ExpeditionPlaytest.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Host/HostCli.VehicleGarage.cs` |
| Tests (`Ashfall.Core.Tests/`) | 10 | `Ashfall.Core.Tests/Campaign/Plans50_53_SharedIntegrationTests.cs`, `Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs`, `Ashfall.Core.Tests/ExpeditionVehicleSystemTests.cs`, `Ashfall.Core.Tests/Expeditions/Plan213VehicleArmorGradeTests.cs`, `Ashfall.Core.Tests/Integration/Plans46_49_CrossSystemIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 23 | `Assets/StreamingAssets/Data/confession_secrets.json`, `Assets/StreamingAssets/Data/dose_locations.json`, `Assets/StreamingAssets/Data/environmental_atmosphere_expansion.json`, `Assets/StreamingAssets/Data/field_guide.json`, `Assets/StreamingAssets/Data/item_description_texts.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `dose_ledger` |
| `expedition` |
| `expedition_stealth` |
| `field_guide` |
| `fluid_logistics` |
| `shelter_atmosphere` |
| `vehicle_garage` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **18** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--atmosphere-selftest` |
| `--dose-ledger-selftest` |
| `--dose-uitest` |
| `--expedition-encounter-bridge-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--expedition-playtest-selftest` |
| `--expedition-selftest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **13**.

| Event | First declaration |
|---|---|
| `OnDoseChanged` | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` |
| `OnDoseCorrected` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnEnvironmentalCrisisTriggered` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnExpeditionCompleted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionFailed` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionStarted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionTick` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnItemAdded` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnItemDegraded` | `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` |
| `OnItemRemoved` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnRadiationDoseResetRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnVehicleBreakdown` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` |
| `Assets/StreamingAssets/Data/atmosphere_profiles.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/confession_secrets.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/dose_items.json` |
| `Assets/StreamingAssets/Data/dose_locations.json` |
| `Assets/StreamingAssets/Data/dose_quests.json` |
| `Assets/StreamingAssets/Data/dose_registers.json` |
| `Assets/StreamingAssets/Data/duty_roster_locations.json` |
| `Assets/StreamingAssets/Data/environmental_atmosphere_expansion.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **13** (150 files, 1056 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Campaign` | 32 | 187 |
| `DutyRoster` | 5 | 49 |
| `Economy` | 41 | 329 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Inventory` | 15 | 114 |
| `Presentation` | 1 | 5 |
| `Progression` | 11 | 83 |

**Verdict:** 1056 cases sit under matching regions — run those first (`Audio`, `Balance`, `Campaign`, `DutyRoster`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **447**
(230 of them panels/HUD).

| Host file |
|---|
| `src/Disease/DiseaseHostSession.cs` |
| `src/Dose/DoseRegisterSurface.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/AutopsyHostSession.cs` |
| `src/Host/BallisticShieldHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **43**, of which versioned-ladder sections:
**2**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `apprenticeship` | no |
| `archive_desk` | no |
| `autopsy` | no |
| `ballistic_shield` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `caravan_trade_network` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **15**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `cupola_foundry` |
| `deep_coast` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **118**
(CODEX_ONLY 28, GAMEPLAY_CONSUMED 62, OPTIONAL 8, UNRESOLVED 20).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `confession_secrets.json` | OPTIONAL |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |

**Verdict:** 20 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 6
**Surface:** save sections 43 (laddered 2) · RNG streams 15 · host files 23 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-VEHICLE-CUSTOMIZATION-TRUTH-154
wave: 12
status: PROPOSED — foreman claim required
packages: VCT-154A, VCT-154B, VCT-154C, VCT-154D, VCT-154E
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Dose/DoseRegisterSurface.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/anomalous_expedition_encounters.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/atmosphere_profiles.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --atmosphere-selftest
dependencies:
  - coordinate: 6 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 2 versioned save ladder(s) — extend, never fork
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
