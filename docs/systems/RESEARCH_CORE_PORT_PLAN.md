# Research Core Port Plan

**Target:** Build the Research / R&D / Breakthrough engine from scratch. No
existing Core engine, no data sidecars, no host adapter — the Phase 9
modal uses hardcoded placeholder strings only.

**Pattern:** Mirror Phase-18 Skill Progression + Phase-27 Standing Record.
Engine-agnostic, no `UnityEngine.*` / `Godot.*` / `JsonUtility`, pure C#
with `IFileIO` / `IJsonSerializer` / `ISeededRng`.

---

## Why this port now

- `SURFACE_GAP_REPORT.md` (Phase 26 close) flagged `ResearchPanel` as
  `MISSING (awaiting Core)` — the last remaining MISSING surface.
- `Assets/Ashfall.Core/Journal/KnowledgeBase.cs` exists as a read-only
  knowledge tracker (14 canonical discovery keys), but does not power
  Research / R&D / Breakthrough trees.
- No `research*.json` sidecars exist in `StreamingAssets/Data/`.
- No host adapter exists.

---

## Four-phase plan

### Phase 1 — Definitions + State + Engine (Core)

`Assets/Ashfall.Core/Research/ResearchKnowledgeDef.cs` — individual
knowledge node. Engine-agnostic POCO.

```csharp
public sealed class ResearchKnowledgeDef
{
    public string id;           // snake_case knowledge id
    public string displayName;  // player-facing label
    public string category;     // discipline category
    public string description;  // one-sentence flavour
    public string[] prerequisites; // knowledge ids that must be unlocked first
    public string breakthroughItem; // item_id awarded on completion
    public int daysToComplete;  // days needed in research queue
}
```

`Assets/Ashfall.Core/Research/ResearchState.cs` — unified state envelope.

```csharp
[Serializable]
public sealed class ResearchState
{
    public string systemId = ResearchSystem.SystemId;
    public bool expansionUnlocked;
    public int currentDay;
    public List<string> unlockedIds = new();
    public string activeResearchId; // currently researching node (empty = idle)
    public int activeResearchDays;  // days spent on current node so far
    public List<string> completedIds = new();
}
```

`Assets/Ashfall.Core/Research/ResearchSystem.cs` — engine. Inline
default catalog (~15 nodes).

```csharp
public sealed class ResearchSystem
{
    public const string SystemId = "research_system";

    public ResearchState State { get; private set; }

    private readonly Dictionary<string, ResearchKnowledgeDef> _catalog = new();
    private readonly ILog _log;

    public ResearchSystem(ILog log = null, ResearchState state = null) { ... }

    public void Register(ResearchKnowledgeDef def) { ... }
    public void RegisterDefaults() { /* ~15 nodes inline */ }

    public bool StartResearch(string id, int day) { ... }
    public void Tick(int newDay) { /* progress active node by (newDay - currentDay) days */ }
    public bool CompleteResearch(string id) { ... }

    public ResearchState CaptureState() => State;
    public void RestoreState(ResearchState saved) { ... }
}
```

### Phase 2 — Default catalog (~15 nodes, inline)

| id | displayName | category | days |
|---|---|---|---|
| knowledge_water_basics | Water Purification Basics | survival | 5 |
| knowledge_water_advanced | Advanced Water Filtration | survival | 12 |
| knowledge_radiation_basics | Radiation Medicine Basics | medical | 5 |
| knowledge_radiation_shielding | Radiation Shielding Materials | engineering | 15 |
| knowledge_gas_mask_improved | Improved Gas Masks | engineering | 10 |
| knowledge_hydroponics | Hydroponic Cultivation | survival | 8 |
| knowledge_solar_basics | Solar Power Basics | engineering | 7 |
| knowledge_solar_advanced | Solar Power Systems | engineering | 14 |
| knowledge_food_preservation | Food Preservation | survival | 10 |
| knowledge_radio_basics | Radio Signal Processing | science | 6 |
| knowledge_radio_advanced | Encrypted Radio Communication | science | 12 |
| knowledge_shelter_insulation | Shelter Insulation | engineering | 8 |
| knowledge_air_filtration | Air Filtration Systems | engineering | 10 |
| knowledge_scavenge_efficiency | Scavenge Efficiency | scavenging | 7 |
| knowledge_combat_training | Combat Training Doctrine | combat | 8 |

### Phase 3 — Host adapter (Godot)

`src/Host/ResearchHostSession.cs` — owns `ResearchSystem`, wires
`SurvivorsHostSession.AdvanceDay`, exposes `CaptureSave()` /
`RestoreSave()`.

### Phase 4 — UI dashboard (Tier-3 HYBRID)

`src/UI/ResearchAtlasPanel.cs` — Tier-3 HYBRID sub-card sibling of
`ResearchPanel.cs`. 6-card status rail (Total nodes / Unlocked /
Active / Completed / Days remaining / Breakthroughs) + 3 DataGrid
tiles (Knowledge nodes / Active research / Breakthrough items) +
right-side detail inspector. Reuses 5 primitives.

Snapshot target `research_atlas_default`.

### Tests

`Ashfall.Core.Tests/ResearchSystemTests.cs` — 8 tests:
1. Register → catalog size = 15
2. StartResearch sets activeResearchId
3. Tick progresses active node by days
4. CompleteResearch awards breakthroughItem + marks completed
5. StartResearch_PrerequisiteGated — rejected if prerequisite missing
6. StartResearch_AlreadyCompleted — rejected
7. CaptureState round-trip
8. Determinism under same seed

---

## Files

| Path | New? | Lines |
|---|---|---|
| `Assets/Ashfall.Core/Research/ResearchKnowledgeDef.cs` | NEW | ~30 |
| `Assets/Ashfall.Core/Research/ResearchState.cs` | NEW | ~30 |
| `Assets/Ashfall.Core/Research/ResearchSystem.cs` | NEW | ~180 |
| `Ashfall.Core.Tests/ResearchSystemTests.cs` | NEW | ~180 |
| `src/Host/ResearchHostSession.cs` | NEW | ~120 |
| `src/UI/ResearchAtlasPanel.cs` | NEW | ~470 |
| `docs/systems/RESEARCH_CORE_PORT_PLAN.md` | NEW | this file |

Six files total — mirrors the Phase-18 Skill Progression port.

---

## Verification checklist

```
1. dotnet build Ashfall.Core/Ashfall.Core.csproj          # 0/0
2. dotnet test Ashfall.Core.Tests/...csproj               # +8 tests, all PASS
3. dotnet build Ashfall.csproj                             # Godot host: 0/0
4. godot --path . -- --ui-snapshot-uitest                  # 29/29
```

**SHIPPED at Phase 28** — All 8 tests PASS, 2016/2016 Core tests, 29/29 snapshots green, 0 build warnings. The Research engine (`ResearchSystem`) and host adapter (`ResearchHostSession`) are live; the dashboard (`ResearchAtlasPanel`) renders the 15-node catalog with prerequisite gating and breakthrough awards. No regression introduced.

---

## Why this port now

- `SURFACE_GAP_REPORT.md` (Phase 26 close) flagged `ResearchPanel` as
  `MISSING (awaiting Core)` — the last remaining MISSING surface.
- `Assets/Ashfall.Core/Journal/KnowledgeBase.cs` exists as a read-only
  knowledge tracker (14 canonical discovery keys), but does not power
  Research / R&D / Breakthrough trees.
- No `research*.json` sidecars exist in `StreamingAssets/Data/`.
- No host adapter exists.

---

## Four-phase plan

### Phase 1 — Definitions + State + Engine (Core)

`Assets/Ashfall.Core/Research/ResearchKnowledgeDef.cs` — individual
knowledge node. Engine-agnostic POCO.

```csharp
public sealed class ResearchKnowledgeDef
{
    public string id;           // snake_case knowledge id
    public string displayName;  // player-facing label
    public string category;     // discipline category
    public string description;  // one-sentence flavour
    public string[] prerequisites; // knowledge ids that must be unlocked first
    public string breakthroughItem; // item_id awarded on completion
    public int daysToComplete;  // days needed in research queue
}
```

`Assets/Ashfall.Core/Research/ResearchState.cs` — unified state envelope.

```csharp
[Serializable]
public sealed class ResearchState
{
    public string systemId = ResearchSystem.SystemId;
    public bool expansionUnlocked;
    public int currentDay;
    public List<string> unlockedIds = new();
    public string activeResearchId; // currently researching node (empty = idle)
    public int activeResearchDays;  // days spent on current node so far
    public List<string> completedIds = new();
}
```

`Assets/Ashfall.Core/Research/ResearchSystem.cs` — engine. Inline
default catalog (~15 nodes).

```csharp
public sealed class ResearchSystem
{
    public const string SystemId = "research_system";

    public ResearchState State { get; private set; }

    private readonly Dictionary<string, ResearchKnowledgeDef> _catalog = new();
    private readonly ILog _log;

    public ResearchSystem(ILog log = null, ResearchState state = null) { ... }

    public void Register(ResearchKnowledgeDef def) { ... }
    public void RegisterDefaults() { /* ~15 nodes inline */ }

    public bool StartResearch(string id, int day) { ... }
    public void Tick(int newDay) { /* progress active node by (newDay - currentDay) days */ }
    public bool CompleteResearch(string id) { ... }

    public ResearchState CaptureState() => State;
    public void RestoreState(ResearchState saved) { ... }
}
```

### Phase 2 — Default catalog (~15 nodes, inline)

| id | displayName | category | days |
|---|---|---|---|
| knowledge_water_basics | Water Purification Basics | survival | 5 |
| knowledge_water_advanced | Advanced Water Filtration | survival | 12 |
| knowledge_radiation_basics | Radiation Medicine Basics | medical | 5 |
| knowledge_radiation_shielding | Radiation Shielding Materials | engineering | 15 |
| knowledge_gas_mask_improved | Improved Gas Masks | engineering | 10 |
| knowledge_hydroponics | Hydroponic Cultivation | survival | 8 |
| knowledge_solar_basics | Solar Power Basics | engineering | 7 |
| knowledge_solar_advanced | Solar Power Systems | engineering | 14 |
| knowledge_food_preservation | Food Preservation | survival | 10 |
| knowledge_radio_basics | Radio Signal Processing | science | 6 |
| knowledge_radio_advanced | Encrypted Radio Communication | science | 12 |
| knowledge_shelter_insulation | Shelter Insulation | engineering | 8 |
| knowledge_air_filtration | Air Filtration Systems | engineering | 10 |
| knowledge_scavenge_efficiency | Scavenge Efficiency | scavenging | 7 |
| knowledge_combat_training | Combat Training Doctrine | combat | 8 |

### Phase 3 — Host adapter (Godot)

`src/Host/ResearchHostSession.cs` — owns `ResearchSystem`, wires
`SurvivorsHostSession.AdvanceDay`, exposes `CaptureSave()` /
`RestoreSave()`.

### Phase 4 — UI dashboard (Tier-3 HYBRID)

`src/UI/ResearchAtlasPanel.cs` — Tier-3 HYBRID sub-card sibling of
`ResearchPanel.cs`. 6-card status rail (Total nodes / Unlocked /
Active / Completed / Days remaining / Breakthroughs) + 3 DataGrid
tiles (Knowledge nodes / Active research / Breakthrough items) +
right-side detail inspector. Reuses 5 primitives.

Snapshot target `research_atlas_default`.

### Tests

`Ashfall.Core.Tests/ResearchSystemTests.cs` — 8 tests:
1. Register → catalog size = 15
2. StartResearch sets activeResearchId
3. Tick progresses active node by days
4. CompleteResearch awards breakthroughItem + marks completed
5. StartResearch_PrerequisiteGated — rejected if prerequisite missing
6. StartResearch_AlreadyCompleted — rejected
7. CaptureState round-trip
8. Determinism under same seed

---

## Files

| Path | New? | Lines |
|---|---|---|
| `Assets/Ashfall.Core/Research/ResearchKnowledgeDef.cs` | NEW | ~30 |
| `Assets/Ashfall.Core/Research/ResearchState.cs` | NEW | ~30 |
| `Assets/Ashfall.Core/Research/ResearchSystem.cs` | NEW | ~180 |
| `Ashfall.Core.Tests/ResearchSystemTests.cs` | NEW | ~180 |
| `src/Host/ResearchHostSession.cs` | NEW | ~120 |
| `src/UI/ResearchAtlasPanel.cs` | NEW | ~400 |
| `docs/systems/RESEARCH_CORE_PORT_PLAN.md` | NEW | this file |

Six files total — mirrors the Phase-18 Skill Progression port.

---

## Verification checklist

```
1. dotnet build Ashfall.Core/Ashfall.Core.csproj          # 0/0
2. dotnet test Ashfall.Core.Tests/...csproj               # +8 tests, all PASS
3. dotnet build Ashfall.csproj                             # Godot host: 0/0
4. godot --path . -- --ui-snapshot-uitest                  # 29/29
```
