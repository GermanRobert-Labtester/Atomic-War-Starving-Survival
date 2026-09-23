# ASHFALL — Oldest Partial / Non-Integrated Plans Audit (2026-09-23)

**Role:** read-only audit produced for the foreman/user, in support of the
next integrator batch.
**Status:** REFERENCE — this document is **not a claim**, grants no path
ownership, and changes no production file, data file, test, or live ledger.
**Method:** current-source verification per `AGENTS.md` Rule 7. For every
candidate plan the named Core authority file was located on disk and the
Godot host (`src/`) was grepped for the type name. A candidate is *partial /
non-integrated* when the Core authority and its tests exist but **the Godot
host has zero references** (host-unreachable). Plan numbers are the canonical
`Next-steps-plans/shipped_to_chat/Plan_NN_*.md` series; titles are the plan
title. This audit does not re-run the full reachability tool; it is a focused
re-measurement of the oldest remaining queue.

**Current queue head (from `INTEGRATION_PLANS.md`):** the 2026-09-23
`UNBLOCK-OLDEST-BATCH1…4` packages completed Plans **38, 39, 42, 46, 49, 51,
52, 54**. The ledger records the next batch as **Plans 55 and 58** — this audit
confirms those are the two oldest remaining host-unreachable plans.

---

## 1. Verified oldest-partial / non-integrated plans (20)

Ordered by canonical plan number (oldest first). `src/ refs` is the number of
Godot host files referencing the authority type (measured 2026-09-23).

| # | Plan | Title | Core authority | `src/` refs | Core test | Class | Blocking note |
|---:|---:|---|---|---:|---|---|---|
| 1 | **55** | The Long Haul: Retention, a Save Corpus, and the 400-Year Campaign | `RetentionPolicyCatalog` / `RollingLog<T>` — `Assets/Ashfall.Core/Records/RetentionPolicy.cs` | **0** | `Ashfall.Core.Tests/Records/Plan55RetentionPolicyIntegrationTests.cs` | gameplay/save | none (additive) |
| 2 | **58** | The Continuation: Outposts, Waystations, and a Second Holdfast | `OutpostSettlementSystem` — `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs` | **0** | `Ashfall.Core.Tests/Settlements/Plan58OutpostSettlementIntegrationTests.cs` | gameplay | **no `CaptureState`/`RestoreState`**; needs custody decision vs `WaystationSystem` / `ColonySystem` / `SettlementCatalog` |
| 3 | 59 | Retrospective: Turn Nine Waves of Findings into Rules | `StandingGateRegistry` — `Assets/Ashfall.Core/Governance/StandingGateRegistry.cs` | 0 | `Plan59StandingGateIntegrationTests` (per ledger) | governance | non-gameplay; likely excluded from the gameplay queue |
| 4 | 135 | Weather → Deep Gameplay Cascade | `WeatherGameplayCascadeEngine` / `WeatherCascadeSystem` — `Assets/Ashfall.Core/Weather/` | 0 | `Plan135WeatherCascadeIntegrationTests` | gameplay | weather owner must supply the live drives |
| 5 | 136 | Wildlife Trapping → Food Pipeline & Cooking | `CookingSystem` — `Assets/Ashfall.Core/Cooking/CookingSystem.cs` | 0 | `Plan136WildlifeCookingIntegrationTests` | gameplay | trapping→inventory transfer seam |
| 6 | 140 | Generational Legacy & Campaign Inheritance | `CampaignLegacySystem` — `Assets/Ashfall.Core/Legacy/CampaignLegacySystem.cs` | 0 | `Plan140GenerationalLegacyIntegrationTests` | gameplay/cross-run | needs completion-record + NG+ seam |
| 7 | 141 | Research → Downstream Unlocks Bridge | `ResearchUnlockBridge` — `Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs` | 0 | `Plan141ResearchUnlockBridgeIntegrationTests` | gameplay | bind to `ResearchSystem.OnResearchCompleted` |
| 8 | 145 | Unified Ending Resolution & Epilogue Personalization | `UnifiedEndingResolver` — `Assets/Ashfall.Core/Endgame/UnifiedEndingResolver.cs` | 0 | `Plan145UnifiedEndingIntegrationTests` | endgame | consumes many read models |
| 9 | 147 | Per-NPC Memory & Relationship Depth | `NpcMemorySystem` — `Assets/Ashfall.Core/Narrative/NpcMemorySystem.cs` | 0 | `NpcMemorySystemTests` | gameplay | NPC authority ownership |
| 10 | 148 | Ideological Friction → Events & Quests | `IdeologicalFrictionEvents` — `Assets/Ashfall.Core/Survivors/IdeologicalFrictionEvents.cs` | 0 | `Plan148IdeologicalFrictionIntegrationTests` | gameplay | needs belief/affinity owner |
| 11 | 150 | Romance & Family Dynamics | `RomanceFamilySystem` — `Assets/Ashfall.Core/Survivors/RomanceFamilySystem.cs` | 0 | `Plan150RomanceFamilyIntegrationTests` | gameplay | seeded RNG stream |
| 12 | 152 | Vehicle Customization & Mobile Base | `VehicleCustomizationSystem` — `Assets/Ashfall.Core/Vehicles/VehicleCustomizationSystem.cs` | 0 | `Plan152VehicleCustomizationIntegrationTests` | gameplay | extend `VehicleGarageSystem` (Plan 50 seam) |
| 13 | 159 | Shelter Governance & Political System | `ShelterGovernanceEngine` — `Assets/Ashfall.Core/Governance/ShelterGovernanceEngine.cs` | 0 | `ShelterGovernanceEngineTests` | gameplay | extends `PolicySystem`/`LeadershipSystem` |
| 14 | 165 | Modding Support & Mod Data Contract | `ModSupportSystem` — `Assets/Ashfall.Core/Mods/ModDataContract.cs` | 0 | `Plan165ModdingIntegrationTests` | tooling/content | overlaps `JsonModLayering` host path |
| 15 | 166 | Shelter Identity, Naming, Origin & Reputation Projection | `ShelterIdentitySystem` — `Assets/Ashfall.Core/Shelter/ShelterIdentitySystem.cs` | 0 | `ShelterIdentitySystemTests` | gameplay/presentation | overlaps reputation owner |
| 16 | 169 | Adaptive Audio Accessibility & Mix Legibility | `AudioAccessibilityCoordinator` — `Assets/Ashfall.Core/Audio/AudioAccessibilityCoordinator.cs` | 0 | `Plan169AudioAccessibilityIntegrationTests` | presentation | binds `AudioManager`/settings |
| 17 | 174 | Procedural Survivor Backstories | `BackstorySystem` — `Assets/Ashfall.Core/Survivors/BackstorySystem.cs` | 0 | (Survivors region) | gameplay/narrative | survivor identity owner |
| 18 | 176 | Aging & Elderly Survivor System | `AgingSystem` / `SurvivorAgingProgressionEngine` — `Assets/Ashfall.Core/Survivors/` | 0 | `SurvivorAgingProgressionEngineTests` | gameplay | tenure clock is single source |
| 19 | 186 | Shelter Maintenance & Degradation | `ShelterMaintenanceSystem` — `Assets/Ashfall.Core/Shelter/ShelterMaintenanceSystem.cs` | 0 | (Shelter region) | gameplay | condition owner |
| 20 | 188 | Individual Survivor Daily Routines | `SurvivorRoutineSystem` — `Assets/Ashfall.Core/Survivors/SurvivorRoutineSystem.cs` | 0 | (Survivors region) | gameplay | needs duty/needs/schedule owners |

Rows 3, 14 and 16 are flagged **non-gameplay / tooling / presentation**; if
the foreman wants a pure gameplay queue, remove them and continue at Plans
189, 191, 194, 196, 197, 199, 202, 204, 214 (all confirmed host-unreachable in
the same measurement).

## 2. Already-sealed neighbours (why the gaps exist)

These earlier-numbered plans are **not** in the queue because their authority
is either host-wired or the plan is non-gameplay tooling/CI: 40 (identity —
`SurvivorDetailPanel`), 41 (memory), 43 (governance/consent), 44 (relations),
45 (content-acceptance CI), 47 (mod contract), 48 (release craft — tooling),
50 (asset truth — tooling), 53 (ambition governance — docs/CI), 56 (repo
hygiene — tooling), 57 (store kit — tooling), 60 (medicine legibility builds on
the already host-wired `DiseaseSystem`). The Plan 160/161/162/163/164/165/157/
158/154/156/144 etc. Core-only authorities were host-wired by
`ORPHAN-SEAL-PRIORITY-W1` and the Wave42 batch series; the remainder of that
cohort is outside the 20 above.

## 3. Queue decision

The ledger's stated head matches this measurement: **Batch 5 = Plans 55 + 58**.
Both are gameplay-bearing, both have Core authorities and focused tests, and
both have zero host references. Plan 55 is additive/low-risk; Plan 58 carries
the named capture/restore + custody blocker that must be resolved inside the
batch (see the batch plan).

**Update 2026-09-23 — Batch 5 and Batch 6 are DONE.** Batch 5 integrated
Plans 55 + 58 (retention, outposts) and Batch 6 integrated Plans 135 + 59
(weather cascade, standing-gate register); see the Batch 5 and Batch 6 entries in
`INTEGRATION_PLANS.md`. The next two oldest host-unreachable plans are **Plan 136**
and **Plan 140**, so the next validation-only governance package should focus on
their Core authorities and confirm the reachability premise in source and data
before claiming paths.

## 4. Re-run command (read-only)

```bash
for n in RetentionPolicy RollingLog OutpostSettlementSystem StandingGateRegistry \
         WeatherCascadeSystem CookingSystem CampaignLegacySystem ResearchUnlockBridge \
         UnifiedEndingResolver NpcMemorySystem IdeologicalFrictionEvents \
         RomanceFamilySystem VehicleCustomizationSystem ShelterGovernanceEngine \
         ModSupportSystem ShelterIdentitySystem AudioAccessibilityCoordinator \
         BackstorySystem AgingSystem SurvivorAgingProgressionEngine \
         ShelterMaintenanceSystem SurvivorRoutineSystem; do
  echo "$n host_refs=$(grep -rl "\b$n\b" src/ --include='*.cs' 2>/dev/null | wc -l)"; \
done
```

No production change, no test change, no data change, no ledger change.
