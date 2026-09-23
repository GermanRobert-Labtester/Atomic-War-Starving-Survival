# PLAN-COMBAT-FAMILY-TRUTH-273 — Tactical AI, Catalogs & Partial Classes

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-COMBAT-DEPTH-62, PLAN-FACTION-BRANCH-STATUS-TRUTH-228, PLAN-DETERMINISM-CROSS-HOST-89.
**Non-goals:** no combat redesign; the family is audited for AI determinism and
catalog wiring.

## 1. Outcome
**12 `Combat/` files** are referenced by no plan: `CombatAiMove`,
`CombatCatalog`, `CombatTypes`, `SoundRangingCatalog`,
`CombatFactionStandingBridge`, `CombatHeadlessDemo`, and the
`TacticalCombatSystem.*` partials plus `BallisticsWorkbench` relatives. AI move
selection is a determinism risk; the bridge is a faction-write risk.

| Deliverable | Detail |
|---|---|
| AI determinism | `CombatAiMove` selection is seeded and stable; paired-run fixture |
| Partial inventory | every `TacticalCombatSystem.*` partial accounted for; no orphaned branch |
| Catalog wiring | combat/sound catalogs resolve to their systems |
| Bridge audit | standing bridge writes only through Plan 29/228 owners |
| Demo truth | headless demo resolves to a verb (Plan 86 pattern) |

## 2. Evidence
- 12 `Combat/` basenames absent from every plan body (Wave 19 file-level audit).
- Plan 62 owns combat; Plan 89 supplies the paired-run method.
- Plan 184's workbench references the ammunition side.

## 3. Packages
- **CBF-273A** AI determinism fixture.
- **CBF-273B** partial inventory.
- **CBF-273C** catalog wiring tests.
- **CBF-273D** bridge write-path audit.
- **CBF-273E** demo→verb check.

## 4. Acceptance & verification
- AI picks are seed-stable; partials are complete; catalogs resolve; bridge writes are owner-routed.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Combat/`.

## 5. Risks
Nondeterministic AI → paired-run fixture.
Standing leakage → write-path audit.

---

## 6. Expanded census (25 family files · 8,317 lines)

Scope: files under `Assets/Ashfall.Core/Combat/` whose basename is referenced
by no plan body (the Wave 19 family definition). Class distribution: Support 11 · System 10 · Catalog 2 · Demo 1 · DTO/Type 1.

| File | Lines | Class | Banned refs | Empty catches | Capture/Restore |
|---|---:|---|---:|---:|---:|
| `BallisticShieldEngine.cs` | 437 | System | 0 | 0 | 2 |
| `BallisticsSystem.cs` | 331 | System | 0 | 0 | 0 |
| `BallisticsWorkbenchSystem.cs` | 572 | System | 0 | 0 | 2 |
| `ChemWarfareSystem.cs` | 336 | System | 0 | 0 | 2 |
| `ChemicalPlumeDispersionEngine.cs` | 286 | System | 0 | 0 | 0 |
| `CombatAiMove.cs` | 65 | Support | 0 | 0 | 0 |
| `CombatBreachingEngine.cs` | 650 | System | 0 | 0 | 0 |
| `CombatCatalog.cs` | 670 | Catalog | 0 | 0 | 0 |
| `CombatDoctrineCapability.cs` | 45 | Support | 0 | 0 | 0 |
| `CombatFactionStandingBridge.cs` | 256 | Support | 0 | 1 | 0 |
| `CombatHeadlessDemo.cs` | 289 | Demo | 0 | 0 | 4 |
| `CombatPerks.cs` | 248 | Support | 0 | 0 | 2 |
| `CombatTypes.cs` | 401 | DTO/Type | 0 | 0 | 0 |
| `EnemyCompositionSelector.cs` | 211 | Support | 0 | 0 | 0 |
| `SoundRangingCatalog.cs` | 270 | Catalog | 0 | 0 | 0 |
| `SoundRangingThreatEngine.cs` | 335 | System | 0 | 0 | 2 |
| `StealthSystem.cs` | 340 | System | 0 | 0 | 2 |
| `TacticalCombatSystem.Actions.cs` | 563 | Support | 0 | 0 | 0 |
| `TacticalCombatSystem.Breaching.cs` | 255 | Support | 0 | 0 | 0 |
| `TacticalCombatSystem.Damage.cs` | 357 | Support | 0 | 0 | 0 |
| `TacticalCombatSystem.Persistence.cs` | 425 | Support | 0 | 0 | 2 |
| `TacticalCombatSystem.Targeting.cs` | 131 | Support | 0 | 0 | 0 |
| `TacticalCombatSystem.cs` | 409 | System | 0 | 0 | 0 |
| `WeaponConditionSystem.cs` | 320 | System | 0 | 0 | 0 |
| `WeaponEquipmentBridge.cs` | 115 | Support | 0 | 0 | 0 |

**Census totals:** 0 banned nondeterministic references · 1 empty-catch sites · 8 files with capture/restore methods.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `combat_catalog.json` | object[6 keys] |
| `faction_combat_thresholds.json` | object[3 keys] |

**State surfaces (capture/restore present):**

- `BallisticShieldEngine.cs`
- `BallisticsWorkbenchSystem.cs`
- `ChemWarfareSystem.cs`
- `CombatHeadlessDemo.cs`
- `CombatPerks.cs`
- `SoundRangingThreatEngine.cs`
- `StealthSystem.cs`
- `TacticalCombatSystem.Persistence.cs`

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/Combat/` |
| Family files referenced by tests | 61 name references across the test tree |
| Determinism scan | 0 banned references to fix or justify |
| Failure scan | 1 empty-catch sites to route through Plan 35's rules |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census first (section 6) — classify every family file; no edits in this step.
2. Catalogs and loaders: prove a consumer or report the file as inert.
3. DTO/Type files: round-trip or consume-only proof; unknown values fail typed.
4. System files: confirm the single owner per state; remove duplicated stores.
5. Demo/tooling files: resolve to a real CLI verb or retire (Plan 86 pattern).
6. Regression: focused region plus this family census regenerated.

## 10. Acceptance matrix

| File class | Acceptance |
|---|---|
| Catalog | resolves through a loader; malformed fixture fails typed with the field named |
| Loader | valid/invalid fixture pair; unknown id names the field |
| DTO/Type | round-trip or consume-only proof; no orphan type |
| Save | capture/restore round-trip; key per Plan 1 Appendix Q |
| System | one owner per state; no parallel store |
| Demo | resolves to an existing verb or is retired |
| Support | consumed by a system or reported ownerless |

**Non-goals unchanged:** this expansion adds census and verification detail; it
does not widen the plan's scope or create new authorities.

---

## 12. Cross-plan coupling

This is a family-survey plan; the domain set is the plan's own `.cs` enumeration
(20 files). Other plans referencing those names: **10**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-COMBAT-DEPTH-62` | 19 |
| `PLAN-BASE-DEFENSE-RAIDS-61` | 4 |
| `PLAN-BALLISTICS-WORKBENCH-TRUTH-184` | 3 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 2 |
| `PLAN-WEAPON-CONDITION-TRUTH-242` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-SILENT-FAILURE-35` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CBF-273A` | no name match — resolve at claim time |
| `CBF-273B` | no name match — resolve at claim time |
| `CBF-273C` | `CombatCatalog.cs`, `SoundRangingCatalog.cs` |
| `CBF-273D` | `CombatFactionStandingBridge.cs`, `WeaponEquipmentBridge.cs` |
| `CBF-273E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 20; intra-domain edges: **19**; isolated files:
**6**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `BallisticsWorkbenchSystem` | `TacticalCombatSystem` |
| `CombatAiMove` | `TacticalCombatSystem` |
| `CombatCatalog` | `CombatAiMove` |
| `CombatCatalog` | `TacticalCombatSystem` |
| `CombatDoctrineCapability` | `TacticalCombatSystem` |
| `CombatHeadlessDemo` | `CombatCatalog` |
| `CombatHeadlessDemo` | `TacticalCombatSystem` |
| `CombatHeadlessDemo` | `WeaponConditionSystem` |
| `CombatTypes` | `CombatCatalog` |
| `CombatTypes` | `TacticalCombatSystem` |
| `SoundRangingThreatEngine` | `SoundRangingCatalog` |
| `TacticalCombatSystem` | `BallisticsSystem` |
| `TacticalCombatSystem` | `CombatCatalog` |
| `TacticalCombatSystem` | `CombatDoctrineCapability` |
| `TacticalCombatSystem` | `CombatPerks` |
| `TacticalCombatSystem` | `WeaponConditionSystem` |
| `WeaponConditionSystem` | `CombatCatalog` |
| `WeaponConditionSystem` | `StealthSystem` |
| `WeaponEquipmentBridge` | `WeaponConditionSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `TacticalCombatSystem` | 6 |
| `CombatCatalog` | 4 |
| `WeaponConditionSystem` | 3 |
| `BallisticsSystem` | 1 |
| `CombatAiMove` | 1 |
| `CombatDoctrineCapability` | 1 |
| `CombatPerks` | 1 |
| `SoundRangingCatalog` | 1 |
| `StealthSystem` | 1 |
| `BallisticShieldEngine` | 0 |

**Class split:** hub 5 · sink 4 · source 5 · isolated 6.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 20. Host files: **32** · Test files: **37** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 32 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Audio/ExpansionAudioBridge.cs`, `src/Host/BallisticShieldHostSession.cs`, `src/Host/CombatHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 37 | `Ashfall.Core.Tests/Combat/ChemWarfareSystemTests.cs`, `Ashfall.Core.Tests/Combat/ChemicalPlumeDispersionEngineTests.cs`, `Ashfall.Core.Tests/Combat/CombatBreachingEngineTests.cs`, `Ashfall.Core.Tests/Combat/CombatFactionStandingBridgeTests.cs`, `Ashfall.Core.Tests/Combat/Plan10_11CombatExplorationIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **12** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `ballistic_shield` |
| `ballistics_workbench` |
| `chem_warfare` |
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `combat` |
| `equipment` |
| `equipment_condition` |
| `expedition_stealth` |
| `faction_espionage` |
| `sound_ranging` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **8** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--chemical-dependency-save-selftest` |
| `--combat-breaching-selftest` |
| `--combat-selftest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--ice-road-tick-demo` |
| `--sound-ranging-selftest` |
| `--standing-record-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **15**.

| Event | First declaration |
|---|---|
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnCombatPenaltyChanged` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnCombatPerkEarned` | `Assets/Ashfall.Core/Combat/CombatPerks.cs` |
| `OnConditionChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnConditionStarted` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnConditionStopped` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnDeviceConditionChanged` | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` |
| `OnDoctrineChanged` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnEquipmentChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnEquipmentDamaged` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/ballistic_shield_catalog.json` |
| `Assets/StreamingAssets/Data/ballistics_workbench_catalog.json` |
| `Assets/StreamingAssets/Data/breaching_equipment_catalog.json` |
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_territory.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (11 files, 88 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Combat` | 10 | 84 |
| `Equipment` | 1 | 4 |

**Verdict:** 88 cases sit under matching regions — run those first (`Combat`, `Equipment`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **52**
(21 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Host/BallisticShieldHostSession.cs` |
| `src/Host/BallisticShieldSaveStore.cs` |
| `src/Host/ChemWarfareSaveStore.cs` |
| `src/Host/ChemicalDependencyHostSession.cs` |
| `src/Host/ChemicalDependencySaveSelfTest.cs` |
| `src/Host/ChemicalDependencySaveStore.cs` |
| `src/Host/ChemicalReconHostSession.cs` |
| `src/Host/ChemicalReconSaveStore.cs` |
| `src/Host/ChemicalSynthesisHostSession.cs` |
| `src/Host/ChemicalSynthesisSaveStore.cs` |
| `src/Host/CombatHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **12**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `ballistic_shield` | no |
| `ballistics_workbench` | no |
| `chem_warfare` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `combat` | no |
| `equipment` | no |
| `equipment_condition` | no |
| `expedition_stealth` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `combat` |
| `mineral_chemical` |
| `sound_ranging` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **32**
(CODEX_ONLY 4, GAMEPLAY_CONSUMED 21, UNRESOLVED 7).

| Catalog | Classification |
|---|---|
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_territory.json` | UNRESOLVED |
| `faction_war_communiques.json` | GAMEPLAY_CONSUMED |
| `faction_war_dialogue.json` | GAMEPLAY_CONSUMED |

**Verdict:** 7 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 10
**Surface:** save sections 12 (laddered 0) · RNG streams 3 · host files 17 · catalogs 22 · test regions 2 · flags 8

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-COMBAT-FAMILY-TRUTH-273
wave: 19
status: PROPOSED — foreman claim required
packages: CBF-273A, CBF-273B, CBF-273C, CBF-273D, CBF-273E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Host/BallisticShieldHostSession.cs  # §19 candidate host surface
  - src/Host/BallisticShieldSaveStore.cs  # §19 candidate host surface
  - src/Host/ChemWarfareSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/ballistic_shield_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/ballistics_workbench_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Combat/
  - godot --headless --path . -- --chemical-dependency-save-selftest
dependencies:
  - coordinate: 10 other plan(s) name these artifacts (§12)
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
