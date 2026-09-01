# Plan 172 — Radiation Mutation System

## Goal

Create a radiation mutation system where long-term radiation exposure causes permanent genetic changes in survivors — some harmful (deformities, illness), some beneficial (resistance, enhanced abilities), creating a risk/reward dynamic around radiation zones. Currently `RadiationSystem` tracks radiation doses and `NeedsSystem` tracks health, but radiation only causes damage and death — no mutations, no permanent genetic changes, no adaptive evolution. This plan adds biological depth to radiation exposure and creates meaningful choices about radiation risk.

## Why

**Repository evidence:** Grep for `MutationSystem`, `MutantTrait`, `GeneticMutation` in Core returns only code-level "mutation" (state mutation, list mutation) — no biological mutation system. `CatalogIntegrityValidator.cs` recognizes `mutation_` as a known prefix but no mutation catalog exists. Plan 146 (radiation→economy/social bridge) mentions "Radiation mutation system (long-term exposure causes permanent changes)" as a follow-on opportunity but doesn't implement it. Radiation causes damage and death but no permanent genetic transformation.

**What is missing:** No mutation system. No permanent genetic changes from radiation. No beneficial mutations (radiation resistance, enhanced strength). No harmful mutations (deformities, reduced lifespan). No mutation progression (increasing exposure → increasing mutation). No mutation inheritance (mutations passed to children). Radiation is purely negative — no adaptive evolution, no risk/reward.

**Why existing plans don't solve it:** Plan 146 (radiation bridge) connects radiation to economy/social but doesn't add mutation mechanics. Plan 137 (needs→performance) connects needs to work but not radiation mutations. Plan 164 (nuclear winter) adds climate progression but not biological mutation. No plan addresses radiation-induced genetic mutation.

**Player value:** Creates risk/reward decisions (radiation zones have valuable resources but mutation risk), adds biological depth (survivors change genetically), generates emergent stories (mutation discoveries, mutation discrimination), and makes radiation more than just a damage-over-time mechanic.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` — radiation tracking
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` — health tracking
- `Assets/Ashfall.Core/Survivors/SurvivorLifecycle.cs` — survivor life/death
- `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` — disease pipeline
- `Assets/StreamingAssets/Data/items.json` — radiation items
- NEW: `Assets/Ashfall.Core/Radiation/MutationSystem.cs`
- NEW: `Assets/StreamingAssets/Data/mutation_traits.json`

## Main Task 1 — Foundation / System Contract

1. Create `MutationSystem.cs` in `Assets/Ashfall.Core/Radiation/`
2. Define `MutationTrait` DTO: `traitId`, `traitName`, `traitType` (beneficial/harmful/neutral/mixed), `radiationThreshold` (cumulative dose required), `probability` (0-1, chance of manifesting at threshold), `effects` (list of modifiers: health, strength, intelligence, appearance, lifespan), `description`, `flavorText`
3. Define `SurvivorMutation` DTO: `survivorId`, `traitId`, `manifestedDay`, `severity` (0-100), `progression` (stable/worsening/improving), `visible` bool (affects appearance), `inherited` bool (passed from parent)
4. Define `MutationState` DTO: list of survivor mutations, list of manifested traits, mutation pool (available traits), mutation seed for determinism
5. Implement `CaptureState/RestoreState` with schema versioning
6. Define mutation categories:
   - **Beneficial**: radiation resistance (+20%), enhanced strength (+10%), night vision, disease resistance, extended lifespan
   - **Harmful**: deformity (social penalty), reduced fertility, chronic pain, shortened lifespan, cognitive decline
   - **Neutral**: unusual appearance (no mechanical effect), changed voice, extra fingers
   - **Mixed**: enhanced strength but reduced charisma, radiation resistance but reduced fertility
7. Define mutation mechanics:
   - Cumulative radiation dose tracked per survivor
   - At dose thresholds, mutation check triggered
   - Check rolls against trait probability with `ISeededRng`
   - Successful roll: trait manifests
   - Multiple traits can manifest (increasing severity)
   - Mutations are permanent (cannot be cured, only managed)
   - Some mutations progress (worsen over time with continued exposure)
8. Define mutation inheritance:
   - Mutated survivors can pass mutations to children
   - Inheritance probability: 50% per trait
   - Children of mutated parents start with lower threshold
   - Inbreeding increases mutation risk
   - Inherited mutations present from birth
9. Define mutation visibility:
   - Some mutations are visible (affect appearance, social interactions)
   - Visible mutations cause social stigma (reduced trade, fear from others)
   - Hidden mutations only detectable through medical examination
   - Visibility affects survivor morale and relationships
10. Define mutation management:
    - Medical treatment can slow mutation progression
    - Chelation therapy reduces radiation dose (slows new mutations)
    - Gene therapy (research unlock) can stabilize mutations
    - Mutation cannot be reversed (permanent change)
    - Management is about slowing, not curing
11. Define mutation effects on gameplay:
    - Beneficial mutations: survival advantages (resistance, strength)
    - Harmful mutations: survival challenges (illness, social penalty)
    - Mixed mutations: trade-offs (power at a cost)
    - Mutations affect expedition capability
    - Mutations affect social interactions
    - Mutations affect reproduction
12. Add deterministic seeding: mutation rolls use `ISeededRng`
13. Wire into `GameBootstrap`: `SetupMutations`, `TickMutations`, `SaveMutations`
14. Create `MutationTraitCatalogLoader` for trait definitions
15. Implement mutation UI: mutation panel showing survivor mutations

## Main Task 2 — Implementation / Manifestation / Inheritance / Management / UI

1. Implement mutation threshold tracking:
   - Cumulative radiation dose tracked per survivor
   - Dose thresholds defined per trait (100/200/500/1000/2000 mSv)
   - At threshold, mutation check triggered
   - Check uses `ISeededRng` against trait probability
   - Multiple checks possible (different traits at different thresholds)
2. Implement trait manifestation:
   - Successful check: trait manifests
   - Trait effects applied to survivor stats
   - Trait recorded in survivor mutation list
   - Manifestation event logged
   - Visible traits change survivor appearance
3. Implement mutation progression:
   - Some traits worsen with continued exposure
   - Progression tracked (stable/worsening/improving)
   - Worsening traits increase severity over time
   - Reducing exposure stabilizes/improves traits
   - Severity affects effect magnitude
4. Implement mutation inheritance:
   - Parent mutations checked during reproduction
   - 50% inheritance chance per trait
   - Inherited traits present from birth
   - Inherited traits have lower thresholds (more sensitive)
   - Children of mutated parents tracked
5. Implement mutation visibility:
   - Visible traits affect survivor appearance description
   - Visible traits cause social reactions (fear, discrimination)
   - Social reactions affect trade, relationships, morale
   - Hidden traits only found through medical examination
   - Medical examination reveals all traits
6. Implement mutation management:
   - Chelation therapy reduces cumulative dose
   - Medical treatment slows progression
   - Gene therapy (research unlock) stabilizes traits
   - Management requires resources and time
   - Management cannot reverse existing mutations
7. Implement mutation social effects:
   - Visible mutations cause social stigma
   - Stigma affects faction interactions
   - Stigma affects trade prices
   - Stigma affects survivor relationships
   - Stigma affects morale (self-consciousness)
8. Create mutation events:
   - "The Change" — mutation manifests in survivor
   - "The Discovery" — medical examination reveals mutation
   - "The Inheritance" — child born with inherited mutation
   - "The Progression" — mutation worsens
   - "The Stabilization" — mutation stabilized by treatment
   - "The Stigma" — social discrimination against mutated survivor
   - "The Acceptance" — shelter accepts mutated survivor
9. Add mutation quest hooks:
   - "The Changed" — first mutation manifests
   - "The Doctor" — diagnose and treat mutations
   - "The Gene" — research gene therapy
   - "The Inheritance" — mutated survivor has child
   - "The Stigma" — deal with social discrimination
   - "The Resistance" — beneficial mutation provides advantage
   - "The Choice" — accept or reject mutated survivor
10. Implement mutation UI:
    - Mutation panel: shows survivor mutations, severity, progression
    - Medical examination: reveals hidden mutations
    - Mutation log: history of mutation events
    - Mutation filter: by type (beneficial/harmful/neutral/mixed)
    - Mutation tooltip: hover shows trait effects
11. Add mutation journal: automatic log of mutation events
12. Implement mutation tutorial: first mutation explains system
13. Add mutation tooltips: hover over trait shows effects
14. Create 20 mutation traits in data file (5 per category)

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `RadiationSystem`: cumulative dose triggers mutation checks
2. Connect to `SurvivorLifecycle`: mutations affect lifespan and reproduction
3. Integrate with `DiseaseSystem`: mutations interact with disease pipeline
4. Connect to `NeedsSystem`: mutations affect health and morale
5. Wire into `FactionBranchCoordinator`: visible mutations affect faction interactions
6. Connect to `ResearchSystem`: gene therapy research unlock
7. Implement old-save compatibility: existing saves get empty mutation state
8. Add deterministic seeding: mutation rolls use `ISeededRng`
9. Create exploit prevention: mutations are permanent, management is slow
10. Add tests: mutation manifestation, inheritance, progression, management, save round-trip
11. Verify catalog integrity: all trait IDs resolve
12. Test edge cases: no mutations (low radiation), many mutations (high radiation)
13. Verify headless behavior: mutations process correctly without UI
14. Add data-integrity-selftest: mutation traits validate against survivor catalogs
15. Create `--mutation-selftest` verb for CI validation

## State / System Interaction Model

```text
Radiation mutation system
├─ Mutation threshold tracking
│  ├─ Cumulative dose per survivor
│  ├─ Thresholds: 100/200/500/1000/2000 mSv
│  ├─ Mutation check at each threshold
│  └─ Check uses ISeededRng
├─ Trait manifestation
│  ├─ 20 traits (5 beneficial, 5 harmful, 5 neutral, 5 mixed)
│  ├─ Successful check: trait manifests
│  ├─ Effects applied to survivor
│  ├─ Visible traits change appearance
│  └─ Trait recorded in mutation list
├─ Mutation progression
│  ├─ Some traits worsen with exposure
│  ├─ Progression: stable/worsening/improving
│  ├─ Reduced exposure stabilizes
│  └─ Severity affects magnitude
├─ Mutation inheritance
│  ├─ 50% inheritance per trait
│  ├─ Inherited traits from birth
│  ├─ Lower thresholds for inherited
│  └─ Children tracked
├─ Mutation management
│  ├─ Chelation reduces dose
│  ├─ Treatment slows progression
│  ├─ Gene therapy stabilizes
│  └─ Cannot reverse existing
└─ Integration
   ├─ Radiation (dose triggers)
   ├─ Lifecycle (lifespan, reproduction)
   ├─ Disease (interaction)
   ├─ Needs (health, morale)
   ├─ Factions (social effects)
   └─ Research (gene therapy)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --mutation-selftest
```

## Risk

**MEDIUM** — Mutation system complexity can overwhelm if too many traits and interactions exist. Risk of mutations feeling punitive rather than interesting. Mitigation: balance beneficial/harmful traits, make management meaningful, show clear mutation effects, and ensure mutations create interesting choices rather than pure punishment.

## Definition of Done

- `MutationSystem.cs` exists with full `CaptureState/RestoreState`
- 20 mutation traits (5 beneficial, 5 harmful, 5 neutral, 5 mixed)
- Mutation threshold and manifestation system
- Mutation progression (stable/worsening/improving)
- Mutation inheritance (parent → child)
- Mutation visibility and social effects
- Mutation management (chelation, treatment, gene therapy)
- Mutation events and quest hooks
- Save/load round-trip tested
- Deterministic mutation rolls verified
- Old saves load without error
- 20 mutation traits in data authority
- UI mutation panel
- Cross-system integration (radiation, lifecycle, disease, needs, factions, research)

## Follow-On Opportunities

- Mutation specialization (mutations enable unique abilities)
- Mutation research (study mutations for scientific knowledge)
- Mutation legacy (famous mutations remembered)
- Mutation quests (cure specific mutation, study rare trait)
- Mutation trading (trade mutated survivors with factions)
