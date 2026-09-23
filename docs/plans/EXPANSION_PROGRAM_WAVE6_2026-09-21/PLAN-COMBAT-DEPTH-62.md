# PLAN-COMBAT-DEPTH-62 — Ballistics, Breaching, Plume, Armor & Injury

**Wave 6 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-BASE-DEFENSE-RAIDS-61, PLAN-VERTICAL-BODY-INDUSTRY-05.
**Expanded appendix:** [`PLAN-COMBAT-DEPTH-62_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-COMBAT-DEPTH-62_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's combat depth
systems (1 authorities), each mapped to its parent-plan mechanic row.

**Implementation scaffold:** [`PLAN-COMBAT-DEPTH-62_APPENDIX-A_SCAFFOLD.md`](PLAN-COMBAT-DEPTH-62_APPENDIX-A_SCAFFOLD.md) — source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no real weapon/armor data, no gore, no damage-model rewrite of
`TacticalCombatSystem`.

## Outcome
Combat is deep in files (25+ in `Combat/`: `TacticalCombatSystem` partials,
`BallisticsSystem` orphan, `CombatBreachingEngine` orphan,
`ChemicalPlumeDispersionEngine` orphan, `BallisticShieldEngine`,
`ChemWarfareSystem`, `EnemyCompositionSelector`, `CombatPerks`,
`CombatDoctrineCapability`, `StealthSystem`, `SoundRangingThreatEngine`) but
thin in the loop: few player-facing choices beyond resolve. This plan adds
**preparation, tools and consequences** without changing the resolution
authority.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Loadout | inventory + equip gates | choose weapons/armor/tools | accuracy, protection, weight |
| Ballistics | `BallisticsSystem` | range/cover choices | hit/crit bands, ammo use |
| Breaching | `CombatBreachingEngine` | breach doors/walls | entry speed, noise, structural damage |
| Chemical | `ChemicalPlumeDispersionEngine` | gas/smoke, masks | area denial, contamination |
| Armor/shield | `BallisticShieldEngine`, armor grades | equip shields/plates | damage reduction, mobility |
| Injury | medical continuum | treat wounds | lasting conditions, prosthetics |
| Doctrine/perks | `CombatPerks`, `CombatDoctrineCapability` | train doctrine | team behavior, options |
| Stealth | `StealthSystem` | avoid, ambush | detection bands, first strike |

## Evidence
- Core: `Combat/` 25 files; orphan authorities `BallisticsSystem`, `CombatBreachingEngine`, `ChemicalPlumeDispersionEngine`.
- Data: `items.json` weapon/armor rows, `combat` catalogs, `vehicle_armor_grades.json` (CF-P6), `sound_ranging_catalog.json`.
- Sealed prior: `--combat-selftest`, `--combat-breaching-selftest`, `--chemical-recon-selftest`, tactical persistence tests, Plan 213 material quality feeding equipment.
- Contracts: body integrity (DEC-21/38) owns injury state; equipment condition owner exists.

## Packages
- **CD-62A** loadout surface: equip gating, weight, condition; no bypass of inventory authority.
- **CD-62B** ballistics consumer: range/cover modifiers visible; ammo and condition consumed.
- **CD-62C** breaching tools: entry routes with noise/structural consequences routed to shelter damage.
- **CD-62D** chemical warfare: plume, masks, decontamination; contamination routed to radiation/medical owners.
- **CD-62E** injury pipeline: wound → treatment → recovery/rehab → chronic/prosthetic; uses the clinic continuum.
- **CD-62F** doctrine/perks: training consumes time; options unlock (suppress, flank, cover) within resolution.
- **CD-62G** stealth/ambush: detection bands, first-strike outcome, escape.

## Acceptance & verification
- Preparation measurably changes outcomes; injuries persist and heal through canonical owners; determinism.
- `godot --headless --path . -- --combat-selftest`; `--combat-breaching-selftest`; `bash scripts/run_test.sh Ashfall.Core.Tests/Combat/`.

## Risks
Combat becoming the main game → raids/episodes remain optional pressure; survival stays the core loop.

---

## 6. Expanded census (19 files · 6,539 lines)

Scope: `Assets/Ashfall.Core/Combat/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · DTO/Type 1 · Demo 1 · Support 10 · System 6

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `BallisticShieldEngine.cs` | 437 | System | — | 0 | 0 | 2 |
| `BallisticsSystem.cs` | 331 | System | — | 0 | 0 | 0 |
| `BallisticsWorkbenchSystem.cs` | 572 | System | **yes** | 0 | 0 | 2 |
| `CombatAiMove.cs` | 65 | Support | — | 0 | 0 | 0 |
| `CombatBreachingEngine.cs` | 650 | System | — | 0 | 0 | 0 |
| `CombatCatalog.cs` | 670 | Catalog | — | 0 | 0 | 0 |
| `CombatDoctrineCapability.cs` | 45 | Support | — | 0 | 0 | 0 |
| `CombatFactionStandingBridge.cs` | 256 | Support | — | 0 | 1 | 0 |
| `CombatHeadlessDemo.cs` | 289 | Demo | — | 0 | 0 | 4 |
| `CombatPerks.cs` | 248 | Support | — | 0 | 0 | 2 |
| `CombatTypes.cs` | 401 | DTO/Type | — | 0 | 0 | 0 |
| `TacticalCombatSystem.Actions.cs` | 563 | Support | — | 0 | 0 | 0 |
| `TacticalCombatSystem.Breaching.cs` | 255 | Support | — | 0 | 0 | 0 |
| `TacticalCombatSystem.Damage.cs` | 357 | Support | — | 0 | 0 | 0 |
| `TacticalCombatSystem.Persistence.cs` | 425 | Support | — | 0 | 0 | 2 |
| `TacticalCombatSystem.Targeting.cs` | 131 | Support | — | 0 | 0 | 0 |
| `TacticalCombatSystem.cs` | 409 | System | **yes** | 0 | 0 | 0 |
| `WeaponConditionSystem.cs` | 320 | System | **yes** | 0 | 0 | 0 |
| `WeaponEquipmentBridge.cs` | 115 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 1 empty catches · 5 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `chemical_weapons.json` | object[2 keys] |
| `ballistic_shield_catalog.json` | object[2 keys] |
| `ballistics_workbench_catalog.json` | object[2 keys] |
| `breaching_equipment_catalog.json` | object[4 keys] |
| `combat_catalog.json` | object[6 keys] |
| `faction_combat_thresholds.json` | object[3 keys] |

**State surfaces:** `BallisticShieldEngine.cs`, `BallisticsWorkbenchSystem.cs`, `CombatHeadlessDemo.cs`, `CombatPerks.cs`, `TacticalCombatSystem.Persistence.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Combat/` |
| Test references | 48 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 1 empty-catch sites routed through Plan 35's rules |
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

## 11. Tier-2: intra-domain reference graph

Computed across 25 domain files: **175 type-reference edges**.

| File | Lines | In-degree | Out-degree |
|---|---:|---:|---:|
| `CombatCatalog.cs` | 670 | 10 | 11 |
| `CombatBreachingEngine.cs` | 650 | 4 | 2 |
| `BallisticsWorkbenchSystem.cs` | 572 | 0 | 7 |
| `TacticalCombatSystem.Actions.cs` | 563 | 11 | 17 |
| `BallisticShieldEngine.cs` | 437 | 0 | 0 |
| `TacticalCombatSystem.Persistence.cs` | 425 | 11 | 25 |
| `TacticalCombatSystem.cs` | 409 | 11 | 20 |
| `CombatTypes.cs` | 401 | 70 | 9 |
| `TacticalCombatSystem.Damage.cs` | 357 | 11 | 15 |
| `StealthSystem.cs` | 340 | 1 | 0 |
| `ChemWarfareSystem.cs` | 336 | 0 | 1 |
| `SoundRangingThreatEngine.cs` | 335 | 0 | 2 |

**Highest-coupling files (in×2 + out):**

- `CombatTypes.cs` — in 70, out 9
- `TacticalCombatSystem.Persistence.cs` — in 11, out 25
- `TacticalCombatSystem.cs` — in 11, out 20
- `TacticalCombatSystem.Actions.cs` — in 11, out 17
- `TacticalCombatSystem.Breaching.cs` — in 11, out 17
- `TacticalCombatSystem.Damage.cs` — in 11, out 15
- `CombatCatalog.cs` — in 10, out 11
- `TacticalCombatSystem.Targeting.cs` — in 11, out 9
- `WeaponConditionSystem.cs` — in 10, out 4
- `CombatHeadlessDemo.cs` — in 0, out 15

**Ordering implication:** wire in-dependent files first (high in-degree, low
out-degree), then the terminal consumers. A file with many outgoing edges is a
dependency: it should be sealed or verified before its dependents claim work.

---

## 12. Cross-plan coupling

Domain files: 19. Other plans referencing their names: **7**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-COMBAT-FAMILY-TRUTH-273` | 19 |
| `PLAN-BALLISTICS-WORKBENCH-TRUTH-184` | 3 |
| `PLAN-BASE-DEFENSE-RAIDS-61` | 2 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 2 |
| `PLAN-WEAPON-CONDITION-TRUTH-242` | 2 |
| `PLAN-SILENT-FAILURE-35` | 1 |
| `PLAN-DEV-TOOLING-TRUTH-75` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CD-62A` | `WeaponConditionSystem.cs`, `WeaponEquipmentBridge.cs` |
| `CD-62B` | `BallisticShieldEngine.cs`, `BallisticsSystem.cs`, `BallisticsWorkbenchSystem.cs` |
| `CD-62C` | `CombatBreachingEngine.cs`, `TacticalCombatSystem.Breaching.cs`, `TacticalCombatSystem.Damage.cs` |
| `CD-62D` | no name match — resolve at claim time |
| `CD-62E` | no name match — resolve at claim time |
| `CD-62F` | `CombatDoctrineCapability.cs`, `CombatPerks.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 17. Host files: **30** · Test files: **35** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 30 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Audio/ExpansionAudioBridge.cs`, `src/Host/BallisticShieldHostSession.cs`, `src/Host/CombatHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 35 | `Ashfall.Core.Tests/Combat/ChemWarfareSystemTests.cs`, `Ashfall.Core.Tests/Combat/CombatBreachingEngineTests.cs`, `Ashfall.Core.Tests/Combat/CombatFactionStandingBridgeTests.cs`, `Ashfall.Core.Tests/Combat/Plan10_11CombatExplorationIntegrationTests.cs`, `Ashfall.Core.Tests/Combat/Plan123SoundRangingThreatEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **9** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `ballistic_shield` |
| `ballistics_workbench` |
| `chem_warfare` |
| `combat` |
| `equipment` |
| `equipment_condition` |
| `expedition_stealth` |
| `faction_espionage` |
| `sound_ranging` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
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
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_territory.json` |
| `Assets/StreamingAssets/Data/faction_war_communiques.json` |
| `Assets/StreamingAssets/Data/faction_war_dialogue.json` |
| `Assets/StreamingAssets/Data/faction_war_events.json` |

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

Host files (`src/`) whose names share a domain token: **40**
(17 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Host/BallisticShieldHostSession.cs` |
| `src/Host/BallisticShieldSaveStore.cs` |
| `src/Host/ChemWarfareSaveStore.cs` |
| `src/Host/CombatHostSession.cs` |
| `src/Host/CombatSaveStore.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/EquipmentConditionHostSession.cs` |
| `src/Host/FactionBranchHostSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HostCli.ExpansionDepth.cs` |
| `src/Host/HostCli.FactionCommuniqueSelfTests.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **9**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `ballistic_shield` | no |
| `ballistics_workbench` | no |
| `chem_warfare` | no |
| `combat` | no |
| `equipment` | no |
| `equipment_condition` | no |
| `expedition_stealth` | no |
| `faction_espionage` | no |
| `sound_ranging` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `combat` |
| `sound_ranging` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **28**
(CODEX_ONLY 4, GAMEPLAY_CONSUMED 18, UNRESOLVED 6).

| Catalog | Classification |
|---|---|
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_territory.json` | UNRESOLVED |
| `faction_war_communiques.json` | GAMEPLAY_CONSUMED |
| `faction_war_dialogue.json` | GAMEPLAY_CONSUMED |
| `faction_war_events.json` | GAMEPLAY_CONSUMED |
| `faction_war_journal.json` | GAMEPLAY_CONSUMED |
| `faction_war_location_overrides.json` | GAMEPLAY_CONSUMED |

**Verdict:** 6 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 7
**Surface:** save sections 9 (laddered 0) · RNG streams 2 · host files 16 · catalogs 22 · test regions 2 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-COMBAT-DEPTH-62
wave: 6
status: PROPOSED — foreman claim required
packages: CD-62A, CD-62B, CD-62C, CD-62D, CD-62E, CD-62F, CD-62G
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Host/BallisticShieldHostSession.cs  # §19 candidate host surface
  - src/Host/BallisticShieldSaveStore.cs  # §19 candidate host surface
  - src/Host/ChemWarfareSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/ballistic_shield_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/ballistics_workbench_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Combat/
  - godot --headless --path . -- --combat-breaching-selftest
dependencies:
  - coordinate: 7 other plan(s) name these artifacts (§12)
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
