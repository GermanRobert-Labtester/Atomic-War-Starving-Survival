# ASHFALL — Expansion Wave 1 Index (Expansions 12–16)

**Status:** Design plans (pre-integration). Not claims, not authorizations.
**Date:** 2026-09-20
**Purpose:** Index and evidence summary for the five Wave 1 expansion bibles.

These documents investigate the live JSON data authority and Core systems, find
domains where content is thin relative to a working system, and propose expansions
that attach to existing owners. They are design bibles: they do not claim paths,
change code, or authorize implementation. Any implementation must later pass through
`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

---

## Why these five

A census of `Assets/StreamingAssets/Data/` (622 JSON files, ~13 MB) and a read of the
live Core systems found five domains with strong systems but small authored content:

| Expansion | System (live) | Current content | Gap |
|---|---|---|---|
| 12 · The Second Generation | `CohortSystem`, `ChildDevelopmentSystem`, `GenerationalSystem`, `ApprenticeshipSystem` | 7 development traits, 4 tuning fields | No curriculum, milestones, kinship, rites, succession content |
| 13 · The Faithful & The Fractured | `SpiritualMeaningCoordinator`, `ZealotrySystem`, `IdeologicalFrictionSystem` | 3 movements, 3 religion profiles, 9 rituals, 5 ceremonies | No pilgrimage, relics, shrines, schism, holy calendar |
| 14 · Above the Ash | `AviationSystem`, `SkyDefenseBatterySystem`, `SkyLayerArmorSystem`, `OrbitalHarrowTelemetrySystem` | 3 aircraft, 6 ordnance, 6 armor configs, 12 orbital events | No sky trade, airdrop contest, survey, seasons, warning network |
| 15 · The Deep Root | `AgricultureSystem`, `GreenhouseSystem`, `ApicultureSystem`, `CompanionAnimalSystem` | 2 agriculture items, 12 strains, 5 companion species | No open ground, orchard, livestock, vet, aquaculture, seed bank |
| 16 · The Rebuilt Body | `AmputationSystem`, `BionicsSystem`, `RoboticsSystem` | 5 implants, 5 robots | No simple prosthetics, rehab content, complications, automation, drones, rogue arcs |

Each expansion is grounded in source-verified authority split comments. For example,
`ZealotrySystem` mandates mechanically symmetrical, fictional belief; `BionicsSystem`
forbids a second body model and blanket electrical damage; `AgricultureSystem` delegates
growth to `GreenhouseSystem`; `SpiritualMeaningCoordinator` forbids a piety meter.

---

## The five plans

1. `expansion_12_the_second_generation_plan.md` — 63,753 chars.
   Generational survival: curriculum, milestones, kinship, succession dossiers,
   coming-of-age rites. Extends `CohortSystem` / `GenerationalSystem`.
2. `expansion_13_the_faithful_and_the_fractured_plan.md` — 53,593 chars.
   Belief, ritual, pilgrimage, relics, shrines, schism. Extends
   `SpiritualMeaningCoordinator` / `ZealotrySystem`.
3. `expansion_14_above_the_ash_plan.md` — 51,356 chars.
   Aviation, sky trade, airdrops, sky-layer armor, orbital harrow seasons.
   Extends `AviationSystem` / `SkyDefenseBatterySystem` / `SkyLayerArmorSystem`.
4. `expansion_15_the_deep_root_plan.md` — 50,111 chars.
   Soil reclamation, orchards, livestock, veterinary care, aquaculture, seed bank.
   Extends `AgricultureSystem` / `GreenhouseSystem` / `CompanionAnimalSystem`.
5. `expansion_16_the_rebuilt_body_plan.md` — 50,164 chars.
   Simple prosthetics, rehabilitation, complications, automation, drones, rogue logic.
   Extends `AmputationSystem` / `BionicsSystem` / `RoboticsSystem`.

---

## Shared design constraints (all five)

- **Godot authoritative; Core engine-free.** No Godot/Unity reference in Core logic.
- **JSON data authoritative.** New content lives in snake_case catalogs with integer
  `schema_version`, validated by `CatalogIntegrityValidator` and registered with
  `ContentUtilizationScanner`.
- **One authority per concern.** Every plan contains an explicit non-duplication
  statement and an integration-seam table. No second needs, inventory, growth,
  limb, robot, belief, or power model is introduced.
- **Deterministic.** All rolls use the host-forked `ISeededRng` (`CampaignRngStream`);
  never `System.Random`, wall-clock time, or hash iteration order. Paired-replay
  hashes must match across continuous and interrupted runs.
- **Persistence.** State is captured in existing save envelopes
  (`GenerationalSaveStore`, `SpiritualSaveStore`, `AviationSaveStore`,
  `AgricultureSaveStore`, `BionicsSaveStore`, `RoboticsSaveStore`, etc.) as additive
  sub-objects. Legacy saves load neutral; the Triad drift gate must pass.
- **Tone.** Restrained, human, fictional. No real countries, wars, people, faiths,
  aircraft, ordnance, implants, or AI systems. No glorified child combat, belief
  violence, air war, or transhumanist ascension.
- **Verification.** Focused tests per `TEST_POLICY.md`, plus `--data-integrity-selftest`
  and `--content-utilization-selftest`. A compile-green result is not acceptance.

---

## Cross-expansion hooks

The five plans interlock additively; each can ship alone.

```
      12 Second Generation ──┬── 13 Faithful (coming-of-age rites)
                            ├── 15 Deep Root (child growers, seed inheritance)
                            └── 16 Rebuilt Body (pediatric prosthetics)

      13 Faithful ───────────┬── 14 Above the Ash (Listener mast, sky signs)
                            └── 16 Rebuilt Body (Iron Silence, machine reverence)

      14 Above the Ash ──────┬── 15 Deep Root (aerial seeding, crop survey)
                            └── 16 Rebuilt Body (bionic pilot, drone relay)

      15 Deep Root ──────────┴── 16 Rebuilt Body (vet prosthetics, robotic herders)
```

---

## How to promote a Wave 1 plan

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

- Content volume budgets (each plan lists an authoring estimate; total Wave 1 is
  roughly 250,000–320,000 words of new prose if all five are fully authored).
- Save-section placement (additive sub-objects vs. sibling sections) for each domain.
- Whether any Wave 1 expansion introduces a new headless selftest verb or extends an
  existing one. Each plan recommends extending the existing verb.
- Priority order. Recommended: 12 (generational) and 13 (belief) first, because both
  are content-light and socially central; 15 (agriculture) second; 14 (sky) and 16
  (body) third, because both touch power and contest balance.

---

## Evidence anchors (file references used across the plans)

- `Assets/StreamingAssets/Data/` — 622 JSON files, canonical content.
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` — data integrity gate.
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` — dead-data gate.
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` — save section registry.
- `docs/ASHFALL_EXPANSION_CONTEXT_ATLAS.md` — low-connectivity island analysis.
- `docs/expansions/EXPANSIONS_MASTER_CATALOG.md` — expansions 01–11 catalog.
- `docs/CURRENT_AUTHORITY.md` — authority map.
- `AGENTS.md`, `TEST_POLICY.md`, `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`.