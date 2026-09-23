# PLAN-BASE-DEFENSE-RAIDS-61 — Perimeter, Watch, Raids & Sky Defense

**Wave 6 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-WARLORDS-DIPLOMACY-29, PLAN-SHELTER-ARCHITECTURE-40.
**Expanded appendix:** [`PLAN-BASE-DEFENSE-RAIDS-61_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-BASE-DEFENSE-RAIDS-61_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's base defense & raids
systems (3 authorities), each mapped to its parent-plan mechanic row.

**Non-goals:** no tactical RTS layer, no second combat authority
(`TacticalCombatSystem` remains canonical), no real weapons/tactics.

## Outcome
Defense exists in pieces: `ShelterSecuritySystem` (zones, clearance, lockdown,
alarms — sealed), `AirlockSecuritySystem`, `PerimeterEarlyWarningEngine`
(orphan), `NightWatchPatrolReadinessEngine` (orphan), `SkyDefense` battery +
ordnance catalog, `Defense` systems, `SoundRangingThreatEngine`,
`StealthSystem`, and patrol/territory hooks. This plan turns them into a
**defense cycle the player runs**: detect → alarm → muster → fight → repair →
learn.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Early warning | `PerimeterEarlyWarningEngine`, sensors, sound ranging | place sensors, set watch | warning time, false alarms |
| Watch & muster | `NightWatchPatrolReadinessEngine`, duty roster | staff watch, call muster | readiness, response speed |
| Security posture | `ShelterSecuritySystem` | zones, lockdown, clearance | access control under attack |
| Air defense | `SkyDefenseBatterySystem` + ordnance catalog | arm/assign battery | intercept, debris risk |
| Breach & fight | `TacticalCombatSystem`, `CombatBreachingEngine` | fight at the airlock | casualties, damage, capture |
| Aftermath | shelter maintenance, medical, memorial | repair, treat, bury | recovery, morale |
| Doctrine | journal/briefing | review the raid | next-raid preparation |

## Evidence
- Core: `Defense/PerimeterEarlyWarningEngine.cs` (orphan, 9 tests), `NightWatchPatrolReadinessEngine.cs` (orphan, 9), `ShelterSecuritySystem` (sealed 9/9), `SkyDefense/*`, `Combat/CombatBreachingEngine.cs` (orphan), `SoundRangingThreatEngine`, `StealthSystem`.
- Data: `shelter_security_zones.json`, `sky_defense` catalogs, `faction_combat_thresholds.json`, `night_watch`/patrol rows if authored.
- Sealed prior: Plan 138 zones (9/9), Plan 122–125 sound ranging, `OnTerritorialClashOccurred` routing.
- Guarantees: alarm relay/lockdown bridges live; violence stops at typed threats in social systems.

## Packages
- **BD-61A** warning net: sensors + watch produce warnings with time-to-contact and a false-alarm band; false alarms have a cost.
- **BD-61B** watch/muster: readiness score drives response; overwork and low fitness reduce it (duty fitness owner).
- **BD-61C** raid resolution: attackers use doctrine/faction state; defense uses readiness + fortification + air support; outcome typed (repelled/losses/breach/capture).
- **BD-61D** sky defense: intercept windows, ordnance consumption, debris/damage risk.
- **BD-61E** aftermath: casualties, damage, loot, memorial/journal; recovery tasks.
- **BD-61F** content volumes: +8 raid profiles, +10 defense upgrades, +8 warning events; fictional.

## Acceptance & verification
- A seeded raid is winnable with preparation and costly without; no random wipe; determinism.
- `godot --headless --path . -- --defense-selftest`; `bash scripts/run_test.sh Ashfall.Core.Tests/Defense/`; shelter security suites.

## Risks
Defense fatigue → raids are episodic, forecast by warning signs; difficulty presets scale intensity.

---

## 6. Expanded census (6 files · 2,337 lines)

Scope: `Assets/Ashfall.Core/Defense/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Support 1 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `DefenseSystem.cs` | 571 | System | — | 0 | 0 | 2 |
| `PerimeterDefenseCatalog.cs` | 152 | Catalog | — | 0 | 0 | 0 |
| `PerimeterDefenseSystem.cs` | 796 | System | — | 0 | 0 | 2 |
| `PerimeterEarlyWarningEngine.cs` | 280 | System | **yes** | 0 | 0 | 4 |
| `NightWatchPatrolReadinessEngine.cs` | 279 | System | **yes** | 0 | 0 | 0 |
| `PatrolTerritoryAuthority.cs` | 259 | Support | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 4 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `defenses.json` | object[2 keys] |
| `sky_defense_ordnance.json` | object[2 keys] |
| `perimeter_defenses.json` | object[2 keys] |
| `patrol_debriefs.json` | array[36] |
| `wick_braiding_priming_reports.json` | array[7] |

**State surfaces:** `DefenseSystem.cs`, `PerimeterDefenseSystem.cs`, `PerimeterEarlyWarningEngine.cs`, `PatrolTerritoryAuthority.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Defense/` |
| Test references | 25 name references across the test tree |
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

Domain files: 6. Other plans referencing their names: **7**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-PERIMETER-DEFENSE-TRUTH-165` | 3 |
| `PLAN-DEFENSE-COMMAND-TRUTH-207` | 3 |
| `PLAN-ORPHAN-SEAL-01` | 2 |
| `EVIDENCE` | 2 |
| `PLAN-SPATIAL-SIM-AUTHORITY-95` | 2 |
| `PLAN-NOISE-DISCIPLINE-TRUTH-116` | 2 |
| `PLAN-WARLORDS-DIPLOMACY-29` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `BD-61A` | `PerimeterEarlyWarningEngine.cs`, `NightWatchPatrolReadinessEngine.cs` |
| `BD-61B` | `NightWatchPatrolReadinessEngine.cs` |
| `BD-61C` | `DefenseSystem.cs`, `PerimeterDefenseCatalog.cs`, `PerimeterDefenseSystem.cs` |
| `BD-61D` | `DefenseSystem.cs`, `PerimeterDefenseCatalog.cs`, `PerimeterDefenseSystem.cs` |
| `BD-61E` | no name match — resolve at claim time |
| `BD-61F` | `DefenseSystem.cs`, `PerimeterDefenseCatalog.cs`, `PerimeterDefenseSystem.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 6; intra-domain edges: **2**; isolated files:
**2**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `DefenseSystem` | `PerimeterDefenseSystem` |
| `NightWatchPatrolReadinessEngine` | `PatrolTerritoryAuthority` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `PatrolTerritoryAuthority` | 1 |
| `PerimeterDefenseSystem` | 1 |
| `DefenseSystem` | 0 |
| `NightWatchPatrolReadinessEngine` | 0 |
| `PerimeterDefenseCatalog` | 0 |
| `PerimeterEarlyWarningEngine` | 0 |

**Class split:** hub 0 · sink 2 · source 2 · isolated 2.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 6. Host files: **5** · Test files: **13** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Host/DefenseHostSession.cs`, `src/Host/HostCli.Plans162_165.cs`, `src/Main.AdvancedShelterSystems.cs`, `src/Main.Companion.cs`, `src/Main.Plans162_165.cs` |
| Tests (`Ashfall.Core.Tests/`) | 13 | `Ashfall.Core.Tests/Defense/PerimeterDefensePhase7Tests.cs`, `Ashfall.Core.Tests/Defense/PerimeterDefensePlan203Tests.cs`, `Ashfall.Core.Tests/Defense/PerimeterDefenseTests.cs`, `Ashfall.Core.Tests/Defense/PerimeterEarlyWarningEngineTests.cs`, `Ashfall.Core.Tests/DefensePersistenceTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `perimeter_defense` |
| `sky_defense_battery` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **3** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--defense-selftest` |
| `--patrol-encounter-selftest` |
| `--sky-defense-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnCampNightSegmentResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnHazardWarning` | `Assets/Ashfall.Core/VentilationSystem.cs` |
| `OnImpactWarning` | `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` |
| `OnSafetyWarning` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnTerritoryChanged` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **6**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/faction_territory.json` |
| `Assets/StreamingAssets/Data/narrative/night_watch_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/night_watch_logbook.json` |
| `Assets/StreamingAssets/Data/narrative/patrol_debriefs.json` |
| `Assets/StreamingAssets/Data/perimeter_defenses.json` |
| `Assets/StreamingAssets/Data/sky_defense_ordnance.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (4 files, 36 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Defense` | 4 | 36 |

**Verdict:** 36 cases sit under matching regions — run those first (`Defense`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **11**
(4 of them panels/HUD).

| Host file |
|---|
| `src/Host/DefenseHostSession.cs` |
| `src/Host/DefenseSaveStore.cs` |
| `src/Host/HostCli.SkyDefense.cs` |
| `src/Host/HostSessionBase.cs` |
| `src/Host/PerimeterDefenseSaveStore.cs` |
| `src/Host/SkyDefenseBatterySaveStore.cs` |
| `src/Main.SkyDefense.cs` |
| `src/UI/AnomalyWatchPanel.cs` |
| `src/UI/ChemWarfareDefensePanel.cs` |
| `src/UI/DefenseGridPanel.cs` |
| `src/UI/SkyDefenseBatteryPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `perimeter_defense` | no |
| `sky_defense_battery` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `defense_capture` |
| `defense_damage` |
| `defense_targeting` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **6**
(CODEX_ONLY 3, GAMEPLAY_CONSUMED 1, UNRESOLVED 2).

| Catalog | Classification |
|---|---|
| `faction_territory.json` | UNRESOLVED |
| `narrative/night_watch_expansion.json` | CODEX_ONLY |
| `narrative/night_watch_logbook.json` | CODEX_ONLY |
| `narrative/patrol_debriefs.json` | CODEX_ONLY |
| `perimeter_defenses.json` | GAMEPLAY_CONSUMED |
| `sky_defense_ordnance.json` | UNRESOLVED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 7
**Surface:** save sections 2 (laddered 0) · RNG streams 3 · host files 14 · catalogs 12 · test regions 1 · flags 3

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-BASE-DEFENSE-RAIDS-61
wave: 6
status: PROPOSED — foreman claim required
packages: BD-61A, BD-61B, BD-61C, BD-61D, BD-61E, BD-61F
claim paths:
  - src/Host/DefenseHostSession.cs  # §19 candidate host surface
  - src/Host/DefenseSaveStore.cs  # §19 candidate host surface
  - src/Host/HostCli.SkyDefense.cs  # §19 candidate host surface
  - src/Host/HostSessionBase.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/faction_territory.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/night_watch_expansion.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Defense/
  - godot --headless --path . -- --defense-selftest
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
