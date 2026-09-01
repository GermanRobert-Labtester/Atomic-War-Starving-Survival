#!/usr/bin/env python3
"""
Rewrite src/Main.UiPanels.cs so each migrated *DetailPanel, *AfflictionsPanel,
*FactionDetailPanel etc. is instantiated via PanelSceneLoader.Load<...>(res:// path)
instead of `new <ClassName>()`. Non-migrated panels retain their C# constructor.
"""

import re
from pathlib import Path

REPO = Path(".").resolve()
TARGET = REPO / "src" / "Main.UiPanels.cs"

# (ClassName, scene path) — only panels whose .tscn now exists
MIGRATIONS = {
    "InventoryDetailPanel": "res://assets/ui/panels/InventoryDetailPanel.tscn",
    "AfflictionsPanel": "res://assets/ui/panels/AfflictionsPanel.tscn",
    "SurvivorDetailPanel": "res://assets/ui/panels/SurvivorDetailPanel.tscn",
    "WeatherDetailPanel": "res://assets/ui/panels/WeatherDetailPanel.tscn",
    "QuestDetailPanel": "res://assets/ui/panels/QuestDetailPanel.tscn",
    "MapDetailPanel": "res://assets/ui/panels/MapDetailPanel.tscn",
    "RadiationDetailPanel": "res://assets/ui/panels/RadiationDetailPanel.tscn",
    "EconomyDetailPanel": "res://assets/ui/panels/EconomyDetailPanel.tscn",
    "CombatDetailPanel": "res://assets/ui/panels/CombatDetailPanel.tscn",
    "FactionDetailPanel": "res://assets/ui/panels/FactionDetailPanel.tscn",
    "JournalDetailPanel": "res://assets/ui/panels/JournalDetailPanel.tscn",
    "EventDetailPanel": "res://assets/ui/panels/EventDetailPanel.tscn",
    "DutyRosterDetailPanel": "res://assets/ui/panels/DutyRosterDetailPanel.tscn",
    "SurvivalDetailPanel": "res://assets/ui/panels/SurvivalDetailPanel.tscn",
}

src = TARGET.read_text(encoding="utf-8")
count = 0
for class_name, scene_path in MIGRATIONS.items():
    # Look for `_var = new <ClassName>();` pattern.
    pattern = re.compile(rf"(\b_\w+)\s*=\s*new\s+{re.escape(class_name)}\s*\(\s*\)\s*;", re.DOTALL)
    def repl(m):
        global count
        count += 1
        var = m.group(1)
        return f'{var} = PanelSceneLoader.Load<{class_name}>("{scene_path}");'
    new = pattern.sub(repl, src)
    if new != src:
        print(f"Migrated: {class_name}")
    src = new

if count == 0:
    print("No changes")
else:
    TARGET.write_text(src, encoding="utf-8")
    print(f"Wrote {TARGET}")
