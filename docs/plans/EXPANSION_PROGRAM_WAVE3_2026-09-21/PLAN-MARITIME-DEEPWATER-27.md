# PLAN-MARITIME-DEEPWATER-27 — Tides, Dives, Flotilla & the Frozen Road

**Wave:** 3 (2026-09-21) · **Kind:** EXPANSION
**Status:** PROPOSED — not a claim. Foreman claim required.
**Depends on:** PLAN-ORPHAN-SEAL-01 Wave 8, PLAN-TRANSPORT-EXPEDITION-30,
PLAN-DETERMINISM-REPLAY-13.
**Expanded appendix:** [`PLAN-MARITIME-DEEPWATER-27_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-MARITIME-DEEPWATER-27_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's maritime & deep water
systems (1 authorities), each mapped to its parent-plan mechanic row.

**Non-goals:** no new map authority, no second expedition system, no real
shipping/nautical data.

---

## 1. Outcome

Make water a first-class frontier. The corpus already has the machinery:
`MaritimeDiveSystem`, `DiveSiteCatalog`, `StealthDiveInstance`,
`DiveInstanceRunner`, `ProceduralScavengeSystem`, `SafeCrackingSystem`,
`TideCalendar`, `VariableLootNode`, `PsychologicalContaminationSystem`,
`BlackFlotillaStanding`, `MaritimeExplorationSystem`, plus
`ExpeditionNavalSystem`, `IceRoadSystem` and `District8DeepCoastSystem` on the
expedition side, and `dive_sites.json` / `maritime_zones.json` /
`naval_vessels.json` / `deep_lore_locations.json` / `black_flotilla_items.json`
as data.

Player loop: **read the tide → plan a dive or ice crossing → manage air,
cold, contamination and detection → salvage or trade → return changed**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Tide windows | `TideCalendar`, `weather_seasons.json` | schedule dive/ice route | safe window vs. cut-off, current risk |
| Dives | `MaritimeDiveSystem`, `DiveSiteCatalog`, `StealthDiveInstance` | assign divers, air, gear | depth/time budget, findings |
| Salvage | `ProceduralScavengeSystem`, `VariableLootNode` | search, cut, crack | loot tables, hazards |
| Strongboxes | `SafeCrackingSystem` | crack a safe | timed minigame outcome, noise |
| Contamination | `PsychologicalContaminationSystem` | post-dive care | trauma/contamination states |
| Flotilla | `BlackFlotillaStanding`, `naval_vessels.json` | trade, smuggle, refuse | standing, access, prices |
| Ice road | `IceRoadSystem`, `weather_seasons.json` | cross frozen water | load limits, ice failure risk |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Core files | `Maritime/` (15 files), `Expeditions/ExpeditionNavalSystem.cs`, `IceRoadSystem.cs`, `District8DeepCoastSystem.cs` |
| Data | `dive_sites.json`, `maritime_zones.json`, `naval_vessels.json`, `deep_lore_locations.json`, `black_flotilla_items.json`, `weather_seasons.json` |
| Prior work | Plan 116 (25 deep-lore sites), Plan 115 (crossing encounters), `DeepCoastHeadlessDemo`, `--deep-coast-selftest` |
| Host status | `MaritimeDiveSystem` (6 tests) and `MaritimeExplorationSystem` host-unreachable |
| Related | `TideCalendar`/`VariableLootNode` are test-covered, unreachable |

---

## 3. Packages

### MW-27A — Tide and dive planning
- Bind `TideCalendar` to the weather day owner; expose tide windows in the map/
  expedition surface. `MaritimeDiveSystem` gets a host session; air supply,
  depth, and current are derived from authored `dive_sites.json` rows.
- **Acceptance:** the same site is reachable only in its window; aborted dives
  return divers and gear intact; no shadow inventory.
- **Verify:** focused maritime tests + `--deep-coast-route-selftest`.

### MW-27B — Salvage and strongboxes
- `ProceduralScavengeSystem` + `VariableLootNode` resolve loot through the
  existing expedition loot validator (no parallel loot tables);
  `SafeCrackingSystem` exposes a timed action whose outcome costs noise/time.
- **Acceptance:** every loot item resolves to `items.json`; determinism across
  replay; cracking failure has a cost, not a soft-lock.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Maritime/`.

### MW-27C — Flotilla standing and naval trade
- `BlackFlotillaStanding` binds to faction standing owners (no second standing
  store); naval trade uses `naval_vessels.json` routes and the canonical
  economy; smuggling risk routes to contraband/heat (PLAN-VERTICAL-BODY-INDUSTRY-05).
- **Acceptance:** standing changes are explainable; trade uses canonical
  prices; no new currency.
- **Verify:** faction + economy focused suites.

### MW-27D — Ice road and winter crossings
- `IceRoadSystem` consumes real season/weather state; load limits per vehicle;
  ice failure events produce a rescue/recovery decision rather than a silent
  vehicle loss.
- **Acceptance:** crossing risk is forecast-visible; failure is recoverable;
  vehicles obey the canonical garage/inventory.
- **Verify:** `--ice-road-route-selftest` (existing) + focused tests.

### MW-27E — Contamination and post-dive care
- `PsychologicalContaminationSystem` produces typed states consumed by mental
  health; medical care routes through `MedicalWardSystem`; no duplicate trauma
  ledger.
- **Acceptance:** states are recoverable with rest/care; journal entries
  deduplicated by severity; determinism.
- **Verify:** mental-health + medical focused suites.

### MW-27F — Content volumes
- +10 dive sites, +6 vessels, +5 zones, +12 salvage rows, +8 flotilla
  encounters; fictional only; every row consumer-reachable.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Dive minigames become the game | bounded to one action + outcome; expedition loop stays primary |
| Flotilla overlaps faction authority | standing/route via existing owners only |
| Ice failure feels unfair | forecast + abort option + rescue path |
| Loot inflation | authored tables + existing rarity bands |

## 5. Verification

```bash
godot --headless --path . -- --deep-coast-selftest
godot --headless --path . -- --deep-coast-route-selftest
godot --headless --path . -- --ice-road-route-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/Maritime/
bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/
```

---

## 6. Expanded census (4 files · 1,664 lines)

Scope: `Assets/Ashfall.Core/Maritime/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Support 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `DiveSiteCatalog.cs` | 136 | Catalog | — | 0 | 0 | 0 |
| `MaritimeDiveSystem.cs` | 765 | System | — | 0 | 0 | 3 |
| `MaritimeExplorationSystem.cs` | 743 | System | **yes** | 0 | 0 | 2 |
| `StealthDiveInstance.cs` | 20 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `dive_sites.json` | object[2 keys] |
| `naval_vessels.json` | object[2 keys] |
| `maritime_zones.json` | object[2 keys] |
| `pneumatic_tube_diverter_audits.json` | array[8] |

**State surfaces:** `MaritimeDiveSystem.cs`, `MaritimeExplorationSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Maritime/` |
| Test references | 16 name references across the test tree |
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

Domain files: 4. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 6; intra-domain edges: **3**; isolated files:
**1**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `District8DeepCoastSystem` | `IceRoadSystem` |
| `DiveSiteCatalog` | `StealthDiveInstance` |
| `StealthDiveInstance` | `MaritimeDiveSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `IceRoadSystem` | 1 |
| `MaritimeDiveSystem` | 1 |
| `StealthDiveInstance` | 1 |
| `District8DeepCoastSystem` | 0 |
| `DiveSiteCatalog` | 0 |
| `MaritimeExplorationSystem` | 0 |

**Class split:** hub 1 · sink 2 · source 2 · isolated 1.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 6. Host files: **8** · Test files: **15** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 8 | `src/Host/CoreDemoSession.cs`, `src/Host/DeepCoastHostSession.cs`, `src/Host/DutyRosterHostSession.cs`, `src/Host/HostCli.SelfTests.cs`, `src/Host/MaritimeHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 15 | `Ashfall.Core.Tests/BlackFlotillaTests.cs`, `Ashfall.Core.Tests/DeepCoastCommandTests.cs`, `Ashfall.Core.Tests/District8DeepCoastTests.cs`, `Ashfall.Core.Tests/DutyRosterIntegrationTests.cs`, `Ashfall.Core.Tests/HoldfastSaveTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `deep_well` |
| `expedition_stealth` |
| `maritime` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--deep-coast-host-selftest` |
| `--deep-coast-playthrough` |
| `--deep-coast-route-selftest` |
| `--deep-coast-selftest` |
| `--ice-road-selftest` |
| `--ice-road-tick-demo` |
| `--maritime-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnIceRoadClosed` | `Assets/Ashfall.Core/IceRoadSystem.cs` |
| `OnIceRoadOpened` | `Assets/Ashfall.Core/IceRoadSystem.cs` |
| `OnSiteEncounterResolved` | `Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs` |
| `OnSiteEncounterStarted` | `Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **6**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/dive_sites.json` |
| `Assets/StreamingAssets/Data/gpr_exploration_catalog.json` |
| `Assets/StreamingAssets/Data/maritime_zones.json` |
| `Assets/StreamingAssets/Data/narrative/deep_lore_texts.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (3 files, 20 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Exploration` | 2 | 14 |
| `Maritime` | 1 | 6 |

**Verdict:** 20 cases sit under matching regions — run those first (`Exploration`, `Maritime`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **13**
(4 of them panels/HUD).

| Host file |
|---|
| `src/Host/DeepCoastHostSession.cs` |
| `src/Host/DeepWellHostSession.cs` |
| `src/Host/DeepWellSaveStore.cs` |
| `src/Host/HostCli.WorldExploration.cs` |
| `src/Host/MaritimeHostSession.cs` |
| `src/Host/MaritimeSaveStore.cs` |
| `src/Host/StealthSaveStore.cs` |
| `src/Main.DeepWell.cs` |
| `src/Main.Maritime.cs` |
| `src/UI/DeepCoastPanel.cs` |
| `src/UI/MaritimeAtlasPanel.cs` |
| `src/UI/MaritimePanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `deep_well` | no |
| `expedition_stealth` | no |
| `maritime` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `deep_coast` |
| `maritime` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **4**
(CODEX_ONLY 1, GAMEPLAY_CONSUMED 2, OPTIONAL 1).

| Catalog | Classification |
|---|---|
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `dive_sites.json` | GAMEPLAY_CONSUMED |
| `narrative/deep_lore_texts.json` | CODEX_ONLY |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_branch_mercy_road_locked` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 3 (laddered 0) · RNG streams 2 · host files 15 · catalogs 10 · test regions 2 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MARITIME-DEEPWATER-27
wave: —
status: PROPOSED — foreman claim required
packages: MW-27A, MW-27B, MW-27C, MW-27D, MW-27E, MW-27F
claim paths:
  - src/Host/DeepCoastHostSession.cs  # §19 candidate host surface
  - src/Host/DeepWellHostSession.cs  # §19 candidate host surface
  - src/Host/DeepWellSaveStore.cs  # §19 candidate host surface
  - src/Host/HostCli.WorldExploration.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/deep_lore_locations.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/deep_lore_survivor_fields.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Exploration/
  - godot --headless --path . -- --deep-coast-host-selftest
dependencies:
  - coordinate: 3 other plan(s) name these artifacts (§12)
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
