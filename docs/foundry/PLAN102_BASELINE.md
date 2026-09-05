# Plan 102 — Baseline Reconnaissance & Treaty Accord Scope

**Plan:** Plan 102 — Foundry Accords Expansion: 4 → 10 Inter-Faction Treaties
**Data Authority:** `Assets/StreamingAssets/Data/foundry_accords.json`
**C# Models:** `Assets/Ashfall.Core/Narrative/RegionalTreatyCatalog.cs` (`RegionalTreatiesFile`, `RegionalTreatyEntry`)
**Loader / Host Consumers:** `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs`, `src/Foundry/SilentFoundryHostSession.cs`, `src/Main.ShelterSocial.cs`

---

## 1. Executive Context & Repository Truth

The historical roadmap item for Plan 102 (`piagentsplans/102-foundry-accords-expansion.md`) called for expanding `foundry_accords.json` from the original 4 District 8 treaties to at least 10 inter-faction treaties.

In the active codebase:
- Baseline District 8 accords: 4 treaties (`treaty_brine_pipe_and_iodine_exchange`, `treaty_cluster_labour_schedule`, `treaty_road_iron_charter`, `treaty_the_cluster_charter`).
- Regional expansion accords: 8 treaties spanning the Verge, Flotilla Shallows, High Scarp, Caravanserai, Recovery Yard, Neutral Ground buffer, and Deep Coast.
- Total accords active in authority: **12 treaties** (exceeding the 10-treaty target).
- All 12 treaties are active, loaded by `RegionalTreatyCatalog`, and tested by `Plan16CartographyTests.cs`, `SilentFoundryHeadlessDemo.cs`, and `FoundryAccordExpansionTests.cs`.

---

## 2. Parity Invariant

The 4 original District 8 accords:
1. `treaty_brine_pipe_and_iodine_exchange` (Ratified Day 280)
2. `treaty_cluster_labour_schedule` (Ratified Day 305)
3. `treaty_road_iron_charter` (Ratified Day 330)
4. `treaty_the_cluster_charter` (Ratified Day 365)

remain **strictly preserved without any modification** to their IDs, ratified days, titles, signatories, water allocations, power quotas, tariffs, articles, penalties, or tags.

---

## 3. Schema & Data Flow

```text
Assets/StreamingAssets/Data/foundry_accords.json
        ↓ (RegionalTreatiesFile DTO)
SilentFoundryCatalogLoader.LoadAccordRatificationDays() / RegionalTreatyCatalog.Load()
        ↓
SilentFoundrySystem.AssessTreatyCompliance() / SilentFoundryHostSession
        ↓
Assets/StreamingAssets/Data/foundry_treaty_consequences.json (Plan 103)
        ↓
FactionStanceEngine (Standing) / MarketSystem (Price/Demand Modifiers)
```
