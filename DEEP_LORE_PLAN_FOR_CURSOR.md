# ASHFALL — DEEP LORE & CHARACTER PROGRESSION: UI & ASSET PLAN FOR CURSOR

> **Target**: Cursor IDE with Figma MCP (Edu Pro) + Canva Business MCP
> **Scope**: 3 UI widgets + 12 Canva assets + GameBootstrap wiring
> **Handoff from**: Pi (all C# systems + JSON data will be complete before UI begins)

---

## I. FIGMA MCP — DESIGN REFERENCE QUERIES

Run these queries against the ASHFALL Figma design file to extract the existing visual language:

```
1. "Get the color palette from all named color styles"
   → Extract hex values for:
     - Amber/primary: #FFC107
     - Red/danger: #F44336
     - Green/success: #4CAF50
     - Grey/disabled: #9E9E9E
     - Dark bg: #1A1A1A
     - Panel bg: #2C2C2C
     - Text primary: #E0E0E0
     - Text secondary: #9E9E9E

2. "Get the typography scale from all text styles"
   → Extract Barlow Condensed variants:
     - H1: 28px SemiBold
     - H2: 22px SemiBold
     - H3: 18px Regular
     - Body: 14px Regular
     - Small: 11px Regular
     - Mono: 12px (for data/lore entries)

3. "Get the panel/modal component dimensions and border styles"
   → Reuse for LoreCodexPanel:
     - Border radius: 8px
     - Border: 1px solid rgba(255, 193, 7, 0.3)
     - Background: rgba(28, 28, 28, 0.95)
     - Backdrop blur: if available

4. "Get the tab bar component styling"
   → Reuse for LoreCodexPanel tabs (History / Factions / Characters)
     - Active tab: amber underline, white text
     - Inactive tab: grey text, no underline
     - Tab padding/spacing

5. "Get the scroll view styling for long content lists"
   → Reuse for world history timeline and faction lore entries
     - Scrollbar: thin, amber thumb
     - Content spacing between entries
```

---

## II. CANVA BUSINESS MCP — ASSET GENERATION

### Batch 1: Lore Codex Textures (PNG)

| # | Asset Name | Description | Size | Colors |
|---|-----------|-------------|------|--------|
| 1 | `texture_parchment_bg` | Aged parchment texture for history entries | 512×512 | Sepia/brown tones, subtle edge burn |
| 2 | `texture_terminal_screen` | CRT terminal screen texture for data entries | 512×512 | Dark green-black with scan lines |
| 3 | `texture_faction_card_bg` | Dark slate card background for faction entries | 256×320 | Dark grey #2C2C2C with subtle grain |
| 4 | `texture_character_portrait_frame` | Ornate but damaged frame for survivor portraits | 128×160 | Tarnished gold #8D6E2E on dark bg |

**Canva prompt for parchment**:
> "Aged, distressed parchment texture. Sepia and brown tones. Dark burned edges fading inward. Subtle fiber texture visible. No text. Seamless edge for tiling. Post-apocalyptic aesthetic."

**Canva prompt for terminal**:
> "Dark CRT monitor screen texture. Deep green-black background with horizontal scan lines. Subtle pixel grid. Slight vignette at corners. Retro computer terminal aesthetic."

### Batch 2: Era Timeline Icons (SVG, 32×32)

| # | Asset Name | Description | Colors |
|---|-----------|-------------|--------|
| 5 | `icon_era_pre_exchange` | Peaceful city skyline before destruction | Blue-grey sky, white buildings |
| 6 | `icon_era_hour_zero` | Mushroom cloud silhouette | Red-orange on black circle |
| 7 | `icon_era_black_sky` | Dark sky with no sun, falling ash particles | Dark grey, white ash dots |
| 8 | `icon_era_ashfall` | Ruined building half-buried in grey drifts | Grey tones |

### Batch 3: Faction Relationship Icons (SVG, 16×16)

| # | Asset Name | Description | Colors |
|---|-----------|-------------|--------|
| 9 | `icon_hostile_relation` | Jagged red lightning bolt between nodes | Red #F44336 |
| 10 | `icon_neutral_relation` | Dashed grey line connector | Grey #9E9E9E |
| 11 | `icon_allied_relation` | Solid blue linked rings | Blue #42A5F5 |

### Batch 4: Character Arc Icons (SVG)

| # | Asset Name | Description | Size | Colors |
|---|-----------|-------------|------|--------|
| 12 | `icon_arc_branch_a` | Branching path arrow pointing up-right | 24×24 | Green #4CAF50 |
| 13 | `icon_arc_branch_b` | Branching path arrow pointing down-right | 24×24 | Amber #FFC107 |
| 14 | `icon_arc_complete` | Completed circle with inner star | 32×32 | Gold #FFD54F |
| 15 | `icon_arc_locked` | Locked padlock over circle | 32×32 | Grey #757575 |
| 16 | `icon_arc_active` | Pulsing circle with glow | 32×32 | White #FFFFFF with glow |

---

## III. 3 WIDGETS — COMPLETE SPECIFICATIONS

### WIDGET 1: LoreCodexPanel

**Files**: `LoreCodexPanel.cs`, `.uxml`, `.uss`

**Purpose**: Diegetic encyclopedia of everything the player has discovered — world history, faction lore, and character backgrounds. Unlocks entries as they're found through journals, radio intercepts, and exploration.

**UXML structure**:
```xml
<ui:VisualElement class="lore-codex-panel">
    <!-- Header -->
    <ui:VisualElement class="codex-header">
        <ui:Label text="SURVIVAL CODEX" class="codex-title" />
        <ui:VisualElement class="codex-tabs">
            <ui:Button name="tab-history" text="HISTORY" class="codex-tab" />
            <ui:Button name="tab-factions" text="FACTIONS" class="codex-tab" />
            <ui:Button name="tab-characters" text="SURVIVORS" class="codex-tab" />
        </ui:VisualElement>
        <ui:Button name="close-button" text="×" class="close-button" />
    </ui:VisualElement>

    <!-- HISTORY TAB -->
    <ui:VisualElement name="history-content" class="codex-content">
        <ui:VisualElement class="timeline-header">
            <ui:Label text="CHRONOLOGY OF THE FALLOUT" class="section-title" />
        </ui:VisualElement>
        <ui:ScrollView name="history-scroll" class="codex-scroll">
            <ui:VisualElement name="era-pre-exchange" class="era-section">
                <ui:VisualElement class="era-header">
                    <ui:VisualElement name="icon-pre-exchange" class="era-icon" />
                    <ui:Label text="PRE-EXCHANGE ERA" class="era-title" />
                </ui:VisualElement>
                <!-- Dynamic history entries -->
            </ui:VisualElement>
            <ui:VisualElement class="era-divider" />
            <ui:VisualElement name="era-hour-zero" class="era-section">
                <ui:VisualElement class="era-header">
                    <ui:VisualElement name="icon-hour-zero" class="era-icon" />
                    <ui:Label text="HOUR ZERO: THE EXCHANGE" class="era-title" />
                </ui:VisualElement>
            </ui:VisualElement>
            <ui:VisualElement class="era-divider" />
            <ui:VisualElement name="era-black-sky" class="era-section">
                <ui:VisualElement class="era-header">
                    <ui:VisualElement name="icon-black-sky" class="era-icon" />
                    <ui:Label text="THE BLACK SKY (MONTHS 1–6)" class="era-title" />
                </ui:VisualElement>
            </ui:VisualElement>
            <ui:VisualElement class="era-divider" />
            <ui:VisualElement name="era-ashfall" class="era-section">
                <ui:VisualElement class="era-header">
                    <ui:VisualElement name="icon-ashfall" class="era-icon" />
                    <ui:Label text="THE ASHFALL ERA (PRESENT DAY)" class="era-title" />
                </ui:VisualElement>
            </ui:VisualElement>
        </ui:ScrollView>
    </ui:VisualElement>

    <!-- FACTIONS TAB (hidden by default) -->
    <ui:VisualElement name="factions-content" class="codex-content" style="display: none;">
        <ui:ScrollView name="factions-scroll" class="codex-scroll">
            <!-- Dynamic faction entries -->
        </ui:ScrollView>
    </ui:VisualElement>

    <!-- CHARACTERS TAB (hidden by default) -->
    <ui:VisualElement name="characters-content" class="codex-content" style="display: none;">
        <ui:ScrollView name="characters-scroll" class="codex-scroll">
            <!-- Dynamic character entries -->
        </ui:ScrollView>
    </ui:VisualElement>
</ui:VisualElement>
```

**Dynamic entry template** (for history entries):
```xml
<ui:VisualElement class="history-entry">
    <ui:Label name="entry-date" class="entry-date" />
    <ui:Label name="entry-title" class="entry-title" />
    <ui:Label name="entry-body" class="entry-body" />
    <ui:VisualElement name="entry-discovery" class="entry-discovery">
        <ui:Label name="discovery-source" class="discovery-source" />
        <ui:VisualElement name="entry-locked-overlay" class="locked-overlay" />
    </ui:VisualElement>
</ui:VisualElement>
```

**Dynamic faction entry template**:
```xml
<ui:VisualElement class="faction-entry">
    <ui:VisualElement class="faction-header">
        <ui:VisualElement name="faction-emblem" class="faction-emblem" />
        <ui:Label name="faction-name" class="faction-name" />
        <ui:Label name="faction-ideology" class="faction-ideology" />
    </ui:VisualElement>
    <ui:Label name="faction-origin" class="faction-origin" />
    <ui:Label text="KEY BELIEFS" class="subsection-label" />
    <ui:VisualElement name="faction-beliefs" class="faction-beliefs">
        <!-- Dynamic belief items -->
    </ui:VisualElement>
    <ui:VisualElement class="faction-quote-box">
        <ui:Label name="faction-quote" class="faction-quote" />
    </ui:VisualElement>
    <ui:Label text="RELATIONSHIPS" class="subsection-label" />
    <ui:VisualElement name="faction-relationships" class="faction-relationships">
        <!-- Dynamic relationship badges -->
    </ui:VisualElement>
</ui:VisualElement>
```

**C# method signatures**:
```csharp
public void Show();
public void Hide();
public void SwitchTab(string tabId); // "history", "factions", "characters"

// History
public void AddHistoryEntry(string knowledgeKey, string era, string date,
    string title, string body, string discoverySource, bool isDiscovered);
public void UnlockHistoryEntry(string knowledgeKey);

// Factions
public void AddFactionEntry(string factionId, string displayName, string ideology,
    string originStory, string[] keyBeliefs, string signatureQuote,
    Dictionary<string, string> relationships, bool isDiscovered);
public void UnlockFactionEntry(string factionId);

// Characters
public void AddCharacterEntry(string survivorId, string displayName,
    string profession, string bio, string[] traits, int arcMilestone,
    string arcBranch, bool isArcComplete);
public void UpdateCharacterArc(string survivorId, int newMilestone, string branch);
```

**States per entry**:
- **Locked**: Grey overlay with "UNDISCOVERED" text, no body visible, only era icon
- **Discovered**: Full entry visible with parchment/terminal texture bg, discovery source shown
- **New**: Pulsing amber dot on tab when new entries are unlocked since last viewed

**USS styling notes**:
- `.codex-title`: 28px Barlow Condensed SemiBold, amber #FFC107
- `.era-title`: 18px Barlow Condensed SemiBold, white #FFFFFF
- `.entry-title`: 16px Barlow Condensed SemiBold, text primary
- `.entry-body`: 14px Barlow Condensed Regular, text secondary, line-height 1.6
- `.entry-date`: 11px Barlow Condensed Regular, amber #FFC107
- `.faction-quote`: 16px Barlow Condensed Regular, italic, amber #FFC107, centered
- `.locked-overlay`: full cover, rgba(0,0,0,0.7), centered "UNDISCOVERED" text
- `.era-divider`: 1px solid rgba(255,193,7,0.2), full width

---

### WIDGET 2: FactionRelationshipMap

**Files**: `FactionRelationshipMap.cs`, `.uxml`, `.uss`

**Purpose**: Visual node graph showing the 4 factions connected by colored relationship lines. Updated as player actions shift faction standings.

**UXML structure**:
```xml
<ui:VisualElement class="faction-relationship-map">
    <ui:Label text="FACTION RELATIONSHIPS" class="map-title" />
    <ui:VisualElement name="map-canvas" class="map-canvas">
        <!-- SVG relationship lines -->
        <ui:VisualElement name="relation-lines" class="relation-lines" />
        
        <!-- 4 faction nodes positioned in a diamond -->
        <!-- TOP: Iron Garrison -->
        <ui:VisualElement name="node-garrison" class="faction-node garrison-node">
            <ui:VisualElement name="garrison-emblem" class="faction-emblem-large" />
            <ui:Label text="IRON\nGARRISON" class="node-label" />
            <ui:VisualElement name="garrison-standing" class="standing-indicator" />
        </ui:VisualElement>
        
        <!-- LEFT: Ash Militia -->
        <ui:VisualElement name="node-militia" class="faction-node militia-node">
            <ui:VisualElement name="militia-emblem" class="faction-emblem-large" />
            <ui:Label text="ASH\nMILITIA" class="node-label" />
            <ui:VisualElement name="militia-standing" class="standing-indicator" />
        </ui:VisualElement>
        
        <!-- RIGHT: Cult of the Ash Sign -->
        <ui:VisualElement name="node-cult" class="faction-node cult-node">
            <ui:VisualElement name="cult-emblem" class="faction-emblem-large" />
            <ui:Label text="CULT OF\nASH SIGN" class="node-label" />
            <ui:VisualElement name="cult-standing" class="standing-indicator" />
        </ui:VisualElement>
        
        <!-- BOTTOM: Warlords of Sector 4 -->
        <ui:VisualElement name="node-warlords" class="faction-node warlords-node">
            <ui:VisualElement name="warlords-emblem" class="faction-emblem-large" />
            <ui:Label text="WARLORDS\nSECTOR 4" class="node-label" />
            <ui:VisualElement name="warlords-standing" class="standing-indicator" />
        </ui:VisualElement>
    </ui:VisualElement>
    
    <!-- Legend -->
    <ui:VisualElement class="map-legend">
        <ui:VisualElement class="legend-item">
            <ui:VisualElement class="legend-line hostile-line" />
            <ui:Label text="HOSTILE" class="legend-label" />
        </ui:VisualElement>
        <ui:VisualElement class="legend-item">
            <ui:VisualElement class="legend-line suspicious-line" />
            <ui:Label text="SUSPICIOUS" class="legend-label" />
        </ui:VisualElement>
        <ui:VisualElement class="legend-item">
            <ui:VisualElement class="legend-line neutral-line" />
            <ui:Label text="NEUTRAL" class="legend-label" />
        </ui:VisualElement>
        <ui:VisualElement class="legend-item">
            <ui:VisualElement class="legend-line allied-line" />
            <ui:Label text="ALLIED" class="legend-label" />
        </ui:VisualElement>
    </ui:VisualElement>
</ui:VisualElement>
```

**C# method signatures**:
```csharp
public void Show();
public void Hide();
public void SetFactionNodeData(string factionId, string displayName,
    float standing, string relationshipToPlayer);
public void SetRelationship(string factionA, string factionB,
    string status); // "hostile", "suspicious", "neutral", "allied"
public void PulseNode(string factionId); // highlight briefly when intel updates
```

**Relationship line colors**:
- Hostile: Red `#F44336`, solid line
- Suspicious: Orange `#FF9800`, dashed line
- Neutral: Grey `#9E9E9E`, dotted line
- Allied: Blue `#42A5F5`, solid line with glow

**Node positioning** (CSS flexbox or absolute):
- Garrison: top-center
- Militia: left-center
- Cult: right-center
- Warlords: bottom-center

**Standing indicators**:
- Green pulse if standing > 50
- Yellow if standing 0-50
- Red if standing < 0
- Grey if no contact

---

### WIDGET 3: CharacterArcProgressPanel

**Files**: `CharacterArcProgressPanel.cs`, `.uxml`, `.uss`

**Purpose**: Shows each named survivor's narrative arc as a 4-stage vertical timeline with branch points visible at the crisis stage.

**UXML structure**:
```xml
<ui:VisualElement class="character-arc-panel">
    <ui:Label name="character-name" class="character-name" />
    <ui:Label name="character-profession" class="character-profession" />
    <ui:Label name="character-bio" class="character-bio" />
    
    <ui:VisualElement class="arc-traits">
        <!-- Starting traits as badges -->
    </ui:VisualElement>
    
    <ui:VisualElement class="arc-timeline">
        <!-- Stage 1: Discovery -->
        <ui:VisualElement name="stage-1" class="arc-stage">
            <ui:VisualElement class="stage-connector" />
            <ui:VisualElement name="stage-1-circle" class="stage-circle" />
            <ui:VisualElement class="stage-info">
                <ui:Label text="STAGE 1: DISCOVERY" class="stage-title" />
                <ui:Label name="stage-1-desc" class="stage-desc" />
            </ui:VisualElement>
        </ui:VisualElement>
        
        <ui:VisualElement class="stage-connector-vertical" />
        
        <!-- Stage 2: Trigger -->
        <ui:VisualElement name="stage-2" class="arc-stage">
            <ui:VisualElement name="stage-2-circle" class="stage-circle" />
            <ui:VisualElement class="stage-info">
                <ui:Label text="STAGE 2: INVESTIGATION" class="stage-title" />
                <ui:Label name="stage-2-desc" class="stage-desc" />
            </ui:VisualElement>
        </ui:VisualElement>
        
        <ui:VisualElement class="stage-connector-vertical" />
        
        <!-- Stage 3: Crisis (branch point) -->
        <ui:VisualElement name="stage-3" class="arc-stage arc-crisis">
            <ui:VisualElement name="stage-3-circle" class="stage-circle crisis-circle" />
            <ui:VisualElement class="stage-info">
                <ui:Label text="STAGE 3: CRISIS" class="stage-title crisis-title" />
                <ui:Label name="stage-3-desc" class="stage-desc" />
            </ui:VisualElement>
            <!-- Branch indicators -->
            <ui:VisualElement class="branch-indicators">
                <ui:VisualElement name="branch-a" class="branch-option">
                    <ui:VisualElement name="branch-a-icon" class="branch-icon" />
                    <ui:Label name="branch-a-label" class="branch-label" />
                </ui:VisualElement>
                <ui:VisualElement name="branch-b" class="branch-option">
                    <ui:VisualElement name="branch-b-icon" class="branch-icon" />
                    <ui:Label name="branch-b-label" class="branch-label" />
                </ui:VisualElement>
            </ui:VisualElement>
        </ui:VisualElement>
        
        <ui:VisualElement class="stage-connector-vertical" />
        
        <!-- Stage 4: Resolution -->
        <ui:VisualElement name="stage-4" class="arc-stage">
            <ui:VisualElement name="stage-4-circle" class="stage-circle" />
            <ui:VisualElement class="stage-info">
                <ui:Label text="STAGE 4: RESOLUTION" class="stage-title" />
                <ui:Label name="stage-4-desc" class="stage-desc" />
            </ui:VisualElement>
        </ui:VisualElement>
    </ui:VisualElement>
    
    <ui:VisualElement class="arc-reward">
        <ui:Label text="ARC REWARD" class="reward-title" />
        <ui:VisualElement name="reward-trait-icon" class="reward-icon" />
        <ui:Label name="reward-trait-name" class="reward-name" />
    </ui:VisualElement>
</ui:VisualElement>
```

**C# method signatures**:
```csharp
public void ShowCharacter(string survivorId, string displayName,
    string profession, string bio, string[] startingTraits,
    int currentMilestone, string branchTaken);
public void SetStageComplete(int stageIndex, string description);
public void SetStageActive(int stageIndex);
public void SetBranchOptions(string branchALabel, string branchBLabel,
    string branchAOutcome, string branchBOutcome);
public void SetBranchChosen(string branchId);
public void SetArcComplete(string rewardTraitName, string rewardTraitIconId);
public void SetStressLevel(float stressAccumulation);
```

**Stage circle states**:
- **Locked** (future): Grey circle with padlock icon, muted text
- **Active** (current): Amber pulsing circle with glow, white text
- **Completed** (past): Green circle with checkmark, dimmed text
- **Crisis** (stage 3, active): Red pulsing circle, branch options visible

**Stage connector lines**:
- Vertical line between stages, 2px wide
- Locked section: grey `#757575`, dashed
- Active section: amber `#FFC107`, solid
- Completed section: green `#4CAF50`, solid

---

## IV. GAMEBOOTSTRAP WIRING

Create `Assets/_Game/Core/GameBootstrap.DeepLoreHud.cs`:

```csharp
private void WireDeepLoreHud()
{
    if (_hud == null) return;

    // 1. LoreCodexPanel — populate from discovered knowledge
    if (_hud.LoreCodexPanel != null && JournalSystem != null)
    {
        // When a knowledge entry is discovered, add it to the codex
        Action<JournalEntry> onEntryAdded = (entry) =>
        {
            if (entry?.KnowledgeKey == null) return;
            if (entry.KnowledgeKey.StartsWith("lore_"))
                _hud.LoreCodexPanel.UnlockHistoryEntry(entry.KnowledgeKey);
        };
        JournalSystem.OnEntryAdded += onEntryAdded;
        _subscriptions.Track(() => JournalSystem.OnEntryAdded -= onEntryAdded);
    }

    // 2. FactionRelationshipMap — update from faction standing changes
    if (_hud.FactionRelationshipMap != null && EconomySystem != null)
    {
        Action<string, float> onStandingChanged = (factionId, newStanding) =>
        {
            _hud.FactionRelationshipMap.SetFactionNodeData(
                factionId, factionId, newStanding, "neutral");
        };
        // Wire to faction economy standing change event
    }

    // 3. CharacterArcProgressPanel — update from arc milestone changes
    if (_hud.CharacterArcProgressPanel != null &&
        SurvivorNarrativeArcSystem != null)
    {
        Action<Survivor, int, string> onMilestone = (sv, milestone, branch) =>
        {
            _hud.CharacterArcProgressPanel.SetStageComplete(milestone,
                $"Milestone {milestone} completed");
            if (!string.IsNullOrEmpty(branch))
                _hud.CharacterArcProgressPanel.SetBranchChosen(branch);
        };
        SurvivorNarrativeArcSystem.OnArcMilestoneReached += onMilestone;
        _subscriptions.Track(() =>
            SurvivorNarrativeArcSystem.OnArcMilestoneReached -= onMilestone);
    }
}
```

Add to `HUD.cs`:
```csharp
[Header("Deep Lore — UI Elements")]
[SerializeField] private LoreCodexPanel           _loreCodexPanel;
[SerializeField] private FactionRelationshipMap    _factionRelationshipMap;
[SerializeField] private CharacterArcProgressPanel _characterArcProgressPanel;

public LoreCodexPanel           LoreCodexPanel           => _loreCodexPanel;
public FactionRelationshipMap    FactionRelationshipMap    => _factionRelationshipMap;
public CharacterArcProgressPanel CharacterArcProgressPanel => _characterArcProgressPanel;
```

---

## V. EDITMODE TESTS

| Test | Widget | What It Verifies |
|------|--------|-----------------|
| `LoreCodexPanel_AddHistoryEntry_RendersCorrectEra` | LoreCodexPanel | Entry with era "hour_zero" appears in correct era section |
| `LoreCodexPanel_LockedEntry_ShowsOverlay` | LoreCodexPanel | Undiscovered entry shows locked overlay |
| `LoreCodexPanel_TabSwitch_ShowsCorrectContent` | LoreCodexPanel | Switching to "factions" hides history, shows factions |
| `LoreCodexPanel_FactionEntry_ShowsQuote` | LoreCodexPanel | Faction entry renders signature quote in amber italic |
| `FactionRelationshipMap_SetRelation_ShowsCorrectLine` | FactionRelationshipMap | Hostile → red line, Allied → blue line |
| `FactionRelationshipMap_StandingUpdate_PulsesNode` | FactionRelationshipMap | Standing change → brief node highlight |
| `CharacterArcPanel_StageComplete_GreenCircle` | CharacterArcProgressPanel | Stage 1 complete → circle turns green with check |
| `CharacterArcPanel_CrisisStage_ShowsBranches` | CharacterArcProgressPanel | Stage 3 active → branch options visible |
| `CharacterArcPanel_BranchChosen_HighlightsSelection` | CharacterArcProgressPanel | Branch A chosen → branch A highlighted, B dimmed |

---

## VI. DELIVERABLE CHECKLIST

### Canva Assets
- [ ] 4 textures generated (parchment, terminal, faction card, portrait frame)
- [ ] 4 era timeline icons (24×24 SVG)
- [ ] 3 relationship line icons (16×16 SVG)
- [ ] 5 arc stage icons (24-32×32 SVG)
- [ ] All imported into Unity as Sprite (2D and UI)

### Figma Reference
- [ ] Color palette extracted to USS variables
- [ ] Typography scale extracted
- [ ] Panel/modal pattern documented
- [ ] Tab bar pattern documented

### Widgets
- [ ] 3 UXML files created
- [ ] 3 USS files created (or shared `DeepLore.uss`)
- [ ] 3 C# widget files fully implemented
- [ ] All public methods working

### Wiring
- [ ] `GameBootstrap.DeepLoreHud.cs` event wiring complete
- [ ] `HUD.cs` fields added
- [ ] `Gameplay.unity` prefab assignments
- [ ] 9 EditMode tests pass

---

## VII. TROUBLESHOOTING

| Problem | Likely Fix |
|---------|-----------|
| History entries not appearing in correct era | Check `knowledge_key` starts with "lore_" and era field matches section name |
| Faction nodes overlapping on map | Use CSS Grid or absolute positioning with percentage offsets |
| Relationship lines not rendering | SVG lines between nodes — use `transform: rotate()` for diagonal connections |
| Character arc stages not connecting | `.stage-connector-vertical` must have explicit height matching stage spacing |
| Tab switch not working | Verify tab button click handlers toggle `display: none/flex` on content sections |
| Canva parchment has visible seams | Use `background-size: cover` or generate at larger resolution |
| Figma colors appear different in Unity | Unity uses Linear color space by default — use hex values directly in USS |
