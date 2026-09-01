# Plan 30 Baseline Inventory — Ritual, Faith & Meaning

---

## 1. Verified Starting Assets & Catalogs

- **Folklore Catalogs:**
  - `Assets/StreamingAssets/Data/narrative/bunker_children_folklore.json` — 7 entries
  - `Assets/StreamingAssets/Data/narrative/bunker_children_folklore_batch_2.json` — 8 entries
  - `Assets/StreamingAssets/Data/narrative/childrens_folklore_expansion.json` — 25 entries
- **Graffiti & Environmental Echoes:**
  - `Assets/StreamingAssets/Data/bunker_graffiti_postings.json` — 10 entries
- **Memorial Catalogs:**
  - `Assets/StreamingAssets/Data/memorials_expansion_05.json` — 18 entries
  - `Assets/StreamingAssets/Data/narrative/memorials_expansion.json` — 18 entries
- **Event Engine:**
  - `Assets/StreamingAssets/Data/events.json` — 112 event arcs
- **Core Runtime Authorities:**
  - `MemorialSystem` (`Assets/Ashfall.Core/Memorial/MemorialSystem.cs`)
  - `GuiltInsomniaSystem` (`Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs`)
  - `IdeologicalFrictionSystem` (`Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs`)
  - `LeadershipSystem` (`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs`)
  - `CohortSystem` (`Assets/Ashfall.Core/CohortSystem.cs`)
  - `FinalWishSystem` (`Assets/Ashfall.Core/Survivors/FinalWishSystem.cs`)

---

## 2. Key Gaps Addressed in Plan 30

1. **Underground Culture Fragmentation:** While child folklore existed in narrative files, there were few explicit operational survival rhymes, emergent optional rituals, or ambient environmental echoes.
2. **Grief Lifecycle Staging:** Death immediately caused a flat morale drop, but lacked a staged human aftermath over time (acute shock -> empty shift -> return of the ordinary -> memorial observance -> long-tail anniversary).
3. **Absence of Fictional Belief Movements:** Survivors had personal philosophical tags in `IdeologicalFrictionSystem`, but no authored post-Exchange belief movements (Ash Witnesses, Rebuilders, Listeners) with comfort themes, blind spots, and event hooks.
