# Roadmap 42 — Batch 1: Scaffolding & Underused-System Catalogs (Plans 32–41)

> **Scope:** Ten focused execution plans that close the largest scaffolding gap in ASHFALL —
> eight Core systems that are fully implemented, save-supported, and tick-registered but have
> **zero data catalogs** (verified 2026-08-30), plus the single biggest content gap: only 2 of
> 115 locations are wired as expedition destinations.
>
> **Bias:** ~80% data-authority work, ~20% minimal loader wiring. Each plan that needs a new
> loader is flagged `NEW SYSTEM JUSTIFICATION REQUIRED` — the loader is mechanical
> deserialization only, no gameplay logic, no new systems.

---

## Evidence base (verified 2026-08-30)

| Catalog / system | Current state | Source |
|---|---|---|
| `expeditions.json` | **2** entries (of 115 locations) | `grep -c '"id": *"loc_' expeditions.json` |
| `skills.json` | **MISSING** (47 hardcoded in C#) | `SkillProgressionSystem.cs`, `SkillDef.cs` |
| `research_catalog.json` | **MISSING** (15 hardcoded in C#) | `ResearchSystem.cs` |
| `wildlife_migration.json` | **MISSING** (system live) | `WildlifeMigrationSystem.cs` |
| `wildlife_trapping_catalog.json` | **MISSING** (system live) | `WildlifeTrappingSystem.cs` |
| `excavation_sites.json` | **MISSING** (system live) | `ExcavationSystem.cs` |
| `sky_layer_armor_catalog.json` | **MISSING** (system live) | `SkyLayerArmorSystem.cs` |
| `orbital_harrow_events.json` | **MISSING** (system live) | `OrbitalHarrowTelemetrySystem.cs` |
| `ledger_debt_templates.json` | **MISSING** (system live) | `LedgerDebtSystem.cs` |
| `shelter_rooms.json` | **MISSING** (system live) | `ShelterAssignmentSystem.cs` |

All ten target systems confirmed present in `Assets/Ashfall.Core/` via `find`.

---

## Plan index

| # | File | Theme | System fed | Content added | Priority | Risk |
|---|---|---|---|---|---|---|
| 32 | `32-expedition-destination-wiring.md` | Expedition scaffolding | `ExpeditionSystem` | 48 wired destinations (2 → 50) | P1 | LOW |
| 33 | `33-skill-catalog-externalization.md` | Skill catalog | `SkillProgressionSystem` | 50 skills (47 → JSON + 3 new) | P1 | MEDIUM |
| 34 | `34-research-tree-externalization.md` | Research tree | `ResearchSystem` | 40 knowledge nodes (15 → JSON + 25) | P1 | MEDIUM |
| 35 | `35-wildlife-migration-catalog.md` | Wildlife migration | `WildlifeMigrationSystem` | 12 migration patterns | P2 | LOW |
| 36 | `36-wildlife-trapping-catalog.md` | Trapping | `WildlifeTrappingSystem` | 10 traps + 15 prey | P2 | LOW |
| 37 | `37-excavation-sites-catalog.md` | Underground exploration | `ExcavationSystem` | 8 deep-strata sites | P2 | MEDIUM |
| 38 | `38-sky-layer-armor-catalog.md` | Shelter defense | `SkyLayerArmorSystem` | 6 armor configs + 10 threats | P2 | MEDIUM |
| 39 | `39-orbital-harrow-telemetry-events.md` | Early warning | `OrbitalHarrowTelemetrySystem` | 12 events + 8 consequences | P2 | MEDIUM |
| 40 | `40-ledger-debt-templates.md` | Economic pressure | `LedgerDebtSystem` | 15 debt templates + 10 consequences | P2 | LOW |
| 41 | `41-shelter-room-catalog.md` | Shelter interior | `ShelterAssignmentSystem` | 20 rooms + 12 assignment rules | P2 | MEDIUM |

---

## Dependency graph

```
32 (expedition wiring) ──► 37 (excavation sites dispatch as subterranean expeditions)
                        ──► 46 (scavenging tables deepen loot categories) [batch 2]
                        ──► 43 (settlements mark friendly vs hostile destinations) [batch 2]

33 (skills) ──► 34 (research prerequisites reference skill_* ids)
            ──► 41 (assignment rules require skill_* ids)
            ──► 36 (trapping success modified by skill_trapping)

34 (research) ──► 41 (laboratory assignment requires knowledge_* node)
              ──► 04 (relic blueprints unlock via advanced research) [existing]
              ──► 22 (foundry/greenhouse tech unlocks) [existing]

35 (migration) ──► 36 (prey availability respects migration windows)
               ──► 28A (wildlife ecology) [existing]
               ──► 13B (hunting loop) [existing]

37 (excavation) ──► 04 (relics as dig loot) [existing]
                ──► 17B (documents as dig loot) [existing]
                ──► 09A (spore-mold disease as depth hazard) [existing]

38 (sky armor) ↔ 39 (telemetry events) — reconcile threat vs detection catalogs
38 ──► 19B (orbital strikes) [existing]
39 ──► 24A (radio schedule — dead-hand pings on shortwave) [existing]

40 (debt) ──► 14 (raids — bounties generate encounters) [existing]
         ──► 44 (faction territory — default shifts control) [batch 2]
         ──► 16C (treaties — debt disputes escalate) [existing]

41 (rooms) ──► 29A (room identity + history) [existing]
          ──► 12B (duty roster — room assignments produce output) [existing]
          ──► 12C (shelter decor) [existing]
```

---

## Execution sequence

### NOW (do first — unblock the most downstream content)
1. **Plan 32** — expedition destination wiring. The single largest gap; unblocks all
   surface exploration, encounter, and scavenging content. Pure data, LOW risk.
2. **Plan 33** — skill catalog externalization. Closes an invariant violation; unblocks
   Plan 34, 41, and skill-gated encounters. MEDIUM risk (loader + hardcoded removal).
3. **Plan 34** — research tree externalization. Same pattern as 33; unlocks the tech-tree
   progression spine. MEDIUM risk.

### NEXT (do after NOW — cross-system + moderate integration)
4. **Plan 35** — wildlife migration. Unlocks the seasonal food/hazard loop. LOW risk.
5. **Plan 36** — wildlife trapping. Depends on 35 (migration windows). LOW risk.
6. **Plan 37** — excavation sites. Depends on 32 (dispatch wiring). MEDIUM risk.
7. **Plan 41** — shelter rooms. Depends on 33/34 (assignment rules). MEDIUM risk.

### LATER (do last — require reconciliation or are self-contained)
8. **Plan 38 + 39** — sky armor + telemetry. Must be reconciled together (threat vs
   detection). MEDIUM risk.
9. **Plan 40** — ledger debt. Self-contained economic pressure. LOW risk.

---

## Cross-system chains activated by this batch

| Chain | Systems spanned | Plans |
|---|---|---|
| Expedition → excavation → relic → workshop → research | ExpeditionSystem → ExcavationSystem → WorkshopReverseEngineering → ResearchSystem | 32 → 37 → 04 → 34 |
| Skill → research → room assignment → duty roster output | SkillProgression → Research → ShelterAssignment → DutyRoster | 33 → 34 → 41 → 12B |
| Migration → trapping → food economy → disease | WildlifeMigration → WildlifeTrapping → EconomySystem → DiseaseSystem | 35 → 36 → 13B → 09A |
| Telemetry → radio → sky armor → shelter damage | OrbitalHarrow → RadioTuner → SkyLayerArmor → ShelterSystem | 39 → 24A → 38 → 29B |
| Debt → bounty → raid → faction reputation | LedgerDebt → RaidSystem → ReputationSystem → FactionSystem | 40 → 14 → 44 |

---

## Content totals added by this batch

* **+48** expedition destinations (2 → 50)
* **+50** skill definitions (47 hardcoded → JSON + 3 new)
* **+40** research nodes (15 hardcoded → JSON + 25 new)
* **+12** wildlife migration patterns
* **+10** trap types + **+15** prey entries
* **+8** deep-strata excavation sites
* **+6** sky-armor configs + **+10** orbital-threat events
* **+12** telemetry events + **+8** strike consequences
* **+15** debt templates + **+10** default consequences
* **+20** shelter rooms + **+12** assignment rules
* **8** JSON-authority invariant violations closed (skills, research, 6 missing catalogs)

---

## Verification (run after each plan, then after the full batch)

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --expedition-selftest   # for plans 32, 37
```

Any plan touching ≥2 coupled variables (e.g. Plan 33 loader + hardcoded removal) requires
**cross-tool QA** (implementer ≠ reviewer) per `AGENTS.md`.
