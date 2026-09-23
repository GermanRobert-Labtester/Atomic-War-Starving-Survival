# ASHFALL — Expansion Wave 2 Index (Expansions 17–21)

**Status:** Design plans (pre-integration). Not claims, not authorizations.
**Date:** 2026-09-20
**Purpose:** Index and evidence summary for the five Wave 2 expansion bibles.

These documents investigate the live JSON data authority and Core systems, find
domains where content is thin relative to a working system, and propose expansions
that attach to existing owners. They are design bibles: they do not claim paths,
change code, or authorize implementation. Any implementation must later pass through
`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

---

## Why these five

Wave 1 (Expansions 12–16) covered generational survival, belief, aviation/orbital,
agriculture/soil, and bionics/robotics. Wave 2 picks five further domains with live
systems and thin authored content, chosen to avoid overlap with Wave 1, with existing
expansion numbers 01–11, and with the XP-04/EN-03 economy legs.

| Expansion | System (live) | Current content | Gap |
|---|---|---|---|
| 17 · The Long Evening | `SurvivorDowntimeSystem`, `VinylMoraleSystem`, `CultureCreationSystem`, `CulturalArchiveVaultSystem` | 6 hobbies, cassette corpus, vinyl records | No performance, festival, sport, game, gallery, broadcast culture |
| 18 · The Underneath | `SubterraneanSystem`, `TunnelNetworkSystem`, `ExcavationSystem`, `SeismicDynamicsSystem`, `HydroGeology` | small zone/strata/fault tables | No ore/mining, subsidence, aquifers, gas fields, cave ecology, deep heritage |
| 19 · The Bitter Air | `ChemWarfareSystem`, `DecontaminationSystem`, `PathogenStrainSystem`, `DiseaseQuarantineCoordinator` | a few agents, pathogens, protocols | No plumes, air quality, mask logistics, quarantine zones, toxic legacy, vault |
| 20 · The Quiet Hand | `EspionageSystem`, `CounterIntelligenceSystem`, `ShelterEspionageSystem`, prisoner/bounty systems | a few missions, profiles, tactics | No informants, dead-drop tradecraft, interrogation ethics, prisoner terms, defectors, hunters |
| 21 · The Grid | `PowerGridSystem`, `PowerDistributionSubgridSystem`, SOFC/solar/turbine/flywheel | small room/node/source tables | No load politics, cascades, fuel chains, storage doctrine, EMP hardening, microgrid |

Every plan opens with a source-verified authority split. Examples: `PowerGridSystem`
declares itself the single power authority and explicitly does not persist generation
contributions; `SubterraneanSystem` owns node state and forbidden concerns; the
strain layer owns only what the canonical disease engine lacks; `ZealotrySystem`'s
symmetry contract (Wave 1) has a Wave 2 analogue in the Quiet Hand's no-torture rule.

---

## The five plans

1. `expansion_17_the_long_evening_plan.md` — 70,611 chars.
   Leisure, music, performance, art, sport, games, festivals, broadcast culture.
   Extends `SurvivorDowntimeSystem`, `VinylMoraleSystem`, `CultureCreationSystem`.
2. `expansion_18_the_underneath_plan.md` — 70,270 chars.
   Subterranean networks, mining, deep geology, subsidence, aquifers, gas, cave
   ecology, deep heritage. Extends `SubterraneanSystem`, `ExcavationSystem`, etc.
3. `expansion_19_the_bitter_air_plan.md` — 70,209 chars.
   Chemical/biological hazards, plumes, air quality, masks, quarantine, toxic
   legacy. Extends the live agent, decon, strain, and quarantine owners.
4. `expansion_20_the_quiet_hand_plan.md` — 72,239 chars.
   Espionage, counterintelligence, informants, dead drops, interrogation ethics,
   prisoners, defectors, bounty hunters. Extends the live faction owners.
5. `expansion_21_the_grid_plan.md` — 70,629 chars.
   Load politics, cascades, fuel chains, storage doctrine, EMP hardening, microgrids.
   Extends `PowerGridSystem` and the distribution/generation/storage systems.

---

## Shared design constraints (all five)

- **Godot authoritative; Core engine-free.** No Godot or Unity reference in Core logic.
- **JSON data authoritative.** New content lives in snake_case catalogs with integer
  `schema_version`, validated by `CatalogIntegrityValidator` and registered with
  `ContentUtilizationScanner`.
- **One authority per concern.** Every plan contains a non-duplication statement and
  an integration-seam table. No second needs, inventory, power, disease, hazard,
  espionage, prisoner, growth, or save model is introduced.
- **Deterministic.** All rolls use the host-forked `ISeededRng` (`CampaignRngStream`);
  never `System.Random`, wall-clock time, or hash iteration order. Paired-replay
  hashes must match across continuous and interrupted runs. Generation contributions
  stay runtime-only per the live power contract.
- **Persistence.** State is captured in existing save envelopes
  (`RecreationSaveStore`, `SubterraneanHostSession`, `ChemWarfareSaveStore`,
  `EspionageSaveStore`, `PowerGridSave`, etc.) as additive sub-objects. Legacy saves
  load neutral; the Triad drift gate must pass.
- **Tone.** Restrained, human, fictional. No real-world CBRN content, no torture
  reward, no spy-thriller fantasy, no physics-simulator claims, no energy utopia.
- **Verification.** Focused tests per `TEST_POLICY.md`, plus
  `--data-integrity-selftest` and `--content-utilization-selftest`. A compile-green
  result is not acceptance.

---

## Ethical and content contracts specific to Wave 2

| Plan | Hard contract |
|---|---|
| 17 Long Evening | No real-world song, play, sport, or holiday; no new currency; gambling harm routes to existing systems |
| 18 Underneath | Permanent node consequences; no monsters or magic materials; no infinite ore; heritage grants no combat power |
| 19 Bitter Air | All agents/strains fictional and abstract; no torture; no weapon-use reward; decon and effluent route through live owners |
| 20 Quiet Hand | No torture mechanic; coercion never yields reliable intelligence; prisoners retain terms and dignity; false accusations have clearing paths |
| 21 Grid | No blackout may kill directly; priority ladder is explicit; no new currency; generation contributions never persisted |

---

## Cross-expansion hooks within Wave 2

```
   17 Long Evening ──┬── 18 Underneath (deep acoustics, cave songs)
                     ├── 19 Bitter Air (quarantine songs, siren festivals)
                     ├── 20 Quiet Hand (rumor as intelligence)
                     └── 21 Grid (the evening's power cost, dark evenings)

   18 Underneath ────┬── 19 Bitter Air (sealed strata, gas legacy)
                     ├── 20 Quiet Hand (hidden bunkers, secret archives)
                     └── 21 Grid (geothermal tap, pump and fan load)

   19 Bitter Air ────┬── 20 Quiet Hand (stealing a strain, vault access)
                     └── 21 Grid (filtration load, decon demand)

   20 Quiet Hand ────── 21 Grid (grid sabotage, intertie intelligence)
```

Wave 1 × Wave 2 hooks are listed in each plan's cross-expansion section. Each plan is
self-contained; none requires another to ship.

---

## How to promote a Wave 2 plan

1. Pick exactly one plan and one phase. Do not start two.
2. Re-audit the premise against current source and data; a plan is not proof an API
   or catalog still exists (`AGENTS.md` Rule 7).
3. Claim exact paths in `WORKTREE_OWNERSHIP.md`; confirm no overlap.
4. Add the package row to `INTEGRATION_PLANS.md` with owner, acceptance, and focused
   verification.
5. Implement data first, then pure Core, then persistence, then host/UI, then content.
6. Verify with focused tests and the data/content selftests; record limitations.
7. Update the live ledger only if you are the foreman or named integrator.

Until a foreman signature exists, these plans remain proposals. The safe pre-signature
work is Phase 1 (schemas and validators), which is additive and reversible.

---

## Open decisions common to the wave

- Content volume budgets (each plan lists an authoring estimate; total Wave 2 is
  roughly 300,000–370,000 words of new prose if all five are fully authored).
- Save-section placement (additive sub-objects vs. sibling sections) for each domain.
- Whether any Wave 2 expansion introduces a new headless selftest verb or extends an
  existing one. Each plan recommends extending existing verbs.
- Priority order. Recommended: 21 (grid) and 19 (bitter air) first, because both
  touch survival-critical systems; 18 (underneath) second, because it is the largest
  content build; 17 (long evening) and 20 (quiet hand) last, because both are social
  and can follow once the mechanical spine is proven.

---

## Evidence anchors (file references used across the plans)

- `Assets/StreamingAssets/Data/` — 622 JSON files, canonical content.
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` — data integrity gate.
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` — dead-data gate.
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` — save section registry.
- `docs/ASHFALL_EXPANSION_CONTEXT_ATLAS.md` — low-connectivity island analysis.
- `docs/expansions/EXPANSIONS_MASTER_CATALOG.md` — expansions 01–11 catalog.
- `docs/expansions/wave1/WAVE1_INDEX.md` — Wave 1 index.
- `docs/CURRENT_AUTHORITY.md` — authority map.
- `AGENTS.md`, `TEST_POLICY.md`, `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`.