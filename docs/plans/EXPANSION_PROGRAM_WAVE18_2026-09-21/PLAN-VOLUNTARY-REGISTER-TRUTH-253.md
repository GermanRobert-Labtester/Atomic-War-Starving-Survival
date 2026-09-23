# PLAN-VOLUNTARY-REGISTER-TRUTH-253 — Volunteers, Consent & Duty Allocation

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SURVIVOR-ROSTER-TRUTH-244, PLAN-DUTY-ROSTER-TRUTH-101, PLAN-MORALE-UNREST-TRUTH-129, PLAN-INSTITUTIONS-TRUTH-141.
**Non-goals:** no person registry (Plan 244), no shift model (Plan 101), no
offices (Plan 141).

## 1. Outcome
`VoluntaryRegisterSystem.cs` (**149 lines**, Core root) is reachable and
unaddressed: who has volunteered for hazardous or additional duty, and how
consent interacts with orders. It is the counterpart of Plan 198's compelled
labor — and the place where "volunteer burnout" becomes visible rather than
hidden.

| Deliverable | Detail |
|---|---|
| Register model | volunteer entries per person with a duty class, a validity window, and a withdrawal rule |
| Allocation | duty assignment prefers volunteers where the roster permits; the preference is visible, not hidden |
| Consent rules | hazardous assignments need consent or a documented emergency override (Plan 37/69 authority) |
| Strain | repeat volunteering raises strain through Plan 64/129 owners as a typed input |
| Save truth | register and consent state restore; no re-volunteer on load |

## 2. Evidence
- `Assets/Ashfall.Core/VoluntaryRegisterSystem.cs` (149 lines; unaddressed — Wave 18 audit).
- Plan 244's registry identifies people; Plan 101's roster consumes preferences.
- Plan 198's compelled labor is the mirror case — boundary stated.
- Plan 64/129 receive strain effects.

## 3. Packages
- **VRT-253A** register model + duty classes.
- **VRT-253B** allocation-preference tests.
- **VRT-253C** consent/override rules per authority.
- **VRT-253D** strain input tests.
- **VRT-253E** save round-trip; no re-volunteer on load.

## 4. Acceptance & verification
- Volunteers are preferred where roster permits; overrides follow documented authority.
- Strain appears in named owners; save/load preserves consent state.
- `bash scripts/run_test.sh` on the roster/survivors region.

## 5. Risks
Hidden conscription → consent/override rules are explicit and tested.
Strain duplication → typed input only.

---

## 6. Expanded census (12 files · 4,152 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · DTO/Type 1 · Demo 1 · Save 1 · Support 4 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `DutyHourLedger.cs` | 126 | Support | — | 0 | 0 | 0 |
| `DutyRosterAssignmentEngine.cs` | 320 | System | — | 0 | 0 | 0 |
| `DutyRosterCatalog.cs` | 227 | Catalog | — | 0 | 0 | 0 |
| `DutyRosterChartEngine.cs` | 298 | System | — | 0 | 0 | 0 |
| `DutyRosterHeadlessDemo.cs` | 153 | Demo | — | 0 | 0 | 4 |
| `DutyRosterHoldfastBridge.cs` | 259 | Support | — | 0 | 0 | 0 |
| `DutyRosterIds.cs` | 129 | DTO/Type | — | 0 | 0 | 0 |
| `DutyRosterOverflowEngine.cs` | 92 | System | — | 0 | 0 | 2 |
| `DutyRosterQuestRuntime.cs` | 484 | Support | — | 0 | 0 | 2 |
| `DutyRosterSave.cs` | 229 | Save | — | 0 | 0 | 13 |
| `DutyRosterSystem.cs` | 925 | System | — | 0 | 0 | 6 |
| `FitnessForDutyModel.cs` | 910 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 5 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `dose_registers.json` | object[7 keys] |
| `duty_roster_locations.json` | object[2 keys] |
| `duty_roster_marks.json` | array[43] |
| `duty_roster_quests.json` | object[2 keys] |
| `duty_roster_seasons.json` | array[8] |
| `duty_roles.json` | object[6 keys] |

**State surfaces:** `DutyRosterHeadlessDemo.cs`, `DutyRosterOverflowEngine.cs`, `DutyRosterQuestRuntime.cs`, `DutyRosterSave.cs`, `DutyRosterSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
| Test references | 88 name references across the test tree |
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

(Corrected scope: the premise system lives at the Core root, so a directory scan
was empty.)

Domain files: 2 — `DoseRegistersCatalog.cs`, `VoluntaryRegisterSystem.cs`.
Other plans referencing their names: **2**.

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-MUTATION-HEREDITY-81` | 1 |
| `PLAN-CORE-ROOT-FAMILY-TRUTH-262` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `VRT-253A` | `DoseRegistersCatalog.cs`, `VoluntaryRegisterSystem.cs` |
| `VRT-253B` | no name match — resolve at claim time |
| `VRT-253C` | no name match — resolve at claim time |
| `VRT-253D` | no name match — resolve at claim time |
| `VRT-253E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 14; intra-domain edges: **24**; isolated files:
**3**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `DutyRosterAssignmentEngine` | `DutyRosterIds` |
| `DutyRosterAssignmentEngine` | `DutyRosterSystem` |
| `DutyRosterCatalog` | `DutyRosterIds` |
| `DutyRosterChartEngine` | `DutyRosterAssignmentEngine` |
| `DutyRosterChartEngine` | `DutyRosterIds` |
| `DutyRosterChartEngine` | `DutyRosterSystem` |
| `DutyRosterHeadlessDemo` | `DutyRosterIds` |
| `DutyRosterHeadlessDemo` | `DutyRosterSystem` |
| `DutyRosterHoldfastBridge` | `DutyRosterIds` |
| `DutyRosterHoldfastBridge` | `DutyRosterQuestRuntime` |
| `DutyRosterHoldfastBridge` | `DutyRosterSystem` |
| `DutyRosterIds` | `DutyRosterSystem` |
| `DutyRosterOverflowEngine` | `DutyRosterSystem` |
| `DutyRosterQuestRuntime` | `DutyRosterCatalog` |
| `DutyRosterQuestRuntime` | `DutyRosterHoldfastBridge` |
| `DutyRosterQuestRuntime` | `DutyRosterIds` |
| `DutyRosterQuestRuntime` | `DutyRosterSystem` |
| `DutyRosterSave` | `DutyRosterQuestRuntime` |
| `DutyRosterSave` | `DutyRosterSystem` |
| `DutyRosterSystem` | `DutyRosterAssignmentEngine` |
| `DutyRosterSystem` | `DutyRosterChartEngine` |
| `DutyRosterSystem` | `DutyRosterIds` |
| `DutyRosterSystem` | `DutyRosterOverflowEngine` |
| `FitnessForDutyModel` | `DutyRosterIds` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `DutyRosterIds` | 8 |
| `DutyRosterSystem` | 8 |
| `DutyRosterAssignmentEngine` | 2 |
| `DutyRosterQuestRuntime` | 2 |
| `DutyRosterCatalog` | 1 |
| `DutyRosterChartEngine` | 1 |
| `DutyRosterHoldfastBridge` | 1 |
| `DutyRosterOverflowEngine` | 1 |
| `DoseRegistersCatalog` | 0 |
| `DutyHourLedger` | 0 |

**Class split:** hub 8 · sink 0 · source 3 · isolated 3.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 14. Host files: **24** · Test files: **54** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 24 | `src/Host/ArchiveDeskHostSession.cs`, `src/Host/ContractorRosterHostSession.cs`, `src/Host/DoseLedgerHostSession.cs`, `src/Host/DutyRosterHostSession.cs`, `src/Host/DutyRosterSaveStore.cs` |
| Tests (`Ashfall.Core.Tests/`) | 54 | `Ashfall.Core.Tests/ApprenticeshipIntegrationTests.cs`, `Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`, `Ashfall.Core.Tests/ArchiveDeskSystemTests.cs`, `Ashfall.Core.Tests/ArchiveInksCatalogTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `contractor_roster` |
| `dose_ledger` |
| `duty_roster` |
| `expansion_quest` |
| `holdfast` |
| `holdfast_trade` |
| `shelter_assignment` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **16** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--dose-ledger-selftest` |
| `--dose-uitest` |
| `--duty-roster-loop-selftest` |
| `--duty-roster-save-selftest` |
| `--duty-roster-selftest` |
| `--duty-roster-uitest` |
| `--holdfast-briefing` |
| `--holdfast-runtime-selftest` |
| `--holdfast-runtime-ui-test` |
| `--holdfast-runtime-uitest` |
| `--holdfast-save-selftest` |
| `--holdfast-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **17**.

| Event | First declaration |
|---|---|
| `OnAssignmentChanged` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnDoseChanged` | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` |
| `OnDoseCorrected` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnDutyVacated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnLedgerCalibrated` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnLedgerTampered` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnQuestChoiceTaken` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |
| `OnQuestCompleted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestFailed` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnQuestStageChanged` | `Assets/Ashfall.Core/HoldfastQuestSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/dose_items.json` |
| `Assets/StreamingAssets/Data/dose_locations.json` |
| `Assets/StreamingAssets/Data/dose_quests.json` |
| `Assets/StreamingAssets/Data/dose_registers.json` |
| `Assets/StreamingAssets/Data/duty_roles.json` |
| `Assets/StreamingAssets/Data/duty_roster_locations.json` |
| `Assets/StreamingAssets/Data/duty_roster_marks.json` |
| `Assets/StreamingAssets/Data/duty_roster_quests.json` |
| `Assets/StreamingAssets/Data/duty_roster_seasons.json` |
| `Assets/StreamingAssets/Data/dynamic_quest_templates.json` |
| `Assets/StreamingAssets/Data/holdfast_factions.json` |
| `Assets/StreamingAssets/Data/holdfast_flavor.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (6 files, 62 cases).

| Region | Files | Cases |
|---|---:|---:|
| `DutyRoster` | 5 | 49 |
| `Holdfast` | 1 | 13 |

**Verdict:** 62 cases sit under matching regions — run those first (`DutyRoster`, `Holdfast`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **42**
(11 of them panels/HUD).

| Host file |
|---|
| `src/Dose/DoseRegisterSurface.cs` |
| `src/Host/ContractorRosterHostSession.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/DoseLedgerHostSession.cs` |
| `src/Host/DoseLedgerSaveStore.cs` |
| `src/Host/DutyRosterHostSession.cs` |
| `src/Host/DutyRosterSaveStore.cs` |
| `src/Host/DynamicQuestSaveStore.cs` |
| `src/Host/ExpansionQuestHostSession.cs` |
| `src/Host/ExpansionQuestSaveStore.cs` |
| `src/Host/HoldfastBriefingView.cs` |
| `src/Host/HoldfastDispatchLog.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **7**, of which versioned-ladder sections:
**2**.

| Section key | Laddered |
|---|---|
| `contractor_roster` | no |
| `dose_ledger` | yes |
| `duty_roster` | no |
| `expansion_quest` | no |
| `holdfast` | yes |
| `holdfast_trade` | no |
| `shelter_assignment` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `duty_roster` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **18**
(CODEX_ONLY 2, GAMEPLAY_CONSUMED 13, OPTIONAL 1, UNRESOLVED 2).

| Catalog | Classification |
|---|---|
| `dose_items.json` | GAMEPLAY_CONSUMED |
| `dose_locations.json` | GAMEPLAY_CONSUMED |
| `dose_quests.json` | GAMEPLAY_CONSUMED |
| `dose_registers.json` | GAMEPLAY_CONSUMED |
| `duty_roster_locations.json` | GAMEPLAY_CONSUMED |
| `duty_roster_marks.json` | GAMEPLAY_CONSUMED |
| `duty_roster_quests.json` | GAMEPLAY_CONSUMED |
| `duty_roster_seasons.json` | GAMEPLAY_CONSUMED |
| `holdfast_factions.json` | GAMEPLAY_CONSUMED |
| `holdfast_flavor.json` | GAMEPLAY_CONSUMED |

**Verdict:** 2 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 7 (laddered 2) · RNG streams 1 · host files 13 · catalogs 22 · test regions 2 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-VOLUNTARY-REGISTER-TRUTH-253
wave: 18
status: PROPOSED — foreman claim required
packages: VRT-253A, VRT-253B, VRT-253C, VRT-253D, VRT-253E
claim paths:
  - src/Dose/DoseRegisterSurface.cs  # §19 candidate host surface
  - src/Host/ContractorRosterHostSession.cs  # §19 candidate host surface
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/DoseLedgerHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/dose_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/dose_locations.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/DutyRoster/
  - godot --headless --path . -- --dose-ledger-selftest
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
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
