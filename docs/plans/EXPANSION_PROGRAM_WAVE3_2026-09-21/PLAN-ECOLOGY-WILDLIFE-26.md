# PLAN-ECOLOGY-WILDLIFE-26 — Living Wasteland: Ecosystems, Harvest Pressure & Bestiary

**Wave:** 3 (2026-09-21) · **Kind:** EXPANSION
**Status:** PROPOSED — not a claim. Foreman claim required.
**Depends on:** PLAN-ORPHAN-SEAL-01 Waves 6/8; PLAN-DETERMINISM-REPLAY-13.
**Expanded appendix:** [`PLAN-ECOLOGY-WILDLIFE-26_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-ECOLOGY-WILDLIFE-26_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's ecology & wildlife
systems (2 authorities), each mapped to its parent-plan mechanic row.

**Non-goals:** no real-world species data, no second loot system, no new map
graph (extend `WastelandMapSystem`).

---

## 1. Outcome

Turn the already-authored wildlife corpus into a **living ecosystem loop** the
player can deplete, manage, and study. Today: `WildlifeTrappingSystem` has a
catalog and a catch-transfer seam; `WildlifeEcosystemSystem`,
`WildlifeHarvestQuotaEngine`, `BestiarySystem`, `EcologicalInfestationSystem`,
and `CompanionAnimalSystem` exist but are host-unreachable.

Player loop: **observe → identify → trap/hunt → respect or exceed quotas →
pressure species → face migration, predators, vermin, and disease →
study and unlock better techniques → steward or farm**.

| Mechanic | Authority (existing) | Player action | Outcome |
|---|---|---|---|
| Regional populations | `WildlifeEcosystemSystem`, `WildlifeMigrationSystem` (+`.Live`) | set trapping zones / rest areas | species counts shift, sightings change |
| Harvest pressure | `WildlifeHarvestQuotaEngine` | harvest within/over quota | depletion, migration, vermin surge |
| Seasonal windows | `WildlifeSeasonalCalendar` | time hunts/traps | yields, bycatch, safety |
| Trapping mastery | `WildlifeTrappingCatalog`/`System` | place/repair traps, check daily | yields, bycatch, disease risk |
| Bestiary | `BestiarySystem`, `WastelandBestiaryCatalog`, `FieldGuideCatalog` | observe, record, study | knowledge unlocks, safer hunts |
| Companions & pests | `CompanionAnimalSystem`, pest/infestation system | adopt, feed, treat; cull vermin | morale/pack bonus, food loss, disease vectors |
| Infestation | `EcologicalInfestationSystem`, `EcologicalInfestationCatalog` | treat granary/mold/mice | spoilage, disease, morale |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Core systems | `World/WildlifeEcosystemSystem.cs`, `World/WildlifeHarvestQuotaEngine.cs`, `WildlifeMigrationSystem.cs(+.Live)`, `WildlifeSeasonalCalendar.cs`, `WildlifeTrappingCatalog/System/Events.cs`, `Ecology/EcologicalInfestation*`, `Ecology/CompanionAnimalSystem.cs`, `Bestiary/BestiarySystem.cs`, `Narrative/WastelandBestiaryCatalog.cs` |
| Data | `wildlife_ecosystem.json`, `wildlife_trapping_catalog.json`, `ecological_infestations.json`, `companion_animals.json` |
| Prior seals | Plan 36 catalog (10 traps / 15 prey), Plan 174 companion animals (persistent bond, food port), map-graph wildlife overlay |
| Host status | `BestiarySystem` and `WildlifeHarvestQuotaEngine` host-unreachable |
| Skills | `ashfall-expand` (canon-aware), `ashfall-balance-sim` (seeded sweeps) |

---

## 3. Packages

### WL-26A — Ecosystem host authority
- Bind `WildlifeEcosystemSystem` + migration + seasonal calendar to a
  `wildlife` day owner (after `weather`, before `hygiene`) with a new save
  section only if it cannot ride `world`/`expedition` (integrator decision).
- Region populations derived from `wildlife_ecosystem.json`; migration hops
  reuse the map graph overlay.
- **Acceptance:** same-seed 30-day run reproduces population curves; no
  duplicate species registry; sightings surface in the field guide surface.
- **Verify:** focused ecology suite + `--world-selftest`.

### WL-26B — Harvest pressure and quota loop
- `WildlifeHarvestQuotaEngine` consumes actual harvests from trapping/hunting;
  over-quota state cascades: depletion → migration → **vermin surge**
  (infestation spawn) → predator pressure (night-watch events).
- **Acceptance:** quota state is explainable in one panel row; depletion is
  reversible with rest quotas; consequences route to existing systems
  (infestation, needs, morale) with bounded magnitudes.
- **Verify:** balance sim (seeded) + focused tests.

### WL-26C — Bestiary and field-guide progression
- `BestiarySystem` tracks observation/specimen knowledge per species; unlocks
  improve trap selection, bycatch avoidance, and safe handling (disease risk
  reduction). `WastelandBestiaryCatalog` + `FieldGuideCatalog` supply entries.
- **Acceptance:** knowledge is progressive and persisted; entries render in the
  field guide; unlocks have measurable effect (documented before/after).
- **Verify:** `--journal-selftest` + focused bestiary tests.

### WL-26D — Trapping loop completion
- Place/check/repair traps through the host; `TransferCatchToInventory` is the
  only loot path; seasonal windows and bycatch disease risk apply; trap damage
  from weather (Plan 205/Plan 135 cascades).
- **Acceptance:** no shadow store; duplicate-check guard; weather interaction
  documented; focused trapping suite stays green.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingSystemTests.cs`.

### WL-26E — Companions, livestock and predators
- Companion bond/training already sealed (Plan 174); extend with predation risk
  on companions/livestock, veterinary care through the medical owner, and
  breeding with a capped population (no unbounded growth).
- **Acceptance:** predator attacks are deterministic and recoverable; breeding
  consumes feed through the canonical inventory; morale effects bounded.
- **Verify:** Plan 174 host wiring tests + focused ecology tests.

### WL-26F — Infestation and disease vectors
- `EcologicalInfestationSystem` operates on room-level pest/mold state; routes
  spoilage to `FoodPreservationSystem`, exposure to `DiseaseSystem.TryExpose`,
  and sanitation pressure to `SanitationSystem`.
- **Acceptance:** infestation is reversible; no parallel disease authority;
  sanitation modifier bounded per Plan 210 contract.
- **Verify:** `bash scripts/run_test.sh` sanitation/disease suites.

### WL-26G — Content volumes
- +8 species rows, +6 infestation types, +12 bestiary entries, +10 field-guide
  observations, +4 companion archetypes; all fictional, with a live consumer
  each; data-integrity + content-utilization green.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Ecosystem bookkeeping becomes invisible complexity | one panel row with population/trend/quota; journal only on thresholds |
| Overhunting is punishing with no recovery | rest quotas, immigration, seasonal reset; balance sim tuned per DEC-07 method |
| Companion breeding becomes an exploit | feed/capacity caps; population ceiling per region |
| Infestation spam | threshold + cooldown + coalescing; one journal key per severity |

## 5. Verification

```bash
godot --headless --path . -- --world-selftest
godot --headless --path . -- --data-integrity-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/World/
bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingSystemTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs
```

---

## 6. Expanded census (3 files · 1,205 lines)

Scope: `Assets/Ashfall.Core/Bestiary/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `BestiarySystem.cs` | 285 | System | **yes** | 0 | 0 | 2 |
| `WildlifeEcosystemSystem.cs` | 652 | System | — | 0 | 0 | 2 |
| `WildlifeHarvestQuotaEngine.cs` | 268 | System | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `wildlife_ecosystem.json` | object[4 keys] |
| `wildlife_trapping_catalog.json` | object[4 keys] |
| `wasteland_wildlife_bestiary.json` | object[3 keys] |
| `wildlife_field_encounter_logs.json` | object[4 keys] |

**State surfaces:** `BestiarySystem.cs`, `WildlifeEcosystemSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Bestiary/` |
| Test references | 4 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 2 |
| `EVIDENCE` | 2 |
| `PLAN-SPATIAL-SIM-AUTHORITY-95` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `WL-26A` | `WildlifeEcosystemSystem.cs` |
| `WL-26B` | `WildlifeHarvestQuotaEngine.cs` |
| `WL-26C` | `BestiarySystem.cs` |
| `WL-26D` | no name match — resolve at claim time |
| `WL-26E` | no name match — resolve at claim time |
| `WL-26F` | no name match — resolve at claim time |
| `WL-26G` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **7** · Test files: **7** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 7 | `src/Host/HostCli.Plans162_165.cs`, `src/Host/WildlifeEcosystemHostSession.cs`, `src/Main.CampaignOwners.cs`, `src/Main.EcologicalInfestations.cs`, `src/Main.EvolvingWorld.cs` |
| Tests (`Ashfall.Core.Tests/`) | 7 | `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs`, `Ashfall.Core.Tests/EcologyBalanceSimulationTests.cs`, `Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs`, `Ashfall.Core.Tests/WildlifeSeasonalCalendarTests.cs`, `Ashfall.Core.Tests/WildlifeTrapping/WildlifeTrappingSeasonMigrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **1**; isolated: **2**.

| From | → To |
|---|---|
| `WildlifeHarvestQuotaEngine` | `WildlifeEcosystemSystem` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `wildlife_ecosystem` |
| `wildlife_trapping` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--faction-ecology-selftest` |
| `--wildlife-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnFirstHarvest` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnHarvest` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnTreatyQuotaMet` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnTreatyQuotaMissed` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **5**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` |
| `Assets/StreamingAssets/Data/narrative/wildlife_field_encounter_logs.json` |
| `Assets/StreamingAssets/Data/seasonal_events.json` |
| `Assets/StreamingAssets/Data/wildlife_ecosystem.json` |
| `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (6 files, 76 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Bestiary` | 1 | 6 |
| `WildlifeTrapping` | 5 | 70 |

**Verdict:** 76 cases sit under matching regions — run those first (`Bestiary`, `WildlifeTrapping`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **6**
(2 of them panels/HUD).

| Host file |
|---|
| `src/Host/WildlifeEcosystemHostSession.cs` |
| `src/Host/WildlifeEcosystemSaveStore.cs` |
| `src/Host/WildlifeTrappingHostSession.cs` |
| `src/Host/WildlifeTrappingSaveStore.cs` |
| `src/UI/BestiaryPanel.cs` |
| `src/UI/WildlifeTrappingPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `wildlife_ecosystem` | no |
| `wildlife_trapping` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **4**.

| Stream |
|---|
| `wildlife_apex` |
| `wildlife_migration` |
| `wildlife_population` |
| `wildlife_taming` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **4**
(CODEX_ONLY 2, GAMEPLAY_CONSUMED 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `narrative/wasteland_wildlife_bestiary.json` | CODEX_ONLY |
| `narrative/wildlife_field_encounter_logs.json` | CODEX_ONLY |
| `seasonal_events.json` | UNRESOLVED |
| `wildlife_trapping_catalog.json` | GAMEPLAY_CONSUMED |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 2 (laddered 0) · RNG streams 4 · host files 10 · catalogs 9 · test regions 2 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ECOLOGY-WILDLIFE-26
wave: —
status: PROPOSED — foreman claim required
packages: WL-26A, WL-26B, WL-26C, WL-26D, WL-26E, WL-26F, WL-26G
claim paths:
  - src/Host/WildlifeEcosystemHostSession.cs  # §19 candidate host surface
  - src/Host/WildlifeEcosystemSaveStore.cs  # §19 candidate host surface
  - src/Host/WildlifeTrappingHostSession.cs  # §19 candidate host surface
  - src/Host/WildlifeTrappingSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/wildlife_field_encounter_logs.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Bestiary/
  - godot --headless --path . -- --faction-ecology-selftest
dependencies:
  - coordinate: 3 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | **no** |
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

**Pre-claim actions:** author or confirm: wave.
