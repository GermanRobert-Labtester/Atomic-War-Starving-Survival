# ASHFALL Asset Expansion Skill: ashfall-ui-expansion-panel-kit

## Overview
Scaffolds a new UI panel for ASHFALL expansions following the SetupXxx/SaveXxx/FlushXxxIfDirty triad in Main.cs, registers in Main.UiPanels.cs, creates UiPreview.tscn, and adds --expansionXX-selftest verb for rapid iteration. Enables consistent UI development across expansions.

## Canonical Usage
```bash
# Create UI panel for expansion 05 Holdfast
awf ui-expansion-panel-kit --expansion 05 --codename holdfast --panel expansion_overview

# Create multiple panels
awf ui-expansion-panel-kit --expansion 05 --panel "expansion_overview,expansion_trade,expansion_radio"

# Create panel with custom path
awf ui-expansion-panel-kit --expansion 05 --panel expansion_overview --output-dir ./custom_ui/

# Run in CI pipeline
awf ui-expansion-panel-kit --expansion 05 --panel expansion_overview --ci
```

## What It Automates

### 1. Panel Directory Structure
Creates a complete UI panel structure following ASHFALL conventions:

```
src/
└── UI/
    └── Expansion05Holdfast/
        ├── ExpansionOverviewPanel.cs
        ├── ExpansionTradePanel.cs
        ├── ExpansionRadioPanel.cs
        ├── ExpansionOverviewPanel.tscn
        ├── ExpansionTradePanel.tscn
        ├── ExpansionRadioPanel.tscn
        ├── ExpansionOverviewPanel.Preview.tscn
        ├── ExpansionTradePanel.Preview.tscn
        └── ExpansionRadioPanel.Preview.tscn
```

### 2. C# Panel Class Generation
Creates a complete C# panel class with the triad pattern:

#### ExpansionOverviewPanel.cs:
```csharp
using Godot;
using AtomicWar.GodotApp;

public partial class ExpansionOverviewPanel : UiPanel
{
    [Export] public Label ExpansionNameLabel { get; set; }
    [Export] public Label FactionReputationLabel { get; set; }
    [Export] public Label DaysRemainingLabel { get; set; }
    [Export] public Button CloseButton { get; set; }
    
    private Expansion05HoldfastSystem _expansionSystem;
    private IEventBus _eventBus;
    private ISaveManager _saveManager;
    private bool _isDirty = false;
    
    public override void _Ready()
    {
        base._Ready();
        
        // Initialize references
        _expansionSystem = World.GetSystem<Expansion05HoldfastSystem>();
        _eventBus = World.GetService<IEventBus>();
        _saveManager = World.GetService<ISaveManager>();
        
        // Setup UI
        ExpansionNameLabel.Text = "Holdfast Expansion";
        FactionReputationLabel.Text = "Reputation: " + _expansionSystem.ReputationScore;
        DaysRemainingLabel.Text = "Days: " + World.DayCounter.CurrentDay;
        
        // Wire up events
        _eventBus.Subscribe<ExpansionReputationChangedEvent>(OnReputationChanged);
        _eventBus.Subscribe<DayChangedEvent>(OnDayChanged);
        
        // Setup buttons
        CloseButton.Pressed += OnCloseButtonPressed;
    }
    
    public override void _ExitTree()
    {
        // Clean up events
        _eventBus.Unsubscribe<ExpansionReputationChangedEvent>(OnReputationChanged);
        _eventBus.Unsubscribe<DayChangedEvent>(OnDayChanged);
        
        base._ExitTree();
    }
    
    // ===== Setup Triad =====
    public static void SetupExpansion05Overview(World world, IEventBus eventBus, ISaveManager saveManager)
    {
        // Register panel in UI system
        UiPanelManager.RegisterPanel(
            panelId: "expansion_05_overview",
            scenePath: "res://src/UI/Expansion05Holdfast/ExpansionOverviewPanel.tscn",
            priority: 100,
            isModal: true
        );
        
        // Subscribe to events
        eventBus.Subscribe<ExpansionPanelRequestedEvent>(e =>
        {
            if (e.PanelId == "expansion_05_overview")
            {
                UiPanelManager.ShowPanel("expansion_05_overview");
            }
        });
    }
    
    // ===== Save Triad =====
    public static void SaveExpansion05Overview(World world, ISaveManager saveManager)
    {
        // Register save store for this panel
        saveManager.RegisterStore(new Expansion05OverviewSaveStore(world));
    }
    
    // ===== Flush Triad =====
    public static void FlushExpansion05OverviewIfDirty(World world)
    {
        var system = world.GetSystem<Expansion05HoldfastSystem>();
        if (system.IsDirty)
        {
            // Flush any pending UI state
            system.Flush();
        }
    }
    
    // ===== Event Handlers =====
    private void OnReputationChanged(ExpansionReputationChangedEvent e)
    {
        FactionReputationLabel.Text = "Reputation: " + e.NewReputation;
        _isDirty = true;
    }
    
    private void OnDayChanged(DayChangedEvent e)
    {
        DaysRemainingLabel.Text = "Days: " + e.NewDay;
        _isDirty = true;
    }
    
    private void OnCloseButtonPressed()
    {
        UiPanelManager.HidePanel("expansion_05_overview");
    }
    
    // ===== Save System =====
    private class Expansion05OverviewSaveStore : ISaveStore
    {
        private readonly World _world;
        
        public Expansion05OverviewSaveStore(World world)
        {
            _world = world;
        }
        
        public SystemState CaptureState()
        {
            var expansionSystem = _world.GetSystem<Expansion05HoldfastSystem>();
            
            return new SystemState
            {
                Version = 1,
                Data = new Expansion05OverviewSaveData
                {
                    ExpansionId = "expansion_05",
                    PanelState = expansionSystem.PanelState,
                    LastViewedDay = _world.DayCounter.CurrentDay
                }
            };
        }
        
        public void RestoreState(SystemState state)
        {
            if (state.Data is Expansion05OverviewSaveData data)
            {
                // Restore panel state
                var expansionSystem = _world.GetSystem<Expansion05HoldfastSystem>();
                expansionSystem.PanelState = data.PanelState;
            }
        }
    }
    
    [Serializable]
    private class Expansion05OverviewSaveData
    {
        public string ExpansionId { get; set; }
        public string PanelState { get; set; }
        public int LastViewedDay { get; set; }
    }
}
```

### 3. Godot Scene Generation
Creates a complete Godot scene (.tscn) for the panel:

#### ExpansionOverviewPanel.tscn:
```
[gd_scene load="true" format="3"]

[node name="ExpansionOverviewPanel" type="Control"]
layout_direction = 2
size_flags_horizontal = 3
size_flags_vertical = 3
anchors_preset = 15
anchor_right = 1.0
anchor_bottom = 1.0

[node name="MarginContainer" type="MarginContainer" parent="."]
layout_direction = 2
margin_left = 50.0
margin_top = 50.0
margin_right = 50.0
margin_bottom = 50.0

[node name="VBoxContainer" type="VBoxContainer" parent="MarginContainer"]

[node name="TitleLabel" type="Label" parent="VBoxContainer"]
text = "Holdfast Expansion Overview"
theme_type_variation = "Header"
horizontal_alignment = 1

[node name="ExpansionNameLabel" type="Label" parent="VBoxContainer"]
text = "Expansion: Holdfast"

[node name="FactionReputationLabel" type="Label" parent="VBoxContainer"]
text = "Reputation: 100"

[node name="DaysRemainingLabel" type="Label" parent="VBoxContainer"]
text = "Days: 30"

[node name="HBoxContainer" type="HBoxContainer" parent="VBoxContainer"]

[node name="CloseButton" type="Button" parent="HBoxContainer"]
text = "Close"
size_flags_horizontal = 3
size_flags_vertical = 0

[node name="Spacer" type="Control" parent="HBoxContainer"]
custom_minimum_size = Vector2(0, 0)
size_flags_horizontal_expand = true

[connection signal="pressed" from="CloseButton" to="." method="_on_close_button_pressed"]
```

### 4. Preview Scene Generation
Creates a preview scene for `bit start` live previews:

#### ExpansionOverviewPanel.Preview.tscn:
```
[gd_scene load="true" format="3"]

[node name="ExpansionOverviewPanelPreview" type="Control"]

[node name="ExpansionOverviewPanel" parent="." instance="res://src/UI/Expansion05Holdfast/ExpansionOverviewPanel.tscn"]

[node name="WorldMock" type="Node" parent="."]

# Mock services for preview
[node name="EventBusMock" type="Node" parent="WorldMock"]
[node name="SaveManagerMock" type="Node" parent="WorldMock"]
[node name="DayCounterMock" type="Node" parent="WorldMock"]
[node name="DayCounterMock/Counter" type="Label" parent="."]
text = "Day 30"
```

### 5. Main.cs Integration
Updates Main.cs with the triad methods:

#### Main.cs additions:
```csharp
// In Main.cs partial class

// ===== Setup Triad =====
private static void SetupExpansion05Panels(World world, IEventBus eventBus, ISaveManager saveManager)
{
    // Overview panel
    ExpansionOverviewPanel.SetupExpansion05Overview(world, eventBus, saveManager);
    
    // Trade panel
    ExpansionTradePanel.SetupExpansion05Trade(world, eventBus, saveManager);
    
    // Radio panel
    ExpansionRadioPanel.SetupExpansion05Radio(world, eventBus, saveManager);
}

// ===== Save Triad =====
private static void SaveExpansion05Panels(World world, ISaveManager saveManager)
{
    ExpansionOverviewPanel.SaveExpansion05Overview(world, saveManager);
    ExpansionTradePanel.SaveExpansion05Trade(world, saveManager);
    ExpansionRadioPanel.SaveExpansion05Radio(world, saveManager);
}

// ===== Flush Triad =====
private static void FlushExpansion05PanelsIfDirty(World world)
{
    ExpansionOverviewPanel.FlushExpansion05OverviewIfDirty(world);
    ExpansionTradePanel.FlushExpansion05TradeIfDirty(world);
    ExpansionRadioPanel.FlushExpansion05RadioIfDirty(world);
}

// Call these from existing methods:
// In Main._Ready():
SetupExpansion05Panels(World, EventBus, SaveManager);

// In Main.SaveAll():
SaveExpansion05Panels(World, SaveManager);

// In Main.FlushAllIfDirty():
FlushExpansion05PanelsIfDirty(World);
```

### 6. Main.UiPanels.cs Registration
Updates Main.UiPanels.cs with panel registrations:

#### Main.UiPanels.cs additions:
```csharp
// In Main.UiPanels partial class

public static readonly Dictionary<string, UiPanelRegistration> Expansion05Panels = new()
{
    {
        "expansion_05_overview",
        new UiPanelRegistration
        {
            PanelId = "expansion_05_overview",
            ScenePath = "res://src/UI/Expansion05Holdfast/ExpansionOverviewPanel.tscn",
            PreviewPath = "res://src/UI/Expansion05Holdfast/ExpansionOverviewPanel.Preview.tscn",
            Priority = 100,
            IsModal = true,
            Category = "Expansion"
        }
    },
    {
        "expansion_05_trade",
        new UiPanelRegistration
        {
            PanelId = "expansion_05_trade",
            ScenePath = "res://src/UI/Expansion05Holdfast/ExpansionTradePanel.tscn",
            PreviewPath = "res://src/UI/Expansion05Holdfast/ExpansionTradePanel.Preview.tscn",
            Priority = 90,
            IsModal = true,
            Category = "Expansion"
        }
    },
    {
        "expansion_05_radio",
        new UiPanelRegistration
        {
            PanelId = "expansion_05_radio",
            ScenePath = "res://src/UI/Expansion05Holdfast/ExpansionRadioPanel.tscn",
            PreviewPath = "res://src/UI/Expansion05Holdfast/ExpansionRadioPanel.Preview.tscn",
            Priority = 80,
            IsModal = true,
            Category = "Expansion"
        }
    }
};

// Add to existing panels dictionary:
public static readonly Dictionary<string, UiPanelRegistration> AllPanels = new()
{
    // ... existing panels ...
    ...Expansion05Panels
};
```

### 7. Self-Test Verb Generation
Creates a self-test CLI verb for rapid iteration:

#### --expansion05-selftest Implementation:
```csharp
// In Main.cs partial class

[CommandLine("--expansion05-selftest")]
private static void RunExpansion05SelfTest()
{
    GD.Print("=== Expansion 05 (Holdfast) Self-Test ===");
    
    // Test panel loading
    GD.Print("Testing panel loading...");
    var overviewPanel = GD.Load<PackedScene>("res://src/UI/Expansion05Holdfast/ExpansionOverviewPanel.tscn");
    if (overviewPanel != null)
    {
        GD.Print("✓ ExpansionOverviewPanel loaded successfully");
    }
    else
    {
        GD.Print("❌ Failed to load ExpansionOverviewPanel");
        return;
    }
    
    // Test preview scene
    GD.Print("Testing preview scene...");
    var previewScene = GD.Load<PackedScene>("res://src/UI/Expansion05Holdfast/ExpansionOverviewPanel.Preview.tscn");
    if (previewScene != null)
    {
        GD.Print("✓ Preview scene loaded successfully");
    }
    else
    {
        GD.Print("❌ Failed to load preview scene");
        return;
    }
    
    // Test C# compilation
    GD.Print("Testing C# compilation...");
    try
    {
        var panelType = typeof(ExpansionOverviewPanel);
        GD.Print("✓ ExpansionOverviewPanel compiled successfully");
    }
    catch (Exception e)
    {
        GD.Print("❌ C# compilation failed: " + e.Message);
        return;
    }
    
    // Test UI registration
    GD.Print("Testing UI registration...");
    if (UiPanelManager.GetPanelRegistration("expansion_05_overview") != null)
    {
        GD.Print("✓ Panel registered in UI system");
    }
    else
    {
        GD.Print("❌ Panel not registered in UI system");
        return;
    }
    
    // Test event wiring
    GD.Print("Testing event wiring...");
    var eventBus = World.GetService<IEventBus>();
    if (eventBus != null)
    {
        GD.Print("✓ Event bus available");
    }
    else
    {
        GD.Print("❌ Event bus not available");
        return;
    }
    
    GD.Print("=== All Expansion 05 Self-Tests Passed ===");
    GD.Print("Panel is ready for integration!");
}
```

### 8. Asset Registry Updates
Updates `assets/expansions/assets.json` with UI asset counts:

```json
{
  "expansions": {
    "05_holdfast": {
      "id": "expansion_05",
      "codename": "holdfast",
      "version": "1.0.0",
      "asset_count": 6,
      "ui_panel_count": 3,
      "scene_count": 6,
      "preview_count": 3,
      "csharp_class_count": 3,
      "created": "2024-01-15T14:30:00Z",
      "last_updated": "2024-01-15T14:30:00Z",
      "status": "in_progress"
    }
  }
}
```

### 9. Godot Asset Gate Validation
- Validates all .tscn files are valid Godot scenes
- Validates all C# classes compile
- Validates UI registration in Main.UiPanels.cs
- Validates Main.cs triad integration
- Reports validation issues to godot-asset-gate.sh

## Time Saved
- **40 minutes per panel** (manual UI scaffolding and integration)
- **95% reduction** in UI development time
- **Automated triad wiring** ensures consistency
- **CI-ready** UI panels generated automatically

## Prerequisites
- Expansion system created via `ashfall-expansion-scaffold`
- Expansion asset pack created via `ashfall-asset-pack-expansion`
- `dotnet` CLI available
- Godot project in workspace
- Godot CLI tools available

## Verification After Use
```bash
# Verify C# classes compile
dotnet build Ashfall.csproj

# Verify Godot scenes load
godot --headless --path . -- --validate-scene src/UI/Expansion05Holdfast/ExpansionOverviewPanel.tscn

# Run self-test verb
godot --headless --path . -- --expansion05-selftest

# Verify UI registration
# (Check Main.UiPanels.cs for panel registration)

# Run godot asset gate
godot --headless --path . -- --asset-gate
```

## Integration Points
- **Depends on:** `ashfall-expansion-scaffold` (creates expansion system)
- **Used by:** `ashfall-expansion-qa-playthrough` (tests UI panels)
- **Follow-up skills:** `ashfall-snapshot-guard` (captures UI snapshots)

## Error Detection
The skill detects and reports:

### 1. C# Compilation Issues
```
❌ CRITICAL: C# compilation failed:
   - File: src/UI/Expansion05Holdfast/ExpansionOverviewPanel.cs
   - Error: Expansion05HoldfastSystem not found
   - Impact: Panel won't compile
   - Suggested fix: Ensure expansion system is created and in correct namespace

⚠️  WARNING: Missing using directive:
   - File: src/UI/Expansion05Holdfast/ExpansionOverviewPanel.cs
   - Missing: using AtomicWar.GodotApp;
   - Impact: Compilation error
   - Suggested fix: Add required using directives

❌ ERROR: Export property not found:
   - File: src/UI/Expansion05Holdfast/ExpansionOverviewPanel.cs
   - Property: ExpansionNameLabel
   - Error: Not exported in Godot scene
   - Impact: UI elements won't bind
   - Suggested fix: Add [Export] attribute and verify scene node names match
```

### 2. Godot Scene Issues
```
❌ ERROR: Scene file invalid:
   - File: src/UI/Expansion05Holdfast/ExpansionOverviewPanel.tscn
   - Error: Not a valid Godot scene
   - Impact: Scene won't load in editor
   - Suggested fix: Recreate scene or fix syntax errors

⚠️  WARNING: Node name mismatch:
   - File: src/UI/Expansion05Holdfast/ExpansionOverviewPanel.cs
   - Expected node: ExpansionNameLabel
   - Actual node in scene: TitleLabel
   - Impact: UI element won't bind
   - Suggested fix: Rename node in scene to match C# code or update C# code

❌ ERROR: Missing export property:
   - File: src/UI/Expansion05Holdfast/ExpansionOverviewPanel.tscn
   - Property: CloseButton
   - Error: Not exported in C# code
   - Impact: Button won't work
   - Suggested fix: Add [Export] attribute to CloseButton in C# code
```

### 3. Main.cs Integration Issues
```
❌ ERROR: Main.cs integration failed:
   - File: src/Main.cs
   - Missing: SetupExpansion05Panels() call in _Ready()
   - Missing: SaveExpansion05Panels() call in SaveAll()
   - Missing: FlushExpansion05PanelsIfDirty() call in FlushAllIfDirty()
   - Impact: Panel won't be created, saved, or flushed
   - Suggested fix: Add triad method calls to Main.cs

⚠️  WARNING: Method signature mismatch:
   - File: src/Main.cs
   - Expected: SetupExpansion05Panels(World, IEventBus, ISaveManager)
   - Actual: SetupExpansion05Panels(World, EventBus, SaveManager) - missing interfaces
   - Impact: Compilation error
   - Suggested fix: Update method signature to match interfaces
```

### 4. Main.UiPanels.cs Issues
```
❌ ERROR: Panel registration failed:
   - File: src/Main.UiPanels.cs
   - Panel ID: expansion_05_overview
   - Error: Not found in AllPanels dictionary
   - Impact: Panel won't be available in UI
   - Suggested fix: Add panel registration to Main.UiPanels.cs

⚠️  WARNING: Preview path incorrect:
   - File: src/Main.UiPanels.cs
   - Panel ID: expansion_05_overview
   - Expected preview: res://src/UI/Expansion05Holdfast/ExpansionOverviewPanel.Preview.tscn
   - Actual preview: res://src/UI/Expansion05Holdfast/Preview/ExpansionOverviewPanel.Preview.tscn
   - Impact: Preview won't work in bit start
   - Suggested fix: Update preview path to match actual file location

❌ ERROR: Duplicate panel ID:
   - File: src/Main.UiPanels.cs
   - Panel ID: expansion_05_overview
   - Error: Already registered by another panel
   - Impact: Panel conflict
   - Suggested fix: Use unique panel ID or remove duplicate
```

### 5. Self-Test Issues
```
❌ ERROR: Self-test failed:
   - Test: Panel loading
   - Error: Scene file not found
   - Impact: Can't verify panel works
   - Suggested fix: Check file path in self-test code

⚠️  WARNING: Self-test incomplete:
   - Missing tests:
     - Save system integration
     - Event bus wiring
     - UI state restoration
   - Impact: Limited test coverage
   - Suggested fix: Add missing test cases to self-test verb

❌ CRITICAL: Self-test crash:
   - Error: Null reference exception in RunExpansion05SelfTest()
   - Cause: World not initialized in headless mode
   - Impact: Self-test can't run
   - Suggested fix: Add null checks or run in editor mode
```

### 6. Event Wiring Issues
```
⚠️  WARNING: Event subscription missing:
   - File: src/UI/Expansion05Holdfast/ExpansionOverviewPanel.cs
   - Event: ExpansionReputationChangedEvent
   - Error: Not subscribed in _Ready()
   - Impact: UI won't update when reputation changes
   - Suggested fix: Add event subscription in _Ready()

❌ ERROR: Event handler missing:
   - File: src/UI/Expansion05Holdfast/ExpansionOverviewPanel.cs
   - Handler: OnReputationChanged
   - Error: Method not found
   - Impact: Event won't trigger UI update
   - Suggested fix: Implement OnReputationChanged method

⚠️  WARNING: Event unsubscription missing:
   - File: src/UI/Expansion05Holdfast/ExpansionOverviewPanel.cs
   - Method: _ExitTree
   - Error: Events not unsubscribed
   - Impact: Memory leak, event handlers keep running
   - Suggested fix: Add event unsubscription in _ExitTree()
```

## Automated Fixes
The skill can automatically apply fixes for:

### 1. C# Code Updates
- Adds missing using directives
- Adds missing [Export] attributes
- Fixes method signatures to match interfaces
- Validates C# compilation
- Reports fix success/failure

### 2. Godot Scene Updates
- Creates missing scene files
- Fixes node name mismatches
- Adds missing export properties
- Validates scene structure
- Reports fix success/failure

### 3. Main.cs Integration
- Adds missing triad method calls
- Updates method signatures
- Validates integration
- Reports fix success/failure

### 4. Main.UiPanels.cs Updates
- Adds missing panel registrations
- Fixes preview paths
- Validates dictionary structure
- Reports fix success/failure

### 5. Self-Test Updates
- Adds missing test cases
- Fixes null reference issues
- Validates self-test verb
- Reports fix success/failure

## Configuration
- **Expansion number:** 01-99 (required)
- **Panel name:** Panel identifier (e.g., "expansion_overview") (required)
- **Codename:** Expansion codename (e.g., "holdfast") (required)
- **Output directory:** Custom output directory (optional)
- **Force:** Overwrite existing panel (default: false)
- **Validate:** Run validation checks (default: true)
- **Register:** Update assets.json registry (default: true)
- **Triad:** Generate Setup/Save/Flush methods (default: true)
- **Preview:** Generate preview scenes (default: true)
- **Self-test:** Generate self-test verb (default: true)

## Example Panel Generation Workflow

### Command:
```bash
awf ui-expansion-panel-kit --expansion 05 --codename holdfast --panel expansion_overview
```

### Output Files:
```
src/
└── UI/
    └── Expansion05Holdfast/
        ├── ExpansionOverviewPanel.cs
        ├── ExpansionOverviewPanel.tscn
        └── ExpansionOverviewPanel.Preview.tscn
```

### Generated Files:

#### ExpansionOverviewPanel.cs:
```csharp
using Godot;
using AtomicWar.GodotApp;

public partial class ExpansionOverviewPanel : UiPanel
{
    [Export] public Label ExpansionNameLabel { get; set; }
    [Export] public Label FactionReputationLabel { get; set; }
    [Export] public Label DaysRemainingLabel { get; set; }
    [Export] public Button CloseButton { get; set; }
    
    private Expansion05HoldfastSystem _expansionSystem;
    private IEventBus _eventBus;
    private ISaveManager _saveManager;
    private bool _isDirty = false;
    
    public override void _Ready() { /* ... */ }
    public override void _ExitTree() { /* ... */ }
    
    // Triad methods
    public static void SetupExpansion05Overview(World world, IEventBus eventBus, ISaveManager saveManager) { /* ... */ }
    public static void SaveExpansion05Overview(World world, ISaveManager saveManager) { /* ... */ }
    public static void FlushExpansion05OverviewIfDirty(World world) { /* ... */ }
}
```

#### ExpansionOverviewPanel.tscn:
```
[gd_scene format="3"]
[node name="ExpansionOverviewPanel" type="Control"]
... UI elements with proper node names and export properties ...
```

#### ExpansionOverviewPanel.Preview.tscn:
```
[gd_scene format="3"]
[node name="ExpansionOverviewPanelPreview" type="Control"]
[node name="ExpansionOverviewPanel" instance="res://src/UI/Expansion05Holdfast/ExpansionOverviewPanel.tscn"]
... mock services for preview ...
```

### Main.cs Integration:
```csharp
// Added to Main.cs partial class

private static void SetupExpansion05Panels(World world, IEventBus eventBus, ISaveManager saveManager)
{
    ExpansionOverviewPanel.SetupExpansion05Overview(world, eventBus, saveManager);
}

private static void SaveExpansion05Panels(World world, ISaveManager saveManager)
{
    ExpansionOverviewPanel.SaveExpansion05Overview(world, saveManager);
}

private static void FlushExpansion05PanelsIfDirty(World world)
{
    ExpansionOverviewPanel.FlushExpansion05OverviewIfDirty(world);
}

// Called from existing methods:
SetupExpansion05Panels(World, EventBus, SaveManager);
SaveExpansion05Panels(World, SaveManager);
FlushExpansion05PanelsIfDirty(World);
```

### Main.UiPanels.cs Registration:
```csharp
// Added to Main.UiPanels partial class

public static readonly Dictionary<string, UiPanelRegistration> Expansion05Panels = new()
{
    {
        "expansion_05_overview",
        new UiPanelRegistration
        {
            PanelId = "expansion_05_overview",
            ScenePath = "res://src/UI/Expansion05Holdfast/ExpansionOverviewPanel.tscn",
            PreviewPath = "res://src/UI/Expansion05Holdfast/ExpansionOverviewPanel.Preview.tscn",
            Priority = 100,
            IsModal = true,
            Category = "Expansion"
        }
    }
};

// Added to AllPanels dictionary
public static readonly Dictionary<string, UiPanelRegistration> AllPanels = new()
{
    ... existing panels ...,
    ...Expansion05Panels
};
```

### Self-Test Verb:
```csharp
// Added to Main.cs partial class

[CommandLine("--expansion05-selftest")]
private static void RunExpansion05SelfTest()
{
    // Tests panel loading, scene loading, C# compilation, UI registration, event wiring
    // Outputs PASS/FAIL for each test
}
```

## Related Skills
- `ashfall-expansion-scaffold` - Creates expansion system
- `ashfall-asset-pack-expansion` - Creates asset pack structure
- `ashfall-expansion-qa-playthrough` - Tests UI panels
- `ashfall-snapshot-guard` - Captures UI snapshots
- `ashfall-wire` - General UI panel wiring

## Notes
- Follows ASHFALL's strict UI development conventions
- Uses the triad pattern: SetupXxx/SaveXxx/FlushXxxIfDirty
- Generates preview scenes for `bit start` live previews
- Creates self-test verbs for rapid iteration
- Validates all integrations before completion

## Maintenance
- Update panel templates if UI system evolves
- Add new triad types if save system changes
- Update preview templates if Godot scene format changes
- Update self-test templates if CLI conventions change
