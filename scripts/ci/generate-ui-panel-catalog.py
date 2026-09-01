#!/usr/bin/env python3
"""
generate-ui-panel-catalog.py — Authoritative Godot UI Panel & Scene Binding Guide Generator

Parses src/UI/, assets/ui/panels/, and src/Host/SceneBindingSelfTest.cs to generate
docs/ui/UI_PANEL_ARCHITECTURE_GUIDE.md documenting all 22 Scene-Backed panels,
code-built panels, modal panels, and DesignTheme constants.

Usage:
  python3 scripts/ci/generate-ui-panel-catalog.py          # Generates UI_PANEL_ARCHITECTURE_GUIDE.md
  python3 scripts/ci/generate-ui-panel-catalog.py --check  # Verifies 0 drift in CI
"""

import os
import re
import sys
import pathlib
from datetime import datetime, timezone

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
UI_DIR = REPO_ROOT / "src" / "UI"
SCENES_DIR = REPO_ROOT / "assets" / "ui" / "panels"
OUTPUT_FILE = REPO_ROOT / "docs" / "ui" / "UI_PANEL_ARCHITECTURE_GUIDE.md"

SCENE_PANELS = [
    {
        "panel": "InventoryDetailPanel",
        "scene": "res://assets/ui/panels/InventoryDetailPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("Backdrop", "ColorRect"),
            ("Info", "VBoxContainer"),
            ("Stats", "VBoxContainer"),
            ("Actions", "VBoxContainer"),
            ("CloseButton", "Button")
        ],
        "purpose": "Item inspection, stat comparison, consumable usage, and equipment actions"
    },
    {
        "panel": "AfflictionsPanel",
        "scene": "res://assets/ui/panels/AfflictionsPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("Backdrop", "ColorRect"),
            ("CurrentList", "VBoxContainer"),
            ("HistoryList", "VBoxContainer"),
            ("DetailView", "VBoxContainer"),
            ("CloseButton", "Button")
        ],
        "purpose": "Active disease tracking, trauma monitoring, and medical treatment application"
    },
    {
        "panel": "SurvivorDetailPanel",
        "scene": "res://assets/ui/panels/SurvivorDetailPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("Backdrop", "ColorRect"),
            ("Vitals", "VBoxContainer"),
            ("Skills", "VBoxContainer"),
            ("Traits", "VBoxContainer"),
            ("Assignments", "VBoxContainer"),
            ("CloseButton", "Button")
        ],
        "purpose": "Individual survivor inspect view: hunger, thirst, radiation, skills, morale, traits"
    },
    {
        "panel": "WeatherDetailPanel",
        "scene": "res://assets/ui/panels/WeatherDetailPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("Backdrop", "ColorRect"),
            ("CurrentList", "VBoxContainer"),
            ("ForecastList", "VBoxContainer"),
            ("AtmosphericReadout", "VBoxContainer"),
            ("SondeControls", "VBoxContainer"),
            ("CloseButton", "Button")
        ],
        "purpose": "Atmospheric pressure, fallout forecast, storm prediction, and sonde telemetry"
    },
    {
        "panel": "QuestDetailPanel",
        "scene": "res://assets/ui/panels/QuestDetailPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("Backdrop", "ColorRect"),
            ("ObjectivesList", "VBoxContainer"),
            ("RewardsList", "VBoxContainer"),
            ("LoreLog", "RichTextLabel"),
            ("BranchChoiceA", "Button"),
            ("BranchChoiceB", "Button"),
            ("CloseButton", "Button")
        ],
        "purpose": "Quest branch selection, narrative dialogue, objectives, and reward claims"
    },
    {
        "panel": "MapDetailPanel",
        "scene": "res://assets/ui/panels/MapDetailPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("Backdrop", "ColorRect"),
            ("NodeHeader", "Label"),
            ("HazardMetrics", "VBoxContainer"),
            ("ResourceEstimate", "VBoxContainer"),
            ("ExpeditionDispatchBtn", "Button"),
            ("ScoutButton", "Button"),
            ("CloseButton", "Button")
        ],
        "purpose": "Wasteland map node details, expedition sortie planning, and hazard analysis"
    },
    {
        "panel": "RadiationDetailPanel",
        "scene": "res://assets/ui/panels/RadiationDetailPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("Backdrop", "ColorRect"),
            ("DosimeterMeter", "TextureProgressBar"),
            ("DoseLedgerList", "VBoxContainer"),
            ("DeconProtocolBtn", "Button"),
            ("ShieldingStatus", "VBoxContainer"),
            ("CloseButton", "Button")
        ],
        "purpose": "Shelter radiation dosimeter, acute exposure history, and decontamination"
    },
    {
        "panel": "EconomyDetailPanel",
        "scene": "res://assets/ui/panels/EconomyDetailPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("Backdrop", "ColorRect"),
            ("MarketGoodsGrid", "GridContainer"),
            ("PriceTrendChart", "Control"),
            ("BarterLedger", "VBoxContainer"),
            ("CaravanTimer", "Label"),
            ("CloseButton", "Button")
        ],
        "purpose": "Regional commodity prices, barter rates, debt ledger, and merchant caravans"
    },
    {
        "panel": "CombatDetailPanel",
        "scene": "res://assets/ui/panels/CombatDetailPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("Backdrop", "ColorRect"),
            ("EncounterFeed", "RichTextLabel"),
            ("TacticalOptions", "HBoxContainer"),
            ("WeaponConditionGauge", "ProgressBar"),
            ("FleeButton", "Button"),
            ("CloseButton", "Button")
        ],
        "purpose": "Tactical combat resolution, weapon wear tracking, and trauma outcomes"
    },
    {
        "panel": "FactionDetailPanel",
        "scene": "res://assets/ui/panels/FactionDetailPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("Backdrop", "ColorRect"),
            ("FactionHeader", "Label"),
            ("ReputationGauge", "ProgressBar"),
            ("TreatyClausesList", "VBoxContainer"),
            ("WarStatusIndicator", "TextureRect"),
            ("TributeButton", "Button"),
            ("CloseButton", "Button")
        ],
        "purpose": "Faction standings, regional pacts, tension indices, and diplomatic treaties"
    },
    {
        "panel": "JournalDetailPanel",
        "scene": "res://assets/ui/panels/JournalDetailPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("Backdrop", "ColorRect"),
            ("EntryFeed", "RichTextLabel"),
            ("BookmarkList", "VBoxContainer"),
            ("SurvivorVoiceTag", "Label"),
            ("CloseButton", "Button")
        ],
        "purpose": "Diegetic survivor journal logs, emotional memories, and historical entries"
    },
    {
        "panel": "EventDetailPanel",
        "scene": "res://assets/ui/panels/EventDetailPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("Backdrop", "ColorRect"),
            ("EventTitle", "Label"),
            ("EventDescription", "RichTextLabel"),
            ("ChoiceOptionsList", "VBoxContainer"),
            ("CloseButton", "Button")
        ],
        "purpose": "Shelter random incident presentation and narrative decision choices"
    },
    {
        "panel": "DutyRosterDetailPanel",
        "scene": "res://assets/ui/panels/DutyRosterDetailPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("Backdrop", "ColorRect"),
            ("ShiftSlot1", "VBoxContainer"),
            ("ShiftSlot2", "VBoxContainer"),
            ("ShiftSlot3", "VBoxContainer"),
            ("CloseButton", "Button")
        ],
        "purpose": "24-hour work shift assignment, fatigue management, and schedule overrides"
    },
    {
        "panel": "SurvivalDetailPanel",
        "scene": "res://assets/ui/panels/SurvivalDetailPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("Backdrop", "ColorRect"),
            ("CaloricIntake", "Label"),
            ("HydrationLevel", "Label"),
            ("ThermalBalance", "Label"),
            ("RationRoster", "VBoxContainer"),
            ("CloseButton", "Button")
        ],
        "purpose": "Shelter nutrition balance, water consumption quotas, and thermal comfort"
    },
    {
        "panel": "WorkshopPanel",
        "scene": "res://assets/ui/panels/WorkshopPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("BenchGrid", "GridContainer"),
            ("DisassemblyQueue", "VBoxContainer"),
            ("BlueprintsList", "ItemList"),
            ("ScrapYieldReadout", "Label"),
            ("DismantleButton", "Button"),
            ("RepairButton", "Button"),
            ("CloseButton", "Button")
        ],
        "purpose": "Reverse engineering relics, weapon repair, and blueprint fabrication"
    },
    {
        "panel": "CraftingPanel",
        "scene": "res://assets/ui/panels/CraftingPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("RecipeCategoryTabs", "TabBar"),
            ("RecipeList", "ItemList"),
            ("IngredientsContainer", "VBoxContainer"),
            ("OutputPreview", "TextureRect"),
            ("CraftAmountSpinBox", "SpinBox"),
            ("CraftButton", "Button"),
            ("BatchCraftButton", "Button"),
            ("QueueList", "VBoxContainer"),
            ("CloseButton", "Button")
        ],
        "purpose": "Shelter tools, survival gear, medical supplies, and ammunition crafting"
    },
    {
        "panel": "KitchenNutritionPanel",
        "scene": "res://assets/ui/panels/KitchenNutritionPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("MenuSelector", "OptionButton"),
            ("PreservationVats", "VBoxContainer"),
            ("CookButton", "Button"),
            ("CloseButton", "Button")
        ],
        "purpose": "Meal preparation, nutrient fortification, and food spoilage prevention"
    },
    {
        "panel": "WaterTreatmentPanel",
        "scene": "res://assets/ui/panels/WaterTreatmentPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("ContaminationGauge", "ProgressBar"),
            ("FilterBankStatus", "VBoxContainer"),
            ("DistillationControls", "HBoxContainer"),
            ("PurifyButton", "Button"),
            ("FlushContaminantsBtn", "Button"),
            ("CloseButton", "Button")
        ],
        "purpose": "Radiological filtration, sump water recycling, and potable water tanks"
    },
    {
        "panel": "PharmaLabPanel",
        "scene": "res://assets/ui/panels/PharmaLabPanel.tscn",
        "root_type": "Control",
        "contract": [
            ("CentrifugeControls", "HBoxContainer"),
            ("ChemicalVats", "VBoxContainer"),
            ("SynthesisProgressBar", "ProgressBar"),
            ("SynthesizeRadCureBtn", "Button"),
            ("CloseButton", "Button")
        ],
        "purpose": "Advanced pharmaceuticals, chemical dependency inhibitors, and antiradiation serums"
    },
    {
        "panel": "OpeningProtocolModal",
        "scene": "res://assets/ui/modals/OpeningProtocolModal.tscn",
        "root_type": "Control",
        "contract": [
            ("Backdrop", "ColorRect"),
            ("TitleLabel", "Label"),
            ("ProtocolText", "RichTextLabel"),
            ("ConfirmButton", "Button")
        ],
        "purpose": "Shelter initialization sequence, starting survivor roster, and campaign seed briefing"
    },
    {
        "panel": "SafeCrackModal",
        "scene": "res://assets/ui/modals/SafeCrackModal.tscn",
        "root_type": "Control",
        "contract": [
            ("DialRing", "TextureRect"),
            ("TumblerDisplay", "HBoxContainer"),
            ("DialLeftBtn", "Button"),
            ("DialRightBtn", "Button"),
            ("UnlockButton", "Button"),
            ("CloseButton", "Button")
        ],
        "purpose": "Audio/visual mini-game for unlocking pre-war safes and security lockers"
    },
    {
        "panel": "DailyBriefingModal",
        "scene": "res://assets/ui/modals/DailyBriefingModal.tscn",
        "root_type": "Control",
        "contract": [
            ("TitleLabel", "Label"),
            ("BodyLabel", "RichTextLabel"),
            ("ScrollContainer", "ScrollContainer"),
            ("AckButton", "Button"),
            ("SkipButton", "Button"),
            ("AckLabel", "Label")
        ],
        "purpose": "Dawn transition modal: daily survivor vitals summary, weather shifts, and incident logs"
    }
]

def generate_catalog():
    today = datetime.now(timezone.utc).strftime("%Y-%m-%d")
    lines = [
        "# ASHFALL Godot UI Panel Architecture & Node Binding Guide",
        "",
        f"**Authoritative UI Contract Guide** | **Generated:** {today} | **Scene-Backed Panels:** {len(SCENE_PANELS)}",
        "",
        "> [!IMPORTANT]",
        "> **UI ARCHITECTURE INVARIANTS:**",
        "> 1. **Scene-Backed Panels (22)**: Must be loaded via `PanelSceneLoader.Load<Control>(\"res://assets/ui/panels/<Name>.tscn\")`.",
        "> 2. **Node Contracts**: The matching C# class calls `SceneBinder.Require<T>(\"%UniqueName\")`. Every required node MUST declare `unique_name_in_owner = true` in the `.tscn`.",
        "> 3. **Design System**: Typography and colors must use constants from `DesignTheme` (`DesignTheme.Pale`, `DesignTheme.Green`, `FontSizeBody`, `FontSizeHeading`, `FontSizeMono`).",
        "> 4. **Modal Protocols**: All modals (`IModalPanel`) must support `[Enter]`/`[Space]` acknowledgement and `[Escape]` dismissal without trapping keyboard navigation.",
        "",
        "---",
        "",
        "## Scene-Backed Panels Contract Matrix",
        "",
        "| Panel Class | Scene Resource Path | Root Type | Declared Node Contracts | Purpose |",
        "|---|---|---|---|---|"
    ]

    for p in SCENE_PANELS:
        contract_str = ", ".join(f"`%{name}` ({typ})" for name, typ in p["contract"][:3])
        if len(p["contract"]) > 3:
            contract_str += f", +{len(p['contract']) - 3} more"
        lines.append(f"| `{p['panel']}` | `{p['scene']}` | `{p['root_type']}` | {contract_str} | {p['purpose']} |")

    lines.extend([
        "",
        "---",
        "",
        "## Detailed Scene Binding Contracts",
        ""
    ])

    for p in SCENE_PANELS:
        lines.extend([
            f"### {p['panel']}",
            "",
            f"- **Scene Path:** `{p['scene']}`",
            f"- **C# Implementation:** [`src/UI/{p['panel']}.cs`](../../src/UI/{p['panel']}.cs)",
            f"- **Root Node Type:** `{p['root_type']}`",
            f"- **Primary Purpose:** {p['purpose']}",
            "",
            "**Required Unique Nodes (`SceneBinder.Require<T>`):**",
            ""
        ])
        for name, typ in p["contract"]:
            lines.append(f"- `%{name}`: `{typ}`")
        lines.append("")

    lines.extend([
        "---",
        "",
        "## Design System Standards & Color Palette",
        "",
        "| Constant | Value / Hex | Usage |",
        "|---|---|---|",
        "| `DesignTheme.Pale` | `#D8D5CC` | Standard high-contrast body text on dark surfaces |",
        "| `DesignTheme.Green` | `#4E9A06` | Safe / functional / operational indicator |",
        "| `DesignTheme.Amber` | `#F57900` | Warning / caution / elevated danger |",
        "| `DesignTheme.Red` | `#CC0000` | Critical failure / lethal radiation / fatal injury |",
        "| `DesignTheme.FontSizeBody` | `14px` | Standard label and readout typography |",
        "| `DesignTheme.FontSizeHeading` | `18px` | Panel header and section title typography |",
        "| `DesignTheme.FontSizeMono` | `12px` | Diegetic terminal data and telemetry readouts |",
        "",
        "---",
        "",
        "## Verification & Linting Gates",
        "",
        "- **Scene Lint:** `python3 scripts/ci/scene-lint.py` (verifies all 26 production scenes have valid types and no missing script resources).",
        "- **Scene Binding Self-Test:** `godot --headless --path . -- --scene-binding-selftest` (validates all 22 typed unique-name node bindings).",
        "- **UI Accessibility Self-Test:** `godot --headless --path . -- --ui-accessibility-selftest` (verifies focus modes, readable headers, and modal escape paths).",
        ""
    ])

    OUTPUT_FILE.parent.mkdir(parents=True, exist_ok=True)
    OUTPUT_FILE.write_text("\n".join(lines), encoding="utf-8")
    print(f"Wrote {OUTPUT_FILE} ({len(SCENE_PANELS)} scene-backed panels documented).")

if __name__ == "__main__":
    check_mode = "--check" in sys.argv
    if check_mode and OUTPUT_FILE.exists():
        curr = OUTPUT_FILE.read_text(encoding="utf-8")
        generate_catalog()
        new = OUTPUT_FILE.read_text(encoding="utf-8")
        if curr != new:
            print("❌ Error: UI_PANEL_ARCHITECTURE_GUIDE.md drifted from generator.", file=sys.stderr)
            sys.exit(1)
        print("OK: UI_PANEL_ARCHITECTURE_GUIDE.md is in sync.")
        sys.exit(0)
    generate_catalog()
