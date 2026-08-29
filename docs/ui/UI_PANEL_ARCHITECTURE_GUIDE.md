# ASHFALL Godot UI Panel Architecture & Node Binding Guide

**Authoritative UI Contract Guide** | **Generated:** 2026-08-29 | **Scene-Backed Panels:** 22

> [!IMPORTANT]
> **UI ARCHITECTURE INVARIANTS:**
> 1. **Scene-Backed Panels (22)**: Must be loaded via `PanelSceneLoader.Load<Control>("res://assets/ui/panels/<Name>.tscn")`.
> 2. **Node Contracts**: The matching C# class calls `SceneBinder.Require<T>("%UniqueName")`. Every required node MUST declare `unique_name_in_owner = true` in the `.tscn`.
> 3. **Design System**: Typography and colors must use constants from `DesignTheme` (`DesignTheme.Pale`, `DesignTheme.Green`, `FontSizeBody`, `FontSizeHeading`, `FontSizeMono`).
> 4. **Modal Protocols**: All modals (`IModalPanel`) must support `[Enter]`/`[Space]` acknowledgement and `[Escape]` dismissal without trapping keyboard navigation.

---

## Scene-Backed Panels Contract Matrix

| Panel Class | Scene Resource Path | Root Type | Declared Node Contracts | Purpose |
|---|---|---|---|---|
| `InventoryDetailPanel` | `res://assets/ui/panels/InventoryDetailPanel.tscn` | `Control` | `%Backdrop` (ColorRect), `%Info` (VBoxContainer), `%Stats` (VBoxContainer), +2 more | Item inspection, stat comparison, consumable usage, and equipment actions |
| `AfflictionsPanel` | `res://assets/ui/panels/AfflictionsPanel.tscn` | `Control` | `%Backdrop` (ColorRect), `%CurrentList` (VBoxContainer), `%HistoryList` (VBoxContainer), +2 more | Active disease tracking, trauma monitoring, and medical treatment application |
| `SurvivorDetailPanel` | `res://assets/ui/panels/SurvivorDetailPanel.tscn` | `Control` | `%Backdrop` (ColorRect), `%Vitals` (VBoxContainer), `%Skills` (VBoxContainer), +3 more | Individual survivor inspect view: hunger, thirst, radiation, skills, morale, traits |
| `WeatherDetailPanel` | `res://assets/ui/panels/WeatherDetailPanel.tscn` | `Control` | `%Backdrop` (ColorRect), `%CurrentList` (VBoxContainer), `%ForecastList` (VBoxContainer), +3 more | Atmospheric pressure, fallout forecast, storm prediction, and sonde telemetry |
| `QuestDetailPanel` | `res://assets/ui/panels/QuestDetailPanel.tscn` | `Control` | `%Backdrop` (ColorRect), `%ObjectivesList` (VBoxContainer), `%RewardsList` (VBoxContainer), +4 more | Quest branch selection, narrative dialogue, objectives, and reward claims |
| `MapDetailPanel` | `res://assets/ui/panels/MapDetailPanel.tscn` | `Control` | `%Backdrop` (ColorRect), `%NodeHeader` (Label), `%HazardMetrics` (VBoxContainer), +4 more | Wasteland map node details, expedition sortie planning, and hazard analysis |
| `RadiationDetailPanel` | `res://assets/ui/panels/RadiationDetailPanel.tscn` | `Control` | `%Backdrop` (ColorRect), `%DosimeterMeter` (TextureProgressBar), `%DoseLedgerList` (VBoxContainer), +3 more | Shelter radiation dosimeter, acute exposure history, and decontamination |
| `EconomyDetailPanel` | `res://assets/ui/panels/EconomyDetailPanel.tscn` | `Control` | `%Backdrop` (ColorRect), `%MarketGoodsGrid` (GridContainer), `%PriceTrendChart` (Control), +3 more | Regional commodity prices, barter rates, debt ledger, and merchant caravans |
| `CombatDetailPanel` | `res://assets/ui/panels/CombatDetailPanel.tscn` | `Control` | `%Backdrop` (ColorRect), `%EncounterFeed` (RichTextLabel), `%TacticalOptions` (HBoxContainer), +3 more | Tactical combat resolution, weapon wear tracking, and trauma outcomes |
| `FactionDetailPanel` | `res://assets/ui/panels/FactionDetailPanel.tscn` | `Control` | `%Backdrop` (ColorRect), `%FactionHeader` (Label), `%ReputationGauge` (ProgressBar), +4 more | Faction standings, regional pacts, tension indices, and diplomatic treaties |
| `JournalDetailPanel` | `res://assets/ui/panels/JournalDetailPanel.tscn` | `Control` | `%Backdrop` (ColorRect), `%EntryFeed` (RichTextLabel), `%BookmarkList` (VBoxContainer), +2 more | Diegetic survivor journal logs, emotional memories, and historical entries |
| `EventDetailPanel` | `res://assets/ui/panels/EventDetailPanel.tscn` | `Control` | `%Backdrop` (ColorRect), `%EventTitle` (Label), `%EventDescription` (RichTextLabel), +2 more | Shelter random incident presentation and narrative decision choices |
| `DutyRosterDetailPanel` | `res://assets/ui/panels/DutyRosterDetailPanel.tscn` | `Control` | `%Backdrop` (ColorRect), `%ShiftSlot1` (VBoxContainer), `%ShiftSlot2` (VBoxContainer), +2 more | 24-hour work shift assignment, fatigue management, and schedule overrides |
| `SurvivalDetailPanel` | `res://assets/ui/panels/SurvivalDetailPanel.tscn` | `Control` | `%Backdrop` (ColorRect), `%CaloricIntake` (Label), `%HydrationLevel` (Label), +3 more | Shelter nutrition balance, water consumption quotas, and thermal comfort |
| `WorkshopPanel` | `res://assets/ui/panels/WorkshopPanel.tscn` | `Control` | `%BenchGrid` (GridContainer), `%DisassemblyQueue` (VBoxContainer), `%BlueprintsList` (ItemList), +4 more | Reverse engineering relics, weapon repair, and blueprint fabrication |
| `CraftingPanel` | `res://assets/ui/panels/CraftingPanel.tscn` | `Control` | `%RecipeCategoryTabs` (TabBar), `%RecipeList` (ItemList), `%IngredientsContainer` (VBoxContainer), +6 more | Shelter tools, survival gear, medical supplies, and ammunition crafting |
| `KitchenNutritionPanel` | `res://assets/ui/panels/KitchenNutritionPanel.tscn` | `Control` | `%MenuSelector` (OptionButton), `%PreservationVats` (VBoxContainer), `%CookButton` (Button), +1 more | Meal preparation, nutrient fortification, and food spoilage prevention |
| `WaterTreatmentPanel` | `res://assets/ui/panels/WaterTreatmentPanel.tscn` | `Control` | `%ContaminationGauge` (ProgressBar), `%FilterBankStatus` (VBoxContainer), `%DistillationControls` (HBoxContainer), +3 more | Radiological filtration, sump water recycling, and potable water tanks |
| `PharmaLabPanel` | `res://assets/ui/panels/PharmaLabPanel.tscn` | `Control` | `%CentrifugeControls` (HBoxContainer), `%ChemicalVats` (VBoxContainer), `%SynthesisProgressBar` (ProgressBar), +2 more | Advanced pharmaceuticals, chemical dependency inhibitors, and antiradiation serums |
| `OpeningProtocolModal` | `res://assets/ui/modals/OpeningProtocolModal.tscn` | `Control` | `%Backdrop` (ColorRect), `%TitleLabel` (Label), `%ProtocolText` (RichTextLabel), +1 more | Shelter initialization sequence, starting survivor roster, and campaign seed briefing |
| `SafeCrackModal` | `res://assets/ui/modals/SafeCrackModal.tscn` | `Control` | `%DialRing` (TextureRect), `%TumblerDisplay` (HBoxContainer), `%DialLeftBtn` (Button), +3 more | Audio/visual mini-game for unlocking pre-war safes and security lockers |
| `DailyBriefingModal` | `res://assets/ui/modals/DailyBriefingModal.tscn` | `Control` | `%TitleLabel` (Label), `%BodyLabel` (RichTextLabel), `%ScrollContainer` (ScrollContainer), +3 more | Dawn transition modal: daily survivor vitals summary, weather shifts, and incident logs |

---

## Detailed Scene Binding Contracts

### InventoryDetailPanel

- **Scene Path:** `res://assets/ui/panels/InventoryDetailPanel.tscn`
- **C# Implementation:** [`src/UI/InventoryDetailPanel.cs`](../../src/UI/InventoryDetailPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Item inspection, stat comparison, consumable usage, and equipment actions

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%Backdrop`: `ColorRect`
- `%Info`: `VBoxContainer`
- `%Stats`: `VBoxContainer`
- `%Actions`: `VBoxContainer`
- `%CloseButton`: `Button`

### AfflictionsPanel

- **Scene Path:** `res://assets/ui/panels/AfflictionsPanel.tscn`
- **C# Implementation:** [`src/UI/AfflictionsPanel.cs`](../../src/UI/AfflictionsPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Active disease tracking, trauma monitoring, and medical treatment application

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%Backdrop`: `ColorRect`
- `%CurrentList`: `VBoxContainer`
- `%HistoryList`: `VBoxContainer`
- `%DetailView`: `VBoxContainer`
- `%CloseButton`: `Button`

### SurvivorDetailPanel

- **Scene Path:** `res://assets/ui/panels/SurvivorDetailPanel.tscn`
- **C# Implementation:** [`src/UI/SurvivorDetailPanel.cs`](../../src/UI/SurvivorDetailPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Individual survivor inspect view: hunger, thirst, radiation, skills, morale, traits

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%Backdrop`: `ColorRect`
- `%Vitals`: `VBoxContainer`
- `%Skills`: `VBoxContainer`
- `%Traits`: `VBoxContainer`
- `%Assignments`: `VBoxContainer`
- `%CloseButton`: `Button`

### WeatherDetailPanel

- **Scene Path:** `res://assets/ui/panels/WeatherDetailPanel.tscn`
- **C# Implementation:** [`src/UI/WeatherDetailPanel.cs`](../../src/UI/WeatherDetailPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Atmospheric pressure, fallout forecast, storm prediction, and sonde telemetry

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%Backdrop`: `ColorRect`
- `%CurrentList`: `VBoxContainer`
- `%ForecastList`: `VBoxContainer`
- `%AtmosphericReadout`: `VBoxContainer`
- `%SondeControls`: `VBoxContainer`
- `%CloseButton`: `Button`

### QuestDetailPanel

- **Scene Path:** `res://assets/ui/panels/QuestDetailPanel.tscn`
- **C# Implementation:** [`src/UI/QuestDetailPanel.cs`](../../src/UI/QuestDetailPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Quest branch selection, narrative dialogue, objectives, and reward claims

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%Backdrop`: `ColorRect`
- `%ObjectivesList`: `VBoxContainer`
- `%RewardsList`: `VBoxContainer`
- `%LoreLog`: `RichTextLabel`
- `%BranchChoiceA`: `Button`
- `%BranchChoiceB`: `Button`
- `%CloseButton`: `Button`

### MapDetailPanel

- **Scene Path:** `res://assets/ui/panels/MapDetailPanel.tscn`
- **C# Implementation:** [`src/UI/MapDetailPanel.cs`](../../src/UI/MapDetailPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Wasteland map node details, expedition sortie planning, and hazard analysis

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%Backdrop`: `ColorRect`
- `%NodeHeader`: `Label`
- `%HazardMetrics`: `VBoxContainer`
- `%ResourceEstimate`: `VBoxContainer`
- `%ExpeditionDispatchBtn`: `Button`
- `%ScoutButton`: `Button`
- `%CloseButton`: `Button`

### RadiationDetailPanel

- **Scene Path:** `res://assets/ui/panels/RadiationDetailPanel.tscn`
- **C# Implementation:** [`src/UI/RadiationDetailPanel.cs`](../../src/UI/RadiationDetailPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Shelter radiation dosimeter, acute exposure history, and decontamination

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%Backdrop`: `ColorRect`
- `%DosimeterMeter`: `TextureProgressBar`
- `%DoseLedgerList`: `VBoxContainer`
- `%DeconProtocolBtn`: `Button`
- `%ShieldingStatus`: `VBoxContainer`
- `%CloseButton`: `Button`

### EconomyDetailPanel

- **Scene Path:** `res://assets/ui/panels/EconomyDetailPanel.tscn`
- **C# Implementation:** [`src/UI/EconomyDetailPanel.cs`](../../src/UI/EconomyDetailPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Regional commodity prices, barter rates, debt ledger, and merchant caravans

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%Backdrop`: `ColorRect`
- `%MarketGoodsGrid`: `GridContainer`
- `%PriceTrendChart`: `Control`
- `%BarterLedger`: `VBoxContainer`
- `%CaravanTimer`: `Label`
- `%CloseButton`: `Button`

### CombatDetailPanel

- **Scene Path:** `res://assets/ui/panels/CombatDetailPanel.tscn`
- **C# Implementation:** [`src/UI/CombatDetailPanel.cs`](../../src/UI/CombatDetailPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Tactical combat resolution, weapon wear tracking, and trauma outcomes

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%Backdrop`: `ColorRect`
- `%EncounterFeed`: `RichTextLabel`
- `%TacticalOptions`: `HBoxContainer`
- `%WeaponConditionGauge`: `ProgressBar`
- `%FleeButton`: `Button`
- `%CloseButton`: `Button`

### FactionDetailPanel

- **Scene Path:** `res://assets/ui/panels/FactionDetailPanel.tscn`
- **C# Implementation:** [`src/UI/FactionDetailPanel.cs`](../../src/UI/FactionDetailPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Faction standings, regional pacts, tension indices, and diplomatic treaties

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%Backdrop`: `ColorRect`
- `%FactionHeader`: `Label`
- `%ReputationGauge`: `ProgressBar`
- `%TreatyClausesList`: `VBoxContainer`
- `%WarStatusIndicator`: `TextureRect`
- `%TributeButton`: `Button`
- `%CloseButton`: `Button`

### JournalDetailPanel

- **Scene Path:** `res://assets/ui/panels/JournalDetailPanel.tscn`
- **C# Implementation:** [`src/UI/JournalDetailPanel.cs`](../../src/UI/JournalDetailPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Diegetic survivor journal logs, emotional memories, and historical entries

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%Backdrop`: `ColorRect`
- `%EntryFeed`: `RichTextLabel`
- `%BookmarkList`: `VBoxContainer`
- `%SurvivorVoiceTag`: `Label`
- `%CloseButton`: `Button`

### EventDetailPanel

- **Scene Path:** `res://assets/ui/panels/EventDetailPanel.tscn`
- **C# Implementation:** [`src/UI/EventDetailPanel.cs`](../../src/UI/EventDetailPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Shelter random incident presentation and narrative decision choices

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%Backdrop`: `ColorRect`
- `%EventTitle`: `Label`
- `%EventDescription`: `RichTextLabel`
- `%ChoiceOptionsList`: `VBoxContainer`
- `%CloseButton`: `Button`

### DutyRosterDetailPanel

- **Scene Path:** `res://assets/ui/panels/DutyRosterDetailPanel.tscn`
- **C# Implementation:** [`src/UI/DutyRosterDetailPanel.cs`](../../src/UI/DutyRosterDetailPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** 24-hour work shift assignment, fatigue management, and schedule overrides

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%Backdrop`: `ColorRect`
- `%ShiftSlot1`: `VBoxContainer`
- `%ShiftSlot2`: `VBoxContainer`
- `%ShiftSlot3`: `VBoxContainer`
- `%CloseButton`: `Button`

### SurvivalDetailPanel

- **Scene Path:** `res://assets/ui/panels/SurvivalDetailPanel.tscn`
- **C# Implementation:** [`src/UI/SurvivalDetailPanel.cs`](../../src/UI/SurvivalDetailPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Shelter nutrition balance, water consumption quotas, and thermal comfort

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%Backdrop`: `ColorRect`
- `%CaloricIntake`: `Label`
- `%HydrationLevel`: `Label`
- `%ThermalBalance`: `Label`
- `%RationRoster`: `VBoxContainer`
- `%CloseButton`: `Button`

### WorkshopPanel

- **Scene Path:** `res://assets/ui/panels/WorkshopPanel.tscn`
- **C# Implementation:** [`src/UI/WorkshopPanel.cs`](../../src/UI/WorkshopPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Reverse engineering relics, weapon repair, and blueprint fabrication

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%BenchGrid`: `GridContainer`
- `%DisassemblyQueue`: `VBoxContainer`
- `%BlueprintsList`: `ItemList`
- `%ScrapYieldReadout`: `Label`
- `%DismantleButton`: `Button`
- `%RepairButton`: `Button`
- `%CloseButton`: `Button`

### CraftingPanel

- **Scene Path:** `res://assets/ui/panels/CraftingPanel.tscn`
- **C# Implementation:** [`src/UI/CraftingPanel.cs`](../../src/UI/CraftingPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Shelter tools, survival gear, medical supplies, and ammunition crafting

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%RecipeCategoryTabs`: `TabBar`
- `%RecipeList`: `ItemList`
- `%IngredientsContainer`: `VBoxContainer`
- `%OutputPreview`: `TextureRect`
- `%CraftAmountSpinBox`: `SpinBox`
- `%CraftButton`: `Button`
- `%BatchCraftButton`: `Button`
- `%QueueList`: `VBoxContainer`
- `%CloseButton`: `Button`

### KitchenNutritionPanel

- **Scene Path:** `res://assets/ui/panels/KitchenNutritionPanel.tscn`
- **C# Implementation:** [`src/UI/KitchenNutritionPanel.cs`](../../src/UI/KitchenNutritionPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Meal preparation, nutrient fortification, and food spoilage prevention

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%MenuSelector`: `OptionButton`
- `%PreservationVats`: `VBoxContainer`
- `%CookButton`: `Button`
- `%CloseButton`: `Button`

### WaterTreatmentPanel

- **Scene Path:** `res://assets/ui/panels/WaterTreatmentPanel.tscn`
- **C# Implementation:** [`src/UI/WaterTreatmentPanel.cs`](../../src/UI/WaterTreatmentPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Radiological filtration, sump water recycling, and potable water tanks

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%ContaminationGauge`: `ProgressBar`
- `%FilterBankStatus`: `VBoxContainer`
- `%DistillationControls`: `HBoxContainer`
- `%PurifyButton`: `Button`
- `%FlushContaminantsBtn`: `Button`
- `%CloseButton`: `Button`

### PharmaLabPanel

- **Scene Path:** `res://assets/ui/panels/PharmaLabPanel.tscn`
- **C# Implementation:** [`src/UI/PharmaLabPanel.cs`](../../src/UI/PharmaLabPanel.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Advanced pharmaceuticals, chemical dependency inhibitors, and antiradiation serums

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%CentrifugeControls`: `HBoxContainer`
- `%ChemicalVats`: `VBoxContainer`
- `%SynthesisProgressBar`: `ProgressBar`
- `%SynthesizeRadCureBtn`: `Button`
- `%CloseButton`: `Button`

### OpeningProtocolModal

- **Scene Path:** `res://assets/ui/modals/OpeningProtocolModal.tscn`
- **C# Implementation:** [`src/UI/OpeningProtocolModal.cs`](../../src/UI/OpeningProtocolModal.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Shelter initialization sequence, starting survivor roster, and campaign seed briefing

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%Backdrop`: `ColorRect`
- `%TitleLabel`: `Label`
- `%ProtocolText`: `RichTextLabel`
- `%ConfirmButton`: `Button`

### SafeCrackModal

- **Scene Path:** `res://assets/ui/modals/SafeCrackModal.tscn`
- **C# Implementation:** [`src/UI/SafeCrackModal.cs`](../../src/UI/SafeCrackModal.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Audio/visual mini-game for unlocking pre-war safes and security lockers

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%DialRing`: `TextureRect`
- `%TumblerDisplay`: `HBoxContainer`
- `%DialLeftBtn`: `Button`
- `%DialRightBtn`: `Button`
- `%UnlockButton`: `Button`
- `%CloseButton`: `Button`

### DailyBriefingModal

- **Scene Path:** `res://assets/ui/modals/DailyBriefingModal.tscn`
- **C# Implementation:** [`src/UI/DailyBriefingModal.cs`](../../src/UI/DailyBriefingModal.cs)
- **Root Node Type:** `Control`
- **Primary Purpose:** Dawn transition modal: daily survivor vitals summary, weather shifts, and incident logs

**Required Unique Nodes (`SceneBinder.Require<T>`):**

- `%TitleLabel`: `Label`
- `%BodyLabel`: `RichTextLabel`
- `%ScrollContainer`: `ScrollContainer`
- `%AckButton`: `Button`
- `%SkipButton`: `Button`
- `%AckLabel`: `Label`

---

## Design System Standards & Color Palette

| Constant | Value / Hex | Usage |
|---|---|---|
| `DesignTheme.Pale` | `#D8D5CC` | Standard high-contrast body text on dark surfaces |
| `DesignTheme.Green` | `#4E9A06` | Safe / functional / operational indicator |
| `DesignTheme.Amber` | `#F57900` | Warning / caution / elevated danger |
| `DesignTheme.Red` | `#CC0000` | Critical failure / lethal radiation / fatal injury |
| `DesignTheme.FontSizeBody` | `14px` | Standard label and readout typography |
| `DesignTheme.FontSizeHeading` | `18px` | Panel header and section title typography |
| `DesignTheme.FontSizeMono` | `12px` | Diegetic terminal data and telemetry readouts |

---

## Verification & Linting Gates

- **Scene Lint:** `python3 scripts/ci/scene-lint.py` (verifies all 26 production scenes have valid types and no missing script resources).
- **Scene Binding Self-Test:** `godot --headless --path . -- --scene-binding-selftest` (validates all 22 typed unique-name node bindings).
- **UI Accessibility Self-Test:** `godot --headless --path . -- --ui-accessibility-selftest` (verifies focus modes, readable headers, and modal escape paths).
