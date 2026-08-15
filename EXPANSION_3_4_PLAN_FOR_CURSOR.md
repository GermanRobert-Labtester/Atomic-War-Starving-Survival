# ASHFALL — EXPANSION 3 & 4: UI & ASSET PLAN FOR CURSOR

> **Target**: Cursor IDE with Figma MCP (Edu Pro) + Canva Business MCP
> **Scope**: 8 new UI widgets + 27 Canva assets + GameBootstrap wiring
> **Handoff from**: Pi (all C# systems + JSON data will be complete before UI begins)

---

## I. PRE-FLIGHT: FIGMA MCP SETUP

### Connect Figma to the ASHFALL Design File
```json
{
  "mcpServers": {
    "figma": {
      "command": "npx",
      "args": ["@anthropic/figma-mcp"],
      "env": {
        "FIGMA_PERSONAL_TOKEN": "<your-edu-pro-token>",
        "FIGMA_FILE_KEY": "<ashfall-hud-design-file-key>"
      }
    }
  }
}
```

### Key Figma Queries to Run
```
1. "Get the color palette from the DiegeticHud frame"
   → Extract amber (#FFC107), red (#F44336), green (#4CAF50), grey (#9E9E9E)
   
2. "Get the typography scale from the Barlow Condensed text styles"
   → Font sizes, weights, letter-spacing for HUD text

3. "Get the spacing grid from the HUD layout"
   → Margin/padding values for widget positioning

4. "Get the SurvivorPortraitCard component dimensions"
   → Know exactly where badge overlays go

5. "Get the EventModalUI background and border styles"
   → Reuse for LocationDetailPanel and QuestlineProgressTracker modals
```

---

## II. PRE-FLIGHT: CANVA BUSINESS MCP SETUP

### Connect Canva
```json
{
  "mcpServers": {
    "canva": {
      "command": "npx",
      "args": ["@canva/mcp"],
      "env": {
        "CANVA_API_KEY": "<your-business-api-key>"
      }
    }
  }
}
```

### Canva Asset Generation Batch
Generate all 27 assets in one session. Use the dark-industrial palette from Figma.

**Batch 1 — Icons (SVG, 24×24 unless noted)**:
```
1. icon_skull_danger — minimalist skull, amber #FFC107
2. icon_radiation_symbol — trefoil, yellow on dark circle
3. icon_collapse_warning — cracked building, red #F44336
4-8. faction_icon_garrison/militia/cult/warlord/scavenger — faction emblems, 32×32
9. icon_biohazard — biohazard symbol, 16×16, orange
10. icon_expired — clock with X, 16×16, grey
11. icon_checkmark — green check in circle, 16×16
12-14. quest_stage_circle_active/completed/locked — 32×32 circles
15-17. icon_hatch_shield_wood/steel/composite — shield icons, 32×32
18-22. icon_command_hold_line/retreat/suppressive/trap/flush — tactical icons, 32×32
23. icon_cargo — box with weight, 24×24
24. icon_winch — hook and cable, 24×24
25. icon_solar_panel — panel with sun rays, 24×24
26. icon_spyglass — magnifying glass, 24×24
27. icon_warning_triangle — yellow triangle with !, 24×24
```

**Batch 2 — Textures/Backgrounds (PNG)**:
```
28. condition_bar_gradient — horizontal gradient green→yellow→red, 128×4px
29. siege_status_bg — dark slate bar texture, 1024×48px
30. vehicle_dashboard_bg — industrial metal texture, 400×200px
31. faction_panel_bg — dark translucent overlay, 300×600px
32. quest_stage_connector_line — horizontal line for quest stages, 64×2px
```

---

## III. 8 WIDGETS — COMPLETE SPECIFICATIONS

### WIDGET 1: LocationDetailPanel
**Files**: `LocationDetailPanel.cs`, `.uxml`, `.uss`

**Purpose**: Shows procedural danger/radiation/collapse/loot data for a selected expedition location.

**UXML structure**:
```xml
<ui:VisualElement class="location-detail-panel">
    <ui:Label name="location-name" class="location-name" />
    <ui:VisualElement name="danger-row" class="stat-row">
        <ui:Label text="DANGER" class="stat-label" />
        <ui:VisualElement name="danger-skulls" class="danger-skulls" />
    </ui:VisualElement>
    <ui:VisualElement name="radiation-row" class="stat-row">
        <ui:Label text="RADIATION" class="stat-label" />
        <ui:ProgressBar name="radiation-bar" class="radiation-bar" />
        <ui:Label name="radiation-value" class="stat-value" />
    </ui:VisualElement>
    <ui:VisualElement name="collapse-row" class="stat-row">
        <ui:Label text="COLLAPSE RISK" class="stat-label" />
        <ui:ProgressBar name="collapse-bar" class="collapse-bar" />
    </ui:VisualElement>
    <ui:VisualElement name="faction-row" class="stat-row">
        <ui:Label text="CONTROLLED BY" class="stat-label" />
        <ui:VisualElement name="faction-icon" class="faction-icon" />
        <ui:Label name="faction-name" class="faction-name" />
    </ui:VisualElement>
    <ui:VisualElement name="loot-preview" class="loot-preview">
        <ui:Label text="EXPECTED LOOT" class="section-label" />
        <!-- Dynamic loot entries -->
    </ui:VisualElement>
</ui:VisualElement>
```

**C# signatures**:
```csharp
public void ShowLocation(string locationId, string displayName,
    float dangerLevel, float ambientSv, float collapseRisk,
    string factionOwner, List<LootPreviewEntry> lootPreview);
public void Hide();
```

**States**:
- Danger: 0-5 skull icons, scale linearly
- Radiation bar: 0-0.1 Sv = green, 0.1-0.5 = yellow, 0.5+ = red
- Collapse bar: 0-0.3 = green, 0.3-0.6 = yellow, 0.6+ = red
- Faction: Show matching faction icon + name
- Loot preview: List 3-5 items with drop chance %

---

### WIDGET 2: ItemConditionBadge
**Files**: `ItemConditionBadge.cs`, `.uxml`, `.uss`

**Purpose**: Small overlay badge on inventory item slots showing condition, contamination, and expiration.

**UXML structure**:
```xml
<ui:VisualElement class="item-condition-badge">
    <ui:VisualElement name="condition-bar-bg" class="condition-bar-bg">
        <ui:VisualElement name="condition-bar-fill" class="condition-bar-fill" />
    </ui:VisualElement>
    <ui:Label name="condition-text" class="condition-text" />
    <ui:VisualElement name="contamination-icon" class="contamination-icon" />
    <ui:Label name="expiration-label" class="expiration-label" />
</ui:VisualElement>
```

**C# signatures**:
```csharp
public void SetCondition(float conditionPct);
public void SetContamination(float contaminationPct);
public void SetExpirationState(ExpirationState state); // Fresh/Expired/Degraded
public void Hide();
```

**States**:
- Condition > 70%: green fill, no text
- Condition 30-70%: yellow fill, "WORN" text
- Condition < 30%: red fill, "DAMAGED" text
- Contamination > 0: show biohazard icon, opacity = contaminationPct
- Expired: red "EXPIRED" overlay
- Degraded: orange "DEGRADED" overlay

---

### WIDGET 3: QuestlineProgressTracker
**Files**: `QuestlineProgressTracker.cs`, `.uxml`, `.uss`

**Purpose**: Visual multi-stage quest tracker showing connected stages with current progress.

**UXML structure**:
```xml
<ui:VisualElement class="questline-tracker">
    <ui:Label name="quest-title" class="quest-title" />
    <ui:VisualElement name="stages-container" class="stages-container">
        <!-- Dynamic stage circles with connector lines -->
    </ui:VisualElement>
    <ui:Label name="stage-description" class="stage-description" />
    <ui:VisualElement name="choices-container" class="choices-container">
        <!-- Choice buttons for crisis stages -->
    </ui:VisualElement>
</ui:VisualElement>
```

**C# signatures**:
```csharp
public void ShowQuest(string questId, string title, QuestStageData[] stages,
    int currentStageIndex);
public void SetStageComplete(int stageIndex);
public void ShowChoices(StageChoice[] choices, Action<string> onChoiceSelected);
public void MarkQuestComplete();
public void MarkQuestFailed();
```

**States per stage**:
- Locked: grey circle, no connector lit
- Active: amber pulsing circle, connector from previous lit
- Completed: green circle with checkmark, connector fully lit
- Choice available: show choice buttons below stages

---

### WIDGET 4: SiegeStatusHUD
**Files**: `SiegeStatusHUD.cs`, `.uxml`, `.uss`

**Purpose**: Top-of-screen siege status bar during hatch defense with tactical command buttons.

**UXML structure**:
```xml
<ui:VisualElement class="siege-status">
    <ui:VisualElement name="hatch-integrity-section" class="siege-section">
        <ui:Label text="HATCH INTEGRITY" class="siege-label" />
        <ui:ProgressBar name="hatch-integrity-bar" class="hatch-integrity-bar" />
        <ui:VisualElement name="reinforcement-icon" class="reinforcement-icon" />
    </ui:VisualElement>
    <ui:VisualElement name="breach-section" class="siege-section">
        <ui:Label text="BREACH PROGRESS" class="siege-label" />
        <ui:ProgressBar name="breach-progress-bar" class="breach-progress-bar" />
    </ui:VisualElement>
    <ui:VisualElement name="active-effects" class="siege-section">
        <!-- Active tactical effect badges -->
    </ui:VisualElement>
    <ui:VisualElement name="command-buttons" class="command-buttons">
        <!-- Dynamic command buttons -->
    </ui:VisualElement>
</ui:VisualElement>
```

**C# signatures**:
```csharp
public void ShowSiege(float hatchIntegrity, int reinforcementTier,
    float breachProgress);
public void UpdateIntegrity(float newValue);
public void UpdateBreachProgress(float newValue);
public void AddActiveEffect(string effectId, float duration);
public void RemoveActiveEffect(string effectId);
public void SetAvailableCommands(SiegeCommand[] commands,
    Action<string> onCommandIssued);
public void HideSiege();
```

**States**:
- Not under siege: hidden
- Under siege: full bar visible, red pulsing border
- Hatch integrity > 60%: green bar
- Hatch integrity 30-60%: yellow bar
- Hatch integrity < 30%: red flashing bar
- Active effects: small badge icons with countdown timers
- Commands: buttons with cooldown indicators, greyed out when unavailable

---

### WIDGET 5: FactionIntelligencePanel
**Files**: `FactionIntelligencePanel.cs`, `.uxml`, `.uss`

**Purpose**: Side panel showing faction standings, active intelligence, and espionage options.

**UXML structure**:
```xml
<ui:VisualElement class="faction-intelligence-panel">
    <ui:TabView name="faction-tabs">
        <!-- One tab per faction -->
    </ui:TabView>
    <ui:VisualElement name="standing-section">
        <ui:Label text="STANDING" class="section-label" />
        <ui:ProgressBar name="standing-bar" class="standing-bar" />
        <ui:Label name="standing-value" class="standing-value" />
    </ui:VisualElement>
    <ui:VisualElement name="intel-section">
        <ui:Label text="ACTIVE INTEL" class="section-label" />
        <!-- Dynamic intel entries -->
    </ui:VisualElement>
    <ui:VisualElement name="tribute-section">
        <ui:Label text="TRIBUTE DUE" class="section-label" />
        <ui:Label name="tribute-info" class="tribute-info" />
    </ui:VisualElement>
    <ui:VisualElement name="agent-section">
        <ui:Label text="COVERT OPERATIONS" class="section-label" />
        <ui:Button name="send-agent-button" text="SEND AGENT" />
        <ui:Button name="propaganda-button" text="BROADCAST" />
    </ui:VisualElement>
</ui:VisualElement>
```

**C# signatures**:
```csharp
public void SetFaction(string factionId, string factionName,
    float standing, bool hasAlliance);
public void AddIntelEntry(IntelEntry entry); // type, description, hoursRemaining
public void ClearIntel();
public void SetTributeDemand(string resourceType, int amount, int dueInDays);
public void SetAgentStatus(string agentName, float discoveryRisk);
public void SetActionsAvailable(bool canSendAgent, bool canBroadcast);
```

---

### WIDGET 6: VehicleStatusPanel
**Files**: `VehicleStatusPanel.cs`, `.uxml`, `.uss`

**Purpose**: Dashboard-style panel showing vehicle condition, fuel, cargo, and modifications.

**UXML structure**:
```xml
<ui:VisualElement class="vehicle-status-panel">
    <ui:Label name="vehicle-name" class="vehicle-name" />
    <ui:VisualElement name="condition-gauge" class="gauge">
        <ui:Label text="CONDITION" class="gauge-label" />
        <ui:ProgressBar name="condition-bar" class="condition-bar" />
    </ui:VisualElement>
    <ui:VisualElement name="fuel-gauge" class="gauge">
        <ui:Label text="FUEL" class="gauge-label" />
        <ui:ProgressBar name="fuel-bar" class="fuel-bar" />
        <ui:Label name="fuel-value" class="gauge-value" />
    </ui:VisualElement>
    <ui:VisualElement name="cargo-gauge" class="gauge">
        <ui:Label text="CARGO" class="gauge-label" />
        <ui:ProgressBar name="cargo-bar" class="cargo-bar" />
        <ui:Label name="cargo-value" class="gauge-value" />
    </ui:VisualElement>
    <ui:VisualElement name="modifications" class="modifications">
        <ui:Label text="MODIFICATIONS" class="section-label" />
        <ui:VisualElement name="mod-slots" class="mod-slots">
            <!-- Dynamic mod icons -->
        </ui:VisualElement>
    </ui:VisualElement>
    <ui:Label name="breakdown-risk" class="breakdown-risk" />
</ui:VisualElement>
```

**C# signatures**:
```csharp
public void ShowVehicle(string vehicleName, float condition, float fuel,
    float maxFuel, float cargo, float maxCargo);
public void UpdateCondition(float newValue);
public void UpdateFuel(float fuel, float maxFuel);
public void UpdateCargo(float cargo, float maxCargo);
public void SetModifications(string[] modIds);
public void SetBreakdownRisk(float risk);
public void Hide();
```

---

### WIDGET 7: TacticalCommandBar
**Files**: `TacticalCommandBar.cs`, `.uxml`, `.uss`

**Purpose**: Bottom-of-screen command bar during combat with cooldown indicators.

**UXML structure**:
```xml
<ui:VisualElement class="tactical-command-bar">
    <ui:Button name="cmd-hold-line" class="command-button">
        <ui:VisualElement name="icon-hold" class="command-icon" />
        <ui:Label text="HOLD LINE" class="command-label" />
        <ui:VisualElement name="cooldown-overlay" class="cooldown" />
    </ui:Button>
    <ui:Button name="cmd-retreat" class="command-button">
        <ui:VisualElement name="icon-retreat" class="command-icon" />
        <ui:Label text="RETREAT" class="command-label" />
    </ui:Button>
    <ui:Button name="cmd-suppressive" class="command-button">
        <ui:VisualElement name="icon-suppressive" class="command-icon" />
        <ui:Label text="SUPPRESSIVE" class="command-label" />
    </ui:Button>
    <ui:Button name="cmd-trap" class="command-button">
        <ui:VisualElement name="icon-trap" class="command-icon" />
        <ui:Label text="DEPLOY TRAP" class="command-label" />
    </ui:Button>
    <ui:Button name="cmd-flush" class="command-button">
        <ui:VisualElement name="icon-flush" class="command-icon" />
        <ui:Label text="DECON FLUSH" class="command-label" />
    </ui:Button>
</ui:VisualElement>
```

**C# signatures**:
```csharp
public void ShowCommands(bool[] available, float[] cooldowns);
// available[5] = which commands are usable
// cooldowns[5] = seconds remaining on cooldown
public void OnCommandClicked(int commandIndex, Action<int> callback);
public void HideCommands();
```

---

### WIDGET 8: QuestlineStageTracker
**Files**: `QuestlineStageTracker.cs`, `.uxml`, `.uss`

**Purpose**: Combined active questlines view with objectives and expedition dispatch buttons.

**UXML structure**:
```xml
<ui:VisualElement class="questline-stage-tracker">
    <ui:ScrollView name="quest-list">
        <!-- Dynamic quest entries -->
    </ui:ScrollView>
    <ui:VisualElement name="quest-detail" class="quest-detail">
        <ui:Label name="objective-text" class="objective-text" />
        <ui:VisualElement name="objective-checkboxes">
            <!-- Dynamic objective rows -->
        </ui:VisualElement>
        <ui:Button name="dispatch-button" text="DISPATCH EXPEDITION" />
    </ui:VisualElement>
</ui:VisualElement>
```

**Each quest entry**:
```xml
<ui:VisualElement class="quest-entry">
    <ui:Label name="quest-name" class="quest-name" />
    <ui:VisualElement name="stage-indicator" class="stage-indicator">
        <ui:Label name="stage-text" text="Stage 2/4" />
    </ui:VisualElement>
    <ui:ProgressBar name="objective-progress" class="objective-progress" />
</ui:VisualElement>
```

**C# signatures**:
```csharp
public void AddQuest(string questId, string title, int currentStage,
    int totalStages, string currentObjective, float objectiveProgress);
public void UpdateQuestStage(string questId, int newStage,
    string newObjective);
public void SetObjectiveProgress(string questId, float progress);
public void OnDispatchRequested(string questId);
public void RemoveQuest(string questId);
```

---

## IV. GAMEBOOTSTRAP WIRING (To Be Completed by Cursor)

Create `Assets/_Game/Core/GameBootstrap.Expansions3to4Hud.cs`:

```csharp
private void WireExpansions3to4Hud()
{
    if (_hud == null) return;

    // 1. LocationDetailPanel — wire to MapScreenUI location selection
    if (_hud.MapScreenUI != null && _hud.LocationDetailPanel != null)
    {
        Action<string> onLocationSelected = (locId) => {
            var loc = _locationCatalog?.GetById(locId);
            if (loc != null)
                _hud.LocationDetailPanel.ShowLocation(locId, loc.displayName,
                    loc.dangerLevel, loc.baseRadsPerHour, /* collapseRisk */
                    LocationEvolutionSystem?.GetLocationState(locId)?.CurrentOwner ?? "none",
                    GetLootPreview(locId));
        };
        _hud.MapScreenUI.OnLocationSelected += onLocationSelected;
        _subscriptions.Track(() => _hud.MapScreenUI.OnLocationSelected -= onLocationSelected);
    }

    // 2. ItemConditionBadge — wire to inventory item display
    // 3. QuestlineProgressTracker — wire to DynamicQuestlineSystem events
    // 4. SiegeStatusHUD — wire to HatchDefenseSystem events
    // 5. FactionIntelligencePanel — wire to FactionIntelligenceSystem events
    // 6. VehicleStatusPanel — wire to VehicleSystem events
    // 7. TacticalCommandBar — wire to HatchDefenseSystem.SiegeTactics
    // 8. QuestlineStageTracker — wire to DynamicQuestlineSystem
}
```

Add to `HUD.cs`:
```csharp
[Header("Expansions 3 & 4 — UI Elements")]
[SerializeField] private LocationDetailPanel      _locationDetailPanel;
[SerializeField] private ItemConditionBadge       _itemConditionBadge;
[SerializeField] private QuestlineProgressTracker _questlineProgressTracker;
[SerializeField] private SiegeStatusHUD           _siegeStatusHud;
[SerializeField] private FactionIntelligencePanel _factionIntelligencePanel;
[SerializeField] private VehicleStatusPanel       _vehicleStatusPanel;
[SerializeField] private TacticalCommandBar       _tacticalCommandBar;
[SerializeField] private QuestlineStageTracker    _questlineStageTracker;
```

---

## V. EDITMODE TESTS TO CREATE

| Test | Widget | What It Verifies |
|------|--------|-----------------|
| `LocationDetailPanel_DangerLevel_ShowsCorrectSkulls` | LocationDetailPanel | danger 0.8 → 4 skulls visible |
| `ItemConditionBadge_LowCondition_ShowsRedBar` | ItemConditionBadge | condition 0.2 → red fill |
| `ItemConditionBadge_Contaminated_ShowsBiohazardIcon` | ItemConditionBadge | contamination > 0 → icon visible |
| `QuestlineProgressTracker_StageComplete_ShowsCheckmark` | QuestlineProgressTracker | stage set complete → green check |
| `SiegeStatusHUD_LowIntegrity_FlashingRed` | SiegeStatusHUD | integrity < 30% → red flash CSS class |
| `FactionIntelligencePanel_StandingChange_UpdatesBar` | FactionIntelligencePanel | standing -50 → bar at 25% |
| `VehicleStatusPanel_FuelDepleted_ShowsWarning` | VehicleStatusPanel | fuel 0 → red indicator |
| `TacticalCommandBar_CooldownActive_ButtonGreyed` | TacticalCommandBar | cooldown > 0 → button disabled |
| `QuestlineStageTracker_NewQuest_AddsEntry` | QuestlineStageTracker | AddQuest → entry in list |

---

## VI. DELIVERABLE CHECKLIST

### Assets
- [ ] 27 icon SVGs generated via Canva and imported into Unity
- [ ] 5 texture PNGs generated via Canva
- [ ] Figma color palette extracted to USS variables
- [ ] Figma typography extracted to USS

### Widgets
- [ ] 8 UXML files created
- [ ] 8 USS files created (or shared `Expansions3to4.uss`)
- [ ] 8 C# widget files fully implemented
- [ ] All public Set/Show/Hide methods working

### Wiring
- [ ] `GameBootstrap.Expansions3to4Hud.cs` event wiring complete
- [ ] `HUD.cs` `[SerializeField]` fields added
- [ ] `Gameplay.unity` prefab assignments complete

### Testing
- [ ] 9 EditMode tests pass
- [ ] Widgets visible and reactive in PlayMode
- [ ] All widgets survive save/load round-trip

---

## VII. TROUBLESHOOTING

| Problem | Likely Fix |
|---------|-----------|
| Location detail panel won't open | Check `MapScreenUI.OnLocationSelected` event — verify it fires on click |
| Item badge not showing on inventory | Badge is overlay — verify z-index in USS, check parent has `overflow: visible` |
| Quest tracker stages not connecting | Connector lines use USS borders — check `.stage-connector` width |
| Siege HUD missing command buttons | Commands only show when `is_under_siege = true` — trigger siege in test |
| Faction panel tabs broken | `TabView` requires registered tab names — check registration order |
| Vehicle panel gauges stuck | Verify `ProgressBar` `high-value` matches `maxFuel`/`maxCargo` |
| Canva SVG not importing | Unity imports as Texture — change to `Sprite (2D and UI)` in import settings |
| Figma colors not matching | Color space difference (sRGB vs Linear) — use hex values directly in USS |
