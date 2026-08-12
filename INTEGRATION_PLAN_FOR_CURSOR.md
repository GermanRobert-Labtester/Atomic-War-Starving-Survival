# ASHFALL — UI & ASSET INTEGRATION PLAN FOR CURSOR

> **Target**: Cursor IDE with Figma MCP (Edu Pro) + Canva Business MCP
> **Goal**: Implement 8 HUD widgets + all supporting UXML/USS + GameBootstrap wiring + prefab assignments
> **Handoff from**: Pi coding agent (all C# systems ✅, all JSON data ✅, UI stubs ⚠)

---

## I. TOOLCHAIN SETUP

### Figma MCP Connection (Edu Pro)
```
1. Open Cursor Settings → MCP → Add MCP Server
2. Configure Figma MCP:
   - Endpoint: https://api.figma.com/mcp
   - Auth token: [your Figma Edu Pro personal access token]
   - Permissions: file:read, file:write, dev_resources:read
3. Reference Figma file: [Insert Figma design file URL for ASHFALL HUD]
4. Use Figma MCP to:
   - Export color palettes → USS variables
   - Export icon SVGs → UI Toolkit VectorImage assets
   - Inspect component spacing → USS dimensions
```

### Canva Business MCP Connection
```
1. Open Cursor Settings → MCP → Add MCP Server
2. Configure Canva MCP:
   - Endpoint: https://api.canva.com/mcp
   - Auth token: [your Canva Business API key]
3. Use Canva MCP to:
   - Generate / edit background textures (memorial wall stone, vignette gradients)
   - Create icon assets (eye icon, shield, heart, pill badge)
   - Export at correct resolutions for UI Toolkit (scale 1x = 100dpi)
```

---

## II. 8 WIDGETS — SPECIFICATIONS

### Widget 1: RadiationPhaseIndicator
**File paths**:
- `Assets/_Game/UI/RadiationPhaseIndicator.cs`
- `Assets/_Game/UI/RadiationPhaseIndicator.uxml`
- `Assets/_Game/UI/RadiationPhaseIndicator.uss`

**Visual spec**:
| Phase | Color | Animation | Icon |
|-------|-------|-----------|------|
| Healthy | `#4CAF50` green | none | `○` circle |
| Prodromal | `#FFC107` amber | Pulse 1s cycle | `◉` filled circle pulsing |
| Latent | `#4CAF50` green | none (deceptive!) | `○?` circle with question |
| ManifestIllness | `#F44336` red | Flash 0.5s | `⬤` filled circle flashing |
| ChronicFibrosis | `#9E9E9E` grey | none | `◉` permanent grey |
| RecoveryOrDeath | `#2196F3` blue | none | `○` recovery blue |

**Data**: `Survivor.SicknessPhase` enum. Subscribe to `RadiationPhaseProgression.OnPhaseChanged`.
**Tooltip**: Call `RadiationPhaseProgression.GetPhasePrognosisText(survivor)` on hover.

**UXML structure**:
```xml
<ui:VisualElement class="radiation-phase-indicator">
    <ui:VisualElement name="phase-dot" class="phase-dot" />
    <ui:Label name="phase-tooltip" class="phase-tooltip" />
</ui:VisualElement>
```

**C# method signatures to implement**:
```csharp
public void SetPhase(string survivorId, RadiationSicknessPhase phase);
public void SetTooltipText(string text);
```

---

### Widget 2: PhantomMemoryVignette
**File paths**:
- `Assets/_Game/UI/PhantomMemoryVignette.cs`
- `Assets/_Game/UI/PhantomMemoryVignette.uxml`
- `Assets/_Game/UI/PhantomMemoryVignette.uss`

**Visual spec**:
- Full-screen overlay, `pointer-events: none`
- **Motivation trigger**: Radial gradient from edges — warm sepia `rgba(180, 120, 60, 0.4)` → transparent center
- **Breakdown trigger**: Radial gradient — cold blue-grey `rgba(60, 80, 120, 0.5)` → transparent center
- **Text**: Centered, 24px Barlow Condensed, fade in 0.3s → hold 2.5s → fade out 0.5s
- **Text color**: Motivation = `#FFD54F` gold, Breakdown = `#90CAF9` pale blue

**Data**: `PhantomMemorySystem.OnPhantomTrigger` (Action\<Survivor, string itemId, bool isMotivation\>)
**Text content**: Look up `phantom_triggers.json` by background + item category → `motivation_text` or `breakdown_text`

**UXML structure**:
```xml
<ui:VisualElement class="phantom-memory-vignette" picking-mode="Ignore">
    <ui:VisualElement name="vignette-gradient" class="vignette-gradient" />
    <ui:Label name="memory-text" class="memory-text" />
</ui:VisualElement>
```

**C# method signatures**:
```csharp
public void Trigger(string survivorName, string itemId, bool isMotivation);
private IEnumerator PlayVignetteAnimation(string text, Color gradientColor, Color textColor);
```

---

### Widget 3: HypervigilanceIndicator
**File paths**:
- `Assets/_Game/UI/HypervigilanceIndicator.cs`
- `Assets/_Game/UI/HypervigilanceIndicator.uxml`
- `Assets/_Game/UI/HypervigilanceIndicator.uss`

**Visual spec**:
- 12×12px badge icon positioned top-right of SurvivorPortraitCard
- **Level < 0.3**: Hidden (`display: none`)
- **Level 0.3–0.6**: Yellow eye icon `#FFC107` with subtle pulse
- **Level > 0.6**: Red eye icon `#F44336` with fast pulse
- **False alarm**: Full-screen red flash overlay 0.5s + icon shake animation

**Data**: `Survivor.HypervigilanceLevel`. Subscribe to `CombatTraumaSystem.OnHypervigilanceIncreased` and `OnFalseAlarmTriggered`.

**UXML structure**:
```xml
<ui:VisualElement class="hypervigilance-indicator">
    <ui:VisualElement name="eye-icon" class="eye-icon" />
</ui:VisualElement>
```

**C# method signatures**:
```csharp
public void UpdateLevel(string survivorId, float level);
public void TriggerFalseAlarm(string survivorId);
```

---

### Widget 4: MoralBranchDisplay
**File paths**:
- `Assets/_Game/UI/MoralBranchDisplay.cs`
- `Assets/_Game/UI/MoralBranchDisplay.uxml`
- `Assets/_Game/UI/MoralBranchDisplay.uss`

**Visual spec**:
- Positioned below survivor name on SurvivorPortraitCard
- **Neutral**: Hidden
- **NumbedResilience**: Grey steel shield icon `#9E9E9E` + "Numbed" label
- **BurdenedCompassion**: Blue heart icon `#42A5F5` + "Compassionate" label
- **Icons**: 16×16px SVG. Use Canva to generate shield and heart icons.

**Data**: `Survivor.BranchDirection`. Subscribe to `MoralBranchingSystem.OnBranchDecided`.

**UXML structure**:
```xml
<ui:VisualElement class="moral-branch-display">
    <ui:VisualElement name="branch-icon" class="branch-icon" />
    <ui:Label name="branch-label" class="branch-label" />
</ui:VisualElement>
```

**C# method signatures**:
```csharp
public void SetBranch(string survivorId, MoralBranchDirection direction);
```

---

### Widget 5: KeepsakeSlotUI
**File paths**:
- `Assets/_Game/UI/KeepsakeSlotUI.cs`
- `Assets/_Game/UI/KeepsakeSlotUI.uxml`
- `Assets/_Game/UI/KeepsakeSlotUI.uss`

**Visual spec**:
- 48×48px bordered slot with gold trim `#FFD54F` on SurvivorPortraitCard detail panel
- **Normal**: Item icon inside slot, gold border
- **Lost**: Greyed-out icon, cracked-glass overlay texture (generate via Canva), red vignette at `KeepsakeGriefLevel * 100%` opacity
- **Hover tooltip**: "Personal Keepsake: [item display name]" or "Lost Keepsake — Grief Level: [X]%"

**Data**: `Survivor.PersonalKeepsakeItemId`, `Survivor.HasLostKeepsake`, `Survivor.KeepsakeGriefLevel`.

**UXML structure**:
```xml
<ui:VisualElement class="keepsake-slot">
    <ui:VisualElement name="slot-border" class="slot-border" />
    <ui:VisualElement name="item-icon" class="item-icon" />
    <ui:VisualElement name="cracked-overlay" class="cracked-overlay" />
    <ui:Label name="grief-vignette" class="grief-vignette" />
</ui:VisualElement>
```

**C# method signatures**:
```csharp
public void SetKeepsake(string itemId, bool isLost, float griefLevel);
public void ClearKeepsake();
```

---

### Widget 6: MemorialWallUI
**File paths**:
- `Assets/_Game/UI/MemorialWallUI.cs`
- `Assets/_Game/UI/MemorialWallUI.uxml`
- `Assets/_Game/UI/MemorialWallUI.uss`

**Visual spec**:
- Full-screen modal with dark stone texture background
- **Generate stone texture via Canva**: Dark slate `#2C2C2C` with subtle engraving marks
- **Entries**: Engraved-style text in `#BDBDBD` on dark background, using Barlow Condensed
- Each entry: Name, faction (if from dog tags), date of death, cause
- Separator line between entries (etched line)
- **"Pay Respects" button**: Bottom-right, amber `#FFC107`, triggers 1h action
- **Empty state**: "The wall is bare. No one has been remembered yet." in italic
- **Comfort buff active**: Subtle warm glow `#FFD54F` at 10% opacity over whole wall

**Data**: Dog tag items in inventory + dead survivor list. Subscribe to `MemorialWallSystem.OnMemorialEntryAdded`.

**UXML structure**:
```xml
<ui:VisualElement class="memorial-wall">
    <ui:VisualElement name="wall-background" class="wall-background" />
    <ui:ScrollView name="entries-scroll" class="entries-scroll">
        <!-- Dynamic memorial entries -->
    </ui:ScrollView>
    <ui:Button name="pay-respects-button" class="pay-respects-button" text="Pay Respects" />
    <ui:Label name="empty-state" class="empty-state" />
</ui:VisualElement>
```

**C# method signatures**:
```csharp
public void Show();
public void Hide();
public void AddEntry(MemorialEntry entry);
public void SetComfortBuffActive(bool active);
public event Action<string> OnPayRespectsRequested; // survivorId
```

---

### Widget 7: TerminalPrognosisBanner
**File paths**:
- `Assets/_Game/UI/TerminalPrognosisBanner.cs`
- `Assets/_Game/UI/TerminalPrognosisBanner.uxml`
- `Assets/_Game/UI/TerminalPrognosisBanner.uss`

**Visual spec**:
- 24px-tall horizontal banner bar at top of SurvivorPortraitCard
- **Hidden**: No terminal prognosis
- **Active countdown**: Amber `#FFC107` background with pulsing border, text: "[X] days remaining — [wish title]"
- **Wish completed**: Gold `#FFD54F` background, text: "Their wish was fulfilled"
- **Wish failed**: Grey `#757575` background, text: "Time ran out"

**Data**: `Survivor.HasTerminalPrognosis`, `Survivor.TerminalPrognosisDaysRemaining`, `Survivor.FinalWishCompleted`.
Subscribe to `FinalWishSystem.OnTerminalPrognosisDeclared`, `OnFinalWishCompleted`, `OnFinalWishFailed`.

**UXML structure**:
```xml
<ui:VisualElement class="terminal-prognosis-banner">
    <ui:Label name="prognosis-text" class="prognosis-text" />
</ui:VisualElement>
```

**C# method signatures**:
```csharp
public void Show(string survivorId, float daysRemaining, string wishTitle);
public void MarkWishCompleted(string survivorId);
public void MarkWishFailed(string survivorId);
public void Hide();
```

---

### Widget 8: AddictionDetoxIndicator
**File paths**:
- `Assets/_Game/UI/AddictionDetoxIndicator.cs`
- `Assets/_Game/UI/AddictionDetoxIndicator.uxml`
- `Assets/_Game/UI/AddictionDetoxIndicator.uss`

**Visual spec**:
- 14×14px pill-shaped badge on SurvivorPortraitCard (similar to BloodTypeIndicator pattern)
- **Clean**: Hidden
- **Dependent**: Orange pill icon `#FF9800` — static
- **Cold-turkey withdrawal**: Red shaking pill `#F44336` — CSS shake animation
- **Managed detox**: Yellow hourglass `#FFC107` — slow pulse animation
- **Recovered**: Green checkmark `#4CAF50` — fade out after 24h

**CSS shake animation** (add to USS):
```css
@keyframes shake {
    0%, 100% { transform: translate(0, 0); }
    25% { transform: translate(-2px, 0); }
    75% { transform: translate(2px, 0); }
}
.shake { animation: shake 0.3s infinite; }
```

**Data**: `Survivor.ChemicalDependencies`, `Survivor.IsInWithdrawal`.
Subscribe to `ChemicalDependencySystem.OnDependencyFormed`, `OnWithdrawalStarted`, `OnDetoxCompleted`.

**UXML structure**:
```xml
<ui:VisualElement class="addiction-detox-indicator">
    <ui:VisualElement name="detox-icon" class="detox-icon" />
</ui:VisualElement>
```

**C# method signatures**:
```csharp
public void ShowDependency(string survivorId, string itemId, string status);
public void ShowWithdrawal(string survivorId);
public void ShowDetoxProgress(string survivorId, float progress);
public void Hide(string survivorId);
```

---

## III. CANVA BUSINESS — ASSET GENERATION LIST

Use Canva MCP to generate these assets at production resolution:

| Asset | Description | Size | Format | Used By |
|-------|-------------|------|--------|---------|
| `stone_wall_bg` | Dark slate texture with subtle engraving marks | 1024×1024 | PNG | MemorialWallUI |
| `cracked_glass_overlay` | Cracked glass effect, transparent background | 128×128 | PNG | KeepsakeSlotUI |
| `vignette_sepia` | Radial gradient: sepia edge → transparent center | 512×512 | PNG | PhantomMemoryVignette |
| `vignette_blue` | Radial gradient: cold blue edge → transparent center | 512×512 | PNG | PhantomMemoryVignette |
| `icon_eye` | Minimalist eye icon | 24×24 | SVG | HypervigilanceIndicator |
| `icon_shield` | Steel shield icon | 32×32 | SVG | MoralBranchDisplay |
| `icon_heart` | Heart icon | 32×32 | SVG | MoralBranchDisplay |
| `icon_pill` | Pill/capsule icon | 24×24 | SVG | AddictionDetoxIndicator |
| `icon_hourglass` | Hourglass icon | 24×24 | SVG | AddictionDetoxIndicator |
| `icon_checkmark` | Green checkmark in circle | 24×24 | SVG | AddictionDetoxIndicator |
| `memorial_name_plate` | Engraved name plate template | 400×60 | PNG | MemorialWallUI |
| `badge_background` | Small badge circle background | 24×24 | PNG | HypervigilanceIndicator |

---

## IV. FIGMA MCP — DESIGN INSPECTION LIST

Use Figma MCP to read the existing `DiegeticHud.uxml` and `UIBatch20_A.uss` palette:

| Query | Purpose |
|-------|---------|
| Extract color palette | `--color-amber`, `--color-red`, `--color-green`, etc. → USS variables |
| Extract typography scale | Font sizes, weights, letter-spacing → USS |
| Extract spacing grid | Margin/padding values → USS dimensions |
| Inspect SurvivorPortraitCard layout | Understand positioning for badge attachments |
| Inspect modal pattern | MemorialWallUI follows same modal as EventModalUI |
| Inspect existing badge (BloodTypeIndicator) | Pattern for AddictionDetoxIndicator |

---

## V. GAMEBOOTSTRAP WIRING (TO BE COMPLETED BY CURSOR)

Complete the implementation in `Assets/_Game/Core/GameBootstrap.Phase11Hud.cs`.

**File already contains**: Method stubs `WirePhase11ExpansionHud()` with placeholder comments.

**What to add**:
1. Actual event subscriptions for all 8 widgets
2. `_subscriptions.Track()` calls for cleanup
3. Initial state push (paint current survivor state on wire)

**Template for each widget**:
```csharp
if (SystemName != null && _hud.WidgetName != null)
{
    Action<Survivor, ...> onEvent = (sv, ...) => _hud.WidgetName.Method(sv.Id, ...);
    SystemName.OnEvent += onEvent;
    _subscriptions.Track(() => SystemName.OnEvent -= onEvent);
}
```

**Call**: Add `WirePhase11ExpansionHud();` at the end of `WireHUD()` in `GameBootstrap.Hud.cs`.

---

## VI. HUD.CS — FIELDS TO ADD

Add in `Assets/_Game/UI/HUD.cs`:

```csharp
[Header("Phase 11 — Expansion UI Elements")]
[SerializeField] private RadiationPhaseIndicator   _radiationPhaseIndicator;
[SerializeField] private PhantomMemoryVignette     _phantomMemoryVignette;
[SerializeField] private HypervigilanceIndicator   _hypervigilanceIndicator;
[SerializeField] private MoralBranchDisplay        _moralBranchDisplay;
[SerializeField] private KeepsakeSlotUI            _keepsakeSlotUi;
[SerializeField] private MemorialWallUI            _memorialWallUi;
[SerializeField] private TerminalPrognosisBanner   _terminalPrognosisBanner;
[SerializeField] private AddictionDetoxIndicator   _addictionDetoxIndicator;

// Public accessors
public RadiationPhaseIndicator   RadiationPhaseIndicator   => _radiationPhaseIndicator;
public PhantomMemoryVignette     PhantomMemoryVignette     => _phantomMemoryVignette;
public HypervigilanceIndicator   HypervigilanceIndicator   => _hypervigilanceIndicator;
public MoralBranchDisplay        MoralBranchDisplay        => _moralBranchDisplay;
public KeepsakeSlotUI            KeepsakeSlotUi            => _keepsakeSlotUi;
public MemorialWallUI            MemorialWallUi            => _memorialWallUi;
public TerminalPrognosisBanner   TerminalPrognosisBanner   => _terminalPrognosisBanner;
public AddictionDetoxIndicator   AddictionDetoxIndicator   => _addictionDetoxIndicator;
```

---

## VII. UNITY INSPECTOR — PREFAB ASSIGNMENTS

After all C#/UXML/USS is done, open `Gameplay.unity` and:

1. Select the `HUD` GameObject
2. In the Inspector → "Phase 11 — Expansion UI Elements" section
3. Drag each widget GameObject from the hierarchy into its `[SerializeField]` slot
4. Verify all 8 slots are assigned (non-null)
5. Save scene

---

## VIII. EDITMODE TESTS TO CREATE

| Test | File |
|------|------|
| `RadiationPhaseIndicator_AllPhases_SetCorrectColor` | `RadiationPhaseIndicatorTests.cs` |
| `PhantomMemoryVignette_Trigger_FiresAnimation` | `PhantomMemoryVignetteTests.cs` |
| `HypervigilanceIndicator_LevelChange_UpdatesVisibility` | `HypervigilanceIndicatorTests.cs` |
| `MoralBranchDisplay_SetBranch_ShowsCorrectIcon` | `MoralBranchDisplayTests.cs` |
| `KeepsakeSlotUI_LostKeepsake_ShowsCrackedOverlay` | `KeepsakeSlotUITests.cs` |
| `MemorialWallUI_AddEntry_RendersNamePlate` | `MemorialWallUITests.cs` |
| `TerminalPrognosisBanner_Show_CountdownVisible` | `TerminalPrognosisBannerTests.cs` |
| `AddictionDetoxIndicator_Withdrawal_ShowsShakingIcon` | `AddictionDetoxIndicatorTests.cs` |

Each test creates the widget via `new WidgetName()`, calls `SetXxx()`, and asserts the correct internal state. No Unity scene needed.

---

## IX. CROSS-REFERENCE: SYSTEM EVENTS → UI WIDGETS

| System Event | Widget |
|-------------|--------|
| `RadiationPhaseProgression.OnPhaseChanged` | `RadiationPhaseIndicator.SetPhase()` |
| `RadiationPhaseProgression.OnLungCapacityReduced` | (tooltip update on RadiationPhaseIndicator) |
| `PhantomMemorySystem.OnPhantomTrigger` | `PhantomMemoryVignette.Trigger()` |
| `CombatTraumaSystem.OnHypervigilanceIncreased` | `HypervigilanceIndicator.UpdateLevel()` |
| `CombatTraumaSystem.OnFalseAlarmTriggered` | `HypervigilanceIndicator.TriggerFalseAlarm()` |
| `MoralBranchingSystem.OnBranchDecided` | `MoralBranchDisplay.SetBranch()` |
| `FinalWishSystem.OnTerminalPrognosisDeclared` | `TerminalPrognosisBanner.Show()` |
| `FinalWishSystem.OnFinalWishCompleted` | `TerminalPrognosisBanner.MarkWishCompleted()` |
| `FinalWishSystem.OnFinalWishFailed` | `TerminalPrognosisBanner.MarkWishFailed()` |
| `ChemicalDependencySystem.OnDependencyFormed` | `AddictionDetoxIndicator.ShowDependency()` |
| `ChemicalDependencySystem.OnWithdrawalStarted` | `AddictionDetoxIndicator.ShowWithdrawal()` |
| `ChemicalDependencySystem.OnDetoxCompleted` | `AddictionDetoxIndicator.Hide()` |
| `MemorialWallSystem.OnMemorialEntryAdded` | `MemorialWallUI.AddEntry()` |

---

## X. DELIVERABLE CHECKLIST

- [x] 8 UXML files created
- [x] 8 USS files created (or shared `Phase11Widgets.uss`) — `UIBatch11_A.uss` / `UIBatch11_B.uss` + DiegeticHud embeds
- [ ] 12 Canva assets exported to `Assets/_Game/UI/Textures/` — deferred (USS solid-color fallbacks)
- [x] Figma color palette extracted to USS variables — tokens in `UIBatch11_A.uss` / `UIBatch11_B.uss`
- [x] 8 C# widget files fully implemented (`BindDocument`, plan API aliases, Label icons)
- [x] `GameBootstrap.Phase11Hud.cs` event wiring complete (+ shared DiegeticHud document bind)
- [x] `HUD.cs` `[SerializeField]` fields added
- [x] `Gameplay.unity` prefab assignments — wired via `GameplaySceneBuilder` (regenerate scene)
- [x] 8 EditMode tests (+ BindDocument / plan-alias coverage) in `Phase11UiWidgetTests.cs`
- [ ] Widgets visible and reactive in PlayMode — blocked on full project compile (concurrent Antigravity wiring errors)
- [x] All 8 widgets survive save/load round-trip — `CaptureState` / `RestoreState` on each widget

---

## XI. TROUBLESHOOTING

| Problem | Likely Fix |
|---------|-----------|
| Widget invisible in PlayMode | Check UXML is assigned in Inspector; check `display: none` in USS |
| Event not firing | Verify `_subscriptions.Track()` is called after `+=` |
| NullReference on system property | System may be named differently in GameBootstrap (e.g. `Addiction` vs `AddictionSystem`) |
| UXML parse error | Validate XML schema; check element names match `ui:` namespace |
| Canva asset not loading | Import as `Sprite` (2D and UI) in Unity; assign to `Background` or `Image` element |
| Figma colors don't match | Check color space (sRGB vs Linear); use hex values directly |
