# Micro-Location UI Presentation Specification & Verification

**Subsystem:** Expedition UI / Narrative Encounter Surface
**Component:** `src/UI/ExpeditionPanel.cs`
**Related Systems:** `AshfallLocalization`, `ExpeditionEncounterBridge`, `Theme`
**Status:** Certified / Production Ready

---

## 1. Overview & Architecture

Micro-locations are non-combat wasteland discovery encounters surfaced during active scavenging sorties. Unlike standard hostile or random event encounters, micro-locations represent environmental storytelling vignettes offering tactical tradeoffs: risk, salvage, moral consequence, journal knowledge unlocks, or cartographic reveals.

The presentation pipeline flows from Core through the encounter bridge to the Godot modal presentation layer:

```
micro_locations.json
  └── NarrativeEncounterSystem.LoadFile()
        └── ExpeditionEncounterBridge.Surface()
              └── ExpeditionHostSession.OnEncounterSurfaced
                    └── ExpeditionPanel.ShowEncounterNotice()
                          ├── ShowNextModal() (Title, Context, Autowrapped Body)
                          └── RenderChoiceButtons() (Badges, Cost/Gain, Focus)
```

---

## 2. Modal Layout & Presentation Contract

### 2.1 Context Header
- When `_lastSurfaced.is_micro_location` is `true`:
  - `_encounterContext.Visible = true;`
  - `_encounterContext.Text = "DISCOVERY · MICRO-LOCATION";`
  - `_encounterFactionEmblem.Visible = false;`
- Title is translated through `AshfallLocalization.Tr($"discovery.{encounter_id}.title", fallback)`.

### 2.2 Narrative Body Typography & Autowrapping
- Autowrap mode: `TextServer.AutowrapMode.WordSmart`
- Layout sizing: `SizeFlagsHorizontal = Control.SizeFlags.ExpandFill`
- Container: Scrollable with `HorizontalScrollMode = ScrollMode.Disabled` and `VerticalScrollMode = ScrollMode.Auto`
- Card dimensions: `CustomMinimumSize = Vector2(480, 0)`
- **Longest Text Guarantee:** Certified against `micro_frozen_bus` (198 characters) without visual clipping or text overflow.

### 2.3 Choice Badges & Semantic Colors

| Indicator | Format | Color Token | Hex | Authority |
|---|---|---|---|---|
| **Reward** | `Gain: {ItemName} ×{Qty}` | `Theme.Lethe` | `#6EA3A8` | Positive `grantItemQuantity` (expedition pack loot) |
| **Offering / Cost** | `Cost: {ItemName} ×{Needed} ({Held}/{Needed})` | `Theme.LetheAmber` / `Theme.Critical` | `#D4A35A` / `#E63333` | Negative `grantItemQuantity` or `costItems` (shelter inventory) |
| **Journal Clue** | `[CODEX] Clue Unlocked: {JournalName}` | `Theme.Cyan` | `#6EA3A8` | `journalUnlockId` |
| **Map Discovery** | `[MAP] Discovers: {LocationName}` | `Theme.Cyan` | `#6EA3A8` | `discoverLocationId` |
| **One-Time Depletion** | `[ONE-TIME]` | `Theme.LetheAmber` | `#D4A35A` | `depletesOnResolve == true` |

### 2.4 Keyboard Navigation & Focus
- The first actionable (enabled) choice button automatically grabs keyboard/gamepad focus via `GrabFocus()`.
- If all choices are unavailable or the notice is non-interactive, focus falls back to the `OK` button (`_encounterBtnOk`).

---

## 3. Exemplar Case Studies

### Exemplar 1: Roadside Memorial (`micro_roadside_memorial`)
- **Title:** Roadside Memorial
- **Context:** `DISCOVERY · MICRO-LOCATION`
- **Choice 1 (`leave_memorial`):** Morale: +1
- **Choice 2 (`take_offering`):** Morale: -1, Guilt: +2
  - Badges: `Gain: Cloth ×1`, `[ONE-TIME]`

### Exemplar 2: Crashed Supply Truck (`micro_crashed_truck`)
- **Title:** Crashed Supply Truck
- **Context:** `DISCOVERY · MICRO-LOCATION`
- **Choice 1 (`search_truck_cargo`):**
  - Badges: `Gain: Canned Food ×2`, `[ONE-TIME]`
- **Choice 2 (`search_truck_cab`):**
  - Badges: `Gain: Sealed Government Document ×1`, `[ONE-TIME]`
- **Choice 3 (`ignore_truck`):** Neutral bypass

### Exemplar 3: Military Observation Post (`micro_observation_post`)
- **Title:** Military Observation Post
- **Context:** `DISCOVERY · MICRO-LOCATION`
- **Choice 1 (`search_observation_post`):** Equipment / intelligence search
- **Choice 2 (`read_grid_references`):**
  - Badges: `[CODEX] Clue Unlocked: Micro Observation Post Grid`, `[MAP] Discovers: Rural Gas Station`

### Exemplar 4: Frozen Evacuation Bus (`micro_frozen_bus`)
- **Title:** Frozen Evacuation Bus
- **Description Length:** 198 characters (Longest production exemplar)
- **Wrapping:** WordSmart autowrap across full width
- **Choice 3 (`read_bus_tag`):**
  - Badges: `[CODEX] Clue Unlocked: Micro Frozen Bus Transit Tag`

---

## 4. Verification

Headless execution via:
```bash
godot --headless --path . -- --expedition-panel-uitest
```
Result: **PASS (exit code 0)**.
